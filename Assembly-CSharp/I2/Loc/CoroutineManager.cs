using System;
using System.Collections;
using UnityEngine;

namespace I2.Loc
{
	public class CoroutineManager : MonoBehaviour
	{
		// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x00063544 File Offset: 0x00061744
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

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0006358E File Offset: 0x0006178E
		private void Awake()
		{
			if (Application.isPlaying)
			{
				global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x000635A2 File Offset: 0x000617A2
		public static Coroutine Start(IEnumerator coroutine)
		{
			return CoroutineManager.pInstance.StartCoroutine(coroutine);
		}

		private static CoroutineManager mInstance;
	}
}
