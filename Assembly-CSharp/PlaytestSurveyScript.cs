using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000D7 RID: 215
public class PlaytestSurveyScript : MonoBehaviour
{
	// Token: 0x06000B26 RID: 2854 RVA: 0x0000EDF0 File Offset: 0x0000CFF0
	private void UpdateButtonText()
	{
		this.buttonText.text = Translation.Get("SURVEY_MEMO");
	}

	// Token: 0x06000B27 RID: 2855 RVA: 0x0000EE07 File Offset: 0x0000D007
	private void Awake()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x06000B28 RID: 2856 RVA: 0x0000EE15 File Offset: 0x0000D015
	private void Start()
	{
		this.UpdateButtonText();
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.UpdateButtonText));
	}

	// Token: 0x06000B29 RID: 2857 RVA: 0x0000EE3D File Offset: 0x0000D03D
	private void OnDestroy()
	{
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.UpdateButtonText));
	}

	// Token: 0x04000B8A RID: 2954
	public TextMeshProUGUI buttonText;

	// Token: 0x04000B8B RID: 2955
	public GameObject holder;
}
