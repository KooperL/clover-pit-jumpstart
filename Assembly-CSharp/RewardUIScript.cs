using System;
using Panik;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class RewardUIScript : MonoBehaviour
{
	// Token: 0x06000B9C RID: 2972 RVA: 0x0000F8A8 File Offset: 0x0000DAA8
	public static bool IsEnabled()
	{
		return RewardUIScript.instance.holder.activeSelf;
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x0000F8B9 File Offset: 0x0000DAB9
	private void Awake()
	{
		RewardUIScript.instance = this;
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x0000F8C1 File Offset: 0x0000DAC1
	private void Start()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x0000F8CF File Offset: 0x0000DACF
	private void OnDestroy()
	{
		if (RewardUIScript.instance == this)
		{
			RewardUIScript.instance = null;
		}
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x0005D994 File Offset: 0x0005BB94
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

	// Token: 0x04000C3B RID: 3131
	public static RewardUIScript instance;

	// Token: 0x04000C3C RID: 3132
	private const float ROT_SPD = 180f;

	// Token: 0x04000C3D RID: 3133
	public GameObject holder;

	// Token: 0x04000C3E RID: 3134
	public Transform drawerKeyHolder;

	// Token: 0x04000C3F RID: 3135
	public Transform doorKeyHolder;

	// Token: 0x04000C40 RID: 3136
	public MeshRenderer skeletonKeyRenderer;
}
