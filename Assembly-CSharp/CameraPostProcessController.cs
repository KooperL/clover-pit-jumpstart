using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;

// Token: 0x0200000D RID: 13
public class CameraPostProcessController : MonoBehaviour
{
	// Token: 0x0600005C RID: 92 RVA: 0x00019DA0 File Offset: 0x00017FA0
	public static CameraPostProcessController GetTopmostCamera()
	{
		CameraPostProcessController cameraPostProcessController = null;
		float num = float.MinValue;
		foreach (CameraPostProcessController cameraPostProcessController2 in CameraPostProcessController.list)
		{
			if (cameraPostProcessController2.myCamera.depth > num)
			{
				cameraPostProcessController = cameraPostProcessController2;
				num = cameraPostProcessController2.myCamera.depth;
			}
		}
		return cameraPostProcessController;
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00007812 File Offset: 0x00005A12
	private void Awake()
	{
		this.myCamera = base.GetComponent<Camera>();
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00007820 File Offset: 0x00005A20
	private void OnEnable()
	{
		CameraPostProcessController.list.Add(this);
		CameraPostProcessController.topMostCameraChached = null;
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00007833 File Offset: 0x00005A33
	private void OnDisable()
	{
		CameraPostProcessController.list.Remove(this);
		CameraPostProcessController.topMostCameraChached = null;
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00007847 File Offset: 0x00005A47
	private void Update()
	{
		if (CameraPostProcessController.topMostCameraChached == null)
		{
			CameraPostProcessController.topMostCameraChached = CameraPostProcessController.GetTopmostCamera();
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00019E10 File Offset: 0x00018010
	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (CameraPostProcessController.topMostCameraChached != this)
		{
			Graphics.Blit(src, dest);
			return;
		}
		if (RenderingMaster.instance == null)
		{
			if (!this.errorDisplayed_RenderingMasterNotAvailable)
			{
				this.errorDisplayed_RenderingMasterNotAvailable = true;
				Debug.LogError("RenderingMaster not available, skipping post processing. Scene: " + Level.CurrentScene.ToString() + " - Camera obj: " + base.name);
			}
			Graphics.Blit(src, dest);
			return;
		}
		RenderingMaster.instance._OnRenderImage(src, dest);
	}

	// Token: 0x040000C7 RID: 199
	public static List<CameraPostProcessController> list = new List<CameraPostProcessController>();

	// Token: 0x040000C8 RID: 200
	public static CameraPostProcessController topMostCameraChached = null;

	// Token: 0x040000C9 RID: 201
	private Camera myCamera;

	// Token: 0x040000CA RID: 202
	private bool errorDisplayed_RenderingMasterNotAvailable;
}
