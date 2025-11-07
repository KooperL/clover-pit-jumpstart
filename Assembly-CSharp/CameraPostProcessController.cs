using System;
using System.Collections.Generic;
using Panik;
using UnityEngine;

public class CameraPostProcessController : MonoBehaviour
{
	// Token: 0x06000051 RID: 81 RVA: 0x00006344 File Offset: 0x00004544
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

	// Token: 0x06000052 RID: 82 RVA: 0x000063B4 File Offset: 0x000045B4
	private void Awake()
	{
		this.myCamera = base.GetComponent<Camera>();
	}

	// Token: 0x06000053 RID: 83 RVA: 0x000063C2 File Offset: 0x000045C2
	private void OnEnable()
	{
		CameraPostProcessController.list.Add(this);
		CameraPostProcessController.topMostCameraChached = null;
	}

	// Token: 0x06000054 RID: 84 RVA: 0x000063D5 File Offset: 0x000045D5
	private void OnDisable()
	{
		CameraPostProcessController.list.Remove(this);
		CameraPostProcessController.topMostCameraChached = null;
	}

	// Token: 0x06000055 RID: 85 RVA: 0x000063E9 File Offset: 0x000045E9
	private void Update()
	{
		if (CameraPostProcessController.topMostCameraChached == null)
		{
			CameraPostProcessController.topMostCameraChached = CameraPostProcessController.GetTopmostCamera();
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00006404 File Offset: 0x00004604
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

	public static List<CameraPostProcessController> list = new List<CameraPostProcessController>();

	public static CameraPostProcessController topMostCameraChached = null;

	private Camera myCamera;

	private bool errorDisplayed_RenderingMasterNotAvailable;
}
