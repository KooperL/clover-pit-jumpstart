using System;
using Panik;
using TMPro;
using UnityEngine;

public class DeadlineScreenScript : MonoBehaviour
{
	// Token: 0x0600085F RID: 2143 RVA: 0x00036AC0 File Offset: 0x00034CC0
	public static void ForceUpdate()
	{
		DeadlineScreenScript.instance.updateTimer = Mathf.Min(0f, DeadlineScreenScript.instance.updateTimer);
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x00036AE0 File Offset: 0x00034CE0
	private void Awake()
	{
		DeadlineScreenScript.instance = this;
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00036AE8 File Offset: 0x00034CE8
	private void OnDestroy()
	{
		if (DeadlineScreenScript.instance == this)
		{
			DeadlineScreenScript.instance = null;
		}
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00036B00 File Offset: 0x00034D00
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
			this.deadlineText.text = Translation.Get("DEADLINE_MONITOR_TEXT") + (GameplayData.DebtIndexGet() + 1).ToStringSmart() + (flag ? " <sprite name=\"SkullSymbolOrange64\">" : null);
		}
	}

	public static DeadlineScreenScript instance;

	public TextMeshProUGUI deadlineText;

	private float updateTimer;
}
