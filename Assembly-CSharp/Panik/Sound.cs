using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	public static class Sound
	{
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x0004AE7A File Offset: 0x0004907A
		// (set) Token: 0x06000B22 RID: 2850 RVA: 0x0004AE86 File Offset: 0x00049086
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

		// Token: 0x06000B23 RID: 2851 RVA: 0x0004AE94 File Offset: 0x00049094
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

		// Token: 0x06000B24 RID: 2852 RVA: 0x0004AF87 File Offset: 0x00049187
		private static bool IsCapsuleOk(Sound.SoundCapsule capsule)
		{
			return capsule != null && !(capsule.myGameObject == null) && !(capsule.myAudioSource == null);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x0004AFB0 File Offset: 0x000491B0
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

		// Token: 0x06000B26 RID: 2854 RVA: 0x0004B028 File Offset: 0x00049228
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

		// Token: 0x06000B27 RID: 2855 RVA: 0x0004B11C File Offset: 0x0004931C
		public static Sound.SoundCapsule Play(string soundName, float vol = 1f, float pitch = 1f)
		{
			return Sound._Play(AssetMaster.GetSound(soundName), false, Vector3.zero, 10f, 1, vol, pitch, true);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x0004B138 File Offset: 0x00049338
		public static Sound.SoundCapsule Play_Unpausable(string soundName, float vol = 1f, float pitch = 1f)
		{
			return Sound._Play(AssetMaster.GetSound(soundName), false, Vector3.zero, 10f, 1, vol, pitch, false);
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0004B154 File Offset: 0x00049354
		public static Sound.SoundCapsule Play3D(string soundName, Vector3 position, float distance3D, float vol = 1f, float pitch = 1f, AudioRolloffMode rollOffMode = 1)
		{
			return Sound._Play(AssetMaster.GetSound(soundName), true, position, distance3D, rollOffMode, vol, pitch, true);
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x0004B16A File Offset: 0x0004936A
		public static Sound.SoundCapsule Play3D_Unpausable(string soundName, Vector3 position, float distance3D, float vol = 1f, float pitch = 1f, AudioRolloffMode rollOffMode = 1)
		{
			return Sound._Play(AssetMaster.GetSound(soundName), true, position, distance3D, rollOffMode, vol, pitch, false);
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x0004B180 File Offset: 0x00049380
		public static Sound.SoundCapsule PlayDelayed(string soundName, float delay, float vol = 1f, float pitch = 1f)
		{
			Sound.tempDelayVar = delay;
			return Sound.Play(soundName, vol, pitch);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x0004B190 File Offset: 0x00049390
		public static Sound.SoundCapsule PlayDelayed_Unpausable(string soundName, float delay, float vol = 1f, float pitch = 1f)
		{
			Sound.tempDelayVar = delay;
			return Sound.Play_Unpausable(soundName, vol, pitch);
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x0004B1A0 File Offset: 0x000493A0
		private static void _Stop(int index)
		{
			Sound.soundCapsulesPlaying[index].myAudioSource.Stop();
			Sound.soundCapsulesPlaying[index].paused = false;
			Sound.soundCapsulesPlaying[index].myGameObject.SetActive(false);
			Sound.soundCapsulesPool.Add(Sound.soundCapsulesPlaying[index]);
			Sound.soundCapsulesPlaying.RemoveAt(index);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x0004B20C File Offset: 0x0004940C
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

		// Token: 0x06000B2F RID: 2863 RVA: 0x0004B268 File Offset: 0x00049468
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

		// Token: 0x06000B30 RID: 2864 RVA: 0x0004B2A4 File Offset: 0x000494A4
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

		// Token: 0x06000B31 RID: 2865 RVA: 0x0004B30C File Offset: 0x0004950C
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

		// Token: 0x06000B32 RID: 2866 RVA: 0x0004B384 File Offset: 0x00049584
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

		// Token: 0x06000B33 RID: 2867 RVA: 0x0004B3FC File Offset: 0x000495FC
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

		// Token: 0x06000B34 RID: 2868 RVA: 0x0004B497 File Offset: 0x00049697
		public static bool IsPlaying(string clipName)
		{
			return Sound._IsPlaying(clipName, false);
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0004B4A0 File Offset: 0x000496A0
		public static bool IsPlayingOrPaused(string clipName)
		{
			return Sound._IsPlaying(clipName, true);
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0004B4AC File Offset: 0x000496AC
		public static float GetVolumeFinal(Sound.SoundCapsule capsule)
		{
			if (capsule == null)
			{
				return Sound.Volume * Sound.volumeFade * 0.75f * Data.settings.volumeMaster;
			}
			return capsule.localVolume * Sound.Volume * Sound.volumeFade * 0.75f * Data.settings.volumeMaster;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0004B4FC File Offset: 0x000496FC
		private static void _ApplyVolumeToSoundCapsule(Sound.SoundCapsule capsule)
		{
			capsule.myAudioSource.volume = capsule.localVolume * Sound.Volume * Sound.volumeFade * 0.75f * Data.settings.volumeMaster;
		}

		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x0004B52C File Offset: 0x0004972C
		// (set) Token: 0x06000B39 RID: 2873 RVA: 0x0004B533 File Offset: 0x00049733
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

		// Token: 0x06000B3A RID: 2874 RVA: 0x0004B53B File Offset: 0x0004973B
		public static void SetVolumeMain(float val)
		{
			Sound.Volume = val;
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0004B543 File Offset: 0x00049743
		public static float GetVolumeMain()
		{
			return Sound.Volume;
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x0004B54A File Offset: 0x0004974A
		public static void SetVolumeFade(float val, float changeSpeed)
		{
			Sound.volumeFadeTo = val;
			Sound.volumeFadeSpeed = changeSpeed;
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0004B558 File Offset: 0x00049758
		public static void SetVolumeFadeInstant(float val)
		{
			Sound.volumeFade = val;
			Sound.volumeFadeTo = val;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0004B566 File Offset: 0x00049766
		public static float GetVolumeFade()
		{
			return Sound.volumeFade;
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0004B56D File Offset: 0x0004976D
		public static float GetVolumeFadeTarget()
		{
			return Sound.volumeFadeTo;
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x0004B574 File Offset: 0x00049774
		public static float GetVolumeFadeChangeSpeed()
		{
			return Sound.volumeFadeSpeed;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x0004B57C File Offset: 0x0004977C
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
			// Token: 0x060012A8 RID: 4776 RVA: 0x00077168 File Offset: 0x00075368
			public SoundCapsule(AudioClip sound)
			{
				this.myGameObject = new GameObject();
				this.myGameObject.name = "SOUND: " + sound.name;
				this.myGameObject.transform.SetParent(Master.instance.audioHolderTr);
				this.myAudioSource = this.myGameObject.AddComponent<AudioSource>();
				this.myAudioSource.clip = sound;
			}

			// Token: 0x060012A9 RID: 4777 RVA: 0x000771EC File Offset: 0x000753EC
			~SoundCapsule()
			{
				if (this.myGameObject != null)
				{
					global::UnityEngine.Object.Destroy(this.myGameObject);
				}
			}

			// Token: 0x060012AA RID: 4778 RVA: 0x0007722C File Offset: 0x0007542C
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
