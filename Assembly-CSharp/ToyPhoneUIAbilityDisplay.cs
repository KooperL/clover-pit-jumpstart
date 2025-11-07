using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToyPhoneUIAbilityDisplay : MonoBehaviour
{
	// Token: 0x06000A42 RID: 2626 RVA: 0x00046A28 File Offset: 0x00044C28
	public void AbilitySet(AbilityScript.Identifier ability, int abilityPickIndex)
	{
		AbilityScript abilityScript = AbilityScript.AbilityGet(ability);
		string text = abilityScript.NameGetTranslated();
		string text2 = abilityScript.DescriptionGetTranslated();
		string text3 = "#" + (abilityPickIndex + 1).ToString();
		Sprite sprite = abilityScript.SpriteGet();
		this.abilityTitleText.text = text;
		this.abilityDescriptionText.text = text2;
		this.abilityPickNumberText.text = text3;
		this.abilityIconImage.sprite = sprite;
		this.abilityTitleText.color = abilityScript.ColorGet();
	}

	public TextMeshProUGUI abilityTitleText;

	public TextMeshProUGUI abilityDescriptionText;

	public TextMeshProUGUI abilityPickNumberText;

	public Image abilityIconImage;
}
