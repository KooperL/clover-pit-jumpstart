using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	public class BaseSpecializationManager
	{
		// Token: 0x06000E30 RID: 3632 RVA: 0x00057774 File Offset: 0x00055974
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

		// Token: 0x06000E31 RID: 3633 RVA: 0x00057883 File Offset: 0x00055A83
		public virtual string GetCurrentSpecialization()
		{
			if (this.mSpecializations == null)
			{
				this.InitializeSpecializations();
			}
			return "PC";
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00057898 File Offset: 0x00055A98
		private bool IsTouchInputSupported()
		{
			return Input.touchSupported;
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x000578A0 File Offset: 0x00055AA0
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
