using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	public class RealTimeTranslation : MonoBehaviour
	{
		// Token: 0x06000DF1 RID: 3569 RVA: 0x00056618 File Offset: 0x00054818
		public void OnGUI()
		{
			GUILayout.Label("Translate:", Array.Empty<GUILayoutOption>());
			this.OriginalText = GUILayout.TextArea(this.OriginalText, new GUILayoutOption[] { GUILayout.Width((float)Screen.width) });
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("English -> Español", new GUILayoutOption[] { GUILayout.Height(100f) }))
			{
				this.StartTranslating("en", "es");
			}
			if (GUILayout.Button("Español -> English", new GUILayoutOption[] { GUILayout.Height(100f) }))
			{
				this.StartTranslating("es", "en");
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.TextArea("Multiple Translation with 1 call:\n'This is an example' -> en,zh\n'Hola' -> en", Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Multi Translate", new GUILayoutOption[] { GUILayout.ExpandHeight(true) }))
			{
				this.ExampleMultiTranslations_Async();
			}
			GUILayout.EndHorizontal();
			GUILayout.TextArea(this.TranslatedText, new GUILayoutOption[] { GUILayout.Width((float)Screen.width) });
			GUILayout.Space(10f);
			if (this.IsTranslating)
			{
				GUILayout.Label("Contacting Google....", Array.Empty<GUILayoutOption>());
			}
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0005675B File Offset: 0x0005495B
		public void StartTranslating(string fromCode, string toCode)
		{
			this.IsTranslating = true;
			GoogleTranslation.Translate(this.OriginalText, fromCode, toCode, new GoogleTranslation.fnOnTranslated(this.OnTranslationReady));
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0005677D File Offset: 0x0005497D
		private void OnTranslationReady(string Translation, string errorMsg)
		{
			this.IsTranslating = false;
			if (errorMsg != null)
			{
				Debug.LogError(errorMsg);
				return;
			}
			this.TranslatedText = Translation;
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00056798 File Offset: 0x00054998
		public void ExampleMultiTranslations_Blocking()
		{
			Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>();
			GoogleTranslation.AddQuery("This is an example", "en", "es", dictionary);
			GoogleTranslation.AddQuery("This is an example", "auto", "zh", dictionary);
			GoogleTranslation.AddQuery("Hola", "es", "en", dictionary);
			if (!GoogleTranslation.ForceTranslate(dictionary, true))
			{
				return;
			}
			Debug.Log(GoogleTranslation.GetQueryResult("This is an example", "en", dictionary));
			Debug.Log(GoogleTranslation.GetQueryResult("This is an example", "zh", dictionary));
			Debug.Log(GoogleTranslation.GetQueryResult("This is an example", "", dictionary));
			Debug.Log(dictionary["Hola"].Results[0]);
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0005684C File Offset: 0x00054A4C
		public void ExampleMultiTranslations_Async()
		{
			this.IsTranslating = true;
			Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>();
			GoogleTranslation.AddQuery("This is an example", "en", "es", dictionary);
			GoogleTranslation.AddQuery("This is an example", "auto", "zh", dictionary);
			GoogleTranslation.AddQuery("Hola", "es", "en", dictionary);
			GoogleTranslation.Translate(dictionary, new GoogleTranslation.fnOnTranslationReady(this.OnMultitranslationReady), true);
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x000568B8 File Offset: 0x00054AB8
		private void OnMultitranslationReady(Dictionary<string, TranslationQuery> dict, string errorMsg)
		{
			if (!string.IsNullOrEmpty(errorMsg))
			{
				Debug.LogError(errorMsg);
				return;
			}
			this.IsTranslating = false;
			this.TranslatedText = "";
			this.TranslatedText = this.TranslatedText + GoogleTranslation.GetQueryResult("This is an example", "es", dict) + "\n";
			this.TranslatedText = this.TranslatedText + GoogleTranslation.GetQueryResult("This is an example", "zh", dict) + "\n";
			this.TranslatedText = this.TranslatedText + GoogleTranslation.GetQueryResult("This is an example", "", dict) + "\n";
			this.TranslatedText += dict["Hola"].Results[0];
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0005697B File Offset: 0x00054B7B
		public bool IsWaitingForTranslation()
		{
			return this.IsTranslating;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00056983 File Offset: 0x00054B83
		public string GetTranslatedText()
		{
			return this.TranslatedText;
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0005698B File Offset: 0x00054B8B
		public void SetOriginalText(string text)
		{
			this.OriginalText = text;
		}

		private string OriginalText = "This is an example showing how to use the google translator to translate chat messages within the game.\nIt also supports multiline translations.";

		private string TranslatedText = string.Empty;

		private bool IsTranslating;
	}
}
