using System;
using Panik;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000A7 RID: 167
public class AimCrossScript : MonoBehaviour
{
	// Token: 0x06000962 RID: 2402 RVA: 0x0000D621 File Offset: 0x0000B821
	public static bool IsEnabled()
	{
		return AimCrossScript.instance.crossHolder.activeSelf;
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x0000D632 File Offset: 0x0000B832
	private void Awake()
	{
		AimCrossScript.instance = this;
		this.myImages = base.GetComponentsInChildren<Image>();
		this.crossHolder.SetActive(false);
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x0000D652 File Offset: 0x0000B852
	private void OnDestroy()
	{
		if (AimCrossScript.instance == this)
		{
			AimCrossScript.instance = null;
		}
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x0004D408 File Offset: 0x0004B608
	private void Update()
	{
		if (!Tick.IsGameRunning)
		{
			return;
		}
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
		bool flag = CameraController.instance != null && CameraController.CanFreeLook() && !CameraController.HasDisabledReasons() && gamePhase != GameplayMaster.GamePhase.intro && !ScreenMenuScript.IsEnabled() && !PowerupTriggerAnimController.HasAnimations();
		if (this.crossHolder.activeSelf != flag)
		{
			this.crossHolder.SetActive(flag);
		}
		bool captureMode = ConsolePrompt.captureMode;
		if (this.shouldHideImagesOld != captureMode)
		{
			this.shouldHideImagesOld = captureMode;
			Image[] array = this.myImages;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = !captureMode;
			}
		}
	}

	// Token: 0x04000954 RID: 2388
	public static AimCrossScript instance;

	// Token: 0x04000955 RID: 2389
	public const bool USE_AS_CURSOR = true;

	// Token: 0x04000956 RID: 2390
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000957 RID: 2391
	public GameObject crossHolder;

	// Token: 0x04000958 RID: 2392
	private Image[] myImages;

	// Token: 0x04000959 RID: 2393
	private bool shouldHideImagesOld;
}
