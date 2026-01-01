using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	// Token: 0x02000177 RID: 375
	public static class Util
	{
		// Token: 0x06001128 RID: 4392 RVA: 0x00013FBA File Offset: 0x000121BA
		public static T Choose<T>(params T[] elements)
		{
			return elements[global::UnityEngine.Random.Range(0, elements.Length)];
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00013FCB File Offset: 0x000121CB
		public static T Choose<T>(List<T> elements)
		{
			return elements[global::UnityEngine.Random.Range(0, elements.Count)];
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00013FDF File Offset: 0x000121DF
		public static float AngleCos(float angleDegrees)
		{
			return Mathf.Cos(angleDegrees * 0.017453292f);
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00013FED File Offset: 0x000121ED
		public static float AngleSin(float angleDegrees)
		{
			return Mathf.Sin(angleDegrees * 0.017453292f);
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x00013FFB File Offset: 0x000121FB
		public static float AxisToAngle2D(Vector2 axis)
		{
			return Mathf.Atan2(axis.y, axis.x) * 57.29578f;
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x00014014 File Offset: 0x00012214
		public static float AxisToAngle2D(float x, float y)
		{
			return Mathf.Atan2(y, x) * 57.29578f;
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x00014023 File Offset: 0x00012223
		public static float AxisToAngle2D(Transform transformFrom, Transform transformTo)
		{
			return Util.AxisToAngle2D(transformTo.GetPos2D() - transformFrom.GetPos2D());
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x0001403B File Offset: 0x0001223B
		public static Vector2 AngleToAxis2D(float angle360, float distanceInUnits = 1f)
		{
			Util.vec2.x = Mathf.Cos(angle360 * 0.017453292f) * distanceInUnits;
			Util.vec2.y = Mathf.Sin(angle360 * 0.017453292f) * distanceInUnits;
			return Util.vec2;
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00073B44 File Offset: 0x00071D44
		public static Vector3 AxisToAngle3D(Vector3 axis)
		{
			return new Vector3(0f, Mathf.Atan2(-axis.z, axis.x) * 57.29578f, Mathf.Atan2(axis.y, new Vector2(axis.x, -axis.z).magnitude) * 57.29578f);
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00014072 File Offset: 0x00012272
		public static Vector3 AngleToAxis3D(float yEulerAngle, float zEulerAngle)
		{
			return new Vector3(Util.AngleCos(yEulerAngle) * Mathf.Abs(Util.AngleCos(zEulerAngle)), Util.AngleSin(zEulerAngle), -Util.AngleSin(yEulerAngle) * Mathf.Abs(Util.AngleCos(zEulerAngle)));
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x000140A4 File Offset: 0x000122A4
		public static Vector3 AxisToFpsVec3(Vector2 inputAxis, float facingYAngle)
		{
			facingYAngle = Util.AxisToAngle2D(-inputAxis.x, inputAxis.y) - 180f + facingYAngle;
			return Util.AngleToAxis3D(facingYAngle, 0f) * inputAxis.magnitude;
		}

		// Token: 0x0400122C RID: 4652
		private static Vector2 vec2;
	}
}
