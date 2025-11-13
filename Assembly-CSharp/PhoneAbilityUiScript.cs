using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhoneAbilityUiScript : MonoBehaviour
{
	// Token: 0x06000987 RID: 2439 RVA: 0x0003EB3C File Offset: 0x0003CD3C
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

	// Token: 0x06000988 RID: 2440 RVA: 0x0003EC0E File Offset: 0x0003CE0E
	public static void SetAbility(int abilityIndex, AbilityScript ability)
	{
		PhoneAbilityUiScript.allAbilities[abilityIndex].SetAbility(ability);
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x0003EC24 File Offset: 0x0003CE24
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

	// Token: 0x0600098A RID: 2442 RVA: 0x0003ECAD File Offset: 0x0003CEAD
	private void Awake()
	{
		PhoneAbilityUiScript.allAbilities.Add(this);
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x0003ECBA File Offset: 0x0003CEBA
	private void OnDestroy()
	{
		PhoneAbilityUiScript.allAbilities.Remove(this);
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x0003ECC8 File Offset: 0x0003CEC8
	private void OnEnable()
	{
		PhoneAbilityUiScript.allEnabled.Add(this);
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x0003ECD5 File Offset: 0x0003CED5
	private void OnDisable()
	{
		PhoneAbilityUiScript.allEnabled.Remove(this);
	}

	public static List<PhoneAbilityUiScript> allAbilities = new List<PhoneAbilityUiScript>();

	public static List<PhoneAbilityUiScript> allEnabled = new List<PhoneAbilityUiScript>();

	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	public TextMeshProUGUI titleText;

	public TextMeshProUGUI descriptionText;

	public Image iconRenderer;

	public Image lastAbilityImage;

	public int abilityIndex = -1;
}
