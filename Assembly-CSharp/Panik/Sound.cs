using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	public static class Sound
	{
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x0004A71A File Offset: 0x0004891A
		// (set) Token: 0x06000B0D RID: 2829 RVA: 0x0004A726 File Offset: 0x00048926
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

		// Token: 0x06000B0E RID: 2830 RVA: 0x0004A734 File Offset: 0x00048934
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

		// Token: 0x06000B0F RID: 2831 RVA: 0x0004A827 File Offset: 0x00048A27
		private static bool IsCapsuleOk(Sound.SoundCapsule capsule)
		{
			return capsule != null && !(capsule.myGameObject == null) && !(capsule.myAudioSource == null);
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0004A850 File Offset: 0x00048A50
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

		// Token: 0x06000B11 RID: 2833 RVA: 0x0004A8C8 File Offset: 0x00048AC8
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

		// Token: 0x06000B12 RID: 2834 RVA: 0x0004A9BC File Offset: 0x00048BBC
		public static Sound.SoundCapsule Play(string soundName, float vol = 1f, float pitch = 1f)
		{
			return Sound._Play(AssetMaster.GetSound(soundName), false, Vector3.zero, 10f, 1, vol, pitch, true);
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0004A9D8 File Offset: 0x00048BD8
		public static Sound.SoundCapsule Play_Unpausable(string soundName, float vol = 1f, float pitch = 1f)
		{
			return Sound._Play(AssetMaster.GetSound(soundName), false, Vector3.zero, 10f, 1, vol, pitch, false);
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0004A9F4 File Offset: 0x00048BF4
		public static Sound.SoundCapsule Play3D(string soundName, Vector3 position, float distance3D, float vol = 1f, float pitch = 1f, AudioRolloffMode rollOffMode = 1)
		{
			return Sound._Play(AssetMaster.GetSound(soundName), true, position, distance3D, rollOffMode, vol, pitch, true);
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0004AA0A File Offset: 0x00048C0A
		public static Sound.SoundCapsule Play3D_Unpausable(string soundName, Vector3 position, float distance3D, float vol = 1f, float pitch = 1f, AudioRolloffMode rollOffMode = 1)
		{
			return Sound._Play(AssetMaster.GetSound(soundName), true, position, distance3D, rollOffMode, vol, pitch, false);
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x0004AA20 File Offset: 0x00048C20
		public static Sound.SoundCapsule PlayDelayed(string soundName, float delay, float vol = 1f, float pitch = 1f)
		{
			Sound.tempDelayVar = delay;
			return Sound.Play(soundName, vol, pitch);
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x0004AA30 File Offset: 0x00048C30
		public static Sound.SoundCapsule PlayDelayed_Unpausable(string soundName, float delay, float vol = 1f, float pitch = 1f)
		{
			Sound.tempDelayVar = delay;
			return Sound.Play_Unpausable(soundName, vol, pitch);
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x0004AA40 File Offset: 0x00048C40
		private static void _Stop(int index)
		{
			Sound.soundCapsulesPlaying[index].myAudioSource.Stop();
			Sound.soundCapsulesPlaying[index].paused = false;
			Sound.soundCapsulesPlaying[index].myGameObject.SetActive(false);
			Sound.soundCapsulesPool.Add(Sound.soundCapsulesPlaying[index]);
			Sound.soundCapsulesPlaying.RemoveAt(index);
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0004AAAC File Offset: 0x00048CAC
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

		// Token: 0x06000B1A RID: 2842 RVA: 0x0004AB08 File Offset: 0x00048D08
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

		// Token: 0x06000B1B RID: 2843 RVA: 0x0004AB44 File Offset: 0x00048D44
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

		// Token: 0x06000B1C RID: 2844 RVA: 0x0004ABAC File Offset: 0x00048DAC
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

		// Token: 0x06000B1D RID: 2845 RVA: 0x0004AC24 File Offset: 0x00048E24
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

		// Token: 0x06000B1E RID: 2846 RVA: 0x0004AC9C File Offset: 0x00048E9C
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

		// Token: 0x06000B1F RID: 2847 RVA: 0x0004AD37 File Offset: 0x00048F37
		public static bool IsPlaying(string clipName)
		{
			return Sound._IsPlaying(clipName, false);
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0004AD40 File Offset: 0x00048F40
		public static bool IsPlayingOrPaused(string clipName)
		{
			return Sound._IsPlaying(clipName, true);
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0004AD4C File Offset: 0x00048F4C
		public static float GetVolumeFinal(Sound.SoundCapsule capsule)
		{
			if (capsule == null)
			{
				return Sound.Volume * Sound.volumeFade * 0.75f * Data.settings.volumeMaster;
			}
			return capsule.localVolume * Sound.Volume * Sound.volumeFade * 0.75f * Data.settings.volumeMaster;
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x0004AD9C File Offset: 0x00048F9C
		private static void _ApplyVolumeToSoundCapsule(Sound.SoundCapsule capsule)
		{
			capsule.myAudioSource.volume = capsule.localVolume * Sound.Volume * Sound.volumeFade * 0.75f * Data.settings.volumeMaster;
		}

		// (get) Token: 0x06000B23 RID: 2851 RVA: 0x0004ADCC File Offset: 0x00048FCC
		// (set) Token: 0x06000B24 RID: 2852 RVA: 0x0004ADD3 File Offset: 0x00048FD3
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

		// Token: 0x06000B25 RID: 2853 RVA: 0x0004ADDB File Offset: 0x00048FDB
		public static void SetVolumeMain(float val)
		{
			Sound.Volume = val;
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x0004ADE3 File Offset: 0x00048FE3
		public static float GetVolumeMain()
		{
			return Sound.Volume;
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x0004ADEA File Offset: 0x00048FEA
		public static void SetVolumeFade(float val, float changeSpeed)
		{
			Sound.volumeFadeTo = val;
			Sound.volumeFadeSpeed = changeSpeed;
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0004ADF8 File Offset: 0x00048FF8
		public static void SetVolumeFadeInstant(float val)
		{
			Sound.volumeFade = val;
			Sound.volumeFadeTo = val;
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0004AE06 File Offset: 0x00049006
		public static float GetVolumeFade()
		{
			return Sound.volumeFade;
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0004AE0D File Offset: 0x0004900D
		public static float GetVolumeFadeTarget()
		{
			return Sound.volumeFadeTo;
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0004AE14 File Offset: 0x00049014
		public static float GetVolumeFadeChangeSpeed()
		{
			return Sound.volumeFadeSpeed;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0004AE1C File Offset: 0x0004901C
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

		public const float DIST_SMALLEST_FOR_REAL = 3f;

		public const float DIST_SMALLEST = 5f;

		public const float DIST_SMALL = 10f;

		public const float DIST_MEDIUM = 20f;

		public const float DIST_HIGH = 30f;

		private const float VOLUME_ADJUST = 0.75f;

		private static float volumeFade = 1f;

		private static float volumeFadeTo = 1f;

		private static float volumeFadeSpeed = 1f;

		public static bool pausableByGame = true;

		private static List<Sound.SoundCapsule> soundCapsulesPool = new List<Sound.SoundCapsule>();

		public static List<Sound.SoundCapsule> soundCapsulesPlaying = new List<Sound.SoundCapsule>();

		private static float tempDelayVar = 0f;

		private static List<Sound.SoundCapsule> delayedSounds = new List<Sound.SoundCapsule>();

		public class SoundCapsule
		{
			// Token: 0x06001291 RID: 4753 RVA: 0x000768D4 File Offset: 0x00074AD4
			public SoundCapsule(AudioClip sound)
			{
				this.myGameObject = new GameObject();
				this.myGameObject.name = "SOUND: " + sound.name;
				this.myGameObject.transform.SetParent(Master.instance.audioHolderTr);
				this.myAudioSource = this.myGameObject.AddComponent<AudioSource>();
				this.myAudioSource.clip = sound;
			}

			// Token: 0x06001292 RID: 4754 RVA: 0x00076958 File Offset: 0x00074B58
			~SoundCapsule()
			{
				if (this.myGameObject != null)
				{
					Object.Destroy(this.myGameObject);
				}
			}

			// Token: 0x06001293 RID: 4755 RVA: 0x00076998 File Offset: 0x00074B98
			public void StopRequest()
			{
				this.myAudioSource.Stop();
				this.paused = false;
				this.soundLife = 0f;
			}

			public float delayTimer;

			public bool doneDelay;

			public GameObject myGameObject;

			public AudioSource myAudioSource;

			public bool pausable = true;

			public bool paused;

			public float localVolume = 1f;

			public float soundLife;

			public AudioClip clip;
		}
	}
}
