using System;
using Panik;
using UnityEngine;

public class FloppySlotScript : MonoBehaviour
{
	// Token: 0x06000461 RID: 1121 RVA: 0x0001D6BC File Offset: 0x0001B8BC
	public static void SixSixSixTextureUpdateToIgnoredCallsLevel(bool computeCallsNum)
	{
		if (FloppySlotScript.instance == null)
		{
			return;
		}
		if (!GameplayData.NineNineNine_IsTime())
		{
			int num;
			if (computeCallsNum)
			{
				num = GameplayData.Phone_Ignored666CallsLevel_DefineAndReturn();
			}
			else
			{
				num = GameplayData.Phone_Ignored666CallsLevel_JustGet();
			}
			switch (num)
			{
			case 0:
				if (FloppySlotScript.instance.meshRenderer.sharedMaterial != FloppySlotScript.instance.material_666)
				{
					FloppySlotScript.instance.meshRenderer.sharedMaterial = FloppySlotScript.instance.material_666;
					return;
				}
				break;
			case 1:
				if (FloppySlotScript.instance.meshRenderer.sharedMaterial != FloppySlotScript.instance.material_666_Ignored1)
				{
					FloppySlotScript.instance.meshRenderer.sharedMaterial = FloppySlotScript.instance.material_666_Ignored1;
					return;
				}
				break;
			case 2:
				if (FloppySlotScript.instance.meshRenderer.sharedMaterial != FloppySlotScript.instance.material_666_Ignored2)
				{
					FloppySlotScript.instance.meshRenderer.sharedMaterial = FloppySlotScript.instance.material_666_Ignored2;
					return;
				}
				break;
			case 3:
				if (FloppySlotScript.instance.meshRenderer.sharedMaterial != FloppySlotScript.instance.material_666_Ignored3)
				{
					FloppySlotScript.instance.meshRenderer.sharedMaterial = FloppySlotScript.instance.material_666_Ignored3;
				}
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0001D7F6 File Offset: 0x0001B9F6
	public static void Initialize(bool isNewGame)
	{
		if (!isNewGame)
		{
			FloppySlotScript.SixSixSixTextureUpdateToIgnoredCallsLevel(false);
		}
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0001D801 File Offset: 0x0001BA01
	private void Awake()
	{
		FloppySlotScript.instance = this;
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0001D809 File Offset: 0x0001BA09
	private void Start()
	{
		this.meshRenderer.enabled = false;
		this.effectsHolder999.SetActive(false);
		this.effectsHolderSuper666.SetActive(false);
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0001D82F File Offset: 0x0001BA2F
	private void OnDestroy()
	{
		if (FloppySlotScript.instance == this)
		{
			FloppySlotScript.instance = null;
		}
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0001D844 File Offset: 0x0001BA44
	private void Update()
	{
		int num = GameplayData.Phone_Ignored666CallsLevel_JustGet();
		bool flag = GameplayData.DebtIndexGet() >= GameplayData.SixSixSix_GetMinimumDebtIndex();
		if (this.is666Time != flag)
		{
			this.is666Time = flag;
			if (this.is666Time)
			{
				this.meshRenderer.enabled = true;
			}
		}
		bool flag2 = GameplayData.NineNineNine_IsTime();
		if (this.is999Time != flag2)
		{
			this.is999Time = flag2;
			if (this.is999Time)
			{
				this.meshRenderer.enabled = true;
				this.meshRenderer.sharedMaterial = this.material_999;
				this.effectsHolder999.SetActive(true);
			}
		}
		bool flag3 = !this.is999Time && this.is666Time && GameplayData.DebtIndexGet() >= GameplayData.SuperSixSixSix_GetMinimumDebtIndex();
		if (this.isSuper666Time != flag3)
		{
			this.isSuper666Time = flag3;
			this.effectsHolderSuper666.SetActive(this.isSuper666Time);
		}
		float num2 = 0.5f + 0.5f * Util.AngleSin(Tick.PassedTime * 360f);
		this.material_666_Ignored1.SetColor("_Emission", this.emissionColor * num2);
		this.material_666_Ignored2.SetColor("_Emission", this.emissionColor * num2);
		this.material_666_Ignored3.SetColor("_Emission", this.emissionColor * num2);
		if (num > 0 && this.meshRenderer.sharedMaterial != this.material_999)
		{
			this.shakeTimer -= Tick.Time;
			if (this.shakeTimer < 0f)
			{
				if (this.shakeTimer < -0.25f)
				{
					this.shakeTimer = 2f;
				}
				switch (num)
				{
				case 1:
					this.holder.transform.localPosition = new Vector3(global::UnityEngine.Random.Range(-0.01f, 0.01f), global::UnityEngine.Random.Range(-0.01f, 0.01f), global::UnityEngine.Random.Range(-0.01f, 0.01f));
					return;
				case 2:
					this.holder.transform.localPosition = new Vector3(global::UnityEngine.Random.Range(-0.02f, 0.02f), global::UnityEngine.Random.Range(-0.02f, 0.02f), global::UnityEngine.Random.Range(-0.02f, 0.02f));
					return;
				case 3:
					this.holder.transform.localPosition = new Vector3(global::UnityEngine.Random.Range(-0.03f, 0.03f), global::UnityEngine.Random.Range(-0.03f, 0.03f), global::UnityEngine.Random.Range(-0.03f, 0.03f));
					break;
				default:
					return;
				}
			}
		}
	}

	public static FloppySlotScript instance;

	public GameObject holder;

	public MeshRenderer meshRenderer;

	public Material material_666;

	public Material material_666_Ignored1;

	public Material material_666_Ignored2;

	public Material material_666_Ignored3;

	public Material material_999;

	public GameObject effectsHolder999;

	public GameObject effectsHolderSuper666;

	private Color emissionColor = new Color(1f, 0.5f, 0f, 1f);

	private float shakeTimer;

	private bool is666Time;

	private bool isSuper666Time;

	private bool is999Time;
}
