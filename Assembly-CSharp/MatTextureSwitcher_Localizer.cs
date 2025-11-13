using System;
using Panik;
using UnityEngine;
using UnityEngine.Events;

public class MatTextureSwitcher_Localizer : MonoBehaviour
{
	// Token: 0x0600034A RID: 842 RVA: 0x00015284 File Offset: 0x00013484
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

	// Token: 0x0600034B RID: 843 RVA: 0x000153A6 File Offset: 0x000135A6
	private void Start()
	{
		this.Refresh();
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.Refresh));
	}

	// Token: 0x0600034C RID: 844 RVA: 0x000153CE File Offset: 0x000135CE
	private void OnDestroy()
	{
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.Refresh));
	}

	public Material sharedMat;

	public Texture textureEnglish;

	public Texture textureChineseSimp;

	public Texture texturePortugueseBrazil;

	public Texture textureJapanese;

	public Texture textureSpanish;

	public Texture textureKorean;

	public Texture textureFrench;

	public Texture textureGerman;

	public Texture textureRussian;

	public Texture textureItalian;
}
