using System;

namespace I2.Loc
{
	public class GlobalParametersExample : RegisterGlobalParameters
	{
		// Token: 0x06000DEF RID: 3567 RVA: 0x000565D8 File Offset: 0x000547D8
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
