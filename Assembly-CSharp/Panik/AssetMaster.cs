using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	public class AssetMaster : MonoBehaviour
	{
		// Token: 0x06000ADB RID: 2779 RVA: 0x000496E4 File Offset: 0x000478E4
		public static AudioClip GetSound(string clipName)
		{
			for (int i = 0; i < AssetMaster.list.Count; i++)
			{
				if (AssetMaster.list[i].soundsDict.ContainsKey(clipName))
				{
					return AssetMaster.list[i].soundsDict[clipName];
				}
			}
			Debug.LogError("ASSET MASTER: sound '" + clipName + "' not found inside the asset master system!");
			return null;
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0004974C File Offset: 0x0004794C
		public static AudioClip GetMusic(string clipName)
		{
			for (int i = 0; i < AssetMaster.list.Count; i++)
			{
				if (AssetMaster.list[i].musicDict.ContainsKey(clipName))
				{
					return AssetMaster.list[i].musicDict[clipName];
				}
			}
			Debug.LogError("ASSET MASTER: ost '" + clipName + "' not found inside the asset master system!");
			return null;
		}

		// Token: 0x06000ADD RID: 2781 RVA: 0x000497B4 File Offset: 0x000479B4
		public static GameObject GetPrefab(string prefabName)
		{
			for (int i = 0; i < AssetMaster.list.Count; i++)
			{
				if (AssetMaster.list[i].prefabsDict.ContainsKey(prefabName))
				{
					return AssetMaster.list[i].prefabsDict[prefabName];
				}
			}
			Debug.LogError("ASSET MASTER: prefab '" + prefabName + "' not found inside the asset master system!");
			return null;
		}

		// Token: 0x06000ADE RID: 2782 RVA: 0x0004981C File Offset: 0x00047A1C
		public static Texture2D GetTexture2D(string textureName)
		{
			for (int i = 0; i < AssetMaster.list.Count; i++)
			{
				if (AssetMaster.list[i].textures2DDict.ContainsKey(textureName))
				{
					return AssetMaster.list[i].textures2DDict[textureName];
				}
			}
			Debug.LogError("ASSET MASTER: texture2D '" + textureName + "' not found inside the asset master system!");
			return null;
		}

		// Token: 0x06000ADF RID: 2783 RVA: 0x00049884 File Offset: 0x00047A84
		public static Sprite GetSprite(string spriteName)
		{
			for (int i = 0; i < AssetMaster.list.Count; i++)
			{
				if (AssetMaster.list[i].spritesDict.ContainsKey(spriteName))
				{
					return AssetMaster.list[i].spritesDict[spriteName];
				}
			}
			Debug.LogError("ASSET MASTER: sprite '" + spriteName + "' not found inside the asset master system!");
			return null;
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x000498EC File Offset: 0x00047AEC
		public static T GetGeneric<T>(string assetName) where T : global::UnityEngine.Object
		{
			for (int i = 0; i < AssetMaster.list.Count; i++)
			{
				if (AssetMaster.list[i].genericsDict.ContainsKey(assetName))
				{
					return AssetMaster.list[i].genericsDict[assetName] as T;
				}
			}
			Debug.LogError("ASSET MASTER: generic asset '" + assetName + "' not found inside the asset master system!");
			return default(T);
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x00049968 File Offset: 0x00047B68
		public static void AddSound(AudioClip soundToAdd, bool addToIndestructibleList = false)
		{
			int i = 0;
			while (i < AssetMaster.list.Count)
			{
				if ((addToIndestructibleList || !AssetMaster.list[i].iAmIndestructible) && (!addToIndestructibleList || AssetMaster.list[i].iAmIndestructible))
				{
					if (AssetMaster.list[i].soundsDict.ContainsKey(soundToAdd.name))
					{
						Debug.LogWarning("ASSET MASTER: sound '" + soundToAdd.name + "' already exists inside the asset master system!");
						return;
					}
					AssetMaster.list[i].soundsDict.Add(soundToAdd.name, soundToAdd);
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x00049A10 File Offset: 0x00047C10
		public static void AddMusic(AudioClip ostToAdd, bool addToIndestructibleList = false)
		{
			int i = 0;
			while (i < AssetMaster.list.Count)
			{
				if ((addToIndestructibleList || !AssetMaster.list[i].iAmIndestructible) && (!addToIndestructibleList || AssetMaster.list[i].iAmIndestructible))
				{
					if (AssetMaster.list[i].musicDict.ContainsKey(ostToAdd.name))
					{
						Debug.LogWarning("ASSET MASTER: ost '" + ostToAdd.name + "' already exists inside the asset master system!");
						return;
					}
					AssetMaster.list[i].musicDict.Add(ostToAdd.name, ostToAdd);
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00049AB8 File Offset: 0x00047CB8
		public static void AddPrefab(GameObject prefabToAdd, bool addToIndestructibleList = false)
		{
			int i = 0;
			while (i < AssetMaster.list.Count)
			{
				if ((addToIndestructibleList || !AssetMaster.list[i].iAmIndestructible) && (!addToIndestructibleList || AssetMaster.list[i].iAmIndestructible))
				{
					if (AssetMaster.list[i].prefabsDict.ContainsKey(prefabToAdd.name))
					{
						Debug.LogWarning("ASSET MASTER: prefab '" + prefabToAdd.name + "' already exists inside the asset master system!");
						return;
					}
					AssetMaster.list[i].prefabsDict.Add(prefabToAdd.name, prefabToAdd);
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00049B60 File Offset: 0x00047D60
		public static void AddTexture2D(Texture2D textureToAdd, bool addToIndestructibleList = false)
		{
			int i = 0;
			while (i < AssetMaster.list.Count)
			{
				if ((addToIndestructibleList || !AssetMaster.list[i].iAmIndestructible) && (!addToIndestructibleList || AssetMaster.list[i].iAmIndestructible))
				{
					if (AssetMaster.list[i].textures2DDict.ContainsKey(textureToAdd.name))
					{
						Debug.LogWarning("ASSET MASTER: texture2D '" + textureToAdd.name + "' already exists inside the asset master system!");
						return;
					}
					AssetMaster.list[i].textures2DDict.Add(textureToAdd.name, textureToAdd);
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00049C08 File Offset: 0x00047E08
		public static void AddSprite(Sprite spriteToAdd, bool addToIndestructibleList = false)
		{
			int i = 0;
			while (i < AssetMaster.list.Count)
			{
				if ((addToIndestructibleList || !AssetMaster.list[i].iAmIndestructible) && (!addToIndestructibleList || AssetMaster.list[i].iAmIndestructible))
				{
					if (AssetMaster.list[i].spritesDict.ContainsKey(spriteToAdd.name))
					{
						Debug.LogWarning("ASSET MASTER: sprite '" + spriteToAdd.name + "' already exists inside the asset master system!");
						return;
					}
					AssetMaster.list[i].spritesDict.Add(spriteToAdd.name, spriteToAdd);
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00049CB0 File Offset: 0x00047EB0
		public static void AddGeneric<T>(T assetToAdd, bool addToIndestructibleList = false) where T : global::UnityEngine.Object
		{
			int i = 0;
			while (i < AssetMaster.list.Count)
			{
				if ((addToIndestructibleList || !AssetMaster.list[i].iAmIndestructible) && (!addToIndestructibleList || AssetMaster.list[i].iAmIndestructible))
				{
					if (AssetMaster.list[i].genericsDict.ContainsKey(assetToAdd.name))
					{
						Debug.LogWarning("ASSET MASTER: generic asset '" + assetToAdd.name + "' already exists inside the asset master system!");
						return;
					}
					AssetMaster.list[i].genericsDict.Add(assetToAdd.name, assetToAdd);
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00049D6C File Offset: 0x00047F6C
		public static bool HasSound(string clipName)
		{
			for (int i = 0; i < AssetMaster.list.Count; i++)
			{
				if (AssetMaster.list[i].soundsDict.ContainsKey(clipName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00049DAC File Offset: 0x00047FAC
		public static bool HasMusic(string clipName)
		{
			for (int i = 0; i < AssetMaster.list.Count; i++)
			{
				if (AssetMaster.list[i].musicDict.ContainsKey(clipName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x00049DEC File Offset: 0x00047FEC
		public static bool HasPrefab(string prefabName)
		{
			for (int i = 0; i < AssetMaster.list.Count; i++)
			{
				if (AssetMaster.list[i].prefabsDict.ContainsKey(prefabName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x00049E2C File Offset: 0x0004802C
		public static bool HasTexture2D(string textureName)
		{
			for (int i = 0; i < AssetMaster.list.Count; i++)
			{
				if (AssetMaster.list[i].textures2DDict.ContainsKey(textureName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00049E6C File Offset: 0x0004806C
		public static bool HasSprite(string spriteName)
		{
			for (int i = 0; i < AssetMaster.list.Count; i++)
			{
				if (AssetMaster.list[i].spritesDict.ContainsKey(spriteName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00049EAC File Offset: 0x000480AC
		public static bool HasGeneric(string assetName)
		{
			for (int i = 0; i < AssetMaster.list.Count; i++)
			{
				if (AssetMaster.list[i].genericsDict.ContainsKey(assetName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00049EEC File Offset: 0x000480EC
		private void Awake()
		{
			if (this.iAmIndestructible)
			{
				base.transform.SetParent(null);
				base.name = "[INDESTRUCTIBLE] " + base.name;
				for (int i = AssetMaster.list.Count - 1; i >= 0; i--)
				{
					if (AssetMaster.list[i].name == base.name)
					{
						global::UnityEngine.Object.Destroy(base.gameObject);
						return;
					}
				}
				global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			}
			for (int j = 0; j < this.sounds.Count; j++)
			{
				this.soundsDict.Add(this.sounds[j].name, this.sounds[j]);
			}
			for (int k = 0; k < this.musics.Count; k++)
			{
				this.musicDict.Add(this.musics[k].name, this.musics[k]);
			}
			for (int l = 0; l < this.prefabs.Count; l++)
			{
				this.prefabsDict.Add(this.prefabs[l].name, this.prefabs[l]);
			}
			for (int m = 0; m < this.textures2D.Count; m++)
			{
				this.textures2DDict.Add(this.textures2D[m].name, this.textures2D[m]);
			}
			for (int n = 0; n < this.sprites.Count; n++)
			{
				this.spritesDict.Add(this.sprites[n].name, this.sprites[n]);
			}
			for (int num = 0; num < this.genericsList.Count; num++)
			{
				this.genericsDict.Add(this.genericsList[num].name, this.genericsList[num]);
			}
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0004A0F4 File Offset: 0x000482F4
		private void OnEnable()
		{
			AssetMaster.list.Add(this);
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0004A101 File Offset: 0x00048301
		private void OnDisable()
		{
			AssetMaster.list.Remove(this);
		}

		public static List<AssetMaster> list = new List<AssetMaster>();

		public bool iAmIndestructible;

		private static AssetMaster assetMasterAppoggioVar;

		private static AudioClip audioClipAppoggioVar;

		private static GameObject gameObjAppoggioVar;

		private static Texture2D texture2DAppoggioVar;

		private static Sprite spriteAppoggioVar;

		private static global::UnityEngine.Object objAppoggioVar;

		public List<AudioClip> sounds = new List<AudioClip>();

		public List<AudioClip> musics = new List<AudioClip>();

		public List<GameObject> prefabs = new List<GameObject>();

		public List<Texture2D> textures2D = new List<Texture2D>();

		public List<Sprite> sprites = new List<Sprite>();

		public List<global::UnityEngine.Object> genericsList = new List<global::UnityEngine.Object>();

		private const int DICT_INIT_SIZE = 250;

		public Dictionary<string, AudioClip> soundsDict = new Dictionary<string, AudioClip>(250);

		public Dictionary<string, AudioClip> musicDict = new Dictionary<string, AudioClip>(250);

		public Dictionary<string, GameObject> prefabsDict = new Dictionary<string, GameObject>(250);

		public Dictionary<string, Texture2D> textures2DDict = new Dictionary<string, Texture2D>(250);

		public Dictionary<string, Sprite> spritesDict = new Dictionary<string, Sprite>(250);

		public Dictionary<string, global::UnityEngine.Object> genericsDict = new Dictionary<string, global::UnityEngine.Object>(250);
	}
}
