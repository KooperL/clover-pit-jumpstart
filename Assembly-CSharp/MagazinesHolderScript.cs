using System;
using Panik;
using UnityEngine;

public class MagazinesHolderScript : MonoBehaviour
{
	// Token: 0x060007F0 RID: 2032 RVA: 0x0003325F File Offset: 0x0003145F
	private void Awake()
	{
		MagazinesHolderScript.instance = this;
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x00033267 File Offset: 0x00031467
	private void OnDestroy()
	{
		if (MagazinesHolderScript.instance == this)
		{
			MagazinesHolderScript.instance = null;
		}
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x0003327C File Offset: 0x0003147C
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		bool flag = !MagazineUiScript.IsEnabled();
		if (this.readableMagazine.activeSelf != flag)
		{
			this.readableMagazine.SetActive(flag);
		}
	}

	public static MagazinesHolderScript instance;

	public GameObject readableMagazine;
}
