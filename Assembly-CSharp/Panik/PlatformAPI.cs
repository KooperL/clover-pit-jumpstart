using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Panik
{
	public abstract class PlatformAPI
	{
		// Token: 0x06000CE3 RID: 3299 RVA: 0x000538DE File Offset: 0x00051ADE
		public static PlatformAPI.ApiKind ApiKindGet()
		{
			return Master._ApiKind;
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x000538E8 File Offset: 0x00051AE8
		public static async void Initialize()
		{
			if (PlatformAPI._initialized)
			{
				Debug.LogError("PlatformAPI already initialized!! Why are you calling it again?");
			}
			else
			{
				await UniTask.Delay(1, false, 8, default(CancellationToken), false);
				Debug.Log("Initializing PlatformAPI");
				PlatformAPI.instance = new PlatformAPI_Steam();
				UniTask<bool>.Awaiter awaiter = PlatformAPI.instance._InitializationBeing().GetAwaiter();
				UniTask<bool>.Awaiter awaiter2;
				if (!awaiter.IsCompleted)
				{
					await awaiter;
					awaiter = awaiter2;
					awaiter2 = default(UniTask<bool>.Awaiter);
				}
				if (!awaiter.GetResult())
				{
					PlatformAPI.failedInitialization = true;
				}
				else
				{
					PlatformAPI.instance._AchievementsUpdate();
					awaiter = PlatformAPI.instance._InitializationFinalize().GetAwaiter();
					if (!awaiter.IsCompleted)
					{
						await awaiter;
						awaiter = awaiter2;
						awaiter2 = default(UniTask<bool>.Awaiter);
					}
					if (!awaiter.GetResult())
					{
						PlatformAPI.failedInitialization = true;
					}
					else
					{
						PlatformAPI._initialized = true;
						Debug.Log("PlatformAPI Initialized");
					}
				}
			}
		}

		protected abstract UniTask<bool> _InitializationBeing();

		protected abstract UniTask<bool> _InitializationFinalize();

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00053917 File Offset: 0x00051B17
		public static bool InitializationFailed()
		{
			return PlatformAPI.failedInitialization;
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0005391E File Offset: 0x00051B1E
		public static bool IsInitialized()
		{
			return !PlatformAPI.failedInitialization && PlatformAPI._initialized;
		}

		public abstract bool IsUsable();

		public abstract bool SupportsOnlineFunctionalities();

		public abstract bool IsOnline();

		// Token: 0x06000CEC RID: 3308 RVA: 0x0005392E File Offset: 0x00051B2E
		public static bool AchievementIsUnlocked_Demo(PlatformAPI.AchievementDemo achievement)
		{
			return Master.IsDemo && PlatformAPI.achievementsData.AchievementUnlockState_Local_DemoGet(achievement);
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x00053944 File Offset: 0x00051B44
		public static bool AchievementIsUnlocked_FullGame(PlatformAPI.AchievementFullGame achievement)
		{
			return !Master.IsDemo && PlatformAPI.achievementsData.AchievementUnlockState_Local_FullGameGet(achievement);
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0005395A File Offset: 0x00051B5A
		public static bool AchievementUnlock_Demo(PlatformAPI.AchievementDemo achievement)
		{
			if (!Master.IsDemo)
			{
				return false;
			}
			if (GameplayMaster.IsCustomSeed())
			{
				return false;
			}
			bool flag = PlatformAPI.achievementsData.AchievementUnlockState_Local_DemoSet(achievement, true);
			if (flag)
			{
				PlatformAPI._achievementsRequestSave = true;
			}
			return flag;
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x00053983 File Offset: 0x00051B83
		public static bool AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame achievement)
		{
			if (Master.IsDemo)
			{
				return false;
			}
			if (GameplayMaster.IsCustomSeed())
			{
				return false;
			}
			if (GameplayMaster.IsCustomSeed())
			{
				return false;
			}
			bool flag = PlatformAPI.achievementsData.AchievementUnlockState_Local_FullGameSet(achievement, true);
			if (flag)
			{
				PlatformAPI._achievementsRequestSave = true;
			}
			return flag;
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x000539B8 File Offset: 0x00051BB8
		public static UniTask<ValueTuple<bool, bool>> AchievementsReset(string debugReason, bool forceInRelease = false)
		{
			PlatformAPI.<AchievementsReset>d__25 <AchievementsReset>d__;
			<AchievementsReset>d__.<>t__builder = AsyncUniTaskMethodBuilder<ValueTuple<bool, bool>>.Create();
			<AchievementsReset>d__.debugReason = debugReason;
			<AchievementsReset>d__.forceInRelease = forceInRelease;
			<AchievementsReset>d__.<>1__state = -1;
			<AchievementsReset>d__.<>t__builder.Start<PlatformAPI.<AchievementsReset>d__25>(ref <AchievementsReset>d__);
			return <AchievementsReset>d__.<>t__builder.Task;
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x00053A03 File Offset: 0x00051C03
		protected static string AchievemntGetEnumString_Demo(PlatformAPI.AchievementDemo achievement)
		{
			if (!PlatformAPI._demoAchEnumNames.ContainsKey(achievement))
			{
				PlatformAPI._demoAchEnumNames.Add(achievement, achievement.ToString());
			}
			return PlatformAPI._demoAchEnumNames[achievement];
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00053A35 File Offset: 0x00051C35
		protected static string AchievemntGetEnumString_FullGame(PlatformAPI.AchievementFullGame achievement)
		{
			if (!PlatformAPI._fullGameAchEnumNames.ContainsKey(achievement))
			{
				PlatformAPI._fullGameAchEnumNames.Add(achievement, achievement.ToString());
			}
			return PlatformAPI._fullGameAchEnumNames[achievement];
		}

		public abstract bool SupportsOnlineAchievements();

		protected abstract UniTask<bool> AchievementIsUnlocked_Online_Demo(PlatformAPI.AchievementDemo achievement, float maxTimeout);

		protected abstract UniTask<bool> AchievementIsUnlocked_Online_FullGame(PlatformAPI.AchievementFullGame achievement, float maxTimeout);

		protected abstract UniTask<bool> AchievementUnlock_Online_Demo(PlatformAPI.AchievementDemo achievement, float maxTimeout);

		protected abstract UniTask<bool> AchievementUnlock_Online_FullGame(PlatformAPI.AchievementFullGame achievement, float maxTimeout);

		protected abstract UniTask<bool> AchievementsClearAll_Online(float maxTimeout);

		// Token: 0x06000CF9 RID: 3321 RVA: 0x00053A67 File Offset: 0x00051C67
		protected virtual void Achievements_Prewarm_Demo()
		{
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x00053A69 File Offset: 0x00051C69
		protected virtual void Achievements_Prewarm_FullGame()
		{
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00053A6C File Offset: 0x00051C6C
		private async void _AchievementsUpdate()
		{
			Debug.Log("Achievements update: waiting for game initialization");
			while (!PlatformMaster.IsInitialized())
			{
				await UniTask.Delay(1, false, 8, default(CancellationToken), false);
			}
			float saveInterval = (float)PlatformDataMaster.SaveCooldownTimeGet;
			int achIter_Demo = 0;
			int achIter_Full = 0;
			for (;;)
			{
				if (this.SupportsOnlineAchievements() && this.IsOnline())
				{
					if (Master.IsDemo)
					{
						this.Achievements_Prewarm_Demo();
						int achCount = 3;
						if (achIter_Demo < achCount)
						{
							PlatformAPI.AchievementDemo ach = (PlatformAPI.AchievementDemo)achIter_Demo;
							bool offlineState = PlatformAPI.AchievementIsUnlocked_Demo(ach);
							bool flag = await this.AchievementIsUnlocked_Online_Demo(ach, 10f);
							if (offlineState != flag)
							{
								if (!offlineState && flag)
								{
									PlatformAPI.AchievementUnlock_Demo(ach);
								}
								else if (offlineState && !flag)
								{
									await this.AchievementUnlock_Online_Demo(ach, 10f);
								}
							}
							achIter_Demo++;
							if (achIter_Demo >= achCount)
							{
								achIter_Demo = 0;
							}
						}
					}
					else
					{
						this.Achievements_Prewarm_FullGame();
						int achCount = 30;
						if (achIter_Full < achCount)
						{
							PlatformAPI.AchievementFullGame ach2 = (PlatformAPI.AchievementFullGame)achIter_Full;
							bool offlineState = PlatformAPI.AchievementIsUnlocked_FullGame(ach2);
							bool flag2 = await this.AchievementIsUnlocked_Online_FullGame(ach2, 10f);
							if (offlineState != flag2)
							{
								if (!offlineState && flag2)
								{
									PlatformAPI.AchievementUnlock_FullGame(ach2);
								}
								else if (offlineState && !flag2)
								{
									await this.AchievementUnlock_Online_FullGame(ach2, 10f);
								}
							}
							achIter_Full++;
							if (achIter_Full >= achCount)
							{
								achIter_Full = 0;
							}
						}
					}
				}
				int waitMillisec = 200;
				await UniTask.Delay(waitMillisec, false, 8, default(CancellationToken), false);
				saveInterval -= (float)waitMillisec;
				if (PlatformAPI._achievementsRequestSave && saveInterval <= 0f)
				{
					Data.SaveAchievements();
					PlatformAPI._achievementsRequestSave = false;
					saveInterval = 10f;
				}
			}
		}

		public abstract string GetUserID_String();

		// Token: 0x06000CFD RID: 3325 RVA: 0x00053AA3 File Offset: 0x00051CA3
		public static string GetOsString()
		{
			return SystemInfo.operatingSystem;
		}

		public abstract void _Update();

		public abstract void _OnClose();

		public static PlatformAPI instance = null;

		public const float ACHIEVEMENTS_UPDATE_INTERVAL = 0.2f;

		public const float ACHIEVEMENTS_NEXT_SAVE_DELAY = 10f;

		public const float ACHIEVEMENTS_ONLINE_OPERATIONS_MAX_TIMEOUT = 10f;

		public const PlatformAPI.ApiKind API_KIND = PlatformAPI.ApiKind.Steam;

		protected static bool failedInitialization = false;

		protected static bool _initialized = false;

		public static PlatformAPI.AchievementsData achievementsData = new PlatformAPI.AchievementsData();

		private static Dictionary<PlatformAPI.AchievementDemo, string> _demoAchEnumNames = new Dictionary<PlatformAPI.AchievementDemo, string>();

		private static Dictionary<PlatformAPI.AchievementFullGame, string> _fullGameAchEnumNames = new Dictionary<PlatformAPI.AchievementFullGame, string>();

		private static bool _achievementsRequestSave = false;

		public enum ApiKind
		{
			Noone,
			Steam,
			Gog,
			Epic,
			Itch,
			GooglePlay,
			AppleStore,
			PlayStation,
			Xbox,
			Nintendo
		}

		public enum AchievementDemo
		{
			ADecentCollection,
			WinnerMentality,
			FearOfNeedles,
			Count,
			Undefined
		}

		public enum AchievementFullGame
		{
			Drawer0,
			Drawer1,
			Drawer2,
			Drawer3,
			CorporeSano,
			AwDangit,
			LuckyDay,
			ThisWillHurt,
			ANewHobby,
			TheNumberOfTheBeast,
			Cultist,
			Rentier,
			Manifesting,
			FullCollection,
			SuperMegaUltraUltimateWin,
			Omnipotent,
			TheRighteousPath,
			DivineIntervention,
			DoNotBeAfraid,
			CompletedDatabase,
			TheStructure,
			Ascension,
			PatternRecognition,
			MyPrecious,
			TheLordOfThePit,
			ItsNotAnAddiction,
			ItsDedication,
			AllMyHardEarnedMoney,
			NearDeathExperience,
			Scrooge,
			Count,
			Undefined
		}

		[SerializeField]
		public class AchievementsData
		{
			// Token: 0x06001409 RID: 5129 RVA: 0x0007C225 File Offset: 0x0007A425
			public void Saving_Prepare()
			{
				this.unlockedAchievements_String_Demo = PlatformDataMaster.EnumListToString<PlatformAPI.AchievementDemo>(this.unlockedAchievements_Demo, ',');
				this.unlockedAchievements_String_FullGame = PlatformDataMaster.EnumListToString<PlatformAPI.AchievementFullGame>(this.unlockedAchievements_FullGame, ',');
			}

			// Token: 0x0600140A RID: 5130 RVA: 0x0007C24D File Offset: 0x0007A44D
			public void Load_Restore()
			{
				this.unlockedAchievements_Demo = PlatformDataMaster.EnumListFromString<PlatformAPI.AchievementDemo>(this.unlockedAchievements_String_Demo, ',');
				this.unlockedAchievements_FullGame = PlatformDataMaster.EnumListFromString<PlatformAPI.AchievementFullGame>(this.unlockedAchievements_String_FullGame, ',');
			}

			// Token: 0x0600140B RID: 5131 RVA: 0x0007C275 File Offset: 0x0007A475
			public bool AchievementUnlockState_Local_DemoGet(PlatformAPI.AchievementDemo achievement)
			{
				return this.unlockedAchievements_Demo.Contains(achievement);
			}

			// Token: 0x0600140C RID: 5132 RVA: 0x0007C283 File Offset: 0x0007A483
			public bool AchievementUnlockState_Local_FullGameGet(PlatformAPI.AchievementFullGame achievement)
			{
				return this.unlockedAchievements_FullGame.Contains(achievement);
			}

			// Token: 0x0600140D RID: 5133 RVA: 0x0007C291 File Offset: 0x0007A491
			public bool AchievementUnlockState_Local_DemoSet(PlatformAPI.AchievementDemo achievement, bool state)
			{
				if (state)
				{
					if (this.unlockedAchievements_Demo.Contains(achievement))
					{
						return false;
					}
					this.unlockedAchievements_Demo.Add(achievement);
				}
				else
				{
					this.unlockedAchievements_Demo.Remove(achievement);
				}
				return true;
			}

			// Token: 0x0600140E RID: 5134 RVA: 0x0007C2C2 File Offset: 0x0007A4C2
			public bool AchievementUnlockState_Local_FullGameSet(PlatformAPI.AchievementFullGame achievement, bool state)
			{
				if (state)
				{
					if (this.unlockedAchievements_FullGame.Contains(achievement))
					{
						return false;
					}
					this.unlockedAchievements_FullGame.Add(achievement);
				}
				else
				{
					this.unlockedAchievements_FullGame.Remove(achievement);
				}
				return true;
			}

			[SerializeField]
			private string unlockedAchievements_String_Demo;

			[SerializeField]
			private string unlockedAchievements_String_FullGame;

			private List<PlatformAPI.AchievementDemo> unlockedAchievements_Demo = new List<PlatformAPI.AchievementDemo>();

			private List<PlatformAPI.AchievementFullGame> unlockedAchievements_FullGame = new List<PlatformAPI.AchievementFullGame>();

			private static StringBuilder _sb;
		}
	}
}
