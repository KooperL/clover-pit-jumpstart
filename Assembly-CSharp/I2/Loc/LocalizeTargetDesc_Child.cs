using System;

namespace I2.Loc
{
	public class LocalizeTargetDesc_Child : LocalizeTargetDesc<LocalizeTarget_UnityStandard_Child>
	{
		// Token: 0x06000F76 RID: 3958 RVA: 0x0006276D File Offset: 0x0006096D
		public override bool CanLocalize(Localize cmp)
		{
			return cmp.transform.childCount > 1;
		}
	}
}
