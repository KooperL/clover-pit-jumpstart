using System;
using System.Numerics;
using System.Text;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DeadlineBonusScreen : MonoBehaviour
{
	// Token: 0x06000857 RID: 2135 RVA: 0x0003689B File Offset: 0x00034A9B
	private void TitleTranslate()
	{
		this.titleText.text = Translation.Get("DEADLINE_REWARD_SCREEN_TITLE");
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x000368B4 File Offset: 0x00034AB4
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

	// Token: 0x06000859 RID: 2137 RVA: 0x000369C4 File Offset: 0x00034BC4
	public static void Initialize()
	{
		if (DeadlineBonusScreen.instance == null)
		{
			return;
		}
		DeadlineBonusScreen.UpdateValues();
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x000369D9 File Offset: 0x00034BD9
	private void Awake()
	{
		DeadlineBonusScreen.instance = this;
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x000369E1 File Offset: 0x00034BE1
	private void Start()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.TitleTranslate));
		this.TitleTranslate();
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x00036A11 File Offset: 0x00034C11
	private void OnDestroy()
	{
		if (DeadlineBonusScreen.instance == this)
		{
			DeadlineBonusScreen.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.TitleTranslate));
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x00036A48 File Offset: 0x00034C48
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

	public static DeadlineBonusScreen instance;

	public TextMeshProUGUI titleText;

	public TextMeshPro bodyText_Coins;

	public TextMeshPro bodyText_Tickets;

	private BigInteger debtIndexOld = -1;

	private StringBuilder sb = new StringBuilder();

	private float updateValuesTimer;
}
