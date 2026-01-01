using System;
using System.Numerics;
using Panik;
using UnityEngine;

// Token: 0x0200000F RID: 15
public static class ClassExtensions
{
	// Token: 0x0600006B RID: 107 RVA: 0x0000796A File Offset: 0x00005B6A
	public static float GetX(this Transform t)
	{
		return t.position.x;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00007977 File Offset: 0x00005B77
	public static float GetY(this Transform t)
	{
		return t.position.y;
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00007984 File Offset: 0x00005B84
	public static float GetZ(this Transform t)
	{
		return t.position.z;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00019E8C File Offset: 0x0001808C
	public static float SetX(this Transform t, float newX)
	{
		ClassExtensions.vec3_0.x = newX;
		ClassExtensions.vec3_0.y = t.position.y;
		ClassExtensions.vec3_0.z = t.position.z;
		t.position = ClassExtensions.vec3_0;
		return t.position.x;
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00019EE4 File Offset: 0x000180E4
	public static float SetY(this Transform t, float newY)
	{
		ClassExtensions.vec3_0.x = t.position.x;
		ClassExtensions.vec3_0.y = newY;
		ClassExtensions.vec3_0.z = t.position.z;
		t.position = ClassExtensions.vec3_0;
		return t.position.y;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00019F3C File Offset: 0x0001813C
	public static float SetZ(this Transform t, float newZ)
	{
		ClassExtensions.vec3_0.x = t.position.x;
		ClassExtensions.vec3_0.y = t.position.y;
		ClassExtensions.vec3_0.z = newZ;
		t.position = ClassExtensions.vec3_0;
		return t.position.z;
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00019F94 File Offset: 0x00018194
	public static float AddX(this Transform t, float addX)
	{
		ClassExtensions.vec3_0.x = addX;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = 0f;
		t.position += ClassExtensions.vec3_0;
		return t.position.x;
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00019FEC File Offset: 0x000181EC
	public static float AddY(this Transform t, float addY)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = addY;
		ClassExtensions.vec3_0.z = 0f;
		t.position += ClassExtensions.vec3_0;
		return t.position.y;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x0001A044 File Offset: 0x00018244
	public static float AddZ(this Transform t, float addZ)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = addZ;
		t.position += ClassExtensions.vec3_0;
		return t.position.z;
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00007991 File Offset: 0x00005B91
	public static float GetLocalX(this Transform t)
	{
		return t.localPosition.x;
	}

	// Token: 0x06000075 RID: 117 RVA: 0x0000799E File Offset: 0x00005B9E
	public static float GetLocalY(this Transform t)
	{
		return t.localPosition.y;
	}

	// Token: 0x06000076 RID: 118 RVA: 0x000079AB File Offset: 0x00005BAB
	public static float GetLocalZ(this Transform t)
	{
		return t.localPosition.z;
	}

	// Token: 0x06000077 RID: 119 RVA: 0x0001A09C File Offset: 0x0001829C
	public static float SetLocalX(this Transform t, float newX)
	{
		ClassExtensions.vec3_0.x = newX;
		ClassExtensions.vec3_0.y = t.localPosition.y;
		ClassExtensions.vec3_0.z = t.localPosition.z;
		t.localPosition = ClassExtensions.vec3_0;
		return t.localPosition.x;
	}

	// Token: 0x06000078 RID: 120 RVA: 0x0001A0F4 File Offset: 0x000182F4
	public static float SetLocalY(this Transform t, float newY)
	{
		ClassExtensions.vec3_0.x = t.localPosition.x;
		ClassExtensions.vec3_0.y = newY;
		ClassExtensions.vec3_0.z = t.localPosition.z;
		t.localPosition = ClassExtensions.vec3_0;
		return t.localPosition.y;
	}

	// Token: 0x06000079 RID: 121 RVA: 0x0001A14C File Offset: 0x0001834C
	public static float SetLocalZ(this Transform t, float newZ)
	{
		ClassExtensions.vec3_0.x = t.localPosition.x;
		ClassExtensions.vec3_0.y = t.localPosition.y;
		ClassExtensions.vec3_0.z = newZ;
		t.localPosition = ClassExtensions.vec3_0;
		return t.localPosition.z;
	}

	// Token: 0x0600007A RID: 122 RVA: 0x0001A1A4 File Offset: 0x000183A4
	public static float AddLocalX(this Transform t, float addX)
	{
		ClassExtensions.vec3_0.x = addX;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = 0f;
		t.localPosition += ClassExtensions.vec3_0;
		return t.localPosition.x;
	}

	// Token: 0x0600007B RID: 123 RVA: 0x0001A1FC File Offset: 0x000183FC
	public static float AddLocalY(this Transform t, float addY)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = addY;
		ClassExtensions.vec3_0.z = 0f;
		t.localPosition += ClassExtensions.vec3_0;
		return t.localPosition.y;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x0001A254 File Offset: 0x00018454
	public static float AddLocalZ(this Transform t, float addZ)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = addZ;
		t.localPosition += ClassExtensions.vec3_0;
		return t.localPosition.z;
	}

	// Token: 0x0600007D RID: 125 RVA: 0x000079B8 File Offset: 0x00005BB8
	public static float GetXAngle(this Transform t)
	{
		return t.eulerAngles.x;
	}

	// Token: 0x0600007E RID: 126 RVA: 0x000079C5 File Offset: 0x00005BC5
	public static float GetYAngle(this Transform t)
	{
		return t.eulerAngles.y;
	}

	// Token: 0x0600007F RID: 127 RVA: 0x000079D2 File Offset: 0x00005BD2
	public static float GetZAngle(this Transform t)
	{
		return t.eulerAngles.z;
	}

	// Token: 0x06000080 RID: 128 RVA: 0x0001A2AC File Offset: 0x000184AC
	public static float SetXAngle(this Transform t, float newX)
	{
		ClassExtensions.vec3_0.x = newX;
		ClassExtensions.vec3_0.y = t.eulerAngles.y;
		ClassExtensions.vec3_0.z = t.eulerAngles.z;
		t.eulerAngles = ClassExtensions.vec3_0;
		return t.eulerAngles.x;
	}

	// Token: 0x06000081 RID: 129 RVA: 0x0001A304 File Offset: 0x00018504
	public static float SetYAngle(this Transform t, float newY)
	{
		ClassExtensions.vec3_0.x = t.eulerAngles.x;
		ClassExtensions.vec3_0.y = newY;
		ClassExtensions.vec3_0.z = t.eulerAngles.z;
		t.eulerAngles = ClassExtensions.vec3_0;
		return t.eulerAngles.y;
	}

	// Token: 0x06000082 RID: 130 RVA: 0x0001A35C File Offset: 0x0001855C
	public static float SetZAngle(this Transform t, float newZ)
	{
		ClassExtensions.vec3_0.x = t.eulerAngles.x;
		ClassExtensions.vec3_0.y = t.eulerAngles.y;
		ClassExtensions.vec3_0.z = newZ;
		t.eulerAngles = ClassExtensions.vec3_0;
		return t.eulerAngles.z;
	}

	// Token: 0x06000083 RID: 131 RVA: 0x0001A3B4 File Offset: 0x000185B4
	public static float AddXAngle(this Transform t, float addX)
	{
		ClassExtensions.vec3_0.x = addX;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = 0f;
		t.eulerAngles += ClassExtensions.vec3_0;
		return t.eulerAngles.x;
	}

	// Token: 0x06000084 RID: 132 RVA: 0x0001A40C File Offset: 0x0001860C
	public static float AddYAngle(this Transform t, float addY)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = addY;
		ClassExtensions.vec3_0.z = 0f;
		t.eulerAngles += ClassExtensions.vec3_0;
		return t.eulerAngles.y;
	}

	// Token: 0x06000085 RID: 133 RVA: 0x0001A464 File Offset: 0x00018664
	public static float AddZAngle(this Transform t, float addZ)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = addZ;
		t.eulerAngles += ClassExtensions.vec3_0;
		return t.eulerAngles.z;
	}

	// Token: 0x06000086 RID: 134 RVA: 0x000079DF File Offset: 0x00005BDF
	public static float GetLocalXAngle(this Transform t)
	{
		return t.localEulerAngles.x;
	}

	// Token: 0x06000087 RID: 135 RVA: 0x000079EC File Offset: 0x00005BEC
	public static float GetLocalYAngle(this Transform t)
	{
		return t.localEulerAngles.y;
	}

	// Token: 0x06000088 RID: 136 RVA: 0x000079F9 File Offset: 0x00005BF9
	public static float GetLocalZAngle(this Transform t)
	{
		return t.localEulerAngles.z;
	}

	// Token: 0x06000089 RID: 137 RVA: 0x0001A4BC File Offset: 0x000186BC
	public static float SetLocalXAngle(this Transform t, float newX)
	{
		ClassExtensions.vec3_0.x = newX;
		ClassExtensions.vec3_0.y = t.localEulerAngles.y;
		ClassExtensions.vec3_0.z = t.localEulerAngles.z;
		t.localEulerAngles = ClassExtensions.vec3_0;
		return t.localEulerAngles.x;
	}

	// Token: 0x0600008A RID: 138 RVA: 0x0001A514 File Offset: 0x00018714
	public static float SetLocalYAngle(this Transform t, float newY)
	{
		ClassExtensions.vec3_0.x = t.localEulerAngles.x;
		ClassExtensions.vec3_0.y = newY;
		ClassExtensions.vec3_0.z = t.localEulerAngles.z;
		t.localEulerAngles = ClassExtensions.vec3_0;
		return t.localEulerAngles.y;
	}

	// Token: 0x0600008B RID: 139 RVA: 0x0001A56C File Offset: 0x0001876C
	public static float SetLocalZAngle(this Transform t, float newZ)
	{
		ClassExtensions.vec3_0.x = t.localEulerAngles.x;
		ClassExtensions.vec3_0.y = t.localEulerAngles.y;
		ClassExtensions.vec3_0.z = newZ;
		t.localEulerAngles = ClassExtensions.vec3_0;
		return t.localEulerAngles.z;
	}

	// Token: 0x0600008C RID: 140 RVA: 0x0001A5C4 File Offset: 0x000187C4
	public static float AddLocalXAngle(this Transform t, float addX)
	{
		ClassExtensions.vec3_0.x = addX;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = 0f;
		t.localEulerAngles += ClassExtensions.vec3_0;
		return t.localEulerAngles.x;
	}

	// Token: 0x0600008D RID: 141 RVA: 0x0001A61C File Offset: 0x0001881C
	public static float AddLocalYAngle(this Transform t, float addY)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = addY;
		ClassExtensions.vec3_0.z = 0f;
		t.localEulerAngles += ClassExtensions.vec3_0;
		return t.localEulerAngles.y;
	}

	// Token: 0x0600008E RID: 142 RVA: 0x0001A674 File Offset: 0x00018874
	public static float AddLocalZAngle(this Transform t, float addZ)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = addZ;
		t.localEulerAngles += ClassExtensions.vec3_0;
		return t.localEulerAngles.z;
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00007A06 File Offset: 0x00005C06
	public static float GetLocalXScale(this Transform t)
	{
		return t.localScale.x;
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00007A13 File Offset: 0x00005C13
	public static float GetLocalYScale(this Transform t)
	{
		return t.localScale.y;
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00007A20 File Offset: 0x00005C20
	public static float GetLocalZScale(this Transform t)
	{
		return t.localScale.z;
	}

	// Token: 0x06000092 RID: 146 RVA: 0x0001A6CC File Offset: 0x000188CC
	public static float SetLocalXScale(this Transform t, float newX)
	{
		ClassExtensions.vec3_0.x = newX;
		ClassExtensions.vec3_0.y = t.localScale.y;
		ClassExtensions.vec3_0.z = t.localScale.z;
		t.localScale = ClassExtensions.vec3_0;
		return t.localScale.x;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x0001A724 File Offset: 0x00018924
	public static float SetLocalYScale(this Transform t, float newY)
	{
		ClassExtensions.vec3_0.x = t.localScale.x;
		ClassExtensions.vec3_0.y = newY;
		ClassExtensions.vec3_0.z = t.localScale.z;
		t.localScale = ClassExtensions.vec3_0;
		return t.localScale.y;
	}

	// Token: 0x06000094 RID: 148 RVA: 0x0001A77C File Offset: 0x0001897C
	public static float SetLocalZScale(this Transform t, float newZ)
	{
		ClassExtensions.vec3_0.x = t.localScale.x;
		ClassExtensions.vec3_0.y = t.localScale.y;
		ClassExtensions.vec3_0.z = newZ;
		t.localScale = ClassExtensions.vec3_0;
		return t.localScale.z;
	}

	// Token: 0x06000095 RID: 149 RVA: 0x0001A7D4 File Offset: 0x000189D4
	public static float AddLocalXScale(this Transform t, float addX)
	{
		ClassExtensions.vec3_0.x = addX;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = 0f;
		t.localScale += ClassExtensions.vec3_0;
		return t.localScale.x;
	}

	// Token: 0x06000096 RID: 150 RVA: 0x0001A82C File Offset: 0x00018A2C
	public static float AddLocalYScale(this Transform t, float addY)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = addY;
		ClassExtensions.vec3_0.z = 0f;
		t.localScale += ClassExtensions.vec3_0;
		return t.localScale.y;
	}

	// Token: 0x06000097 RID: 151 RVA: 0x0001A884 File Offset: 0x00018A84
	public static float AddLocalZScale(this Transform t, float addZ)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = addZ;
		t.localScale += ClassExtensions.vec3_0;
		return t.localScale.z;
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00007A2D File Offset: 0x00005C2D
	public static global::UnityEngine.Vector2 GetPos2D(this Transform t)
	{
		ClassExtensions.vec2_0.x = t.position.x;
		ClassExtensions.vec2_0.y = t.position.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x06000099 RID: 153 RVA: 0x0001A8DC File Offset: 0x00018ADC
	public static global::UnityEngine.Vector2 SetPos2D(this Transform t, global::UnityEngine.Vector2 newPosition)
	{
		ClassExtensions.vec3_0.x = newPosition.x;
		ClassExtensions.vec3_0.y = newPosition.y;
		ClassExtensions.vec3_0.z = t.position.z;
		t.position = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.position.x;
		ClassExtensions.vec2_0.y = t.position.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x0001A958 File Offset: 0x00018B58
	public static global::UnityEngine.Vector2 SetPos2D(this Transform t, float x, float y)
	{
		ClassExtensions.vec3_0.x = x;
		ClassExtensions.vec3_0.y = y;
		ClassExtensions.vec3_0.z = t.position.z;
		t.position = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.position.x;
		ClassExtensions.vec2_0.y = t.position.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x0600009B RID: 155 RVA: 0x0001A9CC File Offset: 0x00018BCC
	public static global::UnityEngine.Vector2 AddPos2D(this Transform t, global::UnityEngine.Vector2 value)
	{
		ClassExtensions.vec3_0.x = t.position.x + value.x;
		ClassExtensions.vec3_0.y = t.position.y + value.y;
		ClassExtensions.vec3_0.z = t.position.z;
		t.position = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.position.x;
		ClassExtensions.vec2_0.y = t.position.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x0600009C RID: 156 RVA: 0x0001AA60 File Offset: 0x00018C60
	public static global::UnityEngine.Vector2 AddPos2D(this Transform t, float x, float y)
	{
		ClassExtensions.vec3_0.x = t.position.x + x;
		ClassExtensions.vec3_0.y = t.position.y + y;
		ClassExtensions.vec3_0.z = t.position.z;
		t.position = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.position.x;
		ClassExtensions.vec2_0.y = t.position.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00007A5E File Offset: 0x00005C5E
	public static global::UnityEngine.Vector2 GetLocalPos2D(this Transform t)
	{
		ClassExtensions.vec2_0.x = t.localPosition.x;
		ClassExtensions.vec2_0.y = t.localPosition.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x0600009E RID: 158 RVA: 0x0001AAEC File Offset: 0x00018CEC
	public static global::UnityEngine.Vector2 SetLocalPos2D(this Transform t, global::UnityEngine.Vector2 newPosition)
	{
		ClassExtensions.vec3_0.x = newPosition.x;
		ClassExtensions.vec3_0.y = newPosition.y;
		ClassExtensions.vec3_0.z = t.localPosition.z;
		t.localPosition = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.localPosition.x;
		ClassExtensions.vec2_0.y = t.localPosition.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x0001AB68 File Offset: 0x00018D68
	public static global::UnityEngine.Vector2 SetLocalPos2D(this Transform t, float x, float y)
	{
		ClassExtensions.vec3_0.x = x;
		ClassExtensions.vec3_0.y = y;
		ClassExtensions.vec3_0.z = t.localPosition.z;
		t.localPosition = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.localPosition.x;
		ClassExtensions.vec2_0.y = t.localPosition.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x0001ABDC File Offset: 0x00018DDC
	public static global::UnityEngine.Vector2 AddLocalPos2D(this Transform t, global::UnityEngine.Vector2 value)
	{
		ClassExtensions.vec3_0.x = t.localPosition.x + value.x;
		ClassExtensions.vec3_0.y = t.localPosition.y + value.y;
		ClassExtensions.vec3_0.z = t.localPosition.z;
		t.localPosition = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.localPosition.x;
		ClassExtensions.vec2_0.y = t.localPosition.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x0001AC70 File Offset: 0x00018E70
	public static global::UnityEngine.Vector2 AddLocalPos2D(this Transform t, float x, float y)
	{
		ClassExtensions.vec3_0.x = t.localPosition.x + x;
		ClassExtensions.vec3_0.y = t.localPosition.y + y;
		ClassExtensions.vec3_0.z = t.localPosition.z;
		t.localPosition = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.localPosition.x;
		ClassExtensions.vec2_0.y = t.localPosition.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00007A8F File Offset: 0x00005C8F
	public static float GetXVel(this Rigidbody2D rb)
	{
		return rb.linearVelocity.x;
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00007A9C File Offset: 0x00005C9C
	public static float GetYVel(this Rigidbody2D rb)
	{
		return rb.linearVelocity.y;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00007AA9 File Offset: 0x00005CA9
	public static float SetXVel(this Rigidbody2D rb, float value)
	{
		ClassExtensions.vec2_0.x = value;
		ClassExtensions.vec2_0.y = rb.linearVelocity.y;
		rb.linearVelocity = ClassExtensions.vec2_0;
		return value;
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00007AD7 File Offset: 0x00005CD7
	public static float SetYVel(this Rigidbody2D rb, float value)
	{
		ClassExtensions.vec2_0.x = rb.linearVelocity.x;
		ClassExtensions.vec2_0.y = value;
		rb.linearVelocity = ClassExtensions.vec2_0;
		return value;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x0001ACFC File Offset: 0x00018EFC
	public static float AddXVel(this Rigidbody2D rb, float value)
	{
		ClassExtensions.vec2_0.x = rb.linearVelocity.x + value;
		ClassExtensions.vec2_0.y = rb.linearVelocity.y;
		rb.linearVelocity = ClassExtensions.vec2_0;
		return rb.linearVelocity.x;
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x0001AD4C File Offset: 0x00018F4C
	public static float AddYVel(this Rigidbody2D rb, float value)
	{
		ClassExtensions.vec2_0.x = rb.linearVelocity.x;
		ClassExtensions.vec2_0.y = rb.linearVelocity.y + value;
		rb.linearVelocity = ClassExtensions.vec2_0;
		return rb.linearVelocity.y;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00007B05 File Offset: 0x00005D05
	public static float GetDirection(this Rigidbody2D rb)
	{
		return Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * 57.29578f;
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x0001AD9C File Offset: 0x00018F9C
	public static float GetSpeed(this Rigidbody2D rb)
	{
		return rb.linearVelocity.magnitude;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x0001ADB8 File Offset: 0x00018FB8
	public static float AddSpeed(this Rigidbody2D rb, float addValue)
	{
		ClassExtensions.flt_1 = rb.GetDirection();
		ClassExtensions.flt_0 = addValue * Mathf.Cos(ClassExtensions.flt_1 * 0.017453292f);
		ClassExtensions.flt_1 = addValue * Mathf.Sin(ClassExtensions.flt_1 * 0.017453292f);
		rb.linearVelocity += new global::UnityEngine.Vector2(ClassExtensions.flt_0, ClassExtensions.flt_1);
		return rb.GetSpeed();
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00007B28 File Offset: 0x00005D28
	public static float AddDirection(this Rigidbody2D rb, float addValue)
	{
		ClassExtensions.flt_0 = rb.GetDirection() + addValue;
		rb.SetDirectionAndSpeed(ClassExtensions.flt_0, rb.GetSpeed());
		return ClassExtensions.flt_0;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00007B4E File Offset: 0x00005D4E
	public static global::UnityEngine.Vector2 SetDirectionAndSpeed(this Rigidbody2D rb, float direction, float speed)
	{
		rb.linearVelocity = Util.AngleToAxis2D(direction, speed);
		return rb.linearVelocity;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00007B63 File Offset: 0x00005D63
	public static float GetXVel(this Rigidbody rb)
	{
		return rb.linearVelocity.x;
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00007B70 File Offset: 0x00005D70
	public static float GetYVel(this Rigidbody rb)
	{
		return rb.linearVelocity.y;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00007B7D File Offset: 0x00005D7D
	public static float GetZVel(this Rigidbody rb)
	{
		return rb.linearVelocity.z;
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x0001AE24 File Offset: 0x00019024
	public static float SetXVel(this Rigidbody rb, float value)
	{
		ClassExtensions.vec3_0.x = value;
		ClassExtensions.vec3_0.y = rb.linearVelocity.y;
		ClassExtensions.vec3_0.z = rb.linearVelocity.z;
		rb.linearVelocity = ClassExtensions.vec3_0;
		return value;
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x0001AE74 File Offset: 0x00019074
	public static float SetYVel(this Rigidbody rb, float value)
	{
		ClassExtensions.vec3_0.x = rb.linearVelocity.x;
		ClassExtensions.vec3_0.y = value;
		ClassExtensions.vec3_0.z = rb.linearVelocity.z;
		rb.linearVelocity = ClassExtensions.vec3_0;
		return value;
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x0001AEC4 File Offset: 0x000190C4
	public static float SetZVel(this Rigidbody rb, float value)
	{
		ClassExtensions.vec3_0.x = rb.linearVelocity.x;
		ClassExtensions.vec3_0.y = rb.linearVelocity.y;
		ClassExtensions.vec3_0.z = value;
		rb.linearVelocity = ClassExtensions.vec3_0;
		return value;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x0001AF14 File Offset: 0x00019114
	public static float AddXVel(this Rigidbody rb, float value)
	{
		ClassExtensions.vec3_0.x = rb.linearVelocity.x + value;
		ClassExtensions.vec3_0.y = rb.linearVelocity.y;
		ClassExtensions.vec3_0.z = rb.linearVelocity.z;
		rb.linearVelocity = ClassExtensions.vec3_0;
		return rb.linearVelocity.x;
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x0001AF78 File Offset: 0x00019178
	public static float AddYVel(this Rigidbody rb, float value)
	{
		ClassExtensions.vec3_0.x = rb.linearVelocity.x;
		ClassExtensions.vec3_0.y = rb.linearVelocity.y + value;
		ClassExtensions.vec3_0.z = rb.linearVelocity.z;
		rb.linearVelocity = ClassExtensions.vec3_0;
		return rb.linearVelocity.y;
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x0001AFDC File Offset: 0x000191DC
	public static float AddZVel(this Rigidbody rb, float value)
	{
		ClassExtensions.vec3_0.x = rb.linearVelocity.x;
		ClassExtensions.vec3_0.y = rb.linearVelocity.y;
		ClassExtensions.vec3_0.z = rb.linearVelocity.z + value;
		rb.linearVelocity = ClassExtensions.vec3_0;
		return rb.linearVelocity.z;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00007B8A File Offset: 0x00005D8A
	public static global::UnityEngine.Vector3 GetEulerFromVelocity(this Rigidbody rb)
	{
		return Util.AxisToAngle3D(rb.linearVelocity);
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x0001B040 File Offset: 0x00019240
	public static float GetSpeed(this Rigidbody rb)
	{
		return rb.linearVelocity.magnitude;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x0001B05C File Offset: 0x0001925C
	public static float AddSpeed(this Rigidbody rb, float addValue)
	{
		ClassExtensions.vec3_0 = rb.linearVelocity.normalized;
		rb.linearVelocity += ClassExtensions.vec3_0 * addValue;
		return rb.linearVelocity.magnitude;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00007B97 File Offset: 0x00005D97
	public static global::UnityEngine.Vector2 SetDirectionAndSpeed(this Rigidbody2D rb, float yAngle, float zAngle, float speed)
	{
		ClassExtensions.vec3_0 = Util.AngleToAxis3D(yAngle, zAngle);
		rb.linearVelocity = ClassExtensions.vec3_0 * speed;
		return rb.linearVelocity;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x0001B0A8 File Offset: 0x000192A8
	public static Texture2D RenderToTexture(this Camera cam, int width, int height)
	{
		RenderTexture targetTexture = cam.targetTexture;
		RenderTexture renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
		cam.targetTexture = renderTexture;
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = cam.targetTexture;
		cam.Render();
		Texture2D texture2D = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)cam.targetTexture.width, (float)cam.targetTexture.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = active;
		cam.targetTexture = targetTexture;
		renderTexture.DiscardContents();
		renderTexture.Release();
		return texture2D;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x0001B150 File Offset: 0x00019350
	public static Texture2D ToTexture2D(this RenderTexture tex)
	{
		Texture2D texture2D = new Texture2D(tex.width, tex.height);
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = tex;
		texture2D.ReadPixels(new Rect(0f, 0f, (float)tex.width, (float)tex.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = active;
		return texture2D;
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00007BC1 File Offset: 0x00005DC1
	public static string ToStringSmart(this BigInteger n)
	{
		if (n > 999999999999L || n < -999999999999L)
		{
			return n.ToString("E4");
		}
		return n.ToString("n0");
	}

	// Token: 0x060000BD RID: 189 RVA: 0x0001B1AC File Offset: 0x000193AC
	public static int CastToInt(this BigInteger n)
	{
		int num;
		if (n > 2147483647L)
		{
			num = int.MaxValue;
		}
		else if (n < -2147483648L)
		{
			num = int.MinValue;
		}
		else
		{
			num = (int)n;
		}
		return num;
	}

	// Token: 0x060000BE RID: 190 RVA: 0x0001B1F0 File Offset: 0x000193F0
	public static long CastToLong(this BigInteger n)
	{
		long num;
		if (n > 9223372036854775807L)
		{
			num = long.MaxValue;
		}
		else if (n < -9223372036854775808L)
		{
			num = long.MinValue;
		}
		else
		{
			num = (long)n;
		}
		return num;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x0001B244 File Offset: 0x00019444
	public static int CastToInt(this long n)
	{
		int num;
		if (n > 2147483647L)
		{
			num = int.MaxValue;
		}
		else if (n < -2147483648L)
		{
			num = int.MinValue;
		}
		else
		{
			num = (int)n;
		}
		return num;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00007BFE File Offset: 0x00005DFE
	public static string ToStringSmart(this double n, string defaultFormat, IFormatProvider formatProvider)
	{
		if (n > 999999999999.0 || n < -999999999999.0)
		{
			return n.ToString("E4", formatProvider);
		}
		return n.ToString(defaultFormat, formatProvider);
	}

	// Token: 0x040000CE RID: 206
	private static global::UnityEngine.Vector2 vec2_0 = global::UnityEngine.Vector2.zero;

	// Token: 0x040000CF RID: 207
	private static global::UnityEngine.Vector2 vec2_1 = global::UnityEngine.Vector2.zero;

	// Token: 0x040000D0 RID: 208
	private static global::UnityEngine.Vector3 vec3_0 = global::UnityEngine.Vector3.zero;

	// Token: 0x040000D1 RID: 209
	private static global::UnityEngine.Vector3 vec3_1 = global::UnityEngine.Vector3.zero;

	// Token: 0x040000D2 RID: 210
	private static float flt_0 = 0f;

	// Token: 0x040000D3 RID: 211
	private static float flt_1 = 0f;

	// Token: 0x040000D4 RID: 212
	public const long E_NOTATION_THRESHOLD = 999999999999L;

	// Token: 0x040000D5 RID: 213
	public const double E_NOTATION_THRESHOLD_AS_DOUBLE = 999999999999.0;
}
