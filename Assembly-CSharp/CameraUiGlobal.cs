using System;
using Panik;
using UnityEngine;

public class CameraUiGlobal : MonoBehaviour
{
	// Token: 0x06000059 RID: 89 RVA: 0x00006497 File Offset: 0x00004697
	public void UpdateRenderingTexture()
	{
		if (!Master.instance.RENDER_TO_TEXTURE)
		{
			this.myCamera.targetTexture = null;
			return;
		}
		this.myCamera.targetTexture = RenderingMaster.renderTextureCurrent;
	}

	// Token: 0x0600005A RID: 90 RVA: 0x000064C2 File Offset: 0x000046C2
	public static bool Debug_UiStateGet()
	{
		return CameraUiGlobal.instance == null || CameraUiGlobal.instance.myCanvas == null || CameraUiGlobal.instance.myCanvas.gameObject.activeSelf;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x000064FB File Offset: 0x000046FB
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

	// Token: 0x0600005C RID: 92 RVA: 0x00006533 File Offset: 0x00004733
	private void Awake()
	{
		CameraUiGlobal.instance = this;
		this.myCamera = base.GetComponent<Camera>();
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00006547 File Offset: 0x00004747
	private void Start()
	{
		if (base.gameObject.layer != 14)
		{
			Debug.LogError("Camera " + base.name + " is not set to the UI Global layer! Please fix it!");
		}
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00006572 File Offset: 0x00004772
	private void OnDestroy()
	{
		if (CameraUiGlobal.instance == this)
		{
			CameraUiGlobal.instance = null;
		}
	}

	public static CameraUiGlobal instance;

	[NonSerialized]
	public Camera myCamera;

	public Canvas myCanvas;
}
