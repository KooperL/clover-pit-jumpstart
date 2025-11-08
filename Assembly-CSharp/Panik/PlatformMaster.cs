using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Panik
{
	public class PlatformMaster : MonoBehaviour
	{
		// Token: 0x06000D31 RID: 3377 RVA: 0x00054149 File Offset: 0x00052349
		public static bool PlatformResolutionCanChange()
		{
			return PlatformMaster.PlatformIsComputer();
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00054150 File Offset: 0x00052350
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

		// Token: 0x06000D33 RID: 3379 RVA: 0x00054169 File Offset: 0x00052369
		public static Data.SettingsData.WidthAspectRatio[] PlatformSupportedWidthAspectRatios()
		{
			if (PlatformMaster.PlatformIsComputer())
			{
				return new Data.SettingsData.WidthAspectRatio[1];
			}
			return new Data.SettingsData.WidthAspectRatio[] { Data.SettingsData.WidthAspectRatio._16_9 };
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00054184 File Offset: 0x00052384
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

		// Token: 0x06000D35 RID: 3381 RVA: 0x000541E2 File Offset: 0x000523E2
		public static bool PlatformSupports_FullscreenSwitching()
		{
			return PlatformMaster.PlatformIsComputer();
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x000541E9 File Offset: 0x000523E9
		public static PlatformMaster.PlatformKind PlatformKindGet()
		{
			return Master._PlatformKind;
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x000541F0 File Offset: 0x000523F0
		public static bool PlatformIsComputer()
		{
			return PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.PC || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.Linux || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.Mac;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x0005420C File Offset: 0x0005240C
		public static bool PlatformIsMobile()
		{
			return PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.Android || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.iOS;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00054221 File Offset: 0x00052421
		public static bool PlatformIsConsole()
		{
			return PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.NintendoSwitch || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.PS4 || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.XboxOne || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.XboxSeries || PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.PS5;
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00054250 File Offset: 0x00052450
		public static bool PlatformIsWeb()
		{
			return PlatformMaster.PlatformKindGet() == PlatformMaster.PlatformKind.WebGL;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x0005425D File Offset: 0x0005245D
		public static bool IsInitialized()
		{
			return PlatformMaster.initialized;
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00054264 File Offset: 0x00052464
		public static void Initialize()
		{
			PlatformMaster.instance.InstantInitialization();
			PlatformMaster.instance.PlatformInitializationCoroutine();
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x0005427C File Offset: 0x0005247C
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

		// Token: 0x06000D3E RID: 3390 RVA: 0x000542D8 File Offset: 0x000524D8
		private UniTask PlatformInitializationCoroutine()
		{
			PlatformMaster.<PlatformInitializationCoroutine>d__20 <PlatformInitializationCoroutine>d__;
			<PlatformInitializationCoroutine>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
			<PlatformInitializationCoroutine>d__.<>4__this = this;
			<PlatformInitializationCoroutine>d__.<>1__state = -1;
			<PlatformInitializationCoroutine>d__.<>t__builder.Start<PlatformMaster.<PlatformInitializationCoroutine>d__20>(ref <PlatformInitializationCoroutine>d__);
			return <PlatformInitializationCoroutine>d__.<>t__builder.Task;
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x0005431B File Offset: 0x0005251B
		public static bool EscButtonCanCloseTheGame()
		{
			return PlatformMaster.PlatformIsComputer() && Master.instance.ESCAPE_CAN_CLOSE_GAME;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00054333 File Offset: 0x00052533
		public static bool IsFullscreenSupported()
		{
			return PlatformMaster.PlatformIsComputer();
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x0005433A File Offset: 0x0005253A
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

		// Token: 0x06000D42 RID: 3394 RVA: 0x0005436B File Offset: 0x0005256B
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

		// Token: 0x06000D43 RID: 3395 RVA: 0x0005439D File Offset: 0x0005259D
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
