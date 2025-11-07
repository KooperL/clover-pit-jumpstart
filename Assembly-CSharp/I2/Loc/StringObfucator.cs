using System;
using System.Text;

namespace I2.Loc
{
	public class StringObfucator
	{
		// Token: 0x06001037 RID: 4151 RVA: 0x00065A5C File Offset: 0x00063C5C
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

		// Token: 0x06001038 RID: 4152 RVA: 0x00065A90 File Offset: 0x00063C90
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

		// Token: 0x06001039 RID: 4153 RVA: 0x00065AC4 File Offset: 0x00063CC4
		private static string ToBase64(string regularString)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(regularString));
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x00065AD8 File Offset: 0x00063CD8
		private static string FromBase64(string base64string)
		{
			byte[] array = Convert.FromBase64String(base64string);
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x00065AFC File Offset: 0x00063CFC
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
