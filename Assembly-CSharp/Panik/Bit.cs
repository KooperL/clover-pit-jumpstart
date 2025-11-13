using System;

namespace Panik
{
	public static class Bit
	{
		// Token: 0x06000D69 RID: 3433 RVA: 0x00055452 File Offset: 0x00053652
		public static int Set(ref int numReference, int bitPosition, bool value)
		{
			if (value)
			{
				return Bit.SetOne(ref numReference, bitPosition);
			}
			return Bit.SetZero(ref numReference, bitPosition);
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x00055466 File Offset: 0x00053666
		public static int SetOne(ref int numReference, int bitPosition)
		{
			numReference |= 1 << bitPosition;
			return numReference;
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00055475 File Offset: 0x00053675
		public static int SetZero(ref int numReference, int bitPosition)
		{
			numReference &= ~(1 << bitPosition);
			return numReference;
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00055485 File Offset: 0x00053685
		public static bool GetBit(int num, int bitPosition)
		{
			return ((num >> bitPosition) & 1) != 0;
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00055492 File Offset: 0x00053692
		public static int GetBitInt(int num, int bitPosition)
		{
			if (((num >> bitPosition) & 1) != 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x000554A1 File Offset: 0x000536A1
		public static bool IsOne(int num, int bitPosition)
		{
			return (num & (1 << bitPosition)) != 0;
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x000554AE File Offset: 0x000536AE
		public static bool IsZero(int num, int bitPosition)
		{
			return (num & (1 << bitPosition)) == 0;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x000554BB File Offset: 0x000536BB
		public static uint ShiftRotateLeft(uint originalNumber, int shiftN)
		{
			return (originalNumber << shiftN) | (originalNumber >> 32 - shiftN);
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x000554CD File Offset: 0x000536CD
		public static uint ShiftRotateRight(uint originalNumber, int shiftN)
		{
			return (originalNumber >> shiftN) | (originalNumber << 32 - shiftN);
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x000554E0 File Offset: 0x000536E0
		public static byte[] BufferFromArray<T>(T[] array, uint sizeOfSingleElement)
		{
			byte[] array2 = new byte[(long)array.Length * (long)((ulong)sizeOfSingleElement)];
			Buffer.BlockCopy(array, 0, array2, 0, array2.Length);
			return array2;
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00055508 File Offset: 0x00053708
		public static T[] BufferToArray<T>(byte[] byteArray, uint sizeOfSingleElement)
		{
			T[] array = new T[(long)byteArray.Length / (long)((ulong)sizeOfSingleElement)];
			Buffer.BlockCopy(byteArray, 0, array, 0, byteArray.Length);
			return array;
		}
	}
}
