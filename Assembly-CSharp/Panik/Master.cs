using System;
using UnityEngine;

namespace Panik
{
	public class Master : MonoBehaviour
	{
		// (get) Token: 0x06000CD8 RID: 3288
		public static bool IsDebugBuild
		{
			get
			{
				return Master.instance._isDebugBuild;
			}
		}

		// (get) Token: 0x06000CD9 RID: 3289
		public static bool IsPlaytestBuild
		{
			get
			{
				return false;
			}
		}

		// (get) Token: 0x06000CDA RID: 3290
		public static bool IsDemo
		{
			get
			{
				return false;
			}
		}

		// (get) Token: 0x06000CDB RID: 3291
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

		// (get) Token: 0x06000CDC RID: 3292
		public static PlatformAPI.ApiKind _ApiKind
		{
			get
			{
				return PlatformAPI.ApiKind.Steam;
			}
		}

		public static bool IsModded()
		{
			return true;
		}

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
			QualitySettings.vSyncCount = ((this.VSYNC_DEFAULT > false) ? 1 : 0);
			this.audioHolderTr = new GameObject().transform;
			this.audioHolderTr.gameObject.name = "audio holder";
			this.audioHolderTr.SetParent(base.transform);
		}

		private void Start()
		{
			PlatformMaster.Initialize();
		}

		private void OnQuit()
		{
			if (Master.instance != this)
			{
				return;
			}
			PlatformDataMaster.EndGameDataIsDoneCheck();
		}

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
