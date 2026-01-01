using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000ED RID: 237
public class SlotSideUIScript : MonoBehaviour
{
	// Token: 0x06000BD6 RID: 3030 RVA: 0x0000FBD8 File Offset: 0x0000DDD8
	public static bool IsEnabled()
	{
		return !(SlotSideUIScript.instance == null) && SlotSideUIScript.instance._enabledState;
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x0000FBF3 File Offset: 0x0000DDF3
	private static void SetEnableTime(float time)
	{
		if (SlotSideUIScript.instance == null)
		{
			return;
		}
		SlotSideUIScript.instance.enableTimer = Mathf.Max(SlotSideUIScript.instance.enableTimer, time);
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x0000FC1D File Offset: 0x0000DE1D
	public static void ShowTry()
	{
		SlotSideUIScript.SetEnableTime(3f);
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x0005F2A4 File Offset: 0x0005D4A4
	private void Awake()
	{
		SlotSideUIScript.instance = this;
		this.leftPromptStartPosition = this.leftPromptHolder.anchoredPosition;
		this.rightPromptStartPosition = this.rightPromptHolder.anchoredPosition;
		this.leftPromptHidePosition = this.leftPromptStartPosition + new Vector2(-100f, 0f);
		this.rightPromptHidePosition = this.rightPromptStartPosition + new Vector2(100f, 0f);
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x0000FC29 File Offset: 0x0000DE29
	private void Start()
	{
		this.leftPromptHolder.anchoredPosition = this.leftPromptHidePosition;
		this.rightPromptHolder.anchoredPosition = this.rightPromptHidePosition;
	}

	// Token: 0x06000BDB RID: 3035 RVA: 0x0000FC4D File Offset: 0x0000DE4D
	private void OnDestroy()
	{
		if (SlotSideUIScript.instance == this)
		{
			SlotSideUIScript.instance = null;
		}
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x0005F31C File Offset: 0x0005D51C
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

	// Token: 0x04000C86 RID: 3206
	public static SlotSideUIScript instance;

	// Token: 0x04000C87 RID: 3207
	public const int PLAYER_INDEX = 0;

	// Token: 0x04000C88 RID: 3208
	private const float ENABLE_TIME_DEFAULT = 3f;

	// Token: 0x04000C89 RID: 3209
	public RectTransform leftPromptHolder;

	// Token: 0x04000C8A RID: 3210
	public RectTransform rightPromptHolder;

	// Token: 0x04000C8B RID: 3211
	public Image leftPromptBackImage;

	// Token: 0x04000C8C RID: 3212
	public Image rightPromptBackImage;

	// Token: 0x04000C8D RID: 3213
	public TextMeshProUGUI leftPromptText;

	// Token: 0x04000C8E RID: 3214
	public TextMeshProUGUI rightPromptText;

	// Token: 0x04000C8F RID: 3215
	private bool _enabledState;

	// Token: 0x04000C90 RID: 3216
	private float enableTimer;

	// Token: 0x04000C91 RID: 3217
	private Vector2 leftPromptStartPosition;

	// Token: 0x04000C92 RID: 3218
	private Vector2 rightPromptStartPosition;

	// Token: 0x04000C93 RID: 3219
	private Vector2 leftPromptHidePosition;

	// Token: 0x04000C94 RID: 3220
	private Vector2 rightPromptHidePosition;
}
