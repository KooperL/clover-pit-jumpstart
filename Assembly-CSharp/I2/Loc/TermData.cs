using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	[Serializable]
	public class TermData
	{
		// Token: 0x06000FD8 RID: 4056 RVA: 0x000632A4 File Offset: 0x000614A4
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

		// Token: 0x06000FD9 RID: 4057 RVA: 0x000632E9 File Offset: 0x000614E9
		public void SetTranslation(int idx, string translation, string specialization = null)
		{
			this.Languages[idx] = SpecializationManager.SetSpecializedText(this.Languages[idx], translation, specialization);
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x00063304 File Offset: 0x00061504
		public void RemoveSpecialization(string specialization)
		{
			for (int i = 0; i < this.Languages.Length; i++)
			{
				this.RemoveSpecialization(i, specialization);
			}
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0006332C File Offset: 0x0006152C
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

		// Token: 0x06000FDC RID: 4060 RVA: 0x00063386 File Offset: 0x00061586
		public bool IsAutoTranslated(int idx, bool IsTouch)
		{
			return (this.Flags[idx] & 2) > 0;
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x00063398 File Offset: 0x00061598
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

		// Token: 0x06000FDE RID: 4062 RVA: 0x00063448 File Offset: 0x00061648
		public bool IsTerm(string name, bool allowCategoryMistmatch)
		{
			if (!allowCategoryMistmatch)
			{
				return name == this.Term;
			}
			return name == LanguageSourceData.GetKeyFromFullTerm(this.Term, false);
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x0006346C File Offset: 0x0006166C
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

		// Token: 0x06000FE0 RID: 4064 RVA: 0x000634B4 File Offset: 0x000616B4
		public List<string> GetAllSpecializations()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.Languages.Length; i++)
			{
				SpecializationManager.AppendSpecializations(this.Languages[i], list);
			}
			return list;
		}

		public string Term = string.Empty;

		public eTermType TermType;

		[NonSerialized]
		public string Description;

		public string[] Languages = Array.Empty<string>();

		public byte[] Flags = Array.Empty<byte>();

		[SerializeField]
		private string[] Languages_Touch;
	}
}
