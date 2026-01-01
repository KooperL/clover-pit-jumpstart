using System;
using Panik;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class RoomScript : MonoBehaviour
{
	// Token: 0x06000929 RID: 2345 RVA: 0x0004AE68 File Offset: 0x00049068
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

	// Token: 0x040008E4 RID: 2276
	public GameObject[] movableDecroationsHolders;
}
