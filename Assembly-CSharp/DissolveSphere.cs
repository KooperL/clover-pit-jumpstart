using System;
using UnityEngine;

public class DissolveSphere : MonoBehaviour
{
	// Token: 0x06000AA0 RID: 2720 RVA: 0x00048934 File Offset: 0x00046B34
	private void Start()
	{
		this.mat = base.GetComponent<Renderer>().material;
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x00048947 File Offset: 0x00046B47
	private void Update()
	{
		this.mat.SetFloat("_DissolveAmount", Mathf.Sin(Time.time) / 2f + 0.5f);
	}

	private Material mat;
}
