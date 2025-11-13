using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	public class Pool : MonoBehaviour
	{
		// Token: 0x06000B18 RID: 2840 RVA: 0x0004AB50 File Offset: 0x00048D50
		public static GameObject GetEx(string prefabName, bool automaticallyEnable)
		{
			Pool.gameObjAppoggio = null;
			Pool.poolCapsAppoggio = null;
			if (!Pool.instance.poolDict.ContainsKey(prefabName))
			{
				Pool.instance.poolDict.Add(prefabName, new Pool.PoolCapsule(prefabName));
			}
			Pool.poolCapsAppoggio = Pool.instance.poolDict[prefabName];
			if (Pool.poolCapsAppoggio.availableGameObjects.Count > 0)
			{
				Pool.gameObjAppoggio = Pool.poolCapsAppoggio.availableGameObjects[Pool.poolCapsAppoggio.availableGameObjects.Count - 1];
				Pool.poolCapsAppoggio.availableGameObjects.RemoveAt(Pool.poolCapsAppoggio.availableGameObjects.Count - 1);
			}
			else
			{
				Pool.gameObjAppoggio = global::UnityEngine.Object.Instantiate<GameObject>(AssetMaster.GetPrefab(prefabName));
				Pool.gameObjAppoggio.name = prefabName;
			}
			if (!Pool.gameObjAppoggio.activeSelf && automaticallyEnable)
			{
				Pool.gameObjAppoggio.SetActive(true);
			}
			return Pool.gameObjAppoggio;
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0004AC3B File Offset: 0x00048E3B
		public static GameObject Get(string prefabName)
		{
			return Pool.GetEx(prefabName, true);
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x0004AC44 File Offset: 0x00048E44
		public static void Destroy(GameObject gameObject, string prefabName = null)
		{
			if (gameObject == null)
			{
				Debug.LogError("You are trying to Pool.destroy(..) a null game object. The prefab name is: " + (string.IsNullOrEmpty(prefabName) ? "NO NAME" : prefabName));
				return;
			}
			if (!gameObject.activeSelf)
			{
				Debug.LogWarning("POOL: Dude, the game object is already disabled! GameObject of name: " + gameObject.name);
				return;
			}
			if (prefabName == null)
			{
				prefabName = gameObject.name;
			}
			if (!Pool.instance.poolDict.ContainsKey(prefabName))
			{
				Pool.instance.poolDict.Add(prefabName, new Pool.PoolCapsule(prefabName));
			}
			Pool.poolCapsAppoggio = Pool.instance.poolDict[prefabName];
			Pool.poolCapsAppoggio.availableGameObjects.Add(gameObject);
			gameObject.SetActive(false);
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x0004ACF8 File Offset: 0x00048EF8
		public static void Prepare(string prefabName, int numberOfDesiredInstances)
		{
			for (int i = 0; i < numberOfDesiredInstances; i++)
			{
				Pool.instance.prepareList.Add(prefabName);
			}
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x0004AD24 File Offset: 0x00048F24
		private void _PrepareDuringUpdate(string prefabName)
		{
			Pool.gameObjAppoggio = null;
			Pool.poolCapsAppoggio = null;
			if (!this.poolDict.ContainsKey(prefabName))
			{
				this.poolDict.Add(prefabName, new Pool.PoolCapsule(prefabName));
			}
			Pool.poolCapsAppoggio = this.poolDict[prefabName];
			Pool.gameObjAppoggio = global::UnityEngine.Object.Instantiate<GameObject>(AssetMaster.GetPrefab(prefabName));
			Pool.gameObjAppoggio.SetActive(false);
			Pool.gameObjAppoggio.name = prefabName;
			Pool.poolCapsAppoggio.availableGameObjects.Add(Pool.gameObjAppoggio);
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x0004ADA8 File Offset: 0x00048FA8
		private void Awake()
		{
			Pool.instance = this;
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0004ADB0 File Offset: 0x00048FB0
		private void OnDestroy()
		{
			if (Pool.instance == this)
			{
				Pool.instance = null;
			}
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0004ADC8 File Offset: 0x00048FC8
		private void Update()
		{
			if (this.prepareList.Count > 0)
			{
				this._PrepareDuringUpdate(this.prepareList[0]);
				string text = this.prepareList[0];
				this.prepareList[0] = this.prepareList[this.prepareList.Count - 1];
				this.prepareList[this.prepareList.Count - 1] = text;
				this.prepareList.RemoveAt(this.prepareList.Count - 1);
			}
		}

		public static Pool instance;

		private static GameObject gameObjAppoggio;

		private static Pool.PoolCapsule poolCapsAppoggio;

		[NonSerialized]
		public Dictionary<string, Pool.PoolCapsule> poolDict = new Dictionary<string, Pool.PoolCapsule>();

		public const int PREPARE_ARRAY_SIZE = 500;

		[NonSerialized]
		public List<string> prepareList = new List<string>(500);

		public class PoolCapsule
		{
			// Token: 0x060012A7 RID: 4775 RVA: 0x0007714C File Offset: 0x0007534C
			public PoolCapsule(string prefabName)
			{
				this.prefabName = prefabName;
			}

			public string prefabName;

			public List<GameObject> availableGameObjects = new List<GameObject>();
		}
	}
}
