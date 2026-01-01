using System;
using Panik;
using UnityEngine;

// Token: 0x02000042 RID: 66
public class SoundAreaPlayer : MonoBehaviour
{
	// Token: 0x06000420 RID: 1056 RVA: 0x0002D5F0 File Offset: 0x0002B7F0
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Player"))
		{
			return;
		}
		if (this.playedOnce && this.playOnce)
		{
			return;
		}
		if (Sound.IsPlaying(this.clip.name))
		{
			return;
		}
		Sound.Play3D(this.clip.name, base.transform.position + this.soundOffset, this.distance, 1f, 1f + global::UnityEngine.Random.Range(-this.pitchVariance / 2f, this.pitchVariance / 2f), AudioRolloffMode.Linear);
		this.playedOnce = true;
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x0002D690 File Offset: 0x0002B890
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position + this.soundOffset, 0.1f);
		Gizmos.DrawWireSphere(base.transform.position + this.soundOffset, this.distance);
	}

	// Token: 0x040003A6 RID: 934
	public AudioClip clip;

	// Token: 0x040003A7 RID: 935
	public Vector3 soundOffset;

	// Token: 0x040003A8 RID: 936
	public float distance = 50f;

	// Token: 0x040003A9 RID: 937
	public float pitchVariance;

	// Token: 0x040003AA RID: 938
	public bool playOnce;

	// Token: 0x040003AB RID: 939
	private bool playedOnce;
}
