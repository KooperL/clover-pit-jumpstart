using System;
using UnityEngine;
using UnityEngine.UI;

namespace Panik
{
	public class FrameAnimator : MonoBehaviour
	{
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x000492DE File Offset: 0x000474DE
		// (set) Token: 0x06000ACE RID: 2766 RVA: 0x000492E6 File Offset: 0x000474E6
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

		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x000492F5 File Offset: 0x000474F5
		// (set) Token: 0x06000AD0 RID: 2768 RVA: 0x000492FD File Offset: 0x000474FD
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

		// (get) Token: 0x06000AD1 RID: 2769 RVA: 0x0004931D File Offset: 0x0004751D
		// (set) Token: 0x06000AD2 RID: 2770 RVA: 0x0004932C File Offset: 0x0004752C
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

		// (get) Token: 0x06000AD3 RID: 2771 RVA: 0x00049388 File Offset: 0x00047588
		// (set) Token: 0x06000AD4 RID: 2772 RVA: 0x000493C0 File Offset: 0x000475C0
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

		// Token: 0x06000AD5 RID: 2773 RVA: 0x000493D0 File Offset: 0x000475D0
		public Sprite GetCurrentSprite()
		{
			return this.frameAnimationCurrent.frames[this._frameIndex];
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x000493E4 File Offset: 0x000475E4
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

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0004952C File Offset: 0x0004772C
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

		// Token: 0x06000AD8 RID: 2776 RVA: 0x000495E0 File Offset: 0x000477E0
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

		// Token: 0x06000AD9 RID: 2777 RVA: 0x0004968B File Offset: 0x0004788B
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

		// (Invoke) Token: 0x060012A2 RID: 4770
		public delegate void Ev(FrameAnimator self);
	}
}
