using System;
using System.Collections;
using System.Numerics;
using Panik;
using UnityEngine;

// Token: 0x0200009A RID: 154
public class PhoneScript : MonoBehaviour
{
	// Token: 0x06000915 RID: 2325 RVA: 0x0000D2D1 File Offset: 0x0000B4D1
	public static PhoneScript.State StateGet()
	{
		return PhoneScript.instance.state;
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x0004A870 File Offset: 0x00048A70
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
					Sound.Play3D("SoundPhonePickup", PhoneScript.instance.transform.position, 20f, 1f, 1f, AudioRolloffMode.Linear);
					return;
				}
				if (flag && !flag2)
				{
					Sound.Play3D("SoundPhonePutDown", PhoneScript.instance.transform.position, 20f, 1f, 1f, AudioRolloffMode.Linear);
				}
				return;
			}
			}
			break;
		}
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x0000D2DD File Offset: 0x0000B4DD
	public static bool IsOn()
	{
		return !(PhoneScript.instance == null) && (PhoneScript.instance.state == PhoneScript.State.onIntro || PhoneScript.instance.state == PhoneScript.State.onChoosing || PhoneScript.instance.state == PhoneScript.State.onFinalization);
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x0000D317 File Offset: 0x0000B517
	public static bool HasNoDialogue()
	{
		return PhoneScript.instance == null || GameplayData.Instance == null || GameplayData.Instance._phone_abilityAlreadyPickedUp;
	}

	// Token: 0x06000919 RID: 2329 RVA: 0x0004A954 File Offset: 0x00048B54
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

	// Token: 0x0600091A RID: 2330 RVA: 0x0004AA34 File Offset: 0x00048C34
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

	// Token: 0x0600091B RID: 2331 RVA: 0x0000D33B File Offset: 0x0000B53B
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
		Sound.Play3D("SoundPhoneRing", base.transform.position, 20f, num2, 1f, AudioRolloffMode.Linear);
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

	// Token: 0x0600091C RID: 2332 RVA: 0x0000D34A File Offset: 0x0000B54A
	public static bool IsPhoneRinging()
	{
		return !(PhoneScript.instance == null) && PhoneScript.instance.phoneRingCoroutine != null;
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x0000D368 File Offset: 0x0000B568
	public void Sound_PhoneMemo()
	{
		Sound.Play3D("SoundPhoneMemo", base.transform.position, 5f, 1f, 1f, AudioRolloffMode.Linear);
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x0000D390 File Offset: 0x0000B590
	private void Awake()
	{
		PhoneScript.instance = this;
		this.animator = base.GetComponentInChildren<Animator>();
		this.myOutline = base.GetComponentInChildren<Outline>();
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x0000D3B0 File Offset: 0x0000B5B0
	private void OnDestroy()
	{
		if (PhoneScript.instance == this)
		{
			PhoneScript.instance = null;
		}
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x0004AA90 File Offset: 0x00048C90
	private void Start()
	{
		this.player = Controls.GetPlayerByIndex(0);
		this.holderWithoutCornetta.SetActive(false);
		this.effectsHolder999.SetActive(false);
		this.lightSpriteColor = this.lightSprite.color;
		this.lightSprite.enabled = false;
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x0004AAE0 File Offset: 0x00048CE0
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

	// Token: 0x040008C6 RID: 2246
	public static PhoneScript instance;

	// Token: 0x040008C7 RID: 2247
	private const int PLAYER_INDEX = 0;

	// Token: 0x040008C8 RID: 2248
	private const int ANIMATION_NOTHING = 0;

	// Token: 0x040008C9 RID: 2249
	private const int ANIMATION_RINGING = 1;

	// Token: 0x040008CA RID: 2250
	private const int ANIMATION_MEMO = 2;

	// Token: 0x040008CB RID: 2251
	private Controls.PlayerExt player;

	// Token: 0x040008CC RID: 2252
	private Animator animator;

	// Token: 0x040008CD RID: 2253
	public GameObject mesheHolder;

	// Token: 0x040008CE RID: 2254
	public GameObject holderWithCornetta;

	// Token: 0x040008CF RID: 2255
	public GameObject holderWithoutCornetta;

	// Token: 0x040008D0 RID: 2256
	public SkinnedMeshRenderer meshRendererWithCornetta;

	// Token: 0x040008D1 RID: 2257
	public MeshRenderer meshRendererWithoutCornetta;

	// Token: 0x040008D2 RID: 2258
	public Material material_999;

	// Token: 0x040008D3 RID: 2259
	public GameObject effectsHolder999;

	// Token: 0x040008D4 RID: 2260
	public SpriteRenderer lightSprite;

	// Token: 0x040008D5 RID: 2261
	private Outline myOutline;

	// Token: 0x040008D6 RID: 2262
	private PhoneScript.State state;

	// Token: 0x040008D7 RID: 2263
	private Color lightSpriteColor;

	// Token: 0x040008D8 RID: 2264
	private Coroutine phoneRingCoroutine;

	// Token: 0x0200009B RID: 155
	public enum State
	{
		// Token: 0x040008DA RID: 2266
		offNothing,
		// Token: 0x040008DB RID: 2267
		offRinging,
		// Token: 0x040008DC RID: 2268
		offMemo,
		// Token: 0x040008DD RID: 2269
		onIntro,
		// Token: 0x040008DE RID: 2270
		onChoosing,
		// Token: 0x040008DF RID: 2271
		onFinalization
	}
}
