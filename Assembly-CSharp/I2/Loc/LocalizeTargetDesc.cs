using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001C5 RID: 453
	public abstract class LocalizeTargetDesc<T> : ILocalizeTargetDescriptor where T : ILocalizeTarget
	{
		// Token: 0x06001353 RID: 4947 RVA: 0x00015267 File Offset: 0x00013467
		public override ILocalizeTarget CreateTarget(Localize cmp)
		{
			return ScriptableObject.CreateInstance<T>();
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x00015273 File Offset: 0x00013473
		public override Type GetTargetType()
		{
			return typeof(T);
		}
	}
}
