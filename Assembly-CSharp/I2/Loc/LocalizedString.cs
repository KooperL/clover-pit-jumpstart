using System;

namespace I2.Loc
{
	[Serializable]
	public struct LocalizedString
	{
		// Token: 0x06001008 RID: 4104 RVA: 0x00063F95 File Offset: 0x00062195
		public static implicit operator string(LocalizedString s)
		{
			return s.ToString();
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x00063FA4 File Offset: 0x000621A4
		public static implicit operator LocalizedString(string term)
		{
			return new LocalizedString
			{
				mTerm = term
			};
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x00063FC2 File Offset: 0x000621C2
		public LocalizedString(LocalizedString str)
		{
			this.mTerm = str.mTerm;
			this.mRTL_IgnoreArabicFix = str.mRTL_IgnoreArabicFix;
			this.mRTL_MaxLineLength = str.mRTL_MaxLineLength;
			this.mRTL_ConvertNumbers = str.mRTL_ConvertNumbers;
			this.m_DontLocalizeParameters = str.m_DontLocalizeParameters;
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00064000 File Offset: 0x00062200
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
