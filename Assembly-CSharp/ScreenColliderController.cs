using System;
using Panik;
using UnityEngine;

public class ScreenColliderController : MonoBehaviour
{
	// Token: 0x060009E6 RID: 2534 RVA: 0x00043B0E File Offset: 0x00041D0E
	private void RefreshClickCounts()
	{
		this.clicksCount = 0;
		this.clicksCountLuckValue = 666;
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x00043B24 File Offset: 0x00041D24
	private bool IsMouseOver()
	{
		bool flag = VirtualCursors.IsCursorVisible(0, true);
		if (!flag)
		{
			return false;
		}
		Vector2 vector = new Vector2(-1f, -1f);
		if (flag)
		{
			Vector2 vector2 = new Vector2((float)this.gameCamera.pixelWidth, (float)this.gameCamera.pixelHeight);
			vector = VirtualCursors.CursorPositionNormalizedCenteredGet_ReferenceResolution(0, vector2);
			vector.x += 0.5f;
			vector.y += 0.5f;
		}
		int num = Physics.RaycastNonAlloc(this.gameCamera.ViewportPointToRay(new Vector3(vector.x, vector.y, 0f)), this.hits, 100f);
		bool flag2 = false;
		for (int i = 0; i < num; i++)
		{
			if (!(this.hits[i].collider.gameObject != base.gameObject))
			{
				flag2 = true;
				this._mouseOverPosition = this.hits[i].point;
				break;
			}
		}
		return flag2;
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x00043C1F File Offset: 0x00041E1F
	private void Start()
	{
		this.gameCamera = CameraGame.list[0].myCamera;
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x00043C38 File Offset: 0x00041E38
	private void Update()
	{
		if (PowerupTriggerAnimController.HasAnimations())
		{
			return;
		}
		if (MainMenuScript.IsEnabled())
		{
			return;
		}
		this._mouseOver = this.IsMouseOver();
		bool flag = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true);
		if (this._mouseOver && flag)
		{
			this.clicksCount++;
			bool flag2 = false;
			if (this.clicksCount % this.clicksCountLuckValue == 0 && this.clicksCount > 0)
			{
				flag2 = true;
				SlotMachineScript.instance.ForceNextLuck_Add(3);
				this.RefreshClickCounts();
			}
			else
			{
				SlotMachineScript.instance.ForceNextLuck_Reset();
			}
			Spawn.FromPool("Effect Star Screen", this._mouseOverPosition, Pool.instance.transform);
			CameraGame.Shake(0.5f);
			Sound.Play("SoundSlotMachineScreenTick", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f) - (flag2 ? 0.5f : 0f));
		}
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x00043D12 File Offset: 0x00041F12
	private void OnEnable()
	{
		this.RefreshClickCounts();
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x00043D1A File Offset: 0x00041F1A
	private void OnDisable()
	{
		this.RefreshClickCounts();
	}

	private const int playerIndex = 0;

	private Camera gameCamera;

	private int clicksCount;

	private int clicksCountLuckValue = -1;

	private RaycastHit[] hits = new RaycastHit[2];

	private bool _mouseOver;

	private Vector3 _mouseOverPosition;
}
