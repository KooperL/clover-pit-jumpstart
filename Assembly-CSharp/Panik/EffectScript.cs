using System;
using UnityEngine;

namespace Panik
{
	public class EffectScript : MonoBehaviour
	{
		// Token: 0x06000AA7 RID: 2727 RVA: 0x00048854 File Offset: 0x00046A54
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

		// Token: 0x06000AA8 RID: 2728 RVA: 0x0004889D File Offset: 0x00046A9D
		private void Awake()
		{
			this.myParticleSystems = base.GetComponentsInChildren<ParticleSystem>(true);
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x000488AC File Offset: 0x00046AAC
		private void OnEnable()
		{
			this.paused = false;
			this.dead = false;
			for (int i = 0; i < this.myParticleSystems.Length; i++)
			{
				this.myParticleSystems[i].Play();
			}
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x000488E8 File Offset: 0x00046AE8
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

		private ParticleSystem[] myParticleSystems;

		public bool pausable = true;

		private bool paused;

		private bool dead;
	}
}
