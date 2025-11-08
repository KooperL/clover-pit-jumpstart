using System;
using Febucci.UI;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class CardsInspectorScript : MonoBehaviour
{
	// Token: 0x06000938 RID: 2360 RVA: 0x0003D42E File Offset: 0x0003B62E
	public static bool IsEnabled()
	{
		return !(CardsInspectorScript.instance == null) && CardsInspectorScript.instance.holder.activeSelf;
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0003D450 File Offset: 0x0003B650
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

	// Token: 0x0600093A RID: 2362 RVA: 0x0003D4A8 File Offset: 0x0003B6A8
	public static void Close()
	{
		if (CardsInspectorScript.instance == null)
		{
			return;
		}
		CardsInspectorScript.instance.holder.SetActive(false);
		CardsInspectorScript.InspectCard_Set(null, true);
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x0003D4D0 File Offset: 0x0003B6D0
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

	// Token: 0x0600093C RID: 2364 RVA: 0x0003D52B File Offset: 0x0003B72B
	private static void _TextUpdate()
	{
		if (CardsInspectorScript.instance.inspectedCard == null || CardsInspectorScript.instance.cardIsHidden)
		{
			CardsInspectorScript.instance._TextUpdateToDefault();
			return;
		}
		CardsInspectorScript.instance._TextUpdateToInspectedCard();
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x0003D560 File Offset: 0x0003B760
	private static void _TextUpdate_ControlsCallback(Controls.InputActionMap map)
	{
		if (CardsInspectorScript.instance.promptKind == CardsInspectorScript.PromptKind.none || CardsInspectorScript.instance.promptKind == CardsInspectorScript.PromptKind.count || CardsInspectorScript.instance.promptKind == CardsInspectorScript.PromptKind.undefined)
		{
			return;
		}
		CardsInspectorScript._TextUpdate();
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x0003D590 File Offset: 0x0003B790
	private void _TextUpdateToDefault()
	{
		this.titleText.SyncText(Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get(this.titleKey), Strings.SanitizationSubKind.none), false);
		this.descriptionText.SyncText(Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get(this.descrKey), Strings.SanitizationSubKind.none), false);
		this.promptsText.SyncText(this.PromptStringGet(), false);
	}

	// Token: 0x0600093F RID: 2367 RVA: 0x0003D5EC File Offset: 0x0003B7EC
	private void _TextUpdateToInspectedCard()
	{
		this.titleText.SyncText(RunModifierScript.TitleGet(this.inspectedCard.identifier), false);
		Strings.SetTemporaryFlag_Sanitize666And999(1);
		this.descriptionText.SyncText(RunModifierScript.DescriptionGet(this.inspectedCard.identifier), false);
		this.promptsText.SyncText(this.PromptStringGet(), false);
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0003D649 File Offset: 0x0003B849
	private string PromptStringGet()
	{
		if (this.promptKind == CardsInspectorScript.PromptKind.none)
		{
			return "";
		}
		Debug.LogError("CardsInspectorScript.PromptStringGet(): Prompt kind not handled: " + this.promptKind.ToString());
		return "";
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0003D680 File Offset: 0x0003B880
	public static bool IsInspectingCard()
	{
		return !(CardsInspectorScript.instance == null) && (CardsInspectorScript.instance.inspectedCard != null && CardsInspectorScript.instance.inspectedCard.identifier != RunModifierScript.Identifier.undefined) && CardsInspectorScript.instance.inspectedCard.identifier != RunModifierScript.Identifier.count;
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0003D6D9 File Offset: 0x0003B8D9
	public static CardScript InspectedCard_Get()
	{
		if (CardsInspectorScript.instance == null)
		{
			return null;
		}
		return CardsInspectorScript.instance.inspectedCard;
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0003D6F4 File Offset: 0x0003B8F4
	private void Awake()
	{
		CardsInspectorScript.instance = this;
		Controls.onPromptsUpdateRequest = (Controls.MapCallback)Delegate.Combine(Controls.onPromptsUpdateRequest, new Controls.MapCallback(CardsInspectorScript._TextUpdate_ControlsCallback));
		this.imageStartingPos = this.textImage.rectTransform.anchoredPosition;
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x0003D732 File Offset: 0x0003B932
	private void Start()
	{
		CardsInspectorScript.Close();
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x0003D739 File Offset: 0x0003B939
	private void OnDestroy()
	{
		if (CardsInspectorScript.instance == this)
		{
			CardsInspectorScript.instance = null;
		}
		Controls.onPromptsUpdateRequest = (Controls.MapCallback)Delegate.Combine(Controls.onPromptsUpdateRequest, new Controls.MapCallback(CardsInspectorScript._TextUpdate_ControlsCallback));
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x0003D770 File Offset: 0x0003B970
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
