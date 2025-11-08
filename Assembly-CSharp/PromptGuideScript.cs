using System;
using System.Collections.Generic;
using System.Numerics;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptGuideScript : MonoBehaviour
{
	// Token: 0x060009C5 RID: 2501 RVA: 0x00041925 File Offset: 0x0003FB25
	public static bool IsEnabled()
	{
		return !(PromptGuideScript.instance == null) && PromptGuideScript.instance.holder.activeSelf;
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x00041948 File Offset: 0x0003FB48
	public static void SetGuideType(PromptGuideScript.GuideType type)
	{
		if (PromptGuideScript.instance == null)
		{
			Debug.LogError("PromptGuideScript: SetType: no instance");
			return;
		}
		BigInteger bigInteger = GameplayData.CoinsGet();
		long num = GameplayData.CloverTicketsGet();
		switch (type)
		{
		case PromptGuideScript.GuideType.slot_Play:
			if (GameplayData.RoundsLeftToDeadline() > 0)
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_BEGIN_ROUND") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			}
			else
			{
				PromptGuideScript.SetText("<color=red>" + Translation.Get("SCREEN_PROMPT_TEXT_NOT_NOW") + "</color>", true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.slot_Play;
			break;
		case PromptGuideScript.GuideType.slot_Spin:
			if (VirtualCursors.IsCursorVisible(0, true))
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_SLOT_SPIN") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			}
			else if (PromptGuideScript.instance.player.lastInputKindUsed == Controls.InputKind.Keyboard)
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					Translation.Get("SCREEN_PROMPT_TEXT_SLOT_SPIN"),
					" ",
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0),
					"   ",
					Translation.Get("SCREEN_PROMPT_TEXT_SLOT_CHANGE_SELECTION"),
					" ",
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.menuMoveUp, 0),
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.menuMoveLeft, 0),
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.menuMoveDown, 0),
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.menuMoveRight, 0)
				}), true);
			}
			else
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					Translation.Get("SCREEN_PROMPT_TEXT_SLOT_SPIN"),
					" ",
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0),
					"   ",
					Translation.Get("SCREEN_PROMPT_TEXT_SLOT_CHANGE_SELECTION"),
					" ",
					PromptsMaster.GetSpriteString_Joystick(Controls.JoystickElement.LeftStickX),
					PromptsMaster.GetSpriteString_Joystick(Controls.JoystickElement.LeftStickY)
				}), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && !SlotMachineScript.IsSpinning() && GameplayData.SpinsLeftGet() > 0 && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.slot_Spin;
			break;
		case PromptGuideScript.GuideType.slot_AutoSpin:
			PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_SLOT_AUTO_SPIN") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.slot_AutoSpin;
			break;
		case PromptGuideScript.GuideType.slot_Exit:
			PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_SLOT_EXIT") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.slot_Exit;
			break;
		case PromptGuideScript.GuideType.atm_insertCoin:
			if (ATMScript.Button_DealIsRunning())
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_ATM_PACK_DEAL") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			}
			else
			{
				BigInteger bigInteger2 = GameplayData.NextDepositAmmountGet(false);
				BigInteger bigInteger3 = GameplayData.CoinsGet();
				BigInteger bigInteger4 = GameplayData.DepositGet();
				BigInteger bigInteger5 = GameplayData.DebtGet();
				bool flag = bigInteger2 >= bigInteger5 - bigInteger4;
				bool flag2 = bigInteger3 <= GameplayData.SpinCostMax_Get();
				string text = (flag ? "<sprite name=\"CardSymb_Victory\"> " : (flag2 ? "<sprite name=\"SlotWarning\"> " : ""));
				if (bigInteger3 >= bigInteger2)
				{
					PromptGuideScript.SetText(string.Concat(new string[]
					{
						text,
						Translation.Get("SCREEN_PROMPT_ATM_DEPOSIT"),
						" ",
						bigInteger2.ToStringSmart(),
						" <sprite name=\"CoinSymbolOrange32\"> ",
						Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0)
					}), true);
				}
				else
				{
					PromptGuideScript.SetText("<color=red>" + Translation.Get("SCREEN_PROMPT_ATM_DEPOSIT_NO_COINS") + "</color> ", true);
				}
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.atm_insertCoin;
			break;
		case PromptGuideScript.GuideType.atm_GetRevenue:
		{
			BigInteger bigInteger6 = GameplayData.InterestEarnedGet();
			if (bigInteger6 > 0L)
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					Translation.Get("SCREEN_PROMPT_ATM_INTERESTS_RETRIEVE"),
					" ",
					bigInteger6.ToStringSmart(),
					" <sprite name=\"CoinSymbolOrange32\"> ",
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0)
				}), true);
			}
			else
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_ATM_INTERESTS_NOT_AVAILABLE"), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.atm_GetRevenue;
			break;
		}
		case PromptGuideScript.GuideType.door_Lock:
			if (DoorScript.IsInConditionToUnlock())
			{
				PromptGuideScript.SetText(Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("SCREEN_PROMPT_TEXT_DOOR_UNLOCK"), Strings.SanitizationSubKind.none), true);
			}
			else
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_DOOR_LOCKED"), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.door_Lock;
			break;
		case PromptGuideScript.GuideType.store_box0:
		{
			BigInteger capsuleCost = StoreCapsuleScript.GetStoreCapsuleById(0).GetCapsuleCost();
			if (capsuleCost < 0L)
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_STORE_EMPTY"), true);
			}
			else if (num >= capsuleCost)
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					Translation.Get("SCREEN_PROMPT_STORE_BUY"),
					" ",
					capsuleCost.ToStringSmart(),
					" <sprite name=\"CloverTicket\"> ",
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0)
				}), true);
			}
			else
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					"<color=red>",
					Translation.Get("SCREEN_PROMPT_STORE_NOT_ENOUGH_TICKETS"),
					"</color> ",
					capsuleCost.ToStringSmart(),
					" <sprite name=\"CloverTicket\">"
				}), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.store_box0;
			break;
		}
		case PromptGuideScript.GuideType.store_box1:
		{
			BigInteger capsuleCost2 = StoreCapsuleScript.GetStoreCapsuleById(1).GetCapsuleCost();
			if (capsuleCost2 < 0L)
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_STORE_EMPTY"), true);
			}
			else if (num >= capsuleCost2)
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					Translation.Get("SCREEN_PROMPT_STORE_BUY"),
					" ",
					capsuleCost2.ToStringSmart(),
					" <sprite name=\"CloverTicket\"> ",
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0)
				}), true);
			}
			else
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					"<color=red>",
					Translation.Get("SCREEN_PROMPT_STORE_NOT_ENOUGH_TICKETS"),
					"</color> ",
					capsuleCost2.ToStringSmart(),
					" <sprite name=\"CloverTicket\">"
				}), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.store_box1;
			break;
		}
		case PromptGuideScript.GuideType.store_box2:
		{
			BigInteger capsuleCost3 = StoreCapsuleScript.GetStoreCapsuleById(2).GetCapsuleCost();
			if (capsuleCost3 < 0L)
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_STORE_EMPTY"), true);
			}
			else if (num >= capsuleCost3)
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					Translation.Get("SCREEN_PROMPT_STORE_BUY"),
					" ",
					capsuleCost3.ToStringSmart(),
					" <sprite name=\"CloverTicket\"> ",
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0)
				}), true);
			}
			else
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					"<color=red>",
					Translation.Get("SCREEN_PROMPT_STORE_NOT_ENOUGH_TICKETS"),
					"</color> ",
					capsuleCost3.ToStringSmart(),
					" <sprite name=\"CloverTicket\">"
				}), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.store_box2;
			break;
		}
		case PromptGuideScript.GuideType.store_refresh:
		{
			BigInteger capsuleCost4 = StoreCapsuleScript.GetStoreCapsuleById(4).GetCapsuleCost();
			string text2 = ((GameplayData.CoinsGet() - capsuleCost4 < GameplayData.SpinCostMax_Get()) ? "<sprite name=\"SlotWarning\"> " : "");
			if (bigInteger >= capsuleCost4)
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					text2,
					Translation.Get("SCREEN_PROMPT_STORE_REFRESH"),
					" ",
					capsuleCost4.ToStringSmart(),
					" <sprite name=\"CoinSymbolOrange64\"> ",
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0)
				}), true);
			}
			else
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					"<color=red>",
					Translation.Get("SCREEN_PROMPT_STORE_REFRESH"),
					"</color> ",
					capsuleCost4.ToStringSmart(),
					" <sprite name=\"CoinSymbolOrange64\">"
				}), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.store_refresh;
			break;
		}
		case PromptGuideScript.GuideType.drawer0:
			if (DrawersScript.IsDrawerUnlocked(0))
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_DRAWER_OPEN") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			}
			else if (DrawersScript.IsInConditionToUnlock())
			{
				PromptGuideScript.SetText(Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("SCREEN_PROMPT_TEXT_DRAWER_UNLOCK"), Strings.SanitizationSubKind.none), true);
			}
			else
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_DRAWER_LOCKED"), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.drawer0;
			break;
		case PromptGuideScript.GuideType.drawer1:
			if (DrawersScript.IsDrawerUnlocked(1))
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_DRAWER_OPEN") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			}
			else if (DrawersScript.IsInConditionToUnlock())
			{
				PromptGuideScript.SetText(Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("SCREEN_PROMPT_TEXT_DRAWER_UNLOCK"), Strings.SanitizationSubKind.none), true);
			}
			else
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_DRAWER_LOCKED"), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.drawer1;
			break;
		case PromptGuideScript.GuideType.drawer2:
			if (DrawersScript.IsDrawerUnlocked(2))
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_DRAWER_OPEN") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			}
			else if (DrawersScript.IsInConditionToUnlock())
			{
				PromptGuideScript.SetText(Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("SCREEN_PROMPT_TEXT_DRAWER_UNLOCK"), Strings.SanitizationSubKind.none), true);
			}
			else
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_DRAWER_LOCKED"), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.drawer2;
			break;
		case PromptGuideScript.GuideType.drawer3:
			if (DrawersScript.IsDrawerUnlocked(3))
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_DRAWER_OPEN") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			}
			else if (DrawersScript.IsInConditionToUnlock())
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_DRAWER_UNLOCK"), true);
			}
			else
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_DRAWER_LOCKED"), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.drawer3;
			break;
		case PromptGuideScript.GuideType.powerup:
			PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_POWERUP_INSPECT") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.powerup;
			break;
		case PromptGuideScript.GuideType.powerupTriggerHold:
			PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_HOLD_TRIGGERING_POWERUP") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			PromptGuideScript.instance.shouldEnableCallback = () => PowerupTriggerAnimController.HasAnimations();
			break;
		case PromptGuideScript.GuideType.store_speedyBox0:
		{
			BigInteger capsuleCost5 = StoreCapsuleScript.GetStoreCapsuleById(3).GetCapsuleCost();
			if (capsuleCost5 < 0L)
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_STORE_EMPTY"), true);
			}
			else if (num >= capsuleCost5)
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					Translation.Get("SCREEN_PROMPT_STORE_BUY"),
					" ",
					capsuleCost5.ToStringSmart(),
					" <sprite name=\"CloverTicket\"> ",
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0)
				}), true);
			}
			else
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					"<color=red>",
					Translation.Get("SCREEN_PROMPT_STORE_NOT_ENOUGH_TICKETS"),
					"</color> ",
					capsuleCost5.ToStringSmart(),
					" <sprite name=\"CloverTicket\">"
				}), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.store_speedyBox0;
			break;
		}
		case PromptGuideScript.GuideType.store_speedyBox1:
		{
			BigInteger capsuleCost6 = StoreCapsuleScript.GetStoreCapsuleById(4).GetCapsuleCost();
			if (capsuleCost6 < 0L)
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_STORE_EMPTY"), true);
			}
			else if (num >= capsuleCost6)
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					Translation.Get("SCREEN_PROMPT_STORE_BUY"),
					" ",
					capsuleCost6.ToStringSmart(),
					" <sprite name=\"CloverTicket\"> ",
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0)
				}), true);
			}
			else
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					"<color=red>",
					Translation.Get("SCREEN_PROMPT_STORE_NOT_ENOUGH_TICKETS"),
					"</color> ",
					capsuleCost6.ToStringSmart(),
					" <sprite name=\"CloverTicket\">"
				}), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.store_speedyBox1;
			break;
		}
		case PromptGuideScript.GuideType.store_speedyBox2:
		{
			BigInteger capsuleCost7 = StoreCapsuleScript.GetStoreCapsuleById(5).GetCapsuleCost();
			if (capsuleCost7 < 0L)
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_STORE_EMPTY"), true);
			}
			else if (num >= capsuleCost7)
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					Translation.Get("SCREEN_PROMPT_STORE_BUY"),
					" ",
					capsuleCost7.ToStringSmart(),
					" <sprite name=\"CloverTicket\"> ",
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0)
				}), true);
			}
			else
			{
				PromptGuideScript.SetText(string.Concat(new string[]
				{
					"<color=red>",
					Translation.Get("SCREEN_PROMPT_STORE_NOT_ENOUGH_TICKETS"),
					"</color> ",
					capsuleCost7.ToStringSmart(),
					" <sprite name=\"CloverTicket\">"
				}), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.store_speedyBox2;
			break;
		}
		case PromptGuideScript.GuideType.menuDrawer_Menu:
			PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_DRAWER_OPEN") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.menuDrawer_Menu;
			break;
		case PromptGuideScript.GuideType.menuDrawer_PowerupInfos:
			PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TEXT_DRAWER_OPEN") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.menuDrawer_PowerupInfos;
			break;
		case PromptGuideScript.GuideType.rewardBox:
		{
			bool flag3 = GameplayMaster.DeathCountdownHasStarted();
			if (RewardBoxScript.IsOpened())
			{
				if (RewardBoxScript.HasPrize())
				{
					PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_REWARD_BOX_PICK") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
				}
				else
				{
					PromptGuideScript.SetText("<color=red>" + Translation.Get("SCREEN_PROMPT_REWARD_BOX_EMPTY") + "</color>", true);
				}
			}
			else if (flag3)
			{
				PromptGuideScript.SetText("<color=red>" + Translation.Get("SCREEN_PROMPT_TEXT_NOT_NOW") + "</color>", true);
			}
			else
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_REWARD_BOX_CLOSED") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.rewardBox;
			break;
		}
		case PromptGuideScript.GuideType.terminal:
			if (GameplayMaster.DeathCountdownHasStarted())
			{
				PromptGuideScript.SetText("<color=red>" + Translation.Get("SCREEN_PROMPT_TEXT_NOT_NOW") + "</color>", true);
			}
			else
			{
				PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TERMINAL_LOG_IN") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			}
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.terminal;
			break;
		case PromptGuideScript.GuideType.pickupPhone:
			PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_PHONE_PICKUP") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.pickupPhone;
			break;
		case PromptGuideScript.GuideType.slot_RedButton:
		{
			int num2 = GameplayData.RedButtonActivationsMultiplierGet(true);
			string text3;
			if (!RedButtonScript.ButtonIsFlashing())
			{
				text3 = "<sprite name=\"RedButton\">" + Translation.Get("SCREEN_PROMPT_TEXT_SLOT_RED_BUTTON_ACTIVATE_NO_TRIGGERS") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0);
			}
			else if (num2 > 1)
			{
				text3 = string.Concat(new string[]
				{
					"<sprite name=\"RedButton\">",
					Translation.Get("SCREEN_PROMPT_TEXT_SLOT_RED_BUTTON_ACTIVATE"),
					" (X",
					num2.ToString(),
					") ",
					Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0)
				});
			}
			else
			{
				text3 = "<sprite name=\"RedButton\">" + Translation.Get("SCREEN_PROMPT_TEXT_SLOT_RED_BUTTON_ACTIVATE") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0);
			}
			PromptGuideScript.SetText(text3, true);
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && !SlotMachineScript.IsSpinning() && GameplayData.SpinsLeftGet() > 0 && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.slot_RedButton;
			break;
		}
		case PromptGuideScript.GuideType.toyPhone:
			PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TOY_PHONE_HEAR") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && !SlotMachineScript.IsSpinning() && GameplayData.SpinsLeftGet() > 0 && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.toyPhone;
			break;
		case PromptGuideScript.GuideType.magazineRead:
			PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_MAGAZINE_READ") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.magazineRead;
			break;
		case PromptGuideScript.GuideType.wcInspect:
			PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_WC_INSPECT") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.wcInspect;
			break;
		case PromptGuideScript.GuideType.deckBoxAltar:
			PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_DECKBOX_ALTAR") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && !SlotMachineScript.IsSpinning() && GameplayData.SpinsLeftGet() > 0 && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.deckBoxAltar;
			break;
		case PromptGuideScript.GuideType.twitchVote:
			PromptGuideScript.SetText(Translation.Get("SCREEN_PROMPT_TWITCH_POLL") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.interact, 0), true);
			PromptGuideScript.instance.shouldEnableCallback = () => !(DiegeticMenuController.ActiveMenu == null) && !(DiegeticMenuController.ActiveMenu.HoveredElement == null) && GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.preparation && DiegeticMenuController.ActiveMenu.HoveredElement.promptGuideType == PromptGuideScript.GuideType.twitchVote;
			break;
		default:
			Debug.LogError("PromptGuideScript: SetGuideType: type not implemented: " + type.ToString());
			break;
		}
		PromptGuideScript.instance.currentType = type;
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x00042C96 File Offset: 0x00040E96
	public static PromptGuideScript.GuideType GetGuideType()
	{
		if (PromptGuideScript.instance == null)
		{
			Debug.LogError("PromptGuideScript: GetGuideType: no instance");
			return PromptGuideScript.GuideType.slot_Play;
		}
		return PromptGuideScript.instance.currentType;
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x00042CBB File Offset: 0x00040EBB
	public static void ResetGuide()
	{
		PromptGuideScript.instance.currentType = PromptGuideScript.GuideType.Undefined;
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00042CC9 File Offset: 0x00040EC9
	public static void ForceClose(bool resetType)
	{
		if (resetType)
		{
			PromptGuideScript.ResetGuide();
		}
		PromptGuideScript.instance.holder.SetActive(false);
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x00042CE3 File Offset: 0x00040EE3
	private static void SetText(string textString, bool sanitizeString = true)
	{
		if (PromptGuideScript.instance == null)
		{
			return;
		}
		if (sanitizeString)
		{
			PromptGuideScript.instance.text.text = Strings.Sanitize(Strings.SantizationKind.ui, textString, Strings.SanitizationSubKind.none);
			return;
		}
		PromptGuideScript.instance.text.text = textString;
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x00042D1E File Offset: 0x00040F1E
	public static bool PreventDepositDuringFlashing()
	{
		return !(PromptGuideScript.instance == null) && PromptGuideScript.instance._textFlashPreventsDeposit;
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x00042D39 File Offset: 0x00040F39
	private void OnInputChange_ForceUpdate(Controls.InputActionMap map)
	{
		if (!PromptGuideScript.IsEnabled())
		{
			return;
		}
		if (this.currentType == PromptGuideScript.GuideType.Undefined || this.currentType == PromptGuideScript.GuideType.Count)
		{
			return;
		}
		PromptGuideScript.SetGuideType(this.currentType);
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x00042D63 File Offset: 0x00040F63
	private void Awake()
	{
		PromptGuideScript.instance = this;
		this.myRect = base.GetComponent<RectTransform>();
		this.imageStartingPos = this.textBackImage.rectTransform.anchoredPosition;
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x00042D8D File Offset: 0x00040F8D
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		PromptGuideScript.ForceClose(true);
		PlatformMaster.IsInitialized();
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x00042DA7 File Offset: 0x00040FA7
	private void OnDestroy()
	{
		if (PromptGuideScript.instance == this)
		{
			PromptGuideScript.instance = null;
		}
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		Controls.onLastInputKindChangedAny = (Controls.MapCallback)Delegate.Remove(Controls.onLastInputKindChangedAny, new Controls.MapCallback(this.OnInputChange_ForceUpdate));
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x00042DE4 File Offset: 0x00040FE4
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
		if (this.lastInputKind != this.player.lastInputKindUsed)
		{
			bool flag = false;
			if (this.lastInputKind == Controls.InputKind.Noone)
			{
				flag = true;
			}
			if (this.lastInputKind == Controls.InputKind.Keyboard && this.player.lastInputKindUsed == Controls.InputKind.Joystick)
			{
				flag = true;
			}
			if (this.lastInputKind == Controls.InputKind.Mouse && this.player.lastInputKindUsed == Controls.InputKind.Joystick)
			{
				flag = true;
			}
			if (this.lastInputKind == Controls.InputKind.Joystick && this.player.lastInputKindUsed == Controls.InputKind.Mouse)
			{
				flag = true;
			}
			if (this.lastInputKind == Controls.InputKind.Joystick && this.player.lastInputKindUsed == Controls.InputKind.Keyboard)
			{
				flag = true;
			}
			if (flag)
			{
				this.lastInputKind = this.player.lastInputKindUsed;
				this.OnInputChange_ForceUpdate(null);
			}
		}
		bool flag2 = false;
		if (this.shouldEnableCallback != null)
		{
			flag2 = this.shouldEnableCallback();
		}
		if (ATMScript.DebtClearCutsceneIsPlaying())
		{
			flag2 = false;
		}
		if (DialogueScript.IsEnabled())
		{
			flag2 = false;
		}
		if (ScreenMenuScript.IsEnabled())
		{
			flag2 = false;
		}
		if (gamePhase == GameplayMaster.GamePhase.death || gamePhase == GameplayMaster.GamePhase.closingGame || gamePhase == GameplayMaster.GamePhase.endingWithoutDeath)
		{
			flag2 = false;
		}
		if (DrawersScript.IsKeyAnimationPlaying())
		{
			flag2 = false;
		}
		if (PhoneScript.IsOn())
		{
			flag2 = false;
		}
		if (ToyPhoneUIScript.IsEnabled())
		{
			flag2 = false;
		}
		if (PowerupTriggerAnimController.IsShowingUnlockAnimation())
		{
			flag2 = false;
		}
		if (PowerupTriggerAnimController.IsShowingDiscardAnimation())
		{
			flag2 = false;
		}
		if (MagazineUiScript.IsEnabled())
		{
			flag2 = false;
		}
		if (WCScript.IsPerformingAction())
		{
			flag2 = false;
		}
		if (MemoryPackDealUI.IsDealRunnning())
		{
			flag2 = false;
		}
		if (DeckBoxUI.IsEnabled())
		{
			flag2 = false;
		}
		if (this.holder.activeSelf != flag2)
		{
			this.holder.SetActive(flag2);
		}
		global::UnityEngine.Vector2 zero = new global::UnityEngine.Vector2(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f));
		if (Data.settings.dyslexicFontEnabled)
		{
			zero = global::UnityEngine.Vector2.zero;
		}
		this.textBackImage.rectTransform.sizeDelta = new global::UnityEngine.Vector2(this.text.preferredWidth + 40f, this.text.preferredHeight + 20f);
		this.textBackImage.rectTransform.anchoredPosition = this.imageStartingPos + zero;
		this.holderRect.sizeDelta = this.textBackImage.rectTransform.sizeDelta;
		this.holderRect.anchoredPosition = new global::UnityEngine.Vector2(this.myRect.sizeDelta.x / 2f, this.myRect.anchoredPosition.y + 8f);
		if (this.currentType == PromptGuideScript.GuideType.atm_insertCoin)
		{
			BigInteger bigInteger = GameplayData.NextDepositAmmountGet(false);
			if (this.nextDepositOld != bigInteger)
			{
				if (this.nextDepositOld > bigInteger)
				{
					this._textFlashPreventsDeposit = true;
				}
				this.nextDepositOld = bigInteger;
				if (!this.flashingTimers.ContainsKey(PromptGuideScript.GuideType.atm_insertCoin))
				{
					this.flashingTimers.Add(PromptGuideScript.GuideType.atm_insertCoin, 1f);
				}
				else
				{
					this.flashingTimers[PromptGuideScript.GuideType.atm_insertCoin] = 1f;
				}
				DiegeticMenuController.ActiveMenu.SetDelay(0.35f);
			}
		}
		if (this.currentType == PromptGuideScript.GuideType.powerupTriggerHold && PowerupTriggerAnimController.HasAnimations())
		{
			if (!this.flashingTimers.ContainsKey(PromptGuideScript.GuideType.powerupTriggerHold))
			{
				this.flashingTimers.Add(PromptGuideScript.GuideType.powerupTriggerHold, 0.75f);
			}
			else
			{
				this.flashingTimers[PromptGuideScript.GuideType.powerupTriggerHold] = 0.75f;
			}
		}
		if ((this.currentType == PromptGuideScript.GuideType.drawer0 || this.currentType == PromptGuideScript.GuideType.drawer1 || this.currentType == PromptGuideScript.GuideType.drawer2 || this.currentType == PromptGuideScript.GuideType.drawer3) && DrawersScript.IsInConditionToUnlock())
		{
			if (!this.flashingTimers.ContainsKey(this.currentType))
			{
				this.flashingTimers.Add(this.currentType, 0.75f);
			}
			else
			{
				this.flashingTimers[this.currentType] = 0.75f;
			}
		}
		if (this.currentType == PromptGuideScript.GuideType.door_Lock && DoorScript.IsInConditionToUnlock())
		{
			if (!this.flashingTimers.ContainsKey(PromptGuideScript.GuideType.door_Lock))
			{
				this.flashingTimers.Add(PromptGuideScript.GuideType.door_Lock, 0.75f);
			}
			else
			{
				this.flashingTimers[PromptGuideScript.GuideType.door_Lock] = 0.75f;
			}
		}
		if (!flag2 && this.flashingTimers.ContainsKey(this.currentType) && this.flashingTimers[this.currentType] > 0f)
		{
			this.flashingTimers[this.currentType] = 0.75f;
		}
		float num = 0f;
		if (this.flashingTimers.ContainsKey(this.currentType))
		{
			num = this.flashingTimers[this.currentType];
			Dictionary<PromptGuideScript.GuideType, float> dictionary = this.flashingTimers;
			PromptGuideScript.GuideType guideType = this.currentType;
			dictionary[guideType] -= Time.deltaTime;
		}
		if (flag2 && num > 0f)
		{
			if (Util.AngleSin(Tick.PassedTime * 1440f) > 0f)
			{
				if (this.text.color != this.C_ORANGE)
				{
					this.text.color = this.C_ORANGE;
					return;
				}
			}
			else if (this.text.color != Color.white)
			{
				this.text.color = Color.white;
				return;
			}
		}
		else
		{
			if (this.text.color != Color.white)
			{
				this.text.color = Color.white;
			}
			this._textFlashPreventsDeposit = false;
		}
	}

	public static PromptGuideScript instance;

	public const int PLAYER_INDEX = 0;

	private const float FLASHING_TIMER_RESET_VISIBLE = 0.75f;

	private const float FLASHING_TIMER_RESET_VISIBLE_LONG = 1f;

	private Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	private Controls.PlayerExt player;

	private RectTransform myRect;

	public GameObject holder;

	public RectTransform holderRect;

	public TextMeshProUGUI text;

	public RawImage textBackImage;

	private PromptGuideScript.GuideType currentType = PromptGuideScript.GuideType.Undefined;

	private global::UnityEngine.Vector2 imageStartingPos;

	private Dictionary<PromptGuideScript.GuideType, float> flashingTimers = new Dictionary<PromptGuideScript.GuideType, float>();

	private bool _textFlashPreventsDeposit;

	private BigInteger nextDepositOld = -1;

	private Controls.InputKind lastInputKind;

	public PromptGuideScript.ShouldEnableCallback shouldEnableCallback;

	public enum GuideType
	{
		slot_Play,
		slot_Spin,
		slot_AutoSpin,
		slot_Exit,
		atm_insertCoin,
		atm_GetRevenue,
		door_Lock,
		store_box0,
		store_box1,
		store_box2,
		store_refresh,
		drawer0,
		drawer1,
		drawer2,
		drawer3,
		powerup,
		powerupTriggerHold,
		store_speedyBox0,
		store_speedyBox1,
		store_speedyBox2,
		menuDrawer_Menu,
		menuDrawer_PowerupInfos,
		rewardBox,
		terminal,
		pickupPhone,
		slot_RedButton,
		toyPhone,
		magazineRead,
		wcInspect,
		deckBoxAltar,
		twitchVote,
		Count,
		Undefined
	}

	// (Invoke) Token: 0x06001234 RID: 4660
	public delegate bool ShouldEnableCallback();
}
