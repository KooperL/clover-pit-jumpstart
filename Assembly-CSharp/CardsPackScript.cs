using System;
using Panik;
using UnityEngine;

public class CardsPackScript : MonoBehaviour
{
	// Token: 0x06000948 RID: 2376 RVA: 0x0003D7D8 File Offset: 0x0003B9D8
	private void Awake()
	{
		this.memPackDealUiScr = base.GetComponentInParent<MemoryPackDealUI>(true);
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x0003D7E8 File Offset: 0x0003B9E8
	private void Animator_ShowCards()
	{
		this.memPackDealUiScr.Pack_ShowCards();
		CameraGame.Shake(10f);
		CameraGame.ChromaticAberrationIntensitySet(2f);
		FlashScreen.SpawnCamera(Color.red, 1f, 2f, CameraUiGlobal.instance.myCamera, 0.5f);
		Sound.Play("SoundPackPunchOpen", 1f, 1f);
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x0003D84D File Offset: 0x0003BA4D
	private void Animator_HidePack()
	{
		this.memPackDealUiScr.Pack_Hide();
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x0003D85C File Offset: 0x0003BA5C
	private void Animator_PackPunch()
	{
		CameraGame.Shake(1f);
		FlashScreen.SpawnCamera(Color.red, 0.2f, 2f, CameraUiGlobal.instance.myCamera, 1f);
		Sound.Play("SoundPackPunch", 1f, Random.Range(0.9f, 1.1f));
	}

	private MemoryPackDealUI memPackDealUiScr;
}
