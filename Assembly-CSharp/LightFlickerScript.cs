using System;
using Panik;
using UnityEngine;

public class LightFlickerScript : MonoBehaviour
{
	// Token: 0x060007EB RID: 2027 RVA: 0x00033103 File Offset: 0x00031303
	private void Reset()
	{
		this.myLight = base.GetComponent<Light>();
	}

	// Token: 0x060007EC RID: 2028 RVA: 0x00033111 File Offset: 0x00031311
	private void Awake()
	{
		LightFlickerScript.instance = this;
	}

	// Token: 0x060007ED RID: 2029 RVA: 0x00033119 File Offset: 0x00031319
	private void OnDestroy()
	{
		if (LightFlickerScript.instance == this)
		{
			LightFlickerScript.instance = null;
		}
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x00033130 File Offset: 0x00031330
	private void Update()
	{
		if (!Tick.IsGameRunning)
		{
			return;
		}
		this.waveTimer += Time.deltaTime * 180f;
		this.waveTimer = Mathf.Repeat(this.waveTimer, 360f);
		bool flag = false;
		this.flickerTimer += Time.deltaTime;
		if (this.flickerTimer >= 0f)
		{
			flag = true;
			if (this.flickerTimer > 0.1f)
			{
				this.flickerTimer -= global::UnityEngine.Random.Range(0.1f, 0.5f) + global::UnityEngine.Random.Range(0.1f, 4.5f);
			}
		}
		if (this.titleScreenTurnOff && GeneralUiScript.instance.IsShowingTitleScreen())
		{
			flag = true;
		}
		float num = 0.2f + Mathf.Sin(this.waveTimer * 0.017453292f) * 0.4f;
		if (flag)
		{
			num = 0.1f;
		}
		this.myLight.intensity = num;
		if (this.lampMeshRenderer != null)
		{
			Material material = (flag ? this.lampMaterial_Off : this.lampMaterial_On);
			if (this.lampMeshRenderer.sharedMaterial != material)
			{
				this.lampMeshRenderer.sharedMaterial = material;
			}
		}
	}

	public static LightFlickerScript instance;

	private const float WAVE_SPEED = 180f;

	public Light myLight;

	public MeshRenderer lampMeshRenderer;

	public Material lampMaterial_On;

	public Material lampMaterial_Off;

	public bool titleScreenTurnOff;

	private float waveTimer;

	private float flickerTimer;
}
