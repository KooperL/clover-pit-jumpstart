using System;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_MeshRenderer : LocalizeTarget<MeshRenderer>
	{
		// Token: 0x06000F9A RID: 3994 RVA: 0x0006302C File Offset: 0x0006122C
		static LocalizeTarget_UnityStandard_MeshRenderer()
		{
			LocalizeTarget_UnityStandard_MeshRenderer.AutoRegister();
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x00063033 File Offset: 0x00061233
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<MeshRenderer, LocalizeTarget_UnityStandard_MeshRenderer>
			{
				Name = "MeshRenderer",
				Priority = 800
			});
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00063055 File Offset: 0x00061255
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Mesh;
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00063058 File Offset: 0x00061258
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Material;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x0006305B File Offset: 0x0006125B
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x0006305E File Offset: 0x0006125E
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00063061 File Offset: 0x00061261
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00063064 File Offset: 0x00061264
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			if (this.mTarget == null)
			{
				string text;
				secondaryTerm = (text = null);
				primaryTerm = text;
			}
			else
			{
				MeshFilter component = this.mTarget.GetComponent<MeshFilter>();
				if (component == null || component.sharedMesh == null)
				{
					primaryTerm = null;
				}
				else
				{
					primaryTerm = component.sharedMesh.name;
				}
			}
			if (this.mTarget == null || this.mTarget.sharedMaterial == null)
			{
				secondaryTerm = null;
				return;
			}
			secondaryTerm = this.mTarget.sharedMaterial.name;
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x000630FC File Offset: 0x000612FC
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			Material secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Material>(ref mainTranslation, ref secondaryTranslation);
			if (secondaryTranslatedObj != null && this.mTarget.sharedMaterial != secondaryTranslatedObj)
			{
				this.mTarget.material = secondaryTranslatedObj;
			}
			Mesh mesh = cmp.FindTranslatedObject<Mesh>(mainTranslation);
			MeshFilter component = this.mTarget.GetComponent<MeshFilter>();
			if (mesh != null && component.sharedMesh != mesh)
			{
				component.mesh = mesh;
			}
		}
	}
}
