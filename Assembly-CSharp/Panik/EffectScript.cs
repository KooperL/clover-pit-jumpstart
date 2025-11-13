using System;
using UnityEngine;

namespace Panik
{
	public class EffectScript : MonoBehaviour
	{
		// Token: 0x06000ABC RID: 2748 RVA: 0x00048FB4 File Offset: 0x000471B4
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

		// Token: 0x06000ABD RID: 2749 RVA: 0x00048FFD File Offset: 0x000471FD
		private void Awake()
		{
			this.myParticleSystems = base.GetComponentsInChildren<ParticleSystem>(true);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0004900C File Offset: 0x0004720C
		private void OnEnable()
		{
			this.paused = false;
			this.dead = false;
			for (int i = 0; i < this.myParticleSystems.Length; i++)
			{
				this.myParticleSystems[i].Play();
			}
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x00049048 File Offset: 0x00047248
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
