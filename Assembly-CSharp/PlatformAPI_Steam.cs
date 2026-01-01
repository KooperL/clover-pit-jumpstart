using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Panik;
using Steamworks;
using Steamworks.Data;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class PlatformAPI_Steam : PlatformAPI
{
	// Token: 0x060003B3 RID: 947 RVA: 0x00028BC0 File Offset: 0x00026DC0
	protected override async UniTask<bool> _InitializationBeing()
	{
		bool flag;
		if (SteamClient.RestartAppIfNecessary(this.GetAppId_UInt()))
		{
			Application.Quit();
			Debug.LogError("Will try to open the application throguht steam! Restarting!");
			flag = false;
		}
		else
		{
			try
			{
				SteamClient.Init(this.GetAppId_UInt(), true);
			}
			catch (Exception)
			{
				Debug.LogError("Steamworks failed to initialize client!");
				return 0;
			}
			flag = true;
		}
		return flag;
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x00028C04 File Offset: 0x00026E04
	protected override async UniTask<bool> _InitializationFinalize()
	{
		return 1;
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x00008B62 File Offset: 0x00006D62
	public override bool IsUsable()
	{
		return PlatformAPI.IsInitialized() && SteamClient.IsValid;
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00008B72 File Offset: 0x00006D72
	public override bool SupportsOnlineFunctionalities()
	{
		return this.IsOnline();
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x00008B7A File Offset: 0x00006D7A
	public override bool IsOnline()
	{
		return PlatformAPI.IsInitialized() && this.IsUsable() && SteamClient.IsLoggedOn;
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x00028C40 File Offset: 0x00026E40
	private void EnsureSteamAchievements()
	{
		this.steamAchievementsDict.Clear();
		foreach (Achievement achievement in SteamUserStats.Achievements)
		{
			string identifier = achievement.Identifier;
			this.steamAchievementsDict.Add(identifier, achievement);
		}
		this._firstTimeEnsuringSteamAchievements = false;
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x00008B92 File Offset: 0x00006D92
	private bool SteamDictionaryCheck_Demo(PlatformAPI.AchievementDemo achievement)
	{
		return this.steamAchievementsDict.ContainsKey(PlatformAPI.AchievemntGetEnumString_Demo(achievement));
	}

	// Token: 0x060003BA RID: 954 RVA: 0x00008BAA File Offset: 0x00006DAA
	private bool SteamDictionaryCheck_FullGame(PlatformAPI.AchievementFullGame achievement)
	{
		return this.steamAchievementsDict.ContainsKey(PlatformAPI.AchievemntGetEnumString_FullGame(achievement));
	}

	// Token: 0x060003BB RID: 955 RVA: 0x00008BC2 File Offset: 0x00006DC2
	public override bool SupportsOnlineAchievements()
	{
		return this.SupportsOnlineFunctionalities();
	}

	// Token: 0x060003BC RID: 956 RVA: 0x00028CAC File Offset: 0x00026EAC
	protected override async UniTask<bool> AchievementIsUnlocked_Online_Demo(PlatformAPI.AchievementDemo achievement, float maxTimeout)
	{
		bool flag;
		if (!this.SteamDictionaryCheck_Demo(achievement))
		{
			flag = false;
		}
		else
		{
			flag = this.steamAchievementsDict[PlatformAPI.AchievemntGetEnumString_Demo(achievement)].State;
		}
		return flag;
	}

	// Token: 0x060003BD RID: 957 RVA: 0x00028CF8 File Offset: 0x00026EF8
	protected override async UniTask<bool> AchievementIsUnlocked_Online_FullGame(PlatformAPI.AchievementFullGame achievement, float maxTimeout)
	{
		bool flag;
		if (!this.SteamDictionaryCheck_FullGame(achievement))
		{
			flag = false;
		}
		else
		{
			flag = this.steamAchievementsDict[PlatformAPI.AchievemntGetEnumString_FullGame(achievement)].State;
		}
		return flag;
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00028D44 File Offset: 0x00026F44
	protected override async UniTask<bool> AchievementUnlock_Online_Demo(PlatformAPI.AchievementDemo achievement, float maxTimeout)
	{
		string text = PlatformAPI.AchievemntGetEnumString_Demo(achievement);
		bool flag;
		if (!this.SteamDictionaryCheck_Demo(achievement))
		{
			flag = false;
		}
		else
		{
			Achievement achievement2 = this.steamAchievementsDict[text];
			if (achievement2.State)
			{
				flag = false;
			}
			else
			{
				try
				{
					achievement2.Trigger(true);
				}
				catch (Exception)
				{
					return 0;
				}
				flag = true;
			}
		}
		return flag;
	}

	// Token: 0x060003BF RID: 959 RVA: 0x00028D90 File Offset: 0x00026F90
	protected override async UniTask<bool> AchievementUnlock_Online_FullGame(PlatformAPI.AchievementFullGame achievement, float maxTimeout)
	{
		string text = PlatformAPI.AchievemntGetEnumString_FullGame(achievement);
		bool flag;
		if (!this.SteamDictionaryCheck_FullGame(achievement))
		{
			flag = false;
		}
		else
		{
			Achievement achievement2 = this.steamAchievementsDict[text];
			if (achievement2.State)
			{
				flag = false;
			}
			else
			{
				try
				{
					achievement2.Trigger(true);
				}
				catch (Exception)
				{
					return 0;
				}
				flag = true;
			}
		}
		return flag;
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x00028DDC File Offset: 0x00026FDC
	protected override async UniTask<bool> AchievementsClearAll_Online(float maxTimeout)
	{
		this.EnsureSteamAchievements();
		try
		{
			foreach (KeyValuePair<string, Achievement> keyValuePair in this.steamAchievementsDict)
			{
				keyValuePair.Value.Clear();
			}
		}
		catch (Exception)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x00008BCA File Offset: 0x00006DCA
	protected override void Achievements_Prewarm_Demo()
	{
		this.EnsureSteamAchievements();
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x00008BCA File Offset: 0x00006DCA
	protected override void Achievements_Prewarm_FullGame()
	{
		this.EnsureSteamAchievements();
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x00028E20 File Offset: 0x00027020
	public override string GetUserID_String()
	{
		return SteamClient.SteamId.ToString();
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x00008BD2 File Offset: 0x00006DD2
	public uint GetAppId_UInt()
	{
		return 3314790U;
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x00008BD9 File Offset: 0x00006DD9
	public static bool IsSteamDeck()
	{
		return SteamUtils.IsRunningOnSteamDeck;
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x0000774E File Offset: 0x0000594E
	public override void _Update()
	{
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x00008BE0 File Offset: 0x00006DE0
	public override void _OnClose()
	{
		SteamClient.Shutdown();
	}

	// Token: 0x040002FA RID: 762
	private Dictionary<string, Achievement> steamAchievementsDict = new Dictionary<string, Achievement>();

	// Token: 0x040002FB RID: 763
	private bool _firstTimeEnsuringSteamAchievements = true;
}
