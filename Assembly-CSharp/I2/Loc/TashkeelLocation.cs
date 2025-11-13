using System;

namespace I2.Loc
{
	internal class TashkeelLocation
	{
		// Token: 0x0600103E RID: 4158 RVA: 0x00065274 File Offset: 0x00063474
		public TashkeelLocation(char tashkeel, int position)
		{
			this.tashkeel = tashkeel;
			this.position = position;
		}

		public char tashkeel;

		public int position;
	}
}
