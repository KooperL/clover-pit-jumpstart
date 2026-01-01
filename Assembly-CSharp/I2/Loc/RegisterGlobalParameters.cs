using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001E6 RID: 486
	public class RegisterGlobalParameters : MonoBehaviour, ILocalizationParamsManager
	{
		// Token: 0x0600141C RID: 5148 RVA: 0x00015787 File Offset: 0x00013987
		public virtual void OnEnable()
		{
			if (!LocalizationManager.ParamManagers.Contains(this))
			{
				LocalizationManager.ParamManagers.Add(this);
				LocalizationManager.LocalizeAll(true);
			}
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x000157A7 File Offset: 0x000139A7
		public virtual void OnDisable()
		{
			LocalizationManager.ParamManagers.Remove(this);
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x000146FE File Offset: 0x000128FE
		public virtual string GetParameterValue(string ParamName)
		{
			return null;
		}
	}
}
