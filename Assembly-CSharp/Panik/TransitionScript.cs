using System;
using UnityEngine;

namespace Panik
{
	public class TransitionScript : MonoBehaviour
	{
		// Token: 0x06000DC3 RID: 3523 RVA: 0x00055F9C File Offset: 0x0005419C
		public static TransitionScript To(int sceneIndex, bool skipLoadingScreen = false)
		{
			if (TransitionScript.instance != null)
			{
				return TransitionScript.instance;
			}
			TransitionScript.instance = Object.Instantiate<GameObject>(AssetMaster.GetPrefab("Transition")).GetComponent<TransitionScript>();
			TransitionScript.instance.targetSceneIndex = sceneIndex;
			TransitionScript.instance.skipLoadingScreen = skipLoadingScreen;
			return TransitionScript.instance;
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x00055FF0 File Offset: 0x000541F0
		public static TransitionScript In()
		{
			if (TransitionScript.instance != null)
			{
				return TransitionScript.instance;
			}
			TransitionScript.instance = Object.Instantiate<GameObject>(AssetMaster.GetPrefab("Transition")).GetComponent<TransitionScript>();
			TransitionScript.instance.targetSceneIndex = -1;
			return TransitionScript.instance;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x0005602E File Offset: 0x0005422E
		private void Finalize()
		{
			if (this.targetSceneIndex >= 0)
			{
				Level.GoTo(this.targetSceneIndex, !this.skipLoadingScreen);
				return;
			}
			Object.Destroy(base.gameObject);
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00056059 File Offset: 0x00054259
		private void Awake()
		{
			TransitionScript.instance = this;
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x00056061 File Offset: 0x00054261
		private void OnDestroy()
		{
			if (TransitionScript.instance == this)
			{
				TransitionScript.instance = null;
			}
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00056076 File Offset: 0x00054276
		private void Update()
		{
			this.Finalize();
		}

		public static TransitionScript instance;

		private int targetSceneIndex = -1;

		private bool skipLoadingScreen;
	}
}
