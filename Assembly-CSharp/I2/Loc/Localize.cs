using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/I2 Localize")]
	public class Localize : MonoBehaviour
	{
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x0005F914 File Offset: 0x0005DB14
		// (set) Token: 0x06000EDA RID: 3802 RVA: 0x0005F91C File Offset: 0x0005DB1C
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

		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x0005F925 File Offset: 0x0005DB25
		// (set) Token: 0x06000EDC RID: 3804 RVA: 0x0005F92D File Offset: 0x0005DB2D
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

		// Token: 0x06000EDD RID: 3805 RVA: 0x0005F937 File Offset: 0x0005DB37
		private void Awake()
		{
			this.UpdateAssetDictionary();
			this.FindTarget();
			if (this.LocalizeOnAwake)
			{
				this.OnLocalize(false);
			}
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0005F955 File Offset: 0x0005DB55
		private void OnEnable()
		{
			this.OnLocalize(false);
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0005F95E File Offset: 0x0005DB5E
		public bool HasCallback()
		{
			return this.LocalizeCallBack.HasCallback() || this.LocalizeEvent.GetPersistentEventCount() > 0;
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0005F980 File Offset: 0x0005DB80
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

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0005FDA4 File Offset: 0x0005DFA4
		public bool FindTarget()
		{
			if (this.mLocalizeTarget != null && this.mLocalizeTarget.IsValid(this))
			{
				return true;
			}
			if (this.mLocalizeTarget != null)
			{
				Object.DestroyImmediate(this.mLocalizeTarget);
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

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0005FEFC File Offset: 0x0005E0FC
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

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0005FF98 File Offset: 0x0005E198
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

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0005FFD9 File Offset: 0x0005E1D9
		public void SetFinalTerms(string Main, string Secondary, out string primaryTerm, out string secondaryTerm, bool RemoveNonASCII)
		{
			primaryTerm = (RemoveNonASCII ? I2Utils.GetValidTermName(Main, false) : Main);
			secondaryTerm = Secondary;
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0005FFF0 File Offset: 0x0005E1F0
		public void SetTerm(string primary)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				this.mTerm = primary;
				this.FinalTerm = primary;
			}
			this.OnLocalize(true);
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0006001C File Offset: 0x0005E21C
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

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00060058 File Offset: 0x0005E258
		internal T GetSecondaryTranslatedObj<T>(ref string mainTranslation, ref string secondaryTranslation) where T : Object
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

		// Token: 0x06000EE8 RID: 3816 RVA: 0x000600B8 File Offset: 0x0005E2B8
		public void UpdateAssetDictionary()
		{
			this.TranslatedObjects.RemoveAll((Object x) => x == null);
			this.mAssetDictionary = (from o in this.TranslatedObjects.Distinct<Object>()
				group o by o.name).ToDictionary((IGrouping<string, Object> g) => g.Key, (IGrouping<string, Object> g) => g.First<Object>());
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x00060168 File Offset: 0x0005E368
		internal T GetObject<T>(string Translation) where T : Object
		{
			if (string.IsNullOrEmpty(Translation))
			{
				return default(T);
			}
			return this.GetTranslatedObject<T>(Translation);
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0006018E File Offset: 0x0005E38E
		private T GetTranslatedObject<T>(string Translation) where T : Object
		{
			return this.FindTranslatedObject<T>(Translation);
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00060198 File Offset: 0x0005E398
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

		// Token: 0x06000EEC RID: 3820 RVA: 0x000601F0 File Offset: 0x0005E3F0
		public T FindTranslatedObject<T>(string value) where T : Object
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
			foreach (KeyValuePair<string, Object> keyValuePair in this.mAssetDictionary)
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

		// Token: 0x06000EED RID: 3821 RVA: 0x000602E0 File Offset: 0x0005E4E0
		public bool HasTranslatedObject(Object Obj)
		{
			return this.TranslatedObjects.Contains(Obj) || ResourceManager.pInstance.HasAsset(Obj);
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x000602FD File Offset: 0x0005E4FD
		public void AddTranslatedObject(Object Obj)
		{
			if (this.TranslatedObjects.Contains(Obj))
			{
				return;
			}
			this.TranslatedObjects.Add(Obj);
			this.UpdateAssetDictionary();
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x00060320 File Offset: 0x0005E520
		public void SetGlobalLanguage(string Language)
		{
			LocalizationManager.CurrentLanguage = Language;
		}

		public string mTerm = string.Empty;

		public string mTermSecondary = string.Empty;

		[NonSerialized]
		public string FinalTerm;

		[NonSerialized]
		public string FinalSecondaryTerm;

		public Localize.TermModification PrimaryTermModifier;

		public Localize.TermModification SecondaryTermModifier;

		public string TermPrefix;

		public string TermSuffix;

		public bool LocalizeOnAwake = true;

		private string LastLocalizedLanguage;

		public bool IgnoreRTL;

		public int MaxCharactersInRTL;

		public bool IgnoreNumbersInRTL = true;

		public bool CorrectAlignmentForRTL = true;

		public bool AddSpacesToJoinedLanguages;

		public bool AllowLocalizedParameters = true;

		public bool AllowParameters = true;

		public List<Object> TranslatedObjects = new List<Object>();

		[NonSerialized]
		public Dictionary<string, Object> mAssetDictionary = new Dictionary<string, Object>(StringComparer.Ordinal);

		public UnityEvent LocalizeEvent = new UnityEvent();

		public static string MainTranslation;

		public static string SecondaryTranslation;

		public static string CallBackTerm;

		public static string CallBackSecondaryTerm;

		public static Localize CurrentLocalizeComponent;

		public bool AlwaysForceLocalize;

		[SerializeField]
		public EventCallback LocalizeCallBack = new EventCallback();

		public bool mGUI_ShowReferences;

		public bool mGUI_ShowTems = true;

		public bool mGUI_ShowCallback;

		public ILocalizeTarget mLocalizeTarget;

		public string mLocalizeTargetName;

		public enum TermModification
		{
			DontModify,
			ToUpper,
			ToLower,
			ToUpperFirst,
			ToTitle
		}
	}
}
