using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EB RID: 235
public class SeedMenuButton : MonoBehaviour
{
	// Token: 0x06000BBF RID: 3007 RVA: 0x0000FA83 File Offset: 0x0000DC83
	public bool IsHovered()
	{
		return this.hovered;
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x0000FA8B File Offset: 0x0000DC8B
	public void HoveredSet(bool state)
	{
		this.hovered = state;
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x0000FA94 File Offset: 0x0000DC94
	public void FlashRed()
	{
		this.redTextTimer = 0.5f;
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x0005E788 File Offset: 0x0005C988
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

	// Token: 0x06000BC3 RID: 3011 RVA: 0x0000FAA1 File Offset: 0x0000DCA1
	public uint GetNumber()
	{
		return this.myNumber;
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x0005E834 File Offset: 0x0005CA34
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

	// Token: 0x06000BC5 RID: 3013 RVA: 0x0000FAA9 File Offset: 0x0000DCA9
	private void OnDestroy()
	{
		if (!this.isOkButton && !this.isBackButton)
		{
			SeedMenuButton.cyphersList.Remove(this);
		}
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x0000FAC7 File Offset: 0x0000DCC7
	private void Start()
	{
		this.originalColor = this.text.color;
		this.cursorImage.enabled = false;
		if (this.isOkButton)
		{
			this.text.text = Translation.Get("MENU_SEED_INPUT_OK_BUTTON");
		}
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x0005E898 File Offset: 0x0005CA98
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

	// Token: 0x04000C69 RID: 3177
	private static List<SeedMenuButton> cyphersList = new List<SeedMenuButton>();

	// Token: 0x04000C6A RID: 3178
	[NonSerialized]
	public RectTransform rectTransform;

	// Token: 0x04000C6B RID: 3179
	[NonSerialized]
	public TextMeshProUGUI text;

	// Token: 0x04000C6C RID: 3180
	[NonSerialized]
	public Image cursorImage;

	// Token: 0x04000C6D RID: 3181
	public SeedMenuButton leftButton;

	// Token: 0x04000C6E RID: 3182
	public SeedMenuButton rightButton;

	// Token: 0x04000C6F RID: 3183
	private bool isOkButton;

	// Token: 0x04000C70 RID: 3184
	private bool isBackButton;

	// Token: 0x04000C71 RID: 3185
	private bool hovered;

	// Token: 0x04000C72 RID: 3186
	private Color redColor = Color.red;

	// Token: 0x04000C73 RID: 3187
	private Color originalColor;

	// Token: 0x04000C74 RID: 3188
	private float redTextTimer;

	// Token: 0x04000C75 RID: 3189
	public bool acceptNumbers = true;

	// Token: 0x04000C76 RID: 3190
	public uint decimalPlaceN;

	// Token: 0x04000C77 RID: 3191
	private uint myNumber;

	// Token: 0x04000C78 RID: 3192
	private Vector2 textStartingPos;
}
