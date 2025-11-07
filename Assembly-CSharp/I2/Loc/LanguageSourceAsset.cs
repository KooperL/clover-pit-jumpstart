using System;
using UnityEngine;

namespace I2.Loc
{
	[CreateAssetMenu(fileName = "I2Languages", menuName = "I2 Localization/LanguageSource", order = 1)]
	public class LanguageSourceAsset : ScriptableObject, ILanguageSource
	{
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x0005CC67 File Offset: 0x0005AE67
		// (set) Token: 0x06000E7B RID: 3707 RVA: 0x0005CC6F File Offset: 0x0005AE6F
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

		public LanguageSourceData mSource = new LanguageSourceData();
	}
}
