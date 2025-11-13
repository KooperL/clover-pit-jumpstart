using System;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class IsModdedScreenScript : MonoBehaviour
{
	// Token: 0x060008D0 RID: 2256 RVA: 0x0003A392 File Offset: 0x00038592
	public bool IsEnabled()
	{
		return !(IsModdedScreenScript.instance == null) && this.holder.activeSelf;
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x0003A3AE File Offset: 0x000385AE
	public static void UpdateState()
	{
		if (IsModdedScreenScript.instance == null)
		{
			return;
		}
		IsModdedScreenScript.instance.holder.SetActive(Master.IsModded());
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x0003A3D2 File Offset: 0x000385D2
	private void Awake()
	{
		IsModdedScreenScript.instance = this;
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x0003A3DA File Offset: 0x000385DA
	private void Start()
	{
		IsModdedScreenScript.UpdateState();
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x0003A3E1 File Offset: 0x000385E1
	private void OnEnable()
	{
		IsModdedScreenScript.UpdateState();
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x0003A3E8 File Offset: 0x000385E8
	private void OnDestroy()
	{
		if (IsModdedScreenScript.instance == this)
		{
			IsModdedScreenScript.instance = null;
		}
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x0003A3FD File Offset: 0x000385FD
	private void Update()
	{
		if (!this.IsEnabled())
		{
			return;
		}
		this.gearImage.transform.AddLocalZAngle(Tick.Time * 45f);
	}

	public static IsModdedScreenScript instance;

	public GameObject holder;

	public Image gearImage;
}
