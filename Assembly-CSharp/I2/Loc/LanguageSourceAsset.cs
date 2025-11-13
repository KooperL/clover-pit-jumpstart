using System;
using UnityEngine;

namespace I2.Loc
{
	[CreateAssetMenu(fileName = "I2Languages", menuName = "I2 Localization/LanguageSource", order = 1)]
	public class LanguageSourceAsset : ScriptableObject, ILanguageSource
	{
		// (get) Token: 0x06000E91 RID: 3729 RVA: 0x0005D443 File Offset: 0x0005B643
		// (set) Token: 0x06000E92 RID: 3730 RVA: 0x0005D44B File Offset: 0x0005B64B
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
