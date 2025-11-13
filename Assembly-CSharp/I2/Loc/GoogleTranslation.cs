using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

namespace I2.Loc
{
	public static class GoogleTranslation
	{
		// Token: 0x06000E4F RID: 3663 RVA: 0x0005BA3B File Offset: 0x00059C3B
		public static bool CanTranslate()
		{
			return LocalizationManager.Sources.Count > 0 && !string.IsNullOrEmpty(LocalizationManager.GetWebServiceURL(null));
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0005BA5C File Offset: 0x00059C5C
		public static void Translate(string text, string LanguageCodeFrom, string LanguageCodeTo, GoogleTranslation.fnOnTranslated OnTranslationReady)
		{
			LocalizationManager.InitializeIfNeeded();
			if (!GoogleTranslation.CanTranslate())
			{
				OnTranslationReady(null, "WebService is not set correctly or needs to be reinstalled");
				return;
			}
			if (LanguageCodeTo == LanguageCodeFrom)
			{
				OnTranslationReady(text, null);
				return;
			}
			Dictionary<string, TranslationQuery> queries = new Dictionary<string, TranslationQuery>(StringComparer.Ordinal);
			if (string.IsNullOrEmpty(LanguageCodeTo))
			{
				OnTranslationReady(string.Empty, null);
				return;
			}
			GoogleTranslation.CreateQueries(text, LanguageCodeFrom, LanguageCodeTo, queries);
			GoogleTranslation.Translate(queries, delegate(Dictionary<string, TranslationQuery> results, string error)
			{
				if (!string.IsNullOrEmpty(error) || results.Count == 0)
				{
					OnTranslationReady(null, error);
					return;
				}
				string text2 = GoogleTranslation.RebuildTranslation(text, queries, LanguageCodeTo);
				OnTranslationReady(text2, null);
			}, true);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0005BB24 File Offset: 0x00059D24
		public static string ForceTranslate(string text, string LanguageCodeFrom, string LanguageCodeTo)
		{
			Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>(StringComparer.Ordinal);
			GoogleTranslation.AddQuery(text, LanguageCodeFrom, LanguageCodeTo, dictionary);
			TranslationJob_Main translationJob_Main = new TranslationJob_Main(dictionary, null);
			TranslationJob.eJobState state;
			do
			{
				state = translationJob_Main.GetState();
			}
			while (state == TranslationJob.eJobState.Running);
			if (state == TranslationJob.eJobState.Failed)
			{
				return null;
			}
			return GoogleTranslation.GetQueryResult(text, "", dictionary);
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0005BB69 File Offset: 0x00059D69
		public static void Translate(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady, bool usePOST = true)
		{
			GoogleTranslation.AddTranslationJob(new TranslationJob_Main(requests, OnTranslationReady));
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0005BB78 File Offset: 0x00059D78
		public static bool ForceTranslate(Dictionary<string, TranslationQuery> requests, bool usePOST = true)
		{
			TranslationJob_Main translationJob_Main = new TranslationJob_Main(requests, null);
			TranslationJob.eJobState state;
			do
			{
				state = translationJob_Main.GetState();
			}
			while (state == TranslationJob.eJobState.Running);
			return state != TranslationJob.eJobState.Failed;
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0005BBA0 File Offset: 0x00059DA0
		public static List<string> ConvertTranslationRequest(Dictionary<string, TranslationQuery> requests, bool encodeGET)
		{
			List<string> list = new List<string>();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, TranslationQuery> keyValuePair in requests)
			{
				TranslationQuery value = keyValuePair.Value;
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append("<I2Loc>");
				}
				stringBuilder.Append(GoogleLanguages.GetGoogleLanguageCode(value.LanguageCode));
				stringBuilder.Append(":");
				for (int i = 0; i < value.TargetLanguagesCode.Length; i++)
				{
					if (i != 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(GoogleLanguages.GetGoogleLanguageCode(value.TargetLanguagesCode[i]));
				}
				stringBuilder.Append("=");
				string text = ((GoogleTranslation.TitleCase(value.Text) == value.Text) ? value.Text.ToLowerInvariant() : value.Text);
				if (!encodeGET)
				{
					stringBuilder.Append(text);
				}
				else
				{
					stringBuilder.Append(Uri.EscapeDataString(text));
					if (stringBuilder.Length > 4000)
					{
						list.Add(stringBuilder.ToString());
						stringBuilder.Length = 0;
					}
				}
			}
			list.Add(stringBuilder.ToString());
			return list;
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x0005BD08 File Offset: 0x00059F08
		private static void AddTranslationJob(TranslationJob job)
		{
			GoogleTranslation.mTranslationJobs.Add(job);
			if (GoogleTranslation.mTranslationJobs.Count == 1)
			{
				CoroutineManager.Start(GoogleTranslation.WaitForTranslations());
			}
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0005BD2D File Offset: 0x00059F2D
		private static IEnumerator WaitForTranslations()
		{
			while (GoogleTranslation.mTranslationJobs.Count > 0)
			{
				foreach (TranslationJob translationJob in GoogleTranslation.mTranslationJobs.ToArray())
				{
					if (translationJob.GetState() != TranslationJob.eJobState.Running)
					{
						GoogleTranslation.mTranslationJobs.Remove(translationJob);
					}
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0005BD38 File Offset: 0x00059F38
		public static string ParseTranslationResult(string html, Dictionary<string, TranslationQuery> requests)
		{
			if (!html.StartsWith("<!DOCTYPE html>") && !html.StartsWith("<HTML>"))
			{
				string[] array = html.Split(new string[] { "<I2Loc>" }, StringSplitOptions.None);
				string[] array2 = new string[] { "<i2>" };
				int num = 0;
				foreach (string text in requests.Keys.ToArray<string>())
				{
					TranslationQuery translationQuery = GoogleTranslation.FindQueryFromOrigText(text, requests);
					string text2 = array[num++];
					if (translationQuery.Tags != null)
					{
						for (int j = translationQuery.Tags.Length - 1; j >= 0; j--)
						{
							text2 = text2.Replace(GoogleTranslation.GetGoogleNoTranslateTag(j), translationQuery.Tags[j]);
						}
					}
					translationQuery.Results = text2.Split(array2, StringSplitOptions.None);
					if (GoogleTranslation.TitleCase(text) == text)
					{
						for (int k = 0; k < translationQuery.Results.Length; k++)
						{
							translationQuery.Results[k] = GoogleTranslation.TitleCase(translationQuery.Results[k]);
						}
					}
					requests[translationQuery.OrigText] = translationQuery;
				}
				return null;
			}
			if (html.Contains("The script completed but did not return anything"))
			{
				return "The current Google WebService is not supported.\nPlease, delete the WebService from the Google Drive and Install the latest version.";
			}
			if (html.Contains("Service invoked too many times in a short time"))
			{
				return "";
			}
			return "There was a problem contacting the WebService. Please try again later\n" + html;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0005BE93 File Offset: 0x0005A093
		public static bool IsTranslating()
		{
			return GoogleTranslation.mCurrentTranslations.Count > 0 || GoogleTranslation.mTranslationJobs.Count > 0;
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0005BEB4 File Offset: 0x0005A0B4
		public static void CancelCurrentGoogleTranslations()
		{
			GoogleTranslation.mCurrentTranslations.Clear();
			foreach (TranslationJob translationJob in GoogleTranslation.mTranslationJobs)
			{
				translationJob.Dispose();
			}
			GoogleTranslation.mTranslationJobs.Clear();
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0005BF18 File Offset: 0x0005A118
		public static void CreateQueries(string text, string LanguageCodeFrom, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			if (!text.Contains("[i2s_"))
			{
				GoogleTranslation.CreateQueries_Plurals(text, LanguageCodeFrom, LanguageCodeTo, dict);
				return;
			}
			foreach (KeyValuePair<string, string> keyValuePair in SpecializationManager.GetSpecializations(text, null))
			{
				GoogleTranslation.CreateQueries_Plurals(keyValuePair.Value, LanguageCodeFrom, LanguageCodeTo, dict);
			}
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x0005BF8C File Offset: 0x0005A18C
		private static void CreateQueries_Plurals(string text, string LanguageCodeFrom, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			bool flag = text.Contains("{[#");
			bool flag2 = text.Contains("[i2p_");
			if (!GoogleTranslation.HasParameters(text) || (!flag && !flag2))
			{
				GoogleTranslation.AddQuery(text, LanguageCodeFrom, LanguageCodeTo, dict);
				return;
			}
			bool flag3 = flag;
			for (ePluralType ePluralType = ePluralType.Zero; ePluralType <= ePluralType.Plural; ePluralType++)
			{
				string text2 = ePluralType.ToString();
				if (GoogleLanguages.LanguageHasPluralType(LanguageCodeTo, text2))
				{
					string text3 = GoogleTranslation.GetPluralText(text, text2);
					int pluralTestNumber = GoogleLanguages.GetPluralTestNumber(LanguageCodeTo, ePluralType);
					string pluralParameter = GoogleTranslation.GetPluralParameter(text3, flag3);
					if (!string.IsNullOrEmpty(pluralParameter))
					{
						text3 = text3.Replace(pluralParameter, pluralTestNumber.ToString());
					}
					GoogleTranslation.AddQuery(text3, LanguageCodeFrom, LanguageCodeTo, dict);
				}
			}
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0005C034 File Offset: 0x0005A234
		public static void AddQuery(string text, string LanguageCodeFrom, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (!dict.ContainsKey(text))
			{
				TranslationQuery translationQuery = new TranslationQuery
				{
					OrigText = text,
					LanguageCode = LanguageCodeFrom,
					TargetLanguagesCode = new string[] { LanguageCodeTo }
				};
				translationQuery.Text = text;
				GoogleTranslation.ParseNonTranslatableElements(ref translationQuery);
				dict[text] = translationQuery;
				return;
			}
			TranslationQuery translationQuery2 = dict[text];
			if (Array.IndexOf<string>(translationQuery2.TargetLanguagesCode, LanguageCodeTo) < 0)
			{
				translationQuery2.TargetLanguagesCode = translationQuery2.TargetLanguagesCode.Concat(new string[] { LanguageCodeTo }).Distinct<string>().ToArray<string>();
			}
			dict[text] = translationQuery2;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0005C0DC File Offset: 0x0005A2DC
		private static string GetTranslation(string text, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			if (!dict.ContainsKey(text))
			{
				return null;
			}
			TranslationQuery translationQuery = dict[text];
			int num = Array.IndexOf<string>(translationQuery.TargetLanguagesCode, LanguageCodeTo);
			if (num < 0)
			{
				return "";
			}
			if (translationQuery.Results == null)
			{
				return "";
			}
			return translationQuery.Results[num];
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0005C12C File Offset: 0x0005A32C
		private static TranslationQuery FindQueryFromOrigText(string origText, Dictionary<string, TranslationQuery> dict)
		{
			foreach (KeyValuePair<string, TranslationQuery> keyValuePair in dict)
			{
				if (keyValuePair.Value.OrigText == origText)
				{
					return keyValuePair.Value;
				}
			}
			return default(TranslationQuery);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0005C19C File Offset: 0x0005A39C
		public static bool HasParameters(string text)
		{
			int num = text.IndexOf("{[", StringComparison.Ordinal);
			return num >= 0 && text.IndexOf("]}", num, StringComparison.Ordinal) > 0;
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x0005C1CC File Offset: 0x0005A3CC
		public static string GetPluralParameter(string text, bool forceTag)
		{
			int num = text.IndexOf("{[#", StringComparison.Ordinal);
			if (num < 0)
			{
				if (forceTag)
				{
					return null;
				}
				num = text.IndexOf("{[", StringComparison.Ordinal);
			}
			if (num < 0)
			{
				return null;
			}
			int num2 = text.IndexOf("]}", num + 2, StringComparison.Ordinal);
			if (num2 < 0)
			{
				return null;
			}
			return text.Substring(num, num2 - num + 2);
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0005C224 File Offset: 0x0005A424
		public static string GetPluralText(string text, string pluralType)
		{
			pluralType = "[i2p_" + pluralType + "]";
			int num = text.IndexOf(pluralType, StringComparison.Ordinal);
			if (num >= 0)
			{
				num += pluralType.Length;
				int num2 = text.IndexOf("[i2p_", num, StringComparison.Ordinal);
				if (num2 < 0)
				{
					num2 = text.Length;
				}
				return text.Substring(num, num2 - num);
			}
			num = text.IndexOf("[i2p_", StringComparison.Ordinal);
			if (num < 0)
			{
				return text;
			}
			if (num > 0)
			{
				return text.Substring(0, num);
			}
			num = text.IndexOf("]", StringComparison.Ordinal);
			if (num < 0)
			{
				return text;
			}
			num++;
			int num3 = text.IndexOf("[i2p_", num, StringComparison.Ordinal);
			if (num3 < 0)
			{
				num3 = text.Length;
			}
			return text.Substring(num, num3 - num);
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0005C2D8 File Offset: 0x0005A4D8
		private static int FindClosingTag(string tag, MatchCollection matches, int startIndex)
		{
			int i = startIndex;
			int count = matches.Count;
			while (i < count)
			{
				string captureMatch = I2Utils.GetCaptureMatch(matches[i]);
				if (captureMatch[0] == '/' && tag.StartsWith(captureMatch.Substring(1), StringComparison.Ordinal))
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		// Token: 0x06000E63 RID: 3683 RVA: 0x0005C324 File Offset: 0x0005A524
		private static string GetGoogleNoTranslateTag(int tagNumber)
		{
			if (tagNumber < 70)
			{
				return "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++".Substring(0, tagNumber + 1);
			}
			string text = "";
			for (int i = -1; i < tagNumber; i++)
			{
				text += "+";
			}
			return text;
		}

		// Token: 0x06000E64 RID: 3684 RVA: 0x0005C364 File Offset: 0x0005A564
		private static void ParseNonTranslatableElements(ref TranslationQuery query)
		{
			MatchCollection matchCollection = Regex.Matches(query.Text, "\\{\\[(.*?)]}|\\[(.*?)]|\\<(.*?)>");
			if (matchCollection == null || matchCollection.Count == 0)
			{
				return;
			}
			string text = query.Text;
			List<string> list = new List<string>();
			int i = 0;
			int count = matchCollection.Count;
			while (i < count)
			{
				string captureMatch = I2Utils.GetCaptureMatch(matchCollection[i]);
				int num = GoogleTranslation.FindClosingTag(captureMatch, matchCollection, i);
				if (num < 0)
				{
					string text2 = matchCollection[i].ToString();
					if (text2.StartsWith("{[", StringComparison.Ordinal) && text2.EndsWith("]}", StringComparison.Ordinal))
					{
						text = text.Replace(text2, GoogleTranslation.GetGoogleNoTranslateTag(list.Count) + " ");
						list.Add(text2);
					}
				}
				else if (captureMatch == "i2nt")
				{
					string text3 = query.Text.Substring(matchCollection[i].Index, matchCollection[num].Index - matchCollection[i].Index + matchCollection[num].Length);
					text = text.Replace(text3, GoogleTranslation.GetGoogleNoTranslateTag(list.Count) + " ");
					list.Add(text3);
				}
				else
				{
					string text4 = matchCollection[i].ToString();
					text = text.Replace(text4, GoogleTranslation.GetGoogleNoTranslateTag(list.Count) + " ");
					list.Add(text4);
					string text5 = matchCollection[num].ToString();
					text = text.Replace(text5, GoogleTranslation.GetGoogleNoTranslateTag(list.Count) + " ");
					list.Add(text5);
				}
				i++;
			}
			query.Text = text;
			query.Tags = list.ToArray();
		}

		// Token: 0x06000E65 RID: 3685 RVA: 0x0005C524 File Offset: 0x0005A724
		public static string GetQueryResult(string text, string LanguageCodeTo, Dictionary<string, TranslationQuery> dict)
		{
			if (!dict.ContainsKey(text))
			{
				return null;
			}
			TranslationQuery translationQuery = dict[text];
			if (translationQuery.Results == null || translationQuery.Results.Length < 0)
			{
				return null;
			}
			if (string.IsNullOrEmpty(LanguageCodeTo))
			{
				return translationQuery.Results[0];
			}
			int num = Array.IndexOf<string>(translationQuery.TargetLanguagesCode, LanguageCodeTo);
			if (num < 0)
			{
				return null;
			}
			return translationQuery.Results[num];
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0005C588 File Offset: 0x0005A788
		public static string RebuildTranslation(string text, Dictionary<string, TranslationQuery> dict, string LanguageCodeTo)
		{
			if (!text.Contains("[i2s_"))
			{
				return GoogleTranslation.RebuildTranslation_Plural(text, dict, LanguageCodeTo);
			}
			Dictionary<string, string> specializations = SpecializationManager.GetSpecializations(text, null);
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.Ordinal);
			foreach (KeyValuePair<string, string> keyValuePair in specializations)
			{
				dictionary[keyValuePair.Key] = GoogleTranslation.RebuildTranslation_Plural(keyValuePair.Value, dict, LanguageCodeTo);
			}
			return SpecializationManager.SetSpecializedText(dictionary);
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0005C618 File Offset: 0x0005A818
		private static string RebuildTranslation_Plural(string text, Dictionary<string, TranslationQuery> dict, string LanguageCodeTo)
		{
			bool flag = text.Contains("{[#");
			bool flag2 = text.Contains("[i2p_");
			if (!GoogleTranslation.HasParameters(text) || (!flag && !flag2))
			{
				return GoogleTranslation.GetTranslation(text, LanguageCodeTo, dict);
			}
			StringBuilder stringBuilder = new StringBuilder();
			string text2 = null;
			bool flag3 = flag;
			for (ePluralType ePluralType = ePluralType.Plural; ePluralType >= ePluralType.Zero; ePluralType--)
			{
				string text3 = ePluralType.ToString();
				if (GoogleLanguages.LanguageHasPluralType(LanguageCodeTo, text3))
				{
					string text4 = GoogleTranslation.GetPluralText(text, text3);
					int pluralTestNumber = GoogleLanguages.GetPluralTestNumber(LanguageCodeTo, ePluralType);
					string pluralParameter = GoogleTranslation.GetPluralParameter(text4, flag3);
					if (!string.IsNullOrEmpty(pluralParameter))
					{
						text4 = text4.Replace(pluralParameter, pluralTestNumber.ToString());
					}
					string text5 = GoogleTranslation.GetTranslation(text4, LanguageCodeTo, dict);
					if (!string.IsNullOrEmpty(pluralParameter))
					{
						text5 = text5.Replace(pluralTestNumber.ToString(), pluralParameter);
					}
					if (ePluralType == ePluralType.Plural)
					{
						text2 = text5;
					}
					else
					{
						if (text5 == text2)
						{
							goto IL_00E9;
						}
						stringBuilder.AppendFormat("[i2p_{0}]", text3);
					}
					stringBuilder.Append(text5);
				}
				IL_00E9:;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0005C724 File Offset: 0x0005A924
		public static string UppercaseFirst(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			char[] array = s.ToLower().ToCharArray();
			array[0] = char.ToUpper(array[0]);
			return new string(array);
		}

		// Token: 0x06000E69 RID: 3689 RVA: 0x0005C75C File Offset: 0x0005A95C
		public static string TitleCase(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
		}

		private static List<UnityWebRequest> mCurrentTranslations = new List<UnityWebRequest>();

		private static List<TranslationJob> mTranslationJobs = new List<TranslationJob>();

		// (Invoke) Token: 0x06001459 RID: 5209
		public delegate void fnOnTranslated(string Translation, string Error);

		// (Invoke) Token: 0x0600145D RID: 5213
		public delegate void fnOnTranslationReady(Dictionary<string, TranslationQuery> dict, string error);
	}
}
