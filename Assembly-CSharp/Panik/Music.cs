using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	public static class Music
	{
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x00049A71 File Offset: 0x00047C71
		// (set) Token: 0x06000ADE RID: 2782 RVA: 0x00049A7D File Offset: 0x00047C7D
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

		// Token: 0x06000ADF RID: 2783 RVA: 0x00049A8C File Offset: 0x00047C8C
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

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00049B26 File Offset: 0x00047D26
		private static bool IsCapsuleOk(Music.MusicCapsule capsule)
		{
			return capsule != null && !(capsule.myGameObject == null) && !(capsule.myAudioSource == null);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00049B50 File Offset: 0x00047D50
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

		// Token: 0x06000AE2 RID: 2786 RVA: 0x00049BC8 File Offset: 0x00047DC8
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

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00049CD1 File Offset: 0x00047ED1
		public static Music.MusicCapsule Play(string musicName)
		{
			return Music._Play(AssetMaster.GetMusic(musicName), false, Vector3.zero, 10f, 1, 1f, true);
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00049CF0 File Offset: 0x00047EF0
		public static Music.MusicCapsule Play_Unpausable(string musicName)
		{
			return Music._Play(AssetMaster.GetMusic(musicName), false, Vector3.zero, 10f, 1, 1f, false);
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00049D0F File Offset: 0x00047F0F
		public static Music.MusicCapsule Play3D(string musicName, Vector3 position, float distance3D, AudioRolloffMode rollOffMode = 1)
		{
			return Music._Play(AssetMaster.GetSound(musicName), true, position, distance3D, rollOffMode, 1f, true);
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00049D26 File Offset: 0x00047F26
		public static Music.MusicCapsule Play3D_Unpausable(string musicName, Vector3 position, float distance3D, AudioRolloffMode rollOffMode = 1)
		{
			return Music._Play(AssetMaster.GetSound(musicName), true, position, distance3D, rollOffMode, 1f, false);
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00049D3D File Offset: 0x00047F3D
		private static void Resume(Music.MusicCapsule capsule)
		{
			capsule.pausedLocally = false;
			if (!capsule.paused)
			{
				capsule.myAudioSource.UnPause();
			}
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00049D5C File Offset: 0x00047F5C
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

		// Token: 0x06000AE9 RID: 2793 RVA: 0x00049DC5 File Offset: 0x00047FC5
		private static void Pause(Music.MusicCapsule capsule)
		{
			capsule.pausedLocally = true;
			capsule.myAudioSource.Pause();
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x00049DDC File Offset: 0x00047FDC
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

		// Token: 0x06000AEB RID: 2795 RVA: 0x00049E48 File Offset: 0x00048048
		private static void _Stop(int index)
		{
			Music.musicCapsulesPlaying[index].myAudioSource.Stop();
			Music.musicCapsulesPlaying[index].paused = false;
			Music.musicCapsulesPlaying[index].pausedLocally = false;
			Music.musicCapsulesPlaying[index].myGameObject.SetActive(false);
			Music.musicCapsulesPool.Add(Music.musicCapsulesPlaying[index]);
			Music.musicCapsulesPlaying.RemoveAt(index);
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00049EC4 File Offset: 0x000480C4
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

		// Token: 0x06000AED RID: 2797 RVA: 0x00049F28 File Offset: 0x00048128
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

		// Token: 0x06000AEE RID: 2798 RVA: 0x00049F64 File Offset: 0x00048164
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

		// Token: 0x06000AEF RID: 2799 RVA: 0x00049FCC File Offset: 0x000481CC
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

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0004A058 File Offset: 0x00048258
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

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0004A0E4 File Offset: 0x000482E4
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

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0004A1AF File Offset: 0x000483AF
		public static bool IsPlaying(string clipName)
		{
			return Music._IsPlaying(clipName, false);
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x0004A1B8 File Offset: 0x000483B8
		public static bool IsPlayingOrPaused(string clipName)
		{
			return Music._IsPlaying(clipName, true);
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0004A1C4 File Offset: 0x000483C4
		public static float GetVolumeFinal(Music.MusicCapsule capsule)
		{
			if (capsule == null)
			{
				return Music.Volume * Music.volumeFade * 1f * Data.settings.volumeMaster;
			}
			return capsule.localVolume * Music.Volume * Music.volumeFade * 1f * Data.settings.volumeMaster;
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0004A214 File Offset: 0x00048414
		private static void _ApplyVolumeToMusicCapsule(Music.MusicCapsule capsule)
		{
			capsule.myAudioSource.volume = capsule.localVolume * Music.Volume * Music.volumeFade * 1f * Data.settings.volumeMaster;
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0004A244 File Offset: 0x00048444
		private static void _ApplyPitchToMusicCapsule(Music.MusicCapsule capsule)
		{
			capsule.myAudioSource.pitch = Music.pitch;
		}

		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x0004A256 File Offset: 0x00048456
		// (set) Token: 0x06000AF8 RID: 2808 RVA: 0x0004A25D File Offset: 0x0004845D
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

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0004A265 File Offset: 0x00048465
		public static void SetVolumeMain(float val)
		{
			Music.Volume = val;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0004A26D File Offset: 0x0004846D
		public static float GetVolumeMain()
		{
			return Music.Volume;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0004A274 File Offset: 0x00048474
		public static void SetVolumeFade(float val, float changeSpeed)
		{
			Music.volumeFadeTo = val;
			Music.volumeFadeSpeed = changeSpeed;
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0004A282 File Offset: 0x00048482
		public static void SetVolumeFadeInstant(float val)
		{
			Music.volumeFade = val;
			Music.volumeFadeTo = val;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0004A290 File Offset: 0x00048490
		public static float GetVolumeFade()
		{
			return Music.volumeFade;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x0004A297 File Offset: 0x00048497
		public static float GetVolumeFadeTarget()
		{
			return Music.volumeFadeTo;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0004A29E File Offset: 0x0004849E
		public static float GetVolumeFadeChangeSpeed()
		{
			return Music.volumeFadeSpeed;
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x0004A2A8 File Offset: 0x000484A8
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

		// Token: 0x06000B01 RID: 2817 RVA: 0x0004A389 File Offset: 0x00048589
		private static void _ResumeMusic_BySystem(Music.MusicCapsule capsule)
		{
			capsule.paused = false;
			capsule.myAudioSource.UnPause();
		}

		private const float VOLUME_ADJUST = 1f;

		private static float volumeFade = 1f;

		private static float volumeFadeTo = 1f;

		private static float volumeFadeSpeed = 1f;

		public static float pitch = 1f;

		public static bool pausableByGame = true;

		private static List<Music.MusicCapsule> musicCapsulesPool = new List<Music.MusicCapsule>();

		public static List<Music.MusicCapsule> musicCapsulesPlaying = new List<Music.MusicCapsule>();

		public class MusicCapsule
		{
			// Token: 0x0600128E RID: 4750 RVA: 0x000767F4 File Offset: 0x000749F4
			public MusicCapsule(AudioClip sound)
			{
				this.myGameObject = new GameObject();
				this.myGameObject.name = "SOUND: " + sound.name;
				this.myGameObject.transform.SetParent(Master.instance.audioHolderTr);
				this.myAudioSource = this.myGameObject.AddComponent<AudioSource>();
				this.myAudioSource.clip = sound;
			}

			// Token: 0x0600128F RID: 4751 RVA: 0x00076878 File Offset: 0x00074A78
			~MusicCapsule()
			{
				if (this.myGameObject != null)
				{
					Object.Destroy(this.myGameObject);
				}
			}

			public GameObject myGameObject;

			public AudioSource myAudioSource;

			public bool pausableByMusicSystem = true;

			public bool paused;

			public bool pausedLocally;

			public float localVolume = 1f;
		}
	}
}
