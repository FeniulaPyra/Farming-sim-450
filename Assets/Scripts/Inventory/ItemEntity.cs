using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : MonoBehaviour
{
	GameObject itemObj;
	GameObject player;
	FarmManager manager;
	Item i;

	public static int DISTANCE_TO_PICKUP = 1;

	// Start is called before the first frame update
	void Start()
	{
		itemObj = gameObject;
		player = GameObject.Find("Player");
		GameObject manObj = GameObject.Find("ManagerObject");
		manager = manObj.GetComponent<FarmManager>();
		i = itemObj.GetComponent<Item>();


	}

	// Update is called once per frame
	void Update()
    {
		if (Vector2.Distance(itemObj.transform.position, player.transform.position) < .5)
		{
			if (!manager.playerInventory.IsTooFull(i, 1))
			{
				manager.playerInventory.AddItems(i, 1);
				Object.Destroy(this.gameObject);
			}
		}
	}
}
