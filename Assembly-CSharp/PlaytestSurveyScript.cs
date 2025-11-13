using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlaytestSurveyScript : MonoBehaviour
{
	// Token: 0x060009AE RID: 2478 RVA: 0x000400B2 File Offset: 0x0003E2B2
	private void UpdateButtonText()
	{
		this.buttonText.text = Translation.Get("SURVEY_MEMO");
	}

	// Token: 0x060009AF RID: 2479 RVA: 0x000400C9 File Offset: 0x0003E2C9
	private void Awake()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x000400D7 File Offset: 0x0003E2D7
	private void Start()
	{
		this.UpdateButtonText();
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.UpdateButtonText));
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x000400FF File Offset: 0x0003E2FF
	private void OnDestroy()
	{
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.UpdateButtonText));
	}

	public TextMeshProUGUI buttonText;

	public GameObject holder;
}
