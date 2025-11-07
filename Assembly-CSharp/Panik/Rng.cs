using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	[Serializable]
	public class Rng
	{
		// Token: 0x06000D90 RID: 3472 RVA: 0x000559BF File Offset: 0x00053BBF
		public Rng(int _seed, uint _stateIndex)
		{
			this.SetState(_seed, _stateIndex);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x000559CF File Offset: 0x00053BCF
		public Rng(int seed)
		{
			this.SetState(seed, 0U);
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x000559E0 File Offset: 0x00053BE0
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

		// Token: 0x06000D93 RID: 3475 RVA: 0x00055A19 File Offset: 0x00053C19
		public void SetState(int _seed)
		{
			this.SetState(_seed, 0U);
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00055A23 File Offset: 0x00053C23
		public uint SeedInternalGet()
		{
			return this.seed;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00055A2C File Offset: 0x00053C2C
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

		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x00055AB2 File Offset: 0x00053CB2
		public float Value
		{
			get
			{
				return this.Raw() % 4.2949673E+09f / 4.2949673E+09f;
			}
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00055AC8 File Offset: 0x00053CC8
		public int Range(int min, int maxExcluded)
		{
			float value = this.Value;
			int num = maxExcluded - min;
			return min + Mathf.FloorToInt(value * (float)num);
		}

		// Token: 0x06000D98 RID: 3480 RVA: 0x00055AEC File Offset: 0x00053CEC
		public float Range(float min, float maxExcluded)
		{
			float value = this.Value;
			float num = maxExcluded - min;
			return min + value * num;
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00055B09 File Offset: 0x00053D09
		public int NumI(int n)
		{
			return this.Range(0, n);
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00055B13 File Offset: 0x00053D13
		public float NumF(float n)
		{
			return this.Range(0f, n);
		}

		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x00055B21 File Offset: 0x00053D21
		public bool FlipCoin
		{
			get
			{
				return this.Range(0, 2) >= 1;
			}
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00055B31 File Offset: 0x00053D31
		public T Choose<T>(params T[] elements)
		{
			return elements[this.Range(0, elements.Length)];
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00055B43 File Offset: 0x00053D43
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
