using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenCallToAction : MonoBehaviour
{
	// Token: 0x060008D8 RID: 2264 RVA: 0x0003A42C File Offset: 0x0003862C
	public static bool IsEnabled()
	{
		return !(LoadingScreenCallToAction.instance == null) && LoadingScreenCallToAction.instance.holder.activeSelf;
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x0003A44C File Offset: 0x0003864C
	private static bool IsBooked()
	{
		return LoadingScreenCallToAction.booked;
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x0003A453 File Offset: 0x00038653
	public static void BookCallToAction()
	{
		LoadingScreenCallToAction.booked = true;
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x0003A45B File Offset: 0x0003865B
	public static bool LoadingShouldWait()
	{
		return !(LoadingScreenCallToAction.instance == null) && (LoadingScreenCallToAction.IsBooked() || LoadingScreenCallToAction.IsEnabled());
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x0003A47C File Offset: 0x0003867C
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

	// Token: 0x060008DD RID: 2269 RVA: 0x0003A52E File Offset: 0x0003872E
	private void Awake()
	{
		LoadingScreenCallToAction.instance = this;
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x0003A538 File Offset: 0x00038738
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

	// Token: 0x060008DF RID: 2271 RVA: 0x0003A595 File Offset: 0x00038795
	private void OnDestroy()
	{
		if (LoadingScreenCallToAction.instance == this)
		{
			LoadingScreenCallToAction.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.TranslationUpdate));
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x0003A5CC File Offset: 0x000387CC
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
