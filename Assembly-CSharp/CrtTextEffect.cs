using System;
using Panik;
using TMPro;
using UnityEngine;

public class CrtTextEffect : MonoBehaviour
{
	// Token: 0x0600084B RID: 2123 RVA: 0x00036541 File Offset: 0x00034741
	private void Reset()
	{
		this.text = base.GetComponent<TextMeshProUGUI>();
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x0003654F File Offset: 0x0003474F
	private void Awake()
	{
		this.text = base.GetComponent<TextMeshProUGUI>();
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x0003655D File Offset: 0x0003475D
	private void OnEnable()
	{
		this.waveTimer = (this.pausable ? Tick.PassedTimePausable : Tick.PassedTime);
		this.flickerTimer = (this.pausable ? Tick.PassedTimePausable : Tick.PassedTime);
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x00036594 File Offset: 0x00034794
	private void Update()
	{
		if (!Tick.IsGameRunning && this.pausable)
		{
			return;
		}
		this.waveTimer += 360f * Tick.Time;
		bool flag = false;
		this.flickerTimer += Tick.Time;
		if (Util.AngleSin(Mathf.Repeat(this.flickerTimer * 180f, 360f)) > 0.975f)
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

	public const float WAVE_SPEED = 1f;

	public TextMeshProUGUI text;

	public bool pausable = true;

	private float waveTimer;

	private float flickerTimer;
}
