using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200018A RID: 394
	public class ToggleLanguage : MonoBehaviour
	{
		// Token: 0x060011B2 RID: 4530 RVA: 0x00014701 File Offset: 0x00012901
		private void Start()
		{
			base.Invoke("test", 3f);
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00075B90 File Offset: 0x00073D90
		private void test()
		{
			List<string> allLanguages = LocalizationManager.GetAllLanguages(true);
			int num = allLanguages.IndexOf(LocalizationManager.CurrentLanguage);
			if (num >= 0)
			{
				num = (num + 1) % allLanguages.Count;
			}
			base.Invoke("test", 3f);
		}
	}
}
