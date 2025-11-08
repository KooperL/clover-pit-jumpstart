using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;

public class AbilityScript
{
	// Token: 0x060003C3 RID: 963 RVA: 0x00019F0B File Offset: 0x0001810B
	public AbilityScript.Identifier IdentifierGet()
	{
		return this.identifier;
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x00019F13 File Offset: 0x00018113
	public AbilityScript.Category CategoryGet()
	{
		return this.category;
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x00019F1B File Offset: 0x0001811B
	public AbilityScript.Archetype ArchetypeGet()
	{
		return this.archetype;
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x00019F23 File Offset: 0x00018123
	public AbilityScript.Rarity RarityGet()
	{
		return this.rarity;
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x00019F2C File Offset: 0x0001812C
	public bool RarityRerollEvaluate()
	{
		float num;
		switch (this.rarity)
		{
		case AbilityScript.Rarity.common:
			num = 0f;
			break;
		case AbilityScript.Rarity.uncommon:
			num = 0.15f;
			break;
		case AbilityScript.Rarity.rare:
			num = 0.35f;
			break;
		case AbilityScript.Rarity.ultraRare:
			num = 0.6f;
			break;
		case AbilityScript.Rarity.legendary:
			num = 0.75f;
			break;
		default:
			Debug.LogError("AbilityScript.RarityRerollChanceGet(): Unknown rarity");
			return false;
		}
		return R.Rng_Phone.Value < num;
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x00019FA6 File Offset: 0x000181A6
	public static AbilityScript AbilityGet(AbilityScript.Identifier identifier)
	{
		if (!AbilityScript.dict_All.ContainsKey(identifier))
		{
			Debug.LogError("AbilityScript.AbilityGet(): Ability not found in dictionary");
			return null;
		}
		return AbilityScript.dict_All[identifier];
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x00019FCC File Offset: 0x000181CC
	public string NameGetTranslated()
	{
		return Translation.Get(this.nameKey);
	}

	// Token: 0x060003CA RID: 970 RVA: 0x00019FD9 File Offset: 0x000181D9
	public string DescriptionGetTranslated()
	{
		Strings.SetTemporaryFlag_Sanitize666And999(1);
		return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get(this.descriptionKey), this._GetAbilitySanitizationSubKind());
	}

	// Token: 0x060003CB RID: 971 RVA: 0x00019FF8 File Offset: 0x000181F8
	public int RepliesGetCount()
	{
		if (this.repliesKeys == null)
		{
			return 0;
		}
		return this.repliesKeys.Length;
	}

	// Token: 0x060003CC RID: 972 RVA: 0x0001A00C File Offset: 0x0001820C
	public string ReplyGet(int index)
	{
		if (this.repliesKeys == null)
		{
			return null;
		}
		if (index < 0 || index >= this.repliesKeys.Length)
		{
			return null;
		}
		return Translation.Get(this.repliesKeys[index]);
	}

	// Token: 0x060003CD RID: 973 RVA: 0x0001A036 File Offset: 0x00018236
	public string ReplyGetRandom()
	{
		if (this.repliesKeys == null)
		{
			return null;
		}
		if (this.repliesKeys.Length == 0)
		{
			return null;
		}
		return Translation.Get(this.repliesKeys[global::UnityEngine.Random.Range(0, this.repliesKeys.Length)]);
	}

	// Token: 0x060003CE RID: 974 RVA: 0x0001A067 File Offset: 0x00018267
	public static string NameGetTranslated(AbilityScript.Identifier identifier)
	{
		return AbilityScript.AbilityGet(identifier).NameGetTranslated();
	}

	// Token: 0x060003CF RID: 975 RVA: 0x0001A074 File Offset: 0x00018274
	public static string NameGetKey(AbilityScript.Identifier identifier)
	{
		return AbilityScript.AbilityGet(identifier).nameKey;
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x0001A081 File Offset: 0x00018281
	public static string DescriptionGetTranslated(AbilityScript.Identifier identifier)
	{
		return AbilityScript.AbilityGet(identifier).DescriptionGetTranslated();
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x0001A08E File Offset: 0x0001828E
	public static string ReplyGet(AbilityScript.Identifier identifier, int index)
	{
		return AbilityScript.AbilityGet(identifier).ReplyGet(index);
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x0001A09C File Offset: 0x0001829C
	public static string ReplyGetRandom(AbilityScript.Identifier identifier)
	{
		return AbilityScript.AbilityGet(identifier).ReplyGetRandom();
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x0001A0AC File Offset: 0x000182AC
	private Strings.SanitizationSubKind _GetAbilitySanitizationSubKind()
	{
		AbilityScript.Identifier identifier = this.identifier;
		if (identifier == AbilityScript.Identifier.jackpotIncreaseBase)
		{
			return Strings.SanitizationSubKind.powerup_ShowPatternsValues;
		}
		if (identifier - AbilityScript.Identifier.patternsValue_3LessElements > 1 && identifier - AbilityScript.Identifier.holyPatternsValue_3LessElements > 1)
		{
			return Strings.SanitizationSubKind.none;
		}
		return Strings.SanitizationSubKind.powerup_ShowAllSymbolsAndPatternsValues;
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x0001A0D9 File Offset: 0x000182D9
	public Sprite SpriteGet()
	{
		return this.mySprite;
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x0001A0E1 File Offset: 0x000182E1
	public static Sprite SpriteGet(AbilityScript.Identifier identifier)
	{
		return AbilityScript.AbilityGet(identifier).SpriteGet();
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0001A0EE File Offset: 0x000182EE
	public static Color ColorGet(AbilityScript.Identifier identifier)
	{
		return AbilityScript.AbilityGet(identifier).ColorGet();
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x0001A0FC File Offset: 0x000182FC
	public Color ColorGet()
	{
		switch (this.category)
		{
		case AbilityScript.Category.normal:
			return AbilityScript.C_ORANGE;
		case AbilityScript.Category.evil:
			return Color.red;
		case AbilityScript.Category.good:
			return Color.yellow;
		default:
			Debug.LogError("AbilityScript.ColorGet(): category not handled: " + this.category.ToString());
			return Color.gray;
		}
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0001A15C File Offset: 0x0001835C
	public void Pick()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			Debug.LogError("AbilityScript.Pick(): GameplayData instance is null");
			return;
		}
		AbilityScript.AbilityCallback onAbilityPick = this.OnAbilityPick;
		if (onAbilityPick != null)
		{
			onAbilityPick(this);
		}
		AbilityScript.AbilityCallback onAbilityPick_Static = this.OnAbilityPick_Static;
		if (onAbilityPick_Static != null)
		{
			onAbilityPick_Static(this);
		}
		instance.phoneAbilitiesPickHistory.Add(this.identifier);
		this.pickedTimes++;
		if (this.sound != null)
		{
			if (!Sound.IsPlaying(this.sound.name))
			{
				Sound.Play(this.sound.name, 1f, 1f);
			}
		}
		else
		{
			switch (this.category)
			{
			case AbilityScript.Category.normal:
				if (!Sound.IsPlaying("SoundPhoneSelectAbility"))
				{
					Sound.Play("SoundPhoneSelectAbility", 1f, 1f);
				}
				break;
			case AbilityScript.Category.evil:
				if (!Sound.IsPlaying("SoundPhoneSelectAbilityEvil"))
				{
					Sound.Play("SoundPhoneSelectAbilityEvil", 1f, 1f);
				}
				break;
			case AbilityScript.Category.good:
				if (!Sound.IsPlaying("SoundPhoneSelectAbilityGood"))
				{
					Sound.Play("SoundPhoneSelectAbilityGood", 1f, 1f);
				}
				break;
			default:
				Debug.LogError("AbilityScript.Pick(): Unknown category for sound");
				break;
			}
		}
		CameraGame.Shake(1f);
		if (FlashScreen.instanceLast == null)
		{
			FlashScreen.SpawnCamera(Color.yellow, 0.25f, 2f, CameraGame.firstInstance.myCamera, 0.5f);
		}
		Data.GameData game = Data.game;
		int unlockSteps_Barathrum = game.UnlockSteps_Barathrum;
		game.UnlockSteps_Barathrum = unlockSteps_Barathrum + 1;
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x0001A2E0 File Offset: 0x000184E0
	public static void Pick(AbilityScript.Identifier identifier)
	{
		if (!AbilityScript.dict_All.ContainsKey(identifier))
		{
			Debug.LogError("AbilityScript.Pick(): Ability not found in dictionary");
			return;
		}
		if (AbilityScript.dict_All[identifier] == null)
		{
			Debug.LogError("AbilityScript.Pick(): Ability instance is null");
			return;
		}
		AbilityScript.dict_All[identifier].Pick();
	}

	// Token: 0x060003DA RID: 986 RVA: 0x0001A330 File Offset: 0x00018530
	public bool CanBePicked()
	{
		if (this.maxPickupTimes >= 0 && this.pickedTimes >= this.maxPickupTimes)
		{
			return false;
		}
		AbilityScript.Identifier identifier;
		if (GameplayData.RunModifier_GetCurrent() == RunModifierScript.Identifier.allCharmsStoreModded)
		{
			identifier = this.identifier;
			if (identifier == AbilityScript.Identifier.evilGeneric_ShinyObjects)
			{
				return false;
			}
			if (identifier == AbilityScript.Identifier.holyGeneric_ModifyStoreCharms_Make1Free)
			{
				return false;
			}
		}
		identifier = this.identifier;
		if (identifier != AbilityScript.Identifier.evilGeneric_SpawnSkeletonPiece)
		{
			if (identifier == AbilityScript.Identifier.holyGeneric_SpawnSacredCharm)
			{
				if (PowerupScript.SacredCharm_GetRandom(true, true) == null)
				{
					return false;
				}
			}
		}
		else
		{
			if (RewardBoxScript.GetRewardKind() != RewardBoxScript.RewardKind.DoorKey)
			{
				return false;
			}
			if (!PowerupScript.IsUnlocked(PowerupScript.Identifier.Skeleton_Head) && !PowerupScript.IsUnlocked(PowerupScript.Identifier.Skeleton_Arm1) && !PowerupScript.IsUnlocked(PowerupScript.Identifier.Skeleton_Arm2) && !PowerupScript.IsUnlocked(PowerupScript.Identifier.Skeleton_Leg1) && !PowerupScript.IsUnlocked(PowerupScript.Identifier.Skeleton_Leg2))
			{
				return false;
			}
			if (PowerupScript._SkeletonPiecesSpawnable_GetListReference(true).Count == 0)
			{
				return false;
			}
		}
		return !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Hole_Cross) || this.identifier != GameplayData.PowerupHoleCross_AbilityGet();
	}

	// Token: 0x060003DB RID: 987 RVA: 0x0001A3F8 File Offset: 0x000185F8
	public static bool CanBePicked(AbilityScript.Identifier identifier)
	{
		if (!AbilityScript.dict_All.ContainsKey(identifier))
		{
			Debug.LogError("AbilityScript.CanBePicked(): Ability not found in dictionary");
			return false;
		}
		if (AbilityScript.dict_All[identifier] == null)
		{
			Debug.LogError("AbilityScript.CanBePicked(): Ability instance is null");
			return false;
		}
		return AbilityScript.dict_All[identifier].CanBePicked();
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0001A447 File Offset: 0x00018647
	public bool IsLastPickAvailable()
	{
		return this.maxPickupTimes - this.pickedTimes == 1;
	}

	// Token: 0x060003DD RID: 989 RVA: 0x0001A45C File Offset: 0x0001865C
	public static PowerupScript GetRandomCharmToModify_OnTableOrSlot()
	{
		if (PowerupScript.list_EquippedNormal.Count == 0 && PowerupScript.list_EquippedSkeleton.Count == 0)
		{
			return null;
		}
		int count = PowerupScript.list_EquippedNormal.Count;
		int count2 = PowerupScript.list_EquippedSkeleton.Count;
		if (count > 0)
		{
			int num = R.Rng_Abilities.Range(0, count);
			for (int i = 0; i < count; i++)
			{
				PowerupScript powerupScript = PowerupScript.list_EquippedNormal[num];
				if (!(powerupScript == null) && GameplayData.Powerup_Modifier_Get(powerupScript.identifier) == PowerupScript.Modifier.none)
				{
					return powerupScript;
				}
				num++;
				if (num >= count)
				{
					num = 0;
				}
			}
			return PowerupScript.list_EquippedNormal[R.Rng_Abilities.Range(0, count)];
		}
		if (count2 > 0)
		{
			int num2 = R.Rng_Abilities.Range(0, count2);
			for (int j = 0; j < count2; j++)
			{
				PowerupScript powerupScript2 = PowerupScript.list_EquippedSkeleton[num2];
				if (!(powerupScript2 == null) && GameplayData.Powerup_Modifier_Get(powerupScript2.identifier) == PowerupScript.Modifier.none)
				{
					return powerupScript2;
				}
				num2++;
				if (num2 >= count2)
				{
					num2 = 0;
				}
			}
			return PowerupScript.list_EquippedSkeleton[R.Rng_Abilities.Range(0, count2)];
		}
		return null;
	}

	// Token: 0x060003DE RID: 990 RVA: 0x0001A570 File Offset: 0x00018770
	public static PowerupScript GetRandomCharmToModify_InDrawers()
	{
		bool flag = true;
		int num = PowerupScript.array_InDrawer.Length;
		int num2 = R.Rng_Abilities.Range(0, PowerupScript.array_InDrawer.Length);
		for (int i = 0; i < num; i++)
		{
			PowerupScript powerupScript = PowerupScript.array_InDrawer[num2];
			if (!(powerupScript == null))
			{
				flag = false;
				if (GameplayData.Powerup_Modifier_Get(powerupScript.identifier) == PowerupScript.Modifier.none)
				{
					return powerupScript;
				}
			}
			num2++;
			if (num2 >= num)
			{
				num2 = 0;
			}
		}
		if (flag)
		{
			return null;
		}
		num2 = R.Rng_Abilities.Range(0, PowerupScript.array_InDrawer.Length);
		for (int j = 0; j < num; j++)
		{
			PowerupScript powerupScript2 = PowerupScript.array_InDrawer[num2];
			if (!(powerupScript2 == null))
			{
				return powerupScript2;
			}
			num2++;
			if (num2 >= num)
			{
				num2 = 0;
			}
		}
		return null;
	}

	// Token: 0x060003DF RID: 991 RVA: 0x0001A620 File Offset: 0x00018820
	public static void ApplyDeviousModifierToRandomCharmOnTable()
	{
		PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
		if (randomCharmToModify_OnTableOrSlot != null)
		{
			GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.devious, true);
		}
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x0001A649 File Offset: 0x00018849
	private static void AFunc_OnPick_ExtraSpace(AbilityScript ability)
	{
		GameplayData.MaxEquippablePowerupsAdd(1);
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x0001A651 File Offset: 0x00018851
	private static void AFunc_OnPick_InterestsUp(AbilityScript ability)
	{
		GameplayData.InterestRateAdd(5f);
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x0001A65D File Offset: 0x0001885D
	private static void AFunc_OnPick_JackpotDouble(AbilityScript ability)
	{
		GameplayData.Pattern_ValueExtra_Add(PatternScript.Kind.jackpot, GameplayData.Pattern_ValueOverall_Get(PatternScript.Kind.jackpot, false));
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x0001A66C File Offset: 0x0001886C
	private static void AFunc_OnPick_TicketsPlus5(AbilityScript ability)
	{
		GameplayData.CloverTicketsAdd(5L, true);
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x0001A676 File Offset: 0x00018876
	private static void AFunc_OnPick_Discount2(AbilityScript ability)
	{
		GameplayData.StoreTemporaryDiscountAdd(2L, true);
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x0001A680 File Offset: 0x00018880
	private static void AFunc_OnPick_AddModifierRandomCharm_CloverKind(AbilityScript ability)
	{
		PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
		if (randomCharmToModify_OnTableOrSlot == null)
		{
			return;
		}
		GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.cloverTicket, true);
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x0001A6AC File Offset: 0x000188AC
	private static void AFunc_OnPick_AddModifierRandomCharm_SymbolsMult(AbilityScript ability)
	{
		PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
		if (randomCharmToModify_OnTableOrSlot == null)
		{
			return;
		}
		GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.symbolMultiplier, true);
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x0001A6D8 File Offset: 0x000188D8
	private static void AFunc_OnPick_AddModifierRandomCharm_PatternsMult(AbilityScript ability)
	{
		PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
		if (randomCharmToModify_OnTableOrSlot == null)
		{
			return;
		}
		GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.patternMultiplier, true);
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x0001A704 File Offset: 0x00018904
	private static void AFunc_OnPick_AddModifierRandomCharm_Obsessive(AbilityScript ability)
	{
		PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
		if (randomCharmToModify_OnTableOrSlot == null)
		{
			return;
		}
		GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.obsessive, true);
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x0001A730 File Offset: 0x00018930
	private static void AFunc_OnPick_AddModifierRandomCharm_Gambler(AbilityScript ability)
	{
		PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
		if (randomCharmToModify_OnTableOrSlot == null)
		{
			return;
		}
		GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.gambler, true);
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x0001A75C File Offset: 0x0001895C
	private static void AFunc_OnPick_AddModifierRandomCharm_Speculative(AbilityScript ability)
	{
		PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
		if (randomCharmToModify_OnTableOrSlot == null)
		{
			return;
		}
		GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.speculative, true);
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x0001A786 File Offset: 0x00018986
	private static void AFunc_OnPick_RechargeRedButtonPowerups(AbilityScript ability)
	{
		GameplayData.Powerup_ButtonChargesUsed_ResetAll(true);
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0001A78F File Offset: 0x0001898F
	private static void AFunc_OnPick_SymbolLemonChanceUp(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.lemon, 0.8f);
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x0001A79C File Offset: 0x0001899C
	private static void AFunc_OnPick_SymbolCherryChanceUp(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.cherry, 0.8f);
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x0001A7A9 File Offset: 0x000189A9
	private static void AFunc_OnPick_SymbolCloverChanceUp(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.clover, 0.8f);
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x0001A7B6 File Offset: 0x000189B6
	private static void AFunc_OnPick_SymbolBellChanceUp(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.bell, 0.8f);
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x0001A7C3 File Offset: 0x000189C3
	private static void AFunc_OnPick_SymbolDiamondChanceUp(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.diamond, 0.8f);
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0001A7D0 File Offset: 0x000189D0
	private static void AFunc_OnPick_SymbolCoinsChanceUp(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.coins, 0.8f);
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x0001A7DD File Offset: 0x000189DD
	private static void AFunc_OnPick_SymbolSevenChanceUp(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.seven, 0.8f);
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x0001A7EA File Offset: 0x000189EA
	private static void AFunc_OnPick_SymbolsValue_LemonAndCherry(AbilityScript ability)
	{
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.lemon, GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.lemon));
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.cherry, GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.cherry));
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x0001A804 File Offset: 0x00018A04
	private static void AFunc_OnPick_SymbolsValue_CloverAndBell(AbilityScript ability)
	{
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.clover, GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.clover));
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.bell, GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.bell));
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x0001A81E File Offset: 0x00018A1E
	private static void AFunc_OnPick_SymbolsValue_DiamondAndCoins(AbilityScript ability)
	{
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.diamond, GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.diamond));
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.coins, GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.coins));
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x0001A838 File Offset: 0x00018A38
	private static void AFunc_OnPick_SymbolsValue_Seven(AbilityScript ability)
	{
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.seven, GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.seven) * 2);
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0001A854 File Offset: 0x00018A54
	public static void AFunc_OnPick_PatternsValue_3OrLessElements(AbilityScript ability)
	{
		List<PatternScript.Kind> list = GameplayData.PatternsAvailable_GetAll();
		for (int i = 0; i < list.Count; i++)
		{
			PatternScript.Kind kind = list[i];
			if (PatternScript.GetElementsCount(kind) <= 3)
			{
				GameplayData.Pattern_ValueExtra_Add(kind, GameplayData.Pattern_Value_GetBasic(kind));
			}
		}
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x0001A898 File Offset: 0x00018A98
	public static void AFunc_OnPick_PatternsValue_4OrMoreElements(AbilityScript ability)
	{
		List<PatternScript.Kind> list = GameplayData.PatternsAvailable_GetAll();
		for (int i = 0; i < list.Count; i++)
		{
			PatternScript.Kind kind = list[i];
			if (PatternScript.GetElementsCount(kind) >= 4)
			{
				GameplayData.Pattern_ValueExtra_Add(kind, GameplayData.Pattern_Value_GetBasic(kind));
			}
		}
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x0001A8DC File Offset: 0x00018ADC
	public static void AFunc_OnPick_EvilGeneric_SpawnSkeletonPiece(AbilityScript ability)
	{
		List<PowerupScript.Identifier> list = PowerupScript._SkeletonPiecesSpawnable_GetListReference(true);
		if (list.Count == 0)
		{
			return;
		}
		int num = -1;
		for (int i = 0; i < PowerupScript.array_InDrawer.Length; i++)
		{
			if (PowerupScript.array_InDrawer[i] == null)
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			int num2 = R.Rng_Abilities.Range(0, PowerupScript.array_InDrawer.Length);
			for (int j = 0; j < PowerupScript.array_InDrawer.Length; j++)
			{
				PowerupScript.Identifier identifier = PowerupScript.array_InDrawer[num2].identifier;
				if (identifier > PowerupScript.Identifier.Skeleton_Leg2)
				{
					num = num2;
					break;
				}
				num2++;
				if (num2 >= PowerupScript.array_InDrawer.Length)
				{
					num2 = 0;
				}
			}
		}
		if (PowerupScript.array_InDrawer[num] != null)
		{
			PowerupScript.ThrowAway(PowerupScript.array_InDrawer[num].identifier, false);
		}
		PowerupScript.PutInDrawer(list[0], false, num);
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x0001A9A7 File Offset: 0x00018BA7
	private static void AFunc_OnPick_EvilGeneric_DoubleCloversButMoney0(AbilityScript ability)
	{
		GameplayData.CloverTicketsAdd(GameplayData.CloverTicketsGet(), true);
		GameplayData.CoinsSet(0);
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x0001A9C0 File Offset: 0x00018BC0
	private static void AFunc_OnPick_EvilGeneric_ShinyObjects(AbilityScript ability)
	{
		for (int i = 0; i < StoreCapsuleScript.storePowerups.Length; i++)
		{
			PowerupScript powerupScript = StoreCapsuleScript.storePowerups[i];
			if (!(powerupScript == null) && GameplayData.Powerup_Modifier_Get(powerupScript.identifier) == PowerupScript.Modifier.none && !powerupScript.IsInstantPowerup())
			{
				int num = 8;
				PowerupScript.Modifier modifier = (PowerupScript.Modifier)R.Rng_Abilities.Range(1, num);
				GameplayData.Powerup_Modifier_Set(powerupScript.identifier, modifier, true);
			}
		}
		AbilityScript.ApplyDeviousModifierToRandomCharmOnTable();
		StoreCapsuleScript.RefreshCostTextAll();
	}

	// Token: 0x060003FC RID: 1020 RVA: 0x0001AA2C File Offset: 0x00018C2C
	private static void AFunc_OnPick_EvilGeneric_2FreeItemsTicketsZero(AbilityScript ability)
	{
		AbilityScript._2fiTk0_StorePowerups.Clear();
		for (int i = 0; i < StoreCapsuleScript.storePowerups.Length; i++)
		{
			PowerupScript powerupScript = StoreCapsuleScript.storePowerups[i];
			if (!(powerupScript == null) && StoreCapsuleScript.GetStoreCapsuleById(i).GetCapsuleCost() > 0L)
			{
				AbilityScript._2fiTk0_StorePowerups.Add(powerupScript);
			}
		}
		if (AbilityScript._2fiTk0_StorePowerups.Count < 2)
		{
			for (int j = 0; j < StoreCapsuleScript.storePowerups.Length; j++)
			{
				PowerupScript powerupScript = StoreCapsuleScript.storePowerups[j];
				if (!(powerupScript == null) && !AbilityScript._2fiTk0_StorePowerups.Contains(powerupScript))
				{
					AbilityScript._2fiTk0_StorePowerups.Add(powerupScript);
				}
			}
		}
		if (AbilityScript._2fiTk0_StorePowerups.Count != 0)
		{
			int num = R.Rng_Abilities.Range(0, AbilityScript._2fiTk0_StorePowerups.Count);
			PowerupScript powerupScript = AbilityScript._2fiTk0_StorePowerups[num];
			GameplayData.StoreTemporaryDiscountPerSlotSet_PerPowerup(powerupScript.identifier, 9999L, true);
			AbilityScript._2fiTk0_StorePowerups.RemoveAt(num);
			if (AbilityScript._2fiTk0_StorePowerups.Count > 0)
			{
				num = R.Rng_Abilities.Range(0, AbilityScript._2fiTk0_StorePowerups.Count);
				powerupScript = AbilityScript._2fiTk0_StorePowerups[num];
				GameplayData.StoreTemporaryDiscountPerSlotSet_PerPowerup(powerupScript.identifier, 9999L, true);
				AbilityScript._2fiTk0_StorePowerups.RemoveAt(num);
			}
		}
		GameplayData.CloverTicketsSet(0L);
	}

	// Token: 0x060003FD RID: 1021 RVA: 0x0001AB70 File Offset: 0x00018D70
	private static void AFunc_OnPick_EvilGeneric_TakeOtherAbilitiesThenDeviousMod(AbilityScript ability)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		for (int i = 0; i < instance._phone_AbilitiesToPick.Count; i++)
		{
			AbilityScript.Identifier identifier = instance._phone_AbilitiesToPick[i];
			if (instance._phone_AbilitiesToPick[i] != ability.identifier && PhoneAbilityUiScript.allAbilities[i].gameObject.activeSelf)
			{
				AbilityScript.Pick(instance._phone_AbilitiesToPick[i]);
			}
		}
		AbilityScript.ApplyDeviousModifierToRandomCharmOnTable();
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x0001ABEB File Offset: 0x00018DEB
	private static void AFunc_OnPick_EvilGeneric_DoubleCoinsTicketsZero(AbilityScript ability)
	{
		GameplayData.CoinsAdd(GameplayData.CoinsGet(), true);
		GameplayData.CloverTicketsSet(0L);
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x0001ABFF File Offset: 0x00018DFF
	public static void AFunc_OnPick_EvilHalvenChances_LemonsAndCherries(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.lemon, -(GameplayData.Symbol_Chance_Get(SymbolScript.Kind.lemon, false, false) / 2f));
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.cherry, -(GameplayData.Symbol_Chance_Get(SymbolScript.Kind.cherry, false, false) / 2f));
		AbilityScript.ApplyDeviousModifierToRandomCharmOnTable();
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0001AC30 File Offset: 0x00018E30
	public static void AFunc_OnPick_EvilHalvenChances_CloversAndBells(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.clover, -(GameplayData.Symbol_Chance_Get(SymbolScript.Kind.clover, false, false) / 2f));
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.bell, -(GameplayData.Symbol_Chance_Get(SymbolScript.Kind.bell, false, false) / 2f));
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0001AC5C File Offset: 0x00018E5C
	public static void AFunc_OnPick_EvilHalvenChances_DiamondCoinsAndSeven(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.diamond, -(GameplayData.Symbol_Chance_Get(SymbolScript.Kind.diamond, false, false) / 2f));
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.coins, -(GameplayData.Symbol_Chance_Get(SymbolScript.Kind.coins, false, false) / 2f));
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.seven, -(GameplayData.Symbol_Chance_Get(SymbolScript.Kind.seven, false, false) / 2f));
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x0001ACA8 File Offset: 0x00018EA8
	private static void AFunc_OnPick_HolyGeneric_SymbolsMultiplier3(AbilityScript ability)
	{
		GameplayData.AllSymbolsMultiplierAdd(3);
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x0001ACB5 File Offset: 0x00018EB5
	private static void AFunc_OnPick_HolyGeneric_PatternsMultiplier1(AbilityScript ability)
	{
		GameplayData.AllPatternsMultiplierAdd(1);
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x0001ACC4 File Offset: 0x00018EC4
	private static void AFunc_OnPick_HolyGeneric_ReduceCharmsChargesNeeded(AbilityScript ability)
	{
		List<PowerupScript> list = RedButtonScript.RegisteredPowerupsGet();
		for (int i = 0; i < list.Count; i++)
		{
			PowerupScript powerupScript = list[i];
			if (!(powerupScript == null) && powerupScript.identifier != PowerupScript.Identifier.undefined && powerupScript.identifier != PowerupScript.Identifier.count)
			{
				int num = GameplayData.Powerup_ButtonChargesMax_Get(powerupScript.identifier);
				num--;
				num = Mathf.Max(1, num);
				GameplayData.Powerup_ButtonChargesMax_Set(powerupScript.identifier, num, true, true);
			}
		}
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x0001AD38 File Offset: 0x00018F38
	private static void AFunc_OnPick_HolyGeneric_ModifyAllStoreCharmsMake1Free(AbilityScript ability)
	{
		for (int i = 0; i < StoreCapsuleScript.storePowerups.Length; i++)
		{
			PowerupScript powerupScript = StoreCapsuleScript.storePowerups[i];
			if (!(powerupScript == null) && GameplayData.Powerup_Modifier_Get(powerupScript.identifier) == PowerupScript.Modifier.none)
			{
				int num = 8;
				PowerupScript.Modifier modifier = (PowerupScript.Modifier)R.Rng_Abilities.Range(1, num);
				GameplayData.Powerup_Modifier_Set(powerupScript.identifier, modifier, true);
			}
		}
		AbilityScript._ModifyAllStoreCharmsMake1Free_StorePowerups.Clear();
		for (int j = 0; j < StoreCapsuleScript.storePowerups.Length; j++)
		{
			PowerupScript powerupScript2 = StoreCapsuleScript.storePowerups[j];
			if (!(powerupScript2 == null))
			{
				AbilityScript._ModifyAllStoreCharmsMake1Free_StorePowerups.Add(powerupScript2);
			}
		}
		int num2 = R.Rng_Abilities.Range(0, AbilityScript._ModifyAllStoreCharmsMake1Free_StorePowerups.Count);
		GameplayData.StoreTemporaryDiscountPerSlotSet_PerPowerup(AbilityScript._ModifyAllStoreCharmsMake1Free_StorePowerups[num2].identifier, 9999L, true);
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x0001AE05 File Offset: 0x00019005
	private static void AFunc_OnPick_HolyGeneric_PatternsRepetitionIncrease(AbilityScript ability)
	{
		GameplayData.AbilityHoly_PatternsRepetitions++;
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x0001AE14 File Offset: 0x00019014
	private static void AFunc_OnPick_SpawnSacredCharm(AbilityScript ability)
	{
		PowerupScript powerupScript = PowerupScript.SacredCharm_GetRandom(true, true);
		if (powerupScript == null)
		{
			return;
		}
		PowerupScript.EquipFlag_IgnoreSpaceCondition();
		PowerupScript.Equip(powerupScript.identifier, false, false);
		PowerupScript.Unlock(powerupScript.identifier);
		if (GameplayData.CoinsGet() > 0L)
		{
			GameplayData.CoinsSet(GameplayData.CoinsGet() / 2);
		}
		PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.DoNotBeAfraid);
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x0001AE80 File Offset: 0x00019080
	public static void AFunc_OnPick_HolyPatternsValue_3OrLessElements(AbilityScript ability)
	{
		List<PatternScript.Kind> list = GameplayData.PatternsAvailable_GetAll();
		for (int i = 0; i < list.Count; i++)
		{
			PatternScript.Kind kind = list[i];
			if (PatternScript.GetElementsCount(kind) <= 3)
			{
				GameplayData.Pattern_ValueExtra_Add(kind, GameplayData.Pattern_ValueOverall_Get(kind, false));
			}
		}
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x0001AEC4 File Offset: 0x000190C4
	public static void AFunc_OnPick_HolyPatternsValue_4OrMoreElements(AbilityScript ability)
	{
		List<PatternScript.Kind> list = GameplayData.PatternsAvailable_GetAll();
		for (int i = 0; i < list.Count; i++)
		{
			PatternScript.Kind kind = list[i];
			if (PatternScript.GetElementsCount(kind) >= 4)
			{
				GameplayData.Pattern_ValueExtra_Add(kind, GameplayData.Pattern_ValueOverall_Get(kind, false));
			}
		}
	}

	// Token: 0x0600040A RID: 1034 RVA: 0x0001AF08 File Offset: 0x00019108
	private void Initialize(AbilityScript.Identifier identifier, AbilityScript.Category category, AbilityScript.Archetype archetype, AbilityScript.Rarity rarity, string nameKey, string descrKey, string[] replies, string spriteName, string soundName, int maxPickupTimes, AbilityScript.AbilityCallback onAbilityPick)
	{
		this.identifier = identifier;
		this.category = category;
		this.archetype = archetype;
		this.rarity = rarity;
		this.nameKey = nameKey;
		this.descriptionKey = descrKey;
		this.repliesKeys = replies;
		this.mySprite = AssetMaster.GetSprite(spriteName);
		if (!string.IsNullOrEmpty(soundName))
		{
			this.sound = AssetMaster.GetSound(soundName);
		}
		else
		{
			this.sound = null;
		}
		this.maxPickupTimes = maxPickupTimes;
		this.OnAbilityPick = onAbilityPick;
		AbilityScript.list_All.Add(this);
		AbilityScript.dict_All.Add(identifier, this);
		AbilityScript.dict_ListsByCategory[category].Add(this);
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x0001AFB0 File Offset: 0x000191B0
	public static void InitializeAll(bool isNewGame)
	{
		AbilityScript.list_All.Clear();
		AbilityScript.dict_All.Clear();
		AbilityScript.dict_ListsByCategory.Clear();
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			AbilityScript.dict_ListsByCategory.Add((AbilityScript.Category)i, new List<AbilityScript>());
		}
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			Debug.LogError("AbilityScript.InitializeAll(): GameplayData instance is null");
			return;
		}
		new AbilityScript().Initialize(AbilityScript.Identifier.extraSpace, AbilityScript.Category.normal, AbilityScript.Archetype.normal, AbilityScript.Rarity.uncommon, "ABILITY_NAME_CHARMS_EXTRA_SPACE", "ABILITY_DESCR_CHARMS_EXTRA_SPACE", new string[] { "ABILITY_REPLY_CHARMS_EXTRA_SPACE" }, "SpriteAbility_Generic_ExtraSpace", null, 1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_ExtraSpace));
		new AbilityScript().Initialize(AbilityScript.Identifier.jackpotIncreaseBase, AbilityScript.Category.normal, AbilityScript.Archetype.normal, AbilityScript.Rarity.common, "ABILITY_NAME_INCREASE_JACKPOT_VALUE", "ABILITY_DESCR_INCREASE_JACKPOT_VALUE", new string[] { "ABILITY_REPLY_INCREASE_JACKPOT_VALUE" }, "SpriteAbility_Generic_JackpotIncreaseBase", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_JackpotDouble));
		new AbilityScript().Initialize(AbilityScript.Identifier.ticketsPlus5, AbilityScript.Category.normal, AbilityScript.Archetype.normal, AbilityScript.Rarity.common, "ABILITY_NAME_EXTRA_TICKETS_5", "ABILITY_DESCR_EXTRA_TICKETS_5", new string[] { "ABILITY_REPLY_EXTRA_TICKETS_5" }, "SpriteAbility_Generic_5ExtraTickets", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_TicketsPlus5));
		new AbilityScript().Initialize(AbilityScript.Identifier.discount1, AbilityScript.Category.normal, AbilityScript.Archetype.normal, AbilityScript.Rarity.common, "ABILITY_NAME_TEMPORARY_DISCOUNT_2", "ABILITY_DESCR_TEMPORARY_DISCOUNT_2", new string[] { "ABILITY_REPLY_TEMPORARY_DISCOUNT_2" }, "SpriteAbility_Generic_Discount1", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_Discount2));
		new AbilityScript().Initialize(AbilityScript.Identifier.rndCharmMod_Clover, AbilityScript.Category.normal, AbilityScript.Archetype.normal, AbilityScript.Rarity.rare, "ABILITY_NAME_MODIFY_RANDOM_CHARM___TICKETS", "ABILITY_DESCR_MODIFY_RANDOM_CHARM___TICKETS", new string[] { "ABILITY_REPLY_MODIFY_RANDOM_CHARM___TICKETS" }, "SpriteAbility_Generic_ModRndCharm_Tickets", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_AddModifierRandomCharm_CloverKind));
		new AbilityScript().Initialize(AbilityScript.Identifier.rndCharmMod_SymbMult, AbilityScript.Category.normal, AbilityScript.Archetype.normal, AbilityScript.Rarity.ultraRare, "ABILITY_NAME_MODIFY_RANDOM_CHARM___SYMB_MULT", "ABILITY_DESCR_MODIFY_RANDOM_CHARM___SYMB_MULT", new string[] { "ABILITY_REPLY_MODIFY_RANDOM_CHARM___SYMB_MULT" }, "SpriteAbility_Generic_ModRndCharm_SymbMult", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_AddModifierRandomCharm_SymbolsMult));
		new AbilityScript().Initialize(AbilityScript.Identifier.rndCharmMod_PatMult, AbilityScript.Category.normal, AbilityScript.Archetype.normal, AbilityScript.Rarity.legendary, "ABILITY_NAME_MODIFY_RANDOM_CHARM___PAT_MULT", "ABILITY_DESCR_MODIFY_RANDOM_CHARM___PAT_MULT", new string[] { "ABILITY_REPLY_MODIFY_RANDOM_CHARM___PAT_MULT" }, "SpriteAbility_Generic_ModRndCharm_PatMult", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_AddModifierRandomCharm_PatternsMult));
		new AbilityScript().Initialize(AbilityScript.Identifier.rndCharmMod_Obsessive, AbilityScript.Category.normal, AbilityScript.Archetype.normal, AbilityScript.Rarity.legendary, "ABILITY_NAME_MODIFY_RANDOM_CHARM___OBSESSIVE", "ABILITY_DESCR_MODIFY_RANDOM_CHARM___OBSESSIVE", new string[] { "ABILITY_REPLY_MODIFY_RANDOM_CHARM___OBSESSIVE" }, "SpriteAbility_Generic_ModRndCharm_Obsessive", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_AddModifierRandomCharm_Obsessive));
		new AbilityScript().Initialize(AbilityScript.Identifier.rndCharmMod_Gambler, AbilityScript.Category.normal, AbilityScript.Archetype.normal, AbilityScript.Rarity.rare, "ABILITY_NAME_MODIFY_RANDOM_CHARM___GAMBLING", "ABILITY_DESCR_MODIFY_RANDOM_CHARM___GAMBLING", new string[] { "ABILITY_REPLY_MODIFY_RANDOM_CHARM___GAMBLING" }, "SpriteAbility_Generic_ModRndCharm_Gambler", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_AddModifierRandomCharm_Gambler));
		new AbilityScript().Initialize(AbilityScript.Identifier.rndCharmMod_Speculative, AbilityScript.Category.normal, AbilityScript.Archetype.normal, AbilityScript.Rarity.ultraRare, "ABILITY_NAME_MODIFY_RANDOM_CHARM___SPECULATIVE", "ABILITY_DESCR_MODIFY_RANDOM_CHARM___SPECULATIVE", new string[] { "ABILITY_REPLY_MODIFY_RANDOM_CHARM___SPECULATIVE" }, "SpriteAbility_Generic_ModRndCharm_Speculative", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_AddModifierRandomCharm_Speculative));
		new AbilityScript().Initialize(AbilityScript.Identifier.rechargeRedButtonPowerups, AbilityScript.Category.normal, AbilityScript.Archetype.normal, AbilityScript.Rarity.common, "ABILITY_NAME_RECHARGE_RED_BUTTON_ITEMS", "ABILITY_DESCR_RECHARGE_RED_BUTTON_ITEMS", new string[] { "ABILITY_REPLY_RECHARGE_RED_BUTTON_ITEMS" }, "SpriteAbility_Generic_RechargeRedButtonItems", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_RechargeRedButtonPowerups));
		new AbilityScript().Initialize(AbilityScript.Identifier.symbolChances_Lemon, AbilityScript.Category.normal, AbilityScript.Archetype.symbolChances, AbilityScript.Rarity.common, "ABILITY_NAME_CHANCES_LEMON", "ABILITY_DESCR_CHANCES_LEMON", new string[] { "ABILITY_REPLY_CHANCES_LEMON" }, "SpriteAbility_ChancesLemon", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_SymbolLemonChanceUp));
		new AbilityScript().Initialize(AbilityScript.Identifier.symbolChances_Cherry, AbilityScript.Category.normal, AbilityScript.Archetype.symbolChances, AbilityScript.Rarity.common, "ABILITY_NAME_CHANCES_CHERRY", "ABILITY_DESCR_CHANCES_CHERRY", new string[] { "ABILITY_REPLY_CHANCES_CHERRY" }, "SpriteAbility_ChancesCherry", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_SymbolCherryChanceUp));
		new AbilityScript().Initialize(AbilityScript.Identifier.symbolChances_Clover, AbilityScript.Category.normal, AbilityScript.Archetype.symbolChances, AbilityScript.Rarity.uncommon, "ABILITY_NAME_CHANCES_CLOVER", "ABILITY_DESCR_CHANCES_CLOVER", new string[] { "ABILITY_REPLY_CHANCES_CLOVER" }, "SpriteAbility_ChancesClover", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_SymbolCloverChanceUp));
		new AbilityScript().Initialize(AbilityScript.Identifier.symbolChances_Bell, AbilityScript.Category.normal, AbilityScript.Archetype.symbolChances, AbilityScript.Rarity.uncommon, "ABILITY_NAME_CHANCES_BELL", "ABILITY_DESCR_CHANCES_BELL", new string[] { "ABILITY_REPLY_CHANCES_BELL" }, "SpriteAbility_ChancesBell", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_SymbolBellChanceUp));
		new AbilityScript().Initialize(AbilityScript.Identifier.symbolChances_Diamond, AbilityScript.Category.normal, AbilityScript.Archetype.symbolChances, AbilityScript.Rarity.rare, "ABILITY_NAME_CHANCES_DIAMOND", "ABILITY_DESCR_CHANCES_DIAMOND", new string[] { "ABILITY_REPLY_CHANCES_DIAMOND" }, "SpriteAbility_ChancesDiamond", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_SymbolDiamondChanceUp));
		new AbilityScript().Initialize(AbilityScript.Identifier.symbolChances_Coins, AbilityScript.Category.normal, AbilityScript.Archetype.symbolChances, AbilityScript.Rarity.rare, "ABILITY_NAME_CHANCES_COINS", "ABILITY_DESCR_CHANCES_COINS", new string[] { "ABILITY_REPLY_CHANCES_COINS" }, "SpriteAbility_ChancesCoins", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_SymbolCoinsChanceUp));
		new AbilityScript().Initialize(AbilityScript.Identifier.symbolChances_Seven, AbilityScript.Category.normal, AbilityScript.Archetype.symbolChances, AbilityScript.Rarity.ultraRare, "ABILITY_NAME_CHANCES_SEVEN", "ABILITY_DESCR_CHANCES_SEVEN", new string[] { "ABILITY_REPLY_CHANCES_SEVEN" }, "SpriteAbility_ChancesSeven", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_SymbolSevenChanceUp));
		new AbilityScript().Initialize(AbilityScript.Identifier.symbolsValue_LemonAndCherry, AbilityScript.Category.normal, AbilityScript.Archetype.symbolsValue, AbilityScript.Rarity.common, "ABILITY_NAME_LEMONS_CHERRY_DOUBLE_ONE", "ABILITY_DESCR_LEMONS_CHERRY_DOUBLE_ONE", new string[] { "ABILITY_REPLY_LEMONS_CHERRY_DOUBLE_ONE" }, "SpriteAbility_SymbolsValue_LemonOrCherryDoubleOne", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_SymbolsValue_LemonAndCherry));
		new AbilityScript().Initialize(AbilityScript.Identifier.symbolsValue_CloverAndBell, AbilityScript.Category.normal, AbilityScript.Archetype.symbolsValue, AbilityScript.Rarity.common, "ABILITY_NAME_CLOVERS_BELLS_DOUBLE_ONE", "ABILITY_DESCR_CLOVERS_BELLS_DOUBLE_ONE", new string[] { "ABILITY_REPLY_CLOVERS_BELLS_DOUBLE_ONE" }, "SpriteAbility_SymbolsValue_CloverOrBellDoubleOne", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_SymbolsValue_CloverAndBell));
		new AbilityScript().Initialize(AbilityScript.Identifier.symbolsValue_DiamondAndCoins, AbilityScript.Category.normal, AbilityScript.Archetype.symbolsValue, AbilityScript.Rarity.uncommon, "ABILITY_NAME_DIAMONDS_COINS_DOUBLE_ONE", "ABILITY_DESCR_DIAMONDS_COINS_DOUBLE_ONE", new string[] { "ABILITY_REPLY_DIAMONDS_COINS_DOUBLE_ONE" }, "SpriteAbility_SymbolsValue_DiamondsOrCoinsDoubleOne", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_SymbolsValue_DiamondAndCoins));
		new AbilityScript().Initialize(AbilityScript.Identifier.symbolsValue_Seven, AbilityScript.Category.normal, AbilityScript.Archetype.symbolsValue, AbilityScript.Rarity.uncommon, "ABILITY_NAME_SEVEN_DOUBLE", "ABILITY_DESCR_SEVEN_DOUBLE", new string[] { "ABILITY_REPLY_SEVEN_DOUBLE" }, "SpriteAbility_SymbolsValue_SevenDouble", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_SymbolsValue_Seven));
		new AbilityScript().Initialize(AbilityScript.Identifier.patternsValue_3LessElements, AbilityScript.Category.normal, AbilityScript.Archetype.patternsValue, AbilityScript.Rarity.uncommon, "ABILITY_NAME_PATTERNS_3LESS_INCREASE_BASE", "ABILITY_DESCR_PATTERNS_3LESS_INCREASE_BASE", new string[] { "ABILITY_REPLY_PATTERNS_3LESS_INCREASE_BASE" }, "SpriteAbility_PatternsValue_3LessIncreaseBase", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_PatternsValue_3OrLessElements));
		new AbilityScript().Initialize(AbilityScript.Identifier.patternsValue_4MoreElements, AbilityScript.Category.normal, AbilityScript.Archetype.patternsValue, AbilityScript.Rarity.uncommon, "ABILITY_NAME_PATTERNS_4MORE_INCREASE_BASE", "ABILITY_DESCR_PATTERNS_4MORE_INCREASE_BASE", new string[] { "ABILITY_REPLY_PATTERNS_4MORE_INCREASE_BASE" }, "SpriteAbility_PatternsValue_4MoreIncreaseBase", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_PatternsValue_4OrMoreElements));
		new AbilityScript().Initialize(AbilityScript.Identifier.evilGeneric_ShinyObjects, AbilityScript.Category.evil, AbilityScript.Archetype.normal, AbilityScript.Rarity.ultraRare, "ABILITY_NAME_EVIL_SHINY_OBJECTS", "ABILITY_DESCR_EVIL_SHINY_OBJECTS", new string[] { "ABILITY_REPLY_EVIL_SHINY_OBJECTS" }, "SpriteAbility_EvilGeneric_ShinyObjects", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_EvilGeneric_ShinyObjects));
		new AbilityScript().Initialize(AbilityScript.Identifier.evilGeneric_DoubleCloversButMoney0, AbilityScript.Category.evil, AbilityScript.Archetype.normal, AbilityScript.Rarity.common, "ABILITY_NAME_EVIL_DOUBLE_CLOVERS_BUT_MONEY_0", "ABILITY_DESCR_EVIL_DOUBLE_CLOVERS_BUT_MONEY_0", new string[] { "ABILITY_REPLY_EVIL_DOUBLE_CLOVERS_BUT_MONEY_0" }, "SpriteAbility_EvilGeneric_DoubleCloversButMoney0", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_EvilGeneric_DoubleCloversButMoney0));
		new AbilityScript().Initialize(AbilityScript.Identifier.evilGeneric_2FreeItemsTicketsZero, AbilityScript.Category.evil, AbilityScript.Archetype.normal, AbilityScript.Rarity.common, "ABILITY_NAME_EVIL_2_FREE_ITEMS_TICKETS_ZERO", "ABILITY_DESCR_EVIL_2_FREE_ITEMS_TICKETS_ZERO", new string[] { "ABILITY_REPLY_EVIL_2_FREE_ITEMS_TICKETS_ZERO" }, "SpriteAbility_EvilGeneric_2FreeItemsTicketsZero", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_EvilGeneric_2FreeItemsTicketsZero));
		new AbilityScript().Initialize(AbilityScript.Identifier.evilGeneric_TakeOtherAbilitiesButDeviousMod, AbilityScript.Category.evil, AbilityScript.Archetype.normal, AbilityScript.Rarity.ultraRare, "ABILITY_NAME_EVIL_OTHER_ABILITIES_NO_CHARMS", "ABILITY_DESCR_EVIL_OTHER_ABILITIES_NO_CHARMS", new string[] { "ABILITY_REPLY_EVIL_OTHER_ABILITIES_NO_CHARMS" }, "SpriteAbility_EvilGeneric_OtherAbilitiesThenDeviousMod", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_EvilGeneric_TakeOtherAbilitiesThenDeviousMod));
		new AbilityScript().Initialize(AbilityScript.Identifier.evilGeneric_DoubleCoinsTicketsZero, AbilityScript.Category.evil, AbilityScript.Archetype.normal, AbilityScript.Rarity.common, "ABILITY_NAME_EVIL_DOUBLE_COINS_TICKETS_ZERO", "ABILITY_DESCR_EVIL_DOUBLE_COINS_TICKETS_ZERO", new string[] { "ABILITY_REPLY_EVIL_DOUBLE_COINS_TICKETS_ZERO" }, "SpriteAbility_EvilGeneric_DoubleCoinsTicketsZero", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_EvilGeneric_DoubleCoinsTicketsZero));
		new AbilityScript().Initialize(AbilityScript.Identifier.evilHalvenChances_LemonAndCherry, AbilityScript.Category.evil, AbilityScript.Archetype.normal, AbilityScript.Rarity.ultraRare, "ABILITY_NAME_EVIL_HALVEN_CHANCES_LEMONS_CHERRIES", "ABILITY_DESCR_EVIL_HALVEN_CHANCES_LEMONS_CHERRIES", new string[] { "ABILITY_REPLY_EVIL_HALVEN_CHANCES_LEMONS_CHERRIES" }, "SpriteAbility_EvilHalvenChances_LemonsAndCherries", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_EvilHalvenChances_LemonsAndCherries));
		new AbilityScript().Initialize(AbilityScript.Identifier.evilHalvenChances_CloverAndBell, AbilityScript.Category.evil, AbilityScript.Archetype.normal, AbilityScript.Rarity.rare, "ABILITY_NAME_EVIL_HALVEN_CHANCES_CLOVERS_BELLS", "ABILITY_DESCR_EVIL_HALVEN_CHANCES_CLOVERS_BELLS", new string[] { "ABILITY_REPLY_EVIL_HALVEN_CHANCES_CLOVERS_BELLS" }, "SpriteAbility_EvilHalvenChances_CloversAndBells", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_EvilHalvenChances_CloversAndBells));
		new AbilityScript().Initialize(AbilityScript.Identifier.evilHalvenChances_DiamondCoinsAndSeven, AbilityScript.Category.evil, AbilityScript.Archetype.normal, AbilityScript.Rarity.uncommon, "ABILITY_NAME_EVIL_HALVEN_CHANCES_DIAMOND_COINS_SEVEN", "ABILITY_DESCR_EVIL_HALVEN_CHANCES_DIAMOND_COINS_SEVEN", new string[] { "ABILITY_REPLY_EVIL_HALVEN_CHANCES_DIAMOND_COINS_SEVEN" }, "SpriteAbility_EvilHalvenChances_DiamondsCoinsAndSevens", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_EvilHalvenChances_DiamondCoinsAndSeven));
		new AbilityScript().Initialize(AbilityScript.Identifier.holyGeneric_MultiplierSymbols_1, AbilityScript.Category.good, AbilityScript.Archetype.normal, AbilityScript.Rarity.common, "ABILITY_NAME_HOLY_SYMBOLS_MULTIPLIER_PLUS_3", "ABILITY_DESCR_HOLY_SYMBOLS_MULTIPLIER_PLUS_3", new string[] { "ABILITY_REPLY_HOLY_SYMBOLS_MULTIPLIER_PLUS_1" }, "SpriteAbility_HolyGeneric_SymbolsMultiplierPlus1", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_HolyGeneric_SymbolsMultiplier3));
		new AbilityScript().Initialize(AbilityScript.Identifier.holyGeneric_MultiplierPatterns_1, AbilityScript.Category.good, AbilityScript.Archetype.normal, AbilityScript.Rarity.common, "ABILITY_NAME_HOLY_PATTERNS_MULTIPLIER_PLUS_1", "ABILITY_DESCR_HOLY_PATTERNS_MULTIPLIER_PLUS_1", new string[] { "ABILITY_REPLY_HOLY_PATTERNS_MULTIPLIER_PLUS_1" }, "SpriteAbility_HolyGeneric_PatternsMultiplierPlus1", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_HolyGeneric_PatternsMultiplier1));
		new AbilityScript().Initialize(AbilityScript.Identifier.holyGeneric_ReduceChargesNeeded_ForRedButtonCharms, AbilityScript.Category.good, AbilityScript.Archetype.normal, AbilityScript.Rarity.rare, "ABILITY_NAME_HOLY_REDUCE_CHARGES_NEEDED_REDBUTTON_CHARMS", "ABILITY_DESCR_HOLY_REDUCE_CHARGES_NEEDED_REDBUTTON_CHARMS", new string[] { "ABILITY_REPLY_HOLY_REDUCE_CHARGES_NEEDED_REDBUTTON_CHARMS" }, "SpriteAbility_HolyGeneric_ReduceChargesNeededOnRedButtonCharms", null, 2, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_HolyGeneric_ReduceCharmsChargesNeeded));
		new AbilityScript().Initialize(AbilityScript.Identifier.holyGeneric_ModifyStoreCharms_Make1Free, AbilityScript.Category.good, AbilityScript.Archetype.normal, AbilityScript.Rarity.uncommon, "ABILITY_NAME_HOLY_MODIFY_CHARMS_PLUS_1_FREE", "ABILITY_DESCR_HOLY_MODIFY_CHARMS_PLUS_1_FREE", new string[] { "ABILITY_REPLY_HOLY_MODIFY_CHARMS_PLUS_1_FREE" }, "SpriteAbility_HolyGeneric_ModifyCharmsPlus1Free", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_HolyGeneric_ModifyAllStoreCharmsMake1Free));
		new AbilityScript().Initialize(AbilityScript.Identifier.holyGeneric_PatternsRepetitionIncrase, AbilityScript.Category.good, AbilityScript.Archetype.normal, AbilityScript.Rarity.legendary, "ABILITY_NAME_HOLY_REPETITIONS_INCREASE_1", "ABILITY_DESCR_HOLY_REPETITIONS_INCREASE_1", new string[] { "ABILITY_REPLY_HOLY_REPETITIONS_INCREASE_1" }, "SpriteAbility_HolyGeneric_PatternsRepetitionPlus1", null, 1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_HolyGeneric_PatternsRepetitionIncrease));
		new AbilityScript().Initialize(AbilityScript.Identifier.holyGeneric_SpawnSacredCharm, AbilityScript.Category.good, AbilityScript.Archetype.normal, AbilityScript.Rarity.legendary, "ABILITY_NAME_HOLY_SPAWN_RANDOM_999_CHARM", "ABILITY_DESCR_HOLY_SPAWN_RANDOM_999_CHARM", new string[] { "ABILITY_REPLY_HOLY_SPAWN_RANDOM_999_CHARM" }, "SpriteAbility_HolyGeneric_SpawnSacredCharm", "SoundPhoneHolyTransformation", -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_SpawnSacredCharm));
		new AbilityScript().Initialize(AbilityScript.Identifier.holyPatternsValue_3LessElements, AbilityScript.Category.good, AbilityScript.Archetype.patternsValue, AbilityScript.Rarity.uncommon, "ABILITY_NAME_HOLY_DOUBLE_3LESS_PATTERNS", "ABILITY_DESCR_HOLY_DOUBLE_3LESS_PATTERNS", new string[] { "ABILITY_REPLY_HOLY_DOUBLE_3LESS_PATTERNS" }, "SpriteAbility_HolyPatternsValue_Double3Less", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_HolyPatternsValue_3OrLessElements));
		new AbilityScript().Initialize(AbilityScript.Identifier.holyPatternsValue_4MoreElements, AbilityScript.Category.good, AbilityScript.Archetype.patternsValue, AbilityScript.Rarity.uncommon, "ABILITY_NAME_HOLY_DOUBLE_4MORE_PATTERNS", "ABILITY_DESCR_HOLY_DOUBLE_4MORE_PATTERNS", new string[] { "ABILITY_REPLY_HOLY_DOUBLE_4MORE_PATTERNS" }, "SpriteAbility_HolyPatternsValue_Double4More", null, -1, new AbilityScript.AbilityCallback(AbilityScript.AFunc_OnPick_HolyPatternsValue_4OrMoreElements));
		if (!isNewGame)
		{
			for (int j = 0; j < instance.phoneAbilitiesPickHistory.Count; j++)
			{
				if (!AbilityScript.dict_All.ContainsKey(instance.phoneAbilitiesPickHistory[j]))
				{
					Debug.LogError("AbilityScript.InitializeAll(): Ability not found in dictionary");
				}
				else if (AbilityScript.dict_All[instance.phoneAbilitiesPickHistory[j]] == null)
				{
					Debug.LogError("AbilityScript.InitializeAll(): Ability instance is null");
				}
				else
				{
					AbilityScript.dict_All[instance.phoneAbilitiesPickHistory[j]].pickedTimes++;
				}
			}
		}
	}

	public static List<AbilityScript> list_All = new List<AbilityScript>();

	public static Dictionary<AbilityScript.Identifier, AbilityScript> dict_All = new Dictionary<AbilityScript.Identifier, AbilityScript>();

	public static Dictionary<AbilityScript.Category, List<AbilityScript>> dict_ListsByCategory = new Dictionary<AbilityScript.Category, List<AbilityScript>>();

	private const int PLAYER_INDEX = 0;

	private const int MAX_PICKUP_TIMES_INFINITE = -1;

	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	private AbilityScript.Identifier identifier;

	private AbilityScript.Category category;

	private AbilityScript.Archetype archetype;

	private AbilityScript.Rarity rarity;

	private string nameKey;

	private string descriptionKey;

	private string[] repliesKeys;

	private Sprite mySprite;

	private AudioClip sound;

	private int maxPickupTimes = -1;

	private int pickedTimes;

	public AbilityScript.AbilityCallback OnAbilityPick;

	public AbilityScript.AbilityCallback OnAbilityPick_Static;

	private static List<PowerupScript> _2fiTk0_StorePowerups = new List<PowerupScript>();

	private static List<PowerupScript> _ModifyAllStoreCharmsMake1Free_StorePowerups = new List<PowerupScript>();

	private static List<AbilityScript> _tempInstList = new List<AbilityScript>();

	public enum Identifier
	{
		undefined = -1,
		extraSpace,
		interestsUp,
		jackpotIncreaseBase,
		ticketsPlus5,
		discount1,
		rndCharmMod_Clover,
		rndCharmMod_SymbMult,
		rndCharmMod_PatMult,
		rndCharmMod_Obsessive,
		rndCharmMod_Gambler,
		rndCharmMod_Speculative,
		rechargeRedButtonPowerups,
		symbolChances_Lemon,
		symbolChances_Cherry,
		symbolChances_Clover,
		symbolChances_Bell,
		symbolChances_Diamond,
		symbolChances_Coins,
		symbolChances_Seven,
		symbolsValue_LemonAndCherry,
		symbolsValue_CloverAndBell,
		symbolsValue_DiamondAndCoins,
		symbolsValue_Seven,
		patternsValue_3LessElements,
		patternsValue_4MoreElements,
		evilGeneric_SpawnSkeletonPiece,
		evilGeneric_DoubleCloversButMoney0,
		evilGeneric_More666,
		evilGeneric_ShinyObjects,
		evilGeneric_2FreeItemsTicketsZero,
		evilGeneric_TakeOtherAbilitiesButDeviousMod,
		evilGeneric_DoubleCoinsTicketsZero,
		evilHalvenChances_LemonAndCherry,
		evilHalvenChances_CloverAndBell,
		evilHalvenChances_DiamondCoinsAndSeven,
		holyGeneric_MultiplierSymbols_1,
		holyGeneric_MultiplierPatterns_1,
		holyGeneric_ReduceChargesNeeded_ForRedButtonCharms,
		holyGeneric_ModifyStoreCharms_Make1Free,
		holyGeneric_PatternsRepetitionIncrase,
		holyGeneric_SpawnSacredCharm,
		holyPatternsValue_3LessElements,
		holyPatternsValue_4MoreElements,
		count
	}

	public enum Category
	{
		undefined = -1,
		normal,
		evil,
		good,
		count
	}

	public enum Archetype
	{
		normal,
		symbolChances,
		symbolsValue,
		patternsValue,
		count
	}

	public enum Rarity
	{
		undefined = -1,
		common,
		uncommon,
		rare,
		ultraRare,
		legendary,
		count
	}

	// (Invoke) Token: 0x0600111A RID: 4378
	public delegate void AbilityCallback(AbilityScript ability);
}
