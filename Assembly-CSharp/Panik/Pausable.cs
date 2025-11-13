using System;
using UnityEngine;

namespace Panik
{
	public class Pausable : MonoBehaviour
	{
		// Token: 0x06000D75 RID: 3445 RVA: 0x00055538 File Offset: 0x00053738
		public bool PausedGet()
		{
			return this._isPaused;
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00055540 File Offset: 0x00053740
		public void PausableSet(bool isPausable)
		{
			this.isPausable = isPausable;
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x00055549 File Offset: 0x00053749
		public bool PausableGet()
		{
			return this.isPausable;
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00055554 File Offset: 0x00053754
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
				this.myRbs[i].constraints = 126;
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
				this.myRbs2D[j].bodyType = 2;
				this.myRbs2D[j].constraints = 7;
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

		// Token: 0x06000D79 RID: 3449 RVA: 0x00055728 File Offset: 0x00053928
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

		// Token: 0x06000D7A RID: 3450 RVA: 0x0005585C File Offset: 0x00053A5C
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

		// Token: 0x06000D7B RID: 3451 RVA: 0x000559BD File Offset: 0x00053BBD
		private void OnDisable()
		{
			this._UnpauseMe();
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x000559C8 File Offset: 0x00053BC8
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

		[NonSerialized]
		public Rigidbody[] myRbs;

		[NonSerialized]
		public Rigidbody2D[] myRbs2D;

		[NonSerialized]
		public Animator[] myAnimators;

		[NonSerialized]
		public float localPauseTimer;

		public bool canPauseChilds = true;

		private bool _isPaused;

		private bool isPausable = true;

		[NonSerialized]
		public Vector3[] velBackup;

		[NonSerialized]
		public Vector3[] angularVelBackup;

		[NonSerialized]
		public bool[] useGravityBackup;

		[NonSerialized]
		public RigidbodyConstraints[] rbsConstraints;

		[NonSerialized]
		public Vector2[] velBackup2D;

		[NonSerialized]
		public float[] angularVelBackup2D;

		[NonSerialized]
		public RigidbodyType2D[] rbs2DType;

		[NonSerialized]
		public RigidbodyConstraints2D[] rbs2DConstraints;

		[NonSerialized]
		public float[] animatorSpeedBackup;

		public Pausable.Ev onPause;

		public Pausable.Ev onResume;

		// (Invoke) Token: 0x06001449 RID: 5193
		public delegate void Ev();
	}
}
