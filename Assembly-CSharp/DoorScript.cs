using System;
using System.Collections;
using Panik;
using UnityEngine;

// Token: 0x0200004E RID: 78
public class DoorScript : MonoBehaviour
{
	// Token: 0x060004A6 RID: 1190 RVA: 0x00009529 File Offset: 0x00007729
	public static bool IsInConditionToUnlock()
	{
		return RewardBoxScript.GetRewardKind() == RewardBoxScript.RewardKind.DoorKey && RewardBoxScript.IsOpened() && !RewardBoxScript.HasPrize() && !GameplayData.PrizeWasUsedGet();
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x0000954D File Offset: 0x0000774D
	public static bool OpenTry()
	{
		return DoorScript.IsInConditionToUnlock();
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x00009559 File Offset: 0x00007759
	public static void DoorKnockPlay_Try()
	{
		if (Data.game.jumperinoScarinoDoorino_DoneOnce && global::UnityEngine.Random.value > 0.3f)
		{
			return;
		}
		Data.game.jumperinoScarinoDoorino_DoneOnce = true;
		DoorScript.instance.StartCoroutine(DoorScript.instance.DoorKnockCoroutine());
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x00009594 File Offset: 0x00007794
	private IEnumerator DoorKnockCoroutine()
	{
		float timer = global::UnityEngine.Random.Range(1.75f, 3f);
		while (timer > 0f)
		{
			if (!DialogueScript.IsEnabled() && !PowerupTriggerAnimController.HasAnimations() && GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.preparation)
			{
				timer -= Tick.Time;
			}
			yield return null;
		}
		Sound.Play3D("SoundDoorKnock", base.transform.position + new Vector3(0f, 6f, 0f), 20f, 1f, 1f, AudioRolloffMode.Linear);
		CameraGame.Shake(2f);
		timer = 0.25f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
			base.transform.position = this.startPos + new Vector3(global::UnityEngine.Random.Range(-timer / 2f, timer / 2f), global::UnityEngine.Random.Range(-timer / 2f, timer / 2f), global::UnityEngine.Random.Range(-timer / 2f, timer / 2f));
		}
		CameraGame.Shake(2f);
		timer = 0.25f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
			base.transform.position = this.startPos + new Vector3(global::UnityEngine.Random.Range(-timer / 2f, timer / 2f), global::UnityEngine.Random.Range(-timer / 2f, timer / 2f), global::UnityEngine.Random.Range(-timer / 2f, timer / 2f));
		}
		CameraGame.Shake(2f);
		timer = 0.25f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
			base.transform.position = this.startPos + new Vector3(global::UnityEngine.Random.Range(-timer / 2f, timer / 2f), global::UnityEngine.Random.Range(-timer / 2f, timer / 2f), global::UnityEngine.Random.Range(-timer / 2f, timer / 2f));
		}
		base.transform.position = this.startPos;
		yield break;
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x000095A3 File Offset: 0x000077A3
	private void Awake()
	{
		DoorScript.instance = this;
		this.startPos = base.transform.position;
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x000095BC File Offset: 0x000077BC
	private void OnDestroy()
	{
		if (DoorScript.instance == this)
		{
			DoorScript.instance = null;
		}
	}

	// Token: 0x0400044A RID: 1098
	public static DoorScript instance;

	// Token: 0x0400044B RID: 1099
	private Vector3 startPos;
}
