using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using Panik;
using TMPro;
using UnityEngine;

// Token: 0x020000D8 RID: 216
public class PosterScript : MonoBehaviour
{
	// Token: 0x06000B2B RID: 2859 RVA: 0x00059D18 File Offset: 0x00057F18
	private void RefreshText()
	{
		PosterScript.PosterKind posterKind = this.posterKind;
		if (posterKind == PosterScript.PosterKind.symbols)
		{
			this._RefreshText_Symbols();
			return;
		}
		if (posterKind != PosterScript.PosterKind.patterns)
		{
			return;
		}
		this._RefreshText_Patterns();
	}

	// Token: 0x06000B2C RID: 2860 RVA: 0x00059D44 File Offset: 0x00057F44
	private void _RefreshText_Symbols()
	{
		GameplayData.SymbolsAvailable_OrderByBasicCoinsValue();
		this.availableSymbols = GameplayData.SymbolsAvailable_GetAll(true);
		this._sb.Clear();
		this._sb.Append(" - ");
		this._sb.Append(Translation.Get("SYMBOLS_POSTER_TITLE"));
		this._sb.Append(" - \n");
		foreach (SymbolScript.Kind kind in this.availableSymbols)
		{
			this._sb.Append(Strings.GetSpriteString_SlotMachineSymbol(kind));
			this._sb.Append("  ");
			int num = GameplayData.Symbol_CoinsValue_GetBasic(kind);
			this._symbolCoinsValue = GameplayData.Symbol_CoinsOverallValue_Get(kind);
			if (Master.instance.POSTER_TEXT_SCALES_WITH_VALUES)
			{
				this._sb.Append("<size=");
				this._sb.Append(this.FontSizePercGet_SymbolsValue(kind));
				this._sb.Append("%>");
			}
			this._sb.Append("<sprite name=\"CoinSymbolOrange64\">");
			this._sb.Append(": ");
			this._sb.Append(this.GetColorString_SymbolValue(num, this._symbolCoinsValue));
			this._sb.Append(this._symbolCoinsValue.ToStringSmart());
			this._sb.Append("</color>");
			this._sb.Append("   ");
			if (Master.instance.POSTER_TEXT_SCALES_WITH_VALUES)
			{
				this._sb.Append("<size=100%>");
			}
			float num2 = GameplayData.Symbol_Chance_Get(kind, true, true);
			float num3 = GameplayData.Symbol_Chance_GetBasic(kind);
			float num4 = GameplayData.Symbol_Chance_GetAsPercentage(kind, true, true);
			if (Master.instance.POSTER_TEXT_SCALES_WITH_VALUES)
			{
				this._sb.Append("<size=");
				this._sb.Append(this.FontSizePercGet_SymbolsChance(kind));
				this._sb.Append("%>");
			}
			this._sb.Append("<sprite name=\"Dice\">");
			this._sb.Append(":");
			this._sb.Append(this.GetColorString_SymbolChance(num3, num2));
			this._sb.Append(num4.ToString("0.0", CultureInfo.InvariantCulture));
			this._sb.Append("</color>");
			this._sb.Append("%");
			this._sb.Append("\n");
			if (Master.instance.POSTER_TEXT_SCALES_WITH_VALUES)
			{
				this._sb.Append("<size=100%>");
			}
			if (!this.unlockConditionPerformed_1000PlusSymbolValue && this._symbolCoinsValue >= 1000L)
			{
				this.unlockConditionPerformed_1000PlusSymbolValue = true;
				PowerupScript.Unlock(PowerupScript.Identifier.ExpiredMedicines);
				PowerupScript.Unlock(PowerupScript.Identifier.GoldenHand_MidasTouch);
				PowerupScript.Unlock(PowerupScript.Identifier.RingBell);
			}
		}
		this.coinsAllMult = GameplayData.AllSymbolsMultiplierGet(true);
		this._sb.Append("<size=0.05>\n</size>");
		this._sb.Append(Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("VARIOUS_POSTER_ALL_SYMBOLS_MULTIPLIER"), Strings.SanitizationSubKind.none));
		if (this.coinsAllMult != 1L)
		{
			this._sb.Append("<color=yellow>");
		}
		this._sb.Append(this.coinsAllMult.ToStringSmart());
		if (this.coinsAllMult != 1L)
		{
			this._sb.Append("</color>");
		}
		this.text.text = this._sb.ToString();
	}

	// Token: 0x06000B2D RID: 2861 RVA: 0x0005A0E8 File Offset: 0x000582E8
	private void _RefreshText_Patterns()
	{
		GameplayData.PatternsAvailable_OrderByValue(true);
		this.availablePatterns = GameplayData.PatternsAvailable_GetAll();
		this._sbPatterns.Clear();
		this._sbPatterns.Append(" - ");
		this._sbPatterns.Append(Translation.Get("PATTERNS_POSTER_TITLE"));
		this._sbPatterns.Append(" - \n");
		foreach (PatternScript.Kind kind in this.availablePatterns)
		{
			this._sbPatterns.Append(Strings.GetSpriteString_SlotMachinePattern(kind));
			this._sbPatterns.Append(PatternScript.GetPatterTranslatedName_Short(kind));
			this._sbPatterns.Append(" ");
			double num = GameplayData.Pattern_Value_GetBasic(kind);
			this._patternsValue = GameplayData.Pattern_ValueOverall_Get(kind, true);
			if (Master.instance.POSTER_TEXT_SCALES_WITH_VALUES)
			{
				this._sbPatterns.Append("<size=");
				this._sbPatterns.Append(this.FontSizePercGet_PatternsValue(kind));
				this._sbPatterns.Append("%>");
			}
			this._sbPatterns.Append("<sprite name=\"CoinSymbolOrange64\">");
			this._sbPatterns.Append("X");
			this._sbPatterns.Append(this.GetColorString_PatternValue(num, this._patternsValue));
			this._sbPatterns.Append(this._patternsValue.ToStringSmart("0.0", CultureInfo.InvariantCulture));
			this._sbPatterns.Append("</color>");
			this._sbPatterns.Append("\n");
			if (Master.instance.POSTER_TEXT_SCALES_WITH_VALUES)
			{
				this._sbPatterns.Append("<size=100%>");
			}
		}
		this.coinsAllMult = GameplayData.AllPatternsMultiplierGet(true);
		this._sbPatterns.Append("<size=0.075>\n</size>");
		this._sbPatterns.Append(Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("VARIOUS_POSTER_ALL_PATTERNS_MULTIPLIER"), Strings.SanitizationSubKind.none));
		if (this.coinsAllMult != 1L)
		{
			this._sbPatterns.Append("<color=yellow>");
		}
		this._sbPatterns.Append(this.coinsAllMult.ToStringSmart());
		if (this.coinsAllMult != 1L)
		{
			this._sbPatterns.Append("</color>");
		}
		this.text.text = this._sbPatterns.ToString();
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x0005A368 File Offset: 0x00058568
	private string GetColorString_SymbolValue(BigInteger baseValue, BigInteger currentValue)
	{
		if (baseValue == currentValue)
		{
			return "<color=white>";
		}
		if (currentValue < baseValue)
		{
			return "<color=red>";
		}
		BigInteger bigInteger = currentValue / baseValue;
		if (bigInteger < baseValue * 10)
		{
			return Colors.GetColorRichTextString("olive");
		}
		if (bigInteger < baseValue * 25)
		{
			return "<color=yellow>";
		}
		return "<color=orange>";
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x0005A3DC File Offset: 0x000585DC
	private string GetColorString_SymbolChance(float baseValue, float currentValue)
	{
		if (Mathf.Abs(baseValue - currentValue) < 0.1f)
		{
			return "<color=white>";
		}
		if (currentValue < baseValue)
		{
			return "<color=red>";
		}
		float num = currentValue / baseValue;
		if (num < baseValue * 5f)
		{
			return Colors.GetColorRichTextString("olive");
		}
		if (num < baseValue * 10f)
		{
			return "<color=yellow>";
		}
		return "<color=orange>";
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x0005A438 File Offset: 0x00058638
	private string GetColorString_PatternValue(double baseValue, double currentValue)
	{
		if (currentValue >= baseValue && currentValue < baseValue + 0.1)
		{
			return "<color=white>";
		}
		if (currentValue <= baseValue && currentValue > baseValue - 0.1)
		{
			return "<color=white>";
		}
		if (currentValue < baseValue)
		{
			return "<color=red>";
		}
		double num = currentValue / baseValue;
		if (num < baseValue * 10.0)
		{
			return Colors.GetColorRichTextString("olive");
		}
		if (num < baseValue * 25.0)
		{
			return "<color=yellow>";
		}
		return "<color=orange>";
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x0005A4B8 File Offset: 0x000586B8
	private float FontSizePercGet_SymbolsValue(SymbolScript.Kind symbolKind)
	{
		List<SymbolScript.Kind> list = GameplayData.SymbolsValueList_Get();
		if (list == null || list.Count == 0)
		{
			Debug.Log("PosterScript.FontSizePercGet..() : list not ready!");
			return 100f;
		}
		BigInteger bigInteger = GameplayData.Symbol_CoinsOverallValue_Get(list[list.Count - 1]);
		BigInteger bigInteger2 = GameplayData.Symbol_CoinsOverallValue_Get(list[0]) - bigInteger;
		BigInteger bigInteger3 = GameplayData.Symbol_CoinsOverallValue_Get(symbolKind);
		if (bigInteger2 <= 0L)
		{
			return 100f;
		}
		float num = (float)((bigInteger3 - bigInteger) * 100 / bigInteger2).CastToInt() / 100f;
		return (0.75f + 0.25f * num) * 100f;
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x0005A560 File Offset: 0x00058760
	private float FontSizePercGet_SymbolsChance(SymbolScript.Kind symbolKind)
	{
		List<SymbolScript.Kind> list = GameplayData.SymbolsChanceList_Get();
		if (list == null || list.Count == 0)
		{
			Debug.Log("PosterScript.FontSizePercGet..() : list not ready!");
			return 100f;
		}
		double num = (double)GameplayData.Symbol_Chance_Get(list[list.Count - 1], true, true);
		double num2 = (double)GameplayData.Symbol_Chance_Get(list[0], true, true) - num;
		float num3 = (float)((double)GameplayData.Symbol_Chance_Get(symbolKind, true, true));
		if (num2 < 0.0)
		{
			num2 = 0.0;
		}
		float num4 = (float)(((double)num3 - num) / num2);
		return (0.75f + 0.25f * num4) * 100f;
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x0005A5F0 File Offset: 0x000587F0
	private float FontSizePercGet_PatternsValue(PatternScript.Kind patterndKind)
	{
		List<PatternScript.Kind> list = GameplayData.PatternsValueList_Get();
		if (list == null || list.Count == 0)
		{
			Debug.Log("PosterScript.FontSizePercGet..() : list not ready!");
			return 100f;
		}
		double num = GameplayData.Pattern_ValueOverall_Get(list[list.Count - 1], true);
		double num2 = GameplayData.Pattern_ValueOverall_Get(list[0], true) - num;
		float num3 = (float)GameplayData.Pattern_ValueOverall_Get(patterndKind, true);
		if (num2 <= 0.0)
		{
			num2 = 1.0;
		}
		float num4 = (float)(((double)num3 - num) / num2);
		return (0.75f + 0.25f * num4) * 100f;
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x0000EE5F File Offset: 0x0000D05F
	public static void Initialize()
	{
		PosterScript.instance.RefreshText();
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x0000EE6B File Offset: 0x0000D06B
	private void Awake()
	{
		PosterScript.instance = this;
		this.refreshTimer = 0.25f;
		this.text.text = "";
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x0000EE8E File Offset: 0x0000D08E
	private void OnDestroy()
	{
		if (PosterScript.instance == this)
		{
			PosterScript.instance = null;
		}
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x0005A67C File Offset: 0x0005887C
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		this.refreshTimer -= Time.deltaTime;
		if (this.refreshTimer > 0f)
		{
			return;
		}
		this.refreshTimer += 1f;
		this.RefreshText();
	}

	// Token: 0x04000B8C RID: 2956
	public static PosterScript instance;

	// Token: 0x04000B8D RID: 2957
	private const float REFRESH_EVERY_SECONDS = 1f;

	// Token: 0x04000B8E RID: 2958
	public TextMeshProUGUI text;

	// Token: 0x04000B8F RID: 2959
	public PosterScript.PosterKind posterKind;

	// Token: 0x04000B90 RID: 2960
	private float refreshTimer;

	// Token: 0x04000B91 RID: 2961
	private BigInteger coinsAllMult;

	// Token: 0x04000B92 RID: 2962
	private List<SymbolScript.Kind> availableSymbols;

	// Token: 0x04000B93 RID: 2963
	private BigInteger _symbolCoinsValue = 0;

	// Token: 0x04000B94 RID: 2964
	private StringBuilder _sb = new StringBuilder(250);

	// Token: 0x04000B95 RID: 2965
	private List<PatternScript.Kind> availablePatterns;

	// Token: 0x04000B96 RID: 2966
	private double _patternsValue;

	// Token: 0x04000B97 RID: 2967
	private StringBuilder _sbPatterns = new StringBuilder(250);

	// Token: 0x04000B98 RID: 2968
	private bool unlockConditionPerformed_1000PlusSymbolValue;

	// Token: 0x04000B99 RID: 2969
	private const float FONT_RANGE_NORMALIZED = 0.25f;

	// Token: 0x020000D9 RID: 217
	public enum PosterKind
	{
		// Token: 0x04000B9B RID: 2971
		symbols,
		// Token: 0x04000B9C RID: 2972
		patterns
	}
}
