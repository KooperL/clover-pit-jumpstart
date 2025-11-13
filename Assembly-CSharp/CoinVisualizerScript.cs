using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;

public class CoinVisualizerScript : MonoBehaviour
{
	// Token: 0x06000421 RID: 1057 RVA: 0x0001C1E9 File Offset: 0x0001A3E9
	private void Record()
	{
		this.localPoints.Add(this.meshHolderTr.localPosition);
		this.localEulers.Add(this.meshHolderTr.localEulerAngles);
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x0001C217 File Offset: 0x0001A417
	private void DeleteLast()
	{
		if (this.localPoints.Count > 0)
		{
			this.localPoints.RemoveAt(this.localPoints.Count - 1);
			this.localEulers.RemoveAt(this.localEulers.Count - 1);
		}
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x0001C257 File Offset: 0x0001A457
	private void ClearAll()
	{
		MonoBehaviour.print("Clearing all");
		this.localPoints.Clear();
		this.localEulers.Clear();
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0001C27C File Offset: 0x0001A47C
	private void SetToLast()
	{
		if (this.localPoints.Count > 0)
		{
			this.meshHolderTr.localPosition = this.localPoints[this.localPoints.Count - 1];
			this.meshHolderTr.localEulerAngles = this.localEulers[this.localEulers.Count - 1];
		}
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x0001C2DD File Offset: 0x0001A4DD
	private void SetToFirst()
	{
		if (this.localPoints.Count > 0)
		{
			this.meshHolderTr.localPosition = this.localPoints[0];
			this.meshHolderTr.localEulerAngles = this.localEulers[0];
		}
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x0001C31B File Offset: 0x0001A51B
	public bool IsShowing()
	{
		return this.show;
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x0001C324 File Offset: 0x0001A524
	public void Show(string audioClipName, float soundDelay, float extraPitch)
	{
		if (this.show)
		{
			return;
		}
		this.show = true;
		this.animationTimer = 0f;
		this.meshHolderTr.gameObject.SetActive(true);
		if (!string.IsNullOrEmpty(audioClipName) && !GeneralUiScript.instance.IsShowingTitleScreen())
		{
			Sound.PlayDelayed(audioClipName, soundDelay, 1f, global::UnityEngine.Random.Range(0.9f, 1.1f) + extraPitch);
		}
		this.SetToFirst();
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x0001C398 File Offset: 0x0001A598
	public void Hide(bool force)
	{
		if (!this.show && !force)
		{
			return;
		}
		this.show = false;
		this.animationTimer = 0f;
		this.animationDelayTimer = 0f;
		this.meshHolderTr.gameObject.SetActive(false);
		this.SetToFirst();
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x0001C3E8 File Offset: 0x0001A5E8
	public static void ArrayCheckShow(CoinVisualizerScript[] array, int maxIndex, float delayBetweenCoins, float delayAccelleration, string audioClipName)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = array.Length - 1; i >= 0; i--)
		{
			if (i <= maxIndex)
			{
				if (!array[i].IsShowing())
				{
					array[i].Show(audioClipName, delayBetweenCoins, num2);
					array[i].animationDelayTimer = num;
					num += delayBetweenCoins;
					delayBetweenCoins -= delayAccelleration;
					delayBetweenCoins = Mathf.Max(0.01f, delayBetweenCoins);
					num2 += 0.025f;
				}
			}
			else
			{
				array[i].Hide(false);
			}
		}
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x0001C460 File Offset: 0x0001A660
	public static void HideAll(CoinVisualizerScript[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Hide(false);
		}
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x0001C484 File Offset: 0x0001A684
	private void Awake()
	{
		CoinVisualizerScript.all.Add(this);
		this.Hide(true);
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x0001C498 File Offset: 0x0001A698
	private void OnDestroy()
	{
		CoinVisualizerScript.all.Remove(this);
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x0001C4A8 File Offset: 0x0001A6A8
	private void Update()
	{
		if (!Tick.IsGameRunning && this.pausable)
		{
			return;
		}
		if (!PlatformMaster.IsInitialized())
		{
			return;
		}
		if (!this.show)
		{
			return;
		}
		if (this.animationDelayTimer > 0f)
		{
			this.animationDelayTimer -= Time.deltaTime;
			return;
		}
		this.animationTimer += Time.deltaTime * this.animationSpeed;
		this.animationTimer = Mathf.Clamp(this.animationTimer, 0f, 1f);
		int num = Mathf.FloorToInt(this.animationTimer * (float)(this.localPoints.Count - 1));
		int num2 = Mathf.Clamp(num + 1, 0, this.localPoints.Count - 1);
		float num3;
		if (num == num2)
		{
			num3 = 0f;
		}
		else
		{
			num3 = this.animationTimer - (float)num / (float)(this.localPoints.Count - 1);
		}
		this.meshHolderTr.localPosition = Vector3.Lerp(this.localPoints[num], this.localPoints[num2], num3);
		this.meshHolderTr.localEulerAngles = Vector3.Lerp(this.localEulers[num], this.localEulers[num2], num3);
	}

	public static List<CoinVisualizerScript> all = new List<CoinVisualizerScript>();

	public Transform meshHolderTr;

	public bool pausable = true;

	public List<Vector3> localPoints = new List<Vector3>();

	public List<Vector3> localEulers = new List<Vector3>();

	public bool _buttonField_Record;

	public bool _buttonField_DeleteLast;

	public bool _buttonField_ClearAll;

	public bool _buttonField_SetToLast;

	public bool _buttonField_SetToFirst;

	public float animationSpeed = 2f;

	private float animationDelayTimer;

	private bool show;

	private float animationTimer;
}
