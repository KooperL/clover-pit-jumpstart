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
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x0005330F File Offset: 0x0005150F
		public static double SaveCooldownTimeGet
		{
			get
			{
				return 0.0;
			}
		}

		// (get) Token: 0x06000CEC RID: 3308 RVA: 0x0005331A File Offset: 0x0005151A
		public static double LoadCooldownTimeGet
		{
			get
			{
				return 0.0;
			}
		}

		// (get) Token: 0x06000CED RID: 3309 RVA: 0x00053325 File Offset: 0x00051525
		public static double DeleteCooldownTimeGet
		{
			get
			{
				return 0.0;
			}
		}

		// (get) Token: 0x06000CEE RID: 3310 RVA: 0x00053330 File Offset: 0x00051530
		public static string ExecutablePath
		{
			get
			{
				return Application.dataPath + "/../";
			}
		}

		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x00053341 File Offset: 0x00051541
		public static string OutsideExecutablePath
		{
			get
			{
				return Application.dataPath + "/../../";
			}
		}

		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00053352 File Offset: 0x00051552
		public static string Mount
		{
			get
			{
				return PlatformDataMaster.ExecutablePath;
			}
		}

		// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x00053359 File Offset: 0x00051559
		public static string CommonPath
		{
			get
			{
				return PlatformDataMaster.Mount + "/SaveData/";
			}
		}

		// (get) Token: 0x06000CF2 RID: 3314 RVA: 0x0005336A File Offset: 0x0005156A
		public static string VersionFolderPath
		{
			get
			{
				return PlatformDataMaster.CommonPath + "/_VD/";
			}
		}

		// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x0005337B File Offset: 0x0005157B
		public static string SettingsFolderPath
		{
			get
			{
				return PlatformDataMaster.CommonPath + "/SettingsData/";
			}
		}

		// (get) Token: 0x06000CF4 RID: 3316 RVA: 0x0005338C File Offset: 0x0005158C
		public static string AchievementsFolderPath
		{
			get
			{
				return PlatformDataMaster.GameFolderPath + "/_TD/";
			}
		}

		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x0005339D File Offset: 0x0005159D
		public static string GameFolderPath
		{
			get
			{
				return PlatformDataMaster.CommonPath + "/GameData/";
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x000533AE File Offset: 0x000515AE
		public static string PathGet_VersionFile()
		{
			return PlatformDataMaster.VersionFolderPath + "VersionDataFull.json";
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x000533BF File Offset: 0x000515BF
		public static string PathGet_SettingsFile()
		{
			return PlatformDataMaster.SettingsFolderPath + "SettingsDataFull.json";
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x000533D0 File Offset: 0x000515D0
		public static string PathGet_AchievementsFile()
		{
			return PlatformDataMaster.AchievementsFolderPath + "ADFv2.json";
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x000533E1 File Offset: 0x000515E1
		public static string PathGet_GameDataFile(int gameDataIndex, string extraAppendix = "")
		{
			return PlatformDataMaster.GameFolderPath + "GameDataFull" + extraAppendix + ".json";
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x000533F8 File Offset: 0x000515F8
		public static string ToJson<T>(T obj)
		{
			return JsonUtility.ToJson(obj, true);
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x00053408 File Offset: 0x00051608
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

		// Token: 0x06000CFC RID: 3324 RVA: 0x00053448 File Offset: 0x00051648
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

		// Token: 0x06000CFD RID: 3325 RVA: 0x000534A0 File Offset: 0x000516A0
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

		// Token: 0x06000CFE RID: 3326 RVA: 0x00053504 File Offset: 0x00051704
		public static T[] EnumArrayFromString<T>(string enumArrayString, char separator = ',')
		{
			List<T> list = PlatformDataMaster.EnumListFromString<T>(enumArrayString, separator);
			if (list == null)
			{
				return null;
			}
			return list.ToArray();
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x00053524 File Offset: 0x00051724
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

		// Token: 0x06000D00 RID: 3328 RVA: 0x00053590 File Offset: 0x00051790
		public static List<T> EnumListFromString<T>(string enumListString, char separator = ',')
		{
			List<T> list = new List<T>();
			PlatformDataMaster.EnumListFromString<T>(enumListString, ref list, false, separator);
			return list;
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x000535B0 File Offset: 0x000517B0
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

		// Token: 0x06000D02 RID: 3330 RVA: 0x00053634 File Offset: 0x00051834
		public static string EnumEntryToString<T>(T enumEntry)
		{
			return enumEntry.ToString();
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x00053644 File Offset: 0x00051844
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

		// Token: 0x06000D04 RID: 3332 RVA: 0x00053680 File Offset: 0x00051880
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

		// Token: 0x06000D05 RID: 3333 RVA: 0x000536C8 File Offset: 0x000518C8
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

		// Token: 0x06000D06 RID: 3334 RVA: 0x000537CC File Offset: 0x000519CC
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

		// Token: 0x06000D07 RID: 3335 RVA: 0x000538D4 File Offset: 0x00051AD4
		private static long OperationRecord_Save()
		{
			PlatformDataMaster.operationIDCounter_Save += 1L;
			long num = PlatformDataMaster.operationIDCounter_Save;
			PlatformDataMaster.bookedSaveOperations.Add(num);
			return num;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00053900 File Offset: 0x00051B00
		private static bool OperationCanGo_Save(long operationID)
		{
			return PlatformDataMaster.bookedSaveOperations.Count > 0 && PlatformDataMaster.bookedSaveOperations[0] == operationID;
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x0005391F File Offset: 0x00051B1F
		private static void OperationDoneSet_Save(long operationID)
		{
			PlatformDataMaster.bookedSaveOperations.Remove(operationID);
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00053930 File Offset: 0x00051B30
		private static long OperationRecord_Load()
		{
			PlatformDataMaster.operationIDCounter_Load += 1L;
			long num = PlatformDataMaster.operationIDCounter_Load;
			PlatformDataMaster.bookedLoadOperations.Add(num);
			return num;
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x0005395C File Offset: 0x00051B5C
		private static bool OperationCanGo_Load(long operationID)
		{
			return PlatformDataMaster.bookedLoadOperations.Count > 0 && PlatformDataMaster.bookedLoadOperations[0] == operationID;
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x0005397B File Offset: 0x00051B7B
		private static void OperationDoneSet_Load(long operationID)
		{
			PlatformDataMaster.bookedLoadOperations.Remove(operationID);
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x0005398C File Offset: 0x00051B8C
		private static long OperationRecord_Delete()
		{
			PlatformDataMaster.operationIDCounter_Delete += 1L;
			long num = PlatformDataMaster.operationIDCounter_Delete;
			PlatformDataMaster.bookedDeleteOperations.Add(num);
			return num;
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x000539B8 File Offset: 0x00051BB8
		private static bool OperationCanGo_Delete(long operationID)
		{
			return PlatformDataMaster.bookedDeleteOperations.Count > 0 && PlatformDataMaster.bookedDeleteOperations[0] == operationID;
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x000539D7 File Offset: 0x00051BD7
		private static void OperationDoneSet_Delete(long operationID)
		{
			PlatformDataMaster.bookedDeleteOperations.Remove(operationID);
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x000539E5 File Offset: 0x00051BE5
		public static bool IsSaving()
		{
			return PlatformDataMaster.bookedSaveOperations.Count > 0;
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x000539F4 File Offset: 0x00051BF4
		public static bool IsLoading()
		{
			return PlatformDataMaster.bookedLoadOperations.Count > 0;
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00053A03 File Offset: 0x00051C03
		public static bool IsDeleting()
		{
			return PlatformDataMaster.bookedDeleteOperations.Count > 0;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00053A12 File Offset: 0x00051C12
		public static bool IsSavingOrLoadingOrDeleting()
		{
			return PlatformDataMaster.IsSaving() || PlatformDataMaster.IsLoading() || PlatformDataMaster.IsDeleting();
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00053A2C File Offset: 0x00051C2C
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

		// Token: 0x06000D15 RID: 3349 RVA: 0x00053A64 File Offset: 0x00051C64
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

		// Token: 0x06000D16 RID: 3350 RVA: 0x00053AB0 File Offset: 0x00051CB0
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

		// Token: 0x06000D17 RID: 3351 RVA: 0x00053AFC File Offset: 0x00051CFC
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

		// Token: 0x06000D18 RID: 3352 RVA: 0x00053B48 File Offset: 0x00051D48
		private static UniTask<bool> _Save_NintendoSwitch(string path, string data)
		{
			PlatformDataMaster.<_Save_NintendoSwitch>d__78 <_Save_NintendoSwitch>d__;
			<_Save_NintendoSwitch>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<_Save_NintendoSwitch>d__.<>1__state = -1;
			<_Save_NintendoSwitch>d__.<>t__builder.Start<PlatformDataMaster.<_Save_NintendoSwitch>d__78>(ref <_Save_NintendoSwitch>d__);
			return <_Save_NintendoSwitch>d__.<>t__builder.Task;
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x00053B84 File Offset: 0x00051D84
		public static UniTask<string> Load(string operationTag, string path)
		{
			PlatformDataMaster.<Load>d__79 <Load>d__;
			<Load>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<Load>d__.path = path;
			<Load>d__.<>1__state = -1;
			<Load>d__.<>t__builder.Start<PlatformDataMaster.<Load>d__79>(ref <Load>d__);
			return <Load>d__.<>t__builder.Task;
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00053BC8 File Offset: 0x00051DC8
		private static UniTask<string> _Load_Desktop(string path)
		{
			PlatformDataMaster.<_Load_Desktop>d__80 <_Load_Desktop>d__;
			<_Load_Desktop>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<_Load_Desktop>d__.path = path;
			<_Load_Desktop>d__.<>1__state = -1;
			<_Load_Desktop>d__.<>t__builder.Start<PlatformDataMaster.<_Load_Desktop>d__80>(ref <_Load_Desktop>d__);
			return <_Load_Desktop>d__.<>t__builder.Task;
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00053C0C File Offset: 0x00051E0C
		private static UniTask<string> _Load_WebGl(string path)
		{
			PlatformDataMaster.<_Load_WebGl>d__81 <_Load_WebGl>d__;
			<_Load_WebGl>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<_Load_WebGl>d__.path = path;
			<_Load_WebGl>d__.<>1__state = -1;
			<_Load_WebGl>d__.<>t__builder.Start<PlatformDataMaster.<_Load_WebGl>d__81>(ref <_Load_WebGl>d__);
			return <_Load_WebGl>d__.<>t__builder.Task;
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x00053C50 File Offset: 0x00051E50
		private static UniTask<string> _Load_NintendoSwitch(string path)
		{
			PlatformDataMaster.<_Load_NintendoSwitch>d__82 <_Load_NintendoSwitch>d__;
			<_Load_NintendoSwitch>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<_Load_NintendoSwitch>d__.<>1__state = -1;
			<_Load_NintendoSwitch>d__.<>t__builder.Start<PlatformDataMaster.<_Load_NintendoSwitch>d__82>(ref <_Load_NintendoSwitch>d__);
			return <_Load_NintendoSwitch>d__.<>t__builder.Task;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00053C8C File Offset: 0x00051E8C
		public static UniTask<bool> Delete(string path)
		{
			PlatformDataMaster.<Delete>d__83 <Delete>d__;
			<Delete>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<Delete>d__.path = path;
			<Delete>d__.<>1__state = -1;
			<Delete>d__.<>t__builder.Start<PlatformDataMaster.<Delete>d__83>(ref <Delete>d__);
			return <Delete>d__.<>t__builder.Task;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00053CD0 File Offset: 0x00051ED0
		private static UniTask<bool> _Delete_Desktop(string path)
		{
			PlatformDataMaster.<_Delete_Desktop>d__84 <_Delete_Desktop>d__;
			<_Delete_Desktop>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<_Delete_Desktop>d__.path = path;
			<_Delete_Desktop>d__.<>1__state = -1;
			<_Delete_Desktop>d__.<>t__builder.Start<PlatformDataMaster.<_Delete_Desktop>d__84>(ref <_Delete_Desktop>d__);
			return <_Delete_Desktop>d__.<>t__builder.Task;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x00053D14 File Offset: 0x00051F14
		private static UniTask<bool> _Delete_WebGl(string path)
		{
			PlatformDataMaster.<_Delete_WebGl>d__85 <_Delete_WebGl>d__;
			<_Delete_WebGl>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<_Delete_WebGl>d__.path = path;
			<_Delete_WebGl>d__.<>1__state = -1;
			<_Delete_WebGl>d__.<>t__builder.Start<PlatformDataMaster.<_Delete_WebGl>d__85>(ref <_Delete_WebGl>d__);
			return <_Delete_WebGl>d__.<>t__builder.Task;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x00053D58 File Offset: 0x00051F58
		private static UniTask<bool> _Delete_NintendoSwitch(string path)
		{
			PlatformDataMaster.<_Delete_NintendoSwitch>d__86 <_Delete_NintendoSwitch>d__;
			<_Delete_NintendoSwitch>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<_Delete_NintendoSwitch>d__.<>1__state = -1;
			<_Delete_NintendoSwitch>d__.<>t__builder.Start<PlatformDataMaster.<_Delete_NintendoSwitch>d__86>(ref <_Delete_NintendoSwitch>d__);
			return <_Delete_NintendoSwitch>d__.<>t__builder.Task;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00053D94 File Offset: 0x00051F94
		public static UniTask<bool> SaveVersionData(string data)
		{
			PlatformDataMaster.<SaveVersionData>d__87 <SaveVersionData>d__;
			<SaveVersionData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<SaveVersionData>d__.data = data;
			<SaveVersionData>d__.<>1__state = -1;
			<SaveVersionData>d__.<>t__builder.Start<PlatformDataMaster.<SaveVersionData>d__87>(ref <SaveVersionData>d__);
			return <SaveVersionData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00053DD8 File Offset: 0x00051FD8
		public static UniTask<string> LoadVersionData()
		{
			PlatformDataMaster.<LoadVersionData>d__88 <LoadVersionData>d__;
			<LoadVersionData>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<LoadVersionData>d__.<>1__state = -1;
			<LoadVersionData>d__.<>t__builder.Start<PlatformDataMaster.<LoadVersionData>d__88>(ref <LoadVersionData>d__);
			return <LoadVersionData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00053E14 File Offset: 0x00052014
		public static UniTask<bool> DeleteVersionData()
		{
			PlatformDataMaster.<DeleteVersionData>d__89 <DeleteVersionData>d__;
			<DeleteVersionData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<DeleteVersionData>d__.<>1__state = -1;
			<DeleteVersionData>d__.<>t__builder.Start<PlatformDataMaster.<DeleteVersionData>d__89>(ref <DeleteVersionData>d__);
			return <DeleteVersionData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00053E50 File Offset: 0x00052050
		public static UniTask<bool> SaveSettingsData(string data)
		{
			PlatformDataMaster.<SaveSettingsData>d__90 <SaveSettingsData>d__;
			<SaveSettingsData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<SaveSettingsData>d__.data = data;
			<SaveSettingsData>d__.<>1__state = -1;
			<SaveSettingsData>d__.<>t__builder.Start<PlatformDataMaster.<SaveSettingsData>d__90>(ref <SaveSettingsData>d__);
			return <SaveSettingsData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00053E94 File Offset: 0x00052094
		public static UniTask<string> LoadSettingsData()
		{
			PlatformDataMaster.<LoadSettingsData>d__91 <LoadSettingsData>d__;
			<LoadSettingsData>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<LoadSettingsData>d__.<>1__state = -1;
			<LoadSettingsData>d__.<>t__builder.Start<PlatformDataMaster.<LoadSettingsData>d__91>(ref <LoadSettingsData>d__);
			return <LoadSettingsData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x00053ED0 File Offset: 0x000520D0
		public static UniTask<bool> DeleteSettingsData()
		{
			PlatformDataMaster.<DeleteSettingsData>d__92 <DeleteSettingsData>d__;
			<DeleteSettingsData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<DeleteSettingsData>d__.<>1__state = -1;
			<DeleteSettingsData>d__.<>t__builder.Start<PlatformDataMaster.<DeleteSettingsData>d__92>(ref <DeleteSettingsData>d__);
			return <DeleteSettingsData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00053F0C File Offset: 0x0005210C
		public static UniTask<bool> SaveAchievementsData(string data)
		{
			PlatformDataMaster.<SaveAchievementsData>d__93 <SaveAchievementsData>d__;
			<SaveAchievementsData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<SaveAchievementsData>d__.data = data;
			<SaveAchievementsData>d__.<>1__state = -1;
			<SaveAchievementsData>d__.<>t__builder.Start<PlatformDataMaster.<SaveAchievementsData>d__93>(ref <SaveAchievementsData>d__);
			return <SaveAchievementsData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00053F50 File Offset: 0x00052150
		public static UniTask<string> LoadAchievementsData()
		{
			PlatformDataMaster.<LoadAchievementsData>d__94 <LoadAchievementsData>d__;
			<LoadAchievementsData>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<LoadAchievementsData>d__.<>1__state = -1;
			<LoadAchievementsData>d__.<>t__builder.Start<PlatformDataMaster.<LoadAchievementsData>d__94>(ref <LoadAchievementsData>d__);
			return <LoadAchievementsData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00053F8C File Offset: 0x0005218C
		public static UniTask<bool> DeleteAchievementsData()
		{
			PlatformDataMaster.<DeleteAchievementsData>d__95 <DeleteAchievementsData>d__;
			<DeleteAchievementsData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<DeleteAchievementsData>d__.<>1__state = -1;
			<DeleteAchievementsData>d__.<>t__builder.Start<PlatformDataMaster.<DeleteAchievementsData>d__95>(ref <DeleteAchievementsData>d__);
			return <DeleteAchievementsData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00053FC8 File Offset: 0x000521C8
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

		// Token: 0x06000D2B RID: 3371 RVA: 0x00054014 File Offset: 0x00052214
		public static UniTask<string> LoadGameData(int gameDataIndex)
		{
			PlatformDataMaster.<LoadGameData>d__97 <LoadGameData>d__;
			<LoadGameData>d__.<>t__builder = AsyncUniTaskMethodBuilder<string>.Create();
			<LoadGameData>d__.gameDataIndex = gameDataIndex;
			<LoadGameData>d__.<>1__state = -1;
			<LoadGameData>d__.<>t__builder.Start<PlatformDataMaster.<LoadGameData>d__97>(ref <LoadGameData>d__);
			return <LoadGameData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00054058 File Offset: 0x00052258
		public static UniTask<bool> DeleteGameData(int gameDataIndex)
		{
			PlatformDataMaster.<DeleteGameData>d__98 <DeleteGameData>d__;
			<DeleteGameData>d__.<>t__builder = AsyncUniTaskMethodBuilder<bool>.Create();
			<DeleteGameData>d__.gameDataIndex = gameDataIndex;
			<DeleteGameData>d__.<>1__state = -1;
			<DeleteGameData>d__.<>t__builder.Start<PlatformDataMaster.<DeleteGameData>d__98>(ref <DeleteGameData>d__);
			return <DeleteGameData>d__.<>t__builder.Task;
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x0005409B File Offset: 0x0005229B
		private void Awake()
		{
			if (PlatformDataMaster.instance != null)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			PlatformDataMaster.instance = this;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x000540BC File Offset: 0x000522BC
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
