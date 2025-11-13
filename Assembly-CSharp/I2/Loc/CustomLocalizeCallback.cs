using System;
using UnityEngine;
using UnityEngine.Events;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/I2 Localize Callback")]
	public class CustomLocalizeCallback : MonoBehaviour
	{
		// Token: 0x06001002 RID: 4098 RVA: 0x00063D93 File Offset: 0x00061F93
		public void OnEnable()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
			LocalizationManager.OnLocalizeEvent += this.OnLocalize;
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00063DB7 File Offset: 0x00061FB7
		public void OnDisable()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00063DCA File Offset: 0x00061FCA
		public void OnLocalize()
		{
			this._OnLocalize.Invoke();
		}

		public UnityEvent _OnLocalize = new UnityEvent();
	}
}
