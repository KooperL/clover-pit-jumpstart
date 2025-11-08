using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;

public class PromptsMaster : MonoBehaviour
{
	// Token: 0x060000DB RID: 219 RVA: 0x00009172 File Offset: 0x00007372
	public static string GetSpriteName_Keyboard(Controls.KeyboardElement element)
	{
		return PromptsMaster.instance.promptSpriteNames_Keyboard[(int)element];
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00009180 File Offset: 0x00007380
	public static string GetSpriteName_Mouse(Controls.MouseElement element)
	{
		return PromptsMaster.instance.promptSpriteNames_Mouse[(int)element];
	}

	// Token: 0x060000DD RID: 221 RVA: 0x0000918E File Offset: 0x0000738E
	public static string GetSpriteName_Joystick(Controls.JoystickElement element)
	{
		return PromptsMaster.GetSpriteName_Joystick(element, PlatformMaster.PlatformKindGet());
	}

	// Token: 0x060000DE RID: 222 RVA: 0x0000919C File Offset: 0x0000739C
	public static string GetSpriteName_Joystick(Controls.JoystickElement element, PlatformMaster.PlatformKind platform)
	{
		switch (platform)
		{
		case PlatformMaster.PlatformKind.PS4:
			return PromptsMaster.instance.promptSpriteNames_Joystick_PS4[(int)element];
		case PlatformMaster.PlatformKind.PS5:
			return PromptsMaster.instance.promptSpriteNames_Joystick_PS5[(int)element];
		case PlatformMaster.PlatformKind.XboxOne:
			return PromptsMaster.instance.promptSpriteNames_Joystick_Xbox[(int)element];
		case PlatformMaster.PlatformKind.XboxSeries:
			return PromptsMaster.instance.promptSpriteNames_Joystick_Xbox[(int)element];
		case PlatformMaster.PlatformKind.NintendoSwitch:
			return PromptsMaster.instance.promptSpriteNames_Joystick_NintendoSwitch[(int)element];
		default:
			return PromptsMaster.instance.promptSpriteNames_Joystick_Xbox[(int)element];
		}
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00009214 File Offset: 0x00007414
	public static Sprite GetSprite_Keyboard(Controls.KeyboardElement element)
	{
		string spriteName_Keyboard = PromptsMaster.GetSpriteName_Keyboard(element);
		if (!PromptsMaster.inputSpritesDict.ContainsKey(spriteName_Keyboard))
		{
			return null;
		}
		return PromptsMaster.inputSpritesDict[spriteName_Keyboard];
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00009244 File Offset: 0x00007444
	public static Sprite GetSprite_Mouse(Controls.MouseElement element)
	{
		string spriteName_Mouse = PromptsMaster.GetSpriteName_Mouse(element);
		if (!PromptsMaster.inputSpritesDict.ContainsKey(spriteName_Mouse))
		{
			return null;
		}
		return PromptsMaster.inputSpritesDict[spriteName_Mouse];
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00009272 File Offset: 0x00007472
	public static Sprite GetSprite_Joystick(Controls.JoystickElement element)
	{
		return PromptsMaster.GetSprite_Joystick(element, PlatformMaster.PlatformKindGet());
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00009280 File Offset: 0x00007480
	public static Sprite GetSprite_Joystick(Controls.JoystickElement element, PlatformMaster.PlatformKind platform)
	{
		string spriteName_Joystick = PromptsMaster.GetSpriteName_Joystick(element, platform);
		if (!PromptsMaster.inputSpritesDict.ContainsKey(spriteName_Joystick))
		{
			return null;
		}
		return PromptsMaster.inputSpritesDict[spriteName_Joystick];
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x000092B0 File Offset: 0x000074B0
	private void InitTMPSpriteStrings()
	{
		if (PromptsMaster.promptsSpriteTMPStrings_Keyboard == null)
		{
			PromptsMaster.promptsSpriteTMPStrings_Keyboard = new string[this.promptSpriteNames_Keyboard.Length];
			for (int i = 0; i < this.promptSpriteNames_Keyboard.Length; i++)
			{
				PromptsMaster.promptsSpriteTMPStrings_Keyboard[i] = "<sprite name=\"" + this.promptSpriteNames_Keyboard[i] + "\">";
			}
		}
		if (PromptsMaster.promptsSpriteTMPStrings_Mouse == null)
		{
			PromptsMaster.promptsSpriteTMPStrings_Mouse = new string[this.promptSpriteNames_Mouse.Length];
			for (int j = 0; j < this.promptSpriteNames_Mouse.Length; j++)
			{
				PromptsMaster.promptsSpriteTMPStrings_Mouse[j] = "<sprite name=\"" + this.promptSpriteNames_Mouse[j] + "\">";
			}
		}
		if (PromptsMaster.promptsSpriteTMPStrings_Joystick_Xbox == null)
		{
			PromptsMaster.promptsSpriteTMPStrings_Joystick_Xbox = new string[this.promptSpriteNames_Joystick_Xbox.Length];
			for (int k = 0; k < this.promptSpriteNames_Joystick_Xbox.Length; k++)
			{
				PromptsMaster.promptsSpriteTMPStrings_Joystick_Xbox[k] = "<sprite name=\"" + this.promptSpriteNames_Joystick_Xbox[k] + "\">";
			}
		}
		if (PromptsMaster.promptsSpriteTMPStrings_Joystick_PS4 == null)
		{
			PromptsMaster.promptsSpriteTMPStrings_Joystick_PS4 = new string[this.promptSpriteNames_Joystick_PS4.Length];
			for (int l = 0; l < this.promptSpriteNames_Joystick_PS4.Length; l++)
			{
				PromptsMaster.promptsSpriteTMPStrings_Joystick_PS4[l] = "<sprite name=\"" + this.promptSpriteNames_Joystick_PS4[l] + "\">";
			}
		}
		if (PromptsMaster.promptsSpriteTMPStrings_Joystick_PS5 == null)
		{
			PromptsMaster.promptsSpriteTMPStrings_Joystick_PS5 = new string[this.promptSpriteNames_Joystick_PS5.Length];
			for (int m = 0; m < this.promptSpriteNames_Joystick_PS5.Length; m++)
			{
				PromptsMaster.promptsSpriteTMPStrings_Joystick_PS5[m] = "<sprite name=\"" + this.promptSpriteNames_Joystick_PS5[m] + "\">";
			}
		}
		if (PromptsMaster.promptsSpriteTMPStrings_Joystick_NintendoSwitch == null)
		{
			PromptsMaster.promptsSpriteTMPStrings_Joystick_NintendoSwitch = new string[this.promptSpriteNames_Joystick_NintendoSwitch.Length];
			for (int n = 0; n < this.promptSpriteNames_Joystick_NintendoSwitch.Length; n++)
			{
				PromptsMaster.promptsSpriteTMPStrings_Joystick_NintendoSwitch[n] = "<sprite name=\"" + this.promptSpriteNames_Joystick_NintendoSwitch[n] + "\">";
			}
		}
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00009485 File Offset: 0x00007685
	public static string GetSpriteString_Keyboard(Controls.KeyboardElement element)
	{
		return PromptsMaster.promptsSpriteTMPStrings_Keyboard[(int)element];
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x0000948E File Offset: 0x0000768E
	public static string GetSpriteString_Mouse(Controls.MouseElement element)
	{
		return PromptsMaster.promptsSpriteTMPStrings_Mouse[(int)element];
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00009497 File Offset: 0x00007697
	public static string GetSpriteString_Joystick(Controls.JoystickElement element)
	{
		return PromptsMaster.GetSpriteString_Joystick(element, PlatformMaster.PlatformKindGet());
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x000094A4 File Offset: 0x000076A4
	public static string GetSpriteString_Joystick(Controls.JoystickElement element, PlatformMaster.PlatformKind platform)
	{
		switch (platform)
		{
		case PlatformMaster.PlatformKind.PS4:
			return PromptsMaster.promptsSpriteTMPStrings_Joystick_PS4[(int)element];
		case PlatformMaster.PlatformKind.PS5:
			return PromptsMaster.promptsSpriteTMPStrings_Joystick_PS5[(int)element];
		case PlatformMaster.PlatformKind.XboxOne:
			return PromptsMaster.promptsSpriteTMPStrings_Joystick_Xbox[(int)element];
		case PlatformMaster.PlatformKind.XboxSeries:
			return PromptsMaster.promptsSpriteTMPStrings_Joystick_Xbox[(int)element];
		case PlatformMaster.PlatformKind.NintendoSwitch:
			return PromptsMaster.promptsSpriteTMPStrings_Joystick_NintendoSwitch[(int)element];
		default:
			return PromptsMaster.promptsSpriteTMPStrings_Joystick_Xbox[(int)element];
		}
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x00009500 File Offset: 0x00007700
	public void DebugEnableSet(bool value, bool forceSet = false)
	{
		if (this.debugEnabled == value)
		{
			return;
		}
		this.debugEnabled = value;
		if (this.debugEnabled)
		{
			this._debugActionStatesText.gameObject.SetActive(true);
			if (this.actionNames == null)
			{
				this.actionNames = Enum.GetNames(typeof(Controls.InputAction));
				return;
			}
		}
		else
		{
			this._debugActionStatesText.gameObject.SetActive(false);
		}
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00009566 File Offset: 0x00007766
	public bool DebugEnableGet()
	{
		return this.debugEnabled;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x00009570 File Offset: 0x00007770
	private void DebugUpdate()
	{
		if (!this.debugEnabled)
		{
			return;
		}
		string text = "Action States:\n";
		for (int i = 0; i < this.actionNames.Length; i++)
		{
			text = text + this.actionNames[i] + ": ";
			for (int j = 0; j < 1; j++)
			{
				if (Controls.MapFind_InUse(j, (Controls.InputAction)i) == null)
				{
					text += "0";
				}
				else if (!Controls.ActionButton_HoldGet(j, (Controls.InputAction)i, true))
				{
					text += "0";
				}
				else
				{
					text += "1";
				}
				if (j < 0)
				{
					text += "-";
				}
				else
				{
					text += "\n";
				}
			}
		}
		this._debugActionStatesText.text = text;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00009628 File Offset: 0x00007828
	private void Awake()
	{
		if (PromptsMaster.instance != null)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		PromptsMaster.instance = this;
		for (int i = 0; i < this.inputSprites.Length; i++)
		{
			if (PromptsMaster.inputSpritesDict.ContainsKey(this.inputSprites[i].name))
			{
				Debug.LogError("PromptsMaster: Awake() - There are duplicate sprites in the inputSprites array! Duplicate: " + this.inputSprites[i].name);
			}
			else
			{
				PromptsMaster.inputSpritesDict.Add(this.inputSprites[i].name, this.inputSprites[i]);
			}
		}
		this.InitTMPSpriteStrings();
		this.DebugEnableSet(false, true);
	}

	// Token: 0x060000EC RID: 236 RVA: 0x000096CB File Offset: 0x000078CB
	private void Update()
	{
		this.DebugUpdate();
	}

	public static PromptsMaster instance;

	private string[] promptSpriteNames_Keyboard = new string[]
	{
		"prompts_sheet_keyboard_86", "prompts_sheet_keyboard_50", "prompts_sheet_keyboard_56", "prompts_sheet_keyboard_59", "prompts_sheet_keyboard_61", "prompts_sheet_keyboard_63", "prompts_sheet_keyboard_65", "prompts_sheet_keyboard_80", "prompts_sheet_keyboard_82", "prompts_sheet_keyboard_83",
		"prompts_sheet_keyboard_84", "prompts_sheet_keyboard_85", "prompts_sheet_keyboard_87", "prompts_sheet_keyboard_89", "prompts_sheet_keyboard_90", "prompts_sheet_keyboard_91", "prompts_sheet_keyboard_92", "prompts_sheet_keyboard_96", "prompts_sheet_keyboard_98", "prompts_sheet_keyboard_100",
		"prompts_sheet_keyboard_105", "prompts_sheet_keyboard_106", "prompts_sheet_keyboard_108", "prompts_sheet_keyboard_109", "prompts_sheet_keyboard_110", "prompts_sheet_keyboard_111", "prompts_sheet_keyboard_112", "prompts_sheet_keyboard_40", "prompts_sheet_keyboard_41", "prompts_sheet_keyboard_42",
		"prompts_sheet_keyboard_43", "prompts_sheet_keyboard_44", "prompts_sheet_keyboard_45", "prompts_sheet_keyboard_46", "prompts_sheet_keyboard_47", "prompts_sheet_keyboard_48", "prompts_sheet_keyboard_49", "prompts_sheet_keyboard_40", "prompts_sheet_keyboard_41", "prompts_sheet_keyboard_42",
		"prompts_sheet_keyboard_43", "prompts_sheet_keyboard_44", "prompts_sheet_keyboard_45", "prompts_sheet_keyboard_46", "prompts_sheet_keyboard_47", "prompts_sheet_keyboard_48", "prompts_sheet_keyboard_49", "prompts_sheet_keyboard_26", "prompts_sheet_keyboard_79", "prompts_sheet_keyboard_55",
		"prompts_sheet_keyboard_33", "prompts_sheet_keyboard_38", "prompts_sheet_keyboard_10", "prompts_sheet_keyboard_39", "prompts_sheet_keyboard_0", "prompts_sheet_keyboard_1", "prompts_sheet_keyboard_18", "prompts_sheet_keyboard_117", "prompts_sheet_keyboard_10", "prompts_sheet_keyboard_6",
		"prompts_sheet_keyboard_64", "prompts_sheet_keyboard_4", "prompts_sheet_keyboard_62", "prompts_sheet_keyboard_19", "prompts_sheet_keyboard_20", "prompts_sheet_keyboard_22", "prompts_sheet_keyboard_32", "prompts_sheet_keyboard_23", "prompts_sheet_keyboard_24", "prompts_sheet_keyboard_55",
		"prompts_sheet_keyboard_38", "prompts_sheet_keyboard_25", "prompts_sheet_keyboard_33", "prompts_sheet_keyboard_26", "prompts_sheet_keyboard_27", "prompts_sheet_keyboard_60", "prompts_sheet_keyboard_27", "prompts_sheet_keyboard_122", "prompts_sheet_keyboard_39", "prompts_sheet_keyboard_121",
		"prompts_sheet_keyboard_97", "prompts_sheet_keyboard_28", "prompts_sheet_keyboard_29", "prompts_sheet_keyboard_57", "prompts_sheet_keyboard_30", "prompts_sheet_keyboard_31", "prompts_sheet_keyboard_107", "prompts_sheet_keyboard_34", "prompts_sheet_keyboard_15", "prompts_sheet_keyboard_54",
		"prompts_sheet_keyboard_51", "prompts_sheet_keyboard_53", "prompts_sheet_keyboard_52", "prompts_sheet_keyboard_2", "prompts_sheet_keyboard_13", "prompts_sheet_keyboard_16", "prompts_sheet_keyboard_5", "prompts_sheet_keyboard_3", "prompts_sheet_keyboard_66", "prompts_sheet_keyboard_67",
		"prompts_sheet_keyboard_68", "prompts_sheet_keyboard_69", "prompts_sheet_keyboard_70", "prompts_sheet_keyboard_71", "prompts_sheet_keyboard_72", "prompts_sheet_keyboard_73", "prompts_sheet_keyboard_74", "prompts_sheet_keyboard_75", "prompts_sheet_keyboard_76", "prompts_sheet_keyboard_77",
		"prompts_sheet_keyboard_118", "prompts_sheet_keyboard_119", "prompts_sheet_keyboard_120", "prompts_sheet_keyboard_17", "prompts_sheet_keyboard_11", "prompts_sheet_keyboard_8", "prompts_sheet_keyboard_9", "prompts_sheet_keyboard_9", "prompts_sheet_keyboard_12", "prompts_sheet_keyboard_12",
		"prompts_sheet_keyboard_14", "prompts_sheet_keyboard_14", "prompts_sheet_keyboard_104", "prompts_sheet_keyboard_104", "prompts_sheet_keyboard_104", "prompts_sheet_keyboard_104", "prompts_sheet_keyboard_14", "prompts_sheet_keyboard_113", "prompts_sheet_keyboard_7", "prompts_sheet_keyboard_116",
		"prompts_sheet_keyboard_116", "prompts_sheet_keyboard_114", "prompts_sheet_generic_2", "prompts_sheet_generic_2"
	};

	private string[] promptSpriteNames_Mouse = new string[] { "prompts_sheet_mouse_0", "prompts_sheet_mouse_3", "prompts_sheet_mouse_1", "prompts_sheet_mouse_5", "prompts_sheet_mouse_4", "prompts_sheet_mouse_2", "prompts_sheet_mouse_2", "prompts_sheet_generic_2", "prompts_sheet_generic_2" };

	private string[] promptSpriteNames_Joystick_Xbox = new string[]
	{
		"prompts_sheet_xbox_22", "prompts_sheet_xbox_23", "prompts_sheet_xbox_27", "prompts_sheet_xbox_28", "prompts_sheet_xbox_29", "prompts_sheet_xbox_30", "prompts_sheet_xbox_31", "prompts_sheet_xbox_4", "prompts_sheet_xbox_9", "prompts_sheet_xbox_10",
		"prompts_sheet_xbox_11", "prompts_sheet_xbox_3", "prompts_sheet_xbox_7", "prompts_sheet_xbox_8", "prompts_sheet_xbox_1", "prompts_sheet_xbox_15", "prompts_sheet_xbox_16", "prompts_sheet_xbox_18", "prompts_sheet_xbox_20", "prompts_sheet_xbox_17",
		"prompts_sheet_xbox_21", "prompts_sheet_generic_2", "prompts_sheet_generic_2"
	};

	private string[] promptSpriteNames_Joystick_PS4 = new string[]
	{
		"prompts_sheet_playstation_28", "prompts_sheet_playstation_23", "prompts_sheet_playstation_33", "prompts_sheet_playstation_39", "prompts_sheet_playstation_54", "prompts_sheet_playstation_52", "prompts_sheet_playstation_66", "prompts_sheet_playstation_50", "prompts_sheet_playstation_56", "prompts_sheet_playstation_46",
		"prompts_sheet_playstation_48", "prompts_sheet_playstation_8", "prompts_sheet_playstation_14", "prompts_sheet_playstation_16", "prompts_sheet_playstation_3", "prompts_sheet_playstation_18", "prompts_sheet_playstation_20", "prompts_sheet_playstation_42", "prompts_sheet_playstation_44", "prompts_sheet_playstation_58",
		"prompts_sheet_playstation_60", "prompts_sheet_generic_2", "prompts_sheet_generic_2"
	};

	private string[] promptSpriteNames_Joystick_PS5 = new string[]
	{
		"prompts_sheet_playstation_29", "prompts_sheet_playstation_24", "prompts_sheet_playstation_34", "prompts_sheet_playstation_40", "prompts_sheet_playstation_55", "prompts_sheet_playstation_53", "prompts_sheet_playstation_67", "prompts_sheet_playstation_51", "prompts_sheet_playstation_57", "prompts_sheet_playstation_47",
		"prompts_sheet_playstation_49", "prompts_sheet_playstation_9", "prompts_sheet_playstation_15", "prompts_sheet_playstation_17", "prompts_sheet_playstation_4", "prompts_sheet_playstation_19", "prompts_sheet_playstation_21", "prompts_sheet_playstation_43", "prompts_sheet_playstation_45", "prompts_sheet_playstation_59",
		"prompts_sheet_playstation_61", "prompts_sheet_generic_2", "prompts_sheet_generic_2"
	};

	private string[] promptSpriteNames_Joystick_NintendoSwitch = new string[]
	{
		"prompts_sheet_nintendo_switch_1", "prompts_sheet_nintendo_switch_0", "prompts_sheet_nintendo_switch_17", "prompts_sheet_nintendo_switch_16", "prompts_sheet_nintendo_switch_24", "prompts_sheet_nintendo_switch_23", "prompts_sheet_nintendo_switch_22", "prompts_sheet_nintendo_switch_18", "prompts_sheet_nintendo_switch_19", "prompts_sheet_nintendo_switch_7",
		"prompts_sheet_nintendo_switch_9", "prompts_sheet_nintendo_switch_8", "prompts_sheet_nintendo_switch_12", "prompts_sheet_nintendo_switch_13", "prompts_sheet_nintendo_switch_6", "prompts_sheet_nintendo_switch_2", "prompts_sheet_nintendo_switch_3", "prompts_sheet_nintendo_switch_4", "prompts_sheet_nintendo_switch_5", "prompts_sheet_nintendo_switch_14",
		"prompts_sheet_nintendo_switch_15", "prompts_sheet_generic_2", "prompts_sheet_generic_2"
	};

	public Sprite[] inputSprites;

	private static Dictionary<string, Sprite> inputSpritesDict = new Dictionary<string, Sprite>();

	private static string[] promptsSpriteTMPStrings_Keyboard = null;

	private static string[] promptsSpriteTMPStrings_Mouse = null;

	private static string[] promptsSpriteTMPStrings_Joystick_Xbox = null;

	private static string[] promptsSpriteTMPStrings_Joystick_PS4 = null;

	private static string[] promptsSpriteTMPStrings_Joystick_PS5 = null;

	private static string[] promptsSpriteTMPStrings_Joystick_NintendoSwitch = null;

	private bool debugEnabled;

	public TextMeshProUGUI _debugActionStatesText;

	private string[] actionNames;
}
