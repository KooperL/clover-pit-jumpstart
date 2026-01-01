using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;

// Token: 0x020000A9 RID: 169
public class CharmUiRenderer : MonoBehaviour
{
	// Token: 0x0600096E RID: 2414 RVA: 0x0004D67C File Offset: 0x0004B87C
	public static CharmUiRenderer PoolSpawn(PowerupScript.Identifier identifier, int targetLayer, Transform desiredParent, bool normalizeScale, float scaleMult, float bounceEntryForce, bool considerModifier, bool showDrawerImg, string textString, string textCounterString)
	{
		if (!CharmUiRenderer.dictionary.ContainsKey(identifier))
		{
			CharmUiRenderer.dictionary.Add(identifier, new List<CharmUiRenderer>());
		}
		if (!CharmUiRenderer.dictionaryDisabled.ContainsKey(identifier))
		{
			CharmUiRenderer.dictionaryDisabled.Add(identifier, new List<CharmUiRenderer>());
		}
		CharmUiRenderer charmUiRenderer;
		if (CharmUiRenderer.dictionaryDisabled[identifier].Count > 0)
		{
			for (int i = CharmUiRenderer.dictionaryDisabled[identifier].Count - 1; i >= 0; i--)
			{
				if (CharmUiRenderer.dictionaryDisabled[identifier][i] != null)
				{
					charmUiRenderer = CharmUiRenderer.dictionaryDisabled[identifier][i];
					PowerupScript powerup_Quick = PowerupScript.GetPowerup_Quick(charmUiRenderer.powerupIdentifier);
					CharmUiRenderer.dictionaryDisabled[identifier].RemoveAt(i);
					CharmUiRenderer.dictionary[identifier].Add(charmUiRenderer);
					charmUiRenderer.gameObject.SetActive(true);
					charmUiRenderer.considerModifier = considerModifier;
					charmUiRenderer.powerupModifier = GameplayData.Powerup_Modifier_Get(charmUiRenderer.powerupIdentifier);
					charmUiRenderer.modifierMaterial = powerup_Quick.MaterialModifier_Get(charmUiRenderer.powerupModifier);
					if (charmUiRenderer.myMeshRenderer != null)
					{
						charmUiRenderer.myMeshRenderer.sharedMaterial = charmUiRenderer.defaultMaterial;
					}
					if (charmUiRenderer.mySkinnedMeshRenderer != null)
					{
						charmUiRenderer.mySkinnedMeshRenderer.sharedMaterial = charmUiRenderer.defaultMaterial;
					}
					if (charmUiRenderer.myMeshRenderer != null)
					{
						charmUiRenderer.myMeshRenderer.transform.localScale = (normalizeScale ? powerup_Quick.GetBoundingBoxSizeNormalized() : Vector3.one) * scaleMult;
					}
					if (charmUiRenderer.mySkinnedMeshRenderer != null)
					{
						charmUiRenderer.mySkinnedMeshRenderer.transform.localScale = (normalizeScale ? powerup_Quick.GetBoundingBoxSizeNormalized() : Vector3.one) * scaleMult;
					}
					charmUiRenderer.transform.SetParent(desiredParent);
					charmUiRenderer.transform.localScale = Vector3.one;
					charmUiRenderer.transform.localEulerAngles = Vector3.zero;
					charmUiRenderer.text.text = textString;
					charmUiRenderer.text.ForceMeshUpdate(false, false);
					charmUiRenderer.textCounter.text = textCounterString;
					charmUiRenderer.textCounter.ForceMeshUpdate(false, false);
					charmUiRenderer.Bounce(bounceEntryForce);
					charmUiRenderer.drawerIconObj.SetActive(showDrawerImg);
					if (charmUiRenderer.gameObject.layer != targetLayer)
					{
						Transform[] array = charmUiRenderer.GetComponentsInChildren<Transform>(true);
						for (int j = 0; j < array.Length; j++)
						{
							array[j].gameObject.layer = targetLayer;
						}
					}
					return charmUiRenderer;
				}
			}
		}
		charmUiRenderer = global::UnityEngine.Object.Instantiate<GameObject>(AssetMaster.GetPrefab("Charms Ui Renderer")).GetComponent<CharmUiRenderer>();
		if (charmUiRenderer == null)
		{
			Debug.LogError("CharmUiRenderer.Spawn(): return instance is null. Seems like the prefab is not well setup");
		}
		charmUiRenderer.powerupIdentifier = identifier;
		PowerupScript powerup_Quick2 = PowerupScript.GetPowerup_Quick(identifier);
		powerup_Quick2.MeshSteal(charmUiRenderer.meshRotatorTr, normalizeScale, scaleMult);
		if (charmUiRenderer.meshRotatorTr.childCount > 0)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(charmUiRenderer.meshRotatorTr.GetChild(0).gameObject, charmUiRenderer.meshRotatorTr);
			powerup_Quick2.MeshRestore(true);
			gameObject.transform.localPosition = Vector3.zero;
			ParticleSystem[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystem>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].gameObject.SetActive(false);
			}
			SpriteRenderer[] componentsInChildren2 = gameObject.GetComponentsInChildren<SpriteRenderer>();
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].gameObject.SetActive(false);
			}
		}
		charmUiRenderer.considerModifier = considerModifier;
		charmUiRenderer.powerupModifier = GameplayData.Powerup_Modifier_Get(powerup_Quick2.identifier);
		charmUiRenderer.myMeshRenderer = charmUiRenderer.meshRotatorTr.GetComponentInChildren<MeshRenderer>();
		charmUiRenderer.mySkinnedMeshRenderer = charmUiRenderer.meshRotatorTr.GetComponentInChildren<SkinnedMeshRenderer>();
		charmUiRenderer.defaultMaterial = powerup_Quick2.MaterialDefault_Get();
		charmUiRenderer.modifierMaterial = powerup_Quick2.MaterialModifier_Get(charmUiRenderer.powerupModifier);
		if (charmUiRenderer.myMeshRenderer != null)
		{
			charmUiRenderer.myMeshRenderer.sharedMaterial = powerup_Quick2.MaterialDefault_Get();
		}
		if (charmUiRenderer.mySkinnedMeshRenderer != null)
		{
			charmUiRenderer.mySkinnedMeshRenderer.sharedMaterial = powerup_Quick2.MaterialDefault_Get();
		}
		CharmUiRenderer.dictionary[identifier].Add(charmUiRenderer);
		charmUiRenderer.transform.SetParent(desiredParent);
		charmUiRenderer.transform.localScale = Vector3.one;
		charmUiRenderer.transform.localEulerAngles = Vector3.zero;
		charmUiRenderer.text.text = textString;
		charmUiRenderer.text.ForceMeshUpdate(false, false);
		charmUiRenderer.textCounter.text = textCounterString;
		charmUiRenderer.textCounter.ForceMeshUpdate(false, false);
		charmUiRenderer.Bounce(bounceEntryForce);
		charmUiRenderer.drawerIconObj.SetActive(showDrawerImg);
		if (charmUiRenderer.gameObject.layer != targetLayer)
		{
			Transform[] array = charmUiRenderer.GetComponentsInChildren<Transform>(true);
			for (int j = 0; j < array.Length; j++)
			{
				array[j].gameObject.layer = targetLayer;
			}
		}
		return charmUiRenderer;
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x0004DB28 File Offset: 0x0004BD28
	public static void PoolDestroy(CharmUiRenderer charmInstance)
	{
		if (charmInstance == null)
		{
			Debug.LogError("CharmUiRenderer.PoolDestroy(): cannot pool-destroy instance as charmInstance is null!");
		}
		PowerupScript.Identifier identifier = charmInstance.powerupIdentifier;
		if (!CharmUiRenderer.dictionary.ContainsKey(identifier))
		{
			CharmUiRenderer.dictionary.Add(identifier, new List<CharmUiRenderer>());
		}
		if (!CharmUiRenderer.dictionaryDisabled.ContainsKey(identifier))
		{
			CharmUiRenderer.dictionaryDisabled.Add(identifier, new List<CharmUiRenderer>());
		}
		CharmUiRenderer.dictionary[identifier].Remove(charmInstance);
		if (!CharmUiRenderer.dictionaryDisabled[identifier].Contains(charmInstance))
		{
			CharmUiRenderer.dictionaryDisabled[identifier].Add(charmInstance);
		}
		charmInstance.transform.SetParent(null);
		charmInstance.gameObject.SetActive(false);
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x0000D6C1 File Offset: 0x0000B8C1
	public PowerupScript.Identifier GetIdentifier()
	{
		return this.powerupIdentifier;
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x0000D6C9 File Offset: 0x0000B8C9
	public void Bounce(float force)
	{
		this.bounceScript.SetBounceScale(force);
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x0004DBD8 File Offset: 0x0004BDD8
	private void Update()
	{
		this.meshRotatorTr.AddLocalYAngle(Tick.Time * 180f);
		if (this.considerModifier)
		{
			Material material = this.defaultMaterial;
			if (Util.AngleSin(Tick.PassedTimePausable * 360f) < 0.8f)
			{
				material = this.modifierMaterial;
			}
			if (this.myMeshRenderer != null && this.myMeshRenderer.sharedMaterial != material)
			{
				this.myMeshRenderer.sharedMaterial = material;
			}
			if (this.mySkinnedMeshRenderer != null && this.mySkinnedMeshRenderer.sharedMaterial != material)
			{
				this.mySkinnedMeshRenderer.sharedMaterial = material;
			}
		}
	}

	// Token: 0x04000965 RID: 2405
	public static Dictionary<PowerupScript.Identifier, List<CharmUiRenderer>> dictionary = new Dictionary<PowerupScript.Identifier, List<CharmUiRenderer>>();

	// Token: 0x04000966 RID: 2406
	public static Dictionary<PowerupScript.Identifier, List<CharmUiRenderer>> dictionaryDisabled = new Dictionary<PowerupScript.Identifier, List<CharmUiRenderer>>();

	// Token: 0x04000967 RID: 2407
	public Transform meshRotatorTr;

	// Token: 0x04000968 RID: 2408
	public TextMeshProUGUI text;

	// Token: 0x04000969 RID: 2409
	public TextMeshProUGUI textCounter;

	// Token: 0x0400096A RID: 2410
	public BounceScript bounceScript;

	// Token: 0x0400096B RID: 2411
	private MeshRenderer myMeshRenderer;

	// Token: 0x0400096C RID: 2412
	private SkinnedMeshRenderer mySkinnedMeshRenderer;

	// Token: 0x0400096D RID: 2413
	public GameObject drawerIconObj;

	// Token: 0x0400096E RID: 2414
	private PowerupScript.Identifier powerupIdentifier;

	// Token: 0x0400096F RID: 2415
	private bool considerModifier;

	// Token: 0x04000970 RID: 2416
	private PowerupScript.Modifier powerupModifier;

	// Token: 0x04000971 RID: 2417
	private Material defaultMaterial;

	// Token: 0x04000972 RID: 2418
	private Material modifierMaterial;
}
