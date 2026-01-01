using System;
using UnityEngine;
using UnityEngine.Events;

namespace I2.Loc
{
	// Token: 0x020001DB RID: 475
	[AddComponentMenu("I2/Localization/I2 Localize Callback")]
	public class CustomLocalizeCallback : MonoBehaviour
	{
		// Token: 0x060013F0 RID: 5104 RVA: 0x000156CE File Offset: 0x000138CE
		public void OnEnable()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
			LocalizationManager.OnLocalizeEvent += this.OnLocalize;
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x000156F2 File Offset: 0x000138F2
		public void OnDisable()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x00015705 File Offset: 0x00013905
		public void OnLocalize()
		{
			this._OnLocalize.Invoke();
		}

		// Token: 0x040013BE RID: 5054
		public UnityEvent _OnLocalize = new UnityEvent();
	}
}
