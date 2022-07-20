using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShippingBin : MonoBehaviour
{
    public Collider2D playerCollider;
	public GameObject playerObject;
	//FarmManager farmManager;
	PlayerInventoryManager playerInvManager;
    Inventory playerInventory;
	public Inventory inventory;
    PlayerInteraction player;
    [SerializeField]
    List<Item> itemsToSell = new List<Item>();

    public TMP_Text goldDisplay;

    public FarmingTutorial farmingTutorial;

    public CalculateFarmNetWorth netWorth;

    [SerializeField]
    TMP_Text totalDisplay;

	private void Awake()
	{
		inventory = new Inventory(1, 9);
	/*}

	// Start is called before the first frame update
	void Start()
    {*/
        playerObject = GameObject.Find("Player");
        playerInvManager = playerObject.GetComponent<PlayerInventoryManager>();
		playerInventory = playerInvManager.inv;

        player = playerObject.GetComponent<PlayerInteraction>();

        goldDisplay = GameObject.Find("GoldDisplay").GetComponent<TMP_Text>();

        //totalDisplay = GameObject.Find("TotalDisplay").GetComponent<TMP_Text>();
        totalDisplay = GameObject.Find("Menus").transform.Find("Shipping Menu").transform.Find("TotalDisplay").GetComponent<TMP_Text>();

        goldDisplay.text = $"{player.playerGold} G";

        farmingTutorial = FindObjectOfType<FarmingTutorial>();

        if (GlobalGameSaving.Instance != null)
        {
            if (GlobalGameSaving.Instance.loadingSave == true)
            {
                inventory.SetSaveableInventory(GlobalGameSaving.Instance.shippingInv);
            }
            else if (ScenePersistence.Instance != null)
            {
                if (ScenePersistence.Instance.inventory.Count > 0)
                {
                    inventory.SetSaveableInventory(ScenePersistence.Instance.shippingInv);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTotal();
    }

    void UpdateTotal()
    {
        int toDisplay = 0;

        for (int i = 0; i < inventory.ROWS; i++)
        {
            for (int j = 0; j < inventory.COLUMNS; j++)
            {
                Item item = inventory.GetSlotItem(i, j);
				int amount = inventory.GetSlotAmount(i, j);

                if (item != null && item.isSellable)
                {
                    toDisplay += item.sellValue * amount;
                }
            }
        }

        if (totalDisplay != null && totalDisplay.isActiveAndEnabled == true)
        {
            totalDisplay.text = $"{toDisplay} G";
        }
    }

    public void PutItemInBin()
    {
		Item heldItem = playerInvManager.GetHeldItem();
		int heldAmount = playerInvManager.GetHeldItemAmount();

		//Removes currently held item from inventory and adds it to list
		if (heldItem != null && heldAmount > 0)
        {
            if (heldItem.isSellable == true)
            {
                itemsToSell.Add(heldItem);

				//ItemStack minusOne = new ItemStack(playerInventory.HeldItem.Item, -1);
				//playerInventory.HeldItem.CombineStacks(minusOne, playerInventory.STACK_SIZE);
				playerInventory.RemoveItems(heldItem, 1);
            }
        }
    }

    public void SaveInventory(string what)
    {
        if (what == "persist")
        {
            ScenePersistence.Instance.shippingInv = inventory.GetSaveableInventory();// GetSaveableInventory();
        }
        else if (what == "save")
        {
            GlobalGameSaving.Instance.shippingInv = inventory.GetSaveableInventory();//inventory = inv.GetSaveableInventory();
        }
    }

    public void PayPlayer()
    {
        int oldGold = player.playerGold;

        for (int i = 0; i < inventory.ROWS; i++)
        {
            for (int j = 0; j < inventory.COLUMNS; j++)
            {
				Item itemToSell = inventory.GetSlotItem(i, j);
				int amountToSell = inventory.GetSlotAmount(i, j);

                if (itemToSell != null && itemToSell.isSellable)
                {
                    player.playerGold += itemToSell.sellValue * amountToSell;

                    if (itemToSell.rare == true)
                    {
                        netWorth.CalculateNetWorth(50 * amountToSell);
                    }
                    else
                    {
                        netWorth.CalculateNetWorth(25 * amountToSell);
                    }

                    inventory.DeleteSlot(i, j);
                }
            }
        }

        /*foreach (Item item in itemsToSell)
        {
            player.playerGold += item.sellValue;

            if (item.rare == true)
            {
                netWorth.CalculateNetWorth(100);
            }
            else
            {
                netWorth.CalculateNetWorth(50);
            }
        }*/

        goldDisplay.text = $"{player.playerGold} G";

        if (farmingTutorial != null)
        {
            if (player.playerGold > oldGold && farmingTutorial.tutorialBools[12] == true)//(player.playerGold > oldGold && farmingTutorial.eatingAfter == true)
            {
                farmingTutorial.tutorialBools[14] = true;//farmingTutorial.shippedAfter = true;
                GlobalGameSaving.Instance.tutorialBools[14] = farmingTutorial.tutorialBools[14];
            }
        }

        if (true)
        {

        }

        //itemsToSell.Clear();
    }
}
