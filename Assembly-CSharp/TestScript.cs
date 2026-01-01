using System;
using System.Collections;
using Panik;
using TMPro;
using UnityEngine;

// Token: 0x020000A5 RID: 165
public class TestScript : MonoBehaviour
{
	// Token: 0x06000955 RID: 2389 RVA: 0x0004CF30 File Offset: 0x0004B130
	private void Start()
	{
		this.graphColumns = new Transform[100];
		this.graphsCounter = new int[100];
		for (int i = 0; i < this.graphColumns.Length; i++)
		{
			this.graphColumns[i] = global::UnityEngine.Object.Instantiate<GameObject>(this.InstanceToClone, null).transform;
		}
		this.GraphShow();
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x0000D5A8 File Offset: 0x0000B7A8
	private void GraphShow()
	{
		if (this.graphShowCoroutine != null)
		{
			base.StopCoroutine(this.graphShowCoroutine);
		}
		this.graphShowCoroutine = base.StartCoroutine(this.GraphShowCoroutine());
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x0004CF8C File Offset: 0x0004B18C
	private void TextReset()
	{
		if (this.randomInst == null)
		{
			this.text.text = "No rng inst";
		}
		this.text.text = string.Concat(new string[]
		{
			"seed: ",
			this.randomInst.SeedInternalGet().ToString(),
			"\ninterations: ",
			10000.ToString(),
			" - range: D",
			100.ToString(),
			" - Iter spd (I key): ",
			this.iterateSlowly ? ("Slow(" + 1.ToString() + ")") : "Fast"
		});
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0000D5D0 File Offset: 0x0000B7D0
	private IEnumerator GraphShowCoroutine()
	{
		MonoBehaviour.print("Started");
		yield return null;
		TextMeshPro textMeshPro = this.text;
		textMeshPro.text += "\ngenerating...";
		this.randomInst = new Rng(this.seed, 0U);
		this.TextReset();
		for (int i = 0; i < this.graphColumns.Length; i++)
		{
			this.graphColumns[i].SetLocalYScale(0f);
			this.graphColumns[i].SetLocalXScale(0.1f);
			this.graphColumns[i].SetY(0f);
			this.graphColumns[i].SetX(-5f + (float)i * 0.1f + 0.05f);
		}
		for (int j = 0; j < this.graphsCounter.Length; j++)
		{
			this.graphsCounter[j] = 0;
		}
		if (!this.iterateSlowly)
		{
			yield return new WaitForSeconds(0.2f);
			for (int k = 0; k < 10000; k++)
			{
				int rndVal = this.GetRndVal();
				this.graphsCounter[rndVal]++;
			}
			for (int l = 0; l < this.graphColumns.Length; l++)
			{
				this.graphColumns[l].SetLocalYScale(0.1f * (float)this.graphsCounter[l]);
				this.graphColumns[l].SetY(this.graphColumns[l].GetLocalYScale() * 0.5f + 0.05f);
			}
		}
		else
		{
			int batchIter = 0;
			int num;
			for (int iter = 0; iter < 10000; iter = num + 1)
			{
				int rndVal2 = this.GetRndVal();
				this.graphsCounter[rndVal2]++;
				num = batchIter;
				batchIter = num + 1;
				if (batchIter >= 1)
				{
					batchIter--;
					for (int m = 0; m < this.graphColumns.Length; m++)
					{
						this.graphColumns[m].SetLocalYScale(0.1f * (float)this.graphsCounter[m]);
						this.graphColumns[m].SetY(this.graphColumns[m].GetLocalYScale() * 0.5f + 0.05f);
					}
					float timer = 0.2f;
					while (timer > 0f)
					{
						timer -= Time.deltaTime;
						yield return null;
					}
				}
				num = iter;
			}
		}
		this.text.text = this.text.text.Replace("\ngenerating...", "");
		this.graphShowCoroutine = null;
		yield break;
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x0000D5DF File Offset: 0x0000B7DF
	private int GetRndVal()
	{
		return Mathf.FloorToInt(this.randomInst.Value * 100f);
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x0004D044 File Offset: 0x0004B244
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			this.seed = global::UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			this.GraphShow();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			this.GraphShow();
		}
		if (Input.GetKeyDown(KeyCode.I))
		{
			this.iterateSlowly = !this.iterateSlowly;
			this.GraphShow();
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			Debug.Log("Value: " + this.randomInst.Value.ToString());
		}
	}

	// Token: 0x04000941 RID: 2369
	private const bool USE_UNITY_RNG = false;

	// Token: 0x04000942 RID: 2370
	private const int RANGE = 100;

	// Token: 0x04000943 RID: 2371
	private const int MAX_ITERATIONS = 10000;

	// Token: 0x04000944 RID: 2372
	private bool iterateSlowly;

	// Token: 0x04000945 RID: 2373
	private const int SLOW_ITERATIONS_BATCHES = 1;

	// Token: 0x04000946 RID: 2374
	private const float WAIT_BETWEEN_ITERATION_BATCHES = 0.2f;

	// Token: 0x04000947 RID: 2375
	public GameObject InstanceToClone;

	// Token: 0x04000948 RID: 2376
	private Transform[] graphColumns;

	// Token: 0x04000949 RID: 2377
	private int[] graphsCounter;

	// Token: 0x0400094A RID: 2378
	private int seed = int.MinValue;

	// Token: 0x0400094B RID: 2379
	public TextMeshPro text;

	// Token: 0x0400094C RID: 2380
	private Rng randomInst;

	// Token: 0x0400094D RID: 2381
	private Coroutine graphShowCoroutine;
}
