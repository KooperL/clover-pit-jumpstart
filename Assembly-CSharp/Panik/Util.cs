using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	public static class Util
	{
		// Token: 0x06000D9B RID: 3483 RVA: 0x0005601D File Offset: 0x0005421D
		public static T Choose<T>(params T[] elements)
		{
			return elements[global::UnityEngine.Random.Range(0, elements.Length)];
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x0005602E File Offset: 0x0005422E
		public static T Choose<T>(List<T> elements)
		{
			return elements[global::UnityEngine.Random.Range(0, elements.Count)];
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00056042 File Offset: 0x00054242
		public static float AngleCos(float angleDegrees)
		{
			return Mathf.Cos(angleDegrees * 0.017453292f);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00056050 File Offset: 0x00054250
		public static float AngleSin(float angleDegrees)
		{
			return Mathf.Sin(angleDegrees * 0.017453292f);
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0005605E File Offset: 0x0005425E
		public static float AxisToAngle2D(Vector2 axis)
		{
			return Mathf.Atan2(axis.y, axis.x) * 57.29578f;
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00056077 File Offset: 0x00054277
		public static float AxisToAngle2D(float x, float y)
		{
			return Mathf.Atan2(y, x) * 57.29578f;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00056086 File Offset: 0x00054286
		public static float AxisToAngle2D(Transform transformFrom, Transform transformTo)
		{
			return Util.AxisToAngle2D(transformTo.GetPos2D() - transformFrom.GetPos2D());
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0005609E File Offset: 0x0005429E
		public static Vector2 AngleToAxis2D(float angle360, float distanceInUnits = 1f)
		{
			Util.vec2.x = Mathf.Cos(angle360 * 0.017453292f) * distanceInUnits;
			Util.vec2.y = Mathf.Sin(angle360 * 0.017453292f) * distanceInUnits;
			return Util.vec2;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x000560D8 File Offset: 0x000542D8
		public static Vector3 AxisToAngle3D(Vector3 axis)
		{
			return new Vector3(0f, Mathf.Atan2(-axis.z, axis.x) * 57.29578f, Mathf.Atan2(axis.y, new Vector2(axis.x, -axis.z).magnitude) * 57.29578f);
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x00056132 File Offset: 0x00054332
		public static Vector3 AngleToAxis3D(float yEulerAngle, float zEulerAngle)
		{
			return new Vector3(Util.AngleCos(yEulerAngle) * Mathf.Abs(Util.AngleCos(zEulerAngle)), Util.AngleSin(zEulerAngle), -Util.AngleSin(yEulerAngle) * Mathf.Abs(Util.AngleCos(zEulerAngle)));
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00056164 File Offset: 0x00054364
		public static Vector3 AxisToFpsVec3(Vector2 inputAxis, float facingYAngle)
		{
			facingYAngle = Util.AxisToAngle2D(-inputAxis.x, inputAxis.y) - 180f + facingYAngle;
			return Util.AngleToAxis3D(facingYAngle, 0f) * inputAxis.magnitude;
		}

		private static Vector2 vec2;
	}
}
