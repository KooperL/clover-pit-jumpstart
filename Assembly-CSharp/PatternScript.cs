using System;
using Panik;
using UnityEngine;

// Token: 0x02000056 RID: 86
public static class PatternScript
{
	// Token: 0x060004F5 RID: 1269 RVA: 0x000320F8 File Offset: 0x000302F8
	public static ulong PatternOrderWeightMask(PatternScript.Kind kind)
	{
		ulong num = 1UL;
		switch (kind)
		{
		case PatternScript.Kind.jackpot:
			num <<= 47;
			break;
		case PatternScript.Kind.horizontal2:
			num <<= 32;
			break;
		case PatternScript.Kind.horizontal3:
			num <<= 35;
			break;
		case PatternScript.Kind.horizontal4:
			num <<= 38;
			break;
		case PatternScript.Kind.horizontal5:
			num <<= 39;
			break;
		case PatternScript.Kind.vertical2:
			num <<= 33;
			break;
		case PatternScript.Kind.vertical3:
			num <<= 36;
			break;
		case PatternScript.Kind.diagonal2:
			num <<= 34;
			break;
		case PatternScript.Kind.diagonal3:
			num <<= 37;
			break;
		case PatternScript.Kind.pyramid:
			num <<= 40;
			break;
		case PatternScript.Kind.pyramidInverted:
			num <<= 41;
			break;
		case PatternScript.Kind.triangle:
			num <<= 42;
			break;
		case PatternScript.Kind.triangleInverted:
			num <<= 43;
			break;
		case PatternScript.Kind.snakeUpDown:
			num <<= 44;
			break;
		case PatternScript.Kind.snakeDownUp:
			num <<= 45;
			break;
		case PatternScript.Kind.eye:
			num <<= 46;
			break;
		default:
			Debug.LogError("PatternScript.PatternOrderWeightMask(): kind not handled: " + kind.ToString());
			num = 0UL;
			break;
		}
		return num;
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x000321E8 File Offset: 0x000303E8
	public static string GetPatterTranslatedName(PatternScript.Kind patternKind)
	{
		switch (patternKind)
		{
		case PatternScript.Kind.jackpot:
			return Translation.Get("SLOT_COMBO_TEXT_JACKPOT");
		case PatternScript.Kind.horizontal2:
			return Translation.Get("SLOT_COMBO_TEXT_HOR_2");
		case PatternScript.Kind.horizontal3:
			return Translation.Get("SLOT_COMBO_TEXT_HOR_3");
		case PatternScript.Kind.horizontal4:
			return Translation.Get("SLOT_COMBO_TEXT_HOR_4");
		case PatternScript.Kind.horizontal5:
			return Translation.Get("SLOT_COMBO_TEXT_HOR_5");
		case PatternScript.Kind.vertical2:
			return Translation.Get("SLOT_COMBO_TEXT_VER_2");
		case PatternScript.Kind.vertical3:
			return Translation.Get("SLOT_COMBO_TEXT_VER_3");
		case PatternScript.Kind.diagonal2:
			return Translation.Get("SLOT_COMBO_TEXT_DIAGONAL_2");
		case PatternScript.Kind.diagonal3:
			return Translation.Get("SLOT_COMBO_TEXT_DIAGONAL_3");
		case PatternScript.Kind.pyramid:
			return Translation.Get("SLOT_COMBO_TEXT_PYRAMID");
		case PatternScript.Kind.pyramidInverted:
			return Translation.Get("SLOT_COMBO_TEXT_PYRAMID_REVERSED");
		case PatternScript.Kind.triangle:
			return Translation.Get("SLOT_COMBO_TEXT_TRIANGLE");
		case PatternScript.Kind.triangleInverted:
			return Translation.Get("SLOT_COMBO_TEXT_TRIANGLE_REVERSED");
		case PatternScript.Kind.snakeUpDown:
			return Translation.Get("SLOT_COMBO_TEXT_SNAKE_UP_DOWN");
		case PatternScript.Kind.snakeDownUp:
			return Translation.Get("SLOT_COMBO_TEXT_SNAKE_DOWN_UP");
		case PatternScript.Kind.eye:
			return Translation.Get("SLOT_COMBO_TEXT_EYE");
		default:
			Debug.LogError("Cannot return pattern name. Pattern kind not handled: " + patternKind.ToString());
			return null;
		}
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x00032310 File Offset: 0x00030510
	public static string GetPatterTranslatedName_Short(PatternScript.Kind patternKind)
	{
		switch (patternKind)
		{
		case PatternScript.Kind.jackpot:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_JACKPOT");
		case PatternScript.Kind.horizontal2:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_HOR_2");
		case PatternScript.Kind.horizontal3:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_HOR_3");
		case PatternScript.Kind.horizontal4:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_HOR_4");
		case PatternScript.Kind.horizontal5:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_HOR_5");
		case PatternScript.Kind.vertical2:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_VER_2");
		case PatternScript.Kind.vertical3:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_VER_3");
		case PatternScript.Kind.diagonal2:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_DIAGONAL_2");
		case PatternScript.Kind.diagonal3:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_DIAGONAL_3");
		case PatternScript.Kind.pyramid:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_PYRAMID");
		case PatternScript.Kind.pyramidInverted:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_PYRAMID_REVERSED");
		case PatternScript.Kind.triangle:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_TRIANGLE");
		case PatternScript.Kind.triangleInverted:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_TRIANGLE_REVERSED");
		case PatternScript.Kind.snakeUpDown:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_SNAKE_UP_DOWN");
		case PatternScript.Kind.snakeDownUp:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_SNAKE_DOWN_UP");
		case PatternScript.Kind.eye:
			return Translation.Get("SLOT_COMBO_TEXT_SHORT_EYE");
		default:
			Debug.LogError("Cannot return pattern name. Pattern kind not handled: " + patternKind.ToString());
			return null;
		}
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x00032438 File Offset: 0x00030638
	public static bool[][] GetPatternMask(PatternScript.Kind patternKind, bool diagonalAltCheck)
	{
		if (PatternScript._tempPatternMask == null)
		{
			PatternScript._tempPatternMask = new bool[3][];
			for (int i = 0; i < 3; i++)
			{
				PatternScript._tempPatternMask[i] = new bool[5];
			}
		}
		for (int j = 0; j < PatternScript._tempPatternMask.Length; j++)
		{
			for (int k = 0; k < PatternScript._tempPatternMask[j].Length; k++)
			{
				PatternScript._tempPatternMask[j][k] = false;
			}
		}
		switch (patternKind)
		{
		case PatternScript.Kind.jackpot:
		{
			for (int l = 0; l < 3; l++)
			{
				for (int m = 0; m < 5; m++)
				{
					PatternScript._tempPatternMask[l][m] = true;
				}
			}
			break;
		}
		case PatternScript.Kind.horizontal2:
			PatternScript._tempPatternMask[0][0] = true;
			PatternScript._tempPatternMask[0][1] = true;
			break;
		case PatternScript.Kind.horizontal3:
			PatternScript._tempPatternMask[0][0] = true;
			PatternScript._tempPatternMask[0][1] = true;
			PatternScript._tempPatternMask[0][2] = true;
			break;
		case PatternScript.Kind.horizontal4:
			PatternScript._tempPatternMask[0][0] = true;
			PatternScript._tempPatternMask[0][1] = true;
			PatternScript._tempPatternMask[0][2] = true;
			PatternScript._tempPatternMask[0][3] = true;
			break;
		case PatternScript.Kind.horizontal5:
			PatternScript._tempPatternMask[0][0] = true;
			PatternScript._tempPatternMask[0][1] = true;
			PatternScript._tempPatternMask[0][2] = true;
			PatternScript._tempPatternMask[0][3] = true;
			PatternScript._tempPatternMask[0][4] = true;
			break;
		case PatternScript.Kind.vertical2:
			PatternScript._tempPatternMask[0][0] = true;
			PatternScript._tempPatternMask[1][0] = true;
			break;
		case PatternScript.Kind.vertical3:
			PatternScript._tempPatternMask[0][0] = true;
			PatternScript._tempPatternMask[1][0] = true;
			PatternScript._tempPatternMask[2][0] = true;
			break;
		case PatternScript.Kind.diagonal2:
			if (diagonalAltCheck)
			{
				PatternScript._tempPatternMask[0][1] = true;
				PatternScript._tempPatternMask[1][0] = true;
			}
			else
			{
				PatternScript._tempPatternMask[0][0] = true;
				PatternScript._tempPatternMask[1][1] = true;
			}
			break;
		case PatternScript.Kind.diagonal3:
			if (diagonalAltCheck)
			{
				PatternScript._tempPatternMask[2][0] = true;
				PatternScript._tempPatternMask[1][1] = true;
				PatternScript._tempPatternMask[0][2] = true;
			}
			else
			{
				PatternScript._tempPatternMask[0][0] = true;
				PatternScript._tempPatternMask[1][1] = true;
				PatternScript._tempPatternMask[2][2] = true;
			}
			break;
		case PatternScript.Kind.pyramid:
			PatternScript._tempPatternMask[2][0] = true;
			PatternScript._tempPatternMask[1][1] = true;
			PatternScript._tempPatternMask[0][2] = true;
			PatternScript._tempPatternMask[1][3] = true;
			PatternScript._tempPatternMask[2][4] = true;
			break;
		case PatternScript.Kind.pyramidInverted:
			PatternScript._tempPatternMask[0][0] = true;
			PatternScript._tempPatternMask[1][1] = true;
			PatternScript._tempPatternMask[2][2] = true;
			PatternScript._tempPatternMask[1][3] = true;
			PatternScript._tempPatternMask[0][4] = true;
			break;
		case PatternScript.Kind.triangle:
			PatternScript._tempPatternMask[2][0] = true;
			PatternScript._tempPatternMask[1][1] = true;
			PatternScript._tempPatternMask[0][2] = true;
			PatternScript._tempPatternMask[1][3] = true;
			PatternScript._tempPatternMask[2][4] = true;
			PatternScript._tempPatternMask[2][1] = true;
			PatternScript._tempPatternMask[2][2] = true;
			PatternScript._tempPatternMask[2][3] = true;
			break;
		case PatternScript.Kind.triangleInverted:
			PatternScript._tempPatternMask[0][0] = true;
			PatternScript._tempPatternMask[1][1] = true;
			PatternScript._tempPatternMask[2][2] = true;
			PatternScript._tempPatternMask[1][3] = true;
			PatternScript._tempPatternMask[0][4] = true;
			PatternScript._tempPatternMask[0][1] = true;
			PatternScript._tempPatternMask[0][2] = true;
			PatternScript._tempPatternMask[0][3] = true;
			break;
		case PatternScript.Kind.snakeUpDown:
			PatternScript._tempPatternMask[0][0] = true;
			PatternScript._tempPatternMask[1][0] = true;
			PatternScript._tempPatternMask[2][0] = true;
			PatternScript._tempPatternMask[2][1] = true;
			PatternScript._tempPatternMask[2][2] = true;
			PatternScript._tempPatternMask[1][2] = true;
			PatternScript._tempPatternMask[0][2] = true;
			PatternScript._tempPatternMask[0][3] = true;
			PatternScript._tempPatternMask[0][4] = true;
			PatternScript._tempPatternMask[1][4] = true;
			PatternScript._tempPatternMask[2][4] = true;
			break;
		case PatternScript.Kind.snakeDownUp:
			PatternScript._tempPatternMask[2][0] = true;
			PatternScript._tempPatternMask[1][0] = true;
			PatternScript._tempPatternMask[0][0] = true;
			PatternScript._tempPatternMask[0][1] = true;
			PatternScript._tempPatternMask[0][2] = true;
			PatternScript._tempPatternMask[1][2] = true;
			PatternScript._tempPatternMask[2][2] = true;
			PatternScript._tempPatternMask[2][3] = true;
			PatternScript._tempPatternMask[2][4] = true;
			PatternScript._tempPatternMask[1][4] = true;
			PatternScript._tempPatternMask[0][4] = true;
			break;
		case PatternScript.Kind.eye:
			PatternScript._tempPatternMask[1][0] = true;
			PatternScript._tempPatternMask[1][1] = true;
			PatternScript._tempPatternMask[0][1] = true;
			PatternScript._tempPatternMask[2][1] = true;
			PatternScript._tempPatternMask[0][2] = true;
			PatternScript._tempPatternMask[2][2] = true;
			PatternScript._tempPatternMask[0][3] = true;
			PatternScript._tempPatternMask[1][3] = true;
			PatternScript._tempPatternMask[2][3] = true;
			PatternScript._tempPatternMask[1][4] = true;
			break;
		}
		return PatternScript._tempPatternMask;
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x0000985E File Offset: 0x00007A5E
	public static bool IsHorizontal(PatternScript.Kind patternKind)
	{
		return patternKind == PatternScript.Kind.horizontal2 || patternKind == PatternScript.Kind.horizontal3 || patternKind == PatternScript.Kind.horizontal4 || patternKind == PatternScript.Kind.horizontal5;
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x00009872 File Offset: 0x00007A72
	public static bool IsVertical(PatternScript.Kind patternKind)
	{
		return patternKind == PatternScript.Kind.vertical2 || patternKind == PatternScript.Kind.vertical3;
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x0000987E File Offset: 0x00007A7E
	public static bool IsDiagonal(PatternScript.Kind patternKind)
	{
		return patternKind == PatternScript.Kind.diagonal2 || patternKind == PatternScript.Kind.diagonal3;
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x0000988A File Offset: 0x00007A8A
	public static bool IsPyramid(PatternScript.Kind patternKind)
	{
		return patternKind == PatternScript.Kind.pyramid || patternKind == PatternScript.Kind.pyramidInverted;
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x00009898 File Offset: 0x00007A98
	public static bool IsTriangle(PatternScript.Kind patternKind)
	{
		return patternKind == PatternScript.Kind.triangle || patternKind == PatternScript.Kind.triangleInverted;
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x000098A6 File Offset: 0x00007AA6
	public static bool IsSnake(PatternScript.Kind patternKind)
	{
		return patternKind == PatternScript.Kind.snakeUpDown || patternKind == PatternScript.Kind.snakeDownUp;
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x000098B4 File Offset: 0x00007AB4
	public static bool IsEye(PatternScript.Kind patternKind)
	{
		return patternKind == PatternScript.Kind.eye;
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x000328D8 File Offset: 0x00030AD8
	public static int GetElementsCount(PatternScript.Kind patternKind)
	{
		switch (patternKind)
		{
		case PatternScript.Kind.jackpot:
			return 15;
		case PatternScript.Kind.horizontal2:
			return 2;
		case PatternScript.Kind.horizontal3:
			return 3;
		case PatternScript.Kind.horizontal4:
			return 4;
		case PatternScript.Kind.horizontal5:
			return 5;
		case PatternScript.Kind.vertical2:
			return 2;
		case PatternScript.Kind.vertical3:
			return 3;
		case PatternScript.Kind.diagonal2:
			return 2;
		case PatternScript.Kind.diagonal3:
			return 3;
		case PatternScript.Kind.pyramid:
			return 5;
		case PatternScript.Kind.pyramidInverted:
			return 5;
		case PatternScript.Kind.triangle:
			return 8;
		case PatternScript.Kind.triangleInverted:
			return 8;
		case PatternScript.Kind.snakeUpDown:
			return 11;
		case PatternScript.Kind.snakeDownUp:
			return 11;
		case PatternScript.Kind.eye:
			return 10;
		default:
			Debug.LogError("Cannot return pattern elements count. Pattern kind not handled: " + patternKind.ToString());
			return 0;
		}
	}

	// Token: 0x04000495 RID: 1173
	private static bool[][] _tempPatternMask;

	// Token: 0x02000057 RID: 87
	public enum Kind
	{
		// Token: 0x04000497 RID: 1175
		undefined = -1,
		// Token: 0x04000498 RID: 1176
		jackpot,
		// Token: 0x04000499 RID: 1177
		horizontal2,
		// Token: 0x0400049A RID: 1178
		horizontal3,
		// Token: 0x0400049B RID: 1179
		horizontal4,
		// Token: 0x0400049C RID: 1180
		horizontal5,
		// Token: 0x0400049D RID: 1181
		vertical2,
		// Token: 0x0400049E RID: 1182
		vertical3,
		// Token: 0x0400049F RID: 1183
		diagonal2,
		// Token: 0x040004A0 RID: 1184
		diagonal3,
		// Token: 0x040004A1 RID: 1185
		pyramid,
		// Token: 0x040004A2 RID: 1186
		pyramidInverted,
		// Token: 0x040004A3 RID: 1187
		triangle,
		// Token: 0x040004A4 RID: 1188
		triangleInverted,
		// Token: 0x040004A5 RID: 1189
		snakeUpDown,
		// Token: 0x040004A6 RID: 1190
		snakeDownUp,
		// Token: 0x040004A7 RID: 1191
		eye,
		// Token: 0x040004A8 RID: 1192
		count
	}
}
