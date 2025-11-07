using System;
using System.Collections;
using Panik;
using UnityEngine;

public class WCScript : MonoBehaviour
{
	// Token: 0x060007AE RID: 1966 RVA: 0x000323A0 File Offset: 0x000305A0
	public static void StartAction(WCScript.ActionType actionType)
	{
		if (WCScript.instance == null)
		{
			return;
		}
		if (WCScript.instance.actionCoroutine != null)
		{
			WCScript.instance.StopCoroutine(WCScript.instance.actionCoroutine);
		}
		WCScript.instance.actionCoroutine = WCScript.instance.StartCoroutine(WCScript.instance.ActionCoroutine(actionType));
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x000323FA File Offset: 0x000305FA
	private IEnumerator ActionCoroutine(WCScript.ActionType actionType)
	{
		CameraController.SetPosition((actionType == WCScript.ActionType.piss) ? CameraController.PositionKind.WcPiss : CameraController.PositionKind.WcPoop, false, 2f);
		if (actionType == WCScript.ActionType.piss)
		{
			Sound.Play("SoundWcPiss2", 1f, 1f);
		}
		else if (actionType == WCScript.ActionType.poop)
		{
			Sound.Play("SoundWcPoop", 1f, 1f);
		}
		bool farted = false;
		bool farted2 = false;
		float actionTimer = 5f;
		while (actionTimer > 0f)
		{
			if (WCScript.IsForceClosing())
			{
				IL_01D9:
				Sound.Stop("SoundWcPiss", true);
				Sound.Stop("SoundWcPiss2", true);
				Sound.Stop("SoundWcPoop", true);
				if (actionType == WCScript.ActionType.piss)
				{
					PowerupScript.Unlock(PowerupScript.Identifier.PissJar);
				}
				else if (actionType == WCScript.ActionType.poop)
				{
					PowerupScript.Unlock(PowerupScript.Identifier.PoopJar);
				}
				this.actualPiss.SetActive(false);
				CameraController.SetPosition(CameraController.PositionKind.Free, false, 1f);
				VirtualCursors.CursorDesiredVisibilitySet(0, false);
				if (PowerupScript.IsUnlocked(PowerupScript.Identifier.PoopJar) && PowerupScript.IsUnlocked(PowerupScript.Identifier.PissJar))
				{
					PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.CorporeSano);
				}
				this.forceClose_Death = false;
				this.actionCoroutine = null;
				yield break;
			}
			actionTimer -= Tick.Time;
			if (actionType == WCScript.ActionType.piss)
			{
				bool flag = actionTimer < 4.5f;
				if (this.actualPiss.activeSelf != flag)
				{
					this.actualPiss.SetActive(flag);
				}
			}
			else if (actionType == WCScript.ActionType.poop)
			{
				if (!farted && actionTimer < 4.5f)
				{
					farted = true;
					CameraGame.Shake(1f);
					CameraGame.ChromaticAberrationIntensitySet(0.5f);
				}
				else if (!farted2 && actionTimer < 3f)
				{
					farted2 = true;
					CameraGame.Shake(2f);
					CameraGame.ChromaticAberrationIntensitySet(1f);
				}
				else
				{
					CameraGame.Shake(0.5f);
					CameraGame.ChromaticAberrationIntensitySet(0.25f);
				}
			}
			yield return null;
		}
		if (WCScript.IsForceClosing())
		{
			goto IL_01D9;
		}
		if (actionType == WCScript.ActionType.piss)
		{
			Sound.Play("SoundWcZipUp", 1f, 1f);
			goto IL_01D9;
		}
		if (actionType == WCScript.ActionType.poop)
		{
			Sound.Play("SoundWcZipUpFromPoop", 1f, 1f);
			goto IL_01D9;
		}
		goto IL_01D9;
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x00032410 File Offset: 0x00030610
	public static bool IsPerformingAction()
	{
		return !(WCScript.instance == null) && WCScript.instance.actionCoroutine != null;
	}

	// Token: 0x060007B1 RID: 1969 RVA: 0x0003242E File Offset: 0x0003062E
	public static void ForceClose_Death()
	{
		if (WCScript.instance == null)
		{
			return;
		}
		WCScript.instance.forceClose_Death = true;
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x00032449 File Offset: 0x00030649
	public static bool IsForceClosing()
	{
		return !(WCScript.instance == null) && WCScript.instance.forceClose_Death;
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x00032464 File Offset: 0x00030664
	private void Awake()
	{
		WCScript.instance = this;
		this.actualPiss.SetActive(false);
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x00032478 File Offset: 0x00030678
	private void Start()
	{
		if (Data.GameData.IsGameCompletedFully())
		{
			this.wcRenderer.sharedMaterial = this.matGoldenWc;
		}
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x00032492 File Offset: 0x00030692
	private void OnDestroy()
	{
		if (WCScript.instance == this)
		{
			WCScript.instance = null;
		}
	}

	public static WCScript instance;

	public const int PLAYER_INDEX = 0;

	public GameObject actualPiss;

	public MeshRenderer wcRenderer;

	public Material matGoldenWc;

	private Coroutine actionCoroutine;

	private bool forceClose_Death;

	public enum ActionType
	{
		undefined,
		piss,
		poop
	}
}
