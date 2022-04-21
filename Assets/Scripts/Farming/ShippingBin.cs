using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShippingBin : MonoBehaviour
{
    public Collider2D playerCollider;
    FarmManager farmManager;
    Inventory playerInventory;
	public Inventory inventory;
    PlayerInteraction player;
    [SerializeField]
    List<Item> itemsToSell = new List<Item>();

    public TMP_Text goldDisplay;

    public FarmingTutorial farmingTutorial;

    public CalculateFarmNetWorth netWorth;

	private void Awake()
	{
		inventory = new Inventory(1, 9);
		
	}

	// Start is called before the first frame update
	void Start()
    {

        farmManager = FindObjectOfType<FarmManager>();
        playerInventory = farmManager.playerInventory;

        player = FindObjectOfType<PlayerInteraction>();

        goldDisplay.text = $"{player.playerGold} G";

        farmingTutorial = FindObjectOfType<FarmingTutorial>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == playerCollider && Input.GetKeyDown(KeyCode.Space) && player.isTalking == false)
        {
            PutItemInBin();
        }
    }*/

    public void PutItemInBin()
    {
        //Removes currently held item from inventory and adds it to list
        if (playerInventory.HeldItem != null)
        {
            if (playerInventory.HeldItem.Item.isSellable == true)
            {
                itemsToSell.Add(playerInventory.HeldItem.Item);

                ItemStack minusOne = new ItemStack(playerInventory.HeldItem.Item, -1);
                playerInventory.HeldItem.CombineStacks(minusOne, playerInventory.STACK_SIZE);
            }
        }
    }

    public void PayPlayer()
    {
        int oldGold = player.playerGold;

        foreach (Item item in itemsToSell)
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
        }

        goldDisplay.text = $"{player.playerGold} G";

        if (player.playerGold > oldGold && farmingTutorial.eatingAfter == true)
        {
            farmingTutorial.shippedAfter = true;
        }

        if (true)
        {

        }

        itemsToSell.Clear();
    }
}
