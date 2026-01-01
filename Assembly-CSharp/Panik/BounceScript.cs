using System;
using UnityEngine;

namespace Panik
{
	// Token: 0x02000107 RID: 263
	public class BounceScript : MonoBehaviour
	{
		// Token: 0x06000C7E RID: 3198 RVA: 0x0001039C File Offset: 0x0000E59C
		public void SetBounceScale(float force)
		{
			if (!base.enabled)
			{
				base.enabled = true;
			}
			if (force > this.additionalScale)
			{
				this.additionalScale = force;
			}
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x000103BD File Offset: 0x0000E5BD
		public void SetBounceScaleDecaySpeed(float decaySpeed)
		{
			this.additionalScaleDecay = decaySpeed;
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x000103C6 File Offset: 0x0000E5C6
		public void SetBouncesPerSecond(float bouncesPerSecond)
		{
			if (!base.enabled)
			{
				base.enabled = true;
			}
			this.bouncesPerSecond = bouncesPerSecond;
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x000103DE File Offset: 0x0000E5DE
		public void SetBouncesPerSecondDecaySpeed(float decaySpeed)
		{
			this.frequencyDecay = decaySpeed;
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x000103E7 File Offset: 0x0000E5E7
		public void ResetBounceFrequency()
		{
			this.bouncesPerSecond = this._bouncesPerSecondResetValue;
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x000103F5 File Offset: 0x0000E5F5
		private void Awake()
		{
			this._additionalScaleResetValue = this.additionalScale;
			this._bouncesPerSecondResetValue = this.bouncesPerSecond;
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x00062DC0 File Offset: 0x00060FC0
		private void OnEnable()
		{
			if (this.resetOnEnable)
			{
				this.additionalScale = this._additionalScaleResetValue;
				this._bouncesPerSecondResetValue = this.bouncesPerSecond;
				base.transform.localScale = this.baseScale;
			}
			if (this.syncTime)
			{
				if (this.pausable)
				{
					this.passedTime = Tick.PassedTimePausable;
					return;
				}
				this.passedTime = Tick.PassedTime;
			}
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0001040F File Offset: 0x0000E60F
		private void OnDisable()
		{
			if (this.frequencyResets)
			{
				this.ResetBounceFrequency();
			}
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x00062E28 File Offset: 0x00061028
		public void Update()
		{
			if (this.pausable && !Tick.IsGameRunning)
			{
				return;
			}
			this.passedTime += Tick.Time;
			this.additionalScale = Mathf.Max(0f, this.additionalScale - this.additionalScaleDecay * Tick.Time);
			this.bouncesPerSecond = Mathf.Max(0f, this.bouncesPerSecond - this.frequencyDecay * Tick.Time);
			this.calculationAng = this.passedTime * 360f * this.bouncesPerSecond;
			this.calculationCos = Util.AngleCos(this.calculationAng);
			this.calculationSin = Util.AngleSin(this.calculationAng);
			base.transform.localScale = this.baseScale + this.additionalScale * new Vector3(this.calculationCos * this.applicationMultiplier.x, -this.calculationSin * this.applicationMultiplier.y, this.calculationSin * this.applicationMultiplier.z);
			if (this.additionalScale <= 0f && this.bouncesPerSecond <= 0f)
			{
				base.enabled = false;
			}
		}

		// Token: 0x04000D4C RID: 3404
		public Vector3 baseScale = Vector3.one;

		// Token: 0x04000D4D RID: 3405
		public Vector3 applicationMultiplier = Vector3.one;

		// Token: 0x04000D4E RID: 3406
		public bool pausable = true;

		// Token: 0x04000D4F RID: 3407
		public bool resetOnEnable = true;

		// Token: 0x04000D50 RID: 3408
		public bool syncTime;

		// Token: 0x04000D51 RID: 3409
		[NonSerialized]
		public float passedTime;

		// Token: 0x04000D52 RID: 3410
		public float additionalScale = 0.1f;

		// Token: 0x04000D53 RID: 3411
		private float _additionalScaleResetValue;

		// Token: 0x04000D54 RID: 3412
		public float additionalScaleDecay;

		// Token: 0x04000D55 RID: 3413
		public float bouncesPerSecond = 4f;

		// Token: 0x04000D56 RID: 3414
		private float _bouncesPerSecondResetValue;

		// Token: 0x04000D57 RID: 3415
		public bool frequencyResets = true;

		// Token: 0x04000D58 RID: 3416
		public float frequencyDecay;

		// Token: 0x04000D59 RID: 3417
		private float calculationAng;

		// Token: 0x04000D5A RID: 3418
		private float calculationCos;

		// Token: 0x04000D5B RID: 3419
		private float calculationSin;
	}
}
