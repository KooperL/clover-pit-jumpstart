using System;
using Panik;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000B2 RID: 178
public class DiegeticMenuElement : MonoBehaviour
{
	// Token: 0x060009BA RID: 2490 RVA: 0x0000DADB File Offset: 0x0000BCDB
	public void SetMyController(DiegeticMenuController controller)
	{
		this.myController = controller;
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x0000DAE4 File Offset: 0x0000BCE4
	public bool IsHovered()
	{
		return this.isHoveredOld;
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x0000DAEC File Offset: 0x0000BCEC
	public void RefreshHovering(bool desiredHoverState)
	{
		this.isHoveredOld = !desiredHoverState;
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x0000DAF8 File Offset: 0x0000BCF8
	public bool IsMouseOnMe()
	{
		return this._mouseOver;
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x0004F6E4 File Offset: 0x0004D8E4
	private bool IsMouseOver()
	{
		bool flag = VirtualCursors.IsCursorVisible(0, true);
		if (!flag && !AimCrossScript.IsEnabled())
		{
			return false;
		}
		Vector2 vector = new Vector2(0.5f, 0.5f);
		if (flag)
		{
			Vector2 vector2 = new Vector2((float)this.gameCamera.pixelWidth, (float)this.gameCamera.pixelHeight);
			vector = VirtualCursors.CursorPositionNormalizedCenteredGet_ReferenceResolution(0, vector2);
			vector.x += 0.5f;
			vector.y += 0.5f;
		}
		int num = Physics.RaycastNonAlloc(this.gameCamera.ViewportPointToRay(new Vector3(vector.x, vector.y, 0f)), this.hits, 100f);
		float num2 = float.MaxValue;
		float num3 = num2;
		bool flag2 = false;
		for (int i = 0; i < num; i++)
		{
			DiegeticMenuElement component = this.hits[i].collider.GetComponent<DiegeticMenuElement>();
			if (!(component == null) && !(component.myController != DiegeticMenuController.ActiveMenu) && component.enabled)
			{
				if (this.hits[i].collider.gameObject == base.gameObject)
				{
					flag2 = true;
					if (num3 > this.hits[i].distance)
					{
						num3 = this.hits[i].distance;
					}
				}
				if (num2 > this.hits[i].distance)
				{
					num2 = this.hits[i].distance;
				}
			}
		}
		if (num3 > num2 || num3 > this.minimumMouseDistance)
		{
			flag2 = false;
		}
		return flag2;
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x0004F888 File Offset: 0x0004DA88
	public void Select(DiegeticMenuController controller)
	{
		this._ignoreConditionTriggered = false;
		if (this.onIgnoreConditionCallback != null)
		{
			this.onIgnoreConditionCallback.Invoke();
			if (this._ignoreConditionTriggered)
			{
				return;
			}
		}
		if (SlotMachineScript.StateGet() == SlotMachineScript.State.spinning)
		{
			return;
		}
		if (this.onSelectCallback != null)
		{
			this.onSelectCallback.Invoke();
		}
		if (this.soundOnSelect != null && !Sound.IsPlaying(this.soundOnSelect.name))
		{
			Sound.Play(this.soundOnSelect.name, 1f, global::UnityEngine.Random.Range(this.audioPitchMin, this.audioPitchMax));
		}
		this.justSelectedAnimationTimer = 0.2f;
		if (this.vibration_Select)
		{
			Controls.VibrationSet_PreferMax(this.player, 0.25f);
		}
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x0000DB00 File Offset: 0x0000BD00
	public void DirectActionControl_Set(Controls.InputAction action)
	{
		this.directAction = action;
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x0000DB09 File Offset: 0x0000BD09
	public Controls.InputAction DirectActionControl_Get()
	{
		return this.directAction;
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x0000DB11 File Offset: 0x0000BD11
	public void SetPromptGuide()
	{
		PromptGuideScript.SetGuideType(this.promptGuideType);
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0000DB1E File Offset: 0x0000BD1E
	public void SetPromptGuide_Powerup()
	{
		if (GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.gambling)
		{
			return;
		}
		PromptGuideScript.SetGuideType(this.promptGuideType);
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x0000DB34 File Offset: 0x0000BD34
	public void IgnoreOnSlotSpinning()
	{
		if (SlotMachineScript.StateGet() == SlotMachineScript.State.spinning)
		{
			this._ignoreConditionTriggered = true;
		}
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x0000DB45 File Offset: 0x0000BD45
	public void IgnoreOnSlotAutoSpin()
	{
		if (SlotMachineScript.instance.IsAutoSpinning())
		{
			this._ignoreConditionTriggered = true;
		}
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x0000DB5A File Offset: 0x0000BD5A
	private void Reset()
	{
		this.myOutline = base.GetComponent<Outline>();
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x0000DB68 File Offset: 0x0000BD68
	private void Awake()
	{
		this.myCollider = base.GetComponent<Collider>();
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x0000DB76 File Offset: 0x0000BD76
	private void OnDisable()
	{
		if (this.myOutline != null)
		{
			this.myOutline.enabled = false;
		}
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x0004F940 File Offset: 0x0004DB40
	private void Start()
	{
		if (this.myController == null && !this.IllGiveYouAMenuLater)
		{
			Debug.LogError("DiegeticMenuElement: No menu controller found. Disabling.");
			base.enabled = false;
			return;
		}
		this.player = Controls.GetPlayerByIndex(0);
		this.gameCamera = CameraGame.list[0].myCamera;
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x0004F998 File Offset: 0x0004DB98
	private void Update()
	{
		if (this.IllGiveYouAMenuLater && this.myController == null)
		{
			return;
		}
		bool flag = this.myController.IsRunning();
		this._mouseOver = flag && this.IsMouseOver();
		bool flag2 = flag && this.myController.HoveredElement == this;
		if (this.isHoveredOld != flag2)
		{
			this.isHoveredOld = flag2;
			if (flag2 && this.onHoverCallback != null)
			{
				this.onHoverCallback.Invoke();
			}
			if (flag2 && this.soundOnHover != null && !CameraDebug.IsEnabled())
			{
				Sound.Play(this.soundOnHover.name, 1f, global::UnityEngine.Random.Range(this.audioPitchMin, this.audioPitchMax));
			}
			if (flag2 && this.vibration_Hover)
			{
				Controls.VibrationSet_PreferMax(this.player, 0.25f);
			}
		}
		if (!flag2)
		{
			if (this.myOutline != null)
			{
				this.myOutline.enabled = false;
			}
		}
		else
		{
			bool flag3 = false;
			switch (this.hoverVisualBehaviour)
			{
			case DiegeticMenuElement.HoverVisualBehaviour.alwaysOff:
				if (this.myOutline != null)
				{
					flag3 = false;
				}
				break;
			case DiegeticMenuElement.HoverVisualBehaviour.onHover:
				if (this.myOutline != null)
				{
					flag3 = flag2;
				}
				break;
			case DiegeticMenuElement.HoverVisualBehaviour.onSelectOrMouseHover:
				if (this.myOutline != null)
				{
					flag3 = this.justSelectedAnimationTimer > 0f || (flag2 && this._mouseOver);
				}
				break;
			}
			if (CameraDebug.IsEnabled())
			{
				flag3 = false;
			}
			if (this.myOutline != null && this.myOutline.enabled != flag3)
			{
				this.myOutline.enabled = flag3;
			}
			if (this.rainbowOutline)
			{
				this.myOutline.OutlineColor = Colors.GetRainbowColor_Unpausable();
			}
		}
		this.justSelectedAnimationTimer -= Tick.Time;
		this.justSelectedAnimationTimer = Mathf.Max(this.justSelectedAnimationTimer, -1f);
	}

	// Token: 0x040009BC RID: 2492
	private const int PLAYER_INDEX = 0;

	// Token: 0x040009BD RID: 2493
	private Controls.PlayerExt player;

	// Token: 0x040009BE RID: 2494
	private DiegeticMenuController myController;

	// Token: 0x040009BF RID: 2495
	public Outline myOutline;

	// Token: 0x040009C0 RID: 2496
	private Collider myCollider;

	// Token: 0x040009C1 RID: 2497
	private Camera gameCamera;

	// Token: 0x040009C2 RID: 2498
	public bool IllGiveYouAMenuLater;

	// Token: 0x040009C3 RID: 2499
	public bool vibration_Select = true;

	// Token: 0x040009C4 RID: 2500
	public bool vibration_Hover = true;

	// Token: 0x040009C5 RID: 2501
	public AudioClip soundOnHover;

	// Token: 0x040009C6 RID: 2502
	public AudioClip soundOnSelect;

	// Token: 0x040009C7 RID: 2503
	public float audioPitchMin = 1f;

	// Token: 0x040009C8 RID: 2504
	public float audioPitchMax = 1f;

	// Token: 0x040009C9 RID: 2505
	public DiegeticMenuElement.HoverVisualBehaviour hoverVisualBehaviour = DiegeticMenuElement.HoverVisualBehaviour.onHover;

	// Token: 0x040009CA RID: 2506
	public bool rainbowOutline;

	// Token: 0x040009CB RID: 2507
	[NonSerialized]
	public Controls.InputAction directAction = Controls.InputAction._UNDEFINED;

	// Token: 0x040009CC RID: 2508
	public PromptGuideScript.GuideType promptGuideType = PromptGuideScript.GuideType.Undefined;

	// Token: 0x040009CD RID: 2509
	public FrameAnimation cursorAnim;

	// Token: 0x040009CE RID: 2510
	private float minimumMouseDistance = 8f;

	// Token: 0x040009CF RID: 2511
	public DiegeticMenuElement up;

	// Token: 0x040009D0 RID: 2512
	public DiegeticMenuElement down;

	// Token: 0x040009D1 RID: 2513
	public DiegeticMenuElement left;

	// Token: 0x040009D2 RID: 2514
	public DiegeticMenuElement right;

	// Token: 0x040009D3 RID: 2515
	private bool isHoveredOld;

	// Token: 0x040009D4 RID: 2516
	private bool _mouseOver;

	// Token: 0x040009D5 RID: 2517
	private RaycastHit[] hits = new RaycastHit[10];

	// Token: 0x040009D6 RID: 2518
	public UnityEvent onSelectCallback;

	// Token: 0x040009D7 RID: 2519
	public UnityEvent onBackCallback;

	// Token: 0x040009D8 RID: 2520
	public UnityEvent onHoverCallback;

	// Token: 0x040009D9 RID: 2521
	public UnityEvent onHoverStayCallback;

	// Token: 0x040009DA RID: 2522
	public UnityEvent onIgnoreConditionCallback;

	// Token: 0x040009DB RID: 2523
	private bool _ignoreConditionTriggered;

	// Token: 0x040009DC RID: 2524
	private float justSelectedAnimationTimer;

	// Token: 0x020000B3 RID: 179
	public enum HoverVisualBehaviour
	{
		// Token: 0x040009DE RID: 2526
		alwaysOff,
		// Token: 0x040009DF RID: 2527
		onHover,
		// Token: 0x040009E0 RID: 2528
		onSelectOrMouseHover
	}
}
