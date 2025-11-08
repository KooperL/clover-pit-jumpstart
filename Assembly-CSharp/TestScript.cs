using System;
using System.Collections;
using Panik;
using TMPro;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	// Token: 0x06000831 RID: 2097 RVA: 0x00035A48 File Offset: 0x00033C48
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

	// Token: 0x06000832 RID: 2098 RVA: 0x00035AA1 File Offset: 0x00033CA1
	private void GraphShow()
	{
		if (this.graphShowCoroutine != null)
		{
			base.StopCoroutine(this.graphShowCoroutine);
		}
		this.graphShowCoroutine = base.StartCoroutine(this.GraphShowCoroutine());
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x00035ACC File Offset: 0x00033CCC
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

	// Token: 0x06000834 RID: 2100 RVA: 0x00035B84 File Offset: 0x00033D84
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

	// Token: 0x06000835 RID: 2101 RVA: 0x00035B93 File Offset: 0x00033D93
	private int GetRndVal()
	{
		return Mathf.FloorToInt(this.randomInst.Value * 100f);
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x00035BAC File Offset: 0x00033DAC
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
