using System;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_MeshRenderer : LocalizeTarget<MeshRenderer>
	{
		// Token: 0x06000F83 RID: 3971 RVA: 0x00062850 File Offset: 0x00060A50
		static LocalizeTarget_UnityStandard_MeshRenderer()
		{
			LocalizeTarget_UnityStandard_MeshRenderer.AutoRegister();
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x00062857 File Offset: 0x00060A57
		[RuntimeInitializeOnLoadMethod(1)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<MeshRenderer, LocalizeTarget_UnityStandard_MeshRenderer>
			{
				Name = "MeshRenderer",
				Priority = 800
			});
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x00062879 File Offset: 0x00060A79
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Mesh;
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x0006287C File Offset: 0x00060A7C
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Material;
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x0006287F File Offset: 0x00060A7F
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x00062882 File Offset: 0x00060A82
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x00062885 File Offset: 0x00060A85
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x00062888 File Offset: 0x00060A88
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

		// Token: 0x06000F8B RID: 3979 RVA: 0x00062920 File Offset: 0x00060B20
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
