using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;

namespace Panik
{
	public class PlatformDataMaster : MonoBehaviour
	{
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00053AEA File Offset: 0x00051CEA
		public static double SaveCooldownTimeGet
		{
			get
			{
				return 0.0;
			}
		}

		// (get) Token: 0x06000D03 RID: 3331 RVA: 0x00053AF5 File Offset: 0x00051CF5
		public static double LoadCooldownTimeGet
		{
			get
			{
				return 0.0;
			}
		}

		// (get) Token: 0x06000D04 RID: 3332 RVA: 0x00053B00 File Offset: 0x00051D00
		public static double DeleteCooldownTimeGet
		{
			get
			{
				return 0.0;
			}
		}

		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x00053B0B File Offset: 0x00051D0B
		public static string ExecutablePath
		{
			get
			{
				return Application.dataPath + "/../";
			}
		}

		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00053B1C File Offset: 0x00051D1C
		public static string OutsideExecutablePath
		{
			get
			{
				return Application.dataPath + "/../../";
			}
		}

		// (get) Token: 0x06000D07 RID: 3335 RVA: 0x00053B2D File Offset: 0x00051D2D
		public static string Mount
		{
			get
			{
				return PlatformDataMaster.ExecutablePath;
			}
		}

		// (get) Token: 0x06000D08 RID: 3336 RVA: 0x00053B34 File Offset: 0x00051D34
		public static string CommonPath
		{
			get
			{
				return PlatformDataMaster.Mount + "/SaveData/";
			}
		}

		// (get) Token: 0x06000D09 RID: 3337 RVA: 0x00053B45 File Offset: 0x00051D45
		public static string VersionFolderPath
		{
			get
			{
				return PlatformDataMaster.CommonPath + "/_VD/";
			}
		}

		// (get) Token: 0x06000D0A RID: 3338 RVA: 0x00053B56 File Offset: 0x00051D56
		public static string SettingsFolderPath
		{
			get
			{
				return PlatformDataMaster.CommonPath + "/SettingsData/";
			}
		}

		// (get) Token: 0x06000D0B RID: 3339 RVA: 0x00053B67 File Offset: 0x00051D67
		public static string AchievementsFolderPath
		{
			get
			{
				return PlatformDataMaster.GameFolderPath + "/_TD/";
			}
		}

		// (get) Token: 0x06000D0C RID: 3340 RVA: 0x00053B78 File Offset: 0x00051D78
		public static string GameFolderPath
		{
			get
			{
				return PlatformDataMaster.CommonPath + "/GameData/";
			}
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00053B89 File Offset: 0x00051D89
		public static string PathGet_VersionFile()
		{
			return PlatformDataMaster.VersionFolderPath + "VersionDataFull.json";
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00053B9A File Offset: 0x00051D9A
		public static string PathGet_SettingsFile()
		{
			return PlatformDataMaster.SettingsFolderPath + "SettingsDataFull.json";
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00053BAB File Offset: 0x00051DAB
		public static string PathGet_AchievementsFile()
		{
			return PlatformDataMaster.AchievementsFolderPath + "ADFv2.json";
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00053BBC File Offset: 0x00051DBC
		public static string PathGet_GameDataFile(int gameDataIndex, string extraAppendix = "")
		{
			return PlatformDataMaster.GameFolderPath + "GameDataFull" + extraAppendix + ".json";
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00053BD3 File Offset: 0x00051DD3
		public static string ToJson<T>(T obj)
		{
			return JsonUtility.ToJson(obj, true);
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00053BE4 File Offset: 0x00051DE4
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

		// Token: 0x06000D13 RID: 3347 RVA: 0x00053C24 File Offset: 0x00051E24
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

		// Token: 0x06000D14 RID: 3348 RVA: 0x00053C7C File Offset: 0x00051E7C
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

		// Token: 0x06000D15 RID: 3349 RVA: 0x00053CE0 File Offset: 0x00051EE0
		public static T[] EnumArrayFromString<T>(string enumArrayString, char separator = ',')
		{
			List<T> list = PlatformDataMaster.EnumListFromString<T>(enumArrayString, separator);
			if (list == null)
			{
				return null;
			}
			return list.ToArray();
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00053D00 File Offset: 0x00051F00
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

		// Token: 0x06000D17 RID: 3351 RVA: 0x00053D6C File Offset: 0x00051F6C
		public static List<T> EnumListFromString<T>(string enumListString, char separator = ',')
		{
			List<T> list = new List<T>();
			PlatformDataMaster.EnumListFromString<T>(enumListString, ref list, false, separator);
			return list;
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00053D8C File Offset: 0x00051F8C
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

		// Token: 0x06000D19 RID: 3353 RVA: 0x00053E10 File Offset: 0x00052010
		public static string EnumEntryToString<T>(T enumEntry)
		{
			return enumEntry.ToString();
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00053E20 File Offset: 0x00052020
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

		// Token: 0x06000D1B RID: 3355 RVA: 0x00053E5C File Offset: 0x0005205C
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

		// Token: 0x06000D1C RID: 3356 RVA: 0x00053EA4 File Offset: 0x000520A4
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

		// Token: 0x06000D1D RID: 3357 RVA: 0x00053FA8 File Offset: 0x000521A8
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

		// Token: 0x06000D1E RID: 3358 RVA: 0x000540B0 File Offset: 0x000522B0
		private static long OperationRecord_Save()
		{
			PlatformDataMaster.operationIDCounter_Save += 1L;
			long num = PlatformDataMaster.operationIDCounter_Save;
			PlatformDataMaster.bookedSaveOperations.Add(num);
			return num;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x000540DC File Offset: 0x000522DC
		private static bool OperationCanGo_Save(long operationID)
		{
			return PlatformDataMaster.bookedSaveOperations.Count > 0 && PlatformDataMaster.bookedSaveOperations[0] == operationID;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x000540FB File Offset: 0x000522FB
		private static void OperationDoneSet_Save(long operationID)
		{
			PlatformDataMaster.bookedSaveOperations.Remove(operationID);
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0005410C File Offset: 0x0005230C
		private static long OperationRecord_Load()
		{
			PlatformDataMaster.operationIDCounter_Load += 1L;
			long num = PlatformDataMaster.operationIDCounter_Load;
			PlatformDataMaster.bookedLoadOperations.Add(num);
			return num;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00054138 File Offset: 0x00052338
		private static bool OperationCanGo_Load(long operationID)
		{
			return PlatformDataMaster.bookedLoadOperations.Count > 0 && PlatformDataMaster.bookedLoadOperations[0] == operationID;
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00054157 File Offset: 0x00052357
		private static void OperationDoneSet_Load(long operationID)
		{
			PlatformDataMaster.bookedLoadOperations.Remove(operationID);
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00054168 File Offset: 0x00052368
		private static long OperationRecord_Delete()
		{
			PlatformDataMaster.operationIDCounter_Delete += 1L;
			long num = PlatformDataMaster.operationIDCounter_Delete;
			PlatformDataMaster.bookedDeleteOperations.Add(num);
			return num;
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00054194 File Offset: 0x00052394
		private static bool OperationCanGo_Delete(long operationID)
		{
			return PlatformDataMaster.bookedDeleteOperations.Count > 0 && PlatformDataMaster.bookedDeleteOperations[0] == operationID;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x000541B3 File Offset: 0x000523B3
		private static void OperationDoneSet_Delete(long operationID)
		{
			PlatformDataMaster.bookedDeleteOperations.Remove(operationID);
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x000541C1 File Offset: 0x000523C1
		public static bool IsSaving()
		{
			return PlatformDataMaster.bookedSaveOperations.Count > 0;
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x000541D0 File Offset: 0x000523D0
		public static bool IsLoading()
		{
			return PlatformDataMaster.bookedLoadOperations.Count > 0;
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x000541DF File Offset: 0x000523DF
		public static bool IsDeleting()
		{
			return PlatformDataMaster.bookedDeleteOperations.Count > 0;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x000541EE File Offset: 0x000523EE
		public static bool IsSavingOrLoadingOrDeleting()
		{
			return PlatformDataMaster.IsSaving() || PlatformDataMaster.IsLoading() || PlatformDataMaster.IsDeleting();
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00054208 File Offset: 0x00052408
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

		// Token: 0x06000D2C RID: 3372 RVA: 0x00054240 File Offset: 0x00052440
		public static UniTask<bool> Save(string operationTag, string path, string data)
		{
			PlatformDataMaster.<Save>d__75 <Save>d__;
			<Save>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Save>d__.path = path;
			<Save>d__.data = data;
			<Save>d__.<>1__state = -1;
			<Save>d__.<>t__builder.Start<PlatformDataMaster.<Save>d__75>(ref <Save>d__);
			return <Save>d__.<>t__builder.Task;
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0005428C File Offset: 0x0005248C
		private static UniTask<bool> _Save_Desktop(string path, string data)
		{
			PlatformDataMaster.<_Save_Desktop>d__76 <_Save_Desktop>d__;
			<_Save_Desktop>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<_Save_Desktop>d__.path = path;
			<_Save_Desktop>d__.data = data;
			<_Save_Desktop>d__.<>1__state = -1;
			<_Save_Desktop>d__.<>t__builder.Start<PlatformDataMaster.<_Save_Desktop>d__76>(ref <_Save_Desktop>d__);
			return <_Save_Desktop>d__.<>t__builder.Task;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x000542D8 File Offset: 0x000524D8
		private static UniTask<bool> _Save_WebGl(string path, string data)
		{
			PlatformDataMaster.<_Save_WebGl>d__77 <_Save_WebGl>d__;
			<_Save_WebGl>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<_Save_WebGl>d__.path = path;
			<_Save_WebGl>d__.data = data;
			<_Save_WebGl>d__.<>1__state = -1;
			<_Save_WebGl>d__.<>t__builder.Start<PlatformDataMaster.<_Save_WebGl>d__77>(ref <_Save_WebGl>d__);
			return <_Save_WebGl>d__.<>t__builder.Task;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x00054324 File Offset: 0x00052524
		private static UniTask<bool> _Save_NintendoSwitch(string path, string data)
		{
			PlatformDataMaster.<_Save_NintendoSwitch>d__78 <_Save_NintendoSwitch>d__;
			<_Save_NintendoSwitch>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<_Save_NintendoSwitch>d__.<>1__state = -1;
			<_Save_NintendoSwitch>d__.<>t__builder.Start<PlatformDataMaster.<_Save_NintendoSwitch>d__78>(ref <_Save_NintendoSwitch>d__);
			return <_Save_NintendoSwitch>d__.<>t__builder.Task;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x00054360 File Offset: 0x00052560
		public static UniTask<string> Load(string operationTag, string path)
		{
			PlatformDataMaster.<Load>d__79 <Load>d__;
			<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<Load>d__.path = path;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<PlatformDataMaster.<Load>d__79>(ref <Load>d__);
			return <Load>d__.<>t__builder.Task;
		}

		// Token: 0x06000D31 RID: 3377 RVA: 0x000543A4 File Offset: 0x000525A4
		private static UniTask<string> _Load_Desktop(string path)
		{
			PlatformDataMaster.<_Load_Desktop>d__80 <_Load_Desktop>d__;
			<_Load_Desktop>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<_Load_Desktop>d__.path = path;
			<_Load_Desktop>d__.<>1__state = -1;
			<_Load_Desktop>d__.<>t__builder.Start<PlatformDataMaster.<_Load_Desktop>d__80>(ref <_Load_Desktop>d__);
			return <_Load_Desktop>d__.<>t__builder.Task;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x000543E8 File Offset: 0x000525E8
		private static UniTask<string> _Load_WebGl(string path)
		{
			PlatformDataMaster.<_Load_WebGl>d__81 <_Load_WebGl>d__;
			<_Load_WebGl>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<_Load_WebGl>d__.path = path;
			<_Load_WebGl>d__.<>1__state = -1;
			<_Load_WebGl>d__.<>t__builder.Start<PlatformDataMaster.<_Load_WebGl>d__81>(ref <_Load_WebGl>d__);
			return <_Load_WebGl>d__.<>t__builder.Task;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x0005442C File Offset: 0x0005262C
		private static UniTask<string> _Load_NintendoSwitch(string path)
		{
			PlatformDataMaster.<_Load_NintendoSwitch>d__82 <_Load_NintendoSwitch>d__;
			<_Load_NintendoSwitch>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<_Load_NintendoSwitch>d__.<>1__state = -1;
			<_Load_NintendoSwitch>d__.<>t__builder.Start<PlatformDataMaster.<_Load_NintendoSwitch>d__82>(ref <_Load_NintendoSwitch>d__);
			return <_Load_NintendoSwitch>d__.<>t__builder.Task;
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00054468 File Offset: 0x00052668
		public static UniTask<bool> Delete(string path)
		{
			PlatformDataMaster.<Delete>d__83 <Delete>d__;
			<Delete>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Delete>d__.path = path;
			<Delete>d__.<>1__state = -1;
			<Delete>d__.<>t__builder.Start<PlatformDataMaster.<Delete>d__83>(ref <Delete>d__);
			return <Delete>d__.<>t__builder.Task;
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x000544AC File Offset: 0x000526AC
		private static UniTask<bool> _Delete_Desktop(string path)
		{
			PlatformDataMaster.<_Delete_Desktop>d__84 <_Delete_Desktop>d__;
			<_Delete_Desktop>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<_Delete_Desktop>d__.path = path;
			<_Delete_Desktop>d__.<>1__state = -1;
			<_Delete_Desktop>d__.<>t__builder.Start<PlatformDataMaster.<_Delete_Desktop>d__84>(ref <_Delete_Desktop>d__);
			return <_Delete_Desktop>d__.<>t__builder.Task;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x000544F0 File Offset: 0x000526F0
		private static UniTask<bool> _Delete_WebGl(string path)
		{
			PlatformDataMaster.<_Delete_WebGl>d__85 <_Delete_WebGl>d__;
			<_Delete_WebGl>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<_Delete_WebGl>d__.path = path;
			<_Delete_WebGl>d__.<>1__state = -1;
			<_Delete_WebGl>d__.<>t__builder.Start<PlatformDataMaster.<_Delete_WebGl>d__85>(ref <_Delete_WebGl>d__);
			return <_Delete_WebGl>d__.<>t__builder.Task;
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00054534 File Offset: 0x00052734
		private static UniTask<bool> _Delete_NintendoSwitch(string path)
		{
			PlatformDataMaster.<_Delete_NintendoSwitch>d__86 <_Delete_NintendoSwitch>d__;
			<_Delete_NintendoSwitch>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<_Delete_NintendoSwitch>d__.<>1__state = -1;
			<_Delete_NintendoSwitch>d__.<>t__builder.Start<PlatformDataMaster.<_Delete_NintendoSwitch>d__86>(ref <_Delete_NintendoSwitch>d__);
			return <_Delete_NintendoSwitch>d__.<>t__builder.Task;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00054570 File Offset: 0x00052770
		public static UniTask<bool> SaveVersionData(string data)
		{
			PlatformDataMaster.<SaveVersionData>d__87 <SaveVersionData>d__;
			<SaveVersionData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<SaveVersionData>d__.data = data;
			<SaveVersionData>d__.<>1__state = -1;
			<SaveVersionData>d__.<>t__builder.Start<PlatformDataMaster.<SaveVersionData>d__87>(ref <SaveVersionData>d__);
			return <SaveVersionData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x000545B4 File Offset: 0x000527B4
		public static UniTask<string> LoadVersionData()
		{
			PlatformDataMaster.<LoadVersionData>d__88 <LoadVersionData>d__;
			<LoadVersionData>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<LoadVersionData>d__.<>1__state = -1;
			<LoadVersionData>d__.<>t__builder.Start<PlatformDataMaster.<LoadVersionData>d__88>(ref <LoadVersionData>d__);
			return <LoadVersionData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x000545F0 File Offset: 0x000527F0
		public static UniTask<bool> DeleteVersionData()
		{
			PlatformDataMaster.<DeleteVersionData>d__89 <DeleteVersionData>d__;
			<DeleteVersionData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<DeleteVersionData>d__.<>1__state = -1;
			<DeleteVersionData>d__.<>t__builder.Start<PlatformDataMaster.<DeleteVersionData>d__89>(ref <DeleteVersionData>d__);
			return <DeleteVersionData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0005462C File Offset: 0x0005282C
		public static UniTask<bool> SaveSettingsData(string data)
		{
			PlatformDataMaster.<SaveSettingsData>d__90 <SaveSettingsData>d__;
			<SaveSettingsData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<SaveSettingsData>d__.data = data;
			<SaveSettingsData>d__.<>1__state = -1;
			<SaveSettingsData>d__.<>t__builder.Start<PlatformDataMaster.<SaveSettingsData>d__90>(ref <SaveSettingsData>d__);
			return <SaveSettingsData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00054670 File Offset: 0x00052870
		public static UniTask<string> LoadSettingsData()
		{
			PlatformDataMaster.<LoadSettingsData>d__91 <LoadSettingsData>d__;
			<LoadSettingsData>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<LoadSettingsData>d__.<>1__state = -1;
			<LoadSettingsData>d__.<>t__builder.Start<PlatformDataMaster.<LoadSettingsData>d__91>(ref <LoadSettingsData>d__);
			return <LoadSettingsData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x000546AC File Offset: 0x000528AC
		public static UniTask<bool> DeleteSettingsData()
		{
			PlatformDataMaster.<DeleteSettingsData>d__92 <DeleteSettingsData>d__;
			<DeleteSettingsData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<DeleteSettingsData>d__.<>1__state = -1;
			<DeleteSettingsData>d__.<>t__builder.Start<PlatformDataMaster.<DeleteSettingsData>d__92>(ref <DeleteSettingsData>d__);
			return <DeleteSettingsData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x000546E8 File Offset: 0x000528E8
		public static UniTask<bool> SaveAchievementsData(string data)
		{
			PlatformDataMaster.<SaveAchievementsData>d__93 <SaveAchievementsData>d__;
			<SaveAchievementsData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<SaveAchievementsData>d__.data = data;
			<SaveAchievementsData>d__.<>1__state = -1;
			<SaveAchievementsData>d__.<>t__builder.Start<PlatformDataMaster.<SaveAchievementsData>d__93>(ref <SaveAchievementsData>d__);
			return <SaveAchievementsData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0005472C File Offset: 0x0005292C
		public static UniTask<string> LoadAchievementsData()
		{
			PlatformDataMaster.<LoadAchievementsData>d__94 <LoadAchievementsData>d__;
			<LoadAchievementsData>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<LoadAchievementsData>d__.<>1__state = -1;
			<LoadAchievementsData>d__.<>t__builder.Start<PlatformDataMaster.<LoadAchievementsData>d__94>(ref <LoadAchievementsData>d__);
			return <LoadAchievementsData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00054768 File Offset: 0x00052968
		public static UniTask<bool> DeleteAchievementsData()
		{
			PlatformDataMaster.<DeleteAchievementsData>d__95 <DeleteAchievementsData>d__;
			<DeleteAchievementsData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<DeleteAchievementsData>d__.<>1__state = -1;
			<DeleteAchievementsData>d__.<>t__builder.Start<PlatformDataMaster.<DeleteAchievementsData>d__95>(ref <DeleteAchievementsData>d__);
			return <DeleteAchievementsData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x000547A4 File Offset: 0x000529A4
		public static UniTask<bool> SaveGameData(string data, int gameDataIndex)
		{
			PlatformDataMaster.<SaveGameData>d__96 <SaveGameData>d__;
			<SaveGameData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<SaveGameData>d__.data = data;
			<SaveGameData>d__.gameDataIndex = gameDataIndex;
			<SaveGameData>d__.<>1__state = -1;
			<SaveGameData>d__.<>t__builder.Start<PlatformDataMaster.<SaveGameData>d__96>(ref <SaveGameData>d__);
			return <SaveGameData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x000547F0 File Offset: 0x000529F0
		public static UniTask<string> LoadGameData(int gameDataIndex)
		{
			PlatformDataMaster.<LoadGameData>d__97 <LoadGameData>d__;
			<LoadGameData>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<LoadGameData>d__.gameDataIndex = gameDataIndex;
			<LoadGameData>d__.<>1__state = -1;
			<LoadGameData>d__.<>t__builder.Start<PlatformDataMaster.<LoadGameData>d__97>(ref <LoadGameData>d__);
			return <LoadGameData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x00054834 File Offset: 0x00052A34
		public static UniTask<bool> DeleteGameData(int gameDataIndex)
		{
			PlatformDataMaster.<DeleteGameData>d__98 <DeleteGameData>d__;
			<DeleteGameData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<DeleteGameData>d__.gameDataIndex = gameDataIndex;
			<DeleteGameData>d__.<>1__state = -1;
			<DeleteGameData>d__.<>t__builder.Start<PlatformDataMaster.<DeleteGameData>d__98>(ref <DeleteGameData>d__);
			return <DeleteGameData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00054877 File Offset: 0x00052A77
		private void Awake()
		{
			if (PlatformDataMaster.instance != null)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			PlatformDataMaster.instance = this;
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00054898 File Offset: 0x00052A98
		private void OnDestroy()
		{
			if (PlatformDataMaster.instance == this)
			{
				PlatformDataMaster.instance = null;
				return;
			}
		}

		public static PlatformDataMaster instance;

		public const int CRYPTOGRAPHY_MIN_SHIFTS = 8;

		public const int CRYPTOGRAPHY_MAX_SHIFTS = 16;

		public const bool EDITOR_LOG_DATA = true;

		private const int OP_MS_DELAY = 100;

		public const string LOAD_FAILED_DATA_VALUE = "";

		public const string VERSION_DATA_FILE_EXTENSION = ".json";

		public const string SETTINGS_DATA_FILE_EXTENSION = ".json";

		public const string ACHIEVEMENTS_DATA_FILE_EXTENSION = ".json";

		public const string GAME_DATA_FILE_EXTENSION = ".json";

		public const string GAME_DATA_NAME_DEMO = "GameDataDemo";

		public const string GAME_DATA_NAME_FULL = "GameDataFull";

		private static StringBuilder enumSB = new StringBuilder(1024);

		private static Stopwatch dataStopWatch_Save = new Stopwatch();

		private static Stopwatch dataStopWatch_Load = new Stopwatch();

		private static Stopwatch dataStopWatch_Delete = new Stopwatch();

		private static long operationIDCounter_Save = 0L;

		private static long operationIDCounter_Load = 0L;

		private static long operationIDCounter_Delete = 0L;

		private static List<long> bookedSaveOperations = new List<long>();

		private static List<long> bookedLoadOperations = new List<long>();

		private static List<long> bookedDeleteOperations = new List<long>();
	}
}
