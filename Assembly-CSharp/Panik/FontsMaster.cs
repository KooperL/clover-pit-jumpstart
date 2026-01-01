using System;
using TMPro;
using UnityEngine;

namespace Panik
{
	// Token: 0x0200010B RID: 267
	public class FontsMaster : MonoBehaviour
	{
		// Token: 0x06000C9B RID: 3227 RVA: 0x00010521 File Offset: 0x0000E721
		public TMP_FontAsset GetFontNormal(int index)
		{
			return this.fontsNormal[index];
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0001052B File Offset: 0x0000E72B
		public TMP_FontAsset GetFontDyslexic(int index)
		{
			return this.fontsDyslexic[index];
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x00010535 File Offset: 0x0000E735
		public Material GetFontMaterialNormal(int index)
		{
			return this.fontMaterialsNormal[index];
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0001053F File Offset: 0x0000E73F
		public Material GetFontMaterialDyslexic(int index)
		{
			return this.fontMaterialsDyslexic[index];
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00063384 File Offset: 0x00061584
		public TMP_FontAsset GetFontNormalFromDyslexic(TMP_FontAsset dyslexicFont)
		{
			int num = Array.IndexOf<TMP_FontAsset>(this.fontsDyslexic, dyslexicFont);
			if (num <= -1)
			{
				Debug.LogError("FontsMaster: dyslexicFont not found in fontsDyslexic array");
				return null;
			}
			if (num >= this.fontsNormal.Length)
			{
				Debug.LogError("FontsMaster: dyslexicFont index is out of bounds in fontsDyslexic array");
				return null;
			}
			return this.fontsNormal[num];
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x000633D0 File Offset: 0x000615D0
		public TMP_FontAsset GetFontDyslexicFromNormal(TMP_FontAsset normalFont)
		{
			int num = Array.IndexOf<TMP_FontAsset>(this.fontsNormal, normalFont);
			if (num <= -1)
			{
				Debug.LogError("FontsMaster: normalFont not found in fontsNormal array");
				return null;
			}
			if (num >= this.fontsDyslexic.Length)
			{
				Debug.LogError("FontsMaster: normalFont index is out of bounds in fontsNormal array");
				return null;
			}
			return this.fontsDyslexic[num];
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0006341C File Offset: 0x0006161C
		public Material GetFontMaterialNormalFromDyslexic(Material dyslexicFontMaterial)
		{
			int num = Array.IndexOf<Material>(this.fontMaterialsDyslexic, dyslexicFontMaterial);
			if (num <= -1)
			{
				Debug.LogError("FontsMaster: dyslexicFontMaterial not found in fontMaterialsDyslexic array");
				return null;
			}
			if (num >= this.fontMaterialsNormal.Length)
			{
				Debug.LogError("FontsMaster: dyslexicFontMaterial index is out of bounds in fontMaterialsDyslexic array");
				return null;
			}
			return this.fontMaterialsNormal[num];
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x00063468 File Offset: 0x00061668
		public Material GetFontMaterialDyslexicFromNormal(Material normalFontMaterial)
		{
			int num = Array.IndexOf<Material>(this.fontMaterialsNormal, normalFontMaterial);
			if (num <= -1)
			{
				Debug.LogError("FontsMaster: normalFontMaterial not found in fontMaterialsNormal array");
				return null;
			}
			if (num >= this.fontMaterialsDyslexic.Length)
			{
				Debug.LogError("FontsMaster: normalFontMaterial index is out of bounds in fontMaterialsNormal array");
				return null;
			}
			return this.fontMaterialsDyslexic[num];
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x00010549 File Offset: 0x0000E749
		private void Awake()
		{
			if (FontsMaster.instance != null)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			FontsMaster.instance = this;
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0001056A File Offset: 0x0000E76A
		private void Start()
		{
			if (this.fontsNormal.Length != this.fontsDyslexic.Length)
			{
				Debug.LogError("FontsMaster: fontsNormal and fontsDyslexic are not of the same size. You should always have a dyslexic font paired with a normal one!");
				return;
			}
			if (this.fontMaterialsNormal.Length != this.fontMaterialsDyslexic.Length)
			{
				Debug.LogError("FontsMaster: fontMaterialsNormal and fontMaterialsDyslexic are not of the same size. You should always have a dyslexic font material paired with a normal one!");
				return;
			}
		}

		// Token: 0x04000D72 RID: 3442
		public static FontsMaster instance;

		// Token: 0x04000D73 RID: 3443
		public TMP_FontAsset[] fontsNormal;

		// Token: 0x04000D74 RID: 3444
		public TMP_FontAsset[] fontsDyslexic;

		// Token: 0x04000D75 RID: 3445
		public Material[] fontMaterialsNormal;

		// Token: 0x04000D76 RID: 3446
		public Material[] fontMaterialsDyslexic;
	}
}
