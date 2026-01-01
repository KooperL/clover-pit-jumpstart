using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Panik
{
	// Token: 0x02000145 RID: 325
	public static class Level
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600100A RID: 4106 RVA: 0x0006F6B0 File Offset: 0x0006D8B0
		public static int CurrentScene
		{
			get
			{
				return SceneManager.GetActiveScene().buildIndex;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600100B RID: 4107 RVA: 0x00013446 File Offset: 0x00011646
		public static Level.SceneIndex CurrentSceneIndex
		{
			get
			{
				return (Level.SceneIndex)Level.CurrentScene;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600100C RID: 4108 RVA: 0x0001344D File Offset: 0x0001164D
		public static int PreviousScene
		{
			get
			{
				return Level._prevScene;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600100D RID: 4109 RVA: 0x0001344D File Offset: 0x0001164D
		public static Level.SceneIndex PreviousSceneIndex
		{
			get
			{
				return (Level.SceneIndex)Level._prevScene;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x0600100E RID: 4110 RVA: 0x00013454 File Offset: 0x00011654
		public static float GetLoadingStatus
		{
			get
			{
				if (Level.loadingOperationReference == null)
				{
					return 0f;
				}
				return Level.loadingOperationReference.progress;
			}
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0001346D File Offset: 0x0001166D
		public static void GoToNext(bool asyncLoad = true)
		{
			Level.GoTo(Level.CurrentScene + 1, asyncLoad);
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0001347C File Offset: 0x0001167C
		public static void GoPrevious(bool asyncLoad = true)
		{
			Level.GoTo(Level.CurrentScene - 1, asyncLoad);
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0001348B File Offset: 0x0001168B
		public static void Restart(bool asyncLoad = true)
		{
			Level.GoTo(Level.CurrentScene, asyncLoad);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00013498 File Offset: 0x00011698
		public static void GoTo(int sceneIndexToLoad, bool asyncLoad = true)
		{
			Level._prevScene = Level.CurrentScene;
			if (!asyncLoad)
			{
				SceneManager.LoadScene(sceneIndexToLoad);
				return;
			}
			if (Level.levelLoadingCoroutine == null)
			{
				Level.levelLoadingCoroutine = Master.instance.StartCoroutine(Level.LoadSceneAsync(sceneIndexToLoad));
				return;
			}
			Debug.LogWarning("Cannot start the loading of a scene as it looks like an other one is in act!");
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x000134D5 File Offset: 0x000116D5
		public static void GoTo(Level.SceneIndex sceneIndexToLoad, bool asyncLoad = true)
		{
			Level.GoTo((int)sceneIndexToLoad, asyncLoad);
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x000134DE File Offset: 0x000116DE
		private static IEnumerator LoadSceneAsync(int sceneIndexToLoad)
		{
			SceneManager.LoadScene(Level.loadingSceneIndex);
			VirtualCursors.CursorDesiredVisibilitySet(0, false);
			yield return new WaitForSeconds(Level.loadingStartDelay);
			Level.loadingOperationReference = SceneManager.LoadSceneAsync(sceneIndexToLoad);
			Level.loadingOperationReference.allowSceneActivation = false;
			while (Level.loadingOperationReference.progress < 0.9f)
			{
				yield return null;
			}
			while (LoadingScreenNotifications.LoadingShouldWait())
			{
				yield return null;
			}
			while (LoadingScreenCallToAction.LoadingShouldWait())
			{
				yield return null;
			}
			yield return new WaitForSeconds(Level.loadingEndDelay);
			if (Level.loadingOperationReference.progress >= 0.9f && Level.autoTransitToLoadedScene)
			{
				Level.StartLoadedScene();
			}
			while (!Level.loadingOperationReference.isDone)
			{
				yield return null;
			}
			Level.levelLoadingCoroutine = null;
			yield break;
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x000134ED File Offset: 0x000116ED
		public static void StartLoadedScene()
		{
			if (Level.loadingOperationReference == null)
			{
				Debug.LogWarning("LEVEL SYS: cannot start loadied scene. There is no loading in action!");
				return;
			}
			if (Level.loadingOperationReference.progress < 0.9f)
			{
				Debug.LogWarning("LEVEL SYS: cannot start loaded scene yet... it's still loading!");
				return;
			}
			Level.loadingOperationReference.allowSceneActivation = true;
		}

		// Token: 0x040010AE RID: 4270
		public static int loadingSceneIndex = 0;

		// Token: 0x040010AF RID: 4271
		private static int _prevScene = 0;

		// Token: 0x040010B0 RID: 4272
		public static float loadingStartDelay = 0f;

		// Token: 0x040010B1 RID: 4273
		public static float loadingEndDelay = 0f;

		// Token: 0x040010B2 RID: 4274
		public static bool autoTransitToLoadedScene = true;

		// Token: 0x040010B3 RID: 4275
		private static AsyncOperation loadingOperationReference = null;

		// Token: 0x040010B4 RID: 4276
		public static Coroutine levelLoadingCoroutine = null;

		// Token: 0x040010B5 RID: 4277
		public static Level.Ev onSceneAwake;

		// Token: 0x040010B6 RID: 4278
		public static Level.Ev onSceneStart;

		// Token: 0x040010B7 RID: 4279
		public static Level.Ev onSceneEnd;

		// Token: 0x040010B8 RID: 4280
		public static Level.Ev onLoadingSceneStart;

		// Token: 0x040010B9 RID: 4281
		public static Level.Ev onLoadingSceneEnd;

		// Token: 0x02000146 RID: 326
		public enum SceneIndex
		{
			// Token: 0x040010BB RID: 4283
			Loading,
			// Token: 0x040010BC RID: 4284
			Intro,
			// Token: 0x040010BD RID: 4285
			Game
		}

		// Token: 0x02000147 RID: 327
		// (Invoke) Token: 0x06001018 RID: 4120
		public delegate void Ev();
	}
}
