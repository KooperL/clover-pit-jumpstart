using System;

namespace I2.Loc
{
	// Token: 0x020001C4 RID: 452
	public abstract class ILocalizeTargetDescriptor
	{
		// Token: 0x0600134F RID: 4943
		public abstract bool CanLocalize(Localize cmp);

		// Token: 0x06001350 RID: 4944
		public abstract ILocalizeTarget CreateTarget(Localize cmp);

		// Token: 0x06001351 RID: 4945
		public abstract Type GetTargetType();

		// Token: 0x04001394 RID: 5012
		public string Name;

		// Token: 0x04001395 RID: 5013
		public int Priority;
	}
}
