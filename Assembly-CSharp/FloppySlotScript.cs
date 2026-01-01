using System;
using Panik;
using UnityEngine;

// Token: 0x02000055 RID: 85
public class FloppySlotScript : MonoBehaviour
{
	// Token: 0x060004EE RID: 1262 RVA: 0x00031D3C File Offset: 0x0002FF3C
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

	// Token: 0x060004EF RID: 1263 RVA: 0x000097E9 File Offset: 0x000079E9
	public static void Initialize(bool isNewGame)
	{
		if (!isNewGame)
		{
			FloppySlotScript.SixSixSixTextureUpdateToIgnoredCallsLevel(false);
		}
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x000097F4 File Offset: 0x000079F4
	private void Awake()
	{
		FloppySlotScript.instance = this;
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x000097FC File Offset: 0x000079FC
	private void Start()
	{
		this.meshRenderer.enabled = false;
		this.effectsHolder999.SetActive(false);
		this.effectsHolderSuper666.SetActive(false);
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x00009822 File Offset: 0x00007A22
	private void OnDestroy()
	{
		if (FloppySlotScript.instance == this)
		{
			FloppySlotScript.instance = null;
		}
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00031E78 File Offset: 0x00030078
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

	// Token: 0x04000486 RID: 1158
	public static FloppySlotScript instance;

	// Token: 0x04000487 RID: 1159
	public GameObject holder;

	// Token: 0x04000488 RID: 1160
	public MeshRenderer meshRenderer;

	// Token: 0x04000489 RID: 1161
	public Material material_666;

	// Token: 0x0400048A RID: 1162
	public Material material_666_Ignored1;

	// Token: 0x0400048B RID: 1163
	public Material material_666_Ignored2;

	// Token: 0x0400048C RID: 1164
	public Material material_666_Ignored3;

	// Token: 0x0400048D RID: 1165
	public Material material_999;

	// Token: 0x0400048E RID: 1166
	public GameObject effectsHolder999;

	// Token: 0x0400048F RID: 1167
	public GameObject effectsHolderSuper666;

	// Token: 0x04000490 RID: 1168
	private Color emissionColor = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x04000491 RID: 1169
	private float shakeTimer;

	// Token: 0x04000492 RID: 1170
	private bool is666Time;

	// Token: 0x04000493 RID: 1171
	private bool isSuper666Time;

	// Token: 0x04000494 RID: 1172
	private bool is999Time;
}
