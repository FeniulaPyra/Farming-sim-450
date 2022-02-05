using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	public string name;
	public Sprite spr;
	public GameObject itemObj;
	public FarmManager manager;

	public void Start()
	{
		itemObj = gameObject;
	}


	public void OnCollisionEnter2D(Collision2D collision)
	{
		//maybe better way to check this?
		if(collision.gameObject.name == "Player")
		{
			ItemStack items = new ItemStack(this, 1);
			if (/*manager.inventory.IsTooFull()))*/false) { };
			new Inventory().AddItems(new ItemStack(this, 1));
		}
	}
}
