using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
	// Start is called before the first frame update
	public void ShowLabel()
	{
		gameObject.transform.GetChild(2).gameObject.SetActive(true);
	}
	public void HideLabel()
	{
		gameObject.transform.GetChild(2).gameObject.SetActive(false);
	}
}
