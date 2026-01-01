using System;
using Panik;
using TMPro;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class GiantNumberOnTheWall : MonoBehaviour
{
	// Token: 0x060008E9 RID: 2281 RVA: 0x0000D0C4 File Offset: 0x0000B2C4
	private void Start()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		this.text.text = Data.game.goodEndingCounter.ToString("00");
	}

	// Token: 0x0400089C RID: 2204
	public TextMeshPro text;
}
