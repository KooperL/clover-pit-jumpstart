using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001E2 RID: 482
	public class LocalizationParamsManager : MonoBehaviour, ILocalizationParamsManager
	{
		// Token: 0x0600140C RID: 5132 RVA: 0x000822A4 File Offset: 0x000804A4
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

		// Token: 0x0600140D RID: 5133 RVA: 0x00082300 File Offset: 0x00080500
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

		// Token: 0x0600140E RID: 5134 RVA: 0x00082398 File Offset: 0x00080598
		public void OnLocalize()
		{
			Localize component = base.GetComponent<Localize>();
			if (component != null)
			{
				component.OnLocalize(true);
			}
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x00015777 File Offset: 0x00013977
		public virtual void OnEnable()
		{
			if (this._IsGlobalManager)
			{
				this.DoAutoRegister();
			}
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x00015787 File Offset: 0x00013987
		public void DoAutoRegister()
		{
			if (!LocalizationManager.ParamManagers.Contains(this))
			{
				LocalizationManager.ParamManagers.Add(this);
				LocalizationManager.LocalizeAll(true);
			}
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x000157A7 File Offset: 0x000139A7
		public void OnDisable()
		{
			LocalizationManager.ParamManagers.Remove(this);
		}

		// Token: 0x040013C8 RID: 5064
		[SerializeField]
		public List<LocalizationParamsManager.ParamValue> _Params = new List<LocalizationParamsManager.ParamValue>();

		// Token: 0x040013C9 RID: 5065
		public bool _IsGlobalManager;

		// Token: 0x020001E3 RID: 483
		[Serializable]
		public struct ParamValue
		{
			// Token: 0x040013CA RID: 5066
			public string Name;

			// Token: 0x040013CB RID: 5067
			public string Value;
		}
	}
}
