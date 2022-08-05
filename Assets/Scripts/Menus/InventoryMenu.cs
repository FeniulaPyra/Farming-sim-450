using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : TogglableMenu
{
	public Inventory inv;

	public GameObject InventoryMenuObject;

	private GameObject[,] InventorySlots;
	public GameObject InventorySlotPrefab;

	public GameObject ItemManagerObject;
	ItemManager i;

	public GameObject ItemGrabber;
	InventoryItemGrabber grabbedItem;

	// Start is called before the first frame update
	void Start()
	{
        ItemManagerObject = GameObject.Find("ItemManager");
		i = ItemManagerObject.GetComponent<ItemManager>();
		grabbedItem = ItemGrabber.GetComponent<InventoryItemGrabber>();
	}

	public void ClearMenu()
	{
		if (InventorySlots == null || inv == null) return;
		for (int r = 0; r < inv.ROWS; r++)
		{
			for (int c = 0; c < inv.COLUMNS; c++)
			{
				GameObject slot = InventorySlots[r, c];
				if (slot == null) return;
				Destroy(slot);
			}
		}
	}

	public void SetInventoryToDisplay(Inventory inventory)
	{
		inv = inventory;
		InventorySlots = new GameObject[inv.ROWS, inv.COLUMNS];
		for (int r = 0; r < inv.ROWS; r++)
		{
			for (int c = 0; c < inv.COLUMNS; c++)
			{
				//the inventory slot button
				GameObject slot = InventorySlots[r, c] = Instantiate(InventorySlotPrefab);
				slot.transform.position = new Vector2(LEFT + (c + 1) * SLOT_SIZE - SLOT_GAP, TOP + (r + 1) * SLOT_SIZE - SLOT_GAP);

				//the item in the corresponding slot in the inventory object
				Item currentSlotItem = inv.GetSlotItem(r, c);
				int currentSlotAmount = inv.GetSlotAmount(r, c);

				InventorySlot slotScript = slot.GetComponent<InventorySlot>();
				slotScript.SetItem(currentSlotItem, currentSlotAmount);
				slotScript.UpdateDisplay();

				//this is here because of the weird way looping affects adding onclick listeners
				int x = r, y = c;

				//adds click event listener to select slot.
				slot.GetComponent<Button>().onClick.AddListener(delegate {
					Debug.Log("Clicked Inventory button");

					//if there is a grabbed item
					if (grabbedItem != null)
					{
						//...and it is the same item as teh one in the slot
						if (grabbedItem.item == inv.GetSlotItem(x, y))
						{
							//add to slot
							inv.AddToSlot(grabbedItem.amount, x, y);
							grabbedItem.item = null;
							grabbedItem.amount = 0;
						}
						else
						{
							//swap them 
							Item grabbed = grabbedItem.item;
							int amount = grabbedItem.amount;

							grabbedItem.item = inv.GetSlotItem(x, y);
							grabbedItem.amount = inv.GetSlotAmount(x, y);

							inv.SetItem(x, y, grabbed, amount);
						}
					}
					else
					{
						if (inv.GetSlotItem(x, y) != null && inv.GetSlotAmount(x, y) > 0)
						{
							grabbedItem.item = inv.GetSlotItem(x, y);
							grabbedItem.amount = inv.GetSlotAmount(x, y);
							inv.DeleteSlot(x, y);
						}
					}
					
					slot.GetComponent<InventorySlot>().UpdateDisplay();
				});
				UpdateDisplay();
				//adds the slot to the inventory menu
				slot.transform.SetParent(InventoryMenuObject.transform, false);
			}
		}
	}

	public void UpdateDisplay()
	{
		if (inv == null) return;
		for(int r = 0; r < inv.ROWS; r++)
		{
			for(int c = 0; c < inv.COLUMNS; c++)
			{
				GameObject g = InventorySlots[r, c];
				if (g == null) continue;
				InventorySlot slot = g.GetComponent<InventorySlot>();
				slot.SetItem(inv.GetSlotItem(r, c), inv.GetSlotAmount(r, c));
				slot.UpdateDisplay();
			}
		}
	}
}
