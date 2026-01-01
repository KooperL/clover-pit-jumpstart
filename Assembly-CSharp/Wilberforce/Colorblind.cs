using System;
using UnityEngine;

namespace Wilberforce
{
	// Token: 0x02000201 RID: 513
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[HelpURL("https://projectwilberforce.github.io/colorblind/")]
	[AddComponentMenu("Image Effects/Color Adjustments/Colorblind")]
	public class Colorblind : MonoBehaviour
	{
		// Token: 0x060014EF RID: 5359 RVA: 0x00015E6F File Offset: 0x0001406F
		private void ReportError(string error)
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log("Colorblind Effect Error: " + error);
			}
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0008536C File Offset: 0x0008356C
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

		// Token: 0x060014F1 RID: 5361 RVA: 0x00015E88 File Offset: 0x00014088
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

		// Token: 0x060014F2 RID: 5362 RVA: 0x00015EA2 File Offset: 0x000140A2
		private static void DestroyMaterial(Material mat)
		{
			if (mat)
			{
				global::UnityEngine.Object.DestroyImmediate(mat);
				mat = null;
			}
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x00085448 File Offset: 0x00083648
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

		// Token: 0x060014F4 RID: 5364 RVA: 0x00085498 File Offset: 0x00083698
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

		// Token: 0x0400145C RID: 5212
		public int Type;

		// Token: 0x0400145D RID: 5213
		public Shader colorblindShader;

		// Token: 0x0400145E RID: 5214
		private bool isSupported;

		// Token: 0x0400145F RID: 5215
		private Material ColorblindMaterial;
	}
}
