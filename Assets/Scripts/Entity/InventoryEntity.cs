using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEntity : BasicEntity
{
	public Inventory inv;
    // Start is called before the first frame update
    void Start()
    {
		inv = new Inventory(4, 9);
		movementSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
