using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x0200018F RID: 399
	public class BaseSpecializationManager
	{
		// Token: 0x060011CC RID: 4556 RVA: 0x00075FC0 File Offset: 0x000741C0
		public virtual void InitializeSpecializations()
		{
			this.mSpecializations = new string[]
			{
				"Any", "PC", "Touch", "Controller", "VR", "XBox", "PS4", "PS5", "OculusVR", "ViveVR",
				"GearVR", "Android", "IOS", "Switch"
			};
			this.mSpecializationsFallbacks = new Dictionary<string, string>(StringComparer.Ordinal)
			{
				{ "XBox", "Controller" },
				{ "PS4", "Controller" },
				{ "OculusVR", "VR" },
				{ "ViveVR", "VR" },
				{ "GearVR", "VR" },
				{ "Android", "Touch" },
				{ "IOS", "Touch" }
			};
		}

		// Token: 0x060011CD RID: 4557 RVA: 0x0001485F File Offset: 0x00012A5F
		public virtual string GetCurrentSpecialization()
		{
			if (this.mSpecializations == null)
			{
				this.InitializeSpecializations();
			}
			return "PC";
		}

		// Token: 0x060011CE RID: 4558 RVA: 0x00014874 File Offset: 0x00012A74
		private bool IsTouchInputSupported()
		{
			return Input.touchSupported;
		}

		// Token: 0x060011CF RID: 4559 RVA: 0x000760D0 File Offset: 0x000742D0
		public virtual string GetFallbackSpecialization(string specialization)
		{
			if (this.mSpecializationsFallbacks == null)
			{
				this.InitializeSpecializations();
			}
			string text;
			if (this.mSpecializationsFallbacks.TryGetValue(specialization, out text))
			{
				return text;
			}
			return "Any";
		}

		// Token: 0x040012A9 RID: 4777
		public string[] mSpecializations;

		// Token: 0x040012AA RID: 4778
		public Dictionary<string, string> mSpecializationsFallbacks;
	}
}
