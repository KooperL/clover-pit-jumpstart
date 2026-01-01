using System;

namespace I2.Loc
{
	// Token: 0x020001EE RID: 494
	internal class TashkeelLocation
	{
		// Token: 0x06001432 RID: 5170 RVA: 0x000158D9 File Offset: 0x00013AD9
		public TashkeelLocation(char tashkeel, int position)
		{
			this.tashkeel = tashkeel;
			this.position = position;
		}

		// Token: 0x0400142D RID: 5165
		public char tashkeel;

		// Token: 0x0400142E RID: 5166
		public int position;
	}
}
