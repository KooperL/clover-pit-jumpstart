using System;
using Panik;
using TMPro;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class DeadlineScreenScript : MonoBehaviour
{
	// Token: 0x06000982 RID: 2434 RVA: 0x0000D7F8 File Offset: 0x0000B9F8
	public static void ForceUpdate()
	{
		DeadlineScreenScript.instance.updateTimer = Mathf.Min(0f, DeadlineScreenScript.instance.updateTimer);
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x0000D818 File Offset: 0x0000BA18
	private void Awake()
	{
		DeadlineScreenScript.instance = this;
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x0000D820 File Offset: 0x0000BA20
	private void OnDestroy()
	{
		if (DeadlineScreenScript.instance == this)
		{
			DeadlineScreenScript.instance = null;
		}
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x0004DEA4 File Offset: 0x0004C0A4
	private void Update()
	{
		if (!ATMScript.DebtClearCutsceneIsPlaying())
		{
			this.updateTimer -= Tick.Time;
		}
		if (this.updateTimer <= 0f)
		{
			this.updateTimer = 1f;
			bool flag = Data.game != null && GameplayData.Instance != null && Data.game.RunModifier_HardcoreMode_Get(GameplayData.RunModifier_GetCurrent());
			this.deadlineText.text = Translation.Get("DEADLINE_MONITOR_TEXT") + (GameplayData.DebtIndexGet() + 1).ToStringSmart() + (flag ? (" <sprite name=\"SkullSymbolOrange64\"> +" + GameplayData.HardcoreMode_GetDebtIncreasePercentage().ToString() + "%") : null);
		}
	}

	// Token: 0x0400097F RID: 2431
	public static DeadlineScreenScript instance;

	// Token: 0x04000980 RID: 2432
	public TextMeshProUGUI deadlineText;

	// Token: 0x04000981 RID: 2433
	private float updateTimer;
}
