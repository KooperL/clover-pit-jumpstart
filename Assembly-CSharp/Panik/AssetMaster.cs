using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	public class AssetMaster : MonoBehaviour
	{
		// Token: 0x06000AC6 RID: 2758 RVA: 0x00048F84 File Offset: 0x00047184
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

		// Token: 0x06000AC7 RID: 2759 RVA: 0x00048FEC File Offset: 0x000471EC
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

		// Token: 0x06000AC8 RID: 2760 RVA: 0x00049054 File Offset: 0x00047254
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

		// Token: 0x06000AC9 RID: 2761 RVA: 0x000490BC File Offset: 0x000472BC
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

		// Token: 0x06000ACA RID: 2762 RVA: 0x00049124 File Offset: 0x00047324
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

		// Token: 0x06000ACB RID: 2763 RVA: 0x0004918C File Offset: 0x0004738C
		public static T GetGeneric<T>(string assetName) where T : Object
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

		// Token: 0x06000ACC RID: 2764 RVA: 0x00049208 File Offset: 0x00047408
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

		// Token: 0x06000ACD RID: 2765 RVA: 0x000492B0 File Offset: 0x000474B0
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

		// Token: 0x06000ACE RID: 2766 RVA: 0x00049358 File Offset: 0x00047558
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

		// Token: 0x06000ACF RID: 2767 RVA: 0x00049400 File Offset: 0x00047600
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

		// Token: 0x06000AD0 RID: 2768 RVA: 0x000494A8 File Offset: 0x000476A8
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

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00049550 File Offset: 0x00047750
		public static void AddGeneric<T>(T assetToAdd, bool addToIndestructibleList = false) where T : Object
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

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0004960C File Offset: 0x0004780C
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

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0004964C File Offset: 0x0004784C
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

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0004968C File Offset: 0x0004788C
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

		// Token: 0x06000AD5 RID: 2773 RVA: 0x000496CC File Offset: 0x000478CC
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

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0004970C File Offset: 0x0004790C
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

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0004974C File Offset: 0x0004794C
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

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0004978C File Offset: 0x0004798C
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
						Object.Destroy(base.gameObject);
						return;
					}
				}
				Object.DontDestroyOnLoad(base.gameObject);
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

		// Token: 0x06000AD9 RID: 2777 RVA: 0x00049994 File Offset: 0x00047B94
		private void OnEnable()
		{
			AssetMaster.list.Add(this);
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x000499A1 File Offset: 0x00047BA1
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

		private static Object objAppoggioVar;

		public List<AudioClip> sounds = new List<AudioClip>();

		public List<AudioClip> musics = new List<AudioClip>();

		public List<GameObject> prefabs = new List<GameObject>();

		public List<Texture2D> textures2D = new List<Texture2D>();

		public List<Sprite> sprites = new List<Sprite>();

		public List<Object> genericsList = new List<Object>();

		private const int DICT_INIT_SIZE = 250;

		public Dictionary<string, AudioClip> soundsDict = new Dictionary<string, AudioClip>(250);

		public Dictionary<string, AudioClip> musicDict = new Dictionary<string, AudioClip>(250);

		public Dictionary<string, GameObject> prefabsDict = new Dictionary<string, GameObject>(250);

		public Dictionary<string, Texture2D> textures2DDict = new Dictionary<string, Texture2D>(250);

		public Dictionary<string, Sprite> spritesDict = new Dictionary<string, Sprite>(250);

		public Dictionary<string, Object> genericsDict = new Dictionary<string, Object>(250);
	}
}
