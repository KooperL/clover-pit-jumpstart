using System;
using Panik;
using UnityEngine;
using UnityEngine.Events;

public class MatTextureSwitcher_Localizer : MonoBehaviour
{
	// Token: 0x0600034C RID: 844 RVA: 0x00015240 File Offset: 0x00013440
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

	// Token: 0x0600034D RID: 845 RVA: 0x00015362 File Offset: 0x00013562
	private void Start()
	{
		this.Refresh();
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.Refresh));
	}

	// Token: 0x0600034E RID: 846 RVA: 0x0001538A File Offset: 0x0001358A
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
