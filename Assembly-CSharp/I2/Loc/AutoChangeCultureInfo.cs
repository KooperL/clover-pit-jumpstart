using System;
using UnityEngine;

namespace I2.Loc
{
	public class AutoChangeCultureInfo : MonoBehaviour
	{
		// Token: 0x06000FE5 RID: 4069 RVA: 0x00063532 File Offset: 0x00061732
		public void Start()
		{
			LocalizationManager.EnableChangingCultureInfo(true);
		}
	}
}
