using System;
using UnityEngine;

namespace I2.Loc
{
	public abstract class LocalizeTargetDesc<T> : ILocalizeTargetDescriptor where T : ILocalizeTarget
	{
		// Token: 0x06000F65 RID: 3941 RVA: 0x00062659 File Offset: 0x00060859
		public override ILocalizeTarget CreateTarget(Localize cmp)
		{
			return ScriptableObject.CreateInstance<T>();
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00062665 File Offset: 0x00060865
		public override Type GetTargetType()
		{
			return typeof(T);
		}
	}
}
