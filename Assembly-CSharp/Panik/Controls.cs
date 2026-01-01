using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

namespace Panik
{
	// Token: 0x0200011A RID: 282
	public class Controls : MonoBehaviour
	{
		// Token: 0x06000D62 RID: 3426 RVA: 0x00066090 File Offset: 0x00064290
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

		// Token: 0x06000D63 RID: 3427 RVA: 0x000660CC File Offset: 0x000642CC
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

		// Token: 0x06000D64 RID: 3428 RVA: 0x00066188 File Offset: 0x00064388
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

		// Token: 0x06000D65 RID: 3429 RVA: 0x000662E4 File Offset: 0x000644E4
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

		// Token: 0x06000D66 RID: 3430 RVA: 0x00010FC0 File Offset: 0x0000F1C0
		public static int GetPlayersCount()
		{
			return Controls.playersExtList.Count;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00010FCC File Offset: 0x0000F1CC
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

		// Token: 0x06000D68 RID: 3432 RVA: 0x00011002 File Offset: 0x0000F202
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

		// Token: 0x06000D69 RID: 3433 RVA: 0x0001102C File Offset: 0x0000F22C
		public static bool PlayerIsUsingKeyboard(Controls.PlayerExt player)
		{
			return player.rePlayer.controllers.hasKeyboard && player.lastInputKindUsed == Controls.InputKind.Keyboard;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0001104B File Offset: 0x0000F24B
		public static bool PlayerIsUsingKeyboard(int playerIndex)
		{
			return Controls.PlayerIsUsingKeyboard(Controls.GetPlayerByIndex(playerIndex));
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00011058 File Offset: 0x0000F258
		public static bool PlayerIsUsingMouse(Controls.PlayerExt player)
		{
			return player.rePlayer.controllers.hasMouse && player.lastInputKindUsed == Controls.InputKind.Mouse;
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00011077 File Offset: 0x0000F277
		public static bool PlayerIsUsingMouse(int playerIndex)
		{
			return Controls.PlayerIsUsingMouse(Controls.GetPlayerByIndex(playerIndex));
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00011084 File Offset: 0x0000F284
		public static bool PlayerIsUsingJoystick(Controls.PlayerExt player)
		{
			return player.rePlayer.controllers.joystickCount > 0 && player.lastInputKindUsed == Controls.InputKind.Joystick && player.lastUsedJoystickTemplate != null;
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x000110AD File Offset: 0x0000F2AD
		public static bool PlayerIsUsingJoystick(int playerIndex)
		{
			return Controls.PlayerIsUsingJoystick(Controls.GetPlayerByIndex(playerIndex));
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00066670 File Offset: 0x00064870
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

		// Token: 0x06000D70 RID: 3440 RVA: 0x000110BA File Offset: 0x0000F2BA
		public static void PlayersJoinSession_Start()
		{
			if (Controls.playersCanJoin)
			{
				return;
			}
			Controls.playersCanJoin = true;
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x000110CA File Offset: 0x0000F2CA
		public static void PlayersJoinSession_End()
		{
			if (!Controls.playersCanJoin)
			{
				return;
			}
			Controls.playersCanJoin = false;
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x000110DA File Offset: 0x0000F2DA
		public static bool PlayersJoinSession_IsRunning()
		{
			return Controls.playersCanJoin;
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00066784 File Offset: 0x00064984
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

		// Token: 0x06000D74 RID: 3444 RVA: 0x000110E1 File Offset: 0x0000F2E1
		public static bool PlayerJoinTry(int playerIndex)
		{
			return Controls.PlayerJoinTry(Controls.GetPlayerByIndex(playerIndex));
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00066800 File Offset: 0x00064A00
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

		// Token: 0x06000D76 RID: 3446 RVA: 0x000110EE File Offset: 0x0000F2EE
		public static bool PlayerLeaveTry(int playerIndex)
		{
			return Controls.PlayerLeaveTry(Controls.GetPlayerByIndex(playerIndex));
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x000110FB File Offset: 0x0000F2FB
		public static bool PlayerIsPlaying(Controls.PlayerExt player)
		{
			return player.isPlaying && Controls.playersPlaying.Contains(player);
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00011112 File Offset: 0x0000F312
		public static bool PlayerIsPlaying(int playerIndex)
		{
			return Controls.PlayerIsPlaying(Controls.GetPlayerByIndex(playerIndex));
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0001111F File Offset: 0x0000F31F
		public static int PlayingPlayersCount()
		{
			return Controls.playersPlaying.Count;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0001112B File Offset: 0x0000F32B
		public static List<Controls.PlayerExt> PlayingPlayersGetList()
		{
			return Controls.playersPlaying;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00066850 File Offset: 0x00064A50
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

		// Token: 0x06000D7C RID: 3452 RVA: 0x00066890 File Offset: 0x00064A90
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

		// Token: 0x06000D7D RID: 3453 RVA: 0x00011132 File Offset: 0x0000F332
		public static void VibrationSetLeft(Controls.PlayerExt player, float force)
		{
			Controls.instance.VibrationDataGet(player).motorLevelLeft = force;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00011145 File Offset: 0x0000F345
		public static void VibrationSetRight(Controls.PlayerExt player, float force)
		{
			Controls.instance.VibrationDataGet(player).motorLevelRight = force;
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x00011158 File Offset: 0x0000F358
		public static void VibrationSet(Controls.PlayerExt player, float force)
		{
			Controls.VibrationData vibrationData = Controls.instance.VibrationDataGet(player);
			vibrationData.motorLevelLeft = force;
			vibrationData.motorLevelRight = force;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00011172 File Offset: 0x0000F372
		public static void VibrationSet_PreferMax(Controls.PlayerExt player, float vibration)
		{
			Controls.VibrationSet(player, Mathf.Max(new float[]
			{
				Controls.VibrationGet(player),
				Controls.VibrationGet(player),
				vibration
			}));
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0001119B File Offset: 0x0000F39B
		public static float VibrationGetLeft(Controls.PlayerExt player)
		{
			return Controls.instance.VibrationDataGet(player).motorLevelLeft;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x000111AD File Offset: 0x0000F3AD
		public static float VibrationGetRight(Controls.PlayerExt player)
		{
			return Controls.instance.VibrationDataGet(player).motorLevelRight;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x000669FC File Offset: 0x00064BFC
		public static float VibrationGet(Controls.PlayerExt player)
		{
			Controls.VibrationData vibrationData = Controls.instance.VibrationDataGet(player);
			return Mathf.Max(vibrationData.motorLevelLeft, vibrationData.motorLevelRight);
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x000111BF File Offset: 0x0000F3BF
		public static void VibrationSetDecaySpeedLeft(Controls.PlayerExt player, float speed)
		{
			Controls.instance.VibrationDataGet(player).decaySpeedLeft = speed;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x000111D2 File Offset: 0x0000F3D2
		public static void VibrationSetDecaySpeedRight(Controls.PlayerExt player, float speed)
		{
			Controls.instance.VibrationDataGet(player).decaySpeedRight = speed;
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x000111E5 File Offset: 0x0000F3E5
		public static void VibrationSetDecaySpeed(Controls.PlayerExt player, float speed)
		{
			Controls.VibrationData vibrationData = Controls.instance.VibrationDataGet(player);
			vibrationData.decaySpeedLeft = speed;
			vibrationData.decaySpeedRight = speed;
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x000111FF File Offset: 0x0000F3FF
		public static float VibrationGetDecaySpeedLeft(Controls.PlayerExt player)
		{
			return Controls.instance.VibrationDataGet(player).decaySpeedLeft;
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00011211 File Offset: 0x0000F411
		public static float VibrationGetDecaySpeedRight(Controls.PlayerExt player)
		{
			return Controls.instance.VibrationDataGet(player).decaySpeedRight;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00066A28 File Offset: 0x00064C28
		public static float VibrationGetDecaySpeed(Controls.PlayerExt player)
		{
			Controls.VibrationData vibrationData = Controls.instance.VibrationDataGet(player);
			return Mathf.Max(vibrationData.decaySpeedLeft, vibrationData.decaySpeedRight);
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00011223 File Offset: 0x0000F423
		public static void VibrationSetDecaySpeedMult(Controls.PlayerExt player, float mult)
		{
			Controls.instance.VibrationDataGet(player).decaySpeedMult = mult;
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x00011236 File Offset: 0x0000F436
		public static float VibrationGetDecaySpeedMult(Controls.PlayerExt player)
		{
			return Controls.instance.VibrationDataGet(player).decaySpeedMult;
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x00011248 File Offset: 0x0000F448
		public static void VibrationSetPausable(Controls.PlayerExt player, bool pausable)
		{
			Controls.instance.VibrationDataGet(player).pausable = pausable;
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0001125B File Offset: 0x0000F45B
		public static bool VibrationGetPausable(Controls.PlayerExt player)
		{
			return Controls.instance.VibrationDataGet(player).pausable;
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x00066A54 File Offset: 0x00064C54
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

		// Token: 0x06000D8F RID: 3471 RVA: 0x00066AF0 File Offset: 0x00064CF0
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

		// Token: 0x06000D90 RID: 3472 RVA: 0x00066AF0 File Offset: 0x00064CF0
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

		// Token: 0x06000D91 RID: 3473 RVA: 0x00066C48 File Offset: 0x00064E48
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

		// Token: 0x06000D92 RID: 3474 RVA: 0x00066DA0 File Offset: 0x00064FA0
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

		// Token: 0x06000D93 RID: 3475 RVA: 0x00066AF0 File Offset: 0x00064CF0
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

		// Token: 0x06000D94 RID: 3476 RVA: 0x00066EF8 File Offset: 0x000650F8
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

		// Token: 0x06000D95 RID: 3477 RVA: 0x00067020 File Offset: 0x00065220
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

		// Token: 0x06000D96 RID: 3478 RVA: 0x000670B0 File Offset: 0x000652B0
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

		// Token: 0x06000D97 RID: 3479 RVA: 0x0001126D File Offset: 0x0000F46D
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

		// Token: 0x06000D98 RID: 3480 RVA: 0x00067120 File Offset: 0x00065320
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

		// Token: 0x06000D99 RID: 3481 RVA: 0x0001129F File Offset: 0x0000F49F
		public static bool PickStickAxis_Joystick(Controls.PlayerExt player, Controls.JoystickElement xElement, Controls.JoystickElement yElement, out Controls.JoystickElement pickedElement)
		{
			return Controls.PickStickAxis_Joystick(Controls.GetPlayerIndex(player), xElement, yElement, out pickedElement);
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00067180 File Offset: 0x00065380
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

		// Token: 0x06000D9B RID: 3483 RVA: 0x000112AF File Offset: 0x0000F4AF
		public static bool JoystickButton_PressedGet(int playerIndex, Controls.JoystickElement element)
		{
			return Controls.JoystickButton_PressedGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x000671BC File Offset: 0x000653BC
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

		// Token: 0x06000D9D RID: 3485 RVA: 0x000112BD File Offset: 0x0000F4BD
		public static bool JoystickButton_HoldGet(int playerIndex, Controls.JoystickElement element)
		{
			return Controls.JoystickButton_HoldGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x000671F8 File Offset: 0x000653F8
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

		// Token: 0x06000D9F RID: 3487 RVA: 0x000112CB File Offset: 0x0000F4CB
		public static bool JoystickButton_ReleasedGet(int playerIndex, Controls.JoystickElement element)
		{
			return Controls.JoystickButton_ReleasedGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00067234 File Offset: 0x00065434
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

		// Token: 0x06000DA1 RID: 3489 RVA: 0x000112D9 File Offset: 0x0000F4D9
		public static float JoystickAxis_ValueGet(int playerIndex, Controls.JoystickElement element)
		{
			return Controls.JoystickAxis_ValueGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0006727C File Offset: 0x0006547C
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

		// Token: 0x06000DA3 RID: 3491 RVA: 0x000112E7 File Offset: 0x0000F4E7
		public static float JoystickAxis_ValuePreviousGet(int playerIndex, Controls.JoystickElement element)
		{
			return Controls.JoystickAxis_ValuePreviousGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x000112F5 File Offset: 0x0000F4F5
		public static Controls.JoystickElement JoystickSelectionButton_GetByPlatform()
		{
			if (PlatformMaster.PlatformKindGet() != PlatformMaster.PlatformKind.NintendoSwitch)
			{
				return Controls.JoystickElement.ButtonDown;
			}
			return Controls.JoystickElement.ButtonRight;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00011303 File Offset: 0x0000F503
		public static Controls.JoystickElement JoystickBackButton_GetByPlatform()
		{
			if (PlatformMaster.PlatformKindGet() != PlatformMaster.PlatformKind.NintendoSwitch)
			{
				return Controls.JoystickElement.ButtonRight;
			}
			return Controls.JoystickElement.ButtonDown;
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x000672C4 File Offset: 0x000654C4
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

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00011311 File Offset: 0x0000F511
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

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0001132E File Offset: 0x0000F52E
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

		// Token: 0x06000DA9 RID: 3497 RVA: 0x00067348 File Offset: 0x00065548
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

		// Token: 0x06000DAA RID: 3498 RVA: 0x00011353 File Offset: 0x0000F553
		public static bool PickStickAxis_Mouse(Controls.PlayerExt player, Controls.MouseElement xElement, Controls.MouseElement yElement, out Controls.MouseElement pickedElement)
		{
			return Controls.PickStickAxis_Mouse(Controls.GetPlayerIndex(player), xElement, yElement, out pickedElement);
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x000673A4 File Offset: 0x000655A4
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

		// Token: 0x06000DAC RID: 3500 RVA: 0x00011363 File Offset: 0x0000F563
		public static bool MouseButton_PressedGet(int playerIndex, Controls.MouseElement element)
		{
			return Controls.MouseButton_PressedGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x00067518 File Offset: 0x00065718
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

		// Token: 0x06000DAE RID: 3502 RVA: 0x00011371 File Offset: 0x0000F571
		public static bool MouseButton_ReleasedGet(int playerIndex, Controls.MouseElement element)
		{
			return Controls.MouseButton_ReleasedGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0006768C File Offset: 0x0006588C
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

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0001137F File Offset: 0x0000F57F
		public static bool MouseButton_HoldGet(int playerIndex, Controls.MouseElement element)
		{
			return Controls.MouseButton_HoldGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x000677AC File Offset: 0x000659AC
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

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0001138D File Offset: 0x0000F58D
		public static float MouseAxis_ValueGet(int playerIndex, Controls.MouseElement element)
		{
			return Controls.MouseAxis_ValueGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x000678CC File Offset: 0x00065ACC
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

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0001139B File Offset: 0x0000F59B
		public static float MouseAxis_ValuePreviousGet(int playerIndex, Controls.MouseElement element)
		{
			return Controls.MouseAxis_ValuePreviousGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x000679E8 File Offset: 0x00065BE8
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

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00007C86 File Offset: 0x00005E86
		public static bool KeyboardElement_IsButton(Controls.KeyboardElement element)
		{
			return true;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public static bool KeyboardElement_IsAxis(Controls.KeyboardElement element)
		{
			return false;
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x000113A9 File Offset: 0x0000F5A9
		public static bool KeyboardButton_PressedGet(Controls.PlayerExt player, Controls.KeyboardElement element)
		{
			if (player == null)
			{
				Debug.LogError("KeyboardButton_Pressed(): Player is null!");
				return false;
			}
			return player.rePlayer.controllers.hasKeyboard && player.rePlayer.controllers.Keyboard.GetButtonDownById((int)element);
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x000113E4 File Offset: 0x0000F5E4
		public static bool KeyboardButton_PressedGet(int playerIndex, Controls.KeyboardElement element)
		{
			return Controls.KeyboardButton_PressedGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x000113F2 File Offset: 0x0000F5F2
		public static bool KeyboardButton_ReleasedGet(Controls.PlayerExt player, Controls.KeyboardElement element)
		{
			if (player == null)
			{
				Debug.LogError("KeyboardButton_Released(): Player is null!");
				return false;
			}
			return player.rePlayer.controllers.hasKeyboard && player.rePlayer.controllers.Keyboard.GetButtonUpById((int)element);
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0001142D File Offset: 0x0000F62D
		public static bool KeyboardButton_ReleasedGet(int playerIndex, Controls.KeyboardElement element)
		{
			return Controls.KeyboardButton_ReleasedGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0001143B File Offset: 0x0000F63B
		public static bool KeyboardButton_HoldGet(Controls.PlayerExt player, Controls.KeyboardElement element)
		{
			if (player == null)
			{
				Debug.LogError("KeyboardButton_Hold(): Player is null!");
				return false;
			}
			return player.rePlayer.controllers.hasKeyboard && player.rePlayer.controllers.Keyboard.GetButtonById((int)element);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x00011476 File Offset: 0x0000F676
		public static bool KeyboardButton_HoldGet(int playerIndex, Controls.KeyboardElement element)
		{
			return Controls.KeyboardButton_HoldGet(Controls.GetPlayerByIndex(playerIndex), element);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x000681B4 File Offset: 0x000663B4
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

		// Token: 0x06000DBF RID: 3519 RVA: 0x00011484 File Offset: 0x0000F684
		public static Controls.InputActionMap MapFind_InUse(int playerIndex, Controls.InputAction action)
		{
			return Controls.MapFindInArray(Controls.mapsPerPlayerCollection_InUse[playerIndex].maps, action);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x00011498 File Offset: 0x0000F698
		public static Controls.InputActionMap MapFind_InUse(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapFind_InUse(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x000114A6 File Offset: 0x0000F6A6
		public static Controls.InputActionMap MapFind_Default(int playerIndex, Controls.InputAction action)
		{
			return Controls.MapFindInArray(Controls.mapsPerPlayerCollection_Default[playerIndex].maps, action);
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x000114BA File Offset: 0x0000F6BA
		public static Controls.InputActionMap MapFind_Default(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapFind_Default(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x000681E0 File Offset: 0x000663E0
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

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0006825C File Offset: 0x0006645C
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

		// Token: 0x06000DC5 RID: 3525 RVA: 0x000682C4 File Offset: 0x000664C4
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

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00068380 File Offset: 0x00066580
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

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0006840C File Offset: 0x0006660C
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

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00068454 File Offset: 0x00066654
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

		// Token: 0x06000DC9 RID: 3529 RVA: 0x000114C8 File Offset: 0x0000F6C8
		public static void MapRestoreDefault_AllActionsOfPlayer(Controls.PlayerExt player, bool affectKeyboard, bool affectMouse, bool affectJoystick)
		{
			Controls.MapRestoreDefault_AllActionsOfPlayer(Controls.GetPlayerIndex(player), affectKeyboard, affectMouse, affectJoystick, true);
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x000684DC File Offset: 0x000666DC
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

		// Token: 0x06000DCB RID: 3531 RVA: 0x000114D9 File Offset: 0x0000F6D9
		public static void MapRestoreDefault_Action(Controls.PlayerExt player, Controls.InputAction action, bool affectKeyboard, bool affectMouse, bool affectJoystick)
		{
			Controls.MapRestoreDefault_Action(Controls.GetPlayerIndex(player), action, affectKeyboard, affectMouse, affectJoystick);
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x00068538 File Offset: 0x00066738
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

		// Token: 0x06000DCD RID: 3533 RVA: 0x00068588 File Offset: 0x00066788
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

		// Token: 0x06000DCE RID: 3534 RVA: 0x00068604 File Offset: 0x00066804
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

		// Token: 0x06000DCF RID: 3535 RVA: 0x00068654 File Offset: 0x00066854
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

		// Token: 0x06000DD0 RID: 3536 RVA: 0x000686A4 File Offset: 0x000668A4
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

		// Token: 0x06000DD1 RID: 3537 RVA: 0x000686F4 File Offset: 0x000668F4
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

		// Token: 0x06000DD2 RID: 3538 RVA: 0x000688C4 File Offset: 0x00066AC4
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

		// Token: 0x06000DD3 RID: 3539 RVA: 0x000114EB File Offset: 0x0000F6EB
		public static bool RemapIsRunning()
		{
			return Controls.remappingContext.isRunnning;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00068974 File Offset: 0x00066B74
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

		// Token: 0x06000DD5 RID: 3541 RVA: 0x000689C4 File Offset: 0x00066BC4
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

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00068A40 File Offset: 0x00066C40
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

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00068B60 File Offset: 0x00066D60
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

		// Token: 0x06000DD8 RID: 3544 RVA: 0x000114F7 File Offset: 0x0000F6F7
		public static List<Controls.KeyboardElement> MapGetKeyboardInputs(int playerIndex, Controls.InputActionMap map)
		{
			if (map == null)
			{
				return null;
			}
			return map.ElementKeyboard_Get();
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x00011504 File Offset: 0x0000F704
		public static List<Controls.KeyboardElement> MapGetKeyboardInputs(Controls.PlayerExt player, Controls.InputActionMap map)
		{
			return Controls.MapGetKeyboardInputs(Controls.GetPlayerIndex(player), map);
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x00068CB8 File Offset: 0x00066EB8
		public static List<Controls.KeyboardElement> MapGetKeyboardInputs(int playerIndex, Controls.InputAction action)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetKeyboardInputs(playerIndex, inputActionMap);
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x00011512 File Offset: 0x0000F712
		public static List<Controls.KeyboardElement> MapGetKeyboardInputs(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapGetKeyboardInputs(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x00011520 File Offset: 0x0000F720
		public static List<Controls.MouseElement> MapGetMouseInputs(int playerIndex, Controls.InputActionMap map)
		{
			if (map == null)
			{
				return null;
			}
			return map.ElementMouse_Get();
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0001152D File Offset: 0x0000F72D
		public static List<Controls.MouseElement> MapGetMouseInputs(Controls.PlayerExt player, Controls.InputActionMap map)
		{
			return Controls.MapGetMouseInputs(Controls.GetPlayerIndex(player), map);
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x00068CD4 File Offset: 0x00066ED4
		public static List<Controls.MouseElement> MapGetMouseInputs(int playerIndex, Controls.InputAction action)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetMouseInputs(playerIndex, inputActionMap);
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0001153B File Offset: 0x0000F73B
		public static List<Controls.MouseElement> MapGetMouseInputs(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapGetMouseInputs(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x00011549 File Offset: 0x0000F749
		public static List<Controls.JoystickElement> MapGetJoystickInputs(int playerIndex, Controls.InputActionMap map)
		{
			if (map == null)
			{
				return null;
			}
			return map.ElementJoystick_Get();
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x00011556 File Offset: 0x0000F756
		public static List<Controls.JoystickElement> MapGetJoystickInputs(Controls.PlayerExt player, Controls.InputActionMap map)
		{
			return Controls.MapGetJoystickInputs(Controls.GetPlayerIndex(player), map);
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00068CF0 File Offset: 0x00066EF0
		public static List<Controls.JoystickElement> MapGetJoystickInputs(int playerIndex, Controls.InputAction action)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetJoystickInputs(playerIndex, inputActionMap);
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00011564 File Offset: 0x0000F764
		public static List<Controls.JoystickElement> MapGetJoystickInputs(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapGetJoystickInputs(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x00068D0C File Offset: 0x00066F0C
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

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00011572 File Offset: 0x0000F772
		public static List<string> MapGetKeyboardInputs_Names(Controls.PlayerExt player, Controls.InputActionMap map)
		{
			return Controls.MapGetKeyboardInputs_Names(Controls.GetPlayerIndex(player), map);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00068D74 File Offset: 0x00066F74
		public static List<string> MapGetKeyboardInputs_Names(int playerIndex, Controls.InputAction action)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetKeyboardInputs_Names(playerIndex, inputActionMap);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00011580 File Offset: 0x0000F780
		public static List<string> MapGetKeyboardInputs_Names(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapGetKeyboardInputs_Names(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00068D90 File Offset: 0x00066F90
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

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0001158E File Offset: 0x0000F78E
		public static List<string> MapGetMouseInputs_Names(Controls.PlayerExt player, Controls.InputActionMap map)
		{
			return Controls.MapGetMouseInputs_Names(Controls.GetPlayerIndex(player), map);
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00068DF8 File Offset: 0x00066FF8
		public static List<string> MapGetMouseInputs_Names(int playerIndex, Controls.InputAction action)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetMouseInputs_Names(playerIndex, inputActionMap);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0001159C File Offset: 0x0000F79C
		public static List<string> MapGetMouseInputs_Names(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapGetMouseInputs_Names(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00068E14 File Offset: 0x00067014
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

		// Token: 0x06000DED RID: 3565 RVA: 0x000115AA File Offset: 0x0000F7AA
		public static List<string> MapGetJoystickInputs_Names(Controls.PlayerExt player, Controls.InputActionMap map)
		{
			return Controls.MapGetJoystickInputs_Names(Controls.GetPlayerIndex(player), map);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x00068E7C File Offset: 0x0006707C
		public static List<string> MapGetJoystickInputs_Names(int playerIndex, Controls.InputAction action)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetJoystickInputs_Names(playerIndex, inputActionMap);
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x000115B8 File Offset: 0x0000F7B8
		public static List<string> MapGetJoystickInputs_Names(Controls.PlayerExt player, Controls.InputAction action)
		{
			return Controls.MapGetJoystickInputs_Names(Controls.GetPlayerIndex(player), action);
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x00068E98 File Offset: 0x00067098
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

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00068F28 File Offset: 0x00067128
		public static List<string> MapGetLastInputs_Names(Controls.PlayerExt player, Controls.InputAction action, bool keyboardMouseFallback)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(player, action);
			return Controls.MapGetLastInputs_Names(player, inputActionMap, keyboardMouseFallback);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x000115C6 File Offset: 0x0000F7C6
		public static List<string> MapGetLastInputs_Names(int playerIndex, Controls.InputActionMap actionMap, bool keyboardMouseFallback)
		{
			return Controls.MapGetLastInputs_Names(Controls.GetPlayerByIndex(playerIndex), actionMap, keyboardMouseFallback);
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x000115D5 File Offset: 0x0000F7D5
		public static List<string> MapGetLastInputs_Names(int playerIndex, Controls.InputAction action, bool keyboardMouseFallback)
		{
			return Controls.MapGetLastInputs_Names(Controls.GetPlayerByIndex(playerIndex), action, keyboardMouseFallback);
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00068F48 File Offset: 0x00067148
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

		// Token: 0x06000DF5 RID: 3573 RVA: 0x000115E4 File Offset: 0x0000F7E4
		public static void MapGetKeyboardPrompts_Sprites(Controls.PlayerExt player, Controls.InputActionMap map, ref List<Sprite> spritesOut)
		{
			Controls.MapGetKeyboardPrompts_Sprites(Controls.GetPlayerIndex(player), map, ref spritesOut);
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00068FC0 File Offset: 0x000671C0
		public static void MapGetKeyboardPrompts_Sprites(int playerIndex, Controls.InputAction action, ref List<Sprite> spritesOut)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			Controls.MapGetKeyboardPrompts_Sprites(playerIndex, inputActionMap, ref spritesOut);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x000115F3 File Offset: 0x0000F7F3
		public static void MapGetKeyboardPrompts_Sprites(Controls.PlayerExt player, Controls.InputAction action, ref List<Sprite> spritesOut)
		{
			Controls.MapGetKeyboardPrompts_Sprites(Controls.GetPlayerIndex(player), action, ref spritesOut);
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00068FE0 File Offset: 0x000671E0
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

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00011602 File Offset: 0x0000F802
		public static void MapGetMousePrompts_Sprites(Controls.PlayerExt player, Controls.InputActionMap map, ref List<Sprite> spritesOut)
		{
			Controls.MapGetMousePrompts_Sprites(Controls.GetPlayerIndex(player), map, ref spritesOut);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00069058 File Offset: 0x00067258
		public static void MapGetMousePrompts_Sprites(int playerIndex, Controls.InputAction action, ref List<Sprite> spritesOut)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			Controls.MapGetMousePrompts_Sprites(playerIndex, inputActionMap, ref spritesOut);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00011611 File Offset: 0x0000F811
		public static void MapGetMousePrompts_Sprites(Controls.PlayerExt player, Controls.InputAction action, ref List<Sprite> spritesOut)
		{
			Controls.MapGetMousePrompts_Sprites(Controls.GetPlayerIndex(player), action, ref spritesOut);
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x00069078 File Offset: 0x00067278
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

		// Token: 0x06000DFD RID: 3581 RVA: 0x00011620 File Offset: 0x0000F820
		public static void MapGetJoystickPrompts_Sprites(Controls.PlayerExt player, Controls.InputActionMap map, ref List<Sprite> spritesOut)
		{
			Controls.MapGetJoystickPrompts_Sprites(Controls.GetPlayerIndex(player), map, ref spritesOut);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x000690F0 File Offset: 0x000672F0
		public static void MapGetJoystickPrompts_Sprites(int playerIndex, Controls.InputAction action, ref List<Sprite> spritesOut)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			Controls.MapGetJoystickPrompts_Sprites(playerIndex, inputActionMap, ref spritesOut);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0001162F File Offset: 0x0000F82F
		public static void MapGetJoystickPrompts_Sprites(Controls.PlayerExt player, Controls.InputAction action, ref List<Sprite> spritesOut)
		{
			Controls.MapGetJoystickPrompts_Sprites(Controls.GetPlayerIndex(player), action, ref spritesOut);
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00069110 File Offset: 0x00067310
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

		// Token: 0x06000E01 RID: 3585 RVA: 0x000691A0 File Offset: 0x000673A0
		public static void MapGetLastPrompts_Sprites(Controls.PlayerExt player, Controls.InputAction action, ref List<Sprite> spritesOut, bool keyboardMouseFallback)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(player, action);
			Controls.MapGetLastPrompts_Sprites(player, inputActionMap, ref spritesOut, keyboardMouseFallback);
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0001163E File Offset: 0x0000F83E
		public static void MapGetLastPrompts_Sprites(int playerIndex, Controls.InputActionMap map, ref List<Sprite> spritesOut, bool keyboardMouseFallback)
		{
			Controls.MapGetLastPrompts_Sprites(Controls.GetPlayerByIndex(playerIndex), map, ref spritesOut, keyboardMouseFallback);
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0001164E File Offset: 0x0000F84E
		public static void MapGetLastPrompts_Sprites(int playerIndex, Controls.InputAction action, ref List<Sprite> spritesOut, bool keyboardMouseFallback)
		{
			Controls.MapGetLastPrompts_Sprites(Controls.GetPlayerByIndex(playerIndex), action, ref spritesOut, keyboardMouseFallback);
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x000691C0 File Offset: 0x000673C0
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

		// Token: 0x06000E05 RID: 3589 RVA: 0x0001165E File Offset: 0x0000F85E
		public static void MapGetKeyboardPrompts_TextSprites(Controls.PlayerExt player, Controls.InputActionMap map, ref List<string> textSpritesOut)
		{
			Controls.MapGetKeyboardPrompts_TextSprites(Controls.GetPlayerIndex(player), map, ref textSpritesOut);
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x00069238 File Offset: 0x00067438
		public static void MapGetKeyboardPrompts_TextSprites(int playerIndex, Controls.InputAction action, ref List<string> textSpritesOut)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			Controls.MapGetKeyboardPrompts_TextSprites(playerIndex, inputActionMap, ref textSpritesOut);
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0001166D File Offset: 0x0000F86D
		public static void MapGetKeyboardPrompts_TextSprites(Controls.PlayerExt player, Controls.InputAction action, ref List<string> textSpritesOut)
		{
			Controls.MapGetKeyboardPrompts_TextSprites(Controls.GetPlayerIndex(player), action, ref textSpritesOut);
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x00069258 File Offset: 0x00067458
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

		// Token: 0x06000E09 RID: 3593 RVA: 0x0001167C File Offset: 0x0000F87C
		public static void MapGetMousePrompts_TextSprites(Controls.PlayerExt player, Controls.InputActionMap map, ref List<string> textSpritesOut)
		{
			Controls.MapGetMousePrompts_TextSprites(Controls.GetPlayerIndex(player), map, ref textSpritesOut);
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x000692D0 File Offset: 0x000674D0
		public static void MapGetMousePrompts_TextSprites(int playerIndex, Controls.InputAction action, ref List<string> textSpritesOut)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			Controls.MapGetMousePrompts_TextSprites(playerIndex, inputActionMap, ref textSpritesOut);
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x0001168B File Offset: 0x0000F88B
		public static void MapGetMousePrompts_TextSprites(Controls.PlayerExt player, Controls.InputAction action, ref List<string> textSpritesOut)
		{
			Controls.MapGetMousePrompts_TextSprites(Controls.GetPlayerIndex(player), action, ref textSpritesOut);
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x000692F0 File Offset: 0x000674F0
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

		// Token: 0x06000E0D RID: 3597 RVA: 0x0001169A File Offset: 0x0000F89A
		public static void MapGetJoystickPrompts_TextSprites(Controls.PlayerExt player, Controls.InputActionMap map, ref List<string> textSpritesOut)
		{
			Controls.MapGetJoystickPrompts_TextSprites(Controls.GetPlayerIndex(player), map, ref textSpritesOut);
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00069368 File Offset: 0x00067568
		public static void MapGetJoystickPrompts_TextSprites(int playerIndex, Controls.InputAction action, ref List<string> textSpritesOut)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			Controls.MapGetJoystickPrompts_TextSprites(playerIndex, inputActionMap, ref textSpritesOut);
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x000116A9 File Offset: 0x0000F8A9
		public static void MapGetJoystickPrompts_TextSprites(Controls.PlayerExt player, Controls.InputAction action, ref List<string> textSpritesOut)
		{
			Controls.MapGetJoystickPrompts_TextSprites(Controls.GetPlayerIndex(player), action, ref textSpritesOut);
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00069388 File Offset: 0x00067588
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

		// Token: 0x06000E11 RID: 3601 RVA: 0x00069418 File Offset: 0x00067618
		public static void MapGetLastPrompts_TextSprites(Controls.PlayerExt player, Controls.InputAction action, ref List<string> textSpritesOut, bool keyboardMouseFallback)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(player, action);
			Controls.MapGetLastPrompts_TextSprites(player, inputActionMap, ref textSpritesOut, keyboardMouseFallback);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x000116B8 File Offset: 0x0000F8B8
		public static void MapGetLastPrompts_TextSprites(int playerIndex, Controls.InputActionMap map, ref List<string> textSpritesOut, bool keyboardMouseFallback)
		{
			Controls.MapGetLastPrompts_TextSprites(Controls.GetPlayerByIndex(playerIndex), map, ref textSpritesOut, keyboardMouseFallback);
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x000116C8 File Offset: 0x0000F8C8
		public static void MapGetLastPrompts_TextSprites(int playerIndex, Controls.InputAction action, ref List<string> textSpritesOut, bool keyboardMouseFallback)
		{
			Controls.MapGetLastPrompts_TextSprites(Controls.GetPlayerByIndex(playerIndex), action, ref textSpritesOut, keyboardMouseFallback);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00069438 File Offset: 0x00067638
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

		// Token: 0x06000E15 RID: 3605 RVA: 0x000116D8 File Offset: 0x0000F8D8
		public static Sprite MapGetKeyboardPrompt_Sprite(Controls.PlayerExt player, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetKeyboardPrompt_Sprite(Controls.GetPlayerIndex(player), map, index);
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00069470 File Offset: 0x00067670
		public static Sprite MapGetKeyboardPrompt_Sprite(int playerIndex, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetKeyboardPrompt_Sprite(playerIndex, inputActionMap, index);
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x000116E7 File Offset: 0x0000F8E7
		public static Sprite MapGetKeyboardPrompt_Sprite(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetKeyboardPrompt_Sprite(Controls.GetPlayerIndex(player), action, index);
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00069490 File Offset: 0x00067690
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

		// Token: 0x06000E19 RID: 3609 RVA: 0x000116F6 File Offset: 0x0000F8F6
		public static Sprite MapGetMousePrompt_Sprite(Controls.PlayerExt player, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetMousePrompt_Sprite(Controls.GetPlayerIndex(player), map, index);
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x000694C8 File Offset: 0x000676C8
		public static Sprite MapGetMousePrompt_Sprite(int playerIndex, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetMousePrompt_Sprite(playerIndex, inputActionMap, index);
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00011705 File Offset: 0x0000F905
		public static Sprite MapGetMousePrompt_Sprite(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetMousePrompt_Sprite(Controls.GetPlayerIndex(player), action, index);
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x000694E8 File Offset: 0x000676E8
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

		// Token: 0x06000E1D RID: 3613 RVA: 0x00011714 File Offset: 0x0000F914
		public static Sprite MapGetJoystickPrompt_Sprite(Controls.PlayerExt player, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetJoystickPrompt_Sprite(Controls.GetPlayerIndex(player), map, index);
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00069520 File Offset: 0x00067720
		public static Sprite MapGetJoystickPrompt_Sprite(int playerIndex, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetJoystickPrompt_Sprite(playerIndex, inputActionMap, index);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00011723 File Offset: 0x0000F923
		public static Sprite MapGetJoystickPrompt_Sprite(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetJoystickPrompt_Sprite(Controls.GetPlayerIndex(player), action, index);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00069540 File Offset: 0x00067740
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

		// Token: 0x06000E21 RID: 3617 RVA: 0x000695D4 File Offset: 0x000677D4
		public static Sprite MapGetLastPrompt_Sprite(Controls.PlayerExt player, Controls.InputAction action, bool keyboardMouseFallback, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(player, action);
			return Controls.MapGetLastPrompt_Sprite(player, inputActionMap, keyboardMouseFallback, index);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00011732 File Offset: 0x0000F932
		public static Sprite MapGetLastPrompt_Sprite(int playerIndex, Controls.InputActionMap map, bool keyboardMouseFallback, int index = 0)
		{
			return Controls.MapGetLastPrompt_Sprite(Controls.GetPlayerByIndex(playerIndex), map, keyboardMouseFallback, index);
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x00011742 File Offset: 0x0000F942
		public static Sprite MapGetLastPrompt_Sprite(int playerIndex, Controls.InputAction action, bool keyboardMouseFallback, int index = 0)
		{
			return Controls.MapGetLastPrompt_Sprite(Controls.GetPlayerByIndex(playerIndex), action, keyboardMouseFallback, index);
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x000695F4 File Offset: 0x000677F4
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

		// Token: 0x06000E25 RID: 3621 RVA: 0x00011752 File Offset: 0x0000F952
		public static string MapGetKeyboardPrompt_TextSprite(Controls.PlayerExt player, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetKeyboardPrompt_TextSprite(Controls.GetPlayerIndex(player), map, index);
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x0006962C File Offset: 0x0006782C
		public static string MapGetKeyboardPrompt_TextSprite(int playerIndex, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetKeyboardPrompt_TextSprite(playerIndex, inputActionMap, index);
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00011761 File Offset: 0x0000F961
		public static string MapGetKeyboardPrompt_TextSprite(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetKeyboardPrompt_TextSprite(Controls.GetPlayerIndex(player), action, index);
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0006964C File Offset: 0x0006784C
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

		// Token: 0x06000E29 RID: 3625 RVA: 0x00011770 File Offset: 0x0000F970
		public static string MapGetMousePrompt_TextSprite(Controls.PlayerExt player, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetMousePrompt_TextSprite(Controls.GetPlayerIndex(player), map, index);
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00069684 File Offset: 0x00067884
		public static string MapGetMousePrompt_TextSprite(int playerIndex, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetMousePrompt_TextSprite(playerIndex, inputActionMap, index);
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0001177F File Offset: 0x0000F97F
		public static string MapGetMousePrompt_TextSprite(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetMousePrompt_TextSprite(Controls.GetPlayerIndex(player), action, index);
		}

		// Token: 0x06000E2C RID: 3628 RVA: 0x000696A4 File Offset: 0x000678A4
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

		// Token: 0x06000E2D RID: 3629 RVA: 0x0001178E File Offset: 0x0000F98E
		public static string MapGetJoystickPrompt_TextSprite(Controls.PlayerExt player, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetJoystickPrompt_TextSprite(Controls.GetPlayerIndex(player), map, index);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x000696DC File Offset: 0x000678DC
		public static string MapGetJoystickPrompt_TextSprite(int playerIndex, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(playerIndex, action);
			return Controls.MapGetJoystickPrompt_TextSprite(playerIndex, inputActionMap, index);
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0001179D File Offset: 0x0000F99D
		public static string MapGetJoystickPrompt_TextSprite(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetJoystickPrompt_TextSprite(Controls.GetPlayerIndex(player), action, index);
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x000696FC File Offset: 0x000678FC
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

		// Token: 0x06000E31 RID: 3633 RVA: 0x0006978C File Offset: 0x0006798C
		public static string MapGetLastPrompt_TextSprite(Controls.PlayerExt player, Controls.InputAction action, bool keyboardMouseFallback, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(player, action);
			return Controls.MapGetLastPrompt_TextSprite(player, inputActionMap, keyboardMouseFallback, index);
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x000117AC File Offset: 0x0000F9AC
		public static string MapGetLastPrompt_TextSprite(int playerIndex, Controls.InputActionMap map, bool keyboardMouseFallback, int index = 0)
		{
			return Controls.MapGetLastPrompt_TextSprite(Controls.GetPlayerByIndex(playerIndex), map, keyboardMouseFallback, index);
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x000117BC File Offset: 0x0000F9BC
		public static string MapGetLastPrompt_TextSprite(int playerIndex, Controls.InputAction action, bool keyboardMouseFallback, int index = 0)
		{
			return Controls.MapGetLastPrompt_TextSprite(Controls.GetPlayerByIndex(playerIndex), action, keyboardMouseFallback, index);
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x000697AC File Offset: 0x000679AC
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

		// Token: 0x06000E35 RID: 3637 RVA: 0x00069834 File Offset: 0x00067A34
		public static string MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(player, action);
			return Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(player, inputActionMap, index);
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x000117CC File Offset: 0x0000F9CC
		public static string MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(int playerIndex, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(Controls.GetPlayerByIndex(playerIndex), map, index);
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x000117DB File Offset: 0x0000F9DB
		public static string MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(int playerIndex, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(Controls.GetPlayerByIndex(playerIndex), action, index);
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00069854 File Offset: 0x00067A54
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

		// Token: 0x06000E39 RID: 3641 RVA: 0x000698DC File Offset: 0x00067ADC
		public static string MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(Controls.PlayerExt player, Controls.InputAction action, int index = 0)
		{
			Controls.InputActionMap inputActionMap = Controls.MapFind_InUse(player, action);
			return Controls.MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(player, inputActionMap, index);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x000117EA File Offset: 0x0000F9EA
		public static string MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(int playerIndex, Controls.InputActionMap map, int index = 0)
		{
			return Controls.MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(Controls.GetPlayerByIndex(playerIndex), map, index);
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x000117F9 File Offset: 0x0000F9F9
		public static string MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(int playerIndex, Controls.InputAction action, int index = 0)
		{
			return Controls.MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(Controls.GetPlayerByIndex(playerIndex), action, index);
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x000698FC File Offset: 0x00067AFC
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

		// Token: 0x06000E3D RID: 3645 RVA: 0x00069970 File Offset: 0x00067B70
		private void GameMapsInit()
		{
			for (int i = 0; i < 1; i++)
			{
				Controls.PlayerMapCollection playerMapCollection = Controls.mapsPerPlayerCollection_InUse[i];
				Controls.MenuMapsRedefine(true, i, playerMapCollection, true, true, true);
				Controls.GameMapsRedefine(true, i, playerMapCollection, true, true, true);
			}
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x000699A8 File Offset: 0x00067BA8
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

		// Token: 0x06000E3F RID: 3647 RVA: 0x00069E40 File Offset: 0x00068040
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

		// Token: 0x06000E40 RID: 3648 RVA: 0x0006A180 File Offset: 0x00068380
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

		// Token: 0x06000E41 RID: 3649 RVA: 0x0006A7A8 File Offset: 0x000689A8
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

		// Token: 0x06000E42 RID: 3650 RVA: 0x0006A7EC File Offset: 0x000689EC
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

		// Token: 0x06000E43 RID: 3651 RVA: 0x0006A830 File Offset: 0x00068A30
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

		// Token: 0x06000E44 RID: 3652 RVA: 0x00011808 File Offset: 0x0000FA08
		public static bool ActionButton_PressedGet(int playerIndex, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionButton_PressedGet(Controls.GetPlayerByIndex(playerIndex), action, ignoreIfNotPlaying);
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0006A880 File Offset: 0x00068A80
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

		// Token: 0x06000E46 RID: 3654 RVA: 0x0006A8A8 File Offset: 0x00068AA8
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

		// Token: 0x06000E47 RID: 3655 RVA: 0x00011817 File Offset: 0x0000FA17
		public static bool ActionButton_ReleasedGet(int playerIndex, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionButton_ReleasedGet(Controls.GetPlayerByIndex(playerIndex), action, ignoreIfNotPlaying);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x0006A8F8 File Offset: 0x00068AF8
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

		// Token: 0x06000E49 RID: 3657 RVA: 0x0006A920 File Offset: 0x00068B20
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

		// Token: 0x06000E4A RID: 3658 RVA: 0x00011826 File Offset: 0x0000FA26
		public static bool ActionButton_HoldGet(int playerIndex, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionButton_HoldGet(Controls.GetPlayerByIndex(playerIndex), action, ignoreIfNotPlaying);
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0006A970 File Offset: 0x00068B70
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

		// Token: 0x06000E4C RID: 3660 RVA: 0x0006A998 File Offset: 0x00068B98
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

		// Token: 0x06000E4D RID: 3661 RVA: 0x00011835 File Offset: 0x0000FA35
		public static bool ActionButton_HoldPreviousGet(int playerIndex, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionButton_HoldPreviousGet(Controls.GetPlayerByIndex(playerIndex), action, ignoreIfNotPlaying);
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0006A9E8 File Offset: 0x00068BE8
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

		// Token: 0x06000E4F RID: 3663 RVA: 0x0006AA10 File Offset: 0x00068C10
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

		// Token: 0x06000E50 RID: 3664 RVA: 0x00011844 File Offset: 0x0000FA44
		public static float ActionAxis_GetValue(int playerIndex, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionAxis_GetValue(Controls.GetPlayerByIndex(playerIndex), action, ignoreIfNotPlaying);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0006AA6C File Offset: 0x00068C6C
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

		// Token: 0x06000E52 RID: 3666 RVA: 0x00011853 File Offset: 0x0000FA53
		public static float ActionAxis_GetValuePrevious(int playerIndex, Controls.InputAction action, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionAxis_GetValuePrevious(Controls.GetPlayerByIndex(playerIndex), action, ignoreIfNotPlaying);
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0006AAC8 File Offset: 0x00068CC8
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

		// Token: 0x06000E54 RID: 3668 RVA: 0x00011862 File Offset: 0x0000FA62
		public static float ActionAxisPair_GetValue(int playerIndex, Controls.InputAction actionPlus, Controls.InputAction actionMinus, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionAxisPair_GetValue(Controls.GetPlayerByIndex(playerIndex), actionPlus, actionMinus, ignoreIfNotPlaying);
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x0006AB04 File Offset: 0x00068D04
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

		// Token: 0x06000E56 RID: 3670 RVA: 0x00011872 File Offset: 0x0000FA72
		public static float ActionAxisPair_GetValuePrevious(int playerIndex, Controls.InputAction actionPlus, Controls.InputAction actionMinus, bool ignoreIfNotPlaying = true)
		{
			return Controls.ActionAxisPair_GetValuePrevious(Controls.GetPlayerByIndex(playerIndex), actionPlus, actionMinus, ignoreIfNotPlaying);
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00011882 File Offset: 0x0000FA82
		public static bool MenuDirectionalAny_PressedGet(int playerIndex)
		{
			return Controls.ActionButton_PressedGet(playerIndex, Controls.InputAction.menuMoveUp, true) || Controls.ActionButton_PressedGet(playerIndex, Controls.InputAction.menuMoveDown, true) || Controls.ActionButton_PressedGet(playerIndex, Controls.InputAction.menuMoveRight, true) || Controls.ActionButton_PressedGet(playerIndex, Controls.InputAction.menuMoveLeft, true);
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x000118AC File Offset: 0x0000FAAC
		private void Awake()
		{
			if (Controls.instance != null)
			{
				global::UnityEngine.Object.Destroy(this);
				return;
			}
			Controls.instance = this;
			this.PlayersInit();
			this.GameControlsInit();
			this.CallbacksInit();
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x000118DA File Offset: 0x0000FADA
		private void Update()
		{
			this.PlayersUpdate();
			this.SplitScreenUpdate();
			this.VibrationUpdate();
			this.InputActionsUpdate();
			this._RemapContextUpdate();
		}

		// Token: 0x04000DED RID: 3565
		public static Controls instance = null;

		// Token: 0x04000DEE RID: 3566
		public const float VIBRATION_MENU_HOVER = 0.1f;

		// Token: 0x04000DEF RID: 3567
		public const float VIBRATION_MENU_SELECT = 0.25f;

		// Token: 0x04000DF0 RID: 3568
		public const float VIBRATION_MENU_SELECT_STRONG = 0.5f;

		// Token: 0x04000DF1 RID: 3569
		public const float VIBRATION_SLOT_KATLAK = 0.25f;

		// Token: 0x04000DF2 RID: 3570
		public const float VIBRATION_SLOT_SCORE = 0.25f;

		// Token: 0x04000DF3 RID: 3571
		public const float VIBRATION_SLOT_SCORE_666 = 0.5f;

		// Token: 0x04000DF4 RID: 3572
		public const float VIBRATION_SLOT_SCORE_999 = 0.5f;

		// Token: 0x04000DF5 RID: 3573
		public const float VIBRATION_SLOT_FINALIZE = 0.25f;

		// Token: 0x04000DF6 RID: 3574
		public const float VIBRATION_MOMENT_IMPACT = 0.75f;

		// Token: 0x04000DF7 RID: 3575
		public const float VIBRATION_MOMENT_IMPACT_STRONG = 1f;

		// Token: 0x04000DF8 RID: 3576
		private const bool PRINT_ELEMENT_IDENTIFIERS = false;

		// Token: 0x04000DF9 RID: 3577
		public const int MAX_PLAYERS_SUPPORTED = 1;

		// Token: 0x04000DFA RID: 3578
		public Controls.InputKind PLAYER_DESKTOP_PREFERRED_INPUT = Controls.InputKind.Mouse;

		// Token: 0x04000DFB RID: 3579
		public const bool KEYBOARD_AND_MOUSE_ESSENTIAL_TOGHETHER = true;

		// Token: 0x04000DFC RID: 3580
		public const bool MOUSE_MOVEMENT_SWITCHES_LAST_INPUT_DEFAULT = true;

		// Token: 0x04000DFD RID: 3581
		public const bool ALLOW_MOUSE_INPUT = true;

		// Token: 0x04000DFE RID: 3582
		public const bool ALLOW_KEYBOARD_INPUT = true;

		// Token: 0x04000DFF RID: 3583
		public const bool ALLOW_JOYSTICK_INPUT = true;

		// Token: 0x04000E00 RID: 3584
		public const float MOUSE_MOVEMENT_SENSITIVITY_MULT_X = 0.5f;

		// Token: 0x04000E01 RID: 3585
		public const float MOUSE_MOVEMENT_SENSITIVITY_MULT_Y = 0.5f;

		// Token: 0x04000E02 RID: 3586
		private const float VIBRATION_DECAY_SPEED_DEFAULT = 4f;

		// Token: 0x04000E03 RID: 3587
		private const float MOUSE_INPUT_CHANGE_DEADZONE = 0.1f;

		// Token: 0x04000E04 RID: 3588
		private const float MOUSE_GAME_DEADZONE = 0f;

		// Token: 0x04000E05 RID: 3589
		private const float JOYSTICK_GAME_DEADZONE = 0.1f;

		// Token: 0x04000E06 RID: 3590
		public const float JOYSTICK_MENU_DEADZONE = 0.35f;

		// Token: 0x04000E07 RID: 3591
		private const float AXIS_REMAP_DEADZONE = 0.8f;

		// Token: 0x04000E08 RID: 3592
		private const int MAX_MAPS_PER_PLAYER = 40;

		// Token: 0x04000E09 RID: 3593
		private const int MAX_INPUTS_PER_MAP_INPUT_KIND = 4;

		// Token: 0x04000E0A RID: 3594
		private static bool[] mouseMovementSwitchesLastInput = null;

		// Token: 0x04000E0B RID: 3595
		private static List<Controls.PlayerExt> playersPool = new List<Controls.PlayerExt>();

		// Token: 0x04000E0C RID: 3596
		public static List<Controls.PlayerExt> playersExtList = new List<Controls.PlayerExt>();

		// Token: 0x04000E0D RID: 3597
		public static Controls.PlayerExt p1 = null;

		// Token: 0x04000E0E RID: 3598
		private bool controllerNotSupported_Showed;

		// Token: 0x04000E0F RID: 3599
		public static int maxPlayingPlayers = 4;

		// Token: 0x04000E10 RID: 3600
		private static bool playersCanJoin = false;

		// Token: 0x04000E11 RID: 3601
		private static List<Controls.PlayerExt> playersPlaying = new List<Controls.PlayerExt>();

		// Token: 0x04000E12 RID: 3602
		private Dictionary<Controls.PlayerExt, Controls.VibrationData> vibrations = new Dictionary<Controls.PlayerExt, Controls.VibrationData>();

		// Token: 0x04000E13 RID: 3603
		private const int MOUSE_BUTTON_LEFT = 0;

		// Token: 0x04000E14 RID: 3604
		private const int MOUSE_BUTTON_RIGHT = 1;

		// Token: 0x04000E15 RID: 3605
		private const int MOUSE_BUTTON_MIDDLE = 2;

		// Token: 0x04000E16 RID: 3606
		private const int MOUSE_AXIS_SCROLLWHEEL_VERTICAL = 2;

		// Token: 0x04000E17 RID: 3607
		private const int MOUSE_AXIS_SCROLLWHEEL_HORIZONTAL = 3;

		// Token: 0x04000E18 RID: 3608
		private const int MOUSE_AXIS_X = 0;

		// Token: 0x04000E19 RID: 3609
		private const int MOUSE_AXIS_Y = 1;

		// Token: 0x04000E1A RID: 3610
		private static Controls.InputActionMap undefinedGameActionMap = new Controls.InputActionMap(-1, Controls.InputAction._UNDEFINED, Controls.InputActionRange.positive, null, false);

		// Token: 0x04000E1B RID: 3611
		public static Controls.PlayerMapCollection[] mapsPerPlayerCollection_InUse = new Controls.PlayerMapCollection[1];

		// Token: 0x04000E1C RID: 3612
		public static Controls.PlayerMapCollection[] mapsPerPlayerCollection_Default = new Controls.PlayerMapCollection[1];

		// Token: 0x04000E1D RID: 3613
		public static Controls.PlayerChachedActionStates[] playerChachedActionStates = new Controls.PlayerChachedActionStates[1];

		// Token: 0x04000E1E RID: 3614
		private static bool systemReady = false;

		// Token: 0x04000E1F RID: 3615
		public static Controls.PlayerMapCollectionSerializer mapsCollectionSerializer = new Controls.PlayerMapCollectionSerializer();

		// Token: 0x04000E20 RID: 3616
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

		// Token: 0x04000E21 RID: 3617
		private static Controls.MouseElement[] bannedElements_Mouse = new Controls.MouseElement[0];

		// Token: 0x04000E22 RID: 3618
		private static Controls.JoystickElement[] bannedElements_Joystick = new Controls.JoystickElement[]
		{
			Controls.JoystickElement.Select,
			Controls.JoystickElement.Start,
			Controls.JoystickElement.Home
		};

		// Token: 0x04000E23 RID: 3619
		private static Dictionary<Controls.KeyboardElement, bool> bannedElementsDict_Keyboard = null;

		// Token: 0x04000E24 RID: 3620
		private static Dictionary<Controls.MouseElement, bool> bannedElementsDict_Mouse = null;

		// Token: 0x04000E25 RID: 3621
		private static Dictionary<Controls.JoystickElement, bool> bannedElementsDict_Joystick = null;

		// Token: 0x04000E26 RID: 3622
		private static Controls.RemappingContext remappingContext = new Controls.RemappingContext();

		// Token: 0x04000E27 RID: 3623
		public static Controls.MapCallback onRemap_Start;

		// Token: 0x04000E28 RID: 3624
		public static Controls.MapCallback onRemap_InputAdded;

		// Token: 0x04000E29 RID: 3625
		public static Controls.MapCallback onRemap_End_Success;

		// Token: 0x04000E2A RID: 3626
		public static Controls.MapCallback onRemap_End_Abort;

		// Token: 0x04000E2B RID: 3627
		public static Controls.MapCallback onRemap_End_Generic;

		// Token: 0x04000E2C RID: 3628
		public static Controls.MapCallback onLastInputKindChangedAny;

		// Token: 0x04000E2D RID: 3629
		public static Controls.MapCallback onPromptsUpdateRequest;

		// Token: 0x0200011B RID: 283
		public enum InputKind
		{
			// Token: 0x04000E2F RID: 3631
			Noone,
			// Token: 0x04000E30 RID: 3632
			Keyboard,
			// Token: 0x04000E31 RID: 3633
			Mouse,
			// Token: 0x04000E32 RID: 3634
			Joystick
		}

		// Token: 0x0200011C RID: 284
		public class PlayerExt
		{
			// Token: 0x04000E33 RID: 3635
			public Player rePlayer;

			// Token: 0x04000E34 RID: 3636
			public bool isPlaying;

			// Token: 0x04000E35 RID: 3637
			public Controls.InputKind lastInputKindUsed;

			// Token: 0x04000E36 RID: 3638
			public Controls.InputKind lastInputKindUsedOld;

			// Token: 0x04000E37 RID: 3639
			public int lastUsedJoystickIndex = -1;

			// Token: 0x04000E38 RID: 3640
			public Joystick lastJoystickUsed;

			// Token: 0x04000E39 RID: 3641
			public IGamepadTemplate lastUsedJoystickTemplate;
		}

		// Token: 0x0200011D RID: 285
		private class VibrationData
		{
			// Token: 0x06000E5D RID: 3677 RVA: 0x0006AC20 File Offset: 0x00068E20
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

			// Token: 0x04000E3A RID: 3642
			public Controls.PlayerExt myPlayer;

			// Token: 0x04000E3B RID: 3643
			public float motorLevelLeft;

			// Token: 0x04000E3C RID: 3644
			public float motorLevelRight;

			// Token: 0x04000E3D RID: 3645
			public float decaySpeedLeft;

			// Token: 0x04000E3E RID: 3646
			public float decaySpeedRight;

			// Token: 0x04000E3F RID: 3647
			public float decaySpeedMult;

			// Token: 0x04000E40 RID: 3648
			public bool pausable;
		}

		// Token: 0x0200011E RID: 286
		public enum JoystickElement
		{
			// Token: 0x04000E42 RID: 3650
			ButtonDown,
			// Token: 0x04000E43 RID: 3651
			ButtonRight,
			// Token: 0x04000E44 RID: 3652
			ButtonLeft,
			// Token: 0x04000E45 RID: 3653
			ButtonUp,
			// Token: 0x04000E46 RID: 3654
			Start,
			// Token: 0x04000E47 RID: 3655
			Select,
			// Token: 0x04000E48 RID: 3656
			Home,
			// Token: 0x04000E49 RID: 3657
			LeftStickButton,
			// Token: 0x04000E4A RID: 3658
			RightStickButton,
			// Token: 0x04000E4B RID: 3659
			LeftShoulder,
			// Token: 0x04000E4C RID: 3660
			RightShoulder,
			// Token: 0x04000E4D RID: 3661
			DPadUp,
			// Token: 0x04000E4E RID: 3662
			DPadDown,
			// Token: 0x04000E4F RID: 3663
			DPadLeft,
			// Token: 0x04000E50 RID: 3664
			DPadRight,
			// Token: 0x04000E51 RID: 3665
			LeftStickX,
			// Token: 0x04000E52 RID: 3666
			LeftStickY,
			// Token: 0x04000E53 RID: 3667
			RightStickX,
			// Token: 0x04000E54 RID: 3668
			RightStickY,
			// Token: 0x04000E55 RID: 3669
			LeftTrigger,
			// Token: 0x04000E56 RID: 3670
			RightTrigger,
			// Token: 0x04000E57 RID: 3671
			Count,
			// Token: 0x04000E58 RID: 3672
			Undefined
		}

		// Token: 0x0200011F RID: 287
		public enum MouseElement
		{
			// Token: 0x04000E5A RID: 3674
			LeftButton,
			// Token: 0x04000E5B RID: 3675
			RightButton,
			// Token: 0x04000E5C RID: 3676
			MiddleButton,
			// Token: 0x04000E5D RID: 3677
			axisScrollWheelVertical,
			// Token: 0x04000E5E RID: 3678
			axisScrollWheelHorizontal,
			// Token: 0x04000E5F RID: 3679
			axisX,
			// Token: 0x04000E60 RID: 3680
			axisY,
			// Token: 0x04000E61 RID: 3681
			Count,
			// Token: 0x04000E62 RID: 3682
			Undefined
		}

		// Token: 0x02000120 RID: 288
		public enum KeyboardElement
		{
			// Token: 0x04000E64 RID: 3684
			None,
			// Token: 0x04000E65 RID: 3685
			A,
			// Token: 0x04000E66 RID: 3686
			B,
			// Token: 0x04000E67 RID: 3687
			C,
			// Token: 0x04000E68 RID: 3688
			D,
			// Token: 0x04000E69 RID: 3689
			E,
			// Token: 0x04000E6A RID: 3690
			F,
			// Token: 0x04000E6B RID: 3691
			G,
			// Token: 0x04000E6C RID: 3692
			H,
			// Token: 0x04000E6D RID: 3693
			I,
			// Token: 0x04000E6E RID: 3694
			J,
			// Token: 0x04000E6F RID: 3695
			K,
			// Token: 0x04000E70 RID: 3696
			L,
			// Token: 0x04000E71 RID: 3697
			M,
			// Token: 0x04000E72 RID: 3698
			N,
			// Token: 0x04000E73 RID: 3699
			O,
			// Token: 0x04000E74 RID: 3700
			P,
			// Token: 0x04000E75 RID: 3701
			Q,
			// Token: 0x04000E76 RID: 3702
			R,
			// Token: 0x04000E77 RID: 3703
			S,
			// Token: 0x04000E78 RID: 3704
			T,
			// Token: 0x04000E79 RID: 3705
			U,
			// Token: 0x04000E7A RID: 3706
			V,
			// Token: 0x04000E7B RID: 3707
			W,
			// Token: 0x04000E7C RID: 3708
			X,
			// Token: 0x04000E7D RID: 3709
			Y,
			// Token: 0x04000E7E RID: 3710
			Z,
			// Token: 0x04000E7F RID: 3711
			Zero,
			// Token: 0x04000E80 RID: 3712
			One,
			// Token: 0x04000E81 RID: 3713
			Two,
			// Token: 0x04000E82 RID: 3714
			Three,
			// Token: 0x04000E83 RID: 3715
			Four,
			// Token: 0x04000E84 RID: 3716
			Five,
			// Token: 0x04000E85 RID: 3717
			Six,
			// Token: 0x04000E86 RID: 3718
			Seven,
			// Token: 0x04000E87 RID: 3719
			Eight,
			// Token: 0x04000E88 RID: 3720
			Nine,
			// Token: 0x04000E89 RID: 3721
			Keypad_0,
			// Token: 0x04000E8A RID: 3722
			Keypad_1,
			// Token: 0x04000E8B RID: 3723
			Keypad_2,
			// Token: 0x04000E8C RID: 3724
			Keypad_3,
			// Token: 0x04000E8D RID: 3725
			Keypad_4,
			// Token: 0x04000E8E RID: 3726
			Keypad_5,
			// Token: 0x04000E8F RID: 3727
			Keypad_6,
			// Token: 0x04000E90 RID: 3728
			Keypad_7,
			// Token: 0x04000E91 RID: 3729
			Keypad_8,
			// Token: 0x04000E92 RID: 3730
			Keypad_9,
			// Token: 0x04000E93 RID: 3731
			Keypad_Dot,
			// Token: 0x04000E94 RID: 3732
			Keypad_Slash,
			// Token: 0x04000E95 RID: 3733
			Keypad_Asterisk,
			// Token: 0x04000E96 RID: 3734
			Keypad_Minus,
			// Token: 0x04000E97 RID: 3735
			Keypad_Plus,
			// Token: 0x04000E98 RID: 3736
			Keypad_Enter,
			// Token: 0x04000E99 RID: 3737
			Keypad_Equals,
			// Token: 0x04000E9A RID: 3738
			Space,
			// Token: 0x04000E9B RID: 3739
			Backspace,
			// Token: 0x04000E9C RID: 3740
			Tab,
			// Token: 0x04000E9D RID: 3741
			Clear,
			// Token: 0x04000E9E RID: 3742
			Return,
			// Token: 0x04000E9F RID: 3743
			Pause,
			// Token: 0x04000EA0 RID: 3744
			Esc,
			// Token: 0x04000EA1 RID: 3745
			ExclamationMark,
			// Token: 0x04000EA2 RID: 3746
			DoubleQuote,
			// Token: 0x04000EA3 RID: 3747
			Hash,
			// Token: 0x04000EA4 RID: 3748
			Dollar,
			// Token: 0x04000EA5 RID: 3749
			Ampersand,
			// Token: 0x04000EA6 RID: 3750
			SingleQuote,
			// Token: 0x04000EA7 RID: 3751
			OpenParenthesis,
			// Token: 0x04000EA8 RID: 3752
			CloseParenthesis,
			// Token: 0x04000EA9 RID: 3753
			Asterisk,
			// Token: 0x04000EAA RID: 3754
			Plus,
			// Token: 0x04000EAB RID: 3755
			Comma,
			// Token: 0x04000EAC RID: 3756
			Minus,
			// Token: 0x04000EAD RID: 3757
			Dot,
			// Token: 0x04000EAE RID: 3758
			Slash,
			// Token: 0x04000EAF RID: 3759
			Colon,
			// Token: 0x04000EB0 RID: 3760
			Semicolon,
			// Token: 0x04000EB1 RID: 3761
			LessThan,
			// Token: 0x04000EB2 RID: 3762
			Equals,
			// Token: 0x04000EB3 RID: 3763
			GreaterThan,
			// Token: 0x04000EB4 RID: 3764
			QuestionMark,
			// Token: 0x04000EB5 RID: 3765
			At,
			// Token: 0x04000EB6 RID: 3766
			OpenBracket,
			// Token: 0x04000EB7 RID: 3767
			Backslash,
			// Token: 0x04000EB8 RID: 3768
			CloseBracket,
			// Token: 0x04000EB9 RID: 3769
			Caret,
			// Token: 0x04000EBA RID: 3770
			Underscore,
			// Token: 0x04000EBB RID: 3771
			BackQuote,
			// Token: 0x04000EBC RID: 3772
			Delete,
			// Token: 0x04000EBD RID: 3773
			UpArrow,
			// Token: 0x04000EBE RID: 3774
			DownArrow,
			// Token: 0x04000EBF RID: 3775
			RightArrow,
			// Token: 0x04000EC0 RID: 3776
			LeftArrow,
			// Token: 0x04000EC1 RID: 3777
			Insert,
			// Token: 0x04000EC2 RID: 3778
			Home,
			// Token: 0x04000EC3 RID: 3779
			End,
			// Token: 0x04000EC4 RID: 3780
			PageUp,
			// Token: 0x04000EC5 RID: 3781
			PageDown,
			// Token: 0x04000EC6 RID: 3782
			F1,
			// Token: 0x04000EC7 RID: 3783
			F2,
			// Token: 0x04000EC8 RID: 3784
			F3,
			// Token: 0x04000EC9 RID: 3785
			F4,
			// Token: 0x04000ECA RID: 3786
			F5,
			// Token: 0x04000ECB RID: 3787
			F6,
			// Token: 0x04000ECC RID: 3788
			F7,
			// Token: 0x04000ECD RID: 3789
			F8,
			// Token: 0x04000ECE RID: 3790
			F9,
			// Token: 0x04000ECF RID: 3791
			F10,
			// Token: 0x04000ED0 RID: 3792
			F11,
			// Token: 0x04000ED1 RID: 3793
			F12,
			// Token: 0x04000ED2 RID: 3794
			F13,
			// Token: 0x04000ED3 RID: 3795
			F14,
			// Token: 0x04000ED4 RID: 3796
			F15,
			// Token: 0x04000ED5 RID: 3797
			Numlock,
			// Token: 0x04000ED6 RID: 3798
			CapsLock,
			// Token: 0x04000ED7 RID: 3799
			ScrollLock,
			// Token: 0x04000ED8 RID: 3800
			RightShift,
			// Token: 0x04000ED9 RID: 3801
			LeftShift,
			// Token: 0x04000EDA RID: 3802
			RightControl,
			// Token: 0x04000EDB RID: 3803
			LeftControl,
			// Token: 0x04000EDC RID: 3804
			RightAlt,
			// Token: 0x04000EDD RID: 3805
			LeftAlt,
			// Token: 0x04000EDE RID: 3806
			RightCommand,
			// Token: 0x04000EDF RID: 3807
			LeftCommand,
			// Token: 0x04000EE0 RID: 3808
			LeftWindows,
			// Token: 0x04000EE1 RID: 3809
			RightWindows,
			// Token: 0x04000EE2 RID: 3810
			AltGr,
			// Token: 0x04000EE3 RID: 3811
			Help,
			// Token: 0x04000EE4 RID: 3812
			Print,
			// Token: 0x04000EE5 RID: 3813
			SysReq,
			// Token: 0x04000EE6 RID: 3814
			Break,
			// Token: 0x04000EE7 RID: 3815
			Menu,
			// Token: 0x04000EE8 RID: 3816
			Count,
			// Token: 0x04000EE9 RID: 3817
			Undefined
		}

		// Token: 0x02000121 RID: 289
		public enum InputActionRange
		{
			// Token: 0x04000EEB RID: 3819
			positive,
			// Token: 0x04000EEC RID: 3820
			negative
		}

		// Token: 0x02000122 RID: 290
		[Serializable]
		public class InputActionMap
		{
			// Token: 0x06000E5E RID: 3678 RVA: 0x0006AC78 File Offset: 0x00068E78
			public InputActionMap(int playerIndex, Controls.InputAction action, Controls.InputActionRange range, Controls.PlayerMapCollection playerMapCollection, bool updateIfNotPlaying)
			{
				this.ConstructorInitialization(playerIndex, action, range, playerMapCollection, updateIfNotPlaying);
			}

			// Token: 0x06000E5F RID: 3679 RVA: 0x00011923 File Offset: 0x0000FB23
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

			// Token: 0x06000E60 RID: 3680 RVA: 0x0006ACF4 File Offset: 0x00068EF4
			~InputActionMap()
			{
				this.MapCollectionRemoveFrom(this.myPlayerMapCollection);
			}

			// Token: 0x06000E61 RID: 3681 RVA: 0x0006AD28 File Offset: 0x00068F28
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

			// Token: 0x06000E62 RID: 3682 RVA: 0x0006ADB8 File Offset: 0x00068FB8
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

			// Token: 0x06000E63 RID: 3683 RVA: 0x0001195C File Offset: 0x0000FB5C
			public void ClearMaps(bool affectKeyboard, bool affectMouse, bool affectJoystick)
			{
				this.ElementKeyboard_Clear();
				this.ElementMouse_Clear();
				this.ElementJoystick_Clear();
			}

			// Token: 0x06000E64 RID: 3684 RVA: 0x0006AE54 File Offset: 0x00069054
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

			// Token: 0x06000E65 RID: 3685 RVA: 0x0006AEFC File Offset: 0x000690FC
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

			// Token: 0x1700009C RID: 156
			// (get) Token: 0x06000E66 RID: 3686 RVA: 0x00011970 File Offset: 0x0000FB70
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

			// Token: 0x06000E67 RID: 3687 RVA: 0x0001199C File Offset: 0x0000FB9C
			public void MapChangeableSet(bool value)
			{
				this._mapCanBeRemapped = value;
			}

			// Token: 0x06000E68 RID: 3688 RVA: 0x000119A5 File Offset: 0x0000FBA5
			public bool MapChangeableGet()
			{
				return this._mapCanBeRemapped;
			}

			// Token: 0x06000E69 RID: 3689 RVA: 0x0006AF70 File Offset: 0x00069170
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

			// Token: 0x06000E6A RID: 3690 RVA: 0x0006AFC4 File Offset: 0x000691C4
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

			// Token: 0x06000E6B RID: 3691 RVA: 0x0006B018 File Offset: 0x00069218
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

			// Token: 0x06000E6C RID: 3692 RVA: 0x0006B06C File Offset: 0x0006926C
			public void ElementKeyboard_Clear()
			{
				for (int i = 0; i < 4; i++)
				{
					this.keyboardElements[i] = (Controls.KeyboardElement)(-1);
				}
				this.keyboardElementsCount = 0;
			}

			// Token: 0x06000E6D RID: 3693 RVA: 0x0006B098 File Offset: 0x00069298
			public void ElementMouse_Clear()
			{
				for (int i = 0; i < 4; i++)
				{
					this.mouseElements[i] = (Controls.MouseElement)(-1);
				}
				this.mouseElementsCount = 0;
			}

			// Token: 0x06000E6E RID: 3694 RVA: 0x0006B0C4 File Offset: 0x000692C4
			public void ElementJoystick_Clear()
			{
				for (int i = 0; i < 4; i++)
				{
					this.joystickElements[i] = (Controls.JoystickElement)(-1);
				}
				this.joystickElementsCount = 0;
			}

			// Token: 0x06000E6F RID: 3695 RVA: 0x0006B0F0 File Offset: 0x000692F0
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

			// Token: 0x06000E70 RID: 3696 RVA: 0x0006B154 File Offset: 0x00069354
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

			// Token: 0x06000E71 RID: 3697 RVA: 0x0006B1B8 File Offset: 0x000693B8
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

			// Token: 0x06000E72 RID: 3698 RVA: 0x000119AD File Offset: 0x0000FBAD
			public bool HasNoElements()
			{
				return this.keyboardElementsCount == 0 && this.mouseElementsCount == 0 && this.joystickElementsCount == 0;
			}

			// Token: 0x06000E73 RID: 3699 RVA: 0x000119CA File Offset: 0x0000FBCA
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

			// Token: 0x06000E74 RID: 3700 RVA: 0x0006B21C File Offset: 0x0006941C
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

			// Token: 0x04000EED RID: 3821
			private Controls.PlayerExt _myPlayer;

			// Token: 0x04000EEE RID: 3822
			public int myPlayerIndex = -1;

			// Token: 0x04000EEF RID: 3823
			public bool updateIfNotPlaying;

			// Token: 0x04000EF0 RID: 3824
			[SerializeField]
			private bool _mapCanBeRemapped = true;

			// Token: 0x04000EF1 RID: 3825
			public Controls.InputAction myGameAction = Controls.InputAction._UNDEFINED;

			// Token: 0x04000EF2 RID: 3826
			public Controls.InputActionRange myGameActionRange;

			// Token: 0x04000EF3 RID: 3827
			private Controls.PlayerMapCollection myPlayerMapCollection;

			// Token: 0x04000EF4 RID: 3828
			[SerializeField]
			private float inputValue;

			// Token: 0x04000EF5 RID: 3829
			[SerializeField]
			private float inputValuePrevious;

			// Token: 0x04000EF6 RID: 3830
			[SerializeField]
			private Controls.KeyboardElement[] keyboardElements = new Controls.KeyboardElement[4];

			// Token: 0x04000EF7 RID: 3831
			[SerializeField]
			private int keyboardElementsCount;

			// Token: 0x04000EF8 RID: 3832
			[SerializeField]
			private Controls.MouseElement[] mouseElements = new Controls.MouseElement[4];

			// Token: 0x04000EF9 RID: 3833
			[SerializeField]
			private int mouseElementsCount;

			// Token: 0x04000EFA RID: 3834
			[SerializeField]
			private Controls.JoystickElement[] joystickElements = new Controls.JoystickElement[4];

			// Token: 0x04000EFB RID: 3835
			[SerializeField]
			private int joystickElementsCount;

			// Token: 0x04000EFC RID: 3836
			private List<Controls.KeyboardElement> elementsListChache_Keyboard = new List<Controls.KeyboardElement>();

			// Token: 0x04000EFD RID: 3837
			private List<Controls.MouseElement> elementsListChache_Mouse = new List<Controls.MouseElement>();

			// Token: 0x04000EFE RID: 3838
			private List<Controls.JoystickElement> elementsListChache_Joystick = new List<Controls.JoystickElement>();

			// Token: 0x04000EFF RID: 3839
			private bool justPressd;

			// Token: 0x04000F00 RID: 3840
			private bool justReleased;

			// Token: 0x04000F01 RID: 3841
			private bool _updateNoPlayerErrorShown;
		}

		// Token: 0x02000123 RID: 291
		[Serializable]
		public class PlayerMapCollection
		{
			// Token: 0x06000E75 RID: 3701 RVA: 0x0006B6C0 File Offset: 0x000698C0
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

			// Token: 0x06000E76 RID: 3702 RVA: 0x0006B6FC File Offset: 0x000698FC
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

			// Token: 0x04000F02 RID: 3842
			public Controls.InputActionMap[] maps = new Controls.InputActionMap[40];

			// Token: 0x04000F03 RID: 3843
			public int mapsCount;
		}

		// Token: 0x02000124 RID: 292
		public class PlayerChachedActionStates
		{
			// Token: 0x04000F04 RID: 3844
			public Dictionary<Controls.InputAction, bool> _gameActionState_JustPressed = new Dictionary<Controls.InputAction, bool>();

			// Token: 0x04000F05 RID: 3845
			public Dictionary<Controls.InputAction, bool> _gameActionState_JustReleased = new Dictionary<Controls.InputAction, bool>();

			// Token: 0x04000F06 RID: 3846
			public Dictionary<Controls.InputAction, bool> _gameActionState_Hold = new Dictionary<Controls.InputAction, bool>();

			// Token: 0x04000F07 RID: 3847
			public Dictionary<Controls.InputAction, bool> _gameActionState_HoldPrevious = new Dictionary<Controls.InputAction, bool>();

			// Token: 0x04000F08 RID: 3848
			public Dictionary<Controls.InputAction, float> _gameActionState_Axis = new Dictionary<Controls.InputAction, float>();

			// Token: 0x04000F09 RID: 3849
			public Dictionary<Controls.InputAction, float> _gameActionState_AxisPrevious = new Dictionary<Controls.InputAction, float>();
		}

		// Token: 0x02000125 RID: 293
		[Serializable]
		public class PlayerMapCollectionSerializer
		{
			// Token: 0x04000F0A RID: 3850
			public Controls.PlayerMapCollection[] mapsToSerialize = new Controls.PlayerMapCollection[1];
		}

		// Token: 0x02000126 RID: 294
		public class RemappingContext
		{
			// Token: 0x04000F0B RID: 3851
			public bool isRunnning;

			// Token: 0x04000F0C RID: 3852
			public Controls.InputKind remappingIputKind;

			// Token: 0x04000F0D RID: 3853
			public bool allowButtons = true;

			// Token: 0x04000F0E RID: 3854
			public bool allowAxes = true;

			// Token: 0x04000F0F RID: 3855
			public Controls.InputActionMap mapToRemap;

			// Token: 0x04000F10 RID: 3856
			public Controls.InputActionMap tempMap;
		}

		// Token: 0x02000127 RID: 295
		// (Invoke) Token: 0x06000E7C RID: 3708
		public delegate void MapCallback(Controls.InputActionMap map);

		// Token: 0x02000128 RID: 296
		public enum InputAction
		{
			// Token: 0x04000F12 RID: 3858
			menuMoveUp,
			// Token: 0x04000F13 RID: 3859
			menuMoveDown,
			// Token: 0x04000F14 RID: 3860
			menuMoveRight,
			// Token: 0x04000F15 RID: 3861
			menuMoveLeft,
			// Token: 0x04000F16 RID: 3862
			menuSelect,
			// Token: 0x04000F17 RID: 3863
			menuSelectNoMouse,
			// Token: 0x04000F18 RID: 3864
			menuBack,
			// Token: 0x04000F19 RID: 3865
			menuAnswerYes,
			// Token: 0x04000F1A RID: 3866
			menuAnswerNo,
			// Token: 0x04000F1B RID: 3867
			menuPause,
			// Token: 0x04000F1C RID: 3868
			menuTabLeft,
			// Token: 0x04000F1D RID: 3869
			menuTabRight,
			// Token: 0x04000F1E RID: 3870
			menuSocialButton,
			// Token: 0x04000F1F RID: 3871
			cameraUp,
			// Token: 0x04000F20 RID: 3872
			cameraDown,
			// Token: 0x04000F21 RID: 3873
			cameraLeft,
			// Token: 0x04000F22 RID: 3874
			cameraRight,
			// Token: 0x04000F23 RID: 3875
			scrollUp,
			// Token: 0x04000F24 RID: 3876
			scrollDown,
			// Token: 0x04000F25 RID: 3877
			interact,
			// Token: 0x04000F26 RID: 3878
			moveUp,
			// Token: 0x04000F27 RID: 3879
			moveDown,
			// Token: 0x04000F28 RID: 3880
			moveLeft,
			// Token: 0x04000F29 RID: 3881
			moveRight,
			// Token: 0x04000F2A RID: 3882
			gameEmptyAction4,
			// Token: 0x04000F2B RID: 3883
			gameEmptyAction3,
			// Token: 0x04000F2C RID: 3884
			gameEmptyAction2,
			// Token: 0x04000F2D RID: 3885
			gameEmptyAction1,
			// Token: 0x04000F2E RID: 3886
			_COUNT,
			// Token: 0x04000F2F RID: 3887
			_UNDEFINED
		}
	}
}
