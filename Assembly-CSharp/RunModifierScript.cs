using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;

// Token: 0x02000069 RID: 105
public static class RunModifierScript
{
	// Token: 0x06000787 RID: 1927 RVA: 0x0000C2D1 File Offset: 0x0000A4D1
	public static string GetCardPrefabName(RunModifierScript.Identifier identifier)
	{
		return RunModifierScript.cardsPrefabsDictionary[identifier];
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x0003DED0 File Offset: 0x0003C0D0
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

	// Token: 0x06000789 RID: 1929 RVA: 0x0003DF7C File Offset: 0x0003C17C
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

	// Token: 0x0600078A RID: 1930 RVA: 0x0003DFE4 File Offset: 0x0003C1E4
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

	// Token: 0x0600078B RID: 1931 RVA: 0x0003E060 File Offset: 0x0003C260
	public static RunModifierScript.Identifier CardGetFromPack()
	{
		int num = 500;
		RunModifierScript.Identifier identifier = (RunModifierScript.Identifier)R.Rng_Cards.Range(1, 20);
		for (;;)
		{
			bool flag = false;
			num--;
			if (num < 0)
			{
				break;
			}
			if (identifier == RunModifierScript.Identifier.undefined || identifier == RunModifierScript.Identifier.count || identifier == RunModifierScript.Identifier.defaultModifier)
			{
				flag = true;
			}
			if (Data.game.RunModifier_FoilLevel_Get(identifier) >= 2 || Data.game.DesiredFoilLevelGet(identifier) >= 2)
			{
				flag = true;
			}
			if (flag)
			{
				int num2 = (int)identifier;
				num2 += R.Rng_Cards.Choose<int>(new int[] { 1, 1, -1 });
				if (num2 < 1)
				{
					num2 = 19;
				}
				if (num2 >= 20)
				{
					num2 = 1;
				}
				identifier = (RunModifierScript.Identifier)num2;
			}
			else
			{
				float num3 = RunModifierScript.RarityThresholdGet(identifier);
				if (R.Rng_Cards.Value >= num3 || num <= 0)
				{
					return identifier;
				}
				identifier = RunModifierScript.Identifier.undefined;
			}
		}
		identifier = RunModifierScript.Identifier.undefined;
		return identifier;
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x0003E124 File Offset: 0x0003C324
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

	// Token: 0x0600078D RID: 1933 RVA: 0x0003E324 File Offset: 0x0003C524
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

	// Token: 0x0600078E RID: 1934 RVA: 0x0003E524 File Offset: 0x0003C724
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

	// Token: 0x0600078F RID: 1935 RVA: 0x0000C2DE File Offset: 0x0000A4DE
	public static void TriggerAnimation(RunModifierScript.Identifier runModifier)
	{
		PowerupTriggerAnimController.AddAnimation(null, runModifier, PowerupTriggerAnimController.AnimationCapsule.AnimationKind.card);
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x0000C2E8 File Offset: 0x0000A4E8
	public static void TriggerAnimation_IfEquipped(RunModifierScript.Identifier desiredRunModifier)
	{
		if (GameplayData.RunModifier_GetCurrent() != desiredRunModifier)
		{
			return;
		}
		RunModifierScript.TriggerAnimation(desiredRunModifier);
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x0000C2F9 File Offset: 0x0000A4F9
	public static void MFunc_SmallerStoreRestocksBonus()
	{
		GameplayData.StoreFreeRestocksSet(GameplayData.StoreFreeRestocksGet() + 2L);
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x0003E62C File Offset: 0x0003C82C
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

	// Token: 0x06000793 RID: 1939 RVA: 0x0003E698 File Offset: 0x0003C898
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
		case RunModifierScript.Identifier._666BigBetDouble_SmallBetNoone:
		case RunModifierScript.Identifier._666DoubleChances_JackpotRecovers:
		case RunModifierScript.Identifier._666LastRoundGuaranteed:
		case RunModifierScript.Identifier.drawerTableModifications:
		case RunModifierScript.Identifier.drawerModGamble:
		case RunModifierScript.Identifier.charmsRecycling:
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
		case RunModifierScript.Identifier.extraPacks:
			GameplayData.DepositSet(15);
			break;
		case RunModifierScript.Identifier.halven2SymbolsChances:
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
		case RunModifierScript.Identifier.allCharmsStoreModded:
		{
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
			return;
		}
		default:
			return;
		}
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x0003E82C File Offset: 0x0003CA2C
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

	// Token: 0x040006A1 RID: 1697
	private const bool RUN_ERROR_TESTS = true;

	// Token: 0x040006A2 RID: 1698
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

	// Token: 0x0200006A RID: 106
	public enum Identifier
	{
		// Token: 0x040006A4 RID: 1700
		defaultModifier,
		// Token: 0x040006A5 RID: 1701
		phoneEnhancer,
		// Token: 0x040006A6 RID: 1702
		redButtonOverload,
		// Token: 0x040006A7 RID: 1703
		smallerStore,
		// Token: 0x040006A8 RID: 1704
		smallItemPool,
		// Token: 0x040006A9 RID: 1705
		interestsGrow,
		// Token: 0x040006AA RID: 1706
		lessSpaceMoreDiscount,
		// Token: 0x040006AB RID: 1707
		smallRoundsMoreRounds,
		// Token: 0x040006AC RID: 1708
		oneRoundPerDeadline,
		// Token: 0x040006AD RID: 1709
		headStart,
		// Token: 0x040006AE RID: 1710
		extraPacks,
		// Token: 0x040006AF RID: 1711
		_666BigBetDouble_SmallBetNoone,
		// Token: 0x040006B0 RID: 1712
		_666DoubleChances_JackpotRecovers,
		// Token: 0x040006B1 RID: 1713
		_666LastRoundGuaranteed,
		// Token: 0x040006B2 RID: 1714
		drawerTableModifications,
		// Token: 0x040006B3 RID: 1715
		drawerModGamble,
		// Token: 0x040006B4 RID: 1716
		halven2SymbolsChances,
		// Token: 0x040006B5 RID: 1717
		charmsRecycling,
		// Token: 0x040006B6 RID: 1718
		allCharmsStoreModded,
		// Token: 0x040006B7 RID: 1719
		bigDebt,
		// Token: 0x040006B8 RID: 1720
		count,
		// Token: 0x040006B9 RID: 1721
		undefined
	}

	// Token: 0x0200006B RID: 107
	public enum Rarity
	{
		// Token: 0x040006BB RID: 1723
		common,
		// Token: 0x040006BC RID: 1724
		uncommon,
		// Token: 0x040006BD RID: 1725
		rare,
		// Token: 0x040006BE RID: 1726
		epic,
		// Token: 0x040006BF RID: 1727
		count,
		// Token: 0x040006C0 RID: 1728
		undefined
	}
}
