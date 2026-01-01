using System;
using UnityEngine;
using UnityEngine.UI;

namespace Panik
{
	// Token: 0x0200016F RID: 367
	public class RenderingMaster : MonoBehaviour
	{
		// Token: 0x060010E5 RID: 4325 RVA: 0x00013CF2 File Offset: 0x00011EF2
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

		// Token: 0x060010E6 RID: 4326 RVA: 0x000729A4 File Offset: 0x00070BA4
		public static Vector2 GetRawImageSize()
		{
			if (RenderingMaster.instance == null)
			{
				return new Vector2((float)Screen.width, (float)Screen.height);
			}
			return new Vector2(RenderingMaster.instance.renderingRawImage.rectTransform.sizeDelta.x, RenderingMaster.instance.renderingRawImage.rectTransform.sizeDelta.y);
		}

		// Token: 0x060010E7 RID: 4327 RVA: 0x00072A08 File Offset: 0x00070C08
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

		// Token: 0x060010E8 RID: 4328 RVA: 0x00072A74 File Offset: 0x00070C74
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

		// Token: 0x060010E9 RID: 4329 RVA: 0x00013D28 File Offset: 0x00011F28
		public static void RenderingRefresh(bool applyScreenRes)
		{
			RenderingMaster.instance._RenderingRefresh(applyScreenRes);
		}

		// Token: 0x060010EA RID: 4330 RVA: 0x00072B50 File Offset: 0x00070D50
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

		// Token: 0x060010EB RID: 4331 RVA: 0x00013D35 File Offset: 0x00011F35
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

		// Token: 0x060010EC RID: 4332 RVA: 0x00013D67 File Offset: 0x00011F67
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

		// Token: 0x060010ED RID: 4333 RVA: 0x00013DA4 File Offset: 0x00011FA4
		private void OnDestroy()
		{
			if (RenderingMaster.instance == this)
			{
				RenderingMaster.instance = null;
			}
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x00073078 File Offset: 0x00071278
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

		// Token: 0x060010EF RID: 4335 RVA: 0x00013DB9 File Offset: 0x00011FB9
		public void _OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			Graphics.Blit(src, dest, this.crtMaterial);
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x00013DC8 File Offset: 0x00011FC8
		private void OnDrawGizmosSelected()
		{
			this.ReferencesRefresh();
			this.myCanvasScaler.referencePixelsPerUnit = 32f;
		}

		// Token: 0x040011CB RID: 4555
		public static RenderingMaster instance;

		// Token: 0x040011CC RID: 4556
		public Canvas myCanvas;

		// Token: 0x040011CD RID: 4557
		public CanvasScaler myCanvasScaler;

		// Token: 0x040011CE RID: 4558
		public RenderTexture renderTextureInitial;

		// Token: 0x040011CF RID: 4559
		public static RenderTexture renderTextureCurrent;

		// Token: 0x040011D0 RID: 4560
		public Image renderingBackgroundImage;

		// Token: 0x040011D1 RID: 4561
		public RawImage renderingRawImage;

		// Token: 0x040011D2 RID: 4562
		public RectTransform tateModeTransform;

		// Token: 0x040011D3 RID: 4563
		public const FilterMode RENDER_TEXTURE_FILTER_MODE = FilterMode.Point;

		// Token: 0x040011D4 RID: 4564
		public const bool RENDER_TEXTURE_USE_MIP_MAPS = false;

		// Token: 0x040011D5 RID: 4565
		public const bool RENDER_TEXTURE_AUTO_MIP_MAPS = false;

		// Token: 0x040011D6 RID: 4566
		public const int RENDER_TEXTURE_ANISO_LEVEL = 0;

		// Token: 0x040011D7 RID: 4567
		public const TextureWrapMode RENDER_TEXTURE_WRAP_MODE = TextureWrapMode.Clamp;

		// Token: 0x040011D8 RID: 4568
		private Vector2Int displayOldSize = Vector2Int.zero;

		// Token: 0x040011D9 RID: 4569
		public Material crtMaterial;

		// Token: 0x040011DA RID: 4570
		private static bool _splitScreenWarningLogged;

		// Token: 0x040011DB RID: 4571
		private static bool firstBootUpdated;

		// Token: 0x040011DC RID: 4572
		private float? diffOld;

		// Token: 0x040011DD RID: 4573
		private float diffDelayTimer;
	}
}
