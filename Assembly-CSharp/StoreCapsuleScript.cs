using System;
using System.Collections.Generic;
using System.Numerics;
using Panik;
using TMPro;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class StoreCapsuleScript : MonoBehaviour
{
	// Token: 0x06000860 RID: 2144 RVA: 0x00046904 File Offset: 0x00044B04
	public static StoreCapsuleScript GetStoreCapsuleById(int id)
	{
		foreach (StoreCapsuleScript storeCapsuleScript in StoreCapsuleScript.list)
		{
			if (storeCapsuleScript.id == id)
			{
				return storeCapsuleScript;
			}
		}
		Debug.LogError("StoreCapsuleScript: No StoreCapsule found with id: " + id.ToString());
		return null;
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00046978 File Offset: 0x00044B78
	public static BigInteger RestockCost_Get()
	{
		if (GameplayData.StoreFreeRestocksGet() > 0L)
		{
			return 0;
		}
		BigInteger bigInteger = GameplayData.StoreRestockExtraCostGet();
		BigInteger bigInteger2 = GameplayData.DebtGet() / 20;
		BigInteger bigInteger3 = GameplayData.DebtIndexGet();
		if (bigInteger3 > 8L)
		{
			return bigInteger2 + bigInteger;
		}
		if (bigInteger3 == 0L)
		{
			return 3 + bigInteger;
		}
		if (bigInteger3 == 1L)
		{
			return 6 + bigInteger;
		}
		if (bigInteger3 == 2L)
		{
			return 20 + bigInteger;
		}
		if (bigInteger3 == 3L)
		{
			return 69 + bigInteger;
		}
		if (bigInteger3 == 4L)
		{
			return 300 + bigInteger;
		}
		if (bigInteger3 == 5L)
		{
			return 1000 + bigInteger;
		}
		if (bigInteger3 == 6L)
		{
			return 2000 + bigInteger;
		}
		if (bigInteger3 == 7L)
		{
			return 7000 + bigInteger;
		}
		if (bigInteger3 == 8L)
		{
			return 43000 + bigInteger;
		}
		string text = "StoreCapsuleScript: RestockCost_Get: debtIndex is not well handled: ";
		BigInteger bigInteger4 = bigInteger3;
		Debug.LogError(text + bigInteger4.ToString());
		return bigInteger2 + bigInteger;
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00046AD0 File Offset: 0x00044CD0
	private long PowerupDiscountGet()
	{
		long num = GameplayData.StoreTemporaryDiscountGet();
		num += GameplayData.StoreTemporaryDiscountPerSlotGet(this.id);
		num += (long)PowerupScript.FideltyCard_DiscountGet(true);
		if (GameplayData.RunModifier_GetCurrent() == RunModifierScript.Identifier.lessSpaceMoreDiscount)
		{
			num += 1L;
		}
		return num;
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00046B0C File Offset: 0x00044D0C
	public BigInteger GetCapsuleCost()
	{
		if (this.isRefreshButton)
		{
			return StoreCapsuleScript.RestockCost_Get();
		}
		if (StoreCapsuleScript.storePowerups[this.id] == null)
		{
			return -1;
		}
		long num = StoreCapsuleScript.storePowerups[this.id].StartingPriceGet(true, true);
		num -= this.PowerupDiscountGet();
		if (num < 0L)
		{
			num = 0L;
		}
		return num;
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x00046B70 File Offset: 0x00044D70
	public void RefreshCostText()
	{
		BigInteger capsuleCost = this.GetCapsuleCost();
		Color color = new Color(0.7137255f, 0.78431374f, 0.15686275f, 1f);
		Color color2 = new Color(1f, 0.5f, 0f, 1f);
		if (capsuleCost < 0L)
		{
			this.costText.color = new Color(1f, 0.5f, 0f, 1f);
			this.costText.text = "NULL";
			return;
		}
		if (this.isRefreshButton)
		{
			long num = GameplayData.StoreFreeRestocksGet();
			if (num > 0L)
			{
				this.costText.color = color;
				this.costText.text = Translation.Get("STORE_TEXT_FREE") + " (" + num.ToString() + ")";
				return;
			}
		}
		if (capsuleCost <= 0L)
		{
			this.costText.color = color;
			this.costText.text = Translation.Get("STORE_TEXT_FREE");
			return;
		}
		if (!this.isRefreshButton)
		{
			if (this.PowerupDiscountGet() > 0L)
			{
				this.costText.color = color;
			}
			else
			{
				this.costText.color = color2;
			}
		}
		else
		{
			this.costText.color = color2;
		}
		string text = "<sprite name=\"CloverTicket\">";
		if (this.isRefreshButton)
		{
			text = "<sprite name=\"CoinSymbolOrange64\">";
		}
		this.costText.text = capsuleCost.ToStringSmart() + " " + text;
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00046CE0 File Offset: 0x00044EE0
	public static void RefreshCostTextAll()
	{
		foreach (StoreCapsuleScript storeCapsuleScript in StoreCapsuleScript.list)
		{
			storeCapsuleScript.RefreshCostText();
		}
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00046D30 File Offset: 0x00044F30
	public static StoreCapsuleScript.BuyResult BuyTry(int id)
	{
		StoreCapsuleScript storeCapsuleById = StoreCapsuleScript.GetStoreCapsuleById(id);
		if (storeCapsuleById == null)
		{
			Debug.LogError("StoreCapsuleScript: BuyTry: storeCapsule is null");
		}
		long num = GameplayData.CloverTicketsGet();
		BigInteger bigInteger = GameplayData.CoinsGet();
		BigInteger capsuleCost = storeCapsuleById.GetCapsuleCost();
		if (capsuleCost < 0L && storeCapsuleById.isRefreshButton)
		{
			Debug.LogError("StoreCapsuleScript: BuyTry: cost is less than 0 on refresh button");
		}
		if (capsuleCost < 0L)
		{
			CameraGame.Shake(1f);
			Sound.Play("SoundMenuError", 1f, 1f);
			return StoreCapsuleScript.BuyResult.Empty;
		}
		if (storeCapsuleById.isRefreshButton)
		{
			if (bigInteger < capsuleCost)
			{
				CameraGame.Shake(1f);
				Sound.Play("SoundMenuError", 1f, 1f);
				return StoreCapsuleScript.BuyResult.NotEnoughCurrency;
			}
		}
		else if (num < capsuleCost)
		{
			CameraGame.Shake(1f);
			Sound.Play("SoundMenuError", 1f, 1f);
			return StoreCapsuleScript.BuyResult.NotEnoughCurrency;
		}
		if (storeCapsuleById.isRefreshButton)
		{
			StoreCapsuleScript.Restock(false, true, null, false, false);
			long num2 = GameplayData.StoreFreeRestocksGet();
			if (num2 <= 0L)
			{
				BigInteger bigInteger2 = StoreCapsuleScript.RestockCost_Get() / 5;
				if (bigInteger2 < 1L)
				{
					bigInteger2 = 1;
				}
				GameplayData.StoreRestockExtraCostAdd(bigInteger2);
			}
			else
			{
				num2 -= 1L;
				if (num2 < 0L)
				{
					num2 = 0L;
				}
				GameplayData.StoreFreeRestocksSet(num2);
			}
			if (capsuleCost > 0L)
			{
				GameplayData.Stats_RestocksBoughtSet(GameplayData.Stats_RestocksBoughtGet() + 1L);
			}
			GameplayData.Stats_RestocksPerformedSet(GameplayData.Stats_RestocksPerformedGet() + 1L);
			GameplayData.Powerup_BellPepper_LuckBonusSet(GameplayData.Powerup_BellPepper_LuckBonusGet() + 2);
			PowerupScript.ElectrcityCounter_TryRecharge();
			StoreCapsuleScript.RestockSave();
		}
		else
		{
			PowerupScript powerupScript = StoreCapsuleScript.storePowerups[id];
			PowerupScript.Identifier identifier = powerupScript.identifier;
			if (!PowerupScript.Equip(identifier, false, false))
			{
				return StoreCapsuleScript.BuyResult.InventoryFull;
			}
			GameplayData.Powerup_BoughtTimes_Increase(identifier);
			GameplayData.Stats_CharmsBought_Add();
			PowerupScript.Capitalist_GrowValue(powerupScript);
			if (powerupScript.identifier != PowerupScript.Identifier.PlayingCard_ClubsAce)
			{
				PowerupScript.ClassicPlayingCards_AceOfClubs_ProcessSpendedTickets(true, capsuleCost.CastToLong());
			}
			PowerupScript.WolfTrigger(true, powerupScript);
			PowerupScript.Archetype archetype = powerupScript.archetype;
			if (archetype != PowerupScript.Archetype.spicyPeppers)
			{
				if (archetype == PowerupScript.Archetype.symbolInstants)
				{
					Data.GameData game = Data.game;
					int num3 = game.UnlockSteps_PhotoBook;
					game.UnlockSteps_PhotoBook = num3 + 1;
					Data.GameData game2 = Data.game;
					num3 = game2.UnlockSteps_ScratchAndWin;
					game2.UnlockSteps_ScratchAndWin = num3 + 1;
				}
			}
			else
			{
				GameplayData.Stats_PeppersBoughtAdd();
				Data.GameData game3 = Data.game;
				int num3 = game3.UnlockSteps_FortuneCookie;
				game3.UnlockSteps_FortuneCookie = num3 + 1;
				Data.GameData game4 = Data.game;
				num3 = game4.UnlockSteps_FortuneChanneler;
				game4.UnlockSteps_FortuneChanneler = num3 + 1;
			}
			if (powerupScript.identifier == PowerupScript.Identifier.HorseShoe)
			{
				Data.GameData game5 = Data.game;
				int num3 = game5.UnlockSteps_HorseShoeGold;
				game5.UnlockSteps_HorseShoeGold = num3 + 1;
			}
			if (powerupScript.IsInstantPowerup())
			{
				Data.GameData game6 = Data.game;
				int num3 = game6.UnlockSteps_Abyssu;
				game6.UnlockSteps_Abyssu = num3 + 1;
			}
		}
		if (storeCapsuleById.isRefreshButton)
		{
			GameplayData.CoinsAdd(-capsuleCost, false);
		}
		else
		{
			GameplayData.CloverTicketsAdd(-capsuleCost.CastToLong(), false);
		}
		Sound.Play("SoundStoreBuy", 1f, 1f);
		StoreCapsuleScript.RefreshCostTextAll();
		PowerupScript.RefreshPlacementAll();
		storeCapsuleById.bounceScript.SetBounceScale(0.1f);
		if (TwitchUiScript.IsEnabled())
		{
			TwitchUiScript.Close(true);
		}
		if (!storeCapsuleById.isRefreshButton)
		{
			if (GameplayData.Stats_PeppersBoughtGet() >= 2L)
			{
				PowerupScript.Unlock(PowerupScript.Identifier.RedCrystal);
			}
		}
		else
		{
			GameplayData.Stats_RestocksBoughtGet();
			if (GameplayData.Stats_RestocksPerformedGet() >= 30L)
			{
				PowerupScript.Unlock(PowerupScript.Identifier.CloverBell);
			}
		}
		return StoreCapsuleScript.BuyResult.Success;
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0000CA12 File Offset: 0x0000AC12
	public static void RemovePowerupAt(int storeIndex, bool refreshStuff)
	{
		if (StoreCapsuleScript.storePowerups[storeIndex] == null)
		{
			return;
		}
		StoreCapsuleScript.storePowerups[storeIndex] = null;
		if (refreshStuff)
		{
			StoreCapsuleScript.RefreshCostTextAll();
			PowerupScript.RefreshPlacementAll();
		}
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00047050 File Offset: 0x00045250
	public static void RemovePowerup(PowerupScript.Identifier identifier, bool refreshStuff)
	{
		for (int i = 0; i < StoreCapsuleScript.storePowerups.Length; i++)
		{
			if (!(StoreCapsuleScript.storePowerups[i] == null) && StoreCapsuleScript.storePowerups[i].identifier == identifier)
			{
				StoreCapsuleScript.RemovePowerupAt(i, refreshStuff);
				return;
			}
		}
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x00047098 File Offset: 0x00045298
	public static void RemoveAllPowerups()
	{
		for (int i = 0; i < StoreCapsuleScript.storePowerups.Length; i++)
		{
			StoreCapsuleScript.RemovePowerupAt(i, false);
		}
		StoreCapsuleScript.RefreshCostTextAll();
		PowerupScript.RefreshPlacementAll();
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x000470C8 File Offset: 0x000452C8
	public static bool CanBuyAnything()
	{
		if (StoreCapsuleScript.list == null || StoreCapsuleScript.list.Count == 0)
		{
			return false;
		}
		BigInteger bigInteger = GameplayData.CoinsGet();
		for (int i = 0; i < StoreCapsuleScript.list.Count; i++)
		{
			StoreCapsuleScript storeCapsuleScript = StoreCapsuleScript.list[i];
			if (!(storeCapsuleScript == null) && !storeCapsuleScript.isRefreshButton && storeCapsuleScript.GetCapsuleCost() <= bigInteger)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00047134 File Offset: 0x00045334
	private static void PowerupsChain_Initilize(bool isNewGame)
	{
		GameplayData instance = GameplayData.Instance;
		StoreCapsuleScript._powerupChains.Clear();
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.FakeCoin,
			PowerupScript.Identifier.HamsaInverted,
			PowerupScript.Identifier.RedCrystal,
			PowerupScript.Identifier.Calendar,
			PowerupScript.Identifier.HorseShoe
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.LuckyCat,
			PowerupScript.Identifier.Stonks,
			PowerupScript.Identifier.HornChilyGreen,
			PowerupScript.Identifier.GrandmasPurse
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.TarotDeck,
			PowerupScript.Identifier.Dice_4,
			PowerupScript.Identifier.FakeCoin,
			PowerupScript.Identifier.HorseShoe
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.TarotDeck,
			PowerupScript.Identifier.Mushrooms,
			PowerupScript.Identifier.HornChilyGreen,
			PowerupScript.Identifier.HorseShoe
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.TarotDeck,
			PowerupScript.Identifier.YellowStar,
			PowerupScript.Identifier.Rorschach,
			PowerupScript.Identifier.FortuneChanneler
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.HamsaInverted,
			PowerupScript.Identifier.RedCrystal,
			PowerupScript.Identifier.Button2X
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.PoopBeetle,
			PowerupScript.Identifier.CloverPot,
			PowerupScript.Identifier.Hole_Romboid,
			PowerupScript.Identifier.CloversLandPatch,
			PowerupScript.Identifier.Skeleton_Head
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.HornDevil,
			PowerupScript.Identifier.HolyBible,
			PowerupScript.Identifier.Necronomicon,
			PowerupScript.Identifier.Baphomet
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Gabibbh,
			PowerupScript.Identifier.PossessedPhone,
			PowerupScript.Identifier.Rosary,
			PowerupScript.Identifier.Cross
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Cross,
			PowerupScript.Identifier.HolyBible,
			PowerupScript.Identifier.BookOfShadows,
			PowerupScript.Identifier.Gabibbh
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.MysticalTomato,
			PowerupScript.Identifier.HolyBible,
			PowerupScript.Identifier.SymbolInstant_Lemon,
			PowerupScript.Identifier.GoldenSymbol_Lemon
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.MysticalTomato,
			PowerupScript.Identifier.Rosary,
			PowerupScript.Identifier.SymbolInstant_Cherry,
			PowerupScript.Identifier.GoldenSymbol_Cherry
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.RitualBell,
			PowerupScript.Identifier.ShoppingCart,
			PowerupScript.Identifier.Rosary,
			PowerupScript.Identifier.CloverBell,
			PowerupScript.Identifier.SymbolInstant_Clover,
			PowerupScript.Identifier.SymbolInstant_Bell
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.SymbolInstant_Diamond,
			PowerupScript.Identifier.CrystalSkull,
			PowerupScript.Identifier.YellowStar,
			PowerupScript.Identifier.SymbolInstant_Treasure,
			PowerupScript.Identifier.Rorschach,
			PowerupScript.Identifier.SymbolInstant_Seven
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.CloverPet,
			PowerupScript.Identifier.Boardgame_M_Ditale,
			PowerupScript.Identifier.SymbolInstant_Clover,
			PowerupScript.Identifier.Hourglass,
			PowerupScript.Identifier.CloverPot,
			PowerupScript.Identifier.Wallet
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.CloverVoucher,
			PowerupScript.Identifier.CloverPot,
			PowerupScript.Identifier.CloverPet,
			PowerupScript.Identifier.Boardgame_M_FerroDaStiro,
			PowerupScript.Identifier.SymbolInstant_Bell,
			PowerupScript.Identifier.Wallet
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Boardgame_M_FerroDaStiro,
			PowerupScript.Identifier.CloverPot,
			PowerupScript.Identifier.SymbolInstant_Bell,
			PowerupScript.Identifier.Stonks,
			PowerupScript.Identifier.Wallet,
			PowerupScript.Identifier.Cigarettes
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Boardgame_M_Ditale,
			PowerupScript.Identifier.CloverPot,
			PowerupScript.Identifier.SymbolInstant_Clover,
			PowerupScript.Identifier.Stonks,
			PowerupScript.Identifier.Wallet,
			PowerupScript.Identifier.Cigarettes
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.CloverVoucher,
			PowerupScript.Identifier.CloverPot,
			PowerupScript.Identifier.Boardgame_M_FerroDaStiro,
			PowerupScript.Identifier.SymbolInstant_Bell,
			PowerupScript.Identifier.CloverPet,
			PowerupScript.Identifier.Wallet
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.CloversLandPatch,
			PowerupScript.Identifier.Sardines,
			PowerupScript.Identifier.DarkLotus,
			PowerupScript.Identifier.CloverPot,
			PowerupScript.Identifier.Wallet
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.PlayingCard_ClubsAce,
			PowerupScript.Identifier.CloverVoucher,
			PowerupScript.Identifier.CloverPot,
			PowerupScript.Identifier.CloverVoucher,
			PowerupScript.Identifier.CloverPet,
			PowerupScript.Identifier.CloverVoucher
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.DearDiary,
			PowerupScript.Identifier.Megaphone,
			PowerupScript.Identifier.CloverVoucher,
			PowerupScript.Identifier.CloverVoucher,
			PowerupScript.Identifier.CloverPot
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.SuperCapacitor,
			PowerupScript.Identifier.CarBattery,
			PowerupScript.Identifier.GeneralModCharm_CloverBellBattery,
			PowerupScript.Identifier.Button2X
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.CrankGenerator,
			PowerupScript.Identifier.Button2X,
			PowerupScript.Identifier.PuppetElectrician,
			PowerupScript.Identifier.CarBattery
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.PotatoPower,
			PowerupScript.Identifier.SuperCapacitor
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.CarBattery,
			PowerupScript.Identifier.ElectricityCounter
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Boardgame_C_Bricks,
			PowerupScript.Identifier.SymbolInstant_Lemon,
			PowerupScript.Identifier.GeneralModCharm_Clicker,
			PowerupScript.Identifier.PuppetPersonalTrainer
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.GoldenSymbol_Lemon,
			PowerupScript.Identifier.SymbolInstant_Lemon,
			PowerupScript.Identifier.PuppetPersonalTrainer,
			PowerupScript.Identifier.FruitBasket
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Boardgame_C_Wood,
			PowerupScript.Identifier.SymbolInstant_Cherry,
			PowerupScript.Identifier.FruitBasket,
			PowerupScript.Identifier.GeneralModCharm_Clicker,
			PowerupScript.Identifier.PuppetPersonalTrainer
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.GoldenSymbol_Cherry,
			PowerupScript.Identifier.SymbolInstant_Cherry,
			PowerupScript.Identifier.PuppetPersonalTrainer,
			PowerupScript.Identifier.FruitBasket
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.GoldenSymbol_Clover,
			PowerupScript.Identifier.SymbolInstant_Clover,
			PowerupScript.Identifier.Boardgame_M_Ditale
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.GoldenSymbol_Bell,
			PowerupScript.Identifier.SymbolInstant_Bell,
			PowerupScript.Identifier.Boardgame_M_FerroDaStiro
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.GoldenSymbol_Diamond,
			PowerupScript.Identifier.SymbolInstant_Diamond
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.GeneralModCharm_CrystalSphere,
			PowerupScript.Identifier.SymbolInstant_Diamond,
			PowerupScript.Identifier.PuppetFortuneTeller
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.GoldenSymbol_Treasure,
			PowerupScript.Identifier.SymbolInstant_Treasure
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.GeneralModCharm_CrystalSphere,
			PowerupScript.Identifier.SymbolInstant_Treasure,
			PowerupScript.Identifier.PuppetFortuneTeller
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.GoldenSymbol_Seven,
			PowerupScript.Identifier.SymbolInstant_Seven
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.GeneralModCharm_CrystalSphere,
			PowerupScript.Identifier.SymbolInstant_Seven,
			PowerupScript.Identifier.PuppetFortuneTeller
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Mushrooms,
			PowerupScript.Identifier.HornChilyRed,
			PowerupScript.Identifier.Pentacle,
			PowerupScript.Identifier.HornChilyGreen
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.VineSoupShroom,
			PowerupScript.Identifier.HornChilyRed,
			PowerupScript.Identifier.HornChilyGreen
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Mushrooms,
			PowerupScript.Identifier.HornChilyGreen,
			PowerupScript.Identifier.HornChilyGreen,
			PowerupScript.Identifier.GiantShroom
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.YellowStar,
			PowerupScript.Identifier.Rorschach,
			PowerupScript.Identifier.ExpiredMedicines,
			PowerupScript.Identifier.Necklace,
			PowerupScript.Identifier.EyeJar
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.YellowStar,
			PowerupScript.Identifier.AbstractPainting
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Depression,
			PowerupScript.Identifier.LocomotiveDiesel
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Depression,
			PowerupScript.Identifier.LocomotiveSteam
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Depression,
			PowerupScript.Identifier.Nose,
			PowerupScript.Identifier.Baphomet,
			PowerupScript.Identifier.Pareidolia
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier> { PowerupScript.Identifier.HornChilyRed });
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier> { PowerupScript.Identifier.HornChilyGreen });
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier> { PowerupScript.Identifier.FakeCoin });
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier> { PowerupScript.Identifier.AncientCoin });
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier> { PowerupScript.Identifier.ToyTrain });
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier> { PowerupScript.Identifier.RedCrystal });
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier> { PowerupScript.Identifier.CrystalSkull });
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.CrowBar,
			PowerupScript.Identifier.ShoppingCart,
			PowerupScript.Identifier.Dice_6,
			PowerupScript.Identifier.RitualBell
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.BellPepper,
			PowerupScript.Identifier.RitualBell,
			PowerupScript.Identifier.CrowBar
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.SuperCapacitor,
			PowerupScript.Identifier.CatTreats,
			PowerupScript.Identifier.BookOfShadows
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.DiscA,
			PowerupScript.Identifier.DiscB,
			PowerupScript.Identifier.DiscC,
			PowerupScript.Identifier.RedCrystal
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Megaphone,
			PowerupScript.Identifier.VoiceMailTape,
			PowerupScript.Identifier.DearDiary,
			PowerupScript.Identifier.Hole_Cross
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.ChastityBelt,
			PowerupScript.Identifier.PossessedPhone,
			PowerupScript.Identifier.HolyBible,
			PowerupScript.Identifier.Rosary
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Painkillers,
			PowerupScript.Identifier.EyeJar,
			PowerupScript.Identifier.Pareidolia
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Hole_Circle,
			PowerupScript.Identifier.MoneyBriefCase,
			PowerupScript.Identifier.FortuneCookie,
			PowerupScript.Identifier.CloverVoucher,
			PowerupScript.Identifier.MusicTape
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.HornChilyRed,
			PowerupScript.Identifier.RottenPepper,
			PowerupScript.Identifier.HornChilyGreen,
			PowerupScript.Identifier.HorseShoe
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.CloverBell,
			PowerupScript.Identifier.PhotoBook,
			PowerupScript.Identifier.Dice_4
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.GoldenKingMida,
			PowerupScript.Identifier.MoneyBriefCase,
			PowerupScript.Identifier.MusicTape
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Boardgame_C_Dealer,
			PowerupScript.Identifier.PissJar,
			PowerupScript.Identifier.BrokenCalculator
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier>
		{
			PowerupScript.Identifier.Boardgame_M_Capitalist,
			PowerupScript.Identifier.FideltyCard,
			PowerupScript.Identifier.CloverVoucher
		});
		StoreCapsuleScript._powerupChains.Add(new List<PowerupScript.Identifier> { PowerupScript.Identifier.Jimbo });
		if (!isNewGame)
		{
			StoreCapsuleScript.chainIndex_Array = instance.storeChainIndex_Array;
			StoreCapsuleScript.chainIndex_PowerupIdentifier = instance.storeChainIndex_PowerupIdentifier;
			StoreCapsuleScript.chainIndex_Array = Mathf.Clamp(StoreCapsuleScript.chainIndex_Array, 0, StoreCapsuleScript._powerupChains.Count - 1);
			StoreCapsuleScript.chainIndex_PowerupIdentifier = Mathf.Clamp(StoreCapsuleScript.chainIndex_PowerupIdentifier, 0, StoreCapsuleScript._powerupChains[StoreCapsuleScript.chainIndex_Array].Count - 1);
			return;
		}
		StoreCapsuleScript.PowerupChain_PickNewChain();
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00047D38 File Offset: 0x00045F38
	private static PowerupScript.Identifier PowerupChain_GetPowerup()
	{
		if (StoreCapsuleScript._powerupChains == null || StoreCapsuleScript._powerupChains.Count == 0)
		{
			return PowerupScript.Identifier.undefined;
		}
		if (R.Rng_StoreChains.Value < 0.1f)
		{
			if (PowerupScript.IsUnlocked(PowerupScript.Identifier.Skeleton_Head) && !StoreCapsuleScript.PowerupIsForbiddenToRestock(PowerupScript.Identifier.Skeleton_Head, true, true))
			{
				return PowerupScript.Identifier.Skeleton_Head;
			}
			return PowerupScript.Identifier.undefined;
		}
		else
		{
			List<PowerupScript.Identifier> list = StoreCapsuleScript._powerupChains[StoreCapsuleScript.chainIndex_Array];
			if (list.Count == 0)
			{
				return PowerupScript.Identifier.undefined;
			}
			return list[StoreCapsuleScript.chainIndex_PowerupIdentifier];
		}
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x0000CA39 File Offset: 0x0000AC39
	private static void PowerupChain_Next()
	{
		StoreCapsuleScript.chainIndex_PowerupIdentifier++;
		if (StoreCapsuleScript.chainIndex_PowerupIdentifier >= StoreCapsuleScript._powerupChains[StoreCapsuleScript.chainIndex_Array].Count)
		{
			StoreCapsuleScript.PowerupChain_PickNewChain();
		}
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x00047DA8 File Offset: 0x00045FA8
	private static void PowerupChain_PickNewChain()
	{
		int num = StoreCapsuleScript.chainIndex_Array;
		int i = 100;
		while (i > 0)
		{
			i--;
			StoreCapsuleScript.chainIndex_Array = R.Rng_StoreChains.Range(0, StoreCapsuleScript._powerupChains.Count);
			if (StoreCapsuleScript.chainIndex_Array != num)
			{
				int count = StoreCapsuleScript._powerupChains[StoreCapsuleScript.chainIndex_Array].Count;
				float num2 = 0f;
				for (int j = 0; j < count; j++)
				{
					if (StoreCapsuleScript.PowerupIsForbiddenToRestock(StoreCapsuleScript._powerupChains[StoreCapsuleScript.chainIndex_Array][j], false, true))
					{
						num2 += 1f;
					}
				}
				if (num2 < (float)count / 2.5f)
				{
					break;
				}
			}
		}
		StoreCapsuleScript.chainIndex_PowerupIdentifier = 0;
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x0000CA67 File Offset: 0x0000AC67
	public bool IsEnabled()
	{
		return (GameplayData.RunModifier_GetCurrent() != RunModifierScript.Identifier.smallerStore || this.id != 3 || this.isRefreshButton) && (this.useInDemo || !Master.IsDemo);
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x00047E54 File Offset: 0x00046054
	public static bool PowerupIsForbiddenToRestock(PowerupScript.Identifier identifier, bool considerDeadlineNumberCondition, bool considerEquipOrDrawerCondition)
	{
		if (StoreCapsuleScript.powerupsToNeverRestock.Contains(identifier))
		{
			return true;
		}
		if (considerDeadlineNumberCondition)
		{
			BigInteger bigInteger = GameplayData.DebtIndexGet();
			if ((GameplayMaster.instance == null || bigInteger == 0L) && StoreCapsuleScript.powerupsToNotRestockInFirstDeadline.Contains(identifier))
			{
				return true;
			}
			if ((GameplayMaster.instance == null || bigInteger < 2L) && StoreCapsuleScript.powerupsToNotRestockBefore666Deadline.Contains(identifier))
			{
				return true;
			}
		}
		if (considerEquipOrDrawerCondition && (PowerupScript.IsEquipped_Quick(identifier) || PowerupScript.IsInDrawer_Quick(identifier)))
		{
			return true;
		}
		if (!PowerupScript.IsUnlocked(identifier))
		{
			return true;
		}
		PowerupScript powerup_Quick = PowerupScript.GetPowerup_Quick(identifier);
		return powerup_Quick == null || PowerupScript.IsBanned(identifier, powerup_Quick.archetype) || powerup_Quick.archetype == PowerupScript.Archetype.sacred || (powerup_Quick.identifier == PowerupScript.Identifier.WeirdClock && PowerupScript.WeirdClockActivationsLimitReached());
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x00047F2C File Offset: 0x0004612C
	public static void Restock(bool isFirstRestockOfDeadline, bool refreshPlacement, PowerupScript[] predeterminedPowerups, bool isLoadingFromFile, bool saveAfterRestock)
	{
		bool flag = GameplayMaster.instance == null || GameplayData.DebtIndexGet() == 0L;
		RunModifierScript.Identifier identifier = GameplayData.RunModifier_GetCurrent();
		for (int i = 0; i < StoreCapsuleScript.storePowerups.Length; i++)
		{
			if (!(StoreCapsuleScript.storePowerups[i] == null) && StoreCapsuleScript.storePowerups[i].identifier == PowerupScript.Identifier.Jimbo)
			{
				GameplayData.Powerup_Jimbo_ReshuffleAndReset();
				break;
			}
		}
		for (int j = 0; j < StoreCapsuleScript.storePowerups.Length; j++)
		{
			StoreCapsuleScript.storePowerups[j] = null;
		}
		if (predeterminedPowerups != null && isLoadingFromFile)
		{
			for (int k = 0; k < 4; k++)
			{
				if (!StoreCapsuleScript.list[k].IsEnabled())
				{
					StoreCapsuleScript.storePowerups[k] = null;
					break;
				}
				StoreCapsuleScript.storePowerups[k] = predeterminedPowerups[k];
			}
		}
		else
		{
			int num = Mathf.Min(4, PowerupScript.list_NotBought.Count);
			if (num == 0)
			{
				Debug.LogWarning("StoreCapsuleScript: ReassortStore: maxN is 0. There are no powerups to add to the store!");
			}
			bool flag2 = false;
			for (int l = 0; l < num; l++)
			{
				if (!StoreCapsuleScript.list[l].IsEnabled())
				{
					StoreCapsuleScript.storePowerups[l] = null;
				}
				else
				{
					PowerupScript powerupScript = null;
					if (predeterminedPowerups != null && predeterminedPowerups.Length > l && predeterminedPowerups[l] != null)
					{
						powerupScript = predeterminedPowerups[l];
					}
					else
					{
						int num2 = 100;
						while (powerupScript == null && num2 > 0)
						{
							num2--;
							bool flag3 = false;
							bool flag4 = false;
							if (!flag2 && StoreCapsuleScript.chainIndex_Array >= 0)
							{
								flag3 = true;
								flag2 = true;
								flag4 = true;
							}
							for (;;)
							{
								if (!flag3)
								{
									goto IL_01E6;
								}
								PowerupScript.Identifier identifier2 = StoreCapsuleScript.PowerupChain_GetPowerup();
								if (identifier2 == PowerupScript.Identifier.undefined)
								{
									goto IL_01E6;
								}
								for (int m = 0; m < PowerupScript.list_NotBought.Count; m++)
								{
									if (!(PowerupScript.list_NotBought[m] == null) && PowerupScript.list_NotBought[m].identifier == identifier2 && !StoreCapsuleScript.PowerupIsForbiddenToRestock(PowerupScript.list_NotBought[m].identifier, false, true))
									{
										powerupScript = PowerupScript.list_NotBought[m];
									}
								}
								if (powerupScript == null)
								{
									goto IL_01E6;
								}
								IL_022F:
								if (!(powerupScript != null) || !StoreCapsuleScript.PowerupIsForbiddenToRestock(powerupScript.identifier, true, false))
								{
									break;
								}
								continue;
								IL_01E6:
								flag3 = false;
								int count = PowerupScript.list_NotBought.Count;
								StoreCapsuleScript._lastRandomIndex += R.Rng_Store.Range(0, count);
								StoreCapsuleScript._lastRandomIndex %= count;
								int lastRandomIndex = StoreCapsuleScript._lastRandomIndex;
								powerupScript = PowerupScript.list_NotBought[lastRandomIndex];
								goto IL_022F;
							}
							if (flag4)
							{
								StoreCapsuleScript.PowerupChain_Next();
							}
							if (powerupScript != null && Array.IndexOf<PowerupScript>(StoreCapsuleScript.storePowerups, powerupScript) >= 0)
							{
								powerupScript = null;
							}
							if (powerupScript != null)
							{
								int num3 = powerupScript.MaxBuyTimesGet();
								bool flag5 = num3 < 0;
								int num4 = GameplayData.Powerup_BoughtTimes_Get(powerupScript.identifier);
								if (!flag5 && num4 >= num3)
								{
									powerupScript = null;
								}
							}
							bool flag6 = num2 > 10 && !flag3;
							if (powerupScript != null && flag6 && powerupScript.StoreRerollEvaluate())
							{
								powerupScript = null;
							}
							if (powerupScript != null && flag && isFirstRestockOfDeadline && powerupScript.StartingPriceGet(true, false) > 4L && R.Rng_Store.Value < 0.75f)
							{
								powerupScript = null;
							}
						}
					}
					if (powerupScript != null)
					{
						StoreCapsuleScript.storePowerups[l] = powerupScript;
					}
				}
			}
			bool flag7 = identifier == RunModifierScript.Identifier.allCharmsStoreModded;
			bool flag8 = !flag7 && isFirstRestockOfDeadline && flag;
			int num5 = 0;
			while (!flag8 && num5 < StoreCapsuleScript.storePowerups.Length)
			{
				if (!(StoreCapsuleScript.storePowerups[num5] == null))
				{
					StoreCapsuleScript.storePowerups[num5].ModifierReEvaluate(true, flag7);
				}
				num5++;
			}
			int num6 = 0;
			while (num6 < 4 && num6 != StoreCapsuleScript.storePowerups.Length - 1)
			{
				for (int n = num6 + 1; n < StoreCapsuleScript.storePowerups.Length; n++)
				{
					if (!(StoreCapsuleScript.storePowerups[num6] == null) && !(StoreCapsuleScript.storePowerups[n] == null))
					{
						long num7 = StoreCapsuleScript.storePowerups[num6].StartingPriceGet(true, true);
						long num8 = StoreCapsuleScript.storePowerups[n].StartingPriceGet(true, true);
						if (num7 > num8)
						{
							PowerupScript powerupScript2 = StoreCapsuleScript.storePowerups[num6];
							StoreCapsuleScript.storePowerups[num6] = StoreCapsuleScript.storePowerups[n];
							StoreCapsuleScript.storePowerups[n] = powerupScript2;
						}
					}
				}
				num6++;
			}
		}
		StoreCapsuleScript.RefreshCostTextAll();
		foreach (StoreCapsuleScript storeCapsuleScript in StoreCapsuleScript.list)
		{
			storeCapsuleScript.bounceScript.SetBounceScale(0.1f);
		}
		if (refreshPlacement)
		{
			PowerupScript.RefreshPlacementAll();
		}
		GameplayData.StoreLuckReset();
		if (!isLoadingFromFile)
		{
			GameplayData.StoreTemporaryDiscountReset(true);
			GameplayData.StoreTemporaryDiscountPerSlotResetAll(true);
		}
		if (saveAfterRestock)
		{
			StoreCapsuleScript.RestockSave();
		}
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x0000CA96 File Offset: 0x0000AC96
	private static void RestockSave()
	{
		if (PlatformMaster.PlatformIsComputer())
		{
			Data.SaveGame(Data.GameSavingReason.storeBuyOrReroll, -1);
			return;
		}
		string text = "StoreCapsuleScript.RestockSave(): saving not implemented for current platform";
		Debug.LogError(text);
		ConsolePrompt.LogError(text, "", 0f);
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x000483DC File Offset: 0x000465DC
	public static void InitializeAll(bool isNewGame, bool refreshPlacement)
	{
		StoreCapsuleScript._lastRandomIndex = 0;
		StoreCapsuleScript.chainIndex_Array = -1;
		StoreCapsuleScript.chainIndex_PowerupIdentifier = -1;
		StoreCapsuleScript._initializing = true;
		GameplayData instance = GameplayData.Instance;
		int num = 0;
		while (num < StoreCapsuleScript.list.Count && num != StoreCapsuleScript.list.Count - 1)
		{
			for (int i = num + 1; i < StoreCapsuleScript.list.Count; i++)
			{
				if (StoreCapsuleScript.list[num].id > StoreCapsuleScript.list[i].id)
				{
					StoreCapsuleScript storeCapsuleScript = StoreCapsuleScript.list[num];
					StoreCapsuleScript.list[num] = StoreCapsuleScript.list[i];
					StoreCapsuleScript.list[i] = storeCapsuleScript;
				}
			}
			num++;
		}
		StoreCapsuleScript.PowerupsChain_Initilize(isNewGame);
		PowerupScript[] array;
		if (!isNewGame)
		{
			StoreCapsuleScript._lastRandomIndex = instance.storeLastRandomIndex;
			array = new PowerupScript[instance.storePowerups.Length];
			for (int j = 0; j < instance.storePowerups.Length; j++)
			{
				PowerupScript.Identifier identifier;
				if ((identifier = PlatformDataMaster.EnumEntryFromString<PowerupScript.Identifier>(instance.storePowerups[j], PowerupScript.Identifier.undefined)) >= PowerupScript.Identifier.Skeleton_Arm1 && identifier != PowerupScript.Identifier.undefined && identifier != PowerupScript.Identifier.count)
				{
					PowerupScript powerup_Quick = PowerupScript.GetPowerup_Quick(identifier);
					if (!(powerup_Quick == null))
					{
						array[j] = powerup_Quick;
					}
				}
			}
		}
		else
		{
			StoreCapsuleScript.initialCharmsSelection.Clear();
			StoreCapsuleScript.initialCharmsSelection.Add(PowerupScript.Identifier.FakeCoin);
			StoreCapsuleScript.initialCharmsSelection.Add(PowerupScript.Identifier.AncientCoin);
			StoreCapsuleScript.initialCharmsSelection.Add(PowerupScript.Identifier.CatTreats);
			StoreCapsuleScript.initialCharmsSelection.Add(PowerupScript.Identifier.HornChilyRed);
			StoreCapsuleScript.initialCharmsSelection.Add(PowerupScript.Identifier.HornChilyGreen);
			StoreCapsuleScript.initialCharmsSelection.Add(PowerupScript.Identifier.HamsaInverted);
			StoreCapsuleScript.initialCharmsSelection.Add(PowerupScript.Identifier.ToyTrain);
			StoreCapsuleScript.initialCharmsSelection.Add(PowerupScript.Identifier.RedCrystal);
			StoreCapsuleScript.initialCharmsSelection.Add(PowerupScript.Identifier.YellowStar);
			StoreCapsuleScript.initialCharmsSelection.Add(PowerupScript.Identifier.FortuneCookie);
			StoreCapsuleScript.initialCharmsSelection.Add(PowerupScript.Identifier.DiscA);
			StoreCapsuleScript.initialCharmsSelection.Add(PowerupScript.Identifier.DiscB);
			StoreCapsuleScript.initialCharmsSelection.Add(PowerupScript.Identifier.DiscC);
			for (int k = 0; k < StoreCapsuleScript.initialCharmsSelection.Count; k++)
			{
				if (StoreCapsuleScript.PowerupIsForbiddenToRestock(StoreCapsuleScript.initialCharmsSelection[k], false, true))
				{
					StoreCapsuleScript.initialCharmsSelection.RemoveAt(k);
					k--;
				}
			}
			array = new PowerupScript[1];
			for (int l = 0; l < 1; l++)
			{
				int num2 = R.Rng_Store.Range(0, StoreCapsuleScript.initialCharmsSelection.Count);
				array[l] = PowerupScript.GetPowerup_Quick(StoreCapsuleScript.initialCharmsSelection[num2]);
				StoreCapsuleScript.initialCharmsSelection.RemoveAt(num2);
			}
		}
		StoreCapsuleScript.Restock(isNewGame, refreshPlacement, array, !isNewGame, false);
		StoreCapsuleScript._initializing = false;
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x0004865C File Offset: 0x0004685C
	private void Awake()
	{
		StoreCapsuleScript.list.Add(this);
		this.bounceScript = base.GetComponentInChildren<BounceScript>();
		if (!this.isRefreshButton)
		{
			this.woodenPlanksHolder.SetActive(Master.IsDemo && !this.useInDemo);
		}
		if (this.id < 0)
		{
			Debug.LogError("StoreCapsuleScript: id is less than 0. Please define an id for gameObject: " + base.gameObject.name);
		}
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x0000CAC3 File Offset: 0x0000ACC3
	private void OnDestroy()
	{
		StoreCapsuleScript.list.Remove(this);
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x000486CC File Offset: 0x000468CC
	private void Update()
	{
		if (!this.isRefreshButton)
		{
			bool flag = !this.IsEnabled();
			if (this.woodenPlanksHolder.activeSelf != flag)
			{
				this.woodenPlanksHolder.SetActive(flag);
				if (flag)
				{
					StoreCapsuleScript.storePowerups[this.id] = null;
					this.RefreshCostText();
					PowerupScript.RefreshPlacementAll();
				}
			}
		}
	}

	// Token: 0x040007F3 RID: 2035
	public static List<StoreCapsuleScript> list = new List<StoreCapsuleScript>();

	// Token: 0x040007F4 RID: 2036
	public static PowerupScript[] storePowerups = new PowerupScript[4];

	// Token: 0x040007F5 RID: 2037
	public static int[] discountPerBoxes = new int[5];

	// Token: 0x040007F6 RID: 2038
	private const int BOXES_COUNT = 5;

	// Token: 0x040007F7 RID: 2039
	public const int BUYABLE_BOXES_COUNT = 4;

	// Token: 0x040007F8 RID: 2040
	public const int REFRESH_STORE_ID = 4;

	// Token: 0x040007F9 RID: 2041
	public const float BOUNCE_SCALE = 0.1f;

	// Token: 0x040007FA RID: 2042
	private const float CHAIN_COMBO_SKIP_CHANCHE = 0.1f;

	// Token: 0x040007FB RID: 2043
	public const int INITIAL_DEADLINE_LOWPRICE_THRESHOLD = 4;

	// Token: 0x040007FC RID: 2044
	public const float INITIAL_DEADLINE_LOWPRICE_REROLL_CHANCE = 0.75f;

	// Token: 0x040007FD RID: 2045
	public TextMeshPro costText;

	// Token: 0x040007FE RID: 2046
	private BounceScript bounceScript;

	// Token: 0x040007FF RID: 2047
	public GameObject woodenPlanksHolder;

	// Token: 0x04000800 RID: 2048
	public bool isRefreshButton;

	// Token: 0x04000801 RID: 2049
	public bool useInDemo = true;

	// Token: 0x04000802 RID: 2050
	public int id;

	// Token: 0x04000803 RID: 2051
	public static List<List<PowerupScript.Identifier>> _powerupChains = new List<List<PowerupScript.Identifier>>();

	// Token: 0x04000804 RID: 2052
	public static int chainIndex_Array = -1;

	// Token: 0x04000805 RID: 2053
	public static int chainIndex_PowerupIdentifier = -1;

	// Token: 0x04000806 RID: 2054
	private static List<PowerupScript.Identifier> powerupsToNeverRestock = new List<PowerupScript.Identifier>
	{
		PowerupScript.Identifier.Skeleton_Arm1,
		PowerupScript.Identifier.Skeleton_Arm2,
		PowerupScript.Identifier.Skeleton_Leg1,
		PowerupScript.Identifier.Skeleton_Leg2
	};

	// Token: 0x04000807 RID: 2055
	private static List<PowerupScript.Identifier> powerupsToNotRestockInFirstDeadline = new List<PowerupScript.Identifier>
	{
		PowerupScript.Identifier.HolyBible,
		PowerupScript.Identifier.Rosary,
		PowerupScript.Identifier.Baphomet,
		PowerupScript.Identifier.Cross,
		PowerupScript.Identifier.BookOfShadows,
		PowerupScript.Identifier.Gabibbh,
		PowerupScript.Identifier.MysticalTomato,
		PowerupScript.Identifier.RitualBell,
		PowerupScript.Identifier.CrystalSkull
	};

	// Token: 0x04000808 RID: 2056
	private static List<PowerupScript.Identifier> powerupsToNotRestockBefore666Deadline = new List<PowerupScript.Identifier> { PowerupScript.Identifier.EvilDeal };

	// Token: 0x04000809 RID: 2057
	public static int _lastRandomIndex = 0;

	// Token: 0x0400080A RID: 2058
	private static bool _initializing = false;

	// Token: 0x0400080B RID: 2059
	private static List<PowerupScript.Identifier> initialCharmsSelection = new List<PowerupScript.Identifier>();

	// Token: 0x0200007F RID: 127
	public enum BuyResult
	{
		// Token: 0x0400080D RID: 2061
		Success,
		// Token: 0x0400080E RID: 2062
		NotEnoughCurrency,
		// Token: 0x0400080F RID: 2063
		Empty,
		// Token: 0x04000810 RID: 2064
		InventoryFull
	}
}
