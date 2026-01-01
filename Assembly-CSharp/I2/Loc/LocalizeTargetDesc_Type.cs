using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001C6 RID: 454
	public class LocalizeTargetDesc_Type<T, G> : LocalizeTargetDesc<G> where T : global::UnityEngine.Object where G : LocalizeTarget<T>
	{
		// Token: 0x06001356 RID: 4950 RVA: 0x00015287 File Offset: 0x00013487
		public override bool CanLocalize(Localize cmp)
		{
			return cmp.GetComponent<T>() != null;
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x000808CC File Offset: 0x0007EACC
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
