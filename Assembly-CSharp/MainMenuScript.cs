using System;
using System.Collections;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
	// (get) Token: 0x060008EF RID: 2287 RVA: 0x0003AF2F File Offset: 0x0003912F
	private static int INDEX_RESUME
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x0003AF32 File Offset: 0x00039132
	public static bool IsEnabled()
	{
		return !(MainMenuScript.instance == null) && MainMenuScript.instance._isEnabled;
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x0003AF50 File Offset: 0x00039150
	public static void Open()
	{
		if (MainMenuScript.IsEnabled())
		{
			return;
		}
		MemoScript.Close(false);
		MainMenuScript.instance._isEnabled = true;
		MainMenuScript.instance.menuController.OpenMe();
		MainMenuScript.instance.menuIndex = MainMenuScript.MenuIndex.mainMenu;
		MainMenuScript.instance.OptionsUpdate();
		VirtualCursors.CursorPositionNormalizedSet(0, new Vector2(0f, 0f), true);
		MainMenuScript.instance.inputDelay = 0.5f;
		if (GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.preparation)
		{
			Data.SaveGame(Data.GameSavingReason.mainMenuOpened_NotDuringSlotMachine, -1);
		}
		MainMenuScript.instance.saveSettingsOnClose = false;
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x0003AFDA File Offset: 0x000391DA
	public static void Close()
	{
		if (!MainMenuScript.IsEnabled())
		{
			return;
		}
		MainMenuScript.instance._isEnabled = false;
		MainMenuScript.instance.menuController.Back();
		if (MainMenuScript.instance.saveSettingsOnClose)
		{
			Data.SaveSettings();
		}
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x0003B010 File Offset: 0x00039210
	public static void BackInput()
	{
		GameplayMaster.instance.FCall_MenuDrawer_MainMenu_CloseTry();
		Sound.Play("SoundMenuBack", 1f, 1f);
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x0003B031 File Offset: 0x00039231
	public bool CanMenuPerform(bool includeDelay)
	{
		return (!includeDelay || this.inputDelay <= 0f) && this.dataErasureCoroutine == null && (MainMenuScript.IsEnabled() && !ScreenMenuScript.IsEnabled()) && !DialogueScript.IsEnabled();
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x0003B068 File Offset: 0x00039268
	private void LeftRightNavigationUpdate()
	{
		this.rightNavigationPress = false;
		this.leftNavigationPress = false;
		float num = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.menuMoveRight, Controls.InputAction.menuMoveLeft, true);
		if (Mathf.Abs(num) < 0.75f)
		{
			num = 0f;
		}
		if (this.leftRightAxisPrevious != 0f)
		{
			this.leftRightAxisPrevious = num;
			return;
		}
		this.leftRightAxisPrevious = num;
		if (num > 0.75f)
		{
			this.rightNavigationPress = true;
		}
		if (num < -0.75f)
		{
			this.leftNavigationPress = true;
		}
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x0003B0DA File Offset: 0x000392DA
	public bool LeftRightNavigationHasBeenPressed()
	{
		return this.leftNavigationPress || this.rightNavigationPress;
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x0003B0EC File Offset: 0x000392EC
	public void Select(int selectionIndex)
	{
		this.Select(this.menuIndex, selectionIndex);
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0003B0FB File Offset: 0x000392FB
	public void Select(MainMenuScript.MenuIndex _menuIndex, int selectionIndex)
	{
		this.Select_Desktop(_menuIndex, selectionIndex);
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0003B108 File Offset: 0x00039308
	public void Select_Desktop(MainMenuScript.MenuIndex _menuIndex, int selectionIndex)
	{
		if (!this.CanMenuPerform(true))
		{
			return;
		}
		bool flag = Controls.MouseButton_PressedGet(0, Controls.MouseElement.RightButton);
		bool flag2 = this.rightNavigationPress;
		bool flag3 = this.leftNavigationPress;
		int num = 1;
		if (flag || flag3)
		{
			num = -1;
		}
		this.desiredNavigationIndex = -1;
		switch (_menuIndex)
		{
		case MainMenuScript.MenuIndex.mainMenu:
			switch (selectionIndex)
			{
			case 0:
				if (!flag && !flag2 && !flag3)
				{
					MainMenuScript.BackInput();
				}
				break;
			case 1:
				if (!flag && !flag2 && !flag3)
				{
					Sound.Play("SoundMenuSelect", 1f, 1f);
					DialogueScript.Close();
					DialogueScript.NextIsLegalDuringDeathCooldown();
					DialogueScript.SetQuestionDialogue(false, new DialogueScript.AnswerCallback(this._RestartYes), new DialogueScript.AnswerCallback(this._RestartNo), new string[] { "DIALOGUE_MAIN_MENU_RESTART_ARE_YOU_SURE" });
				}
				break;
			case 2:
				if (!flag && !flag2 && !flag3)
				{
					Sound.Play("SoundMenuSelect", 1f, 1f);
					this.menuIndex = MainMenuScript.MenuIndex.settings;
					this.desiredNavigationIndex = 0;
				}
				break;
			case 3:
				if (!flag && !flag2 && !flag3)
				{
					this.MFunc_ResetTutorial();
				}
				break;
			case 4:
				if (!flag && !flag2 && !flag3)
				{
					Sound.Play("SoundMenuSelect", 1f, 1f);
					DialogueScript.NextIsLegalDuringDeathCooldown();
					DialogueScript.SetQuestionDialogue(false, new DialogueScript.AnswerCallback(this._QuitYes), new DialogueScript.AnswerCallback(this._QuitNo), new string[] { GameplayMaster.IsCustomSeed() ? "DIALOGUE_MAIN_MENU_CLOSE_GAME_ARE_YOU_SURE_SEEDED_RUN" : "DIALOGUE_MAIN_MENU_CLOSE_GAME_ARE_YOU_SURE" });
				}
				break;
			}
			break;
		case MainMenuScript.MenuIndex.settings:
			if (TwitchMaster.IsTwitchSupported())
			{
				switch (selectionIndex)
				{
				case 0:
					if (!flag && !flag2 && !flag3)
					{
						Sound.Play("SoundMenuSelect", 1f, 1f);
						this.menuIndex = MainMenuScript.MenuIndex.settingsAccessiblity;
						this.desiredNavigationIndex = 0;
					}
					break;
				case 1:
					if (!flag && !flag2 && !flag3)
					{
						Sound.Play("SoundMenuSelect", 1f, 1f);
						this.menuIndex = MainMenuScript.MenuIndex.settingsVideoAndAudio;
						this.desiredNavigationIndex = 0;
					}
					break;
				case 2:
					if (!flag && !flag2 && !flag3)
					{
						Sound.Play("SoundMenuSelect", 1f, 1f);
						this.menuIndex = MainMenuScript.MenuIndex.settingsOthers;
						this.desiredNavigationIndex = 0;
					}
					break;
				case 3:
					if (!flag && !flag2 && !flag3)
					{
						Sound.Play("SoundMenuSelect", 1f, 1f);
						DialogueScript.NextIsLegalDuringDeathCooldown();
						DialogueScript.SetQuestionDialogue(false, new DialogueScript.AnswerCallback(this._DataResetYes_SpawnSecondQuestion), new DialogueScript.AnswerCallback(this._DataResetNo), new string[] { "DIALOGUE_MAIN_MENU_RESET_DATA_ARE_YOU_SURE" });
						DialogueScript.SetDialogueInputDelay(0.5f);
					}
					break;
				case 4:
					if (!flag && !flag2 && !flag3)
					{
						Sound.Play("SoundMenuSelect", 1f, 1f);
						if (!TwitchMaster.IsLoggedInAndEnabled())
						{
							TwitchMaster.instance.GetAuthInformation();
						}
						else
						{
							TwitchMaster.LogoutTwitch(true);
						}
					}
					break;
				case 5:
					if (!flag && !flag2 && !flag3)
					{
						Sound.Play("SoundMenuBack", 1f, 1f);
						this.menuIndex = MainMenuScript.MenuIndex.mainMenu;
						if (Master.IsDemo)
						{
							this.desiredNavigationIndex = 3;
						}
						else
						{
							this.desiredNavigationIndex = 2;
						}
					}
					break;
				}
			}
			else
			{
				switch (selectionIndex)
				{
				case 0:
					if (!flag && !flag2 && !flag3)
					{
						Sound.Play("SoundMenuSelect", 1f, 1f);
						this.menuIndex = MainMenuScript.MenuIndex.settingsAccessiblity;
						this.desiredNavigationIndex = 0;
					}
					break;
				case 1:
					if (!flag && !flag2 && !flag3)
					{
						Sound.Play("SoundMenuSelect", 1f, 1f);
						this.menuIndex = MainMenuScript.MenuIndex.settingsVideoAndAudio;
						this.desiredNavigationIndex = 0;
					}
					break;
				case 2:
					if (!flag && !flag2 && !flag3)
					{
						Sound.Play("SoundMenuSelect", 1f, 1f);
						this.menuIndex = MainMenuScript.MenuIndex.settingsOthers;
						this.desiredNavigationIndex = 0;
					}
					break;
				case 3:
					if (!flag && !flag2 && !flag3)
					{
						Sound.Play("SoundMenuSelect", 1f, 1f);
						DialogueScript.NextIsLegalDuringDeathCooldown();
						DialogueScript.SetQuestionDialogue(false, new DialogueScript.AnswerCallback(this._DataResetYes_SpawnSecondQuestion), new DialogueScript.AnswerCallback(this._DataResetNo), new string[] { "DIALOGUE_MAIN_MENU_RESET_DATA_ARE_YOU_SURE" });
						DialogueScript.SetDialogueInputDelay(0.5f);
					}
					break;
				case 4:
					if (!flag && !flag2 && !flag3)
					{
						Sound.Play("SoundMenuBack", 1f, 1f);
						this.menuIndex = MainMenuScript.MenuIndex.mainMenu;
						if (Master.IsDemo)
						{
							this.desiredNavigationIndex = 3;
						}
						else
						{
							this.desiredNavigationIndex = 2;
						}
					}
					break;
				}
			}
			break;
		case MainMenuScript.MenuIndex.settingsAccessiblity:
			switch (selectionIndex)
			{
			case 0:
				this.MFunc_Language(num, true);
				break;
			case 1:
				this.MFunc_TextEffects(true);
				break;
			case 2:
				this.MFunc_ScreenShake(true);
				break;
			case 3:
				this.MFunc_WobblyPolygons(true);
				break;
			case 4:
				if (!flag && !flag2 && !flag3)
				{
					Sound.Play("SoundMenuBack", 1f, 1f);
					this.menuIndex = MainMenuScript.MenuIndex.settings;
					this.desiredNavigationIndex = 0;
				}
				break;
			}
			break;
		case MainMenuScript.MenuIndex.settingsVideoAndAudio:
			switch (selectionIndex)
			{
			case 0:
				this.MFunc_FullscreenToggle(true);
				break;
			case 1:
				this.MFunc_ResolutionSwitch(num, true);
				break;
			case 2:
				this.MFunc_Vsync(true);
				break;
			case 3:
				this.MFunc_SfxVolume(num, true);
				break;
			case 4:
				this.MFunc_FanVolume(num, true);
				break;
			case 5:
				if (!flag && !flag2 && !flag3)
				{
					Sound.Play("SoundMenuBack", 1f, 1f);
					this.menuIndex = MainMenuScript.MenuIndex.settings;
					this.desiredNavigationIndex = 1;
				}
				break;
			}
			break;
		case MainMenuScript.MenuIndex.settingsOthers:
			switch (selectionIndex)
			{
			case 0:
				this.MFunc_TransitionSpeed(num, true);
				break;
			case 1:
				this.MFunc_GamepadVibration(true);
				break;
			case 2:
				this.MFunc_CameraSensitivity(num, true);
				break;
			case 3:
				this.MFunc_CursorSensitivity(num, true);
				break;
			case 4:
				this.MFunc_InvertCameraY(true);
				break;
			case 5:
				this.MFunc_KeyboardLayout(num, true);
				break;
			case 6:
				if (!flag && !flag2 && !flag3)
				{
					Sound.Play("SoundMenuBack", 1f, 1f);
					this.menuIndex = MainMenuScript.MenuIndex.settings;
					this.desiredNavigationIndex = 2;
				}
				break;
			}
			break;
		}
		this.OptionsUpdate();
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0003B741 File Offset: 0x00039941
	private void MFunc_ResetTutorial()
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.game.tutorialQuestionEnabled = true;
		DialogueScript.NextIsLegalDuringDeathCooldown();
		DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_MAIN_MENU_TUTORIAL_RESET" });
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0003B77C File Offset: 0x0003997C
	private void MFunc_Language(int _selectionDirection, bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		if (_selectionDirection == 1)
		{
			Translation.LanguageSetNext();
		}
		else
		{
			Translation.LanguageSetPrevious();
		}
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x0003B7AA File Offset: 0x000399AA
	private void MFunc_KeyboardLayout(int _selectionDirection, bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		if (_selectionDirection == 1)
		{
			Controls.KeyboardLayoutNext();
		}
		else
		{
			Controls.KeyboardLayoutPrevious();
		}
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0003B7D8 File Offset: 0x000399D8
	private void MFunc_TextEffects(bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.settings.dyslexicFontEnabled = !Data.settings.dyslexicFontEnabled;
		MainMenuScript.Callback callback = this.onDyslexiaSettingChange;
		if (callback != null)
		{
			callback();
		}
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0003B829 File Offset: 0x00039A29
	private void MFunc_ScreenShake(bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.settings.screenshakeEnabled = !Data.settings.screenshakeEnabled;
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0003B860 File Offset: 0x00039A60
	private void MFunc_WobblyPolygons(bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.settings.wobblyPolygons = !Data.settings.wobblyPolygons;
		Data.settings.Apply(false, false);
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0003B8AC File Offset: 0x00039AAC
	private void MFunc_FullscreenToggle(bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.settings.fullscreenEnabled = !Data.settings.fullscreenEnabled;
		Data.settings.Apply(true, false);
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x0003B8F8 File Offset: 0x00039AF8
	private void MFunc_ResolutionSwitch(int _selectionDirection, bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		if (_selectionDirection > 0)
		{
			Data.settings.VerticalResolutionSetNext();
		}
		else
		{
			Data.settings.VerticalResolutionSetPrevious();
		}
		Data.settings.Apply(true, false);
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x0003B948 File Offset: 0x00039B48
	private void MFunc_Vsync(bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.settings.vsyncEnabled = !Data.settings.vsyncEnabled;
		Data.settings.Apply(true, false);
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x0003B994 File Offset: 0x00039B94
	private void MFunc_SfxVolume(int _selectionDirection, bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		int num = Mathf.RoundToInt(Data.settings.volumeSound * 10f);
		num += _selectionDirection;
		if (num > 10)
		{
			num = 0;
		}
		if (num < 0)
		{
			num = 10;
		}
		Data.settings.volumeSound = (float)num / 10f;
		Data.settings.volumeMusic = Data.settings.volumeSound;
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x0003BA0C File Offset: 0x00039C0C
	private void MFunc_FanVolume(int _selectionDirection, bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		int num = Mathf.RoundToInt(Data.settings.fanVolume * 10f);
		num += _selectionDirection;
		if (num > 10)
		{
			num = 0;
		}
		if (num < 0)
		{
			num = 10;
		}
		Data.settings.fanVolume = (float)num / 10f;
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x0003BA70 File Offset: 0x00039C70
	private void MFunc_TransitionSpeed(int _selectionDirection, bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.settings.transitionSpeed += _selectionDirection;
		if (Data.settings.transitionSpeed < 1)
		{
			Data.settings.transitionSpeed = 4;
		}
		else if (Data.settings.transitionSpeed > 4)
		{
			Data.settings.transitionSpeed = 1;
		}
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x0003BAE0 File Offset: 0x00039CE0
	private void MFunc_GamepadVibration(bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.settings.JoystickVibrationEnabledSet(0, !Data.settings.JoystickVibrationEnabledGet(0));
		if (Data.settings.JoystickVibrationEnabledGet(0))
		{
			Controls.VibrationSet_PreferMax(this.player, 1f);
		}
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0003BB40 File Offset: 0x00039D40
	private void MFunc_CameraSensitivity(int _selectionDirection, bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		int num = Mathf.RoundToInt(Data.settings.CameraSensitivityGet(0).x * 10f);
		num += _selectionDirection;
		if (num > 20)
		{
			num = 1;
		}
		if (num < 1)
		{
			num = 20;
		}
		float num2 = (float)num / 10f;
		Data.settings.CameraSensitivitySet(0, new Vector2(num2, num2));
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x0003BBB4 File Offset: 0x00039DB4
	private void MFunc_CursorSensitivity(int _selectionDirection, bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		int num = Mathf.RoundToInt(Data.settings.VirtualCursorSensitivityGet(0) * 10f);
		num += _selectionDirection;
		if (num > 30)
		{
			num = 1;
		}
		if (num < 1)
		{
			num = 30;
		}
		float num2 = (float)num / 10f;
		Data.settings.VirtualCursorSensitivitySet(0, num2);
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x0003BC1B File Offset: 0x00039E1B
	private void MFunc_InvertCameraY(bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.settings.ControlsInvertCameraYSet(0, !Data.settings.ControlsInvertCameraYGet(0));
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x0003BC52 File Offset: 0x00039E52
	private void _RestartYes()
	{
		GameplayMaster.instance.FCall_MenuDrawer_MainMenu_CloseTry();
		GameplayMaster.instance.DieTry(GameplayMaster.DeathStep.lookAtTrapdoor, false);
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0003BC6B File Offset: 0x00039E6B
	private void _RestartNo()
	{
		Sound.Play("SoundMenuBack", 1f, 1f);
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0003BC82 File Offset: 0x00039E82
	private IEnumerator DataEresureCoroutine()
	{
		yield return null;
		DialogueScript.NextIsLegalDuringDeathCooldown();
		DialogueScript.SetQuestionDialogue(false, new DialogueScript.AnswerCallback(this._DataResetYes), new DialogueScript.AnswerCallback(this._DataResetNo), new string[] { "DIALOGUE_MAIN_MENU_RESET_DATA_ARE_YOU_SURE_2" });
		DialogueScript.SetDialogueInputDelay(0.5f);
		this.dataErasureCoroutine = null;
		yield break;
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0003BC91 File Offset: 0x00039E91
	private void _DataResetYes_SpawnSecondQuestion()
	{
		DialogueScript.Close();
		if (this.dataErasureCoroutine == null)
		{
			this.dataErasureCoroutine = base.StartCoroutine(this.DataEresureCoroutine());
		}
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x0003BCB2 File Offset: 0x00039EB2
	private void _DataResetYes()
	{
		Sound.Play("SoundMenuFileDestroy", 1f, 1f);
		CameraGame.Shake(2f);
		GameplayMaster.GameDataReset_FlagIt();
		GameplayMaster.instance.FCall_MenuDrawer_MainMenu_CloseTry();
		GameplayMaster.instance.DieTry(GameplayMaster.DeathStep.lookAtTrapdoor, false);
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0003BCEF File Offset: 0x00039EEF
	private void _DataResetNo()
	{
		Sound.Play("SoundMenuBack", 1f, 1f);
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0003BD06 File Offset: 0x00039F06
	private void _QuitYes()
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		GameplayMaster.instance.FCall_MenuDrawer_MainMenu_CloseTry();
		GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.closingGame, false, null);
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x0003BD2F File Offset: 0x00039F2F
	private void _QuitNo()
	{
		Sound.Play("SoundMenuBack", 1f, 1f);
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x0003BD46 File Offset: 0x00039F46
	public void OptionsUpdate()
	{
		this.OptionsUpdateText_Desktop();
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x0003BD50 File Offset: 0x00039F50
	private void OptionsUpdateText_Desktop()
	{
		int num = -1;
		switch (this.menuIndex)
		{
		case MainMenuScript.MenuIndex.mainMenu:
			if (Master.IsDemo)
			{
				num = 5;
			}
			else if (Master.IsPlaytestBuild)
			{
				num = 5;
			}
			else
			{
				num = 4;
			}
			break;
		case MainMenuScript.MenuIndex.settings:
			if (TwitchMaster.IsTwitchSupported())
			{
				num = 5;
			}
			else
			{
				num = 4;
			}
			break;
		case MainMenuScript.MenuIndex.settingsAccessiblity:
			num = 4;
			break;
		case MainMenuScript.MenuIndex.settingsVideoAndAudio:
			num = 5;
			break;
		case MainMenuScript.MenuIndex.settingsOthers:
			num = 6;
			break;
		default:
			Debug.LogError("MainMenuScript.OptionsUpdateText(): Cannot define menu options range. Menu index not handled: " + this.menuIndex.ToString());
			break;
		}
		for (int i = 0; i < this.menuElements.Length; i++)
		{
			DiegeticMenuElement diegeticMenuElement = this.menuElements[i];
			if (i > num)
			{
				this.menuController.elements.Remove(diegeticMenuElement);
				diegeticMenuElement.transform.parent.gameObject.SetActive(false);
			}
			else
			{
				if (!this.menuController.elements.Contains(diegeticMenuElement))
				{
					diegeticMenuElement.transform.parent.gameObject.SetActive(true);
					this.menuController.elements.Add(diegeticMenuElement);
					diegeticMenuElement.SetMyController(this.menuController);
				}
				if (!VirtualCursors.IsCursorVisible(0, true) && this.desiredNavigationIndex == i)
				{
					this.menuController.HoveredElement = diegeticMenuElement;
				}
			}
		}
		switch (this.menuIndex)
		{
		case MainMenuScript.MenuIndex.mainMenu:
			this.titleText.text = Translation.Get("MENU_TITLE_MAIN");
			break;
		case MainMenuScript.MenuIndex.settings:
			this.titleText.text = Translation.Get("MENU_TITLE_SETTINGS");
			break;
		case MainMenuScript.MenuIndex.settingsAccessiblity:
			this.titleText.text = Translation.Get("MENU_TITLE_SETTINGS_ACCESSIBILITY");
			break;
		case MainMenuScript.MenuIndex.settingsVideoAndAudio:
			this.titleText.text = Translation.Get("MENU_TITLE_SETTINGS_VIDEO_AND_AUDIO");
			break;
		case MainMenuScript.MenuIndex.settingsOthers:
			this.titleText.text = Translation.Get("MENU_OPTION_SETTINGS_OTHERS");
			break;
		default:
			Debug.LogError("MainMenuScript.OptionsUpdateText(): Cannot translate TITLE. Menu index not handled: " + this.menuIndex.ToString());
			break;
		}
		switch (this.menuIndex)
		{
		case MainMenuScript.MenuIndex.mainMenu:
			this.optionTexts[0].text = Translation.Get("MENU_OPTION_RESUME");
			this.optionTexts[1].text = Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("MENU_OPTION_RESTART"), Strings.SanitizationSubKind.none);
			this.optionTexts[2].text = Translation.Get("MENU_OPTION_SETTINGS") + "<sprite name=\"LanguagesIcon\">";
			this.optionTexts[3].text = Translation.Get("MENU_OPTION_RESET_TUTORIAL");
			this.optionTexts[4].text = Translation.Get("MENU_OPTION_QUIT");
			break;
		case MainMenuScript.MenuIndex.settings:
			if (TwitchMaster.IsTwitchSupported())
			{
				this.optionTexts[0].text = Translation.Get("MENU_OPTION_SETTINGS_ACCESSIBILITY_AND_REGION") + "<sprite name=\"LanguagesIcon\">";
				this.optionTexts[1].text = Translation.Get("MENU_OPTION_SETTINGS_VIDEO_AND_AUDIO");
				this.optionTexts[2].text = Translation.Get("MENU_OPTION_SETTINGS_OTHERS");
				this.optionTexts[3].text = Translation.Get("MENU_OPTION_SETTINGS_OTHERS_DELETE_DATA");
				this.optionTexts[4].text = Translation.Get(TwitchMaster.IsLoggedInAndEnabled() ? "MENU_OPTION_SETTINGS_TWITCH_MODE_ON" : "MENU_OPTION_SETTINGS_TWITCH_MODE_OFF");
				this.optionTexts[5].text = Translation.Get("MENU_OPTION_BACK");
			}
			else
			{
				this.optionTexts[0].text = Translation.Get("MENU_OPTION_SETTINGS_ACCESSIBILITY_AND_REGION") + "<sprite name=\"LanguagesIcon\">";
				this.optionTexts[1].text = Translation.Get("MENU_OPTION_SETTINGS_VIDEO_AND_AUDIO");
				this.optionTexts[2].text = Translation.Get("MENU_OPTION_SETTINGS_OTHERS");
				this.optionTexts[3].text = Translation.Get("MENU_OPTION_SETTINGS_OTHERS_DELETE_DATA");
				this.optionTexts[4].text = Translation.Get("MENU_OPTION_BACK");
			}
			break;
		case MainMenuScript.MenuIndex.settingsAccessiblity:
			this.optionTexts[0].text = Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("MENU_OPTION_SETTINGS_REGION_LANGUAGE"), Strings.SanitizationSubKind.none) + "<sprite name=\"LanguagesIcon\">";
			this.optionTexts[1].text = ((!Data.settings.dyslexicFontEnabled) ? Translation.Get("MENU_OPTION_SETTINGS_ACCESSIBILITY_TEXT_EFFECTS_ON") : Translation.Get("MENU_OPTION_SETTINGS_ACCESSIBILITY_TEXT_EFFECTS_OFF"));
			this.optionTexts[2].text = Translation.Get(Data.settings.screenshakeEnabled ? "MENU_OPTION_SETTINGS_VIDEO_SCREENSHAKE_ON" : "MENU_OPTION_SETTINGS_VIDEO_SCREENSHAKE_OFF");
			this.optionTexts[3].text = Translation.Get(Data.settings.wobblyPolygons ? "MENU_OPTION_SETTINGS_ACCESSIBILITY_WOBBLY_POLYGONS_ON" : "MENU_OPTION_SETTINGS_ACCESSIBILITY_WOBBLY_POLYGONS_OFF");
			this.optionTexts[4].text = Translation.Get("MENU_OPTION_BACK");
			break;
		case MainMenuScript.MenuIndex.settingsVideoAndAudio:
		{
			this.optionTexts[0].text = (Data.settings.fullscreenEnabled ? Translation.Get("MENU_OPTION_SETTINGS_VIDEO_FULLSCREEN_ON") : Translation.Get("MENU_OPTION_SETTINGS_VIDEO_FULLSCREEN_OFF"));
			Data.SettingsData.VerticalResolution verticalResolution = Data.settings.VerticalResolutionGet();
			switch (verticalResolution)
			{
			case Data.SettingsData.VerticalResolution._native:
				this.optionTexts[1].text = Translation.Get("MENU_OPTION_SETTINGS_VIDEO_RESOLUTION_NATIVE");
				break;
			case Data.SettingsData.VerticalResolution._360p:
				this.optionTexts[1].text = Translation.Get("MENU_OPTION_SETTINGS_VIDEO_RESOLUTION_360P");
				break;
			case Data.SettingsData.VerticalResolution._480p:
				this.optionTexts[1].text = Translation.Get("MENU_OPTION_SETTINGS_VIDEO_RESOLUTION_480P");
				break;
			case Data.SettingsData.VerticalResolution._720p:
				this.optionTexts[1].text = Translation.Get("MENU_OPTION_SETTINGS_VIDEO_RESOLUTION_720P");
				break;
			default:
				Debug.LogWarning("MainMenuScript.OptionsUpdateText_Desktop(): Vertical resolution not handled! Resolution: " + verticalResolution.ToString());
				break;
			}
			this.optionTexts[2].text = Translation.Get(Data.settings.vsyncEnabled ? "MENU_OPTION_SETTINGS_VIDEO_VSYNC_ON" : "MENU_OPTION_SETTINGS_VIDEO_VSYNC_OFF");
			int num2 = Mathf.RoundToInt(Data.settings.volumeSound * 100f);
			this.optionTexts[3].text = Translation.Get("MENU_OPTION_SETTINGS_SOUND_VOLUME") + num2.ToString() + "%";
			int num3 = Mathf.RoundToInt(Data.settings.fanVolume * 100f);
			this.optionTexts[4].text = Translation.Get("MENU_OPTION_SETTINGS_FAN_SOUND_VOLUME") + num3.ToString() + "%";
			this.optionTexts[5].text = Translation.Get("MENU_OPTION_BACK");
			break;
		}
		case MainMenuScript.MenuIndex.settingsOthers:
		{
			this.optionTexts[0].text = Translation.Get("MENU_OPTION_SETTINGS_OTHERS_TRANSITION_SPEED") + "X" + Data.settings.transitionSpeed.ToString();
			this.optionTexts[1].text = Translation.Get(Data.settings.JoystickVibrationEnabledGet(0) ? "MENU_OPTION_SETTINGS_CONTROLS_CONTROLLER_VIBRATION_ON" : "MENU_OPTION_SETTINGS_CONTROLS_CONTROLLER_VIBRATION_OFF");
			int num4 = Mathf.RoundToInt(Data.settings.CameraSensitivityGet(0).x * 100f);
			this.optionTexts[2].text = Translation.Get("MENU_OPTION_SETTINGS_CONTROLS_CAMERA_SENSITIVITY_ANY") + num4.ToString() + "%";
			int num5 = Mathf.RoundToInt(Data.settings.VirtualCursorSensitivityGet(0) * 100f);
			this.optionTexts[3].text = Translation.Get("MENU_OPTION_SETTINGS_CONTROLS_CURSOR_SENSITIVITY_ANY") + num5.ToString() + "%";
			this.optionTexts[4].text = Translation.Get(Data.settings.ControlsInvertCameraYGet(0) ? "MENU_OPTION_SETTINGS_CONTROLS_CAMERA_INVERT_Y_ON" : "MENU_OPTION_SETTINGS_CONTROLS_CAMERA_INVERT_Y_OFF");
			Data.SettingsData.KeyboardLayout keyboardLayout = Data.settings.KeyboardLayoutGet();
			switch (keyboardLayout)
			{
			case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
				this.optionTexts[5].text = Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("MENU_OPTION_KEYBOARD_LAYOUT_QWERTY"), Strings.SanitizationSubKind.none);
				break;
			case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
				this.optionTexts[5].text = Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("MENU_OPTION_KEYBOARD_LAYOUT_AZERTY"), Strings.SanitizationSubKind.none);
				break;
			case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
				this.optionTexts[5].text = Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("MENU_OPTION_KEYBOARD_LAYOUT_DVORAK"), Strings.SanitizationSubKind.none);
				break;
			case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
				this.optionTexts[5].text = Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("MENU_OPTION_KEYBOARD_LAYOUT_COLEMAK"), Strings.SanitizationSubKind.none);
				break;
			default:
				Debug.LogWarning("MainMenuScript.OptionsUpdateText_Desktop(): keyboard layout not handled! layout: " + keyboardLayout.ToString());
				break;
			}
			this.optionTexts[6].text = Translation.Get("MENU_OPTION_BACK");
			break;
		}
		default:
			Debug.LogError("MainMenuScript.OptionsUpdateText(): Cannot translate OPTION. Menu index not handled: " + this.menuIndex.ToString());
			break;
		}
		this.twitchText.text = TwitchMaster.GetTwitchMenuString();
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x0003C59A File Offset: 0x0003A79A
	public static void ForceClose_Death()
	{
		MainMenuScript.BackInput();
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x0003C5A1 File Offset: 0x0003A7A1
	private void Awake()
	{
		MainMenuScript.instance = this;
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x0003C5A9 File Offset: 0x0003A7A9
	private void Start()
	{
		this.shifterTransform.localPosition = this.shifterHiddenPos;
		this.player = Controls.GetPlayerByIndex(0);
		this.twitchText.text = "";
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x0003C5D8 File Offset: 0x0003A7D8
	private void OnDestroy()
	{
		if (MainMenuScript.instance == this)
		{
			MainMenuScript.instance = null;
		}
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x0003C5F0 File Offset: 0x0003A7F0
	private void Update()
	{
		bool flag = this.CanMenuPerform(false);
		if (MainMenuScript.IsEnabled())
		{
			this.shifterTransform.localPosition = Vector3.Lerp(this.shifterTransform.localPosition, this.shifterVisiblePos, Tick.Time * 10f);
		}
		else
		{
			this.shifterTransform.localPosition = Vector3.Lerp(this.shifterTransform.localPosition, this.shifterHiddenPos, Tick.Time * 10f);
		}
		float num = (MainMenuScript.IsEnabled() ? 0.75f : 0f);
		this.backImageColor.a = Mathf.Lerp(this.backImageColor.a, num, Tick.Time * 10f);
		this.backgroundImage.color = this.backImageColor;
		bool flag2 = MainMenuScript.IsEnabled() && !DialogueScript.IsEnabled() && this.dataErasureCoroutine == null;
		if (this.titleText.gameObject.activeSelf != flag2)
		{
			this.titleText.gameObject.SetActive(flag2);
			for (int i = 0; i < this.optionTexts.Length; i++)
			{
				if (this.optionTexts[i].gameObject.activeSelf != flag2)
				{
					this.optionTexts[i].gameObject.SetActive(flag2);
				}
			}
		}
		if (!flag)
		{
			return;
		}
		this.inputDelay -= Tick.Time;
		if (this.inputDelay > 0f)
		{
			return;
		}
		for (int j = 0; j < this.optionTexts.Length; j++)
		{
			bool flag3 = false;
			DiegeticMenuElement hoveredElement = DiegeticMenuController.ActiveMenu.HoveredElement;
			if (hoveredElement != null)
			{
				Transform parent = hoveredElement.transform.parent;
				if (parent != null && parent.GetComponentInChildren<TextMeshProUGUI>() == this.optionTexts[j])
				{
					flag3 = true;
				}
			}
			Color color = Color.white;
			if (j == 0 && this.menuIndex == MainMenuScript.MenuIndex.mainMenu && Master.IsPlaytestBuild)
			{
				color = Color.green;
			}
			this.optionTexts[j].color = (flag3 ? Color.yellow : color);
		}
		Vector2 vector = new Vector2((float)CameraGame.list[0].myCamera.pixelWidth, (float)CameraGame.list[0].myCamera.pixelHeight);
		Vector2 vector2 = VirtualCursors.CursorPositionNormalizedCenteredGet_ReferenceResolution(0, vector);
		bool flag4 = Controls.GetPlayerByIndex(0).lastInputKindUsed == Controls.InputKind.Mouse && Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) && (vector2.x < -0.2f || vector2.x > 0.2f);
		this.LeftRightNavigationUpdate();
		if (this.LeftRightNavigationHasBeenPressed() && this.menuController.HoveredElement != null)
		{
			UnityEvent onSelectCallback = this.menuController.HoveredElement.onSelectCallback;
			if (onSelectCallback != null)
			{
				onSelectCallback.Invoke();
			}
		}
		if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true) || flag4)
		{
			this.Select(MainMenuScript.MenuIndex.mainMenu, MainMenuScript.INDEX_RESUME);
		}
	}

	public static MainMenuScript instance;

	private const int PLAYER_INDEX = 0;

	private const float LERP_SPD = 10f;

	private const float INPUT_DELAY_RESET = 0.5f;

	private Controls.PlayerExt player;

	public DiegeticMenuController menuController;

	public DiegeticMenuElement[] menuElements;

	public Transform shifterTransform;

	public Vector3 shifterHiddenPos;

	public Vector3 shifterVisiblePos;

	public RawImage backgroundImage;

	public TextMeshProUGUI titleText;

	public TextMeshProUGUI[] optionTexts;

	public TextMeshProUGUI twitchText;

	private float inputDelay = 0.5f;

	private Color backImageColor = new Color(0f, 0f, 0f, 0f);

	private bool _isEnabled;

	private bool saveSettingsOnClose;

	private MainMenuScript.MenuIndex menuIndex;

	private float leftRightAxisPrevious;

	private bool rightNavigationPress;

	private bool leftNavigationPress;

	private Coroutine dataErasureCoroutine;

	private int desiredNavigationIndex = -1;

	public MainMenuScript.Callback onDyslexiaSettingChange;

	public enum MenuIndex
	{
		undefined = -1,
		mainMenu,
		settings,
		settingsAccessiblity,
		settingsVideoAndAudio,
		settingsOthers
	}

	// (Invoke) Token: 0x060011EF RID: 4591
	public delegate void Callback();
}
