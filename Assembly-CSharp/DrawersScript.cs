using System;
using System.Collections;
using Panik;
using UnityEngine;

// Token: 0x02000050 RID: 80
public class DrawersScript : MonoBehaviour
{
	// Token: 0x060004B3 RID: 1203 RVA: 0x000095E8 File Offset: 0x000077E8
	private void CameraFree_Chache()
	{
		this.cameraRotOld = CameraController.instance.freeCamTransform.eulerAngles;
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x000095FF File Offset: 0x000077FF
	private void CameraFree_Reset()
	{
		CameraController.SetPosition(CameraController.PositionKind.Free, false, 0f);
		CameraController.SetFreeCameraRotation(this.cameraRotOld);
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x000308A4 File Offset: 0x0002EAA4
	public static bool IsInConditionToUnlock()
	{
		RewardBoxScript.RewardKind rewardKind = RewardBoxScript.GetRewardKind();
		return (rewardKind == RewardBoxScript.RewardKind.DrawerKey0 || rewardKind == RewardBoxScript.RewardKind.DrawerKey1 || rewardKind == RewardBoxScript.RewardKind.DrawerKey2 || rewardKind == RewardBoxScript.RewardKind.DrawerKey3) && RewardBoxScript.IsOpened() && !RewardBoxScript.HasPrize() && !GameplayData.PrizeWasUsedGet();
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x000308E4 File Offset: 0x0002EAE4
	public static void Unlock(int index)
	{
		if (DrawersScript.instance == null)
		{
			ConsolePrompt.LogError("DrawersScript: Unlock: instance is null", "", 0f);
			return;
		}
		DrawersScript.instance.drawerIsUnlocked[index] = true;
		switch (DrawersScript.GetDrawersUnlockedNum_Local())
		{
		case 1:
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Drawer0);
			return;
		case 2:
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Drawer1);
			return;
		case 3:
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Drawer2);
			return;
		case 4:
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Drawer3);
			return;
		default:
			return;
		}
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x00030960 File Offset: 0x0002EB60
	public static void LockAll()
	{
		if (DrawersScript.instance == null)
		{
			ConsolePrompt.LogError("DrawersScript: LockAll: instance is null", "", 0f);
			return;
		}
		for (int i = 0; i < DrawersScript.instance.drawerIsUnlocked.Length; i++)
		{
			PowerupScript drawerPowerup = PowerupScript.GetDrawerPowerup(i);
			PowerupScript.ThrowAwayCanTriggerEffects_Set(false);
			if (drawerPowerup != null)
			{
				PowerupScript.ThrowAway(drawerPowerup.identifier, false);
			}
			PowerupScript.ThrowAwayCanTriggerEffects_Set(true);
			DrawersScript.instance.drawerIsUnlocked[i] = false;
		}
		Data.SaveGame(Data.GameSavingReason.debug, -1);
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x000309E4 File Offset: 0x0002EBE4
	public static bool OpenTry(int index)
	{
		if (DrawersScript.instance == null)
		{
			ConsolePrompt.LogError("DrawersScript: OpenTry: instance is null", "", 0f);
			return false;
		}
		if (!DrawersScript.instance.drawerIsUnlocked[index])
		{
			if (DrawersScript.IsInConditionToUnlock())
			{
				DrawersScript.Unlock(index);
				GameplayData.PrizeWasUsedSet();
			}
			else
			{
				CameraGame.Shake(1f);
				Sound.Play("SoundDoorLocked", 1f, 1f);
			}
			return false;
		}
		if (DrawersScript.instance.drawerIsOpen[index])
		{
			return false;
		}
		DrawersScript.lastOpenedIndex = index;
		DrawersScript.instance.CameraFree_Chache();
		DrawersScript.instance.drawerIsOpen[index] = true;
		Sound.Play("SoundDrawerOpen", 1f, 1f);
		switch (index)
		{
		case 0:
			CameraController.SetPosition(CameraController.PositionKind.Drawer0, false, 1f);
			break;
		case 1:
			CameraController.SetPosition(CameraController.PositionKind.Drawer1, false, 1f);
			break;
		case 2:
			CameraController.SetPosition(CameraController.PositionKind.Drawer2, false, 1f);
			break;
		case 3:
			CameraController.SetPosition(CameraController.PositionKind.Drawer3, false, 1f);
			break;
		}
		PowerupScript drawerPowerup = PowerupScript.GetDrawerPowerup(index);
		if (drawerPowerup == null)
		{
			ScreenMenuScript.OptionEvent[] array = new ScreenMenuScript.OptionEvent[]
			{
				new ScreenMenuScript.OptionEvent(DrawersScript._SpawnInfoDialogue),
				new ScreenMenuScript.OptionEvent(DrawersScript._CloseDrawer)
			};
			ScreenMenuScript.Open(true, true, 1, ScreenMenuScript.Positioning.down, 0f, Translation.Get("SCREEN_MENU_TITLE_DRAWER"), new string[]
			{
				Translation.Get("SCREEN_MENU_OPTION_DRAWER_INFO"),
				Translation.Get("SCREEN_MENU_OPTION_DRAWER_CLOSE")
			}, array);
		}
		else
		{
			drawerPowerup.Inspect();
		}
		return true;
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x00009618 File Offset: 0x00007818
	private static void _SpawnInfoDialogue()
	{
		DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_DRAWER_INFO" });
		DialogueScript.instance.onDialogueClose = new DialogueScript.AnswerCallback(DrawersScript._CloseDrawer);
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x00009644 File Offset: 0x00007844
	private static void _CloseDrawer()
	{
		DrawersScript.Close(DrawersScript.lastOpenedIndex);
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x00030B64 File Offset: 0x0002ED64
	public static void Close(int index)
	{
		if (DrawersScript.instance == null)
		{
			ConsolePrompt.LogError("DrawersScript: Close: instance is null", "", 0f);
			return;
		}
		if (!DrawersScript.instance.drawerIsOpen[index])
		{
			return;
		}
		DrawersScript.instance.drawerIsOpen[index] = false;
		Sound.Play("SoundDrawerClose", 1f, 1f);
		DrawersScript.instance.CameraFree_Reset();
		VirtualCursors.CursorDesiredVisibilitySet(0, false);
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x00030BD8 File Offset: 0x0002EDD8
	public static void CloseAll()
	{
		for (int i = 0; i < 4; i++)
		{
			DrawersScript.Close(i);
		}
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x00009650 File Offset: 0x00007850
	public static bool IsDrawerOpen(int index)
	{
		return !(DrawersScript.instance == null) && DrawersScript.instance.drawerIsOpen[index];
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x00030BF8 File Offset: 0x0002EDF8
	public static bool IsAnyDrawerOpened()
	{
		if (DrawersScript.instance == null)
		{
			return false;
		}
		for (int i = 0; i < 4; i++)
		{
			if (DrawersScript.IsDrawerOpen(i))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x0000966D File Offset: 0x0000786D
	public static int GetLastOpenedDrawer()
	{
		return DrawersScript.lastOpenedIndex;
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00030C2C File Offset: 0x0002EE2C
	public static int GetOpenedDrawer()
	{
		if (DrawersScript.IsDrawerOpen(DrawersScript.lastOpenedIndex))
		{
			return DrawersScript.lastOpenedIndex;
		}
		for (int i = 0; i < 4; i++)
		{
			if (DrawersScript.IsDrawerOpen(i))
			{
				Debug.LogWarning("DrawersScript: GetOpenedDrawer: Found an open drawer which is not the last opened one! This is weird!");
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x00009674 File Offset: 0x00007874
	public static bool IsDrawerUnlocked(int index)
	{
		if (DrawersScript.instance == null)
		{
			ConsolePrompt.LogError("DrawersScript: IsDrawerUnlocked: instance is null", "", 0f);
			return false;
		}
		return DrawersScript.instance.drawerIsUnlocked[index];
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x00030C6C File Offset: 0x0002EE6C
	public static int GetDrawersUnlockedNum()
	{
		if (!PlatformAPI.IsInitialized())
		{
			return 0;
		}
		if (Data.game == null)
		{
			Debug.LogError("DrawersScript: GetDrawersUnlockedNum: Data.game is null");
			return 0;
		}
		int num = 0;
		for (int i = 0; i < Data.game.drawersUnlocked.Length; i++)
		{
			if (Data.game.drawersUnlocked[i])
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x00030CC4 File Offset: 0x0002EEC4
	public static int GetDrawersUnlockedNum_Local()
	{
		if (!PlatformAPI.IsInitialized())
		{
			return 0;
		}
		if (Data.game == null)
		{
			Debug.LogError("DrawersScript: GetDrawersUnlockedNum: Data.game is null");
			return 0;
		}
		int num = 0;
		for (int i = 0; i < DrawersScript.instance.drawerIsUnlocked.Length; i++)
		{
			if (DrawersScript.instance.drawerIsUnlocked[i])
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x00030D1C File Offset: 0x0002EF1C
	private void KeysInit()
	{
		for (int i = 0; i < this.keyTransforms.Length; i++)
		{
			bool flag = this.drawerIsUnlocked[i];
			this.keyTransforms[i].SetLocalXAngle(flag ? 0f : (-90f));
			this.keyTransforms[i].SetLocalZ(flag ? 0.7f : 1.75f);
			this.keyTransforms[i].gameObject.SetActive(flag);
		}
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x000096A5 File Offset: 0x000078A5
	private void KeyAnimationStart(int keyIndex)
	{
		if (this.keyAnimationCoroutine[keyIndex] != null)
		{
			return;
		}
		this.keyAnimationCoroutine[keyIndex] = base.StartCoroutine(this.KeyAnimationCoroutine(keyIndex));
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x00030D94 File Offset: 0x0002EF94
	public static bool IsKeyAnimationPlaying()
	{
		if (DrawersScript.instance == null)
		{
			return false;
		}
		for (int i = 0; i < DrawersScript.instance.keyAnimationCoroutine.Length; i++)
		{
			if (DrawersScript.instance.keyAnimationCoroutine[i] != null)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x000096C7 File Offset: 0x000078C7
	private bool _PlayTruncatedKeyAnimation()
	{
		return GameplayMaster.DeathCountdownHasStarted() || GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.death || GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.endingWithoutDeath;
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x000096E2 File Offset: 0x000078E2
	private IEnumerator KeyAnimationCoroutine(int keyIndex)
	{
		if (!this._PlayTruncatedKeyAnimation())
		{
			CameraController.DisableReason_Add("drwrUn");
		}
		this.keyTransforms[keyIndex].SetLocalZ(1.75f);
		this.keyTransforms[keyIndex].SetLocalXAngle(-90f);
		while (this.keyTransforms[keyIndex].GetLocalZ() > 0.8f)
		{
			this.keyTransforms[keyIndex].AddLocalZ((0.7f - this.keyTransforms[keyIndex].GetLocalZ()) * Tick.Time * 10f);
			yield return null;
		}
		this.keyTransforms[keyIndex].SetLocalZ(0.7f);
		Sound.Play3D("SoundDrawerKeyEnter", this.keyTransforms[keyIndex].position, 10f, 1f, 1f, AudioRolloffMode.Linear);
		float timer = 0.25f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		Sound.Play3D("SoundDrawerKeyTurn", this.keyTransforms[keyIndex].position, 10f, 1f, 1f, AudioRolloffMode.Linear);
		while (this.keyTransforms[keyIndex].GetLocalXAngle() < -1f)
		{
			this.keyTransforms[keyIndex].AddLocalXAngle((0f - this.keyTransforms[keyIndex].GetLocalXAngle()) * Tick.Time * 10f);
			yield return null;
		}
		this.keyTransforms[keyIndex].SetLocalXAngle(0f);
		if (!this._PlayTruncatedKeyAnimation())
		{
			timer = 0.25f;
			while (timer > 0f)
			{
				timer -= Tick.Time;
				yield return null;
			}
			DialogueScript.SetQuestionDialogue(false, new DialogueScript.AnswerCallback(this.KeyAnimationAnswer_EndYes), new DialogueScript.AnswerCallback(this.KeyAnimationAnswer_EndNo), new string[] { "DIALOGUE_DRAWER_UNLOCK_END_QUESTION" });
			DialogueScript.SetDialogueInputDelay(0.5f);
			while (DialogueScript.IsEnabled())
			{
				yield return null;
			}
			yield return null;
			if (this.keyAnimationAnswerCoroutine != null)
			{
				yield return this.keyAnimationAnswerCoroutine;
			}
			yield return null;
		}
		this.keyAnimationCoroutine[keyIndex] = null;
		yield break;
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x000096F8 File Offset: 0x000078F8
	private void KeyAnimationAnswer_EndYes()
	{
		this.keyAnimationAnswerCoroutine = base.StartCoroutine(this.KeyAnimationAnswer_Coroutine(false));
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x0000970D File Offset: 0x0000790D
	private void KeyAnimationAnswer_EndNo()
	{
		this.keyAnimationAnswerCoroutine = base.StartCoroutine(this.KeyAnimationAnswer_Coroutine(true));
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x00009722 File Offset: 0x00007922
	private IEnumerator KeyAnimationAnswer_Coroutine(bool die)
	{
		DialogueScript.Close();
		PromptGuideScript.ForceClose(true);
		float delay = 0.5f;
		DiegeticMenuController.ActiveMenu.SetDelay(delay + 0.1f);
		while (delay > 0f)
		{
			delay -= Tick.Time;
			yield return null;
		}
		if (die)
		{
			Sound.Play("SoundTensionViolinLong", 1f, 1f);
			DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_DRAWER_UNLOCK_END_ANSWER_TO_YES" });
			DialogueScript.SetAutoProgress(5f);
			while (DialogueScript.IsEnabled())
			{
				yield return null;
			}
			GameplayMaster.instance.DieTry(GameplayMaster.DeathStep.lookAtTrapdoor, false);
			CameraController.DisableReason_Remove("drwrUn");
			yield break;
		}
		DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_DRAWER_UNLOCK_END_ANSWER_TO_NO" });
		while (DialogueScript.IsEnabled())
		{
			yield return null;
		}
		CameraController.DisableReason_Remove("drwrUn");
		yield break;
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x00030DD8 File Offset: 0x0002EFD8
	public static void SetEasterEgg(bool disableOthers, DrawersScript.EasterEgg easterEgg, int drawerIndex)
	{
		DrawersScript.instance.easterEggs[drawerIndex] = easterEgg;
		if (disableOthers)
		{
			DrawersScript._EasterEgg_DisableAll();
		}
		if (easterEgg == DrawersScript.EasterEgg.Undefined)
		{
			return;
		}
		switch (drawerIndex)
		{
		case 0:
			DrawersScript.instance.easterEggs_Drawer0[(int)easterEgg].SetActive(true);
			return;
		case 1:
			DrawersScript.instance.easterEggs_Drawer1[(int)easterEgg].SetActive(true);
			return;
		case 2:
			DrawersScript.instance.easterEggs_Drawer2[(int)easterEgg].SetActive(true);
			return;
		case 3:
			DrawersScript.instance.easterEggs_Drawer3[(int)easterEgg].SetActive(true);
			return;
		default:
			return;
		}
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x00030E64 File Offset: 0x0002F064
	private static void _EasterEgg_DisableAll()
	{
		GameObject[] array = DrawersScript.instance.easterEggs_Drawer0;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		array = DrawersScript.instance.easterEggs_Drawer1;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		array = DrawersScript.instance.easterEggs_Drawer2;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
		array = DrawersScript.instance.easterEggs_Drawer3;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(false);
		}
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x00009731 File Offset: 0x00007931
	public static DrawersScript.EasterEgg EasterEggGet(int drawerIndex)
	{
		return DrawersScript.instance.easterEggs[drawerIndex];
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x0000973F File Offset: 0x0000793F
	public static bool HasEasterEgg(int drawerIndex)
	{
		return DrawersScript.instance.easterEggs[drawerIndex] != DrawersScript.EasterEgg.Undefined && DrawersScript.instance.easterEggs[drawerIndex] != DrawersScript.EasterEgg.Count;
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x00030EFC File Offset: 0x0002F0FC
	public static void TryPuttingEasterEgg()
	{
		DrawersScript._EasterEgg_DisableAll();
		int num = global::UnityEngine.Random.Range(0, 4);
		for (int i = 0; i < 4; i++)
		{
			if (DrawersScript.IsDrawerUnlocked(num) && !(PowerupScript.array_InDrawer[num] != null))
			{
				DrawersScript.SetEasterEgg(false, (DrawersScript.EasterEgg)global::UnityEngine.Random.Range(0, 5), num);
				DrawersScript.hasSeenEasterEgg = false;
				return;
			}
			num++;
			if (num >= 4)
			{
				num = 0;
			}
		}
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x00030F58 File Offset: 0x0002F158
	public PowerupScript PutRandomCharmIntoDrawer(int desiredDrawerIndex = -1)
	{
		PowerupScript powerupScript = null;
		int num = 100;
		while (powerupScript == null && num > 0)
		{
			num--;
			bool flag;
			do
			{
				int count = PowerupScript.list_NotBought.Count;
				int num2 = R.Rng_Drawers.Range(0, count);
				powerupScript = PowerupScript.list_NotBought[num2];
				if (powerupScript == null)
				{
					flag = true;
				}
				else
				{
					flag = StoreCapsuleScript.PowerupIsForbiddenToRestock(powerupScript.identifier, false, false);
					bool flag2 = false;
					for (int i = 0; i < StoreCapsuleScript.storePowerups.Length; i++)
					{
						if (!(StoreCapsuleScript.storePowerups[i] == null) && StoreCapsuleScript.storePowerups[i].identifier == powerupScript.identifier)
						{
							flag2 = true;
							break;
						}
					}
					if (flag2)
					{
						flag = true;
					}
					if (powerupScript.IsInstantPowerup())
					{
						flag = true;
					}
				}
			}
			while (flag);
			if (!(powerupScript == null))
			{
				int num3 = powerupScript.MaxBuyTimesGet();
				bool flag3 = num3 < 0;
				int num4 = GameplayData.Powerup_BoughtTimes_Get(powerupScript.identifier);
				if (!flag3 && num4 >= num3)
				{
					powerupScript = null;
				}
				if (!(powerupScript == null))
				{
					if (num > 10 && powerupScript.StoreRerollEvaluate())
					{
						powerupScript = null;
					}
					powerupScript == null;
				}
			}
		}
		if (powerupScript == null)
		{
			return null;
		}
		PowerupScript.PutInDrawer(powerupScript.identifier, false, desiredDrawerIndex);
		powerupScript.ModifierReEvaluate(true, false);
		return powerupScript;
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00031090 File Offset: 0x0002F290
	public static int RerollCharmsIntoDrawers()
	{
		if (DrawersScript.instance == null)
		{
			ConsolePrompt.LogError("DrawersScript: RerollCharmsIntoDrawer: instance is null", "", 0f);
			return 0;
		}
		int num = 0;
		for (int i = 0; i < PowerupScript.array_InDrawer.Length; i++)
		{
			PowerupScript powerupScript = PowerupScript.array_InDrawer[i];
			if (!(powerupScript == null))
			{
				num++;
				PowerupScript.ThrowAwayCanTriggerEffects_Set(false);
				PowerupScript.SuppressThrowAwaySound();
				PowerupScript.SuppressThrowAwayAnimation();
				PowerupScript.ThrowAway(powerupScript.identifier, false);
				PowerupScript.ThrowAwayCanTriggerEffects_Set(true);
			}
		}
		for (int j = 0; j < num; j++)
		{
			DrawersScript.instance.PutRandomCharmIntoDrawer(-1);
		}
		return num;
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x00031128 File Offset: 0x0002F328
	public static int RerollARandomCharmInDrawer()
	{
		if (DrawersScript.instance == null)
		{
			ConsolePrompt.LogError("DrawersScript: RerollARandomCharmInDrawer: instance is null", "", 0f);
			return -1;
		}
		int num = 4;
		int num2 = R.Rng_Drawers.Range(0, 4);
		PowerupScript powerupScript = null;
		while (num > 0 && powerupScript == null)
		{
			num--;
			powerupScript = PowerupScript.array_InDrawer[num2];
			if (powerupScript == null)
			{
				num2++;
			}
			if (num2 >= 4)
			{
				num2 = 0;
			}
		}
		if (powerupScript == null)
		{
			return -1;
		}
		if (num2 < 0 || num2 >= 4)
		{
			return -1;
		}
		PowerupScript.ThrowAwayCanTriggerEffects_Set(false);
		PowerupScript.SuppressThrowAwaySound();
		PowerupScript.SuppressThrowAwayAnimation();
		PowerupScript.ThrowAway(powerupScript.identifier, false);
		PowerupScript.ThrowAwayCanTriggerEffects_Set(true);
		if (DrawersScript.instance.PutRandomCharmIntoDrawer(num2) == null)
		{
			return -1;
		}
		return num2;
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00009764 File Offset: 0x00007964
	private IEnumerator SkeletonHorrorSoundCoroutine(int drawerIndex)
	{
		Sound.Play("SoundTensionViolinDown", 1f, 1f);
		float fovExtra = 0f;
		CameraGame.FieldOfViewExtraDecaySpeedSet("SKH", 10f);
		while (Sound.IsPlaying("SoundTensionViolinDown"))
		{
			if (!DrawersScript.IsDrawerOpen(drawerIndex))
			{
				Sound.SoundCapsule soundCapsule = Sound.Find("SoundTensionViolinDown");
				if (soundCapsule != null)
				{
					soundCapsule.localVolume -= Tick.Time * 0.5f;
				}
			}
			else
			{
				fovExtra += Tick.Time * 5f;
				fovExtra = Mathf.Min(fovExtra, 20f);
				CameraGame.FieldOfViewExtraSet("SKH", fovExtra);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x000311E8 File Offset: 0x0002F3E8
	public static void Initialize()
	{
		if (DrawersScript.instance == null)
		{
			Debug.LogError("DrawersScript: Initialize: instance is null");
			return;
		}
		DrawersScript.lastOpenedIndex = -1;
		for (int i = 0; i < PowerupScript.array_InDrawer.Length; i++)
		{
			DrawersScript.instance.drawerIsUnlocked[i] = Data.game.drawersUnlocked[i];
		}
		DrawersScript.instance.KeysInit();
		int drawersUnlockedNum = DrawersScript.GetDrawersUnlockedNum();
		if (drawersUnlockedNum > 0)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Drawer0);
		}
		if (drawersUnlockedNum > 1)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Drawer1);
		}
		if (drawersUnlockedNum > 2)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Drawer2);
		}
		if (drawersUnlockedNum > 3)
		{
			PlatformAPI.AchievementUnlock_FullGame(PlatformAPI.AchievementFullGame.Drawer3);
		}
		DrawersScript.hasSeenEasterEgg = false;
		for (int j = 0; j < 4; j++)
		{
			DrawersScript.SetEasterEgg(true, DrawersScript.EasterEgg.Undefined, j);
		}
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x00009773 File Offset: 0x00007973
	private void Awake()
	{
		DrawersScript.instance = this;
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x00031294 File Offset: 0x0002F494
	private void Start()
	{
		for (int i = 0; i < this.fliesGameObjects.Length; i++)
		{
			Animator componentInChildren = this.fliesGameObjects[i].GetComponentInChildren<Animator>(true);
			if (componentInChildren != null)
			{
				componentInChildren.speed = global::UnityEngine.Random.Range(0.8f, 1.2f);
			}
		}
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x0000977B File Offset: 0x0000797B
	private void OnDestroy()
	{
		if (DrawersScript.instance == this)
		{
			DrawersScript.instance = null;
		}
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x000312E4 File Offset: 0x0002F4E4
	private void Update()
	{
		if (!Tick.IsGameRunning)
		{
			return;
		}
		bool flag = DrawersScript.IsAnyDrawerOpened();
		this.shakeTimer -= Tick.Time;
		if (this.shakeTimer < -0.5f)
		{
			this.shakeTimer = 0.5f;
			bool flag2 = false;
			for (int i = 0; i < PowerupScript.array_InDrawer.Length; i++)
			{
				if (!(PowerupScript.array_InDrawer[i] == null))
				{
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				this.shakeSoundCounter++;
				if (this.shakeSoundCounter >= 2)
				{
					this.shakeSoundCounter = 0;
					Sound.Play3D("SoundDrawerWithItem", base.transform.position, 8f, 1f, 1f, AudioRolloffMode.Linear);
				}
			}
		}
		for (int j = 0; j < this.drawerTransforms.Length; j++)
		{
			Transform transform = this.drawerTransforms[j];
			bool flag3 = this.drawerIsOpen[j];
			if (flag3)
			{
				transform.AddLocalZ((1f - transform.GetLocalZ()) * Tick.Time * 10f);
			}
			else
			{
				transform.AddLocalZ((0f - transform.GetLocalZ()) * Tick.Time * 10f);
			}
			if (!flag3 && PowerupScript.GetDrawerPowerup(j) != null)
			{
				float num = Mathf.Max(this.shakeTimer, 0f) * 0.2f;
				transform.SetLocalX(global::UnityEngine.Random.Range(-num, num));
				transform.SetLocalY(global::UnityEngine.Random.Range(-num, num));
			}
			else
			{
				transform.SetLocalX(0f);
				transform.SetLocalY(0f);
			}
			if (PowerupScript.array_InDrawer[j] != null && PowerupScript.array_InDrawer[j].category == PowerupScript.Category.skeleton && flag3)
			{
				if (!this.skeletonUnclockCalled_PerDrawer[j])
				{
					this.skeletonUnclockCalled_PerDrawer[j] = true;
					PowerupScript.Unlock(PowerupScript.Identifier.Skeleton_Head);
					PowerupScript.Unlock(PowerupScript.Identifier.Skeleton_Arm1);
					PowerupScript.Unlock(PowerupScript.Identifier.Skeleton_Arm2);
					PowerupScript.Unlock(PowerupScript.Identifier.Skeleton_Leg1);
					PowerupScript.Unlock(PowerupScript.Identifier.Skeleton_Leg2);
				}
				if (!DrawersScript.skeletonHorrorSoundPlayed)
				{
					base.StartCoroutine(this.SkeletonHorrorSoundCoroutine(j));
					DrawersScript.skeletonHorrorSoundPlayed = true;
				}
			}
			if (flag3 && DrawersScript.HasEasterEgg(j))
			{
				DrawersScript.hasSeenEasterEgg = true;
			}
		}
		for (int k = 0; k < this.keyTransforms.Length; k++)
		{
			bool flag4 = this.drawerIsUnlocked[k];
			if (this.keyTransforms[k].gameObject.activeSelf != flag4)
			{
				this.keyTransforms[k].gameObject.SetActive(flag4);
				if (flag4)
				{
					this.KeyAnimationStart(k);
				}
			}
			bool flag5 = !flag;
			if (this.keyMeshRenderers[k].enabled != flag5)
			{
				this.keyMeshRenderers[k].enabled = flag5;
			}
		}
		bool flag6 = false;
		for (int l = 0; l < 4; l++)
		{
			this.hasFlies[l] = false;
			if (PowerupScript.array_InDrawer[l] != null && PowerupScript.array_InDrawer[l].category == PowerupScript.Category.skeleton)
			{
				this.hasFlies[l] = true;
				flag6 = true;
			}
			if (this.hasFlies[l])
			{
				if (!this.fliesDrawerHolders[l].activeSelf)
				{
					this.fliesDrawerHolders[l].SetActive(true);
				}
				if (!this.fliesGameObjects[l].activeSelf)
				{
					this.fliesGameObjects[l].SetActive(true);
				}
				if (this.normalDrawerHolders[l].activeSelf)
				{
					this.normalDrawerHolders[l].SetActive(false);
				}
			}
			else
			{
				if (this.fliesDrawerHolders[l].activeSelf)
				{
					this.fliesDrawerHolders[l].SetActive(false);
				}
				if (this.fliesGameObjects[l].activeSelf)
				{
					this.fliesGameObjects[l].SetActive(false);
				}
				if (!this.normalDrawerHolders[l].activeSelf)
				{
					this.normalDrawerHolders[l].SetActive(true);
				}
			}
		}
		if (flag6)
		{
			if (this.myFliesSound == null || !Sound.IsPlaying("SoundDrawerFlies"))
			{
				this.myFliesSound = Sound.Play3D("SoundDrawerFlies", base.transform.position + new Vector3(0f, 4f, 0f), 10f, this.fliesVolume, 1f, AudioRolloffMode.Linear);
				this.fliesVolume -= 0.1f;
				this.fliesVolume = Mathf.Max(this.fliesVolume, 0.2f);
				return;
			}
		}
		else if (this.myFliesSound != null)
		{
			this.myFliesSound.StopRequest();
			this.myFliesSound = null;
		}
	}

	// Token: 0x04000450 RID: 1104
	public static DrawersScript instance;

	// Token: 0x04000451 RID: 1105
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000452 RID: 1106
	public const int MAX_DRAWERS = 4;

	// Token: 0x04000453 RID: 1107
	private const float KEY_INSERTED_Z_POS = 0.7f;

	// Token: 0x04000454 RID: 1108
	private const float KEY_OUT_Z_POS = 1.75f;

	// Token: 0x04000455 RID: 1109
	private const float KEY_ANIM_LERP_SPEED = 10f;

	// Token: 0x04000456 RID: 1110
	private const string DISABLE_CAMERA_REASON_DRAWER_UNLOCK = "drwrUn";

	// Token: 0x04000457 RID: 1111
	public Transform[] drawerTransforms;

	// Token: 0x04000458 RID: 1112
	public Transform[] keyTransforms;

	// Token: 0x04000459 RID: 1113
	public MeshRenderer[] keyMeshRenderers;

	// Token: 0x0400045A RID: 1114
	public GameObject[] normalDrawerHolders;

	// Token: 0x0400045B RID: 1115
	public GameObject[] fliesDrawerHolders;

	// Token: 0x0400045C RID: 1116
	public GameObject[] fliesGameObjects;

	// Token: 0x0400045D RID: 1117
	private bool[] drawerIsUnlocked = new bool[4];

	// Token: 0x0400045E RID: 1118
	private bool[] drawerIsOpen = new bool[4];

	// Token: 0x0400045F RID: 1119
	private static int lastOpenedIndex = -1;

	// Token: 0x04000460 RID: 1120
	private Vector3 cameraRotOld = Vector3.zero;

	// Token: 0x04000461 RID: 1121
	private Coroutine[] keyAnimationCoroutine = new Coroutine[4];

	// Token: 0x04000462 RID: 1122
	private Coroutine keyAnimationAnswerCoroutine;

	// Token: 0x04000463 RID: 1123
	private float fliesVolume = 1f;

	// Token: 0x04000464 RID: 1124
	private Sound.SoundCapsule myFliesSound;

	// Token: 0x04000465 RID: 1125
	private bool[] hasFlies = new bool[4];

	// Token: 0x04000466 RID: 1126
	private DrawersScript.EasterEgg[] easterEggs = new DrawersScript.EasterEgg[]
	{
		DrawersScript.EasterEgg.Undefined,
		DrawersScript.EasterEgg.Undefined,
		DrawersScript.EasterEgg.Undefined,
		DrawersScript.EasterEgg.Undefined
	};

	// Token: 0x04000467 RID: 1127
	public GameObject[] easterEggs_Drawer0;

	// Token: 0x04000468 RID: 1128
	public GameObject[] easterEggs_Drawer1;

	// Token: 0x04000469 RID: 1129
	public GameObject[] easterEggs_Drawer2;

	// Token: 0x0400046A RID: 1130
	public GameObject[] easterEggs_Drawer3;

	// Token: 0x0400046B RID: 1131
	public static bool hasSeenEasterEgg = false;

	// Token: 0x0400046C RID: 1132
	private float shakeTimer;

	// Token: 0x0400046D RID: 1133
	private int shakeSoundCounter;

	// Token: 0x0400046E RID: 1134
	private const string FOV_EXTRA_TAG_SKELETON_HORROR = "SKH";

	// Token: 0x0400046F RID: 1135
	private static bool skeletonHorrorSoundPlayed = false;

	// Token: 0x04000470 RID: 1136
	private bool[] skeletonUnclockCalled_PerDrawer = new bool[4];

	// Token: 0x02000051 RID: 81
	public enum EasterEgg
	{
		// Token: 0x04000472 RID: 1138
		VynilPlayer,
		// Token: 0x04000473 RID: 1139
		AsylumTools,
		// Token: 0x04000474 RID: 1140
		AdmissionDeal,
		// Token: 0x04000475 RID: 1141
		FrankestainBook,
		// Token: 0x04000476 RID: 1142
		PsychiatryBook,
		// Token: 0x04000477 RID: 1143
		Count,
		// Token: 0x04000478 RID: 1144
		Undefined
	}
}
