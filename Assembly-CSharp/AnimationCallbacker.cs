using System;
using UnityEngine;
using UnityEngine.Events;

public class AnimationCallbacker : MonoBehaviour
{
	// Token: 0x060003A5 RID: 933 RVA: 0x000198CF File Offset: 0x00017ACF
	public void CallbackNormal()
	{
		UnityEvent unityEvent = this.callbackNormal;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x000198E1 File Offset: 0x00017AE1
	public void CallbackFloat(float value)
	{
		UnityEvent<float> unityEvent = this.callbackFloat;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(value);
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x000198F4 File Offset: 0x00017AF4
	public void CallbackBool(bool value)
	{
		UnityEvent<bool> unityEvent = this.callbackBool;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(value);
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x00019907 File Offset: 0x00017B07
	public void CallbackInt(int value)
	{
		UnityEvent<int> unityEvent = this.callbackInt;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(value);
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x0001991A File Offset: 0x00017B1A
	public void CallbackString(string value)
	{
		UnityEvent<string> unityEvent = this.callbackString;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(value);
	}

	public UnityEvent callbackNormal;

	public UnityEvent<float> callbackFloat;

	public UnityEvent<bool> callbackBool;

	public UnityEvent<int> callbackInt;

	public UnityEvent<string> callbackString;
}
