using System;
using Panik;
using UnityEngine;

// Token: 0x0200008F RID: 143
public class CranePackScript : MonoBehaviour
{
	// Token: 0x060008D7 RID: 2263 RVA: 0x0000D016 File Offset: 0x0000B216
	public static bool IsEnabled()
	{
		return !(CranePackScript.instance == null) && CranePackScript.instance.holder.activeSelf;
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x0000D036 File Offset: 0x0000B236
	private void Awake()
	{
		CranePackScript.instance = this;
		this.holder.SetActive(false);
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x0000D04A File Offset: 0x0000B24A
	private void OnDestroy()
	{
		if (CranePackScript.instance == this)
		{
			CranePackScript.instance = null;
		}
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x00049CDC File Offset: 0x00047EDC
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		this.holder.transform.AddLocalYAngle(Tick.Time * 90f);
		bool flag = !GameplayMaster.IsCustomSeed() && DeckBoxScript.ItsMemoryCardTime() && Data.game.RunModifier_UnlockedTotalNumber() <= 0;
		if (this.holder.activeSelf != flag)
		{
			this.holder.SetActive(flag);
		}
	}

	// Token: 0x0400088B RID: 2187
	public static CranePackScript instance;

	// Token: 0x0400088C RID: 2188
	public GameObject holder;
}
