using System;
using Rewired.Interfaces;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.Localization
{
	public abstract class LocalizedStringProviderBase : MonoBehaviour, ILocalizedStringProvider
	{
		// (get) Token: 0x06000DF1 RID: 3569 RVA: 0x00056AFB File Offset: 0x00054CFB
		// (set) Token: 0x06000DF2 RID: 3570 RVA: 0x00056B03 File Offset: 0x00054D03
		public virtual bool prefetch
		{
			get
			{
				return this._prefetch;
			}
			set
			{
				this._prefetch = value;
				if (base.gameObject.activeInHierarchy && base.enabled && ReInput.isReady && ReInput.localization.localizedStringProvider == this)
				{
					ReInput.localization.prefetch = value;
				}
			}
		}

		// (get) Token: 0x06000DF3 RID: 3571
		protected abstract bool initialized { get; }

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00056B40 File Offset: 0x00054D40
		protected virtual void OnEnable()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.TrySetLocalizedStringProvider();
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00056B57 File Offset: 0x00054D57
		protected virtual void OnDisable()
		{
			if (ReInput.isReady && ReInput.localization.localizedStringProvider == this)
			{
				ReInput.localization.localizedStringProvider = null;
			}
			ReInput.InitializedEvent -= this.TrySetLocalizedStringProvider;
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00056B8A File Offset: 0x00054D8A
		protected virtual void Update()
		{
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00056B8C File Offset: 0x00054D8C
		protected virtual void TrySetLocalizedStringProvider()
		{
			ReInput.InitializedEvent -= this.TrySetLocalizedStringProvider;
			ReInput.InitializedEvent += this.TrySetLocalizedStringProvider;
			if (!ReInput.isReady)
			{
				return;
			}
			if (!UnityTools.IsNullOrDestroyed<ILocalizedStringProvider>(ReInput.localization.localizedStringProvider))
			{
				Debug.LogWarning("A localized string provider is already set. Only one localized string provider can exist at a time.");
				return;
			}
			ReInput.localization.localizedStringProvider = this;
			ReInput.localization.prefetch = this._prefetch;
		}

		protected abstract bool Initialize();

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00056BFC File Offset: 0x00054DFC
		public virtual void Reload()
		{
			this.Initialize();
			if (base.gameObject.activeInHierarchy && base.enabled && ReInput.isReady && ReInput.localization.localizedStringProvider == this)
			{
				ReInput.localization.Reload();
			}
		}

		protected abstract bool TryGetLocalizedString(string key, out string result);

		// Token: 0x06000DFB RID: 3579 RVA: 0x00056C38 File Offset: 0x00054E38
		bool ILocalizedStringProvider.TryGetLocalizedString(string key, out string result)
		{
			return this.TryGetLocalizedString(key, out result);
		}

		[SerializeField]
		[Tooltip("Determines if localized strings should be fetched immediately in bulk when available. If false, strings will be fetched when queried.")]
		private bool _prefetch;
	}
}
