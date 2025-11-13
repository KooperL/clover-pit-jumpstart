using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace I2.Loc
{
	public class TranslationJob_WEB : TranslationJob_WWW
	{
		// Token: 0x06000E7A RID: 3706 RVA: 0x0005CBFA File Offset: 0x0005ADFA
		public TranslationJob_WEB(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			this.FindAllQueries();
			this.ExecuteNextBatch();
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0005CC1C File Offset: 0x0005AE1C
		private void FindAllQueries()
		{
			this.mQueries = new List<KeyValuePair<string, string>>();
			foreach (KeyValuePair<string, TranslationQuery> keyValuePair in this._requests)
			{
				foreach (string text in keyValuePair.Value.TargetLanguagesCode)
				{
					this.mQueries.Add(new KeyValuePair<string, string>(keyValuePair.Value.OrigText, keyValuePair.Value.LanguageCode + ":" + text));
				}
			}
			this.mQueries.Sort((KeyValuePair<string, string> a, KeyValuePair<string, string> b) => a.Value.CompareTo(b.Value));
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x0005CCF4 File Offset: 0x0005AEF4
		private void ExecuteNextBatch()
		{
			if (this.mQueries.Count == 0)
			{
				this.mJobState = TranslationJob.eJobState.Succeeded;
				return;
			}
			this.mCurrentBatch_Text = new List<string>();
			string text = null;
			int num = 200;
			StringBuilder stringBuilder = new StringBuilder();
			int i;
			for (i = 0; i < this.mQueries.Count; i++)
			{
				string key = this.mQueries[i].Key;
				string value = this.mQueries[i].Value;
				if (text == null || value == text)
				{
					if (i != 0)
					{
						stringBuilder.Append("|||");
					}
					stringBuilder.Append(key);
					this.mCurrentBatch_Text.Add(key);
					text = value;
				}
				if (stringBuilder.Length > num)
				{
					break;
				}
			}
			this.mQueries.RemoveRange(0, i);
			string[] array = text.Split(':', StringSplitOptions.None);
			this.mCurrentBatch_FromLanguageCode = array[0];
			this.mCurrentBatch_ToLanguageCode = array[1];
			string text2 = string.Format("http://www.google.com/translate_t?hl=en&vi=c&ie=UTF8&oe=UTF8&submit=Translate&langpair={0}|{1}&text={2}", this.mCurrentBatch_FromLanguageCode, this.mCurrentBatch_ToLanguageCode, Uri.EscapeUriString(stringBuilder.ToString()));
			Debug.Log(text2);
			this.www = UnityWebRequest.Get(text2);
			I2Utils.SendWebRequest(this.www);
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0005CE20 File Offset: 0x0005B020
		public override TranslationJob.eJobState GetState()
		{
			if (this.www != null && this.www.isDone)
			{
				this.ProcessResult(this.www.downloadHandler.data, this.www.error);
				this.www.Dispose();
				this.www = null;
			}
			if (this.www == null)
			{
				this.ExecuteNextBatch();
			}
			return this.mJobState;
		}

		// Token: 0x06000E7E RID: 3710 RVA: 0x0005CE8C File Offset: 0x0005B08C
		public void ProcessResult(byte[] bytes, string errorMsg)
		{
			if (string.IsNullOrEmpty(errorMsg))
			{
				string @string = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
				Debug.Log(this.ParseTranslationResult(@string, "aab"));
				if (string.IsNullOrEmpty(errorMsg))
				{
					if (this._OnTranslationReady != null)
					{
						this._OnTranslationReady(this._requests, null);
					}
					return;
				}
			}
			this.mJobState = TranslationJob.eJobState.Failed;
			this.mErrorMessage = errorMsg;
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x0005CEF4 File Offset: 0x0005B0F4
		private string ParseTranslationResult(string html, string OriginalText)
		{
			string text2;
			try
			{
				int num = html.IndexOf("TRANSLATED_TEXT='", StringComparison.Ordinal) + "TRANSLATED_TEXT='".Length;
				int num2 = html.IndexOf("';var", num, StringComparison.Ordinal);
				string text = html.Substring(num, num2 - num);
				text = Regex.Replace(text, "\\\\x([a-fA-F0-9]{2})", (Match match) => char.ConvertFromUtf32(int.Parse(match.Groups[1].Value, NumberStyles.HexNumber)));
				text = Regex.Replace(text, "&#(\\d+);", (Match match) => char.ConvertFromUtf32(int.Parse(match.Groups[1].Value)));
				text = text.Replace("<br>", "\n");
				if (OriginalText.ToUpper() == OriginalText)
				{
					text = text.ToUpper();
				}
				else if (GoogleTranslation.UppercaseFirst(OriginalText) == OriginalText)
				{
					text = GoogleTranslation.UppercaseFirst(text);
				}
				else if (GoogleTranslation.TitleCase(OriginalText) == OriginalText)
				{
					text = GoogleTranslation.TitleCase(text);
				}
				text2 = text;
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
				text2 = string.Empty;
			}
			return text2;
		}

		private Dictionary<string, TranslationQuery> _requests;

		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;

		public string mErrorMessage;

		private string mCurrentBatch_ToLanguageCode;

		private string mCurrentBatch_FromLanguageCode;

		private List<string> mCurrentBatch_Text;

		private List<KeyValuePair<string, string>> mQueries;
	}
}
