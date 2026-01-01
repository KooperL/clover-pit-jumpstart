using System;
using Panik;
using TMPro;
using UnityEngine;

// Token: 0x020000EC RID: 236
public class SeedMenuScript : MonoBehaviour
{
	// Token: 0x06000BCA RID: 3018 RVA: 0x0000FB29 File Offset: 0x0000DD29
	public static bool IsEnabled()
	{
		return !(SeedMenuScript.instance == null) && SeedMenuScript.instance.holder.activeSelf;
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x0005E988 File Offset: 0x0005CB88
	public static void Open()
	{
		SeedMenuScript.instance.holder.SetActive(true);
		Sound.Play_Unpausable("SoundMenuPopUp", 1f, 1f);
		for (int i = 0; i < SeedMenuScript.instance.buttons.Length; i++)
		{
			SeedMenuScript.instance.buttons[i].HoveredSet(false);
		}
		SeedMenuScript.instance.currentSeedMenuButton = SeedMenuScript.instance.initialButton;
	}

	// Token: 0x06000BCC RID: 3020 RVA: 0x0005E9F8 File Offset: 0x0005CBF8
	private static void Close()
	{
		SeedMenuScript.instance.holder.SetActive(false);
		SeedMenuScript.instance.seedInputNum = 0U;
		for (int i = 0; i < SeedMenuScript.instance.cypherButtonsInDecimalOrder.Length; i++)
		{
			SeedMenuScript.instance.cypherButtonsInDecimalOrder[i].SetNumber(0U);
		}
	}

	// Token: 0x06000BCD RID: 3021 RVA: 0x0000FB49 File Offset: 0x0000DD49
	private void TextUpdate()
	{
		this.titleText.text = Translation.Get("MENU_SEED_INPUT_TITLE");
		this.descriptionText.text = Translation.Get("MENU_SEED_INPUT_DESCRIPTION");
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x0000FB75 File Offset: 0x0000DD75
	private void Awake()
	{
		SeedMenuScript.instance = this;
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x0000FB7D File Offset: 0x0000DD7D
	private void Start()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x0000FB8B File Offset: 0x0000DD8B
	private void OnEnable()
	{
		this.TextUpdate();
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x0000FB93 File Offset: 0x0000DD93
	private void OnDestroy()
	{
		if (SeedMenuScript.instance == this)
		{
			SeedMenuScript.instance = null;
		}
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x0005EA4C File Offset: 0x0005CC4C
	private void Update()
	{
		if (!SeedMenuScript.IsEnabled())
		{
			return;
		}
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		zero2.x = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.menuMoveRight, Controls.InputAction.menuMoveLeft, true);
		zero2.y = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.menuMoveUp, Controls.InputAction.menuMoveDown, true);
		if (zero2.x > 0.35f && this.axisRawPrevious.x <= 0.35f)
		{
			zero.x += 1f;
		}
		if (zero2.x < -0.35f && this.axisRawPrevious.x >= -0.35f)
		{
			zero.x -= 1f;
		}
		if (zero2.y > 0.35f && this.axisRawPrevious.y <= 0.35f)
		{
			zero.y += 1f;
		}
		if (zero2.y < -0.35f && this.axisRawPrevious.y >= -0.35f)
		{
			zero.y -= 1f;
		}
		this.axisRawPrevious = zero2;
		if (Controls.JoystickButton_PressedGet(0, Controls.JoystickElement.LeftShoulder))
		{
			zero.x -= 1f;
		}
		if (Controls.JoystickButton_PressedGet(0, Controls.JoystickElement.RightShoulder))
		{
			zero.x += 1f;
		}
		bool flag = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true);
		bool keyDown = Input.GetKeyDown(KeyCode.Backspace);
		bool flag2 = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true) && !keyDown;
		bool flag3 = false;
		float num = Controls.MouseAxis_ValueGet(0, Controls.MouseElement.axisScrollWheelVertical);
		if (num > 0f)
		{
			zero.y -= 1f;
		}
		if (num < 0f)
		{
			zero.y += 1f;
		}
		if (Controls.MouseButton_PressedGet(0, Controls.MouseElement.LeftButton))
		{
			zero.y += 1f;
		}
		if (Controls.MouseButton_PressedGet(0, Controls.MouseElement.RightButton))
		{
			zero.y -= 1f;
		}
		Vector2 vector;
		vector.x = (float)CameraUiGlobal.instance.myCamera.pixelWidth;
		vector.y = (float)CameraUiGlobal.instance.myCamera.pixelHeight;
		Vector2 vector2 = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, vector);
		bool flag4 = VirtualCursors.IsCursorVisible(0, true);
		char c = 'A';
		if (Input.inputString.Length > 0)
		{
			c = Input.inputString[Input.inputString.Length - 1];
		}
		int num2 = -1;
		switch (c)
		{
		case '0':
			num2 = 0;
			break;
		case '1':
			num2 = 1;
			break;
		case '2':
			num2 = 2;
			break;
		case '3':
			num2 = 3;
			break;
		case '4':
			num2 = 4;
			break;
		case '5':
			num2 = 5;
			break;
		case '6':
			num2 = 6;
			break;
		case '7':
			num2 = 7;
			break;
		case '8':
			num2 = 8;
			break;
		case '9':
			num2 = 9;
			break;
		}
		if (flag4)
		{
			this.currentSeedMenuButton = null;
		}
		for (int i = 0; i < this.buttons.Length; i++)
		{
			SeedMenuButton seedMenuButton = this.buttons[i];
			if (VirtualCursors.IsCursorVisible(0, true))
			{
				if (vector2.x > seedMenuButton.rectTransform.anchoredPosition.x - seedMenuButton.rectTransform.sizeDelta.x / 2f && vector2.x < seedMenuButton.rectTransform.anchoredPosition.x + seedMenuButton.rectTransform.sizeDelta.x / 2f && vector2.y > seedMenuButton.rectTransform.anchoredPosition.y - seedMenuButton.rectTransform.sizeDelta.y / 2f && vector2.y < seedMenuButton.rectTransform.anchoredPosition.y + seedMenuButton.rectTransform.sizeDelta.y / 2f)
				{
					this.currentSeedMenuButton = seedMenuButton;
				}
			}
			else if (this.currentSeedMenuButton == null)
			{
				if (flag || zero.x != 0f)
				{
					this.currentSeedMenuButton = this.initialButton;
					flag3 = true;
				}
				zero.x = 0f;
			}
			else if (this.currentSeedMenuButton == seedMenuButton)
			{
				if (zero.x < 0f)
				{
					if (this.currentSeedMenuButton.leftButton != null)
					{
						this.currentSeedMenuButton = this.currentSeedMenuButton.leftButton;
					}
					else
					{
						Sound.Play_Unpausable("SoundMenuError", 1f, 1f);
					}
					zero.x = 0f;
				}
				if (zero.x > 0f)
				{
					if (this.currentSeedMenuButton.rightButton != null)
					{
						this.currentSeedMenuButton = this.currentSeedMenuButton.rightButton;
					}
					else
					{
						Sound.Play_Unpausable("SoundMenuError", 1f, 1f);
					}
					zero.x = 0f;
				}
			}
			bool flag5 = seedMenuButton == this.currentSeedMenuButton;
			if (seedMenuButton.IsHovered() != flag5)
			{
				if (flag5)
				{
					Sound.Play_Unpausable("SoundSeedSelectionChange", 1f, 1f);
				}
				seedMenuButton.HoveredSet(flag5);
			}
		}
		if (flag2)
		{
			if (this.currentSeedMenuButton != this.backButton)
			{
				SeedMenuButton seedMenuButton2 = this.currentSeedMenuButton;
				if (seedMenuButton2 != null)
				{
					seedMenuButton2.HoveredSet(false);
				}
				this.currentSeedMenuButton = this.backButton;
				this.currentSeedMenuButton.HoveredSet(true);
				Sound.Play_Unpausable("SoundSeedSelectionChange", 1f, 1f);
			}
			else
			{
				this.Back();
			}
		}
		bool flag6 = false;
		if (flag && !flag3 && this.currentSeedMenuButton != null)
		{
			if (this.currentSeedMenuButton == this.backButton)
			{
				this.Back();
				return;
			}
			if (this.currentSeedMenuButton == this.okButton)
			{
				this.ConfirmSeed();
				return;
			}
			long num3 = (long)((ulong)this.seedInputNum);
			num3 += (long)((ulong)this.currentSeedMenuButton.decimalPlaceN);
			if (num3 > (long)((ulong)(-1)))
			{
				flag6 = true;
				num3 = (long)((ulong)(-1));
			}
			this.seedInputNum = (uint)num3;
			uint num4 = this.seedInputNum / this.currentSeedMenuButton.decimalPlaceN % 10U;
			this.currentSeedMenuButton.SetNumber(num4);
		}
		if (zero.y != 0f && this.currentSeedMenuButton != null && this.currentSeedMenuButton.acceptNumbers)
		{
			if (zero.y > 0f)
			{
				long num5 = (long)((ulong)this.seedInputNum);
				num5 += (long)((ulong)this.currentSeedMenuButton.decimalPlaceN);
				if (num5 > (long)((ulong)(-1)))
				{
					flag6 = true;
					num5 = (long)((ulong)(-1));
				}
				this.seedInputNum = (uint)num5;
			}
			else
			{
				long num6 = (long)((ulong)this.seedInputNum);
				num6 -= (long)((ulong)this.currentSeedMenuButton.decimalPlaceN);
				if (num6 < 0L)
				{
					num6 = 0L;
				}
				this.seedInputNum = (uint)num6;
			}
			uint num7 = this.seedInputNum / this.currentSeedMenuButton.decimalPlaceN % 10U;
			this.currentSeedMenuButton.SetNumber(num7);
		}
		if (num2 >= 0 && this.currentSeedMenuButton != null && this.currentSeedMenuButton.acceptNumbers)
		{
			uint num8 = this.seedInputNum / this.currentSeedMenuButton.decimalPlaceN % 10U * this.currentSeedMenuButton.decimalPlaceN;
			this.seedInputNum -= num8;
			long num9 = (long)((ulong)this.seedInputNum);
			num9 += (long)((ulong)this.currentSeedMenuButton.decimalPlaceN * (ulong)((long)num2));
			if (num9 > (long)((ulong)(-1)))
			{
				flag6 = true;
				num9 = (long)((ulong)(-1));
			}
			this.seedInputNum = (uint)num9;
			uint num10 = this.seedInputNum / this.currentSeedMenuButton.decimalPlaceN % 10U;
			this.currentSeedMenuButton.SetNumber(num10);
			SeedMenuButton seedMenuButton3 = this.currentSeedMenuButton;
			if (seedMenuButton3 != null)
			{
				seedMenuButton3.HoveredSet(false);
			}
			this.currentSeedMenuButton = this.currentSeedMenuButton.rightButton;
			this.currentSeedMenuButton.HoveredSet(true);
		}
		for (int j = 0; j < this.cypherButtonsInDecimalOrder.Length; j++)
		{
			uint decimalPlaceN = this.cypherButtonsInDecimalOrder[j].decimalPlaceN;
			uint num11 = this.seedInputNum / decimalPlaceN % 10U;
			this.cypherButtonsInDecimalOrder[j].SetNumber(num11);
			if (flag6)
			{
				this.cypherButtonsInDecimalOrder[j].FlashRed();
			}
		}
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x0000FBA8 File Offset: 0x0000DDA8
	private bool Back()
	{
		Sound.Play_Unpausable("SoundMenuBack", 1f, 1f);
		SeedMenuScript.Close();
		return true;
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x0005F23C File Offset: 0x0005D43C
	private void ConfirmSeed()
	{
		Sound.Play_Unpausable("SoundMenuSelect", 1f, 1f);
		uint num = 0U;
		for (int i = 0; i < this.cypherButtonsInDecimalOrder.Length; i++)
		{
			num += this.cypherButtonsInDecimalOrder[i].GetNumber() * this.cypherButtonsInDecimalOrder[i].decimalPlaceN;
		}
		GameplayMaster.specificSeedRequest_ForNewGame = new int?((int)num);
		SeedMenuScript.Close();
	}

	// Token: 0x04000C79 RID: 3193
	public static SeedMenuScript instance;

	// Token: 0x04000C7A RID: 3194
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000C7B RID: 3195
	public GameObject holder;

	// Token: 0x04000C7C RID: 3196
	public TextMeshProUGUI titleText;

	// Token: 0x04000C7D RID: 3197
	public TextMeshProUGUI descriptionText;

	// Token: 0x04000C7E RID: 3198
	public SeedMenuButton[] buttons;

	// Token: 0x04000C7F RID: 3199
	public SeedMenuButton[] cypherButtonsInDecimalOrder;

	// Token: 0x04000C80 RID: 3200
	public SeedMenuButton initialButton;

	// Token: 0x04000C81 RID: 3201
	public SeedMenuButton okButton;

	// Token: 0x04000C82 RID: 3202
	public SeedMenuButton backButton;

	// Token: 0x04000C83 RID: 3203
	private Vector2 axisRawPrevious = Vector2.zero;

	// Token: 0x04000C84 RID: 3204
	private SeedMenuButton currentSeedMenuButton;

	// Token: 0x04000C85 RID: 3205
	private uint seedInputNum;
}
