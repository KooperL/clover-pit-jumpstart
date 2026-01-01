using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020000BB RID: 187
public class LoadingScreenCallToAction : MonoBehaviour
{
	// Token: 0x06000A0F RID: 2575 RVA: 0x0000DF8A File Offset: 0x0000C18A
	public static bool IsEnabled()
	{
		return !(LoadingScreenCallToAction.instance == null) && LoadingScreenCallToAction.instance.holder.activeSelf;
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x0000DFAA File Offset: 0x0000C1AA
	private static bool IsBooked()
	{
		return LoadingScreenCallToAction.booked;
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x0000DFB1 File Offset: 0x0000C1B1
	public static void BookCallToAction()
	{
		LoadingScreenCallToAction.booked = true;
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x0000DFB9 File Offset: 0x0000C1B9
	public static bool LoadingShouldWait()
	{
		return !(LoadingScreenCallToAction.instance == null) && (LoadingScreenCallToAction.IsBooked() || LoadingScreenCallToAction.IsEnabled());
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x000518B8 File Offset: 0x0004FAB8
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

	// Token: 0x06000A14 RID: 2580 RVA: 0x0000DFD8 File Offset: 0x0000C1D8
	private void Awake()
	{
		LoadingScreenCallToAction.instance = this;
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x0005196C File Offset: 0x0004FB6C
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

	// Token: 0x06000A16 RID: 2582 RVA: 0x0000DFE0 File Offset: 0x0000C1E0
	private void OnDestroy()
	{
		if (LoadingScreenCallToAction.instance == this)
		{
			LoadingScreenCallToAction.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.TranslationUpdate));
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x000519CC File Offset: 0x0004FBCC
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

	// Token: 0x04000A39 RID: 2617
	public static LoadingScreenCallToAction instance;

	// Token: 0x04000A3A RID: 2618
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000A3B RID: 2619
	private const int BUTTON_IMAGE = 0;

	// Token: 0x04000A3C RID: 2620
	private const int BUTTON_SKIP = 1;

	// Token: 0x04000A3D RID: 2621
	private const float INPUT_DELAY = 0.5f;

	// Token: 0x04000A3E RID: 2622
	public CanvasScaler canvasScaler;

	// Token: 0x04000A3F RID: 2623
	public GameObject holder;

	// Token: 0x04000A40 RID: 2624
	public TextMeshProUGUI textSkip;

	// Token: 0x04000A41 RID: 2625
	public TextMeshProUGUI textInfos;

	// Token: 0x04000A42 RID: 2626
	public TextMeshProUGUI textTitle;

	// Token: 0x04000A43 RID: 2627
	public RawImage displayImage;

	// Token: 0x04000A44 RID: 2628
	public RawImage imageSelector;

	// Token: 0x04000A45 RID: 2629
	public Texture buyTexture;

	// Token: 0x04000A46 RID: 2630
	private static bool booked;

	// Token: 0x04000A47 RID: 2631
	private float inputDelay = 0.5f;

	// Token: 0x04000A48 RID: 2632
	private int selectionIndex = -1;

	// Token: 0x04000A49 RID: 2633
	private int selectionIndexOld = -1;
}
