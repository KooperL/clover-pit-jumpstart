using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class PhoneUiButton : MonoBehaviour
{
	// Token: 0x06000990 RID: 2448 RVA: 0x0003ED26 File Offset: 0x0003CF26
	public bool IsHovered()
	{
		return this.mouseOver;
	}

	// Token: 0x06000991 RID: 2449 RVA: 0x0003ED30 File Offset: 0x0003CF30
	private bool MouseIsOver()
	{
		Vector2 vector;
		vector.x = (float)CameraUiGlobal.instance.myCamera.pixelWidth;
		vector.y = (float)CameraUiGlobal.instance.myCamera.pixelHeight;
		Vector2 vector2 = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, vector) + new Vector2(vector.x / 2f, vector.y / 2f);
		return RectTransformUtility.RectangleContainsScreenPoint(this.myRectTransform, vector2, CameraUiGlobal.instance.myCamera);
	}

	// Token: 0x06000992 RID: 2450 RVA: 0x0003EDB0 File Offset: 0x0003CFB0
	public void Highlight(bool useSoundAndVibration)
	{
		if (!this.highlighted && base.gameObject.activeInHierarchy && useSoundAndVibration)
		{
			Controls.VibrationSet_PreferMax(this.player, 0.1f);
		}
		this.highlighted = true;
		if (this.highlightSound != null && useSoundAndVibration)
		{
			Sound.Play_Unpausable(this.highlightSound.name, 1f, 1f);
		}
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x0003EE19 File Offset: 0x0003D019
	public void HighlightOff()
	{
		this.highlighted = false;
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x0003EE24 File Offset: 0x0003D024
	public static void InitializeAll()
	{
		for (int i = 0; i < PhoneUiButton.allButtons.Count; i++)
		{
			for (int j = i + 1; j < PhoneUiButton.allButtons.Count; j++)
			{
				if (PhoneUiButton.allButtons[i].buttonIndex > PhoneUiButton.allButtons[j].buttonIndex)
				{
					PhoneUiButton phoneUiButton = PhoneUiButton.allButtons[i];
					PhoneUiButton.allButtons[i] = PhoneUiButton.allButtons[j];
					PhoneUiButton.allButtons[j] = phoneUiButton;
				}
			}
		}
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x0003EEAD File Offset: 0x0003D0AD
	private void Awake()
	{
		PhoneUiButton.allButtons.Add(this);
		this.myRectTransform = base.GetComponent<RectTransform>();
		if (this.buttonIndex == -1)
		{
			Debug.LogError("Button index not set!");
		}
	}

	// Token: 0x06000996 RID: 2454 RVA: 0x0003EED9 File Offset: 0x0003D0D9
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
	}

	// Token: 0x06000997 RID: 2455 RVA: 0x0003EEE7 File Offset: 0x0003D0E7
	private void OnDestroy()
	{
		PhoneUiButton.allButtons.Remove(this);
	}

	// Token: 0x06000998 RID: 2456 RVA: 0x0003EEF8 File Offset: 0x0003D0F8
	private void Update()
	{
		this.mouseOver = this.MouseIsOver();
		if (this.highlightImage.enabled != this.highlighted)
		{
			this.highlightImage.enabled = this.highlighted;
		}
		Vector2 zero = new Vector2(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f));
		if (Data.settings.dyslexicFontEnabled)
		{
			zero = Vector2.zero;
		}
		this.vibrateImage.rectTransform.anchoredPosition = zero;
	}

	public static List<PhoneUiButton> allButtons = new List<PhoneUiButton>();

	private const int PLAYER_INDEX = 0;

	private const int ABILITY_INDEX_OFFSET = 2;

	private Controls.PlayerExt player;

	public Image highlightImage;

	public Image vibrateImage;

	private RectTransform myRectTransform;

	public AudioClip highlightSound;

	public PhoneUiButton leftButton;

	public PhoneUiButton rightButton;

	public PhoneUiButton upButton;

	public PhoneUiButton downButton;

	public int buttonIndex = -1;

	private bool mouseOver;

	private bool highlighted;
}
