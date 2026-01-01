using System;
using UnityEngine;

namespace Panik
{
	// Token: 0x02000180 RID: 384
	public class TransitionScript : MonoBehaviour
	{
		// Token: 0x06001176 RID: 4470 RVA: 0x00075494 File Offset: 0x00073694
		public static TransitionScript To(int sceneIndex, bool skipLoadingScreen = false)
		{
			if (TransitionScript.instance != null)
			{
				return TransitionScript.instance;
			}
			TransitionScript.instance = global::UnityEngine.Object.Instantiate<GameObject>(AssetMaster.GetPrefab("Transition")).GetComponent<TransitionScript>();
			TransitionScript.instance.targetSceneIndex = sceneIndex;
			TransitionScript.instance.skipLoadingScreen = skipLoadingScreen;
			return TransitionScript.instance;
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x000143D1 File Offset: 0x000125D1
		public static TransitionScript In()
		{
			if (TransitionScript.instance != null)
			{
				return TransitionScript.instance;
			}
			TransitionScript.instance = global::UnityEngine.Object.Instantiate<GameObject>(AssetMaster.GetPrefab("Transition")).GetComponent<TransitionScript>();
			TransitionScript.instance.targetSceneIndex = -1;
			return TransitionScript.instance;
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x0001440F File Offset: 0x0001260F
		private new void Finalize()
		{
			if (this.targetSceneIndex >= 0)
			{
				Level.GoTo(this.targetSceneIndex, !this.skipLoadingScreen);
				return;
			}
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x0001443A File Offset: 0x0001263A
		private void Awake()
		{
			TransitionScript.instance = this;
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00014442 File Offset: 0x00012642
		private void OnDestroy()
		{
			if (TransitionScript.instance == this)
			{
				TransitionScript.instance = null;
			}
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00014457 File Offset: 0x00012657
		private void Update()
		{
			this.Finalize();
		}

		// Token: 0x0400128C RID: 4748
		public static TransitionScript instance;

		// Token: 0x0400128D RID: 4749
		private int targetSceneIndex = -1;

		// Token: 0x0400128E RID: 4750
		private bool skipLoadingScreen;
	}
}
