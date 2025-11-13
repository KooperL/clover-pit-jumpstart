using System;
using Panik;
using UnityEngine;

public class ScreenColliderController : MonoBehaviour
{
	// Token: 0x060009FA RID: 2554 RVA: 0x00044176 File Offset: 0x00042376
	private void RefreshClickCounts()
	{
		this.clicksCount = 0;
		this.clicksCountLuckValue = 666;
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x0004418C File Offset: 0x0004238C
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

	// Token: 0x060009FC RID: 2556 RVA: 0x00044287 File Offset: 0x00042487
	private void Start()
	{
		this.gameCamera = CameraGame.list[0].myCamera;
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x000442A0 File Offset: 0x000424A0
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

	// Token: 0x060009FE RID: 2558 RVA: 0x0004437A File Offset: 0x0004257A
	private void OnEnable()
	{
		this.RefreshClickCounts();
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x00044382 File Offset: 0x00042582
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
