using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	// Start is called before the first frame update
	void Awake()
	{
		im = ItemManagerObj.GetComponent<ItemManager>();

		inv = new Inventory(4, 9);
		Debug.Log(im.GetItemByName("hoe").name);
		inv.AddItems(im.GetItemByName("hoe"), 1);
		inv.AddItems(im.GetItemByName("watering can"), 1);
		inv.AddItems(im.GetItemByName("sickle"), 1);
		inv.AddItems(im.GetItemByName("red shroom"), 1);
		inv.AddItems(im.GetItemByName("glowy shroom"), 1);
		inv.AddItems(im.GetItemByName("yellow shroom"), 1);
		inv.AddItems(im.GetItemByName("shroom shady"), 1);
		inv.AddItems(im.GetItemByName("white shroom"), 1);
		inv.AddItems(im.GetItemByName("pet dog"), 1);
	}
	private void Start()
	{
		if (GlobalGameSaving.Instance != null)
		{
			if (GlobalGameSaving.Instance.loadingSave == true)
			{
				inv.SetSaveableInventory(GlobalGameSaving.Instance.inventory);
			}
			else if (ScenePersistence.Instance != null)
			{
				if (ScenePersistence.Instance.inventory.Count > 0)
				{
					inv.SetSaveableInventory(ScenePersistence.Instance.inventory);
				}
			}
		}
	}
	// Update is called once per frame
	void Update()
	{
		//rotate hotbar
		if (Input.GetKeyDown((KeyCode)Menu.MenuControls.NEXT_HOTBAR))
			NextHotbar();
		if (Input.GetKeyDown((KeyCode)Menu.MenuControls.PREV_HOTBAR))
			PreviousHotbar();

		//rotate held item
		if (Input.GetKeyDown((KeyCode)Menu.MenuControls.NEXT_HOTBAR_SLOT))
			NextItem();
		if (Input.GetKeyDown((KeyCode)Menu.MenuControls.PREV_HOTBAR_SLOT))
			PreviousItem();

		for (int numKey = 1; numKey <= 9; numKey++)
		{
			if (Input.GetKeyDown("" + numKey))
			{
				HeldSlot = numKey - 1;
				HeldSlot = HeldSlot % (inv.COLUMNS + 1);
			}
		}

		float scroll = Input.GetAxis("Mouse ScrollWheel");

		//scroll controls - putting in seperate if because i am lazy and i
		//dont like when the lines get too long
		if (Input.GetKey(KeyCode.LeftControl))
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

		

		UpdateHeldItem();
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
		}
		else if (what == "save")
		{
			GlobalGameSaving.Instance.inventory = inv.GetSaveableInventory();
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

		ItemManager itemLibrary = im;//GameObject.Find("ItemManager").GetComponent<ItemManager>();
		Camera cam = Camera.main;
		Vector3 mouse = Input.mousePosition;
		mouse.z = 10;
		Vector2 worldMouse = cam.ScreenToWorldPoint(Input.mousePosition);

		Vector2 vec = worldMouse - (Vector2)transform.position;
		float theta = Mathf.Atan2(vec.y, vec.x);

		Instantiate(itemLibrary.GetPrefabByName(i.name), transform.position + new Vector3(Mathf.Cos(theta) * 1.5f, Mathf.Sin(theta) * 1.5f, 0), Quaternion.identity);
	}
	public ItemManager GetItemManager()
	{
		return im;
	}
}
