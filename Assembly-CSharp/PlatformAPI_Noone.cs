using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using Panik;

public class PlatformAPI_Noone : PlatformAPI
{
	// Token: 0x06000350 RID: 848 RVA: 0x000153B4 File Offset: 0x000135B4
	protected override UniTask<bool> _InitializationBeing()
	{
		PlatformAPI_Noone.<_InitializationBeing>d__0 <_InitializationBeing>d__;
		<_InitializationBeing>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<_InitializationBeing>d__.<>1__state = -1;
		<_InitializationBeing>d__.<>t__builder.Start<PlatformAPI_Noone.<_InitializationBeing>d__0>(ref <_InitializationBeing>d__);
		return <_InitializationBeing>d__.<>t__builder.Task;
	}

	// Token: 0x06000351 RID: 849 RVA: 0x000153F0 File Offset: 0x000135F0
	protected override UniTask<bool> _InitializationFinalize()
	{
		PlatformAPI_Noone.<_InitializationFinalize>d__1 <_InitializationFinalize>d__;
		<_InitializationFinalize>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<_InitializationFinalize>d__.<>1__state = -1;
		<_InitializationFinalize>d__.<>t__builder.Start<PlatformAPI_Noone.<_InitializationFinalize>d__1>(ref <_InitializationFinalize>d__);
		return <_InitializationFinalize>d__.<>t__builder.Task;
	}

	// Token: 0x06000352 RID: 850 RVA: 0x0001542B File Offset: 0x0001362B
	public override bool IsUsable()
	{
		return PlatformAPI.IsInitialized();
	}

	// Token: 0x06000353 RID: 851 RVA: 0x00015432 File Offset: 0x00013632
	public override bool SupportsOnlineFunctionalities()
	{
		return false;
	}

	// Token: 0x06000354 RID: 852 RVA: 0x00015435 File Offset: 0x00013635
	public override bool IsOnline()
	{
		PlatformAPI.IsInitialized();
		return false;
	}

	// Token: 0x06000355 RID: 853 RVA: 0x0001543E File Offset: 0x0001363E
	public override bool SupportsOnlineAchievements()
	{
		return false;
	}

	// Token: 0x06000356 RID: 854 RVA: 0x00015444 File Offset: 0x00013644
	protected override UniTask<bool> AchievementIsUnlocked_Online_Demo(PlatformAPI.AchievementDemo achievement, float maxTimeout)
	{
		PlatformAPI_Noone.<AchievementIsUnlocked_Online_Demo>d__6 <AchievementIsUnlocked_Online_Demo>d__;
		<AchievementIsUnlocked_Online_Demo>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementIsUnlocked_Online_Demo>d__.<>4__this = this;
		<AchievementIsUnlocked_Online_Demo>d__.<>1__state = -1;
		<AchievementIsUnlocked_Online_Demo>d__.<>t__builder.Start<PlatformAPI_Noone.<AchievementIsUnlocked_Online_Demo>d__6>(ref <AchievementIsUnlocked_Online_Demo>d__);
		return <AchievementIsUnlocked_Online_Demo>d__.<>t__builder.Task;
	}

	// Token: 0x06000357 RID: 855 RVA: 0x00015488 File Offset: 0x00013688
	protected override UniTask<bool> AchievementIsUnlocked_Online_FullGame(PlatformAPI.AchievementFullGame achievement, float maxTimeout)
	{
		PlatformAPI_Noone.<AchievementIsUnlocked_Online_FullGame>d__7 <AchievementIsUnlocked_Online_FullGame>d__;
		<AchievementIsUnlocked_Online_FullGame>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementIsUnlocked_Online_FullGame>d__.<>4__this = this;
		<AchievementIsUnlocked_Online_FullGame>d__.<>1__state = -1;
		<AchievementIsUnlocked_Online_FullGame>d__.<>t__builder.Start<PlatformAPI_Noone.<AchievementIsUnlocked_Online_FullGame>d__7>(ref <AchievementIsUnlocked_Online_FullGame>d__);
		return <AchievementIsUnlocked_Online_FullGame>d__.<>t__builder.Task;
	}

	// Token: 0x06000358 RID: 856 RVA: 0x000154CC File Offset: 0x000136CC
	protected override UniTask<bool> AchievementUnlock_Online_Demo(PlatformAPI.AchievementDemo achievement, float maxTimeout)
	{
		PlatformAPI_Noone.<AchievementUnlock_Online_Demo>d__8 <AchievementUnlock_Online_Demo>d__;
		<AchievementUnlock_Online_Demo>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementUnlock_Online_Demo>d__.<>4__this = this;
		<AchievementUnlock_Online_Demo>d__.<>1__state = -1;
		<AchievementUnlock_Online_Demo>d__.<>t__builder.Start<PlatformAPI_Noone.<AchievementUnlock_Online_Demo>d__8>(ref <AchievementUnlock_Online_Demo>d__);
		return <AchievementUnlock_Online_Demo>d__.<>t__builder.Task;
	}

	// Token: 0x06000359 RID: 857 RVA: 0x00015510 File Offset: 0x00013710
	protected override UniTask<bool> AchievementUnlock_Online_FullGame(PlatformAPI.AchievementFullGame achievement, float maxTimeout)
	{
		PlatformAPI_Noone.<AchievementUnlock_Online_FullGame>d__9 <AchievementUnlock_Online_FullGame>d__;
		<AchievementUnlock_Online_FullGame>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementUnlock_Online_FullGame>d__.<>4__this = this;
		<AchievementUnlock_Online_FullGame>d__.<>1__state = -1;
		<AchievementUnlock_Online_FullGame>d__.<>t__builder.Start<PlatformAPI_Noone.<AchievementUnlock_Online_FullGame>d__9>(ref <AchievementUnlock_Online_FullGame>d__);
		return <AchievementUnlock_Online_FullGame>d__.<>t__builder.Task;
	}

	// Token: 0x0600035A RID: 858 RVA: 0x00015554 File Offset: 0x00013754
	protected override UniTask<bool> AchievementsClearAll_Online(float maxTimeout)
	{
		PlatformAPI_Noone.<AchievementsClearAll_Online>d__10 <AchievementsClearAll_Online>d__;
		<AchievementsClearAll_Online>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
		<AchievementsClearAll_Online>d__.<>4__this = this;
		<AchievementsClearAll_Online>d__.<>1__state = -1;
		<AchievementsClearAll_Online>d__.<>t__builder.Start<PlatformAPI_Noone.<AchievementsClearAll_Online>d__10>(ref <AchievementsClearAll_Online>d__);
		return <AchievementsClearAll_Online>d__.<>t__builder.Task;
	}

	// Token: 0x0600035B RID: 859 RVA: 0x00015597 File Offset: 0x00013797
	public override string GetUserID_String()
	{
		return "undefined";
	}

	// Token: 0x0600035C RID: 860 RVA: 0x0001559E File Offset: 0x0001379E
	public override void _Update()
	{
	}

	// Token: 0x0600035D RID: 861 RVA: 0x000155A0 File Offset: 0x000137A0
	public override void _OnClose()
	{
	}
}
