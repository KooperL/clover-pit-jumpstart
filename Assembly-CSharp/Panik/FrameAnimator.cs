using System;
using UnityEngine;
using UnityEngine.UI;

namespace Panik
{
	// Token: 0x0200010D RID: 269
	public class FrameAnimator : MonoBehaviour
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x000105AE File Offset: 0x0000E7AE
		// (set) Token: 0x06000CA8 RID: 3240 RVA: 0x000105B6 File Offset: 0x0000E7B6
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

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x000105C5 File Offset: 0x0000E7C5
		// (set) Token: 0x06000CAA RID: 3242 RVA: 0x000105CD File Offset: 0x0000E7CD
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

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000CAB RID: 3243 RVA: 0x000105ED File Offset: 0x0000E7ED
		// (set) Token: 0x06000CAC RID: 3244 RVA: 0x000634B4 File Offset: 0x000616B4
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

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000CAD RID: 3245 RVA: 0x00063510 File Offset: 0x00061710
		// (set) Token: 0x06000CAE RID: 3246 RVA: 0x000105FA File Offset: 0x0000E7FA
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

		// Token: 0x06000CAF RID: 3247 RVA: 0x0001060A File Offset: 0x0000E80A
		public Sprite GetCurrentSprite()
		{
			return this.frameAnimationCurrent.frames[this._frameIndex];
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00063548 File Offset: 0x00061748
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

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00063690 File Offset: 0x00061890
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

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00063744 File Offset: 0x00061944
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

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0001061E File Offset: 0x0000E81E
		private void OnDrawGizmosSelected()
		{
			if (this.defaultAnimation == null && this.animations.Length != 0)
			{
				this.defaultAnimation = this.animations[0];
			}
		}

		// Token: 0x04000D78 RID: 3448
		private SpriteRenderer mySpriteRenderer;

		// Token: 0x04000D79 RID: 3449
		private Image myImageRenderer;

		// Token: 0x04000D7A RID: 3450
		private Renderer myGenericRenderer;

		// Token: 0x04000D7B RID: 3451
		private const bool IGNORE_GENERIC_RENDERER = true;

		// Token: 0x04000D7C RID: 3452
		public bool autoPause = true;

		// Token: 0x04000D7D RID: 3453
		private FrameAnimation frameAnimationCurrent;

		// Token: 0x04000D7E RID: 3454
		private int _frameIndex;

		// Token: 0x04000D7F RID: 3455
		public float frameSpeed = 12f;

		// Token: 0x04000D80 RID: 3456
		[NonSerialized]
		public float frameSpeedMult = 1f;

		// Token: 0x04000D81 RID: 3457
		private float _actualframeSpd;

		// Token: 0x04000D82 RID: 3458
		private float _frameTimer = 1f;

		// Token: 0x04000D83 RID: 3459
		public FrameAnimation defaultAnimation;

		// Token: 0x04000D84 RID: 3460
		public FrameAnimation[] animations;

		// Token: 0x04000D85 RID: 3461
		public FrameAnimator.Ev onRendererUpdate;

		// Token: 0x04000D86 RID: 3462
		public FrameAnimator.Ev onAnimationEnd;

		// Token: 0x04000D87 RID: 3463
		public FrameAnimator.Ev onAnimationChange;

		// Token: 0x04000D88 RID: 3464
		public FrameAnimator.Ev onAnimationFrameChange;

		// Token: 0x0200010E RID: 270
		// (Invoke) Token: 0x06000CB6 RID: 3254
		public delegate void Ev(FrameAnimator self);
	}
}
