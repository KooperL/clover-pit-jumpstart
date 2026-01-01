using System;
using Panik;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class EndingAreaScript : MonoBehaviour
{
	// Token: 0x06000411 RID: 1041 RVA: 0x0002D144 File Offset: 0x0002B344
	public static void DetermineControlPanel()
	{
		Collider[] array;
		if (GameplayData.IsInGoodEndingCondition(false))
		{
			EndingAreaScript.instance.slotObj.SetActive(false);
			EndingAreaScript.instance.planciaObj.SetActive(true);
			array = EndingAreaScript.instance.slotColliders;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = false;
			}
			array = EndingAreaScript.instance.planciaColliders;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = true;
			}
			return;
		}
		EndingAreaScript.instance.slotObj.SetActive(true);
		EndingAreaScript.instance.planciaObj.SetActive(false);
		array = EndingAreaScript.instance.slotColliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = true;
		}
		array = EndingAreaScript.instance.planciaColliders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = false;
		}
	}

	// Token: 0x06000412 RID: 1042 RVA: 0x00008E66 File Offset: 0x00007066
	private void Awake()
	{
		EndingAreaScript.instance = this;
	}

	// Token: 0x06000413 RID: 1043 RVA: 0x0002D224 File Offset: 0x0002B424
	private void Update()
	{
		this.liftTr.localPosition = new Vector3(Util.AngleSin(Tick.PassedTime * 45f) * 0.1f, 0f, Util.AngleSin(Tick.PassedTime * 75f) * 0.1f);
	}

	// Token: 0x04000392 RID: 914
	public static EndingAreaScript instance;

	// Token: 0x04000393 RID: 915
	public Transform spawnPoint;

	// Token: 0x04000394 RID: 916
	public Transform liftTr;

	// Token: 0x04000395 RID: 917
	public GameObject slotObj;

	// Token: 0x04000396 RID: 918
	public Collider[] slotColliders;

	// Token: 0x04000397 RID: 919
	public GameObject planciaObj;

	// Token: 0x04000398 RID: 920
	public Collider[] planciaColliders;
}
