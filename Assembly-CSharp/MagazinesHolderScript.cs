using System;
using Panik;
using UnityEngine;

// Token: 0x02000097 RID: 151
public class MagazinesHolderScript : MonoBehaviour
{
	// Token: 0x06000902 RID: 2306 RVA: 0x0000D1D1 File Offset: 0x0000B3D1
	private void Awake()
	{
		MagazinesHolderScript.instance = this;
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x0000D1D9 File Offset: 0x0000B3D9
	private void OnDestroy()
	{
		if (MagazinesHolderScript.instance == this)
		{
			MagazinesHolderScript.instance = null;
		}
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x0004A548 File Offset: 0x00048748
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

	// Token: 0x040008B5 RID: 2229
	public static MagazinesHolderScript instance;

	// Token: 0x040008B6 RID: 2230
	public GameObject readableMagazine;
}
