using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEntity : BasicEntity
{
	public Inventory inv;
	public GameObject inventoryItemEntity;

	public int ITEM_DROP_RADIUS = 1;

    //List that will save items in order to pre-populate the chest
    public List<Item> items = new List<Item>();
    //Sister list of amounts to alter things in the chest
    public List<int> itemAmounts = new List<int>();
    //Item Manager to make the population possible
    [SerializeField]
    ItemManager manager;
    public ItemManager Manager
    {
        get
        {
            return manager;
        }
        set
        {
            manager = value;
        }
    }

    private void Awake()
    {
        manager = FindObjectOfType<ItemManager>();
    }


    // Start is called before the first frame update
    void Start()
    {
		base.Start();
		if(inv == null || inv.ROWS == 0 || inv.COLUMNS == 0)
			inv = new Inventory(4, 9);
		movementSpeed = 0;

        //manager = FindObjectOfType<ItemManager>();

        PopulateMe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    ///Just a foreach that sets the inventory based on the list 
    ///If a chest is instantiated on command outside of this script, the list can be edited and this function called to them make use of it
    /// </summary>
    public void PopulateMe()
    {
        //Make sure there are enough numbers at all for all of the items the chest will have
        while (itemAmounts.Count < items.Count)
        {
            itemAmounts.Add(1);
        }

        for (int i = 0; i < itemAmounts.Count; i++)
        {
            if (itemAmounts[i] > 99)
            {
                itemAmounts[i] = 99;
            }
            else if (itemAmounts[i] <= 0)
            {
                itemAmounts[i] = 1;
            }
            else
            {
                continue;
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            inv.AddItems(manager.GetItemByName(items[i].name), itemAmounts[i]);
        }
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
