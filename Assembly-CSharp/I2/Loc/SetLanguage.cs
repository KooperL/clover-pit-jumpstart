using System;
using UnityEngine;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/SetLanguage Button")]
	public class SetLanguage : MonoBehaviour
	{
		// Token: 0x06001031 RID: 4145 RVA: 0x00065964 File Offset: 0x00063B64
		private void OnClick()
		{
			this.ApplyLanguage();
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x0006596C File Offset: 0x00063B6C
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
