using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToyPhoneUIButtonScript : MonoBehaviour
{
	// Token: 0x06000A44 RID: 2628 RVA: 0x00046AB4 File Offset: 0x00044CB4
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

	// Token: 0x06000A45 RID: 2629 RVA: 0x00046B1D File Offset: 0x00044D1D
	public void HighlightOff()
	{
		this.highlighted = false;
	}

	// Token: 0x06000A46 RID: 2630 RVA: 0x00046B26 File Offset: 0x00044D26
	public bool IsHighlighted()
	{
		return this.highlighted;
	}

	// Token: 0x06000A47 RID: 2631 RVA: 0x00046B2E File Offset: 0x00044D2E
	public bool IsHovered()
	{
		return this.mouseOver;
	}

	// Token: 0x06000A48 RID: 2632 RVA: 0x00046B38 File Offset: 0x00044D38
	private bool MouseIsOver()
	{
		Vector2 vector;
		vector.x = (float)CameraUiGlobal.instance.myCamera.pixelWidth;
		vector.y = (float)CameraUiGlobal.instance.myCamera.pixelHeight;
		Vector2 vector2 = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, vector) + new Vector2(vector.x / 2f, vector.y / 2f);
		return RectTransformUtility.RectangleContainsScreenPoint(this.myRectTransform, vector2, CameraUiGlobal.instance.myCamera);
	}

	// Token: 0x06000A49 RID: 2633 RVA: 0x00046BB8 File Offset: 0x00044DB8
	public void SetText(string text)
	{
		this.myText.text = text;
	}

	// Token: 0x06000A4A RID: 2634 RVA: 0x00046BC8 File Offset: 0x00044DC8
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

	// Token: 0x06000A4B RID: 2635 RVA: 0x00046C18 File Offset: 0x00044E18
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

	// Token: 0x06000A4C RID: 2636 RVA: 0x00046C68 File Offset: 0x00044E68
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

	// Token: 0x06000A4D RID: 2637 RVA: 0x00046CE3 File Offset: 0x00044EE3
	public bool IsEnabled()
	{
		return this.isEnabled;
	}

	// Token: 0x06000A4E RID: 2638 RVA: 0x00046CEB File Offset: 0x00044EEB
	private void Awake()
	{
		ToyPhoneUIButtonScript.allButtons.Add(this);
		this.myRectTransform = base.GetComponent<RectTransform>();
		this.myText = base.GetComponentInChildren<TextMeshProUGUI>();
	}

	// Token: 0x06000A4F RID: 2639 RVA: 0x00046D10 File Offset: 0x00044F10
	private void OnDestroy()
	{
		ToyPhoneUIButtonScript.allButtons.Remove(this);
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x00046D1E File Offset: 0x00044F1E
	private void Start()
	{
		if (this.buttonIndex < 0)
		{
			Debug.LogError("Button index not set! Button game object: " + base.gameObject.name);
		}
		this.player = Controls.GetPlayerByIndex(0);
		this.HighlightOff();
	}

	// Token: 0x06000A51 RID: 2641 RVA: 0x00046D55 File Offset: 0x00044F55
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

	public static List<ToyPhoneUIButtonScript> allButtons = new List<ToyPhoneUIButtonScript>();

	private const int PLAYER_INDEX = 0;

	private Controls.PlayerExt player;

	private RectTransform myRectTransform;

	public Image highlightImage;

	public AudioClip highlightSound;

	public ToyPhoneUIButtonScript leftButton;

	public ToyPhoneUIButtonScript rightButton;

	private TextMeshProUGUI myText;

	public int buttonIndex = -1;

	private bool highlighted;

	private bool mouseOver;

	private bool isEnabled = true;

	public UnityEvent onSelect;
}
