using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	private int slotColumn;
	private Inventory inventory;
	private PlayerInventoryManager p;
	private Item item;
	private int amount;


	public GameObject IconObject;
	Image icon;

	public GameObject AmtLabelObject;
	Text amountLabel;

	public GameObject ItemLabelObject;
	Text itemLabel;

	private void Start()
	{
		icon = IconObject.GetComponent<Image>();
		amountLabel = AmtLabelObject.GetComponent<Text>();
		itemLabel = ItemLabelObject.GetComponent<Text>();
	}
	public void SetIndex(PlayerInventoryManager p, int column)//int slotIndex, Inventory inventory) 
	{
		this.p = p;
		this.slotColumn = column;
		this.inventory = p.inv;
	}

	public void ShowLabel()
	{
		itemLabel.gameObject.SetActive(true);
	}
	public void HideLabel()
	{
		itemLabel.gameObject.SetActive(false);
	}

	public void SetItem(Item i, int amt)
	{
		item = i;
		amount = amt;
	}

	public void UpdateDisplay()
	{
		if (amountLabel == null || itemLabel == null || icon == null) return;
		if (item != null)
		{
			amountLabel.text = "" + amount;
			itemLabel.text = "" + item.name;
			icon.sprite = item.spr;
			icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 100);

		}
		else
		{
			amountLabel.text = "";
			itemLabel.text = "";
			icon.sprite = null;
			icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0);
		}

	}
	public void SelectSlot()
	{
		p.HeldSlot = slotColumn;
	}
}
