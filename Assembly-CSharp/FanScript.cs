using System;
using Panik;
using UnityEngine;

public class FanScript : MonoBehaviour
{
	// Token: 0x060007DB RID: 2011 RVA: 0x00032EF0 File Offset: 0x000310F0
	private void Update()
	{
		if (!Tick.IsGameRunning)
		{
			return;
		}
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		int gamePhase = (int)GameplayMaster.GetGamePhase();
		bool flag = Sound.IsPlaying("SoundFan");
		float volumeFade = Music.GetVolumeFade();
		if (gamePhase == 5)
		{
		}
		float fanVolume = Data.settings.fanVolume;
		if (!flag)
		{
			Sound.Play3D("SoundFan", base.transform.position, 20f, Mathf.Min(1f, volumeFade * fanVolume), 1f, 1);
		}
	}
}
