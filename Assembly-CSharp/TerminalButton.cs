using System;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class TerminalButton : MonoBehaviour
{
	// Token: 0x06000A2A RID: 2602 RVA: 0x00045F96 File Offset: 0x00044196
	public bool IsMouseOnMe()
	{
		return this._mouseOver;
	}

	// Token: 0x06000A2B RID: 2603 RVA: 0x00045FA0 File Offset: 0x000441A0
	private bool IsMouseOver()
	{
		bool flag = VirtualCursors.IsCursorVisible(0, true);
		if (!flag && !AimCrossScript.IsEnabled())
		{
			return false;
		}
		Vector2 vector;
		vector..ctor(0.5f, 0.5f);
		if (flag)
		{
			Vector2 vector2;
			vector2..ctor((float)this.gameCamera.pixelWidth, (float)this.gameCamera.pixelHeight);
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

	// Token: 0x06000A2C RID: 2604 RVA: 0x000460F1 File Offset: 0x000442F1
	public bool HoveredState_Get()
	{
		return this._hovered;
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x000460F9 File Offset: 0x000442F9
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

	// Token: 0x06000A2E RID: 2606 RVA: 0x00046127 File Offset: 0x00044327
	public void HoverColor()
	{
		this.hoveredColorTimer = 0.15f;
	}

	// Token: 0x06000A2F RID: 2607 RVA: 0x00046134 File Offset: 0x00044334
	public bool FlashState_Get()
	{
		return this._flash;
	}

	// Token: 0x06000A30 RID: 2608 RVA: 0x0004613C File Offset: 0x0004433C
	public void FlashState_Set(bool state)
	{
		this._flash = state;
	}

	// Token: 0x06000A31 RID: 2609 RVA: 0x00046145 File Offset: 0x00044345
	public virtual void Start()
	{
		this.gameCamera = CameraGame.firstInstance.myCamera;
		this.player = Controls.GetPlayerByIndex(0);
	}

	// Token: 0x06000A32 RID: 2610 RVA: 0x00046164 File Offset: 0x00044364
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

	private const int PLAYER_INDEX = 0;

	private Controls.PlayerExt player;

	public Image ImageRenderer;

	private Camera gameCamera;

	private Color hoveredColor = Color.yellow;

	private Color nonHoveredColor = new Color(1f, 0.5f, 0f, 1f);

	private bool _mouseOver;

	private RaycastHit[] hits = new RaycastHit[10];

	private bool _hovered;

	private float hoveredColorTimer;

	private bool _flash;
}
