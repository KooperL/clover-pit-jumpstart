using System;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class PaletteHandler : MonoBehaviour
{
	// Token: 0x06000014 RID: 20 RVA: 0x0000281C File Offset: 0x00000A1C
	public void PaletteAnimationPlay(bool reset = false)
	{
		this.playing = true;
		if (reset)
		{
			this.paletteIndex = 0;
			this.animationTimer = 1f;
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x0000283A File Offset: 0x00000A3A
	public void PaletteAnimationStop()
	{
		this.playing = false;
		this.paletteIndex = 0;
		this.animationTimer = 1f;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002855 File Offset: 0x00000A55
	public void PaletteAnimationPause()
	{
		this.playing = false;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000285E File Offset: 0x00000A5E
	public void SetPalette(Texture2D newPalette)
	{
		this.currentPalette = newPalette;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002868 File Offset: 0x00000A68
	private void Reset()
	{
		this.currentPalette = Object.FindObjectOfType<Colors>().defaultSpritePalette;
		if (this.currentPalette == null)
		{
			Debug.LogError("Please define a default palette sprite inside the game master->child called 'Colors'");
			return;
		}
		bool flag = false;
		for (;;)
		{
			this.mySpr = base.GetComponent<SpriteRenderer>();
			if (this.mySpr != null)
			{
				break;
			}
			if (base.GetComponent<Image>() != null && base.GetComponent<DPSpritePaletteUI>() == null)
			{
				goto Block_5;
			}
			this.mySpr = base.gameObject.AddComponent<SpriteRenderer>();
			DPSpritePaletteUI component = base.GetComponent<DPSpritePaletteUI>();
			if (component != null)
			{
				Object.DestroyImmediate(component);
			}
			if (flag)
			{
				return;
			}
			flag = true;
		}
		this.paletteComponentSprite = base.GetComponent<DPSpritePalette>();
		if (this.paletteComponentSprite == null)
		{
			this.paletteComponentSprite = base.gameObject.AddComponent<DPSpritePalette>();
		}
		this.paletteComponentSprite.paletteTexture = this.currentPalette;
		return;
		Block_5:
		this.paletteComponentUI = base.GetComponent<DPSpritePaletteUI>();
		if (this.paletteComponentUI == null)
		{
			this.paletteComponentUI = base.gameObject.AddComponent<DPSpritePaletteUI>();
		}
		this.paletteComponentUI.paletteTexture = this.currentPalette;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002981 File Offset: 0x00000B81
	private void Awake()
	{
		this.mySpr = base.GetComponent<SpriteRenderer>();
		if (this.mySpr == null)
		{
			this.myImage = base.GetComponent<Image>();
		}
		if (this.playOnAwake)
		{
			this.PaletteAnimationPlay(false);
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x000029B8 File Offset: 0x00000BB8
	public void Update()
	{
		if (this.pausable && !Tick.IsGameRunning)
		{
			return;
		}
		if (this.playing)
		{
			this.animationTimer -= Tick.Time * this.paletteSpeed * this.paletteSpeedMult;
			if (this.animationTimer <= 0f)
			{
				this.animationTimer += 1f;
				this.paletteIndex++;
				if (this.paletteIndex > this.currentPalette.width - 1)
				{
					this.paletteIndex = 0;
					if (!this.loopPalette)
					{
						this.playing = false;
					}
				}
			}
		}
		if (this.paletteComponentSprite != null)
		{
			if (this.paletteComponentSprite.paletteTexture != this.currentPalette)
			{
				this.paletteComponentSprite.paletteTexture = this.currentPalette;
			}
			if (this.paletteComponentSprite.PaletteIndex != this.paletteIndex)
			{
				this.paletteComponentSprite.PaletteIndex = this.paletteIndex;
				return;
			}
		}
		else if (this.paletteComponentUI != null)
		{
			if (this.paletteComponentUI.paletteTexture != this.currentPalette)
			{
				this.paletteComponentUI.paletteTexture = this.currentPalette;
			}
			if (this.paletteComponentUI.PaletteIndex != this.paletteIndex)
			{
				this.paletteComponentUI.PaletteIndex = this.paletteIndex;
			}
		}
	}

	public SpriteRenderer mySpr;

	public Image myImage;

	public DPSpritePalette paletteComponentSprite;

	public DPSpritePaletteUI paletteComponentUI;

	public bool pausable = true;

	public Texture2D currentPalette;

	[NonSerialized]
	public int paletteIndex;

	[NonSerialized]
	public float paletteSpeed = 12f;

	[NonSerialized]
	public float paletteSpeedMult = 1f;

	public bool loopPalette;

	public bool playOnAwake;

	private float animationTimer = 1f;

	private bool playing;
}
