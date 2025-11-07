using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SlotMachineScript : MonoBehaviour
{
	// Token: 0x060006F3 RID: 1779 RVA: 0x0002C900 File Offset: 0x0002AB00
	private void ScoreSquareEnableSet(int columnX, int lineY, bool enable)
	{
		if (this.scoreSquares[columnX + lineY * 5].activeSelf != enable)
		{
			this.scoreSquares[columnX + lineY * 5].SetActive(enable);
		}
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x0002C928 File Offset: 0x0002AB28
	private bool ScoreSquareEnabledGet(int columnX, int lineY)
	{
		return this.scoreSquares[columnX + lineY * 5].activeSelf;
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x0002C93C File Offset: 0x0002AB3C
	private void ScoreSquareEnableSetAll(bool enable)
	{
		for (int i = 0; i < this.scoreSquares.Length; i++)
		{
			if (this.scoreSquares[i].activeSelf != enable)
			{
				this.scoreSquares[i].SetActive(enable);
			}
		}
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x0002C97C File Offset: 0x0002AB7C
	private void ReplacementSquaresInit()
	{
		this.replacementSquaresStartingAnchoredPosition = new Vector2[this.replacementSquaresRectTr.Length];
		for (int i = 0; i < this.replacementSquaresRectTr.Length; i++)
		{
			this.replacementSquaresStartingAnchoredPosition[i] = this.replacementSquaresRectTr[i].anchoredPosition;
			this.replacementSquaresRectTr[i].gameObject.SetActive(false);
		}
	}

	// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0002C9DB File Offset: 0x0002ABDB
	private Vector3 Audio3dPosition
	{
		get
		{
			return base.transform.position + this.audio3dOffset;
		}
	}

	// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0002C9F3 File Offset: 0x0002ABF3
	private Vector3 Audio3dPositionLow
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x0002CA00 File Offset: 0x0002AC00
	public static SlotMachineScript.State StateGet()
	{
		if (SlotMachineScript.instance == null)
		{
			return SlotMachineScript.State.off;
		}
		return SlotMachineScript.instance._state;
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x0002CA1B File Offset: 0x0002AC1B
	public static void StateSet(SlotMachineScript.State newState)
	{
		if (SlotMachineScript.instance == null)
		{
			return;
		}
		if (newState == SlotMachineScript.State.off)
		{
			Debug.LogError("SlotMachineScript.StateSet() - Cannot set state to off. Use TurnOff() instead.");
		}
		if (newState == SlotMachineScript.State.bootingUp)
		{
			Debug.LogError("SlotMachineScript.StateSet() - Cannot set state to bootingUp. Use TurnOn() instead.");
		}
		SlotMachineScript.instance._state = newState;
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x0002CA51 File Offset: 0x0002AC51
	public static bool IsTurnedOn()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance._state > SlotMachineScript.State.off;
	}

	// Token: 0x060006FC RID: 1788 RVA: 0x0002CA70 File Offset: 0x0002AC70
	public void AutoSpinToggle()
	{
		if (this._state != SlotMachineScript.State.idle && this._state != SlotMachineScript.State.spinning)
		{
			return;
		}
		this.autoSpin = !this.autoSpin;
		if (this.autoSpin)
		{
			Sound.Stop("SoundAutoToggleOn", true);
			Sound.Stop("SoundAutoToggleOff", true);
			Sound.Play("SoundAutoToggleOn", 1f, 1f);
		}
		else
		{
			Sound.Stop("SoundAutoToggleOn", true);
			Sound.Stop("SoundAutoToggleOff", true);
			Sound.Play("SoundAutoToggleOff", 1f, 1f);
		}
		this.AutoSpinTopTextSet();
	}

	// Token: 0x060006FD RID: 1789 RVA: 0x0002CB05 File Offset: 0x0002AD05
	public bool IsAutoSpinning()
	{
		return this.autoSpin;
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x0002CB0D File Offset: 0x0002AD0D
	private void AutoSpinTopTextSet()
	{
		if (this.autoSpin)
		{
			this.SetTopScreenText(Translation.Get("SLOT_TOP_SCREEN_AUTO_MODE"), false);
			return;
		}
		this.SetTopScreenText(Translation.Get("SLOT_TOP_SCREEN_MANUAL_MODE"), false);
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x0002CB3C File Offset: 0x0002AD3C
	public void TurnOn()
	{
		if (SlotMachineScript.IsTurnedOn())
		{
			return;
		}
		this._state = SlotMachineScript.State.bootingUp;
		this.autoSpin = false;
		GameplayData.RoundEarnedCoinsReset();
		this.SetTopScreenText(Translation.Get("SLOT_TOP_SCREEN_LETS_GO_GAMBLING"), false);
		this.onTopText_LoopsAround_Temp = (SlotMachineScript.Event)Delegate.Combine(this.onTopText_LoopsAround_Temp, new SlotMachineScript.Event(this.TopTextSet_666Or999_ChancesShow));
		Sound.Play3D("SoundSlotMachineStartupJingle", this.Audio3dPosition, 10f, 1f, 1f, 1);
		this.myMenuController.OpenMe();
		if (this.bootUpCoroutine == null)
		{
			this.bootUpCoroutine = base.StartCoroutine(this.BootUpCoroutine());
		}
		SlotMachineScript.Event onRoundBeing = this.OnRoundBeing;
		if (onRoundBeing == null)
		{
			return;
		}
		onRoundBeing();
	}

	// Token: 0x06000700 RID: 1792 RVA: 0x0002CBF0 File Offset: 0x0002ADF0
	public void TurnOff(bool forceTurnOff)
	{
		if (!SlotMachineScript.IsTurnedOn() && !forceTurnOff)
		{
			return;
		}
		object obj = !forceTurnOff && (this._state == SlotMachineScript.State.idle || this._state == SlotMachineScript.State.spinning || this._state == SlotMachineScript.State.noMoreCoins);
		this._state = SlotMachineScript.State.off;
		this.autoSpin = false;
		this.StopSpin();
		this.Stop_NoMoreSpins();
		this.TopTextSet_BetCost(true);
		object obj2 = obj;
		if (obj2 != null)
		{
			GameplayData.ExtraLuck_ResetAll();
		}
		if (obj2 != null)
		{
			Sound.Play3D("SoundSlotMachineTurnOff", this.Audio3dPosition, 10f, 1f, 1f, 1);
		}
		if (obj2 != null)
		{
			MemoScript.SetMessage(MemoScript.Message.roundsLeft, 1.5f);
		}
		if (obj2 != null)
		{
			this.myMenuController.Back();
		}
	}

	// Token: 0x06000701 RID: 1793 RVA: 0x0002CC94 File Offset: 0x0002AE94
	private static bool HasGoldenKnob()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance.hasGoldenKnob;
	}

	// Token: 0x06000702 RID: 1794 RVA: 0x0002CCAF File Offset: 0x0002AEAF
	public static void SpinExtraCoinsAdd(BigInteger coins)
	{
		if (SlotMachineScript.instance == null)
		{
			return;
		}
		SlotMachineScript.instance.spinExtraCoins += coins;
	}

	// Token: 0x06000703 RID: 1795 RVA: 0x0002CCD5 File Offset: 0x0002AED5
	public static bool IsReplacingSymbols()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance.replaceVisibleSymbolsCallN > 0;
	}

	// Token: 0x06000704 RID: 1796 RVA: 0x0002CCF4 File Offset: 0x0002AEF4
	public static void Symbol_ReplaceVisible(SymbolScript.Kind newKind, SymbolScript.Modifier modifier, int columnX, int lineY, bool pickRandomModifier)
	{
		if (SlotMachineScript.instance == null)
		{
			Debug.LogError("SlotMachineScript.Symbol_ReplaceVisible() - slotmachine instance is null!");
			return;
		}
		if (!SlotMachineScript.instance._legalToReplaceSymbols)
		{
			string text = "SlotMachineScript.Symbol_ReplaceVisible() - It is not legal to replace symbols right now!";
			Debug.LogError(text);
			ConsolePrompt.LogError(text, "", 0f);
			return;
		}
		SlotMachineScript.instance.lines[lineY][columnX] = newKind;
		SlotMachineScript.instance.StartCoroutine(SlotMachineScript.instance._SymbolReplacementAnimationCoroutine(newKind, modifier, columnX, lineY, pickRandomModifier));
	}

	// Token: 0x06000705 RID: 1797 RVA: 0x0002CD6A File Offset: 0x0002AF6A
	private IEnumerator _SymbolReplacementAnimationCoroutine(SymbolScript.Kind newKind, SymbolScript.Modifier modifier, int columnX, int lineY, bool pickRandomModifier)
	{
		this.replaceVisibleSymbolsCallN++;
		this.replacementSquaresRectTr[columnX + lineY * 5].gameObject.SetActive(true);
		if (!Sound.IsPlaying("SoundSlotMachineSymbolReplacement"))
		{
			Sound.Play("SoundSlotMachineSymbolReplacement", 1f, 1f);
		}
		Pool.Destroy(this.Symbol_GetInstanceAtPosition(columnX, lineY).gameObject, null);
		while (Sound.IsPlaying("SoundSlotMachineSymbolReplacement"))
		{
			Vector2 vector = this.replacementSquaresStartingAnchoredPosition[columnX + lineY * 5];
			this.replacementSquaresRectTr[columnX + lineY * 5].anchoredPosition = vector + new Vector2(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f));
			yield return null;
		}
		this.Symbol_SpawnInstance(true, newKind, modifier, columnX, lineY, pickRandomModifier);
		this.replacementSquaresRectTr[columnX + lineY * 5].gameObject.SetActive(false);
		float timer = 0.1f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		this.replaceVisibleSymbolsCallN--;
		yield break;
	}

	// Token: 0x06000706 RID: 1798 RVA: 0x0002CDA0 File Offset: 0x0002AFA0
	public static void Symbol_ReplaceAllVisible(SymbolScript.Kind kind, SymbolScript.Modifier modifier, bool pickRandomModifier)
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				SlotMachineScript.Symbol_ReplaceVisible(kind, modifier, j, i, pickRandomModifier);
			}
		}
	}

	// Token: 0x06000707 RID: 1799 RVA: 0x0002CDD0 File Offset: 0x0002AFD0
	public static void Symbol_ReplaceAllVisibleSymbols(SymbolScript.Kind kindToReplace, SymbolScript.Kind newKind, SymbolScript.Modifier modifier, bool pickRandomModifier)
	{
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (SlotMachineScript.instance.lines[i][j] == kindToReplace)
				{
					SlotMachineScript.Symbol_ReplaceVisible(newKind, modifier, j, i, pickRandomModifier);
				}
			}
		}
	}

	// Token: 0x06000708 RID: 1800 RVA: 0x0002CE10 File Offset: 0x0002B010
	public static int SymbolsCount(SymbolScript.Kind kind)
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (SlotMachineScript.instance.lines[i][j] == kind)
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06000709 RID: 1801 RVA: 0x0002CE4D File Offset: 0x0002B04D
	public static bool IsFirstSpinOfRound()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance._isFirstSpinOfRound;
	}

	// Token: 0x0600070A RID: 1802 RVA: 0x0002CE68 File Offset: 0x0002B068
	public static void FirstSpinFlagReset_ToTrue()
	{
		if (SlotMachineScript.instance == null)
		{
			return;
		}
		SlotMachineScript.instance._isFirstSpinOfRound = true;
	}

	// Token: 0x0600070B RID: 1803 RVA: 0x0002CE83 File Offset: 0x0002B083
	public static bool IsSpinning()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance._state == SlotMachineScript.State.spinning;
	}

	// Token: 0x0600070C RID: 1804 RVA: 0x0002CEA1 File Offset: 0x0002B0A1
	public static bool IsSpinningBeforeCoinsReward()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance._spinningBeforeCoinsReward;
	}

	// Token: 0x0600070D RID: 1805 RVA: 0x0002CEBC File Offset: 0x0002B0BC
	public static bool Has666()
	{
		return !(SlotMachineScript.instance == null) && (SlotMachineScript.instance.lines[1][1] == SymbolScript.Kind.six && SlotMachineScript.instance.lines[1][2] == SymbolScript.Kind.six) && SlotMachineScript.instance.lines[1][3] == SymbolScript.Kind.six;
	}

	// Token: 0x0600070E RID: 1806 RVA: 0x0002CF0D File Offset: 0x0002B10D
	public static void RemoveVisible666()
	{
		SlotMachineScript.Symbol_ReplaceVisible(GameplayData.Symbol_GetRandom_BasedOnSymbolChance(), SymbolScript.Modifier.none, 1, 1, true);
		SlotMachineScript.Symbol_ReplaceVisible(GameplayData.Symbol_GetRandom_BasedOnSymbolChance(), SymbolScript.Modifier.none, 2, 1, true);
		SlotMachineScript.Symbol_ReplaceVisible(GameplayData.Symbol_GetRandom_BasedOnSymbolChance(), SymbolScript.Modifier.none, 3, 1, true);
	}

	// Token: 0x0600070F RID: 1807 RVA: 0x0002CF39 File Offset: 0x0002B139
	public static bool HasShown666()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance._hasShown666;
	}

	// Token: 0x06000710 RID: 1808 RVA: 0x0002CF54 File Offset: 0x0002B154
	public void _666RoundLostCoinsReset()
	{
		this._666RoundLostCoins = 0;
	}

	// Token: 0x06000711 RID: 1809 RVA: 0x0002CF62 File Offset: 0x0002B162
	private void _666RoundLostCoinsAdd(BigInteger n)
	{
		if (n < 0L)
		{
			n = -n;
		}
		this._666RoundLostCoins += n;
	}

	// Token: 0x06000712 RID: 1810 RVA: 0x0002CF88 File Offset: 0x0002B188
	private BigInteger _666RoundLostCoinsGet()
	{
		return this._666RoundLostCoins;
	}

	// Token: 0x06000713 RID: 1811 RVA: 0x0002CF90 File Offset: 0x0002B190
	public static bool Has999()
	{
		return !(SlotMachineScript.instance == null) && (SlotMachineScript.instance.lines[1][1] == SymbolScript.Kind.nine && SlotMachineScript.instance.lines[1][2] == SymbolScript.Kind.nine) && SlotMachineScript.instance.lines[1][3] == SymbolScript.Kind.nine;
	}

	// Token: 0x06000714 RID: 1812 RVA: 0x0002CFE1 File Offset: 0x0002B1E1
	public static bool HasShown999()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance._hasTransformedInto999;
	}

	// Token: 0x06000715 RID: 1813 RVA: 0x0002CFFC File Offset: 0x0002B1FC
	public static bool HasJackpot()
	{
		if (SlotMachineScript.instance == null)
		{
			return false;
		}
		SymbolScript.Kind kind = SlotMachineScript.instance.lines[0][0];
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (SlotMachineScript.instance.lines[i][j] != kind)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06000716 RID: 1814 RVA: 0x0002D053 File Offset: 0x0002B253
	public void ForceNextLuck_Set(int luck)
	{
		this.forcedLuckNext = luck;
	}

	// Token: 0x06000717 RID: 1815 RVA: 0x0002D05C File Offset: 0x0002B25C
	public void ForceNextLuck_Add(int luck)
	{
		this.forcedLuckNext += luck;
	}

	// Token: 0x06000718 RID: 1816 RVA: 0x0002D06C File Offset: 0x0002B26C
	public void ForceNextLuck_Reset()
	{
		this.forcedLuckNext = 0;
	}

	// Token: 0x06000719 RID: 1817 RVA: 0x0002D075 File Offset: 0x0002B275
	public static bool IsAllSamePattern()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance._isAllSamePattern;
	}

	// Token: 0x0600071A RID: 1818 RVA: 0x0002D090 File Offset: 0x0002B290
	public static PatternScript.Kind GetBiggestPatternScored()
	{
		if (SlotMachineScript.instance == null)
		{
			return PatternScript.Kind.undefined;
		}
		return SlotMachineScript.instance._biggestPatternScored;
	}

	// Token: 0x0600071B RID: 1819 RVA: 0x0002D0AB File Offset: 0x0002B2AB
	public SymbolScript.Kind Symbol_GetAtPosition(int columnX, int lineY)
	{
		return this.lines[lineY][columnX];
	}

	// Token: 0x0600071C RID: 1820 RVA: 0x0002D0B7 File Offset: 0x0002B2B7
	public SymbolScript Symbol_GetInstanceAtPosition(int columnX, int lineY)
	{
		return SymbolScript.GetSymbolScript_ByScoringPosition(new Vector2Int(columnX, lineY));
	}

	// Token: 0x0600071D RID: 1821 RVA: 0x0002D0C8 File Offset: 0x0002B2C8
	public void Spin()
	{
		if (this._state != SlotMachineScript.State.idle)
		{
			return;
		}
		this._state = SlotMachineScript.State.spinning;
		this.TopTextSet_666Or999_ChancesShow();
		GameplayData.SpinsDoneInARun_Increment();
		GameplayMaster.SpinsDoneSinceStartup_Increment();
		this.SpinWinText_StopIfAny();
		this.bounceScript.SetBounceScale(0.015f);
		this.myMenuController.HoveredElement = null;
		this.leverMenuElement.RefreshHovering(false);
		this.spinCoroutine = base.StartCoroutine(this._SpinCoroutine());
		if (this.spinFailsafeCoroutine != null)
		{
			base.StopCoroutine(this.spinFailsafeCoroutine);
		}
		this.spinFailsafeCoroutine = base.StartCoroutine(this._SpinFailsafeCoroutine());
		Controls.VibrationSet_PreferMax(this.player, 0.5f);
	}

	// Token: 0x0600071E RID: 1822 RVA: 0x0002D16C File Offset: 0x0002B36C
	public void StopSpin()
	{
		if (this.spinCoroutine != null)
		{
			base.StopCoroutine(this.spinCoroutine);
			this.spinCoroutine = null;
			if (CameraController.GetPositionKind() == CameraController.PositionKind.SlotCoinPlate_Fixed)
			{
				CameraController.SetPosition(CameraController.PositionKind.Slot_Fixed, true, 1f);
			}
		}
		Sound.Stop("SoundSlotMachineRollingTick", true);
		for (int i = this.scoreTextsActive.Count - 1; i >= 0; i--)
		{
			this.ScoreTextDestroy(this.scoreTextsActive[i]);
		}
		this.ScoreSquareEnableSetAll(false);
		if (this.spinFailsafeCoroutine != null)
		{
			base.StopCoroutine(this.spinFailsafeCoroutine);
			this.spinFailsafeCoroutine = null;
		}
	}

	// Token: 0x0600071F RID: 1823 RVA: 0x0002D1FF File Offset: 0x0002B3FF
	private IEnumerator _SpinFailsafeCoroutine()
	{
		this.failSafeTimer = 40f;
		while (this.failSafeTimer > 0f)
		{
			if (!SlotMachineScript.IsSpinning())
			{
				IL_0104:
				this.spinFailsafeCoroutine = null;
				yield break;
			}
			this.failSafeTimer -= Tick.Time;
			yield return null;
		}
		if (SlotMachineScript.IsSpinning())
		{
			Debug.LogError("SPIN ERROR! Spin took too long! Restarting... (you'll keep the run data)");
			if (PlatformMaster.PlatformIsComputer())
			{
				ConsolePrompt.LogError("SPIN ERROR! Spin took too long! Restarting... (you'll keep the run data)", "", 0f);
			}
			this.failSafeTimer = 5f;
			while (this.failSafeTimer > 0f)
			{
				this.failSafeTimer = Mathf.Min(this.failSafeTimer, 5f);
				this.failSafeTimer -= Tick.Time;
				yield return null;
			}
			Level.Restart(true);
			goto IL_0104;
		}
		goto IL_0104;
	}

	// Token: 0x06000720 RID: 1824 RVA: 0x0002D20E File Offset: 0x0002B40E
	private void SpinFailsafeRestartTimer()
	{
		this.failSafeTimer = 40f;
	}

	// Token: 0x06000721 RID: 1825 RVA: 0x0002D21B File Offset: 0x0002B41B
	private IEnumerator _SpinCoroutine()
	{
		this.spinExtraCoins = 0;
		this._spinningBeforeCoinsReward = true;
		this._hasShown666 = false;
		this._hasTransformedInto999 = false;
		this.confettiHolder.SetActive(false);
		SlotMachineScript.Event onSpinPreLuckApplication = this.OnSpinPreLuckApplication;
		if (onSpinPreLuckApplication != null)
		{
			onSpinPreLuckApplication();
		}
		yield return this.WaitForTriggerAnimation();
		this.TopTextSet_666Or999_ChancesShow();
		List<SymbolScript.Kind> symbolsAvailable = GameplayData.SymbolsAvailable_GetAll(true);
		BigInteger debtIndex = GameplayData.DebtIndexGet();
		int num = GameplayData.SpinsDoneInARun_Get();
		int spinSinceStartupN = GameplayMaster.SpinsDoneSinceStartup_Get();
		int num2 = Mathf.RoundToInt(GameplayData.LuckGet());
		int num3 = num2;
		int columnsN = 5;
		int linesN = 3;
		float timer = 0f;
		int _666Count = GameplayData.SixSixSix_GetSpinCount(true);
		bool lastWheelIsSlow = false;
		int jackpotsPerformed = 0;
		if (GameplayData.SixSixSix_BookedSpinGet() == GameplayData.SpinsLeftGet())
		{
			_666Count = 3;
			GameplayData.SixSixSix_BookedSpinSet(-1);
		}
		bool isJackpot = false;
		BigInteger coinsReward = 0;
		bool flag = false;
		bool flag2 = PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Depression);
		if (num > 1 && !flag2)
		{
			if (debtIndex == 0L)
			{
				int num4 = 0;
				if (GameplayData.SlotInitialLuckRndOffset < 0)
				{
					GameplayData.SlotInitialLuckRndOffset = R.Rng_SlotMachineLuck.Range(0, 5);
				}
				int num5 = 4 + Mathf.FloorToInt((float)(num / 6));
				if ((num + GameplayData.SlotInitialLuckRndOffset + 1) % num5 == 0)
				{
					num4 = R.Rng_SlotMachineLuck.Choose<int>(new int[] { 4, 5, 6, 6, 7, 8 });
					flag = true;
				}
				num3 += Mathf.RoundToInt((float)num4);
			}
			else
			{
				if (GameplayData.SlotOccasionalLuckSpinN < 0)
				{
					GameplayData.SlotOccasionalLuckSpinN = num + R.Rng_SlotMachineLuck.Range(0, 5);
				}
				if (GameplayData.SlotOccasionalLuckSpinN <= num)
				{
					if (num % 7 == 0)
					{
						num3 += R.Rng_SlotMachineLuck.Choose<int>(new int[] { 7, 8, 9 });
					}
					if (num % 3 == 0)
					{
						num3 += R.Rng_SlotMachineLuck.Choose<int>(new int[] { 5, 6, 7 });
					}
					else
					{
						num3 += R.Rng_SlotMachineLuck.Choose<int>(new int[] { 3, 4, 5 });
					}
					flag = true;
					int num6 = debtIndex.CastToInt();
					GameplayData.SlotOccasionalLuckSpinN = num + 4 + R.Rng_SlotMachineLuck.Range(0, 2) + Mathf.Max(0, num6);
				}
			}
		}
		if (!flag2)
		{
			int num7 = GameplayData.SpinsWithoutReward_Get();
			if (num7 > 3)
			{
				num3 += num7 + 1;
				flag = true;
			}
		}
		num3 += this.forcedLuckNext;
		this.forcedLuckNext = 0;
		if (num2 > 0)
		{
			if (num3 <= 4)
			{
				num3 += R.Rng_SlotMachineLuck.Choose<int>(new int[] { 0, 0, 1, 2 });
			}
			else if (num3 <= 6)
			{
				if (R.Rng_SlotMachineLuck.Value < 0.75f)
				{
					num3 += R.Rng_SlotMachineLuck.Choose<int>(new int[] { 0, 0, 1 });
				}
			}
			else if (num3 == 7 && R.Rng_SlotMachineLuck.Value <= 0.5f)
			{
				num3 += R.Rng_SlotMachineLuck.Choose<int>(new int[] { 0, 0, 1 });
			}
		}
		if (flag)
		{
			SlotMachineScript.EffectPlay_LeverSparks();
		}
		GameplayData.ExtraLuck_TickDownAll();
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				this.linesOld[i][j] = this.lines[i][j];
			}
		}
		for (int k = 0; k < 3; k++)
		{
			for (int l = 0; l < 5; l++)
			{
				this.lines[k][l] = GameplayData.Symbol_GetRandom_BasedOnSymbolChance();
			}
		}
		this._luckPositions.Clear();
		for (int m = 0; m < 3; m++)
		{
			for (int n = 0; n < 5; n++)
			{
				this._luckPositions.Add(new Vector2Int(n, m));
			}
		}
		for (int num8 = 0; num8 < this._luckPositions.Count; num8++)
		{
			int num9 = R.Rng_SlotMachineLuck.Range(num8, this._luckPositions.Count);
			Vector2Int vector2Int = this._luckPositions[num8];
			this._luckPositions[num8] = this._luckPositions[num9];
			this._luckPositions[num9] = vector2Int;
		}
		int num10 = Mathf.RoundToInt((float)num3);
		SymbolScript.Kind kind = GameplayData.Symbol_GetRandom_BasedOnSymbolChance();
		for (int num11 = this._luckPositions.Count - 1; num11 >= 0; num11--)
		{
			Vector2Int vector2Int2 = this._luckPositions[num11];
			if (num10 > 0)
			{
				this.lines[vector2Int2.y][vector2Int2.x] = kind;
				num10--;
			}
		}
		switch (_666Count)
		{
		case 0:
			break;
		case 1:
			this.lines[1][R.Rng_666.Choose<int>(new int[] { 1, 2, 3 })] = SymbolScript.Kind.six;
			break;
		case 2:
			this.lines[1][1] = SymbolScript.Kind.six;
			this.lines[1][2] = SymbolScript.Kind.six;
			break;
		case 3:
			this.lines[1][1] = SymbolScript.Kind.six;
			this.lines[1][2] = SymbolScript.Kind.six;
			this.lines[1][3] = SymbolScript.Kind.six;
			break;
		default:
			Debug.LogError("SlotMachineScript.SpinCoroutine() - _666Count is not 0, 1, 2 or 3! _666Count: " + _666Count.ToString());
			break;
		}
		if (SlotMachineScript.Has666())
		{
			this._hasShown666 = true;
		}
		SlotMachineScript.Event onSpinStart = this.OnSpinStart;
		if (onSpinStart != null)
		{
			onSpinStart();
		}
		yield return this.WaitForTriggerAnimation();
		this.TopTextSet_666Or999_ChancesShow();
		this.leverButtonVisualizer.Press();
		Sound.Play("SoundSlotLever", 1f, Random.Range(0.9f, 1.1f));
		bool flag3 = false;
		int num12 = 0;
		SymbolScript.Kind kind2 = this.Symbol_GetAtPosition(0, num12);
		if (kind2 == this.Symbol_GetAtPosition(1, num12) && kind2 == this.Symbol_GetAtPosition(2, num12) && kind2 == this.Symbol_GetAtPosition(3, num12))
		{
			flag3 = true;
		}
		num12 = 1;
		kind2 = this.Symbol_GetAtPosition(0, num12);
		if (kind2 == this.Symbol_GetAtPosition(1, num12) && kind2 == this.Symbol_GetAtPosition(2, num12) && kind2 == this.Symbol_GetAtPosition(3, num12))
		{
			flag3 = true;
		}
		num12 = 2;
		kind2 = this.Symbol_GetAtPosition(0, num12);
		if (kind2 == this.Symbol_GetAtPosition(1, num12) && kind2 == this.Symbol_GetAtPosition(2, num12) && kind2 == this.Symbol_GetAtPosition(3, num12))
		{
			flag3 = true;
		}
		if (!flag3)
		{
			kind2 = this.Symbol_GetAtPosition(0, 2);
			if (kind2 == this.Symbol_GetAtPosition(1, 1) && kind2 == this.Symbol_GetAtPosition(2, 0) && kind2 == this.Symbol_GetAtPosition(3, 1))
			{
				flag3 = true;
			}
			kind2 = this.Symbol_GetAtPosition(0, 0);
			if (kind2 == this.Symbol_GetAtPosition(1, 1) && kind2 == this.Symbol_GetAtPosition(2, 2) && kind2 == this.Symbol_GetAtPosition(3, 1))
			{
				flag3 = true;
			}
			if (!flag3)
			{
				bool flag4 = false;
				kind2 = this.Symbol_GetAtPosition(0, 0);
				if (kind2 == this.Symbol_GetAtPosition(0, 1) && kind2 == this.Symbol_GetAtPosition(0, 2) && kind2 == this.Symbol_GetAtPosition(2, 0) && kind2 == this.Symbol_GetAtPosition(2, 1) && kind2 == this.Symbol_GetAtPosition(2, 2))
				{
					flag4 = true;
				}
				if (flag4 && ((kind2 == this.Symbol_GetAtPosition(1, 2) && kind2 == this.Symbol_GetAtPosition(3, 0)) || (kind2 == this.Symbol_GetAtPosition(1, 0) && kind2 == this.Symbol_GetAtPosition(3, 2))))
				{
					flag3 = true;
				}
				if (!flag3)
				{
					kind2 = this.Symbol_GetAtPosition(0, 1);
					if (kind2 == this.Symbol_GetAtPosition(1, 0) && kind2 == this.Symbol_GetAtPosition(1, 1) && kind2 == this.Symbol_GetAtPosition(1, 2) && kind2 == this.Symbol_GetAtPosition(2, 0) && kind2 == this.Symbol_GetAtPosition(2, 2) && kind2 == this.Symbol_GetAtPosition(3, 0) && kind2 == this.Symbol_GetAtPosition(3, 1) && kind2 == this.Symbol_GetAtPosition(3, 2))
					{
						flag3 = true;
					}
				}
			}
		}
		if (flag3)
		{
			lastWheelIsSlow = true;
		}
		Sound.Play3D("SoundSlotMachineRollingTick", this.Audio3dPosition, 10f, 1f, Random.Range(0.95f, 1.05f), 1);
		Sound.Play3D("SoundSlotMachineFanfare", this.Audio3dPosition, 10f, Mathf.Min(0.75f, (float)(GameplayData.SpinsLeftGet() + 1) * 0.075f), 1f, 1);
		int symbolsPerColumn = this._SymbolsSpawn(false, lastWheelIsSlow);
		for (int num13 = 0; num13 < this.spinOffsetPerColumn.Length; num13++)
		{
			this.spinOffsetPerColumn[num13] = 0f;
		}
		bool[] taTlakSoundPlayed = new bool[columnsN];
		for (int num14 = 0; num14 < taTlakSoundPlayed.Length; num14++)
		{
			taTlakSoundPlayed[num14] = false;
		}
		for (int num15 = 0; num15 < SymbolScript.allEnabled.Count; num15++)
		{
			SymbolScript.allEnabled[num15].SpinScalingSet(true);
		}
		float overTheGameSpeedMult = 1.5f + (float)Mathf.Max(0, spinSinceStartupN) * 0.125f;
		overTheGameSpeedMult = Mathf.Min(overTheGameSpeedMult, 1.75f);
		float _666SpinAnimSpeedMult_CLAMPER = 1.75f;
		if (_666Count > 1)
		{
			_666SpinAnimSpeedMult_CLAMPER = 1f;
		}
		if (_666Count == 3)
		{
			overTheGameSpeedMult = 1f;
		}
		float kaTlakVol = 1f - (overTheGameSpeedMult - 1f) * 0.5f;
		while (this.spinOffsetPerColumn[columnsN - 1] < 1f)
		{
			for (int num16 = 0; num16 < columnsN; num16++)
			{
				RectTransform rectTransform = this.columnsRectTr[num16];
				float num17 = 1f - 0.1f * (float)num16;
				float num18 = 1f;
				float num19 = Mathf.Min(overTheGameSpeedMult, _666SpinAnimSpeedMult_CLAMPER);
				if (lastWheelIsSlow && num16 == columnsN - 1)
				{
					num18 = 0.6666f;
				}
				if (this.spinOffsetPerColumn[num16] <= 0.05f)
				{
					num17 = 1f;
				}
				this.spinOffsetPerColumn[num16] += Tick.Time * num17 * 0.5f * num18 * num19;
				if (!taTlakSoundPlayed[num16] && this.spinOffsetPerColumn[num16] >= 0.95f)
				{
					taTlakSoundPlayed[num16] = true;
					Sound.Play("SoundSlotMachineTaTlak", kaTlakVol, 1f + 0.05f * (float)num16);
					Controls.VibrationSet_PreferMax(this.player, 0.25f);
					bool flag5 = false;
					if (this.Symbol_GetAtPosition(num16, 1) == SymbolScript.Kind.six)
					{
						flag5 = true;
					}
					if (flag5)
					{
						FlashScreenSlot.Flash(Color.red, 1f, 4f);
						Sound.Play("SoundSlotMachineSymbolShowUp_6", 1f, 1f + 0.05f * (float)num16);
					}
				}
				if (this.spinOffsetPerColumn[num16] > 1f)
				{
					this.spinOffsetPerColumn[num16] = 1f;
				}
				float num20 = this.spinAnimationCurve.Evaluate(this.spinOffsetPerColumn[num16]) * (float)(symbolsPerColumn - linesN) * 0.5f;
				Vector2 anchoredPosition = rectTransform.anchoredPosition;
				rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, num20);
			}
			yield return null;
		}
		for (int num21 = 0; num21 < SymbolScript.allEnabled.Count; num21++)
		{
			SymbolScript.allEnabled[num21].SpinScalingSet(false);
		}
		Sound.Stop("SoundSlotMachineRollingTick", true);
		Sound.Stop("SoundSlotMachineFanfare", true);
		this._legalToReplaceSymbols = true;
		Delegate[] array = null;
		if (this.OnScoreEvaluationBegin != null)
		{
			array = this.OnScoreEvaluationBegin.GetInvocationList();
		}
		if (array != null && array.Length != 0)
		{
			foreach (SlotMachineScript.Event @event in array)
			{
				if (@event != null)
				{
					@event();
				}
				yield return this.WaitForTriggerAnimation();
				while (SlotMachineScript.IsReplacingSymbols())
				{
					yield return null;
				}
			}
			Delegate[] array2 = null;
		}
		yield return this.WaitForTriggerAnimation();
		while (SlotMachineScript.IsReplacingSymbols())
		{
			yield return null;
		}
		if (GameplayData.NineNineNine_IsTime())
		{
			if (SlotMachineScript.Has666())
			{
				this._hasTransformedInto999 = true;
			}
			SlotMachineScript.Symbol_ReplaceAllVisibleSymbols(SymbolScript.Kind.six, SymbolScript.Kind.nine, SymbolScript.Modifier.none, false);
			while (SlotMachineScript.IsReplacingSymbols())
			{
				yield return null;
			}
		}
		this.TopTextSet_666Or999_ChancesShow();
		this._legalToReplaceSymbols = false;
		this._666GotCoinsRestoredFromJackpot = false;
		yield return this.PatternsCompute_Coroutine();
		if (PowerupScript.IsEquipped_Quick(PowerupScript.Identifier.Painkillers))
		{
			List<SlotMachineScript.PatternInfos> patternsEnabled = this.GetPatternsEnabled();
			if (patternsEnabled.Count > 0 && SlotMachineScript.IsAllSamePattern() && !SlotMachineScript.Has666() && !SlotMachineScript.Has999())
			{
				PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Painkillers);
				this._legalToReplaceSymbols = true;
				SlotMachineScript.PatternInfos patternInfos2 = patternsEnabled[0];
				SymbolScript.Kind kind3 = GameplayData.MostValuableSymbols_GetList()[0];
				if (patternInfos2.symbolKind != kind3)
				{
					for (int num23 = 0; num23 < patternInfos2.positions.Count; num23++)
					{
						SlotMachineScript.Symbol_ReplaceVisible(kind3, SymbolScript.Modifier.none, patternInfos2.positions[num23].x, patternInfos2.positions[num23].y, true);
					}
					while (SlotMachineScript.IsReplacingSymbols())
					{
						yield return null;
					}
					this._legalToReplaceSymbols = false;
					isJackpot = false;
					yield return this.PatternsCompute_Coroutine();
				}
			}
		}
		int num25;
		if (PowerupScript.IsEquipped_Quick(PowerupScript.Identifier._999_Aureola) && (SlotMachineScript.GetPatternsCount() > 0 && SlotMachineScript.IsAllSamePattern()) && !SlotMachineScript.Has666() && !SlotMachineScript.Has999())
		{
			this._aureolaChangedPositions.Clear();
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier._999_Aureola);
			this._legalToReplaceSymbols = true;
			SlotMachineScript.PatternInfos patternInfos3 = this.GetPatternsEnabled()[0];
			for (int num24 = 0; num24 < patternInfos3.positions.Count; num24++)
			{
				Vector2Int vector2Int4;
				Vector2Int vector2Int3 = (vector2Int4 = patternInfos3.positions[num24]);
				num25 = vector2Int4.x;
				vector2Int4.x = num25 - 1;
				if (vector2Int4.x >= 0 && !SlotMachineScript.PatternContainsPosition(patternInfos3, vector2Int4) && !SlotMachineScript.ListContainsPosition(this._aureolaChangedPositions, vector2Int4))
				{
					SlotMachineScript.Symbol_ReplaceVisible(patternInfos3.symbolKind, SymbolScript.Modifier.none, vector2Int4.x, vector2Int4.y, true);
					this._aureolaChangedPositions.Add(vector2Int4);
				}
				vector2Int4 = vector2Int3;
				num25 = vector2Int4.x;
				vector2Int4.x = num25 + 1;
				if (vector2Int4.x < 5 && !SlotMachineScript.PatternContainsPosition(patternInfos3, vector2Int4) && !SlotMachineScript.ListContainsPosition(this._aureolaChangedPositions, vector2Int4))
				{
					SlotMachineScript.Symbol_ReplaceVisible(patternInfos3.symbolKind, SymbolScript.Modifier.none, vector2Int4.x, vector2Int4.y, true);
					this._aureolaChangedPositions.Add(vector2Int4);
				}
				vector2Int4 = vector2Int3;
				num25 = vector2Int4.y;
				vector2Int4.y = num25 - 1;
				if (vector2Int4.y >= 0 && !SlotMachineScript.PatternContainsPosition(patternInfos3, vector2Int4) && !SlotMachineScript.ListContainsPosition(this._aureolaChangedPositions, vector2Int4))
				{
					SlotMachineScript.Symbol_ReplaceVisible(patternInfos3.symbolKind, SymbolScript.Modifier.none, vector2Int4.x, vector2Int4.y, true);
					this._aureolaChangedPositions.Add(vector2Int4);
				}
				vector2Int4 = vector2Int3;
				num25 = vector2Int4.y;
				vector2Int4.y = num25 + 1;
				if (vector2Int4.y < 3 && !SlotMachineScript.PatternContainsPosition(patternInfos3, vector2Int4) && !SlotMachineScript.ListContainsPosition(this._aureolaChangedPositions, vector2Int4))
				{
					SlotMachineScript.Symbol_ReplaceVisible(patternInfos3.symbolKind, SymbolScript.Modifier.none, vector2Int4.x, vector2Int4.y, true);
					this._aureolaChangedPositions.Add(vector2Int4);
				}
			}
			while (SlotMachineScript.IsReplacingSymbols())
			{
				yield return null;
			}
			this._legalToReplaceSymbols = false;
			isJackpot = false;
			yield return this.PatternsCompute_Coroutine();
		}
		if (PowerupScript.DiscC_IsTriggeringTime())
		{
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.DiscC);
			this._legalToReplaceSymbols = true;
			SymbolScript.Kind kind4 = GameplayData.Symbol_GetRandom_BasedOnSymbolChance();
			SlotMachineScript.Symbol_ReplaceVisible(kind4, SymbolScript.Modifier.none, 0, 2, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind4, SymbolScript.Modifier.none, 1, 1, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind4, SymbolScript.Modifier.none, 2, 0, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind4, SymbolScript.Modifier.none, 3, 1, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind4, SymbolScript.Modifier.none, 4, 2, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind4, SymbolScript.Modifier.none, 0, 0, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind4, SymbolScript.Modifier.none, 2, 2, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind4, SymbolScript.Modifier.none, 4, 0, true);
			while (SlotMachineScript.IsReplacingSymbols())
			{
				yield return null;
			}
			this._legalToReplaceSymbols = false;
			isJackpot = false;
			yield return this.PatternsCompute_Coroutine();
		}
		if (PowerupScript.Nose_IsTriggerTime())
		{
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.Nose);
			this._legalToReplaceSymbols = true;
			SymbolScript.Kind kind5 = GameplayData.Symbol_GetRandom_BasedOnSymbolChance();
			if (!R.Rng_Powerup(PowerupScript.Identifier.Nose).FlipCoin)
			{
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 0, 2, true);
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 1, 1, true);
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 1, 2, true);
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 2, 0, true);
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 2, 2, true);
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 3, 1, true);
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 3, 2, true);
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 4, 2, true);
			}
			else
			{
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 0, 0, true);
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 1, 0, true);
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 1, 1, true);
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 2, 0, true);
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 2, 2, true);
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 3, 0, true);
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 3, 1, true);
				SlotMachineScript.Symbol_ReplaceVisible(kind5, SymbolScript.Modifier.none, 4, 0, true);
			}
			while (SlotMachineScript.IsReplacingSymbols())
			{
				yield return null;
			}
			this._legalToReplaceSymbols = false;
			isJackpot = false;
			yield return this.PatternsCompute_Coroutine();
		}
		if (PowerupScript.EyeJar_IsTriggerTime())
		{
			PowerupScript.PlayTriggeredAnimation(PowerupScript.Identifier.EyeJar);
			this._legalToReplaceSymbols = true;
			SymbolScript.Kind kind6 = GameplayData.Symbol_GetRandom_BasedOnSymbolChance();
			SlotMachineScript.Symbol_ReplaceVisible(kind6, SymbolScript.Modifier.none, 0, 1, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind6, SymbolScript.Modifier.none, 1, 0, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind6, SymbolScript.Modifier.none, 1, 1, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind6, SymbolScript.Modifier.none, 1, 2, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind6, SymbolScript.Modifier.none, 2, 0, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind6, SymbolScript.Modifier.none, 2, 2, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind6, SymbolScript.Modifier.none, 3, 0, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind6, SymbolScript.Modifier.none, 3, 1, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind6, SymbolScript.Modifier.none, 3, 2, true);
			SlotMachineScript.Symbol_ReplaceVisible(kind6, SymbolScript.Modifier.none, 4, 1, true);
			while (SlotMachineScript.IsReplacingSymbols())
			{
				yield return null;
			}
			this._legalToReplaceSymbols = false;
			isJackpot = false;
			yield return this.PatternsCompute_Coroutine();
		}
		if (PowerupScript.Dice4_TriggerTry())
		{
			this._legalToReplaceSymbols = true;
			PowerupScript._dice4UntriggeredPositions.Clear();
			for (int num26 = 0; num26 < 3; num26++)
			{
				for (int num27 = 0; num27 < 5; num27++)
				{
					PowerupScript._dice4UntriggeredPositions.Add(new Vector2Int(num27, num26));
				}
			}
			for (int num28 = 0; num28 < this._patternInfos.Count; num28++)
			{
				SlotMachineScript.PatternInfos patternInfos4 = this._patternInfos[num28];
				if (patternInfos4.enabled)
				{
					List<Vector2Int> positions2 = patternInfos4.positions;
					for (int num29 = 0; num29 < positions2.Count; num29++)
					{
						PowerupScript._dice4UntriggeredPositions.Remove(positions2[num29]);
					}
				}
			}
			for (int num30 = 0; num30 < PowerupScript._dice4UntriggeredPositions.Count; num30++)
			{
				Vector2Int vector2Int5 = PowerupScript._dice4UntriggeredPositions[num30];
				SlotMachineScript.Symbol_ReplaceVisible(GameplayData.Symbol_GetRandom_BasedOnSymbolChance(), SymbolScript.Modifier.none, vector2Int5.x, vector2Int5.y, true);
			}
			while (SlotMachineScript.IsReplacingSymbols())
			{
				yield return null;
			}
			this._legalToReplaceSymbols = false;
			isJackpot = false;
			yield return this.PatternsCompute_Coroutine();
		}
		this.PatternComputation_GetResults(out coinsReward);
		int patternsN = SlotMachineScript.GetPatternsCount();
		isJackpot = false;
		bool hasJackpot = false;
		hasJackpot = this.GetPatternsOfKind(PatternScript.Kind.jackpot).Count > 0;
		if (this._666GotCoinsRestoredFromJackpot)
		{
			this._666RoundLostCoinsReset();
		}
		if (debtIndex >= GameplayData.SixSixSix_GetMinimumDebtIndex() && hasJackpot && this._666GotCoinsRestoredFromJackpot)
		{
			Debug.Log("SHOULD TRIGGER");
			RunModifierScript.TriggerAnimation_IfEquipped(RunModifierScript.Identifier._666DoubleChances_JackpotRecovers);
		}
		yield return this.WaitForTriggerAnimation();
		float scoredSoundPitch = 1f;
		float againSoundPitch = 1f;
		SlotMachineScript.PatternInfos lastPatternScored = null;
		int patternsTriggerCounter = 0;
		int batteryChargesToApply = 0;
		int patternsWith5PlusSymbols_Count = 0;
		int patternsWithDiamondsTreasuresOrSevens_Count = 0;
		BigInteger coinsEarnedDuringIteration = 0;
		int unlockCounter_Jackpots = 0;
		int unlockCounter_JackpotsWithFruits = 0;
		int unlockCounter_JackpotsWithCloverBells = 0;
		int unlockCounter_JackpotsWithDiamondTreasuresSevens = 0;
		int unlockCounter_AboveTriggered = 0;
		int unlockCounter_BelowTriggered = 0;
		bool unlockFlag_Lost_1_MillionTo666 = false;
		bool unlockFlag_Lost_100_MillionsTo666 = false;
		List<SlotMachineScript.PatternInfos> patternInfosList = this.GetPatternsEnabled();
		for (int num22 = 0; num22 < patternInfosList.Count; num22 = num25 + 1)
		{
			SlotMachineScript.PatternInfos patternInfos = patternInfosList[num22];
			SlotMachineScript.SensationalLevel sensationalLevel = SlotMachineScript.GetPatternSensationalLevel(patternInfos.patternKind, patternInfos.symbolKind);
			bool is666 = patternInfos.symbolKind == SymbolScript.Kind.six && patternInfos.patternKind == PatternScript.Kind.horizontal3;
			bool is667 = patternInfos.symbolKind == SymbolScript.Kind.nine && patternInfos.patternKind == PatternScript.Kind.horizontal3;
			isJackpot = patternInfos.patternKind == PatternScript.Kind.jackpot;
			bool isRepeatingPattern = lastPatternScored != null && lastPatternScored.IsEqualToOtherPattern(patternInfos);
			bool isLastPattern = num22 == patternInfosList.Count - 1;
			List<Vector2Int> positions = patternInfos.positions;
			num25 = patternsTriggerCounter;
			patternsTriggerCounter = num25 + 1;
			if (isJackpot)
			{
				num25 = jackpotsPerformed;
				jackpotsPerformed = num25 + 1;
				GameplayData.JackpotsScoredCounter += 1L;
			}
			if (patternInfos.positions.Count >= 5)
			{
				num25 = patternsWith5PlusSymbols_Count;
				patternsWith5PlusSymbols_Count = num25 + 1;
			}
			if (patternInfos.symbolKind == SymbolScript.Kind.diamond || patternInfos.symbolKind == SymbolScript.Kind.coins || patternInfos.symbolKind == SymbolScript.Kind.seven)
			{
				num25 = patternsWithDiamondsTreasuresOrSevens_Count;
				patternsWithDiamondsTreasuresOrSevens_Count = num25 + 1;
			}
			if (isJackpot)
			{
				num25 = unlockCounter_Jackpots;
				unlockCounter_Jackpots = num25 + 1;
				switch (patternInfos.symbolKind)
				{
				case SymbolScript.Kind.lemon:
				case SymbolScript.Kind.cherry:
					num25 = unlockCounter_JackpotsWithFruits;
					unlockCounter_JackpotsWithFruits = num25 + 1;
					break;
				case SymbolScript.Kind.clover:
				case SymbolScript.Kind.bell:
					num25 = unlockCounter_JackpotsWithCloverBells;
					unlockCounter_JackpotsWithCloverBells = num25 + 1;
					break;
				case SymbolScript.Kind.diamond:
				case SymbolScript.Kind.coins:
				case SymbolScript.Kind.seven:
					num25 = unlockCounter_JackpotsWithDiamondTreasuresSevens;
					unlockCounter_JackpotsWithDiamondTreasuresSevens = num25 + 1;
					break;
				case SymbolScript.Kind.six:
				case SymbolScript.Kind.nine:
					break;
				default:
					Debug.LogError("SlotMachineScript._SpinCoroutine(): jackpots unlock counters: unhandled symbol: " + patternInfos.symbolKind.ToString());
					break;
				}
			}
			if (patternInfos.patternKind == PatternScript.Kind.triangle)
			{
				num25 = unlockCounter_AboveTriggered;
				unlockCounter_AboveTriggered = num25 + 1;
			}
			else if (patternInfos.patternKind == PatternScript.Kind.triangleInverted)
			{
				num25 = unlockCounter_BelowTriggered;
				unlockCounter_BelowTriggered = num25 + 1;
			}
			SlotMachineScript.PatternEvent onPatternEvaluationStart = this.OnPatternEvaluationStart;
			if (onPatternEvaluationStart != null)
			{
				onPatternEvaluationStart(patternInfos);
			}
			yield return this.WaitForTriggerAnimation();
			Sound.Stop("SoundSlotMachineJackpot", true);
			float num31 = 1f + (float)(jackpotsPerformed - 1) * 0.1f;
			if (isJackpot)
			{
				Sound.Play3D("SoundSlotMachineJackpot", this.Audio3dPosition, 10f, 1f, num31, 1);
				Sound.Play3D("SoundSlotMachineJackpotBell", this.Audio3dPosition, 10f, Mathf.Max(0f, 1f - (float)(jackpotsPerformed - 1) * 0.1f), 1f, 1);
			}
			else if (is666)
			{
				Sound.Play3D("SoundSlotMachineScored666", this.Audio3dPosition, 10f, 1f, 1f, 1);
			}
			else if (is667)
			{
				Sound.Play3D("SoundSlotMachineScored999", this.Audio3dPosition, 10f, 1f, 1f, 1);
			}
			else
			{
				if (hasJackpot)
				{
					Sound.Play3D("SoundSlotMachineScoredWithJackpot", this.Audio3dPosition, 10f, 1f, scoredSoundPitch, 1);
				}
				else
				{
					Sound.Play3D("SoundSlotMachineScored", this.Audio3dPosition, 10f, 1f, scoredSoundPitch, 1);
				}
				scoredSoundPitch = Mathf.Min(scoredSoundPitch + 0.1f, 1.6f);
			}
			if (isRepeatingPattern)
			{
				Sound.Play3D("SoundSlotMachineAgainScore", this.Audio3dPosition, 10f, 1f, againSoundPitch, 1);
				againSoundPitch = Mathf.Min(againSoundPitch + 0.1f, 1.6f);
			}
			else
			{
				againSoundPitch = 1f;
			}
			Controls.VibrationSet_PreferMax(this.player, 0.25f);
			if (hasJackpot)
			{
				if (isJackpot)
				{
					CameraGame.Shake(1f * scoredSoundPitch * scoredSoundPitch + 2.5f);
					CameraGame.ChromaticAberrationIntensitySet(2f);
					this.bounceScript.SetBounceScale(0.015f + 0.005f * (float)jackpotsPerformed);
					FlashScreen.SpawnCamera(Color.yellow, 0.5f, 2f, CameraGame.firstInstance.myCamera, 0.5f);
					Controls.VibrationSet_PreferMax(this.player, 1f);
					int num32 = 0;
					if (jackpotsPerformed >= 2)
					{
						num32++;
					}
					if (jackpotsPerformed >= 4)
					{
						num32++;
					}
					this.JackpotGlowShow(num32);
					base.StartCoroutine(this.JackpotGlowVibration());
					this.JackpotLightSet(Mathf.Min(10f, 1f + 1f * (float)jackpotsPerformed));
					this.JackpotGalaxySetLevel(jackpotsPerformed - 1);
				}
				else
				{
					CameraGame.Shake(1f * scoredSoundPitch * scoredSoundPitch);
					CameraGame.ChromaticAberrationIntensitySet(0.5f);
					this.bounceScript.SetBounceScale(0.0075f);
					Controls.VibrationSet_PreferMax(this.player, 0.5f * scoredSoundPitch);
				}
			}
			switch (patternInfos.patternKind)
			{
			case PatternScript.Kind.triangle:
				Sound.Play3D("SoundSpecialPattern_Light", this.Audio3dPosition, 10f, 1f, 1f, 1);
				SlotMachineScript.ShowSpecialPatternImage(PatternScript.Kind.triangle);
				break;
			case PatternScript.Kind.triangleInverted:
				Sound.Play3D("SoundSpecialPattern_Dark", this.Audio3dPosition, 10f, 1f, 1f, 1);
				SlotMachineScript.ShowSpecialPatternImage(PatternScript.Kind.triangleInverted);
				break;
			case PatternScript.Kind.eye:
				Sound.Play3D("SoundSpecialPattern_Mistery", this.Audio3dPosition, 10f, 1f, 1f, 1);
				SlotMachineScript.ShowSpecialPatternImage(PatternScript.Kind.eye);
				break;
			}
			if (is666)
			{
				FlashScreenSlot.Flash(Color.white, 10f, 0f);
				FlashScreenSlot.SetTexture(AssetMaster.GetTexture2D("TextureSlotMachineFire"), new Vector2(0f, -1f));
				if (debtIndex >= GameplayData.SuperSixSixSix_GetMinimumDebtIndex())
				{
					FlashScreenSlot.SetSecondTexture(AssetMaster.GetTexture2D("TextureSlotMachineFireSuper"), new Vector2(0f, -2f));
				}
				Controls.VibrationSet_PreferMax(this.player, 0.5f);
				SlotMachineScript.PatternEvent on = this.On666;
				if (on != null)
				{
					on(patternInfos);
				}
				yield return this.WaitForTriggerAnimation();
			}
			if (is667)
			{
				FlashScreenSlot.Flash(Color.white, 10f, 0f);
				FlashScreenSlot.SetTexture(AssetMaster.GetTexture2D("TextureSlotMachine999"), new Vector2(0f, -1f));
				Controls.VibrationSet_PreferMax(this.player, 0.5f);
				SlotMachineScript.PatternEvent on2 = this.On999;
				if (on2 != null)
				{
					on2(patternInfos);
				}
				yield return this.WaitForTriggerAnimation();
			}
			bool flag6 = false;
			BigInteger modifier_InstantReward = 0;
			int modifier_TicketBonus = 0;
			int modifier_GoldenBonus = 0;
			double modifier_Repetition = 0.0;
			double modifier_Battery = 0.0;
			double modifier_ChainBonus = 0.0;
			this._modifiersToAnimate_Temp.Clear();
			int totalModifiers_PerType = 0;
			int num33 = 0;
			while (num33 < patternInfos.positions.Count && !is666)
			{
				SymbolScript symbolScript_ByScoringPosition = SymbolScript.GetSymbolScript_ByScoringPosition(patternInfos.positions[num33]);
				SymbolScript.Kind symbolKind = patternInfos.symbolKind;
				if (!(symbolScript_ByScoringPosition == null))
				{
					SymbolScript.Modifier modifier = symbolScript_ByScoringPosition.ModifierGet();
					if (modifier != SymbolScript.Modifier.none && (modifier != SymbolScript.Modifier.repetition || lastPatternScored == null || !lastPatternScored.IsEqualToOtherPattern(patternInfos)))
					{
						flag6 = true;
						if (!this._modifiersToAnimate_Temp.Contains(modifier))
						{
							num25 = totalModifiers_PerType;
							totalModifiers_PerType = num25 + 1;
							this._modifiersToAnimate_Temp.Add(modifier);
						}
						switch (modifier)
						{
						case SymbolScript.Modifier.instantReward:
						{
							BigInteger bigInteger = SymbolScript.ModifierInstantReward_GetAmmount();
							if (bigInteger < 1L)
							{
								bigInteger = 1;
							}
							SlotMachineScript.SpinExtraCoinsAdd(bigInteger);
							modifier_InstantReward += bigInteger;
							break;
						}
						case SymbolScript.Modifier.cloverTicket:
						{
							int num34 = 1;
							GameplayData.CloverTicketsAdd((long)num34, true);
							modifier_TicketBonus += num34;
							break;
						}
						case SymbolScript.Modifier.golden:
						{
							int num35 = GameplayData.Symbol_CoinsValue_GetBasic(symbolKind);
							GameplayData.Symbol_CoinsValueExtra_Add(symbolKind, num35);
							modifier_GoldenBonus += num35;
							break;
						}
						case SymbolScript.Modifier.repetition:
						{
							double num36 = modifier_Repetition;
							modifier_Repetition = num36 + 1.0;
							break;
						}
						case SymbolScript.Modifier.battery:
						{
							num25 = batteryChargesToApply;
							batteryChargesToApply = num25 + 1;
							double num36 = modifier_Battery;
							modifier_Battery = num36 + 1.0;
							break;
						}
						case SymbolScript.Modifier.chain:
						{
							double num37 = GameplayData.Pattern_Value_GetBasic(patternInfos.patternKind);
							GameplayData.Pattern_ValueExtra_Add(patternInfos.patternKind, num37);
							modifier_ChainBonus += num37;
							break;
						}
						default:
							Debug.LogError("SlotMachineScript.SpinCoroutine() - Modifier not handled: " + modifier.ToString());
							break;
						}
						GameplayData.Stats_ModifiedSymbol_TriggeredTimesAdd();
						Data.GameData.Tracker_ModSymbolsCounter_Set(symbolKind, Data.GameData.Tracker_ModSymbolsCounter_Get(symbolKind) + 1L);
						switch (symbolKind)
						{
						case SymbolScript.Kind.lemon:
							GameplayData.Stats_ModifiedLemonTriggeredTimesAdd();
							break;
						case SymbolScript.Kind.cherry:
							GameplayData.Stats_ModifiedCherryTriggeredTimesAdd();
							break;
						case SymbolScript.Kind.clover:
							GameplayData.Stats_ModifiedCloverTriggeredTimesAdd();
							break;
						case SymbolScript.Kind.bell:
							GameplayData.Stats_ModifiedBellTriggeredTimesAdd();
							break;
						case SymbolScript.Kind.diamond:
							GameplayData.Stats_ModifiedDiamondTriggeredTimesAdd();
							break;
						case SymbolScript.Kind.coins:
							GameplayData.Stats_ModifiedCoinsTriggeredTimesAdd();
							break;
						case SymbolScript.Kind.seven:
							GameplayData.Stats_ModifiedSevenTriggeredTimesAdd();
							break;
						case SymbolScript.Kind.six:
						case SymbolScript.Kind.nine:
							break;
						default:
							Debug.LogError("SlotMachineScript.SpinCoroutine() - Some symbol is not handled, and the modifier is not applied! Symbol: " + symbolKind.ToString());
							break;
						}
					}
				}
				num33++;
			}
			if (flag6)
			{
				SlotMachineScript.PatternEvent onModifierScored = this.OnModifierScored;
				if (onModifierScored != null)
				{
					onModifierScored(patternInfos);
				}
			}
			int num38 = PowerupScript.GoldenHandMidaTouch_CurrentlyActiveN(true);
			if (num38 > 0)
			{
				bool flag7 = true;
				SymbolScript.Kind symbolKind2 = patternInfos.symbolKind;
				if (symbolKind2 > SymbolScript.Kind.seven)
				{
					if (symbolKind2 - SymbolScript.Kind.six <= 1)
					{
						flag7 = false;
					}
					else
					{
						Debug.Log("SlotMachineScript, during spin, golden hand doesn't haandle symbol: " + patternInfos.symbolKind.ToString());
					}
				}
				if (flag7)
				{
					int num39 = GameplayData.Symbol_CoinsValue_GetBasic(patternInfos.symbolKind);
					int num40 = num38 * num39;
					GameplayData.Symbol_CoinsValueExtra_Add(patternInfos.symbolKind, num40);
					modifier_GoldenBonus += num40;
					if (!this._modifiersToAnimate_Temp.Contains(SymbolScript.Modifier.golden))
					{
						num25 = totalModifiers_PerType;
						totalModifiers_PerType = num25 + 1;
						this._modifiersToAnimate_Temp.Add(SymbolScript.Modifier.golden);
					}
				}
			}
			yield return this.WaitForTriggerAnimation();
			coinsEarnedDuringIteration += patternInfos.coins;
			this.BurningLevelSet(coinsEarnedDuringIteration);
			if (is666)
			{
				this._666RoundLostCoinsAdd(coinsReward);
				if (coinsReward <= -100000000L)
				{
					unlockFlag_Lost_100_MillionsTo666 = true;
				}
				if (coinsReward <= -1000000L)
				{
					unlockFlag_Lost_1_MillionTo666 = true;
				}
			}
			int repeatAnimationTimes = 1;
			if (this._modifiersToAnimate_Temp.Count > 0)
			{
				repeatAnimationTimes = 2;
			}
			if (is666)
			{
				repeatAnimationTimes = 3;
			}
			if (is667)
			{
				repeatAnimationTimes = 3;
			}
			if (isJackpot)
			{
				repeatAnimationTimes = 3;
			}
			float animationMultiplier = overTheGameSpeedMult * Data.SettingsData.TransitionSpeedMapped_Get((float)Data.settings.transitionSpeed, 1f, 4f, 1f, 3f);
			if (isJackpot)
			{
				animationMultiplier /= 1.15f + Mathf.Min(1.35f, (float)jackpotsPerformed * 0.15f);
				animationMultiplier += Mathf.Clamp(((float)jackpotsPerformed - 7f) * 0.25f, 0f, 2f);
			}
			else
			{
				float num41 = 0.1f;
				if (patternInfosList.Count > 100)
				{
					num41 = 0.25f;
				}
				if (patternInfosList.Count > 75)
				{
					num41 = 0.25f;
				}
				if (patternInfosList.Count > 60)
				{
					num41 = 0.25f;
				}
				if (patternInfosList.Count > 50)
				{
					num41 = 0.5f;
				}
				if (patternInfosList.Count > 30)
				{
					num41 = 0.3f;
				}
				if (patternInfosList.Count > 20)
				{
					num41 = 0.2f;
				}
				else if (patternInfosList.Count > 15)
				{
					num41 = 0.15f;
				}
				animationMultiplier += Mathf.Min(3f, (float)Mathf.Max(0, patternsTriggerCounter - 10) * num41);
				animationMultiplier += Mathf.Min(2f, (float)Mathf.Max(0, patternsTriggerCounter - 20) * num41);
				animationMultiplier += Mathf.Min(2f, (float)Mathf.Max(0, patternsTriggerCounter - 30) * num41);
				animationMultiplier += Mathf.Min(2f, (float)Mathf.Max(0, patternsTriggerCounter - 50) * num41);
			}
			TextMeshProUGUI scTxt;
			do
			{
				for (int num42 = 0; num42 < positions.Count; num42++)
				{
					SymbolScript symbolScript_ByScoringPosition2 = SymbolScript.GetSymbolScript_ByScoringPosition(positions[num42]);
					if (symbolScript_ByScoringPosition2 != null)
					{
						symbolScript_ByScoringPosition2.PlayAnimation(animationMultiplier);
					}
					else
					{
						Debug.LogError("We have a null symbol script! We cannot play its animation. SlotMachine position is x: " + positions[num42].x.ToString() + ", y: " + positions[num42].y.ToString());
					}
					this.ScoreSquareEnableSet(positions[num42].x, positions[num42].y, true);
				}
				if (isJackpot)
				{
					switch (jackpotsPerformed)
					{
					case 0:
					case 1:
						scTxt = this.GetScoreTextFromPool(Translation.Get("SLOT_COMBO_TEXT_JACKPOT") + "\n+" + patternInfos.coins.ToStringSmart(), false);
						break;
					case 2:
						scTxt = this.GetScoreTextFromPool(Translation.Get("SLOT_COMBO_TEXT_JACKPOT_SUPER") + "\n+" + patternInfos.coins.ToStringSmart(), false);
						break;
					case 3:
						scTxt = this.GetScoreTextFromPool(Translation.Get("SLOT_COMBO_TEXT_JACKPOT_MEGA") + "\n+" + patternInfos.coins.ToStringSmart(), false);
						break;
					case 4:
						scTxt = this.GetScoreTextFromPool(Translation.Get("SLOT_COMBO_TEXT_JACKPOT_ULTRA") + "\n+" + patternInfos.coins.ToStringSmart(), false);
						break;
					default:
					{
						int num43 = jackpotsPerformed - 4;
						if (num43 <= 1)
						{
							scTxt = this.GetScoreTextFromPool(Translation.Get("SLOT_COMBO_TEXT_JACKPOT_ULTIMATE") + "\n+" + patternInfos.coins.ToStringSmart(), false);
						}
						else
						{
							scTxt = this.GetScoreTextFromPool(string.Concat(new string[]
							{
								Translation.Get("SLOT_COMBO_TEXT_JACKPOT_ULTIMATE"),
								" X",
								num43.ToString(),
								"\n+",
								patternInfos.coins.ToStringSmart()
							}), false);
						}
						break;
					}
					}
				}
				else if (is666)
				{
					string text = coinsReward.ToStringSmart();
					scTxt = this.GetScoreTextFromPool(((coinsReward == 0L) ? "-" : "") + text, false);
				}
				else if (is667)
				{
					scTxt = this.GetScoreTextFromPool("+" + GameplayData.RoundEarnedCoinsGet().ToStringSmart(), false);
				}
				else if (isRepeatingPattern)
				{
					scTxt = this.GetScoreTextFromPool(Translation.Get("SLOT_COMBO_TEXT_AGAIN") + "\n+" + patternInfos.coins.ToStringSmart(), false);
				}
				else
				{
					scTxt = this.GetScoreTextFromPool("+" + patternInfos.coins.ToStringSmart(), false);
				}
				if (this._modifiersToAnimate_Temp.Count > 0 && repeatAnimationTimes == 1)
				{
					string text2 = null;
					float num44 = 1f + (float)(totalModifiers_PerType - this._modifiersToAnimate_Temp.Count) * 0.1f;
					SymbolScript.Modifier modifier2 = this._modifiersToAnimate_Temp[0];
					this._modifiersToAnimate_Temp.RemoveAt(0);
					if (this._modifiersToAnimate_Temp.Count > 0)
					{
						num25 = repeatAnimationTimes;
						repeatAnimationTimes = num25 + 1;
					}
					switch (modifier2)
					{
					case SymbolScript.Modifier.instantReward:
						text2 = text2 + ((text2 == null) ? "" : " ") + "<sprite name=\"ModInstantReward\">" + modifier_InstantReward.ToStringSmart();
						break;
					case SymbolScript.Modifier.cloverTicket:
						text2 = text2 + ((text2 == null) ? "" : " ") + "<sprite name=\"CloverTicketSaturated\">" + modifier_TicketBonus.ToString();
						break;
					case SymbolScript.Modifier.golden:
						text2 = text2 + ((text2 == null) ? "" : " ") + Strings.GetSpriteString_SlotMachineSymbol(patternInfos.symbolKind) + modifier_GoldenBonus.ToString();
						break;
					case SymbolScript.Modifier.repetition:
						text2 = text2 + ((text2 == null) ? "" : " ") + "<sprite name=\"ModRepetition\">" + modifier_Repetition.ToString();
						break;
					case SymbolScript.Modifier.battery:
						text2 = text2 + ((text2 == null) ? "" : " ") + "<sprite name=\"ModBattery\">" + modifier_Battery.ToString();
						break;
					case SymbolScript.Modifier.chain:
						text2 = text2 + ((text2 == null) ? "" : " ") + Strings.GetSpriteString_SlotMachinePattern(patternInfos.patternKind) + modifier_ChainBonus.ToString();
						break;
					default:
						Debug.LogError("SlotMachineScript.SpinCoroutine() - Some modifier is not handled, and the score string is resulting empty! Modifier: " + modifier2.ToString());
						break;
					}
					scTxt.text = text2;
					Sound.Play3D("SoundSlotMachineModifierScore", this.Audio3dPosition, 10f, 1f, num44, 1);
					Controls.VibrationSet_PreferMax(this.player, 0.25f);
				}
				scTxt.ForceMeshUpdate(false, false);
				scTxt.gameObject.SetActive(true);
				Vector3 scoringPosition_World = patternInfos.GetScoringPosition_World();
				scTxt.transform.position = scoringPosition_World + new Vector3(0f, 0.75f, -10f);
				Vector2 anchoredPosition2 = scTxt.rectTransform.anchoredPosition;
				float preferredWidth = scTxt.preferredWidth;
				float preferredHeight = scTxt.preferredHeight;
				anchoredPosition2.x = Mathf.Clamp(anchoredPosition2.x, -1.1f + preferredWidth / 2f, 1.1f - preferredWidth / 2f);
				anchoredPosition2.y = Mathf.Clamp(anchoredPosition2.y, -0.5f + preferredHeight / 2f, 0.9f - preferredHeight / 2f);
				scTxt.rectTransform.anchoredPosition = anchoredPosition2;
				if (isJackpot)
				{
					scTxt.transform.localScale = Vector2.one * (1f + Mathf.Min((float)(jackpotsPerformed - 1) * 0.05f, 0.25f));
				}
				else
				{
					scTxt.transform.localScale = Vector2.one;
				}
				if (is666)
				{
					this.SetTopScreenText(((coinsReward == 0L) ? "-" : "") + coinsReward.ToStringSmart() + " €", true);
				}
				else if (is667)
				{
					this.SetTopScreenText("+" + GameplayData.RoundEarnedCoinsGet().ToStringSmart() + " €", true);
				}
				else
				{
					this.SetTopScreenText("+" + patternInfos.coins.ToStringSmart() + " €", true);
				}
				for (int num45 = 0; num45 < positions.Count; num45++)
				{
					SymbolScript symbolScript_ByScoringPosition3 = SymbolScript.GetSymbolScript_ByScoringPosition(positions[num45]);
					switch (sensationalLevel)
					{
					case SlotMachineScript.SensationalLevel.noone:
						break;
					case SlotMachineScript.SensationalLevel.lowNice:
						Spawn.FromPool("Effect Small Coin 1", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.lowGreat:
						Spawn.FromPool("Effect Small Coin 2", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.lowFantastic:
						Spawn.FromPool("Effect Small Coin 3", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.lowJackpot:
						Spawn.FromPool("Effect Small Coin Jackpot", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.mediumNice:
						Spawn.FromPool("Effect Slot Stars 1", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.mediumGreat:
						Spawn.FromPool("Effect Slot Stars 2", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.mediumFantastic:
						Spawn.FromPool("Effect Slot Stars 3", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.mediumJackpot:
						Spawn.FromPool("Effect Slot Stars Jackpot", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.highNice:
						Spawn.FromPool("Effect Coins 1", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						if (hasJackpot)
						{
							Spawn.FromPool("Effect Slot Stars 1", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						}
						break;
					case SlotMachineScript.SensationalLevel.highGreat:
						Spawn.FromPool("Effect Coins 2", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						if (hasJackpot)
						{
							Spawn.FromPool("Effect Slot Stars 2", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						}
						break;
					case SlotMachineScript.SensationalLevel.highFantastic:
						Spawn.FromPool("Effect Coins 3", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						if (hasJackpot)
						{
							Spawn.FromPool("Effect Slot Stars 3", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						}
						break;
					case SlotMachineScript.SensationalLevel.highJackpot:
						Spawn.FromPool("Effect Coins Jackpot", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						if (hasJackpot)
						{
							Spawn.FromPool("Effect Slot Stars Jackpot", symbolScript_ByScoringPosition3.transform.position + new Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						}
						break;
					default:
						Debug.LogError("SlotMachineScript.SpinCoroutine() - Sensational level not implemented yet! Sensational level: " + sensationalLevel.ToString());
						break;
					}
				}
				if (patternsTriggerCounter > 11)
				{
					int num46 = (patternsTriggerCounter - 11) / 10;
					this.AllArroundSparksSet(Mathf.Min(num46, this.allArroundSparks.Length - 1));
					this.bounceScript.SetBounceScale(0.0035f + 0.00175f * (float)num46);
				}
				timer = 0.833f / animationMultiplier;
				bool lastPatternAnticipationSoundPlayed = false;
				bool playLastPatternAnticipationSound = isLastPattern && patternsTriggerCounter > 15 && repeatAnimationTimes == 1;
				if (playLastPatternAnticipationSound)
				{
					timer = Mathf.Max(2.3f, timer);
				}
				while (timer > 0f)
				{
					timer -= Tick.Time;
					if (playLastPatternAnticipationSound && !lastPatternAnticipationSoundPlayed)
					{
						lastPatternAnticipationSoundPlayed = true;
						Sound.Play3D("SoundSlotMachineLongStreakEndAnticipation", this.Audio3dPosition, 10f, 1f, 1f, 1);
					}
					if (jackpotsPerformed > 0)
					{
						this.bounceScript.SetBounceScale(Mathf.Min(0.025f, 0.0035f + 0.0035f * (float)jackpotsPerformed));
						this.bounceScript.SetBouncesPerSecond(8f);
					}
					yield return null;
				}
				this.bounceScript.ResetBounceFrequency();
				if (playLastPatternAnticipationSound)
				{
					float num47 = 1f;
					float num48 = 2f;
					if (isJackpot && jackpotsPerformed > 1)
					{
						num47 += 0.2f * (float)jackpotsPerformed;
						num47 = Mathf.Min(1.5f, num47);
					}
					FlashScreen.SpawnCamera(Color.yellow, num47, num48, CameraGame.firstInstance.myCamera, 0.5f);
					this.bounceScript.SetBounceScale(0.025f);
					float num49 = 2f;
					if (isJackpot && jackpotsPerformed > 1)
					{
						num49 = Mathf.Min(4f, 2f + (float)jackpotsPerformed + 0.5f);
					}
					CameraGame.Shake(num49);
					if (isJackpot && playLastPatternAnticipationSound)
					{
						Sound.Play3D("SoundSlotMachineApplause", this.Audio3dPosition, 10f, 1f, 1f, 1);
						this.confettiHolder.SetActive(true);
						timer = (num47 - 1f) / 2f;
						timer = Mathf.Min(timer, 0.5f);
						while (timer > 0f)
						{
							timer -= Tick.Time;
							yield return null;
						}
					}
				}
				if (scTxt != null)
				{
					this.ScoreTextDestroy(scTxt);
				}
				this.ScoreSquareEnableSetAll(false);
				num25 = repeatAnimationTimes;
				repeatAnimationTimes = num25 - 1;
			}
			while (repeatAnimationTimes > 0);
			if (is666 || is667)
			{
				FlashScreenSlot.Stop();
			}
			SlotMachineScript.PatternEvent onPatternEvaluationEnd = this.OnPatternEvaluationEnd;
			if (onPatternEvaluationEnd != null)
			{
				onPatternEvaluationEnd(patternInfos);
			}
			yield return this.WaitForTriggerAnimation();
			lastPatternScored = patternInfos;
			this.SpinFailsafeRestartTimer();
			if (hasJackpot)
			{
				this.JackpotGlowShrink();
			}
			if (hasJackpot)
			{
				this.JackpotLightReset();
			}
			if (hasJackpot && isLastPattern)
			{
				this.JackpotGalaxyReset();
			}
			patternInfos = null;
			positions = null;
			modifier_InstantReward = default(BigInteger);
			scTxt = null;
			num25 = num22;
		}
		if (this._hasShown666)
		{
			GameplayData.Stats_SixSixSix_SeenTimes += 1L;
		}
		if (this._hasShown666)
		{
			Data.GameData game = Data.game;
			num25 = game.PersistentStat_666SeenTimes;
			game.PersistentStat_666SeenTimes = num25 + 1;
		}
		SlotMachineScript.Event onScoreEvaluationEnd = this.OnScoreEvaluationEnd;
		if (onScoreEvaluationEnd != null)
		{
			onScoreEvaluationEnd();
		}
		List<PowerupScript> list = RedButtonScript.RegisteredPowerupsGet();
		for (int num50 = 0; num50 < batteryChargesToApply; num50++)
		{
			if (list.Count > 0)
			{
				int num51 = R.Rng_SymbolsMod.Range(0, list.Count);
				int num52 = 0;
				while (num52 < list.Count && !GameplayData.Powerup_ButtonChargesUsed_RestoreChargesN(list[num51].identifier, 1, true))
				{
					num51++;
					if (num51 > list.Count - 1)
					{
						num51 = 0;
					}
					num52++;
				}
			}
		}
		List<SlotMachineScript.PatternInfos> patternsEnabled2 = this.GetPatternsEnabled();
		bool flag8 = SlotMachineScript.IsAllSamePattern();
		GameplayData.Stats_ModifiedSymbol_TriggeredTimesGet();
		long num53 = GameplayData.Stats_ModifiedLemonTriggeredTimesGet();
		long num54 = GameplayData.Stats_ModifiedCherryTriggeredTimesGet();
		long num55 = GameplayData.Stats_ModifiedCloverTriggeredTimesGet();
		long num56 = GameplayData.Stats_ModifiedBellTriggeredTimesGet();
		long num57 = GameplayData.Stats_ModifiedDiamondTriggeredTimesGet();
		long num58 = GameplayData.Stats_ModifiedCoinsTriggeredTimesGet();
		long num59 = GameplayData.Stats_ModifiedSevenTriggeredTimesGet();
		int num60 = 0;
		int num61 = 0;
		int num62 = 0;
		int num63 = 0;
		while (num63 < patternsEnabled2.Count)
		{
			switch (patternsEnabled2[num63].symbolKind)
			{
			case SymbolScript.Kind.lemon:
				num60++;
				break;
			case SymbolScript.Kind.cherry:
				num60++;
				break;
			case SymbolScript.Kind.clover:
			case SymbolScript.Kind.bell:
			case SymbolScript.Kind.diamond:
			case SymbolScript.Kind.coins:
			case SymbolScript.Kind.seven:
			case SymbolScript.Kind.six:
			case SymbolScript.Kind.nine:
				break;
			default:
				"SlotMachineScript._SpinCoroutine(): inside UNLOCK POWERUPS: Symbol kind not handled: " + patternsEnabled2[num63].symbolKind.ToString();
				break;
			}
			switch (patternsEnabled2[num63].patternKind)
			{
			case PatternScript.Kind.jackpot:
			case PatternScript.Kind.horizontal2:
			case PatternScript.Kind.horizontal3:
			case PatternScript.Kind.horizontal4:
			case PatternScript.Kind.vertical2:
			case PatternScript.Kind.vertical3:
			case PatternScript.Kind.diagonal2:
			case PatternScript.Kind.diagonal3:
			case PatternScript.Kind.triangle:
			case PatternScript.Kind.triangleInverted:
			case PatternScript.Kind.eye:
				break;
			case PatternScript.Kind.horizontal5:
				if (patternsEnabled2[num63].symbolKind == SymbolScript.Kind.diamond)
				{
					num61++;
				}
				if (patternsEnabled2[num63].symbolKind == SymbolScript.Kind.coins)
				{
					num62++;
				}
				break;
			case PatternScript.Kind.pyramid:
				if (patternsEnabled2[num63].symbolKind == SymbolScript.Kind.diamond)
				{
					num61++;
				}
				if (patternsEnabled2[num63].symbolKind == SymbolScript.Kind.coins)
				{
					num62++;
				}
				break;
			case PatternScript.Kind.pyramidInverted:
				if (patternsEnabled2[num63].symbolKind == SymbolScript.Kind.diamond)
				{
					num61++;
				}
				if (patternsEnabled2[num63].symbolKind == SymbolScript.Kind.coins)
				{
					num62++;
				}
				break;
			case PatternScript.Kind.snakeUpDown:
			case PatternScript.Kind.snakeDownUp:
				goto IL_3635;
			default:
				goto IL_3635;
			}
			IL_3659:
			num63++;
			continue;
			IL_3635:
			"SlotMachineScript._SpinCoroutine(): inside UNLOCK POWERUPS: pattern not handled: " + patternsEnabled2[num63].patternKind.ToString();
			goto IL_3659;
		}
		if (num60 >= 7)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.FruitBasket);
		}
		if (num61 > 0 || num62 > 0)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.Necklace);
		}
		if (num59 >= 7L)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.SevenSinsStone);
		}
		if (unlockCounter_Jackpots > 0)
		{
			Data.game.UnlockSteps_AllIn += unlockCounter_Jackpots;
		}
		if (unlockCounter_Jackpots > 0)
		{
			Data.game.UnlockSteps_YellowStar += unlockCounter_Jackpots;
		}
		if (flag8)
		{
			Data.GameData game2 = Data.game;
			num25 = game2.UnlockSteps_PainKillers;
			game2.UnlockSteps_PainKillers = num25 + 1;
		}
		int num64 = unlockCounter_AboveTriggered + unlockCounter_BelowTriggered;
		if (num64 > 0)
		{
			Data.game.UnlockSteps_Baphomet += num64;
		}
		if (unlockCounter_JackpotsWithFruits > 0)
		{
			Data.game.UnlockSteps_RottenPepper += unlockCounter_JackpotsWithFruits;
		}
		if (unlockCounter_JackpotsWithCloverBells > 0)
		{
			Data.game.UnlockSteps_BellPepper += unlockCounter_JackpotsWithCloverBells;
		}
		if (unlockCounter_JackpotsWithDiamondTreasuresSevens > 0)
		{
			Data.game.UnlockSteps_GoldenPepper += unlockCounter_JackpotsWithDiamondTreasuresSevens;
		}
		if (this._hasShown666 && GameplayData.SpinsLeftGet() == 0)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.BookOfShadows);
		}
		if (unlockFlag_Lost_100_MillionsTo666)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.Gabibbh);
		}
		if (unlockFlag_Lost_1_MillionTo666)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.MysticalTomato);
			PowerupScript.Unlock(PowerupScript.Identifier.RitualBell);
			PowerupScript.Unlock(PowerupScript.Identifier.CrystalSkull);
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.AllMyHardEarnedMoney);
		}
		if (num57 + num58 + num59 >= 30L)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.GoldenKingMida);
		}
		if (num53 + num54 >= 30L)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_C_Dealer);
		}
		if (num55 + num56 >= 30L)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.Boardgame_M_Capitalist);
		}
		if (num53 + num54 > 15L)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.PuppetPersonalTrainer);
		}
		if (num55 + num56 > 15L)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.PuppetElectrician);
		}
		if (num57 + num58 + num59 > 15L)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.PuppetFortuneTeller);
		}
		for (int num65 = 0; num65 < symbolsAvailable.Count; num65++)
		{
			SymbolScript.Kind kind7 = symbolsAvailable[num65];
			long num66 = Data.GameData.Tracker_ModSymbolsCounter_Get(kind7);
			switch (kind7)
			{
			case SymbolScript.Kind.lemon:
				if ((long)Data.game.UnlockSteps_BoardgameC_Bricks != num66)
				{
					Data.game.UnlockSteps_BoardgameC_Bricks = num66.CastToInt();
				}
				if ((long)Data.game.UnlockSteps_BoardgameM_Carriola != num66)
				{
					Data.game.UnlockSteps_BoardgameM_Carriola = num66.CastToInt();
				}
				break;
			case SymbolScript.Kind.cherry:
				if ((long)Data.game.UnlockSteps_BoardgameC_Wood != num66)
				{
					Data.game.UnlockSteps_BoardgameC_Wood = num66.CastToInt();
				}
				if ((long)Data.game.UnlockSteps_BoardgameM_Shoe != num66)
				{
					Data.game.UnlockSteps_BoardgameM_Shoe = num66.CastToInt();
				}
				break;
			case SymbolScript.Kind.clover:
				if ((long)Data.game.UnlockSteps_BoardgameC_Sheep != num66)
				{
					Data.game.UnlockSteps_BoardgameC_Sheep = num66.CastToInt();
				}
				if ((long)Data.game.UnlockSteps_BoardgameM_Ditale != num66)
				{
					Data.game.UnlockSteps_BoardgameM_Ditale = num66.CastToInt();
				}
				break;
			case SymbolScript.Kind.bell:
				if ((long)Data.game.UnlockSteps_BoardgameC_Wheat != num66)
				{
					Data.game.UnlockSteps_BoardgameC_Wheat = num66.CastToInt();
				}
				if ((long)Data.game.UnlockSteps_BoardgameM_Iron != num66)
				{
					Data.game.UnlockSteps_BoardgameM_Iron = num66.CastToInt();
				}
				break;
			case SymbolScript.Kind.diamond:
				if ((long)Data.game.UnlockSteps_BoardgameC_Stone != num66)
				{
					Data.game.UnlockSteps_BoardgameC_Stone = num66.CastToInt();
				}
				if ((long)Data.game.UnlockSteps_BoardgameM_Car != num66)
				{
					Data.game.UnlockSteps_BoardgameM_Car = num66.CastToInt();
				}
				break;
			case SymbolScript.Kind.coins:
				if ((long)Data.game.UnlockSteps_BoardgameC_Harbor != num66)
				{
					Data.game.UnlockSteps_BoardgameC_Harbor = num66.CastToInt();
				}
				if ((long)Data.game.UnlockSteps_BoardgameM_Ship != num66)
				{
					Data.game.UnlockSteps_BoardgameM_Ship = num66.CastToInt();
				}
				break;
			case SymbolScript.Kind.seven:
				if ((long)Data.game.UnlockSteps_BoardgameC_Thief != num66)
				{
					Data.game.UnlockSteps_BoardgameC_Thief = num66.CastToInt();
				}
				if ((long)Data.game.UnlockSteps_BoardgameM_TubaHat != num66)
				{
					Data.game.UnlockSteps_BoardgameM_TubaHat = num66.CastToInt();
				}
				break;
			case SymbolScript.Kind.six:
			case SymbolScript.Kind.nine:
				break;
			default:
				Debug.LogError("Slot spin code, board game stats tracking, Symbol not handled: " + kind7.ToString());
				break;
			}
		}
		if (GameplayData.Stats_SixSixSix_SeenTimes >= 3L)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.HornDevil);
		}
		if (GameplayData.Stats_SixSixSix_SeenTimes >= 5L)
		{
			PowerupScript.Unlock(PowerupScript.Identifier.Necronomicon);
		}
		if (isJackpot)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.LuckyDay);
		}
		if (SlotMachineScript.Has666())
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.TheNumberOfTheBeast);
		}
		if (jackpotsPerformed >= 5)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.SuperMegaUltraUltimateWin);
		}
		if (SlotMachineScript.Has999())
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.DivineIntervention);
		}
		if (patternInfosList.Count >= 15)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.PatternRecognition);
		}
		yield return this.WaitForTriggerAnimation();
		if (coinsReward > 0L)
		{
			GameplayData.SpinsWithoutReward_Reset();
		}
		else
		{
			GameplayData.SpinsWithoutReward_Increase();
		}
		if (patternsWith5PlusSymbols_Count > 0)
		{
			GameplayData.ConsecutiveSpinsWithout5PlusPatterns_Set(0);
		}
		else
		{
			GameplayData.ConsecutiveSpinsWithout5PlusPatterns_Set(GameplayData.ConsecutiveSpinsWithout5PlusPatterns_Get() + 1);
		}
		if (patternsWithDiamondsTreasuresOrSevens_Count > 0)
		{
			GameplayData.ConsecutiveSpinsWithDiamondTreasureOrSeven_Set(GameplayData.ConsecutiveSpinsWithDiamondTreasureOrSeven_Get() + 1);
		}
		else
		{
			GameplayData.ConsecutiveSpinsWithDiamondTreasureOrSeven_Set(0);
		}
		if (hasJackpot)
		{
			PowerupScript.AllIn_TryTriggeringAnimation();
			GameplayData.SpinsWithAtLeast1Jackpot_Set(GameplayData.SpinsWithAtLeast1Jackpot_Get() + 1L);
		}
		this.TopTextSet_666Or999_ChancesShow();
		coinsReward += this.spinExtraCoins;
		SlotMachineScript.Event onScoreEvaluationEnd_Late = this.OnScoreEvaluationEnd_Late;
		if (onScoreEvaluationEnd_Late != null)
		{
			onScoreEvaluationEnd_Late();
		}
		yield return this.WaitForTriggerAnimation();
		this._spinningBeforeCoinsReward = false;
		GameplayData.CoinsAdd(coinsReward, coinsReward > 0L);
		GameplayData.RoundEarnedCoinsAdd(coinsReward);
		if (SlotMachineScript.Has666() || SlotMachineScript.Has999())
		{
			GameplayData.Instance._phone_bookSpecialCall = true;
		}
		if (SlotMachineScript.Has666() || SlotMachineScript.Has999())
		{
			GameplayData.LastRoundHad666Or999 = true;
		}
		if (coinsReward > 0L)
		{
			this.SetTopScreenText("+" + coinsReward.ToStringSmart() + " €", true);
		}
		if (coinsReward > 0L && patternsN > 1)
		{
			this.SpinWinSetText(coinsReward);
		}
		if (!(GameplayData.RoundEarnedCoinsGet() <= 0L))
		{
			if (GameplayData.SpinsLeftGet() <= 0)
			{
				while (this.IsSpinWinTextPlaying())
				{
					if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) || Controls.ActionButton_PressedGet(0, Controls.InputAction.interact, true))
					{
						this.SpinWinText_StopIfAny();
						break;
					}
					yield return null;
				}
				int animCoinsRewardInt = GameplayData.RoundEarnedCoinsGet().CastToInt();
				CameraController.SetPosition(CameraController.PositionKind.SlotCoinPlate_Fixed, false, 1f);
				string text3 = "SoundCoinFall";
				if (animCoinsRewardInt >= 5)
				{
					text3 = null;
				}
				if (text3 == null)
				{
					Sound.Play3D("SoundCoinsMultipleFall", this.Audio3dPositionLow, 10f, 1f, Random.Range(0.9f, 1.1f), 1);
				}
				CoinVisualizerScript.ArrayCheckShow(this.coinsVisualizers, animCoinsRewardInt - 1, 0.05f, 0.01f, text3);
				timer = 0.5f;
				while (timer > 0f)
				{
					timer -= Tick.Time;
					yield return null;
				}
				if (animCoinsRewardInt > 0)
				{
					Sound.Play3D("SoundInterestRetrieved", this.Audio3dPositionLow, 10f, 1f, 1f, 1);
				}
				CoinVisualizerScript.HideAll(this.coinsVisualizers);
				timer = 0.25f;
				while (timer > 0f)
				{
					timer -= Tick.Time;
					yield return null;
				}
				CameraController.SetPosition(CameraController.PositionKind.Slot_Fixed, false, 1f);
			}
		}
		SlotMachineScript.Event onSpinEnd = this.OnSpinEnd;
		if (onSpinEnd != null)
		{
			onSpinEnd();
		}
		yield return this.WaitForTriggerAnimation();
		this.TopTextSet_666Or999_ChancesShow();
		this._state = SlotMachineScript.State.idle;
		this.spinCoroutine = null;
		RedButtonScript.ResetTiming(RedButtonScript.RegistrationCapsule.Timing.perSpin);
		this.BurningLevelSteamOff();
		this._isFirstSpinOfRound = false;
		if (this.spinFailsafeCoroutine != null)
		{
			base.StopCoroutine(this.spinFailsafeCoroutine);
		}
		yield break;
	}

	// Token: 0x06000722 RID: 1826 RVA: 0x0002D22C File Offset: 0x0002B42C
	private int _SymbolsSpawn(bool onlyTheDataOnes, bool lastWheelIsSlow)
	{
		int num = 0;
		List<SymbolScript.Kind> list = GameplayData.SymbolsAvailable_GetAll(false);
		this.elementsInColumn.Clear();
		Vector3 vector;
		vector..ctor(0f, 0f, 100f);
		for (int i = 0; i < 5; i++)
		{
			RectTransform rectTransform = this.columnsRectTr[i];
			this.elementsInColumn.Clear();
			foreach (object obj in rectTransform)
			{
				Transform transform = (Transform)obj;
				this.elementsInColumn.Add(transform);
			}
			for (int j = 0; j < this.elementsInColumn.Count; j++)
			{
				Pool.Destroy(this.elementsInColumn[j].gameObject, null);
				this.elementsInColumn[j].SetParent(Pool.instance.transform);
			}
			this.elementsInColumn.Clear();
			for (int k = 0; k < 3; k++)
			{
				SymbolScript.Kind kind = this.lines[k][i];
				SymbolScript symbolScript = this.Symbol_SpawnInstance(true, kind, SymbolScript.Modifier.none, i, this.elementsInColumn.Count, true);
				this.elementsInColumn.Add(symbolScript.transform);
			}
			num = Mathf.Max(num, this.elementsInColumn.Count);
			if (!onlyTheDataOnes)
			{
				int num2 = 24;
				if (lastWheelIsSlow)
				{
					num2 += 12;
				}
				for (int l = 0; l < num2; l++)
				{
					SymbolScript.Kind kind2 = list[R.Rng_SymbolsChance.Range(0, list.Count)];
					SymbolScript symbolScript = this.Symbol_SpawnInstance(false, kind2, SymbolScript.Modifier.none, i, this.elementsInColumn.Count, true);
					this.elementsInColumn.Add(symbolScript.transform);
				}
				for (int m = 0; m < 3; m++)
				{
					GameObject gameObject = Spawn.FromPool(SymbolScript.GetPrefabName(this.linesOld[m][i]), vector, rectTransform);
					gameObject.transform.localPosition = new Vector3(0f, (float)(-(float)this.elementsInColumn.Count) * 0.5f, 0f);
					gameObject.transform.localScale = Vector3.one;
					this.elementsInColumn.Add(gameObject.transform);
				}
				num = Mathf.Max(num, this.elementsInColumn.Count);
			}
		}
		return num;
	}

	// Token: 0x06000723 RID: 1827 RVA: 0x0002D48C File Offset: 0x0002B68C
	public SymbolScript Symbol_SpawnInstance(bool isScoringSymbol, SymbolScript.Kind kind, SymbolScript.Modifier modifier, int columnX, int lineY, bool pickRandomModifier)
	{
		Vector3 vector;
		vector..ctor(0f, 0f, 100f);
		RectTransform rectTransform = this.columnsRectTr[columnX];
		GameObject gameObject = Spawn.FromPool(SymbolScript.GetPrefabName(kind), vector, rectTransform);
		gameObject.transform.localPosition = new Vector3(0f, (float)(-(float)lineY) * 0.5f, 0f);
		gameObject.transform.localScale = Vector3.one;
		SymbolScript component = gameObject.GetComponent<SymbolScript>();
		if (isScoringSymbol)
		{
			component.MarkAsScoringSymbol(columnX, lineY);
		}
		if (pickRandomModifier || modifier != SymbolScript.Modifier.none)
		{
			SymbolScript.Modifier modifier2 = GameplayData.Symbol_Modifier_GetRandom(kind);
			if (modifier != SymbolScript.Modifier.none)
			{
				modifier2 = modifier;
			}
			component.ModifierSet(modifier2);
		}
		else
		{
			component.ModifierSet(SymbolScript.Modifier.none);
		}
		component.MaterialUpdate();
		return component;
	}

	// Token: 0x06000724 RID: 1828 RVA: 0x0002D537 File Offset: 0x0002B737
	private IEnumerator WaitForTriggerAnimation()
	{
		yield return null;
		while (PowerupTriggerAnimController.HasAnimations())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000725 RID: 1829 RVA: 0x0002D540 File Offset: 0x0002B740
	private void PatternInfosResetPool()
	{
		for (int i = 0; i < this._patternInfos.Count; i++)
		{
			this._patternInfos[i].enabled = false;
		}
	}

	// Token: 0x06000726 RID: 1830 RVA: 0x0002D578 File Offset: 0x0002B778
	public void PatternInfoSetup(PatternScript.Kind _patternKind, BigInteger _coins, List<Vector2Int> _positionsToCopy)
	{
		SlotMachineScript.PatternInfos patternInfos = null;
		for (int i = 0; i < this._patternInfos.Count; i++)
		{
			if (!this._patternInfos[i].enabled)
			{
				patternInfos = this._patternInfos[i];
				this._patternInfos[i].enabled = true;
				break;
			}
		}
		if (patternInfos == null)
		{
			patternInfos = new SlotMachineScript.PatternInfos();
			this._patternInfos.Add(patternInfos);
			patternInfos.enabled = true;
		}
		if (patternInfos == null)
		{
			Debug.LogError("PatternInfoSetup() - No pattern info instance was found, and no new one was created! This is a critical error!");
			return;
		}
		patternInfos.Setup(_patternKind, _coins, _positionsToCopy);
	}

	// Token: 0x06000727 RID: 1831 RVA: 0x0002D604 File Offset: 0x0002B804
	public SlotMachineScript.PatternInfos PatternInfosGetLastPattern()
	{
		for (int i = this._patternInfos.Count - 1; i >= 0; i--)
		{
			if (this._patternInfos[i].enabled)
			{
				return this._patternInfos[i];
			}
		}
		return null;
	}

	// Token: 0x06000728 RID: 1832 RVA: 0x0002D64C File Offset: 0x0002B84C
	public List<SlotMachineScript.PatternInfos> GetPatternsEnabled()
	{
		this._PatInf_GetEnabledList.Clear();
		for (int i = 0; i < this._patternInfos.Count; i++)
		{
			if (this._patternInfos[i].enabled)
			{
				this._PatInf_GetEnabledList.Add(this._patternInfos[i]);
			}
		}
		return this._PatInf_GetEnabledList;
	}

	// Token: 0x06000729 RID: 1833 RVA: 0x0002D6AC File Offset: 0x0002B8AC
	public List<SlotMachineScript.PatternInfos> GetPatternsOfKind(PatternScript.Kind patternKind)
	{
		this._PatInf_GetByKindList.Clear();
		for (int i = 0; i < this._patternInfos.Count; i++)
		{
			if (this._patternInfos[i].patternKind == patternKind && this._patternInfos[i].enabled)
			{
				this._PatInf_GetByKindList.Add(this._patternInfos[i]);
			}
		}
		return this._PatInf_GetByKindList;
	}

	// Token: 0x0600072A RID: 1834 RVA: 0x0002D720 File Offset: 0x0002B920
	public static int GetPatternsCount()
	{
		if (SlotMachineScript.instance == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < SlotMachineScript.instance._patternInfos.Count; i++)
		{
			if (SlotMachineScript.instance._patternInfos[i].enabled)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600072B RID: 1835 RVA: 0x0002D774 File Offset: 0x0002B974
	public static int GetPatternsCount_BySymbol(SymbolScript.Kind symbol)
	{
		if (SlotMachineScript.instance == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < SlotMachineScript.instance._patternInfos.Count; i++)
		{
			if (SlotMachineScript.instance._patternInfos[i].symbolKind == symbol && SlotMachineScript.instance._patternInfos[i].enabled)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x0600072C RID: 1836 RVA: 0x0002D7E0 File Offset: 0x0002B9E0
	private static bool PatternContainsPosition(SlotMachineScript.PatternInfos patternInfo, Vector2Int position)
	{
		return patternInfo != null && SlotMachineScript.ListContainsPosition(patternInfo.positions, position);
	}

	// Token: 0x0600072D RID: 1837 RVA: 0x0002D7F4 File Offset: 0x0002B9F4
	private static bool ListContainsPosition(List<Vector2Int> listToCheck, Vector2Int position)
	{
		if (listToCheck == null)
		{
			return false;
		}
		for (int i = 0; i < listToCheck.Count; i++)
		{
			if (listToCheck[i] == position)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600072E RID: 1838 RVA: 0x0002D829 File Offset: 0x0002BA29
	private IEnumerator PatternsCompute_Coroutine()
	{
		bool has666 = false;
		bool has667 = false;
		this._isAllSamePattern = false;
		BigInteger _666LostCoins = this._666RoundLostCoinsGet();
		RunModifierScript.Identifier runModifier = GameplayData.RunModifier_GetCurrent();
		BigInteger debtIndex = GameplayData.DebtIndexGet();
		int genericAdditionalRepeatTimes = PowerupScript.EternityRepetitionsCounterGet();
		int yellowSymbolsRepeatTimes = PowerupScript.PissJar_CurrentlyActiveN(true);
		int NOTyellowSymbolsRepeatTimes = PowerupScript.PoopJar_CurrentlyActiveN(true);
		int fruitsRepeatTimes = PowerupScript.FruitBasketBonusGet() + PowerupScript.MysticalTomatoGetRepetitions();
		int num;
		if (PowerupScript.BrokenCalculatorIsActive())
		{
			num = genericAdditionalRepeatTimes;
			genericAdditionalRepeatTimes = num + 1;
		}
		if (PowerupScript.DiscA_IsTriggeringTime())
		{
			num = genericAdditionalRepeatTimes;
			genericAdditionalRepeatTimes = num + 1;
		}
		if (GameplayData.Powerup_Jimbo_IsAbilityAvailable(GameplayData.JimboAbility.Good_Repetitions, true))
		{
			num = genericAdditionalRepeatTimes;
			genericAdditionalRepeatTimes = num + 1;
		}
		genericAdditionalRepeatTimes += PowerupScript.UpsideHamsa_BonusTriggersGet();
		genericAdditionalRepeatTimes += GameplayData.AbilityHoly_PatternsRepetitions;
		this.PatternInfosResetPool();
		this.PatternComputtaion_ResetResultVariables();
		this._antiduplicateIdsList.Clear();
		List<PatternScript.Kind> patternsAvailableToCheck = GameplayData.PatternsAvailable_GetAll();
		for (int y = 0; y < 3; y = num + 1)
		{
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < patternsAvailableToCheck.Count; j++)
				{
					PatternScript.Kind kind = patternsAvailableToCheck[j];
					bool flag = PatternScript.IsDiagonal(kind);
					bool[][] array = PatternScript.GetPatternMask(kind, false);
					SymbolScript.Kind kind2 = this.GetFirstSymbolOfMask(array, i, y, 5, 3);
					this._patternCordsListTemp.Clear();
					bool flag2 = kind2 != SymbolScript.Kind.undefined && this.PatternMaskCompare(kind2, array, i, y, 5, 3);
					if (flag2)
					{
						flag2 = this.FirstTimeAddingThisPattern(this._patternCordsListTemp);
					}
					if (flag2)
					{
						int num2 = 1 + genericAdditionalRepeatTimes + this._RepetitionModifiersInsidePattern_GetCount(this._patternCordsListTemp);
						if (SymbolScript.IsYellow(kind2))
						{
							num2 += yellowSymbolsRepeatTimes;
						}
						if (!SymbolScript.IsYellow(kind2))
						{
							num2 += NOTyellowSymbolsRepeatTimes;
						}
						if (kind2 == SymbolScript.Kind.lemon || kind2 == SymbolScript.Kind.cherry)
						{
							num2 += fruitsRepeatTimes;
						}
						if (!SymbolScript.SymbolCanRepeatTrigger(kind2))
						{
							num2 = 1;
						}
						for (int k = 0; k < num2; k++)
						{
							this.PatternInfoSetup(kind, SlotMachineScript._ComputePatternValue(kind, kind2), this._patternCordsListTemp);
						}
					}
					if (flag2 && kind == PatternScript.Kind.horizontal3 && kind2 == SymbolScript.Kind.six)
					{
						has666 = true;
					}
					if (flag2 && kind == PatternScript.Kind.horizontal3 && kind2 == SymbolScript.Kind.nine)
					{
						has667 = true;
					}
					if (flag)
					{
						array = PatternScript.GetPatternMask(kind, true);
						kind2 = this.GetFirstSymbolOfMask(array, i, y, 5, 3);
						this._patternCordsListTemp.Clear();
						flag2 = kind2 != SymbolScript.Kind.undefined && this.PatternMaskCompare(kind2, array, i, y, 5, 3);
						if (flag2)
						{
							flag2 = this.FirstTimeAddingThisPattern(this._patternCordsListTemp);
						}
						if (flag2)
						{
							int num3 = 1 + genericAdditionalRepeatTimes + this._RepetitionModifiersInsidePattern_GetCount(this._patternCordsListTemp);
							if (SymbolScript.IsYellow(kind2))
							{
								num3 += yellowSymbolsRepeatTimes;
							}
							if (!SymbolScript.IsYellow(kind2))
							{
								num3 += NOTyellowSymbolsRepeatTimes;
							}
							if (kind2 == SymbolScript.Kind.lemon || kind2 == SymbolScript.Kind.cherry)
							{
								num3 += fruitsRepeatTimes;
							}
							if (!SymbolScript.SymbolCanRepeatTrigger(kind2))
							{
								num3 = 1;
							}
							for (int l = 0; l < num3; l++)
							{
								this.PatternInfoSetup(kind, SlotMachineScript._ComputePatternValue(kind, kind2), this._patternCordsListTemp);
							}
						}
					}
				}
			}
			yield return null;
			num = y;
		}
		if (!Master.instance.SCORE_PATTERNS_INSIDE_JACKPOT && this.GetPatternsOfKind(PatternScript.Kind.jackpot).Count > 0)
		{
			for (int m = 0; m < this._patternInfos.Count; m++)
			{
				if (this._patternInfos[m].patternKind != PatternScript.Kind.jackpot)
				{
					this._patternInfos[m].enabled = false;
				}
			}
		}
		for (int n = 0; n < this._patternInfos.Count; n++)
		{
			if (this._patternInfos[n].enabled)
			{
				SlotMachineScript.PatternInfos patternInfos = this._patternInfos[n];
				for (int num4 = 0; num4 < this._patternInfos.Count; num4++)
				{
					if (this._patternInfos[num4].enabled && n != num4 && this._patternInfos[num4].symbolKind == patternInfos.symbolKind)
					{
						SlotMachineScript.PatternInfos patternInfos2 = this._patternInfos[num4];
						if ((!Master.instance.SCORE_PATTERNS_INSIDE_JACKPOT || patternInfos2.patternKind != PatternScript.Kind.jackpot) && patternInfos2.positions.Count != patternInfos.positions.Count)
						{
							bool flag3 = true;
							for (int num5 = 0; num5 < patternInfos.positions.Count; num5++)
							{
								if (!patternInfos2.positions.Contains(patternInfos.positions[num5]))
								{
									flag3 = false;
									break;
								}
							}
							if (flag3)
							{
								patternInfos.enabled = false;
								break;
							}
						}
					}
				}
				if ((patternInfos.symbolKind == SymbolScript.Kind.six || patternInfos.symbolKind == SymbolScript.Kind.nine) && (patternInfos.patternKind == PatternScript.Kind.horizontal2 || patternInfos.patternKind == PatternScript.Kind.vertical2 || patternInfos.patternKind == PatternScript.Kind.diagonal2))
				{
					patternInfos.enabled = false;
				}
			}
		}
		if (has666)
		{
			if (debtIndex < GameplayData.SuperSixSixSix_GetMinimumDebtIndex())
			{
				this._patternComputationResult_Coins = -GameplayData.RoundEarnedCoinsGet();
			}
			else
			{
				this._patternComputationResult_Coins = -GameplayData.CoinsGet();
			}
			for (int num6 = 0; num6 < this._patternInfos.Count; num6++)
			{
				if (this._patternInfos[num6].symbolKind != SymbolScript.Kind.six)
				{
					this._patternInfos[num6].enabled = false;
				}
			}
			yield break;
		}
		if (has667)
		{
			this._patternComputationResult_Coins = GameplayData.RoundEarnedCoinsGet();
			GameplayData.NineNineNne_TotalRewardEarned_Set(GameplayData.NineNineNne_TotalRewardEarned_Get() + this._patternComputationResult_Coins);
			for (int num7 = 0; num7 < this._patternInfos.Count; num7++)
			{
				if (this._patternInfos[num7].symbolKind != SymbolScript.Kind.nine)
				{
					this._patternInfos[num7].enabled = false;
				}
			}
			yield break;
		}
		int num8 = 0;
		for (int num9 = 0; num9 < this._patternInfos.Count - 1; num9++)
		{
			if (!this._patternInfos[num8].enabled)
			{
				SlotMachineScript.PatternInfos patternInfos3 = this._patternInfos[num8];
				this._patternInfos.RemoveAt(num8);
				this._patternInfos.Add(patternInfos3);
			}
			else
			{
				num8++;
			}
		}
		for (int num10 = 0; num10 < this._patternInfos.Count; num10++)
		{
			for (int num11 = num10 + 1; num11 < this._patternInfos.Count; num11++)
			{
				ulong orderWeight = this._patternInfos[num10].GetOrderWeight();
				ulong orderWeight2 = this._patternInfos[num11].GetOrderWeight();
				if (this._patternInfos[num11].enabled && (!this._patternInfos[num10].enabled || orderWeight > orderWeight2 || (orderWeight == orderWeight2 && this._patternInfos[num10].coins > this._patternInfos[num11].coins)))
				{
					SlotMachineScript.PatternInfos patternInfos4 = this._patternInfos[num10];
					this._patternInfos[num10] = this._patternInfos[num11];
					this._patternInfos[num11] = patternInfos4;
				}
			}
		}
		int num12 = SlotMachineScript.GetPatternsCount();
		this._isAllSamePattern = this.PatternsAreAllTheSameOne(num12);
		if (PowerupScript.NecklaceBonusGet(true) > 0 && num12 > 0 && this._isAllSamePattern)
		{
			for (int num13 = 0; num13 < this._patternInfos.Count; num13++)
			{
				bool flag4 = this._patternInfos[num13].symbolKind == SymbolScript.Kind.diamond || this._patternInfos[num13].symbolKind == SymbolScript.Kind.coins;
				if (this._patternInfos[num13].enabled && flag4)
				{
					this.PatternInfoSetup(this._patternInfos[num13].patternKind, this._patternInfos[num13].coins, this._patternInfos[num13].positions);
					num12++;
					this.PatternInfoSetup(this._patternInfos[num13].patternKind, this._patternInfos[num13].coins * 2, this._patternInfos[num13].positions);
					num12++;
					break;
				}
			}
		}
		int num14 = PowerupScript.ModifiedPowerups_GetCount(PowerupScript.Modifier.obsessive);
		if (num14 > 0 && num12 > 0)
		{
			SlotMachineScript.PatternInfos patternInfos5 = this.PatternInfosGetLastPattern();
			if (patternInfos5 != null && patternInfos5.enabled)
			{
				for (int num15 = 0; num15 < num14; num15++)
				{
					this.PatternInfoSetup(patternInfos5.patternKind, patternInfos5.coins, patternInfos5.positions);
					num12++;
				}
			}
		}
		int num16 = -1;
		this._biggestPatternScored = PatternScript.Kind.undefined;
		for (int num17 = this._patternInfos.Count - 1; num17 >= 0; num17--)
		{
			if (this._patternInfos[num17].enabled)
			{
				int count = this._patternInfos[num17].positions.Count;
				if (count > num16)
				{
					num16 = count;
					this._biggestPatternScored = this._patternInfos[num17].patternKind;
				}
				if (runModifier == RunModifierScript.Identifier._666DoubleChances_JackpotRecovers && this._patternInfos[num17].patternKind == PatternScript.Kind.jackpot && _666LostCoins > 0L)
				{
					this._patternInfos[num17].coins += _666LostCoins;
					this._666GotCoinsRestoredFromJackpot = true;
					_666LostCoins = 0;
				}
				this._patternComputationResult_Coins += this._patternInfos[num17].coins;
			}
		}
		yield break;
	}

	// Token: 0x0600072F RID: 1839 RVA: 0x0002D838 File Offset: 0x0002BA38
	private bool PatternsAreAllTheSameOne(int patternsCount)
	{
		if (patternsCount == 0)
		{
			return false;
		}
		bool flag = true;
		SlotMachineScript.PatternInfos patternInfos = this._patternInfos[0];
		for (int i = 0; i < this._patternInfos.Count; i++)
		{
			if (this._patternInfos[i].enabled && !this._patternInfos[i].IsEqualToOtherPattern(patternInfos))
			{
				flag = false;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06000730 RID: 1840 RVA: 0x0002D89C File Offset: 0x0002BA9C
	private SymbolScript.Kind GetFirstSymbolOfMask(bool[][] patternMask, int x, int y, int COLUMNS_N, int LINES_N)
	{
		for (int i = 0; i < patternMask.Length; i++)
		{
			bool flag = patternMask[i][0];
			int num = y + i;
			if (num >= 0 && num < LINES_N && flag)
			{
				return this.Symbol_GetAtPosition(x, num);
			}
		}
		return SymbolScript.Kind.undefined;
	}

	// Token: 0x06000731 RID: 1841 RVA: 0x0002D8D8 File Offset: 0x0002BAD8
	private bool PatternMaskCompare(SymbolScript.Kind symbolKind, bool[][] patternMask, int x, int y, int COLUMNS_N, int LINES_N)
	{
		bool flag = true;
		for (int i = 0; i < patternMask.Length; i++)
		{
			for (int j = 0; j < patternMask[i].Length; j++)
			{
				if (patternMask[i][j])
				{
					if (x + j >= COLUMNS_N || y + i >= LINES_N)
					{
						flag = false;
						break;
					}
					if (this.Symbol_GetAtPosition(x + j, y + i) != symbolKind)
					{
						flag = false;
						break;
					}
					this._patternCordsListTemp.Add(new Vector2Int(x + j, y + i));
				}
			}
		}
		return flag;
	}

	// Token: 0x06000732 RID: 1842 RVA: 0x0002D94C File Offset: 0x0002BB4C
	private string _PatternId_ComputeIdFromPositions(List<Vector2Int> positions)
	{
		string text = "";
		for (int i = 0; i < positions.Count; i++)
		{
			text = string.Concat(new string[]
			{
				text,
				"x",
				positions[i].x.ToString(),
				"y",
				positions[i].y.ToString()
			});
		}
		return text;
	}

	// Token: 0x06000733 RID: 1843 RVA: 0x0002D9C5 File Offset: 0x0002BBC5
	private void _PatternId_AddToList(string id)
	{
		this._antiduplicateIdsList.Add(id);
	}

	// Token: 0x06000734 RID: 1844 RVA: 0x0002D9D3 File Offset: 0x0002BBD3
	private bool _PatternId_IsAlreadyInList(string id)
	{
		return this._antiduplicateIdsList.Contains(id);
	}

	// Token: 0x06000735 RID: 1845 RVA: 0x0002D9E4 File Offset: 0x0002BBE4
	private bool FirstTimeAddingThisPattern(List<Vector2Int> positions)
	{
		string text = this._PatternId_ComputeIdFromPositions(positions);
		if (this._PatternId_IsAlreadyInList(text))
		{
			return false;
		}
		this._PatternId_AddToList(text);
		return true;
	}

	// Token: 0x06000736 RID: 1846 RVA: 0x0002DA0C File Offset: 0x0002BC0C
	private void PatternComputtaion_ResetResultVariables()
	{
		this._patternComputationResult_Coins = 0;
	}

	// Token: 0x06000737 RID: 1847 RVA: 0x0002DA1A File Offset: 0x0002BC1A
	private void PatternComputation_GetResults(out BigInteger coins)
	{
		coins = this._patternComputationResult_Coins;
	}

	// Token: 0x06000738 RID: 1848 RVA: 0x0002DA28 File Offset: 0x0002BC28
	private int _RepetitionModifiersInsidePattern_GetCount(List<Vector2Int> positions)
	{
		int num = 0;
		for (int i = 0; i < positions.Count; i++)
		{
			SymbolScript symbolScript = this.Symbol_GetInstanceAtPosition(positions[i].x, positions[i].y);
			if (symbolScript != null && symbolScript.ModifierGet() == SymbolScript.Modifier.repetition)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x0002DA84 File Offset: 0x0002BC84
	private static BigInteger _ComputePatternValue(PatternScript.Kind patternKind, SymbolScript.Kind symbolKind)
	{
		if (patternKind == PatternScript.Kind.undefined)
		{
			Debug.LogError("Cannot get pattern value. Pattern kind is undefined.");
			return -1;
		}
		if (patternKind == PatternScript.Kind.count)
		{
			Debug.LogError("Cannot get pattern value. Pattern kind is count.");
			return -1;
		}
		if (symbolKind == SymbolScript.Kind.undefined)
		{
			Debug.LogError("Cannot get pattern value. Symbol kind is undefined.");
			return -1;
		}
		if (symbolKind == SymbolScript.Kind.count)
		{
			Debug.LogError("Cannot get pattern value. Symbol kind is count.");
			return -1;
		}
		BigInteger bigInteger = GameplayData.AllSymbolsMultiplierGet(true);
		BigInteger bigInteger2 = GameplayData.Symbol_CoinsOverallValue_Get(symbolKind);
		BigInteger bigInteger3 = GameplayData.AllPatternsMultiplierGet(true);
		double num = GameplayData.Pattern_ValueOverall_Get(patternKind, true);
		BigInteger bigInteger4;
		if (double.IsInfinity(num))
		{
			bigInteger4 = new BigInteger(double.PositiveInfinity);
		}
		else
		{
			bigInteger4 = new BigInteger(100.0 * num);
		}
		return bigInteger * bigInteger2 * bigInteger3 * bigInteger4 / 100;
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x0002DB50 File Offset: 0x0002BD50
	public static SlotMachineScript.SensationalLevel GetPatternSensationalLevel(PatternScript.Kind pKind, SymbolScript.Kind symbolKind)
	{
		if (symbolKind == SymbolScript.Kind.six)
		{
			return SlotMachineScript.SensationalLevel.noone;
		}
		if (symbolKind == SymbolScript.Kind.six)
		{
			return SlotMachineScript.SensationalLevel.noone;
		}
		List<SymbolScript.Kind> list = GameplayData.SymbolsValueList_Get();
		List<SymbolScript.Kind> list2 = GameplayData.MostValuableSymbols_GetList();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (list2.Contains(symbolKind))
		{
			flag3 = true;
		}
		else if (list.Count >= 3 && list[2] == symbolKind)
		{
			flag3 = true;
		}
		else if (list.Count >= 2 && list[1] == symbolKind)
		{
			flag3 = true;
		}
		else if (list.Count >= 1 && list[0] == symbolKind)
		{
			flag3 = true;
		}
		else if (list.Count >= 2 && list[list.Count - 2] == symbolKind)
		{
			flag = true;
		}
		else if (list.Count >= 1 && list[list.Count - 1] == symbolKind)
		{
			flag = true;
		}
		else
		{
			flag2 = true;
		}
		if (flag3)
		{
			switch (pKind)
			{
			case PatternScript.Kind.jackpot:
				return SlotMachineScript.SensationalLevel.highJackpot;
			case PatternScript.Kind.horizontal2:
				return SlotMachineScript.SensationalLevel.noone;
			case PatternScript.Kind.horizontal3:
				return SlotMachineScript.SensationalLevel.highNice;
			case PatternScript.Kind.horizontal4:
				return SlotMachineScript.SensationalLevel.highNice;
			case PatternScript.Kind.horizontal5:
				return SlotMachineScript.SensationalLevel.highGreat;
			case PatternScript.Kind.vertical2:
				return SlotMachineScript.SensationalLevel.noone;
			case PatternScript.Kind.vertical3:
				return SlotMachineScript.SensationalLevel.highNice;
			case PatternScript.Kind.diagonal2:
				return SlotMachineScript.SensationalLevel.noone;
			case PatternScript.Kind.diagonal3:
				return SlotMachineScript.SensationalLevel.highNice;
			case PatternScript.Kind.pyramid:
				return SlotMachineScript.SensationalLevel.highGreat;
			case PatternScript.Kind.pyramidInverted:
				return SlotMachineScript.SensationalLevel.highGreat;
			case PatternScript.Kind.triangle:
				return SlotMachineScript.SensationalLevel.highFantastic;
			case PatternScript.Kind.triangleInverted:
				return SlotMachineScript.SensationalLevel.highFantastic;
			case PatternScript.Kind.snakeUpDown:
				return SlotMachineScript.SensationalLevel.highFantastic;
			case PatternScript.Kind.snakeDownUp:
				return SlotMachineScript.SensationalLevel.highFantastic;
			case PatternScript.Kind.eye:
				return SlotMachineScript.SensationalLevel.highFantastic;
			default:
				Debug.Log("SlotMachineScript.GetPatternSensationalLevel(): high symbol pattern not handled: " + pKind.ToString());
				return SlotMachineScript.SensationalLevel.noone;
			}
		}
		else if (flag)
		{
			switch (pKind)
			{
			case PatternScript.Kind.jackpot:
				return SlotMachineScript.SensationalLevel.lowJackpot;
			case PatternScript.Kind.horizontal2:
				return SlotMachineScript.SensationalLevel.noone;
			case PatternScript.Kind.horizontal3:
				return SlotMachineScript.SensationalLevel.lowNice;
			case PatternScript.Kind.horizontal4:
				return SlotMachineScript.SensationalLevel.lowNice;
			case PatternScript.Kind.horizontal5:
				return SlotMachineScript.SensationalLevel.lowGreat;
			case PatternScript.Kind.vertical2:
				return SlotMachineScript.SensationalLevel.noone;
			case PatternScript.Kind.vertical3:
				return SlotMachineScript.SensationalLevel.lowNice;
			case PatternScript.Kind.diagonal2:
				return SlotMachineScript.SensationalLevel.noone;
			case PatternScript.Kind.diagonal3:
				return SlotMachineScript.SensationalLevel.lowNice;
			case PatternScript.Kind.pyramid:
				return SlotMachineScript.SensationalLevel.lowGreat;
			case PatternScript.Kind.pyramidInverted:
				return SlotMachineScript.SensationalLevel.lowGreat;
			case PatternScript.Kind.triangle:
				return SlotMachineScript.SensationalLevel.lowFantastic;
			case PatternScript.Kind.triangleInverted:
				return SlotMachineScript.SensationalLevel.lowFantastic;
			case PatternScript.Kind.snakeUpDown:
				return SlotMachineScript.SensationalLevel.lowFantastic;
			case PatternScript.Kind.snakeDownUp:
				return SlotMachineScript.SensationalLevel.lowFantastic;
			case PatternScript.Kind.eye:
				return SlotMachineScript.SensationalLevel.lowFantastic;
			default:
				Debug.Log("SlotMachineScript.GetPatternSensationalLevel(): low symbol pattern not handled: " + pKind.ToString());
				return SlotMachineScript.SensationalLevel.noone;
			}
		}
		else
		{
			if (!flag2)
			{
				Debug.Log("SlotMachineScript.GetPatternSensationalLevel(): couldn't compute a pattern high/medium/low sensational level. Code shouldn't get here!");
				return SlotMachineScript.SensationalLevel.noone;
			}
			switch (pKind)
			{
			case PatternScript.Kind.jackpot:
				return SlotMachineScript.SensationalLevel.mediumJackpot;
			case PatternScript.Kind.horizontal2:
				return SlotMachineScript.SensationalLevel.noone;
			case PatternScript.Kind.horizontal3:
				return SlotMachineScript.SensationalLevel.mediumNice;
			case PatternScript.Kind.horizontal4:
				return SlotMachineScript.SensationalLevel.mediumNice;
			case PatternScript.Kind.horizontal5:
				return SlotMachineScript.SensationalLevel.mediumGreat;
			case PatternScript.Kind.vertical2:
				return SlotMachineScript.SensationalLevel.noone;
			case PatternScript.Kind.vertical3:
				return SlotMachineScript.SensationalLevel.mediumNice;
			case PatternScript.Kind.diagonal2:
				return SlotMachineScript.SensationalLevel.noone;
			case PatternScript.Kind.diagonal3:
				return SlotMachineScript.SensationalLevel.mediumNice;
			case PatternScript.Kind.pyramid:
				return SlotMachineScript.SensationalLevel.mediumGreat;
			case PatternScript.Kind.pyramidInverted:
				return SlotMachineScript.SensationalLevel.mediumGreat;
			case PatternScript.Kind.triangle:
				return SlotMachineScript.SensationalLevel.mediumFantastic;
			case PatternScript.Kind.triangleInverted:
				return SlotMachineScript.SensationalLevel.mediumFantastic;
			case PatternScript.Kind.snakeUpDown:
				return SlotMachineScript.SensationalLevel.mediumFantastic;
			case PatternScript.Kind.snakeDownUp:
				return SlotMachineScript.SensationalLevel.mediumFantastic;
			case PatternScript.Kind.eye:
				return SlotMachineScript.SensationalLevel.mediumFantastic;
			default:
				Debug.Log("SlotMachineScript.GetPatternSensationalLevel(): medium symbol pattern not handled: " + pKind.ToString());
				return SlotMachineScript.SensationalLevel.noone;
			}
		}
	}

	// Token: 0x0600073B RID: 1851 RVA: 0x0002DDC8 File Offset: 0x0002BFC8
	private void SpinWinSetText(BigInteger coins)
	{
		this.SpinWinText_StopIfAny();
		this.spinWinScreenHolder.SetActive(true);
		this.textSpinWin.text = Translation.Get("SLOT_MAIN_TOTAL_SPIN_WIN") + "\n" + coins.ToStringSmart();
		Sound.Play3D("SoundSlotMachineSpinWin", this.Audio3dPosition, 10f, 1f, 1f, 1);
		this.bounceScript.SetBounceScale(0.015f);
		Controls.VibrationSet_PreferMax(this.player, 0.25f);
		this.spinWinTextCoroutine = base.StartCoroutine(this._SpinWinTextCoroutine());
	}

	// Token: 0x0600073C RID: 1852 RVA: 0x0002DE60 File Offset: 0x0002C060
	private void SpinWinText_StopIfAny()
	{
		if (this.spinWinTextCoroutine != null)
		{
			base.StopCoroutine(this.spinWinTextCoroutine);
			this.spinWinTextCoroutine = null;
		}
		this.textSpinWin.text = "";
		this.spinWinScreenHolder.SetActive(false);
		Sound.Stop("SoundSlotMachineSpinWin", true);
	}

	// Token: 0x0600073D RID: 1853 RVA: 0x0002DEAF File Offset: 0x0002C0AF
	public bool IsSpinWinTextPlaying()
	{
		return this.spinWinTextCoroutine != null;
	}

	// Token: 0x0600073E RID: 1854 RVA: 0x0002DEBA File Offset: 0x0002C0BA
	private IEnumerator _SpinWinTextCoroutine()
	{
		float timer = 1f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		this.SpinWinText_StopIfAny();
		yield break;
	}

	// Token: 0x0600073F RID: 1855 RVA: 0x0002DECC File Offset: 0x0002C0CC
	public TextMeshProUGUI GetScoreTextFromPool(string textStr, bool enable)
	{
		if (this.scoreTextsPool.Count == 0)
		{
			TextMeshProUGUI textMeshProUGUI = Object.Instantiate<TextMeshProUGUI>(this.templateSlotScoreText, this.slotMachineCanvas.transform);
			textMeshProUGUI.transform.localScale = Vector3.one;
			textMeshProUGUI.text = textStr;
			textMeshProUGUI.gameObject.SetActive(true);
			this.scoreTextsActive.Add(textMeshProUGUI);
			return textMeshProUGUI;
		}
		TextMeshProUGUI textMeshProUGUI2 = this.scoreTextsPool[this.scoreTextsPool.Count - 1];
		this.scoreTextsPool.RemoveAt(this.scoreTextsPool.Count - 1);
		this.scoreTextsActive.Add(textMeshProUGUI2);
		if (enable)
		{
			textMeshProUGUI2.gameObject.SetActive(true);
		}
		textMeshProUGUI2.text = textStr;
		return textMeshProUGUI2;
	}

	// Token: 0x06000740 RID: 1856 RVA: 0x0002DF82 File Offset: 0x0002C182
	public void ScoreTextDestroy(TextMeshProUGUI text)
	{
		this.scoreTextsActive.Remove(text);
		this.scoreTextsPool.Add(text);
		text.gameObject.SetActive(false);
	}

	// Token: 0x06000741 RID: 1857 RVA: 0x0002DFA9 File Offset: 0x0002C1A9
	private IEnumerator BootUpCoroutine()
	{
		int num = GameplayMaster.SlotAnimationCoinsGet().CastToInt();
		float TIME_TOT = 0.75f;
		int iterations = Mathf.Min(num, 10);
		TIME_TOT += (float)Mathf.Max(0, iterations - 5) / 5f * 0.15f;
		int num2;
		for (int i = 0; i < iterations; i = num2 + 1)
		{
			if (i > 0)
			{
				float timer = TIME_TOT / (float)iterations;
				while (timer > 0f)
				{
					timer -= Tick.Time;
					yield return null;
				}
			}
			this.PlayInsertCoinAnimation((float)i * 0.05f);
			num2 = i;
		}
		yield return this.WaitForTriggerAnimation();
		this.bootUpCoroutine = null;
		this._state = SlotMachineScript.State.idle;
		yield break;
	}

	// Token: 0x06000742 RID: 1858 RVA: 0x0002DFB8 File Offset: 0x0002C1B8
	private void Set_NoMoreSpins()
	{
		this._state = SlotMachineScript.State.noMoreCoins;
		Sound.Play("SoundSlotNoMoreCoins", 1f, 1f);
		Controls.VibrationSet_PreferMax(this.player, 0.25f);
		this.textNoMoreCoins.text = Translation.Get("SLOT_MAIN_NO_MORE_SPINS");
		this.SetTopScreenText(Translation.Get("SLOT_TOP_SCREEN_GAME_OVER"), false);
		this.noMoreSpinsScreenHolder.SetActive(true);
		SlotMachineScript.Event onRoundEnd = this.OnRoundEnd;
		if (onRoundEnd != null)
		{
			onRoundEnd();
		}
		this.noMoreCoinsCoroutine = base.StartCoroutine(this.NoMoreSpinsCoroutine());
	}

	// Token: 0x06000743 RID: 1859 RVA: 0x0002E046 File Offset: 0x0002C246
	private IEnumerator NoMoreSpinsCoroutine()
	{
		this.textNoMoreCoins.enabled = true;
		bool flicker = false;
		float timer = 0f;
		int flickerTimes = 5;
		while (flickerTimes > 0)
		{
			int num = flickerTimes;
			flickerTimes = num - 1;
			flicker = !flicker;
			if (this.textNoMoreCoins.enabled != flicker)
			{
				this.textNoMoreCoins.enabled = flicker;
			}
			if (flicker)
			{
				Sound.Play("SoundBeepFlicker", 1f, 1f);
			}
			timer = 0.35f;
			if (!flicker)
			{
				timer = 0.1f;
			}
			while (timer > 0f)
			{
				timer -= Tick.Time;
				yield return null;
			}
		}
		yield return this.WaitForTriggerAnimation();
		this.TurnOff(false);
		this.Stop_NoMoreSpins();
		yield break;
	}

	// Token: 0x06000744 RID: 1860 RVA: 0x0002E055 File Offset: 0x0002C255
	private void Stop_NoMoreSpins()
	{
		if (this.noMoreCoinsCoroutine != null)
		{
			base.StopCoroutine(this.noMoreCoinsCoroutine);
			this.noMoreCoinsCoroutine = null;
		}
		Sound.Stop("SoundSlotNoMoreCoins", true);
		this.noMoreSpinsScreenHolder.SetActive(false);
	}

	// (get) Token: 0x06000745 RID: 1861 RVA: 0x0002E089 File Offset: 0x0002C289
	private int TopScreenMaxChars
	{
		get
		{
			if (Data.settings.language == Translation.Language.Japanese)
			{
				return 5;
			}
			return 11;
		}
	}

	// Token: 0x06000746 RID: 1862 RVA: 0x0002E09D File Offset: 0x0002C29D
	public void SetTopScreenText(string text, bool flashWhite = false)
	{
		if (this.topScreenString == text)
		{
			return;
		}
		this.topScreenStringOffset = 0;
		this.topScreenString = text;
		this.topScreenStringDouble = text + " ~ " + text;
		this.flashWhite = flashWhite;
	}

	// Token: 0x06000747 RID: 1863 RVA: 0x0002E0D8 File Offset: 0x0002C2D8
	private void TopTextUpdate()
	{
		if (!this.flashWhite)
		{
			if (this.ledTextTop.color != this.topTextColor_Orange)
			{
				this.ledTextTop.color = this.topTextColor_Orange;
			}
		}
		else
		{
			this.flashTimer += Tick.Time * 2f;
			this.flashTimer = Mathf.Repeat(this.flashTimer, 1f);
			if (Util.AngleSin(this.flashTimer * 360f) > 0f)
			{
				if (this.ledTextTop.color != this.topTextColor_White)
				{
					this.ledTextTop.color = this.topTextColor_White;
				}
				this.topScreenString = this.topScreenString.Replace("€", "$");
			}
			else
			{
				if (this.ledTextTop.color != this.topTextColor_Orange)
				{
					this.ledTextTop.color = this.topTextColor_Orange;
				}
				this.topScreenString = this.topScreenString.Replace("$", "€");
			}
		}
		if (this.topScreenString.Length <= this.TopScreenMaxChars)
		{
			this.topScreenStringOffset = 0;
			Strings.SetTemporaryFlag_Sanitize666And999(1);
			this.ledTextTop.text = Strings.Sanitize(Strings.SantizationKind.ui, this.topScreenString, Strings.SanitizationSubKind.none);
			this.topScreenOffsetTimer = 0f;
			return;
		}
		this.topScreenOffsetTimer -= Tick.Time;
		if (this.topScreenOffsetTimer > 0f)
		{
			return;
		}
		this.topScreenOffsetTimer = 0.25f;
		if (this.topScreenStringOffset >= this.topScreenString.Length + " ~ ".Length)
		{
			this.topScreenStringOffset = 0;
		}
		string text = this.topScreenStringDouble.Substring(this.topScreenStringOffset, this.topScreenStringDouble.Length - this.topScreenStringOffset);
		if (text.Length > this.TopScreenMaxChars)
		{
			text = text.Substring(0, this.TopScreenMaxChars);
		}
		Strings.SetTemporaryFlag_Sanitize666And999(1);
		this.ledTextTop.text = Strings.Sanitize(Strings.SantizationKind.ui, text, Strings.SanitizationSubKind.none);
		this.topScreenStringOffset++;
		if (this.topScreenStringOffset >= this.topScreenString.Length + " ~ ".Length)
		{
			this.topScreenStringOffset = 0;
			SlotMachineScript.Event @event = this.onTopText_LoopsAround_Temp;
			if (@event != null)
			{
				@event();
			}
			SlotMachineScript.Event event2 = this.onTopText_LoopsAround_Permanent;
			if (event2 != null)
			{
				event2();
			}
			this.onTopText_LoopsAround_Temp = null;
		}
	}

	// Token: 0x06000748 RID: 1864 RVA: 0x0002E340 File Offset: 0x0002C540
	private void TopTextSet_BetMultiplier_DEPRECATED()
	{
		BigInteger bigInteger = GameplayData.AllSymbolsMultiplierGet(true) * GameplayData.AllPatternsMultiplierGet(true);
		this.SetTopScreenText("X" + bigInteger.ToStringSmart(), false);
	}

	// Token: 0x06000749 RID: 1865 RVA: 0x0002E378 File Offset: 0x0002C578
	public void TopTextSet_666Or999_ChancesShow()
	{
		if (!(GameplayData.DebtIndexGet() < GameplayData.SixSixSix_GetMinimumDebtIndex()))
		{
			this.SetTopScreenText("666 #" + GameplayData.SixSixSix_ChanceGet_AsPercentage(true).ToString("0.0") + "%", false);
			return;
		}
		if (SlotMachineScript.IsSpinningBeforeCoinsReward())
		{
			this.SetTopScreenText("...", false);
			return;
		}
		this.TopTextSet_Spin();
	}

	// Token: 0x0600074A RID: 1866 RVA: 0x0002E3DA File Offset: 0x0002C5DA
	private void TopTextSet_Spin()
	{
		this.SetTopScreenText(Translation.Get("SLOT_TOP_SCREEN_SPIN"), false);
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x0002E3F0 File Offset: 0x0002C5F0
	public void TopTextSet_RoundsOverMaxRounds()
	{
		int num = GameplayData.RoundsOfDeadline_PlayedGet();
		int num2 = GameplayData.RoundsOfDeadline_TotalGet();
		this.SetTopScreenText(string.Concat(new string[]
		{
			Translation.Get("SLOT_TOP_SCREEN_ROUND"),
			" ",
			(num + 1).ToString(),
			"/",
			num2.ToString()
		}), false);
	}

	// Token: 0x0600074C RID: 1868 RVA: 0x0002E450 File Offset: 0x0002C650
	public void TopTextSet_BetCost(bool forceUpdate)
	{
		int hypotehticalMaxSpinsBuyable = GameplayData.GetHypotehticalMaxSpinsBuyable();
		BigInteger bigInteger = GameplayData.SpinCostGet_Single() * hypotehticalMaxSpinsBuyable;
		string text = hypotehticalMaxSpinsBuyable.ToString() + "@ " + bigInteger.ToStringSmart() + "€";
		bool flag = bigInteger == this.betCostOld;
		bool flag2 = this.topScreenString == text;
		if (!forceUpdate && flag && flag2)
		{
			return;
		}
		if (!forceUpdate && this.ledTextTop.text == text)
		{
			return;
		}
		this.SetTopScreenText(text, false);
		this.betCostUpdateTimer -= Tick.Time;
		if (this.betCostUpdateTimer > 0f && !forceUpdate)
		{
			return;
		}
		this.betCostUpdateTimer = 0.5f;
		this.betCostOld = bigInteger;
	}

	// Token: 0x0600074D RID: 1869 RVA: 0x0002E50C File Offset: 0x0002C70C
	private void UpdateTopText_BetCost()
	{
		if (this._state == SlotMachineScript.State.off)
		{
			this.TopTextSet_BetCost(false);
		}
	}

	// Token: 0x0600074E RID: 1870 RVA: 0x0002E520 File Offset: 0x0002C720
	private void EffectsInitialization()
	{
		this.effect_LeverSparks.SetActive(false);
		this.SpecialPatternImageReset();
		this.jackpotGlowHolder.SetActive(false);
		GameObject[] array = this.jackpotGlowParticleHolders;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		this.JackpotLightReset();
		this.confettiHolder.SetActive(false);
		array = this.allArroundSparks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		array = this.steamOffParticles;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		array = this.steamBurningParticles;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		this.JackpotGalaxyReset();
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x0002E5DB File Offset: 0x0002C7DB
	private void EffectsUpdate()
	{
		this.BurningLevelAnimationUpdate();
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x0002E5E4 File Offset: 0x0002C7E4
	public static void EffectPlay_LeverSparks()
	{
		if (SlotMachineScript.instance == null)
		{
			return;
		}
		Sound.Play3D("SoundSpark", SlotMachineScript.instance.effect_LeverSparks.transform.position, 5f, 1f, 1f, 1);
		SlotMachineScript.instance.effect_LeverSparks.SetActive(true);
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x0002E640 File Offset: 0x0002C840
	public static void ShowSpecialPatternImage(PatternScript.Kind patternKind)
	{
		if (SlotMachineScript.instance.specialPatternImageCoroutine != null)
		{
			return;
		}
		SlotMachineScript.instance.specialPatternImagesHolder.localScale = Vector3.one;
		SpriteRenderer spriteRenderer;
		switch (patternKind)
		{
		case PatternScript.Kind.triangle:
			spriteRenderer = SlotMachineScript.instance.specialPSpriteRend_Above;
			SlotMachineScript.instance.specialPSpriteRend_Above.enabled = true;
			SlotMachineScript.instance.specialPSpriteRend_Below.enabled = false;
			SlotMachineScript.instance.specialPSpriteRend_Eye.enabled = false;
			goto IL_011C;
		case PatternScript.Kind.triangleInverted:
			spriteRenderer = SlotMachineScript.instance.specialPSpriteRend_Below;
			SlotMachineScript.instance.specialPSpriteRend_Above.enabled = false;
			SlotMachineScript.instance.specialPSpriteRend_Below.enabled = true;
			SlotMachineScript.instance.specialPSpriteRend_Eye.enabled = false;
			goto IL_011C;
		case PatternScript.Kind.eye:
			spriteRenderer = SlotMachineScript.instance.specialPSpriteRend_Eye;
			SlotMachineScript.instance.specialPSpriteRend_Above.enabled = false;
			SlotMachineScript.instance.specialPSpriteRend_Below.enabled = false;
			SlotMachineScript.instance.specialPSpriteRend_Eye.enabled = true;
			goto IL_011C;
		}
		Debug.LogError("SlotMachineScript.ShowSpecialPatternImage(): pattern not handled: " + patternKind.ToString());
		return;
		IL_011C:
		SlotMachineScript.instance.specialPatternImageCoroutine = SlotMachineScript.instance.StartCoroutine(SlotMachineScript.instance.SpecialPatternImageCoroutine(spriteRenderer));
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x0002E788 File Offset: 0x0002C988
	private IEnumerator SpecialPatternImageCoroutine(SpriteRenderer choosenSpriteRenderer)
	{
		SlotMachineScript.instance.specialPatternImagesHolder.gameObject.SetActive(true);
		Color c = Color.white;
		float scale = 0f;
		while (scale < 1f)
		{
			scale += Tick.Time * 4f;
			this.specialPatternImagesHolder.localScale = Vector3.one * (1f + scale * 0.1f);
			c.a = 1f - scale;
			choosenSpriteRenderer.color = c;
			yield return null;
		}
		scale = Mathf.Min(scale, 1f);
		this.specialPatternImagesHolder.localScale = Vector3.one * (1f + scale * 0.1f);
		c.a = 1f - scale;
		choosenSpriteRenderer.color = c;
		this.SpecialPatternImageReset();
		yield break;
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x0002E79E File Offset: 0x0002C99E
	private void SpecialPatternImageReset()
	{
		this.specialPatternImageCoroutine = null;
		SlotMachineScript.instance.specialPatternImagesHolder.gameObject.SetActive(false);
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x0002E7BC File Offset: 0x0002C9BC
	private void JackpotGlowShow(int particlesIntensity_0To2)
	{
		if (this.shrinkCoroutine != null)
		{
			base.StopCoroutine(this.shrinkCoroutine);
			this.shrinkCoroutine = null;
		}
		this.jackpotGlowHolder.SetActive(true);
		this.jackpotGlowScaler.localScale = Vector3.one;
		for (int i = 0; i < this.jackpotGlowParticleHolders.Length; i++)
		{
			bool flag = i <= particlesIntensity_0To2;
			this.jackpotGlowParticleHolders[i].SetActive(flag);
		}
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x0002E829 File Offset: 0x0002CA29
	private void JackpotGlowShrink()
	{
		if (this.shrinkCoroutine != null)
		{
			return;
		}
		this.shrinkCoroutine = base.StartCoroutine(this.JackpotGlowShrinkCoroutine());
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x0002E846 File Offset: 0x0002CA46
	private IEnumerator JackpotGlowShrinkCoroutine()
	{
		while (this.jackpotGlowScaler.localScale.z > 0.1f)
		{
			this.jackpotGlowScaler.AddLocalZScale(-this.jackpotGlowScaler.GetLocalZScale() * Tick.Time * 20f);
			yield return null;
		}
		this.jackpotGlowHolder.SetActive(false);
		this.shrinkCoroutine = null;
		yield break;
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x0002E855 File Offset: 0x0002CA55
	private IEnumerator JackpotGlowVibration()
	{
		float falloffValue = 1f;
		while (falloffValue > 0f)
		{
			falloffValue -= Tick.Time * 0.5f;
			Controls.VibrationSet_PreferMax(this.player, falloffValue);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x0002E864 File Offset: 0x0002CA64
	private void JackpotLightReset()
	{
		this.jackpotLight.intensity = 2f;
		this.jackpotLight.enabled = false;
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x0002E882 File Offset: 0x0002CA82
	private void JackpotLightSet(float intensity)
	{
		this.jackpotLight.intensity = intensity;
		this.jackpotLight.enabled = true;
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x0002E89C File Offset: 0x0002CA9C
	public void AllArroundSparksSet(int repeatN)
	{
		repeatN = Mathf.Clamp(repeatN, 0, this.allArroundSparks.Length);
		for (int i = 0; i < repeatN; i++)
		{
			this.allArroundSparks[Random.Range(0, this.allArroundSparks.Length)].SetActive(true);
		}
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x0002E8E1 File Offset: 0x0002CAE1
	private Material[] BurningLevelGetMaterials_Slot()
	{
		if (SlotMachineScript.HasGoldenKnob())
		{
			return this.materialBurningSlotMachine_GoldenKnobAlt;
		}
		return this.materialBurningSlotMachine;
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x0002E8F7 File Offset: 0x0002CAF7
	private Material[] BurningLevelGetMaterials_Knob()
	{
		if (SlotMachineScript.HasGoldenKnob())
		{
			return this.materialBurningKnob_GoldenKnobAlt;
		}
		return this.materialBurningKnob;
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x0002E910 File Offset: 0x0002CB10
	private void BurningLevelAnimationUpdate()
	{
		this._burningLevelAnimationTimer -= Tick.Time;
		if (this._burningLevelAnimationTimer <= 0f)
		{
			this._burningLevelAnimationTimer = 0.1f;
			if (this._burningLevelAnimationIndex > this._burningLevel)
			{
				this._burningLevelAnimationIndex--;
			}
			else if (this._burningLevelAnimationIndex < this._burningLevel)
			{
				this._burningLevelAnimationIndex++;
			}
			this.meshRendererSlotMachine.sharedMaterial = this.BurningLevelGetMaterials_Slot()[this._burningLevelAnimationIndex];
			this.meshRendererLever.sharedMaterial = this.BurningLevelGetMaterials_Knob()[this._burningLevelAnimationIndex];
		}
		if (SlotMachineScript.HasGoldenKnob())
		{
			float num = Tick.PassedTime * 360f;
			float num2 = Util.AngleSin(num) * 0.5f + 0.5f;
			Color color = Color.Lerp(SlotMachineScript.C_KNOB_NO_EMISSION, SlotMachineScript.C_KNOB_FULL_EMISSION, num2);
			this.meshRendererLever.sharedMaterial.SetColor("_Emission", color);
			this.goldenKnobLight.intensity = 2f + 1.5f * Util.AngleSin(num);
		}
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x0002EA1C File Offset: 0x0002CC1C
	public void BurningLevelSet(BigInteger currentSpinCoins)
	{
		BigInteger bigInteger = GameplayData.DebtGet();
		BigInteger bigInteger2 = currentSpinCoins * 10 / bigInteger;
		if (bigInteger2 >= 40L)
		{
			this._burningLevel = 7;
		}
		else if (bigInteger2 >= 30L)
		{
			this._burningLevel = 6;
		}
		else if (bigInteger2 >= 20L)
		{
			this._burningLevel = 5;
		}
		else if (bigInteger2 >= 15L)
		{
			this._burningLevel = 4;
		}
		else if (bigInteger2 >= 9L)
		{
			this._burningLevel = 3;
		}
		else if (bigInteger2 >= 5L)
		{
			this._burningLevel = 2;
		}
		else if (bigInteger2 >= 2L)
		{
			this._burningLevel = 1;
		}
		else
		{
			this._burningLevel = 0;
		}
		switch (this._burningLevel)
		{
		case 0:
		{
			GameObject[] array = this.steamBurningParticles;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			return;
		}
		case 1:
		case 2:
			this.steamBurningParticles[0].SetActive(true);
			return;
		case 3:
		case 4:
			this.steamBurningParticles[1].SetActive(true);
			return;
		case 5:
		case 6:
			this.steamBurningParticles[2].SetActive(true);
			return;
		case 7:
			this.steamBurningParticles[3].SetActive(true);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x0002EB64 File Offset: 0x0002CD64
	public void BurningLevelSteamOff()
	{
		if (this._burningLevel > 0)
		{
			for (int i = 0; i < this.steamOffParticles.Length; i++)
			{
				bool flag = this._burningLevel - 1 >= i;
				this.steamOffParticles[i].SetActive(flag);
			}
			FlashScreen.SpawnCamera(Color.white, 0.5f, 2f, CameraGame.firstInstance.myCamera, 0.5f);
			Sound.Play3D("SoundSlotMachineSteamOff", this.Audio3dPosition, 10f, 1f, 1f, 1);
		}
		GameObject[] array = this.steamBurningParticles;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].SetActive(false);
		}
		this._burningLevel = 0;
		this._burningLevelAnimationIndex = 0;
		this.meshRendererSlotMachine.sharedMaterial = this.BurningLevelGetMaterials_Slot()[this._burningLevel];
		this.meshRendererLever.sharedMaterial = this.BurningLevelGetMaterials_Slot()[this._burningLevel];
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x0002EC4C File Offset: 0x0002CE4C
	public void JackpotGalaxyReset()
	{
		for (int i = 0; i < this.jackpotGalaxyParticles.Length; i++)
		{
			this.jackpotGalaxyParticles[i].SetActive(false);
		}
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x0002EC7C File Offset: 0x0002CE7C
	public void JackpotGalaxySetLevel(int level0To7)
	{
		level0To7 = Mathf.Clamp(level0To7, 0, 7);
		if (level0To7 <= 0)
		{
			this.JackpotGalaxyReset();
		}
		for (int i = 0; i < level0To7; i++)
		{
			this.jackpotGalaxyParticles[i].SetActive(true);
		}
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x0002ECB8 File Offset: 0x0002CEB8
	public void PlayInsertCoinAnimation(float pitchMod)
	{
		this.insertCoinHolder.SetLocalZ(1f);
		float num = 0.5f;
		Sound.Play("SoundCoinDeposit", num, 1f + pitchMod);
		Controls.VibrationSet_PreferMax(this.player, 0.25f + pitchMod);
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x0002ED04 File Offset: 0x0002CF04
	private void Awake()
	{
		SlotMachineScript.instance = this;
		this.coinsVisualizers = base.GetComponentsInChildren<CoinVisualizerScript>();
		this.ReplacementSquaresInit();
		this.slotCamera = base.GetComponentInChildren<Camera>();
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.UpdateTopText_BetCost));
		this.materialBurningKnob = new Material[this.materialBurningSlotMachine.Length];
		this.materialBurningKnob_GoldenKnobAlt = new Material[this.materialBurningSlotMachine_GoldenKnobAlt.Length];
		for (int i = 0; i < this.materialBurningKnob.Length; i++)
		{
			this.materialBurningKnob[i] = new Material(this.materialBurningSlotMachine[i]);
			this.materialBurningKnob_GoldenKnobAlt[i] = new Material(this.materialBurningSlotMachine_GoldenKnobAlt[i]);
		}
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x0002EDB8 File Offset: 0x0002CFB8
	private void Start()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		this.player = Controls.GetPlayerByIndex(0);
		this.TurnOff(true);
		this.insertCoinHolder.SetLocalZ(0f);
		List<SymbolScript.Kind> list = GameplayData.SymbolsAvailable_GetAll(true);
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				this.lines[i][j] = list[R.Rng_Garbage.Range(0, list.Count)];
				this.linesOld[i][j] = this.lines[i][j];
			}
		}
		this._SymbolsSpawn(true, false);
		this.templateSlotScoreText.gameObject.SetActive(false);
		this.ScoreSquareEnableSetAll(false);
		this.SpinWinText_StopIfAny();
		this.EffectsInitialization();
		if (Data.game.RunModifier_WonTimes_Get(RunModifierScript.Identifier.bigDebt) > 0)
		{
			this.hasGoldenKnob = true;
		}
		this.goldenKnobLight.enabled = this.hasGoldenKnob;
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x0002EE9C File Offset: 0x0002D09C
	private void OnDestroy()
	{
		if (SlotMachineScript.instance == this)
		{
			SlotMachineScript.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.UpdateTopText_BetCost));
		for (int i = 0; i < this.materialBurningKnob.Length; i++)
		{
			Object.Destroy(this.materialBurningKnob[i]);
			Object.Destroy(this.materialBurningKnob_GoldenKnobAlt[i]);
		}
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x0002EF0C File Offset: 0x0002D10C
	private void Update()
	{
		if (!Tick.IsGameRunning)
		{
			return;
		}
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		this.TopTextUpdate();
		this.EffectsUpdate();
		this.scoreTextFlashingTimer += Tick.Time * 720f;
		this.scoreTextFlashingTimer = Mathf.Repeat(this.scoreTextFlashingTimer, 360f);
		bool flag = Util.AngleSin(this.scoreTextFlashingTimer) > 0f;
		TextMeshProUGUI textMeshProUGUI;
		for (int i = 0; i < this.scoreTextsActive.Count; i++)
		{
			textMeshProUGUI = this.scoreTextsActive[i];
			if (flag)
			{
				textMeshProUGUI.fontSharedMaterial = this.fontMaterial_ScoreYellow;
			}
			else
			{
				textMeshProUGUI.fontSharedMaterial = this.fontMaterial_ScoreOrange;
			}
		}
		textMeshProUGUI = this.textSpinWin;
		if (flag)
		{
			textMeshProUGUI.fontSharedMaterial = this.fontMaterial_ScoreYellow;
		}
		else
		{
			textMeshProUGUI.fontSharedMaterial = this.fontMaterial_ScoreOrange;
		}
		float num = 0f;
		if (CameraDebug.IsEnabled())
		{
			num = 0.175f;
		}
		this.insertCoinHolder.SetLocalZ(Mathf.Lerp(this.insertCoinHolder.GetLocalZ(), num, Tick.Time * 20f));
		bool flag2 = this._state == SlotMachineScript.State.idle || this._state == SlotMachineScript.State.spinning;
		if (this.menuControllerEnabled_Old == null || this.menuControllerEnabled_Old.Value != flag2)
		{
			if (!flag2)
			{
				this.myMenuController.NavigationDisable_SetReason("smd");
			}
			else
			{
				this.myMenuController.NavigationDisable_RemoveReason("smd");
			}
			this.menuControllerEnabled_Old = new bool?(flag2);
		}
		this.titleScreenTimer += Tick.Time;
		if (this._state == SlotMachineScript.State.off)
		{
			bool flag3 = true;
			if (ScreenMenuScript.IsEnabled() && CameraController.GetPositionKind() == CameraController.PositionKind.SlotScreenCloseUp)
			{
				flag3 = false;
			}
			if (this.titleScreenRawImage.enabled != flag3)
			{
				this.titleScreenRawImage.enabled = flag3;
			}
			this.titleScreenRawImage.rectTransform.anchoredPosition = new Vector2(0f, Util.AngleSin(this.titleScreenTimer * 180f) * 0.025f);
		}
		else if (this._state == SlotMachineScript.State.bootingUp)
		{
			bool flag4 = Util.AngleSin(this.titleScreenTimer * 720f) > -0.75f;
			if (this.titleScreenRawImage.enabled != flag4)
			{
				this.titleScreenRawImage.enabled = flag4;
			}
			this.titleScreenRawImage.rectTransform.anchoredPosition = Vector2.zero;
		}
		else if (this.titleScreenRawImage.enabled)
		{
			this.titleScreenRawImage.enabled = false;
		}
		bool flag5 = this._state == SlotMachineScript.State.idle || this._state == SlotMachineScript.State.spinning;
		if (this.gameUiHolder.activeSelf != flag5)
		{
			this.gameUiHolder.SetActive(flag5);
		}
		bool flag6 = this._state == SlotMachineScript.State.spinning || this._state == SlotMachineScript.State.idle;
		if (this.screenCollider.activeSelf != flag6)
		{
			this.screenCollider.SetActive(flag6);
		}
		if (!ScreenMenuScript.IsEnabled())
		{
			this.UpdateTopText_BetCost();
		}
		if (this._state != SlotMachineScript.State.idle)
		{
			return;
		}
		if (DialogueScript.IsEnabled())
		{
			return;
		}
		this.mainMenuInputDelay -= Tick.Time;
		if (MainMenuScript.IsEnabled())
		{
			this.mainMenuInputDelay = 0.5f;
		}
		if (this.mainMenuInputDelay <= 0f && Controls.ActionButton_PressedGet(0, Controls.InputAction.menuPause, true) && !CameraController.SlotMachineLookingAtSides())
		{
			GameplayMaster.instance.FCall_MenuDrawer_MainMenu_OpenTry();
		}
		if (MainMenuScript.IsEnabled())
		{
			return;
		}
		if (GameplayData.SpinsLeftGet() <= 0)
		{
			this.Set_NoMoreSpins();
		}
		if (this.autoSpin)
		{
			this.autoSpinDelay -= Tick.Time;
			if (this.autoSpinDelay <= 0f)
			{
				this.autoSpinDelay = 0.5f;
				GameplayMaster.instance.FCall_SlotSpinTry(true);
			}
		}
	}

	public static SlotMachineScript instance = null;

	private const int PLAYER_INDEX = 0;

	private const float INSET_COIN_LERP_SPEED = 20f;

	private const float TOP_SCREEN_UPDATE_INTERVAL = 0.25f;

	private const float AUTO_SPIN_DELAY = 0.5f;

	private const float SPIN_ANIMATION_SPEED = 0.5f;

	public AnimationCurve spinAnimationCurve;

	private const int ANIMATION_REPEAT_TIMES_MODIFIER = 2;

	private const int ANIMATION_REPEAT_TIMES_JACKPOT = 3;

	private const int ANIMATION_REPEAT_TIMES_666 = 3;

	private const int ANIMATION_REPEAT_TIMES_999 = 3;

	private const float SLOT_MACHINE_ANIM_SPEED_INCREMENT = 0.125f;

	private const float MAX_ANIM_SPEED = 1.75f;

	private const int MAX_LINES = 3;

	private const int MAX_COLUMNS = 5;

	private readonly Vector2 COLUMNS_END_POS = new Vector2(0f, 0f);

	private const float COLUMN_VERTICAL_UNIT = 0.5f;

	private const float LINE_HORIZONTAL_UNIT = 0.275f;

	private const int GARBAGE_SYMBOLS_PER_COLUMN = 24;

	private const int GARBAGE_SYMBOLS_PER_COLUMN_EXTRA = 12;

	private const float LUCK_ELEMENT_COPIES_ANOTHER = 0.0025f;

	private const int INITIAL_ROUNDS_OF_LUCK_BONUS = 31;

	public const int JACKPOT_LUCK_BONUS = 15;

	private const int SCORE_SQUARES_WIDTH = 5;

	private const float BOUNCE_MINIMUM = 0.00175f;

	private const float BOUNCE_REALLY_SMALL = 0.0035f;

	private const float BOUNCE_SMALL = 0.0075f;

	private const float BOUNCE_MEDIUM = 0.015f;

	private const float BOUNCE_HIGH = 0.025f;

	private static Color C_KNOB_NO_EMISSION = new Color(0f, 0f, 0f, 1f);

	private static Color C_KNOB_FULL_EMISSION = new Color(0.5f, 0.25f, 0f, 1f);

	private Controls.PlayerExt player;

	public DiegeticMenuController myMenuController;

	public DiegeticMenuElement leverMenuElement;

	public DiegeticMenuElement redButtonElement;

	public Transform insertCoinHolder;

	public TextMeshProUGUI ledTextTop;

	public BounceScript bounceScript;

	public GameObject gameUiHolder;

	public RawImage mainScreenRendererImage;

	public GameObject screenCollider;

	public GameObject noMoreSpinsScreenHolder;

	public TextMeshProUGUI textNoMoreCoins;

	public GameObject spinWinScreenHolder;

	public TextMeshProUGUI textSpinWin;

	public RawImage titleScreenRawImage;

	private Camera slotCamera;

	public Canvas slotMachineCanvas;

	public RectTransform[] columnsRectTr;

	private CoinVisualizerScript[] coinsVisualizers;

	public ButtonVisualizerScript leverButtonVisualizer;

	public TextMeshProUGUI templateSlotScoreText;

	public Material fontMaterial_ScoreYellow;

	public Material fontMaterial_ScoreOrange;

	public GameObject[] scoreSquares;

	public RectTransform[] replacementSquaresRectTr;

	private Vector2[] replacementSquaresStartingAnchoredPosition;

	public GameObject effect_LeverSparks;

	public Transform specialPatternImagesHolder;

	public SpriteRenderer specialPSpriteRend_Eye;

	public SpriteRenderer specialPSpriteRend_Above;

	public SpriteRenderer specialPSpriteRend_Below;

	public GameObject jackpotGlowHolder;

	public Transform jackpotGlowScaler;

	public GameObject[] jackpotGlowParticleHolders;

	public Light jackpotLight;

	public GameObject confettiHolder;

	public GameObject[] allArroundSparks;

	public MeshRenderer meshRendererSlotMachine;

	public MeshRenderer meshRendererLever;

	public Material[] materialBurningSlotMachine;

	public Material[] materialBurningSlotMachine_GoldenKnobAlt;

	private Material[] materialBurningKnob;

	private Material[] materialBurningKnob_GoldenKnobAlt;

	public GameObject[] steamOffParticles;

	public GameObject[] steamBurningParticles;

	public Light goldenKnobLight;

	public GameObject[] jackpotGalaxyParticles;

	private Vector3 audio3dOffset = new Vector3(0f, 1f, 0f);

	private SlotMachineScript.State _state;

	private bool autoSpin;

	private float autoSpinDelay = 0.5f;

	private float titleScreenTimer;

	private bool? menuControllerEnabled_Old;

	private const string MENU_DISABLED_REASON = "smd";

	private bool hasGoldenKnob;

	private BigInteger spinExtraCoins = 0;

	private bool _legalToReplaceSymbols;

	private int replaceVisibleSymbolsCallN;

	private bool _isFirstSpinOfRound = true;

	private bool _spinningBeforeCoinsReward;

	private bool _hasShown666;

	private BigInteger _666RoundLostCoins = 0;

	private bool _666GotCoinsRestoredFromJackpot;

	private bool _hasTransformedInto999;

	private int forcedLuckNext;

	private bool _isAllSamePattern;

	private PatternScript.Kind _biggestPatternScored = PatternScript.Kind.undefined;

	private SymbolScript.Kind[][] lines = new SymbolScript.Kind[][]
	{
		new SymbolScript.Kind[5],
		new SymbolScript.Kind[5],
		new SymbolScript.Kind[5]
	};

	private SymbolScript.Kind[][] linesOld = new SymbolScript.Kind[][]
	{
		new SymbolScript.Kind[5],
		new SymbolScript.Kind[5],
		new SymbolScript.Kind[5]
	};

	private float[] spinOffsetPerColumn = new float[5];

	private Coroutine spinCoroutine;

	private Coroutine spinFailsafeCoroutine;

	private List<Vector2Int> _luckPositions = new List<Vector2Int>();

	private List<Vector2Int> _aureolaChangedPositions = new List<Vector2Int>();

	private const float FAILSAFE_RESET_TIMER_VALUE = 40f;

	private float failSafeTimer = 40f;

	private List<Transform> elementsInColumn = new List<Transform>(39);

	private List<SymbolScript.Modifier> _modifiersToAnimate_Temp = new List<SymbolScript.Modifier>(4);

	private List<SlotMachineScript.PatternInfos> _patternInfos = new List<SlotMachineScript.PatternInfos>(8);

	private List<SlotMachineScript.PatternInfos> _PatInf_GetEnabledList = new List<SlotMachineScript.PatternInfos>(8);

	private List<SlotMachineScript.PatternInfos> _PatInf_GetByKindList = new List<SlotMachineScript.PatternInfos>(8);

	private List<Vector2Int> _patternCordsListTemp = new List<Vector2Int>(16);

	private List<string> _antiduplicateIdsList = new List<string>(16);

	private StringBuilder _pIdSb = new StringBuilder(64);

	private BigInteger _patternComputationResult_Coins = 0;

	private Coroutine spinWinTextCoroutine;

	private float scoreTextFlashingTimer;

	[NonSerialized]
	public List<TextMeshProUGUI> scoreTextsPool = new List<TextMeshProUGUI>();

	[NonSerialized]
	public List<TextMeshProUGUI> scoreTextsActive = new List<TextMeshProUGUI>();

	private Coroutine bootUpCoroutine;

	private Coroutine noMoreCoinsCoroutine;

	private bool flashWhite;

	private float flashTimer;

	private Color topTextColor_Orange = new Color(1f, 0.5f, 0f, 1f);

	private Color topTextColor_White = new Color(1f, 1f, 1f, 1f);

	private string topScreenString = "";

	private string topScreenStringDouble = "";

	private const string separator = " ~ ";

	private int topScreenStringOffset;

	private float topScreenOffsetTimer;

	private BigInteger betCostOld = -1;

	private float betCostUpdateTimer;

	private Coroutine specialPatternImageCoroutine;

	private Coroutine shrinkCoroutine;

	private int _burningLevel;

	private int _burningLevelAnimationIndex;

	private float _burningLevelAnimationTimer;

	private float mainMenuInputDelay = 0.5f;

	private const float MAIN_MENU_INPUT_DELAY = 0.5f;

	public SlotMachineScript.Event OnRoundBeing;

	public SlotMachineScript.Event OnRoundEnd;

	public SlotMachineScript.Event OnInterestEarn;

	public SlotMachineScript.Event OnInterestEarnPost;

	public SlotMachineScript.Event OnSpinPreLuckApplication;

	public SlotMachineScript.Event OnSpinStart;

	public SlotMachineScript.Event OnSpinEnd;

	public SlotMachineScript.Event OnScoreEvaluationBegin;

	public SlotMachineScript.Event OnScoreEvaluationEnd;

	public SlotMachineScript.Event OnScoreEvaluationEnd_Late;

	public SlotMachineScript.PatternEvent OnPatternEvaluationStart;

	public SlotMachineScript.PatternEvent OnPatternEvaluationEnd;

	public SlotMachineScript.PatternEvent On666;

	public SlotMachineScript.PatternEvent On999;

	public SlotMachineScript.PatternEvent OnModifierScored;

	public SlotMachineScript.Event onTopText_LoopsAround_Temp;

	public SlotMachineScript.Event onTopText_LoopsAround_Permanent;

	public enum State
	{
		off,
		bootingUp,
		noMoreCoins,
		idle,
		spinning
	}

	public class PatternInfos
	{
		// Token: 0x0600115A RID: 4442 RVA: 0x0006A508 File Offset: 0x00068708
		public void Setup(PatternScript.Kind _patternKind, BigInteger _coins, List<Vector2Int> _positionsToCopy)
		{
			if (_positionsToCopy.Count == 0)
			{
				Debug.LogError("PatternInfos.Setup() - Cannot setup a pattern with 0 positions!");
				return;
			}
			this.patternKind = _patternKind;
			this.coins = _coins;
			while (this.positions.Count < _positionsToCopy.Count)
			{
				this.positions.Add(Vector2Int.zero);
			}
			while (this.positions.Count > _positionsToCopy.Count)
			{
				this.positions.RemoveAt(this.positions.Count - 1);
			}
			for (int i = 0; i < _positionsToCopy.Count; i++)
			{
				this.positions[i] = _positionsToCopy[i];
			}
			this.symbolKind = SlotMachineScript.instance.Symbol_GetAtPosition(this.positions[0].x, this.positions[0].y);
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x0006A5E4 File Offset: 0x000687E4
		public Vector3 GetScoringPosition_World()
		{
			switch (this.patternKind)
			{
			case PatternScript.Kind.jackpot:
				return SlotMachineScript.instance.Symbol_GetInstanceAtPosition(2, 1).transform.position;
			case PatternScript.Kind.horizontal2:
				return (SlotMachineScript.instance.Symbol_GetInstanceAtPosition(this.positions[0].x, this.positions[0].y).transform.position + SlotMachineScript.instance.Symbol_GetInstanceAtPosition(this.positions[1].x, this.positions[1].y).transform.position) / 2f;
			case PatternScript.Kind.horizontal3:
				return SlotMachineScript.instance.Symbol_GetInstanceAtPosition(this.positions[1].x, this.positions[1].y).transform.position;
			case PatternScript.Kind.horizontal4:
				return (SlotMachineScript.instance.Symbol_GetInstanceAtPosition(this.positions[2].x, this.positions[2].y).transform.position + SlotMachineScript.instance.Symbol_GetInstanceAtPosition(this.positions[1].x, this.positions[1].y).transform.position) / 2f;
			case PatternScript.Kind.horizontal5:
				return SlotMachineScript.instance.Symbol_GetInstanceAtPosition(this.positions[2].x, this.positions[2].y).transform.position;
			case PatternScript.Kind.vertical2:
				return (SlotMachineScript.instance.Symbol_GetInstanceAtPosition(this.positions[0].x, this.positions[0].y).transform.position + SlotMachineScript.instance.Symbol_GetInstanceAtPosition(this.positions[1].x, this.positions[1].y).transform.position) / 2f;
			case PatternScript.Kind.vertical3:
				return SlotMachineScript.instance.Symbol_GetInstanceAtPosition(this.positions[1].x, this.positions[1].y).transform.position;
			case PatternScript.Kind.diagonal2:
				return (SlotMachineScript.instance.Symbol_GetInstanceAtPosition(this.positions[0].x, this.positions[0].y).transform.position + SlotMachineScript.instance.Symbol_GetInstanceAtPosition(this.positions[1].x, this.positions[1].y).transform.position) / 2f;
			case PatternScript.Kind.diagonal3:
				return SlotMachineScript.instance.Symbol_GetInstanceAtPosition(this.positions[1].x, this.positions[1].y).transform.position;
			case PatternScript.Kind.pyramid:
				return SlotMachineScript.instance.Symbol_GetInstanceAtPosition(2, 1).transform.position;
			case PatternScript.Kind.pyramidInverted:
				return SlotMachineScript.instance.Symbol_GetInstanceAtPosition(2, 1).transform.position;
			case PatternScript.Kind.triangle:
				return SlotMachineScript.instance.Symbol_GetInstanceAtPosition(2, 1).transform.position;
			case PatternScript.Kind.triangleInverted:
				return SlotMachineScript.instance.Symbol_GetInstanceAtPosition(2, 1).transform.position;
			case PatternScript.Kind.snakeUpDown:
				return SlotMachineScript.instance.Symbol_GetInstanceAtPosition(2, 1).transform.position;
			case PatternScript.Kind.snakeDownUp:
				return SlotMachineScript.instance.Symbol_GetInstanceAtPosition(2, 1).transform.position;
			case PatternScript.Kind.eye:
				return SlotMachineScript.instance.Symbol_GetInstanceAtPosition(2, 1).transform.position;
			default:
				Debug.LogError("PatternInfos.GetScoringPosition_World() - Pattern kind not implemented yet! Pattern kind: " + this.patternKind.ToString());
				return Vector3.zero;
			}
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x0006AA34 File Offset: 0x00068C34
		public Vector3 GetMedianPlatePosition()
		{
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < this.positions.Count; i++)
			{
				zero.x += (float)this.positions[i].x;
				zero.y += (float)this.positions[i].y;
			}
			zero.x /= (float)this.positions.Count;
			zero.y /= (float)this.positions.Count;
			return zero;
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x0006AACC File Offset: 0x00068CCC
		public bool IsEqualToOtherPattern(SlotMachineScript.PatternInfos otherPattern)
		{
			if (otherPattern.symbolKind != this.symbolKind)
			{
				return false;
			}
			if (otherPattern.positions.Count != this.positions.Count)
			{
				return false;
			}
			for (int i = 0; i < otherPattern.positions.Count; i++)
			{
				if (!this.positions.Contains(otherPattern.positions[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x0006AB35 File Offset: 0x00068D35
		public ulong GetOrderWeight()
		{
			return SymbolScript.SymbolsOrderWeightMask(this.symbolKind) | PatternScript.PatternOrderWeightMask(this.patternKind);
		}

		public bool enabled;

		public PatternScript.Kind patternKind = PatternScript.Kind.undefined;

		public SymbolScript.Kind symbolKind = SymbolScript.Kind.undefined;

		public BigInteger coins = 0;

		public List<Vector2Int> positions = new List<Vector2Int>(16);

		public int repeatingNChached = 1;
	}

	public enum SensationalLevel
	{
		noone,
		lowNice,
		lowGreat,
		lowFantastic,
		lowJackpot,
		mediumNice,
		mediumGreat,
		mediumFantastic,
		mediumJackpot,
		highNice,
		highGreat,
		highFantastic,
		highJackpot,
		Count
	}

	// (Invoke) Token: 0x06001161 RID: 4449
	public delegate void Event();

	// (Invoke) Token: 0x06001165 RID: 4453
	public delegate void PatternEvent(SlotMachineScript.PatternInfos patternInfo);
}
