using System;
using Panik;
using UnityEngine;

public class TwitchPollObject : MonoBehaviour
{
	// Token: 0x06000A6C RID: 2668 RVA: 0x000472AB File Offset: 0x000454AB
	public static bool IsEnabled()
	{
		return !(TwitchPollObject.instance == null) && TwitchPollObject.instance.holder.activeSelf;
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x000472CC File Offset: 0x000454CC
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

	// Token: 0x06000A6E RID: 2670 RVA: 0x00047349 File Offset: 0x00045549
	private void Awake()
	{
		TwitchPollObject.instance = this;
		this.myDiegeticMenuElement = base.GetComponent<DiegeticMenuElement>();
		this.meshRend = base.GetComponentInChildren<MeshRenderer>();
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x00047369 File Offset: 0x00045569
	private void OnDestroy()
	{
		if (TwitchPollObject.instance == this)
		{
			TwitchPollObject.instance = null;
		}
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x0004737E File Offset: 0x0004557E
	private void Start()
	{
		this.holder.SetActive(false);
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0004738C File Offset: 0x0004558C
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
