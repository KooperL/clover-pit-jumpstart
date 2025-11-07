using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenNotifications : MonoBehaviour
{
	// Token: 0x060008D3 RID: 2259 RVA: 0x0003A6AC File Offset: 0x000388AC
	public static bool IsEnabled()
	{
		return !(LoadingScreenNotifications.instance == null) && LoadingScreenNotifications.instance.notifHolder.activeSelf;
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x0003A6CC File Offset: 0x000388CC
	public static bool LoadingShouldWait()
	{
		return !(LoadingScreenNotifications.instance == null) && (LoadingScreenNotifications.notifKeysQueue.Count > 0 || LoadingScreenNotifications.IsEnabled());
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x0003A6F1 File Offset: 0x000388F1
	public static bool HasNotifications()
	{
		return !(LoadingScreenNotifications.instance == null) && (LoadingScreenNotifications.notifKeysQueue.Count > 0 || LoadingScreenNotifications.IsEnabled());
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x0003A716 File Offset: 0x00038916
	public static void SetNotification(string translationKey)
	{
		if (LoadingScreenNotifications.instance == null)
		{
			return;
		}
		LoadingScreenNotifications.notifKeysQueue.Add(translationKey);
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x0003A731 File Offset: 0x00038931
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

	// Token: 0x060008D8 RID: 2264 RVA: 0x0003A764 File Offset: 0x00038964
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

	// Token: 0x060008D9 RID: 2265 RVA: 0x0003A7E2 File Offset: 0x000389E2
	private void Awake()
	{
		LoadingScreenNotifications.instance = this;
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x0003A7EA File Offset: 0x000389EA
	private void Start()
	{
		this.notifHolder.SetActive(false);
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x0003A7F8 File Offset: 0x000389F8
	private void OnDestroy()
	{
		if (LoadingScreenNotifications.instance == this)
		{
			LoadingScreenNotifications.instance = null;
		}
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x0003A810 File Offset: 0x00038A10
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
