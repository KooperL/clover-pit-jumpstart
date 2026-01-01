using System;
using System.Collections;
using System.Text;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000BD RID: 189
public class MagazineUiScript : MonoBehaviour
{
	// Token: 0x06000A25 RID: 2597 RVA: 0x0000E111 File Offset: 0x0000C311
	public static bool IsEnabled()
	{
		return !(MagazineUiScript.instance == null) && MagazineUiScript.instance.holder.activeSelf;
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x00051ED8 File Offset: 0x000500D8
	public static void Open()
	{
		if (MagazineUiScript.instance == null)
		{
			return;
		}
		MagazineUiScript.instance.holder.SetActive(true);
		Sound.Play3D("SoundMagazineOpen", MagazinesHolderScript.instance.transform.position, 20f, 1f, 1f, AudioRolloffMode.Linear);
		CameraController.DisableReason_Add("mgzn");
		VirtualCursors.CursorDesiredVisibilitySet(0, true);
		VirtualCursors.CursorPositionNormalizedSet(0, Vector2.zero, true);
		MemoScript.Close(false);
		MagazineUiScript.instance.StartCoroutine(MagazineUiScript.instance.MainCoroutine());
	}

	// Token: 0x06000A27 RID: 2599 RVA: 0x00051F68 File Offset: 0x00050168
	public static void Close(bool initialSetup)
	{
		if (MagazineUiScript.instance == null)
		{
			return;
		}
		MagazineUiScript.instance.holder.SetActive(false);
		MagazineUiScript.instance.magazineHolderRecTr.anchoredPosition = new Vector2(0f, -540f);
		if (initialSetup)
		{
			return;
		}
		CameraController.DisableReason_Remove("mgzn");
		VirtualCursors.CursorDesiredVisibilitySet(0, false);
	}

	// Token: 0x06000A28 RID: 2600 RVA: 0x0000E131 File Offset: 0x0000C331
	private IEnumerator MainCoroutine()
	{
		float creditsMovementDelayTimer = 3f;
		float creditsMovementY = 0f;
		float creditsMovementHeight = this.creditsBodyText.preferredHeight + 50f;
		this.creditsBodyText.rectTransform.anchoredPosition = new Vector2(0f, 0f);
		bool mouseHoveringAdsOld = false;
		while (MagazineUiScript.instance.magazineHolderRecTr.anchoredPosition.y < -1f && !MagazineUiScript.IsForceClosing())
		{
			MagazineUiScript.instance.magazineHolderRecTr.anchoredPosition += (Vector2.zero - MagazineUiScript.instance.magazineHolderRecTr.anchoredPosition) * Tick.Time * 20f;
			yield return null;
		}
		MagazineUiScript.instance.magazineHolderRecTr.anchoredPosition = Vector2.zero;
		while (!MagazineUiScript.IsForceClosing())
		{
			bool flag = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true);
			bool flag2 = Controls.ActionButton_HoldGet(0, Controls.InputAction.menuSelect, true);
			bool flag3 = flag && Controls.MouseButton_PressedGet(0, Controls.MouseElement.LeftButton);
			bool flag4 = flag2 && Controls.MouseButton_HoldGet(0, Controls.MouseElement.LeftButton);
			Vector2 vector = VirtualCursors.CursorPositionNormalizedCenteredGet_ReferenceResolution(0, this.canvasScaler.referenceResolution);
			Vector2 vector2 = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, this.canvasScaler.referenceResolution);
			bool flag5 = Mathf.Abs(vector.x) >= 0.4f || Mathf.Abs(vector.y) >= 0.43f;
			if ((flag3 && flag5) || Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true))
			{
				break;
			}
			float num = ((flag2 && (!flag4 || (vector.x > 0f && !flag5))) ? 10f : 1f);
			creditsMovementDelayTimer -= Tick.Time * num;
			if (creditsMovementDelayTimer <= 0f)
			{
				creditsMovementY += 20f * Tick.Time * num;
				if (creditsMovementY > creditsMovementHeight)
				{
					creditsMovementY = -200f;
				}
				this.creditsBodyText.rectTransform.anchoredPosition = new Vector2(0f, creditsMovementY);
			}
			bool flag6 = vector2.x > this.adSelectableRectTr.anchoredPosition.x - this.adSelectableRectTr.sizeDelta.x / 2f && vector2.x < this.adSelectableRectTr.anchoredPosition.x + this.adSelectableRectTr.sizeDelta.x / 2f && vector2.y > this.adSelectableRectTr.anchoredPosition.y - this.adSelectableRectTr.sizeDelta.y / 2f && vector2.y < this.adSelectableRectTr.anchoredPosition.y + this.adSelectableRectTr.sizeDelta.y / 2f;
			bool flag7 = vector.x < 0f && flag6;
			if (mouseHoveringAdsOld != flag7)
			{
				mouseHoveringAdsOld = flag7;
				if (flag7)
				{
					Sound.Play("SoundMenuSelectionChange", 1f, 1f);
					this.adBounceScript.SetBounceScale(0.05f);
				}
			}
			Vector2 zero = Vector2.zero;
			if (flag7)
			{
				zero = new Vector2(0f, 10f);
			}
			this.adPositionShifter.anchoredPosition = Vector2.Lerp(this.adPositionShifter.anchoredPosition, zero, Tick.Time * 20f);
			if (flag3 && flag7)
			{
				this.adBounceScript.SetBounceScale(0.1f);
				MagazineUiScript.AdLinkOpen();
			}
			yield return null;
		}
		Sound.Play3D("SoundMagazineClose", MagazinesHolderScript.instance.transform.position, 20f, 1f, 1f, AudioRolloffMode.Linear);
		while (MagazineUiScript.instance.magazineHolderRecTr.anchoredPosition.y > -539f && !MagazineUiScript.IsForceClosing())
		{
			MagazineUiScript.instance.magazineHolderRecTr.anchoredPosition += (new Vector2(0f, -540f) - MagazineUiScript.instance.magazineHolderRecTr.anchoredPosition) * Tick.Time * 20f;
			yield return null;
		}
		MagazineUiScript.instance.magazineHolderRecTr.anchoredPosition = new Vector2(0f, -540f);
		GameplayMaster.instance.FCall_MagazineClose();
		this.forceClose_Death = false;
		yield break;
	}

	// Token: 0x06000A29 RID: 2601 RVA: 0x0000E140 File Offset: 0x0000C340
	public static void ForceClose_Death()
	{
		if (MagazineUiScript.instance == null)
		{
			return;
		}
		MagazineUiScript.instance.forceClose_Death = true;
	}

	// Token: 0x06000A2A RID: 2602 RVA: 0x0000E15B File Offset: 0x0000C35B
	public static bool IsForceClosing()
	{
		return !(MagazineUiScript.instance == null) && MagazineUiScript.instance.forceClose_Death;
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x00051FC8 File Offset: 0x000501C8
	private void TranslationUpdate()
	{
		this.creditsTitleText.text = Translation.Get("VARIOUS_MAGAZINE_CREDITS_TITLE");
		this.creditsBodyText.text = MagazineUiScript.GetCreditsString();
		if (!PlatformMaster.PlatformIsComputer())
		{
			this.adRawImage.texture = this.adTexture_CloverPitConsole;
			this.adPromptText.text = "~ Panik Arcade";
			return;
		}
		if (Master.instance.MobileAdEnabled())
		{
			this.adRawImage.texture = this.adTexture_CloverPitMobile;
			this.adPromptText.text = Translation.Get("VARIOUS_MAGAZINE_PLAY_MOBILE_VERSION");
			return;
		}
		if (!Master.IsDemo)
		{
			this.adRawImage.texture = this.adTexture_YellowTaxi;
			this.adPromptText.text = Translation.Get("VARIOUS_MAGAZINE_YELLOW_TAXI_PROMPT_DESKTOP");
			return;
		}
		Master.GamePublicState game_PUBLIC_STATE = Master.instance.GAME_PUBLIC_STATE;
		if (game_PUBLIC_STATE == Master.GamePublicState.wishlistOrPrerelease)
		{
			this.adRawImage.texture = this.adTexture_CloverPitWishlist;
			this.adPromptText.text = Translation.Get("VARIOUS_MAGAZINE_WISHLIST_PROMPT_DESKTOP");
			return;
		}
		if (game_PUBLIC_STATE == Master.GamePublicState.released)
		{
			this.adRawImage.texture = this.adTexture_CloverPitBuy;
			this.adPromptText.text = Translation.Get("VARIOUS_MAGAZINE_BUY_PROMPT_DESKTOP");
			return;
		}
		string text = "MagazineUiScript.TranslationUpdate(): Call to action kind not handled: " + Master.instance.GAME_PUBLIC_STATE.ToString();
		Debug.LogError(text);
		ConsolePrompt.LogError(text, "", 0f);
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x0005211C File Offset: 0x0005031C
	public static string GetCreditsString()
	{
		MagazineUiScript._sb.Clear();
		MagazineUiScript._sb.Append(Translation.Get("VARIOUS_MAGAZINE_CREDITS_DEVELOPERS"));
		MagazineUiScript._sb.Append("\n\n");
		MagazineUiScript._sb.Append(Translation.Get("VARIOUS_MAGAZINE_CREDITS_MUSIC"));
		MagazineUiScript._sb.Append("\n\n");
		MagazineUiScript._sb.Append(Translation.Get("VARIOUS_MAGAZINE_CREDITS_PUBLISHER"));
		MagazineUiScript._sb.Append("\n\n");
		MagazineUiScript._sb.Append(Translation.Get("VARIOUS_MAGAZINE_CREDITS_TRAILER"));
		MagazineUiScript._sb.Append("\n\n");
		MagazineUiScript._sb.Append(Translation.Get("VARIOUS_MAGAZINE_CREDITS_PROGRAMMING_HELP"));
		MagazineUiScript._sb.Append("\n\n");
		MagazineUiScript._sb.Append(Translation.Get("VARIOUS_MAGAZINE_CREDITS_TESTERS_EARLY"));
		MagazineUiScript._sb.Append("\n\n");
		MagazineUiScript._sb.Append(Translation.Get("VARIOUS_MAGAZINE_CREDITS_PLAYTEST"));
		MagazineUiScript._sb.Append("\n\n");
		MagazineUiScript._sb.Append(Translation.Get("VARIOUS_MAGAZINE_CREDITS_LOCALIZATION"));
		MagazineUiScript._sb.Append("\n\n");
		MagazineUiScript._sb.Append(Translation.Get("VARIOUS_MAGAZINE_CREDITS_LOCALIZATION_LQA"));
		MagazineUiScript._sb.Append("\n\n");
		MagazineUiScript._sb.Append(Translation.Get("VARIOUS_MAGAZINE_CREDITS_SOUNDS"));
		MagazineUiScript._sb.Append("\n\n");
		MagazineUiScript._sb.Append(Translation.Get("VARIOUS_MAGAZINE_CREDITS_FRIENDS AND FAMILY"));
		MagazineUiScript._sb.Append("\n\n");
		MagazineUiScript._sb.Append(Translation.Get("VARIOUS_MAGAZINE_CREDITS_OTHERS"));
		MagazineUiScript._sb.Append("\n\n");
		return MagazineUiScript._sb.ToString();
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x000522FC File Offset: 0x000504FC
	public static void AdLinkOpen()
	{
		if (Master.instance.MobileAdEnabled())
		{
			Application.OpenURL("https://cloverpit.ateo.ch/");
			return;
		}
		if (PlatformMaster.PlatformIsComputer())
		{
			if (Master.IsDemo)
			{
				Master.GamePublicState game_PUBLIC_STATE = Master.instance.GAME_PUBLIC_STATE;
				if (game_PUBLIC_STATE != Master.GamePublicState.wishlistOrPrerelease)
				{
					if (game_PUBLIC_STATE == Master.GamePublicState.released)
					{
						Application.OpenURL("steam://store/3314790");
					}
					else
					{
						string text = "MagazineUiScript.AdLinkOpen(): Game public state not handled: " + Master.instance.GAME_PUBLIC_STATE.ToString();
						Debug.LogError(text);
						ConsolePrompt.LogError(text, "", 0f);
					}
				}
				else
				{
					Application.OpenURL("steam://store/3314790");
				}
			}
			else
			{
				Application.OpenURL("steam://store/2011780");
			}
			Sound.Play("SoundMenuSelect", 1f, 1f);
			return;
		}
		Debug.Log("Link for platform: " + PlatformMaster.PlatformKindGet().ToString() + "is not implemented!");
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x000523D8 File Offset: 0x000505D8
	public static void SurveyLinkOpen()
	{
		if (PlatformMaster.PlatformIsComputer())
		{
			Application.OpenURL("https://forms.gle/em1oD4CoCaa8D6Dg9");
			return;
		}
		Debug.Log("Link for platform: " + PlatformMaster.PlatformKindGet().ToString() + "is not implemented!");
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x0000E176 File Offset: 0x0000C376
	private void Awake()
	{
		MagazineUiScript.instance = this;
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x0000E17E File Offset: 0x0000C37E
	private void Start()
	{
		MagazineUiScript.Close(true);
		this.TranslationUpdate();
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.TranslationUpdate));
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x0000E1AC File Offset: 0x0000C3AC
	private void OnDestroy()
	{
		if (MagazineUiScript.instance == this)
		{
			MagazineUiScript.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.TranslationUpdate));
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x00052420 File Offset: 0x00050620
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		if (!MagazineUiScript.IsEnabled())
		{
			return;
		}
		this.magazineWaverRecTr.anchoredPosition = Vector2.zero + new Vector2(Util.AngleSin(Tick.PassedTime * 20f) * 2f, Util.AngleCos(Tick.PassedTime * 45f) * 8f);
	}

	// Token: 0x04000A53 RID: 2643
	public static MagazineUiScript instance;

	// Token: 0x04000A54 RID: 2644
	private const string CAMERA_DISABLE_REASON = "mgzn";

	// Token: 0x04000A55 RID: 2645
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000A56 RID: 2646
	private const float MAGAZINE_Y_OUT = -540f;

	// Token: 0x04000A57 RID: 2647
	private const string STEAM_LINK_CLOVERPIT_DESKTOP = "steam://store/3314790";

	// Token: 0x04000A58 RID: 2648
	private const string STEAM_LINK_YELLOW_TAXI_DESKTOP = "steam://store/2011780";

	// Token: 0x04000A59 RID: 2649
	private const string PLAYTEST_SURVEY_LINK = "https://forms.gle/em1oD4CoCaa8D6Dg9";

	// Token: 0x04000A5A RID: 2650
	private const string MOBILE_VERSION_DOWNLOAD_LINK = "https://cloverpit.ateo.ch/";

	// Token: 0x04000A5B RID: 2651
	public GameObject holder;

	// Token: 0x04000A5C RID: 2652
	public RectTransform magazineHolderRecTr;

	// Token: 0x04000A5D RID: 2653
	public RectTransform magazineWaverRecTr;

	// Token: 0x04000A5E RID: 2654
	public CanvasScaler canvasScaler;

	// Token: 0x04000A5F RID: 2655
	public RawImage adRawImage;

	// Token: 0x04000A60 RID: 2656
	public TextMeshProUGUI adPromptText;

	// Token: 0x04000A61 RID: 2657
	public Texture2D adTexture_CloverPitWishlist;

	// Token: 0x04000A62 RID: 2658
	public Texture2D adTexture_CloverPitBuy;

	// Token: 0x04000A63 RID: 2659
	public Texture2D adTexture_YellowTaxi;

	// Token: 0x04000A64 RID: 2660
	public Texture2D adTexture_CloverPitConsole;

	// Token: 0x04000A65 RID: 2661
	public Texture2D adTexture_CloverPitMobile;

	// Token: 0x04000A66 RID: 2662
	public BounceScript adBounceScript;

	// Token: 0x04000A67 RID: 2663
	public RectTransform adSelectableRectTr;

	// Token: 0x04000A68 RID: 2664
	public RectTransform adPositionShifter;

	// Token: 0x04000A69 RID: 2665
	public TextMeshProUGUI creditsTitleText;

	// Token: 0x04000A6A RID: 2666
	public TextMeshProUGUI creditsBodyText;

	// Token: 0x04000A6B RID: 2667
	private bool forceClose_Death;

	// Token: 0x04000A6C RID: 2668
	private static StringBuilder _sb = new StringBuilder();
}
