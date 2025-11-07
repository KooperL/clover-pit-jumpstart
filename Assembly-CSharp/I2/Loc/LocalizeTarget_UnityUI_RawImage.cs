using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	public class LocalizeTarget_UnityUI_RawImage : LocalizeTarget<RawImage>
	{
		// Token: 0x06000FC3 RID: 4035 RVA: 0x00062F7A File Offset: 0x0006117A
		static LocalizeTarget_UnityUI_RawImage()
		{
			LocalizeTarget_UnityUI_RawImage.AutoRegister();
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00062F81 File Offset: 0x00061181
		[RuntimeInitializeOnLoadMethod(1)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<RawImage, LocalizeTarget_UnityUI_RawImage>
			{
				Name = "RawImage",
				Priority = 100
			});
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x00062FA0 File Offset: 0x000611A0
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Texture;
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x00062FA3 File Offset: 0x000611A3
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x00062FA6 File Offset: 0x000611A6
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x00062FA9 File Offset: 0x000611A9
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00062FAC File Offset: 0x000611AC
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00062FAF File Offset: 0x000611AF
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget.mainTexture ? this.mTarget.mainTexture.name : "");
			secondaryTerm = null;
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00062FE4 File Offset: 0x000611E4
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
