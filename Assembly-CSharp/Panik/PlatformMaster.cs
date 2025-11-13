using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Panik
{
	public class PlatformMaster : MonoBehaviour
	{
		// Token: 0x06000D48 RID: 3400 RVA: 0x00054925 File Offset: 0x00052B25
		public static bool PlatformResolutionCanChange()
		{
			return PlatformMaster.PlatformIsComputer();
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0005492C File Offset: 0x00052B2C
		public static Data.SettingsData.VerticalResolution[] PlatformSupportedVerticalResolutions()
		{
			PlatformMaster.PlatformIsComputer();
			return new Data.SettingsData.VerticalResolution[]
			{
				Data.SettingsData.VerticalResolution._native,
				Data.SettingsData.VerticalResolution._360p,
				Data.SettingsData.VerticalResolution._480p,
				Data.SettingsData.VerticalResolution._720p
			};
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00054945 File Offset: 0x00052B45
		public static Data.SettingsData.WidthAspectRatio[] PlatformSupportedWidthAspectRatios()
		{
			if (PlatformMaster.PlatformIsComputer())
			{
				return new Data.SettingsData.WidthAspectRatio[1];
			}
			return new Data.SettingsData.WidthAspectRatio[] { Data.SettingsData.WidthAspectRatio._16_9 };
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00054960 File Offset: 0x00052B60
		public static int QualityDefaultGet()
		{
			switch (PlatformMaster.PlatformKindGet())
			{
			case PlatformMaster.PlatformKind.PC:
				return 2;
			case PlatformMaster.PlatformKind.Linux:
				return 2;
			case PlatformMaster.PlatformKind.Mac:
				return 2;
			case PlatformMaster.PlatformKind.WebGL:
				return 1;
			case PlatformMaster.PlatformKind.Android:
				return 1;
			case PlatformMaster.PlatformKind.iOS:
				return 1;
			case PlatformMaster.PlatformKind.PS4:
				return 2;
			case PlatformMaster.PlatformKind.PS5:
				return 2;
			case PlatformMaster.PlatformKind.XboxOne:
				return 2;
			case PlatformMaster.PlatformKind.XboxSeries:
				return 2;
			case PlatformMaster.PlatformKind.NintendoSwitch:
				return 0;
			default:
				return 1;
			}
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x000549BE File Offset: 0x00052BBE
		public static bool PlatformSupports_FullscreenSwitching()
		{
			return PlatformMaster.PlatformIsComputer();
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x000549C5 File Offset: 0x00052BC5
		public static PlatformMaster.PlatformKind PlatformKindGet()
		{
			return Master._PlatformKind;
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x000549CC File Offset: 0x00052BCC
		public static bool PlatformIsComputer()
		{
			return PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.PC || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.Linux || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.Mac;
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x000549E8 File Offset: 0x00052BE8
		public static bool PlatformIsMobile()
		{
			return PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.Android || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.iOS;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x000549FD File Offset: 0x00052BFD
		public static bool PlatformIsConsole()
		{
			return PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.NintendoSwitch || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.PS4 || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.XboxOne || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.XboxSeries || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.PS5;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00054A2C File Offset: 0x00052C2C
		public static bool PlatformIsWeb()
		{
			return PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.WebGL;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00054A39 File Offset: 0x00052C39
		public static bool IsInitialized()
		{
			return PlatformMaster.initialized;
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00054A40 File Offset: 0x00052C40
		public static void Initialize()
		{
			PlatformMaster.instance.InstantInitialization();
			PlatformMaster.instance.PlatformInitializationCoroutine();
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00054A58 File Offset: 0x00052C58
		private void InstantInitialization()
		{
			if (PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.Undefined)
			{
				Debug.LogError("Platform is undefined! Please set the platform in the Master.cs inspector!");
				return;
			}
			Debug.Log("Platfom: " + PlatformMaster.PlatformKindGet().ToString());
			UnityAction unityAction = this.onInitializationBegin;
			if (unityAction != null)
			{
				unityAction();
			}
			SceneMaster.Initialize();
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00054AB4 File Offset: 0x00052CB4
		private UniTask PlatformInitializationCoroutine()
		{
			PlatformMaster.<PlatformInitializationCoroutine>d__20 <PlatformInitializationCoroutine>d__;
			<PlatformInitializationCoroutine>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<PlatformInitializationCoroutine>d__.<>4__this = this;
			<PlatformInitializationCoroutine>d__.<>1__state = -1;
			<PlatformInitializationCoroutine>d__.<>t__builder.Start<PlatformMaster.<PlatformInitializationCoroutine>d__20>(ref <PlatformInitializationCoroutine>d__);
			return <PlatformInitializationCoroutine>d__.<>t__builder.Task;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00054AF7 File Offset: 0x00052CF7
		public static bool EscButtonCanCloseTheGame()
		{
			return PlatformMaster.PlatformIsComputer() && Master.instance.ESCAPE_CAN_CLOSE_GAME;
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00054B0F File Offset: 0x00052D0F
		public static bool IsFullscreenSupported()
		{
			return PlatformMaster.PlatformIsComputer();
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00054B16 File Offset: 0x00052D16
		private void Awake()
		{
			if (PlatformMaster.instance != null)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			PlatformMaster.instance = this;
			UnityAction unityAction = this.onAwake;
			if (unityAction == null)
			{
				return;
			}
			unityAction();
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x00054B47 File Offset: 0x00052D47
		private void OnDestroy()
		{
			if (PlatformMaster.instance == this)
			{
				PlatformAPI platformAPI = PlatformAPI.instance;
				if (platformAPI != null)
				{
					platformAPI._OnClose();
				}
			}
			if (PlatformMaster.instance == this)
			{
				PlatformMaster.instance = null;
			}
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00054B79 File Offset: 0x00052D79
		private void Update()
		{
			UnityAction unityAction = this.onUpdate;
			if (unityAction != null)
			{
				unityAction();
			}
			PlatformAPI platformAPI = PlatformAPI.instance;
			if (platformAPI == null)
			{
				return;
			}
			platformAPI._Update();
		}

		public static PlatformMaster instance;

		public UnityAction onAwake;

		public UnityAction onInitializationBegin;

		public UnityAction onInitializationEnd;

		public UnityAction onUpdate;

		private static bool initialized;

		public enum PlatformKind
		{
			PC,
			Linux,
			Mac,
			WebGL,
			Android,
			iOS,
			PS4,
			PS5,
			XboxOne,
			XboxSeries,
			NintendoSwitch,
			Count,
			Undefined
		}
	}
}
