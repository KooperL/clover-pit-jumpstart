using System;
using Panik;
using UnityEngine;

public class TrapdoorScript : MonoBehaviour
{
	// Token: 0x060007A6 RID: 1958 RVA: 0x000322B8 File Offset: 0x000304B8
	public static void SetAnimation(TrapdoorScript.AnimationKind kind)
	{
		if (kind != TrapdoorScript.AnimationKind.Shake)
		{
			if (kind != TrapdoorScript.AnimationKind.Open)
			{
				return;
			}
			TrapdoorScript.instance.animator.SetTrigger("open");
			Sound.Play("SoundTrapdoorOpen", 1f, 1f);
			Controls.VibrationSet_PreferMax(TrapdoorScript.instance.player, 1f);
		}
		else
		{
			TrapdoorScript.instance.animator.SetTrigger("shake");
			Sound.Play("SoundTrapdoorShake", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
			Controls.VibrationSet_PreferMax(TrapdoorScript.instance.player, 0.75f);
			if (!TrapdoorScript.instance.heartbeatScarePlayed)
			{
				CameraController.HeartbeatPlay_Default();
				TrapdoorScript.instance.heartbeatScarePlayed = true;
			}
			if (ConsolePrompt.alarmAtTrapdoor)
			{
				AlarmRewardBox.AlarmRing();
				CameraGame.Shake(4f);
				return;
			}
		}
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x00032384 File Offset: 0x00030584
	private void Awake()
	{
		TrapdoorScript.instance = this;
		this.animator = base.GetComponentInChildren<Animator>();
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x00032398 File Offset: 0x00030598
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x000323A6 File Offset: 0x000305A6
	private void OnDestroy()
	{
		if (TrapdoorScript.instance == this)
		{
			TrapdoorScript.instance = null;
		}
	}

	public static TrapdoorScript instance;

	private const int PLAYER_INDEX = 0;

	public const string ANIM_SHAKE = "shake";

	public const string ANIM_OPEN = "open";

	private Controls.PlayerExt player;

	private Animator animator;

	private bool heartbeatScarePlayed;

	public enum AnimationKind
	{
		Shake,
		Open
	}
}
