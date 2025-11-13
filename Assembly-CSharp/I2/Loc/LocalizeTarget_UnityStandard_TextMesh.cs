using System;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_TextMesh : LocalizeTarget<TextMesh>
	{
		// Token: 0x06000FBC RID: 4028 RVA: 0x000633BE File Offset: 0x000615BE
		static LocalizeTarget_UnityStandard_TextMesh()
		{
			LocalizeTarget_UnityStandard_TextMesh.AutoRegister();
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x000633C5 File Offset: 0x000615C5
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMesh, LocalizeTarget_UnityStandard_TextMesh>
			{
				Name = "TextMesh",
				Priority = 100
			});
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x000633E4 File Offset: 0x000615E4
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x000633E7 File Offset: 0x000615E7
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x000633EA File Offset: 0x000615EA
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x000633ED File Offset: 0x000615ED
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x000633F0 File Offset: 0x000615F0
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x000633F4 File Offset: 0x000615F4
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((string.IsNullOrEmpty(Secondary) && this.mTarget.font != null) ? this.mTarget.font.name : null);
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00063450 File Offset: 0x00061650
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
				if (LocalizationManager.IsRight2Left && this.mAlignment_RTL == 2)
				{
					this.mAlignment_LTR = 0;
				}
				if (!LocalizationManager.IsRight2Left && this.mAlignment_LTR == null)
				{
					this.mAlignment_RTL = 2;
				}
			}
			if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (cmp.CorrectAlignmentForRTL && this.mTarget.alignment != 1)
				{
					this.mTarget.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
				}
				this.mTarget.font.RequestCharactersInTexture(mainTranslation);
				this.mTarget.text = mainTranslation;
			}
		}

		private TextAlignment mAlignment_RTL = 2;

		private TextAlignment mAlignment_LTR;

		private bool mAlignmentWasRTL;

		private bool mInitializeAlignment = true;
	}
}
