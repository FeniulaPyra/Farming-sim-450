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
		inv = player.gameObject.GetComponent<PlayerInventoryManager>().inv;//FarmManager.GetComponent<FarmManager>().playerInventory;
		items = playerObj.GetComponent<PlayerInventoryManager>().GetItemManager().gameItems;//menuObj.GetComponent<Menu>().gameItems;
		player = playerObj.GetComponent<PlayerInteraction>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void BuyRedShroom()
	{
		BuyItem(3);
	}
	public void BuyYellowShroom()
	{
		BuyItem(4);
	}
	public void BuyBlueShroom()
	{
		BuyItem(5);
	}
	public void BuyWhiteShroom()
	{
		BuyItem(6);
	}
	public void BuyBlackShroom()
	{
		BuyItem(7);
	}
	public void BuyDog()
	{
		BuyItem(13);
	}
	public void BuyCat()
	{
		BuyItem(14);
	}
	public void BuyFrog()
	{
		BuyItem(15);
	}
	public void BuyDuck()
	{
		BuyItem(16);
	}
	public void BuyCow()
	{
		BuyItem(17);
	}
	public void BuyChicken()
	{
		BuyItem(18);
	}
	public void BuyPig()
	{
		BuyItem(19);
	}
	public void BuySheep()
	{
		BuyItem(20);
	}

	private void BuyItem(int itemID)
	{
		Item shroom = items[itemID];
		float cost = shroom.sellValue * 1.5f;

		if (player.playerGold < cost) return;

		inv.AddItems(shroom, 1);
		player.playerGold -= (int)Mathf.Floor(cost);

        //reduce farm's net worth by half of item worth
        netWorth.CalculateNetWorth(-((int)Mathf.Floor(cost / 2)));
	}
}
