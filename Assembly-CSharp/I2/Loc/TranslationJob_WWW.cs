using System;
using UnityEngine.Networking;

namespace I2.Loc
{
	public class TranslationJob_WWW : TranslationJob
	{
		// Token: 0x06000E57 RID: 3671 RVA: 0x0005BFC8 File Offset: 0x0005A1C8
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
