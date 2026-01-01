using System;
using UnityEngine;

// Token: 0x02000104 RID: 260
public class DissolveSphere : MonoBehaviour
{
	// Token: 0x06000C79 RID: 3193 RVA: 0x00010361 File Offset: 0x0000E561
	private void Start()
	{
		this.mat = base.GetComponent<Renderer>().material;
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x00010374 File Offset: 0x0000E574
	private void Update()
	{
		this.mat.SetFloat("_DissolveAmount", Mathf.Sin(Time.time) / 2f + 0.5f);
	}

	// Token: 0x04000D46 RID: 3398
	private Material mat;
}
