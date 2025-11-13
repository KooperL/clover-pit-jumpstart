using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizationReader
	{
		// Token: 0x06000EE5 RID: 3813 RVA: 0x0005FC70 File Offset: 0x0005DE70
		public static Dictionary<string, string> ReadTextAsset(TextAsset asset)
		{
			StringReader stringReader = new StringReader(Encoding.UTF8.GetString(asset.bytes, 0, asset.bytes.Length).Replace("\r\n", "\n").Replace("\r", "\n"));
			Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.Ordinal);
			string text;
			while ((text = stringReader.ReadLine()) != null)
			{
				string text2;
				string text3;
				string text4;
				string text5;
				string text6;
				if (LocalizationReader.TextAsset_ReadLine(text, out text2, out text3, out text4, out text5, out text6) && !string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
				{
					dictionary[text2] = text3;
				}
			}
			return dictionary;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0005FD00 File Offset: 0x0005DF00
		public static bool TextAsset_ReadLine(string line, out string key, out string value, out string category, out string comment, out string termType)
		{
			key = string.Empty;
			category = string.Empty;
			comment = string.Empty;
			termType = string.Empty;
			value = string.Empty;
			int num = line.LastIndexOf("//", StringComparison.Ordinal);
			if (num >= 0)
			{
				comment = line.Substring(num + 2).Trim();
				comment = LocalizationReader.DecodeString(comment);
				line = line.Substring(0, num);
			}
			int num2 = line.IndexOf("=", StringComparison.Ordinal);
			if (num2 < 0)
			{
				return false;
			}
			key = line.Substring(0, num2).Trim();
			value = line.Substring(num2 + 1).Trim();
			value = value.Replace("\r\n", "\n").Replace("\n", "\\n");
			value = LocalizationReader.DecodeString(value);
			if (key.Length > 2 && key[0] == '[')
			{
				int num3 = key.IndexOf(']');
				if (num3 >= 0)
				{
					termType = key.Substring(1, num3 - 1);
					key = key.Substring(num3 + 1);
				}
			}
			LocalizationReader.ValidateFullTerm(ref key);
			return true;
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0005FE0C File Offset: 0x0005E00C
		public static string ReadCSVfile(string Path, Encoding encoding)
		{
			string text = string.Empty;
			using (StreamReader streamReader = new StreamReader(Path, encoding))
			{
				text = streamReader.ReadToEnd();
			}
			text = text.Replace("\r\n", "\n");
			text = text.Replace("\r", "\n");
			return text;
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0005FE70 File Offset: 0x0005E070
		public static List<string[]> ReadCSV(string Text, char Separator = ',')
		{
			int i = 0;
			List<string[]> list = new List<string[]>();
			while (i < Text.Length)
			{
				string[] array = LocalizationReader.ParseCSVline(Text, ref i, Separator);
				if (array == null)
				{
					break;
				}
				list.Add(array);
			}
			return list;
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0005FEA8 File Offset: 0x0005E0A8
		private static string[] ParseCSVline(string Line, ref int iStart, char Separator)
		{
			List<string> list = new List<string>();
			int length = Line.Length;
			int num = iStart;
			bool flag = false;
			while (iStart < length)
			{
				char c = Line[iStart];
				if (flag)
				{
					if (c == '"')
					{
						if (iStart + 1 >= length || Line[iStart + 1] != '"')
						{
							flag = false;
						}
						else if (iStart + 2 < length && Line[iStart + 2] == '"')
						{
							flag = false;
							iStart += 2;
						}
						else
						{
							iStart++;
						}
					}
				}
				else if (c == '\n' || c == Separator)
				{
					LocalizationReader.AddCSVtoken(ref list, ref Line, iStart, ref num);
					if (c == '\n')
					{
						iStart++;
						break;
					}
				}
				else if (c == '"')
				{
					flag = true;
				}
				iStart++;
			}
			if (iStart > num)
			{
				LocalizationReader.AddCSVtoken(ref list, ref Line, iStart, ref num);
			}
			return list.ToArray();
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0005FF74 File Offset: 0x0005E174
		private static void AddCSVtoken(ref List<string> list, ref string Line, int iEnd, ref int iWordStart)
		{
			string text = Line.Substring(iWordStart, iEnd - iWordStart);
			iWordStart = iEnd + 1;
			text = text.Replace("\"\"", "\"");
			if (text.Length > 1 && text[0] == '"' && text[text.Length - 1] == '"')
			{
				text = text.Substring(1, text.Length - 2);
			}
			list.Add(text);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0005FFE4 File Offset: 0x0005E1E4
		public static List<string[]> ReadI2CSV(string Text)
		{
			string[] array = new string[] { "[*]" };
			string[] array2 = new string[] { "[ln]" };
			List<string[]> list = new List<string[]>();
			foreach (string text in Text.Split(array2, StringSplitOptions.None))
			{
				list.Add(text.Split(array, StringSplitOptions.None));
			}
			return list;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00060048 File Offset: 0x0005E248
		public static void ValidateFullTerm(ref string Term)
		{
			Term = Term.Replace('\\', '/');
			int num = Term.IndexOf('/');
			if (num < 0)
			{
				return;
			}
			int num2;
			while ((num2 = Term.LastIndexOf('/')) != num)
			{
				Term = Term.Remove(num2, 1);
			}
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0006008A File Offset: 0x0005E28A
		public static string EncodeString(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}
			return str.Replace("\r\n", "<\\n>").Replace("\r", "<\\n>").Replace("\n", "<\\n>");
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x000600C8 File Offset: 0x0005E2C8
		public static string DecodeString(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}
			return str.Replace("<\\n>", "\r\n");
		}
	}
}
