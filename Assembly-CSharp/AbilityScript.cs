using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class AbilityScript
{
	// Token: 0x06000427 RID: 1063 RVA: 0x00008F3F File Offset: 0x0000713F
	public AbilityScript.Identifier IdentifierGet()
	{
		return this.identifier;
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x00008F47 File Offset: 0x00007147
	public AbilityScript.Category CategoryGet()
	{
		return this.category;
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x00008F4F File Offset: 0x0000714F
	public AbilityScript.Archetype ArchetypeGet()
	{
		return this.archetype;
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x00008F57 File Offset: 0x00007157
	public AbilityScript.Rarity RarityGet()
	{
		return this.rarity;
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x0002D6E8 File Offset: 0x0002B8E8
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

	// Token: 0x0600042C RID: 1068 RVA: 0x00008F5F File Offset: 0x0000715F
	public static AbilityScript AbilityGet(AbilityScript.Identifier identifier)
	{
		if (!AbilityScript.dict_All.ContainsKey(identifier))
		{
			Debug.LogError("AbilityScript.AbilityGet(): Ability not found in dictionary");
			return null;
		}
		return AbilityScript.dict_All[identifier];
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x00008F85 File Offset: 0x00007185
	public string NameGetTranslated()
	{
		return Translation.Get(this.nameKey);
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x00008F92 File Offset: 0x00007192
	public string DescriptionGetTranslated()
	{
		Strings.SetTemporaryFlag_Sanitize666And999(1);
		return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get(this.descriptionKey), this._GetAbilitySanitizationSubKind());
	}

	// Token: 0x0600042F RID: 1071 RVA: 0x00008FB1 File Offset: 0x000071B1
	public int RepliesGetCount()
	{
		if (this.repliesKeys == null)
		{
			return 0;
		}
		return this.repliesKeys.Length;
	}

	// Token: 0x06000430 RID: 1072 RVA: 0x00008FC5 File Offset: 0x000071C5
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

	// Token: 0x06000431 RID: 1073 RVA: 0x00008FEF File Offset: 0x000071EF
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

	// Token: 0x06000432 RID: 1074 RVA: 0x00009020 File Offset: 0x00007220
	public static string NameGetTranslated(AbilityScript.Identifier identifier)
	{
		return AbilityScript.AbilityGet(identifier).NameGetTranslated();
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x0000902D File Offset: 0x0000722D
	public static string NameGetKey(AbilityScript.Identifier identifier)
	{
		return AbilityScript.AbilityGet(identifier).nameKey;
	}

	// Token: 0x06000434 RID: 1076 RVA: 0x0000903A File Offset: 0x0000723A
	public static string DescriptionGetTranslated(AbilityScript.Identifier identifier)
	{
		return AbilityScript.AbilityGet(identifier).DescriptionGetTranslated();
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x00009047 File Offset: 0x00007247
	public static string ReplyGet(AbilityScript.Identifier identifier, int index)
	{
		return AbilityScript.AbilityGet(identifier).ReplyGet(index);
	}

	// Token: 0x06000436 RID: 1078 RVA: 0x00009055 File Offset: 0x00007255
	public static string ReplyGetRandom(AbilityScript.Identifier identifier)
	{
		return AbilityScript.AbilityGet(identifier).ReplyGetRandom();
	}

	// Token: 0x06000437 RID: 1079 RVA: 0x0002D764 File Offset: 0x0002B964
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

	// Token: 0x06000438 RID: 1080 RVA: 0x00009062 File Offset: 0x00007262
	public Sprite SpriteGet()
	{
		return this.mySprite;
	}

	// Token: 0x06000439 RID: 1081 RVA: 0x0000906A File Offset: 0x0000726A
	public static Sprite SpriteGet(AbilityScript.Identifier identifier)
	{
		return AbilityScript.AbilityGet(identifier).SpriteGet();
	}

	// Token: 0x0600043A RID: 1082 RVA: 0x00009077 File Offset: 0x00007277
	public static Color ColorGet(AbilityScript.Identifier identifier)
	{
		return AbilityScript.AbilityGet(identifier).ColorGet();
	}

	// Token: 0x0600043B RID: 1083 RVA: 0x0002D794 File Offset: 0x0002B994
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

	// Token: 0x0600043C RID: 1084 RVA: 0x0002D7F4 File Offset: 0x0002B9F4
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

	// Token: 0x0600043D RID: 1085 RVA: 0x0002D978 File Offset: 0x0002BB78
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

	// Token: 0x0600043E RID: 1086 RVA: 0x0002D9C8 File Offset: 0x0002BBC8
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

	// Token: 0x0600043F RID: 1087 RVA: 0x0002DA90 File Offset: 0x0002BC90
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

	// Token: 0x06000440 RID: 1088 RVA: 0x00009084 File Offset: 0x00007284
	public bool IsLastPickAvailable()
	{
		return this.maxPickupTimes - this.pickedTimes == 1;
	}

	// Token: 0x06000441 RID: 1089 RVA: 0x0002DAE0 File Offset: 0x0002BCE0
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

	// Token: 0x06000442 RID: 1090 RVA: 0x0002DBF4 File Offset: 0x0002BDF4
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

	// Token: 0x06000443 RID: 1091 RVA: 0x0002DCA4 File Offset: 0x0002BEA4
	public static void ApplyDeviousModifierToRandomCharmOnTable()
	{
		PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
		if (randomCharmToModify_OnTableOrSlot != null)
		{
			GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.devious, true);
		}
	}

	// Token: 0x06000444 RID: 1092 RVA: 0x00009099 File Offset: 0x00007299
	private static void AFunc_OnPick_ExtraSpace(AbilityScript ability)
	{
		GameplayData.MaxEquippablePowerupsAdd(1);
	}

	// Token: 0x06000445 RID: 1093 RVA: 0x000090A1 File Offset: 0x000072A1
	private static void AFunc_OnPick_InterestsUp(AbilityScript ability)
	{
		GameplayData.InterestRateAdd(5f);
	}

	// Token: 0x06000446 RID: 1094 RVA: 0x000090AD File Offset: 0x000072AD
	private static void AFunc_OnPick_JackpotDouble(AbilityScript ability)
	{
		GameplayData.Pattern_ValueExtra_Add(PatternScript.Kind.jackpot, GameplayData.Pattern_ValueOverall_Get(PatternScript.Kind.jackpot, false));
	}

	// Token: 0x06000447 RID: 1095 RVA: 0x000090BC File Offset: 0x000072BC
	private static void AFunc_OnPick_TicketsPlus5(AbilityScript ability)
	{
		GameplayData.CloverTicketsAdd(5L, true);
	}

	// Token: 0x06000448 RID: 1096 RVA: 0x000090C6 File Offset: 0x000072C6
	private static void AFunc_OnPick_Discount2(AbilityScript ability)
	{
		GameplayData.StoreTemporaryDiscountAdd(2L, true);
	}

	// Token: 0x06000449 RID: 1097 RVA: 0x0002DCD0 File Offset: 0x0002BED0
	private static void AFunc_OnPick_AddModifierRandomCharm_CloverKind(AbilityScript ability)
	{
		PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
		if (randomCharmToModify_OnTableOrSlot == null)
		{
			return;
		}
		GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.cloverTicket, true);
	}

	// Token: 0x0600044A RID: 1098 RVA: 0x0002DCFC File Offset: 0x0002BEFC
	private static void AFunc_OnPick_AddModifierRandomCharm_SymbolsMult(AbilityScript ability)
	{
		PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
		if (randomCharmToModify_OnTableOrSlot == null)
		{
			return;
		}
		GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.symbolMultiplier, true);
	}

	// Token: 0x0600044B RID: 1099 RVA: 0x0002DD28 File Offset: 0x0002BF28
	private static void AFunc_OnPick_AddModifierRandomCharm_PatternsMult(AbilityScript ability)
	{
		PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
		if (randomCharmToModify_OnTableOrSlot == null)
		{
			return;
		}
		GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.patternMultiplier, true);
	}

	// Token: 0x0600044C RID: 1100 RVA: 0x0002DD54 File Offset: 0x0002BF54
	private static void AFunc_OnPick_AddModifierRandomCharm_Obsessive(AbilityScript ability)
	{
		PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
		if (randomCharmToModify_OnTableOrSlot == null)
		{
			return;
		}
		GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.obsessive, true);
	}

	// Token: 0x0600044D RID: 1101 RVA: 0x0002DD80 File Offset: 0x0002BF80
	private static void AFunc_OnPick_AddModifierRandomCharm_Gambler(AbilityScript ability)
	{
		PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
		if (randomCharmToModify_OnTableOrSlot == null)
		{
			return;
		}
		GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.gambler, true);
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x0002DDAC File Offset: 0x0002BFAC
	private static void AFunc_OnPick_AddModifierRandomCharm_Speculative(AbilityScript ability)
	{
		PowerupScript randomCharmToModify_OnTableOrSlot = AbilityScript.GetRandomCharmToModify_OnTableOrSlot();
		if (randomCharmToModify_OnTableOrSlot == null)
		{
			return;
		}
		GameplayData.Powerup_Modifier_Set(randomCharmToModify_OnTableOrSlot.identifier, PowerupScript.Modifier.speculative, true);
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x000090D0 File Offset: 0x000072D0
	private static void AFunc_OnPick_RechargeRedButtonPowerups(AbilityScript ability)
	{
		GameplayData.Powerup_ButtonChargesUsed_ResetAll(true);
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x000090D9 File Offset: 0x000072D9
	private static void AFunc_OnPick_SymbolLemonChanceUp(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.lemon, 0.8f);
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x000090E6 File Offset: 0x000072E6
	private static void AFunc_OnPick_SymbolCherryChanceUp(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.cherry, 0.8f);
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x000090F3 File Offset: 0x000072F3
	private static void AFunc_OnPick_SymbolCloverChanceUp(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.clover, 0.8f);
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x00009100 File Offset: 0x00007300
	private static void AFunc_OnPick_SymbolBellChanceUp(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.bell, 0.8f);
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x0000910D File Offset: 0x0000730D
	private static void AFunc_OnPick_SymbolDiamondChanceUp(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.diamond, 0.8f);
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x0000911A File Offset: 0x0000731A
	private static void AFunc_OnPick_SymbolCoinsChanceUp(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.coins, 0.8f);
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x00009127 File Offset: 0x00007327
	private static void AFunc_OnPick_SymbolSevenChanceUp(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.seven, 0.8f);
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x00009134 File Offset: 0x00007334
	private static void AFunc_OnPick_SymbolsValue_LemonAndCherry(AbilityScript ability)
	{
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.lemon, GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.lemon));
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.cherry, GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.cherry));
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x0000914E File Offset: 0x0000734E
	private static void AFunc_OnPick_SymbolsValue_CloverAndBell(AbilityScript ability)
	{
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.clover, GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.clover));
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.bell, GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.bell));
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x00009168 File Offset: 0x00007368
	private static void AFunc_OnPick_SymbolsValue_DiamondAndCoins(AbilityScript ability)
	{
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.diamond, GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.diamond));
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.coins, GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.coins));
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x00009182 File Offset: 0x00007382
	private static void AFunc_OnPick_SymbolsValue_Seven(AbilityScript ability)
	{
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.seven, GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.seven) * 2);
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x0002DDD8 File Offset: 0x0002BFD8
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

	// Token: 0x0600045C RID: 1116 RVA: 0x0002DE1C File Offset: 0x0002C01C
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

	// Token: 0x0600045D RID: 1117 RVA: 0x0002DE60 File Offset: 0x0002C060
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

	// Token: 0x0600045E RID: 1118 RVA: 0x0000919B File Offset: 0x0000739B
	private static void AFunc_OnPick_EvilGeneric_DoubleCloversButMoney0(AbilityScript ability)
	{
		GameplayData.CloverTicketsAdd(GameplayData.CloverTicketsGet(), true);
		GameplayData.CoinsSet(0);
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x0002DF2C File Offset: 0x0002C12C
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

	// Token: 0x06000460 RID: 1120 RVA: 0x0002DF98 File Offset: 0x0002C198
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

	// Token: 0x06000461 RID: 1121 RVA: 0x0002E0DC File Offset: 0x0002C2DC
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

	// Token: 0x06000462 RID: 1122 RVA: 0x000091B3 File Offset: 0x000073B3
	private static void AFunc_OnPick_EvilGeneric_DoubleCoinsTicketsZero(AbilityScript ability)
	{
		GameplayData.CoinsAdd(GameplayData.CoinsGet(), true);
		GameplayData.CloverTicketsSet(0L);
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x000091C7 File Offset: 0x000073C7
	public static void AFunc_OnPick_EvilHalvenChances_LemonsAndCherries(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.lemon, -(GameplayData.Symbol_Chance_Get(SymbolScript.Kind.lemon, false, false) / 2f));
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.cherry, -(GameplayData.Symbol_Chance_Get(SymbolScript.Kind.cherry, false, false) / 2f));
		AbilityScript.ApplyDeviousModifierToRandomCharmOnTable();
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x000091F8 File Offset: 0x000073F8
	public static void AFunc_OnPick_EvilHalvenChances_CloversAndBells(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.clover, -(GameplayData.Symbol_Chance_Get(SymbolScript.Kind.clover, false, false) / 2f));
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.bell, -(GameplayData.Symbol_Chance_Get(SymbolScript.Kind.bell, false, false) / 2f));
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0002E158 File Offset: 0x0002C358
	public static void AFunc_OnPick_EvilHalvenChances_DiamondCoinsAndSeven(AbilityScript ability)
	{
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.diamond, -(GameplayData.Symbol_Chance_Get(SymbolScript.Kind.diamond, false, false) / 2f));
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.coins, -(GameplayData.Symbol_Chance_Get(SymbolScript.Kind.coins, false, false) / 2f));
		GameplayData.Symbol_Chance_Add(SymbolScript.Kind.seven, -(GameplayData.Symbol_Chance_Get(SymbolScript.Kind.seven, false, false) / 2f));
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x00009224 File Offset: 0x00007424
	private static void AFunc_OnPick_HolyGeneric_SymbolsMultiplier3(AbilityScript ability)
	{
		GameplayData.AllSymbolsMultiplierAdd(3);
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x00009231 File Offset: 0x00007431
	private static void AFunc_OnPick_HolyGeneric_PatternsMultiplier1(AbilityScript ability)
	{
		GameplayData.AllPatternsMultiplierAdd(1);
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x0002E1A4 File Offset: 0x0002C3A4
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

	// Token: 0x06000469 RID: 1129 RVA: 0x0002E218 File Offset: 0x0002C418
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

	// Token: 0x0600046A RID: 1130 RVA: 0x0000923E File Offset: 0x0000743E
	private static void AFunc_OnPick_HolyGeneric_PatternsRepetitionIncrease(AbilityScript ability)
	{
		GameplayData.AbilityHoly_PatternsRepetitions++;
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x0002E2E8 File Offset: 0x0002C4E8
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

	// Token: 0x0600046C RID: 1132 RVA: 0x0002E354 File Offset: 0x0002C554
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

	// Token: 0x0600046D RID: 1133 RVA: 0x0002E398 File Offset: 0x0002C598
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

	// Token: 0x0600046E RID: 1134 RVA: 0x0002E3DC File Offset: 0x0002C5DC
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

	// Token: 0x0600046F RID: 1135 RVA: 0x0002E484 File Offset: 0x0002C684
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

	// Token: 0x040003AE RID: 942
	public static List<AbilityScript> list_All = new List<AbilityScript>();

	// Token: 0x040003AF RID: 943
	public static Dictionary<AbilityScript.Identifier, AbilityScript> dict_All = new Dictionary<AbilityScript.Identifier, AbilityScript>();

	// Token: 0x040003B0 RID: 944
	public static Dictionary<AbilityScript.Category, List<AbilityScript>> dict_ListsByCategory = new Dictionary<AbilityScript.Category, List<AbilityScript>>();

	// Token: 0x040003B1 RID: 945
	private const int PLAYER_INDEX = 0;

	// Token: 0x040003B2 RID: 946
	private const int MAX_PICKUP_TIMES_INFINITE = -1;

	// Token: 0x040003B3 RID: 947
	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x040003B4 RID: 948
	private AbilityScript.Identifier identifier;

	// Token: 0x040003B5 RID: 949
	private AbilityScript.Category category;

	// Token: 0x040003B6 RID: 950
	private AbilityScript.Archetype archetype;

	// Token: 0x040003B7 RID: 951
	private AbilityScript.Rarity rarity;

	// Token: 0x040003B8 RID: 952
	private string nameKey;

	// Token: 0x040003B9 RID: 953
	private string descriptionKey;

	// Token: 0x040003BA RID: 954
	private string[] repliesKeys;

	// Token: 0x040003BB RID: 955
	private Sprite mySprite;

	// Token: 0x040003BC RID: 956
	private AudioClip sound;

	// Token: 0x040003BD RID: 957
	private int maxPickupTimes = -1;

	// Token: 0x040003BE RID: 958
	private int pickedTimes;

	// Token: 0x040003BF RID: 959
	public AbilityScript.AbilityCallback OnAbilityPick;

	// Token: 0x040003C0 RID: 960
	public AbilityScript.AbilityCallback OnAbilityPick_Static;

	// Token: 0x040003C1 RID: 961
	private static List<PowerupScript> _2fiTk0_StorePowerups = new List<PowerupScript>();

	// Token: 0x040003C2 RID: 962
	private static List<PowerupScript> _ModifyAllStoreCharmsMake1Free_StorePowerups = new List<PowerupScript>();

	// Token: 0x040003C3 RID: 963
	private static List<AbilityScript> _tempInstList = new List<AbilityScript>();

	// Token: 0x02000045 RID: 69
	public enum Identifier
	{
		// Token: 0x040003C5 RID: 965
		undefined = -1,
		// Token: 0x040003C6 RID: 966
		extraSpace,
		// Token: 0x040003C7 RID: 967
		interestsUp,
		// Token: 0x040003C8 RID: 968
		jackpotIncreaseBase,
		// Token: 0x040003C9 RID: 969
		ticketsPlus5,
		// Token: 0x040003CA RID: 970
		discount1,
		// Token: 0x040003CB RID: 971
		rndCharmMod_Clover,
		// Token: 0x040003CC RID: 972
		rndCharmMod_SymbMult,
		// Token: 0x040003CD RID: 973
		rndCharmMod_PatMult,
		// Token: 0x040003CE RID: 974
		rndCharmMod_Obsessive,
		// Token: 0x040003CF RID: 975
		rndCharmMod_Gambler,
		// Token: 0x040003D0 RID: 976
		rndCharmMod_Speculative,
		// Token: 0x040003D1 RID: 977
		rechargeRedButtonPowerups,
		// Token: 0x040003D2 RID: 978
		symbolChances_Lemon,
		// Token: 0x040003D3 RID: 979
		symbolChances_Cherry,
		// Token: 0x040003D4 RID: 980
		symbolChances_Clover,
		// Token: 0x040003D5 RID: 981
		symbolChances_Bell,
		// Token: 0x040003D6 RID: 982
		symbolChances_Diamond,
		// Token: 0x040003D7 RID: 983
		symbolChances_Coins,
		// Token: 0x040003D8 RID: 984
		symbolChances_Seven,
		// Token: 0x040003D9 RID: 985
		symbolsValue_LemonAndCherry,
		// Token: 0x040003DA RID: 986
		symbolsValue_CloverAndBell,
		// Token: 0x040003DB RID: 987
		symbolsValue_DiamondAndCoins,
		// Token: 0x040003DC RID: 988
		symbolsValue_Seven,
		// Token: 0x040003DD RID: 989
		patternsValue_3LessElements,
		// Token: 0x040003DE RID: 990
		patternsValue_4MoreElements,
		// Token: 0x040003DF RID: 991
		evilGeneric_SpawnSkeletonPiece,
		// Token: 0x040003E0 RID: 992
		evilGeneric_DoubleCloversButMoney0,
		// Token: 0x040003E1 RID: 993
		evilGeneric_More666,
		// Token: 0x040003E2 RID: 994
		evilGeneric_ShinyObjects,
		// Token: 0x040003E3 RID: 995
		evilGeneric_2FreeItemsTicketsZero,
		// Token: 0x040003E4 RID: 996
		evilGeneric_TakeOtherAbilitiesButDeviousMod,
		// Token: 0x040003E5 RID: 997
		evilGeneric_DoubleCoinsTicketsZero,
		// Token: 0x040003E6 RID: 998
		evilHalvenChances_LemonAndCherry,
		// Token: 0x040003E7 RID: 999
		evilHalvenChances_CloverAndBell,
		// Token: 0x040003E8 RID: 1000
		evilHalvenChances_DiamondCoinsAndSeven,
		// Token: 0x040003E9 RID: 1001
		holyGeneric_MultiplierSymbols_1,
		// Token: 0x040003EA RID: 1002
		holyGeneric_MultiplierPatterns_1,
		// Token: 0x040003EB RID: 1003
		holyGeneric_ReduceChargesNeeded_ForRedButtonCharms,
		// Token: 0x040003EC RID: 1004
		holyGeneric_ModifyStoreCharms_Make1Free,
		// Token: 0x040003ED RID: 1005
		holyGeneric_PatternsRepetitionIncrase,
		// Token: 0x040003EE RID: 1006
		holyGeneric_SpawnSacredCharm,
		// Token: 0x040003EF RID: 1007
		holyPatternsValue_3LessElements,
		// Token: 0x040003F0 RID: 1008
		holyPatternsValue_4MoreElements,
		// Token: 0x040003F1 RID: 1009
		count
	}

	// Token: 0x02000046 RID: 70
	public enum Category
	{
		// Token: 0x040003F3 RID: 1011
		undefined = -1,
		// Token: 0x040003F4 RID: 1012
		normal,
		// Token: 0x040003F5 RID: 1013
		evil,
		// Token: 0x040003F6 RID: 1014
		good,
		// Token: 0x040003F7 RID: 1015
		count
	}

	// Token: 0x02000047 RID: 71
	public enum Archetype
	{
		// Token: 0x040003F9 RID: 1017
		normal,
		// Token: 0x040003FA RID: 1018
		symbolChances,
		// Token: 0x040003FB RID: 1019
		symbolsValue,
		// Token: 0x040003FC RID: 1020
		patternsValue,
		// Token: 0x040003FD RID: 1021
		count
	}

	// Token: 0x02000048 RID: 72
	public enum Rarity
	{
		// Token: 0x040003FF RID: 1023
		undefined = -1,
		// Token: 0x04000400 RID: 1024
		common,
		// Token: 0x04000401 RID: 1025
		uncommon,
		// Token: 0x04000402 RID: 1026
		rare,
		// Token: 0x04000403 RID: 1027
		ultraRare,
		// Token: 0x04000404 RID: 1028
		legendary,
		// Token: 0x04000405 RID: 1029
		count
	}

	// Token: 0x02000049 RID: 73
	// (Invoke) Token: 0x06000473 RID: 1139
	public delegate void AbilityCallback(AbilityScript ability);
}
