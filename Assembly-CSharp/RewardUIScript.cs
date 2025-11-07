using System;
using Panik;
using UnityEngine;

public class RewardUIScript : MonoBehaviour
{
	// Token: 0x060009E0 RID: 2528 RVA: 0x00043923 File Offset: 0x00041B23
	public static bool IsEnabled()
	{
		return RewardUIScript.instance.holder.activeSelf;
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x00043934 File Offset: 0x00041B34
	private void Awake()
	{
		RewardUIScript.instance = this;
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x0004393C File Offset: 0x00041B3C
	private void Start()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x0004394A File Offset: 0x00041B4A
	private void OnDestroy()
	{
		if (RewardUIScript.instance == this)
		{
			RewardUIScript.instance = null;
		}
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x00043960 File Offset: 0x00041B60
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
