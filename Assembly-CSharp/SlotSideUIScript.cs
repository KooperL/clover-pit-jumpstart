using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotSideUIScript : MonoBehaviour
{
	// Token: 0x06000A15 RID: 2581 RVA: 0x000454D4 File Offset: 0x000436D4
	public static bool IsEnabled()
	{
		return !(SlotSideUIScript.instance == null) && SlotSideUIScript.instance._enabledState;
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x000454EF File Offset: 0x000436EF
	private static void SetEnableTime(float time)
	{
		if (SlotSideUIScript.instance == null)
		{
			return;
		}
		SlotSideUIScript.instance.enableTimer = Mathf.Max(SlotSideUIScript.instance.enableTimer, time);
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x00045519 File Offset: 0x00043719
	public static void ShowTry()
	{
		SlotSideUIScript.SetEnableTime(3f);
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x00045528 File Offset: 0x00043728
	private void Awake()
	{
		SlotSideUIScript.instance = this;
		this.leftPromptStartPosition = this.leftPromptHolder.anchoredPosition;
		this.rightPromptStartPosition = this.rightPromptHolder.anchoredPosition;
		this.leftPromptHidePosition = this.leftPromptStartPosition + new Vector2(-100f, 0f);
		this.rightPromptHidePosition = this.rightPromptStartPosition + new Vector2(100f, 0f);
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x0004559D File Offset: 0x0004379D
	private void Start()
	{
		this.leftPromptHolder.anchoredPosition = this.leftPromptHidePosition;
		this.rightPromptHolder.anchoredPosition = this.rightPromptHidePosition;
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x000455C1 File Offset: 0x000437C1
	private void OnDestroy()
	{
		if (SlotSideUIScript.instance == this)
		{
			SlotSideUIScript.instance = null;
		}
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x000455D8 File Offset: 0x000437D8
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		CameraController.SlotMachineLookingSides slotMachineLookingSides = CameraController.SlotMachineLook_Get();
		GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
		bool flag = false;
		bool flag2 = false;
		bool flag3 = this.enableTimer > 0f && gamePhase == GameplayMaster.GamePhase.gambling && !SlotMachineScript.IsSpinning() && SlotMachineScript.IsTurnedOn() && !RedButtonScript.UiIsShowing();
		if (!flag3)
		{
			this.enableTimer = 0f;
		}
		else
		{
			this.enableTimer -= Tick.Time;
		}
		Vector2 vector = this.leftPromptStartPosition;
		if (!flag3 || slotMachineLookingSides == CameraController.SlotMachineLookingSides.left)
		{
			flag = true;
			vector = this.leftPromptHidePosition;
		}
		this.leftPromptHolder.anchoredPosition = Vector2.Lerp(this.leftPromptHolder.anchoredPosition, vector, Tick.Time * 20f);
		vector = this.rightPromptStartPosition;
		if (!flag3 || slotMachineLookingSides == CameraController.SlotMachineLookingSides.right)
		{
			flag2 = true;
			vector = this.rightPromptHidePosition;
		}
		this.rightPromptHolder.anchoredPosition = Vector2.Lerp(this.rightPromptHolder.anchoredPosition, vector, Tick.Time * 20f);
		if (!flag3)
		{
			return;
		}
		Vector2 zero;
		zero..ctor(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		if (Data.settings.dyslexicFontEnabled)
		{
			zero = Vector2.zero;
		}
		this.leftPromptBackImage.rectTransform.anchoredPosition = zero;
		this.rightPromptBackImage.rectTransform.anchoredPosition = zero;
		if (!flag)
		{
			this.leftPromptText.text = Controls.MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(0, Controls.InputAction.menuTabLeft, 0);
		}
		if (!flag2)
		{
			this.rightPromptText.text = Controls.MapGetLastPrompt_TextSprite_FavorKeyboardOverMouse(0, Controls.InputAction.menuTabRight, 0);
		}
	}

	public static SlotSideUIScript instance;

	public const int PLAYER_INDEX = 0;

	private const float ENABLE_TIME_DEFAULT = 3f;

	public RectTransform leftPromptHolder;

	public RectTransform rightPromptHolder;

	public Image leftPromptBackImage;

	public Image rightPromptBackImage;

	public TextMeshProUGUI leftPromptText;

	public TextMeshProUGUI rightPromptText;

	private bool _enabledState;

	private float enableTimer;

	private Vector2 leftPromptStartPosition;

	private Vector2 rightPromptStartPosition;

	private Vector2 leftPromptHidePosition;

	private Vector2 rightPromptHidePosition;
}
