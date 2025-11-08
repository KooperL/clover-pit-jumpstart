using System;
using UnityEngine;

namespace I2.Loc
{
	[Serializable]
	public class EventCallback
	{
		// Token: 0x06000E25 RID: 3621 RVA: 0x00057424 File Offset: 0x00055624
		public void Execute(global::UnityEngine.Object Sender = null)
		{
			if (this.HasCallback() && Application.isPlaying)
			{
				this.Target.gameObject.SendMessage(this.MethodName, Sender, SendMessageOptions.DontRequireReceiver);
			}
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0005744D File Offset: 0x0005564D
		public bool HasCallback()
		{
			return this.Target != null && !string.IsNullOrEmpty(this.MethodName);
		}

		public MonoBehaviour Target;

		public string MethodName = string.Empty;
	}
}
