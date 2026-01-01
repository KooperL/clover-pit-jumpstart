using System;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001C2 RID: 450
	public abstract class ILocalizeTarget : ScriptableObject
	{
		// Token: 0x06001344 RID: 4932
		public abstract bool IsValid(Localize cmp);

		// Token: 0x06001345 RID: 4933
		public abstract void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm);

		// Token: 0x06001346 RID: 4934
		public abstract void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation);

		// Token: 0x06001347 RID: 4935
		public abstract bool CanUseSecondaryTerm();

		// Token: 0x06001348 RID: 4936
		public abstract bool AllowMainTermToBeRTL();

		// Token: 0x06001349 RID: 4937
		public abstract bool AllowSecondTermToBeRTL();

		// Token: 0x0600134A RID: 4938
		public abstract eTermType GetPrimaryTermType(Localize cmp);

		// Token: 0x0600134B RID: 4939
		public abstract eTermType GetSecondaryTermType(Localize cmp);
	}
}
