using System;
using Panik;
using TMPro;
using UnityEngine;

public class ItemOrganizerScript : MonoBehaviour
{
	// Token: 0x060007DF RID: 2015 RVA: 0x00032FA7 File Offset: 0x000311A7
	public static Transform GetOrganizerTransform(int index)
	{
		return ItemOrganizerScript.instance.organizerTransforms[index];
	}

	// Token: 0x060007E0 RID: 2016 RVA: 0x00032FB5 File Offset: 0x000311B5
	public static Transform GetDollTransform(int index)
	{
		return ItemOrganizerScript.instance.dollTransforms[index];
	}

	// Token: 0x060007E1 RID: 2017 RVA: 0x00032FC3 File Offset: 0x000311C3
	public static Transform GetDrawerTransform(int index)
	{
		return ItemOrganizerScript.instance.drawerTransforms[index];
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x00032FD1 File Offset: 0x000311D1
	public static Transform GetStoreTransform(int index)
	{
		return ItemOrganizerScript.instance.storeTransforms[index];
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x00032FDF File Offset: 0x000311DF
	public static int CharmsSlotN()
	{
		return 33;
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00032FE3 File Offset: 0x000311E3
	public static int SkeletonSlotsN()
	{
		return 5;
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x00032FE6 File Offset: 0x000311E6
	public static int DrawerSlotsN()
	{
		return 4;
	}

	// Token: 0x060007E6 RID: 2022 RVA: 0x00032FE9 File Offset: 0x000311E9
	public static int StoreSlotsN()
	{
		return 4;
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x00032FEC File Offset: 0x000311EC
	private void Awake()
	{
		ItemOrganizerScript.instance = this;
		if (this.organizerTransforms.Length < 33)
		{
			Debug.LogError("ItemOrganizerScript.Awake() - organizerTransforms.Length < MAX_CHARMS_EQUIPPED");
		}
		if (this.dollTransforms.Length < 5)
		{
			Debug.LogError("ItemOrganizerScript.Awake() - dollTransforms.Length < MAX_SKELETONS_EQUIPPED");
		}
		if (this.drawerTransforms.Length < 4)
		{
			Debug.LogError("ItemOrganizerScript.Awake() - drawerTransforms.Length < MAX_DRAWERS");
		}
		if (this.storeTransforms.Length < 4)
		{
			Debug.LogError("ItemOrganizerScript.Awake() - storeTransforms.Length < MAX_STORE_SLOTS");
		}
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x00033054 File Offset: 0x00031254
	private void OnDestroy()
	{
		if (ItemOrganizerScript.instance == this)
		{
			ItemOrganizerScript.instance = null;
		}
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x0003306C File Offset: 0x0003126C
	private void Update()
	{
		int count = PowerupScript.list_EquippedNormal.Count;
		this.alarmTextUpdateTimer -= Tick.Time;
		if (this.powerupsEquippedNumOld != count || this.alarmTextUpdateTimer <= 0f)
		{
			this.alarmTextUpdateTimer += 1f;
			this.powerupsEquippedNumOld = count;
			this.alarmText.text = "<sprite name=\"Horse Shoe\">: " + count.ToString() + " / " + GameplayData.MaxEquippablePowerupsGet(true).ToString();
		}
	}

	public static ItemOrganizerScript instance;

	public const int MAX_CHARMS_EQUIPPED = 33;

	public const int MAX_SKELETONS_EQUIPPED = 5;

	public const int MAX_DRAWERS = 4;

	public const int MAX_STORE_SLOTS = 4;

	public Transform[] organizerTransforms;

	public Transform[] dollTransforms;

	public Transform[] drawerTransforms;

	public Transform[] storeTransforms;

	public TextMeshProUGUI alarmText;

	private int powerupsEquippedNumOld = -1;

	private float alarmTextUpdateTimer;
}
