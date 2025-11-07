using System;
using UnityEngine;

namespace I2.Loc
{
	public class TermsPopup : PropertyAttribute
	{
		// Token: 0x06000FE2 RID: 4066 RVA: 0x00063512 File Offset: 0x00061712
		public TermsPopup(string filter = "")
		{
			this.Filter = filter;
		}

		// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x00063521 File Offset: 0x00061721
		// (set) Token: 0x06000FE4 RID: 4068 RVA: 0x00063529 File Offset: 0x00061729
		public string Filter { get; private set; }
	}
}
