using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToyPhoneUIButtonScript : MonoBehaviour
{
	// Token: 0x06000A59 RID: 2649 RVA: 0x00047214 File Offset: 0x00045414
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

	// Token: 0x06000A5A RID: 2650 RVA: 0x0004727D File Offset: 0x0004547D
	public void HighlightOff()
	{
		this.highlighted = false;
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x00047286 File Offset: 0x00045486
	public bool IsHighlighted()
	{
		return this.highlighted;
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x0004728E File Offset: 0x0004548E
	public bool IsHovered()
	{
		return this.mouseOver;
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x00047298 File Offset: 0x00045498
	private bool MouseIsOver()
	{
		Vector2 vector;
		vector.x = (float)CameraUiGlobal.instance.myCamera.pixelWidth;
		vector.y = (float)CameraUiGlobal.instance.myCamera.pixelHeight;
		Vector2 vector2 = VirtualCursors.CursorPositionCenteredGet_ReferenceResolution(0, vector) + new Vector2(vector.x / 2f, vector.y / 2f);
		return RectTransformUtility.RectangleContainsScreenPoint(this.myRectTransform, vector2, CameraUiGlobal.instance.myCamera);
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x00047318 File Offset: 0x00045518
	public void SetText(string text)
	{
		this.myText.text = text;
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x00047328 File Offset: 0x00045528
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

	// Token: 0x06000A60 RID: 2656 RVA: 0x00047378 File Offset: 0x00045578
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

	// Token: 0x06000A61 RID: 2657 RVA: 0x000473C8 File Offset: 0x000455C8
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

	// Token: 0x06000A62 RID: 2658 RVA: 0x00047443 File Offset: 0x00045643
	public bool IsEnabled()
	{
		return this.isEnabled;
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x0004744B File Offset: 0x0004564B
	private void Awake()
	{
		ToyPhoneUIButtonScript.allButtons.Add(this);
		this.myRectTransform = base.GetComponent<RectTransform>();
		this.myText = base.GetComponentInChildren<TextMeshProUGUI>();
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x00047470 File Offset: 0x00045670
	private void OnDestroy()
	{
		ToyPhoneUIButtonScript.allButtons.Remove(this);
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x0004747E File Offset: 0x0004567E
	private void Start()
	{
		if (this.buttonIndex < 0)
		{
			Debug.LogError("Button index not set! Button game object: " + base.gameObject.name);
		}
		this.player = Controls.GetPlayerByIndex(0);
		this.HighlightOff();
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x000474B5 File Offset: 0x000456B5
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
