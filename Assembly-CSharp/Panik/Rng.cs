using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	// Token: 0x02000178 RID: 376
	[Serializable]
	public class Rng
	{
		// Token: 0x06001134 RID: 4404 RVA: 0x000140D9 File Offset: 0x000122D9
		public Rng(int _seed, uint _stateIndex)
		{
			this.SetState(_seed, _stateIndex);
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x000140E9 File Offset: 0x000122E9
		public Rng(int seed)
		{
			this.SetState(seed, 0U);
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00073BA0 File Offset: 0x00071DA0
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

		// Token: 0x06001137 RID: 4407 RVA: 0x000140F9 File Offset: 0x000122F9
		public void SetState(int _seed)
		{
			this.SetState(_seed, 0U);
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00014103 File Offset: 0x00012303
		public uint SeedInternalGet()
		{
			return this.seed;
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00073BDC File Offset: 0x00071DDC
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

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600113A RID: 4410 RVA: 0x0001410B File Offset: 0x0001230B
		public float Value
		{
			get
			{
				return this.Raw() % 4.2949673E+09f / 4.2949673E+09f;
			}
		}

		// Token: 0x0600113B RID: 4411 RVA: 0x00073C64 File Offset: 0x00071E64
		public int Range(int min, int maxExcluded)
		{
			float value = this.Value;
			int num = maxExcluded - min;
			return min + Mathf.FloorToInt(value * (float)num);
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00073C88 File Offset: 0x00071E88
		public float Range(float min, float maxExcluded)
		{
			float value = this.Value;
			float num = maxExcluded - min;
			return min + value * num;
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00014121 File Offset: 0x00012321
		public int NumI(int n)
		{
			return this.Range(0, n);
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x0001412B File Offset: 0x0001232B
		public float NumF(float n)
		{
			return this.Range(0f, n);
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600113F RID: 4415 RVA: 0x00014139 File Offset: 0x00012339
		public bool FlipCoin
		{
			get
			{
				return this.Range(0, 2) >= 1;
			}
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00014149 File Offset: 0x00012349
		public T Choose<T>(params T[] elements)
		{
			return elements[this.Range(0, elements.Length)];
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x0001415B File Offset: 0x0001235B
		public T Choose<T>(List<T> elements)
		{
			return elements[this.Range(0, elements.Count)];
		}

		// Token: 0x0400122D RID: 4653
		[SerializeField]
		private uint seed;

		// Token: 0x0400122E RID: 4654
		[SerializeField]
		private uint stateIndex;

		// Token: 0x0400122F RID: 4655
		[SerializeField]
		private uint randomNumber;
	}
}
