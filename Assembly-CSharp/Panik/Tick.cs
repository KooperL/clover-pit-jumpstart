using System;
using UnityEngine;

namespace Panik
{
	public static class Tick
	{
		// (get) Token: 0x06000D7E RID: 3454 RVA: 0x00055A4A File Offset: 0x00053C4A
		public static float Time
		{
			get
			{
				return Tick._time;
			}
		}

		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x00055A51 File Offset: 0x00053C51
		public static float TimeFixed
		{
			get
			{
				return Tick._timeFixed;
			}
		}

		// (get) Token: 0x06000D80 RID: 3456 RVA: 0x00055A58 File Offset: 0x00053C58
		public static float TimeUnscaled
		{
			get
			{
				return Tick._timeUnscaled;
			}
		}

		// (get) Token: 0x06000D81 RID: 3457 RVA: 0x00055A5F File Offset: 0x00053C5F
		public static float TimeFixedUnscaled
		{
			get
			{
				return Tick._timeFixedUnscaled;
			}
		}

		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x00055A66 File Offset: 0x00053C66
		public static float PassedTime
		{
			get
			{
				return Tick._timePassed;
			}
		}

		// (get) Token: 0x06000D83 RID: 3459 RVA: 0x00055A6D File Offset: 0x00053C6D
		public static float PassedTimePausable
		{
			get
			{
				return Tick._timePassedPausable;
			}
		}

		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x00055A74 File Offset: 0x00053C74
		public static float PassedTimeUnscaled
		{
			get
			{
				return Tick._timePassedUnscaled;
			}
		}

		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x00055A7B File Offset: 0x00053C7B
		public static float PassedTimePausableUnscaled
		{
			get
			{
				return Tick._timePassedPausableUnscaled;
			}
		}

		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x00055A82 File Offset: 0x00053C82
		public static float PassedTime01
		{
			get
			{
				return Tick._timePassed01;
			}
		}

		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x00055A89 File Offset: 0x00053C89
		public static float PassedTimePausable01
		{
			get
			{
				return Tick._timePassedPausable01;
			}
		}

		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x00055A90 File Offset: 0x00053C90
		public static float PassedTimeUnscaled01
		{
			get
			{
				return Tick._timePassedUnscaled01;
			}
		}

		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x00055A97 File Offset: 0x00053C97
		public static float PassedTimePausableUnscaled01
		{
			get
			{
				return Tick._timePassedPausableUnscaled01;
			}
		}

		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x00055A9E File Offset: 0x00053C9E
		// (set) Token: 0x06000D8B RID: 3467 RVA: 0x00055AA5 File Offset: 0x00053CA5
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

		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00055AB5 File Offset: 0x00053CB5
		// (set) Token: 0x06000D8D RID: 3469 RVA: 0x00055ABC File Offset: 0x00053CBC
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

		// (get) Token: 0x06000D8E RID: 3470 RVA: 0x00055AC4 File Offset: 0x00053CC4
		public static bool IsGameRunning
		{
			get
			{
				return !Tick._paused && Tick._freezeTime <= 0f;
			}
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x00055AE0 File Offset: 0x00053CE0
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
