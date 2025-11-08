using System;
using UnityEngine;

namespace Panik
{
	public class Master : MonoBehaviour
	{
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x00052F5F File Offset: 0x0005115F
		public static bool IsDebugBuild
		{
			get
			{
				return Master.instance._isDebugBuild;
			}
		}

		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x00052F6B File Offset: 0x0005116B
		public static bool IsPlaytestBuild
		{
			get
			{
				return false;
			}
		}

		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x00052F6E File Offset: 0x0005116E
		public static bool IsDemo
		{
			get
			{
				return false;
			}
		}

		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x00052F71 File Offset: 0x00051171
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

		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x00052F8E File Offset: 0x0005118E
		public static PlatformAPI.ApiKind _ApiKind
		{
			get
			{
				return PlatformAPI.ApiKind.Steam;
			}
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00052F94 File Offset: 0x00051194
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

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0005304B File Offset: 0x0005124B
		private void Start()
		{
			PlatformMaster.Initialize();
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x00053052 File Offset: 0x00051252
		private void OnQuit()
		{
			if (Master.instance != this)
			{
				return;
			}
			PlatformDataMaster.EndGameDataIsDoneCheck();
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00053068 File Offset: 0x00051268
		private void Update()
		{
			Tick.Routine();
			Sound.Routine();
			Music.Routine();
		}

		public static Master instance;

		public const bool IS_DEBUG_BUILD = false;

		public bool _debugInfo = true;

		private bool _isDebugBuild;

		public const bool IS_PLAYTEST_BUIlD = false;

		public bool SPLIT_SCREEN_ALLOW = true;

		public bool RENDER_TO_TEXTURE = true;

		public bool REND_AUTO_UPDATE_CAMERAS = true;

		public int HIGHEST_FRAME_RATE = 360;

		public bool VSYNC_DEFAULT = true;

		public bool EDITOR_QUICK_START = true;

		public bool EDITOR_SAVE_READABLE_DATA = true;

		public bool ESCAPE_CAN_CLOSE_GAME = true;

		public bool LOG_MASTER_SETUP_INFOS;

		public bool SHOW_CURSOR_ON_START = true;

		public bool SHOW_AUTOSAVE_WARNING_STARTUP = true;

		public bool SHOW_SAVE_MEMO_WARNING;

		public bool POSTER_TEXT_SCALES_WITH_VALUES = true;

		public bool SCORE_PATTERNS_INSIDE_JACKPOT = true;

		public bool DEMOORPLAYTEST_HAS_DEVELOPER_LETTER = true;

		public bool ENGLISH_ONLY_BUILD;

		public Texture2D gameIcon_FullVersion;

		public Texture2D gameIcon_DemoVersion;

		public Master.GamePublicState GAME_PUBLIC_STATE;

		public const bool IS_DEMO = false;

		public bool _demoInfo = true;

		public const PlatformMaster.PlatformKind PLATFORM_KIND = PlatformMaster.PlatformKind.PC;

		public bool _platformKindInfo = true;

		public bool _apiKindInfo = true;

		[NonSerialized]
		public Transform audioHolderTr;

		public const float IMG_VB = 1f;

		public enum GamePublicState
		{
			wishlistOrPrerelease,
			released
		}
	}
}
