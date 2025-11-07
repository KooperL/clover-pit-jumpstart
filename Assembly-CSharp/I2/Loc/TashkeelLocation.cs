using System;

namespace I2.Loc
{
	internal class TashkeelLocation
	{
		// Token: 0x06001027 RID: 4135 RVA: 0x00064A98 File Offset: 0x00062C98
		public TashkeelLocation(char tashkeel, int position)
		{
			this.tashkeel = tashkeel;
			this.position = position;
		}

		public char tashkeel;

		public int position;
	}
}
