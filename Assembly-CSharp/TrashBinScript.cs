using System;
using Panik;
using UnityEngine;

// Token: 0x02000088 RID: 136
public class TrashBinScript : MonoBehaviour
{
	// Token: 0x060008AA RID: 2218 RVA: 0x00049574 File Offset: 0x00047774
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

	// Token: 0x060008AB RID: 2219 RVA: 0x0000CE16 File Offset: 0x0000B016
	private void Awake()
	{
		TrashBinScript.instance = this;
		this.bounceScript = base.GetComponentInChildren<BounceScript>();
	}

	// Token: 0x04000863 RID: 2147
	public static TrashBinScript instance;

	// Token: 0x04000864 RID: 2148
	private BounceScript bounceScript;
}
