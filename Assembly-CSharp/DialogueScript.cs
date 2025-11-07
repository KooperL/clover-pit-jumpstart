using System;
using System.Collections.Generic;
using Febucci.UI;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour
{
	// Token: 0x0600085D RID: 2141 RVA: 0x00036921 File Offset: 0x00034B21
	public static bool IsEnabled()
	{
		return DialogueScript.instance.holder.activeSelf;
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x00036932 File Offset: 0x00034B32
	public static bool IsAskingQuestion()
	{
		return (DialogueScript.IsEnabled() && DialogueScript.instance.onYes != null) || DialogueScript.instance.onNo != null;
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x00036958 File Offset: 0x00034B58
	public static void SetDialogue(List<string> keys, bool concatenate)
	{
		if (!DialogueScript.instance.legalDuringDeathCooldown && GameplayMaster.DeathCountdownHasStarted())
		{
			return;
		}
		DialogueScript.instance.legalDuringDeathCooldown = false;
		bool flag = DialogueScript.IsEnabled();
		if (!flag)
		{
			DialogueScript.instance.holder.SetActive(true);
		}
		if (!concatenate || !flag)
		{
			DialogueScript.dialogueKeys.Clear();
			DialogueScript.instance.dialogueIndex = -1;
		}
		DialogueScript.dialogueKeys.AddRange(keys);
		DialogueScript.instance.autoCloseTimer = -1f;
		DialogueScript.instance.autoProgressTimer = -1f;
		DialogueScript.instance.autoProgressTimerLastValue = -1f;
		DialogueScript.instance.onDialogueNext = null;
		DialogueScript.instance.onDialogueClose = null;
		if (DialogueScript.instance.dialogueIndex == -1)
		{
			DialogueScript.instance.NextDialogue();
		}
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x00036A1D File Offset: 0x00034C1D
	public static void SetDialogue(bool concatenate, params string[] keys)
	{
		if (concatenate && (DialogueScript.instance.onYes != null || DialogueScript.instance.onNo != null))
		{
			Debug.LogError("DialogueScript: SetDialogue: onYes or onNo is not null. Cannot concatenate a dialogue after a question! Please wait for the dialogue to Close();");
			return;
		}
		DialogueScript.SetDialogue(new List<string>(keys), concatenate);
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x00036A51 File Offset: 0x00034C51
	public static int GetDialogueIndex()
	{
		return DialogueScript.instance.dialogueIndex;
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x00036A5D File Offset: 0x00034C5D
	public static void SetDialogueInputDelay(float value)
	{
		DialogueScript.instance.allDialoguesInputDelay = value;
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x00036A6C File Offset: 0x00034C6C
	public static void SetQuestionDialogue(bool concatenate, DialogueScript.AnswerCallback onYes, DialogueScript.AnswerCallback onNo, params string[] keys)
	{
		if (!DialogueScript.instance.legalDuringDeathCooldown && GameplayMaster.DeathCountdownHasStarted())
		{
			return;
		}
		if (concatenate && (DialogueScript.instance.onYes != null || DialogueScript.instance.onNo != null))
		{
			Debug.LogError("DialogueScript: SetQuestionDialogue: onYes or onNo is not null. Cannot concatenate a question after another! Please wait for the dialogue to Close();");
			return;
		}
		DialogueScript.SetDialogue(concatenate, keys);
		DialogueScript.instance.onYes = onYes;
		DialogueScript.instance.onNo = onNo;
		DialogueScript.instance.questionDelay = 0.5f;
		DialogueScript.instance.AnswerInputReset();
		string text = "";
		if (DialogueScript.instance.onYes != null)
		{
			text = Translation.Get("MENU_OPTION_YES");
		}
		DialogueScript.instance.answerPromptsPlayer_Yes.textAnimator.tmproText.enabled = true;
		DialogueScript.instance.answerPromptsPlayer_Yes.ShowText(text);
		string text2 = "";
		if (DialogueScript.instance.onNo != null)
		{
			text2 = Translation.Get("MENU_OPTION_NO");
		}
		DialogueScript.instance.answerPromptsPlayer_No.textAnimator.tmproText.enabled = true;
		DialogueScript.instance.answerPromptsPlayer_No.ShowText(text2);
		DialogueScript.instance.question_CursorWasVisibleBeforeQuestion = new bool?(VirtualCursors.CursorDesiredVisibilityGet(0));
		VirtualCursors.CursorDesiredVisibilitySet(0, true);
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x00036B94 File Offset: 0x00034D94
	public static void Close()
	{
		if (!DialogueScript.IsEnabled())
		{
			return;
		}
		if ((DialogueScript.instance.onYes != null || DialogueScript.instance.onNo != null) && DialogueScript.instance.question_CursorWasVisibleBeforeQuestion != null)
		{
			VirtualCursors.CursorDesiredVisibilitySet(0, DialogueScript.instance.question_CursorWasVisibleBeforeQuestion.Value);
		}
		DialogueScript.instance.holder.SetActive(false);
		DialogueScript.instance.onYes = null;
		DialogueScript.instance.onNo = null;
		DialogueScript.instance.AnswerInputReset();
		DialogueScript.instance.answerPromptsPlayer_Yes.textAnimator.tmproText.text = "";
		DialogueScript.instance.answerPromptsPlayer_Yes.ShowText("");
		DialogueScript.instance.answerPromptsPlayer_No.textAnimator.tmproText.text = "";
		DialogueScript.instance.answerPromptsPlayer_No.ShowText("");
		DialogueScript.AnswerCallback answerCallback = DialogueScript.instance.onDialogueClose;
		if (answerCallback != null)
		{
			answerCallback();
		}
		if (DialogueScript.instance.soundOriginTransform == null)
		{
			Sound.Play_Unpausable("SoundDialogueClose", 1f, 1f);
		}
		else
		{
			Sound.Play3D_Unpausable("SoundDialogueClose", DialogueScript.instance.soundOriginTransform.position, 20f, 1f, 1f, 1);
		}
		Controls.VibrationSet_PreferMax(DialogueScript.instance.player, 0.25f);
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x00036CF8 File Offset: 0x00034EF8
	private void NextDialogue()
	{
		this.dialogueIndex++;
		this.currentDialogueString_Translated = Translation.Get(DialogueScript.dialogueKeys[this.dialogueIndex]);
		this.textAnimatorPlayer.textAnimator.tmproText.text = "";
		this.textAnimatorPlayer.ShowText("");
		this.textAnimatorPlayer.ShowText(Strings.Sanitize(Strings.SantizationKind.ui, this.currentDialogueString_Translated, Strings.SanitizationSubKind.none));
		this.AnswerInputReset();
		Controls.VibrationSet_PreferMax(this.player, 0.25f);
		if (this.dialogueIndex == 0)
		{
			Sound.Stop("SoundDialogueClose", true);
			if (!Sound.IsPlaying("SoundATMFanfare"))
			{
				if (DialogueScript.instance.soundOriginTransform == null)
				{
					Sound.Play_Unpausable("SoundDialogueStart", 1f, 1f);
				}
				else
				{
					Sound.Play3D_Unpausable("SoundDialogueStart", DialogueScript.instance.soundOriginTransform.position, 20f, 1f, 1f, 1);
				}
			}
		}
		else if (DialogueScript.instance.soundOriginTransform == null)
		{
			Sound.Play_Unpausable("SoundDialogueNext", 1f, 1f);
		}
		else
		{
			Sound.Play3D_Unpausable("SoundDialogueNext", DialogueScript.instance.soundOriginTransform.position, 20f, 1f, 1f, 1);
		}
		DialogueScript.AnswerCallback answerCallback = this.onDialogueNext;
		if (answerCallback == null)
		{
			return;
		}
		answerCallback();
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x00036E60 File Offset: 0x00035060
	private void AnswerInputCompute()
	{
		if (!DialogueScript.IsEnabled())
		{
			return;
		}
		if (this.onYes == null && this.onNo == null)
		{
			return;
		}
		if (VirtualCursors.IsCursorVisible(0, true))
		{
			Vector2 referenceResolution = this.canvasScaler.referenceResolution;
			Vector2 vector = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, referenceResolution);
			RectTransform rectTransform = this.answerPromptsPlayer_Yes.textAnimator.tmproText.rectTransform;
			if (vector.x > rectTransform.anchoredPosition.x - rectTransform.sizeDelta.x / 2f && vector.x < rectTransform.anchoredPosition.x + rectTransform.sizeDelta.x / 2f && vector.y > rectTransform.anchoredPosition.y - rectTransform.sizeDelta.y / 2f && vector.y < rectTransform.anchoredPosition.y + rectTransform.sizeDelta.y / 2f)
			{
				this.questionAnswer = DialogueScript.QuestionAnswer.yes;
			}
			else
			{
				rectTransform = this.answerPromptsPlayer_No.textAnimator.tmproText.rectTransform;
				if (vector.x > rectTransform.anchoredPosition.x - rectTransform.sizeDelta.x / 2f && vector.x < rectTransform.anchoredPosition.x + rectTransform.sizeDelta.x / 2f && vector.y > rectTransform.anchoredPosition.y - rectTransform.sizeDelta.y / 2f && vector.y < rectTransform.anchoredPosition.y + rectTransform.sizeDelta.y / 2f)
				{
					this.questionAnswer = DialogueScript.QuestionAnswer.no;
				}
				else
				{
					this.questionAnswer = DialogueScript.QuestionAnswer.undefined;
				}
			}
		}
		else
		{
			float num = Controls.ActionAxisPair_GetValue(0, Controls.InputAction.menuMoveRight, Controls.InputAction.menuMoveLeft, true);
			if (num < -0.35f)
			{
				this.questionAnswer = DialogueScript.QuestionAnswer.yes;
			}
			else if (num > 0.35f)
			{
				this.questionAnswer = DialogueScript.QuestionAnswer.no;
			}
			if (this.questionAnswer == DialogueScript.QuestionAnswer.undefined && Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true))
			{
				this.questionAnswer = DialogueScript.QuestionAnswer.yes;
				DialogueScript.SetDialogueInputDelay(0.1f);
			}
			if (this.questionAnswer == DialogueScript.QuestionAnswer.undefined && Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true))
			{
				this.questionAnswer = DialogueScript.QuestionAnswer.no;
				DialogueScript.SetDialogueInputDelay(0.1f);
			}
		}
		if (this.questionAnswerOld != this.questionAnswer)
		{
			this.questionAnswerOld = this.questionAnswer;
			if (this.questionAnswer != DialogueScript.QuestionAnswer.undefined)
			{
				if (DialogueScript.instance.soundOriginTransform == null)
				{
					Sound.Play_Unpausable("SoundMenuSelectionChange", 1f, 1f);
				}
				else
				{
					Sound.Play3D_Unpausable("SoundMenuSelectionChange", DialogueScript.instance.soundOriginTransform.position, 20f, 1f, 1f, 1);
				}
			}
			switch (this.questionAnswer)
			{
			case DialogueScript.QuestionAnswer.undefined:
				this.answerPromptsPlayer_Yes.textAnimator.tmproText.alpha = 0.25f;
				this.answerPromptsPlayer_No.textAnimator.tmproText.alpha = 0.25f;
				return;
			case DialogueScript.QuestionAnswer.yes:
				this.answerPromptsPlayer_Yes.textAnimator.tmproText.alpha = 1f;
				this.answerPromptsPlayer_No.textAnimator.tmproText.alpha = 0.25f;
				return;
			case DialogueScript.QuestionAnswer.no:
				this.answerPromptsPlayer_Yes.textAnimator.tmproText.alpha = 0.25f;
				this.answerPromptsPlayer_No.textAnimator.tmproText.alpha = 1f;
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x000371BC File Offset: 0x000353BC
	private void AnswerInputReset()
	{
		this.questionAnswer = DialogueScript.QuestionAnswer.undefined;
		this.questionAnswerOld = DialogueScript.QuestionAnswer.undefined;
		this.answerPromptsPlayer_Yes.textAnimator.tmproText.alpha = 0.25f;
		this.answerPromptsPlayer_No.textAnimator.tmproText.alpha = 0.25f;
		this.question_CursorWasVisibleBeforeQuestion = null;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x00037217 File Offset: 0x00035417
	public static void SetAutoClose(float time)
	{
		DialogueScript.instance.autoCloseTimer = time;
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x00037224 File Offset: 0x00035424
	public static void SetAutoProgress(float time)
	{
		DialogueScript.instance.autoProgressTimer = time;
		DialogueScript.instance.autoProgressTimerLastValue = time;
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x0003723C File Offset: 0x0003543C
	public static void NextIsLegalDuringDeathCooldown()
	{
		if (DialogueScript.instance == null)
		{
			return;
		}
		DialogueScript.instance.legalDuringDeathCooldown = true;
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00037257 File Offset: 0x00035457
	private void Awake()
	{
		DialogueScript.instance = this;
		DialogueScript.dialogueKeys.Clear();
		this.holder.SetActive(false);
		this.backImageStartingLocalPos = this.backImageRectTransform.anchoredPosition;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x00037286 File Offset: 0x00035486
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		this.AnswerInputReset();
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x0003729A File Offset: 0x0003549A
	private void OnDestroy()
	{
		if (DialogueScript.instance == this)
		{
			DialogueScript.instance = null;
		}
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x000372B0 File Offset: 0x000354B0
	private void Update()
	{
		if (!Tick.IsGameRunning)
		{
			return;
		}
		bool flag = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true);
		this.AnswerInputCompute();
		bool flag2 = this.questionAnswer == DialogueScript.QuestionAnswer.yes && flag;
		bool flag3 = this.questionAnswer == DialogueScript.QuestionAnswer.no && flag;
		bool flag4 = this.onYes != null || this.onNo != null;
		bool flag5 = flag4 && this.dialogueIndex >= DialogueScript.dialogueKeys.Count - 1;
		if (flag4)
		{
			if (!flag5)
			{
				if (this.answerPromptsPlayer_Yes.textAnimator.tmproText.enabled)
				{
					this.answerPromptsPlayer_Yes.textAnimator.tmproText.enabled = false;
				}
				if (this.answerPromptsPlayer_No.textAnimator.tmproText.enabled)
				{
					this.answerPromptsPlayer_No.textAnimator.tmproText.enabled = false;
				}
				this.questionDelay = 0.5f;
			}
			else
			{
				if (!this.answerPromptsPlayer_Yes.textAnimator.tmproText.enabled)
				{
					this.answerPromptsPlayer_Yes.textAnimator.tmproText.enabled = true;
				}
				if (!this.answerPromptsPlayer_No.textAnimator.tmproText.enabled)
				{
					this.answerPromptsPlayer_No.textAnimator.tmproText.enabled = true;
				}
				this.questionDelay -= Tick.Time;
			}
		}
		else
		{
			this.questionDelay = 0.5f;
			if (this.answerPromptsPlayer_Yes.textAnimator.tmproText.enabled)
			{
				this.answerPromptsPlayer_Yes.textAnimator.tmproText.enabled = false;
			}
			if (this.answerPromptsPlayer_No.textAnimator.tmproText.enabled)
			{
				this.answerPromptsPlayer_No.textAnimator.tmproText.enabled = false;
			}
		}
		bool flag6 = this.textAnimatorPlayer.textAnimator.allLettersShown;
		if (flag6)
		{
			this.allDialoguesInputDelay -= Tick.Time;
		}
		if (this.allDialoguesInputDelay <= 0f && this.autoProgressTimer >= 0f && DialogueScript.IsEnabled())
		{
			if (!flag6)
			{
				flag6 = true;
				this.textAnimatorPlayer.textAnimator.ShowAllCharacters(true);
			}
			this.autoProgressTimer -= Tick.Time;
			if (this.autoProgressTimer <= 0f)
			{
				this.autoProgressTimer = this.autoProgressTimerLastValue;
				flag = true;
				flag2 = true;
			}
		}
		if ((flag5 && ((flag2 && this.onYes != null) || (flag3 && this.onNo != null)) && this.questionDelay <= 0f && this.allDialoguesInputDelay <= 0f) || (!flag5 && flag && this.allDialoguesInputDelay <= 0f))
		{
			if (flag6)
			{
				if (this.dialogueIndex < DialogueScript.dialogueKeys.Count - 1)
				{
					this.NextDialogue();
				}
				else
				{
					if (this.onYes != null && flag2)
					{
						this.onYes();
					}
					else if (this.onNo != null && flag3)
					{
						this.onNo();
					}
					DialogueScript.Close();
				}
			}
			else
			{
				this.textAnimatorPlayer.textAnimator.ShowAllCharacters(true);
			}
		}
		if (this.autoCloseTimer >= 0f && DialogueScript.IsEnabled())
		{
			this.autoCloseTimer -= Tick.Time;
			if (this.autoCloseTimer <= 0f)
			{
				DialogueScript.Close();
			}
		}
		Vector2 vector = this.backImageStartingLocalPos + new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		if (Data.settings.dyslexicFontEnabled)
		{
			vector = this.backImageStartingLocalPos;
		}
		this.backImageRectTransform.anchoredPosition = vector;
	}

	public static DialogueScript instance = null;

	public const int playerIndex = 0;

	public const float QUESTION_DELAY = 0.5f;

	public const float SELECTED_ALPHA = 1f;

	public const float UNSELECTED_ALPHA = 0.25f;

	private Controls.PlayerExt player;

	public CanvasScaler canvasScaler;

	public GameObject holder;

	public TextAnimatorPlayer textAnimatorPlayer;

	public TextAnimatorPlayer answerPromptsPlayer_Yes;

	public TextAnimatorPlayer answerPromptsPlayer_No;

	public RectTransform backImageRectTransform;

	public Transform soundOriginTransform;

	public static List<string> dialogueKeys = new List<string>();

	private string currentDialogueString_Translated;

	private int dialogueIndex = -1;

	private float allDialoguesInputDelay;

	private float questionDelay;

	private DialogueScript.QuestionAnswer questionAnswer;

	private DialogueScript.QuestionAnswer questionAnswerOld;

	private bool? question_CursorWasVisibleBeforeQuestion;

	private float autoCloseTimer = -1f;

	private float autoProgressTimer = -1f;

	private float autoProgressTimerLastValue = -1f;

	private bool legalDuringDeathCooldown;

	private Vector2 backImageStartingLocalPos;

	public DialogueScript.AnswerCallback onYes;

	public DialogueScript.AnswerCallback onNo;

	public DialogueScript.AnswerCallback onDialogueNext;

	public DialogueScript.AnswerCallback onDialogueClose;

	private enum QuestionAnswer
	{
		undefined,
		yes,
		no
	}

	// (Invoke) Token: 0x060011D5 RID: 4565
	public delegate void AnswerCallback();
}
