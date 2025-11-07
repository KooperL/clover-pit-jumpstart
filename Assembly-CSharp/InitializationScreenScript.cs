using System;
using System.Collections;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class InitializationScreenScript : MonoBehaviour
{
	// Token: 0x060008BB RID: 2235 RVA: 0x00039BD0 File Offset: 0x00037DD0
	private void Awake()
	{
		if (InitializationScreenScript.instance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		InitializationScreenScript.instance = this;
		base.StartCoroutine(this.UpdateCoroutine());
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x00039BFE File Offset: 0x00037DFE
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
				Object.Destroy(base.gameObject);
			}
			yield return null;
		}
		yield break;
	}

	public static InitializationScreenScript instance;

	private const float SPINNING_WHEEL_SPEED = 360f;

	public Image spinningWheel;
}
