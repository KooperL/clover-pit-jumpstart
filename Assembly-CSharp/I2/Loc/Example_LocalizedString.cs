using System;
using UnityEngine;

namespace I2.Loc
{
	public class Example_LocalizedString : MonoBehaviour
	{
		// Token: 0x06000DED RID: 3565 RVA: 0x000564FC File Offset: 0x000546FC
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

		public LocalizedString _MyLocalizedString;

		public string _NormalString;

		[TermsPopup("")]
		public string _StringWithTermPopup;
	}
}
