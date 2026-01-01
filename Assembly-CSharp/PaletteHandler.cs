using System;
using Panik;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000006 RID: 6
public class PaletteHandler : MonoBehaviour
{
	// Token: 0x06000019 RID: 25 RVA: 0x00007341 File Offset: 0x00005541
	public void PaletteAnimationPlay(bool reset = false)
	{
		this.playing = true;
		if (reset)
		{
			this.paletteIndex = 0;
			this.animationTimer = 1f;
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x0000735F File Offset: 0x0000555F
	public void PaletteAnimationStop()
	{
		this.playing = false;
		this.paletteIndex = 0;
		this.animationTimer = 1f;
	}

	// Token: 0x0600001B RID: 27 RVA: 0x0000737A File Offset: 0x0000557A
	public void PaletteAnimationPause()
	{
		this.playing = false;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00007383 File Offset: 0x00005583
	public void SetPalette(Texture2D newPalette)
	{
		this.currentPalette = newPalette;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000165C0 File Offset: 0x000147C0
	private void Reset()
	{
		this.currentPalette = global::UnityEngine.Object.FindObjectOfType<Colors>().defaultSpritePalette;
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
				global::UnityEngine.Object.DestroyImmediate(component);
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

	// Token: 0x0600001E RID: 30 RVA: 0x0000738C File Offset: 0x0000558C
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

	// Token: 0x0600001F RID: 31 RVA: 0x000166DC File Offset: 0x000148DC
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

	// Token: 0x04000018 RID: 24
	public SpriteRenderer mySpr;

	// Token: 0x04000019 RID: 25
	public Image myImage;

	// Token: 0x0400001A RID: 26
	public DPSpritePalette paletteComponentSprite;

	// Token: 0x0400001B RID: 27
	public DPSpritePaletteUI paletteComponentUI;

	// Token: 0x0400001C RID: 28
	public bool pausable = true;

	// Token: 0x0400001D RID: 29
	public Texture2D currentPalette;

	// Token: 0x0400001E RID: 30
	[NonSerialized]
	public int paletteIndex;

	// Token: 0x0400001F RID: 31
	[NonSerialized]
	public float paletteSpeed = 12f;

	// Token: 0x04000020 RID: 32
	[NonSerialized]
	public float paletteSpeedMult = 1f;

	// Token: 0x04000021 RID: 33
	public bool loopPalette;

	// Token: 0x04000022 RID: 34
	public bool playOnAwake;

	// Token: 0x04000023 RID: 35
	private float animationTimer = 1f;

	// Token: 0x04000024 RID: 36
	private bool playing;
}
