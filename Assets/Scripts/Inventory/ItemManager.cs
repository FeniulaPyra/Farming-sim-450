using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : TogglableMenu
{
	public List<GameObject> gameItemPrefabs;
	public List<Item> gameItems;

	void Awake()
	{
		foreach (GameObject g in gameItemPrefabs)
		{
			gameItems.Add(g.GetComponent<Item>());
		}
	}

	public Item GetItemByName(string name)
	{
		name = name.ToUpper();
		foreach (Item i in gameItems)
		{
			if (i.name.ToUpper() == name)
				return i;
		}
		return null;
	}
	public GameObject GetPrefabByName(string name)
	{
		name = name.ToUpper();
		for(int i = 0; i < gameItems.Count; i++)
		{
			if (gameItems[i].name.ToUpper() == name)
				return gameItemPrefabs[i];
		}
		return null;
	}

	public GameObject FindPrefabByItem(Item item)
	{
		for (int i = 0; i < gameItems.Count; i++)
		{
			if (gameItems[i].name == item.name)
				return gameItemPrefabs[i];
		}
		return null;
	}

	public Item FindItemByID(int i)
	{
		return gameItems[i];
	}
	public GameObject FindPrefabByID(int i)
	{
		return gameItemPrefabs[i];
	}

	/// <summary>
	/// gets item id by item
	/// </summary>
	/// <param name="i">the item to find the id of</param>
	/// <returns>the id of the item or -1 if the item does not exist.</returns>
	public int GetItemID(Item i)
	{
		return GetItemID(i.name);
	}

	/// <summary>
	/// gets item id by item name
	/// </summary>
	/// <param name="name">the name of the item to find the id of</param>
	/// <returns>the id of the item or -1 if the item does not exist.</returns>
	public int GetItemID(string name)
	{
		name = name.ToUpper();
		for(int i = 0; i < gameItems.Count; i++)
		{
			if (gameItems[i].name.ToUpper() == name)
			{
				return i;
			}
		}
		return -1;
	}
}
