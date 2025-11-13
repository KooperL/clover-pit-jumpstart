using System;
using Panik;
using TMPro;
using UnityEngine;

public class GiantNumberOnTheWall : MonoBehaviour
{
	// Token: 0x060007DE RID: 2014 RVA: 0x0003308E File Offset: 0x0003128E
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
