using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001D0 RID: 464
	public class LocalizeTarget_UnityStandard_TextMesh : LocalizeTarget<TextMesh>
	{
		// Token: 0x060013AA RID: 5034 RVA: 0x000154BD File Offset: 0x000136BD
		static LocalizeTarget_UnityStandard_TextMesh()
		{
			LocalizeTarget_UnityStandard_TextMesh.AutoRegister();
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x000154C4 File Offset: 0x000136C4
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMesh, LocalizeTarget_UnityStandard_TextMesh>
			{
				Name = "TextMesh",
				Priority = 100
			});
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00007C86 File Offset: 0x00005E86
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00007C86 File Offset: 0x00005E86
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x00007C86 File Offset: 0x00005E86
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00081354 File Offset: 0x0007F554
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((string.IsNullOrEmpty(Secondary) && this.mTarget.font != null) ? this.mTarget.font.name : null);
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x000813B0 File Offset: 0x0007F5B0
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Font secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null && this.mTarget.font != secondaryTranslatedObj)
			{
				this.mTarget.font = secondaryTranslatedObj;
				this.mTarget.GetComponentInChildren<MeshRenderer>().material = secondaryTranslatedObj.material;
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignment_LTR = (this.mAlignment_RTL = this.mTarget.alignment);
				if (LocalizationManager.IsRight2Left && this.mAlignment_RTL == TextAlignment.Right)
				{
					this.mAlignment_LTR = TextAlignment.Left;
				}
				if (!LocalizationManager.IsRight2Left && this.mAlignment_LTR == TextAlignment.Left)
				{
					this.mAlignment_RTL = TextAlignment.Right;
				}
			}
			if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (cmp.CorrectAlignmentForRTL && this.mTarget.alignment != TextAlignment.Center)
				{
					this.mTarget.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
				}
				this.mTarget.font.RequestCharactersInTexture(mainTranslation);
				this.mTarget.text = mainTranslation;
			}
		}

		// Token: 0x0400139E RID: 5022
		private TextAlignment mAlignment_RTL = TextAlignment.Right;

		// Token: 0x0400139F RID: 5023
		private TextAlignment mAlignment_LTR;

		// Token: 0x040013A0 RID: 5024
		private bool mAlignmentWasRTL;

		// Token: 0x040013A1 RID: 5025
		private bool mInitializeAlignment = true;
	}
}
