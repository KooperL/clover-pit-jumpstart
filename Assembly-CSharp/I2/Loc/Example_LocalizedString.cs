using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000186 RID: 390
	public class Example_LocalizedString : MonoBehaviour
	{
		// Token: 0x060011A0 RID: 4512 RVA: 0x0007575C File Offset: 0x0007395C
		public void Start()
		{
			Debug.Log(this._MyLocalizedString);
			Debug.Log(LocalizationManager.GetTranslation(this._NormalString, true, 0, true, false, null, null, true));
			Debug.Log(LocalizationManager.GetTranslation(this._StringWithTermPopup, true, 0, true, false, null, null, true));
			Debug.Log("Term2");
			Debug.Log(this._MyLocalizedString);
			Debug.Log("Term3");
			LocalizedString localizedString = "Term3";
			localizedString.mRTL_IgnoreArabicFix = true;
			Debug.Log(localizedString);
			LocalizedString localizedString2 = "Term3";
			localizedString2.mRTL_ConvertNumbers = true;
			localizedString2.mRTL_MaxLineLength = 20;
			Debug.Log(localizedString2);
			Debug.Log(localizedString2);
		}

		// Token: 0x0400129D RID: 4765
		public LocalizedString _MyLocalizedString;

		// Token: 0x0400129E RID: 4766
		public string _NormalString;

		// Token: 0x0400129F RID: 4767
		[TermsPopup("")]
		public string _StringWithTermPopup;
	}
}
