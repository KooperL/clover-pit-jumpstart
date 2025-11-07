using System;
using UnityEngine;

namespace I2.Loc
{
	public class RegisterBundlesManager : MonoBehaviour, IResourceManager_Bundles
	{
		// Token: 0x06000DFB RID: 3579 RVA: 0x000569B2 File Offset: 0x00054BB2
		public void OnEnable()
		{
			if (!ResourceManager.pInstance.mBundleManagers.Contains(this))
			{
				ResourceManager.pInstance.mBundleManagers.Add(this);
			}
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x000569D6 File Offset: 0x00054BD6
		public void OnDisable()
		{
			ResourceManager.pInstance.mBundleManagers.Remove(this);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x000569E9 File Offset: 0x00054BE9
		public virtual Object LoadFromBundle(string path, Type assetType)
		{
			return null;
		}
	}
}
