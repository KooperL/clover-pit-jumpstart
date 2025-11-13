using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;

public class MenuDrawerScript : MonoBehaviour
{
	// Token: 0x060007FB RID: 2043 RVA: 0x000334A4 File Offset: 0x000316A4
	public bool IsOpened()
	{
		return this.opened;
	}

	// Token: 0x060007FC RID: 2044 RVA: 0x000334AC File Offset: 0x000316AC
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

	// Token: 0x060007FD RID: 2045 RVA: 0x0003350C File Offset: 0x0003170C
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

	// Token: 0x060007FE RID: 2046 RVA: 0x00033564 File Offset: 0x00031764
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

	// Token: 0x060007FF RID: 2047 RVA: 0x0003359C File Offset: 0x0003179C
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

	// Token: 0x06000800 RID: 2048 RVA: 0x000335F8 File Offset: 0x000317F8
	public void Close()
	{
		if (!this.opened)
		{
			return;
		}
		this.opened = false;
		Controls.VibrationSet_PreferMax(this.player, 0.25f);
	}

	// Token: 0x06000801 RID: 2049 RVA: 0x0003361C File Offset: 0x0003181C
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

	// Token: 0x06000802 RID: 2050 RVA: 0x00033678 File Offset: 0x00031878
	public static void CloseAll()
	{
		foreach (MenuDrawerScript menuDrawerScript in MenuDrawerScript.list)
		{
			menuDrawerScript.Close();
		}
	}

	// Token: 0x06000803 RID: 2051 RVA: 0x000336C8 File Offset: 0x000318C8
	public static void RemoveDirectAction()
	{
		for (int i = 0; i < MenuDrawerScript.list.Count; i++)
		{
			MenuDrawerScript.list[i].myDiegeticMenuElement.DirectActionControl_Set(Controls.InputAction._UNDEFINED);
		}
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x00033701 File Offset: 0x00031901
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

	// Token: 0x06000805 RID: 2053 RVA: 0x00033734 File Offset: 0x00031934
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		this.myDiegeticMenuElement = base.GetComponent<DiegeticMenuElement>();
		this.myDiegeticMenuElement.DirectActionControl_Set(Controls.InputAction.menuPause);
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x0003375B File Offset: 0x0003195B
	private void OnDestroy()
	{
		MenuDrawerScript.list.Remove(this);
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x0003376C File Offset: 0x0003196C
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
				Sound.Play3D("SoundDrawerOpenMetal", this.soundTransform.position, 10f, 1f, global::UnityEngine.Random.Range(0.9f, 1.1f), 1);
				return;
			}
			Sound.Play3D("SoundDrawerCloseMetal", this.soundTransform.position, 10f, 1f, global::UnityEngine.Random.Range(0.9f, 1.1f), 1);
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
