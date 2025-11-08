using System;
using Febucci.UI;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMenuScript : MonoBehaviour
{
	// (get) Token: 0x060009ED RID: 2541 RVA: 0x00043D3D File Offset: 0x00041F3D
	private int OptionsCountMax
	{
		get
		{
			return this.optionsTextArray.Length;
		}
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x00043D47 File Offset: 0x00041F47
	public static bool IsEnabled()
	{
		return ScreenMenuScript.instance.menuHolder.activeSelf;
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x00043D58 File Offset: 0x00041F58
	public static void Close(bool initialSetup)
	{
		if (!ScreenMenuScript.instance.menuHolder.activeSelf)
		{
			return;
		}
		ScreenMenuScript.instance.menuHolder.SetActive(false);
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x00043D7C File Offset: 0x00041F7C
	public static void Open(bool resetCursor, bool closeOnSelect, int cancelOptionIndex, ScreenMenuScript.Positioning positioning, float extraOptionsSpacing, string title, string[] options, params ScreenMenuScript.OptionEvent[] optionEvents)
	{
		if (options.Length > ScreenMenuScript.instance.optionsTextArray.Length)
		{
			Debug.LogError("ScreenMenuScript: Open: options.Length > optionsTextArray.Length");
			return;
		}
		if (options.Length != optionEvents.Length)
		{
			Debug.LogError("ScreenMenuScript: Open: options.Length != optionEvents.Length");
			return;
		}
		if (options.Length < 1)
		{
			Debug.LogError("ScreenMenuScript: Open: options.Length < 1");
			return;
		}
		if (ScreenMenuScript.instance.menuHolder.activeSelf)
		{
			return;
		}
		ScreenMenuScript.instance.closeOnAnyOptionSelect = closeOnSelect;
		ScreenMenuScript.instance.cancelOptionIndex = cancelOptionIndex;
		MemoScript.Close(false);
		ScreenMenuScript.instance.menuHolder.SetActive(true);
		ScreenMenuScript.instance.titleText.text = title;
		ScreenMenuScript.instance.optionsIndex = -1;
		if (ScreenMenuScript.instance.player.lastInputKindUsed != Controls.InputKind.Mouse)
		{
			ScreenMenuScript.instance.optionsIndex = 0;
		}
		ScreenMenuScript.instance.optionsCount = options.Length;
		for (int i = 0; i < ScreenMenuScript.instance.optionsTextArray.Length; i++)
		{
			if (i >= ScreenMenuScript.instance.optionsCount)
			{
				ScreenMenuScript.instance.optionsTextArray[i].text = "";
			}
			else
			{
				TextAnimator component = ScreenMenuScript.instance.optionsTextArray[i].GetComponent<TextAnimator>();
				if (component != null)
				{
					component.SyncText(options[i], false);
				}
				ScreenMenuScript.instance.optionsTextArray[i].alpha = 0.25f;
			}
		}
		if (ScreenMenuScript.instance.optionEventsArray == null)
		{
			ScreenMenuScript.instance.optionEventsArray = new ScreenMenuScript.OptionEvent[ScreenMenuScript.instance.OptionsCountMax];
		}
		for (int j = 0; j < ScreenMenuScript.instance.optionEventsArray.Length; j++)
		{
			if (j >= optionEvents.Length)
			{
				ScreenMenuScript.instance.optionEventsArray[j] = null;
			}
			else
			{
				ScreenMenuScript.instance.optionEventsArray[j] = null;
				ScreenMenuScript.OptionEvent[] array = ScreenMenuScript.instance.optionEventsArray;
				int num = j;
				array[num] = (ScreenMenuScript.OptionEvent)Delegate.Combine(array[num], optionEvents[j]);
			}
		}
		ScreenMenuScript.instance.backShadowImage.enabled = true;
		ScreenMenuScript.instance.hideWhenCameraIsMoving = false;
		ScreenMenuScript.AudioClipsReset();
		float optionsSpacing = ScreenMenuScript.GetOptionsSpacing(extraOptionsSpacing);
		for (int k = 0; k < ScreenMenuScript.instance.optionsTextArray.Length; k++)
		{
			if (k == 0)
			{
				ScreenMenuScript.instance.optionsTextArray[0].rectTransform.anchoredPosition = new Vector2(ScreenMenuScript.instance.optionsTextArray[0].rectTransform.anchoredPosition.x, -50f - extraOptionsSpacing);
			}
			else
			{
				ScreenMenuScript.instance.optionsTextArray[k].rectTransform.anchoredPosition = ScreenMenuScript.instance.optionsTextArray[0].rectTransform.anchoredPosition - new Vector2(0f, optionsSpacing) * (float)k;
			}
		}
		float num2 = 60f + optionsSpacing * (float)options.Length;
		float num3 = ScreenMenuScript.instance.titleText.preferredWidth + 40f;
		for (int l = 0; l < options.Length; l++)
		{
			float num4 = ScreenMenuScript.instance.optionsTextArray[l].preferredWidth + 40f;
			num3 = Mathf.Max(num3, num4);
		}
		ScreenMenuScript.instance.backImage.rectTransform.sizeDelta = new Vector2(num3, num2);
		ScreenMenuScript.SetPositioning(positioning, new Vector2(num3, num2), resetCursor);
		ScreenMenuScript.instance.inputDelay = 0.5f;
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x000440B2 File Offset: 0x000422B2
	private static float GetOptionsSpacing(float extraSpacing)
	{
		return 27.5f + extraSpacing;
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x000440BC File Offset: 0x000422BC
	public static void SetPositioning(ScreenMenuScript.Positioning positioning, Vector2 imageSize, bool resetCursor)
	{
		if (imageSize == Vector2.zero)
		{
			imageSize = ScreenMenuScript.instance.backImage.rectTransform.sizeDelta;
		}
		switch (positioning)
		{
		case ScreenMenuScript.Positioning.center:
			ScreenMenuScript.instance.positionShifter.anchoredPosition = new Vector2(0f, imageSize.y / 2f);
			if (resetCursor)
			{
				VirtualCursors.CursorPositionNormalizedSet(0, new Vector2(0f, 0f), true);
				return;
			}
			break;
		case ScreenMenuScript.Positioning.centerDownLittle:
			ScreenMenuScript.instance.positionShifter.anchoredPosition = new Vector2(0f, imageSize.y / 2.5f);
			if (resetCursor)
			{
				VirtualCursors.CursorPositionNormalizedSet(0, new Vector2(0f, 0f), true);
				return;
			}
			break;
		case ScreenMenuScript.Positioning.down:
		{
			Vector2 vector = new Vector2(0f, -20f - imageSize.y / 2f);
			ScreenMenuScript.instance.positionShifter.anchoredPosition = vector;
			if (resetCursor)
			{
				VirtualCursors.CursorPositionNormalizedSet(0, new Vector2(0f, -0.2f), true);
				return;
			}
			break;
		}
		case ScreenMenuScript.Positioning.downDown:
		{
			Vector2 vector2 = new Vector2(0f, -80f - imageSize.y / 2f);
			ScreenMenuScript.instance.positionShifter.anchoredPosition = vector2;
			if (resetCursor)
			{
				VirtualCursors.CursorPositionNormalizedSet(0, new Vector2(0f, -0.3f), true);
				return;
			}
			break;
		}
		default:
			Debug.LogError("ScreenMenuScript: SetPositioning: position not handled: " + positioning.ToString());
			break;
		}
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x00044237 File Offset: 0x00042437
	public static void HideShadow()
	{
		if (ScreenMenuScript.instance == null)
		{
			return;
		}
		ScreenMenuScript.instance.backShadowImage.enabled = false;
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x00044257 File Offset: 0x00042457
	public static void HideWhenCameraIsMoving(bool instantHide)
	{
		if (ScreenMenuScript.instance == null)
		{
			return;
		}
		ScreenMenuScript.instance.hideWhenCameraIsMoving = true;
		if (instantHide)
		{
			ScreenMenuScript.instance.holderHider.SetActive(false);
		}
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x00044285 File Offset: 0x00042485
	public static void AudioClipsSet(string onShowFromHidden, string onHover, string onSelect, string onBack)
	{
		ScreenMenuScript.instance.audio_OnShowFromHidden = onShowFromHidden;
		ScreenMenuScript.instance.audio_OnHover = onHover;
		ScreenMenuScript.instance.audio_OnSelect = onSelect;
		ScreenMenuScript.instance.audio_OnBack = onBack;
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x000442B3 File Offset: 0x000424B3
	public static void AudioClipsReset()
	{
		ScreenMenuScript.AudioClipsSet(null, "SoundMenuSelectionChange", "SoundMenuSelect", "SoundMenuBack");
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x000442CC File Offset: 0x000424CC
	private void _SelectOption(int index)
	{
		ScreenMenuScript.OptionEvent optionEvent = this.optionEventsArray[index];
		if (optionEvent != null)
		{
			optionEvent();
		}
		if (this.cancelOptionIndex == index)
		{
			if (this.audio_OnBack != null)
			{
				Sound.Play(this.audio_OnBack, 1f, 1f);
			}
		}
		else if (this.audio_OnSelect != null)
		{
			Sound.Play(this.audio_OnSelect, 1f, 1f);
		}
		Controls.VibrationSet_PreferMax(this.player, 0.25f);
		if (this.closeOnAnyOptionSelect)
		{
			ScreenMenuScript.Close(false);
		}
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x00044351 File Offset: 0x00042551
	public static void ForceClose_Death()
	{
		if (ScreenMenuScript.instance == null)
		{
			return;
		}
		if (ScreenMenuScript.instance.cancelOptionIndex < 0)
		{
			ScreenMenuScript.Close(false);
			return;
		}
		ScreenMenuScript.instance._SelectOption(ScreenMenuScript.instance.cancelOptionIndex);
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x00044389 File Offset: 0x00042589
	private void Awake()
	{
		ScreenMenuScript.instance = this;
		this.canvasScaler = base.GetComponentInParent<CanvasScaler>();
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x0004439D File Offset: 0x0004259D
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		ScreenMenuScript.Close(true);
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x000443B1 File Offset: 0x000425B1
	private void OnDestroy()
	{
		if (ScreenMenuScript.instance == this)
		{
			ScreenMenuScript.instance = null;
		}
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x000443C8 File Offset: 0x000425C8
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		if (!this.menuHolder.activeSelf)
		{
			return;
		}
		bool flag = true;
		if (this.hideWhenCameraIsMoving && !CameraController.IsCameraNearPositionAndAngle(0.05f))
		{
			flag = false;
		}
		if (this.holderHider.activeSelf != flag)
		{
			if (flag && this.audio_OnShowFromHidden != null)
			{
				Sound.Play(this.audio_OnShowFromHidden, 1f, 1f);
			}
			this.holderHider.SetActive(flag);
		}
		if (!flag)
		{
			return;
		}
		if (DialogueScript.IsEnabled())
		{
			this.inputDelay = 0.5f;
		}
		if (PowerupTriggerAnimController.HasAnimations() && !DrawersScript.IsAnyDrawerOpened())
		{
			this.inputDelay = 0.5f;
		}
		this.inputDelay -= Tick.Time;
		if (this.inputDelay > 0f)
		{
			return;
		}
		if (VirtualCursors.IsCursorVisible(0, true))
		{
			Vector2 vector = new Vector2(this.canvasScaler.referenceResolution.x, this.canvasScaler.referenceResolution.y);
			Vector2 vector2 = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, vector);
			bool flag2 = Controls.MouseButton_PressedGet(0, Controls.MouseElement.LeftButton);
			bool flag3 = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true);
			this.optionsIndex = -1;
			for (int i = 0; i < this.optionsCount; i++)
			{
				RectTransform rectTransform = this.optionsTextArray[i].rectTransform;
				Vector2 vector3 = this.positionShifter.anchoredPosition + rectTransform.anchoredPosition + new Vector2(-rectTransform.sizeDelta.x / 2f, 0f);
				Vector2 vector4 = this.positionShifter.anchoredPosition + rectTransform.anchoredPosition + new Vector2(rectTransform.sizeDelta.x / 2f, -rectTransform.sizeDelta.y);
				if (vector2.x < vector4.x && vector2.x > vector3.x && vector2.y > vector4.y && vector2.y < vector3.y)
				{
					this.optionsIndex = i;
					break;
				}
			}
			if (this.optionsIndex != -1 && flag2)
			{
				this._SelectOption(this.optionsIndex);
			}
			if (this.cancelOptionIndex >= 0 && (flag3 || (flag2 && this.optionsIndex < 0)))
			{
				this._SelectOption(this.cancelOptionIndex);
			}
		}
		else
		{
			float num = 0f;
			if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuMoveUp, true))
			{
				num += 1f;
			}
			if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuMoveDown, true))
			{
				num -= 1f;
			}
			bool flag4 = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true);
			bool flag5 = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true);
			if (this.optionsIndex < 0)
			{
				if (num != 0f || flag4)
				{
					this.optionsIndex = 0;
				}
			}
			else
			{
				if (num > 0f)
				{
					this.optionsIndex--;
					if (this.optionsIndex < 0)
					{
						this.optionsIndex = 0;
					}
				}
				else if (num < 0f)
				{
					this.optionsIndex++;
					if (this.optionsIndex > this.optionsCount - 1)
					{
						this.optionsIndex = this.optionsCount - 1;
					}
				}
				if (flag4)
				{
					this._SelectOption(this.optionsIndex);
				}
			}
			if (this.cancelOptionIndex >= 0 && flag5)
			{
				this._SelectOption(this.cancelOptionIndex);
			}
		}
		if (this.optionsIndexOld != this.optionsIndex)
		{
			this.optionsIndexOld = this.optionsIndex;
			if (this.optionsIndex != -1 && this.audio_OnHover != null)
			{
				Sound.Play(this.audio_OnHover, 1f, 1f);
			}
			Controls.VibrationSet_PreferMax(this.player, 0.1f);
		}
		for (int j = 0; j < this.optionsTextArray.Length; j++)
		{
			if (j == this.optionsIndex)
			{
				this.optionsTextArray[j].alpha = 1f;
			}
			else
			{
				this.optionsTextArray[j].alpha = 0.25f;
			}
		}
		Vector2 zero = new Vector2(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f));
		if (Data.settings.dyslexicFontEnabled)
		{
			zero = Vector2.zero;
		}
		this.backImage.rectTransform.anchoredPosition = zero;
	}

	public static ScreenMenuScript instance;

	public const int PLAYER_INDEX = 0;

	public const float INPUT_DELAY = 0.5f;

	public const float SELECTED_ALPHA = 1f;

	public const float UNSELECTED_ALPHA = 0.25f;

	private Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	private CanvasScaler canvasScaler;

	private Controls.PlayerExt player;

	public GameObject menuHolder;

	public GameObject holderHider;

	public RectTransform positionShifter;

	public Image backShadowImage;

	public Image backImage;

	public TextMeshProUGUI titleText;

	public TextMeshProUGUI[] optionsTextArray;

	private int optionsCount = -1;

	private int optionsIndex = -1;

	private int optionsIndexOld = -1;

	private bool closeOnAnyOptionSelect;

	private float inputDelay;

	private int cancelOptionIndex = -1;

	private bool hideWhenCameraIsMoving;

	private string audio_OnShowFromHidden;

	private string audio_OnHover = "SoundMenuSelectionChange";

	private string audio_OnSelect = "SoundMenuSelect";

	private string audio_OnBack = "SoundMenuBack";

	public ScreenMenuScript.OptionEvent[] optionEventsArray;

	public enum Positioning
	{
		center,
		centerDownLittle,
		down,
		downDown
	}

	// (Invoke) Token: 0x06001259 RID: 4697
	public delegate void OptionEvent();
}
