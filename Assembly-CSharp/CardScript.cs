using System;
using System.Collections;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;

public class CardScript : MonoBehaviour
{
	// Token: 0x06000929 RID: 2345 RVA: 0x0003CC24 File Offset: 0x0003AE24
	public static CardScript PoolSpawn(RunModifierScript.Identifier identifier, float scale, Transform parent)
	{
		CardScript component = Spawn.FromPool(RunModifierScript.GetCardPrefabName(identifier), Vector3.zero, parent).GetComponent<CardScript>();
		Vector3 vector = Vector3.one * scale;
		component.transform.localScale = vector;
		component.SetRenderingLayer(14);
		component.myOutline.OutlineWidth = Mathf.Max(10f, 10f * scale / 8f);
		return component;
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x0003CC8C File Offset: 0x0003AE8C
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

	// Token: 0x0600092B RID: 2347 RVA: 0x0003CCCF File Offset: 0x0003AECF
	public static void PoolDestroy(CardScript card)
	{
		if (!card.gameObject.activeSelf && card.transform.parent == null)
		{
			return;
		}
		card.transform.SetParent(null);
		Pool.Destroy(card.gameObject, null);
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x0003CD0A File Offset: 0x0003AF0A
	public void PoolDestroy()
	{
		CardScript.PoolDestroy(this);
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x0003CD12 File Offset: 0x0003AF12
	public bool IsHovered()
	{
		return CardsInspectorScript.InspectedCard_Get() == this;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x0003CD1F File Offset: 0x0003AF1F
	public bool IsFaceDown()
	{
		return this.faceDown;
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x0003CD27 File Offset: 0x0003AF27
	public void FlipRequest()
	{
		this.flipRequest = true;
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x0003CD30 File Offset: 0x0003AF30
	public void ForceUnflipped()
	{
		this.faceDown = false;
		this.meshHolder.SetLocalYAngle(0f);
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0003CD4A File Offset: 0x0003AF4A
	public void OutlineForceHidden(bool hide)
	{
		this.forceHideOutline = hide;
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x0003CD53 File Offset: 0x0003AF53
	public void OutlineSetColor(Color c)
	{
		this.myOutline.OutlineColor = c;
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x0003CD64 File Offset: 0x0003AF64
	public void TextUpdate()
	{
		int num = Data.game.RunModifier_OwnedCount_Get(this.identifier);
		int num2 = Data.game.RunModifier_WonTimes_Get(this.identifier);
		RunModifierScript.Identifier identifier = GameplayData.RunModifier_GetCurrent();
		if (!DeckBoxScript.CanActuallyCountVictoriesForCards())
		{
			this.textVictories.text = "";
		}
		else
		{
			this.textVictories.text = "<sprite name=\"CardSymb_Victory\">" + num2.ToString();
		}
		if (num < 0)
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

	// Token: 0x06000934 RID: 2356 RVA: 0x0003CE8E File Offset: 0x0003B08E
	public void TextForceHidden(bool hide)
	{
		this.forceHideText = hide;
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x0003CE97 File Offset: 0x0003B097
	public void Bounce()
	{
		this.bounceScript.SetBounceScale(0.05f);
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x0003CEAC File Offset: 0x0003B0AC
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

	// Token: 0x06000937 RID: 2359 RVA: 0x0003D098 File Offset: 0x0003B298
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

	// Token: 0x06000938 RID: 2360 RVA: 0x0003D0A7 File Offset: 0x0003B2A7
	private void HideAllParticles()
	{
		this.particlesCommon.SetActive(false);
		this.particlesUncommon.SetActive(false);
		this.particlesRare.SetActive(false);
		this.particlesEpic.SetActive(false);
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x0003D0D9 File Offset: 0x0003B2D9
	public void CardColorSet(Color c)
	{
		this.cardMeshRenderer.sharedMaterial.SetColor("_Color", c);
	}

	// Token: 0x0600093A RID: 2362 RVA: 0x0003D0F4 File Offset: 0x0003B2F4
	private int DesiredFoilLevelGet()
	{
		int num = Data.game.RunModifier_WonTimes_Get(this.identifier);
		if (Data.game.RunModifier_WonTimesHARDCORE_Get(this.identifier) > 0)
		{
			return 2;
		}
		if (num > 0)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x0003D12E File Offset: 0x0003B32E
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

	// Token: 0x0600093C RID: 2364 RVA: 0x0003D13D File Offset: 0x0003B33D
	public bool IsFoiling()
	{
		return this.coroutineFoiling != null;
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x0003D148 File Offset: 0x0003B348
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

	// Token: 0x0600093E RID: 2366 RVA: 0x0003D194 File Offset: 0x0003B394
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

	// Token: 0x0600093F RID: 2367 RVA: 0x0003D284 File Offset: 0x0003B484
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

	// Token: 0x06000940 RID: 2368 RVA: 0x0003D304 File Offset: 0x0003B504
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

	// Token: 0x06000941 RID: 2369 RVA: 0x0003D394 File Offset: 0x0003B594
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

	// Token: 0x06000942 RID: 2370 RVA: 0x0003D47C File Offset: 0x0003B67C
	private void OnDisable()
	{
		CardScript.cardsEnabled.Remove(this);
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0003D48A File Offset: 0x0003B68A
	private void OnDestroy()
	{
		if (this.materialCopy != null)
		{
			global::UnityEngine.Object.Destroy(this.materialCopy);
		}
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x0003D4A8 File Offset: 0x0003B6A8
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

	private static List<CardScript> cardsEnabled = new List<CardScript>();

	private const int PLAYER_INDEX = 0;

	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	private const string ANIM_IDLE = "CardIdleAnim";

	private const string ANIM_FOILING = "CardFoilingAnim";

	public GameObject holder;

	public Transform meshHolder;

	public MeshRenderer cardMeshRenderer;

	private Material materialCopy;

	private Transform[] transforms;

	public TextMeshPro textVictories;

	public TextMeshPro textCopies;

	public TextMeshPro textActive;

	[NonSerialized]
	public RectTransform rectTransform;

	[NonSerialized]
	public BounceScript bounceScript;

	public GameObject particlesCommon;

	public GameObject particlesUncommon;

	public GameObject particlesRare;

	public GameObject particlesEpic;

	public GameObject canFlipGlow;

	private Outline myOutline;

	private Animator animator;

	public GameObject foilingParticles0;

	public GameObject foilingParticles1;

	public RunModifierScript.Identifier identifier = RunModifierScript.Identifier.undefined;

	private bool faceDown;

	private bool flipRequest;

	private bool forceHideOutline;

	private bool forceHideText;

	private Coroutine coroutineFoiling;
}
