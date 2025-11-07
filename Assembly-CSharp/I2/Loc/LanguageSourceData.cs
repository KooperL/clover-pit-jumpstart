using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	[ExecuteInEditMode]
	[Serializable]
	public class LanguageSourceData
	{
		// (get) Token: 0x06000E7F RID: 3711 RVA: 0x0005CC8B File Offset: 0x0005AE8B
		public Object ownerObject
		{
			get
			{
				return this.owner as Object;
			}
		}

		// (add) Token: 0x06000E80 RID: 3712 RVA: 0x0005CC98 File Offset: 0x0005AE98
		// (remove) Token: 0x06000E81 RID: 3713 RVA: 0x0005CCD0 File Offset: 0x0005AED0
		public event LanguageSource.fnOnSourceUpdated Event_OnSourceUpdateFromGoogle;

		// Token: 0x06000E82 RID: 3714 RVA: 0x0005CD05 File Offset: 0x0005AF05
		public void Awake()
		{
			LocalizationManager.AddSource(this);
			this.UpdateDictionary(false);
			this.UpdateAssetDictionary();
			LocalizationManager.LocalizeAll(true);
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0005CD20 File Offset: 0x0005AF20
		public void OnDestroy()
		{
			LocalizationManager.RemoveSource(this);
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x0005CD28 File Offset: 0x0005AF28
		public bool IsEqualTo(LanguageSourceData Source)
		{
			if (Source.mLanguages.Count != this.mLanguages.Count)
			{
				return false;
			}
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (Source.GetLanguageIndex(this.mLanguages[i].Name, true, true) < 0)
				{
					return false;
				}
				i++;
			}
			if (Source.mTerms.Count != this.mTerms.Count)
			{
				return false;
			}
			for (int j = 0; j < this.mTerms.Count; j++)
			{
				if (Source.GetTermData(this.mTerms[j].Term, false) == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x0005CDD4 File Offset: 0x0005AFD4
		internal bool ManagerHasASimilarSource()
		{
			int i = 0;
			int count = LocalizationManager.Sources.Count;
			while (i < count)
			{
				LanguageSourceData languageSourceData = LocalizationManager.Sources[i];
				if (languageSourceData != null && languageSourceData.IsEqualTo(this) && languageSourceData != this)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x0005CE17 File Offset: 0x0005B017
		public void ClearAllData()
		{
			this.mTerms.Clear();
			this.mLanguages.Clear();
			this.mDictionary.Clear();
			this.mAssetDictionary.Clear();
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0005CE45 File Offset: 0x0005B045
		public bool IsGlobalSource()
		{
			return this.mIsGlobalSource;
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x0005CE4D File Offset: 0x0005B04D
		public void Editor_SetDirty()
		{
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x0005CE50 File Offset: 0x0005B050
		public void UpdateAssetDictionary()
		{
			this.Assets.RemoveAll((Object x) => x == null);
			this.mAssetDictionary = this.Assets.Distinct<Object>().GroupBy((Object o) => o.name, StringComparer.Ordinal).ToDictionary((IGrouping<string, Object> g) => g.Key, (IGrouping<string, Object> g) => g.First<Object>(), StringComparer.Ordinal);
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x0005CF0C File Offset: 0x0005B10C
		public Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				if (this.mAssetDictionary == null || this.mAssetDictionary.Count != this.Assets.Count)
				{
					this.UpdateAssetDictionary();
				}
				Object @object;
				if (this.mAssetDictionary.TryGetValue(Name, out @object))
				{
					return @object;
				}
			}
			return null;
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x0005CF5A File Offset: 0x0005B15A
		public bool HasAsset(Object Obj)
		{
			return this.Assets.Contains(Obj);
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0005CF68 File Offset: 0x0005B168
		public void AddAsset(Object Obj)
		{
			if (this.Assets.Contains(Obj))
			{
				return;
			}
			this.Assets.Add(Obj);
			this.UpdateAssetDictionary();
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0005CF8C File Offset: 0x0005B18C
		private string Export_Language_to_Cache(int langIndex, bool fillTermWithFallback)
		{
			if (!this.mLanguages[langIndex].IsLoaded())
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.mTerms.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append("[i2t]");
				}
				TermData termData = this.mTerms[i];
				stringBuilder.Append(termData.Term);
				stringBuilder.Append("=");
				string text = termData.Languages[langIndex];
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.Fallback && string.IsNullOrEmpty(text) && this.TryGetFallbackTranslation(termData, out text, langIndex, null, true))
				{
					stringBuilder.Append("[i2fb]");
					if (fillTermWithFallback)
					{
						termData.Languages[langIndex] = text;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0005D05C File Offset: 0x0005B25C
		public string Export_I2CSV(string Category, char Separator = ',', bool specializationsAsRows = true, bool sortRows = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Key[*]Type[*]Desc");
			foreach (LanguageData languageData in this.mLanguages)
			{
				stringBuilder.Append("[*]");
				if (!languageData.IsEnabled())
				{
					stringBuilder.Append('$');
				}
				stringBuilder.Append(GoogleLanguages.GetCodedLanguage(languageData.Name, languageData.Code));
			}
			stringBuilder.Append("[ln]");
			if (sortRows)
			{
				this.mTerms.Sort((TermData a, TermData b) => string.CompareOrdinal(a.Term, b.Term));
			}
			int count = this.mLanguages.Count;
			bool flag = true;
			foreach (TermData termData in this.mTerms)
			{
				string text;
				if (string.IsNullOrEmpty(Category) || (Category == LanguageSourceData.EmptyCategory && termData.Term.IndexOfAny(LanguageSourceData.CategorySeparators) < 0))
				{
					text = termData.Term;
				}
				else
				{
					if (!termData.Term.StartsWith(Category + "/", StringComparison.Ordinal) || !(Category != termData.Term))
					{
						continue;
					}
					text = termData.Term.Substring(Category.Length + 1);
				}
				if (!flag)
				{
					stringBuilder.Append("[ln]");
				}
				flag = false;
				if (!specializationsAsRows)
				{
					LanguageSourceData.AppendI2Term(stringBuilder, count, text, termData, Separator, null);
				}
				else
				{
					List<string> allSpecializations = termData.GetAllSpecializations();
					for (int i = 0; i < allSpecializations.Count; i++)
					{
						if (i != 0)
						{
							stringBuilder.Append("[ln]");
						}
						string text2 = allSpecializations[i];
						LanguageSourceData.AppendI2Term(stringBuilder, count, text, termData, Separator, text2);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0005D280 File Offset: 0x0005B480
		private static void AppendI2Term(StringBuilder Builder, int nLanguages, string Term, TermData termData, char Separator, string forceSpecialization)
		{
			LanguageSourceData.AppendI2Text(Builder, Term);
			if (!string.IsNullOrEmpty(forceSpecialization) && forceSpecialization != "Any")
			{
				Builder.Append("[");
				Builder.Append(forceSpecialization);
				Builder.Append("]");
			}
			Builder.Append("[*]");
			Builder.Append(termData.TermType.ToString());
			Builder.Append("[*]");
			Builder.Append(termData.Description);
			for (int i = 0; i < Mathf.Min(nLanguages, termData.Languages.Length); i++)
			{
				Builder.Append("[*]");
				string text = termData.Languages[i];
				if (!string.IsNullOrEmpty(forceSpecialization))
				{
					text = termData.GetTranslation(i, forceSpecialization, false);
				}
				LanguageSourceData.AppendI2Text(Builder, text);
			}
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0005D352 File Offset: 0x0005B552
		private static void AppendI2Text(StringBuilder Builder, string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (text.StartsWith("'", StringComparison.Ordinal) || text.StartsWith("=", StringComparison.Ordinal))
			{
				Builder.Append('\'');
			}
			Builder.Append(text);
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x0005D38C File Offset: 0x0005B58C
		public string Export_CSV(string Category, char Separator = ',', bool specializationsAsRows = true, bool sortRows = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = this.mLanguages.Count;
			stringBuilder.AppendFormat("Key{0}Type{0}Desc", Separator);
			foreach (LanguageData languageData in this.mLanguages)
			{
				stringBuilder.Append(Separator);
				if (!languageData.IsEnabled())
				{
					stringBuilder.Append('$');
				}
				LanguageSourceData.AppendString(stringBuilder, GoogleLanguages.GetCodedLanguage(languageData.Name, languageData.Code), Separator);
			}
			stringBuilder.Append("\n");
			if (sortRows)
			{
				this.mTerms.Sort((TermData a, TermData b) => string.CompareOrdinal(a.Term, b.Term));
			}
			foreach (TermData termData in this.mTerms)
			{
				string text;
				if (string.IsNullOrEmpty(Category) || (Category == LanguageSourceData.EmptyCategory && termData.Term.IndexOfAny(LanguageSourceData.CategorySeparators) < 0))
				{
					text = termData.Term;
				}
				else
				{
					if (!termData.Term.StartsWith(Category + "/", StringComparison.Ordinal) || !(Category != termData.Term))
					{
						continue;
					}
					text = termData.Term.Substring(Category.Length + 1);
				}
				if (specializationsAsRows)
				{
					using (List<string>.Enumerator enumerator3 = termData.GetAllSpecializations().GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							string text2 = enumerator3.Current;
							LanguageSourceData.AppendTerm(stringBuilder, count, text, termData, text2, Separator);
						}
						continue;
					}
				}
				LanguageSourceData.AppendTerm(stringBuilder, count, text, termData, null, Separator);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x0005D580 File Offset: 0x0005B780
		private static void AppendTerm(StringBuilder Builder, int nLanguages, string Term, TermData termData, string specialization, char Separator)
		{
			LanguageSourceData.AppendString(Builder, Term, Separator);
			if (!string.IsNullOrEmpty(specialization) && specialization != "Any")
			{
				Builder.AppendFormat("[{0}]", specialization);
			}
			Builder.Append(Separator);
			Builder.Append(termData.TermType.ToString());
			Builder.Append(Separator);
			LanguageSourceData.AppendString(Builder, termData.Description, Separator);
			for (int i = 0; i < Mathf.Min(nLanguages, termData.Languages.Length); i++)
			{
				Builder.Append(Separator);
				string text = termData.Languages[i];
				if (!string.IsNullOrEmpty(specialization))
				{
					text = termData.GetTranslation(i, specialization, false);
				}
				LanguageSourceData.AppendTranslation(Builder, text, Separator, null);
			}
			Builder.Append("\n");
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x0005D648 File Offset: 0x0005B848
		private static void AppendString(StringBuilder Builder, string Text, char Separator)
		{
			if (string.IsNullOrEmpty(Text))
			{
				return;
			}
			Text = Text.Replace("\\n", "\n");
			if (Text.IndexOfAny((Separator.ToString() + "\n\"").ToCharArray()) >= 0)
			{
				Text = Text.Replace("\"", "\"\"");
				Builder.AppendFormat("\"{0}\"", Text);
				return;
			}
			Builder.Append(Text);
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0005D6B8 File Offset: 0x0005B8B8
		private static void AppendTranslation(StringBuilder Builder, string Text, char Separator, string tags)
		{
			if (string.IsNullOrEmpty(Text))
			{
				return;
			}
			Text = Text.Replace("\\n", "\n");
			if (Text.IndexOfAny((Separator.ToString() + "\n\"").ToCharArray()) >= 0)
			{
				Text = Text.Replace("\"", "\"\"");
				Builder.AppendFormat("\"{0}{1}\"", tags, Text);
				return;
			}
			Builder.Append(tags);
			Builder.Append(Text);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x0005D730 File Offset: 0x0005B930
		public UnityWebRequest Export_Google_CreateWWWcall(eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			string text = this.Export_Google_CreateData();
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("key", this.Google_SpreadsheetKey);
			wwwform.AddField("action", "SetLanguageSource");
			wwwform.AddField("data", text);
			wwwform.AddField("updateMode", UpdateMode.ToString());
			UnityWebRequest unityWebRequest = UnityWebRequest.Post(LocalizationManager.GetWebServiceURL(this), wwwform);
			I2Utils.SendWebRequest(unityWebRequest);
			return unityWebRequest;
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x0005D7A4 File Offset: 0x0005B9A4
		private string Export_Google_CreateData()
		{
			List<string> categories = this.GetCategories(true, null);
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (string text in categories)
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append("<I2Loc>");
				}
				bool flag2 = true;
				bool flag3 = true;
				string text2 = this.Export_I2CSV(text, ',', flag2, flag3);
				stringBuilder.Append(text);
				stringBuilder.Append("<I2Loc>");
				stringBuilder.Append(text2);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x0005D848 File Offset: 0x0005BA48
		public string Import_CSV(string Category, string CSVstring, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace, char Separator = ',')
		{
			List<string[]> list = LocalizationReader.ReadCSV(CSVstring, Separator);
			return this.Import_CSV(Category, list, UpdateMode);
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x0005D868 File Offset: 0x0005BA68
		public string Import_I2CSV(string Category, string I2CSVstring, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			List<string[]> list = LocalizationReader.ReadI2CSV(I2CSVstring);
			return this.Import_CSV(Category, list, UpdateMode);
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0005D888 File Offset: 0x0005BA88
		public string Import_CSV(string Category, List<string[]> CSV, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			string[] array = CSV[0];
			int num = 1;
			int num2 = -1;
			int num3 = -1;
			string[] array2 = new string[] { "Key" };
			string[] array3 = new string[] { "Type" };
			string[] array4 = new string[] { "Desc", "Description" };
			if (array.Length > 1 && this.ArrayContains(array[0], array2))
			{
				if (UpdateMode == eSpreadsheetUpdateMode.Replace)
				{
					this.ClearAllData();
				}
				if (array.Length > 2)
				{
					if (this.ArrayContains(array[1], array3))
					{
						num2 = 1;
						num = 2;
					}
					if (this.ArrayContains(array[1], array4))
					{
						num3 = 1;
						num = 2;
					}
				}
				if (array.Length > 3)
				{
					if (this.ArrayContains(array[2], array3))
					{
						num2 = 2;
						num = 3;
					}
					if (this.ArrayContains(array[2], array4))
					{
						num3 = 2;
						num = 3;
					}
				}
				int num4 = Mathf.Max(array.Length - num, 0);
				int[] array5 = new int[num4];
				for (int i = 0; i < num4; i++)
				{
					if (string.IsNullOrEmpty(array[i + num]))
					{
						array5[i] = -1;
					}
					else
					{
						string text = array[i + num];
						bool flag = true;
						if (text.StartsWith("$", StringComparison.Ordinal))
						{
							flag = false;
							text = text.Substring(1);
						}
						string text2;
						string text3;
						GoogleLanguages.UnPackCodeFromLanguageName(text, out text2, out text3);
						int num5;
						if (!string.IsNullOrEmpty(text3))
						{
							num5 = this.GetLanguageIndexFromCode(text3, true, false);
						}
						else
						{
							num5 = this.GetLanguageIndex(text2, true, false);
						}
						if (num5 < 0)
						{
							LanguageData languageData = new LanguageData();
							languageData.Name = text2;
							languageData.Code = text3;
							languageData.Flags = (byte)(0 | (flag ? 0 : 1));
							this.mLanguages.Add(languageData);
							num5 = this.mLanguages.Count - 1;
						}
						array5[i] = num5;
					}
				}
				num4 = this.mLanguages.Count;
				int j = 0;
				int count = this.mTerms.Count;
				while (j < count)
				{
					TermData termData = this.mTerms[j];
					if (termData.Languages.Length < num4)
					{
						Array.Resize<string>(ref termData.Languages, num4);
						Array.Resize<byte>(ref termData.Flags, num4);
					}
					j++;
				}
				int k = 1;
				int count2 = CSV.Count;
				while (k < count2)
				{
					array = CSV[k];
					string text4 = (string.IsNullOrEmpty(Category) ? array[0] : (Category + "/" + array[0]));
					string text5 = null;
					if (text4.EndsWith("]", StringComparison.Ordinal))
					{
						int num6 = text4.LastIndexOf('[');
						if (num6 > 0)
						{
							text5 = text4.Substring(num6 + 1, text4.Length - num6 - 2);
							if (text5 == "touch")
							{
								text5 = "Touch";
							}
							text4 = text4.Remove(num6);
						}
					}
					LanguageSourceData.ValidateFullTerm(ref text4);
					if (!string.IsNullOrEmpty(text4))
					{
						TermData termData2 = this.GetTermData(text4, false);
						if (termData2 == null)
						{
							termData2 = new TermData();
							termData2.Term = text4;
							termData2.Languages = new string[this.mLanguages.Count];
							termData2.Flags = new byte[this.mLanguages.Count];
							for (int l = 0; l < this.mLanguages.Count; l++)
							{
								termData2.Languages[l] = string.Empty;
							}
							this.mTerms.Add(termData2);
							this.mDictionary.Add(text4, termData2);
						}
						else if (UpdateMode == eSpreadsheetUpdateMode.AddNewTerms)
						{
							goto IL_03E3;
						}
						if (num2 > 0)
						{
							termData2.TermType = LanguageSourceData.GetTermType(array[num2]);
						}
						if (num3 > 0)
						{
							termData2.Description = array[num3];
						}
						int num7 = 0;
						while (num7 < array5.Length && num7 < array.Length - num)
						{
							if (!string.IsNullOrEmpty(array[num7 + num]))
							{
								int num8 = array5[num7];
								if (num8 >= 0)
								{
									string text6 = array[num7 + num];
									if (text6 == "-")
									{
										text6 = string.Empty;
									}
									else if (text6 == "")
									{
										text6 = null;
									}
									termData2.SetTranslation(num8, text6, text5);
								}
							}
							num7++;
						}
					}
					IL_03E3:
					k++;
				}
				if (Application.isPlaying)
				{
					this.SaveLanguages(this.HasUnloadedLanguages(), PersistentStorage.eFileType.Temporal);
				}
				return string.Empty;
			}
			return "Bad Spreadsheet Format.\nFirst columns should be 'Key', 'Type' and 'Desc'";
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0005DCA0 File Offset: 0x0005BEA0
		private bool ArrayContains(string MainText, params string[] texts)
		{
			int i = 0;
			int num = texts.Length;
			while (i < num)
			{
				if (MainText.IndexOf(texts[i], StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x0005DCD0 File Offset: 0x0005BED0
		public static eTermType GetTermType(string type)
		{
			int i = 0;
			int num = 10;
			while (i <= num)
			{
				eTermType eTermType = (eTermType)i;
				if (string.Equals(eTermType.ToString(), type, StringComparison.OrdinalIgnoreCase))
				{
					return (eTermType)i;
				}
				i++;
			}
			return eTermType.Text;
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x0005DD08 File Offset: 0x0005BF08
		private void Import_Language_from_Cache(int langIndex, string langData, bool useFallback, bool onlyCurrentSpecialization)
		{
			int num;
			for (int i = 0; i < langData.Length; i = num + 5)
			{
				num = langData.IndexOf("[i2t]", i, StringComparison.Ordinal);
				if (num < 0)
				{
					num = langData.Length;
				}
				int num2 = langData.IndexOf("=", i, StringComparison.Ordinal);
				if (num2 >= num)
				{
					return;
				}
				string text = langData.Substring(i, num2 - i);
				i = num2 + 1;
				TermData termData = this.GetTermData(text, false);
				if (termData != null)
				{
					string text2 = null;
					if (i != num)
					{
						text2 = langData.Substring(i, num - i);
						if (text2.StartsWith("[i2fb]", StringComparison.Ordinal))
						{
							text2 = (useFallback ? text2.Substring(6) : null);
						}
						if (onlyCurrentSpecialization && text2 != null)
						{
							text2 = SpecializationManager.GetSpecializedText(text2, null);
						}
					}
					termData.Languages[langIndex] = text2;
				}
			}
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0005DDC4 File Offset: 0x0005BFC4
		public static void FreeUnusedLanguages()
		{
			LanguageSourceData languageSourceData = LocalizationManager.Sources[0];
			int languageIndex = languageSourceData.GetLanguageIndex(LocalizationManager.CurrentLanguage, true, true);
			for (int i = 0; i < languageSourceData.mTerms.Count; i++)
			{
				TermData termData = languageSourceData.mTerms[i];
				for (int j = 0; j < termData.Languages.Length; j++)
				{
					if (j != languageIndex)
					{
						termData.Languages[j] = null;
					}
				}
			}
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x0005DE34 File Offset: 0x0005C034
		public void Import_Google_FromCache()
		{
			if (this.GoogleUpdateFrequency == LanguageSourceData.eGoogleUpdateFrequency.Never)
			{
				return;
			}
			if (!I2Utils.IsPlaying())
			{
				return;
			}
			string sourcePlayerPrefName = this.GetSourcePlayerPrefName();
			string text = PersistentStorage.LoadFile(PersistentStorage.eFileType.Persistent, "I2Source_" + sourcePlayerPrefName + ".loc", false);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (text.StartsWith("[i2e]", StringComparison.Ordinal))
			{
				text = StringObfucator.Decode(text.Substring(5, text.Length - 5));
			}
			bool flag = false;
			string text2 = this.Google_LastUpdatedVersion;
			if (PersistentStorage.HasSetting("I2SourceVersion_" + sourcePlayerPrefName))
			{
				text2 = PersistentStorage.GetSetting_String("I2SourceVersion_" + sourcePlayerPrefName, this.Google_LastUpdatedVersion);
				flag = this.IsNewerVersion(this.Google_LastUpdatedVersion, text2);
			}
			if (!flag)
			{
				PersistentStorage.DeleteFile(PersistentStorage.eFileType.Persistent, "I2Source_" + sourcePlayerPrefName + ".loc", false);
				PersistentStorage.DeleteSetting("I2SourceVersion_" + sourcePlayerPrefName);
				return;
			}
			if (text2.Length > 19)
			{
				text2 = string.Empty;
			}
			this.Google_LastUpdatedVersion = text2;
			this.Import_Google_Result(text, eSpreadsheetUpdateMode.Replace, false);
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0005DF2C File Offset: 0x0005C12C
		private bool IsNewerVersion(string currentVersion, string newVersion)
		{
			long num;
			long num2;
			return !string.IsNullOrEmpty(newVersion) && (string.IsNullOrEmpty(currentVersion) || (!long.TryParse(newVersion, out num) || !long.TryParse(currentVersion, out num2)) || num > num2);
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x0005DF68 File Offset: 0x0005C168
		public void Import_Google(bool ForceUpdate, bool justCheck)
		{
			if (!ForceUpdate && this.GoogleUpdateFrequency == LanguageSourceData.eGoogleUpdateFrequency.Never)
			{
				return;
			}
			if (!I2Utils.IsPlaying())
			{
				return;
			}
			LanguageSourceData.eGoogleUpdateFrequency googleUpdateFrequency = this.GoogleUpdateFrequency;
			string sourcePlayerPrefName = this.GetSourcePlayerPrefName();
			if (!ForceUpdate && googleUpdateFrequency != LanguageSourceData.eGoogleUpdateFrequency.Always)
			{
				string setting_String = PersistentStorage.GetSetting_String("LastGoogleUpdate_" + sourcePlayerPrefName, "");
				try
				{
					DateTime dateTime;
					if (DateTime.TryParse(setting_String, out dateTime))
					{
						double totalDays = (DateTime.Now - dateTime).TotalDays;
						switch (googleUpdateFrequency)
						{
						case LanguageSourceData.eGoogleUpdateFrequency.Daily:
							if (totalDays >= 1.0)
							{
								goto IL_00BF;
							}
							break;
						case LanguageSourceData.eGoogleUpdateFrequency.Weekly:
							if (totalDays >= 8.0)
							{
								goto IL_00BF;
							}
							break;
						case LanguageSourceData.eGoogleUpdateFrequency.Monthly:
							if (totalDays >= 31.0)
							{
								goto IL_00BF;
							}
							break;
						case LanguageSourceData.eGoogleUpdateFrequency.OnlyOnce:
							break;
						case LanguageSourceData.eGoogleUpdateFrequency.EveryOtherDay:
							if (totalDays >= 2.0)
							{
								goto IL_00BF;
							}
							break;
						default:
							goto IL_00BF;
						}
						return;
					}
					IL_00BF:;
				}
				catch (Exception)
				{
				}
			}
			PersistentStorage.SetSetting_String("LastGoogleUpdate_" + sourcePlayerPrefName, DateTime.Now.ToString());
			CoroutineManager.Start(this.Import_Google_Coroutine(ForceUpdate, justCheck));
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0005E078 File Offset: 0x0005C278
		private string GetSourcePlayerPrefName()
		{
			if (this.owner == null)
			{
				return null;
			}
			string text = (this.owner as Object).name;
			if (!string.IsNullOrEmpty(this.Google_SpreadsheetKey))
			{
				text += this.Google_SpreadsheetKey;
			}
			if (Array.IndexOf<string>(LocalizationManager.GlobalSources, (this.owner as Object).name) >= 0)
			{
				return text;
			}
			return SceneManager.GetActiveScene().name + "_" + text;
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x0005E0F1 File Offset: 0x0005C2F1
		private IEnumerator Import_Google_Coroutine(bool forceUpdate, bool JustCheck)
		{
			UnityWebRequest www = this.Import_Google_CreateWWWcall(forceUpdate, JustCheck);
			if (www == null)
			{
				yield break;
			}
			while (!www.isDone)
			{
				yield return null;
			}
			byte[] data = www.downloadHandler.data;
			if (string.IsNullOrEmpty(www.error) && data != null)
			{
				string @string = Encoding.UTF8.GetString(data, 0, data.Length);
				bool flag = string.IsNullOrEmpty(@string) || @string == "\"\"";
				if (JustCheck)
				{
					if (!flag)
					{
						Debug.LogWarning("Spreadsheet is not up-to-date and Google Live Synchronization is enabled\nWhen playing in the device the Spreadsheet will be downloaded and translations may not behave as what you see in the editor.\nTo fix this, Import or Export replace to Google");
						this.GoogleLiveSyncIsUptoDate = false;
					}
					yield break;
				}
				if (!flag)
				{
					this.mDelayedGoogleData = @string;
					switch (this.GoogleUpdateSynchronization)
					{
					case LanguageSourceData.eGoogleUpdateSynchronization.OnSceneLoaded:
						SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.ApplyDownloadedDataOnSceneLoaded);
						break;
					case LanguageSourceData.eGoogleUpdateSynchronization.AsSoonAsDownloaded:
						this.ApplyDownloadedDataFromGoogle();
						break;
					}
					yield break;
				}
			}
			if (this.Event_OnSourceUpdateFromGoogle != null)
			{
				this.Event_OnSourceUpdateFromGoogle(this, false, www.error);
			}
			Debug.Log("Language Source was up-to-date with Google Spreadsheet");
			yield break;
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0005E10E File Offset: 0x0005C30E
		private void ApplyDownloadedDataOnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.ApplyDownloadedDataOnSceneLoaded);
			this.ApplyDownloadedDataFromGoogle();
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x0005E128 File Offset: 0x0005C328
		public void ApplyDownloadedDataFromGoogle()
		{
			if (string.IsNullOrEmpty(this.mDelayedGoogleData))
			{
				return;
			}
			if (string.IsNullOrEmpty(this.Import_Google_Result(this.mDelayedGoogleData, eSpreadsheetUpdateMode.Replace, true)))
			{
				if (this.Event_OnSourceUpdateFromGoogle != null)
				{
					this.Event_OnSourceUpdateFromGoogle(this, true, "");
				}
				LocalizationManager.LocalizeAll(true);
				Debug.Log("Done Google Sync");
				return;
			}
			if (this.Event_OnSourceUpdateFromGoogle != null)
			{
				this.Event_OnSourceUpdateFromGoogle(this, false, "");
			}
			Debug.Log("Done Google Sync: source was up-to-date");
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0005E1A8 File Offset: 0x0005C3A8
		public UnityWebRequest Import_Google_CreateWWWcall(bool ForceUpdate, bool justCheck)
		{
			if (!this.HasGoogleSpreadsheet())
			{
				return null;
			}
			string text = PersistentStorage.GetSetting_String("I2SourceVersion_" + this.GetSourcePlayerPrefName(), this.Google_LastUpdatedVersion);
			if (text.Length > 19)
			{
				text = string.Empty;
			}
			if (this.IsNewerVersion(text, this.Google_LastUpdatedVersion))
			{
				this.Google_LastUpdatedVersion = text;
			}
			UnityWebRequest unityWebRequest = UnityWebRequest.Get(string.Format("{0}?key={1}&action=GetLanguageSource&version={2}", LocalizationManager.GetWebServiceURL(this), this.Google_SpreadsheetKey, ForceUpdate ? "0" : this.Google_LastUpdatedVersion));
			I2Utils.SendWebRequest(unityWebRequest);
			return unityWebRequest;
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x0005E232 File Offset: 0x0005C432
		public bool HasGoogleSpreadsheet()
		{
			return !string.IsNullOrEmpty(this.Google_WebServiceURL) && !string.IsNullOrEmpty(this.Google_SpreadsheetKey) && !string.IsNullOrEmpty(LocalizationManager.GetWebServiceURL(this));
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0005E260 File Offset: 0x0005C460
		public string Import_Google_Result(string JsonString, eSpreadsheetUpdateMode UpdateMode, bool saveInPlayerPrefs = false)
		{
			string text;
			try
			{
				string empty = string.Empty;
				if (string.IsNullOrEmpty(JsonString) || JsonString == "\"\"")
				{
					text = empty;
				}
				else
				{
					int num = JsonString.IndexOf("version=", StringComparison.Ordinal);
					int num2 = JsonString.IndexOf("script_version=", StringComparison.Ordinal);
					if (num < 0 || num2 < 0)
					{
						text = "Invalid Response from Google, Most likely the WebService needs to be updated";
					}
					else
					{
						num += "version=".Length;
						num2 += "script_version=".Length;
						string text2 = JsonString.Substring(num, JsonString.IndexOf(",", num, StringComparison.Ordinal) - num);
						int num3 = int.Parse(JsonString.Substring(num2, JsonString.IndexOf(",", num2, StringComparison.Ordinal) - num2));
						if (text2.Length > 19)
						{
							text2 = string.Empty;
						}
						if (num3 != LocalizationManager.GetRequiredWebServiceVersion())
						{
							text = "The current Google WebService is not supported.\nPlease, delete the WebService from the Google Drive and Install the latest version.";
						}
						else if (saveInPlayerPrefs && !this.IsNewerVersion(this.Google_LastUpdatedVersion, text2))
						{
							text = "LanguageSource is up-to-date";
						}
						else
						{
							if (saveInPlayerPrefs)
							{
								string sourcePlayerPrefName = this.GetSourcePlayerPrefName();
								PersistentStorage.SaveFile(PersistentStorage.eFileType.Persistent, "I2Source_" + sourcePlayerPrefName + ".loc", "[i2e]" + StringObfucator.Encode(JsonString), true);
								PersistentStorage.SetSetting_String("I2SourceVersion_" + sourcePlayerPrefName, text2);
								PersistentStorage.ForceSaveSettings();
							}
							this.Google_LastUpdatedVersion = text2;
							if (UpdateMode == eSpreadsheetUpdateMode.Replace)
							{
								this.ClearAllData();
							}
							int i = JsonString.IndexOf("[i2category]", StringComparison.Ordinal);
							while (i > 0)
							{
								i += "[i2category]".Length;
								int num4 = JsonString.IndexOf("[/i2category]", i, StringComparison.Ordinal);
								string text3 = JsonString.Substring(i, num4 - i);
								num4 += "[/i2category]".Length;
								int num5 = JsonString.IndexOf("[/i2csv]", num4, StringComparison.Ordinal);
								string text4 = JsonString.Substring(num4, num5 - num4);
								i = JsonString.IndexOf("[i2category]", num5, StringComparison.Ordinal);
								this.Import_I2CSV(text3, text4, UpdateMode);
								if (UpdateMode == eSpreadsheetUpdateMode.Replace)
								{
									UpdateMode = eSpreadsheetUpdateMode.Merge;
								}
							}
							this.GoogleLiveSyncIsUptoDate = true;
							if (I2Utils.IsPlaying())
							{
								this.SaveLanguages(true, PersistentStorage.eFileType.Temporal);
							}
							if (!string.IsNullOrEmpty(empty))
							{
								this.Editor_SetDirty();
							}
							text = empty;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning(ex);
				text = ex.ToString();
			}
			return text;
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x0005E494 File Offset: 0x0005C694
		public int GetLanguageIndex(string language, bool AllowDiscartingRegion = true, bool SkipDisabled = true)
		{
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if ((!SkipDisabled || this.mLanguages[i].IsEnabled()) && string.Compare(this.mLanguages[i].Name, language, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
				i++;
			}
			if (AllowDiscartingRegion)
			{
				int num = -1;
				int num2 = 0;
				int j = 0;
				int count2 = this.mLanguages.Count;
				while (j < count2)
				{
					if (!SkipDisabled || this.mLanguages[j].IsEnabled())
					{
						int commonWordInLanguageNames = LanguageSourceData.GetCommonWordInLanguageNames(this.mLanguages[j].Name, language);
						if (commonWordInLanguageNames > num2)
						{
							num2 = commonWordInLanguageNames;
							num = j;
						}
					}
					j++;
				}
				if (num >= 0)
				{
					return num;
				}
			}
			return -1;
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0005E554 File Offset: 0x0005C754
		public LanguageData GetLanguageData(string language, bool AllowDiscartingRegion = true)
		{
			int languageIndex = this.GetLanguageIndex(language, AllowDiscartingRegion, false);
			if (languageIndex >= 0)
			{
				return this.mLanguages[languageIndex];
			}
			return null;
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0005E57D File Offset: 0x0005C77D
		public bool IsCurrentLanguage(int languageIndex)
		{
			return LocalizationManager.CurrentLanguage == this.mLanguages[languageIndex].Name;
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0005E59C File Offset: 0x0005C79C
		public int GetLanguageIndexFromCode(string Code, bool exactMatch = true, bool ignoreDisabled = false)
		{
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if ((!ignoreDisabled || this.mLanguages[i].IsEnabled()) && string.Compare(this.mLanguages[i].Code, Code, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return i;
				}
				i++;
			}
			if (!exactMatch)
			{
				int j = 0;
				int count2 = this.mLanguages.Count;
				while (j < count2)
				{
					if ((!ignoreDisabled || this.mLanguages[j].IsEnabled()) && string.Compare(this.mLanguages[j].Code, 0, Code, 0, 2, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return j;
					}
					j++;
				}
			}
			return -1;
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x0005E644 File Offset: 0x0005C844
		public static int GetCommonWordInLanguageNames(string Language1, string Language2)
		{
			if (string.IsNullOrEmpty(Language1) || string.IsNullOrEmpty(Language2))
			{
				return 0;
			}
			char[] array = "( )-/\\".ToCharArray();
			string[] array2 = Language1.ToLower().Split(array);
			string[] array3 = Language2.ToLower().Split(array);
			int num = 0;
			foreach (string text in array2)
			{
				if (!string.IsNullOrEmpty(text) && array3.Contains(text))
				{
					num++;
				}
			}
			foreach (string text2 in array3)
			{
				if (!string.IsNullOrEmpty(text2) && array2.Contains(text2))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x0005E6F3 File Offset: 0x0005C8F3
		public static bool AreTheSameLanguage(string Language1, string Language2)
		{
			Language1 = LanguageSourceData.GetLanguageWithoutRegion(Language1);
			Language2 = LanguageSourceData.GetLanguageWithoutRegion(Language2);
			return string.Compare(Language1, Language2, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x0005E710 File Offset: 0x0005C910
		public static string GetLanguageWithoutRegion(string Language)
		{
			int num = Language.IndexOfAny("(/\\[,{".ToCharArray());
			if (num < 0)
			{
				return Language;
			}
			return Language.Substring(0, num).Trim();
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x0005E741 File Offset: 0x0005C941
		public void AddLanguage(string LanguageName)
		{
			this.AddLanguage(LanguageName, GoogleLanguages.GetLanguageCode(LanguageName, false));
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0005E754 File Offset: 0x0005C954
		public void AddLanguage(string LanguageName, string LanguageCode)
		{
			if (this.GetLanguageIndex(LanguageName, false, true) >= 0)
			{
				return;
			}
			LanguageData languageData = new LanguageData();
			languageData.Name = LanguageName;
			languageData.Code = LanguageCode;
			this.mLanguages.Add(languageData);
			int count = this.mLanguages.Count;
			int i = 0;
			int count2 = this.mTerms.Count;
			while (i < count2)
			{
				Array.Resize<string>(ref this.mTerms[i].Languages, count);
				Array.Resize<byte>(ref this.mTerms[i].Flags, count);
				i++;
			}
			this.Editor_SetDirty();
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x0005E7E8 File Offset: 0x0005C9E8
		public void RemoveLanguage(string LanguageName)
		{
			int languageIndex = this.GetLanguageIndex(LanguageName, false, false);
			if (languageIndex < 0)
			{
				return;
			}
			int count = this.mLanguages.Count;
			int i = 0;
			int count2 = this.mTerms.Count;
			while (i < count2)
			{
				for (int j = languageIndex + 1; j < count; j++)
				{
					this.mTerms[i].Languages[j - 1] = this.mTerms[i].Languages[j];
					this.mTerms[i].Flags[j - 1] = this.mTerms[i].Flags[j];
				}
				Array.Resize<string>(ref this.mTerms[i].Languages, count - 1);
				Array.Resize<byte>(ref this.mTerms[i].Flags, count - 1);
				i++;
			}
			this.mLanguages.RemoveAt(languageIndex);
			this.Editor_SetDirty();
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0005E8D8 File Offset: 0x0005CAD8
		public List<string> GetLanguages(bool skipDisabled = true)
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (!skipDisabled || this.mLanguages[i].IsEnabled())
				{
					list.Add(this.mLanguages[i].Name);
				}
				i++;
			}
			return list;
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x0005E934 File Offset: 0x0005CB34
		public List<string> GetLanguagesCode(bool allowRegions = true, bool skipDisabled = true)
		{
			List<string> list = new List<string>();
			int i = 0;
			int count = this.mLanguages.Count;
			while (i < count)
			{
				if (!skipDisabled || this.mLanguages[i].IsEnabled())
				{
					string text = this.mLanguages[i].Code;
					if (!allowRegions && text != null && text.Length > 2)
					{
						text = text.Substring(0, 2);
					}
					if (!string.IsNullOrEmpty(text) && !list.Contains(text))
					{
						list.Add(text);
					}
				}
				i++;
			}
			return list;
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x0005E9B8 File Offset: 0x0005CBB8
		public bool IsLanguageEnabled(string Language)
		{
			int languageIndex = this.GetLanguageIndex(Language, false, true);
			return languageIndex >= 0 && this.mLanguages[languageIndex].IsEnabled();
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x0005E9E8 File Offset: 0x0005CBE8
		public void EnableLanguage(string Language, bool bEnabled)
		{
			int languageIndex = this.GetLanguageIndex(Language, false, false);
			if (languageIndex >= 0)
			{
				this.mLanguages[languageIndex].SetEnabled(bEnabled);
			}
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x0005EA15 File Offset: 0x0005CC15
		public bool AllowUnloadingLanguages()
		{
			return this._AllowUnloadingLanguages > LanguageSourceData.eAllowUnloadLanguages.Never;
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x0005EA20 File Offset: 0x0005CC20
		private string GetSavedLanguageFileName(int languageIndex)
		{
			if (languageIndex < 0)
			{
				return null;
			}
			return string.Concat(new string[]
			{
				"LangSource_",
				this.GetSourcePlayerPrefName(),
				"_",
				this.mLanguages[languageIndex].Name,
				".loc"
			});
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x0005EA74 File Offset: 0x0005CC74
		public void LoadLanguage(int languageIndex, bool UnloadOtherLanguages, bool useFallback, bool onlyCurrentSpecialization, bool forceLoad)
		{
			if (!this.AllowUnloadingLanguages())
			{
				return;
			}
			if (!PersistentStorage.CanAccessFiles())
			{
				return;
			}
			if (languageIndex >= 0 && (forceLoad || !this.mLanguages[languageIndex].IsLoaded()))
			{
				string savedLanguageFileName = this.GetSavedLanguageFileName(languageIndex);
				string text = PersistentStorage.LoadFile(PersistentStorage.eFileType.Temporal, savedLanguageFileName, false);
				if (!string.IsNullOrEmpty(text))
				{
					this.Import_Language_from_Cache(languageIndex, text, useFallback, onlyCurrentSpecialization);
					this.mLanguages[languageIndex].SetLoaded(true);
				}
			}
			if (UnloadOtherLanguages && I2Utils.IsPlaying())
			{
				for (int i = 0; i < this.mLanguages.Count; i++)
				{
					if (i != languageIndex)
					{
						this.UnloadLanguage(i);
					}
				}
			}
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0005EB10 File Offset: 0x0005CD10
		public void LoadAllLanguages(bool forceLoad = false)
		{
			for (int i = 0; i < this.mLanguages.Count; i++)
			{
				this.LoadLanguage(i, false, false, false, forceLoad);
			}
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0005EB40 File Offset: 0x0005CD40
		public void UnloadLanguage(int languageIndex)
		{
			if (!this.AllowUnloadingLanguages())
			{
				return;
			}
			if (!PersistentStorage.CanAccessFiles())
			{
				return;
			}
			if (!I2Utils.IsPlaying() || !this.mLanguages[languageIndex].IsLoaded() || !this.mLanguages[languageIndex].CanBeUnloaded() || this.IsCurrentLanguage(languageIndex) || !PersistentStorage.HasFile(PersistentStorage.eFileType.Temporal, this.GetSavedLanguageFileName(languageIndex), true))
			{
				return;
			}
			foreach (TermData termData in this.mTerms)
			{
				termData.Languages[languageIndex] = null;
			}
			this.mLanguages[languageIndex].SetLoaded(false);
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x0005EC00 File Offset: 0x0005CE00
		public void SaveLanguages(bool unloadAll, PersistentStorage.eFileType fileLocation = PersistentStorage.eFileType.Temporal)
		{
			if (!this.AllowUnloadingLanguages())
			{
				return;
			}
			if (!PersistentStorage.CanAccessFiles())
			{
				return;
			}
			for (int i = 0; i < this.mLanguages.Count; i++)
			{
				if (string.IsNullOrEmpty(this.mLanguages[i].Name))
				{
					Debug.LogError(string.Format("Language {0} has no name, please assign a name to the language or it may not show on a build", i));
				}
				else
				{
					string text = this.Export_Language_to_Cache(i, this.IsCurrentLanguage(i));
					if (!string.IsNullOrEmpty(text))
					{
						PersistentStorage.SaveFile(PersistentStorage.eFileType.Temporal, this.GetSavedLanguageFileName(i), text, true);
					}
				}
			}
			if (unloadAll)
			{
				for (int j = 0; j < this.mLanguages.Count; j++)
				{
					if (unloadAll && !this.IsCurrentLanguage(j))
					{
						this.UnloadLanguage(j);
					}
				}
			}
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0005ECB8 File Offset: 0x0005CEB8
		public bool HasUnloadedLanguages()
		{
			for (int i = 0; i < this.mLanguages.Count; i++)
			{
				if (!this.mLanguages[i].IsLoaded())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x0005ECF4 File Offset: 0x0005CEF4
		public List<string> GetCategories(bool OnlyMainCategory = false, List<string> Categories = null)
		{
			if (Categories == null)
			{
				Categories = new List<string>();
			}
			foreach (TermData termData in this.mTerms)
			{
				string categoryFromFullTerm = LanguageSourceData.GetCategoryFromFullTerm(termData.Term, OnlyMainCategory);
				if (!Categories.Contains(categoryFromFullTerm))
				{
					Categories.Add(categoryFromFullTerm);
				}
			}
			Categories.Sort();
			return Categories;
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x0005ED6C File Offset: 0x0005CF6C
		public static string GetKeyFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = (OnlyMainCategory ? FullTerm.IndexOfAny(LanguageSourceData.CategorySeparators) : FullTerm.LastIndexOfAny(LanguageSourceData.CategorySeparators));
			if (num >= 0)
			{
				return FullTerm.Substring(num + 1);
			}
			return FullTerm;
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x0005EDA4 File Offset: 0x0005CFA4
		public static string GetCategoryFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = (OnlyMainCategory ? FullTerm.IndexOfAny(LanguageSourceData.CategorySeparators) : FullTerm.LastIndexOfAny(LanguageSourceData.CategorySeparators));
			if (num >= 0)
			{
				return FullTerm.Substring(0, num);
			}
			return LanguageSourceData.EmptyCategory;
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0005EDE0 File Offset: 0x0005CFE0
		public static void DeserializeFullTerm(string FullTerm, out string Key, out string Category, bool OnlyMainCategory = false)
		{
			int num = (OnlyMainCategory ? FullTerm.IndexOfAny(LanguageSourceData.CategorySeparators) : FullTerm.LastIndexOfAny(LanguageSourceData.CategorySeparators));
			if (num < 0)
			{
				Category = LanguageSourceData.EmptyCategory;
				Key = FullTerm;
				return;
			}
			Category = FullTerm.Substring(0, num);
			Key = FullTerm.Substring(num + 1);
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0005EE30 File Offset: 0x0005D030
		public void UpdateDictionary(bool force = false)
		{
			if (!force && this.mDictionary != null && this.mDictionary.Count == this.mTerms.Count)
			{
				return;
			}
			StringComparer stringComparer = (this.CaseInsensitiveTerms ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
			if (this.mDictionary.Comparer != stringComparer)
			{
				this.mDictionary = new Dictionary<string, TermData>(stringComparer);
			}
			else
			{
				this.mDictionary.Clear();
			}
			int i = 0;
			int count = this.mTerms.Count;
			while (i < count)
			{
				TermData termData = this.mTerms[i];
				LanguageSourceData.ValidateFullTerm(ref termData.Term);
				this.mDictionary[termData.Term] = this.mTerms[i];
				this.mTerms[i].Validate();
				i++;
			}
			if (I2Utils.IsPlaying())
			{
				this.SaveLanguages(true, PersistentStorage.eFileType.Temporal);
			}
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x0005EF0C File Offset: 0x0005D10C
		public string GetTranslation(string term, string overrideLanguage = null, string overrideSpecialization = null, bool skipDisabled = false, bool allowCategoryMistmatch = false)
		{
			string text;
			this.TryGetTranslation(term, out text, overrideLanguage, overrideSpecialization, skipDisabled, allowCategoryMistmatch);
			return text;
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0005EF2C File Offset: 0x0005D12C
		public bool TryGetTranslation(string term, out string Translation, string overrideLanguage = null, string overrideSpecialization = null, bool skipDisabled = false, bool allowCategoryMistmatch = false)
		{
			int languageIndex = this.GetLanguageIndex((overrideLanguage == null) ? LocalizationManager.CurrentLanguage : overrideLanguage, true, false);
			if (languageIndex >= 0 && (!skipDisabled || this.mLanguages[languageIndex].IsEnabled()))
			{
				TermData termData = this.GetTermData(term, allowCategoryMistmatch);
				if (termData != null)
				{
					Translation = termData.GetTranslation(languageIndex, overrideSpecialization, true);
					if (Translation == "---")
					{
						Translation = string.Empty;
						return true;
					}
					if (!string.IsNullOrEmpty(Translation))
					{
						return true;
					}
					Translation = null;
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.ShowWarning)
				{
					Translation = "<!-Missing Translation [" + term + "]-!>";
					Debug.LogWarning("Missing Translation for '" + term + "'", Localize.CurrentLocalizeComponent);
					return false;
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.Fallback && termData != null)
				{
					return this.TryGetFallbackTranslation(termData, out Translation, languageIndex, overrideSpecialization, skipDisabled);
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.Empty)
				{
					Translation = string.Empty;
					return false;
				}
				if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.ShowTerm)
				{
					Translation = term;
					return false;
				}
			}
			Translation = null;
			return false;
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0005F020 File Offset: 0x0005D220
		private bool TryGetFallbackTranslation(TermData termData, out string Translation, int langIndex, string overrideSpecialization = null, bool skipDisabled = false)
		{
			string text = this.mLanguages[langIndex].Code;
			if (!string.IsNullOrEmpty(text))
			{
				if (text.Contains("-"))
				{
					text = text.Substring(0, text.IndexOf('-'));
				}
				for (int i = 0; i < this.mLanguages.Count; i++)
				{
					if (i != langIndex && this.mLanguages[i].Code.StartsWith(text, StringComparison.Ordinal) && (!skipDisabled || this.mLanguages[i].IsEnabled()))
					{
						Translation = termData.GetTranslation(i, overrideSpecialization, true);
						if (!string.IsNullOrEmpty(Translation))
						{
							return true;
						}
					}
				}
			}
			for (int j = 0; j < this.mLanguages.Count; j++)
			{
				if (j != langIndex && (!skipDisabled || this.mLanguages[j].IsEnabled()) && (text == null || !this.mLanguages[j].Code.StartsWith(text, StringComparison.Ordinal)))
				{
					Translation = termData.GetTranslation(j, overrideSpecialization, true);
					if (!string.IsNullOrEmpty(Translation))
					{
						return true;
					}
				}
			}
			Translation = null;
			return false;
		}

		// Token: 0x06000EC5 RID: 3781 RVA: 0x0005F12F File Offset: 0x0005D32F
		public TermData AddTerm(string term)
		{
			return this.AddTerm(term, eTermType.Text, true);
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x0005F13C File Offset: 0x0005D33C
		public TermData GetTermData(string term, bool allowCategoryMistmatch = false)
		{
			if (string.IsNullOrEmpty(term))
			{
				return null;
			}
			if (this.mDictionary.Count == 0)
			{
				this.UpdateDictionary(false);
			}
			TermData termData;
			if (this.mDictionary.TryGetValue(term, out termData))
			{
				return termData;
			}
			TermData termData2 = null;
			if (allowCategoryMistmatch)
			{
				string keyFromFullTerm = LanguageSourceData.GetKeyFromFullTerm(term, false);
				foreach (KeyValuePair<string, TermData> keyValuePair in this.mDictionary)
				{
					if (keyValuePair.Value.IsTerm(keyFromFullTerm, true))
					{
						if (termData2 != null)
						{
							return null;
						}
						termData2 = keyValuePair.Value;
					}
				}
				return termData2;
			}
			return termData2;
		}

		// Token: 0x06000EC7 RID: 3783 RVA: 0x0005F1EC File Offset: 0x0005D3EC
		public bool ContainsTerm(string term)
		{
			return this.GetTermData(term, false) != null;
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x0005F1FC File Offset: 0x0005D3FC
		public List<string> GetTermsList(string Category = null)
		{
			if (this.mDictionary.Count != this.mTerms.Count)
			{
				this.UpdateDictionary(false);
			}
			if (string.IsNullOrEmpty(Category))
			{
				return new List<string>(this.mDictionary.Keys);
			}
			List<string> list = new List<string>();
			for (int i = 0; i < this.mTerms.Count; i++)
			{
				TermData termData = this.mTerms[i];
				if (LanguageSourceData.GetCategoryFromFullTerm(termData.Term, false) == Category)
				{
					list.Add(termData.Term);
				}
			}
			return list;
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x0005F28C File Offset: 0x0005D48C
		public TermData AddTerm(string NewTerm, eTermType termType, bool SaveSource = true)
		{
			LanguageSourceData.ValidateFullTerm(ref NewTerm);
			NewTerm = NewTerm.Trim();
			if (this.mLanguages.Count == 0)
			{
				this.AddLanguage("English", "en");
			}
			TermData termData = this.GetTermData(NewTerm, false);
			if (termData == null)
			{
				termData = new TermData();
				termData.Term = NewTerm;
				termData.TermType = termType;
				termData.Languages = new string[this.mLanguages.Count];
				termData.Flags = new byte[this.mLanguages.Count];
				this.mTerms.Add(termData);
				this.mDictionary.Add(NewTerm, termData);
			}
			return termData;
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x0005F32C File Offset: 0x0005D52C
		public void RemoveTerm(string term)
		{
			int i = 0;
			int count = this.mTerms.Count;
			while (i < count)
			{
				if (this.mTerms[i].Term == term)
				{
					this.mTerms.RemoveAt(i);
					this.mDictionary.Remove(term);
					return;
				}
				i++;
			}
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x0005F384 File Offset: 0x0005D584
		public static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			Term = Term.Trim();
			if (Term.StartsWith(LanguageSourceData.EmptyCategory, StringComparison.Ordinal) && Term.Length > LanguageSourceData.EmptyCategory.Length && Term[LanguageSourceData.EmptyCategory.Length] == '/')
			{
				Term = Term.Substring(LanguageSourceData.EmptyCategory.Length + 1);
			}
			Term = I2Utils.GetValidTermName(Term, true);
		}

		[NonSerialized]
		public ILanguageSource owner;

		public bool UserAgreesToHaveItOnTheScene;

		public bool UserAgreesToHaveItInsideThePluginsFolder;

		public bool GoogleLiveSyncIsUptoDate = true;

		[NonSerialized]
		public bool mIsGlobalSource;

		public List<TermData> mTerms = new List<TermData>();

		public bool CaseInsensitiveTerms;

		[NonSerialized]
		public Dictionary<string, TermData> mDictionary = new Dictionary<string, TermData>(StringComparer.Ordinal);

		public LanguageSourceData.MissingTranslationAction OnMissingTranslation = LanguageSourceData.MissingTranslationAction.Fallback;

		public string mTerm_AppName;

		public List<LanguageData> mLanguages = new List<LanguageData>();

		public bool IgnoreDeviceLanguage;

		public LanguageSourceData.eAllowUnloadLanguages _AllowUnloadingLanguages;

		public string Google_WebServiceURL;

		public string Google_SpreadsheetKey;

		public string Google_SpreadsheetName;

		public string Google_LastUpdatedVersion;

		public LanguageSourceData.eGoogleUpdateFrequency GoogleUpdateFrequency = LanguageSourceData.eGoogleUpdateFrequency.Weekly;

		public LanguageSourceData.eGoogleUpdateFrequency GoogleInEditorCheckFrequency = LanguageSourceData.eGoogleUpdateFrequency.Daily;

		public LanguageSourceData.eGoogleUpdateSynchronization GoogleUpdateSynchronization = LanguageSourceData.eGoogleUpdateSynchronization.OnSceneLoaded;

		public float GoogleUpdateDelay;

		public List<Object> Assets = new List<Object>();

		[NonSerialized]
		public Dictionary<string, Object> mAssetDictionary = new Dictionary<string, Object>(StringComparer.Ordinal);

		private string mDelayedGoogleData;

		public static string EmptyCategory = "Default";

		public static char[] CategorySeparators = "/\\".ToCharArray();

		public enum MissingTranslationAction
		{
			Empty,
			Fallback,
			ShowWarning,
			ShowTerm
		}

		public enum eAllowUnloadLanguages
		{
			Never,
			OnlyInDevice,
			EditorAndDevice
		}

		public enum eGoogleUpdateFrequency
		{
			Always,
			Never,
			Daily,
			Weekly,
			Monthly,
			OnlyOnce,
			EveryOtherDay
		}

		public enum eGoogleUpdateSynchronization
		{
			Manual,
			OnSceneLoaded,
			AsSoonAsDownloaded
		}
	}
}
