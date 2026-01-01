using System;

namespace I2.Loc
{
	// Token: 0x02000187 RID: 391
	public class GlobalParametersExample : RegisterGlobalParameters
	{
		// Token: 0x060011A2 RID: 4514 RVA: 0x00075830 File Offset: 0x00073A30
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
