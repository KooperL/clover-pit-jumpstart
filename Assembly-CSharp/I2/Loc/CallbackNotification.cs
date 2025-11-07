using System;
using UnityEngine;

namespace I2.Loc
{
	public class CallbackNotification : MonoBehaviour
	{
		// Token: 0x06000DE6 RID: 3558 RVA: 0x00056470 File Offset: 0x00054670
		public void OnModifyLocalization()
		{
			if (string.IsNullOrEmpty(Localize.MainTranslation))
			{
				return;
			}
			string translation = LocalizationManager.GetTranslation("Color/Red", true, 0, true, false, null, null, true);
			Localize.MainTranslation = Localize.MainTranslation.Replace("{PLAYER_COLOR}", translation);
		}
	}
}
