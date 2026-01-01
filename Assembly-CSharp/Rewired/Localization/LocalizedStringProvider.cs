using System;
using System.Collections.Generic;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Localization
{
	// Token: 0x02000182 RID: 386
	[AddComponentMenu("Rewired/Localization/Localized String Provider")]
	public class LocalizedStringProvider : LocalizedStringProviderBase
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06001184 RID: 4484 RVA: 0x000144CA File Offset: 0x000126CA
		// (set) Token: 0x06001185 RID: 4485 RVA: 0x000144D2 File Offset: 0x000126D2
		protected virtual Dictionary<string, string> dictionary
		{
			get
			{
				return this._dictionary;
			}
			set
			{
				this._dictionary = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06001186 RID: 4486 RVA: 0x000144DB File Offset: 0x000126DB
		// (set) Token: 0x06001187 RID: 4487 RVA: 0x000144E3 File Offset: 0x000126E3
		public virtual TextAsset localizedStringsFile
		{
			get
			{
				return this._localizedStringsFile;
			}
			set
			{
				this._localizedStringsFile = value;
				this.Reload();
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06001188 RID: 4488 RVA: 0x000144F2 File Offset: 0x000126F2
		protected override bool initialized
		{
			get
			{
				return this._initialized;
			}
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x000144FA File Offset: 0x000126FA
		protected override bool Initialize()
		{
			this._initialized = this.TryLoadLocalizedStringData();
			return this._initialized;
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x00075644 File Offset: 0x00073844
		protected virtual bool TryLoadLocalizedStringData()
		{
			this._dictionary.Clear();
			if (this._localizedStringsFile != null)
			{
				try
				{
					this._dictionary = JsonParser.FromJson<Dictionary<string, string>>(this._localizedStringsFile.text);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex);
				}
			}
			return this._dictionary.Count > 0;
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x0001450E File Offset: 0x0001270E
		protected override bool TryGetLocalizedString(string key, out string result)
		{
			if (!this._initialized)
			{
				result = null;
				return false;
			}
			return this._dictionary.TryGetValue(key, out result);
		}

		// Token: 0x04001299 RID: 4761
		[SerializeField]
		[Tooltip("A JSON file containing localizied string key value pairs.")]
		private TextAsset _localizedStringsFile;

		// Token: 0x0400129A RID: 4762
		[NonSerialized]
		private Dictionary<string, string> _dictionary = new Dictionary<string, string>();

		// Token: 0x0400129B RID: 4763
		[NonSerialized]
		private bool _initialized;
	}
}
