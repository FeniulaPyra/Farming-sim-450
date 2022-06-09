using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEntity : BasicEntity
{
	public Inventory inv;
	public GameObject inventoryItemEntity;

	public int ITEM_DROP_RADIUS = 1;

    // Start is called before the first frame update
    void Start()
    {
		base.Start();
		if(inv == null || inv.ROWS == 0 || inv.COLUMNS == 0)
			inv = new Inventory(4, 9);
		movementSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void DestroyMe()
	{
		ItemManager im = GameObject.Find("ItemManager").GetComponent<ItemManager>();
		for(int r = 0; r < inv.ROWS; r++)
		{
			for(int c = 0; c < inv.COLUMNS; c++)
			{
				Item current = inv.GetSlotItem(r,c);
				//drops one of each item (
				for (int i = 0; i < inv.GetSlotAmount(r,c); i++)
				{
					Vector3 randomPosition = gameObject.transform.position + new Vector3(Random.value - .5f, Random.value - .5f);
					Instantiate(im.FindPrefabByItem(current), randomPosition, Quaternion.identity);
				}
				//clears slot
				inv.SetItem(r, c, null, 0);
			}
		}

		Instantiate(inventoryItemEntity, gameObject.transform.position, Quaternion.identity);
		Destroy(gameObject);
		
	}


	public void SaveEntity(out SaveInventoryEntity entity)
	{
		entity = new SaveInventoryEntity(movementSpeed, menu, size, speedCurve, maxSeekDistance, minSeekDistance, sr, player, rb, facing, gameObject, gameObject.transform.position, gameObject, inv);

	}
}
