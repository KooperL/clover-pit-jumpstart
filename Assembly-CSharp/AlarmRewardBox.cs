using System;
using System.Collections;
using Panik;
using UnityEngine;

public class AlarmRewardBox : MonoBehaviour
{
	// Token: 0x060007C3 RID: 1987 RVA: 0x00032897 File Offset: 0x00030A97
	public static bool IsRinging()
	{
		return !(AlarmRewardBox.instance == null) && AlarmRewardBox.instance._ringing;
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x000328B2 File Offset: 0x00030AB2
	public static void AlarmRing()
	{
		if (AlarmRewardBox.instance == null)
		{
			return;
		}
		AlarmRewardBox.instance.LightFlashOn();
		AlarmRewardBox.instance.StartCoroutine(AlarmRewardBox.instance.RingCoroutine());
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x000328E1 File Offset: 0x00030AE1
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

	// Token: 0x060007C6 RID: 1990 RVA: 0x000328F0 File Offset: 0x00030AF0
	public void LightFlashOn()
	{
		this.light.enabled = true;
	}

	// Token: 0x060007C7 RID: 1991 RVA: 0x000328FE File Offset: 0x00030AFE
	public void LightFlashOff()
	{
		this.light.enabled = false;
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x0003290C File Offset: 0x00030B0C
	private void Awake()
	{
		AlarmRewardBox.instance = this;
	}

	// Token: 0x060007C9 RID: 1993 RVA: 0x00032914 File Offset: 0x00030B14
	private void Start()
	{
		this.light.enabled = false;
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x00032922 File Offset: 0x00030B22
	private void OnDestroy()
	{
		AlarmRewardBox.instance = null;
	}

	public static AlarmRewardBox instance;

	public Animator myAnimator;

	public Light light;

	private bool _ringing;
}
