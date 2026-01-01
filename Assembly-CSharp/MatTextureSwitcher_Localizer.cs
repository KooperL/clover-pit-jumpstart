using System;
using Panik;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000027 RID: 39
public class MatTextureSwitcher_Localizer : MonoBehaviour
{
	// Token: 0x06000392 RID: 914 RVA: 0x00028680 File Offset: 0x00026880
	public void Refresh()
	{
		Translation.Language language = Translation.LanguageGet();
		switch (language)
		{
		case Translation.Language.English:
			this.sharedMat.mainTexture = this.textureEnglish;
			return;
		case Translation.Language.Italian:
			this.sharedMat.mainTexture = this.textureItalian;
			return;
		case Translation.Language.French:
			this.sharedMat.mainTexture = this.textureFrench;
			return;
		case Translation.Language.German:
			this.sharedMat.mainTexture = this.textureGerman;
			return;
		case Translation.Language.Spanish:
			this.sharedMat.mainTexture = this.textureSpanish;
			return;
		case Translation.Language.PortugueseBrazil:
			this.sharedMat.mainTexture = this.texturePortugueseBrazil;
			return;
		case Translation.Language.ChineseSimplified:
			this.sharedMat.mainTexture = this.textureChineseSimp;
			return;
		case Translation.Language.Japanese:
			this.sharedMat.mainTexture = this.textureJapanese;
			return;
		case Translation.Language.Russian:
			this.sharedMat.mainTexture = this.textureRussian;
			return;
		case Translation.Language.Korean:
			this.sharedMat.mainTexture = this.textureKorean;
			return;
		}
		Debug.LogError("MatTextureSwitcher_Localizer.Refresh(): language not handled: " + language.ToString());
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00008A94 File Offset: 0x00006C94
	private void Start()
	{
		this.Refresh();
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.Refresh));
	}

	// Token: 0x06000394 RID: 916 RVA: 0x00008ABC File Offset: 0x00006CBC
	private void OnDestroy()
	{
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.Refresh));
	}

	// Token: 0x040002DC RID: 732
	public Material sharedMat;

	// Token: 0x040002DD RID: 733
	public Texture textureEnglish;

	// Token: 0x040002DE RID: 734
	public Texture textureChineseSimp;

	// Token: 0x040002DF RID: 735
	public Texture texturePortugueseBrazil;

	// Token: 0x040002E0 RID: 736
	public Texture textureJapanese;

	// Token: 0x040002E1 RID: 737
	public Texture textureSpanish;

	// Token: 0x040002E2 RID: 738
	public Texture textureKorean;

	// Token: 0x040002E3 RID: 739
	public Texture textureFrench;

	// Token: 0x040002E4 RID: 740
	public Texture textureGerman;

	// Token: 0x040002E5 RID: 741
	public Texture textureRussian;

	// Token: 0x040002E6 RID: 742
	public Texture textureItalian;
}
