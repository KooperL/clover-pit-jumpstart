using System;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_TextMesh : LocalizeTarget<TextMesh>
	{
		// Token: 0x06000FA5 RID: 4005 RVA: 0x00062BE2 File Offset: 0x00060DE2
		static LocalizeTarget_UnityStandard_TextMesh()
		{
			LocalizeTarget_UnityStandard_TextMesh.AutoRegister();
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x00062BE9 File Offset: 0x00060DE9
		[RuntimeInitializeOnLoadMethod(1)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMesh, LocalizeTarget_UnityStandard_TextMesh>
			{
				Name = "TextMesh",
				Priority = 100
			});
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00062C08 File Offset: 0x00060E08
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00062C0B File Offset: 0x00060E0B
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00062C0E File Offset: 0x00060E0E
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00062C11 File Offset: 0x00060E11
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x00062C14 File Offset: 0x00060E14
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x00062C18 File Offset: 0x00060E18
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((string.IsNullOrEmpty(Secondary) && this.mTarget.font != null) ? this.mTarget.font.name : null);
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x00062C74 File Offset: 0x00060E74
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
