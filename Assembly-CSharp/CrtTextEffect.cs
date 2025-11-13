using System;
using Panik;
using TMPro;
using UnityEngine;

public class CrtTextEffect : MonoBehaviour
{
	// Token: 0x06000852 RID: 2130 RVA: 0x00036795 File Offset: 0x00034995
	private void Reset()
	{
		this.text = base.GetComponent<TextMeshProUGUI>();
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x000367A3 File Offset: 0x000349A3
	private void Awake()
	{
		this.text = base.GetComponent<TextMeshProUGUI>();
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x000367B1 File Offset: 0x000349B1
	private void OnEnable()
	{
		this.waveTimer = (this.pausable ? Tick.PassedTimePausable : Tick.PassedTime);
		this.flickerTimer = (this.pausable ? Tick.PassedTimePausable : Tick.PassedTime);
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x000367E8 File Offset: 0x000349E8
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
