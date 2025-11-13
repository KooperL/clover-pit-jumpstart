using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	public class CameraUi : MonoBehaviour
	{
		// Token: 0x06000B74 RID: 2932 RVA: 0x0004C3CF File Offset: 0x0004A5CF
		public int CameraIndex()
		{
			if (this._myIndex < 0)
			{
				this._myIndex = CameraUi.list.IndexOf(this);
			}
			return this._myIndex;
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x0004C3F1 File Offset: 0x0004A5F1
		public void UpdateRenderingTexture()
		{
			if (!Master.instance.RENDER_TO_TEXTURE)
			{
				this.myCamera.targetTexture = null;
				return;
			}
			this.myCamera.targetTexture = RenderingMaster.renderTextureCurrent;
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0004C41C File Offset: 0x0004A61C
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

		// Token: 0x06000B77 RID: 2935 RVA: 0x0004C4BE File Offset: 0x0004A6BE
		public void _SplitScreenUpdate()
		{
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0004C4C0 File Offset: 0x0004A6C0
		private void Awake()
		{
			if (CameraUi.firstInstance == null)
			{
				CameraUi.firstInstance = this;
			}
			CameraUi.list.Add(this);
			this.myCamera = base.GetComponent<Camera>();
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0004C4EC File Offset: 0x0004A6EC
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

		// Token: 0x06000B7A RID: 2938 RVA: 0x0004C6D4 File Offset: 0x0004A8D4
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
