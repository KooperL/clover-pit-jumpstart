using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x020001D4 RID: 468
	public class LocalizeTarget_UnityUI_Text : LocalizeTarget<Text>
	{
		// Token: 0x060013D2 RID: 5074 RVA: 0x000155D4 File Offset: 0x000137D4
		static LocalizeTarget_UnityUI_Text()
		{
			LocalizeTarget_UnityUI_Text.AutoRegister();
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x000155DB File Offset: 0x000137DB
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<Text, LocalizeTarget_UnityUI_Text>
			{
				Name = "Text",
				Priority = 100
			});
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00007C86 File Offset: 0x00005E86
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00007C86 File Offset: 0x00005E86
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00007C86 File Offset: 0x00005E86
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00081658 File Offset: 0x0007F858
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.font != null) ? this.mTarget.font.name : string.Empty);
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x000816B0 File Offset: 0x0007F8B0
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

		// Token: 0x060013DB RID: 5083 RVA: 0x000817DC File Offset: 0x0007F9DC
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

		// Token: 0x040013A2 RID: 5026
		private TextAnchor mAlignment_RTL = TextAnchor.UpperRight;

		// Token: 0x040013A3 RID: 5027
		private TextAnchor mAlignment_LTR;

		// Token: 0x040013A4 RID: 5028
		private bool mAlignmentWasRTL;

		// Token: 0x040013A5 RID: 5029
		private bool mInitializeAlignment = true;
	}
}
