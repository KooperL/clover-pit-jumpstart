using System;
using Panik;
using UnityEngine;

public class SoundFilterInfluencer : MonoBehaviour
{
	// Token: 0x060003BF RID: 959 RVA: 0x00019EA7 File Offset: 0x000180A7
	public static void ResetReverb()
	{
		CameraGame.firstInstance.audioReverbFilter.reverbLevel = SoundFilterInfluencer.reverbDefault;
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x00019EBD File Offset: 0x000180BD
	private void Start()
	{
		if (SoundFilterInfluencer.reverbDefault == 0f)
		{
			SoundFilterInfluencer.reverbDefault = CameraGame.firstInstance.audioReverbFilter.reverbLevel;
		}
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x00019EDF File Offset: 0x000180DF
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
