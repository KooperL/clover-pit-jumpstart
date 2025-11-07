using System;
using System.Numerics;
using Panik;
using UnityEngine;

public static class ClassExtensions
{
	// Token: 0x06000060 RID: 96 RVA: 0x0000658F File Offset: 0x0000478F
	public static float GetX(this Transform t)
	{
		return t.position.x;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x0000659C File Offset: 0x0000479C
	public static float GetY(this Transform t)
	{
		return t.position.y;
	}

	// Token: 0x06000062 RID: 98 RVA: 0x000065A9 File Offset: 0x000047A9
	public static float GetZ(this Transform t)
	{
		return t.position.z;
	}

	// Token: 0x06000063 RID: 99 RVA: 0x000065B8 File Offset: 0x000047B8
	public static float SetX(this Transform t, float newX)
	{
		ClassExtensions.vec3_0.x = newX;
		ClassExtensions.vec3_0.y = t.position.y;
		ClassExtensions.vec3_0.z = t.position.z;
		t.position = ClassExtensions.vec3_0;
		return t.position.x;
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00006610 File Offset: 0x00004810
	public static float SetY(this Transform t, float newY)
	{
		ClassExtensions.vec3_0.x = t.position.x;
		ClassExtensions.vec3_0.y = newY;
		ClassExtensions.vec3_0.z = t.position.z;
		t.position = ClassExtensions.vec3_0;
		return t.position.y;
	}

	// Token: 0x06000065 RID: 101 RVA: 0x00006668 File Offset: 0x00004868
	public static float SetZ(this Transform t, float newZ)
	{
		ClassExtensions.vec3_0.x = t.position.x;
		ClassExtensions.vec3_0.y = t.position.y;
		ClassExtensions.vec3_0.z = newZ;
		t.position = ClassExtensions.vec3_0;
		return t.position.z;
	}

	// Token: 0x06000066 RID: 102 RVA: 0x000066C0 File Offset: 0x000048C0
	public static float AddX(this Transform t, float addX)
	{
		ClassExtensions.vec3_0.x = addX;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = 0f;
		t.position += ClassExtensions.vec3_0;
		return t.position.x;
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00006718 File Offset: 0x00004918
	public static float AddY(this Transform t, float addY)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = addY;
		ClassExtensions.vec3_0.z = 0f;
		t.position += ClassExtensions.vec3_0;
		return t.position.y;
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00006770 File Offset: 0x00004970
	public static float AddZ(this Transform t, float addZ)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = addZ;
		t.position += ClassExtensions.vec3_0;
		return t.position.z;
	}

	// Token: 0x06000069 RID: 105 RVA: 0x000067C7 File Offset: 0x000049C7
	public static float GetLocalX(this Transform t)
	{
		return t.localPosition.x;
	}

	// Token: 0x0600006A RID: 106 RVA: 0x000067D4 File Offset: 0x000049D4
	public static float GetLocalY(this Transform t)
	{
		return t.localPosition.y;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x000067E1 File Offset: 0x000049E1
	public static float GetLocalZ(this Transform t)
	{
		return t.localPosition.z;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x000067F0 File Offset: 0x000049F0
	public static float SetLocalX(this Transform t, float newX)
	{
		ClassExtensions.vec3_0.x = newX;
		ClassExtensions.vec3_0.y = t.localPosition.y;
		ClassExtensions.vec3_0.z = t.localPosition.z;
		t.localPosition = ClassExtensions.vec3_0;
		return t.localPosition.x;
	}

	// Token: 0x0600006D RID: 109 RVA: 0x00006848 File Offset: 0x00004A48
	public static float SetLocalY(this Transform t, float newY)
	{
		ClassExtensions.vec3_0.x = t.localPosition.x;
		ClassExtensions.vec3_0.y = newY;
		ClassExtensions.vec3_0.z = t.localPosition.z;
		t.localPosition = ClassExtensions.vec3_0;
		return t.localPosition.y;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x000068A0 File Offset: 0x00004AA0
	public static float SetLocalZ(this Transform t, float newZ)
	{
		ClassExtensions.vec3_0.x = t.localPosition.x;
		ClassExtensions.vec3_0.y = t.localPosition.y;
		ClassExtensions.vec3_0.z = newZ;
		t.localPosition = ClassExtensions.vec3_0;
		return t.localPosition.z;
	}

	// Token: 0x0600006F RID: 111 RVA: 0x000068F8 File Offset: 0x00004AF8
	public static float AddLocalX(this Transform t, float addX)
	{
		ClassExtensions.vec3_0.x = addX;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = 0f;
		t.localPosition += ClassExtensions.vec3_0;
		return t.localPosition.x;
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00006950 File Offset: 0x00004B50
	public static float AddLocalY(this Transform t, float addY)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = addY;
		ClassExtensions.vec3_0.z = 0f;
		t.localPosition += ClassExtensions.vec3_0;
		return t.localPosition.y;
	}

	// Token: 0x06000071 RID: 113 RVA: 0x000069A8 File Offset: 0x00004BA8
	public static float AddLocalZ(this Transform t, float addZ)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = addZ;
		t.localPosition += ClassExtensions.vec3_0;
		return t.localPosition.z;
	}

	// Token: 0x06000072 RID: 114 RVA: 0x000069FF File Offset: 0x00004BFF
	public static float GetXAngle(this Transform t)
	{
		return t.eulerAngles.x;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00006A0C File Offset: 0x00004C0C
	public static float GetYAngle(this Transform t)
	{
		return t.eulerAngles.y;
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00006A19 File Offset: 0x00004C19
	public static float GetZAngle(this Transform t)
	{
		return t.eulerAngles.z;
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00006A28 File Offset: 0x00004C28
	public static float SetXAngle(this Transform t, float newX)
	{
		ClassExtensions.vec3_0.x = newX;
		ClassExtensions.vec3_0.y = t.eulerAngles.y;
		ClassExtensions.vec3_0.z = t.eulerAngles.z;
		t.eulerAngles = ClassExtensions.vec3_0;
		return t.eulerAngles.x;
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00006A80 File Offset: 0x00004C80
	public static float SetYAngle(this Transform t, float newY)
	{
		ClassExtensions.vec3_0.x = t.eulerAngles.x;
		ClassExtensions.vec3_0.y = newY;
		ClassExtensions.vec3_0.z = t.eulerAngles.z;
		t.eulerAngles = ClassExtensions.vec3_0;
		return t.eulerAngles.y;
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00006AD8 File Offset: 0x00004CD8
	public static float SetZAngle(this Transform t, float newZ)
	{
		ClassExtensions.vec3_0.x = t.eulerAngles.x;
		ClassExtensions.vec3_0.y = t.eulerAngles.y;
		ClassExtensions.vec3_0.z = newZ;
		t.eulerAngles = ClassExtensions.vec3_0;
		return t.eulerAngles.z;
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00006B30 File Offset: 0x00004D30
	public static float AddXAngle(this Transform t, float addX)
	{
		ClassExtensions.vec3_0.x = addX;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = 0f;
		t.eulerAngles += ClassExtensions.vec3_0;
		return t.eulerAngles.x;
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00006B88 File Offset: 0x00004D88
	public static float AddYAngle(this Transform t, float addY)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = addY;
		ClassExtensions.vec3_0.z = 0f;
		t.eulerAngles += ClassExtensions.vec3_0;
		return t.eulerAngles.y;
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00006BE0 File Offset: 0x00004DE0
	public static float AddZAngle(this Transform t, float addZ)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = addZ;
		t.eulerAngles += ClassExtensions.vec3_0;
		return t.eulerAngles.z;
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00006C37 File Offset: 0x00004E37
	public static float GetLocalXAngle(this Transform t)
	{
		return t.localEulerAngles.x;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00006C44 File Offset: 0x00004E44
	public static float GetLocalYAngle(this Transform t)
	{
		return t.localEulerAngles.y;
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00006C51 File Offset: 0x00004E51
	public static float GetLocalZAngle(this Transform t)
	{
		return t.localEulerAngles.z;
	}

	// Token: 0x0600007E RID: 126 RVA: 0x00006C60 File Offset: 0x00004E60
	public static float SetLocalXAngle(this Transform t, float newX)
	{
		ClassExtensions.vec3_0.x = newX;
		ClassExtensions.vec3_0.y = t.localEulerAngles.y;
		ClassExtensions.vec3_0.z = t.localEulerAngles.z;
		t.localEulerAngles = ClassExtensions.vec3_0;
		return t.localEulerAngles.x;
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00006CB8 File Offset: 0x00004EB8
	public static float SetLocalYAngle(this Transform t, float newY)
	{
		ClassExtensions.vec3_0.x = t.localEulerAngles.x;
		ClassExtensions.vec3_0.y = newY;
		ClassExtensions.vec3_0.z = t.localEulerAngles.z;
		t.localEulerAngles = ClassExtensions.vec3_0;
		return t.localEulerAngles.y;
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00006D10 File Offset: 0x00004F10
	public static float SetLocalZAngle(this Transform t, float newZ)
	{
		ClassExtensions.vec3_0.x = t.localEulerAngles.x;
		ClassExtensions.vec3_0.y = t.localEulerAngles.y;
		ClassExtensions.vec3_0.z = newZ;
		t.localEulerAngles = ClassExtensions.vec3_0;
		return t.localEulerAngles.z;
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00006D68 File Offset: 0x00004F68
	public static float AddLocalXAngle(this Transform t, float addX)
	{
		ClassExtensions.vec3_0.x = addX;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = 0f;
		t.localEulerAngles += ClassExtensions.vec3_0;
		return t.localEulerAngles.x;
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00006DC0 File Offset: 0x00004FC0
	public static float AddLocalYAngle(this Transform t, float addY)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = addY;
		ClassExtensions.vec3_0.z = 0f;
		t.localEulerAngles += ClassExtensions.vec3_0;
		return t.localEulerAngles.y;
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00006E18 File Offset: 0x00005018
	public static float AddLocalZAngle(this Transform t, float addZ)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = addZ;
		t.localEulerAngles += ClassExtensions.vec3_0;
		return t.localEulerAngles.z;
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00006E6F File Offset: 0x0000506F
	public static float GetLocalXScale(this Transform t)
	{
		return t.localScale.x;
	}

	// Token: 0x06000085 RID: 133 RVA: 0x00006E7C File Offset: 0x0000507C
	public static float GetLocalYScale(this Transform t)
	{
		return t.localScale.y;
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00006E89 File Offset: 0x00005089
	public static float GetLocalZScale(this Transform t)
	{
		return t.localScale.z;
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00006E98 File Offset: 0x00005098
	public static float SetLocalXScale(this Transform t, float newX)
	{
		ClassExtensions.vec3_0.x = newX;
		ClassExtensions.vec3_0.y = t.localScale.y;
		ClassExtensions.vec3_0.z = t.localScale.z;
		t.localScale = ClassExtensions.vec3_0;
		return t.localScale.x;
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00006EF0 File Offset: 0x000050F0
	public static float SetLocalYScale(this Transform t, float newY)
	{
		ClassExtensions.vec3_0.x = t.localScale.x;
		ClassExtensions.vec3_0.y = newY;
		ClassExtensions.vec3_0.z = t.localScale.z;
		t.localScale = ClassExtensions.vec3_0;
		return t.localScale.y;
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00006F48 File Offset: 0x00005148
	public static float SetLocalZScale(this Transform t, float newZ)
	{
		ClassExtensions.vec3_0.x = t.localScale.x;
		ClassExtensions.vec3_0.y = t.localScale.y;
		ClassExtensions.vec3_0.z = newZ;
		t.localScale = ClassExtensions.vec3_0;
		return t.localScale.z;
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00006FA0 File Offset: 0x000051A0
	public static float AddLocalXScale(this Transform t, float addX)
	{
		ClassExtensions.vec3_0.x = addX;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = 0f;
		t.localScale += ClassExtensions.vec3_0;
		return t.localScale.x;
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00006FF8 File Offset: 0x000051F8
	public static float AddLocalYScale(this Transform t, float addY)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = addY;
		ClassExtensions.vec3_0.z = 0f;
		t.localScale += ClassExtensions.vec3_0;
		return t.localScale.y;
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00007050 File Offset: 0x00005250
	public static float AddLocalZScale(this Transform t, float addZ)
	{
		ClassExtensions.vec3_0.x = 0f;
		ClassExtensions.vec3_0.y = 0f;
		ClassExtensions.vec3_0.z = addZ;
		t.localScale += ClassExtensions.vec3_0;
		return t.localScale.z;
	}

	// Token: 0x0600008D RID: 141 RVA: 0x000070A7 File Offset: 0x000052A7
	public static Vector2 GetPos2D(this Transform t)
	{
		ClassExtensions.vec2_0.x = t.position.x;
		ClassExtensions.vec2_0.y = t.position.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x0600008E RID: 142 RVA: 0x000070D8 File Offset: 0x000052D8
	public static Vector2 SetPos2D(this Transform t, Vector2 newPosition)
	{
		ClassExtensions.vec3_0.x = newPosition.x;
		ClassExtensions.vec3_0.y = newPosition.y;
		ClassExtensions.vec3_0.z = t.position.z;
		t.position = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.position.x;
		ClassExtensions.vec2_0.y = t.position.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00007154 File Offset: 0x00005354
	public static Vector2 SetPos2D(this Transform t, float x, float y)
	{
		ClassExtensions.vec3_0.x = x;
		ClassExtensions.vec3_0.y = y;
		ClassExtensions.vec3_0.z = t.position.z;
		t.position = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.position.x;
		ClassExtensions.vec2_0.y = t.position.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x06000090 RID: 144 RVA: 0x000071C8 File Offset: 0x000053C8
	public static Vector2 AddPos2D(this Transform t, Vector2 value)
	{
		ClassExtensions.vec3_0.x = t.position.x + value.x;
		ClassExtensions.vec3_0.y = t.position.y + value.y;
		ClassExtensions.vec3_0.z = t.position.z;
		t.position = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.position.x;
		ClassExtensions.vec2_0.y = t.position.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x06000091 RID: 145 RVA: 0x0000725C File Offset: 0x0000545C
	public static Vector2 AddPos2D(this Transform t, float x, float y)
	{
		ClassExtensions.vec3_0.x = t.position.x + x;
		ClassExtensions.vec3_0.y = t.position.y + y;
		ClassExtensions.vec3_0.z = t.position.z;
		t.position = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.position.x;
		ClassExtensions.vec2_0.y = t.position.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x06000092 RID: 146 RVA: 0x000072E6 File Offset: 0x000054E6
	public static Vector2 GetLocalPos2D(this Transform t)
	{
		ClassExtensions.vec2_0.x = t.localPosition.x;
		ClassExtensions.vec2_0.y = t.localPosition.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00007318 File Offset: 0x00005518
	public static Vector2 SetLocalPos2D(this Transform t, Vector2 newPosition)
	{
		ClassExtensions.vec3_0.x = newPosition.x;
		ClassExtensions.vec3_0.y = newPosition.y;
		ClassExtensions.vec3_0.z = t.localPosition.z;
		t.localPosition = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.localPosition.x;
		ClassExtensions.vec2_0.y = t.localPosition.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00007394 File Offset: 0x00005594
	public static Vector2 SetLocalPos2D(this Transform t, float x, float y)
	{
		ClassExtensions.vec3_0.x = x;
		ClassExtensions.vec3_0.y = y;
		ClassExtensions.vec3_0.z = t.localPosition.z;
		t.localPosition = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.localPosition.x;
		ClassExtensions.vec2_0.y = t.localPosition.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00007408 File Offset: 0x00005608
	public static Vector2 AddLocalPos2D(this Transform t, Vector2 value)
	{
		ClassExtensions.vec3_0.x = t.localPosition.x + value.x;
		ClassExtensions.vec3_0.y = t.localPosition.y + value.y;
		ClassExtensions.vec3_0.z = t.localPosition.z;
		t.localPosition = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.localPosition.x;
		ClassExtensions.vec2_0.y = t.localPosition.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x06000096 RID: 150 RVA: 0x0000749C File Offset: 0x0000569C
	public static Vector2 AddLocalPos2D(this Transform t, float x, float y)
	{
		ClassExtensions.vec3_0.x = t.localPosition.x + x;
		ClassExtensions.vec3_0.y = t.localPosition.y + y;
		ClassExtensions.vec3_0.z = t.localPosition.z;
		t.localPosition = ClassExtensions.vec3_0;
		ClassExtensions.vec2_0.x = t.localPosition.x;
		ClassExtensions.vec2_0.y = t.localPosition.y;
		return ClassExtensions.vec2_0;
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00007526 File Offset: 0x00005726
	public static float GetXVel(this Rigidbody2D rb)
	{
		return rb.linearVelocity.x;
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00007533 File Offset: 0x00005733
	public static float GetYVel(this Rigidbody2D rb)
	{
		return rb.linearVelocity.y;
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00007540 File Offset: 0x00005740
	public static float SetXVel(this Rigidbody2D rb, float value)
	{
		ClassExtensions.vec2_0.x = value;
		ClassExtensions.vec2_0.y = rb.linearVelocity.y;
		rb.linearVelocity = ClassExtensions.vec2_0;
		return value;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x0000756E File Offset: 0x0000576E
	public static float SetYVel(this Rigidbody2D rb, float value)
	{
		ClassExtensions.vec2_0.x = rb.linearVelocity.x;
		ClassExtensions.vec2_0.y = value;
		rb.linearVelocity = ClassExtensions.vec2_0;
		return value;
	}

	// Token: 0x0600009B RID: 155 RVA: 0x0000759C File Offset: 0x0000579C
	public static float AddXVel(this Rigidbody2D rb, float value)
	{
		ClassExtensions.vec2_0.x = rb.linearVelocity.x + value;
		ClassExtensions.vec2_0.y = rb.linearVelocity.y;
		rb.linearVelocity = ClassExtensions.vec2_0;
		return rb.linearVelocity.x;
	}

	// Token: 0x0600009C RID: 156 RVA: 0x000075EC File Offset: 0x000057EC
	public static float AddYVel(this Rigidbody2D rb, float value)
	{
		ClassExtensions.vec2_0.x = rb.linearVelocity.x;
		ClassExtensions.vec2_0.y = rb.linearVelocity.y + value;
		rb.linearVelocity = ClassExtensions.vec2_0;
		return rb.linearVelocity.y;
	}

	// Token: 0x0600009D RID: 157 RVA: 0x0000763B File Offset: 0x0000583B
	public static float GetDirection(this Rigidbody2D rb)
	{
		return Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * 57.29578f;
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00007660 File Offset: 0x00005860
	public static float GetSpeed(this Rigidbody2D rb)
	{
		return rb.linearVelocity.magnitude;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x0000767C File Offset: 0x0000587C
	public static float AddSpeed(this Rigidbody2D rb, float addValue)
	{
		ClassExtensions.flt_1 = rb.GetDirection();
		ClassExtensions.flt_0 = addValue * Mathf.Cos(ClassExtensions.flt_1 * 0.017453292f);
		ClassExtensions.flt_1 = addValue * Mathf.Sin(ClassExtensions.flt_1 * 0.017453292f);
		rb.linearVelocity += new Vector2(ClassExtensions.flt_0, ClassExtensions.flt_1);
		return rb.GetSpeed();
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x000076E8 File Offset: 0x000058E8
	public static float AddDirection(this Rigidbody2D rb, float addValue)
	{
		ClassExtensions.flt_0 = rb.GetDirection() + addValue;
		rb.SetDirectionAndSpeed(ClassExtensions.flt_0, rb.GetSpeed());
		return ClassExtensions.flt_0;
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x0000770E File Offset: 0x0000590E
	public static Vector2 SetDirectionAndSpeed(this Rigidbody2D rb, float direction, float speed)
	{
		rb.linearVelocity = Util.AngleToAxis2D(direction, speed);
		return rb.linearVelocity;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00007723 File Offset: 0x00005923
	public static float GetXVel(this Rigidbody rb)
	{
		return rb.linearVelocity.x;
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00007730 File Offset: 0x00005930
	public static float GetYVel(this Rigidbody rb)
	{
		return rb.linearVelocity.y;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x0000773D File Offset: 0x0000593D
	public static float GetZVel(this Rigidbody rb)
	{
		return rb.linearVelocity.z;
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x0000774C File Offset: 0x0000594C
	public static float SetXVel(this Rigidbody rb, float value)
	{
		ClassExtensions.vec3_0.x = value;
		ClassExtensions.vec3_0.y = rb.linearVelocity.y;
		ClassExtensions.vec3_0.z = rb.linearVelocity.z;
		rb.linearVelocity = ClassExtensions.vec3_0;
		return value;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x0000779C File Offset: 0x0000599C
	public static float SetYVel(this Rigidbody rb, float value)
	{
		ClassExtensions.vec3_0.x = rb.linearVelocity.x;
		ClassExtensions.vec3_0.y = value;
		ClassExtensions.vec3_0.z = rb.linearVelocity.z;
		rb.linearVelocity = ClassExtensions.vec3_0;
		return value;
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x000077EC File Offset: 0x000059EC
	public static float SetZVel(this Rigidbody rb, float value)
	{
		ClassExtensions.vec3_0.x = rb.linearVelocity.x;
		ClassExtensions.vec3_0.y = rb.linearVelocity.y;
		ClassExtensions.vec3_0.z = value;
		rb.linearVelocity = ClassExtensions.vec3_0;
		return value;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x0000783C File Offset: 0x00005A3C
	public static float AddXVel(this Rigidbody rb, float value)
	{
		ClassExtensions.vec3_0.x = rb.linearVelocity.x + value;
		ClassExtensions.vec3_0.y = rb.linearVelocity.y;
		ClassExtensions.vec3_0.z = rb.linearVelocity.z;
		rb.linearVelocity = ClassExtensions.vec3_0;
		return rb.linearVelocity.x;
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x000078A0 File Offset: 0x00005AA0
	public static float AddYVel(this Rigidbody rb, float value)
	{
		ClassExtensions.vec3_0.x = rb.linearVelocity.x;
		ClassExtensions.vec3_0.y = rb.linearVelocity.y + value;
		ClassExtensions.vec3_0.z = rb.linearVelocity.z;
		rb.linearVelocity = ClassExtensions.vec3_0;
		return rb.linearVelocity.y;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00007904 File Offset: 0x00005B04
	public static float AddZVel(this Rigidbody rb, float value)
	{
		ClassExtensions.vec3_0.x = rb.linearVelocity.x;
		ClassExtensions.vec3_0.y = rb.linearVelocity.y;
		ClassExtensions.vec3_0.z = rb.linearVelocity.z + value;
		rb.linearVelocity = ClassExtensions.vec3_0;
		return rb.linearVelocity.z;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x00007968 File Offset: 0x00005B68
	public static Vector3 GetEulerFromVelocity(this Rigidbody rb)
	{
		return Util.AxisToAngle3D(rb.linearVelocity);
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00007978 File Offset: 0x00005B78
	public static float GetSpeed(this Rigidbody rb)
	{
		return rb.linearVelocity.magnitude;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00007994 File Offset: 0x00005B94
	public static float AddSpeed(this Rigidbody rb, float addValue)
	{
		ClassExtensions.vec3_0 = rb.linearVelocity.normalized;
		rb.linearVelocity += ClassExtensions.vec3_0 * addValue;
		return rb.linearVelocity.magnitude;
	}

	// Token: 0x060000AE RID: 174 RVA: 0x000079DE File Offset: 0x00005BDE
	public static Vector2 SetDirectionAndSpeed(this Rigidbody2D rb, float yAngle, float zAngle, float speed)
	{
		ClassExtensions.vec3_0 = Util.AngleToAxis3D(yAngle, zAngle);
		rb.linearVelocity = ClassExtensions.vec3_0 * speed;
		return rb.linearVelocity;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00007A08 File Offset: 0x00005C08
	public static Texture2D RenderToTexture(this Camera cam, int width, int height)
	{
		RenderTexture targetTexture = cam.targetTexture;
		RenderTexture renderTexture = new RenderTexture(width, height, 24, 0);
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

	// Token: 0x060000B0 RID: 176 RVA: 0x00007AB0 File Offset: 0x00005CB0
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

	// Token: 0x060000B1 RID: 177 RVA: 0x00007B0B File Offset: 0x00005D0B
	public static string ToStringSmart(this BigInteger n)
	{
		if (n > 999999999999L || n < -999999999999L)
		{
			return n.ToString("E4");
		}
		return n.ToString("n0");
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00007B48 File Offset: 0x00005D48
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

	// Token: 0x060000B3 RID: 179 RVA: 0x00007B8C File Offset: 0x00005D8C
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

	// Token: 0x060000B4 RID: 180 RVA: 0x00007BE0 File Offset: 0x00005DE0
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

	// Token: 0x060000B5 RID: 181 RVA: 0x00007C15 File Offset: 0x00005E15
	public static string ToStringSmart(this double n, string defaultFormat, IFormatProvider formatProvider)
	{
		if (n > 999999999999.0 || n < -999999999999.0)
		{
			return n.ToString("E4", formatProvider);
		}
		return n.ToString(defaultFormat, formatProvider);
	}

	private static Vector2 vec2_0 = Vector2.zero;

	private static Vector2 vec2_1 = Vector2.zero;

	private static Vector3 vec3_0 = Vector3.zero;

	private static Vector3 vec3_1 = Vector3.zero;

	private static float flt_0 = 0f;

	private static float flt_1 = 0f;

	public const long E_NOTATION_THRESHOLD = 999999999999L;

	public const double E_NOTATION_THRESHOLD_AS_DOUBLE = 999999999999.0;
}
