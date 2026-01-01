using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D3 RID: 211
public class PhoneAbilityUiScript : MonoBehaviour
{
	// Token: 0x06000AF9 RID: 2809 RVA: 0x000582F0 File Offset: 0x000564F0
	private void SetAbility(AbilityScript ability)
	{
		base.gameObject.SetActive(true);
		if (ability == null)
		{
			this.titleText.text = "! NULL ABILITY !";
			this.descriptionText.text = "...";
			this.iconRenderer.sprite = null;
			this.iconRenderer.enabled = false;
			this.lastAbilityImage.enabled = false;
			this.titleText.color = Color.red;
			return;
		}
		this.titleText.text = ability.NameGetTranslated();
		this.descriptionText.text = ability.DescriptionGetTranslated();
		this.iconRenderer.sprite = ability.SpriteGet();
		this.iconRenderer.enabled = true;
		this.lastAbilityImage.enabled = ability.IsLastPickAvailable();
		this.titleText.color = ability.ColorGet();
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x0000EB8B File Offset: 0x0000CD8B
	public static void SetAbility(int abilityIndex, AbilityScript ability)
	{
		PhoneAbilityUiScript.allAbilities[abilityIndex].SetAbility(ability);
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x000583C4 File Offset: 0x000565C4
	public static void InitializeAll()
	{
		for (int i = 0; i < PhoneAbilityUiScript.allAbilities.Count; i++)
		{
			for (int j = i + 1; j < PhoneAbilityUiScript.allAbilities.Count; j++)
			{
				if (PhoneAbilityUiScript.allAbilities[i].abilityIndex > PhoneAbilityUiScript.allAbilities[j].abilityIndex)
				{
					PhoneAbilityUiScript phoneAbilityUiScript = PhoneAbilityUiScript.allAbilities[i];
					PhoneAbilityUiScript.allAbilities[i] = PhoneAbilityUiScript.allAbilities[j];
					PhoneAbilityUiScript.allAbilities[j] = phoneAbilityUiScript;
				}
			}
		}
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x0000EB9E File Offset: 0x0000CD9E
	private void Awake()
	{
		PhoneAbilityUiScript.allAbilities.Add(this);
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x0000EBAB File Offset: 0x0000CDAB
	private void OnDestroy()
	{
		PhoneAbilityUiScript.allAbilities.Remove(this);
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x0000EBB9 File Offset: 0x0000CDB9
	private void OnEnable()
	{
		PhoneAbilityUiScript.allEnabled.Add(this);
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x0000EBC6 File Offset: 0x0000CDC6
	private void OnDisable()
	{
		PhoneAbilityUiScript.allEnabled.Remove(this);
	}

	// Token: 0x04000B4D RID: 2893
	public static List<PhoneAbilityUiScript> allAbilities = new List<PhoneAbilityUiScript>();

	// Token: 0x04000B4E RID: 2894
	public static List<PhoneAbilityUiScript> allEnabled = new List<PhoneAbilityUiScript>();

	// Token: 0x04000B4F RID: 2895
	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x04000B50 RID: 2896
	public TextMeshProUGUI titleText;

	// Token: 0x04000B51 RID: 2897
	public TextMeshProUGUI descriptionText;

	// Token: 0x04000B52 RID: 2898
	public Image iconRenderer;

	// Token: 0x04000B53 RID: 2899
	public Image lastAbilityImage;

	// Token: 0x04000B54 RID: 2900
	public int abilityIndex = -1;
}
