using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	// Token: 0x02000114 RID: 276
	public static class Sound
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00010884 File Offset: 0x0000EA84
		// (set) Token: 0x06000D03 RID: 3331 RVA: 0x00010890 File Offset: 0x0000EA90
		private static float Volume
		{
			get
			{
				return Data.settings.volumeSound;
			}
			set
			{
				Data.settings.volumeSound = value;
			}
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x00064E40 File Offset: 0x00063040
		private static Sound.SoundCapsule GetSoundCapsule(AudioClip clip)
		{
			Sound.SoundCapsule soundCapsule;
			if (Sound.soundCapsulesPool.Count > 0)
			{
				soundCapsule = Sound.soundCapsulesPool[Sound.soundCapsulesPool.Count - 1];
				soundCapsule.myGameObject.SetActive(true);
				soundCapsule.myGameObject.name = "SOUND: " + clip.name;
				soundCapsule.myAudioSource.clip = clip;
				soundCapsule.delayTimer = Sound.tempDelayVar;
				soundCapsule.doneDelay = Sound.tempDelayVar <= 0f;
				Sound.tempDelayVar = 0f;
				Sound.soundCapsulesPlaying.Add(soundCapsule);
				Sound.soundCapsulesPool.RemoveAt(Sound.soundCapsulesPool.Count - 1);
				return soundCapsule;
			}
			soundCapsule = new Sound.SoundCapsule(clip);
			soundCapsule.delayTimer = Sound.tempDelayVar;
			soundCapsule.doneDelay = Sound.tempDelayVar <= 0f;
			Sound.tempDelayVar = 0f;
			Sound.soundCapsulesPlaying.Add(soundCapsule);
			return soundCapsule;
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0001089D File Offset: 0x0000EA9D
		private static bool IsCapsuleOk(Sound.SoundCapsule capsule)
		{
			return capsule != null && !(capsule.myGameObject == null) && !(capsule.myAudioSource == null);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00064F34 File Offset: 0x00063134
		private static void CheckListsForNullValues()
		{
			for (int i = Sound.soundCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (!Sound.IsCapsuleOk(Sound.soundCapsulesPlaying[i]))
				{
					Sound.soundCapsulesPlaying.RemoveAt(i);
				}
			}
			for (int j = Sound.soundCapsulesPool.Count - 1; j >= 0; j--)
			{
				if (!Sound.IsCapsuleOk(Sound.soundCapsulesPool[j]))
				{
					Sound.soundCapsulesPool.RemoveAt(j);
				}
			}
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x00064FAC File Offset: 0x000631AC
		public static Sound.SoundCapsule _Play(AudioClip sound, bool _3d, Vector3 position, float distance3D, AudioRolloffMode rollOffMode, float vol, float pitch, bool pausable)
		{
			Sound.SoundCapsule soundCapsule = Sound.GetSoundCapsule(sound);
			soundCapsule.myAudioSource.spatialBlend = (_3d ? 1f : 0f);
			soundCapsule.localVolume = vol;
			Sound._ApplyVolumeToSoundCapsule(soundCapsule);
			soundCapsule.myAudioSource.pitch = pitch;
			if (_3d)
			{
				soundCapsule.myAudioSource.dopplerLevel = 0f;
				soundCapsule.myAudioSource.spread = 180f;
				soundCapsule.myAudioSource.rolloffMode = rollOffMode;
				soundCapsule.myAudioSource.maxDistance = distance3D;
				soundCapsule.myAudioSource.minDistance = 0f;
				soundCapsule.myGameObject.transform.position = position;
			}
			soundCapsule.paused = false;
			soundCapsule.pausable = pausable;
			soundCapsule.soundLife = sound.length + 1f;
			soundCapsule.clip = sound;
			if (soundCapsule.delayTimer <= 0f)
			{
				soundCapsule.myAudioSource.Play();
			}
			else
			{
				soundCapsule.myAudioSource.Stop();
			}
			return soundCapsule;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x000108C5 File Offset: 0x0000EAC5
		public static Sound.SoundCapsule Play(string soundName, float vol = 1f, float pitch = 1f)
		{
			return Sound._Play(AssetMaster.GetSound(soundName), false, Vector3.zero, 10f, AudioRolloffMode.Linear, vol, pitch, true);
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x000108E1 File Offset: 0x0000EAE1
		public static Sound.SoundCapsule Play_Unpausable(string soundName, float vol = 1f, float pitch = 1f)
		{
			return Sound._Play(AssetMaster.GetSound(soundName), false, Vector3.zero, 10f, AudioRolloffMode.Linear, vol, pitch, false);
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x000108FD File Offset: 0x0000EAFD
		public static Sound.SoundCapsule Play3D(string soundName, Vector3 position, float distance3D, float vol = 1f, float pitch = 1f, AudioRolloffMode rollOffMode = AudioRolloffMode.Linear)
		{
			return Sound._Play(AssetMaster.GetSound(soundName), true, position, distance3D, rollOffMode, vol, pitch, true);
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x00010913 File Offset: 0x0000EB13
		public static Sound.SoundCapsule Play3D_Unpausable(string soundName, Vector3 position, float distance3D, float vol = 1f, float pitch = 1f, AudioRolloffMode rollOffMode = AudioRolloffMode.Linear)
		{
			return Sound._Play(AssetMaster.GetSound(soundName), true, position, distance3D, rollOffMode, vol, pitch, false);
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x00010929 File Offset: 0x0000EB29
		public static Sound.SoundCapsule PlayDelayed(string soundName, float delay, float vol = 1f, float pitch = 1f)
		{
			Sound.tempDelayVar = delay;
			return Sound.Play(soundName, vol, pitch);
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00010939 File Offset: 0x0000EB39
		public static Sound.SoundCapsule PlayDelayed_Unpausable(string soundName, float delay, float vol = 1f, float pitch = 1f)
		{
			Sound.tempDelayVar = delay;
			return Sound.Play_Unpausable(soundName, vol, pitch);
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x000650A0 File Offset: 0x000632A0
		private static void _Stop(int index)
		{
			Sound.soundCapsulesPlaying[index].myAudioSource.Stop();
			Sound.soundCapsulesPlaying[index].paused = false;
			Sound.soundCapsulesPlaying[index].myGameObject.SetActive(false);
			Sound.soundCapsulesPool.Add(Sound.soundCapsulesPlaying[index]);
			Sound.soundCapsulesPlaying.RemoveAt(index);
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x0006510C File Offset: 0x0006330C
		public static void Stop(string clipName, bool stopAllSoundsWithSameName = true)
		{
			for (int i = Sound.soundCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (Sound.IsCapsuleOk(Sound.soundCapsulesPlaying[i]) && Sound.soundCapsulesPlaying[i].clip.name == clipName)
				{
					Sound._Stop(i);
					if (!stopAllSoundsWithSameName)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00065168 File Offset: 0x00063368
		public static void StopAll()
		{
			for (int i = Sound.soundCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (Sound.IsCapsuleOk(Sound.soundCapsulesPlaying[i]))
				{
					Sound._Stop(i);
				}
			}
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x000651A4 File Offset: 0x000633A4
		public static Sound.SoundCapsule Find(string clipName)
		{
			for (int i = Sound.soundCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (Sound.IsCapsuleOk(Sound.soundCapsulesPlaying[i]) && Sound.soundCapsulesPlaying[i].myAudioSource.clip.name == clipName)
				{
					return Sound.soundCapsulesPlaying[i];
				}
			}
			return null;
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x0006520C File Offset: 0x0006340C
		public static Sound.SoundCapsule FindUnpaused(string clipName)
		{
			for (int i = Sound.soundCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (Sound.IsCapsuleOk(Sound.soundCapsulesPlaying[i]) && !Sound.soundCapsulesPlaying[i].paused && Sound.soundCapsulesPlaying[i].myAudioSource.clip.name == clipName)
				{
					return Sound.soundCapsulesPlaying[i];
				}
			}
			return null;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00065284 File Offset: 0x00063484
		public static Sound.SoundCapsule FindPaused(string clipName)
		{
			for (int i = Sound.soundCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (Sound.IsCapsuleOk(Sound.soundCapsulesPlaying[i]) && Sound.soundCapsulesPlaying[i].paused && Sound.soundCapsulesPlaying[i].myAudioSource.clip.name == clipName)
				{
					return Sound.soundCapsulesPlaying[i];
				}
			}
			return null;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x000652FC File Offset: 0x000634FC
		public static bool _IsPlaying(string clipName, bool returnTrueIfPausedSoundIsFound)
		{
			for (int i = Sound.soundCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (Sound.IsCapsuleOk(Sound.soundCapsulesPlaying[i]) && (!Sound.soundCapsulesPlaying[i].paused || returnTrueIfPausedSoundIsFound) && Sound.soundCapsulesPlaying[i].myAudioSource.clip.name == clipName && (Sound.soundCapsulesPlaying[i].myAudioSource.isPlaying || (Sound.soundCapsulesPlaying[i].paused && returnTrueIfPausedSoundIsFound)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00010949 File Offset: 0x0000EB49
		public static bool IsPlaying(string clipName)
		{
			return Sound._IsPlaying(clipName, false);
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00010952 File Offset: 0x0000EB52
		public static bool IsPlayingOrPaused(string clipName)
		{
			return Sound._IsPlaying(clipName, true);
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00065398 File Offset: 0x00063598
		public static float GetVolumeFinal(Sound.SoundCapsule capsule)
		{
			if (capsule == null)
			{
				return Sound.Volume * Sound.volumeFade * 0.75f * Data.settings.volumeMaster;
			}
			return capsule.localVolume * Sound.Volume * Sound.volumeFade * 0.75f * Data.settings.volumeMaster;
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x0001095B File Offset: 0x0000EB5B
		private static void _ApplyVolumeToSoundCapsule(Sound.SoundCapsule capsule)
		{
			capsule.myAudioSource.volume = capsule.localVolume * Sound.Volume * Sound.volumeFade * 0.75f * Data.settings.volumeMaster;
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x0001098B File Offset: 0x0000EB8B
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x00010992 File Offset: 0x0000EB92
		public static float VolumeMain
		{
			get
			{
				return Sound.Volume;
			}
			set
			{
				Sound.Volume = value;
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00010992 File Offset: 0x0000EB92
		public static void SetVolumeMain(float val)
		{
			Sound.Volume = val;
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0001098B File Offset: 0x0000EB8B
		public static float GetVolumeMain()
		{
			return Sound.Volume;
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0001099A File Offset: 0x0000EB9A
		public static void SetVolumeFade(float val, float changeSpeed)
		{
			Sound.volumeFadeTo = val;
			Sound.volumeFadeSpeed = changeSpeed;
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x000109A8 File Offset: 0x0000EBA8
		public static void SetVolumeFadeInstant(float val)
		{
			Sound.volumeFade = val;
			Sound.volumeFadeTo = val;
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x000109B6 File Offset: 0x0000EBB6
		public static float GetVolumeFade()
		{
			return Sound.volumeFade;
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x000109BD File Offset: 0x0000EBBD
		public static float GetVolumeFadeTarget()
		{
			return Sound.volumeFadeTo;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x000109C4 File Offset: 0x0000EBC4
		public static float GetVolumeFadeChangeSpeed()
		{
			return Sound.volumeFadeSpeed;
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x000653E8 File Offset: 0x000635E8
		public static void Routine()
		{
			Sound.CheckListsForNullValues();
			if (Sound.volumeFade < Sound.volumeFadeTo)
			{
				Sound.volumeFade = Mathf.Min(Sound.volumeFade + Tick.TimeUnscaled * Sound.volumeFadeSpeed, Sound.volumeFadeTo);
			}
			if (Sound.volumeFade > Sound.volumeFadeTo)
			{
				Sound.volumeFade = Mathf.Max(Sound.volumeFade - Tick.TimeUnscaled * Sound.volumeFadeSpeed, Sound.volumeFadeTo);
			}
			for (int i = Sound.soundCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				Sound.SoundCapsule soundCapsule = Sound.soundCapsulesPlaying[i];
				Sound._ApplyVolumeToSoundCapsule(soundCapsule);
				if (Tick.Paused && Sound.pausableByGame)
				{
					if (soundCapsule.pausable && !soundCapsule.paused)
					{
						soundCapsule.paused = true;
						soundCapsule.myAudioSource.Pause();
					}
					else if (!soundCapsule.pausable)
					{
						if (soundCapsule.delayTimer > 0f)
						{
							soundCapsule.delayTimer -= Tick.Time;
							if (soundCapsule.delayTimer <= 0f && !soundCapsule.doneDelay)
							{
								soundCapsule.doneDelay = true;
								soundCapsule.myAudioSource.Play();
							}
						}
						else
						{
							soundCapsule.soundLife -= Tick.Time;
						}
					}
				}
				else if (soundCapsule.paused)
				{
					soundCapsule.paused = false;
					soundCapsule.myAudioSource.UnPause();
				}
				else if (soundCapsule.delayTimer > 0f)
				{
					soundCapsule.delayTimer -= Tick.Time;
					if (soundCapsule.delayTimer <= 0f && !soundCapsule.doneDelay)
					{
						soundCapsule.doneDelay = true;
						soundCapsule.myAudioSource.Play();
					}
				}
				else
				{
					soundCapsule.soundLife -= Tick.Time;
				}
				if (!soundCapsule.myAudioSource.isPlaying && !soundCapsule.paused && soundCapsule.soundLife <= 0f)
				{
					Sound._Stop(i);
				}
			}
		}

		// Token: 0x04000DB4 RID: 3508
		public const float DIST_SMALLEST_FOR_REAL = 3f;

		// Token: 0x04000DB5 RID: 3509
		public const float DIST_SMALLEST = 5f;

		// Token: 0x04000DB6 RID: 3510
		public const float DIST_SMALL = 10f;

		// Token: 0x04000DB7 RID: 3511
		public const float DIST_MEDIUM = 20f;

		// Token: 0x04000DB8 RID: 3512
		public const float DIST_HIGH = 30f;

		// Token: 0x04000DB9 RID: 3513
		private const float VOLUME_ADJUST = 0.75f;

		// Token: 0x04000DBA RID: 3514
		private static float volumeFade = 1f;

		// Token: 0x04000DBB RID: 3515
		private static float volumeFadeTo = 1f;

		// Token: 0x04000DBC RID: 3516
		private static float volumeFadeSpeed = 1f;

		// Token: 0x04000DBD RID: 3517
		public static bool pausableByGame = true;

		// Token: 0x04000DBE RID: 3518
		private static List<Sound.SoundCapsule> soundCapsulesPool = new List<Sound.SoundCapsule>();

		// Token: 0x04000DBF RID: 3519
		public static List<Sound.SoundCapsule> soundCapsulesPlaying = new List<Sound.SoundCapsule>();

		// Token: 0x04000DC0 RID: 3520
		private static float tempDelayVar = 0f;

		// Token: 0x04000DC1 RID: 3521
		private static List<Sound.SoundCapsule> delayedSounds = new List<Sound.SoundCapsule>();

		// Token: 0x02000115 RID: 277
		public class SoundCapsule
		{
			// Token: 0x06000D24 RID: 3364 RVA: 0x00065628 File Offset: 0x00063828
			public SoundCapsule(AudioClip sound)
			{
				this.myGameObject = new GameObject();
				this.myGameObject.name = "SOUND: " + sound.name;
				this.myGameObject.transform.SetParent(Master.instance.audioHolderTr);
				this.myAudioSource = this.myGameObject.AddComponent<AudioSource>();
				this.myAudioSource.clip = sound;
			}

			// Token: 0x06000D25 RID: 3365 RVA: 0x000656AC File Offset: 0x000638AC
			~SoundCapsule()
			{
				if (this.myGameObject != null)
				{
					global::UnityEngine.Object.Destroy(this.myGameObject);
				}
			}

			// Token: 0x06000D26 RID: 3366 RVA: 0x000109CB File Offset: 0x0000EBCB
			public void StopRequest()
			{
				this.myAudioSource.Stop();
				this.paused = false;
				this.soundLife = 0f;
			}

			// Token: 0x04000DC2 RID: 3522
			public float delayTimer;

			// Token: 0x04000DC3 RID: 3523
			public bool doneDelay;

			// Token: 0x04000DC4 RID: 3524
			public GameObject myGameObject;

			// Token: 0x04000DC5 RID: 3525
			public AudioSource myAudioSource;

			// Token: 0x04000DC6 RID: 3526
			public bool pausable = true;

			// Token: 0x04000DC7 RID: 3527
			public bool paused;

			// Token: 0x04000DC8 RID: 3528
			public float localVolume = 1f;

			// Token: 0x04000DC9 RID: 3529
			public float soundLife;

			// Token: 0x04000DCA RID: 3530
			public AudioClip clip;
		}
	}
}
