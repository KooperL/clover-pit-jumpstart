using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	public static class I2Utils
	{
		// Token: 0x06001008 RID: 4104 RVA: 0x00063FD0 File Offset: 0x000621D0
		public static string ReverseText(string source)
		{
			I2Utils.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.source = source;
			int length = CS$<>8__locals1.source.Length;
			CS$<>8__locals1.output = new char[length];
			char[] array = new char[] { '\r', '\n' };
			int i = 0;
			while (i < length)
			{
				int num = CS$<>8__locals1.source.IndexOfAny(array, i);
				if (num < 0)
				{
					num = length;
				}
				I2Utils.<ReverseText>g__Reverse|3_0(i, num - 1, ref CS$<>8__locals1);
				i = num;
				while (i < length && (CS$<>8__locals1.source[i] == '\r' || CS$<>8__locals1.source[i] == '\n'))
				{
					CS$<>8__locals1.output[i] = CS$<>8__locals1.source[i];
					i++;
				}
			}
			return new string(CS$<>8__locals1.output);
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x00064088 File Offset: 0x00062288
		public static string GetValidTermName(string text, bool allowCategory = false)
		{
			if (text == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			char c = '\0';
			bool flag2 = false;
			for (int i = 0; i < text.Length; i++)
			{
				char c2 = text[i];
				bool flag3 = true;
				if ((c2 == '{' || c2 == '[' || c2 == '<') && !flag)
				{
					if (c2 != '{' || i + 1 >= text.Length || text[i + 1] == '[')
					{
						flag = true;
						flag3 = false;
						c = c2;
					}
				}
				else if (flag && ((c2 == '}' && c == '{') || (c2 == ']' && c == '[') || (c2 == '>' && c == '<')))
				{
					flag = false;
					flag3 = false;
				}
				else if (flag)
				{
					flag3 = false;
				}
				if (flag3)
				{
					char c3 = ' ';
					if ((allowCategory && (c2 == '\\' || c2 == '"' || c2 == '/')) || char.IsLetterOrDigit(c2) || ".-_$#@*()[]{}+:?!&',^=<>~`".IndexOf(c2) >= 0)
					{
						c3 = c2;
					}
					if (char.IsWhiteSpace(c2))
					{
						if (!flag2)
						{
							if (stringBuilder.Length > 0)
							{
								stringBuilder.Append(' ');
							}
							flag2 = true;
						}
					}
					else
					{
						flag2 = false;
						stringBuilder.Append(c3);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x000641AC File Offset: 0x000623AC
		public static string SplitLine(string line, int maxCharacters)
		{
			if (maxCharacters <= 0 || line.Length < maxCharacters)
			{
				return line;
			}
			char[] array = line.ToCharArray();
			bool flag = true;
			bool flag2 = false;
			int i = 0;
			int num = 0;
			while (i < array.Length)
			{
				if (flag)
				{
					num++;
					if (array[i] == '\n')
					{
						num = 0;
					}
					if (num >= maxCharacters && char.IsWhiteSpace(array[i]))
					{
						array[i] = '\n';
						flag = false;
						flag2 = false;
					}
				}
				else if (!char.IsWhiteSpace(array[i]))
				{
					flag = true;
					num = 0;
				}
				else if (array[i] != '\n')
				{
					array[i] = '\0';
				}
				else
				{
					if (!flag2)
					{
						array[i] = '\0';
					}
					flag2 = true;
				}
				i++;
			}
			return new string(array.Where((char c) => c > '\0').ToArray<char>());
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00064268 File Offset: 0x00062468
		public static bool FindNextTag(string line, int iStart, out int tagStart, out int tagEnd)
		{
			tagStart = -1;
			tagEnd = -1;
			int length = line.Length;
			tagStart = iStart;
			while (tagStart < length && line[tagStart] != '[' && line[tagStart] != '(' && line[tagStart] != '{' && line[tagStart] != '<')
			{
				tagStart++;
			}
			if (tagStart == length)
			{
				return false;
			}
			bool flag = false;
			for (tagEnd = tagStart + 1; tagEnd < length; tagEnd++)
			{
				char c = line[tagEnd];
				if (c == ']' || c == ')' || c == '}' || c == '>')
				{
					return !flag || I2Utils.FindNextTag(line, tagEnd + 1, out tagStart, out tagEnd);
				}
				if (c > 'ÿ')
				{
					flag = true;
				}
			}
			return false;
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x00064318 File Offset: 0x00062518
		public static string RemoveTags(string text)
		{
			return Regex.Replace(text, "\\{\\[(.*?)]}|\\[(.*?)]|\\<(.*?)>", "");
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0006432C File Offset: 0x0006252C
		public static bool RemoveResourcesPath(ref string sPath)
		{
			int num = sPath.IndexOf("\\Resources\\", StringComparison.Ordinal);
			int num2 = sPath.IndexOf("\\Resources/", StringComparison.Ordinal);
			int num3 = sPath.IndexOf("/Resources\\", StringComparison.Ordinal);
			int num4 = sPath.IndexOf("/Resources/", StringComparison.Ordinal);
			int num5 = Mathf.Max(new int[] { num, num2, num3, num4 });
			bool flag = false;
			if (num5 >= 0)
			{
				sPath = sPath.Substring(num5 + 11);
				flag = true;
			}
			else
			{
				num5 = sPath.LastIndexOfAny(LanguageSourceData.CategorySeparators);
				if (num5 > 0)
				{
					sPath = sPath.Substring(num5 + 1);
				}
			}
			string extension = Path.GetExtension(sPath);
			if (!string.IsNullOrEmpty(extension))
			{
				sPath = sPath.Substring(0, sPath.Length - extension.Length);
			}
			return flag;
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x000643F6 File Offset: 0x000625F6
		public static bool IsPlaying()
		{
			return Application.isPlaying;
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00064404 File Offset: 0x00062604
		public static string GetPath(this Transform tr)
		{
			Transform parent = tr.parent;
			if (tr == null)
			{
				return tr.name;
			}
			return parent.GetPath() + "/" + tr.name;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0006443E File Offset: 0x0006263E
		public static Transform FindObject(string objectPath)
		{
			return I2Utils.FindObject(SceneManager.GetActiveScene(), objectPath);
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0006444C File Offset: 0x0006264C
		public static Transform FindObject(Scene scene, string objectPath)
		{
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				Transform transform = rootGameObjects[i].transform;
				if (transform.name == objectPath)
				{
					return transform;
				}
				if (objectPath.StartsWith(transform.name + "/", StringComparison.Ordinal))
				{
					return I2Utils.FindObject(transform, objectPath.Substring(transform.name.Length + 1));
				}
			}
			return null;
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x000644BC File Offset: 0x000626BC
		public static Transform FindObject(Transform root, string objectPath)
		{
			for (int i = 0; i < root.childCount; i++)
			{
				Transform child = root.GetChild(i);
				if (child.name == objectPath)
				{
					return child;
				}
				if (objectPath.StartsWith(child.name + "/", StringComparison.Ordinal))
				{
					return I2Utils.FindObject(child, objectPath.Substring(child.name.Length + 1));
				}
			}
			return null;
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00064528 File Offset: 0x00062728
		public static H FindInParents<H>(Transform tr) where H : Component
		{
			if (!tr)
			{
				return default(H);
			}
			H h = tr.GetComponent<H>();
			while (!h && tr)
			{
				h = tr.GetComponent<H>();
				tr = tr.parent;
			}
			return h;
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x00064578 File Offset: 0x00062778
		public static string GetCaptureMatch(Match match)
		{
			for (int i = match.Groups.Count - 1; i >= 0; i--)
			{
				if (match.Groups[i].Success)
				{
					return match.Groups[i].ToString();
				}
			}
			return match.ToString();
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x000645C8 File Offset: 0x000627C8
		public static void SendWebRequest(UnityWebRequest www)
		{
			www.SendWebRequest();
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x000645D4 File Offset: 0x000627D4
		[CompilerGenerated]
		internal static void <ReverseText>g__Reverse|3_0(int start, int end, ref I2Utils.<>c__DisplayClass3_0 A_2)
		{
			for (int i = 0; i <= end - start; i++)
			{
				A_2.output[end - i] = A_2.source[start + i];
			}
		}

		public const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";

		public const string NumberChars = "0123456789";

		public const string ValidNameSymbols = ".-_$#@*()[]{}+:?!&',^=<>~`";
	}
}
