using System;
using TMPro;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001C7 RID: 455
	public class LocalizeTarget_TextMeshPro_Label : LocalizeTarget<TextMeshPro>
	{
		// Token: 0x06001359 RID: 4953 RVA: 0x000152A2 File Offset: 0x000134A2
		static LocalizeTarget_TextMeshPro_Label()
		{
			LocalizeTarget_TextMeshPro_Label.AutoRegister();
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x000152A9 File Offset: 0x000134A9
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void AutoRegister()
		{
			LocalizationManager.RegisterTarget(new LocalizeTargetDesc_Type<TextMeshPro, LocalizeTarget_TextMeshPro_Label>
			{
				Name = "TextMeshPro Label",
				Priority = 100
			});
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override eTermType GetPrimaryTermType(Localize cmp)
		{
			return eTermType.Text;
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x00007C86 File Offset: 0x00005E86
		public override eTermType GetSecondaryTermType(Localize cmp)
		{
			return eTermType.Font;
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x00007C86 File Offset: 0x00005E86
		public override bool CanUseSecondaryTerm()
		{
			return true;
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00007C86 File Offset: 0x00005E86
		public override bool AllowMainTermToBeRTL()
		{
			return true;
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x00008AE5 File Offset: 0x00006CE5
		public override bool AllowSecondTermToBeRTL()
		{
			return false;
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x00080908 File Offset: 0x0007EB08
		public override void GetFinalTerms(Localize cmp, string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = (this.mTarget ? this.mTarget.text : null);
			secondaryTerm = ((this.mTarget.font != null) ? this.mTarget.font.name : string.Empty);
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x00080960 File Offset: 0x0007EB60
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

		// Token: 0x06001362 RID: 4962 RVA: 0x00080B14 File Offset: 0x0007ED14
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

		// Token: 0x06001363 RID: 4963 RVA: 0x00080B8C File Offset: 0x0007ED8C
		internal static void InitAlignment_TMPro(bool isRTL, TextAlignmentOptions alignment, out TextAlignmentOptions alignLTR, out TextAlignmentOptions alignRTL)
		{
			alignRTL = alignment;
			alignLTR = alignment;
			if (isRTL)
			{
				if (alignment <= TextAlignmentOptions.BottomRight)
				{
					if (alignment <= TextAlignmentOptions.Left)
					{
						if (alignment == TextAlignmentOptions.TopLeft)
						{
							alignLTR = TextAlignmentOptions.TopRight;
							return;
						}
						if (alignment == TextAlignmentOptions.TopRight)
						{
							alignLTR = TextAlignmentOptions.TopLeft;
							return;
						}
						if (alignment != TextAlignmentOptions.Left)
						{
							return;
						}
						alignLTR = TextAlignmentOptions.Right;
						return;
					}
					else
					{
						if (alignment == TextAlignmentOptions.Right)
						{
							alignLTR = TextAlignmentOptions.Left;
							return;
						}
						if (alignment == TextAlignmentOptions.BottomLeft)
						{
							alignLTR = TextAlignmentOptions.BottomRight;
							return;
						}
						if (alignment != TextAlignmentOptions.BottomRight)
						{
							return;
						}
						alignLTR = TextAlignmentOptions.BottomLeft;
						return;
					}
				}
				else if (alignment <= TextAlignmentOptions.MidlineLeft)
				{
					if (alignment == TextAlignmentOptions.BaselineLeft)
					{
						alignLTR = TextAlignmentOptions.BaselineRight;
						return;
					}
					if (alignment == TextAlignmentOptions.BaselineRight)
					{
						alignLTR = TextAlignmentOptions.BaselineLeft;
						return;
					}
					if (alignment != TextAlignmentOptions.MidlineLeft)
					{
						return;
					}
					alignLTR = TextAlignmentOptions.MidlineRight;
					return;
				}
				else
				{
					if (alignment == TextAlignmentOptions.MidlineRight)
					{
						alignLTR = TextAlignmentOptions.MidlineLeft;
						return;
					}
					if (alignment == TextAlignmentOptions.CaplineLeft)
					{
						alignLTR = TextAlignmentOptions.CaplineRight;
						return;
					}
					if (alignment != TextAlignmentOptions.CaplineRight)
					{
						return;
					}
					alignLTR = TextAlignmentOptions.CaplineLeft;
					return;
				}
			}
			else if (alignment <= TextAlignmentOptions.BottomRight)
			{
				if (alignment <= TextAlignmentOptions.Left)
				{
					if (alignment == TextAlignmentOptions.TopLeft)
					{
						alignRTL = TextAlignmentOptions.TopRight;
						return;
					}
					if (alignment == TextAlignmentOptions.TopRight)
					{
						alignRTL = TextAlignmentOptions.TopLeft;
						return;
					}
					if (alignment != TextAlignmentOptions.Left)
					{
						return;
					}
					alignRTL = TextAlignmentOptions.Right;
					return;
				}
				else
				{
					if (alignment == TextAlignmentOptions.Right)
					{
						alignRTL = TextAlignmentOptions.Left;
						return;
					}
					if (alignment == TextAlignmentOptions.BottomLeft)
					{
						alignRTL = TextAlignmentOptions.BottomRight;
						return;
					}
					if (alignment != TextAlignmentOptions.BottomRight)
					{
						return;
					}
					alignRTL = TextAlignmentOptions.BottomLeft;
					return;
				}
			}
			else if (alignment <= TextAlignmentOptions.MidlineLeft)
			{
				if (alignment == TextAlignmentOptions.BaselineLeft)
				{
					alignRTL = TextAlignmentOptions.BaselineRight;
					return;
				}
				if (alignment == TextAlignmentOptions.BaselineRight)
				{
					alignRTL = TextAlignmentOptions.BaselineLeft;
					return;
				}
				if (alignment != TextAlignmentOptions.MidlineLeft)
				{
					return;
				}
				alignRTL = TextAlignmentOptions.MidlineRight;
				return;
			}
			else
			{
				if (alignment == TextAlignmentOptions.MidlineRight)
				{
					alignRTL = TextAlignmentOptions.MidlineLeft;
					return;
				}
				if (alignment == TextAlignmentOptions.CaplineLeft)
				{
					alignRTL = TextAlignmentOptions.CaplineRight;
					return;
				}
				if (alignment != TextAlignmentOptions.CaplineRight)
				{
					return;
				}
				alignRTL = TextAlignmentOptions.CaplineLeft;
				return;
			}
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x000152C8 File Offset: 0x000134C8
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

		// Token: 0x06001365 RID: 4965 RVA: 0x000152F9 File Offset: 0x000134F9
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

		// Token: 0x04001396 RID: 5014
		private TextAlignmentOptions mAlignment_RTL = TextAlignmentOptions.Right;

		// Token: 0x04001397 RID: 5015
		private TextAlignmentOptions mAlignment_LTR = TextAlignmentOptions.Left;

		// Token: 0x04001398 RID: 5016
		private bool mAlignmentWasRTL;

		// Token: 0x04001399 RID: 5017
		private bool mInitializeAlignment = true;
	}
}
