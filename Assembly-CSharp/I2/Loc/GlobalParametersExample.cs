using System;

namespace I2.Loc
{
	public class GlobalParametersExample : RegisterGlobalParameters
	{
		// Token: 0x06000E06 RID: 3590 RVA: 0x00056DB4 File Offset: 0x00054FB4
		public override string GetParameterValue(string ParamName)
		{
			if (ParamName == "WINNER")
			{
				return "Javier";
			}
			if (ParamName == "NUM PLAYERS")
			{
				return 5.ToString();
			}
			return null;
		}
	}
}
