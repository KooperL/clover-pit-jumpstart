using System;
using System.Collections.Generic;
using Panik;
using TMPro;
using UnityEngine;

public class CharmUiRenderer : MonoBehaviour
{
	// Token: 0x06000844 RID: 2116 RVA: 0x00035F70 File Offset: 0x00034170
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

	// Token: 0x06000845 RID: 2117 RVA: 0x000363B0 File Offset: 0x000345B0
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

	// Token: 0x06000846 RID: 2118 RVA: 0x0003645F File Offset: 0x0003465F
	public PowerupScript.Identifier GetIdentifier()
	{
		return this.powerupIdentifier;
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x00036467 File Offset: 0x00034667
	public void Bounce(float force)
	{
		this.bounceScript.SetBounceScale(force);
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x00036478 File Offset: 0x00034678
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

	public static Dictionary<PowerupScript.Identifier, List<CharmUiRenderer>> dictionary = new Dictionary<PowerupScript.Identifier, List<CharmUiRenderer>>();

	public static Dictionary<PowerupScript.Identifier, List<CharmUiRenderer>> dictionaryDisabled = new Dictionary<PowerupScript.Identifier, List<CharmUiRenderer>>();

	public Transform meshRotatorTr;

	public TextMeshProUGUI text;

	public TextMeshProUGUI textCounter;

	public BounceScript bounceScript;

	private MeshRenderer myMeshRenderer;

	private SkinnedMeshRenderer mySkinnedMeshRenderer;

	public GameObject drawerIconObj;

	private PowerupScript.Identifier powerupIdentifier;

	private bool considerModifier;

	private PowerupScript.Modifier powerupModifier;

	private Material defaultMaterial;

	private Material modifierMaterial;
}
