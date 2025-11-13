using System;
using Febucci.UI;
using Panik;
using UnityEngine;

public class TextAccessibilityController : MonoBehaviour
{
	// Token: 0x06000A51 RID: 2641 RVA: 0x00047058 File Offset: 0x00045258
	public void Refresh()
	{
		this.textAnimator.BehaviourEffectsEnaabledSet(!Data.settings.dyslexicFontEnabled);
	}

	// Token: 0x06000A52 RID: 2642 RVA: 0x00047074 File Offset: 0x00045274
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

	// Token: 0x06000A53 RID: 2643 RVA: 0x000470EF File Offset: 0x000452EF
	private void Start()
	{
		this._bookedRefresh = true;
	}

	// Token: 0x06000A54 RID: 2644 RVA: 0x000470F8 File Offset: 0x000452F8
	private void OnDestroy()
	{
		if (MainMenuScript.instance != null)
		{
			MainMenuScript instance = MainMenuScript.instance;
			instance.onDyslexiaSettingChange = (MainMenuScript.Callback)Delegate.Remove(instance.onDyslexiaSettingChange, new MainMenuScript.Callback(this.Refresh));
		}
	}

	// Token: 0x06000A55 RID: 2645 RVA: 0x00047130 File Offset: 0x00045330
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

	private TextAnimator textAnimator;

	private TextAnimatorPlayer textAnimatorPlayer;

	private bool _enabledStateOld;

	private bool _bookedRefresh;
}
