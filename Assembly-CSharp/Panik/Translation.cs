using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.Events;

namespace Panik
{
	public static class Translation
	{
		// (get) Token: 0x06000D79 RID: 3449 RVA: 0x00055458 File Offset: 0x00053658
		public static Translation.Language[] LanguagesInOrder
		{
			get
			{
				if (Translation._languagesInOrder == null)
				{
					if (Master.instance.ENGLISH_ONLY_BUILD)
					{
						Translation._languagesInOrder = new Translation.Language[1];
					}
					else
					{
						Translation._languagesInOrder = new Translation.Language[]
						{
							Translation.Language.English,
							Translation.Language.ChineseSimplified,
							Translation.Language.PortugueseBrazil,
							Translation.Language.Japanese,
							Translation.Language.Spanish,
							Translation.Language.Korean,
							Translation.Language.French,
							Translation.Language.German,
							Translation.Language.Russian,
							Translation.Language.Italian
						};
					}
				}
				return Translation._languagesInOrder;
			}
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00055498 File Offset: 0x00053698
		public static void LanguageSet(Translation.Language language)
		{
			if (language == Translation.Language.Undefined)
			{
				Debug.LogError("Language is undefined! User will need to select one in the intro of the game or the main menu!");
				language = Translation.Language.English;
			}
			Translation.Language language2 = Data.settings.language;
			Data.settings.language = language;
			LocalizationManager.CurrentLanguage = Translation.languagesI2Names[(int)Data.settings.language];
			UnityAction onLanguageChanged = Translation.OnLanguageChanged;
			if (onLanguageChanged == null)
			{
				return;
			}
			onLanguageChanged();
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x000554F0 File Offset: 0x000536F0
		public static Translation.Language LanguageGet()
		{
			return Data.settings.language;
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x000554FC File Offset: 0x000536FC
		public static void LanguageSetNext()
		{
			Translation.Language language = Data.settings.language;
			int num = -1;
			for (int i = 0; i < Translation.LanguagesInOrder.Length; i++)
			{
				if (Translation.LanguagesInOrder[i] == language)
				{
					num = i;
				}
			}
			if (num < 0)
			{
				Debug.LogError("Translation.LanguageSetNext(): cannot change language. Current language: " + language.ToString());
			}
			num++;
			if (num >= Translation.LanguagesInOrder.Length)
			{
				num = 0;
			}
			Translation.LanguageSet(Translation.LanguagesInOrder[num]);
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00055570 File Offset: 0x00053770
		public static void LanguageSetPrevious()
		{
			Translation.Language language = Data.settings.language;
			int num = -1;
			for (int i = 0; i < Translation.LanguagesInOrder.Length; i++)
			{
				if (Translation.LanguagesInOrder[i] == language)
				{
					num = i;
				}
			}
			if (num < 0)
			{
				Debug.LogError("Translation.LanguageSetPrevious(): cannot change language. Current language: " + language.ToString());
			}
			num--;
			if (num < 0)
			{
				num = Translation.LanguagesInOrder.Length - 1;
			}
			Translation.LanguageSet(Translation.LanguagesInOrder[num]);
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x000555E6 File Offset: 0x000537E6
		public static string LanguageNameGetCurrent()
		{
			return LocalizationManager.GetTermTranslation("YOUR_LANGUAGE_TRANSLATED", true, 0, true, false, null, null, true);
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x000555F9 File Offset: 0x000537F9
		public static string LanguageNameGetTranslated(Translation.Language language)
		{
			return Translation.languageNamesTranslated[(int)language];
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00055602 File Offset: 0x00053802
		public static string LanguageI2NameGet(Translation.Language language)
		{
			return Translation.languagesI2Names[(int)language];
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0005560C File Offset: 0x0005380C
		public static string PlatformKeySuffixGet()
		{
			if (Translation.currentPlatformKeySuffix == null)
			{
				RuntimePlatform platform = Application.platform;
				if (platform != RuntimePlatform.PS4)
				{
					if (platform != RuntimePlatform.XboxOne)
					{
						switch (platform)
						{
						case RuntimePlatform.Switch:
							Translation.currentPlatformKeySuffix = "_NINTENDO";
							goto IL_0093;
						case RuntimePlatform.GameCoreScarlett:
							Translation.currentPlatformKeySuffix = "_XBOX";
							goto IL_0093;
						case RuntimePlatform.GameCoreXboxOne:
							Translation.currentPlatformKeySuffix = "_XBOX";
							goto IL_0093;
						case RuntimePlatform.PS5:
							Translation.currentPlatformKeySuffix = "_PS";
							goto IL_0093;
						}
						Translation.currentPlatformKeySuffix = "";
					}
					else
					{
						Translation.currentPlatformKeySuffix = "_XBOX";
					}
				}
				else
				{
					Translation.currentPlatformKeySuffix = "_PS";
				}
			}
			IL_0093:
			return Translation.currentPlatformKeySuffix;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x000556B4 File Offset: 0x000538B4
		public static string Get(string key)
		{
			string text = LocalizationManager.GetTermTranslation(key + Translation.PlatformKeySuffixGet(), true, 0, true, false, null, null, true);
			if (string.IsNullOrEmpty(text))
			{
				text = LocalizationManager.GetTermTranslation(key, true, 0, true, false, null, null, true);
			}
			if (string.IsNullOrEmpty(text))
			{
				return string.Concat(new string[]
				{
					"No string found for key: ",
					key,
					" in language: ",
					Data.settings.language.ToString(),
					"!"
				});
			}
			return text;
		}

		public static string[] languagesI2Names = new string[]
		{
			"English", "Italian", "French", "German", "Spanish", "SpanishAmerica", "Portuguese", "PortugueseBrazil", "Chinese (Simplified)", "Japanese",
			"Ukraine", "Russian", "Korean"
		};

		public static string[] languageNamesTranslated = new string[]
		{
			"English", "Italiano", "Français", "Deutsch", "Español", "Español (America)", "Português", "Português (Brazil)", "简体中文", "日本語",
			"Українська", "Русский", "한국어"
		};

		private static Translation.Language[] _languagesInOrder = null;

		private const string PLATFORM_KEY_SUFFIX_CHILL = "";

		private const string PLATFORM_KEY_SUFFIX_PLAY_STATION = "_PS";

		private const string PLATFORM_KEY_SUFFIX_NINTENDO = "_NINTENDO";

		private const string PLATFORM_KEY_SUFFIX_XBOX = "_XBOX";

		private static string currentPlatformKeySuffix = null;

		public static UnityAction OnLanguageChanged;

		public enum Language
		{
			Undefined = -1,
			English,
			Italian,
			French,
			German,
			Spanish,
			SpanishAmerica,
			Portuguese,
			PortugueseBrazil,
			ChineseSimplified,
			Japanese,
			Ukraine,
			Russian,
			Korean
		}
	}
}
