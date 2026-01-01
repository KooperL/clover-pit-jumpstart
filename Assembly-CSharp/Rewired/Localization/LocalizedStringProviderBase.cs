using System;
using Rewired.Interfaces;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.Localization
{
	// Token: 0x02000183 RID: 387
	public abstract class LocalizedStringProviderBase : MonoBehaviour, ILocalizedStringProvider
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x0600118D RID: 4493 RVA: 0x0001453D File Offset: 0x0001273D
		// (set) Token: 0x0600118E RID: 4494 RVA: 0x00014545 File Offset: 0x00012745
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

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x0600118F RID: 4495
		protected abstract bool initialized { get; }

		// Token: 0x06001190 RID: 4496 RVA: 0x00014582 File Offset: 0x00012782
		protected virtual void OnEnable()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.TrySetLocalizedStringProvider();
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x00014599 File Offset: 0x00012799
		protected virtual void OnDisable()
		{
			if (ReInput.isReady && ReInput.localization.localizedStringProvider == this)
			{
				ReInput.localization.localizedStringProvider = null;
			}
			ReInput.InitializedEvent -= this.TrySetLocalizedStringProvider;
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0000774E File Offset: 0x0000594E
		protected virtual void Update()
		{
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x000756A8 File Offset: 0x000738A8
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

		// Token: 0x06001194 RID: 4500
		protected abstract bool Initialize();

		// Token: 0x06001195 RID: 4501 RVA: 0x000145CC File Offset: 0x000127CC
		public virtual void Reload()
		{
			this.Initialize();
			if (base.gameObject.activeInHierarchy && base.enabled && ReInput.isReady && ReInput.localization.localizedStringProvider == this)
			{
				ReInput.localization.Reload();
			}
		}

		// Token: 0x06001196 RID: 4502
		protected abstract bool TryGetLocalizedString(string key, out string result);

		// Token: 0x06001197 RID: 4503 RVA: 0x00014608 File Offset: 0x00012808
		bool ILocalizedStringProvider.TryGetLocalizedString(string key, out string result)
		{
			return this.TryGetLocalizedString(key, out result);
		}

		// Token: 0x0400129C RID: 4764
		[SerializeField]
		[Tooltip("Determines if localized strings should be fetched immediately in bulk when available. If false, strings will be fetched when queried.")]
		private bool _prefetch;
	}
}
