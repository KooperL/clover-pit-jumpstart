using System;

namespace I2.Loc
{
	// Token: 0x020001CA RID: 458
	public class LocalizeTargetDesc_Child : LocalizeTargetDesc<LocalizeTarget_UnityStandard_Child>
	{
		// Token: 0x0600137B RID: 4987 RVA: 0x000153CF File Offset: 0x000135CF
		public override bool CanLocalize(Localize cmp)
		{
			return cmp.transform.childCount > 1;
		}
	}
}
