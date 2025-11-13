using System;
using System.Collections;
using Panik;
using UnityEngine;

public class AlarmRewardBox : MonoBehaviour
{
	// Token: 0x060007C3 RID: 1987 RVA: 0x0003294F File Offset: 0x00030B4F
	public static bool IsRinging()
	{
		return !(AlarmRewardBox.instance == null) && AlarmRewardBox.instance._ringing;
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x0003296A File Offset: 0x00030B6A
	public static void AlarmRing()
	{
		if (AlarmRewardBox.instance == null)
		{
			return;
		}
		AlarmRewardBox.instance.LightFlashOn();
		AlarmRewardBox.instance.StartCoroutine(AlarmRewardBox.instance.RingCoroutine());
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x00032999 File Offset: 0x00030B99
	private IEnumerator RingCoroutine()
	{
		this._ringing = true;
		this.myAnimator.SetTrigger("alarm");
		Sound.Play3D("SoundRewardBoxAlarm", base.transform.position, 20f, 1f, 1f, 1);
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

	// Token: 0x060007C6 RID: 1990 RVA: 0x000329A8 File Offset: 0x00030BA8
	public void LightFlashOn()
	{
		this.light.enabled = true;
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x000329B6 File Offset: 0x00030BB6
	public void LightFlashOff()
	{
		this.light.enabled = false;
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x000329C4 File Offset: 0x00030BC4
	private void Awake()
	{
		AlarmRewardBox.instance = this;
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x000329CC File Offset: 0x00030BCC
	private void Start()
	{
		this.light.enabled = false;
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x000329DA File Offset: 0x00030BDA
	private void OnDestroy()
	{
		AlarmRewardBox.instance = null;
	}

	public static AlarmRewardBox instance;

	public Animator myAnimator;

	public Light light;

	private bool _ringing;
}
