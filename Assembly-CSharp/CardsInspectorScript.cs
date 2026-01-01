using System;
using Febucci.UI;
using Panik;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C6 RID: 198
public class CardsInspectorScript : MonoBehaviour
{
	// Token: 0x06000A9B RID: 2715 RVA: 0x0000E6E4 File Offset: 0x0000C8E4
	public static bool IsEnabled()
	{
		return !(CardsInspectorScript.instance == null) && CardsInspectorScript.instance.holder.activeSelf;
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x00054E64 File Offset: 0x00053064
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

	// Token: 0x06000A9D RID: 2717 RVA: 0x0000E704 File Offset: 0x0000C904
	public static void Close()
	{
		if (CardsInspectorScript.instance == null)
		{
			return;
		}
		CardsInspectorScript.instance.holder.SetActive(false);
		CardsInspectorScript.InspectCard_Set(null, true);
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x00054EBC File Offset: 0x000530BC
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

	// Token: 0x06000A9F RID: 2719 RVA: 0x0000E72B File Offset: 0x0000C92B
	private static void _TextUpdate()
	{
		if (CardsInspectorScript.instance.inspectedCard == null || CardsInspectorScript.instance.cardIsHidden)
		{
			CardsInspectorScript.instance._TextUpdateToDefault();
			return;
		}
		CardsInspectorScript.instance._TextUpdateToInspectedCard();
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x0000E760 File Offset: 0x0000C960
	private static void _TextUpdate_ControlsCallback(Controls.InputActionMap map)
	{
		if (CardsInspectorScript.instance.promptKind == CardsInspectorScript.PromptKind.none || CardsInspectorScript.instance.promptKind == CardsInspectorScript.PromptKind.count || CardsInspectorScript.instance.promptKind == CardsInspectorScript.PromptKind.undefined)
		{
			return;
		}
		CardsInspectorScript._TextUpdate();
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00054F18 File Offset: 0x00053118
	private void _TextUpdateToDefault()
	{
		this.titleText.SyncText(Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get(this.titleKey), Strings.SanitizationSubKind.none), false);
		this.descriptionText.SyncText(Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get(this.descrKey), Strings.SanitizationSubKind.none), false);
		this.promptsText.SyncText(this.PromptStringGet(), false);
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x00054F74 File Offset: 0x00053174
	private void _TextUpdateToInspectedCard()
	{
		this.titleText.SyncText(RunModifierScript.TitleGet(this.inspectedCard.identifier), false);
		Strings.SetTemporaryFlag_Sanitize666And999(1);
		this.descriptionText.SyncText(RunModifierScript.DescriptionGet(this.inspectedCard.identifier), false);
		this.promptsText.SyncText(this.PromptStringGet(), false);
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x0000E78E File Offset: 0x0000C98E
	private string PromptStringGet()
	{
		if (this.promptKind == CardsInspectorScript.PromptKind.none)
		{
			return "";
		}
		Debug.LogError("CardsInspectorScript.PromptStringGet(): Prompt kind not handled: " + this.promptKind.ToString());
		return "";
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x00054FD4 File Offset: 0x000531D4
	public static bool IsInspectingCard()
	{
		return !(CardsInspectorScript.instance == null) && (CardsInspectorScript.instance.inspectedCard != null && CardsInspectorScript.instance.inspectedCard.identifier != RunModifierScript.Identifier.undefined) && CardsInspectorScript.instance.inspectedCard.identifier != RunModifierScript.Identifier.count;
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x0000E7C3 File Offset: 0x0000C9C3
	public static CardScript InspectedCard_Get()
	{
		if (CardsInspectorScript.instance == null)
		{
			return null;
		}
		return CardsInspectorScript.instance.inspectedCard;
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x0000E7DE File Offset: 0x0000C9DE
	private void Awake()
	{
		CardsInspectorScript.instance = this;
		Controls.onPromptsUpdateRequest = (Controls.MapCallback)Delegate.Combine(Controls.onPromptsUpdateRequest, new Controls.MapCallback(CardsInspectorScript._TextUpdate_ControlsCallback));
		this.imageStartingPos = this.textImage.rectTransform.anchoredPosition;
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x0000E81C File Offset: 0x0000CA1C
	private void Start()
	{
		CardsInspectorScript.Close();
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x0000E823 File Offset: 0x0000CA23
	private void OnDestroy()
	{
		if (CardsInspectorScript.instance == this)
		{
			CardsInspectorScript.instance = null;
		}
		Controls.onPromptsUpdateRequest = (Controls.MapCallback)Delegate.Combine(Controls.onPromptsUpdateRequest, new Controls.MapCallback(CardsInspectorScript._TextUpdate_ControlsCallback));
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x00055030 File Offset: 0x00053230
	private void Update()
	{
		Vector2 vector = this.imageStartingPos + new Vector2(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f));
		if (Data.settings.dyslexicFontEnabled)
		{
			vector = this.imageStartingPos;
		}
		this.textImage.rectTransform.anchoredPosition = vector;
	}

	// Token: 0x04000AC3 RID: 2755
	public static CardsInspectorScript instance;

	// Token: 0x04000AC4 RID: 2756
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000AC5 RID: 2757
	public GameObject holder;

	// Token: 0x04000AC6 RID: 2758
	public TextAnimator titleText;

	// Token: 0x04000AC7 RID: 2759
	public TextAnimator descriptionText;

	// Token: 0x04000AC8 RID: 2760
	public TextAnimator promptsText;

	// Token: 0x04000AC9 RID: 2761
	public Image textImage;

	// Token: 0x04000ACA RID: 2762
	private CardsInspectorScript.PromptKind promptKind;

	// Token: 0x04000ACB RID: 2763
	private string titleKey;

	// Token: 0x04000ACC RID: 2764
	private string descrKey;

	// Token: 0x04000ACD RID: 2765
	private CardScript inspectedCard;

	// Token: 0x04000ACE RID: 2766
	private bool cardIsHidden;

	// Token: 0x04000ACF RID: 2767
	private Vector2 imageStartingPos;

	// Token: 0x020000C7 RID: 199
	public enum PromptKind
	{
		// Token: 0x04000AD1 RID: 2769
		none,
		// Token: 0x04000AD2 RID: 2770
		count,
		// Token: 0x04000AD3 RID: 2771
		undefined
	}
}
