using System;
using UnityEngine;
using UnityEngine.Video;

namespace I2.Loc
{
	// Token: 0x020001D1 RID: 465
	public class LocalizeTarget_UnityStandard_VideoPlayer : LocalizeTarget<VideoPlayer>
	{
		// Token: 0x060013B4 RID: 5044 RVA: 0x000154F9 File Offset: 0x000136F9
		static LocalizeTarget_UnityStandard_VideoPlayer()
		{
			LocalizeTarget_UnityStandard_VideoPlayer.AutoRegister();
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00015500 File Offset: 0x00013700
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<VideoPlayer, LocalizeTarget_UnityStandard_VideoPlayer>
			{
				Name = "VideoPlayer",
				Priority = 100
			});
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x0001551F File Offset: 0x0001371F
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Video;
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x000814C8 File Offset: 0x0007F6C8
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			VideoClip clip = this.mTarget.clip;
			primaryTerm = ((clip != null) ? clip.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x00081500 File Offset: 0x0007F700
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
