using System;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTargetDesc_Type<T, G> : LocalizeTargetDesc<G> where T : global::UnityEngine.Object where G : LocalizeTarget<T>
	{
		// Token: 0x06000F68 RID: 3944 RVA: 0x00062679 File Offset: 0x00060879
		public override bool CanLocalize(Localize cmp)
		{
			return cmp.GetComponent<T>() != null;
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x0006268C File Offset: 0x0006088C
		public override ILocalizeTarget CreateTarget(Localize cmp)
		{
			T component = cmp.GetComponent<T>();
			if (component == null)
			{
				return null;
			}
			G g = ScriptableObject.CreateInstance<G>();
			g.mTarget = component;
			return g;
		}
	}
}
