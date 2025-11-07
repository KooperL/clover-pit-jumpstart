using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	public class Pool : MonoBehaviour
	{
		// Token: 0x06000B03 RID: 2819 RVA: 0x0004A3F0 File Offset: 0x000485F0
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
				Pool.gameObjAppoggio = Object.Instantiate<GameObject>(AssetMaster.GetPrefab(prefabName));
				Pool.gameObjAppoggio.name = prefabName;
			}
			if (!Pool.gameObjAppoggio.activeSelf && automaticallyEnable)
			{
				Pool.gameObjAppoggio.SetActive(true);
			}
			return Pool.gameObjAppoggio;
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x0004A4DB File Offset: 0x000486DB
		public static GameObject Get(string prefabName)
		{
			return Pool.GetEx(prefabName, true);
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x0004A4E4 File Offset: 0x000486E4
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

		// Token: 0x06000B06 RID: 2822 RVA: 0x0004A598 File Offset: 0x00048798
		public static void Prepare(string prefabName, int numberOfDesiredInstances)
		{
			for (int i = 0; i < numberOfDesiredInstances; i++)
			{
				Pool.instance.prepareList.Add(prefabName);
			}
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0004A5C4 File Offset: 0x000487C4
		private void _PrepareDuringUpdate(string prefabName)
		{
			Pool.gameObjAppoggio = null;
			Pool.poolCapsAppoggio = null;
			if (!this.poolDict.ContainsKey(prefabName))
			{
				this.poolDict.Add(prefabName, new Pool.PoolCapsule(prefabName));
			}
			Pool.poolCapsAppoggio = this.poolDict[prefabName];
			Pool.gameObjAppoggio = Object.Instantiate<GameObject>(AssetMaster.GetPrefab(prefabName));
			Pool.gameObjAppoggio.SetActive(false);
			Pool.gameObjAppoggio.name = prefabName;
			Pool.poolCapsAppoggio.availableGameObjects.Add(Pool.gameObjAppoggio);
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x0004A648 File Offset: 0x00048848
		private void Awake()
		{
			Pool.instance = this;
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0004A650 File Offset: 0x00048850
		private void OnDestroy()
		{
			if (Pool.instance == this)
			{
				Pool.instance = null;
			}
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x0004A668 File Offset: 0x00048868
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
			// Token: 0x06001290 RID: 4752 RVA: 0x000768B8 File Offset: 0x00074AB8
			public PoolCapsule(string prefabName)
			{
				this.prefabName = prefabName;
			}

			public string prefabName;

			public List<GameObject> availableGameObjects = new List<GameObject>();
		}
	}
}
