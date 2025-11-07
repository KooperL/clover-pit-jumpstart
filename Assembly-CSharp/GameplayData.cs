using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Panik;
using UnityEngine;

[Serializable]
public class GameplayData
{
	// (get) Token: 0x060000EF RID: 239 RVA: 0x00009F87 File Offset: 0x00008187
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

	// Token: 0x060000F0 RID: 240 RVA: 0x00009F9C File Offset: 0x0000819C
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

	// Token: 0x060000F1 RID: 241 RVA: 0x0000A174 File Offset: 0x00008374
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

	// Token: 0x060000F2 RID: 242 RVA: 0x0000A26E File Offset: 0x0000846E
	private BigInteger BigIntegerFromByteArray(byte[] byteArray, BigInteger defaultValue)
	{
		if (byteArray == null || byteArray.Length == 0)
		{
			return defaultValue;
		}
		return new BigInteger(byteArray);
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x0000A280 File Offset: 0x00008480
	public static int SeedGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.seed;
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x0000A2A0 File Offset: 0x000084A0
	public static uint SeedGetAsUInt()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0U;
		}
		return (uint)instance.seed;
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x0000A2C0 File Offset: 0x000084C0
	public static string SeedGetAsString()
	{
		return GameplayData.SeedGetAsUInt().ToString("0000000000");
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x0000A2E0 File Offset: 0x000084E0
	public static void SeedInitRandom()
	{
		Random.InitState((int)DateTime.Now.Ticks);
		uint num = (uint)Random.Range(int.MinValue, int.MaxValue);
		uint num2 = num;
		uint num3 = (uint)(5 + Random.Range(0, 5));
		int num4 = 0;
		while ((long)num4 < (long)((ulong)num3))
		{
			num = (num + num3) ^ Bit.ShiftRotateLeft(num, 31) ^ Bit.ShiftRotateLeft(num, 21) ^ Bit.ShiftRotateLeft(num, 13) ^ Bit.ShiftRotateLeft(num, 1) ^ num2;
			num4++;
		}
		GameplayData.SeedInitSpecific((int)num);
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x0000A360 File Offset: 0x00008560
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

	// Token: 0x060000F8 RID: 248 RVA: 0x0000A388 File Offset: 0x00008588
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

	// Token: 0x060000F9 RID: 249 RVA: 0x0000A524 File Offset: 0x00008724
	public static BigInteger StoreRestockExtraCostGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._storeRestockExtraCost;
	}

	// Token: 0x060000FA RID: 250 RVA: 0x0000A548 File Offset: 0x00008748
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

	// Token: 0x060000FB RID: 251 RVA: 0x0000A584 File Offset: 0x00008784
	public static void StoreRestockExtraCostAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.StoreRestockExtraCostSet(instance._storeRestockExtraCost + value);
	}

	// Token: 0x060000FC RID: 252 RVA: 0x0000A5AC File Offset: 0x000087AC
	public static long StoreFreeRestocksGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._storeFreeRestocks;
	}

	// Token: 0x060000FD RID: 253 RVA: 0x0000A5CC File Offset: 0x000087CC
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

	// Token: 0x060000FE RID: 254 RVA: 0x0000A5F0 File Offset: 0x000087F0
	public static long StoreTemporaryDiscountGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.temporaryDiscount;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x0000A610 File Offset: 0x00008810
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

	// Token: 0x06000100 RID: 256 RVA: 0x0000A648 File Offset: 0x00008848
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

	// Token: 0x06000101 RID: 257 RVA: 0x0000A678 File Offset: 0x00008878
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

	// Token: 0x06000102 RID: 258 RVA: 0x0000A6A0 File Offset: 0x000088A0
	public static long StoreTemporaryDiscountPerSlotGet(int slotIndex)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.temporaryDiscountPerSlot[slotIndex];
	}

	// Token: 0x06000103 RID: 259 RVA: 0x0000A6C4 File Offset: 0x000088C4
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

	// Token: 0x06000104 RID: 260 RVA: 0x0000A704 File Offset: 0x00008904
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

	// Token: 0x06000105 RID: 261 RVA: 0x0000A75C File Offset: 0x0000895C
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

	// Token: 0x06000106 RID: 262 RVA: 0x0000A78C File Offset: 0x0000898C
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

	// Token: 0x06000107 RID: 263 RVA: 0x0000A7E4 File Offset: 0x000089E4
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

	// Token: 0x06000108 RID: 264 RVA: 0x0000A820 File Offset: 0x00008A20
	private void _Coins_PrepareForSerialization()
	{
		this.coins_ByteArray = this.coins.ToByteArray();
		this.depositedCoins_ByteArray = this.depositedCoins.ToByteArray();
		this.interestEarned_ByteArray = this.interestEarned.ToByteArray();
		this.roundEarnedCoins_ByteArray = this.roundEarnedCoins.ToByteArray();
	}

	// Token: 0x06000109 RID: 265 RVA: 0x0000A874 File Offset: 0x00008A74
	private void _Coins_RestoreFromSerialization()
	{
		this.coins = this.BigIntegerFromByteArray(this.coins_ByteArray, 13);
		this.depositedCoins = this.BigIntegerFromByteArray(this.depositedCoins_ByteArray, 30);
		this.interestEarned = this.BigIntegerFromByteArray(this.interestEarned_ByteArray, 0);
		this.roundEarnedCoins = this.BigIntegerFromByteArray(this.roundEarnedCoins_ByteArray, 0);
	}

	// Token: 0x0600010A RID: 266 RVA: 0x0000A8E3 File Offset: 0x00008AE3
	public static BigInteger CoinsGet()
	{
		if (GameplayData.Instance == null)
		{
			return 13;
		}
		return GameplayData.Instance.coins;
	}

	// Token: 0x0600010B RID: 267 RVA: 0x0000A900 File Offset: 0x00008B00
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

	// Token: 0x0600010C RID: 268 RVA: 0x0000A93C File Offset: 0x00008B3C
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

	// Token: 0x0600010D RID: 269 RVA: 0x0000A977 File Offset: 0x00008B77
	public static BigInteger DepositGet()
	{
		if (GameplayData.Instance == null)
		{
			return 30;
		}
		return GameplayData.Instance.depositedCoins;
	}

	// Token: 0x0600010E RID: 270 RVA: 0x0000A994 File Offset: 0x00008B94
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

	// Token: 0x0600010F RID: 271 RVA: 0x0000A9D0 File Offset: 0x00008BD0
	public static void DepositAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.DepositSet(instance.depositedCoins + value);
	}

	// Token: 0x06000110 RID: 272 RVA: 0x0000A9F8 File Offset: 0x00008BF8
	public static bool HasDepositedSomething()
	{
		return GameplayData.DepositGet() > 30L;
	}

	// Token: 0x06000111 RID: 273 RVA: 0x0000AA08 File Offset: 0x00008C08
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

	// Token: 0x06000112 RID: 274 RVA: 0x0000AAE2 File Offset: 0x00008CE2
	public static BigInteger InterestEarnedGet()
	{
		if (GameplayData.Instance == null)
		{
			return 0;
		}
		return GameplayData.Instance.interestEarned;
	}

	// Token: 0x06000113 RID: 275 RVA: 0x0000AAFC File Offset: 0x00008CFC
	public static BigInteger InterestEarnedHypotetically()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.depositedCoins * (int)GameplayData.InterestRateGet() / 100;
	}

	// Token: 0x06000114 RID: 276 RVA: 0x0000AB3B File Offset: 0x00008D3B
	public static void InterestEarnedGrow()
	{
		GameplayData.InterestEarnedGrow_Manual(GameplayData.InterestEarnedHypotetically());
	}

	// Token: 0x06000115 RID: 277 RVA: 0x0000AB48 File Offset: 0x00008D48
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

	// Token: 0x06000116 RID: 278 RVA: 0x0000AB8C File Offset: 0x00008D8C
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

	// Token: 0x06000117 RID: 279 RVA: 0x0000ABBC File Offset: 0x00008DBC
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

	// Token: 0x06000118 RID: 280 RVA: 0x0000AC48 File Offset: 0x00008E48
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

	// Token: 0x06000119 RID: 281 RVA: 0x0000AC98 File Offset: 0x00008E98
	public static void InterestRateAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.InterestRateSet(instance.interestRate + value);
	}

	// Token: 0x0600011A RID: 282 RVA: 0x0000ACBC File Offset: 0x00008EBC
	public static BigInteger RoundEarnedCoinsGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.roundEarnedCoins;
	}

	// Token: 0x0600011B RID: 283 RVA: 0x0000ACE0 File Offset: 0x00008EE0
	public static void RoundEarnedCoinsSet(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.roundEarnedCoins = value;
	}

	// Token: 0x0600011C RID: 284 RVA: 0x0000AD00 File Offset: 0x00008F00
	public static void RoundEarnedCoinsAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.RoundEarnedCoinsSet(instance.roundEarnedCoins + value);
	}

	// Token: 0x0600011D RID: 285 RVA: 0x0000AD28 File Offset: 0x00008F28
	public static void RoundEarnedCoinsReset()
	{
		GameplayData.RoundEarnedCoinsSet(0);
	}

	// Token: 0x0600011E RID: 286 RVA: 0x0000AD38 File Offset: 0x00008F38
	public static long CloverTicketsGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 2L;
		}
		return instance.cloverTickets;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x0000AD58 File Offset: 0x00008F58
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

	// Token: 0x06000120 RID: 288 RVA: 0x0000AD88 File Offset: 0x00008F88
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

	// Token: 0x06000121 RID: 289 RVA: 0x0000ADBC File Offset: 0x00008FBC
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

	// Token: 0x06000122 RID: 290 RVA: 0x0000ADEC File Offset: 0x00008FEC
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

	// Token: 0x06000123 RID: 291 RVA: 0x0000AE1C File Offset: 0x0000901C
	public static void CloverTickets_BonusLittleBet_Add(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.CloverTickets_BonusLittleBet_Set(instance.cloverTickets_BonusFor_LittleBet + value);
	}

	// Token: 0x06000124 RID: 292 RVA: 0x0000AE40 File Offset: 0x00009040
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

	// Token: 0x06000125 RID: 293 RVA: 0x0000AE70 File Offset: 0x00009070
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

	// Token: 0x06000126 RID: 294 RVA: 0x0000AEA0 File Offset: 0x000090A0
	public static void CloverTickets_BonusBigBet_Add(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.CloverTickets_BonusBigBet_Set(instance.cloverTickets_BonusFor_BigBet + value);
	}

	// Token: 0x06000127 RID: 295 RVA: 0x0000AEC4 File Offset: 0x000090C4
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

	// Token: 0x06000128 RID: 296 RVA: 0x0000AEF0 File Offset: 0x000090F0
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

	// Token: 0x06000129 RID: 297 RVA: 0x0000AF20 File Offset: 0x00009120
	public static void CloverTickets_BonusRoundsLeft_Add(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.CloverTickets_BonusRoundsLeft_Set(instance.cloverTickets_BonusFor_RoundsLeft + value);
	}

	// Token: 0x0600012A RID: 298 RVA: 0x0000AF44 File Offset: 0x00009144
	public static bool ATMDeadline_RewardPickupMemo_MessageShownGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.atmDeadline_RewardPickupMemo_MessageShown;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x0000AF64 File Offset: 0x00009164
	public static void ATMDeadline_RewardPickupMemo_MessageShownSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.atmDeadline_RewardPickupMemo_MessageShown = true;
	}

	// Token: 0x0600012C RID: 300 RVA: 0x0000AF84 File Offset: 0x00009184
	public static BigInteger DeadlineReward_CoinsGet(BigInteger currentDebtIndex)
	{
		if (GameplayData.Instance == null)
		{
			return 0;
		}
		0;
		if (currentDebtIndex < 0L)
		{
			currentDebtIndex = 0;
		}
		return GameplayData.SpinCostGet_Single(currentDebtIndex + 1) * 3;
	}

	// Token: 0x0600012D RID: 301 RVA: 0x0000AFD4 File Offset: 0x000091D4
	public static BigInteger DeadlineReward_CoinsGet()
	{
		if (GameplayData.Instance == null)
		{
			return 0;
		}
		return GameplayData.DeadlineReward_CoinsGet(GameplayData.DebtIndexGet());
	}

	// Token: 0x0600012E RID: 302 RVA: 0x0000AFEE File Offset: 0x000091EE
	public static long DeadlineReward_CloverTickets(int extraRounds)
	{
		return GameplayData.CloverTickets_BonusRoundsLeft_Get() * (long)extraRounds * (long)PowerupScript.EvilDealBonusMultiplier();
	}

	// Token: 0x0600012F RID: 303 RVA: 0x0000AFFF File Offset: 0x000091FF
	public static long DeadlineReward_CloverTickets_Extras()
	{
		return ((long)(PowerupScript.ModifiedPowerups_GetTicketsBonus() + PowerupScript.Hourglass_BonusTicketsGet(true)) + PowerupScript.CloverPotTicketsBonus(true) + (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_Tickets, true) ? 3L : 0L)) * (long)PowerupScript.EvilDealBonusMultiplier();
	}

	// Token: 0x06000130 RID: 304 RVA: 0x0000B02C File Offset: 0x0000922C
	private void _Debt_PrepareForSerialization()
	{
		this.debtIndex_ByteArray = this.debtIndex.ToByteArray();
		this.debtOutOfRangeMult_ByteArray = this.debtOutOfRangeMult.ToByteArray();
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0000B050 File Offset: 0x00009250
	private void _Debt_RestoreFromSerialization()
	{
		this.debtIndex = this.BigIntegerFromByteArray(this.debtIndex_ByteArray, 0);
		this.debtOutOfRangeMult = this.BigIntegerFromByteArray(this.debtOutOfRangeMult_ByteArray, 6);
	}

	// Token: 0x06000132 RID: 306 RVA: 0x0000B084 File Offset: 0x00009284
	public static int RoundDeadlineTrail_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.roundDeadlineTrail;
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0000B0A4 File Offset: 0x000092A4
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

	// Token: 0x06000134 RID: 308 RVA: 0x0000B0D4 File Offset: 0x000092D4
	public static void RoundDeadlineTrail_Increment()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.roundDeadlineTrail++;
	}

	// Token: 0x06000135 RID: 309 RVA: 0x0000B0FC File Offset: 0x000092FC
	public static int RoundsReallyPlayedGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.roundsReallyPlayed;
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0000B11C File Offset: 0x0000931C
	public static void RoundsReallyPlayedIncrement()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.roundsReallyPlayed++;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000B144 File Offset: 0x00009344
	public static int RoundDeadlineTrail_AtDeadlineBegin_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.roundDeadlineTrail_AtDeadlineBegin;
	}

	// Token: 0x06000138 RID: 312 RVA: 0x0000B164 File Offset: 0x00009364
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

	// Token: 0x06000139 RID: 313 RVA: 0x0000B192 File Offset: 0x00009392
	public static void RoundDeadlineTrail_AtDeadlineBegin_CheckpointSet()
	{
		GameplayData.RoundDeadlineTrail_AtDeadlineBegin_Set(GameplayData.RoundDeadlineTrail_Get());
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000B1A0 File Offset: 0x000093A0
	public static int RoundsLeftToDeadline()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance.roundOfDeadline - instance.roundDeadlineTrail;
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000B1C8 File Offset: 0x000093C8
	public static int RoundsOfDeadline_TotalGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance.roundOfDeadline - instance.roundDeadlineTrail_AtDeadlineBegin;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x0000B1F0 File Offset: 0x000093F0
	public static int RoundsOfDeadline_PlayedGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance.roundDeadlineTrail - instance.roundDeadlineTrail_AtDeadlineBegin;
	}

	// Token: 0x0600013D RID: 317 RVA: 0x0000B218 File Offset: 0x00009418
	public static int RoundOfDeadlineGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance.roundOfDeadline;
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0000B238 File Offset: 0x00009438
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

	// Token: 0x0600013F RID: 319 RVA: 0x0000B258 File Offset: 0x00009458
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

	// Token: 0x06000140 RID: 320 RVA: 0x0000B290 File Offset: 0x00009490
	public static void DeadlineRoundsIncrement()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.DeadlineRoundsIncrement_Set(instance.roundOfDeadline + GameplayData._GetDebtRoundDeadline_NextIncrement());
	}

	// Token: 0x06000141 RID: 321 RVA: 0x0000B2B8 File Offset: 0x000094B8
	public static void DeadlineRoundsIncrement_Set(int ammount)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.roundOfDeadline = ammount;
	}

	// Token: 0x06000142 RID: 322 RVA: 0x0000B2D6 File Offset: 0x000094D6
	public static bool AreWeOverTheDebtRange(BigInteger debtIndex)
	{
		return GameplayData.Instance == null || (GameplayData.debtsInRange == null || GameplayData.debtsInRange.Length == 0) || debtIndex >= (long)GameplayData.debtsInRange.Length;
	}

	// Token: 0x06000143 RID: 323 RVA: 0x0000B300 File Offset: 0x00009500
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

	// Token: 0x06000144 RID: 324 RVA: 0x0000B358 File Offset: 0x00009558
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
		return bigInteger;
	}

	// Token: 0x06000145 RID: 325 RVA: 0x0000B404 File Offset: 0x00009604
	public static BigInteger DebtGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return GameplayData.debtsInRange[0];
		}
		return GameplayData.DebtGetExt(instance.debtIndex);
	}

	// Token: 0x06000146 RID: 326 RVA: 0x0000B434 File Offset: 0x00009634
	public static BigInteger DebtIndexGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.debtIndex;
	}

	// Token: 0x06000147 RID: 327 RVA: 0x0000B458 File Offset: 0x00009658
	public static void DebtIndexSet(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.debtIndex = value;
	}

	// Token: 0x06000148 RID: 328 RVA: 0x0000B476 File Offset: 0x00009676
	public static void DebtIndexAdd(BigInteger value)
	{
		GameplayData.DebtIndexSet(GameplayData.DebtIndexGet() + value);
	}

	// Token: 0x06000149 RID: 329 RVA: 0x0000B488 File Offset: 0x00009688
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

	// Token: 0x0600014A RID: 330 RVA: 0x0000B4CC File Offset: 0x000096CC
	public static BigInteger DebtOutOfRangeMultiplier_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 6;
		}
		return instance.debtOutOfRangeMult;
	}

	// Token: 0x0600014B RID: 331 RVA: 0x0000B4F0 File Offset: 0x000096F0
	public static void DebtOutOfRangeMultiplier_Set(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.debtOutOfRangeMult = value;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x0000B510 File Offset: 0x00009710
	public static bool SkeletonIsCompletedGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.skeletonIsCompleted;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x0000B530 File Offset: 0x00009730
	public static void SkeletonIsCompletedSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.skeletonIsCompleted = true;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x0000B550 File Offset: 0x00009750
	public static int SpinsLeftGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.spinsLeft;
	}

	// Token: 0x0600014F RID: 335 RVA: 0x0000B570 File Offset: 0x00009770
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

	// Token: 0x06000150 RID: 336 RVA: 0x0000B5A0 File Offset: 0x000097A0
	public static void SpinsLeftAdd(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.SpinsLeftSet(instance.spinsLeft + n);
	}

	// Token: 0x06000151 RID: 337 RVA: 0x0000B5C4 File Offset: 0x000097C4
	public static void SpinConsume()
	{
		GameplayData.SpinsLeftAdd(-1);
	}

	// Token: 0x06000152 RID: 338 RVA: 0x0000B5CC File Offset: 0x000097CC
	public static int SpinsDoneInARun_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.spinsDoneInARun;
	}

	// Token: 0x06000153 RID: 339 RVA: 0x0000B5EC File Offset: 0x000097EC
	public static void SpinsDoneInARun_Increment()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.spinsDoneInARun++;
	}

	// Token: 0x06000154 RID: 340 RVA: 0x0000B614 File Offset: 0x00009814
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

	// Token: 0x06000155 RID: 341 RVA: 0x0000B668 File Offset: 0x00009868
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

	// Token: 0x06000156 RID: 342 RVA: 0x0000B698 File Offset: 0x00009898
	public static void ExtraSpinsAdd(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.ExtraSpinsSet(instance.extraSpins + n);
	}

	// Token: 0x06000157 RID: 343 RVA: 0x0000B6BC File Offset: 0x000098BC
	public static BigInteger SpinCostGet_Single()
	{
		return GameplayData.SpinCostGet_Single(GameplayData.DebtIndexGet());
	}

	// Token: 0x06000158 RID: 344 RVA: 0x0000B6C8 File Offset: 0x000098C8
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

	// Token: 0x06000159 RID: 345 RVA: 0x0000B710 File Offset: 0x00009910
	public static int GetHypotehticalMaxSpinsBuyable()
	{
		if (GameplayData.Instance == null)
		{
			return 0;
		}
		BigInteger bigInteger = GameplayData.CoinsGet();
		BigInteger bigInteger2 = GameplayData.SpinCostGet_Single();
		BigInteger bigInteger3 = bigInteger / bigInteger2;
		int num = GameplayData.MaxSpins_Get();
		if (bigInteger3 > (long)num)
		{
			bigInteger3 = num;
		}
		return bigInteger3.CastToInt();
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000B755 File Offset: 0x00009955
	public static int GetHypotehticalMidSpinsBuyable()
	{
		return Mathf.FloorToInt((float)(GameplayData.GetHypotehticalMaxSpinsBuyable() / 2));
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000B764 File Offset: 0x00009964
	public static BigInteger SpinCostMax_Get()
	{
		return GameplayData.GetHypotehticalMaxSpinsBuyable() * GameplayData.SpinCostGet_Single();
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000B77A File Offset: 0x0000997A
	public static BigInteger SpinCostMid_Get()
	{
		return GameplayData.GetHypotehticalMidSpinsBuyable() * GameplayData.SpinCostGet_Single();
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000B790 File Offset: 0x00009990
	public static void LastBet_IsSmallSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.lastBetIsSmall = true;
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0000B7B0 File Offset: 0x000099B0
	public static void LastBet_IsBigSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.lastBetIsSmall = false;
	}

	// Token: 0x0600015F RID: 351 RVA: 0x0000B7D0 File Offset: 0x000099D0
	public static bool LastBet_IsSmallGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance == null || instance.lastBetIsSmall;
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000B7EE File Offset: 0x000099EE
	public static bool LastBet_IsBigGet()
	{
		return !GameplayData.LastBet_IsSmallGet();
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0000B7F8 File Offset: 0x000099F8
	public static int MaxSpins_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.maxSpins;
	}

	// Token: 0x06000162 RID: 354 RVA: 0x0000B818 File Offset: 0x00009A18
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

	// Token: 0x06000163 RID: 355 RVA: 0x0000B848 File Offset: 0x00009A48
	public static void MaxSpins_Add(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.MaxSpins_Set(instance.maxSpins + n);
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0000B86C File Offset: 0x00009A6C
	public static long SmallBetPickCount()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._smallBetsPickedCounter;
	}

	// Token: 0x06000165 RID: 357 RVA: 0x0000B88C File Offset: 0x00009A8C
	public static long BigBetPickCount()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._bigBetsPickedCounter;
	}

	// Token: 0x06000166 RID: 358 RVA: 0x0000B8AC File Offset: 0x00009AAC
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

	// Token: 0x06000167 RID: 359 RVA: 0x0000B8E4 File Offset: 0x00009AE4
	public static int SpinsWithoutReward_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.spinsWithoutReward;
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0000B904 File Offset: 0x00009B04
	public static void SpinsWithoutReward_Increase()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.spinsWithoutReward++;
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000B92C File Offset: 0x00009B2C
	public static void SpinsWithoutReward_Reset()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.spinsWithoutReward = 0;
	}

	// Token: 0x0600016A RID: 362 RVA: 0x0000B94C File Offset: 0x00009B4C
	public static int ConsecutiveSpinsWithout5PlusPatterns_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.spinsWithout5PlusPatterns;
	}

	// Token: 0x0600016B RID: 363 RVA: 0x0000B96C File Offset: 0x00009B6C
	public static void ConsecutiveSpinsWithout5PlusPatterns_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.spinsWithout5PlusPatterns = n;
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000B98C File Offset: 0x00009B8C
	public static int ConsecutiveSpinsWithDiamondTreasureOrSeven_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.consecutiveSpinsWithDiamTreasSevens;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0000B9AC File Offset: 0x00009BAC
	public static void ConsecutiveSpinsWithDiamondTreasureOrSeven_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.consecutiveSpinsWithDiamTreasSevens = n;
	}

	// (get) Token: 0x0600016E RID: 366 RVA: 0x0000B9CC File Offset: 0x00009BCC
	// (set) Token: 0x0600016F RID: 367 RVA: 0x0000B9EC File Offset: 0x00009BEC
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

	// Token: 0x06000170 RID: 368 RVA: 0x0000BA0C File Offset: 0x00009C0C
	public static long SpinsWithAtLeast1Jackpot_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._spinsWithAtleast1Jackpot;
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000BA2C File Offset: 0x00009C2C
	public static void SpinsWithAtLeast1Jackpot_Set(long n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._spinsWithAtleast1Jackpot = n;
	}

	// (get) Token: 0x06000172 RID: 370 RVA: 0x0000BA4C File Offset: 0x00009C4C
	// (set) Token: 0x06000173 RID: 371 RVA: 0x0000BA6C File Offset: 0x00009C6C
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

	// (get) Token: 0x06000174 RID: 372 RVA: 0x0000BA8C File Offset: 0x00009C8C
	// (set) Token: 0x06000175 RID: 373 RVA: 0x0000BAAC File Offset: 0x00009CAC
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

	// Token: 0x06000176 RID: 374 RVA: 0x0000BACA File Offset: 0x00009CCA
	public static float LuckGet()
	{
		if (GameplayData.Instance == null)
		{
			return 0f;
		}
		return 0f + GameplayData.ExtraLuck_GetTotal() + PowerupScript.CrystalLuckIncreaseGet(true);
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0000BAEC File Offset: 0x00009CEC
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

	// Token: 0x06000178 RID: 376 RVA: 0x0000BB3C File Offset: 0x00009D3C
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

	// Token: 0x06000179 RID: 377 RVA: 0x0000BBA0 File Offset: 0x00009DA0
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

	// Token: 0x0600017A RID: 378 RVA: 0x0000BC20 File Offset: 0x00009E20
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

	// Token: 0x0600017B RID: 379 RVA: 0x0000BC98 File Offset: 0x00009E98
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

	// Token: 0x0600017C RID: 380 RVA: 0x0000BCF8 File Offset: 0x00009EF8
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

	// Token: 0x0600017D RID: 381 RVA: 0x0000BDDC File Offset: 0x00009FDC
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

	// Token: 0x0600017E RID: 382 RVA: 0x0000BE5C File Offset: 0x0000A05C
	public static float PowerupLuckGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1f;
		}
		return instance.powerupLuck;
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0000BE80 File Offset: 0x0000A080
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

	// Token: 0x06000180 RID: 384 RVA: 0x0000BEB8 File Offset: 0x0000A0B8
	public static void PowerupLuckAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.PowerupLuckSet(instance.powerupLuck + value);
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0000BEDC File Offset: 0x0000A0DC
	public static void PowerupLuckReset()
	{
		GameplayData.PowerupLuckSet(1f);
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0000BEE8 File Offset: 0x0000A0E8
	public static float ActivationLuckGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1f;
		}
		return instance.activationLuck + PowerupScript.HorseShoesLuckGet() + PowerupScript.GoldenHorseShoe_RandomActivationChanceBonusGet(true);
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0000BF18 File Offset: 0x0000A118
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

	// Token: 0x06000184 RID: 388 RVA: 0x0000BF50 File Offset: 0x0000A150
	public static void ActivationLuckAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.ActivationLuckSet(instance.activationLuck + value);
	}

	// Token: 0x06000185 RID: 389 RVA: 0x0000BF74 File Offset: 0x0000A174
	public static void ActivationLuckReset()
	{
		GameplayData.ActivationLuckSet(1f);
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0000BF80 File Offset: 0x0000A180
	public static float StoreLuckGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1f;
		}
		return instance.storeLuck;
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0000BFA4 File Offset: 0x0000A1A4
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

	// Token: 0x06000188 RID: 392 RVA: 0x0000BFDC File Offset: 0x0000A1DC
	public static void StoreLuckAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.StoreLuckSet(instance.storeLuck + value);
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0000C000 File Offset: 0x0000A200
	public static void StoreLuckReset()
	{
		GameplayData.StoreLuckSet(1f);
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000C00C File Offset: 0x0000A20C
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

	// Token: 0x0600018B RID: 395 RVA: 0x0000C10C File Offset: 0x0000A30C
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

	// Token: 0x0600018C RID: 396 RVA: 0x0000C1F0 File Offset: 0x0000A3F0
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

	// Token: 0x0600018D RID: 397 RVA: 0x0000C2D0 File Offset: 0x0000A4D0
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

	// Token: 0x0600018E RID: 398 RVA: 0x0000C304 File Offset: 0x0000A504
	public static void SymbolsAvilable_Remove(SymbolScript.Kind kind)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.symbolsAvailable.Remove(kind);
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0000C328 File Offset: 0x0000A528
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
				float num2 = (float)GameplayData.Symbol_CoinsValue_GetBasic(instance.symbolsAvailable[i]);
				float num3 = (float)GameplayData.Symbol_CoinsValue_GetBasic(instance.symbolsAvailable[num]);
				if (num2 > num3)
				{
					SymbolScript.Kind kind = instance.symbolsAvailable[i];
					instance.symbolsAvailable[i] = instance.symbolsAvailable[num];
					instance.symbolsAvailable[num] = kind;
				}
				num++;
			}
		}
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0000C3E0 File Offset: 0x0000A5E0
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

	// Token: 0x06000191 RID: 401 RVA: 0x0000C5D8 File Offset: 0x0000A7D8
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

	// Token: 0x06000192 RID: 402 RVA: 0x0000C614 File Offset: 0x0000A814
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

	// Token: 0x06000193 RID: 403 RVA: 0x0000C680 File Offset: 0x0000A880
	public static BigInteger Symbol_CoinsValueExtra_Get(SymbolScript.Kind kind)
	{
		GameplayData.SymbolData symbolData = GameplayData._SymbolDataFind(kind);
		if (symbolData == null)
		{
			return 0;
		}
		return symbolData.extraValue;
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0000C6A4 File Offset: 0x0000A8A4
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

	// Token: 0x06000195 RID: 405 RVA: 0x0000C6E8 File Offset: 0x0000A8E8
	public static void Symbol_CoinsValueExtra_Add(SymbolScript.Kind kind, BigInteger value)
	{
		GameplayData.Symbol_CoinsValueExtra_Set(kind, GameplayData.Symbol_CoinsValueExtra_Get(kind) + value);
	}

	// Token: 0x06000196 RID: 406 RVA: 0x0000C6FC File Offset: 0x0000A8FC
	public static void Symbol_CoinsValueExtra_Reset(SymbolScript.Kind kind)
	{
		GameplayData.Symbol_CoinsValueExtra_Set(kind, 0);
	}

	// Token: 0x06000197 RID: 407 RVA: 0x0000C70C File Offset: 0x0000A90C
	public static BigInteger Symbol_CoinsOverallValue_Get(SymbolScript.Kind kind)
	{
		BigInteger bigInteger = GameplayData.Symbol_CoinsValue_GetBasic(kind) + GameplayData.Symbol_CoinsValueExtra_Get(kind);
		bigInteger += PowerupScript.PoopBeetle_ExtraCoinsForAllSymbolsGet(true, kind);
		bigInteger += PowerupScript.Calendar_GetSymbolBonus(true, kind);
		bigInteger += PowerupScript.ShroomsBonusGet(kind);
		bigInteger += PowerupScript.RingBell_BonusGet(kind);
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

	// Token: 0x06000198 RID: 408 RVA: 0x0000C7DC File Offset: 0x0000A9DC
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

	// Token: 0x06000199 RID: 409 RVA: 0x0000C970 File Offset: 0x0000AB70
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

	// Token: 0x0600019A RID: 410 RVA: 0x0000C9C0 File Offset: 0x0000ABC0
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

	// Token: 0x0600019B RID: 411 RVA: 0x0000C9F8 File Offset: 0x0000ABF8
	public static void Symbol_Chance_Add(SymbolScript.Kind kind, float value)
	{
		GameplayData.SymbolData symbolData = GameplayData._SymbolDataFind(kind);
		if (symbolData == null)
		{
			return;
		}
		GameplayData.Symbol_Chance_Set(kind, symbolData.spawnChance + value);
	}

	// Token: 0x0600019C RID: 412 RVA: 0x0000CA20 File Offset: 0x0000AC20
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

	// Token: 0x0600019D RID: 413 RVA: 0x0000CAB0 File Offset: 0x0000ACB0
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

	// Token: 0x0600019E RID: 414 RVA: 0x0000CB60 File Offset: 0x0000AD60
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

	// Token: 0x0600019F RID: 415 RVA: 0x0000CD3B File Offset: 0x0000AF3B
	public static List<SymbolScript.Kind> SymbolsValueList_Get()
	{
		return GameplayData._symbolsOrderedByHighestValueToLowest;
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x0000CD42 File Offset: 0x0000AF42
	public static List<SymbolScript.Kind> MostValuableSymbols_GetList()
	{
		return GameplayData._mostValuableSymbols;
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x0000CD49 File Offset: 0x0000AF49
	public static List<SymbolScript.Kind> LeastValuableSymbols_GetList()
	{
		return GameplayData._leastValuableSymbols;
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x0000CD50 File Offset: 0x0000AF50
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

	// Token: 0x060001A3 RID: 419 RVA: 0x0000CF28 File Offset: 0x0000B128
	public static List<SymbolScript.Kind> SymbolsChanceList_Get()
	{
		return GameplayData._symbolsOrderedByHighestChanceToLowest;
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x0000CF2F File Offset: 0x0000B12F
	public static List<SymbolScript.Kind> MostProbableSymbols_GetList()
	{
		return GameplayData._mostProbableSymbols;
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x0000CF36 File Offset: 0x0000B136
	public static List<SymbolScript.Kind> LeastProbableSymbols_GetList()
	{
		return GameplayData._leastProbableSymbols;
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x0000CF40 File Offset: 0x0000B140
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

	// Token: 0x060001A7 RID: 423 RVA: 0x0000D51F File Offset: 0x0000B71F
	public static float Symbol_ModifierChance_GetAsPercentage(SymbolScript.Kind symbolKind, SymbolScript.Modifier modifier)
	{
		return GameplayData.Symbol_ModifierChance_Get(symbolKind, modifier) * 100f;
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x0000D530 File Offset: 0x0000B730
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

	// Token: 0x060001A9 RID: 425 RVA: 0x0000D5CC File Offset: 0x0000B7CC
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

	// Token: 0x060001AA RID: 426 RVA: 0x0000D660 File Offset: 0x0000B860
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

	// Token: 0x060001AB RID: 427 RVA: 0x0000D75C File Offset: 0x0000B95C
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
			bigInteger += PowerupScript.ModifiedPowerups_GetCount(PowerupScript.Modifier.symbolMultiplier) + PowerupScript.TarotDeckRewardGet(true) + PowerupScript.PentacleBonusGet(true) + PowerupScript.CloverPetSymbolsMultiplierBonus(true) + PowerupScript.VoiceMail_MultiplierBonusGet(true) + PowerupScript.Garbage_MultiplierBonusGet(true) + PowerupScript.AllIn_MultiplierBonusGet(true) + PowerupScript.DarkLotus_MultiplierBonus_Get(true) + PowerupScript.ShoppingCart_MultiplierBonusGet(true) + (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_SymbMult, true) ? 5 : 0);
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

	// Token: 0x060001AC RID: 428 RVA: 0x0000D878 File Offset: 0x0000BA78
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

	// Token: 0x060001AD RID: 429 RVA: 0x0000D8B4 File Offset: 0x0000BAB4
	public static void AllSymbolsMultiplierAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.AllSymbolsMultiplierSet(instance.allSymbolsMultiplier + value);
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0000D8DC File Offset: 0x0000BADC
	public static void allSymbolsMultiplierReset()
	{
		GameplayData.AllSymbolsMultiplierSet(1);
	}

	// Token: 0x060001AF RID: 431 RVA: 0x0000D8EC File Offset: 0x0000BAEC
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

	// Token: 0x060001B0 RID: 432 RVA: 0x0000D9BC File Offset: 0x0000BBBC
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

	// Token: 0x060001B1 RID: 433 RVA: 0x0000DA3C File Offset: 0x0000BC3C
	public static List<PatternScript.Kind> PatternsAvailable_GetAll()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return new List<PatternScript.Kind>();
		}
		return instance.patternsAvailable;
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x0000DA60 File Offset: 0x0000BC60
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

	// Token: 0x060001B3 RID: 435 RVA: 0x0000DA94 File Offset: 0x0000BC94
	public static void PatternsAvailable_Remove(PatternScript.Kind kind)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.patternsAvailable.Remove(kind);
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x0000DAB8 File Offset: 0x0000BCB8
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
				double num2 = GameplayData.Pattern_Value_GetBasic(instance.patternsAvailable[i]);
				double num3 = GameplayData.Pattern_Value_GetBasic(instance.patternsAvailable[num]);
				if (num2 > num3)
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

	// Token: 0x060001B5 RID: 437 RVA: 0x0000DB98 File Offset: 0x0000BD98
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

	// Token: 0x060001B6 RID: 438 RVA: 0x0000DCE4 File Offset: 0x0000BEE4
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

	// Token: 0x060001B7 RID: 439 RVA: 0x0000DED8 File Offset: 0x0000C0D8
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

	// Token: 0x060001B8 RID: 440 RVA: 0x0000DF14 File Offset: 0x0000C114
	public static double Pattern_ValueExtra_Get(PatternScript.Kind kind)
	{
		GameplayData.PatternData patternData = GameplayData._PatternDataFind(kind);
		if (patternData == null)
		{
			return 0.0;
		}
		return patternData.extraValue;
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x0000DF3C File Offset: 0x0000C13C
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

	// Token: 0x060001BA RID: 442 RVA: 0x0000DF7C File Offset: 0x0000C17C
	public static void Pattern_ValueExtra_Add(PatternScript.Kind kind, double value)
	{
		GameplayData.PatternData patternData = GameplayData._PatternDataFind(kind);
		if (patternData == null)
		{
			return;
		}
		GameplayData.Pattern_ValueExtra_Set(kind, patternData.extraValue + value);
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0000DFA2 File Offset: 0x0000C1A2
	public static void Pattern_ValueExtra_Reset(PatternScript.Kind kind)
	{
		GameplayData.Pattern_ValueExtra_Set(kind, 0.0);
	}

	// Token: 0x060001BC RID: 444 RVA: 0x0000DFB4 File Offset: 0x0000C1B4
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
		return num2;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x0000E02C File Offset: 0x0000C22C
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

	// Token: 0x060001BE RID: 446 RVA: 0x0000E1FA File Offset: 0x0000C3FA
	public static List<PatternScript.Kind> PatternsValueList_Get()
	{
		return GameplayData._patternsOrderedByHighestValueToLowest;
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0000E201 File Offset: 0x0000C401
	public static List<PatternScript.Kind> MostValuablePatterns_GetList()
	{
		return GameplayData._mostValuablePatterns;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0000E208 File Offset: 0x0000C408
	public static List<PatternScript.Kind> LeastValuablePatterns_GetList()
	{
		return GameplayData._leastValuablePatterns;
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0000E210 File Offset: 0x0000C410
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
			bigInteger += (long)(PowerupScript.ModifiedPowerups_GetCount(PowerupScript.Modifier.patternMultiplier) + PowerupScript.Necronomicon_AdditionalPatternsMultiplierGet()) + PowerupScript.Cross_PatternsMultiplierBonus_Get(true) + (long)PowerupScript.TheCollector_MultiplierGet(true) + (long)PowerupScript.ChastityBelt_MultiplierBonusGet(true) + PowerupScript.Wallet_PatternsMultiplierBonus(true) + (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_PatternsMult, true) ? 2L : 0L);
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

	// Token: 0x060001C2 RID: 450 RVA: 0x0000E2D4 File Offset: 0x0000C4D4
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

	// Token: 0x060001C3 RID: 451 RVA: 0x0000E310 File Offset: 0x0000C510
	public static void AllPatternsMultiplierAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.AllPatternsMultiplierSet(instance.allPatternsMultiplier + value);
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000E338 File Offset: 0x0000C538
	public static void AllPatternsMultiplierReset()
	{
		GameplayData.AllPatternsMultiplierSet(1);
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0000E345 File Offset: 0x0000C545
	public static BigInteger SixSixSix_GetMinimumDebtIndex()
	{
		return 2;
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x0000E34D File Offset: 0x0000C54D
	public static BigInteger SuperSixSixSix_GetMinimumDebtIndex()
	{
		if (RewardBoxScript.GetRewardKind() != RewardBoxScript.RewardKind.DoorKey)
		{
			return 666;
		}
		return 6;
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x0000E368 File Offset: 0x0000C568
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
		bool flag = GameplayData.Powerup_PossessedPhone_TriggersCount_Get() > 0;
		bool flag2 = GameplayData.SixSixSix_SuppressedSpinsGet() > 0;
		if (updateValues)
		{
			GameplayData.Powerup_PossessedPhone_TriggersCount_Set(GameplayData.Powerup_PossessedPhone_TriggersCount_Get() - 1);
			GameplayData.SixSixSix_SuppressedSpinsSet(GameplayData.SixSixSix_SuppressedSpinsGet() - 1);
		}
		if (flag)
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

	// Token: 0x060001C8 RID: 456 RVA: 0x0000E450 File Offset: 0x0000C650
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

	// Token: 0x060001C9 RID: 457 RVA: 0x0000E509 File Offset: 0x0000C709
	public static float SixSixSix_ChanceGet_AsPercentage(bool considerMaximum)
	{
		return GameplayData.SixSixSix_ChanceGet(considerMaximum) * 100f;
	}

	// Token: 0x060001CA RID: 458 RVA: 0x0000E517 File Offset: 0x0000C717
	public static void OBSOLETE_SixSixSix_IncrementChance()
	{
		GameplayData.OBSOLETE_SixSixSix_IncrementChanceManual(0.0015f);
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0000E524 File Offset: 0x0000C724
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

	// Token: 0x060001CC RID: 460 RVA: 0x0000E57C File Offset: 0x0000C77C
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

	// Token: 0x060001CD RID: 461 RVA: 0x0000E5B0 File Offset: 0x0000C7B0
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

	// Token: 0x060001CE RID: 462 RVA: 0x0000E5E8 File Offset: 0x0000C7E8
	public static void SixSixSix_SuppressedSpinsSet(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._666SuppressedSpinsLeft = Mathf.Max(0, n);
	}

	// Token: 0x060001CF RID: 463 RVA: 0x0000E60C File Offset: 0x0000C80C
	public static int SixSixSix_SuppressedSpinsGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._666SuppressedSpinsLeft;
	}

	// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000E62A File Offset: 0x0000C82A
	// (set) Token: 0x060001D1 RID: 465 RVA: 0x0000E640 File Offset: 0x0000C840
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

	// Token: 0x060001D2 RID: 466 RVA: 0x0000E660 File Offset: 0x0000C860
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

	// Token: 0x060001D3 RID: 467 RVA: 0x0000E6DA File Offset: 0x0000C8DA
	private void _PowerupsData_PrepareForSerialization()
	{
		GameplayData._EnsurePowerupDataArray(this);
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x0000E6E2 File Offset: 0x0000C8E2
	private void _PowerupsData_RestoreFromSerialization()
	{
		GameplayData._EnsurePowerupDataArray(this);
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x0000E6EC File Offset: 0x0000C8EC
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

	// Token: 0x060001D6 RID: 470 RVA: 0x0000E8EC File Offset: 0x0000CAEC
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

	// Token: 0x060001D7 RID: 471 RVA: 0x0000E928 File Offset: 0x0000CB28
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

	// Token: 0x060001D8 RID: 472 RVA: 0x0000E94C File Offset: 0x0000CB4C
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

	// Token: 0x060001D9 RID: 473 RVA: 0x0000E990 File Offset: 0x0000CB90
	public Rng PowerupRngGet(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return null;
		}
		return powerupData.charmSpecificRng;
	}

	// Token: 0x060001DA RID: 474 RVA: 0x0000E9B0 File Offset: 0x0000CBB0
	public static int Powerup_BoughtTimes_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.boughtTimes;
	}

	// Token: 0x060001DB RID: 475 RVA: 0x0000E9D0 File Offset: 0x0000CBD0
	public static void Powerup_BoughtTimes_Increase(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return;
		}
		powerupData.boughtTimes++;
	}

	// Token: 0x060001DC RID: 476 RVA: 0x0000E9F8 File Offset: 0x0000CBF8
	public static PowerupScript.Modifier Powerup_Modifier_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return PowerupScript.Modifier.none;
		}
		return powerupData.modifier;
	}

	// Token: 0x060001DD RID: 477 RVA: 0x0000EA18 File Offset: 0x0000CC18
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

	// Token: 0x060001DE RID: 478 RVA: 0x0000EA64 File Offset: 0x0000CC64
	public static int Powerup_ButtonBurnedOut_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.buttonBurnOutCounter;
	}

	// Token: 0x060001DF RID: 479 RVA: 0x0000EA84 File Offset: 0x0000CC84
	public static void Powerup_ButtonBurnedOut_Set(PowerupScript.Identifier identifier, int n)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return;
		}
		powerupData.buttonBurnOutCounter = n;
	}

	// Token: 0x060001E0 RID: 480 RVA: 0x0000EAA3 File Offset: 0x0000CCA3
	public static void Powerup_ButtonBurnedOut_Increaase(PowerupScript.Identifier identifier)
	{
		GameplayData.Powerup_ButtonBurnedOut_Set(identifier, GameplayData.Powerup_ButtonBurnedOut_Get(identifier) + 1);
	}

	// Token: 0x060001E1 RID: 481 RVA: 0x0000EAB4 File Offset: 0x0000CCB4
	public static int Powerup_ButtonChargesUsed_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.buttonChargesCounter;
	}

	// Token: 0x060001E2 RID: 482 RVA: 0x0000EAD4 File Offset: 0x0000CCD4
	public static int Powerup_ButtonChargesUsed_GetAbsolute(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.buttonChargesCounter_Absolute;
	}

	// Token: 0x060001E3 RID: 483 RVA: 0x0000EAF4 File Offset: 0x0000CCF4
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

	// Token: 0x060001E4 RID: 484 RVA: 0x0000EB48 File Offset: 0x0000CD48
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

	// Token: 0x060001E5 RID: 485 RVA: 0x0000EBD4 File Offset: 0x0000CDD4
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

	// Token: 0x060001E6 RID: 486 RVA: 0x0000EC39 File Offset: 0x0000CE39
	public static bool Powerup_ButtonChargesUsed_RestoreChargesN(PowerupScript.Identifier identifier, int value, bool triggerRechargeAnimation)
	{
		return GameplayData.Powerup_ButtonChargesUsed_RestoreChargesN_Ext(identifier, value, triggerRechargeAnimation, true);
	}

	// Token: 0x060001E7 RID: 487 RVA: 0x0000EC44 File Offset: 0x0000CE44
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

	// Token: 0x060001E8 RID: 488 RVA: 0x0000ECD8 File Offset: 0x0000CED8
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

	// Token: 0x060001E9 RID: 489 RVA: 0x0000ED00 File Offset: 0x0000CF00
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

	// Token: 0x060001EA RID: 490 RVA: 0x0000ED40 File Offset: 0x0000CF40
	public static int Powerup_ResellBonus_Get(PowerupScript.Identifier identifier)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return 0;
		}
		return powerupData.resellBonus;
	}

	// Token: 0x060001EB RID: 491 RVA: 0x0000ED60 File Offset: 0x0000CF60
	public static void Powerup_ResellBonus_Set(PowerupScript.Identifier identifier, int n)
	{
		GameplayData.PowerupData powerupData = GameplayData._PowerupDataFind(identifier);
		if (powerupData == null)
		{
			return;
		}
		powerupData.resellBonus = Mathf.Max(0, n);
	}

	// (get) Token: 0x060001EC RID: 492 RVA: 0x0000ED88 File Offset: 0x0000CF88
	// (set) Token: 0x060001ED RID: 493 RVA: 0x0000EDA8 File Offset: 0x0000CFA8
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

	// (get) Token: 0x060001EE RID: 494 RVA: 0x0000EDC8 File Offset: 0x0000CFC8
	// (set) Token: 0x060001EF RID: 495 RVA: 0x0000EDE8 File Offset: 0x0000CFE8
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

	// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000EE08 File Offset: 0x0000D008
	// (set) Token: 0x060001F1 RID: 497 RVA: 0x0000EE28 File Offset: 0x0000D028
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

	// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000EE48 File Offset: 0x0000D048
	// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000EE68 File Offset: 0x0000D068
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

	// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000EE88 File Offset: 0x0000D088
	// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000EEA8 File Offset: 0x0000D0A8
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

	// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000EEC8 File Offset: 0x0000D0C8
	// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000EEE8 File Offset: 0x0000D0E8
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

	// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000EF08 File Offset: 0x0000D108
	// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000EF28 File Offset: 0x0000D128
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

	// (get) Token: 0x060001FA RID: 506 RVA: 0x0000EF48 File Offset: 0x0000D148
	// (set) Token: 0x060001FB RID: 507 RVA: 0x0000EF68 File Offset: 0x0000D168
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

	// (get) Token: 0x060001FC RID: 508 RVA: 0x0000EF88 File Offset: 0x0000D188
	// (set) Token: 0x060001FD RID: 509 RVA: 0x0000EFA8 File Offset: 0x0000D1A8
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

	// (get) Token: 0x060001FE RID: 510 RVA: 0x0000EFC8 File Offset: 0x0000D1C8
	// (set) Token: 0x060001FF RID: 511 RVA: 0x0000EFE8 File Offset: 0x0000D1E8
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

	// (get) Token: 0x06000200 RID: 512 RVA: 0x0000F008 File Offset: 0x0000D208
	// (set) Token: 0x06000201 RID: 513 RVA: 0x0000F028 File Offset: 0x0000D228
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

	// (get) Token: 0x06000202 RID: 514 RVA: 0x0000F048 File Offset: 0x0000D248
	// (set) Token: 0x06000203 RID: 515 RVA: 0x0000F068 File Offset: 0x0000D268
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

	// Token: 0x06000204 RID: 516 RVA: 0x0000F088 File Offset: 0x0000D288
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

	// Token: 0x06000205 RID: 517 RVA: 0x0000F120 File Offset: 0x0000D320
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

	// Token: 0x06000206 RID: 518 RVA: 0x0000F1CC File Offset: 0x0000D3CC
	public static int Powerup_Hourglass_DeadlinesLeftGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupHourglass_DeadlinesLeft;
	}

	// Token: 0x06000207 RID: 519 RVA: 0x0000F1EC File Offset: 0x0000D3EC
	public static void Powerup_Hourglass_DeadlinesLeftSet(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupHourglass_DeadlinesLeft = value;
		if (instance._powerupHourglass_DeadlinesLeft < 0)
		{
			instance._powerupHourglass_DeadlinesLeft = 0;
		}
	}

	// Token: 0x06000208 RID: 520 RVA: 0x0000F21C File Offset: 0x0000D41C
	public static void Powerup_Hourglass_DeadlinesLeftAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_Hourglass_DeadlinesLeftSet(instance._powerupHourglass_DeadlinesLeft + value);
	}

	// Token: 0x06000209 RID: 521 RVA: 0x0000F240 File Offset: 0x0000D440
	public static void Powerup_Hourglass_DeadlinesLeftReset()
	{
		GameplayData.Powerup_Hourglass_DeadlinesLeftSet(3);
	}

	// Token: 0x0600020A RID: 522 RVA: 0x0000F248 File Offset: 0x0000D448
	public static int Powerup_FruitsBasket_RoundsLeftGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupFruitsBasket_RoundsLeft;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0000F268 File Offset: 0x0000D468
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

	// Token: 0x0600020C RID: 524 RVA: 0x0000F298 File Offset: 0x0000D498
	public static void Powerup_FruitBasket_RoundsLeftReset()
	{
		GameplayData.Powerup_FruitsBasket_RoundsLeftSet(7);
	}

	// Token: 0x0600020D RID: 525 RVA: 0x0000F2A0 File Offset: 0x0000D4A0
	public static BigInteger Powerup_TarotDeck_RewardGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupTarotDeck_Reward;
	}

	// Token: 0x0600020E RID: 526 RVA: 0x0000F2C4 File Offset: 0x0000D4C4
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

	// Token: 0x0600020F RID: 527 RVA: 0x0000F300 File Offset: 0x0000D500
	public static void Powerup_TarotDeck_RewardAdd(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_TarotDeck_RewardSet(instance._powerupTarotDeck_Reward + value);
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0000F328 File Offset: 0x0000D528
	public static BigInteger Powerup_PoopBeetle_SymbolsIncreaseN_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupPoopBeetle_SymbolsIncreaserMult;
	}

	// Token: 0x06000211 RID: 529 RVA: 0x0000F34C File Offset: 0x0000D54C
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

	// Token: 0x06000212 RID: 530 RVA: 0x0000F388 File Offset: 0x0000D588
	public static void Powerup_PoopBeetle_SymbolsIncreaseN_Add(BigInteger value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_PoopBeetle_SymbolsIncreaseN_Set(instance._powerupPoopBeetle_SymbolsIncreaserMult + value);
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000F3B0 File Offset: 0x0000D5B0
	public static int Powerup_GrandmasPurse_ExtraInterestGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupGrandmasPurse_ExtraInterest;
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0000F3D0 File Offset: 0x0000D5D0
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

	// Token: 0x06000215 RID: 533 RVA: 0x0000F400 File Offset: 0x0000D600
	public static void Powerup_GrandmasPurse_ExtraInterestAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_GrandmasPurse_ExtraInterestSet(instance._powerupGrandmasPurse_ExtraInterest + value);
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0000F424 File Offset: 0x0000D624
	public static void Powerup_GrandmasPurse_ExtraInterestReset()
	{
		GameplayData.Powerup_GrandmasPurse_ExtraInterestSet(15);
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0000F430 File Offset: 0x0000D630
	public static int Powerup_OneTrickPony_TargetSpinIndexGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return -1;
		}
		return instance._powerupOneTrickPony_TargetSpinsLeftIndex;
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0000F450 File Offset: 0x0000D650
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

	// Token: 0x06000219 RID: 537 RVA: 0x0000F480 File Offset: 0x0000D680
	public static int Powerup_Pentacle_TriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupPentacleTriggeredTimes;
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0000F4A0 File Offset: 0x0000D6A0
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

	// Token: 0x0600021B RID: 539 RVA: 0x0000F4D0 File Offset: 0x0000D6D0
	public static void Powerup_Pentacle_TriggeredTimesAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_Pentacle_TriggeredTimesSet(instance._powerupPentacleTriggeredTimes + value);
	}

	// Token: 0x0600021C RID: 540 RVA: 0x0000F4F4 File Offset: 0x0000D6F4
	public static BigInteger Powerup_Calendar_SymbolsIncreaseN_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupCalendar_SymbolsIncreaserMult;
	}

	// Token: 0x0600021D RID: 541 RVA: 0x0000F518 File Offset: 0x0000D718
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

	// Token: 0x0600021E RID: 542 RVA: 0x0000F554 File Offset: 0x0000D754
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

	// Token: 0x0600021F RID: 543 RVA: 0x0000F5D8 File Offset: 0x0000D7D8
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

	// Token: 0x06000220 RID: 544 RVA: 0x0000F690 File Offset: 0x0000D890
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

	// Token: 0x06000221 RID: 545 RVA: 0x0000F734 File Offset: 0x0000D934
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

	// Token: 0x06000222 RID: 546 RVA: 0x0000F850 File Offset: 0x0000DA50
	public static int Powerup_GoldenHorseShoe_SpinsLeftGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupGoldenHorseShoe_SpinsLeft;
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000F870 File Offset: 0x0000DA70
	public static void Powerup_GoldenHorseShoe_SpinsLeftSet(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupGoldenHorseShoe_SpinsLeft = Mathf.Max(0, n);
	}

	// Token: 0x06000224 RID: 548 RVA: 0x0000F894 File Offset: 0x0000DA94
	public static int Powerup_AncientCoin_SpinsLeftGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupAncientCoin_SpinsLeft;
	}

	// Token: 0x06000225 RID: 549 RVA: 0x0000F8B4 File Offset: 0x0000DAB4
	public static void Powerup_AncientCoin_SpinsLeftSet(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupAncientCoin_SpinsLeft = Mathf.Max(0, n);
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0000F8D8 File Offset: 0x0000DAD8
	public static int Powerup_ChannelerOfFortune_ActivationsCounterGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupChannelerOfFortunes_ActivationsCounter;
	}

	// Token: 0x06000227 RID: 551 RVA: 0x0000F8F8 File Offset: 0x0000DAF8
	public static void Powerup_ChannelerOfFortune_ActivationsCounterSet(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupChannelerOfFortunes_ActivationsCounter = n;
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0000F916 File Offset: 0x0000DB16
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

	// Token: 0x06000229 RID: 553 RVA: 0x0000F946 File Offset: 0x0000DB46
	private void Powerup_Pareidolia_SerializationPrepare()
	{
		this.PowerupPareidoliaArrayEnsure();
	}

	// Token: 0x0600022A RID: 554 RVA: 0x0000F94E File Offset: 0x0000DB4E
	private void Powerup_Pareidolia_SerializationRestore()
	{
		this.PowerupPareidoliaArrayEnsure();
	}

	// Token: 0x0600022B RID: 555 RVA: 0x0000F958 File Offset: 0x0000DB58
	public static double Powerup_PareidoliaMultiplierBonus_Get(PatternScript.Kind kind)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0.0;
		}
		return instance._powerupPareidolia_PatternBonuses[(int)kind];
	}

	// Token: 0x0600022C RID: 556 RVA: 0x0000F980 File Offset: 0x0000DB80
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

	// Token: 0x0600022D RID: 557 RVA: 0x0000F9B8 File Offset: 0x0000DBB8
	public static long Powerup_RingBell_Bonus_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupRingBell_BonusCounter;
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000F9D8 File Offset: 0x0000DBD8
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

	// Token: 0x0600022F RID: 559 RVA: 0x0000FA00 File Offset: 0x0000DC00
	public static long Powerup_ConsolationPrize_Bonus_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupConsolationPrize_BonusCounter;
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0000FA20 File Offset: 0x0000DC20
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

	// Token: 0x06000231 RID: 561 RVA: 0x0000FA48 File Offset: 0x0000DC48
	public static int Powerup_StepsCounter_TriggersCounter_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupStepsCounter_TriggersCounter;
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0000FA68 File Offset: 0x0000DC68
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

	// Token: 0x06000233 RID: 563 RVA: 0x0000FA90 File Offset: 0x0000DC90
	public static int Powerup_DieselLocomotive_Bonus_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupDieselLocomotiveBonus;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0000FAB0 File Offset: 0x0000DCB0
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

	// Token: 0x06000235 RID: 565 RVA: 0x0000FAD8 File Offset: 0x0000DCD8
	public static int Powerup_SteamLocomotive_Bonus_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupSteamLocomotiveBonus;
	}

	// Token: 0x06000236 RID: 566 RVA: 0x0000FAF8 File Offset: 0x0000DCF8
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

	// (get) Token: 0x06000237 RID: 567 RVA: 0x0000FB20 File Offset: 0x0000DD20
	// (set) Token: 0x06000238 RID: 568 RVA: 0x0000FB40 File Offset: 0x0000DD40
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

	// (get) Token: 0x06000239 RID: 569 RVA: 0x0000FB60 File Offset: 0x0000DD60
	// (set) Token: 0x0600023A RID: 570 RVA: 0x0000FB80 File Offset: 0x0000DD80
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

	// (get) Token: 0x0600023B RID: 571 RVA: 0x0000FBA0 File Offset: 0x0000DDA0
	// (set) Token: 0x0600023C RID: 572 RVA: 0x0000FBC0 File Offset: 0x0000DDC0
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

	// (get) Token: 0x0600023D RID: 573 RVA: 0x0000FBE0 File Offset: 0x0000DDE0
	// (set) Token: 0x0600023E RID: 574 RVA: 0x0000FC00 File Offset: 0x0000DE00
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

	// (get) Token: 0x0600023F RID: 575 RVA: 0x0000FC20 File Offset: 0x0000DE20
	// (set) Token: 0x06000240 RID: 576 RVA: 0x0000FC40 File Offset: 0x0000DE40
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

	// (get) Token: 0x06000241 RID: 577 RVA: 0x0000FC60 File Offset: 0x0000DE60
	// (set) Token: 0x06000242 RID: 578 RVA: 0x0000FC80 File Offset: 0x0000DE80
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

	// Token: 0x06000243 RID: 579 RVA: 0x0000FCA0 File Offset: 0x0000DEA0
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

	// Token: 0x06000244 RID: 580 RVA: 0x0000FDB0 File Offset: 0x0000DFB0
	public static List<GameplayData.JimboAbility> Powerup_Jimbo_AbilitiesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return null;
		}
		return instance.jimboAbilities_Selected;
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000FDD0 File Offset: 0x0000DFD0
	public static bool Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility ability, bool considerEquippedState)
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && (!considerEquippedState || PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Jimbo)) && instance.jimboAbilities_Selected.Contains(ability);
	}

	// Token: 0x06000246 RID: 582 RVA: 0x0000FE08 File Offset: 0x0000E008
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

	// Token: 0x06000247 RID: 583 RVA: 0x0000FE50 File Offset: 0x0000E050
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

	// Token: 0x06000248 RID: 584 RVA: 0x0000FF04 File Offset: 0x0000E104
	public static int Powerup_RedPepper_ActivationsCounterGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupRedPepper_ActivationsCounter;
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0000FF24 File Offset: 0x0000E124
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

	// Token: 0x0600024A RID: 586 RVA: 0x0000FF54 File Offset: 0x0000E154
	public static void Powerup_RedPepper_ActivationsCounterAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_RedPepper_ActivationsCounterSet(instance._powerupRedPepper_ActivationsCounter + value);
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0000FF78 File Offset: 0x0000E178
	public static int Powerup_GreenPepper_ActivationsCounterGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupGreenPepper_ActivationsCounter;
	}

	// Token: 0x0600024C RID: 588 RVA: 0x0000FF98 File Offset: 0x0000E198
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

	// Token: 0x0600024D RID: 589 RVA: 0x0000FFC8 File Offset: 0x0000E1C8
	public static void Powerup_GreenPepper_ActivationsCounterAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_GreenPepper_ActivationsCounterSet(instance._powerupGreenPepper_ActivationsCounter + value);
	}

	// Token: 0x0600024E RID: 590 RVA: 0x0000FFEC File Offset: 0x0000E1EC
	public static int Powerup_GoldenPepper_LuckBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupGoldenPepper_LuckBonus;
	}

	// Token: 0x0600024F RID: 591 RVA: 0x0001000C File Offset: 0x0000E20C
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

	// Token: 0x06000250 RID: 592 RVA: 0x0001003C File Offset: 0x0000E23C
	public static int Powerup_RottenPepper_LuckBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupRottenPepper_LuckBonus;
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0001005C File Offset: 0x0000E25C
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

	// Token: 0x06000252 RID: 594 RVA: 0x0001008C File Offset: 0x0000E28C
	public static int Powerup_BellPepper_LuckBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupBellPepper_LuckBonus;
	}

	// Token: 0x06000253 RID: 595 RVA: 0x000100AC File Offset: 0x0000E2AC
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

	// Token: 0x06000254 RID: 596 RVA: 0x000100DC File Offset: 0x0000E2DC
	public static long Powerup_DevilHorn_AdditionalMultiplierGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupDevilHorn_AdditionalMultiplier;
	}

	// Token: 0x06000255 RID: 597 RVA: 0x000100FC File Offset: 0x0000E2FC
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

	// Token: 0x06000256 RID: 598 RVA: 0x0001012C File Offset: 0x0000E32C
	public static void Powerup_DevilHorn_AdditionalMultiplierAdd(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.Powerup_DevilHorn_AdditionalMultiplierSet(instance._powerupDevilHorn_AdditionalMultiplier + value);
	}

	// Token: 0x06000257 RID: 599 RVA: 0x00010150 File Offset: 0x0000E350
	public static int Powerup_Baphomet_ActivationsCounterGet_Above()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupBaphomet_SymbolsBonus;
	}

	// Token: 0x06000258 RID: 600 RVA: 0x00010170 File Offset: 0x0000E370
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

	// Token: 0x06000259 RID: 601 RVA: 0x000101A0 File Offset: 0x0000E3A0
	public static int Powerup_Baphomet_ActivationsCounterGet_Below()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupBaphomet_PatternsBonus;
	}

	// Token: 0x0600025A RID: 602 RVA: 0x000101C0 File Offset: 0x0000E3C0
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

	// Token: 0x0600025B RID: 603 RVA: 0x000101F0 File Offset: 0x0000E3F0
	public static long Powerup_Cross_TriggersCount_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupCross_Triggers;
	}

	// Token: 0x0600025C RID: 604 RVA: 0x00010210 File Offset: 0x0000E410
	public static void Powerup_Cross_TriggersCount_Set(long i)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupCross_Triggers = i;
	}

	// Token: 0x0600025D RID: 605 RVA: 0x00010230 File Offset: 0x0000E430
	public static int Powerup_PossessedPhone_TriggersCount_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance._powerupPossessedPhone_SpinsCount;
	}

	// Token: 0x0600025E RID: 606 RVA: 0x00010250 File Offset: 0x0000E450
	public static void Powerup_PossessedPhone_TriggersCount_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupPossessedPhone_SpinsCount = Mathf.Max(0, n);
	}

	// Token: 0x0600025F RID: 607 RVA: 0x00010274 File Offset: 0x0000E474
	public static float Powerup_GoldenKingMida_ExtraBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupGoldenKingMida_ExtraBonus;
	}

	// Token: 0x06000260 RID: 608 RVA: 0x00010298 File Offset: 0x0000E498
	public static void Powerup_GoldenKingMida_ExtraBonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupGoldenKingMida_ExtraBonus = value;
	}

	// Token: 0x06000261 RID: 609 RVA: 0x000102B8 File Offset: 0x0000E4B8
	public static float Powerup_Dealer_ExtraBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupDealer_ExtraBonus;
	}

	// Token: 0x06000262 RID: 610 RVA: 0x000102DC File Offset: 0x0000E4DC
	public static void Powerup_Dealer_ExtraBonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupDealer_ExtraBonus = value;
	}

	// Token: 0x06000263 RID: 611 RVA: 0x000102FC File Offset: 0x0000E4FC
	public static float Powerup_Capitalist_ExtraBonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupCapitalist_ExtraBonus;
	}

	// Token: 0x06000264 RID: 612 RVA: 0x00010320 File Offset: 0x0000E520
	public static void Powerup_Capitalist_ExtraBonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupCapitalist_ExtraBonus = value;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x00010340 File Offset: 0x0000E540
	public static float Powerup_PersonalTrainer_BonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupPersonalTrainer_Bonus;
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00010364 File Offset: 0x0000E564
	public static void Powerup_PersonalTrainer_BonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupPersonalTrainer_Bonus = value;
	}

	// Token: 0x06000267 RID: 615 RVA: 0x00010382 File Offset: 0x0000E582
	public static void Powerup_PersonalTrainer_BonusReset()
	{
		GameplayData.Powerup_PersonalTrainer_BonusSet(0.25f);
	}

	// Token: 0x06000268 RID: 616 RVA: 0x00010390 File Offset: 0x0000E590
	public static float Powerup_Electrician_BonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupElectrician_Bonus;
	}

	// Token: 0x06000269 RID: 617 RVA: 0x000103B4 File Offset: 0x0000E5B4
	public static void Powerup_Electrician_BonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupElectrician_Bonus = value;
	}

	// Token: 0x0600026A RID: 618 RVA: 0x000103D2 File Offset: 0x0000E5D2
	public static void Powerup_Electrician_BonusReset()
	{
		GameplayData.Powerup_Electrician_BonusSet(0.05f);
	}

	// Token: 0x0600026B RID: 619 RVA: 0x000103E0 File Offset: 0x0000E5E0
	public static float Powerup_FortuneTeller_BonusGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0f;
		}
		return instance._powerupFortuneTeller_Bonus;
	}

	// Token: 0x0600026C RID: 620 RVA: 0x00010404 File Offset: 0x0000E604
	public static void Powerup_FortuneTeller_BonusSet(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupFortuneTeller_Bonus = value;
	}

	// Token: 0x0600026D RID: 621 RVA: 0x00010422 File Offset: 0x0000E622
	public static void Powerup_FortuneTeller_BonusReset()
	{
		GameplayData.Powerup_FortuneTeller_BonusSet(0.25f);
	}

	// Token: 0x0600026E RID: 622 RVA: 0x00010430 File Offset: 0x0000E630
	public static long Powerup_AceOfClubs_TicketsSpentGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupAceOfClubs_TicketsSpent;
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00010450 File Offset: 0x0000E650
	public static void Powerup_AceOfClubs_TicketsSpentSet(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupAceOfClubs_TicketsSpent = value;
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00010470 File Offset: 0x0000E670
	public static long Powerup_AceOfSpades_ActivationsCounterGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._powerupAceOfSpades_ActivationsCounter;
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00010490 File Offset: 0x0000E690
	public static void Powerup_AceOfSpades_ActivationsCounterSet(long value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupAceOfSpades_ActivationsCounter = value;
	}

	// Token: 0x06000272 RID: 626 RVA: 0x000104B0 File Offset: 0x0000E6B0
	public static PowerupScript.Identifier PowerupHoleCircle_CharmGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return PowerupScript.Identifier.undefined;
		}
		return instance._powerupHoleCircle_CharmIdentifier;
	}

	// Token: 0x06000273 RID: 627 RVA: 0x000104D0 File Offset: 0x0000E6D0
	public static void PowerupHoleCircle_CharmSet(PowerupScript.Identifier identifier)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupHoleCircle_CharmIdentifier = identifier;
	}

	// Token: 0x06000274 RID: 628 RVA: 0x000104F0 File Offset: 0x0000E6F0
	public static PowerupScript.Identifier PowerupHoleRomboid_CharmGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return PowerupScript.Identifier.undefined;
		}
		return instance._powerupHoleRomboid_CharmIdentifier;
	}

	// Token: 0x06000275 RID: 629 RVA: 0x00010510 File Offset: 0x0000E710
	public static void PowerupHoleRomboid_CharmSet(PowerupScript.Identifier identifier)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupHoleRomboid_CharmIdentifier = identifier;
	}

	// Token: 0x06000276 RID: 630 RVA: 0x00010530 File Offset: 0x0000E730
	public static AbilityScript.Identifier PowerupHoleCross_AbilityGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return AbilityScript.Identifier.undefined;
		}
		return instance._powerupHoleCross_AbilityIdentifier;
	}

	// Token: 0x06000277 RID: 631 RVA: 0x00010550 File Offset: 0x0000E750
	public static void PowerupHoleCross_AbilitySet(AbilityScript.Identifier identifier)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._powerupHoleCross_AbilityIdentifier = identifier;
	}

	// (get) Token: 0x06000278 RID: 632 RVA: 0x00010570 File Offset: 0x0000E770
	// (set) Token: 0x06000279 RID: 633 RVA: 0x00010590 File Offset: 0x0000E790
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

	// Token: 0x0600027A RID: 634 RVA: 0x000105B4 File Offset: 0x0000E7B4
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
		}
		return num;
	}

	// Token: 0x0600027B RID: 635 RVA: 0x000105F4 File Offset: 0x0000E7F4
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

	// Token: 0x0600027C RID: 636 RVA: 0x0001064C File Offset: 0x0000E84C
	public static void MaxEquippablePowerupsAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.MaxEquippablePowerupsSet(instance.maxEquippablePowerups + value);
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00010670 File Offset: 0x0000E870
	public static void MaxEquippablePowerupsReset()
	{
		GameplayData.MaxEquippablePowerupsSet(7);
	}

	// Token: 0x0600027E RID: 638 RVA: 0x00010678 File Offset: 0x0000E878
	private static int _MaxEquippablePowerupsGet_AbsoluteMaximum()
	{
		return ItemOrganizerScript.CharmsSlotN();
	}

	// Token: 0x0600027F RID: 639 RVA: 0x00010680 File Offset: 0x0000E880
	public static float PowerupCoinsMultiplierGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1f;
		}
		return instance.powerupCoinsMultiplier;
	}

	// Token: 0x06000280 RID: 640 RVA: 0x000106A4 File Offset: 0x0000E8A4
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

	// Token: 0x06000281 RID: 641 RVA: 0x000106DC File Offset: 0x0000E8DC
	public static void PowerupCoinsMultiplierAdd(float value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.PowerupCoinsMultiplierSet(instance.powerupCoinsMultiplier + value);
	}

	// Token: 0x06000282 RID: 642 RVA: 0x00010700 File Offset: 0x0000E900
	public static void PowerupCoinsMultiplierReset()
	{
		GameplayData.PowerupCoinsMultiplierSet(1f);
	}

	// Token: 0x06000283 RID: 643 RVA: 0x0001070C File Offset: 0x0000E90C
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

	// Token: 0x06000284 RID: 644 RVA: 0x00010744 File Offset: 0x0000E944
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

	// Token: 0x06000285 RID: 645 RVA: 0x00010774 File Offset: 0x0000E974
	public static void RedButtonActivationsMultiplierAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.RedButtonActivationsMultiplierSet(instance._redButtonActivationsMultiplier + value);
	}

	// Token: 0x06000286 RID: 646 RVA: 0x00010798 File Offset: 0x0000E998
	private void _Phone_PrepareForSerialization()
	{
		this.phoneAbilitiesPickHistory_AsString = PlatformDataMaster.EnumListToString<AbilityScript.Identifier>(this.phoneAbilitiesPickHistory, ',');
		this._phone_AbilitiesToPick_String = PlatformDataMaster.EnumListToString<AbilityScript.Identifier>(this._phone_AbilitiesToPick, ',');
		this.nineNineNine_TotalRewardEarned_ByteArray = this.nineNineNine_TotalRewardEarned.ToByteArray();
	}

	// Token: 0x06000287 RID: 647 RVA: 0x000107D1 File Offset: 0x0000E9D1
	private void _Phone_RestoreFromSerialization()
	{
		this.phoneAbilitiesPickHistory = PlatformDataMaster.EnumListFromString<AbilityScript.Identifier>(this.phoneAbilitiesPickHistory_AsString, ',');
		this._phone_AbilitiesToPick = PlatformDataMaster.EnumListFromString<AbilityScript.Identifier>(this._phone_AbilitiesToPick_String, ',');
		this.nineNineNine_TotalRewardEarned = this.BigIntegerFromByteArray(this.nineNineNine_TotalRewardEarned_ByteArray, 0);
	}

	// (get) Token: 0x06000288 RID: 648 RVA: 0x00010814 File Offset: 0x0000EA14
	// (set) Token: 0x06000289 RID: 649 RVA: 0x00010834 File Offset: 0x0000EA34
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

	// Token: 0x0600028A RID: 650 RVA: 0x00010858 File Offset: 0x0000EA58
	public static void Phone_SpeciallCallBooking_Reset()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._phone_bookSpecialCall = false;
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00010878 File Offset: 0x0000EA78
	public static bool NineNineNine_IsTime()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance._phone_SpecialCalls_Counter > 3 && instance._phone_EvilCallsPicked_Counter <= 0;
	}

	// Token: 0x0600028C RID: 652 RVA: 0x000108A8 File Offset: 0x0000EAA8
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

	// Token: 0x0600028D RID: 653 RVA: 0x000108F8 File Offset: 0x0000EAF8
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

	// Token: 0x0600028E RID: 654 RVA: 0x00010928 File Offset: 0x0000EB28
	public static int PhoneAbilitiesNumber_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 3;
		}
		return instance._phoneAbilitiesNumber;
	}

	// Token: 0x0600028F RID: 655 RVA: 0x00010948 File Offset: 0x0000EB48
	public static void PhoneAbilitiesNumber_SetToMAX()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._phoneAbilitiesNumber = 4;
	}

	// Token: 0x06000290 RID: 656 RVA: 0x00010968 File Offset: 0x0000EB68
	public static void PhoneAbilitiesNumber_SetToDefault()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._phoneAbilitiesNumber = 3;
	}

	// Token: 0x06000291 RID: 657 RVA: 0x00010988 File Offset: 0x0000EB88
	public static long PhoneRerollCostGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 1L;
		}
		return instance._phoneRerollCost;
	}

	// Token: 0x06000292 RID: 658 RVA: 0x000109A8 File Offset: 0x0000EBA8
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

	// Token: 0x06000293 RID: 659 RVA: 0x000109E8 File Offset: 0x0000EBE8
	public static void PhoneRerollCostIncrease()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._phoneRerollCost += instance._phoneRerollCostIncrease;
	}

	// Token: 0x06000294 RID: 660 RVA: 0x00010A14 File Offset: 0x0000EC14
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

	// Token: 0x06000295 RID: 661 RVA: 0x00010A4C File Offset: 0x0000EC4C
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

	// Token: 0x06000296 RID: 662 RVA: 0x00010A7C File Offset: 0x0000EC7C
	public static void PhonePickMultiplierAdd(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		GameplayData.PhonePickMultiplierSet(instance._phonePickMultiplier + value);
	}

	// Token: 0x06000297 RID: 663 RVA: 0x00010AA0 File Offset: 0x0000ECA0
	public static BigInteger NineNineNne_TotalRewardEarned_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.nineNineNine_TotalRewardEarned;
	}

	// Token: 0x06000298 RID: 664 RVA: 0x00010AC4 File Offset: 0x0000ECC4
	public static void NineNineNne_TotalRewardEarned_Set(BigInteger n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.nineNineNine_TotalRewardEarned = n;
	}

	// Token: 0x06000299 RID: 665 RVA: 0x00010AE4 File Offset: 0x0000ECE4
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

	// Token: 0x0600029A RID: 666 RVA: 0x00010BE0 File Offset: 0x0000EDE0
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

	// Token: 0x0600029B RID: 667 RVA: 0x00010C18 File Offset: 0x0000EE18
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

	// Token: 0x0600029C RID: 668 RVA: 0x00010C50 File Offset: 0x0000EE50
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

	// Token: 0x0600029D RID: 669 RVA: 0x00010C88 File Offset: 0x0000EE88
	public static int PhoneAbilities_GetSkippedCount_Total()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.phoneEasyCounter_SkippedCalls_Total;
	}

	// Token: 0x0600029E RID: 670 RVA: 0x00010CA8 File Offset: 0x0000EEA8
	public static int PhoneAbilities_GetSkippedCount_Normal()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.phoneEasyCounter_SkippedCalls_Normal;
	}

	// Token: 0x0600029F RID: 671 RVA: 0x00010CC8 File Offset: 0x0000EEC8
	public static int PhoneAbilities_GetSkippedCount_Evil()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.phoneEasyCounter_SkippedCalls_Evil;
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x00010CE8 File Offset: 0x0000EEE8
	public static int PhoneAbilities_GetSkippedCount_Good()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.phoneEasyCounter_SkippedCalls_Good;
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x00010D08 File Offset: 0x0000EF08
	public static long PhoneRerollPerformed_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance._phoneRerollsPerformed;
	}

	// Token: 0x060002A2 RID: 674 RVA: 0x00010D28 File Offset: 0x0000EF28
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

	// (get) Token: 0x060002A3 RID: 675 RVA: 0x00010D50 File Offset: 0x0000EF50
	// (set) Token: 0x060002A4 RID: 676 RVA: 0x00010D70 File Offset: 0x0000EF70
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

	// Token: 0x060002A5 RID: 677 RVA: 0x00010D8E File Offset: 0x0000EF8E
	private void RunModifiers_SavePreparing()
	{
		if (this.runModifierPicked == RunModifierScript.Identifier.undefined || this.runModifierPicked == RunModifierScript.Identifier.count)
		{
			this.runModifierPicked = RunModifierScript.Identifier.defaultModifier;
		}
		this.runModifierPicked_AsString = PlatformDataMaster.EnumEntryToString<RunModifierScript.Identifier>(this.runModifierPicked);
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00010DBC File Offset: 0x0000EFBC
	private void RunModifiers_LoadFormat()
	{
		this.runModifierPicked = PlatformDataMaster.EnumEntryFromString<RunModifierScript.Identifier>(this.runModifierPicked_AsString, RunModifierScript.Identifier.defaultModifier);
		if (this.runModifierPicked == RunModifierScript.Identifier.undefined || this.runModifierPicked == RunModifierScript.Identifier.count)
		{
			this.runModifierPicked = RunModifierScript.Identifier.defaultModifier;
		}
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x00010DEC File Offset: 0x0000EFEC
	public static RunModifierScript.Identifier RunModifier_GetCurrent()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return RunModifierScript.Identifier.defaultModifier;
		}
		return instance.runModifierPicked;
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00010E0C File Offset: 0x0000F00C
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

	// Token: 0x060002A9 RID: 681 RVA: 0x00010E90 File Offset: 0x0000F090
	public static bool RunModifier_AlreadyPicked()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance._runModifier_AlreadySet;
	}

	// Token: 0x060002AA RID: 682 RVA: 0x00010EB0 File Offset: 0x0000F0B0
	public static bool RunModifier_DealIsAvailable_Get()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.runModifier_DealIsAvailable;
	}

	// Token: 0x060002AB RID: 683 RVA: 0x00010ED0 File Offset: 0x0000F0D0
	public static void RunModifier_DealIsAvailable_Set(bool value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.runModifier_DealIsAvailable = value;
	}

	// Token: 0x060002AC RID: 684 RVA: 0x00010EF0 File Offset: 0x0000F0F0
	public static int RunModifier_AcceptedDealsCounter_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.runModifier_AcceptedDealsCounter;
	}

	// Token: 0x060002AD RID: 685 RVA: 0x00010F10 File Offset: 0x0000F110
	public static void RunModifier_AcceptedDealsCounter_Set(int n)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.runModifier_AcceptedDealsCounter = Mathf.Max(0, n);
	}

	// Token: 0x060002AE RID: 686 RVA: 0x00010F34 File Offset: 0x0000F134
	public static int RunModifier_BonusPacksThisTime_Get()
	{
		int num = (int)GameplayData.RunModifier_GetCurrent();
		int num2 = 0;
		if (num == 10)
		{
			num2++;
		}
		return GameplayData.RunModifier_AcceptedDealsCounter_Get() + 1 + num2;
	}

	// Token: 0x060002AF RID: 687 RVA: 0x00010F59 File Offset: 0x0000F159
	private void _MetaProgression_PrepareForSerialization()
	{
	}

	// Token: 0x060002B0 RID: 688 RVA: 0x00010F5B File Offset: 0x0000F15B
	private void _MetaProgression_RestoreFromSerialization()
	{
	}

	// Token: 0x060002B1 RID: 689 RVA: 0x00010F60 File Offset: 0x0000F160
	public static bool AlreadyBoughtPowerupAtTerminalGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance._alreadyBoughtPowerupAtTerminal;
	}

	// Token: 0x060002B2 RID: 690 RVA: 0x00010F80 File Offset: 0x0000F180
	public static void AlreadyBoughtPowerupAtTerminalSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance._alreadyBoughtPowerupAtTerminal = true;
	}

	// Token: 0x060002B3 RID: 691 RVA: 0x00010FA0 File Offset: 0x0000F1A0
	public static bool NewGameIntroFinished_Get()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.newGameIntro_Finished;
	}

	// Token: 0x060002B4 RID: 692 RVA: 0x00010FC0 File Offset: 0x0000F1C0
	public static void NewGameIntroFinished_Set(bool finished)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.newGameIntro_Finished = finished;
	}

	// Token: 0x060002B5 RID: 693 RVA: 0x00010FDE File Offset: 0x0000F1DE
	private void _Ending_PrepareForSerialization()
	{
		this.rewardBoxDebtIndex_ByteArray = this.rewardBoxDebtIndex.ToByteArray();
	}

	// Token: 0x060002B6 RID: 694 RVA: 0x00010FF1 File Offset: 0x0000F1F1
	private void _Ending_RestoreFromSerialization()
	{
		this.rewardBoxDebtIndex = this.BigIntegerFromByteArray(this.rewardBoxDebtIndex_ByteArray, -1);
	}

	// Token: 0x060002B7 RID: 695 RVA: 0x0001100C File Offset: 0x0000F20C
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

	// Token: 0x060002B8 RID: 696 RVA: 0x0001109C File Offset: 0x0000F29C
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

	// Token: 0x060002B9 RID: 697 RVA: 0x00011160 File Offset: 0x0000F360
	public static BigInteger GetRewardDeadlineDebt()
	{
		if (GameplayData.Instance == null)
		{
			return 100000;
		}
		return GameplayData.DebtGetExt(GameplayData.GetRewardBoxDebtIndex());
	}

	// Token: 0x060002BA RID: 698 RVA: 0x0001117E File Offset: 0x0000F37E
	public static bool RewardTimeToShowAmmount()
	{
		return GameplayData.Instance != null && GameplayData.DebtIndexGet() >= GameplayData.GetRewardBoxDebtIndex();
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00011198 File Offset: 0x0000F398
	public static bool WinConditionAlreadyAchieved()
	{
		return GameplayData.Instance != null && (GameplayData.DebtIndexGet() > GameplayData.GetRewardBoxDebtIndex() || RewardBoxScript.IsOpened());
	}

	// Token: 0x060002BC RID: 700 RVA: 0x000111BC File Offset: 0x0000F3BC
	public static bool KeptPlayingPastWinConditionGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.keptPlayingPastWinCondition;
	}

	// Token: 0x060002BD RID: 701 RVA: 0x000111DC File Offset: 0x0000F3DC
	public static void KeptPlayingPastWinConditionSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.keptPlayingPastWinCondition = true;
	}

	// Token: 0x060002BE RID: 702 RVA: 0x000111FC File Offset: 0x0000F3FC
	public static bool RewardBoxIsOpened()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.rewardBoxWasOpened;
	}

	// Token: 0x060002BF RID: 703 RVA: 0x0001121C File Offset: 0x0000F41C
	public static void RewardBoxSetOpened()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.rewardBoxWasOpened = true;
	}

	// Token: 0x060002C0 RID: 704 RVA: 0x0001123C File Offset: 0x0000F43C
	public static bool RewardBoxHasPrize()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.rewardBoxHasPrize;
	}

	// Token: 0x060002C1 RID: 705 RVA: 0x0001125C File Offset: 0x0000F45C
	public static void RewardBoxPickupPrize()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.rewardBoxHasPrize = false;
	}

	// Token: 0x060002C2 RID: 706 RVA: 0x0001127C File Offset: 0x0000F47C
	public static bool PrizeWasUsedGet()
	{
		GameplayData instance = GameplayData.Instance;
		return instance != null && instance.prizeWasUsed;
	}

	// Token: 0x060002C3 RID: 707 RVA: 0x0001129C File Offset: 0x0000F49C
	public static void PrizeWasUsedSet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.prizeWasUsed = true;
	}

	// Token: 0x060002C4 RID: 708 RVA: 0x000112BA File Offset: 0x0000F4BA
	public static bool IsInVictoryCondition()
	{
		return GameplayData.Instance != null && !GameplayMaster.GameIsResetting() && GameplayData.RewardBoxIsOpened() && !GameplayData.RewardBoxHasPrize() && GameplayData.PrizeWasUsedGet();
	}

	// Token: 0x060002C5 RID: 709 RVA: 0x000112EA File Offset: 0x0000F4EA
	public static bool IsInGoodEndingCondition(bool considerKeyState)
	{
		if (!considerKeyState)
		{
			return GameplayData.NineNineNine_IsTime();
		}
		return GameplayData.IsInVictoryCondition() && GameplayData.NineNineNine_IsTime();
	}

	// Token: 0x060002C6 RID: 710 RVA: 0x00011303 File Offset: 0x0000F503
	public static bool IsInBadEndingCondition(bool considerKeyState)
	{
		if (!considerKeyState)
		{
			return !GameplayData.NineNineNine_IsTime();
		}
		return GameplayData.IsInVictoryCondition() && !GameplayData.NineNineNine_IsTime();
	}

	// Token: 0x060002C7 RID: 711 RVA: 0x00011324 File Offset: 0x0000F524
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

	// (get) Token: 0x060002C8 RID: 712 RVA: 0x00011368 File Offset: 0x0000F568
	// (set) Token: 0x060002C9 RID: 713 RVA: 0x0001139C File Offset: 0x0000F59C
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

	// Token: 0x060002CA RID: 714 RVA: 0x000113BC File Offset: 0x0000F5BC
	public static long Stats_ModifiedLemonTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedLemonTriggeredTimes;
	}

	// Token: 0x060002CB RID: 715 RVA: 0x000113DC File Offset: 0x0000F5DC
	public static void Stats_ModifiedLemonTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedLemonTriggeredTimes += 1L;
	}

	// Token: 0x060002CC RID: 716 RVA: 0x00011404 File Offset: 0x0000F604
	public static long Stats_ModifiedCherryTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedCherryTriggeredTimes;
	}

	// Token: 0x060002CD RID: 717 RVA: 0x00011424 File Offset: 0x0000F624
	public static void Stats_ModifiedCherryTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedCherryTriggeredTimes += 1L;
	}

	// Token: 0x060002CE RID: 718 RVA: 0x0001144C File Offset: 0x0000F64C
	public static long Stats_ModifiedCloverTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedCloverTriggeredTimes;
	}

	// Token: 0x060002CF RID: 719 RVA: 0x0001146C File Offset: 0x0000F66C
	public static void Stats_ModifiedCloverTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedCloverTriggeredTimes += 1L;
	}

	// Token: 0x060002D0 RID: 720 RVA: 0x00011494 File Offset: 0x0000F694
	public static long Stats_ModifiedBellTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedBellTriggeredTimes;
	}

	// Token: 0x060002D1 RID: 721 RVA: 0x000114B4 File Offset: 0x0000F6B4
	public static void Stats_ModifiedBellTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedBellTriggeredTimes += 1L;
	}

	// Token: 0x060002D2 RID: 722 RVA: 0x000114DC File Offset: 0x0000F6DC
	public static long Stats_ModifiedDiamondTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedDiamondTriggeredTimes;
	}

	// Token: 0x060002D3 RID: 723 RVA: 0x000114FC File Offset: 0x0000F6FC
	public static void Stats_ModifiedDiamondTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedDiamondTriggeredTimes += 1L;
	}

	// Token: 0x060002D4 RID: 724 RVA: 0x00011524 File Offset: 0x0000F724
	public static long Stats_ModifiedCoinsTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedCoinsTriggeredTimes;
	}

	// Token: 0x060002D5 RID: 725 RVA: 0x00011544 File Offset: 0x0000F744
	public static void Stats_ModifiedCoinsTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedCoinsTriggeredTimes += 1L;
	}

	// Token: 0x060002D6 RID: 726 RVA: 0x0001156C File Offset: 0x0000F76C
	public static long Stats_ModifiedSevenTriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedSevenTriggeredTimes;
	}

	// Token: 0x060002D7 RID: 727 RVA: 0x0001158C File Offset: 0x0000F78C
	public static void Stats_ModifiedSevenTriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedSevenTriggeredTimes += 1L;
	}

	// Token: 0x060002D8 RID: 728 RVA: 0x000115B4 File Offset: 0x0000F7B4
	public static long Stats_ModifiedSymbol_TriggeredTimesGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_ModifiedSymbolTriggeredTimes;
	}

	// Token: 0x060002D9 RID: 729 RVA: 0x000115D4 File Offset: 0x0000F7D4
	public static void Stats_ModifiedSymbol_TriggeredTimesAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_ModifiedSymbolTriggeredTimes += 1L;
	}

	// Token: 0x060002DA RID: 730 RVA: 0x000115FC File Offset: 0x0000F7FC
	public static int Stats_RedButtonEffectiveActivations_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.stats_RedButtonEffectiveActivationsCounter;
	}

	// Token: 0x060002DB RID: 731 RVA: 0x0001161C File Offset: 0x0000F81C
	public static void Stats_RedButtonEffectiveActivations_Set(int value)
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_RedButtonEffectiveActivationsCounter = value;
	}

	// Token: 0x060002DC RID: 732 RVA: 0x0001163C File Offset: 0x0000F83C
	public static long Stats_RestocksBoughtGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_RestocksBoughtN;
	}

	// Token: 0x060002DD RID: 733 RVA: 0x0001165C File Offset: 0x0000F85C
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

	// Token: 0x060002DE RID: 734 RVA: 0x0001168C File Offset: 0x0000F88C
	public static long Stats_RestocksPerformedGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_RestocksPerformed;
	}

	// Token: 0x060002DF RID: 735 RVA: 0x000116AC File Offset: 0x0000F8AC
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

	// Token: 0x060002E0 RID: 736 RVA: 0x000116DC File Offset: 0x0000F8DC
	public static long Stats_PeppersBoughtGet()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_PeppersBought;
	}

	// Token: 0x060002E1 RID: 737 RVA: 0x000116FC File Offset: 0x0000F8FC
	public static void Stats_PeppersBoughtAdd()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_PeppersBought += 1L;
	}

	// (get) Token: 0x060002E2 RID: 738 RVA: 0x00011724 File Offset: 0x0000F924
	// (set) Token: 0x060002E3 RID: 739 RVA: 0x00011744 File Offset: 0x0000F944
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

	// Token: 0x060002E4 RID: 740 RVA: 0x00011762 File Offset: 0x0000F962
	private void _GameStats_PrepareForSerialization()
	{
		this.stats_CoinsEarned_ByteArray = this.stats_CoinsEarned.ToByteArray();
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x00011775 File Offset: 0x0000F975
	private void _GameStats_RestoreFromSerialization()
	{
		this.stats_CoinsEarned = this.BigIntegerFromByteArray(this.stats_CoinsEarned_ByteArray, 0);
	}

	// Token: 0x060002E6 RID: 742 RVA: 0x00011790 File Offset: 0x0000F990
	public static long Stats_PlayTime_GetSeconds()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_PlayTime_Seconds;
	}

	// Token: 0x060002E7 RID: 743 RVA: 0x000117B0 File Offset: 0x0000F9B0
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

	// Token: 0x060002E8 RID: 744 RVA: 0x000117E8 File Offset: 0x0000F9E8
	public static long Stats_DeadlinesCompleted_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_DeadlinesCompleted;
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x00011808 File Offset: 0x0000FA08
	public static void Stats_DeadlinesCompleted_Add()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return;
		}
		instance.stats_DeadlinesCompleted += 1L;
	}

	// Token: 0x060002EA RID: 746 RVA: 0x00011830 File Offset: 0x0000FA30
	public static BigInteger Stats_CoinsEarned_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0;
		}
		return instance.stats_CoinsEarned;
	}

	// Token: 0x060002EB RID: 747 RVA: 0x00011854 File Offset: 0x0000FA54
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

	// Token: 0x060002EC RID: 748 RVA: 0x00011898 File Offset: 0x0000FA98
	public static long Stats_TicketsEarned_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_TicketsEarned;
	}

	// Token: 0x060002ED RID: 749 RVA: 0x000118B8 File Offset: 0x0000FAB8
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

	// Token: 0x060002EE RID: 750 RVA: 0x000118F0 File Offset: 0x0000FAF0
	public static long Stats_CharmsBought_Get()
	{
		GameplayData instance = GameplayData.Instance;
		if (instance == null)
		{
			return 0L;
		}
		return instance.stats_CharmsBought;
	}

	// Token: 0x060002EF RID: 751 RVA: 0x00011910 File Offset: 0x0000FB10
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
	private long _storeFreeRestocks;

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

	public const long CLOVER_TICKETS_INITIAL = 2L;

	private const long CLOVER_TICKETS_BONUS_FOR_LITTLE_BET = 2L;

	private const long CLOVER_TICKETS_BONUS_FOR_BIG_BET = 1L;

	private const long CLOVER_TICKETS_BONUS_FOR_ROUNDS_LEFT = 4L;

	[SerializeField]
	private long cloverTickets = 2L;

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

	private const int POWERUP_FRUIT_BASKET_DEFAULT_ROUNDS = 7;

	[SerializeField]
	private int _powerupFruitsBasket_RoundsLeft = 7;

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
		// Token: 0x060010CA RID: 4298 RVA: 0x000673DC File Offset: 0x000655DC
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
		// Token: 0x060010CC RID: 4300 RVA: 0x00067463 File Offset: 0x00065663
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
		// Token: 0x060010CE RID: 4302 RVA: 0x000674A1 File Offset: 0x000656A1
		public PowerupScript.Identifier IdentifierGetInferred()
		{
			if (this._identifier == PowerupScript.Identifier.undefined)
			{
				this._identifier = PlatformDataMaster.EnumEntryFromString<PowerupScript.Identifier>(this.powerupIdentifierAsString, PowerupScript.Identifier.undefined);
			}
			return this._identifier;
		}

		// Token: 0x060010CF RID: 4303 RVA: 0x000674C4 File Offset: 0x000656C4
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
