using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Panik
{
	public static class Level
	{
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x00053588 File Offset: 0x00051788
		public static int CurrentScene
		{
			get
			{
				return SceneManager.GetActiveScene().buildIndex;
			}
		}

		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x000535A2 File Offset: 0x000517A2
		public static Level.SceneIndex CurrentSceneIndex
		{
			get
			{
				return (Level.SceneIndex)Level.CurrentScene;
			}
		}

		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x000535A9 File Offset: 0x000517A9
		public static int PreviousScene
		{
			get
			{
				return Level._prevScene;
			}
		}

		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x000535B0 File Offset: 0x000517B0
		public static Level.SceneIndex PreviousSceneIndex
		{
			get
			{
				return (Level.SceneIndex)Level._prevScene;
			}
		}

		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x000535B7 File Offset: 0x000517B7
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

		// Token: 0x06000CD0 RID: 3280 RVA: 0x000535D0 File Offset: 0x000517D0
		public static void GoToNext(bool asyncLoad = true)
		{
			Level.GoTo(Level.CurrentScene + 1, asyncLoad);
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x000535DF File Offset: 0x000517DF
		public static void GoPrevious(bool asyncLoad = true)
		{
			Level.GoTo(Level.CurrentScene - 1, asyncLoad);
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x000535EE File Offset: 0x000517EE
		public static void Restart(bool asyncLoad = true)
		{
			Level.GoTo(Level.CurrentScene, asyncLoad);
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x000535FB File Offset: 0x000517FB
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

		// Token: 0x06000CD4 RID: 3284 RVA: 0x00053638 File Offset: 0x00051838
		public static void GoTo(Level.SceneIndex sceneIndexToLoad, bool asyncLoad = true)
		{
			Level.GoTo((int)sceneIndexToLoad, asyncLoad);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x00053641 File Offset: 0x00051841
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

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00053650 File Offset: 0x00051850
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

		public static int loadingSceneIndex = 0;

		private static int _prevScene = 0;

		public static float loadingStartDelay = 0f;

		public static float loadingEndDelay = 0f;

		public static bool autoTransitToLoadedScene = true;

		private static AsyncOperation loadingOperationReference = null;

		public static Coroutine levelLoadingCoroutine = null;

		public static Level.Ev onSceneAwake;

		public static Level.Ev onSceneStart;

		public static Level.Ev onSceneEnd;

		public static Level.Ev onLoadingSceneStart;

		public static Level.Ev onLoadingSceneEnd;

		public enum SceneIndex
		{
			Loading,
			Intro,
			Game
		}

		// (Invoke) Token: 0x06001400 RID: 5120
		public delegate void Ev();
	}
}
