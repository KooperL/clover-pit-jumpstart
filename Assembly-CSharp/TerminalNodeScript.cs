using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F6 RID: 246
public class TerminalNodeScript : TerminalButton
{
	// Token: 0x06000C0D RID: 3085 RVA: 0x0000FE56 File Offset: 0x0000E056
	public PowerupScript PowerupAssigned_Get()
	{
		return this.myPowerup;
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x00060B38 File Offset: 0x0005ED38
	public void PowerupAssigned_Set(PowerupScript powerup, TerminalScript.TerminalPowerupState terminalPowerupState)
	{
		if (this.myPowerup != null)
		{
			if (this.powerupMeshRendererCopies.ContainsKey(this.myPowerup))
			{
				this.powerupMeshRendererCopies[this.myPowerup].gameObject.SetActive(false);
			}
			if (this.powerupSkinnedMeshRendererCopies.ContainsKey(this.myPowerup))
			{
				this.powerupSkinnedMeshRendererCopies[this.myPowerup].gameObject.SetActive(false);
			}
		}
		this.myPowerup = powerup;
		if (powerup == null)
		{
			this.extraIconImage.sprite = this.extraSprite_Nothing;
			return;
		}
		MeshRenderer meshRenderer = powerup.MeshRendererGet();
		SkinnedMeshRenderer skinnedMeshRenderer = powerup.SkinnedMeshRendererGet();
		Vector3 vector = 0.075f * powerup.GetBoundingBoxSizeNormalized();
		if (meshRenderer != null)
		{
			if (!this.powerupMeshRendererCopies.ContainsKey(powerup))
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(meshRenderer.gameObject, this.MeshHolder);
				gameObject.transform.localPosition = Vector3.zero + TerminalNodeScript.PowerupPositionOffset(powerup.identifier);
				gameObject.transform.localEulerAngles = new Vector3(-110f, 0f, -90f) + TerminalNodeScript.PowerupAngleOffset(powerup.identifier);
				gameObject.transform.localScale = vector * this.PowerupScaleMultiplier(powerup.identifier);
				this.powerupMeshRendererCopies.Add(powerup, gameObject.GetComponent<MeshRenderer>());
			}
			else
			{
				this.powerupMeshRendererCopies[powerup].gameObject.SetActive(true);
			}
			this.powerupMeshRendererCopies[powerup].sharedMaterial = powerup.MaterialDefault_Get();
		}
		else if (skinnedMeshRenderer != null)
		{
			if (!this.powerupSkinnedMeshRendererCopies.ContainsKey(powerup))
			{
				GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(skinnedMeshRenderer.transform.parent.gameObject, this.MeshHolder);
				gameObject2.transform.localPosition = Vector3.zero + TerminalNodeScript.PowerupPositionOffset(powerup.identifier);
				gameObject2.transform.localEulerAngles = new Vector3(0f, 90f, -20f) + TerminalNodeScript.PowerupAngleOffset(powerup.identifier);
				gameObject2.transform.localScale = vector * this.PowerupScaleMultiplier(powerup.identifier);
				this.powerupSkinnedMeshRendererCopies.Add(powerup, gameObject2.GetComponentInChildren<SkinnedMeshRenderer>());
			}
			else
			{
				this.powerupSkinnedMeshRendererCopies[powerup].gameObject.SetActive(true);
			}
			this.powerupSkinnedMeshRendererCopies[powerup].sharedMaterial = powerup.MaterialDefault_Get();
		}
		switch (terminalPowerupState)
		{
		case TerminalScript.TerminalPowerupState.owned:
			this.extraIconImage.sprite = this.extraSprite_Nothing;
			return;
		case TerminalScript.TerminalPowerupState.offered:
			this.extraIconImage.sprite = this.extraSprite_Offer;
			return;
		case TerminalScript.TerminalPowerupState.locked:
			this.extraIconImage.sprite = this.extraSprite_Locked;
			return;
		case TerminalScript.TerminalPowerupState.outOfStock:
			this.extraIconImage.sprite = this.extraSprite_OutOfStock;
			return;
		case TerminalScript.TerminalPowerupState.justUnlocked:
			this.extraIconImage.sprite = this.extraSprite_JustUnlocked;
			return;
		default:
			return;
		}
	}

	// Token: 0x06000C0F RID: 3087 RVA: 0x00060E38 File Offset: 0x0005F038
	public static Vector3 PowerupPositionOffset(PowerupScript.Identifier identifier)
	{
		switch (identifier)
		{
		case PowerupScript.Identifier.Hole_Circle:
			return new Vector3(0f, -0.012f, 0f);
		case PowerupScript.Identifier.Hole_Romboid:
			return new Vector3(0f, -0.008f, 0f);
		case PowerupScript.Identifier.Hole_Cross:
			return new Vector3(0f, -0.01f, 0f);
		default:
			if (identifier != PowerupScript.Identifier.Dice_20)
			{
				return Vector3.zero;
			}
			return new Vector3(0f, -0.0025f, 0f);
		}
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x00060EC0 File Offset: 0x0005F0C0
	public static Vector3 PowerupAngleOffset(PowerupScript.Identifier identifier)
	{
		if (identifier <= PowerupScript.Identifier.GrandmasPurse)
		{
			switch (identifier)
			{
			case PowerupScript.Identifier.Skeleton_Arm1:
				return new Vector3(0f, 90f, 0f);
			case PowerupScript.Identifier.Skeleton_Arm2:
				return new Vector3(0f, 90f, 0f);
			case PowerupScript.Identifier.Skeleton_Leg1:
				return new Vector3(0f, 90f, 0f);
			case PowerupScript.Identifier.Skeleton_Leg2:
				return new Vector3(0f, 90f, 0f);
			default:
				if (identifier == PowerupScript.Identifier.GrandmasPurse)
				{
					return new Vector3(-90f, 0f, 0f);
				}
				break;
			}
		}
		else
		{
			if (identifier == PowerupScript.Identifier.CloverVoucher)
			{
				return new Vector3(-90f, 0f, 0f);
			}
			if (identifier - PowerupScript.Identifier.SymbolInstant_Lemon <= 6)
			{
				return new Vector3(-90f, 0f, 0f);
			}
		}
		return Vector3.zero;
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x00060FA0 File Offset: 0x0005F1A0
	public float PowerupScaleMultiplier(PowerupScript.Identifier identifier)
	{
		if (identifier == PowerupScript.Identifier.Hole_Circle)
		{
			return 0.9f;
		}
		if (identifier == PowerupScript.Identifier.Boardgame_M_Ditale)
		{
			return 0.75f;
		}
		switch (identifier)
		{
		case PowerupScript.Identifier.Dice_4:
			return 0.9f;
		case PowerupScript.Identifier.Dice_6:
			return 0.7f;
		case PowerupScript.Identifier.Dice_20:
			return 0.9f;
		default:
			return 1f;
		}
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x0000FE5E File Offset: 0x0000E05E
	public override void Start()
	{
		base.Start();
		this.nodeImage.sprite = this.nodeSprite_Off;
		this.cameraGame = CameraGame.firstInstance.myCamera;
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x00060FF8 File Offset: 0x0005F1F8
	public override void Update()
	{
		base.Update();
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		this._hoveredShifterOffset = (base.HoveredState_Get() ? new Vector3(0f, 0.008f, 0f) : Vector3.zero);
		this.MeshShifter.localPosition = Vector3.Lerp(this.MeshShifter.localPosition, this._hoveredShifterOffset, Time.deltaTime * 10f);
		Sprite sprite = (base.HoveredState_Get() ? this.nodeSprite_On : this.nodeSprite_Off);
		if (this.nodeImage.sprite != sprite)
		{
			this.nodeImage.sprite = sprite;
		}
		bool flag = this.myPowerup != null;
		if (this.nodeImage.enabled != flag)
		{
			this.nodeImage.enabled = flag;
		}
		this.extraIconRotator.SetLocalYAngle(90f + Util.AxisToAngle2D(this.cameraGame.transform.position.x - this.extraIconRotator.transform.position.x, -(this.cameraGame.transform.position.z - this.extraIconRotator.transform.position.z)));
		this.extraIconImage.transform.SetYAngle(this.cameraGame.transform.eulerAngles.y - 180f);
	}

	// Token: 0x04000CDC RID: 3292
	private Camera cameraGame;

	// Token: 0x04000CDD RID: 3293
	public Image nodeImage;

	// Token: 0x04000CDE RID: 3294
	public Sprite nodeSprite_Off;

	// Token: 0x04000CDF RID: 3295
	public Sprite nodeSprite_On;

	// Token: 0x04000CE0 RID: 3296
	public Transform MeshShifter;

	// Token: 0x04000CE1 RID: 3297
	public Transform MeshHolder;

	// Token: 0x04000CE2 RID: 3298
	public Transform extraIconRotator;

	// Token: 0x04000CE3 RID: 3299
	public Image extraIconImage;

	// Token: 0x04000CE4 RID: 3300
	public Sprite extraSprite_Locked;

	// Token: 0x04000CE5 RID: 3301
	public Sprite extraSprite_Offer;

	// Token: 0x04000CE6 RID: 3302
	public Sprite extraSprite_OutOfStock;

	// Token: 0x04000CE7 RID: 3303
	public Sprite extraSprite_Nothing;

	// Token: 0x04000CE8 RID: 3304
	public Sprite extraSprite_JustUnlocked;

	// Token: 0x04000CE9 RID: 3305
	private Dictionary<PowerupScript, MeshRenderer> powerupMeshRendererCopies = new Dictionary<PowerupScript, MeshRenderer>();

	// Token: 0x04000CEA RID: 3306
	private Dictionary<PowerupScript, SkinnedMeshRenderer> powerupSkinnedMeshRendererCopies = new Dictionary<PowerupScript, SkinnedMeshRenderer>();

	// Token: 0x04000CEB RID: 3307
	private PowerupScript myPowerup;

	// Token: 0x04000CEC RID: 3308
	private Vector3 _hoveredShifterOffset = Vector3.zero;
}
