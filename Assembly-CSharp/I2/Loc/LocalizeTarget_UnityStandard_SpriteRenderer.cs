using System;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_SpriteRenderer : LocalizeTarget<SpriteRenderer>
	{
		// Token: 0x06000F9B RID: 3995 RVA: 0x00062B2A File Offset: 0x00060D2A
		static LocalizeTarget_UnityStandard_SpriteRenderer()
		{
			LocalizeTarget_UnityStandard_SpriteRenderer.AutoRegister();
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00062B31 File Offset: 0x00060D31
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<SpriteRenderer, LocalizeTarget_UnityStandard_SpriteRenderer>
			{
				Name = "SpriteRenderer",
				Priority = 100
			});
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00062B50 File Offset: 0x00060D50
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Sprite;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x00062B53 File Offset: 0x00060D53
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x00062B56 File Offset: 0x00060D56
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00062B59 File Offset: 0x00060D59
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00062B5C File Offset: 0x00060D5C
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00062B60 File Offset: 0x00060D60
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			Sprite sprite = this.mTarget.sprite;
			primaryTerm = ((sprite != null) ? sprite.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00062B98 File Offset: 0x00060D98
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
