using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryManager : MonoBehaviour
{
	public Inventory inv;

	public GameObject ItemManagerObj;
	public ItemManager im;

	public int HeldSlot;
	public int Hotbar;

	public Item heldItem;
	public int heldAmount;

	public double MouseScrollDeadzone;
	public double currentMouseScroll;

    bool hotBarModifier;

	// Start is called before the first frame update
	void Awake()
	{
        //im = ItemManagerObj.GetComponent<ItemManager>();
        im = FindObjectOfType<ItemManager>();//.GetComponent<ItemManager>();

        inv = new Inventory(4, 9);
		//Debug.Log(im.GetItemByName("hoe").name);
		inv.AddItems(im.GetItemByName("basic sword"), 1);
		inv.AddItems(im.GetItemByName("basic bow"), 1);
		inv.AddItems(im.GetItemByName("hoe"), 1);
		inv.AddItems(im.GetItemByName("watering can"), 1);
		inv.AddItems(im.GetItemByName("sickle"), 1);
		inv.AddItems(im.GetItemByName("red shroom"), 1);
		inv.AddItems(im.GetItemByName("glowy shroom"), 1);
		inv.AddItems(im.GetItemByName("yellow shroom"), 1);
		inv.AddItems(im.GetItemByName("shroom shady"), 1);
		inv.AddItems(im.GetItemByName("white shroom"), 1);
	}
	private void Start()
	{
		if (GlobalGameSaving.Instance != null)
		{
			if (GlobalGameSaving.Instance.loadingSave == true)
			{
				inv.SetSaveableInventory(GlobalGameSaving.Instance.inventory);
				gameObject.GetComponent<CombatantEquipment>().SetSaveableEquipment(GlobalGameSaving.Instance.playerEquipment, im);
			}
			else if (ScenePersistence.Instance != null)
			{
				if (ScenePersistence.Instance.inventory.Count > 0)
				{
					inv.SetSaveableInventory(ScenePersistence.Instance.inventory);
					gameObject.GetComponent<CombatantEquipment>().SetSaveableEquipment(ScenePersistence.Instance.playerEquipment, im);
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
        if (ScenePersistence.Instance.gamePaused == false)
        {
            /*if (Keyboard.current.semicolonKey.wasPressedThisFrame)
		    {
		    	foreach(Item i in im.gameItems)
		    	{
		    		inv.AddItems(im.GetItemByName(i.name), 1);
		    	}
		    }*/


            if (Keyboard.current.digit1Key.wasPressedThisFrame == true)
            {
                HeldSlot = 0 % (inv.COLUMNS + 1);
            }
            else if (Keyboard.current.digit2Key.wasPressedThisFrame == true)
            {
                HeldSlot = 1 % (inv.COLUMNS + 1);
            }
            else if (Keyboard.current.digit3Key.wasPressedThisFrame == true)
            {
                HeldSlot = 2 % (inv.COLUMNS + 1);
            }
            else if (Keyboard.current.digit4Key.wasPressedThisFrame == true)
            {
                HeldSlot = 3 % (inv.COLUMNS + 1);
            }
            else if (Keyboard.current.digit5Key.wasPressedThisFrame == true)
            {
                HeldSlot = 4 % (inv.COLUMNS + 1);
            }
            else if (Keyboard.current.digit6Key.wasPressedThisFrame == true)
            {
                HeldSlot = 5 % (inv.COLUMNS + 1);
            }
            else if (Keyboard.current.digit7Key.wasPressedThisFrame == true)
            {
                HeldSlot = 6 % (inv.COLUMNS + 1);
            }
            else if (Keyboard.current.digit8Key.wasPressedThisFrame == true)
            {
                HeldSlot = 7 % (inv.COLUMNS + 1);
            }
            else if (Keyboard.current.digit9Key.wasPressedThisFrame == true)
            {
                HeldSlot = 8 % (inv.COLUMNS + 1);
            }		

		    UpdateHeldItem();
        }
	}

    void OnMenuMovement(InputValue value)
    {
        Vector2 v = value.Get<Vector2>();

        inv.selectedRow += (int)v.y;
        inv.selectedColumn += (int)v.x;

        if (inv.selectedRow < 0)
        {
            inv.selectedRow = inv.ROWS - 1;
        }
        else if (inv.selectedRow > inv.ROWS)
        {
            inv.selectedRow = 0;
        }

        if (inv.selectedColumn < 0)
        {
            inv.selectedColumn = inv.COLUMNS - 1;
        }
        else if (inv.selectedColumn > inv.COLUMNS)
        {
            inv.selectedColumn = 0;
        }

        Debug.Log($"Selected Slot is {inv.selectedRow},{inv.selectedColumn}");
    }

    void OnMenuInteract()
    {
        Debug.Log($"The selected item is {inv.GetSlotItem(inv.selectedRow, inv.selectedColumn).name}");
    }

    void OnSwitchHotbarModifier(InputValue value)
    {
        hotBarModifier = value.isPressed;
    }

    void OnHotbarScrolling(InputValue value)
    {
        var gamepad = Gamepad.current;

        if (gamepad != null)
        {
            if (value.Get<Vector2>().x == -1)
            {
                PreviousItem();
            }
            else if (value.Get<Vector2>().x == 1)
            {
                NextItem();
            }
            else if (value.Get<Vector2>().y == -1)
            {
                PreviousHotbar();
            }
            else if (value.Get<Vector2>().y == 1)
            {
                NextHotbar();
            }
        }

        float scroll = value.Get<Vector2>().y;

        if (hotBarModifier == true)
        {
            if (scroll < -MouseScrollDeadzone)
            {
                NextHotbar();
            }
            if (scroll > MouseScrollDeadzone)
            {
                PreviousHotbar();
            }
        }
        else
        {
            if (scroll < -MouseScrollDeadzone)
            {
                NextItem();
            }
            if (scroll > MouseScrollDeadzone)
            {
                PreviousItem();
            }
        }
    }

    void NextHotbar()
	{
		Hotbar++;
		Hotbar = Hotbar % (inv.ROWS);
	}
	void PreviousHotbar()
	{
		Hotbar--;
		if (Hotbar < 0) Hotbar = inv.ROWS - 1;
		Hotbar = Hotbar % (inv.ROWS);
	}
	void NextItem()
	{
		HeldSlot++;
		HeldSlot = HeldSlot % (inv.COLUMNS);
	}
	void PreviousItem()
	{
		HeldSlot--;
		if (HeldSlot < 0) HeldSlot = inv.COLUMNS - 1;
		HeldSlot = HeldSlot % (inv.COLUMNS);
	}
	void UpdateHeldItem()
	{
		heldItem = inv.GetSlotItem(Hotbar, HeldSlot);
		heldAmount = inv.GetSlotAmount(Hotbar, HeldSlot);
	}


	public void SaveInventory(string what)
	{
		if (what == "persist")
		{
			ScenePersistence.Instance.inventory = inv.GetSaveableInventory();
			ScenePersistence.Instance.playerEquipment = gameObject.GetComponent<CombatantEquipment>().GetSaveableEquipment();
		}
		else if (what == "save")
		{
			GlobalGameSaving.Instance.inventory = inv.GetSaveableInventory();
			GlobalGameSaving.Instance.playerEquipment = gameObject.GetComponent<CombatantEquipment>().GetSaveableEquipment();
		}
	}

	/// <summary>
	/// gets the type of item the player is holding
	/// </summary>
	/// <returns>the item object the player is holding</returns>
	public Item GetHeldItem()
	{
		heldItem = inv.GetSlotItem(Hotbar, HeldSlot);
		return heldItem;
	}
	/// <summary>
	/// gets the number of items the player is holding
	/// </summary>
	/// <returns>the number of items</returns>
	public int GetHeldItemAmount()
	{
		heldAmount = inv.GetSlotAmount(Hotbar, HeldSlot);
		return heldAmount;
	}

	/// <summary>
	/// removes the given amount of items from the item the player is holding
	/// </summary>
	/// <param name="amount">the amount of items to remove</param>
	public void RemoveHeldItems(int amount)
	{
		if (heldItem != null)
			inv.RemoveFromSlot(amount, Hotbar, HeldSlot);
		GetHeldItemAmount();
		GetHeldItem();
	}
	/// <summary>
	/// drops the item the player is currently holding and removes it from the inventory.
	/// </summary>
	public void DropHeldItems()
	{
		DropItems(Hotbar, HeldSlot, inv);
	}

	/// <summary>
	/// drops one item from the player's held item.
	/// </summary>
	public void DropHeldItem()
	{
		if (inv.GetSlotAmount(Hotbar, HeldSlot) <= 0) return;
		DropItem(heldItem);
		inv.RemoveFromSlot(1, Hotbar, HeldSlot);
	}

	/// <summary>
	/// Drops an item in the specified slot
	/// </summary>
	/// <param name="row">the row of the slot to drop from</param>
	/// <param name="column">the column of the slot to drop from</param>
	/// <param name="inv">the inventory to dropt the items from.</param>
	public void DropItems(int row, int column, Inventory inv)
	{
		if (inv.GetSlotAmount(row, column) <= 0) return;
		DropItems(inv.GetSlotItem(row, column), inv.GetSlotAmount(row, column));
		inv.DeleteSlot(row, column);
	}

	/// <summary>
	/// drops an amount of items on the ground
	/// </summary>
	/// <param name="i">item to drop</param>
	/// <param name="amount">amount of that item to drop</param>
	public void DropItems(Item item, int amount)
	{
		for(int i = 0; i < amount; i++)
		{
			DropItem(item);
		}
	}

	/// <summary>
	/// Drops a single item
	/// </summary>
	/// <param name="i"></param>
	public void DropItem(Item i) {
		if (i == null) return;

		ItemManager itemLibrary = im;
		Camera cam = Camera.main;
        Vector3 mouse = Mouse.current.position.ReadValue();
        mouse.z = 10;
        Vector2 worldMouse = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 vec = worldMouse - (Vector2)transform.position;
		float theta = Mathf.Atan2(vec.y, vec.x);

		Instantiate(itemLibrary.GetPrefabByName(i.name), transform.position + new Vector3(Mathf.Cos(theta) * 1.5f, Mathf.Sin(theta) * 1.5f, 0), Quaternion.identity);
	}
	public ItemManager GetItemManager()
	{
		return im;
	}
}
