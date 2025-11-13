using System;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_UnityStandard_AudioSource : LocalizeTarget<AudioSource>
	{
		// Token: 0x06000F83 RID: 3971 RVA: 0x00062E5C File Offset: 0x0006105C
		static LocalizeTarget_UnityStandard_AudioSource()
		{
			LocalizeTarget_UnityStandard_AudioSource.AutoRegister();
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x00062E63 File Offset: 0x00061063
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<AudioSource, LocalizeTarget_UnityStandard_AudioSource>
			{
				Name = "AudioSource",
				Priority = 100
			});
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x00062E82 File Offset: 0x00061082
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.AudioClip;
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x00062E85 File Offset: 0x00061085
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x00062E88 File Offset: 0x00061088
		public override bool CanUseSecondaryTerm()
		{
			return false;
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x00062E8B File Offset: 0x0006108B
		public override bool AllowMainTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x00062E8E File Offset: 0x0006108E
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x00062E94 File Offset: 0x00061094
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			AudioClip clip = this.mTarget.clip;
			primaryTerm = (clip ? clip.name : string.Empty);
			secondaryTerm = null;
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x00062ECC File Offset: 0x000610CC
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
