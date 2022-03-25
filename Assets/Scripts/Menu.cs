using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;


public class Menu : MonoBehaviour
{
	public Inventory inv;

	//the item prefabs and their respective attached item script
	public List<GameObject> gameItemPrefabs;
	private List<Item> gameItems;

	//inventory ui objects
	public GameObject InventoryMenu;
	private GameObject[,] InventorySlots;
	public GameObject InventorySlotPrefab;

	public GameObject PauseMenu;

	//the ui object that represents the currently selected item. hovers over the players mouse.
	public GameObject selectedItem;

	//hotbar ui objects
	public GameObject HotbarGameobject;
	private GameObject[] HotbarSlots;
	public GameObject HeldHotbarItem;
	public GameObject HotbarIndicator;
	public GameObject HotbarItemLabel;
	public GameObject InventoryItemLabel;


	private int startingX = -296-64;
	private int startingY = -148;
	private int gap = 10;

	public int invHotbarNum;

	public GameObject FarmManager;
	public GameObject player;

	public double MouseScrollDeadzone;
	public double currentMouseScroll;

	public enum MenuControls
	{
		OPEN_INVENTORY = KeyCode.E,
		NEXT_HOTBAR = KeyCode.I,
		PREV_HOTBAR = KeyCode.K,
		NEXT_HOTBAR_SLOT = KeyCode.L,
		PREV_HOTBAR_SLOT = KeyCode.J,
		DROP = KeyCode.Q
	}

	public enum MenuState
	{
		NO_MENU,
		INVENTORY,
		PAUSE,
		SETTINGS,
		HELP
	}

	MenuState state = MenuState.NO_MENU;

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

		HotbarItemLabel.GetComponent<Text>().text = "butthoels";
		Debug.Log(HotbarItemLabel.GetComponent<Text>().text);

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
				Text slotItemLabel = slot.transform.GetChild(2).GetComponent<Text>();

				//the item in the corresponding slot in the inventory object
				ItemStack currentSlot = inv.GetSlot(r, c);

				//labels the slot
				if (currentSlot != null)
				{
					slotLabel.text = "" + currentSlot.Amount;
					slotItemLabel.text = "" + currentSlot.Item.name;
					slot.name = "R" + r + " C" + currentSlot.Item.name;
					slotIcon.sprite = currentSlot.Item.spr;
					slotIcon.color = new Color(slotIcon.color.r, slotIcon.color.g, slotIcon.color.b, 100);
				}
				else
				{
					slotLabel.text = "";
					slot.name = "R" + r + " C" + c;
					slotIcon.sprite = null;
					slotIcon.color = new Color(slotIcon.color.r, slotIcon.color.g, slotIcon.color.b, 0);
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
				HotbarIndicator.transform.SetParent(InventoryMenu.transform, false);

			}
		}

		//highlights selected hotbar + item

		//shows/generates hotbar
		for (int c = 0; c < Inventory.COLUMNS; c++)
		{
			//the inventory slot button
			GameObject slot = HotbarSlots[c] = Instantiate(InventorySlotPrefab);
			slot.transform.position = new Vector2(startingX + (c + 1) * 74 - 10,
				startingY);
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
				slotIcon.color = new Color(slotIcon.color.r, slotIcon.color.g, slotIcon.color.b, 100);

			}
			else
			{
				slotLabel.text = "";
				slot.name = "Hotbar Slot " + c;
				slot.name = "Hotbar Slot " + c;
				slotIcon.sprite = null;
				slotIcon.color = new Color(slotIcon.color.r, slotIcon.color.g, slotIcon.color.b, 100);

			}
			slot.transform.SetParent(HotbarGameobject.transform, false);
		}



		//debugging stuff
		inv.AddItems(new ItemStack(gameItems[0], 1));
		inv.AddItems(new ItemStack(gameItems[1], 1));
		inv.AddItems(new ItemStack(gameItems[2], 1));
        inv.AddItems(new ItemStack(gameItems[3], 1));
        inv.AddItems(new ItemStack(gameItems[4], 1));
        inv.AddItems(new ItemStack(gameItems[5], 1));
        UpdateInventory();
    }

	void UpdateInventory()
	{
		invHotbarNum = inv.hotbarRowNumber;
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

		HotbarIndicator.transform.localPosition =
			new Vector2(
			0,
			(inv.hotbarRowNumber-1) * 74 - 10);

        if (inv.HeldItem != null)
        {
            HotbarItemLabel.GetComponent<Text>().text = inv.HeldItem.Item.name;
            if (inv.HeldItem.Item.staminaUsed > 0)
            {
                HotbarItemLabel.GetComponent<Text>().text += $" (-{inv.HeldItem.Item.staminaUsed} Stamina)";
            }
        }
        else
        {
            HotbarItemLabel.GetComponent<Text>().text = "";
        }
        /*if (inv.HeldItem != null)
            HotbarItemLabel.GetComponent<Text>().text = inv.HeldItem.Item.name;
            if (inv.HeldItem.Item.staminaUsed > 0)
                HotbarItemLabel.GetComponent<Text>().text += $" - {inv.HeldItem.Item.staminaUsed} Stamina";
        else
            HotbarItemLabel.GetComponent<Text>().text = "";*/

        //updates inventory slots
        for (int r = 0; r < Inventory.ROWS; r++)
		{
			for (int c = 0; c < Inventory.COLUMNS; c++)
			{

				//gets the inventory slot menu button and info about the inventory slot
				GameObject slot = InventorySlots[r, c];
				Image slotIcon = slot.transform.GetChild(0).GetComponent<Image>();
				Text slotLabel = slot.transform.GetChild(1).GetComponent<Text>();
				Text slotItemLabel = slot.transform.GetChild(2).GetComponent<Text>();
				//corresponding inventory slot
				ItemStack currentSlot = inv.GetSlot(r, c);

				//updates slot info
				if (currentSlot != null)
				{
					slotLabel.text = "" + currentSlot.Amount;
					slotItemLabel.text = "" + currentSlot.Item.name;
					slotIcon.sprite = currentSlot.Item.spr;
					slotIcon.color = new Color(slotIcon.color.r, slotIcon.color.g, slotIcon.color.b, 100);

				}
				else
				{
					slotLabel.text = "";
					slotItemLabel.text = "";
					slotIcon.sprite = null;
					slotIcon.color = new Color(slotIcon.color.r, slotIcon.color.g, slotIcon.color.b, 0);

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
				slotIcon.color = new Color(slotIcon.color.r, slotIcon.color.g, slotIcon.color.b, 100);
			}
			else
			{
				slotLabel.text = "";
				slot.name = "Hotbar Slot " + c;
				slot.name = "Hotbar Slot " + c;
				slotIcon.sprite = null;
				slotIcon.color = new Color(slotIcon.color.r, slotIcon.color.g, slotIcon.color.b, 0);
			}
			slot.transform.SetParent(HotbarGameobject.transform, false);
		}
		inv.HoldItem(inv.slotHeld);
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
			if(gameItems[i].name == itemname)
			{
				return gameItemPrefabs[i];
			}
		}
		return null;
	}

	// Update is called once per frame
	void Update()
	{

		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (InventoryMenu.activeSelf)
		{
		}
		Debug.Log("menustate: " + state);

		switch(state)
		{
			case MenuState.INVENTORY:
				//https://stackoverflow.com/questions/43802207/position-ui-to-mouse-position-make-tooltip-panel-follow-cursor
				Vector2 pos;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(
				InventoryMenu.transform.parent.transform as RectTransform, Input.mousePosition,
				InventoryMenu.transform.parent.GetComponent<Canvas>().worldCamera,
					out pos);
				//transform.position = InventoryMenu.transform.parent.transform.TransformPoint(pos);

				selectedItem.transform.position = InventoryMenu.transform.parent.transform.TransformPoint(new Vector3(pos.x, pos.y + 33, -1));//new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

				//drop selected item
				if(Input.GetKeyDown(KeyCode.Q))
				{
					if (InventoryMenu.active && inv.selectedStack != null)
					{
						//drop selected Item
						for (int i = 0; i < inv.selectedStack.Amount; i++)
						{
							Instantiate(GetItemPrefab(inv.selectedStack.Item.name), player.transform.position + new Vector3(0, -1f, 0), Quaternion.identity);
						}
						inv.DeleteSelectedItemStack();
					}
				}

				//scroll hotbar up and down
				if (scroll < -MouseScrollDeadzone)
				{
					inv.selectNextHotbar();
				}
				if (scroll > MouseScrollDeadzone)
				{
					inv.selectPreviousHotbar();
				}

				//close inventory
				if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
				{
					if(inv.selectedStack != null)
					{
						inv.AddItems(inv.selectedStack);
						inv.selectedStack = null;
					}
					InventoryMenu.SetActive(false);
					state = MenuState.NO_MENU;
				}
				break;
			case MenuState.PAUSE:

				if (Input.GetKeyDown(KeyCode.Escape))
				{
					PauseMenu.SetActive(false);
					state = MenuState.NO_MENU;
				}
				break;
			case MenuState.SETTINGS:
				break;
			case MenuState.HELP:
				break;
			case MenuState.NO_MENU:
				//selects hotbar item to pressed numkey
				for (int numKey = 0; numKey <= 9; numKey++)
				{
					if (Input.GetKeyDown("" + numKey))
					{
						inv.HoldItem(numKey - 1);
					}
				}
				
				//rotate hotbar
				if (Input.GetKeyDown((KeyCode)MenuControls.NEXT_HOTBAR))
				{
					inv.selectNextHotbar();
				}
				if (Input.GetKeyDown((KeyCode)MenuControls.PREV_HOTBAR))
				{
					inv.selectPreviousHotbar();
				}

				//rotate held item
				if (Input.GetKeyDown((KeyCode)MenuControls.NEXT_HOTBAR_SLOT))
				{
					inv.HoldNextItem();
				}
				if (Input.GetKeyDown((KeyCode)MenuControls.PREV_HOTBAR_SLOT))
				{
					inv.HoldPreviousItem();
				}

				//scroll controls - putting in seperate if because i am lazy and i
				//dont like when the lines get too long
				if (Input.GetKey(KeyCode.LeftControl)) {
					if (scroll < -MouseScrollDeadzone)
					{
						inv.selectNextHotbar();
					}
					if (scroll > MouseScrollDeadzone)
					{
						inv.selectPreviousHotbar();
					}
				}
				else
				{
					if (scroll < -MouseScrollDeadzone)
					{
						inv.HoldNextItem();
					}
					if (scroll > MouseScrollDeadzone)
					{
						inv.HoldPreviousItem();
					}
				}

				//open/close inventory
				if (Input.GetKeyDown(KeyCode.E))
				{
					InventoryMenu.SetActive(true);//!InventoryMenu.activeSelf);
					state = MenuState.INVENTORY;
				}

				//drop held item
				if(Input.GetKeyDown(KeyCode.Q))
				{
					if(inv.HeldItem != null)
					{
						//drop held item
						for (int i = 0; i < inv.HeldItem.Amount; i++)
						{
							Instantiate(GetItemPrefab(inv.HeldItem.Item.name), player.transform.position + new Vector3(0, -1f, 0), Quaternion.identity);
						}
						inv.DeleteHeldItemStack(); 
					}
				}
				
				if(Input.GetKeyDown(KeyCode.Escape))
				{
					PauseMenu.SetActive(true);
					state = MenuState.PAUSE;
				}

				break;
			default:
				break;
		}
		UpdateInventory();
	}
	
	public List<Item> GetGameItemList()
	{
		return gameItems;
	}
	
	//hides the pause menu - for button use
	public void HidePauseMenu()
	{
		PauseMenu.SetActive(false);
		state = MenuState.NO_MENU;
	}

	//quits game - for button use;
	public void QuitGame()
	{
		Application.Quit();
	}
}
