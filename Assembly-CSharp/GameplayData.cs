using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Panik;
using UnityEngine;

[Serializable]
public class GameplayData
{
	// (get) Token: 0x060000EF RID: 239
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

	private BigInteger BigIntegerFromByteArray(byte[] byteArray, BigInteger defaultValue)
	{
		if (byteArray == null || byteArray.Length == 0)
		{
			return defaultValue;
		}
		return new BigInteger(byteArray);
	}

	public static int SeedGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.seed;
	}

	public static uint SeedGetAsUInt()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0U;
		}
		return (uint)instance.seed;
	}

	public static string SeedGetAsString()
	{
		return GameplayData.SeedGetAsUInt().ToString("0000000000");
	}

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

	public static BigInteger StoreRestockExtraCostGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._storeRestockExtraCost;
	}

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

	public static void StoreRestockExtraCostAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.StoreRestockExtraCostSet(instance._storeRestockExtraCost + value);
	}

	public static long StoreFreeRestocksGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._storeFreeRestocks;
	}

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

	public static long StoreTemporaryDiscountGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.temporaryDiscount;
	}

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

	public static long StoreTemporaryDiscountPerSlotGet(int slotIndex)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.temporaryDiscountPerSlot[slotIndex];
	}

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

	private void _Coins_PrepareForSerialization()
	{
		this.coins_ByteArray = this.coins.ToByteArray();
		this.depositedCoins_ByteArray = this.depositedCoins.ToByteArray();
		this.interestEarned_ByteArray = this.interestEarned.ToByteArray();
		this.roundEarnedCoins_ByteArray = this.roundEarnedCoins.ToByteArray();
	}

	private void _Coins_RestoreFromSerialization()
	{
		this.coins = this.BigIntegerFromByteArray(this.coins_ByteArray, 13);
		this.depositedCoins = this.BigIntegerFromByteArray(this.depositedCoins_ByteArray, 30);
		this.interestEarned = this.BigIntegerFromByteArray(this.interestEarned_ByteArray, 0);
		this.roundEarnedCoins = this.BigIntegerFromByteArray(this.roundEarnedCoins_ByteArray, 0);
	}

	public static BigInteger CoinsGet()
	{
		if (GameplayData.Instance == null)
		{
			return 13;
		}
		return GameplayData.Instance.coins;
	}

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

	public static BigInteger DepositGet()
	{
		if (GameplayData.Instance == null)
		{
			return 30;
		}
		return GameplayData.Instance.depositedCoins;
	}

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

	public static void DepositAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.DepositSet(instance.depositedCoins + value);
	}

	public static bool HasDepositedSomething()
	{
		return GameplayData.DepositGet() > 30L;
	}

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

	public static BigInteger InterestEarnedGet()
	{
		if (GameplayData.Instance == null)
		{
			return 0;
		}
		return GameplayData.Instance.interestEarned;
	}

	public static BigInteger InterestEarnedHypotetically()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.depositedCoins * (int)GameplayData.InterestRateGet() / 100;
	}

	public static void InterestEarnedGrow()
	{
		GameplayData.InterestEarnedGrow_Manual(GameplayData.InterestEarnedHypotetically());
	}

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

	public static void InterestRateAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.InterestRateSet(instance.interestRate + value);
	}

	public static BigInteger RoundEarnedCoinsGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.roundEarnedCoins;
	}

	public static void RoundEarnedCoinsSet(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.roundEarnedCoins = value;
	}

	public static void RoundEarnedCoinsAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.RoundEarnedCoinsSet(instance.roundEarnedCoins + value);
	}

	public static void RoundEarnedCoinsReset()
	{
		GameplayData.RoundEarnedCoinsSet(0);
	}

	public static long CloverTicketsGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 2L;
		}
		return instance.cloverTickets;
	}

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

	public static void CloverTickets_BonusLittleBet_Add(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.CloverTickets_BonusLittleBet_Set(instance.cloverTickets_BonusFor_LittleBet + value);
	}

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

	public static void CloverTickets_BonusBigBet_Add(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.CloverTickets_BonusBigBet_Set(instance.cloverTickets_BonusFor_BigBet + value);
	}

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

	public static void CloverTickets_BonusRoundsLeft_Add(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.CloverTickets_BonusRoundsLeft_Set(instance.cloverTickets_BonusFor_RoundsLeft + value);
	}

	public static bool ATMDeadline_RewardPickupMemo_MessageShownGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.atmDeadline_RewardPickupMemo_MessageShown;
	}

	public static void ATMDeadline_RewardPickupMemo_MessageShownSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.atmDeadline_RewardPickupMemo_MessageShown = true;
	}

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

	public static BigInteger DeadlineReward_CoinsGet()
	{
		if (GameplayData.Instance == null)
		{
			return 0;
		}
		return GameplayData.DeadlineReward_CoinsGet(GameplayData.DebtIndexGet());
	}

	public static long DeadlineReward_CloverTickets(int extraRounds)
	{
		return GameplayData.CloverTickets_BonusRoundsLeft_Get() * (long)extraRounds * (long)PowerupScript.EvilDealBonusMultiplier();
	}

	public static long DeadlineReward_CloverTickets_Extras(bool rewardTime)
	{
		return ((long)PowerupScript.ModifiedPowerups_GetTicketsBonus() + PowerupScript.CloverPotTicketsBonus(true, rewardTime) + (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_Tickets, true) ? 3L : 0L)) * (long)PowerupScript.EvilDealBonusMultiplier();
	}

	private void _Debt_PrepareForSerialization()
	{
		this.debtIndex_ByteArray = this.debtIndex.ToByteArray();
		this.debtOutOfRangeMult_ByteArray = this.debtOutOfRangeMult.ToByteArray();
	}

	private void _Debt_RestoreFromSerialization()
	{
		this.debtIndex = this.BigIntegerFromByteArray(this.debtIndex_ByteArray, 0);
		this.debtOutOfRangeMult = this.BigIntegerFromByteArray(this.debtOutOfRangeMult_ByteArray, 6);
	}

	public static int RoundDeadlineTrail_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.roundDeadlineTrail;
	}

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

	public static void RoundDeadlineTrail_Increment()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.roundDeadlineTrail++;
	}

	public static int RoundsReallyPlayedGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.roundsReallyPlayed;
	}

	public static void RoundsReallyPlayedIncrement()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.roundsReallyPlayed++;
	}

	public static int RoundDeadlineTrail_AtDeadlineBegin_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.roundDeadlineTrail_AtDeadlineBegin;
	}

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

	public static void RoundDeadlineTrail_AtDeadlineBegin_CheckpointSet()
	{
		GameplayData.RoundDeadlineTrail_AtDeadlineBegin_Set(GameplayData.RoundDeadlineTrail_Get());
	}

	public static int RoundsLeftToDeadline()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance.roundOfDeadline - instance.roundDeadlineTrail;
	}

	public static int RoundsOfDeadline_TotalGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance.roundOfDeadline - instance.roundDeadlineTrail_AtDeadlineBegin;
	}

	public static int RoundsOfDeadline_PlayedGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance.roundDeadlineTrail - instance.roundDeadlineTrail_AtDeadlineBegin;
	}

	public static int RoundOfDeadlineGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance.roundOfDeadline;
	}

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

	public static void DeadlineRoundsIncrement()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.DeadlineRoundsIncrement_Set(instance.roundOfDeadline + GameplayData._GetDebtRoundDeadline_NextIncrement());
	}

	public static void DeadlineRoundsIncrement_Set(int ammount)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.roundOfDeadline = ammount;
	}

	public static bool AreWeOverTheDebtRange(BigInteger debtIndex)
	{
		return GameplayData.Instance == null || GameplayData.debtsInRange == null || GameplayData.debtsInRange.Length == 0 || debtIndex >= (long)GameplayData.debtsInRange.Length;
	}

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

	public static BigInteger DebtGetExt(BigInteger debtIndex)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1000;
		}
		BigInteger bigInteger;
		if (!GameplayData.AreWeOverTheDebtRange(debtIndex))
		{
			if (debtIndex < 0L)
			{
				debtIndex = 0;
			}
			int num = debtIndex.CastToInt();
			bigInteger = GameplayData.debtsInRange[num];
		}
		else
		{
			bigInteger = GameplayData.debtsInRange[GameplayData.debtsInRange.Length - 1];
			bigInteger *= instance.debtOutOfRangeMult;
		}
		bigInteger *= PowerupScript.SkeletonPiecesDebtIncreasePercentage();
		bigInteger /= 100;
		if (GameplayData.RunModifier_GetCurrent() == RunModifierScript.Identifier.bigDebt)
		{
			bigInteger *= 2;
		}
		if (Data.game.RunModifier_HardcoreMode_Get(GameplayData.RunModifier_GetCurrent()))
		{
			bigInteger *= 2;
		}
		return bigInteger;
	}

	public static BigInteger DebtGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return GameplayData.debtsInRange[0];
		}
		return GameplayData.DebtGetExt(instance.debtIndex);
	}

	public static BigInteger DebtIndexGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.debtIndex;
	}

	public static void DebtIndexSet(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.debtIndex = value;
	}

	public static void DebtIndexAdd(BigInteger value)
	{
		GameplayData.DebtIndexSet(GameplayData.DebtIndexGet() + value);
	}

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

	public static BigInteger DebtOutOfRangeMultiplier_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 6;
		}
		return instance.debtOutOfRangeMult;
	}

	public static void DebtOutOfRangeMultiplier_Set(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.debtOutOfRangeMult = value;
	}

	public static bool SkeletonIsCompletedGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.skeletonIsCompleted;
	}

	public static void SkeletonIsCompletedSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.skeletonIsCompleted = true;
	}

	public static int SpinsLeftGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.spinsLeft;
	}

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

	public static void SpinsLeftAdd(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.SpinsLeftSet(instance.spinsLeft + n);
	}

	public static void SpinConsume()
	{
		GameplayData.SpinsLeftAdd(-1);
	}

	public static int SpinsDoneInARun_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.spinsDoneInARun;
	}

	public static void SpinsDoneInARun_Increment()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.spinsDoneInARun++;
	}

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

	public static void ExtraSpinsAdd(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.ExtraSpinsSet(instance.extraSpins + n);
	}

	public static BigInteger SpinCostGet_Single()
	{
		return GameplayData.SpinCostGet_Single(GameplayData.DebtIndexGet());
	}

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

	public static int GetHypotehticalMidSpinsBuyable()
	{
		return Mathf.FloorToInt((float)(GameplayData.GetHypotehticalMaxSpinsBuyable() / 2));
	}

	public static BigInteger SpinCostMax_Get()
	{
		return GameplayData.GetHypotehticalMaxSpinsBuyable() * GameplayData.SpinCostGet_Single();
	}

	public static BigInteger SpinCostMid_Get()
	{
		return GameplayData.GetHypotehticalMidSpinsBuyable() * GameplayData.SpinCostGet_Single();
	}

	public static void LastBet_IsSmallSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.lastBetIsSmall = true;
	}

	public static void LastBet_IsBigSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.lastBetIsSmall = false;
	}

	public static bool LastBet_IsSmallGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance == null || instance.lastBetIsSmall;
	}

	public static bool LastBet_IsBigGet()
	{
		return !GameplayData.LastBet_IsSmallGet();
	}

	public static int MaxSpins_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.maxSpins;
	}

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

	public static void MaxSpins_Add(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.MaxSpins_Set(instance.maxSpins + n);
	}

	public static long SmallBetPickCount()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._smallBetsPickedCounter;
	}

	public static long BigBetPickCount()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._bigBetsPickedCounter;
	}

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

	public static int SpinsWithoutReward_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.spinsWithoutReward;
	}

	public static void SpinsWithoutReward_Increase()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.spinsWithoutReward++;
	}

	public static void SpinsWithoutReward_Reset()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.spinsWithoutReward = 0;
	}

	public static int ConsecutiveSpinsWithout5PlusPatterns_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.spinsWithout5PlusPatterns;
	}

	public static void ConsecutiveSpinsWithout5PlusPatterns_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.spinsWithout5PlusPatterns = n;
	}

	public static int ConsecutiveSpinsWithDiamondTreasureOrSeven_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.consecutiveSpinsWithDiamTreasSevens;
	}

	public static void ConsecutiveSpinsWithDiamondTreasureOrSeven_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.consecutiveSpinsWithDiamTreasSevens = n;
	}

	// (get) Token: 0x0600016E RID: 366
	// (set) Token: 0x0600016F RID: 367
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

	public static long SpinsWithAtLeast1Jackpot_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._spinsWithAtleast1Jackpot;
	}

	public static void SpinsWithAtLeast1Jackpot_Set(long n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._spinsWithAtleast1Jackpot = n;
	}

	// (get) Token: 0x06000172 RID: 370
	// (set) Token: 0x06000173 RID: 371
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

	// (get) Token: 0x06000174 RID: 372
	// (set) Token: 0x06000175 RID: 373
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

	public static float LuckGet()
	{
		if (GameplayData.Instance == null)
		{
			return 0f;
		}
		return 0f + GameplayData.ExtraLuck_GetTotal() + PowerupScript.CrystalLuckIncreaseGet(true);
	}

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

	public static float PowerupLuckGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1f;
		}
		return instance.powerupLuck;
	}

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

	public static void PowerupLuckAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.PowerupLuckSet(instance.powerupLuck + value);
	}

	public static void PowerupLuckReset()
	{
		GameplayData.PowerupLuckSet(1f);
	}

	public static float ActivationLuckGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1f;
		}
		return instance.activationLuck + PowerupScript.HorseShoesLuckGet() + PowerupScript.GoldenHorseShoe_RandomActivationChanceBonusGet(true);
	}

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

	public static void ActivationLuckAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.ActivationLuckSet(instance.activationLuck + value);
	}

	public static void ActivationLuckReset()
	{
		GameplayData.ActivationLuckSet(1f);
	}

	public static float StoreLuckGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1f;
		}
		return instance.storeLuck;
	}

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

	public static void StoreLuckAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.StoreLuckSet(instance.storeLuck + value);
	}

	public static void StoreLuckReset()
	{
		GameplayData.StoreLuckSet(1f);
	}

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

	public static void SymbolsAvilable_Remove(SymbolScript.Kind kind)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.symbolsAvailable.Remove(kind);
	}

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

	public static BigInteger Symbol_CoinsValueExtra_Get(SymbolScript.Kind kind)
	{
		GameplayData.SymbolData symbolData = GameplayData._SymbolDataFind(kind);
		if (symbolData == null)
		{
			return 0;
		}
		return symbolData.extraValue;
	}

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

	public static void Symbol_CoinsValueExtra_Add(SymbolScript.Kind kind, BigInteger value)
	{
		GameplayData.Symbol_CoinsValueExtra_Set(kind, GameplayData.Symbol_CoinsValueExtra_Get(kind) + value);
	}

	public static void Symbol_CoinsValueExtra_Reset(SymbolScript.Kind kind)
	{
		GameplayData.Symbol_CoinsValueExtra_Set(kind, 0);
	}

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

	public static void Symbol_Chance_Add(SymbolScript.Kind kind, float value)
	{
		GameplayData.SymbolData symbolData = GameplayData._SymbolDataFind(kind);
		if (symbolData == null)
		{
			return;
		}
		GameplayData.Symbol_Chance_Set(kind, symbolData.spawnChance + value);
	}

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

	public static List<SymbolScript.Kind> SymbolsValueList_Get()
	{
		return GameplayData._symbolsOrderedByHighestValueToLowest;
	}

	public static List<SymbolScript.Kind> MostValuableSymbols_GetList()
	{
		return GameplayData._mostValuableSymbols;
	}

	public static List<SymbolScript.Kind> LeastValuableSymbols_GetList()
	{
		return GameplayData._leastValuableSymbols;
	}

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

	public static List<SymbolScript.Kind> SymbolsChanceList_Get()
	{
		return GameplayData._symbolsOrderedByHighestChanceToLowest;
	}

	public static List<SymbolScript.Kind> MostProbableSymbols_GetList()
	{
		return GameplayData._mostProbableSymbols;
	}

	public static List<SymbolScript.Kind> LeastProbableSymbols_GetList()
	{
		return GameplayData._leastProbableSymbols;
	}

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

	public static float Symbol_ModifierChance_GetAsPercentage(SymbolScript.Kind symbolKind, SymbolScript.Modifier modifier)
	{
		return GameplayData.Symbol_ModifierChance_Get(symbolKind, modifier) * 100f;
	}

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

	public static void AllSymbolsMultiplierAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.AllSymbolsMultiplierSet(instance.allSymbolsMultiplier + value);
	}

	public static void allSymbolsMultiplierReset()
	{
		GameplayData.AllSymbolsMultiplierSet(1);
	}

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

	public static List<PatternScript.Kind> PatternsAvailable_GetAll()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return new List<PatternScript.Kind>();
		}
		return instance.patternsAvailable;
	}

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

	public static void PatternsAvailable_Remove(PatternScript.Kind kind)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.patternsAvailable.Remove(kind);
	}

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

	public static double Pattern_ValueExtra_Get(PatternScript.Kind kind)
	{
		GameplayData.PatternData patternData = GameplayData._PatternDataFind(kind);
		if (patternData == null)
		{
			return 0.0;
		}
		return patternData.extraValue;
	}

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

	public static void Pattern_ValueExtra_Add(PatternScript.Kind kind, double value)
	{
		GameplayData.PatternData patternData = GameplayData._PatternDataFind(kind);
		if (patternData == null)
		{
			return;
		}
		GameplayData.Pattern_ValueExtra_Set(kind, patternData.extraValue + value);
	}

	public static void Pattern_ValueExtra_Reset(PatternScript.Kind kind)
	{
		GameplayData.Pattern_ValueExtra_Set(kind, 0.0);
	}

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

	public static List<PatternScript.Kind> PatternsValueList_Get()
	{
		return GameplayData._patternsOrderedByHighestValueToLowest;
	}

	public static List<PatternScript.Kind> MostValuablePatterns_GetList()
	{
		return GameplayData._mostValuablePatterns;
	}

	public static List<PatternScript.Kind> LeastValuablePatterns_GetList()
	{
		return GameplayData._leastValuablePatterns;
	}

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

	public static void AllPatternsMultiplierAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.AllPatternsMultiplierSet(instance.allPatternsMultiplier + value);
	}

	public static void AllPatternsMultiplierReset()
	{
		GameplayData.AllPatternsMultiplierSet(1);
	}

	public static BigInteger SixSixSix_GetMinimumDebtIndex()
	{
		return 2;
	}

	public static BigInteger SuperSixSixSix_GetMinimumDebtIndex()
	{
		if (RewardBoxScript.GetRewardKind() != RewardBoxScript.RewardKind.DoorKey)
		{
			return 666;
		}
		return 6;
	}

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

	public static float SixSixSix_ChanceGet_AsPercentage(bool considerMaximum)
	{
		return GameplayData.SixSixSix_ChanceGet(considerMaximum) * 100f;
	}

	public static void OBSOLETE_SixSixSix_IncrementChance()
	{
		GameplayData.OBSOLETE_SixSixSix_IncrementChanceManual(0.0015f);
	}

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

	public static void SixSixSix_SuppressedSpinsSet(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._666SuppressedSpinsLeft = Mathf.Max(0, n);
	}

	public static int SixSixSix_SuppressedSpinsGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._666SuppressedSpinsLeft;
	}

	// (get) Token: 0x060001D0 RID: 464
	// (set) Token: 0x060001D1 RID: 465
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

	private void _PowerupsData_PrepareForSerialization()
	{
		GameplayData._EnsurePowerupDataArray(this);
	}

	private void _PowerupsData_RestoreFromSerialization()
	{
		GameplayData._EnsurePowerupDataArray(this);
	}

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

	public Rng PowerupRngGet(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return null;
		}
		return powerupData.charmSpecificRng;
	}

	public static int Powerup_BoughtTimes_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.boughtTimes;
	}

	public static void Powerup_BoughtTimes_Increase(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return;
		}
		powerupData.boughtTimes++;
	}

	public static PowerupScript.Modifier Powerup_Modifier_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return PowerupScript.Modifier.none;
		}
		return powerupData.modifier;
	}

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

	public static int Powerup_ButtonBurnedOut_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.buttonBurnOutCounter;
	}

	public static void Powerup_ButtonBurnedOut_Set(PowerupScript.Identifier identifier, int n)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return;
		}
		powerupData.buttonBurnOutCounter = n;
	}

	public static void Powerup_ButtonBurnedOut_Increaase(PowerupScript.Identifier identifier)
	{
		GameplayData.Powerup_ButtonBurnedOut_Set(identifier, GameplayData.Powerup_ButtonBurnedOut_Get(identifier) + 1);
	}

	public static int Powerup_ButtonChargesUsed_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.buttonChargesCounter;
	}

	public static int Powerup_ButtonChargesUsed_GetAbsolute(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.buttonChargesCounter_Absolute;
	}

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

	public static bool Powerup_ButtonChargesUsed_RestoreChargesN(PowerupScript.Identifier identifier, int value, bool triggerRechargeAnimation)
	{
		return GameplayData.Powerup_ButtonChargesUsed_RestoreChargesN_Ext(identifier, value, triggerRechargeAnimation, true);
	}

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

	public static int Powerup_ResellBonus_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.resellBonus;
	}

	public static void Powerup_ResellBonus_Set(PowerupScript.Identifier identifier, int n)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return;
		}
		powerupData.resellBonus = Mathf.Max(0, n);
	}

	// (get) Token: 0x060001EC RID: 492
	// (set) Token: 0x060001ED RID: 493
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

	// (get) Token: 0x060001EE RID: 494
	// (set) Token: 0x060001EF RID: 495
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

	// (get) Token: 0x060001F0 RID: 496
	// (set) Token: 0x060001F1 RID: 497
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

	// (get) Token: 0x060001F2 RID: 498
	// (set) Token: 0x060001F3 RID: 499
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

	// (get) Token: 0x060001F4 RID: 500
	// (set) Token: 0x060001F5 RID: 501
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

	// (get) Token: 0x060001F6 RID: 502
	// (set) Token: 0x060001F7 RID: 503
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

	// (get) Token: 0x060001F8 RID: 504
	// (set) Token: 0x060001F9 RID: 505
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

	// (get) Token: 0x060001FA RID: 506
	// (set) Token: 0x060001FB RID: 507
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

	// (get) Token: 0x060001FC RID: 508
	// (set) Token: 0x060001FD RID: 509
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

	// (get) Token: 0x060001FE RID: 510
	// (set) Token: 0x060001FF RID: 511
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

	// (get) Token: 0x06000200 RID: 512
	// (set) Token: 0x06000201 RID: 513
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

	// (get) Token: 0x06000202 RID: 514
	// (set) Token: 0x06000203 RID: 515
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

	// (get) Token: 0x06000206 RID: 518
	// (set) Token: 0x06000207 RID: 519
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

	public static int Powerup_FruitsBasket_RoundsLeftGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupFruitsBasket_RoundsLeft;
	}

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

	public static void Powerup_FruitBasket_RoundsLeftReset()
	{
		GameplayData.Powerup_FruitsBasket_RoundsLeftSet(10);
	}

	public static BigInteger Powerup_TarotDeck_RewardGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupTarotDeck_Reward;
	}

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

	public static void Powerup_TarotDeck_RewardAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_TarotDeck_RewardSet(instance._powerupTarotDeck_Reward + value);
	}

	public static BigInteger Powerup_PoopBeetle_SymbolsIncreaseN_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupPoopBeetle_SymbolsIncreaserMult;
	}

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

	public static void Powerup_PoopBeetle_SymbolsIncreaseN_Add(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_PoopBeetle_SymbolsIncreaseN_Set(instance._powerupPoopBeetle_SymbolsIncreaserMult + value);
	}

	public static int Powerup_GrandmasPurse_ExtraInterestGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupGrandmasPurse_ExtraInterest;
	}

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

	public static void Powerup_GrandmasPurse_ExtraInterestAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_GrandmasPurse_ExtraInterestSet(instance._powerupGrandmasPurse_ExtraInterest + value);
	}

	public static void Powerup_GrandmasPurse_ExtraInterestReset()
	{
		GameplayData.Powerup_GrandmasPurse_ExtraInterestSet(15);
	}

	public static int Powerup_OneTrickPony_TargetSpinIndexGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return -1;
		}
		return instance._powerupOneTrickPony_TargetSpinsLeftIndex;
	}

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

	public static int Powerup_Pentacle_TriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupPentacleTriggeredTimes;
	}

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

	public static void Powerup_Pentacle_TriggeredTimesAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_Pentacle_TriggeredTimesSet(instance._powerupPentacleTriggeredTimes + value);
	}

	public static BigInteger Powerup_Calendar_SymbolsIncreaseN_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupCalendar_SymbolsIncreaserMult;
	}

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

	public static int Powerup_GoldenHorseShoe_SpinsLeftGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupGoldenHorseShoe_SpinsLeft;
	}

	public static void Powerup_GoldenHorseShoe_SpinsLeftSet(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupGoldenHorseShoe_SpinsLeft = Mathf.Max(0, n);
	}

	public static int Powerup_AncientCoin_SpinsLeftGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupAncientCoin_SpinsLeft;
	}

	public static void Powerup_AncientCoin_SpinsLeftSet(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupAncientCoin_SpinsLeft = Mathf.Max(0, n);
	}

	public static int Powerup_ChannelerOfFortune_ActivationsCounterGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupChannelerOfFortunes_ActivationsCounter;
	}

	public static void Powerup_ChannelerOfFortune_ActivationsCounterSet(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupChannelerOfFortunes_ActivationsCounter = n;
	}

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

	private void Powerup_Pareidolia_SerializationPrepare()
	{
		this.PowerupPareidoliaArrayEnsure();
	}

	private void Powerup_Pareidolia_SerializationRestore()
	{
		this.PowerupPareidoliaArrayEnsure();
	}

	public static double Powerup_PareidoliaMultiplierBonus_Get(PatternScript.Kind kind)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0.0;
		}
		return instance._powerupPareidolia_PatternBonuses[(int)kind];
	}

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

	public static long Powerup_RingBell_Bonus_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupRingBell_BonusCounter;
	}

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

	public static long Powerup_ConsolationPrize_Bonus_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupConsolationPrize_BonusCounter;
	}

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

	public static int Powerup_StepsCounter_TriggersCounter_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupStepsCounter_TriggersCounter;
	}

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

	public static int Powerup_DieselLocomotive_Bonus_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupDieselLocomotiveBonus;
	}

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

	public static int Powerup_SteamLocomotive_Bonus_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupSteamLocomotiveBonus;
	}

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

	// (get) Token: 0x06000235 RID: 565
	// (set) Token: 0x06000236 RID: 566
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

	// (get) Token: 0x06000237 RID: 567
	// (set) Token: 0x06000238 RID: 568
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

	// (get) Token: 0x06000239 RID: 569
	// (set) Token: 0x0600023A RID: 570
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

	// (get) Token: 0x0600023B RID: 571
	// (set) Token: 0x0600023C RID: 572
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

	// (get) Token: 0x0600023D RID: 573
	// (set) Token: 0x0600023E RID: 574
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

	// (get) Token: 0x0600023F RID: 575
	// (set) Token: 0x06000240 RID: 576
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

	public static List<GameplayData.JimboAbility> Powerup_Jimbo_AbilitiesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return null;
		}
		return instance.jimboAbilities_Selected;
	}

	public static bool Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility ability, bool considerEquippedState)
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && (!considerEquippedState || PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Jimbo)) && instance.jimboAbilities_Selected.Contains(ability);
	}

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

	public static int Powerup_RedPepper_ActivationsCounterGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupRedPepper_ActivationsCounter;
	}

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

	public static void Powerup_RedPepper_ActivationsCounterAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_RedPepper_ActivationsCounterSet(instance._powerupRedPepper_ActivationsCounter + value);
	}

	public static int Powerup_GreenPepper_ActivationsCounterGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupGreenPepper_ActivationsCounter;
	}

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

	public static void Powerup_GreenPepper_ActivationsCounterAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_GreenPepper_ActivationsCounterSet(instance._powerupGreenPepper_ActivationsCounter + value);
	}

	public static int Powerup_GoldenPepper_LuckBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupGoldenPepper_LuckBonus;
	}

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

	public static int Powerup_RottenPepper_LuckBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupRottenPepper_LuckBonus;
	}

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

	public static int Powerup_BellPepper_LuckBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupBellPepper_LuckBonus;
	}

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

	public static long Powerup_DevilHorn_AdditionalMultiplierGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupDevilHorn_AdditionalMultiplier;
	}

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

	public static void Powerup_DevilHorn_AdditionalMultiplierAdd(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_DevilHorn_AdditionalMultiplierSet(instance._powerupDevilHorn_AdditionalMultiplier + value);
	}

	public static int Powerup_Baphomet_ActivationsCounterGet_Above()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupBaphomet_SymbolsBonus;
	}

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

	public static int Powerup_Baphomet_ActivationsCounterGet_Below()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupBaphomet_PatternsBonus;
	}

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

	public static long Powerup_Cross_TriggersCount_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupCross_Triggers;
	}

	public static void Powerup_Cross_TriggersCount_Set(long i)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupCross_Triggers = i;
	}

	public static int Powerup_PossessedPhone_TriggersCount_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupPossessedPhone_SpinsCount;
	}

	public static void Powerup_PossessedPhone_TriggersCount_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupPossessedPhone_SpinsCount = Mathf.Max(0, n);
	}

	public static float Powerup_GoldenKingMida_ExtraBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupGoldenKingMida_ExtraBonus;
	}

	public static void Powerup_GoldenKingMida_ExtraBonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupGoldenKingMida_ExtraBonus = value;
	}

	public static float Powerup_Dealer_ExtraBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupDealer_ExtraBonus;
	}

	public static void Powerup_Dealer_ExtraBonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupDealer_ExtraBonus = value;
	}

	public static float Powerup_Capitalist_ExtraBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupCapitalist_ExtraBonus;
	}

	public static void Powerup_Capitalist_ExtraBonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupCapitalist_ExtraBonus = value;
	}

	public static float Powerup_PersonalTrainer_BonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupPersonalTrainer_Bonus;
	}

	public static void Powerup_PersonalTrainer_BonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupPersonalTrainer_Bonus = value;
	}

	public static void Powerup_PersonalTrainer_BonusReset()
	{
		GameplayData.Powerup_PersonalTrainer_BonusSet(0.25f);
	}

	public static float Powerup_Electrician_BonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupElectrician_Bonus;
	}

	public static void Powerup_Electrician_BonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupElectrician_Bonus = value;
	}

	public static void Powerup_Electrician_BonusReset()
	{
		GameplayData.Powerup_Electrician_BonusSet(0.05f);
	}

	public static float Powerup_FortuneTeller_BonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupFortuneTeller_Bonus;
	}

	public static void Powerup_FortuneTeller_BonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupFortuneTeller_Bonus = value;
	}

	public static void Powerup_FortuneTeller_BonusReset()
	{
		GameplayData.Powerup_FortuneTeller_BonusSet(0.25f);
	}

	public static long Powerup_AceOfClubs_TicketsSpentGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupAceOfClubs_TicketsSpent;
	}

	public static void Powerup_AceOfClubs_TicketsSpentSet(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupAceOfClubs_TicketsSpent = value;
	}

	public static long Powerup_AceOfSpades_ActivationsCounterGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupAceOfSpades_ActivationsCounter;
	}

	public static void Powerup_AceOfSpades_ActivationsCounterSet(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupAceOfSpades_ActivationsCounter = value;
	}

	public static PowerupScript.Identifier PowerupHoleCircle_CharmGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return PowerupScript.Identifier.undefined;
		}
		return instance._powerupHoleCircle_CharmIdentifier;
	}

	public static void PowerupHoleCircle_CharmSet(PowerupScript.Identifier identifier)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupHoleCircle_CharmIdentifier = identifier;
	}

	public static PowerupScript.Identifier PowerupHoleRomboid_CharmGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return PowerupScript.Identifier.undefined;
		}
		return instance._powerupHoleRomboid_CharmIdentifier;
	}

	public static void PowerupHoleRomboid_CharmSet(PowerupScript.Identifier identifier)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupHoleRomboid_CharmIdentifier = identifier;
	}

	public static AbilityScript.Identifier PowerupHoleCross_AbilityGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return AbilityScript.Identifier.undefined;
		}
		return instance._powerupHoleCross_AbilityIdentifier;
	}

	public static void PowerupHoleCross_AbilitySet(AbilityScript.Identifier identifier)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupHoleCross_AbilityIdentifier = identifier;
	}

	// (get) Token: 0x06000276 RID: 630
	// (set) Token: 0x06000277 RID: 631
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

	public static void MaxEquippablePowerupsAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.MaxEquippablePowerupsSet(instance.maxEquippablePowerups + value);
	}

	public static void MaxEquippablePowerupsReset()
	{
		GameplayData.MaxEquippablePowerupsSet(7);
	}

	private static int _MaxEquippablePowerupsGet_AbsoluteMaximum()
	{
		return ItemOrganizerScript.CharmsSlotN();
	}

	public static float PowerupCoinsMultiplierGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1f;
		}
		return instance.powerupCoinsMultiplier;
	}

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

	public static void PowerupCoinsMultiplierAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.PowerupCoinsMultiplierSet(instance.powerupCoinsMultiplier + value);
	}

	public static void PowerupCoinsMultiplierReset()
	{
		GameplayData.PowerupCoinsMultiplierSet(1f);
	}

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

	public static void RedButtonActivationsMultiplierAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.RedButtonActivationsMultiplierSet(instance._redButtonActivationsMultiplier + value);
	}

	private void _Phone_PrepareForSerialization()
	{
		this.phoneAbilitiesPickHistory_AsString = PlatformDataMaster.EnumListToString<AbilityScript.Identifier>(this.phoneAbilitiesPickHistory, ',');
		this._phone_AbilitiesToPick_String = PlatformDataMaster.EnumListToString<AbilityScript.Identifier>(this._phone_AbilitiesToPick, ',');
		this.nineNineNine_TotalRewardEarned_ByteArray = this.nineNineNine_TotalRewardEarned.ToByteArray();
	}

	private void _Phone_RestoreFromSerialization()
	{
		this.phoneAbilitiesPickHistory = PlatformDataMaster.EnumListFromString<AbilityScript.Identifier>(this.phoneAbilitiesPickHistory_AsString, ',');
		this._phone_AbilitiesToPick = PlatformDataMaster.EnumListFromString<AbilityScript.Identifier>(this._phone_AbilitiesToPick_String, ',');
		this.nineNineNine_TotalRewardEarned = this.BigIntegerFromByteArray(this.nineNineNine_TotalRewardEarned_ByteArray, 0);
	}

	// (get) Token: 0x06000286 RID: 646
	// (set) Token: 0x06000287 RID: 647
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

	public static void Phone_SpeciallCallBooking_Reset()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._phone_bookSpecialCall = false;
	}

	public static bool NineNineNine_IsTime()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance._phone_SpecialCalls_Counter > 3 && instance._phone_EvilCallsPicked_Counter <= 0;
	}

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

	public static int PhoneAbilitiesNumber_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance._phoneAbilitiesNumber;
	}

	public static void PhoneAbilitiesNumber_SetToMAX()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._phoneAbilitiesNumber = 4;
	}

	public static void PhoneAbilitiesNumber_SetToDefault()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._phoneAbilitiesNumber = 3;
	}

	public static long PhoneRerollCostGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1L;
		}
		return instance._phoneRerollCost;
	}

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

	public static void PhoneRerollCostIncrease()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._phoneRerollCost += instance._phoneRerollCostIncrease;
	}

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

	public static void PhonePickMultiplierAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.PhonePickMultiplierSet(instance._phonePickMultiplier + value);
	}

	public static BigInteger NineNineNne_TotalRewardEarned_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.nineNineNine_TotalRewardEarned;
	}

	public static void NineNineNne_TotalRewardEarned_Set(BigInteger n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.nineNineNine_TotalRewardEarned = n;
	}

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

	public static int PhoneAbilities_GetSkippedCount_Total()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.phoneEasyCounter_SkippedCalls_Total;
	}

	public static int PhoneAbilities_GetSkippedCount_Normal()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.phoneEasyCounter_SkippedCalls_Normal;
	}

	public static int PhoneAbilities_GetSkippedCount_Evil()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.phoneEasyCounter_SkippedCalls_Evil;
	}

	public static int PhoneAbilities_GetSkippedCount_Good()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.phoneEasyCounter_SkippedCalls_Good;
	}

	public static long PhoneRerollPerformed_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._phoneRerollsPerformed;
	}

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

	// (get) Token: 0x060002A1 RID: 673
	// (set) Token: 0x060002A2 RID: 674
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

	private void RunModifiers_SavePreparing()
	{
		if (this.runModifierPicked == RunModifierScript.Identifier.undefined || this.runModifierPicked == RunModifierScript.Identifier.count)
		{
			this.runModifierPicked = RunModifierScript.Identifier.defaultModifier;
		}
		this.runModifierPicked_AsString = PlatformDataMaster.EnumEntryToString<RunModifierScript.Identifier>(this.runModifierPicked);
	}

	private void RunModifiers_LoadFormat()
	{
		this.runModifierPicked = PlatformDataMaster.EnumEntryFromString<RunModifierScript.Identifier>(this.runModifierPicked_AsString, RunModifierScript.Identifier.defaultModifier);
		if (this.runModifierPicked == RunModifierScript.Identifier.undefined || this.runModifierPicked == RunModifierScript.Identifier.count)
		{
			this.runModifierPicked = RunModifierScript.Identifier.defaultModifier;
		}
	}

	public static RunModifierScript.Identifier RunModifier_GetCurrent()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return RunModifierScript.Identifier.defaultModifier;
		}
		return instance.runModifierPicked;
	}

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
				if (identifier != RunModifierScript.Identifier.defaultModifier)
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

	public static bool RunModifier_AlreadyPicked()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance._runModifier_AlreadySet;
	}

	public static bool RunModifier_DealIsAvailable_Get()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.runModifier_DealIsAvailable;
	}

	public static void RunModifier_DealIsAvailable_Set(bool value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.runModifier_DealIsAvailable = value;
	}

	public static int RunModifier_AcceptedDealsCounter_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.runModifier_AcceptedDealsCounter;
	}

	public static void RunModifier_AcceptedDealsCounter_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.runModifier_AcceptedDealsCounter = Mathf.Max(0, n);
	}

	public static int RunModifier_BonusPacksThisTime_Get()
	{
		int num3 = (int)GameplayData.RunModifier_GetCurrent();
		int num2 = 0;
		if (num3 == 10)
		{
			num2++;
		}
		return GameplayData.RunModifier_AcceptedDealsCounter_Get() + 1 + num2;
	}

	private void _MetaProgression_PrepareForSerialization()
	{
	}

	private void _MetaProgression_RestoreFromSerialization()
	{
	}

	public static bool AlreadyBoughtPowerupAtTerminalGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance._alreadyBoughtPowerupAtTerminal;
	}

	public static void AlreadyBoughtPowerupAtTerminalSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._alreadyBoughtPowerupAtTerminal = true;
	}

	public static bool NewGameIntroFinished_Get()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.newGameIntro_Finished;
	}

	public static void NewGameIntroFinished_Set(bool finished)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.newGameIntro_Finished = finished;
	}

	private void _Ending_PrepareForSerialization()
	{
		this.rewardBoxDebtIndex_ByteArray = this.rewardBoxDebtIndex.ToByteArray();
	}

	private void _Ending_RestoreFromSerialization()
	{
		this.rewardBoxDebtIndex = this.BigIntegerFromByteArray(this.rewardBoxDebtIndex_ByteArray, -1);
	}

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

	public static BigInteger GetRewardDeadlineDebt()
	{
		if (GameplayData.Instance == null)
		{
			return 100000;
		}
		return GameplayData.DebtGetExt(GameplayData.GetRewardBoxDebtIndex());
	}

	public static bool RewardTimeToShowAmmount()
	{
		return GameplayData.Instance != null && GameplayData.DebtIndexGet() >= GameplayData.GetRewardBoxDebtIndex();
	}

	public static bool WinConditionAlreadyAchieved()
	{
		return GameplayData.Instance != null && (GameplayData.DebtIndexGet() > GameplayData.GetRewardBoxDebtIndex() || RewardBoxScript.IsOpened());
	}

	public static bool KeptPlayingPastWinConditionGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.keptPlayingPastWinCondition;
	}

	public static void KeptPlayingPastWinConditionSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.keptPlayingPastWinCondition = true;
	}

	public static bool RewardBoxIsOpened()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.rewardBoxWasOpened;
	}

	public static void RewardBoxSetOpened()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.rewardBoxWasOpened = true;
	}

	public static bool RewardBoxHasPrize()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.rewardBoxHasPrize;
	}

	public static void RewardBoxPickupPrize()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.rewardBoxHasPrize = false;
	}

	public static bool PrizeWasUsedGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.prizeWasUsed;
	}

	public static void PrizeWasUsedSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.prizeWasUsed = true;
	}

	public static bool IsInVictoryCondition()
	{
		return GameplayData.Instance != null && !GameplayMaster.GameIsResetting() && GameplayData.RewardBoxIsOpened() && !GameplayData.RewardBoxHasPrize() && GameplayData.PrizeWasUsedGet();
	}

	public static bool IsInGoodEndingCondition(bool considerKeyState)
	{
		if (!considerKeyState)
		{
			return GameplayData.NineNineNine_IsTime();
		}
		return GameplayData.IsInVictoryCondition() && GameplayData.NineNineNine_IsTime();
	}

	public static bool IsInBadEndingCondition(bool considerKeyState)
	{
		if (!considerKeyState)
		{
			return !GameplayData.NineNineNine_IsTime();
		}
		return GameplayData.IsInVictoryCondition() && !GameplayData.NineNineNine_IsTime();
	}

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

	// (get) Token: 0x060002C6 RID: 710
	// (set) Token: 0x060002C7 RID: 711
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

	public static long Stats_ModifiedLemonTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedLemonTriggeredTimes;
	}

	public static void Stats_ModifiedLemonTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedLemonTriggeredTimes += 1L;
	}

	public static long Stats_ModifiedCherryTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedCherryTriggeredTimes;
	}

	public static void Stats_ModifiedCherryTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedCherryTriggeredTimes += 1L;
	}

	public static long Stats_ModifiedCloverTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedCloverTriggeredTimes;
	}

	public static void Stats_ModifiedCloverTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedCloverTriggeredTimes += 1L;
	}

	public static long Stats_ModifiedBellTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedBellTriggeredTimes;
	}

	public static void Stats_ModifiedBellTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedBellTriggeredTimes += 1L;
	}

	public static long Stats_ModifiedDiamondTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedDiamondTriggeredTimes;
	}

	public static void Stats_ModifiedDiamondTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedDiamondTriggeredTimes += 1L;
	}

	public static long Stats_ModifiedCoinsTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedCoinsTriggeredTimes;
	}

	public static void Stats_ModifiedCoinsTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedCoinsTriggeredTimes += 1L;
	}

	public static long Stats_ModifiedSevenTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedSevenTriggeredTimes;
	}

	public static void Stats_ModifiedSevenTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedSevenTriggeredTimes += 1L;
	}

	public static long Stats_ModifiedSymbol_TriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedSymbolTriggeredTimes;
	}

	public static void Stats_ModifiedSymbol_TriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedSymbolTriggeredTimes += 1L;
	}

	public static int Stats_RedButtonEffectiveActivations_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.stats_RedButtonEffectiveActivationsCounter;
	}

	public static void Stats_RedButtonEffectiveActivations_Set(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_RedButtonEffectiveActivationsCounter = value;
	}

	public static long Stats_RestocksBoughtGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_RestocksBoughtN;
	}

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

	public static long Stats_RestocksPerformedGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_RestocksPerformed;
	}

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

	public static long Stats_PeppersBoughtGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_PeppersBought;
	}

	public static void Stats_PeppersBoughtAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_PeppersBought += 1L;
	}

	// (get) Token: 0x060002E0 RID: 736
	// (set) Token: 0x060002E1 RID: 737
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

	private void _GameStats_PrepareForSerialization()
	{
		this.stats_CoinsEarned_ByteArray = this.stats_CoinsEarned.ToByteArray();
	}

	private void _GameStats_RestoreFromSerialization()
	{
		this.stats_CoinsEarned = this.BigIntegerFromByteArray(this.stats_CoinsEarned_ByteArray, 0);
	}

	public static long Stats_PlayTime_GetSeconds()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_PlayTime_Seconds;
	}

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

	public static long Stats_DeadlinesCompleted_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_DeadlinesCompleted;
	}

	public static void Stats_DeadlinesCompleted_Add()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_DeadlinesCompleted += 1L;
	}

	public static BigInteger Stats_CoinsEarned_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.stats_CoinsEarned;
	}

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

	public static long Stats_TicketsEarned_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_TicketsEarned;
	}

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

	public static long Stats_CharmsBought_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_CharmsBought;
	}

	public static void Stats_CharmsBought_Add()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_CharmsBought += 1L;
	}

	[SerializeField]
	private int seed;

	[SerializeField]
	public Rng rngRunMod;

	[SerializeField]
	public Rng rngSymbolsMod;

	[SerializeField]
	public Rng rngPowerupsMod;

	[SerializeField]
	public Rng rngSymbolsChance;

	[SerializeField]
	public Rng rngCards;

	[SerializeField]
	public Rng rngPowerupsAll;

	[SerializeField]
	public Rng rngAbilities;

	[SerializeField]
	public Rng rngDrawers;

	[SerializeField]
	public Rng rngStore;

	[SerializeField]
	public Rng rngStoreChains;

	[SerializeField]
	public Rng rngPhone;

	[SerializeField]
	public Rng rngSlotMachineLuck;

	[SerializeField]
	public Rng rng666;

	[SerializeField]
	public Rng rngGarbage;

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

	public string[] equippedPowerups_Skeleton = new string[]
	{
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined)
	};

	public string[] drawerPowerups = new string[]
	{
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined),
		PlatformDataMaster.EnumEntryToString<PowerupScript.Identifier>(PowerupScript.Identifier.undefined)
	};

	public string[] storePowerups = new string[4];

	public int storeChainIndex_Array;

	public int storeChainIndex_PowerupIdentifier;

	public int storeLastRandomIndex;

	private BigInteger _storeRestockExtraCost = 0;

	[SerializeField]
	private byte[] _storeRestockExtraCost_ByteArray;

	[SerializeField]
	private long _storeFreeRestocks = 1L;

	[SerializeField]
	private long temporaryDiscount;

	[SerializeField]
	private long[] temporaryDiscountPerSlot = new long[4];

	private const int COINS_INITIAL = 13;

	private const int COINS_DEPOSITED_INITIAL = 30;

	private const int COINS_INTEREST_EARNED_INITIAL = 0;

	private const float COINS_INTEREST_RATE = 7f;

	private BigInteger coins = 13;

	private BigInteger depositedCoins = 30;

	private BigInteger interestEarned = 0;

	[SerializeField]
	private float interestRate = 7f;

	private BigInteger roundEarnedCoins = 0;

	[SerializeField]
	private byte[] coins_ByteArray;

	[SerializeField]
	private byte[] depositedCoins_ByteArray;

	[SerializeField]
	private byte[] interestEarned_ByteArray;

	[SerializeField]
	private byte[] roundEarnedCoins_ByteArray;

	public const long CLOVER_TICKETS_INITIAL = 4L;

	private const long CLOVER_TICKETS_BONUS_FOR_LITTLE_BET = 2L;

	private const long CLOVER_TICKETS_BONUS_FOR_BIG_BET = 1L;

	private const long CLOVER_TICKETS_BONUS_FOR_ROUNDS_LEFT = 4L;

	[SerializeField]
	private long cloverTickets = 4L;

	[SerializeField]
	private long cloverTickets_BonusFor_LittleBet = 2L;

	[SerializeField]
	private long cloverTickets_BonusFor_BigBet = 1L;

	[SerializeField]
	private long cloverTickets_BonusFor_RoundsLeft = 4L;

	[SerializeField]
	private bool atmDeadline_RewardPickupMemo_MessageShown;

	private const int ROUNDS_PER_DEADLINE_DEFAULT = 3;

	private const int ROUNDS_PER_DEADLINE_RUN_MOD_MORE_ROUNDS_SMALL_ROUNDS = 7;

	private const int ROUNDS_PER_DEADLINE_RUN_MOD_ONE_ROUND_PER_DEADLINE = 1;

	private const int DEBT_INDEX_DEFAULT = 0;

	public const int DEBT_OUT_OF_RANGE_MULTIPLIER_DEFAULT = 6;

	[SerializeField]
	private int roundDeadlineTrail;

	[SerializeField]
	private int roundDeadlineTrail_AtDeadlineBegin;

	[SerializeField]
	private int roundsReallyPlayed;

	[SerializeField]
	private int roundOfDeadline = 3;

	private BigInteger debtIndex = 0;

	private BigInteger debtOutOfRangeMult = 6;

	private static int[] debtsInRange = new int[] { 75, 200, 666, 2222, 12500, 33333, 66666, 200000, 1000000 };

	[SerializeField]
	private byte[] debtIndex_ByteArray;

	[SerializeField]
	private byte[] debtOutOfRangeMult_ByteArray;

	[SerializeField]
	private bool skeletonIsCompleted;

	[SerializeField]
	private bool victoryDeathConditionMet;

	public const int MAX_BUYABLE_SPINS_PER_ROUND = 7;

	[SerializeField]
	private int spinsLeft;

	[SerializeField]
	private int spinsDoneInARun;

	[SerializeField]
	private int extraSpins;

	[SerializeField]
	private int maxSpins = 7;

	[SerializeField]
	private bool lastBetIsSmall;

	[SerializeField]
	private int spinsWithoutReward;

	[SerializeField]
	private long _smallBetsPickedCounter;

	[SerializeField]
	private long _bigBetsPickedCounter;

	[SerializeField]
	private int spinsWithout5PlusPatterns;

	[SerializeField]
	private int consecutiveSpinsWithDiamTreasSevens;

	[SerializeField]
	private long _jackpotsScoredCounter;

	[SerializeField]
	private long _spinsWithAtleast1Jackpot;

	[SerializeField]
	private int _slotInitialLuckRndOffset = -1;

	[SerializeField]
	private int _slotOccasionalLuckSpinN = -1;

	private const float BASE_LUCK_MIN = 0.25f;

	private const float BASE_LUCK_DECREASE_PER_SPIN = 0.001f;

	private const int EXTRA_LUCK_MAX_ENTRIES = 20;

	private const float POWERUP_LUCK_DEFAULT = 1f;

	private const float POWERUP_LUCK_MIN = 0.5f;

	private const float ACTIVATION_LUCK_DEFAULT = 1f;

	private const float ACTIVATION_LUCK_MIN = 0.5f;

	private const float STORE_LUCK_DEFAULT = 1f;

	private const float STORE_LUCK_MIN = 0.5f;

	[SerializeField]
	private GameplayData.ExtraLuckEntry[] extraLuckEntries;

	[SerializeField]
	private float powerupLuck = 1f;

	[SerializeField]
	private float activationLuck = 1f;

	[SerializeField]
	private float storeLuck = 1f;

	private const int SYMBOL_COINS_EXTRA_VALUE_DEFAULT = 0;

	private const int SYMBOL_CHANCE_MIN_VALUE = 0;

	private const int ALL_SYMBOLS_MULTIPLIER_DEFAULT = 1;

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

	[SerializeField]
	private string[] symbolsAvailable_AsString;

	[SerializeField]
	private GameplayData.SymbolData[] symbolsData;

	private BigInteger allSymbolsMultiplier = 1;

	[SerializeField]
	private byte[] allSymbolsMultiplier_ByteArray;

	private static List<SymbolScript.Kind> _symbolsOrderedByHighestValueToLowest = new List<SymbolScript.Kind>();

	private static List<SymbolScript.Kind> _mostValuableSymbols = new List<SymbolScript.Kind>();

	private static List<SymbolScript.Kind> _leastValuableSymbols = new List<SymbolScript.Kind>();

	private static List<SymbolScript.Kind> _symbolsOrderedByHighestChanceToLowest = new List<SymbolScript.Kind>();

	private static List<SymbolScript.Kind> _mostProbableSymbols = new List<SymbolScript.Kind>();

	private static List<SymbolScript.Kind> _leastProbableSymbols = new List<SymbolScript.Kind>();

	private static float[] modifierChanceFloats = new float[6];

	private static float[] modifierChanceFloats_SuccessThreshold = new float[6];

	private const int ALL_PATTERNS_MULTIPLIER_DEFAULT = 1;

	private const int _666_MINIMUM_DEBT_INDEX = 2;

	private const int _SUPER_666_MINIMUM_DEBT_INDEX = 6;

	private const float _666_CHANCE_DEFAULT = 0.015f;

	private const float _666_CHANCE_DEFAULT_MAX_ABSOLUTE = 0.3f;

	private const float SIX_SIX_SIX_GRADUAL_INCREASE_AMMOUNT = 0.0015f;

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

	[SerializeField]
	private string[] patternsAvailable_AsString;

	[SerializeField]
	private GameplayData.PatternData[] patternsData;

	private BigInteger allPatternsMultiplier = 1;

	[SerializeField]
	private byte[] allPatternsMultiplier_ByteArray;

	[SerializeField]
	private float _666Chance = 0.015f;

	[SerializeField]
	private float _666ChanceMaxAbsolute = 0.3f;

	[SerializeField]
	private int _666BookedSpin = -1;

	[SerializeField]
	private int _666SuppressedSpinsLeft;

	[SerializeField]
	private bool _lastRoundHadA666;

	private static List<PatternScript.Kind> _patternsOrderedByHighestValueToLowest = new List<PatternScript.Kind>();

	private static List<PatternScript.Kind> _mostValuablePatterns = new List<PatternScript.Kind>();

	private static List<PatternScript.Kind> _leastValuablePatterns = new List<PatternScript.Kind>();

	[SerializeField]
	private GameplayData.PowerupData[] powerupsData;

	[SerializeField]
	private int _rndActivationFailsafe_ConsolationPrize;

	[SerializeField]
	private int _rndActivationFailsafe_BrokenCalculator;

	[SerializeField]
	private int _rndActivationFailsafe_CrankGenerator;

	[SerializeField]
	private int _rndActivationFailsafe_FakeCoin;

	[SerializeField]
	private int _rndActivationFailsafe_RedPepper;

	[SerializeField]
	private int _rndActivationFailsafe_GreenPepper;

	[SerializeField]
	private int _rndActivationFailsafe_GoldenPepper;

	[SerializeField]
	private int _rndActivationFailsafe_RottenPepper;

	[SerializeField]
	private int _rndActivationFailsafe_BellPepper;

	[SerializeField]
	private int _rndActivationFailsafe_Rosary;

	[SerializeField]
	private int _rndActivationFailsafe_Dice4;

	[SerializeField]
	private int _rndActivationFailsafe_SacredHeart;

	[SerializeField]
	private int _powerupHourglass_DeadlinesLeft = 3;

	[SerializeField]
	private int _powerupHourglass_DeadlinesCounter;

	private const int POWERUP_FRUIT_BASKET_DEFAULT_ROUNDS = 10;

	[SerializeField]
	private int _powerupFruitsBasket_RoundsLeft = 10;

	private BigInteger _powerupTarotDeck_Reward = 0;

	[SerializeField]
	private byte[] _powerupTarotDeck_Reward_ByteArray;

	private BigInteger _powerupPoopBeetle_SymbolsIncreaserMult = 0;

	[SerializeField]
	private byte[] _powerupPoopBeetle_SymbolsIncreasetMultByteArray;

	private const int _POWERUP_GRANDMAS_PURSE_INTEREST_DEFAULT = 15;

	[SerializeField]
	private int _powerupGrandmasPurse_ExtraInterest = 15;

	[SerializeField]
	private int _powerupOneTrickPony_TargetSpinsLeftIndex = -1;

	[SerializeField]
	private int _powerupPentacleTriggeredTimes;

	private BigInteger _powerupCalendar_SymbolsIncreaserMult = 0;

	[SerializeField]
	private byte[] _powerupCalendar_SymbolsIncreaserMultByteArray;

	private BigInteger _powerupGigaMushroom_SymbLemonsValue = 0;

	private BigInteger _powerupGigaMushroom_SymbCherriesValue = 0;

	private BigInteger _powerupGigaMushroom_SymbCloversValue = 0;

	private BigInteger _powerupGigaMushroom_SymbBellsValue = 0;

	private BigInteger _powerupGigaMushroom_SymbDiamondsValue = 0;

	private BigInteger _powerupGigaMushroom_SymbCoinsValue = 0;

	private BigInteger _powerupGigaMushroom_SymbSevensValue = 0;

	[SerializeField]
	private byte[] _powerupGigaMushroom_SymbLemonsValue_ByteArray;

	[SerializeField]
	private byte[] _powerupGigaMushroom_SymbCherriesValue_ByteArray;

	[SerializeField]
	private byte[] _powerupGigaMushroom_SymbCloversValue_ByteArray;

	[SerializeField]
	private byte[] _powerupGigaMushroom_SymbBellsValue_ByteArray;

	[SerializeField]
	private byte[] _powerupGigaMushroom_SymbDiamondsValue_ByteArray;

	[SerializeField]
	private byte[] _powerupGigaMushroom_SymbCoinsValue_ByteArray;

	[SerializeField]
	private byte[] _powerupGigaMushroom_SymbSevensValue_ByteArray;

	[SerializeField]
	private int _powerupGoldenHorseShoe_SpinsLeft;

	[SerializeField]
	private int _powerupAncientCoin_SpinsLeft;

	[SerializeField]
	private int _powerupChannelerOfFortunes_ActivationsCounter;

	[SerializeField]
	private double[] _powerupPareidolia_PatternBonuses = new double[16];

	[SerializeField]
	private long _powerupRingBell_BonusCounter;

	[SerializeField]
	private long _powerupConsolationPrize_BonusCounter;

	[SerializeField]
	private int _powerupStepsCounter_TriggersCounter;

	[SerializeField]
	private int _powerupDieselLocomotiveBonus;

	[SerializeField]
	private int _powerupSteamLocomotiveBonus;

	[SerializeField]
	private int _powerupDiscA_SpinsCounter;

	[SerializeField]
	private int _powerupDiscB_SpinsCounter;

	[SerializeField]
	private int _powerupDiscC_SpinsCounter;

	[SerializeField]
	private int _powerupWeirdClock_DeadlineUses;

	[SerializeField]
	private int _powerupCigarettesActivationsCounter;

	[SerializeField]
	private int jimboRoundsLeft = -1;

	private List<GameplayData.JimboAbility> jimboAbilities_BadPool = new List<GameplayData.JimboAbility>
	{
		GameplayData.JimboAbility.Bad_666Chance1_5,
		GameplayData.JimboAbility.Bad_666Chance3,
		GameplayData.JimboAbility.Bad_2LessSpin,
		GameplayData.JimboAbility.Bad_3LessSpins,
		GameplayData.JimboAbility.Bad_Discard6,
		GameplayData.JimboAbility.Bad_Discard3
	};

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

	private List<GameplayData.JimboAbility> jimboAbilities_Selected = new List<GameplayData.JimboAbility>(3);

	[SerializeField]
	private string jimboAbilities_Selected_Str = "";

	private static StringBuilder jimboSB = new StringBuilder();

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

	[SerializeField]
	private int _powerupRedPepper_ActivationsCounter;

	[SerializeField]
	private int _powerupGreenPepper_ActivationsCounter;

	[SerializeField]
	private int _powerupGoldenPepper_LuckBonus;

	[SerializeField]
	private int _powerupRottenPepper_LuckBonus;

	[SerializeField]
	private int _powerupBellPepper_LuckBonus;

	[SerializeField]
	private long _powerupDevilHorn_AdditionalMultiplier;

	[SerializeField]
	private int _powerupBaphomet_SymbolsBonus = 1;

	[SerializeField]
	private int _powerupBaphomet_PatternsBonus = 1;

	[SerializeField]
	private long _powerupCross_Triggers;

	[SerializeField]
	private int _powerupPossessedPhone_SpinsCount;

	[SerializeField]
	private float _powerupGoldenKingMida_ExtraBonus;

	[SerializeField]
	private float _powerupDealer_ExtraBonus;

	[SerializeField]
	private float _powerupCapitalist_ExtraBonus;

	private const float POWERUP_PERSONAL_TRAINER_BONUS_DEFAULT = 0.25f;

	[SerializeField]
	private float _powerupPersonalTrainer_Bonus = 0.25f;

	private const float POWERUP_ELECTRICIAN_BONUS_DEFAULT = 0.05f;

	[SerializeField]
	private float _powerupElectrician_Bonus = 0.05f;

	private const float POWERUP_FORTUNE_TELLER_BONUS_DEFAULT = 0.25f;

	[SerializeField]
	private float _powerupFortuneTeller_Bonus = 0.25f;

	[SerializeField]
	private long _powerupAceOfClubs_TicketsSpent;

	[SerializeField]
	private long _powerupAceOfSpades_ActivationsCounter;

	[SerializeField]
	private string _powerupHoleCircle_CharmStr;

	private PowerupScript.Identifier _powerupHoleCircle_CharmIdentifier = PowerupScript.Identifier.undefined;

	[SerializeField]
	private string _powerupHoleRomboid_CharmStr;

	private PowerupScript.Identifier _powerupHoleRomboid_CharmIdentifier = PowerupScript.Identifier.undefined;

	[SerializeField]
	private string _powerupHoleCross_AbilityStr;

	private AbilityScript.Identifier _powerupHoleCross_AbilityIdentifier = AbilityScript.Identifier.undefined;

	[SerializeField]
	private int _powerupOphanimWheels_JackpotsCounter;

	public const int POWERUPS_MAX_EQUIPPABLE_DEFAULT = 7;

	private const float POWERUP_GRANTED_COINS_MULTIPLIER = 1f;

	[SerializeField]
	private int maxEquippablePowerups = 7;

	[SerializeField]
	private float powerupCoinsMultiplier = 1f;

	[SerializeField]
	private int _redButtonActivationsMultiplier = 1;

	[SerializeField]
	private int _abilityHoly_PatternsRepetitions;

	[NonSerialized]
	public List<AbilityScript.Identifier> phoneAbilitiesPickHistory = new List<AbilityScript.Identifier>();

	[SerializeField]
	private string phoneAbilitiesPickHistory_AsString = "";

	[SerializeField]
	public int _phone_PickupWithAbilities_OverallCounter;

	[SerializeField]
	public bool _phone_bookSpecialCall;

	[SerializeField]
	public int _phone_SpecialCalls_Counter;

	[SerializeField]
	public int _phone_EvilCallsPicked_Counter;

	[SerializeField]
	public int _phone_EvilCallsIgnored_Counter;

	[SerializeField]
	public bool _phoneAlreadyTransformed;

	[SerializeField]
	public bool _phone_pickedUpOnceLastDeadline;

	[SerializeField]
	public bool _phone_abilityAlreadyPickedUp = true;

	[SerializeField]
	public AbilityScript.Category _phone_lastAbilityCategory = AbilityScript.Category.undefined;

	[SerializeField]
	private string _phone_AbilitiesToPick_String = "";

	[NonSerialized]
	public List<AbilityScript.Identifier> _phone_AbilitiesToPick = new List<AbilityScript.Identifier>();

	private const int PHONE_ABILITIES_NUM_DEFAULT = 3;

	public const int PHONE_ABILITIES_NUM_MAX = 4;

	[SerializeField]
	private int _phoneAbilitiesNumber = 3;

	private const long PHONE_REROLL_COST_DEFAULT = 1L;

	[SerializeField]
	private long _phoneRerollCostIncrease = 1L;

	[SerializeField]
	private long _phoneRerollCost = 1L;

	private const int PHONE_PICK_MULTIPLIER_DEFAULT = 1;

	[SerializeField]
	private int _phonePickMultiplier = 1;

	[SerializeField]
	private byte[] nineNineNine_TotalRewardEarned_ByteArray;

	private BigInteger nineNineNine_TotalRewardEarned = 0;

	private int _phoneAbilChahce_CounterPrevAbilitiesCount = -1;

	private int _phoneAbilChache_normalCount;

	private int _phoneAbilChache_evilCount;

	private int _phoneAbilChache_holyCount;

	private int _pTempNormal;

	private int _pTempEvil;

	private int _pTempGood;

	[SerializeField]
	public int phoneEasyCounter_SkippedCalls_Total;

	[SerializeField]
	public int phoneEasyCounter_SkippedCalls_Normal;

	[SerializeField]
	public int phoneEasyCounter_SkippedCalls_Evil;

	[SerializeField]
	public int phoneEasyCounter_SkippedCalls_Good;

	[SerializeField]
	private long _phoneRerollsPerformed;

	[SerializeField]
	private long _phoneRerollsPerformed_PerDeadline;

	private const bool RUN_MODIFIERS_LOG = false;

	[SerializeField]
	private string runModifierPicked_AsString;

	private RunModifierScript.Identifier runModifierPicked;

	[SerializeField]
	private bool _runModifier_AlreadySet;

	[SerializeField]
	private bool runModifier_DealIsAvailable;

	[SerializeField]
	private int runModifier_AcceptedDealsCounter;

	[SerializeField]
	private bool _alreadyBoughtPowerupAtTerminal;

	[SerializeField]
	private bool newGameIntro_Finished;

	private BigInteger rewardBoxDebtIndex = 7;

	[SerializeField]
	private byte[] rewardBoxDebtIndex_ByteArray;

	[SerializeField]
	private bool doorKeyDeadlineDefined;

	[SerializeField]
	private bool keptPlayingPastWinCondition;

	[SerializeField]
	private bool rewardBoxWasOpened;

	[SerializeField]
	private bool rewardBoxHasPrize = true;

	[SerializeField]
	private bool prizeWasUsed;

	[SerializeField]
	private int rewardKind = 7;

	[SerializeField]
	private long stats_ModifiedLemonTriggeredTimes;

	[SerializeField]
	private long stats_ModifiedCherryTriggeredTimes;

	[SerializeField]
	private long stats_ModifiedCloverTriggeredTimes;

	[SerializeField]
	private long stats_ModifiedBellTriggeredTimes;

	[SerializeField]
	private long stats_ModifiedDiamondTriggeredTimes;

	[SerializeField]
	private long stats_ModifiedCoinsTriggeredTimes;

	[SerializeField]
	private long stats_ModifiedSevenTriggeredTimes;

	[SerializeField]
	private long stats_ModifiedSymbolTriggeredTimes;

	[SerializeField]
	private int stats_RedButtonEffectiveActivationsCounter;

	[SerializeField]
	private long stats_RestocksBoughtN;

	[SerializeField]
	private long stats_RestocksPerformed;

	[SerializeField]
	private long stats_PeppersBought;

	[SerializeField]
	private long stats_PlayTime_Seconds;

	[SerializeField]
	private long stats_DeadlinesCompleted;

	private BigInteger stats_CoinsEarned = 13;

	[SerializeField]
	private byte[] stats_CoinsEarned_ByteArray;

	[SerializeField]
	private long stats_TicketsEarned = 2L;

	[SerializeField]
	private long stats_CharmsBought;

	[SerializeField]
	private long sixSixSixSeen;

	[Serializable]
	private class ExtraLuckEntry
	{
		public string tag;

		public float luck;

		public float luckMax;

		public int spinsLeft;

		public int spinsLeftMax;
	}

	[Serializable]
	private class SymbolData
	{
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

		public string symbolKindAsString = "";

		public BigInteger extraValue = 0;

		public byte[] extraValue_ByteArray;

		public float spawnChance = 1f;

		public float modifierChance01_InstantReward;

		public float modifierChance01_CloverTicket;

		public float modifierChance01_Golden;

		public float modifierChance01_Repetition;

		public float modifierChance01_Battery;

		public float modifierChance01_Chain;
	}

	[Serializable]
	private class PatternData
	{
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

		public string patternKindAsString = "";

		public double extraValue;
	}

	[Serializable]
	public class PowerupData
	{
		public PowerupScript.Identifier IdentifierGetInferred()
		{
			if (this._identifier == PowerupScript.Identifier.undefined)
			{
				this._identifier = PlatformDataMaster.EnumEntryFromString<PowerupScript.Identifier>(this.powerupIdentifierAsString, PowerupScript.Identifier.undefined);
			}
			return this._identifier;
		}

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

		public string powerupIdentifierAsString = "";

		private PowerupScript.Identifier _identifier = PowerupScript.Identifier.undefined;

		public int boughtTimes;

		public PowerupScript.Modifier modifier;

		public int buttonBurnOutCounter;

		public int buttonChargesCounter;

		public int buttonChargesCounter_Absolute;

		public int buttonChargesMax;

		public int resellBonus;

		public Rng charmSpecificRng;
	}

	public enum JimboAbility
	{
		Bad_666Chance1_5,
		Bad_666Chance3,
		Bad_2LessSpin,
		Bad_3LessSpins,
		Bad_Discard6,
		Bad_Discard3,
		Good_Repetitions,
		Good_SymbMult,
		Good_PatternsMult,
		Good_Interest,
		Good_Tickets,
		Good_Luck,
		Good_MoreSpins,
		Good_LemonCherryManifest,
		Good_CloverBellManifest,
		Good_DiamondChestsManifest,
		Good_SevenManifest,
		Good_FreeRestocks,
		Count
	}
}
