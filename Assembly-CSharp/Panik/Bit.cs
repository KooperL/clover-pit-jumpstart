using System;

namespace Panik
{
	public static class Bit
	{
		// Token: 0x06000D52 RID: 3410 RVA: 0x00054C76 File Offset: 0x00052E76
		public static int Set(ref int numReference, int bitPosition, bool value)
		{
			if (value)
			{
				return Bit.SetOne(ref numReference, bitPosition);
			}
			return Bit.SetZero(ref numReference, bitPosition);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00054C8A File Offset: 0x00052E8A
		public static int SetOne(ref int numReference, int bitPosition)
		{
			numReference |= 1 << bitPosition;
			return numReference;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00054C99 File Offset: 0x00052E99
		public static int SetZero(ref int numReference, int bitPosition)
		{
			numReference &= ~(1 << bitPosition);
			return numReference;
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00054CA9 File Offset: 0x00052EA9
		public static bool GetBit(int num, int bitPosition)
		{
			return ((num >> bitPosition) & 1) != 0;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00054CB6 File Offset: 0x00052EB6
		public static int GetBitInt(int num, int bitPosition)
		{
			if (((num >> bitPosition) & 1) != 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00054CC5 File Offset: 0x00052EC5
		public static bool IsOne(int num, int bitPosition)
		{
			return (num & (1 << bitPosition)) != 0;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00054CD2 File Offset: 0x00052ED2
		public static bool IsZero(int num, int bitPosition)
		{
			return (num & (1 << bitPosition)) == 0;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00054CDF File Offset: 0x00052EDF
		public static uint ShiftRotateLeft(uint originalNumber, int shiftN)
		{
			return (originalNumber << shiftN) | (originalNumber >> 32 - shiftN);
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00054CF1 File Offset: 0x00052EF1
		public static uint ShiftRotateRight(uint originalNumber, int shiftN)
		{
			return (originalNumber >> shiftN) | (originalNumber << 32 - shiftN);
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00054D04 File Offset: 0x00052F04
		public static byte[] BufferFromArray<T>(T[] array, uint sizeOfSingleElement)
		{
			byte[] array2 = new byte[(long)array.Length * (long)((ulong)sizeOfSingleElement)];
			Buffer.BlockCopy(array, 0, array2, 0, array2.Length);
			return array2;
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00054D2C File Offset: 0x00052F2C
		public static T[] BufferToArray<T>(byte[] byteArray, uint sizeOfSingleElement)
		{
			T[] array = new T[(long)byteArray.Length / (long)((ulong)sizeOfSingleElement)];
			Buffer.BlockCopy(byteArray, 0, array, 0, byteArray.Length);
			return array;
		}
	}
}
