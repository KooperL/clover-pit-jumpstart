using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextRenderingSceneController : MonoBehaviour
{
	// Token: 0x06000A9C RID: 2716 RVA: 0x0004882C File Offset: 0x00046A2C
	private void Awake()
	{
		this.originalPositions = new Vector3[this.shakeThoseTransforms.Length];
		for (int i = 0; i < this.shakeThoseTransforms.Length; i++)
		{
			this.originalPositions[i] = this.shakeThoseTransforms[i].localPosition;
		}
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x00048878 File Offset: 0x00046A78
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x000488A0 File Offset: 0x00046AA0
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
