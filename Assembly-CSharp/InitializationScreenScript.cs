using System;
using System.Collections;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class InitializationScreenScript : MonoBehaviour
{
	// Token: 0x060008C2 RID: 2242 RVA: 0x00039E50 File Offset: 0x00038050
	private void Awake()
	{
		if (InitializationScreenScript.instance != null)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		InitializationScreenScript.instance = this;
		base.StartCoroutine(this.UpdateCoroutine());
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x00039E7E File Offset: 0x0003807E
	private IEnumerator UpdateCoroutine()
	{
		for (;;)
		{
			if (this.spinningWheel != null)
			{
				this.spinningWheel.transform.AddLocalZAngle(Tick.Time * 360f);
			}
			if (PlatformMaster.IsInitialized())
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
			yield return null;
		}
		yield break;
	}

	public static InitializationScreenScript instance;

	private const float SPINNING_WHEEL_SPEED = 360f;

	public Image spinningWheel;
}
