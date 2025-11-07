using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	public class LocalizeTarget_UnityUI_Text : LocalizeTarget<Text>
	{
		// Token: 0x06000FCD RID: 4045 RVA: 0x0006302E File Offset: 0x0006122E
		static LocalizeTarget_UnityUI_Text()
		{
			LocalizeTarget_UnityUI_Text.AutoRegister();
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x00063035 File Offset: 0x00061235
		[RuntimeInitializeOnLoadMethod(1)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<Text, LocalizeTarget_UnityUI_Text>
			{
				Name = "Text",
				Priority = 100
			});
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00063054 File Offset: 0x00061254
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00063057 File Offset: 0x00061257
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0006305A File Offset: 0x0006125A
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0006305D File Offset: 0x0006125D
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00063060 File Offset: 0x00061260
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00063064 File Offset: 0x00061264
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.font != null) ? this.mTarget.font.name : string.Empty);
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x000630BC File Offset: 0x000612BC
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Font secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null && secondaryTranslatedObj != this.mTarget.font)
			{
				this.mTarget.font = secondaryTranslatedObj;
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
				this.InitAlignment(this.mAlignmentWasRTL, this.mTarget.alignment, out this.mAlignment_LTR, out this.mAlignment_RTL);
			}
			else
			{
				TextAnchor textAnchor;
				TextAnchor textAnchor2;
				this.InitAlignment(this.mAlignmentWasRTL, this.mTarget.alignment, out textAnchor, out textAnchor2);
				if ((this.mAlignmentWasRTL && this.mAlignment_RTL != textAnchor2) || (!this.mAlignmentWasRTL && this.mAlignment_LTR != textAnchor))
				{
					this.mAlignment_LTR = textAnchor;
					this.mAlignment_RTL = textAnchor2;
				}
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
			}
			if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (cmp.CorrectAlignmentForRTL)
				{
					this.mTarget.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
				}
				this.mTarget.text = mainTranslation;
				this.mTarget.SetVerticesDirty();
			}
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x000631E8 File Offset: 0x000613E8
		private void InitAlignment(bool isRTL, TextAnchor alignment, out TextAnchor alignLTR, out TextAnchor alignRTL)
		{
			alignRTL = alignment;
			alignLTR = alignment;
			if (isRTL)
			{
				switch (alignment)
				{
				case 0:
					alignLTR = 2;
					return;
				case 1:
				case 4:
				case 7:
					break;
				case 2:
					alignLTR = 0;
					return;
				case 3:
					alignLTR = 5;
					return;
				case 5:
					alignLTR = 3;
					return;
				case 6:
					alignLTR = 8;
					return;
				case 8:
					alignLTR = 6;
					return;
				default:
					return;
				}
			}
			else
			{
				switch (alignment)
				{
				case 0:
					alignRTL = 2;
					return;
				case 1:
				case 4:
				case 7:
					break;
				case 2:
					alignRTL = 0;
					return;
				case 3:
					alignRTL = 5;
					return;
				case 5:
					alignRTL = 3;
					return;
				case 6:
					alignRTL = 8;
					break;
				case 8:
					alignRTL = 6;
					return;
				default:
					return;
				}
			}
		}

		private TextAnchor mAlignment_RTL = 2;

		private TextAnchor mAlignment_LTR;

		private bool mAlignmentWasRTL;

		private bool mInitializeAlignment = true;
	}
}
