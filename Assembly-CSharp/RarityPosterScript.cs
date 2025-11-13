using System;
using System.Text;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RarityPosterScript : MonoBehaviour
{
	// Token: 0x060009EF RID: 2543 RVA: 0x00043DC8 File Offset: 0x00041FC8
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

	// Token: 0x060009F0 RID: 2544 RVA: 0x00043F13 File Offset: 0x00042113
	private void Awake()
	{
		RarityPosterScript.instance = this;
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x00043F1B File Offset: 0x0004211B
	private void Start()
	{
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.TranslateTexts));
		this.TranslateTexts();
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x00043F43 File Offset: 0x00042143
	private void OnDestroy()
	{
		if (RarityPosterScript.instance == this)
		{
			RarityPosterScript.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.TranslateTexts));
	}

	public static RarityPosterScript instance;

	public TextMeshProUGUI titleText;

	public TextMeshProUGUI bodyText;

	private StringBuilder sb = new StringBuilder();
}
