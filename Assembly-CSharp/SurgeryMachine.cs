using System;
using UnityEngine;

// Token: 0x02000080 RID: 128
public class SurgeryMachine : MonoBehaviour
{
	// Token: 0x06000879 RID: 2169 RVA: 0x0000CAE0 File Offset: 0x0000ACE0
	private void Start()
	{
		this.surgeryMachineHolder.SetActive(false);
	}

	// Token: 0x04000811 RID: 2065
	public GameObject surgeryMachineHolder;
}
