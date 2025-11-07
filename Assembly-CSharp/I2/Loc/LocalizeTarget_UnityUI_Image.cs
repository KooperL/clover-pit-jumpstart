using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	public class LocalizeTarget_UnityUI_Image : LocalizeTarget<Image>
	{
		// Token: 0x06000FB9 RID: 4025 RVA: 0x00062E5A File Offset: 0x0006105A
		static LocalizeTarget_UnityUI_Image()
		{
			LocalizeTarget_UnityUI_Image.AutoRegister();
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00062E61 File Offset: 0x00061061
		[RuntimeInitializeOnLoadMethod(1)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<Image, LocalizeTarget_UnityUI_Image>
			{
				Name = "Image",
				Priority = 100
			});
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00062E80 File Offset: 0x00061080
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x00062E83 File Offset: 0x00061083
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x00062E86 File Offset: 0x00061086
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x00062E89 File Offset: 0x00061089
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			if (!(this.mTarget.sprite == null))
			{
				return eTermType.Sprite;
			}
			return eTermType.Texture;
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x00062EA1 File Offset: 0x000610A1
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x00062EA4 File Offset: 0x000610A4
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget.mainTexture ? this.mTarget.mainTexture.name : "");
			if (this.mTarget.sprite != null && this.mTarget.sprite.name != primaryTerm)
			{
				primaryTerm = primaryTerm + "." + this.mTarget.sprite.name;
			}
			secondaryTerm = null;
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00062F30 File Offset: 0x00061130
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Sprite sprite = this.mTarget.sprite;
			if (sprite == null || sprite.name != mainTranslation)
			{
				this.mTarget.sprite = cmp.FindTranslatedObject<Sprite>(mainTranslation);
			}
		}
	}
}
