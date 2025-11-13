using System;
using Panik;
using TwitchSDK;
using TwitchSDK.Interop;
using UnityEngine;

public class TwitchMaster : MonoBehaviour
{
	// Token: 0x0600039A RID: 922 RVA: 0x00019560 File Offset: 0x00017760
	public static bool IsTwitchSupported()
	{
		return !(TwitchMaster.instance == null) && PlatformMaster.IsInitialized() && !TwitchMaster.instance._osString.Contains("linux") && (PlatformAPI.ApiKindGet() != PlatformAPI.ApiKind.Steam || !PlatformAPI_Steam.IsSteamDeck());
	}

	// Token: 0x0600039B RID: 923 RVA: 0x000195B0 File Offset: 0x000177B0
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
				if (this.curAuthState.MaybeResult.Status == 3)
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
				if (this.curAuthState.MaybeResult.Status == null)
				{
					this.GetAuthInformation();
					TwitchMaster.LogoutTwitch(false);
				}
				if (this.curAuthState.MaybeResult.Status == 2 && this.AuthInfoTask != null)
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

	// Token: 0x0600039C RID: 924 RVA: 0x000196D8 File Offset: 0x000178D8
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

	// Token: 0x0600039D RID: 925 RVA: 0x00019740 File Offset: 0x00017940
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
			if (TwitchMaster.instance.curAuthState.MaybeResult.Status == 3)
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

	// Token: 0x0600039E RID: 926 RVA: 0x000197C0 File Offset: 0x000179C0
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

	// Token: 0x0600039F RID: 927 RVA: 0x000197F7 File Offset: 0x000179F7
	public static string GetTwitchMenuString()
	{
		return TwitchMaster.instance.twitchMenuString;
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x00019803 File Offset: 0x00017A03
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

	// Token: 0x060003A1 RID: 929 RVA: 0x00019841 File Offset: 0x00017A41
	private void Start()
	{
		if (TwitchMaster.instance != this)
		{
			return;
		}
		this._osString = PlatformAPI.GetOsString().ToLower();
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x00019861 File Offset: 0x00017A61
	private void OnDestroy()
	{
		if (TwitchMaster.instance == this)
		{
			TwitchMaster.instance = null;
		}
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x00019876 File Offset: 0x00017A76
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

	public static TwitchMaster instance;

	public const bool SHOW_TWITCH_CODE = false;

	private string _osString;

	private GameTask<AuthenticationInfo> AuthInfoTask;

	private GameTask<AuthState> curAuthState;

	private float authStateUpdateTimer;

	private float authStateUpdateTimerMax = 0.5f;

	private bool urlOpened;

	private AuthStatus? statusOld;

	private string twitchMenuString = "";
}
