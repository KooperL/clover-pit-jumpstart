using System;
using Panik;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
	// Token: 0x06000818 RID: 2072 RVA: 0x00033D80 File Offset: 0x00031F80
	private void Start()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		for (int i = 0; i < this.movableDecroationsHolders.Length; i++)
		{
			this.movableDecroationsHolders[i].SetActive(Data.game.goodEndingCounter % this.movableDecroationsHolders.Length == i);
		}
	}

	public GameObject[] movableDecroationsHolders;
}
