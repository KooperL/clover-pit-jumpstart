using System;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectorScript : MonoBehaviour
{
	// Token: 0x060008C5 RID: 2245 RVA: 0x00039E95 File Offset: 0x00038095
	public static PowerupScript CurrentlyInspectedPowerupGet()
	{
		if (InspectorScript.instance == null)
		{
			return null;
		}
		return InspectorScript.instance.powerupInspected;
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x00039EB0 File Offset: 0x000380B0
	public static bool IsEnabled()
	{
		return !(InspectorScript.instance == null) && InspectorScript.instance.holder.activeSelf;
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x00039ED0 File Offset: 0x000380D0
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

	// Token: 0x060008C8 RID: 2248 RVA: 0x00039FAB File Offset: 0x000381AB
	public static void Open_AsPowerup(PowerupScript powerup)
	{
		InspectorScript.instance.powerupInspected = powerup;
		InspectorScript.Open(powerup.NameGet(true, true), powerup.DescriptionGet(true, false, true, 5f));
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x00039FD3 File Offset: 0x000381D3
	public static void Close()
	{
		if (!InspectorScript.IsEnabled())
		{
			return;
		}
		InspectorScript.instance.powerupInspected = null;
		InspectorScript.instance.holder.SetActive(false);
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x00039FF8 File Offset: 0x000381F8
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

	// Token: 0x060008CB RID: 2251 RVA: 0x0003A2FC File Offset: 0x000384FC
	private void Awake()
	{
		InspectorScript.instance = this;
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0003A304 File Offset: 0x00038504
	private void Start()
	{
		InspectorScript.Close();
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x0003A30B File Offset: 0x0003850B
	private void OnDestroy()
	{
		if (InspectorScript.instance == this)
		{
			InspectorScript.instance = null;
		}
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x0003A320 File Offset: 0x00038520
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

	public static InspectorScript instance;

	public GameObject holder;

	public Image textBackImage;

	public TextMeshProUGUI titleText;

	public TextMeshProUGUI descriptionText;

	private PowerupScript powerupInspected;

	private bool storeEnabledAutomatically;

	private PowerupScript.Identifier oldStorePowerup = PowerupScript.Identifier.undefined;
}
