using System;
using System.Collections;
using Panik;
using UnityEngine;

// Token: 0x02000089 RID: 137
public class WCScript : MonoBehaviour
{
	// Token: 0x060008AD RID: 2221 RVA: 0x000495EC File Offset: 0x000477EC
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

	// Token: 0x060008AE RID: 2222 RVA: 0x0000CE2A File Offset: 0x0000B02A
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

	// Token: 0x060008AF RID: 2223 RVA: 0x0000CE40 File Offset: 0x0000B040
	public static bool IsPerformingAction()
	{
		return !(WCScript.instance == null) && WCScript.instance.actionCoroutine != null;
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x0000CE5E File Offset: 0x0000B05E
	public static void ForceClose_Death()
	{
		if (WCScript.instance == null)
		{
			return;
		}
		WCScript.instance.forceClose_Death = true;
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x0000CE79 File Offset: 0x0000B079
	public static bool IsForceClosing()
	{
		return !(WCScript.instance == null) && WCScript.instance.forceClose_Death;
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x0000CE94 File Offset: 0x0000B094
	private void Awake()
	{
		WCScript.instance = this;
		this.actualPiss.SetActive(false);
	}

	// Token: 0x060008B3 RID: 2227 RVA: 0x0000CEA8 File Offset: 0x0000B0A8
	private void Start()
	{
		if (Data.GameData.IsGameCompletedFully())
		{
			this.wcRenderer.sharedMaterial = this.matGoldenWc;
		}
	}

	// Token: 0x060008B4 RID: 2228 RVA: 0x0000CEC2 File Offset: 0x0000B0C2
	private void OnDestroy()
	{
		if (WCScript.instance == this)
		{
			WCScript.instance = null;
		}
	}

	// Token: 0x04000865 RID: 2149
	public static WCScript instance;

	// Token: 0x04000866 RID: 2150
	public const int PLAYER_INDEX = 0;

	// Token: 0x04000867 RID: 2151
	public GameObject actualPiss;

	// Token: 0x04000868 RID: 2152
	public MeshRenderer wcRenderer;

	// Token: 0x04000869 RID: 2153
	public Material matGoldenWc;

	// Token: 0x0400086A RID: 2154
	private Coroutine actionCoroutine;

	// Token: 0x0400086B RID: 2155
	private bool forceClose_Death;

	// Token: 0x0200008A RID: 138
	public enum ActionType
	{
		// Token: 0x0400086D RID: 2157
		undefined,
		// Token: 0x0400086E RID: 2158
		piss,
		// Token: 0x0400086F RID: 2159
		poop
	}
}
