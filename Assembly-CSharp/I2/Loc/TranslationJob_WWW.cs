using System;
using UnityEngine.Networking;

namespace I2.Loc
{
	// Token: 0x0200019D RID: 413
	public class TranslationJob_WWW : TranslationJob
	{
		// Token: 0x0600121A RID: 4634 RVA: 0x000149CD File Offset: 0x00012BCD
		public override void Dispose()
		{
			if (this.www != null)
			{
				this.www.Dispose();
			}
			this.www = null;
		}

		// Token: 0x040012CD RID: 4813
		public UnityWebRequest www;
	}
}
