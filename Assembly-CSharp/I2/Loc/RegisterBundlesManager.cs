using System;
using UnityEngine;

namespace I2.Loc
{
	public class RegisterBundlesManager : MonoBehaviour, IResourceManager_Bundles
	{
		// Token: 0x06000E12 RID: 3602 RVA: 0x0005718E File Offset: 0x0005538E
		public void OnEnable()
		{
			if (!ResourceManager.pInstance.mBundleManagers.Contains(this))
			{
				ResourceManager.pInstance.mBundleManagers.Add(this);
			}
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x000571B2 File Offset: 0x000553B2
		public void OnDisable()
		{
			ResourceManager.pInstance.mBundleManagers.Remove(this);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x000571C5 File Offset: 0x000553C5
		public virtual global::UnityEngine.Object LoadFromBundle(string path, Type assetType)
		{
			return null;
		}
	}
}
