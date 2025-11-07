using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;

namespace I2.Loc
{
	public class TranslationJob_GET : TranslationJob_WWW
	{
		// Token: 0x06000E59 RID: 3673 RVA: 0x0005BFEC File Offset: 0x0005A1EC
		public TranslationJob_GET(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			this.mQueries = GoogleTranslation.ConvertTranslationRequest(requests, true);
			this.GetState();
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0005C018 File Offset: 0x0005A218
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

		// Token: 0x06000E5B RID: 3675 RVA: 0x0005C08C File Offset: 0x0005A28C
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

		// Token: 0x06000E5C RID: 3676 RVA: 0x0005C0F8 File Offset: 0x0005A2F8
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

		private Dictionary<string, TranslationQuery> _requests;

		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;

		private List<string> mQueries;

		public string mErrorMessage;
	}
}
