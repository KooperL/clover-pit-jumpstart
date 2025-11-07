using System;
using UnityEngine;

namespace I2.Loc
{
	public class RegisterGlobalParameters : MonoBehaviour, ILocalizationParamsManager
	{
		// Token: 0x06001011 RID: 4113 RVA: 0x00064081 File Offset: 0x00062281
		public virtual void OnEnable()
		{
			if (!LocalizationManager.ParamManagers.Contains(this))
			{
				LocalizationManager.ParamManagers.Add(this);
				LocalizationManager.LocalizeAll(true);
			}
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x000640A1 File Offset: 0x000622A1
		public virtual void OnDisable()
		{
			LocalizationManager.ParamManagers.Remove(this);
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x000640AF File Offset: 0x000622AF
		public virtual string GetParameterValue(string ParamName)
		{
			return null;
		}
	}
}
