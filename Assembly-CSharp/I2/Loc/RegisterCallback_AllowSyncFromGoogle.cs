using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001E5 RID: 485
	public class RegisterCallback_AllowSyncFromGoogle : MonoBehaviour
	{
		// Token: 0x06001417 RID: 5143 RVA: 0x00015815 File Offset: 0x00013A15
		public void Awake()
		{
			LocalizationManager.Callback_AllowSyncFromGoogle = new Func<LanguageSourceData, bool>(this.AllowSyncFromGoogle);
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x00015815 File Offset: 0x00013A15
		public void OnEnable()
		{
			LocalizationManager.Callback_AllowSyncFromGoogle = new Func<LanguageSourceData, bool>(this.AllowSyncFromGoogle);
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x00015829 File Offset: 0x00013A29
		public void OnDisable()
		{
			LocalizationManager.Callback_AllowSyncFromGoogle = null;
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x00007C86 File Offset: 0x00005E86
		public virtual bool AllowSyncFromGoogle(LanguageSourceData Source)
		{
			return true;
		}
	}
}
