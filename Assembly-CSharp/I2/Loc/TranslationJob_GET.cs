using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace I2.Loc
{
	// Token: 0x0200019E RID: 414
	public class TranslationJob_GET : TranslationJob_WWW
	{
		// Token: 0x0600121C RID: 4636 RVA: 0x000149F1 File Offset: 0x00012BF1
		public TranslationJob_GET(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			this.mQueries = GoogleTranslation.ConvertTranslationRequest(requests, true);
			this.GetState();
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x0007AF50 File Offset: 0x00079150
		private void ExecuteNextQuery()
		{
			if (this.mQueries.Count == 0)
			{
				this.mJobState = TranslationJob.eJobState.Succeeded;
				return;
			}
			int num = this.mQueries.Count - 1;
			string text = this.mQueries[num];
			this.mQueries.RemoveAt(num);
			string text2 = LocalizationManager.GetWebServiceURL(null) + "?action=Translate&list=" + text;
			this.www = UnityWebRequest.Get(text2);
			I2Utils.SendWebRequest(this.www);
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0007AFC4 File Offset: 0x000791C4
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
				this.ExecuteNextQuery();
			}
			return this.mJobState;
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0007B030 File Offset: 0x00079230
		public void ProcessResult(byte[] bytes, string errorMsg)
		{
			if (string.IsNullOrEmpty(errorMsg))
			{
				errorMsg = GoogleTranslation.ParseTranslationResult(Encoding.UTF8.GetString(bytes, 0, bytes.Length), this._requests);
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

		// Token: 0x040012CE RID: 4814
		private Dictionary<string, TranslationQuery> _requests;

		// Token: 0x040012CF RID: 4815
		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;

		// Token: 0x040012D0 RID: 4816
		private List<string> mQueries;

		// Token: 0x040012D1 RID: 4817
		public string mErrorMessage;
	}
}
