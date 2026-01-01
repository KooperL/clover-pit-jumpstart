using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001F0 RID: 496
	[AddComponentMenu("I2/Localization/SetLanguage Button")]
	public class SetLanguage : MonoBehaviour
	{
		// Token: 0x0600143C RID: 5180 RVA: 0x000158F7 File Offset: 0x00013AF7
		private void OnClick()
		{
			this.ApplyLanguage();
		}

		// Token: 0x0600143D RID: 5181 RVA: 0x000158FF File Offset: 0x00013AFF
		public void ApplyLanguage()
		{
			if (LocalizationManager.HasLanguage(this._Language, true, true, true))
			{
				LocalizationManager.CurrentLanguage = this._Language;
			}
		}

		// Token: 0x04001431 RID: 5169
		public string _Language;
	}
}
