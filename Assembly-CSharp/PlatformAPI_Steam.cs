using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Panik;
using Steamworks;
using Steamworks.Data;

public class PlatformAPI_Steam : PlatformAPI
{
	// Token: 0x0600035F RID: 863 RVA: 0x000155AC File Offset: 0x000137AC
	protected override UniTask<bool> _InitializationBeing()
	{
		PlatformAPI_Steam.<_InitializationBeing>d__0 <_InitializationBeing>d__;
		<_InitializationBeing>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<_InitializationBeing>d__.<>4__this = this;
		<_InitializationBeing>d__.<>1__state = -1;
		<_InitializationBeing>d__.<>t__builder.Start<PlatformAPI_Steam.<_InitializationBeing>d__0>(ref <_InitializationBeing>d__);
		return <_InitializationBeing>d__.<>t__builder.Task;
	}

	// Token: 0x06000360 RID: 864 RVA: 0x000155F0 File Offset: 0x000137F0
	protected override UniTask<bool> _InitializationFinalize()
	{
		PlatformAPI_Steam.<_InitializationFinalize>d__1 <_InitializationFinalize>d__;
		<_InitializationFinalize>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<_InitializationFinalize>d__.<>1__state = -1;
		<_InitializationFinalize>d__.<>t__builder.Start<PlatformAPI_Steam.<_InitializationFinalize>d__1>(ref <_InitializationFinalize>d__);
		return <_InitializationFinalize>d__.<>t__builder.Task;
	}

	// Token: 0x06000361 RID: 865 RVA: 0x0001562B File Offset: 0x0001382B
	public override bool IsUsable()
	{
		return PlatformAPI.IsInitialized() && SteamClient.IsValid;
	}

	// Token: 0x06000362 RID: 866 RVA: 0x0001563B File Offset: 0x0001383B
	public override bool SupportsOnlineFunctionalities()
	{
		return this.IsOnline();
	}

	// Token: 0x06000363 RID: 867 RVA: 0x00015643 File Offset: 0x00013843
	public override bool IsOnline()
	{
		return PlatformAPI.IsInitialized() && this.IsUsable() && SteamClient.IsLoggedOn;
	}

	// Token: 0x06000364 RID: 868 RVA: 0x0001565C File Offset: 0x0001385C
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

	// Token: 0x06000365 RID: 869 RVA: 0x000156C8 File Offset: 0x000138C8
	private bool SteamDictionaryCheck_Demo(PlatformAPI.AchievementDemo achievement)
	{
		return this.steamAchievementsDict.ContainsKey(PlatformAPI.AchievemntGetEnumString_Demo(achievement));
	}

	// Token: 0x06000366 RID: 870 RVA: 0x000156E0 File Offset: 0x000138E0
	private bool SteamDictionaryCheck_FullGame(PlatformAPI.AchievementFullGame achievement)
	{
		return this.steamAchievementsDict.ContainsKey(PlatformAPI.AchievemntGetEnumString_FullGame(achievement));
	}

	// Token: 0x06000367 RID: 871 RVA: 0x000156F8 File Offset: 0x000138F8
	public override bool SupportsOnlineAchievements()
	{
		return this.SupportsOnlineFunctionalities();
	}

	// Token: 0x06000368 RID: 872 RVA: 0x00015700 File Offset: 0x00013900
	protected override UniTask<bool> AchievementIsUnlocked_Online_Demo(PlatformAPI.AchievementDemo achievement, float maxTimeout)
	{
		PlatformAPI_Steam.<AchievementIsUnlocked_Online_Demo>d__11 <AchievementIsUnlocked_Online_Demo>d__;
		<AchievementIsUnlocked_Online_Demo>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementIsUnlocked_Online_Demo>d__.<>4__this = this;
		<AchievementIsUnlocked_Online_Demo>d__.achievement = achievement;
		<AchievementIsUnlocked_Online_Demo>d__.<>1__state = -1;
		<AchievementIsUnlocked_Online_Demo>d__.<>t__builder.Start<PlatformAPI_Steam.<AchievementIsUnlocked_Online_Demo>d__11>(ref <AchievementIsUnlocked_Online_Demo>d__);
		return <AchievementIsUnlocked_Online_Demo>d__.<>t__builder.Task;
	}

	// Token: 0x06000369 RID: 873 RVA: 0x0001574C File Offset: 0x0001394C
	protected override UniTask<bool> AchievementIsUnlocked_Online_FullGame(PlatformAPI.AchievementFullGame achievement, float maxTimeout)
	{
		PlatformAPI_Steam.<AchievementIsUnlocked_Online_FullGame>d__12 <AchievementIsUnlocked_Online_FullGame>d__;
		<AchievementIsUnlocked_Online_FullGame>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementIsUnlocked_Online_FullGame>d__.<>4__this = this;
		<AchievementIsUnlocked_Online_FullGame>d__.achievement = achievement;
		<AchievementIsUnlocked_Online_FullGame>d__.<>1__state = -1;
		<AchievementIsUnlocked_Online_FullGame>d__.<>t__builder.Start<PlatformAPI_Steam.<AchievementIsUnlocked_Online_FullGame>d__12>(ref <AchievementIsUnlocked_Online_FullGame>d__);
		return <AchievementIsUnlocked_Online_FullGame>d__.<>t__builder.Task;
	}

	// Token: 0x0600036A RID: 874 RVA: 0x00015798 File Offset: 0x00013998
	protected override UniTask<bool> AchievementUnlock_Online_Demo(PlatformAPI.AchievementDemo achievement, float maxTimeout)
	{
		PlatformAPI_Steam.<AchievementUnlock_Online_Demo>d__13 <AchievementUnlock_Online_Demo>d__;
		<AchievementUnlock_Online_Demo>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementUnlock_Online_Demo>d__.<>4__this = this;
		<AchievementUnlock_Online_Demo>d__.achievement = achievement;
		<AchievementUnlock_Online_Demo>d__.<>1__state = -1;
		<AchievementUnlock_Online_Demo>d__.<>t__builder.Start<PlatformAPI_Steam.<AchievementUnlock_Online_Demo>d__13>(ref <AchievementUnlock_Online_Demo>d__);
		return <AchievementUnlock_Online_Demo>d__.<>t__builder.Task;
	}

	// Token: 0x0600036B RID: 875 RVA: 0x000157E4 File Offset: 0x000139E4
	protected override UniTask<bool> AchievementUnlock_Online_FullGame(PlatformAPI.AchievementFullGame achievement, float maxTimeout)
	{
		PlatformAPI_Steam.<AchievementUnlock_Online_FullGame>d__14 <AchievementUnlock_Online_FullGame>d__;
		<AchievementUnlock_Online_FullGame>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementUnlock_Online_FullGame>d__.<>4__this = this;
		<AchievementUnlock_Online_FullGame>d__.achievement = achievement;
		<AchievementUnlock_Online_FullGame>d__.<>1__state = -1;
		<AchievementUnlock_Online_FullGame>d__.<>t__builder.Start<PlatformAPI_Steam.<AchievementUnlock_Online_FullGame>d__14>(ref <AchievementUnlock_Online_FullGame>d__);
		return <AchievementUnlock_Online_FullGame>d__.<>t__builder.Task;
	}

	// Token: 0x0600036C RID: 876 RVA: 0x00015830 File Offset: 0x00013A30
	protected override UniTask<bool> AchievementsClearAll_Online(float maxTimeout)
	{
		PlatformAPI_Steam.<AchievementsClearAll_Online>d__15 <AchievementsClearAll_Online>d__;
		<AchievementsClearAll_Online>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementsClearAll_Online>d__.<>4__this = this;
		<AchievementsClearAll_Online>d__.<>1__state = -1;
		<AchievementsClearAll_Online>d__.<>t__builder.Start<PlatformAPI_Steam.<AchievementsClearAll_Online>d__15>(ref <AchievementsClearAll_Online>d__);
		return <AchievementsClearAll_Online>d__.<>t__builder.Task;
	}

	// Token: 0x0600036D RID: 877 RVA: 0x00015873 File Offset: 0x00013A73
	protected override void Achievements_Prewarm_Demo()
	{
		this.EnsureSteamAchievements();
	}

	// Token: 0x0600036E RID: 878 RVA: 0x0001587B File Offset: 0x00013A7B
	protected override void Achievements_Prewarm_FullGame()
	{
		this.EnsureSteamAchievements();
	}

	// Token: 0x0600036F RID: 879 RVA: 0x00015884 File Offset: 0x00013A84
	public override string GetUserID_String()
	{
		return SteamClient.SteamId.ToString();
	}

	// Token: 0x06000370 RID: 880 RVA: 0x000158A4 File Offset: 0x00013AA4
	public uint GetAppId_UInt()
	{
		return 3314790U;
	}

	// Token: 0x06000371 RID: 881 RVA: 0x000158AB File Offset: 0x00013AAB
	public static bool IsSteamDeck()
	{
		return SteamUtils.IsRunningOnSteamDeck;
	}

	// Token: 0x06000372 RID: 882 RVA: 0x000158B2 File Offset: 0x00013AB2
	public override void _Update()
	{
	}

	// Token: 0x06000373 RID: 883 RVA: 0x000158B4 File Offset: 0x00013AB4
	public override void _OnClose()
	{
		SteamClient.Shutdown();
	}

	private Dictionary<string, Achievement> steamAchievementsDict = new Dictionary<string, Achievement>();

	private bool _firstTimeEnsuringSteamAchievements = true;
}
