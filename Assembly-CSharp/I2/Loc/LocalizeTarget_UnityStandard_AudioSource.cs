using System;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_AudioSource : LocalizeTarget<AudioSource>
	{
		// Token: 0x06000F6C RID: 3948 RVA: 0x00062680 File Offset: 0x00060880
		static LocalizeTarget_UnityStandard_AudioSource()
		{
			LocalizeTarget_UnityStandard_AudioSource.AutoRegister();
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x00062687 File Offset: 0x00060887
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<AudioSource, LocalizeTarget_UnityStandard_AudioSource>
			{
				Name = "AudioSource",
				Priority = 100
			});
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x000626A6 File Offset: 0x000608A6
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.AudioClip;
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x000626A9 File Offset: 0x000608A9
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x000626AC File Offset: 0x000608AC
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x000626AF File Offset: 0x000608AF
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x000626B2 File Offset: 0x000608B2
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x000626B8 File Offset: 0x000608B8
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			AudioClip clip = this.mTarget.clip;
			primaryTerm = (clip ? clip.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x000626F0 File Offset: 0x000608F0
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			bool flag = (this.mTarget.isPlaying || this.mTarget.loop) && Application.isPlaying;
			global::UnityEngine.Object clip = this.mTarget.clip;
			AudioClip audioClip = cmp.FindTranslatedObject<AudioClip>(mainTranslation);
			if (clip != audioClip)
			{
				this.mTarget.clip = audioClip;
			}
			if (flag && this.mTarget.clip)
			{
				this.mTarget.Play();
			}
		}
	}
}
