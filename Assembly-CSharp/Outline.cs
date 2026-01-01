using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000002 RID: 2
[DisallowMultipleComponent]
public class Outline : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000001 RID: 1 RVA: 0x00007258 File Offset: 0x00005458
	// (set) Token: 0x06000002 RID: 2 RVA: 0x00007260 File Offset: 0x00005460
	public Outline.Mode OutlineMode
	{
		get
		{
			return this.outlineMode;
		}
		set
		{
			this.outlineMode = value;
			this.needsUpdate = true;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000003 RID: 3 RVA: 0x00007270 File Offset: 0x00005470
	// (set) Token: 0x06000004 RID: 4 RVA: 0x00007278 File Offset: 0x00005478
	public Color OutlineColor
	{
		get
		{
			return this.outlineColor;
		}
		set
		{
			this.outlineColor = value;
			this.needsUpdate = true;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000005 RID: 5 RVA: 0x00007288 File Offset: 0x00005488
	// (set) Token: 0x06000006 RID: 6 RVA: 0x00007290 File Offset: 0x00005490
	public float OutlineWidth
	{
		get
		{
			return this.outlineWidth;
		}
		set
		{
			this.outlineWidth = value;
			this.needsUpdate = true;
		}
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00015EB8 File Offset: 0x000140B8
	private void Awake()
	{
		this.renderersList.Clear();
		this.renderersList.AddRange(base.GetComponentsInChildren<Renderer>());
		if (!this.considerSprites)
		{
			for (int i = this.renderersList.Count - 1; i >= 0; i--)
			{
				if (this.renderersList[i].GetComponent<SpriteRenderer>() != null)
				{
					this.renderersList.RemoveAt(i);
				}
			}
		}
		if (this.renderersToExclude.Count > 0)
		{
			for (int j = 0; j < this.renderersToExclude.Count; j++)
			{
				this.renderersList.Remove(this.renderersToExclude[j]);
			}
		}
		this.outlineMaskMaterial = global::UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>("Materials/OutlineMask"));
		this.outlineFillMaterial = global::UnityEngine.Object.Instantiate<Material>(Resources.Load<Material>("Materials/OutlineFill"));
		this.outlineMaskMaterial.name = "OutlineMask (Instance)";
		this.outlineFillMaterial.name = "OutlineFill (Instance)";
		this.LoadSmoothNormals();
		this.needsUpdate = true;
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00015FBC File Offset: 0x000141BC
	private void OnEnable()
	{
		foreach (Renderer renderer in this.renderersList)
		{
			List<Material> list = renderer.sharedMaterials.ToList<Material>();
			list.Add(this.outlineMaskMaterial);
			list.Add(this.outlineFillMaterial);
			renderer.materials = list.ToArray();
		}
	}

	// Token: 0x06000009 RID: 9 RVA: 0x00016038 File Offset: 0x00014238
	private void OnValidate()
	{
		this.needsUpdate = true;
		if ((!this.precomputeOutline && this.bakeKeys.Count != 0) || this.bakeKeys.Count != this.bakeValues.Count)
		{
			this.bakeKeys.Clear();
			this.bakeValues.Clear();
		}
		if (this.precomputeOutline && this.bakeKeys.Count == 0)
		{
			this.Bake();
		}
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000072A0 File Offset: 0x000054A0
	private void Update()
	{
		if (this.needsUpdate)
		{
			this.needsUpdate = false;
			this.UpdateMaterialProperties();
		}
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000160AC File Offset: 0x000142AC
	private void OnDisable()
	{
		foreach (Renderer renderer in this.renderersList)
		{
			List<Material> list = renderer.sharedMaterials.ToList<Material>();
			list.Remove(this.outlineMaskMaterial);
			list.Remove(this.outlineFillMaterial);
			renderer.materials = list.ToArray();
		}
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000072B7 File Offset: 0x000054B7
	private void OnDestroy()
	{
		global::UnityEngine.Object.Destroy(this.outlineMaskMaterial);
		global::UnityEngine.Object.Destroy(this.outlineFillMaterial);
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00016128 File Offset: 0x00014328
	private void Bake()
	{
		HashSet<Mesh> hashSet = new HashSet<Mesh>();
		foreach (MeshFilter meshFilter in base.GetComponentsInChildren<MeshFilter>())
		{
			if (hashSet.Add(meshFilter.sharedMesh))
			{
				List<Vector3> list = this.SmoothNormals(meshFilter.sharedMesh);
				this.bakeKeys.Add(meshFilter.sharedMesh);
				this.bakeValues.Add(new Outline.ListVector3
				{
					data = list
				});
			}
		}
	}

	// Token: 0x0600000E RID: 14 RVA: 0x0001619C File Offset: 0x0001439C
	private void LoadSmoothNormals()
	{
		foreach (MeshFilter meshFilter in base.GetComponentsInChildren<MeshFilter>())
		{
			if (Outline.registeredMeshes.Add(meshFilter.sharedMesh))
			{
				int num = this.bakeKeys.IndexOf(meshFilter.sharedMesh);
				List<Vector3> list = ((num >= 0) ? this.bakeValues[num].data : this.SmoothNormals(meshFilter.sharedMesh));
				meshFilter.sharedMesh.SetUVs(3, list);
				Renderer component = meshFilter.GetComponent<Renderer>();
				if (component != null)
				{
					this.CombineSubmeshes(meshFilter.sharedMesh, component.sharedMaterials);
				}
			}
		}
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in base.GetComponentsInChildren<SkinnedMeshRenderer>())
		{
			if (Outline.registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
			{
				skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
				this.CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);
			}
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x000162A8 File Offset: 0x000144A8
	private List<Vector3> SmoothNormals(Mesh mesh)
	{
		IEnumerable<IGrouping<Vector3, KeyValuePair<Vector3, int>>> enumerable = from pair in mesh.vertices.Select((Vector3 vertex, int index) => new KeyValuePair<Vector3, int>(vertex, index))
			group pair by pair.Key;
		List<Vector3> list = new List<Vector3>(mesh.normals);
		foreach (IGrouping<Vector3, KeyValuePair<Vector3, int>> grouping in enumerable)
		{
			if (grouping.Count<KeyValuePair<Vector3, int>>() != 1)
			{
				Vector3 vector = Vector3.zero;
				foreach (KeyValuePair<Vector3, int> keyValuePair in grouping)
				{
					vector += list[keyValuePair.Value];
				}
				vector.Normalize();
				foreach (KeyValuePair<Vector3, int> keyValuePair2 in grouping)
				{
					list[keyValuePair2.Value] = vector;
				}
			}
		}
		return list;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000163F0 File Offset: 0x000145F0
	private void CombineSubmeshes(Mesh mesh, Material[] materials)
	{
		if (mesh.subMeshCount == 1)
		{
			return;
		}
		if (mesh.subMeshCount > materials.Length)
		{
			return;
		}
		int subMeshCount = mesh.subMeshCount;
		mesh.subMeshCount = subMeshCount + 1;
		mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00016438 File Offset: 0x00014638
	private void UpdateMaterialProperties()
	{
		this.outlineFillMaterial.SetColor("_OutlineColor", this.outlineColor);
		switch (this.outlineMode)
		{
		case Outline.Mode.OutlineAll:
			this.outlineMaskMaterial.SetFloat("_ZTest", 8f);
			this.outlineFillMaterial.SetFloat("_ZTest", 8f);
			this.outlineFillMaterial.SetFloat("_OutlineWidth", this.outlineWidth);
			return;
		case Outline.Mode.OutlineVisible:
			this.outlineMaskMaterial.SetFloat("_ZTest", 8f);
			this.outlineFillMaterial.SetFloat("_ZTest", 4f);
			this.outlineFillMaterial.SetFloat("_OutlineWidth", this.outlineWidth);
			return;
		case Outline.Mode.OutlineHidden:
			this.outlineMaskMaterial.SetFloat("_ZTest", 8f);
			this.outlineFillMaterial.SetFloat("_ZTest", 5f);
			this.outlineFillMaterial.SetFloat("_OutlineWidth", this.outlineWidth);
			return;
		case Outline.Mode.OutlineAndSilhouette:
			this.outlineMaskMaterial.SetFloat("_ZTest", 4f);
			this.outlineFillMaterial.SetFloat("_ZTest", 8f);
			this.outlineFillMaterial.SetFloat("_OutlineWidth", this.outlineWidth);
			return;
		case Outline.Mode.SilhouetteOnly:
			this.outlineMaskMaterial.SetFloat("_ZTest", 4f);
			this.outlineFillMaterial.SetFloat("_ZTest", 5f);
			this.outlineFillMaterial.SetFloat("_OutlineWidth", 0f);
			return;
		default:
			return;
		}
	}

	// Token: 0x04000001 RID: 1
	private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

	// Token: 0x04000002 RID: 2
	[SerializeField]
	private Outline.Mode outlineMode;

	// Token: 0x04000003 RID: 3
	[SerializeField]
	private Color outlineColor = Color.white;

	// Token: 0x04000004 RID: 4
	[SerializeField]
	[Range(0f, 10f)]
	private float outlineWidth = 2f;

	// Token: 0x04000005 RID: 5
	public bool considerSprites;

	// Token: 0x04000006 RID: 6
	public List<Renderer> renderersToExclude;

	// Token: 0x04000007 RID: 7
	[Header("Optional")]
	[SerializeField]
	[Tooltip("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")]
	private bool precomputeOutline;

	// Token: 0x04000008 RID: 8
	[SerializeField]
	[HideInInspector]
	private List<Mesh> bakeKeys = new List<Mesh>();

	// Token: 0x04000009 RID: 9
	[SerializeField]
	[HideInInspector]
	private List<Outline.ListVector3> bakeValues = new List<Outline.ListVector3>();

	// Token: 0x0400000A RID: 10
	private List<Renderer> renderersList = new List<Renderer>(8);

	// Token: 0x0400000B RID: 11
	private Material outlineMaskMaterial;

	// Token: 0x0400000C RID: 12
	private Material outlineFillMaterial;

	// Token: 0x0400000D RID: 13
	private bool needsUpdate;

	// Token: 0x02000003 RID: 3
	public enum Mode
	{
		// Token: 0x0400000F RID: 15
		OutlineAll,
		// Token: 0x04000010 RID: 16
		OutlineVisible,
		// Token: 0x04000011 RID: 17
		OutlineHidden,
		// Token: 0x04000012 RID: 18
		OutlineAndSilhouette,
		// Token: 0x04000013 RID: 19
		SilhouetteOnly
	}

	// Token: 0x02000004 RID: 4
	[Serializable]
	private class ListVector3
	{
		// Token: 0x04000014 RID: 20
		public List<Vector3> data;
	}
}
