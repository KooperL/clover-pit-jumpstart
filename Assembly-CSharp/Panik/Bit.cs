using System;

namespace Panik
{
	// Token: 0x02000170 RID: 368
	public static class Bit
	{
		// Token: 0x060010F2 RID: 4338 RVA: 0x00013DF3 File Offset: 0x00011FF3
		public static int Set(ref int numReference, int bitPosition, bool value)
		{
			if (value)
			{
				return Bit.SetOne(ref numReference, bitPosition);
			}
			return Bit.SetZero(ref numReference, bitPosition);
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x00013E07 File Offset: 0x00012007
		public static int SetOne(ref int numReference, int bitPosition)
		{
			numReference |= 1 << bitPosition;
			return numReference;
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x00013E16 File Offset: 0x00012016
		public static int SetZero(ref int numReference, int bitPosition)
		{
			numReference &= ~(1 << bitPosition);
			return numReference;
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x00013E26 File Offset: 0x00012026
		public static bool GetBit(int num, int bitPosition)
		{
			return ((num >> bitPosition) & 1) != 0;
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x00013E33 File Offset: 0x00012033
		public static int GetBitInt(int num, int bitPosition)
		{
			if (((num >> bitPosition) & 1) != 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x00013E42 File Offset: 0x00012042
		public static bool IsOne(int num, int bitPosition)
		{
			return (num & (1 << bitPosition)) != 0;
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00013E4F File Offset: 0x0001204F
		public static bool IsZero(int num, int bitPosition)
		{
			return (num & (1 << bitPosition)) == 0;
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00013E5C File Offset: 0x0001205C
		public static uint ShiftRotateLeft(uint originalNumber, int shiftN)
		{
			return (originalNumber << shiftN) | (originalNumber >> 32 - shiftN);
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x00013E6E File Offset: 0x0001206E
		public static uint ShiftRotateRight(uint originalNumber, int shiftN)
		{
			return (originalNumber >> shiftN) | (originalNumber << 32 - shiftN);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0007314C File Offset: 0x0007134C
		public static byte[] BufferFromArray<T>(T[] array, uint sizeOfSingleElement)
		{
			byte[] array2 = new byte[(long)array.Length * (long)((ulong)sizeOfSingleElement)];
			Buffer.BlockCopy(array, 0, array2, 0, array2.Length);
			return array2;
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00073174 File Offset: 0x00071374
		public static T[] BufferToArray<T>(byte[] byteArray, uint sizeOfSingleElement)
		{
			T[] array = new T[(long)byteArray.Length / (long)((ulong)sizeOfSingleElement)];
			Buffer.BlockCopy(byteArray, 0, array, 0, byteArray.Length);
			return array;
		}
	}
}
