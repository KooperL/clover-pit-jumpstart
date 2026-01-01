using System;
using I2.Loc;
using UnityEngine;
using UnityEngine.Events;

namespace Panik
{
	// Token: 0x02000175 RID: 373
	public static class Translation
	{
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600111D RID: 4381 RVA: 0x00013F4B File Offset: 0x0001214B
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

		// Token: 0x0600111E RID: 4382 RVA: 0x000737C8 File Offset: 0x000719C8
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

		// Token: 0x0600111F RID: 4383 RVA: 0x00013F89 File Offset: 0x00012189
		public static Translation.Language LanguageGet()
		{
			return Data.settings.language;
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00073820 File Offset: 0x00071A20
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

		// Token: 0x06001121 RID: 4385 RVA: 0x00073894 File Offset: 0x00071A94
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

		// Token: 0x06001122 RID: 4386 RVA: 0x00013F95 File Offset: 0x00012195
		public static string LanguageNameGetCurrent()
		{
			return LocalizationManager.GetTermTranslation("YOUR_LANGUAGE_TRANSLATED", true, 0, true, false, null, null, true);
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00013FA8 File Offset: 0x000121A8
		public static string LanguageNameGetTranslated(Translation.Language language)
		{
			return Translation.languageNamesTranslated[(int)language];
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00013FB1 File Offset: 0x000121B1
		public static string LanguageI2NameGet(Translation.Language language)
		{
			return Translation.languagesI2Names[(int)language];
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x0007390C File Offset: 0x00071B0C
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
						case RuntimePlatform.GameCoreXboxSeries:
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

		// Token: 0x06001126 RID: 4390 RVA: 0x000739B4 File Offset: 0x00071BB4
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

		// Token: 0x04001214 RID: 4628
		public static string[] languagesI2Names = new string[]
		{
			"English", "Italian", "French", "German", "Spanish", "SpanishAmerica", "Portuguese", "PortugueseBrazil", "Chinese (Simplified)", "Japanese",
			"Ukraine", "Russian", "Korean"
		};

		// Token: 0x04001215 RID: 4629
		public static string[] languageNamesTranslated = new string[]
		{
			"English", "Italiano", "Français", "Deutsch", "Español", "Español (America)", "Português", "Português (Brazil)", "简体中文", "日本語",
			"Українська", "Русский", "한국어"
		};

		// Token: 0x04001216 RID: 4630
		private static Translation.Language[] _languagesInOrder = null;

		// Token: 0x04001217 RID: 4631
		private const string PLATFORM_KEY_SUFFIX_CHILL = "";

		// Token: 0x04001218 RID: 4632
		private const string PLATFORM_KEY_SUFFIX_PLAY_STATION = "_PS";

		// Token: 0x04001219 RID: 4633
		private const string PLATFORM_KEY_SUFFIX_NINTENDO = "_NINTENDO";

		// Token: 0x0400121A RID: 4634
		private const string PLATFORM_KEY_SUFFIX_XBOX = "_XBOX";

		// Token: 0x0400121B RID: 4635
		private static string currentPlatformKeySuffix = null;

		// Token: 0x0400121C RID: 4636
		public static UnityAction OnLanguageChanged;

		// Token: 0x02000176 RID: 374
		public enum Language
		{
			// Token: 0x0400121E RID: 4638
			Undefined = -1,
			// Token: 0x0400121F RID: 4639
			English,
			// Token: 0x04001220 RID: 4640
			Italian,
			// Token: 0x04001221 RID: 4641
			French,
			// Token: 0x04001222 RID: 4642
			German,
			// Token: 0x04001223 RID: 4643
			Spanish,
			// Token: 0x04001224 RID: 4644
			SpanishAmerica,
			// Token: 0x04001225 RID: 4645
			Portuguese,
			// Token: 0x04001226 RID: 4646
			PortugueseBrazil,
			// Token: 0x04001227 RID: 4647
			ChineseSimplified,
			// Token: 0x04001228 RID: 4648
			Japanese,
			// Token: 0x04001229 RID: 4649
			Ukraine,
			// Token: 0x0400122A RID: 4650
			Russian,
			// Token: 0x0400122B RID: 4651
			Korean
		}
	}
}
