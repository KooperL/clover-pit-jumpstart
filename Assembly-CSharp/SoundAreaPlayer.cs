using System;
using Panik;
using UnityEngine;

public class SoundAreaPlayer : MonoBehaviour
{
	// Token: 0x060003BC RID: 956 RVA: 0x00019D9C File Offset: 0x00017F9C
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

	// Token: 0x060003BD RID: 957 RVA: 0x00019E3C File Offset: 0x0001803C
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position + this.soundOffset, 0.1f);
		Gizmos.DrawWireSphere(base.transform.position + this.soundOffset, this.distance);
	}

	public AudioClip clip;

	public Vector3 soundOffset;

	public float distance = 50f;

	public float pitchVariance;

	public bool playOnce;

	private bool playedOnce;
}
