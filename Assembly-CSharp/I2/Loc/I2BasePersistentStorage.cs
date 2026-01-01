using System;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200018D RID: 397
	public abstract class I2BasePersistentStorage
	{
		// Token: 0x060011BF RID: 4543 RVA: 0x00075BD4 File Offset: 0x00073DD4
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

		// Token: 0x060011C0 RID: 4544 RVA: 0x00075C7C File Offset: 0x00073E7C
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

		// Token: 0x060011C1 RID: 4545 RVA: 0x00075D20 File Offset: 0x00073F20
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

		// Token: 0x060011C2 RID: 4546 RVA: 0x00014848 File Offset: 0x00012A48
		public virtual void ForceSaveSettings()
		{
			PlayerPrefs.Save();
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x0001484F File Offset: 0x00012A4F
		public virtual bool HasSetting(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x00007C86 File Offset: 0x00005E86
		public virtual bool CanAccessFiles()
		{
			return true;
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x00075DB4 File Offset: 0x00073FB4
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

		// Token: 0x060011C6 RID: 4550 RVA: 0x00075E14 File Offset: 0x00074014
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

		// Token: 0x060011C7 RID: 4551 RVA: 0x00075E84 File Offset: 0x00074084
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

		// Token: 0x060011C8 RID: 4552 RVA: 0x00075EF0 File Offset: 0x000740F0
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

		// Token: 0x060011C9 RID: 4553 RVA: 0x00075F58 File Offset: 0x00074158
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
