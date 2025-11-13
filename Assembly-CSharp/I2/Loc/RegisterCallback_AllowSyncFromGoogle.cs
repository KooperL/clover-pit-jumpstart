using System;
using UnityEngine;

namespace I2.Loc
{
	public class RegisterCallback_AllowSyncFromGoogle : MonoBehaviour
	{
		// Token: 0x06001023 RID: 4131 RVA: 0x00064822 File Offset: 0x00062A22
		public void Awake()
		{
			LocalizationManager.Callback_AllowSyncFromGoogle = new Func<LanguageSourceData, bool>(this.AllowSyncFromGoogle);
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x00064836 File Offset: 0x00062A36
		public void OnEnable()
		{
			LocalizationManager.Callback_AllowSyncFromGoogle = new Func<LanguageSourceData, bool>(this.AllowSyncFromGoogle);
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0006484A File Offset: 0x00062A4A
		public void OnDisable()
		{
			LocalizationManager.Callback_AllowSyncFromGoogle = null;
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x00064852 File Offset: 0x00062A52
		public virtual bool AllowSyncFromGoogle(LanguageSourceData Source)
		{
			return true;
		}
	}
}
