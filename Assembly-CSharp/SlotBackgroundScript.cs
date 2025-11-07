using System;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class SlotBackgroundScript : MonoBehaviour
{
	// Token: 0x060006EF RID: 1775 RVA: 0x0002C836 File Offset: 0x0002AA36
	private void Awake()
	{
		SlotBackgroundScript.instance = this;
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x0002C83E File Offset: 0x0002AA3E
	private void OnDestroy()
	{
		if (SlotBackgroundScript.instance == this)
		{
			SlotBackgroundScript.instance = null;
		}
	}

	// Token: 0x060006F1 RID: 1777 RVA: 0x0002C854 File Offset: 0x0002AA54
	private void Update()
	{
		if (!Tick.IsGameRunning)
		{
			return;
		}
		for (int i = 0; i < this.rawImages.Length; i++)
		{
			float num = 0.1f * this.speedMultipliers[i];
			Rect uvRect = this.rawImages[i].uvRect;
			Vector2 position = uvRect.position;
			position.y += num * Tick.Time;
			position.y = Mathf.Repeat(position.y, 1f);
			uvRect.position = position;
			this.rawImages[i].uvRect = uvRect;
		}
	}

	public static SlotBackgroundScript instance;

	private const float BASIC_SPEED = 0.1f;

	public RawImage[] rawImages;

	private float[] speedMultipliers = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 1f, 1.5f, 2f, 2.5f, 3f };
}
