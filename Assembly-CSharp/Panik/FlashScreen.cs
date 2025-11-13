using System;
using UnityEngine;
using UnityEngine.UI;

namespace Panik
{
	public class FlashScreen : MonoBehaviour
	{
		// Token: 0x06000DC4 RID: 3524 RVA: 0x000563E9 File Offset: 0x000545E9
		public static void PositionResetToDefault()
		{
			FlashScreen.spawnPosition = FlashScreen.spawnPositionDefault;
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x000563F5 File Offset: 0x000545F5
		public static void PositionSet(Vector3 position)
		{
			FlashScreen.spawnPosition = position;
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x000563FD File Offset: 0x000545FD
		public static Vector3 PositionGet()
		{
			return FlashScreen.spawnPosition;
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x00056404 File Offset: 0x00054604
		public static Vector3 PositionDefaultGet()
		{
			return FlashScreen.spawnPositionDefault;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0005640B File Offset: 0x0005460B
		public static void PositionDefaultSet(Vector3 position)
		{
			FlashScreen.spawnPositionDefault = position;
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x00056414 File Offset: 0x00054614
		public static FlashScreen SpawnEx(Color color, float alpha, float alphaDecaySpeed, Camera targetCamera, float cameraDistance, bool forceSpawn = false)
		{
			if (Data.SettingsData.inst.flashingLightsReducedEnabled && !forceSpawn)
			{
				return null;
			}
			FlashScreen component = Spawn.FromPool((targetCamera == null) ? "FlashScreenWorld" : "FlashScreenCamera", FlashScreen.spawnPosition, null).GetComponent<FlashScreen>();
			if (component != null)
			{
				component.alpha = alpha;
				component.alphaDecaySpeed = alphaDecaySpeed;
				component.color = color;
				component.color.a = alpha;
				component.myImage.color = component.color;
				component.pausable = component.pausableBackup;
				if (targetCamera != null)
				{
					component.myCanvas.worldCamera = targetCamera;
					component.myCanvas.planeDistance = cameraDistance;
					component.gameObject.layer = targetCamera.gameObject.layer;
				}
			}
			return component;
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x000564D9 File Offset: 0x000546D9
		public static FlashScreen SpawnWorld(Color color, float alpha, float alphaDecaySpeed, Vector3 position)
		{
			FlashScreen.PositionSet(position);
			return FlashScreen.SpawnEx(color, alpha, alphaDecaySpeed, null, 0f, false);
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x000564F0 File Offset: 0x000546F0
		public static FlashScreen SpawnCamera(Color color, float alpha, float alphaDecaySpeed, Camera targetCamera, float cameraDistance = 0.5f)
		{
			return FlashScreen.SpawnEx(color, alpha, alphaDecaySpeed, targetCamera, cameraDistance, false);
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x000564FE File Offset: 0x000546FE
		private void Awake()
		{
			this.pausableBackup = this.pausable;
			this.myCanvas = base.GetComponent<Canvas>();
			this.myImage = base.GetComponentInChildren<Image>();
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x00056524 File Offset: 0x00054724
		private void OnEnable()
		{
			this.pausable = this.pausableBackup;
			FlashScreen.instanceLast = this;
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x00056538 File Offset: 0x00054738
		private void OnDisable()
		{
			if (FlashScreen.instanceLast == this)
			{
				FlashScreen.instanceLast = null;
			}
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x00056550 File Offset: 0x00054750
		private void Update()
		{
			if (this.pausable && !Tick.IsGameRunning)
			{
				return;
			}
			this.alpha -= this.alphaDecaySpeed * Tick.Time;
			this.color.a = this.alpha;
			this.myImage.color = this.color;
			if (this.alpha <= 0f)
			{
				Pool.Destroy(base.gameObject, null);
			}
		}

		public static FlashScreen instanceLast;

		[NonSerialized]
		public bool pausable = true;

		private bool pausableBackup;

		private Canvas myCanvas;

		private Image myImage;

		private float alpha = 1f;

		private float alphaDecaySpeed = 1f;

		private Color color;

		private static Vector3 spawnPositionDefault = new Vector3(0f, 0f, -150f);

		private static Vector3 spawnPosition = new Vector3(0f, 0f, -150f);
	}
}
