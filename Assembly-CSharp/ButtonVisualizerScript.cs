using System;
using Panik;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000A8 RID: 168
public class ButtonVisualizerScript : MonoBehaviour
{
	// Token: 0x06000967 RID: 2407 RVA: 0x0000D667 File Offset: 0x0000B867
	public void Press()
	{
		if (this.runningAnimation)
		{
			return;
		}
		this.AnimationReset(true);
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x0000D679 File Offset: 0x0000B879
	private void AnimationReset(bool runningState)
	{
		this.runningAnimation = runningState;
		this.animDirection = 1f;
		this.animTimer = 0f;
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0000D698 File Offset: 0x0000B898
	private void Reset()
	{
		this.myElement = base.GetComponentInParent<DiegeticMenuElement>();
		this.myButtonTransform = base.transform;
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x0004D4B8 File Offset: 0x0004B6B8
	private void Awake()
	{
		this.myElement = base.GetComponentInParent<DiegeticMenuElement>();
		this.buttonStartingLocalPosition = this.myButtonTransform.localPosition;
		this.buttonStartingLocalEuler = this.myButtonTransform.localEulerAngles;
		this.targetLocalPosOffset += this.buttonStartingLocalPosition;
		this.targetLocalEuler += this.buttonStartingLocalEuler;
		if (this.selfDetect)
		{
			this.myElement.onSelectCallback.AddListener(new UnityAction(this.Press));
		}
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x0004D548 File Offset: 0x0004B748
	private void Update()
	{
		if (!Tick.IsGameRunning)
		{
			return;
		}
		if (this.runningAnimation)
		{
			this.animTimer += 8f * this.animDirection * Tick.Time;
			if (this.animTimer >= 1f && this.animDirection > 0f)
			{
				this.animDirection = -1f;
				this.animTimer = 1f;
			}
			if (this.animTimer <= 0f && this.animDirection < 0f)
			{
				this.AnimationReset(false);
			}
			this.myButtonTransform.localPosition = Vector3.Lerp(this.buttonStartingLocalPosition, this.targetLocalPosOffset, this.animTimer);
			this.myButtonTransform.localEulerAngles = Vector3.Lerp(this.buttonStartingLocalEuler, this.targetLocalEuler, this.animTimer);
		}
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0004D61C File Offset: 0x0004B81C
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.myButtonTransform.position, this.myButtonTransform.position + this.targetLocalPosOffset);
		Gizmos.DrawWireSphere(this.myButtonTransform.position + this.targetLocalPosOffset, 0.1f);
	}

	// Token: 0x0400095A RID: 2394
	public const float ANIM_SPEED = 8f;

	// Token: 0x0400095B RID: 2395
	public DiegeticMenuElement myElement;

	// Token: 0x0400095C RID: 2396
	public Transform myButtonTransform;

	// Token: 0x0400095D RID: 2397
	private Vector3 buttonStartingLocalPosition;

	// Token: 0x0400095E RID: 2398
	private Vector3 buttonStartingLocalEuler;

	// Token: 0x0400095F RID: 2399
	public Vector3 targetLocalPosOffset;

	// Token: 0x04000960 RID: 2400
	public Vector3 targetLocalEuler;

	// Token: 0x04000961 RID: 2401
	private bool runningAnimation;

	// Token: 0x04000962 RID: 2402
	private float animTimer;

	// Token: 0x04000963 RID: 2403
	private float animDirection;

	// Token: 0x04000964 RID: 2404
	public bool selfDetect = true;
}
