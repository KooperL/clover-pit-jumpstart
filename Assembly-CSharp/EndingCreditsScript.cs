using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingCreditsScript : MonoBehaviour
{
	// Token: 0x060003B1 RID: 945 RVA: 0x000199B2 File Offset: 0x00017BB2
	public static bool IsEnabled()
	{
		return !(EndingCreditsScript.instance == null) && EndingCreditsScript.instance.holder.activeSelf;
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x000199D4 File Offset: 0x00017BD4
	public static void Open(bool goodEnding, bool skippable)
	{
		EndingCreditsScript.instance.holder.SetActive(true);
		EndingCreditsScript.instance.UpdateText();
		EndingCreditsScript.instance.skippable = skippable;
		if (goodEnding)
		{
			EndingCreditsScript.instance.backBlackImage.color = Color.black;
			for (int i = 0; i < EndingCreditsScript.instance.goodEndingBackgrounds.Length; i++)
			{
				EndingCreditsScript.instance.goodEndingBackgrounds[i].enabled = true;
			}
		}
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x00019A46 File Offset: 0x00017C46
	private static void Close()
	{
		EndingCreditsScript.instance.holder.SetActive(false);
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x00019A58 File Offset: 0x00017C58
	private void UpdateText()
	{
		this.titleText.text = Translation.Get("VARIOUS_MAGAZINE_CREDITS_TITLE");
		this.creditsBodyText.text = MagazineUiScript.GetCreditsString();
		this.titleText.ForceMeshUpdate(false, false);
		this.creditsBodyText.ForceMeshUpdate(false, false);
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x00019AA4 File Offset: 0x00017CA4
	private void Awake()
	{
		EndingCreditsScript.instance = this;
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x00019AAC File Offset: 0x00017CAC
	private void OnDestroy()
	{
		if (EndingCreditsScript.instance == this)
		{
			EndingCreditsScript.instance = null;
		}
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x00019AC4 File Offset: 0x00017CC4
	private void Start()
	{
		this.holder.SetActive(false);
		for (int i = 0; i < this.goodEndingBackgrounds.Length; i++)
		{
			this.goodEndingBackgrounds[i].enabled = false;
		}
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x00019B00 File Offset: 0x00017D00
	private void Update()
	{
		if (!EndingCreditsScript.IsEnabled())
		{
			return;
		}
		float num = Mathf.Abs(this.creditsBodyText.rectTransform.anchoredPosition.y) + this.creditsBodyText.renderedHeight + 10f;
		this.scrollDelayTimer -= Tick.Time;
		Vector2 anchoredPosition = this.scroller.anchoredPosition;
		if (this.scrollDelayTimer <= 0f)
		{
			anchoredPosition.y += Tick.Time * num / 92f;
			this.scroller.anchoredPosition = anchoredPosition;
		}
		if (this.skippable && this.scrollDelayTimer < 3.5f && !this.skip && (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) || Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true)))
		{
			this.skip = true;
		}
		if (anchoredPosition.y > num || this.skip)
		{
			Color color = this.backBlackImage.color;
			color.a += Tick.Time * 0.5f;
			this.backBlackImage.color = color;
			if (color.a >= 1f)
			{
				EndingCreditsScript.Close();
			}
		}
		else
		{
			Color color2 = this.backBlackImage.color;
			color2.a = Mathf.MoveTowards(color2.a, 0.5f, Tick.Time * 0.1f);
			this.backBlackImage.color = color2;
		}
		for (int i = 0; i < this.goodEndingBackgrounds.Length; i++)
		{
			Rect uvRect = this.goodEndingBackgrounds[i].uvRect;
			uvRect.x -= (float)i * 0.01f * Tick.Time;
			this.goodEndingBackgrounds[i].uvRect = uvRect;
		}
	}

	private const float CREDITS_TIME = 92f;

	public static EndingCreditsScript instance;

	public GameObject holder;

	public RectTransform scroller;

	public TextMeshProUGUI titleText;

	public TextMeshProUGUI creditsBodyText;

	public Image backBlackImage;

	public RawImage[] goodEndingBackgrounds;

	private float scrollDelayTimer = 2f;

	private bool skippable;

	private bool skip;
}
