using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
	public List<GameObject> itemPrefabs;

	private List<Item> itemList;

	public Dictionary<string, Item> itemDict;

	private void Start()
	{
		foreach(GameObject iObj in itemPrefabs)
		{
			Item i = iObj.GetComponent<Item>();
			itemList.Add(i);
			itemDict.Add(i.name, i);
		}
	}
	private void Update()
	{
	}


	public Item GetItemByID(int id)
	{
		return itemList[id];
	}
	public Item GetItemByName(string name)
	{
		return itemDict[name];
	}
}
