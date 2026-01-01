using System;
using Panik;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B4 RID: 180
public class FlashScreenSlot : MonoBehaviour
{
	// Token: 0x060009CC RID: 2508 RVA: 0x0004FBDC File Offset: 0x0004DDDC
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

	// Token: 0x060009CD RID: 2509 RVA: 0x0004FC68 File Offset: 0x0004DE68
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

	// Token: 0x060009CE RID: 2510 RVA: 0x0000DB92 File Offset: 0x0000BD92
	public static void SetSecondTexture(Texture2D texture, Vector2 textureSpeed)
	{
		FlashScreenSlot.instance.myImage2.enabled = true;
		FlashScreenSlot.instance.textureOffset2 = Vector2.zero;
		FlashScreenSlot.instance.textureSpeed2 = textureSpeed;
		FlashScreenSlot.instance.myImage2.texture = texture;
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x0000DBCE File Offset: 0x0000BDCE
	public bool IsEnabled()
	{
		return this.myImage.enabled;
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x0004FCC0 File Offset: 0x0004DEC0
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

	// Token: 0x060009D1 RID: 2513 RVA: 0x0000DBDB File Offset: 0x0000BDDB
	private void Awake()
	{
		FlashScreenSlot.instance = this;
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x0000DBE3 File Offset: 0x0000BDE3
	private void OnDestroy()
	{
		if (FlashScreenSlot.instance == this)
		{
			FlashScreenSlot.instance = null;
		}
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x0004FD1C File Offset: 0x0004DF1C
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

	// Token: 0x040009E1 RID: 2529
	public static FlashScreenSlot instance;

	// Token: 0x040009E2 RID: 2530
	public RawImage myImage;

	// Token: 0x040009E3 RID: 2531
	public RawImage myImage2;

	// Token: 0x040009E4 RID: 2532
	private Color imageColor;

	// Token: 0x040009E5 RID: 2533
	private float alpha;

	// Token: 0x040009E6 RID: 2534
	private float alphaSpeed = 1f;

	// Token: 0x040009E7 RID: 2535
	private Vector2 textureOffset = Vector2.zero;

	// Token: 0x040009E8 RID: 2536
	private Vector2 textureSpeed = new Vector2(0f, 0f);

	// Token: 0x040009E9 RID: 2537
	private Vector2 textureOffset2 = Vector2.zero;

	// Token: 0x040009EA RID: 2538
	private Vector2 textureSpeed2 = new Vector2(0f, 0f);
}
