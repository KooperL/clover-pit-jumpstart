using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using Panik;
using TMPro;
using UnityEngine;

public class PosterScript : MonoBehaviour
{
	// Token: 0x0600099F RID: 2463 RVA: 0x0003FAC8 File Offset: 0x0003DCC8
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

	// Token: 0x060009A0 RID: 2464 RVA: 0x0003FAF4 File Offset: 0x0003DCF4
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

	// Token: 0x060009A1 RID: 2465 RVA: 0x0003FE98 File Offset: 0x0003E098
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

	// Token: 0x060009A2 RID: 2466 RVA: 0x00040118 File Offset: 0x0003E318
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

	// Token: 0x060009A3 RID: 2467 RVA: 0x0004018C File Offset: 0x0003E38C
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

	// Token: 0x060009A4 RID: 2468 RVA: 0x000401E8 File Offset: 0x0003E3E8
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

	// Token: 0x060009A5 RID: 2469 RVA: 0x00040268 File Offset: 0x0003E468
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

	// Token: 0x060009A6 RID: 2470 RVA: 0x00040310 File Offset: 0x0003E510
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

	// Token: 0x060009A7 RID: 2471 RVA: 0x000403A0 File Offset: 0x0003E5A0
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

	// Token: 0x060009A8 RID: 2472 RVA: 0x00040429 File Offset: 0x0003E629
	public static void Initialize()
	{
		PosterScript.instance.RefreshText();
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x00040435 File Offset: 0x0003E635
	private void Awake()
	{
		PosterScript.instance = this;
		this.refreshTimer = 0.25f;
		this.text.text = "";
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x00040458 File Offset: 0x0003E658
	private void OnDestroy()
	{
		if (PosterScript.instance == this)
		{
			PosterScript.instance = null;
		}
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x00040470 File Offset: 0x0003E670
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

	public static PosterScript instance;

	private const float REFRESH_EVERY_SECONDS = 1f;

	public TextMeshProUGUI text;

	public PosterScript.PosterKind posterKind;

	private float refreshTimer;

	private BigInteger coinsAllMult;

	private List<SymbolScript.Kind> availableSymbols;

	private BigInteger _symbolCoinsValue = 0;

	private StringBuilder _sb = new StringBuilder(250);

	private List<PatternScript.Kind> availablePatterns;

	private double _patternsValue;

	private StringBuilder _sbPatterns = new StringBuilder(250);

	private bool unlockConditionPerformed_1000PlusSymbolValue;

	private const float FONT_RANGE_NORMALIZED = 0.25f;

	public enum PosterKind
	{
		symbols,
		patterns
	}
}
