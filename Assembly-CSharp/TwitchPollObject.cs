using System;
using Panik;
using UnityEngine;

public class TwitchPollObject : MonoBehaviour
{
	// Token: 0x06000A81 RID: 2689 RVA: 0x00047A0B File Offset: 0x00045C0B
	public static bool IsEnabled()
	{
		return !(TwitchPollObject.instance == null) && TwitchPollObject.instance.holder.activeSelf;
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x00047A2C File Offset: 0x00045C2C
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

	// Token: 0x06000A83 RID: 2691 RVA: 0x00047AA9 File Offset: 0x00045CA9
	private void Awake()
	{
		TwitchPollObject.instance = this;
		this.myDiegeticMenuElement = base.GetComponent<DiegeticMenuElement>();
		this.meshRend = base.GetComponentInChildren<MeshRenderer>();
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x00047AC9 File Offset: 0x00045CC9
	private void OnDestroy()
	{
		if (TwitchPollObject.instance == this)
		{
			TwitchPollObject.instance = null;
		}
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x00047ADE File Offset: 0x00045CDE
	private void Start()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x00047AEC File Offset: 0x00045CEC
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

	public static TwitchPollObject instance;

	public GameObject holder;

	private DiegeticMenuElement myDiegeticMenuElement;

	private MeshRenderer meshRend;

	public Material matDefault;

	public Material matBright;
}
