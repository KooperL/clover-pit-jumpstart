using System;
using Panik;
using UnityEngine;

public class CardsPackScript : MonoBehaviour
{
	// Token: 0x06000957 RID: 2391 RVA: 0x0003DB3C File Offset: 0x0003BD3C
	private void Awake()
	{
		this.memPackDealUiScr = base.GetComponentInParent<MemoryPackDealUI>(true);
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0003DB4C File Offset: 0x0003BD4C
	private void Animator_ShowCards()
	{
		this.memPackDealUiScr.Pack_ShowCards();
		CameraGame.Shake(10f);
		CameraGame.ChromaticAberrationIntensitySet(2f);
		FlashScreen.SpawnCamera(Color.red, 1f, 2f, CameraUiGlobal.instance.myCamera, 0.5f);
		Sound.Play("SoundPackPunchOpen", 1f, 1f);
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x0003DBB1 File Offset: 0x0003BDB1
	private void Animator_HidePack()
	{
		this.memPackDealUiScr.Pack_Hide();
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x0003DBC0 File Offset: 0x0003BDC0
	private void Animator_PackPunch()
	{
		CameraGame.Shake(1f);
		FlashScreen.SpawnCamera(Color.red, 0.2f, 2f, CameraUiGlobal.instance.myCamera, 1f);
		Sound.Play("SoundPackPunch", 1f, global::UnityEngine.Random.Range(0.9f, 1.1f));
	}

	private MemoryPackDealUI memPackDealUiScr;
}
