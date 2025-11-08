using System;
using Panik;
using TwitchSDK;
using TwitchSDK.Interop;
using UnityEngine;

public class TwitchMaster : MonoBehaviour
{
	// Token: 0x0600039C RID: 924 RVA: 0x000194E1 File Offset: 0x000176E1
	public static bool IsTwitchSupported()
	{
		return PlatformMaster.IsInitialized() && (PlatformAPI.ApiKindGet() != PlatformAPI.ApiKind.Steam || !PlatformAPI_Steam.IsSteamDeck());
	}

	// Token: 0x0600039D RID: 925 RVA: 0x00019500 File Offset: 0x00017700
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

	// Token: 0x0600039E RID: 926 RVA: 0x00019628 File Offset: 0x00017828
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

	// Token: 0x0600039F RID: 927 RVA: 0x00019690 File Offset: 0x00017890
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

	// Token: 0x060003A0 RID: 928 RVA: 0x00019710 File Offset: 0x00017910
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

	// Token: 0x060003A1 RID: 929 RVA: 0x00019747 File Offset: 0x00017947
	public static string GetTwitchMenuString()
	{
		return TwitchMaster.instance.twitchMenuString;
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x00019753 File Offset: 0x00017953
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

	// Token: 0x060003A3 RID: 931 RVA: 0x00019791 File Offset: 0x00017991
	private void Start()
	{
		TwitchMaster.instance != this;
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0001979F File Offset: 0x0001799F
	private void OnDestroy()
	{
		if (TwitchMaster.instance == this)
		{
			TwitchMaster.instance = null;
		}
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x000197B4 File Offset: 0x000179B4
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

	private GameTask<AuthenticationInfo> AuthInfoTask;

	private GameTask<AuthState> curAuthState;

	private float authStateUpdateTimer;

	private float authStateUpdateTimerMax = 0.5f;

	private bool urlOpened;

	private AuthStatus? statusOld;

	private string twitchMenuString = "";
}
