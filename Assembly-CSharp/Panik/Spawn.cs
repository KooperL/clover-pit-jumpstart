using System;
using UnityEngine;

namespace Panik
{
	public static class Spawn
	{
		// Token: 0x06000B2E RID: 2862 RVA: 0x0004B059 File Offset: 0x00049259
		public static GameObject Instance(string prefabName, Vector3 position, Transform parent = null)
		{
			return global::UnityEngine.Object.Instantiate<GameObject>(AssetMaster.GetPrefab(prefabName), position, Quaternion.identity, parent);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x0004B06D File Offset: 0x0004926D
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

		// Token: 0x06000B30 RID: 2864 RVA: 0x0004B0A8 File Offset: 0x000492A8
		public static Rigidbody2D InstanceMoving2D(string prefabName, Vector3 position, Vector2 velocity, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.Instance(prefabName, position, parent);
			Spawn.rb2DAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody2D>();
			Spawn.rb2DAppoggio.linearVelocity = velocity;
			return Spawn.rb2DAppoggio;
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x0004B0D6 File Offset: 0x000492D6
		public static Rigidbody2D InstanceMoving2D(string prefabName, Vector3 position, float direction, float speed, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.Instance(prefabName, position, parent);
			Spawn.rb2DAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody2D>();
			Spawn.rb2DAppoggio.linearVelocity = Util.AngleToAxis2D(direction, speed);
			return Spawn.rb2DAppoggio;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x0004B10B File Offset: 0x0004930B
		public static Rigidbody InstanceMoving3D(string prefabName, Vector3 position, Vector3 velocity, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.Instance(prefabName, position, parent);
			Spawn.rbAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody>();
			Spawn.rbAppoggio.linearVelocity = velocity;
			return Spawn.rbAppoggio;
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x0004B139 File Offset: 0x00049339
		public static Rigidbody InstanceMoving3D(string prefabName, Vector3 position, float yEulerDirection, float zEulerDirection, float speed, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.Instance(prefabName, position, parent);
			Spawn.rbAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody>();
			Spawn.rbAppoggio.linearVelocity = Util.AngleToAxis3D(yEulerDirection, zEulerDirection) * speed;
			return Spawn.rbAppoggio;
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x0004B175 File Offset: 0x00049375
		public static Rigidbody2D FromPoolMoving2D(string prefabName, Vector3 position, Vector2 velocity, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.FromPool(prefabName, position, parent);
			Spawn.rb2DAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody2D>();
			Spawn.rb2DAppoggio.linearVelocity = velocity;
			return Spawn.rb2DAppoggio;
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x0004B1A3 File Offset: 0x000493A3
		public static Rigidbody2D FromPoolMoving2D(string prefabName, Vector3 position, float direction, float speed, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.FromPool(prefabName, position, parent);
			Spawn.rb2DAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody2D>();
			Spawn.rb2DAppoggio.linearVelocity = Util.AngleToAxis2D(direction, speed);
			return Spawn.rb2DAppoggio;
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x0004B1D8 File Offset: 0x000493D8
		public static Rigidbody FromPoolMoving3D(string prefabName, Vector3 position, Vector3 velocity, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.FromPool(prefabName, position, parent);
			Spawn.rbAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody>();
			Spawn.rbAppoggio.linearVelocity = velocity;
			return Spawn.rbAppoggio;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0004B206 File Offset: 0x00049406
		public static Rigidbody FromPoolMoving3D(string prefabName, Vector3 position, float yEulerDirection, float zEulerDirection, float speed, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.FromPool(prefabName, position, parent);
			Spawn.rbAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody>();
			Spawn.rbAppoggio.linearVelocity = Util.AngleToAxis3D(yEulerDirection, zEulerDirection) * speed;
			return Spawn.rbAppoggio;
		}

		private static GameObject gObjAppoggio;

		private static Rigidbody rbAppoggio;

		private static Rigidbody2D rb2DAppoggio;
	}
}
