using System;
using System.Collections;
using Febucci.UI;
using Panik;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToyPhoneUIScript : MonoBehaviour
{
	// Token: 0x06000A69 RID: 2665 RVA: 0x00047511 File Offset: 0x00045711
	public static bool IsEnabled()
	{
		return !(ToyPhoneUIScript.instance == null) && ToyPhoneUIScript.instance.holder.activeSelf;
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x00047534 File Offset: 0x00045734
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

	// Token: 0x06000A6B RID: 2667 RVA: 0x00047591 File Offset: 0x00045791
	public void HangUp()
	{
		this.holder.SetActive(false);
		if (this.oldCameraPositionKind != CameraController.PositionKind.Undefined)
		{
			CameraController.SetPosition(this.oldCameraPositionKind, false, 0f);
		}
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x000475BA File Offset: 0x000457BA
	private IEnumerator MainCoroutine()
	{
		this.requestedExit = false;
		this.pageIndex = 0;
		int count = GameplayData.Instance.phoneAbilitiesPickHistory.Count;
		this.pagesNumberChached = Mathf.CeilToInt((float)count / 3f);
		this.cursorPreviousState = VirtualCursors.CursorDesiredVisibilityGet(0);
		VirtualCursors.CursorDesiredVisibilitySet(0, true);
		this.visualsAndButtonnsHolder.SetActive(false);
		Sound.Play3D("SoundToyPhonePickup", ToyPhoneScript.instance.transform.position, 10f, 1f, 1f, 1);
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
							Sound.Play3D_Unpausable("SoundToyPhoneSelect", ToyPhoneScript.instance.transform.position, 10f, 1f, 1f, 1);
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
						Sound.Play3D_Unpausable("SoundToyPhoneSelect", ToyPhoneScript.instance.transform.position, 10f, 1f, 1f, 1);
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
		Sound.Play3D("SoundToyPhoneHangUp", ToyPhoneScript.instance.transform.position, 10f, 1f, 1f, 1);
		GameplayMaster.instance.FCAll_ToyPhone_Hangup();
		VirtualCursors.CursorDesiredVisibilitySet(0, this.cursorPreviousState);
		this._forceClose_Death = false;
		this.mainCoroutine = null;
		yield break;
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x000475C9 File Offset: 0x000457C9
	public static int Pages_GetIndex()
	{
		return ToyPhoneUIScript.instance.pageIndex;
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x000475D5 File Offset: 0x000457D5
	public static int Pages_GetMax()
	{
		return ToyPhoneUIScript.instance.pagesNumberChached;
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x000475E4 File Offset: 0x000457E4
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

	// Token: 0x06000A70 RID: 2672 RVA: 0x0004778C File Offset: 0x0004598C
	public void ButtonRequest_Exit()
	{
		this.requestedExit = true;
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x00047795 File Offset: 0x00045995
	public void ButtonRequest_PageNext()
	{
		this.pageIndex++;
		if (this.pageIndex >= this.pagesNumberChached)
		{
			this.pageIndex = this.pagesNumberChached - 1;
		}
		this.PageApply();
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x000477C7 File Offset: 0x000459C7
	public void ButtonRequest_PagePrevious()
	{
		this.pageIndex--;
		if (this.pageIndex < 0)
		{
			this.pageIndex = 0;
		}
		this.PageApply();
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x000477ED File Offset: 0x000459ED
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

	// Token: 0x06000A74 RID: 2676 RVA: 0x00047815 File Offset: 0x00045A15
	public static bool IsForceClosing()
	{
		return !(ToyPhoneUIScript.instance == null) && ToyPhoneUIScript.instance._forceClose_Death;
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x00047830 File Offset: 0x00045A30
	private void Awake()
	{
		ToyPhoneUIScript.instance = this;
		this.holder.SetActive(false);
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x00047844 File Offset: 0x00045A44
	private void OnDestroy()
	{
		if (ToyPhoneUIScript.instance == this)
		{
			ToyPhoneUIScript.instance = null;
		}
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x0004785C File Offset: 0x00045A5C
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		this.imagesToShake_StartingPositions = new Vector2[this.imagesToShake.Length];
		for (int i = 0; i < this.imagesToShake.Length; i++)
		{
			this.imagesToShake_StartingPositions[i] = this.imagesToShake[i].rectTransform.anchoredPosition;
		}
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x000478BC File Offset: 0x00045ABC
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

	public static ToyPhoneUIScript instance;

	public const int PLAYER_INDEX = 0;

	private Controls.PlayerExt player;

	public GameObject holder;

	public GameObject visualsAndButtonnsHolder;

	public Image[] imagesToShake;

	private Vector2[] imagesToShake_StartingPositions;

	public TextAnimatorPlayer dialogueTextAnimator;

	public ToyPhoneUIAbilityDisplay[] abilityDisplays;

	public ToyPhoneUIButtonScript backButton;

	public ToyPhoneUIButtonScript nextButton;

	public ToyPhoneUIButtonScript previousButton;

	private CameraController.PositionKind oldCameraPositionKind = CameraController.PositionKind.Undefined;

	private Coroutine mainCoroutine;

	private bool cursorPreviousState;

	private int pageIndex;

	private int pagesNumberChached = -1;

	private bool requestedExit;

	private bool _forceClose_Death;
}
