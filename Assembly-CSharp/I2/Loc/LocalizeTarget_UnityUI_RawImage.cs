using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	public class LocalizeTarget_UnityUI_RawImage : LocalizeTarget<RawImage>
	{
		// Token: 0x06000FDA RID: 4058 RVA: 0x00063756 File Offset: 0x00061956
		static LocalizeTarget_UnityUI_RawImage()
		{
			LocalizeTarget_UnityUI_RawImage.AutoRegister();
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0006375D File Offset: 0x0006195D
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<RawImage, LocalizeTarget_UnityUI_RawImage>
			{
				Name = "RawImage",
				Priority = 100
			});
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0006377C File Offset: 0x0006197C
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Texture;
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x0006377F File Offset: 0x0006197F
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x00063782 File Offset: 0x00061982
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00063785 File Offset: 0x00061985
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x00063788 File Offset: 0x00061988
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0006378B File Offset: 0x0006198B
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget.mainTexture ? this.mTarget.mainTexture.name : "");
			secondaryTerm = null;
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x000637C0 File Offset: 0x000619C0
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Texture texture = this.mTarget.texture;
			if (texture == null || texture.name != mainTranslation)
			{
				this.mTarget.texture = cmp.FindTranslatedObject<Texture>(mainTranslation);
			}
		}
	}
}
