using System;

namespace I2.Loc
{
	public class TranslationJob : IDisposable
	{
		// Token: 0x06000E54 RID: 3668 RVA: 0x0005BFB6 File Offset: 0x0005A1B6
		public virtual TranslationJob.eJobState GetState()
		{
			return this.mJobState;
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x0005BFBE File Offset: 0x0005A1BE
		public virtual void Dispose()
		{
		}

		public TranslationJob.eJobState mJobState;

		public enum eJobState
		{
			Running,
			Succeeded,
			Failed
		}
	}
}
