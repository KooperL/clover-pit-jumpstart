using System;
using System.Collections;
using System.Numerics;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200004A RID: 74
public class ATMScript : MonoBehaviour
{
	// Token: 0x17000030 RID: 48
	// (get) Token: 0x06000476 RID: 1142 RVA: 0x0000925B File Offset: 0x0000745B
	public global::UnityEngine.Vector3 Sound3dOffset
	{
		get
		{
			return base.transform.position + this.sound3dOffset;
		}
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0002EEDC File Offset: 0x0002D0DC
	public void UpdateStrings(bool forceUpdate)
	{
		GameplayMaster.GetGamePhase();
		BigInteger bigInteger = GameplayData.RoundsLeftToDeadline();
		if (bigInteger != this.roundsLeftOld || forceUpdate)
		{
			string text;
			if (bigInteger == 1L)
			{
				text = Translation.Get("ATM_ROUND_LEFT");
			}
			else
			{
				text = Translation.Get("ATM_ROUNDS_LEFT");
			}
			text = Strings.Sanitize(Strings.SantizationKind.ui, text, Strings.SanitizationSubKind.none);
			this.textRoundsLeft.text = "<sprite name=\"SkullSymbolOrange32\"> " + text + " <sprite name=\"SkullSymbolOrange32\">";
			this.roundsLeftOld = bigInteger;
		}
		if (forceUpdate)
		{
			this.textLabelsReachDepositInterest.text = string.Concat(new string[]
			{
				Translation.Get("ATM_DEBT"),
				"\n\n",
				Translation.Get("ATM_DEPOSIT_LABEL"),
				"\n\n",
				Translation.Get("ATM_INTEREST_LABEL")
			});
		}
		BigInteger bigInteger2 = GameplayData.DebtGet();
		if (bigInteger2 != this.debtOld || forceUpdate)
		{
			this.textReachCoins.text = bigInteger2.ToStringSmart() + " <sprite name=\"CoinSymbolOrange32\">";
			this.debtOld = bigInteger2;
		}
		BigInteger bigInteger3 = GameplayData.DepositGet();
		if (bigInteger3 != this.depositedOld || forceUpdate)
		{
			this.textDeposited.text = bigInteger3.ToStringSmart() + " <sprite name=\"CoinSymbolOrange32\">";
			this.depositedOld = bigInteger3;
			this.DebtTextFlash();
		}
		float num = GameplayData.InterestRateGet();
		BigInteger bigInteger4 = GameplayData.InterestEarnedHypotetically();
		if (num != this.interestOld || this.hypoteticalInterestOld != bigInteger4 || forceUpdate)
		{
			this.textInterest.text = num.ToString("0.0") + "% (" + bigInteger4.ToStringSmart() + "<sprite name=\"CoinSymbolOrange32\">)";
			this.interestOld = num;
			this.hypoteticalInterestOld = bigInteger4;
			if (GameplayData.InterestRateGet() >= 30f)
			{
				PowerupScript.Unlock(PowerupScript.Identifier.Wolf);
			}
		}
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x00009273 File Offset: 0x00007473
	private void OnLanguageChange()
	{
		this.UpdateStrings(true);
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0000927C File Offset: 0x0000747C
	public void InsertCoinAnimation()
	{
		this.insertCoinHolderTr.SetLocalZ(-0.75f);
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0000928F File Offset: 0x0000748F
	public static bool DebtClearCutsceneIsPlaying()
	{
		return ATMScript.instance != null && ATMScript.instance.debtClearCutscenePlaying;
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x000092AA File Offset: 0x000074AA
	private void DebtClearCutsceneStart()
	{
		if (this.debtClearCutscenePlaying)
		{
			return;
		}
		this.debtClearCutscenePlaying = true;
		base.StartCoroutine(this.DebtClearCoroutine());
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x000092C9 File Offset: 0x000074C9
	private IEnumerator DebtClearCoroutine()
	{
		float transitionSpeed = (float)Data.settings.transitionSpeed;
		RunModifierScript.Identifier runModifier = GameplayData.RunModifier_GetCurrent();
		bool isRewardBoxTime = GameplayData.GetRewardBoxDebtIndex() <= GameplayData.DebtIndexGet() && !RewardBoxScript.IsOpened();
		bool lastDemoDeadline = isRewardBoxTime && Master.IsDemo;
		PromptGuideScript.ForceClose(false);
		int roundsLeft = GameplayData.RoundsLeftToDeadline();
		GameplayMaster.GamePhase oldGamePhase = GameplayMaster.GetGamePhase();
		CameraController.PositionKind oldCameraPosition = CameraController.GetPositionKind();
		GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.cutscene, false, null);
		global::UnityEngine.Vector3 backupCameraRotation = CameraController.GetFreeCameraRotation();
		CameraController.SetPosition(CameraController.PositionKind.ATM, false, 1f * transitionSpeed);
		float passedTime = Tick.PassedTime;
		float timer = 0.75f;
		bool garbageCollected = false;
		while (timer > 0f)
		{
			timer -= Tick.Time * transitionSpeed;
			yield return null;
		}
		if (!garbageCollected && CameraController.GetPositionDifferenceMagnitude() < 0.2f)
		{
			garbageCollected = true;
			GC.Collect();
		}
		this.crownImageGameObject.SetActive(true);
		Sound.Play3D("SoundATMFanfare", this.Sound3dOffset, 20f, 1f, 1f, AudioRolloffMode.Linear);
		BigInteger debtIndexOld = GameplayData.DebtIndexGet();
		int num = debtIndexOld.CastToInt();
		GameplayMaster.FCall_DebtNext(true, lastDemoDeadline);
		if (debtIndexOld < 2147483647L)
		{
			switch (num)
			{
			case 0:
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_DEBT_INCREASE_FIRST_TIME_0", "DIALOGUE_DEBT_NEW_AMMOUNT" });
				break;
			case 1:
				Strings.SetTemporaryFlag_Sanitize666And999(2);
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_DEBT_INCREASE_SECOND_TIME_0", "DIALOGUE_DEBT_NEW_AMMOUNT" });
				DialogueScript.SetDialogueInputDelay(0.5f);
				break;
			case 2:
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_DEBT_INCREASE_THIRD_TIME_0", "DIALOGUE_DEBT_NEW_AMMOUNT" });
				break;
			default:
				if (!isRewardBoxTime)
				{
					DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_DEBT_NEW_AMMOUNT" });
				}
				break;
			}
		}
		while (DialogueScript.IsEnabled())
		{
			yield return null;
		}
		if (isRewardBoxTime)
		{
			timer = 0.5f;
			while (timer > 0f)
			{
				timer -= Tick.Time;
				yield return null;
			}
			CameraController.SetPosition(CameraController.PositionKind.RewardBox, false, 1f * transitionSpeed);
			RewardBoxScript.Open();
			switch (RewardBoxScript.GetRewardKind())
			{
			case RewardBoxScript.RewardKind.DemoPrize:
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_REWARD_BOX_JUST_OPENED_DEMO" });
				break;
			case RewardBoxScript.RewardKind.DrawerKey0:
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_REWARD_BOX_JUST_OPENED" });
				break;
			case RewardBoxScript.RewardKind.DrawerKey1:
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_REWARD_BOX_JUST_OPENED" });
				break;
			case RewardBoxScript.RewardKind.DrawerKey2:
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_REWARD_BOX_JUST_OPENED" });
				break;
			case RewardBoxScript.RewardKind.DrawerKey3:
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_REWARD_BOX_JUST_OPENED" });
				break;
			case RewardBoxScript.RewardKind.DoorKey:
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_REWARD_BOX_JUST_OPENED_FINAL_KEY" });
				DialogueScript.SetDialogueInputDelay(0.5f);
				break;
			}
			while (RewardBoxScript.IsOpening() || DialogueScript.IsEnabled())
			{
				yield return null;
			}
			CameraController.SetPosition(CameraController.PositionKind.ATM, false, 0f);
		}
		else
		{
			bool flag = RewardBoxScript.IsOpened() && RewardBoxScript.HasPrize();
			if (GameplayData.RewardTimeToShowAmmount() && !RewardBoxScript.IsOpened())
			{
				CameraController.SetPosition(CameraController.PositionKind.RewardBox, false, 1f * transitionSpeed);
				AlarmRewardBox.AlarmRing();
				while (AlarmRewardBox.IsRinging())
				{
					yield return null;
				}
				RewardBoxScript.RefreshText_ToDeadlineDebtToReach();
				timer = 1f;
				while (timer > 0f)
				{
					timer -= Tick.Time;
					yield return null;
				}
				if (RewardBoxScript.GetRewardKind() == RewardBoxScript.RewardKind.DoorKey)
				{
					DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_REWARD_BOX_DECIDED_DEADLINE_FINAL_KEY" });
				}
				else
				{
					DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_REWARD_BOX_DECIDED_DEADLINE" });
				}
				while (DialogueScript.IsEnabled())
				{
					yield return null;
				}
				CameraController.SetPosition(CameraController.PositionKind.ATM, false, 0f);
			}
			else if (flag && !GameplayData.ATMDeadline_RewardPickupMemo_MessageShownGet())
			{
				GameplayData.ATMDeadline_RewardPickupMemo_MessageShownSet();
				CameraController.SetPosition(CameraController.PositionKind.RewardBox, false, 1f * transitionSpeed);
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_REWARD_BOX_DONT_FORGET_PICKUP_PRIZE" });
				while (DialogueScript.IsEnabled())
				{
					yield return null;
				}
				CameraController.SetPosition(CameraController.PositionKind.ATM, false, 0f);
			}
		}
		if (debtIndexOld == GameplayData.SuperSixSixSix_GetMinimumDebtIndex() - 1 && (!isRewardBoxTime || !Master.IsDemo) && !GameplayData.NineNineNine_IsTime())
		{
			Strings.SetTemporaryFlag_Sanitize666And999(1);
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_DEBT_SUPER_666_0" });
			DialogueScript.SetDialogueInputDelay(0.5f);
			while (DialogueScript.IsEnabled())
			{
				yield return null;
			}
		}
		float _deadlineReward_Time = Tick.PassedTime;
		CameraController.SetPosition(CameraController.PositionKind.DeadlineBonus, false, 1f * transitionSpeed);
		if (!isRewardBoxTime && !ATMScript.deadlineBonusDialogueShowedSinceStartup)
		{
			ATMScript.deadlineBonusDialogueShowedSinceStartup = true;
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_DEADLINE_BONUS_0" });
			DialogueScript.SetDialogueInputDelay(0.5f);
		}
		GameplayMaster.Event onDeadlineBonus = GameplayMaster.instance.onDeadlineBonus;
		if (onDeadlineBonus != null)
		{
			onDeadlineBonus();
		}
		long num2 = GameplayData.DeadlineReward_CloverTickets(Mathf.Max(roundsLeft, 0));
		num2 += GameplayData.DeadlineReward_CloverTickets_Extras(true);
		if (num2 > 0L)
		{
			GameplayData.CloverTicketsAdd(num2, true);
			yield return null;
		}
		yield return null;
		GameplayData.InterestEarnedGrow_Manual(GameplayData.DeadlineReward_CoinsGet(debtIndexOld));
		if (runModifier == RunModifierScript.Identifier.interestsGrow)
		{
			GameplayData.InterestEarnedGrow();
			RunModifierScript.TriggerAnimation_IfEquipped(RunModifierScript.Identifier.interestsGrow);
		}
		GameplayMaster.Event onDeadlineBonus_Late = GameplayMaster.instance.onDeadlineBonus_Late;
		if (onDeadlineBonus_Late != null)
		{
			onDeadlineBonus_Late();
		}
		PowerupScript.WeirdClockDeadlineReset();
		PowerupScript.GiantShroom_DeadlineEndReset();
		PowerupScript.Calendar_IncreaseBonus(true, roundsLeft);
		PowerupScript.GoldenKingMida_GrowValue(roundsLeft);
		GameplayData.Powerup_GoldenPepper_LuckBonusSet(roundsLeft * 4);
		GameplayData.Powerup_BellPepper_LuckBonusSet(0);
		PowerupScript.ShoppingCart_TriggerAtDeadlineBegin();
		GameplayData.Powerup_Cigarettes_PriceIncrease = 0L;
		if (roundsLeft > 0)
		{
			Data.game.UnlockSteps_Calendar += roundsLeft;
		}
		while (DialogueScript.IsEnabled())
		{
			yield return null;
		}
		while (PowerupTriggerAnimController.HasAnimations())
		{
			yield return null;
		}
		while (Tick.PassedTime < _deadlineReward_Time + 2f && (Tick.PassedTime <= _deadlineReward_Time + 0.5f || !Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true)))
		{
			yield return null;
		}
		CameraController.SetPosition(CameraController.PositionKind.ATM, false, 0f);
		while (DialogueScript.IsEnabled())
		{
			yield return null;
		}
		if (!ATMScript.costIncreaseMessageShown && GameplayData.DebtIndexGet() == 1L && DrawersScript.GetDrawersUnlockedNum() < 4)
		{
			ATMScript.costIncreaseMessageShown = true;
			CameraController.PositionKind _cp = CameraController.GetPositionKind();
			CameraController.SetPosition(CameraController.PositionKind.Slot_Fixed, false, 1f * transitionSpeed);
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_SLOT_MACHINE_COST_INCREASED" });
			while (DialogueScript.IsEnabled())
			{
				yield return null;
			}
			CameraController.SetPosition(_cp, false, 0f);
		}
		try
		{
			PowerupScript.SkeletonFillDrawersWithCharms_Try();
			goto IL_0903;
		}
		catch (Exception ex)
		{
			string text = string.Concat(new string[] { "Error while putting powerups into drawers after debt increase. SOURCE: ", ex.Source, "\nMESSAGE: ", ex.Message, "\nSTACK TRACE: ", ex.StackTrace });
			Debug.LogError(text);
			ConsolePrompt.LogError(text, "", 0f);
			goto IL_0903;
		}
		IL_08EB:
		yield return null;
		IL_0903:
		if (!PowerupTriggerAnimController.HasAnimations())
		{
			if (!lastDemoDeadline)
			{
				this.crownImageGameObject.SetActive(false);
			}
			CameraController.SetPosition(oldCameraPosition, false, 0f);
			CameraController.SetFreeCameraRotation(backupCameraRotation);
			RedButtonScript.ButtonVisualsRefresh();
			if (!lastDemoDeadline)
			{
				PhoneScript.PhoneRing();
			}
			DeadlineBonusScreen.UpdateValues();
			DeadlineScreenScript.ForceUpdate();
			GameplayData.RunModifier_DealIsAvailable_Set(false);
			if (DeckBoxScript.ItsMemoryCardTime())
			{
				BigInteger bigInteger = GameplayData.CoinsGet();
				BigInteger bigInteger2 = GameplayData.DepositGet();
				BigInteger bigInteger3 = GameplayData.DebtGet();
				bool flag2 = RunModifierScript.CardGetFromPack() == RunModifierScript.Identifier.undefined;
				if (bigInteger + bigInteger2 > bigInteger3 && !flag2)
				{
					CameraController.SetPosition(CameraController.PositionKind.ATMStraight, false, 0f);
					yield return null;
					timer = 1.5f;
					while (timer > 0f)
					{
						timer -= Tick.Time;
						if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true))
						{
							break;
						}
						yield return null;
					}
					float timeNow = Tick.PassedTime;
					if (GameplayMaster.IsCustomSeed())
					{
						this.dealsProposalCounter++;
						if (this.dealsProposalCounter <= 1)
						{
							DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_SKIP_DEADLINE_BUT_SEEDED_RUN" });
							DialogueScript.SetDialogueInputDelay(0.5f);
						}
					}
					else
					{
						GameplayData.RunModifier_DealIsAvailable_Set(true);
						ATMScript.Button_SetDeal();
						this.dealsProposalCounter++;
						if (this.dealsProposalCounter <= 1)
						{
							DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_SKIP_DEADLINE_REMINDER_0", "DIALOGUE_SKIP_DEADLINE_REMINDER_1" });
							DialogueScript.SetDialogueInputDelay(0.5f);
						}
					}
					while (DialogueScript.IsEnabled())
					{
						yield return null;
					}
					while (Tick.PassedTime - timeNow < 1.5f)
					{
						yield return null;
					}
				}
				CameraController.SetPosition(oldCameraPosition, false, 0f);
				CameraController.SetFreeCameraRotation(backupCameraRotation);
			}
			DrawersScript.TryPuttingEasterEgg();
			GameplayMaster.SetGamePhase(oldGamePhase, false, null);
			Data.SaveGame(Data.GameSavingReason.endOfDeadline_AfterCutscene, -1);
			this.depositButtonDelay = 1f;
			this.debtClearCutscenePlaying = false;
			yield break;
		}
		goto IL_08EB;
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x000092D8 File Offset: 0x000074D8
	public void DebtTextFlash()
	{
		this.flashDepositTextTimer = 0.75f;
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x0002F09C File Offset: 0x0002D29C
	private void DepositFlashUpdate()
	{
		this.flashDepositTextTimer -= Tick.Time;
		if (this.flashDepositTextTimer > 0f)
		{
			float num = Util.AngleSin(Tick.PassedTime * 1440f);
			Color color = ((num > 0.3f) ? Color.yellow : ((num > -0.3f) ? ATMScript.C_ORANGE : ATMScript.C_WHITISH_YELLOW));
			if (this.textDeposited.color != color)
			{
				this.textDeposited.color = color;
			}
		}
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x000092E5 File Offset: 0x000074E5
	public static bool IsDepositButtonDelayed()
	{
		return !(ATMScript.instance == null) && ATMScript.instance.depositButtonDelay > 0f;
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0002F11C File Offset: 0x0002D31C
	private void UpdateTextColor()
	{
		Color c_ORANGE = ATMScript.C_ORANGE;
		Color color = c_ORANGE;
		if (GameplayMaster.DeathCountdownHasStarted())
		{
			color = Color.red;
		}
		this.flashDepositTextTimer -= Tick.Time;
		if (this.flashDepositTextTimer > 0f)
		{
			float num = Util.AngleSin(Tick.PassedTime * 1440f);
			Color color2 = ((num > 0.3f) ? Color.yellow : ((num > -0.3f) ? ATMScript.C_ORANGE : ATMScript.C_WHITISH_YELLOW));
			if (this.textDeposited.color != color2)
			{
				this.textDeposited.color = color2;
			}
		}
		else if (this.textDeposited.color != color)
		{
			this.textDeposited.color = color;
		}
		if (this.textRoundsLeft.color != color)
		{
			this.textRoundsLeft.color = color;
			this.textReachCoins.color = color;
			this.textLabelsReachDepositInterest.color = c_ORANGE;
			this.textInterest.color = c_ORANGE;
		}
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x00009307 File Offset: 0x00007507
	public static void Button_SetDeal()
	{
		if (ATMScript.instance == null)
		{
			return;
		}
		if (ATMScript.instance.buttonDealCoroutine != null)
		{
			return;
		}
		ATMScript.instance.buttonDealCoroutine = ATMScript.instance.StartCoroutine(ATMScript.instance.Button_DealCoroutine());
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x00009342 File Offset: 0x00007542
	private IEnumerator Button_DealCoroutine()
	{
		float timer = 0.5f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		this.buttonParticles.SetActive(false);
		this.buttonParticles.SetActive(true);
		if (GameplayMaster.GetGamePhase() != GameplayMaster.GamePhase.intro)
		{
			Sound.Play3D("SoundAtmButtonFlashStart", this.buttonMeshRend.transform.position, 20f, 1f, 1f, AudioRolloffMode.Linear);
		}
		bool flashOld = false;
		while (GameplayData.RunModifier_DealIsAvailable_Get())
		{
			GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
			bool flashingLightsReducedEnabled = Data.settings.flashingLightsReducedEnabled;
			bool flag = Util.AngleSin(Tick.PassedTime * (flashingLightsReducedEnabled ? 120f : 360f)) > 0f;
			Material material = (flag ? this.buttonDealMaterial_0 : this.buttonDealMaterial_1);
			if (this.buttonMeshRend.sharedMaterial != material)
			{
				this.buttonMeshRend.sharedMaterial = material;
			}
			if (flashOld != flag)
			{
				flashOld = flag;
				bool flag2 = gamePhase == GameplayMaster.GamePhase.preparation || gamePhase == GameplayMaster.GamePhase.cutscene;
				if (!Sound.IsPlaying("SoundAtmButtonFlashStart") && flag2)
				{
					Sound.Play3D("SoundATMButtonFlash", this.buttonMeshRend.transform.position, 5f, 0.6f, flag ? 1.25f : 1f, AudioRolloffMode.Linear);
				}
			}
			yield return null;
		}
		timer = 0.5f;
		if (MemoryPackDealUI.IsDealRunnning())
		{
			timer = 0f;
		}
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		this.buttonParticles.SetActive(false);
		this.buttonParticles.SetActive(true);
		if (GameplayMaster.GetGamePhase() != GameplayMaster.GamePhase.endingWithoutDeath && GameplayMaster.GetGamePhase() != GameplayMaster.GamePhase.death)
		{
			Sound.Play3D("SoundAtmButtonFlashEnd", this.buttonMeshRend.transform.position, 20f, 1f, 1f, AudioRolloffMode.Linear);
		}
		this.buttonMeshRend.sharedMaterial = this.buttonDefaultMaterial;
		this.buttonDealCoroutine = null;
		yield break;
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x00009351 File Offset: 0x00007551
	public static bool Button_DealIsRunning()
	{
		return !(ATMScript.instance == null) && ATMScript.instance.buttonDealCoroutine != null;
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x0000936F File Offset: 0x0000756F
	public static void Initialize()
	{
		if (GameplayData.RunModifier_DealIsAvailable_Get())
		{
			ATMScript.Button_SetDeal();
		}
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x0000937D File Offset: 0x0000757D
	private void Awake()
	{
		ATMScript.instance = this;
		this.coinVisualizerChildren = base.GetComponentsInChildren<CoinVisualizerScript>(true);
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x00009392 File Offset: 0x00007592
	private void OnDestroy()
	{
		if (ATMScript.instance == this)
		{
			ATMScript.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.OnLanguageChange));
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x0002F218 File Offset: 0x0002D418
	private void Start()
	{
		this.crownImageGameObject.SetActive(false);
		this.skullImageGameObject.SetActive(false);
		this.UpdateStrings(true);
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.OnLanguageChange));
		this.buttonParticles.SetActive(false);
		this.buttonMeshRend.sharedMaterial = this.buttonDefaultMaterial;
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x0002F284 File Offset: 0x0002D484
	private void Update()
	{
		GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
		if (!Tick.IsGameRunning)
		{
			return;
		}
		this.depositButtonDelay -= Tick.Time;
		this.UpdateStrings(false);
		BigInteger bigInteger = GameplayData.InterestEarnedGet();
		int num = bigInteger.CastToInt();
		if (bigInteger != this.interestAccumulatedOld)
		{
			string text = "SoundCoinFall";
			if (bigInteger >= 5L)
			{
				text = null;
			}
			if (text == null && !GeneralUiScript.instance.IsShowingTitleScreen())
			{
				Sound.Play("SoundCoinsMultipleFall", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
			}
			CoinVisualizerScript.ArrayCheckShow(this.coinVisualizerChildren, num - 1, 0.1f, 0.001f, text);
			if (PromptGuideScript.GetGuideType() == PromptGuideScript.GuideType.atm_GetRevenue)
			{
				PromptGuideScript.ResetGuide();
			}
			this.interestAccumulatedOld = bigInteger;
		}
		this.insertCoinHolderTr.localPosition = global::UnityEngine.Vector3.Lerp(this.insertCoinHolderTr.localPosition, global::UnityEngine.Vector3.zero, Time.deltaTime * 20f);
		this.UpdateTextColor();
		if (!ATMScript.DebtClearCutsceneIsPlaying() && GameplayData.DepositGet() >= GameplayData.DebtGet() && !DialogueScript.IsEnabled() && gamePhase == GameplayMaster.GamePhase.preparation && !MemoryPackDealUI.IsDealRunnning())
		{
			if (GameplayMaster.DeathCountdownHasStarted())
			{
				GameplayMaster.DeathCountdownResetRequest(false);
				return;
			}
			this.DebtClearCutsceneStart();
		}
	}

	// Token: 0x04000406 RID: 1030
	public static ATMScript instance;

	// Token: 0x04000407 RID: 1031
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000408 RID: 1032
	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x04000409 RID: 1033
	private static Color C_WHITISH_YELLOW = new Color(1f, 1f, 0.9f, 1f);

	// Token: 0x0400040A RID: 1034
	public TextMeshProUGUI textRoundsLeft;

	// Token: 0x0400040B RID: 1035
	public TextMeshProUGUI textLabelsReachDepositInterest;

	// Token: 0x0400040C RID: 1036
	public TextMeshProUGUI textReachCoins;

	// Token: 0x0400040D RID: 1037
	public TextMeshProUGUI textDeposited;

	// Token: 0x0400040E RID: 1038
	public TextMeshProUGUI textInterest;

	// Token: 0x0400040F RID: 1039
	public Transform insertCoinHolderTr;

	// Token: 0x04000410 RID: 1040
	public GameObject crownImageGameObject;

	// Token: 0x04000411 RID: 1041
	public GameObject skullImageGameObject;

	// Token: 0x04000412 RID: 1042
	private CoinVisualizerScript[] coinVisualizerChildren;

	// Token: 0x04000413 RID: 1043
	public MeshRenderer buttonMeshRend;

	// Token: 0x04000414 RID: 1044
	public Material buttonDefaultMaterial;

	// Token: 0x04000415 RID: 1045
	public Material buttonDealMaterial_0;

	// Token: 0x04000416 RID: 1046
	public Material buttonDealMaterial_1;

	// Token: 0x04000417 RID: 1047
	public GameObject buttonParticles;

	// Token: 0x04000418 RID: 1048
	private global::UnityEngine.Vector3 sound3dOffset = new global::UnityEngine.Vector3(0f, 2f, 0f);

	// Token: 0x04000419 RID: 1049
	private BigInteger roundsLeftOld = -1;

	// Token: 0x0400041A RID: 1050
	private BigInteger debtOld = -1;

	// Token: 0x0400041B RID: 1051
	private BigInteger depositedOld = -1;

	// Token: 0x0400041C RID: 1052
	private float interestOld = -1f;

	// Token: 0x0400041D RID: 1053
	private BigInteger hypoteticalInterestOld = -1;

	// Token: 0x0400041E RID: 1054
	private BigInteger interestAccumulatedOld = -1;

	// Token: 0x0400041F RID: 1055
	private bool debtClearCutscenePlaying;

	// Token: 0x04000420 RID: 1056
	private static bool deadlineBonusDialogueShowedSinceStartup = false;

	// Token: 0x04000421 RID: 1057
	private static bool costIncreaseMessageShown = false;

	// Token: 0x04000422 RID: 1058
	private int dealsProposalCounter;

	// Token: 0x04000423 RID: 1059
	private float flashDepositTextTimer;

	// Token: 0x04000424 RID: 1060
	private float depositButtonDelay;

	// Token: 0x04000425 RID: 1061
	private Coroutine buttonDealCoroutine;
}
