using System;
using Panik;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000BA RID: 186
public class IsModdedScreenScript : MonoBehaviour
{
	// Token: 0x06000A07 RID: 2567 RVA: 0x0000DEFF File Offset: 0x0000C0FF
	public bool IsEnabled()
	{
		return !(IsModdedScreenScript.instance == null) && this.holder.activeSelf;
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x0000DF1B File Offset: 0x0000C11B
	public static void UpdateState()
	{
		if (IsModdedScreenScript.instance == null)
		{
			return;
		}
		IsModdedScreenScript.instance.holder.SetActive(Master.IsModded());
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x0000DF3F File Offset: 0x0000C13F
	private void Awake()
	{
		IsModdedScreenScript.instance = this;
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x0000DF47 File Offset: 0x0000C147
	private void Start()
	{
		IsModdedScreenScript.UpdateState();
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x0000DF47 File Offset: 0x0000C147
	private void OnEnable()
	{
		IsModdedScreenScript.UpdateState();
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x0000DF4E File Offset: 0x0000C14E
	private void OnDestroy()
	{
		if (IsModdedScreenScript.instance == this)
		{
			IsModdedScreenScript.instance = null;
		}
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x0000DF63 File Offset: 0x0000C163
	private void Update()
	{
		if (!this.IsEnabled())
		{
			return;
		}
		this.gearImage.transform.AddLocalZAngle(Tick.Time * 45f);
	}

	// Token: 0x04000A36 RID: 2614
	public static IsModdedScreenScript instance;

	// Token: 0x04000A37 RID: 2615
	public GameObject holder;

	// Token: 0x04000A38 RID: 2616
	public Image gearImage;
}
