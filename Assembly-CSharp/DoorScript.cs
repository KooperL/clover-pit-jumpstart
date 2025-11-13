using System;
using System.Collections;
using Panik;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
	// Token: 0x06000430 RID: 1072 RVA: 0x0001C615 File Offset: 0x0001A815
	public static bool IsInConditionToUnlock()
	{
		return RewardBoxScript.GetRewardKind() == RewardBoxScript.RewardKind.DoorKey && RewardBoxScript.IsOpened() && !RewardBoxScript.HasPrize() && !GameplayData.PrizeWasUsedGet();
	}

	// Token: 0x06000431 RID: 1073 RVA: 0x0001C639 File Offset: 0x0001A839
	public static bool OpenTry()
	{
		return DoorScript.IsInConditionToUnlock();
	}

	// Token: 0x06000432 RID: 1074 RVA: 0x0001C645 File Offset: 0x0001A845
	public static void DoorKnockPlay_Try()
	{
		if (Data.game.jumperinoScarinoDoorino_DoneOnce && global::UnityEngine.Random.value > 0.3f)
		{
			return;
		}
		Data.game.jumperinoScarinoDoorino_DoneOnce = true;
		DoorScript.instance.StartCoroutine(DoorScript.instance.DoorKnockCoroutine());
	}

	// Token: 0x06000433 RID: 1075 RVA: 0x0001C680 File Offset: 0x0001A880
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
		Sound.Play3D("SoundDoorKnock", base.transform.position + new Vector3(0f, 6f, 0f), 20f, 1f, 1f, 1);
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

	// Token: 0x06000434 RID: 1076 RVA: 0x0001C68F File Offset: 0x0001A88F
	private void Awake()
	{
		DoorScript.instance = this;
		this.startPos = base.transform.position;
	}

	// Token: 0x06000435 RID: 1077 RVA: 0x0001C6A8 File Offset: 0x0001A8A8
	private void OnDestroy()
	{
		if (DoorScript.instance == this)
		{
			DoorScript.instance = null;
		}
	}

	public static DoorScript instance;

	private Vector3 startPos;
}
