using System;
using System.Collections;
using Panik;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B7 RID: 183
public class InitializationScreenScript : MonoBehaviour
{
	// Token: 0x060009F3 RID: 2547 RVA: 0x0000DDF0 File Offset: 0x0000BFF0
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

	// Token: 0x060009F4 RID: 2548 RVA: 0x0000DE1E File Offset: 0x0000C01E
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

	// Token: 0x04000A28 RID: 2600
	public static InitializationScreenScript instance;

	// Token: 0x04000A29 RID: 2601
	private const float SPINNING_WHEEL_SPEED = 360f;

	// Token: 0x04000A2A RID: 2602
	public Image spinningWheel;
}
