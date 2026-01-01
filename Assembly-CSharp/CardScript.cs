using System;
using System.Collections;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;

// Token: 0x020000C3 RID: 195
public class CardScript : MonoBehaviour
{
	// Token: 0x06000A71 RID: 2673 RVA: 0x00054304 File Offset: 0x00052504
	public static CardScript PoolSpawn(RunModifierScript.Identifier identifier, float scale, Transform parent)
	{
		CardScript component = Spawn.FromPool(RunModifierScript.GetCardPrefabName(identifier), Vector3.zero, parent).GetComponent<CardScript>();
		Vector3 vector = Vector3.one * scale;
		component.transform.localScale = vector;
		component.SetRenderingLayer(14);
		component.myOutline.OutlineWidth = Mathf.Max(10f, 10f * scale / 8f);
		return component;
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x0005436C File Offset: 0x0005256C
	public void SetRenderingLayer(int layer)
	{
		for (int i = 0; i < this.transforms.Length; i++)
		{
			if (!(this.transforms[i] == null))
			{
				this.transforms[i].gameObject.layer = layer;
			}
		}
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x0000E521 File Offset: 0x0000C721
	public static void PoolDestroy(CardScript card)
	{
		if (!card.gameObject.activeSelf && card.transform.parent == null)
		{
			return;
		}
		card.transform.SetParent(null);
		Pool.Destroy(card.gameObject, null);
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x0000E55C File Offset: 0x0000C75C
	public void PoolDestroy()
	{
		CardScript.PoolDestroy(this);
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x0000E564 File Offset: 0x0000C764
	public bool IsHovered()
	{
		return CardsInspectorScript.InspectedCard_Get() == this;
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0000E571 File Offset: 0x0000C771
	public bool IsFaceDown()
	{
		return this.faceDown;
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x0000E579 File Offset: 0x0000C779
	public void FlipRequest()
	{
		this.flipRequest = true;
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x0000E582 File Offset: 0x0000C782
	public void ForceUnflipped()
	{
		this.faceDown = false;
		this.meshHolder.SetLocalYAngle(0f);
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x0000E59C File Offset: 0x0000C79C
	public void OutlineForceHidden(bool hide)
	{
		this.forceHideOutline = hide;
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0000E5A5 File Offset: 0x0000C7A5
	public void OutlineSetColor(Color c)
	{
		this.myOutline.OutlineColor = c;
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x000543B0 File Offset: 0x000525B0
	public void TextUpdate()
	{
		int num = Data.game.RunModifier_OwnedCount_Get(this.identifier);
		int num2 = Data.game.RunModifier_WonTimes_Get(this.identifier);
		bool flag = Data.game.DesiredFoilLevelGet(this.identifier) >= 2 || Data.game.RunModifier_FoilLevel_Get(this.identifier) >= 2;
		RunModifierScript.Identifier identifier = GameplayData.RunModifier_GetCurrent();
		if (!DeckBoxScript.CanActuallyCountVictoriesForCards())
		{
			this.textVictories.text = "";
		}
		else
		{
			this.textVictories.text = "<sprite name=\"CardSymb_Victory\">" + num2.ToString();
		}
		if (num < 0 || flag)
		{
			this.textCopies.text = "<sprite name=\"Infinite\"><sprite name=\"CardSymb_Copies\">";
		}
		else
		{
			this.textCopies.text = num.ToString() + "<sprite name=\"CardSymb_Copies\">";
		}
		if (num == 0)
		{
			this.textCopies.color = Color.red;
		}
		else
		{
			this.textCopies.color = Color.white;
		}
		if (identifier == this.identifier && !DeckBoxUI.IsPickingCard(true) && !MemoryPackDealUI.IsDealRunnning())
		{
			this.textActive.text = Translation.Get("CARD_PICKED");
			if (Data.game.RunModifier_HardcoreMode_Get(this.identifier))
			{
				TextMeshPro textMeshPro = this.textActive;
				textMeshPro.text += " <sprite name=\"SkullSymbolOrange64\">";
				return;
			}
		}
		else
		{
			this.textActive.text = "";
		}
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x0000E5B3 File Offset: 0x0000C7B3
	public void TextForceHidden(bool hide)
	{
		this.forceHideText = hide;
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x0000E5BC File Offset: 0x0000C7BC
	public void Bounce()
	{
		this.bounceScript.SetBounceScale(0.05f);
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x0005450C File Offset: 0x0005270C
	private void FlipVfxAndSfx()
	{
		switch (RunModifierScript.RarityGet(this.identifier))
		{
		case RunModifierScript.Rarity.common:
			Sound.Play("SoundCardFlip_Common", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
			this.particlesCommon.SetActive(true);
			this.particlesUncommon.SetActive(false);
			this.particlesRare.SetActive(false);
			this.particlesEpic.SetActive(false);
			break;
		case RunModifierScript.Rarity.uncommon:
			Sound.Play("SoundCardFlip_Uncommon", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
			CameraGame.Shake(1f);
			CameraGame.ChromaticAberrationIntensitySet(0.5f);
			this.particlesCommon.SetActive(false);
			this.particlesUncommon.SetActive(true);
			this.particlesRare.SetActive(false);
			this.particlesEpic.SetActive(false);
			break;
		case RunModifierScript.Rarity.rare:
			Sound.Play("SoundCardFlip_Rare", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
			CameraGame.Shake(2f);
			CameraGame.ChromaticAberrationIntensitySet(1f);
			this.particlesCommon.SetActive(false);
			this.particlesUncommon.SetActive(false);
			this.particlesRare.SetActive(true);
			this.particlesEpic.SetActive(false);
			break;
		case RunModifierScript.Rarity.epic:
			Sound.Play("SoundCardFlip_Epic", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
			CameraGame.Shake(4f);
			CameraGame.ChromaticAberrationIntensitySet(2f);
			FlashScreen.SpawnCamera(Color.yellow, 2f, 4f, CameraUiGlobal.instance.myCamera, 100f);
			this.particlesCommon.SetActive(false);
			this.particlesUncommon.SetActive(false);
			this.particlesRare.SetActive(false);
			this.particlesEpic.SetActive(true);
			break;
		}
		base.StartCoroutine(this.ShowParticlesCoroutine());
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x0000E5CE File Offset: 0x0000C7CE
	private IEnumerator ShowParticlesCoroutine()
	{
		float timer = 1.1f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		this.HideAllParticles();
		yield break;
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0000E5DD File Offset: 0x0000C7DD
	private void HideAllParticles()
	{
		this.particlesCommon.SetActive(false);
		this.particlesUncommon.SetActive(false);
		this.particlesRare.SetActive(false);
		this.particlesEpic.SetActive(false);
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0000E60F File Offset: 0x0000C80F
	public void CardColorSet(Color c)
	{
		this.cardMeshRenderer.sharedMaterial.SetColor("_Color", c);
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0000E627 File Offset: 0x0000C827
	public int DesiredFoilLevelGet()
	{
		return Data.game.DesiredFoilLevelGet(this.identifier);
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0000E639 File Offset: 0x0000C839
	private IEnumerator FoilingCoroutine()
	{
		int desiredFoilingLevel = this.DesiredFoilLevelGet();
		this.animator.Play("CardFoilingAnim");
		CameraGame.Shake(1f);
		CameraGame.ChromaticAberrationIntensitySet(1f);
		Sound.Play("SoundCardFoilingStart", 1f, 1f);
		int num = desiredFoilingLevel;
		if (num != 1)
		{
			if (num == 2)
			{
				this.foilingParticles1.SetActive(true);
			}
		}
		else
		{
			this.foilingParticles0.SetActive(true);
		}
		float timer = 1.1f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		Data.game.RunModifier_FoilLevel_Set(this.identifier, desiredFoilingLevel);
		if (desiredFoilingLevel >= 2)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.MyPrecious);
		}
		this.coroutineFoiling = null;
		yield break;
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x0000E648 File Offset: 0x0000C848
	public bool IsFoiling()
	{
		return this.coroutineFoiling != null;
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x000546F8 File Offset: 0x000528F8
	public static bool IsAnyCardFoiling()
	{
		for (int i = 0; i < CardScript.cardsEnabled.Count; i++)
		{
			if (!(CardScript.cardsEnabled[i] == null) && CardScript.cardsEnabled[i].IsFoiling())
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x00054744 File Offset: 0x00052944
	public void AnimatorApplyFoiling()
	{
		int num = this.DesiredFoilLevelGet();
		this.ApplyFoilingSilently(this.DesiredFoilLevelGet());
		if (num != 1)
		{
			if (num == 2)
			{
				FlashScreen.SpawnCamera(Color.white, 0.5f, 2f, CameraUiGlobal.instance.myCamera, 0.5f);
				CameraGame.ChromaticAberrationIntensitySet(1f);
				Sound.Play("SoundCardFoiling1", 1f, 1f);
			}
		}
		else
		{
			FlashScreen.SpawnCamera(Color.white, 0.5f, 2f, CameraUiGlobal.instance.myCamera, 0.5f);
			CameraGame.ChromaticAberrationIntensitySet(1f);
			Sound.Play("SoundCardFoiling0", 1f, 1f);
		}
		if (num == 1)
		{
			this.foilingParticles0.SetActive(false);
			this.foilingParticles0.SetActive(true);
			return;
		}
		if (num != 2)
		{
			return;
		}
		this.foilingParticles1.SetActive(false);
		this.foilingParticles1.SetActive(true);
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x00054834 File Offset: 0x00052A34
	private void ApplyFoilingSilently(int foilingLevel)
	{
		if (foilingLevel == 1)
		{
			this.cardMeshRenderer.sharedMaterial.SetFloat("_FoilLevel0", 1f);
			this.cardMeshRenderer.sharedMaterial.SetFloat("_FoilLevel1", 0f);
			return;
		}
		if (foilingLevel != 2)
		{
			return;
		}
		this.cardMeshRenderer.sharedMaterial.SetFloat("_FoilLevel0", 0f);
		this.cardMeshRenderer.sharedMaterial.SetFloat("_FoilLevel1", 1f);
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x000548B4 File Offset: 0x00052AB4
	private void Awake()
	{
		if (this.identifier == RunModifierScript.Identifier.undefined || this.identifier == RunModifierScript.Identifier.count)
		{
			Debug.LogError("CardScript instance has identifier set to either UNDEFINED or COUNT");
		}
		this.rectTransform = base.GetComponent<RectTransform>();
		this.myOutline = base.GetComponent<Outline>();
		this.bounceScript = base.GetComponentInChildren<BounceScript>();
		this.animator = base.GetComponent<Animator>();
		this.transforms = base.GetComponentsInChildren<Transform>(true);
		this.materialCopy = new Material(this.cardMeshRenderer.sharedMaterial);
		this.cardMeshRenderer.sharedMaterial = this.materialCopy;
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x00054944 File Offset: 0x00052B44
	private void OnEnable()
	{
		CardScript.cardsEnabled.Add(this);
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		this.flipRequest = false;
		this.faceDown = (Data.game.RunModifier_UnlockedTimes_Get(this.identifier) <= 0 && this.identifier != RunModifierScript.Identifier.defaultModifier) || MemoryPackDealUI.IsDealRunnning();
		if (this.faceDown)
		{
			this.meshHolder.SetLocalYAngle(-180f);
		}
		this.myOutline.enabled = false;
		this.canFlipGlow.SetActive(false);
		this.HideAllParticles();
		this.forceHideOutline = false;
		this.OutlineSetColor(Color.yellow);
		this.TextUpdate();
		this.CardColorSet(Color.white);
		this.animator.Play("ANIM_IDLE");
		this.ApplyFoilingSilently(Data.game.RunModifier_FoilLevel_Get(this.identifier));
		this.foilingParticles0.SetActive(false);
		this.foilingParticles1.SetActive(false);
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x0000E653 File Offset: 0x0000C853
	private void OnDisable()
	{
		CardScript.cardsEnabled.Remove(this);
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x0000E661 File Offset: 0x0000C861
	private void OnDestroy()
	{
		if (this.materialCopy != null)
		{
			global::UnityEngine.Object.Destroy(this.materialCopy);
		}
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x00054A2C File Offset: 0x00052C2C
	private void Update()
	{
		bool flag = this.IsHovered();
		bool flag2 = MemoryPackDealUI.IsDealRunnning();
		bool flag3 = Data.game.RunModifier_OwnedCount_Get(this.identifier) != 0;
		bool flag4 = this.faceDown && (Data.game.RunModifier_UnlockedTimes_Get(this.identifier) > 0 || this.identifier == RunModifierScript.Identifier.defaultModifier || flag2);
		bool flag5 = (flag && !flag2) || this.flipRequest;
		this.flipRequest = false;
		if (flag5 && flag4)
		{
			this.faceDown = false;
			this.FlipVfxAndSfx();
			this.TextUpdate();
		}
		if (this.canFlipGlow.activeSelf != flag4)
		{
			this.canFlipGlow.SetActive(flag4);
		}
		float num = (this.faceDown ? (-180f) : 0f);
		float num2 = this.meshHolder.GetLocalYAngle();
		if (num2 > num + 360f)
		{
			num2 -= 360f;
		}
		if (!this.faceDown)
		{
			num2 = Mathf.Lerp(num2, num, Tick.Time * 10f);
		}
		else
		{
			num2 = num;
		}
		this.meshHolder.SetLocalYAngle(num2);
		Color color = ((Util.AngleSin(Tick.PassedTime * 1440f) > 0f) ? Color.yellow : CardScript.C_ORANGE);
		color.a = this.textActive.alpha;
		if (this.textActive.color != color)
		{
			this.textActive.color = color;
		}
		Color color2 = Color.white;
		if (!flag3)
		{
			color2 = ((Util.AngleSin(Tick.PassedTime * 1440f) > 0f) ? Color.red : Color.yellow);
		}
		if (this.textCopies.color != color2)
		{
			this.textCopies.color = color2;
		}
		float num3 = (float)((Mathf.Abs(num2) < 90f && !this.forceHideText) ? 1 : 0);
		if (this.textVictories.alpha != num3)
		{
			this.textVictories.alpha = num3;
		}
		if (this.textCopies.alpha != num3)
		{
			this.textCopies.alpha = num3;
		}
		if (this.textActive.alpha != num3)
		{
			this.textActive.alpha = num3;
		}
		bool flag6 = this.IsFoiling();
		if (!flag6 && flag && !this.IsFaceDown())
		{
			int num4 = this.DesiredFoilLevelGet();
			int num5 = Data.game.RunModifier_FoilLevel_Get(this.identifier);
			if (num4 > num5)
			{
				this.coroutineFoiling = base.StartCoroutine(this.FoilingCoroutine());
			}
		}
		bool flag7 = flag && !flag6 && Mathf.Abs(Util.AngleSin(num2)) < 0.05f && !this.forceHideOutline && !ScreenMenuScript.IsEnabled();
		if (this.myOutline.enabled != flag7)
		{
			this.myOutline.enabled = flag7;
		}
	}

	// Token: 0x04000A9C RID: 2716
	private static List<CardScript> cardsEnabled = new List<CardScript>();

	// Token: 0x04000A9D RID: 2717
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000A9E RID: 2718
	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x04000A9F RID: 2719
	private const string ANIM_IDLE = "CardIdleAnim";

	// Token: 0x04000AA0 RID: 2720
	private const string ANIM_FOILING = "CardFoilingAnim";

	// Token: 0x04000AA1 RID: 2721
	public GameObject holder;

	// Token: 0x04000AA2 RID: 2722
	public Transform meshHolder;

	// Token: 0x04000AA3 RID: 2723
	public MeshRenderer cardMeshRenderer;

	// Token: 0x04000AA4 RID: 2724
	private Material materialCopy;

	// Token: 0x04000AA5 RID: 2725
	private Transform[] transforms;

	// Token: 0x04000AA6 RID: 2726
	public TextMeshPro textVictories;

	// Token: 0x04000AA7 RID: 2727
	public TextMeshPro textCopies;

	// Token: 0x04000AA8 RID: 2728
	public TextMeshPro textActive;

	// Token: 0x04000AA9 RID: 2729
	[NonSerialized]
	public RectTransform rectTransform;

	// Token: 0x04000AAA RID: 2730
	[NonSerialized]
	public BounceScript bounceScript;

	// Token: 0x04000AAB RID: 2731
	public GameObject particlesCommon;

	// Token: 0x04000AAC RID: 2732
	public GameObject particlesUncommon;

	// Token: 0x04000AAD RID: 2733
	public GameObject particlesRare;

	// Token: 0x04000AAE RID: 2734
	public GameObject particlesEpic;

	// Token: 0x04000AAF RID: 2735
	public GameObject canFlipGlow;

	// Token: 0x04000AB0 RID: 2736
	private Outline myOutline;

	// Token: 0x04000AB1 RID: 2737
	private Animator animator;

	// Token: 0x04000AB2 RID: 2738
	public GameObject foilingParticles0;

	// Token: 0x04000AB3 RID: 2739
	public GameObject foilingParticles1;

	// Token: 0x04000AB4 RID: 2740
	public RunModifierScript.Identifier identifier = RunModifierScript.Identifier.undefined;

	// Token: 0x04000AB5 RID: 2741
	private bool faceDown;

	// Token: 0x04000AB6 RID: 2742
	private bool flipRequest;

	// Token: 0x04000AB7 RID: 2743
	private bool forceHideOutline;

	// Token: 0x04000AB8 RID: 2744
	private bool forceHideText;

	// Token: 0x04000AB9 RID: 2745
	private Coroutine coroutineFoiling;
}
