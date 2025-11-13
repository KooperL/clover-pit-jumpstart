using System;
using System.Collections;
using UnityEngine;

namespace I2.Loc
{
	public class CoroutineManager : MonoBehaviour
	{
		// (get) Token: 0x06000FFE RID: 4094 RVA: 0x00063D20 File Offset: 0x00061F20
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

		// Token: 0x06000FFF RID: 4095 RVA: 0x00063D6A File Offset: 0x00061F6A
		private void Awake()
		{
			if (Application.isPlaying)
			{
				global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00063D7E File Offset: 0x00061F7E
		public static Coroutine Start(IEnumerator coroutine)
		{
			return CoroutineManager.pInstance.StartCoroutine(coroutine);
		}

		private static CoroutineManager mInstance;
	}
}
