using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
	private int slotIndex;
	private Inventory inventory;

	public void SetIndex(int slotIndex, Inventory inventory) 
	{ 
		this.slotIndex = slotIndex;
		this.inventory = inventory;
	}

	public void ShowLabel()
	{
		gameObject.transform.GetChild(2).gameObject.SetActive(true);
	}
	public void HideLabel()
	{
		gameObject.transform.GetChild(2).gameObject.SetActive(false);
	}

	public void SelectSlot()
    {
		inventory.HoldItem(slotIndex);
	}
}
