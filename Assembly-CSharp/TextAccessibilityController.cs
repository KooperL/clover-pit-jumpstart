using System;
using Febucci.UI;
using Panik;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public class TextAccessibilityController : MonoBehaviour
{
	// Token: 0x06000C15 RID: 3093 RVA: 0x0000FEB0 File Offset: 0x0000E0B0
	public void Refresh()
	{
		this.textAnimator.BehaviourEffectsEnaabledSet(!Data.settings.dyslexicFontEnabled);
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x00061160 File Offset: 0x0005F360
	private void Awake()
	{
		this.textAnimator = base.GetComponent<TextAnimator>();
		this.textAnimatorPlayer = base.GetComponent<TextAnimatorPlayer>();
		if (this.textAnimator == null)
		{
			Debug.LogError("TextAccessibilityController: Awake: No TextAnimator component found in gameObject: " + base.name);
		}
		if (MainMenuScript.instance != null)
		{
			MainMenuScript instance = MainMenuScript.instance;
			instance.onDyslexiaSettingChange = (MainMenuScript.Callback)Delegate.Combine(instance.onDyslexiaSettingChange, new MainMenuScript.Callback(this.Refresh));
		}
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x0000FECA File Offset: 0x0000E0CA
	private void Start()
	{
		this._bookedRefresh = true;
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x0000FED3 File Offset: 0x0000E0D3
	private void OnDestroy()
	{
		if (MainMenuScript.instance != null)
		{
			MainMenuScript instance = MainMenuScript.instance;
			instance.onDyslexiaSettingChange = (MainMenuScript.Callback)Delegate.Remove(instance.onDyslexiaSettingChange, new MainMenuScript.Callback(this.Refresh));
		}
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x000611DC File Offset: 0x0005F3DC
	private void LateUpdate()
	{
		bool flag = !Data.settings.dyslexicFontEnabled;
		bool flag2 = this.textAnimator.BehaviourEffectsEnabledGet();
		if (this._enabledStateOld != flag || this._bookedRefresh || flag2 != flag)
		{
			this._enabledStateOld = flag;
			this._bookedRefresh = false;
			this.Refresh();
		}
	}

	// Token: 0x04000CED RID: 3309
	private TextAnimator textAnimator;

	// Token: 0x04000CEE RID: 3310
	private TextAnimatorPlayer textAnimatorPlayer;

	// Token: 0x04000CEF RID: 3311
	private bool _enabledStateOld;

	// Token: 0x04000CF0 RID: 3312
	private bool _bookedRefresh;
}
