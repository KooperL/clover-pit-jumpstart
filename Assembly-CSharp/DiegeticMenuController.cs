using System;
using System.Collections.Generic;
using System.Linq;
using Panik;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000B0 RID: 176
public class DiegeticMenuController : MonoBehaviour
{
	// Token: 0x17000069 RID: 105
	// (get) Token: 0x0600099F RID: 2463 RVA: 0x0000D991 File Offset: 0x0000BB91
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

	// Token: 0x060009A0 RID: 2464 RVA: 0x0000D9B8 File Offset: 0x0000BBB8
	public bool IsCurrentMenu()
	{
		return DiegeticMenuController.ActiveMenu == this;
	}

	// Token: 0x1700006A RID: 106
	// (get) Token: 0x060009A1 RID: 2465 RVA: 0x0004EB54 File Offset: 0x0004CD54
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

	// Token: 0x1700006B RID: 107
	// (get) Token: 0x060009A2 RID: 2466 RVA: 0x0004EBB0 File Offset: 0x0004CDB0
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

	// Token: 0x1700006C RID: 108
	// (get) Token: 0x060009A3 RID: 2467 RVA: 0x0004EC0C File Offset: 0x0004CE0C
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

	// Token: 0x060009A4 RID: 2468 RVA: 0x0000D9C5 File Offset: 0x0000BBC5
	private void GetElements()
	{
		this.elements = base.GetComponentsInChildren<DiegeticMenuElement>().ToList<DiegeticMenuElement>();
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x0000D9D8 File Offset: 0x0000BBD8
	public bool IsRunning()
	{
		return this._isRunning;
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x0000D9E0 File Offset: 0x0000BBE0
	public void SetDelay(float delay)
	{
		this.runningDelay = delay;
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060009A7 RID: 2471 RVA: 0x0000D9E9 File Offset: 0x0000BBE9
	// (set) Token: 0x060009A8 RID: 2472 RVA: 0x0000D9F1 File Offset: 0x0000BBF1
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

	// Token: 0x060009A9 RID: 2473 RVA: 0x0004EC68 File Offset: 0x0004CE68
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

	// Token: 0x060009AA RID: 2474 RVA: 0x0004ECB8 File Offset: 0x0004CEB8
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

	// Token: 0x060009AB RID: 2475 RVA: 0x0004ED90 File Offset: 0x0004CF90
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

	// Token: 0x060009AC RID: 2476 RVA: 0x0000DA14 File Offset: 0x0000BC14
	public void NavigationDisable_SetReason(string reason)
	{
		if (!this._navigationDisabledReasons.Contains(reason))
		{
			this._navigationDisabledReasons.Add(reason);
		}
	}

	// Token: 0x060009AD RID: 2477 RVA: 0x0000DA30 File Offset: 0x0000BC30
	public void NavigationDisable_RemoveReason(string reason)
	{
		if (this._navigationDisabledReasons.Contains(reason))
		{
			this._navigationDisabledReasons.Remove(reason);
		}
	}

	// Token: 0x060009AE RID: 2478 RVA: 0x0004EE20 File Offset: 0x0004D020
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

	// Token: 0x060009AF RID: 2479 RVA: 0x0004EFC8 File Offset: 0x0004D1C8
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

	// Token: 0x060009B0 RID: 2480 RVA: 0x0000DA4D File Offset: 0x0000BC4D
	private void Reset()
	{
		if (this.elements.Count == 0)
		{
			this.elements = base.GetComponentsInChildren<DiegeticMenuElement>().ToList<DiegeticMenuElement>();
		}
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x0004F0DC File Offset: 0x0004D2DC
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

	// Token: 0x060009B2 RID: 2482 RVA: 0x0000DA6D File Offset: 0x0000BC6D
	private void OnDestroy()
	{
		DiegeticMenuController.all.Remove(this);
		DiegeticMenuController.stack.Remove(this);
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x0004F150 File Offset: 0x0004D350
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

	// Token: 0x040009A4 RID: 2468
	public static List<DiegeticMenuController> all = new List<DiegeticMenuController>();

	// Token: 0x040009A5 RID: 2469
	public static List<DiegeticMenuController> stack = new List<DiegeticMenuController>();

	// Token: 0x040009A6 RID: 2470
	public List<DiegeticMenuElement> elements = new List<DiegeticMenuElement>();

	// Token: 0x040009A7 RID: 2471
	public bool _buttonField_GetElements;

	// Token: 0x040009A8 RID: 2472
	public DiegeticMenuElement firstElement;

	// Token: 0x040009A9 RID: 2473
	private bool _isRunning;

	// Token: 0x040009AA RID: 2474
	private float runningDelay;

	// Token: 0x040009AB RID: 2475
	public bool allowInteractAction = true;

	// Token: 0x040009AC RID: 2476
	public bool allowButtonsNavigation = true;

	// Token: 0x040009AD RID: 2477
	private bool goBackIfClickNothing;

	// Token: 0x040009AE RID: 2478
	public bool goBackWithButton = true;

	// Token: 0x040009AF RID: 2479
	public bool isMainMenu;

	// Token: 0x040009B0 RID: 2480
	public bool isSlotMenu;

	// Token: 0x040009B1 RID: 2481
	public bool isMainMenuMenu;

	// Token: 0x040009B2 RID: 2482
	public AudioClip soundOnBack;

	// Token: 0x040009B3 RID: 2483
	public bool rightMouseCanSelect;

	// Token: 0x040009B4 RID: 2484
	private DiegeticMenuElement _hoveredElement;

	// Token: 0x040009B5 RID: 2485
	private List<string> _navigationDisabledReasons = new List<string>();

	// Token: 0x040009B6 RID: 2486
	private Vector2 axisRawPrevious = Vector2.zero;

	// Token: 0x040009B7 RID: 2487
	public UnityEvent onSelect;

	// Token: 0x040009B8 RID: 2488
	public UnityEvent onBack;

	// Token: 0x040009B9 RID: 2489
	public UnityEvent OnOpen;

	// Token: 0x040009BA RID: 2490
	public UnityEvent OnRunning;

	// Token: 0x040009BB RID: 2491
	public DiegeticMenuController.CanRun OnCanRun;

	// Token: 0x020000B1 RID: 177
	// (Invoke) Token: 0x060009B7 RID: 2487
	public delegate bool CanRun();
}
