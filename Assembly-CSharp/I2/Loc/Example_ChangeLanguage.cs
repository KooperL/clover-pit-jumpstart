using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000185 RID: 389
	public class Example_ChangeLanguage : MonoBehaviour
	{
		// Token: 0x0600119B RID: 4507 RVA: 0x00014612 File Offset: 0x00012812
		public void SetLanguage_English()
		{
			this.SetLanguage("English");
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0001461F File Offset: 0x0001281F
		public void SetLanguage_French()
		{
			this.SetLanguage("French");
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0001462C File Offset: 0x0001282C
		public void SetLanguage_Spanish()
		{
			this.SetLanguage("Spanish");
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x00014639 File Offset: 0x00012839
		public void SetLanguage(string LangName)
		{
			if (LocalizationManager.HasLanguage(LangName, true, true, true))
			{
				LocalizationManager.CurrentLanguage = LangName;
			}
		}
	}
}
