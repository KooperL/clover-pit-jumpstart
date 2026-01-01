using System;
using System.Collections;
using System.Collections.Generic;
using Panik;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class CameraController : MonoBehaviour
{
	// Token: 0x06000021 RID: 33 RVA: 0x00016830 File Offset: 0x00014A30
	public static void SetPosition(CameraController.PositionKind kind, bool instant, float lerpSpeedMultiplier)
	{
		CameraController.instance.positionKind = kind;
		CameraController.instance.lerpSpeedMultiplier = lerpSpeedMultiplier;
		switch (kind)
		{
		case CameraController.PositionKind.Free:
			if (!ScreenMenuScript.IsEnabled())
			{
				VirtualCursors.CursorDesiredVisibilitySet(0, false);
			}
			PlayerScript.instanceP1.transform.SetX(CameraController.instance.freeCamTransform.position.x);
			PlayerScript.instanceP1.transform.SetZ(CameraController.instance.freeCamTransform.position.z);
			CameraController.instance.targetTransform = CameraController.instance.freeCamTransform;
			goto IL_0470;
		case CameraController.PositionKind.Slot_Fixed:
			VirtualCursors.CursorDesiredVisibilitySet(0, true);
			CameraController.instance.targetTransform = CameraController.instance.slotTransform;
			goto IL_0470;
		case CameraController.PositionKind.SlotCoinPlate_Fixed:
			VirtualCursors.CursorDesiredVisibilitySet(0, true);
			CameraController.instance.targetTransform = CameraController.instance.slotCoinPlateTransform;
			goto IL_0470;
		case CameraController.PositionKind.SlotFromTop:
			CameraController.instance.targetTransform = CameraController.instance.slotFromTopTransform;
			goto IL_0470;
		case CameraController.PositionKind.ATM:
			CameraController.instance.targetTransform = CameraController.instance.ATMTransform;
			goto IL_0470;
		case CameraController.PositionKind.Store:
			VirtualCursors.CursorDesiredVisibilitySet(0, true);
			CameraController.instance.targetTransform = CameraController.instance.StoreTransform;
			goto IL_0470;
		case CameraController.PositionKind.ATMStraight:
			CameraController.instance.targetTransform = CameraController.instance.ATMStraightTransform;
			goto IL_0470;
		case CameraController.PositionKind.TrapDoor:
			CameraController.instance.targetTransform = CameraController.instance.trapDoorTransform;
			goto IL_0470;
		case CameraController.PositionKind.Falling:
			CameraController.instance.targetTransform = CameraController.instance.fallingTransform;
			goto IL_0470;
		case CameraController.PositionKind.Drawer0:
			CameraController.instance.targetTransform = CameraController.instance.drawer0;
			VirtualCursors.CursorDesiredVisibilitySet(0, true);
			goto IL_0470;
		case CameraController.PositionKind.Drawer1:
			CameraController.instance.targetTransform = CameraController.instance.drawer1;
			VirtualCursors.CursorDesiredVisibilitySet(0, true);
			goto IL_0470;
		case CameraController.PositionKind.Drawer2:
			CameraController.instance.targetTransform = CameraController.instance.drawer2;
			VirtualCursors.CursorDesiredVisibilitySet(0, true);
			goto IL_0470;
		case CameraController.PositionKind.Drawer3:
			CameraController.instance.targetTransform = CameraController.instance.drawer3;
			VirtualCursors.CursorDesiredVisibilitySet(0, true);
			goto IL_0470;
		case CameraController.PositionKind.MenuDrawer_Menu:
			CameraController.instance.targetTransform = CameraController.instance.menuDrawer_Menu;
			VirtualCursors.CursorDesiredVisibilitySet(0, true);
			goto IL_0470;
		case CameraController.PositionKind.RewardBox:
			CameraController.instance.targetTransform = CameraController.instance.rewardBoxTransform;
			VirtualCursors.CursorDesiredVisibilitySet(0, false);
			goto IL_0470;
		case CameraController.PositionKind.RoomTopView:
			CameraController.instance.targetTransform = CameraController.instance.roomTopViewTransform;
			VirtualCursors.CursorDesiredVisibilitySet(0, false);
			goto IL_0470;
		case CameraController.PositionKind.CloverTicketsMachine:
			CameraController.instance.targetTransform = CameraController.instance.cloverTicketsMachineTransform;
			VirtualCursors.CursorDesiredVisibilitySet(0, false);
			goto IL_0470;
		case CameraController.PositionKind.doorEndingScene:
			CameraController.instance.targetTransform = CameraController.instance.doorEndingTransform;
			VirtualCursors.CursorDesiredVisibilitySet(0, false);
			goto IL_0470;
		case CameraController.PositionKind.terminal:
			CameraController.instance.targetTransform = CameraController.instance.terminalTransform;
			VirtualCursors.CursorDesiredVisibilitySet(0, true);
			goto IL_0470;
		case CameraController.PositionKind.DeadlineBonus:
			CameraController.instance.targetTransform = CameraController.instance.deadlineBonusTransform;
			goto IL_0470;
		case CameraController.PositionKind.PhoneDoor:
			CameraController.instance.targetTransform = CameraController.instance.phoneDoorTransform;
			goto IL_0470;
		case CameraController.PositionKind.SlotScreenCloseUp:
			CameraController.instance.targetTransform = CameraController.instance.slotScreenCloseUpTransform;
			goto IL_0470;
		case CameraController.PositionKind.ToyPhone:
			CameraController.instance.targetTransform = CameraController.instance.toyPhoneTransform;
			goto IL_0470;
		case CameraController.PositionKind.WcPiss:
			CameraController.instance.targetTransform = CameraController.instance.wcPiss;
			goto IL_0470;
		case CameraController.PositionKind.WcPoop:
			CameraController.instance.targetTransform = CameraController.instance.wcPoop;
			goto IL_0470;
		case CameraController.PositionKind.Drawer0Front:
			CameraController.instance.targetTransform = CameraController.instance.drawer0Front;
			goto IL_0470;
		case CameraController.PositionKind.Drawer1Front:
			CameraController.instance.targetTransform = CameraController.instance.drawer1Front;
			goto IL_0470;
		case CameraController.PositionKind.Drawer2Front:
			CameraController.instance.targetTransform = CameraController.instance.drawer2Front;
			goto IL_0470;
		case CameraController.PositionKind.Drawer3Front:
			CameraController.instance.targetTransform = CameraController.instance.drawer3Front;
			goto IL_0470;
		case CameraController.PositionKind.DrawersAll:
			CameraController.instance.targetTransform = CameraController.instance.drawersAllTransform;
			goto IL_0470;
		case CameraController.PositionKind.DeckBox:
			CameraController.instance.targetTransform = CameraController.instance.deckBoxTransform;
			goto IL_0470;
		case CameraController.PositionKind.CranePack:
			CameraController.instance.targetTransform = CameraController.instance.cranePackTransform;
			goto IL_0470;
		}
		Debug.LogError("CameraController.SetPosition(): Unhandled PositionKind");
		IL_0470:
		CameraController.SlotMachineLookingOnSides_Reset();
		if (instant)
		{
			CameraController.instance.myCamera.transform.position = CameraController.instance.targetTransform.position;
			CameraController.instance.myCamera.transform.rotation = CameraController.instance.targetTransform.rotation;
		}
	}

	// Token: 0x06000022 RID: 34 RVA: 0x000073F3 File Offset: 0x000055F3
	public static CameraController.PositionKind GetPositionKind()
	{
		return CameraController.instance.positionKind;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000073FF File Offset: 0x000055FF
	public static Transform GetTargetTransform()
	{
		return CameraController.instance.targetTransform;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x0000740B File Offset: 0x0000560B
	public static void DisableReason_Add(string reason)
	{
		if (CameraController.instance == null)
		{
			return;
		}
		if (!CameraController.instance.disableReasons.Contains(reason))
		{
			CameraController.instance.disableReasons.Add(reason);
		}
	}

	// Token: 0x06000025 RID: 37 RVA: 0x0000743D File Offset: 0x0000563D
	public static void DisableReason_Remove(string reason)
	{
		if (CameraController.instance == null)
		{
			return;
		}
		CameraController.instance.disableReasons.Remove(reason);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x0000745E File Offset: 0x0000565E
	public static bool HasDisabledReasons()
	{
		return !(CameraController.instance == null) && CameraController.instance.disableReasons.Count > 0;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00007481 File Offset: 0x00005681
	public static bool CanFreeLook()
	{
		return !(CameraController.instance == null) && CameraController.instance._canFreeLook;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x0000749C File Offset: 0x0000569C
	public static void SetSpeedNormalizationSpeed(float normalizationSpeed)
	{
		CameraController.instance.lerpSpeedNormalizationSpeed = normalizationSpeed;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x000074A9 File Offset: 0x000056A9
	public static float GetEulersDifferenceMagnitude()
	{
		if (CameraController.instance == null)
		{
			return 0f;
		}
		return CameraController.instance.eulersDifferenceMagnitude;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x000074C8 File Offset: 0x000056C8
	public static float GetPositionDifferenceMagnitude()
	{
		if (CameraController.instance == null)
		{
			return 0f;
		}
		return CameraController.instance.positionDifferenceMagnitude;
	}

	// Token: 0x0600002B RID: 43 RVA: 0x000074E7 File Offset: 0x000056E7
	public static bool IsCameraNearPositionAndAngle(float magnitude)
	{
		return CameraController.instance == null || (CameraController.instance.eulersDifferenceMagnitude < magnitude && CameraController.instance.positionDifferenceMagnitude < magnitude);
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00016CFC File Offset: 0x00014EFC
	public static void ResetFreeCameraAtSlot(bool resetPlayerAsWell)
	{
		CameraController.instance.freeCameraRotation = new Vector3(15f, 0f, 0f);
		if (CameraController.instance.freeCameraRotation.x > 180f)
		{
			CameraController cameraController = CameraController.instance;
			cameraController.freeCameraRotation.x = cameraController.freeCameraRotation.x - 360f;
		}
		if (CameraController.instance.freeCameraRotation.x < -180f)
		{
			CameraController cameraController2 = CameraController.instance;
			cameraController2.freeCameraRotation.x = cameraController2.freeCameraRotation.x + 360f;
		}
		if (CameraController.instance.freeCameraRotation.y > 180f)
		{
			CameraController cameraController3 = CameraController.instance;
			cameraController3.freeCameraRotation.y = cameraController3.freeCameraRotation.y - 360f;
		}
		if (CameraController.instance.freeCameraRotation.y < -180f)
		{
			CameraController cameraController4 = CameraController.instance;
			cameraController4.freeCameraRotation.y = cameraController4.freeCameraRotation.y + 360f;
		}
		CameraController.instance.freeCamTransform.position = CameraController.instance.freeCameraTransformStartingPosition;
		CameraController.instance.freeCamTransform.eulerAngles = CameraController.instance.freeCameraTransformStartingRotation;
		if (resetPlayerAsWell)
		{
			PlayerScript.instanceP1.transform.SetX(CameraController.instance.freeCamTransform.position.x);
			PlayerScript.instanceP1.transform.SetZ(CameraController.instance.freeCamTransform.position.z);
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00007515 File Offset: 0x00005715
	public static void SetFreeCameraRotation(Vector3 eulers)
	{
		CameraController.instance.freeCameraRotation = eulers;
		CameraController.instance.freeCamTransform.eulerAngles = eulers;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00007532 File Offset: 0x00005732
	public static void SetFreeCameraRotation_ToCamera()
	{
		CameraController.SetFreeCameraRotation(CameraGame.list[0].transform.eulerAngles);
	}

	// Token: 0x0600002F RID: 47 RVA: 0x0000754E File Offset: 0x0000574E
	public static Vector3 GetFreeCameraRotation()
	{
		return CameraController.instance.freeCameraRotation;
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00016E5C File Offset: 0x0001505C
	private void LookUpDown_ScaryRoutine()
	{
		float x = this.freeCameraRotation.x;
		bool flag = x > 65f;
		bool flag2 = x < -65f;
		bool flag3 = Mathf.Abs(CameraGame.firstInstance.transform.position.x) < 2f && Mathf.Abs(CameraGame.firstInstance.transform.position.z) < 2f;
		bool flag4 = flag2 && (this.nowIgnorePositionForLookingUp || flag3);
		if (flag)
		{
			this.lookDown_Timer += Tick.Time;
			if (this.lookDown_Timer > 0.5f)
			{
				this.lookDownFov += Tick.Time * 5f;
			}
			this.lookDownFov = Mathf.Min(this.lookDownFov, 30f);
			if (this.lookDown_Timer > 0.5f && !this.lookdDown_ScarySoundPlayed)
			{
				this.lookdDown_ScarySoundPlayed = true;
				Sound.Play3D("SoundScaryPit" + Util.Choose<int>(new int[] { 1, 2, 3 }).ToString(), new Vector3(0f, -2.5f, 0f), 30f, 1f, 1f, AudioRolloffMode.Linear);
			}
		}
		else
		{
			this.lookDown_Timer = 0f;
			this.lookDownFov = 0f;
		}
		if (flag4)
		{
			this.lookUp_Timer += Tick.Time;
			if (this.lookUp_Timer > 1f)
			{
				this.nowIgnorePositionForLookingUp = true;
			}
			if (this.lookUp_Timer > 1f)
			{
				this.lookUpFov += Tick.Time * 5f;
			}
			this.lookUpFov = Mathf.Min(this.lookUpFov, 20f);
			if (this.lookUp_Timer > 2f && !this.lookdUp_ScarySoundPlayed)
			{
				this.lookdUp_ScarySoundPlayed = true;
				Sound.Play3D("SoundScaryCeiling", new Vector3(0f, 15f, 0f), 30f, 1f, 1f, AudioRolloffMode.Linear);
			}
		}
		else
		{
			this.lookUp_Timer = 0f;
			this.nowIgnorePositionForLookingUp = false;
			this.lookUpFov = 0f;
		}
		float num = this.lookDownFov - this.lookUpFov;
		if (flag4 || flag)
		{
			CameraGame.FieldOfViewExtraSet("LUDS", num);
		}
		CameraGame.FieldOfViewExtraDecaySpeedSet("LUDS", 15f);
	}

	// Token: 0x06000031 RID: 49 RVA: 0x0000755A File Offset: 0x0000575A
	public static bool DeathFallDone()
	{
		return CameraController.instance.deathCameraY <= CameraController.instance.deathFallMaxHeight;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00007575 File Offset: 0x00005775
	private static void SlotMachineLookingOnSides_Reset()
	{
		if (CameraController.instance == null)
		{
			return;
		}
		CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.front);
		CameraController.instance._SMLOS_DelayTimer = 0f;
	}

	// Token: 0x06000033 RID: 51 RVA: 0x000170B0 File Offset: 0x000152B0
	public static void SlotMachineLook_Set(CameraController.SlotMachineLookingSides side)
	{
		if (CameraController.instance == null)
		{
			return;
		}
		if (CameraController.instance._slotMachineLookingOnSides == side)
		{
			return;
		}
		CameraController.instance._slotMachineLookingOnSides = side;
		if (side != CameraController.SlotMachineLookingSides.undefined)
		{
			VirtualCursors.CursorPositionNormalizedSet(0, Vector2.zero, true);
		}
		DiegeticMenuController.SlotMenu.HoveredElement = null;
		bool flag = side == CameraController.SlotMachineLookingSides.front;
		SlotMachineScript.instance.leverMenuElement.enabled = flag;
		SlotMachineScript.instance.redButtonElement.enabled = flag;
		if (flag)
		{
			if (!DiegeticMenuController.SlotMenu.elements.Contains(SlotMachineScript.instance.leverMenuElement))
			{
				DiegeticMenuController.SlotMenu.elements.Add(SlotMachineScript.instance.leverMenuElement);
				SlotMachineScript.instance.leverMenuElement.SetMyController(DiegeticMenuController.SlotMenu);
			}
			if (!DiegeticMenuController.SlotMenu.elements.Contains(SlotMachineScript.instance.redButtonElement))
			{
				DiegeticMenuController.SlotMenu.elements.Add(SlotMachineScript.instance.redButtonElement);
				SlotMachineScript.instance.redButtonElement.SetMyController(DiegeticMenuController.SlotMenu);
			}
			ToyPhoneScript.instance.MenuConnectionUpdate(ToyPhoneScript.MenuConnectionKind.slotMenu);
			DeckBoxScript.instance.MenuConnectionUpdate(DeckBoxScript.MenuConnectionKind.slotMenu);
		}
		else
		{
			DiegeticMenuController.SlotMenu.elements.Remove(SlotMachineScript.instance.leverMenuElement);
			DiegeticMenuController.SlotMenu.elements.Remove(SlotMachineScript.instance.redButtonElement);
			ToyPhoneScript.instance.MenuConnectionUpdate(ToyPhoneScript.MenuConnectionKind.freeRoamMenu);
			DeckBoxScript.instance.MenuConnectionUpdate(DeckBoxScript.MenuConnectionKind.freeRoamMenu);
		}
		if (side == CameraController.SlotMachineLookingSides.undefined)
		{
			foreach (PowerupScript powerupScript in PowerupScript.list_EquippedNormal)
			{
				if (!(powerupScript == null))
				{
					powerupScript.DiegeticStateUpdate(true, PowerupScript.MenuControllerTarget.Room);
				}
			}
			using (List<PowerupScript>.Enumerator enumerator = PowerupScript.list_EquippedSkeleton.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PowerupScript powerupScript2 = enumerator.Current;
					if (!(powerupScript2 == null))
					{
						powerupScript2.DiegeticStateUpdate(true, PowerupScript.MenuControllerTarget.Room);
					}
				}
				goto IL_0390;
			}
		}
		switch (side)
		{
		case CameraController.SlotMachineLookingSides.front:
		{
			foreach (PowerupScript powerupScript3 in PowerupScript.list_EquippedNormal)
			{
				if (!(powerupScript3 == null))
				{
					powerupScript3.DiegeticStateUpdate(false, PowerupScript.MenuControllerTarget.Slot);
				}
			}
			using (List<PowerupScript>.Enumerator enumerator = PowerupScript.list_EquippedSkeleton.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PowerupScript powerupScript4 = enumerator.Current;
					if (!(powerupScript4 == null))
					{
						powerupScript4.DiegeticStateUpdate(true, PowerupScript.MenuControllerTarget.Slot);
					}
				}
				goto IL_0390;
			}
			break;
		}
		case CameraController.SlotMachineLookingSides.left:
			break;
		case CameraController.SlotMachineLookingSides.right:
			goto IL_030C;
		default:
			goto IL_0390;
		}
		foreach (PowerupScript powerupScript5 in PowerupScript.list_EquippedNormal)
		{
			if (!(powerupScript5 == null))
			{
				powerupScript5.DiegeticStateUpdate(false, PowerupScript.MenuControllerTarget.Slot);
			}
		}
		using (List<PowerupScript>.Enumerator enumerator = PowerupScript.list_EquippedSkeleton.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PowerupScript powerupScript6 = enumerator.Current;
				if (!(powerupScript6 == null))
				{
					powerupScript6.DiegeticStateUpdate(false, PowerupScript.MenuControllerTarget.Slot);
				}
			}
			goto IL_0390;
		}
		IL_030C:
		foreach (PowerupScript powerupScript7 in PowerupScript.list_EquippedNormal)
		{
			if (!(powerupScript7 == null))
			{
				powerupScript7.DiegeticStateUpdate(true, PowerupScript.MenuControllerTarget.Slot);
			}
		}
		foreach (PowerupScript powerupScript8 in PowerupScript.list_EquippedSkeleton)
		{
			if (!(powerupScript8 == null))
			{
				powerupScript8.DiegeticStateUpdate(false, PowerupScript.MenuControllerTarget.Slot);
			}
		}
		IL_0390:
		DiegeticMenuController.SlotMenu.RecalculateNavigationBetweenMyElements_XZ(0f);
		if (side == CameraController.SlotMachineLookingSides.right && PowerupScript.list_EquippedNormal.Count > 0)
		{
			int num = 0;
			PowerupScript powerupScript9;
			do
			{
				powerupScript9 = PowerupScript.list_EquippedNormal[num];
				num++;
			}
			while (powerupScript9 == null && num < PowerupScript.list_EquippedNormal.Count);
			if (powerupScript9 != null)
			{
				DiegeticMenuController.SlotMenu.HoveredElement = powerupScript9.DiegeticMenuElement_Get();
			}
		}
	}

	// Token: 0x06000034 RID: 52 RVA: 0x0000759A File Offset: 0x0000579A
	public static CameraController.SlotMachineLookingSides SlotMachineLook_Get()
	{
		if (CameraController.instance == null)
		{
			return CameraController.SlotMachineLookingSides.front;
		}
		return CameraController.instance._slotMachineLookingOnSides;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x000075B5 File Offset: 0x000057B5
	public static bool SlotMachineLookingAtSides()
	{
		return !(CameraController.instance == null) && CameraController.instance._slotMachineLookingOnSides != CameraController.SlotMachineLookingSides.front;
	}

	// Token: 0x06000036 RID: 54 RVA: 0x000075D6 File Offset: 0x000057D6
	public static bool SlotMachineLookingFrontOrUndefined()
	{
		return CameraController.instance == null || CameraController.instance._slotMachineLookingOnSides == CameraController.SlotMachineLookingSides.front || CameraController.instance._slotMachineLookingOnSides == CameraController.SlotMachineLookingSides.undefined;
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00007603 File Offset: 0x00005803
	public static bool SlotMachineLookingLeft()
	{
		return !(CameraController.instance == null) && CameraController.instance._slotMachineLookingOnSides == CameraController.SlotMachineLookingSides.left;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00007621 File Offset: 0x00005821
	public static bool SlotMachineLookingRight()
	{
		return !(CameraController.instance == null) && CameraController.instance._slotMachineLookingOnSides == CameraController.SlotMachineLookingSides.right;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x0000763F File Offset: 0x0000583F
	public static void DollyZoomDividerSet(float divider)
	{
		CameraController.instance.dollyZoomDivider = divider;
	}

	// Token: 0x0600003A RID: 58 RVA: 0x0000764C File Offset: 0x0000584C
	public static void DollyZoomEnable(bool enable)
	{
		CameraController.instance.dollyZoomEnabled = enable;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00007659 File Offset: 0x00005859
	public static bool DollyZoomIsEnabled()
	{
		return CameraController.instance.dollyZoomEnabled;
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00017520 File Offset: 0x00015720
	public static void HeartbeatPlay(int n, float speed)
	{
		if (CameraController.instance == null)
		{
			return;
		}
		if (CameraController.instance.heartbeatCoroutine != null)
		{
			CameraController.instance.StopCoroutine(CameraController.instance.heartbeatCoroutine);
		}
		CameraController.instance.heartbeatCoroutine = CameraController.instance.StartCoroutine(CameraController.instance.HeartbeatCoroutine(n, speed));
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00007665 File Offset: 0x00005865
	public static void HeartbeatPlay_Default()
	{
		CameraController.HeartbeatPlay(7, 2f);
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00007672 File Offset: 0x00005872
	public static void HeartbeatPlay_Slow()
	{
		CameraController.HeartbeatPlay(5, 1f);
	}

	// Token: 0x0600003F RID: 63 RVA: 0x0000767F File Offset: 0x0000587F
	private IEnumerator HeartbeatCoroutine(int n, float speed)
	{
		float vol = 1f;
		float volFrac = vol / (float)n;
		while (n > 0)
		{
			Sound.Play("SoundHeartBeat1", vol, 1f);
			CameraGame.Shake(Mathf.Pow(vol * 2f, 2f) / 2.5f);
			float timer = 0.175f;
			while (timer > 0f)
			{
				timer -= Tick.Time;
				yield return null;
			}
			Sound.Play("SoundHeartBeat2", vol, 1f);
			vol -= volFrac;
			int num = n;
			n = num - 1;
			timer = 0.75f / speed;
			while (timer > 0f)
			{
				timer -= Tick.Time;
				yield return null;
			}
		}
		this.heartbeatCoroutine = null;
		yield break;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x0000769C File Offset: 0x0000589C
	public static void HeartbeatStop()
	{
		if (CameraController.instance == null)
		{
			return;
		}
		if (CameraController.instance.heartbeatCoroutine == null)
		{
			return;
		}
		CameraController.instance.StopCoroutine(CameraController.instance.heartbeatCoroutine);
	}

	// Token: 0x06000041 RID: 65 RVA: 0x000076CD File Offset: 0x000058CD
	public static void ScreenMenuIgnore_SetReason(string reason)
	{
		if (CameraController.instance == null)
		{
			return;
		}
		CameraController.instance.screenMenuIgnorePresence_Reasons.Add(reason);
	}

	// Token: 0x06000042 RID: 66 RVA: 0x000076ED File Offset: 0x000058ED
	public static void ScreenMenuIgnore_RemoveReason(string reason)
	{
		if (CameraController.instance == null)
		{
			return;
		}
		CameraController.instance.screenMenuIgnorePresence_Reasons.Remove(reason);
	}

	// Token: 0x06000043 RID: 67 RVA: 0x0001757C File Offset: 0x0001577C
	private void Awake()
	{
		CameraController.instance = this;
		this.myCamera = global::UnityEngine.Object.FindObjectOfType<CameraGame>();
		if (CameraController.fogStartDist_Default == -1f)
		{
			CameraController.fogStartDist_Default = RenderSettings.fogStartDistance;
		}
		if (CameraController.fogEndDist_Default == -1f)
		{
			CameraController.fogEndDist_Default = RenderSettings.fogEndDistance;
		}
		RenderSettings.fogStartDistance = CameraController.fogStartDist_Default;
		RenderSettings.fogEndDistance = CameraController.fogEndDist_Default;
		this.fogStartDist = CameraController.fogStartDist_Default;
		this.fogEndDist = CameraController.fogEndDist_Default;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000175F0 File Offset: 0x000157F0
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		if (this.positionKind == CameraController.PositionKind.Free)
		{
			this.targetTransform = this.freeCamTransform;
		}
		this.freeCameraTransformStartingPosition = this.freeCamTransform.position;
		this.freeCameraTransformStartingRotation = this.freeCamTransform.eulerAngles;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x0000770E File Offset: 0x0000590E
	private void OnDestroy()
	{
		if (CameraController.instance == this)
		{
			CameraController.instance = null;
		}
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00017640 File Offset: 0x00015840
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		if (!Tick.IsGameRunning)
		{
			return;
		}
		if (CameraDebug.IsEnabled())
		{
			return;
		}
		if (this.disableReasons.Count > 0)
		{
			return;
		}
		this._canFreeLook = false;
		this.offsetPosition = Vector3.zero;
		this.offsetEulers = Vector3.zero;
		float num = Tick.Time * 10f * this.lerpSpeedMultiplier;
		float num2 = Tick.Time * 10f * this.lerpSpeedMultiplier;
		bool flag = true;
		PowerupScript animatedPowerup = PowerupTriggerAnimController.GetAnimatedPowerup();
		if (PowerupTriggerAnimController.HasAnimations() && (animatedPowerup == null || animatedPowerup.archetype != PowerupScript.Archetype.skeleton))
		{
			flag = false;
		}
		if (flag)
		{
			this.lerpSpeedMultiplier = Mathf.MoveTowards(this.lerpSpeedMultiplier, 1f, this.lerpSpeedNormalizationSpeed * Tick.Time);
		}
		if (this.lerpSpeedMultiplier == 1f)
		{
			this.lerpSpeedNormalizationSpeed = this.lerpSpeedNormalizationSpeed_Default;
		}
		if (this.positionKind != CameraController.PositionKind.Slot_Fixed)
		{
			CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.undefined);
		}
		GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
		Vector2 zero = Vector2.zero;
		zero.x = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.cameraRight, Controls.InputAction.cameraLeft, true);
		zero.y = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.cameraUp, Controls.InputAction.cameraDown, true);
		float num3;
		if (this.player.lastInputKindUsed != Controls.InputKind.Mouse)
		{
			num3 = 225f * Tick.Time;
		}
		else
		{
			num3 = 0.5f;
		}
		Vector2 vector = Data.settings.CameraSensitivityGet(0);
		Vector2 vector2 = new Vector2((float)this.myCamera.myCamera.pixelWidth, (float)this.myCamera.myCamera.pixelHeight);
		Vector2 vector3 = VirtualCursors.CursorPositionNormalizedCenteredGet_ReferenceResolution(0, vector2);
		vector3.x = Mathf.Clamp(vector3.x, -0.5f, 0.5f);
		vector3.y = Mathf.Clamp(vector3.y, -0.5f, 0.5f);
		Vector2 vector4 = vector3;
		vector4.x += 0.5f;
		vector4.y += 0.5f;
		if (!DialogueScript.IsEnabled() && (!ScreenMenuScript.IsEnabled() || this.screenMenuIgnorePresence_Reasons.Count != 0))
		{
			switch (this.positionKind)
			{
			case CameraController.PositionKind.Free:
				if (GameplayMaster.GetGamePhase() != GameplayMaster.GamePhase.intro && GameplayMaster.GetGamePhase() != GameplayMaster.GamePhase.tutorialObsolete && !MemoryPackDealUI.IsDealRunnning())
				{
					bool flag2 = PowerupTriggerAnimController.HasAnimations();
					this._canFreeLook = true;
					if (Mathf.Abs(this.lerpSpeedMultiplier - 1f) < 0.05f && !flag2)
					{
						this.freeCameraRotation.x = this.freeCameraRotation.x - zero.y * vector.x * num3;
						this.freeCameraRotation.y = this.freeCameraRotation.y + zero.x * vector.y * num3;
					}
					if (this.freeCameraRotation.x > 180f)
					{
						this.freeCameraRotation.x = this.freeCameraRotation.x - 360f;
					}
					if (this.freeCameraRotation.x < -180f)
					{
						this.freeCameraRotation.x = this.freeCameraRotation.x + 360f;
					}
					this.freeCameraRotation.x = Mathf.Clamp(this.freeCameraRotation.x, -85f, 85f);
					if (this.freeCameraRotation.y > 180f)
					{
						this.freeCameraRotation.y = this.freeCameraRotation.y - 360f;
					}
					if (this.freeCameraRotation.y < -180f)
					{
						this.freeCameraRotation.y = this.freeCameraRotation.y + 360f;
					}
					this.freeCameraRotation.z = 0f;
					num2 *= 8f * Mathf.Pow(this.lerpSpeedMultiplier, 3f);
					this.targetTransform.eulerAngles = this.freeCameraRotation;
					this.targetTransform.SetX(PlayerScript.instanceP1.transform.position.x);
					this.targetTransform.SetZ(PlayerScript.instanceP1.transform.position.z);
					this.LookUpDown_ScaryRoutine();
					goto IL_0D1F;
				}
				goto IL_0D1F;
			case CameraController.PositionKind.Slot_Fixed:
			{
				if (MainMenuScript.IsEnabled())
				{
					goto IL_0D1F;
				}
				Vector2 zero2 = Vector2.zero;
				if (CameraController.SlotMachineLook_Get() == CameraController.SlotMachineLookingSides.undefined)
				{
					CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.front);
				}
				this._SMLOS_DelayTimer -= Tick.Time;
				this._SMLOS_ForceDirectionPersistenceTimer -= Tick.Time;
				Vector2 vector5;
				if (SlotMachineScript.IsSpinning() || SlotMachineScript.instance.IsSpinWinTextPlaying())
				{
					if (this.player.lastInputKindUsed == Controls.InputKind.Mouse)
					{
						zero2.x = vector3.x;
						zero2.y = -vector3.y;
						vector5.x = zero2.y * 15f;
						vector5.y = zero2.x * 15f;
					}
					else
					{
						zero2.x = zero.x;
						zero2.y = -zero.y;
						vector5.x = zero2.y * 15f;
						vector5.y = zero2.x * 15f;
					}
				}
				else
				{
					int num4 = 0;
					if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuTabLeft, true))
					{
						num4 = -1;
					}
					if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuTabRight, true))
					{
						num4 = 1;
					}
					if (num4 != 0)
					{
						this._SMLOS_ForceDirectionPersistenceTimer = 0.3f;
					}
					else if (this._SMLOS_ForceDirectionPersistenceTimer <= 0f)
					{
						this._slotForceDirection_PersistentVal = num4;
					}
					if (num4 != 0 || this._SMLOS_ForceDirectionPersistenceTimer > 0f)
					{
						if (num4 != 0 && this._slotForceDirection_PersistentVal != num4)
						{
							this._slotForceDirection_PersistentVal = num4;
						}
						switch (this._slotMachineLookingOnSides)
						{
						case CameraController.SlotMachineLookingSides.front:
							if (num4 > 0)
							{
								CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.right);
							}
							if (num4 < 0)
							{
								CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.left);
							}
							break;
						case CameraController.SlotMachineLookingSides.left:
							if (num4 > 0)
							{
								CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.front);
								this._SMLOS_DelayTimer = 0.3f;
							}
							break;
						case CameraController.SlotMachineLookingSides.right:
							if (num4 < 0)
							{
								CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.front);
								this._SMLOS_DelayTimer = 0.3f;
							}
							break;
						}
						float num5 = 0f;
						float num6 = 0f;
						if (this._slotMachineLookingOnSides == CameraController.SlotMachineLookingSides.right)
						{
							num6 = 4f;
							num5 = 65f;
						}
						else if (this._slotMachineLookingOnSides == CameraController.SlotMachineLookingSides.left)
						{
							num6 = -12.5f;
							num5 = -45f;
						}
						float num7 = 15f * Mathf.Clamp01((0.5f - this._SMLOS_DelayTimer) / 0.5f);
						if (this._slotMachineLookingOnSides != CameraController.SlotMachineLookingSides.front)
						{
							num7 = 15f;
						}
						Vector2 zero3 = Vector2.zero;
						if (this._slotForceDirection_PersistentVal != 0)
						{
							zero3.x = (float)this._slotForceDirection_PersistentVal;
						}
						vector5.x = zero2.y * num7 + num6;
						vector5.y = zero2.x * num7 + num5;
						if (num4 != 0)
						{
							SlotSideUIScript.ShowTry();
						}
					}
					else if (this.player.lastInputKindUsed == Controls.InputKind.Mouse)
					{
						zero2.x = vector3.x;
						zero2.y = -vector3.y;
						if (this._SMLOS_DelayTimer <= 0f)
						{
							switch (this._slotMachineLookingOnSides)
							{
							case CameraController.SlotMachineLookingSides.front:
								if (zero2.x > 0.42f)
								{
									CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.right);
								}
								else if (zero2.x < -0.42f)
								{
									CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.left);
								}
								break;
							case CameraController.SlotMachineLookingSides.left:
								if (zero2.x > 0.42f)
								{
									CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.front);
								}
								break;
							case CameraController.SlotMachineLookingSides.right:
								if (zero2.x < -0.42f)
								{
									CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.front);
								}
								break;
							}
						}
						float num8 = 0f;
						float num9 = 0f;
						if (this._slotMachineLookingOnSides == CameraController.SlotMachineLookingSides.right)
						{
							num9 = 4f;
							num8 = 65f;
						}
						else if (this._slotMachineLookingOnSides == CameraController.SlotMachineLookingSides.left)
						{
							num9 = -12.5f;
							num8 = -45f;
						}
						vector5.x = zero2.y * 15f + num9;
						vector5.y = zero2.x * 15f + num8;
						bool flag3 = this.eulersDifferenceMagnitude <= 5f;
						if (VirtualCursors.CursorDesiredVisibilityGet(0) != flag3)
						{
							VirtualCursors.CursorDesiredVisibilitySet(0, flag3);
						}
						if (Mathf.Abs(zero2.x) > 0.32f)
						{
							SlotSideUIScript.ShowTry();
						}
					}
					else
					{
						zero2.x = zero.x;
						zero2.y = -zero.y;
						if (this._SMLOS_DelayTimer <= 0f)
						{
							switch (this._slotMachineLookingOnSides)
							{
							case CameraController.SlotMachineLookingSides.front:
								if (zero2.x > 0.42f)
								{
									CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.right);
								}
								else if (zero2.x < -0.42f)
								{
									CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.left);
								}
								break;
							case CameraController.SlotMachineLookingSides.left:
								if (zero2.x > 0.42f)
								{
									CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.front);
								}
								this._SMLOS_DelayTimer = 0.3f;
								break;
							case CameraController.SlotMachineLookingSides.right:
								if (zero2.x < -0.42f)
								{
									CameraController.SlotMachineLook_Set(CameraController.SlotMachineLookingSides.front);
								}
								this._SMLOS_DelayTimer = 0.3f;
								break;
							}
						}
						float num10 = 0f;
						float num11 = 0f;
						if (this._slotMachineLookingOnSides == CameraController.SlotMachineLookingSides.right)
						{
							num11 = 4f;
							num10 = 65f;
						}
						else if (this._slotMachineLookingOnSides == CameraController.SlotMachineLookingSides.left)
						{
							num11 = -12.5f;
							num10 = -45f;
						}
						float num12 = 15f * Mathf.Clamp01((0.5f - this._SMLOS_DelayTimer) / 0.5f);
						if (this._slotMachineLookingOnSides != CameraController.SlotMachineLookingSides.front)
						{
							num12 = 15f;
						}
						vector5.x = zero2.y * num12 + num11;
						vector5.y = zero2.x * num12 + num10;
						if (zero2.magnitude > 0.15f)
						{
							SlotSideUIScript.ShowTry();
						}
					}
				}
				this.offsetEulers = new Vector3(vector5.x, vector5.y, 0f);
				if (gamePhase == GameplayMaster.GamePhase.gambling)
				{
					num2 *= 2f;
				}
				if (this._slotMachineLookingOnSides == CameraController.SlotMachineLookingSides.right)
				{
					CameraGame.FieldOfViewExtraSet("SMLRT", 10f);
				}
				else
				{
					CameraGame.FieldOfViewExtraSet("SMLRT", 0f);
				}
				if (this._slotMachineLookingOnSides != CameraController.SlotMachineLookingSides.front && this._slotMachineLookingOnSides != CameraController.SlotMachineLookingSides.undefined)
				{
					SlotSideUIScript.ShowTry();
					goto IL_0D1F;
				}
				goto IL_0D1F;
			}
			case CameraController.PositionKind.SlotCoinPlate_Fixed:
				num2 *= 2f;
				goto IL_0D1F;
			case CameraController.PositionKind.SlotFromTop:
			case CameraController.PositionKind.ATM:
			case CameraController.PositionKind.ATMStraight:
			case CameraController.PositionKind.Drawer0:
			case CameraController.PositionKind.Drawer1:
			case CameraController.PositionKind.Drawer2:
			case CameraController.PositionKind.Drawer3:
			case CameraController.PositionKind.MenuDrawer_Menu:
			case CameraController.PositionKind.RewardBox:
			case CameraController.PositionKind.RoomTopView:
			case CameraController.PositionKind.CloverTicketsMachine:
			case CameraController.PositionKind.doorEndingScene:
			case CameraController.PositionKind.terminal:
			case CameraController.PositionKind.DeadlineBonus:
			case CameraController.PositionKind.PhoneDoor:
			case CameraController.PositionKind.SlotScreenCloseUp:
			case CameraController.PositionKind.ToyPhone:
			case CameraController.PositionKind.Drawer0Front:
			case CameraController.PositionKind.Drawer1Front:
			case CameraController.PositionKind.Drawer2Front:
			case CameraController.PositionKind.Drawer3Front:
			case CameraController.PositionKind.DrawersAll:
			case CameraController.PositionKind.DeckBox:
			case CameraController.PositionKind.CranePack:
				goto IL_0D1F;
			case CameraController.PositionKind.Store:
				if (this.player.lastInputKindUsed == Controls.InputKind.Mouse)
				{
					this.offsetEulers = new Vector3(-vector3.y * 15f, vector3.x * 15f, 0f);
					goto IL_0D1F;
				}
				this.offsetEulers = new Vector3(-zero.y * vector.x * 15f, zero.x * vector.y * 15f, 0f);
				goto IL_0D1F;
			case CameraController.PositionKind.TrapDoor:
				this.offsetPosition = new Vector3(global::UnityEngine.Random.Range(-0.1f, 0.1f), global::UnityEngine.Random.Range(-0.1f, 0.1f), global::UnityEngine.Random.Range(-0.1f, 0.1f));
				goto IL_0D1F;
			case CameraController.PositionKind.Falling:
			{
				bool flag4 = StatsScript.IsEnabled();
				float num13 = (float)Data.settings.transitionSpeed;
				if (GameplayMaster.restartQuickDeath)
				{
					num13 = 4f;
				}
				float num14 = this.deathMaxFallSpeed * num13;
				CameraGame.Shake(this.deathFallSpeed / num14 * 4f);
				this.deathFallSpeed -= this.deathCameraGravity * Tick.Time * num13;
				this.deathFallSpeed = Mathf.Max(this.deathFallSpeed, num14);
				if (!flag4)
				{
					this.deathCameraY += this.deathFallSpeed * Tick.Time;
				}
				else
				{
					this.deathCameraY = -256f;
				}
				this.fallingTransform.SetY(this.deathCameraY);
				if (!StatsScript.IsEnabled())
				{
					float num15 = Mathf.Abs(this.deathFallSpeed);
					this.fogStartDist -= num15 * Tick.Time;
					this.fogEndDist -= num15 * Tick.Time;
				}
				else
				{
					this.fogStartDist = CameraController.fogStartDist_Default;
					this.fogEndDist = CameraController.fogEndDist_Default;
				}
				RenderSettings.fogStartDistance = this.fogStartDist;
				RenderSettings.fogEndDistance = this.fogEndDist;
				goto IL_0D1F;
			}
			case CameraController.PositionKind.WcPiss:
				if (this.player.lastInputKindUsed == Controls.InputKind.Mouse)
				{
					this.offsetEulers = new Vector3(-vector3.y * 15f, vector3.x * 15f, 0f);
				}
				else
				{
					this.offsetEulers = new Vector3(-zero.y * vector.x * 15f, zero.x * vector.y * 15f, 0f);
				}
				num2 *= 2f;
				goto IL_0D1F;
			case CameraController.PositionKind.WcPoop:
				if (this.player.lastInputKindUsed == Controls.InputKind.Mouse)
				{
					this.offsetEulers = new Vector3(-vector3.y * 15f, vector3.x * 15f, 0f);
				}
				else
				{
					this.offsetEulers = new Vector3(-zero.y * vector.x * 15f, zero.x * vector.y * 15f, 0f);
				}
				num2 *= 2f;
				goto IL_0D1F;
			}
			Debug.LogError("CameraController.Update(): Unhandled PositionKind: " + this.positionKind.ToString());
		}
		IL_0D1F:
		if (this.targetTransform != null)
		{
			Vector3 eulerAngles = this.myCamera.transform.eulerAngles;
			Vector3 vector6 = this.targetTransform.eulerAngles + this.offsetEulers;
			vector6.x = Mathf.Repeat(vector6.x, 360f);
			vector6.y = Mathf.Repeat(vector6.y, 360f);
			vector6.z = Mathf.Repeat(vector6.z, 360f);
			if (eulerAngles.x > vector6.x + 180f)
			{
				eulerAngles.x -= 360f;
			}
			if (eulerAngles.x < vector6.x - 180f)
			{
				eulerAngles.x += 360f;
			}
			if (eulerAngles.y > vector6.y + 180f)
			{
				eulerAngles.y -= 360f;
			}
			if (eulerAngles.y < vector6.y - 180f)
			{
				eulerAngles.y += 360f;
			}
			if (eulerAngles.z > vector6.z + 180f)
			{
				eulerAngles.z -= 360f;
			}
			if (eulerAngles.z < vector6.z - 180f)
			{
				eulerAngles.z += 360f;
			}
			this.myCamera.transform.eulerAngles = Vector3.Lerp(eulerAngles, vector6, num2);
			this.eulersDifferenceMagnitude = (vector6 - eulerAngles).magnitude;
			if (this.eulersDifferenceMagnitude > 180f)
			{
				this.eulersDifferenceMagnitude = 360f - this.eulersDifferenceMagnitude;
			}
			Vector3 vector7 = this.myCamera.transform.position;
			Vector3 vector8 = this.targetTransform.position + this.offsetPosition;
			bool flag5 = false;
			if (this.positionKind == CameraController.PositionKind.Slot_Fixed)
			{
				flag5 = true;
			}
			if (this.dollyZoomEnabled && !flag5)
			{
				float num16 = (this.myCamera.myCamera.fieldOfView - 60f) / this.dollyZoomDivider;
				Vector3 vector9 = this.myCamera.transform.forward * num16;
				vector8 += vector9;
			}
			vector7 += (vector8 - vector7) * num;
			this.myCamera.transform.position = vector7;
			this.positionDifferenceMagnitude = (vector8 - vector7).magnitude;
		}
	}

	// Token: 0x04000025 RID: 37
	public static CameraController instance = null;

	// Token: 0x04000026 RID: 38
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000027 RID: 39
	private CameraGame myCamera;

	// Token: 0x04000028 RID: 40
	private Controls.PlayerExt player;

	// Token: 0x04000029 RID: 41
	public Transform freeCamTransform;

	// Token: 0x0400002A RID: 42
	public Transform slotTransform;

	// Token: 0x0400002B RID: 43
	public Transform slotCoinPlateTransform;

	// Token: 0x0400002C RID: 44
	public Transform slotFromTopTransform;

	// Token: 0x0400002D RID: 45
	public Transform ATMTransform;

	// Token: 0x0400002E RID: 46
	public Transform StoreTransform;

	// Token: 0x0400002F RID: 47
	public Transform ATMStraightTransform;

	// Token: 0x04000030 RID: 48
	public Transform trapDoorTransform;

	// Token: 0x04000031 RID: 49
	public Transform fallingTransform;

	// Token: 0x04000032 RID: 50
	public Transform drawer0;

	// Token: 0x04000033 RID: 51
	public Transform drawer1;

	// Token: 0x04000034 RID: 52
	public Transform drawer2;

	// Token: 0x04000035 RID: 53
	public Transform drawer3;

	// Token: 0x04000036 RID: 54
	public Transform menuDrawer_Menu;

	// Token: 0x04000037 RID: 55
	public Transform rewardBoxTransform;

	// Token: 0x04000038 RID: 56
	public Transform roomTopViewTransform;

	// Token: 0x04000039 RID: 57
	public Transform cloverTicketsMachineTransform;

	// Token: 0x0400003A RID: 58
	public Transform doorEndingTransform;

	// Token: 0x0400003B RID: 59
	public Transform terminalTransform;

	// Token: 0x0400003C RID: 60
	public Transform deadlineBonusTransform;

	// Token: 0x0400003D RID: 61
	public Transform phoneDoorTransform;

	// Token: 0x0400003E RID: 62
	public Transform slotScreenCloseUpTransform;

	// Token: 0x0400003F RID: 63
	public Transform toyPhoneTransform;

	// Token: 0x04000040 RID: 64
	public Transform wcPiss;

	// Token: 0x04000041 RID: 65
	public Transform wcPoop;

	// Token: 0x04000042 RID: 66
	public Transform drawer0Front;

	// Token: 0x04000043 RID: 67
	public Transform drawer1Front;

	// Token: 0x04000044 RID: 68
	public Transform drawer2Front;

	// Token: 0x04000045 RID: 69
	public Transform drawer3Front;

	// Token: 0x04000046 RID: 70
	public Transform drawersAllTransform;

	// Token: 0x04000047 RID: 71
	public Transform deckBoxTransform;

	// Token: 0x04000048 RID: 72
	public Transform cranePackTransform;

	// Token: 0x04000049 RID: 73
	private CameraController.PositionKind positionKind;

	// Token: 0x0400004A RID: 74
	private Transform targetTransform;

	// Token: 0x0400004B RID: 75
	private List<string> disableReasons = new List<string>();

	// Token: 0x0400004C RID: 76
	private bool _canFreeLook;

	// Token: 0x0400004D RID: 77
	private float lerpSpeedMultiplier = 1f;

	// Token: 0x0400004E RID: 78
	private float lerpSpeedNormalizationSpeed = 3f;

	// Token: 0x0400004F RID: 79
	private float lerpSpeedNormalizationSpeed_Default = 3f;

	// Token: 0x04000050 RID: 80
	private Vector3 offsetPosition;

	// Token: 0x04000051 RID: 81
	private Vector3 offsetEulers;

	// Token: 0x04000052 RID: 82
	private float eulersDifferenceMagnitude;

	// Token: 0x04000053 RID: 83
	private float positionDifferenceMagnitude;

	// Token: 0x04000054 RID: 84
	private Vector3 freeCameraTransformStartingPosition;

	// Token: 0x04000055 RID: 85
	private Vector3 freeCameraTransformStartingRotation;

	// Token: 0x04000056 RID: 86
	private Vector3 freeCameraRotation;

	// Token: 0x04000057 RID: 87
	private static float fogStartDist_Default = -1f;

	// Token: 0x04000058 RID: 88
	private static float fogEndDist_Default = -1f;

	// Token: 0x04000059 RID: 89
	private float fogStartDist = -1f;

	// Token: 0x0400005A RID: 90
	private float fogEndDist = -1f;

	// Token: 0x0400005B RID: 91
	private const string FOV_EXTRA_TAG_LOOK_UP_DOWN = "LUDS";

	// Token: 0x0400005C RID: 92
	private const float CAM_FOV_SPD = 5f;

	// Token: 0x0400005D RID: 93
	private bool lookdDown_ScarySoundPlayed;

	// Token: 0x0400005E RID: 94
	private bool lookdUp_ScarySoundPlayed;

	// Token: 0x0400005F RID: 95
	private float lookDown_Timer;

	// Token: 0x04000060 RID: 96
	private float lookUp_Timer;

	// Token: 0x04000061 RID: 97
	private float lookDownFov;

	// Token: 0x04000062 RID: 98
	private float lookUpFov;

	// Token: 0x04000063 RID: 99
	private bool nowIgnorePositionForLookingUp;

	// Token: 0x04000064 RID: 100
	private float deathCameraY = 6f;

	// Token: 0x04000065 RID: 101
	private float deathFallSpeed;

	// Token: 0x04000066 RID: 102
	private float deathCameraGravity = 24f;

	// Token: 0x04000067 RID: 103
	private float deathMaxFallSpeed = -32f;

	// Token: 0x04000068 RID: 104
	private float deathFallMaxHeight = -34f;

	// Token: 0x04000069 RID: 105
	private const string SLOT_MACHINE_LOOKING_RIGHT_FOV_EXTRA_TAG = "SMLRT";

	// Token: 0x0400006A RID: 106
	private CameraController.SlotMachineLookingSides _slotMachineLookingOnSides;

	// Token: 0x0400006B RID: 107
	private float _SMLOS_DelayTimer;

	// Token: 0x0400006C RID: 108
	private float _SMLOS_ForceDirectionPersistenceTimer;

	// Token: 0x0400006D RID: 109
	private int _slotForceDirection_PersistentVal;

	// Token: 0x0400006E RID: 110
	private const float DOLLY_ZOOM_DEFAULT_FOV = 60f;

	// Token: 0x0400006F RID: 111
	private float dollyZoomDivider = 25f;

	// Token: 0x04000070 RID: 112
	private bool dollyZoomEnabled = true;

	// Token: 0x04000071 RID: 113
	private Coroutine heartbeatCoroutine;

	// Token: 0x04000072 RID: 114
	private List<string> screenMenuIgnorePresence_Reasons = new List<string>();

	// Token: 0x02000008 RID: 8
	public enum PositionKind
	{
		// Token: 0x04000074 RID: 116
		Free,
		// Token: 0x04000075 RID: 117
		Slot_Fixed,
		// Token: 0x04000076 RID: 118
		SlotCoinPlate_Fixed,
		// Token: 0x04000077 RID: 119
		SlotFromTop,
		// Token: 0x04000078 RID: 120
		ATM,
		// Token: 0x04000079 RID: 121
		Store,
		// Token: 0x0400007A RID: 122
		ATMStraight,
		// Token: 0x0400007B RID: 123
		TrapDoor,
		// Token: 0x0400007C RID: 124
		Falling,
		// Token: 0x0400007D RID: 125
		Drawer0,
		// Token: 0x0400007E RID: 126
		Drawer1,
		// Token: 0x0400007F RID: 127
		Drawer2,
		// Token: 0x04000080 RID: 128
		Drawer3,
		// Token: 0x04000081 RID: 129
		MenuDrawer_Menu,
		// Token: 0x04000082 RID: 130
		MenuDrawer_PowerupsInfo,
		// Token: 0x04000083 RID: 131
		RewardBox,
		// Token: 0x04000084 RID: 132
		RoomTopView,
		// Token: 0x04000085 RID: 133
		CloverTicketsMachine,
		// Token: 0x04000086 RID: 134
		doorEndingScene,
		// Token: 0x04000087 RID: 135
		terminal,
		// Token: 0x04000088 RID: 136
		DeadlineBonus,
		// Token: 0x04000089 RID: 137
		PhoneDoor,
		// Token: 0x0400008A RID: 138
		SlotScreenCloseUp,
		// Token: 0x0400008B RID: 139
		ToyPhone,
		// Token: 0x0400008C RID: 140
		WcPiss,
		// Token: 0x0400008D RID: 141
		WcPoop,
		// Token: 0x0400008E RID: 142
		Drawer0Front,
		// Token: 0x0400008F RID: 143
		Drawer1Front,
		// Token: 0x04000090 RID: 144
		Drawer2Front,
		// Token: 0x04000091 RID: 145
		Drawer3Front,
		// Token: 0x04000092 RID: 146
		DrawersAll,
		// Token: 0x04000093 RID: 147
		DeckBox,
		// Token: 0x04000094 RID: 148
		CranePack,
		// Token: 0x04000095 RID: 149
		Count,
		// Token: 0x04000096 RID: 150
		Undefined
	}

	// Token: 0x02000009 RID: 9
	public enum SlotMachineLookingSides
	{
		// Token: 0x04000098 RID: 152
		undefined,
		// Token: 0x04000099 RID: 153
		front,
		// Token: 0x0400009A RID: 154
		left,
		// Token: 0x0400009B RID: 155
		right
	}
}
