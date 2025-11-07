using System;
using UnityEngine;

namespace Panik
{
	public class BounceScript : MonoBehaviour
	{
		// Token: 0x06000A90 RID: 2704 RVA: 0x0004828E File Offset: 0x0004648E
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

		// Token: 0x06000A91 RID: 2705 RVA: 0x000482AF File Offset: 0x000464AF
		public void SetBounceScaleDecaySpeed(float decaySpeed)
		{
			this.additionalScaleDecay = decaySpeed;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x000482B8 File Offset: 0x000464B8
		public void SetBouncesPerSecond(float bouncesPerSecond)
		{
			if (!base.enabled)
			{
				base.enabled = true;
			}
			this.bouncesPerSecond = bouncesPerSecond;
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x000482D0 File Offset: 0x000464D0
		public void SetBouncesPerSecondDecaySpeed(float decaySpeed)
		{
			this.frequencyDecay = decaySpeed;
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x000482D9 File Offset: 0x000464D9
		public void ResetBounceFrequency()
		{
			this.bouncesPerSecond = this._bouncesPerSecondResetValue;
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x000482E7 File Offset: 0x000464E7
		private void Awake()
		{
			this._additionalScaleResetValue = this.additionalScale;
			this._bouncesPerSecondResetValue = this.bouncesPerSecond;
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00048304 File Offset: 0x00046504
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

		// Token: 0x06000A97 RID: 2711 RVA: 0x00048369 File Offset: 0x00046569
		private void OnDisable()
		{
			if (this.frequencyResets)
			{
				this.ResetBounceFrequency();
			}
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0004837C File Offset: 0x0004657C
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

		public Vector3 baseScale = Vector3.one;

		public Vector3 applicationMultiplier = Vector3.one;

		public bool pausable = true;

		public bool resetOnEnable = true;

		public bool syncTime;

		[NonSerialized]
		public float passedTime;

		public float additionalScale = 0.1f;

		private float _additionalScaleResetValue;

		public float additionalScaleDecay;

		public float bouncesPerSecond = 4f;

		private float _bouncesPerSecondResetValue;

		public bool frequencyResets = true;

		public float frequencyDecay;

		private float calculationAng;

		private float calculationCos;

		private float calculationSin;
	}
}
