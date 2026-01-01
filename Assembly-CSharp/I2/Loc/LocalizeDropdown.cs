using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	// Token: 0x020001B5 RID: 437
	[AddComponentMenu("I2/Localization/Localize Dropdown")]
	public class LocalizeDropdown : MonoBehaviour
	{
		// Token: 0x060012D1 RID: 4817 RVA: 0x00014F49 File Offset: 0x00013149
		public void Start()
		{
			LocalizationManager.OnLocalizeEvent += this.OnLocalize;
			this.OnLocalize();
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x00014F62 File Offset: 0x00013162
		public void OnDestroy()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x00014F75 File Offset: 0x00013175
		private void OnEnable()
		{
			if (this._Terms.Count == 0)
			{
				this.FillValues();
			}
			this.OnLocalize();
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x00014F90 File Offset: 0x00013190
		public void OnLocalize()
		{
			if (!base.enabled || base.gameObject == null || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
			{
				return;
			}
			this.UpdateLocalization();
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0007EFC8 File Offset: 0x0007D1C8
		private void FillValues()
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (component == null && I2Utils.IsPlaying())
			{
				this.FillValuesTMPro();
				return;
			}
			foreach (Dropdown.OptionData optionData in component.options)
			{
				this._Terms.Add(optionData.text);
			}
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x0007F044 File Offset: 0x0007D244
		public void UpdateLocalization()
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (component == null)
			{
				this.UpdateLocalizationTMPro();
				return;
			}
			component.options.Clear();
			foreach (string text in this._Terms)
			{
				string translation = LocalizationManager.GetTranslation(text, true, 0, true, false, null, null, true);
				component.options.Add(new Dropdown.OptionData(translation));
			}
			component.RefreshShownValue();
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x0007F0D8 File Offset: 0x0007D2D8
		public void UpdateLocalizationTMPro()
		{
			TMP_Dropdown component = base.GetComponent<TMP_Dropdown>();
			if (component == null)
			{
				return;
			}
			component.options.Clear();
			foreach (string text in this._Terms)
			{
				string translation = LocalizationManager.GetTranslation(text, true, 0, true, false, null, null, true);
				component.options.Add(new TMP_Dropdown.OptionData(translation));
			}
			component.RefreshShownValue();
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x0007F164 File Offset: 0x0007D364
		private void FillValuesTMPro()
		{
			TMP_Dropdown component = base.GetComponent<TMP_Dropdown>();
			if (component == null)
			{
				return;
			}
			foreach (TMP_Dropdown.OptionData optionData in component.options)
			{
				this._Terms.Add(optionData.text);
			}
		}

		// Token: 0x04001370 RID: 4976
		public List<string> _Terms = new List<string>();
	}
}
