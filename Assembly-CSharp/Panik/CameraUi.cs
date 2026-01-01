using System;
using System.Collections.Generic;
using UnityEngine;

namespace Panik
{
	// Token: 0x02000119 RID: 281
	public class CameraUi : MonoBehaviour
	{
		// Token: 0x06000D59 RID: 3417 RVA: 0x00010F05 File Offset: 0x0000F105
		public int CameraIndex()
		{
			if (this._myIndex < 0)
			{
				this._myIndex = CameraUi.list.IndexOf(this);
			}
			return this._myIndex;
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00010F27 File Offset: 0x0000F127
		public void UpdateRenderingTexture()
		{
			if (!Master.instance.RENDER_TO_TEXTURE)
			{
				this.myCamera.targetTexture = null;
				return;
			}
			this.myCamera.targetTexture = RenderingMaster.renderTextureCurrent;
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00065E04 File Offset: 0x00064004
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

		// Token: 0x06000D5C RID: 3420 RVA: 0x0000774E File Offset: 0x0000594E
		public void _SplitScreenUpdate()
		{
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00010F52 File Offset: 0x0000F152
		private void Awake()
		{
			if (CameraUi.firstInstance == null)
			{
				CameraUi.firstInstance = this;
			}
			CameraUi.list.Add(this);
			this.myCamera = base.GetComponent<Camera>();
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00065EA8 File Offset: 0x000640A8
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

		// Token: 0x06000D5F RID: 3423 RVA: 0x00010F7E File Offset: 0x0000F17E
		private void OnDestroy()
		{
			if (CameraUi.firstInstance == this)
			{
				CameraUi.firstInstance = null;
			}
			CameraUi.list.Remove(this);
		}

		// Token: 0x04000DE9 RID: 3561
		public static CameraUi firstInstance = null;

		// Token: 0x04000DEA RID: 3562
		public static List<CameraUi> list = new List<CameraUi>();

		// Token: 0x04000DEB RID: 3563
		private int _myIndex = -1;

		// Token: 0x04000DEC RID: 3564
		[NonSerialized]
		public Camera myCamera;
	}
}
