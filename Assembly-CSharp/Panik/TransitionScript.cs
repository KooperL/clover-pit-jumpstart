using System;
using UnityEngine;

namespace Panik
{
	public class TransitionScript : MonoBehaviour
	{
		// Token: 0x06000DDA RID: 3546 RVA: 0x00056778 File Offset: 0x00054978
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

		// Token: 0x06000DDB RID: 3547 RVA: 0x000567CC File Offset: 0x000549CC
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

		// Token: 0x06000DDC RID: 3548 RVA: 0x0005680A File Offset: 0x00054A0A
		private new void Finalize()
		{
			if (this.targetSceneIndex >= 0)
			{
				Level.GoTo(this.targetSceneIndex, !this.skipLoadingScreen);
				return;
			}
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x00056835 File Offset: 0x00054A35
		private void Awake()
		{
			TransitionScript.instance = this;
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0005683D File Offset: 0x00054A3D
		private void OnDestroy()
		{
			if (TransitionScript.instance == this)
			{
				TransitionScript.instance = null;
			}
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x00056852 File Offset: 0x00054A52
		private void Update()
		{
			this.Finalize();
		}

		public static TransitionScript instance;

		private int targetSceneIndex = -1;

		private bool skipLoadingScreen;
	}
}
