using System;

namespace I2.Loc
{
	public class RTLFixer
	{
		// Token: 0x0600101F RID: 4127 RVA: 0x0006446E File Offset: 0x0006266E
		public static string Fix(string str)
		{
			return RTLFixer.Fix(str, false, true);
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00064478 File Offset: 0x00062678
		public static string Fix(string str, bool rtl)
		{
			if (rtl)
			{
				return RTLFixer.Fix(str);
			}
			string[] array = str.Split(' ', StringSplitOptions.None);
			string text = "";
			string text2 = "";
			foreach (string text3 in array)
			{
				if (char.IsLower(text3.ToLower()[text3.Length / 2]))
				{
					text = text + RTLFixer.Fix(text2) + text3 + " ";
					text2 = "";
				}
				else
				{
					text2 = text2 + text3 + " ";
				}
			}
			if (text2 != "")
			{
				text += RTLFixer.Fix(text2);
			}
			return text;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0006451C File Offset: 0x0006271C
		public static string Fix(string str, bool showTashkeel, bool useHinduNumbers)
		{
			string text = HindiFixer.Fix(str);
			if (text != str)
			{
				return text;
			}
			RTLFixerTool.showTashkeel = showTashkeel;
			RTLFixerTool.useHinduNumbers = useHinduNumbers;
			if (str.Contains("\n"))
			{
				str = str.Replace("\n", Environment.NewLine);
			}
			if (!str.Contains(Environment.NewLine))
			{
				return RTLFixerTool.FixLine(str);
			}
			string[] array = new string[] { Environment.NewLine };
			string[] array2 = str.Split(array, StringSplitOptions.None);
			if (array2.Length == 0)
			{
				return RTLFixerTool.FixLine(str);
			}
			if (array2.Length == 1)
			{
				return RTLFixerTool.FixLine(str);
			}
			string text2 = RTLFixerTool.FixLine(array2[0]);
			int i = 1;
			if (array2.Length > 1)
			{
				while (i < array2.Length)
				{
					text2 = text2 + Environment.NewLine + RTLFixerTool.FixLine(array2[i]);
					i++;
				}
			}
			return text2;
		}
	}
}
