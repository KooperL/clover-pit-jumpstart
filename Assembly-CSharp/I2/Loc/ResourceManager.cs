using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	public class ResourceManager : MonoBehaviour
	{
		// (get) Token: 0x0600102D RID: 4141 RVA: 0x00064898 File Offset: 0x00062A98
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

		// Token: 0x0600102E RID: 4142 RVA: 0x00064949 File Offset: 0x00062B49
		public static void MyOnLevelWasLoaded(Scene scene, LoadSceneMode mode)
		{
			ResourceManager.pInstance.CleanResourceCache(false);
			LocalizationManager.UpdateSources();
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x0006495C File Offset: 0x00062B5C
		public T GetAsset<T>(string Name) where T : global::UnityEngine.Object
		{
			T t = this.FindAsset(Name) as T;
			if (t != null)
			{
				return t;
			}
			return this.LoadFromResources<T>(Name);
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00064994 File Offset: 0x00062B94
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

		// Token: 0x06001031 RID: 4145 RVA: 0x000649ED File Offset: 0x00062BED
		public bool HasAsset(global::UnityEngine.Object Obj)
		{
			return this.Assets != null && Array.IndexOf<global::UnityEngine.Object>(this.Assets, Obj) >= 0;
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x00064A0C File Offset: 0x00062C0C
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

		// Token: 0x06001033 RID: 4147 RVA: 0x00064B98 File Offset: 0x00062D98
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

		// Token: 0x06001034 RID: 4148 RVA: 0x00064C0B File Offset: 0x00062E0B
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
