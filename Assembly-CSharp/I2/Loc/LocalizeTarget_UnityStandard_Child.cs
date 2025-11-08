using System;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_Child : LocalizeTarget<GameObject>
	{
		// Token: 0x06000F78 RID: 3960 RVA: 0x00062785 File Offset: 0x00060985
		static LocalizeTarget_UnityStandard_Child()
		{
			LocalizeTarget_UnityStandard_Child.AutoRegister();
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0006278C File Offset: 0x0006098C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Child
			{
				Name = "Child",
				Priority = 200
			});
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x000627AE File Offset: 0x000609AE
		public override bool IsValid(Localize cmp)
		{
			return cmp.transform.childCount > 1;
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x000627BE File Offset: 0x000609BE
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.GameObject;
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x000627C1 File Offset: 0x000609C1
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x000627C4 File Offset: 0x000609C4
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x000627C7 File Offset: 0x000609C7
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x000627CA File Offset: 0x000609CA
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x000627CD File Offset: 0x000609CD
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = cmp.name;
			secondaryTerm = null;
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x000627DC File Offset: 0x000609DC
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
