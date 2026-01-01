using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Panik
{
	// Token: 0x0200012C RID: 300
	public static class Data
	{
		// Token: 0x06000EBF RID: 3775 RVA: 0x00011F44 File Offset: 0x00010144
		public static string PAchievementsDataGet()
		{
			return "afhjttiojd?s0989sdfl12";
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00011F4B File Offset: 0x0001014B
		public static string PGameDataGet(int oldVersionNumber, int gameDataVersion)
		{
			if (gameDataVersion < 2)
			{
				return null;
			}
			return "uoiyiuh_+=-5216gh;lj??!/345";
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x00011F58 File Offset: 0x00010158
		public static string PGameDataGet_LastOne(int oldVersionNumber)
		{
			return Data.PGameDataGet(oldVersionNumber, 3);
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x00011F61 File Offset: 0x00010161
		public static string Encrypt_Wrapped(string input, string password)
		{
			return PlatformDataMaster.EncryptCustom(input, password);
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x00011F6A File Offset: 0x0001016A
		public static string Decrypt_Wrapped(string input, string password)
		{
			return PlatformDataMaster.DecryptCustom(input, password);
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x00011F73 File Offset: 0x00010173
		public static Data.VersionData versionData
		{
			get
			{
				return Data.VersionData.inst;
			}
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0006C508 File Offset: 0x0006A708
		public static async UniTask<bool> _VersionsLoadAndSave()
		{
			string text = await PlatformDataMaster.LoadVersionData();
			if (text != null)
			{
				bool flag = false;
				Data.VersionData versionData = PlatformDataMaster.FromJson<Data.VersionData>(text, out flag);
				if (versionData != null)
				{
					Data.VersionData.settingsVersion_LoadedBackup = versionData.settingsVersion;
					Data.VersionData.controlsVersion_LoadedBackup = versionData.controlsVersion;
					Data.VersionData.gameVersion_LoadedBackup = versionData.gameVersion;
				}
			}
			text = PlatformDataMaster.ToJson<Data.VersionData>(Data.versionData);
			UniTask<bool> uniTask = await PlatformDataMaster.SaveVersionData(text);
			if (Data.versionData.settingsVersion != Data.VersionData.settingsVersion_LoadedBackup)
			{
				Data.OnVersionChange onDataVersionChage_Settings = Data.OnDataVersionChage_Settings;
				if (onDataVersionChage_Settings != null)
				{
					onDataVersionChage_Settings(Data.VersionData.settingsVersion_LoadedBackup, Data.versionData.settingsVersion);
				}
			}
			if (Data.versionData.controlsVersion != Data.VersionData.controlsVersion_LoadedBackup)
			{
				Data.OnVersionChange onDataVersionChange_Controls = Data.OnDataVersionChange_Controls;
				if (onDataVersionChange_Controls != null)
				{
					onDataVersionChange_Controls(Data.VersionData.controlsVersion_LoadedBackup, Data.versionData.controlsVersion);
				}
			}
			if (Data.versionData.gameVersion != Data.VersionData.gameVersion_LoadedBackup)
			{
				Data.OnVersionChange onDataVersionChange_Game = Data.OnDataVersionChange_Game;
				if (onDataVersionChange_Game != null)
				{
					onDataVersionChange_Game(Data.VersionData.gameVersion_LoadedBackup, Data.versionData.gameVersion);
				}
			}
			return uniTask;
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x00011F7A File Offset: 0x0001017A
		private static void OnSettingsVersionChange(int oldVersionNumber, int newVersionNumber)
		{
			if (oldVersionNumber != newVersionNumber)
			{
				Data.settingsResetFlag = true;
			}
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0006C544 File Offset: 0x0006A744
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

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0006C684 File Offset: 0x0006A884
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

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x00011F86 File Offset: 0x00010186
		public static Data.SettingsData settings
		{
			get
			{
				return Data.SettingsData.inst;
			}
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00011F8D File Offset: 0x0001018D
		public static void ApplySettings(bool applyResolution, bool pushControlsJsonToMap)
		{
			Data.settings.Apply(applyResolution, pushControlsJsonToMap);
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000ECB RID: 3787 RVA: 0x00011F9B File Offset: 0x0001019B
		public static Data.GameData game
		{
			get
			{
				return Data.GameData.inst;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000ECC RID: 3788 RVA: 0x00011FA2 File Offset: 0x000101A2
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

		// Token: 0x06000ECD RID: 3789 RVA: 0x00011FB7 File Offset: 0x000101B7
		public static bool JsonErrorWhileLoadingGame_Get()
		{
			return Data.errorFlag_JsonGameDataError;
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x0006C6F0 File Offset: 0x0006A8F0
		public static async UniTask<bool> SaveSettings()
		{
			Data.settings._PrepareForSaving();
			return await PlatformDataMaster.SaveSettingsData(PlatformDataMaster.ToJson<Data.SettingsData>(Data.SettingsData.inst));
		}

		// Token: 0x06000ECF RID: 3791 RVA: 0x0006C72C File Offset: 0x0006A92C
		public static async UniTask<bool> SaveSettingsAndApply(bool applyResolution)
		{
			UniTask<bool>.Awaiter awaiter = Data.SaveSettings().GetAwaiter();
			if (!awaiter.IsCompleted)
			{
				await awaiter;
				UniTask<bool>.Awaiter awaiter2;
				awaiter = awaiter2;
				awaiter2 = default(UniTask<bool>.Awaiter);
			}
			bool flag;
			if (!awaiter.GetResult())
			{
				flag = false;
			}
			else
			{
				Data.settings.Apply(applyResolution, false);
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000ED0 RID: 3792 RVA: 0x0006C770 File Offset: 0x0006A970
		public static async UniTask<bool> LoadSettingsAndApply()
		{
			Data.SettingsData _data = null;
			bool loadedFromDisk = false;
			bool settingsVersionIsCorrect = true;
			if (Data.versionData.settingsVersion != Data.VersionData.settingsVersion_LoadedBackup && !Data.VersionData.settingsVersionFixed)
			{
				Data.VersionData.settingsVersionFixed = true;
				Data.VersionData.controlsVersionFixed = true;
				settingsVersionIsCorrect = false;
				Debug.LogWarning("Settings: Version mismatch. Loading default settings. This also resetted the controls.");
			}
			bool controlsVersionIsCorrect = true;
			if (Data.versionData.controlsVersion != Data.VersionData.controlsVersion_LoadedBackup && !Data.VersionData.controlsVersionFixed)
			{
				Data.VersionData.controlsVersionFixed = true;
				controlsVersionIsCorrect = false;
				Debug.LogWarning("Controls: Version mismatch. Loading default controls.");
			}
			if (settingsVersionIsCorrect)
			{
				string text = await PlatformDataMaster.LoadSettingsData();
				bool flag = false;
				if (!string.IsNullOrEmpty(text))
				{
					_data = PlatformDataMaster.FromJson<Data.SettingsData>(text, out flag);
				}
			}
			if (_data == null)
			{
				_data = new Data.SettingsData();
			}
			else
			{
				loadedFromDisk = true;
			}
			Data.SettingsData.inst = _data;
			if (!controlsVersionIsCorrect)
			{
				Data.SettingsData.inst.controlMapsJson = null;
			}
			Data.settings.Apply(true, true);
			if (!controlsVersionIsCorrect || !settingsVersionIsCorrect)
			{
				Controls.MapsRestoreDefault_AllPlayers(true, true, true);
			}
			if (!settingsVersionIsCorrect || !controlsVersionIsCorrect)
			{
				Data.SaveSettings();
			}
			return loadedFromDisk;
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x0006C7AC File Offset: 0x0006A9AC
		public static async UniTask<bool> DeleteSettingsAndReset()
		{
			await PlatformDataMaster.DeleteSettingsData();
			Data.SettingsData.inst = new Data.SettingsData();
			Controls.MapsRestoreDefault_AllPlayers(true, true, true);
			return await Data.SaveSettingsAndApply(true);
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x0006C7E8 File Offset: 0x0006A9E8
		public static async UniTask<bool> SaveAchievements()
		{
			PlatformAPI.achievementsData.Saving_Prepare();
			return await PlatformDataMaster.SaveAchievementsData(PlatformDataMaster.EncryptCustom(PlatformDataMaster.ToJson<PlatformAPI.AchievementsData>(PlatformAPI.achievementsData), Data.PAchievementsDataGet()));
		}

		// Token: 0x06000ED3 RID: 3795 RVA: 0x0006C824 File Offset: 0x0006AA24
		public static async UniTask<bool> LoadAchievements()
		{
			PlatformAPI.AchievementsData loadedData = null;
			bool loadedFromDisk = false;
			string text = await PlatformDataMaster.LoadAchievementsData();
			if (!string.IsNullOrEmpty(text))
			{
				text = PlatformDataMaster.DecryptCustom(text, Data.PAchievementsDataGet());
				bool flag = false;
				loadedData = PlatformDataMaster.FromJson<PlatformAPI.AchievementsData>(text, out flag);
			}
			if (loadedData == null)
			{
				loadedData = new PlatformAPI.AchievementsData();
			}
			else
			{
				loadedFromDisk = true;
			}
			loadedData.Load_Restore();
			PlatformAPI.achievementsData = loadedData;
			return loadedFromDisk;
		}

		// Token: 0x06000ED4 RID: 3796 RVA: 0x0006C860 File Offset: 0x0006AA60
		public static async UniTask<bool> DeleteAchievements(string debugReason, bool forceInRelease)
		{
			object obj = await PlatformAPI.AchievementsReset(debugReason, forceInRelease);
			bool item = obj.Item1;
			bool item2 = obj.Item2;
			bool flag;
			if (!item && !item2)
			{
				Debug.LogError("Data::DeleteAchievements(..): Reset task failed on both flags. Aborting.");
				flag = false;
			}
			else if (!item)
			{
				Debug.LogError("Data::DeleteAchievements(..): Reset task failed on offline flag. Aborting.");
				flag = false;
			}
			else
			{
				await PlatformDataMaster.DeleteAchievementsData();
				flag = await Data.SaveAchievements();
			}
			return flag;
		}

		// Token: 0x06000ED5 RID: 3797 RVA: 0x0006C8AC File Offset: 0x0006AAAC
		public static async UniTask<bool> SaveGame(Data.GameSavingReason reasonForSaving, int gameDataIndex = -1)
		{
			int num = gameDataIndex;
			bool flag;
			if (reasonForSaving == Data.GameSavingReason._undefined || reasonForSaving == Data.GameSavingReason._count)
			{
				string text = "Data.SaveGame(): provide a valid game-saving reason!";
				Debug.LogError(text);
				ConsolePrompt.LogError(text, "", 0f);
				flag = false;
			}
			else if (GameplayMaster.DeathCountdownHasStarted() && reasonForSaving != Data.GameSavingReason.death)
			{
				flag = false;
			}
			else if (GameplayMaster.IsCustomSeed())
			{
				flag = false;
			}
			else
			{
				if (num < 0)
				{
					num = Data.GameDataIndex;
				}
				if (num < 0)
				{
					num = 0;
					Debug.LogWarning("Data.SaveGame(): Data index to save is below 0. Saving game data index: 0");
				}
				if (Data.GameData.inst == null)
				{
					string text2 = "Data.SaveGame(): There is no GameData instance to save!";
					ConsolePrompt.LogError(text2, "", 0f);
					Debug.LogError(text2);
					flag = false;
				}
				else
				{
					bool flag2 = reasonForSaving != Data.GameSavingReason.death && reasonForSaving != Data.GameSavingReason.endingWithoutDeath && reasonForSaving != Data.GameSavingReason.setFlag_RunForceReset;
					Data.game.Saving_Prepare(flag2, reasonForSaving);
					string text3 = PlatformDataMaster.ToJson<Data.GameData>(Data.GameData.inst);
					string text4 = Data.PGameDataGet(Data.VersionData.gameVersion_LoadedBackup, Data.versionData.gameVersion);
					if (!string.IsNullOrEmpty(text4))
					{
						text3 = Data.Encrypt_Wrapped(text3, text4);
					}
					flag = await PlatformDataMaster.SaveGameData(text3, num);
				}
			}
			return flag;
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x0006C8F8 File Offset: 0x0006AAF8
		public static async UniTask<bool> LoadGame(int gameDataIndex, bool forceLoadSameFile)
		{
			bool flag = Data.GameDataIndex == gameDataIndex;
			bool reloading = flag && forceLoadSameFile;
			bool flag2;
			if (!forceLoadSameFile && flag)
			{
				flag2 = false;
			}
			else
			{
				Data.errorFlag_JsonGameDataError = false;
				Data.GameData _data = null;
				bool loadedFromDisk = false;
				string text = await PlatformDataMaster.LoadGameData(gameDataIndex);
				if (!string.IsNullOrEmpty(text))
				{
					string text2 = Data.PGameDataGet(Data.VersionData.gameVersion_LoadedBackup, Data.versionData.gameVersion);
					if (!string.IsNullOrEmpty(text2))
					{
						text = Data.Decrypt_Wrapped(text, text2);
					}
					_data = PlatformDataMaster.FromJson<Data.GameData>(text, out Data.errorFlag_JsonGameDataError);
				}
				if (_data == null)
				{
					_data = new Data.GameData(gameDataIndex);
				}
				else
				{
					loadedFromDisk = true;
				}
				Data.GameData.inst = _data;
				Data.GameData.inst.Loading_Prepare();
				if (!reloading)
				{
					Data.GameData.inst.dataOpenedTimes++;
				}
				flag2 = loadedFromDisk;
			}
			return flag2;
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x0006C944 File Offset: 0x0006AB44
		public static async UniTask<bool> DeleteGameData(int gameDataIndex)
		{
			UniTask<bool>.Awaiter awaiter = PlatformDataMaster.DeleteGameData(gameDataIndex).GetAwaiter();
			if (!awaiter.IsCompleted)
			{
				await awaiter;
				UniTask<bool>.Awaiter awaiter2;
				awaiter = awaiter2;
				awaiter2 = default(UniTask<bool>.Awaiter);
			}
			bool flag;
			if (!awaiter.GetResult())
			{
				flag = false;
			}
			else
			{
				Data.GameData.inst = new Data.GameData(gameDataIndex);
				flag = true;
			}
			return flag;
		}

		// Token: 0x04000F52 RID: 3922
		public const int GAME_DATA_MAX_NUMBER = 1;

		// Token: 0x04000F53 RID: 3923
		public static Data.OnVersionChange OnDataVersionChage_Settings = new Data.OnVersionChange(Data.OnSettingsVersionChange);

		// Token: 0x04000F54 RID: 3924
		public static Data.OnVersionChange OnDataVersionChange_Controls = null;

		// Token: 0x04000F55 RID: 3925
		public static Data.OnVersionChange OnDataVersionChange_Game = new Data.OnVersionChange(Data.OnGameVersionChange);

		// Token: 0x04000F56 RID: 3926
		public static bool settingsResetFlag = false;

		// Token: 0x04000F57 RID: 3927
		public static bool publisherBuildFlag_FromFirstToSecond = false;

		// Token: 0x04000F58 RID: 3928
		public static bool redButtonChange_ShowPopUps_ForV0_4 = false;

		// Token: 0x04000F59 RID: 3929
		private static bool errorFlag_JsonGameDataError = false;

		// Token: 0x0200012D RID: 301
		[Serializable]
		public class VersionData
		{
			// Token: 0x04000F5A RID: 3930
			public static Data.VersionData inst = new Data.VersionData();

			// Token: 0x04000F5B RID: 3931
			public const int DESIRED_VERSION_SETTINGS = 4;

			// Token: 0x04000F5C RID: 3932
			public const int DESIRED_VERSION_CONTROLS = 0;

			// Token: 0x04000F5D RID: 3933
			public const int DESIRED_VERSION_GAME = 3;

			// Token: 0x04000F5E RID: 3934
			public int settingsVersion = 4;

			// Token: 0x04000F5F RID: 3935
			public int controlsVersion;

			// Token: 0x04000F60 RID: 3936
			public int gameVersion = 3;

			// Token: 0x04000F61 RID: 3937
			public static int settingsVersion_LoadedBackup = 4;

			// Token: 0x04000F62 RID: 3938
			public static int controlsVersion_LoadedBackup = 0;

			// Token: 0x04000F63 RID: 3939
			public static int gameVersion_LoadedBackup = 3;

			// Token: 0x04000F64 RID: 3940
			public static bool settingsVersionFixed = false;

			// Token: 0x04000F65 RID: 3941
			public static bool controlsVersionFixed = false;

			// Token: 0x04000F66 RID: 3942
			public static bool gameVersionFixed = false;
		}

		// Token: 0x0200012E RID: 302
		// (Invoke) Token: 0x06000EDC RID: 3804
		public delegate void OnVersionChange(int oldVersionNumber, int newVersionNumber);

		// Token: 0x0200012F RID: 303
		[Serializable]
		public class SettingsData
		{
			// Token: 0x170000A1 RID: 161
			// (get) Token: 0x06000EDF RID: 3807 RVA: 0x00012004 File Offset: 0x00010204
			public static bool FullscreenDefault
			{
				get
				{
					return PlatformMaster.PlatformSupports_FullscreenSwitching();
				}
			}

			// Token: 0x06000EE0 RID: 3808 RVA: 0x0001200B File Offset: 0x0001020B
			private void VerticalResolutionEnsure()
			{
				if (Data.SettingsData.supportedVerticalResolutions == null)
				{
					Data.SettingsData.supportedVerticalResolutions = PlatformMaster.PlatformSupportedVerticalResolutions();
				}
			}

			// Token: 0x06000EE1 RID: 3809 RVA: 0x0006C9D8 File Offset: 0x0006ABD8
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

			// Token: 0x06000EE2 RID: 3810 RVA: 0x0006CA20 File Offset: 0x0006AC20
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

			// Token: 0x06000EE3 RID: 3811 RVA: 0x0006CA84 File Offset: 0x0006AC84
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

			// Token: 0x06000EE4 RID: 3812 RVA: 0x0001201E File Offset: 0x0001021E
			public Data.SettingsData.VerticalResolution VerticalResolutionGet()
			{
				return this.verticalResolution;
			}

			// Token: 0x06000EE5 RID: 3813 RVA: 0x0006CAE8 File Offset: 0x0006ACE8
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

			// Token: 0x06000EE6 RID: 3814 RVA: 0x0006CB50 File Offset: 0x0006AD50
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

			// Token: 0x06000EE7 RID: 3815 RVA: 0x0006CBDC File Offset: 0x0006ADDC
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

			// Token: 0x06000EE8 RID: 3816 RVA: 0x00012026 File Offset: 0x00010226
			private void WidthAspectRatioEnsure()
			{
				if (Data.SettingsData.supportedWidthAspectRatios == null)
				{
					Data.SettingsData.supportedWidthAspectRatios = PlatformMaster.PlatformSupportedWidthAspectRatios();
				}
			}

			// Token: 0x06000EE9 RID: 3817 RVA: 0x0006CCB4 File Offset: 0x0006AEB4
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

			// Token: 0x06000EEA RID: 3818 RVA: 0x0006CCFC File Offset: 0x0006AEFC
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

			// Token: 0x06000EEB RID: 3819 RVA: 0x0006CD60 File Offset: 0x0006AF60
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

			// Token: 0x06000EEC RID: 3820 RVA: 0x00012039 File Offset: 0x00010239
			public Data.SettingsData.WidthAspectRatio WidthAspectRatioGet()
			{
				return this.widthAspectRatio;
			}

			// Token: 0x06000EED RID: 3821 RVA: 0x0006CDC4 File Offset: 0x0006AFC4
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

			// Token: 0x170000A2 RID: 162
			// (get) Token: 0x06000EEE RID: 3822 RVA: 0x00012041 File Offset: 0x00010241
			public static int QUALITY_DEFAULT
			{
				get
				{
					return PlatformMaster.QualityDefaultGet();
				}
			}

			// Token: 0x06000EEF RID: 3823 RVA: 0x0006CE2C File Offset: 0x0006B02C
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

			// Token: 0x06000EF0 RID: 3824 RVA: 0x00012048 File Offset: 0x00010248
			public void FovSet(int playerIndex, float fov)
			{
				this.FovEnsure();
				this._fieldOfViews[playerIndex] = fov;
				this._fieldOfViews[playerIndex] = Mathf.Clamp(this._fieldOfViews[playerIndex], 30f, 180f);
			}

			// Token: 0x06000EF1 RID: 3825 RVA: 0x00012078 File Offset: 0x00010278
			public float FovGet(int playerIndex)
			{
				this.FovEnsure();
				return this._fieldOfViews[playerIndex];
			}

			// Token: 0x06000EF2 RID: 3826 RVA: 0x00012088 File Offset: 0x00010288
			public void FovAdd(int playerIndex, float fov)
			{
				this.FovEnsure();
				this._fieldOfViews[playerIndex] += fov;
				this._fieldOfViews[playerIndex] = Mathf.Clamp(this._fieldOfViews[playerIndex], 30f, 180f);
			}

			// Token: 0x06000EF3 RID: 3827 RVA: 0x000120C0 File Offset: 0x000102C0
			public void FovReset(int playerIndex)
			{
				this.FovEnsure();
				this._fieldOfViews[playerIndex] = 60f;
			}

			// Token: 0x06000EF4 RID: 3828 RVA: 0x0006CE74 File Offset: 0x0006B074
			public void FovResetAll()
			{
				this.FovEnsure();
				for (int i = 0; i < 1; i++)
				{
					this._fieldOfViews[i] = 60f;
				}
			}

			// Token: 0x06000EF5 RID: 3829 RVA: 0x000120D5 File Offset: 0x000102D5
			public Data.SettingsData.KeyboardLayout KeyboardLayoutGet()
			{
				return this.keyboardLayout;
			}

			// Token: 0x06000EF6 RID: 3830 RVA: 0x000120DD File Offset: 0x000102DD
			public void KeyboardLayoutSet(Data.SettingsData.KeyboardLayout layout)
			{
				this.keyboardLayout = layout;
			}

			// Token: 0x06000EF7 RID: 3831 RVA: 0x0006CEA0 File Offset: 0x0006B0A0
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

			// Token: 0x06000EF8 RID: 3832 RVA: 0x0006CEC8 File Offset: 0x0006B0C8
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

			// Token: 0x06000EF9 RID: 3833 RVA: 0x0006CEF4 File Offset: 0x0006B0F4
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

			// Token: 0x06000EFA RID: 3834 RVA: 0x000120E6 File Offset: 0x000102E6
			public void JoystickVibrationEnabledSet(int playerIndex, bool enabled)
			{
				this.JoystickVibrationEnsure();
				this.joystickVibrationEnabledPerPlayer[playerIndex] = enabled;
			}

			// Token: 0x06000EFB RID: 3835 RVA: 0x000120F7 File Offset: 0x000102F7
			public bool JoystickVibrationEnabledGet(int playerIndex)
			{
				this.JoystickVibrationEnsure();
				return this.joystickVibrationEnabledPerPlayer[playerIndex];
			}

			// Token: 0x06000EFC RID: 3836 RVA: 0x00012107 File Offset: 0x00010307
			public void JoystickVibrationEnabledToggle(int playerIndex)
			{
				this.JoystickVibrationEnsure();
				this.joystickVibrationEnabledPerPlayer[playerIndex] = !this.joystickVibrationEnabledPerPlayer[playerIndex];
			}

			// Token: 0x06000EFD RID: 3837 RVA: 0x0006CF38 File Offset: 0x0006B138
			public void JoystickVibrationEnabledSetAll(bool enabled)
			{
				this.JoystickVibrationEnsure();
				for (int i = 0; i < 1; i++)
				{
					this.joystickVibrationEnabledPerPlayer[i] = enabled;
				}
			}

			// Token: 0x06000EFE RID: 3838 RVA: 0x0006CF60 File Offset: 0x0006B160
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

			// Token: 0x06000EFF RID: 3839 RVA: 0x00012122 File Offset: 0x00010322
			public Vector2 CameraSensitivityGet(int playerIndex)
			{
				this.CameraSensitivityEnsure();
				return this.cameraSensitivity[playerIndex];
			}

			// Token: 0x06000F00 RID: 3840 RVA: 0x0006CFB4 File Offset: 0x0006B1B4
			public void CameraSensitivitySet(int playerIndex, Vector2 sensitivity)
			{
				this.CameraSensitivityEnsure();
				this.cameraSensitivity[playerIndex] = sensitivity;
				this.cameraSensitivity[playerIndex].x = Mathf.Clamp(this.cameraSensitivity[playerIndex].x, 0.1f, 10f);
				this.cameraSensitivity[playerIndex].y = Mathf.Clamp(this.cameraSensitivity[playerIndex].y, 0.1f, 10f);
			}

			// Token: 0x06000F01 RID: 3841 RVA: 0x0006D038 File Offset: 0x0006B238
			public void CameraSensitivityAdd(int playerIndex, Vector2 sensitivity)
			{
				this.CameraSensitivityEnsure();
				this.cameraSensitivity[playerIndex] += sensitivity;
				this.cameraSensitivity[playerIndex].x = Mathf.Clamp(this.cameraSensitivity[playerIndex].x, 0.1f, 10f);
				this.cameraSensitivity[playerIndex].y = Mathf.Clamp(this.cameraSensitivity[playerIndex].y, 0.1f, 10f);
			}

			// Token: 0x06000F02 RID: 3842 RVA: 0x00012136 File Offset: 0x00010336
			public void CameraSensitivityReset(int playerIndex)
			{
				this.CameraSensitivityEnsure();
				this.cameraSensitivity[playerIndex] = new Vector2(1f, 1f);
			}

			// Token: 0x06000F03 RID: 3843 RVA: 0x0006D0CC File Offset: 0x0006B2CC
			public void CameraSensitivityResetAll()
			{
				this.CameraSensitivityEnsure();
				for (int i = 0; i < 1; i++)
				{
					this.cameraSensitivity[i] = new Vector2(1f, 1f);
				}
			}

			// Token: 0x06000F04 RID: 3844 RVA: 0x0006D108 File Offset: 0x0006B308
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

			// Token: 0x06000F05 RID: 3845 RVA: 0x00012159 File Offset: 0x00010359
			public void ControlsInvertMovementXSet(int playerIndex, bool invert)
			{
				this.ControlsInversionEnsure();
				this.controlsInvertMovementX[playerIndex] = invert;
			}

			// Token: 0x06000F06 RID: 3846 RVA: 0x0001216A File Offset: 0x0001036A
			public bool ControlsInvertMovementXGet(int playerIndex)
			{
				this.ControlsInversionEnsure();
				return this.controlsInvertMovementX[playerIndex];
			}

			// Token: 0x06000F07 RID: 3847 RVA: 0x0001217A File Offset: 0x0001037A
			public void ControlsInvertMovementYSet(int playerIndex, bool invert)
			{
				this.ControlsInversionEnsure();
				this.controlsInvertMovementY[playerIndex] = invert;
			}

			// Token: 0x06000F08 RID: 3848 RVA: 0x0001218B File Offset: 0x0001038B
			public bool ControlsInvertMovementYGet(int playerIndex)
			{
				this.ControlsInversionEnsure();
				return this.controlsInvertMovementY[playerIndex];
			}

			// Token: 0x06000F09 RID: 3849 RVA: 0x0001219B File Offset: 0x0001039B
			public void ControlsInvertCameraXSet(int playerIndex, bool invert)
			{
				this.ControlsInversionEnsure();
				this.controlsInvertCameraX[playerIndex] = invert;
			}

			// Token: 0x06000F0A RID: 3850 RVA: 0x000121AC File Offset: 0x000103AC
			public bool ControlsInvertCameraXGet(int playerIndex)
			{
				this.ControlsInversionEnsure();
				return this.controlsInvertCameraX[playerIndex];
			}

			// Token: 0x06000F0B RID: 3851 RVA: 0x000121BC File Offset: 0x000103BC
			public void ControlsInvertCameraYSet(int playerIndex, bool invert)
			{
				this.ControlsInversionEnsure();
				this.controlsInvertCameraY[playerIndex] = invert;
			}

			// Token: 0x06000F0C RID: 3852 RVA: 0x000121CD File Offset: 0x000103CD
			public bool ControlsInvertCameraYGet(int playerIndex)
			{
				this.ControlsInversionEnsure();
				return this.controlsInvertCameraY[playerIndex];
			}

			// Token: 0x06000F0D RID: 3853 RVA: 0x000121DD File Offset: 0x000103DD
			public void ControlsInvertScrollingXSet(int playerIndex, bool invert)
			{
				this.ControlsInversionEnsure();
				this.controlsInvertScrollingX[playerIndex] = invert;
			}

			// Token: 0x06000F0E RID: 3854 RVA: 0x000121EE File Offset: 0x000103EE
			public bool ControlsInvertScrollingXGet(int playerIndex)
			{
				this.ControlsInversionEnsure();
				return this.controlsInvertScrollingX[playerIndex];
			}

			// Token: 0x06000F0F RID: 3855 RVA: 0x000121FE File Offset: 0x000103FE
			public void ControlsInvertScrollingYSet(int playerIndex, bool invert)
			{
				this.ControlsInversionEnsure();
				this.controlsInvertScrollingY[playerIndex] = invert;
			}

			// Token: 0x06000F10 RID: 3856 RVA: 0x0001220F File Offset: 0x0001040F
			public bool ControlsInvertScrollingYGet(int playerIndex)
			{
				this.ControlsInversionEnsure();
				return this.controlsInvertScrollingY[playerIndex];
			}

			// Token: 0x06000F11 RID: 3857 RVA: 0x0006D1B4 File Offset: 0x0006B3B4
			public void ControlsInvertMovementAll()
			{
				this.ControlsInversionEnsure();
				for (int i = 0; i < 1; i++)
				{
					this.controlsInvertMovementX[i] = false;
					this.controlsInvertMovementY[i] = false;
				}
			}

			// Token: 0x06000F12 RID: 3858 RVA: 0x0006D1E8 File Offset: 0x0006B3E8
			public void ControlsInvertCameraAll()
			{
				this.ControlsInversionEnsure();
				for (int i = 0; i < 1; i++)
				{
					this.controlsInvertCameraX[i] = false;
					this.controlsInvertCameraY[i] = false;
				}
			}

			// Token: 0x06000F13 RID: 3859 RVA: 0x0006D21C File Offset: 0x0006B41C
			public void ControlsInvertScrollingAll()
			{
				this.ControlsInversionEnsure();
				for (int i = 0; i < 1; i++)
				{
					this.controlsInvertScrollingX[i] = false;
					this.controlsInvertScrollingY[i] = false;
				}
			}

			// Token: 0x06000F14 RID: 3860 RVA: 0x0006D250 File Offset: 0x0006B450
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

			// Token: 0x06000F15 RID: 3861 RVA: 0x0001221F File Offset: 0x0001041F
			public void VirtualCursorSensitivitySet(int playerIndex, float sensitivity)
			{
				this.VirtualCursorSensitivityEnsure();
				this._virtualCursorSensitivity[playerIndex] = sensitivity;
				this._virtualCursorSensitivity[playerIndex] = Mathf.Clamp(this._virtualCursorSensitivity[playerIndex], 0.1f, 10f);
			}

			// Token: 0x06000F16 RID: 3862 RVA: 0x0001224F File Offset: 0x0001044F
			public float VirtualCursorSensitivityGet(int playerIndex)
			{
				this.VirtualCursorSensitivityEnsure();
				return this._virtualCursorSensitivity[playerIndex];
			}

			// Token: 0x06000F17 RID: 3863 RVA: 0x0001225F File Offset: 0x0001045F
			public void VirtualCursorSensitivityAdd(int playerIndex, float sensitivity)
			{
				this.VirtualCursorSensitivityEnsure();
				this._virtualCursorSensitivity[playerIndex] += sensitivity;
				this._virtualCursorSensitivity[playerIndex] = Mathf.Clamp(this._virtualCursorSensitivity[playerIndex], 0.1f, 10f);
			}

			// Token: 0x06000F18 RID: 3864 RVA: 0x00012297 File Offset: 0x00010497
			public void VirtualCursorSensitivityReset(int playerIndex)
			{
				this.VirtualCursorSensitivityEnsure();
				this._virtualCursorSensitivity[playerIndex] = 1f;
			}

			// Token: 0x06000F19 RID: 3865 RVA: 0x0006D298 File Offset: 0x0006B498
			public void VirtualCursorSensitivityResetAll()
			{
				this.VirtualCursorSensitivityEnsure();
				for (int i = 0; i < 1; i++)
				{
					this._virtualCursorSensitivity[i] = 1f;
				}
			}

			// Token: 0x06000F1A RID: 3866 RVA: 0x0006D2C4 File Offset: 0x0006B4C4
			public static float TransitionSpeedMapped_Get(float from, float fromMin, float fromMax, float toMin, float toMax)
			{
				float num = from - fromMin;
				float num2 = fromMax - fromMin;
				float num3 = num / num2;
				return (toMax - toMin) * num3 + toMin;
			}

			// Token: 0x06000F1B RID: 3867 RVA: 0x000122AC File Offset: 0x000104AC
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

			// Token: 0x06000F1C RID: 3868 RVA: 0x0006D2E4 File Offset: 0x0006B4E4
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

			// Token: 0x04000F67 RID: 3943
			public static Data.SettingsData inst = new Data.SettingsData();

			// Token: 0x04000F68 RID: 3944
			public bool dyslexicFontEnabled;

			// Token: 0x04000F69 RID: 3945
			public bool colorblindModeEnabled;

			// Token: 0x04000F6A RID: 3946
			public bool flashingLightsReducedEnabled;

			// Token: 0x04000F6B RID: 3947
			public bool wobblyPolygons = true;

			// Token: 0x04000F6C RID: 3948
			public bool fullscreenEnabled = Data.SettingsData.FullscreenDefault;

			// Token: 0x04000F6D RID: 3949
			public const int PIXELS_PER_UNIT = 32;

			// Token: 0x04000F6E RID: 3950
			public const Data.SettingsData.VerticalResolution VERTICAL_RESOLUTION_DEFAULT = Data.SettingsData.VerticalResolution._native;

			// Token: 0x04000F6F RID: 3951
			public Data.SettingsData.VerticalResolution verticalResolution;

			// Token: 0x04000F70 RID: 3952
			public static Data.SettingsData.VerticalResolution[] supportedVerticalResolutions = null;

			// Token: 0x04000F71 RID: 3953
			public const Data.SettingsData.WidthAspectRatio WIDTH_ASPECT_RATIO_DEFAULT = Data.SettingsData.WidthAspectRatio._16_9;

			// Token: 0x04000F72 RID: 3954
			public Data.SettingsData.WidthAspectRatio widthAspectRatio = Data.SettingsData.WidthAspectRatio._16_9;

			// Token: 0x04000F73 RID: 3955
			public static Data.SettingsData.WidthAspectRatio[] supportedWidthAspectRatios = null;

			// Token: 0x04000F74 RID: 3956
			public const Data.SettingsData.CrtFilter CRT_FILTER_DEFAULT = Data.SettingsData.CrtFilter.none;

			// Token: 0x04000F75 RID: 3957
			public Data.SettingsData.CrtFilter crtFilter;

			// Token: 0x04000F76 RID: 3958
			public const bool CHROMATIC_ABERRATION_DEFAULT = true;

			// Token: 0x04000F77 RID: 3959
			public bool chromaticAberrationEnabled = true;

			// Token: 0x04000F78 RID: 3960
			public const bool BLOOM_DEFAULT = true;

			// Token: 0x04000F79 RID: 3961
			public bool bloomEnabled = true;

			// Token: 0x04000F7A RID: 3962
			public const bool MOTION_BLUR_DEFAULT = false;

			// Token: 0x04000F7B RID: 3963
			public bool motionBlurEnabled;

			// Token: 0x04000F7C RID: 3964
			public const bool VSYNC_DEFAULT = true;

			// Token: 0x04000F7D RID: 3965
			public bool vsyncEnabled = true;

			// Token: 0x04000F7E RID: 3966
			public const bool SCREENSHAKE_DEFAULT = true;

			// Token: 0x04000F7F RID: 3967
			public bool screenshakeEnabled = true;

			// Token: 0x04000F80 RID: 3968
			public int qualityLevel = -1;

			// Token: 0x04000F81 RID: 3969
			public const Data.SettingsData.TateMode TATE_MODE_DEFAULT = Data.SettingsData.TateMode.none;

			// Token: 0x04000F82 RID: 3970
			public Data.SettingsData.TateMode tateMode;

			// Token: 0x04000F83 RID: 3971
			public const float FOV_DEFAULT = 60f;

			// Token: 0x04000F84 RID: 3972
			public const float FOV_MIN = 30f;

			// Token: 0x04000F85 RID: 3973
			public const float FOV_MAX = 180f;

			// Token: 0x04000F86 RID: 3974
			[SerializeField]
			private float[] _fieldOfViews;

			// Token: 0x04000F87 RID: 3975
			public const Data.SettingsData.SplitScreenKind CAMERA_SPLIT_SCREEN_KIND_DEFAULT = Data.SettingsData.SplitScreenKind.horizontal;

			// Token: 0x04000F88 RID: 3976
			public Data.SettingsData.SplitScreenKind cameraSplitScreenKind;

			// Token: 0x04000F89 RID: 3977
			public const float AUDIO_MASTER_VOLUME_DEFAULT = 1f;

			// Token: 0x04000F8A RID: 3978
			public float volumeMaster = 1f;

			// Token: 0x04000F8B RID: 3979
			public const float AUDIO_MUSIC_VOLUME_DEFAULT = 0.5f;

			// Token: 0x04000F8C RID: 3980
			public float volumeMusic = 0.5f;

			// Token: 0x04000F8D RID: 3981
			public const float AUDIO_SFX_VOLUME_DEFAULT = 0.5f;

			// Token: 0x04000F8E RID: 3982
			public float volumeSound = 0.5f;

			// Token: 0x04000F8F RID: 3983
			public const float AUDIO_FAN_VOLUME_DEFAULT = 1f;

			// Token: 0x04000F90 RID: 3984
			public float fanVolume = 1f;

			// Token: 0x04000F91 RID: 3985
			public const Translation.Language LANGUAGE_DEFAULT = Translation.Language.English;

			// Token: 0x04000F92 RID: 3986
			public Translation.Language language;

			// Token: 0x04000F93 RID: 3987
			public bool initialLanguageSelectionPerfromed;

			// Token: 0x04000F94 RID: 3988
			[SerializeField]
			private Data.SettingsData.KeyboardLayout keyboardLayout;

			// Token: 0x04000F95 RID: 3989
			public const bool JOYSTICK_VIBRATION_DEFAULT = true;

			// Token: 0x04000F96 RID: 3990
			[SerializeField]
			private bool[] joystickVibrationEnabledPerPlayer;

			// Token: 0x04000F97 RID: 3991
			public const float CAMERA_SENSITIVITY_X_DEFAULT = 1f;

			// Token: 0x04000F98 RID: 3992
			public const float CAMERA_SENSITIVITY_Y_DEFAULT = 1f;

			// Token: 0x04000F99 RID: 3993
			public const float CAMERA_SENSITIVITY_LIMIT_MIN = 0.1f;

			// Token: 0x04000F9A RID: 3994
			public const float CAMERA_SENSITIVITY_LIMIT_MAX = 10f;

			// Token: 0x04000F9B RID: 3995
			[SerializeField]
			private Vector2[] cameraSensitivity;

			// Token: 0x04000F9C RID: 3996
			public const bool CONTROLS_INVERT_MOVEMENT_X_DEFAULT = false;

			// Token: 0x04000F9D RID: 3997
			public const bool CONTROLS_INVERT_MOVEMENT_Y_DEFAULT = false;

			// Token: 0x04000F9E RID: 3998
			public const bool CONTROLS_INVERT_CAMERA_X_DEFAULT = false;

			// Token: 0x04000F9F RID: 3999
			public const bool CONTROLS_INVERT_CAMERA_Y_DEFAULT = false;

			// Token: 0x04000FA0 RID: 4000
			public const bool CONTROLS_INVERT_SCROLLING_X_DEFAULT = false;

			// Token: 0x04000FA1 RID: 4001
			public const bool CONTROLS_INVERT_SCROLLING_Y_DEFAULT = false;

			// Token: 0x04000FA2 RID: 4002
			[SerializeField]
			private bool[] controlsInvertMovementX;

			// Token: 0x04000FA3 RID: 4003
			[SerializeField]
			private bool[] controlsInvertMovementY;

			// Token: 0x04000FA4 RID: 4004
			[SerializeField]
			private bool[] controlsInvertCameraX;

			// Token: 0x04000FA5 RID: 4005
			[SerializeField]
			private bool[] controlsInvertCameraY;

			// Token: 0x04000FA6 RID: 4006
			[SerializeField]
			private bool[] controlsInvertScrollingX;

			// Token: 0x04000FA7 RID: 4007
			[SerializeField]
			private bool[] controlsInvertScrollingY;

			// Token: 0x04000FA8 RID: 4008
			public const float VIRTUAL_CURSOR_SENSITIVITY_DEFAULT = 1f;

			// Token: 0x04000FA9 RID: 4009
			public const float VIRTUAL_CURSOR_SENSITIVITY_LIMIT_MIN = 0.1f;

			// Token: 0x04000FAA RID: 4010
			public const float VIRTUAL_CURSOR_SENSITIVITY_LIMIT_MAX = 10f;

			// Token: 0x04000FAB RID: 4011
			[SerializeField]
			private float[] _virtualCursorSensitivity;

			// Token: 0x04000FAC RID: 4012
			public string controlMapsJson;

			// Token: 0x04000FAD RID: 4013
			public int transitionSpeed = 1;

			// Token: 0x02000130 RID: 304
			public enum VerticalResolution
			{
				// Token: 0x04000FAF RID: 4015
				_native,
				// Token: 0x04000FB0 RID: 4016
				_360p,
				// Token: 0x04000FB1 RID: 4017
				_480p,
				// Token: 0x04000FB2 RID: 4018
				_720p,
				// Token: 0x04000FB3 RID: 4019
				_1080p,
				// Token: 0x04000FB4 RID: 4020
				_1440p,
				// Token: 0x04000FB5 RID: 4021
				_4k
			}

			// Token: 0x02000131 RID: 305
			public enum WidthAspectRatio
			{
				// Token: 0x04000FB7 RID: 4023
				_extend,
				// Token: 0x04000FB8 RID: 4024
				_4_3,
				// Token: 0x04000FB9 RID: 4025
				_3_4,
				// Token: 0x04000FBA RID: 4026
				_16_9,
				// Token: 0x04000FBB RID: 4027
				_9_16,
				// Token: 0x04000FBC RID: 4028
				_3_2,
				// Token: 0x04000FBD RID: 4029
				_2_3
			}

			// Token: 0x02000132 RID: 306
			public enum CrtFilter
			{
				// Token: 0x04000FBF RID: 4031
				none,
				// Token: 0x04000FC0 RID: 4032
				border,
				// Token: 0x04000FC1 RID: 4033
				scanlines,
				// Token: 0x04000FC2 RID: 4034
				full,
				// Token: 0x04000FC3 RID: 4035
				_count
			}

			// Token: 0x02000133 RID: 307
			public enum TateMode
			{
				// Token: 0x04000FC5 RID: 4037
				none,
				// Token: 0x04000FC6 RID: 4038
				horizontalLeft,
				// Token: 0x04000FC7 RID: 4039
				horizontalRight,
				// Token: 0x04000FC8 RID: 4040
				upsideDown,
				// Token: 0x04000FC9 RID: 4041
				_count
			}

			// Token: 0x02000134 RID: 308
			public enum SplitScreenKind
			{
				// Token: 0x04000FCB RID: 4043
				horizontal,
				// Token: 0x04000FCC RID: 4044
				vertical
			}

			// Token: 0x02000135 RID: 309
			public enum KeyboardLayout
			{
				// Token: 0x04000FCE RID: 4046
				keyboard_QWERTY,
				// Token: 0x04000FCF RID: 4047
				keyboard_AZERTY,
				// Token: 0x04000FD0 RID: 4048
				keyboard_DVORAK,
				// Token: 0x04000FD1 RID: 4049
				keyboard_COLEMAK,
				// Token: 0x04000FD2 RID: 4050
				count
			}
		}

		// Token: 0x02000136 RID: 310
		public enum GameSavingReason
		{
			// Token: 0x04000FD4 RID: 4052
			_undefined = -1,
			// Token: 0x04000FD5 RID: 4053
			debug,
			// Token: 0x04000FD6 RID: 4054
			setFlag_RunForceReset,
			// Token: 0x04000FD7 RID: 4055
			newGame,
			// Token: 0x04000FD8 RID: 4056
			introFinished,
			// Token: 0x04000FD9 RID: 4057
			mainMenuOpened_NotDuringSlotMachine,
			// Token: 0x04000FDA RID: 4058
			death,
			// Token: 0x04000FDB RID: 4059
			endingWithoutDeath,
			// Token: 0x04000FDC RID: 4060
			beginOfPlayingAtTheSlotMachine,
			// Token: 0x04000FDD RID: 4061
			endOfRound_AfterInterestsAndTicketsCutscene,
			// Token: 0x04000FDE RID: 4062
			endOfDeadline_AfterCutscene,
			// Token: 0x04000FDF RID: 4063
			rewardBox_Opened,
			// Token: 0x04000FE0 RID: 4064
			rewardBox_PickedUpReward,
			// Token: 0x04000FE1 RID: 4065
			powerupUnlock,
			// Token: 0x04000FE2 RID: 4066
			storeBuyOrReroll,
			// Token: 0x04000FE3 RID: 4067
			phoneReroll,
			// Token: 0x04000FE4 RID: 4068
			phoneSaveTime,
			// Token: 0x04000FE5 RID: 4069
			_count
		}

		// Token: 0x02000137 RID: 311
		[Serializable]
		public class GameData
		{
			// Token: 0x06000F1F RID: 3871 RVA: 0x0006D468 File Offset: 0x0006B668
			public GameData(int gameDataIndex)
			{
				this.myGameDataIndex = gameDataIndex;
			}

			// Token: 0x06000F20 RID: 3872 RVA: 0x0006D4EC File Offset: 0x0006B6EC
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

			// Token: 0x06000F21 RID: 3873 RVA: 0x000122F5 File Offset: 0x000104F5
			public void Loading_Prepare()
			{
				this.gameplayData.Load_Format();
				this._LockedPowerups_LoadPrepare();
				this._RunModifiers_LoadPrepare();
			}

			// Token: 0x06000F22 RID: 3874 RVA: 0x0006D54C File Offset: 0x0006B74C
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

			// Token: 0x06000F23 RID: 3875 RVA: 0x0001230E File Offset: 0x0001050E
			public bool GameplayDataIsEmpty()
			{
				return !this.gameplayDataHasSession;
			}

			// Token: 0x06000F24 RID: 3876 RVA: 0x0006D638 File Offset: 0x0006B838
			public static bool IsGameCompletedFully()
			{
				return Data.game != null && Data.game.doorOpenedCounter > 0 && Data.game.badEndingCounter > 0 && Data.game.goodEndingCounter > 0 && Data.game.PowerupRealInstances_AreAllUnlocked() && Data.game.AllCardsUnlocked && Data.game.AllCardsHolographic;
			}

			// Token: 0x06000F25 RID: 3877 RVA: 0x0006D6A8 File Offset: 0x0006B8A8
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

			// Token: 0x170000A3 RID: 163
			// (get) Token: 0x06000F26 RID: 3878 RVA: 0x00012319 File Offset: 0x00010519
			// (set) Token: 0x06000F27 RID: 3879 RVA: 0x00012321 File Offset: 0x00010521
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

			// Token: 0x06000F28 RID: 3880 RVA: 0x0001232A File Offset: 0x0001052A
			public static List<PowerupScript.Identifier> _UnlockedPowerups_Definition()
			{
				return new List<PowerupScript.Identifier> { PowerupScript.Identifier.undefined };
			}

			// Token: 0x06000F29 RID: 3881 RVA: 0x0006D784 File Offset: 0x0006B984
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

			// Token: 0x06000F2A RID: 3882 RVA: 0x00012338 File Offset: 0x00010538
			public static List<PowerupScript.Identifier> _LockedPowerups_ResultingList_Definition()
			{
				if (Data.GameData.inst == null)
				{
					return new List<PowerupScript.Identifier>();
				}
				return new List<PowerupScript.Identifier>(164);
			}

			// Token: 0x06000F2B RID: 3883 RVA: 0x00012351 File Offset: 0x00010551
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

			// Token: 0x06000F2C RID: 3884 RVA: 0x0006DC08 File Offset: 0x0006BE08
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

			// Token: 0x06000F2D RID: 3885 RVA: 0x0001238C File Offset: 0x0001058C
			private void _LockedPowerups_SavePrepare()
			{
				this._LockedPowerupsSystem_ListsEnsure();
				this._unlockedPowerupsString = PlatformDataMaster.EnumListToString<PowerupScript.Identifier>(this.unlockedPowerups, ',');
				this._LockedPowerupsResultingList_Compute(false);
			}

			// Token: 0x06000F2E RID: 3886 RVA: 0x000123AE File Offset: 0x000105AE
			private void _LockedPowerups_LoadPrepare()
			{
				this._LockedPowerupsSystem_ListsEnsure();
				PlatformDataMaster.EnumListFromString<PowerupScript.Identifier>(this._unlockedPowerupsString, ref this.unlockedPowerups, true, ',');
				this._LockedPowerupsResultingList_Compute(false);
			}

			// Token: 0x06000F2F RID: 3887 RVA: 0x0006DCC4 File Offset: 0x0006BEC4
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

			// Token: 0x06000F30 RID: 3888 RVA: 0x000123D1 File Offset: 0x000105D1
			public int LockedPowerups_GetCount()
			{
				this._LockedPowerupsSystem_ListsEnsure();
				if (this.lockedPowerups_ResultingList == null)
				{
					this._LockedPowerupsResultingList_Compute(false);
				}
				return this.lockedPowerups_ResultingList.Count;
			}

			// Token: 0x06000F31 RID: 3889 RVA: 0x000123F3 File Offset: 0x000105F3
			public List<PowerupScript.Identifier> LockedPowerups_GetList()
			{
				this._LockedPowerupsSystem_ListsEnsure();
				this._LockedPowerupsResultingList_Compute(false);
				return this.lockedPowerups_ResultingList;
			}

			// Token: 0x06000F32 RID: 3890 RVA: 0x0006DCFC File Offset: 0x0006BEFC
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

			// Token: 0x06000F33 RID: 3891 RVA: 0x0006DD54 File Offset: 0x0006BF54
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

			// Token: 0x06000F34 RID: 3892 RVA: 0x00012408 File Offset: 0x00010608
			public void _LockedPowerups_LockAll()
			{
				this.unlockedPowerups = Data.GameData._UnlockedPowerups_Definition();
				this.lockedPowerups = Data.GameData._LockedPowerups_Definition();
			}

			// Token: 0x06000F35 RID: 3893 RVA: 0x00012420 File Offset: 0x00010620
			public List<PowerupScript.Identifier> _UnlockedPowerups_GetList()
			{
				this._LockedPowerupsSystem_ListsEnsure();
				return this.unlockedPowerups;
			}

			// Token: 0x06000F36 RID: 3894 RVA: 0x0006DDDC File Offset: 0x0006BFDC
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

			// Token: 0x06000F37 RID: 3895 RVA: 0x0001242E File Offset: 0x0001062E
			public void UnlockableSteps_OnRechargingRedButtonCharges(int chargesN)
			{
				if (GameplayMaster.IsCustomSeed())
				{
					return;
				}
				Data.game.UnlockSteps_SuperCapacitor += chargesN;
				Data.game.UnlockSteps_CrankGenerator += chargesN;
			}

			// Token: 0x06000F38 RID: 3896 RVA: 0x0006DE70 File Offset: 0x0006C070
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

			// Token: 0x170000A4 RID: 164
			// (get) Token: 0x06000F39 RID: 3897 RVA: 0x0001245C File Offset: 0x0001065C
			public static int UnlockStepsMissing_PuppetPersonalTrainer
			{
				get
				{
					return Mathf.Max(0, 15 - GameplayData.Stats_ModifiedLemonTriggeredTimesGet().CastToInt() - GameplayData.Stats_ModifiedCherryTriggeredTimesGet().CastToInt());
				}
			}

			// Token: 0x170000A5 RID: 165
			// (get) Token: 0x06000F3A RID: 3898 RVA: 0x0001247C File Offset: 0x0001067C
			public static int UnlockStepsMissing_PuppetElectrician
			{
				get
				{
					return Mathf.Max(0, 15 - GameplayData.Stats_ModifiedCloverTriggeredTimesGet().CastToInt() - GameplayData.Stats_ModifiedBellTriggeredTimesGet().CastToInt());
				}
			}

			// Token: 0x170000A6 RID: 166
			// (get) Token: 0x06000F3B RID: 3899 RVA: 0x0001249C File Offset: 0x0001069C
			public static int UnlockStepsMissing_PuppetFortuneTeller
			{
				get
				{
					return Mathf.Max(0, 15 - GameplayData.Stats_ModifiedDiamondTriggeredTimesGet().CastToInt() - GameplayData.Stats_ModifiedCoinsTriggeredTimesGet().CastToInt() - GameplayData.Stats_ModifiedSevenTriggeredTimesGet().CastToInt());
				}
			}

			// Token: 0x170000A7 RID: 167
			// (get) Token: 0x06000F3C RID: 3900 RVA: 0x000124C7 File Offset: 0x000106C7
			public static int UnlockStepsMissing_StepsCounter
			{
				get
				{
					return Mathf.Max(0, 20 - GameplayData.Stats_RedButtonEffectiveActivations_Get());
				}
			}

			// Token: 0x170000A8 RID: 168
			// (get) Token: 0x06000F3D RID: 3901 RVA: 0x000124D7 File Offset: 0x000106D7
			public static int UnlockStepsMissing_DevilsHorn
			{
				get
				{
					return Mathf.Max(0, 3 - (int)GameplayData.Stats_SixSixSix_SeenTimes);
				}
			}

			// Token: 0x170000A9 RID: 169
			// (get) Token: 0x06000F3E RID: 3902 RVA: 0x000124E7 File Offset: 0x000106E7
			public static int UnlockStepsMissing_Necronomicon
			{
				get
				{
					return Mathf.Max(0, 5 - (int)GameplayData.Stats_SixSixSix_SeenTimes);
				}
			}

			// Token: 0x170000AA RID: 170
			// (get) Token: 0x06000F3F RID: 3903 RVA: 0x0006DED8 File Offset: 0x0006C0D8
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

			// Token: 0x170000AB RID: 171
			// (get) Token: 0x06000F40 RID: 3904 RVA: 0x0006DF14 File Offset: 0x0006C114
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

			// Token: 0x170000AC RID: 172
			// (get) Token: 0x06000F41 RID: 3905 RVA: 0x0006DF48 File Offset: 0x0006C148
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

			// Token: 0x170000AD RID: 173
			// (get) Token: 0x06000F42 RID: 3906 RVA: 0x000124F7 File Offset: 0x000106F7
			// (set) Token: 0x06000F43 RID: 3907 RVA: 0x000124FF File Offset: 0x000106FF
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

			// Token: 0x170000AE RID: 174
			// (get) Token: 0x06000F44 RID: 3908 RVA: 0x00012522 File Offset: 0x00010722
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

			// Token: 0x170000AF RID: 175
			// (get) Token: 0x06000F45 RID: 3909 RVA: 0x00012540 File Offset: 0x00010740
			// (set) Token: 0x06000F46 RID: 3910 RVA: 0x00012548 File Offset: 0x00010748
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

			// Token: 0x170000B0 RID: 176
			// (get) Token: 0x06000F47 RID: 3911 RVA: 0x0001256E File Offset: 0x0001076E
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

			// Token: 0x170000B1 RID: 177
			// (get) Token: 0x06000F48 RID: 3912 RVA: 0x0001258F File Offset: 0x0001078F
			// (set) Token: 0x06000F49 RID: 3913 RVA: 0x00012597 File Offset: 0x00010797
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

			// Token: 0x170000B2 RID: 178
			// (get) Token: 0x06000F4A RID: 3914 RVA: 0x000125BA File Offset: 0x000107BA
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

			// Token: 0x170000B3 RID: 179
			// (get) Token: 0x06000F4B RID: 3915 RVA: 0x000125D8 File Offset: 0x000107D8
			// (set) Token: 0x06000F4C RID: 3916 RVA: 0x000125E0 File Offset: 0x000107E0
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

			// Token: 0x170000B4 RID: 180
			// (get) Token: 0x06000F4D RID: 3917 RVA: 0x00012603 File Offset: 0x00010803
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

			// Token: 0x170000B5 RID: 181
			// (get) Token: 0x06000F4E RID: 3918 RVA: 0x00012621 File Offset: 0x00010821
			// (set) Token: 0x06000F4F RID: 3919 RVA: 0x00012629 File Offset: 0x00010829
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

			// Token: 0x170000B6 RID: 182
			// (get) Token: 0x06000F50 RID: 3920 RVA: 0x0001264C File Offset: 0x0001084C
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

			// Token: 0x170000B7 RID: 183
			// (get) Token: 0x06000F51 RID: 3921 RVA: 0x0001266A File Offset: 0x0001086A
			// (set) Token: 0x06000F52 RID: 3922 RVA: 0x00012672 File Offset: 0x00010872
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

			// Token: 0x170000B8 RID: 184
			// (get) Token: 0x06000F53 RID: 3923 RVA: 0x00012695 File Offset: 0x00010895
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

			// Token: 0x170000B9 RID: 185
			// (get) Token: 0x06000F54 RID: 3924 RVA: 0x000126B3 File Offset: 0x000108B3
			// (set) Token: 0x06000F55 RID: 3925 RVA: 0x000126BB File Offset: 0x000108BB
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

			// Token: 0x170000BA RID: 186
			// (get) Token: 0x06000F56 RID: 3926 RVA: 0x000126DE File Offset: 0x000108DE
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

			// Token: 0x170000BB RID: 187
			// (get) Token: 0x06000F57 RID: 3927 RVA: 0x000126FC File Offset: 0x000108FC
			// (set) Token: 0x06000F58 RID: 3928 RVA: 0x00012704 File Offset: 0x00010904
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

			// Token: 0x170000BC RID: 188
			// (get) Token: 0x06000F59 RID: 3929 RVA: 0x00012726 File Offset: 0x00010926
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

			// Token: 0x170000BD RID: 189
			// (get) Token: 0x06000F5A RID: 3930 RVA: 0x00012744 File Offset: 0x00010944
			// (set) Token: 0x06000F5B RID: 3931 RVA: 0x0001274C File Offset: 0x0001094C
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

			// Token: 0x170000BE RID: 190
			// (get) Token: 0x06000F5C RID: 3932 RVA: 0x0001276F File Offset: 0x0001096F
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

			// Token: 0x170000BF RID: 191
			// (get) Token: 0x06000F5D RID: 3933 RVA: 0x0001278D File Offset: 0x0001098D
			// (set) Token: 0x06000F5E RID: 3934 RVA: 0x00012795 File Offset: 0x00010995
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

			// Token: 0x170000C0 RID: 192
			// (get) Token: 0x06000F5F RID: 3935 RVA: 0x000127BB File Offset: 0x000109BB
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

			// Token: 0x170000C1 RID: 193
			// (get) Token: 0x06000F60 RID: 3936 RVA: 0x000127DC File Offset: 0x000109DC
			// (set) Token: 0x06000F61 RID: 3937 RVA: 0x000127E4 File Offset: 0x000109E4
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

			// Token: 0x170000C2 RID: 194
			// (get) Token: 0x06000F62 RID: 3938 RVA: 0x00012807 File Offset: 0x00010A07
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

			// Token: 0x170000C3 RID: 195
			// (get) Token: 0x06000F63 RID: 3939 RVA: 0x00012825 File Offset: 0x00010A25
			// (set) Token: 0x06000F64 RID: 3940 RVA: 0x0001282D File Offset: 0x00010A2D
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

			// Token: 0x170000C4 RID: 196
			// (get) Token: 0x06000F65 RID: 3941 RVA: 0x00012850 File Offset: 0x00010A50
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

			// Token: 0x170000C5 RID: 197
			// (get) Token: 0x06000F66 RID: 3942 RVA: 0x0001286E File Offset: 0x00010A6E
			// (set) Token: 0x06000F67 RID: 3943 RVA: 0x00012876 File Offset: 0x00010A76
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

			// Token: 0x170000C6 RID: 198
			// (get) Token: 0x06000F68 RID: 3944 RVA: 0x00012899 File Offset: 0x00010A99
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

			// Token: 0x170000C7 RID: 199
			// (get) Token: 0x06000F69 RID: 3945 RVA: 0x000128B7 File Offset: 0x00010AB7
			// (set) Token: 0x06000F6A RID: 3946 RVA: 0x000128BF File Offset: 0x00010ABF
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

			// Token: 0x170000C8 RID: 200
			// (get) Token: 0x06000F6B RID: 3947 RVA: 0x000128E2 File Offset: 0x00010AE2
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

			// Token: 0x170000C9 RID: 201
			// (get) Token: 0x06000F6C RID: 3948 RVA: 0x00012900 File Offset: 0x00010B00
			// (set) Token: 0x06000F6D RID: 3949 RVA: 0x00012908 File Offset: 0x00010B08
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

			// Token: 0x170000CA RID: 202
			// (get) Token: 0x06000F6E RID: 3950 RVA: 0x0001292B File Offset: 0x00010B2B
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

			// Token: 0x170000CB RID: 203
			// (get) Token: 0x06000F6F RID: 3951 RVA: 0x00012949 File Offset: 0x00010B49
			// (set) Token: 0x06000F70 RID: 3952 RVA: 0x00012951 File Offset: 0x00010B51
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

			// Token: 0x170000CC RID: 204
			// (get) Token: 0x06000F71 RID: 3953 RVA: 0x00012974 File Offset: 0x00010B74
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

			// Token: 0x170000CD RID: 205
			// (get) Token: 0x06000F72 RID: 3954 RVA: 0x00012992 File Offset: 0x00010B92
			// (set) Token: 0x06000F73 RID: 3955 RVA: 0x0001299A File Offset: 0x00010B9A
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

			// Token: 0x170000CE RID: 206
			// (get) Token: 0x06000F74 RID: 3956 RVA: 0x000129BD File Offset: 0x00010BBD
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

			// Token: 0x170000CF RID: 207
			// (get) Token: 0x06000F75 RID: 3957 RVA: 0x000129DB File Offset: 0x00010BDB
			// (set) Token: 0x06000F76 RID: 3958 RVA: 0x000129E3 File Offset: 0x00010BE3
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

			// Token: 0x170000D0 RID: 208
			// (get) Token: 0x06000F77 RID: 3959 RVA: 0x00012A06 File Offset: 0x00010C06
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

			// Token: 0x170000D1 RID: 209
			// (get) Token: 0x06000F78 RID: 3960 RVA: 0x00012A24 File Offset: 0x00010C24
			// (set) Token: 0x06000F79 RID: 3961 RVA: 0x00012A2C File Offset: 0x00010C2C
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

			// Token: 0x170000D2 RID: 210
			// (get) Token: 0x06000F7A RID: 3962 RVA: 0x00012A52 File Offset: 0x00010C52
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

			// Token: 0x170000D3 RID: 211
			// (get) Token: 0x06000F7B RID: 3963 RVA: 0x00012A73 File Offset: 0x00010C73
			// (set) Token: 0x06000F7C RID: 3964 RVA: 0x00012A7B File Offset: 0x00010C7B
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

			// Token: 0x170000D4 RID: 212
			// (get) Token: 0x06000F7D RID: 3965 RVA: 0x00012AA1 File Offset: 0x00010CA1
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

			// Token: 0x170000D5 RID: 213
			// (get) Token: 0x06000F7E RID: 3966 RVA: 0x00012AC2 File Offset: 0x00010CC2
			// (set) Token: 0x06000F7F RID: 3967 RVA: 0x00012ACA File Offset: 0x00010CCA
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

			// Token: 0x170000D6 RID: 214
			// (get) Token: 0x06000F80 RID: 3968 RVA: 0x00012AED File Offset: 0x00010CED
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

			// Token: 0x170000D7 RID: 215
			// (get) Token: 0x06000F81 RID: 3969 RVA: 0x00012B0B File Offset: 0x00010D0B
			// (set) Token: 0x06000F82 RID: 3970 RVA: 0x00012B13 File Offset: 0x00010D13
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

			// Token: 0x170000D8 RID: 216
			// (get) Token: 0x06000F83 RID: 3971 RVA: 0x00012B36 File Offset: 0x00010D36
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

			// Token: 0x170000D9 RID: 217
			// (get) Token: 0x06000F84 RID: 3972 RVA: 0x00012B54 File Offset: 0x00010D54
			// (set) Token: 0x06000F85 RID: 3973 RVA: 0x00012B5C File Offset: 0x00010D5C
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

			// Token: 0x170000DA RID: 218
			// (get) Token: 0x06000F86 RID: 3974 RVA: 0x00012B7D File Offset: 0x00010D7D
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

			// Token: 0x170000DB RID: 219
			// (get) Token: 0x06000F87 RID: 3975 RVA: 0x00012B9A File Offset: 0x00010D9A
			// (set) Token: 0x06000F88 RID: 3976 RVA: 0x00012BA2 File Offset: 0x00010DA2
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

			// Token: 0x170000DC RID: 220
			// (get) Token: 0x06000F89 RID: 3977 RVA: 0x00012BC5 File Offset: 0x00010DC5
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

			// Token: 0x170000DD RID: 221
			// (get) Token: 0x06000F8A RID: 3978 RVA: 0x00012BE3 File Offset: 0x00010DE3
			// (set) Token: 0x06000F8B RID: 3979 RVA: 0x00012BEB File Offset: 0x00010DEB
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

			// Token: 0x170000DE RID: 222
			// (get) Token: 0x06000F8C RID: 3980 RVA: 0x00012C0E File Offset: 0x00010E0E
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

			// Token: 0x170000DF RID: 223
			// (get) Token: 0x06000F8D RID: 3981 RVA: 0x00012C2C File Offset: 0x00010E2C
			// (set) Token: 0x06000F8E RID: 3982 RVA: 0x00012C34 File Offset: 0x00010E34
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

			// Token: 0x170000E0 RID: 224
			// (get) Token: 0x06000F8F RID: 3983 RVA: 0x00012C57 File Offset: 0x00010E57
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

			// Token: 0x170000E1 RID: 225
			// (get) Token: 0x06000F90 RID: 3984 RVA: 0x00012C75 File Offset: 0x00010E75
			// (set) Token: 0x06000F91 RID: 3985 RVA: 0x00012C7D File Offset: 0x00010E7D
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

			// Token: 0x170000E2 RID: 226
			// (get) Token: 0x06000F92 RID: 3986 RVA: 0x00012CA0 File Offset: 0x00010EA0
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

			// Token: 0x170000E3 RID: 227
			// (get) Token: 0x06000F93 RID: 3987 RVA: 0x00012CBE File Offset: 0x00010EBE
			// (set) Token: 0x06000F94 RID: 3988 RVA: 0x00012CC6 File Offset: 0x00010EC6
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

			// Token: 0x170000E4 RID: 228
			// (get) Token: 0x06000F95 RID: 3989 RVA: 0x00012CE9 File Offset: 0x00010EE9
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

			// Token: 0x170000E5 RID: 229
			// (get) Token: 0x06000F96 RID: 3990 RVA: 0x00012D07 File Offset: 0x00010F07
			// (set) Token: 0x06000F97 RID: 3991 RVA: 0x00012D0F File Offset: 0x00010F0F
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

			// Token: 0x170000E6 RID: 230
			// (get) Token: 0x06000F98 RID: 3992 RVA: 0x00012D32 File Offset: 0x00010F32
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

			// Token: 0x170000E7 RID: 231
			// (get) Token: 0x06000F99 RID: 3993 RVA: 0x00012D50 File Offset: 0x00010F50
			// (set) Token: 0x06000F9A RID: 3994 RVA: 0x00012D58 File Offset: 0x00010F58
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

			// Token: 0x170000E8 RID: 232
			// (get) Token: 0x06000F9B RID: 3995 RVA: 0x00012D7E File Offset: 0x00010F7E
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

			// Token: 0x170000E9 RID: 233
			// (get) Token: 0x06000F9C RID: 3996 RVA: 0x00012D9F File Offset: 0x00010F9F
			// (set) Token: 0x06000F9D RID: 3997 RVA: 0x00012DA7 File Offset: 0x00010FA7
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

			// Token: 0x170000EA RID: 234
			// (get) Token: 0x06000F9E RID: 3998 RVA: 0x00012DCA File Offset: 0x00010FCA
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

			// Token: 0x170000EB RID: 235
			// (get) Token: 0x06000F9F RID: 3999 RVA: 0x00012DE8 File Offset: 0x00010FE8
			// (set) Token: 0x06000FA0 RID: 4000 RVA: 0x00012DF0 File Offset: 0x00010FF0
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

			// Token: 0x170000EC RID: 236
			// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x00012E16 File Offset: 0x00011016
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

			// Token: 0x170000ED RID: 237
			// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x00012E34 File Offset: 0x00011034
			// (set) Token: 0x06000FA3 RID: 4003 RVA: 0x00012E3C File Offset: 0x0001103C
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

			// Token: 0x170000EE RID: 238
			// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x00012E62 File Offset: 0x00011062
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

			// Token: 0x170000EF RID: 239
			// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x00012E80 File Offset: 0x00011080
			// (set) Token: 0x06000FA6 RID: 4006 RVA: 0x00012E88 File Offset: 0x00011088
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

			// Token: 0x170000F0 RID: 240
			// (get) Token: 0x06000FA7 RID: 4007 RVA: 0x00012EB1 File Offset: 0x000110B1
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

			// Token: 0x170000F1 RID: 241
			// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x00012ED2 File Offset: 0x000110D2
			// (set) Token: 0x06000FA9 RID: 4009 RVA: 0x00012EDA File Offset: 0x000110DA
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

			// Token: 0x170000F2 RID: 242
			// (get) Token: 0x06000FAA RID: 4010 RVA: 0x00012F03 File Offset: 0x00011103
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

			// Token: 0x170000F3 RID: 243
			// (get) Token: 0x06000FAB RID: 4011 RVA: 0x00012F24 File Offset: 0x00011124
			// (set) Token: 0x06000FAC RID: 4012 RVA: 0x00012F2C File Offset: 0x0001112C
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

			// Token: 0x170000F4 RID: 244
			// (get) Token: 0x06000FAD RID: 4013 RVA: 0x00012F55 File Offset: 0x00011155
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

			// Token: 0x170000F5 RID: 245
			// (get) Token: 0x06000FAE RID: 4014 RVA: 0x00012F76 File Offset: 0x00011176
			// (set) Token: 0x06000FAF RID: 4015 RVA: 0x00012F7E File Offset: 0x0001117E
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

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x00012FA7 File Offset: 0x000111A7
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

			// Token: 0x170000F7 RID: 247
			// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x00012FC8 File Offset: 0x000111C8
			// (set) Token: 0x06000FB2 RID: 4018 RVA: 0x00012FD0 File Offset: 0x000111D0
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

			// Token: 0x170000F8 RID: 248
			// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x00012FF9 File Offset: 0x000111F9
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

			// Token: 0x170000F9 RID: 249
			// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0001301A File Offset: 0x0001121A
			// (set) Token: 0x06000FB5 RID: 4021 RVA: 0x00013022 File Offset: 0x00011222
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

			// Token: 0x170000FA RID: 250
			// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x0001304B File Offset: 0x0001124B
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

			// Token: 0x170000FB RID: 251
			// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x0001306C File Offset: 0x0001126C
			// (set) Token: 0x06000FB8 RID: 4024 RVA: 0x00013074 File Offset: 0x00011274
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

			// Token: 0x170000FC RID: 252
			// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x0001309D File Offset: 0x0001129D
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

			// Token: 0x170000FD RID: 253
			// (get) Token: 0x06000FBA RID: 4026 RVA: 0x000130BE File Offset: 0x000112BE
			// (set) Token: 0x06000FBB RID: 4027 RVA: 0x000130C6 File Offset: 0x000112C6
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

			// Token: 0x170000FE RID: 254
			// (get) Token: 0x06000FBC RID: 4028 RVA: 0x000130EC File Offset: 0x000112EC
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

			// Token: 0x170000FF RID: 255
			// (get) Token: 0x06000FBD RID: 4029 RVA: 0x0001310A File Offset: 0x0001130A
			// (set) Token: 0x06000FBE RID: 4030 RVA: 0x00013112 File Offset: 0x00011312
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

			// Token: 0x17000100 RID: 256
			// (get) Token: 0x06000FBF RID: 4031 RVA: 0x00013138 File Offset: 0x00011338
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

			// Token: 0x17000101 RID: 257
			// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x00013156 File Offset: 0x00011356
			// (set) Token: 0x06000FC1 RID: 4033 RVA: 0x0001315E File Offset: 0x0001135E
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

			// Token: 0x17000102 RID: 258
			// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x00013187 File Offset: 0x00011387
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

			// Token: 0x17000103 RID: 259
			// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x000131A8 File Offset: 0x000113A8
			// (set) Token: 0x06000FC4 RID: 4036 RVA: 0x000131B0 File Offset: 0x000113B0
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

			// Token: 0x17000104 RID: 260
			// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x000131D9 File Offset: 0x000113D9
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

			// Token: 0x17000105 RID: 261
			// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x000131FA File Offset: 0x000113FA
			// (set) Token: 0x06000FC7 RID: 4039 RVA: 0x00013202 File Offset: 0x00011402
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

			// Token: 0x17000106 RID: 262
			// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x0001322B File Offset: 0x0001142B
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

			// Token: 0x06000FC9 RID: 4041 RVA: 0x0006DF7C File Offset: 0x0006C17C
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

			// Token: 0x06000FCA RID: 4042 RVA: 0x0006E030 File Offset: 0x0006C230
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

			// Token: 0x06000FCB RID: 4043 RVA: 0x0001324C File Offset: 0x0001144C
			private void _TerminalNotificationsListEnsure()
			{
				if (this.terminalNotifications == null)
				{
					this.terminalNotifications = new List<Data.GameData.TerminalNotification>();
				}
			}

			// Token: 0x06000FCC RID: 4044 RVA: 0x00013261 File Offset: 0x00011461
			public bool TerminalNotification_HasAny()
			{
				this._TerminalNotificationsListEnsure();
				return this.terminalNotifications.Count > 0;
			}

			// Token: 0x06000FCD RID: 4045 RVA: 0x00013277 File Offset: 0x00011477
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

			// Token: 0x06000FCE RID: 4046 RVA: 0x000132A9 File Offset: 0x000114A9
			public void TerminalNotification_Set(Data.GameData.TerminalNotification notification)
			{
				this._TerminalNotificationsListEnsure();
				this.terminalNotifications.Add(notification);
			}

			// Token: 0x17000107 RID: 263
			// (get) Token: 0x06000FCF RID: 4047 RVA: 0x000132BD File Offset: 0x000114BD
			public bool AllCardsUnlocked
			{
				get
				{
					return this._allCardsUnlocked;
				}
			}

			// Token: 0x17000108 RID: 264
			// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x000132C5 File Offset: 0x000114C5
			public bool AllCardsHolographic
			{
				get
				{
					return this._allCardsHolographic;
				}
			}

			// Token: 0x06000FD1 RID: 4049 RVA: 0x0006E0E4 File Offset: 0x0006C2E4
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

			// Token: 0x06000FD2 RID: 4050 RVA: 0x0006E124 File Offset: 0x0006C324
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

			// Token: 0x06000FD3 RID: 4051 RVA: 0x0006E160 File Offset: 0x0006C360
			private void RunMod_EnsureAllCapsules()
			{
				int num = 20;
				for (int i = 0; i < num; i++)
				{
					this.RunModCapsuleEnsure((RunModifierScript.Identifier)i);
				}
			}

			// Token: 0x06000FD4 RID: 4052 RVA: 0x0006E184 File Offset: 0x0006C384
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

			// Token: 0x06000FD5 RID: 4053 RVA: 0x000132CD File Offset: 0x000114CD
			private Data.GameData.RunModifierCapsule _RunModifier_GetWorkingCapsule(RunModifierScript.Identifier identifier)
			{
				Data.game.RunModCapsuleEnsure(identifier);
				return Data.game.runModCapsulesDictionary[identifier];
			}

			// Token: 0x06000FD6 RID: 4054 RVA: 0x0006E1D4 File Offset: 0x0006C3D4
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

			// Token: 0x06000FD7 RID: 4055 RVA: 0x0006E1FC File Offset: 0x0006C3FC
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

			// Token: 0x06000FD8 RID: 4056 RVA: 0x0006E220 File Offset: 0x0006C420
			public int RunModifier_UnlockedTimes_Get(RunModifierScript.Identifier identifier)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return 0;
				}
				return runModifierCapsule.unlockedTimes;
			}

			// Token: 0x06000FD9 RID: 4057 RVA: 0x0006E240 File Offset: 0x0006C440
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

			// Token: 0x06000FDA RID: 4058 RVA: 0x0006E268 File Offset: 0x0006C468
			public int RunModifier_PlayedTimes_Get(RunModifierScript.Identifier identifier)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return 0;
				}
				return runModifierCapsule.playedTimes;
			}

			// Token: 0x06000FDB RID: 4059 RVA: 0x0006E288 File Offset: 0x0006C488
			public void RunModifier_PlayedTimes_Set(RunModifierScript.Identifier identifier, int n)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return;
				}
				runModifierCapsule.playedTimes = n;
			}

			// Token: 0x06000FDC RID: 4060 RVA: 0x0006E2A8 File Offset: 0x0006C4A8
			public int RunModifier_WonTimes_Get(RunModifierScript.Identifier identifier)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return 0;
				}
				return runModifierCapsule.wonTimes;
			}

			// Token: 0x06000FDD RID: 4061 RVA: 0x0006E2C8 File Offset: 0x0006C4C8
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

			// Token: 0x06000FDE RID: 4062 RVA: 0x0006E2F8 File Offset: 0x0006C4F8
			public int RunModifier_WonTimesHARDCORE_Get(RunModifierScript.Identifier identifier)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return 0;
				}
				return runModifierCapsule.wonTimesHardcore;
			}

			// Token: 0x06000FDF RID: 4063 RVA: 0x0006E318 File Offset: 0x0006C518
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

			// Token: 0x06000FE0 RID: 4064 RVA: 0x0006E348 File Offset: 0x0006C548
			public int DesiredFoilLevelGet(RunModifierScript.Identifier identifier)
			{
				int num = Data.game.RunModifier_WonTimes_Get(identifier);
				if (Data.game.RunModifier_WonTimesHARDCORE_Get(identifier) > 0)
				{
					return 2;
				}
				if (num > 0)
				{
					return 1;
				}
				return this.RunModifier_FoilLevel_Get(identifier);
			}

			// Token: 0x06000FE1 RID: 4065 RVA: 0x0006E380 File Offset: 0x0006C580
			public int RunModifier_FoilLevel_Get(RunModifierScript.Identifier identifier)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return 0;
				}
				return runModifierCapsule.foilLevel;
			}

			// Token: 0x06000FE2 RID: 4066 RVA: 0x0006E3A0 File Offset: 0x0006C5A0
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

			// Token: 0x06000FE3 RID: 4067 RVA: 0x0006E3C8 File Offset: 0x0006C5C8
			public bool RunModifier_HardcoreMode_Get(RunModifierScript.Identifier identifier)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				return runModifierCapsule != null && runModifierCapsule.isInHardcoreMode;
			}

			// Token: 0x06000FE4 RID: 4068 RVA: 0x0006E3E8 File Offset: 0x0006C5E8
			public void RunModifier_HardcoreMode_Set(RunModifierScript.Identifier identifier, bool value)
			{
				Data.GameData.RunModifierCapsule runModifierCapsule = this._RunModifier_GetWorkingCapsule(identifier);
				if (runModifierCapsule == null)
				{
					return;
				}
				runModifierCapsule.isInHardcoreMode = value;
			}

			// Token: 0x06000FE5 RID: 4069 RVA: 0x0006E408 File Offset: 0x0006C608
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

			// Token: 0x06000FE6 RID: 4070 RVA: 0x0006E49C File Offset: 0x0006C69C
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

			// Token: 0x06000FE7 RID: 4071 RVA: 0x0006E52C File Offset: 0x0006C72C
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

			// Token: 0x06000FE8 RID: 4072 RVA: 0x0006E5B0 File Offset: 0x0006C7B0
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

			// Token: 0x06000FE9 RID: 4073 RVA: 0x0006E640 File Offset: 0x0006C840
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

			// Token: 0x06000FEA RID: 4074 RVA: 0x000132EA File Offset: 0x000114EA
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

			// Token: 0x06000FEB RID: 4075 RVA: 0x0006E6D4 File Offset: 0x0006C8D4
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

			// Token: 0x06000FEC RID: 4076 RVA: 0x0006E74C File Offset: 0x0006C94C
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

			// Token: 0x04000FE6 RID: 4070
			public static Data.GameData inst = null;

			// Token: 0x04000FE7 RID: 4071
			public int myGameDataIndex = -1;

			// Token: 0x04000FE8 RID: 4072
			public int dataOpenedTimes;

			// Token: 0x04000FE9 RID: 4073
			public string lastGameVersionThatSavedMe;

			// Token: 0x04000FEA RID: 4074
			public bool enforceRunReset;

			// Token: 0x04000FEB RID: 4075
			public bool gameplayDataHasSession;

			// Token: 0x04000FEC RID: 4076
			public GameplayData gameplayData = new GameplayData();

			// Token: 0x04000FED RID: 4077
			public int runsDone;

			// Token: 0x04000FEE RID: 4078
			public int deathsDone;

			// Token: 0x04000FEF RID: 4079
			public bool tutorialQuestionEnabled = true;

			// Token: 0x04000FF0 RID: 4080
			public bool[] drawersUnlocked = new bool[4];

			// Token: 0x04000FF1 RID: 4081
			public int doorOpenedCounter;

			// Token: 0x04000FF2 RID: 4082
			public int badEndingCounter;

			// Token: 0x04000FF3 RID: 4083
			public int goodEndingCounter;

			// Token: 0x04000FF4 RID: 4084
			public bool creditsSeenOnce;

			// Token: 0x04000FF5 RID: 4085
			public bool bookedBadEndingDialogue;

			// Token: 0x04000FF6 RID: 4086
			public bool demoVoucherUnlocked;

			// Token: 0x04000FF7 RID: 4087
			public bool jumperinoScarinoDoorino_DoneOnce;

			// Token: 0x04000FF8 RID: 4088
			[SerializeField]
			private int persistentStat_666SeenTimes;

			// Token: 0x04000FF9 RID: 4089
			private List<PowerupScript.Identifier> unlockedPowerups = Data.GameData._UnlockedPowerups_Definition();

			// Token: 0x04000FFA RID: 4090
			[SerializeField]
			private string _unlockedPowerupsString;

			// Token: 0x04000FFB RID: 4091
			private List<PowerupScript.Identifier> lockedPowerups = Data.GameData._LockedPowerups_Definition();

			// Token: 0x04000FFC RID: 4092
			private List<PowerupScript.Identifier> lockedPowerups_ResultingList = Data.GameData._LockedPowerups_ResultingList_Definition();

			// Token: 0x04000FFD RID: 4093
			private bool allPowerupsUnlockedChacheResult;

			// Token: 0x04000FFE RID: 4094
			public bool hasEverUnlockedAPowerup;

			// Token: 0x04000FFF RID: 4095
			public const int UNLSTEPS_MAX_PUPPET_TRAINER = 15;

			// Token: 0x04001000 RID: 4096
			public const int UNLSTEPS_MAX_PUPPET_ELECTRICIAN = 15;

			// Token: 0x04001001 RID: 4097
			public const int UNLSTEPS_MAX_PUPPET_FORTUNE_TELLER = 15;

			// Token: 0x04001002 RID: 4098
			public const int UNLSTEPS_MAX_STEPS_COUNTER = 20;

			// Token: 0x04001003 RID: 4099
			public const int UNLSTEPS_MAX_DEVIL_HORN = 3;

			// Token: 0x04001004 RID: 4100
			public const int UNLSTEPS_MAX_NECRONOMICON = 5;

			// Token: 0x04001005 RID: 4101
			public const int UNLSTEPS_MAX_KING_MIDA = 30;

			// Token: 0x04001006 RID: 4102
			public const int UNLSTEPS_MAX_DEALER = 30;

			// Token: 0x04001007 RID: 4103
			public const int UNLSTEPS_MAX_RAGING_CAPITALIST = 30;

			// Token: 0x04001008 RID: 4104
			private const int UNLSTEPS_MAX_ELECTRICITY_COUNTER = 50;

			// Token: 0x04001009 RID: 4105
			[SerializeField]
			private int unlockSteps_ElectricityCounter;

			// Token: 0x0400100A RID: 4106
			private const int UNLSTEPS_MAX_DARK_LOTUS = 200;

			// Token: 0x0400100B RID: 4107
			[SerializeField]
			private int unlockSteps_DarkLotus;

			// Token: 0x0400100C RID: 4108
			private const int UNLSTEPS_MAX_CLOVERS_LAND_PATCH = 50;

			// Token: 0x0400100D RID: 4109
			[SerializeField]
			private int unlockSteps_CloversLandPatch;

			// Token: 0x0400100E RID: 4110
			private const int UNLSTEPS_MAX_ALL_IN = 100;

			// Token: 0x0400100F RID: 4111
			[SerializeField]
			private int unlockSteps_AllIn;

			// Token: 0x04001010 RID: 4112
			private const int UNLSTEPS_MAX_GARBAGE = 50;

			// Token: 0x04001011 RID: 4113
			[SerializeField]
			private int unlockSteps_Garbage;

			// Token: 0x04001012 RID: 4114
			private const int UNLSTEPS_MAX_VOICE_MAIL = 50;

			// Token: 0x04001013 RID: 4115
			[SerializeField]
			private int unlockSteps_VoiceMail;

			// Token: 0x04001014 RID: 4116
			private const int UNLSTEPS_MAX_FORTUNE_CHANNELER = 100;

			// Token: 0x04001015 RID: 4117
			[SerializeField]
			private int unlockSteps_FortuneChanneler;

			// Token: 0x04001016 RID: 4118
			private const int UNLSTEPS_MAX_HAMSA_UPSIDE = 100;

			// Token: 0x04001017 RID: 4119
			[SerializeField]
			private int unlockSteps_HamsaUpside;

			// Token: 0x04001018 RID: 4120
			private const int UNLSTEPS_MAX_ANCIENT_COIN = 100;

			// Token: 0x04001019 RID: 4121
			[SerializeField]
			private int unlockSteps_AncientCoin;

			// Token: 0x0400101A RID: 4122
			private const int UNLSTEPS_MAX_SARDINES = 200;

			// Token: 0x0400101B RID: 4123
			[SerializeField]
			private int unlockSteps_Sardines;

			// Token: 0x0400101C RID: 4124
			private const int UNLSTEPS_MAX_ROSARY = 10;

			// Token: 0x0400101D RID: 4125
			[SerializeField]
			private int unlockSteps_Rosary;

			// Token: 0x0400101E RID: 4126
			private const int UNLSTEPS_MAX_FORTUNE_COOKIE = 10;

			// Token: 0x0400101F RID: 4127
			[SerializeField]
			private int unlockSteps_FortuneCookie;

			// Token: 0x04001020 RID: 4128
			private const int UNLSTEPS_MAX_YELLOW_STAR = 10;

			// Token: 0x04001021 RID: 4129
			[SerializeField]
			private int unlockSteps_YellowStar;

			// Token: 0x04001022 RID: 4130
			private const int UNLSTEPS_MAX_CROSS = 100;

			// Token: 0x04001023 RID: 4131
			[SerializeField]
			private int unlockSteps_Cross;

			// Token: 0x04001024 RID: 4132
			private const int UNLSTEPS_MAX_CALENDAR = 30;

			// Token: 0x04001025 RID: 4133
			[SerializeField]
			private int unlockSteps_Calendar;

			// Token: 0x04001026 RID: 4134
			private const int UNLSTEPS_MAX_PAINKILLERS = 50;

			// Token: 0x04001027 RID: 4135
			[SerializeField]
			private int unlockSteps_PainKillers;

			// Token: 0x04001028 RID: 4136
			private const int UNLSTEPS_MAX_SCRATCH_AND_WIN = 25;

			// Token: 0x04001029 RID: 4137
			[SerializeField]
			private int unlockSteps_ScratchAndWin;

			// Token: 0x0400102A RID: 4138
			private const int UNLSTEPS_MAX_PHOTO_BOOK = 50;

			// Token: 0x0400102B RID: 4139
			[SerializeField]
			private int unlockSteps_PhotoBook;

			// Token: 0x0400102C RID: 4140
			private const int UNLSTEPS_MAX_BAPHOMET = 666;

			// Token: 0x0400102D RID: 4141
			[SerializeField]
			private int unlockSteps_Baphomet;

			// Token: 0x0400102E RID: 4142
			private const int UNLSTEPS_MAX_DUNG_BEETLE_STERCORARO = 300;

			// Token: 0x0400102F RID: 4143
			[SerializeField]
			private int unlockSteps_DungBeetleStercoRaro;

			// Token: 0x04001030 RID: 4144
			private const int UNLSTEPS_MAX_LUCKY_CAT_FAT = 50;

			// Token: 0x04001031 RID: 4145
			[SerializeField]
			private int unlockSteps_LuckyCatFat;

			// Token: 0x04001032 RID: 4146
			private const int UNLSTEPS_MAX_LUCKY_CAT_SWOLE = 25;

			// Token: 0x04001033 RID: 4147
			[SerializeField]
			private int unlockSteps_LuckyCatSwole;

			// Token: 0x04001034 RID: 4148
			private const int UNLSTEPS_MAX_GOLDEN_HORSESHOE = 7;

			// Token: 0x04001035 RID: 4149
			[SerializeField]
			private int unlockSteps_HorseShoeGold;

			// Token: 0x04001036 RID: 4150
			private const int UNLSTEPS_MAX_GOLDEN_PEPPER = 30;

			// Token: 0x04001037 RID: 4151
			[SerializeField]
			private int unlockSteps_GoldenPepper;

			// Token: 0x04001038 RID: 4152
			private const int UNLSTEPS_MAX_ROTTEN_PEPPER = 20;

			// Token: 0x04001039 RID: 4153
			[SerializeField]
			private int unlockSteps_RottenPepper;

			// Token: 0x0400103A RID: 4154
			private const int UNLSTEPS_MAX_BELL_PEPPER = 30;

			// Token: 0x0400103B RID: 4155
			[SerializeField]
			private int unlockSteps_BellPepper;

			// Token: 0x0400103C RID: 4156
			private const int UNLSTEPS_MAX_ABYSSU = 100;

			// Token: 0x0400103D RID: 4157
			[SerializeField]
			private int unlockSteps_Abyssu;

			// Token: 0x0400103E RID: 4158
			private const int UNLSTEPS_MAX_VORAGO = 100;

			// Token: 0x0400103F RID: 4159
			[SerializeField]
			private int unlockSteps_Vorago;

			// Token: 0x04001040 RID: 4160
			private const int UNLSTEPS_MAX_BARATHRUM = 100;

			// Token: 0x04001041 RID: 4161
			[SerializeField]
			private int unlockSteps_Barathrum;

			// Token: 0x04001042 RID: 4162
			private const int UNLSTEPS_MAX_SUPER_CAPACITOR = 200;

			// Token: 0x04001043 RID: 4163
			[SerializeField]
			private int unlockSteps_SuperCapacitor;

			// Token: 0x04001044 RID: 4164
			private const int UNLSTEPS_MAX_CRANK_GENERATOR = 50;

			// Token: 0x04001045 RID: 4165
			[SerializeField]
			private int unlockSteps_CrankGenerator;

			// Token: 0x04001046 RID: 4166
			private const int UNLSTEPS_MAX_BRICKS = 50;

			// Token: 0x04001047 RID: 4167
			[SerializeField]
			private int unlockSteps_BoardgameC_Bricks;

			// Token: 0x04001048 RID: 4168
			private const int UNLSTEPS_MAX_WOOD = 50;

			// Token: 0x04001049 RID: 4169
			[SerializeField]
			private int unlockSteps_BoardgameC_Wood;

			// Token: 0x0400104A RID: 4170
			private const int UNLSTEPS_MAX_SHEEP = 500;

			// Token: 0x0400104B RID: 4171
			[SerializeField]
			private int unlockSteps_BoardgameC_Sheep;

			// Token: 0x0400104C RID: 4172
			private const int UNLSTEPS_MAX_WHEAT = 500;

			// Token: 0x0400104D RID: 4173
			[SerializeField]
			private int unlockSteps_BoardgameC_Wheat;

			// Token: 0x0400104E RID: 4174
			private const int UNLSTEPS_MAX_STONE = 1000;

			// Token: 0x0400104F RID: 4175
			[SerializeField]
			private int unlockSteps_BoardgameC_Stone;

			// Token: 0x04001050 RID: 4176
			private const int UNLSTEPS_MAX_HARBOR = 1000;

			// Token: 0x04001051 RID: 4177
			[SerializeField]
			private int unlockSteps_BoardgameC_Harbor;

			// Token: 0x04001052 RID: 4178
			private const int UNLSTEPS_MAX_THIEF = 1000;

			// Token: 0x04001053 RID: 4179
			[SerializeField]
			private int unlockSteps_BoardgameC_Thief;

			// Token: 0x04001054 RID: 4180
			private const int UNLSTEPS_MAX_CARRIOLA = 500;

			// Token: 0x04001055 RID: 4181
			[SerializeField]
			private int unlockSteps_BoardgameM_Carriola;

			// Token: 0x04001056 RID: 4182
			private const int UNLSTEPS_MAX_SHOE = 500;

			// Token: 0x04001057 RID: 4183
			[SerializeField]
			private int unlockSteps_BoardgameM_Shoe;

			// Token: 0x04001058 RID: 4184
			private const int UNLSTEPS_MAX_DITALE = 50;

			// Token: 0x04001059 RID: 4185
			[SerializeField]
			private int unlockSteps_BoardgameM_Ditale;

			// Token: 0x0400105A RID: 4186
			private const int UNLSTEPS_MAX_IRON = 50;

			// Token: 0x0400105B RID: 4187
			[SerializeField]
			private int unlockSteps_BoardgameM_Iron;

			// Token: 0x0400105C RID: 4188
			private const int UNLSTEPS_MAX_CAR = 1000;

			// Token: 0x0400105D RID: 4189
			[SerializeField]
			private int unlockSteps_BoardgameM_Car;

			// Token: 0x0400105E RID: 4190
			private const int UNLSTEPS_MAX_SHIP = 1000;

			// Token: 0x0400105F RID: 4191
			[SerializeField]
			private int unlockSteps_BoardgameM_Ship;

			// Token: 0x04001060 RID: 4192
			private const int UNLSTEPS_MAX_TUBA_HAT = 1000;

			// Token: 0x04001061 RID: 4193
			[SerializeField]
			private int unlockSteps_BoardgameM_TubaHat;

			// Token: 0x04001062 RID: 4194
			[SerializeField]
			private long modSymbolTriggersCounter_Lemons;

			// Token: 0x04001063 RID: 4195
			[SerializeField]
			private long modSymbolTriggersCounter_Cherries;

			// Token: 0x04001064 RID: 4196
			[SerializeField]
			private long modSymbolTriggersCounter_Clovers;

			// Token: 0x04001065 RID: 4197
			[SerializeField]
			private long modSymbolTriggersCounter_Bells;

			// Token: 0x04001066 RID: 4198
			[SerializeField]
			private long modSymbolTriggersCounter_Diamonds;

			// Token: 0x04001067 RID: 4199
			[SerializeField]
			private long modSymbolTriggersCounter_Treasures;

			// Token: 0x04001068 RID: 4200
			[SerializeField]
			private long modSymbolTriggersCounter_Sevens;

			// Token: 0x04001069 RID: 4201
			public static PowerupScript.Identifier _terminalNotificationStringPowerup = PowerupScript.Identifier.undefined;

			// Token: 0x0400106A RID: 4202
			[SerializeField]
			private List<Data.GameData.TerminalNotification> terminalNotifications = new List<Data.GameData.TerminalNotification>();

			// Token: 0x0400106B RID: 4203
			[SerializeField]
			private bool _allCardsUnlocked;

			// Token: 0x0400106C RID: 4204
			[SerializeField]
			private bool _allCardsHolographic;

			// Token: 0x0400106D RID: 4205
			[NonSerialized]
			private Dictionary<RunModifierScript.Identifier, Data.GameData.RunModifierCapsule> runModCapsulesDictionary = new Dictionary<RunModifierScript.Identifier, Data.GameData.RunModifierCapsule>();

			// Token: 0x0400106E RID: 4206
			[SerializeField]
			private List<Data.GameData.RunModifierCapsule> _runModSavingList = new List<Data.GameData.RunModifierCapsule>();

			// Token: 0x02000138 RID: 312
			[Serializable]
			public class TerminalNotification
			{
				// Token: 0x06000FEE RID: 4078 RVA: 0x00013320 File Offset: 0x00011520
				public TerminalNotification(PowerupScript.Identifier powerupIdentifier, string titleKey, string messageKey)
				{
					this.powerupIdentifierAsString = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(powerupIdentifier);
					this.titleKey = titleKey;
					this.messageKey = messageKey;
				}

				// Token: 0x06000FEF RID: 4079 RVA: 0x00013342 File Offset: 0x00011542
				public string GetTitle()
				{
					return Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get(this.titleKey), Strings.SanitizationSubKind.none);
				}

				// Token: 0x06000FF0 RID: 4080 RVA: 0x00013356 File Offset: 0x00011556
				public string GetMessage()
				{
					Data.GameData._terminalNotificationStringPowerup = PlatformDataMaster.EnumEntryFromString<PowerupScript.Identifier>(this.powerupIdentifierAsString, PowerupScript.Identifier.undefined);
					return Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get(this.messageKey), Strings.SanitizationSubKind.none);
				}

				// Token: 0x06000FF1 RID: 4081 RVA: 0x0001337B File Offset: 0x0001157B
				public PowerupScript.Identifier GetPowerupIdentifier()
				{
					return PlatformDataMaster.EnumEntryFromString<PowerupScript.Identifier>(this.powerupIdentifierAsString, PowerupScript.Identifier.undefined);
				}

				// Token: 0x0400106F RID: 4207
				[SerializeField]
				private string powerupIdentifierAsString;

				// Token: 0x04001070 RID: 4208
				[SerializeField]
				private string titleKey;

				// Token: 0x04001071 RID: 4209
				[SerializeField]
				private string messageKey;
			}

			// Token: 0x02000139 RID: 313
			[Serializable]
			public class RunModifierCapsule
			{
				// Token: 0x06000FF2 RID: 4082 RVA: 0x00013389 File Offset: 0x00011589
				public RunModifierCapsule(RunModifierScript.Identifier identifier)
				{
					this.runModifierIdentifierAsString = PlatformDataMaster.EnumEntryToString<RunModifierScript.Identifier>(identifier);
				}

				// Token: 0x06000FF3 RID: 4083 RVA: 0x0001339D File Offset: 0x0001159D
				public RunModifierScript.Identifier GetIdentifier()
				{
					return PlatformDataMaster.EnumEntryFromString<RunModifierScript.Identifier>(this.runModifierIdentifierAsString, RunModifierScript.Identifier.undefined);
				}

				// Token: 0x04001072 RID: 4210
				[SerializeField]
				private string runModifierIdentifierAsString;

				// Token: 0x04001073 RID: 4211
				[SerializeField]
				public int ownedCount;

				// Token: 0x04001074 RID: 4212
				[SerializeField]
				public int unlockedTimes;

				// Token: 0x04001075 RID: 4213
				[SerializeField]
				public int playedTimes;

				// Token: 0x04001076 RID: 4214
				[SerializeField]
				public int wonTimes;

				// Token: 0x04001077 RID: 4215
				[SerializeField]
				public int wonTimesHardcore;

				// Token: 0x04001078 RID: 4216
				[SerializeField]
				public int foilLevel;

				// Token: 0x04001079 RID: 4217
				[SerializeField]
				public bool isInHardcoreMode;
			}
		}
	}
}
