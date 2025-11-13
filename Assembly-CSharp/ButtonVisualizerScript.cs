using System;
using Panik;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVisualizerScript : MonoBehaviour
{
	// Token: 0x06000844 RID: 2116 RVA: 0x00035F3C File Offset: 0x0003413C
	public void Press()
	{
		if (this.runningAnimation)
		{
			return;
		}
		this.AnimationReset(true);
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x00035F4E File Offset: 0x0003414E
	private void AnimationReset(bool runningState)
	{
		this.runningAnimation = runningState;
		this.animDirection = 1f;
		this.animTimer = 0f;
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x00035F6D File Offset: 0x0003416D
	private void Reset()
	{
		this.myElement = base.GetComponentInParent<DiegeticMenuElement>();
		this.myButtonTransform = base.transform;
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x00035F88 File Offset: 0x00034188
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

	// Token: 0x06000848 RID: 2120 RVA: 0x00036018 File Offset: 0x00034218
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

	// Token: 0x06000849 RID: 2121 RVA: 0x000360EC File Offset: 0x000342EC
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
