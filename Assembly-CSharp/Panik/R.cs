using System;

namespace Panik
{
	// Token: 0x02000179 RID: 377
	public static class R
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x00014170 File Offset: 0x00012370
		public static Rng Rng_RunMod
		{
			get
			{
				return GameplayData.Instance.rngRunMod;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06001143 RID: 4419 RVA: 0x0001417C File Offset: 0x0001237C
		public static Rng Rng_SymbolsMod
		{
			get
			{
				return GameplayData.Instance.rngSymbolsMod;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x00014188 File Offset: 0x00012388
		public static Rng Rng_PowerupsMod
		{
			get
			{
				return GameplayData.Instance.rngPowerupsMod;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06001145 RID: 4421 RVA: 0x00014194 File Offset: 0x00012394
		public static Rng Rng_SymbolsChance
		{
			get
			{
				return GameplayData.Instance.rngSymbolsChance;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x000141A0 File Offset: 0x000123A0
		public static Rng Rng_Cards
		{
			get
			{
				return GameplayData.Instance.rngCards;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06001147 RID: 4423 RVA: 0x000141AC File Offset: 0x000123AC
		public static Rng Rng_PowerupsAll
		{
			get
			{
				return GameplayData.Instance.rngPowerupsAll;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x000141B8 File Offset: 0x000123B8
		public static Rng Rng_Abilities
		{
			get
			{
				return GameplayData.Instance.rngAbilities;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06001149 RID: 4425 RVA: 0x000141C4 File Offset: 0x000123C4
		public static Rng Rng_Drawers
		{
			get
			{
				return GameplayData.Instance.rngDrawers;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x000141D0 File Offset: 0x000123D0
		public static Rng Rng_Store
		{
			get
			{
				return GameplayData.Instance.rngStore;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600114B RID: 4427 RVA: 0x000141DC File Offset: 0x000123DC
		public static Rng Rng_StoreChains
		{
			get
			{
				return GameplayData.Instance.rngStoreChains;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x000141E8 File Offset: 0x000123E8
		public static Rng Rng_Phone
		{
			get
			{
				return GameplayData.Instance.rngPhone;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x000141F4 File Offset: 0x000123F4
		public static Rng Rng_SlotMachineLuck
		{
			get
			{
				return GameplayData.Instance.rngSlotMachineLuck;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x00014200 File Offset: 0x00012400
		public static Rng Rng_666
		{
			get
			{
				return GameplayData.Instance.rng666;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600114F RID: 4431 RVA: 0x0001420C File Offset: 0x0001240C
		public static Rng Rng_Garbage
		{
			get
			{
				return GameplayData.Instance.rngGarbage;
			}
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x00014218 File Offset: 0x00012418
		public static Rng Rng_Powerup(PowerupScript.Identifier identifier)
		{
			return GameplayData.Instance.PowerupRngGet(identifier);
		}
	}
}
