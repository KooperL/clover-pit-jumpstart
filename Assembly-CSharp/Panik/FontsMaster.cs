using System;
using TMPro;
using UnityEngine;

namespace Panik
{
	public class FontsMaster : MonoBehaviour
	{
		// Token: 0x06000AC1 RID: 2753 RVA: 0x0004911C File Offset: 0x0004731C
		public TMP_FontAsset GetFontNormal(int index)
		{
			return this.fontsNormal[index];
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00049126 File Offset: 0x00047326
		public TMP_FontAsset GetFontDyslexic(int index)
		{
			return this.fontsDyslexic[index];
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00049130 File Offset: 0x00047330
		public Material GetFontMaterialNormal(int index)
		{
			return this.fontMaterialsNormal[index];
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x0004913A File Offset: 0x0004733A
		public Material GetFontMaterialDyslexic(int index)
		{
			return this.fontMaterialsDyslexic[index];
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00049144 File Offset: 0x00047344
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

		// Token: 0x06000AC6 RID: 2758 RVA: 0x00049190 File Offset: 0x00047390
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

		// Token: 0x06000AC7 RID: 2759 RVA: 0x000491DC File Offset: 0x000473DC
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

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00049228 File Offset: 0x00047428
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

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00049271 File Offset: 0x00047471
		private void Awake()
		{
			if (FontsMaster.instance != null)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			FontsMaster.instance = this;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00049292 File Offset: 0x00047492
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
