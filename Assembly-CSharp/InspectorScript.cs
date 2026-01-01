using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000B9 RID: 185
public class InspectorScript : MonoBehaviour
{
	// Token: 0x060009FC RID: 2556 RVA: 0x0000DE44 File Offset: 0x0000C044
	public static PowerupScript CurrentlyInspectedPowerupGet()
	{
		if (InspectorScript.instance == null)
		{
			return null;
		}
		return InspectorScript.instance.powerupInspected;
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x0000DE5F File Offset: 0x0000C05F
	public static bool IsEnabled()
	{
		return !(InspectorScript.instance == null) && InspectorScript.instance.holder.activeSelf;
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x00051474 File Offset: 0x0004F674
	public static void Open(string title, string description)
	{
		if (InspectorScript.IsEnabled())
		{
			return;
		}
		InspectorScript.instance.holder.SetActive(true);
		MemoScript.Close(false);
		InspectorScript.instance.titleText.text = title;
		InspectorScript.instance.titleText.ForceMeshUpdate(false, false);
		InspectorScript.instance.descriptionText.text = description;
		InspectorScript.instance.descriptionText.ForceMeshUpdate(false, false);
		float num = Mathf.Max(InspectorScript.instance.titleText.renderedWidth, InspectorScript.instance.descriptionText.renderedWidth);
		num = Mathf.Min(num, 460f);
		Vector2 vector = new Vector2(num + 40f, 50f + InspectorScript.instance.descriptionText.renderedHeight + 10f);
		InspectorScript.instance.textBackImage.rectTransform.sizeDelta = vector;
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x0000DE7F File Offset: 0x0000C07F
	public static void Open_AsPowerup(PowerupScript powerup)
	{
		InspectorScript.instance.powerupInspected = powerup;
		InspectorScript.Open(powerup.NameGet(true, true), powerup.DescriptionGet(true, false, true, 5f));
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x0000DEA7 File Offset: 0x0000C0A7
	public static void Close()
	{
		if (!InspectorScript.IsEnabled())
		{
			return;
		}
		InspectorScript.instance.powerupInspected = null;
		InspectorScript.instance.holder.SetActive(false);
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x00051550 File Offset: 0x0004F750
	private void StoreAutoEnableCheck()
	{
		bool flag = PromptGuideScript.IsEnabled();
		PromptGuideScript.GuideType guideType = PromptGuideScript.GetGuideType();
		bool flag2 = guideType == PromptGuideScript.GuideType.store_box0 || guideType == PromptGuideScript.GuideType.store_box1 || guideType == PromptGuideScript.GuideType.store_box2 || guideType == PromptGuideScript.GuideType.store_speedyBox0 || guideType == PromptGuideScript.GuideType.store_speedyBox1 || guideType == PromptGuideScript.GuideType.store_speedyBox2;
		if (this.storeEnabledAutomatically)
		{
			if (flag && flag2)
			{
				switch (guideType)
				{
				case PromptGuideScript.GuideType.store_box0:
					if (!(StoreCapsuleScript.storePowerups[0] == null))
					{
						if (this.oldStorePowerup == StoreCapsuleScript.storePowerups[0].identifier)
						{
							goto IL_017A;
						}
					}
					break;
				case PromptGuideScript.GuideType.store_box1:
					if (!(StoreCapsuleScript.storePowerups[1] == null))
					{
						if (this.oldStorePowerup == StoreCapsuleScript.storePowerups[1].identifier)
						{
							goto IL_017A;
						}
					}
					break;
				case PromptGuideScript.GuideType.store_box2:
					if (!(StoreCapsuleScript.storePowerups[2] == null))
					{
						if (this.oldStorePowerup == StoreCapsuleScript.storePowerups[2].identifier)
						{
							goto IL_017A;
						}
					}
					break;
				default:
					switch (guideType)
					{
					case PromptGuideScript.GuideType.store_speedyBox0:
						if (!(StoreCapsuleScript.storePowerups[3] == null))
						{
							if (this.oldStorePowerup == StoreCapsuleScript.storePowerups[3].identifier)
							{
								goto IL_017A;
							}
						}
						break;
					case PromptGuideScript.GuideType.store_speedyBox1:
						if (!(StoreCapsuleScript.storePowerups[4] == null))
						{
							if (this.oldStorePowerup == StoreCapsuleScript.storePowerups[4].identifier)
							{
								goto IL_017A;
							}
						}
						break;
					case PromptGuideScript.GuideType.store_speedyBox2:
						if (!(StoreCapsuleScript.storePowerups[5] == null) && this.oldStorePowerup == StoreCapsuleScript.storePowerups[5].identifier)
						{
							goto IL_017A;
						}
						break;
					default:
						goto IL_017A;
					}
					break;
				}
			}
			this.storeEnabledAutomatically = false;
			InspectorScript.Close();
			return;
		}
		IL_017A:
		if (!this.storeEnabledAutomatically && flag && flag2)
		{
			switch (guideType)
			{
			case PromptGuideScript.GuideType.store_box0:
				if (!(StoreCapsuleScript.storePowerups[0] != null))
				{
					return;
				}
				InspectorScript.Open_AsPowerup(StoreCapsuleScript.storePowerups[0]);
				this.oldStorePowerup = StoreCapsuleScript.storePowerups[0].identifier;
				break;
			case PromptGuideScript.GuideType.store_box1:
				if (!(StoreCapsuleScript.storePowerups[1] != null))
				{
					return;
				}
				InspectorScript.Open_AsPowerup(StoreCapsuleScript.storePowerups[1]);
				this.oldStorePowerup = StoreCapsuleScript.storePowerups[1].identifier;
				break;
			case PromptGuideScript.GuideType.store_box2:
				if (!(StoreCapsuleScript.storePowerups[2] != null))
				{
					return;
				}
				InspectorScript.Open_AsPowerup(StoreCapsuleScript.storePowerups[2]);
				this.oldStorePowerup = StoreCapsuleScript.storePowerups[2].identifier;
				break;
			default:
				switch (guideType)
				{
				case PromptGuideScript.GuideType.store_speedyBox0:
					if (!(StoreCapsuleScript.storePowerups[3] != null))
					{
						return;
					}
					InspectorScript.Open_AsPowerup(StoreCapsuleScript.storePowerups[3]);
					this.oldStorePowerup = StoreCapsuleScript.storePowerups[3].identifier;
					break;
				case PromptGuideScript.GuideType.store_speedyBox1:
					if (!(StoreCapsuleScript.storePowerups[4] != null))
					{
						return;
					}
					InspectorScript.Open_AsPowerup(StoreCapsuleScript.storePowerups[4]);
					this.oldStorePowerup = StoreCapsuleScript.storePowerups[4].identifier;
					break;
				case PromptGuideScript.GuideType.store_speedyBox2:
					if (!(StoreCapsuleScript.storePowerups[5] != null))
					{
						return;
					}
					InspectorScript.Open_AsPowerup(StoreCapsuleScript.storePowerups[5]);
					this.oldStorePowerup = StoreCapsuleScript.storePowerups[5].identifier;
					break;
				default:
					return;
				}
				break;
			}
			this.storeEnabledAutomatically = true;
			return;
		}
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x0000DECC File Offset: 0x0000C0CC
	private void Awake()
	{
		InspectorScript.instance = this;
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x0000DED4 File Offset: 0x0000C0D4
	private void Start()
	{
		InspectorScript.Close();
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x0000DEDB File Offset: 0x0000C0DB
	private void OnDestroy()
	{
		if (InspectorScript.instance == this)
		{
			InspectorScript.instance = null;
		}
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x00051854 File Offset: 0x0004FA54
	private void Update()
	{
		this.StoreAutoEnableCheck();
		if (!InspectorScript.IsEnabled())
		{
			return;
		}
		Vector2 zero = new Vector2(global::UnityEngine.Random.Range(-1f, 1f), global::UnityEngine.Random.Range(-1f, 1f));
		if (Data.settings.dyslexicFontEnabled)
		{
			zero = Vector2.zero;
		}
		this.textBackImage.rectTransform.anchoredPosition = zero;
	}

	// Token: 0x04000A2E RID: 2606
	public static InspectorScript instance;

	// Token: 0x04000A2F RID: 2607
	public GameObject holder;

	// Token: 0x04000A30 RID: 2608
	public Image textBackImage;

	// Token: 0x04000A31 RID: 2609
	public TextMeshProUGUI titleText;

	// Token: 0x04000A32 RID: 2610
	public TextMeshProUGUI descriptionText;

	// Token: 0x04000A33 RID: 2611
	private PowerupScript powerupInspected;

	// Token: 0x04000A34 RID: 2612
	private bool storeEnabledAutomatically;

	// Token: 0x04000A35 RID: 2613
	private PowerupScript.Identifier oldStorePowerup = PowerupScript.Identifier.undefined;
}
