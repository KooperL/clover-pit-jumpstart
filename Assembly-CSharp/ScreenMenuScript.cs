using System;
using Febucci.UI;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMenuScript : MonoBehaviour
{
	// (get) Token: 0x06000A01 RID: 2561 RVA: 0x000443A5 File Offset: 0x000425A5
	private int OptionsCountMax
	{
		get
		{
			return this.optionsTextArray.Length;
		}
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x000443AF File Offset: 0x000425AF
	public static bool IsEnabled()
	{
		return ScreenMenuScript.instance.menuHolder.activeSelf;
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x000443C0 File Offset: 0x000425C0
	public static void Close(bool initialSetup)
	{
		if (!ScreenMenuScript.instance.menuHolder.activeSelf)
		{
			return;
		}
		ScreenMenuScript.instance.menuHolder.SetActive(false);
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x000443E4 File Offset: 0x000425E4
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
		ScreenMenuScript.instance.BackAlphaSet(0.5f);
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

	// Token: 0x06000A05 RID: 2565 RVA: 0x00044729 File Offset: 0x00042929
	private static float GetOptionsSpacing(float extraSpacing)
	{
		return 27.5f + extraSpacing;
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x00044734 File Offset: 0x00042934
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
		case ScreenMenuScript.Positioning.centerTopALittlle:
			ScreenMenuScript.instance.positionShifter.anchoredPosition = new Vector2(0f, 40f + imageSize.y / 2f);
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

	// Token: 0x06000A07 RID: 2567 RVA: 0x000448FB File Offset: 0x00042AFB
	public static void HideShadow()
	{
		if (ScreenMenuScript.instance == null)
		{
			return;
		}
		ScreenMenuScript.instance.backShadowImage.enabled = false;
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x0004491B File Offset: 0x00042B1B
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

	// Token: 0x06000A09 RID: 2569 RVA: 0x00044949 File Offset: 0x00042B49
	public static void AudioClipsSet(string onShowFromHidden, string onHover, string onSelect, string onBack)
	{
		ScreenMenuScript.instance.audio_OnShowFromHidden = onShowFromHidden;
		ScreenMenuScript.instance.audio_OnHover = onHover;
		ScreenMenuScript.instance.audio_OnSelect = onSelect;
		ScreenMenuScript.instance.audio_OnBack = onBack;
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x00044977 File Offset: 0x00042B77
	public static void AudioClipsReset()
	{
		ScreenMenuScript.AudioClipsSet(null, "SoundMenuSelectionChange", "SoundMenuSelect", "SoundMenuBack");
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x00044990 File Offset: 0x00042B90
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

	// Token: 0x06000A0C RID: 2572 RVA: 0x00044A18 File Offset: 0x00042C18
	public void BackAlphaSet(float a)
	{
		Color color = ScreenMenuScript.instance.backShadowImage.color;
		color.a = a;
		ScreenMenuScript.instance.backShadowImage.color = color;
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x00044A4D File Offset: 0x00042C4D
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

	// Token: 0x06000A0E RID: 2574 RVA: 0x00044A85 File Offset: 0x00042C85
	private void Awake()
	{
		ScreenMenuScript.instance = this;
		this.canvasScaler = base.GetComponentInParent<CanvasScaler>();
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x00044A99 File Offset: 0x00042C99
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		ScreenMenuScript.Close(true);
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x00044AAD File Offset: 0x00042CAD
	private void OnDestroy()
	{
		if (ScreenMenuScript.instance == this)
		{
			ScreenMenuScript.instance = null;
		}
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x00044AC4 File Offset: 0x00042CC4
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
		centerTopALittlle,
		down,
		downDown
	}

	// (Invoke) Token: 0x06001270 RID: 4720
	public delegate void OptionEvent();
}
