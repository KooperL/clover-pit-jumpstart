using System;
using System.Collections.Generic;

namespace I2.Loc
{
	public class SpecializationManager : BaseSpecializationManager
	{
		// Token: 0x06000E35 RID: 3637 RVA: 0x000578DA File Offset: 0x00055ADA
		private SpecializationManager()
		{
			this.InitializeSpecializations();
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x000578E8 File Offset: 0x00055AE8
		public static string GetSpecializedText(string text, string specialization = null)
		{
			int num = text.IndexOf("[i2s_", StringComparison.Ordinal);
			if (num < 0)
			{
				return text;
			}
			if (string.IsNullOrEmpty(specialization))
			{
				specialization = SpecializationManager.Singleton.GetCurrentSpecialization();
			}
			while (!string.IsNullOrEmpty(specialization) && specialization != "Any")
			{
				string text2 = "[i2s_" + specialization + "]";
				int num2 = text.IndexOf(text2, StringComparison.Ordinal);
				if (num2 >= 0)
				{
					num2 += text2.Length;
					int num3 = text.IndexOf("[i2s_", num2, StringComparison.Ordinal);
					if (num3 < 0)
					{
						num3 = text.Length;
					}
					return text.Substring(num2, num3 - num2);
				}
				specialization = SpecializationManager.Singleton.GetFallbackSpecialization(specialization);
			}
			return text.Substring(0, num);
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00057998 File Offset: 0x00055B98
		public static string SetSpecializedText(string text, string newText, string specialization)
		{
			if (string.IsNullOrEmpty(specialization))
			{
				specialization = "Any";
			}
			if ((text == null || !text.Contains("[i2s_")) && specialization == "Any")
			{
				return newText;
			}
			Dictionary<string, string> specializations = SpecializationManager.GetSpecializations(text, null);
			specializations[specialization] = newText;
			return SpecializationManager.SetSpecializedText(specializations);
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x000579E8 File Offset: 0x00055BE8
		public static string SetSpecializedText(Dictionary<string, string> specializations)
		{
			string text;
			if (!specializations.TryGetValue("Any", out text))
			{
				text = string.Empty;
			}
			foreach (KeyValuePair<string, string> keyValuePair in specializations)
			{
				if (keyValuePair.Key != "Any" && !string.IsNullOrEmpty(keyValuePair.Value))
				{
					text = string.Concat(new string[] { text, "[i2s_", keyValuePair.Key, "]", keyValuePair.Value });
				}
			}
			return text;
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00057A9C File Offset: 0x00055C9C
		public static Dictionary<string, string> GetSpecializations(string text, Dictionary<string, string> buffer = null)
		{
			if (buffer == null)
			{
				buffer = new Dictionary<string, string>(StringComparer.Ordinal);
			}
			else
			{
				buffer.Clear();
			}
			if (text == null)
			{
				buffer["Any"] = "";
				return buffer;
			}
			int num = text.IndexOf("[i2s_", StringComparison.Ordinal);
			if (num < 0)
			{
				num = text.Length;
			}
			buffer["Any"] = text.Substring(0, num);
			for (int i = num; i < text.Length; i = num)
			{
				i += "[i2s_".Length;
				int num2 = text.IndexOf(']', i);
				if (num2 < 0)
				{
					break;
				}
				string text2 = text.Substring(i, num2 - i);
				i = num2 + 1;
				num = text.IndexOf("[i2s_", i, StringComparison.Ordinal);
				if (num < 0)
				{
					num = text.Length;
				}
				string text3 = text.Substring(i, num - i);
				buffer[text2] = text3;
			}
			return buffer;
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00057B6C File Offset: 0x00055D6C
		public static void AppendSpecializations(string text, List<string> list = null)
		{
			if (text == null)
			{
				return;
			}
			if (list == null)
			{
				list = new List<string>();
			}
			if (!list.Contains("Any"))
			{
				list.Add("Any");
			}
			int i = 0;
			while (i < text.Length)
			{
				i = text.IndexOf("[i2s_", i, StringComparison.Ordinal);
				if (i < 0)
				{
					break;
				}
				i += "[i2s_".Length;
				int num = text.IndexOf(']', i);
				if (num < 0)
				{
					break;
				}
				string text2 = text.Substring(i, num - i);
				if (!list.Contains(text2))
				{
					list.Add(text2);
				}
			}
		}

		public static SpecializationManager Singleton = new SpecializationManager();
	}
}
