using System;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_Prefab : LocalizeTarget<GameObject>
	{
		// Token: 0x06000F8F RID: 3983 RVA: 0x000629A5 File Offset: 0x00060BA5
		static LocalizeTarget_UnityStandard_Prefab()
		{
			LocalizeTarget_UnityStandard_Prefab.AutoRegister();
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x000629AC File Offset: 0x00060BAC
		[RuntimeInitializeOnLoadMethod(1)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Prefab
			{
				Name = "Prefab",
				Priority = 250
			});
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x000629CE File Offset: 0x00060BCE
		public override bool IsValid(Localize cmp)
		{
			return true;
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x000629D1 File Offset: 0x00060BD1
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.GameObject;
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x000629D4 File Offset: 0x00060BD4
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x000629D7 File Offset: 0x00060BD7
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x000629DA File Offset: 0x00060BDA
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x000629DD File Offset: 0x00060BDD
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x000629E0 File Offset: 0x00060BE0
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = cmp.name;
			secondaryTerm = null;
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x000629F0 File Offset: 0x00060BF0
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			if (string.IsNullOrEmpty(mainTranslation))
			{
				return;
			}
			if (this.mTarget && this.mTarget.name == mainTranslation)
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
			Transform transform2 = this.InstantiateNewPrefab(cmp, mainTranslation);
			if (transform2 == null)
			{
				return;
			}
			transform2.name = text;
			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				Transform child = transform.GetChild(i);
				if (child != transform2)
				{
					Object.Destroy(child.gameObject);
				}
			}
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00062A9C File Offset: 0x00060C9C
		private Transform InstantiateNewPrefab(Localize cmp, string mainTranslation)
		{
			GameObject gameObject = cmp.FindTranslatedObject<GameObject>(mainTranslation);
			if (gameObject == null)
			{
				return null;
			}
			GameObject mTarget = this.mTarget;
			this.mTarget = Object.Instantiate<GameObject>(gameObject);
			if (this.mTarget == null)
			{
				return null;
			}
			Transform transform = cmp.transform;
			Transform transform2 = this.mTarget.transform;
			transform2.SetParent(transform);
			Transform transform3 = (mTarget ? mTarget.transform : transform);
			transform2.rotation = transform3.rotation;
			transform2.position = transform3.position;
			return transform2;
		}
	}
}
