using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Panik
{
	public static class Data
	{
		// Token: 0x06000CB1 RID: 3249 RVA: 0x0005303D File Offset: 0x0005123D
		public static string PAchievementsDataGet()
		{
			return "afhjttiojd?s0989sdfl12";
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00053044 File Offset: 0x00051244
		public static string PGameDataGet(int oldVersionNumber, int gameDataVersion)
		{
			if (gameDataVersion < 2)
			{
				return null;
			}
			return "uoiyiuh_+=-5216gh;lj??!/345";
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x00053051 File Offset: 0x00051251
		public static string PGameDataGet_LastOne(int oldVersionNumber)
		{
			return Data.PGameDataGet(oldVersionNumber, 3);
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0005305A File Offset: 0x0005125A
		public static string Encrypt_Wrapped(string input, string password)
		{
			return PlatformDataMaster.EncryptCustom(input, password);
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x00053063 File Offset: 0x00051263
		public static string Decrypt_Wrapped(string input, string password)
		{
			return PlatformDataMaster.DecryptCustom(input, password);
		}

		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x0005306C File Offset: 0x0005126C
		public static Data.VersionData versionData
		{
			get
			{
				return Data.VersionData.inst;
			}
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x00053074 File Offset: 0x00051274
		public static UniTask<bool> _VersionsLoadAndSave()
		{
			Data.<_VersionsLoadAndSave>d__9 <_VersionsLoadAndSave>d__;
			<_VersionsLoadAndSave>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<_VersionsLoadAndSave>d__.<>1__state = -1;
			<_VersionsLoadAndSave>d__.<>t__builder.Start<Data.<_VersionsLoadAndSave>d__9>(ref <_VersionsLoadAndSave>d__);
			return <_VersionsLoadAndSave>d__.<>t__builder.Task;
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x000530AF File Offset: 0x000512AF
		private static void OnSettingsVersionChange(int oldVersionNumber, int newVersionNumber)
		{
			if (oldVersionNumber != newVersionNumber)
			{
				Data.settingsResetFlag = true;
			}
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x000530BC File Offset: 0x000512BC
		private static void OnGameVersionChange(int oldVersionNumber, int newVersionNumber)
		{
			Debug.Log("<color=orange>Game Data version differs!!! On disk: " + oldVersionNumber.ToString() + ", while game is: " + newVersionNumber.ToString());
			if (oldVersionNumber == 0 && newVersionNumber == 1)
			{
				Data.OnGameVersionChange_FirstPublisherBuildToSecond();
				return;
			}
			if (oldVersionNumber < 2 && newVersionNumber >= 2)
			{
				if (PlatformMaster.PlatformIsComputer())
				{
					string gameFolderPath = PlatformDataMaster.GameFolderPath;
					if (!Directory.Exists(gameFolderPath))
					{
						Directory.CreateDirectory(gameFolderPath);
					}
					string text = PlatformDataMaster.PathGet_GameDataFile(0, "");
					if (!File.Exists(text))
					{
						goto IL_0109;
					}
					string text2 = null;
					try
					{
						text2 = File.ReadAllText(text);
					}
					catch (Exception ex)
					{
						string text3 = "Failed to read data while trying to encrypt it! Exception:\n" + ex.Message;
						ConsolePrompt.LogError(text3, "", 0f);
						Debug.LogError(text3);
					}
					if (string.IsNullOrEmpty(text2))
					{
						goto IL_0109;
					}
					text2 = Data.Encrypt_Wrapped(text2, Data.PGameDataGet(oldVersionNumber, newVersionNumber));
					try
					{
						File.WriteAllText(text, text2);
						goto IL_0109;
					}
					catch (Exception ex2)
					{
						string text4 = "Failed to write back data while trying to encrypt it! Exception:\n" + ex2.Message;
						ConsolePrompt.LogError(text4, "", 0f);
						Debug.LogError(text4);
						goto IL_0109;
					}
				}
				string text5 = "Encryption of an old file is supported only for desktop!";
				Debug.LogError(text5);
				ConsolePrompt.LogError(text5, "", 0f);
			}
			IL_0109:
			if (oldVersionNumber < 3 && newVersionNumber >= 3)
			{
				Data.redButtonChange_ShowPopUps_ForV0_4 = true;
			}
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x000531FC File Offset: 0x000513FC
		private static void OnGameVersionChange_FirstPublisherBuildToSecond()
		{
			if (!PlatformMaster.PlatformIsComputer())
			{
				string text = "OnGameVersionChange_FirstPublisherBuildToSecond(): This function is only meant to be called on the computer platform!";
				Debug.LogError(text);
				ConsolePrompt.LogError(text, "", 0f);
				return;
			}
			string gameFolderPath = PlatformDataMaster.GameFolderPath;
			string text2 = PlatformDataMaster.PathGet_GameDataFile(0, "");
			if (!Directory.Exists(gameFolderPath))
			{
				return;
			}
			if (File.Exists(text2))
			{
				Data.publisherBuildFlag_FromFirstToSecond = true;
				string text3 = PlatformDataMaster.PathGet_GameDataFile(0, "_OLD_0.1.0");
				File.Move(text2, text3);
			}
		}

		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x00053265 File Offset: 0x00051465
		public static Data.SettingsData settings
		{
			get
			{
				return Data.SettingsData.inst;
			}
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0005326C File Offset: 0x0005146C
		public static void ApplySettings(bool applyResolution, bool pushControlsJsonToMap)
		{
			Data.settings.Apply(applyResolution, pushControlsJsonToMap);
		}

		// (get) Token: 0x06000CBD RID: 3261 RVA: 0x0005327A File Offset: 0x0005147A
		public static Data.GameData game
		{
			get
			{
				return Data.GameData.inst;
			}
		}

		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00053281 File Offset: 0x00051481
		public static int GameDataIndex
		{
			get
			{
				if (Data.GameData.inst == null)
				{
					return -1;
				}
				return Data.GameData.inst.myGameDataIndex;
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00053296 File Offset: 0x00051496
		public static bool JsonErrorWhileLoadingGame_Get()
		{
			return Data.errorFlag_JsonGameDataError;
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x000532A0 File Offset: 0x000514A0
		public static UniTask<bool> SaveSettings()
		{
			Data.<SaveSettings>d__32 <SaveSettings>d__;
			<SaveSettings>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<SaveSettings>d__.<>1__state = -1;
			<SaveSettings>d__.<>t__builder.Start<Data.<SaveSettings>d__32>(ref <SaveSettings>d__);
			return <SaveSettings>d__.<>t__builder.Task;
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x000532DC File Offset: 0x000514DC
		public static UniTask<bool> SaveSettingsAndApply(bool applyResolution)
		{
			Data.<SaveSettingsAndApply>d__33 <SaveSettingsAndApply>d__;
			<SaveSettingsAndApply>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<SaveSettingsAndApply>d__.applyResolution = applyResolution;
			<SaveSettingsAndApply>d__.<>1__state = -1;
			<SaveSettingsAndApply>d__.<>t__builder.Start<Data.<SaveSettingsAndApply>d__33>(ref <SaveSettingsAndApply>d__);
			return <SaveSettingsAndApply>d__.<>t__builder.Task;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00053320 File Offset: 0x00051520
		public static UniTask<bool> LoadSettingsAndApply()
		{
			Data.<LoadSettingsAndApply>d__34 <LoadSettingsAndApply>d__;
			<LoadSettingsAndApply>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<LoadSettingsAndApply>d__.<>1__state = -1;
			<LoadSettingsAndApply>d__.<>t__builder.Start<Data.<LoadSettingsAndApply>d__34>(ref <LoadSettingsAndApply>d__);
			return <LoadSettingsAndApply>d__.<>t__builder.Task;
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0005335C File Offset: 0x0005155C
		public static UniTask<bool> DeleteSettingsAndReset()
		{
			Data.<DeleteSettingsAndReset>d__35 <DeleteSettingsAndReset>d__;
			<DeleteSettingsAndReset>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<DeleteSettingsAndReset>d__.<>1__state = -1;
			<DeleteSettingsAndReset>d__.<>t__builder.Start<Data.<DeleteSettingsAndReset>d__35>(ref <DeleteSettingsAndReset>d__);
			return <DeleteSettingsAndReset>d__.<>t__builder.Task;
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x00053398 File Offset: 0x00051598
		public static UniTask<bool> SaveAchievements()
		{
			Data.<SaveAchievements>d__36 <SaveAchievements>d__;
			<SaveAchievements>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<SaveAchievements>d__.<>1__state = -1;
			<SaveAchievements>d__.<>t__builder.Start<Data.<SaveAchievements>d__36>(ref <SaveAchievements>d__);
			return <SaveAchievements>d__.<>t__builder.Task;
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x000533D4 File Offset: 0x000515D4
		public static UniTask<bool> LoadAchievements()
		{
			Data.<LoadAchievements>d__37 <LoadAchievements>d__;
			<LoadAchievements>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<LoadAchievements>d__.<>1__state = -1;
			<LoadAchievements>d__.<>t__builder.Start<Data.<LoadAchievements>d__37>(ref <LoadAchievements>d__);
			return <LoadAchievements>d__.<>t__builder.Task;
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00053410 File Offset: 0x00051610
		public static UniTask<bool> DeleteAchievements(string debugReason, bool forceInRelease)
		{
			Data.<DeleteAchievements>d__38 <DeleteAchievements>d__;
			<DeleteAchievements>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<DeleteAchievements>d__.debugReason = debugReason;
			<DeleteAchievements>d__.forceInRelease = forceInRelease;
			<DeleteAchievements>d__.<>1__state = -1;
			<DeleteAchievements>d__.<>t__builder.Start<Data.<DeleteAchievements>d__38>(ref <DeleteAchievements>d__);
			return <DeleteAchievements>d__.<>t__builder.Task;
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x0005345C File Offset: 0x0005165C
		public static UniTask<bool> SaveGame(Data.GameSavingReason reasonForSaving, int gameDataIndex = -1)
		{
			Data.<SaveGame>d__39 <SaveGame>d__;
			<SaveGame>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<SaveGame>d__.reasonForSaving = reasonForSaving;
			<SaveGame>d__.gameDataIndex = gameDataIndex;
			<SaveGame>d__.<>1__state = -1;
			<SaveGame>d__.<>t__builder.Start<Data.<SaveGame>d__39>(ref <SaveGame>d__);
			return <SaveGame>d__.<>t__builder.Task;
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x000534A8 File Offset: 0x000516A8
		public static UniTask<bool> LoadGame(int gameDataIndex, bool forceLoadSameFile)
		{
			Data.<LoadGame>d__40 <LoadGame>d__;
			<LoadGame>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<LoadGame>d__.gameDataIndex = gameDataIndex;
			<LoadGame>d__.forceLoadSameFile = forceLoadSameFile;
			<LoadGame>d__.<>1__state = -1;
			<LoadGame>d__.<>t__builder.Start<Data.<LoadGame>d__40>(ref <LoadGame>d__);
			return <LoadGame>d__.<>t__builder.Task;
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x000534F4 File Offset: 0x000516F4
		public static UniTask<bool> DeleteGameData(int gameDataIndex)
		{
			Data.<DeleteGameData>d__41 <DeleteGameData>d__;
			<DeleteGameData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<DeleteGameData>d__.gameDataIndex = gameDataIndex;
			<DeleteGameData>d__.<>1__state = -1;
			<DeleteGameData>d__.<>t__builder.Start<Data.<DeleteGameData>d__41>(ref <DeleteGameData>d__);
			return <DeleteGameData>d__.<>t__builder.Task;
		}

		public const int GAME_DATA_MAX_NUMBER = 1;

		public static Data.OnVersionChange OnDataVersionChage_Settings = new Data.OnVersionChange(Data.OnSettingsVersionChange);

		public static Data.OnVersionChange OnDataVersionChange_Controls = null;

		public static Data.OnVersionChange OnDataVersionChange_Game = new Data.OnVersionChange(Data.OnGameVersionChange);

		public static bool settingsResetFlag = false;

		public static bool publisherBuildFlag_FromFirstToSecond = false;

		public static bool redButtonChange_ShowPopUps_ForV0_4 = false;

		private static bool errorFlag_JsonGameDataError = false;

		[Serializable]
		public class VersionData
		{
			public static Data.VersionData inst = new Data.VersionData();

			public const int DESIRED_VERSION_SETTINGS = 4;

			public const int DESIRED_VERSION_CONTROLS = 0;

			public const int DESIRED_VERSION_GAME = 3;

			public int settingsVersion = 4;

			public int controlsVersion;

			public int gameVersion = 3;

			public static int settingsVersion_LoadedBackup = 4;

			public static int controlsVersion_LoadedBackup = 0;

			public static int gameVersion_LoadedBackup = 3;

			public static bool settingsVersionFixed = false;

			public static bool controlsVersionFixed = false;

			public static bool gameVersionFixed = false;
		}

		// (Invoke) Token: 0x060012D8 RID: 4824
		public delegate void OnVersionChange(int oldVersionNumber, int newVersionNumber);

		[Serializable]
		public class SettingsData
		{
			// (get) Token: 0x060012DB RID: 4827 RVA: 0x00078025 File Offset: 0x00076225
			public static bool FullscreenDefault
			{
				get
				{
					return PlatformMaster.PlatformSupports_FullscreenSwitching();
				}
			}

			// Token: 0x060012DC RID: 4828 RVA: 0x0007802C File Offset: 0x0007622C
			private void VerticalResolutionEnsure()
			{
				if (Data.SettingsData.supportedVerticalResolutions == null)
				{
					Data.SettingsData.supportedVerticalResolutions = PlatformMaster.PlatformSupportedVerticalResolutions();
				}
			}

			// Token: 0x060012DD RID: 4829 RVA: 0x00078040 File Offset: 0x00076240
			public void VerticalResolutionSet(Data.SettingsData.VerticalResolution vRes)
			{
				this.VerticalResolutionEnsure();
				bool flag = true;
				if (Data.SettingsData.supportedVerticalResolutions == null)
				{
					flag = false;
				}
				else if (Array.IndexOf<Data.SettingsData.VerticalResolution>(Data.SettingsData.supportedVerticalResolutions, vRes) == -1)
				{
					flag = false;
				}
				if (!flag)
				{
					Debug.LogError("Settings: Vertical Resolution not supported by platform.");
					return;
				}
				this.verticalResolution = vRes;
			}

			// Token: 0x060012DE RID: 4830 RVA: 0x00078088 File Offset: 0x00076288
			public void VerticalResolutionSetNext()
			{
				this.VerticalResolutionEnsure();
				if (Data.SettingsData.supportedVerticalResolutions == null)
				{
					Debug.LogError("VerticalResolutionSetNext(): Supported vertical resolutions array not set.");
					return;
				}
				int num = Array.IndexOf<Data.SettingsData.VerticalResolution>(Data.SettingsData.supportedVerticalResolutions, this.verticalResolution);
				if (num == -1)
				{
					Debug.LogError("VerticalResolutionSetNext(): Vertical resolution not found in supported resolutions array.");
					return;
				}
				num++;
				if (num >= Data.SettingsData.supportedVerticalResolutions.Length)
				{
					num = 0;
				}
				this.VerticalResolutionSet(Data.SettingsData.supportedVerticalResolutions[num]);
			}

			// Token: 0x060012DF RID: 4831 RVA: 0x000780EC File Offset: 0x000762EC
			public void VerticalResolutionSetPrevious()
			{
				this.VerticalResolutionEnsure();
				if (Data.SettingsData.supportedVerticalResolutions == null)
				{
					Debug.LogError("VerticalResolutionSetPrevious(): Supported vertical resolutions array not set.");
					return;
				}
				int num = Array.IndexOf<Data.SettingsData.VerticalResolution>(Data.SettingsData.supportedVerticalResolutions, this.verticalResolution);
				if (num == -1)
				{
					Debug.LogError("VerticalResolutionSetPrevious(): Vertical resolution not found in supported resolutions array.");
					return;
				}
				num--;
				if (num < 0)
				{
					num = Data.SettingsData.supportedVerticalResolutions.Length - 1;
				}
				this.VerticalResolutionSet(Data.SettingsData.supportedVerticalResolutions[num]);
			}

			// Token: 0x060012E0 RID: 4832 RVA: 0x00078150 File Offset: 0x00076350
			public Data.SettingsData.VerticalResolution VerticalResolutionGet()
			{
				return this.verticalResolution;
			}

			// Token: 0x060012E1 RID: 4833 RVA: 0x00078158 File Offset: 0x00076358
			public string VerticalResolutionGetDebugName()
			{
				switch (this.verticalResolution)
				{
				case Data.SettingsData.VerticalResolution._native:
					return "Vertical Resolution: Native";
				case Data.SettingsData.VerticalResolution._360p:
					return "Vertical Resolution: 360p";
				case Data.SettingsData.VerticalResolution._480p:
					return "Vertical Resolution: 480p";
				case Data.SettingsData.VerticalResolution._720p:
					return "Vertical Resolution: 720p";
				case Data.SettingsData.VerticalResolution._1080p:
					return "Vertical Resolution: 1080p";
				case Data.SettingsData.VerticalResolution._1440p:
					return "Vertical Resolution: 1440p";
				case Data.SettingsData.VerticalResolution._4k:
					return "Vertical Resolution: 4k";
				default:
					return "Undefined";
				}
			}

			// Token: 0x060012E2 RID: 4834 RVA: 0x000781C0 File Offset: 0x000763C0
			public int ResolutionDesiredHeightGet()
			{
				int num = Display.main.systemHeight;
				switch (this.verticalResolution)
				{
				case Data.SettingsData.VerticalResolution._native:
					num = Display.main.systemHeight;
					break;
				case Data.SettingsData.VerticalResolution._360p:
					num = 360;
					break;
				case Data.SettingsData.VerticalResolution._480p:
					num = 480;
					break;
				case Data.SettingsData.VerticalResolution._720p:
					num = 720;
					break;
				case Data.SettingsData.VerticalResolution._1080p:
					num = 1080;
					break;
				case Data.SettingsData.VerticalResolution._1440p:
					num = 1440;
					break;
				case Data.SettingsData.VerticalResolution._4k:
					num = 2160;
					break;
				default:
					num = Display.main.systemHeight;
					break;
				}
				return num;
			}

			// Token: 0x060012E3 RID: 4835 RVA: 0x0007824C File Offset: 0x0007644C
			public int ResolutionDesiredWidthGet()
			{
				int num = this.ResolutionDesiredHeightGet();
				switch (this.widthAspectRatio)
				{
				case Data.SettingsData.WidthAspectRatio._extend:
					return (int)((float)Display.main.systemWidth / (float)Display.main.systemHeight * (float)num);
				case Data.SettingsData.WidthAspectRatio._4_3:
					return (int)((float)num * 4f / 3f);
				case Data.SettingsData.WidthAspectRatio._3_4:
					return (int)((float)num * 3f / 4f);
				case Data.SettingsData.WidthAspectRatio._16_9:
					return (int)((float)num * 16f / 9f);
				case Data.SettingsData.WidthAspectRatio._9_16:
					return (int)((float)num * 9f / 16f);
				case Data.SettingsData.WidthAspectRatio._3_2:
					return (int)((float)num * 3f / 2f);
				case Data.SettingsData.WidthAspectRatio._2_3:
					return (int)((float)num * 2f / 3f);
				default:
					return (int)((float)Display.main.systemWidth / (float)Display.main.systemHeight * (float)num);
				}
			}

			// Token: 0x060012E4 RID: 4836 RVA: 0x00078322 File Offset: 0x00076522
			private void WidthAspectRatioEnsure()
			{
				if (Data.SettingsData.supportedWidthAspectRatios == null)
				{
					Data.SettingsData.supportedWidthAspectRatios = PlatformMaster.PlatformSupportedWidthAspectRatios();
				}
			}

			// Token: 0x060012E5 RID: 4837 RVA: 0x00078338 File Offset: 0x00076538
			public void WidthAspectRatioSet(Data.SettingsData.WidthAspectRatio aspectRatio)
			{
				this.WidthAspectRatioEnsure();
				bool flag = true;
				if (Data.SettingsData.supportedWidthAspectRatios == null)
				{
					flag = false;
				}
				else if (Array.IndexOf<Data.SettingsData.WidthAspectRatio>(Data.SettingsData.supportedWidthAspectRatios, aspectRatio) == -1)
				{
					flag = false;
				}
				if (!flag)
				{
					Debug.LogError("Settings: Aspect ratio not supported by platform.");
					return;
				}
				this.widthAspectRatio = aspectRatio;
			}

			// Token: 0x060012E6 RID: 4838 RVA: 0x00078380 File Offset: 0x00076580
			public void WidthAspectRatioSetNext()
			{
				this.WidthAspectRatioEnsure();
				if (Data.SettingsData.supportedWidthAspectRatios == null)
				{
					Debug.LogError("Settings: Aspect ratio array not set.");
					return;
				}
				int num = Array.IndexOf<Data.SettingsData.WidthAspectRatio>(Data.SettingsData.supportedWidthAspectRatios, this.widthAspectRatio);
				if (num == -1)
				{
					Debug.LogError("Settings: Aspect ratio not found in aspect ratios array.");
					return;
				}
				num++;
				if (num >= Data.SettingsData.supportedWidthAspectRatios.Length)
				{
					num = 0;
				}
				this.WidthAspectRatioSet(Data.SettingsData.supportedWidthAspectRatios[num]);
			}

			// Token: 0x060012E7 RID: 4839 RVA: 0x000783E4 File Offset: 0x000765E4
			public void WidthAspectRatioSetPrevious()
			{
				this.WidthAspectRatioEnsure();
				if (Data.SettingsData.supportedWidthAspectRatios == null)
				{
					Debug.LogError("Settings: Aspect ratio array not set.");
					return;
				}
				int num = Array.IndexOf<Data.SettingsData.WidthAspectRatio>(Data.SettingsData.supportedWidthAspectRatios, this.widthAspectRatio);
				if (num == -1)
				{
					Debug.LogError("Settings: Aspect ratio not found in aspect ratios array.");
					return;
				}
				num--;
				if (num < 0)
				{
					num = Data.SettingsData.supportedWidthAspectRatios.Length - 1;
				}
				this.WidthAspectRatioSet(Data.SettingsData.supportedWidthAspectRatios[num]);
			}

			// Token: 0x060012E8 RID: 4840 RVA: 0x00078448 File Offset: 0x00076648
			public Data.SettingsData.WidthAspectRatio WidthAspectRatioGet()
			{
				return this.widthAspectRatio;
			}

			// Token: 0x060012E9 RID: 4841 RVA: 0x00078450 File Offset: 0x00076650
			public string WidthAspectRatioGetDebugName()
			{
				switch (this.widthAspectRatio)
				{
				case Data.SettingsData.WidthAspectRatio._extend:
					return "Width Aspect Ratio: Extend";
				case Data.SettingsData.WidthAspectRatio._4_3:
					return "Width Aspect Ratio: 4:3";
				case Data.SettingsData.WidthAspectRatio._3_4:
					return "Width Aspect Ratio: 3:4";
				case Data.SettingsData.WidthAspectRatio._16_9:
					return "Width Aspect Ratio: 16:9";
				case Data.SettingsData.WidthAspectRatio._9_16:
					return "Width Aspect Ratio: 9:16";
				case Data.SettingsData.WidthAspectRatio._3_2:
					return "Width Aspect Ratio: 3:2";
				case Data.SettingsData.WidthAspectRatio._2_3:
					return "Width Aspect Ratio: 2:3";
				default:
					return "Undefined";
				}
			}

			// (get) Token: 0x060012EA RID: 4842 RVA: 0x000784B7 File Offset: 0x000766B7
			public static int QUALITY_DEFAULT
			{
				get
				{
					return PlatformMaster.QualityDefaultGet();
				}
			}

			// Token: 0x060012EB RID: 4843 RVA: 0x000784C0 File Offset: 0x000766C0
			private void FovEnsure()
			{
				if (this._fieldOfViews != null && this._fieldOfViews.Length == 1)
				{
					return;
				}
				this._fieldOfViews = new float[1];
				for (int i = 0; i < 1; i++)
				{
					this._fieldOfViews[i] = 60f;
				}
			}

			// Token: 0x060012EC RID: 4844 RVA: 0x00078506 File Offset: 0x00076706
			public void FovSet(int playerIndex, float fov)
			{
				this.FovEnsure();
				this._fieldOfViews[playerIndex] = fov;
				this._fieldOfViews[playerIndex] = Mathf.Clamp(this._fieldOfViews[playerIndex], 30f, 180f);
			}

			// Token: 0x060012ED RID: 4845 RVA: 0x00078536 File Offset: 0x00076736
			public float FovGet(int playerIndex)
			{
				this.FovEnsure();
				return this._fieldOfViews[playerIndex];
			}

			// Token: 0x060012EE RID: 4846 RVA: 0x00078546 File Offset: 0x00076746
			public void FovAdd(int playerIndex, float fov)
			{
				this.FovEnsure();
				this._fieldOfViews[playerIndex] += fov;
				this._fieldOfViews[playerIndex] = Mathf.Clamp(this._fieldOfViews[playerIndex], 30f, 180f);
			}

			// Token: 0x060012EF RID: 4847 RVA: 0x0007857E File Offset: 0x0007677E
			public void FovReset(int playerIndex)
			{
				this.FovEnsure();
				this._fieldOfViews[playerIndex] = 60f;
			}

			// Token: 0x060012F0 RID: 4848 RVA: 0x00078594 File Offset: 0x00076794
			public void FovResetAll()
			{
				this.FovEnsure();
				for (int i = 0; i < 1; i++)
				{
					this._fieldOfViews[i] = 60f;
				}
			}

			// Token: 0x060012F1 RID: 4849 RVA: 0x000785C0 File Offset: 0x000767C0
			public Data.SettingsData.KeyboardLayout KeyboardLayoutGet()
			{
				return this.keyboardLayout;
			}

			// Token: 0x060012F2 RID: 4850 RVA: 0x000785C8 File Offset: 0x000767C8
			public void KeyboardLayoutSet(Data.SettingsData.KeyboardLayout layout)
			{
				this.keyboardLayout = layout;
			}

			// Token: 0x060012F3 RID: 4851 RVA: 0x000785D4 File Offset: 0x000767D4
			public void KeyboardLayourNext()
			{
				int num = 4;
				int num2 = (int)this.keyboardLayout;
				num2++;
				if (num2 >= num)
				{
					num2 = 0;
				}
				this.KeyboardLayoutSet((Data.SettingsData.KeyboardLayout)num2);
			}

			// Token: 0x060012F4 RID: 4852 RVA: 0x000785FC File Offset: 0x000767FC
			public void KeyboardLayourPrevious()
			{
				int num = 4;
				int num2 = (int)this.keyboardLayout;
				num2--;
				if (num2 < 0)
				{
					num2 = num - 1;
				}
				this.KeyboardLayoutSet((Data.SettingsData.KeyboardLayout)num2);
			}

			// Token: 0x060012F5 RID: 4853 RVA: 0x00078628 File Offset: 0x00076828
			private void JoystickVibrationEnsure()
			{
				if (this.joystickVibrationEnabledPerPlayer != null && this.joystickVibrationEnabledPerPlayer.Length == 1)
				{
					return;
				}
				this.joystickVibrationEnabledPerPlayer = new bool[1];
				for (int i = 0; i < 1; i++)
				{
					this.joystickVibrationEnabledPerPlayer[i] = true;
				}
			}

			// Token: 0x060012F6 RID: 4854 RVA: 0x0007866A File Offset: 0x0007686A
			public void JoystickVibrationEnabledSet(int playerIndex, bool enabled)
			{
				this.JoystickVibrationEnsure();
				this.joystickVibrationEnabledPerPlayer[playerIndex] = enabled;
			}

			// Token: 0x060012F7 RID: 4855 RVA: 0x0007867B File Offset: 0x0007687B
			public bool JoystickVibrationEnabledGet(int playerIndex)
			{
				this.JoystickVibrationEnsure();
				return this.joystickVibrationEnabledPerPlayer[playerIndex];
			}

			// Token: 0x060012F8 RID: 4856 RVA: 0x0007868B File Offset: 0x0007688B
			public void JoystickVibrationEnabledToggle(int playerIndex)
			{
				this.JoystickVibrationEnsure();
				this.joystickVibrationEnabledPerPlayer[playerIndex] = !this.joystickVibrationEnabledPerPlayer[playerIndex];
			}

			// Token: 0x060012F9 RID: 4857 RVA: 0x000786A8 File Offset: 0x000768A8
			public void JoystickVibrationEnabledSetAll(bool enabled)
			{
				this.JoystickVibrationEnsure();
				for (int i = 0; i < 1; i++)
				{
					this.joystickVibrationEnabledPerPlayer[i] = enabled;
				}
			}

			// Token: 0x060012FA RID: 4858 RVA: 0x000786D0 File Offset: 0x000768D0
			private void CameraSensitivityEnsure()
			{
				if (this.cameraSensitivity != null && this.cameraSensitivity.Length == 1)
				{
					return;
				}
				this.cameraSensitivity = new Vector2[1];
				for (int i = 0; i < 1; i++)
				{
					this.cameraSensitivity[i] = new Vector2(1f, 1f);
				}
			}

			// Token: 0x060012FB RID: 4859 RVA: 0x00078724 File Offset: 0x00076924
			public Vector2 CameraSensitivityGet(int playerIndex)
			{
				this.CameraSensitivityEnsure();
				return this.cameraSensitivity[playerIndex];
			}

			// Token: 0x060012FC RID: 4860 RVA: 0x00078738 File Offset: 0x00076938
			public void CameraSensitivitySet(int playerIndex, Vector2 sensitivity)
			{
				this.CameraSensitivityEnsure();
				this.cameraSensitivity[playerIndex] = sensitivity;
				this.cameraSensitivity[playerIndex].x = Mathf.Clamp(this.cameraSensitivity[playerIndex].x, 0.1f, 10f);
				this.cameraSensitivity[playerIndex].y = Mathf.Clamp(this.cameraSensitivity[playerIndex].y, 0.1f, 10f);
			}

			// Token: 0x060012FD RID: 4861 RVA: 0x000787BC File Offset: 0x000769BC
			public void CameraSensitivityAdd(int playerIndex, Vector2 sensitivity)
			{
				this.CameraSensitivityEnsure();
				this.cameraSensitivity[playerIndex] += sensitivity;
				this.cameraSensitivity[playerIndex].x = Mathf.Clamp(this.cameraSensitivity[playerIndex].x, 0.1f, 10f);
				this.cameraSensitivity[playerIndex].y = Mathf.Clamp(this.cameraSensitivity[playerIndex].y, 0.1f, 10f);
			}

			// Token: 0x060012FE RID: 4862 RVA: 0x0007884E File Offset: 0x00076A4E
			public void CameraSensitivityReset(int playerIndex)
			{
				this.CameraSensitivityEnsure();
				this.cameraSensitivity[playerIndex] = new Vector2(1f, 1f);
			}

			// Token: 0x060012FF RID: 4863 RVA: 0x00078874 File Offset: 0x00076A74
			public void CameraSensitivityResetAll()
			{
				this.CameraSensitivityEnsure();
				for (int i = 0; i < 1; i++)
				{
					this.cameraSensitivity[i] = new Vector2(1f, 1f);
				}
			}

			// Token: 0x06001300 RID: 4864 RVA: 0x000788B0 File Offset: 0x00076AB0
			private void ControlsInversionEnsure()
			{
				if (this.controlsInvertMovementX != null && this.controlsInvertMovementX.Length == 1)
				{
					return;
				}
				this.controlsInvertMovementX = new bool[1];
				this.controlsInvertMovementY = new bool[1];
				this.controlsInvertCameraX = new bool[1];
				this.controlsInvertCameraY = new bool[1];
				this.controlsInvertScrollingX = new bool[1];
				this.controlsInvertScrollingY = new bool[1];
				for (int i = 0; i < 1; i++)
				{
					this.controlsInvertMovementX[i] = false;
					this.controlsInvertMovementY[i] = false;
					this.controlsInvertCameraX[i] = false;
					this.controlsInvertCameraY[i] = false;
					this.controlsInvertScrollingX[i] = false;
					this.controlsInvertScrollingY[i] = false;
				}
			}

			// Token: 0x06001301 RID: 4865 RVA: 0x0007895B File Offset: 0x00076B5B
			public void ControlsInvertMovementXSet(int playerIndex, bool invert)
			{
				this.ControlsInversionEnsure();
				this.controlsInvertMovementX[playerIndex] = invert;
			}

			// Token: 0x06001302 RID: 4866 RVA: 0x0007896C File Offset: 0x00076B6C
			public bool ControlsInvertMovementXGet(int playerIndex)
			{
				this.ControlsInversionEnsure();
				return this.controlsInvertMovementX[playerIndex];
			}

			// Token: 0x06001303 RID: 4867 RVA: 0x0007897C File Offset: 0x00076B7C
			public void ControlsInvertMovementYSet(int playerIndex, bool invert)
			{
				this.ControlsInversionEnsure();
				this.controlsInvertMovementY[playerIndex] = invert;
			}

			// Token: 0x06001304 RID: 4868 RVA: 0x0007898D File Offset: 0x00076B8D
			public bool ControlsInvertMovementYGet(int playerIndex)
			{
				this.ControlsInversionEnsure();
				return this.controlsInvertMovementY[playerIndex];
			}

			// Token: 0x06001305 RID: 4869 RVA: 0x0007899D File Offset: 0x00076B9D
			public void ControlsInvertCameraXSet(int playerIndex, bool invert)
			{
				this.ControlsInversionEnsure();
				this.controlsInvertCameraX[playerIndex] = invert;
			}

			// Token: 0x06001306 RID: 4870 RVA: 0x000789AE File Offset: 0x00076BAE
			public bool ControlsInvertCameraXGet(int playerIndex)
			{
				this.ControlsInversionEnsure();
				return this.controlsInvertCameraX[playerIndex];
			}

			// Token: 0x06001307 RID: 4871 RVA: 0x000789BE File Offset: 0x00076BBE
			public void ControlsInvertCameraYSet(int playerIndex, bool invert)
			{
				this.ControlsInversionEnsure();
				this.controlsInvertCameraY[playerIndex] = invert;
			}

			// Token: 0x06001308 RID: 4872 RVA: 0x000789CF File Offset: 0x00076BCF
			public bool ControlsInvertCameraYGet(int playerIndex)
			{
				this.ControlsInversionEnsure();
				return this.controlsInvertCameraY[playerIndex];
			}

			// Token: 0x06001309 RID: 4873 RVA: 0x000789DF File Offset: 0x00076BDF
			public void ControlsInvertScrollingXSet(int playerIndex, bool invert)
			{
				this.ControlsInversionEnsure();
				this.controlsInvertScrollingX[playerIndex] = invert;
			}

			// Token: 0x0600130A RID: 4874 RVA: 0x000789F0 File Offset: 0x00076BF0
			public bool ControlsInvertScrollingXGet(int playerIndex)
			{
				this.ControlsInversionEnsure();
				return this.controlsInvertScrollingX[playerIndex];
			}

			// Token: 0x0600130B RID: 4875 RVA: 0x00078A00 File Offset: 0x00076C00
			public void ControlsInvertScrollingYSet(int playerIndex, bool invert)
			{
				this.ControlsInversionEnsure();
				this.controlsInvertScrollingY[playerIndex] = invert;
			}

			// Token: 0x0600130C RID: 4876 RVA: 0x00078A11 File Offset: 0x00076C11
			public bool ControlsInvertScrollingYGet(int playerIndex)
			{
				this.ControlsInversionEnsure();
				return this.controlsInvertScrollingY[playerIndex];
			}

			// Token: 0x0600130D RID: 4877 RVA: 0x00078A24 File Offset: 0x00076C24
			public void ControlsInvertMovementAll()
			{
				this.ControlsInversionEnsure();
				for (int i = 0; i < 1; i++)
				{
					this.controlsInvertMovementX[i] = false;
					this.controlsInvertMovementY[i] = false;
				}
			}

			// Token: 0x0600130E RID: 4878 RVA: 0x00078A58 File Offset: 0x00076C58
			public void ControlsInvertCameraAll()
			{
				this.ControlsInversionEnsure();
				for (int i = 0; i < 1; i++)
				{
					this.controlsInvertCameraX[i] = false;
					this.controlsInvertCameraY[i] = false;
				}
			}

			// Token: 0x0600130F RID: 4879 RVA: 0x00078A8C File Offset: 0x00076C8C
			public void ControlsInvertScrollingAll()
			{
				this.ControlsInversionEnsure();
				for (int i = 0; i < 1; i++)
				{
					this.controlsInvertScrollingX[i] = false;
					this.controlsInvertScrollingY[i] = false;
				}
			}

			// Token: 0x06001310 RID: 4880 RVA: 0x00078AC0 File Offset: 0x00076CC0
			private void VirtualCursorSensitivityEnsure()
			{
				if (this._virtualCursorSensitivity != null && this._virtualCursorSensitivity.Length == 1)
				{
					return;
				}
				this._virtualCursorSensitivity = new float[1];
				for (int i = 0; i < 1; i++)
				{
					this._virtualCursorSensitivity[i] = 1f;
				}
			}

			// Token: 0x06001311 RID: 4881 RVA: 0x00078B06 File Offset: 0x00076D06
			public void VirtualCursorSensitivitySet(int playerIndex, float sensitivity)
			{
				this.VirtualCursorSensitivityEnsure();
				this._virtualCursorSensitivity[playerIndex] = sensitivity;
				this._virtualCursorSensitivity[playerIndex] = Mathf.Clamp(this._virtualCursorSensitivity[playerIndex], 0.1f, 10f);
			}

			// Token: 0x06001312 RID: 4882 RVA: 0x00078B36 File Offset: 0x00076D36
			public float VirtualCursorSensitivityGet(int playerIndex)
			{
				this.VirtualCursorSensitivityEnsure();
				return this._virtualCursorSensitivity[playerIndex];
			}

			// Token: 0x06001313 RID: 4883 RVA: 0x00078B46 File Offset: 0x00076D46
			public void VirtualCursorSensitivityAdd(int playerIndex, float sensitivity)
			{
				this.VirtualCursorSensitivityEnsure();
				this._virtualCursorSensitivity[playerIndex] += sensitivity;
				this._virtualCursorSensitivity[playerIndex] = Mathf.Clamp(this._virtualCursorSensitivity[playerIndex], 0.1f, 10f);
			}

			// Token: 0x06001314 RID: 4884 RVA: 0x00078B7E File Offset: 0x00076D7E
			public void VirtualCursorSensitivityReset(int playerIndex)
			{
				this.VirtualCursorSensitivityEnsure();
				this._virtualCursorSensitivity[playerIndex] = 1f;
			}

			// Token: 0x06001315 RID: 4885 RVA: 0x00078B94 File Offset: 0x00076D94
			public void VirtualCursorSensitivityResetAll()
			{
				this.VirtualCursorSensitivityEnsure();
				for (int i = 0; i < 1; i++)
				{
					this._virtualCursorSensitivity[i] = 1f;
				}
			}

			// Token: 0x06001316 RID: 4886 RVA: 0x00078BC0 File Offset: 0x00076DC0
			public static float TransitionSpeedMapped_Get(float from, float fromMin, float fromMax, float toMin, float toMax)
			{
				float num = from - fromMin;
				float num2 = fromMax - fromMin;
				float num3 = num / num2;
				return (toMax - toMin) * num3 + toMin;
			}

			// Token: 0x06001317 RID: 4887 RVA: 0x00078BDF File Offset: 0x00076DDF
			public void _PrepareForSaving()
			{
				this.VerticalResolutionEnsure();
				this.WidthAspectRatioEnsure();
				this.JoystickVibrationEnsure();
				this.CameraSensitivityEnsure();
				this.ControlsInversionEnsure();
				this.VirtualCursorSensitivityEnsure();
				this.controlMapsJson = Controls.SaveMapsToJson();
			}

			// Token: 0x06001318 RID: 4888 RVA: 0x00078C10 File Offset: 0x00076E10
			public void Apply(bool applyResolution, bool pushControlsJsonToMap)
			{
				if (this.colorblindModeEnabled)
				{
					Debug.LogWarning("Colorblind mode setting is not implemented");
				}
				CameraGame.UpdatePSXEffectsToSettings_All();
				bool flag = false;
				if (PlatformMaster.PlatformResolutionCanChange() && this.fullscreenEnabled != Screen.fullScreen)
				{
					flag = true;
				}
				if (!PlatformMaster.PlatformSupports_FullscreenSwitching())
				{
					Debug.LogWarning("Settings: Fullscreen switching is not supported by the platform. Resetting settings var to current fullscreen state.");
					this.fullscreenEnabled = Screen.fullScreen;
				}
				QualitySettings.vSyncCount = (this.vsyncEnabled ? 1 : 0);
				QualitySettings.SetQualityLevel(this.qualityLevel, true);
				if (this.tateMode != Data.SettingsData.TateMode.none && !Master.instance.RENDER_TO_TEXTURE)
				{
					Debug.LogWarning("Settings: Tate mode is not supported in render to texture mode. Disabling tate mode.");
					this.tateMode = Data.SettingsData.TateMode.none;
				}
				foreach (CameraGame cameraGame in CameraGame.list)
				{
					cameraGame.FieldOfViewDefaultUpdate();
				}
				if (applyResolution)
				{
					RenderingMaster.RenderingRefresh(flag);
				}
				if (pushControlsJsonToMap)
				{
					Controls.LoadMapsFromJson(this.controlMapsJson, true);
				}
				Translation.LanguageSet(this.language);
			}

			public static Data.SettingsData inst = new Data.SettingsData();

			public bool dyslexicFontEnabled;

			public bool colorblindModeEnabled;

			public bool flashingLightsReducedEnabled;

			public bool wobblyPolygons = true;

			public bool fullscreenEnabled = Data.SettingsData.FullscreenDefault;

			public const int PIXELS_PER_UNIT = 32;

			public const Data.SettingsData.VerticalResolution VERTICAL_RESOLUTION_DEFAULT = Data.SettingsData.VerticalResolution._native;

			public Data.SettingsData.VerticalResolution verticalResolution;

			public static Data.SettingsData.VerticalResolution[] supportedVerticalResolutions = null;

			public const Data.SettingsData.WidthAspectRatio WIDTH_ASPECT_RATIO_DEFAULT = Data.SettingsData.WidthAspectRatio._16_9;

			public Data.SettingsData.WidthAspectRatio widthAspectRatio = Data.SettingsData.WidthAspectRatio._16_9;

			public static Data.SettingsData.WidthAspectRatio[] supportedWidthAspectRatios = null;

			public const Data.SettingsData.CrtFilter CRT_FILTER_DEFAULT = Data.SettingsData.CrtFilter.none;

			public Data.SettingsData.CrtFilter crtFilter;

			public const bool CHROMATIC_ABERRATION_DEFAULT = true;

			public bool chromaticAberrationEnabled = true;

			public const bool BLOOM_DEFAULT = true;

			public bool bloomEnabled = true;

			public const bool MOTION_BLUR_DEFAULT = false;

			public bool motionBlurEnabled;

			public const bool VSYNC_DEFAULT = true;

			public bool vsyncEnabled = true;

			public const bool SCREENSHAKE_DEFAULT = true;

			public bool screenshakeEnabled = true;

			public int qualityLevel = -1;

			public const Data.SettingsData.TateMode TATE_MODE_DEFAULT = Data.SettingsData.TateMode.none;

			public Data.SettingsData.TateMode tateMode;

			public const float FOV_DEFAULT = 60f;

			public const float FOV_MIN = 30f;

			public const float FOV_MAX = 180f;

			[SerializeField]
			private float[] _fieldOfViews;

			public const Data.SettingsData.SplitScreenKind CAMERA_SPLIT_SCREEN_KIND_DEFAULT = Data.SettingsData.SplitScreenKind.horizontal;

			public Data.SettingsData.SplitScreenKind cameraSplitScreenKind;

			public const float AUDIO_MASTER_VOLUME_DEFAULT = 1f;

			public float volumeMaster = 1f;

			public const float AUDIO_MUSIC_VOLUME_DEFAULT = 0.5f;

			public float volumeMusic = 0.5f;

			public const float AUDIO_SFX_VOLUME_DEFAULT = 0.5f;

			public float volumeSound = 0.5f;

			public const float AUDIO_FAN_VOLUME_DEFAULT = 1f;

			public float fanVolume = 1f;

			public const Translation.Language LANGUAGE_DEFAULT = Translation.Language.English;

			public Translation.Language language;

			public bool initialLanguageSelectionPerfromed;

			[SerializeField]
			private Data.SettingsData.KeyboardLayout keyboardLayout;

			public const bool JOYSTICK_VIBRATION_DEFAULT = true;

			[SerializeField]
			private bool[] joystickVibrationEnabledPerPlayer;

			public const float CAMERA_SENSITIVITY_X_DEFAULT = 1f;

			public const float CAMERA_SENSITIVITY_Y_DEFAULT = 1f;

			public const float CAMERA_SENSITIVITY_LIMIT_MIN = 0.1f;

			public const float CAMERA_SENSITIVITY_LIMIT_MAX = 10f;

			[SerializeField]
			private Vector2[] cameraSensitivity;

			public const bool CONTROLS_INVERT_MOVEMENT_X_DEFAULT = false;

			public const bool CONTROLS_INVERT_MOVEMENT_Y_DEFAULT = false;

			public const bool CONTROLS_INVERT_CAMERA_X_DEFAULT = false;

			public const bool CONTROLS_INVERT_CAMERA_Y_DEFAULT = false;

			public const bool CONTROLS_INVERT_SCROLLING_X_DEFAULT = false;

			public const bool CONTROLS_INVERT_SCROLLING_Y_DEFAULT = false;

			[SerializeField]
			private bool[] controlsInvertMovementX;

			[SerializeField]
			private bool[] controlsInvertMovementY;

			[SerializeField]
			private bool[] controlsInvertCameraX;

			[SerializeField]
			private bool[] controlsInvertCameraY;

			[SerializeField]
			private bool[] controlsInvertScrollingX;

			[SerializeField]
			private bool[] controlsInvertScrollingY;

			public const float VIRTUAL_CURSOR_SENSITIVITY_DEFAULT = 1f;

			public const float VIRTUAL_CURSOR_SENSITIVITY_LIMIT_MIN = 0.1f;

			public const float VIRTUAL_CURSOR_SENSITIVITY_LIMIT_MAX = 10f;

			[SerializeField]
			private float[] _virtualCursorSensitivity;

			public string controlMapsJson;

			public int transitionSpeed = 1;

			public enum VerticalResolution
			{
				_native,
				_360p,
				_480p,
				_720p,
				_1080p,
				_1440p,
				_4k
			}

			public enum WidthAspectRatio
			{
				_extend,
				_4_3,
				_3_4,
				_16_9,
				_9_16,
				_3_2,
				_2_3
			}

			public enum CrtFilter
			{
				none,
				border,
				scanlines,
				full,
				_count
			}

			public enum TateMode
			{
				none,
				horizontalLeft,
				horizontalRight,
				upsideDown,
				_count
			}

			public enum SplitScreenKind
			{
				horizontal,
				vertical
			}

			public enum KeyboardLayout
			{
				keyboard_QWERTY,
				keyboard_AZERTY,
				keyboard_DVORAK,
				keyboard_COLEMAK,
				count
			}
		}

		public enum GameSavingReason
		{
			_undefined = -1,
			debug,
			setFlag_RunForceReset,
			newGame,
			introFinished,
			mainMenuOpened_NotDuringSlotMachine,
			death,
			endingWithoutDeath,
			beginOfPlayingAtTheSlotMachine,
			endOfRound_AfterInterestsAndTicketsCutscene,
			endOfDeadline_AfterCutscene,
			rewardBox_Opened,
			rewardBox_PickedUpReward,
			powerupUnlock,
			storeBuyOrReroll,
			phoneReroll,
			phoneSaveTime,
			_count
		}

		[Serializable]
		public class GameData
		{
			// Token: 0x0600131B RID: 4891 RVA: 0x00078DAC File Offset: 0x00076FAC
			public GameData(int gameDataIndex)
			{
				this.myGameDataIndex = gameDataIndex;
			}

			// Token: 0x0600131C RID: 4892 RVA: 0x00078E30 File Offset: 0x00077030
			public void Saving_Prepare(bool saveGameplayData, Data.GameSavingReason reason)
			{
				this.lastGameVersionThatSavedMe = Application.version;
				bool flag = true;
				if (reason == Data.GameSavingReason.setFlag_RunForceReset)
				{
					flag = false;
				}
				if (flag)
				{
					for (int i = 0; i < 4; i++)
					{
						this.drawersUnlocked[i] = DrawersScript.IsDrawerUnlocked(i);
					}
				}
				if (saveGameplayData)
				{
					this.gameplayDataHasSession = true;
					this.gameplayData.Save_Prepare();
				}
				this._LockedPowerups_SavePrepare();
				this._RunModifiers_SavePrepare();
			}

			// Token: 0x0600131D RID: 4893 RVA: 0x00078E8E File Offset: 0x0007708E
			public void Loading_Prepare()
			{
				this.gameplayData.Load_Format();
				this._LockedPowerups_LoadPrepare();
				this._RunModifiers_LoadPrepare();
			}

			// Token: 0x0600131E RID: 4894 RVA: 0x00078EA8 File Offset: 0x000770A8
			public void GameplayDataReset(bool transformSkeletonPieces)
			{
				bool[] array = new bool[4];
				for (int i = 0; i < 4; i++)
				{
					array[i] = false;
					if (DrawersScript.IsDrawerUnlocked(i) && !(PowerupScript.array_InDrawer[i] == null))
					{
						array[i] = true;
					}
				}
				this.gameplayDataHasSession = false;
				this.gameplayData = new GameplayData();
				if (transformSkeletonPieces)
				{
					int num = 0;
					for (int j = 0; j < 4; j++)
					{
						if (array[j])
						{
							switch (num)
							{
							case 0:
								this.gameplayData.drawerPowerups[j] = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.Skeleton_Arm1);
								break;
							case 1:
								this.gameplayData.drawerPowerups[j] = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.Skeleton_Leg1);
								break;
							case 2:
								this.gameplayData.drawerPowerups[j] = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.Skeleton_Arm2);
								break;
							case 3:
								this.gameplayData.drawerPowerups[j] = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.Skeleton_Leg2);
								break;
							default:
								Debug.LogError("GamePlay Data: Skeleton case not handled!");
								break;
							}
							num++;
						}
					}
				}
			}

			// Token: 0x0600131F RID: 4895 RVA: 0x00078F92 File Offset: 0x00077192
			public bool GameplayDataIsEmpty()
			{
				return !this.gameplayDataHasSession;
			}

			// Token: 0x06001320 RID: 4896 RVA: 0x00078FA0 File Offset: 0x000771A0
			public static bool IsGameCompletedFully()
			{
				return Data.game != null && Data.game.doorOpenedCounter > 0 && Data.game.badEndingCounter > 0 && Data.game.goodEndingCounter > 0 && Data.game.PowerupRealInstances_AreAllUnlocked() && Data.game.AllCardsUnlocked && Data.game.AllCardsHolographic;
			}

			// Token: 0x06001321 RID: 4897 RVA: 0x00079010 File Offset: 0x00077210
			public static int GameCompletitionPercentage_Get()
			{
				int num = 0;
				if (Data.game == null)
				{
					return num;
				}
				if (Data.game.badEndingCounter > 0)
				{
					num++;
				}
				if (Data.game.goodEndingCounter > 0)
				{
					num++;
				}
				num += Data.game.RunModifier_WonOnce_TotalNumber();
				num += Data.game.RunModifier_InHolographicCondition_TotalNumber();
				num += DrawersScript.GetDrawersUnlockedNum();
				int count = PowerupScript.all.Count;
				int num2 = 0;
				List<PowerupScript.Identifier> list = Data.game.LockedPowerups_GetList();
				for (int i = 0; i < count; i++)
				{
					PowerupScript powerupScript = PowerupScript.all[i];
					if (!(powerupScript == null) && powerupScript.identifier != PowerupScript.Identifier.undefined && powerupScript.identifier != PowerupScript.Identifier.count && !list.Contains(powerupScript.identifier))
					{
						num2++;
					}
				}
				return num + Mathf.FloorToInt((float)num2 / (float)count * 54f);
			}

			// (get) Token: 0x06001322 RID: 4898 RVA: 0x000790EB File Offset: 0x000772EB
			// (set) Token: 0x06001323 RID: 4899 RVA: 0x000790F3 File Offset: 0x000772F3
			public int PersistentStat_666SeenTimes
			{
				get
				{
					return this.persistentStat_666SeenTimes;
				}
				set
				{
					this.persistentStat_666SeenTimes = value;
				}
			}

			// Token: 0x06001324 RID: 4900 RVA: 0x000790FC File Offset: 0x000772FC
			public static List<PowerupScript.Identifier> _UnlockedPowerups_Definition()
			{
				return new List<PowerupScript.Identifier> { PowerupScript.Identifier.undefined };
			}

			// Token: 0x06001325 RID: 4901 RVA: 0x0007910C File Offset: 0x0007730C
			public static List<PowerupScript.Identifier> _LockedPowerups_Definition()
			{
				return new List<PowerupScript.Identifier>
				{
					PowerupScript.Identifier.Skeleton_Arm1,
					PowerupScript.Identifier.Skeleton_Arm2,
					PowerupScript.Identifier.Skeleton_Leg1,
					PowerupScript.Identifier.Skeleton_Leg2,
					PowerupScript.Identifier.Skeleton_Head,
					PowerupScript.Identifier.OneTrickPony,
					PowerupScript.Identifier.Ankh,
					PowerupScript.Identifier.FruitBasket,
					PowerupScript.Identifier.CloverBell,
					PowerupScript.Identifier.Necklace,
					PowerupScript.Identifier.SevenSinsStone,
					PowerupScript.Identifier.RedCrystal,
					PowerupScript.Identifier.Hole_Circle,
					PowerupScript.Identifier.Hole_Romboid,
					PowerupScript.Identifier.Hole_Cross,
					PowerupScript.Identifier.PissJar,
					PowerupScript.Identifier.PoopJar,
					PowerupScript.Identifier.Button2X,
					PowerupScript.Identifier.SuperCapacitor,
					PowerupScript.Identifier.CarBattery,
					PowerupScript.Identifier.CardboardHouse,
					PowerupScript.Identifier.Cigarettes,
					PowerupScript.Identifier.ElectricityCounter,
					PowerupScript.Identifier.PotatoPower,
					PowerupScript.Identifier.Necronomicon,
					PowerupScript.Identifier.HornDevil,
					PowerupScript.Identifier.CrankGenerator,
					PowerupScript.Identifier.Baphomet,
					PowerupScript.Identifier.GeneralModCharm_Clicker,
					PowerupScript.Identifier.GeneralModCharm_CloverBellBattery,
					PowerupScript.Identifier.GeneralModCharm_CrystalSphere,
					PowerupScript.Identifier.GoldenKingMida,
					PowerupScript.Identifier.Boardgame_C_Dealer,
					PowerupScript.Identifier.Boardgame_M_Capitalist,
					PowerupScript.Identifier.PuppetPersonalTrainer,
					PowerupScript.Identifier.PuppetElectrician,
					PowerupScript.Identifier.PuppetFortuneTeller,
					PowerupScript.Identifier.Boardgame_C_Bricks,
					PowerupScript.Identifier.Boardgame_C_Harbor,
					PowerupScript.Identifier.Boardgame_C_Sheep,
					PowerupScript.Identifier.Boardgame_C_Stone,
					PowerupScript.Identifier.Boardgame_C_Thief,
					PowerupScript.Identifier.Boardgame_C_Wheat,
					PowerupScript.Identifier.Boardgame_C_Wood,
					PowerupScript.Identifier.Boardgame_M_Car,
					PowerupScript.Identifier.Boardgame_M_Carriola,
					PowerupScript.Identifier.Boardgame_M_Ditale,
					PowerupScript.Identifier.Boardgame_M_FerroDaStiro,
					PowerupScript.Identifier.Boardgame_M_Hat,
					PowerupScript.Identifier.Boardgame_M_Ship,
					PowerupScript.Identifier.Boardgame_M_Shoe,
					PowerupScript.Identifier.Dice_4,
					PowerupScript.Identifier.Dice_6,
					PowerupScript.Identifier.Dice_20,
					PowerupScript.Identifier.GoldenPepper,
					PowerupScript.Identifier.RottenPepper,
					PowerupScript.Identifier.BellPepper,
					PowerupScript.Identifier.HorseShoeGold,
					PowerupScript.Identifier.LuckyCatFat,
					PowerupScript.Identifier.LuckyCatSwole,
					PowerupScript.Identifier.PoopBeetle,
					PowerupScript.Identifier.Baphomet,
					PowerupScript.Identifier.HouseContract,
					PowerupScript.Identifier.DearDiary,
					PowerupScript.Identifier.PhotoBook,
					PowerupScript.Identifier.GrattaEVinci_ScratchAndWin,
					PowerupScript.Identifier.ExpiredMedicines,
					PowerupScript.Identifier.GoldenHand_MidasTouch,
					PowerupScript.Identifier.Painkillers,
					PowerupScript.Identifier.Wallet,
					PowerupScript.Identifier.Calendar,
					PowerupScript.Identifier.Cross,
					PowerupScript.Identifier.YellowStar,
					PowerupScript.Identifier.Wolf,
					PowerupScript.Identifier.FortuneCookie,
					PowerupScript.Identifier.VineSoupShroom,
					PowerupScript.Identifier.GiantShroom,
					PowerupScript.Identifier.FideltyCard,
					PowerupScript.Identifier.BrokenCalculator,
					PowerupScript.Identifier.TheCollector,
					PowerupScript.Identifier.Rosary,
					PowerupScript.Identifier.BookOfShadows,
					PowerupScript.Identifier.Gabibbh,
					PowerupScript.Identifier.PossessedPhone,
					PowerupScript.Identifier.MysticalTomato,
					PowerupScript.Identifier.RitualBell,
					PowerupScript.Identifier.CrystalSkull,
					PowerupScript.Identifier.Sardines,
					PowerupScript.Identifier.AncientCoin,
					PowerupScript.Identifier.HamsaUpside,
					PowerupScript.Identifier.FortuneChanneler,
					PowerupScript.Identifier.Nose,
					PowerupScript.Identifier.EyeJar,
					PowerupScript.Identifier.AbstractPainting,
					PowerupScript.Identifier.Pareidolia,
					PowerupScript.Identifier.EvilDeal,
					PowerupScript.Identifier.ChastityBelt,
					PowerupScript.Identifier.VoiceMailTape,
					PowerupScript.Identifier.Garbage,
					PowerupScript.Identifier.AllIn,
					PowerupScript.Identifier.RingBell,
					PowerupScript.Identifier.ConsolationPrize,
					PowerupScript.Identifier.CloversLandPatch,
					PowerupScript.Identifier.DarkLotus,
					PowerupScript.Identifier.StepsCounter,
					PowerupScript.Identifier.Depression,
					PowerupScript.Identifier.LocomotiveDiesel,
					PowerupScript.Identifier.LocomotiveSteam,
					PowerupScript.Identifier.WeirdClock,
					PowerupScript.Identifier.MusicTape,
					PowerupScript.Identifier.DiscA,
					PowerupScript.Identifier.DiscB,
					PowerupScript.Identifier.DiscC,
					PowerupScript.Identifier.Jimbo,
					PowerupScript.Identifier.ShoppingCart,
					PowerupScript.Identifier.CrowBar,
					PowerupScript.Identifier.PlayingCard_HeartsAce,
					PowerupScript.Identifier.PlayingCard_ClubsAce,
					PowerupScript.Identifier.PlayingCard_DiamondsAce,
					PowerupScript.Identifier.PlayingCard_SpadesAce,
					PowerupScript.Identifier._999_AngelHand,
					PowerupScript.Identifier._999_EyeOfGod,
					PowerupScript.Identifier._999_HolySpirit,
					PowerupScript.Identifier._999_SacredHeart,
					PowerupScript.Identifier._999_Aureola,
					PowerupScript.Identifier._999_TheBlood,
					PowerupScript.Identifier._999_TheBody,
					PowerupScript.Identifier._999_Eternity,
					PowerupScript.Identifier._999_AdamsRibcage,
					PowerupScript.Identifier._999_OphanimWheels,
					PowerupScript.Identifier.undefined
				};
			}

			// Token: 0x06001326 RID: 4902 RVA: 0x0007958D File Offset: 0x0007778D
			public static List<PowerupScript.Identifier> _LockedPowerups_ResultingList_Definition()
			{
				if (Data.GameData.inst == null)
				{
					return new List<PowerupScript.Identifier>();
				}
				return new List<PowerupScript.Identifier>(164);
			}

			// Token: 0x06001327 RID: 4903 RVA: 0x000795A6 File Offset: 0x000777A6
			private void _LockedPowerupsSystem_ListsEnsure()
			{
				if (this.unlockedPowerups == null)
				{
					this.unlockedPowerups = Data.GameData._UnlockedPowerups_Definition();
				}
				if (this.lockedPowerups == null)
				{
					this.lockedPowerups = Data.GameData._LockedPowerups_Definition();
				}
				if (this.lockedPowerups_ResultingList == null)
				{
					this.lockedPowerups_ResultingList = Data.GameData._LockedPowerups_ResultingList_Definition();
				}
			}

			// Token: 0x06001328 RID: 4904 RVA: 0x000795E4 File Offset: 0x000777E4
			private void _LockedPowerupsResultingList_Compute(bool ensureLists)
			{
				if (ensureLists)
				{
					this._LockedPowerupsSystem_ListsEnsure();
				}
				this.lockedPowerups_ResultingList.Clear();
				foreach (PowerupScript.Identifier identifier in this.lockedPowerups)
				{
					if (identifier != PowerupScript.Identifier.undefined)
					{
						this.lockedPowerups_ResultingList.Add(identifier);
					}
				}
				foreach (PowerupScript.Identifier identifier2 in this.unlockedPowerups)
				{
					if (identifier2 != PowerupScript.Identifier.undefined)
					{
						this.lockedPowerups_ResultingList.Remove(identifier2);
					}
				}
			}

			// Token: 0x06001329 RID: 4905 RVA: 0x000796A0 File Offset: 0x000778A0
			private void _LockedPowerups_SavePrepare()
			{
				this._LockedPowerupsSystem_ListsEnsure();
				this._unlockedPowerupsString = PlatformDataMaster.EnumListToString<PowerupScript.Identifier>(this.unlockedPowerups, ',');
				this._LockedPowerupsResultingList_Compute(false);
			}

			// Token: 0x0600132A RID: 4906 RVA: 0x000796C2 File Offset: 0x000778C2
			private void _LockedPowerups_LoadPrepare()
			{
				this._LockedPowerupsSystem_ListsEnsure();
				PlatformDataMaster.EnumListFromString<PowerupScript.Identifier>(this._unlockedPowerupsString, ref this.unlockedPowerups, true, ',');
				this._LockedPowerupsResultingList_Compute(false);
			}

			// Token: 0x0600132B RID: 4907 RVA: 0x000796E8 File Offset: 0x000778E8
			public static bool IsPowerupSecret(PowerupScript.Identifier powerup)
			{
				PowerupScript powerup_Quick = PowerupScript.GetPowerup_Quick(powerup);
				if (powerup_Quick == null)
				{
					return false;
				}
				PowerupScript.Archetype archetype = powerup_Quick.archetype;
				return archetype == PowerupScript.Archetype.skeleton || archetype == PowerupScript.Archetype.sacred;
			}

			// Token: 0x0600132C RID: 4908 RVA: 0x0007971F File Offset: 0x0007791F
			public int LockedPowerups_GetCount()
			{
				this._LockedPowerupsSystem_ListsEnsure();
				if (this.lockedPowerups_ResultingList == null)
				{
					this._LockedPowerupsResultingList_Compute(false);
				}
				return this.lockedPowerups_ResultingList.Count;
			}

			// Token: 0x0600132D RID: 4909 RVA: 0x00079741 File Offset: 0x00077941
			public List<PowerupScript.Identifier> LockedPowerups_GetList()
			{
				this._LockedPowerupsSystem_ListsEnsure();
				this._LockedPowerupsResultingList_Compute(false);
				return this.lockedPowerups_ResultingList;
			}

			// Token: 0x0600132E RID: 4910 RVA: 0x00079758 File Offset: 0x00077958
			public void LockedPowerups_Unlock(PowerupScript.Identifier powerup)
			{
				this._LockedPowerupsSystem_ListsEnsure();
				this.unlockedPowerups.Add(powerup);
				this.lockedPowerups_ResultingList.Remove(powerup);
				if (!this.hasEverUnlockedAPowerup)
				{
					this.hasEverUnlockedAPowerup = true;
					if (TerminalScript.instance != null && !TerminalScript.IsLoggedIn())
					{
						GameplayMaster.unlockPowerupFirstTimeDialogueBooked = true;
					}
				}
			}

			// Token: 0x0600132F RID: 4911 RVA: 0x000797B0 File Offset: 0x000779B0
			public void _LockedPowerups_UnlockAll()
			{
				this._LockedPowerupsSystem_ListsEnsure();
				foreach (PowerupScript.Identifier identifier in this.lockedPowerups)
				{
					this.unlockedPowerups.Add(identifier);
					this.lockedPowerups_ResultingList.Remove(identifier);
				}
				if (!this.hasEverUnlockedAPowerup)
				{
					this.hasEverUnlockedAPowerup = true;
					if (!TerminalScript.IsLoggedIn())
					{
						GameplayMaster.unlockPowerupFirstTimeDialogueBooked = true;
					}
				}
			}

			// Token: 0x06001330 RID: 4912 RVA: 0x00079838 File Offset: 0x00077A38
			public void _LockedPowerups_LockAll()
			{
				this.unlockedPowerups = Data.GameData._UnlockedPowerups_Definition();
				this.lockedPowerups = Data.GameData._LockedPowerups_Definition();
			}

			// Token: 0x06001331 RID: 4913 RVA: 0x00079850 File Offset: 0x00077A50
			public List<PowerupScript.Identifier> _UnlockedPowerups_GetList()
			{
				this._LockedPowerupsSystem_ListsEnsure();
				return this.unlockedPowerups;
			}

			// Token: 0x06001332 RID: 4914 RVA: 0x00079860 File Offset: 0x00077A60
			public bool PowerupRealInstances_AreAllUnlocked()
			{
				if (this.allPowerupsUnlockedChacheResult)
				{
					return this.allPowerupsUnlockedChacheResult;
				}
				if (PowerupScript.all.Count == 0)
				{
					Debug.LogError("Data.game.AllPowerupsAreUnlocked(): Cannot check if there are no powerups instances.");
					return false;
				}
				List<PowerupScript.Identifier> list = this.LockedPowerups_GetList();
				for (int i = 0; i < PowerupScript.all.Count; i++)
				{
					PowerupScript powerupScript = PowerupScript.all[i];
					if (!(powerupScript == null) && powerupScript.identifier != PowerupScript.Identifier.undefined && powerupScript.identifier != PowerupScript.Identifier.count && list.Contains(powerupScript.identifier))
					{
						return false;
					}
				}
				this.allPowerupsUnlockedChacheResult = true;
				return true;
			}

			// Token: 0x06001333 RID: 4915 RVA: 0x000798F3 File Offset: 0x00077AF3
			public void UnlockableSteps_OnRechargingRedButtonCharges(int chargesN)
			{
				if (GameplayMaster.IsCustomSeed())
				{
					return;
				}
				Data.game.UnlockSteps_SuperCapacitor += chargesN;
				Data.game.UnlockSteps_CrankGenerator += chargesN;
			}

			// Token: 0x06001334 RID: 4916 RVA: 0x00079924 File Offset: 0x00077B24
			public void UnlockableSteps_OnCharmDiscard(int stepsN)
			{
				if (GameplayMaster.IsCustomSeed())
				{
					return;
				}
				int num = this.UnlockSteps_Vorago;
				this.UnlockSteps_Vorago = num + 1;
				num = this.UnlockSteps_DungBeetleStercoRaro;
				this.UnlockSteps_DungBeetleStercoRaro = num + 1;
				num = this.UnlockSteps_Sardines;
				this.UnlockSteps_Sardines = num + 1;
				num = this.UnlockSteps_CloversLandPatch;
				this.UnlockSteps_CloversLandPatch = num + 1;
				num = this.UnlockSteps_DarkLotus;
				this.UnlockSteps_DarkLotus = num + 1;
			}

			// (get) Token: 0x06001335 RID: 4917 RVA: 0x00079989 File Offset: 0x00077B89
			public static int UnlockStepsMissing_PuppetPersonalTrainer
			{
				get
				{
					return Mathf.Max(0, 15 - GameplayData.Stats_ModifiedLemonTriggeredTimesGet().CastToInt() - GameplayData.Stats_ModifiedCherryTriggeredTimesGet().CastToInt());
				}
			}

			// (get) Token: 0x06001336 RID: 4918 RVA: 0x000799A9 File Offset: 0x00077BA9
			public static int UnlockStepsMissing_PuppetElectrician
			{
				get
				{
					return Mathf.Max(0, 15 - GameplayData.Stats_ModifiedCloverTriggeredTimesGet().CastToInt() - GameplayData.Stats_ModifiedBellTriggeredTimesGet().CastToInt());
				}
			}

			// (get) Token: 0x06001337 RID: 4919 RVA: 0x000799C9 File Offset: 0x00077BC9
			public static int UnlockStepsMissing_PuppetFortuneTeller
			{
				get
				{
					return Mathf.Max(0, 15 - GameplayData.Stats_ModifiedDiamondTriggeredTimesGet().CastToInt() - GameplayData.Stats_ModifiedCoinsTriggeredTimesGet().CastToInt() - GameplayData.Stats_ModifiedSevenTriggeredTimesGet().CastToInt());
				}
			}

			// (get) Token: 0x06001338 RID: 4920 RVA: 0x000799F4 File Offset: 0x00077BF4
			public static int UnlockStepsMissing_StepsCounter
			{
				get
				{
					return Mathf.Max(0, 20 - GameplayData.Stats_RedButtonEffectiveActivations_Get());
				}
			}

			// (get) Token: 0x06001339 RID: 4921 RVA: 0x00079A04 File Offset: 0x00077C04
			public static int UnlockStepsMissing_DevilsHorn
			{
				get
				{
					return Mathf.Max(0, 3 - (int)GameplayData.Stats_SixSixSix_SeenTimes);
				}
			}

			// (get) Token: 0x0600133A RID: 4922 RVA: 0x00079A14 File Offset: 0x00077C14
			public static int UnlockStepsMissing_Necronomicon
			{
				get
				{
					return Mathf.Max(0, 5 - (int)GameplayData.Stats_SixSixSix_SeenTimes);
				}
			}

			// (get) Token: 0x0600133B RID: 4923 RVA: 0x00079A24 File Offset: 0x00077C24
			public static int UnlockStepsMissing_KingMida
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					long num = 30L - GameplayData.Stats_ModifiedDiamondTriggeredTimesGet() - GameplayData.Stats_ModifiedCoinsTriggeredTimesGet() - GameplayData.Stats_ModifiedSevenTriggeredTimesGet();
					if (num < 0L)
					{
						num = 0L;
					}
					return num.CastToInt();
				}
			}

			// (get) Token: 0x0600133C RID: 4924 RVA: 0x00079A60 File Offset: 0x00077C60
			public static int UnlockStepsMissing_Dealer
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					long num = 30L - GameplayData.Stats_ModifiedLemonTriggeredTimesGet() - GameplayData.Stats_ModifiedCherryTriggeredTimesGet();
					if (num < 0L)
					{
						num = 0L;
					}
					return num.CastToInt();
				}
			}

			// (get) Token: 0x0600133D RID: 4925 RVA: 0x00079A94 File Offset: 0x00077C94
			public static int UnlockStepsMissing_RagingCapitalist
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					long num = 30L - GameplayData.Stats_ModifiedCloverTriggeredTimesGet() - GameplayData.Stats_ModifiedBellTriggeredTimesGet();
					if (num < 0L)
					{
						num = 0L;
					}
					return num.CastToInt();
				}
			}

			// (get) Token: 0x0600133E RID: 4926 RVA: 0x00079AC8 File Offset: 0x00077CC8
			// (set) Token: 0x0600133F RID: 4927 RVA: 0x00079AD0 File Offset: 0x00077CD0
			public int UnlockSteps_ElectricityCounter
			{
				get
				{
					return this.unlockSteps_ElectricityCounter;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_ElectricityCounter = value;
					if (this.unlockSteps_ElectricityCounter >= 50)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.ElectricityCounter);
					}
				}
			}

			// (get) Token: 0x06001340 RID: 4928 RVA: 0x00079AF3 File Offset: 0x00077CF3
			public static int UnlockStepsMissing_ElectricityCounter
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 50 - Data.game.UnlockSteps_ElectricityCounter);
				}
			}

			// (get) Token: 0x06001341 RID: 4929 RVA: 0x00079B11 File Offset: 0x00077D11
			// (set) Token: 0x06001342 RID: 4930 RVA: 0x00079B19 File Offset: 0x00077D19
			public int UnlockSteps_DarkLotus
			{
				get
				{
					return this.unlockSteps_DarkLotus;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_DarkLotus = value;
					if (this.unlockSteps_DarkLotus >= 200)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.DarkLotus);
					}
				}
			}

			// (get) Token: 0x06001343 RID: 4931 RVA: 0x00079B3F File Offset: 0x00077D3F
			public static int UnlockStepsMissing_DarkLotus
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 200 - Data.game.UnlockSteps_DarkLotus);
				}
			}

			// (get) Token: 0x06001344 RID: 4932 RVA: 0x00079B60 File Offset: 0x00077D60
			// (set) Token: 0x06001345 RID: 4933 RVA: 0x00079B68 File Offset: 0x00077D68
			public int UnlockSteps_CloversLandPatch
			{
				get
				{
					return this.unlockSteps_CloversLandPatch;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_CloversLandPatch = value;
					if (this.unlockSteps_CloversLandPatch >= 50)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.CloversLandPatch);
					}
				}
			}

			// (get) Token: 0x06001346 RID: 4934 RVA: 0x00079B8B File Offset: 0x00077D8B
			public static int UnlockStepsMissing_CloversLandPatch
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 50 - Data.game.UnlockSteps_CloversLandPatch);
				}
			}

			// (get) Token: 0x06001347 RID: 4935 RVA: 0x00079BA9 File Offset: 0x00077DA9
			// (set) Token: 0x06001348 RID: 4936 RVA: 0x00079BB1 File Offset: 0x00077DB1
			public int UnlockSteps_AllIn
			{
				get
				{
					return this.unlockSteps_AllIn;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_AllIn = value;
					if (this.unlockSteps_AllIn >= 100)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.AllIn);
					}
				}
			}

			// (get) Token: 0x06001349 RID: 4937 RVA: 0x00079BD4 File Offset: 0x00077DD4
			public static int UnlockStepsMissing_AllIn
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 100 - Data.game.UnlockSteps_AllIn);
				}
			}

			// (get) Token: 0x0600134A RID: 4938 RVA: 0x00079BF2 File Offset: 0x00077DF2
			// (set) Token: 0x0600134B RID: 4939 RVA: 0x00079BFA File Offset: 0x00077DFA
			public int UnlockSteps_Garbage
			{
				get
				{
					return this.unlockSteps_Garbage;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_Garbage = value;
					if (this.unlockSteps_Garbage >= 50)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Garbage);
					}
				}
			}

			// (get) Token: 0x0600134C RID: 4940 RVA: 0x00079C1D File Offset: 0x00077E1D
			public static int UnlockStepsMissing_Garbage
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 50 - Data.game.UnlockSteps_Garbage);
				}
			}

			// (get) Token: 0x0600134D RID: 4941 RVA: 0x00079C3B File Offset: 0x00077E3B
			// (set) Token: 0x0600134E RID: 4942 RVA: 0x00079C43 File Offset: 0x00077E43
			public int UnlockSteps_VoiceMail
			{
				get
				{
					return this.unlockSteps_VoiceMail;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_VoiceMail = value;
					if (this.unlockSteps_VoiceMail >= 50)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.VoiceMailTape);
					}
				}
			}

			// (get) Token: 0x0600134F RID: 4943 RVA: 0x00079C66 File Offset: 0x00077E66
			public static int UnlockStepsMissing_VoiceMail
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 50 - Data.game.UnlockSteps_VoiceMail);
				}
			}

			// (get) Token: 0x06001350 RID: 4944 RVA: 0x00079C84 File Offset: 0x00077E84
			// (set) Token: 0x06001351 RID: 4945 RVA: 0x00079C8C File Offset: 0x00077E8C
			public int UnlockSteps_FortuneChanneler
			{
				get
				{
					return this.unlockSteps_FortuneChanneler;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_FortuneChanneler = value;
					if (this.unlockSteps_FortuneChanneler >= 100)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.FortuneChanneler);
					}
				}
			}

			// (get) Token: 0x06001352 RID: 4946 RVA: 0x00079CAF File Offset: 0x00077EAF
			public static int UnlockStepsMissing_FortuneChanneler
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 100 - Data.game.UnlockSteps_FortuneChanneler);
				}
			}

			// (get) Token: 0x06001353 RID: 4947 RVA: 0x00079CCD File Offset: 0x00077ECD
			// (set) Token: 0x06001354 RID: 4948 RVA: 0x00079CD5 File Offset: 0x00077ED5
			public int UnlockSteps_HamsaUpside
			{
				get
				{
					return this.unlockSteps_HamsaUpside;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_HamsaUpside = value;
					if (this.unlockSteps_HamsaUpside >= 100)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.HamsaUpside);
					}
				}
			}

			// (get) Token: 0x06001355 RID: 4949 RVA: 0x00079CF7 File Offset: 0x00077EF7
			public static int UnlockStepsMissing_HamsaUpside
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 100 - Data.game.UnlockSteps_HamsaUpside);
				}
			}

			// (get) Token: 0x06001356 RID: 4950 RVA: 0x00079D15 File Offset: 0x00077F15
			// (set) Token: 0x06001357 RID: 4951 RVA: 0x00079D1D File Offset: 0x00077F1D
			public int UnlockSteps_AncientCoin
			{
				get
				{
					return this.unlockSteps_AncientCoin;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_AncientCoin = value;
					if (this.unlockSteps_AncientCoin >= 100)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.AncientCoin);
					}
				}
			}

			// (get) Token: 0x06001358 RID: 4952 RVA: 0x00079D40 File Offset: 0x00077F40
			public static int UnlockStepsMissing_AncientCoin
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 100 - Data.game.UnlockSteps_AncientCoin);
				}
			}

			// (get) Token: 0x06001359 RID: 4953 RVA: 0x00079D5E File Offset: 0x00077F5E
			// (set) Token: 0x0600135A RID: 4954 RVA: 0x00079D66 File Offset: 0x00077F66
			public int UnlockSteps_Sardines
			{
				get
				{
					return this.unlockSteps_Sardines;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_Sardines = value;
					if (this.unlockSteps_Sardines >= 200)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Sardines);
					}
				}
			}

			// (get) Token: 0x0600135B RID: 4955 RVA: 0x00079D8C File Offset: 0x00077F8C
			public static int UnlockStepsMissing_Sardines
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 200 - Data.game.UnlockSteps_Sardines);
				}
			}

			// (get) Token: 0x0600135C RID: 4956 RVA: 0x00079DAD File Offset: 0x00077FAD
			// (set) Token: 0x0600135D RID: 4957 RVA: 0x00079DB5 File Offset: 0x00077FB5
			public int UnlockSteps_Rosary
			{
				get
				{
					return this.unlockSteps_Rosary;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_Rosary = value;
					if (this.unlockSteps_Rosary >= 10)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Rosary);
					}
				}
			}

			// (get) Token: 0x0600135E RID: 4958 RVA: 0x00079DD8 File Offset: 0x00077FD8
			public static int UnlockStepsMissing_Rosary
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 10 - Data.game.UnlockSteps_Rosary);
				}
			}

			// (get) Token: 0x0600135F RID: 4959 RVA: 0x00079DF6 File Offset: 0x00077FF6
			// (set) Token: 0x06001360 RID: 4960 RVA: 0x00079DFE File Offset: 0x00077FFE
			public int UnlockSteps_FortuneCookie
			{
				get
				{
					return this.unlockSteps_FortuneCookie;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_FortuneCookie = value;
					if (this.unlockSteps_FortuneCookie >= 10)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.FortuneCookie);
					}
				}
			}

			// (get) Token: 0x06001361 RID: 4961 RVA: 0x00079E21 File Offset: 0x00078021
			public static int UnlockStepsMissing_FortuneCookie
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 10 - Data.game.UnlockSteps_FortuneCookie);
				}
			}

			// (get) Token: 0x06001362 RID: 4962 RVA: 0x00079E3F File Offset: 0x0007803F
			// (set) Token: 0x06001363 RID: 4963 RVA: 0x00079E47 File Offset: 0x00078047
			public int UnlockSteps_YellowStar
			{
				get
				{
					return this.unlockSteps_YellowStar;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_YellowStar = value;
					if (this.unlockSteps_YellowStar >= 10)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.YellowStar);
					}
				}
			}

			// (get) Token: 0x06001364 RID: 4964 RVA: 0x00079E6A File Offset: 0x0007806A
			public static int UnlockStepsMissing_YellowStar
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 10 - Data.game.UnlockSteps_YellowStar);
				}
			}

			// (get) Token: 0x06001365 RID: 4965 RVA: 0x00079E88 File Offset: 0x00078088
			// (set) Token: 0x06001366 RID: 4966 RVA: 0x00079E90 File Offset: 0x00078090
			public int UnlockSteps_Cross
			{
				get
				{
					return this.unlockSteps_Cross;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_Cross = value;
					if (this.unlockSteps_Cross >= 100)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Cross);
					}
				}
			}

			// (get) Token: 0x06001367 RID: 4967 RVA: 0x00079EB3 File Offset: 0x000780B3
			public static int UnlockStepsMissing_Cross
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 100 - Data.game.UnlockSteps_Cross);
				}
			}

			// (get) Token: 0x06001368 RID: 4968 RVA: 0x00079ED1 File Offset: 0x000780D1
			// (set) Token: 0x06001369 RID: 4969 RVA: 0x00079ED9 File Offset: 0x000780D9
			public int UnlockSteps_Calendar
			{
				get
				{
					return this.unlockSteps_Calendar;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_Calendar = value;
					if (this.unlockSteps_Calendar >= 30)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Calendar);
					}
				}
			}

			// (get) Token: 0x0600136A RID: 4970 RVA: 0x00079EFC File Offset: 0x000780FC
			public static int UnlockStepsMissing_Calendar
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 30 - Data.game.UnlockSteps_Calendar);
				}
			}

			// (get) Token: 0x0600136B RID: 4971 RVA: 0x00079F1A File Offset: 0x0007811A
			// (set) Token: 0x0600136C RID: 4972 RVA: 0x00079F22 File Offset: 0x00078122
			public int UnlockSteps_PainKillers
			{
				get
				{
					return this.unlockSteps_PainKillers;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_PainKillers = value;
					if (this.unlockSteps_PainKillers >= 50)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Painkillers);
					}
				}
			}

			// (get) Token: 0x0600136D RID: 4973 RVA: 0x00079F45 File Offset: 0x00078145
			public static int UnlockStepsMissing_PainKillers
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 50 - Data.game.UnlockSteps_PainKillers);
				}
			}

			// (get) Token: 0x0600136E RID: 4974 RVA: 0x00079F63 File Offset: 0x00078163
			// (set) Token: 0x0600136F RID: 4975 RVA: 0x00079F6B File Offset: 0x0007816B
			public int UnlockSteps_ScratchAndWin
			{
				get
				{
					return this.unlockSteps_ScratchAndWin;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_ScratchAndWin = value;
					if (this.unlockSteps_ScratchAndWin >= 25)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.GrattaEVinci_ScratchAndWin);
					}
				}
			}

			// (get) Token: 0x06001370 RID: 4976 RVA: 0x00079F8E File Offset: 0x0007818E
			public static int UnlockStepsMissing_ScratchAndWin
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 25 - Data.game.UnlockSteps_ScratchAndWin);
				}
			}

			// (get) Token: 0x06001371 RID: 4977 RVA: 0x00079FAC File Offset: 0x000781AC
			// (set) Token: 0x06001372 RID: 4978 RVA: 0x00079FB4 File Offset: 0x000781B4
			public int UnlockSteps_PhotoBook
			{
				get
				{
					return this.unlockSteps_PhotoBook;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_PhotoBook = value;
					if (this.unlockSteps_PhotoBook >= 50)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.PhotoBook);
					}
				}
			}

			// (get) Token: 0x06001373 RID: 4979 RVA: 0x00079FD7 File Offset: 0x000781D7
			public static int UnlockStepsMissing_PhotoBook
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 50 - Data.game.UnlockSteps_PhotoBook);
				}
			}

			// (get) Token: 0x06001374 RID: 4980 RVA: 0x00079FF5 File Offset: 0x000781F5
			// (set) Token: 0x06001375 RID: 4981 RVA: 0x00079FFD File Offset: 0x000781FD
			public int UnlockSteps_Baphomet
			{
				get
				{
					return this.unlockSteps_Baphomet;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_Baphomet = value;
					if (this.unlockSteps_Baphomet >= 666)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Baphomet);
					}
				}
			}

			// (get) Token: 0x06001376 RID: 4982 RVA: 0x0007A023 File Offset: 0x00078223
			public static int UnlockStepsMissing_Baphomet
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 666 - Data.game.UnlockSteps_Baphomet);
				}
			}

			// (get) Token: 0x06001377 RID: 4983 RVA: 0x0007A044 File Offset: 0x00078244
			// (set) Token: 0x06001378 RID: 4984 RVA: 0x0007A04C File Offset: 0x0007824C
			public int UnlockSteps_DungBeetleStercoRaro
			{
				get
				{
					return this.unlockSteps_DungBeetleStercoRaro;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_DungBeetleStercoRaro = value;
					if (this.unlockSteps_DungBeetleStercoRaro >= 300)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.PoopBeetle);
					}
				}
			}

			// (get) Token: 0x06001379 RID: 4985 RVA: 0x0007A072 File Offset: 0x00078272
			public static int UnlockStepsMissing_DungBeetleStercoRaro
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 300 - Data.game.UnlockSteps_DungBeetleStercoRaro);
				}
			}

			// (get) Token: 0x0600137A RID: 4986 RVA: 0x0007A093 File Offset: 0x00078293
			// (set) Token: 0x0600137B RID: 4987 RVA: 0x0007A09B File Offset: 0x0007829B
			public int UnlockSteps_LuckyCatFat
			{
				get
				{
					return this.unlockSteps_LuckyCatFat;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_LuckyCatFat = value;
					if (this.unlockSteps_LuckyCatFat >= 50)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.LuckyCatFat);
					}
				}
			}

			// (get) Token: 0x0600137C RID: 4988 RVA: 0x0007A0BE File Offset: 0x000782BE
			public static int UnlockStepsMissing_LuckyCatFat
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 50 - Data.game.UnlockSteps_LuckyCatFat);
				}
			}

			// (get) Token: 0x0600137D RID: 4989 RVA: 0x0007A0DC File Offset: 0x000782DC
			// (set) Token: 0x0600137E RID: 4990 RVA: 0x0007A0E4 File Offset: 0x000782E4
			public int UnlockSteps_LuckyCatSwole
			{
				get
				{
					return this.unlockSteps_LuckyCatSwole;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_LuckyCatSwole = value;
					if (this.unlockSteps_LuckyCatSwole >= 25)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.LuckyCatSwole);
					}
				}
			}

			// (get) Token: 0x0600137F RID: 4991 RVA: 0x0007A107 File Offset: 0x00078307
			public static int UnlockStepsMissing_LuckyCatSwole
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 25 - Data.game.UnlockSteps_LuckyCatSwole);
				}
			}

			// (get) Token: 0x06001380 RID: 4992 RVA: 0x0007A125 File Offset: 0x00078325
			// (set) Token: 0x06001381 RID: 4993 RVA: 0x0007A12D File Offset: 0x0007832D
			public int UnlockSteps_HorseShoeGold
			{
				get
				{
					return this.unlockSteps_HorseShoeGold;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_HorseShoeGold = value;
					if (this.unlockSteps_HorseShoeGold >= 7)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.HorseShoeGold);
					}
				}
			}

			// (get) Token: 0x06001382 RID: 4994 RVA: 0x0007A14E File Offset: 0x0007834E
			public static int UnlockStepsMissing_HorseShoeGold
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 7 - Data.game.UnlockSteps_HorseShoeGold);
				}
			}

			// (get) Token: 0x06001383 RID: 4995 RVA: 0x0007A16B File Offset: 0x0007836B
			// (set) Token: 0x06001384 RID: 4996 RVA: 0x0007A173 File Offset: 0x00078373
			public int UnlockSteps_GoldenPepper
			{
				get
				{
					return this.unlockSteps_GoldenPepper;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_GoldenPepper = value;
					if (this.unlockSteps_GoldenPepper >= 30)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.GoldenPepper);
					}
				}
			}

			// (get) Token: 0x06001385 RID: 4997 RVA: 0x0007A196 File Offset: 0x00078396
			public static int UnlockStepsMissing_GoldenPepper
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 30 - Data.game.UnlockSteps_GoldenPepper);
				}
			}

			// (get) Token: 0x06001386 RID: 4998 RVA: 0x0007A1B4 File Offset: 0x000783B4
			// (set) Token: 0x06001387 RID: 4999 RVA: 0x0007A1BC File Offset: 0x000783BC
			public int UnlockSteps_RottenPepper
			{
				get
				{
					return this.unlockSteps_RottenPepper;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_RottenPepper = value;
					if (this.unlockSteps_RottenPepper >= 20)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.RottenPepper);
					}
				}
			}

			// (get) Token: 0x06001388 RID: 5000 RVA: 0x0007A1DF File Offset: 0x000783DF
			public static int UnlockStepsMissing_RottenPepper
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 20 - Data.game.UnlockSteps_RottenPepper);
				}
			}

			// (get) Token: 0x06001389 RID: 5001 RVA: 0x0007A1FD File Offset: 0x000783FD
			// (set) Token: 0x0600138A RID: 5002 RVA: 0x0007A205 File Offset: 0x00078405
			public int UnlockSteps_BellPepper
			{
				get
				{
					return this.unlockSteps_BellPepper;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BellPepper = value;
					if (this.unlockSteps_BellPepper >= 30)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.BellPepper);
					}
				}
			}

			// (get) Token: 0x0600138B RID: 5003 RVA: 0x0007A228 File Offset: 0x00078428
			public static int UnlockStepsMissing_BellPepper
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 30 - Data.game.UnlockSteps_BellPepper);
				}
			}

			// (get) Token: 0x0600138C RID: 5004 RVA: 0x0007A246 File Offset: 0x00078446
			// (set) Token: 0x0600138D RID: 5005 RVA: 0x0007A24E File Offset: 0x0007844E
			public int UnlockSteps_Abyssu
			{
				get
				{
					return this.unlockSteps_Abyssu;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_Abyssu = value;
					if (this.unlockSteps_Abyssu >= 100)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Hole_Circle);
					}
				}
			}

			// (get) Token: 0x0600138E RID: 5006 RVA: 0x0007A271 File Offset: 0x00078471
			public static int UnlockStepsMissing_Abyssu
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 100 - Data.game.UnlockSteps_Abyssu);
				}
			}

			// (get) Token: 0x0600138F RID: 5007 RVA: 0x0007A28F File Offset: 0x0007848F
			// (set) Token: 0x06001390 RID: 5008 RVA: 0x0007A297 File Offset: 0x00078497
			public int UnlockSteps_Vorago
			{
				get
				{
					return this.unlockSteps_Vorago;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_Vorago = value;
					if (this.unlockSteps_Vorago >= 100)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Hole_Romboid);
					}
				}
			}

			// (get) Token: 0x06001391 RID: 5009 RVA: 0x0007A2BA File Offset: 0x000784BA
			public static int UnlockStepsMissing_Vorago
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 100 - Data.game.UnlockSteps_Vorago);
				}
			}

			// (get) Token: 0x06001392 RID: 5010 RVA: 0x0007A2D8 File Offset: 0x000784D8
			// (set) Token: 0x06001393 RID: 5011 RVA: 0x0007A2E0 File Offset: 0x000784E0
			public int UnlockSteps_Barathrum
			{
				get
				{
					return this.unlockSteps_Barathrum;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_Barathrum = value;
					if (this.unlockSteps_Barathrum >= 100)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Hole_Cross);
					}
				}
			}

			// (get) Token: 0x06001394 RID: 5012 RVA: 0x0007A303 File Offset: 0x00078503
			public static int UnlockStepsMissing__Barathrum
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 100 - Data.game.UnlockSteps_Barathrum);
				}
			}

			// (get) Token: 0x06001395 RID: 5013 RVA: 0x0007A321 File Offset: 0x00078521
			// (set) Token: 0x06001396 RID: 5014 RVA: 0x0007A329 File Offset: 0x00078529
			public int UnlockSteps_SuperCapacitor
			{
				get
				{
					return this.unlockSteps_SuperCapacitor;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_SuperCapacitor = value;
					if (this.unlockSteps_SuperCapacitor >= 200)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.SuperCapacitor);
					}
				}
			}

			// (get) Token: 0x06001397 RID: 5015 RVA: 0x0007A34F File Offset: 0x0007854F
			public static int UnlockStepsMissing__SuperCapacitor
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 200 - Data.game.UnlockSteps_SuperCapacitor);
				}
			}

			// (get) Token: 0x06001398 RID: 5016 RVA: 0x0007A370 File Offset: 0x00078570
			// (set) Token: 0x06001399 RID: 5017 RVA: 0x0007A378 File Offset: 0x00078578
			public int UnlockSteps_CrankGenerator
			{
				get
				{
					return this.unlockSteps_CrankGenerator;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_CrankGenerator = value;
					if (this.unlockSteps_CrankGenerator >= 50)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.CrankGenerator);
					}
				}
			}

			// (get) Token: 0x0600139A RID: 5018 RVA: 0x0007A39B File Offset: 0x0007859B
			public static int UnlockStepsMissing__CrankGenerator
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 50 - Data.game.UnlockSteps_CrankGenerator);
				}
			}

			// (get) Token: 0x0600139B RID: 5019 RVA: 0x0007A3B9 File Offset: 0x000785B9
			// (set) Token: 0x0600139C RID: 5020 RVA: 0x0007A3C1 File Offset: 0x000785C1
			public int UnlockSteps_BoardgameC_Bricks
			{
				get
				{
					return this.unlockSteps_BoardgameC_Bricks;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BoardgameC_Bricks = value;
					if (this.unlockSteps_BoardgameC_Bricks >= 50)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_C_Bricks);
					}
				}
			}

			// (get) Token: 0x0600139D RID: 5021 RVA: 0x0007A3E7 File Offset: 0x000785E7
			public static int UnlockStepsMissing__BoardgameC_Bricks
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 50 - Data.game.UnlockSteps_BoardgameC_Bricks);
				}
			}

			// (get) Token: 0x0600139E RID: 5022 RVA: 0x0007A405 File Offset: 0x00078605
			// (set) Token: 0x0600139F RID: 5023 RVA: 0x0007A40D File Offset: 0x0007860D
			public int UnlockSteps_BoardgameC_Wood
			{
				get
				{
					return this.unlockSteps_BoardgameC_Wood;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BoardgameC_Wood = value;
					if (this.unlockSteps_BoardgameC_Wood >= 50)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_C_Wood);
					}
				}
			}

			// (get) Token: 0x060013A0 RID: 5024 RVA: 0x0007A433 File Offset: 0x00078633
			public static int UnlockStepsMissing__BoardgameC_Wood
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 50 - Data.game.UnlockSteps_BoardgameC_Wood);
				}
			}

			// (get) Token: 0x060013A1 RID: 5025 RVA: 0x0007A451 File Offset: 0x00078651
			// (set) Token: 0x060013A2 RID: 5026 RVA: 0x0007A459 File Offset: 0x00078659
			public int UnlockSteps_BoardgameC_Sheep
			{
				get
				{
					return this.unlockSteps_BoardgameC_Sheep;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BoardgameC_Sheep = value;
					if (this.unlockSteps_BoardgameC_Sheep >= 500)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_C_Sheep);
					}
				}
			}

			// (get) Token: 0x060013A3 RID: 5027 RVA: 0x0007A482 File Offset: 0x00078682
			public static int UnlockStepsMissing__BoardgameC_Sheep
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 500 - Data.game.UnlockSteps_BoardgameC_Sheep);
				}
			}

			// (get) Token: 0x060013A4 RID: 5028 RVA: 0x0007A4A3 File Offset: 0x000786A3
			// (set) Token: 0x060013A5 RID: 5029 RVA: 0x0007A4AB File Offset: 0x000786AB
			public int UnlockSteps_BoardgameC_Wheat
			{
				get
				{
					return this.unlockSteps_BoardgameC_Wheat;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BoardgameC_Wheat = value;
					if (this.unlockSteps_BoardgameC_Wheat >= 500)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_C_Wheat);
					}
				}
			}

			// (get) Token: 0x060013A6 RID: 5030 RVA: 0x0007A4D4 File Offset: 0x000786D4
			public static int UnlockStepsMissing__BoardgameC_Wheat
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 500 - Data.game.UnlockSteps_BoardgameC_Wheat);
				}
			}

			// (get) Token: 0x060013A7 RID: 5031 RVA: 0x0007A4F5 File Offset: 0x000786F5
			// (set) Token: 0x060013A8 RID: 5032 RVA: 0x0007A4FD File Offset: 0x000786FD
			public int UnlockSteps_BoardgameC_Stone
			{
				get
				{
					return this.unlockSteps_BoardgameC_Stone;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BoardgameC_Stone = value;
					if (this.unlockSteps_BoardgameC_Stone >= 1000)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_C_Stone);
					}
				}
			}

			// (get) Token: 0x060013A9 RID: 5033 RVA: 0x0007A526 File Offset: 0x00078726
			public static int UnlockStepsMissing__BoardgameC_Stone
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 1000 - Data.game.UnlockSteps_BoardgameC_Stone);
				}
			}

			// (get) Token: 0x060013AA RID: 5034 RVA: 0x0007A547 File Offset: 0x00078747
			// (set) Token: 0x060013AB RID: 5035 RVA: 0x0007A54F File Offset: 0x0007874F
			public int UnlockSteps_BoardgameC_Harbor
			{
				get
				{
					return this.unlockSteps_BoardgameC_Harbor;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BoardgameC_Harbor = value;
					if (this.unlockSteps_BoardgameC_Harbor >= 1000)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_C_Harbor);
					}
				}
			}

			// (get) Token: 0x060013AC RID: 5036 RVA: 0x0007A578 File Offset: 0x00078778
			public static int UnlockStepsMissing__BoardgameC_Harbor
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 1000 - Data.game.UnlockSteps_BoardgameC_Harbor);
				}
			}

			// (get) Token: 0x060013AD RID: 5037 RVA: 0x0007A599 File Offset: 0x00078799
			// (set) Token: 0x060013AE RID: 5038 RVA: 0x0007A5A1 File Offset: 0x000787A1
			public int UnlockSteps_BoardgameC_Thief
			{
				get
				{
					return this.unlockSteps_BoardgameC_Thief;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BoardgameC_Thief = value;
					if (this.unlockSteps_BoardgameC_Thief >= 1000)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_C_Thief);
					}
				}
			}

			// (get) Token: 0x060013AF RID: 5039 RVA: 0x0007A5CA File Offset: 0x000787CA
			public static int UnlockStepsMissing__BoardgameC_Thief
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 1000 - Data.game.UnlockSteps_BoardgameC_Thief);
				}
			}

			// (get) Token: 0x060013B0 RID: 5040 RVA: 0x0007A5EB File Offset: 0x000787EB
			// (set) Token: 0x060013B1 RID: 5041 RVA: 0x0007A5F3 File Offset: 0x000787F3
			public int UnlockSteps_BoardgameM_Carriola
			{
				get
				{
					return this.unlockSteps_BoardgameM_Carriola;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BoardgameM_Carriola = value;
					if (this.unlockSteps_BoardgameM_Carriola >= 500)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_M_Carriola);
					}
				}
			}

			// (get) Token: 0x060013B2 RID: 5042 RVA: 0x0007A61C File Offset: 0x0007881C
			public static int UnlockStepsMissing__BoardgameM_Carriola
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 500 - Data.game.UnlockSteps_BoardgameM_Carriola);
				}
			}

			// (get) Token: 0x060013B3 RID: 5043 RVA: 0x0007A63D File Offset: 0x0007883D
			// (set) Token: 0x060013B4 RID: 5044 RVA: 0x0007A645 File Offset: 0x00078845
			public int UnlockSteps_BoardgameM_Shoe
			{
				get
				{
					return this.unlockSteps_BoardgameM_Shoe;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BoardgameM_Shoe = value;
					if (this.unlockSteps_BoardgameM_Shoe >= 500)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_M_Shoe);
					}
				}
			}

			// (get) Token: 0x060013B5 RID: 5045 RVA: 0x0007A66E File Offset: 0x0007886E
			public static int UnlockStepsMissing__BoardgameM_Shoe
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 500 - Data.game.UnlockSteps_BoardgameM_Shoe);
				}
			}

			// (get) Token: 0x060013B6 RID: 5046 RVA: 0x0007A68F File Offset: 0x0007888F
			// (set) Token: 0x060013B7 RID: 5047 RVA: 0x0007A697 File Offset: 0x00078897
			public int UnlockSteps_BoardgameM_Ditale
			{
				get
				{
					return this.unlockSteps_BoardgameM_Ditale;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BoardgameM_Ditale = value;
					if (this.unlockSteps_BoardgameM_Ditale >= 50)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_M_Ditale);
					}
				}
			}

			// (get) Token: 0x060013B8 RID: 5048 RVA: 0x0007A6BD File Offset: 0x000788BD
			public static int UnlockStepsMissing__BoardgameM_Ditale
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 50 - Data.game.UnlockSteps_BoardgameM_Ditale);
				}
			}

			// (get) Token: 0x060013B9 RID: 5049 RVA: 0x0007A6DB File Offset: 0x000788DB
			// (set) Token: 0x060013BA RID: 5050 RVA: 0x0007A6E3 File Offset: 0x000788E3
			public int UnlockSteps_BoardgameM_Iron
			{
				get
				{
					return this.unlockSteps_BoardgameM_Iron;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BoardgameM_Iron = value;
					if (this.unlockSteps_BoardgameM_Iron >= 50)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_M_FerroDaStiro);
					}
				}
			}

			// (get) Token: 0x060013BB RID: 5051 RVA: 0x0007A709 File Offset: 0x00078909
			public static int UnlockStepsMissing__BoardgameM_Iron
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 50 - Data.game.UnlockSteps_BoardgameM_Iron);
				}
			}

			// (get) Token: 0x060013BC RID: 5052 RVA: 0x0007A727 File Offset: 0x00078927
			// (set) Token: 0x060013BD RID: 5053 RVA: 0x0007A72F File Offset: 0x0007892F
			public int UnlockSteps_BoardgameM_Car
			{
				get
				{
					return this.unlockSteps_BoardgameM_Car;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BoardgameM_Car = value;
					if (this.unlockSteps_BoardgameM_Car >= 1000)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_M_Car);
					}
				}
			}

			// (get) Token: 0x060013BE RID: 5054 RVA: 0x0007A758 File Offset: 0x00078958
			public static int UnlockStepsMissing__BoardgameM_Car
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 1000 - Data.game.UnlockSteps_BoardgameM_Car);
				}
			}

			// (get) Token: 0x060013BF RID: 5055 RVA: 0x0007A779 File Offset: 0x00078979
			// (set) Token: 0x060013C0 RID: 5056 RVA: 0x0007A781 File Offset: 0x00078981
			public int UnlockSteps_BoardgameM_Ship
			{
				get
				{
					return this.unlockSteps_BoardgameM_Ship;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BoardgameM_Ship = value;
					if (this.unlockSteps_BoardgameM_Ship >= 1000)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_M_Ship);
					}
				}
			}

			// (get) Token: 0x060013C1 RID: 5057 RVA: 0x0007A7AA File Offset: 0x000789AA
			public static int UnlockStepsMissing__BoardgameM_Ship
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 1000 - Data.game.UnlockSteps_BoardgameM_Ship);
				}
			}

			// (get) Token: 0x060013C2 RID: 5058 RVA: 0x0007A7CB File Offset: 0x000789CB
			// (set) Token: 0x060013C3 RID: 5059 RVA: 0x0007A7D3 File Offset: 0x000789D3
			public int UnlockSteps_BoardgameM_TubaHat
			{
				get
				{
					return this.unlockSteps_BoardgameM_TubaHat;
				}
				set
				{
					if (GameplayMaster.IsCustomSeed())
					{
						return;
					}
					this.unlockSteps_BoardgameM_TubaHat = value;
					if (this.unlockSteps_BoardgameM_TubaHat >= 1000)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_M_Hat);
					}
				}
			}

			// (get) Token: 0x060013C4 RID: 5060 RVA: 0x0007A7FC File Offset: 0x000789FC
			public static int UnlockStepsMissing__BoardgameM_TubaHat
			{
				get
				{
					if (Data.game == null)
					{
						return 0;
					}
					return Mathf.Max(0, 1000 - Data.game.UnlockSteps_BoardgameM_TubaHat);
				}
			}

			// Token: 0x060013C5 RID: 5061 RVA: 0x0007A820 File Offset: 0x00078A20
			public static long Tracker_ModSymbolsCounter_Get(SymbolScript.Kind symbolKind)
			{
				if (Data.game == null)
				{
					return 0L;
				}
				switch (symbolKind)
				{
				case SymbolScript.Kind.lemon:
					return Data.game.modSymbolTriggersCounter_Lemons;
				case SymbolScript.Kind.cherry:
					return Data.game.modSymbolTriggersCounter_Cherries;
				case SymbolScript.Kind.clover:
					return Data.game.modSymbolTriggersCounter_Clovers;
				case SymbolScript.Kind.bell:
					return Data.game.modSymbolTriggersCounter_Bells;
				case SymbolScript.Kind.diamond:
					return Data.game.modSymbolTriggersCounter_Diamonds;
				case SymbolScript.Kind.coins:
					return Data.game.modSymbolTriggersCounter_Treasures;
				case SymbolScript.Kind.seven:
					return Data.game.modSymbolTriggersCounter_Sevens;
				case SymbolScript.Kind.six:
				case SymbolScript.Kind.nine:
					return 0L;
				default:
					Debug.LogError("Data.GameData.Tracker_ModSymbolsCounter_Get(): Symbol not handled: " + symbolKind.ToString());
					return 0L;
				}
			}

			// Token: 0x060013C6 RID: 5062 RVA: 0x0007A8D4 File Offset: 0x00078AD4
			public static void Tracker_ModSymbolsCounter_Set(SymbolScript.Kind symbolKind, long n)
			{
				if (Data.game == null)
				{
					return;
				}
				switch (symbolKind)
				{
				case SymbolScript.Kind.lemon:
					Data.game.modSymbolTriggersCounter_Lemons = n;
					return;
				case SymbolScript.Kind.cherry:
					Data.game.modSymbolTriggersCounter_Cherries = n;
					return;
				case SymbolScript.Kind.clover:
					Data.game.modSymbolTriggersCounter_Clovers = n;
					return;
				case SymbolScript.Kind.bell:
					Data.game.modSymbolTriggersCounter_Bells = n;
					return;
				case SymbolScript.Kind.diamond:
					Data.game.modSymbolTriggersCounter_Diamonds = n;
					return;
				case SymbolScript.Kind.coins:
					Data.game.modSymbolTriggersCounter_Treasures = n;
					return;
				case SymbolScript.Kind.seven:
					Data.game.modSymbolTriggersCounter_Sevens = n;
					return;
				case SymbolScript.Kind.six:
				case SymbolScript.Kind.nine:
					return;
				default:
					Debug.LogError("Data.GameData.Tracker_ModSymbolsCounter_Get(): Symbol not handled: " + symbolKind.ToString());
					return;
				}
			}

			// Token: 0x060013C7 RID: 5063 RVA: 0x0007A986 File Offset: 0x00078B86
			private void _TerminalNotificationsListEnsure()
			{
				if (this.terminalNotifications == null)
				{
					this.terminalNotifications = new List<Data.GameData.TerminalNotification>();
				}
			}

			// Token: 0x060013C8 RID: 5064 RVA: 0x0007A99B File Offset: 0x00078B9B
			public bool TerminalNotification_HasAny()
			{
				this._TerminalNotificationsListEnsure();
				return this.terminalNotifications.Count > 0;
			}

			// Token: 0x060013C9 RID: 5065 RVA: 0x0007A9B1 File Offset: 0x00078BB1
			public Data.GameData.TerminalNotification TerminaNotification_GetFirst(bool remove)
			{
				this._TerminalNotificationsListEnsure();
				if (this.terminalNotifications.Count == 0)
				{
					return null;
				}
				Data.GameData.TerminalNotification terminalNotification = this.terminalNotifications[0];
				if (remove)
				{
					this.terminalNotifications.RemoveAt(0);
				}
				return terminalNotification;
			}

			// Token: 0x060013CA RID: 5066 RVA: 0x0007A9E3 File Offset: 0x00078BE3
			public void TerminalNotification_Set(Data.GameData.TerminalNotification notification)
			{
				this._TerminalNotificationsListEnsure();
				this.terminalNotifications.Add(notification);
			}

			// (get) Token: 0x060013CB RID: 5067 RVA: 0x0007A9F7 File Offset: 0x00078BF7
			public bool AllCardsUnlocked
			{
				get
				{
					return this._allCardsUnlocked;
				}
			}

			// (get) Token: 0x060013CC RID: 5068 RVA: 0x0007A9FF File Offset: 0x00078BFF
			public bool AllCardsHolographic
			{
				get
				{
					return this._allCardsHolographic;
				}
			}

			// Token: 0x060013CD RID: 5069 RVA: 0x0007AA08 File Offset: 0x00078C08
			public static void AllCardsUnlockedCompute()
			{
				bool flag = true;
				int num = 20;
				for (int i = 0; i < num; i++)
				{
					if (Data.game.RunModifier_UnlockedTimes_Get((RunModifierScript.Identifier)i) <= 0 && i != 0)
					{
						flag = false;
						break;
					}
				}
				Data.game._allCardsUnlocked = flag;
			}

			// Token: 0x060013CE RID: 5070 RVA: 0x0007AA48 File Offset: 0x00078C48
			public static void AllCardsHolographicCompute()
			{
				bool flag = true;
				int num = 20;
				for (int i = 0; i < num; i++)
				{
					if (Data.game.RunModifier_FoilLevel_Get((RunModifierScript.Identifier)i) < 2)
					{
						flag = false;
						break;
					}
				}
				Data.game._allCardsHolographic = flag;
			}

			// Token: 0x060013CF RID: 5071 RVA: 0x0007AA84 File Offset: 0x00078C84
			private void RunMod_EnsureAllCapsules()
			{
				int num = 20;
				for (int i = 0; i < num; i++)
				{
					this.RunModCapsuleEnsure((RunModifierScript.Identifier)i);
				}
			}

			// Token: 0x060013D0 RID: 5072 RVA: 0x0007AAA8 File Offset: 0x00078CA8
			private void RunModCapsuleEnsure(RunModifierScript.Identifier identifier)
			{
				if (this.runModCapsulesDictionary.ContainsKey(identifier))
				{
					if (this.runModCapsulesDictionary[identifier] != null)
					{
						return;
					}
					this.runModCapsulesDictionary.Remove(identifier);
				}
				Data.GameData.RunModifierCapsule runModifierCapsule = new Data.GameData.RunModifierCapsule(identifier);
				this.runModCapsulesDictionary.Add(identifier, runModifierCapsule);
			}

			// Token: 0x060013D1 RID: 5073 RVA: 0x0007AAF5 File Offset: 0x00078CF5
			private Data.GameData.RunModifierCapsule _RunModifier_GetWorkingCapsule(RunModifierScript.Identifier identifier)
			{
				Data.game.RunModCapsuleEnsure(identifier);
				return Data.game.runModCapsulesDictionary[identifier];
			}

			// Token: 0x060013D2 RID: 5074 RVA: 0x0007AB14 File Offset: 0x00078D14
			public int RunModifier_OwnedCount_Get(RunModifierScript.Identifier identifier)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return 0;
				}
				if (identifier == RunModifierScript.Identifier.defaultModifier)
				{
					return -1;
				}
				return runModifierCapsule.ownedCount;
			}

			// Token: 0x060013D3 RID: 5075 RVA: 0x0007AB3C File Offset: 0x00078D3C
			public void RunModifier_OwnedCount_Set(RunModifierScript.Identifier identifier, int n)
			{
				if (identifier == RunModifierScript.Identifier.defaultModifier)
				{
					return;
				}
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return;
				}
				runModifierCapsule.ownedCount = n;
			}

			// Token: 0x060013D4 RID: 5076 RVA: 0x0007AB60 File Offset: 0x00078D60
			public int RunModifier_UnlockedTimes_Get(RunModifierScript.Identifier identifier)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return 0;
				}
				return runModifierCapsule.unlockedTimes;
			}

			// Token: 0x060013D5 RID: 5077 RVA: 0x0007AB80 File Offset: 0x00078D80
			public void RunModifier_UnlockedTimes_Set(RunModifierScript.Identifier identifier, int n)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return;
				}
				runModifierCapsule.unlockedTimes = n;
				Data.GameData.AllCardsUnlockedCompute();
			}

			// Token: 0x060013D6 RID: 5078 RVA: 0x0007ABA8 File Offset: 0x00078DA8
			public int RunModifier_PlayedTimes_Get(RunModifierScript.Identifier identifier)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return 0;
				}
				return runModifierCapsule.playedTimes;
			}

			// Token: 0x060013D7 RID: 5079 RVA: 0x0007ABC8 File Offset: 0x00078DC8
			public void RunModifier_PlayedTimes_Set(RunModifierScript.Identifier identifier, int n)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return;
				}
				runModifierCapsule.playedTimes = n;
			}

			// Token: 0x060013D8 RID: 5080 RVA: 0x0007ABE8 File Offset: 0x00078DE8
			public int RunModifier_WonTimes_Get(RunModifierScript.Identifier identifier)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return 0;
				}
				return runModifierCapsule.wonTimes;
			}

			// Token: 0x060013D9 RID: 5081 RVA: 0x0007AC08 File Offset: 0x00078E08
			public void RunModifier_WonTimes_Set(RunModifierScript.Identifier identifier, int n)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return;
				}
				runModifierCapsule.wonTimes = n;
				if (identifier == RunModifierScript.Identifier.bigDebt)
				{
					PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Omnipotent);
				}
			}

			// Token: 0x060013DA RID: 5082 RVA: 0x0007AC38 File Offset: 0x00078E38
			public int RunModifier_WonTimesHARDCORE_Get(RunModifierScript.Identifier identifier)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return 0;
				}
				return runModifierCapsule.wonTimesHardcore;
			}

			// Token: 0x060013DB RID: 5083 RVA: 0x0007AC58 File Offset: 0x00078E58
			public void RunModifier_WonTimesHARDCORE_Set(RunModifierScript.Identifier identifier, int n)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return;
				}
				runModifierCapsule.wonTimesHardcore = n;
				if (identifier == RunModifierScript.Identifier.bigDebt)
				{
					PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Omnipotent);
				}
			}

			// Token: 0x060013DC RID: 5084 RVA: 0x0007AC88 File Offset: 0x00078E88
			public int RunModifier_FoilLevel_Get(RunModifierScript.Identifier identifier)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return 0;
				}
				return runModifierCapsule.foilLevel;
			}

			// Token: 0x060013DD RID: 5085 RVA: 0x0007ACA8 File Offset: 0x00078EA8
			public void RunModifier_FoilLevel_Set(RunModifierScript.Identifier identifier, int n)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return;
				}
				runModifierCapsule.foilLevel = n;
				Data.GameData.AllCardsHolographicCompute();
			}

			// Token: 0x060013DE RID: 5086 RVA: 0x0007ACD0 File Offset: 0x00078ED0
			public bool RunModifier_HardcoreMode_Get(RunModifierScript.Identifier identifier)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				return runModifierCapsule != null && runModifierCapsule.isInHardcoreMode;
			}

			// Token: 0x060013DF RID: 5087 RVA: 0x0007ACF0 File Offset: 0x00078EF0
			public void RunModifier_HardcoreMode_Set(RunModifierScript.Identifier identifier, bool value)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return;
				}
				runModifierCapsule.isInHardcoreMode = value;
			}

			// Token: 0x060013E0 RID: 5088 RVA: 0x0007AD10 File Offset: 0x00078F10
			public int RunModifier_UnlockedTotalNumber()
			{
				int num = 0;
				foreach (KeyValuePair<RunModifierScript.Identifier, Data.GameData.RunModifierCapsule> keyValuePair in this.runModCapsulesDictionary)
				{
					if (keyValuePair.Key != RunModifierScript.Identifier.undefined && keyValuePair.Key != RunModifierScript.Identifier.count && keyValuePair.Value != null && keyValuePair.Value.unlockedTimes > 0)
					{
						num += Mathf.Max(0, keyValuePair.Value.unlockedTimes);
					}
				}
				return num;
			}

			// Token: 0x060013E1 RID: 5089 RVA: 0x0007ADA4 File Offset: 0x00078FA4
			public int RunModifier_UnlockedOnce_TotalNumber()
			{
				int num = 0;
				foreach (KeyValuePair<RunModifierScript.Identifier, Data.GameData.RunModifierCapsule> keyValuePair in this.runModCapsulesDictionary)
				{
					if (keyValuePair.Key != RunModifierScript.Identifier.undefined && keyValuePair.Key != RunModifierScript.Identifier.count && keyValuePair.Value != null && (keyValuePair.Value.GetIdentifier() == RunModifierScript.Identifier.defaultModifier || keyValuePair.Value.unlockedTimes > 0))
					{
						num++;
					}
				}
				return num;
			}

			// Token: 0x060013E2 RID: 5090 RVA: 0x0007AE34 File Offset: 0x00079034
			public int RunModifier_WonOnce_TotalNumber()
			{
				int num = 0;
				foreach (KeyValuePair<RunModifierScript.Identifier, Data.GameData.RunModifierCapsule> keyValuePair in this.runModCapsulesDictionary)
				{
					if (keyValuePair.Key != RunModifierScript.Identifier.undefined && keyValuePair.Key != RunModifierScript.Identifier.count && keyValuePair.Value != null && keyValuePair.Value.wonTimes > 0)
					{
						num++;
					}
				}
				return num;
			}

			// Token: 0x060013E3 RID: 5091 RVA: 0x0007AEB8 File Offset: 0x000790B8
			public int RunModifier_InHolographicCondition_TotalNumber()
			{
				int num = 0;
				foreach (KeyValuePair<RunModifierScript.Identifier, Data.GameData.RunModifierCapsule> keyValuePair in this.runModCapsulesDictionary)
				{
					if (keyValuePair.Key != RunModifierScript.Identifier.undefined && keyValuePair.Key != RunModifierScript.Identifier.count && keyValuePair.Value != null && (keyValuePair.Value.wonTimesHardcore > 0 || keyValuePair.Value.foilLevel >= 2))
					{
						num++;
					}
				}
				return num;
			}

			// Token: 0x060013E4 RID: 5092 RVA: 0x0007AF48 File Offset: 0x00079148
			public int RunModifier_OwnedCopiesTotalNumber()
			{
				int num = 0;
				foreach (KeyValuePair<RunModifierScript.Identifier, Data.GameData.RunModifierCapsule> keyValuePair in this.runModCapsulesDictionary)
				{
					if (keyValuePair.Key != RunModifierScript.Identifier.undefined && keyValuePair.Key != RunModifierScript.Identifier.count && keyValuePair.Value != null && keyValuePair.Value.ownedCount > 0)
					{
						num += Mathf.Max(0, keyValuePair.Value.ownedCount);
					}
				}
				return num;
			}

			// Token: 0x060013E5 RID: 5093 RVA: 0x0007AFDC File Offset: 0x000791DC
			private void _RunMod_DictAndListEnsure()
			{
				if (this.runModCapsulesDictionary == null)
				{
					this.runModCapsulesDictionary = new Dictionary<RunModifierScript.Identifier, Data.GameData.RunModifierCapsule>();
				}
				if (this._runModSavingList == null)
				{
					this._runModSavingList = new List<Data.GameData.RunModifierCapsule>();
				}
			}

			// Token: 0x060013E6 RID: 5094 RVA: 0x0007B004 File Offset: 0x00079204
			private void _RunModifiers_SavePrepare()
			{
				this._RunMod_DictAndListEnsure();
				this.RunMod_EnsureAllCapsules();
				this._runModSavingList.Clear();
				foreach (KeyValuePair<RunModifierScript.Identifier, Data.GameData.RunModifierCapsule> keyValuePair in this.runModCapsulesDictionary)
				{
					this._runModSavingList.Add(keyValuePair.Value);
				}
			}

			// Token: 0x060013E7 RID: 5095 RVA: 0x0007B07C File Offset: 0x0007927C
			private void _RunModifiers_LoadPrepare()
			{
				this._RunMod_DictAndListEnsure();
				for (int i = 0; i < this._runModSavingList.Count; i++)
				{
					if (this._runModSavingList[i] != null)
					{
						RunModifierScript.Identifier identifier = this._runModSavingList[i].GetIdentifier();
						if (identifier != RunModifierScript.Identifier.undefined && identifier != RunModifierScript.Identifier.count && !this.runModCapsulesDictionary.ContainsKey(identifier))
						{
							this.runModCapsulesDictionary.Add(identifier, this._runModSavingList[i]);
						}
					}
				}
				this.RunMod_EnsureAllCapsules();
			}

			public static Data.GameData inst = null;

			public int myGameDataIndex = -1;

			public int dataOpenedTimes;

			public string lastGameVersionThatSavedMe;

			public bool enforceRunReset;

			public bool gameplayDataHasSession;

			public GameplayData gameplayData = new GameplayData();

			public int runsDone;

			public int deathsDone;

			public bool tutorialQuestionEnabled = true;

			public bool[] drawersUnlocked = new bool[4];

			public int doorOpenedCounter;

			public int badEndingCounter;

			public int goodEndingCounter;

			public bool creditsSeenOnce;

			public bool bookedBadEndingDialogue;

			public bool demoVoucherUnlocked;

			public bool jumperinoScarinoDoorino_DoneOnce;

			[SerializeField]
			private int persistentStat_666SeenTimes;

			private List<PowerupScript.Identifier> unlockedPowerups = Data.GameData._UnlockedPowerups_Definition();

			[SerializeField]
			private string _unlockedPowerupsString;

			private List<PowerupScript.Identifier> lockedPowerups = Data.GameData._LockedPowerups_Definition();

			private List<PowerupScript.Identifier> lockedPowerups_ResultingList = Data.GameData._LockedPowerups_ResultingList_Definition();

			private bool allPowerupsUnlockedChacheResult;

			public bool hasEverUnlockedAPowerup;

			public const int UNLSTEPS_MAX_PUPPET_TRAINER = 15;

			public const int UNLSTEPS_MAX_PUPPET_ELECTRICIAN = 15;

			public const int UNLSTEPS_MAX_PUPPET_FORTUNE_TELLER = 15;

			public const int UNLSTEPS_MAX_STEPS_COUNTER = 20;

			public const int UNLSTEPS_MAX_DEVIL_HORN = 3;

			public const int UNLSTEPS_MAX_NECRONOMICON = 5;

			public const int UNLSTEPS_MAX_KING_MIDA = 30;

			public const int UNLSTEPS_MAX_DEALER = 30;

			public const int UNLSTEPS_MAX_RAGING_CAPITALIST = 30;

			private const int UNLSTEPS_MAX_ELECTRICITY_COUNTER = 50;

			[SerializeField]
			private int unlockSteps_ElectricityCounter;

			private const int UNLSTEPS_MAX_DARK_LOTUS = 200;

			[SerializeField]
			private int unlockSteps_DarkLotus;

			private const int UNLSTEPS_MAX_CLOVERS_LAND_PATCH = 50;

			[SerializeField]
			private int unlockSteps_CloversLandPatch;

			private const int UNLSTEPS_MAX_ALL_IN = 100;

			[SerializeField]
			private int unlockSteps_AllIn;

			private const int UNLSTEPS_MAX_GARBAGE = 50;

			[SerializeField]
			private int unlockSteps_Garbage;

			private const int UNLSTEPS_MAX_VOICE_MAIL = 50;

			[SerializeField]
			private int unlockSteps_VoiceMail;

			private const int UNLSTEPS_MAX_FORTUNE_CHANNELER = 100;

			[SerializeField]
			private int unlockSteps_FortuneChanneler;

			private const int UNLSTEPS_MAX_HAMSA_UPSIDE = 100;

			[SerializeField]
			private int unlockSteps_HamsaUpside;

			private const int UNLSTEPS_MAX_ANCIENT_COIN = 100;

			[SerializeField]
			private int unlockSteps_AncientCoin;

			private const int UNLSTEPS_MAX_SARDINES = 200;

			[SerializeField]
			private int unlockSteps_Sardines;

			private const int UNLSTEPS_MAX_ROSARY = 10;

			[SerializeField]
			private int unlockSteps_Rosary;

			private const int UNLSTEPS_MAX_FORTUNE_COOKIE = 10;

			[SerializeField]
			private int unlockSteps_FortuneCookie;

			private const int UNLSTEPS_MAX_YELLOW_STAR = 10;

			[SerializeField]
			private int unlockSteps_YellowStar;

			private const int UNLSTEPS_MAX_CROSS = 100;

			[SerializeField]
			private int unlockSteps_Cross;

			private const int UNLSTEPS_MAX_CALENDAR = 30;

			[SerializeField]
			private int unlockSteps_Calendar;

			private const int UNLSTEPS_MAX_PAINKILLERS = 50;

			[SerializeField]
			private int unlockSteps_PainKillers;

			private const int UNLSTEPS_MAX_SCRATCH_AND_WIN = 25;

			[SerializeField]
			private int unlockSteps_ScratchAndWin;

			private const int UNLSTEPS_MAX_PHOTO_BOOK = 50;

			[SerializeField]
			private int unlockSteps_PhotoBook;

			private const int UNLSTEPS_MAX_BAPHOMET = 666;

			[SerializeField]
			private int unlockSteps_Baphomet;

			private const int UNLSTEPS_MAX_DUNG_BEETLE_STERCORARO = 300;

			[SerializeField]
			private int unlockSteps_DungBeetleStercoRaro;

			private const int UNLSTEPS_MAX_LUCKY_CAT_FAT = 50;

			[SerializeField]
			private int unlockSteps_LuckyCatFat;

			private const int UNLSTEPS_MAX_LUCKY_CAT_SWOLE = 25;

			[SerializeField]
			private int unlockSteps_LuckyCatSwole;

			private const int UNLSTEPS_MAX_GOLDEN_HORSESHOE = 7;

			[SerializeField]
			private int unlockSteps_HorseShoeGold;

			private const int UNLSTEPS_MAX_GOLDEN_PEPPER = 30;

			[SerializeField]
			private int unlockSteps_GoldenPepper;

			private const int UNLSTEPS_MAX_ROTTEN_PEPPER = 20;

			[SerializeField]
			private int unlockSteps_RottenPepper;

			private const int UNLSTEPS_MAX_BELL_PEPPER = 30;

			[SerializeField]
			private int unlockSteps_BellPepper;

			private const int UNLSTEPS_MAX_ABYSSU = 100;

			[SerializeField]
			private int unlockSteps_Abyssu;

			private const int UNLSTEPS_MAX_VORAGO = 100;

			[SerializeField]
			private int unlockSteps_Vorago;

			private const int UNLSTEPS_MAX_BARATHRUM = 100;

			[SerializeField]
			private int unlockSteps_Barathrum;

			private const int UNLSTEPS_MAX_SUPER_CAPACITOR = 200;

			[SerializeField]
			private int unlockSteps_SuperCapacitor;

			private const int UNLSTEPS_MAX_CRANK_GENERATOR = 50;

			[SerializeField]
			private int unlockSteps_CrankGenerator;

			private const int UNLSTEPS_MAX_BRICKS = 50;

			[SerializeField]
			private int unlockSteps_BoardgameC_Bricks;

			private const int UNLSTEPS_MAX_WOOD = 50;

			[SerializeField]
			private int unlockSteps_BoardgameC_Wood;

			private const int UNLSTEPS_MAX_SHEEP = 500;

			[SerializeField]
			private int unlockSteps_BoardgameC_Sheep;

			private const int UNLSTEPS_MAX_WHEAT = 500;

			[SerializeField]
			private int unlockSteps_BoardgameC_Wheat;

			private const int UNLSTEPS_MAX_STONE = 1000;

			[SerializeField]
			private int unlockSteps_BoardgameC_Stone;

			private const int UNLSTEPS_MAX_HARBOR = 1000;

			[SerializeField]
			private int unlockSteps_BoardgameC_Harbor;

			private const int UNLSTEPS_MAX_THIEF = 1000;

			[SerializeField]
			private int unlockSteps_BoardgameC_Thief;

			private const int UNLSTEPS_MAX_CARRIOLA = 500;

			[SerializeField]
			private int unlockSteps_BoardgameM_Carriola;

			private const int UNLSTEPS_MAX_SHOE = 500;

			[SerializeField]
			private int unlockSteps_BoardgameM_Shoe;

			private const int UNLSTEPS_MAX_DITALE = 50;

			[SerializeField]
			private int unlockSteps_BoardgameM_Ditale;

			private const int UNLSTEPS_MAX_IRON = 50;

			[SerializeField]
			private int unlockSteps_BoardgameM_Iron;

			private const int UNLSTEPS_MAX_CAR = 1000;

			[SerializeField]
			private int unlockSteps_BoardgameM_Car;

			private const int UNLSTEPS_MAX_SHIP = 1000;

			[SerializeField]
			private int unlockSteps_BoardgameM_Ship;

			private const int UNLSTEPS_MAX_TUBA_HAT = 1000;

			[SerializeField]
			private int unlockSteps_BoardgameM_TubaHat;

			[SerializeField]
			private long modSymbolTriggersCounter_Lemons;

			[SerializeField]
			private long modSymbolTriggersCounter_Cherries;

			[SerializeField]
			private long modSymbolTriggersCounter_Clovers;

			[SerializeField]
			private long modSymbolTriggersCounter_Bells;

			[SerializeField]
			private long modSymbolTriggersCounter_Diamonds;

			[SerializeField]
			private long modSymbolTriggersCounter_Treasures;

			[SerializeField]
			private long modSymbolTriggersCounter_Sevens;

			public static PowerupScript.Identifier _terminalNotificationStringPowerup = PowerupScript.Identifier.undefined;

			[SerializeField]
			private List<Data.GameData.TerminalNotification> terminalNotifications = new List<Data.GameData.TerminalNotification>();

			[SerializeField]
			private bool _allCardsUnlocked;

			[SerializeField]
			private bool _allCardsHolographic;

			[NonSerialized]
			private Dictionary<RunModifierScript.Identifier, Data.GameData.RunModifierCapsule> runModCapsulesDictionary = new Dictionary<RunModifierScript.Identifier, Data.GameData.RunModifierCapsule>();

			[SerializeField]
			private List<Data.GameData.RunModifierCapsule> _runModSavingList = new List<Data.GameData.RunModifierCapsule>();

			[Serializable]
			public class TerminalNotification
			{
				// Token: 0x060014E4 RID: 5348 RVA: 0x000808C7 File Offset: 0x0007EAC7
				public TerminalNotification(PowerupScript.Identifier powerupIdentifier, string titleKey, string messageKey)
				{
					this.powerupIdentifierAsString = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(powerupIdentifier);
					this.titleKey = titleKey;
					this.messageKey = messageKey;
				}

				// Token: 0x060014E5 RID: 5349 RVA: 0x000808E9 File Offset: 0x0007EAE9
				public string GetTitle()
				{
					return Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get(this.titleKey), Strings.SanitizationSubKind.none);
				}

				// Token: 0x060014E6 RID: 5350 RVA: 0x000808FD File Offset: 0x0007EAFD
				public string GetMessage()
				{
					Data.GameData._terminalNotificationStringPowerup = PlatformDataMaster.EnumEntryFromString<PowerupScript.Identifier>(this.powerupIdentifierAsString, PowerupScript.Identifier.undefined);
					return Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get(this.messageKey), Strings.SanitizationSubKind.none);
				}

				// Token: 0x060014E7 RID: 5351 RVA: 0x00080922 File Offset: 0x0007EB22
				public PowerupScript.Identifier GetPowerupIdentifier()
				{
					return PlatformDataMaster.EnumEntryFromString<PowerupScript.Identifier>(this.powerupIdentifierAsString, PowerupScript.Identifier.undefined);
				}

				[SerializeField]
				private string powerupIdentifierAsString;

				[SerializeField]
				private string titleKey;

				[SerializeField]
				private string messageKey;
			}

			[Serializable]
			public class RunModifierCapsule
			{
				// Token: 0x060014E8 RID: 5352 RVA: 0x00080930 File Offset: 0x0007EB30
				public RunModifierCapsule(RunModifierScript.Identifier identifier)
				{
					this.runModifierIdentifierAsString = PlatformDataMaster.EnumEntryToString<RunModifierScript.Identifier>(identifier);
				}

				// Token: 0x060014E9 RID: 5353 RVA: 0x00080944 File Offset: 0x0007EB44
				public RunModifierScript.Identifier GetIdentifier()
				{
					return PlatformDataMaster.EnumEntryFromString<RunModifierScript.Identifier>(this.runModifierIdentifierAsString, RunModifierScript.Identifier.undefined);
				}

				[SerializeField]
				private string runModifierIdentifierAsString;

				[SerializeField]
				public int ownedCount;

				[SerializeField]
				public int unlockedTimes;

				[SerializeField]
				public int playedTimes;

				[SerializeField]
				public int wonTimes;

				[SerializeField]
				public int wonTimesHardcore;

				[SerializeField]
				public int foilLevel;

				[SerializeField]
				public bool isInHardcoreMode;
			}
		}
	}
}
