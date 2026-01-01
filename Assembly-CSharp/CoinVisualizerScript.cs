using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;

// Token: 0x0200004D RID: 77
public class CoinVisualizerScript : MonoBehaviour
{
	// Token: 0x06000497 RID: 1175 RVA: 0x000093F5 File Offset: 0x000075F5
	private void Record()
	{
		this.localPoints.Add(this.meshHolderTr.localPosition);
		this.localEulers.Add(this.meshHolderTr.localEulerAngles);
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x00009423 File Offset: 0x00007623
	private void DeleteLast()
	{
		if (this.localPoints.Count > 0)
		{
			this.localPoints.RemoveAt(this.localPoints.Count - 1);
			this.localEulers.RemoveAt(this.localEulers.Count - 1);
		}
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x00009463 File Offset: 0x00007663
	private void ClearAll()
	{
		MonoBehaviour.print("Clearing all");
		this.localPoints.Clear();
		this.localEulers.Clear();
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x00030260 File Offset: 0x0002E460
	private void SetToLast()
	{
		if (this.localPoints.Count > 0)
		{
			this.meshHolderTr.localPosition = this.localPoints[this.localPoints.Count - 1];
			this.meshHolderTr.localEulerAngles = this.localEulers[this.localEulers.Count - 1];
		}
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x00009485 File Offset: 0x00007685
	private void SetToFirst()
	{
		if (this.localPoints.Count > 0)
		{
			this.meshHolderTr.localPosition = this.localPoints[0];
			this.meshHolderTr.localEulerAngles = this.localEulers[0];
		}
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x000094C3 File Offset: 0x000076C3
	public bool IsShowing()
	{
		return this.show;
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x000302C4 File Offset: 0x0002E4C4
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

	// Token: 0x0600049E RID: 1182 RVA: 0x00030338 File Offset: 0x0002E538
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

	// Token: 0x0600049F RID: 1183 RVA: 0x00030388 File Offset: 0x0002E588
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

	// Token: 0x060004A0 RID: 1184 RVA: 0x00030400 File Offset: 0x0002E600
	public static void HideAll(CoinVisualizerScript[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Hide(false);
		}
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x000094CB File Offset: 0x000076CB
	private void Awake()
	{
		CoinVisualizerScript.all.Add(this);
		this.Hide(true);
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x000094DF File Offset: 0x000076DF
	private void OnDestroy()
	{
		CoinVisualizerScript.all.Remove(this);
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x00030424 File Offset: 0x0002E624
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

	// Token: 0x0400043C RID: 1084
	public static List<CoinVisualizerScript> all = new List<CoinVisualizerScript>();

	// Token: 0x0400043D RID: 1085
	public Transform meshHolderTr;

	// Token: 0x0400043E RID: 1086
	public bool pausable = true;

	// Token: 0x0400043F RID: 1087
	public List<Vector3> localPoints = new List<Vector3>();

	// Token: 0x04000440 RID: 1088
	public List<Vector3> localEulers = new List<Vector3>();

	// Token: 0x04000441 RID: 1089
	public bool _buttonField_Record;

	// Token: 0x04000442 RID: 1090
	public bool _buttonField_DeleteLast;

	// Token: 0x04000443 RID: 1091
	public bool _buttonField_ClearAll;

	// Token: 0x04000444 RID: 1092
	public bool _buttonField_SetToLast;

	// Token: 0x04000445 RID: 1093
	public bool _buttonField_SetToFirst;

	// Token: 0x04000446 RID: 1094
	public float animationSpeed = 2f;

	// Token: 0x04000447 RID: 1095
	private float animationDelayTimer;

	// Token: 0x04000448 RID: 1096
	private bool show;

	// Token: 0x04000449 RID: 1097
	private float animationTimer;
}
