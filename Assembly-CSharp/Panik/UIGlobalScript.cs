using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Panik
{
	public class UIGlobalScript : MonoBehaviour
	{
		// Token: 0x06000DE1 RID: 3553 RVA: 0x0005686C File Offset: 0x00054A6C
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

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00056971 File Offset: 0x00054B71
		private void Awake()
		{
			UIGlobalScript.instance = this;
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x00056979 File Offset: 0x00054B79
		private void Start()
		{
			this.saveIconImage = this.saveIconHolder.GetComponentInChildren<Image>();
			this.saveIconHolder.SetActive(false);
			this.SaveIconAndTextUpdate();
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0005699E File Offset: 0x00054B9E
		private void OnDestroy()
		{
			if (UIGlobalScript.instance == this)
			{
				UIGlobalScript.instance = null;
			}
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x000569B3 File Offset: 0x00054BB3
		private void Update()
		{
			this.SaveIconAndTextUpdate();
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x000569BB File Offset: 0x00054BBB
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
