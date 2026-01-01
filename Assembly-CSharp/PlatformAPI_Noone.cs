using System;
using Cysharp.Threading.Tasks;
using Panik;

// Token: 0x02000028 RID: 40
public class PlatformAPI_Noone : PlatformAPI
{
	// Token: 0x06000396 RID: 918 RVA: 0x000287A4 File Offset: 0x000269A4
	protected override async UniTask<bool> _InitializationBeing()
	{
		return 1;
	}

	// Token: 0x06000397 RID: 919 RVA: 0x000287E0 File Offset: 0x000269E0
	protected override async UniTask<bool> _InitializationFinalize()
	{
		return 1;
	}

	// Token: 0x06000398 RID: 920 RVA: 0x00008ADE File Offset: 0x00006CDE
	public override bool IsUsable()
	{
		return PlatformAPI.IsInitialized();
	}

	// Token: 0x06000399 RID: 921 RVA: 0x00008AE5 File Offset: 0x00006CE5
	public override bool SupportsOnlineFunctionalities()
	{
		return false;
	}

	// Token: 0x0600039A RID: 922 RVA: 0x00008AE8 File Offset: 0x00006CE8
	public override bool IsOnline()
	{
		PlatformAPI.IsInitialized();
		return false;
	}

	// Token: 0x0600039B RID: 923 RVA: 0x00008AE5 File Offset: 0x00006CE5
	public override bool SupportsOnlineAchievements()
	{
		return false;
	}

	// Token: 0x0600039C RID: 924 RVA: 0x0002881C File Offset: 0x00026A1C
	protected override async UniTask<bool> AchievementIsUnlocked_Online_Demo(PlatformAPI.AchievementDemo achievement, float maxTimeout)
	{
		return this.SupportsOnlineAchievements();
	}

	// Token: 0x0600039D RID: 925 RVA: 0x00028860 File Offset: 0x00026A60
	protected override async UniTask<bool> AchievementIsUnlocked_Online_FullGame(PlatformAPI.AchievementFullGame achievement, float maxTimeout)
	{
		return this.SupportsOnlineAchievements();
	}

	// Token: 0x0600039E RID: 926 RVA: 0x000288A4 File Offset: 0x00026AA4
	protected override async UniTask<bool> AchievementUnlock_Online_Demo(PlatformAPI.AchievementDemo achievement, float maxTimeout)
	{
		return this.SupportsOnlineAchievements();
	}

	// Token: 0x0600039F RID: 927 RVA: 0x000288E8 File Offset: 0x00026AE8
	protected override async UniTask<bool> AchievementUnlock_Online_FullGame(PlatformAPI.AchievementFullGame achievement, float maxTimeout)
	{
		return this.SupportsOnlineAchievements();
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x0002892C File Offset: 0x00026B2C
	protected override async UniTask<bool> AchievementsClearAll_Online(float maxTimeout)
	{
		return this.SupportsOnlineAchievements();
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x00008AF1 File Offset: 0x00006CF1
	public override string GetUserID_String()
	{
		return "undefined";
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0000774E File Offset: 0x0000594E
	public override void _Update()
	{
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x0000774E File Offset: 0x0000594E
	public override void _OnClose()
	{
	}
}
