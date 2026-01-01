using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001C3 RID: 451
	public abstract class LocalizeTarget<T> : ILocalizeTarget where T : global::UnityEngine.Object
	{
		// Token: 0x0600134D RID: 4941 RVA: 0x00080840 File Offset: 0x0007EA40
		public override bool IsValid(Localize cmp)
		{
			if (this.mTarget != null)
			{
				Component component = this.mTarget as Component;
				if (component != null && component.gameObject != cmp.gameObject)
				{
					this.mTarget = default(T);
				}
			}
			if (this.mTarget == null)
			{
				this.mTarget = cmp.GetComponent<T>();
			}
			return this.mTarget != null;
		}

		// Token: 0x04001393 RID: 5011
		public T mTarget;
	}
}
