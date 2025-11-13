using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ToyPhoneScript : MonoBehaviour
{
	// Token: 0x06000831 RID: 2097 RVA: 0x0003593C File Offset: 0x00033B3C
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

	// Token: 0x06000832 RID: 2098 RVA: 0x00035AB4 File Offset: 0x00033CB4
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

	// Token: 0x06000833 RID: 2099 RVA: 0x00035B33 File Offset: 0x00033D33
	private void Awake()
	{
		ToyPhoneScript.instance = this;
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x00035B3B File Offset: 0x00033D3B
	private void OnDestroy()
	{
		if (ToyPhoneScript.instance == this)
		{
			ToyPhoneScript.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.UpdateLabelText));
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x00035B70 File Offset: 0x00033D70
	private void Start()
	{
		this.UpdateLabelText();
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.UpdateLabelText));
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x00035B98 File Offset: 0x00033D98
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
