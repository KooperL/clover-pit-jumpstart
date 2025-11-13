using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/SetLanguage Dropdown")]
	public class SetLanguageDropdown : MonoBehaviour
	{
		// Token: 0x0600104B RID: 4171 RVA: 0x00066170 File Offset: 0x00064370
		private void OnEnable()
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (component == null)
			{
				return;
			}
			string currentLanguage = LocalizationManager.CurrentLanguage;
			if (LocalizationManager.Sources.Count == 0)
			{
				LocalizationManager.UpdateSources();
			}
			List<string> allLanguages = LocalizationManager.GetAllLanguages(true);
			component.ClearOptions();
			component.AddOptions(allLanguages);
			component.value = allLanguages.IndexOf(currentLanguage);
			component.onValueChanged.RemoveListener(new UnityAction<int>(this.OnValueChanged));
			component.onValueChanged.AddListener(new UnityAction<int>(this.OnValueChanged));
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x000661F8 File Offset: 0x000643F8
		private void OnValueChanged(int index)
		{
			Dropdown component = base.GetComponent<Dropdown>();
			if (index < 0)
			{
				index = 0;
				component.value = index;
			}
			LocalizationManager.CurrentLanguage = component.options[index].text;
		}
	}
}
