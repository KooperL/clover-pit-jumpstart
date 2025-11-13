using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/Localize Dropdown")]
	public class LocalizeDropdown : MonoBehaviour
	{
		// Token: 0x06000F08 RID: 3848 RVA: 0x00060B88 File Offset: 0x0005ED88
		public void Start()
		{
			LocalizationManager.OnLocalizeEvent += this.OnLocalize;
			this.OnLocalize();
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x00060BA1 File Offset: 0x0005EDA1
		public void OnDestroy()
		{
			LocalizationManager.OnLocalizeEvent -= this.OnLocalize;
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x00060BB4 File Offset: 0x0005EDB4
		private void OnEnable()
		{
			if (this._Terms.Count == 0)
			{
				this.FillValues();
			}
			this.OnLocalize();
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x00060BCF File Offset: 0x0005EDCF
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

		// Token: 0x06000F0C RID: 3852 RVA: 0x00060C08 File Offset: 0x0005EE08
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

		// Token: 0x06000F0D RID: 3853 RVA: 0x00060C84 File Offset: 0x0005EE84
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

		// Token: 0x06000F0E RID: 3854 RVA: 0x00060D18 File Offset: 0x0005EF18
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

		// Token: 0x06000F0F RID: 3855 RVA: 0x00060DA4 File Offset: 0x0005EFA4
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

		public List<string> _Terms = new List<string>();
	}
}
