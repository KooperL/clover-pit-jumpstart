using System;
using UnityEngine;

namespace I2.Loc
{
	public class TermsPopup : PropertyAttribute
	{
		// Token: 0x06000FF9 RID: 4089 RVA: 0x00063CEE File Offset: 0x00061EEE
		public TermsPopup(string filter = "")
		{
			this.Filter = filter;
		}

		// (get) Token: 0x06000FFA RID: 4090 RVA: 0x00063CFD File Offset: 0x00061EFD
		// (set) Token: 0x06000FFB RID: 4091 RVA: 0x00063D05 File Offset: 0x00061F05
		public string Filter { get; private set; }
	}
}
