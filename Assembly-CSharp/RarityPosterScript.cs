using System;
using System.Text;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000E5 RID: 229
public class RarityPosterScript : MonoBehaviour
{
	// Token: 0x06000B97 RID: 2967 RVA: 0x0005D848 File Offset: 0x0005BA48
	private void TranslateTexts()
	{
		this.titleText.text = Translation.Get("RARITY_POSTER_TITLE");
		this.sb.Clear();
		this.sb.Append(Strings.GetPowerupRarity_SpriteString(PowerupScript.PublicRarity.common));
		this.sb.Append("\n");
		this.sb.Append(Strings.GetPowerupRarity_StringKey(PowerupScript.PublicRarity.common));
		this.sb.Append("\n<size=0.05>\n</size>");
		this.sb.Append(Strings.GetPowerupRarity_SpriteString(PowerupScript.PublicRarity.uncommon));
		this.sb.Append("\n");
		this.sb.Append(Strings.GetPowerupRarity_StringKey(PowerupScript.PublicRarity.uncommon));
		this.sb.Append("\n<size=0.05>\n</size>");
		this.sb.Append(Strings.GetPowerupRarity_SpriteString(PowerupScript.PublicRarity.rare));
		this.sb.Append("\n");
		this.sb.Append(Strings.GetPowerupRarity_StringKey(PowerupScript.PublicRarity.rare));
		this.sb.Append("\n<size=0.05>\n</size>");
		this.sb.Append(Strings.GetPowerupRarity_SpriteString(PowerupScript.PublicRarity.epic));
		this.sb.Append("\n");
		this.sb.Append(Strings.GetPowerupRarity_StringKey(PowerupScript.PublicRarity.epic));
		this.bodyText.text = this.sb.ToString();
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x0000F830 File Offset: 0x0000DA30
	private void Awake()
	{
		RarityPosterScript.instance = this;
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x0000F838 File Offset: 0x0000DA38
	private void Start()
	{
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.TranslateTexts));
		this.TranslateTexts();
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x0000F860 File Offset: 0x0000DA60
	private void OnDestroy()
	{
		if (RarityPosterScript.instance == this)
		{
			RarityPosterScript.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.TranslateTexts));
	}

	// Token: 0x04000C37 RID: 3127
	public static RarityPosterScript instance;

	// Token: 0x04000C38 RID: 3128
	public TextMeshProUGUI titleText;

	// Token: 0x04000C39 RID: 3129
	public TextMeshProUGUI bodyText;

	// Token: 0x04000C3A RID: 3130
	private StringBuilder sb = new StringBuilder();
}
