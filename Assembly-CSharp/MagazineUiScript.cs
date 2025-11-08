using System;
using System.Collections;
using System.Text;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MagazineUiScript : MonoBehaviour
{
	// Token: 0x060008DF RID: 2271 RVA: 0x0003A8DD File Offset: 0x00038ADD
	public static bool IsEnabled()
	{
		return !(MagazineUiScript.instance == null) && MagazineUiScript.instance.holder.activeSelf;
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x0003A900 File Offset: 0x00038B00
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

	// Token: 0x060008E1 RID: 2273 RVA: 0x0003A990 File Offset: 0x00038B90
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

	// Token: 0x060008E2 RID: 2274 RVA: 0x0003A9EE File Offset: 0x00038BEE
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

	// Token: 0x060008E3 RID: 2275 RVA: 0x0003A9FD File Offset: 0x00038BFD
	public static void ForceClose_Death()
	{
		if (MagazineUiScript.instance == null)
		{
			return;
		}
		MagazineUiScript.instance.forceClose_Death = true;
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x0003AA18 File Offset: 0x00038C18
	public static bool IsForceClosing()
	{
		return !(MagazineUiScript.instance == null) && MagazineUiScript.instance.forceClose_Death;
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x0003AA34 File Offset: 0x00038C34
	private void TranslationUpdate()
	{
		if (!PlatformMaster.PlatformIsComputer())
		{
			this.adRawImage.texture = this.adTexture_CloverPitConsole;
			this.adPromptText.text = "~ Panik Arcade";
			return;
		}
		if (Master.IsDemo)
		{
			Master.GamePublicState game_PUBLIC_STATE = Master.instance.GAME_PUBLIC_STATE;
			if (game_PUBLIC_STATE != Master.GamePublicState.wishlistOrPrerelease)
			{
				if (game_PUBLIC_STATE == Master.GamePublicState.released)
				{
					this.adRawImage.texture = this.adTexture_CloverPitBuy;
					this.adPromptText.text = Translation.Get("VARIOUS_MAGAZINE_BUY_PROMPT_DESKTOP");
				}
				else
				{
					string text = "MagazineUiScript.TranslationUpdate(): Call to action kind not handled: " + Master.instance.GAME_PUBLIC_STATE.ToString();
					Debug.LogError(text);
					ConsolePrompt.LogError(text, "", 0f);
				}
			}
			else
			{
				this.adRawImage.texture = this.adTexture_CloverPitWishlist;
				this.adPromptText.text = Translation.Get("VARIOUS_MAGAZINE_WISHLIST_PROMPT_DESKTOP");
			}
		}
		else
		{
			this.adRawImage.texture = this.adTexture_YellowTaxi;
			this.adPromptText.text = Translation.Get("VARIOUS_MAGAZINE_YELLOW_TAXI_PROMPT_DESKTOP");
		}
		this.creditsTitleText.text = Translation.Get("VARIOUS_MAGAZINE_CREDITS_TITLE");
		this.creditsBodyText.text = MagazineUiScript.GetCreditsString();
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x0003AB5C File Offset: 0x00038D5C
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

	// Token: 0x060008E7 RID: 2279 RVA: 0x0003AD3C File Offset: 0x00038F3C
	public static void AdLinkOpen()
	{
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

	// Token: 0x060008E8 RID: 2280 RVA: 0x0003AE04 File Offset: 0x00039004
	public static void SurveyLinkOpen()
	{
		if (PlatformMaster.PlatformIsComputer())
		{
			Application.OpenURL("https://forms.gle/em1oD4CoCaa8D6Dg9");
			return;
		}
		Debug.Log("Link for platform: " + PlatformMaster.PlatformKindGet().ToString() + "is not implemented!");
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x0003AE4A File Offset: 0x0003904A
	private void Awake()
	{
		MagazineUiScript.instance = this;
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x0003AE52 File Offset: 0x00039052
	private void Start()
	{
		MagazineUiScript.Close(true);
		this.TranslationUpdate();
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.TranslationUpdate));
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x0003AE80 File Offset: 0x00039080
	private void OnDestroy()
	{
		if (MagazineUiScript.instance == this)
		{
			MagazineUiScript.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.TranslationUpdate));
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x0003AEB8 File Offset: 0x000390B8
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

	public static MagazineUiScript instance;

	private const string CAMERA_DISABLE_REASON = "mgzn";

	private const int PLAYER_INDEX = 0;

	private const float MAGAZINE_Y_OUT = -540f;

	private const string STEAM_LINK_CLOVERPIT_DESKTOP = "steam://store/3314790";

	private const string STEAM_LINK_YELLOW_TAXI_DESKTOP = "steam://store/2011780";

	private const string PLAYTEST_SURVEY_LINK = "https://forms.gle/em1oD4CoCaa8D6Dg9";

	public GameObject holder;

	public RectTransform magazineHolderRecTr;

	public RectTransform magazineWaverRecTr;

	public CanvasScaler canvasScaler;

	public RawImage adRawImage;

	public TextMeshProUGUI adPromptText;

	public Texture2D adTexture_CloverPitWishlist;

	public Texture2D adTexture_CloverPitBuy;

	public Texture2D adTexture_YellowTaxi;

	public Texture2D adTexture_CloverPitConsole;

	public BounceScript adBounceScript;

	public RectTransform adSelectableRectTr;

	public RectTransform adPositionShifter;

	public TextMeshProUGUI creditsTitleText;

	public TextMeshProUGUI creditsBodyText;

	private bool forceClose_Death;

	private static StringBuilder _sb = new StringBuilder();
}
