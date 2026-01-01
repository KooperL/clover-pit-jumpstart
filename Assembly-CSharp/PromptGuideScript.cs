using System;
using System.Collections.Generic;
using System.Numerics;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E0 RID: 224
public class PromptGuideScript : MonoBehaviour
{
	// Token: 0x06000B5C RID: 2908 RVA: 0x0000F092 File Offset: 0x0000D292
	public static bool IsEnabled()
	{
		return !(PromptGuideScript.instance == null) && PromptGuideScript.instance.holder.activeSelf;
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x0005BA7C File Offset: 0x00059C7C
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

	// Token: 0x06000B5E RID: 2910 RVA: 0x0000F0B2 File Offset: 0x0000D2B2
	public static PromptGuideScript.GuideType GetGuideType()
	{
		if (PromptGuideScript.instance == null)
		{
			Debug.LogError("PromptGuideScript: GetGuideType: no instance");
			return PromptGuideScript.GuideType.slot_Play;
		}
		return PromptGuideScript.instance.currentType;
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x0000F0D7 File Offset: 0x0000D2D7
	public static void ResetGuide()
	{
		PromptGuideScript.instance.currentType = PromptGuideScript.GuideType.Undefined;
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x0000F0E5 File Offset: 0x0000D2E5
	public static void ForceClose(bool resetType)
	{
		if (resetType)
		{
			PromptGuideScript.ResetGuide();
		}
		PromptGuideScript.instance.holder.SetActive(false);
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x0000F0FF File Offset: 0x0000D2FF
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

	// Token: 0x06000B62 RID: 2914 RVA: 0x0000F13A File Offset: 0x0000D33A
	public static bool PreventDepositDuringFlashing()
	{
		return !(PromptGuideScript.instance == null) && PromptGuideScript.instance._textFlashPreventsDeposit;
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x0000F155 File Offset: 0x0000D355
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

	// Token: 0x06000B64 RID: 2916 RVA: 0x0000F17F File Offset: 0x0000D37F
	private void Awake()
	{
		PromptGuideScript.instance = this;
		this.myRect = base.GetComponent<RectTransform>();
		this.imageStartingPos = this.textBackImage.rectTransform.anchoredPosition;
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x0000F1A9 File Offset: 0x0000D3A9
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		PromptGuideScript.ForceClose(true);
		PlatformMaster.IsInitialized();
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x0000F1C3 File Offset: 0x0000D3C3
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

	// Token: 0x06000B67 RID: 2919 RVA: 0x0005CDCC File Offset: 0x0005AFCC
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

	// Token: 0x04000BDC RID: 3036
	public static PromptGuideScript instance;

	// Token: 0x04000BDD RID: 3037
	public const int PLAYER_INDEX = 0;

	// Token: 0x04000BDE RID: 3038
	private const float FLASHING_TIMER_RESET_VISIBLE = 0.75f;

	// Token: 0x04000BDF RID: 3039
	private const float FLASHING_TIMER_RESET_VISIBLE_LONG = 1f;

	// Token: 0x04000BE0 RID: 3040
	private Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x04000BE1 RID: 3041
	private Controls.PlayerExt player;

	// Token: 0x04000BE2 RID: 3042
	private RectTransform myRect;

	// Token: 0x04000BE3 RID: 3043
	public GameObject holder;

	// Token: 0x04000BE4 RID: 3044
	public RectTransform holderRect;

	// Token: 0x04000BE5 RID: 3045
	public TextMeshProUGUI text;

	// Token: 0x04000BE6 RID: 3046
	public RawImage textBackImage;

	// Token: 0x04000BE7 RID: 3047
	private PromptGuideScript.GuideType currentType = PromptGuideScript.GuideType.Undefined;

	// Token: 0x04000BE8 RID: 3048
	private global::UnityEngine.Vector2 imageStartingPos;

	// Token: 0x04000BE9 RID: 3049
	private Dictionary<PromptGuideScript.GuideType, float> flashingTimers = new Dictionary<PromptGuideScript.GuideType, float>();

	// Token: 0x04000BEA RID: 3050
	private bool _textFlashPreventsDeposit;

	// Token: 0x04000BEB RID: 3051
	private BigInteger nextDepositOld = -1;

	// Token: 0x04000BEC RID: 3052
	private Controls.InputKind lastInputKind;

	// Token: 0x04000BED RID: 3053
	public PromptGuideScript.ShouldEnableCallback shouldEnableCallback;

	// Token: 0x020000E1 RID: 225
	public enum GuideType
	{
		// Token: 0x04000BEF RID: 3055
		slot_Play,
		// Token: 0x04000BF0 RID: 3056
		slot_Spin,
		// Token: 0x04000BF1 RID: 3057
		slot_AutoSpin,
		// Token: 0x04000BF2 RID: 3058
		slot_Exit,
		// Token: 0x04000BF3 RID: 3059
		atm_insertCoin,
		// Token: 0x04000BF4 RID: 3060
		atm_GetRevenue,
		// Token: 0x04000BF5 RID: 3061
		door_Lock,
		// Token: 0x04000BF6 RID: 3062
		store_box0,
		// Token: 0x04000BF7 RID: 3063
		store_box1,
		// Token: 0x04000BF8 RID: 3064
		store_box2,
		// Token: 0x04000BF9 RID: 3065
		store_refresh,
		// Token: 0x04000BFA RID: 3066
		drawer0,
		// Token: 0x04000BFB RID: 3067
		drawer1,
		// Token: 0x04000BFC RID: 3068
		drawer2,
		// Token: 0x04000BFD RID: 3069
		drawer3,
		// Token: 0x04000BFE RID: 3070
		powerup,
		// Token: 0x04000BFF RID: 3071
		powerupTriggerHold,
		// Token: 0x04000C00 RID: 3072
		store_speedyBox0,
		// Token: 0x04000C01 RID: 3073
		store_speedyBox1,
		// Token: 0x04000C02 RID: 3074
		store_speedyBox2,
		// Token: 0x04000C03 RID: 3075
		menuDrawer_Menu,
		// Token: 0x04000C04 RID: 3076
		menuDrawer_PowerupInfos,
		// Token: 0x04000C05 RID: 3077
		rewardBox,
		// Token: 0x04000C06 RID: 3078
		terminal,
		// Token: 0x04000C07 RID: 3079
		pickupPhone,
		// Token: 0x04000C08 RID: 3080
		slot_RedButton,
		// Token: 0x04000C09 RID: 3081
		toyPhone,
		// Token: 0x04000C0A RID: 3082
		magazineRead,
		// Token: 0x04000C0B RID: 3083
		wcInspect,
		// Token: 0x04000C0C RID: 3084
		deckBoxAltar,
		// Token: 0x04000C0D RID: 3085
		twitchVote,
		// Token: 0x04000C0E RID: 3086
		Count,
		// Token: 0x04000C0F RID: 3087
		Undefined
	}

	// Token: 0x020000E2 RID: 226
	// (Invoke) Token: 0x06000B6A RID: 2922
	public delegate bool ShouldEnableCallback();
}
