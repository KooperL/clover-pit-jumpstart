using System;
using UnityEngine;

namespace I2.Loc
{
	public class AutoChangeCultureInfo : MonoBehaviour
	{
		// Token: 0x06000FFC RID: 4092 RVA: 0x00063D0E File Offset: 0x00061F0E
		public void Start()
		{
			LocalizationManager.EnableChangingCultureInfo(true);
		}
	}
}
