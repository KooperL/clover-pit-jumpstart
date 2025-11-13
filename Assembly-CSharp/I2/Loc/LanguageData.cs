using System;

namespace I2.Loc
{
	[Serializable]
	public class LanguageData
	{
		// Token: 0x06000E80 RID: 3712 RVA: 0x0005D004 File Offset: 0x0005B204
		public bool IsEnabled()
		{
			return (this.Flags & 1) == 0;
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x0005D011 File Offset: 0x0005B211
		public void SetEnabled(bool bEnabled)
		{
			if (bEnabled)
			{
				this.Flags = (byte)((int)this.Flags & -2);
				return;
			}
			this.Flags |= 1;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x0005D036 File Offset: 0x0005B236
		public bool IsLoaded()
		{
			return (this.Flags & 4) == 0;
		}

		// Token: 0x06000E83 RID: 3715 RVA: 0x0005D043 File Offset: 0x0005B243
		public bool CanBeUnloaded()
		{
			return (this.Flags & 2) == 0;
		}

		// Token: 0x06000E84 RID: 3716 RVA: 0x0005D050 File Offset: 0x0005B250
		public void SetLoaded(bool loaded)
		{
			if (loaded)
			{
				this.Flags = (byte)((int)this.Flags & -5);
				return;
			}
			this.Flags |= 4;
		}

		// Token: 0x06000E85 RID: 3717 RVA: 0x0005D075 File Offset: 0x0005B275
		public void SetCanBeUnLoaded(bool allowUnloading)
		{
			if (allowUnloading)
			{
				this.Flags = (byte)((int)this.Flags & -3);
				return;
			}
			this.Flags |= 2;
		}

		public string Name;

		public string Code;

		public byte Flags;

		[NonSerialized]
		public bool Compressed;
	}
}
