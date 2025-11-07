using System;
using Febucci.UI;
using Panik;
using UnityEngine;

public class TextAccessibilityController : MonoBehaviour
{
	// Token: 0x06000A3C RID: 2620 RVA: 0x000468F8 File Offset: 0x00044AF8
	public void Refresh()
	{
		this.textAnimator.BehaviourEffectsEnaabledSet(!Data.settings.dyslexicFontEnabled);
	}

	// Token: 0x06000A3D RID: 2621 RVA: 0x00046914 File Offset: 0x00044B14
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

	// Token: 0x06000A3E RID: 2622 RVA: 0x0004698F File Offset: 0x00044B8F
	private void Start()
	{
		this._bookedRefresh = true;
	}

	// Token: 0x06000A3F RID: 2623 RVA: 0x00046998 File Offset: 0x00044B98
	private void OnDestroy()
	{
		if (MainMenuScript.instance != null)
		{
			MainMenuScript instance = MainMenuScript.instance;
			instance.onDyslexiaSettingChange = (MainMenuScript.Callback)Delegate.Remove(instance.onDyslexiaSettingChange, new MainMenuScript.Callback(this.Refresh));
		}
	}

	// Token: 0x06000A40 RID: 2624 RVA: 0x000469D0 File Offset: 0x00044BD0
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
