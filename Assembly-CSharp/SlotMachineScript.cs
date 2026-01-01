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

// Token: 0x0200006D RID: 109
public class SlotMachineScript : MonoBehaviour
{
	// Token: 0x0600079A RID: 1946 RVA: 0x0000C345 File Offset: 0x0000A545
	private void ScoreSquareEnableSet(int columnX, int lineY, bool enable)
	{
		if (this.scoreSquares[columnX + lineY * 5].activeSelf != enable)
		{
			this.scoreSquares[columnX + lineY * 5].SetActive(enable);
		}
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x0000C36D File Offset: 0x0000A56D
	private bool ScoreSquareEnabledGet(int columnX, int lineY)
	{
		return this.scoreSquares[columnX + lineY * 5].activeSelf;
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x0003EA94 File Offset: 0x0003CC94
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

	// Token: 0x0600079D RID: 1949 RVA: 0x0003EAD4 File Offset: 0x0003CCD4
	private void ReplacementSquaresInit()
	{
		this.replacementSquaresStartingAnchoredPosition = new global::UnityEngine.Vector2[this.replacementSquaresRectTr.Length];
		for (int i = 0; i < this.replacementSquaresRectTr.Length; i++)
		{
			this.replacementSquaresStartingAnchoredPosition[i] = this.replacementSquaresRectTr[i].anchoredPosition;
			this.replacementSquaresRectTr[i].gameObject.SetActive(false);
		}
	}

	// Token: 0x17000041 RID: 65
	// (get) Token: 0x0600079E RID: 1950 RVA: 0x0000C380 File Offset: 0x0000A580
	private global::UnityEngine.Vector3 Audio3dPosition
	{
		get
		{
			return base.transform.position + this.audio3dOffset;
		}
	}

	// Token: 0x17000042 RID: 66
	// (get) Token: 0x0600079F RID: 1951 RVA: 0x0000C398 File Offset: 0x0000A598
	private global::UnityEngine.Vector3 Audio3dPositionLow
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x0000C3A5 File Offset: 0x0000A5A5
	public static SlotMachineScript.State StateGet()
	{
		if (SlotMachineScript.instance == null)
		{
			return SlotMachineScript.State.off;
		}
		return SlotMachineScript.instance._state;
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x0000C3C0 File Offset: 0x0000A5C0
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

	// Token: 0x060007A2 RID: 1954 RVA: 0x0000C3F6 File Offset: 0x0000A5F6
	public static bool IsTurnedOn()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance._state > SlotMachineScript.State.off;
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x0003EB34 File Offset: 0x0003CD34
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

	// Token: 0x060007A4 RID: 1956 RVA: 0x0000C414 File Offset: 0x0000A614
	public bool IsAutoSpinning()
	{
		return this.autoSpin;
	}

	// Token: 0x060007A5 RID: 1957 RVA: 0x0000C41C File Offset: 0x0000A61C
	private void AutoSpinTopTextSet()
	{
		if (this.autoSpin)
		{
			this.SetTopScreenText(Translation.Get("SLOT_TOP_SCREEN_AUTO_MODE"), false);
			return;
		}
		this.SetTopScreenText(Translation.Get("SLOT_TOP_SCREEN_MANUAL_MODE"), false);
	}

	// Token: 0x060007A6 RID: 1958 RVA: 0x0003EBCC File Offset: 0x0003CDCC
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
		Sound.Play3D("SoundSlotMachineStartupJingle", this.Audio3dPosition, 10f, 1f, 1f, AudioRolloffMode.Linear);
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

	// Token: 0x060007A7 RID: 1959 RVA: 0x0003EC80 File Offset: 0x0003CE80
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
			Sound.Play3D("SoundSlotMachineTurnOff", this.Audio3dPosition, 10f, 1f, 1f, AudioRolloffMode.Linear);
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

	// Token: 0x060007A8 RID: 1960 RVA: 0x0000C449 File Offset: 0x0000A649
	private static bool HasGoldenKnob()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance.hasGoldenKnob;
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x0000C464 File Offset: 0x0000A664
	public static void SpinExtraCoinsAdd(BigInteger coins)
	{
		if (SlotMachineScript.instance == null)
		{
			return;
		}
		SlotMachineScript.instance.spinExtraCoins += coins;
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x0000C48A File Offset: 0x0000A68A
	public static bool IsReplacingSymbols()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance.replaceVisibleSymbolsCallN > 0;
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x0003ED24 File Offset: 0x0003CF24
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

	// Token: 0x060007AC RID: 1964 RVA: 0x0000C4A8 File Offset: 0x0000A6A8
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
			global::UnityEngine.Vector2 vector = this.replacementSquaresStartingAnchoredPosition[columnX + lineY * 5];
			this.replacementSquaresRectTr[columnX + lineY * 5].anchoredPosition = vector + new global::UnityEngine.Vector2(global::UnityEngine.Random.Range(-0.01f, 0.01f), global::UnityEngine.Random.Range(-0.01f, 0.01f));
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

	// Token: 0x060007AD RID: 1965 RVA: 0x0003ED9C File Offset: 0x0003CF9C
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

	// Token: 0x060007AE RID: 1966 RVA: 0x0003EDCC File Offset: 0x0003CFCC
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

	// Token: 0x060007AF RID: 1967 RVA: 0x0003EE0C File Offset: 0x0003D00C
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

	// Token: 0x060007B0 RID: 1968 RVA: 0x0000C4DC File Offset: 0x0000A6DC
	public static bool IsFirstSpinOfRound()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance._isFirstSpinOfRound;
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x0000C4F7 File Offset: 0x0000A6F7
	public static void FirstSpinFlagReset_ToTrue()
	{
		if (SlotMachineScript.instance == null)
		{
			return;
		}
		SlotMachineScript.instance._isFirstSpinOfRound = true;
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x0000C512 File Offset: 0x0000A712
	public static bool IsSpinning()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance._state == SlotMachineScript.State.spinning;
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x0000C530 File Offset: 0x0000A730
	public static bool IsSpinningBeforeCoinsReward()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance._spinningBeforeCoinsReward;
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x0003EE4C File Offset: 0x0003D04C
	public static bool Has666()
	{
		return !(SlotMachineScript.instance == null) && (SlotMachineScript.instance.lines[1][1] == SymbolScript.Kind.six && SlotMachineScript.instance.lines[1][2] == SymbolScript.Kind.six) && SlotMachineScript.instance.lines[1][3] == SymbolScript.Kind.six;
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x0000C54B File Offset: 0x0000A74B
	public static void RemoveVisible666()
	{
		SlotMachineScript.Symbol_ReplaceVisible(GameplayData.Symbol_GetRandom_BasedOnSymbolChance(), SymbolScript.Modifier.none, 1, 1, true);
		SlotMachineScript.Symbol_ReplaceVisible(GameplayData.Symbol_GetRandom_BasedOnSymbolChance(), SymbolScript.Modifier.none, 2, 1, true);
		SlotMachineScript.Symbol_ReplaceVisible(GameplayData.Symbol_GetRandom_BasedOnSymbolChance(), SymbolScript.Modifier.none, 3, 1, true);
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x0000C577 File Offset: 0x0000A777
	public static bool HasShown666()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance._hasShown666;
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x0000C592 File Offset: 0x0000A792
	public void _666RoundLostCoinsReset()
	{
		this._666RoundLostCoins = 0;
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x0000C5A0 File Offset: 0x0000A7A0
	private void _666RoundLostCoinsAdd(BigInteger n)
	{
		if (n < 0L)
		{
			n = -n;
		}
		this._666RoundLostCoins += n;
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x0000C5C6 File Offset: 0x0000A7C6
	private BigInteger _666RoundLostCoinsGet()
	{
		return this._666RoundLostCoins;
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x0003EEA0 File Offset: 0x0003D0A0
	public static bool Has999()
	{
		return !(SlotMachineScript.instance == null) && (SlotMachineScript.instance.lines[1][1] == SymbolScript.Kind.nine && SlotMachineScript.instance.lines[1][2] == SymbolScript.Kind.nine) && SlotMachineScript.instance.lines[1][3] == SymbolScript.Kind.nine;
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x0000C5CE File Offset: 0x0000A7CE
	public static bool HasShown999()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance._hasTransformedInto999;
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x0003EEF4 File Offset: 0x0003D0F4
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

	// Token: 0x060007BD RID: 1981 RVA: 0x0000C5E9 File Offset: 0x0000A7E9
	public void ForceNextLuck_Set(int luck)
	{
		this.forcedLuckNext = luck;
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x0000C5F2 File Offset: 0x0000A7F2
	public void ForceNextLuck_Add(int luck)
	{
		this.forcedLuckNext += luck;
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x0000C602 File Offset: 0x0000A802
	public void ForceNextLuck_Reset()
	{
		this.forcedLuckNext = 0;
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x0000C60B File Offset: 0x0000A80B
	public static bool IsAllSamePattern()
	{
		return !(SlotMachineScript.instance == null) && SlotMachineScript.instance._isAllSamePattern;
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x0000C626 File Offset: 0x0000A826
	public static PatternScript.Kind GetBiggestPatternScored()
	{
		if (SlotMachineScript.instance == null)
		{
			return PatternScript.Kind.undefined;
		}
		return SlotMachineScript.instance._biggestPatternScored;
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x0000C641 File Offset: 0x0000A841
	public SymbolScript.Kind Symbol_GetAtPosition(int columnX, int lineY)
	{
		return this.lines[lineY][columnX];
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x0000C64D File Offset: 0x0000A84D
	public SymbolScript Symbol_GetInstanceAtPosition(int columnX, int lineY)
	{
		return SymbolScript.GetSymbolScript_ByScoringPosition(new Vector2Int(columnX, lineY));
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x0003EF4C File Offset: 0x0003D14C
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

	// Token: 0x060007C5 RID: 1989 RVA: 0x0003EFF0 File Offset: 0x0003D1F0
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

	// Token: 0x060007C6 RID: 1990 RVA: 0x0000C65B File Offset: 0x0000A85B
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

	// Token: 0x060007C7 RID: 1991 RVA: 0x0000C66A File Offset: 0x0000A86A
	private void SpinFailsafeRestartTimer()
	{
		this.failSafeTimer = 40f;
	}

	// Token: 0x060007C8 RID: 1992 RVA: 0x0000C677 File Offset: 0x0000A877
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
		Sound.Play("SoundSlotLever", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
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
		Sound.Play3D("SoundSlotMachineRollingTick", this.Audio3dPosition, 10f, 1f, global::UnityEngine.Random.Range(0.95f, 1.05f), AudioRolloffMode.Linear);
		Sound.Play3D("SoundSlotMachineFanfare", this.Audio3dPosition, 10f, Mathf.Min(0.75f, (float)(GameplayData.SpinsLeftGet() + 1) * 0.075f), 1f, AudioRolloffMode.Linear);
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
				global::UnityEngine.Vector2 anchoredPosition = rectTransform.anchoredPosition;
				rectTransform.anchoredPosition = new global::UnityEngine.Vector2(rectTransform.anchoredPosition.x, num20);
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
				Sound.Play3D("SoundSlotMachineJackpot", this.Audio3dPosition, 10f, 1f, num31, AudioRolloffMode.Linear);
				Sound.Play3D("SoundSlotMachineJackpotBell", this.Audio3dPosition, 10f, Mathf.Max(0f, 1f - (float)(jackpotsPerformed - 1) * 0.1f), 1f, AudioRolloffMode.Linear);
			}
			else if (is666)
			{
				Sound.Play3D("SoundSlotMachineScored666", this.Audio3dPosition, 10f, 1f, 1f, AudioRolloffMode.Linear);
			}
			else if (is667)
			{
				Sound.Play3D("SoundSlotMachineScored999", this.Audio3dPosition, 10f, 1f, 1f, AudioRolloffMode.Linear);
			}
			else
			{
				if (hasJackpot)
				{
					Sound.Play3D("SoundSlotMachineScoredWithJackpot", this.Audio3dPosition, 10f, 1f, scoredSoundPitch, AudioRolloffMode.Linear);
				}
				else
				{
					Sound.Play3D("SoundSlotMachineScored", this.Audio3dPosition, 10f, 1f, scoredSoundPitch, AudioRolloffMode.Linear);
				}
				scoredSoundPitch = Mathf.Min(scoredSoundPitch + 0.1f, 1.6f);
			}
			if (isRepeatingPattern)
			{
				Sound.Play3D("SoundSlotMachineAgainScore", this.Audio3dPosition, 10f, 1f, againSoundPitch, AudioRolloffMode.Linear);
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
				Sound.Play3D("SoundSpecialPattern_Light", this.Audio3dPosition, 10f, 1f, 1f, AudioRolloffMode.Linear);
				SlotMachineScript.ShowSpecialPatternImage(PatternScript.Kind.triangle);
				break;
			case PatternScript.Kind.triangleInverted:
				Sound.Play3D("SoundSpecialPattern_Dark", this.Audio3dPosition, 10f, 1f, 1f, AudioRolloffMode.Linear);
				SlotMachineScript.ShowSpecialPatternImage(PatternScript.Kind.triangleInverted);
				break;
			case PatternScript.Kind.eye:
				Sound.Play3D("SoundSpecialPattern_Mistery", this.Audio3dPosition, 10f, 1f, 1f, AudioRolloffMode.Linear);
				SlotMachineScript.ShowSpecialPatternImage(PatternScript.Kind.eye);
				break;
			}
			if (is666)
			{
				FlashScreenSlot.Flash(Color.white, 10f, 0f);
				FlashScreenSlot.SetTexture(AssetMaster.GetTexture2D("TextureSlotMachineFire"), new global::UnityEngine.Vector2(0f, -1f));
				if (debtIndex >= GameplayData.SuperSixSixSix_GetMinimumDebtIndex())
				{
					FlashScreenSlot.SetSecondTexture(AssetMaster.GetTexture2D("TextureSlotMachineFireSuper"), new global::UnityEngine.Vector2(0f, -2f));
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
				FlashScreenSlot.SetTexture(AssetMaster.GetTexture2D("TextureSlotMachine999"), new global::UnityEngine.Vector2(0f, -1f));
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
					Sound.Play3D("SoundSlotMachineModifierScore", this.Audio3dPosition, 10f, 1f, num44, AudioRolloffMode.Linear);
					Controls.VibrationSet_PreferMax(this.player, 0.25f);
				}
				scTxt.ForceMeshUpdate(false, false);
				scTxt.gameObject.SetActive(true);
				global::UnityEngine.Vector3 scoringPosition_World = patternInfos.GetScoringPosition_World();
				scTxt.transform.position = scoringPosition_World + new global::UnityEngine.Vector3(0f, 0.75f, -10f);
				global::UnityEngine.Vector2 anchoredPosition2 = scTxt.rectTransform.anchoredPosition;
				float preferredWidth = scTxt.preferredWidth;
				float preferredHeight = scTxt.preferredHeight;
				anchoredPosition2.x = Mathf.Clamp(anchoredPosition2.x, -1.1f + preferredWidth / 2f, 1.1f - preferredWidth / 2f);
				anchoredPosition2.y = Mathf.Clamp(anchoredPosition2.y, -0.5f + preferredHeight / 2f, 0.9f - preferredHeight / 2f);
				scTxt.rectTransform.anchoredPosition = anchoredPosition2;
				if (isJackpot)
				{
					scTxt.transform.localScale = global::UnityEngine.Vector2.one * (1f + Mathf.Min((float)(jackpotsPerformed - 1) * 0.05f, 0.25f));
				}
				else
				{
					scTxt.transform.localScale = global::UnityEngine.Vector2.one;
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
						Spawn.FromPool("Effect Small Coin 1", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.lowGreat:
						Spawn.FromPool("Effect Small Coin 2", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.lowFantastic:
						Spawn.FromPool("Effect Small Coin 3", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.lowJackpot:
						Spawn.FromPool("Effect Small Coin Jackpot", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.mediumNice:
						Spawn.FromPool("Effect Slot Stars 1", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.mediumGreat:
						Spawn.FromPool("Effect Slot Stars 2", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.mediumFantastic:
						Spawn.FromPool("Effect Slot Stars 3", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.mediumJackpot:
						Spawn.FromPool("Effect Slot Stars Jackpot", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						break;
					case SlotMachineScript.SensationalLevel.highNice:
						Spawn.FromPool("Effect Coins 1", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						if (hasJackpot)
						{
							Spawn.FromPool("Effect Slot Stars 1", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						}
						break;
					case SlotMachineScript.SensationalLevel.highGreat:
						Spawn.FromPool("Effect Coins 2", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						if (hasJackpot)
						{
							Spawn.FromPool("Effect Slot Stars 2", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						}
						break;
					case SlotMachineScript.SensationalLevel.highFantastic:
						Spawn.FromPool("Effect Coins 3", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						if (hasJackpot)
						{
							Spawn.FromPool("Effect Slot Stars 3", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						}
						break;
					case SlotMachineScript.SensationalLevel.highJackpot:
						Spawn.FromPool("Effect Coins Jackpot", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
						if (hasJackpot)
						{
							Spawn.FromPool("Effect Slot Stars Jackpot", symbolScript_ByScoringPosition3.transform.position + new global::UnityEngine.Vector3(0f, 0.3f, 5f), Pool.instance.transform);
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
						Sound.Play3D("SoundSlotMachineLongStreakEndAnticipation", this.Audio3dPosition, 10f, 1f, 1f, AudioRolloffMode.Linear);
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
						Sound.Play3D("SoundSlotMachineApplause", this.Audio3dPosition, 10f, 1f, 1f, AudioRolloffMode.Linear);
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
					Sound.Play3D("SoundCoinsMultipleFall", this.Audio3dPositionLow, 10f, 1f, global::UnityEngine.Random.Range(0.9f, 1.1f), AudioRolloffMode.Linear);
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
					Sound.Play3D("SoundInterestRetrieved", this.Audio3dPositionLow, 10f, 1f, 1f, AudioRolloffMode.Linear);
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

	// Token: 0x060007C9 RID: 1993 RVA: 0x0003F084 File Offset: 0x0003D284
	private int _SymbolsSpawn(bool onlyTheDataOnes, bool lastWheelIsSlow)
	{
		int num = 0;
		List<SymbolScript.Kind> list = GameplayData.SymbolsAvailable_GetAll(false);
		this.elementsInColumn.Clear();
		global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector3(0f, 0f, 100f);
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
					gameObject.transform.localPosition = new global::UnityEngine.Vector3(0f, (float)(-(float)this.elementsInColumn.Count) * 0.5f, 0f);
					gameObject.transform.localScale = global::UnityEngine.Vector3.one;
					this.elementsInColumn.Add(gameObject.transform);
				}
				num = Mathf.Max(num, this.elementsInColumn.Count);
			}
		}
		return num;
	}

	// Token: 0x060007CA RID: 1994 RVA: 0x0003F2E4 File Offset: 0x0003D4E4
	public SymbolScript Symbol_SpawnInstance(bool isScoringSymbol, SymbolScript.Kind kind, SymbolScript.Modifier modifier, int columnX, int lineY, bool pickRandomModifier)
	{
		global::UnityEngine.Vector3 vector = new global::UnityEngine.Vector3(0f, 0f, 100f);
		RectTransform rectTransform = this.columnsRectTr[columnX];
		GameObject gameObject = Spawn.FromPool(SymbolScript.GetPrefabName(kind), vector, rectTransform);
		gameObject.transform.localPosition = new global::UnityEngine.Vector3(0f, (float)(-(float)lineY) * 0.5f, 0f);
		gameObject.transform.localScale = global::UnityEngine.Vector3.one;
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

	// Token: 0x060007CB RID: 1995 RVA: 0x0000C686 File Offset: 0x0000A886
	private IEnumerator WaitForTriggerAnimation()
	{
		yield return null;
		while (PowerupTriggerAnimController.HasAnimations())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060007CC RID: 1996 RVA: 0x0003F390 File Offset: 0x0003D590
	private void PatternInfosResetPool()
	{
		for (int i = 0; i < this._patternInfos.Count; i++)
		{
			this._patternInfos[i].enabled = false;
		}
	}

	// Token: 0x060007CD RID: 1997 RVA: 0x0003F3C8 File Offset: 0x0003D5C8
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

	// Token: 0x060007CE RID: 1998 RVA: 0x0003F454 File Offset: 0x0003D654
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

	// Token: 0x060007CF RID: 1999 RVA: 0x0003F49C File Offset: 0x0003D69C
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

	// Token: 0x060007D0 RID: 2000 RVA: 0x0003F4FC File Offset: 0x0003D6FC
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

	// Token: 0x060007D1 RID: 2001 RVA: 0x0003F570 File Offset: 0x0003D770
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

	// Token: 0x060007D2 RID: 2002 RVA: 0x0003F5C4 File Offset: 0x0003D7C4
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

	// Token: 0x060007D3 RID: 2003 RVA: 0x0000C68E File Offset: 0x0000A88E
	private static bool PatternContainsPosition(SlotMachineScript.PatternInfos patternInfo, Vector2Int position)
	{
		return patternInfo != null && SlotMachineScript.ListContainsPosition(patternInfo.positions, position);
	}

	// Token: 0x060007D4 RID: 2004 RVA: 0x0003F630 File Offset: 0x0003D830
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

	// Token: 0x060007D5 RID: 2005 RVA: 0x0000C6A1 File Offset: 0x0000A8A1
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

	// Token: 0x060007D6 RID: 2006 RVA: 0x0003F668 File Offset: 0x0003D868
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

	// Token: 0x060007D7 RID: 2007 RVA: 0x0003F6CC File Offset: 0x0003D8CC
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

	// Token: 0x060007D8 RID: 2008 RVA: 0x0003F708 File Offset: 0x0003D908
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

	// Token: 0x060007D9 RID: 2009 RVA: 0x0003F77C File Offset: 0x0003D97C
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

	// Token: 0x060007DA RID: 2010 RVA: 0x0000C6B0 File Offset: 0x0000A8B0
	private void _PatternId_AddToList(string id)
	{
		this._antiduplicateIdsList.Add(id);
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x0000C6BE File Offset: 0x0000A8BE
	private bool _PatternId_IsAlreadyInList(string id)
	{
		return this._antiduplicateIdsList.Contains(id);
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x0003F7F8 File Offset: 0x0003D9F8
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

	// Token: 0x060007DD RID: 2013 RVA: 0x0000C6CC File Offset: 0x0000A8CC
	private void PatternComputtaion_ResetResultVariables()
	{
		this._patternComputationResult_Coins = 0;
	}

	// Token: 0x060007DE RID: 2014 RVA: 0x0000C6DA File Offset: 0x0000A8DA
	private void PatternComputation_GetResults(out BigInteger coins)
	{
		coins = this._patternComputationResult_Coins;
	}

	// Token: 0x060007DF RID: 2015 RVA: 0x0003F820 File Offset: 0x0003DA20
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

	// Token: 0x060007E0 RID: 2016 RVA: 0x0003F87C File Offset: 0x0003DA7C
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

	// Token: 0x060007E1 RID: 2017 RVA: 0x0003F948 File Offset: 0x0003DB48
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

	// Token: 0x060007E2 RID: 2018 RVA: 0x0003FBC0 File Offset: 0x0003DDC0
	private void SpinWinSetText(BigInteger coins)
	{
		this.SpinWinText_StopIfAny();
		this.spinWinScreenHolder.SetActive(true);
		this.textSpinWin.text = Translation.Get("SLOT_MAIN_TOTAL_SPIN_WIN") + "\n" + coins.ToStringSmart();
		Sound.Play3D("SoundSlotMachineSpinWin", this.Audio3dPosition, 10f, 1f, 1f, AudioRolloffMode.Linear);
		this.bounceScript.SetBounceScale(0.015f);
		Controls.VibrationSet_PreferMax(this.player, 0.25f);
		this.spinWinTextCoroutine = base.StartCoroutine(this._SpinWinTextCoroutine());
	}

	// Token: 0x060007E3 RID: 2019 RVA: 0x0003FC58 File Offset: 0x0003DE58
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

	// Token: 0x060007E4 RID: 2020 RVA: 0x0000C6E8 File Offset: 0x0000A8E8
	public bool IsSpinWinTextPlaying()
	{
		return this.spinWinTextCoroutine != null;
	}

	// Token: 0x060007E5 RID: 2021 RVA: 0x0000C6F3 File Offset: 0x0000A8F3
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

	// Token: 0x060007E6 RID: 2022 RVA: 0x0003FCA8 File Offset: 0x0003DEA8
	public TextMeshProUGUI GetScoreTextFromPool(string textStr, bool enable)
	{
		if (this.scoreTextsPool.Count == 0)
		{
			TextMeshProUGUI textMeshProUGUI = global::UnityEngine.Object.Instantiate<TextMeshProUGUI>(this.templateSlotScoreText, this.slotMachineCanvas.transform);
			textMeshProUGUI.transform.localScale = global::UnityEngine.Vector3.one;
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

	// Token: 0x060007E7 RID: 2023 RVA: 0x0000C702 File Offset: 0x0000A902
	public void ScoreTextDestroy(TextMeshProUGUI text)
	{
		this.scoreTextsActive.Remove(text);
		this.scoreTextsPool.Add(text);
		text.gameObject.SetActive(false);
	}

	// Token: 0x060007E8 RID: 2024 RVA: 0x0000C729 File Offset: 0x0000A929
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

	// Token: 0x060007E9 RID: 2025 RVA: 0x0003FD60 File Offset: 0x0003DF60
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

	// Token: 0x060007EA RID: 2026 RVA: 0x0000C738 File Offset: 0x0000A938
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

	// Token: 0x060007EB RID: 2027 RVA: 0x0000C747 File Offset: 0x0000A947
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

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x060007EC RID: 2028 RVA: 0x0000C77B File Offset: 0x0000A97B
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

	// Token: 0x060007ED RID: 2029 RVA: 0x0000C78F File Offset: 0x0000A98F
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

	// Token: 0x060007EE RID: 2030 RVA: 0x0003FDF0 File Offset: 0x0003DFF0
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

	// Token: 0x060007EF RID: 2031 RVA: 0x00040058 File Offset: 0x0003E258
	private void TopTextSet_BetMultiplier_DEPRECATED()
	{
		BigInteger bigInteger = GameplayData.AllSymbolsMultiplierGet(true) * GameplayData.AllPatternsMultiplierGet(true);
		this.SetTopScreenText("X" + bigInteger.ToStringSmart(), false);
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x00040090 File Offset: 0x0003E290
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

	// Token: 0x060007F1 RID: 2033 RVA: 0x0000C7C7 File Offset: 0x0000A9C7
	private void TopTextSet_Spin()
	{
		this.SetTopScreenText(Translation.Get("SLOT_TOP_SCREEN_SPIN"), false);
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x000400F4 File Offset: 0x0003E2F4
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

	// Token: 0x060007F3 RID: 2035 RVA: 0x00040154 File Offset: 0x0003E354
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

	// Token: 0x060007F4 RID: 2036 RVA: 0x0000C7DA File Offset: 0x0000A9DA
	private void UpdateTopText_BetCost()
	{
		if (this._state == SlotMachineScript.State.off)
		{
			this.TopTextSet_BetCost(false);
		}
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x00040210 File Offset: 0x0003E410
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

	// Token: 0x060007F6 RID: 2038 RVA: 0x0000C7EB File Offset: 0x0000A9EB
	private void EffectsUpdate()
	{
		this.BurningLevelAnimationUpdate();
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x000402CC File Offset: 0x0003E4CC
	public static void EffectPlay_LeverSparks()
	{
		if (SlotMachineScript.instance == null)
		{
			return;
		}
		Sound.Play3D("SoundSpark", SlotMachineScript.instance.effect_LeverSparks.transform.position, 5f, 1f, 1f, AudioRolloffMode.Linear);
		SlotMachineScript.instance.effect_LeverSparks.SetActive(true);
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x00040328 File Offset: 0x0003E528
	public static void ShowSpecialPatternImage(PatternScript.Kind patternKind)
	{
		if (SlotMachineScript.instance.specialPatternImageCoroutine != null)
		{
			return;
		}
		SlotMachineScript.instance.specialPatternImagesHolder.localScale = global::UnityEngine.Vector3.one;
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

	// Token: 0x060007F9 RID: 2041 RVA: 0x0000C7F3 File Offset: 0x0000A9F3
	private IEnumerator SpecialPatternImageCoroutine(SpriteRenderer choosenSpriteRenderer)
	{
		SlotMachineScript.instance.specialPatternImagesHolder.gameObject.SetActive(true);
		Color c = Color.white;
		float scale = 0f;
		while (scale < 1f)
		{
			scale += Tick.Time * 4f;
			this.specialPatternImagesHolder.localScale = global::UnityEngine.Vector3.one * (1f + scale * 0.1f);
			c.a = 1f - scale;
			choosenSpriteRenderer.color = c;
			yield return null;
		}
		scale = Mathf.Min(scale, 1f);
		this.specialPatternImagesHolder.localScale = global::UnityEngine.Vector3.one * (1f + scale * 0.1f);
		c.a = 1f - scale;
		choosenSpriteRenderer.color = c;
		this.SpecialPatternImageReset();
		yield break;
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x0000C809 File Offset: 0x0000AA09
	private void SpecialPatternImageReset()
	{
		this.specialPatternImageCoroutine = null;
		SlotMachineScript.instance.specialPatternImagesHolder.gameObject.SetActive(false);
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00040470 File Offset: 0x0003E670
	private void JackpotGlowShow(int particlesIntensity_0To2)
	{
		if (this.shrinkCoroutine != null)
		{
			base.StopCoroutine(this.shrinkCoroutine);
			this.shrinkCoroutine = null;
		}
		this.jackpotGlowHolder.SetActive(true);
		this.jackpotGlowScaler.localScale = global::UnityEngine.Vector3.one;
		if (Data.settings.flashingLightsReducedEnabled)
		{
			particlesIntensity_0To2 = 0;
		}
		for (int i = 0; i < this.jackpotGlowParticleHolders.Length; i++)
		{
			bool flag = i <= particlesIntensity_0To2;
			this.jackpotGlowParticleHolders[i].SetActive(flag);
		}
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x0000C827 File Offset: 0x0000AA27
	private void JackpotGlowShrink()
	{
		if (this.shrinkCoroutine != null)
		{
			return;
		}
		this.shrinkCoroutine = base.StartCoroutine(this.JackpotGlowShrinkCoroutine());
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x0000C844 File Offset: 0x0000AA44
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

	// Token: 0x060007FE RID: 2046 RVA: 0x0000C853 File Offset: 0x0000AA53
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

	// Token: 0x060007FF RID: 2047 RVA: 0x0000C862 File Offset: 0x0000AA62
	private void JackpotLightReset()
	{
		this.jackpotLight.intensity = 2f;
		this.jackpotLight.enabled = false;
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x0000C880 File Offset: 0x0000AA80
	private void JackpotLightSet(float intensity)
	{
		this.jackpotLight.intensity = intensity;
		this.jackpotLight.enabled = true;
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x000404EC File Offset: 0x0003E6EC
	public void AllArroundSparksSet(int repeatN)
	{
		repeatN = Mathf.Clamp(repeatN, 0, this.allArroundSparks.Length);
		for (int i = 0; i < repeatN; i++)
		{
			this.allArroundSparks[global::UnityEngine.Random.Range(0, this.allArroundSparks.Length)].SetActive(true);
		}
	}

	// Token: 0x06000802 RID: 2050 RVA: 0x0000C89A File Offset: 0x0000AA9A
	private Material[] BurningLevelGetMaterials_Slot()
	{
		if (SlotMachineScript.HasGoldenKnob())
		{
			return this.materialBurningSlotMachine_GoldenKnobAlt;
		}
		return this.materialBurningSlotMachine;
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x0000C8B0 File Offset: 0x0000AAB0
	private Material[] BurningLevelGetMaterials_Knob()
	{
		if (SlotMachineScript.HasGoldenKnob())
		{
			return this.materialBurningKnob_GoldenKnobAlt;
		}
		return this.materialBurningKnob;
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x00040534 File Offset: 0x0003E734
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

	// Token: 0x06000805 RID: 2053 RVA: 0x00040640 File Offset: 0x0003E840
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

	// Token: 0x06000806 RID: 2054 RVA: 0x00040788 File Offset: 0x0003E988
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
			Sound.Play3D("SoundSlotMachineSteamOff", this.Audio3dPosition, 10f, 1f, 1f, AudioRolloffMode.Linear);
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

	// Token: 0x06000807 RID: 2055 RVA: 0x00040870 File Offset: 0x0003EA70
	public void JackpotGalaxyReset()
	{
		for (int i = 0; i < this.jackpotGalaxyParticles.Length; i++)
		{
			this.jackpotGalaxyParticles[i].SetActive(false);
		}
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x000408A0 File Offset: 0x0003EAA0
	public void JackpotGalaxySetLevel(int level0To7)
	{
		if (Data.settings.flashingLightsReducedEnabled)
		{
			level0To7 = 0;
		}
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

	// Token: 0x06000809 RID: 2057 RVA: 0x000408EC File Offset: 0x0003EAEC
	public void PlayInsertCoinAnimation(float pitchMod)
	{
		this.insertCoinHolder.SetLocalZ(1f);
		float num = 0.5f;
		Sound.Play("SoundCoinDeposit", num, 1f + pitchMod);
		Controls.VibrationSet_PreferMax(this.player, 0.25f + pitchMod);
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x00040938 File Offset: 0x0003EB38
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

	// Token: 0x0600080B RID: 2059 RVA: 0x000409EC File Offset: 0x0003EBEC
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

	// Token: 0x0600080C RID: 2060 RVA: 0x00040AD0 File Offset: 0x0003ECD0
	private void OnDestroy()
	{
		if (SlotMachineScript.instance == this)
		{
			SlotMachineScript.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.UpdateTopText_BetCost));
		for (int i = 0; i < this.materialBurningKnob.Length; i++)
		{
			global::UnityEngine.Object.Destroy(this.materialBurningKnob[i]);
			global::UnityEngine.Object.Destroy(this.materialBurningKnob_GoldenKnobAlt[i]);
		}
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x00040B40 File Offset: 0x0003ED40
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
			this.titleScreenRawImage.rectTransform.anchoredPosition = new global::UnityEngine.Vector2(0f, Util.AngleSin(this.titleScreenTimer * 180f) * 0.025f);
		}
		else if (this._state == SlotMachineScript.State.bootingUp)
		{
			bool flag4 = Util.AngleSin(this.titleScreenTimer * 720f) > -0.75f;
			if (this.titleScreenRawImage.enabled != flag4)
			{
				this.titleScreenRawImage.enabled = flag4;
			}
			this.titleScreenRawImage.rectTransform.anchoredPosition = global::UnityEngine.Vector2.zero;
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

	// Token: 0x040006C5 RID: 1733
	public static SlotMachineScript instance = null;

	// Token: 0x040006C6 RID: 1734
	private const int PLAYER_INDEX = 0;

	// Token: 0x040006C7 RID: 1735
	private const float INSET_COIN_LERP_SPEED = 20f;

	// Token: 0x040006C8 RID: 1736
	private const float TOP_SCREEN_UPDATE_INTERVAL = 0.25f;

	// Token: 0x040006C9 RID: 1737
	private const float AUTO_SPIN_DELAY = 0.5f;

	// Token: 0x040006CA RID: 1738
	private const float SPIN_ANIMATION_SPEED = 0.5f;

	// Token: 0x040006CB RID: 1739
	public AnimationCurve spinAnimationCurve;

	// Token: 0x040006CC RID: 1740
	private const int ANIMATION_REPEAT_TIMES_MODIFIER = 2;

	// Token: 0x040006CD RID: 1741
	private const int ANIMATION_REPEAT_TIMES_JACKPOT = 3;

	// Token: 0x040006CE RID: 1742
	private const int ANIMATION_REPEAT_TIMES_666 = 3;

	// Token: 0x040006CF RID: 1743
	private const int ANIMATION_REPEAT_TIMES_999 = 3;

	// Token: 0x040006D0 RID: 1744
	private const float SLOT_MACHINE_ANIM_SPEED_INCREMENT = 0.125f;

	// Token: 0x040006D1 RID: 1745
	private const float MAX_ANIM_SPEED = 1.75f;

	// Token: 0x040006D2 RID: 1746
	private const int MAX_LINES = 3;

	// Token: 0x040006D3 RID: 1747
	private const int MAX_COLUMNS = 5;

	// Token: 0x040006D4 RID: 1748
	private readonly global::UnityEngine.Vector2 COLUMNS_END_POS = new global::UnityEngine.Vector2(0f, 0f);

	// Token: 0x040006D5 RID: 1749
	private const float COLUMN_VERTICAL_UNIT = 0.5f;

	// Token: 0x040006D6 RID: 1750
	private const float LINE_HORIZONTAL_UNIT = 0.275f;

	// Token: 0x040006D7 RID: 1751
	private const int GARBAGE_SYMBOLS_PER_COLUMN = 24;

	// Token: 0x040006D8 RID: 1752
	private const int GARBAGE_SYMBOLS_PER_COLUMN_EXTRA = 12;

	// Token: 0x040006D9 RID: 1753
	private const float LUCK_ELEMENT_COPIES_ANOTHER = 0.0025f;

	// Token: 0x040006DA RID: 1754
	private const int INITIAL_ROUNDS_OF_LUCK_BONUS = 31;

	// Token: 0x040006DB RID: 1755
	public const int JACKPOT_LUCK_BONUS = 15;

	// Token: 0x040006DC RID: 1756
	private const int SCORE_SQUARES_WIDTH = 5;

	// Token: 0x040006DD RID: 1757
	private const float BOUNCE_MINIMUM = 0.00175f;

	// Token: 0x040006DE RID: 1758
	private const float BOUNCE_REALLY_SMALL = 0.0035f;

	// Token: 0x040006DF RID: 1759
	private const float BOUNCE_SMALL = 0.0075f;

	// Token: 0x040006E0 RID: 1760
	private const float BOUNCE_MEDIUM = 0.015f;

	// Token: 0x040006E1 RID: 1761
	private const float BOUNCE_HIGH = 0.025f;

	// Token: 0x040006E2 RID: 1762
	private static Color C_KNOB_NO_EMISSION = new Color(0f, 0f, 0f, 1f);

	// Token: 0x040006E3 RID: 1763
	private static Color C_KNOB_FULL_EMISSION = new Color(0.5f, 0.25f, 0f, 1f);

	// Token: 0x040006E4 RID: 1764
	private Controls.PlayerExt player;

	// Token: 0x040006E5 RID: 1765
	public DiegeticMenuController myMenuController;

	// Token: 0x040006E6 RID: 1766
	public DiegeticMenuElement leverMenuElement;

	// Token: 0x040006E7 RID: 1767
	public DiegeticMenuElement redButtonElement;

	// Token: 0x040006E8 RID: 1768
	public Transform insertCoinHolder;

	// Token: 0x040006E9 RID: 1769
	public TextMeshProUGUI ledTextTop;

	// Token: 0x040006EA RID: 1770
	public BounceScript bounceScript;

	// Token: 0x040006EB RID: 1771
	public GameObject gameUiHolder;

	// Token: 0x040006EC RID: 1772
	public RawImage mainScreenRendererImage;

	// Token: 0x040006ED RID: 1773
	public GameObject screenCollider;

	// Token: 0x040006EE RID: 1774
	public GameObject noMoreSpinsScreenHolder;

	// Token: 0x040006EF RID: 1775
	public TextMeshProUGUI textNoMoreCoins;

	// Token: 0x040006F0 RID: 1776
	public GameObject spinWinScreenHolder;

	// Token: 0x040006F1 RID: 1777
	public TextMeshProUGUI textSpinWin;

	// Token: 0x040006F2 RID: 1778
	public RawImage titleScreenRawImage;

	// Token: 0x040006F3 RID: 1779
	private Camera slotCamera;

	// Token: 0x040006F4 RID: 1780
	public Canvas slotMachineCanvas;

	// Token: 0x040006F5 RID: 1781
	public RectTransform[] columnsRectTr;

	// Token: 0x040006F6 RID: 1782
	private CoinVisualizerScript[] coinsVisualizers;

	// Token: 0x040006F7 RID: 1783
	public ButtonVisualizerScript leverButtonVisualizer;

	// Token: 0x040006F8 RID: 1784
	public TextMeshProUGUI templateSlotScoreText;

	// Token: 0x040006F9 RID: 1785
	public Material fontMaterial_ScoreYellow;

	// Token: 0x040006FA RID: 1786
	public Material fontMaterial_ScoreOrange;

	// Token: 0x040006FB RID: 1787
	public GameObject[] scoreSquares;

	// Token: 0x040006FC RID: 1788
	public RectTransform[] replacementSquaresRectTr;

	// Token: 0x040006FD RID: 1789
	private global::UnityEngine.Vector2[] replacementSquaresStartingAnchoredPosition;

	// Token: 0x040006FE RID: 1790
	public GameObject effect_LeverSparks;

	// Token: 0x040006FF RID: 1791
	public Transform specialPatternImagesHolder;

	// Token: 0x04000700 RID: 1792
	public SpriteRenderer specialPSpriteRend_Eye;

	// Token: 0x04000701 RID: 1793
	public SpriteRenderer specialPSpriteRend_Above;

	// Token: 0x04000702 RID: 1794
	public SpriteRenderer specialPSpriteRend_Below;

	// Token: 0x04000703 RID: 1795
	public GameObject jackpotGlowHolder;

	// Token: 0x04000704 RID: 1796
	public Transform jackpotGlowScaler;

	// Token: 0x04000705 RID: 1797
	public GameObject[] jackpotGlowParticleHolders;

	// Token: 0x04000706 RID: 1798
	public Light jackpotLight;

	// Token: 0x04000707 RID: 1799
	public GameObject confettiHolder;

	// Token: 0x04000708 RID: 1800
	public GameObject[] allArroundSparks;

	// Token: 0x04000709 RID: 1801
	public MeshRenderer meshRendererSlotMachine;

	// Token: 0x0400070A RID: 1802
	public MeshRenderer meshRendererLever;

	// Token: 0x0400070B RID: 1803
	public Material[] materialBurningSlotMachine;

	// Token: 0x0400070C RID: 1804
	public Material[] materialBurningSlotMachine_GoldenKnobAlt;

	// Token: 0x0400070D RID: 1805
	private Material[] materialBurningKnob;

	// Token: 0x0400070E RID: 1806
	private Material[] materialBurningKnob_GoldenKnobAlt;

	// Token: 0x0400070F RID: 1807
	public GameObject[] steamOffParticles;

	// Token: 0x04000710 RID: 1808
	public GameObject[] steamBurningParticles;

	// Token: 0x04000711 RID: 1809
	public Light goldenKnobLight;

	// Token: 0x04000712 RID: 1810
	public GameObject[] jackpotGalaxyParticles;

	// Token: 0x04000713 RID: 1811
	private global::UnityEngine.Vector3 audio3dOffset = new global::UnityEngine.Vector3(0f, 1f, 0f);

	// Token: 0x04000714 RID: 1812
	private SlotMachineScript.State _state;

	// Token: 0x04000715 RID: 1813
	private bool autoSpin;

	// Token: 0x04000716 RID: 1814
	private float autoSpinDelay = 0.5f;

	// Token: 0x04000717 RID: 1815
	private float titleScreenTimer;

	// Token: 0x04000718 RID: 1816
	private bool? menuControllerEnabled_Old;

	// Token: 0x04000719 RID: 1817
	private const string MENU_DISABLED_REASON = "smd";

	// Token: 0x0400071A RID: 1818
	private bool hasGoldenKnob;

	// Token: 0x0400071B RID: 1819
	private BigInteger spinExtraCoins = 0;

	// Token: 0x0400071C RID: 1820
	private bool _legalToReplaceSymbols;

	// Token: 0x0400071D RID: 1821
	private int replaceVisibleSymbolsCallN;

	// Token: 0x0400071E RID: 1822
	private bool _isFirstSpinOfRound = true;

	// Token: 0x0400071F RID: 1823
	private bool _spinningBeforeCoinsReward;

	// Token: 0x04000720 RID: 1824
	private bool _hasShown666;

	// Token: 0x04000721 RID: 1825
	private BigInteger _666RoundLostCoins = 0;

	// Token: 0x04000722 RID: 1826
	private bool _666GotCoinsRestoredFromJackpot;

	// Token: 0x04000723 RID: 1827
	private bool _hasTransformedInto999;

	// Token: 0x04000724 RID: 1828
	private int forcedLuckNext;

	// Token: 0x04000725 RID: 1829
	private bool _isAllSamePattern;

	// Token: 0x04000726 RID: 1830
	private PatternScript.Kind _biggestPatternScored = PatternScript.Kind.undefined;

	// Token: 0x04000727 RID: 1831
	private SymbolScript.Kind[][] lines = new SymbolScript.Kind[][]
	{
		new SymbolScript.Kind[5],
		new SymbolScript.Kind[5],
		new SymbolScript.Kind[5]
	};

	// Token: 0x04000728 RID: 1832
	private SymbolScript.Kind[][] linesOld = new SymbolScript.Kind[][]
	{
		new SymbolScript.Kind[5],
		new SymbolScript.Kind[5],
		new SymbolScript.Kind[5]
	};

	// Token: 0x04000729 RID: 1833
	private float[] spinOffsetPerColumn = new float[5];

	// Token: 0x0400072A RID: 1834
	private Coroutine spinCoroutine;

	// Token: 0x0400072B RID: 1835
	private Coroutine spinFailsafeCoroutine;

	// Token: 0x0400072C RID: 1836
	private List<Vector2Int> _luckPositions = new List<Vector2Int>();

	// Token: 0x0400072D RID: 1837
	private List<Vector2Int> _aureolaChangedPositions = new List<Vector2Int>();

	// Token: 0x0400072E RID: 1838
	private const float FAILSAFE_RESET_TIMER_VALUE = 40f;

	// Token: 0x0400072F RID: 1839
	private float failSafeTimer = 40f;

	// Token: 0x04000730 RID: 1840
	private List<Transform> elementsInColumn = new List<Transform>(39);

	// Token: 0x04000731 RID: 1841
	private List<SymbolScript.Modifier> _modifiersToAnimate_Temp = new List<SymbolScript.Modifier>(4);

	// Token: 0x04000732 RID: 1842
	private List<SlotMachineScript.PatternInfos> _patternInfos = new List<SlotMachineScript.PatternInfos>(8);

	// Token: 0x04000733 RID: 1843
	private List<SlotMachineScript.PatternInfos> _PatInf_GetEnabledList = new List<SlotMachineScript.PatternInfos>(8);

	// Token: 0x04000734 RID: 1844
	private List<SlotMachineScript.PatternInfos> _PatInf_GetByKindList = new List<SlotMachineScript.PatternInfos>(8);

	// Token: 0x04000735 RID: 1845
	private List<Vector2Int> _patternCordsListTemp = new List<Vector2Int>(16);

	// Token: 0x04000736 RID: 1846
	private List<string> _antiduplicateIdsList = new List<string>(16);

	// Token: 0x04000737 RID: 1847
	private StringBuilder _pIdSb = new StringBuilder(64);

	// Token: 0x04000738 RID: 1848
	private BigInteger _patternComputationResult_Coins = 0;

	// Token: 0x04000739 RID: 1849
	private Coroutine spinWinTextCoroutine;

	// Token: 0x0400073A RID: 1850
	private float scoreTextFlashingTimer;

	// Token: 0x0400073B RID: 1851
	[NonSerialized]
	public List<TextMeshProUGUI> scoreTextsPool = new List<TextMeshProUGUI>();

	// Token: 0x0400073C RID: 1852
	[NonSerialized]
	public List<TextMeshProUGUI> scoreTextsActive = new List<TextMeshProUGUI>();

	// Token: 0x0400073D RID: 1853
	private Coroutine bootUpCoroutine;

	// Token: 0x0400073E RID: 1854
	private Coroutine noMoreCoinsCoroutine;

	// Token: 0x0400073F RID: 1855
	private bool flashWhite;

	// Token: 0x04000740 RID: 1856
	private float flashTimer;

	// Token: 0x04000741 RID: 1857
	private Color topTextColor_Orange = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x04000742 RID: 1858
	private Color topTextColor_White = new Color(1f, 1f, 1f, 1f);

	// Token: 0x04000743 RID: 1859
	private string topScreenString = "";

	// Token: 0x04000744 RID: 1860
	private string topScreenStringDouble = "";

	// Token: 0x04000745 RID: 1861
	private const string separator = " ~ ";

	// Token: 0x04000746 RID: 1862
	private int topScreenStringOffset;

	// Token: 0x04000747 RID: 1863
	private float topScreenOffsetTimer;

	// Token: 0x04000748 RID: 1864
	private BigInteger betCostOld = -1;

	// Token: 0x04000749 RID: 1865
	private float betCostUpdateTimer;

	// Token: 0x0400074A RID: 1866
	private Coroutine specialPatternImageCoroutine;

	// Token: 0x0400074B RID: 1867
	private Coroutine shrinkCoroutine;

	// Token: 0x0400074C RID: 1868
	private int _burningLevel;

	// Token: 0x0400074D RID: 1869
	private int _burningLevelAnimationIndex;

	// Token: 0x0400074E RID: 1870
	private float _burningLevelAnimationTimer;

	// Token: 0x0400074F RID: 1871
	private float mainMenuInputDelay = 0.5f;

	// Token: 0x04000750 RID: 1872
	private const float MAIN_MENU_INPUT_DELAY = 0.5f;

	// Token: 0x04000751 RID: 1873
	public SlotMachineScript.Event OnRoundBeing;

	// Token: 0x04000752 RID: 1874
	public SlotMachineScript.Event OnRoundEnd;

	// Token: 0x04000753 RID: 1875
	public SlotMachineScript.Event OnInterestEarn;

	// Token: 0x04000754 RID: 1876
	public SlotMachineScript.Event OnInterestEarnPost;

	// Token: 0x04000755 RID: 1877
	public SlotMachineScript.Event OnSpinPreLuckApplication;

	// Token: 0x04000756 RID: 1878
	public SlotMachineScript.Event OnSpinStart;

	// Token: 0x04000757 RID: 1879
	public SlotMachineScript.Event OnSpinEnd;

	// Token: 0x04000758 RID: 1880
	public SlotMachineScript.Event OnScoreEvaluationBegin;

	// Token: 0x04000759 RID: 1881
	public SlotMachineScript.Event OnScoreEvaluationEnd;

	// Token: 0x0400075A RID: 1882
	public SlotMachineScript.Event OnScoreEvaluationEnd_Late;

	// Token: 0x0400075B RID: 1883
	public SlotMachineScript.PatternEvent OnPatternEvaluationStart;

	// Token: 0x0400075C RID: 1884
	public SlotMachineScript.PatternEvent OnPatternEvaluationEnd;

	// Token: 0x0400075D RID: 1885
	public SlotMachineScript.PatternEvent On666;

	// Token: 0x0400075E RID: 1886
	public SlotMachineScript.PatternEvent On999;

	// Token: 0x0400075F RID: 1887
	public SlotMachineScript.PatternEvent OnModifierScored;

	// Token: 0x04000760 RID: 1888
	public SlotMachineScript.Event onTopText_LoopsAround_Temp;

	// Token: 0x04000761 RID: 1889
	public SlotMachineScript.Event onTopText_LoopsAround_Permanent;

	// Token: 0x0200006E RID: 110
	public enum State
	{
		// Token: 0x04000763 RID: 1891
		off,
		// Token: 0x04000764 RID: 1892
		bootingUp,
		// Token: 0x04000765 RID: 1893
		noMoreCoins,
		// Token: 0x04000766 RID: 1894
		idle,
		// Token: 0x04000767 RID: 1895
		spinning
	}

	// Token: 0x0200006F RID: 111
	public class PatternInfos
	{
		// Token: 0x06000810 RID: 2064 RVA: 0x000410F4 File Offset: 0x0003F2F4
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

		// Token: 0x06000811 RID: 2065 RVA: 0x000411D0 File Offset: 0x0003F3D0
		public global::UnityEngine.Vector3 GetScoringPosition_World()
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
				return global::UnityEngine.Vector3.zero;
			}
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00041620 File Offset: 0x0003F820
		public global::UnityEngine.Vector3 GetMedianPlatePosition()
		{
			global::UnityEngine.Vector3 zero = global::UnityEngine.Vector3.zero;
			for (int i = 0; i < this.positions.Count; i++)
			{
				zero.x += (float)this.positions[i].x;
				zero.y += (float)this.positions[i].y;
			}
			zero.x /= (float)this.positions.Count;
			zero.y /= (float)this.positions.Count;
			return zero;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x000416B8 File Offset: 0x0003F8B8
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

		// Token: 0x06000814 RID: 2068 RVA: 0x0000C8C6 File Offset: 0x0000AAC6
		public ulong GetOrderWeight()
		{
			return SymbolScript.SymbolsOrderWeightMask(this.symbolKind) | PatternScript.PatternOrderWeightMask(this.patternKind);
		}

		// Token: 0x04000768 RID: 1896
		public bool enabled;

		// Token: 0x04000769 RID: 1897
		public PatternScript.Kind patternKind = PatternScript.Kind.undefined;

		// Token: 0x0400076A RID: 1898
		public SymbolScript.Kind symbolKind = SymbolScript.Kind.undefined;

		// Token: 0x0400076B RID: 1899
		public BigInteger coins = 0;

		// Token: 0x0400076C RID: 1900
		public List<Vector2Int> positions = new List<Vector2Int>(16);

		// Token: 0x0400076D RID: 1901
		public int repeatingNChached = 1;
	}

	// Token: 0x02000070 RID: 112
	public enum SensationalLevel
	{
		// Token: 0x0400076F RID: 1903
		noone,
		// Token: 0x04000770 RID: 1904
		lowNice,
		// Token: 0x04000771 RID: 1905
		lowGreat,
		// Token: 0x04000772 RID: 1906
		lowFantastic,
		// Token: 0x04000773 RID: 1907
		lowJackpot,
		// Token: 0x04000774 RID: 1908
		mediumNice,
		// Token: 0x04000775 RID: 1909
		mediumGreat,
		// Token: 0x04000776 RID: 1910
		mediumFantastic,
		// Token: 0x04000777 RID: 1911
		mediumJackpot,
		// Token: 0x04000778 RID: 1912
		highNice,
		// Token: 0x04000779 RID: 1913
		highGreat,
		// Token: 0x0400077A RID: 1914
		highFantastic,
		// Token: 0x0400077B RID: 1915
		highJackpot,
		// Token: 0x0400077C RID: 1916
		Count
	}

	// Token: 0x02000071 RID: 113
	// (Invoke) Token: 0x06000817 RID: 2071
	public delegate void Event();

	// Token: 0x02000072 RID: 114
	// (Invoke) Token: 0x0600081B RID: 2075
	public delegate void PatternEvent(SlotMachineScript.PatternInfos patternInfo);
}
