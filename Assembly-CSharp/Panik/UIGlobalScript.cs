using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Panik
{
	// Token: 0x02000181 RID: 385
	public class UIGlobalScript : MonoBehaviour
	{
		// Token: 0x0600117D RID: 4477 RVA: 0x000754E8 File Offset: 0x000736E8
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

		// Token: 0x0600117E RID: 4478 RVA: 0x0001446E File Offset: 0x0001266E
		private void Awake()
		{
			UIGlobalScript.instance = this;
		}

		// Token: 0x0600117F RID: 4479 RVA: 0x00014476 File Offset: 0x00012676
		private void Start()
		{
			this.saveIconImage = this.saveIconHolder.GetComponentInChildren<Image>();
			this.saveIconHolder.SetActive(false);
			this.SaveIconAndTextUpdate();
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x0001449B File Offset: 0x0001269B
		private void OnDestroy()
		{
			if (UIGlobalScript.instance == this)
			{
				UIGlobalScript.instance = null;
			}
		}

		// Token: 0x06001181 RID: 4481 RVA: 0x000144B0 File Offset: 0x000126B0
		private void Update()
		{
			this.SaveIconAndTextUpdate();
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x000144B8 File Offset: 0x000126B8
		private void OnDrawGizmosSelected()
		{
			this.myCanvasScaler.referencePixelsPerUnit = 32f;
		}

		// Token: 0x0400128F RID: 4751
		public static UIGlobalScript instance;

		// Token: 0x04001290 RID: 4752
		private const float SAV_ICO_LERP_SPEED = 20f;

		// Token: 0x04001291 RID: 4753
		public Canvas myCanvas;

		// Token: 0x04001292 RID: 4754
		public CanvasScaler myCanvasScaler;

		// Token: 0x04001293 RID: 4755
		public RectTransform saveIconShifter;

		// Token: 0x04001294 RID: 4756
		public GameObject saveIconHolder;

		// Token: 0x04001295 RID: 4757
		public TextMeshProUGUI saveText;

		// Token: 0x04001296 RID: 4758
		private Image saveIconImage;

		// Token: 0x04001297 RID: 4759
		private Color savIcColor_Full = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04001298 RID: 4760
		private Color savIcColor_Transp = new Color(1f, 1f, 1f, 0.25f);
	}
}
