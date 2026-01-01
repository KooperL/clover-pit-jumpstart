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
	// Token: 0x020001B6 RID: 438
	public static class LocalizationManager
	{
		// Token: 0x060012DA RID: 4826 RVA: 0x00014FDC File Offset: 0x000131DC
		public static void InitializeIfNeeded()
		{
			if (string.IsNullOrEmpty(LocalizationManager.mCurrentLanguage) || LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.AutoLoadGlobalParamManagers();
				LocalizationManager.UpdateSources();
				LocalizationManager.SelectStartupLanguage();
			}
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x00015006 File Offset: 0x00013206
		public static string GetVersion()
		{
			return "2.8.22 f6";
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x0000D17C File Offset: 0x0000B37C
		public static int GetRequiredWebServiceVersion()
		{
			return 5;
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x0007F1D4 File Offset: 0x0007D3D4
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

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x0001500D File Offset: 0x0001320D
		// (set) Token: 0x060012DF RID: 4831 RVA: 0x0007F24C File Offset: 0x0007D44C
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

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060012E0 RID: 4832 RVA: 0x00015019 File Offset: 0x00013219
		// (set) Token: 0x060012E1 RID: 4833 RVA: 0x0007F28C File Offset: 0x0007D48C
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

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060012E2 RID: 4834 RVA: 0x0007F2C4 File Offset: 0x0007D4C4
		// (set) Token: 0x060012E3 RID: 4835 RVA: 0x0007F334 File Offset: 0x0007D534
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

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060012E4 RID: 4836 RVA: 0x0007F3BC File Offset: 0x0007D5BC
		// (set) Token: 0x060012E5 RID: 4837 RVA: 0x0007F3F4 File Offset: 0x0007D5F4
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

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x00015025 File Offset: 0x00013225
		public static CultureInfo CurrentCulture
		{
			get
			{
				return LocalizationManager.mCurrentCulture;
			}
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x0007F438 File Offset: 0x0007D638
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

		// Token: 0x060012E8 RID: 4840 RVA: 0x0007F4BC File Offset: 0x0007D6BC
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

		// Token: 0x060012E9 RID: 4841 RVA: 0x0001502C File Offset: 0x0001322C
		public static void EnableChangingCultureInfo(bool bEnable)
		{
			if (!LocalizationManager.mChangeCultureInfo && bEnable)
			{
				LocalizationManager.SetCurrentCultureInfo();
			}
			LocalizationManager.mChangeCultureInfo = bEnable;
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x00015045 File Offset: 0x00013245
		private static void SetCurrentCultureInfo()
		{
			Thread.CurrentThread.CurrentCulture = LocalizationManager.mCurrentCulture;
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0007F4EC File Offset: 0x0007D6EC
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

		// Token: 0x060012EC RID: 4844 RVA: 0x0007F62C File Offset: 0x0007D82C
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

		// Token: 0x060012ED RID: 4845 RVA: 0x0007F6A4 File Offset: 0x0007D8A4
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

		// Token: 0x060012EE RID: 4846 RVA: 0x0007F818 File Offset: 0x0007DA18
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

		// Token: 0x060012EF RID: 4847 RVA: 0x0007F888 File Offset: 0x0007DA88
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

		// Token: 0x060012F0 RID: 4848 RVA: 0x0007F8F8 File Offset: 0x0007DAF8
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

		// Token: 0x060012F1 RID: 4849 RVA: 0x0007F988 File Offset: 0x0007DB88
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

		// Token: 0x060012F2 RID: 4850 RVA: 0x0007FA04 File Offset: 0x0007DC04
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

		// Token: 0x060012F3 RID: 4851 RVA: 0x0007FA40 File Offset: 0x0007DC40
		private static void LoadCurrentLanguage()
		{
			for (int i = 0; i < LocalizationManager.Sources.Count; i++)
			{
				int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(LocalizationManager.mCurrentLanguage, true, false);
				LocalizationManager.Sources[i].LoadLanguage(languageIndex, true, true, true, false);
			}
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x00015056 File Offset: 0x00013256
		public static void PreviewLanguage(string NewLanguage)
		{
			LocalizationManager.mCurrentLanguage = NewLanguage;
			LocalizationManager.mLanguageCode = LocalizationManager.GetLanguageCode(LocalizationManager.mCurrentLanguage);
			LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
			LocalizationManager.HasJoinedWords = GoogleLanguages.LanguageCode_HasJoinedWord(LocalizationManager.mLanguageCode);
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0007FA90 File Offset: 0x0007DC90
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

		// Token: 0x060012F6 RID: 4854 RVA: 0x0001508B File Offset: 0x0001328B
		public static void ApplyLocalizationParams(ref string translation, bool allowLocalizedParameters = true)
		{
			LocalizationManager.ApplyLocalizationParams(ref translation, (string p) => LocalizationManager.GetLocalizationParam(p, null), allowLocalizedParameters);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0007FADC File Offset: 0x0007DCDC
		public static void ApplyLocalizationParams(ref string translation, GameObject root, bool allowLocalizedParameters = true)
		{
			LocalizationManager.ApplyLocalizationParams(ref translation, (string p) => LocalizationManager.GetLocalizationParam(p, root), allowLocalizedParameters);
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x0007FB0C File Offset: 0x0007DD0C
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

		// Token: 0x060012F9 RID: 4857 RVA: 0x0007FB3C File Offset: 0x0007DD3C
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

		// Token: 0x060012FA RID: 4858 RVA: 0x0007FD10 File Offset: 0x0007DF10
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

		// Token: 0x060012FB RID: 4859 RVA: 0x0007FD9C File Offset: 0x0007DF9C
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

		// Token: 0x060012FC RID: 4860 RVA: 0x000150B3 File Offset: 0x000132B3
		public static string ApplyRTLfix(string line)
		{
			return LocalizationManager.ApplyRTLfix(line, 0, true);
		}

		// Token: 0x060012FD RID: 4861 RVA: 0x0007FE1C File Offset: 0x0007E01C
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

		// Token: 0x060012FE RID: 4862 RVA: 0x000150BD File Offset: 0x000132BD
		public static string FixRTL_IfNeeded(string text, int maxCharacters = 0, bool ignoreNumber = false)
		{
			if (LocalizationManager.IsRight2Left)
			{
				return LocalizationManager.ApplyRTLfix(text, maxCharacters, ignoreNumber);
			}
			return text;
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x000150D0 File Offset: 0x000132D0
		public static bool IsRTL(string Code)
		{
			return Array.IndexOf<string>(LocalizationManager.LanguagesRTL, Code) >= 0;
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x000150E3 File Offset: 0x000132E3
		public static bool UpdateSources()
		{
			LocalizationManager.UnregisterDeletededSources();
			LocalizationManager.RegisterSourceInResources();
			LocalizationManager.RegisterSceneSources();
			return LocalizationManager.Sources.Count > 0;
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x0007FF30 File Offset: 0x0007E130
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

		// Token: 0x06001302 RID: 4866 RVA: 0x0007FF74 File Offset: 0x0007E174
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

		// Token: 0x06001303 RID: 4867 RVA: 0x0007FFE0 File Offset: 0x0007E1E0
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

		// Token: 0x06001304 RID: 4868 RVA: 0x00015101 File Offset: 0x00013301
		private static bool AllowSyncFromGoogle(LanguageSourceData Source)
		{
			return LocalizationManager.Callback_AllowSyncFromGoogle == null || LocalizationManager.Callback_AllowSyncFromGoogle(Source);
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x0008005C File Offset: 0x0007E25C
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

		// Token: 0x06001306 RID: 4870 RVA: 0x00015117 File Offset: 0x00013317
		private static IEnumerator Delayed_Import_Google(LanguageSourceData source, float delay, bool justCheck)
		{
			yield return new WaitForSeconds(delay);
			if (source != null)
			{
				source.Import_Google(false, justCheck);
			}
			yield break;
		}

		// Token: 0x06001307 RID: 4871 RVA: 0x00015134 File Offset: 0x00013334
		internal static void RemoveSource(LanguageSourceData Source)
		{
			LocalizationManager.Sources.Remove(Source);
		}

		// Token: 0x06001308 RID: 4872 RVA: 0x00015142 File Offset: 0x00013342
		public static bool IsGlobalSource(string SourceName)
		{
			return Array.IndexOf<string>(LocalizationManager.GlobalSources, SourceName) >= 0;
		}

		// Token: 0x06001309 RID: 4873 RVA: 0x0008010C File Offset: 0x0007E30C
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

		// Token: 0x0600130A RID: 4874 RVA: 0x00080178 File Offset: 0x0007E378
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

		// Token: 0x0600130B RID: 4875 RVA: 0x000801BC File Offset: 0x0007E3BC
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

		// Token: 0x0600130C RID: 4876 RVA: 0x00015155 File Offset: 0x00013355
		public static string GetCurrentDeviceLanguage(bool force = false)
		{
			if (force || string.IsNullOrEmpty(LocalizationManager.mCurrentDeviceLanguage))
			{
				LocalizationManager.DetectDeviceLanguage();
			}
			return LocalizationManager.mCurrentDeviceLanguage;
		}

		// Token: 0x0600130D RID: 4877 RVA: 0x000801F0 File Offset: 0x0007E3F0
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

		// Token: 0x0600130E RID: 4878 RVA: 0x0008024C File Offset: 0x0007E44C
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

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600130F RID: 4879 RVA: 0x000802D4 File Offset: 0x0007E4D4
		// (remove) Token: 0x06001310 RID: 4880 RVA: 0x00080308 File Offset: 0x0007E508
		public static event LocalizationManager.OnLocalizeCallback OnLocalizeEvent;

		// Token: 0x06001311 RID: 4881 RVA: 0x0008033C File Offset: 0x0007E53C
		public static string GetTranslation(string Term, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null, bool allowLocalizedParameters = true)
		{
			string text = null;
			LocalizationManager.TryGetTranslation(Term, out text, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage, allowLocalizedParameters);
			return text;
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x00015170 File Offset: 0x00013370
		public static string GetTermTranslation(string Term, bool FixForRTL = true, int maxLineLengthForRTL = 0, bool ignoreRTLnumbers = true, bool applyParameters = false, GameObject localParametersRoot = null, string overrideLanguage = null, bool allowLocalizedParameters = true)
		{
			return LocalizationManager.GetTranslation(Term, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage, allowLocalizedParameters);
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x00080360 File Offset: 0x0007E560
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

		// Token: 0x06001314 RID: 4884 RVA: 0x000803D8 File Offset: 0x0007E5D8
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

		// Token: 0x06001315 RID: 4885 RVA: 0x00015183 File Offset: 0x00013383
		public static T GetTranslatedObjectByTermName<T>(string Term, Localize optionalLocComp = null) where T : global::UnityEngine.Object
		{
			return LocalizationManager.GetTranslatedObject<T>(LocalizationManager.GetTranslation(Term, false, 0, true, false, null, null, true), null);
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x00080424 File Offset: 0x0007E624
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

		// Token: 0x06001317 RID: 4887 RVA: 0x00015198 File Offset: 0x00013398
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

		// Token: 0x06001318 RID: 4888 RVA: 0x000151CC File Offset: 0x000133CC
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

		// Token: 0x06001319 RID: 4889 RVA: 0x000804BC File Offset: 0x0007E6BC
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

		// Token: 0x0600131A RID: 4890 RVA: 0x00080508 File Offset: 0x0007E708
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

		// Token: 0x0600131B RID: 4891 RVA: 0x00080548 File Offset: 0x0007E748
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

		// Token: 0x0600131C RID: 4892 RVA: 0x000805C0 File Offset: 0x0007E7C0
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

		// Token: 0x0600131D RID: 4893 RVA: 0x00080604 File Offset: 0x0007E804
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

		// Token: 0x04001371 RID: 4977
		private static string mCurrentLanguage;

		// Token: 0x04001372 RID: 4978
		private static string mLanguageCode;

		// Token: 0x04001373 RID: 4979
		private static CultureInfo mCurrentCulture;

		// Token: 0x04001374 RID: 4980
		private static bool mChangeCultureInfo;

		// Token: 0x04001375 RID: 4981
		public static bool IsRight2Left;

		// Token: 0x04001376 RID: 4982
		public static bool HasJoinedWords;

		// Token: 0x04001377 RID: 4983
		public static List<ILocalizationParamsManager> ParamManagers = new List<ILocalizationParamsManager>();

		// Token: 0x04001378 RID: 4984
		public static LocalizationManager.FnCustomApplyLocalizationParams CustomApplyLocalizationParams;

		// Token: 0x04001379 RID: 4985
		private static string[] LanguagesRTL = new string[]
		{
			"ar-DZ", "ar", "ar-BH", "ar-EG", "ar-IQ", "ar-JO", "ar-KW", "ar-LB", "ar-LY", "ar-MA",
			"ar-OM", "ar-QA", "ar-SA", "ar-SY", "ar-TN", "ar-AE", "ar-YE", "fa", "he", "ur",
			"ji"
		};

		// Token: 0x0400137A RID: 4986
		public static List<LanguageSourceData> Sources = new List<LanguageSourceData>();

		// Token: 0x0400137B RID: 4987
		public static string[] GlobalSources = new string[] { "I2Languages" };

		// Token: 0x0400137C RID: 4988
		public static Func<LanguageSourceData, bool> Callback_AllowSyncFromGoogle = null;

		// Token: 0x0400137D RID: 4989
		private static string mCurrentDeviceLanguage;

		// Token: 0x0400137E RID: 4990
		public static List<ILocalizeTargetDescriptor> mLocalizeTargets = new List<ILocalizeTargetDescriptor>();

		// Token: 0x04001380 RID: 4992
		private static bool mLocalizeIsScheduled;

		// Token: 0x04001381 RID: 4993
		private static bool mLocalizeIsScheduledWithForcedValue;

		// Token: 0x04001382 RID: 4994
		public static bool HighlightLocalizedTargets = false;

		// Token: 0x020001B7 RID: 439
		// (Invoke) Token: 0x06001320 RID: 4896
		public delegate bool FnCustomApplyLocalizationParams(ref string translation, LocalizationManager._GetParam getParam, bool allowLocalizedParameters);

		// Token: 0x020001B8 RID: 440
		// (Invoke) Token: 0x06001324 RID: 4900
		public delegate object _GetParam(string param);

		// Token: 0x020001B9 RID: 441
		// (Invoke) Token: 0x06001328 RID: 4904
		public delegate void OnLocalizeCallback();
	}
}
