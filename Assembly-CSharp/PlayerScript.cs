using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	// Token: 0x060007B7 RID: 1975 RVA: 0x000324AF File Offset: 0x000306AF
	private void PlayerExtChacheIt()
	{
		this._myPlayerExtChached = this.GetMyPlayerExt();
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x000324BD File Offset: 0x000306BD
	public Controls.PlayerExt GetMyPlayerExt()
	{
		return Controls.GetPlayerByIndex(this.playerIndex);
	}

	// (get) Token: 0x060007B9 RID: 1977 RVA: 0x000324CA File Offset: 0x000306CA
	public Controls.PlayerExt MyPlayerExt
	{
		get
		{
			return this._myPlayerExtChached;
		}
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x000324D4 File Offset: 0x000306D4
	private string GetStepSound()
	{
		if (this.onConcrete)
		{
			return "SoundStepConcrete" + Util.Choose<int>(new int[] { 1, 2, 3 }).ToString();
		}
		return "SoundStep" + Util.Choose<int>(new int[] { 1, 2, 3 }).ToString();
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x0003253C File Offset: 0x0003073C
	private void Awake()
	{
		switch (this.playerIndex)
		{
		case 0:
			PlayerScript.instanceP1 = this;
			break;
		case 1:
			PlayerScript.instanceP2 = this;
			break;
		case 2:
			PlayerScript.instanceP3 = this;
			break;
		case 3:
			PlayerScript.instanceP4 = this;
			break;
		}
		PlayerScript.list.Add(this);
		this.rb = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x0003259D File Offset: 0x0003079D
	private void Start()
	{
		this.PlayerExtChacheIt();
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x000325A8 File Offset: 0x000307A8
	private void OnDestroy()
	{
		switch (this.playerIndex)
		{
		case 0:
			if (PlayerScript.instanceP1 == this)
			{
				PlayerScript.instanceP1 = null;
			}
			break;
		case 1:
			if (PlayerScript.instanceP2 == this)
			{
				PlayerScript.instanceP2 = null;
			}
			break;
		case 2:
			if (PlayerScript.instanceP3 == this)
			{
				PlayerScript.instanceP3 = null;
			}
			break;
		case 3:
			if (PlayerScript.instanceP4 == this)
			{
				PlayerScript.instanceP4 = null;
			}
			break;
		}
		PlayerScript.list.Remove(this);
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x00032634 File Offset: 0x00030834
	private void Update()
	{
		if (Tick.IsGameRunning && PlatformMaster.IsInitialized() && (GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.preparation || GameplayMaster.EndingFreeRoaming))
		{
			this.PlayerExtChacheIt();
			if (GameplayMaster.GetGamePhase() != GameplayMaster.GamePhase.intro && GameplayMaster.GetGamePhase() != GameplayMaster.GamePhase.tutorialObsolete && !PowerupTriggerAnimController.HasAnimations() && !MemoryPackDealUI.IsDealRunnning() && !CameraDebug.IsEnabled())
			{
				Transform targetTransform = CameraController.GetTargetTransform();
				Vector2 vector = Vector2.zero;
				vector.x = Controls.ActionAxisPair_GetValue(this.playerIndex, Controls.InputAction.moveRight, Controls.InputAction.moveLeft, true);
				vector.y = Controls.ActionAxisPair_GetValue(this.playerIndex, Controls.InputAction.moveUp, Controls.InputAction.moveDown, true);
				if (ConsolePrompt.ConsoleIsEnabled())
				{
					vector = Vector2.zero;
				}
				float num = 6f;
				if (GameplayMaster.EndingFreeRoaming)
				{
					num = 12f;
				}
				this.rb.linearVelocity += Util.AxisToFpsVec3(vector, targetTransform.eulerAngles.y) * 128f * Tick.Time;
				this.rb.linearVelocity = Vector3.ClampMagnitude(this.rb.linearVelocity, num * Mathf.Min(1f, vector.magnitude));
				if (vector.magnitude < 0.1f)
				{
					this.rb.linearVelocity = Vector3.Lerp(this.rb.linearVelocity, Vector3.zero, Tick.Time * 50f);
					return;
				}
				float num2 = this.rb.linearVelocity.magnitude / num;
				this.stepTimer -= Tick.Time * num2;
				if (this.stepTimer <= 0f)
				{
					this.stepTimer = this.stepTimerMax + Random.Range(-this.stepTimerRnd, this.stepTimerRnd);
					Sound.Play3D(this.GetStepSound(), base.transform.position, 10f, 1f, Random.Range(0.9f, 1.1f), 1);
				}
				return;
			}
		}
		this.rb.linearVelocity = Vector3.zero;
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x00032836 File Offset: 0x00030A36
	private void FixedUpdate()
	{
		this.onConcrete = false;
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x0003283F File Offset: 0x00030A3F
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("SoundConcrete"))
		{
			this.onConcrete = true;
		}
	}

	public static PlayerScript instanceP1 = null;

	public static PlayerScript instanceP2 = null;

	public static PlayerScript instanceP3 = null;

	public static PlayerScript instanceP4 = null;

	public static List<PlayerScript> list = new List<PlayerScript>();

	public int playerIndex;

	private Controls.PlayerExt _myPlayerExtChached;

	private Rigidbody rb;

	private float stepTimer;

	private float stepTimerMax = 0.5f;

	private float stepTimerRnd = 0.05f;

	private bool onConcrete;
}
