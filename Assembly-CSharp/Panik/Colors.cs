using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	public class Colors : MonoBehaviour
	{
		// Token: 0x06000A9A RID: 2714 RVA: 0x00048500 File Offset: 0x00046700
		public static Material GetMaterial_RainbowPausable()
		{
			return Colors.instance.rainbowMaterial_Pausable;
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0004850C File Offset: 0x0004670C
		public static Material GetMaterial_RainbowUnpausable()
		{
			return Colors.instance.rainbowMaterial_Unpausable;
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x00048518 File Offset: 0x00046718
		public static Material GetMaterial_GoldenSymbolUnpausable()
		{
			return Colors.instance.goldenSymbolMaterial_Unpausable;
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00048524 File Offset: 0x00046724
		public static Color GetColor(string colorName)
		{
			if (Colors.instance.colorDictionary.ContainsKey(colorName.ToLower()))
			{
				return Colors.instance.colorDictionary[colorName].color;
			}
			return Color.white;
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00048558 File Offset: 0x00046758
		public static string GetColorHTML(string colorName)
		{
			if (Colors.instance.colorDictionary.ContainsKey(colorName.ToLower()))
			{
				return Colors.instance.colorDictionary[colorName].colorHTML;
			}
			return "#ffffffff";
		}

		// Token: 0x06000A9F RID: 2719 RVA: 0x0004858C File Offset: 0x0004678C
		public static string GetColorRichTextString(string colorName)
		{
			if (Colors.instance.colorDictionary.ContainsKey(colorName.ToLower()))
			{
				return Colors.instance.colorDictionary[colorName].colorRichTextString;
			}
			return "<color=#ffffffff>";
		}

		// Token: 0x06000AA0 RID: 2720 RVA: 0x000485C0 File Offset: 0x000467C0
		public static Color GetRainbowColor_Pausable()
		{
			return Colors.instance.rainbowColor_Pausable;
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x000485CC File Offset: 0x000467CC
		public static Color GetRainbowColor_Unpausable()
		{
			return Colors.instance.rainbowColor_Unpausable;
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x000485D8 File Offset: 0x000467D8
		public static Color GetGoldenSymbolColor_Unpausable()
		{
			return Colors.instance.goldenSymbolColor_Unpausable;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x000485E4 File Offset: 0x000467E4
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

		// Token: 0x06000AA4 RID: 2724 RVA: 0x00048697 File Offset: 0x00046897
		private void Start()
		{
		}

		// Token: 0x06000AA5 RID: 2725 RVA: 0x0004869C File Offset: 0x0004689C
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
