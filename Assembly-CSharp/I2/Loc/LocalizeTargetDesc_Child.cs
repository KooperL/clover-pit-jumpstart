using System;

namespace I2.Loc
{
	public class LocalizeTargetDesc_Child : LocalizeTargetDesc<LocalizeTarget_UnityStandard_Child>
	{
		// Token: 0x06000F8D RID: 3981 RVA: 0x00062F49 File Offset: 0x00061149
		public override bool CanLocalize(Localize cmp)
		{
			return cmp.transform.childCount > 1;
		}
	}
}
