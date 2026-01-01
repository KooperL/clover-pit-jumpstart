using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001D9 RID: 473
	public class AutoChangeCultureInfo : MonoBehaviour
	{
		// Token: 0x060013EA RID: 5098 RVA: 0x000156A5 File Offset: 0x000138A5
		public void Start()
		{
			LocalizationManager.EnableChangingCultureInfo(true);
		}
	}
}
