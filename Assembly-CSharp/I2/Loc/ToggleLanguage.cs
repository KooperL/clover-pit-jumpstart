using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	public class ToggleLanguage : MonoBehaviour
	{
		// Token: 0x06000DFF RID: 3583 RVA: 0x000569F4 File Offset: 0x00054BF4
		private void Start()
		{
			base.Invoke("test", 3f);
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00056A08 File Offset: 0x00054C08
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
