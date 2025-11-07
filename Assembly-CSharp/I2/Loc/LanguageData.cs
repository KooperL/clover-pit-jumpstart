using System;

namespace I2.Loc
{
	[Serializable]
	public class LanguageData
	{
		// Token: 0x06000E69 RID: 3689 RVA: 0x0005C828 File Offset: 0x0005AA28
		public bool IsEnabled()
		{
			return (this.Flags & 1) == 0;
		}

		// Token: 0x06000E6A RID: 3690 RVA: 0x0005C835 File Offset: 0x0005AA35
		public void SetEnabled(bool bEnabled)
		{
			if (bEnabled)
			{
				this.Flags = (byte)((int)this.Flags & -2);
				return;
			}
			this.Flags |= 1;
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x0005C85A File Offset: 0x0005AA5A
		public bool IsLoaded()
		{
			return (this.Flags & 4) == 0;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0005C867 File Offset: 0x0005AA67
		public bool CanBeUnloaded()
		{
			return (this.Flags & 2) == 0;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0005C874 File Offset: 0x0005AA74
		public void SetLoaded(bool loaded)
		{
			if (loaded)
			{
				this.Flags = (byte)((int)this.Flags & -5);
				return;
			}
			this.Flags |= 4;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x0005C899 File Offset: 0x0005AA99
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
