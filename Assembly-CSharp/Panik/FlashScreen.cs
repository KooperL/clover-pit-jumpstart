using System;
using UnityEngine;
using UnityEngine.UI;

namespace Panik
{
	// Token: 0x0200017B RID: 379
	public class FlashScreen : MonoBehaviour
	{
		// Token: 0x06001151 RID: 4433 RVA: 0x00014225 File Offset: 0x00012425
		public static void PositionResetToDefault()
		{
			FlashScreen.spawnPosition = FlashScreen.spawnPositionDefault;
		}

		// Token: 0x06001152 RID: 4434 RVA: 0x00014231 File Offset: 0x00012431
		public static void PositionSet(Vector3 position)
		{
			FlashScreen.spawnPosition = position;
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x00014239 File Offset: 0x00012439
		public static Vector3 PositionGet()
		{
			return FlashScreen.spawnPosition;
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x00014240 File Offset: 0x00012440
		public static Vector3 PositionDefaultGet()
		{
			return FlashScreen.spawnPositionDefault;
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00014247 File Offset: 0x00012447
		public static void PositionDefaultSet(Vector3 position)
		{
			FlashScreen.spawnPositionDefault = position;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00073CA8 File Offset: 0x00071EA8
		public static FlashScreen SpawnEx(Color color, float alpha, float alphaDecaySpeed, Camera targetCamera, float cameraDistance, bool forceSpawn = false)
		{
			if (Data.SettingsData.inst.flashingLightsReducedEnabled)
			{
				color.r = 0f;
				color.g = 0f;
				color.b = 0f;
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

		// Token: 0x06001157 RID: 4439 RVA: 0x0001424F File Offset: 0x0001244F
		public static FlashScreen SpawnWorld(Color color, float alpha, float alphaDecaySpeed, Vector3 position)
		{
			FlashScreen.PositionSet(position);
			return FlashScreen.SpawnEx(color, alpha, alphaDecaySpeed, null, 0f, false);
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00014266 File Offset: 0x00012466
		public static FlashScreen SpawnCamera(Color color, float alpha, float alphaDecaySpeed, Camera targetCamera, float cameraDistance = 0.5f)
		{
			return FlashScreen.SpawnEx(color, alpha, alphaDecaySpeed, targetCamera, cameraDistance, false);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00014274 File Offset: 0x00012474
		private void Awake()
		{
			this.pausableBackup = this.pausable;
			this.myCanvas = base.GetComponent<Canvas>();
			this.myImage = base.GetComponentInChildren<Image>();
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x0001429A File Offset: 0x0001249A
		private void OnEnable()
		{
			this.pausable = this.pausableBackup;
			FlashScreen.instanceLast = this;
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x000142AE File Offset: 0x000124AE
		private void OnDisable()
		{
			if (FlashScreen.instanceLast == this)
			{
				FlashScreen.instanceLast = null;
			}
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x00073D8C File Offset: 0x00071F8C
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

		// Token: 0x04001244 RID: 4676
		public static FlashScreen instanceLast;

		// Token: 0x04001245 RID: 4677
		[NonSerialized]
		public bool pausable = true;

		// Token: 0x04001246 RID: 4678
		private bool pausableBackup;

		// Token: 0x04001247 RID: 4679
		private Canvas myCanvas;

		// Token: 0x04001248 RID: 4680
		private Image myImage;

		// Token: 0x04001249 RID: 4681
		private float alpha = 1f;

		// Token: 0x0400124A RID: 4682
		private float alphaDecaySpeed = 1f;

		// Token: 0x0400124B RID: 4683
		private Color color;

		// Token: 0x0400124C RID: 4684
		private static Vector3 spawnPositionDefault = new Vector3(0f, 0f, -150f);

		// Token: 0x0400124D RID: 4685
		private static Vector3 spawnPosition = new Vector3(0f, 0f, -150f);
	}
}
