using System;
using System.Numerics;
using System.Text;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000AB RID: 171
public class DeadlineBonusScreen : MonoBehaviour
{
	// Token: 0x0600097A RID: 2426 RVA: 0x0000D740 File Offset: 0x0000B940
	private void TitleTranslate()
	{
		this.titleText.text = Translation.Get("DEADLINE_REWARD_SCREEN_TITLE");
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x0004DD38 File Offset: 0x0004BF38
	public static void UpdateValues()
	{
		if (DeadlineBonusScreen.instance == null)
		{
			return;
		}
		BigInteger bigInteger = GameplayData.DebtIndexGet();
		int num = GameplayData.RoundsLeftToDeadline();
		DeadlineBonusScreen.instance.sb.Clear();
		DeadlineBonusScreen.instance.sb.Append(GameplayData.DeadlineReward_CoinsGet(bigInteger).ToStringSmart());
		DeadlineBonusScreen.instance.sb.Append("<sprite name=\"CoinSymbolOrange64\">");
		DeadlineBonusScreen.instance.bodyText_Coins.text = DeadlineBonusScreen.instance.sb.ToString();
		DeadlineBonusScreen.instance.sb.Clear();
		DeadlineBonusScreen.instance.sb.Append(GameplayData.DeadlineReward_CloverTickets(num));
		long num2 = GameplayData.DeadlineReward_CloverTickets_Extras(false);
		if (num2 > 0L)
		{
			DeadlineBonusScreen.instance.sb.Append("+");
			DeadlineBonusScreen.instance.sb.Append(num2);
		}
		DeadlineBonusScreen.instance.sb.Append("<sprite name=\"CloverTicket\">");
		DeadlineBonusScreen.instance.bodyText_Tickets.text = DeadlineBonusScreen.instance.sb.ToString();
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x0000D757 File Offset: 0x0000B957
	public static void Initialize()
	{
		if (DeadlineBonusScreen.instance == null)
		{
			return;
		}
		DeadlineBonusScreen.UpdateValues();
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x0000D76C File Offset: 0x0000B96C
	private void Awake()
	{
		DeadlineBonusScreen.instance = this;
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x0000D774 File Offset: 0x0000B974
	private void Start()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.TitleTranslate));
		this.TitleTranslate();
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x0000D7A4 File Offset: 0x0000B9A4
	private void OnDestroy()
	{
		if (DeadlineBonusScreen.instance == this)
		{
			DeadlineBonusScreen.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.TitleTranslate));
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x0004DE48 File Offset: 0x0004C048
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		bool flag = true;
		if (ATMScript.DebtClearCutsceneIsPlaying())
		{
			flag = false;
		}
		if (flag)
		{
			this.updateValuesTimer -= Tick.Time;
			if (this.updateValuesTimer <= 0f && !ATMScript.DebtClearCutsceneIsPlaying())
			{
				this.updateValuesTimer = 0.5f;
				DeadlineBonusScreen.UpdateValues();
			}
		}
	}

	// Token: 0x04000978 RID: 2424
	public static DeadlineBonusScreen instance;

	// Token: 0x04000979 RID: 2425
	public TextMeshProUGUI titleText;

	// Token: 0x0400097A RID: 2426
	public TextMeshPro bodyText_Coins;

	// Token: 0x0400097B RID: 2427
	public TextMeshPro bodyText_Tickets;

	// Token: 0x0400097C RID: 2428
	private BigInteger debtIndexOld = -1;

	// Token: 0x0400097D RID: 2429
	private StringBuilder sb = new StringBuilder();

	// Token: 0x0400097E RID: 2430
	private float updateValuesTimer;
}
