using System;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class AimCrossScript : MonoBehaviour
{
	// Token: 0x0600083F RID: 2111 RVA: 0x00035E3C File Offset: 0x0003403C
	public static bool IsEnabled()
	{
		return AimCrossScript.instance.crossHolder.activeSelf;
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x00035E4D File Offset: 0x0003404D
	private void Awake()
	{
		AimCrossScript.instance = this;
		this.myImages = base.GetComponentsInChildren<Image>();
		this.crossHolder.SetActive(false);
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x00035E6D File Offset: 0x0003406D
	private void OnDestroy()
	{
		if (AimCrossScript.instance == this)
		{
			AimCrossScript.instance = null;
		}
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x00035E84 File Offset: 0x00034084
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
