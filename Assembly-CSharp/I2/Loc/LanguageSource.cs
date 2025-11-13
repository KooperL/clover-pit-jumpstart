using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/Source")]
	[ExecuteInEditMode]
	public class LanguageSource : MonoBehaviour, ISerializationCallbackReceiver, ILanguageSource
	{
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0005D0A2 File Offset: 0x0005B2A2
		// (set) Token: 0x06000E88 RID: 3720 RVA: 0x0005D0AA File Offset: 0x0005B2AA
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

		// (add) Token: 0x06000E89 RID: 3721 RVA: 0x0005D0B4 File Offset: 0x0005B2B4
		// (remove) Token: 0x06000E8A RID: 3722 RVA: 0x0005D0EC File Offset: 0x0005B2EC
		public event LanguageSource.fnOnSourceUpdated Event_OnSourceUpdateFromGoogle;

		// Token: 0x06000E8B RID: 3723 RVA: 0x0005D121 File Offset: 0x0005B321
		private void Awake()
		{
			this.mSource.owner = this;
			this.mSource.Awake();
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0005D13A File Offset: 0x0005B33A
		private void OnDestroy()
		{
			this.NeverDestroy = false;
			if (!this.NeverDestroy)
			{
				this.mSource.OnDestroy();
			}
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x0005D158 File Offset: 0x0005B358
		public string GetSourceName()
		{
			string text = base.gameObject.name;
			Transform transform = base.transform.parent;
			while (transform)
			{
				text = transform.name + "_" + text;
				transform = transform.parent;
			}
			return text;
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0005D1A1 File Offset: 0x0005B3A1
		public void OnBeforeSerialize()
		{
			this.version = 1;
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x0005D1AC File Offset: 0x0005B3AC
		public void OnAfterDeserialize()
		{
			if (this.version == 0 || this.mSource == null)
			{
				this.mSource = new LanguageSourceData();
				this.mSource.owner = this;
				this.mSource.UserAgreesToHaveItOnTheScene = this.UserAgreesToHaveItOnTheScene;
				this.mSource.UserAgreesToHaveItInsideThePluginsFolder = this.UserAgreesToHaveItInsideThePluginsFolder;
				this.mSource.IgnoreDeviceLanguage = this.IgnoreDeviceLanguage;
				this.mSource._AllowUnloadingLanguages = this._AllowUnloadingLanguages;
				this.mSource.CaseInsensitiveTerms = this.CaseInsensitiveTerms;
				this.mSource.OnMissingTranslation = this.OnMissingTranslation;
				this.mSource.mTerm_AppName = this.mTerm_AppName;
				this.mSource.GoogleLiveSyncIsUptoDate = this.GoogleLiveSyncIsUptoDate;
				this.mSource.Google_WebServiceURL = this.Google_WebServiceURL;
				this.mSource.Google_SpreadsheetKey = this.Google_SpreadsheetKey;
				this.mSource.Google_SpreadsheetName = this.Google_SpreadsheetName;
				this.mSource.Google_LastUpdatedVersion = this.Google_LastUpdatedVersion;
				this.mSource.GoogleUpdateFrequency = this.GoogleUpdateFrequency;
				this.mSource.GoogleUpdateDelay = this.GoogleUpdateDelay;
				this.mSource.Event_OnSourceUpdateFromGoogle += this.Event_OnSourceUpdateFromGoogle;
				if (this.mLanguages != null && this.mLanguages.Count > 0)
				{
					this.mSource.mLanguages.Clear();
					this.mSource.mLanguages.AddRange(this.mLanguages);
					this.mLanguages.Clear();
				}
				if (this.Assets != null && this.Assets.Count > 0)
				{
					this.mSource.Assets.Clear();
					this.mSource.Assets.AddRange(this.Assets);
					this.Assets.Clear();
				}
				if (this.mTerms != null && this.mTerms.Count > 0)
				{
					this.mSource.mTerms.Clear();
					for (int i = 0; i < this.mTerms.Count; i++)
					{
						this.mSource.mTerms.Add(this.mTerms[i]);
					}
					this.mTerms.Clear();
				}
				this.version = 1;
				this.Event_OnSourceUpdateFromGoogle = null;
			}
		}

		public LanguageSourceData mSource = new LanguageSourceData();

		public int version;

		public bool NeverDestroy;

		public bool UserAgreesToHaveItOnTheScene;

		public bool UserAgreesToHaveItInsideThePluginsFolder;

		public bool GoogleLiveSyncIsUptoDate = true;

		public List<global::UnityEngine.Object> Assets = new List<global::UnityEngine.Object>();

		public string Google_WebServiceURL;

		public string Google_SpreadsheetKey;

		public string Google_SpreadsheetName;

		public string Google_LastUpdatedVersion;

		public LanguageSourceData.eGoogleUpdateFrequency GoogleUpdateFrequency = LanguageSourceData.eGoogleUpdateFrequency.Weekly;

		public float GoogleUpdateDelay = 5f;

		public List<LanguageData> mLanguages = new List<LanguageData>();

		public bool IgnoreDeviceLanguage;

		public LanguageSourceData.eAllowUnloadLanguages _AllowUnloadingLanguages;

		public List<TermData> mTerms = new List<TermData>();

		public bool CaseInsensitiveTerms;

		public LanguageSourceData.MissingTranslationAction OnMissingTranslation = LanguageSourceData.MissingTranslationAction.Fallback;

		public string mTerm_AppName;

		// (Invoke) Token: 0x0600146E RID: 5230
		public delegate void fnOnSourceUpdated(LanguageSourceData source, bool ReceivedNewData, string errorMsg);
	}
}
