using System;
using System.Numerics;
using System.Text;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DeadlineBonusScreen : MonoBehaviour
{
	// Token: 0x06000850 RID: 2128 RVA: 0x00036647 File Offset: 0x00034847
	private void TitleTranslate()
	{
		this.titleText.text = Translation.Get("DEADLINE_REWARD_SCREEN_TITLE");
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x00036660 File Offset: 0x00034860
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
		long num2 = GameplayData.DeadlineReward_CloverTickets_Extras();
		if (num2 > 0L)
		{
			DeadlineBonusScreen.instance.sb.Append("+");
			DeadlineBonusScreen.instance.sb.Append(num2);
		}
		DeadlineBonusScreen.instance.sb.Append("<sprite name=\"CloverTicket\">");
		DeadlineBonusScreen.instance.bodyText_Tickets.text = DeadlineBonusScreen.instance.sb.ToString();
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x0003676F File Offset: 0x0003496F
	public static void Initialize()
	{
		if (DeadlineBonusScreen.instance == null)
		{
			return;
		}
		DeadlineBonusScreen.UpdateValues();
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x00036784 File Offset: 0x00034984
	private void Awake()
	{
		DeadlineBonusScreen.instance = this;
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x0003678C File Offset: 0x0003498C
	private void Start()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.TitleTranslate));
		this.TitleTranslate();
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x000367BC File Offset: 0x000349BC
	private void OnDestroy()
	{
		if (DeadlineBonusScreen.instance == this)
		{
			DeadlineBonusScreen.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.TitleTranslate));
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x000367F4 File Offset: 0x000349F4
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
