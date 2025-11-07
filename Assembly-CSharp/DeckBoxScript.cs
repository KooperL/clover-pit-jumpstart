using System;
using Panik;
using UnityEngine;

public class DeckBoxScript : MonoBehaviour
{
	// Token: 0x060007D1 RID: 2001 RVA: 0x000329F1 File Offset: 0x00030BF1
	public static bool IsEnabled()
	{
		return !(DeckBoxScript.instance == null) && DeckBoxScript.instance.holder.activeSelf;
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x00032A14 File Offset: 0x00030C14
	public void MenuConnectionUpdate(DeckBoxScript.MenuConnectionKind menuConnectionKind)
	{
		if (!this.holder.activeSelf)
		{
			if (DiegeticMenuController.MainMenu.elements.Contains(DeckBoxScript.instance.myDiegeticMenuElement))
			{
				DiegeticMenuController.MainMenu.elements.Remove(DeckBoxScript.instance.myDiegeticMenuElement);
			}
			if (DiegeticMenuController.SlotMenu.elements.Contains(DeckBoxScript.instance.myDiegeticMenuElement))
			{
				DiegeticMenuController.SlotMenu.elements.Remove(DeckBoxScript.instance.myDiegeticMenuElement);
			}
			return;
		}
		if (menuConnectionKind != DeckBoxScript.MenuConnectionKind.freeRoamMenu)
		{
			if (menuConnectionKind != DeckBoxScript.MenuConnectionKind.slotMenu)
			{
				Debug.LogError("ToyPhoneScript: MenuConnectionUpdate: menu connection kind not handled: " + menuConnectionKind.ToString());
			}
			else if (!DiegeticMenuController.SlotMenu.elements.Contains(DeckBoxScript.instance.myDiegeticMenuElement))
			{
				DiegeticMenuController.SlotMenu.elements.Add(DeckBoxScript.instance.myDiegeticMenuElement);
				DeckBoxScript.instance.myDiegeticMenuElement.SetMyController(DiegeticMenuController.SlotMenu);
				DiegeticMenuController.MainMenu.elements.Remove(DeckBoxScript.instance.myDiegeticMenuElement);
				return;
			}
		}
		else if (!DiegeticMenuController.MainMenu.elements.Contains(DeckBoxScript.instance.myDiegeticMenuElement))
		{
			DiegeticMenuController.SlotMenu.elements.Remove(DeckBoxScript.instance.myDiegeticMenuElement);
			DiegeticMenuController.MainMenu.elements.Add(DeckBoxScript.instance.myDiegeticMenuElement);
			DeckBoxScript.instance.myDiegeticMenuElement.SetMyController(DiegeticMenuController.MainMenu);
			return;
		}
	}

	// Token: 0x060007D3 RID: 2003 RVA: 0x00032B8C File Offset: 0x00030D8C
	public static void SetSummonedCard()
	{
		if (DeckBoxScript.instance == null)
		{
			return;
		}
		RunModifierScript.Identifier identifier = GameplayData.RunModifier_GetCurrent();
		if (identifier == RunModifierScript.Identifier.undefined || identifier == RunModifierScript.Identifier.count)
		{
			return;
		}
		CardScript cardScript = CardScript.PoolSpawn(identifier, 1f, DeckBoxScript.instance.summonedCardHolder);
		cardScript.transform.localPosition = Vector3.zero;
		cardScript.transform.localEulerAngles = Vector3.zero;
		cardScript.TextForceHidden(true);
		cardScript.CardColorSet(Color.white);
		DeckBoxScript.instance.summonedCard = cardScript;
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x00032C0C File Offset: 0x00030E0C
	public static bool ItsMemoryCardTime()
	{
		RewardBoxScript.RewardKind rewardKind = RewardBoxScript.GetRewardKind();
		switch (rewardKind)
		{
		case RewardBoxScript.RewardKind.DrawerKey0:
		case RewardBoxScript.RewardKind.DrawerKey1:
			return false;
		case RewardBoxScript.RewardKind.DrawerKey2:
			return true;
		case RewardBoxScript.RewardKind.DrawerKey3:
			return true;
		case RewardBoxScript.RewardKind.DoorKey:
			return true;
		case RewardBoxScript.RewardKind.Undefined:
			return false;
		}
		Debug.LogError("Cannot determine if it's time to unlock modifiers for reward kind: " + rewardKind.ToString());
		return false;
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x00032C6C File Offset: 0x00030E6C
	public static bool CanActuallyCountVictoriesForCards()
	{
		RewardBoxScript.RewardKind rewardKind = RewardBoxScript.GetRewardKind();
		switch (rewardKind)
		{
		case RewardBoxScript.RewardKind.DrawerKey0:
		case RewardBoxScript.RewardKind.DrawerKey1:
		case RewardBoxScript.RewardKind.DrawerKey2:
		case RewardBoxScript.RewardKind.DrawerKey3:
			return false;
		case RewardBoxScript.RewardKind.DoorKey:
			return true;
		case RewardBoxScript.RewardKind.Undefined:
			return false;
		}
		Debug.LogError("Cannot determine if it's time to count victories for mem cards. Reward kind: " + rewardKind.ToString());
		return false;
	}

	// Token: 0x060007D6 RID: 2006 RVA: 0x00032CC8 File Offset: 0x00030EC8
	private void Awake()
	{
		DeckBoxScript.instance = this;
		this.summonedCardHolderStartPosition_Local = this.summonedCardHolder.localPosition;
	}

	// Token: 0x060007D7 RID: 2007 RVA: 0x00032CE1 File Offset: 0x00030EE1
	private void Start()
	{
		this.holder.SetActive(false);
		this.summonedCardHolder.localScale = Vector3.zero;
	}

	// Token: 0x060007D8 RID: 2008 RVA: 0x00032CFF File Offset: 0x00030EFF
	private void OnDestroy()
	{
		DeckBoxScript.instance == this;
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x00032D10 File Offset: 0x00030F10
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		if (!this.holder.activeSelf)
		{
			bool flag = Data.game.RunModifier_UnlockedTotalNumber() > 0 && DeckBoxScript.ItsMemoryCardTime();
			this.holder.SetActive(flag);
			this.MenuConnectionUpdate((GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.gambling) ? DeckBoxScript.MenuConnectionKind.slotMenu : DeckBoxScript.MenuConnectionKind.freeRoamMenu);
		}
		if (DeckBoxUI.IsEnabled())
		{
			if (!this.meshOpened.gameObject.activeSelf)
			{
				this.meshOpened.gameObject.SetActive(true);
			}
			if (this.meshClosed.gameObject.activeSelf)
			{
				this.meshClosed.gameObject.SetActive(false);
			}
			if (this.headset.activeSelf)
			{
				this.headset.SetActive(false);
			}
		}
		else
		{
			if (this.meshOpened.gameObject.activeSelf)
			{
				this.meshOpened.gameObject.SetActive(false);
			}
			if (!this.meshClosed.gameObject.activeSelf)
			{
				this.meshClosed.gameObject.SetActive(true);
			}
			if (!this.headset.activeSelf)
			{
				this.headset.SetActive(true);
			}
		}
		bool flag2 = true;
		if (this.summonedCard == null || DeckBoxUI.IsEnabled() || ToyPhoneUIScript.IsEnabled() || CameraController.GetPositionKind() == CameraController.PositionKind.DeckBox)
		{
			flag2 = false;
		}
		if (this.summonedCardHolder.gameObject.activeSelf != flag2)
		{
			this.summonedCardHolder.gameObject.SetActive(flag2);
		}
		if (flag2)
		{
			this.summonedCardHolder.localScale = Vector3.Lerp(this.summonedCardHolder.localScale, Vector3.one, Tick.Time * 5f);
		}
		this.summonedCardHolder.localPosition = this.summonedCardHolderStartPosition_Local + new Vector3(0f, Util.AngleSin(Tick.PassedTime * 90f), 0f) * 0.05f;
	}

	public static DeckBoxScript instance;

	public GameObject holder;

	public DiegeticMenuElement myDiegeticMenuElement;

	public MeshRenderer meshOpened;

	public MeshRenderer meshClosed;

	public Transform summonedCardHolder;

	public GameObject headset;

	private CardScript summonedCard;

	private Vector3 summonedCardHolderStartPosition_Local;

	public enum MenuConnectionKind
	{
		undefined,
		freeRoamMenu,
		slotMenu
	}
}
