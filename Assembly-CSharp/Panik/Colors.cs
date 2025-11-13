using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	public class Colors : MonoBehaviour
	{
		// Token: 0x06000AAF RID: 2735 RVA: 0x00048C60 File Offset: 0x00046E60
		public static Material GetMaterial_RainbowPausable()
		{
			return Colors.instance.rainbowMaterial_Pausable;
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00048C6C File Offset: 0x00046E6C
		public static Material GetMaterial_RainbowUnpausable()
		{
			return Colors.instance.rainbowMaterial_Unpausable;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00048C78 File Offset: 0x00046E78
		public static Material GetMaterial_GoldenSymbolUnpausable()
		{
			return Colors.instance.goldenSymbolMaterial_Unpausable;
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00048C84 File Offset: 0x00046E84
		public static Color GetColor(string colorName)
		{
			if (Colors.instance.colorDictionary.ContainsKey(colorName.ToLower()))
			{
				return Colors.instance.colorDictionary[colorName].color;
			}
			return Color.white;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00048CB8 File Offset: 0x00046EB8
		public static string GetColorHTML(string colorName)
		{
			if (Colors.instance.colorDictionary.ContainsKey(colorName.ToLower()))
			{
				return Colors.instance.colorDictionary[colorName].colorHTML;
			}
			return "#ffffffff";
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00048CEC File Offset: 0x00046EEC
		public static string GetColorRichTextString(string colorName)
		{
			if (Colors.instance.colorDictionary.ContainsKey(colorName.ToLower()))
			{
				return Colors.instance.colorDictionary[colorName].colorRichTextString;
			}
			return "<color=#ffffffff>";
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00048D20 File Offset: 0x00046F20
		public static Color GetRainbowColor_Pausable()
		{
			return Colors.instance.rainbowColor_Pausable;
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00048D2C File Offset: 0x00046F2C
		public static Color GetRainbowColor_Unpausable()
		{
			return Colors.instance.rainbowColor_Unpausable;
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x00048D38 File Offset: 0x00046F38
		public static Color GetGoldenSymbolColor_Unpausable()
		{
			return Colors.instance.goldenSymbolColor_Unpausable;
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x00048D44 File Offset: 0x00046F44
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

		// Token: 0x06000AB9 RID: 2745 RVA: 0x00048DF7 File Offset: 0x00046FF7
		private void Start()
		{
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x00048DFC File Offset: 0x00046FFC
		private void Update()
		{
			this.rainbowTimerUnpausable += Tick.Time;
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
			this.rainbowTimerPausable += Tick.Time;
			this.rainbowTimerPausable = Mathf.Repeat(this.rainbowTimerPausable, 1f);
			this.rainbowColor_Pausable = Color.HSVToRGB(this.rainbowTimerPausable, 1f, 1f);
			this.rainbowColor_Pausable.a = 1f;
			this.rainbowMaterial_Pausable.color = this.rainbowColor_Pausable;
		}

		public static Colors instance;

		public Color[] colors;

		public string[] colorNames;

		private Color rainbowColor_Unpausable = Color.HSVToRGB(0f, 1f, 1f);

		private Color rainbowColor_Pausable = Color.HSVToRGB(0f, 1f, 1f);

		private Color goldenSymbolColor_Unpausable = Color.HSVToRGB(0.1f, 1f, 1f);

		private Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

		public Texture2D defaultSpritePalette;

		public Material rainbowMaterial_Pausable;

		public Material rainbowMaterial_Unpausable;

		public Material goldenSymbolMaterial_Unpausable;

		private float rainbowTimerPausable;

		private float rainbowTimerUnpausable;

		private float goldenSymbolTimerUnpausable;

		private Dictionary<string, Colors.ColorCapsule> colorDictionary = new Dictionary<string, Colors.ColorCapsule>();

		private class ColorCapsule
		{
			public Color color;

			public string colorHTML;

			public string colorRichTextString;
		}
	}
}
