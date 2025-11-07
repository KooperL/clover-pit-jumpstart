using System;
using System.Collections;
using Panik;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
	// Token: 0x06000A65 RID: 2661 RVA: 0x000471F4 File Offset: 0x000453F4
	public static bool IsEnabled()
	{
		return !(TutorialScript.instance == null) && TutorialScript.instance.holder.activeSelf;
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x00047214 File Offset: 0x00045414
	public static void StartTutorial()
	{
		if (TutorialScript.instance.tutorialCoroutine != null)
		{
			Debug.LogError("Somehow the tutorial coroutine was already started!! this should not be possible");
			TutorialScript.instance.StopCoroutine(TutorialScript.instance.tutorialCoroutine);
		}
		TutorialScript.instance.tutorialCoroutine = TutorialScript.instance.StartCoroutine(TutorialScript.instance.TutorialCoroutine());
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x00047269 File Offset: 0x00045469
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

	// Token: 0x06000A68 RID: 2664 RVA: 0x00047278 File Offset: 0x00045478
	private void Awake()
	{
		TutorialScript.instance = this;
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x00047280 File Offset: 0x00045480
	private void Start()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x0004728E File Offset: 0x0004548E
	private void OnDestroy()
	{
		if (TutorialScript.instance == this)
		{
			TutorialScript.instance = null;
		}
	}

	public static TutorialScript instance;

	public GameObject holder;

	private Coroutine tutorialCoroutine;
}
