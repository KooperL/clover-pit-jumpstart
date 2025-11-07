using System;
using UnityEngine;
using UnityEngine.UI;

namespace Panik
{
	public class RenderingMaster : MonoBehaviour
	{
		// Token: 0x06000D45 RID: 3397 RVA: 0x000543C7 File Offset: 0x000525C7
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

		// Token: 0x06000D46 RID: 3398 RVA: 0x00054400 File Offset: 0x00052600
		public static Vector2 GetRawImageSize()
		{
			if (RenderingMaster.instance == null)
			{
				return new Vector2((float)Screen.width, (float)Screen.height);
			}
			return new Vector2(RenderingMaster.instance.renderingRawImage.rectTransform.sizeDelta.x, RenderingMaster.instance.renderingRawImage.rectTransform.sizeDelta.y);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00054464 File Offset: 0x00052664
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

		// Token: 0x06000D48 RID: 3400 RVA: 0x000544D0 File Offset: 0x000526D0
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

		// Token: 0x06000D49 RID: 3401 RVA: 0x000545AC File Offset: 0x000527AC
		public static void RenderingRefresh(bool applyScreenRes)
		{
			RenderingMaster.instance._RenderingRefresh(applyScreenRes);
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x000545BC File Offset: 0x000527BC
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
					renderTexture.filterMode = 0;
					renderTexture.useMipMap = false;
					renderTexture.autoGenerateMips = false;
					renderTexture.anisoLevel = 0;
					renderTexture.wrapMode = 1;
					renderTexture.Create();
					RenderingMaster.renderTextureCurrent.Release();
					if (RenderingMaster.renderTextureCurrent != this.renderTextureInitial && RenderingMaster.renderTextureCurrent != null)
					{
						Object.Destroy(RenderingMaster.renderTextureCurrent);
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
							Screen.fullScreenMode = 3;
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

		// Token: 0x06000D4B RID: 3403 RVA: 0x00054AE4 File Offset: 0x00052CE4
		private void Awake()
		{
			if (RenderingMaster.instance != null)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			RenderingMaster.instance = this;
			this.ReferencesRefresh();
			RenderingMaster.renderTextureCurrent = this.renderTextureInitial;
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00054B16 File Offset: 0x00052D16
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

		// Token: 0x06000D4D RID: 3405 RVA: 0x00054B53 File Offset: 0x00052D53
		private void OnDestroy()
		{
			if (RenderingMaster.instance == this)
			{
				RenderingMaster.instance = null;
			}
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00054B68 File Offset: 0x00052D68
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

		// Token: 0x06000D4F RID: 3407 RVA: 0x00054C3C File Offset: 0x00052E3C
		public void _OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			Graphics.Blit(src, dest, this.crtMaterial);
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00054C4B File Offset: 0x00052E4B
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

		public const FilterMode RENDER_TEXTURE_FILTER_MODE = 0;

		public const bool RENDER_TEXTURE_USE_MIP_MAPS = false;

		public const bool RENDER_TEXTURE_AUTO_MIP_MAPS = false;

		public const int RENDER_TEXTURE_ANISO_LEVEL = 0;

		public const TextureWrapMode RENDER_TEXTURE_WRAP_MODE = 1;

		private Vector2Int displayOldSize = Vector2Int.zero;

		public Material crtMaterial;

		private static bool _splitScreenWarningLogged;

		private static bool firstBootUpdated;

		private float? diffOld;

		private float diffDelayTimer;
	}
}
