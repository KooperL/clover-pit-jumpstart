using System;
using UnityEngine;

namespace I2.Loc
{
	public class CallbackNotification : MonoBehaviour
	{
		// Token: 0x06000DFD RID: 3581 RVA: 0x00056C4C File Offset: 0x00054E4C
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
