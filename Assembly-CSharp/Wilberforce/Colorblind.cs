using System;
using UnityEngine;

namespace Wilberforce
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[HelpURL("https://projectwilberforce.github.io/colorblind/")]
	[AddComponentMenu("Image Effects/Color Adjustments/Colorblind")]
	public class Colorblind : MonoBehaviour
	{
		// Token: 0x060010B0 RID: 4272 RVA: 0x00066FA3 File Offset: 0x000651A3
		private void ReportError(string error)
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("Colorblind Effect Error: " + error);
			}
		}

		// Token: 0x060010B1 RID: 4273 RVA: 0x00066FBC File Offset: 0x000651BC
		private void Start()
		{
			if (this.colorblindShader == null)
			{
				this.colorblindShader = Shader.Find("Hidden/Wilberforce/Colorblind");
			}
			if (this.colorblindShader == null)
			{
				this.ReportError("Could not locate Colorblind Shader. Make sure there is 'Colorblind.shader' file added to the project.");
				this.isSupported = false;
				base.enabled = false;
				return;
			}
			if (!SystemInfo.supportsImageEffects || SystemInfo.graphicsShaderLevel < 30)
			{
				if (!SystemInfo.supportsImageEffects)
				{
					this.ReportError("System does not support image effects.");
				}
				if (SystemInfo.graphicsShaderLevel < 30)
				{
					this.ReportError("This effect needs at least Shader Model 3.0.");
				}
				this.isSupported = false;
				base.enabled = false;
				return;
			}
			this.EnsureMaterials();
			if (!this.ColorblindMaterial || this.ColorblindMaterial.passCount != 1)
			{
				this.ReportError("Could not create shader.");
				this.isSupported = false;
				base.enabled = false;
				return;
			}
			this.isSupported = true;
		}

		// Token: 0x060010B2 RID: 4274 RVA: 0x00067096 File Offset: 0x00065296
		private static Material CreateMaterial(Shader shader)
		{
			if (!shader)
			{
				return null;
			}
			return new Material(shader)
			{
				hideFlags = HideFlags.HideAndDontSave
			};
		}

		// Token: 0x060010B3 RID: 4275 RVA: 0x000670B0 File Offset: 0x000652B0
		private static void DestroyMaterial(Material mat)
		{
			if (mat)
			{
				global::UnityEngine.Object.DestroyImmediate(mat);
				mat = null;
			}
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x000670C4 File Offset: 0x000652C4
		private void EnsureMaterials()
		{
			if (!this.ColorblindMaterial && this.colorblindShader.isSupported)
			{
				this.ColorblindMaterial = Colorblind.CreateMaterial(this.colorblindShader);
			}
			if (!this.colorblindShader.isSupported)
			{
				this.ReportError("Could not create shader (Shader not supported).");
			}
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00067114 File Offset: 0x00065314
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.isSupported || !this.colorblindShader.isSupported)
			{
				base.enabled = false;
				return;
			}
			this.EnsureMaterials();
			this.ColorblindMaterial.SetInt("type", this.Type);
			Graphics.Blit(source, destination, this.ColorblindMaterial, 0);
		}

		public int Type;

		public Shader colorblindShader;

		private bool isSupported;

		private Material ColorblindMaterial;
	}
}
