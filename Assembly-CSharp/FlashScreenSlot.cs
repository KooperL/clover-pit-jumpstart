using System;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class FlashScreenSlot : MonoBehaviour
{
	// Token: 0x060008A1 RID: 2209 RVA: 0x00038B7C File Offset: 0x00036D7C
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

	// Token: 0x060008A2 RID: 2210 RVA: 0x00038C08 File Offset: 0x00036E08
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

	// Token: 0x060008A3 RID: 2211 RVA: 0x00038C5D File Offset: 0x00036E5D
	public static void SetSecondTexture(Texture2D texture, Vector2 textureSpeed)
	{
		FlashScreenSlot.instance.myImage2.enabled = true;
		FlashScreenSlot.instance.textureOffset2 = Vector2.zero;
		FlashScreenSlot.instance.textureSpeed2 = textureSpeed;
		FlashScreenSlot.instance.myImage2.texture = texture;
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x00038C99 File Offset: 0x00036E99
	public bool IsEnabled()
	{
		return this.myImage.enabled;
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x00038CA8 File Offset: 0x00036EA8
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

	// Token: 0x060008A6 RID: 2214 RVA: 0x00038D04 File Offset: 0x00036F04
	private void Awake()
	{
		FlashScreenSlot.instance = this;
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x00038D0C File Offset: 0x00036F0C
	private void OnDestroy()
	{
		if (FlashScreenSlot.instance == this)
		{
			FlashScreenSlot.instance = null;
		}
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x00038D24 File Offset: 0x00036F24
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
