using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopMenu : MonoBehaviour
{

	//public GameObject FarmManager;
	public GameObject menuObj;
	public GameObject playerObj;
    public CalculateFarmNetWorth netWorth;
	//public GameObject ItemManagerObj;
	public ItemManager itemManager;

	//List<Item> items;
	Inventory inv;
	PlayerInteraction player;

    public TMP_Text goldDisplay;

    public FarmingTutorial farmingTutorial;

    // Start is called before the first frame update
    void Start()
    {

        //FarmManager = this;
        menuObj = GameObject.Find("Menus");
        playerObj = GameObject.Find("Player");
        netWorth = FindObjectOfType<CalculateFarmNetWorth>();
        itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();

        inv = playerObj.GetComponent<PlayerInventoryManager>().inv;//FarmManager.GetComponent<FarmManager>().playerInventory;
		//items = playerObj.GetComponent<PlayerInventoryManager>().GetItemManager().gameItems;//menuObj.GetComponent<Menu>().gameItems;
		player = playerObj.GetComponent<PlayerInteraction>();
		

        goldDisplay = GameObject.Find("GoldDisplay").GetComponent<TMP_Text>();
        //goldDisplay = gameObject.gameObject.gameObject.transform.Find("InformationCanvas").transform.Find("GoldDisplay").GetComponent<TMP_Text>();

        farmingTutorial = FindObjectOfType<FarmingTutorial>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void BuyRedShroom()
	{
		BuyItem("Red Shroom");


    }
	public void BuyYellowShroom()
	{
		BuyItem("Yellow Shroom");

        if (farmingTutorial != null)
        {
            if (farmingTutorial.tutorialBools[16] == false)
            {
                farmingTutorial.tutorialBools[16] = true;
                GlobalGameSaving.Instance.tutorialBools[16] = farmingTutorial.tutorialBools[16];
            }
        }
    }
	public void BuyBlueShroom()
	{
		BuyItem("Glowy Shroom");

        if (farmingTutorial != null)
        {
            if (farmingTutorial.tutorialBools[16] == false)
            {
                farmingTutorial.tutorialBools[16] = true;
                GlobalGameSaving.Instance.tutorialBools[16] = farmingTutorial.tutorialBools[16];
            }
        }
    }
	public void BuyWhiteShroom()
	{
		BuyItem("White shroom");

        if (farmingTutorial != null)
        {
            if (farmingTutorial.tutorialBools[16] == false)
            {
                farmingTutorial.tutorialBools[16] = true;
                GlobalGameSaving.Instance.tutorialBools[16] = farmingTutorial.tutorialBools[16];
            }
        }
    }
	public void BuyBlackShroom()
	{
		BuyItem("shroom shady");

        if (farmingTutorial != null)
        {
            if (farmingTutorial.tutorialBools[16] == false)
            {
                farmingTutorial.tutorialBools[16] = true;
                GlobalGameSaving.Instance.tutorialBools[16] = farmingTutorial.tutorialBools[16];
            }
        }
    }
	public void BuyDog()
	{
		BuyItem("pet dog");
	}
	public void BuyCat()
	{
		BuyItem("pet cat");
	}
	public void BuyFrog()
	{
		BuyItem("pet frog");
	}
	public void BuyDuck()
	{
		BuyItem("pet duck");
	}
	public void BuyCow()
	{
		BuyItem("pet cow");
	}
	public void BuyChicken()
	{
		BuyItem("pet chicken");
	}
	public void BuyPig()
	{
		BuyItem("pet pig");
	}
	public void BuySheep()
	{
		BuyItem("pet sheep");
	}
	public void BuyChest()
	{
		BuyItem("chest");
	}

	private void BuyItem(string itemName)//int itemID)
	{
		Item item = itemManager.GetItemByName(itemName);//items[itemID];
		float cost = item.sellValue * 1.5f;

		if (player.playerGold < cost) return;

		if (farmingTutorial != null)
		{
			if (farmingTutorial.tutorialBools[16] == false && itemName.ToLower().Contains("shroom"))
			{
				farmingTutorial.tutorialBools[16] = true;
				GlobalGameSaving.Instance.tutorialBools[16] = farmingTutorial.tutorialBools[16];
			}
		}

		inv.AddItems(item, 1);
		player.playerGold -= (int)Mathf.Floor(cost);

        goldDisplay.text = $"{player.playerGold} G";

        //reduce farm's net worth by half of item worth
        netWorth.CalculateNetWorth(-((int)Mathf.Floor(cost / 2)));
	}
}
