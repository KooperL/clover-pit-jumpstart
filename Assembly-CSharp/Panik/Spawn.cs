using System;
using UnityEngine;

namespace Panik
{
	public static class Spawn
	{
		// Token: 0x06000B43 RID: 2883 RVA: 0x0004B7B9 File Offset: 0x000499B9
		public static GameObject Instance(string prefabName, Vector3 position, Transform parent = null)
		{
			return global::UnityEngine.Object.Instantiate<GameObject>(AssetMaster.GetPrefab(prefabName), position, Quaternion.identity, parent);
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x0004B7CD File Offset: 0x000499CD
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

		// Token: 0x06000B45 RID: 2885 RVA: 0x0004B808 File Offset: 0x00049A08
		public static Rigidbody2D InstanceMoving2D(string prefabName, Vector3 position, Vector2 velocity, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.Instance(prefabName, position, parent);
			Spawn.rb2DAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody2D>();
			Spawn.rb2DAppoggio.linearVelocity = velocity;
			return Spawn.rb2DAppoggio;
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0004B836 File Offset: 0x00049A36
		public static Rigidbody2D InstanceMoving2D(string prefabName, Vector3 position, float direction, float speed, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.Instance(prefabName, position, parent);
			Spawn.rb2DAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody2D>();
			Spawn.rb2DAppoggio.linearVelocity = Util.AngleToAxis2D(direction, speed);
			return Spawn.rb2DAppoggio;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x0004B86B File Offset: 0x00049A6B
		public static Rigidbody InstanceMoving3D(string prefabName, Vector3 position, Vector3 velocity, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.Instance(prefabName, position, parent);
			Spawn.rbAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody>();
			Spawn.rbAppoggio.linearVelocity = velocity;
			return Spawn.rbAppoggio;
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x0004B899 File Offset: 0x00049A99
		public static Rigidbody InstanceMoving3D(string prefabName, Vector3 position, float yEulerDirection, float zEulerDirection, float speed, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.Instance(prefabName, position, parent);
			Spawn.rbAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody>();
			Spawn.rbAppoggio.linearVelocity = Util.AngleToAxis3D(yEulerDirection, zEulerDirection) * speed;
			return Spawn.rbAppoggio;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0004B8D5 File Offset: 0x00049AD5
		public static Rigidbody2D FromPoolMoving2D(string prefabName, Vector3 position, Vector2 velocity, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.FromPool(prefabName, position, parent);
			Spawn.rb2DAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody2D>();
			Spawn.rb2DAppoggio.linearVelocity = velocity;
			return Spawn.rb2DAppoggio;
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0004B903 File Offset: 0x00049B03
		public static Rigidbody2D FromPoolMoving2D(string prefabName, Vector3 position, float direction, float speed, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.FromPool(prefabName, position, parent);
			Spawn.rb2DAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody2D>();
			Spawn.rb2DAppoggio.linearVelocity = Util.AngleToAxis2D(direction, speed);
			return Spawn.rb2DAppoggio;
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0004B938 File Offset: 0x00049B38
		public static Rigidbody FromPoolMoving3D(string prefabName, Vector3 position, Vector3 velocity, Transform parent = null)
		{
			Spawn.gObjAppoggio = Spawn.FromPool(prefabName, position, parent);
			Spawn.rbAppoggio = Spawn.gObjAppoggio.GetComponent<Rigidbody>();
			Spawn.rbAppoggio.linearVelocity = velocity;
			return Spawn.rbAppoggio;
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0004B966 File Offset: 0x00049B66
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
