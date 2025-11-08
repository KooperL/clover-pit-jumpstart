using System;
using System.Collections.Generic;
using System.Text;
using Panik;
using UnityEngine;
using UnityEngine.UI;

public class RedButtonScript : MonoBehaviour
{
	// Token: 0x060006B1 RID: 1713 RVA: 0x0002A3DC File Offset: 0x000285DC
	private static void CapsuleRemoveAndCleanUp(RedButtonScript.RegistrationCapsule capsule)
	{
		if (capsule == null)
		{
			return;
		}
		RedButtonScript.instance.registrations.Remove(capsule.powerup);
		RedButtonScript.instance.registrationsPool.Add(capsule);
		if (capsule.powerup != null)
		{
			GameplayData.Powerup_ButtonBurnedOut_Set(capsule.powerup.identifier, 0);
		}
		capsule.powerup = null;
		capsule.onPressed = null;
		capsule.onReset = null;
		capsule.timing = RedButtonScript.RegistrationCapsule.Timing.undefined;
	}

	// Token: 0x060006B2 RID: 1714 RVA: 0x0002A44E File Offset: 0x0002864E
	public static bool IsCharmTriggerableNow(PowerupScript.Identifier identifier)
	{
		return GameplayData.Powerup_ButtonChargesUsed_Get(identifier) <= 0;
	}

	// Token: 0x060006B3 RID: 1715 RVA: 0x0002A45C File Offset: 0x0002865C
	public static List<PowerupScript> RegisteredPowerupsGet()
	{
		if (RedButtonScript.instance == null)
		{
			return null;
		}
		RedButtonScript.instance._tempRegistrationsList.Clear();
		foreach (KeyValuePair<PowerupScript, RedButtonScript.RegistrationCapsule> keyValuePair in RedButtonScript.instance.registrations)
		{
			if (keyValuePair.Key != null && keyValuePair.Value != null)
			{
				RedButtonScript.instance._tempRegistrationsList.Add(keyValuePair.Key);
			}
		}
		return RedButtonScript.instance._tempRegistrationsList;
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x0002A504 File Offset: 0x00028704
	public static List<PowerupScript> RegisteredPowerupsGet_OnlyTriggerables()
	{
		if (RedButtonScript.instance == null)
		{
			return null;
		}
		RedButtonScript.instance._temp_AvailableRegistrations_List.Clear();
		foreach (KeyValuePair<PowerupScript, RedButtonScript.RegistrationCapsule> keyValuePair in RedButtonScript.instance.registrations)
		{
			if (keyValuePair.Key != null && keyValuePair.Value != null && RedButtonScript.IsCharmTriggerableNow(keyValuePair.Value.powerup.identifier))
			{
				RedButtonScript.instance._temp_AvailableRegistrations_List.Add(keyValuePair.Key);
			}
		}
		return RedButtonScript.instance._temp_AvailableRegistrations_List;
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x0002A5C4 File Offset: 0x000287C4
	public static List<PowerupScript> RegisteredPowerupsGet_ActivatedOnes()
	{
		if (RedButtonScript.instance == null)
		{
			return null;
		}
		RedButtonScript.instance._temp_UntriggerableRegistrations_List.Clear();
		foreach (KeyValuePair<PowerupScript, RedButtonScript.RegistrationCapsule> keyValuePair in RedButtonScript.instance.registrations)
		{
			if (keyValuePair.Key != null && keyValuePair.Value != null)
			{
				PowerupScript key = keyValuePair.Key;
				int num = GameplayData.Powerup_ButtonBurnedOut_Get(keyValuePair.Value.powerup.identifier);
				if (RedButtonScript.instance._IsNoTimingCharm(key))
				{
					int num2 = RedButtonScript.instance._NoTimingCharm_GetCounterToShow(key.identifier);
					if (num2 >= 0)
					{
						num = num2;
					}
				}
				if (num > 0)
				{
					RedButtonScript.instance._temp_UntriggerableRegistrations_List.Add(keyValuePair.Key);
				}
			}
		}
		return RedButtonScript.instance._temp_UntriggerableRegistrations_List;
	}

	// Token: 0x060006B6 RID: 1718 RVA: 0x0002A6C0 File Offset: 0x000288C0
	public static bool HasPowerupsRegistered()
	{
		return !(RedButtonScript.instance == null) && RedButtonScript.RegisteredPowerupsGet().Count > 0;
	}

	// Token: 0x060006B7 RID: 1719 RVA: 0x0002A6DE File Offset: 0x000288DE
	public static void DamagedCapsules_Add(RedButtonScript.RegistrationCapsule capsule)
	{
		if (RedButtonScript.instance == null)
		{
			return;
		}
		RedButtonScript.instance.damagedRegistrations.Add(capsule);
	}

	// Token: 0x060006B8 RID: 1720 RVA: 0x0002A700 File Offset: 0x00028900
	public static void DamagedCapsules_Clean()
	{
		if (RedButtonScript.instance == null)
		{
			return;
		}
		if (RedButtonScript.instance.damagedRegistrations.Count == 0)
		{
			return;
		}
		RedButtonScript.damagedCapsulesStringBuilder.Clear();
		RedButtonScript.damagedCapsulesStringBuilder.Append("Error! Red button has -Damaged Capsules-:\n");
		foreach (RedButtonScript.RegistrationCapsule registrationCapsule in RedButtonScript.instance.damagedRegistrations)
		{
			if (registrationCapsule != null)
			{
				RedButtonScript.damagedCapsulesStringBuilder.Append(registrationCapsule.powerup.name);
				if (registrationCapsule.powerup == null)
				{
					RedButtonScript.damagedCapsulesStringBuilder.Append(" - Powerup is null");
				}
				if (registrationCapsule.onPressed == null)
				{
					RedButtonScript.damagedCapsulesStringBuilder.Append(" - Callback is null");
				}
				if (registrationCapsule.timing != RedButtonScript.RegistrationCapsule.Timing.undefined || registrationCapsule.timing == RedButtonScript.RegistrationCapsule.Timing.count)
				{
					RedButtonScript.damagedCapsulesStringBuilder.Append(" - Timing: " + registrationCapsule.timing.ToString());
				}
				RedButtonScript.damagedCapsulesStringBuilder.Append("\n");
				RedButtonScript.CapsuleRemoveAndCleanUp(registrationCapsule);
			}
		}
		string text = RedButtonScript.damagedCapsulesStringBuilder.ToString();
		Debug.LogError(text);
		ConsolePrompt.LogError(text, "", 0f);
		RedButtonScript.instance.damagedRegistrations.Clear();
	}

	// Token: 0x060006B9 RID: 1721 RVA: 0x0002A860 File Offset: 0x00028A60
	public static void PowerupRegistration_Add(PowerupScript powerup, RedButtonScript.ButtonCallback onPress, RedButtonScript.RegistrationCapsule.Timing timing, RedButtonScript.ButtonCallback onReset = null)
	{
		if (RedButtonScript.instance == null)
		{
			return;
		}
		RedButtonScript.RegistrationCapsule registrationCapsule;
		if (RedButtonScript.instance.registrations.ContainsKey(powerup))
		{
			registrationCapsule = RedButtonScript.instance.registrations[powerup];
		}
		else
		{
			registrationCapsule = ((RedButtonScript.instance.registrationsPool.Count > 0) ? RedButtonScript.instance.registrationsPool[0] : new RedButtonScript.RegistrationCapsule());
			RedButtonScript.instance.registrations.Add(powerup, registrationCapsule);
		}
		registrationCapsule.powerup = powerup;
		registrationCapsule.onPressed = onPress;
		registrationCapsule.onReset = onReset;
		registrationCapsule.timing = timing;
		if (timing == RedButtonScript.RegistrationCapsule.Timing.noTiming && !RedButtonScript.instance.noTimingCharmsDictionary.ContainsKey(powerup))
		{
			RedButtonScript.instance.noTimingCharmsDictionary.Add(powerup, true);
		}
		RedButtonScript.instance.registrationsPool.Remove(registrationCapsule);
	}

	// Token: 0x060006BA RID: 1722 RVA: 0x0002A92E File Offset: 0x00028B2E
	public static void PowerupRegistration_Remove(PowerupScript powerup)
	{
		if (RedButtonScript.instance == null)
		{
			return;
		}
		if (!RedButtonScript.instance.registrations.ContainsKey(powerup))
		{
			return;
		}
		RedButtonScript.CapsuleRemoveAndCleanUp(RedButtonScript.instance.registrations[powerup]);
	}

	// Token: 0x060006BB RID: 1723 RVA: 0x0002A968 File Offset: 0x00028B68
	public void Press()
	{
		if (RedButtonScript.instance == null)
		{
			return;
		}
		bool flag = false;
		bool flag2 = true;
		foreach (KeyValuePair<PowerupScript, RedButtonScript.RegistrationCapsule> keyValuePair in RedButtonScript.instance.registrations)
		{
			if (keyValuePair.Value == null || keyValuePair.Value.powerup == null || keyValuePair.Value.onPressed == null || keyValuePair.Value.timing == RedButtonScript.RegistrationCapsule.Timing.undefined || keyValuePair.Value.timing == RedButtonScript.RegistrationCapsule.Timing.count)
			{
				RedButtonScript.DamagedCapsules_Add(keyValuePair.Value);
			}
			else
			{
				PowerupScript.Identifier identifier = keyValuePair.Value.powerup.identifier;
				if (RedButtonScript.IsCharmTriggerableNow(identifier))
				{
					if (!flag && PowerupScript.SacredHeart_EvaluateNoChargeConsumption())
					{
						flag2 = false;
					}
					flag = true;
					int num = GameplayData.RedButtonActivationsMultiplierGet(true);
					for (int i = 0; i < num; i++)
					{
						GameplayData.Powerup_ButtonBurnedOut_Increaase(identifier);
						if (keyValuePair.Value.onPressed != null)
						{
							keyValuePair.Value.onPressed(keyValuePair.Value.powerup);
						}
					}
					if (flag2)
					{
						GameplayData.Powerup_ButtonChargesUsed_ConsumeAllCharges(identifier, true);
					}
				}
			}
		}
		RedButtonScript.DamagedCapsules_Clean();
		RedButtonScript.ButtonVisualsRefresh();
		if (flag)
		{
			CameraGame.Shake(1f);
			CameraGame.ChromaticAberrationIntensitySet(1f);
			Sound.Play("SoundRedButtonPressWithActivations", 1f, 1f);
		}
		else
		{
			Sound.Play("SoundRedButtonPress", 1f, 1f);
		}
		if (flag)
		{
			RedButtonScript.RedButtonEvent redButtonEvent = this.onButtonActivatedSomething;
			if (redButtonEvent != null)
			{
				redButtonEvent();
			}
		}
		if (flag && !this.buttonHasTriggers)
		{
			DiegeticMenuController.SlotMenu.HoveredElement = SlotMachineScript.instance.leverMenuElement;
		}
		int num2 = GameplayData.Stats_RedButtonEffectiveActivations_Get();
		num2++;
		GameplayData.Stats_RedButtonEffectiveActivations_Set(num2);
		if (flag)
		{
			Data.GameData game = Data.game;
			int num3 = game.UnlockSteps_Cross;
			game.UnlockSteps_Cross = num3 + 1;
			Data.GameData game2 = Data.game;
			num3 = game2.UnlockSteps_ElectricityCounter;
			game2.UnlockSteps_ElectricityCounter = num3 + 1;
		}
		if (flag)
		{
			RunModifierScript.TriggerAnimation_IfEquipped(RunModifierScript.Identifier.redButtonOverload);
		}
		if (flag)
		{
			if (num2 >= 1)
			{
				PowerupScript.Unlock(PowerupScript.Identifier.CarBattery);
			}
			if (num2 >= 3)
			{
				PowerupScript.Unlock(PowerupScript.Identifier.Button2X);
			}
			if (num2 >= 20)
			{
				PowerupScript.Unlock(PowerupScript.Identifier.StepsCounter);
			}
		}
	}

	// Token: 0x060006BC RID: 1724 RVA: 0x0002AB9C File Offset: 0x00028D9C
	public static void ResetTiming(RedButtonScript.RegistrationCapsule.Timing timing)
	{
		if (RedButtonScript.instance == null)
		{
			return;
		}
		RedButtonScript.instance.tempRegistrations.Clear();
		foreach (KeyValuePair<PowerupScript, RedButtonScript.RegistrationCapsule> keyValuePair in RedButtonScript.instance.registrations)
		{
			RedButtonScript.instance.tempRegistrations.Add(keyValuePair.Value);
		}
		for (int i = 0; i < RedButtonScript.instance.tempRegistrations.Count; i++)
		{
			RedButtonScript.RegistrationCapsule registrationCapsule = RedButtonScript.instance.tempRegistrations[i];
			if (registrationCapsule == null || registrationCapsule.powerup == null || registrationCapsule.onPressed == null)
			{
				RedButtonScript.DamagedCapsules_Add(registrationCapsule);
			}
			else if (GameplayData.Powerup_ButtonBurnedOut_Get(registrationCapsule.powerup.identifier) > 0 && (registrationCapsule.timing == timing || registrationCapsule.timing == RedButtonScript.RegistrationCapsule.Timing.noTiming))
			{
				GameplayData.Powerup_ButtonBurnedOut_Set(registrationCapsule.powerup.identifier, 0);
				if (registrationCapsule.onReset != null)
				{
					registrationCapsule.onReset(registrationCapsule.powerup);
				}
			}
		}
		RedButtonScript.DamagedCapsules_Clean();
		RedButtonScript.ButtonVisualsRefresh();
	}

	// Token: 0x060006BD RID: 1725 RVA: 0x0002ACCC File Offset: 0x00028ECC
	public static bool IsAnyTriggerAvailable()
	{
		if (RedButtonScript.instance == null)
		{
			return false;
		}
		if (RedButtonScript.instance.registrations.Count == 0)
		{
			return false;
		}
		bool flag = false;
		foreach (KeyValuePair<PowerupScript, RedButtonScript.RegistrationCapsule> keyValuePair in RedButtonScript.instance.registrations)
		{
			if (keyValuePair.Value == null || keyValuePair.Value.powerup == null || keyValuePair.Value.onPressed == null || keyValuePair.Value.timing == RedButtonScript.RegistrationCapsule.Timing.undefined || keyValuePair.Value.timing == RedButtonScript.RegistrationCapsule.Timing.count)
			{
				RedButtonScript.DamagedCapsules_Add(keyValuePair.Value);
			}
			else if (RedButtonScript.IsCharmTriggerableNow(keyValuePair.Value.powerup.identifier))
			{
				flag = true;
				break;
			}
		}
		RedButtonScript.DamagedCapsules_Clean();
		return flag;
	}

	// Token: 0x060006BE RID: 1726 RVA: 0x0002ADBC File Offset: 0x00028FBC
	public static int TriggersAvailableGetCount()
	{
		if (RedButtonScript.instance == null)
		{
			return 0;
		}
		if (RedButtonScript.instance.registrations.Count == 0)
		{
			return 0;
		}
		int num = 0;
		foreach (KeyValuePair<PowerupScript, RedButtonScript.RegistrationCapsule> keyValuePair in RedButtonScript.instance.registrations)
		{
			if (keyValuePair.Value == null || keyValuePair.Value.powerup == null || keyValuePair.Value.onPressed == null || keyValuePair.Value.timing == RedButtonScript.RegistrationCapsule.Timing.undefined || keyValuePair.Value.timing == RedButtonScript.RegistrationCapsule.Timing.count)
			{
				RedButtonScript.DamagedCapsules_Add(keyValuePair.Value);
			}
			else if (RedButtonScript.IsCharmTriggerableNow(keyValuePair.Value.powerup.identifier))
			{
				num++;
			}
		}
		RedButtonScript.DamagedCapsules_Clean();
		return num;
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x0002AEAC File Offset: 0x000290AC
	public static void RestoreCharges(int n)
	{
		if (RedButtonScript.instance == null)
		{
			return;
		}
		if (n <= 0)
		{
			return;
		}
		foreach (KeyValuePair<PowerupScript, RedButtonScript.RegistrationCapsule> keyValuePair in RedButtonScript.instance.registrations)
		{
			if (keyValuePair.Value == null || keyValuePair.Value.powerup == null || keyValuePair.Value.onPressed == null || keyValuePair.Value.timing == RedButtonScript.RegistrationCapsule.Timing.undefined || keyValuePair.Value.timing == RedButtonScript.RegistrationCapsule.Timing.count)
			{
				RedButtonScript.DamagedCapsules_Add(keyValuePair.Value);
			}
			else
			{
				GameplayData.Powerup_ButtonChargesUsed_RestoreChargesN_Ext(keyValuePair.Value.powerup.identifier, n, true, false);
			}
		}
		RedButtonScript.DamagedCapsules_Clean();
		RedButtonScript.ButtonVisualsRefresh();
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x0002AF90 File Offset: 0x00029190
	private void VisualsRoutine()
	{
		GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
		if (this.buttonHasTriggers)
		{
			this.flashing = true;
			if (Util.AngleSin(Tick.PassedTime * 720f) > 0f)
			{
				if (this.buttonRenderer.sharedMaterial != this.buttonMaterial_On)
				{
					this.buttonRenderer.sharedMaterial = this.buttonMaterial_On;
				}
				if (this.buttonBaseRenderer.sharedMaterial != this.buttonMaterial_On)
				{
					this.buttonBaseRenderer.sharedMaterial = this.buttonMaterial_On;
				}
			}
			else
			{
				if (this.buttonRenderer.sharedMaterial != this.buttonMaterial_Off)
				{
					this.buttonRenderer.sharedMaterial = this.buttonMaterial_Off;
				}
				if (this.buttonBaseRenderer.sharedMaterial != this.buttonMaterial_Off)
				{
					this.buttonBaseRenderer.sharedMaterial = this.buttonMaterial_Off;
				}
			}
			this.buttonTwitchTimer -= Tick.Time;
			if (this.buttonTwitchTimer <= 0f)
			{
				this.buttonTwitchTimer = 1f;
				this.twitchBounceScript.SetBounceScale(0.15f);
				float num = 1f;
				if (gamePhase == GameplayMaster.GamePhase.gambling && !SlotMachineScript.IsSpinning())
				{
					num = 0.5f;
				}
				Sound.Play3D("SoundRedButtonTwitching", base.transform.position, 3f, num, global::UnityEngine.Random.Range(0.9f, 1.1f), AudioRolloffMode.Linear);
			}
		}
		else
		{
			this.flashing = false;
			if (this.buttonRenderer.sharedMaterial != this.buttonMaterial_Off)
			{
				this.buttonRenderer.sharedMaterial = this.buttonMaterial_Off;
			}
			if (this.buttonBaseRenderer.sharedMaterial != this.buttonMaterial_Off)
			{
				this.buttonBaseRenderer.sharedMaterial = this.buttonMaterial_Off;
			}
		}
		int count = PowerupScript.list_EquippedNormal.Count;
		if (count != this.powerupsEquippedCountOld)
		{
			this.powerupsEquippedCountOld = count;
			RedButtonScript.ButtonVisualsRefresh();
		}
	}

	// Token: 0x060006C1 RID: 1729 RVA: 0x0002B16F File Offset: 0x0002936F
	public static void ButtonVisualsRefresh()
	{
		if (RedButtonScript.instance == null)
		{
			return;
		}
		RedButtonScript.instance.buttonHasTriggers = RedButtonScript.IsAnyTriggerAvailable();
		PowerupScript.RedButtonTextRefresh_All();
		RedButtonScript.instance.stackRequestUpdate = true;
	}

	// Token: 0x060006C2 RID: 1730 RVA: 0x0002B19E File Offset: 0x0002939E
	public static bool ButtonIsFlashing()
	{
		return !(RedButtonScript.instance == null) && RedButtonScript.instance.flashing;
	}

	// Token: 0x060006C3 RID: 1731 RVA: 0x0002B1BC File Offset: 0x000293BC
	private void UiRoutine(List<PowerupScript> triggerableCharms, List<PowerupScript> activeCharms)
	{
		bool flag = this.myMenuElement.IsHovered() && GameplayMaster.GetGamePhase() == GameplayMaster.GamePhase.gambling && !SlotMachineScript.IsSpinning() && triggerableCharms.Count > 0 && CameraController.SlotMachineLook_Get() == CameraController.SlotMachineLookingSides.front;
		if (this.baloonHolder.activeSelf != flag)
		{
			this._uiIsShowing = flag;
			if (flag)
			{
				for (int i = this.charmUiRenderers.Count - 1; i >= 0; i--)
				{
					if (!(this.charmUiRenderers[i] == null))
					{
						CharmUiRenderer.PoolDestroy(this.charmUiRenderers[i]);
					}
				}
				this.charmUiRenderers.Clear();
				float num = Mathf.Min(0.9f, 0.3f * (float)triggerableCharms.Count);
				float num2 = num / (float)triggerableCharms.Count;
				bool flag2 = triggerableCharms.Count > 3;
				for (int j = 0; j < triggerableCharms.Count; j++)
				{
					CharmUiRenderer charmUiRenderer = CharmUiRenderer.PoolSpawn(triggerableCharms[j].identifier, 0, this.uiBaloonImage.transform, true, 0.4f, 0.5f, true, false, flag2 ? "" : triggerableCharms[j].GetBatteryString(), null);
					float num3 = 0.2f;
					if (flag2)
					{
						num3 = ((j % 2 == 0) ? 0.1f : 0.3f);
					}
					float num4 = 0.15f;
					charmUiRenderer.transform.localPosition = new Vector3(num3, num4 + num2 * (float)j, 0f);
					this.charmUiRenderers.Add(charmUiRenderer);
				}
				this.uiBaloonImage.rectTransform.sizeDelta = new Vector2(this.uiBaloonImage.rectTransform.sizeDelta.x, Mathf.Max(0.3f, num + 0.25f));
			}
			this.baloonHolder.SetActive(flag);
		}
		if (this._uiIsShowing)
		{
			float num5 = 0.001f;
			this.uiBaloonShakerTransform.localPosition = new Vector3(global::UnityEngine.Random.Range(-num5, num5), global::UnityEngine.Random.Range(-num5, num5), global::UnityEngine.Random.Range(-num5, num5));
		}
		if (this.stackRequestUpdate)
		{
			this.stackRequestUpdate = false;
			for (int k = this.charmUiRenderers_ActiveOnes.Count - 1; k >= 0; k--)
			{
				if (!(this.charmUiRenderers_ActiveOnes[k] == null))
				{
					CharmUiRenderer.PoolDestroy(this.charmUiRenderers_ActiveOnes[k]);
				}
			}
			this.charmUiRenderers_ActiveOnes.Clear();
			float num6 = Mathf.Min(0.9f, 0.075f * (float)activeCharms.Count) / (float)activeCharms.Count;
			for (int l = 0; l < activeCharms.Count; l++)
			{
				int num7 = GameplayData.Powerup_ButtonBurnedOut_Get(activeCharms[l].identifier);
				if (this._IsNoTimingCharm(activeCharms[l]))
				{
					int num8 = this._NoTimingCharm_GetCounterToShow(activeCharms[l].identifier);
					if (num8 >= 0)
					{
						num7 = num8;
					}
				}
				CharmUiRenderer charmUiRenderer2 = CharmUiRenderer.PoolSpawn(activeCharms[l].identifier, 0, this.stackTransform, true, 0.25f, 0.4f, true, false, "", (num7 <= 0) ? null : num7.ToString());
				float num9 = ((l % 2 == 0) ? 0f : 0.1f);
				float num10 = 0f;
				charmUiRenderer2.transform.localPosition = new Vector3(num9, num10 + num6 * (float)l, 0f);
				this.charmUiRenderers_ActiveOnes.Add(charmUiRenderer2);
			}
		}
	}

	// Token: 0x060006C4 RID: 1732 RVA: 0x0002B52B File Offset: 0x0002972B
	public static bool UiIsShowing()
	{
		return !(RedButtonScript.instance == null) && RedButtonScript.instance._uiIsShowing;
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x0002B546 File Offset: 0x00029746
	private bool _IsNoTimingCharm(PowerupScript powerup)
	{
		return this.noTimingCharmsDictionary.ContainsKey(powerup) && this.noTimingCharmsDictionary[powerup];
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x0002B564 File Offset: 0x00029764
	private int _NoTimingCharm_GetCounterToShow(PowerupScript.Identifier powerupIdentifier)
	{
		if (powerupIdentifier <= PowerupScript.Identifier.RingBell)
		{
			if (powerupIdentifier == PowerupScript.Identifier.HorseShoeGold)
			{
				return GameplayData.Powerup_GoldenHorseShoe_SpinsLeftGet();
			}
			if (powerupIdentifier == PowerupScript.Identifier.RingBell)
			{
				return 0;
			}
		}
		else
		{
			if (powerupIdentifier == PowerupScript.Identifier.WeirdClock)
			{
				return 0;
			}
			if (powerupIdentifier == PowerupScript.Identifier.AncientCoin)
			{
				return GameplayData.Powerup_AncientCoin_SpinsLeftGet();
			}
			if (powerupIdentifier == PowerupScript.Identifier.Cross)
			{
				return 0;
			}
		}
		return -1;
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x0002B598 File Offset: 0x00029798
	private void Awake()
	{
		RedButtonScript.instance = this;
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x0002B5A0 File Offset: 0x000297A0
	private void OnDestroy()
	{
		if (RedButtonScript.instance == this)
		{
			RedButtonScript.instance = null;
		}
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x0002B5B5 File Offset: 0x000297B5
	private void Start()
	{
		RedButtonScript.ButtonVisualsRefresh();
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x0002B5BC File Offset: 0x000297BC
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		this.VisualsRoutine();
		this.UiRoutine(RedButtonScript.RegisteredPowerupsGet_OnlyTriggerables(), RedButtonScript.RegisteredPowerupsGet_ActivatedOnes());
	}

	public static RedButtonScript instance;

	public static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	public DiegeticMenuElement myMenuElement;

	public MeshRenderer buttonRenderer;

	public MeshRenderer buttonBaseRenderer;

	public Material buttonMaterial_Off;

	public Material buttonMaterial_On;

	public BounceScript twitchBounceScript;

	public GameObject baloonHolder;

	public Transform uiBaloonShakerTransform;

	public RawImage uiBaloonImage;

	public RawImage uiBaloonPointImage;

	public Transform stackTransform;

	private List<CharmUiRenderer> charmUiRenderers = new List<CharmUiRenderer>();

	private List<CharmUiRenderer> charmUiRenderers_ActiveOnes = new List<CharmUiRenderer>();

	private List<RedButtonScript.RegistrationCapsule> registrationsPool = new List<RedButtonScript.RegistrationCapsule>();

	private Dictionary<PowerupScript, RedButtonScript.RegistrationCapsule> registrations = new Dictionary<PowerupScript, RedButtonScript.RegistrationCapsule>();

	private int registeredCount;

	private List<PowerupScript> _tempRegistrationsList = new List<PowerupScript>();

	private List<PowerupScript> _temp_AvailableRegistrations_List = new List<PowerupScript>();

	private List<PowerupScript> _temp_UntriggerableRegistrations_List = new List<PowerupScript>();

	private static StringBuilder damagedCapsulesStringBuilder = new StringBuilder();

	private List<RedButtonScript.RegistrationCapsule> damagedRegistrations = new List<RedButtonScript.RegistrationCapsule>();

	private List<RedButtonScript.RegistrationCapsule> tempRegistrations = new List<RedButtonScript.RegistrationCapsule>();

	private bool buttonHasTriggers;

	private bool flashing;

	private int powerupsEquippedCountOld;

	private float buttonTwitchTimer;

	private bool stackRequestUpdate;

	private bool _uiIsShowing;

	private Dictionary<PowerupScript, bool> noTimingCharmsDictionary = new Dictionary<PowerupScript, bool>();

	public RedButtonScript.RedButtonEvent onButtonActivatedSomething;

	// (Invoke) Token: 0x0600114C RID: 4428
	public delegate void ButtonCallback(PowerupScript powerup);

	public class RegistrationCapsule
	{
		public PowerupScript powerup;

		public RedButtonScript.ButtonCallback onPressed;

		public RedButtonScript.ButtonCallback onReset;

		public RedButtonScript.RegistrationCapsule.Timing timing = RedButtonScript.RegistrationCapsule.Timing.undefined;

		public enum Timing
		{
			undefined = -1,
			noTiming,
			perSpin,
			perRound,
			perDeadline,
			count
		}
	}

	// (Invoke) Token: 0x06001151 RID: 4433
	public delegate void RedButtonEvent();
}
