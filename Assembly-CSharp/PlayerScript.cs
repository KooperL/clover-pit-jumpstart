using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;

// Token: 0x0200008C RID: 140
public class PlayerScript : MonoBehaviour
{
	// Token: 0x060008BC RID: 2236 RVA: 0x0000CEEE File Offset: 0x0000B0EE
	private void PlayerExtChacheIt()
	{
		this._myPlayerExtChached = this.GetMyPlayerExt();
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x0000CEFC File Offset: 0x0000B0FC
	public Controls.PlayerExt GetMyPlayerExt()
	{
		return Controls.GetPlayerByIndex(this.playerIndex);
	}

	// Token: 0x1700005E RID: 94
	// (get) Token: 0x060008BE RID: 2238 RVA: 0x0000CF09 File Offset: 0x0000B109
	public Controls.PlayerExt MyPlayerExt
	{
		get
		{
			return this._myPlayerExtChached;
		}
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x000498BC File Offset: 0x00047ABC
	private string GetStepSound()
	{
		if (this.onConcrete)
		{
			return "SoundStepConcrete" + Util.Choose<int>(new int[] { 1, 2, 3 }).ToString();
		}
		return "SoundStep" + Util.Choose<int>(new int[] { 1, 2, 3 }).ToString();
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00049924 File Offset: 0x00047B24
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

	// Token: 0x060008C1 RID: 2241 RVA: 0x0000CF11 File Offset: 0x0000B111
	private void Start()
	{
		this.PlayerExtChacheIt();
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x00049988 File Offset: 0x00047B88
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

	// Token: 0x060008C3 RID: 2243 RVA: 0x00049A14 File Offset: 0x00047C14
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
					this.stepTimer = this.stepTimerMax + global::UnityEngine.Random.Range(-this.stepTimerRnd, this.stepTimerRnd);
					Sound.Play3D(this.GetStepSound(), base.transform.position, 10f, 1f, global::UnityEngine.Random.Range(0.9f, 1.1f), AudioRolloffMode.Linear);
				}
				return;
			}
		}
		this.rb.linearVelocity = Vector3.zero;
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0000CF19 File Offset: 0x0000B119
	private void FixedUpdate()
	{
		this.onConcrete = false;
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x0000CF22 File Offset: 0x0000B122
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("SoundConcrete"))
		{
			this.onConcrete = true;
		}
	}

	// Token: 0x04000877 RID: 2167
	public static PlayerScript instanceP1 = null;

	// Token: 0x04000878 RID: 2168
	public static PlayerScript instanceP2 = null;

	// Token: 0x04000879 RID: 2169
	public static PlayerScript instanceP3 = null;

	// Token: 0x0400087A RID: 2170
	public static PlayerScript instanceP4 = null;

	// Token: 0x0400087B RID: 2171
	public static List<PlayerScript> list = new List<PlayerScript>();

	// Token: 0x0400087C RID: 2172
	public int playerIndex;

	// Token: 0x0400087D RID: 2173
	private Controls.PlayerExt _myPlayerExtChached;

	// Token: 0x0400087E RID: 2174
	private Rigidbody rb;

	// Token: 0x0400087F RID: 2175
	private float stepTimer;

	// Token: 0x04000880 RID: 2176
	private float stepTimerMax = 0.5f;

	// Token: 0x04000881 RID: 2177
	private float stepTimerRnd = 0.05f;

	// Token: 0x04000882 RID: 2178
	private bool onConcrete;
}
