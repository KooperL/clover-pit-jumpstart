using System;
using Panik;
using TwitchSDK;
using TwitchSDK.Interop;
using UnityEngine;

// Token: 0x0200003C RID: 60
public class TwitchMaster : MonoBehaviour
{
	// Token: 0x060003FE RID: 1022 RVA: 0x0002CDC4 File Offset: 0x0002AFC4
	public static bool IsTwitchSupported()
	{
		return !(TwitchMaster.instance == null) && PlatformMaster.IsInitialized() && !TwitchMaster.instance._osString.Contains("linux") && (PlatformAPI.ApiKindGet() != PlatformAPI.ApiKind.Steam || !PlatformAPI_Steam.IsSteamDeck());
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x0002CE14 File Offset: 0x0002B014
	public void UpdateAuthState()
	{
		if (!TwitchMaster.IsTwitchSupported())
		{
			return;
		}
		try
		{
			this.curAuthState = Twitch.API.GetAuthState();
			if (this.curAuthState != null)
			{
				if (this.curAuthState.MaybeResult.Status == AuthStatus.LoggedIn)
				{
					string text = "";
					GameTask<UserInfo> myUserInfo = Twitch.API.GetMyUserInfo();
					UserInfo userInfo = null;
					if (myUserInfo != null)
					{
						userInfo = myUserInfo.MaybeResult;
					}
					if (userInfo != null)
					{
						text = userInfo.DisplayName;
					}
					this.twitchMenuString = Translation.Get("MENU_OPTION_SETTINGS_TWITCH_MODE_NAME") + text;
					if (MainMenuScript.IsEnabled())
					{
						MainMenuScript.instance.OptionsUpdate();
					}
				}
				if (this.curAuthState.MaybeResult.Status == AuthStatus.LoggedOut)
				{
					this.GetAuthInformation();
					TwitchMaster.LogoutTwitch(false);
				}
				if (this.curAuthState.MaybeResult.Status == AuthStatus.WaitingForCode && this.AuthInfoTask != null)
				{
					AuthenticationInfo maybeResult = this.AuthInfoTask.MaybeResult;
					if (maybeResult != null && !this.urlOpened)
					{
						Application.OpenURL(maybeResult.Uri);
						this.urlOpened = true;
						if (MainMenuScript.IsEnabled())
						{
							MainMenuScript.instance.OptionsUpdate();
						}
					}
				}
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0002CF3C File Offset: 0x0002B13C
	public void GetAuthInformation()
	{
		this.urlOpened = false;
		this.authStateUpdateTimer = 0.1f;
		if (this.AuthInfoTask == null)
		{
			TwitchOAuthScope twitchOAuthScope = new TwitchOAuthScope(TwitchOAuthScope.Bits.Read.Scope + " " + TwitchOAuthScope.Channel.ManagePolls.Scope);
			this.AuthInfoTask = Twitch.API.GetAuthenticationInfo(new TwitchOAuthScope[] { twitchOAuthScope });
		}
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0002CFA4 File Offset: 0x0002B1A4
	public static bool IsLoggedInAndEnabled()
	{
		if (!TwitchMaster.IsTwitchSupported())
		{
			return false;
		}
		if (TwitchMaster.instance == null)
		{
			return false;
		}
		try
		{
			TwitchMaster.instance.curAuthState = Twitch.API.GetAuthState();
			if (TwitchMaster.instance.curAuthState == null)
			{
				return false;
			}
			if (TwitchMaster.instance.curAuthState.MaybeResult.Status == AuthStatus.LoggedIn)
			{
				return true;
			}
		}
		catch (Exception)
		{
			return false;
		}
		return false;
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x0002D024 File Offset: 0x0002B224
	public static async void LogoutTwitch(bool menuCall)
	{
		await Twitch.API.LogOut();
		TwitchMaster.instance.AuthInfoTask = null;
		if (MainMenuScript.IsEnabled() && !menuCall)
		{
			MainMenuScript.instance.OptionsUpdate();
		}
		TwitchMaster.instance.twitchMenuString = "";
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x00008D22 File Offset: 0x00006F22
	public static string GetTwitchMenuString()
	{
		return TwitchMaster.instance.twitchMenuString;
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x00008D2E File Offset: 0x00006F2E
	private void Awake()
	{
		if (!PlatformMaster.PlatformIsComputer())
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			Debug.Log("Cannot use Twitch on this platform. Removing Twitch Master!");
			return;
		}
		if (TwitchMaster.instance != null)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		TwitchMaster.instance = this;
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x00008D6C File Offset: 0x00006F6C
	private void Start()
	{
		if (TwitchMaster.instance != this)
		{
			return;
		}
		this._osString = PlatformAPI.GetOsString().ToLower();
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x00008D8C File Offset: 0x00006F8C
	private void OnDestroy()
	{
		if (TwitchMaster.instance == this)
		{
			TwitchMaster.instance = null;
		}
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x00008DA1 File Offset: 0x00006FA1
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		this.authStateUpdateTimer -= Tick.Time;
		if (this.authStateUpdateTimer <= 0f)
		{
			this.authStateUpdateTimer = this.authStateUpdateTimerMax;
			this.UpdateAuthState();
		}
	}

	// Token: 0x0400037F RID: 895
	public static TwitchMaster instance;

	// Token: 0x04000380 RID: 896
	public const bool SHOW_TWITCH_CODE = false;

	// Token: 0x04000381 RID: 897
	private string _osString;

	// Token: 0x04000382 RID: 898
	private GameTask<AuthenticationInfo> AuthInfoTask;

	// Token: 0x04000383 RID: 899
	private GameTask<AuthState> curAuthState;

	// Token: 0x04000384 RID: 900
	private float authStateUpdateTimer;

	// Token: 0x04000385 RID: 901
	private float authStateUpdateTimerMax = 0.5f;

	// Token: 0x04000386 RID: 902
	private bool urlOpened;

	// Token: 0x04000387 RID: 903
	private AuthStatus? statusOld;

	// Token: 0x04000388 RID: 904
	private string twitchMenuString = "";
}
