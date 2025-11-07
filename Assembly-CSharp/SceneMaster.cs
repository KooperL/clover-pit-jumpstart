using System;
using Panik;
using UnityEngine;

public class SceneMaster : MonoBehaviour
{
	// Token: 0x06000375 RID: 885 RVA: 0x000158D8 File Offset: 0x00013AD8
	public static void Initialize()
	{
		Level.onSceneAwake = (Level.Ev)Delegate.Combine(Level.onSceneAwake, new Level.Ev(SceneMaster.OnSceneAwake));
		Level.onSceneStart = (Level.Ev)Delegate.Combine(Level.onSceneStart, new Level.Ev(SceneMaster.OnSceneStart));
		Level.onSceneEnd = (Level.Ev)Delegate.Combine(Level.onSceneEnd, new Level.Ev(SceneMaster.OnSceneEnd));
		Level.onLoadingSceneStart = (Level.Ev)Delegate.Combine(Level.onLoadingSceneStart, new Level.Ev(SceneMaster.OnLoadingSceneStart));
		Level.onLoadingSceneEnd = (Level.Ev)Delegate.Combine(Level.onLoadingSceneEnd, new Level.Ev(SceneMaster.OnLoadingSceneEnd));
	}

	// Token: 0x06000376 RID: 886 RVA: 0x00015985 File Offset: 0x00013B85
	public static void OnSceneAwake()
	{
		if (Level.CurrentSceneIndex != Level.SceneIndex.Game)
		{
			VirtualCursors.CursorDesiredVisibilitySet(0, true);
		}
	}

	// Token: 0x06000377 RID: 887 RVA: 0x00015998 File Offset: 0x00013B98
	public static void OnSceneStart()
	{
		RenderingMaster.RenderingRefresh(false);
		if (Level.CurrentSceneIndex != Level.SceneIndex.Loading && Level.CurrentSceneIndex != Level.SceneIndex.Intro)
		{
			TransitionScript.In();
		}
		if (!Music.IsPlaying("SoundtrackLoopAmbience1"))
		{
			Music.Play("SoundtrackLoopAmbience1");
			Music.SetVolumeFadeInstant(0f);
			Music.SetVolumeFade(1f, 0.25f);
		}
	}

	// Token: 0x06000378 RID: 888 RVA: 0x000159F0 File Offset: 0x00013BF0
	public static void OnLoadingSceneStart()
	{
	}

	// Token: 0x06000379 RID: 889 RVA: 0x000159F2 File Offset: 0x00013BF2
	public static void OnSceneEnd()
	{
		Tick.Paused = false;
		Tick.FreezeTimer = 0f;
	}

	// Token: 0x0600037A RID: 890 RVA: 0x00015A04 File Offset: 0x00013C04
	public static void OnLoadingSceneEnd()
	{
	}

	// Token: 0x0600037B RID: 891 RVA: 0x00015A06 File Offset: 0x00013C06
	private void Awake()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		SceneMaster.instance = this;
		Level.Ev onSceneAwake = Level.onSceneAwake;
		if (onSceneAwake == null)
		{
			return;
		}
		onSceneAwake();
	}

	// Token: 0x0600037C RID: 892 RVA: 0x00015A25 File Offset: 0x00013C25
	private void Start()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		Level.Ev onSceneStart = Level.onSceneStart;
		if (onSceneStart != null)
		{
			onSceneStart();
		}
		if (this.isLoadingScene)
		{
			Level.Ev onLoadingSceneStart = Level.onLoadingSceneStart;
			if (onLoadingSceneStart == null)
			{
				return;
			}
			onLoadingSceneStart();
		}
	}

	// Token: 0x0600037D RID: 893 RVA: 0x00015A58 File Offset: 0x00013C58
	private void OnDestroy()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		Level.Ev onSceneEnd = Level.onSceneEnd;
		if (onSceneEnd != null)
		{
			onSceneEnd();
		}
		if (this.isLoadingScene)
		{
			Level.Ev onLoadingSceneEnd = Level.onLoadingSceneEnd;
			if (onLoadingSceneEnd != null)
			{
				onLoadingSceneEnd();
			}
		}
		if (SceneMaster.instance == this)
		{
			SceneMaster.instance = null;
		}
	}

	public static SceneMaster instance;

	public bool isLoadingScene;

	public bool canSplitScreen;
}
