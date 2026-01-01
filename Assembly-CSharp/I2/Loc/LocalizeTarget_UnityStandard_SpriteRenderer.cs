using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001CF RID: 463
	public class LocalizeTarget_UnityStandard_SpriteRenderer : LocalizeTarget<SpriteRenderer>
	{
		// Token: 0x060013A0 RID: 5024 RVA: 0x0001548F File Offset: 0x0001368F
		static LocalizeTarget_UnityStandard_SpriteRenderer()
		{
			LocalizeTarget_UnityStandard_SpriteRenderer.AutoRegister();
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x00015496 File Offset: 0x00013696
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<SpriteRenderer, LocalizeTarget_UnityStandard_SpriteRenderer>
			{
				Name = "SpriteRenderer",
				Priority = 100
			});
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x0000D17C File Offset: 0x0000B37C
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Sprite;
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x000812D8 File Offset: 0x0007F4D8
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			Sprite sprite = this.mTarget.sprite;
			primaryTerm = ((sprite != null) ? sprite.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x00081310 File Offset: 0x0007F510
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
