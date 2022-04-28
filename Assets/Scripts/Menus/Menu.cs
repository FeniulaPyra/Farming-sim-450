using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//using UnityEngine.UIElements;


public class Menu : MonoBehaviour
{
	public Inventory inv;
	public Inventory shippingInventory;

	//the item prefabs and their respective attached item script
	public List<GameObject> gameItemPrefabs;
	public List<Item> gameItems;

	//inventory ui objects
	public GameObject InventoryMenu;
	private GameObject[,] InventorySlots;
	public GameObject InventorySlotPrefab;

	public GameObject PauseMenu;
	public GameObject SettingsMenu;
	public GameObject HelpMenu;
	public GameObject SaveMenu;
	public GameObject LoadMenu;
	public GameObject ShopMenu;
	public GameObject ShippingMenu;

	public GameObject gameInfo;
	public GameObject controls;

	public GameObject BubbleToggle;


	//the ui object that represents the currently selected item. hovers over the players mouse.
	public GameObject selectedItem;
	ItemStack curSelected;

	//hotbar ui objects
	public GameObject HotbarGameobject;
	private GameObject[] HotbarSlots;
	public GameObject HeldHotbarItem;
	public GameObject HotbarIndicator;
	public GameObject HotbarItemLabel;
	public GameObject InventoryItemLabel;

	public GameObject[] ShippingBinSlots;

	private int startingX = -296-64;
	private int startingY = -148;
	private int gap = 10;

	public int invHotbarNum;

	public GameObject FarmManager;
	public GameObject ShippingBin;
	public GameObject player;
	private PlayerInteraction pi;

	public double MouseScrollDeadzone;
	public double currentMouseScroll;

	public Camera cam;

    //Player's bed
    Bed bed;

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
		HELP,
		SHOP,
		SHIPPING_BIN,
		BED,
		DIALOG
	}

    [SerializeField]
	MenuState state = MenuState.NO_MENU;

	// Start is called before the first frame update
	void Start()
    {
		gameItems = new List<Item>();
		//TODO have this be grabbed from the player once that is be do be done-ificated
		inv = FarmManager.GetComponent<FarmManager>().playerInventory; //new Inventory();
		shippingInventory = ShippingBin.GetComponent<ShippingBin>().inventory;
		pi = player.GetComponent<PlayerInteraction>();
		InventorySlots = new GameObject[inv.ROWS, inv.COLUMNS];
		HotbarSlots = new GameObject[inv.COLUMNS];

		ShippingBinSlots = new GameObject[9];
		

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
		for (int r = 0; r < inv.ROWS; r++)
		{
			for (int c = 0; c < inv.COLUMNS; c++)
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
					Debug.Log("Clicked Inventory button");
					//if the curently selecteditem isn ot from the inventory, simply select the shipping bin stack
					if (shippingInventory.selectedStack == null)
					{
						inv.SelectSlot(x, y);
					}
					//if the currently selcected stack is in the inventory, swap if possible.
					else
					{

						//selected stack copy
						ItemStack shippingSelected = new ItemStack(shippingInventory.selectedStack);

						//clicked slot is empty
						if (inv.GetSlot(x, y) == null)
						{
							//add item to slot
							inv.SetItem(x, y, shippingSelected);

							//clear selected item
							shippingInventory.selectedStack = null;
						}
						//clicked slot has an item
						else
						{
							//set selected stack to item
							shippingInventory.selectedStack = inv.GetSlot(x, y);
							Debug.Log(inv.selectedStack.Item.name);

							//set slot to item
							inv.SetItem(x, y, shippingSelected);
						}
					}
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
		for (int c = 0; c < inv.COLUMNS; c++)
		{
			//the inventory slot button
			GameObject slot = HotbarSlots[c] = Instantiate(InventorySlotPrefab);
			slot.transform.position = new Vector2(startingX + (c + 1) * 74 - 10,
				startingY);

			slot.GetComponent<InventorySlot>().SetIndex(c, inv);

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
		
		//generates shipping bin slots
		for(int c = 0; c < shippingInventory.COLUMNS; c++)
		{
			GameObject slot = ShippingBinSlots[c] = Instantiate(InventorySlotPrefab);
			slot.transform.position = new Vector2(startingX + (c + 1) * 74 - 10, -170);

			//the text and image of the button
			Image slotIcon = slot.transform.GetChild(0).GetComponent<Image>();
			Text slotLabel = slot.transform.GetChild(1).GetComponent<Text>();
			Text slotItemLabel = slot.transform.GetChild(2).GetComponent<Text>();

			//the item in the corresponding slot in the inventory object
			ItemStack currentSlot = shippingInventory.GetSlot(0, c);

			//labels the slot
			if (currentSlot != null)
			{
				slotLabel.text = "" + currentSlot.Amount;
				slotItemLabel.text = "" + currentSlot.Item.name;
				slot.name = $"C {currentSlot.Item.name}";
				slotIcon.sprite = currentSlot.Item.spr;
				slotIcon.color = new Color(slotIcon.color.r, slotIcon.color.g, slotIcon.color.b, 100);
			}
			else
			{
				slotLabel.text = "";
				slot.name = "C" + c;
				slotIcon.sprite = null;
				slotIcon.color = new Color(slotIcon.color.r, slotIcon.color.g, slotIcon.color.b, 0);
			}
			int y = c;

			//when clicked swaps the thing
			slot.GetComponent<Button>().onClick.AddListener(delegate {

				//if the curently selecteditem isn ot from the inventory, simply select the shipping bin stack
				if (inv.selectedStack == null)
				{
					shippingInventory.SelectSlot(0, y);
				}
				//if the currently selcected stack is in the inventory, swap if possible.
				else
				{

					//selected stack copy
					ItemStack invSelected = new ItemStack(inv.selectedStack);

					if (!invSelected.Item.isSellable) return;
					//clicked slot is empty
					if (shippingInventory.GetSlot(0, y) == null)
					{
						//add item to slot
						shippingInventory.SetItem(0, y, invSelected);

						//clear selected item
						inv.selectedStack = null;
					}
					//clicked slot has an item
					else
					{
						//set selected stack to item
						inv.selectedStack = shippingInventory.GetSlot(0, y);
						Debug.Log(inv.selectedStack.Item.name);

						//set slot to item
						shippingInventory.SetItem(0, y, invSelected);
					}
				}

				UpdateInventory();
			});
			slot.transform.SetParent(ShippingMenu.transform, false);
		}

		//debugging stuff
		inv.AddItems(new ItemStack(gameItems[0], 1));
		//shippingInventory.AddItems(new ItemStack(gameItems[0], 1));
		inv.AddItems(new ItemStack(gameItems[1], 1));
		inv.AddItems(new ItemStack(gameItems[2], 1));
        inv.AddItems(new ItemStack(gameItems[3], 1));
        inv.AddItems(new ItemStack(gameItems[4], 1));
        inv.AddItems(new ItemStack(gameItems[5], 1));
        inv.AddItems(new ItemStack(gameItems[6], 1));
        inv.AddItems(new ItemStack(gameItems[7], 1));
        UpdateInventory();

        bed = FindObjectOfType<Bed>().GetComponent<Bed>();
    }

	void UpdateInventory()
	{
		invHotbarNum = inv.hotbarRowNumber;
		if (shippingInventory.selectedStack != null)
			curSelected = shippingInventory.selectedStack;
		else
			curSelected = inv.selectedStack;


		//updates selected item
		Image selectedItemIcon = selectedItem.transform.GetChild(0).GetComponent<Image>();
		Text selectedItemLabel = selectedItem.transform.GetChild(1).GetComponent<Text>();

		if (curSelected != null)
		{
			selectedItemLabel.text = "" + curSelected.Amount;
			selectedItemIcon.enabled = true;
			selectedItemIcon.sprite = curSelected.Item.spr;
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
			(inv.hotbarRowNumber - 1) * 74 - 10);

		if (inv.HeldItem != null)
		{
			HotbarItemLabel.GetComponent<Text>().text = inv.HeldItem.Item.name + "\n";
			if (inv.HeldItem.Item.staminaUsed > 0)
			{
				HotbarItemLabel.GetComponent<Text>().text += $" (Use: -{inv.HeldItem.Item.staminaUsed} Stamina)";
			}
			if (inv.HeldItem.Item.isEdible)
			{
				HotbarItemLabel.GetComponent<Text>().text += $" (Eat: +{inv.HeldItem.Item.staminaToRestore} Stamina)";
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
		for (int r = 0; r < inv.ROWS; r++)
		{
			for (int c = 0; c < inv.COLUMNS; c++)
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
		for (int c = 0; c < inv.COLUMNS; c++)
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

		for (int c = 0; c < shippingInventory.COLUMNS; c++)
		{
			GameObject slot = ShippingBinSlots[c];

			//the text and image of the button
			Image slotIcon = slot.transform.GetChild(0).GetComponent<Image>();
			Text slotLabel = slot.transform.GetChild(1).GetComponent<Text>();

			//the item in the corresponding slot in the inventory object
			ItemStack currentSlot = shippingInventory.GetSlot(0, c);

			//labels the slot
			if (currentSlot != null)
			{
				slotLabel.text = "" + currentSlot.Amount;
				slot.name = "Shipping Slot " + currentSlot.Item.name;
				slotIcon.sprite = currentSlot.Item.spr;
				slotIcon.color = new Color(slotIcon.color.r, slotIcon.color.g, slotIcon.color.b, 100);
			}
			else
			{
				slotLabel.text = "";
				slot.name = "Shipping Slot " + c;
				slot.name = "Shipping Slot " + c;
				slotIcon.sprite = null;
				slotIcon.color = new Color(slotIcon.color.r, slotIcon.color.g, slotIcon.color.b, 0);
			}
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

        Debug.Log("Before switch");
		switch(state)
		{
			case MenuState.INVENTORY:
                #region MenuState.INVENTORY
                Debug.Log("Inventory swtichcase");
                MakeSelectFollowMouse();
				//drop selected item
				if(Input.GetKeyDown(KeyCode.Q))
				{
					if (InventoryMenu.active && inv.selectedStack != null)
					{
						//drop selected Item
						for (int i = 0; i < inv.selectedStack.Amount; i++)
						{
							float theta = Random.Range(0, 2 * Mathf.PI);
							Instantiate(GetItemPrefab(inv.selectedStack.Item.name), player.transform.position + new Vector3(Mathf.Cos(theta) * 1.5f, Mathf.Sin(theta) * 1.5f, 0), Quaternion.identity);
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
					if (inv.selectedStack != null)
					{
						inv.AddItems(inv.selectedStack);
						inv.selectedStack = null;
					}
					InventoryMenu.SetActive(false);
					inv.isShown = false;
					selectedItem.SetActive(false);
					state = MenuState.NO_MENU;
					pi.CanInteract = true;
				}
				break;
			#endregion
			case MenuState.PAUSE:
                #region MenuState.PAUSE
                Debug.Log("Pause swtichcase");
                if (Input.GetKeyDown(KeyCode.Escape))
				{
					PauseMenu.SetActive(false);
					state = MenuState.NO_MENU;
					pi.CanInteract = true;
				}
				break;
			#endregion
			case MenuState.SETTINGS:
                #region MenuState.SETTINGS
                Debug.Log("Settings swtichcase");
                if (Input.GetKeyDown(KeyCode.Escape))
				{
					SettingsMenu.SetActive(false);
					PauseMenu.SetActive(true);
					state = MenuState.PAUSE;
					pi.CanInteract = true;
				}
				break;
				#endregion
			case MenuState.HELP:
                #region MenuState.HELP
                Debug.Log("Help swtichcase");
                if (Input.GetKeyDown(KeyCode.Escape))
				{
					HelpMenu.SetActive(false);
					PauseMenu.SetActive(true);
					state = MenuState.PAUSE;
					pi.CanInteract = true;
				}
				break;
				#endregion
			case MenuState.SHOP:
                #region MenuState.SHOP
                Debug.Log("Shop swtichcase");
                if (Input.GetKeyDown(KeyCode.Escape))
				{
					ShopMenu.SetActive(false);
					PauseMenu.SetActive(true);
					state = MenuState.PAUSE;
					pi.CanInteract = true;
				}
				break;
				#endregion
			case MenuState.SHIPPING_BIN:
                #region MenuState.SHIPPING_BIN
                Debug.Log("Bin swtichcase");
                MakeSelectFollowMouse();
				//drop selected item
				if (Input.GetKeyDown(KeyCode.Q))
				{
					if (InventoryMenu.active && curSelected != null)
					{
						//drop selected Item
						for (int i = 0; i < curSelected.Amount; i++)
						{
							Instantiate(GetItemPrefab(curSelected.Item.name), player.transform.position + new Vector3(0, -1f, 0), Quaternion.identity);
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

				//close shipping bin
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					if (inv.selectedStack != null)
					{
						inv.AddItems(inv.selectedStack);
						inv.selectedStack = null;
					}
					if(shippingInventory.selectedStack != null)
					{
						shippingInventory.AddItems(shippingInventory.selectedStack);
						shippingInventory.selectedStack = null;
					}
					ShippingMenu.SetActive(false);
					InventoryMenu.SetActive(false);
					selectedItem.SetActive(false);
					state = MenuState.NO_MENU;
					pi.CanInteract = true;

				}
				break;
				#endregion
			case MenuState.NO_MENU:
                #region MenuState.NO_MENU
                Debug.Log("Bin swtichcase");
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
					inv.isShown = true;
					InventoryMenu.SetActive(true);//!InventoryMenu.activeSelf);
					selectedItem.SetActive(true);
					state = MenuState.INVENTORY;
					pi.CanInteract = false;
				}

				//drop held item
				if(Input.GetKeyDown(KeyCode.Q))
				{
					if (inv.HeldItem != null)
					{
						//drop held item
						for (int i = 0; i < inv.HeldItem.Amount; i++)
						{
							DropItem(inv.HeldItem.Item);
							//float theta = Random.Range(0, 2 * Mathf.PI);
							//Instantiate(GetItemPrefab(inv.selectedStack.Item.name), player.transform.position + new Vector3(Mathf.Cos(theta) * 1.5f, Mathf.Sin(theta) * 1.5f, 0), Quaternion.identity);

							//Instantiate(GetItemPrefab(inv.HeldItem.Item.name), player.transform.position + new Vector3(Mathf.Cos(theta) * 1.5f, Mathf.Sin(theta) * 1.5f, 0), Quaternion.identity);
						}
						inv.DeleteHeldItemStack(); 
					}
				}
				
				if(Input.GetKeyDown(KeyCode.Escape))
				{
					pi.CanInteract = false;
					PauseMenu.SetActive(true);
					state = MenuState.PAUSE;
				}

				break;
            #endregion
            case MenuState.DIALOG:
                Debug.Log("Dialog default");
                break;
			default:
                Debug.Log("default switch");
                break;
		}
        Debug.Log("After switch");

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
		pi.CanInteract = true;
	}

	public void OpenSettings()
	{
		PauseMenu.SetActive(false);
		SettingsMenu.SetActive(true);
		state = MenuState.SETTINGS;
		pi.CanInteract = false;
	}

	//quits game - for button use;
	public void QuitGame()
	{
		Application.Quit();
	}

	//switches to main menu scene - for button use
	public void ReturnToMenu()
	{
		SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
	}

	public void OpenHelp()
	{
		PauseMenu.SetActive(false);
		HelpMenu.SetActive(true);
		state = MenuState.HELP;
		pi.CanInteract = false;

	}

	public void CloseHelp()
	{
		HelpMenu.SetActive(false);
		PauseMenu.SetActive(true);
		state = MenuState.PAUSE;
	}
	public void OpenShop()
	{
		PauseMenu.SetActive(false);
		ShopMenu.SetActive(true);
		state = MenuState.SHOP;
		pi.CanInteract = false;

	}

	public void OpenControls()
	{
		if(state == MenuState.HELP)
		{
			gameInfo.SetActive(false);
			controls.SetActive(true);
		}
	}

	public void OpenGameInfo()
	{
		if (state == MenuState.HELP)
		{
			gameInfo.SetActive(true);
			controls.SetActive(false);
		}
	}

	public void OpenShippingBin()
	{
		if(state == MenuState.NO_MENU)
		{
			InventoryMenu.SetActive(true);
			ShippingMenu.SetActive(true);
			selectedItem.SetActive(true);
			state = MenuState.SHIPPING_BIN;
			pi.CanInteract = false;

		}
	}

	public void OpenBed()
    {
		if(state == MenuState.NO_MENU)
        {
			state = MenuState.BED;
            bed.SetTextObjectsActive(true);
			pi.CanInteract = false;
        }
    }

	public void CloseBed()
    {
		if(state == MenuState.BED)
        {
			state = MenuState.NO_MENU;
            bed.SetTextObjectsActive(false);
            pi.CanInteract = true;
        }
    }

	public void OpenDialog()
	{
		state = MenuState.DIALOG;
		pi.CanInteract = false;
	}

	public void CloseDialog()
	{
		if(state == MenuState.DIALOG)
		{
			state = MenuState.NO_MENU;
			pi.CanInteract = true;
		}
	}

	public void MakeSelectFollowMouse()
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
	public void DropItem(Item i)
	{
		//float theta = Random.Range(0, 2 * Mathf.PI);
		Vector3 mouse = Input.mousePosition;
		mouse.z = 10;
		Vector2 worldMouse = cam.ScreenToWorldPoint(Input.mousePosition);

		Vector2 vec = worldMouse - (Vector2)player.transform.position;
		float theta = Mathf.Atan2(vec.y, vec.x);

		Instantiate(GetItemPrefab(i.name), player.transform.position + new Vector3(Mathf.Cos(theta) * 1.5f, Mathf.Sin(theta) * 1.5f, 0), Quaternion.identity);

	}

}
