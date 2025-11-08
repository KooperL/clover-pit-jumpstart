using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	public static class Util
	{
		// Token: 0x06000D84 RID: 3460 RVA: 0x00055841 File Offset: 0x00053A41
		public static T Choose<T>(params T[] elements)
		{
			return elements[global::UnityEngine.Random.Range(0, elements.Length)];
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00055852 File Offset: 0x00053A52
		public static T Choose<T>(List<T> elements)
		{
			return elements[global::UnityEngine.Random.Range(0, elements.Count)];
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00055866 File Offset: 0x00053A66
		public static float AngleCos(float angleDegrees)
		{
			return Mathf.Cos(angleDegrees * 0.017453292f);
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00055874 File Offset: 0x00053A74
		public static float AngleSin(float angleDegrees)
		{
			return Mathf.Sin(angleDegrees * 0.017453292f);
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00055882 File Offset: 0x00053A82
		public static float AxisToAngle2D(Vector2 axis)
		{
			return Mathf.Atan2(axis.y, axis.x) * 57.29578f;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0005589B File Offset: 0x00053A9B
		public static float AxisToAngle2D(float x, float y)
		{
			return Mathf.Atan2(y, x) * 57.29578f;
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x000558AA File Offset: 0x00053AAA
		public static float AxisToAngle2D(Transform transformFrom, Transform transformTo)
		{
			return Util.AxisToAngle2D(transformTo.GetPos2D() - transformFrom.GetPos2D());
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x000558C2 File Offset: 0x00053AC2
		public static Vector2 AngleToAxis2D(float angle360, float distanceInUnits = 1f)
		{
			Util.vec2.x = Mathf.Cos(angle360 * 0.017453292f) * distanceInUnits;
			Util.vec2.y = Mathf.Sin(angle360 * 0.017453292f) * distanceInUnits;
			return Util.vec2;
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x000558FC File Offset: 0x00053AFC
		public static Vector3 AxisToAngle3D(Vector3 axis)
		{
			return new Vector3(0f, Mathf.Atan2(-axis.z, axis.x) * 57.29578f, Mathf.Atan2(axis.y, new Vector2(axis.x, -axis.z).magnitude) * 57.29578f);
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x00055956 File Offset: 0x00053B56
		public static Vector3 AngleToAxis3D(float yEulerAngle, float zEulerAngle)
		{
			return new Vector3(Util.AngleCos(yEulerAngle) * Mathf.Abs(Util.AngleCos(zEulerAngle)), Util.AngleSin(zEulerAngle), -Util.AngleSin(yEulerAngle) * Mathf.Abs(Util.AngleCos(zEulerAngle)));
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00055988 File Offset: 0x00053B88
		public static Vector3 AxisToFpsVec3(Vector2 inputAxis, float facingYAngle)
		{
			facingYAngle = Util.AxisToAngle2D(-inputAxis.x, inputAxis.y) - 180f + facingYAngle;
			return Util.AngleToAxis3D(facingYAngle, 0f) * inputAxis.magnitude;
		}

		private static Vector2 vec2;
	}
}
