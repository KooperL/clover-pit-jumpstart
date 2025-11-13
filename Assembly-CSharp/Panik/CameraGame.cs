using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Panik
{
	public class CameraGame : MonoBehaviour
	{
		// Token: 0x06000B4D RID: 2893 RVA: 0x0004B9A2 File Offset: 0x00049BA2
		public int CameraIndex()
		{
			if (this._myIndex < 0)
			{
				this._myIndex = CameraGame.list.IndexOf(this);
			}
			return this._myIndex;
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0004B9C4 File Offset: 0x00049BC4
		public void UpdateRenderingTexture()
		{
			if (!Master.instance.RENDER_TO_TEXTURE)
			{
				this.myCamera.targetTexture = null;
				return;
			}
			this.myCamera.targetTexture = RenderingMaster.renderTextureCurrent;
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0004B9EF File Offset: 0x00049BEF
		public static void Shake(float magnitude)
		{
			if (!Data.settings.screenshakeEnabled)
			{
				return;
			}
			CameraGame.firstInstance.shakeMagnitude = Mathf.Max(magnitude, CameraGame.firstInstance.shakeMagnitude);
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0004BA18 File Offset: 0x00049C18
		public static void ShakeDecaySpeedSet(float speed)
		{
			CameraGame.firstInstance.shakeMagnitudeDecaySpeed = speed;
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0004BA25 File Offset: 0x00049C25
		public static float shakeGet()
		{
			return CameraGame.firstInstance.shakeMagnitude;
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0004BA31 File Offset: 0x00049C31
		public static float ShakeDecaySpeedGet()
		{
			return CameraGame.firstInstance.shakeMagnitudeDecaySpeed;
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0004BA3D File Offset: 0x00049C3D
		public static bool ShakePausableGet()
		{
			return CameraGame.firstInstance.shakePausable;
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0004BA49 File Offset: 0x00049C49
		public static void ShakePausableSet(bool pausable)
		{
			CameraGame.firstInstance.shakePausable = pausable;
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0004BA58 File Offset: 0x00049C58
		private void ScreenshakeUpdate()
		{
			this.shakeMagnitude = Mathf.Max(0f, this.shakeMagnitude - this.shakeMagnitudeDecaySpeed * Tick.Time);
			base.transform.SetLocalZAngle(global::UnityEngine.Random.Range(-this.shakeMagnitude, this.shakeMagnitude));
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0004BAA6 File Offset: 0x00049CA6
		public static void ChromaticAberrationIntensitySet(float intensity)
		{
			if (!Data.settings.chromaticAberrationEnabled)
			{
				CameraGame.firstInstance.chromaticAberrationIntensity = 0f;
				return;
			}
			CameraGame.firstInstance.chromaticAberrationIntensity = Mathf.Max(intensity, CameraGame.firstInstance.chromaticAberrationIntensity);
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0004BADE File Offset: 0x00049CDE
		public static void ChromaticAberrationIntensityDecaySpeedSet(float speed)
		{
			CameraGame.firstInstance.chromaticAberrationIntensityDecaySpeed = speed;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0004BAEB File Offset: 0x00049CEB
		public static float ChromaticAberrationIntensityGet()
		{
			return CameraGame.firstInstance.chromaticAberrationIntensity;
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0004BAF7 File Offset: 0x00049CF7
		public static float ChromaticAberrationIntensityDecaySpeedGet()
		{
			return CameraGame.firstInstance.chromaticAberrationIntensityDecaySpeed;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0004BB03 File Offset: 0x00049D03
		public static void BloomIntensitySet(float intensity)
		{
			if (!Data.settings.bloomEnabled)
			{
				CameraGame.firstInstance.bloomIntensity = 0f;
				return;
			}
			CameraGame.firstInstance.bloomIntensity = Mathf.Max(intensity, CameraGame.firstInstance.bloomIntensity);
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0004BB3B File Offset: 0x00049D3B
		public static void BloomIntensityDecaySpeedSet(float speed)
		{
			CameraGame.firstInstance.bloomIntensityDecaySpeed = speed;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0004BB48 File Offset: 0x00049D48
		public static float BloomIntensityGet()
		{
			return CameraGame.firstInstance.bloomIntensity;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x0004BB54 File Offset: 0x00049D54
		public static float BloomIntensityDecaySpeedGet()
		{
			return CameraGame.firstInstance.bloomIntensityDecaySpeed;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0004BB60 File Offset: 0x00049D60
		private void PostProcessingRoutine()
		{
			if (Data.settings.chromaticAberrationEnabled)
			{
				if (!this.chromaticAberrationSetting.enabled.value)
				{
					this.chromaticAberrationSetting.enabled.value = true;
				}
				if (this.chromaticAberrationIntensity > 0f)
				{
					this.chromaticAberrationIntensity -= this.chromaticAberrationIntensityDecaySpeed * Tick.Time;
					if (this.chromaticAberrationIntensity <= 0f)
					{
						this.chromaticAberrationIntensity = 0f;
					}
					this.chromaticAberrationSetting.intensity.value = this.chromaticAberrationIntensity;
				}
			}
			else
			{
				this.chromaticAberrationSetting.intensity.value = 0f;
				if (this.chromaticAberrationSetting.enabled.value)
				{
					this.chromaticAberrationSetting.enabled.value = false;
				}
			}
			if (Data.settings.bloomEnabled)
			{
				if (!this.bloomSetting.enabled.value)
				{
					this.bloomSetting.enabled.value = true;
				}
				if (this.bloomIntensity > 0f)
				{
					this.bloomIntensity -= this.bloomIntensityDecaySpeed * Tick.Time;
					if (this.bloomIntensity <= 0f)
					{
						this.bloomIntensity = 0f;
					}
					this.bloomSetting.intensity.value = this.bloomIntensity;
				}
			}
			else
			{
				this.bloomSetting.intensity.value = 0f;
				if (this.bloomSetting.enabled.value)
				{
					this.bloomSetting.enabled.value = false;
				}
			}
			if (Data.settings.motionBlurEnabled)
			{
				if (!this.motionBlurSetting.enabled.value)
				{
					this.motionBlurSetting.enabled.value = true;
					return;
				}
			}
			else if (this.motionBlurSetting.enabled.value)
			{
				this.motionBlurSetting.enabled.value = false;
			}
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0004BD40 File Offset: 0x00049F40
		public void FieldOfViewDefaultUpdate()
		{
			int num = this.CameraIndex();
			this.fieldOfViewDefault = Data.settings.FovGet(num);
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0004BD65 File Offset: 0x00049F65
		private void FovExtraEntryEnsure(string tag)
		{
			if (!this.fovExtraEntries.ContainsKey(tag))
			{
				this.fovExtraEntries.Add(tag, new CameraGame.FovEntry());
				this._fovExtraEntriesList.Add(this.fovExtraEntries[tag]);
			}
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x0004BDA0 File Offset: 0x00049FA0
		private float _FovExtraCompute()
		{
			float num = 0f;
			foreach (KeyValuePair<string, CameraGame.FovEntry> keyValuePair in this.fovExtraEntries)
			{
				num += keyValuePair.Value.value;
			}
			if (num >= 0f)
			{
				float num2 = 0f;
				foreach (KeyValuePair<string, CameraGame.FovEntry> keyValuePair2 in this.fovExtraEntries)
				{
					num2 = Mathf.Max(num2, keyValuePair2.Value.value);
				}
				return num2;
			}
			float num3 = 0f;
			foreach (KeyValuePair<string, CameraGame.FovEntry> keyValuePair3 in this.fovExtraEntries)
			{
				num3 = Mathf.Min(num3, keyValuePair3.Value.value);
			}
			return num3;
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0004BEBC File Offset: 0x0004A0BC
		public static void FieldOfViewExtraSet(string tag, float extra)
		{
			CameraGame.firstInstance.FovExtraEntryEnsure(tag);
			CameraGame.firstInstance.fovExtraEntries[tag].value = extra;
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0004BEDF File Offset: 0x0004A0DF
		public static void FieldOfViewExtraDecaySpeedSet(string tag, float speed)
		{
			CameraGame.firstInstance.FovExtraEntryEnsure(tag);
			CameraGame.firstInstance.fovExtraEntries[tag].decaySpeed = speed;
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0004BF02 File Offset: 0x0004A102
		public static float FieldOfViewExtraGet()
		{
			return CameraGame.firstInstance.fovExtraTotal;
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0004BF0E File Offset: 0x0004A10E
		public static float FieldOfViewExtraDecaySpeedGet(string tag)
		{
			CameraGame.firstInstance.FovExtraEntryEnsure(tag);
			return CameraGame.firstInstance.fovExtraEntries[tag].decaySpeed;
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0004BF30 File Offset: 0x0004A130
		public static bool FieldOfViewExtraPausableGet(string tag)
		{
			CameraGame.firstInstance.FovExtraEntryEnsure(tag);
			return CameraGame.firstInstance.fovExtraEntries[tag].pausable;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0004BF52 File Offset: 0x0004A152
		public static void FieldOfViewExtraPausableSet(string tag, bool pausable)
		{
			CameraGame.firstInstance.FovExtraEntryEnsure(tag);
			CameraGame.firstInstance.fovExtraEntries[tag].pausable = pausable;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0004BF78 File Offset: 0x0004A178
		public void FieldOfViewUpdate()
		{
			for (int i = 0; i < this._fovExtraEntriesList.Count; i++)
			{
				if (!this._fovExtraEntriesList[i].pausable || Tick.IsGameRunning)
				{
					if (this._fovExtraEntriesList[i].value > 0f)
					{
						this._fovExtraEntriesList[i].value = Mathf.Max(0f, this._fovExtraEntriesList[i].value - this._fovExtraEntriesList[i].decaySpeed * Tick.Time);
					}
					else if (this._fovExtraEntriesList[i].value < 0f)
					{
						this._fovExtraEntriesList[i].value = Mathf.Min(0f, this._fovExtraEntriesList[i].value + this._fovExtraEntriesList[i].decaySpeed * Tick.Time);
					}
				}
			}
			this.fovExtraTotal = this._FovExtraCompute();
			this.myCamera.fieldOfView = this.fieldOfViewDefault + CameraGame.FieldOfViewExtraGet();
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0004C09C File Offset: 0x0004A29C
		public void CullingMaskUpdate()
		{
			switch (this.CameraIndex())
			{
			case 0:
				base.gameObject.layer = 6;
				break;
			case 1:
				base.gameObject.layer = 7;
				break;
			case 2:
				base.gameObject.layer = 8;
				break;
			case 3:
				base.gameObject.layer = 9;
				break;
			default:
				throw new NotImplementedException();
			}
			int num = -1;
			if (this.cullOutMyLayer)
			{
				num &= ~(1 << base.gameObject.layer);
			}
			if (this.cullOutUICameras)
			{
				num &= -1025;
				num &= -2049;
				num &= -4097;
				num &= -8193;
				num &= -16385;
			}
			this.myCamera.cullingMask = num;
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0004C161 File Offset: 0x0004A361
		public void _SplitScreenUpdate()
		{
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0004C164 File Offset: 0x0004A364
		public static void UpdatePSXEffectsToSettings_All()
		{
			foreach (CameraGame cameraGame in CameraGame.list)
			{
				cameraGame.UpdatePSXEffectsToSettings();
			}
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0004C1B4 File Offset: 0x0004A3B4
		private void UpdatePSXEffectsToSettings()
		{
			this.pSXEffects.vertexInaccuracy = (Data.settings.wobblyPolygons ? 64 : 0);
			this.pSXEffects.worldSpaceSnapping = Data.settings.wobblyPolygons;
			this.pSXEffects.camSnapping = Data.settings.wobblyPolygons;
			this.pSXEffects.UpdateProperties();
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0004C212 File Offset: 0x0004A412
		public static void SetColorDepth(int depth0_24)
		{
			CameraGame.firstInstance.pSXEffects.colorDepth = depth0_24;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0004C224 File Offset: 0x0004A424
		private void Awake()
		{
			if (CameraGame.firstInstance == null)
			{
				CameraGame.firstInstance = this;
			}
			CameraGame.list.Add(this);
			this.myCamera = base.GetComponent<Camera>();
			this.postProcessVolume = base.GetComponent<PostProcessVolume>();
			this.postProcessProfile = this.postProcessVolume.profile;
			this.chromaticAberrationSetting = this.postProcessProfile.GetSetting<ChromaticAberration>();
			this.bloomSetting = this.postProcessProfile.GetSetting<Bloom>();
			this.motionBlurSetting = this.postProcessProfile.GetSetting<MotionBlur>();
			this.pSXEffects = base.GetComponent<PSXEffects>();
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0004C2B7 File Offset: 0x0004A4B7
		private void Start()
		{
			this.FieldOfViewDefaultUpdate();
			this.FieldOfViewUpdate();
			if (GameplayMaster.instance == null)
			{
				this.audioReverbFilter.enabled = false;
			}
			else
			{
				this.audioReverbFilter.enabled = true;
			}
			this.UpdatePSXEffectsToSettings();
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0004C2F2 File Offset: 0x0004A4F2
		private void OnDestroy()
		{
			if (CameraGame.firstInstance == this)
			{
				CameraGame.firstInstance = null;
			}
			CameraGame.list.Remove(this);
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0004C313 File Offset: 0x0004A513
		private void Update()
		{
			this._myIndex = -1;
			this.PostProcessingRoutine();
			if (!this.shakePausable)
			{
				this.ScreenshakeUpdate();
			}
			this.FieldOfViewUpdate();
			if (!Tick.IsGameRunning)
			{
				return;
			}
			if (this.shakePausable)
			{
				this.ScreenshakeUpdate();
			}
		}

		public static CameraGame firstInstance = null;

		public static List<CameraGame> list = new List<CameraGame>();

		private int _myIndex = -1;

		[NonSerialized]
		public Camera myCamera;

		public AudioReverbFilter audioReverbFilter;

		private PSXEffects pSXEffects;

		private float shakeMagnitude;

		private float shakeMagnitudeDecaySpeed = 8f;

		private bool shakePausable = true;

		private ChromaticAberration chromaticAberrationSetting;

		private float chromaticAberrationIntensity;

		private float chromaticAberrationIntensityDecaySpeed = 8f;

		private Bloom bloomSetting;

		private float bloomIntensity;

		private float bloomIntensityDecaySpeed = 8f;

		private MotionBlur motionBlurSetting;

		[NonSerialized]
		public PostProcessVolume postProcessVolume;

		[NonSerialized]
		public PostProcessProfile postProcessProfile;

		private float fieldOfViewDefault = 60f;

		private Dictionary<string, CameraGame.FovEntry> fovExtraEntries = new Dictionary<string, CameraGame.FovEntry>();

		private List<CameraGame.FovEntry> _fovExtraEntriesList = new List<CameraGame.FovEntry>();

		private float fovExtraTotal;

		public bool cullOutMyLayer = true;

		public bool cullOutUICameras = true;

		private class FovEntry
		{
			public float value;

			public float decaySpeed = 1f;

			public bool pausable = true;
		}
	}
}
