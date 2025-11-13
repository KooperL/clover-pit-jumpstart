using System;
using System.Collections.Generic;
using Rewired.Utils.Libraries.TinyJson;
using UnityEngine;

namespace Rewired.Localization
{
	[AddComponentMenu("Rewired/Localization/Localized String Provider")]
	public class LocalizedStringProvider : LocalizedStringProviderBase
	{
		// (get) Token: 0x06000DE8 RID: 3560 RVA: 0x00056A21 File Offset: 0x00054C21
		// (set) Token: 0x06000DE9 RID: 3561 RVA: 0x00056A29 File Offset: 0x00054C29
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

		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x00056A32 File Offset: 0x00054C32
		// (set) Token: 0x06000DEB RID: 3563 RVA: 0x00056A3A File Offset: 0x00054C3A
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

		// (get) Token: 0x06000DEC RID: 3564 RVA: 0x00056A49 File Offset: 0x00054C49
		protected override bool initialized
		{
			get
			{
				return this._initialized;
			}
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00056A51 File Offset: 0x00054C51
		protected override bool Initialize()
		{
			this._initialized = this.TryLoadLocalizedStringData();
			return this._initialized;
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00056A68 File Offset: 0x00054C68
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

		// Token: 0x06000DEF RID: 3567 RVA: 0x00056ACC File Offset: 0x00054CCC
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
