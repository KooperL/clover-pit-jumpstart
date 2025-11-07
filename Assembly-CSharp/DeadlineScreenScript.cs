using System;
using Panik;
using TMPro;
using UnityEngine;

public class DeadlineScreenScript : MonoBehaviour
{
	// Token: 0x06000858 RID: 2136 RVA: 0x0003686C File Offset: 0x00034A6C
	public static void ForceUpdate()
	{
		DeadlineScreenScript.instance.updateTimer = Mathf.Min(0f, DeadlineScreenScript.instance.updateTimer);
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x0003688C File Offset: 0x00034A8C
	private void Awake()
	{
		DeadlineScreenScript.instance = this;
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x00036894 File Offset: 0x00034A94
	private void OnDestroy()
	{
		if (DeadlineScreenScript.instance == this)
		{
			DeadlineScreenScript.instance = null;
		}
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x000368AC File Offset: 0x00034AAC
	private void Update()
	{
		if (!ATMScript.DebtClearCutsceneIsPlaying())
		{
			this.updateTimer -= Tick.Time;
		}
		if (this.updateTimer <= 0f)
		{
			this.updateTimer = 1f;
			this.deadlineText.text = Translation.Get("DEADLINE_MONITOR_TEXT") + (GameplayData.DebtIndexGet() + 1).ToStringSmart();
		}
	}

	public static DeadlineScreenScript instance;

	public TextMeshProUGUI deadlineText;

	private float updateTimer;
}
