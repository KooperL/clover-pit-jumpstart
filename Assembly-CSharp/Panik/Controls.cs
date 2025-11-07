using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

namespace Panik
{
	public class Controls : MonoBehaviour
	{
		// Token: 0x06000B68 RID: 2920 RVA: 0x0004BFB8 File Offset: 0x0004A1B8
		public static bool MouseMovementSwitchesLastInputGet(int playerIndex)
		{
			if (Controls.mouseMovementSwitchesLastInput == null)
			{
				Controls.mouseMovementSwitchesLastInput = new bool[1];
				for (int i = 0; i < 1; i++)
				{
					Controls.mouseMovementSwitchesLastInput[i] = true;
				}
			}
			return Controls.mouseMovementSwitchesLastInput[playerIndex];
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0004BFF4 File Offset: 0x0004A1F4
		private void PlayersInit()
		{
			this._PlayersListUpdate();
			foreach (Controls.PlayerExt playerExt in Controls.playersExtList)
			{
				if (playerExt == null)
				{
					Debug.LogError("Controls: Player is null!");
				}
				else if (PlatformMaster.PlatformIsComputer())
				{
					playerExt.lastInputKindUsed = this.PLAYER_DESKTOP_PREFERRED_INPUT;
					if (playerExt.rePlayer.controllers.joystickCount > 0)
					{
						playerExt.lastInputKindUsed = Controls.InputKind.Joystick;
					}
				}
				else if (PlatformMaster.PlatformIsMobile())
				{
					string text = "Controls: Mobile platform has not initial input defined!";
					Debug.LogError(text);
					ConsolePrompt.LogError(text, "", 0f);
				}
				else
				{
					playerExt.lastInputKindUsed = Controls.InputKind.Joystick;
				}
			}
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0004C0B0 File Offset: 0x0004A2B0
		private void _PlayersListUpdate()
		{
			if (ReInput.players.playerCount == 0)
			{
				Debug.LogWarning("Controls: No players found!");
				return;
			}
			foreach (Controls.PlayerExt playerExt in Controls.playersExtList)
			{
				if (!Controls.playersPool.Contains(playerExt))
				{
					Controls.playersPool.Add(playerExt);
				}
			}
			Controls.playersExtList.Clear();
			using (IEnumerator<Player> enumerator2 = ReInput.players.GetPlayers(false).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Player p = enumerator2.Current;
					Controls.PlayerExt playerExt2 = Controls.playersPool.Find((Controls.PlayerExt x) => x.rePlayer == p);
					if (playerExt2 == null)
					{
						playerExt2 = new Controls.PlayerExt();
						playerExt2.rePlayer = p;
					}
					Controls.playersExtList.Add(playerExt2);
					Controls.playersPool.Remove(playerExt2);
				}
			}
			if (Controls.playersExtList.Count > 0 && Controls.p1 == null)
			{
				Controls.p1 = Controls.playersExtList[0];
			}
			if (Controls.p1 == null)
			{
				Debug.LogError("Controls: Player 1 is null!");
				return;
			}
			if (Controls.p1 != null)
			{
				Controls.p1.isPlaying = true;
			}
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0004C20C File Offset: 0x0004A40C
		private void PlayersUpdate()
		{
			bool flag = false;
			if (ReInput.players == null)
			{
				Debug.LogWarning("Controls: No players found!");
				return;
			}
			if (ReInput.players.playerCount == 0)
			{
				Debug.LogWarning("Controls: No players found!");
				return;
			}
			if (ReInput.players.playerCount != Controls.playersExtList.Count)
			{
				this._PlayersListUpdate();
				Debug.LogWarning("Controls: Players count changed! list was updated!");
			}
			foreach (Controls.PlayerExt playerExt in Controls.playersExtList)
			{
				if (playerExt == null)
				{
					Debug.LogError("Controls: Player is null!");
				}
				else
				{
					if (playerExt.rePlayer.controllers.hasKeyboard && playerExt.rePlayer.controllers.Keyboard.GetAnyButtonDown())
					{
						playerExt.lastInputKindUsed = Controls.InputKind.Keyboard;
					}
					if (playerExt.rePlayer.controllers.hasMouse)
					{
						bool anyButtonDown = playerExt.rePlayer.controllers.Mouse.GetAnyButtonDown();
						bool flag2 = Mathf.Abs(Controls.MouseAxis_ValueGet(playerExt, Controls.MouseElement.axisX)) > 0.1f || Mathf.Abs(Controls.MouseAxis_ValueGet(playerExt, Controls.MouseElement.axisY)) > 0.1f;
						bool flag3 = Controls.MouseAxis_ValueGet(playerExt, Controls.MouseElement.axisScrollWheelHorizontal) != 0f || Controls.MouseAxis_ValueGet(playerExt, Controls.MouseElement.axisScrollWheelVertical) != 0f;
						if (anyButtonDown || flag3 || (Controls.MouseMovementSwitchesLastInputGet(Controls.GetPlayerIndex(playerExt)) && flag2))
						{
							playerExt.lastInputKindUsed = Controls.InputKind.Mouse;
						}
					}
					try
					{
						if (playerExt.rePlayer.controllers.joystickCount > 0)
						{
							for (int i = 0; i < playerExt.rePlayer.controllers.joystickCount; i++)
							{
								Joystick joystick = playerExt.rePlayer.controllers.Joysticks[i];
								if (joystick != null)
								{
									IGamepadTemplate template = joystick.GetTemplate<IGamepadTemplate>();
									bool flag4 = template.leftStick.horizontal.value != 0f || template.leftStick.vertical.value != 0f || template.rightStick.horizontal.value != 0f || template.rightStick.vertical.value != 0f || template.leftTrigger.value != 0f || template.rightTrigger.value != 0f;
									if (joystick.GetAnyButtonDown() || flag4)
									{
										playerExt.lastInputKindUsed = Controls.InputKind.Joystick;
										playerExt.lastUsedJoystickIndex = i;
										playerExt.lastJoystickUsed = joystick;
										playerExt.lastUsedJoystickTemplate = template;
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						if (!this.controllerNotSupported_Showed)
						{
							string text = "Controller not supported! If game breaks, disconnect controller + restart game!";
							this.controllerNotSupported_Showed = true;
							if (PlatformMaster.PlatformIsComputer())
							{
								ConsolePrompt.LogError(text + " /// Ctrl+Tab to close this message.", "", 0f);
							}
							Debug.LogError(text + "\n" + ex.Message);
						}
					}
					if (playerExt.lastInputKindUsed != playerExt.lastInputKindUsedOld)
					{
						bool flag5 = playerExt.lastInputKindUsed == Controls.InputKind.Mouse && playerExt.lastInputKindUsedOld == Controls.InputKind.Keyboard;
						bool flag6 = playerExt.lastInputKindUsed == Controls.InputKind.Keyboard && playerExt.lastInputKindUsedOld == Controls.InputKind.Mouse;
						if (flag5 || flag6)
						{
						}
						playerExt.lastInputKindUsedOld = playerExt.lastInputKindUsed;
						flag = true;
					}
				}
			}
			if (flag)
			{
				Controls.MapCallback mapCallback = Controls.onLastInputKindChangedAny;
				if (mapCallback != null)
				{
					mapCallback(null);
				}
				Controls.MapCallback mapCallback2 = Controls.onPromptsUpdateRequest;
				if (mapCallback2 == null)
				{
					return;
				}
				mapCallback2(null);
			}
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0004C598 File Offset: 0x0004A798
		public static int GetPlayersCount()
		{
			return Controls.playersExtList.Count;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0004C5A4 File Offset: 0x0004A7A4
		public static Controls.PlayerExt GetPlayerByIndex(int index)
		{
			if (index < 0)
			{
				Debug.LogError("Controls: Player index is < 0!");
				return null;
			}
			if (index >= Controls.playersExtList.Count)
			{
				Debug.LogError("Controls: Player index is >= players count!");
				return null;
			}
			return Controls.playersExtList[index];
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0004C5DA File Offset: 0x0004A7DA
		public static int GetPlayerIndex(Controls.PlayerExt player)
		{
			if (player == null)
			{
				Debug.LogError("Controls: Player is null!");
				return -1;
			}
			int num = Controls.playersExtList.IndexOf(player);
			if (num < 0)
			{
				Debug.LogError("Controls: Player is not in the list!");
			}
			return num;
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0004C604 File Offset: 0x0004A804
		public static bool PlayerIsUsingKeyboard(Controls.PlayerExt player)
		{
			return player.rePlayer.controllers.hasKeyboard && player.lastInputKindUsed == Controls.InputKind.Keyboard;
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0004C623 File Offset: 0x0004A823
		public static bool PlayerIsUsingKeyboard(int playerIndex)
		{
			return Controls.PlayerIsUsingKeyboard(Controls.GetPlayerByIndex(playerIndex));
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0004C630 File Offset: 0x0004A830
		public static bool PlayerIsUsingMouse(Controls.PlayerExt player)
		{
			return player.rePlayer.controllers.hasMouse && player.lastInputKindUsed == Controls.InputKind.Mouse;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x0004C64F File Offset: 0x0004A84F
		public static bool PlayerIsUsingMouse(int playerIndex)
		{
			return Controls.PlayerIsUsingMouse(Controls.GetPlayerByIndex(playerIndex));
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0004C65C File Offset: 0x0004A85C
		public static bool PlayerIsUsingJoystick(Controls.PlayerExt player)
		{
			return player.rePlayer.controllers.joystickCount > 0 && player.lastInputKindUsed == Controls.InputKind.Joystick && player.lastUsedJoystickTemplate != null;
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0004C685 File Offset: 0x0004A885
		public static bool PlayerIsUsingJoystick(int playerIndex)
		{
			return Controls.PlayerIsUsingJoystick(Controls.GetPlayerByIndex(playerIndex));
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0004C694 File Offset: 0x0004A894
		private void SplitScreenUpdate()
		{
			if (Controls.p1 != null && !Controls.p1.isPlaying)
			{
				Controls.p1.isPlaying = true;
				if (!Controls.playersPlaying.Contains(Controls.p1))
				{
					Controls.playersPlaying.Add(Controls.p1);
				}
			}
			foreach (Controls.PlayerExt playerExt in Controls.playersPlaying)
			{
				if (!Controls.playersExtList.Contains(playerExt))
				{
					Controls.playersPlaying.Remove(playerExt);
				}
				else if (!playerExt.isPlaying)
				{
					Controls.playersPlaying.Remove(playerExt);
				}
			}
			foreach (Controls.PlayerExt playerExt2 in Controls.playersExtList)
			{
				if (playerExt2.isPlaying && !Controls.playersPlaying.Contains(playerExt2))
				{
					Controls.playersPlaying.Add(playerExt2);
				}
			}
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0004C7A8 File Offset: 0x0004A9A8
		public static void PlayersJoinSession_Start()
		{
			if (Controls.playersCanJoin)
			{
				return;
			}
			Controls.playersCanJoin = true;
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0004C7B8 File Offset: 0x0004A9B8
		public static void PlayersJoinSession_End()
		{
			if (!Controls.playersCanJoin)
			{
				return;
			}
			Controls.playersCanJoin = false;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0004C7C8 File Offset: 0x0004A9C8
		public static bool PlayersJoinSession_IsRunning()
		{
			return Controls.playersCanJoin;
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0004C7D0 File Offset: 0x0004A9D0
		public static bool PlayerJoinTry(Controls.PlayerExt player)
		{
			if (!Controls.playersCanJoin)
			{
				Debug.LogError("Controls: Players can't join session!");
				return false;
			}
			if (player.isPlaying)
			{
				Debug.LogError("Controls: Player is already playing!");
				return false;
			}
			if (Controls.PlayingPlayersCount() >= Controls.maxPlayingPlayers)
			{
				return false;
			}
			if (Controls.playersPlaying.Contains(player) || player.isPlaying)
			{
				return false;
			}
			player.isPlaying = true;
			if (!Controls.playersPlaying.Contains(player))
			{
				Controls.playersPlaying.Add(player);
			}
			return true;
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0004C849 File Offset: 0x0004AA49
		public static bool PlayerJoinTry(int playerIndex)
		{
			return Controls.PlayerJoinTry(Controls.GetPlayerByIndex(playerIndex));
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0004C858 File Offset: 0x0004AA58
		public static bool PlayerLeaveTry(Controls.PlayerExt player)
		{
			if (!player.isPlaying || !Controls.playersPlaying.Contains(player))
			{
				return false;
			}
			if (player == Controls.p1)
			{
				return false;
			}
			player.isPlaying = false;
			if (Controls.playersPlaying.Contains(player))
			{
				Controls.playersPlaying.Remove(player);
			}
			return true;
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0004C8A7 File Offset: 0x0004AAA7
		public static bool PlayerLeaveTry(int playerIndex)
		{
			return Controls.PlayerLeaveTry(Controls.GetPlayerByIndex(playerIndex));
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x0004C8B4 File Offset: 0x0004AAB4
		public static bool PlayerIsPlaying(Controls.PlayerExt player)
		{
			return player.isPlaying && Controls.playersPlaying.Contains(player);
		}

		// Token: 0x06000B7E RID: 2942 RVA: 0x0004C8CB File Offset: 0x0004AACB
		public static bool PlayerIsPlaying(int playerIndex)
		{
			return Controls.PlayerIsPlaying(Controls.GetPlayerByIndex(playerIndex));
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x0004C8D8 File Offset: 0x0004AAD8
		public static int PlayingPlayersCount()
		{
			return Controls.playersPlaying.Count;
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0004C8E4 File Offset: 0x0004AAE4
		public static List<Controls.PlayerExt> PlayingPlayersGetList()
		{
			return Controls.playersPlaying;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0004C8EC File Offset: 0x0004AAEC
		private Controls.VibrationData VibrationDataGet(Controls.PlayerExt playerExt)
		{
			if (this.vibrations.ContainsKey(playerExt))
			{
				return this.vibrations[playerExt];
			}
			Controls.VibrationData vibrationData = new Controls.VibrationData(playerExt);
			this.vibrations.Add(playerExt, vibrationData);
			return vibrationData;
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0004C92C File Offset: 0x0004AB2C
		private void VibrationUpdate()
		{
			foreach (Controls.PlayerExt playerExt in Controls.playersExtList)
			{
				int playerIndex = Controls.GetPlayerIndex(playerExt);
				if (playerExt.rePlayer.controllers.joystickCount != 0 && playerExt.lastInputKindUsed == Controls.InputKind.Joystick && playerExt.lastUsedJoystickIndex >= 0)
				{
					Controls.VibrationData vibrationData = this.VibrationDataGet(playerExt);
					if (!vibrationData.pausable || !Tick.Paused)
					{
						vibrationData.motorLevelLeft = Mathf.MoveTowards(vibrationData.motorLevelLeft, 0f, vibrationData.decaySpeedLeft * vibrationData.decaySpeedMult * Tick.Time);
						vibrationData.motorLevelRight = Mathf.MoveTowards(vibrationData.motorLevelRight, 0f, vibrationData.decaySpeedRight * vibrationData.decaySpeedMult * Tick.Time);
						for (int i = 0; i < playerExt.rePlayer.controllers.joystickCount; i++)
						{
							Joystick joystick = playerExt.rePlayer.controllers.Joysticks[i];
							if (joystick != null)
							{
								if (i == playerExt.lastUsedJoystickIndex && Data.settings.JoystickVibrationEnabledGet(playerIndex))
								{
									joystick.SetVibration(vibrationData.motorLevelLeft, vibrationData.motorLevelRight);
								}
								else
								{
									joystick.StopVibration();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0004CA98 File Offset: 0x0004AC98
		public static void VibrationSetLeft(Controls.PlayerExt player, float force)
		{
			Controls.instance.VibrationDataGet(player).motorLevelLeft = force;
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0004CAAB File Offset: 0x0004ACAB
		public static void VibrationSetRight(Controls.PlayerExt player, float force)
		{
			Controls.instance.VibrationDataGet(player).motorLevelRight = force;
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0004CABE File Offset: 0x0004ACBE
		public static void VibrationSet(Controls.PlayerExt player, float force)
		{
			Controls.VibrationData vibrationData = Controls.instance.VibrationDataGet(player);
			vibrationData.motorLevelLeft = force;
			vibrationData.motorLevelRight = force;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0004CAD8 File Offset: 0x0004ACD8
		public static void VibrationSet_PreferMax(Controls.PlayerExt player, float vibration)
		{
			Controls.VibrationSet(player, Mathf.Max(new float[]
			{
				Controls.VibrationGet(player),
				Controls.VibrationGet(player),
				vibration
			}));
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0004CB01 File Offset: 0x0004AD01
		public static float VibrationGetLeft(Controls.PlayerExt player)
		{
			return Controls.instance.VibrationDataGet(player).motorLevelLeft;
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0004CB13 File Offset: 0x0004AD13
		public static float VibrationGetRight(Controls.PlayerExt player)
		{
			return Controls.instance.VibrationDataGet(player).motorLevelRight;
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0004CB28 File Offset: 0x0004AD28
		public static float VibrationGet(Controls.PlayerExt player)
		{
			Controls.VibrationData vibrationData = Controls.instance.VibrationDataGet(player);
			return Mathf.Max(vibrationData.motorLevelLeft, vibrationData.motorLevelRight);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0004CB52 File Offset: 0x0004AD52
		public static void VibrationSetDecaySpeedLeft(Controls.PlayerExt player, float speed)
		{
			Controls.instance.VibrationDataGet(player).decaySpeedLeft = speed;
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0004CB65 File Offset: 0x0004AD65
		public static void VibrationSetDecaySpeedRight(Controls.PlayerExt player, float speed)
		{
			Controls.instance.VibrationDataGet(player).decaySpeedRight = speed;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0004CB78 File Offset: 0x0004AD78
		public static void VibrationSetDecaySpeed(Controls.PlayerExt player, float speed)
		{
			Controls.VibrationData vibrationData = Controls.instance.VibrationDataGet(player);
			vibrationData.decaySpeedLeft = speed;
			vibrationData.decaySpeedRight = speed;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0004CB92 File Offset: 0x0004AD92
		public static float VibrationGetDecaySpeedLeft(Controls.PlayerExt player)
		{
			return Controls.instance.VibrationDataGet(player).decaySpeedLeft;
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0004CBA4 File Offset: 0x0004ADA4
		public static float VibrationGetDecaySpeedRight(Controls.PlayerExt player)
		{
			return Controls.instance.VibrationDataGet(player).decaySpeedRight;
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0004CBB8 File Offset: 0x0004ADB8
		public static float VibrationGetDecaySpeed(Controls.PlayerExt player)
		{
			Controls.VibrationData vibrationData = Controls.instance.VibrationDataGet(player);
			return Mathf.Max(vibrationData.decaySpeedLeft, vibrationData.decaySpeedRight);
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0004CBE2 File Offset: 0x0004ADE2
		public static void VibrationSetDecaySpeedMult(Controls.PlayerExt player, float mult)
		{
			Controls.instance.VibrationDataGet(player).decaySpeedMult = mult;
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0004CBF5 File Offset: 0x0004ADF5
		public static float VibrationGetDecaySpeedMult(Controls.PlayerExt player)
		{
			return Controls.instance.VibrationDataGet(player).decaySpeedMult;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0004CC07 File Offset: 0x0004AE07
		public static void VibrationSetPausable(Controls.PlayerExt player, bool pausable)
		{
			Controls.instance.VibrationDataGet(player).pausable = pausable;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0004CC1A File Offset: 0x0004AE1A
		public static bool VibrationGetPausable(Controls.PlayerExt player)
		{
			return Controls.instance.VibrationDataGet(player).pausable;
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0004CC2C File Offset: 0x0004AE2C
		public static string JoystickElementName(Controls.JoystickElement element)
		{
			switch (PlatformMaster.PlatformKindGet())
			{
			case PlatformMaster.PlatformKind.PC:
				return Controls._JoystickElementName_Computer(element);
			case PlatformMaster.PlatformKind.Linux:
				return Controls._JoystickElementName_Computer(element);
			case PlatformMaster.PlatformKind.Mac:
				return Controls._JoystickElementName_Computer(element);
			case PlatformMaster.PlatformKind.WebGL:
				return Controls._JoystickElementName_Computer(element);
			case PlatformMaster.PlatformKind.Android:
				return Controls._JoystickElementName_Mobile(element);
			case PlatformMaster.PlatformKind.iOS:
				return Controls._JoystickElementName_Mobile(element);
			case PlatformMaster.PlatformKind.PS4:
				return Controls._JoystickElementName_Playstation(element);
			case PlatformMaster.PlatformKind.PS5:
				return Controls._JoystickElementName_Playstation(element);
			case PlatformMaster.PlatformKind.XboxOne:
				return Controls._JoystickElementName_Xbox(element);
			case PlatformMaster.PlatformKind.XboxSeries:
				return Controls._JoystickElementName_Xbox(element);
			case PlatformMaster.PlatformKind.NintendoSwitch:
				return Controls._JoystickElementName_NintendoSwitch(element);
			default:
				return "undef joy name";
			}
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x0004CCC8 File Offset: 0x0004AEC8
		private static string _JoystickElementName_Computer(Controls.JoystickElement element)
		{
			switch (element)
			{
			case Controls.JoystickElement.ButtonDown:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_A");
			case Controls.JoystickElement.ButtonRight:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_B");
			case Controls.JoystickElement.ButtonLeft:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_X");
			case Controls.JoystickElement.ButtonUp:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_Y");
			case Controls.JoystickElement.Start:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_START");
			case Controls.JoystickElement.Select:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_SELECT");
			case Controls.JoystickElement.Home:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_HOME");
			case Controls.JoystickElement.LeftStickButton:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_STICK_BUTTON");
			case Controls.JoystickElement.RightStickButton:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_STICK_BUTTON");
			case Controls.JoystickElement.LeftShoulder:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_SHOULDER");
			case Controls.JoystickElement.RightShoulder:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_SHOULDER");
			case Controls.JoystickElement.DPadUp:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_UP");
			case Controls.JoystickElement.DPadDown:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_DOWN");
			case Controls.JoystickElement.DPadLeft:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT");
			case Controls.JoystickElement.DPadRight:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT");
			case Controls.JoystickElement.LeftStickX:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_STICK_X");
			case Controls.JoystickElement.LeftStickY:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_STICK_Y");
			case Controls.JoystickElement.RightStickX:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_STICK_X");
			case Controls.JoystickElement.RightStickY:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_STICK_Y");
			case Controls.JoystickElement.LeftTrigger:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_TRIGGER");
			case Controls.JoystickElement.RightTrigger:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_TRIGGER");
			default:
				return "Unknown";
			}
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0004CE20 File Offset: 0x0004B020
		private static string _JoystickElementName_Xbox(Controls.JoystickElement element)
		{
			switch (element)
			{
			case Controls.JoystickElement.ButtonDown:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_A");
			case Controls.JoystickElement.ButtonRight:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_B");
			case Controls.JoystickElement.ButtonLeft:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_X");
			case Controls.JoystickElement.ButtonUp:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_Y");
			case Controls.JoystickElement.Start:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_START");
			case Controls.JoystickElement.Select:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_SELECT");
			case Controls.JoystickElement.Home:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_HOME");
			case Controls.JoystickElement.LeftStickButton:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_STICK_BUTTON");
			case Controls.JoystickElement.RightStickButton:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_STICK_BUTTON");
			case Controls.JoystickElement.LeftShoulder:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_SHOULDER");
			case Controls.JoystickElement.RightShoulder:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_SHOULDER");
			case Controls.JoystickElement.DPadUp:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_UP");
			case Controls.JoystickElement.DPadDown:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_DOWN");
			case Controls.JoystickElement.DPadLeft:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT");
			case Controls.JoystickElement.DPadRight:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT");
			case Controls.JoystickElement.LeftStickX:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_STICK_X");
			case Controls.JoystickElement.LeftStickY:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_STICK_Y");
			case Controls.JoystickElement.RightStickX:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_STICK_X");
			case Controls.JoystickElement.RightStickY:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_STICK_Y");
			case Controls.JoystickElement.LeftTrigger:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_TRIGGER");
			case Controls.JoystickElement.RightTrigger:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_TRIGGER");
			default:
				return "Unknown";
			}
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0004CF78 File Offset: 0x0004B178
		private static string _JoystickElementName_Playstation(Controls.JoystickElement element)
		{
			switch (element)
			{
			case Controls.JoystickElement.ButtonDown:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_CROSS");
			case Controls.JoystickElement.ButtonRight:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_CIRCLE");
			case Controls.JoystickElement.ButtonLeft:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_SQUARE");
			case Controls.JoystickElement.ButtonUp:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_TRIANGLE");
			case Controls.JoystickElement.Start:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_OPTIONS");
			case Controls.JoystickElement.Select:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_SHARE");
			case Controls.JoystickElement.Home:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_PS");
			case Controls.JoystickElement.LeftStickButton:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_LEFT_STICK_BUTTON");
			case Controls.JoystickElement.RightStickButton:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_RIGHT_STICK_BUTTON");
			case Controls.JoystickElement.LeftShoulder:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_L1");
			case Controls.JoystickElement.RightShoulder:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_R1");
			case Controls.JoystickElement.DPadUp:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_UP");
			case Controls.JoystickElement.DPadDown:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_DOWN");
			case Controls.JoystickElement.DPadLeft:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_LEFT");
			case Controls.JoystickElement.DPadRight:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_RIGHT");
			case Controls.JoystickElement.LeftStickX:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_LEFT_STICK_X");
			case Controls.JoystickElement.LeftStickY:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_LEFT_STICK_Y");
			case Controls.JoystickElement.RightStickX:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_RIGHT_STICK_X");
			case Controls.JoystickElement.RightStickY:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_RIGHT_STICK_Y");
			case Controls.JoystickElement.LeftTrigger:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_L2");
			case Controls.JoystickElement.RightTrigger:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_PS_R2");
			default:
				return "Unknown";
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0004D0D0 File Offset: 0x0004B2D0
		private static string _JoystickElementName_NintendoSwitch(Controls.JoystickElement element)
		{
			switch (element)
			{
			case Controls.JoystickElement.ButtonDown:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_B");
			case Controls.JoystickElement.ButtonRight:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_A");
			case Controls.JoystickElement.ButtonLeft:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_Y");
			case Controls.JoystickElement.ButtonUp:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_X");
			case Controls.JoystickElement.Start:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_PLUS");
			case Controls.JoystickElement.Select:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_MINUS");
			case Controls.JoystickElement.Home:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_HOME");
			case Controls.JoystickElement.LeftStickButton:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_LEFT_STICK_BUTTON");
			case Controls.JoystickElement.RightStickButton:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_RIGHT_STICK_BUTTON");
			case Controls.JoystickElement.LeftShoulder:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_L");
			case Controls.JoystickElement.RightShoulder:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_R");
			case Controls.JoystickElement.DPadUp:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_UP");
			case Controls.JoystickElement.DPadDown:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_DOWN");
			case Controls.JoystickElement.DPadLeft:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_LEFT");
			case Controls.JoystickElement.DPadRight:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_RIGHT");
			case Controls.JoystickElement.LeftStickX:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_LEFT_STICK_X");
			case Controls.JoystickElement.LeftStickY:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_LEFT_STICK_Y");
			case Controls.JoystickElement.RightStickX:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_RIGHT_STICK_X");
			case Controls.JoystickElement.RightStickY:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_RIGHT_STICK_Y");
			case Controls.JoystickElement.LeftTrigger:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_ZL");
			case Controls.JoystickElement.RightTrigger:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_NINTENDO_ZR");
			default:
				return "Unknown";
			}
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0004D228 File Offset: 0x0004B428
		private static string _JoystickElementName_Mobile(Controls.JoystickElement element)
		{
			switch (element)
			{
			case Controls.JoystickElement.ButtonDown:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_A");
			case Controls.JoystickElement.ButtonRight:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_B");
			case Controls.JoystickElement.ButtonLeft:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_X");
			case Controls.JoystickElement.ButtonUp:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_Y");
			case Controls.JoystickElement.Start:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_START");
			case Controls.JoystickElement.Select:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_SELECT");
			case Controls.JoystickElement.Home:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_HOME");
			case Controls.JoystickElement.LeftStickButton:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_STICK_BUTTON");
			case Controls.JoystickElement.RightStickButton:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_STICK_BUTTON");
			case Controls.JoystickElement.LeftShoulder:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_SHOULDER");
			case Controls.JoystickElement.RightShoulder:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_SHOULDER");
			case Controls.JoystickElement.DPadUp:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_UP");
			case Controls.JoystickElement.DPadDown:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_DOWN");
			case Controls.JoystickElement.DPadLeft:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT");
			case Controls.JoystickElement.DPadRight:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT");
			case Controls.JoystickElement.LeftStickX:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_STICK_X");
			case Controls.JoystickElement.LeftStickY:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_STICK_Y");
			case Controls.JoystickElement.RightStickX:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_STICK_X");
			case Controls.JoystickElement.RightStickY:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_STICK_Y");
			case Controls.JoystickElement.LeftTrigger:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_LEFT_TRIGGER");
			case Controls.JoystickElement.RightTrigger:
				return Translation.Get("JOYSTICK_ELEMENT_NAME_XBOX_RIGHT_TRIGGER");
			default:
				return "Unknown";
			}
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0004D380 File Offset: 0x0004B580
		private static IControllerTemplateButton _JoystickElementToTemplateButton(Controls.PlayerExt player, Controls.JoystickElement element)
		{
			switch (element)
			{
			case Controls.JoystickElement.ButtonDown:
				return player.lastUsedJoystickTemplate.actionBottomRow1;
			case Controls.JoystickElement.ButtonRight:
				return player.lastUsedJoystickTemplate.actionBottomRow2;
			case Controls.JoystickElement.ButtonLeft:
				return player.lastUsedJoystickTemplate.actionTopRow1;
			case Controls.JoystickElement.ButtonUp:
				return player.lastUsedJoystickTemplate.actionTopRow2;
			case Controls.JoystickElement.Start:
				return player.lastUsedJoystickTemplate.center2;
			case Controls.JoystickElement.Select:
				return player.lastUsedJoystickTemplate.center1;
			case Controls.JoystickElement.Home:
				return player.lastUsedJoystickTemplate.center3;
			case Controls.JoystickElement.LeftStickButton:
				return player.lastUsedJoystickTemplate.leftStick.press;
			case Controls.JoystickElement.RightStickButton:
				return player.lastUsedJoystickTemplate.rightStick.press;
			case Controls.JoystickElement.LeftShoulder:
				return player.lastUsedJoystickTemplate.leftShoulder1;
			case Controls.JoystickElement.RightShoulder:
				return player.lastUsedJoystickTemplate.rightShoulder1;
			case Controls.JoystickElement.DPadUp:
				return player.lastUsedJoystickTemplate.dPad.up;
			case Controls.JoystickElement.DPadDown:
				return player.lastUsedJoystickTemplate.dPad.down;
			case Controls.JoystickElement.DPadLeft:
				return player.lastUsedJoystickTemplate.dPad.left;
			case Controls.JoystickElement.DPadRight:
				return player.lastUsedJoystickTemplate.dPad.right;
			default:
				return null;
			}
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0004D4A8 File Offset: 0x0004B6A8
		private static IControllerTemplateAxis _JoystickElementToTemplateAxis(Controls.PlayerExt player, Controls.JoystickElement element)
		{
			switch (element)
			{
			case Controls.JoystickElement.LeftStickX:
				return player.lastUsedJoystickTemplate.leftStick.horizontal;
			case Controls.JoystickElement.LeftStickY:
				return player.lastUsedJoystickTemplate.leftStick.vertical;
			case Controls.JoystickElement.RightStickX:
				return player.lastUsedJoystickTemplate.rightStick.horizontal;
			case Controls.JoystickElement.RightStickY:
				return player.lastUsedJoystickTemplate.rightStick.vertical;
			case Controls.JoystickElement.LeftTrigger:
				return player.lastUsedJoystickTemplate.leftTrigger;
			case Controls.JoystickElement.RightTrigger:
				return player.lastUsedJoystickTemplate.rightTrigger;
			default:
				return null;
			}
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0004D538 File Offset: 0x0004B738
		public static bool JoystickElement_IsButton(Controls.JoystickElement element)
		{
			switch (element)
			{
			case Controls.JoystickElement.ButtonDown:
				return true;
			case Controls.JoystickElement.ButtonRight:
				return true;
			case Controls.JoystickElement.ButtonLeft:
				return true;
			case Controls.JoystickElement.ButtonUp:
				return true;
			case Controls.JoystickElement.Start:
				return true;
			case Controls.JoystickElement.Select:
				return true;
			case Controls.JoystickElement.Home:
				return true;
			case Controls.JoystickElement.LeftStickButton:
				return true;
			case Controls.JoystickElement.RightStickButton:
				return true;
			case Controls.JoystickElement.LeftShoulder:
				return true;
			case Controls.JoystickElement.RightShoulder:
				return true;
			case Controls.JoystickElement.DPadUp:
				return true;
			case Controls.JoystickElement.DPadDown:
				return true;
			case Controls.JoystickElement.DPadLeft:
				return true;
			case Controls.JoystickElement.DPadRight:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0004D5A8 File Offset: 0x0004B7A8
		public static bool JoystickElement_IsAxis(Controls.JoystickElement element)
		{
			switch (element)
			{
			case Controls.JoystickElement.LeftStickX:
				return true;
			case Controls.JoystickElement.LeftStickY:
				return true;
			case Controls.JoystickElement.RightStickX:
				return true;
			case Controls.JoystickElement.RightStickY:
				return true;
			case Controls.JoystickElement.LeftTrigger:
				return true;
			case Controls.JoystickElement.RightTrigger:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0004D5DC File Offset: 0x0004B7DC
		public static bool PickStickAxis_Joystick(int playerIndex, Controls.JoystickElement xElement, Controls.JoystickElement yElement, out Controls.JoystickElement pickedElement)
		{
			if (xElement == Controls.JoystickElement.Undefined || yElement == Controls.JoystickElement.Undefined)
			{
				pickedElement = Controls.JoystickElement.Undefined;
				return false;
			}
			float num = Controls.JoystickAxis_ValueGet(playerIndex, xElement);
			float num2 = Controls.JoystickAxis_ValueGet(playerIndex, yElement);
			if (new Vector2(num, num2).magnitude > 0.8f)
			{
				if (Mathf.Abs(num) > Mathf.Abs(num2))
				{
					pickedElement = xElement;
				}
				else
				{
					pickedElement = yElement;
				}
				return true;
			}
			pickedElement = Controls.JoystickElement.Undefined;
			return false;
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0004D63C File Offset: 0x0004B83C
		public static bool PickStickAxis_Joystick(Controls.PlayerExt player, Controls.JoystickElement xElement, Controls.JoystickElement yElement, out Controls.JoystickElement pickedElement)
		{
			return Controls.PickStickAxis_Joystick(Controls.GetPlayerIndex(player), xElement, yElement, out pickedElement);
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0004D64C File Offset: 0x0004B84C
		public static bool JoystickButton_PressedGet(Controls.PlayerExt player, Controls.JoystickElement element)
		{
			if (player == null)
			{
				Debug.LogError("JoystickButton_Pressed(): Player is null!");
				return false;
			}
			if (player.lastUsedJoystickTemplate == null)
			{
				return false;
			}
			IControllerTemplateButton controllerTemplateButton = Controls._JoystickElementToTemplateButton(player, element);
			return controllerTemplateButton != null && controllerTemplateButton.justPressed;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0004D685 File Offset: 0x0004B885
		public static bool JoystickButton_PressedGet(int playerIndex, Controls.JoystickElement element)
		{
			return Controls.JoystickButton_PressedGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0004D694 File Offset: 0x0004B894
		public static bool JoystickButton_HoldGet(Controls.PlayerExt player, Controls.JoystickElement element)
		{
			if (player == null)
			{
				Debug.LogError("JoystickButton_Hold(): Player is null!");
				return false;
			}
			if (player.lastUsedJoystickTemplate == null)
			{
				return false;
			}
			IControllerTemplateButton controllerTemplateButton = Controls._JoystickElementToTemplateButton(player, element);
			return controllerTemplateButton != null && controllerTemplateButton.value;
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0004D6CD File Offset: 0x0004B8CD
		public static bool JoystickButton_HoldGet(int playerIndex, Controls.JoystickElement element)
		{
			return Controls.JoystickButton_HoldGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0004D6DC File Offset: 0x0004B8DC
		public static bool JoystickButton_ReleasedGet(Controls.PlayerExt player, Controls.JoystickElement element)
		{
			if (player == null)
			{
				Debug.LogError("JoystickButton_Released(): Player is null!");
				return false;
			}
			if (player.lastUsedJoystickTemplate == null)
			{
				return false;
			}
			IControllerTemplateButton controllerTemplateButton = Controls._JoystickElementToTemplateButton(player, element);
			return controllerTemplateButton != null && controllerTemplateButton.justReleased;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0004D715 File Offset: 0x0004B915
		public static bool JoystickButton_ReleasedGet(int playerIndex, Controls.JoystickElement element)
		{
			return Controls.JoystickButton_ReleasedGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0004D724 File Offset: 0x0004B924
		public static float JoystickAxis_ValueGet(Controls.PlayerExt player, Controls.JoystickElement element)
		{
			if (player == null)
			{
				Debug.LogError("JoystickAxis_Hold(): Player is null!");
				return 0f;
			}
			if (player.lastUsedJoystickTemplate == null)
			{
				return 0f;
			}
			IControllerTemplateAxis controllerTemplateAxis = Controls._JoystickElementToTemplateAxis(player, element);
			if (controllerTemplateAxis == null)
			{
				return 0f;
			}
			return controllerTemplateAxis.value;
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0004D769 File Offset: 0x0004B969
		public static float JoystickAxis_ValueGet(int playerIndex, Controls.JoystickElement element)
		{
			return Controls.JoystickAxis_ValueGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0004D778 File Offset: 0x0004B978
		public static float JoystickAxis_ValuePreviousGet(Controls.PlayerExt player, Controls.JoystickElement element)
		{
			if (player == null)
			{
				Debug.LogError("JoystickAxis_Hold(): Player is null!");
				return 0f;
			}
			if (player.lastUsedJoystickTemplate == null)
			{
				return 0f;
			}
			IControllerTemplateAxis controllerTemplateAxis = Controls._JoystickElementToTemplateAxis(player, element);
			if (controllerTemplateAxis == null)
			{
				return 0f;
			}
			return controllerTemplateAxis.valuePrev;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0004D7BD File Offset: 0x0004B9BD
		public static float JoystickAxis_ValuePreviousGet(int playerIndex, Controls.JoystickElement element)
		{
			return Controls.JoystickAxis_ValuePreviousGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0004D7CB File Offset: 0x0004B9CB
		public static Controls.JoystickElement JoystickSelectionButton_GetByPlatform()
		{
			if (PlatformMaster.PlatformKindGet() != PlatformMaster.PlatformKind.NintendoSwitch)
			{
				return Controls.JoystickElement.ButtonDown;
			}
			return Controls.JoystickElement.ButtonRight;
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0004D7D9 File Offset: 0x0004B9D9
		public static Controls.JoystickElement JoystickBackButton_GetByPlatform()
		{
			if (PlatformMaster.PlatformKindGet() != PlatformMaster.PlatformKind.NintendoSwitch)
			{
				return Controls.JoystickElement.ButtonRight;
			}
			return Controls.JoystickElement.ButtonDown;
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x0004D7E8 File Offset: 0x0004B9E8
		public static string MouseElementName(Controls.MouseElement element)
		{
			switch (element)
			{
			case Controls.MouseElement.LeftButton:
				return Translation.Get("MOUSE_ELEMENT_NAME_BUTTON_LEFT");
			case Controls.MouseElement.RightButton:
				return Translation.Get("MOUSE_ELEMENT_NAME_BUTTON_RIGHT");
			case Controls.MouseElement.MiddleButton:
				return Translation.Get("MOUSE_ELEMENT_NAME_BUTTON_MIDDLE");
			case Controls.MouseElement.axisScrollWheelVertical:
				return Translation.Get("MOUSE_ELEMENT_NAME_AXIS_WHEEL_VERTICAL");
			case Controls.MouseElement.axisScrollWheelHorizontal:
				return Translation.Get("MOUSE_ELEMENT_NAME_AXIS_WHEEL_HORIZONTAL");
			case Controls.MouseElement.axisX:
				return Translation.Get("MOUSE_ELEMENT_NAME_AXIS_X");
			case Controls.MouseElement.axisY:
				return Translation.Get("MOUSE_ELEMENT_NAME_AXIS_Y");
			default:
				return "Unknown";
			}
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0004D86B File Offset: 0x0004BA6B
		public static bool MouseElement_IsButton(Controls.MouseElement element)
		{
			switch (element)
			{
			case Controls.MouseElement.LeftButton:
				return true;
			case Controls.MouseElement.RightButton:
				return true;
			case Controls.MouseElement.MiddleButton:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0004D888 File Offset: 0x0004BA88
		public static bool MouseElement_IsAxis(Controls.MouseElement element)
		{
			switch (element)
			{
			case Controls.MouseElement.axisScrollWheelVertical:
				return true;
			case Controls.MouseElement.axisScrollWheelHorizontal:
				return true;
			case Controls.MouseElement.axisX:
				return true;
			case Controls.MouseElement.axisY:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x0004D8B0 File Offset: 0x0004BAB0
		public static bool PickStickAxis_Mouse(int playerIndex, Controls.MouseElement xElement, Controls.MouseElement yElement, out Controls.MouseElement pickedElement)
		{
			if (xElement == Controls.MouseElement.Undefined || yElement == Controls.MouseElement.Undefined)
			{
				pickedElement = Controls.MouseElement.Undefined;
				return false;
			}
			float num = Controls.MouseAxis_ValueGet(playerIndex, xElement);
			float num2 = Controls.MouseAxis_ValueGet(playerIndex, yElement);
			if (new Vector2(num, num2).magnitude > 0.8f)
			{
				if (Mathf.Abs(num) > Mathf.Abs(num2))
				{
					pickedElement = xElement;
				}
				else
				{
					pickedElement = yElement;
				}
				return true;
			}
			pickedElement = Controls.MouseElement.Undefined;
			return false;
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0004D90C File Offset: 0x0004BB0C
		public static bool PickStickAxis_Mouse(Controls.PlayerExt player, Controls.MouseElement xElement, Controls.MouseElement yElement, out Controls.MouseElement pickedElement)
		{
			return Controls.PickStickAxis_Mouse(Controls.GetPlayerIndex(player), xElement, yElement, out pickedElement);
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0004D91C File Offset: 0x0004BB1C
		public static bool MouseButton_PressedGet(Controls.PlayerExt player, Controls.MouseElement element)
		{
			if (!player.rePlayer.controllers.hasMouse)
			{
				return false;
			}
			switch (element)
			{
			case Controls.MouseElement.LeftButton:
				return player.rePlayer.controllers.Mouse.GetButtonDown(0);
			case Controls.MouseElement.RightButton:
				return player.rePlayer.controllers.Mouse.GetButtonDown(1);
			case Controls.MouseElement.MiddleButton:
				return player.rePlayer.controllers.Mouse.GetButtonDown(2);
			case Controls.MouseElement.axisScrollWheelVertical:
			{
				float axis = player.rePlayer.controllers.Mouse.GetAxis(2);
				return Mathf.Abs(axis) > 0f && Mathf.Abs(axis) <= 0f;
			}
			case Controls.MouseElement.axisScrollWheelHorizontal:
			{
				float num = player.rePlayer.controllers.Mouse.GetAxis(3);
				num *= -1f;
				return Mathf.Abs(num) > 0f && Mathf.Abs(num) <= 0f;
			}
			case Controls.MouseElement.axisX:
			{
				float axis2 = player.rePlayer.controllers.Mouse.GetAxis(0);
				return Mathf.Abs(axis2) > 0f && Mathf.Abs(axis2) <= 0f;
			}
			case Controls.MouseElement.axisY:
			{
				float axis3 = player.rePlayer.controllers.Mouse.GetAxis(1);
				return Mathf.Abs(axis3) > 0f && Mathf.Abs(axis3) <= 0f;
			}
			default:
				return false;
			}
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0004DA8E File Offset: 0x0004BC8E
		public static bool MouseButton_PressedGet(int playerIndex, Controls.MouseElement element)
		{
			return Controls.MouseButton_PressedGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0004DA9C File Offset: 0x0004BC9C
		public static bool MouseButton_ReleasedGet(Controls.PlayerExt player, Controls.MouseElement element)
		{
			if (!player.rePlayer.controllers.hasMouse)
			{
				return false;
			}
			switch (element)
			{
			case Controls.MouseElement.LeftButton:
				return player.rePlayer.controllers.Mouse.GetButtonUp(0);
			case Controls.MouseElement.RightButton:
				return player.rePlayer.controllers.Mouse.GetButtonUp(1);
			case Controls.MouseElement.MiddleButton:
				return player.rePlayer.controllers.Mouse.GetButtonUp(2);
			case Controls.MouseElement.axisScrollWheelVertical:
			{
				float axis = player.rePlayer.controllers.Mouse.GetAxis(2);
				return Mathf.Abs(axis) < 0f && Mathf.Abs(axis) >= 0f;
			}
			case Controls.MouseElement.axisScrollWheelHorizontal:
			{
				float num = player.rePlayer.controllers.Mouse.GetAxis(3);
				num *= -1f;
				return Mathf.Abs(num) < 0f && Mathf.Abs(num) >= 0f;
			}
			case Controls.MouseElement.axisX:
			{
				float axis2 = player.rePlayer.controllers.Mouse.GetAxis(0);
				return Mathf.Abs(axis2) < 0f && Mathf.Abs(axis2) >= 0f;
			}
			case Controls.MouseElement.axisY:
			{
				float axis3 = player.rePlayer.controllers.Mouse.GetAxis(1);
				return Mathf.Abs(axis3) < 0f && Mathf.Abs(axis3) >= 0f;
			}
			default:
				return false;
			}
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0004DC0E File Offset: 0x0004BE0E
		public static bool MouseButton_ReleasedGet(int playerIndex, Controls.MouseElement element)
		{
			return Controls.MouseButton_ReleasedGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0004DC1C File Offset: 0x0004BE1C
		public static bool MouseButton_HoldGet(Controls.PlayerExt player, Controls.MouseElement element)
		{
			if (!player.rePlayer.controllers.hasMouse)
			{
				return false;
			}
			switch (element)
			{
			case Controls.MouseElement.LeftButton:
				return player.rePlayer.controllers.Mouse.GetButton(0);
			case Controls.MouseElement.RightButton:
				return player.rePlayer.controllers.Mouse.GetButton(1);
			case Controls.MouseElement.MiddleButton:
				return player.rePlayer.controllers.Mouse.GetButton(2);
			case Controls.MouseElement.axisScrollWheelVertical:
				return Mathf.Abs(player.rePlayer.controllers.Mouse.GetAxis(2)) > 0f;
			case Controls.MouseElement.axisScrollWheelHorizontal:
				return Mathf.Abs(player.rePlayer.controllers.Mouse.GetAxis(3) * -1f) > 0f;
			case Controls.MouseElement.axisX:
				return Mathf.Abs(player.rePlayer.controllers.Mouse.GetAxis(0)) > 0f;
			case Controls.MouseElement.axisY:
				return Mathf.Abs(player.rePlayer.controllers.Mouse.GetAxis(1)) > 0f;
			default:
				return false;
			}
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0004DD3C File Offset: 0x0004BF3C
		public static bool MouseButton_HoldGet(int playerIndex, Controls.MouseElement element)
		{
			return Controls.MouseButton_HoldGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0004DD4C File Offset: 0x0004BF4C
		public static float MouseAxis_ValueGet(Controls.PlayerExt player, Controls.MouseElement element)
		{
			if (!player.rePlayer.controllers.hasMouse)
			{
				return 0f;
			}
			Controls.GetPlayerIndex(player);
			switch (element)
			{
			case Controls.MouseElement.LeftButton:
				return (float)(player.rePlayer.controllers.Mouse.GetButton(0) ? 1 : 0);
			case Controls.MouseElement.RightButton:
				return (float)(player.rePlayer.controllers.Mouse.GetButton(1) ? 1 : 0);
			case Controls.MouseElement.MiddleButton:
				return (float)(player.rePlayer.controllers.Mouse.GetButton(2) ? 1 : 0);
			case Controls.MouseElement.axisScrollWheelVertical:
				return player.rePlayer.controllers.Mouse.GetAxis(2);
			case Controls.MouseElement.axisScrollWheelHorizontal:
				return player.rePlayer.controllers.Mouse.GetAxis(3) * -1f;
			case Controls.MouseElement.axisX:
				return player.rePlayer.controllers.Mouse.GetAxis(0) * 0.5f;
			case Controls.MouseElement.axisY:
				return player.rePlayer.controllers.Mouse.GetAxis(1) * 0.5f;
			default:
				return 0f;
			}
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0004DE6C File Offset: 0x0004C06C
		public static float MouseAxis_ValueGet(int playerIndex, Controls.MouseElement element)
		{
			return Controls.MouseAxis_ValueGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0004DE7C File Offset: 0x0004C07C
		public static float MouseAxis_ValuePreviousGet(Controls.PlayerExt player, Controls.MouseElement element)
		{
			if (!player.rePlayer.controllers.hasMouse)
			{
				return 0f;
			}
			switch (element)
			{
			case Controls.MouseElement.LeftButton:
				return (float)(player.rePlayer.controllers.Mouse.GetButtonPrev(0) ? 1 : 0);
			case Controls.MouseElement.RightButton:
				return (float)(player.rePlayer.controllers.Mouse.GetButtonPrev(1) ? 1 : 0);
			case Controls.MouseElement.MiddleButton:
				return (float)(player.rePlayer.controllers.Mouse.GetButtonPrev(2) ? 1 : 0);
			case Controls.MouseElement.axisScrollWheelVertical:
				return player.rePlayer.controllers.Mouse.GetAxisPrev(2);
			case Controls.MouseElement.axisScrollWheelHorizontal:
				return player.rePlayer.controllers.Mouse.GetAxisPrev(3) * -1f;
			case Controls.MouseElement.axisX:
				return player.rePlayer.controllers.Mouse.GetAxisPrev(0) * 0.5f;
			case Controls.MouseElement.axisY:
				return player.rePlayer.controllers.Mouse.GetAxisPrev(1) * 0.5f;
			default:
				return 0f;
			}
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0004DF95 File Offset: 0x0004C195
		public static float MouseAxis_ValuePreviousGet(int playerIndex, Controls.MouseElement element)
		{
			return Controls.MouseAxis_ValuePreviousGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0004DFA4 File Offset: 0x0004C1A4
		public static string KeyboardElementName(Controls.KeyboardElement element)
		{
			switch (element)
			{
			case Controls.KeyboardElement.A:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_A");
			case Controls.KeyboardElement.B:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_B");
			case Controls.KeyboardElement.C:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_C");
			case Controls.KeyboardElement.D:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_D");
			case Controls.KeyboardElement.E:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_E");
			case Controls.KeyboardElement.F:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F");
			case Controls.KeyboardElement.G:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_G");
			case Controls.KeyboardElement.H:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_H");
			case Controls.KeyboardElement.I:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_I");
			case Controls.KeyboardElement.J:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_J");
			case Controls.KeyboardElement.K:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_K");
			case Controls.KeyboardElement.L:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_L");
			case Controls.KeyboardElement.M:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_M");
			case Controls.KeyboardElement.N:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_N");
			case Controls.KeyboardElement.O:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_O");
			case Controls.KeyboardElement.P:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_P");
			case Controls.KeyboardElement.Q:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_Q");
			case Controls.KeyboardElement.R:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_R");
			case Controls.KeyboardElement.S:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_S");
			case Controls.KeyboardElement.T:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_T");
			case Controls.KeyboardElement.U:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_U");
			case Controls.KeyboardElement.V:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_V");
			case Controls.KeyboardElement.W:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_W");
			case Controls.KeyboardElement.X:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_X");
			case Controls.KeyboardElement.Y:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_Y");
			case Controls.KeyboardElement.Z:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_Z");
			case Controls.KeyboardElement.Zero:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_ZERO");
			case Controls.KeyboardElement.One:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_ONE");
			case Controls.KeyboardElement.Two:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_TWO");
			case Controls.KeyboardElement.Three:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_THREE");
			case Controls.KeyboardElement.Four:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_FOUR");
			case Controls.KeyboardElement.Five:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_FIVE");
			case Controls.KeyboardElement.Six:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_SIX");
			case Controls.KeyboardElement.Seven:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_SEVEN");
			case Controls.KeyboardElement.Eight:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_EIGHT");
			case Controls.KeyboardElement.Nine:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_NINE");
			case Controls.KeyboardElement.Keypad_0:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_0");
			case Controls.KeyboardElement.Keypad_1:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_1");
			case Controls.KeyboardElement.Keypad_2:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_2");
			case Controls.KeyboardElement.Keypad_3:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_3");
			case Controls.KeyboardElement.Keypad_4:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_4");
			case Controls.KeyboardElement.Keypad_5:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_5");
			case Controls.KeyboardElement.Keypad_6:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_6");
			case Controls.KeyboardElement.Keypad_7:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_7");
			case Controls.KeyboardElement.Keypad_8:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_8");
			case Controls.KeyboardElement.Keypad_9:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_9");
			case Controls.KeyboardElement.Keypad_Dot:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_DOT");
			case Controls.KeyboardElement.Keypad_Slash:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_SLASH");
			case Controls.KeyboardElement.Keypad_Asterisk:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_ASTERISK");
			case Controls.KeyboardElement.Keypad_Minus:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_MINUS");
			case Controls.KeyboardElement.Keypad_Plus:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_PLUS");
			case Controls.KeyboardElement.Keypad_Enter:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_ENTER");
			case Controls.KeyboardElement.Keypad_Equals:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_KEYPAD_EQUALS");
			case Controls.KeyboardElement.Space:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_SPACE");
			case Controls.KeyboardElement.Backspace:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_BACKSPACE");
			case Controls.KeyboardElement.Tab:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_TAB");
			case Controls.KeyboardElement.Clear:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_CLEAR");
			case Controls.KeyboardElement.Return:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_RETURN");
			case Controls.KeyboardElement.Pause:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_PAUSE");
			case Controls.KeyboardElement.Esc:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_ESC");
			case Controls.KeyboardElement.ExclamationMark:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_EXCLAMATION_MARK");
			case Controls.KeyboardElement.DoubleQuote:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_DOUBLE_QUOTE");
			case Controls.KeyboardElement.Hash:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_HASH");
			case Controls.KeyboardElement.Dollar:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_DOLLAR");
			case Controls.KeyboardElement.Ampersand:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_AMPERSAND");
			case Controls.KeyboardElement.SingleQuote:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_SINGLE_QUOTE");
			case Controls.KeyboardElement.OpenParenthesis:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_OPEN_PARENTHESIS");
			case Controls.KeyboardElement.CloseParenthesis:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_CLOSE_PARENTHESIS");
			case Controls.KeyboardElement.Asterisk:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_ASTERISK");
			case Controls.KeyboardElement.Plus:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_PLUS");
			case Controls.KeyboardElement.Comma:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_COMMA");
			case Controls.KeyboardElement.Minus:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_MINUS");
			case Controls.KeyboardElement.Dot:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_DOT");
			case Controls.KeyboardElement.Slash:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_SLASH");
			case Controls.KeyboardElement.Colon:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_COLON");
			case Controls.KeyboardElement.Semicolon:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_SEMICOLON");
			case Controls.KeyboardElement.LessThan:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_LESS_THAN");
			case Controls.KeyboardElement.Equals:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_EQUALS");
			case Controls.KeyboardElement.GreaterThan:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_GREATER_THAN");
			case Controls.KeyboardElement.QuestionMark:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_QUESTION_MARK");
			case Controls.KeyboardElement.At:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_AT");
			case Controls.KeyboardElement.OpenBracket:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_OPEN_BRACKET");
			case Controls.KeyboardElement.Backslash:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_BACKSLASH");
			case Controls.KeyboardElement.CloseBracket:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_CLOSE_BRACKET");
			case Controls.KeyboardElement.Caret:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_CARET");
			case Controls.KeyboardElement.Underscore:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_UNDERSCORE");
			case Controls.KeyboardElement.BackQuote:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_BACK_QUOTE");
			case Controls.KeyboardElement.Delete:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_DELETE");
			case Controls.KeyboardElement.UpArrow:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_UP_ARROW");
			case Controls.KeyboardElement.DownArrow:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_DOWN_ARROW");
			case Controls.KeyboardElement.RightArrow:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_RIGHT_ARROW");
			case Controls.KeyboardElement.LeftArrow:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_LEFT_ARROW");
			case Controls.KeyboardElement.Insert:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_INSERT");
			case Controls.KeyboardElement.Home:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_HOME");
			case Controls.KeyboardElement.End:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_END");
			case Controls.KeyboardElement.PageUp:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_PAGE_UP");
			case Controls.KeyboardElement.PageDown:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_PAGE_DOWN");
			case Controls.KeyboardElement.F1:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F1");
			case Controls.KeyboardElement.F2:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F2");
			case Controls.KeyboardElement.F3:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F3");
			case Controls.KeyboardElement.F4:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F4");
			case Controls.KeyboardElement.F5:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F5");
			case Controls.KeyboardElement.F6:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F6");
			case Controls.KeyboardElement.F7:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F7");
			case Controls.KeyboardElement.F8:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F8");
			case Controls.KeyboardElement.F9:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F9");
			case Controls.KeyboardElement.F10:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F10");
			case Controls.KeyboardElement.F11:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F11");
			case Controls.KeyboardElement.F12:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F12");
			case Controls.KeyboardElement.F13:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F13");
			case Controls.KeyboardElement.F14:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F14");
			case Controls.KeyboardElement.F15:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_F15");
			case Controls.KeyboardElement.Numlock:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_NUMLOCK");
			case Controls.KeyboardElement.CapsLock:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_CAPSLOCK");
			case Controls.KeyboardElement.ScrollLock:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_SCROLLLOCK");
			case Controls.KeyboardElement.RightShift:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_RIGHT_SHIFT");
			case Controls.KeyboardElement.LeftShift:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_LEFT_SHIFT");
			case Controls.KeyboardElement.RightControl:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_RIGHT_CONTROL");
			case Controls.KeyboardElement.LeftControl:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_LEFT_CONTROL");
			case Controls.KeyboardElement.RightAlt:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_RIGHT_ALT");
			case Controls.KeyboardElement.LeftAlt:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_LEFT_ALT");
			case Controls.KeyboardElement.RightCommand:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_RIGHT_COMMAND");
			case Controls.KeyboardElement.LeftCommand:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_LEFT_COMMAND");
			case Controls.KeyboardElement.LeftWindows:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_LEFT_WINDOWS");
			case Controls.KeyboardElement.RightWindows:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_RIGHT_WINDOWS");
			case Controls.KeyboardElement.AltGr:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_ALTGR");
			case Controls.KeyboardElement.Help:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_HELP");
			case Controls.KeyboardElement.Print:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_PRINT");
			case Controls.KeyboardElement.SysReq:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_SYSREQ");
			case Controls.KeyboardElement.Break:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_BREAK");
			case Controls.KeyboardElement.Menu:
				return Translation.Get("KEYBOARD_ELEMENT_NAME_MENU");
			default:
				return "Unknown";
			}
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0004E770 File Offset: 0x0004C970
		public static bool KeyboardElement_IsButton(Controls.KeyboardElement element)
		{
			return true;
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0004E773 File Offset: 0x0004C973
		public static bool KeyboardElement_IsAxis(Controls.KeyboardElement element)
		{
			return false;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0004E776 File Offset: 0x0004C976
		public static bool KeyboardButton_PressedGet(Controls.PlayerExt player, Controls.KeyboardElement element)
		{
			if (player == null)
			{
				Debug.LogError("KeyboardButton_Pressed(): Player is null!");
				return false;
			}
			return player.rePlayer.controllers.hasKeyboard && player.rePlayer.controllers.Keyboard.GetButtonDownById((int)element);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0004E7B1 File Offset: 0x0004C9B1
		public static bool KeyboardButton_PressedGet(int playerIndex, Controls.KeyboardElement element)
		{
			return Controls.KeyboardButton_PressedGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0004E7BF File Offset: 0x0004C9BF
		public static bool KeyboardButton_ReleasedGet(Controls.PlayerExt player, Controls.KeyboardElement element)
		{
			if (player == null)
			{
				Debug.LogError("KeyboardButton_Released(): Player is null!");
				return false;
			}
			return player.rePlayer.controllers.hasKeyboard && player.rePlayer.controllers.Keyboard.GetButtonUpById((int)element);
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0004E7FA File Offset: 0x0004C9FA
		public static bool KeyboardButton_ReleasedGet(int playerIndex, Controls.KeyboardElement element)
		{
			return Controls.KeyboardButton_ReleasedGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0004E808 File Offset: 0x0004CA08
		public static bool KeyboardButton_HoldGet(Controls.PlayerExt player, Controls.KeyboardElement element)
		{
			if (player == null)
			{
				Debug.LogError("KeyboardButton_Hold(): Player is null!");
				return false;
			}
			return player.rePlayer.controllers.hasKeyboard && player.rePlayer.controllers.Keyboard.GetButtonById((int)element);
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0004E843 File Offset: 0x0004CA43
		public static bool KeyboardButton_HoldGet(int playerIndex, Controls.KeyboardElement element)
		{
			return Controls.KeyboardButton_HoldGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0004E854 File Offset: 0x0004CA54
		public static Controls.InputActionMap MapFindInArray(Controls.InputActionMap[] array, Controls.InputAction action)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].myGameAction == action)
				{
					return array[i];
				}
			}
			return null;
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0004E87F File Offset: 0x0004CA7F
		public static Controls.InputActionMap MapFind_InUse(int playerIndex, Controls.InputAction action)
		{
			return Controls.MapFindInArray(Controls.mapsPerPlayerCollection_InUse[playerIndex].maps, action);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0004E893 File Offset: 0x0004CA93
		public static Controls.InputActionMap MapFind_InUse(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapFind_InUse(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0004E8A1 File Offset: 0x0004CAA1
		public static Controls.InputActionMap MapFind_Default(int playerIndex, Controls.InputAction action)
		{
			return Controls.MapFindInArray(Controls.mapsPerPlayerCollection_Default[playerIndex].maps, action);
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0004E8B5 File Offset: 0x0004CAB5
		public static Controls.InputActionMap MapFind_Default(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapFind_Default(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0004E8C4 File Offset: 0x0004CAC4
		public static bool GameActionIsInverted(int playerIndex, Controls.InputAction gameAction)
		{
			switch (gameAction)
			{
			case Controls.InputAction.cameraUp:
				return Data.settings.ControlsInvertCameraYGet(playerIndex);
			case Controls.InputAction.cameraDown:
				return Data.settings.ControlsInvertCameraYGet(playerIndex);
			case Controls.InputAction.cameraLeft:
				return Data.settings.ControlsInvertCameraXGet(playerIndex);
			case Controls.InputAction.cameraRight:
				return Data.settings.ControlsInvertCameraXGet(playerIndex);
			case Controls.InputAction.scrollUp:
				return Data.settings.ControlsInvertScrollingYGet(playerIndex);
			case Controls.InputAction.scrollDown:
				return Data.settings.ControlsInvertScrollingYGet(playerIndex);
			default:
				return false;
			}
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0004E940 File Offset: 0x0004CB40
		private void GameControlsInit()
		{
			for (int i = 0; i < Controls.mapsPerPlayerCollection_InUse.Length; i++)
			{
				Controls.mapsPerPlayerCollection_InUse[i] = new Controls.PlayerMapCollection();
				Controls.mapsPerPlayerCollection_InUse[i].Init();
			}
			for (int j = 0; j < Controls.playerChachedActionStates.Length; j++)
			{
				Controls.playerChachedActionStates[j] = new Controls.PlayerChachedActionStates();
			}
			this.GameMapsInit();
			this._MappingDefaultGenerate();
			Controls.systemReady = true;
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x0004E9A8 File Offset: 0x0004CBA8
		private void InputActionsUpdate()
		{
			for (int i = 0; i < 1; i++)
			{
				Controls.playerChachedActionStates[i]._gameActionState_JustPressed.Clear();
				Controls.playerChachedActionStates[i]._gameActionState_JustReleased.Clear();
				Controls.playerChachedActionStates[i]._gameActionState_Hold.Clear();
				Controls.playerChachedActionStates[i]._gameActionState_HoldPrevious.Clear();
				Controls.playerChachedActionStates[i]._gameActionState_Axis.Clear();
				Controls.playerChachedActionStates[i]._gameActionState_AxisPrevious.Clear();
			}
			for (int j = 0; j < 1; j++)
			{
				foreach (Controls.InputActionMap inputActionMap in Controls.mapsPerPlayerCollection_InUse[j].maps)
				{
					if (inputActionMap != Controls.undefinedGameActionMap)
					{
						inputActionMap.UpdateState();
					}
				}
			}
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0004EA64 File Offset: 0x0004CC64
		private void _MappingDefaultGenerate()
		{
			if (Controls.mapsPerPlayerCollection_Default == null)
			{
				Controls.mapsPerPlayerCollection_Default = new Controls.PlayerMapCollection[1];
			}
			for (int i = 0; i < 1; i++)
			{
				if (Controls.mapsPerPlayerCollection_Default[i] == null)
				{
					Controls.mapsPerPlayerCollection_Default[i] = new Controls.PlayerMapCollection();
					Controls.mapsPerPlayerCollection_Default[i].Init();
				}
				Controls.PlayerMapCollection playerMapCollection = Controls.mapsPerPlayerCollection_Default[i];
				for (int j = 0; j < Controls.mapsPerPlayerCollection_InUse[i].mapsCount; j++)
				{
					Controls.InputActionMap inputActionMap = Controls.mapsPerPlayerCollection_InUse[i].maps[j];
					if (inputActionMap != Controls.undefinedGameActionMap)
					{
						inputActionMap.Copy(playerMapCollection);
					}
				}
			}
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x0004EAF0 File Offset: 0x0004CCF0
		public static void MapsRestoreDefault_AllPlayers(bool affectKeyboard, bool affectMouse, bool affectJoystick)
		{
			if (!Controls.systemReady)
			{
				Debug.LogWarning("Can't restore default mappings. Controls system is not ready yet");
				return;
			}
			for (int i = 0; i < 1; i++)
			{
				Controls.MapRestoreDefault_AllActionsOfPlayer(i, affectKeyboard, affectMouse, affectJoystick, false);
			}
			Controls.MapCallback mapCallback = Controls.onPromptsUpdateRequest;
			if (mapCallback == null)
			{
				return;
			}
			mapCallback(null);
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x0004EB38 File Offset: 0x0004CD38
		public static void MapRestoreDefault_AllActionsOfPlayer(int playerIndex, bool affectKeyboard, bool affectMouse, bool affectJoystick, bool callback = true)
		{
			if (!Controls.systemReady)
			{
				Debug.LogWarning("Can't restore default mappings. Controls system is not ready yet");
				return;
			}
			Controls.PlayerMapCollection playerMapCollection = Controls.mapsPerPlayerCollection_Default[playerIndex];
			for (int i = 0; i < Controls.mapsPerPlayerCollection_InUse[playerIndex].mapsCount; i++)
			{
				Controls.InputActionMap inputActionMap = Controls.mapsPerPlayerCollection_InUse[playerIndex].maps[i];
				if (inputActionMap != null && inputActionMap != Controls.undefinedGameActionMap)
				{
					Controls.InputActionMap inputActionMap2 = Controls.MapFind_Default(playerIndex, inputActionMap.myGameAction);
					if (inputActionMap2 != null)
					{
						inputActionMap2.CopyTo(inputActionMap, affectKeyboard, affectMouse, affectJoystick);
					}
				}
			}
			if (callback)
			{
				Controls.MapCallback mapCallback = Controls.onPromptsUpdateRequest;
				if (mapCallback == null)
				{
					return;
				}
				mapCallback(null);
			}
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x0004EBBE File Offset: 0x0004CDBE
		public static void MapRestoreDefault_AllActionsOfPlayer(Controls.PlayerExt player, bool affectKeyboard, bool affectMouse, bool affectJoystick)
		{
			Controls.MapRestoreDefault_AllActionsOfPlayer(Controls.GetPlayerIndex(player), affectKeyboard, affectMouse, affectJoystick, true);
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0004EBD0 File Offset: 0x0004CDD0
		public static void MapRestoreDefault_Action(int playerIndex, Controls.InputAction action, bool affectKeyboard, bool affectMouse, bool affectJoystick)
		{
			if (!Controls.systemReady)
			{
				Debug.LogWarning("Can't restore default mappings. Controls system is not ready yet");
				return;
			}
			Controls.PlayerMapCollection playerMapCollection = Controls.mapsPerPlayerCollection_Default[playerIndex];
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			if (inputActionMap == null)
			{
				return;
			}
			Controls.InputActionMap inputActionMap2 = Controls.MapFind_Default(playerIndex, action);
			if (inputActionMap2 == null)
			{
				return;
			}
			inputActionMap2.CopyTo(inputActionMap, affectKeyboard, affectMouse, affectJoystick);
			Controls.MapCallback mapCallback = Controls.onPromptsUpdateRequest;
			if (mapCallback == null)
			{
				return;
			}
			mapCallback(null);
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x0004EC2A File Offset: 0x0004CE2A
		public static void MapRestoreDefault_Action(Controls.PlayerExt player, Controls.InputAction action, bool affectKeyboard, bool affectMouse, bool affectJoystick)
		{
			Controls.MapRestoreDefault_Action(Controls.GetPlayerIndex(player), action, affectKeyboard, affectMouse, affectJoystick);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0004EC3C File Offset: 0x0004CE3C
		public static string SaveMapsToJson()
		{
			if (!Controls.systemReady)
			{
				Debug.LogWarning("Can't save game mappings to json. Controls system is not ready yet");
				return "";
			}
			if (Controls.mapsCollectionSerializer == null)
			{
				Controls.mapsCollectionSerializer = new Controls.PlayerMapCollectionSerializer();
			}
			Controls.mapsCollectionSerializer.mapsToSerialize = Controls.mapsPerPlayerCollection_InUse;
			return PlatformDataMaster.ToJson<Controls.PlayerMapCollectionSerializer>(Controls.mapsCollectionSerializer);
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x0004EC8C File Offset: 0x0004CE8C
		public static bool LoadMapsFromJson(string json, bool saveSettingsIfWrongVersion)
		{
			if (!Controls.systemReady)
			{
				Debug.LogWarning("Can't load game mappings from json. Controls system is not ready yet");
				return false;
			}
			if (string.IsNullOrEmpty(json))
			{
				return false;
			}
			bool flag = false;
			Controls.mapsCollectionSerializer = PlatformDataMaster.FromJson<Controls.PlayerMapCollectionSerializer>(json, out flag);
			if (Controls.mapsCollectionSerializer == null)
			{
				Debug.LogWarning("Failed to load game mappings from json");
				return false;
			}
			Controls.mapsPerPlayerCollection_InUse = Controls.mapsCollectionSerializer.mapsToSerialize;
			for (int i = 0; i < Controls.mapsPerPlayerCollection_InUse.Length; i++)
			{
				Controls.mapsPerPlayerCollection_InUse[i].DeserializationCheck();
			}
			return true;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0004ED08 File Offset: 0x0004CF08
		private static bool _IsElementBanned_Keyboard(Controls.KeyboardElement element)
		{
			if (Controls.bannedElementsDict_Keyboard == null)
			{
				Controls.bannedElementsDict_Keyboard = new Dictionary<Controls.KeyboardElement, bool>();
				for (int i = 0; i < Controls.bannedElements_Keyboard.Length; i++)
				{
					Controls.bannedElementsDict_Keyboard.Add(Controls.bannedElements_Keyboard[i], true);
				}
			}
			return Controls.bannedElementsDict_Keyboard.ContainsKey(element);
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x0004ED58 File Offset: 0x0004CF58
		private static bool _IsElementBanned_Mouse(Controls.MouseElement element)
		{
			if (Controls.bannedElementsDict_Mouse == null)
			{
				Controls.bannedElementsDict_Mouse = new Dictionary<Controls.MouseElement, bool>();
				for (int i = 0; i < Controls.bannedElements_Mouse.Length; i++)
				{
					Controls.bannedElementsDict_Mouse.Add(Controls.bannedElements_Mouse[i], true);
				}
			}
			return Controls.bannedElementsDict_Mouse.ContainsKey(element);
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0004EDA8 File Offset: 0x0004CFA8
		private static bool _IsElementBanned_Joystick(Controls.JoystickElement element)
		{
			if (Controls.bannedElementsDict_Joystick == null)
			{
				Controls.bannedElementsDict_Joystick = new Dictionary<Controls.JoystickElement, bool>();
				for (int i = 0; i < Controls.bannedElements_Joystick.Length; i++)
				{
					Controls.bannedElementsDict_Joystick.Add(Controls.bannedElements_Joystick[i], true);
				}
			}
			return Controls.bannedElementsDict_Joystick.ContainsKey(element);
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x0004EDF8 File Offset: 0x0004CFF8
		public static void RemapStart(int playerIndex, Controls.InputKind remapInputKind, Controls.InputAction action, bool allowButtons, bool allowAxes)
		{
			if (Controls.remappingContext.isRunnning)
			{
				Debug.LogError("Can't start remapping. Remapping is already running");
				return;
			}
			if (playerIndex < 0 || playerIndex >= 1)
			{
				Debug.LogError("Can't start remapping. Player index out of bounds: " + playerIndex.ToString());
				return;
			}
			if (remapInputKind == Controls.InputKind.Noone)
			{
				Debug.LogError("Can't start remapping. No input kind defined");
				return;
			}
			if (action == Controls.InputAction._UNDEFINED)
			{
				Debug.LogError("Can't start remapping. You are trying to remap an action of kind 'undefined'");
				return;
			}
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			if (inputActionMap == null)
			{
				Debug.LogError("Can't start remapping. Action not found: " + action.ToString());
				return;
			}
			if (inputActionMap == Controls.undefinedGameActionMap)
			{
				Debug.LogError("Can't start remapping. Action is undefined: " + action.ToString());
				return;
			}
			if (!inputActionMap.MapChangeableGet())
			{
				Debug.LogError("Can't start remapping. Action is not changeable: " + action.ToString());
				return;
			}
			Controls.remappingContext.mapToRemap = inputActionMap;
			if (Controls.remappingContext.tempMap == null)
			{
				Controls.remappingContext.tempMap = new Controls.InputActionMap(playerIndex, action, inputActionMap.myGameActionRange, null, inputActionMap.updateIfNotPlaying);
			}
			inputActionMap.CopyTo(Controls.remappingContext.tempMap, true, true, true);
			Controls.remappingContext.tempMap.myPlayerIndex = playerIndex;
			switch (remapInputKind)
			{
			case Controls.InputKind.Keyboard:
				Controls.remappingContext.tempMap.ElementKeyboard_Clear();
				break;
			case Controls.InputKind.Mouse:
				Controls.remappingContext.tempMap.ElementMouse_Clear();
				break;
			case Controls.InputKind.Joystick:
				Controls.remappingContext.tempMap.ElementJoystick_Clear();
				break;
			}
			if (Controls.remappingContext.tempMap == null)
			{
				Debug.LogError("Can't start remapping. Failed to create temp map");
				return;
			}
			Controls.remappingContext.remappingIputKind = remapInputKind;
			Controls.remappingContext.allowButtons = allowButtons;
			Controls.remappingContext.allowAxes = allowAxes;
			Controls.remappingContext.isRunnning = true;
			Controls.MapCallback mapCallback = Controls.onRemap_Start;
			if (mapCallback == null)
			{
				return;
			}
			mapCallback(Controls.remappingContext.mapToRemap);
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x0004EFC8 File Offset: 0x0004D1C8
		public static void RemapEnd(bool abortRemapping)
		{
			if (!Controls.remappingContext.isRunnning)
			{
				Debug.LogError("Can't end remapping. Remapping is not running");
				return;
			}
			if (!abortRemapping && !Controls.remappingContext.tempMap.HasNoElements())
			{
				Controls.remappingContext.tempMap.CopyTo(Controls.remappingContext.mapToRemap, true, true, true);
			}
			Controls.remappingContext.isRunnning = false;
			Controls.MapCallback mapCallback = Controls.onRemap_End_Generic;
			if (mapCallback != null)
			{
				mapCallback(Controls.remappingContext.mapToRemap);
			}
			if (!abortRemapping)
			{
				Controls.MapCallback mapCallback2 = Controls.onRemap_End_Success;
				if (mapCallback2 == null)
				{
					return;
				}
				mapCallback2(Controls.remappingContext.mapToRemap);
				return;
			}
			else
			{
				Controls.MapCallback mapCallback3 = Controls.onRemap_End_Abort;
				if (mapCallback3 == null)
				{
					return;
				}
				mapCallback3(Controls.remappingContext.mapToRemap);
				return;
			}
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x0004F077 File Offset: 0x0004D277
		public static bool RemapIsRunning()
		{
			return Controls.remappingContext.isRunnning;
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x0004F084 File Offset: 0x0004D284
		private void _RemapContextUpdate()
		{
			if (!Controls.remappingContext.isRunnning)
			{
				return;
			}
			switch (Controls.remappingContext.remappingIputKind)
			{
			case Controls.InputKind.Keyboard:
				this._RemapContextUpdate_Keyboard();
				return;
			case Controls.InputKind.Mouse:
				this._RemapContextUpdate_Mouse();
				return;
			case Controls.InputKind.Joystick:
				this._RemapContextUpdate_Joystick();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x0004F0D4 File Offset: 0x0004D2D4
		private void _RemapContextUpdate_Keyboard()
		{
			if (!Controls.remappingContext.allowButtons)
			{
				return;
			}
			int num = 132;
			for (int i = 0; i < num; i++)
			{
				Controls.KeyboardElement keyboardElement = (Controls.KeyboardElement)i;
				if (!Controls._IsElementBanned_Keyboard(keyboardElement) && Controls.KeyboardButton_PressedGet(Controls.remappingContext.mapToRemap.myPlayer, keyboardElement) && Controls.remappingContext.tempMap.ElementKeyboard_Add(keyboardElement))
				{
					Controls.MapCallback mapCallback = Controls.onRemap_InputAdded;
					if (mapCallback != null)
					{
						mapCallback(Controls.remappingContext.tempMap);
					}
				}
			}
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x0004F150 File Offset: 0x0004D350
		private void _RemapContextUpdate_Mouse()
		{
			int num = 7;
			for (int i = 0; i < num; i++)
			{
				Controls.MouseElement mouseElement = (Controls.MouseElement)i;
				if (!Controls._IsElementBanned_Mouse(mouseElement))
				{
					if (Controls.MouseElement_IsButton(mouseElement) && Controls.remappingContext.allowButtons)
					{
						if (Controls.MouseButton_PressedGet(Controls.remappingContext.mapToRemap.myPlayer, mouseElement) && Controls.remappingContext.tempMap.ElementMouse_Add(mouseElement))
						{
							Controls.MapCallback mapCallback = Controls.onRemap_InputAdded;
							if (mapCallback != null)
							{
								mapCallback(Controls.remappingContext.tempMap);
							}
						}
					}
					else if (Controls.MouseElement_IsAxis(mouseElement) && Controls.remappingContext.allowAxes)
					{
						Controls.MouseElement mouseElement2 = Controls.MouseElement.Undefined;
						Controls.MouseElement mouseElement3 = Controls.MouseElement.Undefined;
						Controls.MouseElement mouseElement4 = Controls.MouseElement.Undefined;
						if (mouseElement == Controls.MouseElement.axisX || mouseElement == Controls.MouseElement.axisY)
						{
							mouseElement2 = Controls.MouseElement.axisX;
							mouseElement3 = Controls.MouseElement.axisY;
						}
						else if (mouseElement == Controls.MouseElement.axisScrollWheelHorizontal || mouseElement == Controls.MouseElement.axisScrollWheelVertical)
						{
							mouseElement2 = Controls.MouseElement.axisScrollWheelHorizontal;
							mouseElement3 = Controls.MouseElement.axisScrollWheelVertical;
						}
						if (mouseElement4 == Controls.MouseElement.Undefined)
						{
							Controls.PickStickAxis_Mouse(Controls.remappingContext.mapToRemap.myPlayerIndex, mouseElement2, mouseElement3, out mouseElement4);
						}
						if (mouseElement4 != Controls.MouseElement.Undefined && Controls.remappingContext.tempMap.ElementMouse_Add(mouseElement4))
						{
							Controls.MapCallback mapCallback2 = Controls.onRemap_InputAdded;
							if (mapCallback2 != null)
							{
								mapCallback2(Controls.remappingContext.tempMap);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x0004F270 File Offset: 0x0004D470
		private void _RemapContextUpdate_Joystick()
		{
			int num = 21;
			for (int i = 0; i < num; i++)
			{
				Controls.JoystickElement joystickElement = (Controls.JoystickElement)i;
				if (!Controls._IsElementBanned_Joystick(joystickElement))
				{
					bool flag = joystickElement == Controls.JoystickElement.LeftTrigger || joystickElement == Controls.JoystickElement.RightTrigger;
					if ((Controls.JoystickElement_IsButton(joystickElement) || flag) && Controls.remappingContext.allowButtons)
					{
						if (Controls.JoystickButton_PressedGet(Controls.remappingContext.mapToRemap.myPlayer, joystickElement) && Controls.remappingContext.tempMap.ElementJoystick_Add(joystickElement))
						{
							Controls.MapCallback mapCallback = Controls.onRemap_InputAdded;
							if (mapCallback != null)
							{
								mapCallback(Controls.remappingContext.tempMap);
							}
						}
					}
					else if (Controls.JoystickElement_IsAxis(joystickElement) && Controls.remappingContext.allowAxes)
					{
						Controls.JoystickElement joystickElement2 = Controls.JoystickElement.Undefined;
						Controls.JoystickElement joystickElement3 = Controls.JoystickElement.Undefined;
						Controls.JoystickElement joystickElement4 = Controls.JoystickElement.Undefined;
						if (joystickElement == Controls.JoystickElement.LeftStickX || joystickElement == Controls.JoystickElement.LeftStickY)
						{
							joystickElement2 = Controls.JoystickElement.LeftStickX;
							joystickElement3 = Controls.JoystickElement.LeftStickY;
						}
						else if (joystickElement == Controls.JoystickElement.RightStickX || joystickElement == Controls.JoystickElement.RightStickY)
						{
							joystickElement2 = Controls.JoystickElement.RightStickX;
							joystickElement3 = Controls.JoystickElement.RightStickY;
						}
						if (joystickElement == Controls.JoystickElement.LeftTrigger)
						{
							joystickElement4 = Controls.JoystickElement.LeftTrigger;
						}
						else if (joystickElement == Controls.JoystickElement.RightTrigger)
						{
							joystickElement4 = Controls.JoystickElement.RightTrigger;
						}
						if (joystickElement4 == Controls.JoystickElement.Undefined)
						{
							Controls.PickStickAxis_Joystick(Controls.remappingContext.mapToRemap.myPlayerIndex, joystickElement2, joystickElement3, out joystickElement4);
						}
						if (joystickElement4 != Controls.JoystickElement.Undefined && Controls.remappingContext.tempMap.ElementJoystick_Add(joystickElement4))
						{
							Controls.MapCallback mapCallback2 = Controls.onRemap_InputAdded;
							if (mapCallback2 != null)
							{
								mapCallback2(Controls.remappingContext.tempMap);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x0004F3C8 File Offset: 0x0004D5C8
		public static List<Controls.KeyboardElement> MapGetKeyboardInputs(int playerIndex, Controls.InputActionMap map)
		{
			if (map == null)
			{
				return null;
			}
			return map.ElementKeyboard_Get();
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x0004F3D5 File Offset: 0x0004D5D5
		public static List<Controls.KeyboardElement> MapGetKeyboardInputs(Controls.PlayerExt player, Controls.InputActionMap map)
		{
			return Controls.MapGetKeyboardInputs(Controls.GetPlayerIndex(player), map);
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x0004F3E4 File Offset: 0x0004D5E4
		public static List<Controls.KeyboardElement> MapGetKeyboardInputs(int playerIndex, Controls.InputAction action)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetKeyboardInputs(playerIndex, inputActionMap);
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x0004F400 File Offset: 0x0004D600
		public static List<Controls.KeyboardElement> MapGetKeyboardInputs(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapGetKeyboardInputs(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x0004F40E File Offset: 0x0004D60E
		public static List<Controls.MouseElement> MapGetMouseInputs(int playerIndex, Controls.InputActionMap map)
		{
			if (map == null)
			{
				return null;
			}
			return map.ElementMouse_Get();
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x0004F41B File Offset: 0x0004D61B
		public static List<Controls.MouseElement> MapGetMouseInputs(Controls.PlayerExt player, Controls.InputActionMap map)
		{
			return Controls.MapGetMouseInputs(Controls.GetPlayerIndex(player), map);
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x0004F42C File Offset: 0x0004D62C
		public static List<Controls.MouseElement> MapGetMouseInputs(int playerIndex, Controls.InputAction action)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetMouseInputs(playerIndex, inputActionMap);
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0004F448 File Offset: 0x0004D648
		public static List<Controls.MouseElement> MapGetMouseInputs(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapGetMouseInputs(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0004F456 File Offset: 0x0004D656
		public static List<Controls.JoystickElement> MapGetJoystickInputs(int playerIndex, Controls.InputActionMap map)
		{
			if (map == null)
			{
				return null;
			}
			return map.ElementJoystick_Get();
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x0004F463 File Offset: 0x0004D663
		public static List<Controls.JoystickElement> MapGetJoystickInputs(Controls.PlayerExt player, Controls.InputActionMap map)
		{
			return Controls.MapGetJoystickInputs(Controls.GetPlayerIndex(player), map);
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x0004F474 File Offset: 0x0004D674
		public static List<Controls.JoystickElement> MapGetJoystickInputs(int playerIndex, Controls.InputAction action)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetJoystickInputs(playerIndex, inputActionMap);
		}

		// Token: 0x06000BE9 RID: 3049 RVA: 0x0004F490 File Offset: 0x0004D690
		public static List<Controls.JoystickElement> MapGetJoystickInputs(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapGetJoystickInputs(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x0004F4A0 File Offset: 0x0004D6A0
		public static List<string> MapGetKeyboardInputs_Names(int playerIndex, Controls.InputActionMap map)
		{
			List<Controls.KeyboardElement> list = Controls.MapGetKeyboardInputs(playerIndex, map);
			if (list == null)
			{
				return null;
			}
			List<string> list2 = new List<string>();
			foreach (Controls.KeyboardElement keyboardElement in list)
			{
				list2.Add(Controls.KeyboardElementName(keyboardElement));
			}
			return list2;
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x0004F508 File Offset: 0x0004D708
		public static List<string> MapGetKeyboardInputs_Names(Controls.PlayerExt player, Controls.InputActionMap map)
		{
			return Controls.MapGetKeyboardInputs_Names(Controls.GetPlayerIndex(player), map);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x0004F518 File Offset: 0x0004D718
		public static List<string> MapGetKeyboardInputs_Names(int playerIndex, Controls.InputAction action)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetKeyboardInputs_Names(playerIndex, inputActionMap);
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x0004F534 File Offset: 0x0004D734
		public static List<string> MapGetKeyboardInputs_Names(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapGetKeyboardInputs_Names(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x0004F544 File Offset: 0x0004D744
		public static List<string> MapGetMouseInputs_Names(int playerIndex, Controls.InputActionMap map)
		{
			List<Controls.MouseElement> list = Controls.MapGetMouseInputs(playerIndex, map);
			if (list == null)
			{
				return null;
			}
			List<string> list2 = new List<string>();
			foreach (Controls.MouseElement mouseElement in list)
			{
				list2.Add(Controls.MouseElementName(mouseElement));
			}
			return list2;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x0004F5AC File Offset: 0x0004D7AC
		public static List<string> MapGetMouseInputs_Names(Controls.PlayerExt player, Controls.InputActionMap map)
		{
			return Controls.MapGetMouseInputs_Names(Controls.GetPlayerIndex(player), map);
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x0004F5BC File Offset: 0x0004D7BC
		public static List<string> MapGetMouseInputs_Names(int playerIndex, Controls.InputAction action)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetMouseInputs_Names(playerIndex, inputActionMap);
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x0004F5D8 File Offset: 0x0004D7D8
		public static List<string> MapGetMouseInputs_Names(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapGetMouseInputs_Names(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x0004F5E8 File Offset: 0x0004D7E8
		public static List<string> MapGetJoystickInputs_Names(int playerIndex, Controls.InputActionMap map)
		{
			List<Controls.JoystickElement> list = Controls.MapGetJoystickInputs(playerIndex, map);
			if (list == null)
			{
				return null;
			}
			List<string> list2 = new List<string>();
			foreach (Controls.JoystickElement joystickElement in list)
			{
				list2.Add(Controls.JoystickElementName(joystickElement));
			}
			return list2;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x0004F650 File Offset: 0x0004D850
		public static List<string> MapGetJoystickInputs_Names(Controls.PlayerExt player, Controls.InputActionMap map)
		{
			return Controls.MapGetJoystickInputs_Names(Controls.GetPlayerIndex(player), map);
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x0004F660 File Offset: 0x0004D860
		public static List<string> MapGetJoystickInputs_Names(int playerIndex, Controls.InputAction action)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetJoystickInputs_Names(playerIndex, inputActionMap);
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0004F67C File Offset: 0x0004D87C
		public static List<string> MapGetJoystickInputs_Names(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapGetJoystickInputs_Names(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0004F68C File Offset: 0x0004D88C
		public static List<string> MapGetLastInputs_Names(Controls.PlayerExt player, Controls.InputActionMap map, bool keyboardMouseFallback)
		{
			switch (player.lastInputKindUsed)
			{
			case Controls.InputKind.Noone:
				Debug.LogError("Can't get last inputs names. The last input kind is noone");
				return null;
			case Controls.InputKind.Keyboard:
			{
				List<string> list = Controls.MapGetKeyboardInputs_Names(player, map);
				if (!keyboardMouseFallback)
				{
					return list;
				}
				if (list == null || list.Count == 0)
				{
					return Controls.MapGetMouseInputs_Names(player, map);
				}
				return list;
			}
			case Controls.InputKind.Mouse:
			{
				List<string> list = Controls.MapGetMouseInputs_Names(player, map);
				if (!keyboardMouseFallback)
				{
					return list;
				}
				if (list == null || list.Count == 0)
				{
					return Controls.MapGetKeyboardInputs_Names(player, map);
				}
				return list;
			}
			case Controls.InputKind.Joystick:
				return Controls.MapGetJoystickInputs_Names(player, map);
			default:
				Debug.LogError("Can't get last inputs names. The last input kind is apparently invalid. We reached the default state of the switch");
				return null;
			}
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0004F71C File Offset: 0x0004D91C
		public static List<string> MapGetLastInputs_Names(Controls.PlayerExt player, Controls.InputAction action, bool keyboardMouseFallback)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(player, action);
			return Controls.MapGetLastInputs_Names(player, inputActionMap, keyboardMouseFallback);
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x0004F739 File Offset: 0x0004D939
		public static List<string> MapGetLastInputs_Names(int playerIndex, Controls.InputActionMap actionMap, bool keyboardMouseFallback)
		{
			return Controls.MapGetLastInputs_Names(Controls.GetPlayerByIndex(playerIndex), actionMap, keyboardMouseFallback);
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0004F748 File Offset: 0x0004D948
		public static List<string> MapGetLastInputs_Names(int playerIndex, Controls.InputAction action, bool keyboardMouseFallback)
		{
			return Controls.MapGetLastInputs_Names(Controls.GetPlayerByIndex(playerIndex), action, keyboardMouseFallback);
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0004F758 File Offset: 0x0004D958
		public static void MapGetKeyboardPrompts_Sprites(int playerIndex, Controls.InputActionMap map, ref List<Sprite> spritesOut)
		{
			if (spritesOut == null)
			{
				Debug.LogError("Can't get keyboard prompts sprites. List is null");
				return;
			}
			spritesOut.Clear();
			List<Controls.KeyboardElement> list = Controls.MapGetKeyboardInputs(playerIndex, map);
			if (list == null)
			{
				return;
			}
			foreach (Controls.KeyboardElement keyboardElement in list)
			{
				spritesOut.Add(PromptsMaster.GetSprite_Keyboard(keyboardElement));
			}
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0004F7D0 File Offset: 0x0004D9D0
		public static void MapGetKeyboardPrompts_Sprites(Controls.PlayerExt player, Controls.InputActionMap map, ref List<Sprite> spritesOut)
		{
			Controls.MapGetKeyboardPrompts_Sprites(Controls.GetPlayerIndex(player), map, ref spritesOut);
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0004F7E0 File Offset: 0x0004D9E0
		public static void MapGetKeyboardPrompts_Sprites(int playerIndex, Controls.InputAction action, ref List<Sprite> spritesOut)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			Controls.MapGetKeyboardPrompts_Sprites(playerIndex, inputActionMap, ref spritesOut);
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x0004F7FD File Offset: 0x0004D9FD
		public static void MapGetKeyboardPrompts_Sprites(Controls.PlayerExt player, Controls.InputAction action, ref List<Sprite> spritesOut)
		{
			Controls.MapGetKeyboardPrompts_Sprites(Controls.GetPlayerIndex(player), action, ref spritesOut);
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0004F80C File Offset: 0x0004DA0C
		public static void MapGetMousePrompts_Sprites(int playerIndex, Controls.InputActionMap map, ref List<Sprite> spritesOut)
		{
			if (spritesOut == null)
			{
				Debug.LogError("Can't get mouse prompts sprites. List is null");
				return;
			}
			spritesOut.Clear();
			List<Controls.MouseElement> list = Controls.MapGetMouseInputs(playerIndex, map);
			if (list == null)
			{
				return;
			}
			foreach (Controls.MouseElement mouseElement in list)
			{
				spritesOut.Add(PromptsMaster.GetSprite_Mouse(mouseElement));
			}
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x0004F884 File Offset: 0x0004DA84
		public static void MapGetMousePrompts_Sprites(Controls.PlayerExt player, Controls.InputActionMap map, ref List<Sprite> spritesOut)
		{
			Controls.MapGetMousePrompts_Sprites(Controls.GetPlayerIndex(player), map, ref spritesOut);
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x0004F894 File Offset: 0x0004DA94
		public static void MapGetMousePrompts_Sprites(int playerIndex, Controls.InputAction action, ref List<Sprite> spritesOut)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			Controls.MapGetMousePrompts_Sprites(playerIndex, inputActionMap, ref spritesOut);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x0004F8B1 File Offset: 0x0004DAB1
		public static void MapGetMousePrompts_Sprites(Controls.PlayerExt player, Controls.InputAction action, ref List<Sprite> spritesOut)
		{
			Controls.MapGetMousePrompts_Sprites(Controls.GetPlayerIndex(player), action, ref spritesOut);
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0004F8C0 File Offset: 0x0004DAC0
		public static void MapGetJoystickPrompts_Sprites(int playerIndex, Controls.InputActionMap map, ref List<Sprite> spritesOut)
		{
			if (spritesOut == null)
			{
				Debug.LogError("Can't get joystick prompts sprites. List is null");
				return;
			}
			spritesOut.Clear();
			List<Controls.JoystickElement> list = Controls.MapGetJoystickInputs(playerIndex, map);
			if (list == null)
			{
				return;
			}
			foreach (Controls.JoystickElement joystickElement in list)
			{
				spritesOut.Add(PromptsMaster.GetSprite_Joystick(joystickElement));
			}
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0004F938 File Offset: 0x0004DB38
		public static void MapGetJoystickPrompts_Sprites(Controls.PlayerExt player, Controls.InputActionMap map, ref List<Sprite> spritesOut)
		{
			Controls.MapGetJoystickPrompts_Sprites(Controls.GetPlayerIndex(player), map, ref spritesOut);
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x0004F948 File Offset: 0x0004DB48
		public static void MapGetJoystickPrompts_Sprites(int playerIndex, Controls.InputAction action, ref List<Sprite> spritesOut)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			Controls.MapGetJoystickPrompts_Sprites(playerIndex, inputActionMap, ref spritesOut);
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0004F965 File Offset: 0x0004DB65
		public static void MapGetJoystickPrompts_Sprites(Controls.PlayerExt player, Controls.InputAction action, ref List<Sprite> spritesOut)
		{
			Controls.MapGetJoystickPrompts_Sprites(Controls.GetPlayerIndex(player), action, ref spritesOut);
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x0004F974 File Offset: 0x0004DB74
		public static void MapGetLastPrompts_Sprites(Controls.PlayerExt player, Controls.InputActionMap map, ref List<Sprite> spritesOut, bool keyboardMouseFallback)
		{
			switch (player.lastInputKindUsed)
			{
			case Controls.InputKind.Noone:
				Debug.LogError("Can't get last prompts sprites. The last input kind is noone");
				return;
			case Controls.InputKind.Keyboard:
				Controls.MapGetKeyboardPrompts_Sprites(player, map, ref spritesOut);
				if (keyboardMouseFallback && (spritesOut == null || spritesOut.Count == 0))
				{
					Controls.MapGetMousePrompts_Sprites(player, map, ref spritesOut);
					return;
				}
				break;
			case Controls.InputKind.Mouse:
				Controls.MapGetMousePrompts_Sprites(player, map, ref spritesOut);
				if (keyboardMouseFallback && (spritesOut == null || spritesOut.Count == 0))
				{
					Controls.MapGetKeyboardPrompts_Sprites(player, map, ref spritesOut);
					return;
				}
				break;
			case Controls.InputKind.Joystick:
				Controls.MapGetJoystickPrompts_Sprites(player, map, ref spritesOut);
				return;
			default:
				Debug.LogError("Can't get last prompts sprites. The last input kind is apparently invalid. We reached the default state of the switch");
				return;
			}
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0004FA04 File Offset: 0x0004DC04
		public static void MapGetLastPrompts_Sprites(Controls.PlayerExt player, Controls.InputAction action, ref List<Sprite> spritesOut, bool keyboardMouseFallback)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(player, action);
			Controls.MapGetLastPrompts_Sprites(player, inputActionMap, ref spritesOut, keyboardMouseFallback);
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0004FA22 File Offset: 0x0004DC22
		public static void MapGetLastPrompts_Sprites(int playerIndex, Controls.InputActionMap map, ref List<Sprite> spritesOut, bool keyboardMouseFallback)
		{
			Controls.MapGetLastPrompts_Sprites(Controls.GetPlayerByIndex(playerIndex), map, ref spritesOut, keyboardMouseFallback);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0004FA32 File Offset: 0x0004DC32
		public static void MapGetLastPrompts_Sprites(int playerIndex, Controls.InputAction action, ref List<Sprite> spritesOut, bool keyboardMouseFallback)
		{
			Controls.MapGetLastPrompts_Sprites(Controls.GetPlayerByIndex(playerIndex), action, ref spritesOut, keyboardMouseFallback);
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x0004FA44 File Offset: 0x0004DC44
		public static void MapGetKeyboardPrompts_TextSprites(int playerIndex, Controls.InputActionMap map, ref List<string> textSpritesOut)
		{
			if (textSpritesOut == null)
			{
				Debug.LogError("Can't get keyboard prompts text sprites. List is null");
				return;
			}
			textSpritesOut.Clear();
			List<Controls.KeyboardElement> list = Controls.MapGetKeyboardInputs(playerIndex, map);
			if (list == null)
			{
				return;
			}
			foreach (Controls.KeyboardElement keyboardElement in list)
			{
				textSpritesOut.Add(PromptsMaster.GetSpriteString_Keyboard(keyboardElement));
			}
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0004FABC File Offset: 0x0004DCBC
		public static void MapGetKeyboardPrompts_TextSprites(Controls.PlayerExt player, Controls.InputActionMap map, ref List<string> textSpritesOut)
		{
			Controls.MapGetKeyboardPrompts_TextSprites(Controls.GetPlayerIndex(player), map, ref textSpritesOut);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0004FACC File Offset: 0x0004DCCC
		public static void MapGetKeyboardPrompts_TextSprites(int playerIndex, Controls.InputAction action, ref List<string> textSpritesOut)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			Controls.MapGetKeyboardPrompts_TextSprites(playerIndex, inputActionMap, ref textSpritesOut);
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x0004FAE9 File Offset: 0x0004DCE9
		public static void MapGetKeyboardPrompts_TextSprites(Controls.PlayerExt player, Controls.InputAction action, ref List<string> textSpritesOut)
		{
			Controls.MapGetKeyboardPrompts_TextSprites(Controls.GetPlayerIndex(player), action, ref textSpritesOut);
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0004FAF8 File Offset: 0x0004DCF8
		public static void MapGetMousePrompts_TextSprites(int playerIndex, Controls.InputActionMap map, ref List<string> textSpritesOut)
		{
			if (textSpritesOut == null)
			{
				Debug.LogError("Can't get keyboard prompts text sprites. List is null");
				return;
			}
			textSpritesOut.Clear();
			List<Controls.MouseElement> list = Controls.MapGetMouseInputs(playerIndex, map);
			if (list == null)
			{
				return;
			}
			foreach (Controls.MouseElement mouseElement in list)
			{
				textSpritesOut.Add(PromptsMaster.GetSpriteString_Mouse(mouseElement));
			}
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0004FB70 File Offset: 0x0004DD70
		public static void MapGetMousePrompts_TextSprites(Controls.PlayerExt player, Controls.InputActionMap map, ref List<string> textSpritesOut)
		{
			Controls.MapGetMousePrompts_TextSprites(Controls.GetPlayerIndex(player), map, ref textSpritesOut);
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0004FB80 File Offset: 0x0004DD80
		public static void MapGetMousePrompts_TextSprites(int playerIndex, Controls.InputAction action, ref List<string> textSpritesOut)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			Controls.MapGetMousePrompts_TextSprites(playerIndex, inputActionMap, ref textSpritesOut);
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0004FB9D File Offset: 0x0004DD9D
		public static void MapGetMousePrompts_TextSprites(Controls.PlayerExt player, Controls.InputAction action, ref List<string> textSpritesOut)
		{
			Controls.MapGetMousePrompts_TextSprites(Controls.GetPlayerIndex(player), action, ref textSpritesOut);
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x0004FBAC File Offset: 0x0004DDAC
		public static void MapGetJoystickPrompts_TextSprites(int playerIndex, Controls.InputActionMap map, ref List<string> textSpritesOut)
		{
			if (textSpritesOut == null)
			{
				Debug.LogError("Can't get keyboard prompts text sprites. List is null");
				return;
			}
			textSpritesOut.Clear();
			List<Controls.JoystickElement> list = Controls.MapGetJoystickInputs(playerIndex, map);
			if (list == null)
			{
				return;
			}
			foreach (Controls.JoystickElement joystickElement in list)
			{
				textSpritesOut.Add(PromptsMaster.GetSpriteString_Joystick(joystickElement));
			}
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0004FC24 File Offset: 0x0004DE24
		public static void MapGetJoystickPrompts_TextSprites(Controls.PlayerExt player, Controls.InputActionMap map, ref List<string> textSpritesOut)
		{
			Controls.MapGetJoystickPrompts_TextSprites(Controls.GetPlayerIndex(player), map, ref textSpritesOut);
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0004FC34 File Offset: 0x0004DE34
		public static void MapGetJoystickPrompts_TextSprites(int playerIndex, Controls.InputAction action, ref List<string> textSpritesOut)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			Controls.MapGetJoystickPrompts_TextSprites(playerIndex, inputActionMap, ref textSpritesOut);
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0004FC51 File Offset: 0x0004DE51
		public static void MapGetJoystickPrompts_TextSprites(Controls.PlayerExt player, Controls.InputAction action, ref List<string> textSpritesOut)
		{
			Controls.MapGetJoystickPrompts_TextSprites(Controls.GetPlayerIndex(player), action, ref textSpritesOut);
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0004FC60 File Offset: 0x0004DE60
		public static void MapGetLastPrompts_TextSprites(Controls.PlayerExt player, Controls.InputActionMap map, ref List<string> textSpritesOut, bool keyboardMouseFallback)
		{
			switch (player.lastInputKindUsed)
			{
			case Controls.InputKind.Noone:
				Debug.LogError("Can't get last prompts text sprites. The last input kind is noone");
				return;
			case Controls.InputKind.Keyboard:
				Controls.MapGetKeyboardPrompts_TextSprites(player, map, ref textSpritesOut);
				if (keyboardMouseFallback && (textSpritesOut == null || textSpritesOut.Count == 0))
				{
					Controls.MapGetMousePrompts_TextSprites(player, map, ref textSpritesOut);
					return;
				}
				break;
			case Controls.InputKind.Mouse:
				Controls.MapGetMousePrompts_TextSprites(player, map, ref textSpritesOut);
				if (keyboardMouseFallback && (textSpritesOut == null || textSpritesOut.Count == 0))
				{
					Controls.MapGetKeyboardPrompts_TextSprites(player, map, ref textSpritesOut);
					return;
				}
				break;
			case Controls.InputKind.Joystick:
				Controls.MapGetJoystickPrompts_TextSprites(player, map, ref textSpritesOut);
				return;
			default:
				Debug.LogError("Can't get last prompts text sprites. The last input kind is apparently invalid. We reached the default state of the switch");
				return;
			}
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0004FCF0 File Offset: 0x0004DEF0
		public static void MapGetLastPrompts_TextSprites(Controls.PlayerExt player, Controls.InputAction action, ref List<string> textSpritesOut, bool keyboardMouseFallback)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(player, action);
			Controls.MapGetLastPrompts_TextSprites(player, inputActionMap, ref textSpritesOut, keyboardMouseFallback);
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x0004FD0E File Offset: 0x0004DF0E
		public static void MapGetLastPrompts_TextSprites(int playerIndex, Controls.InputActionMap map, ref List<string> textSpritesOut, bool keyboardMouseFallback)
		{
			Controls.MapGetLastPrompts_TextSprites(Controls.GetPlayerByIndex(playerIndex), map, ref textSpritesOut, keyboardMouseFallback);
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0004FD1E File Offset: 0x0004DF1E
		public static void MapGetLastPrompts_TextSprites(int playerIndex, Controls.InputAction action, ref List<string> textSpritesOut, bool keyboardMouseFallback)
		{
			Controls.MapGetLastPrompts_TextSprites(Controls.GetPlayerByIndex(playerIndex), action, ref textSpritesOut, keyboardMouseFallback);
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0004FD30 File Offset: 0x0004DF30
		public static Sprite MapGetKeyboardPrompt_Sprite(int playerIndex, Controls.InputActionMap map, int index = 0)
		{
			List<Controls.KeyboardElement> list = Controls.MapGetKeyboardInputs(playerIndex, map);
			if (list == null)
			{
				return null;
			}
			if (index < 0 || index >= list.Count)
			{
				return null;
			}
			return PromptsMaster.GetSprite_Keyboard(list[index]);
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0004FD65 File Offset: 0x0004DF65
		public static Sprite MapGetKeyboardPrompt_Sprite(Controls.PlayerExt player, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetKeyboardPrompt_Sprite(Controls.GetPlayerIndex(player), map, index);
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x0004FD74 File Offset: 0x0004DF74
		public static Sprite MapGetKeyboardPrompt_Sprite(int playerIndex, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetKeyboardPrompt_Sprite(playerIndex, inputActionMap, index);
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0004FD91 File Offset: 0x0004DF91
		public static Sprite MapGetKeyboardPrompt_Sprite(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetKeyboardPrompt_Sprite(Controls.GetPlayerIndex(player), action, index);
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0004FDA0 File Offset: 0x0004DFA0
		public static Sprite MapGetMousePrompt_Sprite(int playerIndex, Controls.InputActionMap map, int index = 0)
		{
			List<Controls.MouseElement> list = Controls.MapGetMouseInputs(playerIndex, map);
			if (list == null)
			{
				return null;
			}
			if (index < 0 || index >= list.Count)
			{
				return null;
			}
			return PromptsMaster.GetSprite_Mouse(list[index]);
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0004FDD5 File Offset: 0x0004DFD5
		public static Sprite MapGetMousePrompt_Sprite(Controls.PlayerExt player, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetMousePrompt_Sprite(Controls.GetPlayerIndex(player), map, index);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x0004FDE4 File Offset: 0x0004DFE4
		public static Sprite MapGetMousePrompt_Sprite(int playerIndex, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetMousePrompt_Sprite(playerIndex, inputActionMap, index);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0004FE01 File Offset: 0x0004E001
		public static Sprite MapGetMousePrompt_Sprite(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetMousePrompt_Sprite(Controls.GetPlayerIndex(player), action, index);
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0004FE10 File Offset: 0x0004E010
		public static Sprite MapGetJoystickPrompt_Sprite(int playerIndex, Controls.InputActionMap map, int index = 0)
		{
			List<Controls.JoystickElement> list = Controls.MapGetJoystickInputs(playerIndex, map);
			if (list == null)
			{
				return null;
			}
			if (index < 0 || index >= list.Count)
			{
				return null;
			}
			return PromptsMaster.GetSprite_Joystick(list[index]);
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x0004FE45 File Offset: 0x0004E045
		public static Sprite MapGetJoystickPrompt_Sprite(Controls.PlayerExt player, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetJoystickPrompt_Sprite(Controls.GetPlayerIndex(player), map, index);
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0004FE54 File Offset: 0x0004E054
		public static Sprite MapGetJoystickPrompt_Sprite(int playerIndex, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetJoystickPrompt_Sprite(playerIndex, inputActionMap, index);
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x0004FE71 File Offset: 0x0004E071
		public static Sprite MapGetJoystickPrompt_Sprite(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetJoystickPrompt_Sprite(Controls.GetPlayerIndex(player), action, index);
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0004FE80 File Offset: 0x0004E080
		public static Sprite MapGetLastPrompt_Sprite(Controls.PlayerExt player, Controls.InputActionMap map, bool keyboardMouseFallback, int index = 0)
		{
			switch (player.lastInputKindUsed)
			{
			case Controls.InputKind.Noone:
				Debug.LogError("Can't get last prompt sprite. The last input kind is noone");
				return null;
			case Controls.InputKind.Keyboard:
			{
				Sprite sprite = Controls.MapGetKeyboardPrompt_Sprite(player, map, index);
				if (!keyboardMouseFallback)
				{
					return sprite;
				}
				if (sprite == null)
				{
					return Controls.MapGetMousePrompt_Sprite(player, map, index);
				}
				return sprite;
			}
			case Controls.InputKind.Mouse:
			{
				Sprite sprite = Controls.MapGetMousePrompt_Sprite(player, map, index);
				if (!keyboardMouseFallback)
				{
					return sprite;
				}
				if (sprite == null)
				{
					return Controls.MapGetKeyboardPrompt_Sprite(player, map, index);
				}
				return sprite;
			}
			case Controls.InputKind.Joystick:
				return Controls.MapGetJoystickPrompt_Sprite(player, map, index);
			default:
				Debug.LogError("Can't get last prompt sprite. The last input kind is apparently invalid. We reached the default state of the switch");
				return null;
			}
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x0004FF14 File Offset: 0x0004E114
		public static Sprite MapGetLastPrompt_Sprite(Controls.PlayerExt player, Controls.InputAction action, bool keyboardMouseFallback, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(player, action);
			return Controls.MapGetLastPrompt_Sprite(player, inputActionMap, keyboardMouseFallback, index);
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x0004FF32 File Offset: 0x0004E132
		public static Sprite MapGetLastPrompt_Sprite(int playerIndex, Controls.InputActionMap map, bool keyboardMouseFallback, int index = 0)
		{
			return Controls.MapGetLastPrompt_Sprite(Controls.GetPlayerByIndex(playerIndex), map, keyboardMouseFallback, index);
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x0004FF42 File Offset: 0x0004E142
		public static Sprite MapGetLastPrompt_Sprite(int playerIndex, Controls.InputAction action, bool keyboardMouseFallback, int index = 0)
		{
			return Controls.MapGetLastPrompt_Sprite(Controls.GetPlayerByIndex(playerIndex), action, keyboardMouseFallback, index);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0004FF54 File Offset: 0x0004E154
		public static string MapGetKeyboardPrompt_TextSprite(int playerIndex, Controls.InputActionMap map, int index = 0)
		{
			List<Controls.KeyboardElement> list = Controls.MapGetKeyboardInputs(playerIndex, map);
			if (list == null)
			{
				return null;
			}
			if (index < 0 || index >= list.Count)
			{
				return null;
			}
			return PromptsMaster.GetSpriteString_Keyboard(list[index]);
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x0004FF89 File Offset: 0x0004E189
		public static string MapGetKeyboardPrompt_TextSprite(Controls.PlayerExt player, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetKeyboardPrompt_TextSprite(Controls.GetPlayerIndex(player), map, index);
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x0004FF98 File Offset: 0x0004E198
		public static string MapGetKeyboardPrompt_TextSprite(int playerIndex, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetKeyboardPrompt_TextSprite(playerIndex, inputActionMap, index);
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x0004FFB5 File Offset: 0x0004E1B5
		public static string MapGetKeyboardPrompt_TextSprite(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetKeyboardPrompt_TextSprite(Controls.GetPlayerIndex(player), action, index);
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x0004FFC4 File Offset: 0x0004E1C4
		public static string MapGetMousePrompt_TextSprite(int playerIndex, Controls.InputActionMap map, int index = 0)
		{
			List<Controls.MouseElement> list = Controls.MapGetMouseInputs(playerIndex, map);
			if (list == null)
			{
				return null;
			}
			if (index < 0 || index >= list.Count)
			{
				return null;
			}
			return PromptsMaster.GetSpriteString_Mouse(list[index]);
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x0004FFF9 File Offset: 0x0004E1F9
		public static string MapGetMousePrompt_TextSprite(Controls.PlayerExt player, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetMousePrompt_TextSprite(Controls.GetPlayerIndex(player), map, index);
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x00050008 File Offset: 0x0004E208
		public static string MapGetMousePrompt_TextSprite(int playerIndex, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetMousePrompt_TextSprite(playerIndex, inputActionMap, index);
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x00050025 File Offset: 0x0004E225
		public static string MapGetMousePrompt_TextSprite(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetMousePrompt_TextSprite(Controls.GetPlayerIndex(player), action, index);
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x00050034 File Offset: 0x0004E234
		public static string MapGetJoystickPrompt_TextSprite(int playerIndex, Controls.InputActionMap map, int index = 0)
		{
			List<Controls.JoystickElement> list = Controls.MapGetJoystickInputs(playerIndex, map);
			if (list == null)
			{
				return null;
			}
			if (index < 0 || index >= list.Count)
			{
				return null;
			}
			return PromptsMaster.GetSpriteString_Joystick(list[index]);
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x00050069 File Offset: 0x0004E269
		public static string MapGetJoystickPrompt_TextSprite(Controls.PlayerExt player, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetJoystickPrompt_TextSprite(Controls.GetPlayerIndex(player), map, index);
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00050078 File Offset: 0x0004E278
		public static string MapGetJoystickPrompt_TextSprite(int playerIndex, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetJoystickPrompt_TextSprite(playerIndex, inputActionMap, index);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x00050095 File Offset: 0x0004E295
		public static string MapGetJoystickPrompt_TextSprite(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetJoystickPrompt_TextSprite(Controls.GetPlayerIndex(player), action, index);
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x000500A4 File Offset: 0x0004E2A4
		public static string MapGetLastPrompt_TextSprite(Controls.PlayerExt player, Controls.InputActionMap map, bool keyboardMouseFallback, int index = 0)
		{
			switch (player.lastInputKindUsed)
			{
			case Controls.InputKind.Noone:
				Debug.LogError("Can't get last prompt text sprite. The last input kind is noone");
				return null;
			case Controls.InputKind.Keyboard:
			{
				string text = Controls.MapGetKeyboardPrompt_TextSprite(player, map, index);
				if (!keyboardMouseFallback)
				{
					return text;
				}
				if (string.IsNullOrEmpty(text))
				{
					return Controls.MapGetMousePrompt_TextSprite(player, map, index);
				}
				return text;
			}
			case Controls.InputKind.Mouse:
			{
				string text = Controls.MapGetMousePrompt_TextSprite(player, map, index);
				if (!keyboardMouseFallback)
				{
					return text;
				}
				if (string.IsNullOrEmpty(text))
				{
					return Controls.MapGetKeyboardPrompt_TextSprite(player, map, index);
				}
				return text;
			}
			case Controls.InputKind.Joystick:
				return Controls.MapGetJoystickPrompt_TextSprite(player, map, index);
			default:
				Debug.LogError("Can't get last prompt text sprite. The last input kind is apparently invalid. We reached the default state of the switch");
				return null;
			}
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x00050134 File Offset: 0x0004E334
		public static string MapGetLastPrompt_TextSprite(Controls.PlayerExt player, Controls.InputAction action, bool keyboardMouseFallback, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(player, action);
			return Controls.MapGetLastPrompt_TextSprite(player, inputActionMap, keyboardMouseFallback, index);
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x00050152 File Offset: 0x0004E352
		public static string MapGetLastPrompt_TextSprite(int playerIndex, Controls.InputActionMap map, bool keyboardMouseFallback, int index = 0)
		{
			return Controls.MapGetLastPrompt_TextSprite(Controls.GetPlayerByIndex(playerIndex), map, keyboardMouseFallback, index);
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x00050162 File Offset: 0x0004E362
		public static string MapGetLastPrompt_TextSprite(int playerIndex, Controls.InputAction action, bool keyboardMouseFallback, int index = 0)
		{
			return Controls.MapGetLastPrompt_TextSprite(Controls.GetPlayerByIndex(playerIndex), action, keyboardMouseFallback, index);
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x00050174 File Offset: 0x0004E374
		public static string MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(Controls.PlayerExt player, Controls.InputActionMap map, int index = 0)
		{
			switch (player.lastInputKindUsed)
			{
			case Controls.InputKind.Noone:
				Debug.LogError("Can't get last prompt text sprite. The last input kind is noone");
				return null;
			case Controls.InputKind.Keyboard:
			{
				string text = Controls.MapGetMousePrompt_TextSprite(player, map, index);
				if (string.IsNullOrEmpty(text))
				{
					return Controls.MapGetKeyboardPrompt_TextSprite(player, map, index);
				}
				return text;
			}
			case Controls.InputKind.Mouse:
			{
				string text = Controls.MapGetMousePrompt_TextSprite(player, map, index);
				if (string.IsNullOrEmpty(text))
				{
					return Controls.MapGetKeyboardPrompt_TextSprite(player, map, index);
				}
				return text;
			}
			case Controls.InputKind.Joystick:
				return Controls.MapGetJoystickPrompt_TextSprite(player, map, index);
			default:
				Debug.LogError("Can't get last prompt text sprite. The last input kind is apparently invalid. We reached the default state of the switch");
				return null;
			}
		}

		// Token: 0x06000C3B RID: 3131 RVA: 0x000501FC File Offset: 0x0004E3FC
		public static string MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(player, action);
			return Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(player, inputActionMap, index);
		}

		// Token: 0x06000C3C RID: 3132 RVA: 0x00050219 File Offset: 0x0004E419
		public static string MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(int playerIndex, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(Controls.GetPlayerByIndex(playerIndex), map, index);
		}

		// Token: 0x06000C3D RID: 3133 RVA: 0x00050228 File Offset: 0x0004E428
		public static string MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(int playerIndex, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(Controls.GetPlayerByIndex(playerIndex), action, index);
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x00050238 File Offset: 0x0004E438
		public static string MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(Controls.PlayerExt player, Controls.InputActionMap map, int index = 0)
		{
			switch (player.lastInputKindUsed)
			{
			case Controls.InputKind.Noone:
				Debug.LogError("Can't get last prompt text sprite. The last input kind is noone");
				return null;
			case Controls.InputKind.Keyboard:
			{
				string text = Controls.MapGetKeyboardPrompt_TextSprite(player, map, index);
				if (string.IsNullOrEmpty(text))
				{
					return Controls.MapGetMousePrompt_TextSprite(player, map, index);
				}
				return text;
			}
			case Controls.InputKind.Mouse:
			{
				string text = Controls.MapGetKeyboardPrompt_TextSprite(player, map, index);
				if (string.IsNullOrEmpty(text))
				{
					return Controls.MapGetMousePrompt_TextSprite(player, map, index);
				}
				return text;
			}
			case Controls.InputKind.Joystick:
				return Controls.MapGetJoystickPrompt_TextSprite(player, map, index);
			default:
				Debug.LogError("Can't get last prompt text sprite. The last input kind is apparently invalid. We reached the default state of the switch");
				return null;
			}
		}

		// Token: 0x06000C3F RID: 3135 RVA: 0x000502C0 File Offset: 0x0004E4C0
		public static string MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(player, action);
			return Controls.MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(player, inputActionMap, index);
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x000502DD File Offset: 0x0004E4DD
		public static string MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(int playerIndex, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(Controls.GetPlayerByIndex(playerIndex), map, index);
		}

		// Token: 0x06000C41 RID: 3137 RVA: 0x000502EC File Offset: 0x0004E4EC
		public static string MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(int playerIndex, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(Controls.GetPlayerByIndex(playerIndex), action, index);
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x000502FC File Offset: 0x0004E4FC
		private void CallbacksInit()
		{
			Controls.onRemap_End_Success = (Controls.MapCallback)Delegate.Combine(Controls.onRemap_End_Success, new Controls.MapCallback(delegate(Controls.InputActionMap map)
			{
				Controls.MapCallback mapCallback = Controls.onPromptsUpdateRequest;
				if (mapCallback == null)
				{
					return;
				}
				mapCallback(map);
			}));
			Controls.onRemap_InputAdded = (Controls.MapCallback)Delegate.Combine(Controls.onRemap_InputAdded, new Controls.MapCallback(delegate(Controls.InputActionMap map)
			{
				Controls.MapCallback mapCallback2 = Controls.onPromptsUpdateRequest;
				if (mapCallback2 == null)
				{
					return;
				}
				mapCallback2(map);
			}));
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00050370 File Offset: 0x0004E570
		private void GameMapsInit()
		{
			for (int i = 0; i < 1; i++)
			{
				Controls.PlayerMapCollection playerMapCollection = Controls.mapsPerPlayerCollection_InUse[i];
				Controls.MenuMapsRedefine(true, i, playerMapCollection, true, true, true);
				Controls.GameMapsRedefine(true, i, playerMapCollection, true, true, true);
			}
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x000503A8 File Offset: 0x0004E5A8
		private static void MenuMapsRedefine(bool generateNew, int playerIndex, Controls.PlayerMapCollection USE_THIS_COLLECTION, bool redefineKeyboard, bool redefineMouse, bool redefineJoystick)
		{
			Controls.InputActionMap inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.menuMoveUp, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.menuMoveUp));
			inputActionMap.MapChangeableSet(false);
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardElement.UpArrow);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.DPadUp);
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.LeftStickY);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.menuMoveDown, Controls.InputActionRange.negative, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.menuMoveDown));
			inputActionMap.MapChangeableSet(false);
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardElement.DownArrow);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.DPadDown);
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.LeftStickY);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.menuMoveRight, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.menuMoveRight));
			inputActionMap.MapChangeableSet(false);
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardElement.RightArrow);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.DPadRight);
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.LeftStickX);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.menuMoveLeft, Controls.InputActionRange.negative, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.menuMoveLeft));
			inputActionMap.MapChangeableSet(false);
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardElement.LeftArrow);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.DPadLeft);
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.LeftStickX);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.menuSelect, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.menuSelect));
			inputActionMap.MapChangeableSet(false);
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardElement.Space);
			}
			if (redefineMouse)
			{
				inputActionMap.ElementMouse_Clear();
				inputActionMap.ElementMouse_Add(Controls.MouseElement.LeftButton);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickSelectionButton_GetByPlatform());
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.menuSelectNoMouse, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.menuSelectNoMouse));
			inputActionMap.MapChangeableSet(false);
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardElement.Space);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickSelectionButton_GetByPlatform());
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.menuBack, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.menuBack));
			inputActionMap.MapChangeableSet(false);
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardElement.Backspace);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickBackButton_GetByPlatform());
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.menuAnswerYes, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.menuAnswerYes));
			inputActionMap.MapChangeableSet(false);
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
			}
			if (redefineMouse)
			{
				inputActionMap.ElementMouse_Clear();
				inputActionMap.ElementMouse_Add(Controls.MouseElement.LeftButton);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickSelectionButton_GetByPlatform());
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.menuAnswerNo, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.menuAnswerNo));
			inputActionMap.MapChangeableSet(false);
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
			}
			if (redefineMouse)
			{
				inputActionMap.ElementMouse_Clear();
				inputActionMap.ElementMouse_Add(Controls.MouseElement.RightButton);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickBackButton_GetByPlatform());
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.menuPause, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.menuPause));
			inputActionMap.MapChangeableSet(false);
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.Start);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.menuTabLeft, Controls.InputActionRange.negative, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.menuTabLeft));
			inputActionMap.MapChangeableSet(false);
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.LeftShoulder);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.menuTabRight, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.menuTabRight));
			inputActionMap.MapChangeableSet(false);
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.RightShoulder);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.menuSocialButton, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.menuSocialButton));
			inputActionMap.MapChangeableSet(false);
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.Select);
			}
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00050840 File Offset: 0x0004EA40
		private static void GameMapsRedefine(bool generateNew, int playerIndex, Controls.PlayerMapCollection USE_THIS_COLLECTION, bool redefineKeyboard, bool redefineMouse, bool redefineJoystick)
		{
			Controls.InputActionMap inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.cameraUp, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.cameraUp));
			if (redefineMouse)
			{
				inputActionMap.ElementMouse_Clear();
				inputActionMap.ElementMouse_Add(Controls.MouseElement.axisY);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.RightStickY);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.cameraDown, Controls.InputActionRange.negative, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.cameraDown));
			if (redefineMouse)
			{
				inputActionMap.ElementMouse_Clear();
				inputActionMap.ElementMouse_Add(Controls.MouseElement.axisY);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.RightStickY);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.cameraLeft, Controls.InputActionRange.negative, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.cameraLeft));
			if (redefineMouse)
			{
				inputActionMap.ElementMouse_Clear();
				inputActionMap.ElementMouse_Add(Controls.MouseElement.axisX);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.RightStickX);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.cameraRight, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.cameraRight));
			if (redefineMouse)
			{
				inputActionMap.ElementMouse_Clear();
				inputActionMap.ElementMouse_Add(Controls.MouseElement.axisX);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.RightStickX);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.scrollUp, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.scrollUp));
			if (redefineMouse)
			{
				inputActionMap.ElementMouse_Clear();
				inputActionMap.ElementMouse_Add(Controls.MouseElement.axisScrollWheelVertical);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.RightShoulder);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.scrollDown, Controls.InputActionRange.negative, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.scrollDown));
			if (redefineMouse)
			{
				inputActionMap.ElementMouse_Clear();
				inputActionMap.ElementMouse_Add(Controls.MouseElement.axisScrollWheelVertical);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.LeftShoulder);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.interact, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.interact));
			if (redefineMouse)
			{
				inputActionMap.ElementMouse_Clear();
				inputActionMap.ElementMouse_Add(Controls.MouseElement.LeftButton);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickSelectionButton_GetByPlatform());
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.RightTrigger);
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.LeftTrigger);
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.ButtonLeft);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.moveUp, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.moveUp));
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardElement.UpArrow);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.LeftStickY);
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.DPadUp);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.moveDown, Controls.InputActionRange.negative, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.moveDown));
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardElement.DownArrow);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.LeftStickY);
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.DPadDown);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.moveLeft, Controls.InputActionRange.negative, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.moveLeft));
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardElement.LeftArrow);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.LeftStickX);
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.DPadLeft);
			}
			inputActionMap = (generateNew ? new Controls.InputActionMap(playerIndex, Controls.InputAction.moveRight, Controls.InputActionRange.positive, USE_THIS_COLLECTION, false) : Controls.MapFind_InUse(playerIndex, Controls.InputAction.moveRight));
			if (redefineKeyboard)
			{
				inputActionMap.ElementKeyboard_Clear();
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardLayoutGetElement(inputActionMap.myGameAction));
				inputActionMap.ElementKeyboard_Add(Controls.KeyboardElement.RightArrow);
			}
			if (redefineJoystick)
			{
				inputActionMap.ElementJoystick_Clear();
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.LeftStickX);
				inputActionMap.ElementJoystick_Add(Controls.JoystickElement.DPadRight);
			}
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00050B80 File Offset: 0x0004ED80
		public static Controls.KeyboardElement KeyboardLayoutGetElement(Controls.InputAction action)
		{
			Data.SettingsData.KeyboardLayout keyboardLayout = Data.SettingsData.KeyboardLayout.keyboard_QWERTY;
			if (Data.settings != null)
			{
				keyboardLayout = Data.settings.KeyboardLayoutGet();
			}
			string text;
			switch (action)
			{
			case Controls.InputAction.menuMoveUp:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.W;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.Z;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.Comma;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.W;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.menuMoveDown:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.S;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.S;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.O;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.R;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.menuMoveRight:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.D;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.D;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.E;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.S;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.menuMoveLeft:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.A;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.Q;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.A;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.A;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.menuSelect:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.Return;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.Return;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.Return;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.Return;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.menuSelectNoMouse:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.Return;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.Return;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.Return;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.Return;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.menuBack:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.Esc;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.Esc;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.Esc;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.Esc;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.menuAnswerYes:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.Return;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.Return;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.Return;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.Return;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.menuAnswerNo:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.Esc;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.Esc;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.Esc;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.Esc;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.menuPause:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.Esc;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.Esc;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.Esc;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.Esc;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.menuTabLeft:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.Q;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.A;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.SingleQuote;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.Q;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.menuTabRight:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.E;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.E;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.Dot;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.F;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.menuSocialButton:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.T;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.T;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.T;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.T;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.moveUp:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.W;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.Z;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.Comma;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.W;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.moveDown:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.S;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.S;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.O;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.R;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.moveLeft:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.A;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.Q;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.A;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.A;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			case Controls.InputAction.moveRight:
				switch (keyboardLayout)
				{
				case Data.SettingsData.KeyboardLayout.keyboard_QWERTY:
					return Controls.KeyboardElement.D;
				case Data.SettingsData.KeyboardLayout.keyboard_AZERTY:
					return Controls.KeyboardElement.D;
				case Data.SettingsData.KeyboardLayout.keyboard_DVORAK:
					return Controls.KeyboardElement.E;
				case Data.SettingsData.KeyboardLayout.keyboard_COLEMAK:
					return Controls.KeyboardElement.S;
				default:
					text = "Controls.KeyboardLayoutGetElement(): layout not handled: " + keyboardLayout.ToString() + " - for action: " + action.ToString();
					goto IL_0606;
				}
				break;
			}
			text = "Controls.KeyboardLayoutGetElement(): action not handled: " + action.ToString();
			IL_0606:
			if (!string.IsNullOrEmpty(text))
			{
				Debug.LogWarning(text);
			}
			return Controls.KeyboardElement.Undefined;
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x000511A8 File Offset: 0x0004F3A8
		public static void KeyboardLayoutNext()
		{
			Data.settings.KeyboardLayourNext();
			for (int i = 0; i < 1; i++)
			{
				Controls.PlayerMapCollection playerMapCollection = Controls.mapsPerPlayerCollection_InUse[i];
				Controls.MenuMapsRedefine(false, i, playerMapCollection, true, false, false);
				Controls.GameMapsRedefine(false, i, playerMapCollection, true, false, false);
			}
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x000511EC File Offset: 0x0004F3EC
		public static void KeyboardLayoutPrevious()
		{
			Data.settings.KeyboardLayourPrevious();
			for (int i = 0; i < 1; i++)
			{
				Controls.PlayerMapCollection playerMapCollection = Controls.mapsPerPlayerCollection_InUse[i];
				Controls.MenuMapsRedefine(false, i, playerMapCollection, true, false, false);
				Controls.GameMapsRedefine(false, i, playerMapCollection, true, false, false);
			}
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00051230 File Offset: 0x0004F430
		public static bool ActionButton_PressedGet(Controls.PlayerExt player, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			if (player == null)
			{
				return false;
			}
			if (ignoreIfNotPlaying && !player.isPlaying)
			{
				return false;
			}
			int playerIndex = Controls.GetPlayerIndex(player);
			return Controls.playerChachedActionStates[playerIndex]._gameActionState_JustPressed.ContainsKey(action) && Controls.playerChachedActionStates[playerIndex]._gameActionState_JustPressed[action];
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0005127E File Offset: 0x0004F47E
		public static bool ActionButton_PressedGet(int playerIndex, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionButton_PressedGet(Controls.GetPlayerByIndex(playerIndex), action, ignoreIfNotPlaying);
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00051290 File Offset: 0x0004F490
		public static bool ActionButton_PressedGetAnyPlayer(Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			for (int i = 0; i < 1; i++)
			{
				if (Controls.ActionButton_PressedGet(i, action, ignoreIfNotPlaying))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x000512B8 File Offset: 0x0004F4B8
		public static bool ActionButton_ReleasedGet(Controls.PlayerExt player, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			if (player == null)
			{
				return false;
			}
			if (ignoreIfNotPlaying && !player.isPlaying)
			{
				return false;
			}
			int playerIndex = Controls.GetPlayerIndex(player);
			return Controls.playerChachedActionStates[playerIndex]._gameActionState_JustReleased.ContainsKey(action) && Controls.playerChachedActionStates[playerIndex]._gameActionState_JustReleased[action];
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00051306 File Offset: 0x0004F506
		public static bool ActionButton_ReleasedGet(int playerIndex, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionButton_ReleasedGet(Controls.GetPlayerByIndex(playerIndex), action, ignoreIfNotPlaying);
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00051318 File Offset: 0x0004F518
		public static bool ActionButton_ReleasedGetAnyPlayer(Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			for (int i = 0; i < 1; i++)
			{
				if (Controls.ActionButton_ReleasedGet(i, action, ignoreIfNotPlaying))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00051340 File Offset: 0x0004F540
		public static bool ActionButton_HoldGet(Controls.PlayerExt player, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			if (player == null)
			{
				return false;
			}
			if (ignoreIfNotPlaying && !player.isPlaying)
			{
				return false;
			}
			int playerIndex = Controls.GetPlayerIndex(player);
			return Controls.playerChachedActionStates[playerIndex]._gameActionState_Hold.ContainsKey(action) && Controls.playerChachedActionStates[playerIndex]._gameActionState_Hold[action];
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x0005138E File Offset: 0x0004F58E
		public static bool ActionButton_HoldGet(int playerIndex, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionButton_HoldGet(Controls.GetPlayerByIndex(playerIndex), action, ignoreIfNotPlaying);
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x000513A0 File Offset: 0x0004F5A0
		public static bool ActionButton_HoldGetAnyPlayer(Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			for (int i = 0; i < 1; i++)
			{
				if (Controls.ActionButton_HoldGet(i, action, ignoreIfNotPlaying))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x000513C8 File Offset: 0x0004F5C8
		public static bool ActionButton_HoldPreviousGet(Controls.PlayerExt player, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			if (player == null)
			{
				return false;
			}
			if (ignoreIfNotPlaying && !player.isPlaying)
			{
				return false;
			}
			int playerIndex = Controls.GetPlayerIndex(player);
			return Controls.playerChachedActionStates[playerIndex]._gameActionState_HoldPrevious.ContainsKey(action) && Controls.playerChachedActionStates[playerIndex]._gameActionState_HoldPrevious[action];
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00051416 File Offset: 0x0004F616
		public static bool ActionButton_HoldPreviousGet(int playerIndex, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionButton_HoldPreviousGet(Controls.GetPlayerByIndex(playerIndex), action, ignoreIfNotPlaying);
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00051428 File Offset: 0x0004F628
		public static bool ActionButton_HoldPreviousGetAnyPlayer(Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			for (int i = 0; i < 1; i++)
			{
				if (Controls.ActionButton_HoldPreviousGet(i, action, ignoreIfNotPlaying))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00051450 File Offset: 0x0004F650
		public static float ActionAxis_GetValue(Controls.PlayerExt player, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			if (player == null)
			{
				return 0f;
			}
			if (ignoreIfNotPlaying && !player.isPlaying)
			{
				return 0f;
			}
			int playerIndex = Controls.GetPlayerIndex(player);
			if (Controls.playerChachedActionStates[playerIndex]._gameActionState_Axis.ContainsKey(action))
			{
				return Controls.playerChachedActionStates[playerIndex]._gameActionState_Axis[action];
			}
			return 0f;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x000514AA File Offset: 0x0004F6AA
		public static float ActionAxis_GetValue(int playerIndex, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionAxis_GetValue(Controls.GetPlayerByIndex(playerIndex), action, ignoreIfNotPlaying);
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x000514BC File Offset: 0x0004F6BC
		public static float ActionAxis_GetValuePrevious(Controls.PlayerExt player, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			if (player == null)
			{
				return 0f;
			}
			if (ignoreIfNotPlaying && !player.isPlaying)
			{
				return 0f;
			}
			int playerIndex = Controls.GetPlayerIndex(player);
			if (Controls.playerChachedActionStates[playerIndex]._gameActionState_AxisPrevious.ContainsKey(action))
			{
				return Controls.playerChachedActionStates[playerIndex]._gameActionState_AxisPrevious[action];
			}
			return 0f;
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00051516 File Offset: 0x0004F716
		public static float ActionAxis_GetValuePrevious(int playerIndex, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionAxis_GetValuePrevious(Controls.GetPlayerByIndex(playerIndex), action, ignoreIfNotPlaying);
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x00051528 File Offset: 0x0004F728
		public static float ActionAxisPair_GetValue(Controls.PlayerExt player, Controls.InputAction actionPlus, Controls.InputAction actionMinus, bool ignoreIfNotPlaying = true)
		{
			if (player == null)
			{
				return 0f;
			}
			if (ignoreIfNotPlaying && !player.isPlaying)
			{
				return 0f;
			}
			float num = Controls.ActionAxis_GetValue(player, actionPlus, ignoreIfNotPlaying);
			float num2 = Controls.ActionAxis_GetValue(player, actionMinus, ignoreIfNotPlaying);
			return num + num2;
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00051562 File Offset: 0x0004F762
		public static float ActionAxisPair_GetValue(int playerIndex, Controls.InputAction actionPlus, Controls.InputAction actionMinus, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionAxisPair_GetValue(Controls.GetPlayerByIndex(playerIndex), actionPlus, actionMinus, ignoreIfNotPlaying);
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00051574 File Offset: 0x0004F774
		public static float ActionAxisPair_GetValuePrevious(Controls.PlayerExt player, Controls.InputAction actionPlus, Controls.InputAction actionMinus, bool ignoreIfNotPlaying = true)
		{
			if (player == null)
			{
				return 0f;
			}
			if (ignoreIfNotPlaying && !player.isPlaying)
			{
				return 0f;
			}
			float num = Controls.ActionAxis_GetValuePrevious(player, actionPlus, ignoreIfNotPlaying);
			float num2 = Controls.ActionAxis_GetValuePrevious(player, actionMinus, ignoreIfNotPlaying);
			return num + num2;
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x000515AE File Offset: 0x0004F7AE
		public static float ActionAxisPair_GetValuePrevious(int playerIndex, Controls.InputAction actionPlus, Controls.InputAction actionMinus, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionAxisPair_GetValuePrevious(Controls.GetPlayerByIndex(playerIndex), actionPlus, actionMinus, ignoreIfNotPlaying);
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x000515BE File Offset: 0x0004F7BE
		public static bool MenuDirectionalAny_PressedGet(int playerIndex)
		{
			return Controls.ActionButton_PressedGet(playerIndex, Controls.InputAction.menuMoveUp, true) || Controls.ActionButton_PressedGet(playerIndex, Controls.InputAction.menuMoveDown, true) || Controls.ActionButton_PressedGet(playerIndex, Controls.InputAction.menuMoveRight, true) || Controls.ActionButton_PressedGet(playerIndex, Controls.InputAction.menuMoveLeft, true);
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x000515E8 File Offset: 0x0004F7E8
		private void Awake()
		{
			if (Controls.instance != null)
			{
				Object.Destroy(this);
				return;
			}
			Controls.instance = this;
			this.PlayersInit();
			this.GameControlsInit();
			this.CallbacksInit();
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00051616 File Offset: 0x0004F816
		private void Update()
		{
			this.PlayersUpdate();
			this.SplitScreenUpdate();
			this.VibrationUpdate();
			this.InputActionsUpdate();
			this._RemapContextUpdate();
		}

		public static Controls instance = null;

		public const float VIBRATION_MENU_HOVER = 0.1f;

		public const float VIBRATION_MENU_SELECT = 0.25f;

		public const float VIBRATION_MENU_SELECT_STRONG = 0.5f;

		public const float VIBRATION_SLOT_KATLAK = 0.25f;

		public const float VIBRATION_SLOT_SCORE = 0.25f;

		public const float VIBRATION_SLOT_SCORE_666 = 0.5f;

		public const float VIBRATION_SLOT_SCORE_999 = 0.5f;

		public const float VIBRATION_SLOT_FINALIZE = 0.25f;

		public const float VIBRATION_MOMENT_IMPACT = 0.75f;

		public const float VIBRATION_MOMENT_IMPACT_STRONG = 1f;

		private const bool PRINT_ELEMENT_IDENTIFIERS = false;

		public const int MAX_PLAYERS_SUPPORTED = 1;

		public Controls.InputKind PLAYER_DESKTOP_PREFERRED_INPUT = Controls.InputKind.Mouse;

		public const bool KEYBOARD_AND_MOUSE_ESSENTIAL_TOGHETHER = true;

		public const bool MOUSE_MOVEMENT_SWITCHES_LAST_INPUT_DEFAULT = true;

		public const bool ALLOW_MOUSE_INPUT = true;

		public const bool ALLOW_KEYBOARD_INPUT = true;

		public const bool ALLOW_JOYSTICK_INPUT = true;

		public const float MOUSE_MOVEMENT_SENSITIVITY_MULT_X = 0.5f;

		public const float MOUSE_MOVEMENT_SENSITIVITY_MULT_Y = 0.5f;

		private const float VIBRATION_DECAY_SPEED_DEFAULT = 4f;

		private const float MOUSE_INPUT_CHANGE_DEADZONE = 0.1f;

		private const float MOUSE_GAME_DEADZONE = 0f;

		private const float JOYSTICK_GAME_DEADZONE = 0.1f;

		public const float JOYSTICK_MENU_DEADZONE = 0.35f;

		private const float AXIS_REMAP_DEADZONE = 0.8f;

		private const int MAX_MAPS_PER_PLAYER = 40;

		private const int MAX_INPUTS_PER_MAP_INPUT_KIND = 4;

		private static bool[] mouseMovementSwitchesLastInput = null;

		private static List<Controls.PlayerExt> playersPool = new List<Controls.PlayerExt>();

		public static List<Controls.PlayerExt> playersExtList = new List<Controls.PlayerExt>();

		public static Controls.PlayerExt p1 = null;

		private bool controllerNotSupported_Showed;

		public static int maxPlayingPlayers = 4;

		private static bool playersCanJoin = false;

		private static List<Controls.PlayerExt> playersPlaying = new List<Controls.PlayerExt>();

		private Dictionary<Controls.PlayerExt, Controls.VibrationData> vibrations = new Dictionary<Controls.PlayerExt, Controls.VibrationData>();

		private const int MOUSE_BUTTON_LEFT = 0;

		private const int MOUSE_BUTTON_RIGHT = 1;

		private const int MOUSE_BUTTON_MIDDLE = 2;

		private const int MOUSE_AXIS_SCROLLWHEEL_VERTICAL = 2;

		private const int MOUSE_AXIS_SCROLLWHEEL_HORIZONTAL = 3;

		private const int MOUSE_AXIS_X = 0;

		private const int MOUSE_AXIS_Y = 1;

		private static Controls.InputActionMap undefinedGameActionMap = new Controls.InputActionMap(-1, Controls.InputAction._UNDEFINED, Controls.InputActionRange.positive, null, false);

		public static Controls.PlayerMapCollection[] mapsPerPlayerCollection_InUse = new Controls.PlayerMapCollection[1];

		public static Controls.PlayerMapCollection[] mapsPerPlayerCollection_Default = new Controls.PlayerMapCollection[1];

		public static Controls.PlayerChachedActionStates[] playerChachedActionStates = new Controls.PlayerChachedActionStates[1];

		private static bool systemReady = false;

		public static Controls.PlayerMapCollectionSerializer mapsCollectionSerializer = new Controls.PlayerMapCollectionSerializer();

		private static Controls.KeyboardElement[] bannedElements_Keyboard = new Controls.KeyboardElement[]
		{
			Controls.KeyboardElement.Esc,
			Controls.KeyboardElement.Backspace,
			Controls.KeyboardElement.Return,
			Controls.KeyboardElement.LeftWindows,
			Controls.KeyboardElement.RightWindows,
			Controls.KeyboardElement.LeftCommand,
			Controls.KeyboardElement.RightCommand
		};

		private static Controls.MouseElement[] bannedElements_Mouse = new Controls.MouseElement[0];

		private static Controls.JoystickElement[] bannedElements_Joystick = new Controls.JoystickElement[]
		{
			Controls.JoystickElement.Select,
			Controls.JoystickElement.Start,
			Controls.JoystickElement.Home
		};

		private static Dictionary<Controls.KeyboardElement, bool> bannedElementsDict_Keyboard = null;

		private static Dictionary<Controls.MouseElement, bool> bannedElementsDict_Mouse = null;

		private static Dictionary<Controls.JoystickElement, bool> bannedElementsDict_Joystick = null;

		private static Controls.RemappingContext remappingContext = new Controls.RemappingContext();

		public static Controls.MapCallback onRemap_Start;

		public static Controls.MapCallback onRemap_InputAdded;

		public static Controls.MapCallback onRemap_End_Success;

		public static Controls.MapCallback onRemap_End_Abort;

		public static Controls.MapCallback onRemap_End_Generic;

		public static Controls.MapCallback onLastInputKindChangedAny;

		public static Controls.MapCallback onPromptsUpdateRequest;

		public enum InputKind
		{
			Noone,
			Keyboard,
			Mouse,
			Joystick
		}

		public class PlayerExt
		{
			public Player rePlayer;

			public bool isPlaying;

			public Controls.InputKind lastInputKindUsed;

			public Controls.InputKind lastInputKindUsedOld;

			public int lastUsedJoystickIndex = -1;

			public Joystick lastJoystickUsed;

			public IGamepadTemplate lastUsedJoystickTemplate;
		}

		private class VibrationData
		{
			// Token: 0x06001296 RID: 4758 RVA: 0x000769E0 File Offset: 0x00074BE0
			public VibrationData(Controls.PlayerExt myPlayer)
			{
				this.myPlayer = myPlayer;
				this.motorLevelLeft = 0f;
				this.motorLevelRight = 0f;
				this.decaySpeedLeft = 4f;
				this.decaySpeedRight = 4f;
				this.decaySpeedMult = 1f;
				this.pausable = true;
			}

			public Controls.PlayerExt myPlayer;

			public float motorLevelLeft;

			public float motorLevelRight;

			public float decaySpeedLeft;

			public float decaySpeedRight;

			public float decaySpeedMult;

			public bool pausable;
		}

		public enum JoystickElement
		{
			ButtonDown,
			ButtonRight,
			ButtonLeft,
			ButtonUp,
			Start,
			Select,
			Home,
			LeftStickButton,
			RightStickButton,
			LeftShoulder,
			RightShoulder,
			DPadUp,
			DPadDown,
			DPadLeft,
			DPadRight,
			LeftStickX,
			LeftStickY,
			RightStickX,
			RightStickY,
			LeftTrigger,
			RightTrigger,
			Count,
			Undefined
		}

		public enum MouseElement
		{
			LeftButton,
			RightButton,
			MiddleButton,
			axisScrollWheelVertical,
			axisScrollWheelHorizontal,
			axisX,
			axisY,
			Count,
			Undefined
		}

		public enum KeyboardElement
		{
			None,
			A,
			B,
			C,
			D,
			E,
			F,
			G,
			H,
			I,
			J,
			K,
			L,
			M,
			N,
			O,
			P,
			Q,
			R,
			S,
			T,
			U,
			V,
			W,
			X,
			Y,
			Z,
			Zero,
			One,
			Two,
			Three,
			Four,
			Five,
			Six,
			Seven,
			Eight,
			Nine,
			Keypad_0,
			Keypad_1,
			Keypad_2,
			Keypad_3,
			Keypad_4,
			Keypad_5,
			Keypad_6,
			Keypad_7,
			Keypad_8,
			Keypad_9,
			Keypad_Dot,
			Keypad_Slash,
			Keypad_Asterisk,
			Keypad_Minus,
			Keypad_Plus,
			Keypad_Enter,
			Keypad_Equals,
			Space,
			Backspace,
			Tab,
			Clear,
			Return,
			Pause,
			Esc,
			ExclamationMark,
			DoubleQuote,
			Hash,
			Dollar,
			Ampersand,
			SingleQuote,
			OpenParenthesis,
			CloseParenthesis,
			Asterisk,
			Plus,
			Comma,
			Minus,
			Dot,
			Slash,
			Colon,
			Semicolon,
			LessThan,
			Equals,
			GreaterThan,
			QuestionMark,
			At,
			OpenBracket,
			Backslash,
			CloseBracket,
			Caret,
			Underscore,
			BackQuote,
			Delete,
			UpArrow,
			DownArrow,
			RightArrow,
			LeftArrow,
			Insert,
			Home,
			End,
			PageUp,
			PageDown,
			F1,
			F2,
			F3,
			F4,
			F5,
			F6,
			F7,
			F8,
			F9,
			F10,
			F11,
			F12,
			F13,
			F14,
			F15,
			Numlock,
			CapsLock,
			ScrollLock,
			RightShift,
			LeftShift,
			RightControl,
			LeftControl,
			RightAlt,
			LeftAlt,
			RightCommand,
			LeftCommand,
			LeftWindows,
			RightWindows,
			AltGr,
			Help,
			Print,
			SysReq,
			Break,
			Menu,
			Count,
			Undefined
		}

		public enum InputActionRange
		{
			positive,
			negative
		}

		[Serializable]
		public class InputActionMap
		{
			// Token: 0x06001297 RID: 4759 RVA: 0x00076A38 File Offset: 0x00074C38
			public InputActionMap(int playerIndex, Controls.InputAction action, Controls.InputActionRange range, Controls.PlayerMapCollection playerMapCollection, bool updateIfNotPlaying)
			{
				this.ConstructorInitialization(playerIndex, action, range, playerMapCollection, updateIfNotPlaying);
			}

			// Token: 0x06001298 RID: 4760 RVA: 0x00076AB3 File Offset: 0x00074CB3
			private void ConstructorInitialization(int playerIndex, Controls.InputAction action, Controls.InputActionRange range, Controls.PlayerMapCollection playerMapCollection, bool updateIfNotPlaying)
			{
				this.myPlayerIndex = playerIndex;
				this.updateIfNotPlaying = updateIfNotPlaying;
				this.myGameAction = action;
				this.myGameActionRange = range;
				this.ElementKeyboard_Clear();
				this.ElementMouse_Clear();
				this.ElementJoystick_Clear();
				this.MapCollectionAssign(playerMapCollection);
			}

			// Token: 0x06001299 RID: 4761 RVA: 0x00076AEC File Offset: 0x00074CEC
			~InputActionMap()
			{
				this.MapCollectionRemoveFrom(this.myPlayerMapCollection);
			}

			// Token: 0x0600129A RID: 4762 RVA: 0x00076B20 File Offset: 0x00074D20
			public Controls.InputActionMap Copy(Controls.PlayerMapCollection newTargetPlayerCollection)
			{
				Controls.InputActionMap inputActionMap = new Controls.InputActionMap(this.myPlayerIndex, this.myGameAction, this.myGameActionRange, newTargetPlayerCollection, this.updateIfNotPlaying);
				for (int i = 0; i < 4; i++)
				{
					inputActionMap.keyboardElements[i] = this.keyboardElements[i];
					inputActionMap.keyboardElementsCount = this.keyboardElementsCount;
					inputActionMap.mouseElements[i] = this.mouseElements[i];
					inputActionMap.mouseElementsCount = this.mouseElementsCount;
					inputActionMap.joystickElements[i] = this.joystickElements[i];
					inputActionMap.joystickElementsCount = this.joystickElementsCount;
				}
				return inputActionMap;
			}

			// Token: 0x0600129B RID: 4763 RVA: 0x00076BB0 File Offset: 0x00074DB0
			public void CopyTo(Controls.InputActionMap targetMap, bool affectKeyboard, bool affectMouse, bool affectJoystick)
			{
				targetMap.updateIfNotPlaying = this.updateIfNotPlaying;
				targetMap.myGameAction = this.myGameAction;
				targetMap.myGameActionRange = this.myGameActionRange;
				for (int i = 0; i < 4; i++)
				{
					if (affectKeyboard)
					{
						targetMap.keyboardElements[i] = this.keyboardElements[i];
						targetMap.keyboardElementsCount = this.keyboardElementsCount;
					}
					if (affectMouse)
					{
						targetMap.mouseElements[i] = this.mouseElements[i];
						targetMap.mouseElementsCount = this.mouseElementsCount;
					}
					if (affectJoystick)
					{
						targetMap.joystickElements[i] = this.joystickElements[i];
						targetMap.joystickElementsCount = this.joystickElementsCount;
					}
				}
			}

			// Token: 0x0600129C RID: 4764 RVA: 0x00076C4B File Offset: 0x00074E4B
			public void ClearMaps(bool affectKeyboard, bool affectMouse, bool affectJoystick)
			{
				this.ElementKeyboard_Clear();
				this.ElementMouse_Clear();
				this.ElementJoystick_Clear();
			}

			// Token: 0x0600129D RID: 4765 RVA: 0x00076C60 File Offset: 0x00074E60
			public void MapCollectionAssign(Controls.PlayerMapCollection playerMapCollection)
			{
				if (this.myPlayerMapCollection != null)
				{
					this.MapCollectionRemoveFrom(this.myPlayerMapCollection);
				}
				this.myPlayerMapCollection = playerMapCollection;
				if (this.myPlayerMapCollection == null)
				{
					return;
				}
				if (this.myPlayerMapCollection.mapsCount >= this.myPlayerMapCollection.maps.Length)
				{
					Debug.LogError("Can't add more maps to this player map collection. Action: " + this.myGameAction.ToString() + " - Player: " + this.myPlayerIndex.ToString());
					return;
				}
				this.myPlayerMapCollection.maps[this.myPlayerMapCollection.mapsCount] = this;
				this.myPlayerMapCollection.mapsCount++;
			}

			// Token: 0x0600129E RID: 4766 RVA: 0x00076D08 File Offset: 0x00074F08
			public void MapCollectionRemoveFrom(Controls.PlayerMapCollection playerMapCollection)
			{
				if (this.myPlayerMapCollection == null)
				{
					return;
				}
				if (this.myPlayerMapCollection.mapsCount <= 0)
				{
					return;
				}
				for (int i = 0; i < this.myPlayerMapCollection.mapsCount; i++)
				{
					if (this.myPlayerMapCollection.maps[i] == this)
					{
						this.myPlayerMapCollection.maps[i] = Controls.undefinedGameActionMap;
						this.myPlayerMapCollection.mapsCount--;
						return;
					}
				}
			}

			// (get) Token: 0x0600129F RID: 4767 RVA: 0x00076D79 File Offset: 0x00074F79
			public Controls.PlayerExt myPlayer
			{
				get
				{
					if (this.myPlayerIndex < 0)
					{
						return null;
					}
					if (this._myPlayer == null)
					{
						this._myPlayer = Controls.GetPlayerByIndex(this.myPlayerIndex);
					}
					return this._myPlayer;
				}
			}

			// Token: 0x060012A0 RID: 4768 RVA: 0x00076DA5 File Offset: 0x00074FA5
			public void MapChangeableSet(bool value)
			{
				this._mapCanBeRemapped = value;
			}

			// Token: 0x060012A1 RID: 4769 RVA: 0x00076DAE File Offset: 0x00074FAE
			public bool MapChangeableGet()
			{
				return this._mapCanBeRemapped;
			}

			// Token: 0x060012A2 RID: 4770 RVA: 0x00076DB8 File Offset: 0x00074FB8
			public bool ElementKeyboard_Add(Controls.KeyboardElement elementId)
			{
				if (this.keyboardElementsCount >= 4)
				{
					return false;
				}
				for (int i = 0; i < this.keyboardElementsCount; i++)
				{
					if (this.keyboardElements[i] == elementId)
					{
						return false;
					}
				}
				this.keyboardElements[this.keyboardElementsCount] = elementId;
				this.keyboardElementsCount++;
				return true;
			}

			// Token: 0x060012A3 RID: 4771 RVA: 0x00076E0C File Offset: 0x0007500C
			public bool ElementMouse_Add(Controls.MouseElement elementId)
			{
				if (this.mouseElementsCount >= 4)
				{
					return false;
				}
				for (int i = 0; i < this.mouseElementsCount; i++)
				{
					if (this.mouseElements[i] == elementId)
					{
						return false;
					}
				}
				this.mouseElements[this.mouseElementsCount] = elementId;
				this.mouseElementsCount++;
				return true;
			}

			// Token: 0x060012A4 RID: 4772 RVA: 0x00076E60 File Offset: 0x00075060
			public bool ElementJoystick_Add(Controls.JoystickElement elementId)
			{
				if (this.joystickElementsCount >= 4)
				{
					return false;
				}
				for (int i = 0; i < this.joystickElementsCount; i++)
				{
					if (this.joystickElements[i] == elementId)
					{
						return false;
					}
				}
				this.joystickElements[this.joystickElementsCount] = elementId;
				this.joystickElementsCount++;
				return true;
			}

			// Token: 0x060012A5 RID: 4773 RVA: 0x00076EB4 File Offset: 0x000750B4
			public void ElementKeyboard_Clear()
			{
				for (int i = 0; i < 4; i++)
				{
					this.keyboardElements[i] = (Controls.KeyboardElement)(-1);
				}
				this.keyboardElementsCount = 0;
			}

			// Token: 0x060012A6 RID: 4774 RVA: 0x00076EE0 File Offset: 0x000750E0
			public void ElementMouse_Clear()
			{
				for (int i = 0; i < 4; i++)
				{
					this.mouseElements[i] = (Controls.MouseElement)(-1);
				}
				this.mouseElementsCount = 0;
			}

			// Token: 0x060012A7 RID: 4775 RVA: 0x00076F0C File Offset: 0x0007510C
			public void ElementJoystick_Clear()
			{
				for (int i = 0; i < 4; i++)
				{
					this.joystickElements[i] = (Controls.JoystickElement)(-1);
				}
				this.joystickElementsCount = 0;
			}

			// Token: 0x060012A8 RID: 4776 RVA: 0x00076F38 File Offset: 0x00075138
			public List<Controls.KeyboardElement> ElementKeyboard_Get()
			{
				if (this.elementsListChache_Keyboard == null)
				{
					this.elementsListChache_Keyboard = new List<Controls.KeyboardElement>();
				}
				else
				{
					this.elementsListChache_Keyboard.Clear();
				}
				int num = 0;
				while (num < this.keyboardElementsCount && this.keyboardElements[num] >= Controls.KeyboardElement.None)
				{
					this.elementsListChache_Keyboard.Add(this.keyboardElements[num]);
					num++;
				}
				return this.elementsListChache_Keyboard;
			}

			// Token: 0x060012A9 RID: 4777 RVA: 0x00076F9C File Offset: 0x0007519C
			public List<Controls.MouseElement> ElementMouse_Get()
			{
				if (this.elementsListChache_Mouse == null)
				{
					this.elementsListChache_Mouse = new List<Controls.MouseElement>();
				}
				else
				{
					this.elementsListChache_Mouse.Clear();
				}
				int num = 0;
				while (num < this.mouseElementsCount && this.mouseElements[num] >= Controls.MouseElement.LeftButton)
				{
					this.elementsListChache_Mouse.Add(this.mouseElements[num]);
					num++;
				}
				return this.elementsListChache_Mouse;
			}

			// Token: 0x060012AA RID: 4778 RVA: 0x00077000 File Offset: 0x00075200
			public List<Controls.JoystickElement> ElementJoystick_Get()
			{
				if (this.elementsListChache_Joystick == null)
				{
					this.elementsListChache_Joystick = new List<Controls.JoystickElement>();
				}
				else
				{
					this.elementsListChache_Joystick.Clear();
				}
				int num = 0;
				while (num < this.joystickElementsCount && this.joystickElements[num] >= Controls.JoystickElement.ButtonDown)
				{
					this.elementsListChache_Joystick.Add(this.joystickElements[num]);
					num++;
				}
				return this.elementsListChache_Joystick;
			}

			// Token: 0x060012AB RID: 4779 RVA: 0x00077062 File Offset: 0x00075262
			public bool HasNoElements()
			{
				return this.keyboardElementsCount == 0 && this.mouseElementsCount == 0 && this.joystickElementsCount == 0;
			}

			// Token: 0x060012AC RID: 4780 RVA: 0x0007707F File Offset: 0x0007527F
			public void ResetState(bool forcePreviousValueToZero)
			{
				this.justPressd = false;
				this.justReleased = false;
				if (forcePreviousValueToZero)
				{
					this.inputValuePrevious = 0f;
				}
				else
				{
					this.inputValuePrevious = this.inputValue;
				}
				this.inputValue = 0f;
			}

			// Token: 0x060012AD RID: 4781 RVA: 0x000770B8 File Offset: 0x000752B8
			public void UpdateState()
			{
				if (this.myPlayer == null || this._updateNoPlayerErrorShown)
				{
					this._updateNoPlayerErrorShown = true;
					Debug.LogError("You shouldn't be executing maps without a player assigned to them. Action: " + this.myGameAction.ToString() + " Have you changed inputs to a new version? You should issue a reset of settings to avoid loading old input maps!");
					return;
				}
				this.ResetState(false);
				float num = (Controls.GameActionIsInverted(this.myPlayerIndex, this.myGameAction) ? (-1f) : 1f);
				float num2 = ((this.myGameActionRange == Controls.InputActionRange.positive) ? 1f : (-1f)) * num;
				if (this.myPlayer.isPlaying || this.updateIfNotPlaying)
				{
					int num3 = 0;
					while (num3 < this.keyboardElementsCount && this.myPlayer.rePlayer.controllers.hasKeyboard)
					{
						Controls.KeyboardElement keyboardElement = this.keyboardElements[num3];
						if (keyboardElement >= Controls.KeyboardElement.None && Controls.KeyboardButton_HoldGet(this.myPlayer, keyboardElement))
						{
							this.inputValue += num2;
						}
						num3++;
					}
					int num4 = 0;
					while (num4 < this.mouseElementsCount && this.myPlayer.rePlayer.controllers.hasMouse)
					{
						Controls.MouseElement mouseElement = this.mouseElements[num4];
						if (mouseElement >= Controls.MouseElement.LeftButton)
						{
							if (Controls.MouseElement_IsButton(mouseElement))
							{
								if (Controls.MouseButton_HoldGet(this.myPlayer, mouseElement))
								{
									this.inputValue += num2;
								}
							}
							else
							{
								float num5 = Controls.MouseAxis_ValueGet(this.myPlayer, mouseElement);
								if (Mathf.Abs(num5) > 0f && Mathf.Sign(num5) == Mathf.Sign(num2))
								{
									this.inputValue += num5 * num;
								}
							}
						}
						num4++;
					}
					int num6 = 0;
					while (num6 < this.joystickElementsCount && this.myPlayer.lastUsedJoystickTemplate != null)
					{
						Controls.JoystickElement joystickElement = this.joystickElements[num6];
						if (joystickElement >= Controls.JoystickElement.ButtonDown)
						{
							if (Controls.JoystickElement_IsButton(joystickElement))
							{
								if (Controls.JoystickButton_HoldGet(this.myPlayer, joystickElement))
								{
									this.inputValue += num2;
								}
							}
							else
							{
								float num7 = Controls.JoystickAxis_ValueGet(this.myPlayer, joystickElement);
								if (Mathf.Abs(num7) > 0.1f && Mathf.Sign(num7) == Mathf.Sign(num2))
								{
									this.inputValue += num7 * num;
								}
							}
						}
						num6++;
					}
				}
				if (this.inputValue != this.inputValuePrevious)
				{
					if (num2 > 0f)
					{
						if (this.inputValue > 0f && this.inputValuePrevious == 0f)
						{
							this.justPressd = true;
						}
						else if (this.inputValue == 0f && this.inputValuePrevious > 0f)
						{
							this.justReleased = true;
						}
					}
					else if (num2 < 0f)
					{
						if (this.inputValue < 0f && this.inputValuePrevious == 0f)
						{
							this.justPressd = true;
						}
						else if (this.inputValue == 0f && this.inputValuePrevious < 0f)
						{
							this.justReleased = true;
						}
					}
				}
				Controls.PlayerChachedActionStates playerChachedActionStates = Controls.playerChachedActionStates[this.myPlayerIndex];
				if (playerChachedActionStates._gameActionState_JustPressed.ContainsKey(this.myGameAction))
				{
					playerChachedActionStates._gameActionState_JustPressed[this.myGameAction] = this.justPressd;
				}
				else
				{
					playerChachedActionStates._gameActionState_JustPressed.Add(this.myGameAction, this.justPressd);
				}
				if (playerChachedActionStates._gameActionState_JustReleased.ContainsKey(this.myGameAction))
				{
					playerChachedActionStates._gameActionState_JustReleased[this.myGameAction] = this.justReleased;
				}
				else
				{
					playerChachedActionStates._gameActionState_JustReleased.Add(this.myGameAction, this.justReleased);
				}
				if (playerChachedActionStates._gameActionState_Hold.ContainsKey(this.myGameAction))
				{
					playerChachedActionStates._gameActionState_Hold[this.myGameAction] = this.inputValue != 0f;
				}
				else
				{
					playerChachedActionStates._gameActionState_Hold.Add(this.myGameAction, this.inputValue != 0f);
				}
				if (playerChachedActionStates._gameActionState_HoldPrevious.ContainsKey(this.myGameAction))
				{
					playerChachedActionStates._gameActionState_HoldPrevious[this.myGameAction] = this.inputValuePrevious != 0f;
				}
				else
				{
					playerChachedActionStates._gameActionState_HoldPrevious.Add(this.myGameAction, this.inputValuePrevious != 0f);
				}
				if (playerChachedActionStates._gameActionState_Axis.ContainsKey(this.myGameAction))
				{
					playerChachedActionStates._gameActionState_Axis[this.myGameAction] = this.inputValue;
				}
				else
				{
					playerChachedActionStates._gameActionState_Axis.Add(this.myGameAction, this.inputValue);
				}
				if (playerChachedActionStates._gameActionState_AxisPrevious.ContainsKey(this.myGameAction))
				{
					playerChachedActionStates._gameActionState_AxisPrevious[this.myGameAction] = this.inputValuePrevious;
					return;
				}
				playerChachedActionStates._gameActionState_AxisPrevious.Add(this.myGameAction, this.inputValuePrevious);
			}

			private Controls.PlayerExt _myPlayer;

			public int myPlayerIndex = -1;

			public bool updateIfNotPlaying;

			[SerializeField]
			private bool _mapCanBeRemapped = true;

			public Controls.InputAction myGameAction = Controls.InputAction._UNDEFINED;

			public Controls.InputActionRange myGameActionRange;

			private Controls.PlayerMapCollection myPlayerMapCollection;

			[SerializeField]
			private float inputValue;

			[SerializeField]
			private float inputValuePrevious;

			[SerializeField]
			private Controls.KeyboardElement[] keyboardElements = new Controls.KeyboardElement[4];

			[SerializeField]
			private int keyboardElementsCount;

			[SerializeField]
			private Controls.MouseElement[] mouseElements = new Controls.MouseElement[4];

			[SerializeField]
			private int mouseElementsCount;

			[SerializeField]
			private Controls.JoystickElement[] joystickElements = new Controls.JoystickElement[4];

			[SerializeField]
			private int joystickElementsCount;

			private List<Controls.KeyboardElement> elementsListChache_Keyboard = new List<Controls.KeyboardElement>();

			private List<Controls.MouseElement> elementsListChache_Mouse = new List<Controls.MouseElement>();

			private List<Controls.JoystickElement> elementsListChache_Joystick = new List<Controls.JoystickElement>();

			private bool justPressd;

			private bool justReleased;

			private bool _updateNoPlayerErrorShown;
		}

		[Serializable]
		public class PlayerMapCollection
		{
			// Token: 0x060012AE RID: 4782 RVA: 0x0007755C File Offset: 0x0007575C
			public void Init()
			{
				if (this.maps == null)
				{
					this.maps = new Controls.InputActionMap[40];
				}
				for (int i = 0; i < 40; i++)
				{
					this.maps[i] = Controls.undefinedGameActionMap;
				}
			}

			// Token: 0x060012AF RID: 4783 RVA: 0x00077598 File Offset: 0x00075798
			public void DeserializationCheck()
			{
				if (this.maps.Length != 40)
				{
					Controls.InputActionMap[] array = new Controls.InputActionMap[40];
					for (int i = 0; i < 40; i++)
					{
						if (i < this.maps.Length)
						{
							array[i] = this.maps[i];
						}
						else
						{
							array[i] = Controls.undefinedGameActionMap;
						}
					}
					this.maps = array;
					if (this.mapsCount > 40)
					{
						this.mapsCount = 40;
					}
					Debug.LogWarning("PlayerMapCollection array was resized! Seems like we changed the MAX MAPS PER PLAYER");
				}
				for (int j = 0; j < this.maps.Length; j++)
				{
					if (this.maps[j] == null || this.maps[j].myGameAction == Controls.InputAction._UNDEFINED)
					{
						this.maps[j] = Controls.undefinedGameActionMap;
					}
				}
				for (int k = 0; k < this.maps.Length; k++)
				{
					this.maps[k].ResetState(true);
				}
			}

			public Controls.InputActionMap[] maps = new Controls.InputActionMap[40];

			public int mapsCount;
		}

		public class PlayerChachedActionStates
		{
			public Dictionary<Controls.InputAction, bool> _gameActionState_JustPressed = new Dictionary<Controls.InputAction, bool>();

			public Dictionary<Controls.InputAction, bool> _gameActionState_JustReleased = new Dictionary<Controls.InputAction, bool>();

			public Dictionary<Controls.InputAction, bool> _gameActionState_Hold = new Dictionary<Controls.InputAction, bool>();

			public Dictionary<Controls.InputAction, bool> _gameActionState_HoldPrevious = new Dictionary<Controls.InputAction, bool>();

			public Dictionary<Controls.InputAction, float> _gameActionState_Axis = new Dictionary<Controls.InputAction, float>();

			public Dictionary<Controls.InputAction, float> _gameActionState_AxisPrevious = new Dictionary<Controls.InputAction, float>();
		}

		[Serializable]
		public class PlayerMapCollectionSerializer
		{
			public Controls.PlayerMapCollection[] mapsToSerialize = new Controls.PlayerMapCollection[1];
		}

		public class RemappingContext
		{
			public bool isRunnning;

			public Controls.InputKind remappingIputKind;

			public bool allowButtons = true;

			public bool allowAxes = true;

			public Controls.InputActionMap mapToRemap;

			public Controls.InputActionMap tempMap;
		}

		// (Invoke) Token: 0x060012B5 RID: 4789
		public delegate void MapCallback(Controls.InputActionMap map);

		public enum InputAction
		{
			menuMoveUp,
			menuMoveDown,
			menuMoveRight,
			menuMoveLeft,
			menuSelect,
			menuSelectNoMouse,
			menuBack,
			menuAnswerYes,
			menuAnswerNo,
			menuPause,
			menuTabLeft,
			menuTabRight,
			menuSocialButton,
			cameraUp,
			cameraDown,
			cameraLeft,
			cameraRight,
			scrollUp,
			scrollDown,
			interact,
			moveUp,
			moveDown,
			moveLeft,
			moveRight,
			gameEmptyAction4,
			gameEmptyAction3,
			gameEmptyAction2,
			gameEmptyAction1,
			_COUNT,
			_UNDEFINED
		}
	}
}
