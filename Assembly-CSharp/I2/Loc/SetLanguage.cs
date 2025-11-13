using System;
using UnityEngine;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/SetLanguage Button")]
	public class SetLanguage : MonoBehaviour
	{
		// Token: 0x06001048 RID: 4168 RVA: 0x00066140 File Offset: 0x00064340
		private void OnClick()
		{
			this.ApplyLanguage();
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00066148 File Offset: 0x00064348
		public void ApplyLanguage()
		{
			if (LocalizationManager.HasLanguage(this._Language, true, true, true))
			{
				LocalizationManager.CurrentLanguage = this._Language;
			}
		}

		public string _Language;
	}
}
