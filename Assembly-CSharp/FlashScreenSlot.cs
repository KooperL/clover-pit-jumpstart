using System;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class FlashScreenSlot : MonoBehaviour
{
	// Token: 0x0600089A RID: 2202 RVA: 0x000388FC File Offset: 0x00036AFC
	public static FlashScreenSlot Flash(Color c, float alpha, float alphaSpeed)
	{
		if (FlashScreenSlot.instance == null)
		{
			return null;
		}
		FlashScreenSlot.instance.imageColor = c;
		FlashScreenSlot.instance.imageColor.a = alpha;
		FlashScreenSlot.instance.alpha = alpha;
		FlashScreenSlot.instance.alphaSpeed = alphaSpeed;
		FlashScreenSlot.instance.myImage.color = FlashScreenSlot.instance.imageColor;
		FlashScreenSlot.instance.myImage.texture = null;
		FlashScreenSlot.instance.myImage.enabled = true;
		return FlashScreenSlot.instance;
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x00038988 File Offset: 0x00036B88
	public static void SetTexture(Texture2D texture, Vector2 textureSpeed)
	{
		if (FlashScreenSlot.instance == null)
		{
			return;
		}
		FlashScreenSlot.instance.textureOffset = Vector2.zero;
		FlashScreenSlot.instance.textureSpeed = textureSpeed;
		FlashScreenSlot.instance.myImage.texture = texture;
		FlashScreenSlot.instance.myImage2.enabled = false;
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x000389DD File Offset: 0x00036BDD
	public static void SetSecondTexture(Texture2D texture, Vector2 textureSpeed)
	{
		FlashScreenSlot.instance.myImage2.enabled = true;
		FlashScreenSlot.instance.textureOffset2 = Vector2.zero;
		FlashScreenSlot.instance.textureSpeed2 = textureSpeed;
		FlashScreenSlot.instance.myImage2.texture = texture;
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x00038A19 File Offset: 0x00036C19
	public bool IsEnabled()
	{
		return this.myImage.enabled;
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x00038A28 File Offset: 0x00036C28
	public static void Stop()
	{
		if (FlashScreenSlot.instance == null)
		{
			return;
		}
		if (!FlashScreenSlot.instance.myImage.enabled)
		{
			return;
		}
		FlashScreenSlot.instance.alpha = 0f;
		FlashScreenSlot.instance.myImage.enabled = false;
		FlashScreenSlot.instance.myImage2.enabled = false;
	}

	// Token: 0x0600089F RID: 2207 RVA: 0x00038A84 File Offset: 0x00036C84
	private void Awake()
	{
		FlashScreenSlot.instance = this;
	}

	// Token: 0x060008A0 RID: 2208 RVA: 0x00038A8C File Offset: 0x00036C8C
	private void OnDestroy()
	{
		if (FlashScreenSlot.instance == this)
		{
			FlashScreenSlot.instance = null;
		}
	}

	// Token: 0x060008A1 RID: 2209 RVA: 0x00038AA4 File Offset: 0x00036CA4
	private void Update()
	{
		if (!this.myImage.enabled)
		{
			return;
		}
		this.alpha -= this.alphaSpeed * Tick.Time;
		if (this.alpha <= 0f)
		{
			this.alpha = 0f;
			this.myImage.enabled = false;
			this.myImage2.enabled = false;
		}
		this.imageColor.a = this.alpha;
		this.myImage.color = this.imageColor;
		this.myImage2.color = this.imageColor;
		if (this.myImage.texture != null)
		{
			this.textureOffset += this.textureSpeed * Tick.Time;
			this.myImage.uvRect = new Rect(this.textureOffset.x, this.textureOffset.y, 1f, 1f);
		}
		if (this.myImage.texture != null)
		{
			this.textureOffset2 += this.textureSpeed2 * Tick.Time;
			this.myImage2.uvRect = new Rect(this.textureOffset2.x, this.textureOffset2.y, 1f, 1f);
		}
	}

	public static FlashScreenSlot instance;

	public RawImage myImage;

	public RawImage myImage2;

	private Color imageColor;

	private float alpha;

	private float alphaSpeed = 1f;

	private Vector2 textureOffset = Vector2.zero;

	private Vector2 textureSpeed = new Vector2(0f, 0f);

	private Vector2 textureOffset2 = Vector2.zero;

	private Vector2 textureSpeed2 = new Vector2(0f, 0f);
}
