using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	public string name;
	public Sprite spr;
	public GameObject itemObj;
	public FarmManager manager;

    public bool isSellable;

    public int sellValue;

	public GameObject player;

	public static int DISTANCE_TO_PICKUP = 1;

    public int staminaUsed;

    public bool isEdible;
    public int staminaToRestore;
    public bool rare;

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

		if( Vector2.Distance(itemObj.transform.position, player.transform.position) < .5) 
		{
			ItemStack items = new ItemStack(this, 1);
			if (!manager.playerInventory.IsTooFull(items))
			{
				manager.playerInventory.AddItems(items);
				Object.Destroy(this.gameObject);
			}
		}
	}
}
