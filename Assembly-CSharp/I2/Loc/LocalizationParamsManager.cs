using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizationParamsManager : MonoBehaviour, ILocalizationParamsManager
	{
		// Token: 0x06001001 RID: 4097 RVA: 0x00063E2C File Offset: 0x0006202C
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

		// Token: 0x06001002 RID: 4098 RVA: 0x00063E88 File Offset: 0x00062088
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

		// Token: 0x06001003 RID: 4099 RVA: 0x00063F20 File Offset: 0x00062120
		public void OnLocalize()
		{
			Localize component = base.GetComponent<Localize>();
			if (component != null)
			{
				component.OnLocalize(true);
			}
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00063F44 File Offset: 0x00062144
		public virtual void OnEnable()
		{
			if (this._IsGlobalManager)
			{
				this.DoAutoRegister();
			}
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00063F54 File Offset: 0x00062154
		public void DoAutoRegister()
		{
			if (!LocalizationManager.ParamManagers.Contains(this))
			{
				LocalizationManager.ParamManagers.Add(this);
				LocalizationManager.LocalizeAll(true);
			}
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x00063F74 File Offset: 0x00062174
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
