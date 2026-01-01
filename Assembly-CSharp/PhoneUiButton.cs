using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D4 RID: 212
public class PhoneUiButton : MonoBehaviour
{
	// Token: 0x06000B02 RID: 2818 RVA: 0x0000EC17 File Offset: 0x0000CE17
	public bool IsHovered()
	{
		return this.mouseOver;
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x00058450 File Offset: 0x00056650
	private bool MouseIsOver()
	{
		Vector2 vector;
		vector.x = (float)CameraUiGlobal.instance.myCamera.pixelWidth;
		vector.y = (float)CameraUiGlobal.instance.myCamera.pixelHeight;
		Vector2 vector2 = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, vector) + new Vector2(vector.x / 2f, vector.y / 2f);
		return RectTransformUtility.RectangleContainsScreenPoint(this.myRectTransform, vector2, CameraUiGlobal.instance.myCamera);
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x000584D0 File Offset: 0x000566D0
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

	// Token: 0x06000B05 RID: 2821 RVA: 0x0000EC1F File Offset: 0x0000CE1F
	public void HighlightOff()
	{
		this.highlighted = false;
	}

	// Token: 0x06000B06 RID: 2822 RVA: 0x0005853C File Offset: 0x0005673C
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

	// Token: 0x06000B07 RID: 2823 RVA: 0x0000EC28 File Offset: 0x0000CE28
	private void Awake()
	{
		PhoneUiButton.allButtons.Add(this);
		this.myRectTransform = base.GetComponent<RectTransform>();
		if (this.buttonIndex == -1)
		{
			Debug.LogError("Button index not set!");
		}
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0000EC54 File Offset: 0x0000CE54
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x0000EC62 File Offset: 0x0000CE62
	private void OnDestroy()
	{
		PhoneUiButton.allButtons.Remove(this);
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x000585C8 File Offset: 0x000567C8
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

	// Token: 0x04000B55 RID: 2901
	public static List<PhoneUiButton> allButtons = new List<PhoneUiButton>();

	// Token: 0x04000B56 RID: 2902
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000B57 RID: 2903
	private const int ABILITY_INDEX_OFFSET = 2;

	// Token: 0x04000B58 RID: 2904
	private Controls.PlayerExt player;

	// Token: 0x04000B59 RID: 2905
	public Image highlightImage;

	// Token: 0x04000B5A RID: 2906
	public Image vibrateImage;

	// Token: 0x04000B5B RID: 2907
	private RectTransform myRectTransform;

	// Token: 0x04000B5C RID: 2908
	public AudioClip highlightSound;

	// Token: 0x04000B5D RID: 2909
	public PhoneUiButton leftButton;

	// Token: 0x04000B5E RID: 2910
	public PhoneUiButton rightButton;

	// Token: 0x04000B5F RID: 2911
	public PhoneUiButton upButton;

	// Token: 0x04000B60 RID: 2912
	public PhoneUiButton downButton;

	// Token: 0x04000B61 RID: 2913
	public int buttonIndex = -1;

	// Token: 0x04000B62 RID: 2914
	private bool mouseOver;

	// Token: 0x04000B63 RID: 2915
	private bool highlighted;
}
