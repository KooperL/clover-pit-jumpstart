using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020000F9 RID: 249
public class ToyPhoneUIButtonScript : MonoBehaviour
{
	// Token: 0x06000C1D RID: 3101 RVA: 0x000612B0 File Offset: 0x0005F4B0
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

	// Token: 0x06000C1E RID: 3102 RVA: 0x0000FF08 File Offset: 0x0000E108
	public void HighlightOff()
	{
		this.highlighted = false;
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x0000FF11 File Offset: 0x0000E111
	public bool IsHighlighted()
	{
		return this.highlighted;
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x0000FF19 File Offset: 0x0000E119
	public bool IsHovered()
	{
		return this.mouseOver;
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x0006131C File Offset: 0x0005F51C
	private bool MouseIsOver()
	{
		Vector2 vector;
		vector.x = (float)CameraUiGlobal.instance.myCamera.pixelWidth;
		vector.y = (float)CameraUiGlobal.instance.myCamera.pixelHeight;
		Vector2 vector2 = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, vector) + new Vector2(vector.x / 2f, vector.y / 2f);
		return RectTransformUtility.RectangleContainsScreenPoint(this.myRectTransform, vector2, CameraUiGlobal.instance.myCamera);
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x0000FF21 File Offset: 0x0000E121
	public void SetText(string text)
	{
		this.myText.text = text;
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x0006139C File Offset: 0x0005F59C
	public ToyPhoneUIButtonScript GetButtonLeft()
	{
		if (this.leftButton == null)
		{
			return null;
		}
		if (this.leftButton.gameObject.activeInHierarchy && this.leftButton.IsEnabled())
		{
			return this.leftButton;
		}
		return this.leftButton.GetButtonLeft();
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x000613EC File Offset: 0x0005F5EC
	public ToyPhoneUIButtonScript GetButtonRight()
	{
		if (this.rightButton == null)
		{
			return null;
		}
		if (this.rightButton.gameObject.activeInHierarchy && this.rightButton.IsEnabled())
		{
			return this.rightButton;
		}
		return this.rightButton.GetButtonRight();
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x0006143C File Offset: 0x0005F63C
	public void SetEnabled(bool enabled)
	{
		this.isEnabled = enabled;
		if (!this.isEnabled)
		{
			this.myText.color = new Color(0.25f, 0.25f, 0.25f, 1f);
		}
		else
		{
			this.myText.color = new Color(1f, 1f, 1f, 1f);
		}
		if (!this.isEnabled)
		{
			this.mouseOver = false;
			this.HighlightOff();
		}
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x0000FF2F File Offset: 0x0000E12F
	public bool IsEnabled()
	{
		return this.isEnabled;
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x0000FF37 File Offset: 0x0000E137
	private void Awake()
	{
		ToyPhoneUIButtonScript.allButtons.Add(this);
		this.myRectTransform = base.GetComponent<RectTransform>();
		this.myText = base.GetComponentInChildren<TextMeshProUGUI>();
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x0000FF5C File Offset: 0x0000E15C
	private void OnDestroy()
	{
		ToyPhoneUIButtonScript.allButtons.Remove(this);
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x0000FF6A File Offset: 0x0000E16A
	private void Start()
	{
		if (this.buttonIndex < 0)
		{
			Debug.LogError("Button index not set! Button game object: " + base.gameObject.name);
		}
		this.player = Controls.GetPlayerByIndex(0);
		this.HighlightOff();
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x0000FFA1 File Offset: 0x0000E1A1
	private void Update()
	{
		if (this.isEnabled)
		{
			this.mouseOver = this.MouseIsOver();
		}
		if (this.highlightImage.enabled != this.highlighted)
		{
			this.highlightImage.enabled = this.highlighted;
		}
	}

	// Token: 0x04000CF5 RID: 3317
	public static List<ToyPhoneUIButtonScript> allButtons = new List<ToyPhoneUIButtonScript>();

	// Token: 0x04000CF6 RID: 3318
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000CF7 RID: 3319
	private Controls.PlayerExt player;

	// Token: 0x04000CF8 RID: 3320
	private RectTransform myRectTransform;

	// Token: 0x04000CF9 RID: 3321
	public Image highlightImage;

	// Token: 0x04000CFA RID: 3322
	public AudioClip highlightSound;

	// Token: 0x04000CFB RID: 3323
	public ToyPhoneUIButtonScript leftButton;

	// Token: 0x04000CFC RID: 3324
	public ToyPhoneUIButtonScript rightButton;

	// Token: 0x04000CFD RID: 3325
	private TextMeshProUGUI myText;

	// Token: 0x04000CFE RID: 3326
	public int buttonIndex = -1;

	// Token: 0x04000CFF RID: 3327
	private bool highlighted;

	// Token: 0x04000D00 RID: 3328
	private bool mouseOver;

	// Token: 0x04000D01 RID: 3329
	private bool isEnabled = true;

	// Token: 0x04000D02 RID: 3330
	public UnityEvent onSelect;
}
