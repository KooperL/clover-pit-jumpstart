using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	[Serializable]
	public class Rng
	{
		// Token: 0x06000DA7 RID: 3495 RVA: 0x0005619B File Offset: 0x0005439B
		public Rng(int _seed, uint _stateIndex)
		{
			this.SetState(_seed, _stateIndex);
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x000561AB File Offset: 0x000543AB
		public Rng(int seed)
		{
			this.SetState(seed, 0U);
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x000561BC File Offset: 0x000543BC
		public void SetState(int _seed, uint stateIndex)
		{
			this.seed = (uint)_seed;
			this.stateIndex = stateIndex;
			this.randomNumber = (uint)_seed;
			int num = 0;
			while ((long)num < (long)((ulong)stateIndex))
			{
				this.Raw();
				num++;
			}
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x000561F5 File Offset: 0x000543F5
		public void SetState(int _seed)
		{
			this.SetState(_seed, 0U);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x000561FF File Offset: 0x000543FF
		public uint SeedInternalGet()
		{
			return this.seed;
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x00056208 File Offset: 0x00054408
		public uint Raw()
		{
			this.randomNumber = (this.randomNumber + this.stateIndex) ^ Bit.ShiftRotateLeft(this.randomNumber, 31) ^ Bit.ShiftRotateLeft(this.randomNumber, 21) ^ Bit.ShiftRotateLeft(this.randomNumber, 13) ^ Bit.ShiftRotateLeft(this.randomNumber, 1) ^ this.seed;
			this.stateIndex += 1U;
			if (this.stateIndex >= 2147483646U)
			{
				this.stateIndex = 0U;
			}
			return this.randomNumber;
		}

		// (get) Token: 0x06000DAD RID: 3501 RVA: 0x0005628E File Offset: 0x0005448E
		public float Value
		{
			get
			{
				return this.Raw() % 4.2949673E+09f / 4.2949673E+09f;
			}
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x000562A4 File Offset: 0x000544A4
		public int Range(int min, int maxExcluded)
		{
			float value = this.Value;
			int num = maxExcluded - min;
			return min + Mathf.FloorToInt(value * (float)num);
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x000562C8 File Offset: 0x000544C8
		public float Range(float min, float maxExcluded)
		{
			float value = this.Value;
			float num = maxExcluded - min;
			return min + value * num;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x000562E5 File Offset: 0x000544E5
		public int NumI(int n)
		{
			return this.Range(0, n);
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x000562EF File Offset: 0x000544EF
		public float NumF(float n)
		{
			return this.Range(0f, n);
		}

		// (get) Token: 0x06000DB2 RID: 3506 RVA: 0x000562FD File Offset: 0x000544FD
		public bool FlipCoin
		{
			get
			{
				return this.Range(0, 2) >= 1;
			}
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0005630D File Offset: 0x0005450D
		public T Choose<T>(params T[] elements)
		{
			return elements[this.Range(0, elements.Length)];
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0005631F File Offset: 0x0005451F
		public T Choose<T>(List<T> elements)
		{
			return elements[this.Range(0, elements.Count)];
		}

		[SerializeField]
		private uint seed;

		[SerializeField]
		private uint stateIndex;

		[SerializeField]
		private uint randomNumber;
	}
}
