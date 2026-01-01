using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001A5 RID: 421
	[AddComponentMenu("I2/Localization/Source")]
	[ExecuteInEditMode]
	public class LanguageSource : MonoBehaviour, ISerializationCallbackReceiver, ILanguageSource
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06001238 RID: 4664 RVA: 0x00014B8C File Offset: 0x00012D8C
		// (set) Token: 0x06001239 RID: 4665 RVA: 0x00014B94 File Offset: 0x00012D94
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

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600123A RID: 4666 RVA: 0x0007B6E4 File Offset: 0x000798E4
		// (remove) Token: 0x0600123B RID: 4667 RVA: 0x0007B71C File Offset: 0x0007991C
		public event LanguageSource.fnOnSourceUpdated Event_OnSourceUpdateFromGoogle;

		// Token: 0x0600123C RID: 4668 RVA: 0x00014B9D File Offset: 0x00012D9D
		private void Awake()
		{
			this.mSource.owner = this;
			this.mSource.Awake();
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x00014BB6 File Offset: 0x00012DB6
		private void OnDestroy()
		{
			this.NeverDestroy = false;
			if (!this.NeverDestroy)
			{
				this.mSource.OnDestroy();
			}
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0007B754 File Offset: 0x00079954
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

		// Token: 0x0600123F RID: 4671 RVA: 0x00014BD2 File Offset: 0x00012DD2
		public void OnBeforeSerialize()
		{
			this.version = 1;
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x0007B7A0 File Offset: 0x000799A0
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

		// Token: 0x040012ED RID: 4845
		public LanguageSourceData mSource = new LanguageSourceData();

		// Token: 0x040012EE RID: 4846
		public int version;

		// Token: 0x040012EF RID: 4847
		public bool NeverDestroy;

		// Token: 0x040012F0 RID: 4848
		public bool UserAgreesToHaveItOnTheScene;

		// Token: 0x040012F1 RID: 4849
		public bool UserAgreesToHaveItInsideThePluginsFolder;

		// Token: 0x040012F2 RID: 4850
		public bool GoogleLiveSyncIsUptoDate = true;

		// Token: 0x040012F3 RID: 4851
		public List<global::UnityEngine.Object> Assets = new List<global::UnityEngine.Object>();

		// Token: 0x040012F4 RID: 4852
		public string Google_WebServiceURL;

		// Token: 0x040012F5 RID: 4853
		public string Google_SpreadsheetKey;

		// Token: 0x040012F6 RID: 4854
		public string Google_SpreadsheetName;

		// Token: 0x040012F7 RID: 4855
		public string Google_LastUpdatedVersion;

		// Token: 0x040012F8 RID: 4856
		public LanguageSourceData.eGoogleUpdateFrequency GoogleUpdateFrequency = LanguageSourceData.eGoogleUpdateFrequency.Weekly;

		// Token: 0x040012F9 RID: 4857
		public float GoogleUpdateDelay = 5f;

		// Token: 0x040012FB RID: 4859
		public List<LanguageData> mLanguages = new List<LanguageData>();

		// Token: 0x040012FC RID: 4860
		public bool IgnoreDeviceLanguage;

		// Token: 0x040012FD RID: 4861
		public LanguageSourceData.eAllowUnloadLanguages _AllowUnloadingLanguages;

		// Token: 0x040012FE RID: 4862
		public List<TermData> mTerms = new List<TermData>();

		// Token: 0x040012FF RID: 4863
		public bool CaseInsensitiveTerms;

		// Token: 0x04001300 RID: 4864
		public LanguageSourceData.MissingTranslationAction OnMissingTranslation = LanguageSourceData.MissingTranslationAction.Fallback;

		// Token: 0x04001301 RID: 4865
		public string mTerm_AppName;

		// Token: 0x020001A6 RID: 422
		// (Invoke) Token: 0x06001243 RID: 4675
		public delegate void fnOnSourceUpdated(LanguageSourceData source, bool ReceivedNewData, string errorMsg);
	}
}
