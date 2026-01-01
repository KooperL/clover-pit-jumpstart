using System;
using Panik;
using TMPro;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class PillarScript : MonoBehaviour
{
	// Token: 0x0600041E RID: 1054 RVA: 0x0002D51C File Offset: 0x0002B71C
	private void Start()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		if (this.numberText != null && this.numberText.enabled)
		{
			int num = -1;
			int goodEndingCounter = Data.game.goodEndingCounter;
			int num2 = goodEndingCounter % 4;
			int num3 = Mathf.FloorToInt((float)(goodEndingCounter / 4)) * 4 + num2;
			switch (this.index)
			{
			case 0:
				num = goodEndingCounter;
				break;
			case 1:
				if (num2 > 2)
				{
					if (num2 == 3)
					{
						num = num3 - 3;
					}
				}
				else
				{
					num = num3 + 1;
				}
				break;
			case 2:
				if (num2 > 1)
				{
					if (num2 - 2 <= 1)
					{
						num = num3 - 2;
					}
				}
				else
				{
					num = num3 + 2;
				}
				break;
			case 3:
				if (num2 != 0)
				{
					if (num2 - 1 <= 2)
					{
						num = num3 - 1;
					}
				}
				else
				{
					num = num3 + 3;
				}
				break;
			}
			this.numberText.text = num.ToString("00");
		}
	}

	// Token: 0x040003A4 RID: 932
	public TextMeshPro numberText;

	// Token: 0x040003A5 RID: 933
	public int index;
}
