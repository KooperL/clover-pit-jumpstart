using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class TerminalNodeScript : TerminalButton
{
	// Token: 0x06000A49 RID: 2633 RVA: 0x000469D6 File Offset: 0x00044BD6
	public PowerupScript PowerupAssigned_Get()
	{
		return this.myPowerup;
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x000469E0 File Offset: 0x00044BE0
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

	// Token: 0x06000A4B RID: 2635 RVA: 0x00046CE0 File Offset: 0x00044EE0
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

	// Token: 0x06000A4C RID: 2636 RVA: 0x00046D68 File Offset: 0x00044F68
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

	// Token: 0x06000A4D RID: 2637 RVA: 0x00046E48 File Offset: 0x00045048
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

	// Token: 0x06000A4E RID: 2638 RVA: 0x00046E9F File Offset: 0x0004509F
	public override void Start()
	{
		base.Start();
		this.nodeImage.sprite = this.nodeSprite_Off;
		this.cameraGame = CameraGame.firstInstance.myCamera;
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x00046EC8 File Offset: 0x000450C8
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

	private Camera cameraGame;

	public Image nodeImage;

	public Sprite nodeSprite_Off;

	public Sprite nodeSprite_On;

	public Transform MeshShifter;

	public Transform MeshHolder;

	public Transform extraIconRotator;

	public Image extraIconImage;

	public Sprite extraSprite_Locked;

	public Sprite extraSprite_Offer;

	public Sprite extraSprite_OutOfStock;

	public Sprite extraSprite_Nothing;

	public Sprite extraSprite_JustUnlocked;

	private Dictionary<PowerupScript, MeshRenderer> powerupMeshRendererCopies = new Dictionary<PowerupScript, MeshRenderer>();

	private Dictionary<PowerupScript, SkinnedMeshRenderer> powerupSkinnedMeshRendererCopies = new Dictionary<PowerupScript, SkinnedMeshRenderer>();

	private PowerupScript myPowerup;

	private Vector3 _hoveredShifterOffset = Vector3.zero;
}
