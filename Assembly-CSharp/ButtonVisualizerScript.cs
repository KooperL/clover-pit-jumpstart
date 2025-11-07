using System;
using Panik;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVisualizerScript : MonoBehaviour
{
	// Token: 0x0600083D RID: 2109 RVA: 0x00035D54 File Offset: 0x00033F54
	public void Press()
	{
		if (this.runningAnimation)
		{
			return;
		}
		this.AnimationReset(true);
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x00035D66 File Offset: 0x00033F66
	private void AnimationReset(bool runningState)
	{
		this.runningAnimation = runningState;
		this.animDirection = 1f;
		this.animTimer = 0f;
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x00035D85 File Offset: 0x00033F85
	private void Reset()
	{
		this.myElement = base.GetComponentInParent<DiegeticMenuElement>();
		this.myButtonTransform = base.transform;
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x00035DA0 File Offset: 0x00033FA0
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

	// Token: 0x06000841 RID: 2113 RVA: 0x00035E30 File Offset: 0x00034030
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

	// Token: 0x06000842 RID: 2114 RVA: 0x00035F04 File Offset: 0x00034104
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.myButtonTransform.position, this.myButtonTransform.position + this.targetLocalPosOffset);
		Gizmos.DrawWireSphere(this.myButtonTransform.position + this.targetLocalPosOffset, 0.1f);
	}

	public const float ANIM_SPEED = 8f;

	public DiegeticMenuElement myElement;

	public Transform myButtonTransform;

	private Vector3 buttonStartingLocalPosition;

	private Vector3 buttonStartingLocalEuler;

	public Vector3 targetLocalPosOffset;

	public Vector3 targetLocalEuler;

	private bool runningAnimation;

	private float animTimer;

	private float animDirection;

	public bool selfDetect = true;
}
