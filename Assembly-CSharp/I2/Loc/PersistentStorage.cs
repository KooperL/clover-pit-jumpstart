using System;

namespace I2.Loc
{
	// Token: 0x0200018B RID: 395
	public static class PersistentStorage
	{
		// Token: 0x060011B5 RID: 4533 RVA: 0x00014713 File Offset: 0x00012913
		public static void SetSetting_String(string key, string value)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.SetSetting_String(key, value);
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00014732 File Offset: 0x00012932
		public static string GetSetting_String(string key, string defaultValue)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.GetSetting_String(key, defaultValue);
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00014751 File Offset: 0x00012951
		public static void DeleteSetting(string key)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.DeleteSetting(key);
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x0001476F File Offset: 0x0001296F
		public static bool HasSetting(string key)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.HasSetting(key);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0001478D File Offset: 0x0001298D
		public static void ForceSaveSettings()
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			PersistentStorage.mStorage.ForceSaveSettings();
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x000147AA File Offset: 0x000129AA
		public static bool CanAccessFiles()
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.CanAccessFiles();
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x000147C7 File Offset: 0x000129C7
		public static bool SaveFile(PersistentStorage.eFileType fileType, string fileName, string data, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.SaveFile(fileType, fileName, data, logExceptions);
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x000147E8 File Offset: 0x000129E8
		public static string LoadFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.LoadFile(fileType, fileName, logExceptions);
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00014808 File Offset: 0x00012A08
		public static bool DeleteFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.DeleteFile(fileType, fileName, logExceptions);
		}

		// Token: 0x060011BE RID: 4542 RVA: 0x00014828 File Offset: 0x00012A28
		public static bool HasFile(PersistentStorage.eFileType fileType, string fileName, bool logExceptions = true)
		{
			if (PersistentStorage.mStorage == null)
			{
				PersistentStorage.mStorage = new I2CustomPersistentStorage();
			}
			return PersistentStorage.mStorage.HasFile(fileType, fileName, logExceptions);
		}

		// Token: 0x040012A3 RID: 4771
		private static I2CustomPersistentStorage mStorage;

		// Token: 0x0200018C RID: 396
		public enum eFileType
		{
			// Token: 0x040012A5 RID: 4773
			Raw,
			// Token: 0x040012A6 RID: 4774
			Persistent,
			// Token: 0x040012A7 RID: 4775
			Temporal,
			// Token: 0x040012A8 RID: 4776
			Streaming
		}
	}
}
