using System;

namespace Panik
{
	public static class R
	{
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x00056334 File Offset: 0x00054534
		public static Rng Rng_RunMod
		{
			get
			{
				return GameplayData.Instance.rngRunMod;
			}
		}

		// (get) Token: 0x06000DB6 RID: 3510 RVA: 0x00056340 File Offset: 0x00054540
		public static Rng Rng_SymbolsMod
		{
			get
			{
				return GameplayData.Instance.rngSymbolsMod;
			}
		}

		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x0005634C File Offset: 0x0005454C
		public static Rng Rng_PowerupsMod
		{
			get
			{
				return GameplayData.Instance.rngPowerupsMod;
			}
		}

		// (get) Token: 0x06000DB8 RID: 3512 RVA: 0x00056358 File Offset: 0x00054558
		public static Rng Rng_SymbolsChance
		{
			get
			{
				return GameplayData.Instance.rngSymbolsChance;
			}
		}

		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x00056364 File Offset: 0x00054564
		public static Rng Rng_Cards
		{
			get
			{
				return GameplayData.Instance.rngCards;
			}
		}

		// (get) Token: 0x06000DBA RID: 3514 RVA: 0x00056370 File Offset: 0x00054570
		public static Rng Rng_PowerupsAll
		{
			get
			{
				return GameplayData.Instance.rngPowerupsAll;
			}
		}

		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x0005637C File Offset: 0x0005457C
		public static Rng Rng_Abilities
		{
			get
			{
				return GameplayData.Instance.rngAbilities;
			}
		}

		// (get) Token: 0x06000DBC RID: 3516 RVA: 0x00056388 File Offset: 0x00054588
		public static Rng Rng_Drawers
		{
			get
			{
				return GameplayData.Instance.rngDrawers;
			}
		}

		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x00056394 File Offset: 0x00054594
		public static Rng Rng_Store
		{
			get
			{
				return GameplayData.Instance.rngStore;
			}
		}

		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x000563A0 File Offset: 0x000545A0
		public static Rng Rng_StoreChains
		{
			get
			{
				return GameplayData.Instance.rngStoreChains;
			}
		}

		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x000563AC File Offset: 0x000545AC
		public static Rng Rng_Phone
		{
			get
			{
				return GameplayData.Instance.rngPhone;
			}
		}

		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x000563B8 File Offset: 0x000545B8
		public static Rng Rng_SlotMachineLuck
		{
			get
			{
				return GameplayData.Instance.rngSlotMachineLuck;
			}
		}

		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x000563C4 File Offset: 0x000545C4
		public static Rng Rng_666
		{
			get
			{
				return GameplayData.Instance.rng666;
			}
		}

		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x000563D0 File Offset: 0x000545D0
		public static Rng Rng_Garbage
		{
			get
			{
				return GameplayData.Instance.rngGarbage;
			}
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x000563DC File Offset: 0x000545DC
		public static Rng Rng_Powerup(PowerupScript.Identifier identifier)
		{
			return GameplayData.Instance.PowerupRngGet(identifier);
		}
	}
}
