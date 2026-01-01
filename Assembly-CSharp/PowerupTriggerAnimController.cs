using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000DA RID: 218
public class PowerupTriggerAnimController : MonoBehaviour
{
	// Token: 0x06000B39 RID: 2873 RVA: 0x0000EED7 File Offset: 0x0000D0D7
	public static bool ShouldHide()
	{
		return PowerupTriggerAnimController.instance == null || GameplayMaster.instance == null || GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.intro;
	}

	// Token: 0x06000B3A RID: 2874 RVA: 0x0005A6CC File Offset: 0x000588CC
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

	// Token: 0x06000B3B RID: 2875 RVA: 0x0000EF01 File Offset: 0x0000D101
	public static bool HasAnimations()
	{
		return !(PowerupTriggerAnimController.instance == null) && (PowerupTriggerAnimController.instance.animationPlaying || PowerupTriggerAnimController.instance.inLineAnimations.Count > 0);
	}

	// Token: 0x06000B3C RID: 2876 RVA: 0x0000EF32 File Offset: 0x0000D132
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

	// Token: 0x06000B3D RID: 2877 RVA: 0x0000EF60 File Offset: 0x0000D160
	public static bool IsShowingUnlockAnimation()
	{
		return !(PowerupTriggerAnimController.instance == null) && PowerupTriggerAnimController.instance.currentlyAnimatedCapsule != null && PowerupTriggerAnimController.instance.currentlyAnimatedCapsule.animationKind == PowerupTriggerAnimController.AnimationCapsule.AnimationKind.unlock;
	}

	// Token: 0x06000B3E RID: 2878 RVA: 0x0000EF60 File Offset: 0x0000D160
	public static bool IsShowingDiscardAnimation()
	{
		return !(PowerupTriggerAnimController.instance == null) && PowerupTriggerAnimController.instance.currentlyAnimatedCapsule != null && PowerupTriggerAnimController.instance.currentlyAnimatedCapsule.animationKind == PowerupTriggerAnimController.AnimationCapsule.AnimationKind.unlock;
	}

	// Token: 0x06000B3F RID: 2879 RVA: 0x0000EF91 File Offset: 0x0000D191
	public static void AnimationSetSpeed(float speed)
	{
		if (PowerupTriggerAnimController.instance == null)
		{
			return;
		}
		PowerupTriggerAnimController.instance.animSpeedMultOverride = speed;
	}

	// Token: 0x06000B40 RID: 2880 RVA: 0x0000EFAC File Offset: 0x0000D1AC
	private void _AnimationExitSound()
	{
		if (!PowerupTriggerAnimController.ShouldHide())
		{
			Sound.Play("SoundPowerupTriggerExit", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
		}
	}

	// Token: 0x06000B41 RID: 2881 RVA: 0x0005A75C File Offset: 0x0005895C
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

	// Token: 0x06000B42 RID: 2882 RVA: 0x0005A7F8 File Offset: 0x000589F8
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

	// Token: 0x06000B43 RID: 2883 RVA: 0x0005AA28 File Offset: 0x00058C28
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

	// Token: 0x06000B44 RID: 2884 RVA: 0x0005AB4C File Offset: 0x00058D4C
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

	// Token: 0x06000B45 RID: 2885 RVA: 0x0005B020 File Offset: 0x00059220
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

	// Token: 0x06000B46 RID: 2886 RVA: 0x0005B11C File Offset: 0x0005931C
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

	// Token: 0x06000B47 RID: 2887 RVA: 0x0005B224 File Offset: 0x00059424
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

	// Token: 0x06000B48 RID: 2888 RVA: 0x0005B2B8 File Offset: 0x000594B8
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

	// Token: 0x06000B49 RID: 2889 RVA: 0x0000EFD4 File Offset: 0x0000D1D4
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

	// Token: 0x06000B4A RID: 2890 RVA: 0x0000F007 File Offset: 0x0000D207
	private void AnimationParticlesReset()
	{
		this.animationParticles_DiscardBlood.gameObject.SetActive(false);
		this.animationParticles_RechargeRedButton.gameObject.SetActive(false);
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x0000F02B File Offset: 0x0000D22B
	private void Awake()
	{
		PowerupTriggerAnimController.instance = this;
		this.myCamera = base.GetComponent<Camera>();
		this.myAnimator = base.GetComponent<Animator>();
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x0000F04B File Offset: 0x0000D24B
	private void Start()
	{
		this.AnimationParticlesReset();
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x0000F053 File Offset: 0x0000D253
	private void OnDestroy()
	{
		if (PowerupTriggerAnimController.instance == this)
		{
			PowerupTriggerAnimController.instance = null;
		}
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x0005B568 File Offset: 0x00059768
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

	// Token: 0x04000B9D RID: 2973
	public static PowerupTriggerAnimController instance;

	// Token: 0x04000B9E RID: 2974
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000B9F RID: 2975
	private const float ANIM_LERP_SPEED = 30f;

	// Token: 0x04000BA0 RID: 2976
	private const float ANIM_SPEED_UNLOCK = 0.8f;

	// Token: 0x04000BA1 RID: 2977
	private const float ANIM_SPEED_DISCARD = 0.6f;

	// Token: 0x04000BA2 RID: 2978
	private const float ANIM_SPEED_DEFAULT = 1.25f;

	// Token: 0x04000BA3 RID: 2979
	private const float ANIM_SPEED_INCREMENT = 0.05f;

	// Token: 0x04000BA4 RID: 2980
	private const float ANIM_SPEED_MAX = 2f;

	// Token: 0x04000BA5 RID: 2981
	private const float ANIM_SPEED_SKIP = 3f;

	// Token: 0x04000BA6 RID: 2982
	private const float ANIM_SPEED_HIDDEN = 10f;

	// Token: 0x04000BA7 RID: 2983
	private const float MAX_BACK_ALPHA = 0.9f;

	// Token: 0x04000BA8 RID: 2984
	private const float Y_OFFSET_ON_HOLD = -0.15f;

	// Token: 0x04000BA9 RID: 2985
	private static Color COLOR_GRAY_MODEL = new Color(0.75f, 0.75f, 0.75f, 1f);

	// Token: 0x04000BAA RID: 2986
	public Camera myCamera;

	// Token: 0x04000BAB RID: 2987
	private Animator myAnimator;

	// Token: 0x04000BAC RID: 2988
	public Transform meshPoolHolder;

	// Token: 0x04000BAD RID: 2989
	public Transform myPowerupMeshHolder;

	// Token: 0x04000BAE RID: 2990
	public Transform cameraDistantiator;

	// Token: 0x04000BAF RID: 2991
	public RawImage myBackImage;

	// Token: 0x04000BB0 RID: 2992
	public TextMeshProUGUI triggerText;

	// Token: 0x04000BB1 RID: 2993
	public Transform unlockRotator;

	// Token: 0x04000BB2 RID: 2994
	public TextMeshPro unlockText;

	// Token: 0x04000BB3 RID: 2995
	public SpriteRenderer unlockBackSprite;

	// Token: 0x04000BB4 RID: 2996
	public Material tmpMat_Orange;

	// Token: 0x04000BB5 RID: 2997
	public Material tmpMat_Yellow;

	// Token: 0x04000BB6 RID: 2998
	public Material materialDiscardDefault;

	// Token: 0x04000BB7 RID: 2999
	public ParticleSystem animationParticles_RechargeRedButton;

	// Token: 0x04000BB8 RID: 3000
	public ParticleSystem animationParticles_DiscardBlood;

	// Token: 0x04000BB9 RID: 3001
	public Transform cardHolder;

	// Token: 0x04000BBA RID: 3002
	private List<PowerupTriggerAnimController.AnimationCapsule> inLineAnimations = new List<PowerupTriggerAnimController.AnimationCapsule>();

	// Token: 0x04000BBB RID: 3003
	private List<PowerupTriggerAnimController.AnimationCapsule> animationsPool = new List<PowerupTriggerAnimController.AnimationCapsule>();

	// Token: 0x04000BBC RID: 3004
	private PowerupTriggerAnimController.AnimationCapsule currentlyAnimatedCapsule;

	// Token: 0x04000BBD RID: 3005
	private float animSpeedMult = 1.25f;

	// Token: 0x04000BBE RID: 3006
	private static float animSpeedMultMax = 1.25f;

	// Token: 0x04000BBF RID: 3007
	private float animSpeedMultOverride = -1f;

	// Token: 0x04000BC0 RID: 3008
	private bool animationPlaying;

	// Token: 0x04000BC1 RID: 3009
	private Color backImageColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x04000BC2 RID: 3010
	private Dictionary<PowerupScript.Identifier, PowerupTriggerAnimController.MeshCopyCapsule> powerupCopies_MeshRenderer = new Dictionary<PowerupScript.Identifier, PowerupTriggerAnimController.MeshCopyCapsule>();

	// Token: 0x04000BC3 RID: 3011
	private Dictionary<PowerupScript.Identifier, PowerupTriggerAnimController.MeshCopyCapsule> powerupCopies_SkinnedMeshRenderer = new Dictionary<PowerupScript.Identifier, PowerupTriggerAnimController.MeshCopyCapsule>();

	// Token: 0x04000BC4 RID: 3012
	private List<CardScript> _cards = new List<CardScript>();

	// Token: 0x04000BC5 RID: 3013
	public PowerupTriggerAnimController.PowerupEvent OnAnimationStart;

	// Token: 0x04000BC6 RID: 3014
	public PowerupTriggerAnimController.PowerupEvent OnAnimationEnd;

	// Token: 0x04000BC7 RID: 3015
	public PowerupTriggerAnimController.PowerupEvent OnAnimationStart_Unresettable;

	// Token: 0x04000BC8 RID: 3016
	public PowerupTriggerAnimController.PowerupEvent OnAnimationEnd_Unresettable;

	// Token: 0x04000BC9 RID: 3017
	public PowerupTriggerAnimController.PowerupAgnosticEvent OnAllAnimationsEnd;

	// Token: 0x04000BCA RID: 3018
	public PowerupTriggerAnimController.PowerupAgnosticEvent OnAllAnimationsEnd_Unresettable;

	// Token: 0x020000DB RID: 219
	public class AnimationCapsule
	{
		// Token: 0x04000BCB RID: 3019
		public PowerupScript powerup;

		// Token: 0x04000BCC RID: 3020
		public RunModifierScript.Identifier runModifierCard;

		// Token: 0x04000BCD RID: 3021
		public PowerupTriggerAnimController.AnimationCapsule.AnimationKind animationKind;

		// Token: 0x04000BCE RID: 3022
		public PowerupTriggerAnimController.MeshCopyCapsule meshCopyCapsule;

		// Token: 0x020000DC RID: 220
		public enum AnimationKind
		{
			// Token: 0x04000BD0 RID: 3024
			normalTrigger,
			// Token: 0x04000BD1 RID: 3025
			unlock,
			// Token: 0x04000BD2 RID: 3026
			discard,
			// Token: 0x04000BD3 RID: 3027
			recharge_RedButton,
			// Token: 0x04000BD4 RID: 3028
			powerDown,
			// Token: 0x04000BD5 RID: 3029
			card
		}
	}

	// Token: 0x020000DD RID: 221
	public class MeshCopyCapsule
	{
		// Token: 0x06000B52 RID: 2898 RVA: 0x0005B918 File Offset: 0x00059B18
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

		// Token: 0x06000B53 RID: 2899 RVA: 0x0005BA08 File Offset: 0x00059C08
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

		// Token: 0x04000BD6 RID: 3030
		public MeshRenderer meshRenderer;

		// Token: 0x04000BD7 RID: 3031
		public SkinnedMeshRenderer skinnedMeshRenderer;

		// Token: 0x04000BD8 RID: 3032
		public Material materialDefault;

		// Token: 0x04000BD9 RID: 3033
		public Material materialDiscard;

		// Token: 0x04000BDA RID: 3034
		public Material materialRecharge;

		// Token: 0x04000BDB RID: 3035
		public Material materialPowerDown;
	}

	// Token: 0x020000DE RID: 222
	// (Invoke) Token: 0x06000B55 RID: 2901
	public delegate void PowerupAgnosticEvent(PowerupTriggerAnimController.AnimationCapsule animationCapsule);

	// Token: 0x020000DF RID: 223
	// (Invoke) Token: 0x06000B59 RID: 2905
	public delegate void PowerupEvent(PowerupTriggerAnimController.AnimationCapsule animationCapsule);
}
