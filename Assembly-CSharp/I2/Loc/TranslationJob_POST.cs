using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace I2.Loc
{
	public class TranslationJob_POST : TranslationJob_WWW
	{
		// Token: 0x06000E60 RID: 3680 RVA: 0x0005C2F8 File Offset: 0x0005A4F8
		public TranslationJob_POST(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			List<string> list = GoogleTranslation.ConvertTranslationRequest(requests, false);
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("action", "Translate");
			wwwform.AddField("list", list[0]);
			this.www = UnityWebRequest.Post(LocalizationManager.GetWebServiceURL(null), wwwform);
			I2Utils.SendWebRequest(this.www);
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0005C368 File Offset: 0x0005A568
		public override TranslationJob.eJobState GetState()
		{
			if (this.www != null && this.www.isDone)
			{
				this.ProcessResult(this.www.downloadHandler.data, this.www.error);
				this.www.Dispose();
				this.www = null;
			}
			return this.mJobState;
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0005C3C4 File Offset: 0x0005A5C4
		public void ProcessResult(byte[] bytes, string errorMsg)
		{
			if (!string.IsNullOrEmpty(errorMsg))
			{
				this.mJobState = TranslationJob.eJobState.Failed;
				return;
			}
			errorMsg = GoogleTranslation.ParseTranslationResult(Encoding.UTF8.GetString(bytes, 0, bytes.Length), this._requests);
			if (this._OnTranslationReady != null)
			{
				this._OnTranslationReady(this._requests, errorMsg);
			}
			this.mJobState = TranslationJob.eJobState.Succeeded;
		}

		private Dictionary<string, TranslationQuery> _requests;

		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;
	}
}
