using System;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_Prefab : LocalizeTarget<GameObject>
	{
		// Token: 0x06000FA6 RID: 4006 RVA: 0x00063181 File Offset: 0x00061381
		static LocalizeTarget_UnityStandard_Prefab()
		{
			LocalizeTarget_UnityStandard_Prefab.AutoRegister();
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00063188 File Offset: 0x00061388
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Prefab
			{
				Name = "Prefab",
				Priority = 250
			});
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x000631AA File Offset: 0x000613AA
		public override bool IsValid(Localize cmp)
		{
			return true;
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x000631AD File Offset: 0x000613AD
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.GameObject;
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x000631B0 File Offset: 0x000613B0
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x000631B3 File Offset: 0x000613B3
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x000631B6 File Offset: 0x000613B6
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x000631B9 File Offset: 0x000613B9
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x000631BC File Offset: 0x000613BC
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = cmp.name;
			secondaryTerm = null;
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x000631CC File Offset: 0x000613CC
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
					global::UnityEngine.Object.Destroy(child.gameObject);
				}
			}
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00063278 File Offset: 0x00061478
		private Transform InstantiateNewPrefab(Localize cmp, string mainTranslation)
		{
			GameObject gameObject = cmp.FindTranslatedObject<GameObject>(mainTranslation);
			if (gameObject == null)
			{
				return null;
			}
			GameObject mTarget = this.mTarget;
			this.mTarget = global::UnityEngine.Object.Instantiate<GameObject>(gameObject);
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
