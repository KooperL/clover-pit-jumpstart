using System;
using System.Collections.Generic;
using System.Numerics;
using Panik;
using UnityEngine;

public class SymbolScript : MonoBehaviour
{
	// Token: 0x06000782 RID: 1922 RVA: 0x0003147F File Offset: 0x0002F67F
	public static string GetPrefabName(SymbolScript.Kind kind)
	{
		if (kind == SymbolScript.Kind.undefined)
		{
			Debug.LogError("Cannot get Symbol's prefab. Symbol kind is undefined.");
			return null;
		}
		if (kind == SymbolScript.Kind.count)
		{
			Debug.LogError("Cannot get Symbol's prefab. Symbol kind is count.");
			return null;
		}
		return SymbolScript.prefabDict[kind];
	}

	// Token: 0x06000783 RID: 1923 RVA: 0x000314AD File Offset: 0x0002F6AD
	public static GameObject GetPrefab(SymbolScript.Kind kind)
	{
		return AssetMaster.GetPrefab(SymbolScript.GetPrefabName(kind));
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x000314BC File Offset: 0x0002F6BC
	public static BigInteger ModifierInstantReward_GetAmmount()
	{
		if (GameplayData.Instance == null)
		{
			return 1;
		}
		BigInteger bigInteger = GameplayData.InterestEarnedHypotetically() / 2;
		if (bigInteger < 1L)
		{
			bigInteger = 1;
		}
		return bigInteger;
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x000314FA File Offset: 0x0002F6FA
	public void MarkAsScoringSymbol(int columnX, int lineY)
	{
		if (this._isScoringSymbol)
		{
			return;
		}
		this._isScoringSymbol = true;
		this.scoringSymbol_X = columnX;
		this.scoringSymbol_Y = lineY;
		SymbolScript.scoringSymbols.Add(this);
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x00031525 File Offset: 0x0002F725
	public void RemoveFromScoringSymbols()
	{
		SymbolScript.scoringSymbols.Remove(this);
		this._isScoringSymbol = false;
		this.scoringSymbol_X = -1;
		this.scoringSymbol_Y = -1;
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x00031548 File Offset: 0x0002F748
	public void ModifierSet(SymbolScript.Modifier _modifier)
	{
		this.modifier = _modifier;
		switch (this.modifier)
		{
		case SymbolScript.Modifier.none:
			this.mainSkinnedMeshRenderer.sharedMaterial = this.matDefault;
			this.modifierHolder_InstantReward.SetActive(false);
			this.modifierHolder_Ticket.SetActive(false);
			this.modifierHolder_Golden.SetActive(false);
			this.modifierHolder_Repetition.SetActive(false);
			this.modifierHolder_Battery.SetActive(false);
			this.modifierHolder_Chain.SetActive(false);
			return;
		case SymbolScript.Modifier.instantReward:
			this.mainSkinnedMeshRenderer.sharedMaterial = this.matDefault;
			this.modifierHolder_InstantReward.SetActive(true);
			this.modifierHolder_Ticket.SetActive(false);
			this.modifierHolder_Golden.SetActive(false);
			this.modifierHolder_Repetition.SetActive(false);
			this.modifierHolder_Battery.SetActive(false);
			this.modifierHolder_Chain.SetActive(false);
			return;
		case SymbolScript.Modifier.cloverTicket:
			this.mainSkinnedMeshRenderer.sharedMaterial = this.matDefault;
			this.modifierHolder_InstantReward.SetActive(false);
			this.modifierHolder_Ticket.SetActive(true);
			this.modifierHolder_Golden.SetActive(false);
			this.modifierHolder_Repetition.SetActive(false);
			this.modifierHolder_Battery.SetActive(false);
			this.modifierHolder_Chain.SetActive(false);
			return;
		case SymbolScript.Modifier.golden:
			this.mainSkinnedMeshRenderer.sharedMaterial = Colors.GetMaterial_GoldenSymbolUnpausable();
			this.modifierHolder_InstantReward.SetActive(false);
			this.modifierHolder_Ticket.SetActive(false);
			this.modifierHolder_Golden.SetActive(true);
			this.modifierHolder_Repetition.SetActive(false);
			this.modifierHolder_Battery.SetActive(false);
			this.modifierHolder_Chain.SetActive(false);
			return;
		case SymbolScript.Modifier.repetition:
			this.mainSkinnedMeshRenderer.sharedMaterial = this.matDefault;
			this.modifierHolder_InstantReward.SetActive(false);
			this.modifierHolder_Ticket.SetActive(false);
			this.modifierHolder_Golden.SetActive(false);
			this.modifierHolder_Repetition.SetActive(true);
			this.modifierHolder_Battery.SetActive(false);
			this.modifierHolder_Chain.SetActive(false);
			return;
		case SymbolScript.Modifier.battery:
			this.mainSkinnedMeshRenderer.sharedMaterial = this.matDefault;
			this.modifierHolder_InstantReward.SetActive(false);
			this.modifierHolder_Ticket.SetActive(false);
			this.modifierHolder_Golden.SetActive(false);
			this.modifierHolder_Repetition.SetActive(false);
			this.modifierHolder_Battery.SetActive(true);
			this.modifierHolder_Chain.SetActive(false);
			return;
		case SymbolScript.Modifier.chain:
			this.mainSkinnedMeshRenderer.sharedMaterial = this.matDefault;
			this.modifierHolder_InstantReward.SetActive(false);
			this.modifierHolder_Ticket.SetActive(false);
			this.modifierHolder_Golden.SetActive(false);
			this.modifierHolder_Repetition.SetActive(false);
			this.modifierHolder_Battery.SetActive(false);
			this.modifierHolder_Chain.SetActive(true);
			return;
		default:
			return;
		}
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x000317FA File Offset: 0x0002F9FA
	public SymbolScript.Modifier ModifierGet()
	{
		return this.modifier;
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x00031804 File Offset: 0x0002FA04
	public static int ModifierGetArrayIndex(SymbolScript.Modifier modifier)
	{
		switch (modifier)
		{
		case SymbolScript.Modifier.none:
			return -1;
		case SymbolScript.Modifier.instantReward:
			return 0;
		case SymbolScript.Modifier.cloverTicket:
			return 1;
		case SymbolScript.Modifier.golden:
			return 2;
		case SymbolScript.Modifier.repetition:
			return 3;
		case SymbolScript.Modifier.battery:
			return 4;
		case SymbolScript.Modifier.chain:
			return 5;
		default:
			Debug.LogError("SymbolScript.ModifierGetArrayIndex(): index not handled for modifier: " + modifier.ToString());
			return -1;
		}
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x00031860 File Offset: 0x0002FA60
	public static SymbolScript.Modifier ModifierFromArrayIndex(int index)
	{
		switch (index)
		{
		case -1:
			return SymbolScript.Modifier.none;
		case 0:
			return SymbolScript.Modifier.instantReward;
		case 1:
			return SymbolScript.Modifier.cloverTicket;
		case 2:
			return SymbolScript.Modifier.golden;
		case 3:
			return SymbolScript.Modifier.repetition;
		case 4:
			return SymbolScript.Modifier.battery;
		case 5:
			return SymbolScript.Modifier.chain;
		default:
			Debug.LogError("SymbolScript.ModifierFromArrayIndex(): modifier not handled for index: " + index.ToString());
			return SymbolScript.Modifier.none;
		}
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x000318B8 File Offset: 0x0002FAB8
	public static SymbolScript GetSymbolScript_ByScoringPosition(Vector2Int columnXLineY)
	{
		foreach (SymbolScript symbolScript in SymbolScript.scoringSymbols)
		{
			if (symbolScript.scoringSymbol_X == columnXLineY.x && symbolScript.scoringSymbol_Y == columnXLineY.y)
			{
				return symbolScript;
			}
		}
		return null;
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00031928 File Offset: 0x0002FB28
	public void MaterialUpdate()
	{
		this.mainSkinnedMeshRenderer.sharedMaterial = this.matDefault;
		if (this.kind == SymbolScript.Kind.six && GameplayData.DebtIndexGet() >= GameplayData.SuperSixSixSix_GetMinimumDebtIndex())
		{
			this.mainSkinnedMeshRenderer.sharedMaterial = this.matAlternative;
		}
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x00031968 File Offset: 0x0002FB68
	public static ulong SymbolsOrderWeightMask(SymbolScript.Kind kind)
	{
		ulong num = 1UL;
		switch (kind)
		{
		case SymbolScript.Kind.lemon:
			num <<= 1;
			break;
		case SymbolScript.Kind.cherry:
			num <<= 2;
			break;
		case SymbolScript.Kind.clover:
			num <<= 3;
			break;
		case SymbolScript.Kind.bell:
			num <<= 4;
			break;
		case SymbolScript.Kind.diamond:
			num <<= 5;
			break;
		case SymbolScript.Kind.coins:
			num <<= 6;
			break;
		case SymbolScript.Kind.seven:
			num <<= 7;
			break;
		case SymbolScript.Kind.six:
		case SymbolScript.Kind.nine:
			num = 0UL;
			break;
		default:
			Debug.LogError("SymbolScript.SymbolsOrderWeightMask(): kind not handled: " + kind.ToString());
			num = 0UL;
			break;
		}
		return num;
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x000319F4 File Offset: 0x0002FBF4
	public static bool IsYellow(SymbolScript.Kind kind)
	{
		switch (kind)
		{
		case SymbolScript.Kind.lemon:
			return true;
		case SymbolScript.Kind.cherry:
			return false;
		case SymbolScript.Kind.clover:
			return false;
		case SymbolScript.Kind.bell:
			return true;
		case SymbolScript.Kind.diamond:
			return false;
		case SymbolScript.Kind.coins:
			return true;
		case SymbolScript.Kind.seven:
			return true;
		case SymbolScript.Kind.six:
			return false;
		case SymbolScript.Kind.nine:
			return false;
		default:
			Debug.LogError("IsYellow() error. Kind not handled: " + kind.ToString());
			return false;
		}
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x00031A5C File Offset: 0x0002FC5C
	public static bool SymbolCanRepeatTrigger(SymbolScript.Kind kind)
	{
		if (kind <= SymbolScript.Kind.seven)
		{
			return true;
		}
		if (kind - SymbolScript.Kind.six > 1)
		{
			Debug.LogError("SymbolScript.SymbolCanRepeatTrigger(): symbol not handled: " + kind.ToString());
			return false;
		}
		return false;
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x00031A8B File Offset: 0x0002FC8B
	public bool IsAnimationPlaying()
	{
		return this.animationTimer > 0f;
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x00031A9A File Offset: 0x0002FC9A
	public void PlayAnimation(float speed)
	{
		this.animationTimer = 0.833f;
		this.animator.SetTrigger("playAnim");
		this.animator.speed = speed;
		this.outlineAnimator.SetTrigger("playAnim");
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00031AD4 File Offset: 0x0002FCD4
	public void SpinScalingSet(bool state)
	{
		if (state != this.spinScaling)
		{
			this.spinScaling = state;
			if (!this.spinScaling)
			{
				base.transform.localScale = global::UnityEngine.Vector3.one;
				return;
			}
			this.spinScalingOldY = base.transform.position.y;
		}
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x00031B20 File Offset: 0x0002FD20
	private void Awake()
	{
		this.animator = base.GetComponentInChildren<Animator>();
		this.mainSkinnedMeshRenderer = base.GetComponentInChildren<SkinnedMeshRenderer>();
		this.matDefault = this.mainSkinnedMeshRenderer.sharedMaterial;
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.animator.gameObject);
		gameObject.transform.SetParent(this.animator.transform.parent);
		gameObject.transform.localPosition = this.animator.transform.localPosition + new global::UnityEngine.Vector3(0f, 0f, 2.5f);
		gameObject.transform.localEulerAngles = this.animator.transform.eulerAngles;
		gameObject.transform.localScale = this.animator.transform.localScale;
		this.outlineAnimator = gameObject.GetComponent<Animator>();
		this.outlineSkinnedMeshrenderer = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
		this.outlineSkinnedMeshrenderer.sharedMaterial = Colors.GetMaterial_RainbowPausable();
		if (this.kind == SymbolScript.Kind.undefined)
		{
			Debug.LogError("Symbol kind is undefined. GameObject: " + base.gameObject.name);
		}
		base.gameObject.layer = this.layer;
		Transform[] componentsInChildren = base.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = this.layer;
		}
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x00031C71 File Offset: 0x0002FE71
	private void OnEnable()
	{
		SymbolScript.allEnabled.Add(this);
		this.ModifierSet(SymbolScript.Modifier.none);
		base.transform.localScale = global::UnityEngine.Vector3.one;
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x00031C95 File Offset: 0x0002FE95
	private void OnDisable()
	{
		SymbolScript.allEnabled.Remove(this);
		this.RemoveFromScoringSymbols();
		this.SpinScalingSet(false);
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x00031CB0 File Offset: 0x0002FEB0
	private void Update()
	{
		if (!Tick.IsGameRunning)
		{
			return;
		}
		bool flag = this.IsAnimationPlaying();
		if (this.outlineAnimator.gameObject.activeSelf != flag)
		{
			this.outlineAnimator.gameObject.SetActive(flag);
		}
		if (flag)
		{
			global::UnityEngine.Vector2 vector;
			vector.x = Util.AngleCos(this.animationTimer * 720f) * 0.025f;
			vector.y = Util.AngleSin(this.animationTimer * 720f) * 0.025f;
			this.outlineAnimator.transform.SetLocalX(vector.x);
			this.outlineAnimator.transform.SetLocalY(vector.y);
		}
		this.animationTimer -= Tick.Time;
		if (this.modifier == SymbolScript.Modifier.golden)
		{
			Material material = ((Util.AngleSin(Tick.PassedTimePausable * 720f) > 0f) ? Colors.GetMaterial_GoldenSymbolUnpausable() : this.matDefault);
			if (this.mainSkinnedMeshRenderer.sharedMaterial != material)
			{
				this.mainSkinnedMeshRenderer.sharedMaterial = material;
			}
		}
		if (this.spinScaling)
		{
			float num = base.transform.position.y - this.spinScalingOldY;
			this.spinScalingOldY = base.transform.position.y;
			global::UnityEngine.Vector2 vector2 = base.transform.localScale;
			vector2.x = Mathf.Clamp(1f + 0.005f * num / Tick.Time, 0.8f, 1.2f);
			vector2.y = Mathf.Clamp(1f + 0.005f * -num / Tick.Time, 0.8f, 1.4000001f);
			base.transform.SetLocalXScale(vector2.x);
			base.transform.SetLocalYScale(vector2.y);
		}
	}

	public static List<SymbolScript> allEnabled = new List<SymbolScript>();

	public static List<SymbolScript> scoringSymbols = new List<SymbolScript>();

	public static Dictionary<SymbolScript.Kind, string> prefabDict = new Dictionary<SymbolScript.Kind, string>
	{
		{
			SymbolScript.Kind.lemon,
			"Symbol Lemon"
		},
		{
			SymbolScript.Kind.cherry,
			"Symbol Ciliegia"
		},
		{
			SymbolScript.Kind.clover,
			"Symbol Clover Quadrifoglio"
		},
		{
			SymbolScript.Kind.bell,
			"Symbol Bell"
		},
		{
			SymbolScript.Kind.seven,
			"Symbol 7"
		},
		{
			SymbolScript.Kind.diamond,
			"Symbol Diamond"
		},
		{
			SymbolScript.Kind.coins,
			"Symbol Coin Pile"
		},
		{
			SymbolScript.Kind.six,
			"Symbol 6"
		},
		{
			SymbolScript.Kind.nine,
			"Symbol 9"
		}
	};

	public const float SCORE_ANIMATION_TIME = 0.833f;

	private SkinnedMeshRenderer mainSkinnedMeshRenderer;

	private SkinnedMeshRenderer outlineSkinnedMeshrenderer;

	private Material matDefault;

	public Material matAlternative;

	private Animator animator;

	private Animator outlineAnimator;

	public GameObject modifierHolder_InstantReward;

	public GameObject modifierHolder_Ticket;

	public GameObject modifierHolder_Golden;

	public GameObject modifierHolder_Repetition;

	public GameObject modifierHolder_Battery;

	public GameObject modifierHolder_Chain;

	public SymbolScript.Kind kind = SymbolScript.Kind.undefined;

	private SymbolScript.Modifier modifier;

	private bool _isScoringSymbol;

	private int scoringSymbol_X = -1;

	private int scoringSymbol_Y = -1;

	public int layer = 11;

	private float animationTimer;

	private bool spinScaling;

	private float spinScalingOldY;

	public enum Kind
	{
		undefined = -1,
		lemon,
		cherry,
		clover,
		bell,
		diamond,
		coins,
		seven,
		six,
		nine,
		count
	}

	public enum Modifier
	{
		none,
		instantReward,
		cloverTicket,
		golden,
		repetition,
		battery,
		chain
	}
}
