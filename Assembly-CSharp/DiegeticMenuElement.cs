using System;
using Panik;
using UnityEngine;
using UnityEngine.Events;

public class DiegeticMenuElement : MonoBehaviour
{
	// Token: 0x06000888 RID: 2184 RVA: 0x0003834C File Offset: 0x0003654C
	public void SetMyController(DiegeticMenuController controller)
	{
		this.myController = controller;
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00038355 File Offset: 0x00036555
	public bool IsHovered()
	{
		return this.isHoveredOld;
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x0003835D File Offset: 0x0003655D
	public void RefreshHovering(bool desiredHoverState)
	{
		this.isHoveredOld = !desiredHoverState;
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x00038369 File Offset: 0x00036569
	public bool IsMouseOnMe()
	{
		return this._mouseOver;
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x00038374 File Offset: 0x00036574
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

	// Token: 0x0600088D RID: 2189 RVA: 0x00038518 File Offset: 0x00036718
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

	// Token: 0x0600088E RID: 2190 RVA: 0x000385CE File Offset: 0x000367CE
	public void DirectActionControl_Set(Controls.InputAction action)
	{
		this.directAction = action;
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x000385D7 File Offset: 0x000367D7
	public Controls.InputAction DirectActionControl_Get()
	{
		return this.directAction;
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x000385DF File Offset: 0x000367DF
	public void SetPromptGuide()
	{
		PromptGuideScript.SetGuideType(this.promptGuideType);
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x000385EC File Offset: 0x000367EC
	public void SetPromptGuide_Powerup()
	{
		if (GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.gambling)
		{
			return;
		}
		PromptGuideScript.SetGuideType(this.promptGuideType);
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x00038602 File Offset: 0x00036802
	public void IgnoreOnSlotSpinning()
	{
		if (SlotMachineScript.StateGet() == SlotMachineScript.State.spinning)
		{
			this._ignoreConditionTriggered = true;
		}
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x00038613 File Offset: 0x00036813
	public void IgnoreOnSlotAutoSpin()
	{
		if (SlotMachineScript.instance.IsAutoSpinning())
		{
			this._ignoreConditionTriggered = true;
		}
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x00038628 File Offset: 0x00036828
	private void Reset()
	{
		this.myOutline = base.GetComponent<Outline>();
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x00038636 File Offset: 0x00036836
	private void Awake()
	{
		this.myCollider = base.GetComponent<Collider>();
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x00038644 File Offset: 0x00036844
	private void OnDisable()
	{
		if (this.myOutline != null)
		{
			this.myOutline.enabled = false;
		}
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x00038660 File Offset: 0x00036860
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

	// Token: 0x06000898 RID: 2200 RVA: 0x000386B8 File Offset: 0x000368B8
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
