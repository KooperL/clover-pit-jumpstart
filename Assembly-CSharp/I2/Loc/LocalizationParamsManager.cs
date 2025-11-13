using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizationParamsManager : MonoBehaviour, ILocalizationParamsManager
	{
		// Token: 0x06001018 RID: 4120 RVA: 0x00064608 File Offset: 0x00062808
		public string GetParameterValue(string ParamName)
		{
			if (this._Params != null)
			{
				int i = 0;
				int count = this._Params.Count;
				while (i < count)
				{
					if (this._Params[i].Name == ParamName)
					{
						return this._Params[i].Value;
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x00064664 File Offset: 0x00062864
		public void SetParameterValue(string ParamName, string ParamValue, bool localize = true)
		{
			bool flag = false;
			int i = 0;
			int count = this._Params.Count;
			while (i < count)
			{
				if (this._Params[i].Name == ParamName)
				{
					LocalizationParamsManager.ParamValue paramValue = this._Params[i];
					paramValue.Value = ParamValue;
					this._Params[i] = paramValue;
					flag = true;
					break;
				}
				i++;
			}
			if (!flag)
			{
				this._Params.Add(new LocalizationParamsManager.ParamValue
				{
					Name = ParamName,
					Value = ParamValue
				});
			}
			if (localize)
			{
				this.OnLocalize();
			}
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x000646FC File Offset: 0x000628FC
		public void OnLocalize()
		{
			Localize component = base.GetComponent<Localize>();
			if (component != null)
			{
				component.OnLocalize(true);
			}
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00064720 File Offset: 0x00062920
		public virtual void OnEnable()
		{
			if (this._IsGlobalManager)
			{
				this.DoAutoRegister();
			}
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00064730 File Offset: 0x00062930
		public void DoAutoRegister()
		{
			if (!LocalizationManager.ParamManagers.Contains(this))
			{
				LocalizationManager.ParamManagers.Add(this);
				LocalizationManager.LocalizeAll(true);
			}
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00064750 File Offset: 0x00062950
		public void OnDisable()
		{
			LocalizationManager.ParamManagers.Remove(this);
		}

		[SerializeField]
		public List<LocalizationParamsManager.ParamValue> _Params = new List<LocalizationParamsManager.ParamValue>();

		public bool _IsGlobalManager;

		[Serializable]
		public struct ParamValue
		{
			public string Name;

			public string Value;
		}
	}
}
