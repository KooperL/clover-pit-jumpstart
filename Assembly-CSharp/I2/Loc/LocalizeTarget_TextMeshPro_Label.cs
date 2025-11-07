using System;
using TMPro;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_TextMeshPro_Label : LocalizeTarget<TextMeshPro>
	{
		// Token: 0x06000F54 RID: 3924 RVA: 0x00061EF2 File Offset: 0x000600F2
		static LocalizeTarget_TextMeshPro_Label()
		{
			LocalizeTarget_TextMeshPro_Label.AutoRegister();
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x00061EF9 File Offset: 0x000600F9
		[RuntimeInitializeOnLoadMethod(1)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMeshPro, LocalizeTarget_TextMeshPro_Label>
			{
				Name = "TextMeshPro Label",
				Priority = 100
			});
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x00061F18 File Offset: 0x00060118
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x00061F1B File Offset: 0x0006011B
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x00061F1E File Offset: 0x0006011E
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x00061F21 File Offset: 0x00060121
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x00061F24 File Offset: 0x00060124
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x00061F28 File Offset: 0x00060128
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.font != null) ? this.mTarget.font.name : string.Empty);
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x00061F80 File Offset: 0x00060180
		public override void DoLocalize(Localize cmp, string mainTranslation, string secondaryTranslation)
		{
			TMP_FontAsset tmp_FontAsset = cmp.GetSecondaryTranslatedObj<TMP_FontAsset>(ref mainTranslation, ref secondaryTranslation);
			if (tmp_FontAsset != null)
			{
				LocalizeTarget_TextMeshPro_Label.SetFont(this.mTarget, tmp_FontAsset);
			}
			else
			{
				Material secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Material>(ref mainTranslation, ref secondaryTranslation);
				if (secondaryTranslatedObj != null && this.mTarget.fontMaterial != secondaryTranslatedObj)
				{
					if (!secondaryTranslatedObj.name.StartsWith(this.mTarget.font.name, StringComparison.Ordinal))
					{
						tmp_FontAsset = LocalizeTarget_TextMeshPro_Label.GetTMPFontFromMaterial(cmp, secondaryTranslation.EndsWith(secondaryTranslatedObj.name, StringComparison.Ordinal) ? secondaryTranslation : secondaryTranslatedObj.name);
						if (tmp_FontAsset != null)
						{
							LocalizeTarget_TextMeshPro_Label.SetFont(this.mTarget, tmp_FontAsset);
						}
					}
					LocalizeTarget_TextMeshPro_Label.SetMaterial(this.mTarget, secondaryTranslatedObj);
				}
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
				LocalizeTarget_TextMeshPro_Label.InitAlignment_TMPro(this.mAlignmentWasRTL, this.mTarget.alignment, out this.mAlignment_LTR, out this.mAlignment_RTL);
			}
			else
			{
				TextAlignmentOptions textAlignmentOptions;
				TextAlignmentOptions textAlignmentOptions2;
				LocalizeTarget_TextMeshPro_Label.InitAlignment_TMPro(this.mAlignmentWasRTL, this.mTarget.alignment, out textAlignmentOptions, out textAlignmentOptions2);
				if ((this.mAlignmentWasRTL && this.mAlignment_RTL != textAlignmentOptions2) || (!this.mAlignmentWasRTL && this.mAlignment_LTR != textAlignmentOptions))
				{
					this.mAlignment_LTR = textAlignmentOptions;
					this.mAlignment_RTL = textAlignmentOptions2;
				}
				this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
			}
			if (mainTranslation != null && this.mTarget.text != mainTranslation)
			{
				if (cmp.CorrectAlignmentForRTL)
				{
					this.mTarget.alignment = (LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR);
				}
				this.mTarget.isRightToLeftText = LocalizationManager.IsRight2Left;
				if (LocalizationManager.IsRight2Left)
				{
					mainTranslation = I2Utils.ReverseText(mainTranslation);
				}
				this.mTarget.text = mainTranslation;
			}
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00062134 File Offset: 0x00060334
		internal static TMP_FontAsset GetTMPFontFromMaterial(Localize cmp, string matName)
		{
			string text = " .\\/-[]()";
			int i = matName.Length - 1;
			while (i > 0)
			{
				while (i > 0 && text.IndexOf(matName[i]) >= 0)
				{
					i--;
				}
				if (i <= 0)
				{
					break;
				}
				string text2 = matName.Substring(0, i + 1);
				TMP_FontAsset @object = cmp.GetObject<TMP_FontAsset>(text2);
				if (@object != null)
				{
					return @object;
				}
				while (i > 0 && text.IndexOf(matName[i]) < 0)
				{
					i--;
				}
			}
			return null;
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x000621AC File Offset: 0x000603AC
		internal static void InitAlignment_TMPro(bool isRTL, TextAlignmentOptions alignment, out TextAlignmentOptions alignLTR, out TextAlignmentOptions alignRTL)
		{
			alignRTL = alignment;
			alignLTR = alignment;
			if (isRTL)
			{
				if (alignment <= 1028)
				{
					if (alignment <= 513)
					{
						if (alignment == 257)
						{
							alignLTR = 260;
							return;
						}
						if (alignment == 260)
						{
							alignLTR = 257;
							return;
						}
						if (alignment != 513)
						{
							return;
						}
						alignLTR = 516;
						return;
					}
					else
					{
						if (alignment == 516)
						{
							alignLTR = 513;
							return;
						}
						if (alignment == 1025)
						{
							alignLTR = 1028;
							return;
						}
						if (alignment != 1028)
						{
							return;
						}
						alignLTR = 1025;
						return;
					}
				}
				else if (alignment <= 4097)
				{
					if (alignment == 2049)
					{
						alignLTR = 2052;
						return;
					}
					if (alignment == 2052)
					{
						alignLTR = 2049;
						return;
					}
					if (alignment != 4097)
					{
						return;
					}
					alignLTR = 4100;
					return;
				}
				else
				{
					if (alignment == 4100)
					{
						alignLTR = 4097;
						return;
					}
					if (alignment == 8193)
					{
						alignLTR = 8196;
						return;
					}
					if (alignment != 8196)
					{
						return;
					}
					alignLTR = 8193;
					return;
				}
			}
			else if (alignment <= 1028)
			{
				if (alignment <= 513)
				{
					if (alignment == 257)
					{
						alignRTL = 260;
						return;
					}
					if (alignment == 260)
					{
						alignRTL = 257;
						return;
					}
					if (alignment != 513)
					{
						return;
					}
					alignRTL = 516;
					return;
				}
				else
				{
					if (alignment == 516)
					{
						alignRTL = 513;
						return;
					}
					if (alignment == 1025)
					{
						alignRTL = 1028;
						return;
					}
					if (alignment != 1028)
					{
						return;
					}
					alignRTL = 1025;
					return;
				}
			}
			else if (alignment <= 4097)
			{
				if (alignment == 2049)
				{
					alignRTL = 2052;
					return;
				}
				if (alignment == 2052)
				{
					alignRTL = 2049;
					return;
				}
				if (alignment != 4097)
				{
					return;
				}
				alignRTL = 4100;
				return;
			}
			else
			{
				if (alignment == 4100)
				{
					alignRTL = 4097;
					return;
				}
				if (alignment == 8193)
				{
					alignRTL = 8196;
					return;
				}
				if (alignment != 8196)
				{
					return;
				}
				alignRTL = 8193;
				return;
			}
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00062390 File Offset: 0x00060590
		internal static void SetFont(TMP_Text label, TMP_FontAsset newFont)
		{
			if (label.font != newFont)
			{
				label.font = newFont;
			}
			if (label.linkedTextComponent != null)
			{
				LocalizeTarget_TextMeshPro_Label.SetFont(label.linkedTextComponent, newFont);
			}
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x000623C1 File Offset: 0x000605C1
		internal static void SetMaterial(TMP_Text label, Material newMat)
		{
			if (label.fontSharedMaterial != newMat)
			{
				label.fontSharedMaterial = newMat;
			}
			if (label.linkedTextComponent != null)
			{
				LocalizeTarget_TextMeshPro_Label.SetMaterial(label.linkedTextComponent, newMat);
			}
		}

		private TextAlignmentOptions mAlignment_RTL = 516;

		private TextAlignmentOptions mAlignment_LTR = 513;

		private bool mAlignmentWasRTL;

		private bool mInitializeAlignment = true;
	}
}
