using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000103 RID: 259
public class TextRenderingSceneController : MonoBehaviour
{
	// Token: 0x06000C75 RID: 3189 RVA: 0x00062C58 File Offset: 0x00060E58
	private void Awake()
	{
		this.originalPositions = new Vector3[this.shakeThoseTransforms.Length];
		for (int i = 0; i < this.shakeThoseTransforms.Length; i++)
		{
			this.originalPositions[i] = this.shakeThoseTransforms[i].localPosition;
		}
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x00062CA4 File Offset: 0x00060EA4
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x00062CCC File Offset: 0x00060ECC
	private void FixedUpdate()
	{
		for (int i = 0; i < this.shakeThoseTransforms.Length; i++)
		{
			float num = this.shakeMultipliers[i];
			Transform transform = this.shakeThoseTransforms[i];
			transform.localPosition = this.originalPositions[i] + new Vector3(global::UnityEngine.Random.Range(-this.shakeScale, this.shakeScale) * num, global::UnityEngine.Random.Range(-this.shakeScale, this.shakeScale) * num, transform.localPosition.z);
		}
	}

	// Token: 0x04000D42 RID: 3394
	public Transform[] shakeThoseTransforms;

	// Token: 0x04000D43 RID: 3395
	private Vector3[] originalPositions;

	// Token: 0x04000D44 RID: 3396
	public float[] shakeMultipliers;

	// Token: 0x04000D45 RID: 3397
	public float shakeScale = 1f;
}
