using System;
using TMPro;
using UnityEngine;

namespace I2.Loc
{
	public class LocalizeTarget_TextMeshPro_Label : LocalizeTarget<TextMeshPro>
	{
		// Token: 0x06000F6B RID: 3947 RVA: 0x000626CE File Offset: 0x000608CE
		static LocalizeTarget_TextMeshPro_Label()
		{
			LocalizeTarget_TextMeshPro_Label.AutoRegister();
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x000626D5 File Offset: 0x000608D5
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMeshPro, LocalizeTarget_TextMeshPro_Label>
			{
				Name = "TextMeshPro Label",
				Priority = 100
			});
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x000626F4 File Offset: 0x000608F4
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x000626F7 File Offset: 0x000608F7
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x000626FA File Offset: 0x000608FA
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x000626FD File Offset: 0x000608FD
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00062700 File Offset: 0x00060900
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00062704 File Offset: 0x00060904
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.font != null) ? this.mTarget.font.name : string.Empty);
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x0006275C File Offset: 0x0006095C
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

		// Token: 0x06000F74 RID: 3956 RVA: 0x00062910 File Offset: 0x00060B10
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

		// Token: 0x06000F75 RID: 3957 RVA: 0x00062988 File Offset: 0x00060B88
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

		// Token: 0x06000F76 RID: 3958 RVA: 0x00062B6C File Offset: 0x00060D6C
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

		// Token: 0x06000F77 RID: 3959 RVA: 0x00062B9D File Offset: 0x00060D9D
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
