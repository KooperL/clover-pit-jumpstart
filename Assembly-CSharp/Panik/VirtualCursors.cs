using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Panik
{
	// Token: 0x0200012B RID: 299
	public class VirtualCursors : MonoBehaviour
	{
		// Token: 0x06000E85 RID: 3717 RVA: 0x0006B824 File Offset: 0x00069A24
		public Vector2 GetMouseSpeedMultiplier_FromResolution()
		{
			Vector2 vector = new Vector2((float)Display.main.systemWidth, (float)Display.main.systemHeight);
			return new Vector2(vector.x / this.referenceResolution.x, vector.y / this.referenceResolution.y);
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x0006B878 File Offset: 0x00069A78
		public float GetResolutionMaxDiffMult()
		{
			Vector2 mouseSpeedMultiplier_FromResolution = this.GetMouseSpeedMultiplier_FromResolution();
			return Mathf.Max(mouseSpeedMultiplier_FromResolution.x, mouseSpeedMultiplier_FromResolution.y);
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x00011A6E File Offset: 0x0000FC6E
		public static void CursorDesiredVisibilitySet(int playerIndex, bool value)
		{
			if (VirtualCursors.instance == null)
			{
				return;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				return;
			}
			VirtualCursors.instance.cursordDesiredVisibility[playerIndex] = value;
		}

		// Token: 0x06000E88 RID: 3720 RVA: 0x00011AA2 File Offset: 0x0000FCA2
		public static bool CursorDesiredVisibilityGet(int playerIndex)
		{
			return !(VirtualCursors.instance == null) && playerIndex >= 0 && playerIndex < VirtualCursors.instance.cursorImages.Count && VirtualCursors.instance.cursordDesiredVisibility[playerIndex];
		}

		// Token: 0x06000E89 RID: 3721 RVA: 0x00011AD7 File Offset: 0x0000FCD7
		public static void CursorDesiredVisibility_AskToHide_Kindly(int playerIndex)
		{
			VirtualCursors.instance.cursorDesiredVisibility_KindlyAskToHidePrettyPlease[playerIndex] = true;
		}

		// Token: 0x06000E8A RID: 3722 RVA: 0x00011AE6 File Offset: 0x0000FCE6
		public static void CursorSmartVisibility_EnableSet(bool enable)
		{
			VirtualCursors.cursorSmartVisibility_EnableState = enable;
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00011AEE File Offset: 0x0000FCEE
		public static bool CursorSmartVisibility_EnableGet()
		{
			return VirtualCursors.cursorSmartVisibility_EnableState;
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x00011AF5 File Offset: 0x0000FCF5
		public static bool IsCursorVisible(int playerIndex, bool considerPlayingState = true)
		{
			return !(VirtualCursors.instance == null) && (!considerPlayingState || Controls.PlayerIsPlaying(playerIndex)) && VirtualCursors.instance.cursorImagesVisibleState[playerIndex];
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00011B28 File Offset: 0x0000FD28
		public static void CursorScale_Set(int playerIndex, float value)
		{
			if (VirtualCursors.instance == null)
			{
				return;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				return;
			}
			VirtualCursors.instance.scale[playerIndex] = value;
		}

		// Token: 0x06000E8E RID: 3726 RVA: 0x0006B8A0 File Offset: 0x00069AA0
		public static void CursorScale_SetAll(float value)
		{
			if (VirtualCursors.instance == null)
			{
				return;
			}
			for (int i = 0; i < VirtualCursors.instance.cursorImages.Count; i++)
			{
				VirtualCursors.instance.scale[i] = value;
			}
		}

		// Token: 0x06000E8F RID: 3727 RVA: 0x00011B5C File Offset: 0x0000FD5C
		public static void CursorTargetScale_Set(int playerIndex, float value)
		{
			if (VirtualCursors.instance == null)
			{
				return;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				return;
			}
			VirtualCursors.instance.scaleTarget[playerIndex] = value;
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x0006B8E4 File Offset: 0x00069AE4
		public static void CursorTargetScale_SetAll(float value)
		{
			if (VirtualCursors.instance == null)
			{
				return;
			}
			for (int i = 0; i < VirtualCursors.instance.cursorImages.Count; i++)
			{
				VirtualCursors.instance.scaleTarget[i] = value;
			}
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x0006B928 File Offset: 0x00069B28
		public static void CursorSet(int playerIndex, string spriteName)
		{
			if (VirtualCursors.instance == null)
			{
				return;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				return;
			}
			if (!Controls.PlayerIsPlaying(playerIndex))
			{
				return;
			}
			if (string.IsNullOrEmpty(spriteName))
			{
				return;
			}
			VirtualCursors.instance.cursorImageAnimators[playerIndex].AnimationName = spriteName;
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x00011B90 File Offset: 0x0000FD90
		public static void CursorSet(Controls.PlayerExt player, string spriteName)
		{
			VirtualCursors.CursorSet(Controls.GetPlayerIndex(player), spriteName);
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x0006B984 File Offset: 0x00069B84
		public static void CursorSetAll(string spriteName)
		{
			for (int i = 0; i < VirtualCursors.instance.cursorImages.Count; i++)
			{
				VirtualCursors.CursorSet(i, spriteName);
			}
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x00011B9E File Offset: 0x0000FD9E
		public static void CursorSetDefault(int playerIndex)
		{
			VirtualCursors.CursorSet(playerIndex, VirtualCursors.instance.cursorImageAnimators[playerIndex].defaultAnimation.name);
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00011BC0 File Offset: 0x0000FDC0
		public static void CursorSetDefault(Controls.PlayerExt player)
		{
			VirtualCursors.CursorSetDefault(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x0006B9B4 File Offset: 0x00069BB4
		public static void CursorSetDefaultAll()
		{
			for (int i = 0; i < VirtualCursors.instance.cursorImages.Count; i++)
			{
				VirtualCursors.CursorSetDefault(i);
			}
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00011BCD File Offset: 0x0000FDCD
		public static string CursorNameGet(int playerIndex)
		{
			if (VirtualCursors.instance == null)
			{
				return null;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				return null;
			}
			return VirtualCursors.instance.cursorImageAnimators[playerIndex].AnimationName;
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00011C0B File Offset: 0x0000FE0B
		public static string CursorNameGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorNameGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00011C18 File Offset: 0x0000FE18
		public static FrameAnimation CursorGet(int playerIndex)
		{
			if (VirtualCursors.instance == null)
			{
				return null;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				return null;
			}
			return VirtualCursors.instance.cursorImageAnimators[playerIndex].Animation;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x00011C56 File Offset: 0x0000FE56
		public static FrameAnimation CursorGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00011C63 File Offset: 0x0000FE63
		public static FrameAnimation CursorGetDeafault(int playerIndex)
		{
			if (VirtualCursors.instance == null)
			{
				return null;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				return null;
			}
			return VirtualCursors.instance.cursorImageAnimators[playerIndex].defaultAnimation;
		}

		// Token: 0x06000E9C RID: 3740 RVA: 0x00011CA1 File Offset: 0x0000FEA1
		public static FrameAnimation CursorGetDeafault(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorGetDeafault(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000E9D RID: 3741 RVA: 0x0006B9E4 File Offset: 0x00069BE4
		public static Vector2 CursorPositionGet(int playerIndex)
		{
			if (VirtualCursors.instance == null)
			{
				return Vector2.zero;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				return Vector2.zero;
			}
			return VirtualCursors.instance.cursorImages[playerIndex].rectTransform.anchoredPosition;
		}

		// Token: 0x06000E9E RID: 3742 RVA: 0x00011CAE File Offset: 0x0000FEAE
		public static Vector2 CursorPositionGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorPositionGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000E9F RID: 3743 RVA: 0x0006BA3C File Offset: 0x00069C3C
		public static Vector2 CursorPositionNormalizedGet(int playerIndex)
		{
			if (VirtualCursors.instance == null)
			{
				return Vector2.zero;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				return Vector2.zero;
			}
			Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
			Vector2 anchoredPosition = VirtualCursors.instance.cursorImages[playerIndex].rectTransform.anchoredPosition;
			anchoredPosition.x = anchoredPosition.x / vector.x + 0.5f;
			anchoredPosition.y = anchoredPosition.y / vector.y + 0.5f;
			return anchoredPosition;
		}

		// Token: 0x06000EA0 RID: 3744 RVA: 0x00011CBB File Offset: 0x0000FEBB
		public static Vector2 CursorPositionNormalizedGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorPositionNormalizedGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x0006BADC File Offset: 0x00069CDC
		public static Vector2 CursorPositionScreenGet(int playerIndex)
		{
			if (VirtualCursors.instance == null)
			{
				return Vector2.zero;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				return Vector2.zero;
			}
			Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
			Vector2 anchoredPosition = VirtualCursors.instance.cursorImages[playerIndex].rectTransform.anchoredPosition;
			anchoredPosition.x += vector.x / 2f;
			anchoredPosition.y += vector.y / 2f;
			return anchoredPosition;
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x00011CC8 File Offset: 0x0000FEC8
		public static Vector2 CursorPositionScreenGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorPositionScreenGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x0006BB78 File Offset: 0x00069D78
		public static Vector2 CursorPositionCenteredGet_ReferenceResolution(int playerIndex, Vector2 resolution)
		{
			Vector2 vector = VirtualCursors.CursorPositionNormalizedGet(playerIndex);
			vector.x -= 0.5f;
			vector.y -= 0.5f;
			Vector2 zero = Vector2.zero;
			float num = resolution.x / resolution.y / ((float)Screen.width / (float)Screen.height);
			zero.x = vector.x * resolution.x / Mathf.Min(1f, num);
			zero.y = vector.y * resolution.y * Mathf.Max(1f, num);
			return zero;
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x00011CD5 File Offset: 0x0000FED5
		public static Vector2 CursorPositionCenteredGet_ReferenceResolution(Controls.PlayerExt player, float resolutionX, float resolutionY)
		{
			return VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(Controls.GetPlayerIndex(player), new Vector2(resolutionX, resolutionY));
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x0006BC10 File Offset: 0x00069E10
		public static Vector2 CursorPositionNormalizedCenteredGet_ReferenceResolution(int playerIndex, Vector2 resolution)
		{
			Vector2 vector = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(playerIndex, resolution);
			vector.x /= resolution.x;
			vector.y /= resolution.y;
			return vector;
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00011CE9 File Offset: 0x0000FEE9
		public static Vector2 CursorPositionNormalizedCenteredGet_ReferenceResolution(Controls.PlayerExt player, Vector2 resolution)
		{
			return VirtualCursors.CursorPositionNormalizedCenteredGet_ReferenceResolution(Controls.GetPlayerIndex(player), resolution);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x0006BC50 File Offset: 0x00069E50
		public static void CursorPositionNormalizedSet(int playerIndex, Vector2 normalizedPosition, bool scaleZero)
		{
			if (VirtualCursors.instance == null)
			{
				return;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				return;
			}
			Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
			Vector2 vector2;
			vector2.x = normalizedPosition.x * vector.x;
			vector2.y = normalizedPosition.y * vector.y;
			VirtualCursors.instance.cursorImages[playerIndex].rectTransform.anchoredPosition = vector2;
			if (scaleZero)
			{
				VirtualCursors.CursorScale_Set(playerIndex, 0f);
			}
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00011CF7 File Offset: 0x0000FEF7
		public static void CursorPositionNormalizedSet(Controls.PlayerExt player, Vector2 normalizedPosition, bool scaleZero)
		{
			VirtualCursors.CursorPositionNormalizedSet(Controls.GetPlayerIndex(player), normalizedPosition, scaleZero);
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x0006BCE8 File Offset: 0x00069EE8
		public static void CursorPositionScreenSet(int playerIndex, Vector2 screenPosition, bool scaleZero)
		{
			if (VirtualCursors.instance == null)
			{
				return;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				return;
			}
			Vector2 vector;
			vector.x = screenPosition.x;
			vector.y = screenPosition.y;
			VirtualCursors.instance.cursorImages[playerIndex].rectTransform.anchoredPosition = vector;
			if (scaleZero)
			{
				VirtualCursors.CursorScale_Set(playerIndex, 0f);
			}
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x00011D06 File Offset: 0x0000FF06
		public static void CursorPositionScreenSet(Controls.PlayerExt player, Vector2 screenPosition, bool scaleZero)
		{
			VirtualCursors.CursorPositionScreenSet(Controls.GetPlayerIndex(player), screenPosition, scaleZero);
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x00011D15 File Offset: 0x0000FF15
		public static void JoystickCursor_RightStickEnabledSet(int playerIndex, bool canMove)
		{
			if (VirtualCursors.instance == null)
			{
				return;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				return;
			}
			VirtualCursors.instance.joystickRightStickCanMoveCursor[playerIndex] = canMove;
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x00011D49 File Offset: 0x0000FF49
		public static void JoystickCursor_RightStickEnabledSet(Controls.PlayerExt player, bool canMove)
		{
			VirtualCursors.JoystickCursor_RightStickEnabledSet(Controls.GetPlayerIndex(player), canMove);
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x00011D57 File Offset: 0x0000FF57
		public static bool JoystickCursor_RightStickEnabledGet(int playerIndex)
		{
			return !(VirtualCursors.instance == null) && playerIndex >= 0 && playerIndex < VirtualCursors.instance.cursorImages.Count && VirtualCursors.instance.joystickRightStickCanMoveCursor[playerIndex];
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00011D8C File Offset: 0x0000FF8C
		public static void JoystickCursor_LeftStickEnabledSet(int playerIndex, bool canMove)
		{
			if (VirtualCursors.instance == null)
			{
				return;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				return;
			}
			VirtualCursors.instance.joystickLeftStickCanMoveCursor[playerIndex] = canMove;
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x00011DC0 File Offset: 0x0000FFC0
		public static bool JoystickCursor_LeftStickEnabledGet(int playerIndex)
		{
			return !(VirtualCursors.instance == null) && playerIndex >= 0 && playerIndex < VirtualCursors.instance.cursorImages.Count && VirtualCursors.instance.joystickLeftStickCanMoveCursor[playerIndex];
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x0006BD60 File Offset: 0x00069F60
		public static bool CursorClick_PressedGet(int playerIndex)
		{
			return !(VirtualCursors.instance == null) && playerIndex >= 0 && playerIndex < VirtualCursors.instance.cursorImages.Count && VirtualCursors.instance.cursorLeftClickState[playerIndex] && !VirtualCursors.instance.cursorLeftClickStateOld[playerIndex];
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x00011DF5 File Offset: 0x0000FFF5
		public static bool CursorClick_PressedGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorClick_PressedGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0006BDB4 File Offset: 0x00069FB4
		public static bool CursorClick_ReleasedGet(int playerIndex)
		{
			return !(VirtualCursors.instance == null) && playerIndex >= 0 && playerIndex < VirtualCursors.instance.cursorImages.Count && !VirtualCursors.instance.cursorLeftClickState[playerIndex] && VirtualCursors.instance.cursorLeftClickStateOld[playerIndex];
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00011E02 File Offset: 0x00010002
		public static bool CursorClick_ReleasedGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorClick_ReleasedGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00011E0F File Offset: 0x0001000F
		public static bool CursorClick_HoldGet(int playerIndex)
		{
			return !(VirtualCursors.instance == null) && playerIndex >= 0 && playerIndex < VirtualCursors.instance.cursorImages.Count && VirtualCursors.instance.cursorLeftClickState[playerIndex];
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00011E44 File Offset: 0x00010044
		public static bool CursorClick_HoldGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorClick_HoldGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00011E51 File Offset: 0x00010051
		public static void CursorHasMoved(int playerIndex, out bool hasMoved)
		{
			if (VirtualCursors.instance == null)
			{
				hasMoved = false;
				return;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				hasMoved = false;
				return;
			}
			hasMoved = VirtualCursors.instance.cursorHasMoved[playerIndex];
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00011E8C File Offset: 0x0001008C
		public static void CursorHasMoved(Controls.PlayerExt player, out bool hasMoved)
		{
			VirtualCursors.CursorHasMoved(Controls.GetPlayerIndex(player), out hasMoved);
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00011E9A File Offset: 0x0001009A
		public static void CursorHasMovedOld(int playerIndex, out bool hasMoved)
		{
			if (VirtualCursors.instance == null)
			{
				hasMoved = false;
				return;
			}
			if (playerIndex < 0 || playerIndex >= VirtualCursors.instance.cursorImages.Count)
			{
				hasMoved = false;
				return;
			}
			hasMoved = VirtualCursors.instance.cursorHasMovedOld[playerIndex];
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x00011ED5 File Offset: 0x000100D5
		public static void CursorHasMovedOld(Controls.PlayerExt player, out bool hasMoved)
		{
			VirtualCursors.CursorHasMovedOld(Controls.GetPlayerIndex(player), out hasMoved);
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0006BE04 File Offset: 0x0006A004
		private void Awake()
		{
			if (VirtualCursors.instance != null)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			VirtualCursors.instance = this;
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			int num = 1;
			while (this.cursorImages.Count < num)
			{
				Image component = global::UnityEngine.Object.Instantiate<GameObject>(this.cursorImageTemplate.gameObject, base.transform).GetComponent<Image>();
				this.cursorImages.Add(component);
				component.rectTransform.anchoredPosition = Vector2.zero;
			}
			this.cursorImageAnimators.Clear();
			for (int i = 0; i < this.cursorImages.Count; i++)
			{
				this.cursorImageAnimators.Add(this.cursorImages[i].GetComponent<FrameAnimator>());
				this.cursorImageAnimators[i].Animation = this.cursorImageAnimators[i].defaultAnimation;
				this.cursorImages[i].name = "Cursor - player index: " + i.ToString();
			}
			this.cursorImageTemplate.gameObject.SetActive(false);
			while (this.cursorImagesVisibleState.Count < num)
			{
				this.cursorImagesVisibleState.Add(Master.instance.SHOW_CURSOR_ON_START);
			}
			this.cursorHasMoved = new bool[num];
			this.cursorHasMovedOld = new bool[num];
			for (int j = 0; j < num; j++)
			{
				this.cursorHasMoved[j] = false;
				this.cursorHasMovedOld[j] = false;
			}
			this.joystickRightStickCanMoveCursor = new bool[num];
			this.joystickLeftStickCanMoveCursor = new bool[num];
			for (int k = 0; k < num; k++)
			{
				this.joystickRightStickCanMoveCursor[k] = false;
				this.joystickLeftStickCanMoveCursor[k] = false;
			}
			this.cursorLeftClickState = new bool[num];
			this.cursorLeftClickStateOld = new bool[num];
			this.cursordDesiredVisibility = new bool[num];
			this.cursorDesiredVisibility_KindlyAskToHidePrettyPlease = new bool[num];
			this.scale = new float[num];
			this.scaleTarget = new float[num];
			for (int l = 0; l < num; l++)
			{
				this.scale[l] = 1f;
				this.scaleTarget[l] = 1f;
			}
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x00011EE3 File Offset: 0x000100E3
		private void OnDestroy()
		{
			if (VirtualCursors.instance == this)
			{
				VirtualCursors.instance = null;
			}
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0006C024 File Offset: 0x0006A224
		private void Update()
		{
			Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
			for (int i = 0; i < this.cursorImages.Count; i++)
			{
				int num = i;
				Controls.GetPlayerByIndex(num);
				Vector2 anchoredPosition = this.cursorImages[i].rectTransform.anchoredPosition;
				bool flag = Controls.ActionButton_PressedGet(num, Controls.InputAction.menuSelect, true);
				float num2 = Data.settings.VirtualCursorSensitivityGet(num) * 4f;
				this.cursorLeftClickStateOld[i] = this.cursorLeftClickState[i];
				this.cursorLeftClickState[i] = false;
				Vector2 mouseSpeedMultiplier_FromResolution = this.GetMouseSpeedMultiplier_FromResolution();
				Vector2 vector2;
				vector2.x = Controls.MouseAxis_ValueGet(num, Controls.MouseElement.axisX);
				vector2.y = Controls.MouseAxis_ValueGet(num, Controls.MouseElement.axisY);
				vector2.x *= mouseSpeedMultiplier_FromResolution.x;
				vector2.y *= mouseSpeedMultiplier_FromResolution.y;
				vector2 *= num2;
				this.cursorImages[i].rectTransform.anchoredPosition += vector2;
				if (this.joystickRightStickCanMoveCursor[i])
				{
					vector2.x = Controls.JoystickAxis_ValueGet(num, Controls.JoystickElement.RightStickX);
					vector2.y = Controls.JoystickAxis_ValueGet(num, Controls.JoystickElement.RightStickY);
					vector2.x *= mouseSpeedMultiplier_FromResolution.x;
					vector2.y *= mouseSpeedMultiplier_FromResolution.y;
					vector2 *= 720f;
					vector2 *= num2;
					this.cursorImages[i].rectTransform.anchoredPosition += vector2 * Time.deltaTime;
				}
				if (this.joystickLeftStickCanMoveCursor[i])
				{
					vector2.x = Controls.JoystickAxis_ValueGet(num, Controls.JoystickElement.LeftStickX);
					vector2.y = Controls.JoystickAxis_ValueGet(num, Controls.JoystickElement.LeftStickY);
					vector2.x *= mouseSpeedMultiplier_FromResolution.x;
					vector2.y *= mouseSpeedMultiplier_FromResolution.y;
					vector2 *= 720f;
					vector2 *= num2;
					this.cursorImages[i].rectTransform.anchoredPosition += vector2 * Time.deltaTime;
				}
				float num3 = this.GetResolutionMaxDiffMult();
				if (this.cursorImages[i].enabled)
				{
					this.scale[i] = Mathf.Lerp(this.scale[i], this.scaleTarget[i], Time.deltaTime * 10f);
				}
				num3 *= this.scale[i];
				this.cursorImages[i].rectTransform.localScale = new Vector3(num3, num3, 1f) * 4f;
				bool flag2 = anchoredPosition != this.cursorImages[i].rectTransform.anchoredPosition;
				this.cursorHasMovedOld[i] = this.cursorHasMoved[i];
				this.cursorHasMoved[i] = flag2;
				Vector2 anchoredPosition2 = this.cursorImages[i].rectTransform.anchoredPosition;
				anchoredPosition2.x = Mathf.Clamp(anchoredPosition2.x, -vector.x / 2f, vector.x / 2f);
				anchoredPosition2.y = Mathf.Clamp(anchoredPosition2.y, -vector.y / 2f, vector.y / 2f);
				this.cursorImages[i].rectTransform.anchoredPosition = anchoredPosition2;
				if (VirtualCursors.cursorSmartVisibility_EnableState)
				{
					bool flag3 = Controls.MouseButton_PressedGet(num, Controls.MouseElement.LeftButton);
					bool flag4 = Controls.playersExtList[num].lastInputKindUsed == Controls.InputKind.Keyboard;
					bool flag5 = Controls.playersExtList[num].lastInputKindUsed == Controls.InputKind.Joystick && !this.joystickLeftStickCanMoveCursor[i] && !this.joystickRightStickCanMoveCursor[i];
					bool flag6 = this.cursorDesiredVisibility_KindlyAskToHidePrettyPlease[i];
					this.cursorDesiredVisibility_KindlyAskToHidePrettyPlease[i] = false;
					if (flag2 || flag3)
					{
						this.cursorImagesVisibleState[i] = true;
						UnityAction unityAction = this.onSmartVisibilitySwitch_On;
						if (unityAction != null)
						{
							unityAction();
						}
					}
					else if (Controls.MenuDirectionalAny_PressedGet(num) || (flag && (flag5 || flag4)) || flag6)
					{
						this.cursorImagesVisibleState[i] = false;
						UnityAction unityAction2 = this.onSmartVisibilitySwitch_Off;
						if (unityAction2 != null)
						{
							unityAction2();
						}
					}
				}
				if (this.cursorImagesVisibleState[i])
				{
					this.cursorImagesVisibleState[i] = this.cursordDesiredVisibility[i];
				}
				bool flag7 = Controls.PlayerIsPlaying(num);
				bool flag8 = this.cursorImagesVisibleState[i] && flag7;
				if (this.cursorImages[i].enabled != flag8)
				{
					this.cursorImages[i].enabled = flag8;
				}
				if (flag8)
				{
					if (Controls.MouseButton_HoldGet(num, Controls.MouseElement.LeftButton))
					{
						this.cursorLeftClickState[i] = true;
					}
					if ((this.joystickLeftStickCanMoveCursor[i] || this.joystickRightStickCanMoveCursor[i]) && flag && Controls.playersExtList[num].lastInputKindUsed == Controls.InputKind.Joystick)
					{
						this.cursorLeftClickState[i] = true;
					}
				}
			}
		}

		// Token: 0x04000F34 RID: 3892
		public static VirtualCursors instance = null;

		// Token: 0x04000F35 RID: 3893
		private const bool HIDE_SYSTEM_MOUSE = true;

		// Token: 0x04000F36 RID: 3894
		private const bool JOYSTICK_CURSOR_RIGHT_STICK_DEFAULT = false;

		// Token: 0x04000F37 RID: 3895
		private const bool JOYSTICK_CURSOR_LEFT_STICK_DEFAULT = false;

		// Token: 0x04000F38 RID: 3896
		private const float JOYSTICK_CURSOR_SPEED = 1440f;

		// Token: 0x04000F39 RID: 3897
		public const bool SMART_VISIBILITY_FEATURE_CAN_BE_TOGGLED = true;

		// Token: 0x04000F3A RID: 3898
		public const bool SMART_VISIBILITY_INITIAL_FEATURE_STATE = true;

		// Token: 0x04000F3B RID: 3899
		public const bool RESET_CURSOR_POSITION_WHEN_INVISIBLE = false;

		// Token: 0x04000F3C RID: 3900
		public const bool RESET_SPRITE_ANIMATION_WHEN_INVISIBLE = false;

		// Token: 0x04000F3D RID: 3901
		public const float SENSITIVITY_MULT = 4f;

		// Token: 0x04000F3E RID: 3902
		public const float SENSITIVITY_MULT_GAMEPAD = 0.5f;

		// Token: 0x04000F3F RID: 3903
		public const float SCALE_MULT = 4f;

		// Token: 0x04000F40 RID: 3904
		public Vector2 referenceResolution = new Vector2(1920f, 1080f);

		// Token: 0x04000F41 RID: 3905
		private List<Image> cursorImages = new List<Image>();

		// Token: 0x04000F42 RID: 3906
		private List<bool> cursorImagesVisibleState = new List<bool>();

		// Token: 0x04000F43 RID: 3907
		private List<FrameAnimator> cursorImageAnimators = new List<FrameAnimator>();

		// Token: 0x04000F44 RID: 3908
		public Image cursorImageTemplate;

		// Token: 0x04000F45 RID: 3909
		private bool[] cursorLeftClickState;

		// Token: 0x04000F46 RID: 3910
		private bool[] cursorLeftClickStateOld;

		// Token: 0x04000F47 RID: 3911
		private bool[] cursorHasMoved;

		// Token: 0x04000F48 RID: 3912
		private bool[] cursorHasMovedOld;

		// Token: 0x04000F49 RID: 3913
		private bool[] joystickRightStickCanMoveCursor;

		// Token: 0x04000F4A RID: 3914
		private bool[] joystickLeftStickCanMoveCursor;

		// Token: 0x04000F4B RID: 3915
		private bool[] cursordDesiredVisibility;

		// Token: 0x04000F4C RID: 3916
		private bool[] cursorDesiredVisibility_KindlyAskToHidePrettyPlease;

		// Token: 0x04000F4D RID: 3917
		private static bool cursorSmartVisibility_EnableState = true;

		// Token: 0x04000F4E RID: 3918
		private float[] scale;

		// Token: 0x04000F4F RID: 3919
		private float[] scaleTarget;

		// Token: 0x04000F50 RID: 3920
		public UnityAction onSmartVisibilitySwitch_On;

		// Token: 0x04000F51 RID: 3921
		public UnityAction onSmartVisibilitySwitch_Off;
	}
}
