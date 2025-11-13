using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	public class ToggleLanguage : MonoBehaviour
	{
		// Token: 0x06000E16 RID: 3606 RVA: 0x000571D0 File Offset: 0x000553D0
		private void Start()
		{
			base.Invoke("test", 3f);
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x000571E4 File Offset: 0x000553E4
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
