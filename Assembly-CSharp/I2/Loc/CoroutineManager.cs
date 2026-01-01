using System;
using System.Collections;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001DA RID: 474
	public class CoroutineManager : MonoBehaviour
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060013EC RID: 5100 RVA: 0x00081A7C File Offset: 0x0007FC7C
		private static CoroutineManager pInstance
		{
			get
			{
				if (CoroutineManager.mInstance == null)
				{
					GameObject gameObject = new GameObject("_Coroutiner");
					gameObject.hideFlags = HideFlags.HideAndDontSave;
					CoroutineManager.mInstance = gameObject.AddComponent<CoroutineManager>();
					if (Application.isPlaying)
					{
						global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
				}
				return CoroutineManager.mInstance;
			}
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x000156AD File Offset: 0x000138AD
		private void Awake()
		{
			if (Application.isPlaying)
			{
				global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x060013EE RID: 5102 RVA: 0x000156C1 File Offset: 0x000138C1
		public static Coroutine Start(IEnumerator coroutine)
		{
			return CoroutineManager.pInstance.StartCoroutine(coroutine);
		}

		// Token: 0x040013BD RID: 5053
		private static CoroutineManager mInstance;
	}
}
