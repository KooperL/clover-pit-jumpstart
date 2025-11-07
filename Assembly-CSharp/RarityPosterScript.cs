using System;
using System.Text;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RarityPosterScript : MonoBehaviour
{
	// Token: 0x060009DB RID: 2523 RVA: 0x00043760 File Offset: 0x00041960
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

	// Token: 0x060009DC RID: 2524 RVA: 0x000438AB File Offset: 0x00041AAB
	private void Awake()
	{
		RarityPosterScript.instance = this;
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x000438B3 File Offset: 0x00041AB3
	private void Start()
	{
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.TranslateTexts));
		this.TranslateTexts();
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x000438DB File Offset: 0x00041ADB
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
