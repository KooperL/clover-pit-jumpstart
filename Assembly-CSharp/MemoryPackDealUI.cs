using System;
using System.Collections;
using System.Numerics;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class MemoryPackDealUI : MonoBehaviour
{
	// Token: 0x06000971 RID: 2417 RVA: 0x0003E681 File Offset: 0x0003C881
	public static bool IsDealRunnning()
	{
		return !(MemoryPackDealUI.instance == null) && MemoryPackDealUI.instance.dealCoroutine != null;
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0003E6A0 File Offset: 0x0003C8A0
	public static void DealPropose()
	{
		if (MemoryPackDealUI.instance == null)
		{
			return;
		}
		if (MemoryPackDealUI.instance.dealCoroutine != null)
		{
			return;
		}
		MemoryPackDealUI.instance.holder.SetActive(true);
		MemoryPackDealUI.instance.dealCoroutine = MemoryPackDealUI.instance.StartCoroutine(MemoryPackDealUI.instance.DealCoroutine());
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x0003E6F6 File Offset: 0x0003C8F6
	private IEnumerator DealCoroutine()
	{
		float transitionSpeed = (float)Data.settings.transitionSpeed;
		BigInteger debtGet = GameplayData.DebtGet();
		int packsOfferN = GameplayData.RunModifier_BonusPacksThisTime_Get();
		CameraController.PositionKind backupCameraPosition = CameraController.GetPositionKind();
		CameraController.SetPosition(CameraController.PositionKind.ATMStraight, false, 1f);
		float timer = 0.5f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		RunModifierScript.TriggerAnimation_IfEquipped(RunModifierScript.Identifier.extraPacks);
		while (PowerupTriggerAnimController.HasAnimations())
		{
			yield return null;
		}
		if (packsOfferN <= 1)
		{
			DialogueScript.SetQuestionDialogue(false, new DialogueScript.AnswerCallback(this.PacksOffer_OnYes), new DialogueScript.AnswerCallback(this.PacksOffer_OnNo), new string[] { "DIALOGUE_SKIP_DEADLINE_PROPOSAL_0", "DIALOGUE_SKIP_DEADLINE_PROPOSAL_1" });
		}
		else
		{
			DialogueScript.SetQuestionDialogue(false, new DialogueScript.AnswerCallback(this.PacksOffer_OnYes), new DialogueScript.AnswerCallback(this.PacksOffer_OnNo), new string[] { "DIALOGUE_SKIP_DEADLINE_PROPOSAL_0", "DIALOGUE_SKIP_DEADLINE_PROPOSAL_1_ALT_PLURAL" });
		}
		while (DialogueScript.IsEnabled())
		{
			yield return null;
		}
		if (this._skipDeadlineDealAnswer)
		{
			bool firstTimeEverOpeningAPack = Data.game.RunModifier_UnlockedTotalNumber() <= 0;
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_SKIP_DEADLINE_ANSWER_YES_0" });
			float depositDelay = 0.3f;
			float depositDelayTimer = depositDelay;
			while (GameplayData.DepositGet() < debtGet)
			{
				GameplayMaster.instance.FCall_DepositTry();
				while (depositDelayTimer > 0f)
				{
					depositDelayTimer -= Tick.Time * transitionSpeed;
					yield return null;
				}
				depositDelayTimer = depositDelay;
				depositDelay = Mathf.Max(0.1f, depositDelay - 0.05f);
			}
			while (DialogueScript.IsEnabled())
			{
				yield return null;
			}
			if (packsOfferN <= 1)
			{
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_SKIP_DEADLINE_ANSWER_YES_1" });
			}
			else
			{
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_SKIP_DEADLINE_ANSWER_YES_1_ALT_PLURAL" });
			}
			int packsIterator = 0;
			while (packsIterator < packsOfferN)
			{
				this.showCards = false;
				float scale = 0f;
				this.packCenterer.localScale = global::UnityEngine.Vector3.zero;
				this.packHolder.transform.SetLocalY(1024f);
				this.packHolder.SetActive(false);
				this.packParticles.SetActive(false);
				this.backTargetColor = MemoryPackDealUI.C_BLACK_TRASPARENT;
				this.packAnimator.Play("Closed");
				this.packHolder.SetActive(true);
				Sound.Play("SoundPackEnterView", 1f, 1f);
				Sound.Play("SoundPackFlicker", 1f, 1f);
				FlashScreen.SpawnCamera(Color.black, 0.5f, 1f, CameraUiGlobal.instance.myCamera, 100f);
				this.packBounceScr.SetBounceScale(0.1f);
				while (scale < 0.95f)
				{
					scale = Mathf.Lerp(scale, 1f, Tick.Time * 20f);
					this.packCenterer.localScale = global::UnityEngine.Vector3.one * scale * 500f;
					yield return null;
				}
				scale = 1f;
				this.packCenterer.localScale = global::UnityEngine.Vector3.one * scale * 500f;
				FlashScreen.SpawnCamera(Color.black, 0.5f, 1f, CameraUiGlobal.instance.myCamera, 100f);
				while (DialogueScript.IsEnabled())
				{
					yield return null;
				}
				timer = 0.5f;
				while (timer > 0f)
				{
					timer -= Tick.Time;
					yield return null;
				}
				while (!Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true))
				{
					yield return null;
				}
				this.packAnimator.Play("Opened");
				if (packsOfferN >= 3)
				{
					this.packAnimator.speed = 2f;
				}
				timer = 3f;
				while (!this.showCards && timer > 0f)
				{
					timer -= Tick.Time;
					yield return null;
				}
				this.packParticles.SetActive(true);
				PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.ANewHobby);
				CardScript card0 = CardScript.PoolSpawn(RunModifierScript.CardGetFromPack(), 500f, this.cardsHolder);
				CardScript card = CardScript.PoolSpawn(RunModifierScript.CardGetFromPack(), 500f, this.cardsHolder);
				CardScript card2 = CardScript.PoolSpawn(RunModifierScript.CardGetFromPack(), 500f, this.cardsHolder);
				card0.rectTransform.anchoredPosition = global::UnityEngine.Vector2.zero;
				card.rectTransform.anchoredPosition = global::UnityEngine.Vector2.zero;
				card2.rectTransform.anchoredPosition = global::UnityEngine.Vector2.zero;
				card0.rectTransform.SetLocalZ(0f);
				card.rectTransform.SetLocalZ(0f);
				card2.rectTransform.SetLocalZ(0f);
				Data.game.RunModifier_OwnedCount_Set(card0.identifier, Data.game.RunModifier_OwnedCount_Get(card0.identifier) + 1);
				Data.game.RunModifier_OwnedCount_Set(card.identifier, Data.game.RunModifier_OwnedCount_Get(card.identifier) + 1);
				Data.game.RunModifier_OwnedCount_Set(card2.identifier, Data.game.RunModifier_OwnedCount_Get(card2.identifier) + 1);
				Data.game.RunModifier_UnlockedTimes_Set(card0.identifier, Data.game.RunModifier_UnlockedTimes_Get(card0.identifier) + 1);
				Data.game.RunModifier_UnlockedTimes_Set(card.identifier, Data.game.RunModifier_UnlockedTimes_Get(card.identifier) + 1);
				Data.game.RunModifier_UnlockedTimes_Set(card2.identifier, Data.game.RunModifier_UnlockedTimes_Get(card2.identifier) + 1);
				CardScript cardScript = card0;
				if (cardScript != null)
				{
					cardScript.TextUpdate();
				}
				CardScript cardScript2 = card;
				if (cardScript2 != null)
				{
					cardScript2.TextUpdate();
				}
				CardScript cardScript3 = card2;
				if (cardScript3 != null)
				{
					cardScript3.TextUpdate();
				}
				if (this.coroutineCardsMoveAround != null)
				{
					base.StopCoroutine(this.coroutineCardsMoveAround);
				}
				this.coroutineCardsMoveAround = base.StartCoroutine(this.CardsMoveAroundCoroutine(card0, card, card2));
				timer = 1f;
				while (timer > 0f)
				{
					timer -= Tick.Time;
					yield return null;
				}
				CardsInspectorScript.Open("CARDS_INSPECTOR_TITLE__PACK_OPENED", "CARDS_INSPECTOR_DESCRIPTION__PACK_OPENED", CardsInspectorScript.PromptKind.none);
				bool allCardsRevealed = false;
				CardScript hoveredCard = null;
				CardScript hoveredCardOld = null;
				float forceNoCardTimer = 0f;
				float axisXPrevious = 0f;
				for (;;)
				{
					bool flag = false;
					if (CardScript.IsAnyCardFoiling())
					{
						yield return null;
					}
					else
					{
						if (!card0.IsFaceDown() && !card.IsFaceDown() && !card2.IsFaceDown() && !allCardsRevealed)
						{
							allCardsRevealed = true;
							flag = true;
						}
						if (flag)
						{
							CardsInspectorScript.Close();
							CardsInspectorScript.Open("CARDS_INSPECTOR_TITLE__PACK_OPENED_ALL_INSPECTED", "CARDS_INSPECTOR_DESCRIPTION__PACK_OPENED_ALL_INSPECTED", CardsInspectorScript.PromptKind.none);
							forceNoCardTimer = 0.5f;
						}
						float axisX = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.menuMoveRight, Controls.InputAction.menuMoveLeft, true);
						global::UnityEngine.Vector2 vector = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, this.canvasScaler.referenceResolution);
						bool flag2 = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true);
						bool flag3 = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true);
						bool flag4 = false;
						if (forceNoCardTimer > 0f)
						{
							forceNoCardTimer -= Tick.Time;
							flag4 = true;
						}
						else if (VirtualCursors.IsCursorVisible(0, true))
						{
							hoveredCard = null;
							CardScript cardScript4 = card0;
							if (vector.x < cardScript4.rectTransform.anchoredPosition.x + cardScript4.rectTransform.sizeDelta.x / 2f * 500f && vector.x > cardScript4.rectTransform.anchoredPosition.x - cardScript4.rectTransform.sizeDelta.x / 2f * 500f && vector.y < cardScript4.rectTransform.anchoredPosition.y + cardScript4.rectTransform.sizeDelta.y / 2f * 500f && vector.y > cardScript4.rectTransform.anchoredPosition.y - cardScript4.rectTransform.sizeDelta.y / 2f * 500f)
							{
								hoveredCard = cardScript4;
							}
							cardScript4 = card;
							if (vector.x < cardScript4.rectTransform.anchoredPosition.x + cardScript4.rectTransform.sizeDelta.x / 2f * 500f && vector.x > cardScript4.rectTransform.anchoredPosition.x - cardScript4.rectTransform.sizeDelta.x / 2f * 500f && vector.y < cardScript4.rectTransform.anchoredPosition.y + cardScript4.rectTransform.sizeDelta.y / 2f * 500f && vector.y > cardScript4.rectTransform.anchoredPosition.y - cardScript4.rectTransform.sizeDelta.y / 2f * 500f)
							{
								hoveredCard = cardScript4;
							}
							cardScript4 = card2;
							if (vector.x < cardScript4.rectTransform.anchoredPosition.x + cardScript4.rectTransform.sizeDelta.x / 2f * 500f && vector.x > cardScript4.rectTransform.anchoredPosition.x - cardScript4.rectTransform.sizeDelta.x / 2f * 500f && vector.y < cardScript4.rectTransform.anchoredPosition.y + cardScript4.rectTransform.sizeDelta.y / 2f * 500f && vector.y > cardScript4.rectTransform.anchoredPosition.y - cardScript4.rectTransform.sizeDelta.y / 2f * 500f)
							{
								hoveredCard = cardScript4;
							}
						}
						else if (hoveredCard == null)
						{
							if (Mathf.Abs(axisX) > 0.35f || flag2)
							{
								hoveredCard = card0;
								flag4 = true;
							}
						}
						else
						{
							if (axisX > 0.35f && axisXPrevious <= 0.35f)
							{
								if (hoveredCard == card)
								{
									hoveredCard = card2;
								}
								else if (hoveredCard == card0)
								{
									hoveredCard = card;
								}
							}
							else if (axisX < -0.35f && axisXPrevious >= -0.35f)
							{
								if (hoveredCard == card)
								{
									hoveredCard = card0;
								}
								else if (hoveredCard == card2)
								{
									hoveredCard = card;
								}
							}
							if (flag3 && hoveredCard != null)
							{
								hoveredCard = null;
							}
						}
						if (hoveredCardOld != hoveredCard)
						{
							hoveredCardOld = hoveredCard;
							if (hoveredCard != null)
							{
								Sound.Play("SoundCardChange", 1f, 1f);
							}
						}
						CardsInspectorScript.InspectCard_Set(hoveredCard, hoveredCard == null || hoveredCard.IsFaceDown());
						if (hoveredCard != null && hoveredCard.IsFaceDown() && flag2 && !flag4)
						{
							hoveredCard.Bounce();
							Sound.Play("SoundCardSelect", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
							timer = 0.15f;
							if (RunModifierScript.RarityGet(hoveredCard.identifier) == RunModifierScript.Rarity.rare)
							{
								timer += 0.05f;
							}
							if (RunModifierScript.RarityGet(hoveredCard.identifier) == RunModifierScript.Rarity.epic)
							{
								timer += 0.1f;
							}
							while (timer > 0f)
							{
								timer -= Tick.Time;
								yield return null;
							}
							hoveredCard.FlipRequest();
							hoveredCard.OutlineSetColor(MemoryPackDealUI.C_ORANGE);
						}
						if (allCardsRevealed)
						{
							bool flag5 = VirtualCursors.IsCursorVisible(0, true) && hoveredCard != null;
							if ((Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) && !flag5) || Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true))
							{
								break;
							}
						}
						axisXPrevious = axisX;
						yield return null;
					}
				}
				card0.OutlineForceHidden(true);
				card.OutlineForceHidden(true);
				card2.OutlineForceHidden(true);
				this.showCards = false;
				Sound.Play("SoundCardsMoveOut", 1f, 1f);
				timer = 0.5f;
				while (timer > 0f)
				{
					timer -= Tick.Time;
					yield return null;
				}
				if (this.coroutineCardsMoveAround != null)
				{
					base.StopCoroutine(this.coroutineCardsMoveAround);
				}
				CardScript cardScript5 = card0;
				if (cardScript5 != null)
				{
					cardScript5.PoolDestroy();
				}
				CardScript cardScript6 = card;
				if (cardScript6 != null)
				{
					cardScript6.PoolDestroy();
				}
				CardScript cardScript7 = card2;
				if (cardScript7 != null)
				{
					cardScript7.PoolDestroy();
				}
				CardsInspectorScript.Close();
				timer = 3f;
				while (this.coroutineHidePack != null && timer > 0f)
				{
					timer -= Tick.Time;
					yield return null;
				}
				int num = packsIterator;
				packsIterator = num + 1;
				card0 = null;
				card = null;
				card2 = null;
				hoveredCard = null;
				hoveredCardOld = null;
			}
			this.backTargetColor = MemoryPackDealUI.C_INVISIBLE;
			if (firstTimeEverOpeningAPack)
			{
				CameraController.SetPosition(CameraController.PositionKind.DeckBox, false, 1f * transitionSpeed);
				timer = 0.5f;
				while (timer > 0f)
				{
					timer -= Tick.Time;
					yield return null;
				}
				DialogueScript.SetDialogue(true, new string[] { "DIALOGUE_SKIP_CARDS_EXPLANATION_0" });
				DialogueScript.SetDialogueInputDelay(0.5f);
				while (DialogueScript.IsEnabled())
				{
					yield return null;
				}
				CameraController.SetPosition(CameraController.PositionKind.ATMStraight, false, 0f);
				while (!CameraController.IsCameraNearPositionAndAngle(0.1f))
				{
					yield return null;
				}
			}
			GameplayData.RunModifier_AcceptedDealsCounter_Set(GameplayData.RunModifier_AcceptedDealsCounter_Get() + 1);
			bool flag6 = true;
			int num2 = 20;
			for (int i = 0; i < num2; i++)
			{
				RunModifierScript.Identifier identifier = (RunModifierScript.Identifier)i;
				if (identifier != RunModifierScript.Identifier.defaultModifier && Data.game.RunModifier_UnlockedTimes_Get(identifier) <= 0)
				{
					flag6 = false;
					break;
				}
			}
			if (flag6)
			{
				PowerupScript.Unlock(PowerupScript.Identifier.TheCollector);
			}
			while (PowerupTriggerAnimController.HasAnimations())
			{
				yield return null;
			}
		}
		else
		{
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_SKIP_DEADLINE_ANSWER_NO" });
			while (DialogueScript.IsEnabled())
			{
				yield return null;
			}
		}
		VirtualCursors.CursorDesiredVisibilitySet(0, false);
		CameraController.SetPosition(backupCameraPosition, false, 0f);
		CameraController.SetFreeCameraRotation(CameraController.instance.ATMStraightTransform.eulerAngles);
		this.holder.SetActive(false);
		this.dealCoroutine = null;
		yield break;
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x0003E705 File Offset: 0x0003C905
	private void PacksOffer_OnYes()
	{
		this._skipDeadlineDealAnswer = true;
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x0003E70E File Offset: 0x0003C90E
	private void PacksOffer_OnNo()
	{
		this._skipDeadlineDealAnswer = false;
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x0003E717 File Offset: 0x0003C917
	public void Pack_Hide()
	{
		if (this.coroutineHidePack == null)
		{
			base.StartCoroutine(this.HidePackCoroutine());
		}
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0003E72E File Offset: 0x0003C92E
	private IEnumerator HidePackCoroutine()
	{
		this.packParticles.SetActive(false);
		float scale = 1f;
		while (scale > 0f)
		{
			scale -= Tick.Time * 4f;
			this.packCenterer.localScale = global::UnityEngine.Vector3.one * scale * 500f;
			yield return null;
		}
		this.packCenterer.localScale = global::UnityEngine.Vector3.zero;
		this.coroutineHidePack = null;
		yield break;
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x0003E73D File Offset: 0x0003C93D
	public void Pack_ShowCards()
	{
		this.showCards = true;
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x0003E746 File Offset: 0x0003C946
	private IEnumerator CardsMoveAroundCoroutine(CardScript c0, CardScript c1, CardScript c2)
	{
		float lerpSpeed = 0f;
		while (c0 != null && c1 != null && c2 != null)
		{
			lerpSpeed = Mathf.Min(1f, lerpSpeed + Tick.Time * 1f);
			global::UnityEngine.Vector2 vector;
			global::UnityEngine.Vector2 vector2;
			global::UnityEngine.Vector2 vector3;
			if (this.showCards)
			{
				vector.x = -212f + Util.AngleSin(Tick.PassedTime * 45f) * 8f;
				vector.y = 32f + Util.AngleSin(Tick.PassedTime * 35f) * 4f;
				vector2.x = 0f + Util.AngleSin(Tick.PassedTime * 45f + 90f) * 8f;
				vector2.y = 32f + Util.AngleSin(Tick.PassedTime * 35f + 90f) * 4f;
				vector3.x = 212f + Util.AngleSin(Tick.PassedTime * 45f + 180f) * 8f;
				vector3.y = 32f + Util.AngleSin(Tick.PassedTime * 35f + 180f) * 4f;
			}
			else
			{
				vector.x = 1172f + Util.AngleSin(Tick.PassedTime * 45f) * 8f;
				vector.y = 32f + Util.AngleSin(Tick.PassedTime * 35f) * 4f;
				vector2.x = 1172f + Util.AngleSin(Tick.PassedTime * 45f + 90f) * 8f;
				vector2.y = 32f + Util.AngleSin(Tick.PassedTime * 35f + 90f) * 4f;
				vector3.x = 1172f + Util.AngleSin(Tick.PassedTime * 45f + 180f) * 8f;
				vector3.y = 32f + Util.AngleSin(Tick.PassedTime * 35f + 180f) * 4f;
			}
			float num = Util.AngleSin(Tick.PassedTime * 30f) * 5f;
			float num2 = Util.AngleSin(Tick.PassedTime * 30f + 90f) * 5f;
			float num3 = Util.AngleSin(Tick.PassedTime * 30f + 180f) * 5f;
			if (c0 != null)
			{
				c0.rectTransform.anchoredPosition = global::UnityEngine.Vector2.Lerp(c0.rectTransform.anchoredPosition, vector, Tick.Time * 10f * lerpSpeed);
				c0.rectTransform.SetLocalXAngle(0f);
				c0.rectTransform.SetLocalYAngle(0f);
				c0.rectTransform.SetLocalZAngle(num);
			}
			if (c1 != null)
			{
				c1.rectTransform.anchoredPosition = global::UnityEngine.Vector2.Lerp(c1.rectTransform.anchoredPosition, vector2, Tick.Time * 10f * lerpSpeed);
				c1.rectTransform.SetLocalXAngle(0f);
				c1.rectTransform.SetLocalYAngle(0f);
				c1.rectTransform.SetLocalZAngle(num2);
			}
			if (c2 != null)
			{
				c2.rectTransform.anchoredPosition = global::UnityEngine.Vector2.Lerp(c2.rectTransform.anchoredPosition, vector3, Tick.Time * 10f * lerpSpeed);
				c2.rectTransform.SetLocalXAngle(0f);
				c2.rectTransform.SetLocalYAngle(0f);
				c2.rectTransform.SetLocalZAngle(num3);
			}
			yield return null;
		}
		this.coroutineCardsMoveAround = null;
		yield break;
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x0003E76A File Offset: 0x0003C96A
	private void Awake()
	{
		MemoryPackDealUI.instance = this;
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x0003E772 File Offset: 0x0003C972
	private void Start()
	{
		this.holder.SetActive(false);
		this.packHolder.SetActive(false);
		this.packParticles.SetActive(false);
		this.backImage.color = this.backTargetColor;
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x0003E7A9 File Offset: 0x0003C9A9
	private void OnDestroy()
	{
		if (MemoryPackDealUI.instance == this)
		{
			MemoryPackDealUI.instance = null;
		}
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x0003E7C0 File Offset: 0x0003C9C0
	private void Update()
	{
		if (!this.holder.activeSelf)
		{
			return;
		}
		global::UnityEngine.Vector2 vector;
		vector.x = Util.AngleSin(Tick.PassedTime * 65f) * 16f;
		vector.y = Util.AngleSin(Tick.PassedTime * 135f) * 12f + (DialogueScript.IsEnabled() ? 48f : 0f);
		global::UnityEngine.Vector3 vector2 = new global::UnityEngine.Vector3(vector.x, vector.y, 75f);
		this.packHolder.transform.localPosition = global::UnityEngine.Vector3.Lerp(this.packHolder.transform.localPosition, vector2, Tick.Time * 20f);
		global::UnityEngine.Vector2 vector3;
		vector3.x = Util.AngleSin(Tick.PassedTime * 45f) * 30f;
		vector3.y = Util.AngleSin(Tick.PassedTime * 90f) * 15f;
		this.packCenterer.localEulerAngles = new global::UnityEngine.Vector3(vector3.x, vector3.y, 0f);
		this.backImage.color = Color.Lerp(this.backImage.color, this.backTargetColor, Tick.Time * 10f);
	}

	public static MemoryPackDealUI instance;

	public const int PLAYER_INDEX = 0;

	private const float PACK_SCALE_MULT = 500f;

	private const float CARD_X_DIST = 212f;

	private const float CARD_SIZE_MULT = 500f;

	private static Color C_INVISIBLE = new Color(0f, 0f, 0f, 0f);

	private static Color C_BLACK_TRASPARENT = new Color(0f, 0f, 0f, 0.5f);

	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	public GameObject holder;

	public CanvasScaler canvasScaler;

	public Image backImage;

	public GameObject packHolder;

	public Transform packCenterer;

	public BounceScript packBounceScr;

	public Animator packAnimator;

	public GameObject packParticles;

	public Transform cardsHolder;

	private Coroutine dealCoroutine;

	private bool _skipDeadlineDealAnswer;

	private Color backTargetColor = MemoryPackDealUI.C_INVISIBLE;

	private Coroutine coroutineHidePack;

	private bool showCards;

	private Coroutine coroutineCardsMoveAround;
}
