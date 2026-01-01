using System;
using UnityEngine;

namespace Panik
{
	// Token: 0x02000174 RID: 372
	public static class Tick
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600110B RID: 4363 RVA: 0x00013EB7 File Offset: 0x000120B7
		public static float Time
		{
			get
			{
				return Tick._time;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600110C RID: 4364 RVA: 0x00013EBE File Offset: 0x000120BE
		public static float TimeFixed
		{
			get
			{
				return Tick._timeFixed;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600110D RID: 4365 RVA: 0x00013EC5 File Offset: 0x000120C5
		public static float TimeUnscaled
		{
			get
			{
				return Tick._timeUnscaled;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600110E RID: 4366 RVA: 0x00013ECC File Offset: 0x000120CC
		public static float TimeFixedUnscaled
		{
			get
			{
				return Tick._timeFixedUnscaled;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x00013ED3 File Offset: 0x000120D3
		public static float PassedTime
		{
			get
			{
				return Tick._timePassed;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x00013EDA File Offset: 0x000120DA
		public static float PassedTimePausable
		{
			get
			{
				return Tick._timePassedPausable;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x00013EE1 File Offset: 0x000120E1
		public static float PassedTimeUnscaled
		{
			get
			{
				return Tick._timePassedUnscaled;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06001112 RID: 4370 RVA: 0x00013EE8 File Offset: 0x000120E8
		public static float PassedTimePausableUnscaled
		{
			get
			{
				return Tick._timePassedPausableUnscaled;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x00013EEF File Offset: 0x000120EF
		public static float PassedTime01
		{
			get
			{
				return Tick._timePassed01;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06001114 RID: 4372 RVA: 0x00013EF6 File Offset: 0x000120F6
		public static float PassedTimePausable01
		{
			get
			{
				return Tick._timePassedPausable01;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06001115 RID: 4373 RVA: 0x00013EFD File Offset: 0x000120FD
		public static float PassedTimeUnscaled01
		{
			get
			{
				return Tick._timePassedUnscaled01;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06001116 RID: 4374 RVA: 0x00013F04 File Offset: 0x00012104
		public static float PassedTimePausableUnscaled01
		{
			get
			{
				return Tick._timePassedPausableUnscaled01;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06001117 RID: 4375 RVA: 0x00013F0B File Offset: 0x0001210B
		// (set) Token: 0x06001118 RID: 4376 RVA: 0x00013F12 File Offset: 0x00012112
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

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06001119 RID: 4377 RVA: 0x00013F22 File Offset: 0x00012122
		// (set) Token: 0x0600111A RID: 4378 RVA: 0x00013F29 File Offset: 0x00012129
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

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600111B RID: 4379 RVA: 0x00013F31 File Offset: 0x00012131
		public static bool IsGameRunning
		{
			get
			{
				return !Tick._paused && Tick._freezeTime <= 0f;
			}
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00073674 File Offset: 0x00071874
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

		// Token: 0x040011FE RID: 4606
		public const float HZ_1 = 360f;

		// Token: 0x040011FF RID: 4607
		public const float HZ_2 = 720f;

		// Token: 0x04001200 RID: 4608
		public const float HZ_3 = 1080f;

		// Token: 0x04001201 RID: 4609
		public const float HZ_4 = 1440f;

		// Token: 0x04001202 RID: 4610
		public const float HZ_5 = 1800f;

		// Token: 0x04001203 RID: 4611
		public const float HZ_6 = 2160f;

		// Token: 0x04001204 RID: 4612
		public const float HZ_7 = 2520f;

		// Token: 0x04001205 RID: 4613
		public const float HZ_8 = 2880f;

		// Token: 0x04001206 RID: 4614
		private static float _time;

		// Token: 0x04001207 RID: 4615
		private static float _timeFixed;

		// Token: 0x04001208 RID: 4616
		private static float _timeUnscaled;

		// Token: 0x04001209 RID: 4617
		private static float _timeFixedUnscaled;

		// Token: 0x0400120A RID: 4618
		private static float _timePassed;

		// Token: 0x0400120B RID: 4619
		private static float _timePassedPausable;

		// Token: 0x0400120C RID: 4620
		private static float _timePassedUnscaled;

		// Token: 0x0400120D RID: 4621
		private static float _timePassedPausableUnscaled;

		// Token: 0x0400120E RID: 4622
		private static float _timePassed01;

		// Token: 0x0400120F RID: 4623
		private static float _timePassedPausable01;

		// Token: 0x04001210 RID: 4624
		private static float _timePassedUnscaled01;

		// Token: 0x04001211 RID: 4625
		private static float _timePassedPausableUnscaled01;

		// Token: 0x04001212 RID: 4626
		private static bool _paused;

		// Token: 0x04001213 RID: 4627
		private static float _freezeTime;
	}
}
