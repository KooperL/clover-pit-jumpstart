using System;
using Panik;
using UnityEngine;

public class SoundFilterInfluencer : MonoBehaviour
{
	// Token: 0x060003BD RID: 957 RVA: 0x00019F6B File Offset: 0x0001816B
	public static void ResetReverb()
	{
		CameraGame.firstInstance.audioReverbFilter.reverbLevel = SoundFilterInfluencer.reverbDefault;
	}

	// Token: 0x060003BE RID: 958 RVA: 0x00019F81 File Offset: 0x00018181
	private void Start()
	{
		if (SoundFilterInfluencer.reverbDefault == 0f)
		{
			SoundFilterInfluencer.reverbDefault = CameraGame.firstInstance.audioReverbFilter.reverbLevel;
		}
	}

	// Token: 0x060003BF RID: 959 RVA: 0x00019FA3 File Offset: 0x000181A3
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			CameraGame.firstInstance.audioReverbFilter.reverbLevel = this.reverbLevel;
		}
	}

	public float reverbLevel;

	private static float reverbDefault;
}
