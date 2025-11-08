using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextRenderingSceneController : MonoBehaviour
{
	// Token: 0x06000A87 RID: 2695 RVA: 0x000480CC File Offset: 0x000462CC
	private void Awake()
	{
		this.originalPositions = new Vector3[this.shakeThoseTransforms.Length];
		for (int i = 0; i < this.shakeThoseTransforms.Length; i++)
		{
			this.originalPositions[i] = this.shakeThoseTransforms[i].localPosition;
		}
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x00048118 File Offset: 0x00046318
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x00048140 File Offset: 0x00046340
	private void FixedUpdate()
	{
		for (int i = 0; i < this.shakeThoseTransforms.Length; i++)
		{
			float num = this.shakeMultipliers[i];
			Transform transform = this.shakeThoseTransforms[i];
			transform.localPosition = this.originalPositions[i] + new Vector3(global::UnityEngine.Random.Range(-this.shakeScale, this.shakeScale) * num, global::UnityEngine.Random.Range(-this.shakeScale, this.shakeScale) * num, transform.localPosition.z);
		}
	}

	public Transform[] shakeThoseTransforms;

	private Vector3[] originalPositions;

	public float[] shakeMultipliers;

	public float shakeScale = 1f;
}
