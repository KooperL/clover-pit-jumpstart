using System;
using Panik;
using UnityEngine;

public class TrashBinScript : MonoBehaviour
{
	// Token: 0x060007AB RID: 1963 RVA: 0x000323C4 File Offset: 0x000305C4
	public static void TrashAnimation(bool playSound)
	{
		if (TrashBinScript.instance == null)
		{
			return;
		}
		TrashBinScript.instance.bounceScript.SetBounceScale(0.01f);
		if (playSound)
		{
			Sound.Play3D("SoundTrashHorror", TrashBinScript.instance.transform.position + new Vector3(0f, 2f, 0f), 10f, 1f, 1f, 1);
		}
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x00032439 File Offset: 0x00030639
	private void Awake()
	{
		TrashBinScript.instance = this;
		this.bounceScript = base.GetComponentInChildren<BounceScript>();
	}

	public static TrashBinScript instance;

	private BounceScript bounceScript;
}
