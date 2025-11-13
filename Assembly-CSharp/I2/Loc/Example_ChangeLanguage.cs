using System;
using UnityEngine;

namespace I2.Loc
{
	public class Example_ChangeLanguage : MonoBehaviour
	{
		// Token: 0x06000DFF RID: 3583 RVA: 0x00056C95 File Offset: 0x00054E95
		public void SetLanguage_English()
		{
			this.SetLanguage("English");
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00056CA2 File Offset: 0x00054EA2
		public void SetLanguage_French()
		{
			this.SetLanguage("French");
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00056CAF File Offset: 0x00054EAF
		public void SetLanguage_Spanish()
		{
			this.SetLanguage("Spanish");
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x00056CBC File Offset: 0x00054EBC
		public void SetLanguage(string LangName)
		{
			if (LocalizationManager.HasLanguage(LangName, true, true, true))
			{
				LocalizationManager.CurrentLanguage = LangName;
			}
		}
	}
}
