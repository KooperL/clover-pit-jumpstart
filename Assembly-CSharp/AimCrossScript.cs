using System;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class AimCrossScript : MonoBehaviour
{
	// Token: 0x06000838 RID: 2104 RVA: 0x00035C54 File Offset: 0x00033E54
	public static bool IsEnabled()
	{
		return AimCrossScript.instance.crossHolder.activeSelf;
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x00035C65 File Offset: 0x00033E65
	private void Awake()
	{
		AimCrossScript.instance = this;
		this.myImages = base.GetComponentsInChildren<Image>();
		this.crossHolder.SetActive(false);
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x00035C85 File Offset: 0x00033E85
	private void OnDestroy()
	{
		if (AimCrossScript.instance == this)
		{
			AimCrossScript.instance = null;
		}
	}

	// Token: 0x0600083B RID: 2107 RVA: 0x00035C9C File Offset: 0x00033E9C
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

	public static AimCrossScript instance;

	public const bool USE_AS_CURSOR = true;

	private const int PLAYER_INDEX = 0;

	public GameObject crossHolder;

	private Image[] myImages;

	private bool shouldHideImagesOld;
}
