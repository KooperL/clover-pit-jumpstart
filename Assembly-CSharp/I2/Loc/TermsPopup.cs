using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001D8 RID: 472
	public class TermsPopup : PropertyAttribute
	{
		// Token: 0x060013E7 RID: 5095 RVA: 0x00015685 File Offset: 0x00013885
		public TermsPopup(string filter = "")
		{
			this.Filter = filter;
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060013E8 RID: 5096 RVA: 0x00015694 File Offset: 0x00013894
		// (set) Token: 0x060013E9 RID: 5097 RVA: 0x0001569C File Offset: 0x0001389C
		public string Filter { get; private set; }
	}
}
