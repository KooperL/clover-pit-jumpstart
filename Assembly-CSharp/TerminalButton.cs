using System;
using Panik;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000F5 RID: 245
public class TerminalButton : MonoBehaviour
{
	// Token: 0x06000C03 RID: 3075 RVA: 0x0000FD9D File Offset: 0x0000DF9D
	public bool IsMouseOnMe()
	{
		return this._mouseOver;
	}

	// Token: 0x06000C04 RID: 3076 RVA: 0x00060910 File Offset: 0x0005EB10
	private bool IsMouseOver()
	{
		bool flag = VirtualCursors.IsCursorVisible(0, true);
		if (!flag && !AimCrossScript.IsEnabled())
		{
			return false;
		}
		Vector2 vector = new Vector2(0.5f, 0.5f);
		if (flag)
		{
			Vector2 vector2 = new Vector2((float)this.gameCamera.pixelWidth, (float)this.gameCamera.pixelHeight);
			vector = VirtualCursors.CursorPositionNormalizedCenteredGet_ReferenceResolution(0, vector2);
			vector.x += 0.5f;
			vector.y += 0.5f;
		}
		int num = Physics.RaycastNonAlloc(this.gameCamera.ViewportPointToRay(new Vector3(vector.x, vector.y, 0f)), this.hits, 100f);
		float num2 = float.MaxValue;
		float num3 = num2;
		bool flag2 = false;
		for (int i = 0; i < num; i++)
		{
			if (this.hits[i].collider.gameObject == base.gameObject)
			{
				flag2 = true;
				if (num3 > this.hits[i].distance)
				{
					num3 = this.hits[i].distance;
				}
			}
			if (num2 > this.hits[i].distance)
			{
				num2 = this.hits[i].distance;
			}
		}
		if (num3 > num2)
		{
			flag2 = false;
		}
		return flag2;
	}

	// Token: 0x06000C05 RID: 3077 RVA: 0x0000FDA5 File Offset: 0x0000DFA5
	public bool HoveredState_Get()
	{
		return this._hovered;
	}

	// Token: 0x06000C06 RID: 3078 RVA: 0x0000FDAD File Offset: 0x0000DFAD
	public void HoveredState_Set(bool state)
	{
		if (state != this._hovered && state)
		{
			Controls.VibrationSet_PreferMax(this.player, 0.1f);
		}
		this._hovered = state;
		if (state)
		{
			this.HoverColor();
		}
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x0000FDDB File Offset: 0x0000DFDB
	public void HoverColor()
	{
		this.hoveredColorTimer = 0.15f;
	}

	// Token: 0x06000C08 RID: 3080 RVA: 0x0000FDE8 File Offset: 0x0000DFE8
	public bool FlashState_Get()
	{
		return this._flash;
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x0000FDF0 File Offset: 0x0000DFF0
	public void FlashState_Set(bool state)
	{
		this._flash = state;
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x0000FDF9 File Offset: 0x0000DFF9
	public virtual void Start()
	{
		this.gameCamera = CameraGame.firstInstance.myCamera;
		this.player = Controls.GetPlayerByIndex(0);
	}

	// Token: 0x06000C0B RID: 3083 RVA: 0x00060A64 File Offset: 0x0005EC64
	public virtual void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		this._mouseOver = this.IsMouseOver();
		if (!this._hovered)
		{
			this.hoveredColorTimer -= Tick.Time;
		}
		Color color = ((this.hoveredColorTimer > 0f) ? this.hoveredColor : this.nonHoveredColor);
		if (this.ImageRenderer.color != color)
		{
			this.ImageRenderer.color = color;
		}
		if (this._flash)
		{
			if (Util.AngleSin(Tick.PassedTime * 1440f) > 0f || this.hoveredColorTimer > 0f)
			{
				color = this.hoveredColor;
			}
			else
			{
				color = this.nonHoveredColor;
			}
			if (this.ImageRenderer.color != color)
			{
				this.ImageRenderer.color = color;
			}
		}
	}

	// Token: 0x04000CD1 RID: 3281
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000CD2 RID: 3282
	private Controls.PlayerExt player;

	// Token: 0x04000CD3 RID: 3283
	public Image ImageRenderer;

	// Token: 0x04000CD4 RID: 3284
	private Camera gameCamera;

	// Token: 0x04000CD5 RID: 3285
	private Color hoveredColor = Color.yellow;

	// Token: 0x04000CD6 RID: 3286
	private Color nonHoveredColor = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x04000CD7 RID: 3287
	private bool _mouseOver;

	// Token: 0x04000CD8 RID: 3288
	private RaycastHit[] hits = new RaycastHit[10];

	// Token: 0x04000CD9 RID: 3289
	private bool _hovered;

	// Token: 0x04000CDA RID: 3290
	private float hoveredColorTimer;

	// Token: 0x04000CDB RID: 3291
	private bool _flash;
}
