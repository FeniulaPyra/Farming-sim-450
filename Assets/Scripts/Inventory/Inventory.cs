using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
	public static int STACK_SIZE = 99;
	public static int ROWS = 4;
	public static int COLUMNS = 9;

	public int hotbarRowNumber;
	public int slotHeld;

	public GameObject player;

	public bool isShown;

	private ItemStack[,] items;

	public ItemStack selectedStack;

	public ItemStack SelectedStack
	{
		get{return selectedStack;}
	}

	/// <summary>
	/// The itemstacks in the currently selected hotbar row
	/// </summary>
	public ItemStack[] SelectedHotbar
	{
		get
		{
			ItemStack[] row = new ItemStack[COLUMNS];
			for (int i = 0; i < COLUMNS; i++)
			{
				row[i] = items[hotbarRowNumber, i];
			}

			return row;
		}
	}
	/// <summary>
	/// The Currently held item
	/// </summary>
	public ItemStack HeldItem
	{
		get
		{
			return SelectedHotbar[slotHeld];
		}
	}

	public Inventory()
	{
		items = new ItemStack[ROWS,COLUMNS];
		selectedStack = null;
	}

	/// <summary>
	/// Picks up and selects the given Itemstack. To be used with SelectSlot.
	/// </summary>
	/// <param name="stack">The stack to select</param>
	public void SelectStack(ItemStack stack)
	{
		selectedStack = stack;
	}

	/// <summary>
	/// Selects the slot at the given row/column coordinates
	/// </summary>
	/// <param name="row">the row of the selected slot</param>
	/// <param name="column">the column of the selected slot</param>
	public void SelectSlot(int row, int column)
	{
		if (selectedStack != null)
		{
			SlotSelected(row, column);
		}
		else
		{
			selectedStack = items[row, column];
			items[row, column] = null;

		}
	}

	/// <summary>
	/// Gives whatever is in the given slot
	/// </summary>
	/// <param name="row">the row of the slot</param>
	/// <param name="column">the column of the slot</param>
	/// <returns>The ItemStack in the given slot</returns>
	public ItemStack GetSlot(int row, int column)
	{
		return items[row, column];
	}

	/// <summary>
	/// Adds the selected itemstack to the specified inventory slot.
	/// </summary>
	/// <param name="row">The row position of the selected slot.</param>
	/// <param name="column">The column position of the selected slot.</param>
	public void SlotSelected(int row, int column)
	{
		//ignores if user has not selected a stack yet.
		if (selectedStack == null) return;

		//if the slot is empty, 
		if(items[row, column] == null)
		{
			items[row, column] = selectedStack;
			selectedStack = null;
		}
		else
		{
			//tries to combine the stacks
			try
			{
				selectedStack = items[row, column].CombineStacks(selectedStack, STACK_SIZE);
			}
			//if they are not the same item type, swap the selected stack with the one in the slot.
			catch(System.ArgumentException ae)
			{
				Debug.Log(ae);
				ItemStack temp = selectedStack;
				selectedStack = items[row, column];
				items[row, column] = temp;
			}
		}
	}

	/// <summary>
	/// Adds the given stack of items to the inventory. Any extras will 
	/// be dropped on the ground if the inventory is closed or selected if the inventory is open.
	/// </summary>
	/// <param name="item">an ItemStack of the items to be added</param>
	public void AddItems(ItemStack item)
	{
		for(int r = 0; r < 4; r++)
		{
			for(int c = 0; c < 9; c++)
			{
				ItemStack i = items[r, c];

				if (i == null)
				{
					items[r, c] = item;
					return;
				}
				else if(i.Item == item.Item && i.Amount < STACK_SIZE)
				{
					//if they are the same item and they add up to a smaller amount than the stack size
					ItemStack leftovers = i.CombineStacks(item, STACK_SIZE);

					if (leftovers != null && leftovers.Amount > 0)
					{
						AddItems(leftovers);
					}
					return;
				}
			}
		}
	}

	/// <summary>
	/// Adds the given amount of a given item to the inventory.
	/// </summary>
	/// <param name="item">The item to add</param>
	/// <param name="amount">the amount of the item to be added</param>
	public void AddItems(Item item, int amount)
	{
		this.AddItems(new ItemStack(item, amount));
	}

	public bool IsTooFull(ItemStack items)
	{
		//check if inventory is too full to fit new item
		return true;
	}

	/// <summary>
	/// This row is selected as the hotbar.
	/// selecting the row does not change its order in the inventory array.
	/// </summary>
	public void selectHotbar(int rowNumber)
	{
		this.hotbarRowNumber = rowNumber;

	}

	/// <summary>
	/// Selects the next row in the inventory. Loops around - if the user 
	/// has the last row selected and tries to select the next row, the first
	/// row in the inventory will be selected instead.
	/// </summary>
	public void selectNextHotbar()
	{
		hotbarRowNumber = (hotbarRowNumber + 1) % ROWS;
	}

	/// <summary>
	/// Selects the previous row in the inventory. Loops around - if the user 
	/// has the first row selected aand tries to select the previous row, the 
	/// last row in the inventory will be selected instead
	/// </summary>
	public void selectPreviousHotbar()
	{
		hotbarRowNumber = (hotbarRowNumber - 1);
		if (hotbarRowNumber < 0)
			hotbarRowNumber = ROWS - 1;
	}

	/// <summary>
	/// Returns the items in the row currently selected as the hotbar
	/// </summary>
	/// <returns>an array of ItemStacks representing the current hotbar items</returns>
	public ItemStack[] GetHotbarItems()
	{
		ItemStack[] row = new ItemStack[COLUMNS];
		for (int i = 0; i < COLUMNS; i++)
		{
			row[i] = items[hotbarRowNumber, COLUMNS];
		}

		return row;
	}

	/// <summary>
	/// Selects the given slot of the hotbar for the player to hold or equip
	/// </summary>
	/// <param name="hotbarSlot">The column in the hotbar row of the item for the player to hold</param>
	public void HoldItem(int hotbarSlot)
	{
		if (hotbarSlot < 0) hotbarSlot = 0;
		if (hotbarSlot > COLUMNS) hotbarSlot = COLUMNS - 1;
		slotHeld = hotbarSlot; 
	}

	/// <summary>
	/// Rotates to the next item in the hotbar. Loops around - if the user
	/// is holding the last item in the hotbar and attempts to select the
	/// next item, the first item in the hotbar will be selected
	/// </summary>
	public void HoldNextItem()
	{
		slotHeld = (slotHeld + 1) % COLUMNS;
	}

	/// <summary>
	/// Rotates to the previous item in the hotbar. Loops around - if the user
	/// is holding the first item in the hotbar and attempts to select the
	/// next item, the last item in the hotbar will be selected
	/// </summary>
	public void HoldPreviousItem()
	{
		slotHeld = (slotHeld - 1);
		if (slotHeld < 0)
			slotHeld = COLUMNS - 1;
	}

	public void DropItems() {

	}

	public void DropSelectedItems()
	{
		//GameObject.Instantiate
	}

	public void DropItemsInSlot()
	{

	}
}
