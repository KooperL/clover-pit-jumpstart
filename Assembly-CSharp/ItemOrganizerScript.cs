using System;
using Panik;
using TMPro;
using UnityEngine;

public class ItemOrganizerScript : MonoBehaviour
{
	// Token: 0x060007E6 RID: 2022 RVA: 0x0003318E File Offset: 0x0003138E
	public static Transform GetOrganizerTransform(int index)
	{
		return ItemOrganizerScript.instance.organizerTransforms[index];
	}

	// Token: 0x060007E7 RID: 2023 RVA: 0x0003319C File Offset: 0x0003139C
	public static Transform GetDollTransform(int index)
	{
		return ItemOrganizerScript.instance.dollTransforms[index];
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x000331AA File Offset: 0x000313AA
	public static Transform GetDrawerTransform(int index)
	{
		return ItemOrganizerScript.instance.drawerTransforms[index];
	}

	// Token: 0x060007E9 RID: 2025 RVA: 0x000331B8 File Offset: 0x000313B8
	public static Transform GetStoreTransform(int index)
	{
		return ItemOrganizerScript.instance.storeTransforms[index];
	}

	// Token: 0x060007EA RID: 2026 RVA: 0x000331C6 File Offset: 0x000313C6
	public static int CharmsSlotN()
	{
		return 33;
	}

	// Token: 0x060007EB RID: 2027 RVA: 0x000331CA File Offset: 0x000313CA
	public static int SkeletonSlotsN()
	{
		return 5;
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x000331CD File Offset: 0x000313CD
	public static int DrawerSlotsN()
	{
		return 4;
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x000331D0 File Offset: 0x000313D0
	public static int StoreSlotsN()
	{
		return 4;
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x000331D4 File Offset: 0x000313D4
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

	// Token: 0x060007EF RID: 2031 RVA: 0x0003323C File Offset: 0x0003143C
	private void OnDestroy()
	{
		if (ItemOrganizerScript.instance == this)
		{
			ItemOrganizerScript.instance = null;
		}
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x00033254 File Offset: 0x00031454
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
