using System;
using System.Collections.Generic;
using System.Text;
using Panik;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000061 RID: 97
public class RedButtonScript : MonoBehaviour
{
	// Token: 0x06000749 RID: 1865 RVA: 0x0003C59C File Offset: 0x0003A79C
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

	// Token: 0x0600074A RID: 1866 RVA: 0x0000BFDF File Offset: 0x0000A1DF
	public static bool IsCharmTriggerableNow(PowerupScript.Identifier identifier)
	{
		return GameplayData.Powerup_ButtonChargesUsed_Get(identifier) <= 0;
	}

	// Token: 0x0600074B RID: 1867 RVA: 0x0003C610 File Offset: 0x0003A810
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

	// Token: 0x0600074C RID: 1868 RVA: 0x0003C6B8 File Offset: 0x0003A8B8
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

	// Token: 0x0600074D RID: 1869 RVA: 0x0003C778 File Offset: 0x0003A978
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

	// Token: 0x0600074E RID: 1870 RVA: 0x0000BFED File Offset: 0x0000A1ED
	public static bool HasPowerupsRegistered()
	{
		return !(RedButtonScript.instance == null) && RedButtonScript.RegisteredPowerupsGet().Count > 0;
	}

	// Token: 0x0600074F RID: 1871 RVA: 0x0000C00B File Offset: 0x0000A20B
	public static void DamagedCapsules_Add(RedButtonScript.RegistrationCapsule capsule)
	{
		if (RedButtonScript.instance == null)
		{
			return;
		}
		RedButtonScript.instance.damagedRegistrations.Add(capsule);
	}

	// Token: 0x06000750 RID: 1872 RVA: 0x0003C874 File Offset: 0x0003AA74
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

	// Token: 0x06000751 RID: 1873 RVA: 0x0003C9D4 File Offset: 0x0003ABD4
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

	// Token: 0x06000752 RID: 1874 RVA: 0x0000C02B File Offset: 0x0000A22B
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

	// Token: 0x06000753 RID: 1875 RVA: 0x0003CAA4 File Offset: 0x0003ACA4
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

	// Token: 0x06000754 RID: 1876 RVA: 0x0003CCD8 File Offset: 0x0003AED8
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

	// Token: 0x06000755 RID: 1877 RVA: 0x0003CE08 File Offset: 0x0003B008
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

	// Token: 0x06000756 RID: 1878 RVA: 0x0003CEF8 File Offset: 0x0003B0F8
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

	// Token: 0x06000757 RID: 1879 RVA: 0x0003CFE8 File Offset: 0x0003B1E8
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

	// Token: 0x06000758 RID: 1880 RVA: 0x0003D0CC File Offset: 0x0003B2CC
	private void VisualsRoutine()
	{
		GameplayMaster.GamePhase gamePhase = GameplayMaster.GetGamePhase();
		if (this.buttonHasTriggers)
		{
			this.flashing = true;
			bool flashingLightsReducedEnabled = Data.settings.flashingLightsReducedEnabled;
			if (Util.AngleSin(Tick.PassedTime * (flashingLightsReducedEnabled ? 180f : 720f)) > 0f)
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

	// Token: 0x06000759 RID: 1881 RVA: 0x0000C063 File Offset: 0x0000A263
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

	// Token: 0x0600075A RID: 1882 RVA: 0x0000C092 File Offset: 0x0000A292
	public static bool ButtonIsFlashing()
	{
		return !(RedButtonScript.instance == null) && RedButtonScript.instance.flashing;
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x0003D2C0 File Offset: 0x0003B4C0
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

	// Token: 0x0600075C RID: 1884 RVA: 0x0000C0AD File Offset: 0x0000A2AD
	public static bool UiIsShowing()
	{
		return !(RedButtonScript.instance == null) && RedButtonScript.instance._uiIsShowing;
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x0000C0C8 File Offset: 0x0000A2C8
	private bool _IsNoTimingCharm(PowerupScript powerup)
	{
		return this.noTimingCharmsDictionary.ContainsKey(powerup) && this.noTimingCharmsDictionary[powerup];
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x0000C0E6 File Offset: 0x0000A2E6
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

	// Token: 0x0600075F RID: 1887 RVA: 0x0000C11A File Offset: 0x0000A31A
	private void Awake()
	{
		RedButtonScript.instance = this;
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x0000C122 File Offset: 0x0000A322
	private void OnDestroy()
	{
		if (RedButtonScript.instance == this)
		{
			RedButtonScript.instance = null;
		}
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x0000C137 File Offset: 0x0000A337
	private void Start()
	{
		RedButtonScript.ButtonVisualsRefresh();
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x0000C13E File Offset: 0x0000A33E
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		this.VisualsRoutine();
		this.UiRoutine(RedButtonScript.RegisteredPowerupsGet_OnlyTriggerables(), RedButtonScript.RegisteredPowerupsGet_ActivatedOnes());
	}

	// Token: 0x0400064D RID: 1613
	public static RedButtonScript instance;

	// Token: 0x0400064E RID: 1614
	public static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x0400064F RID: 1615
	public DiegeticMenuElement myMenuElement;

	// Token: 0x04000650 RID: 1616
	public MeshRenderer buttonRenderer;

	// Token: 0x04000651 RID: 1617
	public MeshRenderer buttonBaseRenderer;

	// Token: 0x04000652 RID: 1618
	public Material buttonMaterial_Off;

	// Token: 0x04000653 RID: 1619
	public Material buttonMaterial_On;

	// Token: 0x04000654 RID: 1620
	public BounceScript twitchBounceScript;

	// Token: 0x04000655 RID: 1621
	public GameObject baloonHolder;

	// Token: 0x04000656 RID: 1622
	public Transform uiBaloonShakerTransform;

	// Token: 0x04000657 RID: 1623
	public RawImage uiBaloonImage;

	// Token: 0x04000658 RID: 1624
	public RawImage uiBaloonPointImage;

	// Token: 0x04000659 RID: 1625
	public Transform stackTransform;

	// Token: 0x0400065A RID: 1626
	private List<CharmUiRenderer> charmUiRenderers = new List<CharmUiRenderer>();

	// Token: 0x0400065B RID: 1627
	private List<CharmUiRenderer> charmUiRenderers_ActiveOnes = new List<CharmUiRenderer>();

	// Token: 0x0400065C RID: 1628
	private List<RedButtonScript.RegistrationCapsule> registrationsPool = new List<RedButtonScript.RegistrationCapsule>();

	// Token: 0x0400065D RID: 1629
	private Dictionary<PowerupScript, RedButtonScript.RegistrationCapsule> registrations = new Dictionary<PowerupScript, RedButtonScript.RegistrationCapsule>();

	// Token: 0x0400065E RID: 1630
	private int registeredCount;

	// Token: 0x0400065F RID: 1631
	private List<PowerupScript> _tempRegistrationsList = new List<PowerupScript>();

	// Token: 0x04000660 RID: 1632
	private List<PowerupScript> _temp_AvailableRegistrations_List = new List<PowerupScript>();

	// Token: 0x04000661 RID: 1633
	private List<PowerupScript> _temp_UntriggerableRegistrations_List = new List<PowerupScript>();

	// Token: 0x04000662 RID: 1634
	private static StringBuilder damagedCapsulesStringBuilder = new StringBuilder();

	// Token: 0x04000663 RID: 1635
	private List<RedButtonScript.RegistrationCapsule> damagedRegistrations = new List<RedButtonScript.RegistrationCapsule>();

	// Token: 0x04000664 RID: 1636
	private List<RedButtonScript.RegistrationCapsule> tempRegistrations = new List<RedButtonScript.RegistrationCapsule>();

	// Token: 0x04000665 RID: 1637
	private bool buttonHasTriggers;

	// Token: 0x04000666 RID: 1638
	private bool flashing;

	// Token: 0x04000667 RID: 1639
	private int powerupsEquippedCountOld;

	// Token: 0x04000668 RID: 1640
	private float buttonTwitchTimer;

	// Token: 0x04000669 RID: 1641
	private bool stackRequestUpdate;

	// Token: 0x0400066A RID: 1642
	private bool _uiIsShowing;

	// Token: 0x0400066B RID: 1643
	private Dictionary<PowerupScript, bool> noTimingCharmsDictionary = new Dictionary<PowerupScript, bool>();

	// Token: 0x0400066C RID: 1644
	public RedButtonScript.RedButtonEvent onButtonActivatedSomething;

	// Token: 0x02000062 RID: 98
	// (Invoke) Token: 0x06000766 RID: 1894
	public delegate void ButtonCallback(PowerupScript powerup);

	// Token: 0x02000063 RID: 99
	public class RegistrationCapsule
	{
		// Token: 0x0400066D RID: 1645
		public PowerupScript powerup;

		// Token: 0x0400066E RID: 1646
		public RedButtonScript.ButtonCallback onPressed;

		// Token: 0x0400066F RID: 1647
		public RedButtonScript.ButtonCallback onReset;

		// Token: 0x04000670 RID: 1648
		public RedButtonScript.RegistrationCapsule.Timing timing = RedButtonScript.RegistrationCapsule.Timing.undefined;

		// Token: 0x02000064 RID: 100
		public enum Timing
		{
			// Token: 0x04000672 RID: 1650
			undefined = -1,
			// Token: 0x04000673 RID: 1651
			noTiming,
			// Token: 0x04000674 RID: 1652
			perSpin,
			// Token: 0x04000675 RID: 1653
			perRound,
			// Token: 0x04000676 RID: 1654
			perDeadline,
			// Token: 0x04000677 RID: 1655
			count
		}
	}

	// Token: 0x02000065 RID: 101
	// (Invoke) Token: 0x0600076B RID: 1899
	public delegate void RedButtonEvent();
}
