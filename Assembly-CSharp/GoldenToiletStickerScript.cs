using System;
using Panik;
using TMPro;
using UnityEngine;

public class GoldenToiletStickerScript : MonoBehaviour
{
	// Token: 0x060007E0 RID: 2016 RVA: 0x000330C0 File Offset: 0x000312C0
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

	// Token: 0x060007E1 RID: 2017 RVA: 0x00033133 File Offset: 0x00031333
	public static void RefreshVisualsStatic()
	{
		if (GoldenToiletStickerScript.instance == null)
		{
			return;
		}
		GoldenToiletStickerScript.instance.RefreshVisuals();
	}

	// Token: 0x060007E2 RID: 2018 RVA: 0x0003314D File Offset: 0x0003134D
	private void Awake()
	{
		GoldenToiletStickerScript.instance = this;
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x00033155 File Offset: 0x00031355
	private void Start()
	{
		if (Master.IsDemo)
		{
			base.gameObject.SetActive(false);
			return;
		}
		this.RefreshVisuals();
	}

	// Token: 0x060007E4 RID: 2020 RVA: 0x00033171 File Offset: 0x00031371
	private void OnDestroy()
	{
		if (GoldenToiletStickerScript.instance == this)
		{
			GoldenToiletStickerScript.instance = null;
		}
	}

	public static GoldenToiletStickerScript instance;

	public SpriteRenderer sprRend;

	public TextMeshPro percText;
}
