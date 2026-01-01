using System;
using UnityEngine;

namespace Panik
{
	// Token: 0x02000172 RID: 370
	public class Pausable : MonoBehaviour
	{
		// Token: 0x060010FE RID: 4350 RVA: 0x00013E80 File Offset: 0x00012080
		public bool PausedGet()
		{
			return this._isPaused;
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00013E88 File Offset: 0x00012088
		public void PausableSet(bool isPausable)
		{
			this.isPausable = isPausable;
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00013E91 File Offset: 0x00012091
		public bool PausableGet()
		{
			return this.isPausable;
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0007319C File Offset: 0x0007139C
		private void _PauseMe()
		{
			this._isPaused = true;
			for (int i = 0; i < this.myRbs.Length; i++)
			{
				this.velBackup[i] = this.myRbs[i].linearVelocity;
				this.angularVelBackup[i] = this.myRbs[i].angularVelocity;
				this.useGravityBackup[i] = this.myRbs[i].useGravity;
				this.rbsConstraints[i] = this.myRbs[i].constraints;
				this.myRbs[i].linearVelocity = Vector3.zero;
				this.myRbs[i].angularVelocity = Vector3.zero;
				this.myRbs[i].useGravity = false;
				this.myRbs[i].constraints = RigidbodyConstraints.FreezeAll;
				this.myRbs[i].detectCollisions = false;
			}
			for (int j = 0; j < this.myRbs2D.Length; j++)
			{
				this.velBackup2D[j] = this.myRbs2D[j].linearVelocity;
				this.angularVelBackup2D[j] = this.myRbs2D[j].angularVelocity;
				this.rbs2DType[j] = this.myRbs2D[j].bodyType;
				this.rbs2DConstraints[j] = this.myRbs2D[j].constraints;
				this.myRbs2D[j].linearVelocity = Vector2.zero;
				this.myRbs2D[j].angularVelocity = 0f;
				this.myRbs2D[j].bodyType = RigidbodyType2D.Static;
				this.myRbs2D[j].constraints = RigidbodyConstraints2D.FreezeAll;
			}
			for (int k = 0; k < this.myAnimators.Length; k++)
			{
				this.animatorSpeedBackup[k] = this.myAnimators[k].speed;
				this.myAnimators[k].speed = 0f;
			}
			Pausable.Ev ev = this.onPause;
			if (ev == null)
			{
				return;
			}
			ev();
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00073370 File Offset: 0x00071570
		private void _UnpauseMe()
		{
			this._isPaused = false;
			for (int i = 0; i < this.myRbs.Length; i++)
			{
				this.myRbs[i].linearVelocity = this.velBackup[i];
				this.myRbs[i].angularVelocity = this.angularVelBackup[i];
				this.myRbs[i].useGravity = this.useGravityBackup[i];
				this.myRbs[i].constraints = this.rbsConstraints[i];
				this.myRbs[i].detectCollisions = true;
			}
			for (int j = 0; j < this.myRbs2D.Length; j++)
			{
				this.myRbs2D[j].linearVelocity = this.velBackup2D[j];
				this.myRbs2D[j].angularVelocity = this.angularVelBackup2D[j];
				this.myRbs2D[j].bodyType = this.rbs2DType[j];
				this.myRbs2D[j].constraints = this.rbs2DConstraints[j];
			}
			for (int k = 0; k < this.myAnimators.Length; k++)
			{
				this.myAnimators[k].speed = this.animatorSpeedBackup[k];
			}
			Pausable.Ev ev = this.onResume;
			if (ev == null)
			{
				return;
			}
			ev();
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x000734A4 File Offset: 0x000716A4
		private void Awake()
		{
			if (this.canPauseChilds)
			{
				this.myRbs = base.GetComponentsInChildren<Rigidbody>(true);
				this.myRbs2D = base.GetComponentsInChildren<Rigidbody2D>(true);
				this.myAnimators = base.GetComponentsInChildren<Animator>(true);
			}
			else
			{
				this.myRbs = base.GetComponents<Rigidbody>();
				this.myRbs2D = base.GetComponents<Rigidbody2D>();
				this.myAnimators = base.GetComponents<Animator>();
			}
			if (this.myRbs != null && this.myRbs.Length != 0)
			{
				this.velBackup = new Vector3[this.myRbs.Length];
				this.angularVelBackup = new Vector3[this.myRbs.Length];
				this.useGravityBackup = new bool[this.myRbs.Length];
				this.rbsConstraints = new RigidbodyConstraints[this.myRbs.Length];
			}
			if (this.myRbs2D != null && this.myRbs2D.Length != 0)
			{
				this.velBackup2D = new Vector2[this.myRbs2D.Length];
				this.angularVelBackup2D = new float[this.myRbs2D.Length];
				this.rbs2DType = new RigidbodyType2D[this.myRbs2D.Length];
				this.rbs2DConstraints = new RigidbodyConstraints2D[this.myRbs2D.Length];
			}
			if (this.myAnimators != null && this.myAnimators.Length != 0)
			{
				this.animatorSpeedBackup = new float[this.myAnimators.Length];
			}
			for (int i = 0; i < this.myAnimators.Length; i++)
			{
				this.myAnimators[i].keepAnimatorStateOnDisable = true;
			}
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00013E99 File Offset: 0x00012099
		private void OnDisable()
		{
			this._UnpauseMe();
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00073608 File Offset: 0x00071808
		public void Update()
		{
			this.localPauseTimer -= Tick.Time;
			if (Tick.IsGameRunning && this.localPauseTimer < 0f && this._isPaused)
			{
				this._UnpauseMe();
				return;
			}
			if ((!Tick.IsGameRunning || this.localPauseTimer > 0f) && !this._isPaused && this.isPausable)
			{
				this._PauseMe();
			}
		}

		// Token: 0x040011EC RID: 4588
		[NonSerialized]
		public Rigidbody[] myRbs;

		// Token: 0x040011ED RID: 4589
		[NonSerialized]
		public Rigidbody2D[] myRbs2D;

		// Token: 0x040011EE RID: 4590
		[NonSerialized]
		public Animator[] myAnimators;

		// Token: 0x040011EF RID: 4591
		[NonSerialized]
		public float localPauseTimer;

		// Token: 0x040011F0 RID: 4592
		public bool canPauseChilds = true;

		// Token: 0x040011F1 RID: 4593
		private bool _isPaused;

		// Token: 0x040011F2 RID: 4594
		private bool isPausable = true;

		// Token: 0x040011F3 RID: 4595
		[NonSerialized]
		public Vector3[] velBackup;

		// Token: 0x040011F4 RID: 4596
		[NonSerialized]
		public Vector3[] angularVelBackup;

		// Token: 0x040011F5 RID: 4597
		[NonSerialized]
		public bool[] useGravityBackup;

		// Token: 0x040011F6 RID: 4598
		[NonSerialized]
		public RigidbodyConstraints[] rbsConstraints;

		// Token: 0x040011F7 RID: 4599
		[NonSerialized]
		public Vector2[] velBackup2D;

		// Token: 0x040011F8 RID: 4600
		[NonSerialized]
		public float[] angularVelBackup2D;

		// Token: 0x040011F9 RID: 4601
		[NonSerialized]
		public RigidbodyType2D[] rbs2DType;

		// Token: 0x040011FA RID: 4602
		[NonSerialized]
		public RigidbodyConstraints2D[] rbs2DConstraints;

		// Token: 0x040011FB RID: 4603
		[NonSerialized]
		public float[] animatorSpeedBackup;

		// Token: 0x040011FC RID: 4604
		public Pausable.Ev onPause;

		// Token: 0x040011FD RID: 4605
		public Pausable.Ev onResume;

		// Token: 0x02000173 RID: 371
		// (Invoke) Token: 0x06001108 RID: 4360
		public delegate void Ev();
	}
}
