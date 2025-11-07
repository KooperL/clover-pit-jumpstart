using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Panik
{
	public class VirtualCursors : MonoBehaviour
	{
		// Token: 0x06000C62 RID: 3170 RVA: 0x00051730 File Offset: 0x0004F930
		public Vector2 GetMouseSpeedMultiplier_FromResolution()
		{
			Vector2 vector;
			vector..ctor((float)Display.main.systemWidth, (float)Display.main.systemHeight);
			return new Vector2(vector.x / this.referenceResolution.x, vector.y / this.referenceResolution.y);
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00051784 File Offset: 0x0004F984
		public float GetResolutionMaxDiffMult()
		{
			Vector2 mouseSpeedMultiplier_FromResolution = this.GetMouseSpeedMultiplier_FromResolution();
			return Mathf.Max(mouseSpeedMultiplier_FromResolution.x, mouseSpeedMultiplier_FromResolution.y);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x000517A9 File Offset: 0x0004F9A9
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

		// Token: 0x06000C65 RID: 3173 RVA: 0x000517DD File Offset: 0x0004F9DD
		public static bool CursorDesiredVisibilityGet(int playerIndex)
		{
			return !(VirtualCursors.instance == null) && playerIndex >= 0 && playerIndex < VirtualCursors.instance.cursorImages.Count && VirtualCursors.instance.cursordDesiredVisibility[playerIndex];
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x00051812 File Offset: 0x0004FA12
		public static void CursorDesiredVisibility_AskToHide_Kindly(int playerIndex)
		{
			VirtualCursors.instance.cursorDesiredVisibility_KindlyAskToHidePrettyPlease[playerIndex] = true;
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00051821 File Offset: 0x0004FA21
		public static void CursorSmartVisibility_EnableSet(bool enable)
		{
			VirtualCursors.cursorSmartVisibility_EnableState = enable;
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00051829 File Offset: 0x0004FA29
		public static bool CursorSmartVisibility_EnableGet()
		{
			return VirtualCursors.cursorSmartVisibility_EnableState;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00051830 File Offset: 0x0004FA30
		public static bool IsCursorVisible(int playerIndex, bool considerPlayingState = true)
		{
			return !(VirtualCursors.instance == null) && (!considerPlayingState || Controls.PlayerIsPlaying(playerIndex)) && VirtualCursors.instance.cursorImagesVisibleState[playerIndex];
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00051863 File Offset: 0x0004FA63
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

		// Token: 0x06000C6B RID: 3179 RVA: 0x00051898 File Offset: 0x0004FA98
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

		// Token: 0x06000C6C RID: 3180 RVA: 0x000518DA File Offset: 0x0004FADA
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

		// Token: 0x06000C6D RID: 3181 RVA: 0x00051910 File Offset: 0x0004FB10
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

		// Token: 0x06000C6E RID: 3182 RVA: 0x00051954 File Offset: 0x0004FB54
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

		// Token: 0x06000C6F RID: 3183 RVA: 0x000519AE File Offset: 0x0004FBAE
		public static void CursorSet(Controls.PlayerExt player, string spriteName)
		{
			VirtualCursors.CursorSet(Controls.GetPlayerIndex(player), spriteName);
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x000519BC File Offset: 0x0004FBBC
		public static void CursorSetAll(string spriteName)
		{
			for (int i = 0; i < VirtualCursors.instance.cursorImages.Count; i++)
			{
				VirtualCursors.CursorSet(i, spriteName);
			}
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x000519EA File Offset: 0x0004FBEA
		public static void CursorSetDefault(int playerIndex)
		{
			VirtualCursors.CursorSet(playerIndex, VirtualCursors.instance.cursorImageAnimators[playerIndex].defaultAnimation.name);
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00051A0C File Offset: 0x0004FC0C
		public static void CursorSetDefault(Controls.PlayerExt player)
		{
			VirtualCursors.CursorSetDefault(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00051A1C File Offset: 0x0004FC1C
		public static void CursorSetDefaultAll()
		{
			for (int i = 0; i < VirtualCursors.instance.cursorImages.Count; i++)
			{
				VirtualCursors.CursorSetDefault(i);
			}
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x00051A49 File Offset: 0x0004FC49
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

		// Token: 0x06000C75 RID: 3189 RVA: 0x00051A87 File Offset: 0x0004FC87
		public static string CursorNameGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorNameGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00051A94 File Offset: 0x0004FC94
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

		// Token: 0x06000C77 RID: 3191 RVA: 0x00051AD2 File Offset: 0x0004FCD2
		public static FrameAnimation CursorGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00051ADF File Offset: 0x0004FCDF
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

		// Token: 0x06000C79 RID: 3193 RVA: 0x00051B1D File Offset: 0x0004FD1D
		public static FrameAnimation CursorGetDeafault(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorGetDeafault(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x00051B2C File Offset: 0x0004FD2C
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

		// Token: 0x06000C7B RID: 3195 RVA: 0x00051B82 File Offset: 0x0004FD82
		public static Vector2 CursorPositionGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorPositionGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00051B90 File Offset: 0x0004FD90
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
			Vector2 vector;
			vector..ctor((float)Screen.width, (float)Screen.height);
			Vector2 anchoredPosition = VirtualCursors.instance.cursorImages[playerIndex].rectTransform.anchoredPosition;
			anchoredPosition.x = anchoredPosition.x / vector.x + 0.5f;
			anchoredPosition.y = anchoredPosition.y / vector.y + 0.5f;
			return anchoredPosition;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x00051C2F File Offset: 0x0004FE2F
		public static Vector2 CursorPositionNormalizedGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorPositionNormalizedGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x00051C3C File Offset: 0x0004FE3C
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
			Vector2 vector;
			vector..ctor((float)Screen.width, (float)Screen.height);
			Vector2 anchoredPosition = VirtualCursors.instance.cursorImages[playerIndex].rectTransform.anchoredPosition;
			anchoredPosition.x += vector.x / 2f;
			anchoredPosition.y += vector.y / 2f;
			return anchoredPosition;
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x00051CD5 File Offset: 0x0004FED5
		public static Vector2 CursorPositionScreenGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorPositionScreenGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00051CE4 File Offset: 0x0004FEE4
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

		// Token: 0x06000C81 RID: 3201 RVA: 0x00051D7B File Offset: 0x0004FF7B
		public static Vector2 CursorPositionCenteredGet_ReferenceResolution(Controls.PlayerExt player, float resolutionX, float resolutionY)
		{
			return VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(Controls.GetPlayerIndex(player), new Vector2(resolutionX, resolutionY));
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x00051D90 File Offset: 0x0004FF90
		public static Vector2 CursorPositionNormalizedCenteredGet_ReferenceResolution(int playerIndex, Vector2 resolution)
		{
			Vector2 vector = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(playerIndex, resolution);
			vector.x /= resolution.x;
			vector.y /= resolution.y;
			return vector;
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x00051DCE File Offset: 0x0004FFCE
		public static Vector2 CursorPositionNormalizedCenteredGet_ReferenceResolution(Controls.PlayerExt player, Vector2 resolution)
		{
			return VirtualCursors.CursorPositionNormalizedCenteredGet_ReferenceResolution(Controls.GetPlayerIndex(player), resolution);
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x00051DDC File Offset: 0x0004FFDC
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
			Vector2 vector;
			vector..ctor((float)Screen.width, (float)Screen.height);
			Vector2 vector2;
			vector2.x = normalizedPosition.x * vector.x;
			vector2.y = normalizedPosition.y * vector.y;
			VirtualCursors.instance.cursorImages[playerIndex].rectTransform.anchoredPosition = vector2;
			if (scaleZero)
			{
				VirtualCursors.CursorScale_Set(playerIndex, 0f);
			}
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x00051E72 File Offset: 0x00050072
		public static void CursorPositionNormalizedSet(Controls.PlayerExt player, Vector2 normalizedPosition, bool scaleZero)
		{
			VirtualCursors.CursorPositionNormalizedSet(Controls.GetPlayerIndex(player), normalizedPosition, scaleZero);
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x00051E84 File Offset: 0x00050084
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

		// Token: 0x06000C87 RID: 3207 RVA: 0x00051EF9 File Offset: 0x000500F9
		public static void CursorPositionScreenSet(Controls.PlayerExt player, Vector2 screenPosition, bool scaleZero)
		{
			VirtualCursors.CursorPositionScreenSet(Controls.GetPlayerIndex(player), screenPosition, scaleZero);
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00051F08 File Offset: 0x00050108
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

		// Token: 0x06000C89 RID: 3209 RVA: 0x00051F3C File Offset: 0x0005013C
		public static void JoystickCursor_RightStickEnabledSet(Controls.PlayerExt player, bool canMove)
		{
			VirtualCursors.JoystickCursor_RightStickEnabledSet(Controls.GetPlayerIndex(player), canMove);
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00051F4A File Offset: 0x0005014A
		public static bool JoystickCursor_RightStickEnabledGet(int playerIndex)
		{
			return !(VirtualCursors.instance == null) && playerIndex >= 0 && playerIndex < VirtualCursors.instance.cursorImages.Count && VirtualCursors.instance.joystickRightStickCanMoveCursor[playerIndex];
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00051F7F File Offset: 0x0005017F
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

		// Token: 0x06000C8C RID: 3212 RVA: 0x00051FB3 File Offset: 0x000501B3
		public static bool JoystickCursor_LeftStickEnabledGet(int playerIndex)
		{
			return !(VirtualCursors.instance == null) && playerIndex >= 0 && playerIndex < VirtualCursors.instance.cursorImages.Count && VirtualCursors.instance.joystickLeftStickCanMoveCursor[playerIndex];
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x00051FE8 File Offset: 0x000501E8
		public static bool CursorClick_PressedGet(int playerIndex)
		{
			return !(VirtualCursors.instance == null) && playerIndex >= 0 && playerIndex < VirtualCursors.instance.cursorImages.Count && VirtualCursors.instance.cursorLeftClickState[playerIndex] && !VirtualCursors.instance.cursorLeftClickStateOld[playerIndex];
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x0005203B File Offset: 0x0005023B
		public static bool CursorClick_PressedGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorClick_PressedGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00052048 File Offset: 0x00050248
		public static bool CursorClick_ReleasedGet(int playerIndex)
		{
			return !(VirtualCursors.instance == null) && playerIndex >= 0 && playerIndex < VirtualCursors.instance.cursorImages.Count && !VirtualCursors.instance.cursorLeftClickState[playerIndex] && VirtualCursors.instance.cursorLeftClickStateOld[playerIndex];
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00052098 File Offset: 0x00050298
		public static bool CursorClick_ReleasedGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorClick_ReleasedGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x000520A5 File Offset: 0x000502A5
		public static bool CursorClick_HoldGet(int playerIndex)
		{
			return !(VirtualCursors.instance == null) && playerIndex >= 0 && playerIndex < VirtualCursors.instance.cursorImages.Count && VirtualCursors.instance.cursorLeftClickState[playerIndex];
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x000520DA File Offset: 0x000502DA
		public static bool CursorClick_HoldGet(Controls.PlayerExt player)
		{
			return VirtualCursors.CursorClick_HoldGet(Controls.GetPlayerIndex(player));
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x000520E7 File Offset: 0x000502E7
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

		// Token: 0x06000C94 RID: 3220 RVA: 0x00052122 File Offset: 0x00050322
		public static void CursorHasMoved(Controls.PlayerExt player, out bool hasMoved)
		{
			VirtualCursors.CursorHasMoved(Controls.GetPlayerIndex(player), out hasMoved);
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00052130 File Offset: 0x00050330
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

		// Token: 0x06000C96 RID: 3222 RVA: 0x0005216B File Offset: 0x0005036B
		public static void CursorHasMovedOld(Controls.PlayerExt player, out bool hasMoved)
		{
			VirtualCursors.CursorHasMovedOld(Controls.GetPlayerIndex(player), out hasMoved);
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0005217C File Offset: 0x0005037C
		private void Awake()
		{
			if (VirtualCursors.instance != null)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			VirtualCursors.instance = this;
			Cursor.visible = false;
			Cursor.lockState = 1;
			int num = 1;
			while (this.cursorImages.Count < num)
			{
				Image component = Object.Instantiate<GameObject>(this.cursorImageTemplate.gameObject, base.transform).GetComponent<Image>();
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

		// Token: 0x06000C98 RID: 3224 RVA: 0x0005239B File Offset: 0x0005059B
		private void OnDestroy()
		{
			if (VirtualCursors.instance == this)
			{
				VirtualCursors.instance = null;
			}
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x000523B0 File Offset: 0x000505B0
		private void Update()
		{
			Vector2 vector;
			vector..ctor((float)Screen.width, (float)Screen.height);
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
							unityAction.Invoke();
						}
					}
					else if (Controls.MenuDirectionalAny_PressedGet(num) || (flag && (flag5 || flag4)) || flag6)
					{
						this.cursorImagesVisibleState[i] = false;
						UnityAction unityAction2 = this.onSmartVisibilitySwitch_Off;
						if (unityAction2 != null)
						{
							unityAction2.Invoke();
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

		public static VirtualCursors instance = null;

		private const bool HIDE_SYSTEM_MOUSE = true;

		private const bool JOYSTICK_CURSOR_RIGHT_STICK_DEFAULT = false;

		private const bool JOYSTICK_CURSOR_LEFT_STICK_DEFAULT = false;

		private const float JOYSTICK_CURSOR_SPEED = 1440f;

		public const bool SMART_VISIBILITY_FEATURE_CAN_BE_TOGGLED = true;

		public const bool SMART_VISIBILITY_INITIAL_FEATURE_STATE = true;

		public const bool RESET_CURSOR_POSITION_WHEN_INVISIBLE = false;

		public const bool RESET_SPRITE_ANIMATION_WHEN_INVISIBLE = false;

		public const float SENSITIVITY_MULT = 4f;

		public const float SENSITIVITY_MULT_GAMEPAD = 0.5f;

		public const float SCALE_MULT = 4f;

		public Vector2 referenceResolution = new Vector2(1920f, 1080f);

		private List<Image> cursorImages = new List<Image>();

		private List<bool> cursorImagesVisibleState = new List<bool>();

		private List<FrameAnimator> cursorImageAnimators = new List<FrameAnimator>();

		public Image cursorImageTemplate;

		private bool[] cursorLeftClickState;

		private bool[] cursorLeftClickStateOld;

		private bool[] cursorHasMoved;

		private bool[] cursorHasMovedOld;

		private bool[] joystickRightStickCanMoveCursor;

		private bool[] joystickLeftStickCanMoveCursor;

		private bool[] cursordDesiredVisibility;

		private bool[] cursorDesiredVisibility_KindlyAskToHidePrettyPlease;

		private static bool cursorSmartVisibility_EnableState = true;

		private float[] scale;

		private float[] scaleTarget;

		public UnityAction onSmartVisibilitySwitch_On;

		public UnityAction onSmartVisibilitySwitch_Off;
	}
}
