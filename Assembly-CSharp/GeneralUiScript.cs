using System;
using System.Collections;
using System.Numerics;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GeneralUiScript : MonoBehaviour
{
	// Token: 0x060008A3 RID: 2211 RVA: 0x00038C62 File Offset: 0x00036E62
	public static bool IsEnabled()
	{
		return GeneralUiScript.instance.holder.activeSelf;
	}

	// Token: 0x060008A4 RID: 2212 RVA: 0x00038C73 File Offset: 0x00036E73
	public static void ForceEnabled()
	{
		GeneralUiScript.instance.holder.SetActive(true);
	}

	// Token: 0x060008A5 RID: 2213 RVA: 0x00038C85 File Offset: 0x00036E85
	private void Initialize()
	{
		this.titleScreenRectTransform = this.titleScreenHolder.GetComponent<RectTransform>();
		this.fadedIn = false;
		this.fadeImage.color = this.fadeImageColor;
	}

	// Token: 0x060008A6 RID: 2214 RVA: 0x00038CB0 File Offset: 0x00036EB0
	public bool HasFadedIn()
	{
		return this.fadedIn;
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x00038CB8 File Offset: 0x00036EB8
	public bool IsShowingTitleScreen()
	{
		return this.titleScreenShowing;
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x00038CC0 File Offset: 0x00036EC0
	public static void ComingFromVictoryFlag_Set(bool value)
	{
		GeneralUiScript.comingFromVictory = value;
	}

	// Token: 0x060008A9 RID: 2217 RVA: 0x00038CC8 File Offset: 0x00036EC8
	private IEnumerator IntroAndTitleScreen_Coroutine()
	{
		bool hasOldSession = GameplayData.NewGameIntroFinished_Get();
		bool _comingFromVictoryScreen = GeneralUiScript.comingFromVictory;
		bool canInputSeed = GameplayMaster.CanInputSeed();
		GeneralUiScript.comingFromVictory = false;
		float alpha = 1f;
		this.fadeImageColor.a = alpha;
		this.fadeImage.color = this.fadeImageColor;
		float transitionSpeed = (float)Data.settings.transitionSpeed;
		CameraController.SetPosition(CameraController.PositionKind.RoomTopView, true, 1f);
		PlatformAPI.AchievementUnlock_Demo(PlatformAPI.AchievementDemo.FearOfNeedles);
		if (!GeneralUiScript.requestedNewSessionReset)
		{
			if (GameplayMaster.HasDiedOnce() && !_comingFromVictoryScreen)
			{
				if (!canInputSeed)
				{
					goto IL_06A2;
				}
			}
			else
			{
				this.titleScreenShowing = true;
				yield return null;
				if ((Master.IsDemo || Master.IsPlaytestBuild) && Master.instance.DEMOORPLAYTEST_HAS_DEVELOPER_LETTER)
				{
					this.developerLetterHolder.SetActive(true);
					this.developerLetterPrompt.alpha = 0f;
					Sound.Play("SoundMenuPopUp", 1f, 1f);
					float _inputDelayTimer = 0.5f;
					if (Master.IsPlaytestBuild)
					{
						_inputDelayTimer = 3f;
					}
					for (;;)
					{
						_inputDelayTimer -= Tick.Time;
						if (_inputDelayTimer > 0f)
						{
							yield return null;
						}
						else
						{
							this.developerLetterPrompt.alpha = 1f;
							if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true))
							{
								break;
							}
							yield return null;
						}
					}
					Sound.Play("SoundMenuSelect", 1f, 1f);
					this.developerLetterHolder.SetActive(false);
				}
				this.titleScreenHolder.SetActive(true);
				Sound.Play("SoundTitleImpact", 1f, 1f);
				alpha = 1f;
				while (alpha > 0f)
				{
					if (Master.instance.EDITOR_QUICK_START)
					{
						alpha = 0f;
					}
					alpha -= Tick.Time * 1f * transitionSpeed;
					this.fadeImageColor.a = alpha;
					this.fadeImage.color = this.fadeImageColor;
					yield return null;
				}
				this.fadeImageColor.a = 0f;
				this.fadeImage.color = this.fadeImageColor;
				while (!Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true))
				{
					yield return null;
				}
				Sound.Play_Unpausable("SoundMenuStart", 1f, 1f);
				Controls.VibrationSet_PreferMax(this.player, 0.25f);
				float flickerTime = 0.5f;
				while (flickerTime > 0f)
				{
					flickerTime -= Tick.Time;
					this.textTitleStart.enabled = Util.AngleSin(Tick.PassedTime * 1440f) > -0.5f;
					yield return null;
				}
				this.textTitleStart.enabled = false;
				alpha = 0f;
				while (alpha < 1f)
				{
					if (Master.instance.EDITOR_QUICK_START)
					{
						alpha = 1f;
					}
					alpha += Tick.Time * 1f * transitionSpeed;
					this.fadeImageColor.a = alpha;
					this.fadeImage.color = this.fadeImageColor;
					yield return null;
				}
				this.fadeImageColor.a = 1f;
				this.fadeImage.color = this.fadeImageColor;
				this.titleScreenShowing = false;
				if (Data.game.enforceRunReset)
				{
					GeneralUiScript.requestedNewSessionReset = true;
					Data.game.enforceRunReset = false;
					goto IL_0688;
				}
				if (GeneralUiScript.requestedNewSessionReset)
				{
					Debug.LogError("IF you requested a new session already, before spawning the  menu, flow shouldn't get here! Are you forgetting a goto? ");
				}
			}
			if (hasOldSession || canInputSeed)
			{
				this.titleScreenHolder.SetActive(false);
				yield return null;
				VirtualCursors.CursorDesiredVisibilitySet(0, true);
				do
				{
					this.gotoSeedInputMenu = false;
					string[] array = new string[] { "ERROR", "ERROR" };
					string[] array2;
					ScreenMenuScript.OptionEvent[] array3;
					if (hasOldSession)
					{
						string text = Translation.Get("SCREEN_MENU_OPTION_NEW_RUN_SEEDED");
						if (!canInputSeed)
						{
							text += " <sprite name=\"RedLock\">";
						}
						array2 = new string[]
						{
							Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_OPTION_CONTINUE"), Strings.SanitizationSubKind.none),
							Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_OPTION_NEW_RUN"), Strings.SanitizationSubKind.none),
							Strings.Sanitize(Strings.SantizationKind.menus, text, Strings.SanitizationSubKind.none)
						};
						array3 = new ScreenMenuScript.OptionEvent[]
						{
							new ScreenMenuScript.OptionEvent(this._IntroMenuContinue),
							new ScreenMenuScript.OptionEvent(this._IntroMenuNewGame),
							new ScreenMenuScript.OptionEvent(this._IntroMenuNewSeededGame)
						};
					}
					else
					{
						array2 = new string[]
						{
							Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_OPTION_NEW_RUN"), Strings.SanitizationSubKind.none),
							Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_OPTION_NEW_RUN_SEEDED"), Strings.SanitizationSubKind.none)
						};
						array3 = new ScreenMenuScript.OptionEvent[]
						{
							new ScreenMenuScript.OptionEvent(this._IntroMenuNewGame),
							new ScreenMenuScript.OptionEvent(this._IntroMenuNewSeededGame)
						};
					}
					ScreenMenuScript.Open(true, false, -1, ScreenMenuScript.Positioning.center, 5f, Translation.Get("SCREEN_MENU_TITLE_RUN"), array2, array3);
					Sound.Play("SoundMenuPopUp", 1f, 1f);
					yield return null;
					while (ScreenMenuScript.IsEnabled())
					{
						yield return null;
					}
					if (!this.gotoSeedInputMenu)
					{
						goto IL_0688;
					}
					SeedMenuScript.Open();
					while (SeedMenuScript.IsEnabled())
					{
						yield return null;
					}
					yield return null;
				}
				while (GameplayMaster.specificSeedRequest_ForNewGame == null);
				GeneralUiScript.requestedNewSessionReset = true;
			}
			IL_0688:
			if (GeneralUiScript.requestedNewSessionReset)
			{
				Data.game.GameplayDataReset(true);
				Level.Restart(false);
				yield break;
			}
		}
		IL_06A2:
		this.titleScreenHolder.SetActive(false);
		GeneralUiScript.requestedNewSessionReset = false;
		float quickRestartSpeed = 1f;
		if (GameplayMaster.HasDiedOnce() || _comingFromVictoryScreen)
		{
			quickRestartSpeed = 2f;
		}
		CameraController.SetPosition(CameraController.PositionKind.Free, true, 1f);
		while (alpha > 0f)
		{
			if (Master.instance.EDITOR_QUICK_START)
			{
				alpha = 0f;
			}
			alpha -= Tick.Time * 0.5f * quickRestartSpeed * transitionSpeed;
			this.fadeImageColor.a = alpha;
			this.fadeImage.color = this.fadeImageColor;
			yield return null;
		}
		this.fadeImageColor.a = 0f;
		this.fadeImage.color = this.fadeImageColor;
		this.fadedIn = true;
		yield break;
	}

	// Token: 0x060008AA RID: 2218 RVA: 0x00038CD7 File Offset: 0x00036ED7
	private void _IntroMenuNewGame()
	{
		GeneralUiScript.requestedNewSessionReset = true;
		ScreenMenuScript.Close(false);
	}

	// Token: 0x060008AB RID: 2219 RVA: 0x00038CE5 File Offset: 0x00036EE5
	private void _IntroMenuContinue()
	{
		VirtualCursors.CursorDesiredVisibilitySet(0, false);
		if (GameplayData.HasDepositedSomething())
		{
			GameplayMaster.FailsafeOverDeposit_SetTriggered();
		}
		if (PowerupScript.list_EquippedNormal.Count > 0)
		{
			GameplayMaster.FailsafeCharms_SetTriggered();
		}
		ScreenMenuScript.Close(false);
	}

	// Token: 0x060008AC RID: 2220 RVA: 0x00038D12 File Offset: 0x00036F12
	private void _IntroMenuNewSeededGame()
	{
		if (!GameplayMaster.CanInputSeed())
		{
			Sound.Play_Unpausable("SoundMenuError", 1f, 1f);
			return;
		}
		this.gotoSeedInputMenu = true;
		ScreenMenuScript.Close(false);
	}

	// Token: 0x060008AD RID: 2221 RVA: 0x00038D3E File Offset: 0x00036F3E
	public void FadeIntro()
	{
		this.titleScreenHolder.SetActive(false);
		this.TranslateText(null);
		base.StartCoroutine(this.IntroAndTitleScreen_Coroutine());
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x00038D60 File Offset: 0x00036F60
	private void TranslateText(Controls.InputActionMap map)
	{
		if (Master.IsDemo)
		{
			if (Master.instance.GAME_PUBLIC_STATE == Master.GamePublicState.wishlistOrPrerelease)
			{
				this.developerLetterText.text = Translation.Get("INTRO_DEVELOPER_LETTER_DURING_DEVELOPMENT");
			}
			else
			{
				this.developerLetterText.text = Translation.Get("INTRO_DEVELOPER_LETTER_POST_RELEASE");
			}
		}
		else
		{
			this.developerLetterText.text = Translation.Get("INTRO_DEVELOPER_LETTER_PLAYTEST");
		}
		this.developerLetterPrompt.text = Translation.Get("MENU_OPTION_CONTINUE") + " " + Controls.MapGetLastPrompt_TextSprite(0, Controls.InputAction.menuSelect, true, 0);
		if (GeneralUiScript.instance.textTitleStart != null)
		{
			string text = Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("MENU_OPTION_START_PROMPT"), Strings.SanitizationSubKind.none);
			GeneralUiScript.instance.textTitleStart.text = text;
			GeneralUiScript.instance.textTitleStart.ForceMeshUpdate(false, false);
		}
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x00038E30 File Offset: 0x00037030
	public static void CoinUiForceShow(float time = 3f)
	{
		if (GeneralUiScript.instance == null)
		{
			return;
		}
		GeneralUiScript.instance.coinsScreenKeepTimer = time;
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x00038E4B File Offset: 0x0003704B
	public static void CoinsTextForceUpdate()
	{
		if (GeneralUiScript.instance == null)
		{
			return;
		}
		GeneralUiScript.instance.coinsUpdateAgain = true;
		if (GeneralUiScript.instance.coinsAddTimer > 0f)
		{
			GeneralUiScript.instance.coinsAddTimer = 0f;
		}
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x00038E86 File Offset: 0x00037086
	public static void CoinsTextInstantUpdate()
	{
		if (GeneralUiScript.instance == null)
		{
			return;
		}
		GeneralUiScript.instance.coinsInstantUpdate = true;
		GeneralUiScript.CoinsTextForceUpdate();
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x00038EA6 File Offset: 0x000370A6
	public static void TicketsForceShow(float time = 3f)
	{
		if (GeneralUiScript.instance == null)
		{
			return;
		}
		GeneralUiScript.instance.coinsScreenKeepTimer = time;
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x00038EC1 File Offset: 0x000370C1
	public static void TicketsTextForceUpdate()
	{
		if (GeneralUiScript.instance == null)
		{
			return;
		}
		GeneralUiScript.instance.ticketsUpdateAgain = true;
		if (GeneralUiScript.instance.ticketsAddTimer > 0f)
		{
			GeneralUiScript.instance.ticketsAddTimer = 0f;
		}
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x00038EFC File Offset: 0x000370FC
	private void VersionTextRefresh()
	{
		this.textGameVersion.text = GeneralUiScript.GameVersionString_Get();
		if (GameplayMaster.IsCustomSeed())
		{
			TextMeshProUGUI textMeshProUGUI = this.textGameVersion;
			textMeshProUGUI.text = string.Concat(new string[]
			{
				textMeshProUGUI.text,
				" - <color=red>",
				Translation.Get("MENU_LABEL_SEED_DOUBLE_DOT"),
				GameplayData.SeedGetAsString(),
				"</color>"
			});
		}
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x00038F66 File Offset: 0x00037166
	public static string GameVersionString_Get()
	{
		return (Master.IsDemo ? "Demo " : "") + "v" + Application.version;
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x00038F8A File Offset: 0x0003718A
	private void Awake()
	{
		GeneralUiScript.instance = this;
		this.Initialize();
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x00038F98 File Offset: 0x00037198
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		this.holder.SetActive(false);
		this.developerLetterHolder.SetActive(false);
		Controls.onPromptsUpdateRequest = (Controls.MapCallback)Delegate.Combine(Controls.onPromptsUpdateRequest, new Controls.MapCallback(this.TranslateText));
		this.VersionTextRefresh();
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.VersionTextRefresh));
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x00039010 File Offset: 0x00037210
	private void OnDestroy()
	{
		Controls.onPromptsUpdateRequest = (Controls.MapCallback)Delegate.Remove(Controls.onPromptsUpdateRequest, new Controls.MapCallback(this.TranslateText));
		if (GeneralUiScript.instance == this)
		{
			GeneralUiScript.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.VersionTextRefresh));
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x00039070 File Offset: 0x00037270
	private void Update()
	{
		if (!Tick.IsGameRunning)
		{
			return;
		}
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
		bool flag = true;
		if (GameplayMaster.instance == null)
		{
			flag = false;
		}
		if (this.holder.activeSelf != flag)
		{
			this.holder.SetActive(flag);
		}
		this.titleScreenRectTransform.anchoredPosition = new global::UnityEngine.Vector2(0f, Util.AngleSin(Tick.PassedTime * 45f) * 5f);
		bool flag2 = false;
		if (MainMenuScript.IsEnabled())
		{
			flag2 = true;
		}
		if (flag2 != this.textGameVersion.enabled)
		{
			this.textGameVersion.enabled = flag2;
		}
		bool flag3 = false;
		BigInteger bigInteger = GameplayData.CoinsGet();
		if (bigInteger != this.coinsOld || this.coinsUpdateAgain)
		{
			this.coinsAddTimer -= Tick.Time;
			if (this.coinsAddTimer <= 0f)
			{
				if (this.coinsInstantUpdate)
				{
					this.coinsAddTimer += 0.1f;
				}
				else
				{
					this.coinsAddTimer += 0.05f;
				}
				BigInteger bigInteger2 = bigInteger - this.coinsOld;
				BigInteger bigInteger3 = bigInteger2;
				if (bigInteger3 < 0L)
				{
					bigInteger3 *= -1;
				}
				BigInteger bigInteger4 = 0;
				int sign = bigInteger2.Sign;
				if (bigInteger3 > 2147483647L || this.coinsInstantUpdate)
				{
					bigInteger4 = sign * bigInteger3;
				}
				else if (bigInteger3 > 100000000L)
				{
					bigInteger4 = sign * 100000000;
				}
				else if (bigInteger3 > 10000000L)
				{
					bigInteger4 = sign * 10000000;
				}
				else if (bigInteger3 > 1000000L)
				{
					bigInteger4 = sign * 1000000;
				}
				else if (bigInteger3 > 100000L)
				{
					bigInteger4 = sign * 100000;
				}
				else if (bigInteger3 > 10000L)
				{
					bigInteger4 = sign * 10000;
				}
				else if (bigInteger3 > 1000L)
				{
					bigInteger4 = sign * 1000;
				}
				else if (bigInteger3 > 100L)
				{
					bigInteger4 = sign * 100;
				}
				else if (bigInteger3 > 15L)
				{
					bigInteger4 = sign * 10;
				}
				else if (bigInteger3 > 0L)
				{
					bigInteger4 = sign;
				}
				this.textCoin.text = this.coinsOld.ToStringSmart() + " <sprite name=\"CoinSymbolOrange32\">";
				if (bigInteger3 > 0L)
				{
					TextMeshProUGUI textMeshProUGUI = this.textCoin;
					textMeshProUGUI.text = string.Concat(new string[]
					{
						textMeshProUGUI.text,
						"   ",
						(bigInteger2 < 0L) ? "-" : "+",
						bigInteger3.ToStringSmart(),
						"<sprite name=\"CoinSymbolOrange32\">"
					});
				}
				this.coinsOld += bigInteger4;
				if (!this.coinsUpdateAgain)
				{
					if (bigInteger2 > 0L)
					{
						this.coinsAddTimer += 1f;
					}
					else if (bigInteger2 < 0L)
					{
						if (gamePhase == GameplayMaster.GamePhase.gambling)
						{
							this.coinsAddTimer += 0.25f;
						}
						else
						{
							this.coinsAddTimer += 0.05f;
						}
					}
					if (bigInteger4 > 0L)
					{
						this.coinsTextOffset.y = 5f;
					}
					else if (bigInteger4 < 0L)
					{
						this.coinsTextOffset.y = -5f;
					}
				}
				this.coinsUpdateAgain = bigInteger3 > 0L;
				flag3 = true;
				this.coinsInstantUpdate = false;
			}
		}
		if (GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.gambling)
		{
			int num = GameplayData.SpinsLeftGet();
			if (num != this.spinsLeftOld || flag3)
			{
				TextMeshProUGUI textMeshProUGUI = this.textCoin;
				textMeshProUGUI.text = string.Concat(new string[]
				{
					textMeshProUGUI.text,
					"\n",
					Translation.Get("DIALOGUE_SLOT_BET_WORD_SPINS_LEFT"),
					" ",
					num.ToString()
				});
				this.spinsLeftOld = num;
			}
		}
		this.coinsScreenKeepTimer -= Tick.Time;
		global::UnityEngine.Vector2 coins_HOLDER_DEFAULT_POSITION = this.COINS_HOLDER_DEFAULT_POSITION;
		bool flag4 = true;
		bool flag5 = gamePhase == GameplayMaster.GamePhase.intro || gamePhase == GameplayMaster.GamePhase.closingGame || gamePhase == GameplayMaster.GamePhase.endingWithoutDeath;
		bool flag6 = !GameplayMaster.IsIntroDialogueFinished() && !TutorialScript.IsEnabled();
		bool flag7 = CameraController.instance == null || CameraController.GetPositionKind() == CameraController.PositionKind.TrapDoor || CameraController.GetPositionKind() == CameraController.PositionKind.Falling;
		bool flag8 = !CameraController.SlotMachineLookingFrontOrUndefined();
		bool flag9 = ToyPhoneUIScript.IsEnabled() || MagazineUiScript.IsEnabled() || DeckBoxUI.IsEnabled();
		if (flag5 || flag7 || flag8 || flag9)
		{
			this.coinsScreenKeepTimer = 0f;
		}
		if ((flag5 || flag6 || flag7 || flag8 || flag9) && this.coinsScreenKeepTimer <= 0f)
		{
			flag4 = false;
			coins_HOLDER_DEFAULT_POSITION.y = 100f;
		}
		this.coinsHolder.anchoredPosition = global::UnityEngine.Vector2.Lerp(this.coinsHolder.anchoredPosition, coins_HOLDER_DEFAULT_POSITION, Tick.Time * 10f);
		if (flag4 && this.coinsScreenKeepTimer < -0.5f)
		{
			this.coinsScreenKeepTimer = 3f;
		}
		global::UnityEngine.Vector2 vector;
		vector.x = this.textCoin.renderedWidth + 40f;
		vector.y = this.textCoin.preferredHeight + 20f;
		this.coinsTextBackImage.rectTransform.sizeDelta = global::UnityEngine.Vector2.Lerp(this.coinsTextBackImage.rectTransform.sizeDelta, vector, Tick.Time * 20f);
		global::UnityEngine.Vector2 zero = new global::UnityEngine.Vector2(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f));
		if (Data.settings.dyslexicFontEnabled)
		{
			zero = global::UnityEngine.Vector2.zero;
		}
		this.coinsTextBackImage.rectTransform.anchoredPosition = zero;
		this.coinsTextOffset = global::UnityEngine.Vector2.Lerp(this.coinsTextOffset, this.COINS_TEXT_OFFSET, Tick.Time * 10f);
		this.textCoin.rectTransform.anchoredPosition = new global::UnityEngine.Vector2(this.coinsTextOffset.x - zero.x, this.coinsTextOffset.y - zero.y);
		bool flag10 = InspectorScript.IsEnabled();
		if (this.textCoin.enableAutoSizing != flag10)
		{
			this.textCoin.enableAutoSizing = flag10;
		}
		long num2 = GameplayData.CloverTicketsGet();
		if (this.ticketsOld != num2 || this.ticketsUpdateAgain)
		{
			this.ticketsAddTimer -= Tick.Time;
			if (this.ticketsAddTimer <= 0f)
			{
				this.ticketsAddTimer += 0.05f;
				long num3 = num2 - this.ticketsOld;
				long num4 = num3;
				if (num4 < 0L)
				{
					num4 *= -1L;
				}
				long num5 = 0L;
				int num6 = ((num3 > 0L) ? 1 : (-1));
				if (num4 > 100000000L)
				{
					num5 = (long)(num6 * 100000000);
				}
				else if (num4 > 10000000L)
				{
					num5 = (long)(num6 * 10000000);
				}
				else if (num4 > 1000000L)
				{
					num5 = (long)(num6 * 1000000);
				}
				else if (num4 > 100000L)
				{
					num5 = (long)(num6 * 100000);
				}
				else if (num4 > 10000L)
				{
					num5 = (long)(num6 * 10000);
				}
				else if (num4 > 1000L)
				{
					num5 = (long)(num6 * 1000);
				}
				else if (num4 > 100L)
				{
					num5 = (long)(num6 * 100);
				}
				else if (num4 > 15L)
				{
					num5 = (long)(num6 * 10);
				}
				else if (num4 > 0L)
				{
					num5 = (long)num6;
				}
				this.textTickets.text = this.ticketsOld.ToString() + " <sprite name=\"CloverTicket\">";
				if (num4 > 0L)
				{
					this.textTickets.text = string.Concat(new string[]
					{
						num4.ToString(),
						"<sprite name=\"CloverTicket\">",
						(num3 < 0L) ? "-" : "+",
						"   ",
						this.textTickets.text
					});
				}
				this.ticketsOld += num5;
				if (!this.ticketsUpdateAgain)
				{
					if (num3 > 0L)
					{
						this.ticketsAddTimer += 3f;
					}
					else if (num3 < 0L)
					{
						this.ticketsAddTimer += 0f;
					}
					if (num5 > 0L)
					{
						this.ticketsTextOffset.y = 5f;
					}
					else if (num5 < 0L)
					{
						this.ticketsTextOffset.y = -5f;
					}
				}
				this.ticketsUpdateAgain = num4 > 0L;
			}
		}
		this.ticketsScreenKeepTimer -= Tick.Time;
		global::UnityEngine.Vector2 tickets_HOLDER_DEFAULT_POSITION = this.TICKETS_HOLDER_DEFAULT_POSITION;
		bool flag11 = true;
		bool flag12 = gamePhase == GameplayMaster.GamePhase.gambling && (!CameraController.SlotMachineLookingFrontOrUndefined() || (SlotMachineScript.instance.leverMenuElement.IsHovered() && !SlotMachineScript.IsSpinning()));
		if (flag5 || flag7 || flag12 || flag9)
		{
			this.ticketsScreenKeepTimer = 0f;
		}
		if ((flag5 || flag6 || flag7 || flag12 || flag9) && this.ticketsScreenKeepTimer <= 0f)
		{
			flag11 = false;
			tickets_HOLDER_DEFAULT_POSITION.y = 100f;
		}
		this.ticketsHolder.anchoredPosition = global::UnityEngine.Vector2.Lerp(this.ticketsHolder.anchoredPosition, tickets_HOLDER_DEFAULT_POSITION, Tick.Time * 10f);
		if (flag11 && this.ticketsScreenKeepTimer < -0.5f)
		{
			this.ticketsScreenKeepTimer = 3f;
		}
		global::UnityEngine.Vector2 vector2;
		vector2.x = this.textTickets.renderedWidth + 40f;
		vector2.y = this.textTickets.preferredHeight + 20f;
		this.ticketsTextBackImage.rectTransform.sizeDelta = global::UnityEngine.Vector2.Lerp(this.ticketsTextBackImage.rectTransform.sizeDelta, vector2, Tick.Time * 20f);
		global::UnityEngine.Vector2 zero2 = new global::UnityEngine.Vector2(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f));
		if (Data.settings.dyslexicFontEnabled)
		{
			zero2 = global::UnityEngine.Vector2.zero;
		}
		this.ticketsTextBackImage.rectTransform.anchoredPosition = zero2;
		this.ticketsTextOffset = global::UnityEngine.Vector2.Lerp(this.ticketsTextOffset, this.TICKETS_TEXT_OFFSET, Tick.Time * 10f);
		this.textTickets.rectTransform.anchoredPosition = new global::UnityEngine.Vector2(this.ticketsTextOffset.x - zero2.x, this.ticketsTextOffset.y - zero2.y);
		bool flag13 = InspectorScript.IsEnabled();
		if (this.textTickets.enableAutoSizing != flag13)
		{
			this.textTickets.enableAutoSizing = flag13;
		}
	}

	public static GeneralUiScript instance;

	private const int PLAYER_INDEX = 0;

	private const float TITLE_SCREEN_FADE_IN_SPEED = 1f;

	private const float TITLE_SCREEN_FADE_OUT_SPEED = 1f;

	private const float FADE_IN_SPEED = 0.5f;

	private const float COINS_SCREEN_KEEP_TIME = 3f;

	private const float COINS_UPDATE_TIMER_RESET = 0.05f;

	private const float COINS_UPDATE_TIMER_RESET_LONG = 0.1f;

	private global::UnityEngine.Vector2 COINS_HOLDER_DEFAULT_POSITION = new global::UnityEngine.Vector2(10f, -10f);

	private global::UnityEngine.Vector2 COINS_TEXT_OFFSET = new global::UnityEngine.Vector2(20f, -13f);

	private const float TICKETS_SCREEN_KEEP_TIME = 3f;

	private const float TICKETS_UPDATE_TIMER_RESET = 0.05f;

	private global::UnityEngine.Vector2 TICKETS_HOLDER_DEFAULT_POSITION = new global::UnityEngine.Vector2(-10f, -10f);

	private global::UnityEngine.Vector2 TICKETS_TEXT_OFFSET = new global::UnityEngine.Vector2(-18f, -13f);

	private Controls.PlayerExt player;

	public GameObject holder;

	public Image fadeImage;

	public GameObject developerLetterHolder;

	public RectTransform developerLetterShifter;

	public TextMeshProUGUI developerLetterText;

	public TextMeshProUGUI developerLetterPrompt;

	public GameObject titleScreenHolder;

	private RectTransform titleScreenRectTransform;

	public TextMeshProUGUI textTitleStart;

	public RectTransform coinsHolder;

	public TextMeshProUGUI textCoin;

	public Image coinsTextBackImage;

	public RectTransform ticketsHolder;

	public TextMeshProUGUI textTickets;

	public Image ticketsTextBackImage;

	public RectTransform gameVersionHolder;

	public TextMeshProUGUI textGameVersion;

	private bool fadedIn;

	private bool titleScreenShowing;

	private static bool requestedNewSessionReset;

	private Color fadeImageColor = new Color(0f, 0f, 0f, 1f);

	private static bool comingFromVictory;

	private bool gotoSeedInputMenu;

	private BigInteger coinsOld = 0;

	private int spinsLeftOld;

	private float coinsAddTimer;

	private bool coinsUpdateAgain;

	private global::UnityEngine.Vector2 coinsTextOffset = global::UnityEngine.Vector2.zero;

	private float coinsScreenKeepTimer;

	private bool coinsInstantUpdate;

	private long ticketsOld;

	private float ticketsAddTimer;

	private bool ticketsUpdateAgain;

	private global::UnityEngine.Vector2 ticketsTextOffset = global::UnityEngine.Vector2.zero;

	private float ticketsScreenKeepTimer;
}
