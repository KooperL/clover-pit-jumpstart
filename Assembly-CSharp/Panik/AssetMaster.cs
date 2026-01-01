using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	// Token: 0x0200010F RID: 271
	public class AssetMaster : MonoBehaviour
	{
		// Token: 0x06000CB9 RID: 3257 RVA: 0x000637F0 File Offset: 0x000619F0
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

		// Token: 0x06000CBA RID: 3258 RVA: 0x00063858 File Offset: 0x00061A58
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

		// Token: 0x06000CBB RID: 3259 RVA: 0x000638C0 File Offset: 0x00061AC0
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

		// Token: 0x06000CBC RID: 3260 RVA: 0x00063928 File Offset: 0x00061B28
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

		// Token: 0x06000CBD RID: 3261 RVA: 0x00063990 File Offset: 0x00061B90
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

		// Token: 0x06000CBE RID: 3262 RVA: 0x000639F8 File Offset: 0x00061BF8
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

		// Token: 0x06000CBF RID: 3263 RVA: 0x00063A74 File Offset: 0x00061C74
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

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00063B1C File Offset: 0x00061D1C
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

		// Token: 0x06000CC1 RID: 3265 RVA: 0x00063BC4 File Offset: 0x00061DC4
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

		// Token: 0x06000CC2 RID: 3266 RVA: 0x00063C6C File Offset: 0x00061E6C
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

		// Token: 0x06000CC3 RID: 3267 RVA: 0x00063D14 File Offset: 0x00061F14
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

		// Token: 0x06000CC4 RID: 3268 RVA: 0x00063DBC File Offset: 0x00061FBC
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

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00063E78 File Offset: 0x00062078
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

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00063EB8 File Offset: 0x000620B8
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

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00063EF8 File Offset: 0x000620F8
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

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00063F38 File Offset: 0x00062138
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

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00063F78 File Offset: 0x00062178
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

		// Token: 0x06000CCA RID: 3274 RVA: 0x00063FB8 File Offset: 0x000621B8
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

		// Token: 0x06000CCB RID: 3275 RVA: 0x00063FF8 File Offset: 0x000621F8
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

		// Token: 0x06000CCC RID: 3276 RVA: 0x00010675 File Offset: 0x0000E875
		private void OnEnable()
		{
			AssetMaster.list.Add(this);
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00010682 File Offset: 0x0000E882
		private void OnDisable()
		{
			AssetMaster.list.Remove(this);
		}

		// Token: 0x04000D89 RID: 3465
		public static List<AssetMaster> list = new List<AssetMaster>();

		// Token: 0x04000D8A RID: 3466
		public bool iAmIndestructible;

		// Token: 0x04000D8B RID: 3467
		private static AssetMaster assetMasterAppoggioVar;

		// Token: 0x04000D8C RID: 3468
		private static AudioClip audioClipAppoggioVar;

		// Token: 0x04000D8D RID: 3469
		private static GameObject gameObjAppoggioVar;

		// Token: 0x04000D8E RID: 3470
		private static Texture2D texture2DAppoggioVar;

		// Token: 0x04000D8F RID: 3471
		private static Sprite spriteAppoggioVar;

		// Token: 0x04000D90 RID: 3472
		private static global::UnityEngine.Object objAppoggioVar;

		// Token: 0x04000D91 RID: 3473
		public List<AudioClip> sounds = new List<AudioClip>();

		// Token: 0x04000D92 RID: 3474
		public List<AudioClip> musics = new List<AudioClip>();

		// Token: 0x04000D93 RID: 3475
		public List<GameObject> prefabs = new List<GameObject>();

		// Token: 0x04000D94 RID: 3476
		public List<Texture2D> textures2D = new List<Texture2D>();

		// Token: 0x04000D95 RID: 3477
		public List<Sprite> sprites = new List<Sprite>();

		// Token: 0x04000D96 RID: 3478
		public List<global::UnityEngine.Object> genericsList = new List<global::UnityEngine.Object>();

		// Token: 0x04000D97 RID: 3479
		private const int DICT_INIT_SIZE = 250;

		// Token: 0x04000D98 RID: 3480
		public Dictionary<string, AudioClip> soundsDict = new Dictionary<string, AudioClip>(250);

		// Token: 0x04000D99 RID: 3481
		public Dictionary<string, AudioClip> musicDict = new Dictionary<string, AudioClip>(250);

		// Token: 0x04000D9A RID: 3482
		public Dictionary<string, GameObject> prefabsDict = new Dictionary<string, GameObject>(250);

		// Token: 0x04000D9B RID: 3483
		public Dictionary<string, Texture2D> textures2DDict = new Dictionary<string, Texture2D>(250);

		// Token: 0x04000D9C RID: 3484
		public Dictionary<string, Sprite> spritesDict = new Dictionary<string, Sprite>(250);

		// Token: 0x04000D9D RID: 3485
		public Dictionary<string, global::UnityEngine.Object> genericsDict = new Dictionary<string, global::UnityEngine.Object>(250);
	}
}
