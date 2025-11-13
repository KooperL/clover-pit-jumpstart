using System;
using UnityEngine;

namespace Panik
{
	public class BounceScript : MonoBehaviour
	{
		// Token: 0x06000AA5 RID: 2725 RVA: 0x000489EE File Offset: 0x00046BEE
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

		// Token: 0x06000AA6 RID: 2726 RVA: 0x00048A0F File Offset: 0x00046C0F
		public void SetBounceScaleDecaySpeed(float decaySpeed)
		{
			this.additionalScaleDecay = decaySpeed;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00048A18 File Offset: 0x00046C18
		public void SetBouncesPerSecond(float bouncesPerSecond)
		{
			if (!base.enabled)
			{
				base.enabled = true;
			}
			this.bouncesPerSecond = bouncesPerSecond;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00048A30 File Offset: 0x00046C30
		public void SetBouncesPerSecondDecaySpeed(float decaySpeed)
		{
			this.frequencyDecay = decaySpeed;
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00048A39 File Offset: 0x00046C39
		public void ResetBounceFrequency()
		{
			this.bouncesPerSecond = this._bouncesPerSecondResetValue;
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x00048A47 File Offset: 0x00046C47
		private void Awake()
		{
			this._additionalScaleResetValue = this.additionalScale;
			this._bouncesPerSecondResetValue = this.bouncesPerSecond;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00048A64 File Offset: 0x00046C64
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

		// Token: 0x06000AAC RID: 2732 RVA: 0x00048AC9 File Offset: 0x00046CC9
		private void OnDisable()
		{
			if (this.frequencyResets)
			{
				this.ResetBounceFrequency();
			}
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00048ADC File Offset: 0x00046CDC
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
