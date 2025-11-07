using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PowerupScript : MonoBehaviour
{
	// Token: 0x06000474 RID: 1140 RVA: 0x0001E3C0 File Offset: 0x0001C5C0
	public long StartingPriceGet(bool considerModifier, bool considerRunModifiers)
	{
		long num = this.startingPrice;
		if (considerModifier)
		{
			num += this.ModifierExtraPrice();
		}
		if (num < 0L)
		{
			num = 0L;
		}
		return num;
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x0001E3EC File Offset: 0x0001C5EC
	private long ModifierExtraPrice()
	{
		PowerupScript.Modifier modifier = GameplayData.Powerup_Modifier_Get(this.identifier);
		switch (modifier)
		{
		case PowerupScript.Modifier.none:
			return 0L;
		case PowerupScript.Modifier.symbolMultiplier:
			return 2L;
		case PowerupScript.Modifier.patternMultiplier:
			return 3L;
		case PowerupScript.Modifier.cloverTicket:
			return 1L;
		case PowerupScript.Modifier.obsessive:
			return 3L;
		case PowerupScript.Modifier.gambler:
			return 1L;
		case PowerupScript.Modifier.speculative:
			return 2L;
		case PowerupScript.Modifier.devious:
			return -1L;
		default:
			Debug.LogError("PowerupScript: ModifierExtraPrice: Modifier not handled! Modifier: " + modifier.ToString());
			return 0L;
		}
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x0001E464 File Offset: 0x0001C664
	public long ResellValueGet()
	{
		long num = 1L;
		if (this.identifier == PowerupScript.Identifier.Sardines)
		{
			num = 2L;
		}
		return (this.StartingPriceGet(true, true) + (long)GameplayData.Powerup_ResellBonus_Get(this.identifier)) * num / 2L;
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x0001E49B File Offset: 0x0001C69B
	public bool IsInstantPowerup()
	{
		return this.isInstantPowerup;
	}

	// Token: 0x06000478 RID: 1144 RVA: 0x0001E4A3 File Offset: 0x0001C6A3
	public int MaxBuyTimesGet()
	{
		return this.maxBuyTimes;
	}

	// Token: 0x06000479 RID: 1145 RVA: 0x0001E4AC File Offset: 0x0001C6AC
	public bool StoreRerollEvaluate()
	{
		float num = GameplayData.StoreLuckGet();
		return R.Rng_Store.Value * num <= this.storeRerollChance;
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0001E4D6 File Offset: 0x0001C6D6
	public bool IsCommon()
	{
		return this.storeRerollChance < 0.1f;
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x0001E4E5 File Offset: 0x0001C6E5
	public bool IsUncommon()
	{
		return this.storeRerollChance < 0.2f && this.storeRerollChance >= 0.1f;
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x0001E506 File Offset: 0x0001C706
	public bool IsMildlyRare()
	{
		return this.storeRerollChance < 0.35f && this.storeRerollChance >= 0.2f;
	}

	// Token: 0x0600047D RID: 1149 RVA: 0x0001E527 File Offset: 0x0001C727
	public bool IsRare()
	{
		return this.storeRerollChance < 0.5f && this.storeRerollChance >= 0.35f;
	}

	// Token: 0x0600047E RID: 1150 RVA: 0x0001E548 File Offset: 0x0001C748
	public bool IsVeryRare()
	{
		return this.storeRerollChance < 0.65f && this.storeRerollChance >= 0.5f;
	}

	// Token: 0x0600047F RID: 1151 RVA: 0x0001E569 File Offset: 0x0001C769
	public bool IsLegendary()
	{
		return this.storeRerollChance >= 0.65f;
	}

	// Token: 0x06000480 RID: 1152 RVA: 0x0001E57C File Offset: 0x0001C77C
	public PowerupScript.PublicRarity RarityPublicGet()
	{
		if (this.IsCommon())
		{
			return PowerupScript.PublicRarity.common;
		}
		if (this.IsUncommon())
		{
			return PowerupScript.PublicRarity.uncommon;
		}
		if (this.IsMildlyRare())
		{
			return PowerupScript.PublicRarity.uncommon;
		}
		if (this.IsRare())
		{
			return PowerupScript.PublicRarity.rare;
		}
		if (this.IsVeryRare())
		{
			return PowerupScript.PublicRarity.rare;
		}
		if (this.IsLegendary())
		{
			return PowerupScript.PublicRarity.epic;
		}
		Debug.LogError("PowerupScript: RarityPublicGet: No rarity found! Identifier: " + this.identifier.ToString());
		return PowerupScript.PublicRarity.common;
	}

	// Token: 0x06000481 RID: 1153 RVA: 0x0001E5E8 File Offset: 0x0001C7E8
	private Strings.SanitizationSubKind _GetPowerupStringSanitizationSubKind()
	{
		Strings.SanitizationSubKind sanitizationSubKind = Strings.SanitizationSubKind.none;
		switch (this.archetype)
		{
		case PowerupScript.Archetype.spicyPeppers:
			sanitizationSubKind = Strings.SanitizationSubKind.powerup_SpicyPeppers;
			break;
		case PowerupScript.Archetype.generalModifierCharm:
		case PowerupScript.Archetype.commonModPuppets:
		case PowerupScript.Archetype.goldenSymbols:
		case PowerupScript.Archetype.boardgameC:
		case PowerupScript.Archetype.boardgameM:
			sanitizationSubKind = Strings.SanitizationSubKind.powerup_SymbolModifier;
			break;
		case PowerupScript.Archetype.skeleton:
			sanitizationSubKind = Strings.SanitizationSubKind.powerup_Skeleton;
			break;
		}
		if (sanitizationSubKind == Strings.SanitizationSubKind.none)
		{
			PowerupScript.Identifier identifier = this.identifier;
			if (identifier > PowerupScript.Identifier.YellowStar)
			{
				if (identifier <= PowerupScript.Identifier.Baphomet)
				{
					switch (identifier)
					{
					case PowerupScript.Identifier.Hole_Circle:
						return Strings.SanitizationSubKind.powerup_Hole;
					case PowerupScript.Identifier.Hole_Romboid:
						return Strings.SanitizationSubKind.powerup_Hole;
					case PowerupScript.Identifier.Hole_Cross:
						return Strings.SanitizationSubKind.powerup_Hole;
					case PowerupScript.Identifier.Rorschach:
						return Strings.SanitizationSubKind.powerup_ShowAllSymbolsAndPatternsValues;
					case PowerupScript.Identifier.AbstractPainting:
					case PowerupScript.Identifier.Pareidolia:
					case PowerupScript.Identifier.FruitBasket:
					case PowerupScript.Identifier.Necklace:
						return sanitizationSubKind;
					case PowerupScript.Identifier.Hourglass:
						return Strings.SanitizationSubKind.powerup_Hourglass;
					case PowerupScript.Identifier.SevenSinsStone:
					case PowerupScript.Identifier.CloverBell:
						break;
					default:
						if (identifier != PowerupScript.Identifier.Baphomet)
						{
							return sanitizationSubKind;
						}
						return Strings.SanitizationSubKind.powerup_Baphomet;
					}
				}
				else if (identifier != PowerupScript.Identifier.Jimbo)
				{
					if (identifier - PowerupScript.Identifier._999_AngelHand <= 1)
					{
						return Strings.SanitizationSubKind.powerup_ShowPatternsValues;
					}
					if (identifier - PowerupScript.Identifier._999_TheBlood > 1)
					{
						return sanitizationSubKind;
					}
				}
				return Strings.SanitizationSubKind.powerup_SymbolModifier;
			}
			if (identifier <= PowerupScript.Identifier.GoldenHand_MidasTouch)
			{
				if (identifier == PowerupScript.Identifier.GrandmasPurse)
				{
					return Strings.SanitizationSubKind.powerup_GrandmasPurse;
				}
				if (identifier != PowerupScript.Identifier.GoldenHand_MidasTouch)
				{
					return sanitizationSubKind;
				}
				return Strings.SanitizationSubKind.powerup_ShowAllSymbolsAndPatternsValues;
			}
			else
			{
				if (identifier - PowerupScript.Identifier.Wallet <= 1)
				{
					return Strings.SanitizationSubKind.powerup_DebtPercentages;
				}
				if (identifier != PowerupScript.Identifier.YellowStar)
				{
					return sanitizationSubKind;
				}
			}
			return Strings.SanitizationSubKind.powerup_ShowPatternsValues;
		}
		return sanitizationSubKind;
	}

	// Token: 0x06000482 RID: 1154 RVA: 0x0001E70C File Offset: 0x0001C90C
	public string NameGet(bool includeModTag, bool sanitize)
	{
		if (string.IsNullOrEmpty(this.nameKey))
		{
			Debug.LogError("PowerupScript: NameGet: nameKey is null or empty! GameObject: " + base.gameObject.name);
			return null;
		}
		string text;
		if (sanitize)
		{
			text = Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get(this.nameKey), this._GetPowerupStringSanitizationSubKind());
		}
		else
		{
			text = Translation.Get(this.nameKey);
		}
		if (includeModTag)
		{
			PowerupScript.Modifier modifier = GameplayData.Powerup_Modifier_Get(this.identifier);
			if (modifier != PowerupScript.Modifier.none)
			{
				switch (modifier)
				{
				case PowerupScript.Modifier.symbolMultiplier:
					text = text + " (" + Strings.Sanitize(Strings.SantizationKind.powerupKeywords, "[CMOD_NAME_SMULT]", Strings.SanitizationSubKind.none) + ")";
					break;
				case PowerupScript.Modifier.patternMultiplier:
					text = text + " (" + Strings.Sanitize(Strings.SantizationKind.powerupKeywords, "[CMOD_NAME_PMULT]", Strings.SanitizationSubKind.none) + ")";
					break;
				case PowerupScript.Modifier.cloverTicket:
					text = text + " (" + Strings.Sanitize(Strings.SantizationKind.powerupKeywords, "[CMOD_NAME_TICKET]", Strings.SanitizationSubKind.none) + ")";
					break;
				case PowerupScript.Modifier.obsessive:
					text = text + " (" + Strings.Sanitize(Strings.SantizationKind.powerupKeywords, "[CMOD_NAME_OBSESSIVE]", Strings.SanitizationSubKind.none) + ")";
					break;
				case PowerupScript.Modifier.gambler:
					text = text + " (" + Strings.Sanitize(Strings.SantizationKind.powerupKeywords, "[CMOD_NAME_GAMBLER]", Strings.SanitizationSubKind.none) + ")";
					break;
				case PowerupScript.Modifier.speculative:
					text = text + " (" + Strings.Sanitize(Strings.SantizationKind.powerupKeywords, "[CMOD_NAME_SPECULATIVE]", Strings.SanitizationSubKind.none) + ")";
					break;
				case PowerupScript.Modifier.devious:
					text = text + " (" + Strings.Sanitize(Strings.SantizationKind.powerupKeywords, "[CMOD_NAME_DEVIOUS]", Strings.SanitizationSubKind.none) + ")";
					break;
				}
			}
		}
		return text;
	}

	// Token: 0x06000483 RID: 1155 RVA: 0x0001E88A File Offset: 0x0001CA8A
	public string NameKeyGet()
	{
		return this.nameKey;
	}

	// Token: 0x06000484 RID: 1156 RVA: 0x0001E894 File Offset: 0x0001CA94
	public string DescriptionGet(bool includeRarity, bool includeBaseCost, bool includeModifier, float extrasNewLineSize)
	{
		if (string.IsNullOrEmpty(this.descriptionKey))
		{
			Debug.LogError("PowerupScript: DescriptionGet: descriptionKey is null or empty! GameObject: " + base.gameObject.name);
			return null;
		}
		string text = "";
		string text2 = "<size=" + extrasNewLineSize.ToString() + ">\n\n</size>";
		text += Translation.Get(this.descriptionKey);
		Strings.SetTemporaryFlag_Sanitize666And999(1);
		text = Strings.Sanitize(Strings.SantizationKind.powerupKeywords, text, this._GetPowerupStringSanitizationSubKind());
		float num = extrasNewLineSize * 3f;
		string text3 = this.ModifierDescriptionGet();
		if (includeModifier && !string.IsNullOrEmpty(text3))
		{
			Strings.SetTemporaryFlag_Sanitize666And999(1);
			text = string.Concat(new string[]
			{
				"<i>",
				Strings.Sanitize(Strings.SantizationKind.ui, string.Concat(new string[]
				{
					"<wave a=4><size=",
					num.ToString(),
					">",
					text3,
					"</size></wave>"
				}), Strings.SanitizationSubKind.none),
				"</i>",
				text2,
				text
			});
		}
		if (includeRarity)
		{
			text = text + text2 + Strings.GetPowerupRarity_SpriteString(this);
		}
		if (includeBaseCost)
		{
			text = text + "  " + Mathf.Max(0f, (float)this.StartingPriceGet(false, false)).ToString() + "<sprite name=\"CloverTicket\">";
		}
		return text;
	}

	// Token: 0x06000485 RID: 1157 RVA: 0x0001E9CF File Offset: 0x0001CBCF
	public string ModifierDescriptionGet()
	{
		return PowerupScript.ModifierDescriptionGet(GameplayData.Powerup_Modifier_Get(this.identifier));
	}

	// Token: 0x06000486 RID: 1158 RVA: 0x0001E9E4 File Offset: 0x0001CBE4
	public static string ModifierDescriptionGet(PowerupScript.Modifier powerupModifier)
	{
		switch (powerupModifier)
		{
		case PowerupScript.Modifier.none:
			return "";
		case PowerupScript.Modifier.symbolMultiplier:
			return Translation.Get("POWERUP_CHARM_MODIFIER_SYMBOLS_MULT");
		case PowerupScript.Modifier.patternMultiplier:
			return Translation.Get("POWERUP_CHARM_MODIFIER_PATTERN_MULT");
		case PowerupScript.Modifier.cloverTicket:
			return Translation.Get("POWERUP_CHARM_MODIFIER_CLOVER_TICKET");
		case PowerupScript.Modifier.obsessive:
			return Translation.Get("POWERUP_CHARM_MODIFIER_OBSESSIVE");
		case PowerupScript.Modifier.gambler:
			return Translation.Get("POWERUP_CHARM_MODIFIER_GAMBLER");
		case PowerupScript.Modifier.speculative:
			return Translation.Get("POWERUP_CHARM_MODIFIER_SPECULATIVE");
		case PowerupScript.Modifier.devious:
			return Translation.Get("POWERUP_CHARM_MODIFIER_DEVIOUS");
		default:
			Debug.LogError("PowerupScript: ModifierDescriptionGet: Modifier not handled! Modifier: " + powerupModifier.ToString());
			return "";
		}
	}

	// Token: 0x06000487 RID: 1159 RVA: 0x0001EA8D File Offset: 0x0001CC8D
	public string GetBatteryString()
	{
		Strings.SetTemporaryInspectedPowerup(this);
		return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, "[CHARGE_BAR]", Strings.SanitizationSubKind.none);
	}

	// Token: 0x06000488 RID: 1160 RVA: 0x0001EAA4 File Offset: 0x0001CCA4
	public string UnlockMissionGet()
	{
		if (string.IsNullOrEmpty(this.unlockMissionKey))
		{
			Debug.LogError("PowerupScript: UnlockTextGet: unlockTextKey is null or empty! GameObject: " + base.gameObject.name);
			return null;
		}
		return Strings.Sanitize(Strings.SantizationKind.powerupKeywords, Translation.Get(this.unlockMissionKey), this._GetPowerupStringSanitizationSubKind());
	}

	// Token: 0x06000489 RID: 1161 RVA: 0x0001EAF1 File Offset: 0x0001CCF1
	public static bool IsUnlocked(PowerupScript.Identifier powerupIdentifier)
	{
		return !PowerupScript.dict_IsLocked.ContainsKey(powerupIdentifier) || !PowerupScript.dict_IsLocked[powerupIdentifier];
	}

	// Token: 0x0600048A RID: 1162 RVA: 0x0001EB10 File Offset: 0x0001CD10
	public static bool Unlock(PowerupScript.Identifier powerupIdentifier)
	{
		return PowerupScript.UnlockExt(powerupIdentifier, true, true);
	}

	// Token: 0x0600048B RID: 1163 RVA: 0x0001EB1C File Offset: 0x0001CD1C
	public static bool UnlockExt(PowerupScript.Identifier powerupIdentifier, bool notifyPlayer, bool saveGame)
	{
		if (PowerupScript.IsUnlocked(powerupIdentifier))
		{
			return false;
		}
		if (GameplayMaster.IsCustomSeed())
		{
			return false;
		}
		PowerupScript powerup_Quick = PowerupScript.GetPowerup_Quick(powerupIdentifier);
		if (powerup_Quick == null)
		{
			return false;
		}
		if (notifyPlayer)
		{
			if (!TerminalScript.IsLoggedIn())
			{
				PowerupScript.PlayUnlockedAnimation(powerupIdentifier);
			}
			TerminalScript.NotificationSet(powerupIdentifier);
		}
		Data.game.LockedPowerups_Unlock(powerupIdentifier);
		if (saveGame && GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.preparation)
		{
			Data.SaveGame(Data.GameSavingReason.powerupUnlock, -1);
		}
		PowerupScript.dict_IsLocked[powerupIdentifier] = false;
		powerup_Quick.MaterialColorReset();
		if (Data.game.PowerupRealInstances_AreAllUnlocked())
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.CompletedDatabase);
			PlatformAPI.AchievementUnlock_Demo(PlatformAPI.AchievementDemo.ADecentCollection);
		}
		return true;
	}

	// Token: 0x0600048C RID: 1164 RVA: 0x0001EBB1 File Offset: 0x0001CDB1
	public BigInteger UnlockPriceGet()
	{
		return this.unlockPrice;
	}

	// Token: 0x0600048D RID: 1165 RVA: 0x0001EBB9 File Offset: 0x0001CDB9
	public bool IsBaseSet()
	{
		return this._isBaseSet;
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x0001EBC4 File Offset: 0x0001CDC4
	public static GameObject GetPrefab(PowerupScript.Identifier identifier)
	{
		if (PowerupScript.dict_IdentifierToPrefabName.ContainsKey(identifier))
		{
			string text = PowerupScript.dict_IdentifierToPrefabName[identifier];
			return AssetMaster.GetPrefab(text);
		}
		Debug.LogError("PowerupScript: GetPrefab: Prefab name not found! Identifier: " + identifier.ToString());
		return null;
	}

	// Token: 0x0600048F RID: 1167 RVA: 0x0001EC14 File Offset: 0x0001CE14
	public static PowerupScript FindPowerup(PowerupScript.Identifier identifier, out bool isEquipped, out bool isInDrawer)
	{
		if (identifier == PowerupScript.Identifier.undefined)
		{
			isEquipped = false;
			isInDrawer = false;
			return null;
		}
		if (identifier == PowerupScript.Identifier.count)
		{
			isEquipped = false;
			isInDrawer = false;
			return null;
		}
		foreach (PowerupScript powerupScript in PowerupScript.list_NotBought)
		{
			if (powerupScript.identifier == identifier)
			{
				isEquipped = false;
				isInDrawer = false;
				return powerupScript;
			}
		}
		foreach (PowerupScript powerupScript2 in PowerupScript.array_InDrawer)
		{
			if (!(powerupScript2 == null) && powerupScript2.identifier == identifier)
			{
				isEquipped = false;
				isInDrawer = true;
				return powerupScript2;
			}
		}
		foreach (PowerupScript powerupScript3 in PowerupScript.list_EquippedSkeleton)
		{
			if (powerupScript3.identifier == identifier)
			{
				isEquipped = true;
				isInDrawer = false;
				return powerupScript3;
			}
		}
		foreach (PowerupScript powerupScript4 in PowerupScript.list_EquippedNormal)
		{
			if (powerupScript4.identifier == identifier)
			{
				isEquipped = true;
				isInDrawer = false;
				return powerupScript4;
			}
		}
		foreach (PowerupScript powerupScript5 in PowerupScript._initializationTempList)
		{
			if (powerupScript5.identifier == identifier)
			{
				isEquipped = false;
				isInDrawer = false;
				return powerupScript5;
			}
		}
		Debug.LogError("PowerupScript: GetPowerup: Powerup not found! Identifier: " + identifier.ToString());
		isEquipped = false;
		isInDrawer = false;
		return null;
	}

	// Token: 0x06000490 RID: 1168 RVA: 0x0001EDE8 File Offset: 0x0001CFE8
	public static PowerupScript GetPowerup_Quick(PowerupScript.Identifier identifier)
	{
		if (PowerupScript.dict_IdentifierToInstance.ContainsKey(identifier))
		{
			return PowerupScript.dict_IdentifierToInstance[identifier];
		}
		return null;
	}

	// Token: 0x06000491 RID: 1169 RVA: 0x0001EE04 File Offset: 0x0001D004
	public static PowerupScript GetDrawerPowerup(int index)
	{
		return PowerupScript.array_InDrawer[index];
	}

	// Token: 0x06000492 RID: 1170 RVA: 0x0001EE10 File Offset: 0x0001D010
	public static bool IsEquipped(PowerupScript.Identifier identifier)
	{
		if (identifier == PowerupScript.Identifier.undefined)
		{
			return false;
		}
		if (identifier == PowerupScript.Identifier.count)
		{
			return false;
		}
		using (List<PowerupScript>.Enumerator enumerator = PowerupScript.list_EquippedSkeleton.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.identifier == identifier)
				{
					return true;
				}
			}
		}
		using (List<PowerupScript>.Enumerator enumerator = PowerupScript.list_EquippedNormal.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.identifier == identifier)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000493 RID: 1171 RVA: 0x0001EEC0 File Offset: 0x0001D0C0
	public static int IsInDrawer(PowerupScript.Identifier identifier)
	{
		if (identifier == PowerupScript.Identifier.undefined)
		{
			return -1;
		}
		if (identifier == PowerupScript.Identifier.count)
		{
			return -1;
		}
		for (int i = 0; i < PowerupScript.array_InDrawer.Length; i++)
		{
			if (!(PowerupScript.array_InDrawer[i] == null) && PowerupScript.array_InDrawer[i].identifier == identifier)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x0001EF10 File Offset: 0x0001D110
	public static bool IsDrawerAvailable(int drawerIndex)
	{
		return DrawersScript.IsDrawerUnlocked(drawerIndex) && PowerupScript.array_InDrawer[drawerIndex] == null;
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x0001EF30 File Offset: 0x0001D130
	public static bool IsNotBought(PowerupScript.Identifier identifier)
	{
		if (identifier == PowerupScript.Identifier.undefined)
		{
			return false;
		}
		if (identifier == PowerupScript.Identifier.count)
		{
			return false;
		}
		using (List<PowerupScript>.Enumerator enumerator = PowerupScript.list_NotBought.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.identifier == identifier)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x0001EF9C File Offset: 0x0001D19C
	public static bool IsEquipped_Quick(PowerupScript.Identifier identifier)
	{
		PowerupScript powerup_Quick = PowerupScript.GetPowerup_Quick(identifier);
		return !(powerup_Quick == null) && powerup_Quick.equippedChached;
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x0001EFC4 File Offset: 0x0001D1C4
	public static bool IsInDrawer_Quick(PowerupScript.Identifier identifier)
	{
		PowerupScript powerup_Quick = PowerupScript.GetPowerup_Quick(identifier);
		return !(powerup_Quick == null) && powerup_Quick.inDrawerChached;
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x0001EFE9 File Offset: 0x0001D1E9
	public DiegeticMenuElement DiegeticMenuElement_Get()
	{
		return this.diegeticMenuElement;
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x0001EFF4 File Offset: 0x0001D1F4
	public void DiegeticStateUpdate(bool enable, PowerupScript.MenuControllerTarget targetMenu)
	{
		if (!enable)
		{
			this.diegeticMenuElement.enabled = false;
			DiegeticMenuController.MainMenu.elements.Remove(this.diegeticMenuElement);
			DiegeticMenuController.SlotMenu.elements.Remove(this.diegeticMenuElement);
			return;
		}
		if (targetMenu == PowerupScript.MenuControllerTarget.Room)
		{
			this.diegeticMenuElement.enabled = true;
			this.diegeticMenuElement.SetMyController(DiegeticMenuController.MainMenu);
			if (!DiegeticMenuController.MainMenu.elements.Contains(this.diegeticMenuElement))
			{
				DiegeticMenuController.MainMenu.elements.Add(this.diegeticMenuElement);
			}
			DiegeticMenuController.SlotMenu.elements.Remove(this.diegeticMenuElement);
			return;
		}
		if (targetMenu != PowerupScript.MenuControllerTarget.Slot)
		{
			return;
		}
		this.diegeticMenuElement.enabled = true;
		this.diegeticMenuElement.SetMyController(DiegeticMenuController.SlotMenu);
		DiegeticMenuController.MainMenu.elements.Remove(this.diegeticMenuElement);
		if (!DiegeticMenuController.SlotMenu.elements.Contains(this.diegeticMenuElement))
		{
			DiegeticMenuController.SlotMenu.elements.Add(this.diegeticMenuElement);
		}
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x0001F102 File Offset: 0x0001D302
	public static void EquipFlag_IgnoreSpaceCondition()
	{
		PowerupScript.equipFlag_DontCheckForSpace = true;
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x0001F10C File Offset: 0x0001D30C
	public static bool Equip(PowerupScript.Identifier identifier, bool isInitializationCall, bool putInNotBoughtListIfFull)
	{
		bool flag = PowerupScript.equipFlag_DontCheckForSpace;
		PowerupScript.equipFlag_DontCheckForSpace = false;
		bool flag2 = false;
		bool flag3 = false;
		PowerupScript powerupScript = PowerupScript.FindPowerup(identifier, out flag2, out flag3);
		if (powerupScript == null)
		{
			Debug.LogError("PowerupScript: Equip: Powerup not found! Identifier: " + identifier.ToString());
			return false;
		}
		if (flag2)
		{
			Debug.LogError("PowerupScript: Equip: Powerup already equipped! Identifier: " + identifier.ToString());
			return false;
		}
		if (!powerupScript.isInstantPowerup)
		{
			int num = ItemOrganizerScript.SkeletonSlotsN();
			int num2 = GameplayData.MaxEquippablePowerupsGet(true);
			bool flag4 = false;
			if (powerupScript.category == PowerupScript.Category.skeleton && PowerupScript.list_EquippedSkeleton.Count >= num)
			{
				flag4 = true;
			}
			if (powerupScript.category == PowerupScript.Category.normal && PowerupScript.list_EquippedNormal.Count >= num2)
			{
				flag4 = true;
			}
			if (flag4 && !flag)
			{
				CameraGame.Shake(1f);
				Sound.Play("SoundMenuError", 1f, 1f);
				DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_POWERUP_NO_SPACE_TO_EQUIP" });
				if (putInNotBoughtListIfFull)
				{
					PowerupScript.ThrowAway(identifier, isInitializationCall);
				}
				return false;
			}
			if (flag3)
			{
				int num3 = Array.IndexOf<PowerupScript>(PowerupScript.array_InDrawer, powerupScript);
				if (num3 >= 0)
				{
					PowerupScript.array_InDrawer[num3] = null;
				}
			}
			else
			{
				PowerupScript.list_NotBought.Remove(powerupScript);
			}
			if (powerupScript.category == PowerupScript.Category.skeleton)
			{
				if (!PowerupScript.list_EquippedSkeleton.Contains(powerupScript))
				{
					PowerupScript.list_EquippedSkeleton.Add(powerupScript);
				}
			}
			else if (!PowerupScript.list_EquippedNormal.Contains(powerupScript))
			{
				PowerupScript.list_EquippedNormal.Add(powerupScript);
			}
			powerupScript.DiegeticStateUpdate(true, PowerupScript.MenuControllerTarget.Room);
			if (!isInitializationCall)
			{
				PowerupScript.RefreshPlacementAll();
			}
			if (!isInitializationCall)
			{
				Sound.Play("SoundPowerupEquip", 1f, 1f);
			}
		}
		for (int i = 0; i < 4; i++)
		{
			if (StoreCapsuleScript.storePowerups[i] == powerupScript)
			{
				StoreCapsuleScript.storePowerups[i] = null;
				break;
			}
		}
		powerupScript.equippedChached = true;
		powerupScript.inDrawerChached = false;
		if (!isInitializationCall)
		{
			GameplayMaster.FailsafeCharms_SetTriggered();
		}
		if (!isInitializationCall)
		{
			powerupScript.JustEquippedGlowSet();
		}
		if (powerupScript.archetype == PowerupScript.Archetype.sacred)
		{
			powerupScript.sacredGlowHolder.SetActive(true);
		}
		if (powerupScript.isInstantPowerup)
		{
			powerupScript.PlaceFarAway();
		}
		if (powerupScript.onEquip != null)
		{
			powerupScript.onEquip(powerupScript);
		}
		if (PowerupScript.onEquipStatic != null)
		{
			PowerupScript.onEquipStatic(powerupScript);
		}
		if (isInitializationCall)
		{
			PowerupScript._initializationTempList.Remove(powerupScript);
		}
		if (!isInitializationCall && powerupScript.isInstantPowerup)
		{
			PowerupScript.HoleCircle_RecordCharmTry(powerupScript.identifier);
		}
		if (!isInitializationCall && powerupScript.archetype == PowerupScript.Archetype.skeleton && PowerupScript.list_EquippedSkeleton.Count == 1)
		{
			DoorScript.DoorKnockPlay_Try();
		}
		if (!isInitializationCall && powerupScript.archetype == PowerupScript.Archetype.skeleton)
		{
			RewardBoxScript.RefreshText_ToDeadlineDebtToReach();
		}
		return true;
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x0001F390 File Offset: 0x0001D590
	public static bool PutInDrawer(PowerupScript.Identifier identifier, bool isInitializationCall, int desiredDrawerIndex = -1)
	{
		bool flag = false;
		bool flag2 = false;
		PowerupScript powerupScript = PowerupScript.FindPowerup(identifier, out flag, out flag2);
		if (powerupScript == null)
		{
			Debug.LogError("PowerupScript: PutInDrawer: Powerup not found! Identifier: " + identifier.ToString());
			return false;
		}
		if (flag2)
		{
			Debug.LogError("PowerupScript: PutInDrawer: Powerup already in drawer! Identifier: " + identifier.ToString());
			return false;
		}
		bool flag3 = false;
		int num = -1;
		for (int i = 0; i < PowerupScript.array_InDrawer.Length; i++)
		{
			if (PowerupScript.IsDrawerAvailable(i) && (desiredDrawerIndex < 0 || i == desiredDrawerIndex))
			{
				flag3 = true;
				PowerupScript.array_InDrawer[i] = powerupScript;
				num = i;
				break;
			}
		}
		if (flag3)
		{
			if (flag)
			{
				if (powerupScript.category == PowerupScript.Category.skeleton)
				{
					PowerupScript.list_EquippedSkeleton.Remove(powerupScript);
				}
				else
				{
					PowerupScript.list_EquippedNormal.Remove(powerupScript);
				}
			}
			else
			{
				PowerupScript.list_NotBought.Remove(powerupScript);
			}
			powerupScript.DiegeticStateUpdate(false, PowerupScript.MenuControllerTarget.Room);
			if (!isInitializationCall)
			{
				PowerupScript.RefreshPlacementAll();
			}
			if (!isInitializationCall && !Sound.IsPlaying("SoundPowerupPutInDrawer"))
			{
				Sound.Play("SoundPowerupPutInDrawer", 1f, 1f);
			}
			powerupScript.sacredGlowHolder.SetActive(false);
			powerupScript.equippedChached = false;
			powerupScript.inDrawerChached = true;
			if (flag && powerupScript.onUnequip != null)
			{
				powerupScript.onUnequip(powerupScript);
			}
			if (powerupScript.onPutInDrawer != null)
			{
				powerupScript.onPutInDrawer(powerupScript);
			}
			if (flag && PowerupScript.onUnequipStatic != null)
			{
				PowerupScript.onUnequipStatic(powerupScript);
			}
			if (PowerupScript.onPutInDrawerStatic != null)
			{
				PowerupScript.onPutInDrawerStatic(powerupScript);
			}
			if (isInitializationCall)
			{
				PowerupScript._initializationTempList.Remove(powerupScript);
			}
			if (DrawersScript.HasEasterEgg(num) && DrawersScript.hasSeenEasterEgg)
			{
				if (DrawersScript.EasterEggGet(num) == DrawersScript.EasterEgg.VynilPlayer)
				{
					switch (powerupScript.identifier)
					{
					case PowerupScript.Identifier.DiscA:
						Music.StopAll();
						Music.Play("OstDemoTrailer");
						Music.Find("OstDemoTrailer").myAudioSource.loop = false;
						break;
					case PowerupScript.Identifier.DiscB:
						Music.StopAll();
						Music.Play("OstReleaseTrailer");
						Music.Find("OstReleaseTrailer").myAudioSource.loop = false;
						break;
					case PowerupScript.Identifier.DiscC:
						Music.StopAll();
						Music.Play("OstCredits");
						Music.Find("OstCredits").myAudioSource.loop = false;
						break;
					}
				}
				DrawersScript.SetEasterEgg(DrawersScript.EasterEgg.Undefined, num);
			}
			return true;
		}
		if (isInitializationCall)
		{
			Debug.LogError("PowerupScript: PutInDrawer: No drawer available for initialization! Identifier: " + identifier.ToString());
			return false;
		}
		CameraGame.Shake(1f);
		Sound.Play("SoundMenuError", 1f, 1f);
		DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_POWERUP_NO_SPACE_IN_DRAWERS" });
		return false;
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x0001F62D File Offset: 0x0001D82D
	public static bool PutInDrawer_Quick(PowerupScript.Identifier kind)
	{
		return PowerupScript.PutInDrawer(kind, false, -1);
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x0001F638 File Offset: 0x0001D838
	public static bool ThrowAway(PowerupScript.Identifier identifier, bool isInitializationCall)
	{
		bool flag = false;
		bool flag2 = false;
		PowerupScript powerupScript = PowerupScript.FindPowerup(identifier, out flag, out flag2);
		if (powerupScript == null)
		{
			Debug.LogError("PowerupScript: ThrowAway: Powerup not found! Identifier: " + identifier.ToString());
			return false;
		}
		if (!isInitializationCall && !flag && !flag2)
		{
			Debug.LogError("PowerupScript: ThrowAway: Powerup not equipped or in drawer! Cannot throw it away! Identifier: " + identifier.ToString());
			return false;
		}
		if (flag2)
		{
			int num = PowerupScript.IsInDrawer(identifier);
			if (num >= 0)
			{
				PowerupScript.array_InDrawer[num] = null;
			}
		}
		if (flag)
		{
			if (powerupScript.category == PowerupScript.Category.skeleton)
			{
				PowerupScript.list_EquippedSkeleton.Remove(powerupScript);
			}
			else
			{
				PowerupScript.list_EquippedNormal.Remove(powerupScript);
			}
		}
		if (!PowerupScript.list_NotBought.Contains(powerupScript))
		{
			PowerupScript.list_NotBought.Add(powerupScript);
		}
		powerupScript.DiegeticStateUpdate(false, PowerupScript.MenuControllerTarget.Room);
		if (!isInitializationCall)
		{
			PowerupScript.RefreshPlacementAll();
		}
		if (!isInitializationCall)
		{
			TrashBinScript.TrashAnimation(!PowerupScript._suppressThrowAwaySound);
		}
		PowerupScript._suppressThrowAwaySound = false;
		if (!isInitializationCall)
		{
			CameraGame.Shake(1f);
		}
		powerupScript.equippedChached = false;
		powerupScript.inDrawerChached = false;
		GameplayData.Powerup_ButtonChargesUsed_Reset(identifier, false);
		GameplayData.Powerup_ButtonBurnedOut_Set(identifier, 0);
		GameplayData.Powerup_ResellBonus_Set(identifier, 0);
		powerupScript.sacredGlowHolder.SetActive(false);
		if (!isInitializationCall && (flag || flag2) && !PowerupScript._suppressThrowAwayAnimation)
		{
			PowerupScript.PlayDiscardedAnimation(identifier);
		}
		PowerupScript._suppressThrowAwayAnimation = false;
		if (flag && powerupScript.onUnequip != null)
		{
			powerupScript.onUnequip(powerupScript);
		}
		if (powerupScript.onThrowAway != null && !isInitializationCall)
		{
			powerupScript.onThrowAway(powerupScript);
		}
		if (flag && PowerupScript.onUnequipStatic != null)
		{
			PowerupScript.onUnequipStatic(powerupScript);
		}
		if (PowerupScript.onThrowAwayStatic != null && !isInitializationCall)
		{
			PowerupScript.onThrowAwayStatic(powerupScript);
		}
		if (isInitializationCall)
		{
			PowerupScript._initializationTempList.Remove(powerupScript);
		}
		if (!isInitializationCall && !powerupScript.isInstantPowerup && flag && !flag2)
		{
			PowerupScript.HoleRomboid_RecordCharmTry(powerupScript.identifier);
		}
		if (!isInitializationCall && !powerupScript.isInstantPowerup)
		{
			Data.game.UnlockableSteps_OnCharmDiscard(1);
		}
		return true;
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x0001F80F File Offset: 0x0001DA0F
	public static void SuppressThrowAwaySound()
	{
		PowerupScript._suppressThrowAwaySound = true;
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x0001F817 File Offset: 0x0001DA17
	public static void SuppressThrowAwayAnimation()
	{
		PowerupScript._suppressThrowAwayAnimation = true;
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x0001F81F File Offset: 0x0001DA1F
	public static void PlayTriggeredAnimation(PowerupScript.Identifier identifier)
	{
		PowerupTriggerAnimController.AddAnimation(PowerupScript.GetPowerup_Quick(identifier), RunModifierScript.Identifier.undefined, PowerupTriggerAnimController.AnimationCapsule.AnimationKind.normalTrigger);
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0001F82F File Offset: 0x0001DA2F
	public static void PlayUnlockedAnimation(PowerupScript.Identifier identifier)
	{
		PowerupTriggerAnimController.AddAnimation(PowerupScript.GetPowerup_Quick(identifier), RunModifierScript.Identifier.undefined, PowerupTriggerAnimController.AnimationCapsule.AnimationKind.unlock);
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x0001F83F File Offset: 0x0001DA3F
	public static void PlayDiscardedAnimation(PowerupScript.Identifier identifier)
	{
		PowerupTriggerAnimController.AddAnimation(PowerupScript.GetPowerup_Quick(identifier), RunModifierScript.Identifier.undefined, PowerupTriggerAnimController.AnimationCapsule.AnimationKind.discard);
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x0001F84F File Offset: 0x0001DA4F
	public static void PlayRechargeAnimation(PowerupScript.Identifier identifier)
	{
		PowerupTriggerAnimController.AddAnimation(PowerupScript.GetPowerup_Quick(identifier), RunModifierScript.Identifier.undefined, PowerupTriggerAnimController.AnimationCapsule.AnimationKind.recharge_RedButton);
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x0001F85F File Offset: 0x0001DA5F
	public static void PlayPowerDownAnimation(PowerupScript.Identifier identifier)
	{
		PowerupTriggerAnimController.AddAnimation(PowerupScript.GetPowerup_Quick(identifier), RunModifierScript.Identifier.undefined, PowerupTriggerAnimController.AnimationCapsule.AnimationKind.powerDown);
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x0001F870 File Offset: 0x0001DA70
	public Bounds? GetBoundingBox()
	{
		if (this.meshRenderer != null)
		{
			return new Bounds?(this.meshRenderer.bounds);
		}
		if (this.skinnedMeshRenderer != null)
		{
			return new Bounds?(this.skinnedMeshRenderer.bounds);
		}
		return null;
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x0001F8C4 File Offset: 0x0001DAC4
	public Vector3 GetBoundingBoxCenter()
	{
		Bounds? boundingBox = this.GetBoundingBox();
		if (boundingBox == null)
		{
			return base.transform.position;
		}
		return boundingBox.Value.center;
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x0001F8FC File Offset: 0x0001DAFC
	public Vector3 GetBoundingBoxSize()
	{
		Bounds? boundingBox = this.GetBoundingBox();
		if (boundingBox == null)
		{
			return Vector3.one * 0.5f;
		}
		return boundingBox.Value.size;
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x0001F938 File Offset: 0x0001DB38
	public Vector3 GetBoundingBoxSizeNormalized()
	{
		if (this._boundsSize == null)
		{
			this._boundsSize = new Vector3?(this.GetBoundingBoxSize());
		}
		return Vector3.one / Mathf.Max(new float[]
		{
			this._boundsSize.Value.x,
			this._boundsSize.Value.y,
			this._boundsSize.Value.z
		});
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x0001F9B4 File Offset: 0x0001DBB4
	public void MeshSteal(Transform targetParent, bool normalizeScale, float scaleMult)
	{
		this.meshIsStolen = true;
		Vector3 boundingBoxSizeNormalized = this.GetBoundingBoxSizeNormalized();
		this.meshHolder.transform.SetParent(targetParent);
		this.meshHolder.transform.localPosition = Vector3.zero;
		this.meshHolder.transform.localRotation = Quaternion.identity;
		this.meshHolder.transform.localScale = (normalizeScale ? boundingBoxSizeNormalized : Vector3.one) * scaleMult;
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x0001FA2C File Offset: 0x0001DC2C
	public void MeshRestore(bool resetMaterial)
	{
		if (!this.meshIsStolen)
		{
			return;
		}
		this.meshHolder.transform.SetParent(base.transform);
		this.meshHolder.transform.localPosition = Vector3.zero;
		this.meshHolder.transform.localRotation = Quaternion.identity;
		this.meshHolder.transform.localScale = Vector3.one;
		if (resetMaterial)
		{
			this.MaterialColorReset();
		}
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0001FAA0 File Offset: 0x0001DCA0
	public bool MeshIsStolen()
	{
		return this.meshIsStolen;
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x0001FAA8 File Offset: 0x0001DCA8
	public void MaterialColorReset()
	{
		if (GameplayData.Powerup_Modifier_Get(this.identifier) != PowerupScript.Modifier.none)
		{
			return;
		}
		if (PowerupScript.IsUnlocked(this.identifier))
		{
			this.MaterialColor(Color.white);
			return;
		}
		if (Data.GameData.IsPowerupSecret(this.identifier))
		{
			this.MaterialColor(Color.black);
			return;
		}
		this.MaterialColor(PowerupScript.colorGrayedOut);
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x0001FB00 File Offset: 0x0001DD00
	public void MaterialColor(Color color)
	{
		if (this.meshRenderer != null)
		{
			this.meshRenderer.sharedMaterial.color = color;
		}
		if (this.skinnedMeshRenderer != null)
		{
			this.skinnedMeshRenderer.sharedMaterial.color = color;
		}
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x0001FB40 File Offset: 0x0001DD40
	public void MaterialRefresh()
	{
		Material material = this.materialDefault;
		PowerupScript.Modifier modifier = GameplayData.Powerup_Modifier_Get(this.identifier);
		if (!this.sacredPropertyApplied)
		{
			this.sacredPropertyApplied = true;
			this.materialDefault.SetFloat("_Sacred", (this.archetype == PowerupScript.Archetype.sacred) ? 1f : 0f);
		}
		if (Util.AngleSin(Tick.PassedTimePausable * 360f) < 0.8f)
		{
			material = this.MaterialModifier_Get(modifier);
		}
		bool flag = !this.MeshIsStolen() && !TerminalScript.IsLoggedIn() && !PowerupTriggerAnimController.HasAnimations();
		this.ApplyModifierEffects((!flag) ? PowerupScript.Modifier.none : modifier);
		if (this.meshRenderer != null && this.meshRenderer.sharedMaterial != material)
		{
			this.meshRenderer.sharedMaterial = material;
		}
		if (this.skinnedMeshRenderer != null && this.skinnedMeshRenderer.sharedMaterial != material)
		{
			this.skinnedMeshRenderer.sharedMaterial = material;
		}
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x0001FC38 File Offset: 0x0001DE38
	public Material MaterialModifier_Get(PowerupScript.Modifier modifier)
	{
		switch (modifier)
		{
		case PowerupScript.Modifier.none:
			return this.materialDefault;
		case PowerupScript.Modifier.symbolMultiplier:
			if (this.materialMod_SymbolMult == null)
			{
				this.materialMod_SymbolMult = AssetMaster.GetGeneric<Material>("MatPowerupMod SymbolMult");
			}
			return this.materialMod_SymbolMult;
		case PowerupScript.Modifier.patternMultiplier:
			if (this.materialMod_PatternMult == null)
			{
				this.materialMod_PatternMult = AssetMaster.GetGeneric<Material>("MatPowerupMod PatternMult");
			}
			return this.materialMod_PatternMult;
		case PowerupScript.Modifier.cloverTicket:
			if (this.materialMod_CloverTicket == null)
			{
				this.materialMod_CloverTicket = AssetMaster.GetGeneric<Material>("MatPowerupMod CloverTicket");
			}
			return this.materialMod_CloverTicket;
		case PowerupScript.Modifier.obsessive:
			if (this.materialMod_Obsessive == null)
			{
				this.materialMod_Obsessive = AssetMaster.GetGeneric<Material>("MatPowerupMod Obsessive");
			}
			return this.materialMod_Obsessive;
		case PowerupScript.Modifier.gambler:
			if (this.materialMod_Gambler == null)
			{
				this.materialMod_Gambler = AssetMaster.GetGeneric<Material>("MatPowerupMod Gambler");
			}
			return this.materialMod_Gambler;
		case PowerupScript.Modifier.speculative:
			if (this.materialMod_Speculative == null)
			{
				this.materialMod_Speculative = AssetMaster.GetGeneric<Material>("MatPowerupMod Speculative");
			}
			return this.materialMod_Speculative;
		case PowerupScript.Modifier.devious:
			if (this.materialMod_Devious == null)
			{
				this.materialMod_Devious = AssetMaster.GetGeneric<Material>("MatPowerupMod Devious");
			}
			return this.materialMod_Devious;
		default:
			Debug.LogError("PowerupScript: MaterialModifier_Get: Modifier not handled! Modifier: " + modifier.ToString());
			return this.materialDefault;
		}
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x0001FD9C File Offset: 0x0001DF9C
	public Material MaterialDefault_Get()
	{
		return this.materialDefault;
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x0001FDA4 File Offset: 0x0001DFA4
	public void ApplyModifierEffects(PowerupScript.Modifier modifier)
	{
		if (this._modifierEffects_LastModifier == modifier)
		{
			return;
		}
		this._modifierEffects_LastModifier = modifier;
		switch (modifier)
		{
		case PowerupScript.Modifier.none:
			this.modEffect_SymbolMult.SetActive(false);
			this.modEffect_PatternMult.SetActive(false);
			this.modEffect_CloverTicket.SetActive(false);
			this.modEffect_Obsessive.SetActive(false);
			this.modEffect_Gambler.SetActive(false);
			this.modEffect_Speculative.SetActive(false);
			this.modEffect_Devious.SetActive(false);
			return;
		case PowerupScript.Modifier.symbolMultiplier:
			this.modEffect_SymbolMult.SetActive(true);
			this.modEffect_PatternMult.SetActive(false);
			this.modEffect_CloverTicket.SetActive(false);
			this.modEffect_Obsessive.SetActive(false);
			this.modEffect_Gambler.SetActive(false);
			this.modEffect_Speculative.SetActive(false);
			this.modEffect_Devious.SetActive(false);
			return;
		case PowerupScript.Modifier.patternMultiplier:
			this.modEffect_SymbolMult.SetActive(false);
			this.modEffect_PatternMult.SetActive(true);
			this.modEffect_CloverTicket.SetActive(false);
			this.modEffect_Obsessive.SetActive(false);
			this.modEffect_Gambler.SetActive(false);
			this.modEffect_Speculative.SetActive(false);
			this.modEffect_Devious.SetActive(false);
			return;
		case PowerupScript.Modifier.cloverTicket:
			this.modEffect_SymbolMult.SetActive(false);
			this.modEffect_PatternMult.SetActive(false);
			this.modEffect_CloverTicket.SetActive(true);
			this.modEffect_Obsessive.SetActive(false);
			this.modEffect_Gambler.SetActive(false);
			this.modEffect_Speculative.SetActive(false);
			this.modEffect_Devious.SetActive(false);
			return;
		case PowerupScript.Modifier.obsessive:
			this.modEffect_SymbolMult.SetActive(false);
			this.modEffect_PatternMult.SetActive(false);
			this.modEffect_CloverTicket.SetActive(false);
			this.modEffect_Obsessive.SetActive(true);
			this.modEffect_Gambler.SetActive(false);
			this.modEffect_Speculative.SetActive(false);
			this.modEffect_Devious.SetActive(false);
			return;
		case PowerupScript.Modifier.gambler:
			this.modEffect_SymbolMult.SetActive(false);
			this.modEffect_PatternMult.SetActive(false);
			this.modEffect_CloverTicket.SetActive(false);
			this.modEffect_Obsessive.SetActive(false);
			this.modEffect_Gambler.SetActive(true);
			this.modEffect_Speculative.SetActive(false);
			this.modEffect_Devious.SetActive(false);
			return;
		case PowerupScript.Modifier.speculative:
			this.modEffect_SymbolMult.SetActive(false);
			this.modEffect_PatternMult.SetActive(false);
			this.modEffect_CloverTicket.SetActive(false);
			this.modEffect_Obsessive.SetActive(false);
			this.modEffect_Gambler.SetActive(false);
			this.modEffect_Speculative.SetActive(true);
			this.modEffect_Devious.SetActive(false);
			return;
		case PowerupScript.Modifier.devious:
			this.modEffect_SymbolMult.SetActive(false);
			this.modEffect_PatternMult.SetActive(false);
			this.modEffect_CloverTicket.SetActive(false);
			this.modEffect_Obsessive.SetActive(false);
			this.modEffect_Gambler.SetActive(false);
			this.modEffect_Speculative.SetActive(false);
			this.modEffect_Devious.SetActive(true);
			return;
		default:
			Debug.LogError("PowerupScript: ApplyModifierEffects: Modifier not handled! Modifier: " + modifier.ToString());
			return;
		}
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x000200B1 File Offset: 0x0001E2B1
	public MeshRenderer MeshRendererGet()
	{
		return this.meshRenderer;
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x000200B9 File Offset: 0x0001E2B9
	public SkinnedMeshRenderer SkinnedMeshRendererGet()
	{
		return this.skinnedMeshRenderer;
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x000200C1 File Offset: 0x0001E2C1
	public Mesh MeshGet()
	{
		if (this.meshFilter != null)
		{
			return this.meshFilter.mesh;
		}
		if (this.skinnedMeshRenderer != null)
		{
			return this.skinnedMeshRenderer.sharedMesh;
		}
		return null;
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x000200F8 File Offset: 0x0001E2F8
	public void ModifierReEvaluate(bool canForceNone, bool forceSomeModifier)
	{
		if (this.isInstantPowerup)
		{
			return;
		}
		PowerupScript.Modifier modifier = PowerupScript.Modifier.none;
		if (!canForceNone || Data.game.runsDone > 3 || DrawersScript.GetDrawersUnlockedNum() >= 1 || !(GameplayData.DebtIndexGet() == 0L))
		{
			float num = R.Rng_PowerupsMod.Value * 100f;
			if (forceSomeModifier)
			{
				num = R.Rng_PowerupsMod.Value * 10.4999f;
			}
			if (num <= 2f)
			{
				modifier = PowerupScript.Modifier.cloverTicket;
			}
			else if (num <= 3.5f)
			{
				modifier = PowerupScript.Modifier.symbolMultiplier;
			}
			else if (num <= 4.5f)
			{
				modifier = PowerupScript.Modifier.patternMultiplier;
			}
			else if (num <= 5.5f)
			{
				modifier = PowerupScript.Modifier.obsessive;
			}
			else if (num <= 7.5f)
			{
				modifier = PowerupScript.Modifier.gambler;
			}
			else if (num <= 9f)
			{
				modifier = PowerupScript.Modifier.speculative;
			}
			else if (num <= 10.5f && DrawersScript.GetDrawersUnlockedNum() > 0)
			{
				modifier = PowerupScript.Modifier.devious;
			}
		}
		GameplayData.Powerup_Modifier_Set(this.identifier, modifier, true);
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x000201C4 File Offset: 0x0001E3C4
	public static void ModifiedPowerups_EquippedCounter_Refresh()
	{
		PowerupScript.modifiedPowerups_EquippedCounter_SymbolMult = 0;
		PowerupScript.modifiedPowerups_EquippedCounter_PatternMult = 0;
		PowerupScript.modifiedPowerups_EquippedCounter_CloverTicket = 0;
		PowerupScript.modifiedPowerups_EquippedCounter_Obsessive = 0;
		PowerupScript.modifiedPowerups_EquippedCounter_Gambler = 0;
		PowerupScript.modifiedPowerups_EquippedCounter_Speculative = 0;
		PowerupScript.modifiedPowerups_EquippedCounter_Devious = 0;
		using (List<PowerupScript>.Enumerator enumerator = PowerupScript.list_EquippedNormal.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				switch (GameplayData.Powerup_Modifier_Get(enumerator.Current.identifier))
				{
				case PowerupScript.Modifier.symbolMultiplier:
					PowerupScript.modifiedPowerups_EquippedCounter_SymbolMult++;
					break;
				case PowerupScript.Modifier.patternMultiplier:
					PowerupScript.modifiedPowerups_EquippedCounter_PatternMult++;
					break;
				case PowerupScript.Modifier.cloverTicket:
					PowerupScript.modifiedPowerups_EquippedCounter_CloverTicket++;
					break;
				case PowerupScript.Modifier.obsessive:
					PowerupScript.modifiedPowerups_EquippedCounter_Obsessive++;
					break;
				case PowerupScript.Modifier.gambler:
					PowerupScript.modifiedPowerups_EquippedCounter_Gambler++;
					break;
				case PowerupScript.Modifier.speculative:
					PowerupScript.modifiedPowerups_EquippedCounter_Speculative++;
					break;
				case PowerupScript.Modifier.devious:
					PowerupScript.modifiedPowerups_EquippedCounter_Devious++;
					break;
				}
			}
		}
		using (List<PowerupScript>.Enumerator enumerator = PowerupScript.list_EquippedSkeleton.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				switch (GameplayData.Powerup_Modifier_Get(enumerator.Current.identifier))
				{
				case PowerupScript.Modifier.symbolMultiplier:
					PowerupScript.modifiedPowerups_EquippedCounter_SymbolMult++;
					break;
				case PowerupScript.Modifier.patternMultiplier:
					PowerupScript.modifiedPowerups_EquippedCounter_PatternMult++;
					break;
				case PowerupScript.Modifier.cloverTicket:
					PowerupScript.modifiedPowerups_EquippedCounter_CloverTicket++;
					break;
				case PowerupScript.Modifier.obsessive:
					PowerupScript.modifiedPowerups_EquippedCounter_Obsessive++;
					break;
				case PowerupScript.Modifier.gambler:
					PowerupScript.modifiedPowerups_EquippedCounter_Gambler++;
					break;
				case PowerupScript.Modifier.speculative:
					PowerupScript.modifiedPowerups_EquippedCounter_Speculative++;
					break;
				case PowerupScript.Modifier.devious:
					PowerupScript.modifiedPowerups_EquippedCounter_Devious++;
					break;
				}
			}
		}
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x000203A0 File Offset: 0x0001E5A0
	public static int ModifiedPowerups_GetCount(PowerupScript.Modifier modifier)
	{
		switch (modifier)
		{
		case PowerupScript.Modifier.symbolMultiplier:
			return PowerupScript.modifiedPowerups_EquippedCounter_SymbolMult;
		case PowerupScript.Modifier.patternMultiplier:
			return PowerupScript.modifiedPowerups_EquippedCounter_PatternMult;
		case PowerupScript.Modifier.cloverTicket:
			return PowerupScript.modifiedPowerups_EquippedCounter_CloverTicket;
		case PowerupScript.Modifier.obsessive:
			return PowerupScript.modifiedPowerups_EquippedCounter_Obsessive;
		case PowerupScript.Modifier.gambler:
			return PowerupScript.modifiedPowerups_EquippedCounter_Gambler;
		case PowerupScript.Modifier.speculative:
			return PowerupScript.modifiedPowerups_EquippedCounter_Speculative;
		case PowerupScript.Modifier.devious:
			return PowerupScript.modifiedPowerups_EquippedCounter_Devious;
		default:
			Debug.LogError("PowerupScript: ModifiedPowerups_GetCount: Modifier not handled! Modifier: " + modifier.ToString());
			return 0;
		}
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x0002041A File Offset: 0x0001E61A
	public static int ModifiedPowerups_GetTicketsBonus()
	{
		return PowerupScript.ModifiedPowerups_GetCount(PowerupScript.Modifier.cloverTicket) * 2;
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00020424 File Offset: 0x0001E624
	public static int ModifiedPowerups_GetInterestBonus()
	{
		return PowerupScript.ModifiedPowerups_GetCount(PowerupScript.Modifier.speculative) * 3;
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0002042E File Offset: 0x0001E62E
	public static float ModifiedPowerups_Get666AdditionalChance()
	{
		return (float)PowerupScript.ModifiedPowerups_GetCount(PowerupScript.Modifier.devious) * 0.006f;
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x00020440 File Offset: 0x0001E640
	public void Inspect()
	{
		if (PowerupScript.inspectedPowerup != null)
		{
			return;
		}
		if (SlotMachineScript.IsSpinning())
		{
			return;
		}
		PowerupScript.inspectedPowerup = this;
		int num = PowerupScript.IsInDrawer(this.identifier);
		string text = null;
		long num2 = this.ResellValueGet();
		if (num2 > 0L)
		{
			text = " +" + num2.ToString() + "<sprite name=\"CloverTicket\">";
		}
		if (num >= 0)
		{
			ScreenMenuScript.OptionEvent[] array = new ScreenMenuScript.OptionEvent[]
			{
				new ScreenMenuScript.OptionEvent(this._InspectClose),
				new ScreenMenuScript.OptionEvent(this._InspectEquipTry),
				new ScreenMenuScript.OptionEvent(this._InspectThrowAway)
			};
			ScreenMenuScript.Open(true, true, 0, ScreenMenuScript.Positioning.down, 0f, Translation.Get("SCREEN_MENU_TITLE_POWERUP"), new string[]
			{
				Translation.Get("SCREEN_MENU_OPTION_POWERUP_CANCEL"),
				Translation.Get("SCREEN_MENU_OPTION_POWERUP_EQUIP"),
				Translation.Get("SCREEN_MENU_OPTION_POWERUP_THROW_AWAY") + text
			}, array);
		}
		else
		{
			int gamePhase = (int)GameplayMaster.GetGamePhase();
			bool flag = true;
			bool flag2 = gamePhase == 5;
			if (this.category == PowerupScript.Category.skeleton)
			{
				ScreenMenuScript.OptionEvent[] array2 = new ScreenMenuScript.OptionEvent[]
				{
					new ScreenMenuScript.OptionEvent(this._InspectClose),
					new ScreenMenuScript.OptionEvent(this._InspectClose),
					new ScreenMenuScript.OptionEvent(this._InspectClose)
				};
				ScreenMenuScript.Open(flag, true, 0, ScreenMenuScript.Positioning.down, 0f, Translation.Get("SCREEN_MENU_TITLE_POWERUP"), new string[]
				{
					Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("SCREEN_MENU_OPTION_POWERUP_CANCEL_SKELETON"), Strings.SanitizationSubKind.none),
					Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("SCREEN_MENU_OPTION_POWERUP_CANCEL_SKELETON"), Strings.SanitizationSubKind.none),
					Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("SCREEN_MENU_OPTION_POWERUP_CANCEL_SKELETON"), Strings.SanitizationSubKind.none)
				}, array2);
			}
			else
			{
				Sound.Play("SoundMenuPopUp", 1f, 1f);
				if (flag2)
				{
					ScreenMenuScript.OptionEvent[] array3 = new ScreenMenuScript.OptionEvent[]
					{
						new ScreenMenuScript.OptionEvent(this._InspectClose)
					};
					ScreenMenuScript.Open(flag, true, 0, ScreenMenuScript.Positioning.downDown, 0f, Translation.Get("SCREEN_MENU_TITLE_POWERUP"), new string[] { Translation.Get("SCREEN_MENU_OPTION_POWERUP_CANCEL") }, array3);
				}
				else
				{
					ScreenMenuScript.OptionEvent[] array4 = new ScreenMenuScript.OptionEvent[]
					{
						new ScreenMenuScript.OptionEvent(this._InspectClose),
						new ScreenMenuScript.OptionEvent(this._InspectPutInDrawerTry),
						new ScreenMenuScript.OptionEvent(this._InspectThrowAway)
					};
					ScreenMenuScript.Open(flag, true, 0, ScreenMenuScript.Positioning.down, 0f, Translation.Get("SCREEN_MENU_TITLE_POWERUP"), new string[]
					{
						Translation.Get("SCREEN_MENU_OPTION_POWERUP_CANCEL"),
						Translation.Get("SCREEN_MENU_OPTION_POWERUP_PUT_IN_DRAWER"),
						Translation.Get("SCREEN_MENU_OPTION_POWERUP_THROW_AWAY") + text
					}, array4);
				}
			}
		}
		InspectorScript.Open_AsPowerup(PowerupScript.inspectedPowerup);
		if (this.inspectionZoomCoroutine == null && !PowerupScript.IsInDrawer_Quick(this.identifier))
		{
			this.inspectionZoomCoroutine = base.StartCoroutine(this.InspectionCameraZoomRoutine());
		}
		if (num < 0)
		{
			VirtualCursors.CursorDesiredVisibilitySet(0, true);
		}
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x000206F7 File Offset: 0x0001E8F7
	private IEnumerator InspectionCameraZoomRoutine()
	{
		PowerupScript._cameraInspectionRunning = true;
		float zoomLevel = 0f;
		CameraController.DisableReason_Add("PIn");
		CameraController.DollyZoomEnable(false);
		Vector3 camera_OldEulers = CameraGame.firstInstance.transform.rotation.eulerAngles;
		Vector3 zero = Vector3.zero;
		if (InspectorScript.instance.textBackImage.rectTransform.sizeDelta.y > 198f)
		{
			zero.y = 0.175f;
		}
		CameraGame.firstInstance.transform.LookAt(this.GetBoundingBoxCenter() + zero);
		Vector3 camera_TargetEulers = CameraGame.firstInstance.transform.eulerAngles;
		CameraGame.firstInstance.transform.eulerAngles = camera_OldEulers;
		if (camera_TargetEulers.x > CameraGame.firstInstance.transform.eulerAngles.x + 180f)
		{
			camera_TargetEulers.x -= 360f;
		}
		if (camera_TargetEulers.x < CameraGame.firstInstance.transform.eulerAngles.x - 180f)
		{
			camera_TargetEulers.x += 360f;
		}
		if (camera_TargetEulers.y > CameraGame.firstInstance.transform.eulerAngles.y + 180f)
		{
			camera_TargetEulers.y -= 360f;
		}
		if (camera_TargetEulers.y < CameraGame.firstInstance.transform.eulerAngles.y - 180f)
		{
			camera_TargetEulers.y += 360f;
		}
		if (camera_TargetEulers.z > CameraGame.firstInstance.transform.eulerAngles.z + 180f)
		{
			camera_TargetEulers.z -= 360f;
		}
		if (camera_TargetEulers.z < CameraGame.firstInstance.transform.eulerAngles.z - 180f)
		{
			camera_TargetEulers.z += 360f;
		}
		while (PowerupScript.inspectedPowerup != null)
		{
			if (PowerupScript.ForceClosingInspection())
			{
				IL_0380:
				CameraController.DollyZoomEnable(true);
				CameraGame.firstInstance.transform.eulerAngles = camera_OldEulers;
				CameraController.DisableReason_Remove("PIn");
				PowerupScript._cameraInspectionRunning = false;
				this.inspectionZoomCoroutine = null;
				PowerupScript._forceClosingInspection_Death = false;
				yield break;
			}
			zoomLevel = Mathf.Lerp(zoomLevel, 1f, Time.unscaledDeltaTime * 20f);
			CameraGame.FieldOfViewExtraSet("pzct", -zoomLevel * 20f);
			CameraGame.firstInstance.transform.eulerAngles = Vector3.Lerp(camera_OldEulers, camera_TargetEulers, zoomLevel);
			yield return null;
		}
		while (zoomLevel > 0f && !PowerupScript.ForceClosingInspection())
		{
			zoomLevel = Mathf.Lerp(zoomLevel, 0f, Time.unscaledDeltaTime * 20f);
			if (zoomLevel < 0.01f)
			{
				zoomLevel = 0f;
			}
			CameraGame.FieldOfViewExtraSet("pzct", -zoomLevel * 20f);
			CameraGame.firstInstance.transform.eulerAngles = Vector3.Lerp(camera_OldEulers, camera_TargetEulers, zoomLevel);
			yield return null;
		}
		goto IL_0380;
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x00020706 File Offset: 0x0001E906
	public static bool CameraIsInspecting()
	{
		return PowerupScript._cameraInspectionRunning;
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x0002070D File Offset: 0x0001E90D
	private void _InspectClose()
	{
		if (PowerupScript.inspectedPowerup == null)
		{
			return;
		}
		int num = PowerupScript.IsInDrawer(this.identifier);
		PowerupScript.inspectedPowerup = null;
		if (num < 0 && GameplayMaster.GetGamePhase() != GameplayMaster.GamePhase.gambling)
		{
			VirtualCursors.CursorDesiredVisibilitySet(0, false);
		}
		DrawersScript.CloseAll();
		InspectorScript.Close();
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x0002074A File Offset: 0x0001E94A
	private void _InspectEquipTry()
	{
		this._InspectClose();
		PowerupScript.Equip(this.identifier, false, false);
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x00020760 File Offset: 0x0001E960
	private void _InspectPutInDrawerTry()
	{
		this._InspectClose();
		PowerupScript.PutInDrawer(this.identifier, false, -1);
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x00020778 File Offset: 0x0001E978
	private void _InspectThrowAway()
	{
		this._InspectClose();
		long num = this.ResellValueGet();
		if (!PowerupScript.ThrowAway(this.identifier, false))
		{
			return;
		}
		if (num > 0L)
		{
			GameplayData.CloverTicketsAdd(num, true);
		}
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x000207AD File Offset: 0x0001E9AD
	public static void ForceCloseInspection_Death()
	{
		PowerupScript._forceClosingInspection_Death = true;
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x000207B5 File Offset: 0x0001E9B5
	private static bool ForceClosingInspection()
	{
		return PowerupScript._forceClosingInspection_Death;
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x000207BC File Offset: 0x0001E9BC
	public static bool RedButtonUsesFinished(PowerupScript.Identifier identifier)
	{
		int num = GameplayData.Powerup_ButtonChargesMax_Get(identifier);
		return GameplayData.Powerup_ButtonChargesUsed_Get(identifier) >= num;
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x000207DC File Offset: 0x0001E9DC
	public static bool RedButtonCallback_IdentityCheck(PowerupScript powerup, PowerupScript.Identifier desiredIdentifier, bool log)
	{
		if (powerup.identifier != desiredIdentifier)
		{
			if (log)
			{
				string text = "RedButton Callback Check failed on powerup! Desired identifier: " + desiredIdentifier.ToString() + " - while Powerup identifier is: " + powerup.identifier.ToString();
				Debug.LogError(text);
				ConsolePrompt.LogError(text, "", 0f);
			}
			return true;
		}
		return false;
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x0002083A File Offset: 0x0001EA3A
	public bool IsRedButtonCharm()
	{
		if (this._isRedButtonCharm == null)
		{
			this._isRedButtonCharm = new bool?(GameplayData.Powerup_ButtonChargesMax_Get(this.identifier) >= 0);
		}
		return this._isRedButtonCharm.Value;
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x00020870 File Offset: 0x0001EA70
	public static bool IsRedButtonCharm(PowerupScript.Identifier identifier)
	{
		PowerupScript powerup_Quick = PowerupScript.GetPowerup_Quick(identifier);
		return !(powerup_Quick == null) && powerup_Quick.IsRedButtonCharm();
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x00020898 File Offset: 0x0001EA98
	private void RedButtonText_PositionInit()
	{
		Bounds? boundingBox = this.GetBoundingBox();
		float num;
		if (boundingBox == null)
		{
			num = 0.75f;
		}
		else
		{
			num = boundingBox.Value.center.y + boundingBox.Value.extents.y - base.transform.position.y + 0.15f;
		}
		this.redButtonUiCanvas.transform.SetLocalY(num);
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x00020910 File Offset: 0x0001EB10
	private void _RedButtonTextRefresh()
	{
		this._redButton_TextUpdateRequest = true;
		this._redButton_TextString = this.GetBatteryString();
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x00020928 File Offset: 0x0001EB28
	public static void RedButtonTextRefresh_All()
	{
		for (int i = 0; i < PowerupScript.list_EquippedNormal.Count; i++)
		{
			PowerupScript.list_EquippedNormal[i]._RedButtonTextRefresh();
		}
		for (int j = 0; j < PowerupScript.list_EquippedSkeleton.Count; j++)
		{
			PowerupScript.list_EquippedSkeleton[j]._RedButtonTextRefresh();
		}
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x00020980 File Offset: 0x0001EB80
	public static bool IsBanned(PowerupScript.Identifier identifer, PowerupScript.Archetype archetype)
	{
		return identifer != PowerupScript.Identifier.undefined && identifer != PowerupScript.Identifier.count && ((GameplayData.RunModifier_GetCurrent() == RunModifierScript.Identifier.smallItemPool && (archetype == PowerupScript.Archetype.symbolInstants || archetype - PowerupScript.Archetype.goldenSymbols <= 2)) || (PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Hole_Circle) && identifer == GameplayData.PowerupHoleCircle_CharmGet()) || (PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Hole_Romboid) && identifer == GameplayData.PowerupHoleRomboid_CharmGet()));
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x000209D6 File Offset: 0x0001EBD6
	private void JustEquippedGlowSet()
	{
		if (this.category == PowerupScript.Category.skeleton)
		{
			return;
		}
		if (this.isInstantPowerup)
		{
			return;
		}
		this.glowHolder.SetActive(true);
		this.glowHolder.transform.localScale = Vector3.one;
		this.justEquippedGlowTimer = 60f;
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00020A18 File Offset: 0x0001EC18
	public static List<PowerupScript.Identifier> _SkeletonPiecesSpawnable_GetListReference(bool computeAvailability)
	{
		if (!computeAvailability)
		{
			return PowerupScript._skeletonPiecesSpawnable;
		}
		PowerupScript._skeletonPiecesSpawnable.Clear();
		PowerupScript._skeletonPiecesSpawnable.Add(PowerupScript.Identifier.Skeleton_Arm1);
		PowerupScript._skeletonPiecesSpawnable.Add(PowerupScript.Identifier.Skeleton_Leg1);
		PowerupScript._skeletonPiecesSpawnable.Add(PowerupScript.Identifier.Skeleton_Arm2);
		PowerupScript._skeletonPiecesSpawnable.Add(PowerupScript.Identifier.Skeleton_Leg2);
		for (int i = 0; i < PowerupScript.array_InDrawer.Length; i++)
		{
			if (!(PowerupScript.array_InDrawer[i] == null))
			{
				PowerupScript.Identifier identifier = PowerupScript.array_InDrawer[i].identifier;
				if (identifier <= PowerupScript.Identifier.Skeleton_Leg2)
				{
					PowerupScript._skeletonPiecesSpawnable.Remove(identifier);
				}
			}
		}
		for (int j = 0; j < PowerupScript.list_EquippedSkeleton.Count; j++)
		{
			if (!(PowerupScript.list_EquippedSkeleton[j] == null))
			{
				PowerupScript.Identifier identifier2 = PowerupScript.list_EquippedSkeleton[j].identifier;
				if (identifier2 <= PowerupScript.Identifier.Skeleton_Leg2)
				{
					PowerupScript._skeletonPiecesSpawnable.Remove(identifier2);
				}
			}
		}
		return PowerupScript._skeletonPiecesSpawnable;
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x00020AF0 File Offset: 0x0001ECF0
	public static PowerupScript SacredCharm_GetRandom(bool onlyIfNotEquippedOrNotInDrawers, bool favorLockedOnes)
	{
		if (favorLockedOnes)
		{
			for (int i = 0; i < PowerupScript.sacredCharms.Count; i++)
			{
				PowerupScript powerupScript = PowerupScript.sacredCharms[i];
				if (!(powerupScript == null) && (!onlyIfNotEquippedOrNotInDrawers || !PowerupScript.IsEquipped_Quick(powerupScript.identifier)) && (!onlyIfNotEquippedOrNotInDrawers || !PowerupScript.IsInDrawer_Quick(powerupScript.identifier)) && !PowerupScript.IsUnlocked(powerupScript.identifier))
				{
					return powerupScript;
				}
			}
		}
		int num = R.Rng_PowerupsAll.Range(0, PowerupScript.sacredCharms.Count);
		for (int j = 0; j < PowerupScript.sacredCharms.Count; j++)
		{
			PowerupScript powerupScript2 = PowerupScript.sacredCharms[num];
			if (!(powerupScript2 == null) && (!onlyIfNotEquippedOrNotInDrawers || !PowerupScript.IsEquipped_Quick(powerupScript2.identifier)) && (!onlyIfNotEquippedOrNotInDrawers || !PowerupScript.IsInDrawer_Quick(powerupScript2.identifier)))
			{
				return powerupScript2;
			}
			num++;
			if (num >= PowerupScript.sacredCharms.Count)
			{
				num = 0;
			}
		}
		return null;
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x00020BD4 File Offset: 0x0001EDD4
	private void PlaceFarAway()
	{
		base.transform.SetParent(null);
		base.transform.position = new Vector3(0f, 0f, 25f + (float)this.identifier * 2.5f);
		base.transform.localScale = PowerupScript.PowerupScaleGet_NotEquipped(this);
		this.myOutline.enabled = false;
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x00020C38 File Offset: 0x0001EE38
	private static void PlaceFarAwayAll()
	{
		foreach (PowerupScript powerupScript in PowerupScript.all)
		{
			powerupScript.PlaceFarAway();
		}
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00020C88 File Offset: 0x0001EE88
	private static void PlacementRefresh_EquippedSkeleton()
	{
		for (int i = 0; i < PowerupScript.list_EquippedSkeleton.Count; i++)
		{
			PowerupScript powerupScript = PowerupScript.list_EquippedSkeleton[i];
			if (powerupScript == null)
			{
				PowerupScript.list_EquippedSkeleton.RemoveAt(i);
				i--;
			}
			else
			{
				powerupScript.transform.SetParent(ItemOrganizerScript.GetDollTransform(i));
				powerupScript.transform.localPosition = Vector3.zero;
				powerupScript.transform.localRotation = Quaternion.identity;
				powerupScript.transform.localScale = PowerupScript.PowerupScaleGet_Equipped(powerupScript);
			}
		}
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x00020D14 File Offset: 0x0001EF14
	private static void PlacementRefresh_EquippedNormal()
	{
		for (int i = 0; i < PowerupScript.list_EquippedNormal.Count; i++)
		{
			PowerupScript powerupScript = PowerupScript.list_EquippedNormal[i];
			if (powerupScript == null)
			{
				PowerupScript.list_EquippedNormal.RemoveAt(i);
				i--;
			}
			else
			{
				powerupScript.transform.SetParent(ItemOrganizerScript.GetOrganizerTransform(i));
				powerupScript.transform.localPosition = Vector3.zero;
				powerupScript.transform.localRotation = Quaternion.identity;
				powerupScript.transform.localScale = PowerupScript.PowerupScaleGet_Equipped(powerupScript);
			}
		}
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00020DA0 File Offset: 0x0001EFA0
	private static void PlacementRefresh_Drawer()
	{
		for (int i = 0; i < PowerupScript.array_InDrawer.Length; i++)
		{
			PowerupScript powerupScript = PowerupScript.array_InDrawer[i];
			if (!(powerupScript == null))
			{
				powerupScript.transform.SetParent(ItemOrganizerScript.GetDrawerTransform(i));
				powerupScript.transform.localPosition = Vector3.zero;
				powerupScript.transform.localRotation = Quaternion.identity;
				powerupScript.transform.localScale = PowerupScript.PowerupScaleGet_InDrawer(powerupScript);
			}
		}
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x00020E14 File Offset: 0x0001F014
	private static void PlacementRefresh_Store()
	{
		for (int i = 0; i < StoreCapsuleScript.storePowerups.Length; i++)
		{
			PowerupScript powerupScript = StoreCapsuleScript.storePowerups[i];
			if (!(powerupScript == null))
			{
				powerupScript.transform.SetParent(ItemOrganizerScript.GetStoreTransform(i));
				powerupScript.transform.localPosition = Vector3.zero;
				powerupScript.transform.localRotation = Quaternion.identity;
				powerupScript.transform.localScale = PowerupScript.PowerupScaleGet_Store(powerupScript);
			}
		}
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x00020E86 File Offset: 0x0001F086
	public static void RefreshPlacementAll()
	{
		PowerupScript.PlaceFarAwayAll();
		PowerupScript.PlacementRefresh_EquippedSkeleton();
		PowerupScript.PlacementRefresh_EquippedNormal();
		PowerupScript.PlacementRefresh_Drawer();
		PowerupScript.PlacementRefresh_Store();
		PowerupScript.ModifiedPowerups_EquippedCounter_Refresh();
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x00020EA6 File Offset: 0x0001F0A6
	public static Vector3 PowerupScaleGet_Equipped(PowerupScript powerup)
	{
		return Vector3.one;
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x00020EAD File Offset: 0x0001F0AD
	public static Vector3 PowerupScaleGet_InDrawer(PowerupScript powerup)
	{
		if (powerup.category == PowerupScript.Category.skeleton)
		{
			return Vector3.one * 1.5f;
		}
		return Vector3.one;
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x00020ECC File Offset: 0x0001F0CC
	public static Vector3 PowerupScaleGet_Store(PowerupScript powerup)
	{
		return Vector3.one;
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x00020ED3 File Offset: 0x0001F0D3
	public static Vector3 PowerupScaleGet_NotEquipped(PowerupScript powerup)
	{
		return Vector3.one;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x00020EDA File Offset: 0x0001F0DA
	public static bool ThrowAwayCanTriggerEffects_Get()
	{
		return PowerupScript.throwAwayCanTriggerEffects;
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00020EE1 File Offset: 0x0001F0E1
	public static void ThrowAwayCanTriggerEffects_Set(bool value)
	{
		PowerupScript.throwAwayCanTriggerEffects = value;
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x00020EE9 File Offset: 0x0001F0E9
	public static int SkeletonPiecesDebtIncreasePercentage()
	{
		return 100 + PowerupScript.list_EquippedSkeleton.Count * 5;
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x00020EFC File Offset: 0x0001F0FC
	public static bool SkeletonFillDrawersWithCharms_Try()
	{
		if (DrawersScript.instance == null)
		{
			return false;
		}
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Skeleton_Head))
		{
			return false;
		}
		int num = PowerupScript.list_EquippedSkeleton.Count - 1;
		if (num <= 0)
		{
			return false;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Skeleton_Head);
		for (int i = 0; i < num; i++)
		{
			int num2 = -1;
			for (int j = 0; j < 4; j++)
			{
				if (DrawersScript.IsDrawerUnlocked(j) && PowerupScript.array_InDrawer[j] == null)
				{
					num2 = j;
					break;
				}
			}
			if (num2 != -1)
			{
				DrawersScript.instance.PutRandomCharmIntoDrawer(-1);
			}
		}
		return true;
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x00020F84 File Offset: 0x0001F184
	public static void SkeletonEreasePiecesLeftIntoDrawers()
	{
		if (DrawersScript.instance == null)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			PowerupScript powerupScript = PowerupScript.array_InDrawer[i];
			if (!(powerupScript == null) && powerupScript.category == PowerupScript.Category.skeleton && powerupScript.identifier != PowerupScript.Identifier.Skeleton_Head)
			{
				PowerupScript.ThrowAway(powerupScript.identifier, false);
			}
		}
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x00020FDA File Offset: 0x0001F1DA
	public static void PFunc_OnEquip_SkeletonPiecesCommon(PowerupScript powerup)
	{
		PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.ThisWillHurt);
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x00020FE3 File Offset: 0x0001F1E3
	public static void ShroomsReset()
	{
		PowerupScript._shromsActivationsCounter = 0;
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00020FEB File Offset: 0x0001F1EB
	public static int ShroomsRawBonusGet()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Mushrooms))
		{
			return 0;
		}
		return PowerupScript._shromsActivationsCounter;
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x00020FFD File Offset: 0x0001F1FD
	public static long ShroomsBonusGet(SymbolScript.Kind symbolKind)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Mushrooms))
		{
			return 0L;
		}
		return (long)(PowerupScript._shromsActivationsCounter * GameplayData.Symbol_CoinsValue_GetBasic(symbolKind));
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x00021018 File Offset: 0x0001F218
	public static void PFunc_OnEquip_Shrooms(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_Shrooms));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.ShroomsReset));
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x00021074 File Offset: 0x0001F274
	public static void PFunc_OnUnequip_Shrooms(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_Shrooms));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.ShroomsReset));
		PowerupScript.ShroomsReset();
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x000210D4 File Offset: 0x0001F2D4
	private static void Trigger_Shrooms()
	{
		int patternsCount = SlotMachineScript.GetPatternsCount();
		if (patternsCount < 3)
		{
			return;
		}
		PowerupScript._shromsActivationsCounter += Mathf.Max(0, patternsCount - 2);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Mushrooms);
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x00021107 File Offset: 0x0001F307
	public static void RorschachReset()
	{
		PowerupScript._rorschachActivationsCounter = 0;
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x0002110F File Offset: 0x0001F30F
	public static double RorschachBonusMultiplierGet()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Rorschach))
		{
			return 0.0;
		}
		return (double)PowerupScript._rorschachActivationsCounter;
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x0002112C File Offset: 0x0001F32C
	public static void PFunc_OnEquip_Rorschach(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_Rorschach));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.RorschachReset));
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x00021188 File Offset: 0x0001F388
	public static void PFunc_OnUnequip_Rorschach(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_Rorschach));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.RorschachReset));
		PowerupScript.RorschachReset();
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x000211E8 File Offset: 0x0001F3E8
	private static void Trigger_Rorschach()
	{
		PatternScript.Kind biggestPatternScored = SlotMachineScript.GetBiggestPatternScored();
		if (biggestPatternScored == PatternScript.Kind.undefined)
		{
			return;
		}
		if (PatternScript.GetElementsCount(biggestPatternScored) < 4)
		{
			return;
		}
		PowerupScript._rorschachActivationsCounter++;
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Rorschach);
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x0002121D File Offset: 0x0001F41D
	public static long CloverPotTicketsBonus(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.CloverPot) && considerEquippedState)
		{
			return 0L;
		}
		return GameplayData.CloverTicketsGet() / 3L;
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00021238 File Offset: 0x0001F438
	public static void PFunc_OnEquip_CloverPot(PowerupScript powerup)
	{
		GameplayMaster instance = GameplayMaster.instance;
		instance.onDeadlineBonus = (GameplayMaster.Event)Delegate.Combine(instance.onDeadlineBonus, new GameplayMaster.Event(PowerupScript.Trigger_CloverPot));
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x00021260 File Offset: 0x0001F460
	public static void PFunc_OnUnequip_CloverPot(PowerupScript powerup)
	{
		GameplayMaster instance = GameplayMaster.instance;
		instance.onDeadlineBonus = (GameplayMaster.Event)Delegate.Remove(instance.onDeadlineBonus, new GameplayMaster.Event(PowerupScript.Trigger_CloverPot));
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00021288 File Offset: 0x0001F488
	private static void Trigger_CloverPot()
	{
		if (PowerupScript.CloverPotTicketsBonus(false) >= 3L)
		{
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.CloverPot);
		}
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x0002129B File Offset: 0x0001F49B
	public static int Hourglass_BonusTicketsGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Hourglass) && considerEquippedState)
		{
			return 0;
		}
		return GameplayData.Powerup_Hourglass_DeadlinesLeftGet();
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x000212B2 File Offset: 0x0001F4B2
	public static void PFunc_OnEquip_Hourglass(PowerupScript powerup)
	{
		GameplayMaster instance = GameplayMaster.instance;
		instance.onDeadlineBonus_Late = (GameplayMaster.Event)Delegate.Combine(instance.onDeadlineBonus_Late, new GameplayMaster.Event(PowerupScript.Trigger_Hourglass));
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x000212DA File Offset: 0x0001F4DA
	public static void PFunc_OnUnequip_Hourglass(PowerupScript powerup)
	{
		GameplayMaster instance = GameplayMaster.instance;
		instance.onDeadlineBonus_Late = (GameplayMaster.Event)Delegate.Remove(instance.onDeadlineBonus_Late, new GameplayMaster.Event(PowerupScript.Trigger_Hourglass));
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00021302 File Offset: 0x0001F502
	public static void PFunc_OnThrowAway_Hourglass(PowerupScript powerup)
	{
		GameplayData.Powerup_Hourglass_DeadlinesLeftReset();
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x00021309 File Offset: 0x0001F509
	public static void Trigger_Hourglass()
	{
		PowerupScript.PlayPowerDownAnimation(PowerupScript.Identifier.Hourglass);
		GameplayData.Powerup_Hourglass_DeadlinesLeftAdd(-1);
		if (GameplayData.Powerup_Hourglass_DeadlinesLeftGet() <= 0)
		{
			PowerupScript.ThrowAway(PowerupScript.Identifier.Hourglass, false);
		}
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x00021329 File Offset: 0x0001F529
	public static int FruitBasketBonusGet()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.FruitBasket))
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x00021338 File Offset: 0x0001F538
	public static void PFunc_OnEquip_FruitBasket(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnRoundBeing = (SlotMachineScript.Event)Delegate.Combine(instance.OnRoundBeing, new SlotMachineScript.Event(PowerupScript.FrutiBasket_OnRoundStart));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.FrutiBasket_OnRoundEnd));
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x00021394 File Offset: 0x0001F594
	public static void PFunc_OnUnequip_FruitBasket(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnRoundBeing = (SlotMachineScript.Event)Delegate.Remove(instance.OnRoundBeing, new SlotMachineScript.Event(PowerupScript.FrutiBasket_OnRoundStart));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.FrutiBasket_OnRoundEnd));
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x000213ED File Offset: 0x0001F5ED
	public static void PFunc_OnThrowAway_FruitBasket(PowerupScript powerup)
	{
		GameplayData.Powerup_FruitBasket_RoundsLeftReset();
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x000213F4 File Offset: 0x0001F5F4
	private static void FrutiBasket_OnRoundStart()
	{
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.FruitBasket);
		GameplayData.Powerup_FruitsBasket_RoundsLeftSet(GameplayData.Powerup_FruitsBasket_RoundsLeftGet() - 1);
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x00021409 File Offset: 0x0001F609
	private static void FrutiBasket_OnRoundEnd()
	{
		if (GameplayData.Powerup_FruitsBasket_RoundsLeftGet() <= 0)
		{
			PowerupScript.ThrowAway(PowerupScript.Identifier.FruitBasket, false);
			return;
		}
		PowerupScript.PlayPowerDownAnimation(PowerupScript.Identifier.FruitBasket);
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x00021424 File Offset: 0x0001F624
	public static void PFunc_OnEquip_7SinsStone(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationBegin = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationBegin, new SlotMachineScript.Event(PowerupScript.Trigger_7SinsStone));
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x0002144C File Offset: 0x0001F64C
	public static void PFunc_OnUnequip_7SinsStone(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationBegin = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationBegin, new SlotMachineScript.Event(PowerupScript.Trigger_7SinsStone));
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x00021474 File Offset: 0x0001F674
	private static void Trigger_7SinsStone()
	{
		if (SlotMachineScript.SymbolsCount(SymbolScript.Kind.seven) < 7)
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.SevenSinsStone);
		SlotMachineScript.Symbol_ReplaceAllVisibleSymbols(SymbolScript.Kind.seven, SymbolScript.Kind.seven, SymbolScript.Modifier.golden, false);
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x00021490 File Offset: 0x0001F690
	public static int NecklaceBonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Necklace) && considerEquippedState)
		{
			return 0;
		}
		return 2;
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x000214A3 File Offset: 0x0001F6A3
	public static void PFunc_OnEquip_CloverBell(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_CloverBell));
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x000214CB File Offset: 0x0001F6CB
	public static void PFunc_OnUnequip_CloverBell(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_CloverBell));
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x000214F4 File Offset: 0x0001F6F4
	private static void Trigger_CloverBell()
	{
		int patternsCount_BySymbol = SlotMachineScript.GetPatternsCount_BySymbol(SymbolScript.Kind.clover);
		int patternsCount_BySymbol2 = SlotMachineScript.GetPatternsCount_BySymbol(SymbolScript.Kind.bell);
		if (patternsCount_BySymbol <= 0 || patternsCount_BySymbol2 <= 0)
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.CloverBell);
		GameplayData.StoreFreeRestocksSet(GameplayData.StoreFreeRestocksGet() + 3L);
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0002152A File Offset: 0x0001F72A
	public static BigInteger CigarettesGetSymbolBonus(SymbolScript.Kind symbKind)
	{
		return GameplayData.Symbol_CoinsValue_GetBasic(symbKind) * GameplayData.Powerup_Cigarettes_ActivationsCounter;
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x0002153D File Offset: 0x0001F73D
	public static void PFunc_OnEquip_Cigarettes(PowerupScript powerup)
	{
		GameplayData.Powerup_Cigarettes_ActivationsCounter++;
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Cigarettes);
		StoreCapsuleScript.Restock(false, true, new PowerupScript[] { PowerupScript.GetPowerup_Quick(PowerupScript.Identifier.Cigarettes) }, false, true);
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x0002156C File Offset: 0x0001F76C
	public static void ElectrcityCounter_TryRecharge()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.ElectricityCounter))
		{
			return;
		}
		bool flag = false;
		List<PowerupScript> list = RedButtonScript.RegisteredPowerupsGet();
		int num = R.Rng_SymbolsMod.Range(0, list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			flag = GameplayData.Powerup_ButtonChargesUsed_RestoreChargesN(list[num].identifier, 1, true);
			if (flag)
			{
				break;
			}
			num++;
			if (num > list.Count - 1)
			{
				num = 0;
			}
		}
		if (flag)
		{
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.ElectricityCounter);
		}
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x000215E0 File Offset: 0x0001F7E0
	public static void PFunc_OnEquip_PotatoPower(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd_Late = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd_Late, new SlotMachineScript.Event(PowerupScript.Trigger_PotatoPower));
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00021608 File Offset: 0x0001F808
	public static void PFunc_OnUnequip_PotatoPower(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd_Late = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd_Late, new SlotMachineScript.Event(PowerupScript.Trigger_PotatoPower));
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x00021630 File Offset: 0x0001F830
	private static void Trigger_PotatoPower()
	{
		if (GameplayData.SpinsWithoutReward_Get() < 2)
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.PotatoPower);
		List<PowerupScript> list = RedButtonScript.RegisteredPowerupsGet();
		for (int i = 0; i < list.Count; i++)
		{
			PowerupScript powerupScript = list[i];
			if (!(powerupScript == null))
			{
				GameplayData.Powerup_ButtonChargesUsed_RestoreChargesN(powerupScript.identifier, 1, true);
			}
		}
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00021683 File Offset: 0x0001F883
	public static void PFunc_OnEquip_CardboardHouse(PowerupScript powerup)
	{
		GameplayData.MaxEquippablePowerupsAdd(1);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.CardboardHouse);
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x00021692 File Offset: 0x0001F892
	public static void PFunc_OnEquip_CrowBar(PowerupScript powerup)
	{
		GameplayData.StoreFreeRestocksSet(GameplayData.StoreFreeRestocksGet() + 3L);
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x000216A1 File Offset: 0x0001F8A1
	public static long ShoppingCart_MultiplierBonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.ShoppingCart) && considerEquippedState)
		{
			return 0L;
		}
		return GameplayData.StoreFreeRestocksGet() * 2L;
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x000216BC File Offset: 0x0001F8BC
	public static void ShoppingCart_TriggerAtDeadlineBegin()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.ShoppingCart))
		{
			return;
		}
		GameplayData.StoreFreeRestocksSet(GameplayData.StoreFreeRestocksGet() + 1L);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.ShoppingCart);
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x000216DC File Offset: 0x0001F8DC
	public static void PFunc_OnEquip_Jimbo(PowerupScript powerup)
	{
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Jimbo);
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.Jimbo_OnRoundEnd));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Combine(instance2.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.Jimbo_OnPreLuckApplication));
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x00021740 File Offset: 0x0001F940
	public static void PFunc_OnUnequip_Jimbo(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.Jimbo_OnRoundEnd));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Remove(instance2.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.Jimbo_OnPreLuckApplication));
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x00021799 File Offset: 0x0001F999
	public static void PFunc_OnThrowAway_Jimbo(PowerupScript powerup)
	{
		GameplayData.Powerup_Jimbo_ReshuffleAndReset();
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x000217A0 File Offset: 0x0001F9A0
	private static void Jimbo_OnRoundEnd()
	{
		if (!GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Bad_Discard3, true) && !GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Bad_Discard6, true))
		{
			return;
		}
		int num = --GameplayData.Powerup_Jimbo_RoundsLeft;
		PowerupScript.PlayPowerDownAnimation(PowerupScript.Identifier.Jimbo);
		if (num <= 0)
		{
			PowerupScript.ThrowAway(PowerupScript.Identifier.Jimbo, false);
		}
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x000217DB File Offset: 0x0001F9DB
	private static void Jimbo_OnPreLuckApplication()
	{
		if (!GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_Luck, true))
		{
			return;
		}
		if (!SlotMachineScript.IsFirstSpinOfRound())
		{
			return;
		}
		GameplayData.ExtraLuck_SetEntry("tl_jmb", 5f, 1, true);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Jimbo);
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x0002180C File Offset: 0x0001FA0C
	public static void PFunc_OnEquip_DiscC(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Combine(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.DiscC_TriggerTry));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnSpinEnd, new SlotMachineScript.Event(PowerupScript.DiscC_FinalizeSpin));
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x00021868 File Offset: 0x0001FA68
	public static void PFunc_OnUnequip_DiscC(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Remove(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.DiscC_TriggerTry));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnSpinEnd, new SlotMachineScript.Event(PowerupScript.DiscC_FinalizeSpin));
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x000218C1 File Offset: 0x0001FAC1
	private static void DiscC_TriggerTry()
	{
		GameplayData.Powerup_DiscC_SpinsCounter++;
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x000218D0 File Offset: 0x0001FAD0
	private static void DiscC_FinalizeSpin()
	{
		int num = GameplayData.Powerup_DiscC_SpinsCounter;
		if (num >= 7)
		{
			num -= 7;
		}
		GameplayData.Powerup_DiscC_SpinsCounter = num;
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x000218F1 File Offset: 0x0001FAF1
	public static bool DiscC_IsTriggeringTime()
	{
		return PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.DiscC) && !SlotMachineScript.Has666() && !SlotMachineScript.Has999() && GameplayData.Powerup_DiscC_SpinsCounter == 7;
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x00021916 File Offset: 0x0001FB16
	public static int DiscC_MissingSpinsGet()
	{
		return 7 - GameplayData.Powerup_DiscC_SpinsCounter;
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x00021920 File Offset: 0x0001FB20
	public static void PFunc_OnEquip_DiscB(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Combine(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.DiscB_TriggerTry));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnSpinEnd, new SlotMachineScript.Event(PowerupScript.DiscB_FinalizeSpin));
	}

	// Token: 0x06000518 RID: 1304 RVA: 0x0002197C File Offset: 0x0001FB7C
	public static void PFunc_OnUnequip_DiscB(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Remove(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.DiscB_TriggerTry));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnSpinEnd, new SlotMachineScript.Event(PowerupScript.DiscB_FinalizeSpin));
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x000219D5 File Offset: 0x0001FBD5
	private static void DiscB_TriggerTry()
	{
		int num = GameplayData.Powerup_DiscB_SpinsCounter + 1;
		if (num == 7)
		{
			GameplayData.ExtraLuck_SetEntry("tl_DskB", 7f, 1, true);
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.DiscB);
		}
		GameplayData.Powerup_DiscB_SpinsCounter = num;
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00021A00 File Offset: 0x0001FC00
	private static void DiscB_FinalizeSpin()
	{
		int num = GameplayData.Powerup_DiscB_SpinsCounter;
		if (num >= 7)
		{
			num -= 7;
		}
		GameplayData.Powerup_DiscB_SpinsCounter = num;
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x00021A21 File Offset: 0x0001FC21
	public static int DiscB_MissingSpinsGet()
	{
		return 7 - GameplayData.Powerup_DiscB_SpinsCounter;
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x00021A2C File Offset: 0x0001FC2C
	public static void PFunc_OnEquip_DiscA(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Combine(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.DiscA_TriggerTry));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnSpinEnd, new SlotMachineScript.Event(PowerupScript.DiscA_FinalizeSpin));
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00021A88 File Offset: 0x0001FC88
	public static void PFunc_OnUnequip_DiscA(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Remove(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.DiscA_TriggerTry));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnSpinEnd, new SlotMachineScript.Event(PowerupScript.DiscA_FinalizeSpin));
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x00021AE1 File Offset: 0x0001FCE1
	private static void DiscA_TriggerTry()
	{
		int num = GameplayData.Powerup_DiscA_SpinsCounter + 1;
		if (num == 7)
		{
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.DiscA);
		}
		GameplayData.Powerup_DiscA_SpinsCounter = num;
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x00021AFC File Offset: 0x0001FCFC
	private static void DiscA_FinalizeSpin()
	{
		int num = GameplayData.Powerup_DiscA_SpinsCounter;
		if (num >= 7)
		{
			num -= 7;
		}
		GameplayData.Powerup_DiscA_SpinsCounter = num;
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x00021B1D File Offset: 0x0001FD1D
	public static bool DiscA_IsTriggeringTime()
	{
		return PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.DiscA) && GameplayData.Powerup_DiscA_SpinsCounter == 7;
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x00021B32 File Offset: 0x0001FD32
	public static int DiscA_MissingSpinsGet()
	{
		return 7 - GameplayData.Powerup_DiscA_SpinsCounter;
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x00021B3B File Offset: 0x0001FD3B
	public static void PFunc_OnEquip_MusicTape(PowerupScript powerup)
	{
		GameplayData.DeadlineRoundsIncrement_Manual(1);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.MusicTape);
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x00021B4C File Offset: 0x0001FD4C
	public static void PFunc_OnEquip_WeirdClock(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.WeirdClock_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.noTiming, null);
		GameplayMaster instance = GameplayMaster.instance;
		instance.onDeadlineBonus = (GameplayMaster.Event)Delegate.Combine(instance.onDeadlineBonus, new GameplayMaster.Event(PowerupScript.WeirdClockDeadlineReset));
		RedButtonScript instance2 = RedButtonScript.instance;
		instance2.onButtonActivatedSomething = (RedButtonScript.RedButtonEvent)Delegate.Combine(instance2.onButtonActivatedSomething, new RedButtonScript.RedButtonEvent(PowerupScript.WeirdClockThrowAwayCheck));
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x00021BBC File Offset: 0x0001FDBC
	public static void PFunc_OnUnequip_WeirdClock(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
		GameplayMaster instance = GameplayMaster.instance;
		instance.onDeadlineBonus = (GameplayMaster.Event)Delegate.Remove(instance.onDeadlineBonus, new GameplayMaster.Event(PowerupScript.WeirdClockDeadlineReset));
		RedButtonScript instance2 = RedButtonScript.instance;
		instance2.onButtonActivatedSomething = (RedButtonScript.RedButtonEvent)Delegate.Remove(instance2.onButtonActivatedSomething, new RedButtonScript.RedButtonEvent(PowerupScript.WeirdClockThrowAwayCheck));
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00021C1B File Offset: 0x0001FE1B
	public static void PFunc_OnThrowAway_WeirdClock(PowerupScript powerup)
	{
		PowerupScript.WeirdClockDeadlineReset();
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00021C22 File Offset: 0x0001FE22
	private static void WeirdClock_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.WeirdClock, true))
		{
			return;
		}
		GameplayData.DeadlineRoundsIncrement_Manual(1);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.WeirdClock);
		GameplayData.Powerup_WeirdClock_DeadlineUses++;
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00021C49 File Offset: 0x0001FE49
	private static void WeirdClockDeadlineReset()
	{
		GameplayData.Powerup_WeirdClock_DeadlineUses = 0;
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x00021C51 File Offset: 0x0001FE51
	private static void WeirdClockThrowAwayCheck()
	{
		if (GameplayData.Powerup_WeirdClock_DeadlineUses >= 5)
		{
			PowerupScript.ThrowAway(PowerupScript.Identifier.WeirdClock, false);
			PowerupScript.WeirdClockDeadlineReset();
		}
	}

	// Token: 0x06000529 RID: 1321 RVA: 0x00021C69 File Offset: 0x0001FE69
	public static void PFunc_OnEquip_SteamLocomotive(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd_Late = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd_Late, new SlotMachineScript.Event(PowerupScript.Trigger_SteamLocomotive));
	}

	// Token: 0x0600052A RID: 1322 RVA: 0x00021C91 File Offset: 0x0001FE91
	public static void PFunc_OnUnequip_SteamLocomotive(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd_Late = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd_Late, new SlotMachineScript.Event(PowerupScript.Trigger_SteamLocomotive));
	}

	// Token: 0x0600052B RID: 1323 RVA: 0x00021CB9 File Offset: 0x0001FEB9
	private static void Trigger_SteamLocomotive()
	{
		if (GameplayData.SpinsWithoutReward_Get() < 3)
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.LocomotiveSteam);
		GameplayData.Powerup_SteamLocomotive_Bonus_Set(GameplayData.Powerup_SteamLocomotive_Bonus_Get() + 1);
	}

	// Token: 0x0600052C RID: 1324 RVA: 0x00021CD7 File Offset: 0x0001FED7
	public static long SteamLocomotive_SymbolsBonus_Get(SymbolScript.Kind symbolKind)
	{
		return (long)(GameplayData.Symbol_CoinsValue_GetBasic(symbolKind) * GameplayData.Powerup_SteamLocomotive_Bonus_Get());
	}

	// Token: 0x0600052D RID: 1325 RVA: 0x00021CE6 File Offset: 0x0001FEE6
	public static void PFunc_OnEquip_DieselLocomotive(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd_Late = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd_Late, new SlotMachineScript.Event(PowerupScript.Trigger_DieselLocomotive));
	}

	// Token: 0x0600052E RID: 1326 RVA: 0x00021D0E File Offset: 0x0001FF0E
	public static void PFunc_OnUnequip_DieselLocomotive(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd_Late = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd_Late, new SlotMachineScript.Event(PowerupScript.Trigger_DieselLocomotive));
	}

	// Token: 0x0600052F RID: 1327 RVA: 0x00021D36 File Offset: 0x0001FF36
	private static void Trigger_DieselLocomotive()
	{
		if (GameplayData.SpinsWithoutReward_Get() < 3)
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.LocomotiveDiesel);
		GameplayData.Powerup_DieselLocomotive_Bonus_Set(GameplayData.Powerup_DieselLocomotive_Bonus_Get() + 1);
	}

	// Token: 0x06000530 RID: 1328 RVA: 0x00021D54 File Offset: 0x0001FF54
	public static double DieselLocomotive_PatternsBonus_Get(PatternScript.Kind patternKind)
	{
		return GameplayData.Pattern_Value_GetBasic(patternKind) * (double)GameplayData.Powerup_DieselLocomotive_Bonus_Get();
	}

	// Token: 0x06000531 RID: 1329 RVA: 0x00021D63 File Offset: 0x0001FF63
	public static void PFunc_OnEquip_Depression(PowerupScript powerup)
	{
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Depression);
	}

	// Token: 0x06000532 RID: 1330 RVA: 0x00021D6C File Offset: 0x0001FF6C
	public static void PFunc_OnEquip_StepsCounter(PowerupScript powerup)
	{
		RedButtonScript instance = RedButtonScript.instance;
		instance.onButtonActivatedSomething = (RedButtonScript.RedButtonEvent)Delegate.Combine(instance.onButtonActivatedSomething, new RedButtonScript.RedButtonEvent(PowerupScript.StepsCounter_ButtonTrigger));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Combine(instance2.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.StepsCounter_SpinTrigger));
	}

	// Token: 0x06000533 RID: 1331 RVA: 0x00021DC8 File Offset: 0x0001FFC8
	public static void PFunc_OnUnequip_StepsCounter(PowerupScript powerup)
	{
		RedButtonScript instance = RedButtonScript.instance;
		instance.onButtonActivatedSomething = (RedButtonScript.RedButtonEvent)Delegate.Remove(instance.onButtonActivatedSomething, new RedButtonScript.RedButtonEvent(PowerupScript.StepsCounter_ButtonTrigger));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Remove(instance2.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.StepsCounter_SpinTrigger));
	}

	// Token: 0x06000534 RID: 1332 RVA: 0x00021E21 File Offset: 0x00020021
	public static void StepsCounter_ButtonTrigger()
	{
		GameplayData.Powerup_StepsCounter_TriggersCounter_Set(GameplayData.Powerup_StepsCounter_TriggersCounter_Get() + 1);
	}

	// Token: 0x06000535 RID: 1333 RVA: 0x00021E30 File Offset: 0x00020030
	public static void StepsCounter_SpinTrigger()
	{
		int num = GameplayData.Powerup_StepsCounter_TriggersCounter_Get();
		if (num < 3)
		{
			return;
		}
		num -= 3;
		GameplayData.Powerup_StepsCounter_TriggersCounter_Set(num);
		GameplayData.ExtraLuck_SetEntry("tl_StpsCntr", 7f, 1, true);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.StepsCounter);
	}

	// Token: 0x06000536 RID: 1334 RVA: 0x00021E6A File Offset: 0x0002006A
	public static int StepsCounter_TriggersNeededGet()
	{
		return 3 - GameplayData.Powerup_StepsCounter_TriggersCounter_Get();
	}

	// Token: 0x06000537 RID: 1335 RVA: 0x00021E74 File Offset: 0x00020074
	public static long DarkLotus_MultiplierBonus_Get(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.DarkLotus) && considerEquippedState)
		{
			return 0L;
		}
		long num = 0L;
		for (int i = 0; i < PowerupScript.list_EquippedNormal.Count; i++)
		{
			PowerupScript powerupScript = PowerupScript.list_EquippedNormal[i];
			if (!(powerupScript == null))
			{
				num += powerupScript.ResellValueGet();
			}
		}
		return num;
	}

	// Token: 0x06000538 RID: 1336 RVA: 0x00021EC9 File Offset: 0x000200C9
	public static void PFunc_OnEquip_FieldOfClovers(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.FieldOfClovers_Trigger));
	}

	// Token: 0x06000539 RID: 1337 RVA: 0x00021EF1 File Offset: 0x000200F1
	public static void PFunc_OnUnequip_FieldOfClovers(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.FieldOfClovers_Trigger));
	}

	// Token: 0x0600053A RID: 1338 RVA: 0x00021F1C File Offset: 0x0002011C
	private static void FieldOfClovers_Trigger()
	{
		for (int i = 0; i < PowerupScript.list_EquippedNormal.Count; i++)
		{
			PowerupScript powerupScript = PowerupScript.list_EquippedNormal[i];
			if (!(powerupScript == null))
			{
				GameplayData.Powerup_ResellBonus_Set(powerupScript.identifier, GameplayData.Powerup_ResellBonus_Get(powerupScript.identifier) + 1);
			}
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.CloversLandPatch);
	}

	// Token: 0x0600053B RID: 1339 RVA: 0x00021F72 File Offset: 0x00020172
	public static void PFunc_OnEquip_ConsolationPrize(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.TriggerConsolationPrize));
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00021F9A File Offset: 0x0002019A
	public static void PFunc_OnUnequip_ConsolationPrize(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.TriggerConsolationPrize));
	}

	// Token: 0x0600053D RID: 1341 RVA: 0x00021FC4 File Offset: 0x000201C4
	public static void TriggerConsolationPrize()
	{
		if (SlotMachineScript.GetPatternsCount() > 0)
		{
			return;
		}
		float num = GameplayData.ActivationLuckGet();
		GameplayData.RndActivationFailsafe_ConsolationPrize++;
		if (R.Rng_Powerup(PowerupScript.Identifier.ConsolationPrize).Value > 0.25f * num && GameplayData.RndActivationFailsafe_ConsolationPrize < 4)
		{
			return;
		}
		GameplayData.RndActivationFailsafe_ConsolationPrize = 0;
		GameplayData.Powerup_ConsolationPrize_Bonus_Set(GameplayData.Powerup_ConsolationPrize_Bonus_Get() + 1L);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.ConsolationPrize);
	}

	// Token: 0x0600053E RID: 1342 RVA: 0x00022024 File Offset: 0x00020224
	public static long ConsolationPrizeBonusGet(SymbolScript.Kind symbolKind)
	{
		return (long)GameplayData.Symbol_CoinsValue_GetBasic(symbolKind) * GameplayData.Powerup_ConsolationPrize_Bonus_Get();
	}

	// Token: 0x0600053F RID: 1343 RVA: 0x00022033 File Offset: 0x00020233
	public static void PFunc_OnEquip_RingBell(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.RingBell_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.noTiming, null);
	}

	// Token: 0x06000540 RID: 1344 RVA: 0x00022049 File Offset: 0x00020249
	public static void PFunc_OnUnequip_RingBell(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
	}

	// Token: 0x06000541 RID: 1345 RVA: 0x00022051 File Offset: 0x00020251
	private static void RingBell_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.RingBell, true))
		{
			return;
		}
		GameplayData.Powerup_RingBell_Bonus_Set(GameplayData.Powerup_RingBell_Bonus_Get() + 1L);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.RingBell);
	}

	// Token: 0x06000542 RID: 1346 RVA: 0x00022073 File Offset: 0x00020273
	public static long RingBell_BonusGet(SymbolScript.Kind symbolKind)
	{
		return (long)GameplayData.Symbol_CoinsValue_GetBasic(symbolKind) * GameplayData.Powerup_RingBell_Bonus_Get();
	}

	// Token: 0x06000543 RID: 1347 RVA: 0x00022082 File Offset: 0x00020282
	public static long AllIn_MultiplierBonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.AllIn) && considerEquippedState)
		{
			return 0L;
		}
		return GameplayData.JackpotsScoredCounter;
	}

	// Token: 0x06000544 RID: 1348 RVA: 0x0002209A File Offset: 0x0002029A
	public static void AllIn_TryTriggeringAnimation()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.AllIn))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.AllIn);
	}

	// Token: 0x06000545 RID: 1349 RVA: 0x000220AD File Offset: 0x000202AD
	public static long Garbage_MultiplierBonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Garbage) && considerEquippedState)
		{
			return 0L;
		}
		return GameplayData.SmallBetPickCount();
	}

	// Token: 0x06000546 RID: 1350 RVA: 0x000220C5 File Offset: 0x000202C5
	public static void Garbage_TryTriggeringAnimation()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Garbage))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Garbage);
	}

	// Token: 0x06000547 RID: 1351 RVA: 0x000220D8 File Offset: 0x000202D8
	public static long VoiceMail_MultiplierBonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.VoiceMailTape) && considerEquippedState)
		{
			return 0L;
		}
		long num = GameplayData.PhoneRerollPerformed_Get();
		if (num < 0L)
		{
			return 0L;
		}
		return num;
	}

	// Token: 0x06000548 RID: 1352 RVA: 0x00022105 File Offset: 0x00020305
	public static void PFunc_OnEquip_Pareidolia(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_Pareidolia));
	}

	// Token: 0x06000549 RID: 1353 RVA: 0x0002212D File Offset: 0x0002032D
	public static void PFunc_OnUnequip_Pareidolia(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_Pareidolia));
	}

	// Token: 0x0600054A RID: 1354 RVA: 0x00022155 File Offset: 0x00020355
	public static double PareidoliaBonusMultiplierGet(PatternScript.Kind kind)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Pareidolia))
		{
			return 0.0;
		}
		return GameplayData.Powerup_PareidoliaMultiplierBonus_Get(kind);
	}

	// Token: 0x0600054B RID: 1355 RVA: 0x00022170 File Offset: 0x00020370
	private static void Trigger_Pareidolia()
	{
		if (SlotMachineScript.GetBiggestPatternScored() != PatternScript.Kind.eye)
		{
			return;
		}
		if (!SlotMachineScript.IsAllSamePattern())
		{
			return;
		}
		int num = 16;
		for (int i = 0; i < num; i++)
		{
			GameplayData.Powerup_PareidoliaMultiplierBonus_Set((PatternScript.Kind)i, GameplayData.Powerup_PareidoliaMultiplierBonus_Get((PatternScript.Kind)i) + GameplayData.Pattern_ValueOverall_Get((PatternScript.Kind)i, true));
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Pareidolia);
	}

	// Token: 0x0600054C RID: 1356 RVA: 0x000221BC File Offset: 0x000203BC
	public static void AbstractPaintingDictEnsure()
	{
		if (PowerupScript._abstractPaintingBonusValue == null)
		{
			PowerupScript._abstractPaintingBonusValue = new Dictionary<PatternScript.Kind, double>();
		}
		if (PowerupScript._abstractPaintingBonusValue.Count > 0)
		{
			return;
		}
		int num = 16;
		for (int i = 0; i < num; i++)
		{
			PowerupScript._abstractPaintingBonusValue.Add((PatternScript.Kind)i, 0.0);
		}
	}

	// Token: 0x0600054D RID: 1357 RVA: 0x0002220C File Offset: 0x0002040C
	public static void PFunc_OnEquip_AbstractPainting(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_AbstractPainting));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.AbstractPaintingReset));
	}

	// Token: 0x0600054E RID: 1358 RVA: 0x00022268 File Offset: 0x00020468
	public static void PFunc_OnUnequip_AbstractPainting(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_AbstractPainting));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.AbstractPaintingReset));
		PowerupScript.AbstractPaintingReset();
	}

	// Token: 0x0600054F RID: 1359 RVA: 0x000222C8 File Offset: 0x000204C8
	public static void AbstractPaintingReset()
	{
		PowerupScript.AbstractPaintingDictEnsure();
		int num = 16;
		for (int i = 0; i < num; i++)
		{
			PowerupScript._abstractPaintingBonusValue[(PatternScript.Kind)i] = 0.0;
		}
	}

	// Token: 0x06000550 RID: 1360 RVA: 0x000222FD File Offset: 0x000204FD
	public static double AbstractPaintingBonusMultiplierGet(PatternScript.Kind kind)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.AbstractPainting))
		{
			return 0.0;
		}
		return PowerupScript._abstractPaintingBonusValue[kind];
	}

	// Token: 0x06000551 RID: 1361 RVA: 0x00022320 File Offset: 0x00020520
	private static void Trigger_AbstractPainting()
	{
		PatternScript.Kind biggestPatternScored = SlotMachineScript.GetBiggestPatternScored();
		if (biggestPatternScored == PatternScript.Kind.undefined)
		{
			return;
		}
		if (PatternScript.GetElementsCount(biggestPatternScored) < 5)
		{
			return;
		}
		PowerupScript.AbstractPaintingDictEnsure();
		int num = 16;
		for (int i = 0; i < num; i++)
		{
			Dictionary<PatternScript.Kind, double> abstractPaintingBonusValue = PowerupScript._abstractPaintingBonusValue;
			PatternScript.Kind kind = (PatternScript.Kind)i;
			abstractPaintingBonusValue[kind] += GameplayData.Pattern_ValueOverall_Get((PatternScript.Kind)i, true);
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.AbstractPainting);
	}

	// Token: 0x06000552 RID: 1362 RVA: 0x0002237E File Offset: 0x0002057E
	public static bool EyeJar_IsTriggerTime()
	{
		return PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.EyeJar) && GameplayData.ConsecutiveSpinsWithDiamondTreasureOrSeven_Get() >= 3 && !SlotMachineScript.Has666() && !SlotMachineScript.Has999();
	}

	// Token: 0x06000553 RID: 1363 RVA: 0x000223A6 File Offset: 0x000205A6
	public static bool Nose_IsTriggerTime()
	{
		return PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Nose) && GameplayData.ConsecutiveSpinsWithout5PlusPatterns_Get() >= 3 && !SlotMachineScript.Has666() && !SlotMachineScript.Has999();
	}

	// Token: 0x06000554 RID: 1364 RVA: 0x000223D0 File Offset: 0x000205D0
	public static void PFunc_OnEquip_ChannelerOfFortune(PowerupScript powerup)
	{
		PowerupTriggerAnimController instance = PowerupTriggerAnimController.instance;
		instance.OnAnimationEnd_Unresettable = (PowerupTriggerAnimController.PowerupEvent)Delegate.Combine(instance.OnAnimationEnd_Unresettable, new PowerupTriggerAnimController.PowerupEvent(PowerupScript.ChannelerOfFortune_GrowActivationsCounter));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Combine(instance2.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.ChannelerOfFortune_ReleaseTry));
	}

	// Token: 0x06000555 RID: 1365 RVA: 0x0002242C File Offset: 0x0002062C
	public static void PFunc_OnUnequip_ChannelerOfFortune(PowerupScript powerup)
	{
		PowerupTriggerAnimController instance = PowerupTriggerAnimController.instance;
		instance.OnAnimationEnd_Unresettable = (PowerupTriggerAnimController.PowerupEvent)Delegate.Remove(instance.OnAnimationEnd_Unresettable, new PowerupTriggerAnimController.PowerupEvent(PowerupScript.ChannelerOfFortune_GrowActivationsCounter));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Remove(instance2.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.ChannelerOfFortune_ReleaseTry));
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00022485 File Offset: 0x00020685
	private static void ChannelerOfFortune_GrowActivationsCounter(PowerupTriggerAnimController.AnimationCapsule animationCapsule)
	{
		if (animationCapsule.powerup.identifier == PowerupScript.Identifier.FortuneChanneler)
		{
			return;
		}
		if (animationCapsule.animationKind != PowerupTriggerAnimController.AnimationCapsule.AnimationKind.normalTrigger)
		{
			return;
		}
		if (GameplayMaster.GetGamePhase() != GameplayMaster.GamePhase.gambling)
		{
			return;
		}
		GameplayData.Powerup_ChannelerOfFortune_ActivationsCounterSet(GameplayData.Powerup_ChannelerOfFortune_ActivationsCounterGet() + 1);
	}

	// Token: 0x06000557 RID: 1367 RVA: 0x000224B8 File Offset: 0x000206B8
	private static void ChannelerOfFortune_ReleaseTry()
	{
		int num = GameplayData.Powerup_ChannelerOfFortune_ActivationsCounterGet();
		if (num < 5)
		{
			return;
		}
		GameplayData.ExtraLuck_SetEntry("tl_frtnChnlr", 7f, 1, true);
		num -= 5;
		GameplayData.Powerup_ChannelerOfFortune_ActivationsCounterSet(num);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.FortuneChanneler);
	}

	// Token: 0x06000558 RID: 1368 RVA: 0x000224F4 File Offset: 0x000206F4
	public static int TheCollector_MultiplierGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.TheCollector) && considerEquippedState)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < PowerupScript.list_EquippedNormal.Count; i++)
		{
			if (!(PowerupScript.list_EquippedNormal[i] == null) && GameplayData.Powerup_Modifier_Get(PowerupScript.list_EquippedNormal[i].identifier) != PowerupScript.Modifier.none)
			{
				num++;
			}
		}
		for (int j = 0; j < PowerupScript.list_EquippedSkeleton.Count; j++)
		{
			if (!(PowerupScript.list_EquippedSkeleton[j] == null) && GameplayData.Powerup_Modifier_Get(PowerupScript.list_EquippedSkeleton[j].identifier) != PowerupScript.Modifier.none)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000559 RID: 1369 RVA: 0x0002259C File Offset: 0x0002079C
	public static void PFunc_OnEquip_BrokenCalculator(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinStart = (SlotMachineScript.Event)Delegate.Combine(instance.OnSpinStart, new SlotMachineScript.Event(PowerupScript.BrokenCalculatorTriggerTry));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnSpinEnd, new SlotMachineScript.Event(PowerupScript.BrokenCalculatorReset));
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x000225F8 File Offset: 0x000207F8
	public static void PFunc_OnUnequip_BrokenCalculator(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinStart = (SlotMachineScript.Event)Delegate.Remove(instance.OnSpinStart, new SlotMachineScript.Event(PowerupScript.BrokenCalculatorTriggerTry));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnSpinEnd, new SlotMachineScript.Event(PowerupScript.BrokenCalculatorReset));
		PowerupScript.BrokenCalculatorReset();
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x00022658 File Offset: 0x00020858
	private static void BrokenCalculatorTriggerTry()
	{
		float num = GameplayData.ActivationLuckGet();
		GameplayData.RndActivationFailsafe_BrokenCalculator++;
		if (R.Rng_Powerup(PowerupScript.Identifier.BrokenCalculator).Value > 0.35f * num && GameplayData.RndActivationFailsafe_BrokenCalculator < 4)
		{
			return;
		}
		GameplayData.RndActivationFailsafe_BrokenCalculator = 0;
		PowerupScript._brokenCalculatorIsActive = true;
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.BrokenCalculator);
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x000226A8 File Offset: 0x000208A8
	private static void BrokenCalculatorReset()
	{
		PowerupScript._brokenCalculatorIsActive = false;
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x000226B0 File Offset: 0x000208B0
	public static bool BrokenCalculatorIsActive()
	{
		return PowerupScript._brokenCalculatorIsActive;
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x000226B7 File Offset: 0x000208B7
	public static int FideltyCard_DiscountGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.FideltyCard) && considerEquippedState)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x000226CA File Offset: 0x000208CA
	public static void PFunc_OnEquip_FideltyCard(PowerupScript powerup)
	{
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.FideltyCard);
		StoreCapsuleScript.RefreshCostTextAll();
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x000226D8 File Offset: 0x000208D8
	public static void PFunc_OnUnequip_FideltyCard(PowerupScript powerup)
	{
		StoreCapsuleScript.RefreshCostTextAll();
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x000226DF File Offset: 0x000208DF
	public static BigInteger GiantShroom_MultiplierBonusGet(SymbolScript.Kind kind)
	{
		return GameplayData.Powerup_GigaMushroom_SymbolValueGet(kind);
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x000226E7 File Offset: 0x000208E7
	public static void PFunc_OnEquip_GiantShroom(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_GiantShroom));
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x0002270F File Offset: 0x0002090F
	public static void PFunc_OnUnequip_GiantShroom(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_GiantShroom));
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x00022738 File Offset: 0x00020938
	private static void Trigger_GiantShroom()
	{
		if (SlotMachineScript.GetPatternsCount() < 15)
		{
			return;
		}
		int num = 9;
		for (int i = 0; i < num; i++)
		{
			GameplayData.Powerup_GigaMushroom_SymbolValueSet((SymbolScript.Kind)i, GameplayData.Powerup_GigaMushroom_SymbolValueGet((SymbolScript.Kind)i) + GameplayData.Symbol_CoinsOverallValue_Get((SymbolScript.Kind)i));
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.GiantShroom);
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x0002277C File Offset: 0x0002097C
	public static void GiantShroom_DeadlineEndReset()
	{
		bool flag = false;
		int num = 9;
		int num2 = 0;
		while (num2 < num && !(PowerupScript.GiantShroom_MultiplierBonusGet((SymbolScript.Kind)num2) <= 0L))
		{
			flag = true;
			GameplayData.Powerup_GigaMushroom_SymbolValueSet((SymbolScript.Kind)num2, 0);
			num2++;
		}
		if (flag)
		{
			PowerupScript.PlayPowerDownAnimation(PowerupScript.Identifier.GiantShroom);
		}
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x000227C4 File Offset: 0x000209C4
	public static void VinesoupDictEnsure()
	{
		if (PowerupScript._vinesoupBonusValue == null)
		{
			PowerupScript._vinesoupBonusValue = new Dictionary<SymbolScript.Kind, BigInteger>();
		}
		if (PowerupScript._vinesoupBonusValue.Count > 0)
		{
			return;
		}
		int num = 9;
		for (int i = 0; i < num; i++)
		{
			PowerupScript._vinesoupBonusValue.Add((SymbolScript.Kind)i, 0);
		}
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x00022810 File Offset: 0x00020A10
	public static void VineShroomsReset()
	{
		PowerupScript.VinesoupDictEnsure();
		int num = 9;
		for (int i = 0; i < num; i++)
		{
			PowerupScript._vinesoupBonusValue[(SymbolScript.Kind)i] = 0;
		}
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x00022842 File Offset: 0x00020A42
	public static BigInteger VineShroomsBonusGet_Min0(SymbolScript.Kind symbolKind)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.VineSoupShroom))
		{
			return 0;
		}
		PowerupScript.VinesoupDictEnsure();
		return PowerupScript._vinesoupBonusValue[symbolKind];
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x00022864 File Offset: 0x00020A64
	public static void PFunc_OnEquip_VineShroom(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_VineShroom));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.VineShroomsReset));
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x000228C0 File Offset: 0x00020AC0
	public static void PFunc_OnUnequip_VineShroom(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_VineShroom));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.VineShroomsReset));
		PowerupScript.VineShroomsReset();
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x00022920 File Offset: 0x00020B20
	private static void Trigger_VineShroom()
	{
		if (SlotMachineScript.GetPatternsCount() < 5)
		{
			return;
		}
		PowerupScript.VinesoupDictEnsure();
		int num = 9;
		for (int i = 0; i < num; i++)
		{
			Dictionary<SymbolScript.Kind, BigInteger> vinesoupBonusValue = PowerupScript._vinesoupBonusValue;
			SymbolScript.Kind kind = (SymbolScript.Kind)i;
			vinesoupBonusValue[kind] += GameplayData.Symbol_CoinsOverallValue_Get((SymbolScript.Kind)i);
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.VineSoupShroom);
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x00022974 File Offset: 0x00020B74
	public static void PFunc_OnEquip_FortuneCookie(PowerupScript powerup)
	{
		for (int i = 0; i < PowerupScript._fortuneCookiePowerups.Length; i++)
		{
			PowerupScript._fortuneCookiePowerups[i] = null;
		}
		int num = 0;
		for (int j = 0; j < PowerupScript.luckBasedCharms.Count; j++)
		{
			PowerupScript powerupScript = PowerupScript.luckBasedCharms[j];
			if (!(powerupScript == null) && PowerupScript.IsUnlocked(powerupScript.identifier) && !PowerupScript.IsEquipped_Quick(powerupScript.identifier) && !PowerupScript.IsInDrawer_Quick(powerupScript.identifier) && Array.IndexOf<PowerupScript>(PowerupScript._fortuneCookiePowerups, powerupScript) < 0 && !PowerupScript.IsBanned(powerupScript.identifier, powerupScript.archetype))
			{
				PowerupScript._fortuneCookiePowerups[num] = powerupScript;
				num++;
				if (num >= PowerupScript._fortuneCookiePowerups.Length)
				{
					break;
				}
			}
		}
		StoreCapsuleScript.Restock(false, true, PowerupScript._fortuneCookiePowerups, false, true);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.FortuneCookie);
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x00022A3D File Offset: 0x00020C3D
	public static void WolfTrigger(bool considerEquippedState, PowerupScript powerup)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Wolf) && considerEquippedState)
		{
			return;
		}
		if (powerup.identifier == PowerupScript.Identifier.Wolf)
		{
			return;
		}
		GameplayData.InterestEarnedGrow();
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Wolf);
	}

	// Token: 0x0600056E RID: 1390 RVA: 0x00022A65 File Offset: 0x00020C65
	public static void PFunc_OnEquip_YellowStar(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationBegin = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationBegin, new SlotMachineScript.Event(PowerupScript.Trigger_YellowStar));
	}

	// Token: 0x0600056F RID: 1391 RVA: 0x00022A8D File Offset: 0x00020C8D
	public static void PFunc_OnUnequip_YellowStar(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationBegin = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationBegin, new SlotMachineScript.Event(PowerupScript.Trigger_YellowStar));
	}

	// Token: 0x06000570 RID: 1392 RVA: 0x00022AB8 File Offset: 0x00020CB8
	private static void Trigger_YellowStar()
	{
		if (GameplayData.SpinsWithoutReward_Get() < 1)
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.YellowStar);
		List<SlotMachineScript.PatternInfos> patternsOfKind = SlotMachineScript.instance.GetPatternsOfKind(PatternScript.Kind.horizontal5);
		for (int i = 0; i < patternsOfKind.Count; i++)
		{
			if (patternsOfKind[i].positions[0].x == 0 && patternsOfKind[i].positions[0].y == 1)
			{
				return;
			}
		}
		SymbolScript.Kind kind = GameplayData.Symbol_GetRandom_BasedOnSymbolChance();
		SlotMachineScript.Symbol_ReplaceVisible(kind, SymbolScript.Modifier.none, 0, 1, true);
		SlotMachineScript.Symbol_ReplaceVisible(kind, SymbolScript.Modifier.none, 1, 1, true);
		SlotMachineScript.Symbol_ReplaceVisible(kind, SymbolScript.Modifier.none, 2, 1, true);
		SlotMachineScript.Symbol_ReplaceVisible(kind, SymbolScript.Modifier.none, 3, 1, true);
		SlotMachineScript.Symbol_ReplaceVisible(kind, SymbolScript.Modifier.none, 4, 1, true);
	}

	// Token: 0x06000571 RID: 1393 RVA: 0x00022B62 File Offset: 0x00020D62
	public static BigInteger Calendar_GetSymbolBonus(bool considerEquippedState, SymbolScript.Kind symbolKind)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Calendar) && considerEquippedState)
		{
			return 0;
		}
		return GameplayData.Powerup_Calendar_SymbolsIncreaseN_Get() * GameplayData.Symbol_CoinsValue_GetBasic(symbolKind);
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x00022B8E File Offset: 0x00020D8E
	public static void Calendar_IncreaseBonus(bool considerEquippedState, int roundsSkipped)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Calendar) && considerEquippedState)
		{
			return;
		}
		GameplayData.Powerup_Calendar_SymbolsIncreaseN_Set(GameplayData.Powerup_Calendar_SymbolsIncreaseN_Get() + roundsSkipped + 1);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Calendar);
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x00022BC6 File Offset: 0x00020DC6
	public static void PFunc_OnEquip_MoneyBriefCase(PowerupScript powerup)
	{
		GameplayData.CoinsAdd(GameplayData.DebtGet() * 30 / 100, true);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.MoneyBriefCase);
	}

	// Token: 0x06000574 RID: 1396 RVA: 0x00022BF4 File Offset: 0x00020DF4
	public static long Wallet_PatternsMultiplierBonus(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Wallet) && considerEquippedState)
		{
			return 0L;
		}
		long num = GameplayData.CloverTicketsGet() / 15L;
		if (num < 0L)
		{
			num = 0L;
		}
		return num;
	}

	// Token: 0x06000575 RID: 1397 RVA: 0x00022C28 File Offset: 0x00020E28
	public static int PoopJar_CurrentlyActiveN(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.PoopJar) && considerEquippedState)
		{
			return 0;
		}
		int num = GameplayData.Powerup_ButtonBurnedOut_Get(PowerupScript.Identifier.PoopJar);
		if (num <= 0)
		{
			return 0;
		}
		return num;
	}

	// Token: 0x06000576 RID: 1398 RVA: 0x00022C54 File Offset: 0x00020E54
	public static void PFunc_OnEquip_PoopJar(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.PoopJar_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.perRound, null);
	}

	// Token: 0x06000577 RID: 1399 RVA: 0x00022C6A File Offset: 0x00020E6A
	public static void PFunc_OnUnequip_PoopJar(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
	}

	// Token: 0x06000578 RID: 1400 RVA: 0x00022C72 File Offset: 0x00020E72
	private static void PoopJar_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.PoopJar, true))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.PoopJar);
	}

	// Token: 0x06000579 RID: 1401 RVA: 0x00022C88 File Offset: 0x00020E88
	public static int PissJar_CurrentlyActiveN(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.PissJar) && considerEquippedState)
		{
			return 0;
		}
		int num = GameplayData.Powerup_ButtonBurnedOut_Get(PowerupScript.Identifier.PissJar);
		if (num <= 0)
		{
			return 0;
		}
		return num;
	}

	// Token: 0x0600057A RID: 1402 RVA: 0x00022CB4 File Offset: 0x00020EB4
	public static void PFunc_OnEquip_PissJar(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.PissJar_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.perRound, null);
	}

	// Token: 0x0600057B RID: 1403 RVA: 0x00022CCA File Offset: 0x00020ECA
	public static void PFunc_OnUnequip_PissJar(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x00022CD2 File Offset: 0x00020ED2
	private static void PissJar_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.PissJar, true))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.PissJar);
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x00022CE8 File Offset: 0x00020EE8
	public static int GoldenHandMidaTouch_CurrentlyActiveN(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GoldenHand_MidasTouch))
		{
			return 0;
		}
		int num = GameplayData.Powerup_ButtonBurnedOut_Get(PowerupScript.Identifier.GoldenHand_MidasTouch);
		if (num <= 0)
		{
			return 0;
		}
		return num;
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x00022D12 File Offset: 0x00020F12
	public static void PFunc_OnEquip_GoldenHandMidaTouch(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.GoldenHandMidaTouch_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.perRound, null);
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x00022D28 File Offset: 0x00020F28
	public static void PFunc_OnUnequip_GoldenHandMidaTouch(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x00022D30 File Offset: 0x00020F30
	private static void GoldenHandMidaTouch_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.GoldenHand_MidasTouch, true))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.GoldenHand_MidasTouch);
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x00022D45 File Offset: 0x00020F45
	public static bool ExpiredMeds_SettingChancesToZeroForSymbol(SymbolScript.Kind symbolKind, bool considerEquippedState)
	{
		return (PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.ExpiredMedicines) || !considerEquippedState) && GameplayData.MostValuableSymbols_GetList().Contains(symbolKind);
	}

	// Token: 0x06000582 RID: 1410 RVA: 0x00022D62 File Offset: 0x00020F62
	public static void PFunc_OnEquip_CloverVoucher(PowerupScript powerup)
	{
		GameplayData.CloverTicketsAdd(4L, true);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.CloverVoucher);
	}

	// Token: 0x06000583 RID: 1411 RVA: 0x00022D73 File Offset: 0x00020F73
	public static void PFunc_OnEquip_CarBattery(PowerupScript powerup)
	{
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.CarBattery);
		GameplayData.Powerup_ButtonChargesUsed_ResetAll(true);
	}

	// Token: 0x06000584 RID: 1412 RVA: 0x00022D83 File Offset: 0x00020F83
	public static float StonksInterestBonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Stonks) && considerEquippedState)
		{
			return 0f;
		}
		return 5f;
	}

	// Token: 0x06000585 RID: 1413 RVA: 0x00022D9E File Offset: 0x00020F9E
	public static void PFunc_OnEquip_ToyTrain(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Combine(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.Trigger_ToyTrain));
	}

	// Token: 0x06000586 RID: 1414 RVA: 0x00022DC6 File Offset: 0x00020FC6
	public static void PFunc_OnUnequip_ToyTrain(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Remove(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.Trigger_ToyTrain));
	}

	// Token: 0x06000587 RID: 1415 RVA: 0x00022DF0 File Offset: 0x00020FF0
	private static void Trigger_ToyTrain()
	{
		int num = GameplayData.SpinsWithoutReward_Get();
		if (num < 2)
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.ToyTrain);
		int num2 = num - 2;
		float num3 = 2f * (float)num2;
		num3 = Mathf.Min(num3, 10f);
		GameplayData.ExtraLuck_SetEntry("tl_TTrain", 5f + num3, 1, true);
	}

	// Token: 0x06000588 RID: 1416 RVA: 0x00022E3B File Offset: 0x0002103B
	public static float ScratchAndWin_ChanceBonusGet(bool considerEquippedState, SymbolScript.Kind symbolKind)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GrattaEVinci_ScratchAndWin) && considerEquippedState)
		{
			return 0f;
		}
		if (!GameplayData.MostProbableSymbols_GetList().Contains(symbolKind))
		{
			return 0f;
		}
		return 0.8f;
	}

	// Token: 0x06000589 RID: 1417 RVA: 0x00022E69 File Offset: 0x00021069
	public static void PFunc_OnEquip_CrankGenerator(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.Trigger_CrankGenerator));
	}

	// Token: 0x0600058A RID: 1418 RVA: 0x00022E91 File Offset: 0x00021091
	public static void PFunc_OnUnequip_CrankGenerator(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.Trigger_CrankGenerator));
	}

	// Token: 0x0600058B RID: 1419 RVA: 0x00022EBC File Offset: 0x000210BC
	private static void Trigger_CrankGenerator()
	{
		float num = GameplayData.ActivationLuckGet();
		GameplayData.RndActivationFailsafe_CrankGenerator++;
		if (R.Rng_Powerup(PowerupScript.Identifier.CrankGenerator).Value > 0.4f * num && GameplayData.RndActivationFailsafe_CrankGenerator < 3)
		{
			return;
		}
		GameplayData.RndActivationFailsafe_CrankGenerator = 0;
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.CrankGenerator);
		List<PowerupScript> list = RedButtonScript.RegisteredPowerupsGet();
		for (int i = 0; i < list.Count; i++)
		{
			PowerupScript powerupScript = list[i];
			if (!(powerupScript == null))
			{
				GameplayData.Powerup_ButtonChargesUsed_RestoreChargesN(powerupScript.identifier, 1, true);
			}
		}
	}

	// Token: 0x0600058C RID: 1420 RVA: 0x00022F3C File Offset: 0x0002113C
	public static void PFunc_OnEquip_SuperCapacitor(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_SuperCapacitor));
	}

	// Token: 0x0600058D RID: 1421 RVA: 0x00022F64 File Offset: 0x00021164
	public static void PFunc_OnUnequip_SuperCapacitor(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_SuperCapacitor));
	}

	// Token: 0x0600058E RID: 1422 RVA: 0x00022F8C File Offset: 0x0002118C
	private static void Trigger_SuperCapacitor()
	{
		List<PowerupScript> list = RedButtonScript.RegisteredPowerupsGet();
		if (list.Count <= 0)
		{
			return;
		}
		int num = RedButtonScript.TriggersAvailableGetCount();
		if (num <= 0)
		{
			return;
		}
		if (num == list.Count)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < num; i++)
		{
			int num2 = R.Rng_Powerup(PowerupScript.Identifier.SuperCapacitor).Range(0, list.Count);
			for (int j = 0; j < list.Count; j++)
			{
				flag = GameplayData.Powerup_ButtonChargesUsed_RestoreChargesN(list[num2].identifier, 1, true);
				if (flag)
				{
					break;
				}
				num2++;
				if (num2 > list.Count - 1)
				{
					num2 = 0;
				}
			}
		}
		if (flag)
		{
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.SuperCapacitor);
		}
	}

	// Token: 0x0600058F RID: 1423 RVA: 0x0002302D File Offset: 0x0002122D
	public static int Megaphone_SpaceMalusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Megaphone) && considerEquippedState)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x06000590 RID: 1424 RVA: 0x00023040 File Offset: 0x00021240
	public static int Megaphone_PickMultiplierBonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Megaphone) && considerEquippedState)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x06000591 RID: 1425 RVA: 0x00023053 File Offset: 0x00021253
	public static void PFunc_OnEquip_DearDiary(PowerupScript powerup)
	{
		GameplayData.PhoneAbilitiesNumber_SetToMAX();
	}

	// Token: 0x06000592 RID: 1426 RVA: 0x0002305A File Offset: 0x0002125A
	public static void PFunc_OnUnequip_DearDiary(PowerupScript powerup)
	{
		GameplayData.PhoneAbilitiesNumber_SetToDefault();
	}

	// Token: 0x06000593 RID: 1427 RVA: 0x00023061 File Offset: 0x00021261
	public static int Button2x_SpaceMalusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Button2X) && considerEquippedState)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x06000594 RID: 1428 RVA: 0x00023074 File Offset: 0x00021274
	public static int Button2X_ActivationsMultiplierBonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Button2X) && considerEquippedState)
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x06000595 RID: 1429 RVA: 0x00023088 File Offset: 0x00021288
	public static long CloverPetSymbolsMultiplierBonus(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.CloverPet) && considerEquippedState)
		{
			return 0L;
		}
		long num = GameplayData.CloverTicketsGet() / 5L;
		if (num < 0L)
		{
			num = 0L;
		}
		return num;
	}

	// Token: 0x06000596 RID: 1430 RVA: 0x000230B8 File Offset: 0x000212B8
	public static int CatTreatsBonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.CatTreats) && considerEquippedState)
		{
			return 0;
		}
		return 2;
	}

	// Token: 0x06000597 RID: 1431 RVA: 0x000230CB File Offset: 0x000212CB
	public static int HouseContractBonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.HouseContract) && considerEquippedState)
		{
			return 0;
		}
		return 2;
	}

	// Token: 0x06000598 RID: 1432 RVA: 0x000230DE File Offset: 0x000212DE
	public static void PFunc_OnEquip_HouseContract(PowerupScript powerup)
	{
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.HouseContract);
		PowerupScript.Unlock(PowerupScript.Identifier.CardboardHouse);
	}

	// Token: 0x06000599 RID: 1433 RVA: 0x000230EF File Offset: 0x000212EF
	public static int PentacleBonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Pentacle) && considerEquippedState)
		{
			return 0;
		}
		return GameplayData.Powerup_Pentacle_TriggeredTimesGet() + 1;
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x00023108 File Offset: 0x00021308
	public static void PFunc_OnEquip_Pentacle(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_Pentacle));
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x00023130 File Offset: 0x00021330
	public static void PFunc_OnUnequip_Pentacle(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_Pentacle));
		PowerupScript.ShroomsReset();
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x0002315D File Offset: 0x0002135D
	private static void Trigger_Pentacle()
	{
		if (SlotMachineScript.GetPatternsCount() < 5)
		{
			return;
		}
		GameplayData.Powerup_Pentacle_TriggeredTimesAdd(1);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Pentacle);
	}

	// Token: 0x0600059D RID: 1437 RVA: 0x00023175 File Offset: 0x00021375
	public static int GrandmasPurse_ExtraInterestGet(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GrandmasPurse))
		{
			return 0;
		}
		return GameplayData.Powerup_GrandmasPurse_ExtraInterestGet();
	}

	// Token: 0x0600059E RID: 1438 RVA: 0x0002318A File Offset: 0x0002138A
	public static void PFunc_OnEquip_GrandmasPurse(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnInterestEarnPost = (SlotMachineScript.Event)Delegate.Combine(instance.OnInterestEarnPost, new SlotMachineScript.Event(PowerupScript.PFunc_GrandmasPurse_OnInterestPost));
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x000231B2 File Offset: 0x000213B2
	public static void PFunc_OnUnequip_GrandmasPurse(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnInterestEarnPost = (SlotMachineScript.Event)Delegate.Remove(instance.OnInterestEarnPost, new SlotMachineScript.Event(PowerupScript.PFunc_GrandmasPurse_OnInterestPost));
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x000231DA File Offset: 0x000213DA
	public static void PFunc_OnThrowAway_GrandmasPurse(PowerupScript powerup)
	{
		GameplayData.Powerup_GrandmasPurse_ExtraInterestReset();
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x000231E1 File Offset: 0x000213E1
	private static void PFunc_GrandmasPurse_OnInterestPost()
	{
		PowerupScript.PlayPowerDownAnimation(PowerupScript.Identifier.GrandmasPurse);
		GameplayData.Powerup_GrandmasPurse_ExtraInterestAdd(-3);
		if (GameplayData.Powerup_GrandmasPurse_ExtraInterestGet() <= 0)
		{
			PowerupScript.ThrowAway(PowerupScript.Identifier.GrandmasPurse, false);
		}
	}

	// Token: 0x060005A2 RID: 1442 RVA: 0x00023202 File Offset: 0x00021402
	public static BigInteger TarotDeckRewardGet(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.TarotDeck))
		{
			return 0;
		}
		return GameplayData.Powerup_TarotDeck_RewardGet();
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x0002321C File Offset: 0x0002141C
	public static void PFunc_OnEquip_TarotDeck(PowerupScript powerup)
	{
		PowerupTriggerAnimController instance = PowerupTriggerAnimController.instance;
		instance.OnAnimationEnd_Unresettable = (PowerupTriggerAnimController.PowerupEvent)Delegate.Combine(instance.OnAnimationEnd_Unresettable, new PowerupTriggerAnimController.PowerupEvent(PowerupScript.Trigger_TarotDeck));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnSpinEnd, new SlotMachineScript.Event(PowerupScript.PFunc_OnSpinEnd_TarotDeck));
	}

	// Token: 0x060005A4 RID: 1444 RVA: 0x00023278 File Offset: 0x00021478
	public static void PFunc_OnUnequip_TarotDeck(PowerupScript powerup)
	{
		PowerupTriggerAnimController instance = PowerupTriggerAnimController.instance;
		instance.OnAnimationEnd_Unresettable = (PowerupTriggerAnimController.PowerupEvent)Delegate.Remove(instance.OnAnimationEnd_Unresettable, new PowerupTriggerAnimController.PowerupEvent(PowerupScript.Trigger_TarotDeck));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnSpinEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnSpinEnd, new SlotMachineScript.Event(PowerupScript.PFunc_OnSpinEnd_TarotDeck));
	}

	// Token: 0x060005A5 RID: 1445 RVA: 0x000232D4 File Offset: 0x000214D4
	private static void Trigger_TarotDeck(PowerupTriggerAnimController.AnimationCapsule animationCapsule)
	{
		if (animationCapsule.powerup.identifier == PowerupScript.Identifier.TarotDeck)
		{
			return;
		}
		if (animationCapsule.animationKind != PowerupTriggerAnimController.AnimationCapsule.AnimationKind.normalTrigger)
		{
			return;
		}
		if (!SlotMachineScript.IsSpinningBeforeCoinsReward())
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.TarotDeck);
		PowerupScript._tarotDeck_TriggersPerSpin += 1L;
		GameplayData.Powerup_TarotDeck_RewardAdd(1);
	}

	// Token: 0x060005A6 RID: 1446 RVA: 0x00023321 File Offset: 0x00021521
	private static void PFunc_OnSpinEnd_TarotDeck()
	{
		if (PowerupScript._tarotDeck_TriggersPerSpin <= 0L && GameplayData.Powerup_TarotDeck_RewardGet() > 0L)
		{
			GameplayData.Powerup_TarotDeck_RewardSet(0);
			PowerupScript.PlayPowerDownAnimation(PowerupScript.Identifier.TarotDeck);
		}
		PowerupScript._tarotDeck_TriggersPerSpin = 0L;
	}

	// Token: 0x060005A7 RID: 1447 RVA: 0x00023353 File Offset: 0x00021553
	public static void PFunc_OnEquip_FakeCoin(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_FakeCoin));
	}

	// Token: 0x060005A8 RID: 1448 RVA: 0x0002337B File Offset: 0x0002157B
	public static void PFunc_OnUnequip_FakeCoin(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_FakeCoin));
	}

	// Token: 0x060005A9 RID: 1449 RVA: 0x000233A4 File Offset: 0x000215A4
	private static void Trigger_FakeCoin()
	{
		float num = GameplayData.ActivationLuckGet();
		GameplayData.RndActivationFailsafe_FakeCoin++;
		if (R.Rng_Powerup(PowerupScript.Identifier.FakeCoin).Value < 0.1f * num || GameplayData.RndActivationFailsafe_FakeCoin >= 10)
		{
			GameplayData.SpinsLeftAdd(1);
			GeneralUiScript.CoinsTextForceUpdate();
			GameplayData.ExtraLuck_SetEntry("tl_LckCn", 4f, 1, true);
			GameplayData.RndActivationFailsafe_FakeCoin = 0;
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.FakeCoin);
			Data.GameData game = Data.game;
			int unlockSteps_AncientCoin = game.UnlockSteps_AncientCoin;
			game.UnlockSteps_AncientCoin = unlockSteps_AncientCoin + 1;
			return;
		}
	}

	// Token: 0x060005AA RID: 1450 RVA: 0x00023420 File Offset: 0x00021620
	public static void PFunc_OnEquip_AncientCoin(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.AncientCoin_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.noTiming, null);
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.AncientCoinTrigger));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.AncientCoinOnRoundEnd));
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x00023490 File Offset: 0x00021690
	public static void PFunc_OnUnequip_AncientCoin(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.AncientCoinTrigger));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.AncientCoinOnRoundEnd));
		PowerupScript.AncientCoinOnRoundEnd();
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x000234F4 File Offset: 0x000216F4
	private static void AncientCoin_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.AncientCoin, true))
		{
			return;
		}
		GameplayData.Powerup_AncientCoin_SpinsLeftSet(GameplayData.Powerup_AncientCoin_SpinsLeftGet() + 1);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.AncientCoin);
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x00023518 File Offset: 0x00021718
	private static void AncientCoinTrigger()
	{
		if (GameplayData.Powerup_AncientCoin_SpinsLeftGet() <= 0)
		{
			return;
		}
		GameplayData.Powerup_AncientCoin_SpinsLeftSet(GameplayData.Powerup_AncientCoin_SpinsLeftGet() - 1);
		GameplayData.SpinsLeftAdd(1);
		GeneralUiScript.CoinsTextForceUpdate();
		GameplayData.ExtraLuck_SetEntry("tl_AncntCn", 7f, 1, true);
		PowerupScript._ancientCoinRoundTriggersCounter++;
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.AncientCoin);
		if (PowerupScript._ancientCoinRoundTriggersCounter > 3 && R.Rng_Powerup(PowerupScript.Identifier.AncientCoin).Value > 0.85f)
		{
			PowerupScript.ThrowAway(PowerupScript.Identifier.AncientCoin, false);
		}
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x0002358D File Offset: 0x0002178D
	private static void AncientCoinOnRoundEnd()
	{
		PowerupScript._ancientCoinRoundTriggersCounter = 0;
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x00023598 File Offset: 0x00021798
	public static void PFunc_OnEquip_OneTrickPony(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnRoundBeing = (SlotMachineScript.Event)Delegate.Combine(instance.OnRoundBeing, new SlotMachineScript.Event(PowerupScript.OneTrickPony_EvaluateTargetSpin_AtRoundBegin));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnScoreEvaluationBegin = (SlotMachineScript.Event)Delegate.Combine(instance2.OnScoreEvaluationBegin, new SlotMachineScript.Event(PowerupScript.Trigger_OneTrickPony));
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x000235F4 File Offset: 0x000217F4
	public static void PFunc_OnUnequip_OneTrickPony(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnRoundBeing = (SlotMachineScript.Event)Delegate.Remove(instance.OnRoundBeing, new SlotMachineScript.Event(PowerupScript.OneTrickPony_EvaluateTargetSpin_AtRoundBegin));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnScoreEvaluationBegin = (SlotMachineScript.Event)Delegate.Remove(instance2.OnScoreEvaluationBegin, new SlotMachineScript.Event(PowerupScript.Trigger_OneTrickPony));
	}

	// Token: 0x060005B1 RID: 1457 RVA: 0x00023650 File Offset: 0x00021850
	private static void OneTrickPony_EvaluateTargetSpin_AtRoundBegin()
	{
		int num = GameplayData.SpinsLeftGet();
		GameplayData.Powerup_OneTrickPony_TargetSpinIndexSet(R.Rng_Powerup(PowerupScript.Identifier.OneTrickPony).Range(0, num));
	}

	// Token: 0x060005B2 RID: 1458 RVA: 0x00023676 File Offset: 0x00021876
	private static void Trigger_OneTrickPony()
	{
		if (GameplayData.Powerup_OneTrickPony_TargetSpinIndexGet() != GameplayData.SpinsLeftGet())
		{
			return;
		}
		SlotMachineScript.Symbol_ReplaceAllVisible(GameplayData.Symbol_GetRandom_BasedOnSymbolChance(), SymbolScript.Modifier.none, true);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.OneTrickPony);
		PowerupScript.ThrowAway(PowerupScript.Identifier.OneTrickPony, false);
	}

	// Token: 0x060005B3 RID: 1459 RVA: 0x000236A1 File Offset: 0x000218A1
	public static void PFunc_OnEquip_Ankh(PowerupScript powerup)
	{
		GameplayMaster instance = GameplayMaster.instance;
		instance.onDeathLastChance = (GameplayMaster.Event)Delegate.Combine(instance.onDeathLastChance, new GameplayMaster.Event(PowerupScript.Trigger_Ankh));
	}

	// Token: 0x060005B4 RID: 1460 RVA: 0x000236C9 File Offset: 0x000218C9
	public static void PFunc_OnUnequip_Ankh(PowerupScript powerup)
	{
		GameplayMaster instance = GameplayMaster.instance;
		instance.onDeathLastChance = (GameplayMaster.Event)Delegate.Remove(instance.onDeathLastChance, new GameplayMaster.Event(PowerupScript.Trigger_Ankh));
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x000236F4 File Offset: 0x000218F4
	private static void Trigger_Ankh()
	{
		GameplayData.DeadlineRoundsIncrement_Manual(2);
		PowerupTriggerAnimController instance = PowerupTriggerAnimController.instance;
		instance.OnAnimationStart = (PowerupTriggerAnimController.PowerupEvent)Delegate.Combine(instance.OnAnimationStart, new PowerupTriggerAnimController.PowerupEvent(PowerupScript.AnkhOnTriggerAnimStart));
		PowerupTriggerAnimController instance2 = PowerupTriggerAnimController.instance;
		instance2.OnAnimationEnd = (PowerupTriggerAnimController.PowerupEvent)Delegate.Combine(instance2.OnAnimationEnd, new PowerupTriggerAnimController.PowerupEvent(PowerupScript.AnkhOnTriggerAnimEnd));
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Ankh);
		PowerupTriggerAnimController.AnimationSetSpeed(1f);
		PowerupScript.ThrowAway(PowerupScript.Identifier.Ankh, false);
		ScreenMenuScript.ForceClose_Death();
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x00023770 File Offset: 0x00021970
	private static void AnkhOnTriggerAnimStart(PowerupTriggerAnimController.AnimationCapsule animationCapsule)
	{
		if (animationCapsule.powerup.identifier != PowerupScript.Identifier.Ankh)
		{
			return;
		}
		if (animationCapsule.animationKind != PowerupTriggerAnimController.AnimationCapsule.AnimationKind.normalTrigger)
		{
			return;
		}
		PowerupScript._ankhCameraPositionRestore = CameraController.GetPositionKind();
		CameraController.SetPosition(CameraController.PositionKind.TrapDoor, false, 1f);
		TrapdoorScript.SetAnimation(TrapdoorScript.AnimationKind.Shake);
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x000237A8 File Offset: 0x000219A8
	private static void AnkhOnTriggerAnimEnd(PowerupTriggerAnimController.AnimationCapsule animationCapsule)
	{
		if (animationCapsule.powerup.identifier != PowerupScript.Identifier.Ankh)
		{
			return;
		}
		if (animationCapsule.animationKind != PowerupTriggerAnimController.AnimationCapsule.AnimationKind.normalTrigger)
		{
			return;
		}
		DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_POWERUP_ANKH_SAVE" });
		if (PowerupScript._ankhCameraPositionRestore != CameraController.PositionKind.Undefined && PowerupScript._ankhCameraPositionRestore != CameraController.PositionKind.Count)
		{
			CameraController.SetPosition(PowerupScript._ankhCameraPositionRestore, false, 1f);
		}
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x00023803 File Offset: 0x00021A03
	public static float HorseShoesLuckGet()
	{
		return PowerupScript.horseShoesLuck;
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x0002380A File Offset: 0x00021A0A
	public static void PFunc_OnEquip_HorseShoe(PowerupScript powerup)
	{
		PowerupScript.horseShoesLuck += 1f;
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.HorseShoe);
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x00023822 File Offset: 0x00021A22
	public static void PFunc_OnUnequip_HorseShoe(PowerupScript powerup)
	{
		PowerupScript.horseShoesLuck -= 1f;
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x00023834 File Offset: 0x00021A34
	public static float GoldenHorseShoe_RandomActivationChanceBonusGet(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.HorseShoeGold))
		{
			return 0f;
		}
		if (GameplayData.Powerup_GoldenHorseShoe_SpinsLeftGet() <= 0)
		{
			return 0f;
		}
		return 1000f;
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x0002385C File Offset: 0x00021A5C
	public static void PFunc_OnEquip_GoldenHorseShoe(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.GoldenHorseShoe_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.noTiming, null);
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnSpinEnd, new SlotMachineScript.Event(PowerupScript.GoldenHorseShoe_DecreaseSpinsForBonus));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.GoldenHorseShoe_OnRoundEndReset));
	}

	// Token: 0x060005BD RID: 1469 RVA: 0x000238CC File Offset: 0x00021ACC
	public static void PFunc_OnUnequip_GoldenHorseShoe(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnSpinEnd, new SlotMachineScript.Event(PowerupScript.GoldenHorseShoe_DecreaseSpinsForBonus));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.GoldenHorseShoe_OnRoundEndReset));
		GameplayData.Powerup_GoldenHorseShoe_SpinsLeftSet(0);
	}

	// Token: 0x060005BE RID: 1470 RVA: 0x00023931 File Offset: 0x00021B31
	private static void GoldenHorseShoe_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.HorseShoeGold, true))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.HorseShoeGold);
		GameplayData.Powerup_GoldenHorseShoe_SpinsLeftSet(GameplayData.Powerup_GoldenHorseShoe_SpinsLeftGet() + 1);
		PowerupScript._goldenHorseShoeRoundUsesCounter++;
	}

	// Token: 0x060005BF RID: 1471 RVA: 0x0002395C File Offset: 0x00021B5C
	public static void GoldenHorseShoe_DecreaseSpinsForBonus()
	{
		GameplayData.Powerup_GoldenHorseShoe_SpinsLeftSet(GameplayData.Powerup_GoldenHorseShoe_SpinsLeftGet() - 1);
		if (PowerupScript._goldenHorseShoeRoundUsesCounter > 5 && R.Rng_Powerup(PowerupScript.Identifier.HorseShoeGold).Value > 0.9f)
		{
			PowerupScript.ThrowAway(PowerupScript.Identifier.HorseShoeGold, false);
		}
	}

	// Token: 0x060005C0 RID: 1472 RVA: 0x0002398C File Offset: 0x00021B8C
	private static void GoldenHorseShoe_OnRoundEndReset()
	{
		PowerupScript._goldenHorseShoeRoundUsesCounter = 0;
	}

	// Token: 0x060005C1 RID: 1473 RVA: 0x00023994 File Offset: 0x00021B94
	public static BigInteger _LuckyCatBonusCoinsGet()
	{
		double num = (double)GameplayData.PowerupCoinsMultiplierGet();
		return GameplayData.InterestEarnedHypotetically() * (BigInteger)(num * 100.0) / 100;
	}

	// Token: 0x060005C2 RID: 1474 RVA: 0x000239CE File Offset: 0x00021BCE
	public static void PFunc_OnEquip_LuckyCat(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_LuckyCat));
	}

	// Token: 0x060005C3 RID: 1475 RVA: 0x000239F6 File Offset: 0x00021BF6
	public static void PFunc_OnUnequip_LuckyCat(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_LuckyCat));
	}

	// Token: 0x060005C4 RID: 1476 RVA: 0x00023A20 File Offset: 0x00021C20
	private static void Trigger_LuckyCat()
	{
		if (SlotMachineScript.GetPatternsCount() < 3)
		{
			return;
		}
		SlotMachineScript.SpinExtraCoinsAdd(PowerupScript._LuckyCatBonusCoinsGet());
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.LuckyCat);
		Data.GameData game = Data.game;
		int unlockSteps_LuckyCatFat = game.UnlockSteps_LuckyCatFat;
		game.UnlockSteps_LuckyCatFat = unlockSteps_LuckyCatFat + 1;
	}

	// Token: 0x060005C5 RID: 1477 RVA: 0x00023A5C File Offset: 0x00021C5C
	public static BigInteger _LuckyCatFatBonusCoinsGet()
	{
		double num = (double)GameplayData.PowerupCoinsMultiplierGet();
		return GameplayData.InterestEarnedHypotetically() * (BigInteger)(num * 100.0) / 100 * 2;
	}

	// Token: 0x060005C6 RID: 1478 RVA: 0x00023AA1 File Offset: 0x00021CA1
	public static void PFunc_OnEquip_LuckyCatFat(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_LuckyCatFat));
	}

	// Token: 0x060005C7 RID: 1479 RVA: 0x00023AC9 File Offset: 0x00021CC9
	public static void PFunc_OnUnequip_LuckyCatFat(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_LuckyCatFat));
	}

	// Token: 0x060005C8 RID: 1480 RVA: 0x00023AF4 File Offset: 0x00021CF4
	private static void Trigger_LuckyCatFat()
	{
		if (SlotMachineScript.GetPatternsCount() < 7)
		{
			return;
		}
		SlotMachineScript.SpinExtraCoinsAdd(PowerupScript._LuckyCatFatBonusCoinsGet());
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.LuckyCatFat);
		Data.GameData game = Data.game;
		int unlockSteps_LuckyCatSwole = game.UnlockSteps_LuckyCatSwole;
		game.UnlockSteps_LuckyCatSwole = unlockSteps_LuckyCatSwole + 1;
	}

	// Token: 0x060005C9 RID: 1481 RVA: 0x00023B30 File Offset: 0x00021D30
	public static BigInteger _LuckyCatSwoleBonusCoinsGet()
	{
		double num = (double)GameplayData.PowerupCoinsMultiplierGet();
		return GameplayData.InterestEarnedHypotetically() * (BigInteger)(num * 100.0) / 100 * 4;
	}

	// Token: 0x060005CA RID: 1482 RVA: 0x00023B75 File Offset: 0x00021D75
	public static void PFunc_OnEquip_LuckyCatSwole(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_LuckyCatSwole));
	}

	// Token: 0x060005CB RID: 1483 RVA: 0x00023B9D File Offset: 0x00021D9D
	public static void PFunc_OnUnequip_LuckyCatSwole(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_LuckyCatSwole));
	}

	// Token: 0x060005CC RID: 1484 RVA: 0x00023BC5 File Offset: 0x00021DC5
	private static void Trigger_LuckyCatSwole()
	{
		if (SlotMachineScript.GetPatternsCount() < 15)
		{
			return;
		}
		SlotMachineScript.SpinExtraCoinsAdd(PowerupScript._LuckyCatSwoleBonusCoinsGet());
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.LuckyCatSwole);
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x00023BE2 File Offset: 0x00021DE2
	public static void PFunc_OnEquip_InvertedHamsa(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Combine(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.Trigger_InvertedHamsa));
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x00023C0A File Offset: 0x00021E0A
	public static void PFunc_OnUnequip_InvertedHamsa(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Remove(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.Trigger_InvertedHamsa));
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x00023C34 File Offset: 0x00021E34
	private static void Trigger_InvertedHamsa()
	{
		if (GameplayData.SpinsLeftGet() > 0)
		{
			return;
		}
		GameplayData.ExtraLuck_SetEntry("tl_hamsa", 7f, 1, true);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.HamsaInverted);
		Data.GameData game = Data.game;
		int unlockSteps_HamsaUpside = game.UnlockSteps_HamsaUpside;
		game.UnlockSteps_HamsaUpside = unlockSteps_HamsaUpside + 1;
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x00023C76 File Offset: 0x00021E76
	public static void PFunc_OnEquip_UpsideHamsa(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Combine(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.Trigger_UpsideHamsa));
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x00023C9E File Offset: 0x00021E9E
	public static void PFunc_OnUnequip_UpsideHamsa(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Remove(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.Trigger_UpsideHamsa));
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x00023CC6 File Offset: 0x00021EC6
	private static void Trigger_UpsideHamsa()
	{
		if (!PowerupScript._HamsaUpside_CanTrigger())
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.HamsaUpside);
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x00023CD6 File Offset: 0x00021ED6
	public static int UpsideHamsa_BonusTriggersGet()
	{
		if (!PowerupScript._HamsaUpside_CanTrigger())
		{
			return 0;
		}
		return 1;
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x00023CE2 File Offset: 0x00021EE2
	private static bool _HamsaUpside_CanTrigger()
	{
		return PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.HamsaUpside) && SlotMachineScript.IsFirstSpinOfRound();
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x00023CF8 File Offset: 0x00021EF8
	public static float CrystalLuckIncreaseGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.RedCrystal) && considerEquippedState)
		{
			return 0f;
		}
		int num = GameplayData.Powerup_ButtonBurnedOut_Get(PowerupScript.Identifier.RedCrystal);
		if (num <= 0)
		{
			return 0f;
		}
		return 4f * (float)num * GameplayData.PowerupLuckGet();
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x00023D39 File Offset: 0x00021F39
	public static void PFunc_OnEquip_RedCrystal(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.RedCrystal_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.perSpin, null);
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x00023D4F File Offset: 0x00021F4F
	public static void PFunc_OnUnequip_RedCrystal(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x00023D57 File Offset: 0x00021F57
	private static void RedCrystal_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.RedCrystal, true))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.RedCrystal);
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x00023D6C File Offset: 0x00021F6C
	public static BigInteger PoopBeetle_ExtraCoinsForAllSymbolsGet(bool considerEquippedState, SymbolScript.Kind symbolKind)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.PoopBeetle))
		{
			return 0;
		}
		return GameplayData.Powerup_PoopBeetle_SymbolsIncreaseN_Get() * GameplayData.Symbol_CoinsValue_GetBasic(symbolKind);
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x00023D96 File Offset: 0x00021F96
	public static void PFunc_OnEquip_PoopBeetle(PowerupScript powerup)
	{
		PowerupScript.onThrowAwayStatic = (PowerupScript.PowerupEvent)Delegate.Combine(PowerupScript.onThrowAwayStatic, new PowerupScript.PowerupEvent(PowerupScript.Trigger_PoopBeetle));
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x00023DB8 File Offset: 0x00021FB8
	public static void PFunc_OnUnequip_PoopBeetle(PowerupScript powerup)
	{
		PowerupScript.onThrowAwayStatic = (PowerupScript.PowerupEvent)Delegate.Remove(PowerupScript.onThrowAwayStatic, new PowerupScript.PowerupEvent(PowerupScript.Trigger_PoopBeetle));
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x00023DDA File Offset: 0x00021FDA
	private static void Trigger_PoopBeetle(PowerupScript powerup)
	{
		if (!PowerupScript.ThrowAwayCanTriggerEffects_Get())
		{
			return;
		}
		if (powerup.identifier == PowerupScript.Identifier.PoopBeetle)
		{
			return;
		}
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.PoopBeetle))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.PoopBeetle);
		GameplayData.Powerup_PoopBeetle_SymbolsIncreaseN_Add(1);
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x00023E0B File Offset: 0x0002200B
	public static int RedPepper_ActivationsLeft()
	{
		return 12 - GameplayData.Powerup_RedPepper_ActivationsCounterGet();
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x00023E15 File Offset: 0x00022015
	public static void PFunc_OnEquip_SpicyPepper_Red(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Combine(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.PFunc_Trigger_SpicyPepper_Red));
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x00023E3D File Offset: 0x0002203D
	public static void PFunc_OnUnequip_SpicyPepper_Red(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Remove(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.PFunc_Trigger_SpicyPepper_Red));
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x00023E65 File Offset: 0x00022065
	public static void PFunc_OnThrowAway_SpicyPepper_Red(PowerupScript powerup)
	{
		GameplayData.Powerup_RedPepper_ActivationsCounterSet(0);
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x00023E70 File Offset: 0x00022070
	public static void PFunc_Trigger_SpicyPepper_Red()
	{
		float num = GameplayData.ActivationLuckGet();
		GameplayData.RndActivationFailsafe_RedPepper++;
		if (R.Rng_Powerup(PowerupScript.Identifier.HornChilyRed).Value > 0.2f * num && GameplayData.RndActivationFailsafe_RedPepper <= 5)
		{
			return;
		}
		GameplayData.RndActivationFailsafe_RedPepper = 0;
		GameplayData.ExtraLuck_SetEntry("tl_spPeppR", 5f, 1, true);
		GameplayData.Powerup_RedPepper_ActivationsCounterAdd(1);
		if (GameplayData.Powerup_RedPepper_ActivationsCounterGet() >= 12)
		{
			PowerupTriggerAnimController instance = PowerupTriggerAnimController.instance;
			instance.OnAnimationEnd = (PowerupTriggerAnimController.PowerupEvent)Delegate.Combine(instance.OnAnimationEnd, new PowerupTriggerAnimController.PowerupEvent(PowerupScript._RedPepper_RemoveAtLimit));
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.HornChilyRed);
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x00023F00 File Offset: 0x00022100
	private static void _RedPepper_RemoveAtLimit(PowerupTriggerAnimController.AnimationCapsule animationCapsule)
	{
		if (animationCapsule.powerup.identifier != PowerupScript.Identifier.HornChilyRed)
		{
			return;
		}
		if (animationCapsule.animationKind != PowerupTriggerAnimController.AnimationCapsule.AnimationKind.normalTrigger)
		{
			return;
		}
		PowerupScript.ThrowAway(PowerupScript.Identifier.HornChilyRed, false);
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x00023F24 File Offset: 0x00022124
	public static int GreenPepper_ActivationsLeft()
	{
		return 9 - GameplayData.Powerup_GreenPepper_ActivationsCounterGet();
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x00023F2E File Offset: 0x0002212E
	public static void PFunc_OnEquip_SpicyPepper_Green(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Combine(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.PFunc_Trigger_SpicyPepper_Green));
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x00023F56 File Offset: 0x00022156
	public static void PFunc_OnUnequip_SpicyPepper_Green(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Remove(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.PFunc_Trigger_SpicyPepper_Green));
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x00023F7E File Offset: 0x0002217E
	public static void PFunc_OnThrowAway_SpicyPepper_Green(PowerupScript powerup)
	{
		GameplayData.Powerup_GreenPepper_ActivationsCounterSet(0);
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x00023F88 File Offset: 0x00022188
	public static void PFunc_Trigger_SpicyPepper_Green()
	{
		float num = GameplayData.ActivationLuckGet();
		GameplayData.RndActivationFailsafe_GreenPepper++;
		if (R.Rng_Powerup(PowerupScript.Identifier.HornChilyGreen).Value > 0.15f * num && GameplayData.RndActivationFailsafe_GreenPepper <= 7)
		{
			return;
		}
		GameplayData.RndActivationFailsafe_GreenPepper = 0;
		GameplayData.ExtraLuck_SetEntry("tl_spPeppG", 7f, 1, true);
		GameplayData.Powerup_GreenPepper_ActivationsCounterAdd(1);
		if (GameplayData.Powerup_GreenPepper_ActivationsCounterGet() >= 9)
		{
			PowerupTriggerAnimController instance = PowerupTriggerAnimController.instance;
			instance.OnAnimationEnd = (PowerupTriggerAnimController.PowerupEvent)Delegate.Combine(instance.OnAnimationEnd, new PowerupTriggerAnimController.PowerupEvent(PowerupScript._GreenPepper_RemoveAtLimit));
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.HornChilyGreen);
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x00024018 File Offset: 0x00022218
	private static void _GreenPepper_RemoveAtLimit(PowerupTriggerAnimController.AnimationCapsule animationCapsule)
	{
		if (animationCapsule.powerup.identifier != PowerupScript.Identifier.HornChilyGreen)
		{
			return;
		}
		if (animationCapsule.animationKind != PowerupTriggerAnimController.AnimationCapsule.AnimationKind.normalTrigger)
		{
			return;
		}
		PowerupScript.ThrowAway(PowerupScript.Identifier.HornChilyGreen, false);
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0002403C File Offset: 0x0002223C
	public static void PFunc_OnEquip_GoldenPepper(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Combine(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.TriggerGoldenPepper));
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x00024064 File Offset: 0x00022264
	public static void PFunc_OnUnequip_GoldenPepper(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Remove(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.TriggerGoldenPepper));
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0002408C File Offset: 0x0002228C
	public static void PFunc_OnThrowAway_GoldenPepper(PowerupScript powerup)
	{
		GameplayData.Powerup_GoldenPepper_LuckBonusSet(0);
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x00024094 File Offset: 0x00022294
	public static void TriggerGoldenPepper()
	{
		float num = (float)GameplayData.Powerup_GoldenPepper_LuckBonusGet();
		if (num <= 0f)
		{
			return;
		}
		float num2 = GameplayData.ActivationLuckGet();
		GameplayData.RndActivationFailsafe_GoldenPepper++;
		if (R.Rng_Powerup(PowerupScript.Identifier.GoldenPepper).Value > 0.15f * num2 && GameplayData.RndActivationFailsafe_GoldenPepper <= 7)
		{
			return;
		}
		GameplayData.RndActivationFailsafe_GoldenPepper = 0;
		GameplayData.ExtraLuck_SetEntry("tl_spGdPpr", num, 1, true);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.GoldenPepper);
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x000240FC File Offset: 0x000222FC
	public static void PFunc_OnEquip_RottenPepper(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Combine(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.TriggerRottenPepper));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.RottenPepper_GrowthTrigger));
		GameplayMaster instance3 = GameplayMaster.instance;
		instance3.onDeadlineBonus_Late = (GameplayMaster.Event)Delegate.Combine(instance3.onDeadlineBonus_Late, new GameplayMaster.Event(PowerupScript.RottenPepper_Reset));
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0002417C File Offset: 0x0002237C
	public static void PFunc_OnUnequip_RottenPepper(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Remove(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.TriggerRottenPepper));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.RottenPepper_GrowthTrigger));
		GameplayMaster instance3 = GameplayMaster.instance;
		instance3.onDeadlineBonus_Late = (GameplayMaster.Event)Delegate.Remove(instance3.onDeadlineBonus_Late, new GameplayMaster.Event(PowerupScript.RottenPepper_Reset));
		PowerupScript.RottenPepper_Reset();
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x00024200 File Offset: 0x00022400
	public static void PFunc_OnThrowAway_RottenPepper(PowerupScript powerup)
	{
		PowerupScript.RottenPepper_Reset();
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x00024208 File Offset: 0x00022408
	public static void TriggerRottenPepper()
	{
		float num = (float)GameplayData.Powerup_RottenPepper_LuckBonusGet();
		if (num <= 0f)
		{
			return;
		}
		float num2 = GameplayData.ActivationLuckGet();
		GameplayData.RndActivationFailsafe_RottenPepper++;
		if (R.Rng_Powerup(PowerupScript.Identifier.RottenPepper).Value > 0.1f * num2 && GameplayData.RndActivationFailsafe_RottenPepper <= 9)
		{
			return;
		}
		GameplayData.RndActivationFailsafe_RottenPepper = 0;
		GameplayData.ExtraLuck_SetEntry("tl_spRtnPpr", num, 1, true);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.RottenPepper);
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x00024270 File Offset: 0x00022470
	private static void RottenPepper_GrowthTrigger()
	{
		if (SlotMachineScript.GetPatternsCount() < 3)
		{
			return;
		}
		GameplayData.Powerup_RottenPepper_LuckBonusSet(GameplayData.Powerup_RottenPepper_LuckBonusGet() + 3);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.RottenPepper);
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x0002428E File Offset: 0x0002248E
	private static void RottenPepper_Reset()
	{
		GameplayData.Powerup_RottenPepper_LuckBonusSet(0);
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x00024296 File Offset: 0x00022496
	public static void PFunc_OnEquip_BellPepper(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Combine(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.TriggerBellPepper));
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x000242BE File Offset: 0x000224BE
	public static void PFunc_OnUnequip_BellPepper(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnSpinPreLuckApplication = (SlotMachineScript.Event)Delegate.Remove(instance.OnSpinPreLuckApplication, new SlotMachineScript.Event(PowerupScript.TriggerBellPepper));
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x000242E6 File Offset: 0x000224E6
	public static void PFunc_OnThrowAway_BellPepper(PowerupScript powerup)
	{
		GameplayData.Powerup_BellPepper_LuckBonusSet(0);
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x000242F0 File Offset: 0x000224F0
	public static void TriggerBellPepper()
	{
		float num = (float)GameplayData.Powerup_BellPepper_LuckBonusGet();
		if (num <= 0f)
		{
			return;
		}
		float num2 = GameplayData.ActivationLuckGet();
		GameplayData.RndActivationFailsafe_BellPepper++;
		if (R.Rng_Powerup(PowerupScript.Identifier.BellPepper).Value > 0.1f * num2 && GameplayData.RndActivationFailsafe_BellPepper <= 9)
		{
			return;
		}
		GameplayData.RndActivationFailsafe_BellPepper = 0;
		GameplayData.ExtraLuck_SetEntry("tl_spBllPpr", num, 1, true);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.BellPepper);
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x00024358 File Offset: 0x00022558
	public static void PFunc_OnEquip_DevilHorn(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_DevilHorn));
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x00024380 File Offset: 0x00022580
	public static void PFunc_OnUnequip_DevilHorn(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_DevilHorn));
	}

	// Token: 0x060005F9 RID: 1529 RVA: 0x000243A8 File Offset: 0x000225A8
	private static void Trigger_DevilHorn()
	{
		if (!SlotMachineScript.HasShown666())
		{
			return;
		}
		GameplayData.ExtraLuck_SetEntry("tl_dvlhrn", 10f, 1, true);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.HornDevil);
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x000243CA File Offset: 0x000225CA
	public static int Necronomicon_AdditionalPatternsMultiplierGet()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Necronomicon))
		{
			return 0;
		}
		return 2;
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x000243D8 File Offset: 0x000225D8
	public static float Necronomicon_666AdditionalChanceGet()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Necronomicon))
		{
			return 0f;
		}
		return 0.03f;
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x000243EE File Offset: 0x000225EE
	public static void PFunc_OnEquip_Necronomicon(PowerupScript powerup)
	{
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Necronomicon);
	}

	// Token: 0x060005FD RID: 1533 RVA: 0x000243F7 File Offset: 0x000225F7
	public static void PFunc_OnEquip_HolyBible(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationBegin = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationBegin, new SlotMachineScript.Event(PowerupScript.Trigger_HolyBible));
	}

	// Token: 0x060005FE RID: 1534 RVA: 0x0002441F File Offset: 0x0002261F
	public static void PFunc_OnUnequip_HolyBible(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationBegin = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationBegin, new SlotMachineScript.Event(PowerupScript.Trigger_HolyBible));
	}

	// Token: 0x060005FF RID: 1535 RVA: 0x00024448 File Offset: 0x00022648
	private static void Trigger_HolyBible()
	{
		if (!SlotMachineScript.Has666())
		{
			return;
		}
		PowerupTriggerAnimController instance = PowerupTriggerAnimController.instance;
		instance.OnAnimationEnd = (PowerupTriggerAnimController.PowerupEvent)Delegate.Combine(instance.OnAnimationEnd, new PowerupTriggerAnimController.PowerupEvent(PowerupScript._OnHolyBibleAnimationEnd));
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.HolyBible);
		Data.GameData game = Data.game;
		int unlockSteps_Rosary = game.UnlockSteps_Rosary;
		game.UnlockSteps_Rosary = unlockSteps_Rosary + 1;
		if (R.Rng_Powerup(PowerupScript.Identifier.HolyBible).Value > 0.5f)
		{
			PowerupScript.ThrowAway(PowerupScript.Identifier.HolyBible, false);
		}
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x000244BA File Offset: 0x000226BA
	private static void _OnHolyBibleAnimationEnd(PowerupTriggerAnimController.AnimationCapsule animationCapsule)
	{
		if (animationCapsule.powerup.identifier != PowerupScript.Identifier.HolyBible)
		{
			return;
		}
		if (animationCapsule.animationKind != PowerupTriggerAnimController.AnimationCapsule.AnimationKind.normalTrigger)
		{
			return;
		}
		SlotMachineScript.RemoveVisible666();
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x000244DA File Offset: 0x000226DA
	public static void PFunc_OnEquip_Rosary(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationBegin = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationBegin, new SlotMachineScript.Event(PowerupScript.Trigger_Rosary));
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x00024502 File Offset: 0x00022702
	public static void PFunc_OnUnequip_Rosary(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationBegin = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationBegin, new SlotMachineScript.Event(PowerupScript.Trigger_Rosary));
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0002452C File Offset: 0x0002272C
	private static void Trigger_Rosary()
	{
		if (!SlotMachineScript.Has666())
		{
			return;
		}
		float num = GameplayData.ActivationLuckGet();
		GameplayData.RndActivationFailsafe_Rosary++;
		if (R.Rng_Powerup(PowerupScript.Identifier.Rosary).Value > 0.35f * num && GameplayData.RndActivationFailsafe_Rosary < 3)
		{
			return;
		}
		GameplayData.RndActivationFailsafe_Rosary = 0;
		PowerupTriggerAnimController instance = PowerupTriggerAnimController.instance;
		instance.OnAnimationEnd = (PowerupTriggerAnimController.PowerupEvent)Delegate.Combine(instance.OnAnimationEnd, new PowerupTriggerAnimController.PowerupEvent(PowerupScript._OnRosaryAnimationEnd));
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Rosary);
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x000245A4 File Offset: 0x000227A4
	private static void _OnRosaryAnimationEnd(PowerupTriggerAnimController.AnimationCapsule animationCapsule)
	{
		if (animationCapsule.powerup.identifier != PowerupScript.Identifier.Rosary)
		{
			return;
		}
		if (animationCapsule.animationKind != PowerupTriggerAnimController.AnimationCapsule.AnimationKind.normalTrigger)
		{
			return;
		}
		SlotMachineScript.RemoveVisible666();
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x000245C4 File Offset: 0x000227C4
	public static void PFunc_OnEquip_Baphomet(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_Baphomet));
	}

	// Token: 0x06000606 RID: 1542 RVA: 0x000245EC File Offset: 0x000227EC
	public static void PFunc_OnUnequip_Baphomet(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_Baphomet));
	}

	// Token: 0x06000607 RID: 1543 RVA: 0x00024614 File Offset: 0x00022814
	private static void Trigger_Baphomet()
	{
		if (!SlotMachineScript.HasShown666())
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Baphomet);
		double num = GameplayData.Pattern_ValueOverall_Get(PatternScript.Kind.triangle, false);
		GameplayData.Pattern_ValueExtra_Add(PatternScript.Kind.triangle, num);
		double num2 = GameplayData.Pattern_ValueOverall_Get(PatternScript.Kind.triangleInverted, false);
		GameplayData.Pattern_ValueExtra_Add(PatternScript.Kind.triangleInverted, num2);
	}

	// Token: 0x06000608 RID: 1544 RVA: 0x00024652 File Offset: 0x00022852
	public static void PFunc_OnEquip_Cross(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.Cross_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.noTiming, null);
	}

	// Token: 0x06000609 RID: 1545 RVA: 0x00024668 File Offset: 0x00022868
	public static void PFunc_OnUnequip_Cross(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
	}

	// Token: 0x0600060A RID: 1546 RVA: 0x00024670 File Offset: 0x00022870
	private static void Cross_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.Cross, true))
		{
			return;
		}
		GameplayData.Powerup_Cross_TriggersCount_Set(GameplayData.Powerup_Cross_TriggersCount_Get() + 1L);
		if (PowerupScript.CrossIsUp())
		{
			powerup.triggerSpecificSound = AssetMaster.GetSound("SoundPowerup_Holy_Trigger");
		}
		else
		{
			powerup.triggerSpecificSound = AssetMaster.GetSound("SoundPowerup_DevilHorn_Trigger");
		}
		if (PowerupScript.CrossIsUp())
		{
			powerup.animator_IfAny.SetTrigger("TurnUp");
		}
		else
		{
			powerup.animator_IfAny.SetTrigger("TurnDown");
		}
		powerup.animator_IfAny.Update(0f);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Cross);
		SlotMachineScript.instance.TopTextSet_666Or999_ChancesShow();
	}

	// Token: 0x0600060B RID: 1547 RVA: 0x00024709 File Offset: 0x00022909
	public static bool CrossIsUp()
	{
		return GameplayData.Powerup_Cross_TriggersCount_Get() % 2L == 0L;
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x00024717 File Offset: 0x00022917
	public static long Cross_PatternsMultiplierBonus_Get(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Cross))
		{
			return 0L;
		}
		return GameplayData.Powerup_Cross_TriggersCount_Get() / 2L;
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x00024730 File Offset: 0x00022930
	public static float Cross_666Malus_Get(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Cross))
		{
			return 0f;
		}
		if (!PowerupScript.CrossIsUp())
		{
			return 0.03f;
		}
		return 0f;
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x00024758 File Offset: 0x00022958
	public static void PFunc_OnEquip_BookOfShadows(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_BookOfShadows));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.BookOfShadowsReset));
	}

	// Token: 0x0600060F RID: 1551 RVA: 0x000247B4 File Offset: 0x000229B4
	public static void PFunc_OnUnequip_BookOfShadows(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_BookOfShadows));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.BookOfShadowsReset));
		PowerupScript.BookOfShadowsReset();
	}

	// Token: 0x06000610 RID: 1552 RVA: 0x00024814 File Offset: 0x00022A14
	private static void Trigger_BookOfShadows()
	{
		if (!SlotMachineScript.HasShown666())
		{
			return;
		}
		if (PowerupScript._bookOfShadowsRoundActivationsCounter >= 3)
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.BookOfShadows);
		PowerupScript._bookOfShadowsRoundActivationsCounter++;
		switch (PowerupScript._bookOfShadowsRoundActivationsCounter)
		{
		case 1:
			GameplayData.SpinsLeftAdd(3);
			break;
		case 2:
			GameplayData.SpinsLeftAdd(2);
			break;
		case 3:
			GameplayData.SpinsLeftAdd(1);
			break;
		default:
			GameplayData.SpinsLeftAdd(0);
			break;
		}
		GeneralUiScript.CoinsTextForceUpdate();
	}

	// Token: 0x06000611 RID: 1553 RVA: 0x00024884 File Offset: 0x00022A84
	public static float BookOfShadows_SixSixSixChanceGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.BookOfShadows) && considerEquippedState)
		{
			return 0f;
		}
		return 0.015f;
	}

	// Token: 0x06000612 RID: 1554 RVA: 0x0002489F File Offset: 0x00022A9F
	private static void BookOfShadowsReset()
	{
		PowerupScript._bookOfShadowsRoundActivationsCounter = 0;
	}

	// Token: 0x06000613 RID: 1555 RVA: 0x000248A7 File Offset: 0x00022AA7
	public static long GabibbhMultiplierGet(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Gabibbh))
		{
			return 1L;
		}
		return PowerupScript._gabibbhMultiplier;
	}

	// Token: 0x06000614 RID: 1556 RVA: 0x000248C0 File Offset: 0x00022AC0
	public static void PFunc_OnEquip_Gabibbh(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_Gabibbh));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.GabibbhReset));
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x0002491C File Offset: 0x00022B1C
	public static void PFunc_OnUnequip_Gabibbh(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_Gabibbh));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.GabibbhReset));
		PowerupScript.GabibbhReset();
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x0002497A File Offset: 0x00022B7A
	private static void Trigger_Gabibbh()
	{
		if (!SlotMachineScript.HasShown666())
		{
			return;
		}
		PowerupScript._gabibbhMultiplier *= 2L;
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Gabibbh);
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x00024998 File Offset: 0x00022B98
	private static void GabibbhReset()
	{
		PowerupScript._gabibbhMultiplier = 1L;
	}

	// Token: 0x06000618 RID: 1560 RVA: 0x000249A1 File Offset: 0x00022BA1
	public static void PFunc_OnEquip_PossessedPhone(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.PossessedPhone_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.perSpin, null);
	}

	// Token: 0x06000619 RID: 1561 RVA: 0x000249B7 File Offset: 0x00022BB7
	public static void PFunc_OnUnequip_PossessedPhone(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
		GameplayData.Powerup_PossessedPhone_TriggersCount_Set(0);
	}

	// Token: 0x0600061A RID: 1562 RVA: 0x000249C5 File Offset: 0x00022BC5
	private static void PossessedPhone_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.PossessedPhone, true))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.PossessedPhone);
		GameplayData.Powerup_PossessedPhone_TriggersCount_Set(GameplayData.Powerup_PossessedPhone_TriggersCount_Get() + 1);
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x000249E8 File Offset: 0x00022BE8
	public static void PFunc_OnEquip_MysticalTomato(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_MysticalTomato));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.MysticalTomatoReset));
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x00024A44 File Offset: 0x00022C44
	public static void PFunc_OnUnequip_MysticalTomato(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_MysticalTomato));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.MysticalTomatoReset));
		PowerupScript.MysticalTomatoReset();
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x00024AA2 File Offset: 0x00022CA2
	private static void Trigger_MysticalTomato()
	{
		if (!SlotMachineScript.HasShown666())
		{
			return;
		}
		PowerupScript._mysticalTomatoRepetitions++;
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.MysticalTomato);
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x00024ABF File Offset: 0x00022CBF
	private static void MysticalTomatoReset()
	{
		PowerupScript._mysticalTomatoRepetitions = 0;
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x00024AC7 File Offset: 0x00022CC7
	public static int MysticalTomatoGetRepetitions()
	{
		return PowerupScript._mysticalTomatoRepetitions;
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x00024ACE File Offset: 0x00022CCE
	public static void PFunc_OnEquip_RitualBell(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_RitualBell));
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x00024AF6 File Offset: 0x00022CF6
	public static void PFunc_OnUnequip_RitualBell(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_RitualBell));
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x00024B1E File Offset: 0x00022D1E
	private static void Trigger_RitualBell()
	{
		if (!SlotMachineScript.HasShown666())
		{
			return;
		}
		GameplayData.StoreFreeRestocksSet(GameplayData.StoreFreeRestocksGet() + 3L);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.RitualBell);
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x00024B3C File Offset: 0x00022D3C
	public static BigInteger CrystalSkullMultiplierGet(SymbolScript.Kind symbolKind)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.CrystalSkull))
		{
			return 0;
		}
		switch (symbolKind)
		{
		case SymbolScript.Kind.diamond:
			return PowerupScript._crystalSkullBonus_Diamonds;
		case SymbolScript.Kind.coins:
			return PowerupScript._crystalSkullBonus_Coins;
		case SymbolScript.Kind.seven:
			return PowerupScript._crystalSkullBonus_Sevens;
		default:
			return 0;
		}
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x00024B7C File Offset: 0x00022D7C
	public static void PFunc_OnEquip_CrystalSkull(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_CrystalSkull));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.CrystalSkullReset));
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x00024BD8 File Offset: 0x00022DD8
	public static void PFunc_OnUnequip_CrystalSkull(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_CrystalSkull));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.CrystalSkullReset));
		PowerupScript.CrystalSkullReset();
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x00024C38 File Offset: 0x00022E38
	private static void Trigger_CrystalSkull()
	{
		if (!SlotMachineScript.HasShown666())
		{
			return;
		}
		PowerupScript._crystalSkullBonus_Diamonds += GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.diamond) * 2;
		PowerupScript._crystalSkullBonus_Coins += GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.coins) * 2;
		PowerupScript._crystalSkullBonus_Sevens += GameplayData.Symbol_CoinsOverallValue_Get(SymbolScript.Kind.seven) * 2;
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.CrystalSkull);
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x00024CB4 File Offset: 0x00022EB4
	private static void CrystalSkullReset()
	{
		PowerupScript._crystalSkullBonus_Diamonds = 0;
		PowerupScript._crystalSkullBonus_Coins = 0;
		PowerupScript._crystalSkullBonus_Sevens = 0;
	}

	// Token: 0x06000628 RID: 1576 RVA: 0x00024CD7 File Offset: 0x00022ED7
	public static float EvilDealBonusMultiplier_Float()
	{
		return (float)PowerupScript.EvilDealBonusMultiplier();
	}

	// Token: 0x06000629 RID: 1577 RVA: 0x00024CDF File Offset: 0x00022EDF
	public static int EvilDealBonusMultiplier()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.EvilDeal))
		{
			return 1;
		}
		return 2;
	}

	// Token: 0x0600062A RID: 1578 RVA: 0x00024CED File Offset: 0x00022EED
	public static int ChastityBelt_MultiplierBonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.ChastityBelt) && considerEquippedState)
		{
			return 0;
		}
		return GameplayData.PhoneAbilities_GetSkippedCount_Evil();
	}

	// Token: 0x0600062B RID: 1579 RVA: 0x00024D04 File Offset: 0x00022F04
	public static void PFunc_OnEquip_PhotoBook(PowerupScript powerup)
	{
		for (int i = 0; i < PowerupScript._photoBookPowerups.Length; i++)
		{
			PowerupScript._photoBookPowerups[i] = null;
		}
		int num = 0;
		for (int j = 0; j < PowerupScript.picturesOfSymbols.Count; j++)
		{
			PowerupScript powerupScript = PowerupScript.picturesOfSymbols[j];
			if (!(powerupScript == null) && PowerupScript.IsUnlocked(powerupScript.identifier) && !PowerupScript.IsEquipped_Quick(powerupScript.identifier) && !PowerupScript.IsInDrawer_Quick(powerupScript.identifier) && Array.IndexOf<PowerupScript>(PowerupScript._photoBookPowerups, powerupScript) < 0 && !PowerupScript.IsBanned(powerupScript.identifier, powerupScript.archetype))
			{
				PowerupScript._photoBookPowerups[num] = powerupScript;
				num++;
				if (num >= PowerupScript._photoBookPowerups.Length)
				{
					break;
				}
			}
		}
		StoreCapsuleScript.Restock(false, true, PowerupScript._photoBookPowerups, false, true);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.PhotoBook);
	}

	// Token: 0x0600062C RID: 1580 RVA: 0x00024DD0 File Offset: 0x00022FD0
	public static float LemonPicture_ChanceIncreaseGet(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.SymbolInstant_Lemon))
		{
			return 0f;
		}
		int num = GameplayData.Powerup_ButtonBurnedOut_Get(PowerupScript.Identifier.SymbolInstant_Lemon);
		if (num <= 0)
		{
			return 0f;
		}
		return 1.6f * (float)num;
	}

	// Token: 0x0600062D RID: 1581 RVA: 0x00024E09 File Offset: 0x00023009
	public static void PFunc_OnEquip_LemonPicture(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.LemonPicture_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.perDeadline, null);
	}

	// Token: 0x0600062E RID: 1582 RVA: 0x00024E1F File Offset: 0x0002301F
	public static void PFunc_OnUnequip_LemonPicture(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
	}

	// Token: 0x0600062F RID: 1583 RVA: 0x00024E27 File Offset: 0x00023027
	private static void LemonPicture_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.SymbolInstant_Lemon, true))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.SymbolInstant_Lemon);
	}

	// Token: 0x06000630 RID: 1584 RVA: 0x00024E3C File Offset: 0x0002303C
	public static float CherryPicture_ChanceIncreaseGet(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.SymbolInstant_Cherry))
		{
			return 0f;
		}
		int num = GameplayData.Powerup_ButtonBurnedOut_Get(PowerupScript.Identifier.SymbolInstant_Cherry);
		if (num <= 0)
		{
			return 0f;
		}
		return 1.6f * (float)num;
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x00024E75 File Offset: 0x00023075
	public static void PFunc_OnEquip_CherryPicture(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.CherryPicture_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.perDeadline, null);
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x00024E8B File Offset: 0x0002308B
	public static void PFunc_OnUnequip_CherryPicture(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x00024E93 File Offset: 0x00023093
	private static void CherryPicture_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.SymbolInstant_Cherry, true))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.SymbolInstant_Cherry);
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x00024EA8 File Offset: 0x000230A8
	public static float CloverPicture_ChanceIncreaseGet(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.SymbolInstant_Clover))
		{
			return 0f;
		}
		int num = GameplayData.Powerup_ButtonBurnedOut_Get(PowerupScript.Identifier.SymbolInstant_Clover);
		if (num <= 0)
		{
			return 0f;
		}
		return 1.6f * (float)num;
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x00024EE1 File Offset: 0x000230E1
	public static void PFunc_OnEquip_CloverPicture(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.CloverPicture_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.perDeadline, null);
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x00024EF7 File Offset: 0x000230F7
	public static void PFunc_OnUnequip_CloverPicture(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x00024EFF File Offset: 0x000230FF
	private static void CloverPicture_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.SymbolInstant_Clover, true))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.SymbolInstant_Clover);
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x00024F14 File Offset: 0x00023114
	public static float BellPicture_ChanceIncreaseGet(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.SymbolInstant_Bell))
		{
			return 0f;
		}
		int num = GameplayData.Powerup_ButtonBurnedOut_Get(PowerupScript.Identifier.SymbolInstant_Bell);
		if (num <= 0)
		{
			return 0f;
		}
		return 1.6f * (float)num;
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x00024F4D File Offset: 0x0002314D
	public static void PFunc_OnEquip_BellPicture(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.BellPicture_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.perDeadline, null);
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x00024F63 File Offset: 0x00023163
	public static void PFunc_OnUnequip_BellPicture(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x00024F6B File Offset: 0x0002316B
	private static void BellPicture_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.SymbolInstant_Bell, true))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.SymbolInstant_Bell);
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x00024F80 File Offset: 0x00023180
	public static float DiamondPicture_ChanceIncreaseGet(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.SymbolInstant_Diamond))
		{
			return 0f;
		}
		int num = GameplayData.Powerup_ButtonBurnedOut_Get(PowerupScript.Identifier.SymbolInstant_Diamond);
		if (num <= 0)
		{
			return 0f;
		}
		return 1.6f * (float)num;
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x00024FB9 File Offset: 0x000231B9
	public static void PFunc_OnEquip_DiamondPicture(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.DiamondPicture_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.perDeadline, null);
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x00024FCF File Offset: 0x000231CF
	public static void PFunc_OnUnequip_DiamondPicture(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x00024FD7 File Offset: 0x000231D7
	private static void DiamondPicture_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.SymbolInstant_Diamond, true))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.SymbolInstant_Diamond);
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x00024FEC File Offset: 0x000231EC
	public static float CoinsPicture_ChanceIncreaseGet(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.SymbolInstant_Treasure))
		{
			return 0f;
		}
		int num = GameplayData.Powerup_ButtonBurnedOut_Get(PowerupScript.Identifier.SymbolInstant_Treasure);
		if (num <= 0)
		{
			return 0f;
		}
		return 1.6f * (float)num;
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x00025025 File Offset: 0x00023225
	public static void PFunc_OnEquip_CoinsPicture(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.CoinsPicture_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.perDeadline, null);
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0002503B File Offset: 0x0002323B
	public static void PFunc_OnUnequip_CoinsPicture(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
	}

	// Token: 0x06000643 RID: 1603 RVA: 0x00025043 File Offset: 0x00023243
	private static void CoinsPicture_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.SymbolInstant_Treasure, true))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.SymbolInstant_Treasure);
	}

	// Token: 0x06000644 RID: 1604 RVA: 0x00025058 File Offset: 0x00023258
	public static float SevenPicture_ChanceIncreaseGet(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.SymbolInstant_Seven))
		{
			return 0f;
		}
		int num = GameplayData.Powerup_ButtonBurnedOut_Get(PowerupScript.Identifier.SymbolInstant_Seven);
		if (num <= 0)
		{
			return 0f;
		}
		return 1.6f * (float)num;
	}

	// Token: 0x06000645 RID: 1605 RVA: 0x00025091 File Offset: 0x00023291
	public static void PFunc_OnEquip_SevenPicture(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.SevenPicture_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.perDeadline, null);
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x000250A7 File Offset: 0x000232A7
	public static void PFunc_OnUnequip_SevenPicture(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x000250AF File Offset: 0x000232AF
	private static void SevenPicture_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier.SymbolInstant_Seven, true))
		{
			return;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.SymbolInstant_Seven);
	}

	// Token: 0x06000648 RID: 1608 RVA: 0x000250C4 File Offset: 0x000232C4
	public static float GeneralModCharm_Clicker_GetModChance(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GeneralModCharm_Clicker) && considerEquippedState)
		{
			return 0f;
		}
		return 0.15f;
	}

	// Token: 0x06000649 RID: 1609 RVA: 0x000250DF File Offset: 0x000232DF
	public static float GeneralModCharm_CloverBellBattery_GetModChance(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GeneralModCharm_CloverBellBattery) && considerEquippedState)
		{
			return 0f;
		}
		return 0.06f;
	}

	// Token: 0x0600064A RID: 1610 RVA: 0x000250FA File Offset: 0x000232FA
	public static float GeneralModCharm_CrystalBall_GetModChance(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GeneralModCharm_CrystalSphere) && considerEquippedState)
		{
			return 0f;
		}
		return 0.12f;
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x00025115 File Offset: 0x00023315
	public static float GoldenKingMida_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GoldenKingMida))
		{
			return 0f;
		}
		return 0.03f + GameplayData.Powerup_GoldenKingMida_ExtraBonusGet();
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x00025134 File Offset: 0x00023334
	public static void GoldenKingMida_GrowValue(int skippedRounds)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GoldenKingMida))
		{
			return;
		}
		if (skippedRounds <= 0)
		{
			return;
		}
		float num = GameplayData.Powerup_GoldenKingMida_ExtraBonusGet();
		bool flag = num < 0.22f;
		GameplayData.Powerup_GoldenKingMida_ExtraBonusSet(Mathf.Min(num + 0.02f * (float)skippedRounds, 0.22f));
		if (flag)
		{
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.GoldenKingMida);
		}
	}

	// Token: 0x0600064D RID: 1613 RVA: 0x00025180 File Offset: 0x00023380
	public static float Dealer_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_C_Dealer))
		{
			return 0f;
		}
		return 0.05f + GameplayData.Powerup_Dealer_ExtraBonusGet();
	}

	// Token: 0x0600064E RID: 1614 RVA: 0x000251A0 File Offset: 0x000233A0
	public static void Dealer_GrowValue()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_C_Dealer))
		{
			return;
		}
		if (SlotMachineScript.GetPatternsCount() < 5)
		{
			return;
		}
		float num = GameplayData.Powerup_Dealer_ExtraBonusGet();
		bool flag = num < 0.2f;
		GameplayData.Powerup_Dealer_ExtraBonusSet(Mathf.Min(num + 0.01f, 0.2f));
		if (flag)
		{
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Boardgame_C_Dealer);
		}
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x000251ED File Offset: 0x000233ED
	public static void PFunc_OnEquip_Dealer(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Dealer_GrowValue));
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x00025215 File Offset: 0x00023415
	public static void PFunc_OnUnequip_Dealer(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Dealer_GrowValue));
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x0002523D File Offset: 0x0002343D
	public static float Capitalist_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_M_Capitalist))
		{
			return 0f;
		}
		return 0.03f + GameplayData.Powerup_Capitalist_ExtraBonusGet();
	}

	// Token: 0x06000652 RID: 1618 RVA: 0x0002525C File Offset: 0x0002345C
	public static void Capitalist_GrowValue(PowerupScript powerup)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_M_Capitalist))
		{
			return;
		}
		if (powerup.identifier == PowerupScript.Identifier.Boardgame_M_Capitalist)
		{
			return;
		}
		float num = GameplayData.Powerup_Capitalist_ExtraBonusGet();
		bool flag = num < 0.22f;
		GameplayData.Powerup_Capitalist_ExtraBonusSet(Mathf.Min(num + 0.01f, 0.22f));
		if (flag)
		{
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Boardgame_M_Capitalist);
		}
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x000252AB File Offset: 0x000234AB
	public static float PuppetPersonalTrainer_BonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.PuppetPersonalTrainer) && considerEquippedState)
		{
			return 0f;
		}
		return GameplayData.Powerup_PersonalTrainer_BonusGet();
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x000252C6 File Offset: 0x000234C6
	public static void PuppetPersonalTrainer_ShrinkBonus()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.PuppetPersonalTrainer))
		{
			return;
		}
		float num = GameplayData.Powerup_PersonalTrainer_BonusGet() - 0.05f;
		GameplayData.Powerup_PersonalTrainer_BonusSet(num);
		PowerupScript.PlayPowerDownAnimation(PowerupScript.Identifier.PuppetPersonalTrainer);
		if (Mathf.RoundToInt(num * 100f) <= 0)
		{
			PowerupScript.ThrowAway(PowerupScript.Identifier.PuppetPersonalTrainer, false);
		}
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x00025301 File Offset: 0x00023501
	public static void PFunc_OnThrowAway_PuppetPersonalTrainer(PowerupScript powerup)
	{
		GameplayData.Powerup_PersonalTrainer_BonusReset();
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x00025308 File Offset: 0x00023508
	public static float PuppetEletrician_BonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.PuppetElectrician) && considerEquippedState)
		{
			return 0f;
		}
		return GameplayData.Powerup_Electrician_BonusGet();
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x00025323 File Offset: 0x00023523
	public static void PuppetEletrician_ShrinkBonus()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.PuppetElectrician))
		{
			return;
		}
		float num = GameplayData.Powerup_Electrician_BonusGet() - 0.01f;
		GameplayData.Powerup_Electrician_BonusSet(num);
		PowerupScript.PlayPowerDownAnimation(PowerupScript.Identifier.PuppetElectrician);
		if (Mathf.RoundToInt(num * 100f) <= 0)
		{
			PowerupScript.ThrowAway(PowerupScript.Identifier.PuppetElectrician, false);
		}
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x0002535E File Offset: 0x0002355E
	public static void PFunc_OnThrowAway_PuppetElectrician(PowerupScript powerup)
	{
		GameplayData.Powerup_Electrician_BonusReset();
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x00025365 File Offset: 0x00023565
	public static float PuppetFortuneTeller_BonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.PuppetFortuneTeller) && considerEquippedState)
		{
			return 0f;
		}
		return GameplayData.Powerup_FortuneTeller_BonusGet();
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x00025380 File Offset: 0x00023580
	public static void PuppetFortuneTeller_ShrinkBonus()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.PuppetFortuneTeller))
		{
			return;
		}
		float num = GameplayData.Powerup_FortuneTeller_BonusGet() - 0.05f;
		GameplayData.Powerup_FortuneTeller_BonusSet(num);
		PowerupScript.PlayPowerDownAnimation(PowerupScript.Identifier.PuppetFortuneTeller);
		if (Mathf.RoundToInt(num * 100f) <= 0)
		{
			PowerupScript.ThrowAway(PowerupScript.Identifier.PuppetFortuneTeller, false);
		}
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x000253BB File Offset: 0x000235BB
	public static void PFunc_OnThrowAway_PuppetFortuneTeller(PowerupScript powerup)
	{
		GameplayData.Powerup_FortuneTeller_BonusReset();
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x000253C2 File Offset: 0x000235C2
	public static float GoldenSymbol_Lemon_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GoldenSymbol_Lemon))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x000253DB File Offset: 0x000235DB
	public static float GoldenSymbol_Cherry_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GoldenSymbol_Cherry))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x0600065E RID: 1630 RVA: 0x000253F4 File Offset: 0x000235F4
	public static float GoldenSymbol_Clover_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GoldenSymbol_Clover))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x0600065F RID: 1631 RVA: 0x0002540D File Offset: 0x0002360D
	public static float GoldenSymbol_Bell_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GoldenSymbol_Bell))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x06000660 RID: 1632 RVA: 0x00025429 File Offset: 0x00023629
	public static float GoldenSymbol_Diamond_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GoldenSymbol_Diamond))
		{
			return 0f;
		}
		return 0.25f;
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x00025445 File Offset: 0x00023645
	public static float GoldenSymbol_Coins_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GoldenSymbol_Treasure))
		{
			return 0f;
		}
		return 0.25f;
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x00025461 File Offset: 0x00023661
	public static float GoldenSymbol_Seven_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.GoldenSymbol_Seven))
		{
			return 0f;
		}
		return 0.3f;
	}

	// Token: 0x06000663 RID: 1635 RVA: 0x0002547D File Offset: 0x0002367D
	public static float BoardgameC_Bricks_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_C_Bricks))
		{
			return 0f;
		}
		return 0.35f;
	}

	// Token: 0x06000664 RID: 1636 RVA: 0x00025499 File Offset: 0x00023699
	public static float BoardgameC_Wood_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_C_Wood))
		{
			return 0f;
		}
		return 0.35f;
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x000254B5 File Offset: 0x000236B5
	public static float BoardgameC_Sheep_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_C_Sheep))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x000254D1 File Offset: 0x000236D1
	public static float BoardgameC_Wheat_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_C_Wheat))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x000254ED File Offset: 0x000236ED
	public static float BoardgameC_Stone_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_C_Stone))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x00025509 File Offset: 0x00023709
	public static float BoardgameC_Harbor_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_C_Harbor))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x00025525 File Offset: 0x00023725
	public static float BoardgameC_Thief_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_C_Thief))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x00025541 File Offset: 0x00023741
	public static float BoardgameM_Carriola_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_M_Carriola))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x0002555D File Offset: 0x0002375D
	public static float BoardgameM_Shoe_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_M_Shoe))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x00025579 File Offset: 0x00023779
	public static float BoardgameM_Ditale_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_M_Ditale))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x0600066D RID: 1645 RVA: 0x00025595 File Offset: 0x00023795
	public static float BoardgameM_FerroDaStiro_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_M_FerroDaStiro))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x000255B1 File Offset: 0x000237B1
	public static float BoardgameM_Car_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_M_Car))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x000255CD File Offset: 0x000237CD
	public static float BoardgameM_Ship_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_M_Ship))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x000255E9 File Offset: 0x000237E9
	public static float BoardgameM_Hat_GetModChance(bool considerEquippedState)
	{
		if (considerEquippedState && !PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Boardgame_M_Hat))
		{
			return 0f;
		}
		return 0.2f;
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x00025605 File Offset: 0x00023805
	public static void PFunc_OnEquip_ClassicPlayingCard_AceOfHearts(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_AceOfHearts));
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x0002562D File Offset: 0x0002382D
	public static void PFunc_OnUnequip_ClassicPlayingCard_AceOfHearts(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_AceOfHearts));
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x00025655 File Offset: 0x00023855
	public static void Trigger_AceOfHearts()
	{
		if (SlotMachineScript.GetPatternsCount() < 3)
		{
			return;
		}
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.lemon, GameplayData.Symbol_CoinsValue_GetBasic(SymbolScript.Kind.lemon));
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.cherry, GameplayData.Symbol_CoinsValue_GetBasic(SymbolScript.Kind.cherry));
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.PlayingCard_HeartsAce);
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0002568C File Offset: 0x0002388C
	public static void ClassicPlayingCards_AceOfClubs_ProcessSpendedTickets(bool considerEquippedState, long ticketsToAdd)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.PlayingCard_ClubsAce) && considerEquippedState)
		{
			return;
		}
		long num = GameplayData.Powerup_AceOfClubs_TicketsSpentGet();
		num += ticketsToAdd;
		long num2 = num / 3L;
		if (num >= 3L)
		{
			GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.clover, (long)GameplayData.Symbol_CoinsValue_GetBasic(SymbolScript.Kind.clover) * num2);
			GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.bell, (long)GameplayData.Symbol_CoinsValue_GetBasic(SymbolScript.Kind.bell) * num2);
			int num3 = 0;
			while ((long)num3 < num2)
			{
				PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.PlayingCard_ClubsAce);
				num3++;
			}
		}
		GameplayData.Powerup_AceOfClubs_TicketsSpentSet(num % 3L);
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x00025707 File Offset: 0x00023907
	public static void PFunc_OnEquip_ClassicPlayingCard_AceOfDiamonds(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_AceOfDiamonds));
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x0002572F File Offset: 0x0002392F
	public static void PFunc_OnUnequip_ClassicPlayingCard_AceOfDiamonds(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.Trigger_AceOfDiamonds));
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x00025758 File Offset: 0x00023958
	public static void Trigger_AceOfDiamonds()
	{
		PatternScript.Kind biggestPatternScored = SlotMachineScript.GetBiggestPatternScored();
		if (biggestPatternScored == PatternScript.Kind.undefined)
		{
			return;
		}
		if (PatternScript.GetElementsCount(biggestPatternScored) < 4)
		{
			return;
		}
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.diamond, GameplayData.Symbol_CoinsValue_GetBasic(SymbolScript.Kind.diamond));
		GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.coins, GameplayData.Symbol_CoinsValue_GetBasic(SymbolScript.Kind.coins));
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.PlayingCard_DiamondsAce);
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x000257A8 File Offset: 0x000239A8
	public static void ClassicPlayingCards_AceOfSpades_ProcessActivation(bool considerEquippedState, PowerupScript.Identifier powerupTriggered)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.PlayingCard_SpadesAce) && considerEquippedState)
		{
			return;
		}
		if (GameplayMaster.GetGamePhase() != GameplayMaster.GamePhase.gambling)
		{
			return;
		}
		if (powerupTriggered == PowerupScript.Identifier.PlayingCard_SpadesAce)
		{
			return;
		}
		long num = GameplayData.Powerup_AceOfSpades_ActivationsCounterGet();
		for (num += 1L; num >= 7L; num -= 7L)
		{
			GameplayData.Symbol_CoinsValueExtra_Add(SymbolScript.Kind.seven, GameplayData.Symbol_CoinsValue_GetBasic(SymbolScript.Kind.seven));
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.PlayingCard_SpadesAce);
		}
		GameplayData.Powerup_AceOfSpades_ActivationsCounterSet(num);
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x00025811 File Offset: 0x00023A11
	private static void DiceD4_Reset()
	{
		PowerupScript._dice4UntriggeredPositions.Clear();
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x00025820 File Offset: 0x00023A20
	public static bool Dice4_TriggerTry()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Dice_4))
		{
			return false;
		}
		float num = GameplayData.ActivationLuckGet();
		GameplayData.RndActivationFailsafe_Dice4++;
		if (R.Rng_Powerup(PowerupScript.Identifier.Dice_4).Value > 0.3f * num && GameplayData.RndActivationFailsafe_Dice4 < 4)
		{
			return false;
		}
		GameplayData.RndActivationFailsafe_Dice4 = 0;
		if (SlotMachineScript.HasJackpot())
		{
			return false;
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Dice_4);
		return true;
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x00025889 File Offset: 0x00023A89
	private static void PFunc_Dice6_OnEquipped(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnInterestEarn = (SlotMachineScript.Event)Delegate.Combine(instance.OnInterestEarn, new SlotMachineScript.Event(PowerupScript.Dice6_TriggerTry));
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x000258B1 File Offset: 0x00023AB1
	private static void PFunc_Dice6_OnUnequipped(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnInterestEarn = (SlotMachineScript.Event)Delegate.Remove(instance.OnInterestEarn, new SlotMachineScript.Event(PowerupScript.Dice6_TriggerTry));
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x000258DC File Offset: 0x00023ADC
	public static void Dice6_TriggerTry()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Dice_6))
		{
			return;
		}
		PowerupTriggerAnimController instance = PowerupTriggerAnimController.instance;
		instance.OnAnimationStart = (PowerupTriggerAnimController.PowerupEvent)Delegate.Combine(instance.OnAnimationStart, new PowerupTriggerAnimController.PowerupEvent(PowerupScript._Dice6_CameraSet));
		PowerupTriggerAnimController instance2 = PowerupTriggerAnimController.instance;
		instance2.OnAnimationEnd = (PowerupTriggerAnimController.PowerupEvent)Delegate.Combine(instance2.OnAnimationEnd, new PowerupTriggerAnimController.PowerupEvent(PowerupScript._Dice6_CameraReset));
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Dice_6);
		PowerupTriggerAnimController.AnimationSetSpeed(1f);
		Sound.Play("SoundStoreBuy", 1f, 1f);
		StoreCapsuleScript.Restock(false, true, null, false, false);
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x00025975 File Offset: 0x00023B75
	public static void _Dice6_CameraSet(PowerupTriggerAnimController.AnimationCapsule animationCapsule)
	{
		if (animationCapsule.powerup.identifier != PowerupScript.Identifier.Dice_6)
		{
			return;
		}
		if (animationCapsule.animationKind != PowerupTriggerAnimController.AnimationCapsule.AnimationKind.normalTrigger)
		{
			return;
		}
		PowerupScript._diceD6CameraPosition = CameraController.GetPositionKind();
		CameraController.SetPosition(CameraController.PositionKind.Store, false, 1f);
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x000259AC File Offset: 0x00023BAC
	public static void _Dice6_CameraReset(PowerupTriggerAnimController.AnimationCapsule animationCapsule)
	{
		if (animationCapsule.powerup.identifier != PowerupScript.Identifier.Dice_6)
		{
			return;
		}
		if (animationCapsule.animationKind != PowerupTriggerAnimController.AnimationCapsule.AnimationKind.normalTrigger)
		{
			return;
		}
		if (PowerupScript._diceD6CameraPosition == CameraController.PositionKind.Undefined)
		{
			CameraController.SetPosition(CameraController.PositionKind.Free, false, 1f);
			return;
		}
		CameraController.SetPosition(PowerupScript._diceD6CameraPosition, false, 1f);
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x000259FB File Offset: 0x00023BFB
	private static void PFunc_Dice20_OnEquipped(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnInterestEarn = (SlotMachineScript.Event)Delegate.Combine(instance.OnInterestEarn, new SlotMachineScript.Event(PowerupScript.D20_Trigger));
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x00025A23 File Offset: 0x00023C23
	private static void PFunc_Dice20_OnUnequipped(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnInterestEarn = (SlotMachineScript.Event)Delegate.Remove(instance.OnInterestEarn, new SlotMachineScript.Event(PowerupScript.D20_Trigger));
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x00025A4C File Offset: 0x00023C4C
	public static void D20_Trigger()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Dice_20))
		{
			return;
		}
		if (DrawersScript.RerollCharmsIntoDrawers() <= 0)
		{
			return;
		}
		PowerupTriggerAnimController instance = PowerupTriggerAnimController.instance;
		instance.OnAnimationStart = (PowerupTriggerAnimController.PowerupEvent)Delegate.Combine(instance.OnAnimationStart, new PowerupTriggerAnimController.PowerupEvent(PowerupScript._Dice20_CameraSet));
		PowerupTriggerAnimController instance2 = PowerupTriggerAnimController.instance;
		instance2.OnAnimationEnd = (PowerupTriggerAnimController.PowerupEvent)Delegate.Combine(instance2.OnAnimationEnd, new PowerupTriggerAnimController.PowerupEvent(PowerupScript._Dice20_CameraReset));
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Dice_20);
		PowerupTriggerAnimController.AnimationSetSpeed(1f);
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x00025ACF File Offset: 0x00023CCF
	public static void _Dice20_CameraSet(PowerupTriggerAnimController.AnimationCapsule animationCapsule)
	{
		if (animationCapsule.powerup.identifier != PowerupScript.Identifier.Dice_20)
		{
			return;
		}
		if (animationCapsule.animationKind != PowerupTriggerAnimController.AnimationCapsule.AnimationKind.normalTrigger)
		{
			return;
		}
		PowerupScript._diceD20CameraPosition = CameraController.GetPositionKind();
		CameraController.SetPosition(CameraController.PositionKind.DrawersAll, false, 1f);
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x00025B04 File Offset: 0x00023D04
	public static void _Dice20_CameraReset(PowerupTriggerAnimController.AnimationCapsule animationCapsule)
	{
		if (animationCapsule.powerup.identifier != PowerupScript.Identifier.Dice_20)
		{
			return;
		}
		if (animationCapsule.animationKind != PowerupTriggerAnimController.AnimationCapsule.AnimationKind.normalTrigger)
		{
			return;
		}
		if (PowerupScript._diceD20CameraPosition == CameraController.PositionKind.Undefined)
		{
			CameraController.SetPosition(CameraController.PositionKind.Free, false, 1f);
			return;
		}
		CameraController.SetPosition(PowerupScript._diceD20CameraPosition, false, 1f);
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x00025B54 File Offset: 0x00023D54
	public static void PFunc_OnThrowAway_HoleCircle(PowerupScript powerup)
	{
		if (PowerupScript.IsInDrawer_Quick(powerup.identifier))
		{
			return;
		}
		PowerupScript.Identifier identifier = GameplayData.PowerupHoleCircle_CharmGet();
		GameplayData.PowerupHoleCircle_CharmSet(PowerupScript.Identifier.undefined);
		if (identifier == PowerupScript.Identifier.undefined || identifier == PowerupScript.Identifier.count)
		{
			return;
		}
		PowerupScript.EquipFlag_IgnoreSpaceCondition();
		PowerupScript.Equip(identifier, false, true);
		RedButtonScript.ButtonVisualsRefresh();
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x00025B9B File Offset: 0x00023D9B
	public static void HoleCircle_RecordCharmTry(PowerupScript.Identifier identifier)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Hole_Circle))
		{
			return;
		}
		GameplayData.PowerupHoleCircle_CharmSet(identifier);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Hole_Circle);
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x00025BB4 File Offset: 0x00023DB4
	public static void PFunc_OnThrowAway_HoleRomboid(PowerupScript powerup)
	{
		if (PowerupScript.IsInDrawer_Quick(powerup.identifier))
		{
			return;
		}
		PowerupScript.Identifier identifier = GameplayData.PowerupHoleRomboid_CharmGet();
		GameplayData.PowerupHoleRomboid_CharmSet(PowerupScript.Identifier.undefined);
		if (identifier == PowerupScript.Identifier.undefined || identifier == PowerupScript.Identifier.count)
		{
			return;
		}
		PowerupScript.EquipFlag_IgnoreSpaceCondition();
		PowerupScript.Equip(identifier, false, true);
		RedButtonScript.ButtonVisualsRefresh();
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x00025BFB File Offset: 0x00023DFB
	public static void HoleRomboid_RecordCharmTry(PowerupScript.Identifier identifier)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Hole_Romboid))
		{
			return;
		}
		GameplayData.PowerupHoleRomboid_CharmSet(identifier);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Hole_Romboid);
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x00025C14 File Offset: 0x00023E14
	public static void PFunc_OnThrowAway_HoleCross(PowerupScript powerup)
	{
		if (PowerupScript.IsInDrawer_Quick(powerup.identifier))
		{
			return;
		}
		AbilityScript.Identifier identifier = GameplayData.PowerupHoleCross_AbilityGet();
		GameplayData.PowerupHoleCross_AbilitySet(AbilityScript.Identifier.undefined);
		if (identifier == AbilityScript.Identifier.undefined || identifier == AbilityScript.Identifier.count)
		{
			return;
		}
		PhoneUiScript.instance.AbilityPick(AbilityScript.AbilityGet(identifier));
		RedButtonScript.ButtonVisualsRefresh();
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x00025C5A File Offset: 0x00023E5A
	public static void HoleCross_RecordAbilityTry(AbilityScript.Identifier identifier)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Hole_Cross))
		{
			return;
		}
		GameplayData.PowerupHoleCross_AbilitySet(identifier);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Hole_Cross);
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x00025C73 File Offset: 0x00023E73
	public static void PFunc_OnEquip_AngelHand(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Add(powerup, new RedButtonScript.ButtonCallback(PowerupScript.AngelHand_RedButtonCallback_OnPress), RedButtonScript.RegistrationCapsule.Timing.perRound, null);
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.AngelHand_RemovePatterns));
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x00025CAF File Offset: 0x00023EAF
	public static void PFunc_OnUnequip_AngelHand(PowerupScript powerup)
	{
		RedButtonScript.PowerupRegistration_Remove(powerup);
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.AngelHand_RemovePatterns));
		PowerupScript.AngelHand_RemovePatterns();
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x00025CE4 File Offset: 0x00023EE4
	private static void AngelHand_RedButtonCallback_OnPress(PowerupScript powerup)
	{
		if (PowerupScript.RedButtonCallback_IdentityCheck(powerup, PowerupScript.Identifier._999_AngelHand, true))
		{
			return;
		}
		bool flag = false;
		if (PowerupScript.angelPatterns.Count > 0)
		{
			int num = R.Rng_Powerup(PowerupScript.Identifier._999_AngelHand).Range(0, PowerupScript.angelPatterns.Count);
			for (int i = 0; i < PowerupScript.angelPatterns.Count; i++)
			{
				PatternScript.Kind kind = PowerupScript.angelPatterns[num];
				if (kind != PatternScript.Kind.undefined && kind != PatternScript.Kind.count && !GameplayData.PatternsAvailable_GetAll().Contains(kind))
				{
					flag = true;
					GameplayData.PatternsAvailable_Add(kind);
					break;
				}
				num++;
				if (num >= PowerupScript.angelPatterns.Count)
				{
					num = 0;
				}
			}
		}
		if (flag)
		{
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier._999_AngelHand);
		}
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x00025D8B File Offset: 0x00023F8B
	private static void AngelHand_RemovePatterns()
	{
		GameplayData.PatternsAvailable_Remove(PatternScript.Kind.horizontal2);
		GameplayData.PatternsAvailable_Remove(PatternScript.Kind.vertical2);
		GameplayData.PatternsAvailable_Remove(PatternScript.Kind.diagonal2);
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x00025D9F File Offset: 0x00023F9F
	public static void PFunc_OnEquip_EyeOfGod(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.EyeOfGodTrigger));
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x00025DC7 File Offset: 0x00023FC7
	public static void PFunc_OnUnequip_EyeOfGod(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.EyeOfGodTrigger));
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x00025DEF File Offset: 0x00023FEF
	private static void EyeOfGodTrigger()
	{
		if (SlotMachineScript.instance.GetPatternsOfKind(PatternScript.Kind.eye).Count <= 0)
		{
			return;
		}
		SlotMachineScript.SpinExtraCoinsAdd(GameplayData.NineNineNne_TotalRewardEarned_Get());
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier._999_EyeOfGod);
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x00025E1A File Offset: 0x0002401A
	public static void PFunc_OnEquip_HolySpirit(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.HolySpiritTrigger));
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x00025E42 File Offset: 0x00024042
	public static void PFunc_OnUnequip_HolySpirit(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.HolySpiritTrigger));
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x00025E6C File Offset: 0x0002406C
	private static void HolySpiritTrigger()
	{
		if (!SlotMachineScript.Has999())
		{
			return;
		}
		for (int i = 0; i < PowerupScript.list_EquippedNormal.Count; i++)
		{
			PowerupScript powerupScript = PowerupScript.list_EquippedNormal[i];
			if (!(powerupScript == null))
			{
				powerupScript.ModifierReEvaluate(false, true);
			}
		}
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier._999_HolySpirit);
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x00025EC0 File Offset: 0x000240C0
	public static bool SacredHeart_EvaluateNoChargeConsumption()
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier._999_SacredHeart))
		{
			return false;
		}
		GameplayData.RndActivationFailsafe_SacredHeart++;
		if (R.Rng_Powerup(PowerupScript.Identifier._999_SacredHeart).FlipCoin && GameplayData.RndActivationFailsafe_SacredHeart < 3)
		{
			return false;
		}
		GameplayData.RndActivationFailsafe_SacredHeart = 0;
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier._999_SacredHeart);
		return true;
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x00025F14 File Offset: 0x00024114
	public static void PFunc_OnEquip_Eternity(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.EternityTrigger));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.EternityReset));
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x00025F70 File Offset: 0x00024170
	public static void PFunc_OnUnequip_Eternity(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.EternityTrigger));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.EternityReset));
		PowerupScript.EternityReset();
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x00025FCE File Offset: 0x000241CE
	private static void EternityTrigger()
	{
		if (!SlotMachineScript.Has999())
		{
			return;
		}
		PowerupScript.eternityRepetitionsCounter++;
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier._999_Eternity);
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x00025FEE File Offset: 0x000241EE
	private static void EternityReset()
	{
		PowerupScript.eternityRepetitionsCounter = 0;
	}

	// Token: 0x0600069A RID: 1690 RVA: 0x00025FF6 File Offset: 0x000241F6
	public static int EternityRepetitionsCounterGet()
	{
		return PowerupScript.eternityRepetitionsCounter;
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x00026000 File Offset: 0x00024200
	public static void PFunc_OnEquip_AdamsRibCage(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.AdamsRibCageTrigger));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Combine(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.AdamsRibCageReset));
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x0002605C File Offset: 0x0002425C
	public static void PFunc_OnUnequip_AdamsRibCage(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.AdamsRibCageTrigger));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnRoundEnd = (SlotMachineScript.Event)Delegate.Remove(instance2.OnRoundEnd, new SlotMachineScript.Event(PowerupScript.AdamsRibCageReset));
		PowerupScript.AdamsRibCageReset();
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x000260BA File Offset: 0x000242BA
	private static void AdamsRibCageTrigger()
	{
		if (!SlotMachineScript.Has999())
		{
			return;
		}
		PowerupScript._adamsRibCageCounter *= 2L;
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier._999_AdamsRibcage);
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x000260DB File Offset: 0x000242DB
	private static void AdamsRibCageReset()
	{
		PowerupScript._adamsRibCageCounter = 1L;
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x000260E4 File Offset: 0x000242E4
	public static long AdamsRibCage_PatternsMultiplierBonusGet(bool considerEquippedState)
	{
		if (!PowerupScript.IsEquipped_Quick(PowerupScript.Identifier._999_AdamsRibcage) && considerEquippedState)
		{
			return 1L;
		}
		return PowerupScript._adamsRibCageCounter;
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x00026100 File Offset: 0x00024300
	public static void PFunc_OnEquip_OphanimWheels(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Combine(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.OphanimWheelsTrigger));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnScoreEvaluationBegin = (SlotMachineScript.Event)Delegate.Combine(instance2.OnScoreEvaluationBegin, new SlotMachineScript.Event(PowerupScript.OphanimWheels_TransformJackpotCheck));
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x0002615C File Offset: 0x0002435C
	public static void PFunc_OnUnequip_OphanimWheels(PowerupScript powerup)
	{
		SlotMachineScript instance = SlotMachineScript.instance;
		instance.OnScoreEvaluationEnd = (SlotMachineScript.Event)Delegate.Remove(instance.OnScoreEvaluationEnd, new SlotMachineScript.Event(PowerupScript.OphanimWheelsTrigger));
		SlotMachineScript instance2 = SlotMachineScript.instance;
		instance2.OnScoreEvaluationBegin = (SlotMachineScript.Event)Delegate.Remove(instance2.OnScoreEvaluationBegin, new SlotMachineScript.Event(PowerupScript.OphanimWheels_TransformJackpotCheck));
		GameplayData.PowerupOphanimWheels_JackpotsCounter = 0;
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x000261BB File Offset: 0x000243BB
	private static void OphanimWheelsTrigger()
	{
		if (!SlotMachineScript.Has999())
		{
			return;
		}
		GameplayData.PowerupOphanimWheels_JackpotsCounter++;
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier._999_OphanimWheels);
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x000261DB File Offset: 0x000243DB
	public static int OphanimWheels_JackpotsBookedGet(bool decreaseCounter)
	{
		int powerupOphanimWheels_JackpotsCounter = GameplayData.PowerupOphanimWheels_JackpotsCounter;
		if (decreaseCounter)
		{
			GameplayData.PowerupOphanimWheels_JackpotsCounter--;
		}
		return powerupOphanimWheels_JackpotsCounter;
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x000261F1 File Offset: 0x000243F1
	private static void OphanimWheels_TransformJackpotCheck()
	{
		if (SlotMachineScript.Has666() || SlotMachineScript.Has999())
		{
			return;
		}
		if (PowerupScript.OphanimWheels_JackpotsBookedGet(true) <= 0)
		{
			return;
		}
		SlotMachineScript.Symbol_ReplaceAllVisible(GameplayData.Symbol_GetRandom_BasedOnSymbolChance(), SymbolScript.Modifier.none, true);
		PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier._999_OphanimWheels);
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x00026224 File Offset: 0x00024424
	public static PowerupScript Spawn(PowerupScript.Identifier identifier)
	{
		return Object.Instantiate<GameObject>(PowerupScript.GetPrefab(identifier)).GetComponent<PowerupScript>();
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x00026238 File Offset: 0x00024438
	public void Initialize(bool isNewGame, PowerupScript.Category category, PowerupScript.Identifier identifier, PowerupScript.Archetype archetype, bool isInstantPowerup, int maxBuyTimes, float storeRerollChance, int startingPrice, BigInteger unlockPrice, string nameKey, string descriptionKey, string unlockMissionKey, PowerupScript.PowerupEvent onEquip, PowerupScript.PowerupEvent onUnequip, PowerupScript.PowerupEvent onPutInDrawer, PowerupScript.PowerupEvent onThrowAway)
	{
		if (unlockPrice > 0L && unlockMissionKey != "POWERUP_UNLOCK_MISSION_BUY_FROM_THE_TERMINAL")
		{
			Debug.LogError("PowerupScript: Initialize: unlockPrice is greater than 0, but unlockMissionKey is not 'POWERUP_UNLOCK_BUY_FROM_THE_TERMINAL'! Powerup: " + identifier.ToString());
			unlockPrice = 0;
		}
		this.category = category;
		this.identifier = identifier;
		this.archetype = archetype;
		this.isInstantPowerup = isInstantPowerup;
		this.maxBuyTimes = maxBuyTimes;
		if (storeRerollChance < 0f)
		{
			Debug.LogError("PowerupScript: Initialize: storeRerollChance is negative! GameObject: " + base.gameObject.name);
			storeRerollChance = 0f;
		}
		if (storeRerollChance >= 1f)
		{
			Debug.LogError("PowerupScript: Initialize: storeRerollChance is too high! Value: " + storeRerollChance.ToString() + " - GameObject: " + base.gameObject.name);
			storeRerollChance = 0.65f;
		}
		this.storeRerollChance = storeRerollChance;
		this.startingPrice = (long)startingPrice;
		this.unlockPrice = unlockPrice;
		this.nameKey = nameKey;
		this.descriptionKey = descriptionKey;
		this.unlockMissionKey = unlockMissionKey;
		this._isBaseSet = unlockPrice <= 0L && unlockMissionKey == "POWERUP_UNLOCK_MISSION_NONE";
		this.onEquip = onEquip;
		this.onUnequip = onUnequip;
		this.onPutInDrawer = onPutInDrawer;
		this.onThrowAway = onThrowAway;
		this.diegeticMenuElement.promptGuideType = PromptGuideScript.GuideType.powerup;
		if (this.diegeticMenuElement.onSelectCallback == null)
		{
			this.diegeticMenuElement.onSelectCallback = new UnityEvent();
		}
		this.diegeticMenuElement.onSelectCallback.AddListener(new UnityAction(this.Inspect));
		if (this.diegeticMenuElement.onHoverCallback == null)
		{
			this.diegeticMenuElement.onHoverCallback = new UnityEvent();
		}
		this.diegeticMenuElement.onHoverCallback.AddListener(new UnityAction(this.diegeticMenuElement.SetPromptGuide_Powerup));
		if (!PowerupScript._initializationTempList.Contains(this))
		{
			PowerupScript._initializationTempList.Add(this);
		}
		if (!PowerupScript.dict_IdentifierToInstance.ContainsKey(identifier))
		{
			PowerupScript.dict_IdentifierToInstance.Add(identifier, this);
		}
		if (!AssetMaster.HasSound(this.triggerSpecificSound.name))
		{
			AssetMaster.AddSound(this.triggerSpecificSound, false);
		}
		if (archetype == PowerupScript.Archetype.sacred)
		{
			PowerupScript.sacredCharms.Add(this);
		}
		if (Translation.Get(descriptionKey).Contains("[K_LUCK]") && identifier != PowerupScript.Identifier.FortuneCookie && identifier != PowerupScript.Identifier.Jimbo)
		{
			PowerupScript.luckBasedCharms.Add(this);
		}
		if (archetype == PowerupScript.Archetype.symbolInstants && identifier != PowerupScript.Identifier.PhotoBook)
		{
			PowerupScript.picturesOfSymbols.Add(this);
		}
		this.MaterialRefresh();
		this.MaterialColorReset();
		if (this.meshRenderer != null)
		{
			this.modEffetctsHolder.SetParent(this.meshRenderer.transform);
			this.modEffetctsHolder.localPosition = Vector3.zero;
			this.modEffetctsHolder.localScale = Vector3.one;
			this.modEffetctsHolder.eulerAngles = Vector3.zero;
		}
		if (this.skinnedMeshRenderer != null)
		{
			this.modEffetctsHolder.SetParent(this.skinnedMeshRenderer.transform);
			this.modEffetctsHolder.localPosition = Vector3.zero;
			this.modEffetctsHolder.localScale = Vector3.one;
			this.modEffetctsHolder.eulerAngles = Vector3.zero;
		}
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x00026559 File Offset: 0x00024759
	private void _Uninitialize()
	{
		if (PowerupScript.dict_IdentifierToInstance.ContainsKey(this.identifier))
		{
			PowerupScript.dict_IdentifierToInstance.Remove(this.identifier);
		}
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x00026580 File Offset: 0x00024780
	public static void InitializeAll(bool placePowerups, bool isNewGame)
	{
		PowerupScript.throwAwayCanTriggerEffects = true;
		PowerupScript.inspectedPowerup = null;
		PowerupScript._forceClosingInspection_Death = false;
		PowerupScript.equipFlag_DontCheckForSpace = false;
		PowerupScript.onEquipStatic = null;
		PowerupScript.onUnequipStatic = null;
		PowerupScript.onPutInDrawerStatic = null;
		PowerupScript.onThrowAwayStatic = null;
		PowerupScript._initializationTempList.Clear();
		PowerupScript.list_NotBought.Clear();
		PowerupScript.list_EquippedSkeleton.Clear();
		PowerupScript.list_EquippedNormal.Clear();
		PowerupScript.sacredCharms.Clear();
		PowerupScript.luckBasedCharms.Clear();
		PowerupScript.picturesOfSymbols.Clear();
		for (int i = 0; i < PowerupScript.array_InDrawer.Length; i++)
		{
			PowerupScript.array_InDrawer[i] = null;
		}
		GameplayData instance = GameplayData.Instance;
		string text = null;
		int num = 164;
		for (int j = 0; j < num; j++)
		{
			PowerupScript.Identifier identifier = (PowerupScript.Identifier)j;
			if (identifier != PowerupScript.Identifier.undefined && identifier != PowerupScript.Identifier.count && !PowerupScript.dict_IdentifierToPrefabName.ContainsKey(identifier))
			{
				if (text == null)
				{
					text = "Missing prefabs for powerup identifiers: \n";
				}
				text = text + identifier.ToString() + "\n";
			}
		}
		if (text != null)
		{
			Debug.LogError(text);
		}
		List<PowerupScript.Identifier> list = Data.game.LockedPowerups_GetList();
		PowerupScript.dict_IsLocked.Clear();
		for (int k = 0; k < list.Count; k++)
		{
			if (list[k] != PowerupScript.Identifier.undefined && !PowerupScript.dict_IsLocked.ContainsKey(list[k]))
			{
				PowerupScript.dict_IsLocked.Add(list[k], true);
			}
		}
		PowerupScript.Spawn(PowerupScript.Identifier.Skeleton_Arm1).Initialize(isNewGame, PowerupScript.Category.skeleton, PowerupScript.Identifier.Skeleton_Arm1, PowerupScript.Archetype.skeleton, false, 0, 0.65f, 0, -1L, "POWERUP_NAME_SKELETON_ARM", "POWERUP_DESCR_SKELETON_ARM", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_SkeletonPiecesCommon), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Skeleton_Arm2).Initialize(isNewGame, PowerupScript.Category.skeleton, PowerupScript.Identifier.Skeleton_Arm2, PowerupScript.Archetype.skeleton, false, 0, 0.65f, 0, -1L, "POWERUP_NAME_SKELETON_ARM", "POWERUP_DESCR_SKELETON_ARM", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_SkeletonPiecesCommon), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Skeleton_Leg1).Initialize(isNewGame, PowerupScript.Category.skeleton, PowerupScript.Identifier.Skeleton_Leg1, PowerupScript.Archetype.skeleton, false, 0, 0.65f, 0, -1L, "POWERUP_NAME_SKELETON_LEG", "POWERUP_DESCR_SKELETON_LEG", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_SkeletonPiecesCommon), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Skeleton_Leg2).Initialize(isNewGame, PowerupScript.Category.skeleton, PowerupScript.Identifier.Skeleton_Leg2, PowerupScript.Archetype.skeleton, false, 0, 0.65f, 0, -1L, "POWERUP_NAME_SKELETON_LEG", "POWERUP_DESCR_SKELETON_LEG", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_SkeletonPiecesCommon), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Skeleton_Head).Initialize(isNewGame, PowerupScript.Category.skeleton, PowerupScript.Identifier.Skeleton_Head, PowerupScript.Archetype.skeleton, false, -1, 0f, 1, -1L, "POWERUP_NAME_SKELETON_HEAD", "POWERUP_DESCR_SKELETON_HEAD", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_SkeletonPiecesCommon), null, null, null);
		PowerupScript.ShroomsReset();
		PowerupScript.Spawn(PowerupScript.Identifier.Mushrooms).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Mushrooms, PowerupScript.Archetype.generic, false, -1, 0f, 2, -1L, "POWERUP_NAME_MUSHROOM", "POWERUP_DESCR_MUSHROOM", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Shrooms), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_Shrooms), null, null);
		PowerupScript.RorschachReset();
		PowerupScript.Spawn(PowerupScript.Identifier.Rorschach).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Rorschach, PowerupScript.Archetype.generic, false, -1, 0f, 1, -1L, "POWERUP_NAME_RORSCHACH", "POWERUP_DESCR_RORSCHACH", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Rorschach), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_Rorschach), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.CloverPot).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.CloverPot, PowerupScript.Archetype.generic, false, -1, 0.2f, 2, -1L, "POWERUP_NAME_CLOVERPOT", "POWERUP_DESCR_CLOVERPOT", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_CloverPot), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_CloverPot), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Hourglass).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Hourglass, PowerupScript.Archetype.generic, false, -1, 0f, 1, -1L, "POWERUP_NAME_HOURGLASS", "POWERUP_DESCR_HOURGLASS", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Hourglass), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_Hourglass), null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_Hourglass));
		PowerupScript.Spawn(PowerupScript.Identifier.FruitBasket).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.FruitBasket, PowerupScript.Archetype.generic, false, -1, 0.35f, 4, -1L, "POWERUP_NAME_FRUIT_BASKET", "POWERUP_DESCR_FRUIT_BASKET", "POWERUP_UNLOCK_MISSION_FRUIT_BASKET", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_FruitBasket), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_FruitBasket), null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_FruitBasket));
		PowerupScript.Spawn(PowerupScript.Identifier.SevenSinsStone).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.SevenSinsStone, PowerupScript.Archetype.generic, false, -1, 0.65f, 2, -1L, "POWERUP_NAME_7_SINS_STONE", "POWERUP_DESCR_7_SINS_STONE", "POWERUP_UNLOCK_MISSION_7_SINS_STONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_7SinsStone), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_7SinsStone), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Necklace).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Necklace, PowerupScript.Archetype.generic, false, -1, 0.35f, 1, -1L, "POWERUP_NAME_WIFE_NECKLACE", "POWERUP_DESCR_WIFE_NECKLACE", "POWERUP_UNLOCK_MISSION_WIFE_NECKLACE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.CloverBell).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.CloverBell, PowerupScript.Archetype.generic, false, -1, 0.35f, 1, -1L, "POWERUP_NAME_CLOVERBELL", "POWERUP_DESCR_CLOVERBELL", "POWERUP_UNLOCK_MISSION_CLOVERBELL", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_CloverBell), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_CloverBell), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Cigarettes).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Cigarettes, PowerupScript.Archetype.generic, true, -1, 0f, 3, -1L, "POWERUP_NAME_CIGARETTES", "POWERUP_DESCR_CIGARETTES", "POWERUP_UNLOCK_CIGARETTES", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Cigarettes), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.ElectricityCounter).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.ElectricityCounter, PowerupScript.Archetype.generic, false, -1, 0.8f, 4, -1L, "POWERUP_NAME_ELECTRICITY_METER", "POWERUP_DESCR_ELECTRICITY_METER", "POWERUP_UNLOCK_ELECTRICITY_METER", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.PotatoPower).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.PotatoPower, PowerupScript.Archetype.generic, false, -1, 0.35f, 1, -1L, "POWERUP_NAME_POTATO_BATTERY", "POWERUP_DESCR_POTATO_BATTERY", "POWERUP_UNLOCK_POTATO_BATTERY", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_PotatoPower), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_PotatoPower), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.CardboardHouse).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.CardboardHouse, PowerupScript.Archetype.generic, true, 1, 0f, 2, -1L, "POWERUP_NAME_CARDBOARD_HOUSE", "POWERUP_DESCR_CARDBOARD_HOUSE", "POWERUP_UNLOCK_CARDBOARD_HOUSE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_CardboardHouse), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.CrowBar).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.CrowBar, PowerupScript.Archetype.generic, true, -1, 0.1f, 2, -1L, "POWERUP_NAME_CROWBAR", "POWERUP_DESCR_CROWBAR", "POWERUP_UNLOCK_CONDITION_CROWBAR", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_CrowBar), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.ShoppingCart).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.ShoppingCart, PowerupScript.Archetype.generic, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_SHOPPING_KART", "POWERUP_DESCR_SHOPPING_KART", "POWERUP_UNLOCK_CONDITION_SHOPPING_KART", null, null, null, null);
		if (isNewGame)
		{
			GameplayData.Powerup_Jimbo_ReshuffleAndReset();
		}
		PowerupScript.Spawn(PowerupScript.Identifier.Jimbo).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Jimbo, PowerupScript.Archetype.generic, false, -1, 0.35f, 5, -1L, "POWERUP_NAME_JIMBO", "POWERUP_DESCR_JIMBO", "POWERUP_UNLOCK_CONDITION_JIMBO", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Jimbo), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_Jimbo), null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_Jimbo));
		PowerupScript.Spawn(PowerupScript.Identifier.DiscC).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.DiscC, PowerupScript.Archetype.generic, false, -1, 0f, 1, -1L, "POWERUP_NAME_VINYL_DISC_C", "POWERUP_DESCR_VINYL_DISC_C", "POWERUP_UNLOCK_CONDITION_DISC_C", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_DiscC), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_DiscC), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.DiscB).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.DiscB, PowerupScript.Archetype.generic, false, -1, 0f, 1, -1L, "POWERUP_NAME_VINYL_DISC_B", "POWERUP_DESCR_VINYL_DISC_B", "POWERUP_UNLOCK_CONDITION_DISC_B", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_DiscB), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_DiscB), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.DiscA).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.DiscA, PowerupScript.Archetype.generic, false, -1, 0f, 1, -1L, "POWERUP_NAME_VINYL_DISC_A", "POWERUP_DESCR_VINYL_DISC_A", "POWERUP_UNLOCK_CONDITION_DISC_A", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_DiscA), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_DiscA), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.MusicTape).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.MusicTape, PowerupScript.Archetype.generic, true, -1, 0.8f, 5, -1L, "POWERUP_NAME_MUSIC_TAPE", "POWERUP_DESCR_MUSIC_TAPE", "POWERUP_UNLOCK_CONDITION_MUSIC_TAPE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_MusicTape), null, null, null);
		PowerupScript.WeirdClockDeadlineReset();
		PowerupScript.Spawn(PowerupScript.Identifier.WeirdClock).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.WeirdClock, PowerupScript.Archetype.generic, false, -1, 0.65f, 5, -1L, "POWERUP_NAME_BACKWARDS_CLOCK", "POWERUP_DESCR_BACKWARDS_CLOCK", "POWERUP_UNLOCK_CONDITION_BACKWARDS_CLOCK", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_WeirdClock), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_WeirdClock), null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_WeirdClock));
		PowerupScript.Spawn(PowerupScript.Identifier.LocomotiveSteam).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.LocomotiveSteam, PowerupScript.Archetype.generic, false, -1, 0.35f, 2, -1L, "POWERUP_NAME_LOCOMOTIVE_STEAM", "POWERUP_DESCR_LOCOMOTIVE_STEAM", "POWERUP_UNLOCK_CONDITION_LOCOMOTIVE_STEAM", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_SteamLocomotive), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_SteamLocomotive), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.LocomotiveDiesel).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.LocomotiveDiesel, PowerupScript.Archetype.generic, false, -1, 0.35f, 2, -1L, "POWERUP_NAME_LOCOMOTIVE_DIESEL", "POWERUP_DESCR_LOCOMOTIVE_DIESEL", "POWERUP_UNLOCK_CONDITION_LOCOMOTIVE_DIESEL", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_DieselLocomotive), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_DieselLocomotive), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Depression).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Depression, PowerupScript.Archetype.generic, false, -1, 0.65f, 1, -1L, "POWERUP_NAME_DEPRESSION", "POWERUP_DESCR_DEPRESSION", "POWERUP_UNLOCK_CONDITION_DEPRESSION", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Depression), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.DarkLotus).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.DarkLotus, PowerupScript.Archetype.generic, false, -1, 0.8f, 3, -1L, "POWERUP_NAME_BLACK_LOTUS", "POWERUP_DESCR_BLACK_LOTUS", "POWERUP_UNLOCK_CONDITION_BLACK_LOTUS", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.CloversLandPatch).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.CloversLandPatch, PowerupScript.Archetype.generic, false, -1, 0.2f, 3, -1L, "POWERUP_NAME_CLOVER_LAND_PATCH", "POWERUP_DESCR_CLOVER_LAND_PATCH", "POWERUP_UNLOCK_CONDITION_LAND_PATCH", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_FieldOfClovers), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_FieldOfClovers), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.ConsolationPrize).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.ConsolationPrize, PowerupScript.Archetype.generic, false, -1, 0f, 2, -1L, "POWERUP_NAME_CONSOLATION_PRIZE", "POWERUP_DESCR_CONSOLATION_PRIZE", "POWERUP_UNLOCK_CONDITION_CONSOLATION_PRIZE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_ConsolationPrize), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_ConsolationPrize), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.RingBell).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.RingBell, PowerupScript.Archetype.generic, false, -1, 0f, 3, -1L, "POWERUP_NAME_RING_BELL", "POWERUP_DESCR_RING_BELL", "POWERUP_UNLOCK_CONDITION_RING_BELL", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_RingBell), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_RingBell), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.VoiceMailTape).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.VoiceMailTape, PowerupScript.Archetype.generic, false, -1, 0.35f, 2, -1L, "POWERUP_NAME_VOICEMAIL_TAPE", "POWERUP_DESCR_VOICEMAIL_TAPE", "POWERUP_UNLOCK_CONDITION_VOICEMAIL_TAPE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Pareidolia).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Pareidolia, PowerupScript.Archetype.generic, false, -1, 0.5f, 2, -1L, "POWERUP_NAME_PARIDOLIA", "POWERUP_DESCR_PARIDOLIA", "POWERUP_UNLOCK_CONDITION_PARIDOLIA", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Pareidolia), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_Pareidolia), null, null);
		PowerupScript.AbstractPaintingReset();
		PowerupScript.Spawn(PowerupScript.Identifier.AbstractPainting).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.AbstractPainting, PowerupScript.Archetype.generic, false, -1, 0.35f, 2, -1L, "POWERUP_NAME_ABSTRACT_PAINTING", "POWERUP_DESCR_ABSTRACT_PAINTING", "POWERUP_UNLOCK_CONDITION_ABSTRACT_PAINTING", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_AbstractPainting), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_AbstractPainting), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.EyeJar).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.EyeJar, PowerupScript.Archetype.generic, false, -1, 0.35f, 2, -1L, "POWERUP_NAME_EYE_JAR", "POWERUP_DESCR_EYE_JAR", "POWERUP_UNLOCK_MISSION_EYE_JAR", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Nose).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Nose, PowerupScript.Archetype.generic, false, -1, 0.2f, 3, -1L, "POWERUP_NAME_DANTES_NOSE", "POWERUP_DESCR_DANTES_NOSE", "POWERUP_UNLOCK_MISSION_NOSE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.FortuneChanneler).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.FortuneChanneler, PowerupScript.Archetype.generic, false, -1, 0.2f, 3, -1L, "POWERUP_NAME_FORTUNE_CHANNELER", "POWERUP_DESCR_FORTUNE_CHANNELER", "POWERUP_UNLOCK_MISSION_FORTUNE_CHANNELER", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_ChannelerOfFortune), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_ChannelerOfFortune), null, null);
		PowerupScript.GoldenHorseShoe_OnRoundEndReset();
		PowerupScript.horseShoesLuck = 0f;
		PowerupScript.Spawn(PowerupScript.Identifier.HorseShoeGold).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.HorseShoeGold, PowerupScript.Archetype.generic, false, -1, 0.65f, 4, -1L, "POWERUP_NAME_GOLDEN_HORSESHOE", "POWERUP_DESCR_GOLDEN_HORSESHOE", "POWERUP_UNLOCK_MISSION_GOLDEN_HORSESHOE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_GoldenHorseShoe), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_GoldenHorseShoe), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.HamsaUpside).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.HamsaUpside, PowerupScript.Archetype.generic, false, -1, 0.1f, 2, -1L, "POWERUP_NAME_HAMSA_UPSIDE", "POWERUP_DESCR_HAMSA_UPSIDE", "POWERUP_UNLOCK_MISSION_HAMSA_UPSIDE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_UpsideHamsa), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_UpsideHamsa), null, null);
		PowerupScript.AncientCoinOnRoundEnd();
		PowerupScript.Spawn(PowerupScript.Identifier.AncientCoin).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.AncientCoin, PowerupScript.Archetype.generic, false, -1, 0f, 1, -1L, "POWERUP_NAME_ANCIENT_COIN", "POWERUP_DESCR_ANCIENT_COIN", "POWERUP_UNLOCK_MISSION_ANCIENT_COIN", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_AncientCoin), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_AncientCoin), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.LuckyCatFat).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.LuckyCatFat, PowerupScript.Archetype.generic, false, -1, 0.2f, 2, -1L, "POWERUP_NAME_LUCKY_CAT_FAT", "POWERUP_DESCR_LUCKY_CAT_FAT", "POWERUP_UNLOCK_MISSION_LUCKY_CAT_FAT", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_LuckyCatFat), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_LuckyCatFat), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.LuckyCatSwole).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.LuckyCatSwole, PowerupScript.Archetype.generic, false, -1, 0.8f, 3, -1L, "POWERUP_NAME_LUCKY_CAT_SWOLE", "POWERUP_DESCR_LUCKY_CAT_SWOLE", "POWERUP_UNLOCK_MISSION_LUCKY_CAT_SWOLE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_LuckyCatSwole), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_LuckyCatSwole), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.TheCollector).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.TheCollector, PowerupScript.Archetype.generic, false, -1, 0.65f, 3, -1L, "POWERUP_NAME_THE_COLLECTOR", "POWERUP_DESCR_THE_COLLECTOR", "POWERUP_UNLOCK_MISSION_THE_COLLECTOR", null, null, null, null);
		PowerupScript.BrokenCalculatorReset();
		PowerupScript.Spawn(PowerupScript.Identifier.BrokenCalculator).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.BrokenCalculator, PowerupScript.Archetype.generic, false, -1, 0.2f, 3, -1L, "POWERUP_NAME_BROKEN_CALCULATOR", "POWERUP_DESCR_BROKEN_CALCULATOR", "POWERUP_UNLOCK_MISSION_BROKEN_CALCULATOR", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_BrokenCalculator), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_BrokenCalculator), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Sardines).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Sardines, PowerupScript.Archetype.generic, false, -1, 0.65f, 1, -1L, "POWERUP_NAME_SARDINES", "POWERUP_DESCR_SARDINES", "POWERUP_UNLOCK_MISSION_SARDINES", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.FideltyCard).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.FideltyCard, PowerupScript.Archetype.generic, false, -1, 0.2f, 2, -1L, "POWERUP_NAME_FIDELTY_CARD", "POWERUP_DESCR_FIDELTY_CARD", "POWERUP_UNLOCK_MISSION_FIDELTY_CARD", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_FideltyCard), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_FideltyCard), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GiantShroom).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GiantShroom, PowerupScript.Archetype.generic, false, -1, 0.65f, 3, -1L, "POWERUP_NAME_GIGA_SHROOM", "POWERUP_DESCR_GIGA_SHROOM", "POWERUP_UNLOCK_MISSION_GIGA_SHROOM", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_GiantShroom), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_GiantShroom), null, null);
		PowerupScript.VineShroomsReset();
		PowerupScript.Spawn(PowerupScript.Identifier.VineSoupShroom).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.VineSoupShroom, PowerupScript.Archetype.generic, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_VINESOUP_SHROOM", "POWERUP_DESCR_VINESOUP_SHROOM", "POWERUP_UNLOCK_MISSION_VINESOUP_SHROOM", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_VineShroom), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_VineShroom), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.FortuneCookie).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.FortuneCookie, PowerupScript.Archetype.generic, true, -1, 0f, 1, -1L, "POWERUP_NAME_FORTUNE_COOKIE", "POWERUP_DESCR_FORTUNE_COOKIE", "POWERUP_UNLOCK_MISSION_FORTUNE_COOKIE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_FortuneCookie), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.YellowStar).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.YellowStar, PowerupScript.Archetype.generic, false, -1, 0f, 3, -1L, "POWERUP_NAME_YELLOW_STAR", "POWERUP_DESCR_YELLOW_STAR", "POWERUP_UNLOCK_MISSION_YELLOW_STAR", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_YellowStar), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_YellowStar), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Calendar).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Calendar, PowerupScript.Archetype.generic, false, -1, 0.1f, 3, -1L, "POWERUP_NAME_CALENDAR", "POWERUP_DESCR_CALENDAR", "POWERUP_UNLOCK_MISSION_CALENDAR", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Wallet).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Wallet, PowerupScript.Archetype.generic, false, -1, 0f, 1, -1L, "POWERUP_NAME_WALLET", "POWERUP_DESCR_WALLET", "POWERUP_UNLOCK_MISSION_WALLET", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Painkillers).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Painkillers, PowerupScript.Archetype.generic, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_PAIN_KILLERS", "POWERUP_DESCR_PAIN_KILLERS", "POWERUP_UNLOCK_MISSION_PAIN_KILLERS", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GoldenHand_MidasTouch).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GoldenHand_MidasTouch, PowerupScript.Archetype.generic, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_GOLDEN_HAND", "POWERUP_DESCR_GOLDEN_HAND", "POWERUP_UNLOCK_MISSION_GOLDEN_HAND", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_GoldenHandMidaTouch), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_GoldenHandMidaTouch), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.ExpiredMedicines).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.ExpiredMedicines, PowerupScript.Archetype.generic, false, -1, 0.2f, 1, -1L, "POWERUP_NAME_EXPIRED_MEDICINES", "POWERUP_DESCR_EXPIRED_MEDICINES", "POWERUP_UNLOCK_MISSION_EXPIRED_MEDICINES", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GrattaEVinci_ScratchAndWin).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GrattaEVinci_ScratchAndWin, PowerupScript.Archetype.generic, false, -1, 0.65f, 5, -1L, "POWERUP_NAME_GRATTA_E_VINCI", "POWERUP_DESCR_GRATTA_E_VINCI", "POWERUP_UNLOCK_MISSION_GRATTA_E_VINCI", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.CrankGenerator).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.CrankGenerator, PowerupScript.Archetype.generic, false, -1, 0f, 2, -1L, "POWERUP_NAME_CRANK_GENERATOR", "POWERUP_DESCR_CRANK_GENERATOR", "POWERUP_UNLOCK_MISSION_CRANK_GENERATOR", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_CrankGenerator), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_CrankGenerator), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.SuperCapacitor).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.SuperCapacitor, PowerupScript.Archetype.generic, false, -1, 0.1f, 4, -1L, "POWERUP_NAME_SUPER_CAPACITOR", "POWERUP_DESCR_SUPER_CAPACITOR", "POWERUP_UNLOCK_MISSION_SUPER_CAPACITOR", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_SuperCapacitor), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_SuperCapacitor), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.DearDiary).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.DearDiary, PowerupScript.Archetype.generic, false, -1, 0f, 2, -1L, "POWERUP_NAME_DIARY", "POWERUP_DESCR_DIARY", "POWERUP_UNLOCK_MISSION_DIARY", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_DearDiary), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_DearDiary), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.HouseContract).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.HouseContract, PowerupScript.Archetype.generic, false, -1, 0.1f, 2, -1L, "POWERUP_NAME_HOUSE_CONTRACT", "POWERUP_DESCR_HOUSE_CONTRACT", "POWERUP_UNLOCK_MISSION_HOUSE_CONTRACT", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_HouseContract), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.PoopBeetle).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.PoopBeetle, PowerupScript.Archetype.generic, false, -1, 0.65f, 5, -1L, "POWERUP_NAME_POOP_BEETLE", "POWERUP_DESCR_POOP_BEETLE", "POWERUP_UNLOCK_MISSION_DUNG_BEETLE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_PoopBeetle), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_PoopBeetle), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.OneTrickPony).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.OneTrickPony, PowerupScript.Archetype.generic, false, -1, 0.35f, 2, -1L, "POWERUP_NAME_ONE_TRICK_PONY", "POWERUP_DESCR_ONE_TRICK_PONY", "POWERUP_UNLOCK_MISSION_ONE_TRICK_PONY", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_OneTrickPony), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_OneTrickPony), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Ankh).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Ankh, PowerupScript.Archetype.generic, false, -1, 0.35f, 2, -1L, "POWERUP_NAME_ANKH", "POWERUP_DESCR_ANKH", "POWERUP_UNLOCK_MISSION_ANKH", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Ankh), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_Ankh), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.CloverVoucher).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.CloverVoucher, PowerupScript.Archetype.generic, true, -1, 0f, 2, -1L, "POWERUP_NAME_CLOVER_VOUCHER", "POWERUP_DESCR_CLOVER_VOUCHER", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_CloverVoucher), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.MoneyBriefCase).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.MoneyBriefCase, PowerupScript.Archetype.generic, true, -1, 0f, 2, -1L, "POWERUP_NAME_MONEY_BRIEF_CASE", "POWERUP_DESCR_MONEY_BRIEF_CASE", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_MoneyBriefCase), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.PoopJar).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.PoopJar, PowerupScript.Archetype.generic, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_POOP_JAR", "POWERUP_DESCR_POOP_JAR", "POWERUP_UNLOCK_POOP_JAR", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_PoopJar), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_PoopJar), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.PissJar).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.PissJar, PowerupScript.Archetype.generic, false, -1, 0.5f, 3, -1L, "POWERUP_NAME_PISS_JAR", "POWERUP_DESCR_PISS_JAR", "POWERUP_UNLOCK_PISS_JAR", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_PissJar), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_PissJar), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.CarBattery).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.CarBattery, PowerupScript.Archetype.generic, true, -1, 0f, 2, -1L, "POWERUP_NAME_CAR_BATTERY", "POWERUP_DESCR_CAR_BATTERY", "POWERUP_UNLOCK_MISSION_CAR_BATTERY", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_CarBattery), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Stonks).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Stonks, PowerupScript.Archetype.generic, false, -1, 0f, 2, -1L, "POWERUP_NAME_STONKS", "POWERUP_DESCR_STONKS", "POWERUP_UNLOCK_MISSION_NONE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.ToyTrain).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.ToyTrain, PowerupScript.Archetype.generic, false, -1, 0f, 1, -1L, "POWERUP_NAME_TOY_TRAIN", "POWERUP_DESCR_TOY_TRAIN", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_ToyTrain), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_ToyTrain), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Megaphone).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Megaphone, PowerupScript.Archetype.generic, false, -1, 0.8f, 7, -1L, "POWERUP_NAME_MEGAPHONE", "POWERUP_DESCR_MEGAPHONE", "POWERUP_UNLOCK_MISSION_NONE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Button2X).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Button2X, PowerupScript.Archetype.generic, false, -1, 0.65f, 4, -1L, "POWERUP_NAME_2XBUTTON", "POWERUP_DESCR_2XBUTTON", "POWERUP_UNLOCK_MISSION_2XBUTTON", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.CloverPet).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.CloverPet, PowerupScript.Archetype.generic, false, -1, 0.35f, 1, -1L, "POWERUP_NAME_CLOVERPET", "POWERUP_DESCR_CLOVERPET", "POWERUP_UNLOCK_MISSION_NONE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.CatTreats).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.CatTreats, PowerupScript.Archetype.generic, false, -1, 0f, 4, -1L, "POWERUP_NAME_CAT_FOOD", "POWERUP_DESCR_CAT_FOOD", "POWERUP_UNLOCK_MISSION_NONE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Pentacle).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Pentacle, PowerupScript.Archetype.generic, false, -1, 0.2f, 3, -1L, "POWERUP_NAME_PENTACLE", "POWERUP_DESCR_PENTACLE", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Pentacle), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_Pentacle), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GrandmasPurse).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GrandmasPurse, PowerupScript.Archetype.generic, false, -1, 0f, 2, -1L, "POWERUP_NAME_GRANDMA_PURSE", "POWERUP_DESCR_GRANDMA_PURSE", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_GrandmasPurse), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_GrandmasPurse), null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_GrandmasPurse));
		PowerupScript.Spawn(PowerupScript.Identifier.RedCrystal).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.RedCrystal, PowerupScript.Archetype.generic, false, -1, 0f, 1, -1L, "POWERUP_NAME_RED_CRYSTAL", "POWERUP_DESCR_RED_CRYSTAL", "POWERUP_UNLOCK_MISSION_RED_CRYSTAL", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_RedCrystal), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_RedCrystal), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.HamsaInverted).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.HamsaInverted, PowerupScript.Archetype.generic, false, -1, 0f, 3, -1L, "POWERUP_NAME_INVERTED_HAMSA", "POWERUP_DESCR_INVERTED_HAMSA", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_InvertedHamsa), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_InvertedHamsa), null, null);
		PowerupScript._tarotDeck_TriggersPerSpin = 0L;
		PowerupScript.Spawn(PowerupScript.Identifier.TarotDeck).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.TarotDeck, PowerupScript.Archetype.generic, false, -1, 0.35f, 2, -1L, "POWERUP_NAME_TAROT_DECK", "POWERUP_DESCR_TAROT_DECK", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_TarotDeck), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_TarotDeck), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.FakeCoin).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.FakeCoin, PowerupScript.Archetype.generic, false, -1, 0f, 1, -1L, "POWERUP_NAME_FAKE_COIN", "POWERUP_DESCR_FAKE_COIN", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_FakeCoin), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_FakeCoin), null, null);
		PowerupScript.horseShoesLuck = 0f;
		PowerupScript.Spawn(PowerupScript.Identifier.HorseShoe).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.HorseShoe, PowerupScript.Archetype.generic, false, -1, 0f, 3, -1L, "POWERUP_NAME_HORSESHOE", "POWERUP_DESCR_HORSESHOE", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_HorseShoe), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_HorseShoe), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.LuckyCat).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.LuckyCat, PowerupScript.Archetype.generic, false, -1, 0f, 1, -1L, "POWERUP_NAME_LUCKY_CAT", "POWERUP_DESCR_LUCKY_CAT", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_LuckyCat), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_LuckyCat), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.HornChilyRed).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.HornChilyRed, PowerupScript.Archetype.spicyPeppers, false, -1, 0f, 1, -1L, "POWERUP_NAME_SPICY_RED", "POWERUP_DESCR_SPICY_RED", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_SpicyPepper_Red), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_SpicyPepper_Red), null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_SpicyPepper_Red));
		PowerupScript.Spawn(PowerupScript.Identifier.HornChilyGreen).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.HornChilyGreen, PowerupScript.Archetype.spicyPeppers, false, -1, 0f, 2, -1L, "POWERUP_NAME_SPICY_GREEN", "POWERUP_DESCR_SPICY_GREEN", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_SpicyPepper_Green), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_SpicyPepper_Green), null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_SpicyPepper_Green));
		PowerupScript.Spawn(PowerupScript.Identifier.GoldenPepper).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GoldenPepper, PowerupScript.Archetype.spicyPeppers, false, -1, 0.35f, 2, -1L, "POWERUP_NAME_GOLDEN_PEPPER", "POWERUP_DESCR_GOLDEN_PEPPER", "POWERUP_UNLOCK_MISSION_GOLDEN_PEPPER", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_GoldenPepper), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_GoldenPepper), null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_GoldenPepper));
		PowerupScript.RottenPepper_Reset();
		PowerupScript.Spawn(PowerupScript.Identifier.RottenPepper).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.RottenPepper, PowerupScript.Archetype.spicyPeppers, false, -1, 0.35f, 2, -1L, "POWERUP_NAME_ROTTEN_PEPPER", "POWERUP_DESCR_ROTTEN_PEPPER", "POWERUP_UNLOCK_MISSION_ROTTEN_PEPPER", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_RottenPepper), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_RottenPepper), null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_RottenPepper));
		PowerupScript.Spawn(PowerupScript.Identifier.BellPepper).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.BellPepper, PowerupScript.Archetype.spicyPeppers, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_BELL_PEPPER", "POWERUP_DESCR_BELL_PEPPER", "POWERUP_UNLOCK_MISSION_BELL_PEPPER", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_BellPepper), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_BellPepper), null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_BellPepper));
		PowerupScript.Spawn(PowerupScript.Identifier.ChastityBelt).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.ChastityBelt, PowerupScript.Archetype.religious, false, -1, 0.5f, 3, -1L, "POWERUP_NAME_CASTITY_BELT", "POWERUP_DESCR_CASTITY_BELT", "POWERUP_UNLOCK_CONDITION_CASTITY_BELT", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.EvilDeal).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.EvilDeal, PowerupScript.Archetype.religious, false, -1, 0.35f, 50, -1L, "POWERUP_NAME_PACT_WITH_THE_DEVIL", "POWERUP_DESCR_PACT_WITH_THE_DEVIL", "POWERUP_UNLOCK_CONDITION_PACT_WITH_THE_DEVIL", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.HornDevil).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.HornDevil, PowerupScript.Archetype.religious, false, -1, 0.1f, 2, -1L, "POWERUP_NAME_DEVIL_HORN", "POWERUP_DESCR_DEVIL_HORN", "POWERUP_UNLOCK_MISSION_DEVIL_HORN", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_DevilHorn), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_DevilHorn), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Necronomicon).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Necronomicon, PowerupScript.Archetype.religious, false, -1, 0.1f, 3, -1L, "POWERUP_NAME_NECRONOMICON", "POWERUP_DESCR_NECRONOMICON", "POWERUP_UNLOCK_MISSION_NECRONOMICON", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Necronomicon), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Baphomet).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Baphomet, PowerupScript.Archetype.religious, false, -1, 0.65f, 3, -1L, "POWERUP_NAME_BAPHOMET", "POWERUP_DESCR_BAPHOMET", "POWERUP_UNLOCK_MISSION_BAPHOMET", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Baphomet), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_Baphomet), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Cross).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Cross, PowerupScript.Archetype.religious, false, -1, 0.65f, 4, -1L, "POWERUP_NAME_CROSS", "POWERUP_DESCR_CROSS", "POWERUP_UNLOCK_MISSION_CROSS", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Cross), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_Cross), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Rosary).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Rosary, PowerupScript.Archetype.religious, false, -1, 0f, 1, -1L, "POWERUP_NAME_ROSARY", "POWERUP_DESCR_ROSARY", "POWERUP_UNLOCK_MISSION_ROSARY", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Rosary), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_Rosary), null, null);
		PowerupScript.BookOfShadowsReset();
		PowerupScript.Spawn(PowerupScript.Identifier.BookOfShadows).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.BookOfShadows, PowerupScript.Archetype.religious, false, -1, 0.35f, 2, -1L, "POWERUP_NAME_BOOK_OF_SHADOWS", "POWERUP_DESCR_BOOK_OF_SHADOWS", "POWERUP_UNLOCK_MISSION_BOOK_OF_SHADOWS", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_BookOfShadows), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_BookOfShadows), null, null);
		PowerupScript.GabibbhReset();
		PowerupScript.Spawn(PowerupScript.Identifier.Gabibbh).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Gabibbh, PowerupScript.Archetype.religious, false, -1, 0.1f, 2, -1L, "POWERUP_NAME_GABIBBH", "POWERUP_DESCR_GABIBBH", "POWERUP_UNLOCK_MISSION_GABIBBH", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Gabibbh), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_Gabibbh), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.PossessedPhone).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.PossessedPhone, PowerupScript.Archetype.religious, false, -1, 0.35f, 1, -1L, "POWERUP_NAME_POSSESSED_PHONE", "POWERUP_DESCR_POSSESSED_PHONE", "POWERUP_UNLOCK_MISSION_POSSESSED_PHONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_PossessedPhone), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_PossessedPhone), null, null);
		PowerupScript.MysticalTomatoReset();
		PowerupScript.Spawn(PowerupScript.Identifier.MysticalTomato).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.MysticalTomato, PowerupScript.Archetype.religious, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_MAGIC_TOMATO", "POWERUP_DESCR_MAGIC_TOMATO", "POWERUP_UNLOCK_MISSION_MAGIC_TOMATO", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_MysticalTomato), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_MysticalTomato), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.RitualBell).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.RitualBell, PowerupScript.Archetype.religious, false, -1, 0f, 1, -1L, "POWERUP_NAME_RITUAL_BELL", "POWERUP_DESCR_RITUAL_BELL", "POWERUP_UNLOCK_MISSION_RITUAL_BELL", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_RitualBell), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_RitualBell), null, null);
		PowerupScript.CrystalSkullReset();
		PowerupScript.Spawn(PowerupScript.Identifier.CrystalSkull).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.CrystalSkull, PowerupScript.Archetype.religious, false, -1, 0.2f, 2, -1L, "POWERUP_NAME_DEMON_CRYSTAL_SKULL", "POWERUP_DESCR_DEMON_CRYSTAL_SKULL", "POWERUP_UNLOCK_MISSION_CRYSTAL_SKULL", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_CrystalSkull), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_CrystalSkull), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.HolyBible).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.HolyBible, PowerupScript.Archetype.religious, false, -1, 0f, 1, -1L, "POWERUP_NAME_HOLY_BIBLE", "POWERUP_DESCR_HOLY_BIBLE", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_HolyBible), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_HolyBible), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.PhotoBook).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.PhotoBook, PowerupScript.Archetype.symbolInstants, true, -1, 0f, 1, -1L, "POWERUP_NAME_PHOTO_BOOK", "POWERUP_DESCR_PHOTO_BOOK", "POWERUP_UNLOCK_MISSION_PHOTO_BOOK", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_PhotoBook), null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.SymbolInstant_Lemon).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.SymbolInstant_Lemon, PowerupScript.Archetype.symbolInstants, false, -1, 0f, 2, -1L, "POWERUP_NAME_LEMON_PICTURE", "POWERUP_DESCR_LEMON_PICTURE", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_LemonPicture), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_LemonPicture), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.SymbolInstant_Cherry).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.SymbolInstant_Cherry, PowerupScript.Archetype.symbolInstants, false, -1, 0f, 2, -1L, "POWERUP_NAME_CHERRY_PICTURE", "POWERUP_DESCR_CHERRY_PICTURE", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_CherryPicture), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_CherryPicture), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.SymbolInstant_Clover).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.SymbolInstant_Clover, PowerupScript.Archetype.symbolInstants, false, -1, 0.1f, 3, -1L, "POWERUP_NAME_CLOVER_PICTURE", "POWERUP_DESCR_CLOVER_PICTURE", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_CloverPicture), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_CloverPicture), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.SymbolInstant_Bell).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.SymbolInstant_Bell, PowerupScript.Archetype.symbolInstants, false, -1, 0.1f, 3, -1L, "POWERUP_NAME_BELL_PICTURE", "POWERUP_DESCR_BELL_PICTURE", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_BellPicture), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_BellPicture), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.SymbolInstant_Diamond).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.SymbolInstant_Diamond, PowerupScript.Archetype.symbolInstants, false, -1, 0.2f, 4, -1L, "POWERUP_NAME_DIAMOND_PICTURE", "POWERUP_DESCR_DIAMOND_PICTURE", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_DiamondPicture), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_DiamondPicture), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.SymbolInstant_Treasure).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.SymbolInstant_Treasure, PowerupScript.Archetype.symbolInstants, false, -1, 0.2f, 4, -1L, "POWERUP_NAME_COINS_PICTURE", "POWERUP_DESCR_COINS_PICTURE", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_CoinsPicture), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_CoinsPicture), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.SymbolInstant_Seven).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.SymbolInstant_Seven, PowerupScript.Archetype.symbolInstants, false, -1, 0.35f, 4, -1L, "POWERUP_NAME_SEVEN_PICTURE", "POWERUP_DESCR_SEVEN_PICTURE", "POWERUP_UNLOCK_MISSION_NONE", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_SevenPicture), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_SevenPicture), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GeneralModCharm_Clicker).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GeneralModCharm_Clicker, PowerupScript.Archetype.generalModifierCharm, false, -1, 0.2f, 3, -1L, "POWERUP_NAME_CLICKER", "POWERUP_DESCR_CLICKER", "POWERUP_UNLOCK_MISSION_CLICKER", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GeneralModCharm_CloverBellBattery).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GeneralModCharm_CloverBellBattery, PowerupScript.Archetype.generalModifierCharm, false, -1, 0.5f, 3, -1L, "POWERUP_NAME_CLOVERBELL_BATTERY", "POWERUP_DESCR_CLOVERBELL_BATTERY", "POWERUP_UNLOCK_MISSION_CLOVERBELL_BATTERY", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GeneralModCharm_CrystalSphere).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GeneralModCharm_CrystalSphere, PowerupScript.Archetype.generalModifierCharm, false, -1, 0.2f, 3, -1L, "POWERUP_NAME_CRYSTAL_SPHERE", "POWERUP_DESCR_CRYSTAL_SPHERE", "POWERUP_UNLOCK_MISSION_CRYSTAL_SPHERE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GoldenKingMida).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GoldenKingMida, PowerupScript.Archetype.commonModPuppets, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_GOLDEN_KING_MIDA", "POWERUP_DESCR_GOLDEN_KING_MIDA", "POWERUP_UNLOCK_MISSION_GOLDEN_KING_MIDA", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_C_Dealer).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_C_Dealer, PowerupScript.Archetype.commonModPuppets, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_BOARDGAME_C_DELAER", "POWERUP_DESCR_BOARDGAME_C_DEALER", "POWERUP_UNLOCK_MISSION_BOARDGAME_C_DEALER", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Dealer), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_Dealer), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.PuppetPersonalTrainer).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.PuppetPersonalTrainer, PowerupScript.Archetype.commonModPuppets, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_COMMON_PUPPET_PERSONAL_TRAINER", "POWERUP_DESCR_COMMON_PUPPET_PERSONAL_TRAINER", "POWERUP_UNLOCK_MISSION_COMMON_PUPPET_PERSONAL_TRAINER", null, null, null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_PuppetPersonalTrainer));
		PowerupScript.Spawn(PowerupScript.Identifier.PuppetElectrician).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.PuppetElectrician, PowerupScript.Archetype.commonModPuppets, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_COMMON_PUPPET_ELETRICIAN", "POWERUP_DESCR_COMMON_PUPPET_ELETRICIAN", "POWERUP_UNLOCK_MISSION_COMMON_PUPPET_ELETRICIAN", null, null, null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_PuppetElectrician));
		PowerupScript.Spawn(PowerupScript.Identifier.PuppetFortuneTeller).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.PuppetFortuneTeller, PowerupScript.Archetype.commonModPuppets, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_COMMON_PUPPET_FORTUNE_TELLER", "POWERUP_DESCR_COMMON_PUPPET_FORTUNE_TELLER", "POWERUP_UNLOCK_MISSION_COMMON_PUPPET_FORTUNE_TELLER", null, null, null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_PuppetFortuneTeller));
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_M_Capitalist).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_M_Capitalist, PowerupScript.Archetype.commonModPuppets, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_BOARDGAME_M_CAPITALIST", "POWERUP_DESCR_BOARDGAME_M_CAPITALIST", "POWERUP_UNLOCK_MISSION_BOARDGAME_M_CAPITALIST", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GoldenSymbol_Lemon).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GoldenSymbol_Lemon, PowerupScript.Archetype.goldenSymbols, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_LEMON_GOLD", "POWERUP_DESCR_LEMON_GOLD", "POWERUP_UNLOCK_MISSION_NONE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GoldenSymbol_Cherry).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GoldenSymbol_Cherry, PowerupScript.Archetype.goldenSymbols, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_CHERRY_GOLD", "POWERUP_DESCR_CHERRY_GOLD", "POWERUP_UNLOCK_MISSION_NONE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GoldenSymbol_Clover).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GoldenSymbol_Clover, PowerupScript.Archetype.goldenSymbols, false, -1, 0.2f, 3, -1L, "POWERUP_NAME_CLOVER_GOLD", "POWERUP_DESCR_CLOVER_GOLD", "POWERUP_UNLOCK_MISSION_NONE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GoldenSymbol_Bell).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GoldenSymbol_Bell, PowerupScript.Archetype.goldenSymbols, false, -1, 0.2f, 3, -1L, "POWERUP_NAME_BELL_GOLD", "POWERUP_DESCR_BELL_GOLD", "POWERUP_UNLOCK_MISSION_NONE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GoldenSymbol_Diamond).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GoldenSymbol_Diamond, PowerupScript.Archetype.goldenSymbols, false, -1, 0f, 2, -1L, "POWERUP_NAME_DIAMOND_GOLD", "POWERUP_DESCR_DIAMOND_GOLD", "POWERUP_UNLOCK_MISSION_NONE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GoldenSymbol_Treasure).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GoldenSymbol_Treasure, PowerupScript.Archetype.goldenSymbols, false, -1, 0f, 2, -1L, "POWERUP_NAME_COINS_GOLD", "POWERUP_DESCR_COINS_GOLD", "POWERUP_UNLOCK_MISSION_NONE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.GoldenSymbol_Seven).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.GoldenSymbol_Seven, PowerupScript.Archetype.goldenSymbols, false, -1, 0f, 1, -1L, "POWERUP_NAME_SEVEN_GOLD", "POWERUP_DESCR_SEVEN_GOLD", "POWERUP_UNLOCK_MISSION_NONE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_C_Bricks).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_C_Bricks, PowerupScript.Archetype.boardgameC, false, -1, 0f, 1, -1L, "POWERUP_NAME_BOARDGAME_C_BRICKS", "POWERUP_DESCR_BOARDGAME_C_BRICKS", "POWERUP_UNLOCK_MISSION_BOARDGAME_C_BRICKS", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_C_Wood).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_C_Wood, PowerupScript.Archetype.boardgameC, false, -1, 0f, 1, -1L, "POWERUP_NAME_BOARDGAME_C_WOOD", "POWERUP_DESCR_BOARDGAME_C_WOOD", "POWERUP_UNLOCK_MISSION_BOARDGAME_C_WOOD", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_C_Sheep).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_C_Sheep, PowerupScript.Archetype.boardgameC, false, -1, 0.1f, 2, -1L, "POWERUP_NAME_BOARDGAME_C_SHEEP", "POWERUP_DESCR_BOARDGAME_C_SHEEP", "POWERUP_UNLOCK_MISSION_BOARDGAME_C_SHEEP", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_C_Wheat).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_C_Wheat, PowerupScript.Archetype.boardgameC, false, -1, 0.1f, 2, -1L, "POWERUP_NAME_BOARDGAME_C_WHEAT", "POWERUP_DESCR_BOARDGAME_C_WHEAT", "POWERUP_UNLOCK_MISSION_BOARDGAME_C_WHEAT", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_C_Stone).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_C_Stone, PowerupScript.Archetype.boardgameC, false, -1, 0.2f, 2, -1L, "POWERUP_NAME_BOARDGAME_C_STONE", "POWERUP_DESCR_BOARDGAME_C_STONE", "POWERUP_UNLOCK_MISSION_BOARDGAME_C_STONE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_C_Harbor).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_C_Harbor, PowerupScript.Archetype.boardgameC, false, -1, 0.2f, 2, -1L, "POWERUP_NAME_BOARDGAME_C_HARBOR", "POWERUP_DESCR_BOARDGAME_C_HARBOR", "POWERUP_UNLOCK_MISSION_BOARDGAME_C_HARBOR", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_C_Thief).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_C_Thief, PowerupScript.Archetype.boardgameC, false, -1, 0.2f, 2, -1L, "POWERUP_NAME_BOARDGAME_C_THIEF", "POWERUP_DESCR_BOARDGAME_C_THIEF", "POWERUP_UNLOCK_MISSION_BOARDGAME_C_THIEF", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_M_Carriola).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_M_Carriola, PowerupScript.Archetype.boardgameM, false, -1, 0.35f, 4, -1L, "POWERUP_NAME_BOARDGAME_M_CARRIOLA", "POWERUP_DESCR_BOARDGAME_M_CARRIOLA", "POWERUP_UNLOCK_MISSION_BOARDGAME_M_CARRIOLA", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_M_Shoe).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_M_Shoe, PowerupScript.Archetype.boardgameM, false, -1, 0.35f, 4, -1L, "POWERUP_NAME_BOARDGAME_M_SHOE", "POWERUP_DESCR_BOARDGAME_M_SHOE", "POWERUP_UNLOCK_MISSION_BOARDGAME_M_SHOE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_M_Ditale).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_M_Ditale, PowerupScript.Archetype.boardgameM, false, -1, 0f, 2, -1L, "POWERUP_NAME_BOARDGAME_M_DITALE", "POWERUP_DESCR_BOARDGAME_M_DITALE", "POWERUP_UNLOCK_MISSION_BOARDGAME_M_DITALE", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_M_FerroDaStiro).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_M_FerroDaStiro, PowerupScript.Archetype.boardgameM, false, -1, 0f, 2, -1L, "POWERUP_NAME_BOARDGAME_M_FERRO_DA_STIRO", "POWERUP_DESCR_BOARDGAME_M_FERRO_DA_STIRO", "POWERUP_UNLOCK_MISSION_BOARDGAME_M_FERRO_DA_STIRO", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_M_Car).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_M_Car, PowerupScript.Archetype.boardgameM, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_BOARDGAME_M_CAR", "POWERUP_DESCR_BOARDGAME_M_CAR", "POWERUP_UNLOCK_MISSION_BOARDGAME_M_CAR", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_M_Ship).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_M_Ship, PowerupScript.Archetype.boardgameM, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_BOARDGAME_M_SHIP", "POWERUP_DESCR_BOARDGAME_M_SHIP", "POWERUP_UNLOCK_MISSION_BOARDGAME_M_SHIP", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Boardgame_M_Hat).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Boardgame_M_Hat, PowerupScript.Archetype.boardgameM, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_BOARDGAME_M_HAT", "POWERUP_DESCR_BOARDGAME_M_HAT", "POWERUP_UNLOCK_MISSION_BOARDGAME_M_HAT", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.PlayingCard_HeartsAce).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.PlayingCard_HeartsAce, PowerupScript.Archetype.classicPlayingCards, false, -1, 0f, 3, -1L, "POWERUP_NAME_PLAYING_CARD_ACE_HEART", "POWERUP_DESCR_PLAYING_CARD_ACE_HEART", "POWERUP_UNLOCK_MISSION_PLAYING_CARD_ACE_HEART", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_ClassicPlayingCard_AceOfHearts), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_ClassicPlayingCard_AceOfHearts), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.PlayingCard_ClubsAce).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.PlayingCard_ClubsAce, PowerupScript.Archetype.classicPlayingCards, false, -1, 0f, 3, -1L, "POWERUP_NAME_PLAYING_CARD_ACE_CLUBS", "POWERUP_DESCR_PLAYING_CARD_ACE_CLUBS", "POWERUP_UNLOCK_MISSION_PLAYING_CARD_ACE_CLUBS", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.PlayingCard_DiamondsAce).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.PlayingCard_DiamondsAce, PowerupScript.Archetype.classicPlayingCards, false, -1, 0f, 3, -1L, "POWERUP_NAME_PLAYING_CARD_ACE_DIAMONDS", "POWERUP_DESCR_PLAYING_CARD_ACE_DIAMONDS", "POWERUP_UNLOCK_MISSION_PLAYING_CARD_ACE_DIAMONDS", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_ClassicPlayingCard_AceOfDiamonds), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_ClassicPlayingCard_AceOfDiamonds), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.PlayingCard_SpadesAce).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.PlayingCard_SpadesAce, PowerupScript.Archetype.classicPlayingCards, false, -1, 0f, 3, -1L, "POWERUP_NAME_PLAYING_CARD_ACE_SPADES", "POWERUP_DESCR_PLAYING_CARD_ACE_SPADES", "POWERUP_UNLOCK_MISSION_PLAYING_CARD_ACE_SPADES", null, null, null, null);
		PowerupScript.DiceD4_Reset();
		PowerupScript.Spawn(PowerupScript.Identifier.Dice_4).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Dice_4, PowerupScript.Archetype.dices, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_DICE_4", "POWERUP_DESCR_DICE_4", "POWERUP_UNLOCK_MISSION_DICE4", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Dice_6).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Dice_6, PowerupScript.Archetype.dices, false, -1, 0.35f, 2, -1L, "POWERUP_NAME_DICE_6", "POWERUP_DESCR_DICE_6", "POWERUP_UNLOCK_MISSION_DICE6", new PowerupScript.PowerupEvent(PowerupScript.PFunc_Dice6_OnEquipped), new PowerupScript.PowerupEvent(PowerupScript.PFunc_Dice6_OnUnequipped), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Dice_20).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Dice_20, PowerupScript.Archetype.dices, false, -1, 0.35f, 5, -1L, "POWERUP_NAME_DICE_20", "POWERUP_DESCR_DICE_20", "POWERUP_UNLOCK_MISSION_DICE20", new PowerupScript.PowerupEvent(PowerupScript.PFunc_Dice20_OnEquipped), new PowerupScript.PowerupEvent(PowerupScript.PFunc_Dice20_OnUnequipped), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier.Hole_Circle).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Hole_Circle, PowerupScript.Archetype.generic, false, -1, 0.35f, 3, -1L, "POWERUP_NAME_HOLE_CIRCLE", "POWERUP_DESCR_HOLE_CIRCLE", "POWERUP_UNLOCK_HOLE_CIRLCE", null, null, null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_HoleCircle));
		PowerupScript.Spawn(PowerupScript.Identifier.Hole_Romboid).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Hole_Romboid, PowerupScript.Archetype.generic, false, -1, 0.5f, 4, -1L, "POWERUP_NAME_HOLE_ROMBOID", "POWERUP_DESCR_HOLE_ROMBOID", "POWERUP_UNLOCK_HOLE_ROMBOID", null, null, null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_HoleRomboid));
		PowerupScript.Spawn(PowerupScript.Identifier.Hole_Cross).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier.Hole_Cross, PowerupScript.Archetype.generic, false, -1, 0.5f, 6, -1L, "POWERUP_NAME_HOLE_CROSS", "POWERUP_DESCR_HOLE_CROSS", "POWERUP_UNLOCK_HOLE_CROSS", null, null, null, new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnThrowAway_HoleCross));
		PowerupScript.AngelHand_RemovePatterns();
		PowerupScript.Spawn(PowerupScript.Identifier._999_AngelHand).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier._999_AngelHand, PowerupScript.Archetype.sacred, false, 0, 0.65f, 0, -1L, "POWERUP_NAME_ANGEL_HAND", "POWERUP_DESCR_ANGEL_HAND", "POWERUP_UNLOCK_MISSION_ALL_SACRED_CHARMS", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_AngelHand), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_AngelHand), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier._999_EyeOfGod).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier._999_EyeOfGod, PowerupScript.Archetype.sacred, false, 0, 0.65f, 0, -1L, "POWERUP_NAME_EYE_OF_GOD", "POWERUP_DESCR_EYE_OF_GOD", "POWERUP_UNLOCK_MISSION_ALL_SACRED_CHARMS", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_EyeOfGod), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_EyeOfGod), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier._999_HolySpirit).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier._999_HolySpirit, PowerupScript.Archetype.sacred, false, 0, 0.65f, 0, -1L, "POWERUP_NAME_HOLY_SPIRIT", "POWERUP_DESCR_HOLY_SPIRIT", "POWERUP_UNLOCK_MISSION_ALL_SACRED_CHARMS", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_HolySpirit), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_HolySpirit), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier._999_SacredHeart).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier._999_SacredHeart, PowerupScript.Archetype.sacred, false, 0, 0.65f, 0, -1L, "POWERUP_NAME_SACRED_HEART", "POWERUP_DESCR_SACRED_HEART", "POWERUP_UNLOCK_MISSION_ALL_SACRED_CHARMS", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier._999_Aureola).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier._999_Aureola, PowerupScript.Archetype.sacred, false, 0, 0.65f, 0, -1L, "POWERUP_NAME_HALO", "POWERUP_DESCR_HALO", "POWERUP_UNLOCK_MISSION_ALL_SACRED_CHARMS", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier._999_TheBlood).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier._999_TheBlood, PowerupScript.Archetype.sacred, false, 0, 0.65f, 0, -1L, "POWERUP_NAME_THE_BLOOD", "POWERUP_DESCR_THE_BLOOD", "POWERUP_UNLOCK_MISSION_ALL_SACRED_CHARMS", null, null, null, null);
		PowerupScript.Spawn(PowerupScript.Identifier._999_TheBody).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier._999_TheBody, PowerupScript.Archetype.sacred, false, 0, 0.65f, 0, -1L, "POWERUP_NAME_THE_BODY", "POWERUP_DESCR_THE_BODY", "POWERUP_UNLOCK_MISSION_ALL_SACRED_CHARMS", null, null, null, null);
		PowerupScript.EternityReset();
		PowerupScript.Spawn(PowerupScript.Identifier._999_Eternity).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier._999_Eternity, PowerupScript.Archetype.sacred, false, 0, 0.65f, 0, -1L, "POWERUP_NAME_ETERNITY", "POWERUP_DESCR_ETERNITY", "POWERUP_UNLOCK_MISSION_ALL_SACRED_CHARMS", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_Eternity), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_Eternity), null, null);
		PowerupScript.AdamsRibCageReset();
		PowerupScript.Spawn(PowerupScript.Identifier._999_AdamsRibcage).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier._999_AdamsRibcage, PowerupScript.Archetype.sacred, false, 0, 0.65f, 0, -1L, "POWERUP_NAME_ADAMS_RIBCAGE", "POWERUP_DESCR_ADAMS_RIBCAGE", "POWERUP_UNLOCK_MISSION_ALL_SACRED_CHARMS", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_AdamsRibCage), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_AdamsRibCage), null, null);
		PowerupScript.Spawn(PowerupScript.Identifier._999_OphanimWheels).Initialize(isNewGame, PowerupScript.Category.normal, PowerupScript.Identifier._999_OphanimWheels, PowerupScript.Archetype.sacred, false, 0, 0.65f, 0, -1L, "POWERUP_NAME_OPHANIM_WHEELS", "POWERUP_DESCR_OPHANIM_WHEELS", "POWERUP_UNLOCK_MISSION_ALL_SACRED_CHARMS", new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnEquip_OphanimWheels), new PowerupScript.PowerupEvent(PowerupScript.PFunc_OnUnequip_OphanimWheels), null, null);
		for (int l = 0; l < instance.equippedPowerups.Length; l++)
		{
			PowerupScript.Identifier identifier2;
			if ((identifier2 = PlatformDataMaster.EnumEntryFromString<PowerupScript.Identifier>(instance.equippedPowerups[l], PowerupScript.Identifier.undefined)) >= PowerupScript.Identifier.Skeleton_Arm1 && identifier2 != PowerupScript.Identifier.undefined && identifier2 != PowerupScript.Identifier.count)
			{
				PowerupScript.EquipFlag_IgnoreSpaceCondition();
				PowerupScript.Equip(identifier2, true, true);
			}
		}
		for (int m = 0; m < instance.equippedPowerups_Skeleton.Length; m++)
		{
			PowerupScript.Identifier identifier3;
			if ((identifier3 = PlatformDataMaster.EnumEntryFromString<PowerupScript.Identifier>(instance.equippedPowerups_Skeleton[m], PowerupScript.Identifier.undefined)) >= PowerupScript.Identifier.Skeleton_Arm1 && identifier3 != PowerupScript.Identifier.undefined && identifier3 != PowerupScript.Identifier.count)
			{
				PowerupScript.Equip(identifier3, true, true);
			}
		}
		for (int n = 0; n < 4; n++)
		{
			PowerupScript.Identifier identifier4;
			if ((identifier4 = PlatformDataMaster.EnumEntryFromString<PowerupScript.Identifier>(GameplayData.Instance.drawerPowerups[n], PowerupScript.Identifier.undefined)) < PowerupScript.Identifier.Skeleton_Arm1 || identifier4 == PowerupScript.Identifier.undefined || identifier4 == PowerupScript.Identifier.count)
			{
				PowerupScript.array_InDrawer[n] = null;
			}
			else
			{
				PowerupScript.PutInDrawer(identifier4, true, n);
			}
		}
		for (int num2 = PowerupScript._initializationTempList.Count - 1; num2 >= 0; num2--)
		{
			PowerupScript.ThrowAway(PowerupScript._initializationTempList[num2].identifier, true);
		}
		if (placePowerups)
		{
			PowerupScript.RefreshPlacementAll();
		}
		if (PowerupScript._initializationTempList.Count > 0)
		{
			Debug.LogError("PowerupScript: InitializeAll: Temp list not cleared! Have you forgot to initialize some powerup?? Count: " + PowerupScript._initializationTempList.Count.ToString());
		}
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x000291F0 File Offset: 0x000273F0
	private void Awake()
	{
		PowerupScript.all.Add(this);
		this.myOutline = base.GetComponent<Outline>();
		this.meshFilter = this.meshHolder.GetComponentInChildren<MeshFilter>(true);
		this.meshRenderer = this.meshHolder.GetComponentInChildren<MeshRenderer>(true);
		this.skinnedMeshRenderer = this.meshHolder.GetComponentInChildren<SkinnedMeshRenderer>(true);
		if (this.meshRenderer != null)
		{
			this.materialDefault = this.meshRenderer.sharedMaterial;
			this.animator_IfAny = this.meshRenderer.GetComponentInChildren<Animator>();
		}
		else if (this.skinnedMeshRenderer != null)
		{
			this.materialDefault = this.skinnedMeshRenderer.sharedMaterial;
			this.animator_IfAny = this.skinnedMeshRenderer.transform.parent.GetComponentInChildren<Animator>();
		}
		if (this.diegeticMenuElement == null)
		{
			this.diegeticMenuElement = base.gameObject.AddComponent<DiegeticMenuElement>();
		}
		this.diegeticMenuElement.SetMyController(DiegeticMenuController.MainMenu);
		this.diegeticMenuElement.myOutline = this.myOutline;
		this.diegeticMenuElement.rainbowOutline = true;
		this.diegeticMenuElement.soundOnHover = AssetMaster.GetSound("SoundMenuSelectionChange");
		this.diegeticMenuElement.enabled = false;
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x00029324 File Offset: 0x00027524
	private void Start()
	{
		if (this.category == PowerupScript.Category.undefined)
		{
			Debug.LogError("PowerupScript: Category is undefined! GameObject: " + base.gameObject.name);
		}
		if (this.category == PowerupScript.Category.count)
		{
			Debug.LogError("PowerupScript: Category is count! GameObject: " + base.gameObject.name);
		}
		if (this.identifier == PowerupScript.Identifier.undefined)
		{
			Debug.LogError("PowerupScript: Kind is undefined! GameObject: " + base.gameObject.name);
		}
		if (this.identifier == PowerupScript.Identifier.count)
		{
			Debug.LogError("PowerupScript: Kind is count! GameObject: " + base.gameObject.name);
		}
		if (this.archetype == PowerupScript.Archetype.undefined)
		{
			Debug.LogError("PowerupScript: Archetype is undefined! GameObject: " + base.gameObject.name);
		}
		if (this.archetype == PowerupScript.Archetype.count)
		{
			Debug.LogError("PowerupScript: Archetype is count! GameObject: " + base.gameObject.name);
		}
		if (string.IsNullOrEmpty(this.nameKey))
		{
			Debug.LogError("PowerupScript: nameKey is null or empty! GameObject: " + base.gameObject.name);
		}
		if (string.IsNullOrEmpty(this.descriptionKey))
		{
			Debug.LogError("PowerupScript: descriptionKey is null or empty! GameObject: " + base.gameObject.name);
		}
		if (this.meshHolder.transform.localPosition != Vector3.zero)
		{
			Debug.LogError("PowerupScript: Mesh holder is not at zero local position! This will cause problems with 'triggered animations' GameObject: " + base.gameObject.name);
		}
		this.updateTimer = Random.value;
		this.glowHolder.SetActive(false);
		this.sacredGlowHolder.SetActive(this.equippedChached && this.archetype == PowerupScript.Archetype.sacred);
		this.redButtonUiHolder.SetActive(false);
		this.RedButtonText_PositionInit();
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x000294D5 File Offset: 0x000276D5
	private void OnDestroy()
	{
		PowerupScript.all.Remove(this);
		this._Uninitialize();
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x000294EC File Offset: 0x000276EC
	private void Update()
	{
		if (!Tick.IsGameRunning)
		{
			return;
		}
		if (!PowerupScript.IsEquipped_Quick(this.identifier))
		{
			this.updateTimer -= Tick.Time;
			if (this.updateTimer > 0f)
			{
				return;
			}
			this.updateTimer = 1f;
		}
		bool flag = !this.myOutline.enabled;
		if (this.modHolder_OutlineSwitcher.gameObject.activeSelf != flag)
		{
			this.modHolder_OutlineSwitcher.gameObject.SetActive(flag);
		}
		if (this.glowHolder.activeSelf)
		{
			this.glowTransform.SetYAngle(CameraGame.firstInstance.transform.eulerAngles.y);
			for (int i = 0; i < this.glowSprites.Length; i++)
			{
				Color color = this.glowSprites[i].color;
				color.a = 0.1f + 0.06f * Util.AngleSin(Tick.PassedTime * 360f);
				this.glowSprites[i].color = color;
			}
			bool flag2 = !TerminalScript.IsLoggedIn();
			if (this.glowTransform.gameObject.activeSelf != flag2)
			{
				this.glowTransform.gameObject.SetActive(flag2);
			}
			this.justEquippedGlowTimer -= Tick.Time;
			if (this.justEquippedGlowTimer <= 0f || this.diegeticMenuElement.IsHovered())
			{
				this.justEquippedGlowTimer = 0f;
				this.glowHolder.transform.localScale = Vector3.Lerp(this.glowHolder.transform.localScale, Vector3.zero, Tick.Time * 10f);
				if (this.glowHolder.transform.localScale.x < 0.1f)
				{
					this.glowHolder.SetActive(false);
				}
			}
		}
		if (this.sacredGlowHolder.activeSelf)
		{
			this.sacredGlowTransform.SetLocalXAngle(0f);
			this.sacredGlowTransform.SetLocalZAngle(0f);
			this.sacredGlowTransform.SetYAngle(CameraGame.firstInstance.transform.eulerAngles.y);
			for (int j = 0; j < this.sacredGlowSprites.Length; j++)
			{
				Color color2 = this.sacredGlowSprites[j].color;
				color2.a = 0.1f + 0.06f * Util.AngleSin(Tick.PassedTime * 360f);
				this.sacredGlowSprites[j].color = color2;
			}
			bool flag3 = !TerminalScript.IsLoggedIn() && !this.diegeticMenuElement.IsHovered();
			if (this.sacredGlowTransform.gameObject.activeSelf != flag3)
			{
				this.sacredGlowTransform.gameObject.SetActive(flag3);
			}
		}
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x0002979C File Offset: 0x0002799C
	public static void UpdateAllVisibles()
	{
		for (int i = 0; i < PowerupScript.list_EquippedNormal.Count; i++)
		{
			PowerupScript.list_EquippedNormal[i].UpdateVisible();
		}
		for (int j = 0; j < PowerupScript.list_EquippedSkeleton.Count; j++)
		{
			PowerupScript.list_EquippedSkeleton[j].UpdateVisible();
		}
		for (int k = 0; k < StoreCapsuleScript.storePowerups.Length; k++)
		{
			if (!(StoreCapsuleScript.storePowerups[k] == null))
			{
				StoreCapsuleScript.storePowerups[k].UpdateVisible();
			}
		}
		for (int l = 0; l < PowerupScript.array_InDrawer.Length; l++)
		{
			if (!(PowerupScript.array_InDrawer[l] == null))
			{
				PowerupScript.array_InDrawer[l].UpdateVisible();
			}
		}
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x00029850 File Offset: 0x00027A50
	public void UpdateVisible()
	{
		this.MaterialRefresh();
		if (this.IsRedButtonCharm())
		{
			bool flag = PowerupScript.IsEquipped_Quick(this.identifier) && !InspectorScript.IsEnabled();
			if (this.redButtonUiHolder.activeSelf != flag)
			{
				this.redButtonUiHolder.SetActive(flag);
			}
			if (this._redButton_TextUpdateRequest)
			{
				this._redButton_TextUpdateRequest = false;
				this.redButtonUiText.text = this._redButton_TextString;
				this.redButtonUiText.ForceMeshUpdate(false, false);
			}
			this.redButtonUiHolder.transform.eulerAngles = new Vector3(0f, CameraGame.firstInstance.transform.eulerAngles.y, 0f);
			this.redButtonUiCanvas.transform.SetXAngle(CameraGame.firstInstance.transform.eulerAngles.x);
		}
	}

	public static List<PowerupScript> all = new List<PowerupScript>();

	public static List<PowerupScript> list_NotBought = new List<PowerupScript>();

	public static List<PowerupScript> list_EquippedSkeleton = new List<PowerupScript>();

	public static List<PowerupScript> list_EquippedNormal = new List<PowerupScript>();

	public static PowerupScript[] array_InDrawer = new PowerupScript[4];

	public static Dictionary<PowerupScript.Identifier, bool> dict_IsLocked = new Dictionary<PowerupScript.Identifier, bool>();

	private static Dictionary<PowerupScript.Identifier, string> dict_IdentifierToPrefabName = new Dictionary<PowerupScript.Identifier, string>
	{
		{
			PowerupScript.Identifier.Skeleton_Arm1,
			"Powerup Skeleton Arm1"
		},
		{
			PowerupScript.Identifier.Skeleton_Arm2,
			"Powerup Skeleton Arm2"
		},
		{
			PowerupScript.Identifier.Skeleton_Leg1,
			"Powerup Skeleton Leg1"
		},
		{
			PowerupScript.Identifier.Skeleton_Leg2,
			"Powerup Skeleton Leg2"
		},
		{
			PowerupScript.Identifier.Skeleton_Head,
			"Powerup Skeleton Head"
		},
		{
			PowerupScript.Identifier.HamsaInverted,
			"Powerup Inverted Hamsa"
		},
		{
			PowerupScript.Identifier.TarotDeck,
			"Powerup Tarot Deck"
		},
		{
			PowerupScript.Identifier.Ankh,
			"Powerup Ankh"
		},
		{
			PowerupScript.Identifier.LuckyCat,
			"Powerup Lucky Cat"
		},
		{
			PowerupScript.Identifier.LuckyCatFat,
			"Powerup Chonky Cat"
		},
		{
			PowerupScript.Identifier.LuckyCatSwole,
			"Powerup Swole Cat"
		},
		{
			PowerupScript.Identifier.FakeCoin,
			"Powerup Fake Coin"
		},
		{
			PowerupScript.Identifier.HorseShoe,
			"Powerup HorseShoe"
		},
		{
			PowerupScript.Identifier.HorseShoeGold,
			"Powerup HorseShoe Gold"
		},
		{
			PowerupScript.Identifier.OneTrickPony,
			"Powerup 1 Trick Pony"
		},
		{
			PowerupScript.Identifier.RedCrystal,
			"Powerup Red Crystal"
		},
		{
			PowerupScript.Identifier.PoopBeetle,
			"Powerup Stercoraro"
		},
		{
			PowerupScript.Identifier.GrandmasPurse,
			"Powerup Grandma's Purse"
		},
		{
			PowerupScript.Identifier.Pentacle,
			"Powerup Pentacle"
		},
		{
			PowerupScript.Identifier.HouseContract,
			"Powerup House Contract"
		},
		{
			PowerupScript.Identifier.CatTreats,
			"Powerup Cat Treats"
		},
		{
			PowerupScript.Identifier.CloverPet,
			"Powerup CloverPet"
		},
		{
			PowerupScript.Identifier.Button2X,
			"Powerup 2xButton"
		},
		{
			PowerupScript.Identifier.DearDiary,
			"Powerup Dear Diary"
		},
		{
			PowerupScript.Identifier.Megaphone,
			"Powerup Megaphone"
		},
		{
			PowerupScript.Identifier.PhotoBook,
			"Powerup Photo Book"
		},
		{
			PowerupScript.Identifier.SuperCapacitor,
			"Powerup Super Capacitor"
		},
		{
			PowerupScript.Identifier.CrankGenerator,
			"Powerup Crank Generator"
		},
		{
			PowerupScript.Identifier.GrattaEVinci_ScratchAndWin,
			"Powerup Gratta e Vinci"
		},
		{
			PowerupScript.Identifier.ToyTrain,
			"Powerup Toy Train"
		},
		{
			PowerupScript.Identifier.Stonks,
			"Powerup Stonks"
		},
		{
			PowerupScript.Identifier.CarBattery,
			"Powerup Car Battery"
		},
		{
			PowerupScript.Identifier.CloverVoucher,
			"Powerup Clover Voucher"
		},
		{
			PowerupScript.Identifier.ExpiredMedicines,
			"Powerup Expired Medicines"
		},
		{
			PowerupScript.Identifier.GoldenHand_MidasTouch,
			"Powerup Golden Hand"
		},
		{
			PowerupScript.Identifier.PissJar,
			"Powerup Piss Jar"
		},
		{
			PowerupScript.Identifier.PoopJar,
			"Powerup Poop Jar"
		},
		{
			PowerupScript.Identifier.Painkillers,
			"Powerup Pain Killers"
		},
		{
			PowerupScript.Identifier.Wallet,
			"Powerup Wallet"
		},
		{
			PowerupScript.Identifier.MoneyBriefCase,
			"Powerup Brief Case"
		},
		{
			PowerupScript.Identifier.Calendar,
			"Powerup Calendar"
		},
		{
			PowerupScript.Identifier.YellowStar,
			"Powerup Yellow Star"
		},
		{
			PowerupScript.Identifier.Wolf,
			"Powerup Wolf"
		},
		{
			PowerupScript.Identifier.FortuneCookie,
			"Powerup Fortune Cookie"
		},
		{
			PowerupScript.Identifier.VineSoupShroom,
			"Powerup Vine Soup"
		},
		{
			PowerupScript.Identifier.GiantShroom,
			"Powerup Giant Shroom"
		},
		{
			PowerupScript.Identifier.FideltyCard,
			"Powerup Fidelty Card"
		},
		{
			PowerupScript.Identifier.Sardines,
			"Powerup Sardines"
		},
		{
			PowerupScript.Identifier.RottenPepper,
			"Powerup Rotten Pepper"
		},
		{
			PowerupScript.Identifier.BellPepper,
			"Powerup Bell Pepper"
		},
		{
			PowerupScript.Identifier.AncientCoin,
			"Powerup Ancient Coin"
		},
		{
			PowerupScript.Identifier.BrokenCalculator,
			"Powerup Broken Calculator"
		},
		{
			PowerupScript.Identifier.HamsaUpside,
			"Powerup Hamsa Upside"
		},
		{
			PowerupScript.Identifier.TheCollector,
			"Powerup The Collector"
		},
		{
			PowerupScript.Identifier.FortuneChanneler,
			"Powerup Fortune Channeler"
		},
		{
			PowerupScript.Identifier.Nose,
			"Powerup Nose"
		},
		{
			PowerupScript.Identifier.EyeJar,
			"Powerup Eye Jar"
		},
		{
			PowerupScript.Identifier.EvilDeal,
			"Powerup Evil Deal"
		},
		{
			PowerupScript.Identifier.ChastityBelt,
			"Powerup Castity Belt"
		},
		{
			PowerupScript.Identifier.VoiceMailTape,
			"Powerup VoiceMail Tape"
		},
		{
			PowerupScript.Identifier.Garbage,
			"Powerup Garbage"
		},
		{
			PowerupScript.Identifier.AllIn,
			"Powerup AllIn"
		},
		{
			PowerupScript.Identifier.RingBell,
			"Powerup Ring Bell"
		},
		{
			PowerupScript.Identifier.ConsolationPrize,
			"Powerup Consolation Prize"
		},
		{
			PowerupScript.Identifier.DarkLotus,
			"Powerup Dark Lotus"
		},
		{
			PowerupScript.Identifier.StepsCounter,
			"Powerup Steps Counter"
		},
		{
			PowerupScript.Identifier.Depression,
			"Powerup Depression"
		},
		{
			PowerupScript.Identifier.WeirdClock,
			"Powerup Weird Clock"
		},
		{
			PowerupScript.Identifier.MusicTape,
			"Powerup Music Tape"
		},
		{
			PowerupScript.Identifier.Jimbo,
			"Powerup Jimbo"
		},
		{
			PowerupScript.Identifier.DiscA,
			"Powerup Disc A"
		},
		{
			PowerupScript.Identifier.DiscB,
			"Powerup Disc B"
		},
		{
			PowerupScript.Identifier.DiscC,
			"Powerup Disc C"
		},
		{
			PowerupScript.Identifier.LocomotiveDiesel,
			"Powerup Locomotive Diesel"
		},
		{
			PowerupScript.Identifier.LocomotiveSteam,
			"Powerup Locomotive Steam"
		},
		{
			PowerupScript.Identifier.CloversLandPatch,
			"Powerup Clovers Land Patch"
		},
		{
			PowerupScript.Identifier.AbstractPainting,
			"Powerup Abstract Painting"
		},
		{
			PowerupScript.Identifier.Pareidolia,
			"Powerup Pareidolia"
		},
		{
			PowerupScript.Identifier.ShoppingCart,
			"Powerup Shopping Kart"
		},
		{
			PowerupScript.Identifier.CrowBar,
			"Powerup CrowBar"
		},
		{
			PowerupScript.Identifier.CardboardHouse,
			"Powerup Cardboard House"
		},
		{
			PowerupScript.Identifier.Cigarettes,
			"Powerup Cigarettes"
		},
		{
			PowerupScript.Identifier.ElectricityCounter,
			"Powerup Electricity Meter"
		},
		{
			PowerupScript.Identifier.PotatoPower,
			"Powerup Potato Power"
		},
		{
			PowerupScript.Identifier.Mushrooms,
			"Powerup Mushrooms"
		},
		{
			PowerupScript.Identifier.Rorschach,
			"Powerup Rorschach"
		},
		{
			PowerupScript.Identifier.CloverPot,
			"Powerup CloverPot"
		},
		{
			PowerupScript.Identifier.Hole_Circle,
			"Powerup Hole Circle"
		},
		{
			PowerupScript.Identifier.Hole_Romboid,
			"Powerup Hole Romboid"
		},
		{
			PowerupScript.Identifier.Hole_Cross,
			"Powerup Hole Cross"
		},
		{
			PowerupScript.Identifier.Hourglass,
			"Powerup Hourglass"
		},
		{
			PowerupScript.Identifier.FruitBasket,
			"Powerup Fruit Basket"
		},
		{
			PowerupScript.Identifier.SevenSinsStone,
			"Powerup Seven Sins Stone"
		},
		{
			PowerupScript.Identifier.Necklace,
			"Powerup Necklace"
		},
		{
			PowerupScript.Identifier.CloverBell,
			"Powerup Clover Bell"
		},
		{
			PowerupScript.Identifier.HornChilyRed,
			"Powerup Horn Chily Red"
		},
		{
			PowerupScript.Identifier.HornChilyGreen,
			"Powerup Horn Chily Green"
		},
		{
			PowerupScript.Identifier.GoldenPepper,
			"Powerup Golden Pepper"
		},
		{
			PowerupScript.Identifier.HornDevil,
			"Powerup Devil Horn"
		},
		{
			PowerupScript.Identifier.Necronomicon,
			"Powerup Necronomicon"
		},
		{
			PowerupScript.Identifier.HolyBible,
			"Powerup Holy Bible"
		},
		{
			PowerupScript.Identifier.Baphomet,
			"Powerup Baphomet"
		},
		{
			PowerupScript.Identifier.Cross,
			"Powerup Cross"
		},
		{
			PowerupScript.Identifier.Rosary,
			"Powerup Rosary"
		},
		{
			PowerupScript.Identifier.BookOfShadows,
			"Powerup Book Of Shadows"
		},
		{
			PowerupScript.Identifier.Gabibbh,
			"Powerup Gabibbh"
		},
		{
			PowerupScript.Identifier.PossessedPhone,
			"Powerup Possessed Phone"
		},
		{
			PowerupScript.Identifier.MysticalTomato,
			"Powerup Mystical Tomato"
		},
		{
			PowerupScript.Identifier.RitualBell,
			"Powerup Ritual Bell"
		},
		{
			PowerupScript.Identifier.CrystalSkull,
			"Powerup Crystal Skull"
		},
		{
			PowerupScript.Identifier.SymbolInstant_Lemon,
			"Powerup Instant Lemon"
		},
		{
			PowerupScript.Identifier.SymbolInstant_Cherry,
			"Powerup Instant Cherry"
		},
		{
			PowerupScript.Identifier.SymbolInstant_Clover,
			"Powerup Instant Clover"
		},
		{
			PowerupScript.Identifier.SymbolInstant_Bell,
			"Powerup Instant Bell"
		},
		{
			PowerupScript.Identifier.SymbolInstant_Diamond,
			"Powerup Instant Diamond"
		},
		{
			PowerupScript.Identifier.SymbolInstant_Treasure,
			"Powerup Instant Treasure"
		},
		{
			PowerupScript.Identifier.SymbolInstant_Seven,
			"Powerup Instant Seven"
		},
		{
			PowerupScript.Identifier.GeneralModCharm_Clicker,
			"Powerup Clicker"
		},
		{
			PowerupScript.Identifier.GeneralModCharm_CloverBellBattery,
			"Powerup CloverBell Battery"
		},
		{
			PowerupScript.Identifier.GeneralModCharm_CrystalSphere,
			"Powerup Crystal Ball"
		},
		{
			PowerupScript.Identifier.GoldenKingMida,
			"Powerup Golden King Mida"
		},
		{
			PowerupScript.Identifier.Boardgame_C_Dealer,
			"Powerup Boardgame C Dealer"
		},
		{
			PowerupScript.Identifier.Boardgame_M_Capitalist,
			"Powerup Boardgame M Capitalist"
		},
		{
			PowerupScript.Identifier.PuppetPersonalTrainer,
			"Powerup Personal Trainer"
		},
		{
			PowerupScript.Identifier.PuppetElectrician,
			"Powerup Eletrician"
		},
		{
			PowerupScript.Identifier.PuppetFortuneTeller,
			"Powerup Fortune Teller"
		},
		{
			PowerupScript.Identifier.GoldenSymbol_Lemon,
			"Powerup Gold Lemon"
		},
		{
			PowerupScript.Identifier.GoldenSymbol_Cherry,
			"Powerup Gold Cherry"
		},
		{
			PowerupScript.Identifier.GoldenSymbol_Clover,
			"Powerup Gold Clover"
		},
		{
			PowerupScript.Identifier.GoldenSymbol_Bell,
			"Powerup Gold Bell"
		},
		{
			PowerupScript.Identifier.GoldenSymbol_Diamond,
			"Powerup Gold Diamond"
		},
		{
			PowerupScript.Identifier.GoldenSymbol_Treasure,
			"Powerup Gold Coins"
		},
		{
			PowerupScript.Identifier.GoldenSymbol_Seven,
			"Powerup Gold Seven"
		},
		{
			PowerupScript.Identifier.Boardgame_C_Thief,
			"Powerup Boardgame C Thief"
		},
		{
			PowerupScript.Identifier.Boardgame_C_Harbor,
			"Powerup Boardgame C Harbor"
		},
		{
			PowerupScript.Identifier.Boardgame_C_Wheat,
			"Powerup Boardgame C Wheat"
		},
		{
			PowerupScript.Identifier.Boardgame_C_Sheep,
			"Powerup Boardgame C Sheep"
		},
		{
			PowerupScript.Identifier.Boardgame_C_Stone,
			"Powerup Boardgame C Stones"
		},
		{
			PowerupScript.Identifier.Boardgame_C_Wood,
			"Powerup Boardgame C Wood"
		},
		{
			PowerupScript.Identifier.Boardgame_C_Bricks,
			"Powerup Boardgame C Bricks"
		},
		{
			PowerupScript.Identifier.Boardgame_M_Carriola,
			"Powerup Boardgame M Carriola"
		},
		{
			PowerupScript.Identifier.Boardgame_M_Shoe,
			"Powerup Boardgame M Shoe"
		},
		{
			PowerupScript.Identifier.Boardgame_M_Ditale,
			"Powerup Boardgame M Ditale"
		},
		{
			PowerupScript.Identifier.Boardgame_M_FerroDaStiro,
			"Powerup Boardgame M Ferro Da Stiro"
		},
		{
			PowerupScript.Identifier.Boardgame_M_Car,
			"Powerup Boardgame M Car"
		},
		{
			PowerupScript.Identifier.Boardgame_M_Ship,
			"Powerup Boardgame M Ship"
		},
		{
			PowerupScript.Identifier.Boardgame_M_Hat,
			"Powerup Boardgame M Hat"
		},
		{
			PowerupScript.Identifier.PlayingCard_HeartsAce,
			"Powerup Ace Of Hearts"
		},
		{
			PowerupScript.Identifier.PlayingCard_ClubsAce,
			"Powerup Ace Of Clubs"
		},
		{
			PowerupScript.Identifier.PlayingCard_DiamondsAce,
			"Powerup Ace Of Diamonds"
		},
		{
			PowerupScript.Identifier.PlayingCard_SpadesAce,
			"Powerup Ace Of Spades"
		},
		{
			PowerupScript.Identifier.Dice_4,
			"Powerup Dice 4"
		},
		{
			PowerupScript.Identifier.Dice_6,
			"Powerup Dice 6"
		},
		{
			PowerupScript.Identifier.Dice_20,
			"Powerup Dice 20"
		},
		{
			PowerupScript.Identifier._999_AngelHand,
			"Powerup Angel Hand"
		},
		{
			PowerupScript.Identifier._999_EyeOfGod,
			"Powerup Eye Of God"
		},
		{
			PowerupScript.Identifier._999_HolySpirit,
			"Powerup Holy Spirit"
		},
		{
			PowerupScript.Identifier._999_SacredHeart,
			"Powerup Sacred Heart"
		},
		{
			PowerupScript.Identifier._999_Aureola,
			"Powerup Aureola"
		},
		{
			PowerupScript.Identifier._999_TheBlood,
			"Powerup The Blood"
		},
		{
			PowerupScript.Identifier._999_TheBody,
			"Powerup The Body"
		},
		{
			PowerupScript.Identifier._999_Eternity,
			"Powerup Eternity"
		},
		{
			PowerupScript.Identifier._999_AdamsRibcage,
			"Powerup Adams Ribcage"
		},
		{
			PowerupScript.Identifier._999_OphanimWheels,
			"Powerup Ophanim Wheels"
		}
	};

	private static Dictionary<PowerupScript.Identifier, PowerupScript> dict_IdentifierToInstance = new Dictionary<PowerupScript.Identifier, PowerupScript>();

	private const int PLAYER_INDEX = 0;

	public const float FAR_AWAY_Z = 25f;

	public const bool ALLOW_SKELETON_PIECES_INSPECTION = true;

	public const float STORE_REROLL_CHANCE_COMMON = 0f;

	public const float STORE_REROLL_CHANCE_UNCOMMON = 0.1f;

	public const float STORE_REROLL_CHANCE_MILDLY_RARE = 0.2f;

	public const float STORE_REROLL_CHANCE_RARE = 0.35f;

	public const float STORE_REROLL_CHANCE_VERY_RARE = 0.5f;

	public const float STORE_REROLL_CHANCE_LEGENDARY = 0.65f;

	public const float STORE_REROLL_CHANCE_LEGENDARY_ULTRA = 0.8f;

	public const int PRICE_FREE = 0;

	public const int PRICE_CHEAPEST = 1;

	public const int PRICE_CHEAP = 2;

	public const int PRICE_NORMAL = 3;

	public const int PRICE_NORMAL_HIGH = 4;

	public const int PRICE_HIGH = 5;

	public const int PRICE_HIGH_HIGH = 6;

	public const int PRICE_REALLY_HIGH = 7;

	public const int PRICE_REALLY_HIGH_HIGH = 8;

	public const int PRICE_INCREDIBLY_HIGH = 9;

	public const int PRICE_INCREDIBLY_HIGH_HIGH = 10;

	public const int PRICE_OVERLY_HIGH = 15;

	public const int PRICE_OVERLY_HIGH_HIGH = 25;

	public const int PRICE_UNREACHABLE = 50;

	public const int PRICE_UNREACHABLE_HIGH = 100;

	public const int PRICE_UNREACHABLE_HIGH_HIGH = 500;

	public const int PRICE_IMPOSSIBLE = 1000;

	private const long UNLOCK_PRICE_NONE = -1L;

	private const string UNLOCK_MISSION_NONE = "POWERUP_UNLOCK_MISSION_NONE";

	private const string UNLOCK_MISSION_BUY_TERMINAL = "POWERUP_UNLOCK_MISSION_BUY_FROM_THE_TERMINAL";

	public const string TEMP_LUCK_TAG_FORTUNE_CHANNELER = "tl_frtnChnlr";

	public const string TEMP_LUCK_TAG_SPICY_PEPPER_RED = "tl_spPeppR";

	public const string TEMP_LUCK_TAG_SPICY_PEPPER_GREEN = "tl_spPeppG";

	public const string TEMP_LUCK_TAG_GOLDEN_PEPPER = "tl_spGdPpr";

	public const string TEMP_LUCK_TAG_ROTTEN_PEPPER = "tl_spRtnPpr";

	public const string TEMP_LUCK_TAG_BELL_PEPPER = "tl_spBllPpr";

	public const string TEMP_LUCK_TAG_HAMSA = "tl_hamsa";

	public const string TEMP_LUCK_TAG_LUCKY_COIN = "tl_LckCn";

	public const string TEMP_LUCK_TAG_ANCIENT_COIN = "tl_AncntCn";

	public const string TEMP_LUCK_TAG_TOY_TRAIN = "tl_TTrain";

	public const string TEMP_LUCK_STEPS_COUNTER = "tl_StpsCntr";

	public const string TEMP_LUCK_DISC_B = "tl_DskB";

	public const string TEMP_LUCK_JIMBO = "tl_jmb";

	public const string TEMP_LUCK_DEVIL_HORN = "tl_dvlhrn";

	private const int MAX_BUY_TIMES_ENDLESS = -1;

	private const int MAX_BUY_TIMES_NEVER = 0;

	private const int MAX_BUY_TIMES_ONCE = 1;

	public const float CHANCE_INCREASE_UNIT_HALF = 0.4f;

	public const float CHANCE_INCREASE_UNIT = 0.8f;

	public const float CHANCE_INCREASE_UNIT_AND_A_HALF = 1.2f;

	public const float CHANCE_INCREASE_UNIT_DOUBLE = 1.6f;

	public const float CHANCE_INCREASE_UNIT_TRIPLE = 2.4f;

	public const float LUCK_UNIT_REALLY_SMALL = 2f;

	public const float LUCK_UNIT_SMALL = 4f;

	public const float LUCK_UNIT_ONE = 5f;

	public const float LUCK_UNIT_ONE_AND_HALF = 7f;

	public const float LUCK_UNIT_TWO = 9f;

	public const float LUCK_UNIT_TWO_TRUE = 10f;

	private const string POWERUP_ZOOM_CAMERA_TAG = "pzct";

	public AudioClip triggerSpecificSound;

	private DiegeticMenuElement diegeticMenuElement;

	public GameObject meshHolder;

	private MeshFilter meshFilter;

	private MeshRenderer meshRenderer;

	private SkinnedMeshRenderer skinnedMeshRenderer;

	private Animator animator_IfAny;

	private Material materialDefault;

	private Material materialMod_SymbolMult;

	private Material materialMod_PatternMult;

	private Material materialMod_CloverTicket;

	private Material materialMod_Obsessive;

	private Material materialMod_Gambler;

	private Material materialMod_Speculative;

	private Material materialMod_Devious;

	public Transform modEffetctsHolder;

	public Transform modHolder_OutlineSwitcher;

	public GameObject modEffect_SymbolMult;

	public GameObject modEffect_PatternMult;

	public GameObject modEffect_CloverTicket;

	public GameObject modEffect_Obsessive;

	public GameObject modEffect_Gambler;

	public GameObject modEffect_Speculative;

	public GameObject modEffect_Devious;

	private Outline myOutline;

	public GameObject glowHolder;

	public Transform glowTransform;

	public SpriteRenderer[] glowSprites;

	public GameObject sacredGlowHolder;

	public Transform sacredGlowTransform;

	public SpriteRenderer[] sacredGlowSprites;

	public GameObject redButtonUiHolder;

	public Canvas redButtonUiCanvas;

	public TextMeshProUGUI redButtonUiText;

	public Image redButtonUiBackgruondImage;

	[NonSerialized]
	public PowerupScript.Identifier identifier = PowerupScript.Identifier.undefined;

	[NonSerialized]
	public PowerupScript.Category category = PowerupScript.Category.undefined;

	[NonSerialized]
	public PowerupScript.Archetype archetype = PowerupScript.Archetype.undefined;

	private long startingPrice = -1L;

	private bool isInstantPowerup;

	private int maxBuyTimes = -1;

	private float storeRerollChance;

	private string nameKey;

	private string descriptionKey;

	private string unlockMissionKey;

	private bool _isBaseSet;

	private BigInteger unlockPrice = -1;

	private bool equippedChached;

	private bool inDrawerChached;

	private static bool equipFlag_DontCheckForSpace = false;

	private static bool _suppressThrowAwaySound = false;

	private static bool _suppressThrowAwayAnimation = false;

	private Vector3? _boundsSize;

	private bool meshIsStolen;

	private static Color colorGrayedOut = new Color(0.75f, 0.75f, 0.75f, 1f);

	private bool sacredPropertyApplied;

	private PowerupScript.Modifier _modifierEffects_LastModifier = PowerupScript.Modifier.count;

	private static int modifiedPowerups_EquippedCounter_SymbolMult = 0;

	private static int modifiedPowerups_EquippedCounter_PatternMult = 0;

	private static int modifiedPowerups_EquippedCounter_CloverTicket = 0;

	private static int modifiedPowerups_EquippedCounter_Obsessive = 0;

	private static int modifiedPowerups_EquippedCounter_Gambler = 0;

	private static int modifiedPowerups_EquippedCounter_Speculative = 0;

	private static int modifiedPowerups_EquippedCounter_Devious = 0;

	public static PowerupScript inspectedPowerup = null;

	private Coroutine inspectionZoomCoroutine;

	private static bool _cameraInspectionRunning = false;

	private static bool _forceClosingInspection_Death = false;

	private bool? _isRedButtonCharm;

	private bool _redButton_TextUpdateRequest;

	private string _redButton_TextString;

	private float updateTimer;

	private float justEquippedGlowTimer;

	private static List<PowerupScript.Identifier> _skeletonPiecesSpawnable = new List<PowerupScript.Identifier>();

	private static List<PowerupScript> sacredCharms = new List<PowerupScript>();

	private static List<PowerupScript> luckBasedCharms = new List<PowerupScript>();

	private static List<PowerupScript> picturesOfSymbols = new List<PowerupScript>();

	private PowerupScript.PowerupEvent onEquip;

	private PowerupScript.PowerupEvent onUnequip;

	private PowerupScript.PowerupEvent onPutInDrawer;

	private PowerupScript.PowerupEvent onThrowAway;

	private static PowerupScript.PowerupEvent onEquipStatic;

	private static PowerupScript.PowerupEvent onUnequipStatic;

	private static PowerupScript.PowerupEvent onPutInDrawerStatic;

	private static PowerupScript.PowerupEvent onThrowAwayStatic;

	private static bool throwAwayCanTriggerEffects = true;

	private static int _shromsActivationsCounter = 0;

	private static int _rorschachActivationsCounter = 0;

	private const int POTATO_POWERUP_THRESHOLD = 2;

	private const int DISC_C_SPINS_THRESHOLD = 7;

	private const int DISC_B_SPINS_THRESHOLD = 7;

	private const int DISC_A_SPINS_THRESHOLD = 7;

	private const int STEAM_LOCOMOTIVE_THRESHOLD = 3;

	private const int DIESEL_LOCOMOTIVE_THRESHOLD = 3;

	private const int STEPS_COUNTER_TRIGGERS_NEEDED = 3;

	private static Dictionary<PatternScript.Kind, double> _abstractPaintingBonusValue = new Dictionary<PatternScript.Kind, double>();

	private static bool _brokenCalculatorIsActive = false;

	private static Dictionary<SymbolScript.Kind, BigInteger> _vinesoupBonusValue = new Dictionary<SymbolScript.Kind, BigInteger>();

	private static PowerupScript[] _fortuneCookiePowerups = new PowerupScript[4];

	private const int YELLOW_STAR_SPINS_THRESHOLD = 1;

	private const int TOY_TRAIN_SPINS_THRESHOLD = 2;

	private static long _tarotDeck_TriggersPerSpin = 0L;

	private static int _ancientCoinRoundTriggersCounter = 0;

	private static CameraController.PositionKind _ankhCameraPositionRestore = CameraController.PositionKind.Undefined;

	private static float horseShoesLuck = 0f;

	private const float HORSESHOE_LUCK_SMALL = 1f;

	private static int _goldenHorseShoeRoundUsesCounter = 0;

	private const int RED_PEPPER_MAX_ACTIVATIONS = 12;

	private const int GREEN_PEPPER_MAX_ACTIVATIONS = 9;

	private const string _CROSS_ANIM_TURN_DOWN = "TurnDown";

	private const string _CROSS_ANIM_TURN_UP = "TurnUp";

	private static int _bookOfShadowsRoundActivationsCounter = 0;

	private static long _gabibbhMultiplier = 1L;

	private static int _mysticalTomatoRepetitions = 0;

	private static BigInteger _crystalSkullBonus_Diamonds;

	private static BigInteger _crystalSkullBonus_Coins;

	private static BigInteger _crystalSkullBonus_Sevens;

	private const RedButtonScript.RegistrationCapsule.Timing SYMBOL_INSTANTS_TIMING = RedButtonScript.RegistrationCapsule.Timing.perDeadline;

	private const float SYMBOLS_INSTANT_CHANCE_BONUS = 1.6f;

	private static PowerupScript[] _photoBookPowerups = new PowerupScript[4];

	private const float GOLDEN_KING_MIDA_BASE_MOD_CHANCE = 0.03f;

	private const float DEALER_BASE_MOD_CHANCE = 0.05f;

	private const float CAPITALIST_BASE_MOD_CHANCE = 0.03f;

	private const float GOLDEN_SYMBOL_MOD_CHANCE_FRUITS = 0.2f;

	private const float GOLDEN_SYMBOL_MOD_CHANCE_CLOVERBELLS = 0.2f;

	private const float GOLDEN_SYMBOL_MOD_CHANCE_DIAMONDCOINS = 0.25f;

	private const float GOLDEN_SYMBOL_MOD_CHANCE_SEVEN = 0.3f;

	private const float BOARDGAME_C_MOD_CHANCE_FRUITS = 0.35f;

	private const float BOARDGAME_C_MOD_CHANCE_CLOVERBELLS = 0.2f;

	private const float BOARDGAME_C_MOD_CHANCE_DIAMONDCOINS = 0.2f;

	private const float BOARDGAME_C_MOD_CHANCE_SEVEN = 0.2f;

	private const float BOARDGAME_M_FRUITS_MOD_CHANCE = 0.2f;

	private const float BOARDGAME_M_CLOVERBELL_MOD_CHANCE = 0.2f;

	private const float BOARDGAME_M_DIAMOND_COINS_SEVEN_MOD_CHANCE = 0.2f;

	public static List<Vector2Int> _dice4UntriggeredPositions = new List<Vector2Int>(16);

	private static CameraController.PositionKind _diceD6CameraPosition = CameraController.PositionKind.Undefined;

	private static CameraController.PositionKind _diceD20CameraPosition = CameraController.PositionKind.Undefined;

	private static List<PatternScript.Kind> angelPatterns = new List<PatternScript.Kind>
	{
		PatternScript.Kind.horizontal2,
		PatternScript.Kind.diagonal2,
		PatternScript.Kind.vertical2
	};

	private static int eternityRepetitionsCounter = 0;

	private static long _adamsRibCageCounter = 1L;

	private static List<PowerupScript> _initializationTempList = new List<PowerupScript>();

	public enum Identifier
	{
		undefined = -1,
		Skeleton_Arm1,
		Skeleton_Arm2,
		Skeleton_Leg1,
		Skeleton_Leg2,
		Skeleton_Head,
		Ankh,
		HorseShoe,
		HorseShoeGold,
		HamsaUpside,
		HamsaInverted,
		LuckyCat,
		LuckyCatFat,
		LuckyCatSwole,
		OneTrickPony,
		RedCrystal,
		TarotDeck,
		PoopBeetle,
		GrandmasPurse,
		Pentacle,
		HouseContract,
		Button2X,
		DearDiary,
		Megaphone,
		SuperCapacitor,
		CrankGenerator,
		GrattaEVinci_ScratchAndWin,
		Stonks,
		CarBattery,
		ExpiredMedicines,
		GoldenHand_MidasTouch,
		PissJar,
		PoopJar,
		Painkillers,
		Wallet,
		MoneyBriefCase,
		Calendar,
		YellowStar,
		Wolf,
		FortuneCookie,
		FideltyCard,
		Sardines,
		BrokenCalculator,
		TheCollector,
		FortuneChanneler,
		Nose,
		EyeJar,
		VoiceMailTape,
		Garbage,
		AllIn,
		RingBell,
		ConsolationPrize,
		DarkLotus,
		StepsCounter,
		WeirdClock,
		MusicTape,
		ShoppingCart,
		CrowBar,
		DiscA,
		DiscB,
		DiscC,
		CardboardHouse,
		Cigarettes,
		ElectricityCounter,
		PotatoPower,
		FakeCoin,
		AncientCoin,
		CatTreats,
		Depression,
		ToyTrain,
		LocomotiveSteam,
		LocomotiveDiesel,
		CloverVoucher,
		CloverPot,
		CloverPet,
		CloversLandPatch,
		Mushrooms,
		VineSoupShroom,
		GiantShroom,
		Hole_Circle,
		Hole_Romboid,
		Hole_Cross,
		Rorschach,
		AbstractPainting,
		Pareidolia,
		Hourglass,
		FruitBasket,
		SevenSinsStone,
		Necklace,
		CloverBell,
		HornChilyRed,
		HornChilyGreen,
		RottenPepper,
		BellPepper,
		GoldenPepper,
		HornDevil,
		Necronomicon,
		HolyBible,
		Baphomet,
		Cross,
		Rosary,
		BookOfShadows,
		Gabibbh,
		PossessedPhone,
		MysticalTomato,
		RitualBell,
		CrystalSkull,
		EvilDeal,
		ChastityBelt,
		PhotoBook,
		SymbolInstant_Lemon,
		SymbolInstant_Cherry,
		SymbolInstant_Clover,
		SymbolInstant_Bell,
		SymbolInstant_Diamond,
		SymbolInstant_Treasure,
		SymbolInstant_Seven,
		GeneralModCharm_Clicker,
		GeneralModCharm_CloverBellBattery,
		GeneralModCharm_CrystalSphere,
		Boardgame_C_Dealer,
		GoldenKingMida,
		Boardgame_M_Capitalist,
		PuppetPersonalTrainer,
		PuppetElectrician,
		PuppetFortuneTeller,
		GoldenSymbol_Lemon,
		GoldenSymbol_Cherry,
		GoldenSymbol_Clover,
		GoldenSymbol_Bell,
		GoldenSymbol_Diamond,
		GoldenSymbol_Treasure,
		GoldenSymbol_Seven,
		Boardgame_C_Bricks,
		Boardgame_C_Wood,
		Boardgame_C_Sheep,
		Boardgame_C_Wheat,
		Boardgame_C_Stone,
		Boardgame_C_Harbor,
		Boardgame_C_Thief,
		Boardgame_M_Carriola,
		Boardgame_M_Shoe,
		Boardgame_M_Ditale,
		Boardgame_M_FerroDaStiro,
		Boardgame_M_Car,
		Boardgame_M_Ship,
		Boardgame_M_Hat,
		PlayingCard_HeartsAce,
		PlayingCard_ClubsAce,
		PlayingCard_DiamondsAce,
		PlayingCard_SpadesAce,
		Jimbo,
		Dice_4,
		Dice_6,
		Dice_20,
		_999_AngelHand,
		_999_EyeOfGod,
		_999_HolySpirit,
		_999_SacredHeart,
		_999_Aureola,
		_999_TheBlood,
		_999_TheBody,
		_999_Eternity,
		_999_AdamsRibcage,
		_999_OphanimWheels,
		count
	}

	public enum Category
	{
		undefined = -1,
		skeleton,
		normal,
		count
	}

	public enum Archetype
	{
		undefined = -1,
		generic,
		spicyPeppers,
		religious,
		symbolInstants,
		generalModifierCharm,
		commonModPuppets,
		goldenSymbols,
		boardgameC,
		boardgameM,
		classicPlayingCards,
		dices,
		skeleton,
		sacred,
		count
	}

	public enum Modifier
	{
		none,
		symbolMultiplier,
		patternMultiplier,
		cloverTicket,
		obsessive,
		gambler,
		speculative,
		devious,
		count
	}

	public enum PublicRarity
	{
		common,
		uncommon,
		rare,
		epic
	}

	public enum MenuControllerTarget
	{
		Room,
		Slot
	}

	// (Invoke) Token: 0x06001142 RID: 4418
	public delegate void PowerupEvent(PowerupScript powerup);
}
