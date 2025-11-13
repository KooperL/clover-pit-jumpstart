using System;
using Panik;
using UnityEngine;

public class MagazinesHolderScript : MonoBehaviour
{
	// Token: 0x060007F7 RID: 2039 RVA: 0x00033447 File Offset: 0x00031647
	private void Awake()
	{
		MagazinesHolderScript.instance = this;
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x0003344F File Offset: 0x0003164F
	private void OnDestroy()
	{
		if (MagazinesHolderScript.instance == this)
		{
			MagazinesHolderScript.instance = null;
		}
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x00033464 File Offset: 0x00031664
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
