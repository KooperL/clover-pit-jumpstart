using System;
using UnityEngine;
using UnityEngine.Events;

public class AnimationCallbacker : MonoBehaviour
{
	// Token: 0x060003A7 RID: 935 RVA: 0x0001980D File Offset: 0x00017A0D
	public void CallbackNormal()
	{
		UnityEvent unityEvent = this.callbackNormal;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x0001981F File Offset: 0x00017A1F
	public void CallbackFloat(float value)
	{
		UnityEvent<float> unityEvent = this.callbackFloat;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(value);
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x00019832 File Offset: 0x00017A32
	public void CallbackBool(bool value)
	{
		UnityEvent<bool> unityEvent = this.callbackBool;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(value);
	}

	// Token: 0x060003AA RID: 938 RVA: 0x00019845 File Offset: 0x00017A45
	public void CallbackInt(int value)
	{
		UnityEvent<int> unityEvent = this.callbackInt;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke(value);
	}

	// Token: 0x060003AB RID: 939 RVA: 0x00019858 File Offset: 0x00017A58
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
