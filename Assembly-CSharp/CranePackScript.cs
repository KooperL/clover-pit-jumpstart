using System;
using Panik;
using UnityEngine;

public class CranePackScript : MonoBehaviour
{
	// Token: 0x060007CC RID: 1996 RVA: 0x000329EA File Offset: 0x00030BEA
	public static bool IsEnabled()
	{
		return !(CranePackScript.instance == null) && CranePackScript.instance.holder.activeSelf;
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x00032A0A File Offset: 0x00030C0A
	private void Awake()
	{
		CranePackScript.instance = this;
		this.holder.SetActive(false);
	}

	// Token: 0x060007CE RID: 1998 RVA: 0x00032A1E File Offset: 0x00030C1E
	private void OnDestroy()
	{
		if (CranePackScript.instance == this)
		{
			CranePackScript.instance = null;
		}
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x00032A34 File Offset: 0x00030C34
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

	public static CranePackScript instance;

	public GameObject holder;
}
