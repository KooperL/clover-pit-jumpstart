using System;
using Panik;
using UnityEngine;

// Token: 0x02000090 RID: 144
public class DeckBoxScript : MonoBehaviour
{
	// Token: 0x060008DC RID: 2268 RVA: 0x0000D05F File Offset: 0x0000B25F
	public static bool IsEnabled()
	{
		return !(DeckBoxScript.instance == null) && DeckBoxScript.instance.holder.activeSelf;
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x00049D4C File Offset: 0x00047F4C
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

	// Token: 0x060008DE RID: 2270 RVA: 0x00049EC4 File Offset: 0x000480C4
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

	// Token: 0x060008DF RID: 2271 RVA: 0x00049F44 File Offset: 0x00048144
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

	// Token: 0x060008E0 RID: 2272 RVA: 0x00049FA4 File Offset: 0x000481A4
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

	// Token: 0x060008E1 RID: 2273 RVA: 0x0004A000 File Offset: 0x00048200
	public static void CandlesStateUpdate(bool forceNormal)
	{
		if (!forceNormal && Data.game.RunModifier_HardcoreMode_Get(GameplayData.RunModifier_GetCurrent()))
		{
			DeckBoxScript.instance.candlesHolder_Normal.SetActive(false);
			DeckBoxScript.instance.candlesHolder_Skull.SetActive(true);
			return;
		}
		DeckBoxScript.instance.candlesHolder_Normal.SetActive(true);
		DeckBoxScript.instance.candlesHolder_Skull.SetActive(false);
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x0000D07F File Offset: 0x0000B27F
	private void Awake()
	{
		DeckBoxScript.instance = this;
		this.summonedCardHolderStartPosition_Local = this.summonedCardHolder.localPosition;
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x0000D098 File Offset: 0x0000B298
	private void Start()
	{
		this.holder.SetActive(false);
		this.summonedCardHolder.localScale = Vector3.zero;
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x0000D0B6 File Offset: 0x0000B2B6
	private void OnDestroy()
	{
		DeckBoxScript.instance == this;
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x0004A064 File Offset: 0x00048264
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

	// Token: 0x0400088D RID: 2189
	public static DeckBoxScript instance;

	// Token: 0x0400088E RID: 2190
	public GameObject holder;

	// Token: 0x0400088F RID: 2191
	public DiegeticMenuElement myDiegeticMenuElement;

	// Token: 0x04000890 RID: 2192
	public MeshRenderer meshOpened;

	// Token: 0x04000891 RID: 2193
	public MeshRenderer meshClosed;

	// Token: 0x04000892 RID: 2194
	public Transform summonedCardHolder;

	// Token: 0x04000893 RID: 2195
	public GameObject headset;

	// Token: 0x04000894 RID: 2196
	public GameObject candlesHolder_Normal;

	// Token: 0x04000895 RID: 2197
	public GameObject candlesHolder_Skull;

	// Token: 0x04000896 RID: 2198
	private CardScript summonedCard;

	// Token: 0x04000897 RID: 2199
	private Vector3 summonedCardHolderStartPosition_Local;

	// Token: 0x02000091 RID: 145
	public enum MenuConnectionKind
	{
		// Token: 0x04000899 RID: 2201
		undefined,
		// Token: 0x0400089A RID: 2202
		freeRoamMenu,
		// Token: 0x0400089B RID: 2203
		slotMenu
	}
}
