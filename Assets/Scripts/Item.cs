using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	public string name;
	public Sprite spr;
	public GameObject itemObj;
	public FarmManager manager;

	public GameObject player;

	public static int DISTANCE_TO_PICKUP = 1;

	public void Start()
	{
		itemObj = gameObject;
		player = GameObject.Find("Player");
		GameObject manObj = GameObject.Find("ManagerObject");
		manager = manObj.GetComponent<FarmManager>();
	}


	//public void OnCollisionEnter2D(Collision2D collision)
	private void Update()
	{
		//maybe better way to check this?
		//if(collision.gameObject.name == "Player")
		if( Vector2.Distance(itemObj.transform.position, player.transform.position) < 1) 
		{
			//Debug.Log("SMACKed  into the player !");
			ItemStack items = new ItemStack(this, 1);
			if (!manager.playerInventory.IsTooFull(items))
			{
				Debug.Log("go in tha invnetoyr!!!1!");
				manager.playerInventory.AddItems(items);
				Object.Destroy(this.gameObject);
			}
		}
	}
}
