using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	// Token: 0x020001A9 RID: 425
	[ExecuteInEditMode]
	[Serializable]
	public class LanguageSourceData
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600124B RID: 4683 RVA: 0x00014BFF File Offset: 0x00012DFF
		public global::UnityEngine.Object ownerObject
		{
			get
			{
				return this.owner as global::UnityEngine.Object;
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600124C RID: 4684 RVA: 0x0007BA38 File Offset: 0x00079C38
		// (remove) Token: 0x0600124D RID: 4685 RVA: 0x0007BA70 File Offset: 0x00079C70
		public event LanguageSource.fnOnSourceUpdated Event_OnSourceUpdateFromGoogle;

		// Token: 0x0600124E RID: 4686 RVA: 0x00014C0C File Offset: 0x00012E0C
		public void Awake()
		{
			LocalizationManager.AddSource(this);
			this.UpdateDictionary(false);
			this.UpdateAssetDictionary();
			LocalizationManager.LocalizeAll(true);
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00014C27 File Offset: 0x00012E27
		public void OnDestroy()
		{
			LocalizationManager.RemoveSource(this);
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x0007BAA8 File Offset: 0x00079CA8
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

		// Token: 0x06001251 RID: 4689 RVA: 0x0007BB54 File Offset: 0x00079D54
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

		// Token: 0x06001252 RID: 4690 RVA: 0x00014C2F File Offset: 0x00012E2F
		public void ClearAllData()
		{
			this.mTerms.Clear();
			this.mLanguages.Clear();
			this.mDictionary.Clear();
			this.mAssetDictionary.Clear();
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x00014C5D File Offset: 0x00012E5D
		public bool IsGlobalSource()
		{
			return this.mIsGlobalSource;
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x0000774E File Offset: 0x0000594E
		public void Editor_SetDirty()
		{
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x0007BB98 File Offset: 0x00079D98
		public void UpdateAssetDictionary()
		{
			this.Assets.RemoveAll((global::UnityEngine.Object x) => x == null);
			this.mAssetDictionary = this.Assets.Distinct<global::UnityEngine.Object>().GroupBy((global::UnityEngine.Object o) => o.name, StringComparer.Ordinal).ToDictionary((IGrouping<string, global::UnityEngine.Object> g) => g.Key, (IGrouping<string, global::UnityEngine.Object> g) => g.First<global::UnityEngine.Object>(), StringComparer.Ordinal);
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x0007BC54 File Offset: 0x00079E54
		public global::UnityEngine.Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				if (this.mAssetDictionary == null || this.mAssetDictionary.Count != this.Assets.Count)
				{
					this.UpdateAssetDictionary();
				}
				global::UnityEngine.Object @object;
				if (this.mAssetDictionary.TryGetValue(Name, out @object))
				{
					return @object;
				}
			}
			return null;
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x00014C65 File Offset: 0x00012E65
		public bool HasAsset(global::UnityEngine.Object Obj)
		{
			return this.Assets.Contains(Obj);
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00014C73 File Offset: 0x00012E73
		public void AddAsset(global::UnityEngine.Object Obj)
		{
			if (this.Assets.Contains(Obj))
			{
				return;
			}
			this.Assets.Add(Obj);
			this.UpdateAssetDictionary();
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x0007BCA4 File Offset: 0x00079EA4
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

		// Token: 0x0600125A RID: 4698 RVA: 0x0007BD74 File Offset: 0x00079F74
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

		// Token: 0x0600125B RID: 4699 RVA: 0x0007BF98 File Offset: 0x0007A198
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

		// Token: 0x0600125C RID: 4700 RVA: 0x00014C96 File Offset: 0x00012E96
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

		// Token: 0x0600125D RID: 4701 RVA: 0x0007C06C File Offset: 0x0007A26C
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

		// Token: 0x0600125E RID: 4702 RVA: 0x0007C260 File Offset: 0x0007A460
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

		// Token: 0x0600125F RID: 4703 RVA: 0x0007C328 File Offset: 0x0007A528
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

		// Token: 0x06001260 RID: 4704 RVA: 0x0007C398 File Offset: 0x0007A598
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

		// Token: 0x06001261 RID: 4705 RVA: 0x0007C410 File Offset: 0x0007A610
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

		// Token: 0x06001262 RID: 4706 RVA: 0x0007C484 File Offset: 0x0007A684
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

		// Token: 0x06001263 RID: 4707 RVA: 0x0007C528 File Offset: 0x0007A728
		public string Import_CSV(string Category, string CSVstring, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace, char Separator = ',')
		{
			List<string[]> list = LocalizationReader.ReadCSV(CSVstring, Separator);
			return this.Import_CSV(Category, list, UpdateMode);
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x0007C548 File Offset: 0x0007A748
		public string Import_I2CSV(string Category, string I2CSVstring, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
		{
			List<string[]> list = LocalizationReader.ReadI2CSV(I2CSVstring);
			return this.Import_CSV(Category, list, UpdateMode);
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x0007C568 File Offset: 0x0007A768
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

		// Token: 0x06001266 RID: 4710 RVA: 0x0007C980 File Offset: 0x0007AB80
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

		// Token: 0x06001267 RID: 4711 RVA: 0x0007C9B0 File Offset: 0x0007ABB0
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

		// Token: 0x06001268 RID: 4712 RVA: 0x0007C9E8 File Offset: 0x0007ABE8
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

		// Token: 0x06001269 RID: 4713 RVA: 0x0007CAA4 File Offset: 0x0007ACA4
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

		// Token: 0x0600126A RID: 4714 RVA: 0x0007CB14 File Offset: 0x0007AD14
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

		// Token: 0x0600126B RID: 4715 RVA: 0x0007CC0C File Offset: 0x0007AE0C
		private bool IsNewerVersion(string currentVersion, string newVersion)
		{
			long num;
			long num2;
			return !string.IsNullOrEmpty(newVersion) && (string.IsNullOrEmpty(currentVersion) || (!long.TryParse(newVersion, out num) || !long.TryParse(currentVersion, out num2)) || num > num2);
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0007CC48 File Offset: 0x0007AE48
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

		// Token: 0x0600126D RID: 4717 RVA: 0x0007CD58 File Offset: 0x0007AF58
		private string GetSourcePlayerPrefName()
		{
			if (this.owner == null)
			{
				return null;
			}
			string text = (this.owner as global::UnityEngine.Object).name;
			if (!string.IsNullOrEmpty(this.Google_SpreadsheetKey))
			{
				text += this.Google_SpreadsheetKey;
			}
			if (Array.IndexOf<string>(LocalizationManager.GlobalSources, (this.owner as global::UnityEngine.Object).name) >= 0)
			{
				return text;
			}
			return SceneManager.GetActiveScene().name + "_" + text;
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x00014CCE File Offset: 0x00012ECE
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
						SceneManager.sceneLoaded += this.ApplyDownloadedDataOnSceneLoaded;
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

		// Token: 0x0600126F RID: 4719 RVA: 0x00014CEB File Offset: 0x00012EEB
		private void ApplyDownloadedDataOnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			SceneManager.sceneLoaded -= this.ApplyDownloadedDataOnSceneLoaded;
			this.ApplyDownloadedDataFromGoogle();
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0007CDD4 File Offset: 0x0007AFD4
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

		// Token: 0x06001271 RID: 4721 RVA: 0x0007CE54 File Offset: 0x0007B054
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

		// Token: 0x06001272 RID: 4722 RVA: 0x00014D04 File Offset: 0x00012F04
		public bool HasGoogleSpreadsheet()
		{
			return !string.IsNullOrEmpty(this.Google_WebServiceURL) && !string.IsNullOrEmpty(this.Google_SpreadsheetKey) && !string.IsNullOrEmpty(LocalizationManager.GetWebServiceURL(this));
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0007CEE0 File Offset: 0x0007B0E0
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

		// Token: 0x06001274 RID: 4724 RVA: 0x0007D114 File Offset: 0x0007B314
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

		// Token: 0x06001275 RID: 4725 RVA: 0x0007D1D4 File Offset: 0x0007B3D4
		public LanguageData GetLanguageData(string language, bool AllowDiscartingRegion = true)
		{
			int languageIndex = this.GetLanguageIndex(language, AllowDiscartingRegion, false);
			if (languageIndex >= 0)
			{
				return this.mLanguages[languageIndex];
			}
			return null;
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x00014D30 File Offset: 0x00012F30
		public bool IsCurrentLanguage(int languageIndex)
		{
			return LocalizationManager.CurrentLanguage == this.mLanguages[languageIndex].Name;
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0007D200 File Offset: 0x0007B400
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

		// Token: 0x06001278 RID: 4728 RVA: 0x0007D2A8 File Offset: 0x0007B4A8
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

		// Token: 0x06001279 RID: 4729 RVA: 0x00014D4D File Offset: 0x00012F4D
		public static bool AreTheSameLanguage(string Language1, string Language2)
		{
			Language1 = LanguageSourceData.GetLanguageWithoutRegion(Language1);
			Language2 = LanguageSourceData.GetLanguageWithoutRegion(Language2);
			return string.Compare(Language1, Language2, StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x0007D358 File Offset: 0x0007B558
		public static string GetLanguageWithoutRegion(string Language)
		{
			int num = Language.IndexOfAny("(/\\[,{".ToCharArray());
			if (num < 0)
			{
				return Language;
			}
			return Language.Substring(0, num).Trim();
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x00014D6A File Offset: 0x00012F6A
		public void AddLanguage(string LanguageName)
		{
			this.AddLanguage(LanguageName, GoogleLanguages.GetLanguageCode(LanguageName, false));
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x0007D38C File Offset: 0x0007B58C
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

		// Token: 0x0600127D RID: 4733 RVA: 0x0007D420 File Offset: 0x0007B620
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

		// Token: 0x0600127E RID: 4734 RVA: 0x0007D510 File Offset: 0x0007B710
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

		// Token: 0x0600127F RID: 4735 RVA: 0x0007D56C File Offset: 0x0007B76C
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

		// Token: 0x06001280 RID: 4736 RVA: 0x0007D5F0 File Offset: 0x0007B7F0
		public bool IsLanguageEnabled(string Language)
		{
			int languageIndex = this.GetLanguageIndex(Language, false, true);
			return languageIndex >= 0 && this.mLanguages[languageIndex].IsEnabled();
		}

		// Token: 0x06001281 RID: 4737 RVA: 0x0007D620 File Offset: 0x0007B820
		public void EnableLanguage(string Language, bool bEnabled)
		{
			int languageIndex = this.GetLanguageIndex(Language, false, false);
			if (languageIndex >= 0)
			{
				this.mLanguages[languageIndex].SetEnabled(bEnabled);
			}
		}

		// Token: 0x06001282 RID: 4738 RVA: 0x00014D7A File Offset: 0x00012F7A
		public bool AllowUnloadingLanguages()
		{
			return this._AllowUnloadingLanguages > LanguageSourceData.eAllowUnloadLanguages.Never;
		}

		// Token: 0x06001283 RID: 4739 RVA: 0x0007D650 File Offset: 0x0007B850
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

		// Token: 0x06001284 RID: 4740 RVA: 0x0007D6A4 File Offset: 0x0007B8A4
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

		// Token: 0x06001285 RID: 4741 RVA: 0x0007D740 File Offset: 0x0007B940
		public void LoadAllLanguages(bool forceLoad = false)
		{
			for (int i = 0; i < this.mLanguages.Count; i++)
			{
				this.LoadLanguage(i, false, false, false, forceLoad);
			}
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x0007D770 File Offset: 0x0007B970
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

		// Token: 0x06001287 RID: 4743 RVA: 0x0007D830 File Offset: 0x0007BA30
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

		// Token: 0x06001288 RID: 4744 RVA: 0x0007D8E8 File Offset: 0x0007BAE8
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

		// Token: 0x06001289 RID: 4745 RVA: 0x0007D924 File Offset: 0x0007BB24
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

		// Token: 0x0600128A RID: 4746 RVA: 0x0007D99C File Offset: 0x0007BB9C
		public static string GetKeyFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = (OnlyMainCategory ? FullTerm.IndexOfAny(LanguageSourceData.CategorySeparators) : FullTerm.LastIndexOfAny(LanguageSourceData.CategorySeparators));
			if (num >= 0)
			{
				return FullTerm.Substring(num + 1);
			}
			return FullTerm;
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0007D9D4 File Offset: 0x0007BBD4
		public static string GetCategoryFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
		{
			int num = (OnlyMainCategory ? FullTerm.IndexOfAny(LanguageSourceData.CategorySeparators) : FullTerm.LastIndexOfAny(LanguageSourceData.CategorySeparators));
			if (num >= 0)
			{
				return FullTerm.Substring(0, num);
			}
			return LanguageSourceData.EmptyCategory;
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x0007DA10 File Offset: 0x0007BC10
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

		// Token: 0x0600128D RID: 4749 RVA: 0x0007DA60 File Offset: 0x0007BC60
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

		// Token: 0x0600128E RID: 4750 RVA: 0x0007DB3C File Offset: 0x0007BD3C
		public string GetTranslation(string term, string overrideLanguage = null, string overrideSpecialization = null, bool skipDisabled = false, bool allowCategoryMistmatch = false)
		{
			string text;
			this.TryGetTranslation(term, out text, overrideLanguage, overrideSpecialization, skipDisabled, allowCategoryMistmatch);
			return text;
		}

		// Token: 0x0600128F RID: 4751 RVA: 0x0007DB5C File Offset: 0x0007BD5C
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

		// Token: 0x06001290 RID: 4752 RVA: 0x0007DC50 File Offset: 0x0007BE50
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

		// Token: 0x06001291 RID: 4753 RVA: 0x00014D85 File Offset: 0x00012F85
		public TermData AddTerm(string term)
		{
			return this.AddTerm(term, eTermType.Text, true);
		}

		// Token: 0x06001292 RID: 4754 RVA: 0x0007DD60 File Offset: 0x0007BF60
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

		// Token: 0x06001293 RID: 4755 RVA: 0x00014D90 File Offset: 0x00012F90
		public bool ContainsTerm(string term)
		{
			return this.GetTermData(term, false) != null;
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0007DE10 File Offset: 0x0007C010
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

		// Token: 0x06001295 RID: 4757 RVA: 0x0007DEA0 File Offset: 0x0007C0A0
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

		// Token: 0x06001296 RID: 4758 RVA: 0x0007DF40 File Offset: 0x0007C140
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

		// Token: 0x06001297 RID: 4759 RVA: 0x0007DF98 File Offset: 0x0007C198
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

		// Token: 0x04001303 RID: 4867
		[NonSerialized]
		public ILanguageSource owner;

		// Token: 0x04001304 RID: 4868
		public bool UserAgreesToHaveItOnTheScene;

		// Token: 0x04001305 RID: 4869
		public bool UserAgreesToHaveItInsideThePluginsFolder;

		// Token: 0x04001306 RID: 4870
		public bool GoogleLiveSyncIsUptoDate = true;

		// Token: 0x04001307 RID: 4871
		[NonSerialized]
		public bool mIsGlobalSource;

		// Token: 0x04001308 RID: 4872
		public List<TermData> mTerms = new List<TermData>();

		// Token: 0x04001309 RID: 4873
		public bool CaseInsensitiveTerms;

		// Token: 0x0400130A RID: 4874
		[NonSerialized]
		public Dictionary<string, TermData> mDictionary = new Dictionary<string, TermData>(StringComparer.Ordinal);

		// Token: 0x0400130B RID: 4875
		public LanguageSourceData.MissingTranslationAction OnMissingTranslation = LanguageSourceData.MissingTranslationAction.Fallback;

		// Token: 0x0400130C RID: 4876
		public string mTerm_AppName;

		// Token: 0x0400130D RID: 4877
		public List<LanguageData> mLanguages = new List<LanguageData>();

		// Token: 0x0400130E RID: 4878
		public bool IgnoreDeviceLanguage;

		// Token: 0x0400130F RID: 4879
		public LanguageSourceData.eAllowUnloadLanguages _AllowUnloadingLanguages;

		// Token: 0x04001310 RID: 4880
		public string Google_WebServiceURL;

		// Token: 0x04001311 RID: 4881
		public string Google_SpreadsheetKey;

		// Token: 0x04001312 RID: 4882
		public string Google_SpreadsheetName;

		// Token: 0x04001313 RID: 4883
		public string Google_LastUpdatedVersion;

		// Token: 0x04001314 RID: 4884
		public LanguageSourceData.eGoogleUpdateFrequency GoogleUpdateFrequency = LanguageSourceData.eGoogleUpdateFrequency.Weekly;

		// Token: 0x04001315 RID: 4885
		public LanguageSourceData.eGoogleUpdateFrequency GoogleInEditorCheckFrequency = LanguageSourceData.eGoogleUpdateFrequency.Daily;

		// Token: 0x04001316 RID: 4886
		public LanguageSourceData.eGoogleUpdateSynchronization GoogleUpdateSynchronization = LanguageSourceData.eGoogleUpdateSynchronization.OnSceneLoaded;

		// Token: 0x04001317 RID: 4887
		public float GoogleUpdateDelay;

		// Token: 0x04001319 RID: 4889
		public List<global::UnityEngine.Object> Assets = new List<global::UnityEngine.Object>();

		// Token: 0x0400131A RID: 4890
		[NonSerialized]
		public Dictionary<string, global::UnityEngine.Object> mAssetDictionary = new Dictionary<string, global::UnityEngine.Object>(StringComparer.Ordinal);

		// Token: 0x0400131B RID: 4891
		private string mDelayedGoogleData;

		// Token: 0x0400131C RID: 4892
		public static string EmptyCategory = "Default";

		// Token: 0x0400131D RID: 4893
		public static char[] CategorySeparators = "/\\".ToCharArray();

		// Token: 0x020001AA RID: 426
		public enum MissingTranslationAction
		{
			// Token: 0x0400131F RID: 4895
			Empty,
			// Token: 0x04001320 RID: 4896
			Fallback,
			// Token: 0x04001321 RID: 4897
			ShowWarning,
			// Token: 0x04001322 RID: 4898
			ShowTerm
		}

		// Token: 0x020001AB RID: 427
		public enum eAllowUnloadLanguages
		{
			// Token: 0x04001324 RID: 4900
			Never,
			// Token: 0x04001325 RID: 4901
			OnlyInDevice,
			// Token: 0x04001326 RID: 4902
			EditorAndDevice
		}

		// Token: 0x020001AC RID: 428
		public enum eGoogleUpdateFrequency
		{
			// Token: 0x04001328 RID: 4904
			Always,
			// Token: 0x04001329 RID: 4905
			Never,
			// Token: 0x0400132A RID: 4906
			Daily,
			// Token: 0x0400132B RID: 4907
			Weekly,
			// Token: 0x0400132C RID: 4908
			Monthly,
			// Token: 0x0400132D RID: 4909
			OnlyOnce,
			// Token: 0x0400132E RID: 4910
			EveryOtherDay
		}

		// Token: 0x020001AD RID: 429
		public enum eGoogleUpdateSynchronization
		{
			// Token: 0x04001330 RID: 4912
			Manual,
			// Token: 0x04001331 RID: 4913
			OnSceneLoaded,
			// Token: 0x04001332 RID: 4914
			AsSoonAsDownloaded
		}
	}
}
