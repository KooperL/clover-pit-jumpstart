using System;
using Panik;
using TMPro;
using UnityEngine;

// Token: 0x02000094 RID: 148
public class GoldenToiletStickerScript : MonoBehaviour
{
	// Token: 0x060008EB RID: 2283 RVA: 0x0004A2BC File Offset: 0x000484BC
	private void RefreshVisuals()
	{
		if (Master.IsDemo)
		{
			return;
		}
		int num = Data.GameData.GameCompletitionPercentage_Get();
		Color white = Color.white;
		white.a = Mathf.Max(0.01f, (float)num / 100f);
		this.sprRend.color = white;
		this.percText.color = white;
		this.percText.text = num.ToString("00") + "%";
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x0000D0ED File Offset: 0x0000B2ED
	public static void RefreshVisualsStatic()
	{
		if (GoldenToiletStickerScript.instance == null)
		{
			return;
		}
		GoldenToiletStickerScript.instance.RefreshVisuals();
	}

	// Token: 0x060008ED RID: 2285 RVA: 0x0000D107 File Offset: 0x0000B307
	private void Awake()
	{
		GoldenToiletStickerScript.instance = this;
	}

	// Token: 0x060008EE RID: 2286 RVA: 0x0000D10F File Offset: 0x0000B30F
	private void Start()
	{
		if (Master.IsDemo)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.RefreshVisuals();
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x0000D12B File Offset: 0x0000B32B
	private void OnDestroy()
	{
		if (GoldenToiletStickerScript.instance == this)
		{
			GoldenToiletStickerScript.instance = null;
		}
	}

	// Token: 0x0400089D RID: 2205
	public static GoldenToiletStickerScript instance;

	// Token: 0x0400089E RID: 2206
	public SpriteRenderer sprRend;

	// Token: 0x0400089F RID: 2207
	public TextMeshPro percText;
}
