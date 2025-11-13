using System;
using Panik;
using UnityEngine;

public class LightFlickerScript : MonoBehaviour
{
	// Token: 0x060007F2 RID: 2034 RVA: 0x000332EB File Offset: 0x000314EB
	private void Reset()
	{
		this.myLight = base.GetComponent<Light>();
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x000332F9 File Offset: 0x000314F9
	private void Awake()
	{
		LightFlickerScript.instance = this;
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x00033301 File Offset: 0x00031501
	private void OnDestroy()
	{
		if (LightFlickerScript.instance == this)
		{
			LightFlickerScript.instance = null;
		}
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x00033318 File Offset: 0x00031518
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
