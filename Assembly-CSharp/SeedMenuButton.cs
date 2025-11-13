using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedMenuButton : MonoBehaviour
{
	// Token: 0x06000A13 RID: 2579 RVA: 0x00044F67 File Offset: 0x00043167
	public bool IsHovered()
	{
		return this.hovered;
	}

	// Token: 0x06000A14 RID: 2580 RVA: 0x00044F6F File Offset: 0x0004316F
	public void HoveredSet(bool state)
	{
		this.hovered = state;
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x00044F78 File Offset: 0x00043178
	public void FlashRed()
	{
		this.redTextTimer = 0.5f;
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x00044F88 File Offset: 0x00043188
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

	// Token: 0x06000A17 RID: 2583 RVA: 0x00045031 File Offset: 0x00043231
	public uint GetNumber()
	{
		return this.myNumber;
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x0004503C File Offset: 0x0004323C
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

	// Token: 0x06000A19 RID: 2585 RVA: 0x0004509E File Offset: 0x0004329E
	private void OnDestroy()
	{
		if (!this.isOkButton && !this.isBackButton)
		{
			SeedMenuButton.cyphersList.Remove(this);
		}
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x000450BC File Offset: 0x000432BC
	private void Start()
	{
		this.originalColor = this.text.color;
		this.cursorImage.enabled = false;
		if (this.isOkButton)
		{
			this.text.text = Translation.Get("MENU_SEED_INPUT_OK_BUTTON");
		}
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x000450F8 File Offset: 0x000432F8
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
