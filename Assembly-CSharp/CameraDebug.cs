using System;
using Panik;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class CameraDebug : MonoBehaviour
{
	// Token: 0x0600004F RID: 79 RVA: 0x000187FC File Offset: 0x000169FC
	private void RecordTrackPoint()
	{
		switch (this.recTrackN)
		{
		case 0:
			Array.Resize<Vector4>(ref this.positionsAndTime_0, this.positionsAndTime_0.Length + 1);
			Array.Resize<Vector4>(ref this.rotationsAndTime_0, this.rotationsAndTime_0.Length + 1);
			this.positionsAndTime_0[this.positionsAndTime_0.Length - 1] = new Vector4(base.transform.position.x, base.transform.position.y, base.transform.position.z, 1f);
			this.rotationsAndTime_0[this.rotationsAndTime_0.Length - 1] = new Vector4(base.transform.eulerAngles.x, base.transform.eulerAngles.y, base.transform.eulerAngles.z, 1f);
			return;
		case 1:
			Array.Resize<Vector4>(ref this.positionsAndTime_1, this.positionsAndTime_1.Length + 1);
			Array.Resize<Vector4>(ref this.rotationsAndTime_1, this.rotationsAndTime_1.Length + 1);
			this.positionsAndTime_1[this.positionsAndTime_1.Length - 1] = new Vector4(base.transform.position.x, base.transform.position.y, base.transform.position.z, 1f);
			this.rotationsAndTime_1[this.rotationsAndTime_1.Length - 1] = new Vector4(base.transform.eulerAngles.x, base.transform.eulerAngles.y, base.transform.eulerAngles.z, 1f);
			return;
		case 2:
			Array.Resize<Vector4>(ref this.positionsAndTime_2, this.positionsAndTime_2.Length + 1);
			Array.Resize<Vector4>(ref this.rotationsAndTime_2, this.rotationsAndTime_2.Length + 1);
			this.positionsAndTime_2[this.positionsAndTime_2.Length - 1] = new Vector4(base.transform.position.x, base.transform.position.y, base.transform.position.z, 1f);
			this.rotationsAndTime_2[this.rotationsAndTime_2.Length - 1] = new Vector4(base.transform.eulerAngles.x, base.transform.eulerAngles.y, base.transform.eulerAngles.z, 1f);
			return;
		case 3:
			Array.Resize<Vector4>(ref this.positionsAndTime_3, this.positionsAndTime_3.Length + 1);
			Array.Resize<Vector4>(ref this.rotationsAndTime_3, this.rotationsAndTime_3.Length + 1);
			this.positionsAndTime_3[this.positionsAndTime_3.Length - 1] = new Vector4(base.transform.position.x, base.transform.position.y, base.transform.position.z, 1f);
			this.rotationsAndTime_3[this.rotationsAndTime_3.Length - 1] = new Vector4(base.transform.eulerAngles.x, base.transform.eulerAngles.y, base.transform.eulerAngles.z, 1f);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00018B4C File Offset: 0x00016D4C
	private void RecordTrackReset()
	{
		switch (this.recTrackN)
		{
		case 0:
			this.positionsAndTime_0 = new Vector4[0];
			this.rotationsAndTime_0 = new Vector4[0];
			break;
		case 1:
			this.positionsAndTime_1 = new Vector4[0];
			this.rotationsAndTime_1 = new Vector4[0];
			break;
		case 2:
			this.positionsAndTime_2 = new Vector4[0];
			this.rotationsAndTime_2 = new Vector4[0];
			break;
		case 3:
			this.positionsAndTime_3 = new Vector4[0];
			this.rotationsAndTime_3 = new Vector4[0];
			break;
		}
		this.recTrackN = -1;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x0000775F File Offset: 0x0000595F
	public static bool IsEnabled()
	{
		return !(CameraDebug.instance == null) && CameraDebug.instance.mode > CameraDebug.Mode.disabled;
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00018BE8 File Offset: 0x00016DE8
	public static void SetMode(CameraDebug.Mode mode)
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		if (CameraDebug.instance == null)
		{
			Debug.LogError("CameraDebug.SetMode() - instance is null");
			return;
		}
		CameraDebug.Mode mode2 = CameraDebug.instance.mode;
		CameraDebug.instance.ResetTrack();
		switch (mode2)
		{
		default:
			switch (mode)
			{
			case CameraDebug.Mode.free:
				CameraDebug.instance.freePosition = CameraGame.list[0].transform.position;
				CameraDebug.instance.freeRotation = CameraGame.list[0].transform.eulerAngles;
				break;
			case CameraDebug.Mode.track0:
				CameraDebug.instance.transform.position = new Vector3(CameraDebug.instance.positionsAndTime_0[0].x, CameraDebug.instance.positionsAndTime_0[0].y, CameraDebug.instance.positionsAndTime_0[0].z);
				CameraDebug.instance.transform.eulerAngles = new Vector3(CameraDebug.instance.rotationsAndTime_0[0].x, CameraDebug.instance.rotationsAndTime_0[0].y, CameraDebug.instance.rotationsAndTime_0[0].z);
				break;
			case CameraDebug.Mode.track1:
				CameraDebug.instance.transform.position = new Vector3(CameraDebug.instance.positionsAndTime_1[0].x, CameraDebug.instance.positionsAndTime_1[0].y, CameraDebug.instance.positionsAndTime_1[0].z);
				CameraDebug.instance.transform.eulerAngles = new Vector3(CameraDebug.instance.rotationsAndTime_1[0].x, CameraDebug.instance.rotationsAndTime_1[0].y, CameraDebug.instance.rotationsAndTime_1[0].z);
				break;
			case CameraDebug.Mode.track2:
				CameraDebug.instance.transform.position = new Vector3(CameraDebug.instance.positionsAndTime_2[0].x, CameraDebug.instance.positionsAndTime_2[0].y, CameraDebug.instance.positionsAndTime_2[0].z);
				CameraDebug.instance.transform.eulerAngles = new Vector3(CameraDebug.instance.rotationsAndTime_2[0].x, CameraDebug.instance.rotationsAndTime_2[0].y, CameraDebug.instance.rotationsAndTime_2[0].z);
				break;
			case CameraDebug.Mode.track3:
				CameraDebug.instance.transform.position = new Vector3(CameraDebug.instance.positionsAndTime_3[0].x, CameraDebug.instance.positionsAndTime_3[0].y, CameraDebug.instance.positionsAndTime_3[0].z);
				CameraDebug.instance.transform.eulerAngles = new Vector3(CameraDebug.instance.rotationsAndTime_3[0].x, CameraDebug.instance.rotationsAndTime_3[0].y, CameraDebug.instance.rotationsAndTime_3[0].z);
				break;
			}
			CameraDebug.instance.mode = mode;
			return;
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x0000777D File Offset: 0x0000597D
	public static void Close()
	{
		CameraDebug.SetMode(CameraDebug.Mode.disabled);
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00007785 File Offset: 0x00005985
	private void ResetTrack()
	{
		this.trackIndex_Positions = 0;
		this.trackIndex_Rotations = 0;
		this.trackTimer_Positions = 0f;
		this.trackTimer_Rotations = 0f;
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00018F60 File Offset: 0x00017160
	private void UpdateTrack(ref Vector4[] freePositions, ref Vector4[] freeRotations)
	{
		if (freePositions.Length == 0 || freeRotations.Length == 0)
		{
			Debug.LogError("CameraDebug.UpdateTrack() - no positions or no rotations for mode: " + this.mode.ToString());
			CameraDebug.SetMode(CameraDebug.Mode.free);
			return;
		}
		if (freePositions.Length == 1 || freeRotations.Length == 1)
		{
			Debug.LogError("CameraDebug.UpdateTrack() - just 1 position or rotation for mode: " + this.mode.ToString());
			CameraDebug.SetMode(CameraDebug.Mode.free);
			return;
		}
		bool flag = false;
		bool flag2 = false;
		if (this.trackIndex_Positions >= freePositions.Length - 1)
		{
			flag = true;
		}
		else
		{
			Vector3 vector = new Vector3(freePositions[this.trackIndex_Positions].x, freePositions[this.trackIndex_Positions].y, freePositions[this.trackIndex_Positions].z);
			Vector3 vector2 = new Vector3(freePositions[this.trackIndex_Positions + 1].x, freePositions[this.trackIndex_Positions + 1].y, freePositions[this.trackIndex_Positions + 1].z);
			float w = freePositions[this.trackIndex_Positions + 1].w;
			this.trackTimer_Positions += Tick.Time * this.speedMultiplier / w;
			if (this.trackTimer_Positions >= 1f)
			{
				this.trackTimer_Positions -= 1f;
				this.trackIndex_Positions++;
				if (this.trackIndex_Positions >= freePositions.Length - 1 && (base.transform.position - vector2).magnitude < 0.1f)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				base.transform.position = Vector3.Lerp(vector, vector2, this.trackTimer_Positions);
			}
		}
		if (this.trackIndex_Rotations >= freeRotations.Length - 1)
		{
			flag2 = true;
		}
		else
		{
			Vector3 vector3 = new Vector3(freeRotations[this.trackIndex_Rotations].x, freeRotations[this.trackIndex_Rotations].y, freeRotations[this.trackIndex_Rotations].z);
			Vector3 vector4 = new Vector3(freeRotations[this.trackIndex_Rotations + 1].x, freeRotations[this.trackIndex_Rotations + 1].y, freeRotations[this.trackIndex_Rotations + 1].z);
			if (vector4.x > vector3.x + 180f)
			{
				vector4.x -= 360f;
			}
			if (vector4.x < vector3.x - 180f)
			{
				vector4.x += 360f;
			}
			if (vector4.y > vector3.y + 180f)
			{
				vector4.y -= 360f;
			}
			if (vector4.y < vector3.y - 180f)
			{
				vector4.y += 360f;
			}
			if (vector4.z > vector3.z + 180f)
			{
				vector4.z -= 360f;
			}
			if (vector4.z < vector3.z - 180f)
			{
				vector4.z += 360f;
			}
			float w2 = freeRotations[this.trackIndex_Rotations + 1].w;
			this.trackTimer_Rotations += Tick.Time * this.speedMultiplier / w2;
			if (this.trackTimer_Rotations >= 1f)
			{
				this.trackTimer_Rotations -= 1f;
				this.trackIndex_Rotations++;
				if (this.trackIndex_Rotations >= freeRotations.Length - 1 && (base.transform.eulerAngles - vector4).magnitude < 0.1f)
				{
					flag2 = true;
				}
			}
			if (!flag2)
			{
				base.transform.eulerAngles = Vector3.Lerp(vector3, vector4, this.trackTimer_Rotations);
			}
		}
		if (flag && flag2)
		{
			CameraDebug.SetMode(CameraDebug.Mode.free);
			return;
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x000077AB File Offset: 0x000059AB
	private void Awake()
	{
		CameraDebug.instance = this;
		this.myCamera = base.GetComponent<Camera>();
	}

	// Token: 0x06000057 RID: 87 RVA: 0x000077BF File Offset: 0x000059BF
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
	}

	// Token: 0x06000058 RID: 88 RVA: 0x000077CD File Offset: 0x000059CD
	private void OnDestroy()
	{
		if (CameraDebug.instance == this)
		{
			CameraDebug.instance = null;
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x0001934C File Offset: 0x0001754C
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		if (this.mode == CameraDebug.Mode.disabled)
		{
			return;
		}
		if (Controls.KeyboardButton_PressedGet(0, Controls.KeyboardElement.F1) || Controls.KeyboardButton_PressedGet(0, Controls.KeyboardElement.One) || Controls.JoystickButton_PressedGet(0, Controls.JoystickElement.DPadRight))
		{
			CameraDebug.SetMode(CameraDebug.Mode.track0);
		}
		if (Controls.KeyboardButton_PressedGet(0, Controls.KeyboardElement.F2) || Controls.KeyboardButton_PressedGet(0, Controls.KeyboardElement.Two) || Controls.JoystickButton_PressedGet(0, Controls.JoystickElement.DPadDown))
		{
			CameraDebug.SetMode(CameraDebug.Mode.track1);
		}
		if (Controls.KeyboardButton_PressedGet(0, Controls.KeyboardElement.F3) || Controls.KeyboardButton_PressedGet(0, Controls.KeyboardElement.Three) || Controls.JoystickButton_PressedGet(0, Controls.JoystickElement.DPadLeft))
		{
			CameraDebug.SetMode(CameraDebug.Mode.track2);
		}
		if (Controls.KeyboardButton_PressedGet(0, Controls.KeyboardElement.F4) || Controls.KeyboardButton_PressedGet(0, Controls.KeyboardElement.Four) || Controls.JoystickButton_PressedGet(0, Controls.JoystickElement.DPadUp))
		{
			CameraDebug.SetMode(CameraDebug.Mode.track3);
		}
		if (Controls.KeyboardButton_PressedGet(0, Controls.KeyboardElement.F10) || Controls.KeyboardButton_PressedGet(0, Controls.KeyboardElement.Zero) || Controls.JoystickButton_PressedGet(0, Controls.JoystickElement.Select))
		{
			CameraDebug.SetMode(CameraDebug.Mode.free);
		}
		bool flag = Controls.KeyboardButton_HoldGet(0, Controls.KeyboardElement.Equals) || Controls.KeyboardButton_HoldGet(0, Controls.KeyboardElement.Keypad_Equals) || Controls.JoystickButton_HoldGet(0, Controls.JoystickElement.RightStickButton);
		bool flag2 = Controls.KeyboardButton_HoldGet(0, Controls.KeyboardElement.Minus) || Controls.KeyboardButton_HoldGet(0, Controls.KeyboardElement.Keypad_Minus) || Controls.JoystickButton_HoldGet(0, Controls.JoystickElement.LeftShoulder);
		if (Controls.KeyboardButton_HoldGet(0, Controls.KeyboardElement.Plus) || Controls.KeyboardButton_HoldGet(0, Controls.KeyboardElement.Keypad_Plus) || Controls.JoystickButton_HoldGet(0, Controls.JoystickElement.RightShoulder))
		{
			this.speedMultiplier += Tick.Time * 0.5f;
		}
		if (flag2)
		{
			this.speedMultiplier -= Tick.Time * 0.5f;
		}
		if (flag)
		{
			this.speedMultiplier = 1f;
		}
		this.speedMultiplier = Mathf.Clamp(this.speedMultiplier, 0.1f, 5f);
		Vector2 zero = Vector2.zero;
		float num = 0f;
		Vector2 zero2 = Vector2.zero;
		float num2 = 1f;
		zero.x = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.moveRight, Controls.InputAction.moveLeft, true);
		zero.y = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.moveUp, Controls.InputAction.moveDown, true);
		if (Controls.KeyboardButton_HoldGet(0, Controls.KeyboardElement.E) || Controls.JoystickButton_HoldGet(0, Controls.JoystickElement.RightTrigger))
		{
			num += 1f;
		}
		if (Controls.KeyboardButton_HoldGet(0, Controls.KeyboardElement.Q) || Controls.JoystickButton_HoldGet(0, Controls.JoystickElement.LeftTrigger))
		{
			num -= 1f;
		}
		zero2.x = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.cameraRight, Controls.InputAction.cameraLeft, true);
		zero2.y = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.cameraUp, Controls.InputAction.cameraDown, true);
		if (Controls.KeyboardButton_PressedGet(0, Controls.KeyboardElement.LeftShift) || Controls.JoystickButton_HoldGet(0, Controls.JoystickElement.LeftStickButton))
		{
			num2 = 2f;
		}
		switch (this.mode)
		{
		case CameraDebug.Mode.disabled:
			break;
		case CameraDebug.Mode.free:
		{
			float num3 = 90f;
			if (this.player.lastInputKindUsed == Controls.InputKind.Joystick)
			{
				num3 = 90f;
			}
			this.freeRotation.x = this.freeRotation.x - zero2.y * Tick.Time * num3 * this.speedMultiplier;
			this.freeRotation.y = this.freeRotation.y + zero2.x * Tick.Time * num3 * this.speedMultiplier;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			if (zero.magnitude != 0f)
			{
				float magnitude = zero.magnitude;
				float num4 = this.freeRotation.y + Util.AxisToAngle2D(zero.x, -zero.y);
				float num5 = -this.freeRotation.x * zero.y;
				vector = Util.AngleToAxis3D(num4, num5) * magnitude;
			}
			if (num != 0f)
			{
				vector2 = Util.AngleToAxis3D(this.freeRotation.y - 90f * num, this.freeRotation.x + 90f * num) * Mathf.Abs(num);
			}
			this.freePosition += (vector + vector2) * Tick.Time * this.freeMovementSpeed * this.speedMultiplier * num2;
			if (this.freeRotation.x > base.transform.eulerAngles.x + 180f)
			{
				this.freeRotation.x = this.freeRotation.x - 360f;
			}
			if (this.freeRotation.x < base.transform.eulerAngles.x - 180f)
			{
				this.freeRotation.x = this.freeRotation.x + 360f;
			}
			if (this.freeRotation.y > base.transform.eulerAngles.y + 180f)
			{
				this.freeRotation.y = this.freeRotation.y - 360f;
			}
			if (this.freeRotation.y < base.transform.eulerAngles.y - 180f)
			{
				this.freeRotation.y = this.freeRotation.y + 360f;
			}
			if (this.freeRotation.z > base.transform.eulerAngles.z + 180f)
			{
				this.freeRotation.z = this.freeRotation.z - 360f;
			}
			if (this.freeRotation.z < base.transform.eulerAngles.z - 180f)
			{
				this.freeRotation.z = this.freeRotation.z + 360f;
			}
			base.transform.eulerAngles = this.freeRotation;
			base.transform.position = this.freePosition;
			return;
		}
		case CameraDebug.Mode.track0:
			this.UpdateTrack(ref this.positionsAndTime_0, ref this.rotationsAndTime_0);
			return;
		case CameraDebug.Mode.track1:
			this.UpdateTrack(ref this.positionsAndTime_1, ref this.rotationsAndTime_1);
			return;
		case CameraDebug.Mode.track2:
			this.UpdateTrack(ref this.positionsAndTime_2, ref this.rotationsAndTime_2);
			return;
		case CameraDebug.Mode.track3:
			this.UpdateTrack(ref this.positionsAndTime_3, ref this.rotationsAndTime_3);
			break;
		default:
			return;
		}
	}

	// Token: 0x0600005A RID: 90 RVA: 0x000198B4 File Offset: 0x00017AB4
	private void OnDrawGizmosSelected()
	{
		if (this.myCamera == null)
		{
			this.myCamera = base.GetComponent<Camera>();
		}
		Gizmos.color = Color.green;
		for (int i = 0; i < this.positionsAndTime_0.Length; i++)
		{
			Vector3 vector2;
			Vector3 vector = (vector2 = new Vector3(this.positionsAndTime_0[i].x, this.positionsAndTime_0[i].y, this.positionsAndTime_0[i].z));
			Vector3 vector3 = new Vector3(this.rotationsAndTime_0[i].x, this.rotationsAndTime_0[i].y, this.rotationsAndTime_0[i].z);
			if (i < this.positionsAndTime_0.Length - 1)
			{
				vector2 = new Vector3(this.positionsAndTime_0[i + 1].x, this.positionsAndTime_0[i + 1].y, this.positionsAndTime_0[i + 1].z);
			}
			Gizmos.DrawSphere(vector, 0.1f);
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawSphere(vector + Util.AngleToAxis3D(vector3.y - 90f, -vector3.x) * 0.15f, 0.05f);
		}
		Gizmos.color = Color.blue;
		for (int j = 0; j < this.positionsAndTime_1.Length; j++)
		{
			Vector3 vector5;
			Vector3 vector4 = (vector5 = new Vector3(this.positionsAndTime_1[j].x, this.positionsAndTime_1[j].y, this.positionsAndTime_1[j].z));
			Vector3 vector6 = new Vector3(this.rotationsAndTime_1[j].x, this.rotationsAndTime_1[j].y, this.rotationsAndTime_1[j].z);
			if (j < this.positionsAndTime_1.Length - 1)
			{
				vector5 = new Vector3(this.positionsAndTime_1[j + 1].x, this.positionsAndTime_1[j + 1].y, this.positionsAndTime_1[j + 1].z);
			}
			Gizmos.DrawSphere(vector4, 0.1f);
			Gizmos.DrawLine(vector4, vector5);
			Gizmos.DrawSphere(vector4 + Util.AngleToAxis3D(vector6.y - 90f, -vector6.x) * 0.15f, 0.05f);
		}
		Gizmos.color = Color.red;
		for (int k = 0; k < this.positionsAndTime_2.Length; k++)
		{
			Vector3 vector8;
			Vector3 vector7 = (vector8 = new Vector3(this.positionsAndTime_2[k].x, this.positionsAndTime_2[k].y, this.positionsAndTime_2[k].z));
			Vector3 vector9 = new Vector3(this.rotationsAndTime_2[k].x, this.rotationsAndTime_2[k].y, this.rotationsAndTime_2[k].z);
			if (k < this.positionsAndTime_2.Length - 1)
			{
				vector8 = new Vector3(this.positionsAndTime_2[k + 1].x, this.positionsAndTime_2[k + 1].y, this.positionsAndTime_2[k + 1].z);
			}
			Gizmos.DrawSphere(vector7, 0.1f);
			Gizmos.DrawLine(vector7, vector8);
			Gizmos.DrawSphere(vector7 + Util.AngleToAxis3D(vector9.y - 90f, -vector9.x) * 0.15f, 0.05f);
		}
		Gizmos.color = Color.yellow;
		for (int l = 0; l < this.positionsAndTime_3.Length; l++)
		{
			Vector3 vector11;
			Vector3 vector10 = (vector11 = new Vector3(this.positionsAndTime_3[l].x, this.positionsAndTime_3[l].y, this.positionsAndTime_3[l].z));
			Vector3 vector12 = new Vector3(this.rotationsAndTime_3[l].x, this.rotationsAndTime_3[l].y, this.rotationsAndTime_3[l].z);
			if (l < this.positionsAndTime_3.Length - 1)
			{
				vector11 = new Vector3(this.positionsAndTime_3[l + 1].x, this.positionsAndTime_3[l + 1].y, this.positionsAndTime_3[l + 1].z);
			}
			Gizmos.DrawSphere(vector10, 0.1f);
			Gizmos.DrawLine(vector10, vector11);
			Gizmos.DrawSphere(vector10 + Util.AngleToAxis3D(vector12.y - 90f, -vector12.x) * 0.15f, 0.05f);
		}
	}

	// Token: 0x040000A4 RID: 164
	public static CameraDebug instance;

	// Token: 0x040000A5 RID: 165
	private const int PLAYER_INDEX = 0;

	// Token: 0x040000A6 RID: 166
	private const float SPEED_MULTIPLIER_VARIATION_SPEED = 0.5f;

	// Token: 0x040000A7 RID: 167
	private const float GAMEPAD_CAMERA_MULTIPLIER = 90f;

	// Token: 0x040000A8 RID: 168
	private const float MOUSE_CAMERA_MULTIPLIER = 90f;

	// Token: 0x040000A9 RID: 169
	public int recTrackN = -1;

	// Token: 0x040000AA RID: 170
	public bool recordTrackPoint;

	// Token: 0x040000AB RID: 171
	public bool recordTrackReset;

	// Token: 0x040000AC RID: 172
	public Camera myCamera;

	// Token: 0x040000AD RID: 173
	private Controls.PlayerExt player;

	// Token: 0x040000AE RID: 174
	public Vector4[] positionsAndTime_0;

	// Token: 0x040000AF RID: 175
	public Vector4[] rotationsAndTime_0;

	// Token: 0x040000B0 RID: 176
	public Vector4[] positionsAndTime_1;

	// Token: 0x040000B1 RID: 177
	public Vector4[] rotationsAndTime_1;

	// Token: 0x040000B2 RID: 178
	public Vector4[] positionsAndTime_2;

	// Token: 0x040000B3 RID: 179
	public Vector4[] rotationsAndTime_2;

	// Token: 0x040000B4 RID: 180
	public Vector4[] positionsAndTime_3;

	// Token: 0x040000B5 RID: 181
	public Vector4[] rotationsAndTime_3;

	// Token: 0x040000B6 RID: 182
	public CameraDebug.Mode mode;

	// Token: 0x040000B7 RID: 183
	private float speedMultiplier = 1f;

	// Token: 0x040000B8 RID: 184
	private int trackIndex_Positions;

	// Token: 0x040000B9 RID: 185
	private int trackIndex_Rotations;

	// Token: 0x040000BA RID: 186
	private float trackTimer_Positions;

	// Token: 0x040000BB RID: 187
	private float trackTimer_Rotations;

	// Token: 0x040000BC RID: 188
	private Vector3 freePosition;

	// Token: 0x040000BD RID: 189
	private Vector3 freeRotation;

	// Token: 0x040000BE RID: 190
	private float freeMovementSpeed = 4f;

	// Token: 0x040000BF RID: 191
	private float freeRotationSpeed = 1f;

	// Token: 0x0200000C RID: 12
	public enum Mode
	{
		// Token: 0x040000C1 RID: 193
		disabled,
		// Token: 0x040000C2 RID: 194
		free,
		// Token: 0x040000C3 RID: 195
		track0,
		// Token: 0x040000C4 RID: 196
		track1,
		// Token: 0x040000C5 RID: 197
		track2,
		// Token: 0x040000C6 RID: 198
		track3
	}
}
