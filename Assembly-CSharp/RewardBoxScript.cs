using System;
using System.Collections;
using System.Text;
using Panik;
using TMPro;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class RewardBoxScript : MonoBehaviour
{
	// Token: 0x0600076E RID: 1902 RVA: 0x0000C197 File Offset: 0x0000A397
	public static bool IsOpened()
	{
		return !(RewardBoxScript.instance == null) && RewardBoxScript.instance._opened;
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x0000C1B2 File Offset: 0x0000A3B2
	public static bool IsOpening()
	{
		return !(RewardBoxScript.instance == null) && RewardBoxScript.instance._isOpening;
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x0003D6B4 File Offset: 0x0003B8B4
	public static void Open()
	{
		if (RewardBoxScript.instance == null)
		{
			Debug.LogError("RewardBoxScript instance is null");
			return;
		}
		if (RewardBoxScript.instance._opened)
		{
			return;
		}
		RewardBoxScript.instance._opened = true;
		GameplayData.RewardBoxSetOpened();
		Data.SaveGame(Data.GameSavingReason.rewardBox_Opened, -1);
		RewardBoxScript.instance.StartCoroutine(RewardBoxScript.instance.OpenCoroutine());
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x0000C1CD File Offset: 0x0000A3CD
	private IEnumerator OpenCoroutine()
	{
		this._isOpening = true;
		float timer = 0f;
		this.effectScript.gameObject.SetActive(true);
		Sound.Play3D("SoundRewardBoxConfetti", base.transform.position, 20f, 1f, 1f, AudioRolloffMode.Linear);
		while (timer < 1f)
		{
			timer += Tick.Time;
			yield return null;
		}
		Sound.Play3D("SoundRewardBoxTrigger", base.transform.position, 20f, 1f, 1f, AudioRolloffMode.Linear);
		while (timer < 0.5f)
		{
			timer += Tick.Time;
			yield return null;
		}
		float animationGravity = -360f;
		float animationSpeed = 0f;
		Vector3 doorEulers = RewardBoxScript.doorClosedEulers;
		int bouncedTimes = 0;
		while (bouncedTimes < 3 || doorEulers.y > -180f)
		{
			animationSpeed += animationGravity * Tick.Time;
			animationSpeed = Mathf.Clamp(animationSpeed, -360f, 360f);
			doorEulers.y += animationSpeed * Tick.Time;
			if (doorEulers.y < -180f && animationSpeed < 0f)
			{
				doorEulers.y = -180f;
				int num = bouncedTimes;
				bouncedTimes = num + 1;
				animationSpeed = 360f / (float)(bouncedTimes + bouncedTimes);
				Sound.Play3D("SoundRewardBoxDoorHit", base.transform.position, 20f, 1f - (float)bouncedTimes * 0.2f, 1f, AudioRolloffMode.Linear);
			}
			this.doorTr.localEulerAngles = doorEulers;
			yield return null;
		}
		this.SetModelAsOpened();
		this._isOpening = false;
		yield break;
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x0000C1DC File Offset: 0x0000A3DC
	public void SetModelAsOpened()
	{
		this.doorTr.localEulerAngles = RewardBoxScript.doorOpenedEulers;
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0003D714 File Offset: 0x0003B914
	public static void Pick()
	{
		if (RewardBoxScript.instance == null)
		{
			Debug.LogError("RewardBoxScript instance is null");
			return;
		}
		if (!RewardBoxScript.IsOpened())
		{
			return;
		}
		if (!RewardBoxScript.HasPrize())
		{
			return;
		}
		RewardBoxScript.instance.prizeOfGame.SetActive(false);
		Sound.Play("SoundRewardBoxPickup", 1f, 1f);
		GameplayData.RewardBoxPickupPrize();
		Data.SaveGame(Data.GameSavingReason.rewardBox_PickedUpReward, -1);
		RewardBoxScript.RefreshText_ToDeadlineDebtToReach();
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x0000C1EE File Offset: 0x0000A3EE
	private static void DilogueDeathCallback()
	{
		GameplayMaster.instance.DieTry(GameplayMaster.DeathStep.lookAtTrapdoor, false);
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x0000C1FD File Offset: 0x0000A3FD
	public static bool HasPrize()
	{
		return !(RewardBoxScript.instance == null) && RewardBoxScript.instance.prizeOfGame.activeSelf;
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x0000C222 File Offset: 0x0000A422
	public static RewardBoxScript.RewardKind GetRewardKind()
	{
		if (RewardBoxScript.instance == null)
		{
			return RewardBoxScript.RewardKind.Undefined;
		}
		return RewardBoxScript.instance.rewardKind;
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x0003D784 File Offset: 0x0003B984
	public static void RefreshText_ToDeadlineDebtToReach()
	{
		if (RewardBoxScript.instance == null)
		{
			return;
		}
		GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
		if (!GameplayData.RewardTimeToShowAmmount())
		{
			RewardBoxScript.instance.monitorText.text = "???";
			return;
		}
		if (RewardBoxScript.IsOpened() || !RewardBoxScript.HasPrize())
		{
			RewardBoxScript.instance.monitorText.fontStyle = FontStyles.Strikethrough;
		}
		RewardBoxScript.instance.monitorText.text = "> " + GameplayData.GetRewardDeadlineDebt().ToStringSmart() + "<sprite name=\"CoinSymbolOrange64\">";
		if (gamePhase != GameplayMaster.GamePhase.intro)
		{
			Sound.Play3D("SoundRewardBoxTextUpdate", RewardBoxScript.instance.transform.position, 30f, 1f, 1f, AudioRolloffMode.Linear);
		}
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x0003D838 File Offset: 0x0003BA38
	private void SkeletonKeyMaterialUpdate()
	{
		Material material = RewardBoxScript.DoorKeyDesiredMaterial_Get();
		if (this.doorKeyMeshRenderer.sharedMaterial != material)
		{
			this.doorKeyMeshRenderer.sharedMaterial = material;
			this.doorKeyHolyEffect.SetActive(material == this.doorKeyMaterial_Good);
		}
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x0003D884 File Offset: 0x0003BA84
	public static Material DoorKeyDesiredMaterial_Get()
	{
		if (RewardBoxScript.instance == null)
		{
			return null;
		}
		Material material = RewardBoxScript.instance.doorKeyMaterial_Default;
		if (GameplayData.CanGetSkeletonKey())
		{
			material = RewardBoxScript.instance.doorKeyMaterial_Evil;
			if (GameplayData.NineNineNine_IsTime())
			{
				material = RewardBoxScript.instance.doorKeyMaterial_Good;
			}
		}
		return material;
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x0003D8D0 File Offset: 0x0003BAD0
	public static void Initialize(bool isNewGame)
	{
		if (isNewGame)
		{
			switch (DrawersScript.GetDrawersUnlockedNum())
			{
			case 0:
				RewardBoxScript.instance.rewardKind = RewardBoxScript.RewardKind.DrawerKey0;
				break;
			case 1:
				RewardBoxScript.instance.rewardKind = RewardBoxScript.RewardKind.DrawerKey1;
				break;
			case 2:
				RewardBoxScript.instance.rewardKind = RewardBoxScript.RewardKind.DrawerKey2;
				break;
			case 3:
				RewardBoxScript.instance.rewardKind = RewardBoxScript.RewardKind.DrawerKey3;
				break;
			case 4:
				RewardBoxScript.instance.rewardKind = RewardBoxScript.RewardKind.DoorKey;
				break;
			default:
				Debug.LogError("RewardBoxScript: Undefined reward kind");
				break;
			}
			GameplayData.RewardKind = RewardBoxScript.instance.rewardKind;
		}
		else
		{
			RewardBoxScript.instance.rewardKind = GameplayData.RewardKind;
		}
		GameObject[] array = RewardBoxScript.instance.prizes;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		switch (RewardBoxScript.GetRewardKind())
		{
		case RewardBoxScript.RewardKind.DemoPrize:
			RewardBoxScript.instance.prizeOfGame = RewardBoxScript.instance.prizes[0];
			break;
		case RewardBoxScript.RewardKind.DrawerKey0:
			RewardBoxScript.instance.prizeOfGame = RewardBoxScript.instance.prizes[1];
			break;
		case RewardBoxScript.RewardKind.DrawerKey1:
			RewardBoxScript.instance.prizeOfGame = RewardBoxScript.instance.prizes[1];
			break;
		case RewardBoxScript.RewardKind.DrawerKey2:
			RewardBoxScript.instance.prizeOfGame = RewardBoxScript.instance.prizes[1];
			break;
		case RewardBoxScript.RewardKind.DrawerKey3:
			RewardBoxScript.instance.prizeOfGame = RewardBoxScript.instance.prizes[1];
			break;
		case RewardBoxScript.RewardKind.DoorKey:
			RewardBoxScript.instance.prizeOfGame = RewardBoxScript.instance.prizes[2];
			break;
		default:
			Debug.LogError("RewardBoxScript: Undefined reward kind");
			break;
		}
		RewardBoxScript.instance.prizeMeshRenderer = RewardBoxScript.instance.prizeOfGame.GetComponentInChildren<MeshRenderer>(true);
		if (Master.IsDemo && Data.game.demoVoucherUnlocked)
		{
			RewardBoxScript.instance.prizeMeshRenderer.sharedMaterial = RewardBoxScript.instance.demoPrizeUsedMaterial;
		}
		RewardBoxScript.instance.prizeDefaultMaterial = RewardBoxScript.instance.prizeMeshRenderer.sharedMaterial;
		RewardBoxScript.instance.prizeOfGame.SetActive(GameplayData.RewardBoxHasPrize());
		RewardBoxScript.instance._opened = GameplayData.RewardBoxIsOpened();
		if (RewardBoxScript.instance._opened)
		{
			RewardBoxScript.instance.SetModelAsOpened();
		}
		RewardBoxScript.RefreshText_ToDeadlineDebtToReach();
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x0000C23D File Offset: 0x0000A43D
	private void Awake()
	{
		RewardBoxScript.instance = this;
		this.myMenuElement = base.GetComponentInChildren<DiegeticMenuElement>();
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x0000C251 File Offset: 0x0000A451
	private void Start()
	{
		this.doorKeyHolyEffect.SetActive(false);
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x0000C25F File Offset: 0x0000A45F
	private void OnDestroy()
	{
		if (RewardBoxScript.instance == this)
		{
			RewardBoxScript.instance = null;
		}
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x0003DAF4 File Offset: 0x0003BCF4
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		if (RewardBoxScript.IsOpened() && RewardBoxScript.HasPrize() && this.prizeMeshRenderer != null)
		{
			Material material = ((Util.AngleSin(Tick.PassedTime * 1440f) > 0.5f) ? this.prizeFlashMaterial : this.prizeDefaultMaterial);
			if (this.prizeMeshRenderer.sharedMaterial != material)
			{
				this.prizeMeshRenderer.sharedMaterial = material;
			}
		}
		if (this.rewardKind == RewardBoxScript.RewardKind.DoorKey)
		{
			this.SkeletonKeyMaterialUpdate();
		}
		if (this.rewardKind == RewardBoxScript.RewardKind.DoorKey)
		{
			if (GameplayData.RewardBoxHasPrize())
			{
				if (!this.doorKeyBonesText.enabled)
				{
					this.doorKeyBonesText.enabled = true;
				}
				int count = PowerupScript.list_EquippedSkeleton.Count;
				if (count != this.bonesOld)
				{
					this.bonesSB.Clear();
					for (int i = 0; i < 5; i++)
					{
						if (i < count)
						{
							this.bonesSB.Append("<sprite name=\"BoneFull\">");
						}
						else
						{
							this.bonesSB.Append("<sprite name=\"BoneEmpty\">");
						}
					}
					this.doorKeyBonesText.text = this.bonesSB.ToString();
					this.bonesOld = count;
					return;
				}
			}
			else if (this.doorKeyBonesText.enabled)
			{
				this.doorKeyBonesText.enabled = false;
				return;
			}
		}
		else if (this.doorKeyBonesText.enabled)
		{
			this.doorKeyBonesText.enabled = false;
		}
	}

	// Token: 0x04000678 RID: 1656
	public static RewardBoxScript instance;

	// Token: 0x04000679 RID: 1657
	private static Vector3 doorClosedEulers = Vector3.zero;

	// Token: 0x0400067A RID: 1658
	private static Vector3 doorOpenedEulers = new Vector3(0f, -180f, 0f);

	// Token: 0x0400067B RID: 1659
	public Transform doorTr;

	// Token: 0x0400067C RID: 1660
	public GameObject[] prizes;

	// Token: 0x0400067D RID: 1661
	private DiegeticMenuElement myMenuElement;

	// Token: 0x0400067E RID: 1662
	public TextMeshProUGUI monitorText;

	// Token: 0x0400067F RID: 1663
	[NonSerialized]
	public MeshRenderer prizeMeshRenderer;

	// Token: 0x04000680 RID: 1664
	private Material prizeDefaultMaterial;

	// Token: 0x04000681 RID: 1665
	public Material prizeFlashMaterial;

	// Token: 0x04000682 RID: 1666
	public Material demoPrizeUsedMaterial;

	// Token: 0x04000683 RID: 1667
	public MeshRenderer doorKeyMeshRenderer;

	// Token: 0x04000684 RID: 1668
	public Material doorKeyMaterial_Default;

	// Token: 0x04000685 RID: 1669
	public Material doorKeyMaterial_Evil;

	// Token: 0x04000686 RID: 1670
	public Material doorKeyMaterial_Good;

	// Token: 0x04000687 RID: 1671
	public GameObject doorKeyHolyEffect;

	// Token: 0x04000688 RID: 1672
	public TextMeshPro doorKeyBonesText;

	// Token: 0x04000689 RID: 1673
	private GameObject prizeOfGame;

	// Token: 0x0400068A RID: 1674
	private bool _opened;

	// Token: 0x0400068B RID: 1675
	private bool _isOpening;

	// Token: 0x0400068C RID: 1676
	public EffectScript effectScript;

	// Token: 0x0400068D RID: 1677
	private RewardBoxScript.RewardKind rewardKind = RewardBoxScript.RewardKind.Undefined;

	// Token: 0x0400068E RID: 1678
	private int bonesOld = -1;

	// Token: 0x0400068F RID: 1679
	private StringBuilder bonesSB = new StringBuilder();

	// Token: 0x02000067 RID: 103
	public enum RewardKind
	{
		// Token: 0x04000691 RID: 1681
		DemoPrize,
		// Token: 0x04000692 RID: 1682
		DrawerKey0,
		// Token: 0x04000693 RID: 1683
		DrawerKey1,
		// Token: 0x04000694 RID: 1684
		DrawerKey2,
		// Token: 0x04000695 RID: 1685
		DrawerKey3,
		// Token: 0x04000696 RID: 1686
		DoorKey,
		// Token: 0x04000697 RID: 1687
		Count,
		// Token: 0x04000698 RID: 1688
		Undefined
	}
}
