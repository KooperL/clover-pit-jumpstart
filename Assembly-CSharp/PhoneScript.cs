using System;
using System.Collections;
using System.Numerics;
using Panik;
using UnityEngine;

public class PhoneScript : MonoBehaviour
{
	// Token: 0x06000803 RID: 2051 RVA: 0x00033690 File Offset: 0x00031890
	public static PhoneScript.State StateGet()
	{
		return PhoneScript.instance.state;
	}

	// Token: 0x06000804 RID: 2052 RVA: 0x0003369C File Offset: 0x0003189C
	public static void StateSet(PhoneScript.State newState)
	{
		if (PhoneScript.instance == null)
		{
			return;
		}
		bool flag = PhoneScript.IsOn();
		switch (PhoneScript.instance.state)
		{
		default:
			switch (newState)
			{
			default:
			{
				PhoneScript.instance.state = newState;
				bool flag2 = PhoneScript.IsOn();
				if (!flag && flag2)
				{
					PhoneUiScript.Open();
					Sound.Play3D("SoundPhonePickup", PhoneScript.instance.transform.position, 20f, 1f, 1f, 1);
					return;
				}
				if (flag && !flag2)
				{
					Sound.Play3D("SoundPhonePutDown", PhoneScript.instance.transform.position, 20f, 1f, 1f, 1);
				}
				return;
			}
			}
			break;
		}
	}

	// Token: 0x06000805 RID: 2053 RVA: 0x0003377D File Offset: 0x0003197D
	public static bool IsOn()
	{
		return !(PhoneScript.instance == null) && (PhoneScript.instance.state == PhoneScript.State.onIntro || PhoneScript.instance.state == PhoneScript.State.onChoosing || PhoneScript.instance.state == PhoneScript.State.onFinalization);
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x000337B7 File Offset: 0x000319B7
	public static bool HasNoDialogue()
	{
		return PhoneScript.instance == null || GameplayData.Instance == null || GameplayData.Instance._phone_abilityAlreadyPickedUp;
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x000337DC File Offset: 0x000319DC
	public static void ResetForDeadline()
	{
		if (PhoneScript.instance == null)
		{
			return;
		}
		GameplayData gameplayData = GameplayData.Instance;
		if (gameplayData == null)
		{
			return;
		}
		AbilityScript.Category phone_lastAbilityCategory = gameplayData._phone_lastAbilityCategory;
		if (!gameplayData._phone_abilityAlreadyPickedUp)
		{
			gameplayData.phoneEasyCounter_SkippedCalls_Total++;
			switch (phone_lastAbilityCategory)
			{
			default:
				gameplayData.phoneEasyCounter_SkippedCalls_Normal++;
				break;
			case AbilityScript.Category.evil:
				gameplayData.phoneEasyCounter_SkippedCalls_Evil++;
				break;
			case AbilityScript.Category.good:
				gameplayData.phoneEasyCounter_SkippedCalls_Good++;
				break;
			}
			if (gameplayData._phone_pickedUpOnceLastDeadline)
			{
				PowerupScript.Unlock(PowerupScript.Identifier.ChastityBelt);
			}
		}
		PhoneScript.instance.state = PhoneScript.State.offNothing;
		gameplayData._phone_pickedUpOnceLastDeadline = false;
		gameplayData._phone_abilityAlreadyPickedUp = false;
		gameplayData._phone_lastAbilityCategory = AbilityScript.Category.normal;
		GameplayData.PhoneRerollCostReset(true);
		GameplayData.PhoneRerollPerformed_PerDeadline = 0L;
		if (GameplayData.DebtIndexGet() == GameplayData.SixSixSix_GetMinimumDebtIndex())
		{
			gameplayData._phone_bookSpecialCall = true;
		}
		FloppySlotScript.SixSixSixTextureUpdateToIgnoredCallsLevel(true);
	}

	// Token: 0x06000808 RID: 2056 RVA: 0x000338BC File Offset: 0x00031ABC
	public static void PhoneRing()
	{
		if (PhoneScript.instance == null)
		{
			return;
		}
		if (PhoneScript.instance.phoneRingCoroutine != null)
		{
			PhoneScript.instance.StopCoroutine(PhoneScript.instance.phoneRingCoroutine);
		}
		PhoneScript.instance.phoneRingCoroutine = PhoneScript.instance.StartCoroutine(PhoneScript.instance.PhoneRingCoroutine());
	}

	// Token: 0x06000809 RID: 2057 RVA: 0x00033915 File Offset: 0x00031B15
	public IEnumerator PhoneRingCoroutine()
	{
		while (PowerupTriggerAnimController.HasAnimations())
		{
			yield return null;
		}
		while (GameplayMaster.unlockPowerupFirstTimeDialogueBooked)
		{
			yield return null;
		}
		while (DialogueScript.IsEnabled())
		{
			yield return null;
		}
		float timer = 0.5f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			yield return null;
		}
		BigInteger bigInteger = GameplayData.DebtIndexGet();
		long num = bigInteger.CastToLong();
		if (bigInteger > 9223372036854775807L)
		{
			num = long.MaxValue;
		}
		float num2 = 0.75f - Mathf.Max((float)num - 1f, 0f) / 10f;
		num2 = Mathf.Max(0.4f, num2);
		Sound.Play3D("SoundPhoneRing", base.transform.position, 20f, num2, 1f, 1);
		timer = 2f;
		while (timer > 0f)
		{
			timer -= Tick.Time;
			if (!ScreenMenuScript.IsEnabled())
			{
				CameraGame.Shake(timer / 2f * 2f);
			}
			if (!ScreenMenuScript.IsEnabled())
			{
				Controls.VibrationSet(this.player, timer / 2f * 1f);
			}
			yield return null;
		}
		this.phoneRingCoroutine = null;
		yield break;
	}

	// Token: 0x0600080A RID: 2058 RVA: 0x00033924 File Offset: 0x00031B24
	public static bool IsPhoneRinging()
	{
		return !(PhoneScript.instance == null) && PhoneScript.instance.phoneRingCoroutine != null;
	}

	// Token: 0x0600080B RID: 2059 RVA: 0x00033942 File Offset: 0x00031B42
	public void Sound_PhoneMemo()
	{
		Sound.Play3D("SoundPhoneMemo", base.transform.position, 5f, 1f, 1f, 1);
	}

	// Token: 0x0600080C RID: 2060 RVA: 0x0003396A File Offset: 0x00031B6A
	private void Awake()
	{
		PhoneScript.instance = this;
		this.animator = base.GetComponentInChildren<Animator>();
		this.myOutline = base.GetComponentInChildren<Outline>();
	}

	// Token: 0x0600080D RID: 2061 RVA: 0x0003398A File Offset: 0x00031B8A
	private void OnDestroy()
	{
		if (PhoneScript.instance == this)
		{
			PhoneScript.instance = null;
		}
	}

	// Token: 0x0600080E RID: 2062 RVA: 0x000339A0 File Offset: 0x00031BA0
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		this.holderWithoutCornetta.SetActive(false);
		this.effectsHolder999.SetActive(false);
		this.lightSpriteColor = this.lightSprite.color;
		this.lightSprite.enabled = false;
	}

	// Token: 0x0600080F RID: 2063 RVA: 0x000339F0 File Offset: 0x00031BF0
	private void Update()
	{
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		bool flag = PhoneScript.IsOn();
		int gamePhase = (int)GameplayMaster.GetGamePhase();
		GameplayData gameplayData = GameplayData.Instance;
		bool flag2 = gamePhase != 0;
		if (this.mesheHolder.activeSelf != flag2)
		{
			this.mesheHolder.SetActive(flag2);
		}
		if (this.holderWithCornetta.activeSelf == flag)
		{
			this.holderWithCornetta.SetActive(!flag);
			this.holderWithoutCornetta.SetActive(flag);
		}
		if (this.holderWithoutCornetta.activeSelf == !flag)
		{
			this.holderWithCornetta.SetActive(flag);
			this.holderWithoutCornetta.SetActive(!flag);
		}
		int num = 0;
		if (!gameplayData._phone_abilityAlreadyPickedUp)
		{
			if (!gameplayData._phone_pickedUpOnceLastDeadline)
			{
				num = 1;
			}
			else
			{
				num = 2;
			}
		}
		this.animator.SetInteger("AnimationIndex", num);
		bool flag3 = num == 2 || num == 1;
		if (this.lightSprite.enabled != flag3)
		{
			this.lightSprite.enabled = flag3;
		}
		if (this.lightSprite.enabled)
		{
			this.lightSpriteColor.a = 0.015f + Util.AngleSin(Tick.PassedTime * 360f) * 0.015f;
			this.lightSprite.color = this.lightSpriteColor;
		}
		if (GameplayData.NineNineNine_IsTime())
		{
			if (this.meshRendererWithoutCornetta.sharedMaterial != this.material_999)
			{
				this.meshRendererWithCornetta.sharedMaterial = this.material_999;
				this.meshRendererWithoutCornetta.sharedMaterial = this.material_999;
			}
			bool flag4 = !this.myOutline.enabled;
			if (this.effectsHolder999.activeSelf != flag4)
			{
				this.effectsHolder999.SetActive(flag4);
			}
		}
	}

	public static PhoneScript instance;

	private const int PLAYER_INDEX = 0;

	private const int ANIMATION_NOTHING = 0;

	private const int ANIMATION_RINGING = 1;

	private const int ANIMATION_MEMO = 2;

	private Controls.PlayerExt player;

	private Animator animator;

	public GameObject mesheHolder;

	public GameObject holderWithCornetta;

	public GameObject holderWithoutCornetta;

	public SkinnedMeshRenderer meshRendererWithCornetta;

	public MeshRenderer meshRendererWithoutCornetta;

	public Material material_999;

	public GameObject effectsHolder999;

	public SpriteRenderer lightSprite;

	private Outline myOutline;

	private PhoneScript.State state;

	private Color lightSpriteColor;

	private Coroutine phoneRingCoroutine;

	public enum State
	{
		offNothing,
		offRinging,
		offMemo,
		onIntro,
		onChoosing,
		onFinalization
	}
}
