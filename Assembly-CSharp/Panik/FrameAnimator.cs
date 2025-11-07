using System;
using UnityEngine;
using UnityEngine.UI;

namespace Panik
{
	public class FrameAnimator : MonoBehaviour
	{
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00048B7E File Offset: 0x00046D7E
		// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x00048B86 File Offset: 0x00046D86
		public int FrameIndex
		{
			get
			{
				return this._frameIndex;
			}
			set
			{
				this._frameIndex = value;
				this.UpdateRenderer();
			}
		}

		// (get) Token: 0x06000ABA RID: 2746 RVA: 0x00048B95 File Offset: 0x00046D95
		// (set) Token: 0x06000ABB RID: 2747 RVA: 0x00048B9D File Offset: 0x00046D9D
		public FrameAnimation Animation
		{
			get
			{
				return this.frameAnimationCurrent;
			}
			set
			{
				this.frameAnimationCurrent = value;
				this.UpdateRenderer();
				FrameAnimator.Ev ev = this.onAnimationChange;
				if (ev == null)
				{
					return;
				}
				ev(this);
			}
		}

		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x00048BBD File Offset: 0x00046DBD
		// (set) Token: 0x06000ABD RID: 2749 RVA: 0x00048BCC File Offset: 0x00046DCC
		public string AnimationName
		{
			get
			{
				return this.frameAnimationCurrent.name;
			}
			set
			{
				for (int i = 0; i < this.animations.Length; i++)
				{
					if (this.animations[i].name == value)
					{
						this.Animation = this.animations[i];
						return;
					}
				}
				Debug.LogWarning("<color=red>Cannot find any frameAnimation inside the animator. You searched for the frameAnimation called: " + value + "</color>");
			}
		}

		// (get) Token: 0x06000ABE RID: 2750 RVA: 0x00048C28 File Offset: 0x00046E28
		// (set) Token: 0x06000ABF RID: 2751 RVA: 0x00048C60 File Offset: 0x00046E60
		public int AnimationIndex
		{
			get
			{
				for (int i = 0; i < this.animations.Length; i++)
				{
					if (this.animations[i] == this.frameAnimationCurrent)
					{
						return i;
					}
				}
				return -1;
			}
			set
			{
				this.Animation = this.animations[value];
			}
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00048C70 File Offset: 0x00046E70
		public Sprite GetCurrentSprite()
		{
			return this.frameAnimationCurrent.frames[this._frameIndex];
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00048C84 File Offset: 0x00046E84
		private void UpdateRenderer()
		{
			if (this._frameIndex < 0)
			{
				this._frameIndex = this.frameAnimationCurrent.frames.Length - 1;
				if (this._actualframeSpd < 0f)
				{
					FrameAnimator.Ev ev = this.onAnimationEnd;
					if (ev != null)
					{
						ev(this);
					}
				}
			}
			if (this._frameIndex > this.frameAnimationCurrent.frames.Length - 1)
			{
				this._frameIndex = 0;
				if (this._actualframeSpd > 0f)
				{
					FrameAnimator.Ev ev2 = this.onAnimationEnd;
					if (ev2 != null)
					{
						ev2(this);
					}
				}
			}
			if (this.mySpriteRenderer != null)
			{
				this.mySpriteRenderer.sprite = this.frameAnimationCurrent.frames[this._frameIndex];
				FrameAnimator.Ev ev3 = this.onRendererUpdate;
				if (ev3 == null)
				{
					return;
				}
				ev3(this);
				return;
			}
			else if (this.myImageRenderer != null)
			{
				this.myImageRenderer.sprite = this.frameAnimationCurrent.frames[this._frameIndex];
				FrameAnimator.Ev ev4 = this.onRendererUpdate;
				if (ev4 == null)
				{
					return;
				}
				ev4(this);
				return;
			}
			else
			{
				if (!(this.myGenericRenderer != null))
				{
					return;
				}
				this.myGenericRenderer.sharedMaterial.mainTexture = this.frameAnimationCurrent.frames[this._frameIndex].texture;
				FrameAnimator.Ev ev5 = this.onRendererUpdate;
				if (ev5 == null)
				{
					return;
				}
				ev5(this);
				return;
			}
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00048DCC File Offset: 0x00046FCC
		private void Awake()
		{
			this.mySpriteRenderer = base.GetComponent<SpriteRenderer>();
			if (this.mySpriteRenderer == null)
			{
				this.myImageRenderer = base.GetComponent<Image>();
			}
			if (this.myImageRenderer == null)
			{
				this.myGenericRenderer = base.GetComponent<Renderer>();
			}
			if (this.defaultAnimation == null)
			{
				if (this.animations.Length != 0)
				{
					this.defaultAnimation = this.animations[0];
				}
				else
				{
					Debug.LogError("<color=red>Cannot find any frameAnimation inside the animator. You searched for the frameAnimation called: " + this.defaultAnimation.name + "</color>");
				}
			}
			if (this.defaultAnimation != null)
			{
				this.frameAnimationCurrent = this.defaultAnimation;
				this.UpdateRenderer();
			}
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00048E80 File Offset: 0x00047080
		private void Update()
		{
			if (this.autoPause && !Tick.IsGameRunning)
			{
				return;
			}
			if (this.frameAnimationCurrent == null)
			{
				return;
			}
			this._actualframeSpd = this.frameSpeed * this.frameSpeedMult;
			this._frameTimer -= Tick.Time * Mathf.Abs(this._actualframeSpd);
			if (this._frameTimer <= 0f)
			{
				this._frameTimer += 1f;
				this.FrameIndex += ((this._actualframeSpd >= 0f) ? 1 : (-1));
				FrameAnimator.Ev ev = this.onAnimationFrameChange;
				if (ev == null)
				{
					return;
				}
				ev(this);
			}
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00048F2B File Offset: 0x0004712B
		private void OnDrawGizmosSelected()
		{
			if (this.defaultAnimation == null && this.animations.Length != 0)
			{
				this.defaultAnimation = this.animations[0];
			}
		}

		private SpriteRenderer mySpriteRenderer;

		private Image myImageRenderer;

		private Renderer myGenericRenderer;

		private const bool IGNORE_GENERIC_RENDERER = true;

		public bool autoPause = true;

		private FrameAnimation frameAnimationCurrent;

		private int _frameIndex;

		public float frameSpeed = 12f;

		[NonSerialized]
		public float frameSpeedMult = 1f;

		private float _actualframeSpd;

		private float _frameTimer = 1f;

		public FrameAnimation defaultAnimation;

		public FrameAnimation[] animations;

		public FrameAnimator.Ev onRendererUpdate;

		public FrameAnimator.Ev onAnimationEnd;

		public FrameAnimator.Ev onAnimationChange;

		public FrameAnimator.Ev onAnimationFrameChange;

		// (Invoke) Token: 0x0600128B RID: 4747
		public delegate void Ev(FrameAnimator self);
	}
}
