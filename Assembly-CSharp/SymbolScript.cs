using System;
using System.Collections.Generic;
using System.Numerics;
using Panik;
using UnityEngine;

// Token: 0x02000081 RID: 129
public class SymbolScript : MonoBehaviour
{
	// Token: 0x0600087B RID: 2171 RVA: 0x0000CAEE File Offset: 0x0000ACEE
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

	// Token: 0x0600087C RID: 2172 RVA: 0x0000CB1C File Offset: 0x0000AD1C
	public static GameObject GetPrefab(SymbolScript.Kind kind)
	{
		return AssetMaster.GetPrefab(SymbolScript.GetPrefabName(kind));
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x00048804 File Offset: 0x00046A04
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

	// Token: 0x0600087E RID: 2174 RVA: 0x0000CB29 File Offset: 0x0000AD29
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

	// Token: 0x0600087F RID: 2175 RVA: 0x0000CB54 File Offset: 0x0000AD54
	public void RemoveFromScoringSymbols()
	{
		SymbolScript.scoringSymbols.Remove(this);
		this._isScoringSymbol = false;
		this.scoringSymbol_X = -1;
		this.scoringSymbol_Y = -1;
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x00048844 File Offset: 0x00046A44
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

	// Token: 0x06000881 RID: 2177 RVA: 0x0000CB77 File Offset: 0x0000AD77
	public SymbolScript.Modifier ModifierGet()
	{
		return this.modifier;
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00048AF8 File Offset: 0x00046CF8
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

	// Token: 0x06000883 RID: 2179 RVA: 0x00048B54 File Offset: 0x00046D54
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

	// Token: 0x06000884 RID: 2180 RVA: 0x00048BAC File Offset: 0x00046DAC
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

	// Token: 0x06000885 RID: 2181 RVA: 0x0000CB7F File Offset: 0x0000AD7F
	public void MaterialUpdate()
	{
		this.mainSkinnedMeshRenderer.sharedMaterial = this.matDefault;
		if (this.kind == SymbolScript.Kind.six && GameplayData.DebtIndexGet() >= GameplayData.SuperSixSixSix_GetMinimumDebtIndex())
		{
			this.mainSkinnedMeshRenderer.sharedMaterial = this.matAlternative;
		}
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00048C1C File Offset: 0x00046E1C
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

	// Token: 0x06000887 RID: 2183 RVA: 0x00048CA8 File Offset: 0x00046EA8
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

	// Token: 0x06000888 RID: 2184 RVA: 0x0000CBBD File Offset: 0x0000ADBD
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

	// Token: 0x06000889 RID: 2185 RVA: 0x0000CBEC File Offset: 0x0000ADEC
	public bool IsAnimationPlaying()
	{
		return this.animationTimer > 0f;
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x0000CBFB File Offset: 0x0000ADFB
	public void PlayAnimation(float speed)
	{
		this.animationTimer = 0.833f;
		this.animator.SetTrigger("playAnim");
		this.animator.speed = speed;
		this.outlineAnimator.SetTrigger("playAnim");
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x00048D10 File Offset: 0x00046F10
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

	// Token: 0x0600088C RID: 2188 RVA: 0x00048D5C File Offset: 0x00046F5C
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

	// Token: 0x0600088D RID: 2189 RVA: 0x0000CC34 File Offset: 0x0000AE34
	private void OnEnable()
	{
		SymbolScript.allEnabled.Add(this);
		this.ModifierSet(SymbolScript.Modifier.none);
		base.transform.localScale = global::UnityEngine.Vector3.one;
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x0000CC58 File Offset: 0x0000AE58
	private void OnDisable()
	{
		SymbolScript.allEnabled.Remove(this);
		this.RemoveFromScoringSymbols();
		this.SpinScalingSet(false);
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x00048EB0 File Offset: 0x000470B0
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

	// Token: 0x04000812 RID: 2066
	public static List<SymbolScript> allEnabled = new List<SymbolScript>();

	// Token: 0x04000813 RID: 2067
	public static List<SymbolScript> scoringSymbols = new List<SymbolScript>();

	// Token: 0x04000814 RID: 2068
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

	// Token: 0x04000815 RID: 2069
	public const float SCORE_ANIMATION_TIME = 0.833f;

	// Token: 0x04000816 RID: 2070
	private SkinnedMeshRenderer mainSkinnedMeshRenderer;

	// Token: 0x04000817 RID: 2071
	private SkinnedMeshRenderer outlineSkinnedMeshrenderer;

	// Token: 0x04000818 RID: 2072
	private Material matDefault;

	// Token: 0x04000819 RID: 2073
	public Material matAlternative;

	// Token: 0x0400081A RID: 2074
	private Animator animator;

	// Token: 0x0400081B RID: 2075
	private Animator outlineAnimator;

	// Token: 0x0400081C RID: 2076
	public GameObject modifierHolder_InstantReward;

	// Token: 0x0400081D RID: 2077
	public GameObject modifierHolder_Ticket;

	// Token: 0x0400081E RID: 2078
	public GameObject modifierHolder_Golden;

	// Token: 0x0400081F RID: 2079
	public GameObject modifierHolder_Repetition;

	// Token: 0x04000820 RID: 2080
	public GameObject modifierHolder_Battery;

	// Token: 0x04000821 RID: 2081
	public GameObject modifierHolder_Chain;

	// Token: 0x04000822 RID: 2082
	public SymbolScript.Kind kind = SymbolScript.Kind.undefined;

	// Token: 0x04000823 RID: 2083
	private SymbolScript.Modifier modifier;

	// Token: 0x04000824 RID: 2084
	private bool _isScoringSymbol;

	// Token: 0x04000825 RID: 2085
	private int scoringSymbol_X = -1;

	// Token: 0x04000826 RID: 2086
	private int scoringSymbol_Y = -1;

	// Token: 0x04000827 RID: 2087
	public int layer = 11;

	// Token: 0x04000828 RID: 2088
	private float animationTimer;

	// Token: 0x04000829 RID: 2089
	private bool spinScaling;

	// Token: 0x0400082A RID: 2090
	private float spinScalingOldY;

	// Token: 0x02000082 RID: 130
	public enum Kind
	{
		// Token: 0x0400082C RID: 2092
		undefined = -1,
		// Token: 0x0400082D RID: 2093
		lemon,
		// Token: 0x0400082E RID: 2094
		cherry,
		// Token: 0x0400082F RID: 2095
		clover,
		// Token: 0x04000830 RID: 2096
		bell,
		// Token: 0x04000831 RID: 2097
		diamond,
		// Token: 0x04000832 RID: 2098
		coins,
		// Token: 0x04000833 RID: 2099
		seven,
		// Token: 0x04000834 RID: 2100
		six,
		// Token: 0x04000835 RID: 2101
		nine,
		// Token: 0x04000836 RID: 2102
		count
	}

	// Token: 0x02000083 RID: 131
	public enum Modifier
	{
		// Token: 0x04000838 RID: 2104
		none,
		// Token: 0x04000839 RID: 2105
		instantReward,
		// Token: 0x0400083A RID: 2106
		cloverTicket,
		// Token: 0x0400083B RID: 2107
		golden,
		// Token: 0x0400083C RID: 2108
		repetition,
		// Token: 0x0400083D RID: 2109
		battery,
		// Token: 0x0400083E RID: 2110
		chain
	}
}
