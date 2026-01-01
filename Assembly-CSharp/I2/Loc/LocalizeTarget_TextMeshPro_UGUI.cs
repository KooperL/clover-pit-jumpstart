using System;
using TMPro;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001C8 RID: 456
	public class LocalizeTarget_TextMeshPro_UGUI : LocalizeTarget<TextMeshProUGUI>
	{
		// Token: 0x06001367 RID: 4967 RVA: 0x0001534F File Offset: 0x0001354F
		static LocalizeTarget_TextMeshPro_UGUI()
		{
			LocalizeTarget_TextMeshPro_UGUI.AutoRegister();
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00015356 File Offset: 0x00013556
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMeshProUGUI, LocalizeTarget_TextMeshPro_UGUI>
			{
				Name = "TextMeshPro UGUI",
				Priority = 100
			});
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x00015375 File Offset: 0x00013575
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.TextMeshPFont;
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x00007C86 File Offset: 0x00005E86
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x00007C86 File Offset: 0x00005E86
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x00080D70 File Offset: 0x0007EF70
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.font != null) ? this.mTarget.font.name : string.Empty);
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x00080DC8 File Offset: 0x0007EFC8
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

		// Token: 0x0400139A RID: 5018
		public TextAlignmentOptions mAlignment_RTL = TextAlignmentOptions.Right;

		// Token: 0x0400139B RID: 5019
		public TextAlignmentOptions mAlignment_LTR = TextAlignmentOptions.Left;

		// Token: 0x0400139C RID: 5020
		public bool mAlignmentWasRTL;

		// Token: 0x0400139D RID: 5021
		public bool mInitializeAlignment = true;
	}
}
