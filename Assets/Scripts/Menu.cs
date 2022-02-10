using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;


public class Menu : MonoBehaviour
{
	public Inventory inv;
	public List<GameObject> gameItemPrefabs;
	private List<Item> gameItems;

	public GameObject InventoryMenu;
	private GameObject[,] InventorySlots;
	public GameObject InventorySlotPrefab;
	public GameObject selectedItem;

	public GameObject HotbarGameobject;
	private GameObject[] HotbarSlots;
	public GameObject HeldHotbarItem;

	public GameObject FarmManager;
	public GameObject player;

	public enum MenuControls
	{
		OPEN_INVENTORY = KeyCode.I,
		NEXT_HOTBAR = KeyCode.UpArrow,
		PREV_HOTBAR = KeyCode.DownArrow,
		NEXT_HOTBAR_SLOT = KeyCode.RightArrow,
		PREV_HOTBAR_SLOT = KeyCode.LeftArrow,
		DROP = KeyCode.Q
	}

	// Start is called before the first frame update
	void Start()
    {
		gameItems = new List<Item>();
		//TODO have this be grabbed from the player once that is be do be done-ificated
		inv = FarmManager.GetComponent<FarmManager>().playerInventory; //new Inventory();

		InventorySlots = new GameObject[Inventory.ROWS,Inventory.COLUMNS];
		HotbarSlots = new GameObject[Inventory.COLUMNS];
		//hides inventory menu at start.
		InventoryMenu.SetActive(false);

		//gets dictionary of all the item prefabs
		foreach (GameObject g in gameItemPrefabs)
		{
			gameItems.Add(g.GetComponent<Item>());
		}

		//top left of the inventory menu
		int startingX = -333;
		int startingY = -148;
		int gap = 10;

		//creates buttons for every inventory slot
		for (int r = 0; r < Inventory.ROWS; r++)
		{
			for (int c = 0; c < Inventory.COLUMNS; c++)
			{
				//the inventory slot button
				GameObject slot = InventorySlots[r, c] = Instantiate(InventorySlotPrefab);
				slot.transform.position = new Vector2(startingX + (c+1) * 74 - 10, startingY + (r+1) * 74 - 10);

				//the text and image of the button
				Image slotIcon = slot.transform.GetChild(0).GetComponent<Image>();
				Text slotLabel = slot.transform.GetChild(1).GetComponent<Text>();

				//the item in the corresponding slot in the inventory object
				ItemStack currentSlot = inv.GetSlot(r, c);

				//labels the slot
				if (currentSlot != null)
				{
					slotLabel.text = "" + currentSlot.Amount;
					slot.name = "R" + r + " C" + currentSlot.Item.name;
					slotIcon.sprite = currentSlot.Item.spr;
				}
				else
				{
					slotLabel.text = "";
					slot.name = "R" + r + " C" + c;
					slotIcon.sprite = null;
				}
				int x = r, y = c;

				//adds click event listener to select slot.
				slot.GetComponent<Button>().onClick.AddListener(delegate {
					Debug.Log("Clicked button");
					inv.SelectSlot(x, y);
					UpdateInventory();
				});
				//slot.GetComponent<Button>().onClick.AddListener(UpdateInventory);

				//adds the slot to the inventory menu
				slot.transform.SetParent(InventoryMenu.transform, false);

			}
		}

		//highlights selected hotbar + item

		//shows/generates hotbar
		for (int c = 0; c < Inventory.COLUMNS; c++)
		{
			//the inventory slot button
			GameObject slot = HotbarSlots[c] = Instantiate(InventorySlotPrefab);
			slot.transform.position = new Vector2(startingX + (c + 1) * 74 - 10, startingY -84);

			//the text and image of the button
			Image slotIcon = slot.transform.GetChild(0).GetComponent<Image>();
			Text slotLabel = slot.transform.GetChild(1).GetComponent<Text>();

			//the item in the corresponding slot in the inventory object
			ItemStack currentSlot = inv.GetSlot(inv.hotbarRowNumber, c);

			//labels the slot
			if (currentSlot != null)
			{
				slotLabel.text = "" + currentSlot.Amount;
				slot.name = "Hotbar Slot " + currentSlot.Item.name;
				slotIcon.sprite = currentSlot.Item.spr;
			}
			else
			{
				slotLabel.text = "";
				slot.name = "Hotbar Slot " + c;
				slot.name = "Hotbar Slot " + c;
				slotIcon.sprite = null;
			}
			slot.transform.SetParent(HotbarGameobject.transform, false);
		}

		//debugging stuff
		inv.AddItems(new ItemStack(gameItems[0], 10));
		inv.AddItems(new ItemStack(gameItems[1], 10));
		inv.AddItems(new ItemStack(gameItems[2], 1));
        inv.AddItems(new ItemStack(gameItems[3], 1));
        inv.AddItems(new ItemStack(gameItems[4], 1));
        inv.AddItems(new ItemStack(gameItems[5], 1));
        UpdateInventory();
    }

	void UpdateInventory()
	{
		//updates selected item
		Image selectedItemIcon = selectedItem.transform.GetChild(0).GetComponent<Image>();
		Text selectedItemLabel = selectedItem.transform.GetChild(1).GetComponent<Text>();

		if (inv.selectedStack != null)
		{
			selectedItemLabel.text = "" + inv.SelectedStack.Amount;
			selectedItemIcon.enabled = true;
			selectedItemIcon.sprite = inv.SelectedStack.Item.spr;
		}
		else
		{
			selectedItemLabel.text = "";
			selectedItemIcon.sprite = null;
			selectedItemIcon.enabled = false;

		}

		//updates inventory slots
		for (int r = 0; r < Inventory.ROWS; r++)
		{
			for (int c = 0; c < Inventory.COLUMNS; c++)
			{

				//gets the inventory slot menu button and info about the inventory slot
				GameObject slot = InventorySlots[r, c];
				Image slotIcon = slot.transform.GetChild(0).GetComponent<Image>();
				Text slotLabel = slot.transform.GetChild(1).GetComponent<Text>();
				//corresponding inventory slot
				ItemStack currentSlot = inv.GetSlot(r, c);

				//updates slot info
				if (currentSlot != null)
				{
					slotLabel.text = "" + currentSlot.Amount;
					slotIcon.sprite = currentSlot.Item.spr;
				}
				else
				{
					slotLabel.text = "";
					slotIcon.sprite = null;
				}
			}
		}

		//updates hotbar
		for (int c = 0; c < Inventory.COLUMNS; c++)
		{
			//the inventory slot button
			GameObject slot = HotbarSlots[c];

			//the text and image of the button
			Image slotIcon = slot.transform.GetChild(0).GetComponent<Image>();
			Text slotLabel = slot.transform.GetChild(1).GetComponent<Text>();

			//the item in the corresponding slot in the inventory object
			ItemStack currentSlot = inv.GetSlot(inv.hotbarRowNumber, c);

			//labels the slot
			if (currentSlot != null)
			{
				slotLabel.text = "" + currentSlot.Amount;
				slot.name = "Hotbar Slot " + currentSlot.Item.name;
				slotIcon.sprite = currentSlot.Item.spr;
			}
			else
			{
				slotLabel.text = "";
				slot.name = "Hotbar Slot " + c;
				slot.name = "Hotbar Slot " + c;
				slotIcon.sprite = null;
			}
			slot.transform.SetParent(HotbarGameobject.transform, false);
		}
		//updates held item indicator
		HeldHotbarItem.transform.position = HotbarSlots[inv.slotHeld].transform.position;
	}

	/// <summary>
	/// Summary of the situation is i have coded myself into a hole.
	/// I'm gonna need to just sit down and redo the whole item system
	/// and how the other classes interact with it cuz its a mess rn
	/// </summary>
	private GameObject GetItemPrefab(string itemname)
	{
		//this is so dumb
		for(int i = 0, len = gameItemPrefabs.Count; i < len; i++)
		{
			//thi sis the most stupid and roundabout way its justa  dictionary
			//why didnt i just make a fucking dictionary
			//aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
			if(gameItems[i].name == itemname)
			{
				//i miss javascript
				//im not stupid in javascritp (yes i amm)
				return gameItemPrefabs[i];
			}
		}
		return null;
	}

	// Update is called once per frame
	void Update()
    {
		
		if(InventoryMenu.activeSelf)
		{
			//https://stackoverflow.com/questions/43802207/position-ui-to-mouse-position-make-tooltip-panel-follow-cursor
			Vector2 pos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(
			   InventoryMenu.transform.parent.transform as RectTransform, Input.mousePosition,
			   InventoryMenu.transform.parent.GetComponent<Canvas>().worldCamera,
			   out pos);
			//transform.position = InventoryMenu.transform.parent.transform.TransformPoint(pos);

			selectedItem.transform.position = InventoryMenu.transform.parent.transform.TransformPoint(new Vector3(pos.x, pos.y + 33, -1));//new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
		}

		//selects hotbar item to pressed numkey
		for(int numKey = 0; numKey <= 9; numKey++)
		{
			if(Input.GetKeyDown("" + numKey))
			{
				inv.HoldItem(numKey - 1);
			}
		}

		//rotate hotbar
		if(Input.GetKeyDown((KeyCode)MenuControls.NEXT_HOTBAR))
		{
			inv.selectNextHotbar();
		}
		if(Input.GetKeyDown((KeyCode)MenuControls.PREV_HOTBAR))
		{
			inv.selectPreviousHotbar();
		}

		//rotate held item
		if(Input.GetKeyDown((KeyCode)MenuControls.NEXT_HOTBAR_SLOT))
		{
			inv.HoldNextItem();
		}
		if(Input.GetKeyDown((KeyCode)MenuControls.PREV_HOTBAR_SLOT))
		{
			inv.HoldPreviousItem();
		}

		//open/close inventory
		if (Input.GetKeyDown((KeyCode)MenuControls.OPEN_INVENTORY))
		{
			InventoryMenu.SetActive(!InventoryMenu.activeSelf);
			if(!InventoryMenu.activeSelf && inv.selectedStack != null)
			{
				inv.AddItems(inv.selectedStack);
				inv.selectedStack = null;
			}

			inv.isShown = InventoryMenu.activeSelf;
		}

		if(Input.GetKeyDown((KeyCode)MenuControls.DROP))
		{
			if (InventoryMenu.active)
			{
				//drop selected Item
				for (int i = 0; i < inv.selectedStack.Amount; i++)
				{
					Instantiate(GetItemPrefab(inv.selectedStack.Item.name), player.transform.position + new Vector3(0, -1.5f, 0), Quaternion.identity);
				}
				inv.DeleteSelectedItemStack(); 
			}
			else
			{
				//drop held item
				for (int i = 0; i < inv.HeldItem.Amount; i++)
				{
					Instantiate(GetItemPrefab(inv.HeldItem.Item.name), player.transform.position + new Vector3(0, -1.5f, 0), Quaternion.identity);
				}
				inv.DeleteHeldItemStack(); 
			}
		}
		UpdateInventory();
	}
}
