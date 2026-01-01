using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace I2.Loc
{
	// Token: 0x020001B2 RID: 434
	[AddComponentMenu("I2/Localization/I2 Localize")]
	public class Localize : MonoBehaviour
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060012B3 RID: 4787 RVA: 0x00014E6D File Offset: 0x0001306D
		// (set) Token: 0x060012B4 RID: 4788 RVA: 0x00014E75 File Offset: 0x00013075
		public string Term
		{
			get
			{
				return this.mTerm;
			}
			set
			{
				this.SetTerm(value);
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060012B5 RID: 4789 RVA: 0x00014E7E File Offset: 0x0001307E
		// (set) Token: 0x060012B6 RID: 4790 RVA: 0x00014E86 File Offset: 0x00013086
		public string SecondaryTerm
		{
			get
			{
				return this.mTermSecondary;
			}
			set
			{
				this.SetTerm(null, value);
			}
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00014E90 File Offset: 0x00013090
		private void Awake()
		{
			this.UpdateAssetDictionary();
			this.FindTarget();
			if (this.LocalizeOnAwake)
			{
				this.OnLocalize(false);
			}
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x00014EAE File Offset: 0x000130AE
		private void OnEnable()
		{
			this.OnLocalize(false);
		}

		// Token: 0x060012B9 RID: 4793 RVA: 0x00014EB7 File Offset: 0x000130B7
		public bool HasCallback()
		{
			return this.LocalizeCallBack.HasCallback() || this.LocalizeEvent.GetPersistentEventCount() > 0;
		}

		// Token: 0x060012BA RID: 4794 RVA: 0x0007E600 File Offset: 0x0007C800
		public void OnLocalize(bool Force = false)
		{
			if (!Force && (!base.enabled || base.gameObject == null || !base.gameObject.activeInHierarchy))
			{
				return;
			}
			if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
			{
				return;
			}
			if (!this.AlwaysForceLocalize && !Force && !this.HasCallback() && this.LastLocalizedLanguage == LocalizationManager.CurrentLanguage)
			{
				return;
			}
			this.LastLocalizedLanguage = LocalizationManager.CurrentLanguage;
			if (string.IsNullOrEmpty(this.FinalTerm) || string.IsNullOrEmpty(this.FinalSecondaryTerm))
			{
				this.GetFinalTerms(out this.FinalTerm, out this.FinalSecondaryTerm);
			}
			bool flag = I2Utils.IsPlaying() && this.HasCallback();
			if (!flag && string.IsNullOrEmpty(this.FinalTerm) && string.IsNullOrEmpty(this.FinalSecondaryTerm))
			{
				return;
			}
			Localize.CurrentLocalizeComponent = this;
			Localize.CallBackTerm = this.FinalTerm;
			Localize.CallBackSecondaryTerm = this.FinalSecondaryTerm;
			Localize.MainTranslation = ((string.IsNullOrEmpty(this.FinalTerm) || this.FinalTerm == "-") ? null : LocalizationManager.GetTranslation(this.FinalTerm, false, 0, true, false, null, null, true));
			Localize.SecondaryTranslation = ((string.IsNullOrEmpty(this.FinalSecondaryTerm) || this.FinalSecondaryTerm == "-") ? null : LocalizationManager.GetTranslation(this.FinalSecondaryTerm, false, 0, true, false, null, null, true));
			if (!flag && string.IsNullOrEmpty(this.FinalTerm) && string.IsNullOrEmpty(Localize.SecondaryTranslation))
			{
				return;
			}
			this.LocalizeCallBack.Execute(this);
			this.LocalizeEvent.Invoke();
			if (this.AllowParameters)
			{
				LocalizationManager.ApplyLocalizationParams(ref Localize.MainTranslation, base.gameObject, this.AllowLocalizedParameters);
			}
			if (!this.FindTarget())
			{
				return;
			}
			bool flag2 = LocalizationManager.IsRight2Left && !this.IgnoreRTL;
			if (Localize.MainTranslation != null)
			{
				switch (this.PrimaryTermModifier)
				{
				case Localize.TermModification.ToUpper:
					Localize.MainTranslation = Localize.MainTranslation.ToUpper();
					break;
				case Localize.TermModification.ToLower:
					Localize.MainTranslation = Localize.MainTranslation.ToLower();
					break;
				case Localize.TermModification.ToUpperFirst:
					Localize.MainTranslation = GoogleTranslation.UppercaseFirst(Localize.MainTranslation);
					break;
				case Localize.TermModification.ToTitle:
					Localize.MainTranslation = GoogleTranslation.TitleCase(Localize.MainTranslation);
					break;
				}
				if (!string.IsNullOrEmpty(this.TermPrefix))
				{
					Localize.MainTranslation = (flag2 ? (Localize.MainTranslation + this.TermPrefix) : (this.TermPrefix + Localize.MainTranslation));
				}
				if (!string.IsNullOrEmpty(this.TermSuffix))
				{
					Localize.MainTranslation = (flag2 ? (this.TermSuffix + Localize.MainTranslation) : (Localize.MainTranslation + this.TermSuffix));
				}
				if (this.AddSpacesToJoinedLanguages && LocalizationManager.HasJoinedWords && !string.IsNullOrEmpty(Localize.MainTranslation))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(Localize.MainTranslation[0]);
					int i = 1;
					int length = Localize.MainTranslation.Length;
					while (i < length)
					{
						stringBuilder.Append(' ');
						stringBuilder.Append(Localize.MainTranslation[i]);
						i++;
					}
					Localize.MainTranslation = stringBuilder.ToString();
				}
				if (flag2 && this.mLocalizeTarget.AllowMainTermToBeRTL() && !string.IsNullOrEmpty(Localize.MainTranslation))
				{
					Localize.MainTranslation = LocalizationManager.ApplyRTLfix(Localize.MainTranslation, this.MaxCharactersInRTL, this.IgnoreNumbersInRTL);
				}
			}
			if (Localize.SecondaryTranslation != null)
			{
				switch (this.SecondaryTermModifier)
				{
				case Localize.TermModification.ToUpper:
					Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToUpper();
					break;
				case Localize.TermModification.ToLower:
					Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToLower();
					break;
				case Localize.TermModification.ToUpperFirst:
					Localize.SecondaryTranslation = GoogleTranslation.UppercaseFirst(Localize.SecondaryTranslation);
					break;
				case Localize.TermModification.ToTitle:
					Localize.SecondaryTranslation = GoogleTranslation.TitleCase(Localize.SecondaryTranslation);
					break;
				}
				if (flag2 && this.mLocalizeTarget.AllowSecondTermToBeRTL() && !string.IsNullOrEmpty(Localize.SecondaryTranslation))
				{
					Localize.SecondaryTranslation = LocalizationManager.ApplyRTLfix(Localize.SecondaryTranslation);
				}
			}
			if (LocalizationManager.HighlightLocalizedTargets)
			{
				Localize.MainTranslation = "LOC:" + this.FinalTerm;
			}
			this.mLocalizeTarget.DoLocalize(this, Localize.MainTranslation, Localize.SecondaryTranslation);
			Localize.CurrentLocalizeComponent = null;
		}

		// Token: 0x060012BB RID: 4795 RVA: 0x0007EA24 File Offset: 0x0007CC24
		public bool FindTarget()
		{
			if (this.mLocalizeTarget != null && this.mLocalizeTarget.IsValid(this))
			{
				return true;
			}
			if (this.mLocalizeTarget != null)
			{
				global::UnityEngine.Object.DestroyImmediate(this.mLocalizeTarget);
				this.mLocalizeTarget = null;
				this.mLocalizeTargetName = null;
			}
			if (!string.IsNullOrEmpty(this.mLocalizeTargetName))
			{
				foreach (ILocalizeTargetDescriptor localizeTargetDescriptor in LocalizationManager.mLocalizeTargets)
				{
					if (this.mLocalizeTargetName == localizeTargetDescriptor.GetTargetType().ToString())
					{
						if (localizeTargetDescriptor.CanLocalize(this))
						{
							this.mLocalizeTarget = localizeTargetDescriptor.CreateTarget(this);
						}
						if (this.mLocalizeTarget != null)
						{
							return true;
						}
					}
				}
			}
			foreach (ILocalizeTargetDescriptor localizeTargetDescriptor2 in LocalizationManager.mLocalizeTargets)
			{
				if (localizeTargetDescriptor2.CanLocalize(this))
				{
					this.mLocalizeTarget = localizeTargetDescriptor2.CreateTarget(this);
					this.mLocalizeTargetName = localizeTargetDescriptor2.GetTargetType().ToString();
					if (this.mLocalizeTarget != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060012BC RID: 4796 RVA: 0x0007EB7C File Offset: 0x0007CD7C
		public void GetFinalTerms(out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = string.Empty;
			secondaryTerm = string.Empty;
			if (!this.FindTarget())
			{
				return;
			}
			if (this.mLocalizeTarget != null)
			{
				this.mLocalizeTarget.GetFinalTerms(this, this.mTerm, this.mTermSecondary, out primaryTerm, out secondaryTerm);
				primaryTerm = I2Utils.GetValidTermName(primaryTerm, false);
			}
			if (!string.IsNullOrEmpty(this.mTerm))
			{
				primaryTerm = this.mTerm;
			}
			if (!string.IsNullOrEmpty(this.mTermSecondary))
			{
				secondaryTerm = this.mTermSecondary;
			}
			if (primaryTerm != null)
			{
				primaryTerm = primaryTerm.Trim();
			}
			if (secondaryTerm != null)
			{
				secondaryTerm = secondaryTerm.Trim();
			}
		}

		// Token: 0x060012BD RID: 4797 RVA: 0x0007EC18 File Offset: 0x0007CE18
		public string GetMainTargetsText()
		{
			string text = null;
			string text2 = null;
			if (this.mLocalizeTarget != null)
			{
				this.mLocalizeTarget.GetFinalTerms(this, null, null, out text, out text2);
			}
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return this.mTerm;
		}

		// Token: 0x060012BE RID: 4798 RVA: 0x00014ED6 File Offset: 0x000130D6
		public void SetFinalTerms(string Main, string Secondary, out string primaryTerm, out string secondaryTerm, bool RemoveNonASCII)
		{
			primaryTerm = (RemoveNonASCII ? I2Utils.GetValidTermName(Main, false) : Main);
			secondaryTerm = Secondary;
		}

		// Token: 0x060012BF RID: 4799 RVA: 0x0007EC5C File Offset: 0x0007CE5C
		public void SetTerm(string primary)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				this.mTerm = primary;
				this.FinalTerm = primary;
			}
			this.OnLocalize(true);
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0007EC88 File Offset: 0x0007CE88
		public void SetTerm(string primary, string secondary)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				this.mTerm = primary;
				this.FinalTerm = primary;
			}
			this.mTermSecondary = secondary;
			this.FinalSecondaryTerm = secondary;
			this.OnLocalize(true);
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0007ECC4 File Offset: 0x0007CEC4
		internal T GetSecondaryTranslatedObj<T>(ref string mainTranslation, ref string secondaryTranslation) where T : global::UnityEngine.Object
		{
			string text;
			string text2;
			this.DeserializeTranslation(mainTranslation, out text, out text2);
			T t = default(T);
			if (!string.IsNullOrEmpty(text2))
			{
				t = this.GetObject<T>(text2);
				if (t != null)
				{
					mainTranslation = text;
					secondaryTranslation = text2;
				}
			}
			if (t == null)
			{
				t = this.GetObject<T>(secondaryTranslation);
			}
			return t;
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x0007ED24 File Offset: 0x0007CF24
		public void UpdateAssetDictionary()
		{
			this.TranslatedObjects.RemoveAll((global::UnityEngine.Object x) => x == null);
			this.mAssetDictionary = (from o in this.TranslatedObjects.Distinct<global::UnityEngine.Object>()
				group o by o.name).ToDictionary((IGrouping<string, global::UnityEngine.Object> g) => g.Key, (IGrouping<string, global::UnityEngine.Object> g) => g.First<global::UnityEngine.Object>());
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x0007EDD4 File Offset: 0x0007CFD4
		internal T GetObject<T>(string Translation) where T : global::UnityEngine.Object
		{
			if (string.IsNullOrEmpty(Translation))
			{
				return default(T);
			}
			return this.GetTranslatedObject<T>(Translation);
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x00014EEC File Offset: 0x000130EC
		private T GetTranslatedObject<T>(string Translation) where T : global::UnityEngine.Object
		{
			return this.FindTranslatedObject<T>(Translation);
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0007EDFC File Offset: 0x0007CFFC
		private void DeserializeTranslation(string translation, out string value, out string secondary)
		{
			if (!string.IsNullOrEmpty(translation) && translation.Length > 1 && translation[0] == '[')
			{
				int num = translation.IndexOf(']');
				if (num > 0)
				{
					secondary = translation.Substring(1, num - 1);
					value = translation.Substring(num + 1);
					return;
				}
			}
			value = translation;
			secondary = string.Empty;
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0007EE54 File Offset: 0x0007D054
		public T FindTranslatedObject<T>(string value) where T : global::UnityEngine.Object
		{
			if (string.IsNullOrEmpty(value))
			{
				T t = default(T);
				return t;
			}
			if (this.mAssetDictionary == null || this.mAssetDictionary.Count != this.TranslatedObjects.Count)
			{
				this.UpdateAssetDictionary();
			}
			foreach (KeyValuePair<string, global::UnityEngine.Object> keyValuePair in this.mAssetDictionary)
			{
				if (keyValuePair.Value is T && value.EndsWith(keyValuePair.Key, StringComparison.OrdinalIgnoreCase) && string.Compare(value, keyValuePair.Key, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return (T)((object)keyValuePair.Value);
				}
			}
			T t2 = LocalizationManager.FindAsset(value) as T;
			if (t2)
			{
				return t2;
			}
			return ResourceManager.pInstance.GetAsset<T>(value);
		}

		// Token: 0x060012C7 RID: 4807 RVA: 0x00014EF5 File Offset: 0x000130F5
		public bool HasTranslatedObject(global::UnityEngine.Object Obj)
		{
			return this.TranslatedObjects.Contains(Obj) || ResourceManager.pInstance.HasAsset(Obj);
		}

		// Token: 0x060012C8 RID: 4808 RVA: 0x00014F12 File Offset: 0x00013112
		public void AddTranslatedObject(global::UnityEngine.Object Obj)
		{
			if (this.TranslatedObjects.Contains(Obj))
			{
				return;
			}
			this.TranslatedObjects.Add(Obj);
			this.UpdateAssetDictionary();
		}

		// Token: 0x060012C9 RID: 4809 RVA: 0x00014F35 File Offset: 0x00013135
		public void SetGlobalLanguage(string Language)
		{
			LocalizationManager.CurrentLanguage = Language;
		}

		// Token: 0x04001345 RID: 4933
		public string mTerm = string.Empty;

		// Token: 0x04001346 RID: 4934
		public string mTermSecondary = string.Empty;

		// Token: 0x04001347 RID: 4935
		[NonSerialized]
		public string FinalTerm;

		// Token: 0x04001348 RID: 4936
		[NonSerialized]
		public string FinalSecondaryTerm;

		// Token: 0x04001349 RID: 4937
		public Localize.TermModification PrimaryTermModifier;

		// Token: 0x0400134A RID: 4938
		public Localize.TermModification SecondaryTermModifier;

		// Token: 0x0400134B RID: 4939
		public string TermPrefix;

		// Token: 0x0400134C RID: 4940
		public string TermSuffix;

		// Token: 0x0400134D RID: 4941
		public bool LocalizeOnAwake = true;

		// Token: 0x0400134E RID: 4942
		private string LastLocalizedLanguage;

		// Token: 0x0400134F RID: 4943
		public bool IgnoreRTL;

		// Token: 0x04001350 RID: 4944
		public int MaxCharactersInRTL;

		// Token: 0x04001351 RID: 4945
		public bool IgnoreNumbersInRTL = true;

		// Token: 0x04001352 RID: 4946
		public bool CorrectAlignmentForRTL = true;

		// Token: 0x04001353 RID: 4947
		public bool AddSpacesToJoinedLanguages;

		// Token: 0x04001354 RID: 4948
		public bool AllowLocalizedParameters = true;

		// Token: 0x04001355 RID: 4949
		public bool AllowParameters = true;

		// Token: 0x04001356 RID: 4950
		public List<global::UnityEngine.Object> TranslatedObjects = new List<global::UnityEngine.Object>();

		// Token: 0x04001357 RID: 4951
		[NonSerialized]
		public Dictionary<string, global::UnityEngine.Object> mAssetDictionary = new Dictionary<string, global::UnityEngine.Object>(StringComparer.Ordinal);

		// Token: 0x04001358 RID: 4952
		public UnityEvent LocalizeEvent = new UnityEvent();

		// Token: 0x04001359 RID: 4953
		public static string MainTranslation;

		// Token: 0x0400135A RID: 4954
		public static string SecondaryTranslation;

		// Token: 0x0400135B RID: 4955
		public static string CallBackTerm;

		// Token: 0x0400135C RID: 4956
		public static string CallBackSecondaryTerm;

		// Token: 0x0400135D RID: 4957
		public static Localize CurrentLocalizeComponent;

		// Token: 0x0400135E RID: 4958
		public bool AlwaysForceLocalize;

		// Token: 0x0400135F RID: 4959
		[SerializeField]
		public EventCallback LocalizeCallBack = new EventCallback();

		// Token: 0x04001360 RID: 4960
		public bool mGUI_ShowReferences;

		// Token: 0x04001361 RID: 4961
		public bool mGUI_ShowTems = true;

		// Token: 0x04001362 RID: 4962
		public bool mGUI_ShowCallback;

		// Token: 0x04001363 RID: 4963
		public ILocalizeTarget mLocalizeTarget;

		// Token: 0x04001364 RID: 4964
		public string mLocalizeTargetName;

		// Token: 0x020001B3 RID: 435
		public enum TermModification
		{
			// Token: 0x04001366 RID: 4966
			DontModify,
			// Token: 0x04001367 RID: 4967
			ToUpper,
			// Token: 0x04001368 RID: 4968
			ToLower,
			// Token: 0x04001369 RID: 4969
			ToUpperFirst,
			// Token: 0x0400136A RID: 4970
			ToTitle
		}
	}
}
