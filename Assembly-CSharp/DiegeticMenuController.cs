using System;
using System.Collections.Generic;
using System.Linq;
using Panik;
using UnityEngine;
using UnityEngine.Events;

public class DiegeticMenuController : MonoBehaviour
{
	// (get) Token: 0x06000878 RID: 2168 RVA: 0x000378F0 File Offset: 0x00035AF0
	public static DiegeticMenuController ActiveMenu
	{
		get
		{
			if (DiegeticMenuController.stack.Count > 0)
			{
				return DiegeticMenuController.stack[DiegeticMenuController.stack.Count - 1];
			}
			return null;
		}
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x00037917 File Offset: 0x00035B17
	public bool IsCurrentMenu()
	{
		return DiegeticMenuController.ActiveMenu == this;
	}

	// (get) Token: 0x0600087A RID: 2170 RVA: 0x00037924 File Offset: 0x00035B24
	public static DiegeticMenuController MainMenu
	{
		get
		{
			foreach (DiegeticMenuController diegeticMenuController in DiegeticMenuController.all)
			{
				if (diegeticMenuController.isMainMenu)
				{
					return diegeticMenuController;
				}
			}
			return null;
		}
	}

	// (get) Token: 0x0600087B RID: 2171 RVA: 0x00037980 File Offset: 0x00035B80
	public static DiegeticMenuController SlotMenu
	{
		get
		{
			foreach (DiegeticMenuController diegeticMenuController in DiegeticMenuController.all)
			{
				if (diegeticMenuController.isSlotMenu)
				{
					return diegeticMenuController;
				}
			}
			return null;
		}
	}

	// (get) Token: 0x0600087C RID: 2172 RVA: 0x000379DC File Offset: 0x00035BDC
	public static DiegeticMenuController MainMenuMenu
	{
		get
		{
			foreach (DiegeticMenuController diegeticMenuController in DiegeticMenuController.all)
			{
				if (diegeticMenuController.isMainMenuMenu)
				{
					return diegeticMenuController;
				}
			}
			return null;
		}
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00037A38 File Offset: 0x00035C38
	private void GetElements()
	{
		this.elements = base.GetComponentsInChildren<DiegeticMenuElement>().ToList<DiegeticMenuElement>();
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x00037A4B File Offset: 0x00035C4B
	public bool IsRunning()
	{
		return this._isRunning;
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x00037A53 File Offset: 0x00035C53
	public void SetDelay(float delay)
	{
		this.runningDelay = delay;
	}

	// (get) Token: 0x06000880 RID: 2176 RVA: 0x00037A5C File Offset: 0x00035C5C
	// (set) Token: 0x06000881 RID: 2177 RVA: 0x00037A64 File Offset: 0x00035C64
	public DiegeticMenuElement HoveredElement
	{
		get
		{
			return this._hoveredElement;
		}
		set
		{
			this._hoveredElement = value;
			if (this._hoveredElement != null)
			{
				this._hoveredElement.RefreshHovering(true);
			}
		}
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00037A88 File Offset: 0x00035C88
	public DiegeticMenuElement PickAStartingElement()
	{
		if (VirtualCursors.IsCursorVisible(0, true) || AimCrossScript.IsEnabled())
		{
			return null;
		}
		if (this.firstElement != null)
		{
			return this.firstElement;
		}
		if (this.elements.Count > 0)
		{
			return this.elements[0];
		}
		return null;
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x00037AD8 File Offset: 0x00035CD8
	public void Back()
	{
		if (DiegeticMenuController.ActiveMenu != this)
		{
			return;
		}
		if (this.isMainMenu)
		{
			return;
		}
		if (DiegeticMenuController.stack.Count == 1)
		{
			return;
		}
		DiegeticMenuController.stack.Remove(this);
		if (this.soundOnBack != null && !Sound.IsPlaying(this.soundOnBack.name))
		{
			Sound.Play(this.soundOnBack.name, 1f, 1f);
		}
		UnityEvent unityEvent = this.onBack;
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
		foreach (DiegeticMenuElement diegeticMenuElement in this.elements)
		{
			UnityEvent onBackCallback = diegeticMenuElement.onBackCallback;
			if (onBackCallback != null)
			{
				onBackCallback.Invoke();
			}
		}
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00037BB0 File Offset: 0x00035DB0
	public void OpenMe()
	{
		if (DiegeticMenuController.stack.Contains(this))
		{
			Debug.LogError("Circular menu selection is not supported yet. Menu is already in the stack: " + base.name);
		}
		DiegeticMenuController.stack.Add(this);
		this._hoveredElement = this.PickAStartingElement();
		UnityEvent onOpen = this.OnOpen;
		if (onOpen != null)
		{
			onOpen.Invoke();
		}
		if (this.firstElement != null && !this.elements.Contains(this.firstElement))
		{
			Debug.LogError("DiegeticMenuController: First element is not in the list of elements. Menu: " + base.name);
		}
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x00037C3D File Offset: 0x00035E3D
	public void NavigationDisable_SetReason(string reason)
	{
		if (!this._navigationDisabledReasons.Contains(reason))
		{
			this._navigationDisabledReasons.Add(reason);
		}
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00037C59 File Offset: 0x00035E59
	public void NavigationDisable_RemoveReason(string reason)
	{
		if (this._navigationDisabledReasons.Contains(reason))
		{
			this._navigationDisabledReasons.Remove(reason);
		}
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x00037C78 File Offset: 0x00035E78
	public void RecalculateNavigationBetweenMyElements_XZ(float yAngOffset = 0f)
	{
		foreach (DiegeticMenuElement diegeticMenuElement in this.elements)
		{
			diegeticMenuElement.up = null;
			diegeticMenuElement.down = null;
			diegeticMenuElement.left = null;
			diegeticMenuElement.right = null;
		}
		for (int i = 0; i < this.elements.Count; i++)
		{
			DiegeticMenuElement diegeticMenuElement2 = this.elements[i];
			if (!(diegeticMenuElement2 == null))
			{
				this._FindClosestElementInDir(diegeticMenuElement2, ref diegeticMenuElement2.up, 90f + yAngOffset, 45f, false);
				if (diegeticMenuElement2.up == null)
				{
					this._FindClosestElementInDir(diegeticMenuElement2, ref diegeticMenuElement2.up, 90f + yAngOffset, 80f, false);
				}
				this._FindClosestElementInDir(diegeticMenuElement2, ref diegeticMenuElement2.down, 270f + yAngOffset, 45f, false);
				if (diegeticMenuElement2.down == null)
				{
					this._FindClosestElementInDir(diegeticMenuElement2, ref diegeticMenuElement2.down, 270f + yAngOffset, 80f, false);
				}
				this._FindClosestElementInDir(diegeticMenuElement2, ref diegeticMenuElement2.left, 180f + yAngOffset, 45f, false);
				if (diegeticMenuElement2.left == null)
				{
					this._FindClosestElementInDir(diegeticMenuElement2, ref diegeticMenuElement2.left, 180f + yAngOffset, 80f, false);
				}
				this._FindClosestElementInDir(diegeticMenuElement2, ref diegeticMenuElement2.right, 0f + yAngOffset, 45f, true);
				if (diegeticMenuElement2.right == null)
				{
					this._FindClosestElementInDir(diegeticMenuElement2, ref diegeticMenuElement2.right, 0f + yAngOffset, 80f, true);
				}
			}
		}
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x00037E20 File Offset: 0x00036020
	private void _FindClosestElementInDir(DiegeticMenuElement currentElement, ref DiegeticMenuElement sideToAssign, float preferredAngle, float angDifMax, bool use180Max)
	{
		float num = float.MaxValue;
		foreach (DiegeticMenuElement diegeticMenuElement in this.elements)
		{
			if (!(diegeticMenuElement == null) && !(diegeticMenuElement == currentElement))
			{
				Vector2 vector = Camera.main.WorldToViewportPoint(currentElement.transform.position);
				Vector2 vector2 = Camera.main.WorldToViewportPoint(diegeticMenuElement.transform.position);
				float num2 = Vector2.Distance(vector, vector2);
				float num3 = Util.AxisToAngle2D(vector2.x - vector.x, vector2.y - vector.y);
				num3 = Mathf.Repeat(num3, 360f);
				if (use180Max && num3 > 180f)
				{
					num3 -= 360f;
				}
				if (Mathf.Abs(preferredAngle - num3) <= angDifMax && num2 < num)
				{
					num = num2;
					sideToAssign = diegeticMenuElement;
				}
			}
		}
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00037F34 File Offset: 0x00036134
	private void Reset()
	{
		if (this.elements.Count == 0)
		{
			this.elements = base.GetComponentsInChildren<DiegeticMenuElement>().ToList<DiegeticMenuElement>();
		}
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x00037F54 File Offset: 0x00036154
	private void Awake()
	{
		DiegeticMenuController.all.Add(this);
		foreach (DiegeticMenuElement diegeticMenuElement in this.elements)
		{
			diegeticMenuElement.SetMyController(this);
		}
		if (this.isMainMenu)
		{
			DiegeticMenuController.stack.Clear();
			this.OpenMe();
		}
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x00037FC8 File Offset: 0x000361C8
	private void OnDestroy()
	{
		DiegeticMenuController.all.Remove(this);
		DiegeticMenuController.stack.Remove(this);
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x00037FE4 File Offset: 0x000361E4
	private void Update()
	{
		this._isRunning = true;
		bool flag = false;
		this.runningDelay -= Tick.Time;
		if (this.OnCanRun != null && !this.OnCanRun())
		{
			flag = true;
		}
		if (DiegeticMenuController.ActiveMenu != this)
		{
			flag = true;
		}
		if (this._navigationDisabledReasons.Count > 0)
		{
			flag = true;
		}
		if (flag)
		{
			this.runningDelay = 0.5f;
			this._isRunning = false;
			return;
		}
		if (this.runningDelay > 0f)
		{
			this._isRunning = false;
			return;
		}
		Controls.GetPlayerByIndex(0);
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
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
		if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true))
		{
			flag2 = true;
		}
		if (this.allowInteractAction && Controls.ActionButton_PressedGet(0, Controls.InputAction.interact, true))
		{
			flag2 = true;
			flag5 = true;
		}
		if (Controls.MouseButton_PressedGet(0, Controls.MouseElement.LeftButton))
		{
			flag3 = true;
		}
		if (this.rightMouseCanSelect && Controls.MouseButton_PressedGet(0, Controls.MouseElement.RightButton))
		{
			flag2 = true;
			flag4 = true;
		}
		if (flag2 && flag3)
		{
			flag4 = true;
		}
		if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true))
		{
			flag6 = true;
		}
		if (this.elements.Count != 0)
		{
			DiegeticMenuElement diegeticMenuElement = null;
			if (!ConsolePrompt.ConsoleIsEnabled())
			{
				foreach (DiegeticMenuElement diegeticMenuElement2 in this.elements)
				{
					if (diegeticMenuElement2.directAction != Controls.InputAction._UNDEFINED && Controls.ActionButton_PressedGet(0, diegeticMenuElement2.directAction, true))
					{
						diegeticMenuElement = diegeticMenuElement2;
						this._hoveredElement = diegeticMenuElement2;
						break;
					}
				}
			}
			if (VirtualCursors.IsCursorVisible(0, true) || AimCrossScript.IsEnabled())
			{
				if (diegeticMenuElement == null)
				{
					this._hoveredElement = null;
					foreach (DiegeticMenuElement diegeticMenuElement3 in this.elements)
					{
						if (diegeticMenuElement3.IsMouseOnMe())
						{
							this._hoveredElement = diegeticMenuElement3;
							break;
						}
					}
				}
				if (flag4 || flag5 || diegeticMenuElement != null)
				{
					if (this._hoveredElement != null)
					{
						this._hoveredElement.Select(this);
						UnityEvent unityEvent = this.onSelect;
						if (unityEvent != null)
						{
							unityEvent.Invoke();
						}
					}
					else if (this.goBackIfClickNothing)
					{
						this.Back();
					}
				}
			}
			else if (this.allowButtonsNavigation)
			{
				bool flag7 = false;
				if (this._hoveredElement == null && (zero != Vector2.zero || (flag2 && !flag4)))
				{
					this._hoveredElement = this.PickAStartingElement();
					flag7 = true;
				}
				if (this._hoveredElement != null)
				{
					if (diegeticMenuElement == null && !flag7)
					{
						if (zero.y > 0f && this._hoveredElement.up != null && this._hoveredElement.up.gameObject.activeInHierarchy)
						{
							this._hoveredElement = this._hoveredElement.up;
						}
						if (zero.y < 0f && this._hoveredElement.down != null && this._hoveredElement.down.gameObject.activeInHierarchy)
						{
							this._hoveredElement = this._hoveredElement.down;
						}
						if (zero.x < 0f && this._hoveredElement.left != null && this._hoveredElement.left.gameObject.activeInHierarchy)
						{
							this._hoveredElement = this._hoveredElement.left;
						}
						if (zero.x > 0f && this._hoveredElement.right != null && this._hoveredElement.right.gameObject.activeInHierarchy)
						{
							this._hoveredElement = this._hoveredElement.right;
						}
					}
					if (((flag2 && !flag7) || diegeticMenuElement != null) && this._hoveredElement != null)
					{
						this._hoveredElement.Select(this);
						UnityEvent unityEvent2 = this.onSelect;
						if (unityEvent2 != null)
						{
							unityEvent2.Invoke();
						}
					}
				}
			}
		}
		string name = VirtualCursors.CursorGet(0).name;
		string name2 = VirtualCursors.CursorGetDeafault(0).name;
		if (this._hoveredElement != null && this._hoveredElement.cursorAnim != null)
		{
			string name3 = this._hoveredElement.cursorAnim.name;
			if (name != name3)
			{
				VirtualCursors.CursorSet(0, name3);
			}
		}
		else if (name != name2)
		{
			VirtualCursors.CursorSetDefault(0);
		}
		if (flag6 && this.goBackWithButton)
		{
			this.Back();
		}
		UnityEvent onRunning = this.OnRunning;
		if (onRunning == null)
		{
			return;
		}
		onRunning.Invoke();
	}

	public static List<DiegeticMenuController> all = new List<DiegeticMenuController>();

	public static List<DiegeticMenuController> stack = new List<DiegeticMenuController>();

	public List<DiegeticMenuElement> elements = new List<DiegeticMenuElement>();

	public bool _buttonField_GetElements;

	public DiegeticMenuElement firstElement;

	private bool _isRunning;

	private float runningDelay;

	public bool allowInteractAction = true;

	public bool allowButtonsNavigation = true;

	private bool goBackIfClickNothing;

	public bool goBackWithButton = true;

	public bool isMainMenu;

	public bool isSlotMenu;

	public bool isMainMenuMenu;

	public AudioClip soundOnBack;

	public bool rightMouseCanSelect;

	private DiegeticMenuElement _hoveredElement;

	private List<string> _navigationDisabledReasons = new List<string>();

	private Vector2 axisRawPrevious = Vector2.zero;

	public UnityEvent onSelect;

	public UnityEvent onBack;

	public UnityEvent OnOpen;

	public UnityEvent OnRunning;

	public DiegeticMenuController.CanRun OnCanRun;

	// (Invoke) Token: 0x060011F0 RID: 4592
	public delegate bool CanRun();
}
