using System;
using UnityEngine;

namespace Panik
{
	// Token: 0x0200010A RID: 266
	public class EffectScript : MonoBehaviour
	{
		// Token: 0x06000C96 RID: 3222 RVA: 0x00063234 File Offset: 0x00061434
		public void KillMe()
		{
			if (this.dead)
			{
				return;
			}
			this.dead = true;
			for (int i = 0; i < this.myParticleSystems.Length; i++)
			{
				this.myParticleSystems[i].Stop();
			}
			Pool.Destroy(base.gameObject, null);
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x00010503 File Offset: 0x0000E703
		private void Awake()
		{
			this.myParticleSystems = base.GetComponentsInChildren<ParticleSystem>(true);
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x00063280 File Offset: 0x00061480
		private void OnEnable()
		{
			this.paused = false;
			this.dead = false;
			for (int i = 0; i < this.myParticleSystems.Length; i++)
			{
				this.myParticleSystems[i].Play();
			}
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x000632BC File Offset: 0x000614BC
		private void Update()
		{
			if (this.pausable)
			{
				if (!Tick.IsGameRunning)
				{
					if (this.paused)
					{
						return;
					}
					this.paused = true;
					for (int i = 0; i < this.myParticleSystems.Length; i++)
					{
						this.myParticleSystems[i].Pause();
					}
					return;
				}
				else if (this.paused)
				{
					this.paused = false;
					for (int j = 0; j < this.myParticleSystems.Length; j++)
					{
						this.myParticleSystems[j].Play();
					}
				}
			}
			bool flag = true;
			for (int k = 0; k < this.myParticleSystems.Length; k++)
			{
				if (this.myParticleSystems[k].IsAlive() || this.myParticleSystems[k].main.loop)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				this.KillMe();
			}
		}

		// Token: 0x04000D6E RID: 3438
		private ParticleSystem[] myParticleSystems;

		// Token: 0x04000D6F RID: 3439
		public bool pausable = true;

		// Token: 0x04000D70 RID: 3440
		private bool paused;

		// Token: 0x04000D71 RID: 3441
		private bool dead;
	}
}
