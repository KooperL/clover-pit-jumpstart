using System;
using TMPro;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_TextMeshPro_UGUI : LocalizeTarget<TextMeshProUGUI>
	{
		// Token: 0x06000F79 RID: 3961 RVA: 0x00062BF3 File Offset: 0x00060DF3
		static LocalizeTarget_TextMeshPro_UGUI()
		{
			LocalizeTarget_TextMeshPro_UGUI.AutoRegister();
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x00062BFA File Offset: 0x00060DFA
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMeshProUGUI, LocalizeTarget_TextMeshPro_UGUI>
			{
				Name = "TextMeshPro UGUI",
				Priority = 100
			});
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x00062C19 File Offset: 0x00060E19
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x00062C1C File Offset: 0x00060E1C
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.TextMeshPFont;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x00062C20 File Offset: 0x00060E20
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x00062C23 File Offset: 0x00060E23
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00062C26 File Offset: 0x00060E26
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x00062C2C File Offset: 0x00060E2C
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.font != null) ? this.mTarget.font.name : string.Empty);
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x00062C84 File Offset: 0x00060E84
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			TMP_FontAsset tmp_FontAsset = cmp.GetSecondaryTranslatedObj<TMP_FontAsset>(ref mainTranslation, ref secondaryTranslation);
			if (tmp_FontAsset != null)
			{
				LocalizeTarget_TextMeshPro_Label.SetFont(this.mTarget, tmp_FontAsset);
			}
			else
			{
				Material secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Material>(ref mainTranslation, ref secondaryTranslation);
				if (secondaryTranslatedObj != null && this.mTarget.fontMaterial != secondaryTranslatedObj)
				{
					if (!secondaryTranslatedObj.name.StartsWith(this.mTarget.font.name, StringComparison.Ordinal))
					{
						tmp_FontAsset = LocalizeTarget_TextMeshPro_Label.GetTMPFontFromMaterial(cmp, secondaryTranslation.EndsWith(secondaryTranslatedObj.name, StringComparison.Ordinal) ? secondaryTranslation : secondaryTranslatedObj.name);
						if (tmp_FontAsset != null)
						{
							LocalizeTarget_TextMeshPro_Label.SetFont(this.mTarget, tmp_FontAsset);
						}
					}
					LocalizeTarget_TextMeshPro_Label.SetMaterial(this.mTarget, secondaryTranslatedObj);
				}
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
				LocalizeTarget_TextMeshPro_Label.InitAlignment_TMPro(this.mAlignmentWasRTL, this.mTarget.alignment, out this.mAlignment_LTR, out this.mAlignment_RTL);
			}
			else
			{
				TextAlignmentOptions textAlignmentOptions;
				TextAlignmentOptions textAlignmentOptions2;
				LocalizeTarget_TextMeshPro_Label.InitAlignment_TMPro(this.mAlignmentWasRTL, this.mTarget.alignment, out textAlignmentOptions, out textAlignmentOptions2);
				if ((this.mAlignmentWasRTL && this.mAlignment_RTL != textAlignmentOptions2) || (!this.mAlignmentWasRTL && this.mAlignment_LTR != textAlignmentOptions))
				{
					this.mAlignment_LTR = textAlignmentOptions;
					this.mAlignment_RTL = textAlignmentOptions2;
				}
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
			}
			if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (cmp.CorrectAlignmentForRTL)
				{
					this.mTarget.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
				}
				if (LocalizationManager.IsRight2Left)
				{
					mainTranslation = I2Utils.ReverseText(mainTranslation);
				}
				this.mTarget.isRightToLeftText = LocalizationManager.IsRight2Left;
				this.mTarget.text = mainTranslation;
			}
		}

		public TextAlignmentOptions mAlignment_RTL = 516;

		public TextAlignmentOptions mAlignment_LTR = 513;

		public bool mAlignmentWasRTL;

		public bool mInitializeAlignment = true;
	}
}
