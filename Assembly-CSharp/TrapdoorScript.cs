using System;
using Panik;
using UnityEngine;

// Token: 0x02000086 RID: 134
public class TrapdoorScript : MonoBehaviour
{
	// Token: 0x060008A5 RID: 2213 RVA: 0x000494A8 File Offset: 0x000476A8
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

	// Token: 0x060008A6 RID: 2214 RVA: 0x0000CDDF File Offset: 0x0000AFDF
	private void Awake()
	{
		TrapdoorScript.instance = this;
		this.animator = base.GetComponentInChildren<Animator>();
	}

	// Token: 0x060008A7 RID: 2215 RVA: 0x0000CDF3 File Offset: 0x0000AFF3
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
	}

	// Token: 0x060008A8 RID: 2216 RVA: 0x0000CE01 File Offset: 0x0000B001
	private void OnDestroy()
	{
		if (TrapdoorScript.instance == this)
		{
			TrapdoorScript.instance = null;
		}
	}

	// Token: 0x04000859 RID: 2137
	public static TrapdoorScript instance;

	// Token: 0x0400085A RID: 2138
	private const int PLAYER_INDEX = 0;

	// Token: 0x0400085B RID: 2139
	public const string ANIM_SHAKE = "shake";

	// Token: 0x0400085C RID: 2140
	public const string ANIM_OPEN = "open";

	// Token: 0x0400085D RID: 2141
	private Controls.PlayerExt player;

	// Token: 0x0400085E RID: 2142
	private Animator animator;

	// Token: 0x0400085F RID: 2143
	private bool heartbeatScarePlayed;

	// Token: 0x02000087 RID: 135
	public enum AnimationKind
	{
		// Token: 0x04000861 RID: 2145
		Shake,
		// Token: 0x04000862 RID: 2146
		Open
	}
}
