using System;
using UnityEngine;

public class DissolveSphere : MonoBehaviour
{
	// Token: 0x06000A8B RID: 2699 RVA: 0x000481D4 File Offset: 0x000463D4
	private void Start()
	{
		this.mat = base.GetComponent<Renderer>().material;
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x000481E7 File Offset: 0x000463E7
	private void Update()
	{
		this.mat.SetFloat("_DissolveAmount", Mathf.Sin(Time.time) / 2f + 0.5f);
	}

	private Material mat;
}
