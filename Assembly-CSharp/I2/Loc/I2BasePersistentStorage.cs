using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

namespace I2.Loc
{
	public abstract class I2BasePersistentStorage
	{
		// Token: 0x06000E0C RID: 3596 RVA: 0x00056B88 File Offset: 0x00054D88
		public virtual void SetSetting_String(string key, string value)
		{
			try
			{
				int length = value.Length;
				int num = 8000;
				if (length <= num)
				{
					PlayerPrefs.SetString(key, value);
				}
				else
				{
					int num2 = Mathf.CeilToInt((float)length / (float)num);
					for (int i = 0; i < num2; i++)
					{
						int num3 = num * i;
						PlayerPrefs.SetString(string.Format("[I2split]{0}{1}", i, key), value.Substring(num3, Mathf.Min(num, length - num3)));
					}
					PlayerPrefs.SetString(key, "[$I2#@div$]" + num2.ToString());
				}
			}
			catch (Exception)
			{
				Debug.LogError("Error saving PlayerPrefs " + key);
			}
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00056C30 File Offset: 0x00054E30
		public virtual string GetSetting_String(string key, string defaultValue)
		{
			string text2;
			try
			{
				string text = PlayerPrefs.GetString(key, defaultValue);
				if (!string.IsNullOrEmpty(text) && text.StartsWith("[I2split]", StringComparison.Ordinal))
				{
					int num = int.Parse(text.Substring("[I2split]".Length), CultureInfo.InvariantCulture);
					text = "";
					for (int i = 0; i < num; i++)
					{
						text += PlayerPrefs.GetString(string.Format("[I2split]{0}{1}", i, key), "");
					}
				}
				text2 = text;
			}
			catch (Exception)
			{
				Debug.LogError("Error loading PlayerPrefs " + key);
				text2 = defaultValue;
			}
			return text2;
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00056CD4 File Offset: 0x00054ED4
		public virtual void DeleteSetting(string key)
		{
			try
			{
				string @string = PlayerPrefs.GetString(key, null);
				if (!string.IsNullOrEmpty(@string) && @string.StartsWith("[I2split]", StringComparison.Ordinal))
				{
					int num = int.Parse(@string.Substring("[I2split]".Length), CultureInfo.InvariantCulture);
					for (int i = 0; i < num; i++)
					{
						PlayerPrefs.DeleteKey(string.Format("[I2split]{0}{1}", i, key));
					}
				}
				PlayerPrefs.DeleteKey(key);
			}
			catch (Exception)
			{
				Debug.LogError("Error deleting PlayerPrefs " + key);
			}
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00056D68 File Offset: 0x00054F68
		public virtual void ForceSaveSettings()
		{
			PlayerPrefs.Save();
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00056D6F File Offset: 0x00054F6F
		public virtual bool HasSetting(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00056D77 File Offset: 0x00054F77
		public virtual bool CanAccessFiles()
		{
			return true;
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00056D7C File Offset: 0x00054F7C
		private string UpdateFilename(PersistentStorage.eFileType fileType, string fileName)
		{
			switch (fileType)
			{
			case PersistentStorage.eFileType.Persistent:
				fileName = Application.persistentDataPath + "/" + fileName;
				break;
			case PersistentStorage.eFileType.Temporal:
				fileName = Application.temporaryCachePath + "/" + fileName;
				break;
			case PersistentStorage.eFileType.Streaming:
				fileName = Application.streamingAssetsPath + "/" + fileName;
				break;
			}
			return fileName;
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00056DDC File Offset: 0x00054FDC
		public virtual bool SaveFile(PersistentStorage.eFileType fileType, string fileName, string data, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return false;
			}
			bool flag;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				File.WriteAllText(fileName, data, Encoding.UTF8);
				flag = true;
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					string text = "Error saving file '";
					string text2 = fileName;
					string text3 = "'\n";
					Exception ex2 = ex;
					Debug.LogError(text + text2 + text3 + ((ex2 != null) ? ex2.ToString() : null));
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00056E4C File Offset: 0x0005504C
		public virtual string LoadFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return null;
			}
			string text;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				text = File.ReadAllText(fileName, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					string text2 = "Error loading file '";
					string text3 = fileName;
					string text4 = "'\n";
					Exception ex2 = ex;
					Debug.LogError(text2 + text3 + text4 + ((ex2 != null) ? ex2.ToString() : null));
				}
				text = null;
			}
			return text;
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x00056EB8 File Offset: 0x000550B8
		public virtual bool DeleteFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return false;
			}
			bool flag;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				File.Delete(fileName);
				flag = true;
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					string text = "Error deleting file '";
					string text2 = fileName;
					string text3 = "'\n";
					Exception ex2 = ex;
					Debug.LogError(text + text2 + text3 + ((ex2 != null) ? ex2.ToString() : null));
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00056F20 File Offset: 0x00055120
		public virtual bool HasFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (!this.CanAccessFiles())
			{
				return false;
			}
			bool flag;
			try
			{
				fileName = this.UpdateFilename(fileType, fileName);
				flag = File.Exists(fileName);
			}
			catch (Exception ex)
			{
				if (logExceptions)
				{
					string text = "Error requesting file '";
					string text2 = fileName;
					string text3 = "'\n";
					Exception ex2 = ex;
					Debug.LogError(text + text2 + text3 + ((ex2 != null) ? ex2.ToString() : null));
				}
				flag = false;
			}
			return flag;
		}
	}
}
