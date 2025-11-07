using System;
using System.Collections.Generic;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Localization
{
	[AddComponentMenu("Rewired/Localization/Localized String Provider")]
	public class LocalizedStringProvider : LocalizedStringProviderBase
	{
		// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x00056245 File Offset: 0x00054445
		// (set) Token: 0x06000DD2 RID: 3538 RVA: 0x0005624D File Offset: 0x0005444D
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

		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x00056256 File Offset: 0x00054456
		// (set) Token: 0x06000DD4 RID: 3540 RVA: 0x0005625E File Offset: 0x0005445E
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

		// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x0005626D File Offset: 0x0005446D
		protected override bool initialized
		{
			get
			{
				return this._initialized;
			}
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00056275 File Offset: 0x00054475
		protected override bool Initialize()
		{
			this._initialized = this.TryLoadLocalizedStringData();
			return this._initialized;
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0005628C File Offset: 0x0005448C
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

		// Token: 0x06000DD8 RID: 3544 RVA: 0x000562F0 File Offset: 0x000544F0
		protected override bool TryGetLocalizedString(string key, out string result)
		{
			if (!this._initialized)
			{
				result = null;
				return false;
			}
			return this._dictionary.TryGetValue(key, out result);
		}

		[SerializeField]
		[Tooltip("A JSON file containing localizied string key value pairs.")]
		private TextAsset _localizedStringsFile;

		[NonSerialized]
		private Dictionary<string, string> _dictionary = new Dictionary<string, string>();

		[NonSerialized]
		private bool _initialized;
	}
}
