using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200003E RID: 62
public class AnimationCallbacker : MonoBehaviour
{
	// Token: 0x0600040B RID: 1035 RVA: 0x00008E08 File Offset: 0x00007008
	public void CallbackNormal()
	{
		UnityEvent unityEvent = this.callbackNormal;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x0600040C RID: 1036 RVA: 0x00008E1A File Offset: 0x0000701A
	public void CallbackFloat(float value)
	{
		UnityEvent<float> unityEvent = this.callbackFloat;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(value);
	}

	// Token: 0x0600040D RID: 1037 RVA: 0x00008E2D File Offset: 0x0000702D
	public void CallbackBool(bool value)
	{
		UnityEvent<bool> unityEvent = this.callbackBool;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(value);
	}

	// Token: 0x0600040E RID: 1038 RVA: 0x00008E40 File Offset: 0x00007040
	public void CallbackInt(int value)
	{
		UnityEvent<int> unityEvent = this.callbackInt;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(value);
	}

	// Token: 0x0600040F RID: 1039 RVA: 0x00008E53 File Offset: 0x00007053
	public void CallbackString(string value)
	{
		UnityEvent<string> unityEvent = this.callbackString;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(value);
	}

	// Token: 0x0400038D RID: 909
	public UnityEvent callbackNormal;

	// Token: 0x0400038E RID: 910
	public UnityEvent<float> callbackFloat;

	// Token: 0x0400038F RID: 911
	public UnityEvent<bool> callbackBool;

	// Token: 0x04000390 RID: 912
	public UnityEvent<int> callbackInt;

	// Token: 0x04000391 RID: 913
	public UnityEvent<string> callbackString;
}
