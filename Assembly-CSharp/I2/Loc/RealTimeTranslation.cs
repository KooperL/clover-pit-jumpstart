using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	public class RealTimeTranslation : MonoBehaviour
	{
		// Token: 0x06000E08 RID: 3592 RVA: 0x00056DF4 File Offset: 0x00054FF4
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

		// Token: 0x06000E09 RID: 3593 RVA: 0x00056F37 File Offset: 0x00055137
		public void StartTranslating(string fromCode, string toCode)
		{
			this.IsTranslating = true;
			GoogleTranslation.Translate(this.OriginalText, fromCode, toCode, new GoogleTranslation.fnOnTranslated(this.OnTranslationReady));
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00056F59 File Offset: 0x00055159
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

		// Token: 0x06000E0B RID: 3595 RVA: 0x00056F74 File Offset: 0x00055174
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

		// Token: 0x06000E0C RID: 3596 RVA: 0x00057028 File Offset: 0x00055228
		public void ExampleMultiTranslations_Async()
		{
			this.IsTranslating = true;
			Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>();
			GoogleTranslation.AddQuery("This is an example", "en", "es", dictionary);
			GoogleTranslation.AddQuery("This is an example", "auto", "zh", dictionary);
			GoogleTranslation.AddQuery("Hola", "es", "en", dictionary);
			GoogleTranslation.Translate(dictionary, new GoogleTranslation.fnOnTranslationReady(this.OnMultitranslationReady), true);
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00057094 File Offset: 0x00055294
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

		// Token: 0x06000E0E RID: 3598 RVA: 0x00057157 File Offset: 0x00055357
		public bool IsWaitingForTranslation()
		{
			return this.IsTranslating;
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0005715F File Offset: 0x0005535F
		public string GetTranslatedText()
		{
			return this.TranslatedText;
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00057167 File Offset: 0x00055367
		public void SetOriginalText(string text)
		{
			this.OriginalText = text;
		}

		private string OriginalText = "This is an example showing how to use the google translator to translate chat messages within the game.\nIt also supports multiline translations.";

		private string TranslatedText = string.Empty;

		private bool IsTranslating;
	}
}
