using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{

	public GameObject FarmManager;
	public GameObject menuObj;
	public GameObject playerObj;
    public CalculateFarmNetWorth netWorth;

	List<Item> items;
	Inventory inv;
	PlayerInteraction player;

    // Start is called before the first frame update
    void Start()
    {
		inv = FarmManager.GetComponent<FarmManager>().playerInventory;
		items = menuObj.GetComponent<Menu>().gameItems;
		player = playerObj.GetComponent<PlayerInteraction>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void BuyRedShroom()
	{
		BuyItem(5);
	}
	public void ByBlueShroom()
	{
		BuyItem(3);
	}

	private void BuyItem(int itemID)
	{
		Item shroom = items[itemID];
		float cost = shroom.sellValue * 1.5f;

		if (player.playerGold < cost) return;

		inv.AddItems(new ItemStack(shroom, 1));
		player.playerGold -= (int)Mathf.Floor(cost);

        //reduce farm's net worth by half of item worth
        netWorth.CalculateNetWorth(-((int)Mathf.Floor(cost / 2)));
	}
}
