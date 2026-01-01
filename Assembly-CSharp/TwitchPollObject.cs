using System;
using Panik;
using UnityEngine;

// Token: 0x020000FF RID: 255
public class TwitchPollObject : MonoBehaviour
{
	// Token: 0x06000C54 RID: 3156 RVA: 0x000101FC File Offset: 0x0000E3FC
	public static bool IsEnabled()
	{
		return !(TwitchPollObject.instance == null) && TwitchPollObject.instance.holder.activeSelf;
	}

	// Token: 0x06000C55 RID: 3157 RVA: 0x00061F50 File Offset: 0x00060150
	public void MenuConnectionUpdate()
	{
		if (!this.holder.activeSelf)
		{
			if (DiegeticMenuController.MainMenu.elements.Contains(TwitchPollObject.instance.myDiegeticMenuElement))
			{
				DiegeticMenuController.MainMenu.elements.Remove(TwitchPollObject.instance.myDiegeticMenuElement);
			}
			return;
		}
		DiegeticMenuController.MainMenu.elements.Add(TwitchPollObject.instance.myDiegeticMenuElement);
		TwitchPollObject.instance.myDiegeticMenuElement.SetMyController(DiegeticMenuController.MainMenu);
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x0001021C File Offset: 0x0000E41C
	private void Awake()
	{
		TwitchPollObject.instance = this;
		this.myDiegeticMenuElement = base.GetComponent<DiegeticMenuElement>();
		this.meshRend = base.GetComponentInChildren<MeshRenderer>();
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x0001023C File Offset: 0x0000E43C
	private void OnDestroy()
	{
		if (TwitchPollObject.instance == this)
		{
			TwitchPollObject.instance = null;
		}
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x00010251 File Offset: 0x0000E451
	private void Start()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x00061FD0 File Offset: 0x000601D0
	private void Update()
	{
		bool flag = TwitchPollObject.IsEnabled();
		bool flag2 = TwitchMaster.IsLoggedInAndEnabled();
		if (flag2 != flag)
		{
			this.holder.SetActive(flag2);
			flag = flag2;
			this.MenuConnectionUpdate();
		}
		if (flag)
		{
			Material material = this.matDefault;
			if (Util.AngleSin(Tick.PassedTime * 360f) > 0.95f)
			{
				material = this.matBright;
			}
			if (this.meshRend.sharedMaterial != material)
			{
				this.meshRend.sharedMaterial = material;
			}
		}
	}

	// Token: 0x04000D22 RID: 3362
	public static TwitchPollObject instance;

	// Token: 0x04000D23 RID: 3363
	public GameObject holder;

	// Token: 0x04000D24 RID: 3364
	private DiegeticMenuElement myDiegeticMenuElement;

	// Token: 0x04000D25 RID: 3365
	private MeshRenderer meshRend;

	// Token: 0x04000D26 RID: 3366
	public Material matDefault;

	// Token: 0x04000D27 RID: 3367
	public Material matBright;
}
