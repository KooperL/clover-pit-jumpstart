using System;
using System.Collections;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TicketMachineScript : MonoBehaviour
{
	// Token: 0x06000799 RID: 1945 RVA: 0x00031F40 File Offset: 0x00030140
	private void SetAnimation(string animTicket)
	{
		this.currentAnim = animTicket;
		this.myAnimator.Play(animTicket, 0);
		this.animTimer = ((animTicket == "Idle") ? 0f : 2f) / 2f;
		this.doneAnimating = animTicket == "Idle";
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x00031F97 File Offset: 0x00030197
	private bool IsAnimation(string animTicket)
	{
		return this.currentAnim == animTicket;
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x00031FA5 File Offset: 0x000301A5
	private bool IsAnimationDone()
	{
		return this.doneAnimating;
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x00031FB0 File Offset: 0x000301B0
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

	// Token: 0x0600079D RID: 1949 RVA: 0x0003204E File Offset: 0x0003024E
	public static bool IsTicketMachineRunning()
	{
		return !(TicketMachineScript.instance == null) && !TicketMachineScript.instance.IsAnimation("Idle");
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x00032071 File Offset: 0x00030271
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

	// Token: 0x0600079F RID: 1951 RVA: 0x000320AF File Offset: 0x000302AF
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
			Sound.Play3D("SoundTicketMachineRunning", base.transform.position, 10f, 1f, 2f, 1);
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
			Sound.Play3D("SoundTicketsRetrieve", base.transform.position, 10f, 1f, 1f, 1);
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

	// Token: 0x060007A0 RID: 1952 RVA: 0x000320C5 File Offset: 0x000302C5
	private void TranslateText()
	{
		this.titleText.text = Translation.Get("TICKET_MACHINE_TITLE");
	}

	// Token: 0x060007A1 RID: 1953 RVA: 0x000320DC File Offset: 0x000302DC
	private void Awake()
	{
		TicketMachineScript.instance = this;
		this.myAnimator = base.GetComponentInChildren<Animator>();
	}

	// Token: 0x060007A2 RID: 1954 RVA: 0x000320F0 File Offset: 0x000302F0
	private void Start()
	{
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.TranslateText));
		this.TranslateText();
	}

	// Token: 0x060007A3 RID: 1955 RVA: 0x00032118 File Offset: 0x00030318
	private void OnDestroy()
	{
		if (TicketMachineScript.instance == this)
		{
			TicketMachineScript.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.TranslateText));
	}

	// Token: 0x060007A4 RID: 1956 RVA: 0x00032150 File Offset: 0x00030350
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

	public static TicketMachineScript instance;

	public const float ANIM_SPD = 2f;

	public const string ANIM_IDLE = "Idle";

	public const string ANIM_TICKET_1 = "Ticket 1_001";

	public const string ANIM_TICKET_2 = "Ticket 2_001";

	public const string ANIM_TICKET_3 = "Ticket 3_001";

	public const string ANIM_TICKET_4 = "Ticket 4_001";

	public const string ANIM_TICKET_5 = "Ticket 5_001";

	public const string ANIM_TICKET_6 = "Ticket 6_001";

	public const string ANIM_TICKET_7 = "Ticket 7_001";

	public const string ANIM_TICKET_8 = "Ticket 8_001";

	public const string ANIM_TICKET_9 = "Ticket 9";

	public const string ANIM_TICKET_10 = "Ticket 10";

	private Animator myAnimator;

	public TextMeshPro titleText;

	private string currentAnim = "";

	private float animTimer;

	private bool doneAnimating;

	private bool _running;

	private long cloverTicketsOld = 2L;
}
