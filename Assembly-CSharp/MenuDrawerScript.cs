using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;

// Token: 0x02000098 RID: 152
public class MenuDrawerScript : MonoBehaviour
{
	// Token: 0x06000906 RID: 2310 RVA: 0x0000D1EE File Offset: 0x0000B3EE
	public bool IsOpened()
	{
		return this.opened;
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0004A580 File Offset: 0x00048780
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

	// Token: 0x06000908 RID: 2312 RVA: 0x0004A5E0 File Offset: 0x000487E0
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

	// Token: 0x06000909 RID: 2313 RVA: 0x0000D1F6 File Offset: 0x0000B3F6
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

	// Token: 0x0600090A RID: 2314 RVA: 0x0004A638 File Offset: 0x00048838
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

	// Token: 0x0600090B RID: 2315 RVA: 0x0000D22C File Offset: 0x0000B42C
	public void Close()
	{
		if (!this.opened)
		{
			return;
		}
		this.opened = false;
		Controls.VibrationSet_PreferMax(this.player, 0.25f);
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0004A694 File Offset: 0x00048894
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

	// Token: 0x0600090D RID: 2317 RVA: 0x0004A6F0 File Offset: 0x000488F0
	public static void CloseAll()
	{
		foreach (MenuDrawerScript menuDrawerScript in MenuDrawerScript.list)
		{
			menuDrawerScript.Close();
		}
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x0004A740 File Offset: 0x00048940
	public static void RemoveDirectAction()
	{
		for (int i = 0; i < MenuDrawerScript.list.Count; i++)
		{
			MenuDrawerScript.list[i].myDiegeticMenuElement.DirectActionControl_Set(Controls.InputAction._UNDEFINED);
		}
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0000D24E File Offset: 0x0000B44E
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

	// Token: 0x06000910 RID: 2320 RVA: 0x0000D281 File Offset: 0x0000B481
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		this.myDiegeticMenuElement = base.GetComponent<DiegeticMenuElement>();
		this.myDiegeticMenuElement.DirectActionControl_Set(Controls.InputAction.menuPause);
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x0000D2A8 File Offset: 0x0000B4A8
	private void OnDestroy()
	{
		MenuDrawerScript.list.Remove(this);
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x0004A77C File Offset: 0x0004897C
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
				Sound.Play3D("SoundDrawerOpenMetal", this.soundTransform.position, 10f, 1f, global::UnityEngine.Random.Range(0.9f, 1.1f), AudioRolloffMode.Linear);
				return;
			}
			Sound.Play3D("SoundDrawerCloseMetal", this.soundTransform.position, 10f, 1f, global::UnityEngine.Random.Range(0.9f, 1.1f), AudioRolloffMode.Linear);
		}
	}

	// Token: 0x040008B7 RID: 2231
	public static List<MenuDrawerScript> list = new List<MenuDrawerScript>();

	// Token: 0x040008B8 RID: 2232
	private const int PLAYER_INDEX = 0;

	// Token: 0x040008B9 RID: 2233
	private const float LERP_SPD = 10f;

	// Token: 0x040008BA RID: 2234
	private Controls.PlayerExt player;

	// Token: 0x040008BB RID: 2235
	private DiegeticMenuElement myDiegeticMenuElement;

	// Token: 0x040008BC RID: 2236
	public MenuDrawerScript.Kind myKind = MenuDrawerScript.Kind._UNDEFINED;

	// Token: 0x040008BD RID: 2237
	public Vector3 openTargetLocalPosition;

	// Token: 0x040008BE RID: 2238
	public Transform soundTransform;

	// Token: 0x040008BF RID: 2239
	private bool opened;

	// Token: 0x040008C0 RID: 2240
	private bool _openedOld;

	// Token: 0x02000099 RID: 153
	public enum Kind
	{
		// Token: 0x040008C2 RID: 2242
		_UNDEFINED = -1,
		// Token: 0x040008C3 RID: 2243
		mainMenu,
		// Token: 0x040008C4 RID: 2244
		powerupsInfos,
		// Token: 0x040008C5 RID: 2245
		_COUNT
	}
}
