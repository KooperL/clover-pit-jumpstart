using System;

namespace I2.Loc
{
	// Token: 0x020001A4 RID: 420
	[Serializable]
	public class LanguageData
	{
		// Token: 0x06001231 RID: 4657 RVA: 0x00014AF6 File Offset: 0x00012CF6
		public bool IsEnabled()
		{
			return (this.Flags & 1) == 0;
		}

		// Token: 0x06001232 RID: 4658 RVA: 0x00014B03 File Offset: 0x00012D03
		public void SetEnabled(bool bEnabled)
		{
			if (bEnabled)
			{
				this.Flags = (byte)((int)this.Flags & -2);
				return;
			}
			this.Flags |= 1;
		}

		// Token: 0x06001233 RID: 4659 RVA: 0x00014B28 File Offset: 0x00012D28
		public bool IsLoaded()
		{
			return (this.Flags & 4) == 0;
		}

		// Token: 0x06001234 RID: 4660 RVA: 0x00014B35 File Offset: 0x00012D35
		public bool CanBeUnloaded()
		{
			return (this.Flags & 2) == 0;
		}

		// Token: 0x06001235 RID: 4661 RVA: 0x00014B42 File Offset: 0x00012D42
		public void SetLoaded(bool loaded)
		{
			if (loaded)
			{
				this.Flags = (byte)((int)this.Flags & -5);
				return;
			}
			this.Flags |= 4;
		}

		// Token: 0x06001236 RID: 4662 RVA: 0x00014B67 File Offset: 0x00012D67
		public void SetCanBeUnLoaded(bool allowUnloading)
		{
			if (allowUnloading)
			{
				this.Flags = (byte)((int)this.Flags & -3);
				return;
			}
			this.Flags |= 2;
		}

		// Token: 0x040012E9 RID: 4841
		public string Name;

		// Token: 0x040012EA RID: 4842
		public string Code;

		// Token: 0x040012EB RID: 4843
		public byte Flags;

		// Token: 0x040012EC RID: 4844
		[NonSerialized]
		public bool Compressed;
	}
}
