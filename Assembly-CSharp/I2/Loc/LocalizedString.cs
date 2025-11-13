using System;

namespace I2.Loc
{
	[Serializable]
	public struct LocalizedString
	{
		// Token: 0x0600101F RID: 4127 RVA: 0x00064771 File Offset: 0x00062971
		public static implicit operator string(LocalizedString s)
		{
			return s.ToString();
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00064780 File Offset: 0x00062980
		public static implicit operator LocalizedString(string term)
		{
			return new LocalizedString
			{
				mTerm = term
			};
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0006479E File Offset: 0x0006299E
		public LocalizedString(LocalizedString str)
		{
			this.mTerm = str.mTerm;
			this.mRTL_IgnoreArabicFix = str.mRTL_IgnoreArabicFix;
			this.mRTL_MaxLineLength = str.mRTL_MaxLineLength;
			this.mRTL_ConvertNumbers = str.mRTL_ConvertNumbers;
			this.m_DontLocalizeParameters = str.m_DontLocalizeParameters;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x000647DC File Offset: 0x000629DC
		public override string ToString()
		{
			string translation = LocalizationManager.GetTranslation(this.mTerm, !this.mRTL_IgnoreArabicFix, this.mRTL_MaxLineLength, !this.mRTL_ConvertNumbers, true, null, null, true);
			LocalizationManager.ApplyLocalizationParams(ref translation, !this.m_DontLocalizeParameters);
			return translation;
		}

		public string mTerm;

		public bool mRTL_IgnoreArabicFix;

		public int mRTL_MaxLineLength;

		public bool mRTL_ConvertNumbers;

		public bool m_DontLocalizeParameters;
	}
}
