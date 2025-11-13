using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Panik;

public class PlatformAPI_Noone : PlatformAPI
{
	// Token: 0x0600034E RID: 846 RVA: 0x000153F8 File Offset: 0x000135F8
	protected override UniTask<bool> _InitializationBeing()
	{
		PlatformAPI_Noone.<_InitializationBeing>d__0 <_InitializationBeing>d__;
		<_InitializationBeing>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<_InitializationBeing>d__.<>1__state = -1;
		<_InitializationBeing>d__.<>t__builder.Start<PlatformAPI_Noone.<_InitializationBeing>d__0>(ref <_InitializationBeing>d__);
		return <_InitializationBeing>d__.<>t__builder.Task;
	}

	// Token: 0x0600034F RID: 847 RVA: 0x00015434 File Offset: 0x00013634
	protected override UniTask<bool> _InitializationFinalize()
	{
		PlatformAPI_Noone.<_InitializationFinalize>d__1 <_InitializationFinalize>d__;
		<_InitializationFinalize>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<_InitializationFinalize>d__.<>1__state = -1;
		<_InitializationFinalize>d__.<>t__builder.Start<PlatformAPI_Noone.<_InitializationFinalize>d__1>(ref <_InitializationFinalize>d__);
		return <_InitializationFinalize>d__.<>t__builder.Task;
	}

	// Token: 0x06000350 RID: 848 RVA: 0x0001546F File Offset: 0x0001366F
	public override bool IsUsable()
	{
		return PlatformAPI.IsInitialized();
	}

	// Token: 0x06000351 RID: 849 RVA: 0x00015476 File Offset: 0x00013676
	public override bool SupportsOnlineFunctionalities()
	{
		return false;
	}

	// Token: 0x06000352 RID: 850 RVA: 0x00015479 File Offset: 0x00013679
	public override bool IsOnline()
	{
		PlatformAPI.IsInitialized();
		return false;
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00015482 File Offset: 0x00013682
	public override bool SupportsOnlineAchievements()
	{
		return false;
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00015488 File Offset: 0x00013688
	protected override UniTask<bool> AchievementIsUnlocked_Online_Demo(PlatformAPI.AchievementDemo achievement, float maxTimeout)
	{
		PlatformAPI_Noone.<AchievementIsUnlocked_Online_Demo>d__6 <AchievementIsUnlocked_Online_Demo>d__;
		<AchievementIsUnlocked_Online_Demo>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementIsUnlocked_Online_Demo>d__.<>4__this = this;
		<AchievementIsUnlocked_Online_Demo>d__.<>1__state = -1;
		<AchievementIsUnlocked_Online_Demo>d__.<>t__builder.Start<PlatformAPI_Noone.<AchievementIsUnlocked_Online_Demo>d__6>(ref <AchievementIsUnlocked_Online_Demo>d__);
		return <AchievementIsUnlocked_Online_Demo>d__.<>t__builder.Task;
	}

	// Token: 0x06000355 RID: 853 RVA: 0x000154CC File Offset: 0x000136CC
	protected override UniTask<bool> AchievementIsUnlocked_Online_FullGame(PlatformAPI.AchievementFullGame achievement, float maxTimeout)
	{
		PlatformAPI_Noone.<AchievementIsUnlocked_Online_FullGame>d__7 <AchievementIsUnlocked_Online_FullGame>d__;
		<AchievementIsUnlocked_Online_FullGame>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementIsUnlocked_Online_FullGame>d__.<>4__this = this;
		<AchievementIsUnlocked_Online_FullGame>d__.<>1__state = -1;
		<AchievementIsUnlocked_Online_FullGame>d__.<>t__builder.Start<PlatformAPI_Noone.<AchievementIsUnlocked_Online_FullGame>d__7>(ref <AchievementIsUnlocked_Online_FullGame>d__);
		return <AchievementIsUnlocked_Online_FullGame>d__.<>t__builder.Task;
	}

	// Token: 0x06000356 RID: 854 RVA: 0x00015510 File Offset: 0x00013710
	protected override UniTask<bool> AchievementUnlock_Online_Demo(PlatformAPI.AchievementDemo achievement, float maxTimeout)
	{
		PlatformAPI_Noone.<AchievementUnlock_Online_Demo>d__8 <AchievementUnlock_Online_Demo>d__;
		<AchievementUnlock_Online_Demo>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementUnlock_Online_Demo>d__.<>4__this = this;
		<AchievementUnlock_Online_Demo>d__.<>1__state = -1;
		<AchievementUnlock_Online_Demo>d__.<>t__builder.Start<PlatformAPI_Noone.<AchievementUnlock_Online_Demo>d__8>(ref <AchievementUnlock_Online_Demo>d__);
		return <AchievementUnlock_Online_Demo>d__.<>t__builder.Task;
	}

	// Token: 0x06000357 RID: 855 RVA: 0x00015554 File Offset: 0x00013754
	protected override UniTask<bool> AchievementUnlock_Online_FullGame(PlatformAPI.AchievementFullGame achievement, float maxTimeout)
	{
		PlatformAPI_Noone.<AchievementUnlock_Online_FullGame>d__9 <AchievementUnlock_Online_FullGame>d__;
		<AchievementUnlock_Online_FullGame>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementUnlock_Online_FullGame>d__.<>4__this = this;
		<AchievementUnlock_Online_FullGame>d__.<>1__state = -1;
		<AchievementUnlock_Online_FullGame>d__.<>t__builder.Start<PlatformAPI_Noone.<AchievementUnlock_Online_FullGame>d__9>(ref <AchievementUnlock_Online_FullGame>d__);
		return <AchievementUnlock_Online_FullGame>d__.<>t__builder.Task;
	}

	// Token: 0x06000358 RID: 856 RVA: 0x00015598 File Offset: 0x00013798
	protected override UniTask<bool> AchievementsClearAll_Online(float maxTimeout)
	{
		PlatformAPI_Noone.<AchievementsClearAll_Online>d__10 <AchievementsClearAll_Online>d__;
		<AchievementsClearAll_Online>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementsClearAll_Online>d__.<>4__this = this;
		<AchievementsClearAll_Online>d__.<>1__state = -1;
		<AchievementsClearAll_Online>d__.<>t__builder.Start<PlatformAPI_Noone.<AchievementsClearAll_Online>d__10>(ref <AchievementsClearAll_Online>d__);
		return <AchievementsClearAll_Online>d__.<>t__builder.Task;
	}

	// Token: 0x06000359 RID: 857 RVA: 0x000155DB File Offset: 0x000137DB
	public override string GetUserID_String()
	{
		return "undefined";
	}

	// Token: 0x0600035A RID: 858 RVA: 0x000155E2 File Offset: 0x000137E2
	public override void _Update()
	{
	}

	// Token: 0x0600035B RID: 859 RVA: 0x000155E4 File Offset: 0x000137E4
	public override void _OnClose()
	{
	}
}
