using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F8 RID: 248
public class ToyPhoneUIAbilityDisplay : MonoBehaviour
{
	// Token: 0x06000C1B RID: 3099 RVA: 0x0006122C File Offset: 0x0005F42C
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

	// Token: 0x04000CF1 RID: 3313
	public TextMeshProUGUI abilityTitleText;

	// Token: 0x04000CF2 RID: 3314
	public TextMeshProUGUI abilityDescriptionText;

	// Token: 0x04000CF3 RID: 3315
	public TextMeshProUGUI abilityPickNumberText;

	// Token: 0x04000CF4 RID: 3316
	public Image abilityIconImage;
}
