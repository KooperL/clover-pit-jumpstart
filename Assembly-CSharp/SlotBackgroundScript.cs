using System;
using Panik;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200006C RID: 108
public class SlotBackgroundScript : MonoBehaviour
{
	// Token: 0x06000796 RID: 1942 RVA: 0x0000C308 File Offset: 0x0000A508
	private void Awake()
	{
		SlotBackgroundScript.instance = this;
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x0000C310 File Offset: 0x0000A510
	private void OnDestroy()
	{
		if (SlotBackgroundScript.instance == this)
		{
			SlotBackgroundScript.instance = null;
		}
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x0003EA08 File Offset: 0x0003CC08
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

	// Token: 0x040006C1 RID: 1729
	public static SlotBackgroundScript instance;

	// Token: 0x040006C2 RID: 1730
	private const float BASIC_SPEED = 0.1f;

	// Token: 0x040006C3 RID: 1731
	public RawImage[] rawImages;

	// Token: 0x040006C4 RID: 1732
	private float[] speedMultipliers = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 1f, 1.5f, 2f, 2.5f, 3f };
}
