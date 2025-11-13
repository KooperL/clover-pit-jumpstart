using System;
using System.Collections;
using Panik;
using TMPro;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	// Token: 0x06000838 RID: 2104 RVA: 0x00035C30 File Offset: 0x00033E30
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

	// Token: 0x06000839 RID: 2105 RVA: 0x00035C89 File Offset: 0x00033E89
	private void GraphShow()
	{
		if (this.graphShowCoroutine != null)
		{
			base.StopCoroutine(this.graphShowCoroutine);
		}
		this.graphShowCoroutine = base.StartCoroutine(this.GraphShowCoroutine());
	}

	// Token: 0x0600083A RID: 2106 RVA: 0x00035CB4 File Offset: 0x00033EB4
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

	// Token: 0x0600083B RID: 2107 RVA: 0x00035D6C File Offset: 0x00033F6C
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

	// Token: 0x0600083C RID: 2108 RVA: 0x00035D7B File Offset: 0x00033F7B
	private int GetRndVal()
	{
		return Mathf.FloorToInt(this.randomInst.Value * 100f);
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00035D94 File Offset: 0x00033F94
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

	private const bool USE_UNITY_RNG = false;

	private const int RANGE = 100;

	private const int MAX_ITERATIONS = 10000;

	private bool iterateSlowly;

	private const int SLOW_ITERATIONS_BATCHES = 1;

	private const float WAIT_BETWEEN_ITERATION_BATCHES = 0.2f;

	public GameObject InstanceToClone;

	private Transform[] graphColumns;

	private int[] graphsCounter;

	private int seed = int.MinValue;

	public TextMeshPro text;

	private Rng randomInst;

	private Coroutine graphShowCoroutine;
}
