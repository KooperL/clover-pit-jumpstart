using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	public static class Music
	{
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x0004A1D1 File Offset: 0x000483D1
		// (set) Token: 0x06000AF3 RID: 2803 RVA: 0x0004A1DD File Offset: 0x000483DD
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

		// Token: 0x06000AF4 RID: 2804 RVA: 0x0004A1EC File Offset: 0x000483EC
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

		// Token: 0x06000AF5 RID: 2805 RVA: 0x0004A286 File Offset: 0x00048486
		private static bool IsCapsuleOk(Music.MusicCapsule capsule)
		{
			return capsule != null && !(capsule.myGameObject == null) && !(capsule.myAudioSource == null);
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x0004A2B0 File Offset: 0x000484B0
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

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0004A328 File Offset: 0x00048528
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

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0004A431 File Offset: 0x00048631
		public static Music.MusicCapsule Play(string musicName)
		{
			return Music._Play(AssetMaster.GetMusic(musicName), false, Vector3.zero, 10f, 1, 1f, true);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0004A450 File Offset: 0x00048650
		public static Music.MusicCapsule Play_Unpausable(string musicName)
		{
			return Music._Play(AssetMaster.GetMusic(musicName), false, Vector3.zero, 10f, 1, 1f, false);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0004A46F File Offset: 0x0004866F
		public static Music.MusicCapsule Play3D(string musicName, Vector3 position, float distance3D, AudioRolloffMode rollOffMode = 1)
		{
			return Music._Play(AssetMaster.GetSound(musicName), true, position, distance3D, rollOffMode, 1f, true);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x0004A486 File Offset: 0x00048686
		public static Music.MusicCapsule Play3D_Unpausable(string musicName, Vector3 position, float distance3D, AudioRolloffMode rollOffMode = 1)
		{
			return Music._Play(AssetMaster.GetSound(musicName), true, position, distance3D, rollOffMode, 1f, false);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x0004A49D File Offset: 0x0004869D
		private static void Resume(Music.MusicCapsule capsule)
		{
			capsule.pausedLocally = false;
			if (!capsule.paused)
			{
				capsule.myAudioSource.UnPause();
			}
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0004A4BC File Offset: 0x000486BC
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

		// Token: 0x06000AFE RID: 2814 RVA: 0x0004A525 File Offset: 0x00048725
		private static void Pause(Music.MusicCapsule capsule)
		{
			capsule.pausedLocally = true;
			capsule.myAudioSource.Pause();
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x0004A53C File Offset: 0x0004873C
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

		// Token: 0x06000B00 RID: 2816 RVA: 0x0004A5A8 File Offset: 0x000487A8
		private static void _Stop(int index)
		{
			Music.musicCapsulesPlaying[index].myAudioSource.Stop();
			Music.musicCapsulesPlaying[index].paused = false;
			Music.musicCapsulesPlaying[index].pausedLocally = false;
			Music.musicCapsulesPlaying[index].myGameObject.SetActive(false);
			Music.musicCapsulesPool.Add(Music.musicCapsulesPlaying[index]);
			Music.musicCapsulesPlaying.RemoveAt(index);
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x0004A624 File Offset: 0x00048824
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

		// Token: 0x06000B02 RID: 2818 RVA: 0x0004A688 File Offset: 0x00048888
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

		// Token: 0x06000B03 RID: 2819 RVA: 0x0004A6C4 File Offset: 0x000488C4
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

		// Token: 0x06000B04 RID: 2820 RVA: 0x0004A72C File Offset: 0x0004892C
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

		// Token: 0x06000B05 RID: 2821 RVA: 0x0004A7B8 File Offset: 0x000489B8
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

		// Token: 0x06000B06 RID: 2822 RVA: 0x0004A844 File Offset: 0x00048A44
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

		// Token: 0x06000B07 RID: 2823 RVA: 0x0004A90F File Offset: 0x00048B0F
		public static bool IsPlaying(string clipName)
		{
			return Music._IsPlaying(clipName, false);
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0004A918 File Offset: 0x00048B18
		public static bool IsPlayingOrPaused(string clipName)
		{
			return Music._IsPlaying(clipName, true);
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0004A924 File Offset: 0x00048B24
		public static float GetVolumeFinal(Music.MusicCapsule capsule)
		{
			if (capsule == null)
			{
				return Music.Volume * Music.volumeFade * 1f * Data.settings.volumeMaster;
			}
			return capsule.localVolume * Music.Volume * Music.volumeFade * 1f * Data.settings.volumeMaster;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0004A974 File Offset: 0x00048B74
		private static void _ApplyVolumeToMusicCapsule(Music.MusicCapsule capsule)
		{
			capsule.myAudioSource.volume = capsule.localVolume * Music.Volume * Music.volumeFade * 1f * Data.settings.volumeMaster;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x0004A9A4 File Offset: 0x00048BA4
		private static void _ApplyPitchToMusicCapsule(Music.MusicCapsule capsule)
		{
			capsule.myAudioSource.pitch = Music.pitch;
		}

		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x0004A9B6 File Offset: 0x00048BB6
		// (set) Token: 0x06000B0D RID: 2829 RVA: 0x0004A9BD File Offset: 0x00048BBD
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

		// Token: 0x06000B0E RID: 2830 RVA: 0x0004A9C5 File Offset: 0x00048BC5
		public static void SetVolumeMain(float val)
		{
			Music.Volume = val;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x0004A9CD File Offset: 0x00048BCD
		public static float GetVolumeMain()
		{
			return Music.Volume;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0004A9D4 File Offset: 0x00048BD4
		public static void SetVolumeFade(float val, float changeSpeed)
		{
			Music.volumeFadeTo = val;
			Music.volumeFadeSpeed = changeSpeed;
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x0004A9E2 File Offset: 0x00048BE2
		public static void SetVolumeFadeInstant(float val)
		{
			Music.volumeFade = val;
			Music.volumeFadeTo = val;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0004A9F0 File Offset: 0x00048BF0
		public static float GetVolumeFade()
		{
			return Music.volumeFade;
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x0004A9F7 File Offset: 0x00048BF7
		public static float GetVolumeFadeTarget()
		{
			return Music.volumeFadeTo;
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x0004A9FE File Offset: 0x00048BFE
		public static float GetVolumeFadeChangeSpeed()
		{
			return Music.volumeFadeSpeed;
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x0004AA08 File Offset: 0x00048C08
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

		// Token: 0x06000B16 RID: 2838 RVA: 0x0004AAE9 File Offset: 0x00048CE9
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
			// Token: 0x060012A5 RID: 4773 RVA: 0x00077088 File Offset: 0x00075288
			public MusicCapsule(AudioClip sound)
			{
				this.myGameObject = new GameObject();
				this.myGameObject.name = "SOUND: " + sound.name;
				this.myGameObject.transform.SetParent(Master.instance.audioHolderTr);
				this.myAudioSource = this.myGameObject.AddComponent<AudioSource>();
				this.myAudioSource.clip = sound;
			}

			// Token: 0x060012A6 RID: 4774 RVA: 0x0007710C File Offset: 0x0007530C
			~MusicCapsule()
			{
				if (this.myGameObject != null)
				{
					global::UnityEngine.Object.Destroy(this.myGameObject);
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
