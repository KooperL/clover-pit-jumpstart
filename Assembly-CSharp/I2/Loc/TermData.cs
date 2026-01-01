using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x020001D7 RID: 471
	[Serializable]
	public class TermData
	{
		// Token: 0x060013DD RID: 5085 RVA: 0x00081880 File Offset: 0x0007FA80
		public string GetTranslation(int idx, string specialization = null, bool editMode = false)
		{
			string text = this.Languages[idx];
			if (text != null)
			{
				text = SpecializationManager.GetSpecializedText(text, specialization);
				if (!editMode)
				{
					text = text.Replace("[i2nt]", "").Replace("[/i2nt]", "");
				}
			}
			return text;
		}

		// Token: 0x060013DE RID: 5086 RVA: 0x00015610 File Offset: 0x00013810
		public void SetTranslation(int idx, string translation, string specialization = null)
		{
			this.Languages[idx] = SpecializationManager.SetSpecializedText(this.Languages[idx], translation, specialization);
		}

		// Token: 0x060013DF RID: 5087 RVA: 0x000818C8 File Offset: 0x0007FAC8
		public void RemoveSpecialization(string specialization)
		{
			for (int i = 0; i < this.Languages.Length; i++)
			{
				this.RemoveSpecialization(i, specialization);
			}
		}

		// Token: 0x060013E0 RID: 5088 RVA: 0x000818F0 File Offset: 0x0007FAF0
		public void RemoveSpecialization(int idx, string specialization)
		{
			string text = this.Languages[idx];
			if (specialization == "Any" || !text.Contains("[i2s_" + specialization + "]"))
			{
				return;
			}
			Dictionary<string, string> specializations = SpecializationManager.GetSpecializations(text, null);
			specializations.Remove(specialization);
			this.Languages[idx] = SpecializationManager.SetSpecializedText(specializations);
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x00015629 File Offset: 0x00013829
		public bool IsAutoTranslated(int idx, bool IsTouch)
		{
			return (this.Flags[idx] & 2) > 0;
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x0008194C File Offset: 0x0007FB4C
		public void Validate()
		{
			int num = Mathf.Max(this.Languages.Length, this.Flags.Length);
			if (this.Languages.Length != num)
			{
				Array.Resize<string>(ref this.Languages, num);
			}
			if (this.Flags.Length != num)
			{
				Array.Resize<byte>(ref this.Flags, num);
			}
			if (this.Languages_Touch != null)
			{
				for (int i = 0; i < Mathf.Min(this.Languages_Touch.Length, num); i++)
				{
					if (string.IsNullOrEmpty(this.Languages[i]) && !string.IsNullOrEmpty(this.Languages_Touch[i]))
					{
						this.Languages[i] = this.Languages_Touch[i];
						this.Languages_Touch[i] = null;
					}
				}
				this.Languages_Touch = null;
			}
		}

		// Token: 0x060013E3 RID: 5091 RVA: 0x00015638 File Offset: 0x00013838
		public bool IsTerm(string name, bool allowCategoryMistmatch)
		{
			if (!allowCategoryMistmatch)
			{
				return name == this.Term;
			}
			return name == LanguageSourceData.GetKeyFromFullTerm(this.Term, false);
		}

		// Token: 0x060013E4 RID: 5092 RVA: 0x000819FC File Offset: 0x0007FBFC
		public bool HasSpecializations()
		{
			for (int i = 0; i < this.Languages.Length; i++)
			{
				if (!string.IsNullOrEmpty(this.Languages[i]) && this.Languages[i].Contains("[i2s_"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00081A44 File Offset: 0x0007FC44
		public List<string> GetAllSpecializations()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.Languages.Length; i++)
			{
				SpecializationManager.AppendSpecializations(this.Languages[i], list);
			}
			return list;
		}

		// Token: 0x040013B6 RID: 5046
		public string Term = string.Empty;

		// Token: 0x040013B7 RID: 5047
		public eTermType TermType;

		// Token: 0x040013B8 RID: 5048
		[NonSerialized]
		public string Description;

		// Token: 0x040013B9 RID: 5049
		public string[] Languages = Array.Empty<string>();

		// Token: 0x040013BA RID: 5050
		public byte[] Flags = Array.Empty<byte>();

		// Token: 0x040013BB RID: 5051
		[SerializeField]
		private string[] Languages_Touch;
	}
}
