using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarMenu : TogglableMenu
{
	public GameObject HeldItemIndicator;

	private GameObject[] HotbarSlots;
	public GameObject InventorySlotPrefab;

	public GameObject playerObj;
	PlayerInventoryManager player;
	Inventory inv;

	public GameObject HotbarItemLabel;
	Text itemLabel;

	// Start is called before the first frame update
	void Start()
	{
		player = playerObj.GetComponent<PlayerInventoryManager>();
		inv = player.inv;

		HotbarSlots = new GameObject[inv.COLUMNS];

		itemLabel = HotbarItemLabel.GetComponent<Text>();
		itemLabel.text = "butthoels";

		for (int c = 0; c < inv.COLUMNS; c++)
		{
			//the inventory slot button
			GameObject slot = HotbarSlots[c] = Instantiate(InventorySlotPrefab);
			slot.transform.position = new Vector2(LEFT + (c + 1) * 74 - 10, TOP);

			InventorySlot slotScript = slot.GetComponent<InventorySlot>();
			slotScript.SetIndex(player, c);

			slot.transform.SetParent(gameObject.transform, false);
		}
		UpdateDisplay();
	}

	// Update is called once per frame
	void Update()
	{
		UpdateDisplay();
	}

	public void UpdateDisplay()
	{

		//updates all the slots to show the correct items.
		for (int c = 0; c < inv.COLUMNS; c++)
		{
			GameObject g = HotbarSlots[c];
			InventorySlot slot = g.GetComponent<InventorySlot>();
			slot.SetItem(inv.GetSlotItem(player.Hotbar, c), inv.GetSlotAmount(player.Hotbar, c));
			slot.UpdateDisplay();
		}

		//updates the info text with info about the held item
		Item heldItem = player.GetHeldItem();
		if (heldItem != null)
		{
			HotbarItemLabel.GetComponent<Text>().text = heldItem.name + "\n";

			//this is a temporary solution.
			if (heldItem.staminaUsed > 0)
			{
				HotbarItemLabel.GetComponent<Text>().text += $" (Use: -{heldItem.staminaUsed} Stamina)";
			}
			if (heldItem.isEdible)
			{
				HotbarItemLabel.GetComponent<Text>().text += $" (Eat: +{heldItem.staminaToRestore} Stamina)";
			}
		}
		else
		{
			HotbarItemLabel.GetComponent<Text>().text = "";
		}
		
		HeldItemIndicator.transform.position = HotbarSlots[player.HeldSlot].transform.position;

	}
}
