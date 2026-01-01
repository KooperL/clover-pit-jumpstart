using System;
using Panik;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class ScreenColliderController : MonoBehaviour
{
	// Token: 0x06000BA2 RID: 2978 RVA: 0x0000F8E4 File Offset: 0x0000DAE4
	private void RefreshClickCounts()
	{
		this.clicksCount = 0;
		this.clicksCountLuckValue = 666;
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x0005DB3C File Offset: 0x0005BD3C
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

	// Token: 0x06000BA4 RID: 2980 RVA: 0x0000F8F8 File Offset: 0x0000DAF8
	private void Start()
	{
		this.gameCamera = CameraGame.list[0].myCamera;
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x0005DC38 File Offset: 0x0005BE38
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

	// Token: 0x06000BA6 RID: 2982 RVA: 0x0000F910 File Offset: 0x0000DB10
	private void OnEnable()
	{
		this.RefreshClickCounts();
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x0000F910 File Offset: 0x0000DB10
	private void OnDisable()
	{
		this.RefreshClickCounts();
	}

	// Token: 0x04000C41 RID: 3137
	private const int playerIndex = 0;

	// Token: 0x04000C42 RID: 3138
	private Camera gameCamera;

	// Token: 0x04000C43 RID: 3139
	private int clicksCount;

	// Token: 0x04000C44 RID: 3140
	private int clicksCountLuckValue = -1;

	// Token: 0x04000C45 RID: 3141
	private RaycastHit[] hits = new RaycastHit[2];

	// Token: 0x04000C46 RID: 3142
	private bool _mouseOver;

	// Token: 0x04000C47 RID: 3143
	private Vector3 _mouseOverPosition;
}
