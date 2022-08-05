using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEntity : BasicEntity
{
	public GameObject playerObj;
	PlayerInventoryManager pim;
	Inventory inv;
	GameObject itemObj;
	//FarmManager manager;
	Item i;

	public static int DISTANCE_TO_PICKUP = 1;

	// Start is called before the first frame update
	void Start()
	{
		base.Start();
		itemObj = gameObject;
		playerObj = GameObject.Find("Player");
		player = playerObj.transform;
		pim = playerObj.GetComponent<PlayerInventoryManager>();
		inv = pim.inv;
		//note must have separate item object because the object gets destroyed
		i = pim.im.GetItemByName(itemObj.GetComponent<Item>().name);


	}

	// Update is called once per frame
	void Update()
    {
        if (ScenePersistence.Instance.gamePaused == false)
        {
            if (Vector2.Distance(itemObj.transform.position, playerObj.transform.position) < .5)
		    {
		    	if (!(inv.IsTooFull(i, 1)))
		    	{
		    		Debug.Log("LEP2738 AAAAAA" + i.name);
		    		inv.AddItems(i, 1);
		    		Object.Destroy(this.gameObject);
		    	}
		    }
        }
	}
}
