using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenCallToAction : MonoBehaviour
{
	// Token: 0x060008C9 RID: 2249 RVA: 0x0003A112 File Offset: 0x00038312
	public static bool IsEnabled()
	{
		return !(LoadingScreenCallToAction.instance == null) && LoadingScreenCallToAction.instance.holder.activeSelf;
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x0003A132 File Offset: 0x00038332
	private static bool IsBooked()
	{
		return LoadingScreenCallToAction.booked;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x0003A139 File Offset: 0x00038339
	public static void BookCallToAction()
	{
		LoadingScreenCallToAction.booked = true;
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0003A141 File Offset: 0x00038341
	public static bool LoadingShouldWait()
	{
		return !(LoadingScreenCallToAction.instance == null) && (LoadingScreenCallToAction.IsBooked() || LoadingScreenCallToAction.IsEnabled());
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x0003A160 File Offset: 0x00038360
	private void TranslationUpdate()
	{
		this.textSkip.text = Translation.Get("MENU_OPTION_SKIP");
		this.textInfos.text = string.Concat(new string[]
		{
			Translation.Get("LOADING_NOTIFICATION_FULL_VERSION_FEATURES_0"),
			"\n\n",
			Translation.Get("LOADING_NOTIFICATION_FULL_VERSION_FEATURES_1"),
			"\n\n",
			Translation.Get("LOADING_NOTIFICATION_FULL_VERSION_FEATURES_2"),
			"\n\n",
			Translation.Get("LOADING_NOTIFICATION_FULL_VERSION_FEATURES_3")
		});
		this.textTitle.text = ((Master.instance.GAME_PUBLIC_STATE == Master.GamePublicState.released) ? Translation.Get("LOADING_NOTIFICATION_CALL_TO_ACTION_BUY") : Translation.Get("LOADING_NOTIFICATION_CALL_TO_ACTION_WISHLIST"));
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x0003A212 File Offset: 0x00038412
	private void Awake()
	{
		LoadingScreenCallToAction.instance = this;
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x0003A21C File Offset: 0x0003841C
	private void Start()
	{
		this.TranslationUpdate();
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.TranslationUpdate));
		this.holder.SetActive(false);
		if (Master.instance.GAME_PUBLIC_STATE == Master.GamePublicState.released)
		{
			this.displayImage.texture = this.buyTexture;
		}
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x0003A279 File Offset: 0x00038479
	private void OnDestroy()
	{
		if (LoadingScreenCallToAction.instance == this)
		{
			LoadingScreenCallToAction.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.TranslationUpdate));
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x0003A2B0 File Offset: 0x000384B0
	private void Update()
	{
		int num = -1;
		if (num < 0)
		{
			num = SceneManager.GetActiveScene().buildIndex;
		}
		if (num != 0)
		{
			return;
		}
		if (LoadingScreenNotifications.HasNotifications())
		{
			return;
		}
		if (!LoadingScreenCallToAction.IsEnabled() && LoadingScreenCallToAction.booked)
		{
			LoadingScreenCallToAction.booked = false;
			this.holder.SetActive(true);
			this.inputDelay = 0.5f;
			this.selectionIndex = -1;
			this.selectionIndexOld = -1;
			this.imageSelector.enabled = false;
			this.textSkip.color = Color.white;
			Sound.Play("SoundMenuPopUp", 1f, 1f);
			return;
		}
		this.inputDelay -= Tick.Time;
		if (this.inputDelay > 0f)
		{
			return;
		}
		float num2 = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.menuMoveUp, Controls.InputAction.menuMoveDown, true);
		bool flag = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true);
		if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true))
		{
			this.holder.SetActive(false);
			Sound.Play("SoundMenuBack", 1f, 1f);
			return;
		}
		if (VirtualCursors.IsCursorVisible(0, true))
		{
			this.selectionIndex = -1;
			Vector2 vector = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, this.canvasScaler.referenceResolution);
			if (vector.x > this.imageSelector.rectTransform.anchoredPosition.x - this.imageSelector.rectTransform.sizeDelta.x / 2f && vector.x < this.imageSelector.rectTransform.anchoredPosition.x + this.imageSelector.rectTransform.sizeDelta.x / 2f && vector.y > this.imageSelector.rectTransform.anchoredPosition.y - this.imageSelector.rectTransform.sizeDelta.y / 2f && vector.y < this.imageSelector.rectTransform.anchoredPosition.y + this.imageSelector.rectTransform.sizeDelta.y / 2f)
			{
				this.selectionIndex = 0;
			}
			else if (vector.x > this.textSkip.rectTransform.anchoredPosition.x - this.textSkip.preferredWidth / 2f && vector.x < this.textSkip.rectTransform.anchoredPosition.x + this.textSkip.preferredWidth / 2f && vector.y > this.textSkip.rectTransform.anchoredPosition.y - this.textSkip.preferredHeight / 2f && vector.y < this.textSkip.rectTransform.anchoredPosition.y + this.textSkip.preferredHeight / 2f)
			{
				this.selectionIndex = 1;
			}
		}
		else if (this.selectionIndex < 0)
		{
			if (num2 != 0f || flag)
			{
				this.selectionIndex = 0;
				return;
			}
		}
		else
		{
			if (num2 > 0f)
			{
				this.selectionIndex = 0;
			}
			if (num2 < 0f)
			{
				this.selectionIndex = 1;
			}
		}
		if (this.selectionIndexOld != this.selectionIndex && this.selectionIndex >= 0)
		{
			Sound.Play("SoundMenuSelectionChange", 1f, 1f);
		}
		if (this.selectionIndex >= 0 && flag)
		{
			int num3 = this.selectionIndex;
			if (num3 != 0)
			{
				if (num3 == 1)
				{
					this.holder.SetActive(false);
				}
			}
			else
			{
				MagazineUiScript.AdLinkOpen();
			}
			Sound.Play("SoundMenuSelect", 1f, 1f);
		}
		this.selectionIndexOld = this.selectionIndex;
		this.imageSelector.enabled = this.selectionIndex == 0;
		this.textSkip.color = ((this.selectionIndex == 1) ? Color.yellow : Color.white);
	}

	public static LoadingScreenCallToAction instance;

	private const int PLAYER_INDEX = 0;

	private const int BUTTON_IMAGE = 0;

	private const int BUTTON_SKIP = 1;

	private const float INPUT_DELAY = 0.5f;

	public CanvasScaler canvasScaler;

	public GameObject holder;

	public TextMeshProUGUI textSkip;

	public TextMeshProUGUI textInfos;

	public TextMeshProUGUI textTitle;

	public RawImage displayImage;

	public RawImage imageSelector;

	public Texture buyTexture;

	private static bool booked;

	private float inputDelay = 0.5f;

	private int selectionIndex = -1;

	private int selectionIndexOld = -1;
}
