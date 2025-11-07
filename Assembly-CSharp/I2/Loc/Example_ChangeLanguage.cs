using System;
using UnityEngine;

namespace I2.Loc
{
	public class Example_ChangeLanguage : MonoBehaviour
	{
		// Token: 0x06000DE8 RID: 3560 RVA: 0x000564B9 File Offset: 0x000546B9
		public void SetLanguage_English()
		{
			this.SetLanguage("English");
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x000564C6 File Offset: 0x000546C6
		public void SetLanguage_French()
		{
			this.SetLanguage("French");
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x000564D3 File Offset: 0x000546D3
		public void SetLanguage_Spanish()
		{
			this.SetLanguage("Spanish");
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x000564E0 File Offset: 0x000546E0
		public void SetLanguage(string LangName)
		{
			if (LocalizationManager.HasLanguage(LangName, true, true, true))
			{
				LocalizationManager.CurrentLanguage = LangName;
			}
		}
	}
}
