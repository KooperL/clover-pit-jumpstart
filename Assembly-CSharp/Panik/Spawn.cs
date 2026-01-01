using System;
using UnityEngine;

namespace Panik
{
	// Token: 0x02000116 RID: 278
	public static class Spawn
	{
		// Token: 0x06000D27 RID: 3367 RVA: 0x000109EA File Offset: 0x0000EBEA
		public static GameObject Instance(string prefabName, Vector3 position, Transform parent = null)
		{
			return global::UnityEngine.Object.Instantiate<GameObject>(AssetMaster.GetPrefab(prefabName), position, Quaternion.identity, parent);
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x000109FE File Offset: 0x0000EBFE
		public static GameObject FromPool(string prefabName, Vector3 position, Transform parent = null)
		{
			Spawn.gObjAppoggio = Pool.Get(prefabName);
			Spawn.gObjAppoggio.transform.position = position;
			if (parent != null)
			{
				Spawn.gObjAppoggio.transform.SetParent(parent);
			}
			return Spawn.gObjAppoggio;
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x00010A39 File Offset: 0x0000EC39
		public static Rigidbody2D InstanceMoving2D(string prefabName, Vector3 position, Vector2 velocity, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.Instance(prefabName, position, parent);
			Spawn.rb2DAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody2D>();
			Spawn.rb2DAppoggio.linearVelocity = velocity;
			return Spawn.rb2DAppoggio;
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x00010A67 File Offset: 0x0000EC67
		public static Rigidbody2D InstanceMoving2D(string prefabName, Vector3 position, float direction, float speed, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.Instance(prefabName, position, parent);
			Spawn.rb2DAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody2D>();
			Spawn.rb2DAppoggio.linearVelocity = Util.AngleToAxis2D(direction, speed);
			return Spawn.rb2DAppoggio;
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00010A9C File Offset: 0x0000EC9C
		public static Rigidbody InstanceMoving3D(string prefabName, Vector3 position, Vector3 velocity, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.Instance(prefabName, position, parent);
			Spawn.rbAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody>();
			Spawn.rbAppoggio.linearVelocity = velocity;
			return Spawn.rbAppoggio;
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x00010ACA File Offset: 0x0000ECCA
		public static Rigidbody InstanceMoving3D(string prefabName, Vector3 position, float yEulerDirection, float zEulerDirection, float speed, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.Instance(prefabName, position, parent);
			Spawn.rbAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody>();
			Spawn.rbAppoggio.linearVelocity = Util.AngleToAxis3D(yEulerDirection, zEulerDirection) * speed;
			return Spawn.rbAppoggio;
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00010B06 File Offset: 0x0000ED06
		public static Rigidbody2D FromPoolMoving2D(string prefabName, Vector3 position, Vector2 velocity, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.FromPool(prefabName, position, parent);
			Spawn.rb2DAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody2D>();
			Spawn.rb2DAppoggio.linearVelocity = velocity;
			return Spawn.rb2DAppoggio;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x00010B34 File Offset: 0x0000ED34
		public static Rigidbody2D FromPoolMoving2D(string prefabName, Vector3 position, float direction, float speed, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.FromPool(prefabName, position, parent);
			Spawn.rb2DAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody2D>();
			Spawn.rb2DAppoggio.linearVelocity = Util.AngleToAxis2D(direction, speed);
			return Spawn.rb2DAppoggio;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x00010B69 File Offset: 0x0000ED69
		public static Rigidbody FromPoolMoving3D(string prefabName, Vector3 position, Vector3 velocity, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.FromPool(prefabName, position, parent);
			Spawn.rbAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody>();
			Spawn.rbAppoggio.linearVelocity = velocity;
			return Spawn.rbAppoggio;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x00010B97 File Offset: 0x0000ED97
		public static Rigidbody FromPoolMoving3D(string prefabName, Vector3 position, float yEulerDirection, float zEulerDirection, float speed, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.FromPool(prefabName, position, parent);
			Spawn.rbAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody>();
			Spawn.rbAppoggio.linearVelocity = Util.AngleToAxis3D(yEulerDirection, zEulerDirection) * speed;
			return Spawn.rbAppoggio;
		}

		// Token: 0x04000DCB RID: 3531
		private static GameObject gObjAppoggio;

		// Token: 0x04000DCC RID: 3532
		private static Rigidbody rbAppoggio;

		// Token: 0x04000DCD RID: 3533
		private static Rigidbody2D rb2DAppoggio;
	}
}
