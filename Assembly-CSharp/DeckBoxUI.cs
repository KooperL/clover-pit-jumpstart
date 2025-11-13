using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckBoxUI : MonoBehaviour
{
	// Token: 0x0600095C RID: 2396 RVA: 0x0003DC22 File Offset: 0x0003BE22
	public static bool IsEnabled()
	{
		return !(DeckBoxUI.instance == null) && DeckBoxUI.instance.holder.activeSelf;
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0003DC42 File Offset: 0x0003BE42
	public static bool IsPickingCard(bool considerEnabledState)
	{
		return !(DeckBoxUI.instance == null) && DeckBoxUI.IsEnabled() && DeckBoxUI.instance.uiKindOpenedTo == DeckBoxUI.UiKind.pickCardForTheRun;
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x0003DC6C File Offset: 0x0003BE6C
	public static void Open(DeckBoxUI.UiKind uiKind)
	{
		if (DeckBoxUI.instance == null)
		{
			return;
		}
		if (DeckBoxUI.instance.uiCoroutine != null)
		{
			return;
		}
		DeckBoxUI.instance.uiKindOpenedTo = uiKind;
		DeckBoxUI.instance.backupCameraPosition = CameraController.GetPositionKind();
		CameraController.SetPosition(CameraController.PositionKind.DeckBox, false, (uiKind == DeckBoxUI.UiKind.pickCardForTheRun) ? 0f : 1f);
		DeckBoxUI.instance.holder.SetActive(true);
		DeckBoxUI.instance._TextUpdate(uiKind);
		Sound.Play3D("SoundDeckBoxOpen", DeckBoxUI.instance.transform.position, 20f, 1f, 1f, 1);
		DeckBoxUI.instance.uiCoroutine = DeckBoxUI.instance.StartCoroutine(DeckBoxUI.instance.UiCoroutine(uiKind));
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x0003DD2C File Offset: 0x0003BF2C
	public static void Close()
	{
		if (DeckBoxUI.instance == null)
		{
			return;
		}
		if (DeckBoxUI.instance.uiCoroutine != null)
		{
			DeckBoxUI.instance.StopCoroutine(DeckBoxUI.instance.uiCoroutine);
		}
		DeckBoxUI.instance.holder.SetActive(false);
		Sound.Play3D("SoundDeckBoxClose", DeckBoxUI.instance.transform.position, 20f, 1f, 1f, 1);
		if (DeckBoxUI.instance.backupCameraPosition != CameraController.PositionKind.Undefined)
		{
			CameraController.SetPosition(DeckBoxUI.instance.backupCameraPosition, false, 1f);
		}
		GoldenToiletStickerScript.RefreshVisualsStatic();
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x0003DDC9 File Offset: 0x0003BFC9
	private IEnumerator UiCoroutine(DeckBoxUI.UiKind uiKind)
	{
		int cardsCount = 20;
		this.buttonLeftCursor.enabled = false;
		this.buttonActionCursor.enabled = false;
		this.buttonRightCursor.enabled = false;
		bool cursorBackupState = VirtualCursors.CursorDesiredVisibilityGet(0);
		VirtualCursors.CursorDesiredVisibilitySet(0, true);
		CardsInspectorScript.Open("CARDS_INSPECTOR_TITLE__UNDISCOVERED", "CARDS_INSPECTOR_DESCRIPTION__UNDISCOVERED", CardsInspectorScript.PromptKind.none);
		this._cardsList.Clear();
		for (int i = 0; i < cardsCount; i++)
		{
			RunModifierScript.Identifier identifier = (RunModifierScript.Identifier)i;
			CardScript cardScript = CardScript.PoolSpawn(identifier, 300f, this.cardsHolder);
			cardScript.rectTransform.anchoredPosition = new Vector2(1324f, 44f);
			cardScript.transform.SetLocalZ(0f);
			int num = Data.game.RunModifier_UnlockedTimes_Get(cardScript.identifier);
			int num2 = Data.game.RunModifier_OwnedCount_Get(identifier);
			if (num > 0 && num2 == 0 && (uiKind == DeckBoxUI.UiKind.pickCardForTheRun || GameplayData.RunModifier_GetCurrent() != cardScript.identifier))
			{
				cardScript.CardColorSet(DeckBoxUI.C_DARK_GRAY);
			}
			else
			{
				cardScript.CardColorSet(Color.white);
			}
			cardScript.OutlineSetColor(Color.black);
			this._cardsList.Add(cardScript);
		}
		for (int j = 0; j < this._cardsList.Count; j++)
		{
			int num3 = RunModifierScript.OrderWeightGet(this._cardsList[j].identifier);
			for (int k = j + 1; k < this._cardsList.Count; k++)
			{
				int num4 = RunModifierScript.OrderWeightGet(this._cardsList[k].identifier);
				if (num3 > num4)
				{
					CardScript cardScript2 = this._cardsList[j];
					this._cardsList[j] = this._cardsList[k];
					this._cardsList[k] = cardScript2;
					num3 = RunModifierScript.OrderWeightGet(this._cardsList[j].identifier);
				}
			}
		}
		this.cardNavigationIndex = 0;
		if (uiKind == DeckBoxUI.UiKind.seeCollection)
		{
			RunModifierScript.Identifier identifier2 = GameplayData.RunModifier_GetCurrent();
			for (int l = 0; l < this._cardsList.Count; l++)
			{
				if (this._cardsList[l].identifier == identifier2)
				{
					this.cardNavigationIndex = l;
					break;
				}
			}
		}
		base.StartCoroutine(this.FlashInstructionsText());
		float timer = 0.5f;
		while (timer > 0f && !DeckBoxUI.IsForceClosing())
		{
			timer -= Tick.Time;
			yield return null;
		}
		RectTransform hoveredRect = null;
		RectTransform hoveredRectOld = null;
		CardScript hoveredCard = null;
		float axisXOld = 0f;
		float axisYOld = 0f;
		while (!DeckBoxUI.IsForceClosing())
		{
			if (CardScript.IsAnyCardFoiling())
			{
				if (DeckBoxUI.IsForceClosing())
				{
					break;
				}
				yield return null;
			}
			else
			{
				hoveredCard = null;
				if (!ScreenMenuScript.IsEnabled())
				{
					if (this.menuSelection_ExitTime)
					{
						break;
					}
					if (this.menuSelection_WaitFrame)
					{
						yield return null;
					}
					this.menuSelection_WaitFrame = false;
					if (uiKind == DeckBoxUI.UiKind.seeCollection && Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true))
					{
						if (!(hoveredRect != null))
						{
							Sound.Play("SoundMenuBack", 1f, 1f);
							break;
						}
						hoveredRect = null;
					}
					bool flag = VirtualCursors.IsCursorVisible(0, true);
					float num5 = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.menuMoveRight, Controls.InputAction.menuMoveLeft, true);
					float num6 = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.menuMoveUp, Controls.InputAction.menuMoveDown, true);
					Vector2 vector = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, this.canvasScaler.referenceResolution);
					bool flag2 = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true);
					bool flag3 = false;
					if (flag)
					{
						hoveredRect = null;
						if (vector.x < this.button_Left.anchoredPosition.x + this.button_Left.sizeDelta.x / 2f && vector.x > this.button_Left.anchoredPosition.x - this.button_Left.sizeDelta.x / 2f && vector.y < this.button_Left.anchoredPosition.y + this.button_Left.sizeDelta.y / 2f && vector.y > this.button_Left.anchoredPosition.y - this.button_Left.sizeDelta.y / 2f)
						{
							hoveredRect = this.button_Left;
						}
						else if (vector.x < this.button_Right.anchoredPosition.x + this.button_Right.sizeDelta.x / 2f && vector.x > this.button_Right.anchoredPosition.x - this.button_Right.sizeDelta.x / 2f && vector.y < this.button_Right.anchoredPosition.y + this.button_Right.sizeDelta.y / 2f && vector.y > this.button_Right.anchoredPosition.y - this.button_Right.sizeDelta.y / 2f)
						{
							hoveredRect = this.button_Right;
						}
						else if (vector.x < this.button_Action.anchoredPosition.x + this.button_Action.sizeDelta.x / 2f && vector.x > this.button_Action.anchoredPosition.x - this.button_Action.sizeDelta.x / 2f && vector.y < this.button_Action.anchoredPosition.y + this.button_Action.sizeDelta.y / 2f && vector.y > this.button_Action.anchoredPosition.y - this.button_Action.sizeDelta.y / 2f)
						{
							hoveredRect = this.button_Action;
						}
						if (hoveredRect == null)
						{
							for (int m = Mathf.Max(0, this.cardNavigationIndex - 5); m < this._cardsList.Count; m++)
							{
								if (m >= this.cardNavigationIndex + 5)
								{
									break;
								}
								float x = this._cardsList[m].rectTransform.localScale.x;
								if (vector.x < this._cardsList[m].rectTransform.anchoredPosition.x + this._cardsList[m].rectTransform.sizeDelta.x / 2f * x && vector.x > this._cardsList[m].rectTransform.anchoredPosition.x - this._cardsList[m].rectTransform.sizeDelta.x / 2f * x && vector.y < this._cardsList[m].rectTransform.anchoredPosition.y + this._cardsList[m].rectTransform.sizeDelta.y / 2f * x && vector.y > this._cardsList[m].rectTransform.anchoredPosition.y - this._cardsList[m].rectTransform.sizeDelta.y / 2f * x)
								{
									hoveredCard = this._cardsList[m];
									break;
								}
							}
						}
					}
					else
					{
						if ((flag2 || (num6 < -0.35f && axisYOld >= -0.35f)) && hoveredRect != this.button_Action)
						{
							hoveredRect = this.button_Action;
							flag3 = true;
						}
						if (num5 < -0.35f && axisXOld >= -0.35f)
						{
							hoveredRect = this.button_Left;
							flag2 = true;
						}
						if (num5 > 0.35f && axisXOld <= 0.35f)
						{
							hoveredRect = this.button_Right;
							flag2 = true;
						}
					}
					if (hoveredRectOld != hoveredRect)
					{
						hoveredRectOld = hoveredRect;
						if (hoveredRect != null)
						{
							Sound.Play("SoundMenuSelectionChange", 1f, 1f);
						}
						this.buttonLeftCursor.enabled = false;
						this.buttonRightCursor.enabled = false;
						this.buttonActionCursor.enabled = false;
						if (hoveredRect == this.button_Left)
						{
							this.buttonLeftCursor.enabled = true;
						}
						if (hoveredRect == this.button_Right)
						{
							this.buttonRightCursor.enabled = true;
						}
						if (hoveredRect == this.button_Action)
						{
							this.buttonActionCursor.enabled = true;
						}
					}
					if (hoveredRect != null && flag2 && !flag3 && this.Select(uiKind, hoveredRect))
					{
						break;
					}
					if (hoveredCard != null && flag2 && !flag3)
					{
						for (int n = 0; n < this._cardsList.Count; n++)
						{
							if (this._cardsList[n] == hoveredCard)
							{
								this.cardNavigationIndex = n;
								break;
							}
						}
						Sound.Play("SoundCardChange", 1f, 1f);
					}
					bool flag4 = Controls.ActionButton_PressedGet(0, Controls.InputAction.scrollUp, true) || Controls.ActionButton_PressedGet(0, Controls.InputAction.menuTabRight, true);
					if (Controls.ActionButton_PressedGet(0, Controls.InputAction.scrollDown, true) || Controls.ActionButton_PressedGet(0, Controls.InputAction.menuTabLeft, true))
					{
						this.cardNavigationIndex--;
						if (this.cardNavigationIndex < 0)
						{
							this.cardNavigationIndex = cardsCount - 1;
						}
						Sound.Play("SoundCardChange", 1f, 1f);
					}
					if (flag4)
					{
						this.cardNavigationIndex++;
						if (this.cardNavigationIndex >= cardsCount)
						{
							this.cardNavigationIndex = 0;
						}
						Sound.Play("SoundCardChange", 1f, 1f);
					}
					axisXOld = num5;
					axisYOld = num6;
					if (!flag && hoveredRect != null && hoveredRect != this.button_Action)
					{
						hoveredRect = null;
						timer = 0.05f;
						while (timer > 0f && !DeckBoxUI.IsForceClosing())
						{
							timer -= Tick.Time;
							yield return null;
						}
					}
				}
				yield return null;
			}
		}
		this.menuSelection_ExitTime = false;
		CardsInspectorScript.Close();
		VirtualCursors.CursorDesiredVisibilitySet(0, cursorBackupState);
		if (this.coroutineFlashInstructionsText != null)
		{
			base.StopCoroutine(this.coroutineFlashInstructionsText);
		}
		for (int num7 = this._cardsList.Count - 1; num7 >= 0; num7--)
		{
			this._cardsList[num7].PoolDestroy();
		}
		this._cardsList.Clear();
		GameplayMaster.instance.FCall_DeckBoxClose();
		this.forceClose_Death = false;
		this.uiCoroutine = null;
		yield break;
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x0003DDE0 File Offset: 0x0003BFE0
	private bool Select(DeckBoxUI.UiKind uiKind, RectTransform hoveredRect)
	{
		bool flag = false;
		int num = 20;
		if (hoveredRect == this.button_Left)
		{
			this.cardNavigationIndex--;
			if (this.cardNavigationIndex < 0)
			{
				this.cardNavigationIndex = num - 1;
			}
			Sound.Play("SoundCardChange", 1f, 1f);
		}
		else if (hoveredRect == this.button_Right)
		{
			this.cardNavigationIndex++;
			if (this.cardNavigationIndex >= num)
			{
				this.cardNavigationIndex = 0;
			}
			Sound.Play("SoundCardChange", 1f, 1f);
		}
		else if (hoveredRect == this.button_Action)
		{
			CardScript cardScript = this._cardsList[this.cardNavigationIndex];
			if (uiKind != DeckBoxUI.UiKind.pickCardForTheRun)
			{
				if (uiKind == DeckBoxUI.UiKind.seeCollection)
				{
					flag = true;
				}
			}
			else if (Data.game.RunModifier_WonTimes_Get(cardScript.identifier) > 0)
			{
				this.OpenDifficultySelectionMenu();
			}
			else
			{
				flag = this.PickCard();
			}
		}
		return flag;
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x0003DED0 File Offset: 0x0003C0D0
	private bool PickCard()
	{
		CardScript cardScript = this._cardsList[this.cardNavigationIndex];
		bool flag = cardScript.identifier == RunModifierScript.Identifier.defaultModifier || Data.game.RunModifier_OwnedCount_Get(cardScript.identifier) > 0;
		if (cardScript != null && !cardScript.IsFaceDown() && flag)
		{
			GameplayData.RunModifier_SetCurrent(cardScript.identifier, true);
			Sound.Play("SoundCardSelectSummon", 1f, 1f);
			FlashScreen.SpawnCamera(Colors.GetColor("blood red"), 2f, 2f, CameraUiGlobal.instance.myCamera, 0.5f);
			return true;
		}
		Sound.Play("SoundMenuError", 1f, 1f);
		CameraGame.Shake(1f);
		return false;
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x0003DF94 File Offset: 0x0003C194
	private void OpenDifficultySelectionMenu()
	{
		ScreenMenuScript.OptionEvent[] array = new ScreenMenuScript.OptionEvent[3];
		ScreenMenuScript.OptionEvent[] array2 = array;
		int num = 0;
		array2[num] = (ScreenMenuScript.OptionEvent)Delegate.Combine(array2[num], new ScreenMenuScript.OptionEvent(this.SelectionMenu_PickNormal));
		ScreenMenuScript.OptionEvent[] array3 = array;
		int num2 = 1;
		array3[num2] = (ScreenMenuScript.OptionEvent)Delegate.Combine(array3[num2], new ScreenMenuScript.OptionEvent(this.SelectionMenu_PickHardcore));
		ScreenMenuScript.OptionEvent[] array4 = array;
		int num3 = 2;
		array4[num3] = (ScreenMenuScript.OptionEvent)Delegate.Combine(array4[num3], new ScreenMenuScript.OptionEvent(this.SelectionMenu_Back));
		ScreenMenuScript.Open(false, true, 2, ScreenMenuScript.Positioning.centerTopALittlle, 0f, Translation.Get("SCREEN_MENU_CARD_DIFFICULTY_TITLE"), new string[]
		{
			Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_CARD_OPTION_NORMAL"), Strings.SanitizationSubKind.none),
			Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_CARD_OPTION_HARDCORE"), Strings.SanitizationSubKind.none),
			Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_CARD_OPTION_CANCEL"), Strings.SanitizationSubKind.none)
		}, array);
		Sound.Play_Unpausable("SoundMenuPopUp", 1f, 1f);
		ScreenMenuScript.instance.BackAlphaSet(0.75f);
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x0003E088 File Offset: 0x0003C288
	private void SelectionMenu_PickNormal()
	{
		CardScript cardScript = this._cardsList[this.cardNavigationIndex];
		Data.game.RunModifier_HardcoreMode_Set(cardScript.identifier, false);
		if (this.PickCard())
		{
			this.menuSelection_ExitTime = true;
		}
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x0003E0C8 File Offset: 0x0003C2C8
	private void SelectionMenu_PickHardcore()
	{
		CardScript cardScript = this._cardsList[this.cardNavigationIndex];
		Data.game.RunModifier_HardcoreMode_Set(cardScript.identifier, true);
		if (this.PickCard())
		{
			this.menuSelection_ExitTime = true;
		}
		DeckBoxScript.CandlesStateUpdate(false);
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x0003E10D File Offset: 0x0003C30D
	private void SelectionMenu_Back()
	{
		Sound.Play_Unpausable("SoundCardChange", 1f, 1f);
		this.menuSelection_WaitFrame = true;
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x0003E12C File Offset: 0x0003C32C
	private void _TextUpdate(DeckBoxUI.UiKind uiKind)
	{
		if (uiKind != DeckBoxUI.UiKind.pickCardForTheRun)
		{
			if (uiKind == DeckBoxUI.UiKind.seeCollection)
			{
				this.textInstructions.text = Translation.Get("DECKBOX_INSTRUCTIONS_COLLECTION");
				this.textActionLabel.text = Translation.Get("DECKBOX_BUTTON_BACK");
			}
		}
		else
		{
			this.textInstructions.text = Translation.Get("DECKBOX_INSTRUCTIONS_PICK_CARD");
			this.textActionLabel.text = Translation.Get("DECKBOX_BUTTON_PICK");
		}
		int num = 20;
		this._sb.Clear();
		this._sb.Append(Translation.Get("CARDS_CLEARED"));
		this._sb.Append(" ");
		this._sb.Append(Data.game.RunModifier_WonOnce_TotalNumber());
		this._sb.Append("/");
		this._sb.Append(num);
		this._sb.Append("\n");
		this._sb.Append(Translation.Get("CARDS_HOLO_OBTAINED"));
		this._sb.Append(" ");
		this._sb.Append(Data.game.RunModifier_InHolographicCondition_TotalNumber());
		this._sb.Append("/");
		this._sb.Append(num);
		this.textCompletedCards.text = this._sb.ToString();
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x0003E284 File Offset: 0x0003C484
	private IEnumerator FlashInstructionsText()
	{
		float timer = 2f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			this.textInstructions.color = ((Util.AngleSin(timer * 1440f) > 0f) ? Color.white : DeckBoxUI.C_ORANGE);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0003E293 File Offset: 0x0003C493
	public static void ForceClose_Death()
	{
		if (DeckBoxUI.instance == null)
		{
			return;
		}
		DeckBoxUI.instance.forceClose_Death = true;
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x0003E2AE File Offset: 0x0003C4AE
	public static bool IsForceClosing()
	{
		return !(DeckBoxUI.instance == null) && DeckBoxUI.instance.forceClose_Death;
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x0003E2CC File Offset: 0x0003C4CC
	private void Awake()
	{
		DeckBoxUI.instance = this;
		this.imagesStartPositions = new Vector2[this.imagesToShake.Length];
		for (int i = 0; i < this.imagesStartPositions.Length; i++)
		{
			this.imagesStartPositions[i] = this.imagesToShake[i].rectTransform.anchoredPosition;
		}
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0003E323 File Offset: 0x0003C523
	private void OnDestroy()
	{
		if (DeckBoxUI.instance == this)
		{
			DeckBoxUI.instance = null;
		}
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0003E338 File Offset: 0x0003C538
	private void Start()
	{
		DeckBoxUI.instance.holder.SetActive(false);
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x0003E34C File Offset: 0x0003C54C
	private void Update()
	{
		bool flag = DeckBoxUI.IsEnabled();
		for (int i = 0; i < this.imagesStartPositions.Length; i++)
		{
			Vector2 vector = this.imagesStartPositions[i];
			Vector2 vector2 = vector + new Vector2(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f));
			if (Data.settings.dyslexicFontEnabled)
			{
				vector2 = vector;
			}
			this.imagesToShake[i].rectTransform.anchoredPosition = vector2;
		}
		if (flag)
		{
			for (int j = 0; j < this._cardsList.Count; j++)
			{
				CardScript cardScript = this._cardsList[j];
				bool flag2 = j == this.cardNavigationIndex;
				Vector2 vector3 = cardScript.rectTransform.anchoredPosition;
				Vector2 vector4 = vector3;
				vector4.x = (float)j * 128f - 128f * (float)this.cardNavigationIndex;
				if (ScreenMenuScript.IsEnabled())
				{
					if (flag2)
					{
						vector4.x -= 256f;
					}
					else
					{
						vector4.x *= 10f;
					}
				}
				vector3 = Vector2.Lerp(vector3, vector4, Tick.Time * 10f);
				cardScript.rectTransform.anchoredPosition = vector3;
				Vector3 vector5 = Vector3.one * 300f;
				if (flag2)
				{
					vector5 = Vector3.one * 450f;
				}
				cardScript.rectTransform.localScale = Vector3.Lerp(cardScript.rectTransform.localScale, vector5, Tick.Time * 10f);
				Vector3 zero = Vector3.zero;
				Vector3 vector6 = cardScript.transform.localEulerAngles;
				vector6.z = 0f;
				if (flag2)
				{
					zero.y = Util.AngleSin(Tick.PassedTime * 90f) * 15f;
					zero.x = Util.AngleSin(Tick.PassedTime * 90f + 90f) * 10f;
				}
				if (vector6.x > zero.x + 180f)
				{
					vector6.x -= 360f;
				}
				if (vector6.x < zero.x - 180f)
				{
					vector6.x += 360f;
				}
				if (vector6.y > zero.y + 180f)
				{
					vector6.y -= 360f;
				}
				if (vector6.y < zero.y - 180f)
				{
					vector6.y += 360f;
				}
				vector6 = Vector3.Lerp(vector6, zero, Tick.Time * 10f);
				cardScript.transform.localEulerAngles = vector6;
				if (flag2 && CardsInspectorScript.InspectedCard_Get() != cardScript)
				{
					CardsInspectorScript.InspectCard_Set(cardScript, cardScript.IsFaceDown());
				}
			}
		}
	}

	public static DeckBoxUI instance;

	private const int PLAYER_INDEX = 0;

	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	private static Color C_DARK_GRAY = new Color(0.55f, 0.55f, 0.55f, 1f);

	private const float CARDS_SCALE = 300f;

	private const float CARDS_SCALE_INSPECTED = 450f;

	private const float CARDS_SPACING = 128f;

	private const float CARDS_Y_DEFAULT = 44f;

	public GameObject holder;

	public CanvasScaler canvasScaler;

	public Image[] imagesToShake;

	private Vector2[] imagesStartPositions;

	public RectTransform cardsHolder;

	public RectTransform button_Left;

	public RectTransform button_Action;

	public RectTransform button_Right;

	public Image buttonLeftCursor;

	public Image buttonActionCursor;

	public Image buttonRightCursor;

	public TextMeshProUGUI textInstructions;

	public TextMeshProUGUI textActionLabel;

	public TextMeshProUGUI textCompletedCards;

	private DeckBoxUI.UiKind uiKindOpenedTo;

	private Coroutine uiCoroutine;

	private List<CardScript> _cardsList = new List<CardScript>();

	private CameraController.PositionKind backupCameraPosition = CameraController.PositionKind.Undefined;

	private int cardNavigationIndex;

	private bool menuSelection_ExitTime;

	private bool menuSelection_WaitFrame;

	private StringBuilder _sb = new StringBuilder();

	private Coroutine coroutineFlashInstructionsText;

	private bool forceClose_Death;

	public enum UiKind
	{
		pickCardForTheRun,
		seeCollection
	}
}
