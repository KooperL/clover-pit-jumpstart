using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	// Token: 0x020001E8 RID: 488
	public class ResourceManager : MonoBehaviour
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06001421 RID: 5153 RVA: 0x00082424 File Offset: 0x00080624
		public static ResourceManager pInstance
		{
			get
			{
				bool flag = ResourceManager.mInstance == null;
				if (ResourceManager.mInstance == null)
				{
					ResourceManager.mInstance = (ResourceManager)global::UnityEngine.Object.FindObjectOfType(typeof(ResourceManager));
				}
				if (ResourceManager.mInstance == null)
				{
					GameObject gameObject = new GameObject("I2ResourceManager", new Type[] { typeof(ResourceManager) });
					gameObject.hideFlags |= HideFlags.HideAndDontSave;
					ResourceManager.mInstance = gameObject.GetComponent<ResourceManager>();
					SceneManager.sceneLoaded += ResourceManager.MyOnLevelWasLoaded;
				}
				if (flag && Application.isPlaying)
				{
					global::UnityEngine.Object.DontDestroyOnLoad(ResourceManager.mInstance.gameObject);
				}
				return ResourceManager.mInstance;
			}
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x00015831 File Offset: 0x00013A31
		public static void MyOnLevelWasLoaded(Scene scene, LoadSceneMode mode)
		{
			ResourceManager.pInstance.CleanResourceCache(false);
			LocalizationManager.UpdateSources();
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x000824D8 File Offset: 0x000806D8
		public T GetAsset<T>(string Name) where T : global::UnityEngine.Object
		{
			T t = this.FindAsset(Name) as T;
			if (t != null)
			{
				return t;
			}
			return this.LoadFromResources<T>(Name);
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x00082510 File Offset: 0x00080710
		private global::UnityEngine.Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				int i = 0;
				int num = this.Assets.Length;
				while (i < num)
				{
					if (this.Assets[i] != null && this.Assets[i].name == Name)
					{
						return this.Assets[i];
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x00015844 File Offset: 0x00013A44
		public bool HasAsset(global::UnityEngine.Object Obj)
		{
			return this.Assets != null && Array.IndexOf<global::UnityEngine.Object>(this.Assets, Obj) >= 0;
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x0008256C File Offset: 0x0008076C
		public T LoadFromResources<T>(string Path) where T : global::UnityEngine.Object
		{
			T t;
			try
			{
				global::UnityEngine.Object @object;
				if (string.IsNullOrEmpty(Path))
				{
					t = default(T);
					t = t;
				}
				else if (this.mResourcesCache.TryGetValue(Path, out @object) && @object != null)
				{
					t = @object as T;
				}
				else
				{
					T t2 = default(T);
					if (Path.EndsWith("]", StringComparison.OrdinalIgnoreCase))
					{
						int num = Path.LastIndexOf("[", StringComparison.OrdinalIgnoreCase);
						int num2 = Path.Length - num - 2;
						string text = Path.Substring(num + 1, num2);
						Path = Path.Substring(0, num);
						T[] array = Resources.LoadAll<T>(Path);
						int i = 0;
						int num3 = array.Length;
						while (i < num3)
						{
							if (array[i].name.Equals(text))
							{
								t2 = array[i];
								break;
							}
							i++;
						}
					}
					else
					{
						t2 = Resources.Load(Path, typeof(T)) as T;
					}
					if (t2 == null)
					{
						t2 = this.LoadFromBundle<T>(Path);
					}
					if (t2 != null)
					{
						this.mResourcesCache[Path] = t2;
					}
					t = t2;
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Unable to load {0} '{1}'\nERROR: {2}", new object[]
				{
					typeof(T),
					Path,
					ex.ToString()
				});
				t = default(T);
			}
			return t;
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x000826F8 File Offset: 0x000808F8
		public T LoadFromBundle<T>(string path) where T : global::UnityEngine.Object
		{
			int i = 0;
			int count = this.mBundleManagers.Count;
			while (i < count)
			{
				if (this.mBundleManagers[i] != null)
				{
					T t = this.mBundleManagers[i].LoadFromBundle(path, typeof(T)) as T;
					if (t != null)
					{
						return t;
					}
				}
				i++;
			}
			return default(T);
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x00015862 File Offset: 0x00013A62
		public void CleanResourceCache(bool unloadResources = false)
		{
			this.mResourcesCache.Clear();
			if (unloadResources)
			{
				Resources.UnloadUnusedAssets();
			}
			base.CancelInvoke();
		}

		// Token: 0x040013D1 RID: 5073
		private static ResourceManager mInstance;

		// Token: 0x040013D2 RID: 5074
		public List<IResourceManager_Bundles> mBundleManagers = new List<IResourceManager_Bundles>();

		// Token: 0x040013D3 RID: 5075
		public global::UnityEngine.Object[] Assets;

		// Token: 0x040013D4 RID: 5076
		private readonly Dictionary<string, global::UnityEngine.Object> mResourcesCache = new Dictionary<string, global::UnityEngine.Object>(StringComparer.Ordinal);
	}
}
