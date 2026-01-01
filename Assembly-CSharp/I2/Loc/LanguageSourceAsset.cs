using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001A7 RID: 423
	[CreateAssetMenu(fileName = "I2Languages", menuName = "I2 Localization/LanguageSource", order = 1)]
	public class LanguageSourceAsset : ScriptableObject, ILanguageSource
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06001246 RID: 4678 RVA: 0x00014BDB File Offset: 0x00012DDB
		// (set) Token: 0x06001247 RID: 4679 RVA: 0x00014BE3 File Offset: 0x00012DE3
		public LanguageSourceData SourceData
		{
			get
			{
				return this.mSource;
			}
			set
			{
				this.mSource = value;
			}
		}

		// Token: 0x04001302 RID: 4866
		public LanguageSourceData mSource = new LanguageSourceData();
	}
}
