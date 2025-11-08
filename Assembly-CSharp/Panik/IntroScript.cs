using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Panik
{
	public class IntroScript : MonoBehaviour
	{
		// Token: 0x06000DBB RID: 3515 RVA: 0x00055E40 File Offset: 0x00054040
		private void PromptsUpdate(Controls.InputActionMap map)
		{
			this.selectPromptSpriteString = Controls.MapGetLastPrompt_TextSprite(0, Controls.InputAction.menuSelect, true, 0);
			this.autosaveWarningPrompt.text = Translation.Get("UI_PROMPT_CONTINUE") + " " + this.selectPromptSpriteString;
			this.popupPrompt.text = Translation.Get("UI_PROMPT_CONTINUE") + " " + this.selectPromptSpriteString;
			this.popupAnswerPrompt_Yes.text = Translation.Get("MENU_OPTION_YES");
			this.popupAnswerPrompt_No.text = Translation.Get("MENU_OPTION_NO");
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x00055ED0 File Offset: 0x000540D0
		private IEnumerator IntroCoroutine()
		{
			this.PromptsUpdate(null);
			Controls.onPromptsUpdateRequest = (Controls.MapCallback)Delegate.Combine(Controls.onPromptsUpdateRequest, new Controls.MapCallback(this.PromptsUpdate));
			this.languageSelectionHolder.gameObject.SetActive(false);
			TextMeshProUGUI[] array = this.languagesButtons;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].text = "";
			}
			this.popUpHolder.SetActive(false);
			this.autosaveWarningHolder.SetActive(false);
			this.publisherIntroHolder.SetActive(false);
			this.developerIntroHolder.SetActive(false);
			this.musicianIntroHolder.SetActive(false);
			this.developerVideoRawImage.color = new Color(1f, 1f, 1f, 0f);
			this.publisherVideoRawImage.color = this.developerVideoRawImage.color;
			yield return new WaitForSeconds(1f);
			float timer;
			if (Master.instance.ENGLISH_ONLY_BUILD)
			{
				Translation.LanguageSet(Translation.Language.English);
			}
			else if (!Data.settings.initialLanguageSelectionPerfromed)
			{
				VirtualCursors.CursorDesiredVisibilitySet(0, true);
				float num = 37f * (float)Translation.LanguagesInOrder.Length;
				this.languageSelectionHolder.gameObject.SetActive(true);
				this.languageSelectionHolder.anchoredPosition = new Vector2(0f, -333f + num);
				this.languageTitle.text = Translation.Get("SCREEN_MENU_TITLE_PICK_A_LANGUAGE");
				for (int j = 0; j < Translation.LanguagesInOrder.Length; j++)
				{
					this.languagesButtons[j].text = Translation.LanguageNameGetTranslated(Translation.LanguagesInOrder[j]);
				}
				timer = 0f;
				while (timer < 1f)
				{
					timer += Tick.Time;
					yield return null;
				}
				int langIndex = -1;
				int langIndexOld = -1;
				for (;;)
				{
					bool flag = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true);
					if (VirtualCursors.IsCursorVisible(0, true))
					{
						langIndex = -1;
						Vector2 vector = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, this.myCanvasScaler.referenceResolution);
						for (int k = 0; k < this.languagesButtons.Length; k++)
						{
							TextMeshProUGUI textMeshProUGUI = this.languagesButtons[k];
							RectTransform rectTransform = this.languagesButtons[k].rectTransform;
							if (vector.x < rectTransform.anchoredPosition.x + textMeshProUGUI.preferredWidth / 2f && vector.x > rectTransform.anchoredPosition.x - textMeshProUGUI.preferredWidth / 2f && vector.y < rectTransform.anchoredPosition.y + this.languageSelectionHolder.anchoredPosition.y + rectTransform.sizeDelta.y / 2f && vector.y > rectTransform.anchoredPosition.y + this.languageSelectionHolder.anchoredPosition.y - rectTransform.sizeDelta.y / 2f)
							{
								langIndex = k;
								break;
							}
						}
					}
					else
					{
						float num2 = 0f;
						if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuMoveUp, true))
						{
							num2 += 1f;
						}
						if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuMoveDown, true))
						{
							num2 -= 1f;
						}
						if (langIndex < 0)
						{
							if (Mathf.Abs(num2) > 0.35f || flag)
							{
								langIndex = 0;
								flag = false;
							}
						}
						else
						{
							if (num2 < -0.35f)
							{
								int i = langIndex;
								langIndex = i + 1;
							}
							if (num2 > 0.35f)
							{
								int i = langIndex;
								langIndex = i - 1;
							}
							if (langIndex < 0)
							{
								langIndex = 0;
							}
							if (langIndex > Translation.LanguagesInOrder.Length - 1)
							{
								langIndex = Translation.LanguagesInOrder.Length - 1;
							}
						}
					}
					bool flag2 = langIndex >= 0 && flag;
					if (langIndex >= 0 && langIndex != langIndexOld)
					{
						Sound.Play("SoundMenuSelectionChange", 1f, 1f);
					}
					if (flag2)
					{
						Sound.Play("SoundMenuSelect", 1f, 1f);
					}
					for (int l = 0; l < this.languagesButtons.Length; l++)
					{
						this.languagesButtons[l].alpha = ((langIndex == l) ? 1f : 0.25f);
					}
					langIndexOld = langIndex;
					if (flag2)
					{
						break;
					}
					yield return null;
				}
				Translation.LanguageSet(Translation.LanguagesInOrder[langIndex]);
				Data.settings.initialLanguageSelectionPerfromed = true;
				if (Translation.LanguagesInOrder[langIndex] == Translation.Language.French)
				{
					while (Data.settings.KeyboardLayoutGet() != Data.SettingsData.KeyboardLayout.keyboard_AZERTY)
					{
						Controls.KeyboardLayoutNext();
					}
				}
				Data.SaveSettings();
				this.PromptsUpdate(null);
				VirtualCursors.CursorDesiredVisibilitySet(0, false);
				this.languageSelectionHolder.gameObject.SetActive(false);
			}
			if (Data.settingsResetFlag)
			{
				yield return this.PopUpShow(Translation.Get("INTRO_POPUP_SETTINGS_CHANGED_TITLE"), Translation.Get("INTRO_POPUP_SETTINGS_CHANGED_DESCR"), 0.5f, 1f, false, null, null);
			}
			if (Data.publisherBuildFlag_FromFirstToSecond)
			{
				yield return this.PopUpShow(Translation.Get("INTRO_POPUP_PUBLISHER_BUILD_DATA_CHANGE_1_TO_2_TITLE"), Translation.Get("INTRO_POPUP_PUBLISHER_BUILD_DATA_CHANGE_1_TO_2_DESCR"), 0.5f, 1f, false, null, null);
			}
			if (Data.redButtonChange_ShowPopUps_ForV0_4)
			{
				Data.game.enforceRunReset = true;
				Data.SaveGame(Data.GameSavingReason.setFlag_RunForceReset, -1);
				yield return this.PopUpShow(Translation.Get("INTRO_POPUP_RED_BUTTON_CHARMS_CHANGE_TITLE"), Translation.Get("INTRO_POPUP_RED_BUTTON_CHARMS_CHANGE_DESCR"), 0.5f, 1f, false, null, null);
				yield return this.PopUpShow(Translation.Get("INTRO_POPUP_GAME_SOFT_RESET_TITLE"), Translation.Get("INTRO_POPUP_GAME_SOFT_RESET_DESCR"), 0.5f, 1f, false, null, null);
			}
			if (PlatformMaster.PlatformIsComputer())
			{
				bool flag3 = false;
				string gameFolderPath = PlatformDataMaster.GameFolderPath;
				string text = PlatformDataMaster.PathGet_GameDataFile(0, "");
				string gameDir_Demo = PlatformDataMaster.OutsideExecutablePath + "CloverPit Demo/SaveData/GameData/";
				string gameDataPath_Demo = gameDir_Demo + "GameDataDemo.json";
				if (!Directory.Exists(gameFolderPath) || !File.Exists(text))
				{
					yield return this.PopUpShow(Translation.Get("INTRO_POPUP_QUESTION_DO_YOU_WANT_TO_IMPORT_DEMO_DATA_TITLE"), Translation.Get("INTRO_POPUP_QUESTION_DO_YOU_WANT_TO_IMPORT_DEMO_DATA_DESCR_NEW_1"), 0.5f, 1f, true, null, null);
					flag3 = this.popupQuestionAnswer == 0;
				}
				if (flag3)
				{
					(new string[1])[0] = "json";
					for (;;)
					{
						bool success = false;
						bool cursorOldState = Cursor.visible;
						CursorLockMode oldLockState = Cursor.lockState;
						Cursor.visible = true;
						Cursor.lockState = CursorLockMode.None;
						yield return null;
						try
						{
							string text2 = null;
							if (!string.IsNullOrEmpty(gameDir_Demo) && Directory.Exists(gameDir_Demo) && !string.IsNullOrEmpty(gameDataPath_Demo) && File.Exists(gameDataPath_Demo))
							{
								text2 = File.ReadAllText(gameDataPath_Demo);
							}
							if (!string.IsNullOrEmpty(text2))
							{
								bool flag4 = false;
								string text3 = null;
								Data.GameData gameData = PlatformDataMaster.FromJsonExt<Data.GameData>(text2, out flag4, out text3);
								if (flag4)
								{
									for (int m = 3; m >= 0; m--)
									{
										string text4 = Data.PGameDataGet_LastOne(m);
										gameData = PlatformDataMaster.FromJsonExt<Data.GameData>(Data.Decrypt_Wrapped(text2, text4), out flag4, out text3);
										if (!flag4)
										{
											break;
										}
									}
								}
								if (gameData != null && !flag4)
								{
									gameData.Loading_Prepare();
									if (gameData.demoVoucherUnlocked)
									{
										Data.game.demoVoucherUnlocked = true;
										if (!Data.game.drawersUnlocked[0])
										{
											Data.game.drawersUnlocked[0] = true;
										}
									}
									List<PowerupScript.Identifier> list = gameData._UnlockedPowerups_GetList();
									for (int n = 0; n < list.Count; n++)
									{
										PowerupScript.Identifier identifier = list[n];
										if (identifier != PowerupScript.Identifier.undefined && identifier != PowerupScript.Identifier.count)
										{
											Data.game.LockedPowerups_Unlock(identifier);
										}
									}
									success = true;
									GameplayMaster.drawerFromDemoUnlocked = true;
								}
								if (flag4 && !string.IsNullOrEmpty(text3) && PlatformMaster.PlatformIsComputer())
								{
									ConsolePrompt.LogWarning(text3, "");
								}
							}
						}
						catch (Exception ex)
						{
							string text5 = "Error while looking for Demo data. Error: " + ex.Message;
							Debug.LogError(text5);
							if (PlatformMaster.PlatformIsComputer())
							{
								ConsolePrompt.LogError(text5, "", 0f);
							}
							success = false;
						}
						Cursor.visible = cursorOldState;
						Cursor.lockState = oldLockState;
						if (success)
						{
							goto IL_0997;
						}
						yield return this.PopUpShow(Translation.Get("INTRO_POPUP_QUESTION_DEMO_DATA_FAIL_TITLE"), Translation.Get("INTRO_POPUP_QUESTION_DEMO_DATA_FAIL_DESCR_NEW_1"), 0.5f, 1f, true, null, null);
						if (this.popupQuestionAnswer == 1)
						{
							break;
						}
						yield return null;
					}
					goto IL_09F4;
					IL_0997:
					yield return this.PopUpShow(Translation.Get("INTRO_POPUP_DEMO_DATA_SUCCESFULL_TITLE"), Translation.Get("INTRO_POPUP_DEMO_DATA_SUCCESFULL_DESCR"), 0.5f, 1f, false, null, null);
				}
				IL_09F4:
				gameDir_Demo = null;
				gameDataPath_Demo = null;
			}
			yield return this.PopUpShow(Translation.Get("INTRO_TEXT_SEIZURE_WARNING_TITLE"), Translation.Get("INTRO_TEXT_SEIZURE_WARNING"), 0.5f, 1f, false, null, null);
			if (Master.instance.SHOW_AUTOSAVE_WARNING_STARTUP)
			{
				this.autosaveWarningTitle.text = Translation.Get("POP_UP_TITLE_WARNING");
				this.autosaveWarningBody.text = Translation.Get("INTRO_TEXT_SAVE_ICON_WARNING_EXPLANATION");
				this.autosaveWarningHolder.SetActive(true);
				Sound.Play_Unpausable("SoundMenuPopUp", 1f, 1f);
				while (!Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) || ConsolePrompt.ConsoleIsEnabled())
				{
					yield return null;
				}
				Sound.Play_Unpausable("SoundMenuSelect", 1f, 1f);
				yield return null;
				this.autosaveWarningHolder.SetActive(false);
			}
			yield return new WaitForSeconds(1f);
			this.developerIntroHolder.SetActive(true);
			this.developerVideoPlayer.Stop();
			this.developerVideoPlayer.Play();
			this.developerVideoPlayer.SetDirectAudioVolume(0, Sound.GetVolumeFinal(null));
			Color devIntroColor = this.developerVideoRawImage.color;
			while (devIntroColor.a < 1f)
			{
				devIntroColor.a += Tick.Time * 10f;
				this.developerVideoRawImage.color = devIntroColor;
				yield return null;
			}
			devIntroColor.a = 1f;
			this.developerVideoRawImage.color = devIntroColor;
			timer = 0f;
			while (timer < (float)this.developerVideoPlayer.clip.length + 0.1f)
			{
				timer += Tick.Time;
				if ((double)timer > 0.5 && Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) && !ConsolePrompt.ConsoleIsEnabled())
				{
					break;
				}
				yield return null;
			}
			this.developerVideoPlayer.Stop();
			this.developerIntroHolder.SetActive(false);
			yield return new WaitForSeconds(1f);
			this.publisherIntroHolder.SetActive(true);
			this.publisherVideoPlayer.Stop();
			this.publisherVideoPlayer.Play();
			this.publisherVideoPlayer.SetDirectAudioVolume(0, Sound.GetVolumeFinal(null));
			Color pubIntroColor = this.publisherVideoRawImage.color;
			while (pubIntroColor.a < 1f)
			{
				pubIntroColor.a += Tick.Time * 10f;
				this.publisherVideoRawImage.color = pubIntroColor;
				yield return null;
			}
			pubIntroColor.a = 1f;
			this.publisherVideoRawImage.color = pubIntroColor;
			timer = 0f;
			while (timer < (float)this.developerVideoPlayer.clip.length + 0.1f)
			{
				timer += Tick.Time;
				if ((double)timer > 0.5 && Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) && !ConsolePrompt.ConsoleIsEnabled())
				{
					break;
				}
				yield return null;
			}
			this.developerVideoPlayer.Stop();
			this.publisherIntroHolder.SetActive(false);
			yield return new WaitForSeconds(1f);
			pubIntroColor = default(Color);
			IL_0E7E:
			Controls.onPromptsUpdateRequest = (Controls.MapCallback)Delegate.Remove(Controls.onPromptsUpdateRequest, new Controls.MapCallback(this.PromptsUpdate));
			Level.GoTo(Level.SceneIndex.Game, false);
			yield break;
			IL_0E09:
			timer += Tick.Time;
			if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) && !ConsolePrompt.ConsoleIsEnabled())
			{
				goto IL_0E51;
			}
			yield return null;
			if (timer < 3f)
			{
				goto IL_0E09;
			}
			IL_0E51:
			this.musicianIntroHolder.SetActive(false);
			yield return new WaitForSeconds(1f);
			goto IL_0E7E;
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00055EE0 File Offset: 0x000540E0
		public IEnumerator PopUpShow(string title, string description, float inputDelay, float endDelay, bool isQuestion, UnityAction onYes, UnityAction onNo)
		{
			this.popupQuestionAnswer = -1;
			this.popupPrompt.enabled = false;
			this.popupAnswerPrompt_Yes.enabled = false;
			this.popupAnswerPrompt_No.enabled = false;
			this.popUpHolder.SetActive(true);
			Sound.Play_Unpausable("SoundMenuPopUp", 1f, 1f);
			this.popupTitle.text = title;
			this.popupDescr.text = description;
			float timer = 0f;
			while (timer < inputDelay)
			{
				timer += Tick.Time;
				yield return null;
			}
			if (!isQuestion)
			{
				this.popupPrompt.enabled = true;
				while (!Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true))
				{
					yield return null;
				}
				Sound.Play_Unpausable("SoundMenuSelect", 1f, 1f);
			}
			else
			{
				this.popupAnswerPrompt_Yes.enabled = true;
				this.popupAnswerPrompt_No.enabled = true;
				this.popupAnswerPrompt_Yes.color = Color.gray;
				this.popupAnswerPrompt_No.color = Color.gray;
				bool cursorPrevState = VirtualCursors.CursorDesiredVisibilityGet(0);
				VirtualCursors.CursorDesiredVisibilitySet(0, true);
				int selectionIndex = -1;
				int selectionIndexOld = -1;
				float ignoreSelectionTimer = 0.15f;
				for (;;)
				{
					float num = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.menuMoveRight, Controls.InputAction.menuMoveLeft, true);
					bool flag = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true);
					bool flag2 = ignoreSelectionTimer > 0f;
					ignoreSelectionTimer -= Tick.Time;
					Vector2 vector = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, this.myCanvasScaler.referenceResolution);
					if (VirtualCursors.IsCursorVisible(0, true))
					{
						selectionIndex = -1;
						if (vector.x < this.popupAnswerPrompt_Yes.rectTransform.anchoredPosition.x + this.popupAnswerPrompt_Yes.rectTransform.sizeDelta.x / 2f && vector.x > this.popupAnswerPrompt_Yes.rectTransform.anchoredPosition.x - this.popupAnswerPrompt_Yes.rectTransform.sizeDelta.x / 2f && vector.y < this.popupAnswerPrompt_Yes.rectTransform.anchoredPosition.y + this.popupAnswerPrompt_Yes.rectTransform.sizeDelta.y / 2f && vector.y > this.popupAnswerPrompt_Yes.rectTransform.anchoredPosition.y - this.popupAnswerPrompt_Yes.rectTransform.sizeDelta.y / 2f)
						{
							selectionIndex = 0;
						}
						else if (vector.x < this.popupAnswerPrompt_No.rectTransform.anchoredPosition.x + this.popupAnswerPrompt_No.rectTransform.sizeDelta.x / 2f && vector.x > this.popupAnswerPrompt_No.rectTransform.anchoredPosition.x - this.popupAnswerPrompt_No.rectTransform.sizeDelta.x / 2f && vector.y < this.popupAnswerPrompt_No.rectTransform.anchoredPosition.y + this.popupAnswerPrompt_No.rectTransform.sizeDelta.y / 2f && vector.y > this.popupAnswerPrompt_No.rectTransform.anchoredPosition.y - this.popupAnswerPrompt_No.rectTransform.sizeDelta.y / 2f)
						{
							selectionIndex = 1;
						}
					}
					else if (selectionIndex == -1)
					{
						if (flag || Mathf.Abs(num) > 0.35f)
						{
							flag2 = true;
							selectionIndex = 0;
						}
					}
					else if (num < -0.35f)
					{
						selectionIndex = 0;
					}
					else if (num > 0.35f)
					{
						selectionIndex = 1;
					}
					if (selectionIndex != selectionIndexOld)
					{
						if (selectionIndex >= 0)
						{
							Sound.Play("SoundMenuSelectionChange", 1f, 1f);
						}
						this.popupAnswerPrompt_Yes.color = ((selectionIndex == 0) ? Color.white : Color.gray);
						this.popupAnswerPrompt_No.color = ((selectionIndex == 1) ? Color.white : Color.gray);
					}
					selectionIndexOld = selectionIndex;
					if (flag && !flag2 && selectionIndex >= 0)
					{
						break;
					}
					yield return null;
				}
				int num2 = selectionIndex;
				if (num2 != 0)
				{
					if (num2 == 1)
					{
						this.popupQuestionAnswer = 1;
						if (onNo != null)
						{
							onNo();
						}
					}
				}
				else
				{
					this.popupQuestionAnswer = 0;
					if (onYes != null)
					{
						onYes();
					}
				}
				Sound.Play("SoundMenuSelect", 1f, 1f);
				VirtualCursors.CursorDesiredVisibilitySet(0, cursorPrevState);
			}
			this.popUpHolder.SetActive(false);
			timer = 0f;
			while (timer < endDelay)
			{
				timer += Tick.Time;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x00055F2F File Offset: 0x0005412F
		private void Awake()
		{
			IntroScript.instance = this;
			this.myCanvas = base.GetComponent<Canvas>();
			this.myCanvasScaler = base.GetComponent<CanvasScaler>();
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x00055F4F File Offset: 0x0005414F
		private void Start()
		{
			base.StartCoroutine(this.IntroCoroutine());
			VirtualCursors.CursorDesiredVisibilitySet(0, false);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00055F65 File Offset: 0x00054165
		private void OnDestroy()
		{
			if (IntroScript.instance == this)
			{
				IntroScript.instance = null;
			}
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x00055F7A File Offset: 0x0005417A
		private void OnDrawGizmosSelected()
		{
			this.myCanvasScaler.referencePixelsPerUnit = 64f;
		}

		public static IntroScript instance;

		private const int PLAYER_INDEX = 0;

		public const bool SKIP_PUBLISHER = false;

		public const bool SKIP_MUSICIAN = true;

		public const bool MUTE_DEV_VIDEO = false;

		private const float SELECTED_ALPHA = 1f;

		private const float UNSELECTED_ALPHA = 0.25f;

		public Canvas myCanvas;

		public CanvasScaler myCanvasScaler;

		public RectTransform languageSelectionHolder;

		public TextMeshProUGUI languageTitle;

		public TextMeshProUGUI[] languagesButtons;

		public GameObject autosaveWarningHolder;

		public TextMeshProUGUI autosaveWarningTitle;

		public TextMeshProUGUI autosaveWarningBody;

		public TextMeshProUGUI autosaveWarningPrompt;

		public GameObject publisherIntroHolder;

		public GameObject developerIntroHolder;

		public GameObject musicianIntroHolder;

		public VideoPlayer developerVideoPlayer;

		public RawImage developerVideoRawImage;

		public VideoPlayer publisherVideoPlayer;

		public RawImage publisherVideoRawImage;

		public GameObject popUpHolder;

		public TextMeshProUGUI popupTitle;

		public TextMeshProUGUI popupDescr;

		public TextMeshProUGUI popupPrompt;

		public TextMeshProUGUI popupAnswerPrompt_Yes;

		public TextMeshProUGUI popupAnswerPrompt_No;

		private string selectPromptSpriteString;

		private int popupQuestionAnswer = -1;
	}
}
