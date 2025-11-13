using System;
using UnityEngine;

namespace I2.Loc
{
	public abstract class LocalizeTarget<T> : ILocalizeTarget where T : global::UnityEngine.Object
	{
		// Token: 0x06000F5F RID: 3935 RVA: 0x000625C0 File Offset: 0x000607C0
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

		public T mTarget;
	}
}
