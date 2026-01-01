using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000BC RID: 188
public class LoadingScreenNotifications : MonoBehaviour
{
	// Token: 0x06000A19 RID: 2585 RVA: 0x0000E036 File Offset: 0x0000C236
	public static bool IsEnabled()
	{
		return !(LoadingScreenNotifications.instance == null) && LoadingScreenNotifications.instance.notifHolder.activeSelf;
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x0000E056 File Offset: 0x0000C256
	public static bool LoadingShouldWait()
	{
		return !(LoadingScreenNotifications.instance == null) && (LoadingScreenNotifications.notifKeysQueue.Count > 0 || LoadingScreenNotifications.IsEnabled());
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x0000E056 File Offset: 0x0000C256
	public static bool HasNotifications()
	{
		return !(LoadingScreenNotifications.instance == null) && (LoadingScreenNotifications.notifKeysQueue.Count > 0 || LoadingScreenNotifications.IsEnabled());
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x0000E07B File Offset: 0x0000C27B
	public static void SetNotification(string translationKey)
	{
		if (LoadingScreenNotifications.instance == null)
		{
			return;
		}
		LoadingScreenNotifications.notifKeysQueue.Add(translationKey);
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0000E096 File Offset: 0x0000C296
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

	// Token: 0x06000A1E RID: 2590 RVA: 0x00051DA8 File Offset: 0x0004FFA8
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

	// Token: 0x06000A1F RID: 2591 RVA: 0x0000E0C7 File Offset: 0x0000C2C7
	private void Awake()
	{
		LoadingScreenNotifications.instance = this;
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x0000E0CF File Offset: 0x0000C2CF
	private void Start()
	{
		this.notifHolder.SetActive(false);
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x0000E0DD File Offset: 0x0000C2DD
	private void OnDestroy()
	{
		if (LoadingScreenNotifications.instance == this)
		{
			LoadingScreenNotifications.instance = null;
		}
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x00051E28 File Offset: 0x00050028
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

	// Token: 0x04000A4A RID: 2634
	public static LoadingScreenNotifications instance;

	// Token: 0x04000A4B RID: 2635
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000A4C RID: 2636
	private const float NOTIFICATIONS_TIMER = 5f;

	// Token: 0x04000A4D RID: 2637
	private const float NOTIFICATIONS_DELAY = 0.5f;

	// Token: 0x04000A4E RID: 2638
	public GameObject notifHolder;

	// Token: 0x04000A4F RID: 2639
	public TextMeshProUGUI notifText;

	// Token: 0x04000A50 RID: 2640
	private float inputDelay;

	// Token: 0x04000A51 RID: 2641
	private float maxNotificationTimer = 5f;

	// Token: 0x04000A52 RID: 2642
	private static List<string> notifKeysQueue = new List<string>();
}
