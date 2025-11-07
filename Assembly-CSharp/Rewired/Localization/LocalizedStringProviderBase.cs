using System;
using Rewired.Interfaces;
using Rewired.Utils;
using UnityEngine;

namespace Rewired.Localization
{
	public abstract class LocalizedStringProviderBase : MonoBehaviour, ILocalizedStringProvider
	{
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x0005631F File Offset: 0x0005451F
		// (set) Token: 0x06000DDB RID: 3547 RVA: 0x00056327 File Offset: 0x00054527
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

		// (get) Token: 0x06000DDC RID: 3548
		protected abstract bool initialized { get; }

		// Token: 0x06000DDD RID: 3549 RVA: 0x00056364 File Offset: 0x00054564
		protected virtual void OnEnable()
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			this.TrySetLocalizedStringProvider();
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0005637B File Offset: 0x0005457B
		protected virtual void OnDisable()
		{
			if (ReInput.isReady && ReInput.localization.localizedStringProvider == this)
			{
				ReInput.localization.localizedStringProvider = null;
			}
			ReInput.InitializedEvent -= this.TrySetLocalizedStringProvider;
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x000563AE File Offset: 0x000545AE
		protected virtual void Update()
		{
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x000563B0 File Offset: 0x000545B0
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

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00056420 File Offset: 0x00054620
		public virtual void Reload()
		{
			this.Initialize();
			if (base.gameObject.activeInHierarchy && base.enabled && ReInput.isReady && ReInput.localization.localizedStringProvider == this)
			{
				ReInput.localization.Reload();
			}
		}

		protected abstract bool TryGetLocalizedString(string key, out string result);

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0005645C File Offset: 0x0005465C
		bool ILocalizedStringProvider.TryGetLocalizedString(string key, out string result)
		{
			return this.TryGetLocalizedString(key, out result);
		}

		[SerializeField]
		[Tooltip("Determines if localized strings should be fetched immediately in bulk when available. If false, strings will be fetched when queried.")]
		private bool _prefetch;
	}
}
