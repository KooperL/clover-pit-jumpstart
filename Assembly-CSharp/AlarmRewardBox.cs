using System;
using System.Collections;
using Panik;
using UnityEngine;

// Token: 0x0200008D RID: 141
public class AlarmRewardBox : MonoBehaviour
{
	// Token: 0x060008C8 RID: 2248 RVA: 0x0000CF7A File Offset: 0x0000B17A
	public static bool IsRinging()
	{
		return !(AlarmRewardBox.instance == null) && AlarmRewardBox.instance._ringing;
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0000CF95 File Offset: 0x0000B195
	public static void AlarmRing()
	{
		if (AlarmRewardBox.instance == null)
		{
			return;
		}
		AlarmRewardBox.instance.LightFlashOn();
		AlarmRewardBox.instance.StartCoroutine(AlarmRewardBox.instance.RingCoroutine());
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x0000CFC4 File Offset: 0x0000B1C4
	private IEnumerator RingCoroutine()
	{
		this._ringing = true;
		this.myAnimator.SetTrigger("alarm");
		Sound.Play3D("SoundRewardBoxAlarm", base.transform.position, 20f, 1f, 1f, AudioRolloffMode.Linear);
		float timer = 1f;
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			yield return null;
		}
		this._ringing = false;
		this.myAnimator.SetTrigger("idle");
		yield break;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x0000CFD3 File Offset: 0x0000B1D3
	public void LightFlashOn()
	{
		this.light.enabled = true;
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0000CFE1 File Offset: 0x0000B1E1
	public void LightFlashOff()
	{
		this.light.enabled = false;
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x0000CFEF File Offset: 0x0000B1EF
	private void Awake()
	{
		AlarmRewardBox.instance = this;
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x0000CFE1 File Offset: 0x0000B1E1
	private void Start()
	{
		this.light.enabled = false;
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x0000CFF7 File Offset: 0x0000B1F7
	private void OnDestroy()
	{
		AlarmRewardBox.instance = null;
	}

	// Token: 0x04000883 RID: 2179
	public static AlarmRewardBox instance;

	// Token: 0x04000884 RID: 2180
	public Animator myAnimator;

	// Token: 0x04000885 RID: 2181
	public Light light;

	// Token: 0x04000886 RID: 2182
	private bool _ringing;
}
