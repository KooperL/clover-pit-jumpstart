using System;
using System.Reflection;
using UnityEngine;

namespace Panik
{
	// Token: 0x02000149 RID: 329
	public class Master : MonoBehaviour
	{
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06001021 RID: 4129 RVA: 0x00013573 File Offset: 0x00011773
		public static bool IsDebugBuild
		{
			get
			{
				return Master.instance._isDebugBuild;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06001022 RID: 4130 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public static bool IsPlaytestBuild
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0001357F File Offset: 0x0001177F
		public bool MobileAdEnabled()
		{
			return this.AD_OVERRIDE_MOBILE && PlatformMaster.PlatformIsComputer();
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06001024 RID: 4132 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public static bool IsDemo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06001025 RID: 4133 RVA: 0x00013590 File Offset: 0x00011790
		public static PlatformMaster.PlatformKind _PlatformKind
		{
			get
			{
				if (Master.instance == null)
				{
					Debug.LogError("Master: instance is null! Cannot get the platform kind!");
					return PlatformMaster.PlatformKind.Undefined;
				}
				return PlatformMaster.PlatformKind.PC;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06001026 RID: 4134 RVA: 0x00007C86 File Offset: 0x00005E86
		public static PlatformAPI.ApiKind _ApiKind
		{
			get
			{
				return PlatformAPI.ApiKind.Steam;
			}
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x0006F82C File Offset: 0x0006DA2C
		public static bool IsModded()
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				string fullName = assemblies[i].FullName;
				if (fullName.StartsWith("MonoMod") || fullName.StartsWith("0Harmony") || fullName.StartsWith("Harmony") || fullName.StartsWith("BepInEx") || fullName.StartsWith("MelonLoader"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x0006F8A0 File Offset: 0x0006DAA0
		private void Awake()
		{
			if (Master.instance != null)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			Master.instance = this;
			Application.quitting += this.OnQuit;
			if (this.RENDER_TO_TEXTURE && this.SPLIT_SCREEN_ALLOW)
			{
				Debug.LogError("Master: You cannot have both RENDER_TO_TEXTURE and SPLIT_SCREEN_ALLOW enabled at the same time. Please disable one of them.");
			}
			if (!Application.isEditor)
			{
				this.EDITOR_QUICK_START = false;
			}
			Application.targetFrameRate = this.HIGHEST_FRAME_RATE;
			QualitySettings.vSyncCount = (this.VSYNC_DEFAULT ? 1 : 0);
			this.audioHolderTr = new GameObject().transform;
			this.audioHolderTr.gameObject.name = "audio holder";
			this.audioHolderTr.SetParent(base.transform);
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x000135AD File Offset: 0x000117AD
		private void Start()
		{
			PlatformMaster.Initialize();
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x000135B4 File Offset: 0x000117B4
		private void OnQuit()
		{
			if (Master.instance != this)
			{
				return;
			}
			PlatformDataMaster.EndGameDataIsDoneCheck();
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x000135CA File Offset: 0x000117CA
		private void Update()
		{
			Tick.Routine();
			Sound.Routine();
			Music.Routine();
		}

		// Token: 0x040010C1 RID: 4289
		public static Master instance;

		// Token: 0x040010C2 RID: 4290
		public const bool IS_DEBUG_BUILD = false;

		// Token: 0x040010C3 RID: 4291
		public bool _debugInfo = true;

		// Token: 0x040010C4 RID: 4292
		private bool _isDebugBuild;

		// Token: 0x040010C5 RID: 4293
		public const bool IS_PLAYTEST_BUIlD = false;

		// Token: 0x040010C6 RID: 4294
		public bool SPLIT_SCREEN_ALLOW = true;

		// Token: 0x040010C7 RID: 4295
		public bool RENDER_TO_TEXTURE = true;

		// Token: 0x040010C8 RID: 4296
		public bool REND_AUTO_UPDATE_CAMERAS = true;

		// Token: 0x040010C9 RID: 4297
		public int HIGHEST_FRAME_RATE = 360;

		// Token: 0x040010CA RID: 4298
		public bool VSYNC_DEFAULT = true;

		// Token: 0x040010CB RID: 4299
		public bool EDITOR_QUICK_START = true;

		// Token: 0x040010CC RID: 4300
		public bool EDITOR_SAVE_READABLE_DATA = true;

		// Token: 0x040010CD RID: 4301
		public bool ESCAPE_CAN_CLOSE_GAME = true;

		// Token: 0x040010CE RID: 4302
		public bool LOG_MASTER_SETUP_INFOS;

		// Token: 0x040010CF RID: 4303
		public bool SHOW_CURSOR_ON_START = true;

		// Token: 0x040010D0 RID: 4304
		public bool SHOW_AUTOSAVE_WARNING_STARTUP = true;

		// Token: 0x040010D1 RID: 4305
		public bool SHOW_SAVE_MEMO_WARNING;

		// Token: 0x040010D2 RID: 4306
		public bool POSTER_TEXT_SCALES_WITH_VALUES = true;

		// Token: 0x040010D3 RID: 4307
		public bool SCORE_PATTERNS_INSIDE_JACKPOT = true;

		// Token: 0x040010D4 RID: 4308
		public bool DEMOORPLAYTEST_HAS_DEVELOPER_LETTER = true;

		// Token: 0x040010D5 RID: 4309
		public bool ENGLISH_ONLY_BUILD;

		// Token: 0x040010D6 RID: 4310
		public Texture2D gameIcon_FullVersion;

		// Token: 0x040010D7 RID: 4311
		public Texture2D gameIcon_DemoVersion;

		// Token: 0x040010D8 RID: 4312
		public Master.GamePublicState GAME_PUBLIC_STATE;

		// Token: 0x040010D9 RID: 4313
		public bool AD_OVERRIDE_MOBILE = true;

		// Token: 0x040010DA RID: 4314
		public const bool IS_DEMO = false;

		// Token: 0x040010DB RID: 4315
		public bool _demoInfo = true;

		// Token: 0x040010DC RID: 4316
		public const PlatformMaster.PlatformKind PLATFORM_KIND = PlatformMaster.PlatformKind.PC;

		// Token: 0x040010DD RID: 4317
		public bool _platformKindInfo = true;

		// Token: 0x040010DE RID: 4318
		public bool _apiKindInfo = true;

		// Token: 0x040010DF RID: 4319
		[NonSerialized]
		public Transform audioHolderTr;

		// Token: 0x040010E0 RID: 4320
		public const float IMG_VB = 1f;

		// Token: 0x0200014A RID: 330
		public enum GamePublicState
		{
			// Token: 0x040010E2 RID: 4322
			wishlistOrPrerelease,
			// Token: 0x040010E3 RID: 4323
			released
		}
	}
}
