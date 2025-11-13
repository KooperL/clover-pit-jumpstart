using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	public class LocalizeTarget_UnityUI_Image : LocalizeTarget<Image>
	{
		// Token: 0x06000FD0 RID: 4048 RVA: 0x00063636 File Offset: 0x00061836
		static LocalizeTarget_UnityUI_Image()
		{
			LocalizeTarget_UnityUI_Image.AutoRegister();
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0006363D File Offset: 0x0006183D
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<Image, LocalizeTarget_UnityUI_Image>
			{
				Name = "Image",
				Priority = 100
			});
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0006365C File Offset: 0x0006185C
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x0006365F File Offset: 0x0006185F
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00063662 File Offset: 0x00061862
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00063665 File Offset: 0x00061865
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			if (!(this.mTarget.sprite == null))
			{
				return eTermType.Sprite;
			}
			return eTermType.Texture;
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0006367D File Offset: 0x0006187D
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00063680 File Offset: 0x00061880
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget.mainTexture ? this.mTarget.mainTexture.name : "");
			if (this.mTarget.sprite != null && this.mTarget.sprite.name != primaryTerm)
			{
				primaryTerm = primaryTerm + "." + this.mTarget.sprite.name;
			}
			secondaryTerm = null;
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0006370C File Offset: 0x0006190C
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
