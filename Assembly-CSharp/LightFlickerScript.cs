using System;
using Panik;
using UnityEngine;

// Token: 0x02000096 RID: 150
public class LightFlickerScript : MonoBehaviour
{
	// Token: 0x060008FD RID: 2301 RVA: 0x0000D1A6 File Offset: 0x0000B3A6
	private void Reset()
	{
		this.myLight = base.GetComponent<Light>();
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0000D1B4 File Offset: 0x0000B3B4
	private void Awake()
	{
		LightFlickerScript.instance = this;
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0000D1BC File Offset: 0x0000B3BC
	private void OnDestroy()
	{
		if (LightFlickerScript.instance == this)
		{
			LightFlickerScript.instance = null;
		}
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0004A420 File Offset: 0x00048620
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

	// Token: 0x040008AC RID: 2220
	public static LightFlickerScript instance;

	// Token: 0x040008AD RID: 2221
	private const float WAVE_SPEED = 180f;

	// Token: 0x040008AE RID: 2222
	public Light myLight;

	// Token: 0x040008AF RID: 2223
	public MeshRenderer lampMeshRenderer;

	// Token: 0x040008B0 RID: 2224
	public Material lampMaterial_On;

	// Token: 0x040008B1 RID: 2225
	public Material lampMaterial_Off;

	// Token: 0x040008B2 RID: 2226
	public bool titleScreenTurnOff;

	// Token: 0x040008B3 RID: 2227
	private float waveTimer;

	// Token: 0x040008B4 RID: 2228
	private float flickerTimer;
}
