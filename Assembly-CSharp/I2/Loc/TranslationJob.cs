using System;

namespace I2.Loc
{
	public class TranslationJob : IDisposable
	{
		// Token: 0x06000E6B RID: 3691 RVA: 0x0005C792 File Offset: 0x0005A992
		public virtual TranslationJob.eJobState GetState()
		{
			return this.mJobState;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0005C79A File Offset: 0x0005A99A
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
