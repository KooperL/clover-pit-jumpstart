using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ToyPhoneScript : MonoBehaviour
{
	// Token: 0x0600082A RID: 2090 RVA: 0x00035754 File Offset: 0x00033954
	public void MenuConnectionUpdate(ToyPhoneScript.MenuConnectionKind menuConnectionKind)
	{
		if (!this.meshHolder.activeSelf)
		{
			if (DiegeticMenuController.MainMenu.elements.Contains(ToyPhoneScript.instance.myDiegeticMenuElement))
			{
				DiegeticMenuController.MainMenu.elements.Remove(ToyPhoneScript.instance.myDiegeticMenuElement);
			}
			if (DiegeticMenuController.SlotMenu.elements.Contains(ToyPhoneScript.instance.myDiegeticMenuElement))
			{
				DiegeticMenuController.SlotMenu.elements.Remove(ToyPhoneScript.instance.myDiegeticMenuElement);
			}
			return;
		}
		if (menuConnectionKind != ToyPhoneScript.MenuConnectionKind.freeRoamMenu)
		{
			if (menuConnectionKind != ToyPhoneScript.MenuConnectionKind.slotMenu)
			{
				Debug.LogError("ToyPhoneScript: MenuConnectionUpdate: menu connection kind not handled: " + menuConnectionKind.ToString());
			}
			else if (!DiegeticMenuController.SlotMenu.elements.Contains(ToyPhoneScript.instance.myDiegeticMenuElement))
			{
				DiegeticMenuController.SlotMenu.elements.Add(ToyPhoneScript.instance.myDiegeticMenuElement);
				ToyPhoneScript.instance.myDiegeticMenuElement.SetMyController(DiegeticMenuController.SlotMenu);
				DiegeticMenuController.MainMenu.elements.Remove(ToyPhoneScript.instance.myDiegeticMenuElement);
				return;
			}
		}
		else if (!DiegeticMenuController.MainMenu.elements.Contains(ToyPhoneScript.instance.myDiegeticMenuElement))
		{
			DiegeticMenuController.SlotMenu.elements.Remove(ToyPhoneScript.instance.myDiegeticMenuElement);
			DiegeticMenuController.MainMenu.elements.Add(ToyPhoneScript.instance.myDiegeticMenuElement);
			ToyPhoneScript.instance.myDiegeticMenuElement.SetMyController(DiegeticMenuController.MainMenu);
			return;
		}
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x000358CC File Offset: 0x00033ACC
	public void UpdateLabelText()
	{
		if (ToyPhoneUIScript.IsEnabled())
		{
			int num = ToyPhoneUIScript.Pages_GetIndex() + 1;
			int num2 = ToyPhoneUIScript.Pages_GetMax();
			this.labelText.text = string.Concat(new string[]
			{
				Translation.Get("TOY_PHONE_PAGE_LABEL"),
				"\n",
				num.ToString(),
				"/",
				num2.ToString()
			});
			return;
		}
		this.labelText.text = Translation.Get("TOY_PHONE_LABEL");
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x0003594B File Offset: 0x00033B4B
	private void Awake()
	{
		ToyPhoneScript.instance = this;
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x00035953 File Offset: 0x00033B53
	private void OnDestroy()
	{
		if (ToyPhoneScript.instance == this)
		{
			ToyPhoneScript.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.UpdateLabelText));
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x00035988 File Offset: 0x00033B88
	private void Start()
	{
		this.UpdateLabelText();
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.UpdateLabelText));
	}

	// Token: 0x0600082F RID: 2095 RVA: 0x000359B0 File Offset: 0x00033BB0
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		if (GameplayData.Instance.phoneAbilitiesPickHistory.Count <= 0)
		{
			if (this.meshHolder.activeSelf)
			{
				this.meshHolder.SetActive(false);
				this.MenuConnectionUpdate(ToyPhoneScript.MenuConnectionKind.undefined);
			}
		}
		else if (!this.meshHolder.activeSelf)
		{
			this.meshHolder.SetActive(true);
			this.MenuConnectionUpdate((GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.gambling) ? ToyPhoneScript.MenuConnectionKind.slotMenu : ToyPhoneScript.MenuConnectionKind.freeRoamMenu);
		}
		bool flag = ToyPhoneUIScript.IsEnabled();
		if (this.labelTextShowsPages != flag)
		{
			this.labelTextShowsPages = flag;
			this.UpdateLabelText();
		}
	}

	public static ToyPhoneScript instance;

	private const int PLAYER_INDEX = 0;

	public DiegeticMenuElement myDiegeticMenuElement;

	public GameObject meshHolder;

	public TextMeshPro labelText;

	private bool labelTextShowsPages;

	public enum MenuConnectionKind
	{
		undefined,
		freeRoamMenu,
		slotMenu
	}
}
