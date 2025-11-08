using System;
using UnityEngine;
using UnityEngine.Video;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_VideoPlayer : LocalizeTarget<VideoPlayer>
	{
		// Token: 0x06000FAF RID: 4015 RVA: 0x00062D9F File Offset: 0x00060F9F
		static LocalizeTarget_UnityStandard_VideoPlayer()
		{
			LocalizeTarget_UnityStandard_VideoPlayer.AutoRegister();
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00062DA6 File Offset: 0x00060FA6
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<VideoPlayer, LocalizeTarget_UnityStandard_VideoPlayer>
			{
				Name = "VideoPlayer",
				Priority = 100
			});
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00062DC5 File Offset: 0x00060FC5
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Video;
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x00062DC9 File Offset: 0x00060FC9
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x00062DCC File Offset: 0x00060FCC
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x00062DCF File Offset: 0x00060FCF
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x00062DD2 File Offset: 0x00060FD2
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00062DD8 File Offset: 0x00060FD8
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			VideoClip clip = this.mTarget.clip;
			primaryTerm = ((clip != null) ? clip.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00062E10 File Offset: 0x00061010
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			VideoClip clip = this.mTarget.clip;
			if (clip == null || clip.name != mainTranslation)
			{
				this.mTarget.clip = cmp.FindTranslatedObject<VideoClip>(mainTranslation);
			}
		}
	}
}
