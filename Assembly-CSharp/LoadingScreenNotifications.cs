using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenNotifications : MonoBehaviour
{
	// Token: 0x060008E2 RID: 2274 RVA: 0x0003A9C8 File Offset: 0x00038BC8
	public static bool IsEnabled()
	{
		return !(LoadingScreenNotifications.instance == null) && LoadingScreenNotifications.instance.notifHolder.activeSelf;
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x0003A9E8 File Offset: 0x00038BE8
	public static bool LoadingShouldWait()
	{
		return !(LoadingScreenNotifications.instance == null) && (LoadingScreenNotifications.notifKeysQueue.Count > 0 || LoadingScreenNotifications.IsEnabled());
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x0003AA0D File Offset: 0x00038C0D
	public static bool HasNotifications()
	{
		return !(LoadingScreenNotifications.instance == null) && (LoadingScreenNotifications.notifKeysQueue.Count > 0 || LoadingScreenNotifications.IsEnabled());
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x0003AA32 File Offset: 0x00038C32
	public static void SetNotification(string translationKey)
	{
		if (LoadingScreenNotifications.instance == null)
		{
			return;
		}
		LoadingScreenNotifications.notifKeysQueue.Add(translationKey);
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x0003AA4D File Offset: 0x00038C4D
	public static void ClearNotifications()
	{
		if (LoadingScreenNotifications.instance == null)
		{
			return;
		}
		LoadingScreenNotifications.notifKeysQueue.Clear();
		if (LoadingScreenNotifications.IsEnabled())
		{
			LoadingScreenNotifications.instance.notifHolder.SetActive(false);
		}
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x0003AA80 File Offset: 0x00038C80
	private void NotificationShow()
	{
		this.notifHolder.SetActive(true);
		this.notifText.text = Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get(LoadingScreenNotifications.notifKeysQueue[0]), Strings.SanitizationSubKind.none);
		this.notifText.ForceMeshUpdate(false, false);
		LoadingScreenNotifications.notifKeysQueue.RemoveAt(0);
		this.inputDelay = 0.5f;
		this.maxNotificationTimer = 5f;
		Sound.Play("SoundMenuPopUp", 1f, 1f);
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x0003AAFE File Offset: 0x00038CFE
	private void Awake()
	{
		LoadingScreenNotifications.instance = this;
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x0003AB06 File Offset: 0x00038D06
	private void Start()
	{
		this.notifHolder.SetActive(false);
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x0003AB14 File Offset: 0x00038D14
	private void OnDestroy()
	{
		if (LoadingScreenNotifications.instance == this)
		{
			LoadingScreenNotifications.instance = null;
		}
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x0003AB2C File Offset: 0x00038D2C
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		int num = -1;
		if (num < 0)
		{
			num = SceneManager.GetActiveScene().buildIndex;
		}
		if (num != 0)
		{
			return;
		}
		if (!LoadingScreenNotifications.IsEnabled())
		{
			if (LoadingScreenNotifications.notifKeysQueue.Count > 0)
			{
				this.NotificationShow();
				return;
			}
		}
		else
		{
			this.inputDelay -= Tick.Time;
			this.maxNotificationTimer -= Tick.Time;
			if (this.inputDelay <= 0f && (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) || Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true) || this.maxNotificationTimer <= 0f))
			{
				this.notifHolder.SetActive(false);
			}
		}
	}

	public static LoadingScreenNotifications instance;

	private const int PLAYER_INDEX = 0;

	private const float NOTIFICATIONS_TIMER = 5f;

	private const float NOTIFICATIONS_DELAY = 0.5f;

	public GameObject notifHolder;

	public TextMeshProUGUI notifText;

	private float inputDelay;

	private float maxNotificationTimer = 5f;

	private static List<string> notifKeysQueue = new List<string>();
}
