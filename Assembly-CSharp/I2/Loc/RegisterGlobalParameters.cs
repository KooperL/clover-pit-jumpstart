using System;
using UnityEngine;

namespace I2.Loc
{
	public class RegisterGlobalParameters : MonoBehaviour, ILocalizationParamsManager
	{
		// Token: 0x06001028 RID: 4136 RVA: 0x0006485D File Offset: 0x00062A5D
		public virtual void OnEnable()
		{
			if (!LocalizationManager.ParamManagers.Contains(this))
			{
				LocalizationManager.ParamManagers.Add(this);
				LocalizationManager.LocalizeAll(true);
			}
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x0006487D File Offset: 0x00062A7D
		public virtual void OnDisable()
		{
			LocalizationManager.ParamManagers.Remove(this);
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x0006488B File Offset: 0x00062A8B
		public virtual string GetParameterValue(string ParamName)
		{
			return null;
		}
	}
}
