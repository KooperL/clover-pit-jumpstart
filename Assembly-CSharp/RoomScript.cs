using System;
using Panik;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
	// Token: 0x06000811 RID: 2065 RVA: 0x00033B98 File Offset: 0x00031D98
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
