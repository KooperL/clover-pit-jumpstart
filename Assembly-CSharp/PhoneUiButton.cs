using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class PhoneUiButton : MonoBehaviour
{
	// Token: 0x0600097C RID: 2428 RVA: 0x0003E6C2 File Offset: 0x0003C8C2
	public bool IsHovered()
	{
		return this.mouseOver;
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x0003E6CC File Offset: 0x0003C8CC
	private bool MouseIsOver()
	{
		Vector2 vector;
		vector.x = (float)CameraUiGlobal.instance.myCamera.pixelWidth;
		vector.y = (float)CameraUiGlobal.instance.myCamera.pixelHeight;
		Vector2 vector2 = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, vector) + new Vector2(vector.x / 2f, vector.y / 2f);
		return RectTransformUtility.RectangleContainsScreenPoint(this.myRectTransform, vector2, CameraUiGlobal.instance.myCamera);
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x0003E74C File Offset: 0x0003C94C
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

	// Token: 0x0600097F RID: 2431 RVA: 0x0003E7B5 File Offset: 0x0003C9B5
	public void HighlightOff()
	{
		this.highlighted = false;
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x0003E7C0 File Offset: 0x0003C9C0
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

	// Token: 0x06000981 RID: 2433 RVA: 0x0003E849 File Offset: 0x0003CA49
	private void Awake()
	{
		PhoneUiButton.allButtons.Add(this);
		this.myRectTransform = base.GetComponent<RectTransform>();
		if (this.buttonIndex == -1)
		{
			Debug.LogError("Button index not set!");
		}
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x0003E875 File Offset: 0x0003CA75
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x0003E883 File Offset: 0x0003CA83
	private void OnDestroy()
	{
		PhoneUiButton.allButtons.Remove(this);
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x0003E894 File Offset: 0x0003CA94
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
