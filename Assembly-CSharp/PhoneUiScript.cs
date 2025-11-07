using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Febucci.UI;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhoneUiScript : MonoBehaviour
{
	// Token: 0x06000987 RID: 2439 RVA: 0x0003E934 File Offset: 0x0003CB34
	public static bool IsEnabled()
	{
		return !(PhoneUiScript.instance == null) && PhoneUiScript.instance.holder.activeSelf;
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x0003E954 File Offset: 0x0003CB54
	public static void Open()
	{
		if (PhoneUiScript.instance == null)
		{
			return;
		}
		PhoneUiScript.instance.holder.SetActive(true);
		if (PhoneUiScript.instance.mainCoroutine != null)
		{
			PhoneUiScript.instance.StopCoroutine(PhoneUiScript.instance.mainCoroutine);
		}
		PhoneUiScript.instance.mainCoroutine = PhoneUiScript.instance.StartCoroutine(PhoneUiScript.instance.MainCoroutine());
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x0003E9BD File Offset: 0x0003CBBD
	private IEnumerator MainCoroutine()
	{
		bool hasNoDialogue = PhoneScript.HasNoDialogue();
		GameplayData gpDataInst = GameplayData.Instance;
		AbilityScript abilityPicked = null;
		VirtualCursors.CursorDesiredVisibilitySet(0, true);
		this.uiHolder.SetActive(false);
		this.twitchLabelHolder.SetActive(false);
		this.phoneHolder.transform.SetLocalY(-640f);
		Sound.Play3D("SoundWooshIn", this.phoneHolder.transform.position, 20f, 1f, 1f, 1);
		while (this.phoneHolder.transform.localPosition.y < -1f)
		{
			this.phoneHolder.transform.AddLocalY((0f - this.phoneHolder.transform.GetLocalY()) * 20f * Tick.Time);
			yield return null;
		}
		this.phoneHolder.transform.SetLocalY(0f);
		if (hasNoDialogue)
		{
			Sound.Play3D("SoundPhoneNoLine", PhoneScript.instance.transform.position, 20f, 1f, 1f, 1);
			float timer = 3f;
			while (timer > 0f)
			{
				timer -= Tick.Time;
				if ((timer < 2f && (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true) || Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true))) || PhoneUiScript.IsForceClosing())
				{
					break;
				}
				yield return null;
			}
			Sound.Stop("SoundPhoneNoLine", true);
		}
		else
		{
			bool flag = false;
			bool flag2 = false;
			this.selectionIndex = -1;
			gpDataInst._phone_PickupWithAbilities_OverallCounter++;
			this.DefinePhoneStuff(false);
			this.uiHolder.SetActive(true);
			if (TwitchMaster.IsLoggedInAndEnabled())
			{
				this.twitchLabelHolder.SetActive(true);
				this.TwitchTextUpdate();
			}
			float timer = 0.5f;
			while (timer > 0f)
			{
				timer -= Tick.Time;
				yield return null;
			}
			while (!PhoneUiScript.IsForceClosing())
			{
				this.NavigationUpdate(gpDataInst, out flag2, out flag, out abilityPicked);
				if (flag2)
				{
					break;
				}
				if (flag)
				{
					this.uiHolder.SetActive(false);
					this.phoneHolder.transform.SetLocalY(-640f);
					Sound.Play3D("SoundWooshIn", this.phoneHolder.transform.position, 20f, 1f, 1f, 1);
					while (this.phoneHolder.transform.localPosition.y < -1f)
					{
						this.phoneHolder.transform.AddLocalY((0f - this.phoneHolder.transform.GetLocalY()) * 20f * Tick.Time);
						yield return null;
					}
					this.phoneHolder.transform.SetLocalY(0f);
					this.uiHolder.SetActive(true);
				}
				if (abilityPicked == null)
				{
					yield return null;
				}
				else
				{
					if (abilityPicked != null && !PhoneUiScript.IsForceClosing())
					{
						yield return null;
						for (int i = 0; i < PhoneUiButton.allButtons.Count; i++)
						{
							if (i != this.selectionIndex)
							{
								PhoneUiButton.allButtons[i].gameObject.SetActive(false);
							}
						}
						timer = 0.5f;
						while (timer > 0f)
						{
							if (PhoneUiScript.IsForceClosing())
							{
								goto IL_0564;
							}
							timer -= Tick.Time;
							if (Util.AngleSin(Tick.PassedTime * 1440f) > 0f)
							{
								PhoneUiButton.allButtons[this.selectionIndex].Highlight(false);
							}
							else
							{
								PhoneUiButton.allButtons[this.selectionIndex].HighlightOff();
							}
							yield return null;
						}
						string text = abilityPicked.ReplyGetRandom();
						this.phoneDialogueTextAnimator.tmproText.text = text;
						this.DialogueSound(gpDataInst, true);
						while (!PhoneUiScript.IsForceClosing())
						{
							if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) || Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true))
							{
								PhoneUiButton.allButtons[this.selectionIndex].HighlightOff();
								RunModifierScript.TriggerAnimation_IfEquipped(RunModifierScript.Identifier.phoneEnhancer);
								break;
							}
							if (Util.AngleSin(Tick.PassedTime * 1440f) > 0f)
							{
								PhoneUiButton.allButtons[this.selectionIndex].Highlight(false);
							}
							else
							{
								PhoneUiButton.allButtons[this.selectionIndex].HighlightOff();
							}
							yield return null;
						}
						break;
					}
					break;
				}
			}
		}
		IL_0564:
		Data.SaveGame(Data.GameSavingReason.phoneSaveTime, -1);
		gpDataInst._phone_pickedUpOnceLastDeadline = true;
		this.uiHolder.SetActive(false);
		this.twitchLabelHolder.SetActive(false);
		VirtualCursors.CursorDesiredVisibilitySet(0, false);
		Sound.Play3D("SoundWooshOut", this.phoneHolder.transform.position, 20f, 1f, 1f, 1);
		while (this.phoneHolder.transform.localPosition.y > -639f && !PhoneUiScript.IsForceClosing())
		{
			this.phoneHolder.transform.AddLocalY((-640f - this.phoneHolder.transform.GetLocalY()) * 20f * Tick.Time);
			yield return null;
		}
		this.phoneHolder.transform.SetLocalY(-640f);
		this.holder.SetActive(false);
		for (int j = 0; j < PhoneUiButton.allButtons.Count; j++)
		{
			PhoneUiButton.allButtons[j].gameObject.SetActive(true);
		}
		GameplayMaster.instance.FCall_Phone_Hangup();
		this.selectionIndex = -1;
		for (int k = 0; k < PhoneUiButton.allButtons.Count; k++)
		{
			PhoneUiButton.allButtons[k].HighlightOff();
		}
		if (abilityPicked != null && !hasNoDialogue && !PhoneUiScript.IsForceClosing())
		{
			this.ClosingDialogueEvaluate();
		}
		this.forceClose_Death = false;
		this.mainCoroutine = null;
		yield break;
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x0003E9CC File Offset: 0x0003CBCC
	private void DefinePhoneStuff(bool refreshRequestedThis)
	{
		GameplayData gameplayData = GameplayData.Instance;
		AbilityScript.Category category = gameplayData._phone_lastAbilityCategory;
		int num = GameplayData.PhoneAbilitiesNumber_Get();
		int num2 = 4;
		BigInteger bigInteger = GameplayData.DebtIndexGet();
		BigInteger bigInteger2 = GameplayData.SixSixSix_GetMinimumDebtIndex();
		BigInteger bigInteger3 = GameplayData.SuperSixSixSix_GetMinimumDebtIndex();
		bool flag = bigInteger == bigInteger2 || bigInteger == bigInteger3;
		if (gameplayData._phone_pickedUpOnceLastDeadline)
		{
			if (!refreshRequestedThis)
			{
				goto IL_026C;
			}
		}
		else
		{
			if (gameplayData._phone_bookSpecialCall || flag)
			{
				gameplayData._phone_SpecialCalls_Counter++;
				if (GameplayData.NineNineNine_IsTime())
				{
					category = AbilityScript.Category.good;
				}
				else
				{
					category = AbilityScript.Category.evil;
				}
			}
			else
			{
				category = AbilityScript.Category.normal;
			}
			if (category == AbilityScript.Category.undefined)
			{
				Debug.LogError("PhoneUiScript: Undefined category for abilities.");
				return;
			}
			gameplayData._phone_lastAbilityCategory = category;
			gameplayData._phone_bookSpecialCall = false;
			if (category == AbilityScript.Category.good && !gameplayData._phoneAlreadyTransformed)
			{
				gameplayData._phoneAlreadyTransformed = true;
				this.closeDialogueFlag_JustTransformedTo999 = true;
				Sound.Play("SoundPhoneHolyTransformation", 1f, 1f);
				FlashScreen.SpawnCamera(Color.white, 1f, 2f, CameraGame.firstInstance.myCamera, 0.5f);
				CameraGame.Shake(2f);
				CameraGame.ChromaticAberrationIntensitySet(1f);
				Controls.VibrationSet_PreferMax(this.player, 0.5f);
				PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.TheRighteousPath);
			}
		}
		this.sameAbilitiesRerollPreventionList.Clear();
		for (int i = 0; i < gameplayData._phone_AbilitiesToPick.Count - 1; i++)
		{
			this.sameAbilitiesRerollPreventionList.Add(AbilityScript.AbilityGet(gameplayData._phone_AbilitiesToPick[i]));
		}
		gameplayData._phone_AbilitiesToPick.Clear();
		List<AbilityScript> list = AbilityScript.dict_ListsByCategory[category];
		int num3 = 1000;
		for (int j = 0; j < num2; j++)
		{
			if (list.Count == 0)
			{
				gameplayData._phone_AbilitiesToPick.Add(AbilityScript.Identifier.undefined);
				break;
			}
			num3--;
			if (num3 <= 0)
			{
				gameplayData._phone_AbilitiesToPick.Add(list[0].IdentifierGet());
				break;
			}
			AbilityScript abilityScript = list[R.Rng_Phone.Range(0, list.Count)];
			if (abilityScript == null)
			{
				j--;
			}
			else
			{
				AbilityScript.Identifier identifier = abilityScript.IdentifierGet();
				if (gameplayData._phone_AbilitiesToPick.Contains(identifier))
				{
					j--;
				}
				else if (!abilityScript.CanBePicked())
				{
					j--;
				}
				else if (this.sameAbilitiesRerollPreventionList.Contains(abilityScript))
				{
					j--;
				}
				else if (num3 > 10 && abilityScript.RarityRerollEvaluate())
				{
					j--;
				}
				else
				{
					gameplayData._phone_AbilitiesToPick.Add(identifier);
				}
			}
		}
		IL_026C:
		string text;
		if (gameplayData._phone_pickedUpOnceLastDeadline && !refreshRequestedThis)
		{
			switch (gameplayData._phone_lastAbilityCategory)
			{
			case AbilityScript.Category.normal:
				text = Translation.Get("PHONE_PICKING_BACK_UP_NORMAL_" + Util.Choose<int>(new int[] { 0, 1, 2 }).ToString());
				break;
			case AbilityScript.Category.evil:
				text = Translation.Get("PHONE_PICKING_BACK_UP_EVIL_" + Util.Choose<int>(new int[] { 0, 1, 2 }).ToString());
				break;
			case AbilityScript.Category.good:
				text = Translation.Get("PHONE_PICKING_BACK_UP_HOLY_" + Util.Choose<int>(new int[] { 0, 1, 2 }).ToString());
				break;
			default:
				Debug.LogError("PhoneUiScript: Undefined category for abilities.");
				return;
			}
		}
		else
		{
			switch (gameplayData._phone_lastAbilityCategory)
			{
			case AbilityScript.Category.normal:
				text = Translation.Get("PHONE_QUESTION_NORMAL_" + Util.Choose<int>(new int[] { 0, 1, 2, 3, 4 }).ToString());
				break;
			case AbilityScript.Category.evil:
				text = Translation.Get("PHONE_QUESTION_EVIL_" + Util.Choose<int>(new int[] { 0, 1, 2, 3, 4 }).ToString());
				break;
			case AbilityScript.Category.good:
				text = Translation.Get("PHONE_QUESTION_HOLY_" + Util.Choose<int>(new int[] { 0, 1, 2, 3, 4 }).ToString());
				break;
			default:
				Debug.LogError("PhoneUiScript: Undefined category for abilities.");
				return;
			}
		}
		this.phoneDialogueTextAnimator.tmproText.text = text;
		this.DialogueSound(gameplayData, false);
		switch (gameplayData._phone_lastAbilityCategory)
		{
		case AbilityScript.Category.normal:
			this.phoneDialogueTextAnimator.tmproText.color = Color.white;
			break;
		case AbilityScript.Category.evil:
			this.phoneDialogueTextAnimator.tmproText.color = Color.red;
			break;
		case AbilityScript.Category.good:
			this.phoneDialogueTextAnimator.tmproText.color = Color.yellow;
			break;
		default:
			Debug.LogError("PhoneUiScript: Undefined category for abilities.");
			return;
		}
		for (int k = 0; k < PhoneAbilityUiScript.allAbilities.Count; k++)
		{
			PhoneAbilityUiScript.allAbilities[k].gameObject.SetActive(false);
		}
		for (int l = 0; l < num; l++)
		{
			if (l >= gameplayData._phone_AbilitiesToPick.Count || gameplayData._phone_AbilitiesToPick[l] == AbilityScript.Identifier.undefined)
			{
				PhoneAbilityUiScript.SetAbility(l, null);
			}
			else
			{
				AbilityScript abilityScript2 = AbilityScript.AbilityGet(gameplayData._phone_AbilitiesToPick[l]);
				if (abilityScript2 == null)
				{
					PhoneAbilityUiScript.SetAbility(l, null);
				}
				else
				{
					PhoneAbilityUiScript.SetAbility(l, abilityScript2);
				}
			}
		}
		this.SecondaryButtonsRefresh();
		gameplayData._phone_pickedUpOnceLastDeadline = true;
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x0003EF03 File Offset: 0x0003D103
	private void SecondaryButtonsRefresh()
	{
		this.rerollButtonText.text = Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("PHONE_REROLL_PROMPT"), Strings.SanitizationSubKind.none);
		this.backButtonText.text = Strings.Sanitize(Strings.SantizationKind.menus, Translation.Get("PHONE_BACK_PROMPT"), Strings.SanitizationSubKind.none);
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x0003EF40 File Offset: 0x0003D140
	private void DialogueSound(GameplayData gpDataInst, bool useReplyVersion)
	{
		switch (gpDataInst._phone_lastAbilityCategory)
		{
		case AbilityScript.Category.normal:
			if (useReplyVersion)
			{
				Sound.Play("SoundVoiceNormal_Reply", 1f, Random.Range(0.9f, 1.1f));
				return;
			}
			Sound.Play("SoundVoiceNormal", 1f, Random.Range(0.9f, 1.1f));
			return;
		case AbilityScript.Category.evil:
			if (useReplyVersion)
			{
				Sound.Play("SoundVoiceEvil_Reply", 1f, Random.Range(0.9f, 1.1f));
				return;
			}
			Sound.Play("SoundVoiceEvil", 1f, Random.Range(0.9f, 1.1f));
			return;
		case AbilityScript.Category.good:
			if (useReplyVersion)
			{
				Sound.Play("SoundVoiceGood_Reply", 1f, Random.Range(0.9f, 1.1f));
				return;
			}
			Sound.Play("SoundVoiceGood", 1f, Random.Range(0.9f, 1.1f));
			return;
		default:
			Debug.LogError("PhoneUiScript: Undefined category for abilities.");
			return;
		}
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x0003F040 File Offset: 0x0003D240
	private void NavigationUpdate(GameplayData gpDataInst, out bool goBack, out bool rerolled, out AbilityScript abilityPicked)
	{
		rerolled = false;
		goBack = false;
		abilityPicked = null;
		bool flag = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true);
		bool flag2 = this.selectionIndex < 0;
		if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true))
		{
			Sound.Play("SoundPhoneBack", 1f, 1f);
			goBack = true;
			return;
		}
		int num = this.selectionIndex;
		if (VirtualCursors.IsCursorVisible(0, true))
		{
			bool flag3 = false;
			for (int i = 0; i < PhoneUiButton.allButtons.Count; i++)
			{
				if (PhoneUiButton.allButtons[i].IsHovered())
				{
					num = i;
					flag3 = true;
					break;
				}
			}
			if (!flag3)
			{
				num = -1;
			}
		}
		else
		{
			Vector2 zero = Vector2.zero;
			Vector2 zero2 = Vector2.zero;
			zero2.x = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.menuMoveRight, Controls.InputAction.menuMoveLeft, true);
			zero2.y = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.menuMoveUp, Controls.InputAction.menuMoveDown, true);
			if (zero2.x > 0.35f && this.inputAxisRawPrev.x <= 0.35f)
			{
				zero.x += 1f;
			}
			if (zero2.x < -0.35f && this.inputAxisRawPrev.x >= -0.35f)
			{
				zero.x -= 1f;
			}
			if (zero2.y > 0.35f && this.inputAxisRawPrev.y <= 0.35f)
			{
				zero.y += 1f;
			}
			if (zero2.y < -0.35f && this.inputAxisRawPrev.y >= -0.35f)
			{
				zero.y -= 1f;
			}
			this.inputAxisRawPrev = zero2;
			if (num < 0 && (flag || zero.magnitude > 0.1f))
			{
				num = 2;
			}
			else
			{
				if (zero.y > 0f && PhoneUiButton.allButtons[this.selectionIndex].upButton != null && PhoneUiButton.allButtons[this.selectionIndex].upButton.gameObject.activeSelf)
				{
					num = PhoneUiButton.allButtons[this.selectionIndex].upButton.buttonIndex;
				}
				if (zero.y < 0f && PhoneUiButton.allButtons[this.selectionIndex].downButton != null && PhoneUiButton.allButtons[this.selectionIndex].downButton.gameObject.activeSelf)
				{
					num = PhoneUiButton.allButtons[this.selectionIndex].downButton.buttonIndex;
				}
				if (zero.x < 0f && PhoneUiButton.allButtons[this.selectionIndex].leftButton != null && PhoneUiButton.allButtons[this.selectionIndex].leftButton.gameObject.activeSelf)
				{
					num = PhoneUiButton.allButtons[this.selectionIndex].leftButton.buttonIndex;
				}
				if (zero.x > 0f && PhoneUiButton.allButtons[this.selectionIndex].rightButton != null && PhoneUiButton.allButtons[this.selectionIndex].rightButton.gameObject.activeSelf)
				{
					num = PhoneUiButton.allButtons[this.selectionIndex].rightButton.buttonIndex;
				}
			}
		}
		if (num != this.selectionIndex)
		{
			this.selectionIndex = num;
			if (this.selectionIndex >= 0)
			{
				Sound.Play("SoundPhoneSelectionChange", 1f, 1f);
			}
			for (int j = 0; j < PhoneUiButton.allButtons.Count; j++)
			{
				PhoneUiButton.allButtons[j].HighlightOff();
			}
			if (this.selectionIndex >= 0)
			{
				PhoneUiButton.allButtons[this.selectionIndex].Highlight(true);
			}
		}
		if (flag && this.selectionIndex >= 0 && !flag2)
		{
			if (this.selectionIndex < 2)
			{
				int num2 = this.selectionIndex;
				if (num2 == 0)
				{
					Sound.Play("SoundPhoneBack", 1f, 1f);
					goBack = true;
					return;
				}
				if (num2 == 1)
				{
					long num3 = GameplayData.CloverTicketsGet();
					long num4 = GameplayData.PhoneRerollCostGet();
					if (num3 >= num4)
					{
						rerolled = true;
						GameplayData.CloverTicketsAdd(-num4, true);
						this.DefinePhoneStuff(true);
						GameplayData.PhoneRerollCostIncrease();
						GameplayData.PhoneRerollPerformed_Set(GameplayData.PhoneRerollPerformed_Get() + 1L);
						GameplayData.PhoneRerollPerformed_PerDeadline += 1L;
						this.SecondaryButtonsRefresh();
						FlashScreen.SpawnCamera(Colors.GetColor("olive"), 1f, 2f, CameraGame.firstInstance.myCamera, 0.5f);
						Sound.Play("SoundPhoneReroll", 1f, 1f);
						Data.GameData game = Data.game;
						int unlockSteps_VoiceMail = game.UnlockSteps_VoiceMail;
						game.UnlockSteps_VoiceMail = unlockSteps_VoiceMail + 1;
						Data.SaveGame(Data.GameSavingReason.phoneReroll, -1);
					}
					else
					{
						CameraGame.Shake(1f);
						Sound.Play("SoundMenuError", 1f, 1f);
					}
				}
				Controls.VibrationSet_PreferMax(this.player, 0.25f);
				if (TwitchUiScript.IsEnabled())
				{
					TwitchUiScript.Close(true);
					return;
				}
			}
			else if (gpDataInst._phone_AbilitiesToPick.Count > this.selectionIndex - 2 && gpDataInst._phone_AbilitiesToPick[this.selectionIndex - 2] != AbilityScript.Identifier.undefined)
			{
				switch (this.selectionIndex)
				{
				case 2:
					abilityPicked = AbilityScript.AbilityGet(gpDataInst._phone_AbilitiesToPick[0]);
					break;
				case 3:
					abilityPicked = AbilityScript.AbilityGet(gpDataInst._phone_AbilitiesToPick[1]);
					break;
				case 4:
					abilityPicked = AbilityScript.AbilityGet(gpDataInst._phone_AbilitiesToPick[2]);
					break;
				case 5:
					abilityPicked = AbilityScript.AbilityGet(gpDataInst._phone_AbilitiesToPick[3]);
					break;
				case 6:
					abilityPicked = AbilityScript.AbilityGet(gpDataInst._phone_AbilitiesToPick[4]);
					break;
				}
				if (abilityPicked != null)
				{
					this.AbilityPick(abilityPicked);
					gpDataInst._phone_abilityAlreadyPickedUp = true;
					PowerupScript.HoleCross_RecordAbilityTry(abilityPicked.IdentifierGet());
				}
				Controls.VibrationSet_PreferMax(Controls.GetPlayerByIndex(0), 0.5f);
				if (abilityPicked.CategoryGet() == AbilityScript.Category.evil)
				{
					gpDataInst._phone_EvilCallsPicked_Counter++;
					FloppySlotScript.SixSixSixTextureUpdateToIgnoredCallsLevel(true);
				}
				if (TwitchUiScript.IsEnabled())
				{
					TwitchUiScript.Close(true);
					return;
				}
			}
			else
			{
				CameraGame.Shake(1f);
				Sound.Play("SoundMenuError", 1f, 1f);
			}
		}
	}

	// Token: 0x0600098E RID: 2446 RVA: 0x0003F688 File Offset: 0x0003D888
	public void AbilityPick(AbilityScript ability)
	{
		int num = GameplayData.PhonePickMultiplierGet(true);
		for (int i = 0; i < num; i++)
		{
			ability.Pick();
		}
	}

	// Token: 0x0600098F RID: 2447 RVA: 0x0003F6AE File Offset: 0x0003D8AE
	public static void ForceClose_Death()
	{
		if (PhoneUiScript.instance == null)
		{
			return;
		}
		if (PhoneUiScript.instance.mainCoroutine == null)
		{
			return;
		}
		PhoneUiScript.instance.forceClose_Death = true;
	}

	// Token: 0x06000990 RID: 2448 RVA: 0x0003F6D6 File Offset: 0x0003D8D6
	public static bool IsForceClosing()
	{
		return !(PhoneUiScript.instance == null) && PhoneUiScript.instance.forceClose_Death;
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x0003F6F4 File Offset: 0x0003D8F4
	private void ClosingDialogueEvaluate()
	{
		if (GameplayData.Instance == null)
		{
			return;
		}
		if (DrawersScript.GetDrawersUnlockedNum() == 0 && !PhoneUiScript.closeDialogueShown_FirstTime && !this.closeDialogueFlag_JustTransformedTo999)
		{
			PhoneUiScript.closeDialogueShown_FirstTime = true;
			DialogueScript.SetDialogue(false, new string[]
			{
				"DIALOGUE_JUST_THREE_DOTS",
				Util.Choose<string>(new string[] { "DIALOGUE_PHONE_FIRST_TIME_ALT_0", "DIALOGUE_PHONE_FIRST_TIME_ALT_1", "DIALOGUE_PHONE_FIRST_TIME_ALT_2" })
			});
		}
		if (GameplayData.NineNineNine_IsTime() && this.closeDialogueFlag_JustTransformedTo999 && !PhoneUiScript.closeDialogueShown_999Transformation)
		{
			PhoneUiScript.closeDialogueShown_999Transformation = true;
			this.closeDialogueFlag_JustTransformedTo999 = false;
			GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.cutscene, false, null);
			CameraController.SetPosition(CameraController.PositionKind.SlotFromTop, false, 2f);
			DialogueScript.SetDialogue(false, new string[]
			{
				"DIALOGUE_JUST_THREE_DOTS",
				Util.Choose<string>(new string[] { "DIALOGUE_PHONE_TRANSFORMATION_ALT_0", "DIALOGUE_PHONE_TRANSFORMATION_ALT_1", "DIALOGUE_PHONE_TRANSFORMATION_ALT_2" })
			});
		}
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x0003F7D3 File Offset: 0x0003D9D3
	private void TwitchUpdate()
	{
		this.TwitchTextUpdate();
		this.TwitchInputCheck();
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x0003F7E4 File Offset: 0x0003D9E4
	private void TwitchTextUpdate()
	{
		if (Controls.GetPlayerByIndex(0).lastInputKindUsed == this.inputKindOld)
		{
			return;
		}
		this.inputKindOld = Controls.GetPlayerByIndex(0).lastInputKindUsed;
		this.twitchLabelText.text = "<sprite name=\"TwitchPoll\"> " + Translation.Get("SCREEN_PROMPT_TWITCH_POLL") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.menuSocialButton, 0);
		this.twitchLabelText.ForceMeshUpdate(false, false);
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x0003F850 File Offset: 0x0003DA50
	private void TwitchInputCheck()
	{
		bool flag = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSocialButton, true);
		if (!flag)
		{
			return;
		}
		if (flag)
		{
			TwitchUiScript.instance.PollStartTry();
		}
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x0003F878 File Offset: 0x0003DA78
	private void Awake()
	{
		PhoneUiScript.instance = this;
		this.dialogueImageStartPos = this.dialogueImage.rectTransform.anchoredPosition;
		this.twitchImageStartPos = this.twitchLabelImage.rectTransform.anchoredPosition;
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x0003F8AC File Offset: 0x0003DAAC
	private void OnDestroy()
	{
		if (PhoneUiScript.instance == this)
		{
			PhoneUiScript.instance = null;
		}
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x0003F8C1 File Offset: 0x0003DAC1
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		this.holder.SetActive(false);
		this.twitchLabelHolder.SetActive(false);
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x0003F8E8 File Offset: 0x0003DAE8
	private void Update()
	{
		if (!PhoneUiScript.IsEnabled())
		{
			return;
		}
		float num = Util.AngleSin(Tick.PassedTime * 90f) * 5f;
		float num2 = Util.AngleSin(Tick.PassedTime * 60f + 90f) * 5f;
		this.phoneRotatorHolder.transform.eulerAngles = new Vector3(0f, num, num2);
		if (GameplayData.NineNineNine_IsTime() && this.cornettaMeshRenderer.material != this.cornettaMaterial_999)
		{
			this.cornettaMeshRenderer.material = this.cornettaMaterial_999;
		}
		Vector2 vector = this.dialogueImageStartPos + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		if (Data.settings.dyslexicFontEnabled)
		{
			vector = this.dialogueImageStartPos;
		}
		this.dialogueImage.rectTransform.anchoredPosition = vector;
		vector = this.twitchImageStartPos + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		if (Data.settings.dyslexicFontEnabled)
		{
			vector = this.twitchImageStartPos;
		}
		this.twitchLabelImage.rectTransform.anchoredPosition = vector;
		this.TwitchUpdate();
	}

	public static PhoneUiScript instance;

	private const int PLAYER_INDEX = 0;

	private const float CORNETTA_OUT_OF_SCREEN_Y = -640f;

	private const int ABILITIES_SELECTION_INDEX_OFFSET = 2;

	private Controls.PlayerExt player;

	public GameObject holder;

	public GameObject phoneHolder;

	public GameObject phoneRotatorHolder;

	public GameObject uiHolder;

	public RectTransform[] choicesRectTransforms;

	public Image[] selectorsImages;

	public Image dialogueImage;

	private Vector2 dialogueImageStartPos;

	public TextAnimator phoneDialogueTextAnimator;

	public TextMeshProUGUI rerollButtonText;

	public TextMeshProUGUI backButtonText;

	public MeshRenderer cornettaMeshRenderer;

	public Material cornettaMaterial_999;

	public GameObject twitchLabelHolder;

	public TextMeshProUGUI twitchLabelText;

	public Image twitchLabelImage;

	private Vector2 twitchImageStartPos;

	private Coroutine mainCoroutine;

	private List<AbilityScript> sameAbilitiesRerollPreventionList = new List<AbilityScript>();

	private int selectionIndex = -1;

	private Vector2 inputAxisRawPrev = Vector2.zero;

	private bool forceClose_Death;

	private static bool closeDialogueShown_FirstTime;

	private bool closeDialogueFlag_JustTransformedTo999;

	private static bool closeDialogueShown_999Transformation;

	private Controls.InputKind inputKindOld;
}
