using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Panik;
using UnityEngine;

public class Strings : MonoBehaviour
{
	// Token: 0x0600037D RID: 893 RVA: 0x00015AF4 File Offset: 0x00013CF4
	public static string GetPowerupRarity_SpriteString(PowerupScript.PublicRarity pubRarity)
	{
		switch (pubRarity)
		{
		case PowerupScript.PublicRarity.common:
			return "<sprite name=\"Rarity_Mild\">";
		case PowerupScript.PublicRarity.uncommon:
			return "<sprite name=\"Rarity_Spicy\">";
		case PowerupScript.PublicRarity.rare:
			return "<sprite name=\"Rarity_Hot\">";
		case PowerupScript.PublicRarity.epic:
			return "<sprite name=\"Rarity_Hell\">";
		default:
			Debug.LogError("GetPowerupRarityString() - Unknown rarity for rarity: " + pubRarity.ToString());
			return "<sprite name=\"Rarity_Mild\">";
		}
	}

	// Token: 0x0600037E RID: 894 RVA: 0x00015B52 File Offset: 0x00013D52
	public static string GetPowerupRarity_SpriteString(PowerupScript powerup)
	{
		return Strings.GetPowerupRarity_SpriteString(powerup.RarityPublicGet());
	}

	// Token: 0x0600037F RID: 895 RVA: 0x00015B60 File Offset: 0x00013D60
	public static string GetPowerupRarity_StringKey(PowerupScript.PublicRarity pubRarity)
	{
		switch (pubRarity)
		{
		case PowerupScript.PublicRarity.common:
			return Translation.Get("RARITY_POSTER_LEVEL_0");
		case PowerupScript.PublicRarity.uncommon:
			return Translation.Get("RARITY_POSTER_LEVEL_1");
		case PowerupScript.PublicRarity.rare:
			return Translation.Get("RARITY_POSTER_LEVEL_2");
		case PowerupScript.PublicRarity.epic:
			return Translation.Get("RARITY_POSTER_LEVEL_3");
		default:
			Debug.LogError("GetPowerupRarityString() - Unknown rarity: " + pubRarity.ToString());
			return Translation.Get("RARITY_POSTER_LEVEL_0");
		}
	}

	// Token: 0x06000380 RID: 896 RVA: 0x00015BD7 File Offset: 0x00013DD7
	public static string GetPowerupRarity_StringKey(PowerupScript powerup)
	{
		return Strings.GetPowerupRarity_StringKey(powerup.RarityPublicGet());
	}

	// Token: 0x06000381 RID: 897 RVA: 0x00015BE4 File Offset: 0x00013DE4
	public static string GetSpriteString_SlotMachineSymbol(SymbolScript.Kind kind)
	{
		switch (kind)
		{
		case SymbolScript.Kind.lemon:
			return "<sprite name=\"S_Lemon\">";
		case SymbolScript.Kind.cherry:
			return "<sprite name=\"S_Cherry\">";
		case SymbolScript.Kind.clover:
			return "<sprite name=\"S_Clover\">";
		case SymbolScript.Kind.bell:
			return "<sprite name=\"S_Bell\">";
		case SymbolScript.Kind.diamond:
			return "<sprite name=\"S_Diamond\">";
		case SymbolScript.Kind.coins:
			return "<sprite name=\"S_Coins\">";
		case SymbolScript.Kind.seven:
			return "<sprite name=\"S_Seven\">";
		default:
			Debug.LogError("GetSpriteString_SlotMachineSymbol() - Unknown symbol kind: " + kind.ToString());
			return "!";
		}
	}

	// Token: 0x06000382 RID: 898 RVA: 0x00015C60 File Offset: 0x00013E60
	public static string GetSpriteString_SlotMachinePattern(PatternScript.Kind kind)
	{
		switch (kind)
		{
		case PatternScript.Kind.jackpot:
			return "<sprite name=\"PtJ\">";
		case PatternScript.Kind.horizontal2:
			return "<sprite name=\"Pt2H\">";
		case PatternScript.Kind.horizontal3:
			return "<sprite name=\"Pt3H\">";
		case PatternScript.Kind.horizontal4:
			return "<sprite name=\"Pt4H\">";
		case PatternScript.Kind.horizontal5:
			return "<sprite name=\"Pt5H\">";
		case PatternScript.Kind.vertical2:
			return "<sprite name=\"Pt2V\">";
		case PatternScript.Kind.vertical3:
			return "<sprite name=\"Pt3V\">";
		case PatternScript.Kind.diagonal2:
			return "<sprite name=\"Pt2D\">";
		case PatternScript.Kind.diagonal3:
			return "<sprite name=\"Pt3D\">";
		case PatternScript.Kind.pyramid:
			return "<sprite name=\"PtP\">";
		case PatternScript.Kind.pyramidInverted:
			return "<sprite name=\"PtPU\">";
		case PatternScript.Kind.triangle:
			return "<sprite name=\"PtT\">";
		case PatternScript.Kind.triangleInverted:
			return "<sprite name=\"PtTU\">";
		case PatternScript.Kind.snakeUpDown:
			return "<sprite name=\"PtSnkUpDown\">";
		case PatternScript.Kind.snakeDownUp:
			return "<sprite name=\"PtSnkDownUp\">";
		case PatternScript.Kind.eye:
			return "<sprite name=\"PtEye\">";
		default:
			Debug.LogError("GetSpriteString_SlotMachinePattern() - Unknown pattern kind: " + kind.ToString());
			return "!";
		}
	}

	// Token: 0x06000383 RID: 899 RVA: 0x00015D36 File Offset: 0x00013F36
	public static void SetTemporaryInspectedPowerup(PowerupScript p)
	{
		Strings.temporaryInspectedPowerup = p;
	}

	// Token: 0x06000384 RID: 900 RVA: 0x00015D3E File Offset: 0x00013F3E
	public static void SetTemporaryFlag_Sanitize666And999(int occurrencies)
	{
		Strings.sanitize666999_Counter = occurrencies;
	}

	// Token: 0x06000385 RID: 901 RVA: 0x00015D48 File Offset: 0x00013F48
	public static string Sanitize(Strings.SantizationKind santizationKind, string input, Strings.SanitizationSubKind subKind = Strings.SanitizationSubKind.none)
	{
		StringBuilder stringBuilder = new StringBuilder(input, input.Length * 2);
		BigInteger bigInteger = GameplayData.RoundsLeftToDeadline();
		BigInteger bigInteger2 = GameplayData.DebtIndexGet();
		GameplayData.DepositGet();
		GameplayData instance = GameplayData.Instance;
		Controls.PlayerExt playerByIndex = Controls.GetPlayerByIndex(0);
		RunModifierScript.Identifier identifier = GameplayData.RunModifier_GetCurrent();
		if (santizationKind == Strings.SantizationKind.all || santizationKind == Strings.SantizationKind.ui || santizationKind == Strings.SantizationKind.uiAndMenus)
		{
			stringBuilder.Replace("[LANGUAGE]", Translation.Get("YOUR_LANGUAGE_TRANSLATED"));
			stringBuilder.Replace("[START_PROMPT]", Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.menuSelect, 0));
			if (playerByIndex.lastInputKindUsed == Controls.InputKind.Joystick)
			{
				stringBuilder.Replace("[INPUTS_MOVE]", Controls.MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(0, Controls.InputAction.moveLeft, 0) + Controls.MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(0, Controls.InputAction.moveUp, 0));
			}
			else
			{
				stringBuilder.Replace("[INPUTS_MOVE]", Controls.MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(0, Controls.InputAction.moveUp, 0) + Controls.MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(0, Controls.InputAction.moveLeft, 0) + Controls.MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(0, Controls.InputAction.moveDown, 0) + Controls.MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(0, Controls.InputAction.moveRight, 0));
			}
			PowerupScript powerup_Quick = PowerupScript.GetPowerup_Quick(Data.GameData._terminalNotificationStringPowerup);
			if (powerup_Quick != null)
			{
				stringBuilder.Replace("[NOTIF_POWERUP_UNLOCKED]", "<color=yellow>" + powerup_Quick.NameGet(false, true) + "</color>");
			}
			stringBuilder.Replace("[ROUNDS_LEFT_N]", bigInteger.ToStringSmart());
			stringBuilder.Replace("$", "<sprite name=\"CoinSymbolWhite64\">");
			stringBuilder.Replace("€", "<sprite name=\"CoinSymbolOrange64\">");
			stringBuilder.Replace("@", "<sprite name=\"SpinSymbol32\">");
			stringBuilder.Replace("#", "<sprite name=\"Dice\">");
			stringBuilder.Replace("[C_TICKET]", "<sprite name=\"CloverTicket\">");
			stringBuilder.Replace("[C_TICKETS_X_ROUND]", GameplayData.CloverTickets_BonusRoundsLeft_Get().ToString());
			stringBuilder.Replace("[SKULL]", "<sprite name=\"SkullSymbolOrange64\">");
			stringBuilder.Replace("[BIG_EYE]", "<sprite name=\"BigEye\">");
			stringBuilder.Replace("[DEBT]", GameplayData.DebtGet().ToStringSmart() + " <sprite name=\"CoinSymbolOrange32\">");
			stringBuilder.Replace("[DEBT_NEXT]", GameplayData.DebtGetExt(bigInteger2 + 1).ToStringSmart() + " <sprite name=\"CoinSymbolOrange32\">");
			stringBuilder.Replace("[K_S_MULT]", "<color=yellow>" + Translation.Get("POWERUP_KEYWORD_SYMBOLS_MULTIPLIER") + "</color>");
			stringBuilder.Replace("[K_P_MULT]", "<color=orange>" + Translation.Get("POWERUP_KEYWORD_PATTERN_MULTIPLIER") + "</color>");
			stringBuilder.Replace("[MEM_PACK_N]", GameplayData.RunModifier_BonusPacksThisTime_Get().ToString());
			if (PlatformMaster.IsInitialized())
			{
				stringBuilder.Replace("[REWARD_DEBT_AMMOUNT]", GameplayData.GetRewardDeadlineDebt().ToStringSmart() + "<sprite name=\"CoinSymbolOrange64\">");
			}
		}
		if (santizationKind == Strings.SantizationKind.all || santizationKind == Strings.SantizationKind.powerupKeywords)
		{
			BigInteger bigInteger3 = GameplayData.InterestEarnedHypotetically();
			if (Strings.temporaryInspectedPowerup == null)
			{
				Strings.temporaryInspectedPowerup = InspectorScript.CurrentlyInspectedPowerupGet();
			}
			PowerupScript powerupScript = Strings.temporaryInspectedPowerup;
			Strings.temporaryInspectedPowerup = null;
			if (TerminalScript.IsLoggedIn() && TerminalScript.HoveredPowerupGet() != null)
			{
				powerupScript = TerminalScript.HoveredPowerupGet();
			}
			string text = ((!TerminalScript.IsLoggedIn()) ? "<size=14>" : "<size=0.015>");
			string text2 = ((!TerminalScript.IsLoggedIn()) ? "</size>" : "<size=0.015>");
			BigInteger bigInteger4 = GameplayData.DebtGet();
			switch (subKind)
			{
			case Strings.SanitizationSubKind.powerup_ShowAllSymbolsAndPatternsValues:
				Strings.SymbolsAndPatternsReplaceAll(stringBuilder);
				break;
			case Strings.SanitizationSubKind.powerup_ShowPatternsValues:
				Strings.PatternReplaceTagAll(stringBuilder);
				Strings.PatternReplaceBaseValueAll(stringBuilder);
				break;
			case Strings.SanitizationSubKind.powerup_Skeleton:
				stringBuilder.Replace("[K_SK_DEBT_INCREASE]", (PowerupScript.SkeletonPiecesDebtIncreasePercentage() - 100).ToString());
				stringBuilder.Replace("[K_ONTO_SLOT]", "<color=yellow>" + Translation.Get("POWERUP_KEYWORD_GO_ONTO_SLOT") + "</color>");
				stringBuilder.Replace("[K_CANNOT_DISCARD]", "<color=yellow>" + Translation.Get("POWERUP_KEYWORD_CANNOT_DISCARD") + "</color>");
				break;
			case Strings.SanitizationSubKind.powerup_GrandmasPurse:
				stringBuilder.Replace("[GR_PURSE_INT]", PowerupScript.GrandmasPurse_ExtraInterestGet(false).ToString());
				break;
			case Strings.SanitizationSubKind.powerup_Hole:
			{
				PowerupScript.Identifier identifier2 = GameplayData.PowerupHoleCircle_CharmGet();
				PowerupScript.Identifier identifier3 = GameplayData.PowerupHoleRomboid_CharmGet();
				AbilityScript.Identifier identifier4 = GameplayData.PowerupHoleCross_AbilityGet();
				stringBuilder.Replace("[K_HOLE_CIRCLE_CHARM]", (identifier2 == PowerupScript.Identifier.undefined) ? "<color=red>NULL</color>" : ("<color=yellow>" + PowerupScript.GetPowerup_Quick(identifier2).NameGet(false, false) + "</color>"));
				stringBuilder.Replace("[K_HOLE_ROMBOID_CHARM]", (identifier3 == PowerupScript.Identifier.undefined) ? "<color=red>NULL</color>" : ("<color=yellow>" + PowerupScript.GetPowerup_Quick(identifier3).NameGet(false, false) + "</color>"));
				stringBuilder.Replace("[K_HOLE_CROSS_CHARM]", (identifier4 == AbilityScript.Identifier.undefined) ? "<color=red>NULL</color>" : ("<color=yellow>" + AbilityScript.AbilityGet(identifier4).NameGetTranslated() + "</color>"));
				break;
			}
			case Strings.SanitizationSubKind.powerup_Hourglass:
			{
				int num = PowerupScript.Hourglass_SymbolsMultiplierBonusGet(false);
				int num2 = PowerupScript.Hourglass_PatternsMultiplierBonusGet(false);
				stringBuilder.Replace("[K_HOURGLASS_S]", num.ToString());
				stringBuilder.Replace("[K_HOURGLASS_P]", num2.ToString());
				stringBuilder.Replace("[K_HOURGLASS_WHICH]", (num > num2) ? "[K_P_MULT]" : "[K_S_MULT]");
				break;
			}
			case Strings.SanitizationSubKind.powerup_SpicyPeppers:
				stringBuilder.Replace("[RP_ACTIVATIONS]", PowerupScript.RedPepper_ActivationsLeft().ToString());
				stringBuilder.Replace("[GP_ACTIVATIONS]", PowerupScript.GreenPepper_ActivationsLeft().ToString());
				stringBuilder.Replace("[GOLDE_P_LUCK]", GameplayData.Powerup_GoldenPepper_LuckBonusGet().ToString());
				stringBuilder.Replace("[ROTT_P_LUCK]", GameplayData.Powerup_RottenPepper_LuckBonusGet().ToString());
				stringBuilder.Replace("[BELL_P_LUCK]", GameplayData.Powerup_BellPepper_LuckBonusGet().ToString());
				break;
			case Strings.SanitizationSubKind.powerup_Baphomet:
				Strings.PatternReplaceTag(stringBuilder, PatternScript.Kind.triangle);
				Strings.PatternReplaceTag(stringBuilder, PatternScript.Kind.triangleInverted);
				break;
			case Strings.SanitizationSubKind.powerup_SymbolModifier:
				stringBuilder.Replace("[SMOD_IREWARD_EXPLANATION]", string.Concat(new string[]
				{
					text,
					"<color=grey>",
					Translation.Get("POWERUP_EXPLANATION_SYMBOL_MODIFIER_INSTANT_REWARD"),
					"</color>",
					text2
				}));
				stringBuilder.Replace("[SMOD_TICKET_EXPLANATION]", string.Concat(new string[]
				{
					text,
					"<color=grey>",
					Translation.Get("POWERUP_EXPLANATION_SYMBOL_MODIFIER_TICKET"),
					"</color>",
					text2
				}));
				stringBuilder.Replace("[SMOD_GOLD_EXPLANATION]", string.Concat(new string[]
				{
					text,
					"<color=grey>",
					Translation.Get("POWERUP_EXPLANATION_SYMBOL_MODIFIER_GOLDEN"),
					"</color>",
					text2
				}));
				stringBuilder.Replace("[SMOD_GOLD_EXPLANATION_ALL]", string.Concat(new string[]
				{
					text,
					"<color=grey>",
					Translation.Get("POWERUP_EXPLANATION_SYMBOL_MODIFIER_GOLDEN_ALL"),
					"</color>",
					text2
				}));
				stringBuilder.Replace("[SMOD_REPETITION_EXPLANATION]", string.Concat(new string[]
				{
					text,
					"<color=grey>",
					Translation.Get("POWERUP_EXPLANATION_SYMBOL_MODIFIER_REPETITION"),
					"</color>",
					text2
				}));
				stringBuilder.Replace("[SMOD_BATTERY_EXPLANATION]", string.Concat(new string[]
				{
					text,
					"<color=grey>",
					Translation.Get("POWERUP_EXPLANATION_SYMBOL_MODIFIER_BATTERY"),
					"</color>",
					text2
				}));
				stringBuilder.Replace("[SMOD_CHAIN_EXPLANATION]", string.Concat(new string[]
				{
					text,
					"<color=grey>",
					Translation.Get("POWERUP_EXPLANATION_SYMBOL_MODIFIER_CHAIN"),
					"</color>",
					text2
				}));
				stringBuilder.Replace("[SMOD_REVENUE]", "<sprite name=\"ModInstantReward\">");
				stringBuilder.Replace("[SMOD_REVENUE_REWARD]", SymbolScript.ModifierInstantReward_GetAmmount().ToStringSmart() + "<sprite name=\"CoinSymbolOrange32\">");
				stringBuilder.Replace("[SMOD_TICKET]", "<sprite name=\"ModTicket\">");
				stringBuilder.Replace("[SMOD_GOLD]", "<sprite name=\"ModGold\">");
				stringBuilder.Replace("[SMOD_REPETITION]", "<sprite name=\"ModRepetition\">");
				stringBuilder.Replace("[SMOD_BATTERY]", "<sprite name=\"ModBattery\">");
				stringBuilder.Replace("[SMOD_CHAIN]", "<sprite name=\"ModChain\">");
				Strings.SymbolsAndPatternsReplaceAll(stringBuilder);
				stringBuilder.Replace("[MIDA_%]", Mathf.RoundToInt(PowerupScript.GoldenKingMida_GetModChance(false) * 100f).ToString("D"));
				stringBuilder.Replace("[DEALER_%]", Mathf.RoundToInt(PowerupScript.Dealer_GetModChance(false) * 100f).ToString("D"));
				stringBuilder.Replace("[CAPIT_%]", Mathf.RoundToInt(PowerupScript.Capitalist_GetModChance(false) * 100f).ToString("D"));
				stringBuilder.Replace("[PTRAINER_LEFT]", Mathf.RoundToInt(PowerupScript.PuppetPersonalTrainer_BonusGet(false) * 100f).ToString("D"));
				stringBuilder.Replace("[ELETRCN_LEFT]", Mathf.RoundToInt(PowerupScript.PuppetEletrician_BonusGet(false) * 100f).ToString("D"));
				stringBuilder.Replace("[FTELLER_LEFT]", Mathf.RoundToInt(PowerupScript.PuppetFortuneTeller_BonusGet(false) * 100f).ToString("D"));
				Strings.PatternReplaceWith_SymbolsAndBValue(stringBuilder, "[ALL_SYMBOLS_W_VALUE]", ",", GameplayData.SymbolsAvailable_GetAll(false));
				break;
			}
			if (powerupScript != null)
			{
				PowerupScript.Identifier identifier5 = powerupScript.identifier;
				if (identifier5 <= PowerupScript.Identifier.Cross)
				{
					if (identifier5 <= PowerupScript.Identifier.SevenSinsStone)
					{
						switch (identifier5)
						{
						case PowerupScript.Identifier.LuckyCat:
							stringBuilder.Replace("[K_LUCKY_CAT_REVENUE]", PowerupScript._LuckyCatBonusCoinsGet().ToStringSmart() + "<sprite name=\"CoinSymbolOrange32\">");
							goto IL_1030;
						case PowerupScript.Identifier.LuckyCatFat:
							stringBuilder.Replace("[K_LUCKY_CAT_FAT_REVENUE]", PowerupScript._LuckyCatFatBonusCoinsGet().ToStringSmart() + "<sprite name=\"CoinSymbolOrange32\">");
							goto IL_1030;
						case PowerupScript.Identifier.LuckyCatSwole:
							stringBuilder.Replace("[K_LUCKY_CAT_GYM_REVENUE]", PowerupScript._LuckyCatSwoleBonusCoinsGet().ToStringSmart() + "<sprite name=\"CoinSymbolOrange32\">");
							goto IL_1030;
						case PowerupScript.Identifier.OneTrickPony:
						case PowerupScript.Identifier.RedCrystal:
						case PowerupScript.Identifier.TarotDeck:
							goto IL_1030;
						case PowerupScript.Identifier.PoopBeetle:
							stringBuilder.Replace("[POOP_BEETLE_BONUS]", "<color=yellow>" + GameplayData.Powerup_PoopBeetle_SymbolsIncreaseN_Get().ToString() + "</color>X " + Strings.SymbolsWithBaseValue_GetString(false));
							goto IL_1030;
						default:
							switch (identifier5)
							{
							case PowerupScript.Identifier.PissJar:
							case PowerupScript.Identifier.PoopJar:
								stringBuilder.Replace("[YELLOW_SYMBOLS]", Strings.Symbols_GetString_YellowOnes());
								stringBuilder.Replace("[NON_YELLOW_SYMBOLS]", Strings.Symbols_GetString_NonYellowOnes());
								goto IL_1030;
							case PowerupScript.Identifier.Painkillers:
							case PowerupScript.Identifier.Wolf:
							case PowerupScript.Identifier.FortuneCookie:
							case PowerupScript.Identifier.FideltyCard:
							case PowerupScript.Identifier.Sardines:
							case PowerupScript.Identifier.BrokenCalculator:
							case PowerupScript.Identifier.TheCollector:
							case PowerupScript.Identifier.WeirdClock:
							case PowerupScript.Identifier.MusicTape:
							case PowerupScript.Identifier.CrowBar:
							case PowerupScript.Identifier.CardboardHouse:
							case PowerupScript.Identifier.Cigarettes:
							case PowerupScript.Identifier.ElectricityCounter:
							case PowerupScript.Identifier.PotatoPower:
							case PowerupScript.Identifier.FakeCoin:
							case PowerupScript.Identifier.AncientCoin:
							case PowerupScript.Identifier.CatTreats:
							case PowerupScript.Identifier.Depression:
							case PowerupScript.Identifier.ToyTrain:
							case PowerupScript.Identifier.CloverVoucher:
							case PowerupScript.Identifier.CloversLandPatch:
							case PowerupScript.Identifier.VineSoupShroom:
							case PowerupScript.Identifier.GiantShroom:
							case PowerupScript.Identifier.Hole_Circle:
							case PowerupScript.Identifier.Hole_Romboid:
							case PowerupScript.Identifier.Hole_Cross:
							case PowerupScript.Identifier.Rorschach:
							case PowerupScript.Identifier.Hourglass:
								goto IL_1030;
							case PowerupScript.Identifier.Wallet:
								stringBuilder.Replace("[WALLET_BONUS]", PowerupScript.Wallet_PatternsMultiplierBonus(false).ToString());
								goto IL_1030;
							case PowerupScript.Identifier.MoneyBriefCase:
								stringBuilder.Replace("[DEBT_30%]", (bigInteger4 * 30 / 100).ToStringSmart());
								goto IL_1030;
							case PowerupScript.Identifier.Calendar:
								stringBuilder.Replace("[CALENDAR_BONUS]", "+" + GameplayData.Powerup_Calendar_SymbolsIncreaseN_Get().ToStringSmart());
								goto IL_1030;
							case PowerupScript.Identifier.YellowStar:
								Strings.PatternReplaceTag(stringBuilder, PatternScript.Kind.jackpot);
								goto IL_1030;
							case PowerupScript.Identifier.FortuneChanneler:
							{
								int num3 = GameplayData.Powerup_ChannelerOfFortune_ActivationsCounterGet();
								num3 = Mathf.Min(num3, 5);
								stringBuilder.Replace("[CHNLR_FRTN_N]", (5 - num3).ToString());
								goto IL_1030;
							}
							case PowerupScript.Identifier.Nose:
								Strings.PatternReplaceTag(stringBuilder, PatternScript.Kind.triangle);
								Strings.PatternReplaceTag(stringBuilder, PatternScript.Kind.triangleInverted);
								goto IL_1030;
							case PowerupScript.Identifier.EyeJar:
								Strings.PatternReplaceTag(stringBuilder, PatternScript.Kind.eye);
								goto IL_1030;
							case PowerupScript.Identifier.VoiceMailTape:
								stringBuilder.Replace("[VOICEM_N]", PowerupScript.VoiceMail_MultiplierBonusGet(false).ToString());
								goto IL_1030;
							case PowerupScript.Identifier.Garbage:
								stringBuilder.Replace("[GARBAGE_N]", PowerupScript.Garbage_MultiplierBonusGet(false).ToString());
								goto IL_1030;
							case PowerupScript.Identifier.AllIn:
								stringBuilder.Replace("[ALL_IN_N]", PowerupScript.AllIn_MultiplierBonusGet(false).ToString());
								Strings.PatternReplaceTag(stringBuilder, PatternScript.Kind.jackpot);
								goto IL_1030;
							case PowerupScript.Identifier.RingBell:
								Strings.SymbolsAndPatternsReplaceAll(stringBuilder);
								goto IL_1030;
							case PowerupScript.Identifier.ConsolationPrize:
								Strings.SymbolsAndPatternsReplaceAll(stringBuilder);
								goto IL_1030;
							case PowerupScript.Identifier.DarkLotus:
								stringBuilder.Replace("[DRK_LTS_VAL]", PowerupScript.DarkLotus_MultiplierBonus_Get(false).ToString());
								goto IL_1030;
							case PowerupScript.Identifier.StepsCounter:
								stringBuilder.Replace("[STP_CNTR_T]", PowerupScript.StepsCounter_TriggersNeededGet().ToString());
								goto IL_1030;
							case PowerupScript.Identifier.ShoppingCart:
								stringBuilder.Replace("[SHPCRT_N]", PowerupScript.ShoppingCart_MultiplierBonusGet(false).ToString());
								goto IL_1030;
							case PowerupScript.Identifier.DiscA:
								stringBuilder.Replace("[DISC_A_SP]", PowerupScript.DiscA_MissingSpinsGet().ToString());
								goto IL_1030;
							case PowerupScript.Identifier.DiscB:
								stringBuilder.Replace("[DISC_B_SP]", PowerupScript.DiscB_MissingSpinsGet().ToString());
								goto IL_1030;
							case PowerupScript.Identifier.DiscC:
								stringBuilder.Replace("[DISC_C_SP]", PowerupScript.DiscC_MissingSpinsGet().ToString());
								Strings.PatternReplaceTag(stringBuilder, PatternScript.Kind.pyramid);
								Strings.PatternReplaceTag(stringBuilder, PatternScript.Kind.pyramidInverted);
								goto IL_1030;
							case PowerupScript.Identifier.LocomotiveSteam:
								Strings.SymbolsAndPatternsReplaceAll(stringBuilder);
								goto IL_1030;
							case PowerupScript.Identifier.LocomotiveDiesel:
								Strings.SymbolsAndPatternsReplaceAll(stringBuilder);
								goto IL_1030;
							case PowerupScript.Identifier.CloverPot:
								stringBuilder.Replace("[CLOVERPOT_BNS]", PowerupScript.CloverPotTicketsBonus(false, false).ToString());
								goto IL_1030;
							case PowerupScript.Identifier.CloverPet:
								stringBuilder.Replace("[CLOVERPET_BONUS]", PowerupScript.CloverPetSymbolsMultiplierBonus(false).ToString());
								goto IL_1030;
							case PowerupScript.Identifier.Mushrooms:
								stringBuilder.Replace("[SHROOMS_BONUS]", "<color=yellow>" + PowerupScript.ShroomsRawBonusGet().ToString() + "</color>X " + Strings.SymbolsWithBaseValue_GetString(false));
								goto IL_1030;
							case PowerupScript.Identifier.AbstractPainting:
								Strings.SymbolsAndPatternsReplaceAll(stringBuilder);
								goto IL_1030;
							case PowerupScript.Identifier.Pareidolia:
								Strings.PatternReplaceTag(stringBuilder, PatternScript.Kind.eye);
								Strings.SymbolsAndPatternsReplaceAll(stringBuilder);
								goto IL_1030;
							case PowerupScript.Identifier.FruitBasket:
								stringBuilder.Replace("[FR_BSKT_ROUNDS_LEFT]", GameplayData.Powerup_FruitsBasket_RoundsLeftGet().ToString());
								goto IL_1030;
							case PowerupScript.Identifier.SevenSinsStone:
								break;
							default:
								goto IL_1030;
							}
							break;
						}
					}
					else
					{
						switch (identifier5)
						{
						case PowerupScript.Identifier.RottenPepper:
							Strings.PatternReplaceTag(stringBuilder, PatternScript.Kind.jackpot);
							goto IL_1030;
						case PowerupScript.Identifier.BellPepper:
							Strings.PatternReplaceTag(stringBuilder, PatternScript.Kind.jackpot);
							goto IL_1030;
						case PowerupScript.Identifier.GoldenPepper:
							Strings.PatternReplaceTag(stringBuilder, PatternScript.Kind.jackpot);
							goto IL_1030;
						default:
							if (identifier5 != PowerupScript.Identifier.Cross)
							{
								goto IL_1030;
							}
							stringBuilder.Replace("[CROSS_MULT]", PowerupScript.Cross_PatternsMultiplierBonus_Get(false).ToString());
							goto IL_1030;
						}
					}
				}
				else if (identifier5 <= PowerupScript.Identifier.GoldenSymbol_Seven)
				{
					if (identifier5 == PowerupScript.Identifier.ChastityBelt)
					{
						stringBuilder.Replace("[CAST_B_N]", PowerupScript.ChastityBelt_MultiplierBonusGet(false).ToString());
						goto IL_1030;
					}
					switch (identifier5)
					{
					case PowerupScript.Identifier.GoldenSymbol_Lemon:
						stringBuilder.Replace("[S_CURRENT]", "[S_LEMON]");
						stringBuilder.Replace("[S_CURRENT_BASE_VAL]", "[S_LEMON_BASE_VALUE]");
						goto IL_1030;
					case PowerupScript.Identifier.GoldenSymbol_Cherry:
						stringBuilder.Replace("[S_CURRENT]", "[S_CHERRY]");
						stringBuilder.Replace("[S_CURRENT_BASE_VAL]", "[S_CHERRY_BASE_VALUE]");
						goto IL_1030;
					case PowerupScript.Identifier.GoldenSymbol_Clover:
						stringBuilder.Replace("[S_CURRENT]", "[S_CLOVER]");
						stringBuilder.Replace("[S_CURRENT_BASE_VAL]", "[S_CLOVER_BASE_VALUE]");
						goto IL_1030;
					case PowerupScript.Identifier.GoldenSymbol_Bell:
						stringBuilder.Replace("[S_CURRENT]", "[S_BELL]");
						stringBuilder.Replace("[S_CURRENT_BASE_VAL]", "[S_BELL_BASE_VALUE]");
						goto IL_1030;
					case PowerupScript.Identifier.GoldenSymbol_Diamond:
						stringBuilder.Replace("[S_CURRENT]", "[S_DIAMOND]");
						stringBuilder.Replace("[S_CURRENT_BASE_VAL]", "[S_DIAMOND_BASE_VALUE]");
						goto IL_1030;
					case PowerupScript.Identifier.GoldenSymbol_Treasure:
						stringBuilder.Replace("[S_CURRENT]", "[S_COINS]");
						stringBuilder.Replace("[S_CURRENT_BASE_VAL]", "[S_COINS_BASE_VALUE]");
						goto IL_1030;
					case PowerupScript.Identifier.GoldenSymbol_Seven:
						break;
					default:
						goto IL_1030;
					}
				}
				else
				{
					if (identifier5 == PowerupScript.Identifier.PlayingCard_ClubsAce)
					{
						stringBuilder.Replace("[ACE_CLUBS_TICKETS]", (3L - GameplayData.Powerup_AceOfClubs_TicketsSpentGet()).ToString());
						goto IL_1030;
					}
					if (identifier5 == PowerupScript.Identifier.PlayingCard_SpadesAce)
					{
						stringBuilder.Replace("[ACE_SPADES_C_ACTIVATIONS]", (7L - GameplayData.Powerup_AceOfSpades_ActivationsCounterGet()).ToString());
						goto IL_1030;
					}
					if (identifier5 != PowerupScript.Identifier._999_OphanimWheels)
					{
						goto IL_1030;
					}
					Strings.PatternReplaceTag(stringBuilder, PatternScript.Kind.jackpot);
					stringBuilder.Replace("[OPH_WHEEL_N]", PowerupScript.OphanimWheels_JackpotsBookedGet(false).ToString());
					goto IL_1030;
				}
				stringBuilder.Replace("[S_CURRENT]", "[S_SEVEN]");
				stringBuilder.Replace("[S_CURRENT_BASE_VAL]", "[S_SEVEN_BASE_VALUE]");
			}
			IL_1030:
			if (powerupScript != null)
			{
				PowerupScript.Identifier identifier5 = powerupScript.identifier;
				if (identifier5 <= PowerupScript.Identifier.StepsCounter)
				{
					if (identifier5 <= PowerupScript.Identifier.PoopBeetle)
					{
						switch (identifier5)
						{
						case PowerupScript.Identifier.HorseShoeGold:
							stringBuilder.Replace("[UNLCK_N_GOLD_HORSESH]", Data.GameData.UnlockStepsMissing_HorseShoeGold.ToString());
							break;
						case PowerupScript.Identifier.HamsaUpside:
							stringBuilder.Replace("[UNLCK_N_HAMSA_UPSIDE]", Data.GameData.UnlockStepsMissing_HamsaUpside.ToString());
							break;
						case PowerupScript.Identifier.HamsaInverted:
						case PowerupScript.Identifier.LuckyCat:
							break;
						case PowerupScript.Identifier.LuckyCatFat:
							stringBuilder.Replace("[UNLCK_N_LCKYCT_FAT]", Data.GameData.UnlockStepsMissing_LuckyCatFat.ToString());
							break;
						case PowerupScript.Identifier.LuckyCatSwole:
							stringBuilder.Replace("[UNLCK_N_LCKYCT_SWOLE]", Data.GameData.UnlockStepsMissing_LuckyCatSwole.ToString());
							break;
						default:
							if (identifier5 == PowerupScript.Identifier.PoopBeetle)
							{
								stringBuilder.Replace("[UNLCK_N_DUNG_BEETLE]", Data.GameData.UnlockStepsMissing_DungBeetleStercoRaro.ToString());
							}
							break;
						}
					}
					else
					{
						switch (identifier5)
						{
						case PowerupScript.Identifier.SuperCapacitor:
							stringBuilder.Replace("[UNLCK_N_SCAPACITOR]", Data.GameData.UnlockStepsMissing__SuperCapacitor.ToString());
							break;
						case PowerupScript.Identifier.CrankGenerator:
							stringBuilder.Replace("[UNLCK_N_CRANK_GEN]", Data.GameData.UnlockStepsMissing__CrankGenerator.ToString());
							break;
						case PowerupScript.Identifier.GrattaEVinci_ScratchAndWin:
							stringBuilder.Replace("[UNLCK_N_GRATT_VINCI]", Data.GameData.UnlockStepsMissing_ScratchAndWin.ToString());
							break;
						default:
							switch (identifier5)
							{
							case PowerupScript.Identifier.Painkillers:
								stringBuilder.Replace("[UNLCK_N_PAIN_KILLER]", Data.GameData.UnlockStepsMissing_PainKillers.ToString());
								break;
							case PowerupScript.Identifier.Calendar:
								stringBuilder.Replace("[UNLCK_N_CALENDAR]", Data.GameData.UnlockStepsMissing_Calendar.ToString());
								break;
							case PowerupScript.Identifier.YellowStar:
								stringBuilder.Replace("[UNLCK_N_Y_STAR]", Data.GameData.UnlockStepsMissing_YellowStar.ToString());
								break;
							case PowerupScript.Identifier.FortuneCookie:
								stringBuilder.Replace("[UNLCK_N_FORTUNE_COOKIE]", Data.GameData.UnlockStepsMissing_FortuneCookie.ToString());
								break;
							case PowerupScript.Identifier.Sardines:
								stringBuilder.Replace("[UNLCK_N_SARDINES]", Data.GameData.UnlockStepsMissing_Sardines.ToString());
								break;
							case PowerupScript.Identifier.FortuneChanneler:
								stringBuilder.Replace("[UNLCK_N_FORTUNE_CHANNELER]", Data.GameData.UnlockStepsMissing_FortuneChanneler.ToString());
								break;
							case PowerupScript.Identifier.VoiceMailTape:
								stringBuilder.Replace("[UNLCK_N_VOICEM]", Data.GameData.UnlockStepsMissing_VoiceMail.ToString());
								break;
							case PowerupScript.Identifier.Garbage:
								stringBuilder.Replace("[UNLCK_N_GARBAGE]", Data.GameData.UnlockStepsMissing_Garbage.ToString());
								break;
							case PowerupScript.Identifier.AllIn:
								stringBuilder.Replace("[UNLCK_N_ALL_IN]", Data.GameData.UnlockStepsMissing_AllIn.ToString());
								break;
							case PowerupScript.Identifier.DarkLotus:
								stringBuilder.Replace("[UNLCK_N_DARK_LOTUS]", Data.GameData.UnlockStepsMissing_DarkLotus.ToString());
								break;
							case PowerupScript.Identifier.StepsCounter:
								stringBuilder.Replace("[UNLCK_N_STEPS_CNTR]", Data.GameData.UnlockStepsMissing_StepsCounter.ToString());
								break;
							}
							break;
						}
					}
				}
				else if (identifier5 <= PowerupScript.Identifier.AncientCoin)
				{
					if (identifier5 != PowerupScript.Identifier.ElectricityCounter)
					{
						if (identifier5 == PowerupScript.Identifier.AncientCoin)
						{
							stringBuilder.Replace("[UNLCK_N_ANCIENT_COIN]", Data.GameData.UnlockStepsMissing_AncientCoin.ToString());
						}
					}
					else
					{
						stringBuilder.Replace("[UNLCK_N_ELECTR_CNTR]", Data.GameData.UnlockStepsMissing_ElectricityCounter.ToString());
					}
				}
				else
				{
					switch (identifier5)
					{
					case PowerupScript.Identifier.CloversLandPatch:
						stringBuilder.Replace("[UNLCK_N_CLOVER_FIELD]", Data.GameData.UnlockStepsMissing_CloversLandPatch.ToString());
						break;
					case PowerupScript.Identifier.Mushrooms:
					case PowerupScript.Identifier.VineSoupShroom:
					case PowerupScript.Identifier.GiantShroom:
						break;
					case PowerupScript.Identifier.Hole_Circle:
						stringBuilder.Replace("[UNLCK_N_ABYSSU]", Data.GameData.UnlockStepsMissing_Abyssu.ToString());
						break;
					case PowerupScript.Identifier.Hole_Romboid:
						stringBuilder.Replace("[UNLCK_N_VORAGO]", Data.GameData.UnlockStepsMissing_Vorago.ToString());
						break;
					case PowerupScript.Identifier.Hole_Cross:
						stringBuilder.Replace("[UNLCK_N_BARATHRUM]", Data.GameData.UnlockStepsMissing__Barathrum.ToString());
						break;
					default:
						switch (identifier5)
						{
						case PowerupScript.Identifier.RottenPepper:
							stringBuilder.Replace("[UNLCK_N_ROTT_PEPP]", Data.GameData.UnlockStepsMissing_RottenPepper.ToString());
							break;
						case PowerupScript.Identifier.BellPepper:
							stringBuilder.Replace("[UNLCK_N_BELL_PEPP]", Data.GameData.UnlockStepsMissing_BellPepper.ToString());
							break;
						case PowerupScript.Identifier.GoldenPepper:
							stringBuilder.Replace("[UNLCK_N_GOLD_PEPP]", Data.GameData.UnlockStepsMissing_GoldenPepper.ToString());
							break;
						case PowerupScript.Identifier.HornDevil:
							stringBuilder.Replace("[UNLCK_N_DEVIL_HORN]", Data.GameData.UnlockStepsMissing_DevilsHorn.ToString());
							break;
						case PowerupScript.Identifier.Necronomicon:
							stringBuilder.Replace("[UNLCK_N_NECRONOMICON]", Data.GameData.UnlockStepsMissing_Necronomicon.ToString());
							break;
						case PowerupScript.Identifier.HolyBible:
						case PowerupScript.Identifier.BookOfShadows:
						case PowerupScript.Identifier.Gabibbh:
						case PowerupScript.Identifier.PossessedPhone:
						case PowerupScript.Identifier.MysticalTomato:
						case PowerupScript.Identifier.RitualBell:
						case PowerupScript.Identifier.CrystalSkull:
						case PowerupScript.Identifier.EvilDeal:
						case PowerupScript.Identifier.ChastityBelt:
						case PowerupScript.Identifier.SymbolInstant_Lemon:
						case PowerupScript.Identifier.SymbolInstant_Cherry:
						case PowerupScript.Identifier.SymbolInstant_Clover:
						case PowerupScript.Identifier.SymbolInstant_Bell:
						case PowerupScript.Identifier.SymbolInstant_Diamond:
						case PowerupScript.Identifier.SymbolInstant_Treasure:
						case PowerupScript.Identifier.SymbolInstant_Seven:
						case PowerupScript.Identifier.GeneralModCharm_Clicker:
						case PowerupScript.Identifier.GeneralModCharm_CloverBellBattery:
						case PowerupScript.Identifier.GeneralModCharm_CrystalSphere:
						case PowerupScript.Identifier.GoldenSymbol_Lemon:
						case PowerupScript.Identifier.GoldenSymbol_Cherry:
						case PowerupScript.Identifier.GoldenSymbol_Clover:
						case PowerupScript.Identifier.GoldenSymbol_Bell:
						case PowerupScript.Identifier.GoldenSymbol_Diamond:
						case PowerupScript.Identifier.GoldenSymbol_Treasure:
						case PowerupScript.Identifier.GoldenSymbol_Seven:
							break;
						case PowerupScript.Identifier.Baphomet:
							stringBuilder.Replace("[UNLCK_N_BAPHO]", Data.GameData.UnlockStepsMissing_Baphomet.ToString());
							break;
						case PowerupScript.Identifier.Cross:
							stringBuilder.Replace("[UNLCK_N_CROSS]", Data.GameData.UnlockStepsMissing_Cross.ToString());
							break;
						case PowerupScript.Identifier.Rosary:
							stringBuilder.Replace("[UNLCK_N_ROSARY]", Data.GameData.UnlockStepsMissing_Rosary.ToString());
							break;
						case PowerupScript.Identifier.PhotoBook:
							stringBuilder.Replace("[UNLCK_N_PH_BOOK]", Data.GameData.UnlockStepsMissing_PhotoBook.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_C_Dealer:
							stringBuilder.Replace("[UNLCK_N_DEALER]", Data.GameData.UnlockStepsMissing_Dealer.ToString());
							break;
						case PowerupScript.Identifier.GoldenKingMida:
							stringBuilder.Replace("[UNLCK_N_MIDA]", Data.GameData.UnlockStepsMissing_KingMida.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_M_Capitalist:
							stringBuilder.Replace("[UNLCK_N_CAPITALIST]", Data.GameData.UnlockStepsMissing_RagingCapitalist.ToString());
							break;
						case PowerupScript.Identifier.PuppetPersonalTrainer:
							stringBuilder.Replace("[UNLCK_N_TRAINER]", Data.GameData.UnlockStepsMissing_PuppetPersonalTrainer.ToString());
							break;
						case PowerupScript.Identifier.PuppetElectrician:
							stringBuilder.Replace("[UNLCK_N_ELECTRICIAN]", Data.GameData.UnlockStepsMissing_PuppetElectrician.ToString());
							break;
						case PowerupScript.Identifier.PuppetFortuneTeller:
							stringBuilder.Replace("[UNLCK_N_FORTUNE_TELLER]", Data.GameData.UnlockStepsMissing_PuppetFortuneTeller.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_C_Bricks:
							stringBuilder.Replace("[UNLCK_N_BRICKS]", Data.GameData.UnlockStepsMissing__BoardgameC_Bricks.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_C_Wood:
							stringBuilder.Replace("[UNLCK_N_WOOD]", Data.GameData.UnlockStepsMissing__BoardgameC_Wood.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_C_Sheep:
							stringBuilder.Replace("[UNLCK_N_SHEEP]", Data.GameData.UnlockStepsMissing__BoardgameC_Sheep.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_C_Wheat:
							stringBuilder.Replace("[UNLCK_N_WHEAT]", Data.GameData.UnlockStepsMissing__BoardgameC_Wheat.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_C_Stone:
							stringBuilder.Replace("[UNLCK_N_STONE]", Data.GameData.UnlockStepsMissing__BoardgameC_Stone.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_C_Harbor:
							stringBuilder.Replace("[UNLCK_N_HARBOR]", Data.GameData.UnlockStepsMissing__BoardgameC_Harbor.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_C_Thief:
							stringBuilder.Replace("[UNLCK_N_THIEF]", Data.GameData.UnlockStepsMissing__BoardgameC_Thief.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_M_Carriola:
							stringBuilder.Replace("[UNLCK_N_CARRIOLA]", Data.GameData.UnlockStepsMissing__BoardgameM_Carriola.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_M_Shoe:
							stringBuilder.Replace("[UNLCK_N_SHOE]", Data.GameData.UnlockStepsMissing__BoardgameM_Shoe.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_M_Ditale:
							stringBuilder.Replace("[UNLCK_N_DITALE]", Data.GameData.UnlockStepsMissing__BoardgameM_Ditale.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_M_FerroDaStiro:
							stringBuilder.Replace("[UNLCK_N_IRON]", Data.GameData.UnlockStepsMissing__BoardgameM_Iron.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_M_Car:
							stringBuilder.Replace("[UNLCK_N_CAR]", Data.GameData.UnlockStepsMissing__BoardgameM_Car.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_M_Ship:
							stringBuilder.Replace("[UNLCK_N_SHIP]", Data.GameData.UnlockStepsMissing__BoardgameM_Ship.ToString());
							break;
						case PowerupScript.Identifier.Boardgame_M_Hat:
							stringBuilder.Replace("[UNLCK_N_M_HAT]", Data.GameData.UnlockStepsMissing__BoardgameM_TubaHat.ToString());
							break;
						default:
							if (identifier5 == PowerupScript.Identifier.Jimbo)
							{
								stringBuilder.Replace("[JIMBO_EFFECTS]", GameplayData.JimboDescriptionStringsGet());
								stringBuilder.Replace("[JIMBO_ROUNDS_LEFT]", GameplayData.Powerup_Jimbo_RoundsLeft.ToString());
							}
							break;
						}
						break;
					}
				}
			}
			stringBuilder.Replace("[S_CHERRY]", "<sprite name=\"S_Cherry\">");
			stringBuilder.Replace("[S_LEMON]", "<sprite name=\"S_Lemon\">");
			stringBuilder.Replace("[S_CLOVER]", "<sprite name=\"S_Clover\">");
			stringBuilder.Replace("[S_BELL]", "<sprite name=\"S_Bell\">");
			stringBuilder.Replace("[S_DIAMOND]", "<sprite name=\"S_Diamond\">");
			stringBuilder.Replace("[S_COINS]", "<sprite name=\"S_Coins\">");
			stringBuilder.Replace("[S_SEVEN]", "<sprite name=\"S_Seven\">");
			stringBuilder.Replace("[S_CHERRY_BASE_VALUE]", GameplayData.Symbol_CoinsValue_GetBasic(SymbolScript.Kind.cherry).ToString() + "<sprite name=\"CoinSymbolOrange32\">");
			stringBuilder.Replace("[S_LEMON_BASE_VALUE]", GameplayData.Symbol_CoinsValue_GetBasic(SymbolScript.Kind.lemon).ToString() + "<sprite name=\"CoinSymbolOrange32\">");
			stringBuilder.Replace("[S_CLOVER_BASE_VALUE]", GameplayData.Symbol_CoinsValue_GetBasic(SymbolScript.Kind.clover).ToString() + "<sprite name=\"CoinSymbolOrange32\">");
			stringBuilder.Replace("[S_BELL_BASE_VALUE]", GameplayData.Symbol_CoinsValue_GetBasic(SymbolScript.Kind.bell).ToString() + "<sprite name=\"CoinSymbolOrange32\">");
			stringBuilder.Replace("[S_DIAMOND_BASE_VALUE]", GameplayData.Symbol_CoinsValue_GetBasic(SymbolScript.Kind.diamond).ToString() + "<sprite name=\"CoinSymbolOrange32\">");
			stringBuilder.Replace("[S_COINS_BASE_VALUE]", GameplayData.Symbol_CoinsValue_GetBasic(SymbolScript.Kind.coins).ToString() + "<sprite name=\"CoinSymbolOrange32\">");
			stringBuilder.Replace("[S_SEVEN_BASE_VALUE]", GameplayData.Symbol_CoinsValue_GetBasic(SymbolScript.Kind.seven).ToString() + "<sprite name=\"CoinSymbolOrange32\">");
			stringBuilder.Replace("[PMOD_SMULT_EXPLANATION]", Translation.Get("POWERUP_CHARM_MODIFIER_SYMBOLS_MULT"));
			stringBuilder.Replace("[PMOD_PMULT_EXPLANATION]", Translation.Get("POWERUP_CHARM_MODIFIER_PATTERN_MULT"));
			stringBuilder.Replace("[PMOD_TICKET_EXPLANATION]", Translation.Get("POWERUP_CHARM_MODIFIER_CLOVER_TICKET"));
			stringBuilder.Replace("[PMOD_OBSESSIVE_EXPLANATION]", Translation.Get("POWERUP_CHARM_MODIFIER_OBSESSIVE"));
			stringBuilder.Replace("[PMOD_GAMBLER_EXPLANATION]", Translation.Get("POWERUP_CHARM_MODIFIER_GAMBLER"));
			stringBuilder.Replace("[PMOD_SPECULATIVE_EXPLANATION]", Translation.Get("POWERUP_CHARM_MODIFIER_SPECULATIVE"));
			stringBuilder.Replace("[PMOD_DEVIOUS_EXPLANATION]", Translation.Get("POWERUP_CHARM_MODIFIER_DEVIOUS"));
			stringBuilder.Replace("[CMOD_NAME_TICKET]", Colors.GetColorRichTextString("olive") + Translation.Get("POWERUP_CHARM_MODIFIER_NAME_TICKET") + "</color>");
			stringBuilder.Replace("[CMOD_NAME_SMULT]", "<color=yellow>" + Translation.Get("POWERUP_CHARM_MODIFIER_NAME_SYMB_MULT") + "</color>");
			stringBuilder.Replace("[CMOD_NAME_PMULT]", "<color=orange>" + Translation.Get("POWERUP_CHARM_MODIFIER_NAME_PATTERN_MULT") + "</color>");
			stringBuilder.Replace("[CMOD_NAME_OBSESSIVE]", Colors.GetColorRichTextString("obsessive") + Translation.Get("POWERUP_CHARM_MODIFIER_NAME_OBSESSIVE") + "</color>");
			stringBuilder.Replace("[CMOD_NAME_GAMBLER]", Colors.GetColorRichTextString("gambler") + Translation.Get("POWERUP_CHARM_MODIFIER_NAME_GAMBLER") + "</color>");
			stringBuilder.Replace("[CMOD_NAME_SPECULATIVE]", Colors.GetColorRichTextString("speculative") + Translation.Get("POWERUP_CHARM_MODIFIER_NAME_SPECULTIVE") + "</color>");
			stringBuilder.Replace("[CMOD_NAME_DEVIOUS]", Colors.GetColorRichTextString("devious") + Translation.Get("POWERUP_CHARM_MODIFIER_NAME_DEVIOUS") + "</color>");
			stringBuilder.Replace("[K_USABLE]", "<sprite name=\"RedButton\"> <color=yellow>" + Translation.Get("POWERUP_KEYWORD_BUTTON_TRIGGERED") + "</color>");
			stringBuilder.Replace("[K_SINGLE_USE]", "<color=yellow>" + Translation.Get("POWERUP_KEYWORD_SINGLE_USE") + "</color>");
			stringBuilder.Replace("[K_CONSUMABLE]", "<color=yellow>" + Translation.Get("POWERUP_KEYWORD_CONSUMABLE") + "</color>");
			stringBuilder.Replace("[K_TRIGGERS_AT_PURCHASE]", "<color=yellow>" + Translation.Get("POWERUP_KEYWORD_TRIGGERS_AT_PURCHASE") + "</color>");
			stringBuilder.Replace("[K_INTEREST_REVENUE]", bigInteger3.ToStringSmart() + "<sprite name=\"CoinSymbolOrange32\">");
			stringBuilder.Replace("[K_RANDOM_ACTIVATION]", "<color=yellow>" + Translation.Get("POWERUP_KEYWORD_RANDOM_ACTIVATION") + "</color>");
			stringBuilder.Replace("[K_LUCK]", "<color=green>" + Translation.Get("POWERUP_KEYWORD_LUCK") + "</color>");
			stringBuilder.Replace("[K_S_MULT]", "<color=yellow>" + Translation.Get("POWERUP_KEYWORD_SYMBOLS_MULTIPLIER") + "</color>");
			stringBuilder.Replace("[K_P_MULT]", "<color=orange>" + Translation.Get("POWERUP_KEYWORD_PATTERN_MULTIPLIER") + "</color>");
			stringBuilder.Replace("$", "<sprite name=\"CoinSymbolWhite64\">");
			stringBuilder.Replace("€", "<sprite name=\"CoinSymbolOrange64\">");
			stringBuilder.Replace("[C_TICKET]", "<sprite name=\"CloverTicket\">");
			stringBuilder.Replace("[C_TICKETS_X_ROUND]", GameplayData.CloverTickets_BonusRoundsLeft_Get().ToString());
			stringBuilder.Replace("[INTEREST_REV]", GameplayData.InterestEarnedHypotetically().ToStringSmart() + "<sprite name=\"CoinSymbolOrange32\">");
			stringBuilder.Replace("[2X_INTEREST_REV]", (GameplayData.InterestEarnedHypotetically() * 2).ToStringSmart() + "<sprite name=\"CoinSymbolOrange32\">");
			stringBuilder.Replace("[3X_INTEREST_REV]", (GameplayData.InterestEarnedHypotetically() * 3).ToStringSmart() + "<sprite name=\"CoinSymbolOrange32\">");
			stringBuilder.Replace("[K_TAROT_REWARD]", PowerupScript.TarotDeckRewardGet(false).ToStringSmart());
			stringBuilder.Replace("[PENTACLE_BONUS]", PowerupScript.PentacleBonusGet(false).ToString());
			bool flag = powerupScript != null && powerupScript.IsRedButtonCharm();
			if (flag)
			{
				int num4 = GameplayData.Powerup_ButtonChargesUsed_Get(powerupScript.identifier);
				int num5 = GameplayData.Powerup_ButtonChargesMax_Get(powerupScript.identifier);
				int num6 = Mathf.Max(0, num5 - num4);
				if (num5 > 0)
				{
					Strings._sb.Clear();
					if (num5 == 1)
					{
						if (num6 == num5)
						{
							Strings._sb.Append("<sprite name=\"BS_Init\">");
							Strings._sb.Append("<sprite name=\"BS_ChargeBigGreen\">");
							Strings._sb.Append("<sprite name=\"BS_End\">");
							Strings._sb.Append("<sprite name=\"Charge\">");
						}
						else
						{
							Strings._sb.Append("<sprite name=\"BS_InitRed\">");
							Strings._sb.Append("<sprite name=\"BS_EmptyBig\">");
							Strings._sb.Append("<sprite name=\"BS_EndRed\">");
							Strings._sb.Append("<sprite name=\"BatteryRecharging\">");
						}
					}
					else if (num6 == num5)
					{
						Strings._sb.Append("<sprite name=\"BS_Init\">");
						for (int i = 0; i < num5; i++)
						{
							Strings._sb.Append("<sprite name=\"BS_ChargeGreen\">");
						}
						Strings._sb.Append("<sprite name=\"BS_End\">");
						Strings._sb.Append("<sprite name=\"Charge\">");
					}
					else
					{
						Strings._sb.Append("<sprite name=\"BS_InitRed\">");
						for (int j = 0; j < num5; j++)
						{
							if (j < num6)
							{
								Strings._sb.Append("<sprite name=\"BS_Charge\">");
							}
							else
							{
								Strings._sb.Append("<sprite name=\"BS_Empty\">");
							}
						}
						Strings._sb.Append("<sprite name=\"BS_EndRed\">");
						Strings._sb.Append("<sprite name=\"BatteryRecharging\">");
					}
					stringBuilder.Replace("[CHARGE_BAR]", Strings._sb.ToString());
				}
				if (identifier == RunModifierScript.Identifier.redButtonOverload)
				{
					stringBuilder.Replace("[RBC_EXPLAIN]", string.Concat(new string[]
					{
						text,
						"<color=grey>",
						Translation.Get("POWERUP_EXPLANATION_RED_BUTTON_CHARM_NO_RECHARGE"),
						"</color>",
						text2
					}));
				}
				else
				{
					stringBuilder.Replace("[RBC_EXPLAIN]", string.Concat(new string[]
					{
						text,
						"<color=grey>",
						Translation.Get("POWERUP_EXPLANATION_RED_BUTTON_CHARM_DEFAULT"),
						"</color>",
						text2
					}));
				}
				stringBuilder.Replace("[RB_USES]", num4.ToString());
				stringBuilder.Replace("[RB_USES_ABS]", GameplayData.Powerup_ButtonChargesUsed_GetAbsolute(powerupScript.identifier).ToString());
				stringBuilder.Replace("[RB_USES_LEFT]", num6.ToString());
				stringBuilder.Replace("[RB_USES_MAX]", num5.ToString());
			}
			else if (Application.isEditor && powerupScript != null && input.Contains("[K_USABLE]") && !flag)
			{
				string text3 = "Charm : " + powerupScript.identifier.ToString() + " - seems a red button charm but has no charges defined.";
				Debug.LogError(text3);
				ConsolePrompt.LogError(text3, "", 0f);
			}
			stringBuilder.Replace("[RED_BUTTON_ICO]", "<sprite name=\"RedButton\">");
			stringBuilder.Replace("[INFINITE_ICO]", "<sprite name=\"Infinite\">");
			stringBuilder.Replace("[CHARGE_ICO]", "<sprite name=\"Charge\">");
		}
		if (santizationKind == Strings.SantizationKind.all || santizationKind == Strings.SantizationKind.menus || santizationKind == Strings.SantizationKind.uiAndMenus)
		{
			long num7 = GameplayData.PhoneRerollCostGet();
			if (GameplayData.PhoneRerollCostGet() <= GameplayData.CloverTicketsGet())
			{
				stringBuilder.Replace("[PHONE_REROLL_COST]", "-" + num7.ToString() + "<sprite name=\"CloverTicket\">");
			}
			else
			{
				stringBuilder.Replace("[PHONE_REROLL_COST]", "<color=red>-" + num7.ToString() + "<sprite name=\"CloverTicket\"></color>");
			}
			int hypotehticalMaxSpinsBuyable = GameplayData.GetHypotehticalMaxSpinsBuyable();
			int hypotehticalMidSpinsBuyable = GameplayData.GetHypotehticalMidSpinsBuyable();
			if (subKind == Strings.SanitizationSubKind.menu_ReducedSanitization)
			{
				stringBuilder.Replace("[SPINS_MID]", hypotehticalMidSpinsBuyable.ToString());
				stringBuilder.Replace("[SPINS_MAX]", hypotehticalMaxSpinsBuyable.ToString());
				stringBuilder.Replace("[SPINS_MID_SHORT]", hypotehticalMidSpinsBuyable.ToString());
				stringBuilder.Replace("[SPINS_MAX_SHORT]", hypotehticalMaxSpinsBuyable.ToString());
			}
			else
			{
				string text4 = "";
				long num8 = GameplayData.CloverTickets_BonusBigBet_Get(true);
				if (num8 > 0L)
				{
					text4 = " +" + num8.ToString() + "<sprite name=\"CloverTicket\">";
				}
				string text5 = "";
				int num9 = GameplayData.ExtraSpinsGet(true);
				if (num9 > 0)
				{
					text5 = "<color=yellow>+" + num9.ToString() + "</color> ";
				}
				else if (num9 < 0)
				{
					text5 = "<color=red>" + num9.ToString() + "</color> ";
				}
				if (hypotehticalMaxSpinsBuyable <= 0)
				{
					stringBuilder.Replace("[BET_MIDDLE]", "0 <sprite name=\"CoinSymbolOrange32\">");
					stringBuilder.Replace("[BET_MAX]", "0 <sprite name=\"CoinSymbolOrange32\">");
					stringBuilder.Replace("[SPINS_MID]", "1 " + Translation.Get("DIALOGUE_SLOT_BET_WORD_SPIN"));
					if (hypotehticalMaxSpinsBuyable + num9 > 0)
					{
						stringBuilder.Replace("[SPINS_MAX]", "1 " + text5 + Translation.Get("DIALOGUE_SLOT_BET_WORD_SPIN") + text4);
					}
					else
					{
						stringBuilder.Replace("[SPINS_MAX]", "<color=red>1 " + Translation.Get("DIALOGUE_SLOT_BET_WORD_SPIN") + "</color>" + text4);
					}
				}
				else if (hypotehticalMaxSpinsBuyable == 1)
				{
					stringBuilder.Replace("[BET_MIDDLE]", "-" + GameplayData.SpinCostMid_Get().ToString() + " <sprite name=\"CoinSymbolOrange32\">");
					stringBuilder.Replace("[BET_MAX]", "-" + GameplayData.SpinCostMax_Get().ToString() + " <sprite name=\"CoinSymbolOrange32\">");
					if (hypotehticalMidSpinsBuyable + num9 > 0)
					{
						stringBuilder.Replace("[SPINS_MID]", "1 " + text5 + Translation.Get("DIALOGUE_SLOT_BET_WORD_SPIN"));
					}
					else
					{
						stringBuilder.Replace("[SPINS_MID]", "<color=red>1 " + Translation.Get("DIALOGUE_SLOT_BET_WORD_SPIN") + "</color>");
					}
					if (hypotehticalMaxSpinsBuyable + num9 > 0)
					{
						stringBuilder.Replace("[SPINS_MAX]", "1 " + text5 + Translation.Get("DIALOGUE_SLOT_BET_WORD_SPIN") + text4);
					}
					else
					{
						stringBuilder.Replace("[SPINS_MAX]", "<color=red>1 " + Translation.Get("DIALOGUE_SLOT_BET_WORD_SPIN") + "</color>" + text4);
					}
				}
				else
				{
					stringBuilder.Replace("[BET_MIDDLE]", "-" + GameplayData.SpinCostMid_Get().ToString() + " <sprite name=\"CoinSymbolOrange32\">");
					stringBuilder.Replace("[BET_MAX]", "-" + GameplayData.SpinCostMax_Get().ToString() + " <sprite name=\"CoinSymbolOrange32\">");
					string text6 = Translation.Get("DIALOGUE_SLOT_BET_WORD_SPIN");
					string text7 = Translation.Get("DIALOGUE_SLOT_BET_WORD_SPINS");
					string text8 = "";
					long num10 = GameplayData.CloverTickets_BonusLittleBet_Get(true);
					if (num10 > 0L)
					{
						text8 = " +" + num10.ToString() + "<sprite name=\"CloverTicket\">";
					}
					if (hypotehticalMidSpinsBuyable + num9 > 0)
					{
						stringBuilder.Replace("[SPINS_MID]", string.Concat(new string[]
						{
							hypotehticalMidSpinsBuyable.ToString(),
							" ",
							text5,
							(hypotehticalMidSpinsBuyable == 1) ? text6 : text7,
							text8
						}));
					}
					else
					{
						stringBuilder.Replace("[SPINS_MID]", "<color=red>1 " + text6 + "</color>" + text8);
					}
					if (hypotehticalMaxSpinsBuyable + num9 > 0)
					{
						stringBuilder.Replace("[SPINS_MAX]", string.Concat(new string[]
						{
							hypotehticalMaxSpinsBuyable.ToString(),
							" ",
							text5,
							(hypotehticalMaxSpinsBuyable == 1) ? text6 : text7,
							text4
						}));
					}
					else
					{
						stringBuilder.Replace("[SPINS_MAX]", string.Concat(new string[] { "<color=red>1 ", text5, text6, "</color>", text4 }));
					}
				}
			}
			stringBuilder.Replace("[SKULL]", "<sprite name=\"SkullSymbolOrange64\">");
		}
		if (santizationKind == Strings.SantizationKind.all || santizationKind == Strings.SantizationKind.endStats)
		{
			stringBuilder.Replace("$", "<sprite name=\"CoinSymbolWhite64\">");
			stringBuilder.Replace("€", "<sprite name=\"CoinSymbolOrange64\">");
			stringBuilder.Replace("[C_TICKET]", "<sprite name=\"CloverTicket\">");
		}
		if (Strings.sanitize666999_Counter > 0)
		{
			stringBuilder.Replace("666", "<color=red>666</color>");
			stringBuilder.Replace("999", "<color=yellow>999</color>");
			Strings.sanitize666999_Counter--;
		}
		stringBuilder.Replace("[C_OLIVE]", "<color=#b6c828>");
		stringBuilder.Replace("[/C]", "</color>");
		stringBuilder.Replace("[10_SPACES]", "          ");
		stringBuilder.Replace("[3_SPACES]", "   ");
		stringBuilder.Replace("[5_SPACES]", "     ");
		stringBuilder.Replace("[TAB]", "   ");
		stringBuilder.Replace("[2TABS]", "      ");
		stringBuilder.Replace("[4_TABS]", "                ");
		stringBuilder.Replace("[SPINS_SYMBOL_32]", "<sprite name=\"SpinSymbol32\">");
		return stringBuilder.ToString();
	}

	// Token: 0x06000386 RID: 902 RVA: 0x00018484 File Offset: 0x00016684
	private static string SymbolsWithBaseValue_GetString(bool includeCoinsIcon)
	{
		Strings._sb.Clear();
		List<SymbolScript.Kind> list = GameplayData.SymbolsAvailable_GetAll(false);
		for (int i = 0; i < list.Count; i++)
		{
			SymbolScript.Kind kind = list[i];
			if (kind == SymbolScript.Kind.undefined || kind == SymbolScript.Kind.count)
			{
				Debug.LogError("Strings > PoopBeetleString(): symbol available is undefined or count.");
			}
			else if (kind == SymbolScript.Kind.six || kind == SymbolScript.Kind.nine)
			{
				Debug.LogError("Strings > PoopBeetleString(): symbol available is six or nine.");
			}
			else
			{
				Strings._sb.Append(Strings.Symbol_GetStringWithValue(kind, includeCoinsIcon));
				if (i < list.Count - 1)
				{
					Strings._sb.Append(" ");
				}
			}
		}
		return Strings._sb.ToString();
	}

	// Token: 0x06000387 RID: 903 RVA: 0x0001851C File Offset: 0x0001671C
	private static void PatternReplaceTag(StringBuilder sbRef, PatternScript.Kind kind)
	{
		switch (kind)
		{
		case PatternScript.Kind.jackpot:
			sbRef.Replace("[PT_J]", "<sprite name=\"PtJ\">");
			return;
		case PatternScript.Kind.horizontal2:
			sbRef.Replace("[PT_2H]", "<sprite name=\"Pt2H\">");
			return;
		case PatternScript.Kind.horizontal3:
			sbRef.Replace("[PT_3H]", "<sprite name=\"Pt3H\">");
			return;
		case PatternScript.Kind.horizontal4:
			break;
		case PatternScript.Kind.horizontal5:
			sbRef.Replace("[PT_5H]", "<sprite name=\"Pt5H\">");
			return;
		case PatternScript.Kind.vertical2:
			sbRef.Replace("[PT_2V]", "<sprite name=\"Pt2V\">");
			return;
		case PatternScript.Kind.vertical3:
			sbRef.Replace("[PT_3V]", "<sprite name=\"Pt3V\">");
			return;
		case PatternScript.Kind.diagonal2:
			sbRef.Replace("[PT_2D]", "<sprite name=\"Pt2D\">");
			return;
		case PatternScript.Kind.diagonal3:
			sbRef.Replace("[PT_3D]", "<sprite name=\"Pt3D\">");
			return;
		case PatternScript.Kind.pyramid:
			sbRef.Replace("[PT_P]", "<sprite name=\"PtP\">");
			return;
		case PatternScript.Kind.pyramidInverted:
			sbRef.Replace("[PT_PU]", "<sprite name=\"PtPU\">");
			return;
		case PatternScript.Kind.triangle:
			sbRef.Replace("[PT_T]", "<sprite name=\"PtT\">");
			return;
		case PatternScript.Kind.triangleInverted:
			sbRef.Replace("[PT_TU]", "<sprite name=\"PtTU\">");
			return;
		case PatternScript.Kind.snakeUpDown:
			sbRef.Replace("[PT_SnkUpDown]", "<sprite name=\"PtSnkUpDown\">");
			return;
		case PatternScript.Kind.snakeDownUp:
			sbRef.Replace("[PT_SnkDownUp]", "<sprite name=\"PtSnkDownUp\">");
			return;
		case PatternScript.Kind.eye:
			sbRef.Replace("[PT_Eye]", "<sprite name=\"PtEye\">");
			return;
		default:
			Debug.LogError("SanitizePatternTag() - Unknown pattern kind: " + kind.ToString());
			break;
		}
	}

	// Token: 0x06000388 RID: 904 RVA: 0x000186A0 File Offset: 0x000168A0
	private static void PatternReplaceTagAll(StringBuilder sbReference)
	{
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.horizontal2);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.horizontal3);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.horizontal4);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.horizontal5);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.vertical2);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.vertical3);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.diagonal2);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.diagonal3);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.pyramid);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.pyramidInverted);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.triangle);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.triangleInverted);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.snakeUpDown);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.snakeDownUp);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.eye);
		Strings.PatternReplaceTag(sbReference, PatternScript.Kind.jackpot);
	}

	// Token: 0x06000389 RID: 905 RVA: 0x00018724 File Offset: 0x00016924
	private static void PatternReplaceBaseValue(StringBuilder sbReference, PatternScript.Kind kind)
	{
		switch (kind)
		{
		case PatternScript.Kind.jackpot:
			sbReference.Replace("[PT_J_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.jackpot).ToString());
			return;
		case PatternScript.Kind.horizontal2:
			sbReference.Replace("[PT_2H_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.horizontal2).ToString());
			return;
		case PatternScript.Kind.horizontal3:
			sbReference.Replace("[PT_3H_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.horizontal3).ToString());
			return;
		case PatternScript.Kind.horizontal4:
			sbReference.Replace("[PT_4H_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.horizontal4).ToString());
			return;
		case PatternScript.Kind.horizontal5:
			sbReference.Replace("[PT_5H_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.horizontal5).ToString());
			return;
		case PatternScript.Kind.vertical2:
			sbReference.Replace("[PT_2V_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.vertical2).ToString());
			return;
		case PatternScript.Kind.vertical3:
			sbReference.Replace("[PT_3V_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.vertical3).ToString());
			return;
		case PatternScript.Kind.diagonal2:
			sbReference.Replace("[PT_2D_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.diagonal2).ToString());
			return;
		case PatternScript.Kind.diagonal3:
			sbReference.Replace("[PT_3D_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.diagonal3).ToString());
			return;
		case PatternScript.Kind.pyramid:
			sbReference.Replace("[PT_P_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.pyramid).ToString());
			return;
		case PatternScript.Kind.pyramidInverted:
			sbReference.Replace("[PT_PU_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.pyramidInverted).ToString());
			return;
		case PatternScript.Kind.triangle:
			sbReference.Replace("[PT_T_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.triangle).ToString());
			return;
		case PatternScript.Kind.triangleInverted:
			sbReference.Replace("[PT_TU_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.triangleInverted).ToString());
			return;
		case PatternScript.Kind.snakeUpDown:
			sbReference.Replace("[PT_SNKUPDOWN_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.snakeUpDown).ToString());
			return;
		case PatternScript.Kind.snakeDownUp:
			sbReference.Replace("[PT_SNKDOWNUP_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.snakeDownUp).ToString());
			return;
		case PatternScript.Kind.eye:
			sbReference.Replace("[PT_EYE_BASE_VALUE]", GameplayData.Pattern_Value_GetBasic(PatternScript.Kind.eye).ToString());
			return;
		default:
			Debug.LogError("SanitizePatternTag() - Unknown pattern kind: " + kind.ToString());
			return;
		}
	}

	// Token: 0x0600038A RID: 906 RVA: 0x00018950 File Offset: 0x00016B50
	private static void PatternReplaceBaseValueAll(StringBuilder sbReference)
	{
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.horizontal2);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.horizontal3);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.horizontal4);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.horizontal5);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.vertical2);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.vertical3);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.diagonal2);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.diagonal3);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.pyramid);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.pyramidInverted);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.triangle);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.triangleInverted);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.snakeUpDown);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.snakeDownUp);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.eye);
		Strings.PatternReplaceBaseValue(sbReference, PatternScript.Kind.jackpot);
	}

	// Token: 0x0600038B RID: 907 RVA: 0x000189D4 File Offset: 0x00016BD4
	private static string Symbol_GetString(SymbolScript.Kind symbolKind)
	{
		switch (symbolKind)
		{
		case SymbolScript.Kind.lemon:
			return "<sprite name=\"S_Lemon\">";
		case SymbolScript.Kind.cherry:
			return "<sprite name=\"S_Cherry\">";
		case SymbolScript.Kind.clover:
			return "<sprite name=\"S_Clover\">";
		case SymbolScript.Kind.bell:
			return "<sprite name=\"S_Bell\">";
		case SymbolScript.Kind.diamond:
			return "<sprite name=\"S_Diamond\">";
		case SymbolScript.Kind.coins:
			return "<sprite name=\"S_Coins\">";
		case SymbolScript.Kind.seven:
			return "<sprite name=\"S_Seven\">";
		}
		Debug.LogError("sprite string for symbol" + symbolKind.ToString() + "is not defined");
		return null;
	}

	// Token: 0x0600038C RID: 908 RVA: 0x00018A5C File Offset: 0x00016C5C
	private static string Symbol_GetStringWithValue(SymbolScript.Kind symbolKind, bool includeCoinIcon)
	{
		return Strings.Symbol_GetString(symbolKind) + GameplayData.Symbol_CoinsValue_GetBasic(symbolKind).ToString() + (includeCoinIcon ? "<sprite name=\"CoinSymbolOrange32\">" : "");
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00018A94 File Offset: 0x00016C94
	private static string Symbols_GetString_YellowOnes()
	{
		Strings._sb.Clear();
		List<SymbolScript.Kind> list = GameplayData.SymbolsAvailable_GetAll(false);
		for (int i = 0; i < list.Count; i++)
		{
			if (SymbolScript.IsYellow(list[i]))
			{
				Strings._sb.Append(Strings.Symbol_GetString(list[i]));
			}
		}
		return Strings._sb.ToString();
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00018AF4 File Offset: 0x00016CF4
	private static string Symbols_GetString_NonYellowOnes()
	{
		Strings._sb.Clear();
		List<SymbolScript.Kind> list = GameplayData.SymbolsAvailable_GetAll(false);
		for (int i = 0; i < list.Count; i++)
		{
			if (!SymbolScript.IsYellow(list[i]))
			{
				Strings._sb.Append(Strings.Symbol_GetString(list[i]));
			}
		}
		return Strings._sb.ToString();
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00018B54 File Offset: 0x00016D54
	private static void PatternReplaceWith_Symbols(StringBuilder sbReference, string keyToReplace, string separator, List<SymbolScript.Kind> symbolsToConsider)
	{
		Strings._sb.Clear();
		for (int i = 0; i < symbolsToConsider.Count; i++)
		{
			SymbolScript.Kind kind = symbolsToConsider[i];
			if (kind != SymbolScript.Kind.six && kind != SymbolScript.Kind.nine)
			{
				Strings._sb.Append(Strings.Symbol_GetString(kind));
				Strings._sb.Append(separator);
			}
		}
		sbReference.Replace(keyToReplace, Strings._sb.ToString());
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00018BBC File Offset: 0x00016DBC
	private static void PatternReplaceWith_SymbolsAndBValue(StringBuilder sbReference, string keyToReplace, string separator, List<SymbolScript.Kind> symbolsToConsider)
	{
		Strings._sb.Clear();
		for (int i = 0; i < symbolsToConsider.Count; i++)
		{
			SymbolScript.Kind kind = symbolsToConsider[i];
			if (kind != SymbolScript.Kind.six && kind != SymbolScript.Kind.nine)
			{
				Strings._sb.Append(Strings.Symbol_GetStringWithValue(kind, true));
				Strings._sb.Append(separator);
			}
		}
		sbReference.Replace(keyToReplace, Strings._sb.ToString());
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00018C28 File Offset: 0x00016E28
	private static void PatternReplaceWith_Patterns(StringBuilder sbReference, string keyToReplace, string separator, List<PatternScript.Kind> patternsToConsider)
	{
		Strings._sb.Clear();
		for (int i = 0; i < patternsToConsider.Count; i++)
		{
			PatternScript.Kind kind = patternsToConsider[i];
			switch (kind)
			{
			case PatternScript.Kind.jackpot:
				Strings._sb.Append("<sprite name=\"PtJ\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.horizontal2:
				Strings._sb.Append("<sprite name=\"Pt2H\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.horizontal3:
				Strings._sb.Append("<sprite name=\"Pt3H\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.horizontal4:
				Strings._sb.Append("<sprite name=\"Pt4H\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.horizontal5:
				Strings._sb.Append("<sprite name=\"Pt5H\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.vertical2:
				Strings._sb.Append("<sprite name=\"Pt2V\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.vertical3:
				Strings._sb.Append("<sprite name=\"Pt3V\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.diagonal2:
				Strings._sb.Append("<sprite name=\"Pt2D\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.diagonal3:
				Strings._sb.Append("<sprite name=\"Pt3D\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.pyramid:
				Strings._sb.Append("<sprite name=\"PtP\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.pyramidInverted:
				Strings._sb.Append("<sprite name=\"PtPU\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.triangle:
				Strings._sb.Append("<sprite name=\"PtT\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.triangleInverted:
				Strings._sb.Append("<sprite name=\"PtTU\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.snakeUpDown:
				Strings._sb.Append("<sprite name=\"PtSnkUpDown\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.snakeDownUp:
				Strings._sb.Append("<sprite name=\"PtSnkDownUp\">");
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.eye:
				Strings._sb.Append("<sprite name=\"PtEye\">");
				Strings._sb.Append(separator);
				break;
			default:
				Debug.LogError("PatterReplaceWith_Patterns() - Unknown pattern kind: " + kind.ToString());
				break;
			}
		}
		sbReference.Replace(keyToReplace, Strings._sb.ToString());
	}

	// Token: 0x06000392 RID: 914 RVA: 0x00018EDC File Offset: 0x000170DC
	private static void PatternReplaceWith_PatternsAndBValue(StringBuilder sbReference, string keyToReplace, string separator, List<PatternScript.Kind> patternsToConsider)
	{
		Strings._sb.Clear();
		for (int i = 0; i < patternsToConsider.Count; i++)
		{
			PatternScript.Kind kind = patternsToConsider[i];
			switch (kind)
			{
			case PatternScript.Kind.jackpot:
				Strings._sb.Append("<sprite name=\"PtJ\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.horizontal2:
				Strings._sb.Append("<sprite name=\"Pt2H\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.horizontal3:
				Strings._sb.Append("<sprite name=\"Pt3H\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.horizontal4:
				Strings._sb.Append("<sprite name=\"Pt4H\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.horizontal5:
				Strings._sb.Append("<sprite name=\"Pt5H\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.vertical2:
				Strings._sb.Append("<sprite name=\"Pt2V\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.vertical3:
				Strings._sb.Append("<sprite name=\"Pt3V\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.diagonal2:
				Strings._sb.Append("<sprite name=\"Pt2D\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.diagonal3:
				Strings._sb.Append("<sprite name=\"Pt3D\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.pyramid:
				Strings._sb.Append("<sprite name=\"PtP\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.pyramidInverted:
				Strings._sb.Append("<sprite name=\"PtPU\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.triangle:
				Strings._sb.Append("<sprite name=\"PtT\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.triangleInverted:
				Strings._sb.Append("<sprite name=\"PtTU\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.snakeUpDown:
				Strings._sb.Append("<sprite name=\"PtSnkUpDown\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.snakeDownUp:
				Strings._sb.Append("<sprite name=\"PtSnkDownUp\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			case PatternScript.Kind.eye:
				Strings._sb.Append("<sprite name=\"PtEye\">");
				Strings._sb.Append(GameplayData.Pattern_Value_GetBasic(kind).ToString());
				Strings._sb.Append(separator);
				break;
			default:
				Debug.LogError("PatterReplaceWith_PatternsAndValue() - Unknown pattern kind: " + kind.ToString());
				break;
			}
		}
		sbReference.Replace(keyToReplace, Strings._sb.ToString());
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00019328 File Offset: 0x00017528
	private static void PatternReplaceWith_Patterns_BelowNElements(StringBuilder sbReference, string keyToReplace, string separator, List<PatternScript.Kind> patternsToConsider, int maxElementsPerPattern)
	{
		Strings._patterns.Clear();
		for (int i = 0; i < patternsToConsider.Count; i++)
		{
			PatternScript.Kind kind = patternsToConsider[i];
			if (PatternScript.GetElementsCount(kind) <= maxElementsPerPattern)
			{
				Strings._patterns.Add(kind);
			}
		}
		Strings.PatternReplaceWith_Patterns(sbReference, keyToReplace, separator, Strings._patterns);
	}

	// Token: 0x06000394 RID: 916 RVA: 0x0001937C File Offset: 0x0001757C
	private static void PatternReplaceWith_PatternsAndBValue_BelowNElements(StringBuilder sbReference, string keyToReplace, string separator, List<PatternScript.Kind> patternsToConsider, int maxElementsPerPattern)
	{
		Strings._patterns.Clear();
		for (int i = 0; i < patternsToConsider.Count; i++)
		{
			PatternScript.Kind kind = patternsToConsider[i];
			if (PatternScript.GetElementsCount(kind) <= maxElementsPerPattern)
			{
				Strings._patterns.Add(kind);
			}
		}
		Strings.PatternReplaceWith_PatternsAndBValue(sbReference, keyToReplace, separator, Strings._patterns);
	}

	// Token: 0x06000395 RID: 917 RVA: 0x000193D0 File Offset: 0x000175D0
	private static void PAtternReplaceWith_Patterns_AboveNElements(StringBuilder sbReference, string keyToReplace, string separator, List<PatternScript.Kind> patternsToConsider, int minElementsPerPattern)
	{
		Strings._patterns.Clear();
		for (int i = 0; i < patternsToConsider.Count; i++)
		{
			PatternScript.Kind kind = patternsToConsider[i];
			if (PatternScript.GetElementsCount(kind) >= minElementsPerPattern)
			{
				Strings._patterns.Add(kind);
			}
		}
		Strings.PatternReplaceWith_Patterns(sbReference, keyToReplace, separator, Strings._patterns);
	}

	// Token: 0x06000396 RID: 918 RVA: 0x00019424 File Offset: 0x00017624
	private static void PatternReplaceWith_PatternsAndBValue_AboveNElements(StringBuilder sbReference, string keyToReplace, string separator, List<PatternScript.Kind> patternsToConsider, int minElementsPerPattern)
	{
		Strings._patterns.Clear();
		for (int i = 0; i < patternsToConsider.Count; i++)
		{
			PatternScript.Kind kind = patternsToConsider[i];
			if (PatternScript.GetElementsCount(kind) >= minElementsPerPattern)
			{
				Strings._patterns.Add(kind);
			}
		}
		Strings.PatternReplaceWith_PatternsAndBValue(sbReference, keyToReplace, separator, Strings._patterns);
	}

	// Token: 0x06000397 RID: 919 RVA: 0x00019478 File Offset: 0x00017678
	private static void SymbolsAndPatternsReplaceAll(StringBuilder sbReference)
	{
		Strings.PatternReplaceWith_Symbols(sbReference, "[ALL_SYMBOLS]", "", GameplayData.SymbolsAvailable_GetAll(false));
		Strings.PatternReplaceWith_SymbolsAndBValue(sbReference, "[ALL_SYMBOLS_W_VALUE]", ",", GameplayData.SymbolsAvailable_GetAll(false));
		Strings.PatternReplaceWith_Patterns(sbReference, "[ALL_PATTERNS]", "", GameplayData.PatternsAvailable_GetAll());
		Strings.PatternReplaceWith_PatternsAndBValue(sbReference, "[ALL_PATTERNS_W_VALUE]", ",", GameplayData.PatternsAvailable_GetAll());
		Strings.PatternReplaceWith_Patterns_BelowNElements(sbReference, "[ALL_PATTERNS_3LESS]", "", GameplayData.PatternsAvailable_GetAll(), 3);
		Strings.PatternReplaceWith_PatternsAndBValue_BelowNElements(sbReference, "[ALL_PATTERNS_3LESS_W_VALUE]", ",", GameplayData.PatternsAvailable_GetAll(), 3);
		Strings.PAtternReplaceWith_Patterns_AboveNElements(sbReference, "[ALL_PATTERNS_4MORE]", "", GameplayData.PatternsAvailable_GetAll(), 4);
		Strings.PatternReplaceWith_PatternsAndBValue_AboveNElements(sbReference, "[ALL_PATTERNS_4MORE_W_VALUE]", ",", GameplayData.PatternsAvailable_GetAll(), 4);
	}

	private const int PLAYER_INDEX = 0;

	public const string TEXT_SPRITE_COIN_SYMBOL_ORANGE_32 = "<sprite name=\"CoinSymbolOrange32\">";

	public const string TEXT_SPRITE_COIN_SYMBOL_ORANGE_64 = "<sprite name=\"CoinSymbolOrange64\">";

	public const string TEXT_SPRITE_COIN_SYMBOL_WHITE_32 = "<sprite name=\"CoinSymbolWhite32\">";

	public const string TEXT_SPRITE_COIN_SYMBOL_WHITE_64 = "<sprite name=\"CoinSymbolWhite64\">";

	public const string TEXT_SPRITE_COIN_32 = "<sprite name=\"Coin32\">";

	public const string TEXT_SPRITE_COIN_WHITE_32 = "<sprite name=\"CoinWhite32\">";

	public const string TEXT_SPRITE_COIN_64 = "<sprite name=\"Coin64\">";

	public const string TEXT_SPRITE_CLOVER_TICKET = "<sprite name=\"CloverTicket\">";

	public const string TEXT_SPRITE_CLOVER_TICKET_SATURATED = "<sprite name=\"CloverTicketSaturated\">";

	public const string TEXT_SPRITE_SKULL_ORANGE_32 = "<sprite name=\"SkullSymbolOrange32\">";

	public const string TEXT_SPRITE_SKULL_ORANGE_64 = "<sprite name=\"SkullSymbolOrange64\">";

	public const string TEXT_SPRITE_SKULL_WHITE_32 = "<sprite name=\"SkullSymbolWhite32\">";

	public const string TEXT_SPRITE_SKULL_WHITE_64 = "<sprite name=\"SkullSymbolWhite64\">";

	public const string TEXT_SPRITE_FINGER_POINTER_ORANGE_32 = "<sprite name=\"FingerPointerOrange\">";

	public const string TEXT_SPRITE_SPIN_SYMBOL_32 = "<sprite name=\"SpinSymbol32\">";

	public const string TEXT_SPRITE_PEPPER_VERTICAL = "<sprite name=\"PepperVertical\">";

	public const string TEXT_SPRITE_HORSE_SHOE = "<sprite name=\"Horse Shoe\">";

	public const string TEXT_SPRITE_RED_BUTTON = "<sprite name=\"RedButton\">";

	public const string TEXT_SPRITE_CHARGE = "<sprite name=\"Charge\">";

	public const string TEXT_SPRITE_INFINITE = "<sprite name=\"Infinite\">";

	public const string TEXT_SPRITE_BATTERY_RECHARING = "<sprite name=\"BatteryRecharging\">";

	public const string TEXT_SPRITE_BATTERY_BAR_BEGIN = "<sprite name=\"BS_Init\">";

	public const string TEXT_SPRITE_BATTERY_BAR_BEGIN_RED = "<sprite name=\"BS_InitRed\">";

	public const string TEXT_SPRITE_BATTERY_BAR_BAR = "<sprite name=\"BS_Charge\">";

	public const string TEXT_SPRITE_BATTERY_BAR_BAR_GREEN = "<sprite name=\"BS_ChargeGreen\">";

	public const string TEXT_SPRITE_BATTERY_BAR_BAR_GREEN_BIG = "<sprite name=\"BS_ChargeBigGreen\">";

	public const string TEXT_SPRITE_BATTERY_BAR_EMPTY = "<sprite name=\"BS_Empty\">";

	public const string TEXT_SPRITE_BATTERY_BAR_EMPTY_BIG = "<sprite name=\"BS_EmptyBig\">";

	public const string TEXT_SPRITE_BATTERY_BAR_END = "<sprite name=\"BS_End\">";

	public const string TEXT_SPRITE_BATTERY_BAR_END_RED = "<sprite name=\"BS_EndRed\">";

	public const string TEXT_SPRITE_BIG_EYE = "<sprite name=\"BigEye\">";

	public const string TEXT_RARITY_MILD = "<sprite name=\"Rarity_Mild\">";

	public const string TEXT_RARITY_SPICY = "<sprite name=\"Rarity_Spicy\">";

	public const string TEXT_RARITY_HOT = "<sprite name=\"Rarity_Hot\">";

	public const string TEXT_RARITY_HELL = "<sprite name=\"Rarity_Hell\">";

	public const string TEXT_SYMBOL_MODIFIER_INSTANT_REWARD = "<sprite name=\"ModInstantReward\">";

	public const string TEXT_SYMBOL_MODIFIER_TICKET = "<sprite name=\"ModTicket\">";

	public const string TEXT_SYMBOL_MODIFIER_GOLDEN = "<sprite name=\"ModGold\">";

	public const string TEXT_SYMBOL_MODIFIER_REPETITION = "<sprite name=\"ModRepetition\">";

	public const string TEXT_SYMBOL_MODIFIER_BATTERY = "<sprite name=\"ModBattery\">";

	public const string TEXT_SYMBOL_MODIFIER_CHAIN = "<sprite name=\"ModChain\">";

	public const string TEXT_SYMBOL_CARD_VICTORY = "<sprite name=\"CardSymb_Victory\">";

	public const string TEXT_SYMBOL_CARD_COPIES = "<sprite name=\"CardSymb_Copies\">";

	public const string TEXT_SYMBOL_LANGUAGES_ICON = "<sprite name=\"LanguagesIcon\">";

	public const string TEXT_SYMBOL_RED_LOCK = "<sprite name=\"RedLock\">";

	public const string TEXT_SYMBOL_TWITCH_POLL = "<sprite name=\"TwitchPoll\">";

	public const string TEXT_SYMBOL_BONE_FULL = "<sprite name=\"BoneFull\">";

	public const string TEXT_SYMBOL_BONE_EMPTY = "<sprite name=\"BoneEmpty\">";

	public const string TEXT_SYMBOL_SLOT_WARNING = "<sprite name=\"SlotWarning\">";

	public const string TEXT_SYMBOL_MEMORY_CARD = "<sprite name=\"MemoryCard\">";

	public const string TEXT_SPRITE_SLOT_SYMBOL_CHERRY = "<sprite name=\"S_Cherry\">";

	public const string TEXT_SPRITE_SLOT_SYMBOL_LEMON = "<sprite name=\"S_Lemon\">";

	public const string TEXT_SPRITE_SLOT_SYMBOL_CLOVER = "<sprite name=\"S_Clover\">";

	public const string TEXT_SPRITE_SLOT_SYMBOL_BELL = "<sprite name=\"S_Bell\">";

	public const string TEXT_SPRITE_SLOT_SYMBOL_DIAMOND = "<sprite name=\"S_Diamond\">";

	public const string TEXT_SPRITE_SLOT_SYMBOL_COINS = "<sprite name=\"S_Coins\">";

	public const string TEXT_SPRITE_SLOT_SYMBOL_SEVEN = "<sprite name=\"S_Seven\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_1 = "<sprite name=\"Pt1\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_2_HOR = "<sprite name=\"Pt2H\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_3_HOR = "<sprite name=\"Pt3H\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_4_HOR = "<sprite name=\"Pt4H\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_5_HOR = "<sprite name=\"Pt5H\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_2_VER = "<sprite name=\"Pt2V\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_3_VER = "<sprite name=\"Pt3V\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_2_DIAG = "<sprite name=\"Pt2D\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_3_DIAG = "<sprite name=\"Pt3D\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_PYRAMID = "<sprite name=\"PtP\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_PYRAMID_UPSIDEDOWN = "<sprite name=\"PtPU\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_TRIANGLE = "<sprite name=\"PtT\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_TRIANGLE_UPSIDEDOWN = "<sprite name=\"PtTU\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_SNAKE_UP_DOWN = "<sprite name=\"PtSnkUpDown\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_SNAKE_DOWN_UP = "<sprite name=\"PtSnkDownUp\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_EYE = "<sprite name=\"PtEye\">";

	public const string TEXT_SPRITE_SLOT_PATTERN_JACKPOT = "<sprite name=\"PtJ\">";

	public const string TEXT_SPRITE_DICE = "<sprite name=\"Dice\">";

	private static PowerupScript temporaryInspectedPowerup = null;

	private static int sanitize666999_Counter = 0;

	private static StringBuilder _sb = new StringBuilder();

	private static List<PatternScript.Kind> _patterns = new List<PatternScript.Kind>();

	public enum SantizationKind
	{
		undefined = -1,
		all,
		ui,
		menus,
		uiAndMenus,
		powerupKeywords,
		endStats,
		count
	}

	public enum SanitizationSubKind
	{
		undefined = -1,
		none,
		powerup_ShowAllSymbolsAndPatternsValues,
		powerup_ShowPatternsValues,
		powerup_DebtPercentages,
		powerup_Skeleton,
		powerup_GrandmasPurse,
		powerup_Hole,
		powerup_Hourglass,
		powerup_SpicyPeppers,
		powerup_Baphomet,
		powerup_SymbolModifier,
		menu_ReducedSanitization,
		count
	}
}
