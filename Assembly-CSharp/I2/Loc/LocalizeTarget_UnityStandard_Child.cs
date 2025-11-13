using System;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_Child : LocalizeTarget<GameObject>
	{
		// Token: 0x06000F8F RID: 3983 RVA: 0x00062F61 File Offset: 0x00061161
		static LocalizeTarget_UnityStandard_Child()
		{
			LocalizeTarget_UnityStandard_Child.AutoRegister();
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x00062F68 File Offset: 0x00061168
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Child
			{
				Name = "Child",
				Priority = 200
			});
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00062F8A File Offset: 0x0006118A
		public override bool IsValid(Localize cmp)
		{
			return cmp.transform.childCount > 1;
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x00062F9A File Offset: 0x0006119A
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.GameObject;
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00062F9D File Offset: 0x0006119D
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00062FA0 File Offset: 0x000611A0
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x00062FA3 File Offset: 0x000611A3
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x00062FA6 File Offset: 0x000611A6
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x00062FA9 File Offset: 0x000611A9
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = cmp.name;
			secondaryTerm = null;
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00062FB8 File Offset: 0x000611B8
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
