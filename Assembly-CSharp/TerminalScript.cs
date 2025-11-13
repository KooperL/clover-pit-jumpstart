using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TerminalScript : MonoBehaviour
{
	// Token: 0x0600081A RID: 2074 RVA: 0x00033DD4 File Offset: 0x00031FD4
	public static void SetState(TerminalScript.State state)
	{
		if (TerminalScript.instance == null)
		{
			return;
		}
		if (TerminalScript.instance.state == state)
		{
			return;
		}
		switch (TerminalScript.instance.state)
		{
		case TerminalScript.State.turnedOff_nothing:
			TerminalScript.instance.offNothingHolder.SetActive(false);
			break;
		case TerminalScript.State.turnedOff_Email:
			TerminalScript.instance.mailHolder.SetActive(false);
			break;
		case TerminalScript.State.turnedOff_OfferPreview:
			TerminalScript.instance.offerPreviewHolder.SetActive(false);
			break;
		case TerminalScript.State.navigation:
			TerminalScript.instance.PowerupMeshes_RestoreAll();
			TerminalScript.instance.navigationHolder.SetActive(false);
			break;
		}
		switch (state)
		{
		case TerminalScript.State.turnedOff_Request:
			TerminalScript.instance.PowerupMeshes_RestoreAll();
			TerminalScript.instance.NavigationReset();
			Sound.Stop("SoundTerminalFanLoop", true);
			break;
		case TerminalScript.State.turnedOff_nothing:
			TerminalScript.instance.offNothingHolder.SetActive(true);
			break;
		case TerminalScript.State.turnedOff_Email:
			TerminalScript.instance.youGotMailTimer = 0.5f;
			TerminalScript.instance.mailHolder.SetActive(true);
			break;
		case TerminalScript.State.turnedOff_OfferPreview:
			TerminalScript.instance.offerPreviewHolder.SetActive(true);
			break;
		case TerminalScript.State.turnedOn_Request:
			TerminalScript.instance.NavigationReset();
			break;
		case TerminalScript.State.navigation:
		{
			TerminalScript.instance.navigationHolder.SetActive(true);
			TerminalScript.instance.PowerupMeshes_RestoreAll();
			if (Master.IsDemo)
			{
				TerminalScript.instance.collectionTitleText.text = Translation.Get("TERMINAL_COLLECTION_TITLE_DEMO");
			}
			else
			{
				TerminalScript.instance.collectionTitleText.text = Translation.Get("TERMINAL_COLLECTION_TITLE");
			}
			int count = PowerupScript.all.Count;
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				if (PowerupScript.IsUnlocked(PowerupScript.all[i].identifier))
				{
					num++;
				}
			}
			TextMeshProUGUI textMeshProUGUI = TerminalScript.instance.collectionTitleText;
			textMeshProUGUI.text = string.Concat(new string[]
			{
				textMeshProUGUI.text,
				" (",
				num.ToString(),
				"/",
				count.ToString(),
				")"
			});
			TerminalScript.instance.inspector_BuyAnnoyingText.text = Translation.Get("TEXT_BUY_ANNOYING_PROMPT");
			TerminalScript.instance.inspector_BannedText.text = Translation.Get("TEXT_BANNED_PROMPT");
			break;
		}
		case TerminalScript.State.notification:
			if (Data.game.TerminalNotification_HasAny())
			{
				TerminalScript.instance.notificationsCoroutine = TerminalScript.instance.StartCoroutine(TerminalScript.instance.NotificationsCoroutine());
			}
			break;
		case TerminalScript.State.offerNotification:
			if (TerminalScript.instance.offeredPowerup != null)
			{
				TerminalScript.instance.offerNotificationCoroutine = TerminalScript.instance.StartCoroutine(TerminalScript.instance.OfferNotificationCoroutine());
			}
			break;
		}
		TerminalScript.instance.state = state;
	}

	// Token: 0x0600081B RID: 2075 RVA: 0x000340A4 File Offset: 0x000322A4
	public static bool IsLoggedIn()
	{
		if (TerminalScript.instance == null)
		{
			return false;
		}
		switch (TerminalScript.instance.state)
		{
		case TerminalScript.State.turnedOff_Request:
		case TerminalScript.State.turnedOff_Email:
		case TerminalScript.State.turnedOff_OfferPreview:
			return false;
		case TerminalScript.State.turnedOn_Request:
		case TerminalScript.State.navigation:
		case TerminalScript.State.notification:
		case TerminalScript.State.offerNotification:
			return true;
		}
		return false;
	}

	// Token: 0x0600081C RID: 2076 RVA: 0x000340F8 File Offset: 0x000322F8
	public void PowerupMesh_Steal(PowerupScript powerupScript, Transform targetParent, bool normalizeScale, float scaleMult)
	{
		powerupScript.MeshSteal(targetParent, normalizeScale, scaleMult);
		powerupScript.MaterialRefresh();
		this.powerupStealedMeshes.Add(powerupScript);
	}

	// Token: 0x0600081D RID: 2077 RVA: 0x00034116 File Offset: 0x00032316
	public void PowerupMesh_Restore(PowerupScript powerupScript)
	{
		powerupScript.MeshRestore(true);
		powerupScript.MaterialRefresh();
		this.powerupStealedMeshes.Remove(powerupScript);
	}

	// Token: 0x0600081E RID: 2078 RVA: 0x00034134 File Offset: 0x00032334
	public void PowerupMeshes_RestoreAll()
	{
		foreach (PowerupScript powerupScript in this.powerupStealedMeshes)
		{
			powerupScript.MeshRestore(true);
			powerupScript.MaterialRefresh();
		}
		this.powerupStealedMeshes.Clear();
	}

	// Token: 0x0600081F RID: 2079 RVA: 0x00034198 File Offset: 0x00032398
	public bool IsPowerupBuyable(PowerupScript.Identifier powerupIdentifier)
	{
		return this.buyablePowerups.Contains(powerupIdentifier);
	}

	// Token: 0x06000820 RID: 2080 RVA: 0x000341A8 File Offset: 0x000323A8
	private TerminalScript.TerminalPowerupState PowerupState_Get(PowerupScript powerup)
	{
		if (powerup == null)
		{
			return TerminalScript.TerminalPowerupState.undefined;
		}
		if (PowerupScript.IsUnlocked(powerup.identifier))
		{
			if (this.justUnlockedPowerups.Contains(powerup))
			{
				return TerminalScript.TerminalPowerupState.justUnlocked;
			}
			return TerminalScript.TerminalPowerupState.owned;
		}
		else
		{
			if (powerup == this.offeredPowerup)
			{
				return TerminalScript.TerminalPowerupState.offered;
			}
			if (this.IsPowerupBuyable(powerup.identifier))
			{
				return TerminalScript.TerminalPowerupState.outOfStock;
			}
			return TerminalScript.TerminalPowerupState.locked;
		}
	}

	// Token: 0x06000821 RID: 2081 RVA: 0x00034200 File Offset: 0x00032400
	private void OffTranslations()
	{
		this.offerNotificationText.text = Translation.Get("TERMINAL_OFFER_NOTIFICATION_TEXT");
	}

	// Token: 0x06000822 RID: 2082 RVA: 0x00034217 File Offset: 0x00032417
	public static void NotificationSet(PowerupScript.Identifier powerupIdentifier)
	{
		Data.game.TerminalNotification_Set(new Data.GameData.TerminalNotification(powerupIdentifier, "TERMINAL_NOTIFICATION_TITLE_CONGRATULATIONS", "TERMINAL_NOTIFICATION_BODY_YOU_UNLOCKED"));
	}

	// Token: 0x06000823 RID: 2083 RVA: 0x00034233 File Offset: 0x00032433
	public IEnumerator NotificationsCoroutine()
	{
		yield return null;
		while (Data.game.TerminalNotification_HasAny())
		{
			Data.GameData.TerminalNotification terminalNotification = Data.game.TerminaNotification_GetFirst(true);
			PowerupScript.Identifier powerupIdentifier = terminalNotification.GetPowerupIdentifier();
			if (powerupIdentifier != PowerupScript.Identifier.undefined && powerupIdentifier != PowerupScript.Identifier.count)
			{
				this.notificationCurrentPowerup = PowerupScript.GetPowerup_Quick(powerupIdentifier);
				if (!(this.notificationCurrentPowerup == null))
				{
					this.notificationsHolder.SetActive(true);
					this.notificationTitle.text = terminalNotification.GetTitle();
					this.notificationBody.text = terminalNotification.GetMessage();
					try
					{
						this.PowerupMesh_Steal(this.notificationCurrentPowerup, this.notificationMeshHolder.transform, true, 0.2f);
					}
					catch (Exception ex)
					{
						Debug.LogWarning("ERROR! TerminalScript.NotificationsCoroutine(): cannot steal powerup mesh. Error: " + ex.Message);
					}
					Sound.Play("SoundTerminalSelectionChange", 1f, 1f);
					while (!Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) && !Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true))
					{
						yield return null;
					}
					Sound.Play("SoundTerminalSelect", 1f, 1f);
					this.notificationsHolder.SetActive(false);
					this.PowerupMesh_Restore(this.notificationCurrentPowerup);
					int num = ((this.offeredPowerup == null) ? 0 : 1);
					this.allPowerups.Remove(this.notificationCurrentPowerup);
					this.allPowerups.Insert(num, this.notificationCurrentPowerup);
					this.justUnlockedPowerups.Add(this.notificationCurrentPowerup);
					if (Data.game.TerminalNotification_HasAny())
					{
						float timer = 0.25f;
						while (timer > 0f)
						{
							timer -= Tick.Time;
							yield return null;
						}
					}
				}
			}
		}
		yield return null;
		this.notificationsCoroutine = null;
		yield break;
	}

	// Token: 0x06000824 RID: 2084 RVA: 0x00034242 File Offset: 0x00032442
	public IEnumerator OfferNotificationCoroutine()
	{
		yield return null;
		this.offerNotificationHolder.SetActive(true);
		this.offerNotification_TitleText.text = Translation.Get("TERMINAL_OFFER_TEXT");
		this.offerNotification_PriceText.text = string.Concat(new string[]
		{
			"<color=yellow>",
			this.offeredPowerup.NameGet(false, true),
			"</color>\n",
			this.offeredPowerup.UnlockPriceGet().ToStringSmart(),
			"<sprite name=\"CoinSymbolOrange64\">"
		});
		this.PowerupMesh_Steal(this.offeredPowerup, this.offerNotification_MeshHolder.transform, true, 0.2f);
		this.offeredPowerup.MaterialColor(Color.white);
		Sound.Play("SoundTerminalOfferAd", 1f, 1f);
		while (!Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) && !Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true))
		{
			yield return null;
		}
		Sound.Play("SoundTerminalSelect", 1f, 1f);
		this.offerNotificationHolder.SetActive(false);
		this.PowerupMesh_Restore(this.offeredPowerup);
		yield return null;
		this.offerNotificationCoroutine = null;
		yield break;
	}

	// Token: 0x06000825 RID: 2085 RVA: 0x00034254 File Offset: 0x00032454
	public void Buttons_EnableStateSet(bool state)
	{
		TerminalNodeScript[] array = this.navigationButtons_PowerupNodes;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].enabled = state;
		}
		this.navigationButton_PageUp.enabled = state;
		this.navigationButton_PageDown.enabled = state;
	}

	// Token: 0x06000826 RID: 2086 RVA: 0x00034297 File Offset: 0x00032497
	public static PowerupScript HoveredPowerupGet()
	{
		if (TerminalScript.instance == null)
		{
			return null;
		}
		return TerminalScript.instance.hoveredPowerup;
	}

	// Token: 0x06000827 RID: 2087 RVA: 0x000342B2 File Offset: 0x000324B2
	public int MaxPowerupsPerPage(int pageIndex)
	{
		if (pageIndex == this.pagesCount - 1)
		{
			return this.allPowerups.Count % this.navigationButtons_PowerupNodes.Length;
		}
		return this.navigationButtons_PowerupNodes.Length;
	}

	// Token: 0x06000828 RID: 2088 RVA: 0x000342DC File Offset: 0x000324DC
	private void NavigationReset()
	{
		this.pageIndex = 0;
		this.nodeIndex = 0;
		this.pageIndexOld = -1;
		this.nodeIndexOld = -1;
	}

	// Token: 0x06000829 RID: 2089 RVA: 0x000342FC File Offset: 0x000324FC
	private void NavigationRoutine()
	{
		this.hoveredPowerup = null;
		bool flag = false;
		bool flag2 = Controls.ActionButton_PressedGet(0, Controls.InputAction.menuBack, true);
		if (this.navigationButton_Logout.IsMouseOnMe())
		{
			this.navigationButton_Logout.HoverColor();
			if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true))
			{
				GameplayMaster.instance.FCall_Terminal_Logout();
			}
		}
		if (VirtualCursors.IsCursorVisible(0, true))
		{
			global::UnityEngine.Vector2 vector = new global::UnityEngine.Vector2((float)this.gameCamera.pixelWidth, (float)this.gameCamera.pixelHeight);
			global::UnityEngine.Vector2 vector2 = VirtualCursors.CursorPositionNormalizedCenteredGet_ReferenceResolution(0, vector);
			if (Controls.MouseButton_PressedGet(0, Controls.MouseElement.LeftButton) && (vector2.x < -0.35f || vector2.x > 0.35f))
			{
				flag2 = true;
			}
		}
		if (flag2)
		{
			GameplayMaster.instance.FCall_Terminal_Logout();
		}
		for (int i = 0; i < this.navigationButtons_PowerupNodes.Length; i++)
		{
			TerminalNodeScript terminalNodeScript = this.navigationButtons_PowerupNodes[i];
			if (terminalNodeScript.IsMouseOnMe() && !(terminalNodeScript.PowerupAssigned_Get() == null) && this.nodeIndex != i)
			{
				this.nodeIndex = i;
				Sound.Play("SoundTerminalSelectionChange", 1f, 1f);
			}
		}
		bool flag3 = this.navigationButton_PageUp.IsMouseOnMe();
		bool flag4 = this.navigationButton_PageDown.IsMouseOnMe();
		if (flag3)
		{
			this.navigationButton_PageUp.HoverColor();
			if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true))
			{
				this.pageIndex++;
				Sound.Play("SoundTerminalSelectionChange", 1f, 1f);
			}
		}
		if (flag4)
		{
			this.navigationButton_PageDown.HoverColor();
			if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true))
			{
				this.pageIndex--;
				Sound.Play("SoundTerminalSelectionChange", 1f, 1f);
			}
		}
		if (this.navigationButton_Buy.gameObject.activeSelf && this.navigationButton_Buy.IsMouseOnMe())
		{
			this.navigationButton_Buy.HoverColor();
			if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true))
			{
				flag = true;
			}
		}
		global::UnityEngine.Vector2 zero = global::UnityEngine.Vector2.zero;
		zero.x += Controls.ActionAxisPair_GetValue(0, Controls.InputAction.menuMoveRight, Controls.InputAction.menuMoveLeft, true);
		zero.x += Controls.ActionAxisPair_GetValue(0, Controls.InputAction.scrollUp, Controls.InputAction.scrollDown, true);
		zero.y += Controls.ActionAxisPair_GetValue(0, Controls.InputAction.menuMoveUp, Controls.InputAction.menuMoveDown, true);
		if (zero.x < -0.7f && this.axisPrevious.x >= -0.7f)
		{
			this.nodeIndex--;
			Sound.Play("SoundTerminalSelectionChange", 1f, 1f);
		}
		if (zero.x > 0.7f && this.axisPrevious.x <= 0.7f)
		{
			this.nodeIndex++;
			Sound.Play("SoundTerminalSelectionChange", 1f, 1f);
		}
		if (zero.y > 0.7f && this.axisPrevious.y <= 0.7f)
		{
			this.pageIndex++;
			Sound.Play("SoundTerminalSelectionChange", 1f, 1f);
			this.navigationButton_PageUp.HoverColor();
		}
		if (zero.y < -0.7f && this.axisPrevious.y >= -0.7f)
		{
			this.pageIndex--;
			Sound.Play("SoundTerminalSelectionChange", 1f, 1f);
			this.navigationButton_PageDown.HoverColor();
		}
		this.axisPrevious = zero;
		if (this.player.lastInputKindUsed == Controls.InputKind.Joystick && Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true))
		{
			flag = true;
		}
		if (this.pageIndex > this.pagesCount - 1)
		{
			this.pageIndex = 0;
			this.nodeIndex = 0;
		}
		if (this.pageIndex < 0)
		{
			this.pageIndex = this.pagesCount - 1;
			this.nodeIndex = this.MaxPowerupsPerPage(this.pageIndex) - 1;
		}
		if (this.nodeIndex < 0 && this.pageIndex == this.pageIndexOld)
		{
			this.pageIndex--;
		}
		if (this.nodeIndex >= this.MaxPowerupsPerPage(this.pageIndex) && this.pageIndex == this.pageIndexOld)
		{
			this.pageIndex++;
		}
		if (this.pageIndex > this.pagesCount - 1)
		{
			this.pageIndex = 0;
			this.nodeIndex = 0;
		}
		if (this.pageIndex < 0)
		{
			this.pageIndex = this.pagesCount - 1;
			this.nodeIndex = this.MaxPowerupsPerPage(this.pageIndex) - 1;
		}
		if (this.nodeIndex < 0)
		{
			this.nodeIndex = this.MaxPowerupsPerPage(this.pageIndex) - 1;
		}
		if (this.nodeIndex >= this.MaxPowerupsPerPage(this.pageIndex))
		{
			this.nodeIndex = 0;
		}
		for (int j = 0; j < this.navigationButtons_PowerupNodes.Length; j++)
		{
			TerminalNodeScript terminalNodeScript2 = this.navigationButtons_PowerupNodes[j];
			bool flag5 = j == this.nodeIndex;
			int num = j + this.pageIndex * this.navigationButtons_PowerupNodes.Length;
			if (num >= this.allPowerups.Count)
			{
				terminalNodeScript2.HoveredState_Set(false);
				terminalNodeScript2.PowerupAssigned_Set(null, TerminalScript.TerminalPowerupState.undefined);
			}
			else
			{
				PowerupScript powerupScript = this.allPowerups[num];
				terminalNodeScript2.HoveredState_Set(flag5);
				this.hoveredPowerup = (flag5 ? terminalNodeScript2.PowerupAssigned_Get() : this.hoveredPowerup);
				TerminalScript.TerminalPowerupState terminalPowerupState = this.PowerupState_Get(powerupScript);
				terminalNodeScript2.PowerupAssigned_Set(powerupScript, terminalPowerupState);
			}
		}
		if (this.pageIndex != this.pageIndexOld || this.nodeIndex != this.nodeIndexOld || this.hoveredPowerup != this.hoveredPowerupOld)
		{
			if (this.hoveredPowerupOld != null)
			{
				this.PowerupMesh_Restore(this.hoveredPowerupOld);
			}
			this.pageText.text = (this.pageIndex + 1).ToString() + "/" + this.pagesCount.ToString();
			if (this.hoveredPowerup == null)
			{
				this.inspector_StateText.text = "";
				this.inspector_TitleText.text = "";
				this.inspector_DescriptionText.text = "";
				this.inspector_UnlockInfosText.text = "";
				this.buyText.text = "";
				this.navigationButton_Buy.gameObject.SetActive(false);
				this.inspector_StateTextBounceScr.SetBounceScaleDecaySpeed(1f);
				this.inspector_BuyAnnoyingText.enabled = false;
				this.inspector_BannedText.enabled = false;
			}
			else
			{
				TerminalScript.TerminalPowerupState terminalPowerupState2 = this.PowerupState_Get(this.hoveredPowerup);
				bool flag6 = Data.GameData.IsPowerupSecret(this.hoveredPowerup.identifier) && (terminalPowerupState2 == TerminalScript.TerminalPowerupState.locked || terminalPowerupState2 == TerminalScript.TerminalPowerupState.outOfStock);
				string text;
				switch (terminalPowerupState2)
				{
				case TerminalScript.TerminalPowerupState.owned:
					text = Translation.Get("TERMINAL_POWERUP_STATE_OWNED");
					break;
				case TerminalScript.TerminalPowerupState.offered:
					text = "<color=white>" + this.hoveredPowerup.UnlockPriceGet().ToStringSmart() + "</color><sprite name=\"CoinSymbolOrange64\">";
					break;
				case TerminalScript.TerminalPowerupState.locked:
					text = "<color=#804000>" + Translation.Get("TERMINAL_POWERUP_STATE_LOCKED") + "</color>";
					break;
				case TerminalScript.TerminalPowerupState.outOfStock:
					text = "<color=red>" + Translation.Get("TERMINAL_POWERUP_STATE_OUT_OF_STOCK") + "</color>";
					break;
				case TerminalScript.TerminalPowerupState.justUnlocked:
					text = Translation.Get("TERMINAL_POWERUP_STATE_JUST_UNLOCKED");
					break;
				default:
					text = "Error!";
					Debug.LogError("TerminalScript: PowerupState not handled. State: " + terminalPowerupState2.ToString());
					break;
				}
				this.inspector_StateText.text = text;
				if (flag6)
				{
					this.inspector_TitleText.text = "???";
				}
				else
				{
					this.inspector_TitleText.text = this.hoveredPowerup.NameGet(false, true);
				}
				string text2;
				if (terminalPowerupState2 != TerminalScript.TerminalPowerupState.outOfStock && !flag6)
				{
					text2 = this.hoveredPowerup.DescriptionGet(true, true, false, 0.01f);
				}
				else
				{
					text2 = "<color=red>...</color>";
				}
				this.inspector_DescriptionText.text = text2;
				string text3;
				if (terminalPowerupState2 != TerminalScript.TerminalPowerupState.outOfStock && !flag6)
				{
					text3 = this.hoveredPowerup.UnlockMissionGet();
				}
				else
				{
					text3 = "<color=red>...</color>";
				}
				if ((terminalPowerupState2 == TerminalScript.TerminalPowerupState.owned || terminalPowerupState2 == TerminalScript.TerminalPowerupState.justUnlocked) && !this.hoveredPowerup.IsBaseSet())
				{
					this.inspector_UnlockInfosText.fontStyle = 64;
				}
				else
				{
					this.inspector_UnlockInfosText.fontStyle = 0;
				}
				this.inspector_UnlockInfosText.text = text3;
				if (terminalPowerupState2 == TerminalScript.TerminalPowerupState.offered)
				{
					BigInteger bigInteger = this.hoveredPowerup.UnlockPriceGet();
					BigInteger bigInteger2 = GameplayData.CoinsGet();
					string text4 = "";
					if (this.player.lastInputKindUsed == Controls.InputKind.Joystick)
					{
						text4 = "   " + Translation.Get("TEXT_PROMPT_CLOSE") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.menuBack, 0);
					}
					if (bigInteger2 >= bigInteger)
					{
						this.buyText.text = string.Concat(new string[]
						{
							Translation.Get("TEXT_PROMPT_BUY"),
							" <color=white>(-",
							this.hoveredPowerup.UnlockPriceGet().ToStringSmart(),
							"<sprite name=\"CoinSymbolOrange64\">)</color> ",
							(this.player.lastInputKindUsed == Controls.InputKind.Joystick) ? (Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.menuSelect, 0) + "</size>") : "<sprite name=\"FingerPointerOrange\">",
							text4
						});
					}
					else
					{
						this.buyText.text = "<color=red>" + Translation.Get("TEXT_PROMPT_NOT_ENOUGH_MONEY") + "</color>" + text4;
					}
				}
				else
				{
					string text5 = "";
					if (this.player.lastInputKindUsed == Controls.InputKind.Joystick)
					{
						text5 = Translation.Get("TEXT_PROMPT_CLOSE") + " " + Controls.MapGetLastPrompt_TextSprite_FavorMouseOverKeyboard(0, Controls.InputAction.menuBack, 0);
					}
					this.buyText.text = text5 ?? "";
				}
				if (this.player.lastInputKindUsed == Controls.InputKind.Joystick)
				{
					this.buyText.rectTransform.sizeDelta = new global::UnityEngine.Vector2(0.4f, this.buyText.rectTransform.sizeDelta.y);
					this.buyText.rectTransform.anchoredPosition = new global::UnityEngine.Vector2(0.348f, this.buyText.rectTransform.anchoredPosition.y);
				}
				else
				{
					this.buyText.rectTransform.sizeDelta = new global::UnityEngine.Vector2(0.25f, this.buyText.rectTransform.sizeDelta.y);
					this.buyText.rectTransform.anchoredPosition = new global::UnityEngine.Vector2(0.194f, this.buyText.rectTransform.anchoredPosition.y);
				}
				if (terminalPowerupState2 == TerminalScript.TerminalPowerupState.offered && this.player.lastInputKindUsed != Controls.InputKind.Joystick)
				{
					this.navigationButton_Buy.gameObject.SetActive(true);
					this.navigationButton_Buy.FlashState_Set(true);
				}
				else
				{
					this.navigationButton_Buy.gameObject.SetActive(false);
				}
				this.navigationButton_Logout.gameObject.SetActive(this.player.lastInputKindUsed != Controls.InputKind.Joystick);
				this.PowerupMesh_Steal(this.hoveredPowerup, this.navigationInspectorMeshHolder.transform, true, 0.175f);
				if (terminalPowerupState2 == TerminalScript.TerminalPowerupState.offered)
				{
					this.inspector_StateTextBounceScr.SetBounceScale(0.1f);
					this.inspector_StateTextBounceScr.SetBounceScaleDecaySpeed(0f);
				}
				else
				{
					this.inspector_StateTextBounceScr.SetBounceScaleDecaySpeed(1f);
				}
				this.inspector_BuyAnnoyingText.enabled = terminalPowerupState2 == TerminalScript.TerminalPowerupState.offered;
				this.inspector_BannedText.enabled = terminalPowerupState2 != TerminalScript.TerminalPowerupState.offered && this.hoveredPowerup != null && PowerupScript.IsBanned(this.hoveredPowerup.identifier, this.hoveredPowerup.archetype);
			}
			this.pageIndexOld = this.pageIndex;
			this.nodeIndexOld = this.nodeIndex;
			this.hoveredPowerupOld = this.hoveredPowerup;
		}
		if (this.hoveredPowerup != null && this.offeredPowerup == this.hoveredPowerup && flag)
		{
			Controls.VibrationSet_PreferMax(this.player, 0.5f);
			BigInteger bigInteger3 = this.hoveredPowerup.UnlockPriceGet();
			if (bigInteger3 < 0L)
			{
				Debug.LogWarning("Cost was negative while trying to unlock powerup by buying. Powerup: " + this.hoveredPowerup.identifier.ToString());
				bigInteger3 = 0;
			}
			if (GameplayData.CoinsGet() >= bigInteger3)
			{
				GameplayData.AlreadyBoughtPowerupAtTerminalSet();
				this.buyablePowerups.Remove(this.hoveredPowerup.identifier);
				this.offeredPowerup = null;
				GameplayData.CoinsAdd(-bigInteger3, false);
				PowerupScript.Unlock(this.hoveredPowerup.identifier);
				Sound.Play("SoundTerminalPowerupBought", 1f, 1f);
				this.NavigationReset();
				return;
			}
			Sound.Play("SoundMenuError", 1f, 1f);
			CameraGame.Shake(1f);
		}
	}

	// Token: 0x0600082A RID: 2090 RVA: 0x00034F48 File Offset: 0x00033148
	public static void Initialize()
	{
		if (TerminalScript.instance == null)
		{
			return;
		}
		TerminalScript.instance.allPowerups.Clear();
		int num = 164;
		for (int i = 0; i < num; i++)
		{
			PowerupScript powerup_Quick = PowerupScript.GetPowerup_Quick((PowerupScript.Identifier)i);
			if (!(powerup_Quick == null))
			{
				TerminalScript.instance.allPowerups.Add(powerup_Quick);
			}
		}
		TerminalScript.instance.buyablePowerups.Clear();
		List<PowerupScript.Identifier> list = Data.game.LockedPowerups_GetList();
		for (int j = 0; j < list.Count; j++)
		{
			PowerupScript.Identifier identifier = list[j];
			if (identifier != PowerupScript.Identifier.undefined)
			{
				PowerupScript powerup_Quick2 = PowerupScript.GetPowerup_Quick(identifier);
				if (!(powerup_Quick2 == null) && !(powerup_Quick2.UnlockPriceGet() <= 0L))
				{
					TerminalScript.instance.buyablePowerups.Add(powerup_Quick2.identifier);
				}
			}
		}
		for (int k = 0; k < TerminalScript.instance.buyablePowerups.Count; k++)
		{
			PowerupScript.Identifier identifier2 = TerminalScript.instance.buyablePowerups[k];
			PowerupScript powerup_Quick3 = PowerupScript.GetPowerup_Quick(identifier2);
			if (!(powerup_Quick3 == null))
			{
				BigInteger bigInteger = powerup_Quick3.UnlockPriceGet();
				for (int l = k + 1; l < TerminalScript.instance.buyablePowerups.Count; l++)
				{
					PowerupScript.Identifier identifier3 = TerminalScript.instance.buyablePowerups[l];
					PowerupScript powerup_Quick4 = PowerupScript.GetPowerup_Quick(identifier3);
					if (!(powerup_Quick4 == null))
					{
						BigInteger bigInteger2 = powerup_Quick4.UnlockPriceGet();
						if (bigInteger > bigInteger2)
						{
							TerminalScript.instance.buyablePowerups[k] = identifier3;
							TerminalScript.instance.buyablePowerups[l] = identifier2;
							bigInteger = bigInteger2;
						}
					}
				}
			}
		}
		TerminalScript.instance.offeredPowerup = null;
		if (TerminalScript.instance.buyablePowerups.Count > 0 && !GameplayData.AlreadyBoughtPowerupAtTerminalGet())
		{
			TerminalScript.instance.offeredPowerup = PowerupScript.GetPowerup_Quick(TerminalScript.instance.buyablePowerups[0]);
		}
		bool flag = false;
		if (TerminalScript.instance.offeredPowerup != null)
		{
			flag = true;
			TerminalScript.instance.allPowerups.Remove(TerminalScript.instance.offeredPowerup);
			TerminalScript.instance.allPowerups.Insert(0, TerminalScript.instance.offeredPowerup);
		}
		for (int m = list.Count - 1; m >= 0; m--)
		{
			PowerupScript.Identifier identifier4 = list[m];
			if (identifier4 != PowerupScript.Identifier.undefined && identifier4 != PowerupScript.Identifier.count)
			{
				PowerupScript powerup_Quick5 = PowerupScript.GetPowerup_Quick(identifier4);
				if (!(powerup_Quick5 == null) && !PowerupScript.IsUnlocked(powerup_Quick5.identifier) && !TerminalScript.instance.IsPowerupBuyable(powerup_Quick5.identifier))
				{
					int num2 = (flag ? 1 : 0);
					TerminalScript.instance.allPowerups.Remove(powerup_Quick5);
					TerminalScript.instance.allPowerups.Insert(num2, powerup_Quick5);
				}
			}
		}
		if (!Master.IsDemo)
		{
			TerminalScript.instance.allPowerups.Remove(PowerupScript.GetPowerup_Quick(PowerupScript.Identifier.Skeleton_Arm1));
			TerminalScript.instance.allPowerups.Remove(PowerupScript.GetPowerup_Quick(PowerupScript.Identifier.Skeleton_Arm2));
			TerminalScript.instance.allPowerups.Remove(PowerupScript.GetPowerup_Quick(PowerupScript.Identifier.Skeleton_Leg1));
			TerminalScript.instance.allPowerups.Remove(PowerupScript.GetPowerup_Quick(PowerupScript.Identifier.Skeleton_Leg2));
			TerminalScript.instance.allPowerups.Remove(PowerupScript.GetPowerup_Quick(PowerupScript.Identifier.Skeleton_Head));
			TerminalScript.instance.allPowerups.Add(PowerupScript.GetPowerup_Quick(PowerupScript.Identifier.Skeleton_Arm1));
			TerminalScript.instance.allPowerups.Add(PowerupScript.GetPowerup_Quick(PowerupScript.Identifier.Skeleton_Arm2));
			TerminalScript.instance.allPowerups.Add(PowerupScript.GetPowerup_Quick(PowerupScript.Identifier.Skeleton_Leg1));
			TerminalScript.instance.allPowerups.Add(PowerupScript.GetPowerup_Quick(PowerupScript.Identifier.Skeleton_Leg2));
			TerminalScript.instance.allPowerups.Add(PowerupScript.GetPowerup_Quick(PowerupScript.Identifier.Skeleton_Head));
		}
		for (int n = 0; n < TerminalScript.instance.buyablePowerups.Count; n++)
		{
			PowerupScript powerup_Quick6 = PowerupScript.GetPowerup_Quick(TerminalScript.instance.buyablePowerups[n]);
			if (!(powerup_Quick6 == null) && !(powerup_Quick6 == TerminalScript.instance.offeredPowerup) && !PowerupScript.IsUnlocked(powerup_Quick6.identifier))
			{
				TerminalScript.instance.allPowerups.Remove(powerup_Quick6);
				TerminalScript.instance.allPowerups.Add(powerup_Quick6);
			}
		}
		TerminalScript.instance.pagesCount = Mathf.CeilToInt((float)TerminalScript.instance.allPowerups.Count / (float)TerminalScript.instance.navigationButtons_PowerupNodes.Length);
	}

	// Token: 0x0600082B RID: 2091 RVA: 0x000353B1 File Offset: 0x000335B1
	private void Awake()
	{
		TerminalScript.instance = this;
	}

	// Token: 0x0600082C RID: 2092 RVA: 0x000353BC File Offset: 0x000335BC
	private void Start()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		this.gameCamera = CameraGame.firstInstance.myCamera;
		this.player = Controls.GetPlayerByIndex(0);
		this.mailHolder.SetActive(false);
		this.notificationsHolder.SetActive(false);
		this.offerPreviewHolder.SetActive(false);
		this.offerNotificationHolder.SetActive(false);
		this.navigationHolder.SetActive(false);
		Translation.OnLanguageChanged = (UnityAction)Delegate.Combine(Translation.OnLanguageChanged, new UnityAction(this.OffTranslations));
		this.OffTranslations();
	}

	// Token: 0x0600082D RID: 2093 RVA: 0x0003544F File Offset: 0x0003364F
	private void OnDestroy()
	{
		if (TerminalScript.instance == this)
		{
			TerminalScript.instance = null;
		}
		Translation.OnLanguageChanged = (UnityAction)Delegate.Remove(Translation.OnLanguageChanged, new UnityAction(this.OffTranslations));
	}

	// Token: 0x0600082E RID: 2094 RVA: 0x00035484 File Offset: 0x00033684
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		TerminalScript.IsLoggedIn();
		bool flag = Data.game.TerminalNotification_HasAny();
		bool flag2 = this.offeredPowerup != null;
		switch (this.state)
		{
		case TerminalScript.State.turnedOff_Request:
			TerminalScript.SetState(TerminalScript.State.turnedOff_nothing);
			break;
		case TerminalScript.State.turnedOff_nothing:
			if (flag)
			{
				TerminalScript.SetState(TerminalScript.State.turnedOff_Email);
			}
			else if (flag2)
			{
				TerminalScript.SetState(TerminalScript.State.turnedOff_OfferPreview);
			}
			else
			{
				bool flag3 = Util.AngleSin(Tick.PassedTime * 720f) > 0.75f;
				this.offNothingText.text = ">" + (flag3 ? "" : "_");
			}
			break;
		case TerminalScript.State.turnedOff_Email:
			if (!flag)
			{
				TerminalScript.SetState(TerminalScript.State.turnedOff_OfferPreview);
			}
			else if (this.youGotMailTimer > 0f)
			{
				if (!PowerupTriggerAnimController.HasAnimations())
				{
					this.youGotMailTimer -= Tick.Time;
				}
				if (this.youGotMailTimer <= 0f)
				{
					Sound.Play3D("SoundTerminalYouGotMail", base.transform.position, 20f, 1f, 1f, 1);
				}
			}
			break;
		case TerminalScript.State.turnedOff_OfferPreview:
			if (!flag2)
			{
				TerminalScript.SetState(TerminalScript.State.turnedOff_nothing);
			}
			else if (flag)
			{
				TerminalScript.SetState(TerminalScript.State.turnedOff_Email);
			}
			else
			{
				bool flag4 = Util.AngleSin(Tick.PassedTime * 720f) <= 0.75f;
				this.offerNotificationText.gameObject.SetActive(flag4);
			}
			break;
		case TerminalScript.State.turnedOn_Request:
			TerminalScript.SetState(TerminalScript.State.navigation);
			break;
		case TerminalScript.State.navigation:
			if (flag)
			{
				TerminalScript.SetState(TerminalScript.State.notification);
			}
			else if (!this.offerNotificationShown && this.offeredPowerup != null)
			{
				this.offerNotificationShown = true;
				TerminalScript.SetState(TerminalScript.State.offerNotification);
			}
			else
			{
				this.NavigationRoutine();
				this.navigationInspectorMeshHolder.transform.AddLocalYAngle(180f * Tick.Time);
				bool flag5 = Util.AngleSin(Tick.PassedTime * 720f) < 0.75f;
				if (flag5 && this.inspector_BuyAnnoyingText.alpha < 0.5f)
				{
					this.inspector_BuyAnnoyingText.alpha = 1f;
				}
				else if (!flag5 && this.inspector_BuyAnnoyingText.alpha > 0.5f)
				{
					this.inspector_BuyAnnoyingText.alpha = 0f;
				}
				if (flag5 && this.inspector_BannedText.alpha < 0.5f)
				{
					this.inspector_BannedText.alpha = 1f;
				}
				else if (!flag5 && this.inspector_BannedText.alpha > 0.5f)
				{
					this.inspector_BannedText.alpha = 0f;
				}
				Color color = TerminalScript.C_ORANGE_FADED;
				if (this.hoveredPowerup != null && this.hoveredPowerup == this.offeredPowerup)
				{
					color = ((Util.AngleSin(Tick.PassedTime * 1440f) > 0f) ? TerminalScript.C_YELLOW : TerminalScript.C_ORANGE);
				}
				if (this.navigationGlobBehindInspectorPowerup.color != color)
				{
					this.navigationGlobBehindInspectorPowerup.color = color;
				}
			}
			break;
		case TerminalScript.State.notification:
			if (!flag && this.notificationsCoroutine == null)
			{
				if (!flag2)
				{
					TerminalScript.SetState(TerminalScript.State.navigation);
				}
				else
				{
					this.offerNotificationShown = true;
					TerminalScript.SetState(TerminalScript.State.offerNotification);
				}
			}
			else
			{
				this.notificationMeshHolder.transform.AddLocalYAngle(180f * Tick.Time);
				this.notificationStarCutoutRectTr.AddLocalZAngle(180f * Tick.Time);
			}
			break;
		case TerminalScript.State.offerNotification:
			if (this.offerNotificationCoroutine == null)
			{
				TerminalScript.SetState(TerminalScript.State.navigation);
			}
			else
			{
				this.offerNotification_MeshHolder.transform.AddLocalYAngle(180f * Tick.Time);
			}
			break;
		}
		bool flag6 = TerminalScript.IsLoggedIn();
		if (flag6 && !Sound.IsPlaying("SoundTerminalFanLoop"))
		{
			Sound.Play3D("SoundTerminalFanLoop", base.transform.position, 10f, 1f, 1f, 1);
		}
		if (!flag6 && this.powerupStealedMeshes.Count > 0)
		{
			this.PowerupMeshes_RestoreAll();
		}
	}

	public static TerminalScript instance;

	private const int PLAYER_INDEX = 0;

	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	private static Color C_ORANGE_FADED = new Color(1f, 0.5f, 0f, 0.25f);

	private static Color C_YELLOW = new Color(1f, 0.75f, 0f, 1f);

	private Camera gameCamera;

	private Controls.PlayerExt player;

	public DiegeticMenuController myMenuController;

	public GameObject offNothingHolder;

	public TextMeshProUGUI offNothingText;

	public GameObject offerPreviewHolder;

	public TextMeshProUGUI offerNotificationText;

	public GameObject mailHolder;

	public GameObject navigationHolder;

	public Transform navigationInspectorMeshHolder;

	public Image navigationGlobBehindInspectorPowerup;

	public TerminalButton navigationButton_Buy;

	public TerminalButton navigationButton_Logout;

	public TerminalButton navigationButton_PageUp;

	public TerminalButton navigationButton_PageDown;

	public TerminalNodeScript[] navigationButtons_PowerupNodes;

	public TextMeshProUGUI collectionTitleText;

	public TextMeshProUGUI buyText;

	public TextMeshProUGUI pageText;

	public TextMeshProUGUI inspector_BuyAnnoyingText;

	public TextMeshProUGUI inspector_BannedText;

	public TextMeshProUGUI inspector_StateText;

	public BounceScript inspector_StateTextBounceScr;

	public TextMeshProUGUI inspector_TitleText;

	public TextMeshProUGUI inspector_DescriptionText;

	public TextMeshProUGUI inspector_UnlockInfosText;

	public GameObject notificationsHolder;

	public TextMeshProUGUI notificationTitle;

	public TextMeshProUGUI notificationBody;

	public GameObject notificationMeshHolder;

	public RectTransform notificationStarCutoutRectTr;

	public GameObject offerNotificationHolder;

	public TextMeshProUGUI offerNotification_TitleText;

	public TextMeshProUGUI offerNotification_PriceText;

	public GameObject offerNotification_MeshHolder;

	[NonSerialized]
	public TerminalScript.State state;

	private List<PowerupScript> powerupStealedMeshes = new List<PowerupScript>();

	private List<PowerupScript> allPowerups = new List<PowerupScript>(200);

	private List<PowerupScript.Identifier> buyablePowerups = new List<PowerupScript.Identifier>();

	private PowerupScript offeredPowerup;

	private float youGotMailTimer;

	private Coroutine notificationsCoroutine;

	private PowerupScript notificationCurrentPowerup;

	private List<PowerupScript> justUnlockedPowerups = new List<PowerupScript>();

	private bool offerNotificationShown;

	private Coroutine offerNotificationCoroutine;

	private global::UnityEngine.Vector2 axisPrevious;

	private int pageIndex;

	private int nodeIndex;

	private int pagesCount = -1;

	private PowerupScript hoveredPowerup;

	private int pageIndexOld = -1;

	private int nodeIndexOld = -1;

	private PowerupScript hoveredPowerupOld;

	public enum State
	{
		turnedOff_Request,
		turnedOff_nothing,
		turnedOff_Email,
		turnedOff_OfferPreview,
		turnedOn_Request,
		navigation,
		notification,
		offerNotification
	}

	public enum TerminalPowerupState
	{
		undefined,
		owned,
		offered,
		locked,
		outOfStock,
		justUnlocked
	}
}
