using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001CB RID: 459
	public class LocalizeTarget_UnityStandard_Child : LocalizeTarget<GameObject>
	{
		// Token: 0x0600137D RID: 4989 RVA: 0x000153E7 File Offset: 0x000135E7
		static LocalizeTarget_UnityStandard_Child()
		{
			LocalizeTarget_UnityStandard_Child.AutoRegister();
		}

		// Token: 0x0600137E RID: 4990 RVA: 0x000153EE File Offset: 0x000135EE
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Child
			{
				Name = "Child",
				Priority = 200
			});
		}

		// Token: 0x0600137F RID: 4991 RVA: 0x000153CF File Offset: 0x000135CF
		public override bool IsValid(Localize cmp)
		{
			return cmp.transform.childCount > 1;
		}

		// Token: 0x06001380 RID: 4992 RVA: 0x0000D17F File Offset: 0x0000B37F
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.GameObject;
		}

		// Token: 0x06001381 RID: 4993 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06001383 RID: 4995 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06001384 RID: 4996 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06001385 RID: 4997 RVA: 0x00015410 File Offset: 0x00013610
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = cmp.name;
			secondaryTerm = null;
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x0008102C File Offset: 0x0007F22C
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			if (string.IsNullOrEmpty(mainTranslation))
			{
				return;
			}
			Transform transform = cmp.transform;
			string text = mainTranslation;
			int num = mainTranslation.LastIndexOfAny(LanguageSourceData.CategorySeparators);
			if (num >= 0)
			{
				text = text.Substring(num + 1);
			}
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				child.gameObject.SetActive(child.name == text);
			}
		}
	}
}
