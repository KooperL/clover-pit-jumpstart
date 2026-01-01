using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Panik;
using UnityEngine;

// Token: 0x02000014 RID: 20
[Serializable]
public class GameplayData
{
	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000102 RID: 258
	public static GameplayData Instance
	{
		get
		{
			if (Data.game == null)
			{
				return null;
			}
			return Data.game.gameplayData;
		}
	}

	// Token: 0x06000103 RID: 259
	public void Save_Prepare()
	{
		for (int i = 0; i < this.equippedPowerups.Length; i++)
		{
			if (PowerupScript.list_EquippedNormal.Count <= i || PowerupScript.list_EquippedNormal[i] == null)
			{
				this.equippedPowerups[i] = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined);
			}
			else
			{
				this.equippedPowerups[i] = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.list_EquippedNormal[i].identifier);
			}
		}
		for (int j = 0; j < this.equippedPowerups_Skeleton.Length; j++)
		{
			if (PowerupScript.list_EquippedSkeleton.Count <= j || PowerupScript.list_EquippedSkeleton[j] == null)
			{
				this.equippedPowerups_Skeleton[j] = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined);
			}
			else
			{
				this.equippedPowerups_Skeleton[j] = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.list_EquippedSkeleton[j].identifier);
			}
		}
		for (int k = 0; k < 4; k++)
		{
			if (PowerupScript.array_InDrawer[k] == null)
			{
				this.drawerPowerups[k] = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined);
			}
			else
			{
				this.drawerPowerups[k] = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.array_InDrawer[k].identifier);
			}
		}
		this._PowerupsData_PrepareForSerialization();
		this._PowerupsSpecific_PrepareForSerialization();
		for (int l = 0; l < 4; l++)
		{
			if (StoreCapsuleScript.storePowerups[l] == null)
			{
				this.storePowerups[l] = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined);
			}
			else
			{
				this.storePowerups[l] = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(StoreCapsuleScript.storePowerups[l].identifier);
			}
		}
		this.storeChainIndex_Array = StoreCapsuleScript.chainIndex_Array;
		this.storeChainIndex_PowerupIdentifier = StoreCapsuleScript.chainIndex_PowerupIdentifier;
		this.storeLastRandomIndex = StoreCapsuleScript._lastRandomIndex;
		this._storeRestockExtraCost_ByteArray = this._storeRestockExtraCost.ToByteArray();
		this._Coins_PrepareForSerialization();
		this._Debt_PrepareForSerialization();
		this._Symbols_PrepareForSerialization();
		this._Patterns_PrepareForSerialization();
		GameplayData._ExtraLuckEnsureArray(this);
		this._Phone_PrepareForSerialization();
		this.RunModifiers_SavePreparing();
		this._Ending_PrepareForSerialization();
		this._MetaProgression_PrepareForSerialization();
		this._GameStats_PrepareForSerialization();
	}

	// Token: 0x06000104 RID: 260
	public void Load_Format()
	{
		this.RngEnsure(false);
		if (this.equippedPowerups.Length != 33)
		{
			Debug.LogWarning("GameplayData.Load_Format() - equippedPowerups array size is not correct! Has the game been updated?");
			Array.Resize<string>(ref this.equippedPowerups, 33);
		}
		if (this.equippedPowerups_Skeleton.Length != 5)
		{
			Debug.LogWarning("GameplayData.Load_Format() - equippedPowerups_Skeleton array size is not correct! Has the game been updated?");
			Array.Resize<string>(ref this.equippedPowerups_Skeleton, 5);
		}
		if (this.drawerPowerups.Length != 4)
		{
			Debug.LogWarning("GameplayData.Load_Format() - drawerPowerups array size is not correct! Has the game been updated?");
			Array.Resize<string>(ref this.drawerPowerups, 4);
		}
		this._PowerupsData_RestoreFromSerialization();
		this._PowerupsSpecific_RestoreFromSerialization();
		if (this.storePowerups.Length != 4)
		{
			Debug.LogError("GameplayData.Load_Format() - storePowerups array size is not correct!");
			Array.Resize<string>(ref this.storePowerups, 4);
		}
		this._storeRestockExtraCost = this.BigIntegerFromByteArray(this._storeRestockExtraCost_ByteArray, 0);
		this._Coins_RestoreFromSerialization();
		this._Debt_RestoreFromSerialization();
		this._Symbols_RestoreFromSerialization();
		this._Patterns_RestoreFromSerialization();
		GameplayData._ExtraLuckEnsureArray(this);
		this._Phone_RestoreFromSerialization();
		this.RunModifiers_LoadFormat();
		this._Ending_RestoreFromSerialization();
		this._MetaProgression_RestoreFromSerialization();
		this._GameStats_RestoreFromSerialization();
	}

	// Token: 0x06000105 RID: 261
	private BigInteger BigIntegerFromByteArray(byte[] byteArray, BigInteger defaultValue)
	{
		if (byteArray == null || byteArray.Length == 0)
		{
			return defaultValue;
		}
		return new BigInteger(byteArray);
	}

	// Token: 0x06000106 RID: 262
	public static int SeedGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.seed;
	}

	// Token: 0x06000107 RID: 263
	public static uint SeedGetAsUInt()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0U;
		}
		return (uint)instance.seed;
	}

	// Token: 0x06000108 RID: 264
	public static string SeedGetAsString()
	{
		return GameplayData.SeedGetAsUInt().ToString("0000000000");
	}

	// Token: 0x06000109 RID: 265
	public static void SeedInitRandom()
	{
		global::UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
		uint num = (uint)global::UnityEngine.Random.Range(int.MinValue, int.MaxValue);
		uint num2 = num;
		uint num3 = (uint)(5 + global::UnityEngine.Random.Range(0, 5));
		int num4 = 0;
		while ((long)num4 < (long)((ulong)num3))
		{
			num = (num + num3) ^ Bit.ShiftRotateLeft(num, 31) ^ Bit.ShiftRotateLeft(num, 21) ^ Bit.ShiftRotateLeft(num, 13) ^ Bit.ShiftRotateLeft(num, 1) ^ num2;
			num4++;
		}
		GameplayData.SeedInitSpecific((int)num);
	}

	// Token: 0x0600010A RID: 266
	public static void SeedInitSpecific(int seed)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.seed = seed;
		instance.RngEnsure(true);
	}

	// Token: 0x0600010B RID: 267
	private void RngEnsure(bool resetForNewGame)
	{
		if (resetForNewGame || this.rngRunMod == null)
		{
			this.rngRunMod = new Rng(this.seed);
		}
		if (resetForNewGame || this.rngSymbolsMod == null)
		{
			this.rngSymbolsMod = new Rng(this.seed);
		}
		if (resetForNewGame || this.rngPowerupsMod == null)
		{
			this.rngPowerupsMod = new Rng(this.seed);
		}
		if (resetForNewGame || this.rngSymbolsChance == null)
		{
			this.rngSymbolsChance = new Rng(this.seed);
		}
		if (resetForNewGame || this.rngCards == null)
		{
			this.rngCards = new Rng(this.seed);
		}
		if (resetForNewGame || this.rngPowerupsAll == null)
		{
			this.rngPowerupsAll = new Rng(this.seed);
		}
		if (resetForNewGame || this.rngAbilities == null)
		{
			this.rngAbilities = new Rng(this.seed);
		}
		if (resetForNewGame || this.rngDrawers == null)
		{
			this.rngDrawers = new Rng(this.seed);
		}
		if (resetForNewGame || this.rngStore == null)
		{
			this.rngStore = new Rng(this.seed);
		}
		if (resetForNewGame || this.rngStoreChains == null)
		{
			this.rngStoreChains = new Rng(this.seed);
		}
		if (resetForNewGame || this.rngPhone == null)
		{
			this.rngPhone = new Rng(this.seed);
		}
		if (resetForNewGame || this.rngSlotMachineLuck == null)
		{
			this.rngSlotMachineLuck = new Rng(this.seed);
		}
		if (resetForNewGame || this.rng666 == null)
		{
			this.rng666 = new Rng(this.seed);
		}
		if (resetForNewGame || this.rngGarbage == null)
		{
			this.rngGarbage = new Rng(this.seed);
		}
		this.PowerupsRngEnsure(resetForNewGame);
	}

	// Token: 0x0600010C RID: 268
	public static BigInteger StoreRestockExtraCostGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._storeRestockExtraCost;
	}

	// Token: 0x0600010D RID: 269
	public static void StoreRestockExtraCostSet(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._storeRestockExtraCost = value;
		if (instance._storeRestockExtraCost < 0L)
		{
			instance._storeRestockExtraCost = 0;
		}
	}

	// Token: 0x0600010E RID: 270
	public static void StoreRestockExtraCostAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.StoreRestockExtraCostSet(instance._storeRestockExtraCost + value);
	}

	// Token: 0x0600010F RID: 271
	public static long StoreFreeRestocksGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._storeFreeRestocks;
	}

	// Token: 0x06000110 RID: 272
	public static void StoreFreeRestocksSet(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._storeFreeRestocks = value;
		StoreCapsuleScript.RefreshCostTextAll();
	}

	// Token: 0x06000111 RID: 273
	public static long StoreTemporaryDiscountGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.temporaryDiscount;
	}

	// Token: 0x06000112 RID: 274
	public static void StoreTemporaryDiscountSet(long value, bool refreshStoreText)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.temporaryDiscount = value;
		if (instance.temporaryDiscount < 0L)
		{
			instance.temporaryDiscount = 0L;
		}
		if (refreshStoreText)
		{
			StoreCapsuleScript.RefreshCostTextAll();
		}
	}

	// Token: 0x06000113 RID: 275
	public static void StoreTemporaryDiscountAdd(long value, bool refreshStoreText)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.StoreTemporaryDiscountSet(instance.temporaryDiscount + value, refreshStoreText);
		if (refreshStoreText)
		{
			StoreCapsuleScript.RefreshCostTextAll();
		}
	}

	// Token: 0x06000114 RID: 276
	public static void StoreTemporaryDiscountReset(bool refreshStoreText)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.temporaryDiscount = 0L;
		if (refreshStoreText)
		{
			StoreCapsuleScript.RefreshCostTextAll();
		}
	}

	// Token: 0x06000115 RID: 277
	public static long StoreTemporaryDiscountPerSlotGet(int slotIndex)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.temporaryDiscountPerSlot[slotIndex];
	}

	// Token: 0x06000116 RID: 278
	public static void StoreTemporaryDiscountPerSlotSet(int slotIndex, long value, bool refreshStoreText)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.temporaryDiscountPerSlot[slotIndex] = value;
		if (instance.temporaryDiscountPerSlot[slotIndex] < 0L)
		{
			instance.temporaryDiscountPerSlot[slotIndex] = 0L;
		}
		if (refreshStoreText)
		{
			StoreCapsuleScript.RefreshCostTextAll();
		}
	}

	// Token: 0x06000117 RID: 279
	public static void StoreTemporaryDiscountPerSlotSet_PerPowerup(PowerupScript.Identifier identifier, long value, bool refreshStoreText)
	{
		if (GameplayData.Instance == null)
		{
			return;
		}
		int num = -1;
		for (int i = 0; i < StoreCapsuleScript.storePowerups.Length; i++)
		{
			if (!(StoreCapsuleScript.storePowerups[i] == null) && StoreCapsuleScript.storePowerups[i].identifier == identifier)
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			return;
		}
		GameplayData.StoreTemporaryDiscountPerSlotSet(num, value, refreshStoreText);
	}

	// Token: 0x06000118 RID: 280
	public static void StoreTemporaryDiscountPerSlotAdd(int slotIndex, long value, bool refreshStoreText)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.StoreTemporaryDiscountPerSlotSet(slotIndex, instance.temporaryDiscountPerSlot[slotIndex] + value, refreshStoreText);
		if (refreshStoreText)
		{
			StoreCapsuleScript.RefreshCostTextAll();
		}
	}

	// Token: 0x06000119 RID: 281
	public static void StoreTemporaryDiscountPerSlotAdd_PerPowerup(PowerupScript.Identifier identifier, long value, bool refreshStoreText)
	{
		if (GameplayData.Instance == null)
		{
			return;
		}
		int num = -1;
		for (int i = 0; i < StoreCapsuleScript.storePowerups.Length; i++)
		{
			if (!(StoreCapsuleScript.storePowerups[i] == null) && StoreCapsuleScript.storePowerups[i].identifier == identifier)
			{
				num = i;
				break;
			}
		}
		if (num < 0)
		{
			return;
		}
		GameplayData.StoreTemporaryDiscountPerSlotAdd(num, value, refreshStoreText);
	}

	// Token: 0x0600011A RID: 282
	public static void StoreTemporaryDiscountPerSlotResetAll(bool refreshStoreText)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		for (int i = 0; i < instance.temporaryDiscountPerSlot.Length; i++)
		{
			instance.temporaryDiscountPerSlot[i] = 0L;
		}
		if (refreshStoreText)
		{
			StoreCapsuleScript.RefreshCostTextAll();
		}
	}

	// Token: 0x0600011B RID: 283
	private void _Coins_PrepareForSerialization()
	{
		this.coins_ByteArray = this.coins.ToByteArray();
		this.depositedCoins_ByteArray = this.depositedCoins.ToByteArray();
		this.interestEarned_ByteArray = this.interestEarned.ToByteArray();
		this.roundEarnedCoins_ByteArray = this.roundEarnedCoins.ToByteArray();
	}

	// Token: 0x0600011C RID: 284
	private void _Coins_RestoreFromSerialization()
	{
		this.coins = this.BigIntegerFromByteArray(this.coins_ByteArray, 13);
		this.depositedCoins = this.BigIntegerFromByteArray(this.depositedCoins_ByteArray, 30);
		this.interestEarned = this.BigIntegerFromByteArray(this.interestEarned_ByteArray, 0);
		this.roundEarnedCoins = this.BigIntegerFromByteArray(this.roundEarnedCoins_ByteArray, 0);
	}

	// Token: 0x0600011D RID: 285
	public static BigInteger CoinsGet()
	{
		if (GameplayData.Instance == null)
		{
			return 13;
		}
		return GameplayData.Instance.coins;
	}

	// Token: 0x0600011E RID: 286
	public static void CoinsSet(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.coins = value;
		if (instance.coins < 0L)
		{
			instance.coins = 0;
		}
	}

	// Token: 0x0600011F RID: 287
	public static void CoinsAdd(BigInteger value, bool addToStats)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.CoinsSet(instance.coins + value);
		if (addToStats && value > 0L)
		{
			GameplayData.Stats_CoinsEarned_Add(value);
		}
	}

	// Token: 0x06000120 RID: 288
	public static BigInteger DepositGet()
	{
		if (GameplayData.Instance == null)
		{
			return 30;
		}
		return GameplayData.Instance.depositedCoins;
	}

	// Token: 0x06000121 RID: 289
	public static void DepositSet(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.depositedCoins = value;
		if (instance.depositedCoins < 0L)
		{
			instance.depositedCoins = 0;
		}
	}

	// Token: 0x06000122 RID: 290
	public static void DepositAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.DepositSet(instance.depositedCoins + value);
	}

	// Token: 0x06000123 RID: 291
	public static bool HasDepositedSomething()
	{
		return GameplayData.DepositGet() > 30L;
	}

	// Token: 0x06000124 RID: 292
	public static BigInteger NextDepositAmmountGet(bool forceDefault = false)
	{
		BigInteger bigInteger = GameplayData.DebtGet();
		BigInteger bigInteger2;
		if (bigInteger > 1073741823L)
		{
			bigInteger2 = bigInteger / 20;
		}
		else
		{
			bigInteger2 = Mathf.FloorToInt((float)GameplayData.DebtGet().CastToInt() / 20f);
		}
		if (forceDefault)
		{
			return bigInteger2;
		}
		BigInteger bigInteger3 = GameplayData.CoinsGet();
		BigInteger bigInteger4 = GameplayData.DebtMissingGet();
		BigInteger bigInteger5;
		if (bigInteger3 > 0L && bigInteger3 < bigInteger2 && bigInteger3 < bigInteger4)
		{
			bigInteger5 = bigInteger3;
		}
		else if (bigInteger4 < bigInteger2)
		{
			bigInteger5 = bigInteger4;
		}
		else
		{
			bigInteger5 = bigInteger2;
		}
		BigInteger bigInteger6 = GameplayData.SpinCostGet_Single();
		if (GameplayData.RoundsLeftToDeadline() > 0 && bigInteger5 > bigInteger6)
		{
			BigInteger bigInteger7 = GameplayData.SpinCostMax_Get();
			if (bigInteger3 - bigInteger5 < bigInteger7)
			{
				if (bigInteger3 > bigInteger7)
				{
					bigInteger5 = bigInteger3 - bigInteger7;
				}
				else
				{
					bigInteger5 = bigInteger6;
				}
			}
		}
		return bigInteger5;
	}

	// Token: 0x06000125 RID: 293
	public static BigInteger InterestEarnedGet()
	{
		if (GameplayData.Instance == null)
		{
			return 0;
		}
		return GameplayData.Instance.interestEarned;
	}

	// Token: 0x06000126 RID: 294
	public static BigInteger InterestEarnedHypotetically()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.depositedCoins * (int)GameplayData.InterestRateGet() / 100;
	}

	// Token: 0x06000127 RID: 295
	public static void InterestEarnedGrow()
	{
		GameplayData.InterestEarnedGrow_Manual(GameplayData.InterestEarnedHypotetically());
	}

	// Token: 0x06000128 RID: 296
	public static void InterestEarnedGrow_Manual(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.interestEarned += value;
		if (instance.interestEarned < 0L)
		{
			instance.interestEarned = 0;
		}
	}

	// Token: 0x06000129 RID: 297
	public static void InterestPickUp()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.CoinsAdd(instance.interestEarned, true);
		instance.interestEarned = 0;
	}

	// Token: 0x0600012A RID: 298
	public static float InterestRateGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 7f;
		}
		float num = instance.interestRate + (float)PowerupScript.GrandmasPurse_ExtraInterestGet(true) + PowerupScript.StonksInterestBonusGet(true) + (float)PowerupScript.ModifiedPowerups_GetInterestBonus() + (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_Interest, true) ? 5f : 0f);
		if (GameplayData.RunModifier_GetCurrent() == RunModifierScript.Identifier.interestsGrow)
		{
			int num2 = GameplayData.DebtIndexGet().CastToInt();
			num += (float)Mathf.Min(2 * num2, 16);
		}
		num *= PowerupScript.EvilDealBonusMultiplier_Float();
		return Mathf.Clamp(num, 0f, 100f);
	}

	// Token: 0x0600012B RID: 299
	public static void InterestRateSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.interestRate = value;
		if (instance.interestRate < 0f)
		{
			instance.interestRate = 0f;
		}
		if (instance.interestRate > 100f)
		{
			instance.interestRate = 100f;
		}
	}

	// Token: 0x0600012C RID: 300
	public static void InterestRateAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.InterestRateSet(instance.interestRate + value);
	}

	// Token: 0x0600012D RID: 301
	public static BigInteger RoundEarnedCoinsGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.roundEarnedCoins;
	}

	// Token: 0x0600012E RID: 302
	public static void RoundEarnedCoinsSet(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.roundEarnedCoins = value;
	}

	// Token: 0x0600012F RID: 303
	public static void RoundEarnedCoinsAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.RoundEarnedCoinsSet(instance.roundEarnedCoins + value);
	}

	// Token: 0x06000130 RID: 304
	public static void RoundEarnedCoinsReset()
	{
		GameplayData.RoundEarnedCoinsSet(0);
	}

	// Token: 0x06000131 RID: 305
	public static long CloverTicketsGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 2L;
		}
		return instance.cloverTickets;
	}

	// Token: 0x06000132 RID: 306
	public static void CloverTicketsSet(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.cloverTickets = value;
		if (instance.cloverTickets < 0L)
		{
			instance.cloverTickets = 0L;
		}
	}

	// Token: 0x06000133 RID: 307
	public static void CloverTicketsAdd(long value, bool addToStats)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.CloverTicketsSet(instance.cloverTickets + value);
		if (addToStats && value > 0L)
		{
			GameplayData.Stats_TicketsEarned_Add(value);
		}
	}

	// Token: 0x06000134 RID: 308
	public static long CloverTickets_BonusLittleBet_Get(bool considerPowerups)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 2L;
		}
		long num = instance.cloverTickets_BonusFor_LittleBet;
		if (considerPowerups)
		{
			num *= (long)PowerupScript.EvilDealBonusMultiplier();
		}
		return num;
	}

	// Token: 0x06000135 RID: 309
	public static void CloverTickets_BonusLittleBet_Set(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.cloverTickets_BonusFor_LittleBet = value;
		if (instance.cloverTickets_BonusFor_LittleBet < 0L)
		{
			instance.cloverTickets_BonusFor_LittleBet = 0L;
		}
	}

	// Token: 0x06000136 RID: 310
	public static void CloverTickets_BonusLittleBet_Add(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.CloverTickets_BonusLittleBet_Set(instance.cloverTickets_BonusFor_LittleBet + value);
	}

	// Token: 0x06000137 RID: 311
	public static long CloverTickets_BonusBigBet_Get(bool considerPowerups)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 2L;
		}
		long num = instance.cloverTickets_BonusFor_BigBet;
		if (considerPowerups)
		{
			num *= (long)PowerupScript.EvilDealBonusMultiplier();
		}
		return num;
	}

	// Token: 0x06000138 RID: 312
	public static void CloverTickets_BonusBigBet_Set(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.cloverTickets_BonusFor_BigBet = value;
		if (instance.cloverTickets_BonusFor_BigBet < 0L)
		{
			instance.cloverTickets_BonusFor_BigBet = 0L;
		}
	}

	// Token: 0x06000139 RID: 313
	public static void CloverTickets_BonusBigBet_Add(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.CloverTickets_BonusBigBet_Set(instance.cloverTickets_BonusFor_BigBet + value);
	}

	// Token: 0x0600013A RID: 314
	public static long CloverTickets_BonusRoundsLeft_Get()
	{
		if (PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Wolf))
		{
			return 0L;
		}
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 2L;
		}
		return instance.cloverTickets_BonusFor_RoundsLeft;
	}

	// Token: 0x0600013B RID: 315
	public static void CloverTickets_BonusRoundsLeft_Set(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.cloverTickets_BonusFor_RoundsLeft = value;
		if (instance.cloverTickets_BonusFor_RoundsLeft < 0L)
		{
			instance.cloverTickets_BonusFor_RoundsLeft = 0L;
		}
	}

	// Token: 0x0600013C RID: 316
	public static void CloverTickets_BonusRoundsLeft_Add(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.CloverTickets_BonusRoundsLeft_Set(instance.cloverTickets_BonusFor_RoundsLeft + value);
	}

	// Token: 0x0600013D RID: 317
	public static bool ATMDeadline_RewardPickupMemo_MessageShownGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.atmDeadline_RewardPickupMemo_MessageShown;
	}

	// Token: 0x0600013E RID: 318
	public static void ATMDeadline_RewardPickupMemo_MessageShownSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.atmDeadline_RewardPickupMemo_MessageShown = true;
	}

	// Token: 0x0600013F RID: 319
	public static BigInteger DeadlineReward_CoinsGet(BigInteger currentDebtIndex)
	{
		if (GameplayData.Instance == null)
		{
			return 0;
		}
		if (currentDebtIndex < 0L)
		{
			currentDebtIndex = 0;
		}
		return GameplayData.SpinCostGet_Single(currentDebtIndex + 1) * 3;
	}

	// Token: 0x06000140 RID: 320
	public static BigInteger DeadlineReward_CoinsGet()
	{
		if (GameplayData.Instance == null)
		{
			return 0;
		}
		return GameplayData.DeadlineReward_CoinsGet(GameplayData.DebtIndexGet());
	}

	// Token: 0x06000141 RID: 321
	public static long DeadlineReward_CloverTickets(int extraRounds)
	{
		return GameplayData.CloverTickets_BonusRoundsLeft_Get() * (long)extraRounds * (long)PowerupScript.EvilDealBonusMultiplier();
	}

	// Token: 0x06000142 RID: 322
	public static long DeadlineReward_CloverTickets_Extras(bool rewardTime)
	{
		return ((long)PowerupScript.ModifiedPowerups_GetTicketsBonus() + PowerupScript.CloverPotTicketsBonus(true, rewardTime) + (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_Tickets, true) ? 3L : 0L)) * (long)PowerupScript.EvilDealBonusMultiplier();
	}

	// Token: 0x06000143 RID: 323
	private void _Debt_PrepareForSerialization()
	{
		this.debtIndex_ByteArray = this.debtIndex.ToByteArray();
		this.debtOutOfRangeMult_ByteArray = this.debtOutOfRangeMult.ToByteArray();
	}

	// Token: 0x06000144 RID: 324
	private void _Debt_RestoreFromSerialization()
	{
		this.debtIndex = this.BigIntegerFromByteArray(this.debtIndex_ByteArray, 0);
		this.debtOutOfRangeMult = this.BigIntegerFromByteArray(this.debtOutOfRangeMult_ByteArray, 6);
	}

	// Token: 0x06000145 RID: 325
	public static int RoundDeadlineTrail_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.roundDeadlineTrail;
	}

	// Token: 0x06000146 RID: 326
	public static void RoundDeadlineTrail_Set(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.roundDeadlineTrail = value;
		if (instance.roundDeadlineTrail < 0)
		{
			instance.roundDeadlineTrail = 0;
		}
	}

	// Token: 0x06000147 RID: 327
	public static void RoundDeadlineTrail_Increment()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.roundDeadlineTrail++;
	}

	// Token: 0x06000148 RID: 328
	public static int RoundsReallyPlayedGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.roundsReallyPlayed;
	}

	// Token: 0x06000149 RID: 329
	public static void RoundsReallyPlayedIncrement()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.roundsReallyPlayed++;
	}

	// Token: 0x0600014A RID: 330
	public static int RoundDeadlineTrail_AtDeadlineBegin_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.roundDeadlineTrail_AtDeadlineBegin;
	}

	// Token: 0x0600014B RID: 331
	public static void RoundDeadlineTrail_AtDeadlineBegin_Set(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.roundDeadlineTrail_AtDeadlineBegin = value;
		if (instance.roundDeadlineTrail_AtDeadlineBegin < 0)
		{
			instance.roundDeadlineTrail_AtDeadlineBegin = 0;
		}
	}

	// Token: 0x0600014C RID: 332
	public static void RoundDeadlineTrail_AtDeadlineBegin_CheckpointSet()
	{
		GameplayData.RoundDeadlineTrail_AtDeadlineBegin_Set(GameplayData.RoundDeadlineTrail_Get());
	}

	// Token: 0x0600014D RID: 333
	public static int RoundsLeftToDeadline()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance.roundOfDeadline - instance.roundDeadlineTrail;
	}

	// Token: 0x0600014E RID: 334
	public static int RoundsOfDeadline_TotalGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance.roundOfDeadline - instance.roundDeadlineTrail_AtDeadlineBegin;
	}

	// Token: 0x0600014F RID: 335
	public static int RoundsOfDeadline_PlayedGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance.roundDeadlineTrail - instance.roundDeadlineTrail_AtDeadlineBegin;
	}

	// Token: 0x06000150 RID: 336
	public static int RoundOfDeadlineGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance.roundOfDeadline;
	}

	// Token: 0x06000151 RID: 337
	public static int _GetDebtRoundDeadline_NextIncrement()
	{
		RunModifierScript.Identifier identifier = GameplayData.RunModifier_GetCurrent();
		if (identifier == RunModifierScript.Identifier.smallRoundsMoreRounds)
		{
			return 7;
		}
		if (identifier == RunModifierScript.Identifier.oneRoundPerDeadline)
		{
			return 1;
		}
		return 3;
	}

	// Token: 0x06000152 RID: 338
	public static void DeadlineRoundsIncrement_Manual(int ammount)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.DeadlineRoundsIncrement_Set(instance.roundOfDeadline + ammount);
		if (ammount > 0 && GameplayMaster.DeathCountdownHasStarted())
		{
			GameplayMaster.DeathCountdownResetRequest(true);
		}
	}

	// Token: 0x06000153 RID: 339
	public static void DeadlineRoundsIncrement()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.DeadlineRoundsIncrement_Set(instance.roundOfDeadline + GameplayData._GetDebtRoundDeadline_NextIncrement());
	}

	// Token: 0x06000154 RID: 340
	public static void DeadlineRoundsIncrement_Set(int ammount)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.roundOfDeadline = ammount;
	}

	// Token: 0x06000155 RID: 341
	public static bool AreWeOverTheDebtRange(BigInteger debtIndex)
	{
		return GameplayData.Instance == null || GameplayData.debtsInRange == null || GameplayData.debtsInRange.Length == 0 || debtIndex >= (long)GameplayData.debtsInRange.Length;
	}

	// Token: 0x06000156 RID: 342
	public static BigInteger DebtOutOfRangeIndexAmmount(BigInteger debtIndex)
	{
		if (GameplayData.Instance == null)
		{
			return 0;
		}
		if (GameplayData.debtsInRange == null || GameplayData.debtsInRange.Length == 0)
		{
			return 0;
		}
		BigInteger bigInteger = debtIndex - (GameplayData.debtsInRange.Length - 1);
		if (bigInteger <= 0L)
		{
			return 0;
		}
		return bigInteger;
	}

	// Token: 0x06000157 RID: 343
	public static BigInteger DebtGetExt(BigInteger debtIndex)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1000;
		}
		bool flag = GameplayData.AreWeOverTheDebtRange(debtIndex);
		int num = debtIndex.CastToInt();
		BigInteger bigInteger;
		if (!flag)
		{
			if (debtIndex < 0L)
			{
				debtIndex = 0;
			}
			bigInteger = GameplayData.debtsInRange[num];
		}
		else
		{
			bigInteger = GameplayData.debtsInRange[GameplayData.debtsInRange.Length - 1];
			bigInteger *= instance.debtOutOfRangeMult;
		}
		bigInteger *= PowerupScript.SkeletonPiecesDebtIncreasePercentage();
		bigInteger /= 100;
		RunModifierScript.Identifier identifier = GameplayData.RunModifier_GetCurrent();
		if (identifier == RunModifierScript.Identifier.bigDebt)
		{
			bigInteger *= 2;
		}
		else if (identifier == RunModifierScript.Identifier.extraPacks)
		{
			bigInteger /= 2;
		}
		if (Data.game.RunModifier_HardcoreMode_Get(GameplayData.RunModifier_GetCurrent()))
		{
			bigInteger = bigInteger * (100 + GameplayData.HardcoreMode_GetDebtIncreasePercentage()) / 100;
		}
		return bigInteger;
	}

	// Token: 0x06000158 RID: 344
	public static BigInteger HardcoreMode_GetDebtIncreasePercentage()
	{
		BigInteger bigInteger = GameplayData.DebtIndexGet();
		return 25 * ((bigInteger >= 0L) ? bigInteger : 0);
	}

	// Token: 0x06000159 RID: 345
	public static BigInteger DebtGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return GameplayData.debtsInRange[0];
		}
		return GameplayData.DebtGetExt(instance.debtIndex);
	}

	// Token: 0x0600015A RID: 346
	public static BigInteger DebtIndexGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.debtIndex;
	}

	// Token: 0x0600015B RID: 347
	public static void DebtIndexSet(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.debtIndex = value;
	}

	// Token: 0x0600015C RID: 348
	public static void DebtIndexAdd(BigInteger value)
	{
		GameplayData.DebtIndexSet(GameplayData.DebtIndexGet() + value);
	}

	// Token: 0x0600015D RID: 349
	public static BigInteger DebtMissingGet()
	{
		if (GameplayData.Instance == null)
		{
			return GameplayData.debtsInRange[0];
		}
		BigInteger bigInteger = GameplayData.DebtGet() - GameplayData.DepositGet();
		if (bigInteger < 0L)
		{
			bigInteger = 0;
		}
		return bigInteger;
	}

	// Token: 0x0600015E RID: 350
	public static BigInteger DebtOutOfRangeMultiplier_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 6;
		}
		return instance.debtOutOfRangeMult;
	}

	// Token: 0x0600015F RID: 351
	public static void DebtOutOfRangeMultiplier_Set(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.debtOutOfRangeMult = value;
	}

	// Token: 0x06000160 RID: 352
	public static bool SkeletonIsCompletedGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.skeletonIsCompleted;
	}

	// Token: 0x06000161 RID: 353
	public static void SkeletonIsCompletedSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.skeletonIsCompleted = true;
	}

	// Token: 0x06000162 RID: 354
	public static int SpinsLeftGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.spinsLeft;
	}

	// Token: 0x06000163 RID: 355
	public static void SpinsLeftSet(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.spinsLeft = n;
		if (instance.spinsLeft < 0)
		{
			instance.spinsLeft = 0;
		}
	}

	// Token: 0x06000164 RID: 356
	public static void SpinsLeftAdd(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.SpinsLeftSet(instance.spinsLeft + n);
	}

	// Token: 0x06000165 RID: 357
	public static void SpinConsume()
	{
		GameplayData.SpinsLeftAdd(-1);
	}

	// Token: 0x06000166 RID: 358
	public static int SpinsDoneInARun_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.spinsDoneInARun;
	}

	// Token: 0x06000167 RID: 359
	public static void SpinsDoneInARun_Increment()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.spinsDoneInARun++;
	}

	// Token: 0x06000168 RID: 360
	public static int ExtraSpinsGet(bool usePowerups)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		int num = instance.extraSpins;
		if (usePowerups)
		{
			num += PowerupScript.CatTreatsBonusGet(true);
			if (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Bad_2LessSpin, true))
			{
				num -= 2;
			}
			if (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Bad_3LessSpins, true))
			{
				num -= 3;
			}
			if (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_MoreSpins, true))
			{
				num += 2;
			}
		}
		return num;
	}

	// Token: 0x06000169 RID: 361
	public static void ExtraSpinsSet(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.extraSpins = n;
		if (instance.extraSpins < 0)
		{
			instance.extraSpins = 0;
		}
	}

	// Token: 0x0600016A RID: 362
	public static void ExtraSpinsAdd(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.ExtraSpinsSet(instance.extraSpins + n);
	}

	// Token: 0x0600016B RID: 363
	public static BigInteger SpinCostGet_Single()
	{
		return GameplayData.SpinCostGet_Single(GameplayData.DebtIndexGet());
	}

	// Token: 0x0600016C RID: 364
	public static BigInteger SpinCostGet_Single(BigInteger debtIndex)
	{
		BigInteger bigInteger = 2 * debtIndex;
		bigInteger += bigInteger * (debtIndex / 5);
		if (bigInteger < 1L)
		{
			bigInteger = 1;
		}
		return bigInteger;
	}

	// Token: 0x0600016D RID: 365
	public static int GetHypotehticalMaxSpinsBuyable()
	{
		if (GameplayData.Instance == null)
		{
			return 0;
		}
		BigInteger bigInteger4 = GameplayData.CoinsGet();
		BigInteger bigInteger2 = GameplayData.SpinCostGet_Single();
		BigInteger bigInteger3 = bigInteger4 / bigInteger2;
		int num = GameplayData.MaxSpins_Get();
		if (bigInteger3 > (long)num)
		{
			bigInteger3 = num;
		}
		return bigInteger3.CastToInt();
	}

	// Token: 0x0600016E RID: 366
	public static int GetHypotehticalMidSpinsBuyable()
	{
		return Mathf.FloorToInt((float)(GameplayData.GetHypotehticalMaxSpinsBuyable() / 2));
	}

	// Token: 0x0600016F RID: 367
	public static BigInteger SpinCostMax_Get()
	{
		return GameplayData.GetHypotehticalMaxSpinsBuyable() * GameplayData.SpinCostGet_Single();
	}

	// Token: 0x06000170 RID: 368
	public static BigInteger SpinCostMid_Get()
	{
		return GameplayData.GetHypotehticalMidSpinsBuyable() * GameplayData.SpinCostGet_Single();
	}

	// Token: 0x06000171 RID: 369
	public static void LastBet_IsSmallSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.lastBetIsSmall = true;
	}

	// Token: 0x06000172 RID: 370
	public static void LastBet_IsBigSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.lastBetIsSmall = false;
	}

	// Token: 0x06000173 RID: 371
	public static bool LastBet_IsSmallGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance == null || instance.lastBetIsSmall;
	}

	// Token: 0x06000174 RID: 372
	public static bool LastBet_IsBigGet()
	{
		return !GameplayData.LastBet_IsSmallGet();
	}

	// Token: 0x06000175 RID: 373
	public static int MaxSpins_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.maxSpins;
	}

	// Token: 0x06000176 RID: 374
	public static void MaxSpins_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.maxSpins = n;
		if (instance.maxSpins < 1)
		{
			instance.maxSpins = 1;
		}
	}

	// Token: 0x06000177 RID: 375
	public static void MaxSpins_Add(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.MaxSpins_Set(instance.maxSpins + n);
	}

	// Token: 0x06000178 RID: 376
	public static long SmallBetPickCount()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._smallBetsPickedCounter;
	}

	// Token: 0x06000179 RID: 377
	public static long BigBetPickCount()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._bigBetsPickedCounter;
	}

	// Token: 0x0600017A RID: 378
	public static void SmallAndBigBet_CountIncrease(int smallBet_AddThisNum, int bigBet_AddThisNum)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._smallBetsPickedCounter += (long)smallBet_AddThisNum;
		instance._bigBetsPickedCounter += (long)bigBet_AddThisNum;
	}

	// Token: 0x0600017B RID: 379
	public static int SpinsWithoutReward_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.spinsWithoutReward;
	}

	// Token: 0x0600017C RID: 380
	public static void SpinsWithoutReward_Increase()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.spinsWithoutReward++;
	}

	// Token: 0x0600017D RID: 381
	public static void SpinsWithoutReward_Reset()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.spinsWithoutReward = 0;
	}

	// Token: 0x0600017E RID: 382
	public static int ConsecutiveSpinsWithout5PlusPatterns_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.spinsWithout5PlusPatterns;
	}

	// Token: 0x0600017F RID: 383
	public static void ConsecutiveSpinsWithout5PlusPatterns_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.spinsWithout5PlusPatterns = n;
	}

	// Token: 0x06000180 RID: 384
	public static int ConsecutiveSpinsWithDiamondTreasureOrSeven_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.consecutiveSpinsWithDiamTreasSevens;
	}

	// Token: 0x06000181 RID: 385
	public static void ConsecutiveSpinsWithDiamondTreasureOrSeven_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.consecutiveSpinsWithDiamTreasSevens = n;
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000182 RID: 386
	// (set) Token: 0x06000183 RID: 387
	public static long JackpotsScoredCounter
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0L;
			}
			return instance._jackpotsScoredCounter;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._jackpotsScoredCounter = value;
		}
	}

	// Token: 0x06000184 RID: 388
	public static long SpinsWithAtLeast1Jackpot_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._spinsWithAtleast1Jackpot;
	}

	// Token: 0x06000185 RID: 389
	public static void SpinsWithAtLeast1Jackpot_Set(long n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._spinsWithAtleast1Jackpot = n;
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x06000186 RID: 390
	// (set) Token: 0x06000187 RID: 391
	public static int SlotInitialLuckRndOffset
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._slotInitialLuckRndOffset;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._slotInitialLuckRndOffset = value;
		}
	}

	// Token: 0x1700000A RID: 10
	// (get) Token: 0x06000188 RID: 392
	// (set) Token: 0x06000189 RID: 393
	public static int SlotOccasionalLuckSpinN
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._slotOccasionalLuckSpinN;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._slotOccasionalLuckSpinN = value;
		}
	}

	// Token: 0x0600018A RID: 394
	public static float LuckGet()
	{
		if (GameplayData.Instance == null)
		{
			return 0f;
		}
		return 0f + GameplayData.ExtraLuck_GetTotal() + PowerupScript.CrystalLuckIncreaseGet(true);
	}

	// Token: 0x0600018B RID: 395
	private static GameplayData.ExtraLuckEntry _ExtraLuckEntryFind(string tag)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return null;
		}
		GameplayData._ExtraLuckEnsureArray(instance);
		for (int i = 0; i < instance.extraLuckEntries.Length; i++)
		{
			if (instance.extraLuckEntries[i].tag == tag)
			{
				return instance.extraLuckEntries[i];
			}
		}
		return null;
	}

	// Token: 0x0600018C RID: 396
	private static GameplayData.ExtraLuckEntry _ExtraLuckEntryFindEmpty()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return null;
		}
		GameplayData._ExtraLuckEnsureArray(instance);
		for (int i = 0; i < instance.extraLuckEntries.Length; i++)
		{
			if (instance.extraLuckEntries[i].tag == null || instance.extraLuckEntries[i].tag == "")
			{
				return instance.extraLuckEntries[i];
			}
		}
		return null;
	}

	// Token: 0x0600018D RID: 397
	private static void _ExtraLuckEnsureArray(GameplayData _inst)
	{
		if (_inst.extraLuckEntries == null)
		{
			_inst.extraLuckEntries = new GameplayData.ExtraLuckEntry[20];
			for (int i = 0; i < _inst.extraLuckEntries.Length; i++)
			{
				if (_inst.extraLuckEntries[i] == null)
				{
					_inst.extraLuckEntries[i] = new GameplayData.ExtraLuckEntry();
				}
			}
			return;
		}
		if (_inst.extraLuckEntries.Length != 20)
		{
			Debug.LogWarning("ExtraLuckEntries array has wrong size. Has the game been updated? Resizing array to " + 20.ToString());
			Array.Resize<GameplayData.ExtraLuckEntry>(ref _inst.extraLuckEntries, 20);
			return;
		}
	}

	// Token: 0x0600018E RID: 398
	public static float ExtraLuck_GetTotal()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		GameplayData._ExtraLuckEnsureArray(instance);
		float num = 0f;
		foreach (GameplayData.ExtraLuckEntry extraLuckEntry in instance.extraLuckEntries)
		{
			if (extraLuckEntry != null && extraLuckEntry.tag != null && !(extraLuckEntry.tag == "") && extraLuckEntry.spinsLeft > 0)
			{
				num += extraLuckEntry.luck;
			}
		}
		return num;
	}

	// Token: 0x0600018F RID: 399
	public static void ExtraLuck_SetEntry(string tag, float luckValue, int desiredSpinsNumber, bool applyPowerupLuck)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData._ExtraLuckEnsureArray(instance);
		GameplayData.ExtraLuckEntry extraLuckEntry = GameplayData._ExtraLuckEntryFind(tag);
		if (extraLuckEntry == null)
		{
			extraLuckEntry = GameplayData._ExtraLuckEntryFindEmpty();
		}
		if (extraLuckEntry == null)
		{
			return;
		}
		if (applyPowerupLuck)
		{
			luckValue *= GameplayData.PowerupLuckGet();
		}
		extraLuckEntry.tag = tag;
		extraLuckEntry.luck = luckValue;
		extraLuckEntry.luckMax = luckValue;
		extraLuckEntry.spinsLeft = desiredSpinsNumber;
		extraLuckEntry.spinsLeftMax = desiredSpinsNumber;
	}

	// Token: 0x06000190 RID: 400
	public static void ExtraLuck_TickDownAll()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData._ExtraLuckEnsureArray(instance);
		for (int i = 0; i < instance.extraLuckEntries.Length; i++)
		{
			instance.extraLuckEntries[i].spinsLeft--;
			if (instance.extraLuckEntries[i].spinsLeft <= 0)
			{
				instance.extraLuckEntries[i].tag = null;
				instance.extraLuckEntries[i].luck = 0f;
				instance.extraLuckEntries[i].luckMax = 0f;
				instance.extraLuckEntries[i].spinsLeft = 0;
				instance.extraLuckEntries[i].spinsLeftMax = 0;
			}
			else
			{
				instance.extraLuckEntries[i].luck = instance.extraLuckEntries[i].luckMax * (float)instance.extraLuckEntries[i].spinsLeft / (float)instance.extraLuckEntries[i].spinsLeftMax;
			}
		}
	}

	// Token: 0x06000191 RID: 401
	public static void ExtraLuck_ResetAll()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData._ExtraLuckEnsureArray(instance);
		for (int i = 0; i < instance.extraLuckEntries.Length; i++)
		{
			instance.extraLuckEntries[i].tag = null;
			instance.extraLuckEntries[i].luck = 0f;
			instance.extraLuckEntries[i].luckMax = 0f;
			instance.extraLuckEntries[i].spinsLeft = 0;
			instance.extraLuckEntries[i].spinsLeftMax = 0;
		}
	}

	// Token: 0x06000192 RID: 402
	public static float PowerupLuckGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1f;
		}
		return instance.powerupLuck;
	}

	// Token: 0x06000193 RID: 403
	public static void PowerupLuckSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.powerupLuck = value;
		if (instance.powerupLuck < 0.5f)
		{
			instance.powerupLuck = 0.5f;
		}
	}

	// Token: 0x06000194 RID: 404
	public static void PowerupLuckAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.PowerupLuckSet(instance.powerupLuck + value);
	}

	// Token: 0x06000195 RID: 405
	public static void PowerupLuckReset()
	{
		GameplayData.PowerupLuckSet(1f);
	}

	// Token: 0x06000196 RID: 406
	public static float ActivationLuckGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1f;
		}
		return instance.activationLuck + PowerupScript.HorseShoesLuckGet() + PowerupScript.GoldenHorseShoe_RandomActivationChanceBonusGet(true);
	}

	// Token: 0x06000197 RID: 407
	public static void ActivationLuckSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.activationLuck = value;
		if (instance.activationLuck < 0.5f)
		{
			instance.activationLuck = 0.5f;
		}
	}

	// Token: 0x06000198 RID: 408
	public static void ActivationLuckAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.ActivationLuckSet(instance.activationLuck + value);
	}

	// Token: 0x06000199 RID: 409
	public static void ActivationLuckReset()
	{
		GameplayData.ActivationLuckSet(1f);
	}

	// Token: 0x0600019A RID: 410
	public static float StoreLuckGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1f;
		}
		return instance.storeLuck;
	}

	// Token: 0x0600019B RID: 411
	public static void StoreLuckSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.storeLuck = value;
		if (instance.storeLuck < 0.5f)
		{
			instance.storeLuck = 0.5f;
		}
	}

	// Token: 0x0600019C RID: 412
	public static void StoreLuckAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.StoreLuckSet(instance.storeLuck + value);
	}

	// Token: 0x0600019D RID: 413
	public static void StoreLuckReset()
	{
		GameplayData.StoreLuckSet(1f);
	}

	// Token: 0x0600019E RID: 414
	private void _Symbols_PrepareForSerialization()
	{
		if (this.symbolsAvailable_AsString == null)
		{
			this.symbolsAvailable_AsString = new string[this.symbolsAvailable.Count];
			for (int i = 0; i < this.symbolsAvailable.Count; i++)
			{
				this.symbolsAvailable_AsString[i] = PlatformDataMaster.EnumEntryToString<SymbolScript.Kind>(this.symbolsAvailable[i]);
			}
		}
		else if (this.symbolsAvailable_AsString.Length != this.symbolsAvailable.Count)
		{
			Array.Resize<string>(ref this.symbolsAvailable_AsString, this.symbolsAvailable.Count);
			for (int j = 0; j < this.symbolsAvailable.Count; j++)
			{
				this.symbolsAvailable_AsString[j] = PlatformDataMaster.EnumEntryToString<SymbolScript.Kind>(this.symbolsAvailable[j]);
			}
		}
		GameplayData._EnsureSymbolsArray(this);
		for (int k = 0; k < this.symbolsData.Length; k++)
		{
			this.symbolsData[k].extraValue_ByteArray = this.symbolsData[k].extraValue.ToByteArray();
		}
		this.allSymbolsMultiplier_ByteArray = this.allSymbolsMultiplier.ToByteArray();
	}

	// Token: 0x0600019F RID: 415
	private void _Symbols_RestoreFromSerialization()
	{
		if (this.symbolsAvailable_AsString != null && this.symbolsAvailable_AsString.Length != 0)
		{
			this.symbolsAvailable.Clear();
			for (int i = 0; i < this.symbolsAvailable_AsString.Length; i++)
			{
				SymbolScript.Kind kind = PlatformDataMaster.EnumEntryFromString<SymbolScript.Kind>(this.symbolsAvailable_AsString[i], SymbolScript.Kind.undefined);
				if (kind != SymbolScript.Kind.undefined && kind != SymbolScript.Kind.count)
				{
					this.symbolsAvailable.Add(kind);
				}
			}
		}
		GameplayData._EnsureSymbolsArray(this);
		for (int j = 0; j < this.symbolsData.Length; j++)
		{
			if (this.symbolsData[j].extraValue_ByteArray != null && this.symbolsData[j].extraValue_ByteArray.Length != 0)
			{
				this.symbolsData[j].extraValue = new BigInteger(this.symbolsData[j].extraValue_ByteArray);
			}
			else
			{
				this.symbolsData[j].extraValue = 0;
			}
		}
		this.allSymbolsMultiplier = this.BigIntegerFromByteArray(this.allSymbolsMultiplier_ByteArray, 1);
	}

	// Token: 0x060001A0 RID: 416
	public static List<SymbolScript.Kind> SymbolsAvailable_GetAll(bool sanityCheck)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return new List<SymbolScript.Kind>();
		}
		if (sanityCheck)
		{
			for (int i = 0; i < instance.symbolsAvailable.Count; i++)
			{
				SymbolScript.Kind kind = instance.symbolsAvailable[i];
				switch (kind)
				{
				case SymbolScript.Kind.lemon:
				case SymbolScript.Kind.cherry:
				case SymbolScript.Kind.clover:
				case SymbolScript.Kind.bell:
				case SymbolScript.Kind.diamond:
				case SymbolScript.Kind.coins:
				case SymbolScript.Kind.seven:
					break;
				case SymbolScript.Kind.six:
					Debug.LogError("SymbolsAvailable_GetAll(): Symbol 'six' is not accepted in the Available List");
					break;
				case SymbolScript.Kind.nine:
					Debug.LogError("SymbolsAvailable_GetAll(): Symbol 'nine' is not accepted in the Available List");
					break;
				default:
					Debug.LogError(string.Concat(new string[]
					{
						"Symbol '",
						kind.ToString(),
						"SymbolsAvailable_GetAll(): Symbol '",
						kind.ToString(),
						"' is not handled to be checked."
					}));
					break;
				}
			}
		}
		return instance.symbolsAvailable;
	}

	// Token: 0x060001A1 RID: 417
	public static void SymbolsAvilable_Add(SymbolScript.Kind kind)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		if (!instance.symbolsAvailable.Contains(kind))
		{
			instance.symbolsAvailable.Add(kind);
		}
	}

	// Token: 0x060001A2 RID: 418
	public static void SymbolsAvilable_Remove(SymbolScript.Kind kind)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.symbolsAvailable.Remove(kind);
	}

	// Token: 0x060001A3 RID: 419
	public static void SymbolsAvailable_OrderByBasicCoinsValue()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		for (int i = 0; i < instance.symbolsAvailable.Count; i++)
		{
			int num = i + 1;
			while (num < instance.symbolsAvailable.Count && num < instance.symbolsAvailable.Count)
			{
				float num3 = (float)GameplayData.Symbol_CoinsValue_GetBasic(instance.symbolsAvailable[i]);
				float num2 = (float)GameplayData.Symbol_CoinsValue_GetBasic(instance.symbolsAvailable[num]);
				if (num3 > num2)
				{
					SymbolScript.Kind kind = instance.symbolsAvailable[i];
					instance.symbolsAvailable[i] = instance.symbolsAvailable[num];
					instance.symbolsAvailable[num] = kind;
				}
				num++;
			}
		}
	}

	// Token: 0x060001A4 RID: 420
	private static void _EnsureSymbolsArray(GameplayData _inst)
	{
		int num = 9;
		if (_inst.symbolsData == null)
		{
			_inst.symbolsData = new GameplayData.SymbolData[num];
			for (int i = 0; i < num; i++)
			{
				SymbolScript.Kind kind = (SymbolScript.Kind)i;
				if (_inst.symbolsData[i] == null)
				{
					_inst.symbolsData[i] = new GameplayData.SymbolData();
					_inst.symbolsData[i].Initialize(PlatformDataMaster.EnumEntryToString<SymbolScript.Kind>(kind));
				}
			}
			return;
		}
		if (_inst.symbolsData.Length != num)
		{
			Debug.LogWarning("SymbolsData array has wrong size. Has the game been updated? Resizing array to " + num.ToString());
			Array.Resize<GameplayData.SymbolData>(ref _inst.symbolsData, num);
		}
		bool flag = false;
		for (int j = 0; j < num; j++)
		{
			string text = PlatformDataMaster.EnumEntryToString<SymbolScript.Kind>((SymbolScript.Kind)j);
			if (_inst.symbolsData[j] == null || !(_inst.symbolsData[j].symbolKindAsString == text))
			{
				bool flag2 = false;
				for (int k = j + 1; k < num; k++)
				{
					if (_inst.symbolsData[k] != null && _inst.symbolsData[k].symbolKindAsString == text)
					{
						GameplayData.SymbolData symbolData = _inst.symbolsData[j];
						_inst.symbolsData[j] = _inst.symbolsData[k];
						_inst.symbolsData[k] = symbolData;
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					if (_inst.symbolsData[j] == null)
					{
						_inst.symbolsData[j] = new GameplayData.SymbolData();
						_inst.symbolsData[j].Initialize(text);
						Debug.LogWarning("Warning! SymbolData for " + text + " was not found. Created new data.");
					}
					else
					{
						bool flag3 = false;
						for (int l = j + 1; l < num; l++)
						{
							if (_inst.symbolsData[l] == null)
							{
								_inst.symbolsData[l] = _inst.symbolsData[j];
								_inst.symbolsData[j] = null;
								flag3 = true;
								break;
							}
						}
						if (flag3)
						{
							j--;
						}
						else
						{
							flag = true;
							_inst.symbolsData[j] = null;
							j--;
						}
					}
				}
			}
		}
		if (flag)
		{
			Debug.LogWarning("Some Symbols Run-Data might be lost. Ctrl+Tab to close, or just wait a few seconds.");
		}
	}

	// Token: 0x060001A5 RID: 421
	private static GameplayData.SymbolData _SymbolDataFind(SymbolScript.Kind kind)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return null;
		}
		GameplayData._EnsureSymbolsArray(instance);
		if (kind < SymbolScript.Kind.lemon || kind >= (SymbolScript.Kind)instance.symbolsData.Length)
		{
			return null;
		}
		return instance.symbolsData[(int)kind];
	}

	// Token: 0x060001A6 RID: 422
	public static int Symbol_CoinsValue_GetBasic(SymbolScript.Kind kind)
	{
		int num;
		switch (kind)
		{
		case SymbolScript.Kind.lemon:
			num = 2;
			break;
		case SymbolScript.Kind.cherry:
			num = 2;
			break;
		case SymbolScript.Kind.clover:
			num = 3;
			break;
		case SymbolScript.Kind.bell:
			num = 3;
			break;
		case SymbolScript.Kind.diamond:
			num = 5;
			break;
		case SymbolScript.Kind.coins:
			num = 5;
			break;
		case SymbolScript.Kind.seven:
			num = 7;
			break;
		case SymbolScript.Kind.six:
			num = 0;
			break;
		case SymbolScript.Kind.nine:
			num = 0;
			break;
		default:
			Debug.LogError("Cannot get pattern value. Symbol kind is not recognized.");
			num = 0;
			break;
		}
		return num;
	}

	// Token: 0x060001A7 RID: 423
	public static BigInteger Symbol_CoinsValueExtra_Get(SymbolScript.Kind kind)
	{
		GameplayData.SymbolData symbolData = GameplayData._SymbolDataFind(kind);
		if (symbolData == null)
		{
			return 0;
		}
		return symbolData.extraValue;
	}

	// Token: 0x060001A8 RID: 424
	public static void Symbol_CoinsValueExtra_Set(SymbolScript.Kind kind, BigInteger value)
	{
		GameplayData.SymbolData symbolData = GameplayData._SymbolDataFind(kind);
		if (symbolData == null)
		{
			return;
		}
		symbolData.extraValue = value;
		int num = -GameplayData.Symbol_CoinsValue_GetBasic(kind) / 2;
		if (symbolData.extraValue < (long)num)
		{
			symbolData.extraValue = num;
		}
	}

	// Token: 0x060001A9 RID: 425
	public static void Symbol_CoinsValueExtra_Add(SymbolScript.Kind kind, BigInteger value)
	{
		GameplayData.Symbol_CoinsValueExtra_Set(kind, GameplayData.Symbol_CoinsValueExtra_Get(kind) + value);
	}

	// Token: 0x060001AA RID: 426
	public static void Symbol_CoinsValueExtra_Reset(SymbolScript.Kind kind)
	{
		GameplayData.Symbol_CoinsValueExtra_Set(kind, 0);
	}

	// Token: 0x060001AB RID: 427
	public static BigInteger Symbol_CoinsOverallValue_Get(SymbolScript.Kind kind)
	{
		BigInteger bigInteger = GameplayData.Symbol_CoinsValue_GetBasic(kind) + GameplayData.Symbol_CoinsValueExtra_Get(kind);
		bigInteger += PowerupScript.PoopBeetle_ExtraCoinsForAllSymbolsGet(true, kind);
		bigInteger += PowerupScript.ShroomsBonusGet(kind);
		bigInteger += PowerupScript.ConsolationPrizeBonusGet(kind);
		bigInteger += PowerupScript.SteamLocomotive_SymbolsBonus_Get(kind);
		bigInteger += PowerupScript.CigarettesGetSymbolBonus(kind);
		bigInteger += PowerupScript.VineShroomsBonusGet_Min0(kind);
		bigInteger += PowerupScript.GiantShroom_MultiplierBonusGet(kind);
		bigInteger += PowerupScript.CrystalSkullMultiplierGet(kind);
		if (bigInteger < 1L)
		{
			bigInteger = 1;
		}
		return bigInteger;
	}

	// Token: 0x060001AC RID: 428
	public static float Symbol_Chance_Get(SymbolScript.Kind kind, bool considerPowerups, bool considerScratchAndWin)
	{
		GameplayData.SymbolData symbolData = GameplayData._SymbolDataFind(kind);
		if (symbolData == null)
		{
			return 1f;
		}
		float num = symbolData.spawnChance;
		if (!considerPowerups)
		{
			return num;
		}
		switch (kind)
		{
		case SymbolScript.Kind.lemon:
			num += PowerupScript.LemonPicture_ChanceIncreaseGet(true);
			if (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_LemonCherryManifest, true))
			{
				num += 1.6f;
			}
			break;
		case SymbolScript.Kind.cherry:
			num += PowerupScript.CherryPicture_ChanceIncreaseGet(true);
			if (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_LemonCherryManifest, true))
			{
				num += 1.6f;
			}
			break;
		case SymbolScript.Kind.clover:
			num += PowerupScript.CloverPicture_ChanceIncreaseGet(true);
			if (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_CloverBellManifest, true))
			{
				num += 1.6f;
			}
			break;
		case SymbolScript.Kind.bell:
			num += PowerupScript.BellPicture_ChanceIncreaseGet(true);
			if (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_CloverBellManifest, true))
			{
				num += 1.6f;
			}
			break;
		case SymbolScript.Kind.diamond:
			num += PowerupScript.DiamondPicture_ChanceIncreaseGet(true);
			if (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_DiamondChestsManifest, true))
			{
				num += 1.6f;
			}
			break;
		case SymbolScript.Kind.coins:
			num += PowerupScript.CoinsPicture_ChanceIncreaseGet(true);
			if (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_DiamondChestsManifest, true))
			{
				num += 1.6f;
			}
			break;
		case SymbolScript.Kind.seven:
			num += PowerupScript.SevenPicture_ChanceIncreaseGet(true);
			if (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_SevenManifest, true))
			{
				num += 1.6f;
			}
			break;
		case SymbolScript.Kind.six:
			return 0f;
		case SymbolScript.Kind.nine:
			return 0f;
		default:
			Debug.LogError("Cannot get pattern Chance value. Symbol kind is not handled: " + kind.ToString());
			return 0f;
		}
		if (considerScratchAndWin)
		{
			num += PowerupScript.ScratchAndWin_ChanceBonusGet(true, kind);
		}
		if (PowerupScript.ExpiredMeds_SettingChancesToZeroForSymbol(kind, true))
		{
			num = 0f;
		}
		return Mathf.Max(0f, num);
	}

	// Token: 0x060001AD RID: 429
	public static float Symbol_Chance_GetAsPercentage(SymbolScript.Kind kind, bool considerPowerups, bool considerScratchAndWin)
	{
		float num = 0f;
		float num2 = GameplayData.Symbol_Chance_Get(kind, considerPowerups, considerScratchAndWin);
		List<SymbolScript.Kind> list = GameplayData.SymbolsAvailable_GetAll(false);
		for (int i = 0; i < list.Count; i++)
		{
			num += GameplayData.Symbol_Chance_Get(list[i], considerPowerups, considerScratchAndWin);
		}
		return num2 * 100f / num;
	}

	// Token: 0x060001AE RID: 430
	public static void Symbol_Chance_Set(SymbolScript.Kind kind, float value)
	{
		GameplayData.SymbolData symbolData = GameplayData._SymbolDataFind(kind);
		if (symbolData == null)
		{
			return;
		}
		symbolData.spawnChance = value;
		if (symbolData.spawnChance < 0f)
		{
			symbolData.spawnChance = 0f;
		}
	}

	// Token: 0x060001AF RID: 431
	public static void Symbol_Chance_Add(SymbolScript.Kind kind, float value)
	{
		GameplayData.SymbolData symbolData = GameplayData._SymbolDataFind(kind);
		if (symbolData == null)
		{
			return;
		}
		GameplayData.Symbol_Chance_Set(kind, symbolData.spawnChance + value);
	}

	// Token: 0x060001B0 RID: 432
	public static float Symbol_Chance_GetBasic(SymbolScript.Kind kind)
	{
		switch (kind)
		{
		case SymbolScript.Kind.lemon:
			return 1.3f;
		case SymbolScript.Kind.cherry:
			return 1.3f;
		case SymbolScript.Kind.clover:
			return 1f;
		case SymbolScript.Kind.bell:
			return 1f;
		case SymbolScript.Kind.diamond:
			return 0.8f;
		case SymbolScript.Kind.coins:
			return 0.8f;
		case SymbolScript.Kind.seven:
			return 0.5f;
		case SymbolScript.Kind.six:
			return 0f;
		case SymbolScript.Kind.nine:
			return 0f;
		default:
			Debug.LogError("Cannot get pattern Chance value. Symbol kind is not handled: " + kind.ToString());
			return 0f;
		}
	}

	// Token: 0x060001B1 RID: 433
	public static SymbolScript.Kind Symbol_GetRandom_BasedOnSymbolChance()
	{
		List<SymbolScript.Kind> list = GameplayData.SymbolsAvailable_GetAll(false);
		float num = 0f;
		for (int i = 0; i < list.Count; i++)
		{
			float num2 = GameplayData.Symbol_Chance_Get(list[i], true, true);
			num += num2;
		}
		float num3 = R.Rng_SymbolsChance.Range(0f, num);
		num = 0f;
		for (int j = 0; j < list.Count; j++)
		{
			float num4 = GameplayData.Symbol_Chance_Get(list[j], true, true);
			num += num4;
			if (num3 <= num)
			{
				return list[j];
			}
		}
		Debug.LogError("GameplayMaster: Symbol_GetBasedOnSymbolChance: code shouldn'treach here!");
		return list[R.Rng_SymbolsChance.Range(0, list.Count)];
	}

	// Token: 0x060001B2 RID: 434
	public static void SymbolsValueList_Order()
	{
		GameplayData._symbolsOrderedByHighestValueToLowest.Clear();
		List<SymbolScript.Kind> list = GameplayData.SymbolsAvailable_GetAll(false);
		for (int i = 0; i < list.Count; i++)
		{
			GameplayData._symbolsOrderedByHighestValueToLowest.Add(list[i]);
		}
		for (int j = 0; j < GameplayData._symbolsOrderedByHighestValueToLowest.Count; j++)
		{
			SymbolScript.Kind kind = GameplayData._symbolsOrderedByHighestValueToLowest[j];
			BigInteger bigInteger = GameplayData.Symbol_CoinsOverallValue_Get(kind);
			for (int k = j + 1; k < GameplayData._symbolsOrderedByHighestValueToLowest.Count; k++)
			{
				SymbolScript.Kind kind2 = GameplayData._symbolsOrderedByHighestValueToLowest[k];
				BigInteger bigInteger2 = GameplayData.Symbol_CoinsOverallValue_Get(kind2);
				if (bigInteger < bigInteger2)
				{
					GameplayData._symbolsOrderedByHighestValueToLowest[j] = kind2;
					GameplayData._symbolsOrderedByHighestValueToLowest[k] = kind;
					kind = kind2;
					bigInteger = bigInteger2;
				}
			}
		}
		GameplayData._mostValuableSymbols.Clear();
		GameplayData._mostValuableSymbols.Add(GameplayData._symbolsOrderedByHighestValueToLowest[0]);
		BigInteger bigInteger3 = GameplayData.Symbol_CoinsOverallValue_Get(GameplayData._symbolsOrderedByHighestValueToLowest[0]);
		int num = 0;
		while (num < GameplayData._symbolsOrderedByHighestValueToLowest.Count && !(GameplayData.Symbol_CoinsOverallValue_Get(GameplayData._symbolsOrderedByHighestValueToLowest[num]) < bigInteger3))
		{
			GameplayData._mostValuableSymbols.Add(GameplayData._symbolsOrderedByHighestValueToLowest[num]);
			num++;
		}
		GameplayData._leastValuableSymbols.Clear();
		GameplayData._leastValuableSymbols.Add(GameplayData._symbolsOrderedByHighestValueToLowest[GameplayData._symbolsOrderedByHighestValueToLowest.Count - 1]);
		bigInteger3 = GameplayData.Symbol_CoinsOverallValue_Get(GameplayData._symbolsOrderedByHighestValueToLowest[GameplayData._symbolsOrderedByHighestValueToLowest.Count - 1]);
		int num2 = GameplayData._symbolsOrderedByHighestValueToLowest.Count - 2;
		while (num2 >= 0 && !(GameplayData.Symbol_CoinsOverallValue_Get(GameplayData._symbolsOrderedByHighestValueToLowest[num2]) > bigInteger3))
		{
			GameplayData._leastValuableSymbols.Add(GameplayData._symbolsOrderedByHighestValueToLowest[num2]);
			num2--;
		}
	}

	// Token: 0x060001B3 RID: 435
	public static List<SymbolScript.Kind> SymbolsValueList_Get()
	{
		return GameplayData._symbolsOrderedByHighestValueToLowest;
	}

	// Token: 0x060001B4 RID: 436
	public static List<SymbolScript.Kind> MostValuableSymbols_GetList()
	{
		return GameplayData._mostValuableSymbols;
	}

	// Token: 0x060001B5 RID: 437
	public static List<SymbolScript.Kind> LeastValuableSymbols_GetList()
	{
		return GameplayData._leastValuableSymbols;
	}

	// Token: 0x060001B6 RID: 438
	public static void SymbolsChanceList_Order()
	{
		GameplayData._symbolsOrderedByHighestChanceToLowest.Clear();
		List<SymbolScript.Kind> list = GameplayData.SymbolsAvailable_GetAll(false);
		for (int i = 0; i < list.Count; i++)
		{
			GameplayData._symbolsOrderedByHighestChanceToLowest.Add(list[i]);
		}
		for (int j = 0; j < GameplayData._symbolsOrderedByHighestChanceToLowest.Count; j++)
		{
			SymbolScript.Kind kind = GameplayData._symbolsOrderedByHighestChanceToLowest[j];
			float num = GameplayData.Symbol_Chance_Get(kind, true, false);
			for (int k = j + 1; k < GameplayData._symbolsOrderedByHighestChanceToLowest.Count; k++)
			{
				SymbolScript.Kind kind2 = GameplayData._symbolsOrderedByHighestChanceToLowest[k];
				float num2 = GameplayData.Symbol_Chance_Get(kind2, true, false);
				if (num < num2)
				{
					GameplayData._symbolsOrderedByHighestChanceToLowest[j] = kind2;
					GameplayData._symbolsOrderedByHighestChanceToLowest[k] = kind;
					kind = kind2;
					num = num2;
				}
			}
		}
		GameplayData._mostProbableSymbols.Clear();
		GameplayData._mostProbableSymbols.Add(GameplayData._symbolsOrderedByHighestChanceToLowest[0]);
		float num3 = GameplayData.Symbol_Chance_Get(GameplayData._symbolsOrderedByHighestChanceToLowest[0], true, false);
		int num4 = 1;
		while (num4 < GameplayData._symbolsOrderedByHighestChanceToLowest.Count && GameplayData.Symbol_Chance_Get(GameplayData._symbolsOrderedByHighestChanceToLowest[num4], true, false) >= num3)
		{
			GameplayData._mostProbableSymbols.Add(GameplayData._symbolsOrderedByHighestChanceToLowest[num4]);
			num4++;
		}
		GameplayData._leastProbableSymbols.Clear();
		GameplayData._leastProbableSymbols.Add(GameplayData._symbolsOrderedByHighestChanceToLowest[GameplayData._symbolsOrderedByHighestChanceToLowest.Count - 1]);
		num3 = GameplayData.Symbol_Chance_Get(GameplayData._symbolsOrderedByHighestChanceToLowest[GameplayData._symbolsOrderedByHighestChanceToLowest.Count - 1], true, false);
		int num5 = GameplayData._symbolsOrderedByHighestChanceToLowest.Count - 2;
		while (num5 >= 0 && GameplayData.Symbol_Chance_Get(GameplayData._symbolsOrderedByHighestChanceToLowest[num5], true, false) <= num3)
		{
			GameplayData._leastProbableSymbols.Add(GameplayData._symbolsOrderedByHighestValueToLowest[num5]);
			num5--;
		}
	}

	// Token: 0x060001B7 RID: 439
	public static List<SymbolScript.Kind> SymbolsChanceList_Get()
	{
		return GameplayData._symbolsOrderedByHighestChanceToLowest;
	}

	// Token: 0x060001B8 RID: 440
	public static List<SymbolScript.Kind> MostProbableSymbols_GetList()
	{
		return GameplayData._mostProbableSymbols;
	}

	// Token: 0x060001B9 RID: 441
	public static List<SymbolScript.Kind> LeastProbableSymbols_GetList()
	{
		return GameplayData._leastProbableSymbols;
	}

	// Token: 0x060001BA RID: 442
	public static float Symbol_ModifierChance_Get(SymbolScript.Kind symbolKind, SymbolScript.Modifier modifier)
	{
		GameplayData.SymbolData symbolData = GameplayData._SymbolDataFind(symbolKind);
		if (symbolData == null)
		{
			return 0f;
		}
		float num = 0f;
		float num2 = PowerupScript.Dealer_GetModChance(true);
		float num3 = PowerupScript.Capitalist_GetModChance(true);
		float num4 = PowerupScript.GoldenKingMida_GetModChance(true);
		float num5 = PowerupScript.PuppetPersonalTrainer_BonusGet(true);
		float num6 = PowerupScript.PuppetEletrician_BonusGet(true);
		float num7 = PowerupScript.PuppetFortuneTeller_BonusGet(true);
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		float num11 = 0f;
		float num12 = 0f;
		float num13 = 0f;
		if (PowerupScript.IsEquipped_Quick(PowerupScript.Identifier._999_TheBlood))
		{
			num11 += 0.15f;
			num12 += 0.05f;
			num13 += 0.15f;
		}
		if (PowerupScript.IsEquipped_Quick(PowerupScript.Identifier._999_TheBody))
		{
			num8 += 0.15f;
			num9 += 0.15f;
			num10 += 0.15f;
		}
		switch (symbolKind)
		{
		case SymbolScript.Kind.lemon:
			switch (modifier)
			{
			case SymbolScript.Modifier.instantReward:
				return symbolData.modifierChance01_InstantReward + PowerupScript.BoardgameC_Bricks_GetModChance(true) + num2 + num8 + num;
			case SymbolScript.Modifier.cloverTicket:
				return symbolData.modifierChance01_CloverTicket + PowerupScript.BoardgameM_Carriola_GetModChance(true) + num3 + num;
			case SymbolScript.Modifier.golden:
				return symbolData.modifierChance01_Golden + PowerupScript.GoldenSymbol_Lemon_GetModChance(true) + num4 + num;
			case SymbolScript.Modifier.repetition:
				return symbolData.modifierChance01_Repetition + PowerupScript.GeneralModCharm_Clicker_GetModChance(true) + num11 + num5 + num;
			case SymbolScript.Modifier.battery:
				return symbolData.modifierChance01_Battery + num6 + num;
			case SymbolScript.Modifier.chain:
				return symbolData.modifierChance01_Chain + num7 + num;
			default:
				Debug.LogError("Symbol_ModifierChance_Get: Modifier not recognized: " + modifier.ToString());
				return 0f;
			}
			break;
		case SymbolScript.Kind.cherry:
			switch (modifier)
			{
			case SymbolScript.Modifier.instantReward:
				return symbolData.modifierChance01_InstantReward + PowerupScript.BoardgameC_Wood_GetModChance(true) + num2 + num8 + num;
			case SymbolScript.Modifier.cloverTicket:
				return symbolData.modifierChance01_CloverTicket + PowerupScript.BoardgameM_Shoe_GetModChance(true) + num3 + num;
			case SymbolScript.Modifier.golden:
				return symbolData.modifierChance01_Golden + PowerupScript.GoldenSymbol_Cherry_GetModChance(true) + num4 + num;
			case SymbolScript.Modifier.repetition:
				return symbolData.modifierChance01_Repetition + PowerupScript.GeneralModCharm_Clicker_GetModChance(true) + num11 + num5 + num;
			case SymbolScript.Modifier.battery:
				return symbolData.modifierChance01_Battery + num6 + num;
			case SymbolScript.Modifier.chain:
				return symbolData.modifierChance01_Chain + num7 + num;
			default:
				Debug.LogError("Symbol_ModifierChance_Get: Modifier not recognized: " + modifier.ToString());
				return 0f;
			}
			break;
		case SymbolScript.Kind.clover:
			switch (modifier)
			{
			case SymbolScript.Modifier.instantReward:
				return symbolData.modifierChance01_InstantReward + PowerupScript.BoardgameC_Sheep_GetModChance(true) + num2 + num;
			case SymbolScript.Modifier.cloverTicket:
				return symbolData.modifierChance01_CloverTicket + PowerupScript.BoardgameM_Ditale_GetModChance(true) + num3 + num9 + num;
			case SymbolScript.Modifier.golden:
				return symbolData.modifierChance01_Golden + PowerupScript.GoldenSymbol_Clover_GetModChance(true) + num4 + num;
			case SymbolScript.Modifier.repetition:
				return symbolData.modifierChance01_Repetition + num5 + num;
			case SymbolScript.Modifier.battery:
				return symbolData.modifierChance01_Battery + PowerupScript.GeneralModCharm_CloverBellBattery_GetModChance(true) + num12 + num6 + num;
			case SymbolScript.Modifier.chain:
				return symbolData.modifierChance01_Chain + num7 + num;
			default:
				Debug.LogError("Symbol_ModifierChance_Get: Modifier not recognized: " + modifier.ToString());
				return 0f;
			}
			break;
		case SymbolScript.Kind.bell:
			switch (modifier)
			{
			case SymbolScript.Modifier.instantReward:
				return symbolData.modifierChance01_InstantReward + PowerupScript.BoardgameC_Wheat_GetModChance(true) + num2 + num;
			case SymbolScript.Modifier.cloverTicket:
				return symbolData.modifierChance01_CloverTicket + PowerupScript.BoardgameM_FerroDaStiro_GetModChance(true) + num3 + num9 + num;
			case SymbolScript.Modifier.golden:
				return symbolData.modifierChance01_Golden + PowerupScript.GoldenSymbol_Bell_GetModChance(true) + num4 + num;
			case SymbolScript.Modifier.repetition:
				return symbolData.modifierChance01_Repetition + num5 + num;
			case SymbolScript.Modifier.battery:
				return symbolData.modifierChance01_Battery + PowerupScript.GeneralModCharm_CloverBellBattery_GetModChance(true) + num12 + num6 + num;
			case SymbolScript.Modifier.chain:
				return symbolData.modifierChance01_Chain + num7 + num;
			default:
				Debug.LogError("Symbol_ModifierChance_Get: Modifier not recognized: " + modifier.ToString());
				return 0f;
			}
			break;
		case SymbolScript.Kind.diamond:
			switch (modifier)
			{
			case SymbolScript.Modifier.instantReward:
				return symbolData.modifierChance01_InstantReward + PowerupScript.BoardgameC_Stone_GetModChance(true) + num2 + num;
			case SymbolScript.Modifier.cloverTicket:
				return symbolData.modifierChance01_CloverTicket + PowerupScript.BoardgameM_Car_GetModChance(true) + num3 + num;
			case SymbolScript.Modifier.golden:
				return symbolData.modifierChance01_Golden + PowerupScript.GoldenSymbol_Diamond_GetModChance(true) + num4 + num10 + num;
			case SymbolScript.Modifier.repetition:
				return symbolData.modifierChance01_Repetition + num5 + num;
			case SymbolScript.Modifier.battery:
				return symbolData.modifierChance01_Battery + num6 + num;
			case SymbolScript.Modifier.chain:
				return symbolData.modifierChance01_Chain + PowerupScript.GeneralModCharm_CrystalBall_GetModChance(true) + num13 + num7 + num;
			default:
				Debug.LogError("Symbol_ModifierChance_Get: Modifier not recognized: " + modifier.ToString());
				return 0f;
			}
			break;
		case SymbolScript.Kind.coins:
			switch (modifier)
			{
			case SymbolScript.Modifier.instantReward:
				return symbolData.modifierChance01_InstantReward + PowerupScript.BoardgameC_Harbor_GetModChance(true) + num2 + num;
			case SymbolScript.Modifier.cloverTicket:
				return symbolData.modifierChance01_CloverTicket + PowerupScript.BoardgameM_Ship_GetModChance(true) + num3 + num;
			case SymbolScript.Modifier.golden:
				return symbolData.modifierChance01_Golden + PowerupScript.GoldenSymbol_Coins_GetModChance(true) + num4 + num10 + num;
			case SymbolScript.Modifier.repetition:
				return symbolData.modifierChance01_Repetition + num5 + num;
			case SymbolScript.Modifier.battery:
				return symbolData.modifierChance01_Battery + num6 + num;
			case SymbolScript.Modifier.chain:
				return symbolData.modifierChance01_Chain + PowerupScript.GeneralModCharm_CrystalBall_GetModChance(true) + num13 + num7 + num;
			default:
				Debug.LogError("Symbol_ModifierChance_Get: Modifier not recognized: " + modifier.ToString());
				return 0f;
			}
			break;
		case SymbolScript.Kind.seven:
			switch (modifier)
			{
			case SymbolScript.Modifier.instantReward:
				return symbolData.modifierChance01_InstantReward + PowerupScript.BoardgameC_Thief_GetModChance(true) + num2 + num;
			case SymbolScript.Modifier.cloverTicket:
				return symbolData.modifierChance01_CloverTicket + PowerupScript.BoardgameM_Hat_GetModChance(true) + num3 + num;
			case SymbolScript.Modifier.golden:
				return symbolData.modifierChance01_Golden + PowerupScript.GoldenSymbol_Seven_GetModChance(true) + num4 + num10 + num;
			case SymbolScript.Modifier.repetition:
				return symbolData.modifierChance01_Repetition + num5 + num;
			case SymbolScript.Modifier.battery:
				return symbolData.modifierChance01_Battery + num6 + num;
			case SymbolScript.Modifier.chain:
				return symbolData.modifierChance01_Chain + PowerupScript.GeneralModCharm_CrystalBall_GetModChance(true) + num13 + num7 + num;
			default:
				Debug.LogError("Symbol_ModifierChance_Get: Modifier not recognized: " + modifier.ToString());
				return 0f;
			}
			break;
		case SymbolScript.Kind.six:
			return 0f;
		case SymbolScript.Kind.nine:
			return 0f;
		default:
			Debug.LogError("Symbol_ModifierChance_Get: Symbol kind not recognized: " + symbolKind.ToString());
			return 0f;
		}
	}

	// Token: 0x060001BB RID: 443
	public static float Symbol_ModifierChance_GetAsPercentage(SymbolScript.Kind symbolKind, SymbolScript.Modifier modifier)
	{
		return GameplayData.Symbol_ModifierChance_Get(symbolKind, modifier) * 100f;
	}

	// Token: 0x060001BC RID: 444
	public static void Symbol_ModifierChance_Set(SymbolScript.Kind symbolKind, SymbolScript.Modifier modifier, float value)
	{
		GameplayData.SymbolData symbolData = GameplayData._SymbolDataFind(symbolKind);
		if (symbolData == null)
		{
			return;
		}
		switch (modifier)
		{
		case SymbolScript.Modifier.none:
			Debug.LogError("Symbol_ModifierChance_Set: Modifier is none. nonsense to set its chance");
			return;
		case SymbolScript.Modifier.cloverTicket:
			symbolData.modifierChance01_CloverTicket = Mathf.Max(0f, value);
			return;
		case SymbolScript.Modifier.golden:
			symbolData.modifierChance01_Golden = Mathf.Max(0f, value);
			return;
		case SymbolScript.Modifier.chain:
			symbolData.modifierChance01_Chain = Mathf.Max(0f, value);
			return;
		}
		Debug.LogError("Symbol_ModifierChance_Set: Modifier not recognized: " + modifier.ToString());
	}

	// Token: 0x060001BD RID: 445
	public static void Symbol_ModifierChance_Add(SymbolScript.Kind symbolKind, SymbolScript.Modifier modifier, float value)
	{
		GameplayData.SymbolData symbolData = GameplayData._SymbolDataFind(symbolKind);
		if (symbolData == null)
		{
			return;
		}
		switch (modifier)
		{
		case SymbolScript.Modifier.none:
			Debug.LogError("Symbol_ModifierChance_Add: Modifier is none. nonsense to add to its chance");
			return;
		case SymbolScript.Modifier.cloverTicket:
			GameplayData.Symbol_ModifierChance_Set(symbolKind, modifier, symbolData.modifierChance01_CloverTicket + value);
			return;
		case SymbolScript.Modifier.golden:
			GameplayData.Symbol_ModifierChance_Set(symbolKind, modifier, symbolData.modifierChance01_Golden + value);
			return;
		case SymbolScript.Modifier.chain:
			GameplayData.Symbol_ModifierChance_Set(symbolKind, modifier, symbolData.modifierChance01_Chain + value);
			return;
		}
		Debug.LogError("Symbol_ModifierChance_Add: Modifier not recognized: " + modifier.ToString());
	}

	// Token: 0x060001BE RID: 446
	public static SymbolScript.Modifier Symbol_Modifier_GetRandom(SymbolScript.Kind symbolKind)
	{
		if (symbolKind == SymbolScript.Kind.six)
		{
			return SymbolScript.Modifier.none;
		}
		if (symbolKind == SymbolScript.Kind.nine)
		{
			return SymbolScript.Modifier.none;
		}
		if (GameplayData._SymbolDataFind(symbolKind) == null)
		{
			return SymbolScript.Modifier.none;
		}
		for (int i = 0; i < GameplayData.modifierChanceFloats.Length; i++)
		{
			GameplayData.modifierChanceFloats[i] = R.Rng_SymbolsMod.Value;
		}
		GameplayData.modifierChanceFloats_SuccessThreshold[SymbolScript.ModifierGetArrayIndex(SymbolScript.Modifier.instantReward)] = GameplayData.Symbol_ModifierChance_Get(symbolKind, SymbolScript.Modifier.instantReward);
		GameplayData.modifierChanceFloats_SuccessThreshold[SymbolScript.ModifierGetArrayIndex(SymbolScript.Modifier.cloverTicket)] = GameplayData.Symbol_ModifierChance_Get(symbolKind, SymbolScript.Modifier.cloverTicket);
		GameplayData.modifierChanceFloats_SuccessThreshold[SymbolScript.ModifierGetArrayIndex(SymbolScript.Modifier.golden)] = GameplayData.Symbol_ModifierChance_Get(symbolKind, SymbolScript.Modifier.golden);
		GameplayData.modifierChanceFloats_SuccessThreshold[SymbolScript.ModifierGetArrayIndex(SymbolScript.Modifier.repetition)] = GameplayData.Symbol_ModifierChance_Get(symbolKind, SymbolScript.Modifier.repetition);
		GameplayData.modifierChanceFloats_SuccessThreshold[SymbolScript.ModifierGetArrayIndex(SymbolScript.Modifier.battery)] = GameplayData.Symbol_ModifierChance_Get(symbolKind, SymbolScript.Modifier.battery);
		GameplayData.modifierChanceFloats_SuccessThreshold[SymbolScript.ModifierGetArrayIndex(SymbolScript.Modifier.chain)] = GameplayData.Symbol_ModifierChance_Get(symbolKind, SymbolScript.Modifier.chain);
		int num = -1;
		float num2 = float.MaxValue;
		for (int j = 0; j < GameplayData.modifierChanceFloats.Length; j++)
		{
			if (GameplayData.modifierChanceFloats[j] < GameplayData.modifierChanceFloats_SuccessThreshold[j] && GameplayData.modifierChanceFloats[j] <= num2)
			{
				num = j;
				num2 = GameplayData.modifierChanceFloats[j];
			}
		}
		return SymbolScript.ModifierFromArrayIndex(num);
	}

	// Token: 0x060001BF RID: 447
	public static BigInteger AllSymbolsMultiplierGet(bool considerPowerups)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1;
		}
		BigInteger bigInteger = instance.allSymbolsMultiplier;
		if (considerPowerups)
		{
			bigInteger += PowerupScript.ModifiedPowerups_GetCount(PowerupScript.Modifier.symbolMultiplier) * 2 + PowerupScript.TarotDeckRewardGet(true) + PowerupScript.PentacleBonusGet(true) + PowerupScript.CloverPetSymbolsMultiplierBonus(true) + PowerupScript.VoiceMail_MultiplierBonusGet(true) + PowerupScript.Garbage_MultiplierBonusGet(true) + PowerupScript.AllIn_MultiplierBonusGet(true) + PowerupScript.DarkLotus_MultiplierBonus_Get(true) + PowerupScript.ShoppingCart_MultiplierBonusGet(true) + (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_SymbMult, true) ? 5 : 0) + PowerupScript.Calendar_GetSymbolsMultiplierBonus(true) + PowerupScript.RingBell_BonusGet() + PowerupScript.Hourglass_SymbolsMultiplierBonusGet(true);
		}
		if (bigInteger < 1L)
		{
			bigInteger = 1;
		}
		BigInteger bigInteger2 = 1;
		if (considerPowerups)
		{
			bigInteger2 *= PowerupScript.GabibbhMultiplierGet(true) * (long)PowerupScript.EvilDealBonusMultiplier();
		}
		if (bigInteger2 < 1L)
		{
			bigInteger2 = 1;
		}
		return bigInteger * bigInteger2;
	}

	// Token: 0x060001C0 RID: 448
	public static void AllSymbolsMultiplierSet(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.allSymbolsMultiplier = value;
		if (instance.allSymbolsMultiplier < 1L)
		{
			instance.allSymbolsMultiplier = 1;
		}
	}

	// Token: 0x060001C1 RID: 449
	public static void AllSymbolsMultiplierAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.AllSymbolsMultiplierSet(instance.allSymbolsMultiplier + value);
	}

	// Token: 0x060001C2 RID: 450
	public static void allSymbolsMultiplierReset()
	{
		GameplayData.AllSymbolsMultiplierSet(1);
	}

	// Token: 0x060001C3 RID: 451
	private void _Patterns_PrepareForSerialization()
	{
		if (this.patternsAvailable_AsString == null)
		{
			this.patternsAvailable_AsString = new string[this.patternsAvailable.Count];
			for (int i = 0; i < this.patternsAvailable.Count; i++)
			{
				this.patternsAvailable_AsString[i] = PlatformDataMaster.EnumEntryToString<PatternScript.Kind>(this.patternsAvailable[i]);
			}
		}
		else if (this.patternsAvailable_AsString.Length != this.patternsAvailable.Count)
		{
			Array.Resize<string>(ref this.patternsAvailable_AsString, this.patternsAvailable.Count);
			for (int j = 0; j < this.patternsAvailable.Count; j++)
			{
				this.patternsAvailable_AsString[j] = PlatformDataMaster.EnumEntryToString<PatternScript.Kind>(this.patternsAvailable[j]);
			}
		}
		GameplayData._EnsurePatternsArray(this);
		this.allPatternsMultiplier_ByteArray = this.allPatternsMultiplier.ToByteArray();
	}

	// Token: 0x060001C4 RID: 452
	private void _Patterns_RestoreFromSerialization()
	{
		if (this.patternsAvailable_AsString != null && this.patternsAvailable_AsString.Length != 0)
		{
			this.patternsAvailable.Clear();
			for (int i = 0; i < this.patternsAvailable_AsString.Length; i++)
			{
				PatternScript.Kind kind = PlatformDataMaster.EnumEntryFromString<PatternScript.Kind>(this.patternsAvailable_AsString[i], PatternScript.Kind.undefined);
				if (kind != PatternScript.Kind.undefined && kind != PatternScript.Kind.count)
				{
					this.patternsAvailable.Add(kind);
				}
			}
		}
		GameplayData._EnsurePatternsArray(this);
		this.allPatternsMultiplier = this.BigIntegerFromByteArray(this.allPatternsMultiplier_ByteArray, 1);
	}

	// Token: 0x060001C5 RID: 453
	public static List<PatternScript.Kind> PatternsAvailable_GetAll()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return new List<PatternScript.Kind>();
		}
		return instance.patternsAvailable;
	}

	// Token: 0x060001C6 RID: 454
	public static void PatternsAvailable_Add(PatternScript.Kind kind)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		if (!instance.patternsAvailable.Contains(kind))
		{
			instance.patternsAvailable.Add(kind);
		}
	}

	// Token: 0x060001C7 RID: 455
	public static void PatternsAvailable_Remove(PatternScript.Kind kind)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.patternsAvailable.Remove(kind);
	}

	// Token: 0x060001C8 RID: 456
	public static void PatternsAvailable_OrderByValue(bool keepJackpotAsLast)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		for (int i = 0; i < instance.patternsAvailable.Count; i++)
		{
			int num = i + 1;
			while (num < instance.patternsAvailable.Count && num < instance.patternsAvailable.Count)
			{
				double num3 = GameplayData.Pattern_Value_GetBasic(instance.patternsAvailable[i]);
				double num2 = GameplayData.Pattern_Value_GetBasic(instance.patternsAvailable[num]);
				if (num3 > num2)
				{
					PatternScript.Kind kind = instance.patternsAvailable[i];
					instance.patternsAvailable[i] = instance.patternsAvailable[num];
					instance.patternsAvailable[num] = kind;
				}
				num++;
			}
		}
		if (keepJackpotAsLast && instance.patternsAvailable.Contains(PatternScript.Kind.jackpot))
		{
			instance.patternsAvailable.Remove(PatternScript.Kind.jackpot);
			instance.patternsAvailable.Add(PatternScript.Kind.jackpot);
		}
	}

	// Token: 0x060001C9 RID: 457
	public static double Pattern_Value_GetBasic(PatternScript.Kind kind)
	{
		double num;
		switch (kind)
		{
		case PatternScript.Kind.jackpot:
			num = (double)(Master.instance.SCORE_PATTERNS_INSIDE_JACKPOT ? 10 : 20);
			break;
		case PatternScript.Kind.horizontal2:
			num = 0.5;
			break;
		case PatternScript.Kind.horizontal3:
			num = 1.0;
			break;
		case PatternScript.Kind.horizontal4:
			num = 2.0;
			break;
		case PatternScript.Kind.horizontal5:
			num = 3.0;
			break;
		case PatternScript.Kind.vertical2:
			num = 0.5;
			break;
		case PatternScript.Kind.vertical3:
			num = 1.0;
			break;
		case PatternScript.Kind.diagonal2:
			num = 0.5;
			break;
		case PatternScript.Kind.diagonal3:
			num = 1.0;
			break;
		case PatternScript.Kind.pyramid:
			num = 4.0;
			break;
		case PatternScript.Kind.pyramidInverted:
			num = 4.0;
			break;
		case PatternScript.Kind.triangle:
			num = 7.0;
			break;
		case PatternScript.Kind.triangleInverted:
			num = 7.0;
			break;
		case PatternScript.Kind.snakeUpDown:
			num = 7.0;
			break;
		case PatternScript.Kind.snakeDownUp:
			num = 7.0;
			break;
		case PatternScript.Kind.eye:
			num = 8.0;
			break;
		default:
			Debug.LogError("Cannot get pattern value. Pattern kind is not recognized.");
			num = 0.0;
			break;
		}
		return num;
	}

	// Token: 0x060001CA RID: 458
	private static void _EnsurePatternsArray(GameplayData _inst)
	{
		int num = 16;
		if (_inst.patternsData == null)
		{
			_inst.patternsData = new GameplayData.PatternData[num];
			for (int i = 0; i < num; i++)
			{
				PatternScript.Kind kind = (PatternScript.Kind)i;
				if (_inst.patternsData[i] == null)
				{
					_inst.patternsData[i] = new GameplayData.PatternData();
					_inst.patternsData[i].Initialize(PlatformDataMaster.EnumEntryToString<PatternScript.Kind>(kind));
				}
			}
		}
		if (_inst.patternsData.Length != num)
		{
			Debug.LogWarning("PatternsData array has wrong size. Has the game been updated? Resizing array to " + num.ToString());
			Array.Resize<GameplayData.PatternData>(ref _inst.patternsData, num);
		}
		bool flag = false;
		for (int j = 0; j < num; j++)
		{
			string text = PlatformDataMaster.EnumEntryToString<PatternScript.Kind>((PatternScript.Kind)j);
			if (_inst.patternsData[j] == null || !(_inst.patternsData[j].patternKindAsString == text))
			{
				bool flag2 = false;
				for (int k = j + 1; k < num; k++)
				{
					if (_inst.patternsData[k] != null && _inst.patternsData[k].patternKindAsString == text)
					{
						GameplayData.PatternData patternData = _inst.patternsData[j];
						_inst.patternsData[j] = _inst.patternsData[k];
						_inst.patternsData[k] = patternData;
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					if (_inst.patternsData[j] == null)
					{
						_inst.patternsData[j] = new GameplayData.PatternData();
						_inst.patternsData[j].Initialize(text);
						Debug.LogWarning("Warning! PatternData for " + text + " was not found. Created new data.");
					}
					else
					{
						bool flag3 = false;
						for (int l = j + 1; l < num; l++)
						{
							if (_inst.patternsData[l] == null)
							{
								_inst.patternsData[l] = _inst.patternsData[j];
								_inst.patternsData[j] = null;
								flag3 = true;
								break;
							}
						}
						if (flag3)
						{
							j--;
						}
						else
						{
							flag = true;
							_inst.patternsData[j] = null;
							j--;
						}
					}
				}
			}
		}
		if (flag)
		{
			Debug.LogWarning("Some Patterns Run-Data might be lost. Ctrl+Tab to close, or just wait a few seconds.");
		}
	}

	// Token: 0x060001CB RID: 459
	private static GameplayData.PatternData _PatternDataFind(PatternScript.Kind kind)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return null;
		}
		GameplayData._EnsurePatternsArray(instance);
		if (kind < PatternScript.Kind.jackpot || kind >= (PatternScript.Kind)instance.patternsData.Length)
		{
			return null;
		}
		return instance.patternsData[(int)kind];
	}

	// Token: 0x060001CC RID: 460
	public static double Pattern_ValueExtra_Get(PatternScript.Kind kind)
	{
		GameplayData.PatternData patternData = GameplayData._PatternDataFind(kind);
		if (patternData == null)
		{
			return 0.0;
		}
		return patternData.extraValue;
	}

	// Token: 0x060001CD RID: 461
	public static void Pattern_ValueExtra_Set(PatternScript.Kind kind, double value)
	{
		GameplayData.PatternData patternData = GameplayData._PatternDataFind(kind);
		if (patternData == null)
		{
			return;
		}
		patternData.extraValue = value;
		double num = -0.5 * GameplayData.Pattern_Value_GetBasic(kind);
		if (patternData.extraValue < num)
		{
			patternData.extraValue = num;
		}
	}

	// Token: 0x060001CE RID: 462
	public static void Pattern_ValueExtra_Add(PatternScript.Kind kind, double value)
	{
		GameplayData.PatternData patternData = GameplayData._PatternDataFind(kind);
		if (patternData == null)
		{
			return;
		}
		GameplayData.Pattern_ValueExtra_Set(kind, patternData.extraValue + value);
	}

	// Token: 0x060001CF RID: 463
	public static void Pattern_ValueExtra_Reset(PatternScript.Kind kind)
	{
		GameplayData.Pattern_ValueExtra_Set(kind, 0.0);
	}

	// Token: 0x060001D0 RID: 464
	public static double Pattern_ValueOverall_Get(PatternScript.Kind kind, bool includePowerups)
	{
		double num = GameplayData.Pattern_Value_GetBasic(kind);
		double num2 = num + GameplayData.Pattern_ValueExtra_Get(kind);
		if (!includePowerups)
		{
			if (num2 < 0.5)
			{
				num2 = 0.5;
			}
			return num2;
		}
		double num3 = num * PowerupScript.RorschachBonusMultiplierGet();
		num2 += num3;
		num2 += PowerupScript.DieselLocomotive_PatternsBonus_Get(kind);
		num2 += PowerupScript.AbstractPaintingBonusMultiplierGet(kind);
		num2 += PowerupScript.PareidoliaBonusMultiplierGet(kind);
		if (num2 < 0.5)
		{
			num2 = 0.5;
		}
		if (double.IsInfinity(num2))
		{
			num2 = double.MaxValue;
		}
		return num2;
	}

	// Token: 0x060001D1 RID: 465
	public static void PatternsValueList_Order()
	{
		GameplayData._patternsOrderedByHighestValueToLowest.Clear();
		List<PatternScript.Kind> list = GameplayData.PatternsAvailable_GetAll();
		for (int i = 0; i < list.Count; i++)
		{
			GameplayData._patternsOrderedByHighestValueToLowest.Add(list[i]);
		}
		for (int j = 0; j < GameplayData._patternsOrderedByHighestValueToLowest.Count; j++)
		{
			PatternScript.Kind kind = GameplayData._patternsOrderedByHighestValueToLowest[j];
			double num = GameplayData.Pattern_ValueOverall_Get(kind, true);
			for (int k = j + 1; k < GameplayData._patternsOrderedByHighestValueToLowest.Count; k++)
			{
				PatternScript.Kind kind2 = GameplayData._patternsOrderedByHighestValueToLowest[k];
				double num2 = GameplayData.Pattern_ValueOverall_Get(kind2, true);
				if (num < num2)
				{
					GameplayData._patternsOrderedByHighestValueToLowest[j] = kind2;
					GameplayData._patternsOrderedByHighestValueToLowest[k] = kind;
					kind = kind2;
					num = num2;
				}
			}
		}
		GameplayData._mostValuablePatterns.Clear();
		GameplayData._mostValuablePatterns.Add(GameplayData._patternsOrderedByHighestValueToLowest[0]);
		double num3 = GameplayData.Pattern_ValueOverall_Get(GameplayData._patternsOrderedByHighestValueToLowest[0], true);
		int num4 = 0;
		while (num4 < GameplayData._patternsOrderedByHighestValueToLowest.Count && GameplayData.Pattern_ValueOverall_Get(GameplayData._patternsOrderedByHighestValueToLowest[num4], true) >= num3)
		{
			GameplayData._mostValuablePatterns.Add(GameplayData._patternsOrderedByHighestValueToLowest[num4]);
			num4++;
		}
		GameplayData._leastValuablePatterns.Clear();
		GameplayData._leastValuablePatterns.Add(GameplayData._patternsOrderedByHighestValueToLowest[GameplayData._patternsOrderedByHighestValueToLowest.Count - 1]);
		num3 = GameplayData.Pattern_ValueOverall_Get(GameplayData._patternsOrderedByHighestValueToLowest[GameplayData._patternsOrderedByHighestValueToLowest.Count - 1], true);
		int num5 = GameplayData._patternsOrderedByHighestValueToLowest.Count - 2;
		while (num5 >= 0 && GameplayData.Pattern_ValueOverall_Get(GameplayData._patternsOrderedByHighestValueToLowest[num5], true) <= num3)
		{
			GameplayData._leastValuablePatterns.Add(GameplayData._patternsOrderedByHighestValueToLowest[num5]);
			num5--;
		}
	}

	// Token: 0x060001D2 RID: 466
	public static List<PatternScript.Kind> PatternsValueList_Get()
	{
		return GameplayData._patternsOrderedByHighestValueToLowest;
	}

	// Token: 0x060001D3 RID: 467
	public static List<PatternScript.Kind> MostValuablePatterns_GetList()
	{
		return GameplayData._mostValuablePatterns;
	}

	// Token: 0x060001D4 RID: 468
	public static List<PatternScript.Kind> LeastValuablePatterns_GetList()
	{
		return GameplayData._leastValuablePatterns;
	}

	// Token: 0x060001D5 RID: 469
	public static BigInteger AllPatternsMultiplierGet(bool considerPowerups)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1;
		}
		BigInteger bigInteger = instance.allPatternsMultiplier;
		if (considerPowerups)
		{
			bigInteger += (long)(PowerupScript.ModifiedPowerups_GetCount(PowerupScript.Modifier.patternMultiplier) + PowerupScript.Necronomicon_AdditionalPatternsMultiplierGet()) + PowerupScript.Cross_PatternsMultiplierBonus_Get(true) + (long)PowerupScript.TheCollector_MultiplierGet(true) + (long)PowerupScript.ChastityBelt_MultiplierBonusGet(true) + PowerupScript.Wallet_PatternsMultiplierBonus(true) + (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_PatternsMult, true) ? 2L : 0L) + (long)PowerupScript.Hourglass_PatternsMultiplierBonusGet(true);
		}
		if (bigInteger < 1L)
		{
			bigInteger = 1;
		}
		BigInteger bigInteger2 = 1;
		if (considerPowerups)
		{
			bigInteger2 *= PowerupScript.AdamsRibCage_PatternsMultiplierBonusGet(true);
			bigInteger2 *= PowerupScript.EvilDealBonusMultiplier();
		}
		if (bigInteger2 < 1L)
		{
			bigInteger2 = 1;
		}
		return bigInteger * bigInteger2;
	}

	// Token: 0x060001D6 RID: 470
	public static void AllPatternsMultiplierSet(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.allPatternsMultiplier = value;
		if (instance.allPatternsMultiplier < 1L)
		{
			instance.allPatternsMultiplier = 1;
		}
	}

	// Token: 0x060001D7 RID: 471
	public static void AllPatternsMultiplierAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.AllPatternsMultiplierSet(instance.allPatternsMultiplier + value);
	}

	// Token: 0x060001D8 RID: 472
	public static void AllPatternsMultiplierReset()
	{
		GameplayData.AllPatternsMultiplierSet(1);
	}

	// Token: 0x060001D9 RID: 473
	public static BigInteger SixSixSix_GetMinimumDebtIndex()
	{
		return 2;
	}

	// Token: 0x060001DA RID: 474
	public static BigInteger SuperSixSixSix_GetMinimumDebtIndex()
	{
		if (RewardBoxScript.GetRewardKind() != RewardBoxScript.RewardKind.DoorKey)
		{
			return 666;
		}
		return 6;
	}

	// Token: 0x060001DB RID: 475
	public static int SixSixSix_GetSpinCount(bool updateValues)
	{
		if (GameplayData.DebtIndexGet() < GameplayData.SixSixSix_GetMinimumDebtIndex())
		{
			return 0;
		}
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		bool flag3 = GameplayData.Powerup_PossessedPhone_TriggersCount_Get() > 0;
		bool flag2 = GameplayData.SixSixSix_SuppressedSpinsGet() > 0;
		if (updateValues)
		{
			GameplayData.Powerup_PossessedPhone_TriggersCount_Set(GameplayData.Powerup_PossessedPhone_TriggersCount_Get() - 1);
			GameplayData.SixSixSix_SuppressedSpinsSet(GameplayData.SixSixSix_SuppressedSpinsGet() - 1);
		}
		if (flag3)
		{
			return 3;
		}
		if (flag2)
		{
			return 0;
		}
		float num = GameplayData.SixSixSix_ChanceGet(true);
		if (R.Rng_666.Value > 1f - num)
		{
			return 3;
		}
		float num2 = instance._666Chance * 4f;
		if (num2 > instance._666ChanceMaxAbsolute)
		{
			num2 = instance._666ChanceMaxAbsolute;
		}
		if (R.Rng_666.Value > 1f - num2)
		{
			return 2;
		}
		float num3 = instance._666Chance * 5f;
		if (num3 > instance._666ChanceMaxAbsolute)
		{
			num3 = instance._666ChanceMaxAbsolute;
		}
		if (R.Rng_666.Value > 1f - num3)
		{
			return 1;
		}
		return 0;
	}

	// Token: 0x060001DC RID: 476
	public static float SixSixSix_ChanceGet(bool considerMaximum)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		float num = instance._666Chance + PowerupScript.Necronomicon_666AdditionalChanceGet() + PowerupScript.ModifiedPowerups_Get666AdditionalChance() + PowerupScript.Cross_666Malus_Get(true) + PowerupScript.BookOfShadows_SixSixSixChanceGet(true);
		if (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Bad_666Chance1_5, true))
		{
			num += 0.015f;
		}
		else if (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Bad_666Chance3, true))
		{
			num += 0.03f;
		}
		RunModifierScript.Identifier identifier = GameplayData.RunModifier_GetCurrent();
		if (identifier == RunModifierScript.Identifier._666BigBetDouble_SmallBetNoone)
		{
			if (GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.gambling)
			{
				if (GameplayData.LastBet_IsSmallGet())
				{
					num *= 0.5f;
				}
				else
				{
					num *= 2f;
				}
			}
		}
		else if (identifier == RunModifierScript.Identifier._666DoubleChances_JackpotRecovers)
		{
			num *= 2f;
		}
		num *= (float)PowerupScript.EvilDealBonusMultiplier();
		if (considerMaximum && num > instance._666ChanceMaxAbsolute)
		{
			num = instance._666ChanceMaxAbsolute;
		}
		return num;
	}

	// Token: 0x060001DD RID: 477
	public static float SixSixSix_ChanceGet_AsPercentage(bool considerMaximum)
	{
		return GameplayData.SixSixSix_ChanceGet(considerMaximum) * 100f;
	}

	// Token: 0x060001DE RID: 478
	public static void OBSOLETE_SixSixSix_IncrementChance()
	{
		GameplayData.OBSOLETE_SixSixSix_IncrementChanceManual(0.0015f);
	}

	// Token: 0x060001DF RID: 479
	public static void OBSOLETE_SixSixSix_IncrementChanceManual(float ammount)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._666Chance += ammount;
		if (instance._666Chance < 0.005f)
		{
			instance._666Chance = 0.005f;
		}
		if (instance._666Chance > instance._666ChanceMaxAbsolute)
		{
			instance._666Chance = instance._666ChanceMaxAbsolute;
		}
	}

	// Token: 0x060001E0 RID: 480
	public static int SixSixSix_BookedSpinGet()
	{
		if (GameplayData.DebtIndexGet() < GameplayData.SixSixSix_GetMinimumDebtIndex())
		{
			return -1;
		}
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return -1;
		}
		return instance._666BookedSpin;
	}

	// Token: 0x060001E1 RID: 481
	public static void SixSixSix_BookedSpinSet(int targetSpinsLeft)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		if (GameplayData.DebtIndexGet() < GameplayData.SixSixSix_GetMinimumDebtIndex())
		{
			instance._666BookedSpin = -1;
			return;
		}
		instance._666BookedSpin = targetSpinsLeft;
	}

	// Token: 0x060001E2 RID: 482
	public static void SixSixSix_SuppressedSpinsSet(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._666SuppressedSpinsLeft = Mathf.Max(0, n);
	}

	// Token: 0x060001E3 RID: 483
	public static int SixSixSix_SuppressedSpinsGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._666SuppressedSpinsLeft;
	}

	// Token: 0x1700000B RID: 11
	// (get) Token: 0x060001E4 RID: 484
	// (set) Token: 0x060001E5 RID: 485
	public static bool LastRoundHad666Or999
	{
		get
		{
			return GameplayData.Instance != null && GameplayData.Instance._lastRoundHadA666;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._lastRoundHadA666 = value;
		}
	}

	// Token: 0x060001E6 RID: 486
	private static int Powerup_RedButtonMaxUses_Init(PowerupScript.Identifier identifier)
	{
		if (identifier <= PowerupScript.Identifier.RingBell)
		{
			if (identifier <= PowerupScript.Identifier.RedCrystal)
			{
				if (identifier == PowerupScript.Identifier.HorseShoeGold)
				{
					return 2;
				}
				if (identifier != PowerupScript.Identifier.RedCrystal)
				{
					return 0;
				}
			}
			else
			{
				if (identifier == PowerupScript.Identifier.GoldenHand_MidasTouch)
				{
					return 4;
				}
				if (identifier - PowerupScript.Identifier.PissJar > 1)
				{
					if (identifier != PowerupScript.Identifier.RingBell)
					{
						return 0;
					}
					return 3;
				}
				else
				{
					if (!Master.IsDemo)
					{
						return 5;
					}
					return 4;
				}
			}
		}
		else if (identifier <= PowerupScript.Identifier.Cross)
		{
			if (identifier == PowerupScript.Identifier.WeirdClock)
			{
				return 12;
			}
			if (identifier != PowerupScript.Identifier.AncientCoin)
			{
				if (identifier != PowerupScript.Identifier.Cross)
				{
					return 0;
				}
				return 2;
			}
		}
		else
		{
			if (identifier == PowerupScript.Identifier.PossessedPhone)
			{
				return 3;
			}
			if (identifier - PowerupScript.Identifier.SymbolInstant_Lemon <= 6)
			{
				return 4;
			}
			if (identifier != PowerupScript.Identifier._999_AngelHand)
			{
				return 0;
			}
			return 6;
		}
		return 1;
	}

	// Token: 0x060001E7 RID: 487
	private void _PowerupsData_PrepareForSerialization()
	{
		GameplayData._EnsurePowerupDataArray(this);
	}

	// Token: 0x060001E8 RID: 488
	private void _PowerupsData_RestoreFromSerialization()
	{
		GameplayData._EnsurePowerupDataArray(this);
	}

	// Token: 0x060001E9 RID: 489
	private static void _EnsurePowerupDataArray(GameplayData _inst)
	{
		int num = 164;
		if (_inst.powerupsData == null)
		{
			_inst.powerupsData = new GameplayData.PowerupData[num];
			for (int i = 0; i < num; i++)
			{
				PowerupScript.Identifier identifier = (PowerupScript.Identifier)i;
				if (_inst.powerupsData[i] == null)
				{
					_inst.powerupsData[i] = new GameplayData.PowerupData();
					_inst.powerupsData[i].Initialize(identifier, PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(identifier));
				}
			}
		}
		if (_inst.powerupsData.Length != num)
		{
			Debug.LogWarning("PowerupsData array has wrong size. Has the game been updated? Resizing array to " + num.ToString());
			Array.Resize<GameplayData.PowerupData>(ref _inst.powerupsData, num);
		}
		bool flag = false;
		for (int j = 0; j < num; j++)
		{
			PowerupScript.Identifier identifier2 = (PowerupScript.Identifier)j;
			string text = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(identifier2);
			if (_inst.powerupsData[j] == null || !(_inst.powerupsData[j].powerupIdentifierAsString == text))
			{
				bool flag2 = false;
				for (int k = j + 1; k < num; k++)
				{
					if (_inst.powerupsData[k] != null && _inst.powerupsData[k].powerupIdentifierAsString == text)
					{
						GameplayData.PowerupData powerupData = _inst.powerupsData[j];
						_inst.powerupsData[j] = _inst.powerupsData[k];
						_inst.powerupsData[k] = powerupData;
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					if (_inst.powerupsData[j] == null)
					{
						_inst.powerupsData[j] = new GameplayData.PowerupData();
						_inst.powerupsData[j].Initialize(identifier2, text);
						Debug.LogWarning("Warning! PowerupData for " + text + " was not found. Created new data.");
					}
					else
					{
						bool flag3 = false;
						for (int l = j + 1; l < num; l++)
						{
							if (_inst.powerupsData[l] == null)
							{
								_inst.powerupsData[l] = _inst.powerupsData[j];
								_inst.powerupsData[j] = null;
								flag3 = true;
								break;
							}
						}
						if (flag3)
						{
							j--;
						}
						else
						{
							flag = true;
							_inst.powerupsData[j] = null;
							j--;
						}
					}
				}
			}
		}
		if (flag)
		{
			Debug.LogWarning("Some Powerups Run-Data might be lost. Ctrl+Tab to close, or just wait a few seconds.");
		}
	}

	// Token: 0x060001EA RID: 490
	private static GameplayData.PowerupData _PowerupDataFind(PowerupScript.Identifier identifier)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return null;
		}
		GameplayData._EnsurePowerupDataArray(instance);
		if (identifier < PowerupScript.Identifier.Skeleton_Arm1 || identifier >= (PowerupScript.Identifier)instance.powerupsData.Length)
		{
			return null;
		}
		return instance.powerupsData[(int)identifier];
	}

	// Token: 0x060001EB RID: 491
	public static GameplayData.PowerupData[] PowerupDataGetCapsules()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return null;
		}
		GameplayData._EnsurePowerupDataArray(instance);
		return instance.powerupsData;
	}

	// Token: 0x060001EC RID: 492
	public void PowerupsRngEnsure(bool resetForNewGame)
	{
		GameplayData.PowerupData[] array = GameplayData.PowerupDataGetCapsules();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].charmSpecificRng == null || resetForNewGame)
			{
				array[i].charmSpecificRng = new Rng(this.seed);
			}
		}
	}

	// Token: 0x060001ED RID: 493
	public Rng PowerupRngGet(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return null;
		}
		return powerupData.charmSpecificRng;
	}

	// Token: 0x060001EE RID: 494
	public static int Powerup_BoughtTimes_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.boughtTimes;
	}

	// Token: 0x060001EF RID: 495
	public static void Powerup_BoughtTimes_Increase(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return;
		}
		powerupData.boughtTimes++;
	}

	// Token: 0x060001F0 RID: 496
	public static PowerupScript.Modifier Powerup_Modifier_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return PowerupScript.Modifier.none;
		}
		return powerupData.modifier;
	}

	// Token: 0x060001F1 RID: 497
	public static void Powerup_Modifier_Set(PowerupScript.Identifier identifier, PowerupScript.Modifier modifier, bool updatePowerup)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return;
		}
		powerupData.modifier = modifier;
		if (GameplayMaster.instance == null)
		{
			return;
		}
		if (!updatePowerup)
		{
			return;
		}
		PowerupScript powerup_Quick = PowerupScript.GetPowerup_Quick(identifier);
		if (powerup_Quick != null)
		{
			powerup_Quick.MaterialRefresh();
		}
		PowerupScript.ModifiedPowerups_EquippedCounter_Refresh();
	}

	// Token: 0x060001F2 RID: 498
	public static int Powerup_ButtonBurnedOut_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.buttonBurnOutCounter;
	}

	// Token: 0x060001F3 RID: 499
	public static void Powerup_ButtonBurnedOut_Set(PowerupScript.Identifier identifier, int n)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return;
		}
		powerupData.buttonBurnOutCounter = n;
	}

	// Token: 0x060001F4 RID: 500
	public static void Powerup_ButtonBurnedOut_Increaase(PowerupScript.Identifier identifier)
	{
		GameplayData.Powerup_ButtonBurnedOut_Set(identifier, GameplayData.Powerup_ButtonBurnedOut_Get(identifier) + 1);
	}

	// Token: 0x060001F5 RID: 501
	public static int Powerup_ButtonChargesUsed_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.buttonChargesCounter;
	}

	// Token: 0x060001F6 RID: 502
	public static int Powerup_ButtonChargesUsed_GetAbsolute(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.buttonChargesCounter_Absolute;
	}

	// Token: 0x060001F7 RID: 503
	public static bool Powerup_ButtonChargesUsed_Reset(PowerupScript.Identifier identifier, bool triggerRechargeAnimation)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return false;
		}
		if (powerupData.buttonChargesCounter <= 0)
		{
			powerupData.buttonChargesCounter = 0;
			return false;
		}
		int buttonChargesCounter = powerupData.buttonChargesCounter;
		powerupData.buttonChargesCounter = 0;
		RedButtonScript.ButtonVisualsRefresh();
		if (triggerRechargeAnimation)
		{
			PowerupScript.PlayRechargeAnimation(identifier);
		}
		Data.game.UnlockableSteps_OnRechargingRedButtonCharges(buttonChargesCounter);
		return true;
	}

	// Token: 0x060001F8 RID: 504
	public static bool Powerup_ButtonChargesUsed_ResetAll(bool triggerRechargeAnimation)
	{
		GameplayData.PowerupData[] array = GameplayData.PowerupDataGetCapsules();
		if (array == null)
		{
			return false;
		}
		int num = 0;
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				if (array[i].buttonChargesCounter <= 0)
				{
					array[i].buttonChargesCounter = 0;
				}
				else
				{
					num += array[i].buttonChargesCounter;
					array[i].buttonChargesCounter = 0;
					flag = true;
					if (triggerRechargeAnimation)
					{
						PowerupScript.Identifier identifier = array[i].IdentifierGetInferred();
						if (PowerupScript.IsEquipped_Quick(identifier))
						{
							PowerupScript.PlayRechargeAnimation(identifier);
						}
					}
				}
			}
		}
		if (flag)
		{
			RedButtonScript.ButtonVisualsRefresh();
		}
		Data.game.UnlockableSteps_OnRechargingRedButtonCharges(num);
		return flag;
	}

	// Token: 0x060001F9 RID: 505
	public static void Powerup_ButtonChargesUsed_ConsumeAllCharges(PowerupScript.Identifier identifier, bool affectAbsoluteCounter)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return;
		}
		int num = Mathf.Max(0, powerupData.buttonChargesMax - powerupData.buttonChargesCounter);
		powerupData.buttonChargesCounter += num;
		if (powerupData.buttonChargesCounter > powerupData.buttonChargesMax)
		{
			powerupData.buttonChargesCounter = powerupData.buttonChargesMax;
		}
		if (affectAbsoluteCounter)
		{
			powerupData.buttonChargesCounter_Absolute += num;
		}
	}

	// Token: 0x060001FA RID: 506
	public static bool Powerup_ButtonChargesUsed_RestoreChargesN(PowerupScript.Identifier identifier, int value, bool triggerRechargeAnimation)
	{
		return GameplayData.Powerup_ButtonChargesUsed_RestoreChargesN_Ext(identifier, value, triggerRechargeAnimation, true);
	}

	// Token: 0x060001FB RID: 507
	public static bool Powerup_ButtonChargesUsed_RestoreChargesN_Ext(PowerupScript.Identifier identifier, int value, bool triggerRechargeAnimation, bool refreshButtonVisuals)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return false;
		}
		if (powerupData.buttonChargesCounter <= 0)
		{
			powerupData.buttonChargesCounter = 0;
			return false;
		}
		powerupData.buttonChargesCounter = Mathf.Min(powerupData.buttonChargesCounter, powerupData.buttonChargesMax);
		powerupData.buttonChargesCounter -= value;
		int num = Mathf.Min(powerupData.buttonChargesCounter, value);
		if (powerupData.buttonChargesCounter < 0)
		{
			powerupData.buttonChargesCounter = 0;
		}
		if (refreshButtonVisuals)
		{
			RedButtonScript.ButtonVisualsRefresh();
		}
		if (triggerRechargeAnimation && powerupData.buttonChargesCounter == 0)
		{
			PowerupScript.PlayRechargeAnimation(identifier);
		}
		Data.game.UnlockableSteps_OnRechargingRedButtonCharges(num);
		return true;
	}

	// Token: 0x060001FC RID: 508
	public static int Powerup_ButtonChargesMax_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return -1;
		}
		int buttonChargesMax = powerupData.buttonChargesMax;
		if (buttonChargesMax <= 0)
		{
			return -1;
		}
		return buttonChargesMax;
	}

	// Token: 0x060001FD RID: 509
	public static void Powerup_ButtonChargesMax_Set(PowerupScript.Identifier identifier, int value, bool triggerRechargeAnimation, bool refreshButtonVisuals)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return;
		}
		if (value < 1)
		{
			value = 1;
		}
		int buttonChargesMax = powerupData.buttonChargesMax;
		powerupData.buttonChargesMax = value;
		if (value < buttonChargesMax)
		{
			int num = buttonChargesMax - value;
			GameplayData.Powerup_ButtonChargesUsed_RestoreChargesN_Ext(identifier, num, triggerRechargeAnimation, refreshButtonVisuals);
		}
	}

	// Token: 0x060001FE RID: 510
	public static int Powerup_ResellBonus_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.resellBonus;
	}

	// Token: 0x060001FF RID: 511
	public static void Powerup_ResellBonus_Set(PowerupScript.Identifier identifier, int n)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return;
		}
		powerupData.resellBonus = Mathf.Max(0, n);
	}

	// Token: 0x1700000C RID: 12
	// (get) Token: 0x06000200 RID: 512
	// (set) Token: 0x06000201 RID: 513
	public static int RndActivationFailsafe_ConsolationPrize
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._rndActivationFailsafe_ConsolationPrize;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._rndActivationFailsafe_ConsolationPrize = value;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x06000202 RID: 514
	// (set) Token: 0x06000203 RID: 515
	public static int RndActivationFailsafe_BrokenCalculator
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._rndActivationFailsafe_BrokenCalculator;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._rndActivationFailsafe_BrokenCalculator = value;
		}
	}

	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000204 RID: 516
	// (set) Token: 0x06000205 RID: 517
	public static int RndActivationFailsafe_CrankGenerator
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._rndActivationFailsafe_CrankGenerator;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._rndActivationFailsafe_CrankGenerator = value;
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000206 RID: 518
	// (set) Token: 0x06000207 RID: 519
	public static int RndActivationFailsafe_FakeCoin
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._rndActivationFailsafe_FakeCoin;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._rndActivationFailsafe_FakeCoin = value;
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000208 RID: 520
	// (set) Token: 0x06000209 RID: 521
	public static int RndActivationFailsafe_RedPepper
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._rndActivationFailsafe_RedPepper;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._rndActivationFailsafe_RedPepper = value;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x0600020A RID: 522
	// (set) Token: 0x0600020B RID: 523
	public static int RndActivationFailsafe_GreenPepper
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._rndActivationFailsafe_GreenPepper;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._rndActivationFailsafe_GreenPepper = value;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x0600020C RID: 524
	// (set) Token: 0x0600020D RID: 525
	public static int RndActivationFailsafe_GoldenPepper
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._rndActivationFailsafe_GoldenPepper;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._rndActivationFailsafe_GoldenPepper = value;
		}
	}

	// Token: 0x17000013 RID: 19
	// (get) Token: 0x0600020E RID: 526
	// (set) Token: 0x0600020F RID: 527
	public static int RndActivationFailsafe_RottenPepper
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._rndActivationFailsafe_RottenPepper;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._rndActivationFailsafe_RottenPepper = value;
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000210 RID: 528
	// (set) Token: 0x06000211 RID: 529
	public static int RndActivationFailsafe_BellPepper
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._rndActivationFailsafe_BellPepper;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._rndActivationFailsafe_BellPepper = value;
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000212 RID: 530
	// (set) Token: 0x06000213 RID: 531
	public static int RndActivationFailsafe_Rosary
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._rndActivationFailsafe_Rosary;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._rndActivationFailsafe_Rosary = value;
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000214 RID: 532
	// (set) Token: 0x06000215 RID: 533
	public static int RndActivationFailsafe_Dice4
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._rndActivationFailsafe_Dice4;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._rndActivationFailsafe_Dice4 = value;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000216 RID: 534
	// (set) Token: 0x06000217 RID: 535
	public static int RndActivationFailsafe_SacredHeart
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._rndActivationFailsafe_SacredHeart;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._rndActivationFailsafe_SacredHeart = value;
		}
	}

	// Token: 0x06000218 RID: 536
	private void _PowerupsSpecific_PrepareForSerialization()
	{
		this._powerupTarotDeck_Reward_ByteArray = this._powerupTarotDeck_Reward.ToByteArray();
		this._powerupPoopBeetle_SymbolsIncreasetMultByteArray = this._powerupPoopBeetle_SymbolsIncreaserMult.ToByteArray();
		this._powerupCalendar_SymbolsIncreaserMultByteArray = this._powerupCalendar_SymbolsIncreaserMult.ToByteArray();
		this._powerupHoleCircle_CharmStr = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(this._powerupHoleCircle_CharmIdentifier);
		this._powerupHoleRomboid_CharmStr = PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(this._powerupHoleRomboid_CharmIdentifier);
		this._powerupHoleCross_AbilityStr = PlatformDataMaster.EnumEntryToString<AbilityScript.Identifier>(this._powerupHoleCross_AbilityIdentifier);
		this.jimboAbilities_Selected_Str = PlatformDataMaster.EnumListToString<GameplayData.JimboAbility>(this.jimboAbilities_Selected, ',');
		GameplayData.JimboStringsEnsure();
		this.Powerup_GigaMushroom_SerializationPrepare();
		this.Powerup_Pareidolia_SerializationPrepare();
	}

	// Token: 0x06000219 RID: 537
	private void _PowerupsSpecific_RestoreFromSerialization()
	{
		this._powerupTarotDeck_Reward = this.BigIntegerFromByteArray(this._powerupTarotDeck_Reward_ByteArray, 0);
		this._powerupPoopBeetle_SymbolsIncreaserMult = this.BigIntegerFromByteArray(this._powerupPoopBeetle_SymbolsIncreasetMultByteArray, 0);
		this._powerupCalendar_SymbolsIncreaserMult = this.BigIntegerFromByteArray(this._powerupCalendar_SymbolsIncreaserMultByteArray, 0);
		this._powerupHoleCircle_CharmIdentifier = PlatformDataMaster.EnumEntryFromString<PowerupScript.Identifier>(this._powerupHoleCircle_CharmStr, PowerupScript.Identifier.undefined);
		this._powerupHoleRomboid_CharmIdentifier = PlatformDataMaster.EnumEntryFromString<PowerupScript.Identifier>(this._powerupHoleRomboid_CharmStr, PowerupScript.Identifier.undefined);
		this._powerupHoleCross_AbilityIdentifier = PlatformDataMaster.EnumEntryFromString<AbilityScript.Identifier>(this._powerupHoleCross_AbilityStr, AbilityScript.Identifier.undefined);
		this.jimboAbilities_Selected = PlatformDataMaster.EnumListFromString<GameplayData.JimboAbility>(this.jimboAbilities_Selected_Str, ',');
		this.Powerup_GigaMushroom_SerializationRestore();
		this.Powerup_Pareidolia_SerializationRestore();
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x0600021A RID: 538
	// (set) Token: 0x0600021B RID: 539
	public static int Powerup_Hourglass_DeadlinesCounter
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._powerupHourglass_DeadlinesCounter;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._powerupHourglass_DeadlinesCounter = value;
		}
	}

	// Token: 0x0600021C RID: 540
	public static int Powerup_FruitsBasket_RoundsLeftGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupFruitsBasket_RoundsLeft;
	}

	// Token: 0x0600021D RID: 541
	public static void Powerup_FruitsBasket_RoundsLeftSet(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupFruitsBasket_RoundsLeft = value;
		instance._powerupFruitsBasket_RoundsLeft = Mathf.Max(0, instance._powerupFruitsBasket_RoundsLeft);
	}

	// Token: 0x0600021E RID: 542
	public static void Powerup_FruitBasket_RoundsLeftReset()
	{
		GameplayData.Powerup_FruitsBasket_RoundsLeftSet(10);
	}

	// Token: 0x0600021F RID: 543
	public static BigInteger Powerup_TarotDeck_RewardGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupTarotDeck_Reward;
	}

	// Token: 0x06000220 RID: 544
	public static void Powerup_TarotDeck_RewardSet(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupTarotDeck_Reward = value;
		if (instance._powerupTarotDeck_Reward < 0L)
		{
			instance._powerupTarotDeck_Reward = 0;
		}
	}

	// Token: 0x06000221 RID: 545
	public static void Powerup_TarotDeck_RewardAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_TarotDeck_RewardSet(instance._powerupTarotDeck_Reward + value);
	}

	// Token: 0x06000222 RID: 546
	public static BigInteger Powerup_PoopBeetle_SymbolsIncreaseN_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupPoopBeetle_SymbolsIncreaserMult;
	}

	// Token: 0x06000223 RID: 547
	public static void Powerup_PoopBeetle_SymbolsIncreaseN_Set(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupPoopBeetle_SymbolsIncreaserMult = value;
		if (instance._powerupPoopBeetle_SymbolsIncreaserMult < 0L)
		{
			instance._powerupPoopBeetle_SymbolsIncreaserMult = 0;
		}
	}

	// Token: 0x06000224 RID: 548
	public static void Powerup_PoopBeetle_SymbolsIncreaseN_Add(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_PoopBeetle_SymbolsIncreaseN_Set(instance._powerupPoopBeetle_SymbolsIncreaserMult + value);
	}

	// Token: 0x06000225 RID: 549
	public static int Powerup_GrandmasPurse_ExtraInterestGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupGrandmasPurse_ExtraInterest;
	}

	// Token: 0x06000226 RID: 550
	public static void Powerup_GrandmasPurse_ExtraInterestSet(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupGrandmasPurse_ExtraInterest = value;
		if (instance._powerupGrandmasPurse_ExtraInterest < 0)
		{
			instance._powerupGrandmasPurse_ExtraInterest = 0;
		}
	}

	// Token: 0x06000227 RID: 551
	public static void Powerup_GrandmasPurse_ExtraInterestAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_GrandmasPurse_ExtraInterestSet(instance._powerupGrandmasPurse_ExtraInterest + value);
	}

	// Token: 0x06000228 RID: 552
	public static void Powerup_GrandmasPurse_ExtraInterestReset()
	{
		GameplayData.Powerup_GrandmasPurse_ExtraInterestSet(15);
	}

	// Token: 0x06000229 RID: 553
	public static int Powerup_OneTrickPony_TargetSpinIndexGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return -1;
		}
		return instance._powerupOneTrickPony_TargetSpinsLeftIndex;
	}

	// Token: 0x0600022A RID: 554
	public static void Powerup_OneTrickPony_TargetSpinIndexSet(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupOneTrickPony_TargetSpinsLeftIndex = value;
		if (instance._powerupOneTrickPony_TargetSpinsLeftIndex < -1)
		{
			instance._powerupOneTrickPony_TargetSpinsLeftIndex = -1;
		}
	}

	// Token: 0x0600022B RID: 555
	public static int Powerup_Pentacle_TriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupPentacleTriggeredTimes;
	}

	// Token: 0x0600022C RID: 556
	public static void Powerup_Pentacle_TriggeredTimesSet(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupPentacleTriggeredTimes = value;
		if (instance._powerupPentacleTriggeredTimes < 0)
		{
			instance._powerupPentacleTriggeredTimes = 0;
		}
	}

	// Token: 0x0600022D RID: 557
	public static void Powerup_Pentacle_TriggeredTimesAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_Pentacle_TriggeredTimesSet(instance._powerupPentacleTriggeredTimes + value);
	}

	// Token: 0x0600022E RID: 558
	public static BigInteger Powerup_Calendar_SymbolsIncreaseN_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupCalendar_SymbolsIncreaserMult;
	}

	// Token: 0x0600022F RID: 559
	public static void Powerup_Calendar_SymbolsIncreaseN_Set(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupCalendar_SymbolsIncreaserMult = value;
		if (instance._powerupCalendar_SymbolsIncreaserMult < 0L)
		{
			instance._powerupCalendar_SymbolsIncreaserMult = 0;
		}
	}

	// Token: 0x06000230 RID: 560
	private void Powerup_GigaMushroom_SerializationPrepare()
	{
		this._powerupGigaMushroom_SymbLemonsValue_ByteArray = this._powerupGigaMushroom_SymbLemonsValue.ToByteArray();
		this._powerupGigaMushroom_SymbCherriesValue_ByteArray = this._powerupGigaMushroom_SymbCherriesValue.ToByteArray();
		this._powerupGigaMushroom_SymbCloversValue_ByteArray = this._powerupGigaMushroom_SymbCloversValue.ToByteArray();
		this._powerupGigaMushroom_SymbBellsValue_ByteArray = this._powerupGigaMushroom_SymbBellsValue.ToByteArray();
		this._powerupGigaMushroom_SymbDiamondsValue_ByteArray = this._powerupGigaMushroom_SymbDiamondsValue.ToByteArray();
		this._powerupGigaMushroom_SymbCoinsValue_ByteArray = this._powerupGigaMushroom_SymbCoinsValue.ToByteArray();
		this._powerupGigaMushroom_SymbSevensValue_ByteArray = this._powerupGigaMushroom_SymbSevensValue.ToByteArray();
	}

	// Token: 0x06000231 RID: 561
	private void Powerup_GigaMushroom_SerializationRestore()
	{
		this._powerupGigaMushroom_SymbLemonsValue = this.BigIntegerFromByteArray(this._powerupGigaMushroom_SymbLemonsValue_ByteArray, 0);
		this._powerupGigaMushroom_SymbCherriesValue = this.BigIntegerFromByteArray(this._powerupGigaMushroom_SymbCherriesValue_ByteArray, 0);
		this._powerupGigaMushroom_SymbCloversValue = this.BigIntegerFromByteArray(this._powerupGigaMushroom_SymbCloversValue_ByteArray, 0);
		this._powerupGigaMushroom_SymbBellsValue = this.BigIntegerFromByteArray(this._powerupGigaMushroom_SymbBellsValue_ByteArray, 0);
		this._powerupGigaMushroom_SymbDiamondsValue = this.BigIntegerFromByteArray(this._powerupGigaMushroom_SymbDiamondsValue_ByteArray, 0);
		this._powerupGigaMushroom_SymbCoinsValue = this.BigIntegerFromByteArray(this._powerupGigaMushroom_SymbCoinsValue_ByteArray, 0);
		this._powerupGigaMushroom_SymbSevensValue = this.BigIntegerFromByteArray(this._powerupGigaMushroom_SymbSevensValue_ByteArray, 0);
	}

	// Token: 0x06000232 RID: 562
	public static BigInteger Powerup_GigaMushroom_SymbolValueGet(SymbolScript.Kind kind)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		switch (kind)
		{
		case SymbolScript.Kind.lemon:
			return instance._powerupGigaMushroom_SymbLemonsValue;
		case SymbolScript.Kind.cherry:
			return instance._powerupGigaMushroom_SymbCherriesValue;
		case SymbolScript.Kind.clover:
			return instance._powerupGigaMushroom_SymbCloversValue;
		case SymbolScript.Kind.bell:
			return instance._powerupGigaMushroom_SymbBellsValue;
		case SymbolScript.Kind.diamond:
			return instance._powerupGigaMushroom_SymbDiamondsValue;
		case SymbolScript.Kind.coins:
			return instance._powerupGigaMushroom_SymbCoinsValue;
		case SymbolScript.Kind.seven:
			return instance._powerupGigaMushroom_SymbSevensValue;
		case SymbolScript.Kind.six:
		case SymbolScript.Kind.nine:
			return 0;
		default:
			Debug.LogError("GameplayData.Powerup_GigaMushroom_SymbolValueGet(): symbol not handled: " + kind.ToString());
			return 0;
		}
	}

	// Token: 0x06000233 RID: 563
	public static void Powerup_GigaMushroom_SymbolValueSet(SymbolScript.Kind kind, BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		switch (kind)
		{
		case SymbolScript.Kind.lemon:
			instance._powerupGigaMushroom_SymbLemonsValue = ((value < 0L) ? 0 : value);
			return;
		case SymbolScript.Kind.cherry:
			instance._powerupGigaMushroom_SymbCherriesValue = ((value < 0L) ? 0 : value);
			return;
		case SymbolScript.Kind.clover:
			instance._powerupGigaMushroom_SymbCloversValue = ((value < 0L) ? 0 : value);
			return;
		case SymbolScript.Kind.bell:
			instance._powerupGigaMushroom_SymbBellsValue = ((value < 0L) ? 0 : value);
			return;
		case SymbolScript.Kind.diamond:
			instance._powerupGigaMushroom_SymbDiamondsValue = ((value < 0L) ? 0 : value);
			return;
		case SymbolScript.Kind.coins:
			instance._powerupGigaMushroom_SymbCoinsValue = ((value < 0L) ? 0 : value);
			return;
		case SymbolScript.Kind.seven:
			instance._powerupGigaMushroom_SymbSevensValue = ((value < 0L) ? 0 : value);
			return;
		case SymbolScript.Kind.six:
		case SymbolScript.Kind.nine:
			return;
		default:
			Debug.LogError("GameplayData.Powerup_GigaMushroom_SymbolValueGet(): symbol not handled: " + kind.ToString());
			return;
		}
	}

	// Token: 0x06000234 RID: 564
	public static int Powerup_GoldenHorseShoe_SpinsLeftGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupGoldenHorseShoe_SpinsLeft;
	}

	// Token: 0x06000235 RID: 565
	public static void Powerup_GoldenHorseShoe_SpinsLeftSet(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupGoldenHorseShoe_SpinsLeft = Mathf.Max(0, n);
	}

	// Token: 0x06000236 RID: 566
	public static int Powerup_AncientCoin_SpinsLeftGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupAncientCoin_SpinsLeft;
	}

	// Token: 0x06000237 RID: 567
	public static void Powerup_AncientCoin_SpinsLeftSet(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupAncientCoin_SpinsLeft = Mathf.Max(0, n);
	}

	// Token: 0x06000238 RID: 568
	public static int Powerup_ChannelerOfFortune_ActivationsCounterGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupChannelerOfFortunes_ActivationsCounter;
	}

	// Token: 0x06000239 RID: 569
	public static void Powerup_ChannelerOfFortune_ActivationsCounterSet(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupChannelerOfFortunes_ActivationsCounter = n;
	}

	// Token: 0x0600023A RID: 570
	private void PowerupPareidoliaArrayEnsure()
	{
		if (this._powerupPareidolia_PatternBonuses == null)
		{
			this._powerupPareidolia_PatternBonuses = new double[16];
		}
		if (this._powerupPareidolia_PatternBonuses.Length != 16)
		{
			Array.Resize<double>(ref this._powerupPareidolia_PatternBonuses, 16);
		}
	}

	// Token: 0x0600023B RID: 571
	private void Powerup_Pareidolia_SerializationPrepare()
	{
		this.PowerupPareidoliaArrayEnsure();
	}

	// Token: 0x0600023C RID: 572
	private void Powerup_Pareidolia_SerializationRestore()
	{
		this.PowerupPareidoliaArrayEnsure();
	}

	// Token: 0x0600023D RID: 573
	public static double Powerup_PareidoliaMultiplierBonus_Get(PatternScript.Kind kind)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0.0;
		}
		return instance._powerupPareidolia_PatternBonuses[(int)kind];
	}

	// Token: 0x0600023E RID: 574
	public static void Powerup_PareidoliaMultiplierBonus_Set(PatternScript.Kind kind, double n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		if (n < 0.0)
		{
			n = 1.0;
		}
		instance._powerupPareidolia_PatternBonuses[(int)kind] = n;
	}

	// Token: 0x0600023F RID: 575
	public static long Powerup_RingBell_Bonus_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupRingBell_BonusCounter;
	}

	// Token: 0x06000240 RID: 576
	public static void Powerup_RingBell_Bonus_Set(long n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		if (n < 0L)
		{
			n = 0L;
		}
		instance._powerupRingBell_BonusCounter = n;
	}

	// Token: 0x06000241 RID: 577
	public static long Powerup_ConsolationPrize_Bonus_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupConsolationPrize_BonusCounter;
	}

	// Token: 0x06000242 RID: 578
	public static void Powerup_ConsolationPrize_Bonus_Set(long n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		if (n < 0L)
		{
			n = 0L;
		}
		instance._powerupConsolationPrize_BonusCounter = n;
	}

	// Token: 0x06000243 RID: 579
	public static int Powerup_StepsCounter_TriggersCounter_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupStepsCounter_TriggersCounter;
	}

	// Token: 0x06000244 RID: 580
	public static void Powerup_StepsCounter_TriggersCounter_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		if (n < 0)
		{
			n = 0;
		}
		instance._powerupStepsCounter_TriggersCounter = n;
	}

	// Token: 0x06000245 RID: 581
	public static int Powerup_DieselLocomotive_Bonus_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupDieselLocomotiveBonus;
	}

	// Token: 0x06000246 RID: 582
	public static void Powerup_DieselLocomotive_Bonus_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		if (n < 0)
		{
			n = 0;
		}
		instance._powerupDieselLocomotiveBonus = n;
	}

	// Token: 0x06000247 RID: 583
	public static int Powerup_SteamLocomotive_Bonus_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupSteamLocomotiveBonus;
	}

	// Token: 0x06000248 RID: 584
	public static void Powerup_SteamLocomotive_Bonus_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		if (n < 0)
		{
			n = 0;
		}
		instance._powerupSteamLocomotiveBonus = n;
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000249 RID: 585
	// (set) Token: 0x0600024A RID: 586
	public static int Powerup_DiscA_SpinsCounter
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._powerupDiscA_SpinsCounter;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._powerupDiscA_SpinsCounter = value;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x0600024B RID: 587
	// (set) Token: 0x0600024C RID: 588
	public static int Powerup_DiscB_SpinsCounter
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._powerupDiscB_SpinsCounter;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._powerupDiscB_SpinsCounter = value;
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x0600024D RID: 589
	// (set) Token: 0x0600024E RID: 590
	public static int Powerup_DiscC_SpinsCounter
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._powerupDiscC_SpinsCounter;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._powerupDiscC_SpinsCounter = value;
		}
	}

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x0600024F RID: 591
	// (set) Token: 0x06000250 RID: 592
	public static int Powerup_WeirdClock_DeadlineUses
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._powerupWeirdClock_DeadlineUses;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._powerupWeirdClock_DeadlineUses = value;
		}
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000251 RID: 593
	// (set) Token: 0x06000252 RID: 594
	public static int Powerup_Cigarettes_ActivationsCounter
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._powerupCigarettesActivationsCounter;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._powerupCigarettesActivationsCounter = value;
		}
	}

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000253 RID: 595
	// (set) Token: 0x06000254 RID: 596
	public static long Powerup_Cigarettes_PriceIncrease
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0L;
			}
			return instance._powerupCigarettesPriceIncrease;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._powerupCigarettesPriceIncrease = ((value < 0L) ? 0L : value);
		}
	}

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000255 RID: 597
	// (set) Token: 0x06000256 RID: 598
	public static int Powerup_Jimbo_RoundsLeft
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return -1;
			}
			return instance.jimboRoundsLeft;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance.jimboRoundsLeft = value;
		}
	}

	// Token: 0x06000257 RID: 599
	public static void Powerup_Jimbo_ReshuffleAndReset()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.jimboAbilities_Selected.Clear();
		int num = instance.jimboAbilities_BadPool.Count;
		int num2 = R.Rng_Powerup(PowerupScript.Identifier.Jimbo).Range(0, num);
		instance.jimboAbilities_Selected.Add(instance.jimboAbilities_BadPool[num2]);
		instance.jimboRoundsLeft = -1;
		GameplayData.JimboAbility jimboAbility = instance.jimboAbilities_BadPool[num2];
		if (jimboAbility != GameplayData.JimboAbility.Bad_Discard6)
		{
			if (jimboAbility == GameplayData.JimboAbility.Bad_Discard3)
			{
				instance.jimboRoundsLeft = 3;
			}
		}
		else
		{
			instance.jimboRoundsLeft = 5;
		}
		for (int i = 0; i < 2; i++)
		{
			num = instance.jimboAbilities_GoodPool.Count;
			num2 = R.Rng_Powerup(PowerupScript.Identifier.Jimbo).Range(0, num);
			int num3 = 0;
			while (num3 < num && instance.jimboAbilities_Selected.Count < 3)
			{
				if (!instance.jimboAbilities_Selected.Contains(instance.jimboAbilities_GoodPool[num2]))
				{
					instance.jimboAbilities_Selected.Add(instance.jimboAbilities_GoodPool[num2]);
					break;
				}
				num2++;
				if (num2 >= num)
				{
					num2 = 0;
				}
				num3++;
			}
		}
	}

	// Token: 0x06000258 RID: 600
	public static List<GameplayData.JimboAbility> Powerup_Jimbo_AbilitiesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return null;
		}
		return instance.jimboAbilities_Selected;
	}

	// Token: 0x06000259 RID: 601
	public static bool Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility ability, bool considerEquippedState)
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && (!considerEquippedState || PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Jimbo)) && instance.jimboAbilities_Selected.Contains(ability);
	}

	// Token: 0x0600025A RID: 602
	private static void JimboStringsEnsure()
	{
		int num = 18;
		for (int i = 0; i < num; i++)
		{
			if (!GameplayData.jimboAbilityKeys.ContainsKey((GameplayData.JimboAbility)i))
			{
				string text = "GameplayData.JimboStringsEnsure(): jimbo has no string def for key: ";
				GameplayData.JimboAbility jimboAbility = (GameplayData.JimboAbility)i;
				Debug.LogError(text + jimboAbility.ToString());
			}
		}
	}

	// Token: 0x0600025B RID: 603
	public static string JimboDescriptionStringsGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return "";
		}
		GameplayData.jimboSB.Clear();
		for (int i = 0; i < instance.jimboAbilities_Selected.Count; i++)
		{
			GameplayData.jimboSB.Append("(");
			GameplayData.jimboSB.Append((i + 1).ToString());
			GameplayData.jimboSB.Append("):");
			GameplayData.jimboSB.Append(Translation.Get(GameplayData.jimboAbilityKeys[instance.jimboAbilities_Selected[i]]));
			GameplayData.jimboSB.Append("   ");
		}
		return GameplayData.jimboSB.ToString();
	}

	// Token: 0x0600025C RID: 604
	public static int Powerup_RedPepper_ActivationsCounterGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupRedPepper_ActivationsCounter;
	}

	// Token: 0x0600025D RID: 605
	public static void Powerup_RedPepper_ActivationsCounterSet(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupRedPepper_ActivationsCounter = value;
		if (instance._powerupRedPepper_ActivationsCounter < 0)
		{
			instance._powerupRedPepper_ActivationsCounter = 0;
		}
	}

	// Token: 0x0600025E RID: 606
	public static void Powerup_RedPepper_ActivationsCounterAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_RedPepper_ActivationsCounterSet(instance._powerupRedPepper_ActivationsCounter + value);
	}

	// Token: 0x0600025F RID: 607
	public static int Powerup_GreenPepper_ActivationsCounterGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupGreenPepper_ActivationsCounter;
	}

	// Token: 0x06000260 RID: 608
	public static void Powerup_GreenPepper_ActivationsCounterSet(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupGreenPepper_ActivationsCounter = value;
		if (instance._powerupGreenPepper_ActivationsCounter < 0)
		{
			instance._powerupGreenPepper_ActivationsCounter = 0;
		}
	}

	// Token: 0x06000261 RID: 609
	public static void Powerup_GreenPepper_ActivationsCounterAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_GreenPepper_ActivationsCounterSet(instance._powerupGreenPepper_ActivationsCounter + value);
	}

	// Token: 0x06000262 RID: 610
	public static int Powerup_GoldenPepper_LuckBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupGoldenPepper_LuckBonus;
	}

	// Token: 0x06000263 RID: 611
	public static void Powerup_GoldenPepper_LuckBonusSet(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupGoldenPepper_LuckBonus = value;
		if (instance._powerupGoldenPepper_LuckBonus < 0)
		{
			instance._powerupGoldenPepper_LuckBonus = 0;
		}
	}

	// Token: 0x06000264 RID: 612
	public static int Powerup_RottenPepper_LuckBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupRottenPepper_LuckBonus;
	}

	// Token: 0x06000265 RID: 613
	public static void Powerup_RottenPepper_LuckBonusSet(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupRottenPepper_LuckBonus = value;
		if (instance._powerupRottenPepper_LuckBonus < 0)
		{
			instance._powerupRottenPepper_LuckBonus = 0;
		}
	}

	// Token: 0x06000266 RID: 614
	public static int Powerup_BellPepper_LuckBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupBellPepper_LuckBonus;
	}

	// Token: 0x06000267 RID: 615
	public static void Powerup_BellPepper_LuckBonusSet(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupBellPepper_LuckBonus = value;
		if (instance._powerupBellPepper_LuckBonus < 0)
		{
			instance._powerupBellPepper_LuckBonus = 0;
		}
	}

	// Token: 0x06000268 RID: 616
	public static long Powerup_DevilHorn_AdditionalMultiplierGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupDevilHorn_AdditionalMultiplier;
	}

	// Token: 0x06000269 RID: 617
	public static void Powerup_DevilHorn_AdditionalMultiplierSet(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupDevilHorn_AdditionalMultiplier = value;
		if (instance._powerupDevilHorn_AdditionalMultiplier < 0L)
		{
			instance._powerupDevilHorn_AdditionalMultiplier = 0L;
		}
	}

	// Token: 0x0600026A RID: 618
	public static void Powerup_DevilHorn_AdditionalMultiplierAdd(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_DevilHorn_AdditionalMultiplierSet(instance._powerupDevilHorn_AdditionalMultiplier + value);
	}

	// Token: 0x0600026B RID: 619
	public static int Powerup_Baphomet_ActivationsCounterGet_Above()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupBaphomet_SymbolsBonus;
	}

	// Token: 0x0600026C RID: 620
	public static void Powerup_Baphomet_ActivationsCounterSet_Above(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupBaphomet_SymbolsBonus = value;
		if (instance._powerupBaphomet_SymbolsBonus < 1)
		{
			instance._powerupBaphomet_SymbolsBonus = 1;
		}
	}

	// Token: 0x0600026D RID: 621
	public static int Powerup_Baphomet_ActivationsCounterGet_Below()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupBaphomet_PatternsBonus;
	}

	// Token: 0x0600026E RID: 622
	public static void Powerup_Baphomet_ActivationsCounterSet_Below(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupBaphomet_PatternsBonus = value;
		if (instance._powerupBaphomet_PatternsBonus < 1)
		{
			instance._powerupBaphomet_PatternsBonus = 1;
		}
	}

	// Token: 0x0600026F RID: 623
	public static long Powerup_Cross_TriggersCount_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupCross_Triggers;
	}

	// Token: 0x06000270 RID: 624
	public static void Powerup_Cross_TriggersCount_Set(long i)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupCross_Triggers = i;
	}

	// Token: 0x06000271 RID: 625
	public static int Powerup_PossessedPhone_TriggersCount_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupPossessedPhone_SpinsCount;
	}

	// Token: 0x06000272 RID: 626
	public static void Powerup_PossessedPhone_TriggersCount_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupPossessedPhone_SpinsCount = Mathf.Max(0, n);
	}

	// Token: 0x06000273 RID: 627
	public static float Powerup_GoldenKingMida_ExtraBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupGoldenKingMida_ExtraBonus;
	}

	// Token: 0x06000274 RID: 628
	public static void Powerup_GoldenKingMida_ExtraBonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupGoldenKingMida_ExtraBonus = value;
	}

	// Token: 0x06000275 RID: 629
	public static float Powerup_Dealer_ExtraBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupDealer_ExtraBonus;
	}

	// Token: 0x06000276 RID: 630
	public static void Powerup_Dealer_ExtraBonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupDealer_ExtraBonus = value;
	}

	// Token: 0x06000277 RID: 631
	public static float Powerup_Capitalist_ExtraBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupCapitalist_ExtraBonus;
	}

	// Token: 0x06000278 RID: 632
	public static void Powerup_Capitalist_ExtraBonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupCapitalist_ExtraBonus = value;
	}

	// Token: 0x06000279 RID: 633
	public static float Powerup_PersonalTrainer_BonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupPersonalTrainer_Bonus;
	}

	// Token: 0x0600027A RID: 634
	public static void Powerup_PersonalTrainer_BonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupPersonalTrainer_Bonus = value;
	}

	// Token: 0x0600027B RID: 635
	public static void Powerup_PersonalTrainer_BonusReset()
	{
		GameplayData.Powerup_PersonalTrainer_BonusSet(0.25f);
	}

	// Token: 0x0600027C RID: 636
	public static float Powerup_Electrician_BonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupElectrician_Bonus;
	}

	// Token: 0x0600027D RID: 637
	public static void Powerup_Electrician_BonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupElectrician_Bonus = value;
	}

	// Token: 0x0600027E RID: 638
	public static void Powerup_Electrician_BonusReset()
	{
		GameplayData.Powerup_Electrician_BonusSet(0.05f);
	}

	// Token: 0x0600027F RID: 639
	public static float Powerup_FortuneTeller_BonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupFortuneTeller_Bonus;
	}

	// Token: 0x06000280 RID: 640
	public static void Powerup_FortuneTeller_BonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupFortuneTeller_Bonus = value;
	}

	// Token: 0x06000281 RID: 641
	public static void Powerup_FortuneTeller_BonusReset()
	{
		GameplayData.Powerup_FortuneTeller_BonusSet(0.25f);
	}

	// Token: 0x06000282 RID: 642
	public static long Powerup_AceOfClubs_TicketsSpentGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupAceOfClubs_TicketsSpent;
	}

	// Token: 0x06000283 RID: 643
	public static void Powerup_AceOfClubs_TicketsSpentSet(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupAceOfClubs_TicketsSpent = value;
	}

	// Token: 0x06000284 RID: 644
	public static long Powerup_AceOfSpades_ActivationsCounterGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupAceOfSpades_ActivationsCounter;
	}

	// Token: 0x06000285 RID: 645
	public static void Powerup_AceOfSpades_ActivationsCounterSet(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupAceOfSpades_ActivationsCounter = value;
	}

	// Token: 0x06000286 RID: 646
	public static PowerupScript.Identifier PowerupHoleCircle_CharmGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return PowerupScript.Identifier.undefined;
		}
		return instance._powerupHoleCircle_CharmIdentifier;
	}

	// Token: 0x06000287 RID: 647
	public static void PowerupHoleCircle_CharmSet(PowerupScript.Identifier identifier)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupHoleCircle_CharmIdentifier = identifier;
	}

	// Token: 0x06000288 RID: 648
	public static PowerupScript.Identifier PowerupHoleRomboid_CharmGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return PowerupScript.Identifier.undefined;
		}
		return instance._powerupHoleRomboid_CharmIdentifier;
	}

	// Token: 0x06000289 RID: 649
	public static void PowerupHoleRomboid_CharmSet(PowerupScript.Identifier identifier)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupHoleRomboid_CharmIdentifier = identifier;
	}

	// Token: 0x0600028A RID: 650
	public static AbilityScript.Identifier PowerupHoleCross_AbilityGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return AbilityScript.Identifier.undefined;
		}
		return instance._powerupHoleCross_AbilityIdentifier;
	}

	// Token: 0x0600028B RID: 651
	public static void PowerupHoleCross_AbilitySet(AbilityScript.Identifier identifier)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupHoleCross_AbilityIdentifier = identifier;
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x0600028C RID: 652
	// (set) Token: 0x0600028D RID: 653
	public static int PowerupOphanimWheels_JackpotsCounter
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._powerupOphanimWheels_JackpotsCounter;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._powerupOphanimWheels_JackpotsCounter = Mathf.Max(0, value);
		}
	}

	// Token: 0x0600028E RID: 654
	public static int MaxEquippablePowerupsGet(bool considerPowerups)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 7;
		}
		int num = instance.maxEquippablePowerups;
		if (considerPowerups)
		{
			num += PowerupScript.HouseContractBonusGet(true);
			num -= PowerupScript.Button2x_SpaceMalusGet(true);
			num -= PowerupScript.Megaphone_SpaceMalusGet(true);
			num -= ((PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Hourglass) > false) ? 1 : 0);
		}
		return num;
	}

	// Token: 0x0600028F RID: 655
	public static void MaxEquippablePowerupsSet(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		if (value > instance.maxEquippablePowerups)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.HouseContract);
		}
		instance.maxEquippablePowerups = value;
		if (instance.maxEquippablePowerups < 1)
		{
			instance.maxEquippablePowerups = 1;
		}
		int num = GameplayData._MaxEquippablePowerupsGet_AbsoluteMaximum();
		if (instance.maxEquippablePowerups > num)
		{
			instance.maxEquippablePowerups = num;
		}
	}

	// Token: 0x06000290 RID: 656
	public static void MaxEquippablePowerupsAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.MaxEquippablePowerupsSet(instance.maxEquippablePowerups + value);
	}

	// Token: 0x06000291 RID: 657
	public static void MaxEquippablePowerupsReset()
	{
		GameplayData.MaxEquippablePowerupsSet(7);
	}

	// Token: 0x06000292 RID: 658
	private static int _MaxEquippablePowerupsGet_AbsoluteMaximum()
	{
		return ItemOrganizerScript.CharmsSlotN();
	}

	// Token: 0x06000293 RID: 659
	public static float PowerupCoinsMultiplierGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1f;
		}
		return instance.powerupCoinsMultiplier;
	}

	// Token: 0x06000294 RID: 660
	public static void PowerupCoinsMultiplierSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.powerupCoinsMultiplier = value;
		if (instance.powerupCoinsMultiplier < 1f)
		{
			instance.powerupCoinsMultiplier = 1f;
		}
	}

	// Token: 0x06000295 RID: 661
	public static void PowerupCoinsMultiplierAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.PowerupCoinsMultiplierSet(instance.powerupCoinsMultiplier + value);
	}

	// Token: 0x06000296 RID: 662
	public static void PowerupCoinsMultiplierReset()
	{
		GameplayData.PowerupCoinsMultiplierSet(1f);
	}

	// Token: 0x06000297 RID: 663
	public static int RedButtonActivationsMultiplierGet(bool considerPowerups)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1;
		}
		int num = instance._redButtonActivationsMultiplier;
		if (considerPowerups)
		{
			num += PowerupScript.Button2X_ActivationsMultiplierBonusGet(true);
		}
		if (GameplayData.RunModifier_GetCurrent() == RunModifierScript.Identifier.redButtonOverload)
		{
			num++;
		}
		return num;
	}

	// Token: 0x06000298 RID: 664
	public static void RedButtonActivationsMultiplierSet(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._redButtonActivationsMultiplier = value;
		if (instance._redButtonActivationsMultiplier < 1)
		{
			instance._redButtonActivationsMultiplier = 1;
		}
	}

	// Token: 0x06000299 RID: 665
	public static void RedButtonActivationsMultiplierAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.RedButtonActivationsMultiplierSet(instance._redButtonActivationsMultiplier + value);
	}

	// Token: 0x0600029A RID: 666
	private void _Phone_PrepareForSerialization()
	{
		this.phoneAbilitiesPickHistory_AsString = PlatformDataMaster.EnumListToString<AbilityScript.Identifier>(this.phoneAbilitiesPickHistory, ',');
		this._phone_AbilitiesToPick_String = PlatformDataMaster.EnumListToString<AbilityScript.Identifier>(this._phone_AbilitiesToPick, ',');
		this.nineNineNine_TotalRewardEarned_ByteArray = this.nineNineNine_TotalRewardEarned.ToByteArray();
	}

	// Token: 0x0600029B RID: 667
	private void _Phone_RestoreFromSerialization()
	{
		this.phoneAbilitiesPickHistory = PlatformDataMaster.EnumListFromString<AbilityScript.Identifier>(this.phoneAbilitiesPickHistory_AsString, ',');
		this._phone_AbilitiesToPick = PlatformDataMaster.EnumListFromString<AbilityScript.Identifier>(this._phone_AbilitiesToPick_String, ',');
		this.nineNineNine_TotalRewardEarned = this.BigIntegerFromByteArray(this.nineNineNine_TotalRewardEarned_ByteArray, 0);
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x0600029C RID: 668
	// (set) Token: 0x0600029D RID: 669
	public static int AbilityHoly_PatternsRepetitions
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0;
			}
			return instance._abilityHoly_PatternsRepetitions;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._abilityHoly_PatternsRepetitions = Mathf.Max(0, value);
		}
	}

	// Token: 0x0600029E RID: 670
	public static void Phone_SpeciallCallBooking_Reset()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._phone_bookSpecialCall = false;
	}

	// Token: 0x0600029F RID: 671
	public static bool NineNineNine_IsTime()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance._phone_SpecialCalls_Counter > 3 && instance._phone_EvilCallsPicked_Counter <= 0;
	}

	// Token: 0x060002A0 RID: 672
	public static int Phone_Ignored666CallsLevel_DefineAndReturn()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		if (instance._phone_EvilCallsPicked_Counter > 0)
		{
			instance._phone_EvilCallsIgnored_Counter = 0;
			return 0;
		}
		int phone_SpecialCalls_Counter = instance._phone_SpecialCalls_Counter;
		if (instance._phone_EvilCallsIgnored_Counter < phone_SpecialCalls_Counter)
		{
			instance._phone_EvilCallsIgnored_Counter = phone_SpecialCalls_Counter;
		}
		return Mathf.Clamp(instance._phone_EvilCallsIgnored_Counter, 0, 3);
	}

	// Token: 0x060002A1 RID: 673
	public static int Phone_Ignored666CallsLevel_JustGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		if (instance._phone_EvilCallsPicked_Counter > 0)
		{
			instance._phone_EvilCallsIgnored_Counter = 0;
		}
		return instance._phone_EvilCallsIgnored_Counter;
	}

	// Token: 0x060002A2 RID: 674
	public static int PhoneAbilitiesNumber_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance._phoneAbilitiesNumber;
	}

	// Token: 0x060002A3 RID: 675
	public static void PhoneAbilitiesNumber_SetToMAX()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._phoneAbilitiesNumber = 4;
	}

	// Token: 0x060002A4 RID: 676
	public static void PhoneAbilitiesNumber_SetToDefault()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._phoneAbilitiesNumber = 3;
	}

	// Token: 0x060002A5 RID: 677
	public static long PhoneRerollCostGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1L;
		}
		return instance._phoneRerollCost;
	}

	// Token: 0x060002A6 RID: 678
	public static void PhoneRerollCostReset(bool considerDeadline)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		long num = GameplayData.DebtIndexGet().CastToLong() - 1L;
		if (num < 0L)
		{
			num = 0L;
		}
		if (!considerDeadline)
		{
			num = 0L;
		}
		instance._phoneRerollCost = 1L + num;
	}

	// Token: 0x060002A7 RID: 679
	public static void PhoneRerollCostIncrease()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._phoneRerollCost += instance._phoneRerollCostIncrease;
	}

	// Token: 0x060002A8 RID: 680
	public static int PhonePickMultiplierGet(bool considerPowerups)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1;
		}
		int num = instance._phonePickMultiplier;
		if (considerPowerups)
		{
			num += PowerupScript.Megaphone_PickMultiplierBonusGet(true);
		}
		if (GameplayData.RunModifier_GetCurrent() == RunModifierScript.Identifier.phoneEnhancer)
		{
			num++;
		}
		return num;
	}

	// Token: 0x060002A9 RID: 681
	public static void PhonePickMultiplierSet(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._phonePickMultiplier = value;
		if (instance._phonePickMultiplier < 1)
		{
			instance._phonePickMultiplier = 1;
		}
	}

	// Token: 0x060002AA RID: 682
	public static void PhonePickMultiplierAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.PhonePickMultiplierSet(instance._phonePickMultiplier + value);
	}

	// Token: 0x060002AB RID: 683
	public static BigInteger NineNineNne_TotalRewardEarned_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.nineNineNine_TotalRewardEarned;
	}

	// Token: 0x060002AC RID: 684
	public static void NineNineNne_TotalRewardEarned_Set(BigInteger n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.nineNineNine_TotalRewardEarned = n;
	}

	// Token: 0x060002AD RID: 685
	public static void PhoneAbilities_GetCount(ref int normalCount, ref int evilCount, ref int goodCount)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			normalCount = 0;
			evilCount = 0;
			goodCount = 0;
			return;
		}
		if (instance.phoneAbilitiesPickHistory.Count == instance._phoneAbilChahce_CounterPrevAbilitiesCount)
		{
			normalCount = instance._phoneAbilChache_normalCount;
			evilCount = instance._phoneAbilChache_evilCount;
			goodCount = instance._phoneAbilChache_holyCount;
			return;
		}
		instance._phoneAbilChache_normalCount = 0;
		instance._phoneAbilChache_evilCount = 0;
		instance._phoneAbilChache_holyCount = 0;
		for (int i = 0; i < instance.phoneAbilitiesPickHistory.Count; i++)
		{
			AbilityScript.Identifier identifier = instance.phoneAbilitiesPickHistory[i];
			if (identifier != AbilityScript.Identifier.undefined && identifier != AbilityScript.Identifier.count)
			{
				AbilityScript abilityScript = AbilityScript.AbilityGet(identifier);
				if (abilityScript != null)
				{
					switch (abilityScript.CategoryGet())
					{
					case AbilityScript.Category.normal:
						instance._phoneAbilChache_normalCount++;
						break;
					case AbilityScript.Category.evil:
						instance._phoneAbilChache_evilCount++;
						break;
					case AbilityScript.Category.good:
						instance._phoneAbilChache_holyCount++;
						break;
					}
				}
			}
		}
		normalCount = instance._phoneAbilChache_normalCount;
		evilCount = instance._phoneAbilChache_evilCount;
		goodCount = instance._phoneAbilChache_holyCount;
	}

	// Token: 0x060002AE RID: 686
	public static int PhoneAbilities_GetCount_Normal()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		GameplayData.PhoneAbilities_GetCount(ref instance._pTempNormal, ref instance._pTempEvil, ref instance._pTempGood);
		return instance._pTempNormal;
	}

	// Token: 0x060002AF RID: 687
	public static int PhoneAbilities_GetCount_Evil()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		GameplayData.PhoneAbilities_GetCount(ref instance._pTempNormal, ref instance._pTempEvil, ref instance._pTempGood);
		return instance._pTempEvil;
	}

	// Token: 0x060002B0 RID: 688
	public static int PhoneAbilities_GetCount_Good()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		GameplayData.PhoneAbilities_GetCount(ref instance._pTempNormal, ref instance._pTempEvil, ref instance._pTempGood);
		return instance._pTempGood;
	}

	// Token: 0x060002B1 RID: 689
	public static int PhoneAbilities_GetSkippedCount_Total()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.phoneEasyCounter_SkippedCalls_Total;
	}

	// Token: 0x060002B2 RID: 690
	public static int PhoneAbilities_GetSkippedCount_Normal()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.phoneEasyCounter_SkippedCalls_Normal;
	}

	// Token: 0x060002B3 RID: 691
	public static int PhoneAbilities_GetSkippedCount_Evil()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.phoneEasyCounter_SkippedCalls_Evil;
	}

	// Token: 0x060002B4 RID: 692
	public static int PhoneAbilities_GetSkippedCount_Good()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.phoneEasyCounter_SkippedCalls_Good;
	}

	// Token: 0x060002B5 RID: 693
	public static long PhoneRerollPerformed_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._phoneRerollsPerformed;
	}

	// Token: 0x060002B6 RID: 694
	public static void PhoneRerollPerformed_Set(long n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		if (n < 0L)
		{
			n = 0L;
		}
		instance._phoneRerollsPerformed = n;
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x060002B7 RID: 695
	// (set) Token: 0x060002B8 RID: 696
	public static long PhoneRerollPerformed_PerDeadline
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0L;
			}
			return instance._phoneRerollsPerformed_PerDeadline;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance._phoneRerollsPerformed_PerDeadline = value;
		}
	}

	// Token: 0x060002B9 RID: 697
	private void RunModifiers_SavePreparing()
	{
		if (this.runModifierPicked == RunModifierScript.Identifier.undefined || this.runModifierPicked == RunModifierScript.Identifier.count)
		{
			this.runModifierPicked = RunModifierScript.Identifier.defaultModifier;
		}
		this.runModifierPicked_AsString = PlatformDataMaster.EnumEntryToString<RunModifierScript.Identifier>(this.runModifierPicked);
	}

	// Token: 0x060002BA RID: 698
	private void RunModifiers_LoadFormat()
	{
		this.runModifierPicked = PlatformDataMaster.EnumEntryFromString<RunModifierScript.Identifier>(this.runModifierPicked_AsString, RunModifierScript.Identifier.defaultModifier);
		if (this.runModifierPicked == RunModifierScript.Identifier.undefined || this.runModifierPicked == RunModifierScript.Identifier.count)
		{
			this.runModifierPicked = RunModifierScript.Identifier.defaultModifier;
		}
	}

	// Token: 0x060002BB RID: 699
	public static RunModifierScript.Identifier RunModifier_GetCurrent()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return RunModifierScript.Identifier.defaultModifier;
		}
		return instance.runModifierPicked;
	}

	// Token: 0x060002BC RID: 700
	public static void RunModifier_SetCurrent(RunModifierScript.Identifier identifier, bool setByPlayer)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		if (instance._runModifier_AlreadySet)
		{
			Debug.LogWarning("Seems like you already set a run modifier. Are you testing stuff?");
		}
		instance._runModifier_AlreadySet = true;
		instance.runModifierPicked = identifier;
		if (setByPlayer)
		{
			if (!GameplayMaster.IsCustomSeed())
			{
				Data.game.RunModifier_PlayedTimes_Set(identifier, Data.game.RunModifier_PlayedTimes_Get(identifier) + 1);
				bool flag = Data.game.DesiredFoilLevelGet(identifier) >= 2 || Data.game.RunModifier_FoilLevel_Get(identifier) >= 2;
				if (identifier != RunModifierScript.Identifier.defaultModifier && !flag)
				{
					int num = Data.game.RunModifier_OwnedCount_Get(identifier);
					if (num > 0)
					{
						num--;
					}
					Data.game.RunModifier_OwnedCount_Set(identifier, num);
				}
			}
			RunModifierScript.OnRunModifierSet(identifier);
		}
	}

	// Token: 0x060002BD RID: 701
	public static bool RunModifier_AlreadyPicked()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance._runModifier_AlreadySet;
	}

	// Token: 0x060002BE RID: 702
	public static bool RunModifier_DealIsAvailable_Get()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.runModifier_DealIsAvailable;
	}

	// Token: 0x060002BF RID: 703
	public static void RunModifier_DealIsAvailable_Set(bool value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.runModifier_DealIsAvailable = value;
	}

	// Token: 0x060002C0 RID: 704
	public static int RunModifier_AcceptedDealsCounter_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.runModifier_AcceptedDealsCounter;
	}

	// Token: 0x060002C1 RID: 705
	public static void RunModifier_AcceptedDealsCounter_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.runModifier_AcceptedDealsCounter = Mathf.Max(0, n);
	}

	// Token: 0x060002C2 RID: 706
	public static int RunModifier_BonusPacksThisTime_Get()
	{
		RunModifierScript.Identifier identifier = GameplayData.RunModifier_GetCurrent();
		int num = 0;
		int num2 = GameplayData.RunModifier_AcceptedDealsCounter_Get() + 1;
		Data.GameData game = Data.game;
		if (identifier != RunModifierScript.Identifier.undefined && identifier != RunModifierScript.Identifier.count && game != null && game.RunModifier_HardcoreMode_Get(identifier))
		{
			num2 *= 2;
		}
		return num2 + num;
	}

	// Token: 0x060002C3 RID: 707
	private void _MetaProgression_PrepareForSerialization()
	{
	}

	// Token: 0x060002C4 RID: 708
	private void _MetaProgression_RestoreFromSerialization()
	{
	}

	// Token: 0x060002C5 RID: 709
	public static bool AlreadyBoughtPowerupAtTerminalGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance._alreadyBoughtPowerupAtTerminal;
	}

	// Token: 0x060002C6 RID: 710
	public static void AlreadyBoughtPowerupAtTerminalSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._alreadyBoughtPowerupAtTerminal = true;
	}

	// Token: 0x060002C7 RID: 711
	public static bool NewGameIntroFinished_Get()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.newGameIntro_Finished;
	}

	// Token: 0x060002C8 RID: 712
	public static void NewGameIntroFinished_Set(bool finished)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.newGameIntro_Finished = finished;
	}

	// Token: 0x060002C9 RID: 713
	private void _Ending_PrepareForSerialization()
	{
		this.rewardBoxDebtIndex_ByteArray = this.rewardBoxDebtIndex.ToByteArray();
	}

	// Token: 0x060002CA RID: 714
	private void _Ending_RestoreFromSerialization()
	{
		this.rewardBoxDebtIndex = this.BigIntegerFromByteArray(this.rewardBoxDebtIndex_ByteArray, -1);
	}

	// Token: 0x060002CB RID: 715
	public static bool CanGetSkeletonKey()
	{
		if (GameplayData.Instance == null)
		{
			return false;
		}
		int count = PowerupScript.list_EquippedSkeleton.Count;
		if (count < 5)
		{
			return false;
		}
		for (int i = 0; i < count; i++)
		{
			if (PowerupScript.list_EquippedSkeleton[i] == null)
			{
				return false;
			}
			if (PowerupScript.list_EquippedSkeleton[i].identifier == PowerupScript.Identifier.undefined)
			{
				return false;
			}
			if (PowerupScript.list_EquippedSkeleton[i].identifier == PowerupScript.Identifier.count)
			{
				return false;
			}
			if (PowerupScript.list_EquippedSkeleton[i].archetype != PowerupScript.Archetype.skeleton)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060002CC RID: 716
	public static BigInteger GetRewardBoxDebtIndex()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 7;
		}
		int drawersUnlockedNum = DrawersScript.GetDrawersUnlockedNum();
		BigInteger bigInteger = GameplayData.DebtIndexGet();
		switch (drawersUnlockedNum)
		{
		case 0:
			instance.rewardBoxDebtIndex = 4;
			break;
		case 1:
			instance.rewardBoxDebtIndex = 5;
			break;
		case 2:
			instance.rewardBoxDebtIndex = 6;
			break;
		case 3:
			instance.rewardBoxDebtIndex = 7;
			break;
		default:
			if (!GameplayData.CanGetSkeletonKey() && !instance.doorKeyDeadlineDefined)
			{
				instance.rewardBoxDebtIndex = bigInteger + 3;
				if (instance.rewardBoxDebtIndex < 7L)
				{
					instance.rewardBoxDebtIndex = 7;
				}
			}
			else
			{
				instance.doorKeyDeadlineDefined = true;
			}
			break;
		}
		return instance.rewardBoxDebtIndex;
	}

	// Token: 0x060002CD RID: 717
	public static BigInteger GetRewardDeadlineDebt()
	{
		if (GameplayData.Instance == null)
		{
			return 100000;
		}
		return GameplayData.DebtGetExt(GameplayData.GetRewardBoxDebtIndex());
	}

	// Token: 0x060002CE RID: 718
	public static bool RewardTimeToShowAmmount()
	{
		return GameplayData.Instance != null && GameplayData.DebtIndexGet() >= GameplayData.GetRewardBoxDebtIndex();
	}

	// Token: 0x060002CF RID: 719
	public static bool WinConditionAlreadyAchieved()
	{
		return GameplayData.Instance != null && (GameplayData.DebtIndexGet() > GameplayData.GetRewardBoxDebtIndex() || RewardBoxScript.IsOpened());
	}

	// Token: 0x060002D0 RID: 720
	public static bool KeptPlayingPastWinConditionGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.keptPlayingPastWinCondition;
	}

	// Token: 0x060002D1 RID: 721
	public static void KeptPlayingPastWinConditionSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.keptPlayingPastWinCondition = true;
	}

	// Token: 0x060002D2 RID: 722
	public static bool RewardBoxIsOpened()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.rewardBoxWasOpened;
	}

	// Token: 0x060002D3 RID: 723
	public static void RewardBoxSetOpened()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.rewardBoxWasOpened = true;
	}

	// Token: 0x060002D4 RID: 724
	public static bool RewardBoxHasPrize()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.rewardBoxHasPrize;
	}

	// Token: 0x060002D5 RID: 725
	public static void RewardBoxPickupPrize()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.rewardBoxHasPrize = false;
	}

	// Token: 0x060002D6 RID: 726
	public static bool PrizeWasUsedGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.prizeWasUsed;
	}

	// Token: 0x060002D7 RID: 727
	public static void PrizeWasUsedSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.prizeWasUsed = true;
	}

	// Token: 0x060002D8 RID: 728
	public static bool IsInVictoryCondition()
	{
		return GameplayData.Instance != null && !GameplayMaster.GameIsResetting() && GameplayData.RewardBoxIsOpened() && !GameplayData.RewardBoxHasPrize() && GameplayData.PrizeWasUsedGet();
	}

	// Token: 0x060002D9 RID: 729
	public static bool IsInGoodEndingCondition(bool considerKeyState)
	{
		if (!considerKeyState)
		{
			return GameplayData.NineNineNine_IsTime();
		}
		return GameplayData.IsInVictoryCondition() && GameplayData.NineNineNine_IsTime();
	}

	// Token: 0x060002DA RID: 730
	public static bool IsInBadEndingCondition(bool considerKeyState)
	{
		if (!considerKeyState)
		{
			return !GameplayData.NineNineNine_IsTime();
		}
		return GameplayData.IsInVictoryCondition() && !GameplayData.NineNineNine_IsTime();
	}

	// Token: 0x060002DB RID: 731
	private static RewardBoxScript.RewardKind DefaultRewardKind()
	{
		switch (DrawersScript.GetDrawersUnlockedNum())
		{
		case 0:
			return RewardBoxScript.RewardKind.DrawerKey0;
		case 1:
			return RewardBoxScript.RewardKind.DrawerKey1;
		case 2:
			return RewardBoxScript.RewardKind.DrawerKey2;
		case 3:
			return RewardBoxScript.RewardKind.DrawerKey3;
		case 4:
			return RewardBoxScript.RewardKind.DoorKey;
		default:
			Debug.LogError("RewardBoxScript: Undefined reward kind");
			return RewardBoxScript.RewardKind.Undefined;
		}
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x060002DC RID: 732
	// (set) Token: 0x060002DD RID: 733
	public static RewardBoxScript.RewardKind RewardKind
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return GameplayData.DefaultRewardKind();
			}
			if (instance.rewardKind == 7)
			{
				return GameplayData.DefaultRewardKind();
			}
			return (RewardBoxScript.RewardKind)instance.rewardKind;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance.rewardKind = (int)value;
		}
	}

	// Token: 0x060002DE RID: 734
	public static long Stats_ModifiedLemonTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedLemonTriggeredTimes;
	}

	// Token: 0x060002DF RID: 735
	public static void Stats_ModifiedLemonTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedLemonTriggeredTimes += 1L;
	}

	// Token: 0x060002E0 RID: 736
	public static long Stats_ModifiedCherryTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedCherryTriggeredTimes;
	}

	// Token: 0x060002E1 RID: 737
	public static void Stats_ModifiedCherryTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedCherryTriggeredTimes += 1L;
	}

	// Token: 0x060002E2 RID: 738
	public static long Stats_ModifiedCloverTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedCloverTriggeredTimes;
	}

	// Token: 0x060002E3 RID: 739
	public static void Stats_ModifiedCloverTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedCloverTriggeredTimes += 1L;
	}

	// Token: 0x060002E4 RID: 740
	public static long Stats_ModifiedBellTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedBellTriggeredTimes;
	}

	// Token: 0x060002E5 RID: 741
	public static void Stats_ModifiedBellTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedBellTriggeredTimes += 1L;
	}

	// Token: 0x060002E6 RID: 742
	public static long Stats_ModifiedDiamondTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedDiamondTriggeredTimes;
	}

	// Token: 0x060002E7 RID: 743
	public static void Stats_ModifiedDiamondTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedDiamondTriggeredTimes += 1L;
	}

	// Token: 0x060002E8 RID: 744
	public static long Stats_ModifiedCoinsTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedCoinsTriggeredTimes;
	}

	// Token: 0x060002E9 RID: 745
	public static void Stats_ModifiedCoinsTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedCoinsTriggeredTimes += 1L;
	}

	// Token: 0x060002EA RID: 746
	public static long Stats_ModifiedSevenTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedSevenTriggeredTimes;
	}

	// Token: 0x060002EB RID: 747
	public static void Stats_ModifiedSevenTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedSevenTriggeredTimes += 1L;
	}

	// Token: 0x060002EC RID: 748
	public static long Stats_ModifiedSymbol_TriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedSymbolTriggeredTimes;
	}

	// Token: 0x060002ED RID: 749
	public static void Stats_ModifiedSymbol_TriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedSymbolTriggeredTimes += 1L;
	}

	// Token: 0x060002EE RID: 750
	public static int Stats_RedButtonEffectiveActivations_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.stats_RedButtonEffectiveActivationsCounter;
	}

	// Token: 0x060002EF RID: 751
	public static void Stats_RedButtonEffectiveActivations_Set(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_RedButtonEffectiveActivationsCounter = value;
	}

	// Token: 0x060002F0 RID: 752
	public static long Stats_RestocksBoughtGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_RestocksBoughtN;
	}

	// Token: 0x060002F1 RID: 753
	public static void Stats_RestocksBoughtSet(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_RestocksBoughtN = value;
		if (instance.stats_RestocksBoughtN < 0L)
		{
			instance.stats_RestocksBoughtN = 0L;
		}
	}

	// Token: 0x060002F2 RID: 754
	public static long Stats_RestocksPerformedGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_RestocksPerformed;
	}

	// Token: 0x060002F3 RID: 755
	public static void Stats_RestocksPerformedSet(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_RestocksPerformed = value;
		if (instance.stats_RestocksPerformed < 0L)
		{
			instance.stats_RestocksPerformed = 0L;
		}
	}

	// Token: 0x060002F4 RID: 756
	public static long Stats_PeppersBoughtGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_PeppersBought;
	}

	// Token: 0x060002F5 RID: 757
	public static void Stats_PeppersBoughtAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_PeppersBought += 1L;
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x060002F6 RID: 758
	// (set) Token: 0x060002F7 RID: 759
	public static long Stats_SixSixSix_SeenTimes
	{
		get
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return 0L;
			}
			return instance.sixSixSixSeen;
		}
		set
		{
			GameplayData instance = GameplayData.Instance;
			if (instance == null)
			{
				return;
			}
			instance.sixSixSixSeen = value;
		}
	}

	// Token: 0x060002F8 RID: 760
	private void _GameStats_PrepareForSerialization()
	{
		this.stats_CoinsEarned_ByteArray = this.stats_CoinsEarned.ToByteArray();
	}

	// Token: 0x060002F9 RID: 761
	private void _GameStats_RestoreFromSerialization()
	{
		this.stats_CoinsEarned = this.BigIntegerFromByteArray(this.stats_CoinsEarned_ByteArray, 0);
	}

	// Token: 0x060002FA RID: 762
	public static long Stats_PlayTime_GetSeconds()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_PlayTime_Seconds;
	}

	// Token: 0x060002FB RID: 763
	public static void Stats_PlayTime_AddSeconds(long seconds)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_PlayTime_Seconds += seconds;
		if (instance.stats_PlayTime_Seconds < 0L)
		{
			instance.stats_PlayTime_Seconds = 0L;
		}
	}

	// Token: 0x060002FC RID: 764
	public static long Stats_DeadlinesCompleted_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_DeadlinesCompleted;
	}

	// Token: 0x060002FD RID: 765
	public static void Stats_DeadlinesCompleted_Add()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_DeadlinesCompleted += 1L;
	}

	// Token: 0x060002FE RID: 766
	public static BigInteger Stats_CoinsEarned_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.stats_CoinsEarned;
	}

	// Token: 0x060002FF RID: 767
	public static void Stats_CoinsEarned_Add(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_CoinsEarned += value;
		if (instance.stats_CoinsEarned < 0L)
		{
			instance.stats_CoinsEarned = 0;
		}
	}

	// Token: 0x06000300 RID: 768
	public static long Stats_TicketsEarned_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_TicketsEarned;
	}

	// Token: 0x06000301 RID: 769
	public static void Stats_TicketsEarned_Add(long ammount)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_TicketsEarned += ammount;
		if (instance.stats_TicketsEarned < 0L)
		{
			instance.stats_TicketsEarned = 0L;
		}
	}

	// Token: 0x06000302 RID: 770
	public static long Stats_CharmsBought_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_CharmsBought;
	}

	// Token: 0x06000303 RID: 771
	public static void Stats_CharmsBought_Add()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_CharmsBought += 1L;
	}

	// Token: 0x0400010A RID: 266
	[SerializeField]
	private int seed;

	// Token: 0x0400010B RID: 267
	[SerializeField]
	public Rng rngRunMod;

	// Token: 0x0400010C RID: 268
	[SerializeField]
	public Rng rngSymbolsMod;

	// Token: 0x0400010D RID: 269
	[SerializeField]
	public Rng rngPowerupsMod;

	// Token: 0x0400010E RID: 270
	[SerializeField]
	public Rng rngSymbolsChance;

	// Token: 0x0400010F RID: 271
	[SerializeField]
	public Rng rngCards;

	// Token: 0x04000110 RID: 272
	[SerializeField]
	public Rng rngPowerupsAll;

	// Token: 0x04000111 RID: 273
	[SerializeField]
	public Rng rngAbilities;

	// Token: 0x04000112 RID: 274
	[SerializeField]
	public Rng rngDrawers;

	// Token: 0x04000113 RID: 275
	[SerializeField]
	public Rng rngStore;

	// Token: 0x04000114 RID: 276
	[SerializeField]
	public Rng rngStoreChains;

	// Token: 0x04000115 RID: 277
	[SerializeField]
	public Rng rngPhone;

	// Token: 0x04000116 RID: 278
	[SerializeField]
	public Rng rngSlotMachineLuck;

	// Token: 0x04000117 RID: 279
	[SerializeField]
	public Rng rng666;

	// Token: 0x04000118 RID: 280
	[SerializeField]
	public Rng rngGarbage;

	// Token: 0x04000119 RID: 281
	public string[] equippedPowerups = new string[]
	{
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined)
	};

	// Token: 0x0400011A RID: 282
	public string[] equippedPowerups_Skeleton = new string[]
	{
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined)
	};

	// Token: 0x0400011B RID: 283
	public string[] drawerPowerups = new string[]
	{
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined)
	};

	// Token: 0x0400011C RID: 284
	public string[] storePowerups = new string[4];

	// Token: 0x0400011D RID: 285
	public int storeChainIndex_Array;

	// Token: 0x0400011E RID: 286
	public int storeChainIndex_PowerupIdentifier;

	// Token: 0x0400011F RID: 287
	public int storeLastRandomIndex;

	// Token: 0x04000120 RID: 288
	private BigInteger _storeRestockExtraCost = 0;

	// Token: 0x04000121 RID: 289
	[SerializeField]
	private byte[] _storeRestockExtraCost_ByteArray;

	// Token: 0x04000122 RID: 290
	[SerializeField]
	private long _storeFreeRestocks = 1L;

	// Token: 0x04000123 RID: 291
	[SerializeField]
	private long temporaryDiscount;

	// Token: 0x04000124 RID: 292
	[SerializeField]
	private long[] temporaryDiscountPerSlot = new long[4];

	// Token: 0x04000125 RID: 293
	private const int COINS_INITIAL = 13;

	// Token: 0x04000126 RID: 294
	private const int COINS_DEPOSITED_INITIAL = 30;

	// Token: 0x04000127 RID: 295
	private const int COINS_INTEREST_EARNED_INITIAL = 0;

	// Token: 0x04000128 RID: 296
	private const float COINS_INTEREST_RATE = 7f;

	// Token: 0x04000129 RID: 297
	private BigInteger coins = 13;

	// Token: 0x0400012A RID: 298
	private BigInteger depositedCoins = 30;

	// Token: 0x0400012B RID: 299
	private BigInteger interestEarned = 0;

	// Token: 0x0400012C RID: 300
	[SerializeField]
	private float interestRate = 7f;

	// Token: 0x0400012D RID: 301
	private BigInteger roundEarnedCoins = 0;

	// Token: 0x0400012E RID: 302
	[SerializeField]
	private byte[] coins_ByteArray;

	// Token: 0x0400012F RID: 303
	[SerializeField]
	private byte[] depositedCoins_ByteArray;

	// Token: 0x04000130 RID: 304
	[SerializeField]
	private byte[] interestEarned_ByteArray;

	// Token: 0x04000131 RID: 305
	[SerializeField]
	private byte[] roundEarnedCoins_ByteArray;

	// Token: 0x04000132 RID: 306
	public const long CLOVER_TICKETS_INITIAL = 4L;

	// Token: 0x04000133 RID: 307
	private const long CLOVER_TICKETS_BONUS_FOR_LITTLE_BET = 2L;

	// Token: 0x04000134 RID: 308
	private const long CLOVER_TICKETS_BONUS_FOR_BIG_BET = 1L;

	// Token: 0x04000135 RID: 309
	private const long CLOVER_TICKETS_BONUS_FOR_ROUNDS_LEFT = 4L;

	// Token: 0x04000136 RID: 310
	[SerializeField]
	private long cloverTickets = 4L;

	// Token: 0x04000137 RID: 311
	[SerializeField]
	private long cloverTickets_BonusFor_LittleBet = 2L;

	// Token: 0x04000138 RID: 312
	[SerializeField]
	private long cloverTickets_BonusFor_BigBet = 1L;

	// Token: 0x04000139 RID: 313
	[SerializeField]
	private long cloverTickets_BonusFor_RoundsLeft = 4L;

	// Token: 0x0400013A RID: 314
	[SerializeField]
	private bool atmDeadline_RewardPickupMemo_MessageShown;

	// Token: 0x0400013B RID: 315
	private const int ROUNDS_PER_DEADLINE_DEFAULT = 3;

	// Token: 0x0400013C RID: 316
	private const int ROUNDS_PER_DEADLINE_RUN_MOD_MORE_ROUNDS_SMALL_ROUNDS = 7;

	// Token: 0x0400013D RID: 317
	private const int ROUNDS_PER_DEADLINE_RUN_MOD_ONE_ROUND_PER_DEADLINE = 1;

	// Token: 0x0400013E RID: 318
	private const int DEBT_INDEX_DEFAULT = 0;

	// Token: 0x0400013F RID: 319
	public const int DEBT_OUT_OF_RANGE_MULTIPLIER_DEFAULT = 6;

	// Token: 0x04000140 RID: 320
	[SerializeField]
	private int roundDeadlineTrail;

	// Token: 0x04000141 RID: 321
	[SerializeField]
	private int roundDeadlineTrail_AtDeadlineBegin;

	// Token: 0x04000142 RID: 322
	[SerializeField]
	private int roundsReallyPlayed;

	// Token: 0x04000143 RID: 323
	[SerializeField]
	private int roundOfDeadline = 3;

	// Token: 0x04000144 RID: 324
	private BigInteger debtIndex = 0;

	// Token: 0x04000145 RID: 325
	private BigInteger debtOutOfRangeMult = 6;

	// Token: 0x04000146 RID: 326
	private static int[] debtsInRange = new int[] { 75, 200, 666, 2222, 12500, 33333, 66666, 200000, 1000000 };

	// Token: 0x04000147 RID: 327
	[SerializeField]
	private byte[] debtIndex_ByteArray;

	// Token: 0x04000148 RID: 328
	[SerializeField]
	private byte[] debtOutOfRangeMult_ByteArray;

	// Token: 0x04000149 RID: 329
	[SerializeField]
	private bool skeletonIsCompleted;

	// Token: 0x0400014A RID: 330
	[SerializeField]
	private bool victoryDeathConditionMet;

	// Token: 0x0400014B RID: 331
	public const int MAX_BUYABLE_SPINS_PER_ROUND = 7;

	// Token: 0x0400014C RID: 332
	[SerializeField]
	private int spinsLeft;

	// Token: 0x0400014D RID: 333
	[SerializeField]
	private int spinsDoneInARun;

	// Token: 0x0400014E RID: 334
	[SerializeField]
	private int extraSpins;

	// Token: 0x0400014F RID: 335
	[SerializeField]
	private int maxSpins = 7;

	// Token: 0x04000150 RID: 336
	[SerializeField]
	private bool lastBetIsSmall;

	// Token: 0x04000151 RID: 337
	[SerializeField]
	private int spinsWithoutReward;

	// Token: 0x04000152 RID: 338
	[SerializeField]
	private long _smallBetsPickedCounter;

	// Token: 0x04000153 RID: 339
	[SerializeField]
	private long _bigBetsPickedCounter;

	// Token: 0x04000154 RID: 340
	[SerializeField]
	private int spinsWithout5PlusPatterns;

	// Token: 0x04000155 RID: 341
	[SerializeField]
	private int consecutiveSpinsWithDiamTreasSevens;

	// Token: 0x04000156 RID: 342
	[SerializeField]
	private long _jackpotsScoredCounter;

	// Token: 0x04000157 RID: 343
	[SerializeField]
	private long _spinsWithAtleast1Jackpot;

	// Token: 0x04000158 RID: 344
	[SerializeField]
	private int _slotInitialLuckRndOffset = -1;

	// Token: 0x04000159 RID: 345
	[SerializeField]
	private int _slotOccasionalLuckSpinN = -1;

	// Token: 0x0400015A RID: 346
	private const float BASE_LUCK_MIN = 0.25f;

	// Token: 0x0400015B RID: 347
	private const float BASE_LUCK_DECREASE_PER_SPIN = 0.001f;

	// Token: 0x0400015C RID: 348
	private const int EXTRA_LUCK_MAX_ENTRIES = 20;

	// Token: 0x0400015D RID: 349
	private const float POWERUP_LUCK_DEFAULT = 1f;

	// Token: 0x0400015E RID: 350
	private const float POWERUP_LUCK_MIN = 0.5f;

	// Token: 0x0400015F RID: 351
	private const float ACTIVATION_LUCK_DEFAULT = 1f;

	// Token: 0x04000160 RID: 352
	private const float ACTIVATION_LUCK_MIN = 0.5f;

	// Token: 0x04000161 RID: 353
	private const float STORE_LUCK_DEFAULT = 1f;

	// Token: 0x04000162 RID: 354
	private const float STORE_LUCK_MIN = 0.5f;

	// Token: 0x04000163 RID: 355
	[SerializeField]
	private GameplayData.ExtraLuckEntry[] extraLuckEntries;

	// Token: 0x04000164 RID: 356
	[SerializeField]
	private float powerupLuck = 1f;

	// Token: 0x04000165 RID: 357
	[SerializeField]
	private float activationLuck = 1f;

	// Token: 0x04000166 RID: 358
	[SerializeField]
	private float storeLuck = 1f;

	// Token: 0x04000167 RID: 359
	private const int SYMBOL_COINS_EXTRA_VALUE_DEFAULT = 0;

	// Token: 0x04000168 RID: 360
	private const int SYMBOL_CHANCE_MIN_VALUE = 0;

	// Token: 0x04000169 RID: 361
	private const int ALL_SYMBOLS_MULTIPLIER_DEFAULT = 1;

	// Token: 0x0400016A RID: 362
	private List<SymbolScript.Kind> symbolsAvailable = new List<SymbolScript.Kind>
	{
		SymbolScript.Kind.lemon,
		SymbolScript.Kind.cherry,
		SymbolScript.Kind.clover,
		SymbolScript.Kind.bell,
		SymbolScript.Kind.diamond,
		SymbolScript.Kind.coins,
		SymbolScript.Kind.seven
	};

	// Token: 0x0400016B RID: 363
	[SerializeField]
	private string[] symbolsAvailable_AsString;

	// Token: 0x0400016C RID: 364
	[SerializeField]
	private GameplayData.SymbolData[] symbolsData;

	// Token: 0x0400016D RID: 365
	private BigInteger allSymbolsMultiplier = 1;

	// Token: 0x0400016E RID: 366
	[SerializeField]
	private byte[] allSymbolsMultiplier_ByteArray;

	// Token: 0x0400016F RID: 367
	private static List<SymbolScript.Kind> _symbolsOrderedByHighestValueToLowest = new List<SymbolScript.Kind>();

	// Token: 0x04000170 RID: 368
	private static List<SymbolScript.Kind> _mostValuableSymbols = new List<SymbolScript.Kind>();

	// Token: 0x04000171 RID: 369
	private static List<SymbolScript.Kind> _leastValuableSymbols = new List<SymbolScript.Kind>();

	// Token: 0x04000172 RID: 370
	private static List<SymbolScript.Kind> _symbolsOrderedByHighestChanceToLowest = new List<SymbolScript.Kind>();

	// Token: 0x04000173 RID: 371
	private static List<SymbolScript.Kind> _mostProbableSymbols = new List<SymbolScript.Kind>();

	// Token: 0x04000174 RID: 372
	private static List<SymbolScript.Kind> _leastProbableSymbols = new List<SymbolScript.Kind>();

	// Token: 0x04000175 RID: 373
	private static float[] modifierChanceFloats = new float[6];

	// Token: 0x04000176 RID: 374
	private static float[] modifierChanceFloats_SuccessThreshold = new float[6];

	// Token: 0x04000177 RID: 375
	private const int ALL_PATTERNS_MULTIPLIER_DEFAULT = 1;

	// Token: 0x04000178 RID: 376
	private const int _666_MINIMUM_DEBT_INDEX = 2;

	// Token: 0x04000179 RID: 377
	private const int _SUPER_666_MINIMUM_DEBT_INDEX = 6;

	// Token: 0x0400017A RID: 378
	private const float _666_CHANCE_DEFAULT = 0.015f;

	// Token: 0x0400017B RID: 379
	private const float _666_CHANCE_DEFAULT_MAX_ABSOLUTE = 0.3f;

	// Token: 0x0400017C RID: 380
	private const float SIX_SIX_SIX_GRADUAL_INCREASE_AMMOUNT = 0.0015f;

	// Token: 0x0400017D RID: 381
	private List<PatternScript.Kind> patternsAvailable = new List<PatternScript.Kind>
	{
		PatternScript.Kind.jackpot,
		PatternScript.Kind.horizontal3,
		PatternScript.Kind.horizontal4,
		PatternScript.Kind.horizontal5,
		PatternScript.Kind.vertical3,
		PatternScript.Kind.diagonal3,
		PatternScript.Kind.triangle,
		PatternScript.Kind.triangleInverted,
		PatternScript.Kind.pyramid,
		PatternScript.Kind.pyramidInverted,
		PatternScript.Kind.eye
	};

	// Token: 0x0400017E RID: 382
	[SerializeField]
	private string[] patternsAvailable_AsString;

	// Token: 0x0400017F RID: 383
	[SerializeField]
	private GameplayData.PatternData[] patternsData;

	// Token: 0x04000180 RID: 384
	private BigInteger allPatternsMultiplier = 1;

	// Token: 0x04000181 RID: 385
	[SerializeField]
	private byte[] allPatternsMultiplier_ByteArray;

	// Token: 0x04000182 RID: 386
	[SerializeField]
	private float _666Chance = 0.015f;

	// Token: 0x04000183 RID: 387
	[SerializeField]
	private float _666ChanceMaxAbsolute = 0.3f;

	// Token: 0x04000184 RID: 388
	[SerializeField]
	private int _666BookedSpin = -1;

	// Token: 0x04000185 RID: 389
	[SerializeField]
	private int _666SuppressedSpinsLeft;

	// Token: 0x04000186 RID: 390
	[SerializeField]
	private bool _lastRoundHadA666;

	// Token: 0x04000187 RID: 391
	private static List<PatternScript.Kind> _patternsOrderedByHighestValueToLowest = new List<PatternScript.Kind>();

	// Token: 0x04000188 RID: 392
	private static List<PatternScript.Kind> _mostValuablePatterns = new List<PatternScript.Kind>();

	// Token: 0x04000189 RID: 393
	private static List<PatternScript.Kind> _leastValuablePatterns = new List<PatternScript.Kind>();

	// Token: 0x0400018A RID: 394
	[SerializeField]
	private GameplayData.PowerupData[] powerupsData;

	// Token: 0x0400018B RID: 395
	[SerializeField]
	private int _rndActivationFailsafe_ConsolationPrize;

	// Token: 0x0400018C RID: 396
	[SerializeField]
	private int _rndActivationFailsafe_BrokenCalculator;

	// Token: 0x0400018D RID: 397
	[SerializeField]
	private int _rndActivationFailsafe_CrankGenerator;

	// Token: 0x0400018E RID: 398
	[SerializeField]
	private int _rndActivationFailsafe_FakeCoin;

	// Token: 0x0400018F RID: 399
	[SerializeField]
	private int _rndActivationFailsafe_RedPepper;

	// Token: 0x04000190 RID: 400
	[SerializeField]
	private int _rndActivationFailsafe_GreenPepper;

	// Token: 0x04000191 RID: 401
	[SerializeField]
	private int _rndActivationFailsafe_GoldenPepper;

	// Token: 0x04000192 RID: 402
	[SerializeField]
	private int _rndActivationFailsafe_RottenPepper;

	// Token: 0x04000193 RID: 403
	[SerializeField]
	private int _rndActivationFailsafe_BellPepper;

	// Token: 0x04000194 RID: 404
	[SerializeField]
	private int _rndActivationFailsafe_Rosary;

	// Token: 0x04000195 RID: 405
	[SerializeField]
	private int _rndActivationFailsafe_Dice4;

	// Token: 0x04000196 RID: 406
	[SerializeField]
	private int _rndActivationFailsafe_SacredHeart;

	// Token: 0x04000197 RID: 407
	[SerializeField]
	private int _powerupHourglass_DeadlinesLeft = 3;

	// Token: 0x04000198 RID: 408
	[SerializeField]
	private int _powerupHourglass_DeadlinesCounter;

	// Token: 0x04000199 RID: 409
	private const int POWERUP_FRUIT_BASKET_DEFAULT_ROUNDS = 10;

	// Token: 0x0400019A RID: 410
	[SerializeField]
	private int _powerupFruitsBasket_RoundsLeft = 10;

	// Token: 0x0400019B RID: 411
	private BigInteger _powerupTarotDeck_Reward = 0;

	// Token: 0x0400019C RID: 412
	[SerializeField]
	private byte[] _powerupTarotDeck_Reward_ByteArray;

	// Token: 0x0400019D RID: 413
	private BigInteger _powerupPoopBeetle_SymbolsIncreaserMult = 0;

	// Token: 0x0400019E RID: 414
	[SerializeField]
	private byte[] _powerupPoopBeetle_SymbolsIncreasetMultByteArray;

	// Token: 0x0400019F RID: 415
	private const int _POWERUP_GRANDMAS_PURSE_INTEREST_DEFAULT = 15;

	// Token: 0x040001A0 RID: 416
	[SerializeField]
	private int _powerupGrandmasPurse_ExtraInterest = 15;

	// Token: 0x040001A1 RID: 417
	[SerializeField]
	private int _powerupOneTrickPony_TargetSpinsLeftIndex = -1;

	// Token: 0x040001A2 RID: 418
	[SerializeField]
	private int _powerupPentacleTriggeredTimes;

	// Token: 0x040001A3 RID: 419
	private BigInteger _powerupCalendar_SymbolsIncreaserMult = 0;

	// Token: 0x040001A4 RID: 420
	[SerializeField]
	private byte[] _powerupCalendar_SymbolsIncreaserMultByteArray;

	// Token: 0x040001A5 RID: 421
	private BigInteger _powerupGigaMushroom_SymbLemonsValue = 0;

	// Token: 0x040001A6 RID: 422
	private BigInteger _powerupGigaMushroom_SymbCherriesValue = 0;

	// Token: 0x040001A7 RID: 423
	private BigInteger _powerupGigaMushroom_SymbCloversValue = 0;

	// Token: 0x040001A8 RID: 424
	private BigInteger _powerupGigaMushroom_SymbBellsValue = 0;

	// Token: 0x040001A9 RID: 425
	private BigInteger _powerupGigaMushroom_SymbDiamondsValue = 0;

	// Token: 0x040001AA RID: 426
	private BigInteger _powerupGigaMushroom_SymbCoinsValue = 0;

	// Token: 0x040001AB RID: 427
	private BigInteger _powerupGigaMushroom_SymbSevensValue = 0;

	// Token: 0x040001AC RID: 428
	[SerializeField]
	private byte[] _powerupGigaMushroom_SymbLemonsValue_ByteArray;

	// Token: 0x040001AD RID: 429
	[SerializeField]
	private byte[] _powerupGigaMushroom_SymbCherriesValue_ByteArray;

	// Token: 0x040001AE RID: 430
	[SerializeField]
	private byte[] _powerupGigaMushroom_SymbCloversValue_ByteArray;

	// Token: 0x040001AF RID: 431
	[SerializeField]
	private byte[] _powerupGigaMushroom_SymbBellsValue_ByteArray;

	// Token: 0x040001B0 RID: 432
	[SerializeField]
	private byte[] _powerupGigaMushroom_SymbDiamondsValue_ByteArray;

	// Token: 0x040001B1 RID: 433
	[SerializeField]
	private byte[] _powerupGigaMushroom_SymbCoinsValue_ByteArray;

	// Token: 0x040001B2 RID: 434
	[SerializeField]
	private byte[] _powerupGigaMushroom_SymbSevensValue_ByteArray;

	// Token: 0x040001B3 RID: 435
	[SerializeField]
	private int _powerupGoldenHorseShoe_SpinsLeft;

	// Token: 0x040001B4 RID: 436
	[SerializeField]
	private int _powerupAncientCoin_SpinsLeft;

	// Token: 0x040001B5 RID: 437
	[SerializeField]
	private int _powerupChannelerOfFortunes_ActivationsCounter;

	// Token: 0x040001B6 RID: 438
	[SerializeField]
	private double[] _powerupPareidolia_PatternBonuses = new double[16];

	// Token: 0x040001B7 RID: 439
	[SerializeField]
	private long _powerupRingBell_BonusCounter;

	// Token: 0x040001B8 RID: 440
	[SerializeField]
	private long _powerupConsolationPrize_BonusCounter;

	// Token: 0x040001B9 RID: 441
	[SerializeField]
	private int _powerupStepsCounter_TriggersCounter;

	// Token: 0x040001BA RID: 442
	[SerializeField]
	private int _powerupDieselLocomotiveBonus;

	// Token: 0x040001BB RID: 443
	[SerializeField]
	private int _powerupSteamLocomotiveBonus;

	// Token: 0x040001BC RID: 444
	[SerializeField]
	private int _powerupDiscA_SpinsCounter;

	// Token: 0x040001BD RID: 445
	[SerializeField]
	private int _powerupDiscB_SpinsCounter;

	// Token: 0x040001BE RID: 446
	[SerializeField]
	private int _powerupDiscC_SpinsCounter;

	// Token: 0x040001BF RID: 447
	[SerializeField]
	private int _powerupWeirdClock_DeadlineUses;

	// Token: 0x040001C0 RID: 448
	[SerializeField]
	private int _powerupCigarettesActivationsCounter;

	// Token: 0x040001C1 RID: 449
	[SerializeField]
	private long _powerupCigarettesPriceIncrease;

	// Token: 0x040001C2 RID: 450
	[SerializeField]
	private int jimboRoundsLeft = -1;

	// Token: 0x040001C3 RID: 451
	private List<GameplayData.JimboAbility> jimboAbilities_BadPool = new List<GameplayData.JimboAbility>
	{
		GameplayData.JimboAbility.Bad_666Chance1_5,
		GameplayData.JimboAbility.Bad_666Chance3,
		GameplayData.JimboAbility.Bad_2LessSpin,
		GameplayData.JimboAbility.Bad_3LessSpins,
		GameplayData.JimboAbility.Bad_Discard6,
		GameplayData.JimboAbility.Bad_Discard3
	};

	// Token: 0x040001C4 RID: 452
	private List<GameplayData.JimboAbility> jimboAbilities_GoodPool = new List<GameplayData.JimboAbility>
	{
		GameplayData.JimboAbility.Good_Repetitions,
		GameplayData.JimboAbility.Good_SymbMult,
		GameplayData.JimboAbility.Good_PatternsMult,
		GameplayData.JimboAbility.Good_Interest,
		GameplayData.JimboAbility.Good_Tickets,
		GameplayData.JimboAbility.Good_Luck,
		GameplayData.JimboAbility.Good_MoreSpins,
		GameplayData.JimboAbility.Good_LemonCherryManifest,
		GameplayData.JimboAbility.Good_CloverBellManifest,
		GameplayData.JimboAbility.Good_DiamondChestsManifest,
		GameplayData.JimboAbility.Good_SevenManifest,
		GameplayData.JimboAbility.Good_FreeRestocks
	};

	// Token: 0x040001C5 RID: 453
	private List<GameplayData.JimboAbility> jimboAbilities_Selected = new List<GameplayData.JimboAbility>(3);

	// Token: 0x040001C6 RID: 454
	[SerializeField]
	private string jimboAbilities_Selected_Str = "";

	// Token: 0x040001C7 RID: 455
	private static StringBuilder jimboSB = new StringBuilder();

	// Token: 0x040001C8 RID: 456
	private static Dictionary<GameplayData.JimboAbility, string> jimboAbilityKeys = new Dictionary<GameplayData.JimboAbility, string>
	{
		{
			GameplayData.JimboAbility.Bad_666Chance1_5,
			"POWERUP_JIMBO_EFFECT_1"
		},
		{
			GameplayData.JimboAbility.Bad_666Chance3,
			"POWERUP_JIMBO_EFFECT_2"
		},
		{
			GameplayData.JimboAbility.Bad_2LessSpin,
			"POWERUP_JIMBO_EFFECT_3"
		},
		{
			GameplayData.JimboAbility.Bad_3LessSpins,
			"POWERUP_JIMBO_EFFECT_4"
		},
		{
			GameplayData.JimboAbility.Bad_Discard6,
			"POWERUP_JIMBO_EFFECT_5"
		},
		{
			GameplayData.JimboAbility.Bad_Discard3,
			"POWERUP_JIMBO_EFFECT_6"
		},
		{
			GameplayData.JimboAbility.Good_Repetitions,
			"POWERUP_JIMBO_EFFECT_7"
		},
		{
			GameplayData.JimboAbility.Good_SymbMult,
			"POWERUP_JIMBO_EFFECT_8"
		},
		{
			GameplayData.JimboAbility.Good_PatternsMult,
			"POWERUP_JIMBO_EFFECT_9"
		},
		{
			GameplayData.JimboAbility.Good_Interest,
			"POWERUP_JIMBO_EFFECT_10"
		},
		{
			GameplayData.JimboAbility.Good_Tickets,
			"POWERUP_JIMBO_EFFECT_11"
		},
		{
			GameplayData.JimboAbility.Good_Luck,
			"POWERUP_JIMBO_EFFECT_12"
		},
		{
			GameplayData.JimboAbility.Good_MoreSpins,
			"POWERUP_JIMBO_EFFECT_13"
		},
		{
			GameplayData.JimboAbility.Good_LemonCherryManifest,
			"POWERUP_JIMBO_EFFECT_14"
		},
		{
			GameplayData.JimboAbility.Good_CloverBellManifest,
			"POWERUP_JIMBO_EFFECT_15"
		},
		{
			GameplayData.JimboAbility.Good_DiamondChestsManifest,
			"POWERUP_JIMBO_EFFECT_16"
		},
		{
			GameplayData.JimboAbility.Good_SevenManifest,
			"POWERUP_JIMBO_EFFECT_17"
		},
		{
			GameplayData.JimboAbility.Good_FreeRestocks,
			"POWERUP_JIMBO_EFFECT_18"
		}
	};

	// Token: 0x040001C9 RID: 457
	[SerializeField]
	private int _powerupRedPepper_ActivationsCounter;

	// Token: 0x040001CA RID: 458
	[SerializeField]
	private int _powerupGreenPepper_ActivationsCounter;

	// Token: 0x040001CB RID: 459
	[SerializeField]
	private int _powerupGoldenPepper_LuckBonus;

	// Token: 0x040001CC RID: 460
	[SerializeField]
	private int _powerupRottenPepper_LuckBonus;

	// Token: 0x040001CD RID: 461
	[SerializeField]
	private int _powerupBellPepper_LuckBonus;

	// Token: 0x040001CE RID: 462
	[SerializeField]
	private long _powerupDevilHorn_AdditionalMultiplier;

	// Token: 0x040001CF RID: 463
	[SerializeField]
	private int _powerupBaphomet_SymbolsBonus = 1;

	// Token: 0x040001D0 RID: 464
	[SerializeField]
	private int _powerupBaphomet_PatternsBonus = 1;

	// Token: 0x040001D1 RID: 465
	[SerializeField]
	private long _powerupCross_Triggers;

	// Token: 0x040001D2 RID: 466
	[SerializeField]
	private int _powerupPossessedPhone_SpinsCount;

	// Token: 0x040001D3 RID: 467
	[SerializeField]
	private float _powerupGoldenKingMida_ExtraBonus;

	// Token: 0x040001D4 RID: 468
	[SerializeField]
	private float _powerupDealer_ExtraBonus;

	// Token: 0x040001D5 RID: 469
	[SerializeField]
	private float _powerupCapitalist_ExtraBonus;

	// Token: 0x040001D6 RID: 470
	private const float POWERUP_PERSONAL_TRAINER_BONUS_DEFAULT = 0.25f;

	// Token: 0x040001D7 RID: 471
	[SerializeField]
	private float _powerupPersonalTrainer_Bonus = 0.25f;

	// Token: 0x040001D8 RID: 472
	private const float POWERUP_ELECTRICIAN_BONUS_DEFAULT = 0.05f;

	// Token: 0x040001D9 RID: 473
	[SerializeField]
	private float _powerupElectrician_Bonus = 0.05f;

	// Token: 0x040001DA RID: 474
	private const float POWERUP_FORTUNE_TELLER_BONUS_DEFAULT = 0.25f;

	// Token: 0x040001DB RID: 475
	[SerializeField]
	private float _powerupFortuneTeller_Bonus = 0.25f;

	// Token: 0x040001DC RID: 476
	[SerializeField]
	private long _powerupAceOfClubs_TicketsSpent;

	// Token: 0x040001DD RID: 477
	[SerializeField]
	private long _powerupAceOfSpades_ActivationsCounter;

	// Token: 0x040001DE RID: 478
	[SerializeField]
	private string _powerupHoleCircle_CharmStr;

	// Token: 0x040001DF RID: 479
	private PowerupScript.Identifier _powerupHoleCircle_CharmIdentifier = PowerupScript.Identifier.undefined;

	// Token: 0x040001E0 RID: 480
	[SerializeField]
	private string _powerupHoleRomboid_CharmStr;

	// Token: 0x040001E1 RID: 481
	private PowerupScript.Identifier _powerupHoleRomboid_CharmIdentifier = PowerupScript.Identifier.undefined;

	// Token: 0x040001E2 RID: 482
	[SerializeField]
	private string _powerupHoleCross_AbilityStr;

	// Token: 0x040001E3 RID: 483
	private AbilityScript.Identifier _powerupHoleCross_AbilityIdentifier = AbilityScript.Identifier.undefined;

	// Token: 0x040001E4 RID: 484
	[SerializeField]
	private int _powerupOphanimWheels_JackpotsCounter;

	// Token: 0x040001E5 RID: 485
	public const int POWERUPS_MAX_EQUIPPABLE_DEFAULT = 7;

	// Token: 0x040001E6 RID: 486
	private const float POWERUP_GRANTED_COINS_MULTIPLIER = 1f;

	// Token: 0x040001E7 RID: 487
	[SerializeField]
	private int maxEquippablePowerups = 7;

	// Token: 0x040001E8 RID: 488
	[SerializeField]
	private float powerupCoinsMultiplier = 1f;

	// Token: 0x040001E9 RID: 489
	[SerializeField]
	private int _redButtonActivationsMultiplier = 1;

	// Token: 0x040001EA RID: 490
	[SerializeField]
	private int _abilityHoly_PatternsRepetitions;

	// Token: 0x040001EB RID: 491
	[NonSerialized]
	public List<AbilityScript.Identifier> phoneAbilitiesPickHistory = new List<AbilityScript.Identifier>();

	// Token: 0x040001EC RID: 492
	[SerializeField]
	private string phoneAbilitiesPickHistory_AsString = "";

	// Token: 0x040001ED RID: 493
	[SerializeField]
	public int _phone_PickupWithAbilities_OverallCounter;

	// Token: 0x040001EE RID: 494
	[SerializeField]
	public bool _phone_bookSpecialCall;

	// Token: 0x040001EF RID: 495
	[SerializeField]
	public int _phone_SpecialCalls_Counter;

	// Token: 0x040001F0 RID: 496
	[SerializeField]
	public int _phone_EvilCallsPicked_Counter;

	// Token: 0x040001F1 RID: 497
	[SerializeField]
	public int _phone_EvilCallsIgnored_Counter;

	// Token: 0x040001F2 RID: 498
	[SerializeField]
	public bool _phoneAlreadyTransformed;

	// Token: 0x040001F3 RID: 499
	[SerializeField]
	public bool _phone_pickedUpOnceLastDeadline;

	// Token: 0x040001F4 RID: 500
	[SerializeField]
	public bool _phone_abilityAlreadyPickedUp = true;

	// Token: 0x040001F5 RID: 501
	[SerializeField]
	public AbilityScript.Category _phone_lastAbilityCategory = AbilityScript.Category.undefined;

	// Token: 0x040001F6 RID: 502
	[SerializeField]
	private string _phone_AbilitiesToPick_String = "";

	// Token: 0x040001F7 RID: 503
	[NonSerialized]
	public List<AbilityScript.Identifier> _phone_AbilitiesToPick = new List<AbilityScript.Identifier>();

	// Token: 0x040001F8 RID: 504
	private const int PHONE_ABILITIES_NUM_DEFAULT = 3;

	// Token: 0x040001F9 RID: 505
	public const int PHONE_ABILITIES_NUM_MAX = 4;

	// Token: 0x040001FA RID: 506
	[SerializeField]
	private int _phoneAbilitiesNumber = 3;

	// Token: 0x040001FB RID: 507
	private const long PHONE_REROLL_COST_DEFAULT = 1L;

	// Token: 0x040001FC RID: 508
	[SerializeField]
	private long _phoneRerollCostIncrease = 1L;

	// Token: 0x040001FD RID: 509
	[SerializeField]
	private long _phoneRerollCost = 1L;

	// Token: 0x040001FE RID: 510
	private const int PHONE_PICK_MULTIPLIER_DEFAULT = 1;

	// Token: 0x040001FF RID: 511
	[SerializeField]
	private int _phonePickMultiplier = 1;

	// Token: 0x04000200 RID: 512
	[SerializeField]
	private byte[] nineNineNine_TotalRewardEarned_ByteArray;

	// Token: 0x04000201 RID: 513
	private BigInteger nineNineNine_TotalRewardEarned = 0;

	// Token: 0x04000202 RID: 514
	private int _phoneAbilChahce_CounterPrevAbilitiesCount = -1;

	// Token: 0x04000203 RID: 515
	private int _phoneAbilChache_normalCount;

	// Token: 0x04000204 RID: 516
	private int _phoneAbilChache_evilCount;

	// Token: 0x04000205 RID: 517
	private int _phoneAbilChache_holyCount;

	// Token: 0x04000206 RID: 518
	private int _pTempNormal;

	// Token: 0x04000207 RID: 519
	private int _pTempEvil;

	// Token: 0x04000208 RID: 520
	private int _pTempGood;

	// Token: 0x04000209 RID: 521
	[SerializeField]
	public int phoneEasyCounter_SkippedCalls_Total;

	// Token: 0x0400020A RID: 522
	[SerializeField]
	public int phoneEasyCounter_SkippedCalls_Normal;

	// Token: 0x0400020B RID: 523
	[SerializeField]
	public int phoneEasyCounter_SkippedCalls_Evil;

	// Token: 0x0400020C RID: 524
	[SerializeField]
	public int phoneEasyCounter_SkippedCalls_Good;

	// Token: 0x0400020D RID: 525
	[SerializeField]
	private long _phoneRerollsPerformed;

	// Token: 0x0400020E RID: 526
	[SerializeField]
	private long _phoneRerollsPerformed_PerDeadline;

	// Token: 0x0400020F RID: 527
	private const bool RUN_MODIFIERS_LOG = false;

	// Token: 0x04000210 RID: 528
	[SerializeField]
	private string runModifierPicked_AsString;

	// Token: 0x04000211 RID: 529
	private RunModifierScript.Identifier runModifierPicked;

	// Token: 0x04000212 RID: 530
	[SerializeField]
	private bool _runModifier_AlreadySet;

	// Token: 0x04000213 RID: 531
	[SerializeField]
	private bool runModifier_DealIsAvailable;

	// Token: 0x04000214 RID: 532
	[SerializeField]
	private int runModifier_AcceptedDealsCounter;

	// Token: 0x04000215 RID: 533
	[SerializeField]
	private bool _alreadyBoughtPowerupAtTerminal;

	// Token: 0x04000216 RID: 534
	[SerializeField]
	private bool newGameIntro_Finished;

	// Token: 0x04000217 RID: 535
	private BigInteger rewardBoxDebtIndex = 7;

	// Token: 0x04000218 RID: 536
	[SerializeField]
	private byte[] rewardBoxDebtIndex_ByteArray;

	// Token: 0x04000219 RID: 537
	[SerializeField]
	private bool doorKeyDeadlineDefined;

	// Token: 0x0400021A RID: 538
	[SerializeField]
	private bool keptPlayingPastWinCondition;

	// Token: 0x0400021B RID: 539
	[SerializeField]
	private bool rewardBoxWasOpened;

	// Token: 0x0400021C RID: 540
	[SerializeField]
	private bool rewardBoxHasPrize = true;

	// Token: 0x0400021D RID: 541
	[SerializeField]
	private bool prizeWasUsed;

	// Token: 0x0400021E RID: 542
	[SerializeField]
	private int rewardKind = 7;

	// Token: 0x0400021F RID: 543
	[SerializeField]
	private long stats_ModifiedLemonTriggeredTimes;

	// Token: 0x04000220 RID: 544
	[SerializeField]
	private long stats_ModifiedCherryTriggeredTimes;

	// Token: 0x04000221 RID: 545
	[SerializeField]
	private long stats_ModifiedCloverTriggeredTimes;

	// Token: 0x04000222 RID: 546
	[SerializeField]
	private long stats_ModifiedBellTriggeredTimes;

	// Token: 0x04000223 RID: 547
	[SerializeField]
	private long stats_ModifiedDiamondTriggeredTimes;

	// Token: 0x04000224 RID: 548
	[SerializeField]
	private long stats_ModifiedCoinsTriggeredTimes;

	// Token: 0x04000225 RID: 549
	[SerializeField]
	private long stats_ModifiedSevenTriggeredTimes;

	// Token: 0x04000226 RID: 550
	[SerializeField]
	private long stats_ModifiedSymbolTriggeredTimes;

	// Token: 0x04000227 RID: 551
	[SerializeField]
	private int stats_RedButtonEffectiveActivationsCounter;

	// Token: 0x04000228 RID: 552
	[SerializeField]
	private long stats_RestocksBoughtN;

	// Token: 0x04000229 RID: 553
	[SerializeField]
	private long stats_RestocksPerformed;

	// Token: 0x0400022A RID: 554
	[SerializeField]
	private long stats_PeppersBought;

	// Token: 0x0400022B RID: 555
	[SerializeField]
	private long stats_PlayTime_Seconds;

	// Token: 0x0400022C RID: 556
	[SerializeField]
	private long stats_DeadlinesCompleted;

	// Token: 0x0400022D RID: 557
	private BigInteger stats_CoinsEarned = 13;

	// Token: 0x0400022E RID: 558
	[SerializeField]
	private byte[] stats_CoinsEarned_ByteArray;

	// Token: 0x0400022F RID: 559
	[SerializeField]
	private long stats_TicketsEarned = 2L;

	// Token: 0x04000230 RID: 560
	[SerializeField]
	private long stats_CharmsBought;

	// Token: 0x04000231 RID: 561
	[SerializeField]
	private long sixSixSixSeen;

	// Token: 0x02000015 RID: 21
	[Serializable]
	private class ExtraLuckEntry
	{
		// Token: 0x04000232 RID: 562
		public string tag;

		// Token: 0x04000233 RID: 563
		public float luck;

		// Token: 0x04000234 RID: 564
		public float luckMax;

		// Token: 0x04000235 RID: 565
		public int spinsLeft;

		// Token: 0x04000236 RID: 566
		public int spinsLeftMax;
	}

	// Token: 0x02000016 RID: 22
	[Serializable]
	private class SymbolData
	{
		// Token: 0x06000307 RID: 775
		public bool Initialize(string symbolKindAsString)
		{
			if (string.IsNullOrEmpty(symbolKindAsString))
			{
				Debug.LogError("Trying to initialize a symbol with an empty string. This is not allowed.");
				return false;
			}
			this.symbolKindAsString = symbolKindAsString;
			SymbolScript.Kind kind = PlatformDataMaster.EnumEntryFromString<SymbolScript.Kind>(symbolKindAsString, SymbolScript.Kind.undefined);
			if (kind == SymbolScript.Kind.undefined || kind == SymbolScript.Kind.count)
			{
				Debug.LogWarning("SymbolData.Initialize(): Failed to convert string into enum for Symbol string: '" + symbolKindAsString + "'.");
				return false;
			}
			this.spawnChance = GameplayData.Symbol_Chance_GetBasic(kind);
			return true;
		}

		// Token: 0x04000237 RID: 567
		public string symbolKindAsString = "";

		// Token: 0x04000238 RID: 568
		public BigInteger extraValue = 0;

		// Token: 0x04000239 RID: 569
		public byte[] extraValue_ByteArray;

		// Token: 0x0400023A RID: 570
		public float spawnChance = 1f;

		// Token: 0x0400023B RID: 571
		public float modifierChance01_InstantReward;

		// Token: 0x0400023C RID: 572
		public float modifierChance01_CloverTicket;

		// Token: 0x0400023D RID: 573
		public float modifierChance01_Golden;

		// Token: 0x0400023E RID: 574
		public float modifierChance01_Repetition;

		// Token: 0x0400023F RID: 575
		public float modifierChance01_Battery;

		// Token: 0x04000240 RID: 576
		public float modifierChance01_Chain;
	}

	// Token: 0x02000017 RID: 23
	[Serializable]
	private class PatternData
	{
		// Token: 0x06000309 RID: 777
		public void Initialize(string patternKindAsString)
		{
			if (string.IsNullOrEmpty(patternKindAsString))
			{
				Debug.LogError("Trying to initialize a pattern with an empty or null string. This is not allowed.");
				return;
			}
			this.patternKindAsString = patternKindAsString;
			this.extraValue = 0.0;
		}

		// Token: 0x04000241 RID: 577
		public string patternKindAsString = "";

		// Token: 0x04000242 RID: 578
		public double extraValue;
	}

	// Token: 0x02000018 RID: 24
	[Serializable]
	public class PowerupData
	{
		// Token: 0x0600030B RID: 779
		public PowerupScript.Identifier IdentifierGetInferred()
		{
			if (this._identifier == PowerupScript.Identifier.undefined)
			{
				this._identifier = PlatformDataMaster.EnumEntryFromString<PowerupScript.Identifier>(this.powerupIdentifierAsString, PowerupScript.Identifier.undefined);
			}
			return this._identifier;
		}

		// Token: 0x0600030C RID: 780
		public void Initialize(PowerupScript.Identifier identifier, string powerupIdentifierAsString)
		{
			if (string.IsNullOrEmpty(powerupIdentifierAsString))
			{
				Debug.LogError("Trying to initialize a powerup with a null or empty string. This is not allowed.");
				return;
			}
			this.powerupIdentifierAsString = powerupIdentifierAsString;
			this.boughtTimes = 0;
			this.modifier = PowerupScript.Modifier.none;
			this.buttonChargesMax = GameplayData.Powerup_RedButtonMaxUses_Init(identifier);
		}

		// Token: 0x04000243 RID: 579
		public string powerupIdentifierAsString = "";

		// Token: 0x04000244 RID: 580
		private PowerupScript.Identifier _identifier = PowerupScript.Identifier.undefined;

		// Token: 0x04000245 RID: 581
		public int boughtTimes;

		// Token: 0x04000246 RID: 582
		public PowerupScript.Modifier modifier;

		// Token: 0x04000247 RID: 583
		public int buttonBurnOutCounter;

		// Token: 0x04000248 RID: 584
		public int buttonChargesCounter;

		// Token: 0x04000249 RID: 585
		public int buttonChargesCounter_Absolute;

		// Token: 0x0400024A RID: 586
		public int buttonChargesMax;

		// Token: 0x0400024B RID: 587
		public int resellBonus;

		// Token: 0x0400024C RID: 588
		public Rng charmSpecificRng;
	}

	// Token: 0x02000019 RID: 25
	public enum JimboAbility
	{
		// Token: 0x0400024E RID: 590
		Bad_666Chance1_5,
		// Token: 0x0400024F RID: 591
		Bad_666Chance3,
		// Token: 0x04000250 RID: 592
		Bad_2LessSpin,
		// Token: 0x04000251 RID: 593
		Bad_3LessSpins,
		// Token: 0x04000252 RID: 594
		Bad_Discard6,
		// Token: 0x04000253 RID: 595
		Bad_Discard3,
		// Token: 0x04000254 RID: 596
		Good_Repetitions,
		// Token: 0x04000255 RID: 597
		Good_SymbMult,
		// Token: 0x04000256 RID: 598
		Good_PatternsMult,
		// Token: 0x04000257 RID: 599
		Good_Interest,
		// Token: 0x04000258 RID: 600
		Good_Tickets,
		// Token: 0x04000259 RID: 601
		Good_Luck,
		// Token: 0x0400025A RID: 602
		Good_MoreSpins,
		// Token: 0x0400025B RID: 603
		Good_LemonCherryManifest,
		// Token: 0x0400025C RID: 604
		Good_CloverBellManifest,
		// Token: 0x0400025D RID: 605
		Good_DiamondChestsManifest,
		// Token: 0x0400025E RID: 606
		Good_SevenManifest,
		// Token: 0x0400025F RID: 607
		Good_FreeRestocks,
		// Token: 0x04000260 RID: 608
		Count
	}
}
