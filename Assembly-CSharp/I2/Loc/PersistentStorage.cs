using System;

namespace I2.Loc
{
	public static class PersistentStorage
	{
		// Token: 0x06000E19 RID: 3609 RVA: 0x0005722F File Offset: 0x0005542F
		public static void SetSetting_String(string key, string value)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.SetSetting_String(key, value);
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x0005724E File Offset: 0x0005544E
		public static string GetSetting_String(string key, string defaultValue)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.GetSetting_String(key, defaultValue);
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0005726D File Offset: 0x0005546D
		public static void DeleteSetting(string key)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.DeleteSetting(key);
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0005728B File Offset: 0x0005548B
		public static bool HasSetting(string key)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.HasSetting(key);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x000572A9 File Offset: 0x000554A9
		public static void ForceSaveSettings()
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.ForceSaveSettings();
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x000572C6 File Offset: 0x000554C6
		public static bool CanAccessFiles()
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.CanAccessFiles();
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x000572E3 File Offset: 0x000554E3
		public static bool SaveFile(PersistentStorage.eFileType fileType, string fileName, string data, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.SaveFile(fileType, fileName, data, logExceptions);
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00057304 File Offset: 0x00055504
		public static string LoadFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.LoadFile(fileType, fileName, logExceptions);
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x00057324 File Offset: 0x00055524
		public static bool DeleteFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.DeleteFile(fileType, fileName, logExceptions);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x00057344 File Offset: 0x00055544
		public static bool HasFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.HasFile(fileType, fileName, logExceptions);
		}

		private static I2CustomPersistentStorage mStorage;

		public enum eFileType
		{
			Raw,
			Persistent,
			Temporal,
			Streaming
		}
	}
}
