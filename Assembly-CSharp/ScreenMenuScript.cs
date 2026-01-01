using System;
using Febucci.UI;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E8 RID: 232
public class ScreenMenuScript : MonoBehaviour
{
	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x0000F933 File Offset: 0x0000DB33
	private int OptionsCountMax
	{
		get
		{
			return this.optionsTextArray.Length;
		}
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x0000F93D File Offset: 0x0000DB3D
	public static bool IsEnabled()
	{
		return ScreenMenuScript.instance.menuHolder.activeSelf;
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x0000F94E File Offset: 0x0000DB4E
	public static void Close(bool initialSetup)
	{
		if (!ScreenMenuScript.instance.menuHolder.activeSelf)
		{
			return;
		}
		ScreenMenuScript.instance.menuHolder.SetActive(false);
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x0005DD14 File Offset: 0x0005BF14
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

	// Token: 0x06000BAD RID: 2989 RVA: 0x0000F972 File Offset: 0x0000DB72
	private static float GetOptionsSpacing(float extraSpacing)
	{
		return 27.5f + extraSpacing;
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x0005E05C File Offset: 0x0005C25C
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

	// Token: 0x06000BAF RID: 2991 RVA: 0x0000F97B File Offset: 0x0000DB7B
	public static void HideShadow()
	{
		if (ScreenMenuScript.instance == null)
		{
			return;
		}
		ScreenMenuScript.instance.backShadowImage.enabled = false;
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x0000F99B File Offset: 0x0000DB9B
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

	// Token: 0x06000BB1 RID: 2993 RVA: 0x0000F9C9 File Offset: 0x0000DBC9
	public static void AudioClipsSet(string onShowFromHidden, string onHover, string onSelect, string onBack)
	{
		ScreenMenuScript.instance.audio_OnShowFromHidden = onShowFromHidden;
		ScreenMenuScript.instance.audio_OnHover = onHover;
		ScreenMenuScript.instance.audio_OnSelect = onSelect;
		ScreenMenuScript.instance.audio_OnBack = onBack;
	}

	// Token: 0x06000BB2 RID: 2994 RVA: 0x0000F9F7 File Offset: 0x0000DBF7
	public static void AudioClipsReset()
	{
		ScreenMenuScript.AudioClipsSet(null, "SoundMenuSelectionChange", "SoundMenuSelect", "SoundMenuBack");
	}

	// Token: 0x06000BB3 RID: 2995 RVA: 0x0005E224 File Offset: 0x0005C424
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

	// Token: 0x06000BB4 RID: 2996 RVA: 0x0005E2AC File Offset: 0x0005C4AC
	public void BackAlphaSet(float a)
	{
		Color color = ScreenMenuScript.instance.backShadowImage.color;
		color.a = a;
		ScreenMenuScript.instance.backShadowImage.color = color;
	}

	// Token: 0x06000BB5 RID: 2997 RVA: 0x0000FA0E File Offset: 0x0000DC0E
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

	// Token: 0x06000BB6 RID: 2998 RVA: 0x0000FA46 File Offset: 0x0000DC46
	private void Awake()
	{
		ScreenMenuScript.instance = this;
		this.canvasScaler = base.GetComponentInParent<CanvasScaler>();
	}

	// Token: 0x06000BB7 RID: 2999 RVA: 0x0000FA5A File Offset: 0x0000DC5A
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		ScreenMenuScript.Close(true);
	}

	// Token: 0x06000BB8 RID: 3000 RVA: 0x0000FA6E File Offset: 0x0000DC6E
	private void OnDestroy()
	{
		if (ScreenMenuScript.instance == this)
		{
			ScreenMenuScript.instance = null;
		}
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x0005E2E4 File Offset: 0x0005C4E4
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

	// Token: 0x04000C48 RID: 3144
	public static ScreenMenuScript instance;

	// Token: 0x04000C49 RID: 3145
	public const int PLAYER_INDEX = 0;

	// Token: 0x04000C4A RID: 3146
	public const float INPUT_DELAY = 0.5f;

	// Token: 0x04000C4B RID: 3147
	public const float SELECTED_ALPHA = 1f;

	// Token: 0x04000C4C RID: 3148
	public const float UNSELECTED_ALPHA = 0.25f;

	// Token: 0x04000C4D RID: 3149
	private Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x04000C4E RID: 3150
	private CanvasScaler canvasScaler;

	// Token: 0x04000C4F RID: 3151
	private Controls.PlayerExt player;

	// Token: 0x04000C50 RID: 3152
	public GameObject menuHolder;

	// Token: 0x04000C51 RID: 3153
	public GameObject holderHider;

	// Token: 0x04000C52 RID: 3154
	public RectTransform positionShifter;

	// Token: 0x04000C53 RID: 3155
	public Image backShadowImage;

	// Token: 0x04000C54 RID: 3156
	public Image backImage;

	// Token: 0x04000C55 RID: 3157
	public TextMeshProUGUI titleText;

	// Token: 0x04000C56 RID: 3158
	public TextMeshProUGUI[] optionsTextArray;

	// Token: 0x04000C57 RID: 3159
	private int optionsCount = -1;

	// Token: 0x04000C58 RID: 3160
	private int optionsIndex = -1;

	// Token: 0x04000C59 RID: 3161
	private int optionsIndexOld = -1;

	// Token: 0x04000C5A RID: 3162
	private bool closeOnAnyOptionSelect;

	// Token: 0x04000C5B RID: 3163
	private float inputDelay;

	// Token: 0x04000C5C RID: 3164
	private int cancelOptionIndex = -1;

	// Token: 0x04000C5D RID: 3165
	private bool hideWhenCameraIsMoving;

	// Token: 0x04000C5E RID: 3166
	private string audio_OnShowFromHidden;

	// Token: 0x04000C5F RID: 3167
	private string audio_OnHover = "SoundMenuSelectionChange";

	// Token: 0x04000C60 RID: 3168
	private string audio_OnSelect = "SoundMenuSelect";

	// Token: 0x04000C61 RID: 3169
	private string audio_OnBack = "SoundMenuBack";

	// Token: 0x04000C62 RID: 3170
	public ScreenMenuScript.OptionEvent[] optionEventsArray;

	// Token: 0x020000E9 RID: 233
	public enum Positioning
	{
		// Token: 0x04000C64 RID: 3172
		center,
		// Token: 0x04000C65 RID: 3173
		centerDownLittle,
		// Token: 0x04000C66 RID: 3174
		centerTopALittlle,
		// Token: 0x04000C67 RID: 3175
		down,
		// Token: 0x04000C68 RID: 3176
		downDown
	}

	// Token: 0x020000EA RID: 234
	// (Invoke) Token: 0x06000BBC RID: 3004
	public delegate void OptionEvent();
}
