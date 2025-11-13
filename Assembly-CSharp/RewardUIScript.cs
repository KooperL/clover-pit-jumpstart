using System;
using Panik;
using UnityEngine;

public class RewardUIScript : MonoBehaviour
{
	// Token: 0x060009F4 RID: 2548 RVA: 0x00043F8B File Offset: 0x0004218B
	public static bool IsEnabled()
	{
		return RewardUIScript.instance.holder.activeSelf;
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x00043F9C File Offset: 0x0004219C
	private void Awake()
	{
		RewardUIScript.instance = this;
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x00043FA4 File Offset: 0x000421A4
	private void Start()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x00043FB2 File Offset: 0x000421B2
	private void OnDestroy()
	{
		if (RewardUIScript.instance == this)
		{
			RewardUIScript.instance = null;
		}
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x00043FC8 File Offset: 0x000421C8
	private void Update()
	{
		bool flag = RewardBoxScript.IsOpened() && !RewardBoxScript.HasPrize();
		if (Master.IsDemo)
		{
			flag = false;
		}
		else if (GameplayMaster.GetGamePhase() != GameplayMaster.GamePhase.preparation)
		{
			flag = false;
		}
		else if (DialogueScript.IsEnabled())
		{
			flag = false;
		}
		else if (ScreenMenuScript.IsEnabled())
		{
			flag = false;
		}
		else if (GameplayData.PrizeWasUsedGet())
		{
			flag = false;
		}
		if (RewardUIScript.IsEnabled() != flag)
		{
			this.holder.SetActive(flag);
			if (flag)
			{
				switch (RewardBoxScript.GetRewardKind())
				{
				case RewardBoxScript.RewardKind.DemoPrize:
					Debug.LogError("Demo prize is not meant to be shown here! Infact, we set shouldEnabled to false, here in this update, if we are in a demo.");
					break;
				case RewardBoxScript.RewardKind.DrawerKey0:
					this.drawerKeyHolder.gameObject.SetActive(true);
					this.doorKeyHolder.gameObject.SetActive(false);
					break;
				case RewardBoxScript.RewardKind.DrawerKey1:
					this.drawerKeyHolder.gameObject.SetActive(true);
					this.doorKeyHolder.gameObject.SetActive(false);
					break;
				case RewardBoxScript.RewardKind.DrawerKey2:
					this.drawerKeyHolder.gameObject.SetActive(true);
					this.doorKeyHolder.gameObject.SetActive(false);
					break;
				case RewardBoxScript.RewardKind.DrawerKey3:
					this.drawerKeyHolder.gameObject.SetActive(true);
					this.doorKeyHolder.gameObject.SetActive(false);
					break;
				case RewardBoxScript.RewardKind.DoorKey:
					this.drawerKeyHolder.gameObject.SetActive(false);
					this.doorKeyHolder.gameObject.SetActive(true);
					this.skeletonKeyRenderer.sharedMaterial = RewardBoxScript.DoorKeyDesiredMaterial_Get();
					break;
				}
			}
		}
		if (!RewardUIScript.IsEnabled())
		{
			return;
		}
		this.drawerKeyHolder.AddLocalYAngle(180f * Tick.Time);
		this.doorKeyHolder.AddLocalYAngle(180f * Tick.Time);
	}

	public static RewardUIScript instance;

	private const float ROT_SPD = 180f;

	public GameObject holder;

	public Transform drawerKeyHolder;

	public Transform doorKeyHolder;

	public MeshRenderer skeletonKeyRenderer;
}
