using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShippingMenu : TogglableMenu
{
	public GameObject ShippingBin;
	Inventory inv;

	public GameObject[] ShippingBinSlots;

	public GameObject InventorySlotPrefab;

	public GameObject ItemGrabber;
	InventoryItemGrabber grabbedItem;

	public GameObject slotsBackgroundObject;


	// Start is called before the first frame update
	void Start()
	{
		RectTransform location = slotsBackgroundObject.GetComponent<RectTransform>();
		TOP = -170;
		LEFT = -180;

		inv = ShippingBin.GetComponent<ShippingBin>().inventory;
		ShippingBinSlots = new GameObject[inv.COLUMNS];
		grabbedItem = ItemGrabber.GetComponent<InventoryItemGrabber>();

		for (int c = 0; c < inv.COLUMNS; c++)
		{
			GameObject slot = ShippingBinSlots[c] = Instantiate(InventorySlotPrefab);
			slot.transform.position = new Vector2(LEFT + (c + 1) * SLOT_SIZE - SLOT_GAP, TOP);
			//the item in the corresponding slot in the inventory object
			Item currentSlotItem = inv.GetSlotItem(0, c);
			int currentSlotAmount = inv.GetSlotAmount(0, c);

			InventorySlot slotScript = slot.GetComponent<InventorySlot>();
			slotScript.SetItem(currentSlotItem, currentSlotAmount);
			slotScript.UpdateDisplay();

			//this is here because of the weird way looping affects adding onclick listeners
			int y = c;

			slot.GetComponent<Button>().onClick.AddListener(delegate
			{

				//if there is a grabbed item
				if (grabbedItem != null)
				{
					//...and it is the same item as teh one in the slot
					if (grabbedItem.item == inv.GetSlotItem(0, y))
					{
						//add to slot
						inv.AddToSlot(grabbedItem.amount, 0, y);
						grabbedItem.item = null;
						grabbedItem.amount = 0;
					}
					else
					{
						//swap them 
						Item grabbed = grabbedItem.item;
						int amount = grabbedItem.amount;

						grabbedItem.item = inv.GetSlotItem(0, y);
						grabbedItem.amount = inv.GetSlotAmount(0, y);

						inv.SetItem(0, y, grabbed, amount);
					}
				}
				else
				{
					if (inv.GetSlotItem(0, y) != null && inv.GetSlotAmount(0, y) > 0)
					{

						grabbedItem.item = inv.GetSlotItem(0, y);
						grabbedItem.amount = inv.GetSlotAmount(0, y);
						inv.DeleteSlot(0, y);
					}
				}
				//slot.GetComponent<InventorySlot>().UpdateDisplay();
				UpdateDisplay();
			});
			UpdateDisplay();
			slot.transform.SetParent(gameObject.transform, false);
		}
		UpdateDisplay();
	}

	public void UpdateDisplay()
	{
		if (inv == null) return;
		for (int c = 0; c < inv.COLUMNS; c++)
		{
			GameObject g = ShippingBinSlots[c];
			if (g == null) return;
			InventorySlot slot = g.GetComponent<InventorySlot>();
			slot.SetItem(inv.GetSlotItem(0, c), inv.GetSlotAmount(0, c));
			slot.UpdateDisplay();
		}
	}
	// Update is called once per frame
	void Update()
    {
        
    }
}
