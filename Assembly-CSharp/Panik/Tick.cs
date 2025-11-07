using System;
using UnityEngine;

namespace Panik
{
	public static class Tick
	{
		// (get) Token: 0x06000D67 RID: 3431 RVA: 0x0005526E File Offset: 0x0005346E
		public static float Time
		{
			get
			{
				return Tick._time;
			}
		}

		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x00055275 File Offset: 0x00053475
		public static float TimeFixed
		{
			get
			{
				return Tick._timeFixed;
			}
		}

		// (get) Token: 0x06000D69 RID: 3433 RVA: 0x0005527C File Offset: 0x0005347C
		public static float TimeUnscaled
		{
			get
			{
				return Tick._timeUnscaled;
			}
		}

		// (get) Token: 0x06000D6A RID: 3434 RVA: 0x00055283 File Offset: 0x00053483
		public static float TimeFixedUnscaled
		{
			get
			{
				return Tick._timeFixedUnscaled;
			}
		}

		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x0005528A File Offset: 0x0005348A
		public static float PassedTime
		{
			get
			{
				return Tick._timePassed;
			}
		}

		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x00055291 File Offset: 0x00053491
		public static float PassedTimePausable
		{
			get
			{
				return Tick._timePassedPausable;
			}
		}

		// (get) Token: 0x06000D6D RID: 3437 RVA: 0x00055298 File Offset: 0x00053498
		public static float PassedTimeUnscaled
		{
			get
			{
				return Tick._timePassedUnscaled;
			}
		}

		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x0005529F File Offset: 0x0005349F
		public static float PassedTimePausableUnscaled
		{
			get
			{
				return Tick._timePassedPausableUnscaled;
			}
		}

		// (get) Token: 0x06000D6F RID: 3439 RVA: 0x000552A6 File Offset: 0x000534A6
		public static float PassedTime01
		{
			get
			{
				return Tick._timePassed01;
			}
		}

		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x000552AD File Offset: 0x000534AD
		public static float PassedTimePausable01
		{
			get
			{
				return Tick._timePassedPausable01;
			}
		}

		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x000552B4 File Offset: 0x000534B4
		public static float PassedTimeUnscaled01
		{
			get
			{
				return Tick._timePassedUnscaled01;
			}
		}

		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x000552BB File Offset: 0x000534BB
		public static float PassedTimePausableUnscaled01
		{
			get
			{
				return Tick._timePassedPausableUnscaled01;
			}
		}

		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x000552C2 File Offset: 0x000534C2
		// (set) Token: 0x06000D74 RID: 3444 RVA: 0x000552C9 File Offset: 0x000534C9
		public static float FreezeTimer
		{
			get
			{
				return Tick._freezeTime;
			}
			set
			{
				if (Tick._freezeTime < value)
				{
					Tick._freezeTime = value;
				}
			}
		}

		// (get) Token: 0x06000D75 RID: 3445 RVA: 0x000552D9 File Offset: 0x000534D9
		// (set) Token: 0x06000D76 RID: 3446 RVA: 0x000552E0 File Offset: 0x000534E0
		public static bool Paused
		{
			get
			{
				return Tick._paused;
			}
			set
			{
				Tick._paused = value;
			}
		}

		// (get) Token: 0x06000D77 RID: 3447 RVA: 0x000552E8 File Offset: 0x000534E8
		public static bool IsGameRunning
		{
			get
			{
				return !Tick._paused && Tick._freezeTime <= 0f;
			}
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00055304 File Offset: 0x00053504
		public static void Routine()
		{
			Tick._time = global::UnityEngine.Time.deltaTime;
			Tick._timeFixed = global::UnityEngine.Time.fixedDeltaTime;
			Tick._timeUnscaled = global::UnityEngine.Time.unscaledDeltaTime;
			Tick._timeFixedUnscaled = global::UnityEngine.Time.fixedUnscaledDeltaTime;
			Tick._freezeTime -= Tick._time;
			Tick._timePassed += Tick._time;
			Tick._timePassedUnscaled += Tick._timeUnscaled;
			Tick._timePassed01 += Tick._time / Tick._timeFixed;
			Tick._timePassedUnscaled01 += Tick._timeUnscaled / Tick._timeFixedUnscaled;
			if (Tick.IsGameRunning)
			{
				Tick._timePassedPausable += Tick._time;
				Tick._timePassedPausableUnscaled += Tick._timeUnscaled;
				Tick._timePassedPausable01 += Tick._time / Tick._timeFixed;
				Tick._timePassedPausableUnscaled01 += Tick._timeUnscaled / Tick._timeFixedUnscaled;
			}
			if (Tick._timePassed01 > 1f)
			{
				Tick._timePassed01 -= 1f;
			}
			if (Tick._timePassedPausable01 > 1f)
			{
				Tick._timePassedPausable01 -= 1f;
			}
			if (Tick._timePassedUnscaled01 > 1f)
			{
				Tick._timePassedUnscaled01 -= 1f;
			}
			if (Tick._timePassedPausableUnscaled01 > 1f)
			{
				Tick._timePassedPausableUnscaled01 -= 1f;
			}
		}

		public const float HZ_1 = 360f;

		public const float HZ_2 = 720f;

		public const float HZ_3 = 1080f;

		public const float HZ_4 = 1440f;

		public const float HZ_5 = 1800f;

		public const float HZ_6 = 2160f;

		public const float HZ_7 = 2520f;

		public const float HZ_8 = 2880f;

		private static float _time;

		private static float _timeFixed;

		private static float _timeUnscaled;

		private static float _timeFixedUnscaled;

		private static float _timePassed;

		private static float _timePassedPausable;

		private static float _timePassedUnscaled;

		private static float _timePassedPausableUnscaled;

		private static float _timePassed01;

		private static float _timePassedPausable01;

		private static float _timePassedUnscaled01;

		private static float _timePassedPausableUnscaled01;

		private static bool _paused;

		private static float _freezeTime;
	}
}
