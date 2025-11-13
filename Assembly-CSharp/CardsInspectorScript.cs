using System;
using Febucci.UI;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class CardsInspectorScript : MonoBehaviour
{
	// Token: 0x06000947 RID: 2375 RVA: 0x0003D791 File Offset: 0x0003B991
	public static bool IsEnabled()
	{
		return !(CardsInspectorScript.instance == null) && CardsInspectorScript.instance.holder.activeSelf;
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x0003D7B4 File Offset: 0x0003B9B4
	public static void Open(string titleKeyString, string descrKeyString, CardsInspectorScript.PromptKind promptKind)
	{
		if (CardsInspectorScript.instance == null)
		{
			return;
		}
		CardsInspectorScript.instance.holder.SetActive(true);
		CardsInspectorScript.instance.titleKey = titleKeyString;
		CardsInspectorScript.instance.descrKey = descrKeyString;
		CardsInspectorScript.instance.promptKind = promptKind;
		CardsInspectorScript.InspectCard_Set(null, true);
		CardsInspectorScript._TextUpdate();
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x0003D80C File Offset: 0x0003BA0C
	public static void Close()
	{
		if (CardsInspectorScript.instance == null)
		{
			return;
		}
		CardsInspectorScript.instance.holder.SetActive(false);
		CardsInspectorScript.InspectCard_Set(null, true);
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x0003D834 File Offset: 0x0003BA34
	public static void InspectCard_Set(CardScript cardToInspect, bool cardIsHidden)
	{
		if (CardsInspectorScript.instance == null)
		{
			return;
		}
		bool flag = cardToInspect != CardsInspectorScript.instance.inspectedCard;
		bool flag2 = cardIsHidden != CardsInspectorScript.instance.cardIsHidden;
		CardsInspectorScript.instance.inspectedCard = cardToInspect;
		CardsInspectorScript.instance.cardIsHidden = cardIsHidden;
		if (flag || flag2)
		{
			CardsInspectorScript._TextUpdate();
		}
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x0003D88F File Offset: 0x0003BA8F
	private static void _TextUpdate()
	{
		if (CardsInspectorScript.instance.inspectedCard == null || CardsInspectorScript.instance.cardIsHidden)
		{
			CardsInspectorScript.instance._TextUpdateToDefault();
			return;
		}
		CardsInspectorScript.instance._TextUpdateToInspectedCard();
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x0003D8C4 File Offset: 0x0003BAC4
	private static void _TextUpdate_ControlsCallback(Controls.InputActionMap map)
	{
		if (CardsInspectorScript.instance.promptKind == CardsInspectorScript.PromptKind.none || CardsInspectorScript.instance.promptKind == CardsInspectorScript.PromptKind.count || CardsInspectorScript.instance.promptKind == CardsInspectorScript.PromptKind.undefined)
		{
			return;
		}
		CardsInspectorScript._TextUpdate();
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0003D8F4 File Offset: 0x0003BAF4
	private void _TextUpdateToDefault()
	{
		this.titleText.SyncText(Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get(this.titleKey), Strings.SanitizationSubKind.none), false);
		this.descriptionText.SyncText(Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get(this.descrKey), Strings.SanitizationSubKind.none), false);
		this.promptsText.SyncText(this.PromptStringGet(), false);
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x0003D950 File Offset: 0x0003BB50
	private void _TextUpdateToInspectedCard()
	{
		this.titleText.SyncText(RunModifierScript.TitleGet(this.inspectedCard.identifier), false);
		Strings.SetTemporaryFlag_Sanitize666And999(1);
		this.descriptionText.SyncText(RunModifierScript.DescriptionGet(this.inspectedCard.identifier), false);
		this.promptsText.SyncText(this.PromptStringGet(), false);
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0003D9AD File Offset: 0x0003BBAD
	private string PromptStringGet()
	{
		if (this.promptKind == CardsInspectorScript.PromptKind.none)
		{
			return "";
		}
		Debug.LogError("CardsInspectorScript.PromptStringGet(): Prompt kind not handled: " + this.promptKind.ToString());
		return "";
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0003D9E4 File Offset: 0x0003BBE4
	public static bool IsInspectingCard()
	{
		return !(CardsInspectorScript.instance == null) && (CardsInspectorScript.instance.inspectedCard != null && CardsInspectorScript.instance.inspectedCard.identifier != RunModifierScript.Identifier.undefined) && CardsInspectorScript.instance.inspectedCard.identifier != RunModifierScript.Identifier.count;
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0003DA3D File Offset: 0x0003BC3D
	public static CardScript InspectedCard_Get()
	{
		if (CardsInspectorScript.instance == null)
		{
			return null;
		}
		return CardsInspectorScript.instance.inspectedCard;
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x0003DA58 File Offset: 0x0003BC58
	private void Awake()
	{
		CardsInspectorScript.instance = this;
		Controls.onPromptsUpdateRequest = (Controls.MapCallback)Delegate.Combine(Controls.onPromptsUpdateRequest, new Controls.MapCallback(CardsInspectorScript._TextUpdate_ControlsCallback));
		this.imageStartingPos = this.textImage.rectTransform.anchoredPosition;
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x0003DA96 File Offset: 0x0003BC96
	private void Start()
	{
		CardsInspectorScript.Close();
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x0003DA9D File Offset: 0x0003BC9D
	private void OnDestroy()
	{
		if (CardsInspectorScript.instance == this)
		{
			CardsInspectorScript.instance = null;
		}
		Controls.onPromptsUpdateRequest = (Controls.MapCallback)Delegate.Combine(Controls.onPromptsUpdateRequest, new Controls.MapCallback(CardsInspectorScript._TextUpdate_ControlsCallback));
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0003DAD4 File Offset: 0x0003BCD4
	private void Update()
	{
		Vector2 vector = this.imageStartingPos + new Vector2(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f));
		if (Data.settings.dyslexicFontEnabled)
		{
			vector = this.imageStartingPos;
		}
		this.textImage.rectTransform.anchoredPosition = vector;
	}

	public static CardsInspectorScript instance;

	private const int PLAYER_INDEX = 0;

	public GameObject holder;

	public TextAnimator titleText;

	public TextAnimator descriptionText;

	public TextAnimator promptsText;

	public Image textImage;

	private CardsInspectorScript.PromptKind promptKind;

	private string titleKey;

	private string descrKey;

	private CardScript inspectedCard;

	private bool cardIsHidden;

	private Vector2 imageStartingPos;

	public enum PromptKind
	{
		none,
		count,
		undefined
	}
}
