using System;

namespace I2.Loc
{
	// Token: 0x0200019B RID: 411
	public class TranslationJob : IDisposable
	{
		// Token: 0x06001217 RID: 4631 RVA: 0x000149C5 File Offset: 0x00012BC5
		public virtual TranslationJob.eJobState GetState()
		{
			return this.mJobState;
		}

		// Token: 0x06001218 RID: 4632 RVA: 0x0000774E File Offset: 0x0000594E
		public virtual void Dispose()
		{
		}

		// Token: 0x040012C8 RID: 4808
		public TranslationJob.eJobState mJobState;

		// Token: 0x0200019C RID: 412
		public enum eJobState
		{
			// Token: 0x040012CA RID: 4810
			Running,
			// Token: 0x040012CB RID: 4811
			Succeeded,
			// Token: 0x040012CC RID: 4812
			Failed
		}
	}
}
