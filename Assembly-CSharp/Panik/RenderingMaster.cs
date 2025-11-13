using System;
using UnityEngine;
using UnityEngine.UI;

namespace Panik
{
	public class RenderingMaster : MonoBehaviour
	{
		// Token: 0x06000D5C RID: 3420 RVA: 0x00054BA3 File Offset: 0x00052DA3
		private void ReferencesRefresh()
		{
			if (this.myCanvas == null)
			{
				this.myCanvas = base.GetComponent<Canvas>();
			}
			if (this.myCanvasScaler == null)
			{
				this.myCanvasScaler = base.GetComponent<CanvasScaler>();
			}
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00054BDC File Offset: 0x00052DDC
		public static Vector2 GetRawImageSize()
		{
			if (RenderingMaster.instance == null)
			{
				return new Vector2((float)Screen.width, (float)Screen.height);
			}
			return new Vector2(RenderingMaster.instance.renderingRawImage.rectTransform.sizeDelta.x, RenderingMaster.instance.renderingRawImage.rectTransform.sizeDelta.y);
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00054C40 File Offset: 0x00052E40
		public static bool CanSplitScreen(ref string warningString)
		{
			bool flag = true;
			if (!Master.instance.SPLIT_SCREEN_ALLOW)
			{
				flag = false;
				if (warningString != null)
				{
					warningString = "Split screen is not allowed globally.";
				}
			}
			else if (Master.instance.RENDER_TO_TEXTURE)
			{
				flag = false;
				if (warningString != null)
				{
					warningString = "Split screen is not allowed when rendering to texture.";
				}
			}
			else if (SceneMaster.instance == null || !SceneMaster.instance.canSplitScreen)
			{
				flag = false;
				if (warningString != null)
				{
					warningString = "Split screen is not allowed in this scene.";
				}
			}
			return flag;
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00054CAC File Offset: 0x00052EAC
		public static void SplitScreenUpdate()
		{
			string text = "";
			if (RenderingMaster.CanSplitScreen(ref text))
			{
				foreach (CameraGame cameraGame in CameraGame.list)
				{
					if (cameraGame != null)
					{
						cameraGame._SplitScreenUpdate();
					}
				}
				foreach (CameraUi cameraUi in CameraUi.list)
				{
					if (cameraUi != null)
					{
						cameraUi._SplitScreenUpdate();
					}
				}
			}
			if (!string.IsNullOrEmpty(text) && !RenderingMaster._splitScreenWarningLogged)
			{
				RenderingMaster._splitScreenWarningLogged = true;
				Debug.LogWarning("RenderingMaster Split Screen: " + text);
			}
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00054D88 File Offset: 0x00052F88
		public static void RenderingRefresh(bool applyScreenRes)
		{
			RenderingMaster.instance._RenderingRefresh(applyScreenRes);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00054D98 File Offset: 0x00052F98
		private void _RenderingRefresh(bool applyScreenRes)
		{
			if (!Master.instance.RENDER_TO_TEXTURE)
			{
				if (!PlatformMaster.PlatformResolutionCanChange())
				{
					Debug.LogWarning("RenderingMaster: Resolution cannot be changed on this platform.");
				}
				else if (applyScreenRes)
				{
					int num = Data.settings.ResolutionDesiredWidthGet();
					int num2 = Data.settings.ResolutionDesiredHeightGet();
					float systemWidth = (float)Display.main.systemWidth;
					int systemHeight = Display.main.systemHeight;
					float num3 = systemWidth / (float)systemHeight;
					float num4 = (float)num / (float)num2;
					if (num4 > num3)
					{
						num = Mathf.RoundToInt((float)num2 * num3);
						num2 = Mathf.RoundToInt((float)num / num4);
					}
					if (applyScreenRes || !RenderingMaster.firstBootUpdated)
					{
						float num5 = (Data.settings.fullscreenEnabled ? 1f : 0.5f);
						Screen.SetResolution((int)((float)num * num5), (int)((float)num2 * num5), Data.settings.fullscreenEnabled);
					}
				}
			}
			else
			{
				int num6 = Data.settings.ResolutionDesiredWidthGet();
				int num7 = Data.settings.ResolutionDesiredHeightGet();
				int systemWidth2 = Display.main.systemWidth;
				int systemHeight2 = Display.main.systemHeight;
				bool flag = RenderingMaster.renderTextureCurrent.width != num6 || RenderingMaster.renderTextureCurrent.height != num7;
				if (flag)
				{
					RenderTexture renderTexture = new RenderTexture(num6, num7, 24);
					renderTexture.filterMode = FilterMode.Point;
					renderTexture.useMipMap = false;
					renderTexture.autoGenerateMips = false;
					renderTexture.anisoLevel = 0;
					renderTexture.wrapMode = TextureWrapMode.Clamp;
					renderTexture.Create();
					RenderingMaster.renderTextureCurrent.Release();
					if (RenderingMaster.renderTextureCurrent != this.renderTextureInitial && RenderingMaster.renderTextureCurrent != null)
					{
						global::UnityEngine.Object.Destroy(RenderingMaster.renderTextureCurrent);
					}
					RenderingMaster.renderTextureCurrent = renderTexture;
					this.renderingRawImage.texture = RenderingMaster.renderTextureCurrent;
				}
				bool flag2 = applyScreenRes || !RenderingMaster.firstBootUpdated;
				if (flag2)
				{
					if (PlatformMaster.PlatformResolutionCanChange())
					{
						if (Data.settings.fullscreenEnabled)
						{
							Screen.SetResolution(systemWidth2, systemHeight2, applyScreenRes ? Data.settings.fullscreenEnabled : Screen.fullScreen);
						}
						else
						{
							Screen.fullScreenMode = FullScreenMode.Windowed;
						}
					}
					else
					{
						Debug.Log("RenderingMaster: While resolution cannot be changed on this platform, we are scaling the render texture and its raw image up to the max resolution available!");
					}
				}
				if (flag || flag2)
				{
					float num8 = 0.005f;
					float num9 = 0.005f;
					while ((float)num6 * (num8 + num9) < (float)Screen.width && (float)num7 * (num8 + num9) < (float)Screen.height)
					{
						num8 += num9;
					}
					this.renderingRawImage.rectTransform.sizeDelta = new Vector2((float)num6, (float)num7);
					this.renderingRawImage.rectTransform.localScale = new Vector3(num8, num8, 1f);
				}
			}
			if (!Master.instance.RENDER_TO_TEXTURE)
			{
				this.renderingBackgroundImage.enabled = false;
				this.renderingRawImage.enabled = false;
			}
			else
			{
				this.renderingBackgroundImage.enabled = true;
				this.renderingRawImage.enabled = true;
			}
			RenderingMaster.SplitScreenUpdate();
			if (Master.instance.RENDER_TO_TEXTURE || Data.settings.tateMode == Data.SettingsData.TateMode.none)
			{
				switch (Data.settings.tateMode)
				{
				case Data.SettingsData.TateMode.none:
					this.tateModeTransform.localEulerAngles = Vector3.zero;
					this.tateModeTransform.localScale = Vector3.one;
					break;
				case Data.SettingsData.TateMode.horizontalLeft:
				{
					this.tateModeTransform.localEulerAngles = new Vector3(0f, 0f, 90f);
					float num10 = this.renderingRawImage.rectTransform.sizeDelta.y / this.renderingRawImage.rectTransform.sizeDelta.x;
					this.tateModeTransform.localScale = new Vector3(num10, num10, 1f);
					break;
				}
				case Data.SettingsData.TateMode.horizontalRight:
				{
					this.tateModeTransform.localEulerAngles = new Vector3(0f, 0f, -90f);
					float num10 = this.renderingRawImage.rectTransform.sizeDelta.y / this.renderingRawImage.rectTransform.sizeDelta.x;
					this.tateModeTransform.localScale = new Vector3(num10, num10, 1f);
					break;
				}
				case Data.SettingsData.TateMode.upsideDown:
					this.tateModeTransform.localEulerAngles = new Vector3(0f, 0f, 180f);
					this.tateModeTransform.localScale = Vector3.one;
					break;
				}
			}
			this.crtMaterial.SetFloat("_TVBorder", (float)((Data.settings.crtFilter == Data.SettingsData.CrtFilter.border || Data.settings.crtFilter == Data.SettingsData.CrtFilter.full) ? 1 : 0));
			this.crtMaterial.SetFloat("_TVEffects", (float)((Data.settings.crtFilter == Data.SettingsData.CrtFilter.scanlines || Data.settings.crtFilter == Data.SettingsData.CrtFilter.full) ? 1 : 0));
			if (Master.instance.REND_AUTO_UPDATE_CAMERAS)
			{
				foreach (CameraGame cameraGame in CameraGame.list)
				{
					cameraGame.UpdateRenderingTexture();
				}
				foreach (CameraUi cameraUi in CameraUi.list)
				{
					cameraUi.UpdateRenderingTexture();
				}
				CameraUiGlobal cameraUiGlobal = CameraUiGlobal.instance;
				if (cameraUiGlobal != null)
				{
					cameraUiGlobal.UpdateRenderingTexture();
				}
			}
			RenderingMaster.firstBootUpdated = true;
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x000552C0 File Offset: 0x000534C0
		private void Awake()
		{
			if (RenderingMaster.instance != null)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			RenderingMaster.instance = this;
			this.ReferencesRefresh();
			RenderingMaster.renderTextureCurrent = this.renderTextureInitial;
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x000552F2 File Offset: 0x000534F2
		private void Start()
		{
			if (Master.instance.RENDER_TO_TEXTURE)
			{
				if (RenderingMaster.renderTextureCurrent == null)
				{
					Debug.LogError("RenderingMaster: renderTextureCurrent is null, but we are rendering to texture. Please assign a RenderTexture to the RenderingMaster component.");
				}
				if (this.renderingRawImage == null)
				{
					Debug.LogError("RenderingMaster: renderingRawImage is null, but we are rendering to texture. Please assign a RawImage to the RenderingMaster component.");
				}
			}
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0005532F File Offset: 0x0005352F
		private void OnDestroy()
		{
			if (RenderingMaster.instance == this)
			{
				RenderingMaster.instance = null;
			}
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00055344 File Offset: 0x00053544
		private void Update()
		{
			if (Master.instance.RENDER_TO_TEXTURE)
			{
				this.diffDelayTimer -= Tick.Time;
				float num = (float)(Mathf.Abs(Screen.width - this.displayOldSize.x) + Mathf.Abs(Screen.height - this.displayOldSize.y));
				if (num > 8f)
				{
					float? num2 = this.diffOld;
					float num3 = num;
					if (!((num2.GetValueOrDefault() == num3) & (num2 != null)))
					{
						this.diffDelayTimer = 0.5f;
					}
				}
				this.diffOld = new float?(num);
				if (this.diffDelayTimer <= 0f && num > 8f)
				{
					RenderingMaster.RenderingRefresh(true);
					this.displayOldSize.x = Screen.width;
					this.displayOldSize.y = Screen.height;
				}
			}
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00055418 File Offset: 0x00053618
		public void _OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			Graphics.Blit(src, dest, this.crtMaterial);
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00055427 File Offset: 0x00053627
		private void OnDrawGizmosSelected()
		{
			this.ReferencesRefresh();
			this.myCanvasScaler.referencePixelsPerUnit = 32f;
		}

		public static RenderingMaster instance;

		public Canvas myCanvas;

		public CanvasScaler myCanvasScaler;

		public RenderTexture renderTextureInitial;

		public static RenderTexture renderTextureCurrent;

		public Image renderingBackgroundImage;

		public RawImage renderingRawImage;

		public RectTransform tateModeTransform;

		public const FilterMode RENDER_TEXTURE_FILTER_MODE = FilterMode.Point;

		public const bool RENDER_TEXTURE_USE_MIP_MAPS = false;

		public const bool RENDER_TEXTURE_AUTO_MIP_MAPS = false;

		public const int RENDER_TEXTURE_ANISO_LEVEL = 0;

		public const TextureWrapMode RENDER_TEXTURE_WRAP_MODE = TextureWrapMode.Clamp;

		private Vector2Int displayOldSize = Vector2Int.zero;

		public Material crtMaterial;

		private static bool _splitScreenWarningLogged;

		private static bool firstBootUpdated;

		private float? diffOld;

		private float diffDelayTimer;
	}
}
