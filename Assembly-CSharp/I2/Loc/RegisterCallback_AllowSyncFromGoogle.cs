using System;
using UnityEngine;

namespace I2.Loc
{
	public class RegisterCallback_AllowSyncFromGoogle : MonoBehaviour
	{
		// Token: 0x0600100C RID: 4108 RVA: 0x00064046 File Offset: 0x00062246
		public void Awake()
		{
			LocalizationManager.Callback_AllowSyncFromGoogle = new Func<LanguageSourceData, bool>(this.AllowSyncFromGoogle);
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0006405A File Offset: 0x0006225A
		public void OnEnable()
		{
			LocalizationManager.Callback_AllowSyncFromGoogle = new Func<LanguageSourceData, bool>(this.AllowSyncFromGoogle);
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0006406E File Offset: 0x0006226E
		public void OnDisable()
		{
			LocalizationManager.Callback_AllowSyncFromGoogle = null;
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00064076 File Offset: 0x00062276
		public virtual bool AllowSyncFromGoogle(LanguageSourceData Source)
		{
			return true;
		}
	}
}
