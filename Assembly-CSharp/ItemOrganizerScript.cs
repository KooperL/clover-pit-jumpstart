using System;
using Panik;
using TMPro;
using UnityEngine;

// Token: 0x02000095 RID: 149
public class ItemOrganizerScript : MonoBehaviour
{
	// Token: 0x060008F1 RID: 2289 RVA: 0x0000D140 File Offset: 0x0000B340
	public static Transform GetOrganizerTransform(int index)
	{
		return ItemOrganizerScript.instance.organizerTransforms[index];
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x0000D14E File Offset: 0x0000B34E
	public static Transform GetDollTransform(int index)
	{
		return ItemOrganizerScript.instance.dollTransforms[index];
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x0000D15C File Offset: 0x0000B35C
	public static Transform GetDrawerTransform(int index)
	{
		return ItemOrganizerScript.instance.drawerTransforms[index];
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x0000D16A File Offset: 0x0000B36A
	public static Transform GetStoreTransform(int index)
	{
		return ItemOrganizerScript.instance.storeTransforms[index];
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x0000D178 File Offset: 0x0000B378
	public static int CharmsSlotN()
	{
		return 33;
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x0000D17C File Offset: 0x0000B37C
	public static int SkeletonSlotsN()
	{
		return 5;
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x0000D17F File Offset: 0x0000B37F
	public static int DrawerSlotsN()
	{
		return 4;
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0000D17F File Offset: 0x0000B37F
	public static int StoreSlotsN()
	{
		return 4;
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0004A330 File Offset: 0x00048530
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

	// Token: 0x060008FA RID: 2298 RVA: 0x0000D182 File Offset: 0x0000B382
	private void OnDestroy()
	{
		if (ItemOrganizerScript.instance == this)
		{
			ItemOrganizerScript.instance = null;
		}
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0004A398 File Offset: 0x00048598
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

	// Token: 0x040008A0 RID: 2208
	public static ItemOrganizerScript instance;

	// Token: 0x040008A1 RID: 2209
	public const int MAX_CHARMS_EQUIPPED = 33;

	// Token: 0x040008A2 RID: 2210
	public const int MAX_SKELETONS_EQUIPPED = 5;

	// Token: 0x040008A3 RID: 2211
	public const int MAX_DRAWERS = 4;

	// Token: 0x040008A4 RID: 2212
	public const int MAX_STORE_SLOTS = 4;

	// Token: 0x040008A5 RID: 2213
	public Transform[] organizerTransforms;

	// Token: 0x040008A6 RID: 2214
	public Transform[] dollTransforms;

	// Token: 0x040008A7 RID: 2215
	public Transform[] drawerTransforms;

	// Token: 0x040008A8 RID: 2216
	public Transform[] storeTransforms;

	// Token: 0x040008A9 RID: 2217
	public TextMeshProUGUI alarmText;

	// Token: 0x040008AA RID: 2218
	private int powerupsEquippedNumOld = -1;

	// Token: 0x040008AB RID: 2219
	private float alarmTextUpdateTimer;
}
