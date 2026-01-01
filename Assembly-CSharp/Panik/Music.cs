using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	// Token: 0x02000110 RID: 272
	public static class Music
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0001069C File Offset: 0x0000E89C
		// (set) Token: 0x06000CD1 RID: 3281 RVA: 0x000106A8 File Offset: 0x0000E8A8
		private static float Volume
		{
			get
			{
				return Data.settings.volumeMusic;
			}
			set
			{
				Data.settings.volumeMusic = value;
			}
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x000642B8 File Offset: 0x000624B8
		private static Music.MusicCapsule GetMusicCapsule(AudioClip clip)
		{
			Music.MusicCapsule musicCapsule;
			if (Music.musicCapsulesPool.Count > 0)
			{
				musicCapsule = Music.musicCapsulesPool[Music.musicCapsulesPool.Count - 1];
				musicCapsule.myGameObject.SetActive(true);
				musicCapsule.myGameObject.name = "SOUND: " + clip.name;
				musicCapsule.myAudioSource.clip = clip;
				Music.musicCapsulesPlaying.Add(musicCapsule);
				Music.musicCapsulesPool.RemoveAt(Music.musicCapsulesPool.Count - 1);
				return musicCapsule;
			}
			musicCapsule = new Music.MusicCapsule(clip);
			Music.musicCapsulesPlaying.Add(musicCapsule);
			return musicCapsule;
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x000106B5 File Offset: 0x0000E8B5
		private static bool IsCapsuleOk(Music.MusicCapsule capsule)
		{
			return capsule != null && !(capsule.myGameObject == null) && !(capsule.myAudioSource == null);
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00064354 File Offset: 0x00062554
		private static void CheckListsForNullValues()
		{
			for (int i = Music.musicCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (!Music.IsCapsuleOk(Music.musicCapsulesPlaying[i]))
				{
					Music.musicCapsulesPlaying.RemoveAt(i);
				}
			}
			for (int j = Music.musicCapsulesPool.Count - 1; j >= 0; j--)
			{
				if (!Music.IsCapsuleOk(Music.musicCapsulesPool[j]))
				{
					Music.musicCapsulesPool.RemoveAt(j);
				}
			}
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x000643CC File Offset: 0x000625CC
		public static Music.MusicCapsule _Play(AudioClip musicClip, bool _3d, Vector3 position, float distance3D, AudioRolloffMode rollOffMode, float vol, bool pausable)
		{
			for (int i = 0; i < Music.musicCapsulesPlaying.Count; i++)
			{
				if (Music.musicCapsulesPlaying[i].myAudioSource.clip == musicClip)
				{
					if (Music.musicCapsulesPlaying[i].pausedLocally)
					{
						Music.Resume(Music.musicCapsulesPlaying[i]);
					}
					return Music.musicCapsulesPlaying[i];
				}
			}
			Music.MusicCapsule musicCapsule = Music.GetMusicCapsule(musicClip);
			musicCapsule.myAudioSource.spatialBlend = (_3d ? 1f : 0f);
			musicCapsule.localVolume = vol;
			Music._ApplyVolumeToMusicCapsule(musicCapsule);
			musicCapsule.myAudioSource.pitch = Music.pitch;
			if (_3d)
			{
				musicCapsule.myAudioSource.rolloffMode = rollOffMode;
				musicCapsule.myAudioSource.maxDistance = distance3D;
				musicCapsule.myAudioSource.minDistance = 0f;
				musicCapsule.myGameObject.transform.position = position;
			}
			musicCapsule.myAudioSource.loop = true;
			musicCapsule.pausableByMusicSystem = pausable;
			musicCapsule.myAudioSource.Play();
			return musicCapsule;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x000106DD File Offset: 0x0000E8DD
		public static Music.MusicCapsule Play(string musicName)
		{
			return Music._Play(AssetMaster.GetMusic(musicName), false, Vector3.zero, 10f, AudioRolloffMode.Linear, 1f, true);
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x000106FC File Offset: 0x0000E8FC
		public static Music.MusicCapsule Play_Unpausable(string musicName)
		{
			return Music._Play(AssetMaster.GetMusic(musicName), false, Vector3.zero, 10f, AudioRolloffMode.Linear, 1f, false);
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0001071B File Offset: 0x0000E91B
		public static Music.MusicCapsule Play3D(string musicName, Vector3 position, float distance3D, AudioRolloffMode rollOffMode = AudioRolloffMode.Linear)
		{
			return Music._Play(AssetMaster.GetSound(musicName), true, position, distance3D, rollOffMode, 1f, true);
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00010732 File Offset: 0x0000E932
		public static Music.MusicCapsule Play3D_Unpausable(string musicName, Vector3 position, float distance3D, AudioRolloffMode rollOffMode = AudioRolloffMode.Linear)
		{
			return Music._Play(AssetMaster.GetSound(musicName), true, position, distance3D, rollOffMode, 1f, false);
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x00010749 File Offset: 0x0000E949
		private static void Resume(Music.MusicCapsule capsule)
		{
			capsule.pausedLocally = false;
			if (!capsule.paused)
			{
				capsule.myAudioSource.UnPause();
			}
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x000644D8 File Offset: 0x000626D8
		public static void Resume(string clipName)
		{
			for (int i = Music.musicCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (Music.IsCapsuleOk(Music.musicCapsulesPlaying[i]) && Music.musicCapsulesPlaying[i].myAudioSource.clip.name == clipName)
				{
					Music.Resume(Music.musicCapsulesPlaying[i]);
					return;
				}
			}
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x00010765 File Offset: 0x0000E965
		private static void Pause(Music.MusicCapsule capsule)
		{
			capsule.pausedLocally = true;
			capsule.myAudioSource.Pause();
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x00064544 File Offset: 0x00062744
		public static void Pause(string clipName)
		{
			for (int i = Music.musicCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (Music.IsCapsuleOk(Music.musicCapsulesPlaying[i]) && Music.musicCapsulesPlaying[i].myAudioSource.clip.name == clipName)
				{
					Music.Pause(Music.musicCapsulesPlaying[i]);
					return;
				}
			}
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x000645B0 File Offset: 0x000627B0
		private static void _Stop(int index)
		{
			Music.musicCapsulesPlaying[index].myAudioSource.Stop();
			Music.musicCapsulesPlaying[index].paused = false;
			Music.musicCapsulesPlaying[index].pausedLocally = false;
			Music.musicCapsulesPlaying[index].myGameObject.SetActive(false);
			Music.musicCapsulesPool.Add(Music.musicCapsulesPlaying[index]);
			Music.musicCapsulesPlaying.RemoveAt(index);
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0006462C File Offset: 0x0006282C
		public static void Stop(string clipName, bool stopAllSoundsWithSameName = true)
		{
			for (int i = Music.musicCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (Music.IsCapsuleOk(Music.musicCapsulesPlaying[i]) && Music.musicCapsulesPlaying[i].myAudioSource.clip.name == clipName)
				{
					Music._Stop(i);
					if (!stopAllSoundsWithSameName)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x00064690 File Offset: 0x00062890
		public static void StopAll()
		{
			for (int i = Music.musicCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (Music.IsCapsuleOk(Music.musicCapsulesPlaying[i]))
				{
					Music._Stop(i);
				}
			}
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x000646CC File Offset: 0x000628CC
		public static Music.MusicCapsule Find(string clipName)
		{
			for (int i = Music.musicCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (Music.IsCapsuleOk(Music.musicCapsulesPlaying[i]) && Music.musicCapsulesPlaying[i].myAudioSource.clip.name == clipName)
				{
					return Music.musicCapsulesPlaying[i];
				}
			}
			return null;
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x00064734 File Offset: 0x00062934
		public static Music.MusicCapsule FindUnpaused(string clipName)
		{
			for (int i = Music.musicCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (Music.IsCapsuleOk(Music.musicCapsulesPlaying[i]) && !Music.musicCapsulesPlaying[i].paused && !Music.musicCapsulesPlaying[i].pausedLocally && Music.musicCapsulesPlaying[i].myAudioSource.clip.name == clipName)
				{
					return Music.musicCapsulesPlaying[i];
				}
			}
			return null;
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x000647C0 File Offset: 0x000629C0
		public static Music.MusicCapsule FindPaused(string clipName)
		{
			for (int i = Music.musicCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (Music.IsCapsuleOk(Music.musicCapsulesPlaying[i]) && (Music.musicCapsulesPlaying[i].paused || Music.musicCapsulesPlaying[i].pausedLocally) && Music.musicCapsulesPlaying[i].myAudioSource.clip.name == clipName)
				{
					return Music.musicCapsulesPlaying[i];
				}
			}
			return null;
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x0006484C File Offset: 0x00062A4C
		public static bool _IsPlaying(string clipName, bool returnTrueIfPausedMusicIsFound)
		{
			for (int i = Music.musicCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				if (Music.IsCapsuleOk(Music.musicCapsulesPlaying[i]) && ((!Music.musicCapsulesPlaying[i].paused && !Music.musicCapsulesPlaying[i].pausedLocally) || returnTrueIfPausedMusicIsFound) && Music.musicCapsulesPlaying[i].myAudioSource.clip.name == clipName && (Music.musicCapsulesPlaying[i].myAudioSource.isPlaying || ((Music.musicCapsulesPlaying[i].paused || Music.musicCapsulesPlaying[i].pausedLocally) && returnTrueIfPausedMusicIsFound)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00010779 File Offset: 0x0000E979
		public static bool IsPlaying(string clipName)
		{
			return Music._IsPlaying(clipName, false);
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x00010782 File Offset: 0x0000E982
		public static bool IsPlayingOrPaused(string clipName)
		{
			return Music._IsPlaying(clipName, true);
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x00064918 File Offset: 0x00062B18
		public static float GetVolumeFinal(Music.MusicCapsule capsule)
		{
			if (capsule == null)
			{
				return Music.Volume * Music.volumeFade * 1f * Data.settings.volumeMaster;
			}
			return capsule.localVolume * Music.Volume * Music.volumeFade * 1f * Data.settings.volumeMaster;
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0001078B File Offset: 0x0000E98B
		private static void _ApplyVolumeToMusicCapsule(Music.MusicCapsule capsule)
		{
			capsule.myAudioSource.volume = capsule.localVolume * Music.Volume * Music.volumeFade * 1f * Data.settings.volumeMaster;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x000107BB File Offset: 0x0000E9BB
		private static void _ApplyPitchToMusicCapsule(Music.MusicCapsule capsule)
		{
			capsule.myAudioSource.pitch = Music.pitch;
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x000107CD File Offset: 0x0000E9CD
		// (set) Token: 0x06000CEB RID: 3307 RVA: 0x000107D4 File Offset: 0x0000E9D4
		public static float VolumeMain
		{
			get
			{
				return Music.Volume;
			}
			set
			{
				Music.Volume = value;
			}
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x000107D4 File Offset: 0x0000E9D4
		public static void SetVolumeMain(float val)
		{
			Music.Volume = val;
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x000107CD File Offset: 0x0000E9CD
		public static float GetVolumeMain()
		{
			return Music.Volume;
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x000107DC File Offset: 0x0000E9DC
		public static void SetVolumeFade(float val, float changeSpeed)
		{
			Music.volumeFadeTo = val;
			Music.volumeFadeSpeed = changeSpeed;
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x000107EA File Offset: 0x0000E9EA
		public static void SetVolumeFadeInstant(float val)
		{
			Music.volumeFade = val;
			Music.volumeFadeTo = val;
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x000107F8 File Offset: 0x0000E9F8
		public static float GetVolumeFade()
		{
			return Music.volumeFade;
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x000107FF File Offset: 0x0000E9FF
		public static float GetVolumeFadeTarget()
		{
			return Music.volumeFadeTo;
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x00010806 File Offset: 0x0000EA06
		public static float GetVolumeFadeChangeSpeed()
		{
			return Music.volumeFadeSpeed;
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x00064968 File Offset: 0x00062B68
		public static void Routine()
		{
			Music.CheckListsForNullValues();
			if (Music.volumeFade < Music.volumeFadeTo)
			{
				Music.volumeFade = Mathf.Min(Music.volumeFade + Tick.TimeUnscaled * Music.volumeFadeSpeed, Music.volumeFadeTo);
			}
			if (Music.volumeFade > Music.volumeFadeTo)
			{
				Music.volumeFade = Mathf.Max(Music.volumeFade - Tick.TimeUnscaled * Music.volumeFadeSpeed, Music.volumeFadeTo);
			}
			for (int i = Music.musicCapsulesPlaying.Count - 1; i >= 0; i--)
			{
				Music.MusicCapsule musicCapsule = Music.musicCapsulesPlaying[i];
				Music._ApplyVolumeToMusicCapsule(musicCapsule);
				Music._ApplyPitchToMusicCapsule(musicCapsule);
				if (Tick.Paused && Music.pausableByGame)
				{
					if (musicCapsule.pausableByMusicSystem && !musicCapsule.paused)
					{
						musicCapsule.paused = true;
						musicCapsule.myAudioSource.Pause();
					}
				}
				else if (musicCapsule.paused && !musicCapsule.pausedLocally)
				{
					Music._ResumeMusic_BySystem(musicCapsule);
				}
			}
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0001080D File Offset: 0x0000EA0D
		private static void _ResumeMusic_BySystem(Music.MusicCapsule capsule)
		{
			capsule.paused = false;
			capsule.myAudioSource.UnPause();
		}

		// Token: 0x04000D9E RID: 3486
		private const float VOLUME_ADJUST = 1f;

		// Token: 0x04000D9F RID: 3487
		private static float volumeFade = 1f;

		// Token: 0x04000DA0 RID: 3488
		private static float volumeFadeTo = 1f;

		// Token: 0x04000DA1 RID: 3489
		private static float volumeFadeSpeed = 1f;

		// Token: 0x04000DA2 RID: 3490
		public static float pitch = 1f;

		// Token: 0x04000DA3 RID: 3491
		public static bool pausableByGame = true;

		// Token: 0x04000DA4 RID: 3492
		private static List<Music.MusicCapsule> musicCapsulesPool = new List<Music.MusicCapsule>();

		// Token: 0x04000DA5 RID: 3493
		public static List<Music.MusicCapsule> musicCapsulesPlaying = new List<Music.MusicCapsule>();

		// Token: 0x02000111 RID: 273
		public class MusicCapsule
		{
			// Token: 0x06000CF6 RID: 3318 RVA: 0x00064A9C File Offset: 0x00062C9C
			public MusicCapsule(AudioClip sound)
			{
				this.myGameObject = new GameObject();
				this.myGameObject.name = "SOUND: " + sound.name;
				this.myGameObject.transform.SetParent(Master.instance.audioHolderTr);
				this.myAudioSource = this.myGameObject.AddComponent<AudioSource>();
				this.myAudioSource.clip = sound;
			}

			// Token: 0x06000CF7 RID: 3319 RVA: 0x00064B20 File Offset: 0x00062D20
			~MusicCapsule()
			{
				if (this.myGameObject != null)
				{
					global::UnityEngine.Object.Destroy(this.myGameObject);
				}
			}

			// Token: 0x04000DA6 RID: 3494
			public GameObject myGameObject;

			// Token: 0x04000DA7 RID: 3495
			public AudioSource myAudioSource;

			// Token: 0x04000DA8 RID: 3496
			public bool pausableByMusicSystem = true;

			// Token: 0x04000DA9 RID: 3497
			public bool paused;

			// Token: 0x04000DAA RID: 3498
			public bool pausedLocally;

			// Token: 0x04000DAB RID: 3499
			public float localVolume = 1f;
		}
	}
}
