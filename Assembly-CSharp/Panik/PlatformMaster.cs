using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Panik
{
	// Token: 0x0200016C RID: 364
	public class PlatformMaster : MonoBehaviour
	{
		// Token: 0x060010CF RID: 4303 RVA: 0x00013B7B File Offset: 0x00011D7B
		public static bool PlatformResolutionCanChange()
		{
			return PlatformMaster.PlatformIsComputer();
		}

		// Token: 0x060010D0 RID: 4304 RVA: 0x00013B82 File Offset: 0x00011D82
		public static Data.SettingsData.VerticalResolution[] PlatformSupportedVerticalResolutions()
		{
			PlatformMaster.PlatformIsComputer();
			return new Data.SettingsData.VerticalResolution[]
			{
				Data.SettingsData.VerticalResolution._native,
				Data.SettingsData.VerticalResolution._360p,
				Data.SettingsData.VerticalResolution._480p,
				Data.SettingsData.VerticalResolution._720p
			};
		}

		// Token: 0x060010D1 RID: 4305 RVA: 0x00013B9B File Offset: 0x00011D9B
		public static Data.SettingsData.WidthAspectRatio[] PlatformSupportedWidthAspectRatios()
		{
			if (PlatformMaster.PlatformIsComputer())
			{
				return new Data.SettingsData.WidthAspectRatio[1];
			}
			return new Data.SettingsData.WidthAspectRatio[] { Data.SettingsData.WidthAspectRatio._16_9 };
		}

		// Token: 0x060010D2 RID: 4306 RVA: 0x0007244C File Offset: 0x0007064C
		public static int QualityDefaultGet()
		{
			switch (PlatformMaster.PlatformKindGet())
			{
			case PlatformMaster.PlatformKind.PC:
				return 2;
			case PlatformMaster.PlatformKind.Linux:
				return 2;
			case PlatformMaster.PlatformKind.Mac:
				return 2;
			case PlatformMaster.PlatformKind.WebGL:
				return 1;
			case PlatformMaster.PlatformKind.Android:
				return 1;
			case PlatformMaster.PlatformKind.iOS:
				return 1;
			case PlatformMaster.PlatformKind.PS4:
				return 2;
			case PlatformMaster.PlatformKind.PS5:
				return 2;
			case PlatformMaster.PlatformKind.XboxOne:
				return 2;
			case PlatformMaster.PlatformKind.XboxSeries:
				return 2;
			case PlatformMaster.PlatformKind.NintendoSwitch:
				return 0;
			default:
				return 1;
			}
		}

		// Token: 0x060010D3 RID: 4307 RVA: 0x00013B7B File Offset: 0x00011D7B
		public static bool PlatformSupports_FullscreenSwitching()
		{
			return PlatformMaster.PlatformIsComputer();
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00013BB5 File Offset: 0x00011DB5
		public static PlatformMaster.PlatformKind PlatformKindGet()
		{
			return Master._PlatformKind;
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x00013BBC File Offset: 0x00011DBC
		public static bool PlatformIsComputer()
		{
			return PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.PC || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.Linux || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.Mac;
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00013BD8 File Offset: 0x00011DD8
		public static bool PlatformIsMobile()
		{
			return PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.Android || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.iOS;
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00013BED File Offset: 0x00011DED
		public static bool PlatformIsConsole()
		{
			return PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.NintendoSwitch || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.PS4 || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.XboxOne || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.XboxSeries || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.PS5;
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00013C1C File Offset: 0x00011E1C
		public static bool PlatformIsWeb()
		{
			return PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.WebGL;
		}

		// Token: 0x060010D9 RID: 4313 RVA: 0x00013C29 File Offset: 0x00011E29
		public static bool IsInitialized()
		{
			return PlatformMaster.initialized;
		}

		// Token: 0x060010DA RID: 4314 RVA: 0x00013C30 File Offset: 0x00011E30
		public static void Initialize()
		{
			PlatformMaster.instance.InstantInitialization();
			PlatformMaster.instance.PlatformInitializationCoroutine();
		}

		// Token: 0x060010DB RID: 4315 RVA: 0x000724AC File Offset: 0x000706AC
		private void InstantInitialization()
		{
			if (PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.Undefined)
			{
				Debug.LogError("Platform is undefined! Please set the platform in the Master.cs inspector!");
				return;
			}
			Debug.Log("Platfom: " + PlatformMaster.PlatformKindGet().ToString());
			UnityAction unityAction = this.onInitializationBegin;
			if (unityAction != null)
			{
				unityAction();
			}
			SceneMaster.Initialize();
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x00072508 File Offset: 0x00070708
		private async UniTask PlatformInitializationCoroutine()
		{
			await UniTask.Yield();
			try
			{
				PlatformAPI.Initialize();
			}
			catch (Exception ex)
			{
				"Catched: " + ex.Message;
			}
			float maxApiInitTime = 5f;
			while (!PlatformAPI.IsInitialized() && maxApiInitTime > 0f)
			{
				maxApiInitTime -= Time.deltaTime;
				await UniTask.Yield();
			}
			if (PlatformAPI.InitializationFailed())
			{
				string text = "Platform API initialization failed! Aborted coroutine!";
				ConsolePrompt.LogError(text, "", 0f);
				Debug.LogError(text);
			}
			else
			{
				bool canBackupData = true;
				try
				{
					UniTask<bool>.Awaiter awaiter = Data._VersionsLoadAndSave().GetAwaiter();
					UniTask<bool>.Awaiter awaiter2;
					if (!awaiter.IsCompleted)
					{
						await awaiter;
						awaiter = awaiter2;
						awaiter2 = default(UniTask<bool>.Awaiter);
					}
					if (!awaiter.GetResult())
					{
						Debug.LogWarning("Error while saving the current game version data! ");
					}
					Data.LoadSettingsAndApply();
					Data.LoadAchievements();
					awaiter = Data.LoadGame(0, false).GetAwaiter();
					if (!awaiter.IsCompleted)
					{
						await awaiter;
						awaiter = awaiter2;
						awaiter2 = default(UniTask<bool>.Awaiter);
					}
					if (!awaiter.GetResult() || Data.JsonErrorWhileLoadingGame_Get())
					{
						canBackupData = false;
					}
				}
				catch (Exception ex2)
				{
					string text2 = "Error while loading data: Exception" + ex2.Message;
					Debug.LogError(text2);
					if (PlatformMaster.PlatformIsComputer())
					{
						ConsolePrompt.LogError(text2, "", 0f);
					}
				}
				if (canBackupData)
				{
					try
					{
						string gameFolderPath = PlatformDataMaster.GameFolderPath;
						if (!Directory.Exists(gameFolderPath))
						{
							Directory.CreateDirectory(gameFolderPath);
						}
						string text3 = PlatformDataMaster.PathGet_GameDataFile(0, "_Backup_Oldest");
						string text4 = PlatformDataMaster.PathGet_GameDataFile(0, "_Backup_Old");
						string text5 = PlatformDataMaster.PathGet_GameDataFile(0, "_Backup_Newest");
						string text6 = PlatformDataMaster.PathGet_GameDataFile(0, "");
						bool flag = File.Exists(text3);
						bool flag2 = File.Exists(text4);
						bool flag3 = File.Exists(text5);
						if (flag && flag2 && flag3)
						{
							File.Delete(text3);
						}
						if (flag2 && flag3)
						{
							File.Move(text4, text3);
						}
						if (flag3)
						{
							File.Move(text5, text4);
						}
						if (File.Exists(text6))
						{
							File.Copy(text6, text5);
						}
						string text7 = gameFolderPath + "Backup README.txt";
						if (!File.Exists(text7))
						{
							File.WriteAllText(text7, "Backups are generated each time you launch CloverPit. You can have up to 3.\n\nBefore restoring a GameData file from a Backup, copy all your Data (backups included) somewhere else (outside of the game folder) for safety reasons.\n\nTo restore your data from a backup, replace the current GameData file (if present) with a backup file:\nTo do so, make sure to rename the backup file by removing the \"_Backup..\" suffix from the name so that it can be recognized as normal GameData.\nIt needs to be placed in the same folder where the original GameData was!\n\nWarning: It's suggested to temporarely pause any Cloud services that syncronizes your GameData (Like Steam Cloud), as it might get in the way!\n\nWarning: Everytime you launch the application, the oldest backup file will likely get replaced by a newer one! This is why it's best to backup your backups (wow) before launching CloverPit again!");
						}
						goto IL_037F;
					}
					catch (Exception)
					{
						goto IL_037F;
					}
				}
				if (Data.JsonErrorWhileLoadingGame_Get())
				{
					ConsolePrompt.LogError("Data may be corrupted. Check your progress, and eventually restore a Backup --- CTRL + TAB for console.", "", 10f);
				}
				IL_037F:
				PlatformMaster.initialized = true;
				if (Level.CurrentSceneIndex == Level.SceneIndex.Loading)
				{
					Level.GoTo(Level.SceneIndex.Intro, false);
				}
				else
				{
					Level.Restart(true);
				}
				UnityAction unityAction = this.onInitializationEnd;
				if (unityAction != null)
				{
					unityAction();
				}
				Debug.Log("Game initialization finished!");
			}
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x00013C47 File Offset: 0x00011E47
		public static bool EscButtonCanCloseTheGame()
		{
			return PlatformMaster.PlatformIsComputer() && Master.instance.ESCAPE_CAN_CLOSE_GAME;
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x00013B7B File Offset: 0x00011D7B
		public static bool IsFullscreenSupported()
		{
			return PlatformMaster.PlatformIsComputer();
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x00013C5F File Offset: 0x00011E5F
		private void Awake()
		{
			if (PlatformMaster.instance != null)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			PlatformMaster.instance = this;
			UnityAction unityAction = this.onAwake;
			if (unityAction == null)
			{
				return;
			}
			unityAction();
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x00013C90 File Offset: 0x00011E90
		private void OnDestroy()
		{
			if (PlatformMaster.instance == this)
			{
				PlatformAPI platformAPI = PlatformAPI.instance;
				if (platformAPI != null)
				{
					platformAPI._OnClose();
				}
			}
			if (PlatformMaster.instance == this)
			{
				PlatformMaster.instance = null;
			}
		}

		// Token: 0x060010E1 RID: 4321 RVA: 0x00013CC2 File Offset: 0x00011EC2
		private void Update()
		{
			UnityAction unityAction = this.onUpdate;
			if (unityAction != null)
			{
				unityAction();
			}
			PlatformAPI platformAPI = PlatformAPI.instance;
			if (platformAPI == null)
			{
				return;
			}
			platformAPI._Update();
		}

		// Token: 0x040011B0 RID: 4528
		public static PlatformMaster instance;

		// Token: 0x040011B1 RID: 4529
		public UnityAction onAwake;

		// Token: 0x040011B2 RID: 4530
		public UnityAction onInitializationBegin;

		// Token: 0x040011B3 RID: 4531
		public UnityAction onInitializationEnd;

		// Token: 0x040011B4 RID: 4532
		public UnityAction onUpdate;

		// Token: 0x040011B5 RID: 4533
		private static bool initialized;

		// Token: 0x0200016D RID: 365
		public enum PlatformKind
		{
			// Token: 0x040011B7 RID: 4535
			PC,
			// Token: 0x040011B8 RID: 4536
			Linux,
			// Token: 0x040011B9 RID: 4537
			Mac,
			// Token: 0x040011BA RID: 4538
			WebGL,
			// Token: 0x040011BB RID: 4539
			Android,
			// Token: 0x040011BC RID: 4540
			iOS,
			// Token: 0x040011BD RID: 4541
			PS4,
			// Token: 0x040011BE RID: 4542
			PS5,
			// Token: 0x040011BF RID: 4543
			XboxOne,
			// Token: 0x040011C0 RID: 4544
			XboxSeries,
			// Token: 0x040011C1 RID: 4545
			NintendoSwitch,
			// Token: 0x040011C2 RID: 4546
			Count,
			// Token: 0x040011C3 RID: 4547
			Undefined
		}
	}
}
