using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Panik
{
	// Token: 0x0200014B RID: 331
	public abstract class PlatformAPI
	{
		// Token: 0x0600102D RID: 4141 RVA: 0x000135DB File Offset: 0x000117DB
		public static PlatformAPI.ApiKind ApiKindGet()
		{
			return Master._ApiKind;
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x0006F9F0 File Offset: 0x0006DBF0
		public static async void Initialize()
		{
			if (PlatformAPI._initialized)
			{
				Debug.LogError("PlatformAPI already initialized!! Why are you calling it again?");
			}
			else
			{
				await UniTask.Delay(1, false, PlayerLoopTiming.Update, default(CancellationToken), false);
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

		// Token: 0x0600102F RID: 4143
		protected abstract UniTask<bool> _InitializationBeing();

		// Token: 0x06001030 RID: 4144
		protected abstract UniTask<bool> _InitializationFinalize();

		// Token: 0x06001031 RID: 4145 RVA: 0x000135E2 File Offset: 0x000117E2
		public static bool InitializationFailed()
		{
			return PlatformAPI.failedInitialization;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x000135E9 File Offset: 0x000117E9
		public static bool IsInitialized()
		{
			return !PlatformAPI.failedInitialization && PlatformAPI._initialized;
		}

		// Token: 0x06001033 RID: 4147
		public abstract bool IsUsable();

		// Token: 0x06001034 RID: 4148
		public abstract bool SupportsOnlineFunctionalities();

		// Token: 0x06001035 RID: 4149
		public abstract bool IsOnline();

		// Token: 0x06001036 RID: 4150 RVA: 0x000135F9 File Offset: 0x000117F9
		public static bool AchievementIsUnlocked_Demo(PlatformAPI.AchievementDemo achievement)
		{
			return Master.IsDemo && PlatformAPI.achievementsData.AchievementUnlockState_Local_DemoGet(achievement);
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x0001360F File Offset: 0x0001180F
		public static bool AchievementIsUnlocked_FullGame(PlatformAPI.AchievementFullGame achievement)
		{
			return !Master.IsDemo && PlatformAPI.achievementsData.AchievementUnlockState_Local_FullGameGet(achievement);
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x00013625 File Offset: 0x00011825
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

		// Token: 0x06001039 RID: 4153 RVA: 0x0001364E File Offset: 0x0001184E
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

		// Token: 0x0600103A RID: 4154 RVA: 0x0006FA20 File Offset: 0x0006DC20
		public static async UniTask<ValueTuple<bool, bool>> AchievementsReset(string debugReason, bool forceInRelease = false)
		{
			ValueTuple<bool, bool> valueTuple;
			if (!Application.isEditor && !Master.IsDebugBuild && !forceInRelease)
			{
				Debug.LogError("AchievementsReset() - This function should only be called in the editor or in debug builds.");
				valueTuple = new ValueTuple<bool, bool>(false, false);
			}
			else
			{
				Debug.LogWarning("Clearing achievements. Debug reason: " + debugReason);
				bool flag = false;
				if (PlatformAPI.instance.SupportsOnlineFunctionalities() && PlatformAPI.instance.IsOnline())
				{
					flag = await PlatformAPI.instance.AchievementsClearAll_Online(10f);
				}
				int num = 30;
				for (int i = 0; i < num; i++)
				{
					PlatformAPI.achievementsData.AchievementUnlockState_Local_FullGameSet((PlatformAPI.AchievementFullGame)i, false);
				}
				PlatformAPI._achievementsRequestSave = true;
				valueTuple = new ValueTuple<bool, bool>(true, flag);
			}
			return valueTuple;
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x00013680 File Offset: 0x00011880
		protected static string AchievemntGetEnumString_Demo(PlatformAPI.AchievementDemo achievement)
		{
			if (!PlatformAPI._demoAchEnumNames.ContainsKey(achievement))
			{
				PlatformAPI._demoAchEnumNames.Add(achievement, achievement.ToString());
			}
			return PlatformAPI._demoAchEnumNames[achievement];
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x000136B2 File Offset: 0x000118B2
		protected static string AchievemntGetEnumString_FullGame(PlatformAPI.AchievementFullGame achievement)
		{
			if (!PlatformAPI._fullGameAchEnumNames.ContainsKey(achievement))
			{
				PlatformAPI._fullGameAchEnumNames.Add(achievement, achievement.ToString());
			}
			return PlatformAPI._fullGameAchEnumNames[achievement];
		}

		// Token: 0x0600103D RID: 4157
		public abstract bool SupportsOnlineAchievements();

		// Token: 0x0600103E RID: 4158
		protected abstract UniTask<bool> AchievementIsUnlocked_Online_Demo(PlatformAPI.AchievementDemo achievement, float maxTimeout);

		// Token: 0x0600103F RID: 4159
		protected abstract UniTask<bool> AchievementIsUnlocked_Online_FullGame(PlatformAPI.AchievementFullGame achievement, float maxTimeout);

		// Token: 0x06001040 RID: 4160
		protected abstract UniTask<bool> AchievementUnlock_Online_Demo(PlatformAPI.AchievementDemo achievement, float maxTimeout);

		// Token: 0x06001041 RID: 4161
		protected abstract UniTask<bool> AchievementUnlock_Online_FullGame(PlatformAPI.AchievementFullGame achievement, float maxTimeout);

		// Token: 0x06001042 RID: 4162
		protected abstract UniTask<bool> AchievementsClearAll_Online(float maxTimeout);

		// Token: 0x06001043 RID: 4163 RVA: 0x0000774E File Offset: 0x0000594E
		protected virtual void Achievements_Prewarm_Demo()
		{
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x0000774E File Offset: 0x0000594E
		protected virtual void Achievements_Prewarm_FullGame()
		{
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0006FA6C File Offset: 0x0006DC6C
		private async void _AchievementsUpdate()
		{
			Debug.Log("Achievements update: waiting for game initialization");
			while (!PlatformMaster.IsInitialized())
			{
				await UniTask.Delay(1, false, PlayerLoopTiming.Update, default(CancellationToken), false);
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
				await UniTask.Delay(waitMillisec, false, PlayerLoopTiming.Update, default(CancellationToken), false);
				saveInterval -= (float)waitMillisec;
				if (PlatformAPI._achievementsRequestSave && saveInterval <= 0f)
				{
					Data.SaveAchievements();
					PlatformAPI._achievementsRequestSave = false;
					saveInterval = 10f;
				}
			}
		}

		// Token: 0x06001046 RID: 4166
		public abstract string GetUserID_String();

		// Token: 0x06001047 RID: 4167 RVA: 0x000136E4 File Offset: 0x000118E4
		public static string GetOsString()
		{
			return SystemInfo.operatingSystem;
		}

		// Token: 0x06001048 RID: 4168
		public abstract void _Update();

		// Token: 0x06001049 RID: 4169
		public abstract void _OnClose();

		// Token: 0x040010E4 RID: 4324
		public static PlatformAPI instance = null;

		// Token: 0x040010E5 RID: 4325
		public const float ACHIEVEMENTS_UPDATE_INTERVAL = 0.2f;

		// Token: 0x040010E6 RID: 4326
		public const float ACHIEVEMENTS_NEXT_SAVE_DELAY = 10f;

		// Token: 0x040010E7 RID: 4327
		public const float ACHIEVEMENTS_ONLINE_OPERATIONS_MAX_TIMEOUT = 10f;

		// Token: 0x040010E8 RID: 4328
		public const PlatformAPI.ApiKind API_KIND = PlatformAPI.ApiKind.Steam;

		// Token: 0x040010E9 RID: 4329
		protected static bool failedInitialization = false;

		// Token: 0x040010EA RID: 4330
		protected static bool _initialized = false;

		// Token: 0x040010EB RID: 4331
		public static PlatformAPI.AchievementsData achievementsData = new PlatformAPI.AchievementsData();

		// Token: 0x040010EC RID: 4332
		private static Dictionary<PlatformAPI.AchievementDemo, string> _demoAchEnumNames = new Dictionary<PlatformAPI.AchievementDemo, string>();

		// Token: 0x040010ED RID: 4333
		private static Dictionary<PlatformAPI.AchievementFullGame, string> _fullGameAchEnumNames = new Dictionary<PlatformAPI.AchievementFullGame, string>();

		// Token: 0x040010EE RID: 4334
		private static bool _achievementsRequestSave = false;

		// Token: 0x0200014C RID: 332
		public enum ApiKind
		{
			// Token: 0x040010F0 RID: 4336
			Noone,
			// Token: 0x040010F1 RID: 4337
			Steam,
			// Token: 0x040010F2 RID: 4338
			Gog,
			// Token: 0x040010F3 RID: 4339
			Epic,
			// Token: 0x040010F4 RID: 4340
			Itch,
			// Token: 0x040010F5 RID: 4341
			GooglePlay,
			// Token: 0x040010F6 RID: 4342
			AppleStore,
			// Token: 0x040010F7 RID: 4343
			PlayStation,
			// Token: 0x040010F8 RID: 4344
			Xbox,
			// Token: 0x040010F9 RID: 4345
			Nintendo
		}

		// Token: 0x0200014D RID: 333
		public enum AchievementDemo
		{
			// Token: 0x040010FB RID: 4347
			ADecentCollection,
			// Token: 0x040010FC RID: 4348
			WinnerMentality,
			// Token: 0x040010FD RID: 4349
			FearOfNeedles,
			// Token: 0x040010FE RID: 4350
			Count,
			// Token: 0x040010FF RID: 4351
			Undefined
		}

		// Token: 0x0200014E RID: 334
		public enum AchievementFullGame
		{
			// Token: 0x04001101 RID: 4353
			Drawer0,
			// Token: 0x04001102 RID: 4354
			Drawer1,
			// Token: 0x04001103 RID: 4355
			Drawer2,
			// Token: 0x04001104 RID: 4356
			Drawer3,
			// Token: 0x04001105 RID: 4357
			CorporeSano,
			// Token: 0x04001106 RID: 4358
			AwDangit,
			// Token: 0x04001107 RID: 4359
			LuckyDay,
			// Token: 0x04001108 RID: 4360
			ThisWillHurt,
			// Token: 0x04001109 RID: 4361
			ANewHobby,
			// Token: 0x0400110A RID: 4362
			TheNumberOfTheBeast,
			// Token: 0x0400110B RID: 4363
			Cultist,
			// Token: 0x0400110C RID: 4364
			Rentier,
			// Token: 0x0400110D RID: 4365
			Manifesting,
			// Token: 0x0400110E RID: 4366
			FullCollection,
			// Token: 0x0400110F RID: 4367
			SuperMegaUltraUltimateWin,
			// Token: 0x04001110 RID: 4368
			Omnipotent,
			// Token: 0x04001111 RID: 4369
			TheRighteousPath,
			// Token: 0x04001112 RID: 4370
			DivineIntervention,
			// Token: 0x04001113 RID: 4371
			DoNotBeAfraid,
			// Token: 0x04001114 RID: 4372
			CompletedDatabase,
			// Token: 0x04001115 RID: 4373
			TheStructure,
			// Token: 0x04001116 RID: 4374
			Ascension,
			// Token: 0x04001117 RID: 4375
			PatternRecognition,
			// Token: 0x04001118 RID: 4376
			MyPrecious,
			// Token: 0x04001119 RID: 4377
			TheLordOfThePit,
			// Token: 0x0400111A RID: 4378
			ItsNotAnAddiction,
			// Token: 0x0400111B RID: 4379
			ItsDedication,
			// Token: 0x0400111C RID: 4380
			AllMyHardEarnedMoney,
			// Token: 0x0400111D RID: 4381
			NearDeathExperience,
			// Token: 0x0400111E RID: 4382
			Scrooge,
			// Token: 0x0400111F RID: 4383
			Count,
			// Token: 0x04001120 RID: 4384
			Undefined
		}

		// Token: 0x0200014F RID: 335
		[SerializeField]
		public class AchievementsData
		{
			// Token: 0x0600104C RID: 4172 RVA: 0x00013723 File Offset: 0x00011923
			public void Saving_Prepare()
			{
				this.unlockedAchievements_String_Demo = PlatformDataMaster.EnumListToString<PlatformAPI.AchievementDemo>(this.unlockedAchievements_Demo, ',');
				this.unlockedAchievements_String_FullGame = PlatformDataMaster.EnumListToString<PlatformAPI.AchievementFullGame>(this.unlockedAchievements_FullGame, ',');
			}

			// Token: 0x0600104D RID: 4173 RVA: 0x0001374B File Offset: 0x0001194B
			public void Load_Restore()
			{
				this.unlockedAchievements_Demo = PlatformDataMaster.EnumListFromString<PlatformAPI.AchievementDemo>(this.unlockedAchievements_String_Demo, ',');
				this.unlockedAchievements_FullGame = PlatformDataMaster.EnumListFromString<PlatformAPI.AchievementFullGame>(this.unlockedAchievements_String_FullGame, ',');
			}

			// Token: 0x0600104E RID: 4174 RVA: 0x00013773 File Offset: 0x00011973
			public bool AchievementUnlockState_Local_DemoGet(PlatformAPI.AchievementDemo achievement)
			{
				return this.unlockedAchievements_Demo.Contains(achievement);
			}

			// Token: 0x0600104F RID: 4175 RVA: 0x00013781 File Offset: 0x00011981
			public bool AchievementUnlockState_Local_FullGameGet(PlatformAPI.AchievementFullGame achievement)
			{
				return this.unlockedAchievements_FullGame.Contains(achievement);
			}

			// Token: 0x06001050 RID: 4176 RVA: 0x0001378F File Offset: 0x0001198F
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

			// Token: 0x06001051 RID: 4177 RVA: 0x000137C0 File Offset: 0x000119C0
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

			// Token: 0x04001121 RID: 4385
			[SerializeField]
			private string unlockedAchievements_String_Demo;

			// Token: 0x04001122 RID: 4386
			[SerializeField]
			private string unlockedAchievements_String_FullGame;

			// Token: 0x04001123 RID: 4387
			private List<PlatformAPI.AchievementDemo> unlockedAchievements_Demo = new List<PlatformAPI.AchievementDemo>();

			// Token: 0x04001124 RID: 4388
			private List<PlatformAPI.AchievementFullGame> unlockedAchievements_FullGame = new List<PlatformAPI.AchievementFullGame>();

			// Token: 0x04001125 RID: 4389
			private static StringBuilder _sb;
		}
	}
}
