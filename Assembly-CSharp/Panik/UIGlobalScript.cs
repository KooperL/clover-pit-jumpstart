using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Panik
{
	public class UIGlobalScript : MonoBehaviour
	{
		// Token: 0x06000DCA RID: 3530 RVA: 0x00056090 File Offset: 0x00054290
		private void SaveIconAndTextUpdate()
		{
			bool flag = PlatformDataMaster.IsSavingOrLoadingOrDeleting();
			bool flag2 = DialogueScript.IsEnabled() || RewardUIScript.IsEnabled();
			if (this.saveIconHolder.activeSelf != flag)
			{
				this.saveIconHolder.SetActive(flag);
				this.saveText.text = "";
				this.saveText.enabled = Master.instance.SHOW_SAVE_MEMO_WARNING;
			}
			if (flag2)
			{
				this.saveIconImage.color = this.savIcColor_Transp;
			}
			else
			{
				this.saveIconImage.color = this.savIcColor_Full;
			}
			Vector2 zero = Vector2.zero;
			if (DialogueScript.IsAskingQuestion())
			{
				zero.y = 32f;
			}
			if (RewardUIScript.IsEnabled())
			{
				zero.x = -80f;
			}
			this.saveIconShifter.anchoredPosition = Vector2.Lerp(this.saveIconShifter.anchoredPosition, zero, Tick.Time * 20f);
			if (!flag)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.saveText.text))
			{
				this.saveText.text = Translation.Get("UI_SAVE_DO_NOT_CLOSE_THE_GAME");
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x00056195 File Offset: 0x00054395
		private void Awake()
		{
			UIGlobalScript.instance = this;
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0005619D File Offset: 0x0005439D
		private void Start()
		{
			this.saveIconImage = this.saveIconHolder.GetComponentInChildren<Image>();
			this.saveIconHolder.SetActive(false);
			this.SaveIconAndTextUpdate();
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x000561C2 File Offset: 0x000543C2
		private void OnDestroy()
		{
			if (UIGlobalScript.instance == this)
			{
				UIGlobalScript.instance = null;
			}
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x000561D7 File Offset: 0x000543D7
		private void Update()
		{
			this.SaveIconAndTextUpdate();
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x000561DF File Offset: 0x000543DF
		private void OnDrawGizmosSelected()
		{
			this.myCanvasScaler.referencePixelsPerUnit = 32f;
		}

		public static UIGlobalScript instance;

		private const float SAV_ICO_LERP_SPEED = 20f;

		public Canvas myCanvas;

		public CanvasScaler myCanvasScaler;

		public RectTransform saveIconShifter;

		public GameObject saveIconHolder;

		public TextMeshProUGUI saveText;

		private Image saveIconImage;

		private Color savIcColor_Full = new Color(1f, 1f, 1f, 1f);

		private Color savIcColor_Transp = new Color(1f, 1f, 1f, 0.25f);
	}
}
