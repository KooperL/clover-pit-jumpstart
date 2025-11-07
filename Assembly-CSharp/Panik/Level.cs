using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Panik
{
	public static class Level
	{
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00052E28 File Offset: 0x00051028
		public static int CurrentScene
		{
			get
			{
				return SceneManager.GetActiveScene().buildIndex;
			}
		}

		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00052E42 File Offset: 0x00051042
		public static Level.SceneIndex CurrentSceneIndex
		{
			get
			{
				return (Level.SceneIndex)Level.CurrentScene;
			}
		}

		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x00052E49 File Offset: 0x00051049
		public static int PreviousScene
		{
			get
			{
				return Level._prevScene;
			}
		}

		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x00052E50 File Offset: 0x00051050
		public static Level.SceneIndex PreviousSceneIndex
		{
			get
			{
				return (Level.SceneIndex)Level._prevScene;
			}
		}

		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x00052E57 File Offset: 0x00051057
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

		// Token: 0x06000CBB RID: 3259 RVA: 0x00052E70 File Offset: 0x00051070
		public static void GoToNext(bool asyncLoad = true)
		{
			Level.GoTo(Level.CurrentScene + 1, asyncLoad);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x00052E7F File Offset: 0x0005107F
		public static void GoPrevious(bool asyncLoad = true)
		{
			Level.GoTo(Level.CurrentScene - 1, asyncLoad);
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00052E8E File Offset: 0x0005108E
		public static void Restart(bool asyncLoad = true)
		{
			Level.GoTo(Level.CurrentScene, asyncLoad);
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00052E9B File Offset: 0x0005109B
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

		// Token: 0x06000CBF RID: 3263 RVA: 0x00052ED8 File Offset: 0x000510D8
		public static void GoTo(Level.SceneIndex sceneIndexToLoad, bool asyncLoad = true)
		{
			Level.GoTo((int)sceneIndexToLoad, asyncLoad);
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00052EE1 File Offset: 0x000510E1
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

		// Token: 0x06000CC1 RID: 3265 RVA: 0x00052EF0 File Offset: 0x000510F0
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

		// (Invoke) Token: 0x060013E1 RID: 5089
		public delegate void Ev();
	}
}
