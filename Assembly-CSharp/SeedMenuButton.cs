using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedMenuButton : MonoBehaviour
{
	// Token: 0x060009FE RID: 2558 RVA: 0x0004486B File Offset: 0x00042A6B
	public bool IsHovered()
	{
		return this.hovered;
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x00044873 File Offset: 0x00042A73
	public void HoveredSet(bool state)
	{
		this.hovered = state;
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x0004487C File Offset: 0x00042A7C
	public void FlashRed()
	{
		this.redTextTimer = 0.5f;
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x0004488C File Offset: 0x00042A8C
	public void SetNumber(uint n)
	{
		if (!this.acceptNumbers)
		{
			Debug.LogError("Wheel doesn't accept numbers. Wheel gObj: " + base.gameObject.name);
		}
		if (this.myNumber != n)
		{
			this.text.rectTransform.anchoredPosition = new Vector2(0f, Mathf.Clamp((n - this.myNumber) * 24f, -24f, 24f));
			Sound.Play_Unpausable("SoundSeedNumberChange", 1f, 1f + n * 0.025f);
		}
		this.myNumber = n;
		this.text.text = n.ToString();
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x00044935 File Offset: 0x00042B35
	public uint GetNumber()
	{
		return this.myNumber;
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x00044940 File Offset: 0x00042B40
	private void Awake()
	{
		if (!this.isOkButton && !this.isBackButton)
		{
			SeedMenuButton.cyphersList.Add(this);
		}
		this.rectTransform = base.GetComponent<RectTransform>();
		this.text = base.GetComponentInChildren<TextMeshProUGUI>();
		this.cursorImage = base.GetComponentInChildren<Image>();
		this.textStartingPos = this.text.rectTransform.anchoredPosition;
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x000449A2 File Offset: 0x00042BA2
	private void OnDestroy()
	{
		if (!this.isOkButton && !this.isBackButton)
		{
			SeedMenuButton.cyphersList.Remove(this);
		}
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x000449C0 File Offset: 0x00042BC0
	private void Start()
	{
		this.originalColor = this.text.color;
		this.cursorImage.enabled = false;
		if (this.isOkButton)
		{
			this.text.text = Translation.Get("MENU_SEED_INPUT_OK_BUTTON");
		}
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x000449FC File Offset: 0x00042BFC
	private void Update()
	{
		bool flag = this.hovered;
		if (flag != this.cursorImage.enabled)
		{
			this.cursorImage.enabled = flag;
		}
		Vector2 vector = new Vector2(0f, this.hovered ? 5f : 0f);
		this.text.rectTransform.anchoredPosition = Vector2.Lerp(this.text.rectTransform.anchoredPosition, this.textStartingPos + vector, Tick.Time * 10f);
		this.redTextTimer -= Tick.Time;
		Color color = ((this.redTextTimer > 0f && Util.AngleSin(Tick.PassedTime * 1440f) > 0f) ? this.redColor : this.originalColor);
		if (this.text.color != color)
		{
			this.text.color = color;
		}
	}

	private static List<SeedMenuButton> cyphersList = new List<SeedMenuButton>();

	[NonSerialized]
	public RectTransform rectTransform;

	[NonSerialized]
	public TextMeshProUGUI text;

	[NonSerialized]
	public Image cursorImage;

	public SeedMenuButton leftButton;

	public SeedMenuButton rightButton;

	private bool isOkButton;

	private bool isBackButton;

	private bool hovered;

	private Color redColor = Color.red;

	private Color originalColor;

	private float redTextTimer;

	public bool acceptNumbers = true;

	public uint decimalPlaceN;

	private uint myNumber;

	private Vector2 textStartingPos;
}
