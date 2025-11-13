using System;
using Panik;
using UnityEngine;
using UnityEngine.Events;

public class DiegeticMenuElement : MonoBehaviour
{
	// Token: 0x0600088F RID: 2191 RVA: 0x000385CC File Offset: 0x000367CC
	public void SetMyController(DiegeticMenuController controller)
	{
		this.myController = controller;
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x000385D5 File Offset: 0x000367D5
	public bool IsHovered()
	{
		return this.isHoveredOld;
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x000385DD File Offset: 0x000367DD
	public void RefreshHovering(bool desiredHoverState)
	{
		this.isHoveredOld = !desiredHoverState;
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x000385E9 File Offset: 0x000367E9
	public bool IsMouseOnMe()
	{
		return this._mouseOver;
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x000385F4 File Offset: 0x000367F4
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

	// Token: 0x06000894 RID: 2196 RVA: 0x00038798 File Offset: 0x00036998
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

	// Token: 0x06000895 RID: 2197 RVA: 0x0003884E File Offset: 0x00036A4E
	public void DirectActionControl_Set(Controls.InputAction action)
	{
		this.directAction = action;
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00038857 File Offset: 0x00036A57
	public Controls.InputAction DirectActionControl_Get()
	{
		return this.directAction;
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0003885F File Offset: 0x00036A5F
	public void SetPromptGuide()
	{
		PromptGuideScript.SetGuideType(this.promptGuideType);
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x0003886C File Offset: 0x00036A6C
	public void SetPromptGuide_Powerup()
	{
		if (GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.gambling)
		{
			return;
		}
		PromptGuideScript.SetGuideType(this.promptGuideType);
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x00038882 File Offset: 0x00036A82
	public void IgnoreOnSlotSpinning()
	{
		if (SlotMachineScript.StateGet() == SlotMachineScript.State.spinning)
		{
			this._ignoreConditionTriggered = true;
		}
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x00038893 File Offset: 0x00036A93
	public void IgnoreOnSlotAutoSpin()
	{
		if (SlotMachineScript.instance.IsAutoSpinning())
		{
			this._ignoreConditionTriggered = true;
		}
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x000388A8 File Offset: 0x00036AA8
	private void Reset()
	{
		this.myOutline = base.GetComponent<Outline>();
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x000388B6 File Offset: 0x00036AB6
	private void Awake()
	{
		this.myCollider = base.GetComponent<Collider>();
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x000388C4 File Offset: 0x00036AC4
	private void OnDisable()
	{
		if (this.myOutline != null)
		{
			this.myOutline.enabled = false;
		}
	}

	// Token: 0x0600089E RID: 2206 RVA: 0x000388E0 File Offset: 0x00036AE0
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

	// Token: 0x0600089F RID: 2207 RVA: 0x00038938 File Offset: 0x00036B38
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

	private const int PLAYER_INDEX = 0;

	private Controls.PlayerExt player;

	private DiegeticMenuController myController;

	public Outline myOutline;

	private Collider myCollider;

	private Camera gameCamera;

	public bool IllGiveYouAMenuLater;

	public bool vibration_Select = true;

	public bool vibration_Hover = true;

	public AudioClip soundOnHover;

	public AudioClip soundOnSelect;

	public float audioPitchMin = 1f;

	public float audioPitchMax = 1f;

	public DiegeticMenuElement.HoverVisualBehaviour hoverVisualBehaviour = DiegeticMenuElement.HoverVisualBehaviour.onHover;

	public bool rainbowOutline;

	[NonSerialized]
	public Controls.InputAction directAction = Controls.InputAction._UNDEFINED;

	public PromptGuideScript.GuideType promptGuideType = PromptGuideScript.GuideType.Undefined;

	public FrameAnimation cursorAnim;

	private float minimumMouseDistance = 8f;

	public DiegeticMenuElement up;

	public DiegeticMenuElement down;

	public DiegeticMenuElement left;

	public DiegeticMenuElement right;

	private bool isHoveredOld;

	private bool _mouseOver;

	private RaycastHit[] hits = new RaycastHit[10];

	public UnityEvent onSelectCallback;

	public UnityEvent onBackCallback;

	public UnityEvent onHoverCallback;

	public UnityEvent onHoverStayCallback;

	public UnityEvent onIgnoreConditionCallback;

	private bool _ignoreConditionTriggered;

	private float justSelectedAnimationTimer;

	public enum HoverVisualBehaviour
	{
		alwaysOff,
		onHover,
		onSelectOrMouseHover
	}
}
