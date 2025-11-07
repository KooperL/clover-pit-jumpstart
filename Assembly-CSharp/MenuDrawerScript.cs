using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;

public class MenuDrawerScript : MonoBehaviour
{
	// Token: 0x060007F4 RID: 2036 RVA: 0x000332BC File Offset: 0x000314BC
	public bool IsOpened()
	{
		return this.opened;
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x000332C4 File Offset: 0x000314C4
	public static bool IsOpened(MenuDrawerScript.Kind kind)
	{
		foreach (MenuDrawerScript menuDrawerScript in MenuDrawerScript.list)
		{
			if (menuDrawerScript.myKind == kind)
			{
				return menuDrawerScript.IsOpened();
			}
		}
		return false;
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x00033324 File Offset: 0x00031524
	public static bool IsAnyOpened()
	{
		using (List<MenuDrawerScript>.Enumerator enumerator = MenuDrawerScript.list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsOpened())
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x0003337C File Offset: 0x0003157C
	public void Open()
	{
		if (this.opened)
		{
			return;
		}
		this.opened = true;
		MainMenuScript.Open();
		if (!MainMenuScript.IsEnabled())
		{
			this.opened = false;
			return;
		}
		Controls.VibrationSet_PreferMax(this.player, 0.25f);
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x000333B4 File Offset: 0x000315B4
	public static void Open(MenuDrawerScript.Kind kind)
	{
		foreach (MenuDrawerScript menuDrawerScript in MenuDrawerScript.list)
		{
			if (menuDrawerScript.myKind == kind)
			{
				menuDrawerScript.Open();
			}
		}
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x00033410 File Offset: 0x00031610
	public void Close()
	{
		if (!this.opened)
		{
			return;
		}
		this.opened = false;
		Controls.VibrationSet_PreferMax(this.player, 0.25f);
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x00033434 File Offset: 0x00031634
	public static void Close(MenuDrawerScript.Kind kind)
	{
		foreach (MenuDrawerScript menuDrawerScript in MenuDrawerScript.list)
		{
			if (menuDrawerScript.myKind == kind)
			{
				menuDrawerScript.Close();
			}
		}
	}

	// Token: 0x060007FB RID: 2043 RVA: 0x00033490 File Offset: 0x00031690
	public static void CloseAll()
	{
		foreach (MenuDrawerScript menuDrawerScript in MenuDrawerScript.list)
		{
			menuDrawerScript.Close();
		}
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x000334E0 File Offset: 0x000316E0
	public static void RemoveDirectAction()
	{
		for (int i = 0; i < MenuDrawerScript.list.Count; i++)
		{
			MenuDrawerScript.list[i].myDiegeticMenuElement.DirectActionControl_Set(Controls.InputAction._UNDEFINED);
		}
	}

	// Token: 0x060007FD RID: 2045 RVA: 0x00033519 File Offset: 0x00031719
	private void Awake()
	{
		MenuDrawerScript.list.Add(this);
		if (this.myKind == MenuDrawerScript.Kind._UNDEFINED)
		{
			Debug.LogError("MenuDrawerScript: Kind is undefined.");
		}
		if (this.myKind == MenuDrawerScript.Kind._COUNT)
		{
			Debug.LogError("MenuDrawerScript: Kind is _COUNT.");
		}
	}

	// Token: 0x060007FE RID: 2046 RVA: 0x0003354C File Offset: 0x0003174C
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		this.myDiegeticMenuElement = base.GetComponent<DiegeticMenuElement>();
		this.myDiegeticMenuElement.DirectActionControl_Set(Controls.InputAction.menuPause);
	}

	// Token: 0x060007FF RID: 2047 RVA: 0x00033573 File Offset: 0x00031773
	private void OnDestroy()
	{
		MenuDrawerScript.list.Remove(this);
	}

	// Token: 0x06000800 RID: 2048 RVA: 0x00033584 File Offset: 0x00031784
	private void Update()
	{
		if (this.opened)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, this.openTargetLocalPosition, Tick.Time * 10f);
		}
		else
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, Vector3.zero, Tick.Time * 10f);
		}
		if (this._openedOld != this.opened)
		{
			this._openedOld = this.opened;
			if (this.opened)
			{
				Sound.Play3D("SoundDrawerOpenMetal", this.soundTransform.position, 10f, 1f, Random.Range(0.9f, 1.1f), 1);
				return;
			}
			Sound.Play3D("SoundDrawerCloseMetal", this.soundTransform.position, 10f, 1f, Random.Range(0.9f, 1.1f), 1);
		}
	}

	public static List<MenuDrawerScript> list = new List<MenuDrawerScript>();

	private const int PLAYER_INDEX = 0;

	private const float LERP_SPD = 10f;

	private Controls.PlayerExt player;

	private DiegeticMenuElement myDiegeticMenuElement;

	public MenuDrawerScript.Kind myKind = MenuDrawerScript.Kind._UNDEFINED;

	public Vector3 openTargetLocalPosition;

	public Transform soundTransform;

	private bool opened;

	private bool _openedOld;

	public enum Kind
	{
		_UNDEFINED = -1,
		mainMenu,
		powerupsInfos,
		_COUNT
	}
}
