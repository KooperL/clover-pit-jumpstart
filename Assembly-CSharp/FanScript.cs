using System;
using Panik;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class FanScript : MonoBehaviour
{
	// Token: 0x060008E7 RID: 2279 RVA: 0x0004A23C File Offset: 0x0004843C
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
			Sound.Play3D("SoundFan", base.transform.position, 20f, Mathf.Min(1f, volumeFade * fanVolume), 1f, AudioRolloffMode.Linear);
		}
	}
}
