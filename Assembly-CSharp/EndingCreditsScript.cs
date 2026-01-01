using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000040 RID: 64
public class EndingCreditsScript : MonoBehaviour
{
	// Token: 0x06000415 RID: 1045 RVA: 0x00008E6E File Offset: 0x0000706E
	public static bool IsEnabled()
	{
		return !(EndingCreditsScript.instance == null) && EndingCreditsScript.instance.holder.activeSelf;
	}

	// Token: 0x06000416 RID: 1046 RVA: 0x0002D274 File Offset: 0x0002B474
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

	// Token: 0x06000417 RID: 1047 RVA: 0x00008E8E File Offset: 0x0000708E
	private static void Close()
	{
		EndingCreditsScript.instance.holder.SetActive(false);
	}

	// Token: 0x06000418 RID: 1048 RVA: 0x0002D2E8 File Offset: 0x0002B4E8
	private void UpdateText()
	{
		this.titleText.text = Translation.Get("VARIOUS_MAGAZINE_CREDITS_TITLE");
		this.creditsBodyText.text = MagazineUiScript.GetCreditsString();
		this.titleText.ForceMeshUpdate(false, false);
		this.creditsBodyText.ForceMeshUpdate(false, false);
	}

	// Token: 0x06000419 RID: 1049 RVA: 0x00008EA0 File Offset: 0x000070A0
	private void Awake()
	{
		EndingCreditsScript.instance = this;
	}

	// Token: 0x0600041A RID: 1050 RVA: 0x00008EA8 File Offset: 0x000070A8
	private void OnDestroy()
	{
		if (EndingCreditsScript.instance == this)
		{
			EndingCreditsScript.instance = null;
		}
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x0002D334 File Offset: 0x0002B534
	private void Start()
	{
		this.holder.SetActive(false);
		for (int i = 0; i < this.goodEndingBackgrounds.Length; i++)
		{
			this.goodEndingBackgrounds[i].enabled = false;
		}
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x0002D370 File Offset: 0x0002B570
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

	// Token: 0x04000399 RID: 921
	private const float CREDITS_TIME = 92f;

	// Token: 0x0400039A RID: 922
	public static EndingCreditsScript instance;

	// Token: 0x0400039B RID: 923
	public GameObject holder;

	// Token: 0x0400039C RID: 924
	public RectTransform scroller;

	// Token: 0x0400039D RID: 925
	public TextMeshProUGUI titleText;

	// Token: 0x0400039E RID: 926
	public TextMeshProUGUI creditsBodyText;

	// Token: 0x0400039F RID: 927
	public Image backBlackImage;

	// Token: 0x040003A0 RID: 928
	public RawImage[] goodEndingBackgrounds;

	// Token: 0x040003A1 RID: 929
	private float scrollDelayTimer = 2f;

	// Token: 0x040003A2 RID: 930
	private bool skippable;

	// Token: 0x040003A3 RID: 931
	private bool skip;
}
