using System;

namespace Panik
{
	public static class R
	{
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x00055B58 File Offset: 0x00053D58
		public static Rng Rng_RunMod
		{
			get
			{
				return GameplayData.Instance.rngRunMod;
			}
		}

		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x00055B64 File Offset: 0x00053D64
		public static Rng Rng_SymbolsMod
		{
			get
			{
				return GameplayData.Instance.rngSymbolsMod;
			}
		}

		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x00055B70 File Offset: 0x00053D70
		public static Rng Rng_PowerupsMod
		{
			get
			{
				return GameplayData.Instance.rngPowerupsMod;
			}
		}

		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x00055B7C File Offset: 0x00053D7C
		public static Rng Rng_SymbolsChance
		{
			get
			{
				return GameplayData.Instance.rngSymbolsChance;
			}
		}

		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x00055B88 File Offset: 0x00053D88
		public static Rng Rng_Cards
		{
			get
			{
				return GameplayData.Instance.rngCards;
			}
		}

		// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x00055B94 File Offset: 0x00053D94
		public static Rng Rng_PowerupsAll
		{
			get
			{
				return GameplayData.Instance.rngPowerupsAll;
			}
		}

		// (get) Token: 0x06000DA4 RID: 3492 RVA: 0x00055BA0 File Offset: 0x00053DA0
		public static Rng Rng_Abilities
		{
			get
			{
				return GameplayData.Instance.rngAbilities;
			}
		}

		// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x00055BAC File Offset: 0x00053DAC
		public static Rng Rng_Drawers
		{
			get
			{
				return GameplayData.Instance.rngDrawers;
			}
		}

		// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x00055BB8 File Offset: 0x00053DB8
		public static Rng Rng_Store
		{
			get
			{
				return GameplayData.Instance.rngStore;
			}
		}

		// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x00055BC4 File Offset: 0x00053DC4
		public static Rng Rng_StoreChains
		{
			get
			{
				return GameplayData.Instance.rngStoreChains;
			}
		}

		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x00055BD0 File Offset: 0x00053DD0
		public static Rng Rng_Phone
		{
			get
			{
				return GameplayData.Instance.rngPhone;
			}
		}

		// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x00055BDC File Offset: 0x00053DDC
		public static Rng Rng_SlotMachineLuck
		{
			get
			{
				return GameplayData.Instance.rngSlotMachineLuck;
			}
		}

		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x00055BE8 File Offset: 0x00053DE8
		public static Rng Rng_666
		{
			get
			{
				return GameplayData.Instance.rng666;
			}
		}

		// (get) Token: 0x06000DAB RID: 3499 RVA: 0x00055BF4 File Offset: 0x00053DF4
		public static Rng Rng_Garbage
		{
			get
			{
				return GameplayData.Instance.rngGarbage;
			}
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00055C00 File Offset: 0x00053E00
		public static Rng Rng_Powerup(PowerupScript.Identifier identifier)
		{
			return GameplayData.Instance.PowerupRngGet(identifier);
		}
	}
}
