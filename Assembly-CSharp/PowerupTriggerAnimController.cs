using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupTriggerAnimController : MonoBehaviour
{
	// Token: 0x060009C1 RID: 2497 RVA: 0x00040B55 File Offset: 0x0003ED55
	public static bool ShouldHide()
	{
		return PowerupTriggerAnimController.instance == null || GameplayMaster.instance == null || GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.intro;
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x00040B80 File Offset: 0x0003ED80
	public static void AddAnimation(PowerupScript powerup, RunModifierScript.Identifier runModifierCard, PowerupTriggerAnimController.AnimationCapsule.AnimationKind animKind)
	{
		PowerupTriggerAnimController.AnimationCapsule animationCapsule;
		if (PowerupTriggerAnimController.instance.animationsPool.Count > 0)
		{
			animationCapsule = PowerupTriggerAnimController.instance.animationsPool[PowerupTriggerAnimController.instance.animationsPool.Count - 1];
			PowerupTriggerAnimController.instance.animationsPool.RemoveAt(PowerupTriggerAnimController.instance.animationsPool.Count - 1);
		}
		else
		{
			animationCapsule = new PowerupTriggerAnimController.AnimationCapsule();
		}
		animationCapsule.powerup = powerup;
		animationCapsule.runModifierCard = runModifierCard;
		animationCapsule.animationKind = animKind;
		PowerupTriggerAnimController.instance.inLineAnimations.Add(animationCapsule);
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x00040C0D File Offset: 0x0003EE0D
	public static bool HasAnimations()
	{
		return !(PowerupTriggerAnimController.instance == null) && (PowerupTriggerAnimController.instance.animationPlaying || PowerupTriggerAnimController.instance.inLineAnimations.Count > 0);
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x00040C3E File Offset: 0x0003EE3E
	public static PowerupScript GetAnimatedPowerup()
	{
		if (PowerupTriggerAnimController.instance == null)
		{
			return null;
		}
		if (PowerupTriggerAnimController.instance.currentlyAnimatedCapsule == null)
		{
			return null;
		}
		return PowerupTriggerAnimController.instance.currentlyAnimatedCapsule.powerup;
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00040C6C File Offset: 0x0003EE6C
	public static bool IsShowingUnlockAnimation()
	{
		return !(PowerupTriggerAnimController.instance == null) && PowerupTriggerAnimController.instance.currentlyAnimatedCapsule != null && PowerupTriggerAnimController.instance.currentlyAnimatedCapsule.animationKind == PowerupTriggerAnimController.AnimationCapsule.AnimationKind.unlock;
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x00040C9D File Offset: 0x0003EE9D
	public static bool IsShowingDiscardAnimation()
	{
		return !(PowerupTriggerAnimController.instance == null) && PowerupTriggerAnimController.instance.currentlyAnimatedCapsule != null && PowerupTriggerAnimController.instance.currentlyAnimatedCapsule.animationKind == PowerupTriggerAnimController.AnimationCapsule.AnimationKind.unlock;
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x00040CCE File Offset: 0x0003EECE
	public static void AnimationSetSpeed(float speed)
	{
		if (PowerupTriggerAnimController.instance == null)
		{
			return;
		}
		PowerupTriggerAnimController.instance.animSpeedMultOverride = speed;
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x00040CE9 File Offset: 0x0003EEE9
	private void _AnimationExitSound()
	{
		if (!PowerupTriggerAnimController.ShouldHide())
		{
			Sound.Play("SoundPowerupTriggerExit", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
		}
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00040D14 File Offset: 0x0003EF14
	private void _AnimationMidwaySound()
	{
		if (this.currentlyAnimatedCapsule == null)
		{
			return;
		}
		if (this.currentlyAnimatedCapsule.animationKind == PowerupTriggerAnimController.AnimationCapsule.AnimationKind.discard)
		{
			return;
		}
		if (this.currentlyAnimatedCapsule.animationKind == PowerupTriggerAnimController.AnimationCapsule.AnimationKind.recharge_RedButton)
		{
			return;
		}
		if (this.currentlyAnimatedCapsule.animationKind == PowerupTriggerAnimController.AnimationCapsule.AnimationKind.card)
		{
			return;
		}
		if (this.currentlyAnimatedCapsule.powerup == null)
		{
			return;
		}
		if (this.currentlyAnimatedCapsule.powerup.triggerSpecificSound != null && !PowerupTriggerAnimController.ShouldHide())
		{
			Sound.Play(this.currentlyAnimatedCapsule.powerup.triggerSpecificSound.name, 1f, 1f);
		}
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x00040DB0 File Offset: 0x0003EFB0
	private void _AnimationPlay()
	{
		if (this.animationPlaying)
		{
			return;
		}
		this.animationPlaying = true;
		this.triggerText.text = Translation.Get("POWERUP_KEYWORD_TRIGGER");
		this.unlockText.text = Translation.Get("POWERUP_ANIMATION_UNLOCK_NEW_LABLE");
		this.currentlyAnimatedCapsule = this.inLineAnimations[0];
		PowerupTriggerAnimController.instance.inLineAnimations.RemoveAt(0);
		this.myAnimator.SetTrigger("play");
		if (this.currentlyAnimatedCapsule.powerup != null)
		{
			this.MeshSet(this.currentlyAnimatedCapsule, this.currentlyAnimatedCapsule.animationKind);
		}
		else if (this.currentlyAnimatedCapsule.runModifierCard != RunModifierScript.Identifier.undefined && this.currentlyAnimatedCapsule.runModifierCard != RunModifierScript.Identifier.count)
		{
			this.CardSet(this.currentlyAnimatedCapsule, this.currentlyAnimatedCapsule.animationKind);
		}
		if (!PowerupTriggerAnimController.ShouldHide())
		{
			switch (this.currentlyAnimatedCapsule.animationKind)
			{
			case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.normalTrigger:
				Sound.Play("SoundPowerupTriggerEntrance", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
				break;
			case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.unlock:
				Sound.Play("SoundPowerupUnlock", 1f, 1f);
				break;
			case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.discard:
				Sound.Play("SoundPowerupThrowAway", 1f, 1f);
				break;
			case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.recharge_RedButton:
				Sound.Play("SoundPowerupRecharge", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
				break;
			case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.powerDown:
				Sound.Play("SoundPowerupPowerDown", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
				break;
			case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.card:
				Sound.Play("SoundCardAnimationTrigger", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
				break;
			}
		}
		if (this.currentlyAnimatedCapsule.powerup != null)
		{
			PowerupTriggerAnimController.PowerupEvent onAnimationStart = this.OnAnimationStart;
			if (onAnimationStart != null)
			{
				onAnimationStart(this.currentlyAnimatedCapsule);
			}
			PowerupTriggerAnimController.PowerupEvent onAnimationStart_Unresettable = this.OnAnimationStart_Unresettable;
			if (onAnimationStart_Unresettable != null)
			{
				onAnimationStart_Unresettable(this.currentlyAnimatedCapsule);
			}
		}
		if (this.currentlyAnimatedCapsule.powerup != null)
		{
			PowerupScript.ClassicPlayingCards_AceOfSpades_ProcessActivation(true, this.currentlyAnimatedCapsule.powerup.identifier);
		}
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x00040FE0 File Offset: 0x0003F1E0
	private void _AnimationEnd()
	{
		if (!this.animationPlaying)
		{
			return;
		}
		if (this.currentlyAnimatedCapsule.powerup != null)
		{
			this.MeshRemove();
		}
		else if (this.currentlyAnimatedCapsule.runModifierCard != RunModifierScript.Identifier.undefined && this.currentlyAnimatedCapsule.runModifierCard != RunModifierScript.Identifier.count)
		{
			this.CardRemove();
		}
		if (this.currentlyAnimatedCapsule.powerup != null)
		{
			PowerupTriggerAnimController.PowerupEvent onAnimationEnd = this.OnAnimationEnd;
			if (onAnimationEnd != null)
			{
				onAnimationEnd(this.currentlyAnimatedCapsule);
			}
			PowerupTriggerAnimController.PowerupEvent onAnimationEnd_Unresettable = this.OnAnimationEnd_Unresettable;
			if (onAnimationEnd_Unresettable != null)
			{
				onAnimationEnd_Unresettable(this.currentlyAnimatedCapsule);
			}
		}
		if (this.currentlyAnimatedCapsule.powerup != null)
		{
			PowerupTriggerAnimController.PowerupAgnosticEvent onAllAnimationsEnd = this.OnAllAnimationsEnd;
			if (onAllAnimationsEnd != null)
			{
				onAllAnimationsEnd(this.currentlyAnimatedCapsule);
			}
			PowerupTriggerAnimController.PowerupAgnosticEvent onAllAnimationsEnd_Unresettable = this.OnAllAnimationsEnd_Unresettable;
			if (onAllAnimationsEnd_Unresettable != null)
			{
				onAllAnimationsEnd_Unresettable(this.currentlyAnimatedCapsule);
			}
		}
		PowerupTriggerAnimController.animSpeedMultMax = Mathf.Min(PowerupTriggerAnimController.animSpeedMultMax + 0.05f, 2f);
		this.animSpeedMultOverride = -1f;
		PowerupTriggerAnimController.instance.animationsPool.Add(this.currentlyAnimatedCapsule);
		this.animationPlaying = false;
		this.currentlyAnimatedCapsule = null;
		GoldenToiletStickerScript.RefreshVisualsStatic();
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x00041104 File Offset: 0x0003F304
	private void MeshSet(PowerupTriggerAnimController.AnimationCapsule animation, PowerupTriggerAnimController.AnimationCapsule.AnimationKind animationKind)
	{
		if (animation == null)
		{
			Debug.LogError("PowerupTriggerAnimationController.MeshSet(): powerup currently animated is null");
		}
		if (animationKind != PowerupTriggerAnimController.AnimationCapsule.AnimationKind.discard)
		{
			if (animationKind == PowerupTriggerAnimController.AnimationCapsule.AnimationKind.recharge_RedButton)
			{
				this.animationParticles_RechargeRedButton.gameObject.SetActive(true);
				this.animationParticles_RechargeRedButton.transform.localScale = Vector3.zero;
			}
		}
		else
		{
			this.animationParticles_DiscardBlood.gameObject.SetActive(true);
			this.animationParticles_DiscardBlood.transform.localScale = Vector3.zero;
		}
		float num = 0.5f * this.ExtraScaleMultGet(animation.powerup.identifier);
		MeshRenderer meshRenderer = animation.powerup.MeshRendererGet();
		SkinnedMeshRenderer skinnedMeshRenderer = animation.powerup.SkinnedMeshRendererGet();
		Animator animator = null;
		PowerupTriggerAnimController.MeshCopyCapsule meshCopyCapsule;
		Animator animator2;
		Material material;
		if (meshRenderer != null)
		{
			if (this.powerupCopies_MeshRenderer.ContainsKey(animation.powerup.identifier))
			{
				meshCopyCapsule = this.powerupCopies_MeshRenderer[animation.powerup.identifier];
			}
			else
			{
				meshCopyCapsule = new PowerupTriggerAnimController.MeshCopyCapsule(global::UnityEngine.Object.Instantiate<GameObject>(meshRenderer.gameObject).GetComponent<MeshRenderer>(), null, animation.powerup.MaterialDefault_Get());
				this.powerupCopies_MeshRenderer.Add(animation.powerup.identifier, meshCopyCapsule);
			}
			Vector3 boundingBoxSizeNormalized = animation.powerup.GetBoundingBoxSizeNormalized();
			meshCopyCapsule.meshRenderer.enabled = true;
			meshCopyCapsule.meshRenderer.transform.SetParent(this.myPowerupMeshHolder);
			meshCopyCapsule.meshRenderer.transform.localPosition = meshRenderer.transform.localPosition;
			meshCopyCapsule.meshRenderer.transform.localEulerAngles = meshRenderer.transform.localEulerAngles;
			meshCopyCapsule.meshRenderer.transform.localScale = boundingBoxSizeNormalized * num;
			animator2 = meshCopyCapsule.meshRenderer.GetComponentInChildren<Animator>(true);
			if (animator2 != null)
			{
				animator = meshRenderer.GetComponentInChildren<Animator>(true);
			}
			material = meshRenderer.sharedMaterial;
			meshCopyCapsule.meshRenderer.gameObject.SetActive(true);
		}
		else
		{
			if (!(skinnedMeshRenderer != null))
			{
				Debug.LogError("PowerupTriggerAnimationController.MeshSet(): cannot retrieve a mesh for powerup: " + animation.ToString());
				return;
			}
			if (this.powerupCopies_SkinnedMeshRenderer.ContainsKey(animation.powerup.identifier))
			{
				meshCopyCapsule = this.powerupCopies_SkinnedMeshRenderer[animation.powerup.identifier];
			}
			else
			{
				meshCopyCapsule = new PowerupTriggerAnimController.MeshCopyCapsule(null, global::UnityEngine.Object.Instantiate<GameObject>(skinnedMeshRenderer.transform.parent.gameObject).GetComponentInChildren<SkinnedMeshRenderer>(), animation.powerup.MaterialDefault_Get());
				this.powerupCopies_SkinnedMeshRenderer.Add(animation.powerup.identifier, meshCopyCapsule);
			}
			Vector3 boundingBoxSizeNormalized2 = animation.powerup.GetBoundingBoxSizeNormalized();
			meshCopyCapsule.skinnedMeshRenderer.enabled = true;
			meshCopyCapsule.skinnedMeshRenderer.transform.parent.SetParent(this.myPowerupMeshHolder);
			meshCopyCapsule.skinnedMeshRenderer.transform.parent.localPosition = skinnedMeshRenderer.transform.parent.localPosition;
			meshCopyCapsule.skinnedMeshRenderer.transform.parent.localEulerAngles = skinnedMeshRenderer.transform.parent.localEulerAngles;
			meshCopyCapsule.skinnedMeshRenderer.transform.parent.localScale = boundingBoxSizeNormalized2 * num;
			animator2 = meshCopyCapsule.skinnedMeshRenderer.transform.parent.GetComponentInChildren<Animator>(true);
			if (animator2 != null)
			{
				animator = skinnedMeshRenderer.transform.parent.GetComponentInChildren<Animator>(true);
			}
			material = skinnedMeshRenderer.sharedMaterial;
			meshCopyCapsule.skinnedMeshRenderer.transform.parent.gameObject.SetActive(true);
		}
		animation.meshCopyCapsule = meshCopyCapsule;
		if (animator2 != null && animator != null)
		{
			string name = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
			float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
			animator2.Play(name, 0, normalizedTime);
			animator2.Update(0f);
		}
		if (material == null)
		{
			Debug.LogError("PowerupTriggerAnimationController.MeshSet(): defaultMaterialReference is null for animation: " + animation.ToString());
			return;
		}
		meshCopyCapsule.materialDiscard.SetFloat("_DistructionValue01", 0f);
		if (!(meshCopyCapsule.meshRenderer != null))
		{
			if (meshCopyCapsule.skinnedMeshRenderer != null)
			{
				switch (animationKind)
				{
				case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.discard:
					meshCopyCapsule.skinnedMeshRenderer.sharedMaterial = meshCopyCapsule.materialDiscard;
					return;
				case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.recharge_RedButton:
					meshCopyCapsule.skinnedMeshRenderer.sharedMaterial = meshCopyCapsule.materialRecharge;
					return;
				case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.powerDown:
					meshCopyCapsule.skinnedMeshRenderer.sharedMaterial = meshCopyCapsule.materialPowerDown;
					return;
				default:
					meshCopyCapsule.skinnedMeshRenderer.sharedMaterial = meshCopyCapsule.materialDefault;
					break;
				}
			}
			return;
		}
		switch (animationKind)
		{
		case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.discard:
			meshCopyCapsule.meshRenderer.sharedMaterial = meshCopyCapsule.materialDiscard;
			return;
		case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.recharge_RedButton:
			meshCopyCapsule.meshRenderer.sharedMaterial = meshCopyCapsule.materialRecharge;
			return;
		case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.powerDown:
			meshCopyCapsule.meshRenderer.sharedMaterial = meshCopyCapsule.materialPowerDown;
			return;
		default:
			meshCopyCapsule.meshRenderer.sharedMaterial = meshCopyCapsule.materialDefault;
			return;
		}
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x000415D8 File Offset: 0x0003F7D8
	private void MeshRemove()
	{
		if (this.currentlyAnimatedCapsule == null)
		{
			Debug.LogError("PowerupTriggerAnimationController.MeshRemove(): powerup currently animated is null");
		}
		this.AnimationParticlesReset();
		global::UnityEngine.Object @object = this.currentlyAnimatedCapsule.powerup.MeshRendererGet();
		SkinnedMeshRenderer skinnedMeshRenderer = this.currentlyAnimatedCapsule.powerup.SkinnedMeshRendererGet();
		if (@object != null)
		{
			MeshRenderer meshRenderer = this.powerupCopies_MeshRenderer[this.currentlyAnimatedCapsule.powerup.identifier].meshRenderer;
			meshRenderer.transform.SetParent(this.meshPoolHolder);
			meshRenderer.gameObject.SetActive(false);
			return;
		}
		if (skinnedMeshRenderer != null)
		{
			SkinnedMeshRenderer skinnedMeshRenderer2 = this.powerupCopies_SkinnedMeshRenderer[this.currentlyAnimatedCapsule.powerup.identifier].skinnedMeshRenderer;
			skinnedMeshRenderer2.transform.parent.SetParent(this.meshPoolHolder);
			skinnedMeshRenderer2.transform.parent.gameObject.SetActive(false);
			return;
		}
		Debug.LogError("PowerupTriggerAnimationController.MeshRemove(): cannot remove a mesh for powerup: " + this.currentlyAnimatedCapsule.ToString());
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x000416D4 File Offset: 0x0003F8D4
	private void CardSet(PowerupTriggerAnimController.AnimationCapsule animation, PowerupTriggerAnimController.AnimationCapsule.AnimationKind animationKind)
	{
		if (animation == null)
		{
			Debug.LogError("PowerupTriggerAnimationController.CardSet(): powerup currently animated is null");
			return;
		}
		if (animation.runModifierCard == RunModifierScript.Identifier.undefined || animation.runModifierCard == RunModifierScript.Identifier.count)
		{
			return;
		}
		this._cards.Add(CardScript.PoolSpawn(animation.runModifierCard, 1.25f, this.cardHolder));
		this._cards[this._cards.Count - 1].transform.localPosition = Vector3.zero;
		this._cards[this._cards.Count - 1].transform.localEulerAngles = new Vector3(0f, -90f, 0f);
		this._cards[this._cards.Count - 1].SetRenderingLayer(0);
		this._cards[this._cards.Count - 1].TextForceHidden(true);
		this._cards[this._cards.Count - 1].ForceUnflipped();
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x000417DC File Offset: 0x0003F9DC
	private void CardRemove()
	{
		if (this.currentlyAnimatedCapsule == null)
		{
			Debug.LogError("PowerupTriggerAnimationController.CardRemove(): powerup currently animated is null");
			return;
		}
		if (this.currentlyAnimatedCapsule.runModifierCard == RunModifierScript.Identifier.undefined || this.currentlyAnimatedCapsule.runModifierCard == RunModifierScript.Identifier.count)
		{
			return;
		}
		for (int i = this._cards.Count - 1; i >= 0; i--)
		{
			if (this._cards[i].identifier == this.currentlyAnimatedCapsule.runModifierCard)
			{
				this._cards[i].PoolDestroy();
				this._cards.RemoveAt(i);
			}
		}
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x00041870 File Offset: 0x0003FA70
	private void MeshUpdateAnimation(float animationTime)
	{
		if (this.currentlyAnimatedCapsule == null)
		{
			return;
		}
		if (this.currentlyAnimatedCapsule.powerup == null)
		{
			return;
		}
		switch (this.currentlyAnimatedCapsule.animationKind)
		{
		case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.discard:
		{
			float num = Mathf.Min(1f, animationTime * 2.5f);
			if (this.currentlyAnimatedCapsule.meshCopyCapsule.meshRenderer != null)
			{
				this.currentlyAnimatedCapsule.meshCopyCapsule.meshRenderer.sharedMaterial.SetFloat("_DistructionValue01", num);
			}
			else if (this.currentlyAnimatedCapsule.meshCopyCapsule.skinnedMeshRenderer != null)
			{
				this.currentlyAnimatedCapsule.meshCopyCapsule.skinnedMeshRenderer.sharedMaterial.SetFloat("_DistructionValue01", num);
			}
			this.animationParticles_DiscardBlood.transform.localScale = Vector3.one * Mathf.Clamp01(num);
			return;
		}
		case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.recharge_RedButton:
			if (this.currentlyAnimatedCapsule.meshCopyCapsule.meshRenderer != null)
			{
				this.currentlyAnimatedCapsule.meshCopyCapsule.meshRenderer.sharedMaterial.SetColor("_Emission", Color.yellow * (0.5f + 0.3f * Util.AngleSin(animationTime * 1440f)));
			}
			else if (this.currentlyAnimatedCapsule.meshCopyCapsule.skinnedMeshRenderer != null)
			{
				this.currentlyAnimatedCapsule.meshCopyCapsule.skinnedMeshRenderer.sharedMaterial.SetColor("_Emission", Color.yellow * (0.75f + 0.15f * Util.AngleSin(animationTime * 1440f)));
			}
			this.animationParticles_RechargeRedButton.transform.localScale = Vector3.one * Mathf.Clamp01(animationTime * 2.5f);
			return;
		case PowerupTriggerAnimController.AnimationCapsule.AnimationKind.powerDown:
		{
			Color color = ((Util.AngleSin(animationTime * 1440f) > -0.5f) ? PowerupTriggerAnimController.COLOR_GRAY_MODEL : Color.red);
			if (this.currentlyAnimatedCapsule.meshCopyCapsule.meshRenderer != null)
			{
				if (this.currentlyAnimatedCapsule.meshCopyCapsule.meshRenderer.sharedMaterial.GetColor("_Color") != color)
				{
					this.currentlyAnimatedCapsule.meshCopyCapsule.meshRenderer.sharedMaterial.SetColor("_Color", color);
					return;
				}
			}
			else if (this.currentlyAnimatedCapsule.meshCopyCapsule.skinnedMeshRenderer != null && this.currentlyAnimatedCapsule.meshCopyCapsule.skinnedMeshRenderer.sharedMaterial.GetColor("_Color") != color)
			{
				this.currentlyAnimatedCapsule.meshCopyCapsule.skinnedMeshRenderer.sharedMaterial.SetColor("_Color", color);
			}
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x00041B1D File Offset: 0x0003FD1D
	private float ExtraScaleMultGet(PowerupScript.Identifier identifier)
	{
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

	// Token: 0x060009D2 RID: 2514 RVA: 0x00041B50 File Offset: 0x0003FD50
	private void AnimationParticlesReset()
	{
		this.animationParticles_DiscardBlood.gameObject.SetActive(false);
		this.animationParticles_RechargeRedButton.gameObject.SetActive(false);
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x00041B74 File Offset: 0x0003FD74
	private void Awake()
	{
		PowerupTriggerAnimController.instance = this;
		this.myCamera = base.GetComponent<Camera>();
		this.myAnimator = base.GetComponent<Animator>();
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x00041B94 File Offset: 0x0003FD94
	private void Start()
	{
		this.AnimationParticlesReset();
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x00041B9C File Offset: 0x0003FD9C
	private void OnDestroy()
	{
		if (PowerupTriggerAnimController.instance == this)
		{
			PowerupTriggerAnimController.instance = null;
		}
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x00041BB4 File Offset: 0x0003FDB4
	private void Update()
	{
		if (DrawersScript.IsAnyDrawerOpened())
		{
			return;
		}
		if (PhoneUiScript.IsEnabled())
		{
			return;
		}
		bool flag = PowerupTriggerAnimController.ShouldHide();
		if (this.myCamera.enabled == flag)
		{
			this.myCamera.enabled = !flag;
		}
		AnimatorStateInfo currentAnimatorStateInfo = this.myAnimator.GetCurrentAnimatorStateInfo(0);
		bool flag2 = currentAnimatorStateInfo.IsName("Empty");
		bool flag3 = this.currentlyAnimatedCapsule != null && this.currentlyAnimatedCapsule.animationKind == PowerupTriggerAnimController.AnimationCapsule.AnimationKind.unlock;
		bool flag4 = this.currentlyAnimatedCapsule != null && this.currentlyAnimatedCapsule.animationKind == PowerupTriggerAnimController.AnimationCapsule.AnimationKind.discard;
		if (this.animationPlaying)
		{
			if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) && currentAnimatorStateInfo.normalizedTime > 0.15f)
			{
				PowerupTriggerAnimController.AnimationSetSpeed(3f);
			}
			float num = PowerupTriggerAnimController.animSpeedMultMax;
			if (flag3)
			{
				num = 0.8f;
			}
			if (flag4)
			{
				num = 0.6f;
			}
			if (this.animSpeedMultOverride > 0f)
			{
				num = this.animSpeedMultOverride;
			}
			num *= Data.SettingsData.TransitionSpeedMapped_Get((float)Data.settings.transitionSpeed, 1f, 4f, 1f, 3f);
			this.animSpeedMult = Mathf.Lerp(this.animSpeedMult, num, Tick.Time * 30f);
			float num2 = this.animSpeedMult;
			if (PowerupTriggerAnimController.ShouldHide())
			{
				num2 = 10f;
			}
			if (this.myAnimator.speed != num2)
			{
				this.myAnimator.speed = num2;
			}
			Sound.SoundCapsule soundCapsule = Sound.Find("SoundPowerupTriggerExit");
			if (soundCapsule != null)
			{
				soundCapsule.myAudioSource.pitch = num2;
			}
			this.backImageColor.a = Mathf.Lerp(this.backImageColor.a, 0.9f, Tick.Time * 10f);
			this.myBackImage.color = this.backImageColor;
			this.MeshUpdateAnimation(currentAnimatorStateInfo.normalizedTime);
		}
		else
		{
			bool flag5 = flag2 && !DialogueScript.IsEnabled();
			if (this.inLineAnimations.Count > 0 && flag5)
			{
				this._AnimationPlay();
			}
			this.backImageColor.a = Mathf.Lerp(this.backImageColor.a, 0f, Tick.Time * 10f);
			this.myBackImage.color = this.backImageColor;
		}
		bool flag6 = flag3;
		if (this.unlockRotator.gameObject.activeSelf != flag6)
		{
			this.unlockRotator.gameObject.SetActive(flag6);
		}
		if (flag6)
		{
			Vector3 vector = Util.AxisToAngle3D(this.unlockRotator.position - this.myCamera.transform.position);
			vector.y += 90f;
			this.unlockRotator.localEulerAngles = vector;
			bool flag7 = Util.AngleSin(Tick.PassedTime * 1440f) > 0f;
			this.unlockText.fontSharedMaterial = (flag7 ? this.tmpMat_Orange : this.tmpMat_Yellow);
			this.unlockBackSprite.transform.AddLocalZAngle(Tick.Time * 180f);
		}
		if (!PowerupTriggerAnimController.HasAnimations())
		{
			if (this.OnAnimationStart != null)
			{
				this.OnAnimationStart = null;
			}
			if (this.OnAnimationEnd != null)
			{
				this.OnAnimationEnd = null;
			}
			if (this.OnAllAnimationsEnd != null)
			{
				this.OnAllAnimationsEnd = null;
			}
		}
	}

	public static PowerupTriggerAnimController instance;

	private const int PLAYER_INDEX = 0;

	private const float ANIM_LERP_SPEED = 30f;

	private const float ANIM_SPEED_UNLOCK = 0.8f;

	private const float ANIM_SPEED_DISCARD = 0.6f;

	private const float ANIM_SPEED_DEFAULT = 1.25f;

	private const float ANIM_SPEED_INCREMENT = 0.05f;

	private const float ANIM_SPEED_MAX = 2f;

	private const float ANIM_SPEED_SKIP = 3f;

	private const float ANIM_SPEED_HIDDEN = 10f;

	private const float MAX_BACK_ALPHA = 0.9f;

	private const float Y_OFFSET_ON_HOLD = -0.15f;

	private static Color COLOR_GRAY_MODEL = new Color(0.75f, 0.75f, 0.75f, 1f);

	public Camera myCamera;

	private Animator myAnimator;

	public Transform meshPoolHolder;

	public Transform myPowerupMeshHolder;

	public Transform cameraDistantiator;

	public RawImage myBackImage;

	public TextMeshProUGUI triggerText;

	public Transform unlockRotator;

	public TextMeshPro unlockText;

	public SpriteRenderer unlockBackSprite;

	public Material tmpMat_Orange;

	public Material tmpMat_Yellow;

	public Material materialDiscardDefault;

	public ParticleSystem animationParticles_RechargeRedButton;

	public ParticleSystem animationParticles_DiscardBlood;

	public Transform cardHolder;

	private List<PowerupTriggerAnimController.AnimationCapsule> inLineAnimations = new List<PowerupTriggerAnimController.AnimationCapsule>();

	private List<PowerupTriggerAnimController.AnimationCapsule> animationsPool = new List<PowerupTriggerAnimController.AnimationCapsule>();

	private PowerupTriggerAnimController.AnimationCapsule currentlyAnimatedCapsule;

	private float animSpeedMult = 1.25f;

	private static float animSpeedMultMax = 1.25f;

	private float animSpeedMultOverride = -1f;

	private bool animationPlaying;

	private Color backImageColor = new Color(0f, 0f, 0f, 0f);

	private Dictionary<PowerupScript.Identifier, PowerupTriggerAnimController.MeshCopyCapsule> powerupCopies_MeshRenderer = new Dictionary<PowerupScript.Identifier, PowerupTriggerAnimController.MeshCopyCapsule>();

	private Dictionary<PowerupScript.Identifier, PowerupTriggerAnimController.MeshCopyCapsule> powerupCopies_SkinnedMeshRenderer = new Dictionary<PowerupScript.Identifier, PowerupTriggerAnimController.MeshCopyCapsule>();

	private List<CardScript> _cards = new List<CardScript>();

	public PowerupTriggerAnimController.PowerupEvent OnAnimationStart;

	public PowerupTriggerAnimController.PowerupEvent OnAnimationEnd;

	public PowerupTriggerAnimController.PowerupEvent OnAnimationStart_Unresettable;

	public PowerupTriggerAnimController.PowerupEvent OnAnimationEnd_Unresettable;

	public PowerupTriggerAnimController.PowerupAgnosticEvent OnAllAnimationsEnd;

	public PowerupTriggerAnimController.PowerupAgnosticEvent OnAllAnimationsEnd_Unresettable;

	public class AnimationCapsule
	{
		public PowerupScript powerup;

		public RunModifierScript.Identifier runModifierCard;

		public PowerupTriggerAnimController.AnimationCapsule.AnimationKind animationKind;

		public PowerupTriggerAnimController.MeshCopyCapsule meshCopyCapsule;

		public enum AnimationKind
		{
			normalTrigger,
			unlock,
			discard,
			recharge_RedButton,
			powerDown,
			card
		}
	}

	public class MeshCopyCapsule
	{
		// Token: 0x06001240 RID: 4672 RVA: 0x00075258 File Offset: 0x00073458
		public MeshCopyCapsule(MeshRenderer meshRenderer, SkinnedMeshRenderer skinnedMeshRenderer, Material materialDefault)
		{
			this.meshRenderer = meshRenderer;
			this.skinnedMeshRenderer = skinnedMeshRenderer;
			this.materialDefault = materialDefault;
			this.materialDiscard = new Material(PowerupTriggerAnimController.instance.materialDiscardDefault);
			this.materialDiscard.SetTexture("_MainTex", materialDefault.GetTexture("_MainTex"));
			this.materialRecharge = new Material(materialDefault);
			this.materialRecharge.SetTexture("_MainTex", materialDefault.GetTexture("_MainTex"));
			this.materialRecharge.SetColor("_Emission", Color.yellow * 0.8f);
			this.materialPowerDown = new Material(materialDefault);
			this.materialPowerDown.SetTexture("_MainTex", materialDefault.GetTexture("_MainTex"));
			this.materialPowerDown.SetColor("_Color", new Color(0.75f, 0.75f, 0.75f, 1f));
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00075348 File Offset: 0x00073548
		~MeshCopyCapsule()
		{
			if (this.materialDiscard != null)
			{
				global::UnityEngine.Object.Destroy(this.materialDiscard);
			}
			if (this.materialRecharge != null)
			{
				global::UnityEngine.Object.Destroy(this.materialRecharge);
			}
			if (this.materialPowerDown != null)
			{
				global::UnityEngine.Object.Destroy(this.materialPowerDown);
			}
		}

		public MeshRenderer meshRenderer;

		public SkinnedMeshRenderer skinnedMeshRenderer;

		public Material materialDefault;

		public Material materialDiscard;

		public Material materialRecharge;

		public Material materialPowerDown;
	}

	// (Invoke) Token: 0x06001243 RID: 4675
	public delegate void PowerupAgnosticEvent(PowerupTriggerAnimController.AnimationCapsule animationCapsule);

	// (Invoke) Token: 0x06001247 RID: 4679
	public delegate void PowerupEvent(PowerupTriggerAnimController.AnimationCapsule animationCapsule);
}
