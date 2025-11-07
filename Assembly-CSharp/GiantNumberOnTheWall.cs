using System;
using Panik;
using TMPro;
using UnityEngine;

public class GiantNumberOnTheWall : MonoBehaviour
{
	// Token: 0x060007DD RID: 2013 RVA: 0x00032F76 File Offset: 0x00031176
	private void Start()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		this.text.text = Data.game.goodEndingCounter.ToString("00");
	}

	public TextMeshPro text;
}
