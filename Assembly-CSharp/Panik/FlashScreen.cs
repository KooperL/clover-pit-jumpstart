using System;
using UnityEngine;
using UnityEngine.UI;

namespace Panik
{
	public class FlashScreen : MonoBehaviour
	{
		// Token: 0x06000DAD RID: 3501 RVA: 0x00055C0D File Offset: 0x00053E0D
		public static void PositionResetToDefault()
		{
			FlashScreen.spawnPosition = FlashScreen.spawnPositionDefault;
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00055C19 File Offset: 0x00053E19
		public static void PositionSet(Vector3 position)
		{
			FlashScreen.spawnPosition = position;
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x00055C21 File Offset: 0x00053E21
		public static Vector3 PositionGet()
		{
			return FlashScreen.spawnPosition;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x00055C28 File Offset: 0x00053E28
		public static Vector3 PositionDefaultGet()
		{
			return FlashScreen.spawnPositionDefault;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00055C2F File Offset: 0x00053E2F
		public static void PositionDefaultSet(Vector3 position)
		{
			FlashScreen.spawnPositionDefault = position;
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00055C38 File Offset: 0x00053E38
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

		// Token: 0x06000DB3 RID: 3507 RVA: 0x00055CFD File Offset: 0x00053EFD
		public static FlashScreen SpawnWorld(Color color, float alpha, float alphaDecaySpeed, Vector3 position)
		{
			FlashScreen.PositionSet(position);
			return FlashScreen.SpawnEx(color, alpha, alphaDecaySpeed, null, 0f, false);
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00055D14 File Offset: 0x00053F14
		public static FlashScreen SpawnCamera(Color color, float alpha, float alphaDecaySpeed, Camera targetCamera, float cameraDistance = 0.5f)
		{
			return FlashScreen.SpawnEx(color, alpha, alphaDecaySpeed, targetCamera, cameraDistance, false);
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00055D22 File Offset: 0x00053F22
		private void Awake()
		{
			this.pausableBackup = this.pausable;
			this.myCanvas = base.GetComponent<Canvas>();
			this.myImage = base.GetComponentInChildren<Image>();
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00055D48 File Offset: 0x00053F48
		private void OnEnable()
		{
			this.pausable = this.pausableBackup;
			FlashScreen.instanceLast = this;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x00055D5C File Offset: 0x00053F5C
		private void OnDisable()
		{
			if (FlashScreen.instanceLast == this)
			{
				FlashScreen.instanceLast = null;
			}
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00055D74 File Offset: 0x00053F74
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
