using System;
using System.Collections;
using System.Text;
using Panik;
using TMPro;
using UnityEngine;

public class RewardBoxScript : MonoBehaviour
{
	// Token: 0x060006CD RID: 1741 RVA: 0x0002B72F File Offset: 0x0002992F
	public static bool IsOpened()
	{
		return !(RewardBoxScript.instance == null) && RewardBoxScript.instance._opened;
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x0002B74A File Offset: 0x0002994A
	public static bool IsOpening()
	{
		return !(RewardBoxScript.instance == null) && RewardBoxScript.instance._isOpening;
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x0002B768 File Offset: 0x00029968
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

	// Token: 0x060006D0 RID: 1744 RVA: 0x0002B7C8 File Offset: 0x000299C8
	private IEnumerator OpenCoroutine()
	{
		this._isOpening = true;
		float timer = 0f;
		this.effectScript.gameObject.SetActive(true);
		Sound.Play3D("SoundRewardBoxConfetti", base.transform.position, 20f, 1f, 1f, 1);
		while (timer < 1f)
		{
			timer += Tick.Time;
			yield return null;
		}
		Sound.Play3D("SoundRewardBoxTrigger", base.transform.position, 20f, 1f, 1f, 1);
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
				Sound.Play3D("SoundRewardBoxDoorHit", base.transform.position, 20f, 1f - (float)bouncedTimes * 0.2f, 1f, 1);
			}
			this.doorTr.localEulerAngles = doorEulers;
			yield return null;
		}
		this.SetModelAsOpened();
		this._isOpening = false;
		yield break;
	}

	// Token: 0x060006D1 RID: 1745 RVA: 0x0002B7D7 File Offset: 0x000299D7
	public void SetModelAsOpened()
	{
		this.doorTr.localEulerAngles = RewardBoxScript.doorOpenedEulers;
	}

	// Token: 0x060006D2 RID: 1746 RVA: 0x0002B7EC File Offset: 0x000299EC
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

	// Token: 0x060006D3 RID: 1747 RVA: 0x0002B859 File Offset: 0x00029A59
	private static void DilogueDeathCallback()
	{
		GameplayMaster.instance.DieTry(GameplayMaster.DeathStep.lookAtTrapdoor, false);
	}

	// Token: 0x060006D4 RID: 1748 RVA: 0x0002B868 File Offset: 0x00029A68
	public static bool HasPrize()
	{
		return !(RewardBoxScript.instance == null) && RewardBoxScript.instance.prizeOfGame.activeSelf;
	}

	// Token: 0x060006D5 RID: 1749 RVA: 0x0002B88D File Offset: 0x00029A8D
	public static RewardBoxScript.RewardKind GetRewardKind()
	{
		if (RewardBoxScript.instance == null)
		{
			return RewardBoxScript.RewardKind.Undefined;
		}
		return RewardBoxScript.instance.rewardKind;
	}

	// Token: 0x060006D6 RID: 1750 RVA: 0x0002B8A8 File Offset: 0x00029AA8
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
			RewardBoxScript.instance.monitorText.fontStyle = 64;
		}
		RewardBoxScript.instance.monitorText.text = "> " + GameplayData.GetRewardDeadlineDebt().ToStringSmart() + "<sprite name=\"CoinSymbolOrange64\">";
		if (gamePhase != GameplayMaster.GamePhase.intro)
		{
			Sound.Play3D("SoundRewardBoxTextUpdate", RewardBoxScript.instance.transform.position, 30f, 1f, 1f, 1);
		}
	}

	// Token: 0x060006D7 RID: 1751 RVA: 0x0002B95C File Offset: 0x00029B5C
	private void SkeletonKeyMaterialUpdate()
	{
		Material material = RewardBoxScript.DoorKeyDesiredMaterial_Get();
		if (this.doorKeyMeshRenderer.sharedMaterial != material)
		{
			this.doorKeyMeshRenderer.sharedMaterial = material;
			this.doorKeyHolyEffect.SetActive(material == this.doorKeyMaterial_Good);
		}
	}

	// Token: 0x060006D8 RID: 1752 RVA: 0x0002B9A8 File Offset: 0x00029BA8
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

	// Token: 0x060006D9 RID: 1753 RVA: 0x0002B9F4 File Offset: 0x00029BF4
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

	// Token: 0x060006DA RID: 1754 RVA: 0x0002BC16 File Offset: 0x00029E16
	private void Awake()
	{
		RewardBoxScript.instance = this;
		this.myMenuElement = base.GetComponentInChildren<DiegeticMenuElement>();
	}

	// Token: 0x060006DB RID: 1755 RVA: 0x0002BC2A File Offset: 0x00029E2A
	private void Start()
	{
		this.doorKeyHolyEffect.SetActive(false);
	}

	// Token: 0x060006DC RID: 1756 RVA: 0x0002BC38 File Offset: 0x00029E38
	private void OnDestroy()
	{
		if (RewardBoxScript.instance == this)
		{
			RewardBoxScript.instance = null;
		}
	}

	// Token: 0x060006DD RID: 1757 RVA: 0x0002BC50 File Offset: 0x00029E50
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

	public static RewardBoxScript instance;

	private static Vector3 doorClosedEulers = Vector3.zero;

	private static Vector3 doorOpenedEulers = new Vector3(0f, -180f, 0f);

	public Transform doorTr;

	public GameObject[] prizes;

	private DiegeticMenuElement myMenuElement;

	public TextMeshProUGUI monitorText;

	[NonSerialized]
	public MeshRenderer prizeMeshRenderer;

	private Material prizeDefaultMaterial;

	public Material prizeFlashMaterial;

	public Material demoPrizeUsedMaterial;

	public MeshRenderer doorKeyMeshRenderer;

	public Material doorKeyMaterial_Default;

	public Material doorKeyMaterial_Evil;

	public Material doorKeyMaterial_Good;

	public GameObject doorKeyHolyEffect;

	public TextMeshPro doorKeyBonesText;

	private GameObject prizeOfGame;

	private bool _opened;

	private bool _isOpening;

	public EffectScript effectScript;

	private RewardBoxScript.RewardKind rewardKind = RewardBoxScript.RewardKind.Undefined;

	private int bonesOld = -1;

	private StringBuilder bonesSB = new StringBuilder();

	public enum RewardKind
	{
		DemoPrize,
		DrawerKey0,
		DrawerKey1,
		DrawerKey2,
		DrawerKey3,
		DoorKey,
		Count,
		Undefined
	}
}
