using System;
using System.Collections;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000BF RID: 191
public class MainMenuScript : MonoBehaviour
{
	// Token: 0x17000074 RID: 116
	// (get) Token: 0x06000A3B RID: 2619 RVA: 0x00008AE5 File Offset: 0x00006CE5
	private static int INDEX_RESUME
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x06000A3C RID: 2620 RVA: 0x0000E204 File Offset: 0x0000C404
	public static bool IsEnabled()
	{
		return !(MainMenuScript.instance == null) && MainMenuScript.instance._isEnabled;
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x00052964 File Offset: 0x00050B64
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

	// Token: 0x06000A3E RID: 2622 RVA: 0x0000E21F File Offset: 0x0000C41F
	public static void Close()
	{
		if (!MainMenuScript.IsEnabled())
		{
			return;
		}
		GC.Collect();
		MainMenuScript.instance._isEnabled = false;
		MainMenuScript.instance.menuController.Back();
		if (MainMenuScript.instance.saveSettingsOnClose)
		{
			Data.SaveSettings();
		}
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x0000E25A File Offset: 0x0000C45A
	public static void BackInput()
	{
		GameplayMaster.instance.FCall_MenuDrawer_MainMenu_CloseTry();
		Sound.Play("SoundMenuBack", 1f, 1f);
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x0000E27B File Offset: 0x0000C47B
	public bool CanMenuPerform(bool includeDelay)
	{
		return (!includeDelay || this.inputDelay <= 0f) && this.dataErasureCoroutine == null && (MainMenuScript.IsEnabled() && !ScreenMenuScript.IsEnabled()) && !DialogueScript.IsEnabled();
	}

	// Token: 0x06000A41 RID: 2625 RVA: 0x000529F0 File Offset: 0x00050BF0
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

	// Token: 0x06000A42 RID: 2626 RVA: 0x0000E2B1 File Offset: 0x0000C4B1
	public bool LeftRightNavigationHasBeenPressed()
	{
		return this.leftNavigationPress || this.rightNavigationPress;
	}

	// Token: 0x06000A43 RID: 2627 RVA: 0x0000E2C3 File Offset: 0x0000C4C3
	public void Select(int selectionIndex)
	{
		this.Select(this.menuIndex, selectionIndex);
	}

	// Token: 0x06000A44 RID: 2628 RVA: 0x0000E2D2 File Offset: 0x0000C4D2
	public void Select(MainMenuScript.MenuIndex _menuIndex, int selectionIndex)
	{
		this.Select_Desktop(_menuIndex, selectionIndex);
	}

	// Token: 0x06000A45 RID: 2629 RVA: 0x00052A64 File Offset: 0x00050C64
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
				this.MFunc_ReducedFlashing(true);
				break;
			case 5:
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

	// Token: 0x06000A46 RID: 2630 RVA: 0x0000E2DC File Offset: 0x0000C4DC
	private void MFunc_ResetTutorial()
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.game.tutorialQuestionEnabled = true;
		DialogueScript.NextIsLegalDuringDeathCooldown();
		DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_MAIN_MENU_TUTORIAL_RESET" });
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x0000E317 File Offset: 0x0000C517
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

	// Token: 0x06000A48 RID: 2632 RVA: 0x0000E345 File Offset: 0x0000C545
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

	// Token: 0x06000A49 RID: 2633 RVA: 0x000530B0 File Offset: 0x000512B0
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

	// Token: 0x06000A4A RID: 2634 RVA: 0x0000E373 File Offset: 0x0000C573
	private void MFunc_ScreenShake(bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.settings.screenshakeEnabled = !Data.settings.screenshakeEnabled;
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000A4B RID: 2635 RVA: 0x00053104 File Offset: 0x00051304
	private void MFunc_WobblyPolygons(bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.settings.wobblyPolygons = !Data.settings.wobblyPolygons;
		Data.settings.Apply(false, false);
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000A4C RID: 2636 RVA: 0x00053150 File Offset: 0x00051350
	private void MFunc_ReducedFlashing(bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.settings.flashingLightsReducedEnabled = !Data.settings.flashingLightsReducedEnabled;
		Data.settings.Apply(false, false);
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000A4D RID: 2637 RVA: 0x0005319C File Offset: 0x0005139C
	private void MFunc_FullscreenToggle(bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.settings.fullscreenEnabled = !Data.settings.fullscreenEnabled;
		Data.settings.Apply(true, false);
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x000531E8 File Offset: 0x000513E8
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

	// Token: 0x06000A4F RID: 2639 RVA: 0x00053238 File Offset: 0x00051438
	private void MFunc_Vsync(bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.settings.vsyncEnabled = !Data.settings.vsyncEnabled;
		Data.settings.Apply(true, false);
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x00053284 File Offset: 0x00051484
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

	// Token: 0x06000A51 RID: 2641 RVA: 0x000532FC File Offset: 0x000514FC
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

	// Token: 0x06000A52 RID: 2642 RVA: 0x00053360 File Offset: 0x00051560
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

	// Token: 0x06000A53 RID: 2643 RVA: 0x000533D0 File Offset: 0x000515D0
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

	// Token: 0x06000A54 RID: 2644 RVA: 0x00053430 File Offset: 0x00051630
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

	// Token: 0x06000A55 RID: 2645 RVA: 0x000534A4 File Offset: 0x000516A4
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

	// Token: 0x06000A56 RID: 2646 RVA: 0x0000E3A8 File Offset: 0x0000C5A8
	private void MFunc_InvertCameraY(bool saveSettingsWhenClosing)
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		Data.settings.ControlsInvertCameraYSet(0, !Data.settings.ControlsInvertCameraYGet(0));
		this.saveSettingsOnClose = saveSettingsWhenClosing;
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x0000E3DF File Offset: 0x0000C5DF
	private void _RestartYes()
	{
		GameplayMaster.instance.FCall_MenuDrawer_MainMenu_CloseTry();
		GameplayMaster.instance.DieTry(GameplayMaster.DeathStep.lookAtTrapdoor, false);
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x0000E3F8 File Offset: 0x0000C5F8
	private void _RestartNo()
	{
		Sound.Play("SoundMenuBack", 1f, 1f);
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x0000E40F File Offset: 0x0000C60F
	private IEnumerator DataEresureCoroutine()
	{
		yield return null;
		DialogueScript.NextIsLegalDuringDeathCooldown();
		DialogueScript.SetQuestionDialogue(false, new DialogueScript.AnswerCallback(this._DataResetYes), new DialogueScript.AnswerCallback(this._DataResetNo), new string[] { "DIALOGUE_MAIN_MENU_RESET_DATA_ARE_YOU_SURE_2" });
		DialogueScript.SetDialogueInputDelay(0.5f);
		this.dataErasureCoroutine = null;
		yield break;
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x0000E41E File Offset: 0x0000C61E
	private void _DataResetYes_SpawnSecondQuestion()
	{
		DialogueScript.Close();
		if (this.dataErasureCoroutine == null)
		{
			this.dataErasureCoroutine = base.StartCoroutine(this.DataEresureCoroutine());
		}
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x0000E43F File Offset: 0x0000C63F
	private void _DataResetYes()
	{
		Sound.Play("SoundMenuFileDestroy", 1f, 1f);
		CameraGame.Shake(2f);
		GameplayMaster.GameDataReset_FlagIt();
		GameplayMaster.instance.FCall_MenuDrawer_MainMenu_CloseTry();
		GameplayMaster.instance.DieTry(GameplayMaster.DeathStep.lookAtTrapdoor, false);
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x0000E3F8 File Offset: 0x0000C5F8
	private void _DataResetNo()
	{
		Sound.Play("SoundMenuBack", 1f, 1f);
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x0000E47C File Offset: 0x0000C67C
	private void _QuitYes()
	{
		Sound.Play("SoundMenuSelect", 1f, 1f);
		GameplayMaster.instance.FCall_MenuDrawer_MainMenu_CloseTry();
		GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.closingGame, false, null);
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x0000E3F8 File Offset: 0x0000C5F8
	private void _QuitNo()
	{
		Sound.Play("SoundMenuBack", 1f, 1f);
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x0000E4A5 File Offset: 0x0000C6A5
	public void OptionsUpdate()
	{
		this.OptionsUpdateText_Desktop();
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x0005350C File Offset: 0x0005170C
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
			num = 5;
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
			this.optionTexts[4].text = Translation.Get(Data.settings.flashingLightsReducedEnabled ? "MENU_OPTION_SETTINGS_ACCESSIBILITY_REDUCE_FLASHES_ON" : "MENU_OPTION_SETTINGS_ACCESSIBILITY_REDUCE_FLASHES_OFF");
			this.optionTexts[5].text = Translation.Get("MENU_OPTION_BACK");
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
		this.adMobileRect.gameObject.SetActive(Master.instance.MobileAdEnabled());
		this.adMobileText.text = Translation.Get("VARIOUS_MAGAZINE_PLAY_MOBILE_VERSION") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.menuSocialButton, 0);
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x0000E4AD File Offset: 0x0000C6AD
	public static void ForceClose_Death()
	{
		MainMenuScript.BackInput();
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x0000E4B4 File Offset: 0x0000C6B4
	private void Awake()
	{
		MainMenuScript.instance = this;
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x00053DC4 File Offset: 0x00051FC4
	private void Start()
	{
		this.shifterTransform.localPosition = this.shifterHiddenPos;
		this.player = Controls.GetPlayerByIndex(0);
		this.twitchText.text = "";
		this.adMobileStartPos = this.adMobileRect.anchoredPosition;
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x0000E4BC File Offset: 0x0000C6BC
	private void OnDestroy()
	{
		if (MainMenuScript.instance == this)
		{
			MainMenuScript.instance = null;
		}
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x00053E14 File Offset: 0x00052014
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
		Vector2 vector3 = VirtualCursors.CursorPositionNormalizedCenteredGet_ReferenceResolution(0, new Vector2(3f, 1.68f));
		bool flag5 = false;
		if (vector3.x < this.adMobileRect.anchoredPosition.x + this.adMobileRect.sizeDelta.x / 2f && vector3.x > this.adMobileRect.anchoredPosition.x - this.adMobileRect.sizeDelta.x / 2f && vector3.y < this.adMobileRect.anchoredPosition.y + this.adMobileRect.sizeDelta.y / 2f && vector3.y > this.adMobileRect.anchoredPosition.y - this.adMobileRect.sizeDelta.y / 2f)
		{
			flag5 = true;
		}
		if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSocialButton, true))
		{
			flag5 = true;
		}
		Vector2 vector4 = this.adMobileStartPos;
		if (flag5)
		{
			vector4.y += 0.02f;
		}
		this.adMobileRect.anchoredPosition = Vector2.Lerp(this.adMobileRect.anchoredPosition, vector4, Tick.Time * 10f);
		if (this.mobileAdButtonHoveredPrev != flag5)
		{
			this.mobileAdButtonHoveredPrev = flag5;
			if (this.mobileAdButtonHoveredPrev)
			{
				Sound.Play("SoundMenuSelectionChange", 1f, 1f);
			}
		}
		if (flag5 && ((VirtualCursors.IsCursorVisible(0, true) && Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true)) || Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSocialButton, true)))
		{
			MagazineUiScript.AdLinkOpen();
		}
	}

	// Token: 0x04000A74 RID: 2676
	public static MainMenuScript instance;

	// Token: 0x04000A75 RID: 2677
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000A76 RID: 2678
	private const float LERP_SPD = 10f;

	// Token: 0x04000A77 RID: 2679
	private const float INPUT_DELAY_RESET = 0.5f;

	// Token: 0x04000A78 RID: 2680
	private Controls.PlayerExt player;

	// Token: 0x04000A79 RID: 2681
	public DiegeticMenuController menuController;

	// Token: 0x04000A7A RID: 2682
	public DiegeticMenuElement[] menuElements;

	// Token: 0x04000A7B RID: 2683
	public Transform shifterTransform;

	// Token: 0x04000A7C RID: 2684
	public Vector3 shifterHiddenPos;

	// Token: 0x04000A7D RID: 2685
	public Vector3 shifterVisiblePos;

	// Token: 0x04000A7E RID: 2686
	public Transform scalerTransform;

	// Token: 0x04000A7F RID: 2687
	public RawImage backgroundImage;

	// Token: 0x04000A80 RID: 2688
	public TextMeshProUGUI titleText;

	// Token: 0x04000A81 RID: 2689
	public TextMeshProUGUI[] optionTexts;

	// Token: 0x04000A82 RID: 2690
	public TextMeshProUGUI twitchText;

	// Token: 0x04000A83 RID: 2691
	public RectTransform adMobileRect;

	// Token: 0x04000A84 RID: 2692
	public TextMeshProUGUI adMobileText;

	// Token: 0x04000A85 RID: 2693
	private float inputDelay = 0.5f;

	// Token: 0x04000A86 RID: 2694
	private Color backImageColor = new Color(0f, 0f, 0f, 0f);

	// Token: 0x04000A87 RID: 2695
	private bool _isEnabled;

	// Token: 0x04000A88 RID: 2696
	private bool saveSettingsOnClose;

	// Token: 0x04000A89 RID: 2697
	private MainMenuScript.MenuIndex menuIndex;

	// Token: 0x04000A8A RID: 2698
	private float leftRightAxisPrevious;

	// Token: 0x04000A8B RID: 2699
	private bool rightNavigationPress;

	// Token: 0x04000A8C RID: 2700
	private bool leftNavigationPress;

	// Token: 0x04000A8D RID: 2701
	private Coroutine dataErasureCoroutine;

	// Token: 0x04000A8E RID: 2702
	private int desiredNavigationIndex = -1;

	// Token: 0x04000A8F RID: 2703
	private Vector3 adMobileStartPos;

	// Token: 0x04000A90 RID: 2704
	private bool mobileAdButtonHoveredPrev;

	// Token: 0x04000A91 RID: 2705
	public MainMenuScript.Callback onDyslexiaSettingChange;

	// Token: 0x020000C0 RID: 192
	public enum MenuIndex
	{
		// Token: 0x04000A93 RID: 2707
		undefined = -1,
		// Token: 0x04000A94 RID: 2708
		mainMenu,
		// Token: 0x04000A95 RID: 2709
		settings,
		// Token: 0x04000A96 RID: 2710
		settingsAccessiblity,
		// Token: 0x04000A97 RID: 2711
		settingsVideoAndAudio,
		// Token: 0x04000A98 RID: 2712
		settingsOthers
	}

	// Token: 0x020000C1 RID: 193
	// (Invoke) Token: 0x06000A68 RID: 2664
	public delegate void Callback();
}
