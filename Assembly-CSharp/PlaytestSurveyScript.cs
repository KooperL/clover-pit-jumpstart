using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlaytestSurveyScript : MonoBehaviour
{
	// Token: 0x0600099A RID: 2458 RVA: 0x0003FA4E File Offset: 0x0003DC4E
	private void UpdateButtonText()
	{
		this.buttonText.text = Translation.Get("SURVEY_MEMO");
	}

	// Token: 0x0600099B RID: 2459 RVA: 0x0003FA65 File Offset: 0x0003DC65
	private void Awake()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x0003FA73 File Offset: 0x0003DC73
	private void Start()
	{
		this.UpdateButtonText();
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.UpdateButtonText));
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x0003FA9B File Offset: 0x0003DC9B
	private void OnDestroy()
	{
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.UpdateButtonText));
	}

	public TextMeshProUGUI buttonText;

	public GameObject holder;
}
