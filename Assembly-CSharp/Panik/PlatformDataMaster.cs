using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Panik
{
	// Token: 0x02000153 RID: 339
	public class PlatformDataMaster : MonoBehaviour
	{
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x00013839 File Offset: 0x00011A39
		public static double SaveCooldownTimeGet
		{
			get
			{
				return 0.0;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600105A RID: 4186 RVA: 0x00013839 File Offset: 0x00011A39
		public static double LoadCooldownTimeGet
		{
			get
			{
				return 0.0;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600105B RID: 4187 RVA: 0x00013839 File Offset: 0x00011A39
		public static double DeleteCooldownTimeGet
		{
			get
			{
				return 0.0;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x00013844 File Offset: 0x00011A44
		public static string ExecutablePath
		{
			get
			{
				return Application.dataPath + "/../";
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600105D RID: 4189 RVA: 0x00013855 File Offset: 0x00011A55
		public static string OutsideExecutablePath
		{
			get
			{
				return Application.dataPath + "/../../";
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600105E RID: 4190 RVA: 0x00013866 File Offset: 0x00011A66
		public static string Mount
		{
			get
			{
				return PlatformDataMaster.ExecutablePath;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600105F RID: 4191 RVA: 0x0001386D File Offset: 0x00011A6D
		public static string CommonPath
		{
			get
			{
				return PlatformDataMaster.Mount + "/SaveData/";
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06001060 RID: 4192 RVA: 0x0001387E File Offset: 0x00011A7E
		public static string VersionFolderPath
		{
			get
			{
				return PlatformDataMaster.CommonPath + "/_VD/";
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06001061 RID: 4193 RVA: 0x0001388F File Offset: 0x00011A8F
		public static string SettingsFolderPath
		{
			get
			{
				return PlatformDataMaster.CommonPath + "/SettingsData/";
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06001062 RID: 4194 RVA: 0x000138A0 File Offset: 0x00011AA0
		public static string AchievementsFolderPath
		{
			get
			{
				return PlatformDataMaster.GameFolderPath + "/_TD/";
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06001063 RID: 4195 RVA: 0x000138B1 File Offset: 0x00011AB1
		public static string GameFolderPath
		{
			get
			{
				return PlatformDataMaster.CommonPath + "/GameData/";
			}
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x000138C2 File Offset: 0x00011AC2
		public static string PathGet_VersionFile()
		{
			return PlatformDataMaster.VersionFolderPath + "VersionDataFull.json";
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x000138D3 File Offset: 0x00011AD3
		public static string PathGet_SettingsFile()
		{
			return PlatformDataMaster.SettingsFolderPath + "SettingsDataFull.json";
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x000138E4 File Offset: 0x00011AE4
		public static string PathGet_AchievementsFile()
		{
			return PlatformDataMaster.AchievementsFolderPath + "ADFv2.json";
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x000138F5 File Offset: 0x00011AF5
		public static string PathGet_GameDataFile(int gameDataIndex, string extraAppendix = "")
		{
			return PlatformDataMaster.GameFolderPath + "GameDataFull" + extraAppendix + ".json";
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x0001390C File Offset: 0x00011B0C
		public static string ToJson<T>(T obj)
		{
			return JsonUtility.ToJson(obj, true);
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x000702D0 File Offset: 0x0006E4D0
		public static T FromJson<T>(string json, out bool error)
		{
			string text = null;
			T t = PlatformDataMaster.FromJsonExt<T>(json, out error, out text);
			if (!string.IsNullOrEmpty(text))
			{
				if (PlatformMaster.PlatformIsComputer())
				{
					ConsolePrompt.LogError(text, "", 0f);
				}
				global::UnityEngine.Debug.LogError(text);
			}
			return t;
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x00070310 File Offset: 0x0006E510
		public static T FromJsonExt<T>(string json, out bool error, out string errorMessage)
		{
			T t;
			try
			{
				error = false;
				errorMessage = null;
				t = JsonUtility.FromJson<T>(json);
			}
			catch (Exception ex)
			{
				errorMessage = "PlatformDataMaster.FromJson(): error while parsing from json. Probably corrupted input? ";
				errorMessage = errorMessage + "Error:\n" + ex.Message;
				error = true;
				t = default(T);
			}
			return t;
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00070368 File Offset: 0x0006E568
		public static string EnumArrayToString<T>(T[] enumArray, char separator = ',')
		{
			PlatformDataMaster.enumSB.Clear();
			for (int i = 0; i < enumArray.Length; i++)
			{
				PlatformDataMaster.enumSB.Append(enumArray[i].ToString());
				if (i < enumArray.Length - 1)
				{
					PlatformDataMaster.enumSB.Append(separator);
				}
			}
			return PlatformDataMaster.enumSB.ToString();
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x000703CC File Offset: 0x0006E5CC
		public static T[] EnumArrayFromString<T>(string enumArrayString, char separator = ',')
		{
			List<T> list = PlatformDataMaster.EnumListFromString<T>(enumArrayString, separator);
			if (list == null)
			{
				return null;
			}
			return list.ToArray();
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x000703EC File Offset: 0x0006E5EC
		public static string EnumListToString<T>(List<T> enumList, char separator = ',')
		{
			PlatformDataMaster.enumSB.Clear();
			for (int i = 0; i < enumList.Count; i++)
			{
				StringBuilder stringBuilder = PlatformDataMaster.enumSB;
				T t = enumList[i];
				stringBuilder.Append(t.ToString());
				if (i < enumList.Count - 1)
				{
					PlatformDataMaster.enumSB.Append(separator);
				}
			}
			return PlatformDataMaster.enumSB.ToString();
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x00070458 File Offset: 0x0006E658
		public static List<T> EnumListFromString<T>(string enumListString, char separator = ',')
		{
			List<T> list = new List<T>();
			PlatformDataMaster.EnumListFromString<T>(enumListString, ref list, false, separator);
			return list;
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x00070478 File Offset: 0x0006E678
		public static void EnumListFromString<T>(string enumListString, ref List<T> targetEnumList, bool affectListOnlyIfDataAvailable, char separator = ',')
		{
			if (string.IsNullOrEmpty(enumListString))
			{
				if (!affectListOnlyIfDataAvailable)
				{
					targetEnumList.Clear();
				}
				return;
			}
			string[] array = enumListString.Split(separator, StringSplitOptions.None);
			targetEnumList.Clear();
			for (int i = 0; i < array.Length; i++)
			{
				try
				{
					targetEnumList.Add((T)((object)Enum.Parse(typeof(T), array[i])));
				}
				catch
				{
					global::UnityEngine.Debug.LogWarning("PlatformDataMaster.EnumListFromString(): Failed parsing of string to enum. String: " + array[i]);
				}
			}
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x0001391A File Offset: 0x00011B1A
		public static string EnumEntryToString<T>(T enumEntry)
		{
			return enumEntry.ToString();
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x000704FC File Offset: 0x0006E6FC
		public static T EnumEntryFromString<T>(string enumEntryString, T defaultValue)
		{
			T t;
			try
			{
				t = (T)((object)Enum.Parse(typeof(T), enumEntryString));
			}
			catch (Exception)
			{
				t = defaultValue;
			}
			return t;
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00070538 File Offset: 0x0006E738
		private static int _CryptoShiftsNumber(string password)
		{
			int num = 8;
			for (int i = 0; i < password.Length; i++)
			{
				num += (int)password[i];
			}
			while (num > 16 || num < 8)
			{
				if (num > 16)
				{
					num -= 16;
				}
				if (num < 8)
				{
					num += 8;
				}
			}
			return num;
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x00070580 File Offset: 0x0006E780
		public static string EncryptCustom(string data, string password)
		{
			if (string.IsNullOrEmpty(data))
			{
				global::UnityEngine.Debug.LogWarning("Encrypting empty data. Returning empty string.");
				return "";
			}
			if (string.IsNullOrEmpty(password))
			{
				global::UnityEngine.Debug.LogWarning("Encrypting with empty password. Returning empty string.");
				return "";
			}
			int length = data.Length;
			StringBuilder stringBuilder = new StringBuilder(data);
			int num = PlatformDataMaster._CryptoShiftsNumber(password);
			char[] array = password.ToCharArray();
			char[] array2 = new char[array.Length];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < array.Length; j++)
				{
					int num2 = Mathf.Abs((int)array[j] % array.Length);
					int num3 = Mathf.FloorToInt(Mathf.Repeat((float)(j + num2), (float)array.Length));
					array2[j] = array[num3];
					char[] array3 = array2;
					int num4 = j;
					array3[num4] ^= array[j];
				}
				for (int k = 0; k < length; k++)
				{
					char c = data[k];
					char c2 = array2[k % array.Length];
					char c3 = c ^ c2;
					stringBuilder[k] = c3;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x00070684 File Offset: 0x0006E884
		public static string DecryptCustom(string cryptedData, string password)
		{
			if (string.IsNullOrEmpty(cryptedData))
			{
				global::UnityEngine.Debug.LogWarning("Decrypting empty data. Returning empty string.");
				return "";
			}
			if (string.IsNullOrEmpty(password))
			{
				global::UnityEngine.Debug.LogWarning("Decrypting with empty password. Returning empty string.");
				return "";
			}
			int length = cryptedData.Length;
			StringBuilder stringBuilder = new StringBuilder(cryptedData);
			int num = PlatformDataMaster._CryptoShiftsNumber(password);
			char[] array = password.ToCharArray();
			char[] array2 = new char[array.Length];
			for (int i = 0; i < num; i++)
			{
				for (int j = array.Length - 1; j >= 0; j--)
				{
					int num2 = Mathf.Abs((int)array[j] % array.Length);
					int num3 = Mathf.FloorToInt(Mathf.Repeat((float)(j + num2), (float)array.Length));
					array2[j] = array[num3];
					char[] array3 = array2;
					int num4 = j;
					array3[num4] ^= array[j];
				}
				for (int k = 0; k < length; k++)
				{
					char c = cryptedData[k];
					char c2 = array2[k % array.Length];
					char c3 = c ^ c2;
					stringBuilder[k] = c3;
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x0007078C File Offset: 0x0006E98C
		private static long OperationRecord_Save()
		{
			PlatformDataMaster.operationIDCounter_Save += 1L;
			long num = PlatformDataMaster.operationIDCounter_Save;
			PlatformDataMaster.bookedSaveOperations.Add(num);
			return num;
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00013929 File Offset: 0x00011B29
		private static bool OperationCanGo_Save(long operationID)
		{
			return PlatformDataMaster.bookedSaveOperations.Count > 0 && PlatformDataMaster.bookedSaveOperations[0] == operationID;
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x00013948 File Offset: 0x00011B48
		private static void OperationDoneSet_Save(long operationID)
		{
			PlatformDataMaster.bookedSaveOperations.Remove(operationID);
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x000707B8 File Offset: 0x0006E9B8
		private static long OperationRecord_Load()
		{
			PlatformDataMaster.operationIDCounter_Load += 1L;
			long num = PlatformDataMaster.operationIDCounter_Load;
			PlatformDataMaster.bookedLoadOperations.Add(num);
			return num;
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x00013956 File Offset: 0x00011B56
		private static bool OperationCanGo_Load(long operationID)
		{
			return PlatformDataMaster.bookedLoadOperations.Count > 0 && PlatformDataMaster.bookedLoadOperations[0] == operationID;
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x00013975 File Offset: 0x00011B75
		private static void OperationDoneSet_Load(long operationID)
		{
			PlatformDataMaster.bookedLoadOperations.Remove(operationID);
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x000707E4 File Offset: 0x0006E9E4
		private static long OperationRecord_Delete()
		{
			PlatformDataMaster.operationIDCounter_Delete += 1L;
			long num = PlatformDataMaster.operationIDCounter_Delete;
			PlatformDataMaster.bookedDeleteOperations.Add(num);
			return num;
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x00013983 File Offset: 0x00011B83
		private static bool OperationCanGo_Delete(long operationID)
		{
			return PlatformDataMaster.bookedDeleteOperations.Count > 0 && PlatformDataMaster.bookedDeleteOperations[0] == operationID;
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x000139A2 File Offset: 0x00011BA2
		private static void OperationDoneSet_Delete(long operationID)
		{
			PlatformDataMaster.bookedDeleteOperations.Remove(operationID);
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x000139B0 File Offset: 0x00011BB0
		public static bool IsSaving()
		{
			return PlatformDataMaster.bookedSaveOperations.Count > 0;
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x000139BF File Offset: 0x00011BBF
		public static bool IsLoading()
		{
			return PlatformDataMaster.bookedLoadOperations.Count > 0;
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x000139CE File Offset: 0x00011BCE
		public static bool IsDeleting()
		{
			return PlatformDataMaster.bookedDeleteOperations.Count > 0;
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x000139DD File Offset: 0x00011BDD
		public static bool IsSavingOrLoadingOrDeleting()
		{
			return PlatformDataMaster.IsSaving() || PlatformDataMaster.IsLoading() || PlatformDataMaster.IsDeleting();
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00070810 File Offset: 0x0006EA10
		public static bool EndGameDataIsDoneCheck()
		{
			bool flag = false;
			if (PlatformDataMaster.IsSaving())
			{
				flag = true;
				global::UnityEngine.Debug.LogWarning("EndGameCheck: There is a saving operation going on but we are closing the game! Data might not be saved.");
			}
			if (PlatformDataMaster.IsDeleting())
			{
				flag = true;
				global::UnityEngine.Debug.LogWarning("EndGameCheck: There is a deleting operation going on but we are closing the game! Data might not be deleted.");
			}
			return flag;
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00070848 File Offset: 0x0006EA48
		public static async UniTask<bool> Save(string operationTag, string path, string data)
		{
			long myId = PlatformDataMaster.OperationRecord_Save();
			while (!PlatformDataMaster.OperationCanGo_Save(myId))
			{
				await UniTask.Delay(100 + (int)myId % 10, false, PlayerLoopTiming.Update, default(CancellationToken), false);
			}
			bool savingResult;
			if (PlatformMaster.PlatformIsComputer())
			{
				savingResult = await PlatformDataMaster._Save_Desktop(path, data);
			}
			else
			{
				savingResult = false;
				string text = "Saving on this platform is not implemented yet. Platform: " + PlatformMaster.PlatformKindGet().ToString();
				ConsolePrompt.LogError(text, "", 0f);
				global::UnityEngine.Debug.LogError(text);
			}
			double delay = PlatformDataMaster.SaveCooldownTimeGet;
			PlatformDataMaster.dataStopWatch_Save.Reset();
			PlatformDataMaster.dataStopWatch_Save.Start();
			while ((double)PlatformDataMaster.dataStopWatch_Save.Elapsed.Seconds < delay)
			{
				await UniTask.Delay(1000, false, PlayerLoopTiming.Update, default(CancellationToken), false);
			}
			PlatformDataMaster.OperationDoneSet_Save(myId);
			return savingResult;
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00070894 File Offset: 0x0006EA94
		private static async UniTask<bool> _Save_Desktop(string path, string data)
		{
			bool flag;
			try
			{
				string directoryName = Path.GetDirectoryName(path);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.WriteAllText(path, data);
				flag = true;
			}
			catch (Exception)
			{
				global::UnityEngine.Debug.LogWarning("Failed to save file at path: " + path);
				ConsolePrompt.LogWarning("Failed to save file at path: " + path, "");
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x000708E0 File Offset: 0x0006EAE0
		private static async UniTask<bool> _Save_WebGl(string path, string data)
		{
			bool flag;
			try
			{
				PlayerPrefs.SetString(path, data);
				flag = true;
			}
			catch (Exception)
			{
				global::UnityEngine.Debug.LogWarning("Failed to save file at path: " + path);
				ConsolePrompt.LogWarning("Failed to save file at path: " + path, "");
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0007092C File Offset: 0x0006EB2C
		private static async UniTask<bool> _Save_NintendoSwitch(string path, string data)
		{
			global::UnityEngine.Debug.LogError("_Save_NintendoSwitch called while not compiling for Nintendo Switch. This should not happen.");
			return 0;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00070968 File Offset: 0x0006EB68
		public static async UniTask<string> Load(string operationTag, string path)
		{
			string loadedData = "";
			long myId = PlatformDataMaster.OperationRecord_Load();
			while (!PlatformDataMaster.OperationCanGo_Load(myId))
			{
				await UniTask.Delay(100 + (int)myId % 10, false, PlayerLoopTiming.Update, default(CancellationToken), false);
			}
			if (PlatformMaster.PlatformIsComputer())
			{
				loadedData = await PlatformDataMaster._Load_Desktop(path);
			}
			else
			{
				loadedData = "";
				string text = "Loading on this platform is not implemented yet. Platform: " + PlatformMaster.PlatformKindGet().ToString();
				ConsolePrompt.LogError(text, "", 0f);
				global::UnityEngine.Debug.LogError(text);
			}
			double delay = PlatformDataMaster.LoadCooldownTimeGet;
			PlatformDataMaster.dataStopWatch_Load.Reset();
			PlatformDataMaster.dataStopWatch_Load.Start();
			while ((double)PlatformDataMaster.dataStopWatch_Load.Elapsed.Seconds < delay)
			{
				await UniTask.Delay(1000, false, PlayerLoopTiming.Update, default(CancellationToken), false);
			}
			PlatformDataMaster.OperationDoneSet_Load(myId);
			return loadedData;
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x000709AC File Offset: 0x0006EBAC
		private static async UniTask<string> _Load_Desktop(string path)
		{
			string text;
			try
			{
				string directoryName = Path.GetDirectoryName(path);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				if (!File.Exists(path))
				{
					text = "";
				}
				else
				{
					text = File.ReadAllText(path);
				}
			}
			catch (Exception)
			{
				text = "";
			}
			return text;
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x000709F0 File Offset: 0x0006EBF0
		private static async UniTask<string> _Load_WebGl(string path)
		{
			string text;
			try
			{
				text = PlayerPrefs.GetString(path, "");
			}
			catch (Exception)
			{
				text = "";
			}
			return text;
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x00070A34 File Offset: 0x0006EC34
		private static async UniTask<string> _Load_NintendoSwitch(string path)
		{
			global::UnityEngine.Debug.LogError("_Load_NintendoSwitch called while not compiling for Nintendo Switch. This should not happen.");
			return "";
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x00070A70 File Offset: 0x0006EC70
		public static async UniTask<bool> Delete(string path)
		{
			long myId = PlatformDataMaster.OperationRecord_Delete();
			while (!PlatformDataMaster.OperationCanGo_Delete(myId))
			{
				await UniTask.Delay(100 + (int)myId % 10, false, PlayerLoopTiming.Update, default(CancellationToken), false);
			}
			bool result;
			if (PlatformMaster.PlatformIsComputer())
			{
				result = await PlatformDataMaster._Delete_Desktop(path);
			}
			else
			{
				result = false;
				string text = "Deleting on this platform is not implemented yet. Platform: " + PlatformMaster.PlatformKindGet().ToString();
				ConsolePrompt.LogError(text, "", 0f);
				global::UnityEngine.Debug.LogError(text);
			}
			double delay = PlatformDataMaster.DeleteCooldownTimeGet;
			PlatformDataMaster.dataStopWatch_Delete.Reset();
			PlatformDataMaster.dataStopWatch_Delete.Start();
			while ((double)PlatformDataMaster.dataStopWatch_Delete.Elapsed.Seconds < delay)
			{
				await UniTask.Delay(1000, false, PlayerLoopTiming.Update, default(CancellationToken), false);
			}
			PlatformDataMaster.OperationDoneSet_Delete(myId);
			return result;
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00070AB4 File Offset: 0x0006ECB4
		private static async UniTask<bool> _Delete_Desktop(string path)
		{
			bool flag;
			try
			{
				string directoryName = Path.GetDirectoryName(path);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				if (!File.Exists(path))
				{
					flag = true;
				}
				else
				{
					File.Delete(path);
					flag = true;
				}
			}
			catch (Exception)
			{
				global::UnityEngine.Debug.LogWarning("Failed to delete file at path: " + path);
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00070AF8 File Offset: 0x0006ECF8
		private static async UniTask<bool> _Delete_WebGl(string path)
		{
			bool flag;
			try
			{
				if (!PlayerPrefs.HasKey(path))
				{
					PlayerPrefs.DeleteKey(path);
				}
				flag = true;
			}
			catch (Exception)
			{
				global::UnityEngine.Debug.LogWarning("Failed to delete file at player preference key: " + path);
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x00070B3C File Offset: 0x0006ED3C
		private static async UniTask<bool> _Delete_NintendoSwitch(string path)
		{
			global::UnityEngine.Debug.LogError("_Delete_NintendoSwitch called while not compiling for Nintendo Switch. This should not happen.");
			return 0;
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00070B78 File Offset: 0x0006ED78
		public static async UniTask<bool> SaveVersionData(string data)
		{
			return await PlatformDataMaster.Save("_VersionData", PlatformDataMaster.PathGet_VersionFile(), data);
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00070BBC File Offset: 0x0006EDBC
		public static async UniTask<string> LoadVersionData()
		{
			return await PlatformDataMaster.Load("_VersionData", PlatformDataMaster.PathGet_VersionFile());
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00070BF8 File Offset: 0x0006EDF8
		public static async UniTask<bool> DeleteVersionData()
		{
			return await PlatformDataMaster.Delete(PlatformDataMaster.PathGet_VersionFile());
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00070C34 File Offset: 0x0006EE34
		public static async UniTask<bool> SaveSettingsData(string data)
		{
			return await PlatformDataMaster.Save("Settings", PlatformDataMaster.PathGet_SettingsFile(), data);
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x00070C78 File Offset: 0x0006EE78
		public static async UniTask<string> LoadSettingsData()
		{
			return await PlatformDataMaster.Load("Settings", PlatformDataMaster.PathGet_SettingsFile());
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00070CB4 File Offset: 0x0006EEB4
		public static async UniTask<bool> DeleteSettingsData()
		{
			return await PlatformDataMaster.Delete(PlatformDataMaster.PathGet_SettingsFile());
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x00070CF0 File Offset: 0x0006EEF0
		public static async UniTask<bool> SaveAchievementsData(string data)
		{
			return await PlatformDataMaster.Save("Achievements", PlatformDataMaster.PathGet_AchievementsFile(), data);
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x00070D34 File Offset: 0x0006EF34
		public static async UniTask<string> LoadAchievementsData()
		{
			return await PlatformDataMaster.Load("Achievements", PlatformDataMaster.PathGet_AchievementsFile());
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x00070D70 File Offset: 0x0006EF70
		public static async UniTask<bool> DeleteAchievementsData()
		{
			return await PlatformDataMaster.Delete(PlatformDataMaster.PathGet_AchievementsFile());
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x00070DAC File Offset: 0x0006EFAC
		public static async UniTask<bool> SaveGameData(string data, int gameDataIndex)
		{
			return await PlatformDataMaster.Save("GameData" + gameDataIndex.ToString(), PlatformDataMaster.PathGet_GameDataFile(gameDataIndex, ""), data);
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x00070DF8 File Offset: 0x0006EFF8
		public static async UniTask<string> LoadGameData(int gameDataIndex)
		{
			return await PlatformDataMaster.Load("GameData" + gameDataIndex.ToString(), PlatformDataMaster.PathGet_GameDataFile(gameDataIndex, ""));
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x00070E3C File Offset: 0x0006F03C
		public static async UniTask<bool> DeleteGameData(int gameDataIndex)
		{
			return await PlatformDataMaster.Delete(PlatformDataMaster.PathGet_GameDataFile(gameDataIndex, ""));
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x000139F4 File Offset: 0x00011BF4
		private void Awake()
		{
			if (PlatformDataMaster.instance != null)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			PlatformDataMaster.instance = this;
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x00013A15 File Offset: 0x00011C15
		private void OnDestroy()
		{
			if (PlatformDataMaster.instance == this)
			{
				PlatformDataMaster.instance = null;
				return;
			}
		}

		// Token: 0x0400113C RID: 4412
		public static PlatformDataMaster instance;

		// Token: 0x0400113D RID: 4413
		public const int CRYPTOGRAPHY_MIN_SHIFTS = 8;

		// Token: 0x0400113E RID: 4414
		public const int CRYPTOGRAPHY_MAX_SHIFTS = 16;

		// Token: 0x0400113F RID: 4415
		public const bool EDITOR_LOG_DATA = true;

		// Token: 0x04001140 RID: 4416
		private const int OP_MS_DELAY = 100;

		// Token: 0x04001141 RID: 4417
		public const string LOAD_FAILED_DATA_VALUE = "";

		// Token: 0x04001142 RID: 4418
		public const string VERSION_DATA_FILE_EXTENSION = ".json";

		// Token: 0x04001143 RID: 4419
		public const string SETTINGS_DATA_FILE_EXTENSION = ".json";

		// Token: 0x04001144 RID: 4420
		public const string ACHIEVEMENTS_DATA_FILE_EXTENSION = ".json";

		// Token: 0x04001145 RID: 4421
		public const string GAME_DATA_FILE_EXTENSION = ".json";

		// Token: 0x04001146 RID: 4422
		public const string GAME_DATA_NAME_DEMO = "GameDataDemo";

		// Token: 0x04001147 RID: 4423
		public const string GAME_DATA_NAME_FULL = "GameDataFull";

		// Token: 0x04001148 RID: 4424
		private static StringBuilder enumSB = new StringBuilder(1024);

		// Token: 0x04001149 RID: 4425
		private static Stopwatch dataStopWatch_Save = new Stopwatch();

		// Token: 0x0400114A RID: 4426
		private static Stopwatch dataStopWatch_Load = new Stopwatch();

		// Token: 0x0400114B RID: 4427
		private static Stopwatch dataStopWatch_Delete = new Stopwatch();

		// Token: 0x0400114C RID: 4428
		private static long operationIDCounter_Save = 0L;

		// Token: 0x0400114D RID: 4429
		private static long operationIDCounter_Load = 0L;

		// Token: 0x0400114E RID: 4430
		private static long operationIDCounter_Delete = 0L;

		// Token: 0x0400114F RID: 4431
		private static List<long> bookedSaveOperations = new List<long>();

		// Token: 0x04001150 RID: 4432
		private static List<long> bookedLoadOperations = new List<long>();

		// Token: 0x04001151 RID: 4433
		private static List<long> bookedDeleteOperations = new List<long>();
	}
}
