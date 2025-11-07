using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoScript : MonoBehaviour
{
	// Token: 0x0600096C RID: 2412 RVA: 0x0003E30F File Offset: 0x0003C50F
	public static bool IsEnabled()
	{
		return !(MemoScript.instance == null) && MemoScript.instance.holder.activeSelf;
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0003E330 File Offset: 0x0003C530
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

	// Token: 0x0600096E RID: 2414 RVA: 0x0003E414 File Offset: 0x0003C614
	public static void Close(bool forceClose)
	{
		if (!MemoScript.IsEnabled() && !forceClose)
		{
			return;
		}
		MemoScript.instance.holder.SetActive(false);
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x0003E431 File Offset: 0x0003C631
	private void Awake()
	{
		MemoScript.instance = this;
		MemoScript.Close(true);
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x0003E43F File Offset: 0x0003C63F
	private void OnDestroy()
	{
		if (MemoScript.instance == this)
		{
			MemoScript.instance = null;
		}
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x0003E454 File Offset: 0x0003C654
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

	public static MemoScript instance;

	public const float TIME_SHORT = 1.5f;

	public const float TIME_LONG = 3f;

	public const float TIME_LONG_LONG = 5f;

	public GameObject holder;

	public Image backImage;

	public TextMeshProUGUI memoText;

	private float lifeTimer;

	public enum Message
	{
		undefined = -1,
		roundsLeft,
		inputsMove,
		count
	}
}
