using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Febucci.UI;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EE RID: 238
public class StatsScript : MonoBehaviour
{
	// Token: 0x06000BDE RID: 3038 RVA: 0x0000FC62 File Offset: 0x0000DE62
	public static bool IsEnabled()
	{
		return !(StatsScript.instance == null) && StatsScript.instance.statsHolder.activeSelf;
	}

	// Token: 0x06000BDF RID: 3039 RVA: 0x0005F49C File Offset: 0x0005D69C
	private void UpdateTextString()
	{
		if (this.textAnimator_StatsText == null)
		{
			return;
		}
		this.sb.Clear();
		if (StatsScript.myShowKind == StatsScript.ShowKind.endDeath)
		{
			this.sb.Append("<color=orange><size=200%>");
			if (GameplayData.IsInVictoryCondition())
			{
				if (Master.IsDemo)
				{
					this.sb.Append(Translation.Get("END_GAME_STATS_TITLE_DEATH_WIN_DEMO"));
				}
				else
				{
					switch (RewardBoxScript.GetRewardKind())
					{
					case RewardBoxScript.RewardKind.DrawerKey0:
						this.sb.Append(Translation.Get("END_GAME_STATS_TITLE_DEATH_WIN_DRAWER"));
						this.sb.Append("1");
						break;
					case RewardBoxScript.RewardKind.DrawerKey1:
						this.sb.Append(Translation.Get("END_GAME_STATS_TITLE_DEATH_WIN_DRAWER"));
						this.sb.Append("2");
						break;
					case RewardBoxScript.RewardKind.DrawerKey2:
						this.sb.Append(Translation.Get("END_GAME_STATS_TITLE_DEATH_WIN_DRAWER"));
						this.sb.Append("3");
						break;
					case RewardBoxScript.RewardKind.DrawerKey3:
						this.sb.Append(Translation.Get("END_GAME_STATS_TITLE_DEATH_WIN_DRAWER"));
						this.sb.Append("4");
						break;
					}
				}
			}
			else
			{
				this.sb.Append(Translation.Get("END_GAME_STATS_TITLE_DEATH"));
			}
		}
		else
		{
			if (GameplayData.NineNineNine_IsTime())
			{
				this.sb.Append("<color=yellow><size=200%>");
			}
			else
			{
				this.sb.Append("<color=orange><size=200%>");
			}
			this.sb.Append(Translation.Get("END_GAME_STATS_TITLE_ALIVE"));
		}
		this.sb.Append("</size></color>\n\n");
		if (GameplayMaster.IsCustomSeed())
		{
			this.sb.Append("( <color=red>");
			this.sb.Append(Translation.Get("END_GAME_STATS_SEEDED_RUN"));
			this.sb.Append("</color> )");
			this.sb.Append("</color> )");
			this.sb.Append("\n");
		}
		this.sb.Append(GeneralUiScript.GameVersionString_Get());
		this.sb.Append("\n");
		object obj = Data.game != null && PowerupScript.all.Count > 0 && Data.game.PowerupRealInstances_AreAllUnlocked() && Data.game.goodEndingCounter > 0;
		this.sb.Append(Translation.Get("MENU_LABEL_SEED_DOUBLE_DOT"));
		this.sb.Append(" ");
		object obj2 = obj;
		if (obj2 != null)
		{
			this.sb.Append("<color=yellow>");
		}
		this.sb.Append(GameplayData.SeedGetAsString());
		if (obj2 != null)
		{
			this.sb.Append("</color>");
		}
		if (Data.game != null && GameplayData.Instance != null && Data.game.RunModifier_HardcoreMode_Get(GameplayData.RunModifier_GetCurrent()))
		{
			this.sb.Append(" ");
			this.sb.Append("<sprite name=\"SkullSymbolOrange64\">");
		}
		this.sb.Append("\n");
		this.sb.Append(Translation.Get("END_GAME_STATS_TIME_PLAYED"));
		this.sb.Append(" ");
		long num = GameplayData.Stats_PlayTime_GetSeconds();
		long num2 = num / 60L;
		long num3 = num % 60L;
		this.sb.Append(num2.ToString("00"));
		this.sb.Append("' ");
		this.sb.Append(num3.ToString("00"));
		this.sb.Append("\"\n");
		this.sb.Append(Translation.Get("END_GAME_STATS_DEADLINES_COMPLETED"));
		this.sb.Append(" ");
		long num4 = GameplayData.Stats_DeadlinesCompleted_Get();
		this.sb.Append(num4);
		this.sb.Append("\n");
		this.sb.Append(Translation.Get("END_GAME_STATS_SPINS_DONE"));
		this.sb.Append(" ");
		int num5 = GameplayData.SpinsDoneInARun_Get();
		this.sb.Append(num5);
		this.sb.Append("\n");
		this.sb.Append(Translation.Get("END_GAME_STATS_COINS_EARNED"));
		this.sb.Append(" ");
		BigInteger bigInteger = GameplayData.Stats_CoinsEarned_Get();
		this.sb.Append(bigInteger.ToStringSmart());
		this.sb.Append(" ");
		this.sb.Append("<sprite name=\"CoinSymbolOrange64\">");
		this.sb.Append("\n");
		this.sb.Append(Translation.Get("END_GAME_STATS_TICKETS_EARNED"));
		this.sb.Append(" ");
		BigInteger bigInteger2 = GameplayData.Stats_TicketsEarned_Get();
		this.sb.Append(bigInteger2.ToStringSmart());
		this.sb.Append(" ");
		this.sb.Append("<sprite name=\"CloverTicket\">");
		this.sb.Append("\n");
		this.sb.Append(Translation.Get("END_GAME_STATS_CHARMS_BOUGHT"));
		this.sb.Append(" ");
		long num6 = GameplayData.Stats_CharmsBought_Get();
		this.sb.Append(num6);
		if (DeckBoxScript.IsEnabled())
		{
			this.sb.Append("\n");
			this.sb.Append("<sprite name=\"MemoryCard\">");
			this.sb.Append("<color=orange>:</color> ");
			this.sb.Append(RunModifierScript.TitleGet(GameplayData.RunModifier_GetCurrent()));
		}
		if (this.promptTime)
		{
			this.sb.Append("\n\n");
		}
		if (this.promptTime)
		{
			this.textAnimator_StatsText.tmproText.verticalAlignment = VerticalAlignmentOptions.Top;
		}
		else
		{
			this.textAnimator_StatsText.tmproText.verticalAlignment = VerticalAlignmentOptions.Middle;
		}
		this.textAnimator_StatsText.tmproText.text = this.sb.ToString();
		this.textAnimator_StatsText.tmproText.ForceMeshUpdate(false, false);
		if (this.promptTime)
		{
			this.tooltipTextAnimator.gameObject.SetActive(true);
			if (!this.tooltipsTextUpdated)
			{
				if (GameplayData.IsInVictoryCondition() && RewardBoxScript.GetRewardKind() == RewardBoxScript.RewardKind.DoorKey)
				{
					this.tooltipTextAnimator.SyncText(Translation.Get("TOOLTIP_ENDING"), false);
				}
				else
				{
					this.tooltipTextAnimator.SyncText(Translation.Get("TOOLTIP_" + ((GameplayMaster.DeathsSinceStartup_GetNum() - 1) % 8).ToString()), false);
				}
			}
			this.tooltipsTextUpdated = true;
		}
		this.textAnimator_CharmsTitle.tmproText.text = Translation.Get("END_GAME_STATS_TITLE_CHARMS");
		this.textAnimator_PhoneTitle.tmproText.text = Translation.Get("END_GAME_STATS_TITLE_PHONE");
		this.noCharmsText.text = Translation.Get("END_GAME_STATS_NO_CHARMS");
		this.noPhoneText.text = Translation.Get("END_GAME_STATS_NO_PHONE_ABILITIES");
	}

	// Token: 0x06000BE0 RID: 3040 RVA: 0x0005FB98 File Offset: 0x0005DD98
	public static void Open(StatsScript.ShowKind showKind)
	{
		if (StatsScript.IsEnabled())
		{
			return;
		}
		StatsScript.instance.statsHolder.SetActive(true);
		StatsScript.myShowKind = showKind;
		Controls.VibrationSet_PreferMax(StatsScript.instance.player, 0.25f);
		StatsScript.instance.StartCoroutine(StatsScript.instance.ShowStatsCoroutine(showKind));
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x0000FC82 File Offset: 0x0000DE82
	private IEnumerator ShowStatsCoroutine(StatsScript.ShowKind showKind)
	{
		this.promptTime = false;
		Color white = Color.white;
		this.textAnimator_StatsText.tmproText.color = white;
		this.textAnimator_Prompt.tmproText.color = white;
		this.tooltipTextAnimator.tmproText.color = white;
		this.textAnimator_CharmsTitle.tmproText.color = StatsScript.C_ORANGE;
		this.textAnimator_PhoneTitle.tmproText.color = StatsScript.C_ORANGE;
		this.textAnimator_StatsText.tmproText.alpha = 0f;
		this.textAnimator_Prompt.tmproText.alpha = 0f;
		this.textAnimator_CharmsTitle.tmproText.alpha = 0f;
		this.textAnimator_PhoneTitle.tmproText.alpha = 0f;
		this.noCharmsText.alpha = 0f;
		this.noPhoneText.alpha = 0f;
		this.statsScaler.localScale = global::UnityEngine.Vector3.one * 1.5f;
		this.UpdateTextString();
		float showTimer = 0f;
		while (showTimer < 1f)
		{
			showTimer += Tick.Time * 10f;
			showTimer = Mathf.Clamp01(showTimer);
			this.textAnimator_StatsText.tmproText.alpha = showTimer * showTimer;
			this.textAnimator_Prompt.tmproText.alpha = showTimer * showTimer;
			this.statsScaler.localScale = global::UnityEngine.Vector3.Lerp(this.statsScaler.localScale, global::UnityEngine.Vector3.one, showTimer);
			yield return null;
		}
		float timer = 0f;
		while (timer < 2f)
		{
			timer += Tick.Time;
			if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) && timer > 0.5f)
			{
				break;
			}
			yield return null;
		}
		this.charmsAndAbilitiesTime = true;
		Sound.Play("SoundMenuPopUp", 1f, 1f);
		this.textAnimator_CharmsTitle.tmproText.alpha = 1f;
		this.textAnimator_PhoneTitle.tmproText.alpha = 1f;
		base.StartCoroutine(this.ShowAbilities(1f));
		base.StartCoroutine(this.ShowCharms(1f));
		this.UpdateTextString();
		timer = 0f;
		while (timer < 2f)
		{
			timer += Tick.Time;
			if (Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true) && timer > 0.5f)
			{
				break;
			}
			yield return null;
		}
		this.promptTime = true;
		this.UpdateTextString();
		Sound.Play("SoundMenuPopUp", 1f, 1f);
		Controls.VibrationSet_PreferMax(StatsScript.instance.player, 0.1f);
		timer = 0f;
		while (timer < 0.25f)
		{
			timer += Tick.Time;
			yield return null;
		}
		while (!Controls.ActionButton_PressedGet(0, Controls.InputAction.menuSelect, true))
		{
			yield return null;
		}
		if (GameplayMaster.GetGamePhase() != GameplayMaster.GamePhase.endingWithoutDeath)
		{
			Sound.Play("SoundMenuSelect", 1f, 1f);
		}
		Controls.VibrationSet_PreferMax(this.player, 0.25f);
		this.statsScaler.gameObject.SetActive(false);
		timer = 0f;
		while (timer < 1.5f)
		{
			timer += Tick.Time;
			yield return null;
		}
		StatsScript.Close();
		yield break;
	}

	// Token: 0x06000BE2 RID: 3042 RVA: 0x0000FC91 File Offset: 0x0000DE91
	private IEnumerator ShowCharms(float maxTime)
	{
		float timer = 0f;
		List<PowerupScript> powerups = new List<PowerupScript>();
		List<PowerupScript> drawersPowerups = new List<PowerupScript>();
		for (int j = 0; j < PowerupScript.list_EquippedSkeleton.Count; j++)
		{
			if (!(PowerupScript.list_EquippedSkeleton[j] == null))
			{
				powerups.Add(PowerupScript.list_EquippedSkeleton[j]);
			}
		}
		for (int k = 0; k < PowerupScript.list_EquippedNormal.Count; k++)
		{
			if (!(PowerupScript.list_EquippedNormal[k] == null))
			{
				powerups.Add(PowerupScript.list_EquippedNormal[k]);
			}
		}
		for (int l = 0; l < PowerupScript.array_InDrawer.Length; l++)
		{
			if (!(PowerupScript.array_InDrawer[l] == null))
			{
				powerups.Add(PowerupScript.array_InDrawer[l]);
				drawersPowerups.Add(PowerupScript.array_InDrawer[l]);
			}
		}
		if (powerups.Count == 0)
		{
			this.noCharmsText.alpha = 1f;
			yield break;
		}
		if (powerups.Count > 20)
		{
			this.charmsArea.anchoredPosition -= new global::UnityEngine.Vector2(0f, 30f);
		}
		else if (powerups.Count <= 15)
		{
			this.charmsArea.anchoredPosition += new global::UnityEngine.Vector2(0f, 30f);
		}
		float scaleMult = ((powerups.Count < 3) ? 1.25f : ((powerups.Count < 5) ? 1f : ((powerups.Count <= 20) ? 0.8f : 0.7f)));
		float xSize = 60f * scaleMult;
		float ySize = 85f * scaleMult;
		float timeToWait = maxTime / 10f / (float)powerups.Count;
		int num3;
		for (int i = 0; i < powerups.Count; i = num3 + 1)
		{
			PowerupScript powerupScript = powerups[i];
			CharmUiRenderer charmUiRenderer = CharmUiRenderer.PoolSpawn(powerupScript.identifier, CameraUiGlobal.instance.gameObject.layer, this.charmsArea, true, scaleMult = scaleMult, 0.5f, true, drawersPowerups.Contains(powerupScript), null, null);
			float num = (float)(-(float)Mathf.Min(powerups.Count - 1, 4)) * xSize * 0.5f;
			float num2 = (float)Mathf.Min(Mathf.Max(0, powerups.Count / 5 - 1), 4) * ySize * 0.5f - 30f;
			charmUiRenderer.transform.localPosition = new global::UnityEngine.Vector3(num + (float)(i % 5) * xSize, num2 - (float)(i / 5) * ySize, 0f);
			charmUiRenderer.transform.localScale = global::UnityEngine.Vector3.one * 60f * 3f;
			charmUiRenderer.transform.eulerAngles = global::UnityEngine.Vector3.zero;
			while (timer < timeToWait)
			{
				timer += Tick.Time;
				yield return null;
			}
			timer -= timeToWait;
			num3 = i;
		}
		yield break;
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x0000FCA7 File Offset: 0x0000DEA7
	private IEnumerator ShowAbilities(float maxTime)
	{
		float timer = 0f;
		List<AbilityScript.Identifier> abilities = new List<AbilityScript.Identifier>();
		for (int j = 0; j < GameplayData.Instance.phoneAbilitiesPickHistory.Count; j++)
		{
			if (GameplayData.Instance.phoneAbilitiesPickHistory[j] != AbilityScript.Identifier.undefined && GameplayData.Instance.phoneAbilitiesPickHistory[j] != AbilityScript.Identifier.count)
			{
				abilities.Add(GameplayData.Instance.phoneAbilitiesPickHistory[j]);
			}
		}
		if (abilities.Count == 0)
		{
			this.noPhoneText.alpha = 1f;
			yield break;
		}
		if (abilities.Count > 20)
		{
			this.phoneAbilitiesArea.anchoredPosition -= new global::UnityEngine.Vector2(0f, 30f);
		}
		else if (abilities.Count <= 15)
		{
			this.phoneAbilitiesArea.anchoredPosition += new global::UnityEngine.Vector2(0f, 30f);
		}
		base.StartCoroutine(this.AbilityImagesAnimationCoroutine());
		float scaleMult = ((abilities.Count < 3) ? 1f : ((abilities.Count < 20) ? 0.75f : ((abilities.Count < 40) ? 0.5f : 0.25f)));
		float xSize = 60f * scaleMult;
		float ySize = 60f * scaleMult;
		float timeToWait = maxTime / 10f / (float)abilities.Count;
		int num3;
		for (int i = 0; i < abilities.Count; i = num3 + 1)
		{
			AbilityScript abilityScript = AbilityScript.AbilityGet(abilities[i]);
			Image component = global::UnityEngine.Object.Instantiate<GameObject>(this.phoneAbilityTemplate.gameObject, this.phoneAbilitiesArea).GetComponent<Image>();
			component.sprite = abilityScript.SpriteGet();
			component.gameObject.SetActive(true);
			StatsScript.AbilityImageAnimation abilityImageAnimation = default(StatsScript.AbilityImageAnimation);
			abilityImageAnimation.image = component;
			abilityImageAnimation.speed = 1f;
			this.abilityImagesAngularSpeed.Add(abilityImageAnimation);
			float num = (float)(-(float)Mathf.Min(abilities.Count - 1, 4)) * xSize * 0.5f;
			float num2 = (float)Mathf.Min(Mathf.Max(0, abilities.Count / 5 - 1), 4) * ySize * 0.5f;
			component.transform.localPosition = new global::UnityEngine.Vector3(num + (float)(i % 5) * xSize, num2 - (float)(i / 5) * ySize, 0f);
			component.transform.localScale = global::UnityEngine.Vector3.one * scaleMult;
			component.transform.eulerAngles = global::UnityEngine.Vector3.zero;
			while (timer < timeToWait)
			{
				timer += Tick.Time;
				yield return null;
			}
			timer -= timeToWait;
			num3 = i;
		}
		yield break;
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x0000FCBD File Offset: 0x0000DEBD
	private IEnumerator AbilityImagesAnimationCoroutine()
	{
		float timer = 2f;
		while (timer > 0f)
		{
			for (int i = 0; i < this.abilityImagesAngularSpeed.Count; i++)
			{
				StatsScript.AbilityImageAnimation abilityImageAnimation = this.abilityImagesAngularSpeed[i];
				if (abilityImageAnimation.speed > 0f)
				{
					abilityImageAnimation.speed -= Tick.Time * 2f;
					abilityImageAnimation.speed = Mathf.Max(abilityImageAnimation.speed, 0f);
					abilityImageAnimation.image.rectTransform.SetLocalZAngle(10f * abilityImageAnimation.speed * Util.AngleSin(abilityImageAnimation.speed * 720f));
					this.abilityImagesAngularSpeed[i] = abilityImageAnimation;
				}
			}
			timer -= Tick.Time;
			yield return null;
		}
		this.abilityImagesAngularSpeed.Clear();
		yield break;
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x0000FCCC File Offset: 0x0000DECC
	public static void Close()
	{
		if (!StatsScript.IsEnabled())
		{
			return;
		}
		StatsScript.instance.statsHolder.SetActive(false);
	}

	// Token: 0x06000BE6 RID: 3046 RVA: 0x0000FCE6 File Offset: 0x0000DEE6
	private void OnPromptChange(Controls.InputActionMap map)
	{
		this.UpdateTextString();
	}

	// Token: 0x06000BE7 RID: 3047 RVA: 0x0000FCEE File Offset: 0x0000DEEE
	private void Awake()
	{
		StatsScript.instance = this;
	}

	// Token: 0x06000BE8 RID: 3048 RVA: 0x0005FBF0 File Offset: 0x0005DDF0
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		this.statsHolder.SetActive(false);
		this.phoneAbilityTemplate.gameObject.SetActive(false);
		Controls.onLastInputKindChangedAny = (Controls.MapCallback)Delegate.Combine(Controls.onLastInputKindChangedAny, new Controls.MapCallback(this.OnPromptChange));
		this.tooltipTextAnimator.gameObject.SetActive(false);
	}

	// Token: 0x04000C95 RID: 3221
	private const int PLAYER_INDEX = 0;

	// Token: 0x04000C96 RID: 3222
	public static StatsScript instance;

	// Token: 0x04000C97 RID: 3223
	private const float SHOW_SPEED = 10f;

	// Token: 0x04000C98 RID: 3224
	private static Color C_ORANGE = new Color(1f, 0.5f, 0f, 1f);

	// Token: 0x04000C99 RID: 3225
	private Controls.PlayerExt player;

	// Token: 0x04000C9A RID: 3226
	public GameObject statsHolder;

	// Token: 0x04000C9B RID: 3227
	public Transform statsScaler;

	// Token: 0x04000C9C RID: 3228
	public TextAnimator textAnimator_StatsText;

	// Token: 0x04000C9D RID: 3229
	public TextAnimator textAnimator_CharmsTitle;

	// Token: 0x04000C9E RID: 3230
	public TextAnimator textAnimator_PhoneTitle;

	// Token: 0x04000C9F RID: 3231
	public TextAnimator textAnimator_Prompt;

	// Token: 0x04000CA0 RID: 3232
	public RectTransform charmsArea;

	// Token: 0x04000CA1 RID: 3233
	public RectTransform phoneAbilitiesArea;

	// Token: 0x04000CA2 RID: 3234
	public Image phoneAbilityTemplate;

	// Token: 0x04000CA3 RID: 3235
	public TextMeshProUGUI noCharmsText;

	// Token: 0x04000CA4 RID: 3236
	public TextMeshProUGUI noPhoneText;

	// Token: 0x04000CA5 RID: 3237
	public TextAnimator tooltipTextAnimator;

	// Token: 0x04000CA6 RID: 3238
	private static StatsScript.ShowKind myShowKind = StatsScript.ShowKind.endDeath;

	// Token: 0x04000CA7 RID: 3239
	private bool tooltipsTextUpdated;

	// Token: 0x04000CA8 RID: 3240
	private StringBuilder sb = new StringBuilder(256);

	// Token: 0x04000CA9 RID: 3241
	private bool charmsAndAbilitiesTime;

	// Token: 0x04000CAA RID: 3242
	private bool promptTime;

	// Token: 0x04000CAB RID: 3243
	private List<StatsScript.AbilityImageAnimation> abilityImagesAngularSpeed = new List<StatsScript.AbilityImageAnimation>(20);

	// Token: 0x020000EF RID: 239
	public enum ShowKind
	{
		// Token: 0x04000CAD RID: 3245
		endDeath,
		// Token: 0x04000CAE RID: 3246
		endAlive
	}

	// Token: 0x020000F0 RID: 240
	private struct AbilityImageAnimation
	{
		// Token: 0x04000CAF RID: 3247
		public Image image;

		// Token: 0x04000CB0 RID: 3248
		public float speed;
	}
}
