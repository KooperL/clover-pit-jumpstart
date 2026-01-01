using System;
using System.Collections.Generic;

namespace I2.Loc
{
	// Token: 0x02000190 RID: 400
	public class SpecializationManager : BaseSpecializationManager
	{
		// Token: 0x060011D1 RID: 4561 RVA: 0x0001487B File Offset: 0x00012A7B
		private SpecializationManager()
		{
			this.InitializeSpecializations();
		}

		// Token: 0x060011D2 RID: 4562 RVA: 0x00076104 File Offset: 0x00074304
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

		// Token: 0x060011D3 RID: 4563 RVA: 0x000761B4 File Offset: 0x000743B4
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

		// Token: 0x060011D4 RID: 4564 RVA: 0x00076204 File Offset: 0x00074404
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

		// Token: 0x060011D5 RID: 4565 RVA: 0x000762B8 File Offset: 0x000744B8
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

		// Token: 0x060011D6 RID: 4566 RVA: 0x00076388 File Offset: 0x00074588
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

		// Token: 0x040012AB RID: 4779
		public static SpecializationManager Singleton = new SpecializationManager();
	}
}
