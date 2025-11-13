using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace I2.Loc
{
	public static class LocalizationManager
	{
		// Token: 0x06000F11 RID: 3857 RVA: 0x00060E27 File Offset: 0x0005F027
		public static void InitializeIfNeeded()
		{
			if (string.IsNullOrEmpty(LocalizationManager.mCurrentLanguage) || LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.AutoLoadGlobalParamManagers();
				LocalizationManager.UpdateSources();
				LocalizationManager.SelectStartupLanguage();
			}
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00060E51 File Offset: 0x0005F051
		public static string GetVersion()
		{
			return "2.8.22 f6";
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00060E58 File Offset: 0x0005F058
		public static int GetRequiredWebServiceVersion()
		{
			return 5;
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00060E5C File Offset: 0x0005F05C
		public static string GetWebServiceURL(LanguageSourceData source = null)
		{
			if (source != null && !string.IsNullOrEmpty(source.Google_WebServiceURL))
			{
				return source.Google_WebServiceURL;
			}
			LocalizationManager.InitializeIfNeeded();
			for (int i = 0; i < LocalizationManager.Sources.Count; i++)
			{
				if (LocalizationManager.Sources[i] != null && !string.IsNullOrEmpty(LocalizationManager.Sources[i].Google_WebServiceURL))
				{
					return LocalizationManager.Sources[i].Google_WebServiceURL;
				}
			}
			return string.Empty;
		}

		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x00060ED4 File Offset: 0x0005F0D4
		// (set) Token: 0x06000F16 RID: 3862 RVA: 0x00060EE0 File Offset: 0x0005F0E0
		public static string CurrentLanguage
		{
			get
			{
				LocalizationManager.InitializeIfNeeded();
				return LocalizationManager.mCurrentLanguage;
			}
			set
			{
				LocalizationManager.InitializeIfNeeded();
				string supportedLanguage = LocalizationManager.GetSupportedLanguage(value, false);
				if (!string.IsNullOrEmpty(supportedLanguage) && LocalizationManager.mCurrentLanguage != supportedLanguage)
				{
					LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage), true, false);
				}
			}
		}

		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x00060F1D File Offset: 0x0005F11D
		// (set) Token: 0x06000F18 RID: 3864 RVA: 0x00060F2C File Offset: 0x0005F12C
		public static string CurrentLanguageCode
		{
			get
			{
				LocalizationManager.InitializeIfNeeded();
				return LocalizationManager.mLanguageCode;
			}
			set
			{
				LocalizationManager.InitializeIfNeeded();
				if (LocalizationManager.mLanguageCode != value)
				{
					string languageFromCode = LocalizationManager.GetLanguageFromCode(value, true);
					if (!string.IsNullOrEmpty(languageFromCode))
					{
						LocalizationManager.SetLanguageAndCode(languageFromCode, value, true, false);
					}
				}
			}
		}

		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x00060F64 File Offset: 0x0005F164
		// (set) Token: 0x06000F1A RID: 3866 RVA: 0x00060FD4 File Offset: 0x0005F1D4
		public static string CurrentRegion
		{
			get
			{
				string currentLanguage = LocalizationManager.CurrentLanguage;
				int num = currentLanguage.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
				{
					return currentLanguage.Substring(num + 1);
				}
				num = currentLanguage.IndexOfAny("[(".ToCharArray());
				int num2 = currentLanguage.LastIndexOfAny("])".ToCharArray());
				if (num > 0 && num != num2)
				{
					return currentLanguage.Substring(num + 1, num2 - num - 1);
				}
				return string.Empty;
			}
			set
			{
				string text = LocalizationManager.CurrentLanguage;
				int num = text.IndexOfAny("/\\".ToCharArray());
				if (num > 0)
				{
					LocalizationManager.CurrentLanguage = text.Substring(num + 1) + value;
					return;
				}
				num = text.IndexOfAny("[(".ToCharArray());
				int num2 = text.LastIndexOfAny("])".ToCharArray());
				if (num > 0 && num != num2)
				{
					text = text.Substring(num);
				}
				LocalizationManager.CurrentLanguage = text + "(" + value + ")";
			}
		}

		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x0006105C File Offset: 0x0005F25C
		// (set) Token: 0x06000F1C RID: 3868 RVA: 0x00061094 File Offset: 0x0005F294
		public static string CurrentRegionCode
		{
			get
			{
				string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
				int num = currentLanguageCode.IndexOfAny(" -_/\\".ToCharArray());
				if (num >= 0)
				{
					return currentLanguageCode.Substring(num + 1);
				}
				return string.Empty;
			}
			set
			{
				string text = LocalizationManager.CurrentLanguageCode;
				int num = text.IndexOfAny(" -_/\\".ToCharArray());
				if (num > 0)
				{
					text = text.Substring(0, num);
				}
				LocalizationManager.CurrentLanguageCode = text + "-" + value;
			}
		}

		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x000610D6 File Offset: 0x0005F2D6
		public static CultureInfo CurrentCulture
		{
			get
			{
				return LocalizationManager.mCurrentCulture;
			}
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x000610E0 File Offset: 0x0005F2E0
		public static void SetLanguageAndCode(string LanguageName, string LanguageCode, bool RememberLanguage = true, bool Force = false)
		{
			if (LocalizationManager.mCurrentLanguage != LanguageName || LocalizationManager.mLanguageCode != LanguageCode || Force)
			{
				if (RememberLanguage)
				{
					PersistentStorage.SetSetting_String("I2 Language", LanguageName);
				}
				LocalizationManager.mCurrentLanguage = LanguageName;
				LocalizationManager.mLanguageCode = LanguageCode;
				LocalizationManager.mCurrentCulture = LocalizationManager.CreateCultureForCode(LanguageCode);
				if (LocalizationManager.mChangeCultureInfo)
				{
					LocalizationManager.SetCurrentCultureInfo();
				}
				LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
				LocalizationManager.HasJoinedWords = GoogleLanguages.LanguageCode_HasJoinedWord(LocalizationManager.mLanguageCode);
				LocalizationManager.LocalizeAll(Force);
			}
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00061164 File Offset: 0x0005F364
		private static CultureInfo CreateCultureForCode(string code)
		{
			CultureInfo cultureInfo;
			try
			{
				cultureInfo = CultureInfo.CreateSpecificCulture(code);
			}
			catch (Exception)
			{
				cultureInfo = CultureInfo.InvariantCulture;
			}
			return cultureInfo;
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x00061194 File Offset: 0x0005F394
		public static void EnableChangingCultureInfo(bool bEnable)
		{
			if (!LocalizationManager.mChangeCultureInfo && bEnable)
			{
				LocalizationManager.SetCurrentCultureInfo();
			}
			LocalizationManager.mChangeCultureInfo = bEnable;
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x000611AD File Offset: 0x0005F3AD
		private static void SetCurrentCultureInfo()
		{
			Thread.CurrentThread.CurrentCulture = LocalizationManager.mCurrentCulture;
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x000611C0 File Offset: 0x0005F3C0
		private static void SelectStartupLanguage()
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				return;
			}
			string setting_String = PersistentStorage.GetSetting_String("I2 Language", string.Empty);
			string currentDeviceLanguage = LocalizationManager.GetCurrentDeviceLanguage(false);
			if (!string.IsNullOrEmpty(setting_String) && LocalizationManager.HasLanguage(setting_String, true, false, true))
			{
				LocalizationManager.SetLanguageAndCode(setting_String, LocalizationManager.GetLanguageCode(setting_String), true, false);
				return;
			}
			if (!LocalizationManager.Sources[0].IgnoreDeviceLanguage)
			{
				string supportedLanguage = LocalizationManager.GetSupportedLanguage(currentDeviceLanguage, true);
				if (!string.IsNullOrEmpty(supportedLanguage))
				{
					LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage), false, false);
					return;
				}
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].mLanguages.Count > 0)
				{
					for (int j = 0; j < LocalizationManager.Sources[i].mLanguages.Count; j++)
					{
						if (LocalizationManager.Sources[i].mLanguages[j].IsEnabled())
						{
							LocalizationManager.SetLanguageAndCode(LocalizationManager.Sources[i].mLanguages[j].Name, LocalizationManager.Sources[i].mLanguages[j].Code, false, false);
							return;
						}
					}
				}
				i++;
			}
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x00061300 File Offset: 0x0005F500
		public static bool HasLanguage(string Language, bool AllowDiscartingRegion = true, bool Initialize = true, bool SkipDisabled = true)
		{
			if (Initialize)
			{
				LocalizationManager.InitializeIfNeeded();
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].GetLanguageIndex(Language, false, SkipDisabled) >= 0)
				{
					return true;
				}
				i++;
			}
			if (AllowDiscartingRegion)
			{
				int j = 0;
				int count2 = LocalizationManager.Sources.Count;
				while (j < count2)
				{
					if (LocalizationManager.Sources[j].GetLanguageIndex(Language, true, SkipDisabled) >= 0)
					{
						return true;
					}
					j++;
				}
			}
			return false;
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00061378 File Offset: 0x0005F578
		public static string GetSupportedLanguage(string Language, bool ignoreDisabled = false)
		{
			string languageCode = GoogleLanguages.GetLanguageCode(Language, false);
			if (!string.IsNullOrEmpty(languageCode))
			{
				int i = 0;
				int count = LocalizationManager.Sources.Count;
				while (i < count)
				{
					int languageIndexFromCode = LocalizationManager.Sources[i].GetLanguageIndexFromCode(languageCode, true, ignoreDisabled);
					if (languageIndexFromCode >= 0)
					{
						return LocalizationManager.Sources[i].mLanguages[languageIndexFromCode].Name;
					}
					i++;
				}
				int j = 0;
				int count2 = LocalizationManager.Sources.Count;
				while (j < count2)
				{
					int languageIndexFromCode2 = LocalizationManager.Sources[j].GetLanguageIndexFromCode(languageCode, false, ignoreDisabled);
					if (languageIndexFromCode2 >= 0)
					{
						return LocalizationManager.Sources[j].mLanguages[languageIndexFromCode2].Name;
					}
					j++;
				}
			}
			int k = 0;
			int count3 = LocalizationManager.Sources.Count;
			while (k < count3)
			{
				int languageIndex = LocalizationManager.Sources[k].GetLanguageIndex(Language, false, ignoreDisabled);
				if (languageIndex >= 0)
				{
					return LocalizationManager.Sources[k].mLanguages[languageIndex].Name;
				}
				k++;
			}
			int l = 0;
			int count4 = LocalizationManager.Sources.Count;
			while (l < count4)
			{
				int languageIndex2 = LocalizationManager.Sources[l].GetLanguageIndex(Language, true, ignoreDisabled);
				if (languageIndex2 >= 0)
				{
					return LocalizationManager.Sources[l].mLanguages[languageIndex2].Name;
				}
				l++;
			}
			return string.Empty;
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x000614EC File Offset: 0x0005F6EC
		public static string GetLanguageCode(string Language)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(Language, true, true);
				if (languageIndex >= 0)
				{
					return LocalizationManager.Sources[i].mLanguages[languageIndex].Code;
				}
				i++;
			}
			return string.Empty;
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0006155C File Offset: 0x0005F75C
		public static string GetLanguageFromCode(string Code, bool exactMatch = true)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				int languageIndexFromCode = LocalizationManager.Sources[i].GetLanguageIndexFromCode(Code, exactMatch, false);
				if (languageIndexFromCode >= 0)
				{
					return LocalizationManager.Sources[i].mLanguages[languageIndexFromCode].Name;
				}
				i++;
			}
			return string.Empty;
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x000615CC File Offset: 0x0005F7CC
		public static List<string> GetAllLanguages(bool SkipDisabled = true)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			List<string> Languages = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			Func<string, bool> <>9__0;
			while (i < count)
			{
				List<string> languages = Languages;
				IEnumerable<string> languages2 = LocalizationManager.Sources[i].GetLanguages(SkipDisabled);
				Func<string, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (string x) => !Languages.Contains(x));
				}
				languages.AddRange(languages2.Where(func));
				i++;
			}
			return Languages;
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x0006165C File Offset: 0x0005F85C
		public static List<string> GetAllLanguagesCode(bool allowRegions = true, bool SkipDisabled = true)
		{
			List<string> Languages = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			Func<string, bool> <>9__0;
			while (i < count)
			{
				List<string> languages = Languages;
				IEnumerable<string> languagesCode = LocalizationManager.Sources[i].GetLanguagesCode(allowRegions, SkipDisabled);
				Func<string, bool> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (string x) => !Languages.Contains(x));
				}
				languages.AddRange(languagesCode.Where(func));
				i++;
			}
			return Languages;
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x000616D8 File Offset: 0x0005F8D8
		public static bool IsLanguageEnabled(string Language)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (!LocalizationManager.Sources[i].IsLanguageEnabled(Language))
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x06000F2A RID: 3882 RVA: 0x00061714 File Offset: 0x0005F914
		private static void LoadCurrentLanguage()
		{
			for (int i = 0; i < LocalizationManager.Sources.Count; i++)
			{
				int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(LocalizationManager.mCurrentLanguage, true, false);
				LocalizationManager.Sources[i].LoadLanguage(languageIndex, true, true, true, false);
			}
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00061763 File Offset: 0x0005F963
		public static void PreviewLanguage(string NewLanguage)
		{
			LocalizationManager.mCurrentLanguage = NewLanguage;
			LocalizationManager.mLanguageCode = LocalizationManager.GetLanguageCode(LocalizationManager.mCurrentLanguage);
			LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
			LocalizationManager.HasJoinedWords = GoogleLanguages.LanguageCode_HasJoinedWord(LocalizationManager.mLanguageCode);
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00061798 File Offset: 0x0005F998
		public static void AutoLoadGlobalParamManagers()
		{
			foreach (LocalizationParamsManager localizationParamsManager in global::UnityEngine.Object.FindObjectsOfType<LocalizationParamsManager>())
			{
				if (localizationParamsManager._IsGlobalManager && !LocalizationManager.ParamManagers.Contains(localizationParamsManager))
				{
					Debug.Log(localizationParamsManager);
					LocalizationManager.ParamManagers.Add(localizationParamsManager);
				}
			}
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x000617E3 File Offset: 0x0005F9E3
		public static void ApplyLocalizationParams(ref string translation, bool allowLocalizedParameters = true)
		{
			LocalizationManager.ApplyLocalizationParams(ref translation, (string p) => LocalizationManager.GetLocalizationParam(p, null), allowLocalizedParameters);
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x0006180C File Offset: 0x0005FA0C
		public static void ApplyLocalizationParams(ref string translation, GameObject root, bool allowLocalizedParameters = true)
		{
			LocalizationManager.ApplyLocalizationParams(ref translation, (string p) => LocalizationManager.GetLocalizationParam(p, root), allowLocalizedParameters);
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x0006183C File Offset: 0x0005FA3C
		public static void ApplyLocalizationParams(ref string translation, Dictionary<string, object> parameters, bool allowLocalizedParameters = true)
		{
			LocalizationManager.ApplyLocalizationParams(ref translation, delegate(string p)
			{
				object obj = null;
				if (parameters.TryGetValue(p, out obj))
				{
					return obj;
				}
				return null;
			}, allowLocalizedParameters);
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0006186C File Offset: 0x0005FA6C
		public static void ApplyLocalizationParams(ref string translation, LocalizationManager._GetParam getParam, bool allowLocalizedParameters = true)
		{
			if (translation == null)
			{
				return;
			}
			if (LocalizationManager.CustomApplyLocalizationParams != null && LocalizationManager.CustomApplyLocalizationParams(ref translation, getParam, allowLocalizedParameters))
			{
				return;
			}
			string text = null;
			int num = translation.Length;
			int num2 = 0;
			while (num2 >= 0 && num2 < translation.Length)
			{
				int num3 = translation.IndexOf("{[", num2, StringComparison.Ordinal);
				if (num3 < 0)
				{
					break;
				}
				int num4 = translation.IndexOf("]}", num3, StringComparison.Ordinal);
				if (num4 < 0)
				{
					break;
				}
				int num5 = translation.IndexOf("{[", num3 + 1, StringComparison.Ordinal);
				if (num5 > 0 && num5 < num4)
				{
					num2 = num5;
				}
				else
				{
					int num6 = ((translation[num3 + 2] == '#') ? 3 : 2);
					string text2 = translation.Substring(num3 + num6, num4 - num3 - num6);
					string text3 = (string)getParam(text2);
					if (text3 != null)
					{
						if (allowLocalizedParameters)
						{
							LanguageSourceData languageSourceData;
							TermData termData = LocalizationManager.GetTermData(text3, out languageSourceData);
							if (termData != null)
							{
								int languageIndex = languageSourceData.GetLanguageIndex(LocalizationManager.CurrentLanguage, true, true);
								if (languageIndex >= 0)
								{
									text3 = termData.GetTranslation(languageIndex, null, false);
								}
							}
						}
						string text4 = translation.Substring(num3, num4 - num3 + 2);
						translation = translation.Replace(text4, text3);
						int num7 = 0;
						if (int.TryParse(text3, out num7))
						{
							text = GoogleLanguages.GetPluralType(LocalizationManager.CurrentLanguageCode, num7).ToString();
						}
						num2 = num3 + text3.Length;
					}
					else
					{
						num2 = num4 + 2;
					}
				}
			}
			if (text != null)
			{
				string text5 = "[i2p_" + text + "]";
				int num8 = translation.IndexOf(text5, StringComparison.OrdinalIgnoreCase);
				if (num8 < 0)
				{
					num8 = 0;
				}
				else
				{
					num8 += text5.Length;
				}
				num = translation.IndexOf("[i2p_", num8 + 1, StringComparison.OrdinalIgnoreCase);
				if (num < 0)
				{
					num = translation.Length;
				}
				translation = translation.Substring(num8, num - num8);
			}
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00061A40 File Offset: 0x0005FC40
		internal static string GetLocalizationParam(string ParamName, GameObject root)
		{
			if (root)
			{
				MonoBehaviour[] components = root.GetComponents<MonoBehaviour>();
				int i = 0;
				int num = components.Length;
				while (i < num)
				{
					ILocalizationParamsManager localizationParamsManager = components[i] as ILocalizationParamsManager;
					if (localizationParamsManager != null && components[i].enabled)
					{
						string text = localizationParamsManager.GetParameterValue(ParamName);
						if (text != null)
						{
							return text;
						}
					}
					i++;
				}
			}
			int j = 0;
			int count = LocalizationManager.ParamManagers.Count;
			while (j < count)
			{
				string text = LocalizationManager.ParamManagers[j].GetParameterValue(ParamName);
				if (text != null)
				{
					return text;
				}
				j++;
			}
			return null;
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00061ACC File Offset: 0x0005FCCC
		private static string GetPluralType(MatchCollection matches, string langCode, LocalizationManager._GetParam getParam)
		{
			int i = 0;
			int count = matches.Count;
			while (i < count)
			{
				Match match = matches[i];
				string value = match.Groups[match.Groups.Count - 1].Value;
				string text = (string)getParam(value);
				if (text != null)
				{
					int num = 0;
					if (int.TryParse(text, out num))
					{
						return GoogleLanguages.GetPluralType(langCode, num).ToString();
					}
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x00061B4B File Offset: 0x0005FD4B
		public static string ApplyRTLfix(string line)
		{
			return LocalizationManager.ApplyRTLfix(line, 0, true);
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00061B58 File Offset: 0x0005FD58
		public static string ApplyRTLfix(string line, int maxCharacters, bool ignoreNumbers)
		{
			if (string.IsNullOrEmpty(line))
			{
				return line;
			}
			char c = line[0];
			if (c == '!' || c == '.' || c == '?')
			{
				line = line.Substring(1) + c.ToString();
			}
			int num = -1;
			int num2 = 0;
			int num3 = 65531;
			num2 = 0;
			List<string> list = new List<string>();
			while (I2Utils.FindNextTag(line, num2, out num, out num2))
			{
				char c2 = (char)(num3 - list.Count);
				list.Add(line.Substring(num, num2 - num + 1));
				line = line.Substring(0, num) + c2.ToString() + line.Substring(num2 + 1);
				num2 = num + 1;
			}
			line = line.Replace("\r\n", "\n");
			line = I2Utils.SplitLine(line, maxCharacters);
			line = RTLFixer.Fix(line, true, !ignoreNumbers);
			for (int i = 0; i < list.Count; i++)
			{
				string text = ((char)(num3 - i)).ToString();
				string text2 = I2Utils.ReverseText(list[i]);
				line = line.Replace(text, text2);
			}
			return line;
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x00061C6A File Offset: 0x0005FE6A
		public static string FixRTL_IfNeeded(string text, int maxCharacters = 0, bool ignoreNumber = false)
		{
			if (LocalizationManager.IsRight2Left)
			{
				return LocalizationManager.ApplyRTLfix(text, maxCharacters, ignoreNumber);
			}
			return text;
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x00061C7D File Offset: 0x0005FE7D
		public static bool IsRTL(string Code)
		{
			return Array.IndexOf<string>(LocalizationManager.LanguagesRTL, Code) >= 0;
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00061C90 File Offset: 0x0005FE90
		public static bool UpdateSources()
		{
			LocalizationManager.UnregisterDeletededSources();
			LocalizationManager.RegisterSourceInResources();
			LocalizationManager.RegisterSceneSources();
			return LocalizationManager.Sources.Count > 0;
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x00061CB0 File Offset: 0x0005FEB0
		private static void UnregisterDeletededSources()
		{
			for (int i = LocalizationManager.Sources.Count - 1; i >= 0; i--)
			{
				if (LocalizationManager.Sources[i] == null)
				{
					LocalizationManager.RemoveSource(LocalizationManager.Sources[i]);
				}
			}
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00061CF4 File Offset: 0x0005FEF4
		private static void RegisterSceneSources()
		{
			foreach (LanguageSource languageSource in (LanguageSource[])Resources.FindObjectsOfTypeAll(typeof(LanguageSource)))
			{
				if (!LocalizationManager.Sources.Contains(languageSource.mSource))
				{
					if (languageSource.mSource.owner == null)
					{
						languageSource.mSource.owner = languageSource;
					}
					LocalizationManager.AddSource(languageSource.mSource);
				}
			}
		}

		// Token: 0x06000F3A RID: 3898 RVA: 0x00061D60 File Offset: 0x0005FF60
		private static void RegisterSourceInResources()
		{
			foreach (string text in LocalizationManager.GlobalSources)
			{
				LanguageSourceAsset asset = ResourceManager.pInstance.GetAsset<LanguageSourceAsset>(text);
				if (asset && !LocalizationManager.Sources.Contains(asset.mSource))
				{
					if (!asset.mSource.mIsGlobalSource)
					{
						asset.mSource.mIsGlobalSource = true;
					}
					asset.mSource.owner = asset;
					LocalizationManager.AddSource(asset.mSource);
				}
			}
		}

		// Token: 0x06000F3B RID: 3899 RVA: 0x00061DDB File Offset: 0x0005FFDB
		private static bool AllowSyncFromGoogle(LanguageSourceData Source)
		{
			return LocalizationManager.Callback_AllowSyncFromGoogle == null || LocalizationManager.Callback_AllowSyncFromGoogle(Source);
		}

		// Token: 0x06000F3C RID: 3900 RVA: 0x00061DF4 File Offset: 0x0005FFF4
		internal static void AddSource(LanguageSourceData Source)
		{
			if (LocalizationManager.Sources.Contains(Source))
			{
				return;
			}
			LocalizationManager.Sources.Add(Source);
			if (Source.HasGoogleSpreadsheet() && Source.GoogleUpdateFrequency != LanguageSourceData.eGoogleUpdateFrequency.Never && LocalizationManager.AllowSyncFromGoogle(Source))
			{
				Source.Import_Google_FromCache();
				bool flag = false;
				if (Source.GoogleUpdateDelay > 0f)
				{
					CoroutineManager.Start(LocalizationManager.Delayed_Import_Google(Source, Source.GoogleUpdateDelay, flag));
				}
				else
				{
					Source.Import_Google(false, flag);
				}
			}
			for (int i = 0; i < Source.mLanguages.Count; i++)
			{
				Source.mLanguages[i].SetLoaded(true);
			}
			if (Source.mDictionary.Count == 0)
			{
				Source.UpdateDictionary(true);
			}
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x00061EA1 File Offset: 0x000600A1
		private static IEnumerator Delayed_Import_Google(LanguageSourceData source, float delay, bool justCheck)
		{
			yield return new WaitForSeconds(delay);
			if (source != null)
			{
				source.Import_Google(false, justCheck);
			}
			yield break;
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x00061EBE File Offset: 0x000600BE
		internal static void RemoveSource(LanguageSourceData Source)
		{
			LocalizationManager.Sources.Remove(Source);
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x00061ECC File Offset: 0x000600CC
		public static bool IsGlobalSource(string SourceName)
		{
			return Array.IndexOf<string>(LocalizationManager.GlobalSources, SourceName) >= 0;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x00061EE0 File Offset: 0x000600E0
		public static LanguageSourceData GetSourceContaining(string term, bool fallbackToFirst = true)
		{
			if (!string.IsNullOrEmpty(term))
			{
				int i = 0;
				int count = LocalizationManager.Sources.Count;
				while (i < count)
				{
					if (LocalizationManager.Sources[i].GetTermData(term, false) != null)
					{
						return LocalizationManager.Sources[i];
					}
					i++;
				}
			}
			if (!fallbackToFirst || LocalizationManager.Sources.Count <= 0)
			{
				return null;
			}
			return LocalizationManager.Sources[0];
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00061F4C File Offset: 0x0006014C
		public static global::UnityEngine.Object FindAsset(string value)
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				global::UnityEngine.Object @object = LocalizationManager.Sources[i].FindAsset(value);
				if (@object)
				{
					return @object;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x00061F90 File Offset: 0x00060190
		public static void ApplyDownloadedDataFromGoogle()
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				LocalizationManager.Sources[i].ApplyDownloadedDataFromGoogle();
				i++;
			}
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x00061FC4 File Offset: 0x000601C4
		public static string GetCurrentDeviceLanguage(bool force = false)
		{
			if (force || string.IsNullOrEmpty(LocalizationManager.mCurrentDeviceLanguage))
			{
				LocalizationManager.DetectDeviceLanguage();
			}
			return LocalizationManager.mCurrentDeviceLanguage;
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x00061FE0 File Offset: 0x000601E0
		private static void DetectDeviceLanguage()
		{
			LocalizationManager.mCurrentDeviceLanguage = Application.systemLanguage.ToString();
			if (LocalizationManager.mCurrentDeviceLanguage == "ChineseSimplified")
			{
				LocalizationManager.mCurrentDeviceLanguage = "Chinese (Simplified)";
			}
			if (LocalizationManager.mCurrentDeviceLanguage == "ChineseTraditional")
			{
				LocalizationManager.mCurrentDeviceLanguage = "Chinese (Traditional)";
			}
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0006203C File Offset: 0x0006023C
		public static void RegisterTarget(ILocalizeTargetDescriptor desc)
		{
			if (LocalizationManager.mLocalizeTargets.FindIndex((ILocalizeTargetDescriptor x) => x.Name == desc.Name) != -1)
			{
				return;
			}
			for (int i = 0; i < LocalizationManager.mLocalizeTargets.Count; i++)
			{
				if (LocalizationManager.mLocalizeTargets[i].Priority > desc.Priority)
				{
					LocalizationManager.mLocalizeTargets.Insert(i, desc);
					return;
				}
			}
			LocalizationManager.mLocalizeTargets.Add(desc);
		}

		// (add) Token: 0x06000F46 RID: 3910 RVA: 0x000620C4 File Offset: 0x000602C4
		// (remove) Token: 0x06000F47 RID: 3911 RVA: 0x000620F8 File Offset: 0x000602F8
		public static event LocalizationManager.OnLocalizeCallback OnLocalizeEvent;

		// Token: 0x06000F48 RID: 3912 RVA: 0x0006212C File Offset: 0x0006032C
		public static string GetTranslation(string Term, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null, bool allowLocalizedParameters = true)
		{
			string text = null;
			LocalizationManager.TryGetTranslation(Term, out text, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage, allowLocalizedParameters);
			return text;
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x00062150 File Offset: 0x00060350
		public static string GetTermTranslation(string Term, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null, bool allowLocalizedParameters = true)
		{
			return LocalizationManager.GetTranslation(Term, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage, allowLocalizedParameters);
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x00062164 File Offset: 0x00060364
		public static bool TryGetTranslation(string Term, out string Translation, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null, bool allowLocalizedParameters = true)
		{
			Translation = null;
			if (string.IsNullOrEmpty(Term))
			{
				return false;
			}
			LocalizationManager.InitializeIfNeeded();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				if (LocalizationManager.Sources[i].TryGetTranslation(Term, out Translation, overrideLanguage, null, false, false))
				{
					if (applyParameters)
					{
						LocalizationManager.ApplyLocalizationParams(ref Translation, localParametersRoot, allowLocalizedParameters);
					}
					if (LocalizationManager.IsRight2Left && FixForRTL)
					{
						Translation = LocalizationManager.ApplyRTLfix(Translation, maxLineLengthForRTL, ignoreRTLnumbers);
					}
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x000621DC File Offset: 0x000603DC
		public static T GetTranslatedObject<T>(string AssetName, Localize optionalLocComp = null) where T : global::UnityEngine.Object
		{
			if (optionalLocComp != null)
			{
				return optionalLocComp.FindTranslatedObject<T>(AssetName);
			}
			T t = LocalizationManager.FindAsset(AssetName) as T;
			if (t)
			{
				return t;
			}
			return ResourceManager.pInstance.GetAsset<T>(AssetName);
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x00062227 File Offset: 0x00060427
		public static T GetTranslatedObjectByTermName<T>(string Term, Localize optionalLocComp = null) where T : global::UnityEngine.Object
		{
			return LocalizationManager.GetTranslatedObject<T>(LocalizationManager.GetTranslation(Term, false, 0, true, false, null, null, true), null);
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0006223C File Offset: 0x0006043C
		public static string GetAppName(string languageCode)
		{
			if (!string.IsNullOrEmpty(languageCode))
			{
				for (int i = 0; i < LocalizationManager.Sources.Count; i++)
				{
					if (!string.IsNullOrEmpty(LocalizationManager.Sources[i].mTerm_AppName))
					{
						int languageIndexFromCode = LocalizationManager.Sources[i].GetLanguageIndexFromCode(languageCode, false, false);
						if (languageIndexFromCode >= 0)
						{
							TermData termData = LocalizationManager.Sources[i].GetTermData(LocalizationManager.Sources[i].mTerm_AppName, false);
							if (termData != null)
							{
								string translation = termData.GetTranslation(languageIndexFromCode, null, false);
								if (!string.IsNullOrEmpty(translation))
								{
									return translation;
								}
							}
						}
					}
				}
			}
			return Application.productName;
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x000622D3 File Offset: 0x000604D3
		public static void LocalizeAll(bool Force = false)
		{
			LocalizationManager.LoadCurrentLanguage();
			if (!Application.isPlaying)
			{
				LocalizationManager.DoLocalizeAll(Force);
				return;
			}
			LocalizationManager.mLocalizeIsScheduledWithForcedValue = LocalizationManager.mLocalizeIsScheduledWithForcedValue || Force;
			if (LocalizationManager.mLocalizeIsScheduled)
			{
				return;
			}
			CoroutineManager.Start(LocalizationManager.Coroutine_LocalizeAll());
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x00062307 File Offset: 0x00060507
		private static IEnumerator Coroutine_LocalizeAll()
		{
			LocalizationManager.mLocalizeIsScheduled = true;
			yield return null;
			LocalizationManager.mLocalizeIsScheduled = false;
			bool flag = LocalizationManager.mLocalizeIsScheduledWithForcedValue;
			LocalizationManager.mLocalizeIsScheduledWithForcedValue = false;
			LocalizationManager.DoLocalizeAll(flag);
			yield break;
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x00062310 File Offset: 0x00060510
		private static void DoLocalizeAll(bool Force = false)
		{
			Localize[] array = (Localize[])Resources.FindObjectsOfTypeAll(typeof(Localize));
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				array[i].OnLocalize(Force);
				i++;
			}
			if (LocalizationManager.OnLocalizeEvent != null)
			{
				LocalizationManager.OnLocalizeEvent();
			}
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0006235C File Offset: 0x0006055C
		public static List<string> GetCategories()
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				LocalizationManager.Sources[i].GetCategories(false, list);
				i++;
			}
			return list;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0006239C File Offset: 0x0006059C
		public static List<string> GetTermsList(string Category = null)
		{
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			if (LocalizationManager.Sources.Count == 1)
			{
				return LocalizationManager.Sources[0].GetTermsList(Category);
			}
			HashSet<string> hashSet = new HashSet<string>();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				hashSet.UnionWith(LocalizationManager.Sources[i].GetTermsList(Category));
				i++;
			}
			return new List<string>(hashSet);
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00062414 File Offset: 0x00060614
		public static TermData GetTermData(string term)
		{
			LocalizationManager.InitializeIfNeeded();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				TermData termData = LocalizationManager.Sources[i].GetTermData(term, false);
				if (termData != null)
				{
					return termData;
				}
				i++;
			}
			return null;
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x00062458 File Offset: 0x00060658
		public static TermData GetTermData(string term, out LanguageSourceData source)
		{
			LocalizationManager.InitializeIfNeeded();
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				TermData termData = LocalizationManager.Sources[i].GetTermData(term, false);
				if (termData != null)
				{
					source = LocalizationManager.Sources[i];
					return termData;
				}
				i++;
			}
			source = null;
			return null;
		}

		private static string mCurrentLanguage;

		private static string mLanguageCode;

		private static CultureInfo mCurrentCulture;

		private static bool mChangeCultureInfo;

		public static bool IsRight2Left;

		public static bool HasJoinedWords;

		public static List<ILocalizationParamsManager> ParamManagers = new List<ILocalizationParamsManager>();

		public static LocalizationManager.FnCustomApplyLocalizationParams CustomApplyLocalizationParams;

		private static string[] LanguagesRTL = new string[]
		{
			"ar-DZ", "ar", "ar-BH", "ar-EG", "ar-IQ", "ar-JO", "ar-KW", "ar-LB", "ar-LY", "ar-MA",
			"ar-OM", "ar-QA", "ar-SA", "ar-SY", "ar-TN", "ar-AE", "ar-YE", "fa", "he", "ur",
			"ji"
		};

		public static List<LanguageSourceData> Sources = new List<LanguageSourceData>();

		public static string[] GlobalSources = new string[] { "I2Languages" };

		public static Func<LanguageSourceData, bool> Callback_AllowSyncFromGoogle = null;

		private static string mCurrentDeviceLanguage;

		public static List<ILocalizeTargetDescriptor> mLocalizeTargets = new List<ILocalizeTargetDescriptor>();

		private static bool mLocalizeIsScheduled;

		private static bool mLocalizeIsScheduledWithForcedValue;

		public static bool HighlightLocalizedTargets = false;

		// (Invoke) Token: 0x06001486 RID: 5254
		public delegate bool FnCustomApplyLocalizationParams(ref string translation, LocalizationManager._GetParam getParam, bool allowLocalizedParameters);

		// (Invoke) Token: 0x0600148A RID: 5258
		public delegate object _GetParam(string param);

		// (Invoke) Token: 0x0600148E RID: 5262
		public delegate void OnLocalizeCallback();
	}
}
