using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000184 RID: 388
	public class CallbackNotification : MonoBehaviour
	{
		// Token: 0x06001199 RID: 4505 RVA: 0x00075718 File Offset: 0x00073918
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
