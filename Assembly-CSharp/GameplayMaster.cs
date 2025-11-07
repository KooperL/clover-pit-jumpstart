using System;
using System.Collections;
using System.Numerics;
using Panik;
using UnityEngine;

public class GameplayMaster : MonoBehaviour
{
	// Token: 0x060002F2 RID: 754 RVA: 0x00012088 File Offset: 0x00010288
	private void ControllerDisconnectionCheckUpdate()
	{
		int joystickCount = this.player.rePlayer.controllers.joystickCount;
		if (this.oldJoystickCount != joystickCount)
		{
			if (this.oldJoystickCount > joystickCount)
			{
				this.joysticDisconnectionCount = this.oldJoystickCount;
			}
			this.oldJoystickCount = joystickCount;
		}
		if (this.joysticDisconnectionCount == joystickCount)
		{
			this.joysticDisconnectionCount = -1;
		}
		GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
		bool flag = !DialogueScript.IsEnabled() && !ScreenMenuScript.IsEnabled() && !PhoneUiScript.IsEnabled() && gamePhase == GameplayMaster.GamePhase.preparation;
		if (this.joysticDisconnectionCount > 0 && flag)
		{
			this.joysticDisconnectionCount = -1;
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_CONTROLLER_DISCONNECTED" });
		}
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x0001212C File Offset: 0x0001032C
	private void TimePlayedCount()
	{
		GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
		bool flag = false;
		if (gamePhase == GameplayMaster.GamePhase.endingWithoutDeath || gamePhase == GameplayMaster.GamePhase.closingGame || gamePhase == GameplayMaster.GamePhase.death)
		{
			flag = true;
		}
		if (flag)
		{
			return;
		}
		this.timePlayed_SecondsTimer += (double)Tick.Time;
		if (this.timePlayed_SecondsTimer >= 1.0)
		{
			this.timePlayed_SecondsTimer -= 1.0;
			GameplayData.Stats_PlayTime_AddSeconds(1L);
		}
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x00012194 File Offset: 0x00010394
	public static int DeathCountdownGet()
	{
		if (GameplayMaster.instance == null)
		{
			return 5;
		}
		return GameplayMaster.instance.deathCountDown;
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x000121AF File Offset: 0x000103AF
	public static bool DeathCountdownHasStarted()
	{
		return !(GameplayMaster.instance == null) && GameplayMaster.instance.deathCountDownStarted;
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x000121CC File Offset: 0x000103CC
	public static void DeathCountdownResetRequest(bool byRoundsIncrease)
	{
		if (GameplayMaster.instance == null)
		{
			return;
		}
		if (!GameplayMaster.instance.deathCountDownStarted)
		{
			return;
		}
		if (GameplayMaster.instance.deathCountDownResetRequest)
		{
			return;
		}
		GameplayMaster.instance.deathCountDownResetRequest = true;
		GameplayMaster.instance.deathCDResetRequest_RoundsFlag = byRoundsIncrease;
		PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.NearDeathExperience);
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x0001221F File Offset: 0x0001041F
	private void _DeathCountdownReset(bool stopAmbience)
	{
		this.deathCountDownResetRequest = false;
		this.deathCDResetRequest_RoundsFlag = false;
		this.deathCountDownStarted = false;
		this.deathCountDownTimer = 0f;
		this.deathCountDown = 5;
		if (stopAmbience)
		{
			Sound.Stop("SoundDeathApproachingAmbience", true);
		}
	}

	// Token: 0x060002F8 RID: 760 RVA: 0x00012256 File Offset: 0x00010456
	public static GameplayMaster.GamePhase GetGamePhase()
	{
		if (GameplayMaster.instance == null)
		{
			return GameplayMaster.GamePhase.Undefined;
		}
		return GameplayMaster.instance.gamePhase;
	}

	// Token: 0x060002F9 RID: 761 RVA: 0x00012274 File Offset: 0x00010474
	public static void SetGamePhase(GameplayMaster.GamePhase phase, bool forceSame, string extraInfos = null)
	{
		if (GameplayMaster.instance == null)
		{
			Debug.LogError("GameplayMaster instance is null");
			return;
		}
		if (!forceSame && GameplayMaster.instance.gamePhase == phase)
		{
			return;
		}
		switch (GameplayMaster.instance.gamePhase)
		{
		case GameplayMaster.GamePhase.intro:
		case GameplayMaster.GamePhase.cutscene:
		case GameplayMaster.GamePhase.preparation:
		case GameplayMaster.GamePhase.equipping:
		case GameplayMaster.GamePhase.death:
		case GameplayMaster.GamePhase.endingWithoutDeath:
		case GameplayMaster.GamePhase.closingGame:
		case GameplayMaster.GamePhase.terminal:
		case GameplayMaster.GamePhase.phone:
		case GameplayMaster.GamePhase.Count:
		case GameplayMaster.GamePhase.Undefined:
			goto IL_009B;
		case GameplayMaster.GamePhase.gambling:
			GeneralUiScript.CoinsTextForceUpdate();
			goto IL_009B;
		}
		Debug.LogError("GamePhase not handled: " + GameplayMaster.instance.gamePhase.ToString());
		IL_009B:
		switch (phase)
		{
		case GameplayMaster.GamePhase.intro:
			GeneralUiScript.instance.FadeIntro();
			goto IL_015F;
		case GameplayMaster.GamePhase.cutscene:
		case GameplayMaster.GamePhase.equipping:
		case GameplayMaster.GamePhase.death:
		case GameplayMaster.GamePhase.endingWithoutDeath:
		case GameplayMaster.GamePhase.terminal:
		case GameplayMaster.GamePhase.phone:
		case GameplayMaster.GamePhase.Count:
		case GameplayMaster.GamePhase.Undefined:
			goto IL_015F;
		case GameplayMaster.GamePhase.preparation:
		{
			float num = 1f;
			if (GameplayMaster.instance.gamePhase == GameplayMaster.GamePhase.cutscene && extraInfos != "From Intro Dialogue")
			{
				num = 0f;
			}
			if (GameplayMaster.instance.gamePhase == GameplayMaster.GamePhase.gambling)
			{
				num = 0f;
			}
			CameraController.SetPosition(CameraController.PositionKind.Free, false, num);
			goto IL_015F;
		}
		case GameplayMaster.GamePhase.gambling:
			CameraController.SetPosition(CameraController.PositionKind.Slot_Fixed, false, 1f);
			goto IL_015F;
		case GameplayMaster.GamePhase.closingGame:
			CameraController.SetPosition(CameraController.PositionKind.RoomTopView, false, 1f);
			goto IL_015F;
		}
		Debug.LogError("GamePhase not handled: " + phase.ToString());
		IL_015F:
		GameplayMaster.instance.gamePhase = phase;
	}

	// Token: 0x060002FA RID: 762 RVA: 0x000123EB File Offset: 0x000105EB
	private void IntroPhaseBehaviour()
	{
		if (!GeneralUiScript.instance.HasFadedIn())
		{
			return;
		}
		GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.cutscene, false, null);
		CameraController.SetPosition(CameraController.PositionKind.Free, true, 0f);
		CameraController.SetFreeCameraRotation_ToCamera();
	}

	// Token: 0x060002FB RID: 763 RVA: 0x00012413 File Offset: 0x00010613
	public static bool IsIntroDialogueFinished()
	{
		return !(GameplayMaster.instance == null) && GameplayMaster.instance.introCutscenePhase == GameplayMaster.IntroCutscenePhase.done;
	}

	// Token: 0x060002FC RID: 764 RVA: 0x00012434 File Offset: 0x00010634
	private void CutscenePhaseBehaviour()
	{
		bool flag = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true);
		BigInteger bigInteger = GameplayData.RoundsLeftToDeadline();
		bool flag2 = this.IsDeathCondition(true, true);
		float num = (float)Data.settings.transitionSpeed;
		RunModifierScript.Identifier identifier = GameplayData.RunModifier_GetCurrent();
		if (ATMScript.DebtClearCutsceneIsPlaying())
		{
			return;
		}
		if (ScreenMenuScript.IsEnabled())
		{
			return;
		}
		if (DialogueScript.IsEnabled())
		{
			this.delay = 0.5f;
			return;
		}
		if (PowerupTriggerAnimController.HasAnimations())
		{
			this.delay = 0.5f;
			this.interestsAndTicketsTimer = 0.5f;
			return;
		}
		if (TutorialScript.IsEnabled())
		{
			return;
		}
		if (DeckBoxUI.IsEnabled())
		{
			this.delay = 0.5f;
			return;
		}
		if (this.introCutscenePhase != GameplayMaster.IntroCutscenePhase.done)
		{
			switch (this.introCutscenePhase)
			{
			case GameplayMaster.IntroCutscenePhase.introDialogueOrDeckBox:
				if (!GameplayData.NewGameIntroFinished_Get())
				{
					if (DeckBoxScript.IsEnabled())
					{
						if (!GameplayData.RunModifier_AlreadyPicked())
						{
							this.FCall_DeckBoxOpen();
							break;
						}
					}
					else
					{
						GameplayData.RunModifier_SetCurrent(RunModifierScript.Identifier.defaultModifier, true);
					}
				}
				DeckBoxScript.SetSummonedCard();
				this.introCutscenePhase = GameplayMaster.IntroCutscenePhase.tutorial;
				if (!GameplayData.NewGameIntroFinished_Get())
				{
					if (Data.game.bookedBadEndingDialogue)
					{
						DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_WELCOME_BACK_AFTER_BAD_ENDING" });
						DialogueScript.SetDialogueInputDelay(1f);
						Data.game.bookedBadEndingDialogue = false;
					}
					else
					{
						string text = RunModifierScript.AlternativeIntroDialogueGetKey(identifier);
						if (!string.IsNullOrEmpty(text))
						{
							DialogueScript.SetDialogue(false, new string[] { text });
						}
						else
						{
							int num2 = GameplayMaster.DeathsSinceStartup_GetNum();
							if (Data.game.tutorialQuestionEnabled)
							{
								DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_INTRO_0", "DIALOGUE_INTRO_1" });
							}
							else if (num2 == 3)
							{
								DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_INTRO_ALT_ALT_0" });
							}
							else if (num2 == 2)
							{
								DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_INTRO_ALT_0", "DIALOGUE_INTRO_ALT_1" });
							}
						}
					}
				}
				else
				{
					DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_WELCOME_BACK_ALT_" + Util.Choose<int>(new int[] { 0, 1, 2 }).ToString() });
				}
				break;
			case GameplayMaster.IntroCutscenePhase.tutorial:
				this.introCutscenePhase = GameplayMaster.IntroCutscenePhase.finalization;
				if (Data.game.tutorialQuestionEnabled)
				{
					Data.game.tutorialQuestionEnabled = false;
					DialogueScript.SetQuestionDialogue(true, delegate
					{
						TutorialScript.StartTutorial();
					}, delegate
					{
					}, new string[] { "DIALOGUE_TUTORIAL_QUESTION_0" });
				}
				break;
			case GameplayMaster.IntroCutscenePhase.finalization:
			{
				this.introCutscenePhase = GameplayMaster.IntroCutscenePhase.done;
				GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.preparation, false, "From Intro Dialogue");
				if (DrawersScript.GetDrawersUnlockedNum() <= 0)
				{
					MemoScript.SetMessage(MemoScript.Message.inputsMove, 3f);
				}
				else
				{
					MemoScript.SetMessage(MemoScript.Message.inputsMove, 1.5f);
				}
				int deathsDone = Data.game.deathsDone;
				if (deathsDone != 3)
				{
					if (deathsDone == 5)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.OneTrickPony);
					}
				}
				else
				{
					PowerupScript.Unlock(PowerupScript.Identifier.Ankh);
				}
				if (!GameplayData.NewGameIntroFinished_Get())
				{
					int num3 = 0;
					switch (RewardBoxScript.GetRewardKind())
					{
					case RewardBoxScript.RewardKind.DrawerKey0:
						num3 = 0;
						break;
					case RewardBoxScript.RewardKind.DrawerKey1:
						num3 = 1;
						break;
					case RewardBoxScript.RewardKind.DrawerKey2:
						num3 = 2;
						break;
					case RewardBoxScript.RewardKind.DrawerKey3:
						num3 = 3;
						break;
					case RewardBoxScript.RewardKind.DoorKey:
						num3 = 4;
						break;
					}
					if (num3 >= 1)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Dice_4);
						PowerupScript.Unlock(PowerupScript.Identifier.GeneralModCharm_Clicker);
						PowerupScript.Unlock(PowerupScript.Identifier.GeneralModCharm_CloverBellBattery);
						PowerupScript.Unlock(PowerupScript.Identifier.GeneralModCharm_CrystalSphere);
						PowerupScript.Unlock(PowerupScript.Identifier.DearDiary);
					}
					if (num3 >= 2)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Dice_6);
						PowerupScript.Unlock(PowerupScript.Identifier.DiscA);
						PowerupScript.Unlock(PowerupScript.Identifier.DiscB);
						PowerupScript.Unlock(PowerupScript.Identifier.DiscC);
						PowerupScript.Unlock(PowerupScript.Identifier.PlayingCard_HeartsAce);
						PowerupScript.Unlock(PowerupScript.Identifier.PlayingCard_ClubsAce);
						PowerupScript.Unlock(PowerupScript.Identifier.PlayingCard_DiamondsAce);
						PowerupScript.Unlock(PowerupScript.Identifier.PlayingCard_SpadesAce);
					}
					if (num3 >= 3)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.VineSoupShroom);
						PowerupScript.Unlock(PowerupScript.Identifier.ShoppingCart);
						PowerupScript.Unlock(PowerupScript.Identifier.CrowBar);
						PowerupScript.Unlock(PowerupScript.Identifier.Cigarettes);
					}
					if (num3 >= 4)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Dice_20);
						PowerupScript.Unlock(PowerupScript.Identifier.GiantShroom);
						PowerupScript.Unlock(PowerupScript.Identifier.FideltyCard);
						PowerupScript.Unlock(PowerupScript.Identifier.BrokenCalculator);
						PowerupScript.Unlock(PowerupScript.Identifier.AbstractPainting);
						PowerupScript.Unlock(PowerupScript.Identifier.Jimbo);
					}
					if (Data.game.doorOpenedCounter > 0)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.Nose);
						PowerupScript.Unlock(PowerupScript.Identifier.EyeJar);
						PowerupScript.Unlock(PowerupScript.Identifier.Pareidolia);
						PowerupScript.Unlock(PowerupScript.Identifier.Depression);
						PowerupScript.Unlock(PowerupScript.Identifier.LocomotiveDiesel);
						PowerupScript.Unlock(PowerupScript.Identifier.LocomotiveSteam);
						PowerupScript.Unlock(PowerupScript.Identifier.PotatoPower);
						PowerupScript.Unlock(PowerupScript.Identifier.MusicTape);
					}
					if (Data.game.doorOpenedCounter > 0 && Data.game.RunModifier_WonTimes_Get(RunModifierScript.Identifier.bigDebt) > 0)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.ConsolationPrize);
					}
					if (Data.game.goodEndingCounter > 0)
					{
						PowerupScript.Unlock(PowerupScript.Identifier.WeirdClock);
					}
				}
				GameplayData.NewGameIntroFinished_Set(true);
				Data.SaveGame(Data.GameSavingReason.introFinished, -1);
				break;
			}
			case GameplayMaster.IntroCutscenePhase.done:
				break;
			default:
				Debug.LogError("IntroCutscenePhase not handled: " + this.introCutscenePhase.ToString());
				break;
			}
		}
		this.interestsAndTicketsTimer -= Tick.Time;
		if (this.interestsAndTicketsPhase == GameplayMaster.InterestsAndTicketsPhase.done)
		{
			this.interestsAndTicketsTimer = 0f;
		}
		if (this.interestsAndTicketsPhase != GameplayMaster.InterestsAndTicketsPhase.done && this.interestsAndTicketsTimer <= 0f)
		{
			switch (this.interestsAndTicketsPhase)
			{
			case GameplayMaster.InterestsAndTicketsPhase.beforeInterestsAndClovers:
			{
				CameraController.SetPosition(CameraController.PositionKind.ATM, false, 1f * num);
				SlotMachineScript.Event onInterestEarn = SlotMachineScript.instance.OnInterestEarn;
				if (onInterestEarn != null)
				{
					onInterestEarn();
				}
				this.interestsAndTicketsPhase = GameplayMaster.InterestsAndTicketsPhase.interestsAtAtm;
				break;
			}
			case GameplayMaster.InterestsAndTicketsPhase.interestsAtAtm:
				if (!CameraController.IsCameraNearPositionAndAngle(2f))
				{
					return;
				}
				if (!this.interestsAwarded)
				{
					this.interestsAwarded = true;
					if (CameraController.GetPositionKind() != CameraController.PositionKind.ATM)
					{
						CameraController.SetPosition(CameraController.PositionKind.ATM, false, 1f * num);
					}
					if (identifier != RunModifierScript.Identifier.interestsGrow)
					{
						GameplayData.InterestEarnedGrow();
					}
					SlotMachineScript.Event onInterestEarnPost = SlotMachineScript.instance.OnInterestEarnPost;
					if (onInterestEarnPost != null)
					{
						onInterestEarnPost();
					}
					PowerupScript.PuppetPersonalTrainer_ShrinkBonus();
					PowerupScript.PuppetEletrician_ShrinkBonus();
					PowerupScript.PuppetFortuneTeller_ShrinkBonus();
					if (identifier != RunModifierScript.Identifier.redButtonOverload)
					{
						RedButtonScript.RestoreCharges(1);
					}
					this.interestsAndTicketsTimer = 0.5f;
				}
				else
				{
					if (this.interestsAndTicketsTimer > -0.5f)
					{
						flag = false;
					}
					if (MemoScript.IsEnabled() && !flag)
					{
						return;
					}
					if (!CameraController.IsCameraNearPositionAndAngle(2f))
					{
						return;
					}
					MemoScript.Close(false);
					this.intAndTickets_ShakedTrapdoor = false;
					this.interestsAndTicketsPhase = GameplayMaster.InterestsAndTicketsPhase.cloverTickets;
					this.interestsAndTicketsTimer = 0f;
					return;
				}
				break;
			case GameplayMaster.InterestsAndTicketsPhase.cloverTickets:
			{
				if (DialogueScript.IsEnabled())
				{
					return;
				}
				long num4 = GameplayData.CloverTickets_BonusBigBet_Get(true);
				if (GameplayData.LastBet_IsSmallGet())
				{
					num4 = GameplayData.CloverTickets_BonusLittleBet_Get(true);
				}
				if (num4 > 0L)
				{
					GameplayData.CloverTicketsAdd(num4, true);
					CameraController.SetPosition(CameraController.PositionKind.CloverTicketsMachine, false, 1f * num);
					this.interestsAndTicketsPhase = GameplayMaster.InterestsAndTicketsPhase.cloverTicketsWait;
					this.interestsAndTicketsTimer = 0f;
					return;
				}
				this.interestsAndTicketsPhase = GameplayMaster.InterestsAndTicketsPhase.shakeTrapdoor_Optional;
				this.interestsAndTicketsTimer = 0f;
				break;
			}
			case GameplayMaster.InterestsAndTicketsPhase.cloverTicketsWait:
				if (this.interestsAndTicketsTimer > -0.15f)
				{
					flag = false;
				}
				if (TicketMachineScript.IsTicketMachineRunning() && !flag)
				{
					return;
				}
				this.interestsAndTicketsPhase = GameplayMaster.InterestsAndTicketsPhase.shakeTrapdoor_Optional;
				this.interestsAndTicketsTimer = 0.25f;
				return;
			case GameplayMaster.InterestsAndTicketsPhase.shakeTrapdoor_Optional:
				if (!this.intAndTickets_ShakedTrapdoor && bigInteger <= 1L)
				{
					TrapdoorScript.SetAnimation(TrapdoorScript.AnimationKind.Shake);
					this.intAndTickets_ShakedTrapdoor = true;
					this.interestsAndTicketsTimer = 1.5f;
					CameraController.SetPosition(CameraController.PositionKind.TrapDoor, false, 1f * num);
				}
				if (bigInteger == 1L && !this._1RoundLeftWarned)
				{
					DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_1_ROUND_LEFT_WARNING" });
					this._1RoundLeftWarned = true;
				}
				if (bigInteger == 0L && !flag2)
				{
					DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_LAST_CHANCE_TO_DEPOSIT_" + Util.Choose<int>(new int[] { 0, 1, 2 }).ToString() });
				}
				Data.SaveGame(Data.GameSavingReason.endOfRound_AfterInterestsAndTicketsCutscene, -1);
				this.interestsAndTicketsPhase = GameplayMaster.InterestsAndTicketsPhase.done;
				this.interestsAndTicketsTimer = 0f;
				return;
			case GameplayMaster.InterestsAndTicketsPhase.done:
				break;
			default:
				Debug.LogError("ShowAtmPhase not handled: " + this.interestsAndTicketsPhase.ToString());
				break;
			}
		}
		if (this.interestsAndTicketsPhase != GameplayMaster.InterestsAndTicketsPhase.done)
		{
			return;
		}
		this.delay -= Tick.Time;
		if (this.delay > 0f)
		{
			return;
		}
		this.delay = 0.5f;
		GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.preparation, false, null);
	}

	// Token: 0x060002FD RID: 765 RVA: 0x00012C1A File Offset: 0x00010E1A
	private void IntroFinalization()
	{
	}

	// Token: 0x060002FE RID: 766 RVA: 0x00012C1C File Offset: 0x00010E1C
	private void TutorialPhaseBehaviour()
	{
	}

	// Token: 0x060002FF RID: 767 RVA: 0x00012C20 File Offset: 0x00010E20
	private void PreparationPhaseBehaviour()
	{
		float num = (float)Data.settings.transitionSpeed;
		BigInteger bigInteger = GameplayData.CoinsGet();
		BigInteger bigInteger2 = GameplayData.DepositGet();
		BigInteger bigInteger3 = GameplayData.DebtGet();
		if (DialogueScript.IsEnabled())
		{
			return;
		}
		if (PowerupTriggerAnimController.HasAnimations())
		{
			return;
		}
		if (ATMScript.DebtClearCutsceneIsPlaying())
		{
			return;
		}
		if (MemoryPackDealUI.IsDealRunnning())
		{
			return;
		}
		if (!GameplayData.SkeletonIsCompletedGet() && PowerupScript.list_EquippedSkeleton.Count >= 5)
		{
			GameplayData.SkeletonIsCompletedSet();
			GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.cutscene, false, null);
			CameraController.SetPosition(CameraController.PositionKind.SlotFromTop, false, 1f * num);
			Sound.Play("SoundSkeletonImpact", 1f, 1f);
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_SKELETON_COMPLETED_ALT_" + Util.Choose<int>(new int[] { 0, 1, 2 }).ToString() });
			return;
		}
		if (GameplayMaster.unlockPowerupFirstTimeDialogueBooked)
		{
			GameplayMaster.unlockPowerupFirstTimeDialogueBooked = false;
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_POWERUP_UNLOCK_FIRST_TIME" });
			return;
		}
		if (this._twitchAffiliationMessageBooked && !PhoneUiScript.IsEnabled())
		{
			this._twitchAffiliationMessageBooked = false;
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_TWITCH_ONLY_FOR_PARTNERS_OR_AFFILIATES" });
			return;
		}
		if (GameplayMaster.drawerFromDemoUnlocked)
		{
			GameplayMaster.drawerFromDemoUnlocked = false;
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_DRAWER_UNLOCKED_FROM_DEMO" });
			return;
		}
		if (CranePackScript.IsEnabled() && !GameplayMaster._cranePackDialogueShown)
		{
			CameraController.SetPosition(CameraController.PositionKind.CranePack, false, 1f);
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_CRANE_PACK_0" });
			DialogueScript.SetDialogueInputDelay(0.5f);
			DialogueScript dialogueScript = DialogueScript.instance;
			dialogueScript.onDialogueClose = (DialogueScript.AnswerCallback)Delegate.Combine(dialogueScript.onDialogueClose, new DialogueScript.AnswerCallback(delegate
			{
				CameraController.SetPosition(CameraController.PositionKind.Free, false, 0f);
			}));
			GameplayMaster._cranePackDialogueShown = true;
		}
		if (bigInteger + bigInteger2 < bigInteger3)
		{
			GameplayMaster.MemoryPack_DealIsOff_EvaluateFlag(true);
		}
		if (GameplayData.RoundsOfDeadline_PlayedGet() > 0)
		{
			GameplayMaster.MemoryPack_DealIsOff_EvaluateFlag(false);
		}
		if (GameplayMaster.MemoryPack_TheDealIsOff_FlagGet())
		{
			bool theDealIsOff_ByCoins = this._theDealIsOff_ByCoins;
			GameplayMaster.MemoryPack_TheDealIsOff_FlagSet(false, false);
			GameplayData.RunModifier_DealIsAvailable_Set(false);
			CameraController.SetPosition(CameraController.PositionKind.ATMStraight, false, 1f);
			DialogueScript.SetDialogue(false, new string[] { theDealIsOff_ByCoins ? "DIALOGUE_SKIP_DEADLINE_THE_DEAL_IS_OFF_BECAUSE_COINS" : "DIALOGUE_SKIP_DEADLINE_THE_DEAL_IS_OFF" });
			DialogueScript dialogueScript2 = DialogueScript.instance;
			dialogueScript2.onDialogueClose = (DialogueScript.AnswerCallback)Delegate.Combine(dialogueScript2.onDialogueClose, new DialogueScript.AnswerCallback(this._DealIsOff_CameraReset));
		}
		bool flag = this.IsDeathCondition(true, true);
		if ((flag || this.deathCountDownStarted) && !this.deathCountDownResetRequest)
		{
			if (!this.deathCountDownStarted)
			{
				this.deathCountDownStarted = true;
				CameraController.HeartbeatPlay_Default();
				DialogueScript.NextIsLegalDuringDeathCooldown();
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_DEATH_TIME_ALT_" + Util.Choose<int>(new int[] { 0, 1, 2, 3, 4 }).ToString() });
				DialogueScript.SetAutoClose(3f);
				DialogueScript.SetDialogueInputDelay(2f);
				FlashScreen.SpawnCamera(Color.red, 0.25f, 0.5f, CameraGame.firstInstance.myCamera, 0.5f);
				CameraGame.Shake(0.5f);
				CameraGame.ChromaticAberrationIntensitySet(1f);
				Sound.Play("SoundDeathApproachingAmbience", 1f, 1f);
				CameraController.SetPosition(CameraController.PositionKind.ATMStraight, false, 0f);
				GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.cutscene, false, null);
				return;
			}
			this.deathCountDownTimer -= Tick.Time;
			this.OnDeathCountdown_During();
			if (this.deathCountDownTimer <= 0f)
			{
				this.deathCountDownTimer += 2f;
				this.deathCountDown--;
				this.OnDeathCountdown_Tick(this.deathCountDown < 0);
				if (this.deathCountDown < 0)
				{
					this.deathCountDown = 0;
					bool flag2 = this.DieTry(GameplayMaster.DeathStep.lookAtAtm, true);
					this._DeathCountdownReset(!flag2);
					if (flag2)
					{
						Sound.Play("SoundDeathBell", 1f, 1f);
					}
					return;
				}
			}
		}
		else
		{
			this.deathCountDownResetRequest = false;
			if (this.deathCountDownStarted)
			{
				bool flag3 = this.deathCDResetRequest_RoundsFlag;
				this._DeathCountdownReset(true);
				Sound.Play("SoundDeathCountdownReset", 1f, 1f);
				FlashScreen.SpawnCamera(Color.yellow, 0.5f, 2f, CameraGame.firstInstance.myCamera, 0.5f);
				DialogueScript.NextIsLegalDuringDeathCooldown();
				if (!flag3)
				{
					DialogueScript.SetDialogue(false, new string[] { Util.Choose<string>(new string[] { "DIALOGUE_DEATH_COUNTDOWN_RESET_ALT_0", "DIALOGUE_DEATH_COUNTDOWN_RESET_ALT_1", "DIALOGUE_DEATH_COUNTDOWN_RESET_ALT_2" }) });
				}
				else
				{
					DialogueScript.SetDialogue(false, new string[] { Util.Choose<string>(new string[] { "DIALOGUE_DEATH_COUNTDOWN_RESET_BY_ROUNDS_INCREASE_ALT_0", "DIALOGUE_DEATH_COUNTDOWN_RESET_BY_ROUNDS_INCREASE_ALT_1", "DIALOGUE_DEATH_COUNTDOWN_RESET_BY_ROUNDS_INCREASE_ALT_2" }) });
				}
				DialogueScript.SetDialogueInputDelay(1.5f);
			}
		}
		if (flag)
		{
			int num2 = this.deathCountDown;
			return;
		}
	}

	// Token: 0x06000300 RID: 768 RVA: 0x000130C0 File Offset: 0x000112C0
	private void OnDeathCountdown_During()
	{
		float num = (5f - (float)this.deathCountDown) / 5f;
		CameraGame.Shake(num * 2f);
		CameraGame.ChromaticAberrationIntensitySet(num * 0.5f);
		if (CameraController.GetPositionKind() == CameraController.PositionKind.Free)
		{
			CameraGame.FieldOfViewExtraSet("DCD", num * 20f);
		}
	}

	// Token: 0x06000301 RID: 769 RVA: 0x00013114 File Offset: 0x00011314
	private void OnDeathCountdown_Tick(bool finalTick)
	{
		float num = (5f - (float)this.deathCountDown) / 5f;
		if (!finalTick)
		{
			FlashScreen.SpawnCamera(Color.red, num * 0.75f, 1f, CameraGame.firstInstance.myCamera, 1f);
		}
		else
		{
			FlashScreen.SpawnCamera(Color.red, 1f, 1f, CameraGame.firstInstance.myCamera, 1f);
		}
		CameraGame.ChromaticAberrationIntensitySet(num);
		if (!finalTick)
		{
			Sound.Play("SoundDeathCountdownAlarm", 1f, 1f);
		}
	}

	// Token: 0x06000302 RID: 770 RVA: 0x000131A2 File Offset: 0x000113A2
	private void _DealIsOff_CameraReset()
	{
		CameraController.SetPosition(CameraController.PositionKind.Free, false, 0f);
	}

	// Token: 0x06000303 RID: 771 RVA: 0x000131B0 File Offset: 0x000113B0
	private void EquippingPhaseBehaviour()
	{
	}

	// Token: 0x06000304 RID: 772 RVA: 0x000131B2 File Offset: 0x000113B2
	private void GamblingPhaseBehaviour()
	{
	}

	// Token: 0x06000305 RID: 773 RVA: 0x000131B4 File Offset: 0x000113B4
	private void DeathPhaseBehaviour()
	{
		bool flag = GameplayMaster.IsCustomSeed();
		float num = (float)Data.settings.transitionSpeed;
		if (GameplayMaster.restartQuickDeath)
		{
			num = 4f;
		}
		if (ScreenMenuScript.IsEnabled())
		{
			return;
		}
		if (DialogueScript.IsEnabled())
		{
			return;
		}
		if (PowerupScript.CameraIsInspecting())
		{
			return;
		}
		if (TerminalScript.IsLoggedIn())
		{
			return;
		}
		if (PhoneUiScript.IsEnabled())
		{
			return;
		}
		if (ToyPhoneUIScript.IsEnabled())
		{
			return;
		}
		if (MainMenuScript.IsEnabled())
		{
			return;
		}
		if (MagazineUiScript.IsEnabled())
		{
			return;
		}
		if (WCScript.IsPerformingAction())
		{
			return;
		}
		if (DeckBoxUI.IsEnabled())
		{
			return;
		}
		if (PowerupTriggerAnimController.HasAnimations())
		{
			return;
		}
		this.deathStepTimer -= Tick.Time;
		if (this.deathStepTimer > 0f)
		{
			return;
		}
		if (this._deathFadeOutTensionSound)
		{
			Sound.SoundCapsule soundCapsule = Sound.Find("SoundTensionViolinLong");
			if (soundCapsule != null)
			{
				soundCapsule.localVolume -= Tick.Time;
			}
		}
		if (Sound.IsPlaying("SoundDeathApproachingAmbience"))
		{
			CameraGame.Shake(2f);
		}
		switch (this.deathStep)
		{
		case GameplayMaster.DeathStep.lookAtAtm:
			CameraController.SetPosition(CameraController.PositionKind.ATMStraight, false, 1f * num);
			Sound.Play("SoundTensionViolinLong", 1f, 1f);
			this.deathStep = GameplayMaster.DeathStep.lookAtTrapdoor;
			this.deathStepTimer = 3f / num;
			return;
		case GameplayMaster.DeathStep.lookAtTrapdoor:
			CameraController.SetPosition(CameraController.PositionKind.TrapDoor, false, 1f * num);
			Music.StopAll();
			TrapdoorScript.SetAnimation(TrapdoorScript.AnimationKind.Shake);
			this.deathStep = GameplayMaster.DeathStep.startFalling;
			this.deathStepTimer = 0.5f / num;
			return;
		case GameplayMaster.DeathStep.startFalling:
			CameraController.SetPosition(CameraController.PositionKind.Falling, false, 1f * num);
			TrapdoorScript.SetAnimation(TrapdoorScript.AnimationKind.Open);
			CameraGame.Shake(2f);
			Sound.Play("SoundTrapdoorFallingWind", 1f, 1f);
			this.deathStep = GameplayMaster.DeathStep.falling;
			return;
		case GameplayMaster.DeathStep.falling:
			if (CameraController.DeathFallDone())
			{
				FlashScreen.SpawnCamera(new Color(0.75f, 0f, 0f, 1f), 1f, 10f, CameraGame.list[0].myCamera, 0.5f);
				Sound.Stop("SoundTrapdoorFallingWind", true);
				Sound.Stop("SoundTensionViolinLong", true);
				Sound.Stop("SoundDeathApproachingAmbience", true);
				Sound.Play("SoundTrapdoorDeathHit", 1f, 1f);
				CameraController.HeartbeatPlay_Slow();
				Controls.VibrationSet_PreferMax(this.player, 1f);
				if (!GameplayMaster.restartQuickDeath)
				{
					StatsScript.Open(StatsScript.ShowKind.endDeath);
				}
				else
				{
					this.deathStepTimer = 0.5f;
				}
				this.deathStep = GameplayMaster.DeathStep.done;
				return;
			}
			break;
		case GameplayMaster.DeathStep.done:
			if (!StatsScript.IsEnabled())
			{
				if (Master.IsDemo)
				{
					LoadingScreenCallToAction.BookCallToAction();
				}
				else if (!flag)
				{
					string text = null;
					if (GameplayData.IsInVictoryCondition())
					{
						int drawersUnlockedNum_Local = DrawersScript.GetDrawersUnlockedNum_Local();
						if (drawersUnlockedNum_Local > 0)
						{
							switch (drawersUnlockedNum_Local)
							{
							case 1:
								text = "LOADING_NOTIFICATION_AFTER_DRAWER_0";
								break;
							case 2:
								text = "LOADING_NOTIFICATION_AFTER_DRAWER_1";
								break;
							case 3:
								text = "LOADING_NOTIFICATION_AFTER_DRAWER_2";
								break;
							case 4:
								text = "LOADING_NOTIFICATION_AFTER_DRAWER_3";
								break;
							}
						}
					}
					else if (Data.game.goodEndingCounter > 0)
					{
						switch (GameplayMaster.provocativeSentencesCounter_AfterGoodEnding)
						{
						case 0:
							text = "LOADING_NOTIFICATION_PROVOCATIVE_AFTER_GOOD_ENDING_0";
							break;
						case 2:
							text = "LOADING_NOTIFICATION_PROVOCATIVE_AFTER_GOOD_ENDING_1";
							break;
						case 4:
							text = "LOADING_NOTIFICATION_PROVOCATIVE_AFTER_GOOD_ENDING_2";
							break;
						}
						GameplayMaster.provocativeSentencesCounter_AfterGoodEnding++;
					}
					else
					{
						switch (GameplayMaster.provocativeSentencesCounter_PreGoodEnding)
						{
						case 0:
							text = "LOADING_NOTIFICATION_PROVOCATIVE_0";
							break;
						case 2:
							text = "LOADING_NOTIFICATION_PROVOCATIVE_1";
							break;
						case 4:
							text = "LOADING_NOTIFICATION_PROVOCATIVE_2";
							break;
						case 6:
							text = "LOADING_NOTIFICATION_PROVOCATIVE_3";
							break;
						case 8:
							text = "LOADING_NOTIFICATION_PROVOCATIVE_4";
							break;
						case 10:
							text = "LOADING_NOTIFICATION_PROVOCATIVE_5";
							break;
						}
						GameplayMaster.provocativeSentencesCounter_PreGoodEnding++;
					}
					if (GameplayMaster.GameIsResetting())
					{
						text = null;
						GameplayMaster.provocativeSentencesCounter_AfterGoodEnding = 0;
						GameplayMaster.provocativeSentencesCounter_PreGoodEnding = 0;
					}
					if (!string.IsNullOrEmpty(text))
					{
						LoadingScreenNotifications.SetNotification(text);
					}
				}
				if (!this.deathSaved)
				{
					this.deathSaved = true;
					Data.game.GameplayDataReset(true);
					if (!flag)
					{
						Data.game.deathsDone++;
					}
					if (GameplayMaster.GameIsResetting())
					{
						Data.DeleteGameData(Data.GameDataIndex);
					}
					if (!GameplayMaster.GameIsResetting())
					{
						Data.SaveGame(Data.GameSavingReason.death, -1);
					}
				}
				if (!PlatformDataMaster.IsSaving())
				{
					Sound.StopAll();
					Level.Restart(true);
					return;
				}
			}
			break;
		default:
			Debug.LogError("DeathStep not handled: " + this.deathStep.ToString());
			break;
		}
	}

	// Token: 0x06000306 RID: 774 RVA: 0x0001361C File Offset: 0x0001181C
	private void EndingWithoutDeathPhaseBehaviour()
	{
		if (DialogueScript.IsEnabled())
		{
			return;
		}
		switch (this.endingWithoutDeath_Phase)
		{
		case GameplayMaster.EndingWithoutDeath_Phase.begin:
			this.endingCoroutine = base.StartCoroutine(this.EndingCoroutine_Main());
			this.endingWithoutDeath_Phase = GameplayMaster.EndingWithoutDeath_Phase.waitingCoroutine;
			return;
		case GameplayMaster.EndingWithoutDeath_Phase.waitingCoroutine:
			if (this.endingCoroutine == null)
			{
				this.endingWithoutDeath_Phase = GameplayMaster.EndingWithoutDeath_Phase.done;
				return;
			}
			break;
		case GameplayMaster.EndingWithoutDeath_Phase.done:
			Level.Restart(true);
			return;
		default:
			Debug.LogError("EndingWithoutDeath_Phase not handled: " + this.endingWithoutDeath_Phase.ToString());
			break;
		}
	}

	// Token: 0x06000307 RID: 775 RVA: 0x0001369D File Offset: 0x0001189D
	private IEnumerator EndingCoroutine_Main()
	{
		bool isGoodEnding = GameplayData.IsInGoodEndingCondition(false);
		bool creditsAreSkippable = Data.game.creditsSeenOnce;
		Data.game.creditsSeenOnce = true;
		MenuDrawerScript.RemoveDirectAction();
		yield return this.EndingWithoutDeath_CommonEarlyCoroutine();
		float timer = 3f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		Sound.Play_Unpausable("SoundMenuPopUp", 1f, 1f);
		StatsScript.Open(StatsScript.ShowKind.endAlive);
		EndingAreaScript.DetermineControlPanel();
		while (StatsScript.IsEnabled())
		{
			yield return null;
		}
		yield return this.EndingWithoutDeath_Saving();
		if (isGoodEnding && Data.game.goodEndingCounter == 1)
		{
			LoadingScreenNotifications.SetNotification("LOADING_NOTIFICATION_POSITIVE_MESSAGE_GOOD_ENDING");
		}
		else if (!isGoodEnding && Data.game.badEndingCounter == 1)
		{
			LoadingScreenNotifications.SetNotification("LOADING_NOTIFICATION_POSITIVE_MESSAGE_BAD_ENDING");
		}
		timer = 3f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		PlayerScript.instanceP1.transform.position = EndingAreaScript.instance.spawnPoint.position;
		CameraController.instance.freeCamTransform.position = EndingAreaScript.instance.spawnPoint.position + new Vector3(0f, 6f, 0f);
		CameraController.SetFreeCameraRotation(new Vector3(0f, 0f, 0f));
		CameraController.SetPosition(CameraController.PositionKind.Free, true, 1f);
		CameraController.DisableReason_Add("endHold");
		RenderSettings.fogColor = Colors.GetColor("ending_atmo");
		RenderSettings.fogStartDistance = 50f;
		RenderSettings.fogEndDistance = 320f;
		CameraGame.firstInstance.myCamera.backgroundColor = RenderSettings.fogColor;
		CameraGame.firstInstance.myCamera.farClipPlane = 320f;
		CameraGame.SetColorDepth(24);
		Music.Play("SoundtrackLoopAmbiencePillars");
		Music.SetVolumeFadeInstant(0f);
		Music.SetVolumeFade(1f, 0.25f);
		FlashScreen.SpawnCamera(Color.black, 1f, 0.5f, CameraGame.firstInstance.myCamera, 0.5f);
		while (FlashScreen.instanceLast != null)
		{
			yield return null;
		}
		CameraController.DisableReason_Remove("endHold");
		GameplayMaster.EndingFreeRoaming = true;
		if (isGoodEnding)
		{
			yield return this.EndingCoroutine_Good();
		}
		else
		{
			yield return this.EndingCoroutine_Bad();
		}
		FlashScreen.SpawnCamera(Color.black, 1f, 4f, CameraGame.firstInstance.myCamera, 0.5f);
		CameraController.SetPosition(CameraController.PositionKind.doorEndingScene, true, 1f);
		CameraGame.firstInstance.myCamera.backgroundColor = Color.black;
		Music.StopAll();
		Sound.Play_Unpausable("SoundExtraKick", 1f, 1f);
		timer = 0.5f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		if (isGoodEnding)
		{
			Sound.Play("SoundEndingElevatorTurnOn", 1f, 1f);
		}
		else
		{
			Sound.Play("SoundSlotMachineStartupJingle", 1f, 1f);
		}
		timer = 5f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		if (isGoodEnding)
		{
			Music.Play("SoundtrackEndingSeaAndSeagulls");
			Music.Find("SoundtrackEndingSeaAndSeagulls").myAudioSource.loop = false;
		}
		else
		{
			Music.Play("OstCredits");
			Music.Find("OstCredits").myAudioSource.loop = false;
		}
		EndingCreditsScript.Open(isGoodEnding, creditsAreSkippable);
		while (EndingCreditsScript.IsEnabled())
		{
			yield return null;
		}
		Music.StopAll();
		timer = 1f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		this.EndingWithoutDeath_CommonEnding();
		this.endingCoroutine = null;
		yield break;
	}

	// Token: 0x06000308 RID: 776 RVA: 0x000136AC File Offset: 0x000118AC
	private IEnumerator EndingCoroutine_Bad()
	{
		while (!this._endingGotoCreditsFlag)
		{
			yield return null;
		}
		GameplayMaster.EndingFreeRoaming = false;
		yield return null;
		yield break;
	}

	// Token: 0x06000309 RID: 777 RVA: 0x000136BB File Offset: 0x000118BB
	private IEnumerator EndingCoroutine_Good()
	{
		while (!this._endingGotoCreditsFlag)
		{
			yield return null;
		}
		GameplayMaster.EndingFreeRoaming = false;
		yield return null;
		yield break;
	}

	// Token: 0x0600030A RID: 778 RVA: 0x000136CA File Offset: 0x000118CA
	private IEnumerator EndingWithoutDeath_CommonEarlyCoroutine()
	{
		while (DialogueScript.IsEnabled())
		{
			yield return null;
		}
		if (GameplayData.IsInGoodEndingCondition(true))
		{
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_DOOR_OPEN_GOOD_ENDING" });
		}
		else
		{
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_DOOR_OPEN_BAD_ENDING" });
		}
		DialogueScript.SetDialogueInputDelay(0.5f);
		Sound.Play("SoundEndingAtmosphereBeforeOpeningDoor", 1f, 1f);
		while (DialogueScript.IsEnabled())
		{
			float num = Util.AngleSin(Tick.PassedTime * 180f);
			CameraGame.ChromaticAberrationIntensitySet(0.5f + num * 0.5f);
			CameraGame.FieldOfViewExtraSet("ewdcec", 7.5f + num * 7.5f);
			yield return null;
		}
		Sound.Stop("SoundEndingAtmosphereBeforeOpeningDoor", true);
		Sound.Play("SoundDoorOpen", 1f, 1f);
		Music.StopAll();
		FlashScreen.SpawnCamera(Color.black, 1f, 4f, CameraGame.firstInstance.myCamera, 0.5f);
		CameraController.SetPosition(CameraController.PositionKind.doorEndingScene, true, 1f);
		yield break;
	}

	// Token: 0x0600030B RID: 779 RVA: 0x000136D2 File Offset: 0x000118D2
	private IEnumerator EndingWithoutDeath_Saving()
	{
		bool flag = GameplayMaster.IsCustomSeed();
		if (!flag)
		{
			RunModifierScript.Identifier identifier = GameplayData.RunModifier_GetCurrent();
			Data.game.RunModifier_WonTimes_Set(identifier, Data.game.RunModifier_WonTimes_Get(identifier) + 1);
		}
		if (!flag)
		{
			Data.game.doorOpenedCounter++;
			if (GameplayData.IsInGoodEndingCondition(true))
			{
				Data.game.goodEndingCounter++;
			}
			else
			{
				Data.game.bookedBadEndingDialogue = true;
				Data.game.badEndingCounter++;
			}
		}
		Data.game.GameplayDataReset(true);
		Data.SaveGame(Data.GameSavingReason.endingWithoutDeath, -1);
		while (PlatformDataMaster.IsSaving())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600030C RID: 780 RVA: 0x000136DA File Offset: 0x000118DA
	private void EndingWithoutDeath_CommonEnding()
	{
		GeneralUiScript.ComingFromVictoryFlag_Set(true);
	}

	// Token: 0x0600030D RID: 781 RVA: 0x000136E2 File Offset: 0x000118E2
	private void ClosingGamePhaseBehaviour()
	{
		if (PlatformDataMaster.IsSaving())
		{
			return;
		}
		Application.Quit();
	}

	// Token: 0x0600030E RID: 782 RVA: 0x000136F1 File Offset: 0x000118F1
	private void TerminalPhaseBehaviour()
	{
	}

	// Token: 0x0600030F RID: 783 RVA: 0x000136F3 File Offset: 0x000118F3
	private void PhonePhaseBehaviour()
	{
	}

	// Token: 0x06000310 RID: 784 RVA: 0x000136F5 File Offset: 0x000118F5
	public static bool IsCustomSeed()
	{
		return !(GameplayMaster.instance == null) && GameplayMaster.instance._isCustomSeedRun;
	}

	// Token: 0x06000311 RID: 785 RVA: 0x00013710 File Offset: 0x00011910
	public static bool CanInputSeed()
	{
		return Data.game != null && Data.game.doorOpenedCounter > 0;
	}

	// Token: 0x06000312 RID: 786 RVA: 0x00013728 File Offset: 0x00011928
	public static BigInteger SlotAnimationCoinsGet()
	{
		return GameplayMaster.instance._slotAnimationCoins_AnimationOnly;
	}

	// Token: 0x06000313 RID: 787 RVA: 0x00013734 File Offset: 0x00011934
	public static void SlotAnimationCoinsSet(BigInteger ammount)
	{
		GameplayMaster.instance._slotAnimationCoins_AnimationOnly = ammount;
	}

	// (get) Token: 0x06000314 RID: 788 RVA: 0x00013741 File Offset: 0x00011941
	// (set) Token: 0x06000315 RID: 789 RVA: 0x0001374D File Offset: 0x0001194D
	public static bool EndingFreeRoaming
	{
		get
		{
			return GameplayMaster.instance._endingFreeRoaming;
		}
		set
		{
			GameplayMaster.instance._endingFreeRoaming = value;
		}
	}

	// Token: 0x06000316 RID: 790 RVA: 0x0001375A File Offset: 0x0001195A
	public void EndingGotoCredits_FlagSet()
	{
		this._endingGotoCreditsFlag = true;
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00013763 File Offset: 0x00011963
	public static bool MemoryPack_TheDealIsOff_FlagGet()
	{
		return !(GameplayMaster.instance == null) && GameplayMaster.instance._theDealIsOff_BookedDialogue;
	}

	// Token: 0x06000318 RID: 792 RVA: 0x0001377E File Offset: 0x0001197E
	public static void MemoryPack_TheDealIsOff_FlagSet(bool setFlag, bool setOffByCoins)
	{
		if (GameplayMaster.instance == null)
		{
			return;
		}
		GameplayMaster.instance._theDealIsOff_BookedDialogue = setFlag;
		GameplayMaster.instance._theDealIsOff_ByCoins = setOffByCoins;
	}

	// Token: 0x06000319 RID: 793 RVA: 0x000137A4 File Offset: 0x000119A4
	public static void MemoryPack_DealIsOff_EvaluateFlag(bool offByCoins)
	{
		if (GameplayMaster.instance == null)
		{
			return;
		}
		if (GameplayData.RunModifier_DealIsAvailable_Get())
		{
			GameplayMaster.MemoryPack_TheDealIsOff_FlagSet(true, offByCoins);
		}
	}

	// Token: 0x0600031A RID: 794 RVA: 0x000137C2 File Offset: 0x000119C2
	public static int SpinsDoneSinceStartup_Get()
	{
		return GameplayMaster.spinsDoneSinceStartup;
	}

	// Token: 0x0600031B RID: 795 RVA: 0x000137C9 File Offset: 0x000119C9
	public static void SpinsDoneSinceStartup_Increment()
	{
		GameplayMaster.spinsDoneSinceStartup++;
	}

	// Token: 0x0600031C RID: 796 RVA: 0x000137D7 File Offset: 0x000119D7
	public static void TwitchAffiliationMessageBook()
	{
		if (GameplayMaster.instance == null)
		{
			return;
		}
		GameplayMaster.instance._twitchAffiliationMessageBooked = true;
	}

	// Token: 0x0600031D RID: 797 RVA: 0x000137F2 File Offset: 0x000119F2
	public static void FailsafeOverDeposit_SetTriggered()
	{
		GameplayMaster._failsafeTriggered_NeverDeposited = true;
	}

	// Token: 0x0600031E RID: 798 RVA: 0x000137FA File Offset: 0x000119FA
	public static void FailsafeCharms_SetTriggered()
	{
		GameplayMaster._failsafeTriggered_NeverEquippedACharm = true;
	}

	// Token: 0x0600031F RID: 799 RVA: 0x00013802 File Offset: 0x00011A02
	public static void GameDataReset_FlagIt()
	{
		GameplayMaster.instance._dataResetFlag = true;
		GameplayMaster.spinsDoneSinceStartup = 0;
		GameplayMaster.deathsNumSinceStartup = 0;
		GameplayMaster._failsafeTriggered_OverDeposit = false;
		GameplayMaster._failsafeTriggered_NeverDeposited = false;
		GameplayMaster._failsafeTriggered_NeverEquippedACharm = false;
		LoadingScreenNotifications.ClearNotifications();
	}

	// Token: 0x06000320 RID: 800 RVA: 0x00013832 File Offset: 0x00011A32
	public static bool GameIsResetting()
	{
		return !(GameplayMaster.instance == null) && GameplayMaster.instance._dataResetFlag;
	}

	// Token: 0x06000321 RID: 801 RVA: 0x00013850 File Offset: 0x00011A50
	public void FCall_SlotMachineTurnOnTry()
	{
		int num = GameplayData.RoundsLeftToDeadline();
		int drawersUnlockedNum = DrawersScript.GetDrawersUnlockedNum();
		RunModifierScript.Identifier identifier = GameplayData.RunModifier_GetCurrent();
		if (num > 0)
		{
			if (identifier != RunModifierScript.Identifier.oneRoundPerDeadline)
			{
				if ((num == 2 || num == 1) && !GameplayMaster._failsafeTriggered_NeverDeposited && drawersUnlockedNum == 0 && !this.dialogueJustInterruptedTurnOn)
				{
					BigInteger bigInteger = GameplayData.CoinsGet();
					BigInteger bigInteger2 = GameplayData.NextDepositAmmountGet(true);
					BigInteger bigInteger3 = GameplayData.SpinCostMax_Get();
					if (bigInteger > bigInteger3 + bigInteger2 * 2)
					{
						CameraController.SetPosition(CameraController.PositionKind.ATMStraight, false, 0f);
						DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_FAILSAFE_DEPOSIT_INTEREST_REMINDER" });
						DialogueScript dialogueScript = DialogueScript.instance;
						dialogueScript.onDialogueClose = (DialogueScript.AnswerCallback)Delegate.Combine(dialogueScript.onDialogueClose, new DialogueScript.AnswerCallback(delegate
						{
							CameraController.SetPosition(CameraController.PositionKind.Free, false, 0f);
						}));
						GameplayMaster._failsafeTriggered_NeverDeposited = true;
						this.dialogueJustInterruptedTurnOn = true;
						return;
					}
				}
				if (num < 3 && !GameplayMaster._failsafeTriggered_NeverEquippedACharm && drawersUnlockedNum == 0 && !this.dialogueJustInterruptedTurnOn && StoreCapsuleScript.CanBuyAnything())
				{
					CameraController.SetPosition(CameraController.PositionKind.Store, false, 0f);
					DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_FAILSAFE_CHARMS_REMINDER" });
					DialogueScript dialogueScript2 = DialogueScript.instance;
					dialogueScript2.onDialogueClose = (DialogueScript.AnswerCallback)Delegate.Combine(dialogueScript2.onDialogueClose, new DialogueScript.AnswerCallback(delegate
					{
						CameraController.SetPosition(CameraController.PositionKind.Free, false, 0f);
					}));
					GameplayMaster._failsafeTriggered_NeverEquippedACharm = true;
					this.dialogueJustInterruptedTurnOn = true;
					return;
				}
			}
			int hypotehticalMaxSpinsBuyable = GameplayData.GetHypotehticalMaxSpinsBuyable();
			GameplayData.GetHypotehticalMidSpinsBuyable();
			ScreenMenuScript.Positioning positioning = ScreenMenuScript.Positioning.centerDownLittle;
			if (hypotehticalMaxSpinsBuyable <= 0)
			{
				ScreenMenuScript.OptionEvent[] array = new ScreenMenuScript.OptionEvent[2];
				ScreenMenuScript.OptionEvent[] array2 = array;
				int num2 = 0;
				array2[num2] = (ScreenMenuScript.OptionEvent)Delegate.Combine(array2[num2], new ScreenMenuScript.OptionEvent(this._GotoGambling_Free));
				ScreenMenuScript.OptionEvent[] array3 = array;
				int num3 = 1;
				array3[num3] = (ScreenMenuScript.OptionEvent)Delegate.Combine(array3[num3], new ScreenMenuScript.OptionEvent(this._CancelSlotMachine));
				ScreenMenuScript.Open(true, true, 1, positioning, 0f, Translation.Get("SCREEN_MENU_TITLE_BET"), new string[]
				{
					Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_OPTION_BET_OFFERED"), Strings.SanitizationSubKind.none),
					Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_OPTION_BET_CANCEL"), Strings.SanitizationSubKind.none)
				}, array);
			}
			else if (hypotehticalMaxSpinsBuyable == 1)
			{
				ScreenMenuScript.OptionEvent[] array4 = new ScreenMenuScript.OptionEvent[2];
				ScreenMenuScript.OptionEvent[] array5 = array4;
				int num4 = 0;
				array5[num4] = (ScreenMenuScript.OptionEvent)Delegate.Combine(array5[num4], new ScreenMenuScript.OptionEvent(this._GotoGambling_Max));
				ScreenMenuScript.OptionEvent[] array6 = array4;
				int num5 = 1;
				array6[num5] = (ScreenMenuScript.OptionEvent)Delegate.Combine(array6[num5], new ScreenMenuScript.OptionEvent(this._CancelSlotMachine));
				ScreenMenuScript.Open(true, true, 1, positioning, 0f, Translation.Get("SCREEN_MENU_TITLE_BET"), new string[]
				{
					Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_OPTION_BET_FULL"), Strings.SanitizationSubKind.none),
					Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_OPTION_BET_CANCEL"), Strings.SanitizationSubKind.none)
				}, array4);
			}
			else
			{
				ScreenMenuScript.OptionEvent[] array7 = new ScreenMenuScript.OptionEvent[3];
				ScreenMenuScript.OptionEvent[] array8 = array7;
				int num6 = 0;
				array8[num6] = (ScreenMenuScript.OptionEvent)Delegate.Combine(array8[num6], new ScreenMenuScript.OptionEvent(this._GotoGambling_Max));
				ScreenMenuScript.OptionEvent[] array9 = array7;
				int num7 = 1;
				array9[num7] = (ScreenMenuScript.OptionEvent)Delegate.Combine(array9[num7], new ScreenMenuScript.OptionEvent(this._GotoGambling_Mid));
				ScreenMenuScript.OptionEvent[] array10 = array7;
				int num8 = 2;
				array10[num8] = (ScreenMenuScript.OptionEvent)Delegate.Combine(array10[num8], new ScreenMenuScript.OptionEvent(this._CancelSlotMachine));
				ScreenMenuScript.Open(true, true, 2, positioning, 0f, Translation.Get("SCREEN_MENU_TITLE_BET"), new string[]
				{
					Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_OPTION_BET_FULL"), Strings.SanitizationSubKind.none),
					Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_OPTION_BET_HALF"), Strings.SanitizationSubKind.none),
					Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_OPTION_BET_CANCEL"), Strings.SanitizationSubKind.none)
				}, array7);
			}
			ScreenMenuScript.HideShadow();
			ScreenMenuScript.HideWhenCameraIsMoving(true);
			ScreenMenuScript.AudioClipsSet("SoundSlotMenuShowUp", "SoundSlotMenuHover", "SoundSlotMenuSelect", "SoundSlotMenuBack");
			VirtualCursors.CursorDesiredVisibilitySet(0, true);
			CameraController.SetPosition(CameraController.PositionKind.SlotScreenCloseUp, false, 2f);
			CameraController.ScreenMenuIgnore_SetReason("slotPickSpins");
			SlotMachineScript.instance.TopTextSet_RoundsOverMaxRounds();
			return;
		}
		CameraGame.Shake(1f);
		Sound.Play("SoundMenuError", 1f, 1f);
	}

	// Token: 0x06000322 RID: 802 RVA: 0x00013C46 File Offset: 0x00011E46
	private void _GotoGambling_Free()
	{
		this._BetFree();
		this._GotoGambling();
	}

	// Token: 0x06000323 RID: 803 RVA: 0x00013C54 File Offset: 0x00011E54
	private void _GotoGambling_Mid()
	{
		this._BetMid();
		this._GotoGambling();
	}

	// Token: 0x06000324 RID: 804 RVA: 0x00013C62 File Offset: 0x00011E62
	private void _GotoGambling_Max()
	{
		this._BetMax();
		this._GotoGambling();
	}

	// Token: 0x06000325 RID: 805 RVA: 0x00013C70 File Offset: 0x00011E70
	private void _GotoGambling()
	{
		BigInteger bigInteger = GameplayData.DebtIndexGet();
		int num = GameplayData.SixSixSix_GetMinimumDebtIndex().CastToInt();
		bool flag = bigInteger >= (long)num;
		int num2 = (int)GameplayData.RunModifier_GetCurrent();
		SlotMachineScript.instance.TurnOn();
		GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.gambling, false, null);
		this.dialogueJustInterruptedTurnOn = false;
		GameplayData.RoundDeadlineTrail_Increment();
		GameplayData.RoundsReallyPlayedIncrement();
		SlotMachineScript.FirstSpinFlagReset_ToTrue();
		SlotMachineScript.instance._666RoundLostCoinsReset();
		if (num2 == 13 && GameplayData.RoundsLeftToDeadline() == 0 && flag)
		{
			GameplayData.SixSixSix_BookedSpinSet(R.Rng_RunMod.Range(0, GameplayData.SpinsLeftGet()));
			RunModifierScript.TriggerAnimation_IfEquipped(RunModifierScript.Identifier._666LastRoundGuaranteed);
		}
		if (flag)
		{
			RunModifierScript.TriggerAnimation_IfEquipped(RunModifierScript.Identifier._666BigBetDouble_SmallBetNoone);
		}
		GameplayData.LastRoundHad666Or999 = false;
		RedButtonScript.ResetTiming(RedButtonScript.RegistrationCapsule.Timing.perRound);
		DeadlineBonusScreen.UpdateValues();
		RedButtonScript.ButtonVisualsRefresh();
		GameplayMaster.MemoryPack_DealIsOff_EvaluateFlag(false);
		if (GameplayData.WinConditionAlreadyAchieved() && !GameplayData.KeptPlayingPastWinConditionGet())
		{
			GameplayData.KeptPlayingPastWinConditionSet();
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_REWARD_BOX_KEEP_PLAYING_INSTEAD_0", "DIALOGUE_REWARD_BOX_KEEP_PLAYING_INSTEAD_1" });
		}
		GameplayData.Phone_SpeciallCallBooking_Reset();
		CameraController.ScreenMenuIgnore_RemoveReason("slotPickSpins");
		Data.SaveGame(Data.GameSavingReason.beginOfPlayingAtTheSlotMachine, -1);
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00013D68 File Offset: 0x00011F68
	private void _BetFree()
	{
		int num = GameplayData.ExtraSpinsGet(true);
		GameplayData.SpinsLeftSet(Mathf.Max(1 + num, 1));
		GameplayMaster.SlotAnimationCoinsSet(0);
		GameplayData.LastBet_IsBigSet();
		GameplayData.SmallAndBigBet_CountIncrease(0, 1);
	}

	// Token: 0x06000327 RID: 807 RVA: 0x00013DA4 File Offset: 0x00011FA4
	private void _BetMid()
	{
		int hypotehticalMidSpinsBuyable = GameplayData.GetHypotehticalMidSpinsBuyable();
		BigInteger bigInteger = GameplayData.SpinCostMid_Get();
		int num = GameplayData.ExtraSpinsGet(true);
		GameplayData.LastBet_IsSmallSet();
		GameplayData.SmallAndBigBet_CountIncrease(1, 0);
		PowerupScript.Garbage_TryTriggeringAnimation();
		Data.GameData game = Data.game;
		int unlockSteps_Garbage = game.UnlockSteps_Garbage;
		game.UnlockSteps_Garbage = unlockSteps_Garbage + 1;
		GameplayData.SpinsLeftSet(Mathf.Max(hypotehticalMidSpinsBuyable + num, 1));
		GameplayData.CoinsAdd(-bigInteger, false);
		GameplayMaster.SlotAnimationCoinsSet(bigInteger);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00013E08 File Offset: 0x00012008
	private void _BetMax()
	{
		int hypotehticalMaxSpinsBuyable = GameplayData.GetHypotehticalMaxSpinsBuyable();
		BigInteger bigInteger = GameplayData.SpinCostMax_Get();
		int num = GameplayData.ExtraSpinsGet(true);
		GameplayData.LastBet_IsBigSet();
		GameplayData.SmallAndBigBet_CountIncrease(0, 1);
		GameplayData.SpinsLeftSet(Mathf.Max(hypotehticalMaxSpinsBuyable + num, 1));
		GameplayData.CoinsAdd(-bigInteger, false);
		GameplayMaster.SlotAnimationCoinsSet(bigInteger);
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00013E52 File Offset: 0x00012052
	private void _CancelSlotMachine()
	{
		VirtualCursors.CursorDesiredVisibilitySet(0, false);
		CameraController.SetPosition(CameraController.PositionKind.Free, false, 1f);
		CameraController.ScreenMenuIgnore_RemoveReason("slotPickSpins");
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00013E74 File Offset: 0x00012074
	public void FCall_StopPlaying()
	{
		this.interestsAwarded = false;
		RedButtonScript.ResetTiming(RedButtonScript.RegistrationCapsule.Timing.perRound);
		this.interestsAndTicketsPhase = GameplayMaster.InterestsAndTicketsPhase.beforeInterestsAndClovers;
		bool flag = PowerupScript.IsUnlocked(PowerupScript.Identifier.Skeleton_Head) && GameplayData.LastRoundHad666Or999;
		if (RewardBoxScript.GetRewardKind() != RewardBoxScript.RewardKind.DoorKey)
		{
			flag = false;
		}
		if (flag)
		{
			AbilityScript.AFunc_OnPick_EvilGeneric_SpawnSkeletonPiece(null);
		}
		GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.cutscene, false, null);
		VirtualCursors.CursorDesiredVisibilitySet(0, false);
		DrawersScript.TryPuttingEasterEgg();
	}

	// Token: 0x0600032B RID: 811 RVA: 0x00013ED0 File Offset: 0x000120D0
	public void FCall_SlotSpinTry(bool calledByAutoSpin)
	{
		if (SlotMachineScript.StateGet() == SlotMachineScript.State.idle && !SlotMachineScript.IsSpinning() && (!SlotMachineScript.instance.IsAutoSpinning() || calledByAutoSpin) && !DialogueScript.IsEnabled() && GameplayData.SpinsLeftGet() > 0)
		{
			if (calledByAutoSpin)
			{
				if (this.leverButton == null)
				{
					foreach (DiegeticMenuElement diegeticMenuElement in Object.FindObjectsOfType<DiegeticMenuElement>())
					{
						if (diegeticMenuElement.promptGuideType == PromptGuideScript.GuideType.slot_Spin)
						{
							this.leverButton = diegeticMenuElement;
							break;
						}
					}
				}
				this.leverButton.GetComponent<ButtonVisualizerScript>().Press();
				Sound.Play3D("SoundSlotLever", SlotMachineScript.instance.transform.position + new Vector3(0f, 1f, 0f), 10f, 1f, 1f, 1);
			}
			GameplayData.SpinConsume();
			GeneralUiScript.CoinsTextForceUpdate();
			SlotMachineScript.instance.Spin();
			return;
		}
		Sound.Play3D("SoundMenuError", SlotMachineScript.instance.transform.position + new Vector3(0f, 1f, 0f), 10f, 1f, 1f, 1);
		CameraGame.Shake(1f);
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0001400C File Offset: 0x0001220C
	public void FCall_DoorOpenTry()
	{
		if (this.doorGameObject == null)
		{
			this.doorGameObject = GameObject.Find("Porta");
		}
		if (DoorScript.OpenTry())
		{
			GameplayData.PrizeWasUsedSet();
			this._DeathCountdownReset(true);
			GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.endingWithoutDeath, false, null);
			Music.Stop("OstDemoTrailer", true);
			Music.Stop("OstReleaseTrailer", true);
			Music.Stop("OstCredits", true);
			return;
		}
		if (RewardBoxScript.GetRewardKind() != RewardBoxScript.RewardKind.DoorKey && !GameplayData.RewardBoxHasPrize())
		{
			DialogueScript.SetDialogue(true, new string[] { "DIALOGUE_DOOR_CLOSED_WRONG_KEY_0" });
		}
		else
		{
			DialogueScript.SetDialogue(true, new string[] { "DIALOGUE_DOOR_CLOSED_0" });
		}
		Sound.Play3D("SoundDoorLocked", this.doorGameObject.transform.position + new Vector3(0f, 4f, 0f), 10f, 1f, 1f, 1);
	}

	// Token: 0x0600032D RID: 813 RVA: 0x000140F8 File Offset: 0x000122F8
	public void FCall_DepositTry()
	{
		if (ATMScript.IsDepositButtonDelayed())
		{
			return;
		}
		BigInteger bigInteger = GameplayData.CoinsGet();
		GameplayData.DebtGet();
		BigInteger bigInteger2 = GameplayData.NextDepositAmmountGet(false);
		bool flag = DeckBoxScript.ItsMemoryCardTime();
		bool flag2 = GameplayData.RunModifier_DealIsAvailable_Get();
		if (flag && flag2)
		{
			MemoryPackDealUI.DealPropose();
			GameplayData.RunModifier_DealIsAvailable_Set(false);
			return;
		}
		if (!ATMScript.DebtClearCutsceneIsPlaying() && (!PromptGuideScript.PreventDepositDuringFlashing() || MemoryPackDealUI.IsDealRunnning()))
		{
			if (GameplayData.RoundsLeftToDeadline() > 0)
			{
				BigInteger bigInteger3 = GameplayData.SpinCostMax_Get();
				if (bigInteger - bigInteger2 < bigInteger3 && (DrawersScript.GetDrawersUnlockedNum() == 0 && !GameplayMaster._failsafeTriggered_OverDeposit))
				{
					GameplayMaster._failsafeTriggered_OverDeposit = true;
					DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_FAILSAFE_OVER_DEPOSITING" });
					return;
				}
			}
			if (bigInteger >= bigInteger2)
			{
				GameplayData.CoinsAdd(-bigInteger2, false);
				GameplayData.DepositAdd(bigInteger2);
				GeneralUiScript.CoinsTextInstantUpdate();
				Sound.Play3D("SoundCoinDeposit", ATMScript.instance.transform.position + new Vector3(0f, 2f, 0f), 10f, 1f, 1f, 1);
				ATMScript.instance.InsertCoinAnimation();
				PromptGuideScript.ResetGuide();
				PromptGuideScript.SetGuideType(PromptGuideScript.GuideType.atm_insertCoin);
				GameplayMaster._failsafeTriggered_NeverDeposited = true;
				BigInteger bigInteger4 = GameplayData.DepositGet();
				if (bigInteger4 > 1000000L)
				{
					PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.ItsNotAnAddiction);
				}
				if (bigInteger4 > 1000000000L)
				{
					PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.ItsDedication);
				}
				return;
			}
		}
		Sound.Play3D("SoundMenuError", ATMScript.instance.transform.position + new Vector3(0f, 2f, 0f), 10f, 1f, 1f, 1);
		CameraGame.Shake(1f);
	}

	// Token: 0x0600032E RID: 814 RVA: 0x000142A4 File Offset: 0x000124A4
	public void FCall_InterestsGetTry()
	{
		if (GameplayData.InterestEarnedGet() > 0L)
		{
			GameplayData.InterestPickUp();
			Sound.Play3D("SoundInterestRetrieved", ATMScript.instance.transform.position + new Vector3(0f, 2f, 0f), 10f, 1f, 1f, 1);
			PromptGuideScript.ResetGuide();
			PromptGuideScript.SetGuideType(PromptGuideScript.GuideType.atm_GetRevenue);
			return;
		}
		Sound.Play3D("SoundMenuError", ATMScript.instance.transform.position + new Vector3(0f, 2f, 0f), 10f, 1f, 1f, 1);
		CameraGame.Shake(1f);
	}

	// Token: 0x0600032F RID: 815 RVA: 0x00014360 File Offset: 0x00012560
	public void FCall_BuyTry(int id)
	{
		StoreCapsuleScript.BuyResult buyResult = StoreCapsuleScript.BuyTry(id);
		switch (buyResult)
		{
		case StoreCapsuleScript.BuyResult.Success:
		case StoreCapsuleScript.BuyResult.NotEnoughCurrency:
		case StoreCapsuleScript.BuyResult.Empty:
		case StoreCapsuleScript.BuyResult.InventoryFull:
			break;
		default:
			Debug.LogError("Buy result not implemented yet! result: " + buyResult.ToString());
			break;
		}
		PromptGuideScript.GuideType guideType = PromptGuideScript.GetGuideType();
		PromptGuideScript.ResetGuide();
		PromptGuideScript.SetGuideType(guideType);
	}

	// Token: 0x06000330 RID: 816 RVA: 0x000143B5 File Offset: 0x000125B5
	public void FCall_DrawerOpenTry(int id)
	{
		DrawersScript.OpenTry(id);
	}

	// Token: 0x06000331 RID: 817 RVA: 0x000143C0 File Offset: 0x000125C0
	public static void FCall_DebtNext(bool increaseRoundsAsWell, bool lastDemoDeadline)
	{
		int num = GameplayData.RoundsLeftToDeadline();
		GameplayData.DebtIndexGet();
		RunModifierScript.Identifier identifier = GameplayData.RunModifier_GetCurrent();
		GameplayData.DebtIndexAdd(1);
		BigInteger bigInteger = GameplayData.DebtIndexGet();
		bool flag = GameplayData.AreWeOverTheDebtRange(bigInteger);
		if (increaseRoundsAsWell)
		{
			GameplayData.RoundDeadlineTrail_Set(GameplayData.RoundOfDeadlineGet());
			GameplayData.DeadlineRoundsIncrement();
		}
		GameplayData.RoundDeadlineTrail_AtDeadlineBegin_CheckpointSet();
		if (flag)
		{
			int num2 = GameplayData.DebtOutOfRangeIndexAmmount(bigInteger).CastToInt();
			BigInteger bigInteger2 = num2;
			bigInteger2 = BigInteger.Pow(bigInteger2, Mathf.Max(0, bigInteger2.CastToInt() - 3));
			if (bigInteger2 < 1L)
			{
				bigInteger2 = 1;
			}
			BigInteger bigInteger3 = 6;
			BigInteger bigInteger4 = 1;
			for (int i = 0; i < num2 - 1; i++)
			{
				bigInteger3 *= 2;
			}
			for (int j = 0; j < num2; j++)
			{
				bigInteger4 *= bigInteger3;
			}
			bigInteger4 *= bigInteger2;
			GameplayData.DebtOutOfRangeMultiplier_Set(bigInteger4);
		}
		PowerupScript.SkeletonEreasePiecesLeftIntoDrawers();
		if (identifier == RunModifierScript.Identifier.drawerTableModifications)
		{
			bool flag2 = false;
			PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
			if (randomCharmToModify_OnTableOrSlot != null)
			{
				flag2 = true;
				GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.devious, true);
			}
			PowerupScript randomCharmToModify_InDrawers = AbilityScript.GetRandomCharmToModify_InDrawers();
			if (randomCharmToModify_InDrawers != null)
			{
				flag2 = true;
				randomCharmToModify_InDrawers.ModifierReEvaluate(false, true);
			}
			if (flag2)
			{
				RunModifierScript.TriggerAnimation_IfEquipped(RunModifierScript.Identifier.drawerTableModifications);
			}
		}
		if (identifier == RunModifierScript.Identifier.drawerModGamble)
		{
			bool flipCoin = R.Rng_RunMod.FlipCoin;
			RunModifierScript.TriggerAnimation_IfEquipped(RunModifierScript.Identifier.drawerModGamble);
			for (int k = 0; k < PowerupScript.array_InDrawer.Length; k++)
			{
				PowerupScript powerupScript = PowerupScript.array_InDrawer[k];
				if (!(powerupScript == null))
				{
					if (flipCoin)
					{
						PowerupScript.ThrowAway(powerupScript.identifier, false);
					}
					else
					{
						powerupScript.ModifierReEvaluate(false, true);
					}
				}
			}
		}
		GameplayData.StoreRestockExtraCostSet(0);
		try
		{
			StoreCapsuleScript.Restock(true, false, null, false, false);
			if (identifier == RunModifierScript.Identifier.phoneEnhancer)
			{
				for (int l = 0; l < StoreCapsuleScript.storePowerups.Length; l++)
				{
					StoreCapsuleScript.storePowerups[l] = null;
				}
				StoreCapsuleScript.RefreshCostTextAll();
				RunModifierScript.TriggerAnimation_IfEquipped(RunModifierScript.Identifier.phoneEnhancer);
			}
		}
		catch (Exception ex)
		{
			string text = string.Concat(new string[] { "Error while restocking the store after debt increase. SOURCE: ", ex.Source, "\nMESSAGE: ", ex.Message, "\nSTACK TRACE: ", ex.StackTrace });
			Debug.LogError(text);
			ConsolePrompt.LogError(text, "", 0f);
		}
		int num3 = PowerupScript.ModifiedPowerups_GetCount(PowerupScript.Modifier.gambler);
		if (num3 > 0)
		{
			GameplayData.StoreFreeRestocksSet(GameplayData.StoreFreeRestocksGet() + (long)num3);
		}
		if (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_FreeRestocks, true))
		{
			GameplayData.StoreFreeRestocksSet(GameplayData.StoreFreeRestocksGet() + 2L);
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Jimbo);
		}
		if (PowerupScript.EvilDealBonusMultiplier() > 1)
		{
			GameplayData.StoreFreeRestocksSet(GameplayData.StoreFreeRestocksGet() + 2L);
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.EvilDeal);
		}
		if (identifier == RunModifierScript.Identifier.smallerStore)
		{
			RunModifierScript.MFunc_SmallerStoreRestocksBonus();
			RunModifierScript.TriggerAnimation_IfEquipped(RunModifierScript.Identifier.smallerStore);
		}
		if (identifier == RunModifierScript.Identifier.charmsRecycling)
		{
			int count = PowerupScript.list_EquippedNormal.Count;
			if (count > 5)
			{
				PowerupScript powerupScript2 = null;
				int num4 = 100;
				while (powerupScript2 == null)
				{
					int num5 = R.Rng_RunMod.Range(0, count);
					powerupScript2 = PowerupScript.list_EquippedNormal[num5];
					num4--;
					if (num4 <= 0)
					{
						break;
					}
				}
				if (powerupScript2 != null)
				{
					RunModifierScript.TriggerAnimation_IfEquipped(RunModifierScript.Identifier.charmsRecycling);
					PowerupScript.ThrowAway(powerupScript2.identifier, false);
					GameplayData.StoreFreeRestocksSet(GameplayData.StoreFreeRestocksGet() + 2L);
				}
			}
		}
		if (!lastDemoDeadline)
		{
			PhoneScript.ResetForDeadline();
		}
		RedButtonScript.ResetTiming(RedButtonScript.RegistrationCapsule.Timing.perDeadline);
		if (identifier != RunModifierScript.Identifier.redButtonOverload)
		{
			RedButtonScript.RestoreCharges(num);
		}
		GameplayData.Stats_DeadlinesCompleted_Add();
		PowerupScript.RefreshPlacementAll();
	}

	// Token: 0x06000332 RID: 818 RVA: 0x0001471C File Offset: 0x0001291C
	public bool IsDeathCondition(bool considerCoins, bool considerRounds)
	{
		BigInteger bigInteger = GameplayData.CoinsGet() + GameplayData.InterestEarnedGet();
		BigInteger bigInteger2 = GameplayData.DebtGet() - GameplayData.DepositGet();
		int num = GameplayData.RoundsLeftToDeadline();
		bool flag = true;
		if (considerCoins && bigInteger >= bigInteger2)
		{
			flag = false;
		}
		if (considerRounds && num > 0)
		{
			flag = false;
		}
		return flag;
	}

	// Token: 0x06000333 RID: 819 RVA: 0x0001476C File Offset: 0x0001296C
	public bool DieTry(GameplayMaster.DeathStep initialDeathStep, bool callLastChanceCallback)
	{
		ScreenMenuScript.ForceClose_Death();
		DialogueScript.Close();
		PowerupScript.ForceCloseInspection_Death();
		PhoneUiScript.ForceClose_Death();
		ToyPhoneUIScript.ForceClose_Death();
		MainMenuScript.ForceClose_Death();
		MagazineUiScript.ForceClose_Death();
		WCScript.ForceClose_Death();
		DeckBoxUI.ForceClose_Death();
		if (callLastChanceCallback && this.onDeathLastChance != null)
		{
			GameplayMaster.Event @event = this.onDeathLastChance;
			if (@event != null)
			{
				@event();
			}
			if (!this.IsDeathCondition(true, true))
			{
				return false;
			}
		}
		if (GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.death)
		{
			return true;
		}
		GameplayMaster.deathsNumSinceStartup++;
		this.deathStep = initialDeathStep;
		GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.death, false, null);
		VirtualCursors.CursorDesiredVisibilitySet(0, false);
		ATMScript.instance.skullImageGameObject.SetActive(true);
		Music.Stop("OstDemoTrailer", true);
		Music.Stop("OstReleaseTrailer", true);
		Music.Stop("OstCredits", true);
		PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.AwDangit);
		return true;
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00014836 File Offset: 0x00012A36
	public static bool HasDiedOnce()
	{
		return GameplayMaster.deathsNumSinceStartup > 0;
	}

	// Token: 0x06000335 RID: 821 RVA: 0x00014840 File Offset: 0x00012A40
	public static int DeathsSinceStartup_GetNum()
	{
		return GameplayMaster.deathsNumSinceStartup;
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00014848 File Offset: 0x00012A48
	public void FCall_MenuDrawer_MainMenu_OpenTry()
	{
		MenuDrawerScript.Open(MenuDrawerScript.Kind.mainMenu);
		bool flag = MenuDrawerScript.IsOpened(MenuDrawerScript.Kind.mainMenu);
		GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
		if (flag && gamePhase == GameplayMaster.GamePhase.preparation)
		{
			CameraController.SetPosition(CameraController.PositionKind.MenuDrawer_Menu, false, 1f);
		}
	}

	// Token: 0x06000337 RID: 823 RVA: 0x0001487A File Offset: 0x00012A7A
	public void FCall_MenuDrawer_MainMenu_CloseTry()
	{
		MainMenuScript.Close();
		MenuDrawerScript.CloseAll();
		if (GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.preparation)
		{
			CameraController.SetPosition(CameraController.PositionKind.Free, false, 0f);
		}
	}

	// Token: 0x06000338 RID: 824 RVA: 0x0001489C File Offset: 0x00012A9C
	public void FCall_RewardBoxPickTry()
	{
		if (!RewardBoxScript.IsOpened())
		{
			if (GameplayMaster.DeathCountdownHasStarted())
			{
				CameraGame.Shake(1f);
				Sound.Play("SoundMenuError", 1f, 1f);
				return;
			}
			if (GameplayData.RewardTimeToShowAmmount())
			{
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_REWARD_BOX_EXPLANATION_REWARD_DECIDED" });
				return;
			}
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_REWARD_BOX_EXPLANATION" });
			return;
		}
		else
		{
			if (RewardBoxScript.HasPrize())
			{
				RewardBoxScript.Pick();
				return;
			}
			if (GameplayMaster.DeathCountdownHasStarted())
			{
				CameraGame.Shake(1f);
				Sound.Play("SoundMenuError", 1f, 1f);
				return;
			}
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_REWARD_BOX_EMPTY" });
			return;
		}
	}

	// Token: 0x06000339 RID: 825 RVA: 0x00014950 File Offset: 0x00012B50
	public void FCall_Terminal_Login()
	{
		if (GameplayMaster.DeathCountdownHasStarted())
		{
			CameraGame.Shake(1f);
			Sound.Play("SoundMenuError", 1f, 1f);
			return;
		}
		TerminalScript.SetState(TerminalScript.State.turnedOn_Request);
		Sound.Play3D("SoundTerminalLogin", TerminalScript.instance.transform.position, 20f, 1f, 1f, 1);
		Controls.VibrationSet_PreferMax(this.player, 0.5f);
		MemoScript.Close(false);
		GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.terminal, false, null);
		CameraController.SetPosition(CameraController.PositionKind.terminal, false, 1f);
		TerminalScript.instance.myMenuController.OpenMe();
	}

	// Token: 0x0600033A RID: 826 RVA: 0x000149F0 File Offset: 0x00012BF0
	public void FCall_Terminal_Logout()
	{
		TerminalScript.SetState(TerminalScript.State.turnedOff_Request);
		Sound.Play3D("SoundTerminalTurnOff", TerminalScript.instance.transform.position, 20f, 1f, 1f, 1);
		Controls.VibrationSet_PreferMax(this.player, 0.5f);
		GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.preparation, false, null);
		CameraController.SetPosition(CameraController.PositionKind.Free, false, 1f);
		TerminalScript.instance.myMenuController.Back();
	}

	// Token: 0x0600033B RID: 827 RVA: 0x00014A60 File Offset: 0x00012C60
	public void FCall_Phone_Pickup()
	{
		PhoneScript.StateSet(PhoneScript.State.onIntro);
		CameraController.SetPosition(CameraController.PositionKind.PhoneDoor, false, 2f);
		PromptGuideScript.ForceClose(true);
	}

	// Token: 0x0600033C RID: 828 RVA: 0x00014A7B File Offset: 0x00012C7B
	public void FCall_Phone_Hangup()
	{
		PhoneScript.StateSet(PhoneScript.State.offNothing);
		CameraController.SetPosition(CameraController.PositionKind.Free, false, 0f);
	}

	// Token: 0x0600033D RID: 829 RVA: 0x00014A8F File Offset: 0x00012C8F
	public void FCall_ToyPhone_Pickup()
	{
		ToyPhoneUIScript.instance.PickUp();
	}

	// Token: 0x0600033E RID: 830 RVA: 0x00014A9B File Offset: 0x00012C9B
	public void FCAll_ToyPhone_Hangup()
	{
		ToyPhoneUIScript.instance.HangUp();
	}

	// Token: 0x0600033F RID: 831 RVA: 0x00014AA7 File Offset: 0x00012CA7
	public void FCall_MagazineRead()
	{
		MagazineUiScript.Open();
	}

	// Token: 0x06000340 RID: 832 RVA: 0x00014AAE File Offset: 0x00012CAE
	public void FCall_MagazineClose()
	{
		MagazineUiScript.Close(false);
	}

	// Token: 0x06000341 RID: 833 RVA: 0x00014AB8 File Offset: 0x00012CB8
	public void FCall_WcMenuOpen()
	{
		ScreenMenuScript.Positioning positioning = ScreenMenuScript.Positioning.center;
		ScreenMenuScript.OptionEvent[] array = new ScreenMenuScript.OptionEvent[3];
		ScreenMenuScript.OptionEvent[] array2 = array;
		int num = 0;
		array2[num] = (ScreenMenuScript.OptionEvent)Delegate.Combine(array2[num], new ScreenMenuScript.OptionEvent(this._WcScreenMenu_Piss));
		ScreenMenuScript.OptionEvent[] array3 = array;
		int num2 = 1;
		array3[num2] = (ScreenMenuScript.OptionEvent)Delegate.Combine(array3[num2], new ScreenMenuScript.OptionEvent(this._WcScreenMenu_Poop));
		ScreenMenuScript.OptionEvent[] array4 = array;
		int num3 = 2;
		array4[num3] = (ScreenMenuScript.OptionEvent)Delegate.Combine(array4[num3], new ScreenMenuScript.OptionEvent(this._WcScreenMenu_Cancel));
		ScreenMenuScript.Open(true, true, 2, positioning, 0f, Translation.Get("SCREEN_MENU_WC_TITLE"), new string[]
		{
			Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_WC_PISS"), Strings.SanitizationSubKind.none),
			Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_WC_POOP"), Strings.SanitizationSubKind.none),
			Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("SCREEN_MENU_WC_NOT_NOW"), Strings.SanitizationSubKind.none)
		}, array);
		CameraController.DisableReason_Add("WC");
		VirtualCursors.CursorDesiredVisibilitySet(0, true);
	}

	// Token: 0x06000342 RID: 834 RVA: 0x00014B9A File Offset: 0x00012D9A
	private void _WcScreenMenu_Piss()
	{
		VirtualCursors.CursorDesiredVisibilitySet(0, false);
		CameraController.DisableReason_Remove("WC");
		WCScript.StartAction(WCScript.ActionType.piss);
	}

	// Token: 0x06000343 RID: 835 RVA: 0x00014BB3 File Offset: 0x00012DB3
	private void _WcScreenMenu_Poop()
	{
		VirtualCursors.CursorDesiredVisibilitySet(0, false);
		CameraController.DisableReason_Remove("WC");
		WCScript.StartAction(WCScript.ActionType.poop);
	}

	// Token: 0x06000344 RID: 836 RVA: 0x00014BCC File Offset: 0x00012DCC
	private void _WcScreenMenu_Cancel()
	{
		VirtualCursors.CursorDesiredVisibilitySet(0, false);
		CameraController.DisableReason_Remove("WC");
	}

	// Token: 0x06000345 RID: 837 RVA: 0x00014BDF File Offset: 0x00012DDF
	public void FCall_DeckBoxOpen()
	{
		DeckBoxUI.Open(GameplayData.RunModifier_AlreadyPicked() ? DeckBoxUI.UiKind.seeCollection : DeckBoxUI.UiKind.pickCardForTheRun);
	}

	// Token: 0x06000346 RID: 838 RVA: 0x00014BF1 File Offset: 0x00012DF1
	public void FCall_DeckBoxClose()
	{
		DeckBoxUI.Close();
	}

	// Token: 0x06000347 RID: 839 RVA: 0x00014BF8 File Offset: 0x00012DF8
	private void Awake()
	{
		if (GameplayMaster.instance != null)
		{
			Debug.LogError("GameplayMaster instance already exists");
			Object.Destroy(this);
			return;
		}
		GameplayMaster.instance = this;
		this.unlockChecksCooldownTimer = 0.5f;
	}

	// Token: 0x06000348 RID: 840 RVA: 0x00014C2C File Offset: 0x00012E2C
	private void Start()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		this.player = Controls.GetPlayerByIndex(0);
		bool flag = Data.game.GameplayDataIsEmpty();
		if (flag)
		{
			Debug.Log("<color=green>NEW game started!</color>");
		}
		else
		{
			Debug.Log("<color=yellow>OLD game loaded!</color>");
		}
		if (flag)
		{
			GameplayData.NewGameIntroFinished_Set(false);
		}
		if (flag)
		{
			Data.game.runsDone++;
		}
		if (flag)
		{
			if (GameplayMaster.specificSeedRequest_ForNewGame == null)
			{
				GameplayData.SeedInitRandom();
			}
			else
			{
				this._isCustomSeedRun = true;
				GameplayData.SeedInitSpecific(GameplayMaster.specificSeedRequest_ForNewGame.Value);
			}
			GameplayMaster.specificSeedRequest_ForNewGame = null;
		}
		GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.intro, false, null);
		GeneralUiScript.ForceEnabled();
		foreach (DiegeticMenuController diegeticMenuController in DiegeticMenuController.all)
		{
			diegeticMenuController.OnCanRun = (DiegeticMenuController.CanRun)Delegate.Combine(diegeticMenuController.OnCanRun, new DiegeticMenuController.CanRun(delegate
			{
				if (GameplayMaster.EndingFreeRoaming)
				{
					return true;
				}
				GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
				return (gamePhase == GameplayMaster.GamePhase.preparation || gamePhase == GameplayMaster.GamePhase.equipping || gamePhase == GameplayMaster.GamePhase.gambling) && !DialogueScript.IsEnabled() && !ScreenMenuScript.IsEnabled() && !PowerupTriggerAnimController.HasAnimations() && !DrawersScript.IsKeyAnimationPlaying() && !PhoneScript.IsOn() && !ToyPhoneUIScript.IsEnabled() && !MagazineUiScript.IsEnabled() && !WCScript.IsPerformingAction() && !MemoryPackDealUI.IsDealRunnning() && !DeckBoxUI.IsEnabled();
			}));
		}
		PosterScript.Initialize();
		DrawersScript.Initialize();
		PowerupScript.InitializeAll(false, flag);
		StoreCapsuleScript.InitializeAll(flag, true);
		AbilityScript.InitializeAll(flag);
		RewardBoxScript.Initialize(flag);
		TerminalScript.Initialize();
		DeadlineBonusScreen.Initialize();
		PhoneUiButton.InitializeAll();
		PhoneAbilityUiScript.InitializeAll();
		FloppySlotScript.Initialize(flag);
		ATMScript.Initialize();
		RunModifierScript.InitializeAll();
		GeneralUiScript.CoinsTextForceUpdate();
		GeneralUiScript.TicketsTextForceUpdate();
		if (flag)
		{
			Data.SaveGame(Data.GameSavingReason.newGame, -1);
		}
		if (Data.game.PowerupRealInstances_AreAllUnlocked())
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.CompletedDatabase);
			PlatformAPI.AchievementUnlock_Demo(PlatformAPI.AchievementDemo.ADecentCollection);
		}
		if (Data.game.doorOpenedCounter > 0)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.TheStructure);
		}
		if (Data.game.goodEndingCounter > 0)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Ascension);
		}
		int drawersUnlockedNum = DrawersScript.GetDrawersUnlockedNum();
		if (drawersUnlockedNum >= 1)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Drawer0);
		}
		if (drawersUnlockedNum >= 2)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Drawer1);
		}
		if (drawersUnlockedNum >= 3)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Drawer2);
		}
		if (drawersUnlockedNum >= 4)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Drawer3);
		}
		Data.GameData.AllCardsUnlockedCompute();
		Data.GameData.AllCardsHolographicCompute();
		if (Data.game.AllCardsUnlocked)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.FullCollection);
		}
		if (Data.game.AllCardsHolographic)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.TheLordOfThePit);
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.MyPrecious);
		}
		if (Data.game.RunModifier_WonTimes_Get(RunModifierScript.Identifier.bigDebt) > 0)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Omnipotent);
		}
	}

	// Token: 0x06000349 RID: 841 RVA: 0x00014E68 File Offset: 0x00013068
	private void OnDestroy()
	{
		if (GameplayMaster.instance == this)
		{
			GameplayMaster.instance = null;
		}
	}

	// Token: 0x0600034A RID: 842 RVA: 0x00014E80 File Offset: 0x00013080
	private void Update()
	{
		if (!Music.IsPlaying("SoundtrackLoopAmbience1") && this.gamePhase != GameplayMaster.GamePhase.endingWithoutDeath)
		{
			int num = 0;
			if (Music.IsPlaying("OstDemoTrailer"))
			{
				num = 1;
			}
			else if (Music.IsPlaying("OstReleaseTrailer"))
			{
				num = 2;
			}
			else if (Music.IsPlaying("OstCredits"))
			{
				num = 3;
			}
			if (num == 0)
			{
				Music.Play("SoundtrackLoopAmbience1");
				Music.SetVolumeFadeInstant(0f);
				Music.SetVolumeFade(1f, 0.25f);
			}
			else
			{
				PowerupScript.Identifier identifier = PowerupScript.Identifier.undefined;
				switch (num)
				{
				case 1:
					identifier = PowerupScript.Identifier.DiscA;
					break;
				case 2:
					identifier = PowerupScript.Identifier.DiscB;
					break;
				case 3:
					identifier = PowerupScript.Identifier.DiscC;
					break;
				default:
					Debug.LogWarning("GameplayMaster.Update(): musicPlayingIndex not handled: " + num.ToString());
					break;
				}
				if (!PowerupScript.IsInDrawer_Quick(identifier))
				{
					switch (identifier)
					{
					case PowerupScript.Identifier.DiscA:
						Music.Stop("OstDemoTrailer", true);
						break;
					case PowerupScript.Identifier.DiscB:
						Music.Stop("OstReleaseTrailer", true);
						break;
					case PowerupScript.Identifier.DiscC:
						Music.Stop("OstCredits", true);
						break;
					}
				}
			}
		}
		if (!Tick.IsGameRunning)
		{
			return;
		}
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		if (this.gamePhase == GameplayMaster.GamePhase.Undefined)
		{
			return;
		}
		this.TimePlayedCount();
		GameplayData.SymbolsValueList_Order();
		GameplayData.SymbolsChanceList_Order();
		GameplayData.PatternsValueList_Order();
		PowerupScript.UpdateAllVisibles();
		switch (this.gamePhase)
		{
		case GameplayMaster.GamePhase.intro:
			this.IntroPhaseBehaviour();
			break;
		case GameplayMaster.GamePhase.cutscene:
			this.CutscenePhaseBehaviour();
			break;
		case GameplayMaster.GamePhase.tutorialObsolete:
			this.TutorialPhaseBehaviour();
			break;
		case GameplayMaster.GamePhase.preparation:
			this.PreparationPhaseBehaviour();
			break;
		case GameplayMaster.GamePhase.equipping:
			this.EquippingPhaseBehaviour();
			break;
		case GameplayMaster.GamePhase.gambling:
			this.GamblingPhaseBehaviour();
			break;
		case GameplayMaster.GamePhase.death:
			this.DeathPhaseBehaviour();
			break;
		case GameplayMaster.GamePhase.endingWithoutDeath:
			this.EndingWithoutDeathPhaseBehaviour();
			break;
		case GameplayMaster.GamePhase.closingGame:
			this.ClosingGamePhaseBehaviour();
			break;
		case GameplayMaster.GamePhase.terminal:
			this.TerminalPhaseBehaviour();
			break;
		case GameplayMaster.GamePhase.phone:
			this.PhonePhaseBehaviour();
			break;
		default:
			Debug.LogError("GamePhase not handled: " + this.gamePhase.ToString());
			break;
		}
		this.ControllerDisconnectionCheckUpdate();
		bool flag = GameplayMaster.DeathCountdownHasStarted() || this.gamePhase == GameplayMaster.GamePhase.death;
		if (this.deathRoomParticles.activeSelf != flag)
		{
			this.deathRoomParticles.SetActive(flag);
		}
		this.unlockChecksCooldownTimer -= Tick.Time;
		if (this.unlockChecksCooldownTimer <= 0f)
		{
			this.unlockChecksCooldownTimer += 1f;
			float num2 = GameplayData.SixSixSix_ChanceGet_AsPercentage(true);
			float num3 = GameplayData.InterestRateGet();
			float num4 = GameplayData.Symbol_Chance_GetAsPercentage(GameplayData.MostProbableSymbols_GetList()[0], true, true);
			long num5 = GameplayData.CloverTicketsGet();
			if (!this.unlockCheckPerformed_PossessedPhone && num2 >= 7f)
			{
				this.unlockCheckPerformed_PossessedPhone = true;
				PowerupScript.Unlock(PowerupScript.Identifier.PossessedPhone);
			}
			if (!this.unlockCheckPerformed_EvilDeal && num2 >= 7f)
			{
				this.unlockCheckPerformed_EvilDeal = true;
				PowerupScript.Unlock(PowerupScript.Identifier.EvilDeal);
			}
			if (!this.unlockCheckPerformed_Wallet && GameplayData.CloverTicketsGet() >= 30L)
			{
				this.unlockCheckPerformed_Wallet = true;
				PowerupScript.Unlock(PowerupScript.Identifier.Wallet);
			}
			Data.GameData.AllCardsUnlockedCompute();
			Data.GameData.AllCardsHolographicCompute();
			if (num2 >= 10f)
			{
				PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Cultist);
			}
			if (num3 >= 30f)
			{
				PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Rentier);
			}
			if (num4 >= 60f)
			{
				PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Manifesting);
			}
			if (Data.game.AllCardsUnlocked)
			{
				PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.FullCollection);
			}
			if (Data.game.AllCardsHolographic)
			{
				PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.TheLordOfThePit);
			}
			if (num5 >= 100L)
			{
				PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Scrooge);
			}
		}
	}

	public static GameplayMaster instance;

	private const int PLAYER_INDEX = 0;

	public const string EXTRA_INFOS_FROM_INTRO_DIALOGUE = "From Intro Dialogue";

	private const float DEATH_DIALOGUES_SELF_CLOSE_TIME = 5f;

	private const float DEATH_DIALOGUES_SELF_CLOSE_TIME_SHORT = 3f;

	private Controls.PlayerExt player;

	public GameObject deathRoomParticles;

	private int oldJoystickCount = -1;

	private int joysticDisconnectionCount = -1;

	private double timePlayed_SecondsTimer;

	private const int DEATH_COUNTDOWN_RESET = 5;

	private int deathCountDown = 5;

	private float deathCountDownTimer;

	private bool deathCountDownStarted;

	private bool deathCountDownResetRequest;

	private bool deathCDResetRequest_RoundsFlag;

	private GameplayMaster.GamePhase gamePhase = GameplayMaster.GamePhase.Undefined;

	private GameplayMaster.IntroCutscenePhase introCutscenePhase;

	private GameplayMaster.InterestsAndTicketsPhase interestsAndTicketsPhase = GameplayMaster.InterestsAndTicketsPhase.done;

	private bool interestsAwarded;

	private bool intAndTickets_ShakedTrapdoor;

	private float interestsAndTicketsTimer;

	private const float interestsAndTicketsTimerDelay = 0.5f;

	private bool _1RoundLeftWarned;

	private const float delayReset = 0.5f;

	private float delay = 0.5f;

	public static bool unlockPowerupFirstTimeDialogueBooked;

	private GameplayMaster.DeathStep deathStep;

	private float deathStepTimer;

	private bool _deathFadeOutTensionSound;

	private bool deathSaved;

	private GameplayMaster.EndingWithoutDeath_Phase endingWithoutDeath_Phase;

	private float glowLerpTime;

	private Color endingGlowColor = new Color(1f, 1f, 1f, 1f);

	private Coroutine endingCoroutine;

	private static bool _cranePackDialogueShown;

	public static int? specificSeedRequest_ForNewGame;

	private bool _isCustomSeedRun;

	private BigInteger _slotAnimationCoins_AnimationOnly = 0;

	public static bool drawerFromDemoUnlocked;

	private bool _endingFreeRoaming;

	private bool _endingGotoCreditsFlag;

	private bool _theDealIsOff_BookedDialogue;

	private bool _theDealIsOff_ByCoins;

	private static int spinsDoneSinceStartup;

	private static int deathsNumSinceStartup;

	public static bool restartQuickDeath;

	private static int provocativeSentencesCounter_PreGoodEnding;

	private static int provocativeSentencesCounter_AfterGoodEnding;

	private bool unlockCheckPerformed_PossessedPhone;

	private bool unlockCheckPerformed_EvilDeal;

	private bool unlockCheckPerformed_Wallet;

	private bool _twitchAffiliationMessageBooked;

	private static bool _failsafeTriggered_OverDeposit;

	private static bool _failsafeTriggered_NeverDeposited;

	private static bool _failsafeTriggered_NeverEquippedACharm;

	private bool _dataResetFlag;

	public GameplayMaster.Event onDeathLastChance;

	public GameplayMaster.Event onDeadlineBonus;

	public GameplayMaster.Event onDeadlineBonus_Late;

	private bool dialogueJustInterruptedTurnOn;

	private DiegeticMenuElement leverButton;

	private GameObject doorGameObject;

	private float unlockChecksCooldownTimer;

	public enum GamePhase
	{
		intro,
		cutscene,
		tutorialObsolete,
		preparation,
		equipping,
		gambling,
		death,
		endingWithoutDeath,
		closingGame,
		terminal,
		phone,
		Count,
		Undefined
	}

	private enum IntroCutscenePhase
	{
		introDialogueOrDeckBox,
		tutorial,
		finalization,
		done
	}

	private enum InterestsAndTicketsPhase
	{
		beforeInterestsAndClovers,
		interestsAtAtm,
		cloverTickets,
		cloverTicketsWait,
		shakeTrapdoor_Optional,
		done
	}

	public enum DeathStep
	{
		lookAtAtm,
		lookAtTrapdoor,
		startFalling,
		falling,
		done
	}

	private enum EndingWithoutDeath_Phase
	{
		begin,
		waitingCoroutine,
		done
	}

	// (Invoke) Token: 0x060010D2 RID: 4306
	public delegate void Event();
}
