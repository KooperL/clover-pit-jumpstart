using System;
using Panik;
using UnityEngine;

// Token: 0x02000038 RID: 56
public class SceneMaster : MonoBehaviour
{
	// Token: 0x060003D7 RID: 983 RVA: 0x00029254 File Offset: 0x00027454
	public static void Initialize()
	{
		Level.onSceneAwake = (Level.Ev)Delegate.Combine(Level.onSceneAwake, new Level.Ev(SceneMaster.OnSceneAwake));
		Level.onSceneStart = (Level.Ev)Delegate.Combine(Level.onSceneStart, new Level.Ev(SceneMaster.OnSceneStart));
		Level.onSceneEnd = (Level.Ev)Delegate.Combine(Level.onSceneEnd, new Level.Ev(SceneMaster.OnSceneEnd));
		Level.onLoadingSceneStart = (Level.Ev)Delegate.Combine(Level.onLoadingSceneStart, new Level.Ev(SceneMaster.OnLoadingSceneStart));
		Level.onLoadingSceneEnd = (Level.Ev)Delegate.Combine(Level.onLoadingSceneEnd, new Level.Ev(SceneMaster.OnLoadingSceneEnd));
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x00008C63 File Offset: 0x00006E63
	public static void OnSceneAwake()
	{
		if (Level.CurrentSceneIndex != Level.SceneIndex.Game)
		{
			VirtualCursors.CursorDesiredVisibilitySet(0, true);
		}
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x00029304 File Offset: 0x00027504
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

	// Token: 0x060003DA RID: 986 RVA: 0x0000774E File Offset: 0x0000594E
	public static void OnLoadingSceneStart()
	{
	}

	// Token: 0x060003DB RID: 987 RVA: 0x00008C74 File Offset: 0x00006E74
	public static void OnSceneEnd()
	{
		Tick.Paused = false;
		Tick.FreezeTimer = 0f;
	}

	// Token: 0x060003DC RID: 988 RVA: 0x0000774E File Offset: 0x0000594E
	public static void OnLoadingSceneEnd()
	{
	}

	// Token: 0x060003DD RID: 989 RVA: 0x00008C86 File Offset: 0x00006E86
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

	// Token: 0x060003DE RID: 990 RVA: 0x00008CA5 File Offset: 0x00006EA5
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

	// Token: 0x060003DF RID: 991 RVA: 0x0002935C File Offset: 0x0002755C
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

	// Token: 0x04000314 RID: 788
	public static SceneMaster instance;

	// Token: 0x04000315 RID: 789
	public bool isLoadingScene;

	// Token: 0x04000316 RID: 790
	public bool canSplitScreen;
}
