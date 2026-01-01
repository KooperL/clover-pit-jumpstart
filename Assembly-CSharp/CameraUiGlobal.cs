using System;
using Panik;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class CameraUiGlobal : MonoBehaviour
{
	// Token: 0x06000064 RID: 100 RVA: 0x0000787A File Offset: 0x00005A7A
	public void UpdateRenderingTexture()
	{
		if (!Master.instance.RENDER_TO_TEXTURE)
		{
			this.myCamera.targetTexture = null;
			return;
		}
		this.myCamera.targetTexture = RenderingMaster.renderTextureCurrent;
	}

	// Token: 0x06000065 RID: 101 RVA: 0x000078A5 File Offset: 0x00005AA5
	public static bool Debug_UiStateGet()
	{
		return CameraUiGlobal.instance == null || CameraUiGlobal.instance.myCanvas == null || CameraUiGlobal.instance.myCanvas.gameObject.activeSelf;
	}

	// Token: 0x06000066 RID: 102 RVA: 0x000078DE File Offset: 0x00005ADE
	public static void Debug_UiStateSet(bool state)
	{
		if (CameraUiGlobal.instance == null)
		{
			return;
		}
		if (CameraUiGlobal.instance.myCanvas == null)
		{
			return;
		}
		CameraUiGlobal.instance.myCanvas.gameObject.SetActive(state);
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00007916 File Offset: 0x00005B16
	private void Awake()
	{
		CameraUiGlobal.instance = this;
		this.myCamera = base.GetComponent<Camera>();
	}

	// Token: 0x06000068 RID: 104 RVA: 0x0000792A File Offset: 0x00005B2A
	private void Start()
	{
		if (base.gameObject.layer != 14)
		{
			Debug.LogError("Camera " + base.name + " is not set to the UI Global layer! Please fix it!");
		}
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00007955 File Offset: 0x00005B55
	private void OnDestroy()
	{
		if (CameraUiGlobal.instance == this)
		{
			CameraUiGlobal.instance = null;
		}
	}

	// Token: 0x040000CB RID: 203
	public static CameraUiGlobal instance;

	// Token: 0x040000CC RID: 204
	[NonSerialized]
	public Camera myCamera;

	// Token: 0x040000CD RID: 205
	public Canvas myCanvas;
}
