using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	public class CameraUi : MonoBehaviour
	{
		// Token: 0x06000B5F RID: 2911 RVA: 0x0004BC6F File Offset: 0x00049E6F
		public int CameraIndex()
		{
			if (this._myIndex < 0)
			{
				this._myIndex = CameraUi.list.IndexOf(this);
			}
			return this._myIndex;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0004BC91 File Offset: 0x00049E91
		public void UpdateRenderingTexture()
		{
			if (!Master.instance.RENDER_TO_TEXTURE)
			{
				this.myCamera.targetTexture = null;
				return;
			}
			this.myCamera.targetTexture = RenderingMaster.renderTextureCurrent;
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0004BCBC File Offset: 0x00049EBC
		public void CullingMaskUpdate()
		{
			switch (this.CameraIndex())
			{
			case 0:
				this.myCamera.cullingMask = 1024;
				base.gameObject.layer = 10;
				return;
			case 1:
				this.myCamera.cullingMask = 2048;
				base.gameObject.layer = 11;
				return;
			case 2:
				this.myCamera.cullingMask = 4096;
				base.gameObject.layer = 12;
				return;
			case 3:
				this.myCamera.cullingMask = 8192;
				base.gameObject.layer = 13;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0004BD5E File Offset: 0x00049F5E
		public void _SplitScreenUpdate()
		{
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0004BD60 File Offset: 0x00049F60
		private void Awake()
		{
			if (CameraUi.firstInstance == null)
			{
				CameraUi.firstInstance = this;
			}
			CameraUi.list.Add(this);
			this.myCamera = base.GetComponent<Camera>();
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0004BD8C File Offset: 0x00049F8C
		private void Start()
		{
			this._myIndex = -1;
			AudioListener component = base.GetComponent<AudioListener>();
			if (component != null)
			{
				global::UnityEngine.Object.Destroy(component);
			}
			this.CullingMaskUpdate();
			int num = this.CameraIndex();
			switch (num)
			{
			case 1:
				if (this.myCamera.cullingMask != 1024)
				{
					Debug.LogError("Camera " + base.name + " is not set to render only the UI layer! Camera index: " + num.ToString());
				}
				if (base.gameObject.layer != 10)
				{
					Debug.LogError("Camera " + base.name + " is not set to the UI layer! Camera index: " + num.ToString());
					return;
				}
				break;
			case 2:
				if (this.myCamera.cullingMask != 2048)
				{
					Debug.LogError("Camera " + base.name + " is not set to render only the UI layer! Camera index: " + num.ToString());
				}
				if (base.gameObject.layer != 11)
				{
					Debug.LogError("Camera " + base.name + " is not set to the UI layer! Camera index: " + num.ToString());
					return;
				}
				break;
			case 3:
				if (this.myCamera.cullingMask != 4096)
				{
					Debug.LogError("Camera " + base.name + " is not set to render only the UI layer! Camera index: " + num.ToString());
				}
				if (base.gameObject.layer != 12)
				{
					Debug.LogError("Camera " + base.name + " is not set to the UI layer! Camera index: " + num.ToString());
					return;
				}
				break;
			case 4:
				if (this.myCamera.cullingMask != 8192)
				{
					Debug.LogError("Camera " + base.name + " is not set to render only the UI layer! Camera index: " + num.ToString());
				}
				if (base.gameObject.layer != 13)
				{
					Debug.LogError("Camera " + base.name + " is not set to the UI layer! Camera index: " + num.ToString());
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0004BF74 File Offset: 0x0004A174
		private void OnDestroy()
		{
			if (CameraUi.firstInstance == this)
			{
				CameraUi.firstInstance = null;
			}
			CameraUi.list.Remove(this);
		}

		public static CameraUi firstInstance = null;

		public static List<CameraUi> list = new List<CameraUi>();

		private int _myIndex = -1;

		[NonSerialized]
		public Camera myCamera;
	}
}
