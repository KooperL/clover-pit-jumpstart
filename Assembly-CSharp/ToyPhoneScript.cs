using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000A3 RID: 163
public class ToyPhoneScript : MonoBehaviour
{
	// Token: 0x0600094E RID: 2382 RVA: 0x0004CCA8 File Offset: 0x0004AEA8
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

	// Token: 0x0600094F RID: 2383 RVA: 0x0004CE20 File Offset: 0x0004B020
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

	// Token: 0x06000950 RID: 2384 RVA: 0x0000D543 File Offset: 0x0000B743
	private void Awake()
	{
		ToyPhoneScript.instance = this;
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0000D54B File Offset: 0x0000B74B
	private void OnDestroy()
	{
		if (ToyPhoneScript.instance == this)
		{
			ToyPhoneScript.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.UpdateLabelText));
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x0000D580 File Offset: 0x0000B780
	private void Start()
	{
		this.UpdateLabelText();
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.UpdateLabelText));
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x0004CEA0 File Offset: 0x0004B0A0
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

	// Token: 0x04000937 RID: 2359
	public static ToyPhoneScript instance;

	// Token: 0x04000938 RID: 2360
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000939 RID: 2361
	public DiegeticMenuElement myDiegeticMenuElement;

	// Token: 0x0400093A RID: 2362
	public GameObject meshHolder;

	// Token: 0x0400093B RID: 2363
	public TextMeshPro labelText;

	// Token: 0x0400093C RID: 2364
	private bool labelTextShowsPages;

	// Token: 0x020000A4 RID: 164
	public enum MenuConnectionKind
	{
		// Token: 0x0400093E RID: 2366
		undefined,
		// Token: 0x0400093F RID: 2367
		freeRoamMenu,
		// Token: 0x04000940 RID: 2368
		slotMenu
	}
}
