using System;
using System.Collections;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000084 RID: 132
public class TicketMachineScript : MonoBehaviour
{
	// Token: 0x06000892 RID: 2194 RVA: 0x0004911C File Offset: 0x0004731C
	private void SetAnimation(string animTicket)
	{
		this.currentAnim = animTicket;
		this.myAnimator.Play(animTicket, 0);
		this.animTimer = ((animTicket == "Idle") ? 0f : 2f) / 2f;
		this.doneAnimating = animTicket == "Idle";
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x0000CC98 File Offset: 0x0000AE98
	private bool IsAnimation(string animTicket)
	{
		return this.currentAnim == animTicket;
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x0000CCA6 File Offset: 0x0000AEA6
	private bool IsAnimationDone()
	{
		return this.doneAnimating;
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x00049174 File Offset: 0x00047374
	private string AnimationGetByIndex(int index)
	{
		switch (index)
		{
		case 0:
			return "Idle";
		case 1:
			return "Ticket 1_001";
		case 2:
			return "Ticket 2_001";
		case 3:
			return "Ticket 3_001";
		case 4:
			return "Ticket 4_001";
		case 5:
			return "Ticket 5_001";
		case 6:
			return "Ticket 6_001";
		case 7:
			return "Ticket 7_001";
		case 8:
			return "Ticket 8_001";
		case 9:
			return "Ticket 9";
		case 10:
			return "Ticket 10";
		default:
			Debug.LogError("AnimationGetByIndex: Invalid index " + index.ToString());
			return "";
		}
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x0000CCAE File Offset: 0x0000AEAE
	public static bool IsTicketMachineRunning()
	{
		return !(TicketMachineScript.instance == null) && !TicketMachineScript.instance.IsAnimation("Idle");
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0000CCD1 File Offset: 0x0000AED1
	private static void TicketMachineRun(int ticketsN)
	{
		if (TicketMachineScript.instance == null)
		{
			return;
		}
		if (TicketMachineScript.instance._running)
		{
			return;
		}
		TicketMachineScript.instance._running = true;
		TicketMachineScript.instance.StartCoroutine(TicketMachineScript.instance.TicketMachineRunCoroutine(ticketsN));
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x0000CD0F File Offset: 0x0000AF0F
	private IEnumerator TicketMachineRunCoroutine(int ticketsN)
	{
		GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
		if (gamePhase == GameplayMaster.GamePhase.intro)
		{
			this._running = false;
			yield break;
		}
		if (gamePhase != GameplayMaster.GamePhase.intro)
		{
			Sound.Play3D("SoundTicketMachineRunning", base.transform.position, 10f, 1f, 2f, AudioRolloffMode.Linear);
		}
		ticketsN = Mathf.Clamp(ticketsN, 1, 10);
		this.SetAnimation(this.AnimationGetByIndex(ticketsN));
		while (!this.IsAnimationDone())
		{
			yield return null;
		}
		float timer = 0.25f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		while (DialogueScript.IsEnabled())
		{
			yield return null;
		}
		if (gamePhase != GameplayMaster.GamePhase.intro)
		{
			Sound.Play3D("SoundTicketsRetrieve", base.transform.position, 10f, 1f, 1f, AudioRolloffMode.Linear);
		}
		timer = 0.2f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		this.SetAnimation("Idle");
		timer = 0.25f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		this._running = false;
		yield break;
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x0000CD25 File Offset: 0x0000AF25
	private void TranslateText()
	{
		this.titleText.text = Translation.Get("TICKET_MACHINE_TITLE");
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x0000CD3C File Offset: 0x0000AF3C
	private void Awake()
	{
		TicketMachineScript.instance = this;
		this.myAnimator = base.GetComponentInChildren<Animator>();
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x0000CD50 File Offset: 0x0000AF50
	private void Start()
	{
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.TranslateText));
		this.TranslateText();
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x0000CD78 File Offset: 0x0000AF78
	private void OnDestroy()
	{
		if (TicketMachineScript.instance == this)
		{
			TicketMachineScript.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.TranslateText));
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x00049214 File Offset: 0x00047414
	private void LateUpdate()
	{
		if (!Tick.IsGameRunning)
		{
			return;
		}
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		if (this.cloverTicketsOld != GameplayData.CloverTicketsGet())
		{
			long num = GameplayData.CloverTicketsGet();
			if (this.cloverTicketsOld < num)
			{
				TicketMachineScript.TicketMachineRun((num - this.cloverTicketsOld).CastToInt());
			}
			this.cloverTicketsOld = num;
		}
		this.myAnimator.GetCurrentAnimatorStateInfo(0);
		if (!this.IsAnimation("Idle"))
		{
			this.animTimer -= Tick.Time;
			if (this.animTimer <= 0f)
			{
				this.doneAnimating = true;
			}
		}
	}

	// Token: 0x0400083F RID: 2111
	public static TicketMachineScript instance;

	// Token: 0x04000840 RID: 2112
	public const float ANIM_SPD = 2f;

	// Token: 0x04000841 RID: 2113
	public const string ANIM_IDLE = "Idle";

	// Token: 0x04000842 RID: 2114
	public const string ANIM_TICKET_1 = "Ticket 1_001";

	// Token: 0x04000843 RID: 2115
	public const string ANIM_TICKET_2 = "Ticket 2_001";

	// Token: 0x04000844 RID: 2116
	public const string ANIM_TICKET_3 = "Ticket 3_001";

	// Token: 0x04000845 RID: 2117
	public const string ANIM_TICKET_4 = "Ticket 4_001";

	// Token: 0x04000846 RID: 2118
	public const string ANIM_TICKET_5 = "Ticket 5_001";

	// Token: 0x04000847 RID: 2119
	public const string ANIM_TICKET_6 = "Ticket 6_001";

	// Token: 0x04000848 RID: 2120
	public const string ANIM_TICKET_7 = "Ticket 7_001";

	// Token: 0x04000849 RID: 2121
	public const string ANIM_TICKET_8 = "Ticket 8_001";

	// Token: 0x0400084A RID: 2122
	public const string ANIM_TICKET_9 = "Ticket 9";

	// Token: 0x0400084B RID: 2123
	public const string ANIM_TICKET_10 = "Ticket 10";

	// Token: 0x0400084C RID: 2124
	private Animator myAnimator;

	// Token: 0x0400084D RID: 2125
	public TextMeshPro titleText;

	// Token: 0x0400084E RID: 2126
	private string currentAnim = "";

	// Token: 0x0400084F RID: 2127
	private float animTimer;

	// Token: 0x04000850 RID: 2128
	private bool doneAnimating;

	// Token: 0x04000851 RID: 2129
	private bool _running;

	// Token: 0x04000852 RID: 2130
	private long cloverTicketsOld = 2L;
}
