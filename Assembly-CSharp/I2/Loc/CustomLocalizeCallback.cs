using System;
using UnityEngine;
using UnityEngine.Events;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/I2 Localize Callback")]
	public class CustomLocalizeCallback : MonoBehaviour
	{
		// Token: 0x06000FEB RID: 4075 RVA: 0x000635B7 File Offset: 0x000617B7
		public void OnEnable()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
			LocalizationManager.OnLocalizeEvent += this.OnLocalize;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x000635DB File Offset: 0x000617DB
		public void OnDisable()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x000635EE File Offset: 0x000617EE
		public void OnLocalize()
		{
			this._OnLocalize.Invoke();
		}

		public UnityEvent _OnLocalize = new UnityEvent();
	}
}
