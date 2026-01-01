using System;
using Panik;
using UnityEngine;

// Token: 0x020000C8 RID: 200
public class CardsPackScript : MonoBehaviour
{
	// Token: 0x06000AAB RID: 2731 RVA: 0x0000E858 File Offset: 0x0000CA58
	private void Awake()
	{
		this.memPackDealUiScr = base.GetComponentInParent<MemoryPackDealUI>(true);
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x00055090 File Offset: 0x00053290
	private void Animator_ShowCards()
	{
		this.memPackDealUiScr.Pack_ShowCards();
		CameraGame.Shake(10f);
		CameraGame.ChromaticAberrationIntensitySet(2f);
		FlashScreen.SpawnCamera(Color.red, 1f, 2f, CameraUiGlobal.instance.myCamera, 0.5f);
		Sound.Play("SoundPackPunchOpen", 1f, 1f);
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0000E867 File Offset: 0x0000CA67
	private void Animator_HidePack()
	{
		this.memPackDealUiScr.Pack_Hide();
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x000550F8 File Offset: 0x000532F8
	private void Animator_PackPunch()
	{
		CameraGame.Shake(1f);
		FlashScreen.SpawnCamera(Color.red, 0.2f, 2f, CameraUiGlobal.instance.myCamera, 1f);
		Sound.Play("SoundPackPunch", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
	}

	// Token: 0x04000AD4 RID: 2772
	private MemoryPackDealUI memPackDealUiScr;
}
