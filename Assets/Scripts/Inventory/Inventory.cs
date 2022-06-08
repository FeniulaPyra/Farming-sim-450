using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Inventory
{
	public int STACK_SIZE = 99;
	public int ROWS; //4
	public int COLUMNS; //9

	public GameObject player;
	public PlayerInventoryManager playerInvManager;

	private ItemSlot[,] items;

	public bool isShown;

    public int selectedRow = 0;
    public int selectedColumn = 0;

	/// <summary>
	/// Creates a new inventory object, with a given number of rows and columns.
	/// </summary>
	/// <param name="rows">The number of rows in the inventory</param>
	/// <param name="columns">The number of columns in the inventory</param>
	/// <param name="stackSize">The maximum number of an item each slot can store.</param>
	public Inventory(int rows, int columns, int stackSize = 99)
	{
		playerInvManager = GameObject.Find("Player").GetComponent<PlayerInventoryManager>(); 
		ROWS = rows;
		COLUMNS = columns;
		items = new ItemSlot[ROWS,COLUMNS];
		for(int r = 0; r < ROWS; r++) 
		{
			for(int c = 0; c < COLUMNS; c++)
			{
				items[r,c] = new ItemSlot(null, 0);
			}
		}
		STACK_SIZE = stackSize;
	}

	/// <summary>
	/// Gives whatever is in the given slot
	/// </summary>
	/// <param name="row">the row of the slot</param>
	/// <param name="column">the column of the slot</param>
	/// <returns>The ItemStack in the given slot</returns>
	public Item GetSlotItem(int row, int column)
	{
		return items[row, column].item;
	}
	public int GetSlotAmount(int row, int column)
	{
		return items[row, column].amt;
	}

	/// <summary>
	/// Adds the given stack of items to the inventory. Any extras will 
	/// be dropped on the ground if the inventory is closed or selected if the inventory is open.
	/// </summary>
	/// <param name="item">an ItemStack of the items to be added</param>
	private void AddItems(ItemSlot item)
	{
		Vector2Int openSlot = new Vector2Int(-1, -1);
		for (int r = 0; r < ROWS; r++)
		{
			for (int c = 0; c < COLUMNS; c++)
			{
				ItemSlot i = items[r, c];

				if(i.item != null && item.item != null && i.amt > 0 && i.item.name == item.item.name && i.amt < STACK_SIZE)
				{
					Debug.Log("LEP2738 CAN ADD ITEM");
					//if they are the same item and they add up to a smaller amount than the stack size
					//ItemSlot leftovers = i.CombineStacks(item, STACK_SIZE);

					int stackLeftovers = i.amt + item.amt - STACK_SIZE;
					//ex combining a 50 stack with a 68 stack:
					//stackleftovers  = 50 + 68 - 99 = 118 - 99 = 19
					//therefore, there should be an extra stack left if stack leftovers are positive.

					if (stackLeftovers > 0) //if there are leftovers
					{
						items[r, c].amt = 99;
						AddItems(i.item, stackLeftovers);
					}
					else
					{
						items[r, c].amt = i.amt + item.amt;
					}
					return;
				}
				//if there is an empty slot, set it as a backup slot
				else if (i.amt == 0 && openSlot.x < 0)
				{
					openSlot = new Vector2Int(r, c);
				}
			}
		}

		//if there is no more room, drop item on the ground.
		if (openSlot.x < 0 || openSlot.y < 0)
		{
			playerInvManager.DropItems(item.item, item.amt);
		}
		else
		{
			//if there wasn't already a slot with this type of item, add it to the empty slot
			items[openSlot.x, openSlot.y].item = item.item;
			items[openSlot.x, openSlot.y].amt = item.amt;
		}

	}

	/// <summary>
	/// Adds the given amount of a given item to the inventory.
	/// </summary>
	/// <param name="item">The item to add</param>
	/// <param name="amount">the amount of the item to be added</param>
	public void AddItems(Item item, int amount)
	{
		this.AddItems(new ItemSlot(item, amount));
	}

	/// <summary>
	/// Checks if the inventory is too full to recieve an item
	/// </summary>
	/// <param name="items">the items the user wants to put in the inventory</param>
	/// <returns>true if the inventory is too full, false if there is space.</returns>
	public bool IsTooFull(Item i, int amt)
	{
		//check if inventory is too full to fit new item

		for(int r =0; r < ROWS; r++)
		{
			for(int c = 0; c < COLUMNS; c++)
			{
				ItemSlot slot = items[r, c];
				//is there an open space in the inventory?
				if(slot.amt < 1)
				{
					return false;
				}
				//is there a stack that has space to add items?
				if(slot.item.name == i.name && slot.amt + amt <= STACK_SIZE)
				{
					return false;
				}
			}
		}
		return true;
	}

	/// <summary>
	/// Returns the items in the row currently selected as the hotbar
	/// </summary>
	/// <returns>an array of ItemStacks representing the current hotbar items</returns>
	public Dictionary<Item, int> GetRowItems(int rowNumber)
	{
		Dictionary<Item, int> row = new Dictionary<Item, int>();
		for (int i = 0; i < COLUMNS; i++)
		{
			ItemSlot slot = items[rowNumber, COLUMNS];
			row.Add(slot.item, slot.amt);
		}

		return row;
	}

	/// <summary>
	/// Sets a slot of the inventory to a given set of items. Will skip if 
	/// the slot x and y are out of bounds.
	/// </summary>
	/// <param name="slotX"> The column of the slot to be set</param>
	/// <param name="slotY"> The row of the slot to be set</param>
	/// <param name="newItems">The items the slot will be set to</param>
	public void SetItem(int slotX, int slotY, Item newItem, int amount)
	{
		//exit if OOB
		if (slotX >= ROWS || slotX < 0 || slotY >= COLUMNS || slotY < 0) return;
		items[slotX, slotY].item = newItem;
		items[slotX, slotY].amt = amount;
	}

	/// <summary>
	/// Deletes the items at the given slot positino
	/// </summary>
	/// <param name="slotX">the column of the slot to be cleared</param>
	/// <param name="slotY">the row of the slot to be cleared</param>
	public void DeleteSlot(int slotX, int slotY)
	{
		items[slotX, slotY].Clear();
	}

	public void RemoveItems(Item item, int amount)
	{
		//all the slots with the given items
		List<int[]> slotsWithItems = new List<int[]>();
		int totalItems = 0;
		for(int r = 0; r < ROWS; r++)
		{
			for(int c = 0; c < COLUMNS; c++)
			{
				if(items[r,c].item != null && items[r, c].item.name == item.name)
				{
					int[] slot = { r, c };
					slotsWithItems.Add(slot);
					totalItems += items[r, c].amt;
				}
			}
		}

		//throws an error if there isnt enough items in the inventory;
		if(totalItems < amount)
		{
			throw new ArgumentException("Not enough items");
		}

		//removes the items
		int leftovers = amount;
		foreach(int[] s in slotsWithItems)
		{
			ItemSlot i = items[s[0], s[1]];
			if (i.item.name == item.name)
			{
				items[s[0], s[1]].amt -= leftovers;
				leftovers -= items[s[0], s[1]].amt;

				if(leftovers == 0)
				{
					DeleteSlot(s[0], s[1]);
				}
				if (leftovers > 0) break;
				if (leftovers < 0) leftovers *= -1;
			}
		}
	}
	
	/// <summary>
	/// Removes a given amount of items from a given slot. If there are less items in
	/// the slot than are trying to be removed, does not remove the items and returns false.
	/// </summary>
	/// <param name="amount">the amount of items to remove</param>
	/// <param name="row">the row of the item slot to remove from</param>
	/// <param name="column">the column of the item slot to remove from</param>
	/// <returns>true if the items were successfully removed. false if they were not - i.e. if there were not enough items in the slot</returns>
	public bool RemoveFromSlot(int amount, int row, int column)
	{
		ItemSlot i = items[row, column];
		if (amount < 0) amount *= -1;
		if (i.item != null && i.amt >= amount)
		{
			items[row, column].amt -= amount;
			if(i.amt <= 0)
			{
				items[row, column].Clear();
			}
			return true;
		}
		else if (i.amt < amount)
		{
			throw new Exception("Not enough items in that slot.");
		}
		return false;
	}
	
	/// <summary>
	/// Adds a given amount of items to the slot at the given position. if the amount
	/// exceeds the stack size, returns a positive number. IF THE NUMBER IS POSITIVE, 
	/// PLEASE DO SOMETHING WITH THE EXCESS ITEMS YOU TRIED TO ADD (drop them on the ground,
	/// add them to the rest of the inventory using AddItems, etc)
	/// </summary>
	/// <param name="amount">amount of items to add</param>
	/// <param name="row">the row of the slot to add items to</param>
	/// <param name="column">the column of the slot to add items to</param>
	/// <returns></returns>
	public int AddToSlot(int amount, int row, int column)
	{
		ItemSlot i = items[row, column];
		if (amount < 0) amount *= -1;
		if (i.item != null && amount > 0)
		{
			if (i.amt + amount <= STACK_SIZE)
			{
				items[row, column].amt += amount;
			}
			else
			{
				items[row, column].amt = 99;
			}
			return 99 - i.amt - amount;
		}
		return amount;
	}
	
	/// <summary>
	/// Returns the inventory as a 2d array of 
	/// arrays containging item ids and ammounts.
	/// </summary>
	/// <returns>An array with ids and amounts formatted as [itemid, amount]</returns>
	public List<int> GetSaveableInventory()
	{
		List<int> sinv = new List<int>(); //len should be row * col * 2;
		//first two ints are row and column
		sinv.Add(ROWS);
		sinv.Add(COLUMNS);

		for (int r = 0; r < ROWS; r++)
		{
			for (int c = 0; c < COLUMNS; c++)
			{
				ItemSlot i = items[r, c];
				if(i.item == null)
				{
					sinv.Add(-1);
					sinv.Add(0);
				}
				else
				{
					sinv.Add(playerInvManager.GetItemManager().GetItemID(i.item));
					sinv.Add(i.amt);
				}
			}
		}

		return sinv;
	}

	/// <summary>
	/// takes the same thing the other function returns and turns it into
	/// an actual invengtory
	/// </summary>
	/// <param name="sinv">the saved inventoyr</param>
	public void SetSaveableInventory(List<int> sinv)
	{
		//Menu menu = GameObject.Find("Menus").GetComponent<Menu>();
		//List<Item> itemsDict = menu.GetGameItemList();
		ROWS = sinv[0];
		COLUMNS = sinv[1];
		items = new ItemSlot[ROWS, COLUMNS];
		for(int i = 2 , j = 0; i < sinv.Count - 1; i+=2, j++) //j is there to represent the actual item pos in the inventory because i am too lazy to do simple math :)
		{
			
            Debug.Log($"sinv[{i}]: {sinv[i]}");
			//sinv[i] = item, sinv[i+1] = amount
			int r = (int)Math.Floor((double)(j / COLUMNS));
			int c = j % COLUMNS;

			//if slot is empty
			if (sinv[i] < 0 || sinv[i + 1] <= 0)
			{
				items[r, c] = new ItemSlot(null, 0);
			}
			else
			{
				ItemSlot slot = new ItemSlot(playerInvManager.GetItemManager().gameItems[sinv[i]], sinv[i + 1]);
				items[r, c] = slot;
			}

		}

	}

	/// <summary>
	/// Counts the number of a given item and returns its amount
	/// </summary>
	/// <param name="i">item to count</param>
	/// <returns>amount of item in inventory</returns>
	public int CountItem(Item i)
	{
		return CountItem(i.name);
	}

	/// <summary>
	/// Counts the number of a given item and returns its amount
	/// </summary>
	/// <param name="name">name of item to count</param>
	/// <returns>amount of item in inventory</returns>
	public int CountItem(String name)
	{
		int amt = 0;
		for (int r = 0; r < ROWS; r++)
		{
			for (int c = 0; c < COLUMNS; c++)
			{
				
				ItemSlot slot = this.items[r, c];
				if (slot.item != null && slot.item.name == name)
					amt += slot.amt;
			}
		}
		return amt;
	}
}

struct ItemSlot
{
	public Item item;
	public int amt;

	public ItemSlot(Item i, int amt)
	{
		item = i;
		this.amt = amt;
	}

	public bool Equals(ItemSlot i) {
		return item.name == i.item.name;
	}
	public void Clear()
	{
		item = null;
		amt = 0;
	}
}