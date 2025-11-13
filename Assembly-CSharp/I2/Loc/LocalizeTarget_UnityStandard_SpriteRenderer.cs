using System;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_SpriteRenderer : LocalizeTarget<SpriteRenderer>
	{
		// Token: 0x06000FB2 RID: 4018 RVA: 0x00063306 File Offset: 0x00061506
		static LocalizeTarget_UnityStandard_SpriteRenderer()
		{
			LocalizeTarget_UnityStandard_SpriteRenderer.AutoRegister();
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x0006330D File Offset: 0x0006150D
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<SpriteRenderer, LocalizeTarget_UnityStandard_SpriteRenderer>
			{
				Name = "SpriteRenderer",
				Priority = 100
			});
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0006332C File Offset: 0x0006152C
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Sprite;
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x0006332F File Offset: 0x0006152F
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00063332 File Offset: 0x00061532
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00063335 File Offset: 0x00061535
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00063338 File Offset: 0x00061538
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0006333C File Offset: 0x0006153C
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			Sprite sprite = this.mTarget.sprite;
			primaryTerm = ((sprite != null) ? sprite.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00063374 File Offset: 0x00061574
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
