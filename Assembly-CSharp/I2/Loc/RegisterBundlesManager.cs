using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000189 RID: 393
	public class RegisterBundlesManager : MonoBehaviour, IResourceManager_Bundles
	{
		// Token: 0x060011AE RID: 4526 RVA: 0x000146C7 File Offset: 0x000128C7
		public void OnEnable()
		{
			if (!ResourceManager.pInstance.mBundleManagers.Contains(this))
			{
				ResourceManager.pInstance.mBundleManagers.Add(this);
			}
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x000146EB File Offset: 0x000128EB
		public void OnDisable()
		{
			ResourceManager.pInstance.mBundleManagers.Remove(this);
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x000146FE File Offset: 0x000128FE
		public virtual global::UnityEngine.Object LoadFromBundle(string path, Type assetType)
		{
			return null;
		}
	}
}
