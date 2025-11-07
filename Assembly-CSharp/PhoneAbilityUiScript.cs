using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhoneAbilityUiScript : MonoBehaviour
{
	// Token: 0x06000973 RID: 2419 RVA: 0x0003E4D8 File Offset: 0x0003C6D8
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

	// Token: 0x06000974 RID: 2420 RVA: 0x0003E5AA File Offset: 0x0003C7AA
	public static void SetAbility(int abilityIndex, AbilityScript ability)
	{
		PhoneAbilityUiScript.allAbilities[abilityIndex].SetAbility(ability);
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x0003E5C0 File Offset: 0x0003C7C0
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

	// Token: 0x06000976 RID: 2422 RVA: 0x0003E649 File Offset: 0x0003C849
	private void Awake()
	{
		PhoneAbilityUiScript.allAbilities.Add(this);
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x0003E656 File Offset: 0x0003C856
	private void OnDestroy()
	{
		PhoneAbilityUiScript.allAbilities.Remove(this);
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x0003E664 File Offset: 0x0003C864
	private void OnEnable()
	{
		PhoneAbilityUiScript.allEnabled.Add(this);
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x0003E671 File Offset: 0x0003C871
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
