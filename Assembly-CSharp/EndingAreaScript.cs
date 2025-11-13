using System;
using Panik;
using UnityEngine;

public class EndingAreaScript : MonoBehaviour
{
	// Token: 0x060003AB RID: 939 RVA: 0x00019938 File Offset: 0x00017B38
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

	// Token: 0x060003AC RID: 940 RVA: 0x00019A16 File Offset: 0x00017C16
	private void Awake()
	{
		EndingAreaScript.instance = this;
	}

	// Token: 0x060003AD RID: 941 RVA: 0x00019A20 File Offset: 0x00017C20
	private void Update()
	{
		this.liftTr.localPosition = new Vector3(Util.AngleSin(Tick.PassedTime * 45f) * 0.1f, 0f, Util.AngleSin(Tick.PassedTime * 75f) * 0.1f);
	}

	public static EndingAreaScript instance;

	public Transform spawnPoint;

	public Transform liftTr;

	public GameObject slotObj;

	public Collider[] slotColliders;

	public GameObject planciaObj;

	public Collider[] planciaColliders;
}
