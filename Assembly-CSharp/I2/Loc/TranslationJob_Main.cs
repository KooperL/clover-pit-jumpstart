using System;
using System.Collections.Generic;

namespace I2.Loc
{
	// Token: 0x0200019F RID: 415
	public class TranslationJob_Main : TranslationJob
	{
		// Token: 0x06001220 RID: 4640 RVA: 0x00014A1B File Offset: 0x00012C1B
		public TranslationJob_Main(Dictionary<string, TranslationQuery> requests, GoogleTranslation.fnOnTranslationReady OnTranslationReady)
		{
			this._requests = requests;
			this._OnTranslationReady = OnTranslationReady;
			this.mPost = new TranslationJob_POST(requests, OnTranslationReady);
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0007B094 File Offset: 0x00079294
		public override TranslationJob.eJobState GetState()
		{
			if (this.mWeb != null)
			{
				switch (this.mWeb.GetState())
				{
				case TranslationJob.eJobState.Running:
					return TranslationJob.eJobState.Running;
				case TranslationJob.eJobState.Succeeded:
					this.mJobState = TranslationJob.eJobState.Succeeded;
					break;
				case TranslationJob.eJobState.Failed:
					this.mWeb.Dispose();
					this.mWeb = null;
					this.mPost = new TranslationJob_POST(this._requests, this._OnTranslationReady);
					break;
				}
			}
			if (this.mPost != null)
			{
				switch (this.mPost.GetState())
				{
				case TranslationJob.eJobState.Running:
					return TranslationJob.eJobState.Running;
				case TranslationJob.eJobState.Succeeded:
					this.mJobState = TranslationJob.eJobState.Succeeded;
					break;
				case TranslationJob.eJobState.Failed:
					this.mPost.Dispose();
					this.mPost = null;
					this.mGet = new TranslationJob_GET(this._requests, this._OnTranslationReady);
					break;
				}
			}
			if (this.mGet != null)
			{
				switch (this.mGet.GetState())
				{
				case TranslationJob.eJobState.Running:
					return TranslationJob.eJobState.Running;
				case TranslationJob.eJobState.Succeeded:
					this.mJobState = TranslationJob.eJobState.Succeeded;
					break;
				case TranslationJob.eJobState.Failed:
					this.mErrorMessage = this.mGet.mErrorMessage;
					if (this._OnTranslationReady != null)
					{
						this._OnTranslationReady(this._requests, this.mErrorMessage);
					}
					this.mGet.Dispose();
					this.mGet = null;
					break;
				}
			}
			return this.mJobState;
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x00014A3E File Offset: 0x00012C3E
		public override void Dispose()
		{
			if (this.mPost != null)
			{
				this.mPost.Dispose();
			}
			if (this.mGet != null)
			{
				this.mGet.Dispose();
			}
			this.mPost = null;
			this.mGet = null;
		}

		// Token: 0x040012D2 RID: 4818
		private TranslationJob_WEB mWeb;

		// Token: 0x040012D3 RID: 4819
		private TranslationJob_POST mPost;

		// Token: 0x040012D4 RID: 4820
		private TranslationJob_GET mGet;

		// Token: 0x040012D5 RID: 4821
		private Dictionary<string, TranslationQuery> _requests;

		// Token: 0x040012D6 RID: 4822
		private GoogleTranslation.fnOnTranslationReady _OnTranslationReady;

		// Token: 0x040012D7 RID: 4823
		public string mErrorMessage;
	}
}
