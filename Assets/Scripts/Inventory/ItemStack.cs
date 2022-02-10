using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemStack
{
	private Item item;
	public Item Item{
		get{return item;}
	}
	private int amount;
	public int Amount
	{
		get { return amount; }
	}

	public ItemStack(Item item, int count)
	{
		this.item = item;
		this.amount = count;
	}

	/// <summary>
	/// Adds a given item stack to this item stack
	/// </summary>
	/// <param name="stack">The itemstack to be added to this one.</param>
	/// <param name="maxStackSize">The maximum stack size for this item.</param>
	/// <returns>ItemStack of the leftover items.</returns>
	public ItemStack CombineStacks(ItemStack stack, int maxStackSize)
	{
		if(stack.Item.name != this.Item.name)
		{
			throw new ArgumentException("Stacks are of different items. Can not combine.");
		}

		int stackLeftovers = amount + stack.amount - maxStackSize;
		//ex combining a 50 stack with a 68 stack:
		//stackleftovers  = 50 + 68 - 99 = 118 - 99 = 19
		//therefore, there should be an extra stack left if stack leftovers are positive.

		if(stackLeftovers > 0) //if there are leftovers
		{
			this.amount = 99;
			return new ItemStack(this.Item, stackLeftovers);
		}
		else
		{
			this.amount = amount + stack.amount;
			return null;
		}
	}

	/// <summary>
	/// Splits this stack into two halves.
	/// </summary>
	/// <returns>The ItemStack with the other half of this stack.</returns>
	public ItemStack SplitStack()
	{
		int halfStack = (int)(this.amount / 2f);
		this.amount -= halfStack;
		return new ItemStack(this.Item, halfStack);
	}
}
