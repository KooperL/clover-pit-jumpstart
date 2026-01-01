using System;
using System.Collections;
using Febucci.UI;
using Panik;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000FA RID: 250
public class ToyPhoneUIScript : MonoBehaviour
{
	// Token: 0x06000C2D RID: 3117 RVA: 0x0000FFFD File Offset: 0x0000E1FD
	public static bool IsEnabled()
	{
		return !(ToyPhoneUIScript.instance == null) && ToyPhoneUIScript.instance.holder.activeSelf;
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x000614B8 File Offset: 0x0005F6B8
	public void PickUp()
	{
		this.oldCameraPositionKind = CameraController.GetPositionKind();
		CameraController.SetPosition(CameraController.PositionKind.ToyPhone, false, 2f);
		this.holder.SetActive(true);
		MemoScript.Close(false);
		if (this.mainCoroutine != null)
		{
			base.StopCoroutine(this.mainCoroutine);
		}
		this.mainCoroutine = base.StartCoroutine(this.MainCoroutine());
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x0001001D File Offset: 0x0000E21D
	public void HangUp()
	{
		this.holder.SetActive(false);
		if (this.oldCameraPositionKind != CameraController.PositionKind.Undefined)
		{
			CameraController.SetPosition(this.oldCameraPositionKind, false, 0f);
		}
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x00010046 File Offset: 0x0000E246
	private IEnumerator MainCoroutine()
	{
		this.requestedExit = false;
		this.pageIndex = 0;
		int count = GameplayData.Instance.phoneAbilitiesPickHistory.Count;
		this.pagesNumberChached = Mathf.CeilToInt((float)count / 3f);
		this.cursorPreviousState = VirtualCursors.CursorDesiredVisibilityGet(0);
		VirtualCursors.CursorDesiredVisibilitySet(0, true);
		this.visualsAndButtonnsHolder.SetActive(false);
		Sound.Play3D("SoundToyPhonePickup", ToyPhoneScript.instance.transform.position, 10f, 1f, 1f, AudioRolloffMode.Linear);
		while (!CameraController.IsCameraNearPositionAndAngle(0.1f) && !ToyPhoneUIScript.IsForceClosing())
		{
			yield return null;
		}
		this.PageApply();
		this.dialogueTextAnimator.textAnimator.tmproText.text = "";
		this.dialogueTextAnimator.ShowText("");
		this.dialogueTextAnimator.ShowText(Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("TOY_PHONE_EXPLANATION_1"), Strings.SanitizationSubKind.none));
		this.backButton.SetText(Translation.Get("TOY_PHONE_BUTTON_BACK"));
		this.nextButton.SetText(Translation.Get("TOY_PHONE_BUTTON_NEXT"));
		this.previousButton.SetText(Translation.Get("TOY_PHONE_BUTTON_PREVIOUS"));
		this.visualsAndButtonnsHolder.SetActive(true);
		if (!ToyPhoneUIScript.IsForceClosing())
		{
			yield return null;
		}
		while (!ToyPhoneUIScript.IsForceClosing())
		{
			bool flag = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuMoveLeft, true);
			bool flag2 = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuMoveRight, true);
			bool flag3 = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true);
			ToyPhoneUIButtonScript toyPhoneUIButtonScript = null;
			for (int i = 0; i < ToyPhoneUIButtonScript.allButtons.Count; i++)
			{
				if (ToyPhoneUIButtonScript.allButtons[i].IsHighlighted())
				{
					toyPhoneUIButtonScript = ToyPhoneUIButtonScript.allButtons[i];
					break;
				}
			}
			if (VirtualCursors.IsCursorVisible(0, true))
			{
				for (int j = 0; j < ToyPhoneUIButtonScript.allButtons.Count; j++)
				{
					ToyPhoneUIButtonScript toyPhoneUIButtonScript2 = ToyPhoneUIButtonScript.allButtons[j];
					if (!toyPhoneUIButtonScript2.IsHovered())
					{
						toyPhoneUIButtonScript2.HighlightOff();
						if (toyPhoneUIButtonScript == toyPhoneUIButtonScript2)
						{
							toyPhoneUIButtonScript = null;
						}
					}
					else
					{
						if (!toyPhoneUIButtonScript2.IsHighlighted())
						{
							toyPhoneUIButtonScript2.Highlight(true);
							toyPhoneUIButtonScript = toyPhoneUIButtonScript2;
						}
						if (flag3)
						{
							UnityEvent onSelect = toyPhoneUIButtonScript2.onSelect;
							if (onSelect != null)
							{
								onSelect.Invoke();
							}
							Sound.Play3D_Unpausable("SoundToyPhoneSelect", ToyPhoneScript.instance.transform.position, 10f, 1f, 1f, AudioRolloffMode.Linear);
						}
					}
				}
			}
			else if (toyPhoneUIButtonScript == null && (flag || flag2 || flag3))
			{
				this.backButton.Highlight(false);
				toyPhoneUIButtonScript = this.backButton;
			}
			else
			{
				ToyPhoneUIButtonScript toyPhoneUIButtonScript3 = ((toyPhoneUIButtonScript != null) ? toyPhoneUIButtonScript.GetButtonLeft() : null);
				ToyPhoneUIButtonScript toyPhoneUIButtonScript4 = ((toyPhoneUIButtonScript != null) ? toyPhoneUIButtonScript.GetButtonRight() : null);
				if (flag && toyPhoneUIButtonScript3 != null)
				{
					toyPhoneUIButtonScript.HighlightOff();
					toyPhoneUIButtonScript3.Highlight(true);
					toyPhoneUIButtonScript = toyPhoneUIButtonScript3;
				}
				else if (flag2 && toyPhoneUIButtonScript4 != null)
				{
					toyPhoneUIButtonScript.HighlightOff();
					toyPhoneUIButtonScript4.Highlight(true);
					toyPhoneUIButtonScript = toyPhoneUIButtonScript4;
				}
				if (flag3 && toyPhoneUIButtonScript != null)
				{
					UnityEvent onSelect2 = toyPhoneUIButtonScript.onSelect;
					if (onSelect2 != null)
					{
						onSelect2.Invoke();
					}
					if (toyPhoneUIButtonScript != this.backButton)
					{
						Sound.Play3D_Unpausable("SoundToyPhoneSelect", ToyPhoneScript.instance.transform.position, 10f, 1f, 1f, AudioRolloffMode.Linear);
					}
				}
			}
			if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true))
			{
				this.requestedExit = true;
			}
			if (this.requestedExit)
			{
				break;
			}
			yield return null;
		}
		Sound.Play3D("SoundToyPhoneHangUp", ToyPhoneScript.instance.transform.position, 10f, 1f, 1f, AudioRolloffMode.Linear);
		GameplayMaster.instance.FCAll_ToyPhone_Hangup();
		VirtualCursors.CursorDesiredVisibilitySet(0, this.cursorPreviousState);
		this._forceClose_Death = false;
		this.mainCoroutine = null;
		yield break;
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x00010055 File Offset: 0x0000E255
	public static int Pages_GetIndex()
	{
		return ToyPhoneUIScript.instance.pageIndex;
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x00010061 File Offset: 0x0000E261
	public static int Pages_GetMax()
	{
		return ToyPhoneUIScript.instance.pagesNumberChached;
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x00061518 File Offset: 0x0005F718
	private void PageApply()
	{
		int count = GameplayData.Instance.phoneAbilitiesPickHistory.Count;
		for (int i = 0; i < 3; i++)
		{
			int num = this.pageIndex * 3 + i;
			if (num >= count)
			{
				if (this.abilityDisplays[i].gameObject.activeSelf)
				{
					this.abilityDisplays[i].gameObject.SetActive(false);
				}
			}
			else
			{
				if (!this.abilityDisplays[i].gameObject.activeSelf)
				{
					this.abilityDisplays[i].gameObject.SetActive(true);
				}
				this.abilityDisplays[i].AbilitySet(GameplayData.Instance.phoneAbilitiesPickHistory[num], num);
			}
		}
		if (this.pageIndex == 0)
		{
			if (!VirtualCursors.IsCursorVisible(0, true) && this.previousButton.IsHighlighted())
			{
				this.nextButton.Highlight(false);
				this.previousButton.HighlightOff();
			}
			this.previousButton.SetEnabled(false);
			if (this.pagesNumberChached > 1)
			{
				this.nextButton.SetEnabled(true);
			}
			else
			{
				this.nextButton.SetEnabled(false);
			}
		}
		else if (this.pageIndex == this.pagesNumberChached - 1)
		{
			if (!VirtualCursors.IsCursorVisible(0, true) && this.nextButton.IsHighlighted())
			{
				this.previousButton.Highlight(false);
				this.nextButton.HighlightOff();
			}
			if (this.pagesNumberChached > 1)
			{
				this.previousButton.SetEnabled(true);
			}
			else
			{
				this.previousButton.SetEnabled(false);
			}
			this.nextButton.SetEnabled(false);
		}
		else
		{
			this.previousButton.SetEnabled(true);
			this.nextButton.SetEnabled(true);
		}
		ToyPhoneScript.instance.UpdateLabelText();
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x0001006D File Offset: 0x0000E26D
	public void ButtonRequest_Exit()
	{
		this.requestedExit = true;
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x00010076 File Offset: 0x0000E276
	public void ButtonRequest_PageNext()
	{
		this.pageIndex++;
		if (this.pageIndex >= this.pagesNumberChached)
		{
			this.pageIndex = this.pagesNumberChached - 1;
		}
		this.PageApply();
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x000100A8 File Offset: 0x0000E2A8
	public void ButtonRequest_PagePrevious()
	{
		this.pageIndex--;
		if (this.pageIndex < 0)
		{
			this.pageIndex = 0;
		}
		this.PageApply();
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x000100CE File Offset: 0x0000E2CE
	public static void ForceClose_Death()
	{
		if (ToyPhoneUIScript.instance == null)
		{
			return;
		}
		if (ToyPhoneUIScript.instance.mainCoroutine == null)
		{
			return;
		}
		ToyPhoneUIScript.instance._forceClose_Death = true;
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x000100F6 File Offset: 0x0000E2F6
	public static bool IsForceClosing()
	{
		return !(ToyPhoneUIScript.instance == null) && ToyPhoneUIScript.instance._forceClose_Death;
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x00010111 File Offset: 0x0000E311
	private void Awake()
	{
		ToyPhoneUIScript.instance = this;
		this.holder.SetActive(false);
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x00010125 File Offset: 0x0000E325
	private void OnDestroy()
	{
		if (ToyPhoneUIScript.instance == this)
		{
			ToyPhoneUIScript.instance = null;
		}
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x000616C0 File Offset: 0x0005F8C0
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		this.imagesToShake_StartingPositions = new Vector2[this.imagesToShake.Length];
		for (int i = 0; i < this.imagesToShake.Length; i++)
		{
			this.imagesToShake_StartingPositions[i] = this.imagesToShake[i].rectTransform.anchoredPosition;
		}
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x00061720 File Offset: 0x0005F920
	private void Update()
	{
		for (int i = 0; i < this.imagesToShake.Length; i++)
		{
			Vector2 vector = this.imagesToShake_StartingPositions[i] + new Vector2(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f));
			if (Data.settings.dyslexicFontEnabled)
			{
				vector = this.imagesToShake_StartingPositions[i];
			}
			this.imagesToShake[i].rectTransform.anchoredPosition = vector;
		}
	}

	// Token: 0x04000D03 RID: 3331
	public static ToyPhoneUIScript instance;

	// Token: 0x04000D04 RID: 3332
	public const int PLAYER_INDEX = 0;

	// Token: 0x04000D05 RID: 3333
	private Controls.PlayerExt player;

	// Token: 0x04000D06 RID: 3334
	public GameObject holder;

	// Token: 0x04000D07 RID: 3335
	public GameObject visualsAndButtonnsHolder;

	// Token: 0x04000D08 RID: 3336
	public Image[] imagesToShake;

	// Token: 0x04000D09 RID: 3337
	private Vector2[] imagesToShake_StartingPositions;

	// Token: 0x04000D0A RID: 3338
	public TextAnimatorPlayer dialogueTextAnimator;

	// Token: 0x04000D0B RID: 3339
	public ToyPhoneUIAbilityDisplay[] abilityDisplays;

	// Token: 0x04000D0C RID: 3340
	public ToyPhoneUIButtonScript backButton;

	// Token: 0x04000D0D RID: 3341
	public ToyPhoneUIButtonScript nextButton;

	// Token: 0x04000D0E RID: 3342
	public ToyPhoneUIButtonScript previousButton;

	// Token: 0x04000D0F RID: 3343
	private CameraController.PositionKind oldCameraPositionKind = CameraController.PositionKind.Undefined;

	// Token: 0x04000D10 RID: 3344
	private Coroutine mainCoroutine;

	// Token: 0x04000D11 RID: 3345
	private bool cursorPreviousState;

	// Token: 0x04000D12 RID: 3346
	private int pageIndex;

	// Token: 0x04000D13 RID: 3347
	private int pagesNumberChached = -1;

	// Token: 0x04000D14 RID: 3348
	private bool requestedExit;

	// Token: 0x04000D15 RID: 3349
	private bool _forceClose_Death;
}
