using System;
using TMPro;
using UnityEngine;

namespace Panik
{
	public class FontsMaster : MonoBehaviour
	{
		// Token: 0x06000AAC RID: 2732 RVA: 0x000489BC File Offset: 0x00046BBC
		public TMP_FontAsset GetFontNormal(int index)
		{
			return this.fontsNormal[index];
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x000489C6 File Offset: 0x00046BC6
		public TMP_FontAsset GetFontDyslexic(int index)
		{
			return this.fontsDyslexic[index];
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x000489D0 File Offset: 0x00046BD0
		public Material GetFontMaterialNormal(int index)
		{
			return this.fontMaterialsNormal[index];
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x000489DA File Offset: 0x00046BDA
		public Material GetFontMaterialDyslexic(int index)
		{
			return this.fontMaterialsDyslexic[index];
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x000489E4 File Offset: 0x00046BE4
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

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00048A30 File Offset: 0x00046C30
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

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00048A7C File Offset: 0x00046C7C
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

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00048AC8 File Offset: 0x00046CC8
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

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00048B11 File Offset: 0x00046D11
		private void Awake()
		{
			if (FontsMaster.instance != null)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			FontsMaster.instance = this;
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00048B32 File Offset: 0x00046D32
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

		public static FontsMaster instance;

		public TMP_FontAsset[] fontsNormal;

		public TMP_FontAsset[] fontsDyslexic;

		public Material[] fontMaterialsNormal;

		public Material[] fontMaterialsDyslexic;
	}
}
