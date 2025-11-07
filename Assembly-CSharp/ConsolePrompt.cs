using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ConsolePrompt : MonoBehaviour
{
	// Token: 0x060000B7 RID: 183 RVA: 0x00007C84 File Offset: 0x00005E84
	public static bool ConsoleEnable()
	{
		ConsolePrompt.instance.autoCloseTimer = 0f;
		bool flag = false;
		if (!ConsolePrompt.instance._enabled)
		{
			flag = true;
			ConsolePrompt.instance.inputString = "";
		}
		ConsolePrompt.instance._enabled = true;
		return flag;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00007CCB File Offset: 0x00005ECB
	public static void ConsoleDisable()
	{
		ConsolePrompt.instance._enabled = false;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00007CD8 File Offset: 0x00005ED8
	public static bool ConsoleIsEnabled()
	{
		return ConsolePrompt.instance._enabled;
	}

	// (get) Token: 0x060000BA RID: 186 RVA: 0x00007CE4 File Offset: 0x00005EE4
	public static bool CanAutoClose
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00007CE8 File Offset: 0x00005EE8
	private void TryExecuteCommand()
	{
		if (this.inputString.Length == 0)
		{
			return;
		}
		this.executionIndex++;
		this.inputString = this.inputString.ToLower();
		int num = 0;
		int.TryParse(this.inputString, out num);
		while (this.inputString.Length > 0 && (this.inputString[this.inputString.Length - 1] == ' ' || this.inputString[this.inputString.Length - 1] == '\t'))
		{
			this.inputString = this.inputString.Substring(0, this.inputString.Length - 1);
		}
		ConsolePrompt.Command command = null;
		if (this.availableCommands.TryGetValue(this.inputString.ToLower(), out command))
		{
			this.outputStringList.Add(this.executionIndex.ToString() + ": " + this.inputString);
			if (!command.TryExecute())
			{
				this.outputStringList.Add(this.executionIndex.ToString() + ": <color=red>Error: </color> Command failed. Check player log for errors! Command: " + this.inputString);
			}
		}
		else
		{
			ConsolePrompt.LogError("Command not found: " + this.inputString, this.executionIndex.ToString() + ": ", 0f);
		}
		if (!string.IsNullOrEmpty(this.inputString))
		{
			this.inputStringOld = this.inputString;
		}
		this.inputString = "";
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00007E60 File Offset: 0x00006060
	private void CommandsGenerate()
	{
		new ConsolePrompt.Command(new string[] { "capture mode", "capture", "capture toggle", "toggle capture", "cattura", "modalità cattura", "social", "modalità social" }, "Toggles the capture mode", delegate
		{
			ConsolePrompt.captureMode = !ConsolePrompt.captureMode;
		});
		new ConsolePrompt.Command(new string[] { "help", "h", "?" }, "Displays all available commands", delegate
		{
			this.outputStringList.Add("--------------------");
			this.outputStringList.Add("Available commands:");
			List<ConsolePrompt.Command> list = new List<ConsolePrompt.Command>();
			int count = this.outputStringList.Count;
			foreach (KeyValuePair<string, ConsolePrompt.Command> keyValuePair in this.availableCommands)
			{
				if (!list.Contains(keyValuePair.Value))
				{
					this.outputStringList.Add("- " + keyValuePair.Value.allowedEntries[0] + " : " + keyValuePair.Value.description);
					list.Add(keyValuePair.Value);
				}
			}
			for (int i = count; i < this.outputStringList.Count; i++)
			{
				for (int j = i + 1; j < this.outputStringList.Count; j++)
				{
					if (string.Compare(this.outputStringList[i], this.outputStringList[j]) > 0)
					{
						string text = this.outputStringList[i];
						this.outputStringList[i] = this.outputStringList[j];
						this.outputStringList[j] = text;
					}
				}
			}
			this.outputStringList.Add("--------------------");
			this.outputStringList.Add("- Scroll up console: Ctrl + Up arrow");
			this.outputStringList.Add("- Scroll down console: Ctrl + Down arrow");
			this.outputStringList.Add("- Scroll ALL up console: Ctrl + Page Up");
			this.outputStringList.Add("- Scroll ALL down console: Ctrl + Page Down");
		});
		new ConsolePrompt.Command(new string[] { "toggle log intercept", "log intercept toggle", "log intercept", "log toggle", "toggle log" }, "Toggles the debug interception", delegate
		{
			this.interceptDebugLog = !this.interceptDebugLog;
			if (this.interceptDebugLog)
			{
				this.RegisterDebugInterception();
			}
			this.outputStringList.Add("Debug interception: " + this.interceptDebugLog.ToString());
		});
		new ConsolePrompt.Command(new string[] { "clear", "c" }, "Clears the console", delegate
		{
			this.outputStringList.Clear();
		});
		new ConsolePrompt.Command(new string[] { "close game", "quit" }, "Closes the game", delegate
		{
			Application.Quit();
		});
		new ConsolePrompt.Command(new string[] { "save settings" }, "Saves the current settings", delegate
		{
			Data.SaveSettings();
			this.outputStringList.Add("Settings saved!");
		});
		new ConsolePrompt.Command(new string[] { "load settings" }, "Loads the settings", delegate
		{
			Data.LoadSettingsAndApply();
			this.outputStringList.Add("Settings loaded!");
		});
		new ConsolePrompt.Command(new string[] { "reset settings" }, "Resets the settings to default", delegate
		{
			Data.DeleteSettingsAndReset();
			this.outputStringList.Add("Settings resetted!");
		});
		new ConsolePrompt.Command(new string[] { "fullscreen switch", "fullscreen toggle", "fullscreen", "f" }, "Switches the fullscreen mode", delegate
		{
			Data.settings.fullscreenEnabled = !Data.settings.fullscreenEnabled;
			Data.SaveSettingsAndApply(true);
			this.outputStringList.Add("Fullscreen: " + Data.settings.fullscreenEnabled.ToString());
		});
		new ConsolePrompt.Command(new string[] { "resolution switch", "resolution toggle", "resolution", "res" }, "Switches resolution", delegate
		{
			Data.settings.VerticalResolutionSetNext();
			Data.SaveSettingsAndApply(true);
			this.outputStringList.Add("Resolution: " + Data.settings.VerticalResolutionGet().ToString());
		});
		new ConsolePrompt.Command(new string[] { "crt next" }, "Sets the next CRT filter", delegate
		{
			int num = (int)Data.settings.crtFilter;
			num++;
			if (num >= 4)
			{
				num = 0;
			}
			Data.settings.crtFilter = (Data.SettingsData.CrtFilter)num;
			Data.SaveSettingsAndApply(true);
			this.outputStringList.Add("Crt: " + Data.settings.crtFilter.ToString());
		});
		new ConsolePrompt.Command(new string[] { "crt previous", "crt prev" }, "Sets the previous CRT filter", delegate
		{
			int num2 = (int)Data.settings.crtFilter;
			num2--;
			if (num2 < 0)
			{
				num2 = 3;
			}
			Data.settings.crtFilter = (Data.SettingsData.CrtFilter)num2;
			Data.SaveSettingsAndApply(true);
			this.outputStringList.Add("Crt: " + Data.settings.crtFilter.ToString());
		});
		new ConsolePrompt.Command(new string[] { "crt reset", "crt default" }, "Resets the CRT filter to the default one", delegate
		{
			Data.settings.crtFilter = Data.SettingsData.CrtFilter.none;
			Data.SaveSettingsAndApply(true);
			this.outputStringList.Add("Crt: " + Data.settings.crtFilter.ToString());
		});
		new ConsolePrompt.Command(new string[] { "tate next", "tate mode next" }, "Sets the next tate mode", delegate
		{
			if (!Master.instance.RENDER_TO_TEXTURE)
			{
				ConsolePrompt.LogError("Tate mode is not available when rendering to a texture is DISABLED!", "", 0f);
				return;
			}
			int num3 = (int)Data.settings.tateMode;
			num3++;
			if (num3 >= 4)
			{
				num3 = 0;
			}
			Data.settings.tateMode = (Data.SettingsData.TateMode)num3;
			Data.SaveSettingsAndApply(true);
			this.outputStringList.Add("Tate mode: " + Data.settings.tateMode.ToString());
		});
		new ConsolePrompt.Command(new string[] { "tate previous", "tate prev", "tate mode previous", "tate mode prev" }, "Sets the previous tate mode", delegate
		{
			if (!Master.instance.RENDER_TO_TEXTURE)
			{
				ConsolePrompt.LogError("Tate mode is not available when rendering to a texture is DISABLED!", "", 0f);
				return;
			}
			int num4 = (int)Data.settings.tateMode;
			num4--;
			if (num4 < 0)
			{
				num4 = 3;
			}
			Data.settings.tateMode = (Data.SettingsData.TateMode)num4;
			Data.SaveSettingsAndApply(true);
			this.outputStringList.Add("Tate mode: " + Data.settings.tateMode.ToString());
		});
		new ConsolePrompt.Command(new string[] { "tate reset", "tate default" }, "Resets the tate mode to the default one", delegate
		{
			Data.settings.tateMode = Data.SettingsData.TateMode.none;
			Data.SaveSettingsAndApply(true);
			this.outputStringList.Add("Tate mode: " + Data.settings.tateMode.ToString());
		});
		new ConsolePrompt.Command(new string[] { "transitions speed", "transitions" }, "Switches the speed of game transitions", delegate
		{
			Data.settings.transitionSpeed++;
			if (Data.settings.transitionSpeed > 4)
			{
				Data.settings.transitionSpeed = 1;
			}
			Data.settings.Apply(false, false);
			this.outputStringList.Add("Transition Speed: X" + Data.settings.transitionSpeed.ToString());
		});
		new ConsolePrompt.Command(new string[] { "wobbly polygons", "polygons" }, "Toggless wobbly polygons on and off", delegate
		{
			Data.settings.wobblyPolygons = !Data.settings.wobblyPolygons;
			Data.settings.Apply(false, false);
			this.outputStringList.Add("Wobbly Polygons: " + Data.settings.wobblyPolygons.ToString());
		});
		new ConsolePrompt.Command(new string[] { "fan Volume", "fan" }, "Switches the volume of the fan", delegate
		{
			Data.settings.fanVolume += 0.1f;
			if (Data.settings.fanVolume > 1.05f)
			{
				Data.settings.fanVolume = 0f;
			}
			Data.settings.Apply(false, false);
			this.outputStringList.Add("Fan Volume: " + Data.settings.fanVolume.ToString());
		});
		new ConsolePrompt.Command(new string[] { "cursor sensitivity", "cursor" }, "Switches the cursor speed", delegate
		{
			float num5 = Data.settings.VirtualCursorSensitivityGet(0);
			num5 += 0.1f;
			if (num5 > 3.05f)
			{
				num5 = 0.1f;
			}
			Data.settings.VirtualCursorSensitivitySet(0, num5);
			Data.settings.Apply(false, false);
			this.outputStringList.Add("Cursor Sensitivty: " + num5.ToString());
		});
		new ConsolePrompt.Command(new string[] { "invert camera y", "camera y" }, "Invert the input on the camera y", delegate
		{
			Data.settings.ControlsInvertCameraYSet(0, !Data.settings.ControlsInvertCameraYGet(0));
			Data.settings.Apply(false, false);
			this.outputStringList.Add("Camera Y inverted: " + Data.settings.ControlsInvertCameraYGet(0).ToString());
		});
		new ConsolePrompt.Command(new string[] { "is debug", "is debug build" }, "Displays if the game is a debug build", delegate
		{
			this.outputStringList.Add("Is debug build: " + Master.IsDebugBuild.ToString());
		});
		new ConsolePrompt.Command(new string[] { "is demo" }, "Displays if the game is a demo", delegate
		{
			this.outputStringList.Add("Is demo: " + Master.IsDemo.ToString());
		});
		new ConsolePrompt.Command(new string[] { "fps", "fps toggle", "toggle fps" }, "Toggles the fps display", delegate
		{
			this.fpsText.enabled = !this.fpsText.enabled;
		});
		new ConsolePrompt.Command(new string[] { "toggle ui", "ui toggle", "ui" }, "Toggles the UI", delegate
		{
			CameraUiGlobal.Debug_UiStateSet(!CameraUiGlobal.Debug_UiStateGet());
		});
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00008373 File Offset: 0x00006573
	public static void Log(string logText, string prefixText = "")
	{
		if (!ConsolePrompt.captureMode)
		{
			ConsolePrompt.ConsoleEnable();
		}
		ConsolePrompt.instance.outputStringList.Add(prefixText + "<color=white>Log: </color> " + logText);
	}

	// Token: 0x060000BE RID: 190 RVA: 0x0000839D File Offset: 0x0000659D
	public static void LogWarning(string warningText, string prefixText = "")
	{
		if (!ConsolePrompt.captureMode)
		{
			ConsolePrompt.ConsoleEnable();
		}
		ConsolePrompt.instance.outputStringList.Add(prefixText + "<color=yellow>Warning: </color> " + warningText);
	}

	// Token: 0x060000BF RID: 191 RVA: 0x000083C7 File Offset: 0x000065C7
	public static void LogError(string errorText, string prefixText = "", float extraTime = 0f)
	{
		if (!ConsolePrompt.captureMode)
		{
			ConsolePrompt.ConsoleEnable();
		}
		ConsolePrompt.instance.outputStringList.Add(prefixText + "<color=red>Error: </color> " + errorText);
		ConsolePrompt.instance.autoCloseTimer -= extraTime;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00008403 File Offset: 0x00006603
	private void RegisterDebugInterception()
	{
		if (ConsolePrompt._registeredDebugInterception)
		{
			return;
		}
		ConsolePrompt._registeredDebugInterception = true;
		Application.logMessageReceived += new Application.LogCallback(this.OnDebugCallback);
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00008424 File Offset: 0x00006624
	private void OnDebugCallback(string condition, string stackTrace, LogType type)
	{
		if (!this.interceptDebugLog)
		{
			return;
		}
		if (type == null || type == 4)
		{
			string text = "<color=red>Error: </color> " + condition + "\n" + stackTrace;
			if (text.Length > this.LOG_INTERCEPTION_MAX_CHARACTERS)
			{
				text = text.Substring(0, this.LOG_INTERCEPTION_MAX_CHARACTERS);
				text += "...";
			}
			text += "\n";
			ConsolePrompt.LogError(text, "", 0f);
			return;
		}
		if (type == 2)
		{
			string text2 = "<color=yellow>Warning: </color> " + condition;
			if (text2.Length > this.LOG_INTERCEPTION_MAX_CHARACTERS)
			{
				text2 = text2.Substring(0, this.LOG_INTERCEPTION_MAX_CHARACTERS);
				text2 += "...";
			}
			text2 += "\n";
			ConsolePrompt.LogWarning(condition + "\n", "");
			return;
		}
		string text3 = "<color=white>Log: </color> " + condition;
		if (text3.Length > this.LOG_INTERCEPTION_MAX_CHARACTERS)
		{
			text3 = text3.Substring(0, this.LOG_INTERCEPTION_MAX_CHARACTERS);
			text3 += "...";
		}
		text3 += "\n";
		ConsolePrompt.Log(condition + "\n", "");
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00008548 File Offset: 0x00006748
	private void Awake()
	{
		if (ConsolePrompt.instance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		ConsolePrompt.instance = this;
		this.CommandsGenerate();
		ConsolePrompt.ConsoleDisable();
		this.fpsText.enabled = Master.IsDebugBuild;
		if (Application.isEditor || !Master.IsDebugBuild)
		{
			this.interceptDebugLog = false;
		}
		if (this.interceptDebugLog)
		{
			this.RegisterDebugInterception();
		}
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x000085B4 File Offset: 0x000067B4
	private void Update()
	{
		bool flag = false;
		if (this.fpsText.enabled)
		{
			if (ConsolePrompt.captureMode)
			{
				this.fpsText.text = "";
			}
			else
			{
				this.fpsText.text = "FPS: " + (1f / Time.deltaTime).ToString("0.0");
			}
		}
		bool flag2 = Input.GetKey(306) || Input.GetKey(99);
		if (!this._enabled)
		{
			if (Input.GetKeyDown(9) && flag2)
			{
				ConsolePrompt.ConsoleEnable();
			}
			if (this.consoleHolder.activeSelf)
			{
				this.consoleHolder.SetActive(false);
			}
			this.autoCloseTimer = 0f;
			return;
		}
		if (Input.GetKeyDown(9) && flag2 && !flag)
		{
			ConsolePrompt.ConsoleDisable();
		}
		if (!this.consoleHolder.activeSelf)
		{
			this.consoleHolder.SetActive(true);
		}
		if (Input.GetKeyDown(273) && !flag2)
		{
			this.autoCloseTimer = 0f;
			this.inputString = this.inputStringOld;
		}
		if (Input.GetKey(306))
		{
			if (Input.GetKeyDown(273))
			{
				this.autoCloseTimer = 0f;
				this.outputCutStringsN++;
				this.outputCutStringsN = Mathf.Min(this.outputStringList.Count - 1, this.outputCutStringsN);
				this.outputCutStringsN = Mathf.Max(0, this.outputCutStringsN);
			}
			else if (Input.GetKeyDown(274))
			{
				this.autoCloseTimer = 0f;
				this.outputCutStringsN--;
				if (this.outputCutStringsN < 0)
				{
					this.outputCutStringsN = 0;
				}
			}
			if (Input.GetKeyDown(280))
			{
				this.autoCloseTimer = 0f;
				this.outputCutStringsN = this.outputStringList.Count - 1;
				this.outputCutStringsN = Mathf.Max(0, this.outputCutStringsN);
			}
			else if (Input.GetKeyDown(281))
			{
				this.autoCloseTimer = 0f;
				this.outputCutStringsN = 0;
			}
		}
		bool flag3 = Input.GetKey(306) || Input.GetKey(305);
		if (Input.anyKeyDown && Input.inputString.Length > 0)
		{
			char c = Input.inputString[Input.inputString.Length - 1];
			if (Input.GetKeyDown(8))
			{
				if (this.inputString.Length > 0)
				{
					if (flag3)
					{
						this.inputString = "";
					}
					else
					{
						this.inputString = this.inputString.Substring(0, this.inputString.Length - 1);
					}
				}
			}
			else if (Input.GetKeyDown(127))
			{
				this.inputString = "";
			}
			else if (c == '\n' || c == '\r' || Input.GetKeyDown(13))
			{
				this.TryExecuteCommand();
			}
			else
			{
				this.inputString += c.ToString();
			}
			this.autoCloseTimer = 0f;
		}
		this.caretFlickerTimer += Time.deltaTime;
		if (this.caretFlickerTimer > 0.5f)
		{
			this.caretFlickerTimer = 0f;
		}
		this.inputText.text = "> " + this.inputString + ((this.caretFlickerTimer > 0.25f) ? "_" : "");
		string text = "";
		int num = this.outputStringList.Count - this.outputCutStringsN;
		for (int i = 0; i < this.outputStringList.Count; i++)
		{
			text = text + this.outputStringList[i] + "\n";
			num--;
			if (num <= 0)
			{
				break;
			}
		}
		this.outputText.text = text;
		if (ConsolePrompt.CanAutoClose)
		{
			this.autoCloseTimer += Time.deltaTime;
			if (this.autoCloseTimer > 10f)
			{
				ConsolePrompt.ConsoleDisable();
			}
		}
	}

	public static ConsolePrompt instance;

	public const KeyCode enableDisableKey = 9;

	public int LOG_INTERCEPTION_MAX_CHARACTERS = 512;

	public const bool LOG_INTERCEPT_DISABLE_IF_NOT_DEBUG = true;

	public const bool LOG_INTERCEPT_ENABLE_STATE_DEFAULT = false;

	private const int PLAYER_INDEX = 0;

	public GameObject consoleHolder;

	public TextMeshProUGUI inputText;

	public TextMeshProUGUI outputText;

	public TextMeshProUGUI fpsText;

	public static bool captureMode;

	public static bool alarmAtTrapdoor;

	private bool _enabled;

	private string inputString = "";

	private string inputStringOld = "";

	private List<string> outputStringList = new List<string>(128);

	private int outputCutStringsN;

	private const string MESSAGE_LOG = "<color=white>Log: </color> ";

	private const string MESSAGE_WARNING = "<color=yellow>Warning: </color> ";

	private const string MESSAGE_ERROR = "<color=red>Error: </color> ";

	private float caretFlickerTimer;

	public const float AUTO_CLOSE_TIME = 10f;

	private float autoCloseTimer;

	private Dictionary<string, ConsolePrompt.Command> availableCommands = new Dictionary<string, ConsolePrompt.Command>();

	private int executionIndex = -1;

	private bool interceptDebugLog;

	private static bool _registeredDebugInterception;

	private class Command
	{
		// Token: 0x060010C2 RID: 4290 RVA: 0x00067334 File Offset: 0x00065534
		public Command(string[] allowedEntries, string description, UnityAction action)
		{
			this.allowedEntries = allowedEntries;
			this.description = description;
			this.action = action;
			foreach (string text in allowedEntries)
			{
				ConsolePrompt.instance.availableCommands.Add(text, this);
			}
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00067381 File Offset: 0x00065581
		public bool TryExecute()
		{
			if (this.action != null)
			{
				this.action.Invoke();
				return true;
			}
			return false;
		}

		public string[] allowedEntries;

		public string description;

		public UnityAction action;
	}
}
