using System;
using Panik;
using UnityEngine;

// Token: 0x02000043 RID: 67
public class SoundFilterInfluencer : MonoBehaviour
{
	// Token: 0x06000423 RID: 1059 RVA: 0x00008EE3 File Offset: 0x000070E3
	public static void ResetReverb()
	{
		CameraGame.firstInstance.audioReverbFilter.reverbLevel = SoundFilterInfluencer.reverbDefault;
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x00008EF9 File Offset: 0x000070F9
	private void Start()
	{
		if (SoundFilterInfluencer.reverbDefault == 0f)
		{
			SoundFilterInfluencer.reverbDefault = CameraGame.firstInstance.audioReverbFilter.reverbLevel;
		}
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x00008F1B File Offset: 0x0000711B
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			CameraGame.firstInstance.audioReverbFilter.reverbLevel = this.reverbLevel;
		}
	}

	// Token: 0x040003AC RID: 940
	public float reverbLevel;

	// Token: 0x040003AD RID: 941
	private static float reverbDefault;
}
