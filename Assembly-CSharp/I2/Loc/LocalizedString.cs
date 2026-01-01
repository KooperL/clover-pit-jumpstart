using System;

namespace I2.Loc
{
	// Token: 0x020001E4 RID: 484
	[Serializable]
	public struct LocalizedString
	{
		// Token: 0x06001413 RID: 5139 RVA: 0x000157C8 File Offset: 0x000139C8
		public static implicit operator string(LocalizedString s)
		{
			return s.ToString();
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x000823BC File Offset: 0x000805BC
		public static implicit operator LocalizedString(string term)
		{
			return new LocalizedString
			{
				mTerm = term
			};
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x000157D7 File Offset: 0x000139D7
		public LocalizedString(LocalizedString str)
		{
			this.mTerm = str.mTerm;
			this.mRTL_IgnoreArabicFix = str.mRTL_IgnoreArabicFix;
			this.mRTL_MaxLineLength = str.mRTL_MaxLineLength;
			this.mRTL_ConvertNumbers = str.mRTL_ConvertNumbers;
			this.m_DontLocalizeParameters = str.m_DontLocalizeParameters;
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x000823DC File Offset: 0x000805DC
		public override string ToString()
		{
			string translation = LocalizationManager.GetTranslation(this.mTerm, !this.mRTL_IgnoreArabicFix, this.mRTL_MaxLineLength, !this.mRTL_ConvertNumbers, true, null, null, true);
			LocalizationManager.ApplyLocalizationParams(ref translation, !this.m_DontLocalizeParameters);
			return translation;
		}

		// Token: 0x040013CC RID: 5068
		public string mTerm;

		// Token: 0x040013CD RID: 5069
		public bool mRTL_IgnoreArabicFix;

		// Token: 0x040013CE RID: 5070
		public int mRTL_MaxLineLength;

		// Token: 0x040013CF RID: 5071
		public bool mRTL_ConvertNumbers;

		// Token: 0x040013D0 RID: 5072
		public bool m_DontLocalizeParameters;
	}
}
