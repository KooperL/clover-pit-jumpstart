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
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
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
				case TextAnchor.UpperLeft:
					alignLTR = TextAnchor.UpperRight;
					return;
				case TextAnchor.UpperCenter:
				case TextAnchor.MiddleCenter:
				case TextAnchor.LowerCenter:
					break;
				case TextAnchor.UpperRight:
					alignLTR = TextAnchor.UpperLeft;
					return;
				case TextAnchor.MiddleLeft:
					alignLTR = TextAnchor.MiddleRight;
					return;
				case TextAnchor.MiddleRight:
					alignLTR = TextAnchor.MiddleLeft;
					return;
				case TextAnchor.LowerLeft:
					alignLTR = TextAnchor.LowerRight;
					return;
				case TextAnchor.LowerRight:
					alignLTR = TextAnchor.LowerLeft;
					return;
				default:
					return;
				}
			}
			else
			{
				switch (alignment)
				{
				case TextAnchor.UpperLeft:
					alignRTL = TextAnchor.UpperRight;
					return;
				case TextAnchor.UpperCenter:
				case TextAnchor.MiddleCenter:
				case TextAnchor.LowerCenter:
					break;
				case TextAnchor.UpperRight:
					alignRTL = TextAnchor.UpperLeft;
					return;
				case TextAnchor.MiddleLeft:
					alignRTL = TextAnchor.MiddleRight;
					return;
				case TextAnchor.MiddleRight:
					alignRTL = TextAnchor.MiddleLeft;
					return;
				case TextAnchor.LowerLeft:
					alignRTL = TextAnchor.LowerRight;
					break;
				case TextAnchor.LowerRight:
					alignRTL = TextAnchor.LowerLeft;
					return;
				default:
					return;
				}
			}
		}

		private TextAnchor mAlignment_RTL = TextAnchor.UpperRight;

		private TextAnchor mAlignment_LTR;

		private bool mAlignmentWasRTL;

		private bool mInitializeAlignment = true;
	}
}
