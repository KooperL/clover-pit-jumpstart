using System;
using UnityEngine.Networking;

namespace I2.Loc
{
	public class TranslationJob_WWW : TranslationJob
	{
		// Token: 0x06000E6E RID: 3694 RVA: 0x0005C7A4 File Offset: 0x0005A9A4
		public override void Dispose()
		{
			if (this.www != null)
			{
				this.www.Dispose();
			}
			this.www = null;
		}

		public UnityWebRequest www;
	}
}
