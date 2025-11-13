using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotSideUIScript : MonoBehaviour
{
	// Token: 0x06000A2A RID: 2602 RVA: 0x00045BD0 File Offset: 0x00043DD0
	public static bool IsEnabled()
	{
		return !(SlotSideUIScript.instance == null) && SlotSideUIScript.instance._enabledState;
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x00045BEB File Offset: 0x00043DEB
	private static void SetEnableTime(float time)
	{
		if (SlotSideUIScript.instance == null)
		{
			return;
		}
		SlotSideUIScript.instance.enableTimer = Mathf.Max(SlotSideUIScript.instance.enableTimer, time);
	}

	// Token: 0x06000A2C RID: 2604 RVA: 0x00045C15 File Offset: 0x00043E15
	public static void ShowTry()
	{
		SlotSideUIScript.SetEnableTime(3f);
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x00045C24 File Offset: 0x00043E24
	private void Awake()
	{
		SlotSideUIScript.instance = this;
		this.leftPromptStartPosition = this.leftPromptHolder.anchoredPosition;
		this.rightPromptStartPosition = this.rightPromptHolder.anchoredPosition;
		this.leftPromptHidePosition = this.leftPromptStartPosition + new Vector2(-100f, 0f);
		this.rightPromptHidePosition = this.rightPromptStartPosition + new Vector2(100f, 0f);
	}

	// Token: 0x06000A2E RID: 2606 RVA: 0x00045C99 File Offset: 0x00043E99
	private void Start()
	{
		this.leftPromptHolder.anchoredPosition = this.leftPromptHidePosition;
		this.rightPromptHolder.anchoredPosition = this.rightPromptHidePosition;
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x00045CBD File Offset: 0x00043EBD
	private void OnDestroy()
	{
		if (SlotSideUIScript.instance == this)
		{
			SlotSideUIScript.instance = null;
		}
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x00045CD4 File Offset: 0x00043ED4
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
		Vector2 zero = new Vector2(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f));
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
