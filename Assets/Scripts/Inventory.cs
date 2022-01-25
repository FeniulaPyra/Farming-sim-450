using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
	public static int STACK_SIZE = 99;
	public static int ROWS = 4;
	public static int COLUMNS = 9;

	private ItemStack[,] items;

	public ItemStack selectedStack;

	public ItemStack SelectedStack
	{
		get{return selectedStack;}
	}

	public Inventory()
	{
		items = new ItemStack[ROWS,COLUMNS];
		selectedStack = null;
	}

	public void SelectStack(ItemStack stack)
	{
		selectedStack = stack;
	}

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

					Debug.Log(i.Amount);

					if (leftovers != null && leftovers.Amount > 0)
					{
						Debug.Log("leftovers=" + leftovers.Amount);
						AddItems(leftovers);
					}
					return;
				}
			}
		}
	}
}
