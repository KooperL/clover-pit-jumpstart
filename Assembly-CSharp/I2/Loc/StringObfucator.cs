using System;
using System.Text;

namespace I2.Loc
{
	public class StringObfucator
	{
		// Token: 0x0600104E RID: 4174 RVA: 0x00066238 File Offset: 0x00064438
		public static string Encode(string NormalString)
		{
			string text;
			try
			{
				text = StringObfucator.ToBase64(StringObfucator.XoREncode(NormalString));
			}
			catch (Exception)
			{
				text = null;
			}
			return text;
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x0006626C File Offset: 0x0006446C
		public static string Decode(string ObfucatedString)
		{
			string text;
			try
			{
				text = StringObfucator.XoREncode(StringObfucator.FromBase64(ObfucatedString));
			}
			catch (Exception)
			{
				text = null;
			}
			return text;
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x000662A0 File Offset: 0x000644A0
		private static string ToBase64(string regularString)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(regularString));
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x000662B4 File Offset: 0x000644B4
		private static string FromBase64(string base64string)
		{
			byte[] array = Convert.FromBase64String(base64string);
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x000662D8 File Offset: 0x000644D8
		private static string XoREncode(string NormalString)
		{
			string text;
			try
			{
				char[] stringObfuscatorPassword = StringObfucator.StringObfuscatorPassword;
				char[] array = NormalString.ToCharArray();
				int num = stringObfuscatorPassword.Length;
				int i = 0;
				int num2 = array.Length;
				while (i < num2)
				{
					array[i] = array[i] ^ stringObfuscatorPassword[i % num] ^ (char)((byte)((i % 2 == 0) ? (i * 23) : (-i * 51)));
					i++;
				}
				text = new string(array);
			}
			catch (Exception)
			{
				text = null;
			}
			return text;
		}

		public static char[] StringObfuscatorPassword = "ÝúbUu\u00b8CÁÂ§*4PÚ©-á©¾@T6Dl±ÒWâuzÅm4GÐóØ$=Íg,¥Që®iKEßr¡×60Ít4öÃ~^«y:Èd1<QÛÝúbUu\u00b8CÁÂ§*4PÚ©-á©¾@T6Dl±ÒWâuzÅm4GÐóØ$=Íg,¥Që®iKEßr¡×60Ít4öÃ~^«y:Èd".ToCharArray();
	}
}
