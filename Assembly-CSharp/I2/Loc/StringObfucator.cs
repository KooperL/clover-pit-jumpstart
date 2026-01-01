using System;
using System.Text;

namespace I2.Loc
{
	// Token: 0x020001F2 RID: 498
	public class StringObfucator
	{
		// Token: 0x06001442 RID: 5186 RVA: 0x00083CBC File Offset: 0x00081EBC
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

		// Token: 0x06001443 RID: 5187 RVA: 0x00083CF0 File Offset: 0x00081EF0
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

		// Token: 0x06001444 RID: 5188 RVA: 0x0001591C File Offset: 0x00013B1C
		private static string ToBase64(string regularString)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(regularString));
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x00083D24 File Offset: 0x00081F24
		private static string FromBase64(string base64string)
		{
			byte[] array = Convert.FromBase64String(base64string);
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x00083D48 File Offset: 0x00081F48
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

		// Token: 0x04001432 RID: 5170
		public static char[] StringObfuscatorPassword = "ÝúbUu\u00b8CÁÂ§*4PÚ©-á©¾@T6Dl±ÒWâuzÅm4GÐóØ$=Íg,¥Që®iKEßr¡×60Ít4öÃ~^«y:Èd1<QÛÝúbUu\u00b8CÁÂ§*4PÚ©-á©¾@T6Dl±ÒWâuzÅm4GÐóØ$=Íg,¥Që®iKEßr¡×60Ít4öÃ~^«y:Èd".ToCharArray();
	}
}
