using System;
using UnityEngine;
using UnityEngine.Video;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_VideoPlayer : LocalizeTarget<VideoPlayer>
	{
		// Token: 0x06000FC6 RID: 4038 RVA: 0x0006357B File Offset: 0x0006177B
		static LocalizeTarget_UnityStandard_VideoPlayer()
		{
			LocalizeTarget_UnityStandard_VideoPlayer.AutoRegister();
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x00063582 File Offset: 0x00061782
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<VideoPlayer, LocalizeTarget_UnityStandard_VideoPlayer>
			{
				Name = "VideoPlayer",
				Priority = 100
			});
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x000635A1 File Offset: 0x000617A1
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Video;
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x000635A5 File Offset: 0x000617A5
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x000635A8 File Offset: 0x000617A8
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x000635AB File Offset: 0x000617AB
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x000635AE File Offset: 0x000617AE
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x000635B4 File Offset: 0x000617B4
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			VideoClip clip = this.mTarget.clip;
			primaryTerm = ((clip != null) ? clip.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x000635EC File Offset: 0x000617EC
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
