using System;
using System.Collections;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeckBoxUI : MonoBehaviour
{
	// Token: 0x0600094D RID: 2381 RVA: 0x0003D8BE File Offset: 0x0003BABE
	public static bool IsEnabled()
	{
		return !(DeckBoxUI.instance == null) && DeckBoxUI.instance.holder.activeSelf;
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x0003D8DE File Offset: 0x0003BADE
	public static bool IsPickingCard(bool considerEnabledState)
	{
		return !(DeckBoxUI.instance == null) && DeckBoxUI.IsEnabled() && DeckBoxUI.instance.uiKindOpenedTo == DeckBoxUI.UiKind.pickCardForTheRun;
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0003D908 File Offset: 0x0003BB08
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
		Sound.Play3D("SoundDeckBoxOpen", DeckBoxUI.instance.transform.position, 20f, 1f, 1f, AudioRolloffMode.Linear);
		DeckBoxUI.instance.uiCoroutine = DeckBoxUI.instance.StartCoroutine(DeckBoxUI.instance.UiCoroutine(uiKind));
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0003D9C8 File Offset: 0x0003BBC8
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
		Sound.Play3D("SoundDeckBoxClose", DeckBoxUI.instance.transform.position, 20f, 1f, 1f, AudioRolloffMode.Linear);
		if (DeckBoxUI.instance.backupCameraPosition != CameraController.PositionKind.Undefined)
		{
			CameraController.SetPosition(DeckBoxUI.instance.backupCameraPosition, false, 1f);
		}
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0003DA60 File Offset: 0x0003BC60
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
				CardScript cardScript3 = null;
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
								cardScript3 = this._cardsList[m];
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
				if (cardScript3 != null && flag2 && !flag3)
				{
					for (int n = 0; n < this._cardsList.Count; n++)
					{
						if (this._cardsList[n] == cardScript3)
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
				yield return null;
			}
		}
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

	// Token: 0x06000952 RID: 2386 RVA: 0x0003DA78 File Offset: 0x0003BC78
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
			else
			{
				bool flag2 = cardScript.identifier == RunModifierScript.Identifier.defaultModifier || Data.game.RunModifier_OwnedCount_Get(cardScript.identifier) > 0;
				if (cardScript != null && !cardScript.IsFaceDown() && flag2)
				{
					GameplayData.RunModifier_SetCurrent(cardScript.identifier, true);
					Sound.Play("SoundCardSelectSummon", 1f, 1f);
					FlashScreen.SpawnCamera(Colors.GetColor("blood red"), 2f, 2f, CameraUiGlobal.instance.myCamera, 0.5f);
					flag = true;
				}
				else
				{
					Sound.Play("SoundMenuError", 1f, 1f);
					CameraGame.Shake(1f);
				}
			}
		}
		return flag;
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x0003DBF4 File Offset: 0x0003BDF4
	private void _TextUpdate(DeckBoxUI.UiKind uiKind)
	{
		if (uiKind == DeckBoxUI.UiKind.pickCardForTheRun)
		{
			this.textInstructions.text = Translation.Get("DECKBOX_INSTRUCTIONS_PICK_CARD");
			this.textActionLabel.text = Translation.Get("DECKBOX_BUTTON_PICK");
			return;
		}
		if (uiKind != DeckBoxUI.UiKind.seeCollection)
		{
			return;
		}
		this.textInstructions.text = Translation.Get("DECKBOX_INSTRUCTIONS_COLLECTION");
		this.textActionLabel.text = Translation.Get("DECKBOX_BUTTON_BACK");
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x0003DC5E File Offset: 0x0003BE5E
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

	// Token: 0x06000955 RID: 2389 RVA: 0x0003DC6D File Offset: 0x0003BE6D
	public static void ForceClose_Death()
	{
		if (DeckBoxUI.instance == null)
		{
			return;
		}
		DeckBoxUI.instance.forceClose_Death = true;
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x0003DC88 File Offset: 0x0003BE88
	public static bool IsForceClosing()
	{
		return !(DeckBoxUI.instance == null) && DeckBoxUI.instance.forceClose_Death;
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x0003DCA4 File Offset: 0x0003BEA4
	private void Awake()
	{
		DeckBoxUI.instance = this;
		this.imagesStartPositions = new Vector2[this.imagesToShake.Length];
		for (int i = 0; i < this.imagesStartPositions.Length; i++)
		{
			this.imagesStartPositions[i] = this.imagesToShake[i].rectTransform.anchoredPosition;
		}
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0003DCFB File Offset: 0x0003BEFB
	private void OnDestroy()
	{
		if (DeckBoxUI.instance == this)
		{
			DeckBoxUI.instance = null;
		}
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x0003DD10 File Offset: 0x0003BF10
	private void Start()
	{
		DeckBoxUI.instance.holder.SetActive(false);
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x0003DD24 File Offset: 0x0003BF24
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

	private DeckBoxUI.UiKind uiKindOpenedTo;

	private Coroutine uiCoroutine;

	private List<CardScript> _cardsList = new List<CardScript>();

	private CameraController.PositionKind backupCameraPosition = CameraController.PositionKind.Undefined;

	private int cardNavigationIndex;

	private Coroutine coroutineFlashInstructionsText;

	private bool forceClose_Death;

	public enum UiKind
	{
		pickCardForTheRun,
		seeCollection
	}
}
