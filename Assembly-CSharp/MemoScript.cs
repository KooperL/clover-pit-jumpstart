using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000D1 RID: 209
public class MemoScript : MonoBehaviour
{
	// Token: 0x06000AF2 RID: 2802 RVA: 0x0000EB2B File Offset: 0x0000CD2B
	public static bool IsEnabled()
	{
		return !(MemoScript.instance == null) && MemoScript.instance.holder.activeSelf;
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x00058190 File Offset: 0x00056390
	public static void SetMessage(MemoScript.Message message, float time)
	{
		if (message != MemoScript.Message.roundsLeft)
		{
			if (message == MemoScript.Message.inputsMove)
			{
				MemoScript.instance.memoText.text = Strings.Sanitize(Strings.SantizationKind.ui, Translation.Get("MEMO_TEXT_MOVEMENT_INPUT"), Strings.SanitizationSubKind.none);
			}
		}
		else
		{
			string text;
			if (GameplayData.RoundsLeftToDeadline() == 1L)
			{
				text = Translation.Get("MEMO_TEXT_ROUND_LEFT");
			}
			else
			{
				text = Translation.Get("MEMO_TEXT_ROUNDS_LEFT");
			}
			text = Strings.Sanitize(Strings.SantizationKind.ui, text, Strings.SanitizationSubKind.none);
			MemoScript.instance.memoText.text = text + " <sprite name=\"SkullSymbolWhite32\">";
		}
		MemoScript.instance.backImage.rectTransform.sizeDelta = new Vector2(MemoScript.instance.memoText.preferredWidth + 40f, MemoScript.instance.memoText.preferredHeight + 20f);
		MemoScript.instance.lifeTimer = time;
		MemoScript.instance.holder.SetActive(true);
	}

	// Token: 0x06000AF4 RID: 2804 RVA: 0x0000EB4B File Offset: 0x0000CD4B
	public static void Close(bool forceClose)
	{
		if (!MemoScript.IsEnabled() && !forceClose)
		{
			return;
		}
		MemoScript.instance.holder.SetActive(false);
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x0000EB68 File Offset: 0x0000CD68
	private void Awake()
	{
		MemoScript.instance = this;
		MemoScript.Close(true);
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x0000EB76 File Offset: 0x0000CD76
	private void OnDestroy()
	{
		if (MemoScript.instance == this)
		{
			MemoScript.instance = null;
		}
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x00058274 File Offset: 0x00056474
	private void Update()
	{
		if (!Tick.IsGameRunning)
		{
			return;
		}
		if (!MemoScript.IsEnabled())
		{
			return;
		}
		if (DialogueScript.IsEnabled())
		{
			this.lifeTimer = Mathf.Min(this.lifeTimer, 0f);
		}
		if (ToyPhoneUIScript.IsEnabled())
		{
			this.lifeTimer = Mathf.Min(this.lifeTimer, 0f);
		}
		this.lifeTimer -= Tick.Time;
		if (this.lifeTimer <= 0f)
		{
			MemoScript.Close(false);
		}
	}

	// Token: 0x04000B40 RID: 2880
	public static MemoScript instance;

	// Token: 0x04000B41 RID: 2881
	public const float TIME_SHORT = 1.5f;

	// Token: 0x04000B42 RID: 2882
	public const float TIME_LONG = 3f;

	// Token: 0x04000B43 RID: 2883
	public const float TIME_LONG_LONG = 5f;

	// Token: 0x04000B44 RID: 2884
	public GameObject holder;

	// Token: 0x04000B45 RID: 2885
	public Image backImage;

	// Token: 0x04000B46 RID: 2886
	public TextMeshProUGUI memoText;

	// Token: 0x04000B47 RID: 2887
	private float lifeTimer;

	// Token: 0x020000D2 RID: 210
	public enum Message
	{
		// Token: 0x04000B49 RID: 2889
		undefined = -1,
		// Token: 0x04000B4A RID: 2890
		roundsLeft,
		// Token: 0x04000B4B RID: 2891
		inputsMove,
		// Token: 0x04000B4C RID: 2892
		count
	}
}
