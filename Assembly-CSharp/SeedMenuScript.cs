using System;
using Panik;
using TMPro;
using UnityEngine;

public class SeedMenuScript : MonoBehaviour
{
	// Token: 0x06000A09 RID: 2569 RVA: 0x00044B10 File Offset: 0x00042D10
	public static bool IsEnabled()
	{
		return !(SeedMenuScript.instance == null) && SeedMenuScript.instance.holder.activeSelf;
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x00044B30 File Offset: 0x00042D30
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

	// Token: 0x06000A0B RID: 2571 RVA: 0x00044BA0 File Offset: 0x00042DA0
	private static void Close()
	{
		SeedMenuScript.instance.holder.SetActive(false);
		SeedMenuScript.instance.seedInputNum = 0U;
		for (int i = 0; i < SeedMenuScript.instance.cypherButtonsInDecimalOrder.Length; i++)
		{
			SeedMenuScript.instance.cypherButtonsInDecimalOrder[i].SetNumber(0U);
		}
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x00044BF1 File Offset: 0x00042DF1
	private void TextUpdate()
	{
		this.titleText.text = Translation.Get("MENU_SEED_INPUT_TITLE");
		this.descriptionText.text = Translation.Get("MENU_SEED_INPUT_DESCRIPTION");
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x00044C1D File Offset: 0x00042E1D
	private void Awake()
	{
		SeedMenuScript.instance = this;
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x00044C25 File Offset: 0x00042E25
	private void Start()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x00044C33 File Offset: 0x00042E33
	private void OnEnable()
	{
		this.TextUpdate();
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x00044C3B File Offset: 0x00042E3B
	private void OnDestroy()
	{
		if (SeedMenuScript.instance == this)
		{
			SeedMenuScript.instance = null;
		}
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x00044C50 File Offset: 0x00042E50
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
		bool keyDown = Input.GetKeyDown(8);
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

	// Token: 0x06000A12 RID: 2578 RVA: 0x0004543E File Offset: 0x0004363E
	private bool Back()
	{
		Sound.Play_Unpausable("SoundMenuBack", 1f, 1f);
		SeedMenuScript.Close();
		return true;
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x0004545C File Offset: 0x0004365C
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

	public static SeedMenuScript instance;

	private const int PLAYER_INDEX = 0;

	public GameObject holder;

	public TextMeshProUGUI titleText;

	public TextMeshProUGUI descriptionText;

	public SeedMenuButton[] buttons;

	public SeedMenuButton[] cypherButtonsInDecimalOrder;

	public SeedMenuButton initialButton;

	public SeedMenuButton okButton;

	public SeedMenuButton backButton;

	private Vector2 axisRawPrevious = Vector2.zero;

	private SeedMenuButton currentSeedMenuButton;

	private uint seedInputNum;
}
