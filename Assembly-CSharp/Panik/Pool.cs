using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	// Token: 0x02000112 RID: 274
	public class Pool : MonoBehaviour
	{
		// Token: 0x06000CF8 RID: 3320 RVA: 0x00064B60 File Offset: 0x00062D60
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

		// Token: 0x06000CF9 RID: 3321 RVA: 0x00010821 File Offset: 0x0000EA21
		public static GameObject Get(string prefabName)
		{
			return Pool.GetEx(prefabName, true);
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x00064C4C File Offset: 0x00062E4C
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

		// Token: 0x06000CFB RID: 3323 RVA: 0x00064D00 File Offset: 0x00062F00
		public static void Prepare(string prefabName, int numberOfDesiredInstances)
		{
			for (int i = 0; i < numberOfDesiredInstances; i++)
			{
				Pool.instance.prepareList.Add(prefabName);
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x00064D2C File Offset: 0x00062F2C
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

		// Token: 0x06000CFD RID: 3325 RVA: 0x0001082A File Offset: 0x0000EA2A
		private void Awake()
		{
			Pool.instance = this;
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x00010832 File Offset: 0x0000EA32
		private void OnDestroy()
		{
			if (Pool.instance == this)
			{
				Pool.instance = null;
			}
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x00064DB0 File Offset: 0x00062FB0
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

		// Token: 0x04000DAC RID: 3500
		public static Pool instance;

		// Token: 0x04000DAD RID: 3501
		private static GameObject gameObjAppoggio;

		// Token: 0x04000DAE RID: 3502
		private static Pool.PoolCapsule poolCapsAppoggio;

		// Token: 0x04000DAF RID: 3503
		[NonSerialized]
		public Dictionary<string, Pool.PoolCapsule> poolDict = new Dictionary<string, Pool.PoolCapsule>();

		// Token: 0x04000DB0 RID: 3504
		public const int PREPARE_ARRAY_SIZE = 500;

		// Token: 0x04000DB1 RID: 3505
		[NonSerialized]
		public List<string> prepareList = new List<string>(500);

		// Token: 0x02000113 RID: 275
		public class PoolCapsule
		{
			// Token: 0x06000D01 RID: 3329 RVA: 0x0001086A File Offset: 0x0000EA6A
			public PoolCapsule(string prefabName)
			{
				this.prefabName = prefabName;
			}

			// Token: 0x04000DB2 RID: 3506
			public string prefabName;

			// Token: 0x04000DB3 RID: 3507
			public List<GameObject> availableGameObjects = new List<GameObject>();
		}
	}
}
