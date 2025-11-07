using System;
using UnityEngine;

namespace I2.Loc
{
	public abstract class LocalizeTargetDesc<T> : ILocalizeTargetDescriptor where T : ILocalizeTarget
	{
		// Token: 0x06000F4E RID: 3918 RVA: 0x00061E7D File Offset: 0x0006007D
		public override ILocalizeTarget CreateTarget(Localize cmp)
		{
			return ScriptableObject.CreateInstance<T>();
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x00061E89 File Offset: 0x00060089
		public override Type GetTargetType()
		{
			return typeof(T);
		}
	}
}
