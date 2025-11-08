using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	public class ResourceManager : MonoBehaviour
	{
		// (get) Token: 0x06001016 RID: 4118 RVA: 0x000640BC File Offset: 0x000622BC
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

		// Token: 0x06001017 RID: 4119 RVA: 0x0006416D File Offset: 0x0006236D
		public static void MyOnLevelWasLoaded(Scene scene, LoadSceneMode mode)
		{
			ResourceManager.pInstance.CleanResourceCache(false);
			LocalizationManager.UpdateSources();
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00064180 File Offset: 0x00062380
		public T GetAsset<T>(string Name) where T : global::UnityEngine.Object
		{
			T t = this.FindAsset(Name) as T;
			if (t != null)
			{
				return t;
			}
			return this.LoadFromResources<T>(Name);
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x000641B8 File Offset: 0x000623B8
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

		// Token: 0x0600101A RID: 4122 RVA: 0x00064211 File Offset: 0x00062411
		public bool HasAsset(global::UnityEngine.Object Obj)
		{
			return this.Assets != null && Array.IndexOf<global::UnityEngine.Object>(this.Assets, Obj) >= 0;
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00064230 File Offset: 0x00062430
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

		// Token: 0x0600101C RID: 4124 RVA: 0x000643BC File Offset: 0x000625BC
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

		// Token: 0x0600101D RID: 4125 RVA: 0x0006442F File Offset: 0x0006262F
		public void CleanResourceCache(bool unloadResources = false)
		{
			this.mResourcesCache.Clear();
			if (unloadResources)
			{
				Resources.UnloadUnusedAssets();
			}
			base.CancelInvoke();
		}

		private static ResourceManager mInstance;

		public List<IResourceManager_Bundles> mBundleManagers = new List<IResourceManager_Bundles>();

		public global::UnityEngine.Object[] Assets;

		private readonly Dictionary<string, global::UnityEngine.Object> mResourcesCache = new Dictionary<string, global::UnityEngine.Object>(StringComparer.Ordinal);
	}
}
