using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	public class BaseSpecializationManager
	{
		// Token: 0x06000E19 RID: 3609 RVA: 0x00056F98 File Offset: 0x00055198
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

		// Token: 0x06000E1A RID: 3610 RVA: 0x000570A7 File Offset: 0x000552A7
		public virtual string GetCurrentSpecialization()
		{
			if (this.mSpecializations == null)
			{
				this.InitializeSpecializations();
			}
			return "PC";
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x000570BC File Offset: 0x000552BC
		private bool IsTouchInputSupported()
		{
			return Input.touchSupported;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x000570C4 File Offset: 0x000552C4
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

		public string[] mSpecializations;

		public Dictionary<string, string> mSpecializationsFallbacks;
	}
}
