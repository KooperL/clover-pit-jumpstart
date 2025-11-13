using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;

public static class RunModifierScript
{
	// Token: 0x060006E0 RID: 1760 RVA: 0x0002BDF4 File Offset: 0x00029FF4
	public static string GetCardPrefabName(RunModifierScript.Identifier identifier)
	{
		return RunModifierScript.cardsPrefabsDictionary[identifier];
	}

	// Token: 0x060006E1 RID: 1761 RVA: 0x0002BE04 File Offset: 0x0002A004
	public static RunModifierScript.Rarity RarityGet(RunModifierScript.Identifier identifier)
	{
		switch (identifier)
		{
		case RunModifierScript.Identifier.defaultModifier:
			return RunModifierScript.Rarity.common;
		case RunModifierScript.Identifier.phoneEnhancer:
			return RunModifierScript.Rarity.epic;
		case RunModifierScript.Identifier.redButtonOverload:
			return RunModifierScript.Rarity.epic;
		case RunModifierScript.Identifier.smallerStore:
			return RunModifierScript.Rarity.common;
		case RunModifierScript.Identifier.smallItemPool:
			return RunModifierScript.Rarity.rare;
		case RunModifierScript.Identifier.interestsGrow:
			return RunModifierScript.Rarity.uncommon;
		case RunModifierScript.Identifier.lessSpaceMoreDiscount:
			return RunModifierScript.Rarity.uncommon;
		case RunModifierScript.Identifier.smallRoundsMoreRounds:
			return RunModifierScript.Rarity.common;
		case RunModifierScript.Identifier.oneRoundPerDeadline:
			return RunModifierScript.Rarity.common;
		case RunModifierScript.Identifier.headStart:
			return RunModifierScript.Rarity.common;
		case RunModifierScript.Identifier.extraPacks:
			return RunModifierScript.Rarity.common;
		case RunModifierScript.Identifier._666BigBetDouble_SmallBetNoone:
			return RunModifierScript.Rarity.uncommon;
		case RunModifierScript.Identifier._666DoubleChances_JackpotRecovers:
			return RunModifierScript.Rarity.uncommon;
		case RunModifierScript.Identifier._666LastRoundGuaranteed:
			return RunModifierScript.Rarity.rare;
		case RunModifierScript.Identifier.drawerTableModifications:
			return RunModifierScript.Rarity.uncommon;
		case RunModifierScript.Identifier.drawerModGamble:
			return RunModifierScript.Rarity.rare;
		case RunModifierScript.Identifier.halven2SymbolsChances:
			return RunModifierScript.Rarity.rare;
		case RunModifierScript.Identifier.charmsRecycling:
			return RunModifierScript.Rarity.common;
		case RunModifierScript.Identifier.allCharmsStoreModded:
			return RunModifierScript.Rarity.epic;
		case RunModifierScript.Identifier.bigDebt:
			return RunModifierScript.Rarity.common;
		default:
			Debug.LogError("RunModifierScript.RarityGet(): rarity not handled for run modifier: " + identifier.ToString());
			return RunModifierScript.Rarity.undefined;
		}
	}

	// Token: 0x060006E2 RID: 1762 RVA: 0x0002BEB0 File Offset: 0x0002A0B0
	private static float RarityThresholdGet(RunModifierScript.Identifier identifier)
	{
		RunModifierScript.Rarity rarity = RunModifierScript.RarityGet(identifier);
		switch (rarity)
		{
		case RunModifierScript.Rarity.common:
			return 0f;
		case RunModifierScript.Rarity.uncommon:
			return 0.15f;
		case RunModifierScript.Rarity.rare:
			return 0.35f;
		case RunModifierScript.Rarity.epic:
			return 0.5f;
		default:
			Debug.LogError("RunModifierScript.RarityThresholdGet(): rarity not handled: " + rarity.ToString());
			return 1f;
		}
	}

	// Token: 0x060006E3 RID: 1763 RVA: 0x0002BF18 File Offset: 0x0002A118
	public static int OrderWeightGet(RunModifierScript.Identifier identifier)
	{
		int num = 0;
		RunModifierScript.Rarity rarity = RunModifierScript.RarityGet(identifier);
		switch (rarity)
		{
		case RunModifierScript.Rarity.common:
			num = 0;
			break;
		case RunModifierScript.Rarity.uncommon:
			num = 1;
			break;
		case RunModifierScript.Rarity.rare:
			num = 2;
			break;
		case RunModifierScript.Rarity.epic:
			num = 3;
			break;
		default:
			Debug.LogError("RunModifierScript.OrderWeightGet(): rarity not handled: " + rarity.ToString());
			break;
		}
		if (Data.game.RunModifier_UnlockedTimes_Get(identifier) <= 0 && identifier != RunModifierScript.Identifier.defaultModifier)
		{
			num += 1000000;
		}
		return num;
	}

	// Token: 0x060006E4 RID: 1764 RVA: 0x0002BF94 File Offset: 0x0002A194
	public static RunModifierScript.Identifier CardGetFromPack()
	{
		RunModifierScript.Identifier identifier = RunModifierScript.Identifier.undefined;
		int num = 100;
		while (identifier == RunModifierScript.Identifier.undefined || identifier == RunModifierScript.Identifier.count || identifier == RunModifierScript.Identifier.defaultModifier)
		{
			identifier = (RunModifierScript.Identifier)R.Rng_Cards.Range(1, 20);
			if (identifier != RunModifierScript.Identifier.undefined && identifier != RunModifierScript.Identifier.count)
			{
				float num2 = RunModifierScript.RarityThresholdGet(identifier);
				if (R.Rng_Cards.Value < num2)
				{
					identifier = RunModifierScript.Identifier.undefined;
				}
				else
				{
					num--;
					if (num < 0)
					{
						identifier = RunModifierScript.Identifier.headStart;
						break;
					}
				}
			}
		}
		return identifier;
	}

	// Token: 0x060006E5 RID: 1765 RVA: 0x0002BFF8 File Offset: 0x0002A1F8
	public static string TitleGet(RunModifierScript.Identifier identifier)
	{
		switch (identifier)
		{
		case RunModifierScript.Identifier.defaultModifier:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_DEFAULT_NO_MOD"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.phoneEnhancer:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_PHONE_ENHANCER"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.redButtonOverload:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_RED_BUTTON_OVERLOAD"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.smallerStore:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_SMALLER_STORE"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.smallItemPool:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_SMALL_ITEM_POOL"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.interestsGrow:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_INTEREST_GROW"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.lessSpaceMoreDiscount:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_LESS_SPACE_MORE_DISCOUNT"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.smallRoundsMoreRounds:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_SMALL_ROUNDS_MORE_ROUNDS"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.oneRoundPerDeadline:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_ONE_ROUND_PER_DEADLINE"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.headStart:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_HEAD_START"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.extraPacks:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_EXTRA_PACKS"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier._666BigBetDouble_SmallBetNoone:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_666_SMALL_BET_SAFE"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier._666DoubleChances_JackpotRecovers:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_666_JACKPOTS_RECOVERS"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier._666LastRoundGuaranteed:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_666_LAST_ROUND_GUARANTEED"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.drawerTableModifications:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_DRAWERS_TABLE_MOD"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.drawerModGamble:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_DRAWERS_MOD_GAMBLE"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.halven2SymbolsChances:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_HALVEN_2_SYMBOLS"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.charmsRecycling:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_CHARMS_RECYCLE"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.allCharmsStoreModded:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_ALL_STORE_MODDED"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.bigDebt:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_TITLE_BIG_DEBT"), Strings.SanitizationSubKind.none);
		default:
		{
			string text = "RunModifierScript.TitleGet(): identifier not handled: " + identifier.ToString();
			Debug.LogError(text);
			ConsolePrompt.LogError(text, "", 0f);
			return text;
		}
		}
	}

	// Token: 0x060006E6 RID: 1766 RVA: 0x0002C1F8 File Offset: 0x0002A3F8
	public static string DescriptionGet(RunModifierScript.Identifier identifier)
	{
		switch (identifier)
		{
		case RunModifierScript.Identifier.defaultModifier:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_DEFAULT_NO_MOD"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.phoneEnhancer:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_PHONE_ENHANCER"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.redButtonOverload:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_RED_BUTTON_OVERLOAD"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.smallerStore:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_SMALLER_STORE"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.smallItemPool:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_SMALL_ITEM_POOL"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.interestsGrow:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_INTEREST_GROW"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.lessSpaceMoreDiscount:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_LESS_SPACE_MORE_DISCOUNT"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.smallRoundsMoreRounds:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_SMALL_ROUNDS_MORE_ROUNDS"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.oneRoundPerDeadline:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_ONE_ROUND_PER_DEADLINE"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.headStart:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_HEAD_START"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.extraPacks:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_EXTRA_PACKS"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier._666BigBetDouble_SmallBetNoone:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_666_SMALL_BET_SAFE"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier._666DoubleChances_JackpotRecovers:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_666_JACKPOTS_RECOVERS"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier._666LastRoundGuaranteed:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_666_LAST_ROUND_GUARANTEED"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.drawerTableModifications:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_DRAWERS_TABLE_MOD"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.drawerModGamble:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_DRAWERS_MOD_GAMBLE"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.halven2SymbolsChances:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_HALVEN_2_SYMBOLS"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.charmsRecycling:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_CHARMS_RECYCLE"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.allCharmsStoreModded:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_ALL_STORE_MODDED"), Strings.SanitizationSubKind.none);
		case RunModifierScript.Identifier.bigDebt:
			return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get("CARD_DESCRIPTION_BIG_DEBT"), Strings.SanitizationSubKind.none);
		default:
		{
			string text = "RunModifierScript.DescriptionGet(): identifier not handled: " + identifier.ToString();
			Debug.LogError(text);
			ConsolePrompt.LogError(text, "", 0f);
			return text;
		}
		}
	}

	// Token: 0x060006E7 RID: 1767 RVA: 0x0002C3F8 File Offset: 0x0002A5F8
	public static string AlternativeIntroDialogueGetKey(RunModifierScript.Identifier identifier)
	{
		switch (identifier)
		{
		case RunModifierScript.Identifier.defaultModifier:
			return null;
		case RunModifierScript.Identifier.phoneEnhancer:
			return "CARD_ALT_INTRO_DIALOGUE_PHONE_ENHANCER";
		case RunModifierScript.Identifier.redButtonOverload:
			return "CARD_ALT_INTRO_DIALOGUE_RED_BUTTON_OVERLOAD";
		case RunModifierScript.Identifier.smallerStore:
			return "CARD_ALT_INTRO_DIALOGUE_SMALLER_STORE";
		case RunModifierScript.Identifier.smallItemPool:
			return "CARD_ALT_INTRO_DIALOGUE_SMALL_ITEM_POOL";
		case RunModifierScript.Identifier.interestsGrow:
			return "CARD_ALT_INTRO_DIALOGUE_INTEREST_GROW";
		case RunModifierScript.Identifier.lessSpaceMoreDiscount:
			return "CARD_ALT_INTRO_DIALOGUE_LESS_SPACE_MORE_DISCOUNT";
		case RunModifierScript.Identifier.smallRoundsMoreRounds:
			return "CARD_ALT_INTRO_DIALOGUE_SMALL_ROUNDS_MORE_ROUNDS";
		case RunModifierScript.Identifier.oneRoundPerDeadline:
			return "CARD_ALT_INTRO_DIALOGUE_ONE_ROUND_PER_DEADLINE";
		case RunModifierScript.Identifier.headStart:
			return "CARD_ALT_INTRO_DIALOGUE_HEAD_START";
		case RunModifierScript.Identifier.extraPacks:
			return "CARD_ALT_INTRO_DIALOGUE_EXTRA_PACKS";
		case RunModifierScript.Identifier._666BigBetDouble_SmallBetNoone:
			return "CARD_ALT_INTRO_DIALOGUE_666_SMALL_BET_SAFE";
		case RunModifierScript.Identifier._666DoubleChances_JackpotRecovers:
			return "CARD_ALT_INTRO_DIALOGUE_666_JACKPOTS_RECOVERS";
		case RunModifierScript.Identifier._666LastRoundGuaranteed:
			return "CARD_ALT_INTRO_DIALOGUE_666_LAST_ROUND_GUARANTEED";
		case RunModifierScript.Identifier.drawerTableModifications:
			return "CARD_ALT_INTRO_DIALOGUE_DRAWERS_TABLE_MOD";
		case RunModifierScript.Identifier.drawerModGamble:
			return "CARD_ALT_INTRO_DIALOGUE_DRAWERS_MOD_GAMBLE";
		case RunModifierScript.Identifier.halven2SymbolsChances:
			return "CARD_ALT_INTRO_DIALOGUE_HALVEN_2_SYMBOLS";
		case RunModifierScript.Identifier.charmsRecycling:
			return "CARD_ALT_INTRO_DIALOGUE_CHARMS_RECYCLE";
		case RunModifierScript.Identifier.allCharmsStoreModded:
			return "CARD_ALT_INTRO_DIALOGUE_ALL_STORE_MODDED";
		case RunModifierScript.Identifier.bigDebt:
			return "CARD_ALT_INTRO_DIALOGUE_BIG_DEBT";
		default:
		{
			string text = "RunModifierScript.DescriptionGet(): identifier not handled: " + identifier.ToString();
			Debug.LogError(text);
			ConsolePrompt.LogError(text, "", 0f);
			return text;
		}
		}
	}

	// Token: 0x060006E8 RID: 1768 RVA: 0x0002C4FE File Offset: 0x0002A6FE
	public static void TriggerAnimation(RunModifierScript.Identifier runModifier)
	{
		PowerupTriggerAnimController.AddAnimation(null, runModifier, PowerupTriggerAnimController.AnimationCapsule.AnimationKind.card);
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x0002C508 File Offset: 0x0002A708
	public static void TriggerAnimation_IfEquipped(RunModifierScript.Identifier desiredRunModifier)
	{
		if (GameplayData.RunModifier_GetCurrent() != desiredRunModifier)
		{
			return;
		}
		RunModifierScript.TriggerAnimation(desiredRunModifier);
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x0002C519 File Offset: 0x0002A719
	public static void MFunc_SmallerStoreRestocksBonus()
	{
		GameplayData.StoreFreeRestocksSet(GameplayData.StoreFreeRestocksGet() + 2L);
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x0002C528 File Offset: 0x0002A728
	public static void MFunc_SmallItemPool_CheckForBannedItems()
	{
		bool flag = false;
		for (int i = 0; i < StoreCapsuleScript.storePowerups.Length; i++)
		{
			if (!(StoreCapsuleScript.storePowerups[i] == null) && PowerupScript.IsBanned(StoreCapsuleScript.storePowerups[i].identifier, StoreCapsuleScript.storePowerups[i].archetype))
			{
				flag = true;
				StoreCapsuleScript.storePowerups[i] = null;
			}
		}
		if (flag)
		{
			StoreCapsuleScript.Restock(true, true, StoreCapsuleScript.storePowerups, false, false);
		}
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x0002C594 File Offset: 0x0002A794
	public static void OnRunModifierSet(RunModifierScript.Identifier identifier)
	{
		switch (identifier)
		{
		case RunModifierScript.Identifier.smallerStore:
			RunModifierScript.MFunc_SmallerStoreRestocksBonus();
			return;
		case RunModifierScript.Identifier.smallItemPool:
			RunModifierScript.MFunc_SmallItemPool_CheckForBannedItems();
			return;
		case RunModifierScript.Identifier.interestsGrow:
			break;
		case RunModifierScript.Identifier.lessSpaceMoreDiscount:
			GameplayData.MaxEquippablePowerupsSet(6);
			StoreCapsuleScript.RefreshCostTextAll();
			return;
		case RunModifierScript.Identifier.smallRoundsMoreRounds:
			GameplayData.DeadlineRoundsIncrement_Manual(4);
			GameplayData.MaxSpins_Set(1);
			GameplayData.CloverTickets_BonusRoundsLeft_Set(2L);
			return;
		case RunModifierScript.Identifier.oneRoundPerDeadline:
			GameplayData.DeadlineRoundsIncrement_Set(1);
			GameplayData.MaxSpins_Set(21);
			GameplayData.CoinsAdd(14, true);
			GameplayData.CloverTicketsAdd(3L, true);
			GameplayData.CloverTickets_BonusBigBet_Add(1L);
			GameplayData.CloverTickets_BonusLittleBet_Add(2L);
			return;
		case RunModifierScript.Identifier.headStart:
			GameplayData.CloverTicketsAdd(10L, true);
			GameplayData.CloverTickets_BonusBigBet_Set(0L);
			GameplayData.CloverTickets_BonusLittleBet_Set(0L);
			return;
		default:
		{
			if (identifier == RunModifierScript.Identifier.halven2SymbolsChances)
			{
				List<SymbolScript.Kind> list = new List<SymbolScript.Kind>(GameplayData.SymbolsAvailable_GetAll(true));
				int num = R.Rng_RunMod.Range(0, list.Count);
				GameplayData.Symbol_Chance_Add(list[num], -(GameplayData.Symbol_Chance_Get(list[num], false, false) / 2f));
				list.RemoveAt(num);
				num = R.Rng_RunMod.Range(0, list.Count);
				GameplayData.Symbol_Chance_Add(list[num], -(GameplayData.Symbol_Chance_Get(list[num], false, false) / 2f));
				list.RemoveAt(num);
				return;
			}
			if (identifier != RunModifierScript.Identifier.allCharmsStoreModded)
			{
				return;
			}
			for (int i = 0; i < StoreCapsuleScript.storePowerups.Length; i++)
			{
				PowerupScript powerupScript = StoreCapsuleScript.storePowerups[i];
				if (!(powerupScript == null))
				{
					powerupScript.ModifierReEvaluate(false, true);
				}
			}
			StoreCapsuleScript.RefreshCostTextAll();
			GameplayData.CloverTicketsAdd(2L, true);
			break;
		}
		}
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x0002C704 File Offset: 0x0002A904
	public static void InitializeAll()
	{
		int num = 20;
		for (int i = 0; i < num; i++)
		{
			RunModifierScript.Identifier identifier = (RunModifierScript.Identifier)i;
			RunModifierScript.RarityGet(identifier);
			RunModifierScript.TitleGet(identifier);
			RunModifierScript.DescriptionGet(identifier);
			RunModifierScript.AlternativeIntroDialogueGetKey(identifier);
			string cardPrefabName = RunModifierScript.GetCardPrefabName(identifier);
			if (string.IsNullOrEmpty(cardPrefabName) || AssetMaster.GetPrefab(cardPrefabName) == null)
			{
				Debug.LogError("RunModifierScript.InitializeAll(): prefab is null for identifier: " + identifier.ToString());
			}
			else
			{
				CardScript component = AssetMaster.GetPrefab(cardPrefabName).GetComponent<CardScript>();
				if (component.identifier != identifier)
				{
					Debug.LogError("RunModifierScript.InitializeAll(): prefab identifier is the wrong one: " + component.identifier.ToString() + " - wants: " + identifier.ToString());
				}
			}
		}
	}

	private const bool RUN_ERROR_TESTS = true;

	private static Dictionary<RunModifierScript.Identifier, string> cardsPrefabsDictionary = new Dictionary<RunModifierScript.Identifier, string>
	{
		{
			RunModifierScript.Identifier.defaultModifier,
			"Card Default Modifier"
		},
		{
			RunModifierScript.Identifier.phoneEnhancer,
			"Card Phone Enhancer"
		},
		{
			RunModifierScript.Identifier.redButtonOverload,
			"Card Red Button Overload"
		},
		{
			RunModifierScript.Identifier.smallerStore,
			"Card Smaller Store"
		},
		{
			RunModifierScript.Identifier.smallItemPool,
			"Card Small Item Pool"
		},
		{
			RunModifierScript.Identifier.interestsGrow,
			"Card Interests Grow"
		},
		{
			RunModifierScript.Identifier.lessSpaceMoreDiscount,
			"Card Less Space More Discount"
		},
		{
			RunModifierScript.Identifier.smallRoundsMoreRounds,
			"Card Small Rounds More Rounds"
		},
		{
			RunModifierScript.Identifier.oneRoundPerDeadline,
			"Card One Round Per Deadline"
		},
		{
			RunModifierScript.Identifier.headStart,
			"Card Head Start"
		},
		{
			RunModifierScript.Identifier.extraPacks,
			"Card Extra Packs"
		},
		{
			RunModifierScript.Identifier._666BigBetDouble_SmallBetNoone,
			"Card 666 Small Bet Safe"
		},
		{
			RunModifierScript.Identifier._666DoubleChances_JackpotRecovers,
			"Card 666 Jackpots Recovers"
		},
		{
			RunModifierScript.Identifier._666LastRoundGuaranteed,
			"Card 666 Last Round"
		},
		{
			RunModifierScript.Identifier.drawerTableModifications,
			"Card Drawers Table Mod"
		},
		{
			RunModifierScript.Identifier.drawerModGamble,
			"Card Drawers Mod Gamble"
		},
		{
			RunModifierScript.Identifier.halven2SymbolsChances,
			"Card Halven 2 Symbols"
		},
		{
			RunModifierScript.Identifier.charmsRecycling,
			"Card Charms Recycle"
		},
		{
			RunModifierScript.Identifier.allCharmsStoreModded,
			"Card All Store Modded"
		},
		{
			RunModifierScript.Identifier.bigDebt,
			"Card Big Debt"
		}
	};

	public enum Identifier
	{
		defaultModifier,
		phoneEnhancer,
		redButtonOverload,
		smallerStore,
		smallItemPool,
		interestsGrow,
		lessSpaceMoreDiscount,
		smallRoundsMoreRounds,
		oneRoundPerDeadline,
		headStart,
		extraPacks,
		_666BigBetDouble_SmallBetNoone,
		_666DoubleChances_JackpotRecovers,
		_666LastRoundGuaranteed,
		drawerTableModifications,
		drawerModGamble,
		halven2SymbolsChances,
		charmsRecycling,
		allCharmsStoreModded,
		bigDebt,
		count,
		undefined
	}

	public enum Rarity
	{
		common,
		uncommon,
		rare,
		epic,
		count,
		undefined
	}
}
