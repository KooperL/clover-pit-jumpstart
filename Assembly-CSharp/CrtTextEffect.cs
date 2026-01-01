using System;
using Panik;
using TMPro;
using UnityEngine;

// Token: 0x020000AA RID: 170
public class CrtTextEffect : MonoBehaviour
{
	// Token: 0x06000975 RID: 2421 RVA: 0x0000D6ED File Offset: 0x0000B8ED
	private void Reset()
	{
		this.text = base.GetComponent<TextMeshProUGUI>();
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x0000D6ED File Offset: 0x0000B8ED
	private void Awake()
	{
		this.text = base.GetComponent<TextMeshProUGUI>();
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0000D6FB File Offset: 0x0000B8FB
	private void OnEnable()
	{
		this.waveTimer = (this.pausable ? Tick.PassedTimePausable : Tick.PassedTime);
		this.flickerTimer = (this.pausable ? Tick.PassedTimePausable : Tick.PassedTime);
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x0004DC84 File Offset: 0x0004BE84
	private void Update()
	{
		if (!Tick.IsGameRunning && this.pausable)
		{
			return;
		}
		this.waveTimer += 360f * Tick.Time;
		bool flag = false;
		bool flashingLightsReducedEnabled = Data.settings.flashingLightsReducedEnabled;
		this.flickerTimer += Tick.Time;
		if (Util.AngleSin(Mathf.Repeat(this.flickerTimer * 180f, 360f)) > 0.975f && !flashingLightsReducedEnabled)
		{
			flag = true;
		}
		float num = 0.9f + Util.AngleSin(Mathf.Repeat(this.waveTimer, 360f)) * 0.1f;
		if (flag)
		{
			num = 0f;
		}
		this.text.alpha = num;
	}

	// Token: 0x04000973 RID: 2419
	public const float WAVE_SPEED = 1f;

	// Token: 0x04000974 RID: 2420
	public TextMeshProUGUI text;

	// Token: 0x04000975 RID: 2421
	public bool pausable = true;

	// Token: 0x04000976 RID: 2422
	private float waveTimer;

	// Token: 0x04000977 RID: 2423
	private float flickerTimer;
}
