using System;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x020001D3 RID: 467
	public class LocalizeTarget_UnityUI_RawImage : LocalizeTarget<RawImage>
	{
		// Token: 0x060013C8 RID: 5064 RVA: 0x00015571 File Offset: 0x00013771
		static LocalizeTarget_UnityUI_RawImage()
		{
			LocalizeTarget_UnityUI_RawImage.AutoRegister();
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00015578 File Offset: 0x00013778
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<RawImage, LocalizeTarget_UnityUI_RawImage>
			{
				Name = "RawImage",
				Priority = 100
			});
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x00015597 File Offset: 0x00013797
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Texture;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0001559A File Offset: 0x0001379A
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget.mainTexture ? this.mTarget.mainTexture.name : "");
			secondaryTerm = null;
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x00081614 File Offset: 0x0007F814
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
