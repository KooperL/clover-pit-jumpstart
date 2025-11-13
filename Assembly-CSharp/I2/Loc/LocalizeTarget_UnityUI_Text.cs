using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	public class LocalizeTarget_UnityUI_Text : LocalizeTarget<Text>
	{
		// Token: 0x06000FE4 RID: 4068 RVA: 0x0006380A File Offset: 0x00061A0A
		static LocalizeTarget_UnityUI_Text()
		{
			LocalizeTarget_UnityUI_Text.AutoRegister();
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x00063811 File Offset: 0x00061A11
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<Text, LocalizeTarget_UnityUI_Text>
			{
				Name = "Text",
				Priority = 100
			});
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x00063830 File Offset: 0x00061A30
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x00063833 File Offset: 0x00061A33
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x00063836 File Offset: 0x00061A36
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x00063839 File Offset: 0x00061A39
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0006383C File Offset: 0x00061A3C
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00063840 File Offset: 0x00061A40
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.font != null) ? this.mTarget.font.name : string.Empty);
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00063898 File Offset: 0x00061A98
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

		// Token: 0x06000FED RID: 4077 RVA: 0x000639C4 File Offset: 0x00061BC4
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
