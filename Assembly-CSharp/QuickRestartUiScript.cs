using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuickRestartUiScript : MonoBehaviour
{
	// Token: 0x060009D2 RID: 2514 RVA: 0x0004333D File Offset: 0x0004153D
	public static bool IsEnabled()
	{
		return !(QuickRestartUiScript.instance == null) && QuickRestartUiScript.instance.holder.activeSelf;
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x0004335D File Offset: 0x0004155D
	private void TextUpdate()
	{
		this.restartText.text = Translation.Get("MENU_OPTION_RESTART");
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x00043374 File Offset: 0x00041574
	private void Awake()
	{
		QuickRestartUiScript.instance = this;
		this.holder.SetActive(false);
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x00043388 File Offset: 0x00041588
	private void Start()
	{
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.TextUpdate));
		this.ResetImageBar();
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x000433B0 File Offset: 0x000415B0
	private void OnDestroy()
	{
		if (QuickRestartUiScript.instance == this)
		{
			QuickRestartUiScript.instance = null;
		}
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x000433C8 File Offset: 0x000415C8
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		if (GameplayMaster.instance == null)
		{
			return;
		}
		bool flag = false;
		if (Controls.KeyboardButton_HoldGet(0, Controls.KeyboardElement.R))
		{
			this.disableDelayTimer = 0.5f;
			flag = true;
		}
		if (GameplayMaster.GetGamePhase() != GameplayMaster.GamePhase.preparation)
		{
			flag = false;
		}
		if (DialogueScript.IsEnabled())
		{
			flag = false;
		}
		if (ScreenMenuScript.IsEnabled())
		{
			flag = false;
		}
		if (TwitchUiScript.IsEnabled())
		{
			flag = false;
		}
		if (RewardUIScript.IsEnabled())
		{
			flag = false;
		}
		if (PhoneUiScript.IsEnabled())
		{
			flag = false;
		}
		if (WCScript.IsPerformingAction())
		{
			flag = false;
		}
		if (MagazineUiScript.IsEnabled())
		{
			flag = false;
		}
		if (ToyPhoneUIScript.IsEnabled())
		{
			flag = false;
		}
		if (DeckBoxUI.IsEnabled())
		{
			flag = false;
		}
		if (flag != QuickRestartUiScript.IsEnabled())
		{
			this.disableDelayTimer -= Tick.Time;
			if (flag || this.disableDelayTimer < 0f)
			{
				this.holder.SetActive(flag);
				if (!flag)
				{
					this.restartDelayTimer = 0f;
					this.disableDelayTimer = 0f;
					this.ResetImageBar();
				}
				else
				{
					Sound.Play("SoundQuickRestartTick", 1f, 1f);
				}
			}
		}
		if (!flag)
		{
			return;
		}
		Vector2 sizeDelta = this.foregroundImageBar.rectTransform.sizeDelta;
		sizeDelta.x += Tick.Time * 112f * 1.5f;
		sizeDelta.x = Mathf.Min(112f, sizeDelta.x);
		if (sizeDelta.x >= 22.4f && this.foregroundImageBar.rectTransform.sizeDelta.x < 22.4f)
		{
			Sound.Play("SoundQuickRestartTick", 1f, 1.1f);
		}
		else if (sizeDelta.x >= 44.8f && this.foregroundImageBar.rectTransform.sizeDelta.x < 44.8f)
		{
			Sound.Play("SoundQuickRestartTick", 1f, 1.1f);
		}
		else if (sizeDelta.x >= 67.2f && this.foregroundImageBar.rectTransform.sizeDelta.x < 67.2f)
		{
			Sound.Play("SoundQuickRestartTick", 1f, 1.2f);
		}
		else if (sizeDelta.x >= 89.6f && this.foregroundImageBar.rectTransform.sizeDelta.x < 89.6f)
		{
			Sound.Play("SoundQuickRestartTick", 1f, 1.3f);
		}
		else if (sizeDelta.x >= 22.4f && this.foregroundImageBar.rectTransform.sizeDelta.x < 22.4f)
		{
			Sound.Play("SoundQuickRestartTick", 1f, 1.4f);
		}
		this.foregroundImageBar.rectTransform.sizeDelta = sizeDelta;
		if (sizeDelta.x >= 111.9f)
		{
			this.restartDelayTimer += Tick.Time;
			if (this.restartDelayTimer > 0.25f)
			{
				GameplayMaster.restartQuickDeath = true;
				GameplayMaster.instance.DieTry(GameplayMaster.DeathStep.lookAtAtm, false);
				this.holder.SetActive(false);
			}
			bool flag2 = Util.AngleSin(Tick.PassedTime * 360f * 8f) > 0f;
			this.foregroundImageBar.color = (flag2 ? Color.white : QuickRestartUiScript.C_ORANGE);
		}
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x000436EC File Offset: 0x000418EC
	private void ResetImageBar()
	{
		Vector2 sizeDelta = this.foregroundImageBar.rectTransform.sizeDelta;
		sizeDelta.x = 0f;
		this.foregroundImageBar.rectTransform.sizeDelta = sizeDelta;
		this.foregroundImageBar.color = QuickRestartUiScript.C_ORANGE;
	}

	public static QuickRestartUiScript instance;

	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	public GameObject holder;

	public Image foregroundImageBar;

	public TextMeshProUGUI restartText;

	private float disableDelayTimer;

	private float restartDelayTimer;
}
