using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	// Token: 0x02000108 RID: 264
	public class Colors : MonoBehaviour
	{
		// Token: 0x06000C88 RID: 3208 RVA: 0x0001041F File Offset: 0x0000E61F
		public static Material GetMaterial_RainbowPausable()
		{
			return Colors.instance.rainbowMaterial_Pausable;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0001042B File Offset: 0x0000E62B
		public static Material GetMaterial_RainbowUnpausable()
		{
			return Colors.instance.rainbowMaterial_Unpausable;
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x00010437 File Offset: 0x0000E637
		public static Material GetMaterial_GoldenSymbolUnpausable()
		{
			return Colors.instance.goldenSymbolMaterial_Unpausable;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00010443 File Offset: 0x0000E643
		public static Color GetColor(string colorName)
		{
			if (Colors.instance.colorDictionary.ContainsKey(colorName.ToLower()))
			{
				return Colors.instance.colorDictionary[colorName].color;
			}
			return Color.white;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00010477 File Offset: 0x0000E677
		public static string GetColorHTML(string colorName)
		{
			if (Colors.instance.colorDictionary.ContainsKey(colorName.ToLower()))
			{
				return Colors.instance.colorDictionary[colorName].colorHTML;
			}
			return "#ffffffff";
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x000104AB File Offset: 0x0000E6AB
		public static string GetColorRichTextString(string colorName)
		{
			if (Colors.instance.colorDictionary.ContainsKey(colorName.ToLower()))
			{
				return Colors.instance.colorDictionary[colorName].colorRichTextString;
			}
			return "<color=#ffffffff>";
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x000104DF File Offset: 0x0000E6DF
		public static Color GetRainbowColor_Pausable()
		{
			return Colors.instance.rainbowColor_Pausable;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x000104EB File Offset: 0x0000E6EB
		public static Color GetRainbowColor_Unpausable()
		{
			return Colors.instance.rainbowColor_Unpausable;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x000104F7 File Offset: 0x0000E6F7
		public static Color GetGoldenSymbolColor_Unpausable()
		{
			return Colors.instance.goldenSymbolColor_Unpausable;
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x00062FAC File Offset: 0x000611AC
		private void Awake()
		{
			if (Colors.instance != null)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			Colors.instance = this;
			for (int i = 0; i < this.colors.Length; i++)
			{
				string text = ColorUtility.ToHtmlStringRGBA(this.colors[i]);
				this.colorDictionary.Add(this.colorNames[i].ToLower(), new Colors.ColorCapsule
				{
					color = this.colors[i],
					colorHTML = ColorUtility.ToHtmlStringRGBA(this.colors[i]),
					colorRichTextString = "<color=#" + text + ">"
				});
			}
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0000774E File Offset: 0x0000594E
		private void Start()
		{
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x00063060 File Offset: 0x00061260
		private void Update()
		{
			float num = 1f;
			if (Data.settings.flashingLightsReducedEnabled)
			{
				num = 0.25f;
			}
			this.rainbowTimerUnpausable += Tick.Time * num;
			this.rainbowTimerUnpausable = Mathf.Repeat(this.rainbowTimerUnpausable, 1f);
			this.rainbowColor_Unpausable = Color.HSVToRGB(this.rainbowTimerUnpausable, 1f, 1f);
			this.rainbowColor_Unpausable.a = 1f;
			this.rainbowMaterial_Unpausable.color = this.rainbowColor_Unpausable;
			this.goldenSymbolTimerUnpausable += Tick.Time;
			this.goldenSymbolColor_Unpausable = ((Util.AngleSin(this.goldenSymbolTimerUnpausable * 1440f) > 0f) ? this.C_ORANGE : Color.yellow);
			this.goldenSymbolMaterial_Unpausable.color = this.goldenSymbolColor_Unpausable;
			if (!Tick.IsGameRunning)
			{
				return;
			}
			this.rainbowTimerPausable += Tick.Time * num;
			this.rainbowTimerPausable = Mathf.Repeat(this.rainbowTimerPausable, 1f);
			this.rainbowColor_Pausable = Color.HSVToRGB(this.rainbowTimerPausable, 1f, 1f);
			this.rainbowColor_Pausable.a = 1f;
			this.rainbowMaterial_Pausable.color = this.rainbowColor_Pausable;
		}

		// Token: 0x04000D5C RID: 3420
		public static Colors instance;

		// Token: 0x04000D5D RID: 3421
		public Color[] colors;

		// Token: 0x04000D5E RID: 3422
		public string[] colorNames;

		// Token: 0x04000D5F RID: 3423
		private Color rainbowColor_Unpausable = Color.HSVToRGB(0f, 1f, 1f);

		// Token: 0x04000D60 RID: 3424
		private Color rainbowColor_Pausable = Color.HSVToRGB(0f, 1f, 1f);

		// Token: 0x04000D61 RID: 3425
		private Color goldenSymbolColor_Unpausable = Color.HSVToRGB(0.1f, 1f, 1f);

		// Token: 0x04000D62 RID: 3426
		private Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

		// Token: 0x04000D63 RID: 3427
		public Texture2D defaultSpritePalette;

		// Token: 0x04000D64 RID: 3428
		public Material rainbowMaterial_Pausable;

		// Token: 0x04000D65 RID: 3429
		public Material rainbowMaterial_Unpausable;

		// Token: 0x04000D66 RID: 3430
		public Material goldenSymbolMaterial_Unpausable;

		// Token: 0x04000D67 RID: 3431
		private float rainbowTimerPausable;

		// Token: 0x04000D68 RID: 3432
		private float rainbowTimerUnpausable;

		// Token: 0x04000D69 RID: 3433
		private float goldenSymbolTimerUnpausable;

		// Token: 0x04000D6A RID: 3434
		private Dictionary<string, Colors.ColorCapsule> colorDictionary = new Dictionary<string, Colors.ColorCapsule>();

		// Token: 0x02000109 RID: 265
		private class ColorCapsule
		{
			// Token: 0x04000D6B RID: 3435
			public Color color;

			// Token: 0x04000D6C RID: 3436
			public string colorHTML;

			// Token: 0x04000D6D RID: 3437
			public string colorRichTextString;
		}
	}
}
