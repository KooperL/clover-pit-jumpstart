using System;
using Panik;
using UnityEngine;

public class TrashBinScript : MonoBehaviour
{
	// Token: 0x060007AB RID: 1963 RVA: 0x0003230C File Offset: 0x0003050C
	public static void TrashAnimation(bool playSound)
	{
		if (TrashBinScript.instance == null)
		{
			return;
		}
		TrashBinScript.instance.bounceScript.SetBounceScale(0.01f);
		if (playSound)
		{
			Sound.Play3D("SoundTrashHorror", TrashBinScript.instance.transform.position + new Vector3(0f, 2f, 0f), 10f, 1f, 1f, AudioRolloffMode.Linear);
		}
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x00032381 File Offset: 0x00030581
	private void Awake()
	{
		TrashBinScript.instance = this;
		this.bounceScript = base.GetComponentInChildren<BounceScript>();
	}

	public static TrashBinScript instance;

	private BounceScript bounceScript;
}
