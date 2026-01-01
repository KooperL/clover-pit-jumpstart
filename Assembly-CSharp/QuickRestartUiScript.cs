using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000E4 RID: 228
public class QuickRestartUiScript : MonoBehaviour
{
	// Token: 0x06000B8E RID: 2958 RVA: 0x0000F788 File Offset: 0x0000D988
	public static bool IsEnabled()
	{
		return !(QuickRestartUiScript.instance == null) && QuickRestartUiScript.instance.holder.activeSelf;
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x0000F7A8 File Offset: 0x0000D9A8
	private void TextUpdate()
	{
		this.restartText.text = Translation.Get("MENU_OPTION_RESTART");
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x0000F7BF File Offset: 0x0000D9BF
	private void Awake()
	{
		QuickRestartUiScript.instance = this;
		this.holder.SetActive(false);
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x0000F7D3 File Offset: 0x0000D9D3
	private void Start()
	{
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.TextUpdate));
		this.ResetImageBar();
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x0000F7FB File Offset: 0x0000D9FB
	private void OnDestroy()
	{
		if (QuickRestartUiScript.instance == this)
		{
			QuickRestartUiScript.instance = null;
		}
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x0005D4D8 File Offset: 0x0005B6D8
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

	// Token: 0x06000B94 RID: 2964 RVA: 0x0005D7FC File Offset: 0x0005B9FC
	private void ResetImageBar()
	{
		Vector2 sizeDelta = this.foregroundImageBar.rectTransform.sizeDelta;
		sizeDelta.x = 0f;
		this.foregroundImageBar.rectTransform.sizeDelta = sizeDelta;
		this.foregroundImageBar.color = QuickRestartUiScript.C_ORANGE;
	}

	// Token: 0x04000C30 RID: 3120
	public static QuickRestartUiScript instance;

	// Token: 0x04000C31 RID: 3121
	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x04000C32 RID: 3122
	public GameObject holder;

	// Token: 0x04000C33 RID: 3123
	public Image foregroundImageBar;

	// Token: 0x04000C34 RID: 3124
	public TextMeshProUGUI restartText;

	// Token: 0x04000C35 RID: 3125
	private float disableDelayTimer;

	// Token: 0x04000C36 RID: 3126
	private float restartDelayTimer;
}
