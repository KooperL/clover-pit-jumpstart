using System;
using Panik;
using UnityEngine;

public static class PatternScript
{
	// Token: 0x06000467 RID: 1127 RVA: 0x0001DBD0 File Offset: 0x0001BDD0
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

	// Token: 0x06000468 RID: 1128 RVA: 0x0001DCC0 File Offset: 0x0001BEC0
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

	// Token: 0x06000469 RID: 1129 RVA: 0x0001DDE8 File Offset: 0x0001BFE8
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

	// Token: 0x0600046A RID: 1130 RVA: 0x0001DF10 File Offset: 0x0001C110
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

	// Token: 0x0600046B RID: 1131 RVA: 0x0001E3AD File Offset: 0x0001C5AD
	public static bool IsHorizontal(PatternScript.Kind patternKind)
	{
		return patternKind == PatternScript.Kind.horizontal2 || patternKind == PatternScript.Kind.horizontal3 || patternKind == PatternScript.Kind.horizontal4 || patternKind == PatternScript.Kind.horizontal5;
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x0001E3C1 File Offset: 0x0001C5C1
	public static bool IsVertical(PatternScript.Kind patternKind)
	{
		return patternKind == PatternScript.Kind.vertical2 || patternKind == PatternScript.Kind.vertical3;
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x0001E3CD File Offset: 0x0001C5CD
	public static bool IsDiagonal(PatternScript.Kind patternKind)
	{
		return patternKind == PatternScript.Kind.diagonal2 || patternKind == PatternScript.Kind.diagonal3;
	}

	// Token: 0x0600046E RID: 1134 RVA: 0x0001E3D9 File Offset: 0x0001C5D9
	public static bool IsPyramid(PatternScript.Kind patternKind)
	{
		return patternKind == PatternScript.Kind.pyramid || patternKind == PatternScript.Kind.pyramidInverted;
	}

	// Token: 0x0600046F RID: 1135 RVA: 0x0001E3E7 File Offset: 0x0001C5E7
	public static bool IsTriangle(PatternScript.Kind patternKind)
	{
		return patternKind == PatternScript.Kind.triangle || patternKind == PatternScript.Kind.triangleInverted;
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x0001E3F5 File Offset: 0x0001C5F5
	public static bool IsSnake(PatternScript.Kind patternKind)
	{
		return patternKind == PatternScript.Kind.snakeUpDown || patternKind == PatternScript.Kind.snakeDownUp;
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x0001E403 File Offset: 0x0001C603
	public static bool IsEye(PatternScript.Kind patternKind)
	{
		return patternKind == PatternScript.Kind.eye;
	}

	// Token: 0x06000472 RID: 1138 RVA: 0x0001E40C File Offset: 0x0001C60C
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

	private static bool[][] _tempPatternMask;

	public enum Kind
	{
		undefined = -1,
		jackpot,
		horizontal2,
		horizontal3,
		horizontal4,
		horizontal5,
		vertical2,
		vertical3,
		diagonal2,
		diagonal3,
		pyramid,
		pyramidInverted,
		triangle,
		triangleInverted,
		snakeUpDown,
		snakeDownUp,
		eye,
		count
	}
}
