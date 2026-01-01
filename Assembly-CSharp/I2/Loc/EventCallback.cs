using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000191 RID: 401
	[Serializable]
	public class EventCallback
	{
		// Token: 0x060011D8 RID: 4568 RVA: 0x00014895 File Offset: 0x00012A95
		public void Execute(global::UnityEngine.Object Sender = null)
		{
			if (this.HasCallback() && Application.isPlaying)
			{
				this.Target.gameObject.SendMessage(this.MethodName, Sender, SendMessageOptions.DontRequireReceiver);
			}
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x000148BE File Offset: 0x00012ABE
		public bool HasCallback()
		{
			return this.Target != null && !string.IsNullOrEmpty(this.MethodName);
		}

		// Token: 0x040012AC RID: 4780
		public MonoBehaviour Target;

		// Token: 0x040012AD RID: 4781
		public string MethodName = string.Empty;
	}
}
