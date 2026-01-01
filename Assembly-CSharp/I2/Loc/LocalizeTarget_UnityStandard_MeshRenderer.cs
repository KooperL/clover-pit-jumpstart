using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001CC RID: 460
	public class LocalizeTarget_UnityStandard_MeshRenderer : LocalizeTarget<MeshRenderer>
	{
		// Token: 0x06001388 RID: 5000 RVA: 0x00015427 File Offset: 0x00013627
		static LocalizeTarget_UnityStandard_MeshRenderer()
		{
			LocalizeTarget_UnityStandard_MeshRenderer.AutoRegister();
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x0001542E File Offset: 0x0001362E
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<MeshRenderer, LocalizeTarget_UnityStandard_MeshRenderer>
			{
				Name = "MeshRenderer",
				Priority = 800
			});
		}

		// Token: 0x0600138A RID: 5002 RVA: 0x00015450 File Offset: 0x00013650
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Mesh;
		}

		// Token: 0x0600138B RID: 5003 RVA: 0x00015453 File Offset: 0x00013653
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Material;
		}

		// Token: 0x0600138C RID: 5004 RVA: 0x00007C86 File Offset: 0x00005E86
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x0600138D RID: 5005 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00081098 File Offset: 0x0007F298
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

		// Token: 0x06001390 RID: 5008 RVA: 0x00081130 File Offset: 0x0007F330
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
