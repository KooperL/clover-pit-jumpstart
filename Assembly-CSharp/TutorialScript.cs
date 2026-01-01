using System;
using System.Collections;
using Panik;
using UnityEngine;

// Token: 0x020000FC RID: 252
public class TutorialScript : MonoBehaviour
{
	// Token: 0x06000C44 RID: 3140 RVA: 0x00010168 File Offset: 0x0000E368
	public static bool IsEnabled()
	{
		return !(TutorialScript.instance == null) && TutorialScript.instance.holder.activeSelf;
	}

	// Token: 0x06000C45 RID: 3141 RVA: 0x00061BC0 File Offset: 0x0005FDC0
	public static void StartTutorial()
	{
		if (TutorialScript.instance.tutorialCoroutine != null)
		{
			Debug.LogError("Somehow the tutorial coroutine was already started!! this should not be possible");
			TutorialScript.instance.StopCoroutine(TutorialScript.instance.tutorialCoroutine);
		}
		TutorialScript.instance.tutorialCoroutine = TutorialScript.instance.StartCoroutine(TutorialScript.instance.TutorialCoroutine());
	}

	// Token: 0x06000C46 RID: 3142 RVA: 0x00010188 File Offset: 0x0000E388
	private IEnumerator TutorialCoroutine()
	{
		this.holder.SetActive(true);
		float timer = 0f;
		GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.cutscene, false, null);
		while (DialogueScript.IsEnabled())
		{
			yield return null;
		}
		DialogueScript.SetDialogue(false, new string[] { "DIALUGUE_TUTORIAL_INTRO_0" });
		while (DialogueScript.IsEnabled())
		{
			yield return null;
		}
		timer = 0.5f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		CameraController.SetPosition(CameraController.PositionKind.ATM, false, 1f);
		DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_TUTORIAL_ATM_0", "DIALOGUE_TUTORIAL_ATM_1", "DIALOGUE_TUTORIAL_ATM_2", "DIALOGUE_TUTORIAL_ATM_3" });
		DialogueScript dialogueScript = DialogueScript.instance;
		dialogueScript.onDialogueNext = (DialogueScript.AnswerCallback)Delegate.Combine(dialogueScript.onDialogueNext, new DialogueScript.AnswerCallback(delegate
		{
			if (DialogueScript.GetDialogueIndex() == 3)
			{
				CameraController.SetPosition(CameraController.PositionKind.CloverTicketsMachine, false, 1f);
			}
		}));
		while (DialogueScript.IsEnabled())
		{
			yield return null;
		}
		timer = 0.5f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		CameraController.SetPosition(CameraController.PositionKind.Slot_Fixed, false, 1f);
		DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_TUTORIAL_SLOT_MACHINE_0", "DIALOGUE_TUTORIAL_SLOT_MACHINE_1" });
		while (DialogueScript.IsEnabled())
		{
			yield return null;
		}
		timer = 0.5f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		CameraController.SetPosition(CameraController.PositionKind.Store, false, 1f);
		DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_TUTORIAL_STORE_0", "DIALOGUE_TUTORIAL_STORE_1" });
		while (DialogueScript.IsEnabled())
		{
			yield return null;
		}
		timer = 0.5f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		CameraController.SetPosition(CameraController.PositionKind.Free, false, 1f);
		DialogueScript.SetDialogue(false, new string[] { "DIALOGUE_TUTORIAL_FINALIZE_0" });
		GameplayMaster.SetGamePhase(GameplayMaster.GamePhase.cutscene, false, null);
		CameraController.ResetFreeCameraAtSlot(true);
		this.holder.SetActive(false);
		this.tutorialCoroutine = null;
		yield break;
	}

	// Token: 0x06000C47 RID: 3143 RVA: 0x00010197 File Offset: 0x0000E397
	private void Awake()
	{
		TutorialScript.instance = this;
	}

	// Token: 0x06000C48 RID: 3144 RVA: 0x0001019F File Offset: 0x0000E39F
	private void Start()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x06000C49 RID: 3145 RVA: 0x000101AD File Offset: 0x0000E3AD
	private void OnDestroy()
	{
		if (TutorialScript.instance == this)
		{
			TutorialScript.instance = null;
		}
	}

	// Token: 0x04000D19 RID: 3353
	public static TutorialScript instance;

	// Token: 0x04000D1A RID: 3354
	public GameObject holder;

	// Token: 0x04000D1B RID: 3355
	private Coroutine tutorialCoroutine;
}
