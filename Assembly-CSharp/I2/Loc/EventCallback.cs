using System;
using UnityEngine;

namespace I2.Loc
{
	[Serializable]
	public class EventCallback
	{
		// Token: 0x06000E3C RID: 3644 RVA: 0x00057C00 File Offset: 0x00055E00
		public void Execute(global::UnityEngine.Object Sender = null)
		{
			if (this.HasCallback() && Application.isPlaying)
			{
				this.Target.gameObject.SendMessage(this.MethodName, Sender, SendMessageOptions.DontRequireReceiver);
			}
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x00057C29 File Offset: 0x00055E29
		public bool HasCallback()
		{
			return this.Target != null && !string.IsNullOrEmpty(this.MethodName);
		}

		public MonoBehaviour Target;

		public string MethodName = string.Empty;
	}
}
