using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Panik
{
	// Token: 0x02000117 RID: 279
	public class CameraGame : MonoBehaviour
	{
		// Token: 0x06000D31 RID: 3377 RVA: 0x00010BD3 File Offset: 0x0000EDD3
		public int CameraIndex()
		{
			if (this._myIndex < 0)
			{
				this._myIndex = CameraGame.list.IndexOf(this);
			}
			return this._myIndex;
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x00010BF5 File Offset: 0x0000EDF5
		public void UpdateRenderingTexture()
		{
			if (!Master.instance.RENDER_TO_TEXTURE)
			{
				this.myCamera.targetTexture = null;
				return;
			}
			this.myCamera.targetTexture = RenderingMaster.renderTextureCurrent;
		}

		// Token: 0x06000D33 RID: 3379 RVA: 0x00010C20 File Offset: 0x0000EE20
		public static void Shake(float magnitude)
		{
			if (!Data.settings.screenshakeEnabled)
			{
				return;
			}
			CameraGame.firstInstance.shakeMagnitude = Mathf.Max(magnitude, CameraGame.firstInstance.shakeMagnitude);
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x00010C49 File Offset: 0x0000EE49
		public static void ShakeDecaySpeedSet(float speed)
		{
			CameraGame.firstInstance.shakeMagnitudeDecaySpeed = speed;
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x00010C56 File Offset: 0x0000EE56
		public static float shakeGet()
		{
			return CameraGame.firstInstance.shakeMagnitude;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00010C62 File Offset: 0x0000EE62
		public static float ShakeDecaySpeedGet()
		{
			return CameraGame.firstInstance.shakeMagnitudeDecaySpeed;
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x00010C6E File Offset: 0x0000EE6E
		public static bool ShakePausableGet()
		{
			return CameraGame.firstInstance.shakePausable;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00010C7A File Offset: 0x0000EE7A
		public static void ShakePausableSet(bool pausable)
		{
			CameraGame.firstInstance.shakePausable = pausable;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x000656EC File Offset: 0x000638EC
		private void ScreenshakeUpdate()
		{
			this.shakeMagnitude = Mathf.Max(0f, this.shakeMagnitude - this.shakeMagnitudeDecaySpeed * Tick.Time);
			base.transform.SetLocalZAngle(global::UnityEngine.Random.Range(-this.shakeMagnitude, this.shakeMagnitude));
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00010C87 File Offset: 0x0000EE87
		public static void ChromaticAberrationIntensitySet(float intensity)
		{
			if (!Data.settings.chromaticAberrationEnabled)
			{
				CameraGame.firstInstance.chromaticAberrationIntensity = 0f;
				return;
			}
			CameraGame.firstInstance.chromaticAberrationIntensity = Mathf.Max(intensity, CameraGame.firstInstance.chromaticAberrationIntensity);
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00010CBF File Offset: 0x0000EEBF
		public static void ChromaticAberrationIntensityDecaySpeedSet(float speed)
		{
			CameraGame.firstInstance.chromaticAberrationIntensityDecaySpeed = speed;
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00010CCC File Offset: 0x0000EECC
		public static float ChromaticAberrationIntensityGet()
		{
			return CameraGame.firstInstance.chromaticAberrationIntensity;
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00010CD8 File Offset: 0x0000EED8
		public static float ChromaticAberrationIntensityDecaySpeedGet()
		{
			return CameraGame.firstInstance.chromaticAberrationIntensityDecaySpeed;
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00010CE4 File Offset: 0x0000EEE4
		public static void BloomIntensitySet(float intensity)
		{
			if (!Data.settings.bloomEnabled)
			{
				CameraGame.firstInstance.bloomIntensity = 0f;
				return;
			}
			CameraGame.firstInstance.bloomIntensity = Mathf.Max(intensity, CameraGame.firstInstance.bloomIntensity);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00010D1C File Offset: 0x0000EF1C
		public static void BloomIntensityDecaySpeedSet(float speed)
		{
			CameraGame.firstInstance.bloomIntensityDecaySpeed = speed;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00010D29 File Offset: 0x0000EF29
		public static float BloomIntensityGet()
		{
			return CameraGame.firstInstance.bloomIntensity;
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x00010D35 File Offset: 0x0000EF35
		public static float BloomIntensityDecaySpeedGet()
		{
			return CameraGame.firstInstance.bloomIntensityDecaySpeed;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x0006573C File Offset: 0x0006393C
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

		// Token: 0x06000D43 RID: 3395 RVA: 0x0006591C File Offset: 0x00063B1C
		public void FieldOfViewDefaultUpdate()
		{
			int num = this.CameraIndex();
			this.fieldOfViewDefault = Data.settings.FovGet(num);
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x00010D41 File Offset: 0x0000EF41
		private void FovExtraEntryEnsure(string tag)
		{
			if (!this.fovExtraEntries.ContainsKey(tag))
			{
				this.fovExtraEntries.Add(tag, new CameraGame.FovEntry());
				this._fovExtraEntriesList.Add(this.fovExtraEntries[tag]);
			}
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x00065944 File Offset: 0x00063B44
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

		// Token: 0x06000D46 RID: 3398 RVA: 0x00010D79 File Offset: 0x0000EF79
		public static void FieldOfViewExtraSet(string tag, float extra)
		{
			CameraGame.firstInstance.FovExtraEntryEnsure(tag);
			CameraGame.firstInstance.fovExtraEntries[tag].value = extra;
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00010D9C File Offset: 0x0000EF9C
		public static void FieldOfViewExtraDecaySpeedSet(string tag, float speed)
		{
			CameraGame.firstInstance.FovExtraEntryEnsure(tag);
			CameraGame.firstInstance.fovExtraEntries[tag].decaySpeed = speed;
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00010DBF File Offset: 0x0000EFBF
		public static float FieldOfViewExtraGet()
		{
			return CameraGame.firstInstance.fovExtraTotal;
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00010DCB File Offset: 0x0000EFCB
		public static float FieldOfViewExtraDecaySpeedGet(string tag)
		{
			CameraGame.firstInstance.FovExtraEntryEnsure(tag);
			return CameraGame.firstInstance.fovExtraEntries[tag].decaySpeed;
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00010DED File Offset: 0x0000EFED
		public static bool FieldOfViewExtraPausableGet(string tag)
		{
			CameraGame.firstInstance.FovExtraEntryEnsure(tag);
			return CameraGame.firstInstance.fovExtraEntries[tag].pausable;
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00010E0F File Offset: 0x0000F00F
		public static void FieldOfViewExtraPausableSet(string tag, bool pausable)
		{
			CameraGame.firstInstance.FovExtraEntryEnsure(tag);
			CameraGame.firstInstance.fovExtraEntries[tag].pausable = pausable;
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x00065A60 File Offset: 0x00063C60
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

		// Token: 0x06000D4D RID: 3405 RVA: 0x00065B84 File Offset: 0x00063D84
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

		// Token: 0x06000D4E RID: 3406 RVA: 0x0000774E File Offset: 0x0000594E
		public void _SplitScreenUpdate()
		{
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x00065C4C File Offset: 0x00063E4C
		public static void UpdatePSXEffectsToSettings_All()
		{
			foreach (CameraGame cameraGame in CameraGame.list)
			{
				cameraGame.UpdatePSXEffectsToSettings();
			}
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00065C9C File Offset: 0x00063E9C
		private void UpdatePSXEffectsToSettings()
		{
			this.pSXEffects.vertexInaccuracy = (Data.settings.wobblyPolygons ? 64 : 0);
			this.pSXEffects.worldSpaceSnapping = Data.settings.wobblyPolygons;
			this.pSXEffects.camSnapping = Data.settings.wobblyPolygons;
			this.pSXEffects.UpdateProperties();
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00010E32 File Offset: 0x0000F032
		public static void SetColorDepth(int depth0_24)
		{
			CameraGame.firstInstance.pSXEffects.colorDepth = depth0_24;
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00065CFC File Offset: 0x00063EFC
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

		// Token: 0x06000D53 RID: 3411 RVA: 0x00010E44 File Offset: 0x0000F044
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

		// Token: 0x06000D54 RID: 3412 RVA: 0x00010E7F File Offset: 0x0000F07F
		private void OnDestroy()
		{
			if (CameraGame.firstInstance == this)
			{
				CameraGame.firstInstance = null;
			}
			CameraGame.list.Remove(this);
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00010EA0 File Offset: 0x0000F0A0
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

		// Token: 0x04000DCE RID: 3534
		public static CameraGame firstInstance = null;

		// Token: 0x04000DCF RID: 3535
		public static List<CameraGame> list = new List<CameraGame>();

		// Token: 0x04000DD0 RID: 3536
		private int _myIndex = -1;

		// Token: 0x04000DD1 RID: 3537
		[NonSerialized]
		public Camera myCamera;

		// Token: 0x04000DD2 RID: 3538
		public AudioReverbFilter audioReverbFilter;

		// Token: 0x04000DD3 RID: 3539
		private PSXEffects pSXEffects;

		// Token: 0x04000DD4 RID: 3540
		private float shakeMagnitude;

		// Token: 0x04000DD5 RID: 3541
		private float shakeMagnitudeDecaySpeed = 8f;

		// Token: 0x04000DD6 RID: 3542
		private bool shakePausable = true;

		// Token: 0x04000DD7 RID: 3543
		private ChromaticAberration chromaticAberrationSetting;

		// Token: 0x04000DD8 RID: 3544
		private float chromaticAberrationIntensity;

		// Token: 0x04000DD9 RID: 3545
		private float chromaticAberrationIntensityDecaySpeed = 8f;

		// Token: 0x04000DDA RID: 3546
		private Bloom bloomSetting;

		// Token: 0x04000DDB RID: 3547
		private float bloomIntensity;

		// Token: 0x04000DDC RID: 3548
		private float bloomIntensityDecaySpeed = 8f;

		// Token: 0x04000DDD RID: 3549
		private MotionBlur motionBlurSetting;

		// Token: 0x04000DDE RID: 3550
		[NonSerialized]
		public PostProcessVolume postProcessVolume;

		// Token: 0x04000DDF RID: 3551
		[NonSerialized]
		public PostProcessProfile postProcessProfile;

		// Token: 0x04000DE0 RID: 3552
		private float fieldOfViewDefault = 60f;

		// Token: 0x04000DE1 RID: 3553
		private Dictionary<string, CameraGame.FovEntry> fovExtraEntries = new Dictionary<string, CameraGame.FovEntry>();

		// Token: 0x04000DE2 RID: 3554
		private List<CameraGame.FovEntry> _fovExtraEntriesList = new List<CameraGame.FovEntry>();

		// Token: 0x04000DE3 RID: 3555
		private float fovExtraTotal;

		// Token: 0x04000DE4 RID: 3556
		public bool cullOutMyLayer = true;

		// Token: 0x04000DE5 RID: 3557
		public bool cullOutUICameras = true;

		// Token: 0x02000118 RID: 280
		private class FovEntry
		{
			// Token: 0x04000DE6 RID: 3558
			public float value;

			// Token: 0x04000DE7 RID: 3559
			public float decaySpeed = 1f;

			// Token: 0x04000DE8 RID: 3560
			public bool pausable = true;
		}
	}
}
